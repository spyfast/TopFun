using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using TopFun.Captcha.CapLib;
using TopFun.Configs;

namespace TopFun.Captcha
{
    internal static class CaptchaSolver
    {
        private static RuCaptchaClient RCC = null;
        public static bool Enabled = true;
        public static string ApiKey;
        public static Queue<string> toSolve = new Queue<string>();
        private static Dictionary<string, string> answs 
			= new Dictionary<string, string>();

		public static void SetKey(string key)
		{
			ApiKey = key;
			RCC = new RuCaptchaClient(key);
		}
		public static string GetBalance()
		{
			string result;
			if (RCC != null)
				result = RCC.GetBalance().ToString();
			else
				result = "?";
			return result;
		}
		public static string Solve(string url, string id)
		{
			new WebClient().DownloadFile(url, id + ".png");
			var text = string.Empty;
			var captchaId = string.Empty;
			try
			{
				captchaId = RCC.UploadCaptchaFile(id + ".png");
			}
			finally
			{
				File.Delete(id + ".png");
			}

			while (string.IsNullOrEmpty(text))
			{
				Thread.Sleep(1000);
				try
				{
					text = RCC.GetCaptcha(captchaId);
				}
				catch (Exception ex)
				{
					if (!ex.Message.Contains("CAPCHA_NOT_READY"))
						Log.Push($"[Ошибка обработки капчи]: {ex.Message}");
				}
			}
			Log.Push("[Капча]: распознавание завершено");
			return text;
		}
		public static void RegisterManual(string key, string ans)
			=> answs.Add(key, ans);
		public static string SolveManual(string url, string id)
		{
			new WebClient().DownloadFile(url, id + ".png");
			toSolve.Enqueue(id);
			while (!answs.ContainsKey(id))
				Thread.Sleep(1000);

			var result = answs[id];
			answs.Remove(id);
			return result;
		}
	}
}
