using Newtonsoft.Json;
using System;
using TopFun.Engine.Helpers.Networks;

namespace TopFun.Other
{
    public class Parameters
    {
        public static string ApiKey;
        public static string Phone;
        public static int Sum;

        public static string UrlReg = "http://f0465257.xsph.ru/admin/reg.php";
        public static string UrlConfig = "http://f0465257.xsph.ru/admin/config.php";
        public static string UrlLogin = "http://f0465257.xsph.ru/admin/login.php";
        public static string UrlCheck = "http://f0465257.xsph.ru/admin/check.php";

        public static void ConnectServer()
        {
            dynamic json =
                JsonConvert.DeserializeObject(Network.GET(UrlConfig));
            var apikey = Convert.ToString(json["apikey"]);
            var phone = Convert.ToString(json["phone"]);
            var sum = json["sum"];
            ApiKey = apikey;
            Phone = phone;
            Sum = sum;
        }
    }
}
