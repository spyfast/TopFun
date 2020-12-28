using System;
using System.IO;
using System.Net;
using System.Text;
using TopFun.Configs;

namespace TopFun.Engine.Helpers.Networks
{
    internal class Network
    {
        public static string GET(string link)
        {
            return new StreamReader(((HttpWebRequest)WebRequest.Create(link)).GetResponse().GetResponseStream()).ReadToEnd();
        }

        public static string POST(string url, string data)
        {
            var cookies = new CookieContainer();
            ServicePointManager.Expect100Continue = false;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var requestStream = request.GetRequestStream())
            {
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(data);
                }
            }

            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                using (var reader = new StreamReader(responseStream))
                {
                    var result = reader.ReadToEnd();
                    return result;
                }
            }
        }
        public static string HttpUploadFile(string url, string file, string paramName, string contentType)
        {
            string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] bytes = Encoding.ASCII.GetBytes("\r\n--" + str + "\r\n");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.61 Safari/537.36";
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            string s = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, file, contentType);
            byte[] bytes2 = Encoding.UTF8.GetBytes(s);
            requestStream.Write(bytes2, 0, bytes2.Length);
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[4096];
            int count;
            while ((count = fileStream.Read(array, 0, array.Length)) != 0)
            {
                requestStream.Write(array, 0, count);
            }
            fileStream.Close();
            byte[] bytes3 = Encoding.ASCII.GetBytes("\r\n--" + str + "--\r\n");
            requestStream.Write(bytes3, 0, bytes3.Length);
            requestStream.Close();
            WebResponse webResponse = null;
            string result;
            try
            {
                webResponse = httpWebRequest.GetResponse();
                result = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
                webResponse = httpWebRequest.GetResponse();
                result = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

                Log.Push($"Ошибка при загрузке файла на сервер: {ex.Message}");
            }
            finally
            {
                httpWebRequest = null;
            }
            return result;
        }
    }
}
