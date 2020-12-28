using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using TopFun.Configs;
using TopFun.Engine;

namespace TopFun.Targets.Flooder
{
    public class FlooderSettings
    {
        private Random _rnd = new Random();
        public string RandomStickers()
        {
            var listStikers = File.ReadAllLines("Txts\\stickers.txt");
            if (listStikers.Length == 0)
            {
                Log.Push("[Флудер]: Отсутствуют стикеры");
                return null;
            }

            return listStikers[_rnd.Next(listStikers.Length)];
        }

        public void RandomDelay(string time, int valueMin, int valueMax)
        {
            switch (time)
            {
                case "Обычная":
                    Thread.Sleep(valueMin);
                    break;
                case "Рандомная":
                    Thread.Sleep(_rnd.Next(valueMin, valueMax));
                    break;
            }
        }
        public string RandomAudio()
        {
            var listAudio = new List<string>();
            var dir = new DirectoryInfo("Audio");
            var fileinfo = dir.GetFiles("*.mp3");

            if (fileinfo.Length == 0)
                return "Not files";
            else
            {
                foreach (var item in fileinfo)
                {
                    listAudio.Add(item.ToString());
                }
                return listAudio[_rnd.Next(listAudio.Count)];
            }
        }
        public string GetHistory(string target, string token)
        {
            var user = new Regex("im\\?sel=([0-9]+)").Match(target);
            var chat = new Regex("im\\?sel=c([0-9]+)").Match(target);

            if (user.Success)
            {
                return VkParameters.APIRequest("messages.getHistory", $"user_id={user.Groups[1].Value}", token, "");
            }

            if (chat.Success)
            {
                return VkParameters.APIRequest("messages.getHistory", $"peer_id={2000000000 + int.Parse(chat.Groups[1].Value)}", token, "");
            }

            return null;
        }

        public List<string> Items = new List<string>()
        {
            "актив",
            "кинь актив",
            "на боте",
            "+ если не бот"
        };
    }
}
