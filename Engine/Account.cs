using AuthorizeVK;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using TopFun.Configs;
using TopFun.Engine.Helpers;
using TopFun.Engine.Helpers.Networks;
using TopFun.Targets.Autoans;
using TopFun.Targets.Flooder;

namespace TopFun.Engine
{
    public class Account
    {
        private readonly Random _rnd = new Random();
        public string Login { get; set; }
        public string Password { get; set; }
        public List<AutoansTarget> AutoansTarget;
        public List<FlooderTarget> FlooderTarget;

        [NonSerialized] public bool Status;
        public string Token { get; set; }

        public string UserId { get; set; }
        public string AccInfo { get; set; }

        // Watch Ignored

        [NonSerialized]
        public
            Stopwatch stopWatch = new Stopwatch();
        public string Userid { get; set; }
        public bool EnabledStopWatch { get; set; }
        public string IdBound { get; set; }
        public string numericWatch { get; set; }
        public string IdUser { get; set; }

        // Flooder
        public bool setActive { get; set; }
        public bool Enabled { get; set; }
        public string TimeDelay { get; set; }
        public int DelayMin { get; set; }
        public string PhrasesFile { get; set; }

        public int DelayMax { get; set; }

        // Start
        [NonSerialized] public static bool Works;

        // Autoans 
        public bool enabledAutoans { get; set; }
        public bool SetActibityAuto { get; set; }
        public int DelayAuto { get; set; }
        public bool ReplyToMessage { get; set; }
        public Account() { }
        public Account(string login, string password)
        {
            _rnd = new Random();
            Login = login;
            Password = password;
            AutoansTarget = new List<AutoansTarget>();
            FlooderTarget = new List<FlooderTarget>();
        }

        public void SetTargetAutoans(DataGridViewRowCollection rows)
        {
            AutoansTarget.Clear();

            foreach (DataGridViewRow dataGrid in rows)
            {
                if (dataGrid.Cells[1].Value != null)
                {
                    AutoansTarget.Add(new AutoansTarget
                    {
                        Name = (dataGrid.Cells[0].Value ?? "").ToString(),
                        Target = dataGrid.Cells[1].Value.ToString(),
                        Ids = dataGrid.Cells[2].Value.ToString(),
                        Content = dataGrid.Cells[3].Value.ToString()
                    });
                }
            }
        }

        public void SetTargetFlooder(DataGridViewRowCollection rows)
        {
            FlooderTarget.Clear();

            foreach (DataGridViewRow dataGrid in rows)
            {
                if (dataGrid.Cells[1].Value != null)
                {
                    FlooderTarget.Add(new FlooderTarget()
                    {
                        Name = (dataGrid.Cells[0].Value ?? "").ToString(),
                        Target = (dataGrid.Cells[1].Value ?? "").ToString(),
                        NamePlace = (dataGrid.Cells[2].Value ?? "").ToString(),
                        Content = (dataGrid.Cells[3].Value ?? "").ToString()
                    });
                }
            }
        }

        public int CheckToken()
        {
            Status = false;
            if (Token == null)
                return -1;
            int result;
            try
            {
                Status = true;
                result = 0;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("could not get application"))
                    result = 1;
                else
                {
                    Log.Push("[Ошибка API]: " + ex.Message);
                    result = 2;
                }
            }

            return result;
        }

        public int GetToken()
        {
            if (CheckToken() == 0)
                return 0;


            try
            {
                var auth = new Auth(Login, Password);
                Token = auth.Authorize();
                UserId = auth.UserId;
                dynamic json =
                    JsonConvert.DeserializeObject(VkParameters.APIRequest("users.get", $"user_ids={UserId}", Token, null));
                AccInfo = $"{json["response"][0]["first_name"]} {json["response"][0]["last_name"]}";
                Status = true;
                return 0;
            }
            catch (Exception ex)
            {
                Log.Push($"Неизвестная ошибка авторизации: {ex.Message}");
                return -1;
            }
        }

        public void SaveToDisk()
        {
            var xmlSerializer = new XmlSerializer(typeof(Account));
            var textWriter = new StreamWriter($"Accounts\\{Login}.xml");
            xmlSerializer.Serialize(textWriter, this);
            textWriter.Close();
        }

        public void TheAnswerToKeyword(int index)
        {
            var target = FlooderTarget[index].Target;
            var fs = new FlooderSettings();
            var pattern = new Regex("im\\?sel=c([0-9]+)").Match(target);

            dynamic json = JsonConvert.DeserializeObject(VkParameters.APIRequest(
                     "messages.getHistory", $"peer_id={2000000000 + int.Parse(pattern.Groups[1].Value)}&count=1",
                     Token, null));

            if (json["response"]["count"] != 0 && !fs.RandomAudio().Contains("Not files"))
            {
                var text = Convert.ToString(json["response"]["items"][0]["text"]);

                if (fs.Items.Contains(text))
                {
                    Log.Push($"[Флудер {Login}]: Начинаю загрузку аудиозаписи на сервер");
                    dynamic url =
                    JsonConvert.DeserializeObject(VkParameters.APIRequest("docs.getUploadServer", "type=audio_message",
                        Token, null));

                    var file = JsonConvert.DeserializeObject(Network.HttpUploadFile(
                        Convert.ToString(url["response"]["upload_url"]), "Audio\\" + fs.RandomAudio(), "file",
                        "audio/MP3"));
                    dynamic saveDocs =
                        JsonConvert.DeserializeObject(VkParameters.APIRequest("docs.save", $"file={file["file"]}", Token, null));

                    var id = Convert.ToString(saveDocs["response"]["audio_message"]["id"]);
                    var owner_id = Convert.ToString(saveDocs["response"]["audio_message"]["owner_id"]);
                    Log.Push($"[Флудер {Login}]: Загрузка окончена. Подготовка к отправке");

                    if (target.StartsWith("im?sel="))
                    {
                        var users = StrWrk.qSubstr(target, "im?sel=");
                        VkParameters.APIRequest("messages.send", $"user_id={users}&attachment=doc{owner_id}_{id}", Token, null);
                    }

                    if (target.StartsWith("im?sel=c"))
                    {
                        var chat = StrWrk.qSubstr(target, "im?sel=c");
                        VkParameters.APIRequest("messages.send", $"chat_id={chat}&attachment=doc{owner_id}_{id}", Token, null);
                    }
                }
            }
        }
        private void StartChecking(string target)
        {
            if (EnabledStopWatch)
            {
                dynamic history = JsonConvert.DeserializeObject(VkParameters.APIRequest("messages.getHistory",
                        $"count=1&peer_id={2000000000 + int.Parse(target)}", Token, null));
                var fromId = Convert.ToString(history["response"]["items"][0]["from_id"]);
                if (IdBound == fromId)
                    stopWatch.Restart();
                foreach (var idsBound in IdBound.Split(','))
                {
                    dynamic getUsers = JsonConvert.DeserializeObject(VkParameters.APIRequest("users.get", $"user_ids={idsBound}", Token, null));
                    var firstName = Convert.ToString(getUsers["response"][0]["first_name"]);
                    var lastName = Convert.ToString(getUsers["response"][0]["last_name"]);

                    if (EnabledStopWatch && (int)stopWatch.Elapsed.TotalMilliseconds >= int.Parse(numericWatch)
                        && fromId != idsBound)
                    {
                        var watchMessage = $"Обнаружен игнор!\nПользователь: *id{idsBound} ({firstName} {lastName})\nВремя: {numericWatch}\nЧат: {target}";
                        VkParameters.APIRequest("messages.send",
                            $"user_id={Userid}&message={watchMessage}", Token, null);
                        Log.Push($"[{Login}]: Время игнора вышло... Обновляю счетчик");
                        stopWatch.Restart();
                    }
                }
            }
        }
        public bool IsMarkSms { get; set; }
        public void FlooderSendMessage(string msg, int index)
        {
            var text = FlooderTarget[index].Target;
            var name = FlooderTarget[index].Name;
            var contains = FlooderTarget[index].Content;
            var namePlace = FlooderTarget[index].NamePlace;
            var fs = new FlooderSettings();
            if (name != null)
            {
                if (namePlace == "Начало")
                    msg = $"{name} {msg}";
                else if (namePlace == "Конец")
                    msg = $"{msg} {name}";
            }
            msg = WebUtility.UrlEncode(msg);
            //try
            {
                if (text.StartsWith("im?sel=c"))
                {

                    var chat = StrWrk.qSubstr(text, "im?sel=c");
                    if (IsMarkSms)
                        VkParameters.APIRequest("messages.markAsAnsweredConversation",
                            $"peer_id={long.Parse(chat) + 2000000000}", Token, null);
                    if (setActive)
                        VkParameters.SetActivity(text, Token);
                    StartChecking(chat);
                    switch (contains)
                    {
                        case "Текст":
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}", Token, null);
                            break;
                        case "Текст+стикер":
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}", Token, null);
                            Thread.Sleep(_rnd.Next(4000, 7000));
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&sticker_id={fs.RandomStickers()}",
                                Token, null);
                            break;
                        case "Текст+фото":
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                            break;
                        case "Текст+фото+стикер":
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                            Thread.Sleep(_rnd.Next(4000, 7000));
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&sticker_id={fs.RandomStickers()}",
                                                            Token, null);
                            break;
                        case "Рандом":
                            var r = _rnd.Next(0, 7);
                            if (r == 1 || r == 3 || r == 0)
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}", Token, null);
                            }
                            else
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={msg}", Token, null);
                                Thread.Sleep(_rnd.Next(4000, 7000));
                                VkParameters.APIRequest("messages.send",
                                    $"chat_id={chat}&sticker_id={fs.RandomStickers()}", Token, null);
                            }
                            break;
                    }

                }
                else if (text.StartsWith("im?sel="))
                {
                    var user = StrWrk.qSubstr(text, "im?sel=");
                    if (IsMarkSms)
                        VkParameters.APIRequest("messages.markAsAnsweredConversation",
                            $"peer_id={long.Parse(user) + 2000000000}", Token, null);
                    if (setActive)
                        VkParameters.SetActivity(text, Token);

                    switch (contains)
                    {
                        case "Текст":
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}", Token, null);
                            break;
                        case "Текст+стикер":
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}", Token, null);
                            Thread.Sleep(_rnd.Next(4000, 7000));
                            VkParameters.APIRequest("messages.send", $"user_id={user}&sticker_id={fs.RandomStickers()}",
                                Token, null);
                            break;
                        case "Текст+фото":
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                            break;
                        case "Текст+фото+стикер":
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                            Thread.Sleep(_rnd.Next(4000, 7000));
                            VkParameters.APIRequest("messages.send", $"user_id={user}&sticker_id={fs.RandomStickers()}",
                                                            Token, null);
                            break;
                        case "Рандом":
                            var r = _rnd.Next(0, 7);
                            if (r == 1 || r == 3 || r == 0)
                            {
                                VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}", Token, null);
                            }
                            else
                            {
                                VkParameters.APIRequest("messages.send", $"user_id={user}&message={msg}", Token, null);
                                Thread.Sleep(_rnd.Next(4000, 7000));
                                VkParameters.APIRequest("messages.send",
                                    $"user_id={user}&sticker_id={fs.RandomStickers()}", Token, null);
                            }
                            break;
                    }
                }
                else
                {
                    Log.Push($"[{Login}]: {text} — неверный формат ссылки");
                }
            }
            //catch (Exception ex)
            {
                //Log.Push("[Ошибка API]: " + ex.Message);
            }
        }
        private void WorksFlooder()
        {
            var fs = new FlooderSettings();
            var index = -1;
            while (Works && Enabled)
            {
                //try
                {
                    if (FlooderTarget.Count == 0)
                    {
                        Log.Push("Отсутствуют цели флудера...");
                        return;
                    }
                    index = (index + 1) % FlooderTarget.Count;

                    //TheAnswerToKeyword(index);
                    FlooderSendMessage(MainForm.phrases[_rnd.Next(0, MainForm.phrases.Length)], index);

                    fs.RandomDelay(TimeDelay, DelayMin, DelayMax);
                }
                //catch (Exception ex)
                {
                    //Log.Push($"[Поток {Login}]: " + ex.Message);
                }

            }
        }
        public bool IsMarkSmsAnswer { get; set; }
        public bool IsRandomAnswer { get; set; }

        public void AutoansSendMessage(string message, int index)
        {
            var target = AutoansTarget[index].Target;
            var name = AutoansTarget[index].Name;
            var ids = AutoansTarget[index].Ids;
            var content = AutoansTarget[index].Content;
            var fs = new FlooderSettings();

            if (!string.IsNullOrEmpty(name))
                message = $"{name} {message}";

            message = WebUtility.UrlEncode(message);

            if (target.StartsWith("im?sel=c"))
            {
                var chat = StrWrk.qSubstr(target, "im?sel=c");

                if (IsMarkSms)
                    VkParameters.APIRequest("messages.markAsAnsweredConversation",
                        $"peer_id={2000000000 + long.Parse(chat)}&answered=1", Token, null);

                var indexing = new List<int>();


                dynamic history = JsonConvert.DeserializeObject(
                    VkParameters.APIRequest("messages.getHistory", $"peer_id={2000000000 + int.Parse(chat)}&count=1",
                        Token, null));

                for (int i = 0; i < 10; i++)
                {
                    if (history["response"]["items"][i]["from_id"] == ids)
                        indexing.Add(i);
                }
                var s = indexing[new Random().Next(0, indexing.Count)];

                var fromId = history["response"]["items"][0]["from_id"];
                string idMessage = string.Empty;
                if (IsRandomAnswer)
                    idMessage = history["response"]["items"][s]["id"];
                else
                    idMessage = history["response"]["items"][0]["id"];
                foreach (var id in ids.Split(','))
                {
                    if (fromId == id)
                    {
                        if (SetActibityAuto)
                        {
                            VkParameters.APIRequest("messages.setActivity", $"chat_id={chat}&type=typing", Token, null);
                            Thread.Sleep(2000);
                        }

                        if (content == "Текст")
                        {
                            if (ReplyToMessage)
                                VkParameters.APIRequest("messages.send",
                                    $"chat_id={chat}&message={message}&reply_to={idMessage}", Token, null);
                            else
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}", Token, null);
                        }
                        else if (content == "Текст+фото")
                        {
                            if (ReplyToMessage)
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}&reply_to={id}",
                                   Token, null);
                            }
                            else
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}", Token, null);

                            }
                            Thread.Sleep(4000);

                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);

                        }
                        else if (content == "Текст+стикер")
                        {
                            if (ReplyToMessage)
                            {
                                VkParameters.APIRequest("messages.send",
                                    $"chat_id={chat}&message={message}&reply_to={idMessage}", Token, null);
                                Thread.Sleep(4000);
                                VkParameters.APIRequest("messages.send",
                                    $"chat_id={chat}&sticker_id={fs.RandomStickers()}", Token, null);
                            }
                            else
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}", Token, null);
                                Thread.Sleep(4000);
                                VkParameters.APIRequest("messages.send",
                                    $"chat_id={chat}&sticker_id={fs.RandomStickers()}", Token, null);
                            }
                        }
                        else if (content == "Текст+фото+стикер")
                        {
                            if (ReplyToMessage)
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}&reply_to={id}",
                                    Token, null);
                            }
                            else
                            {
                                VkParameters.APIRequest("messages.send", $"chat_id={chat}&message={message}", Token, null);

                            }
                            Thread.Sleep(4000);
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                            Thread.Sleep(4000);
                            VkParameters.APIRequest("messages.send", $"chat_id={chat}&sticker_id={fs.RandomStickers()}",
                                Token, null);
                        }
                    }
                }
            }
            else if (target.StartsWith("im?sel="))
            {
                if (ids.Contains(","))
                {
                    Log.Push("Невозможно указать несколько ID для личных сообщений.");
                    return;
                }

                var user = StrWrk.qSubstr(target, "im?sel=");
                if (IsMarkSms)
                    VkParameters.APIRequest("messages.markAsAnsweredConversation",
                        $"peer_id={long.Parse(user) + 2000000000}", Token, null);
                var indexing = new List<int>();
                dynamic json = JsonConvert.DeserializeObject(
                    VkParameters.APIRequest("messages.getHistory", $"user_id={user}&count=10", Token, null));
                for (int i = 0; i < 10; i++)
                {
                    if (json["response"]["items"][i]["from_id"] == ids)
                        indexing.Add(i);
                }
                var s = indexing[new Random().Next(0, indexing.Count)];
                var fromId = json["response"]["items"][0]["from_id"];

                string id = string.Empty;
                if (IsRandomAnswer)
                    id = json["response"]["items"][s]["id"];
                else
                    id = json["response"]["items"][0]["id"];

                if (fromId == ids)
                {
                    if (SetActibityAuto)
                    {
                        VkParameters.APIRequest("messages.setActivity", $"user_id={user}&type=typing", Token, null);
                        Thread.Sleep(2000);
                    }

                    if (content == "Текст")
                    {
                        if (ReplyToMessage)
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}&reply_to={id}",
                                Token, null);
                        else
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}", Token, null);
                    }
                    else if (content == "Текст+фото")
                    {
                        if (ReplyToMessage)
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}&reply_to={id}",
                               Token, null);
                        }
                        else
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}", Token, null);

                        }
                        Thread.Sleep(4000);

                        VkParameters.APIRequest("messages.send", $"user_id={user}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);

                    }
                    else if (content == "Текст+стикер")
                    {
                        if (ReplyToMessage)
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}&reply_to={id}",
                                Token, null);
                        }
                        else
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}", Token, null);

                        }
                        Thread.Sleep(4000);
                        VkParameters.APIRequest("messages.send", $"user_id={user}&sticker_id={fs.RandomStickers()}",
                            Token, null);
                    }
                    else if (content == "Текст+фото+стикер")
                    {
                        if (ReplyToMessage)
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}&reply_to={id}",
                                Token, null);
                        }
                        else
                        {
                            VkParameters.APIRequest("messages.send", $"user_id={user}&message={message}", Token, null);
                            
                        }
                        Thread.Sleep(4000);
                        VkParameters.APIRequest("messages.send", $"user_id={user}&attachment={MainForm.Images[new Random().Next(0, MainForm.Images.Length)]}", Token, null);
                        Thread.Sleep(4000);
                        VkParameters.APIRequest("messages.send", $"user_id={user}&sticker_id={fs.RandomStickers()}",
                            Token, null);
                    }
                }
            }
        }

        public void WorkAutoans()
        {
            var index = -1;
            if (AutoansTarget.Count == 0)
            {
                Log.Push("Отсутствуют цели автоответчика...");
                return;
            }

            while (Works && enabledAutoans)
            {
                try
                {
                    index = (index + 1) % AutoansTarget.Count;
                    AutoansSendMessage(MainForm.phrases[_rnd.Next(MainForm.phrases.Length)], index);

                    Thread.Sleep(DelayAuto);
                }
                catch (Exception ex)
                {
                    Log.Push($"[Автоответчик]: {ex.Message}");
                }
            }
        }
        public void AsyncWorker()
        {
            if (EnabledStopWatch)
                stopWatch.Start();

            if (Enabled)
                new Thread(() => WorksFlooder()) { IsBackground = true }.Start();

            if (enabledAutoans)
                new Thread(() => WorkAutoans()) { IsBackground = true }.Start();
        }
    }
}
