using Newtonsoft.Json;
using System;
using System.Threading;
using TopFun.Captcha;
using TopFun.Captcha.CapLib;
using TopFun.Configs;
using TopFun.Engine.Helpers;
using TopFun.Engine.Helpers.Networks;

namespace TopFun.Engine
{
    internal static class VkParameters
    {
		public static string APIRequest(string method, string param, string token, string cpt)
		{
            var response = string.Empty;

            while (true)
            {
                response = Network.GET(
                    $"https://api.vk.com/method/{method}?{param}&access_token={token}{cpt}&v=5.107&random_id={(int)DateTime.Now.Ticks}");
                if (!response.Contains("\"error_code\":6"))
                    break;
                Thread.Sleep(1000);
            }

            if (response.Contains("Captcha needed"))
            {
                var between = StrWrk.GetBetween(response, "\"captcha_sid\":\"", "\"");
                var url = StrWrk.GetBetween(response, "\"captcha_img\":\"", "\"").Replace("\\", "");
                var str = string.Empty;
                if (CaptchaSolver.Enabled)
                {
                    Log.Push("[Капча]: Обработка капчи...");
                    try
                    {
                        str = CaptchaSolver.Solve(url, between);
                    }
                    catch (RuCaptchaException ex)
                    {
                        Log.Push("[Ошибка обработки капчи]: " + ex.Message);
                    }
                    return APIRequest(method, param, token, "&captcha_sid=" + between + "&captcha_key=" + str);
                }
                Log.Push("[Капча]: изображение поставлено в очередь на ручной ввод");
                str = CaptchaSolver.SolveManual(url, between);
                return APIRequest(method, param, token, $"&captcha_sid={between}&captcha_key={str}");
            }
            return response;
        }

		public static void SetActivity(string text, string token)
		{
			var random = new Random();
			if (text.StartsWith("im?sel=c"))
			{
				var chat = StrWrk.qSubstr(text, "im?sel=c", false);
				APIRequest("messages.setActivity", $"chat_id={chat}&type=typing", token, "");
				Thread.Sleep(random.Next(2000, 7000));
			}
			else if (text.StartsWith("im?sel="))
			{
				var user = StrWrk.qSubstr(text, "im?sel=", false);
				APIRequest("messages.setActivity", $"user_id={user}&type=typing", token, "");
				Thread.Sleep(random.Next(2000, 7000));
			}
		}
	}
}
