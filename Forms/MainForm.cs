using MaterialSkin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using TopFun.Captcha;
using TopFun.Configs;
using TopFun.Engine;
using TopFun.Engine.Helpers;
using TopFun.Engine.Helpers.Networks;
using TopFun.Other;

namespace TopFun
{
    public partial class MainForm : Form
    {
        #region MouseDownForm
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wmsg, int wparam, int lparam);
        #endregion
        private string currManId;
        public static string[] phrases;
        private readonly List<Account> accounts;
        public static string[] Images { get; set; }
        public MainForm()
        {
            InitializeComponent();
            //new Thread(() => 
            //{
            //    dynamic json =
            //        JsonConvert.DeserializeObject(
            //            Network.POST(Parameters.UrlLogin, $"hwid={Managment.GetHWID()}&username={Managment.GetUserName()}&action=check"));

            //    Invoke(new Action(() => 
            //        { zeroitLollipopLabel12.Text = 
            //            $"Ваш Id: {Convert.ToString(json["user"]["id"])}\nВерсия: {Application.ProductVersion}\nUserName: {Managment.GetUserName()}\nHWID: {Managment.GetHWID()}"; }));
            //}) { IsBackground = true }.Start();
            var Skin = MaterialSkinManager.Instance;
            Skin.AddFormToManage(new MaterialSkin.Controls.MaterialForm());
            Skin.ColorScheme = new ColorScheme(
                Primary.Grey900, 
                Primary.Grey900, 
                Primary.Lime900, 
                Accent.Lime700, 
                TextShade.WHITE);

            accounts = new List<Account>();
            if (!Directory.Exists("Accounts"))
                Directory.CreateDirectory("Accounts");
            if (!Directory.Exists("Audio"))
                Directory.CreateDirectory("Audio");
            if (!Directory.Exists("Txts"))
                Directory.CreateDirectory("Txts");
            if (!Directory.Exists("Txts\\Phrases"))
                Directory.CreateDirectory("Txts\\Phrases");
            if (!File.Exists("Txts\\captcha.txt"))
                File.Create("Txts\\captcha.txt").Close();
            if (!File.Exists("Txts\\accounts.txt"))
                File.Create("Txts\\accounts.txt").Close();
            if (!File.Exists("Txts\\stickers.txt"))
                File.Create("Txts\\stickers.txt").Close();
            if (!File.Exists("Txts\\images.txt"))
                File.Create("Txts\\images.txt").Close();
            Images = File.ReadAllLines("Txts\\images.txt");
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            ApiKeyTextBox.Text = File.ReadAllText("Txts\\captcha.txt");
            LoadTxtsFiles();

            var accs = File.ReadAllLines("Txts\\accounts.txt");
            for (var i = 0; i < accs.Length; i++)
            {
                var data = accs[i].Split(':');
                if (data.Length != 2)
                    Log.Push("Неверный формат загрузки аккаунтов");
                else
                {
                    Account account;
                    if (!File.Exists($"Accounts\\{data[0]}.xml"))
                        account = new Account(data[0], data[1]);
                    else
                    {
                        var xmlSerializer = new XmlSerializer(typeof(Account));
                        var fileStream = new FileStream($"Accounts\\{data[0]}.xml", FileMode.Open);
                        var xmlReader = XmlReader.Create(fileStream);
                        account = (Account)xmlSerializer.Deserialize(xmlReader);
                        fileStream.Close();
                        //TabControl.Enabled = true;
                    }

                    accounts.Add(account);
                    comboBox_accountsList.Items.Add(account.AccInfo == null
                        ? $"{account.Login} — не авторизован"
                        : $"{account.Login} ({account.AccInfo})");
                }
            }

            if (comboBox_accountsList.Items.Count != 0)
            {
                comboBox_accountsList.SelectedIndex = 0;
                comboBox_accountsList_SelectedIndexChanged(null, null);
            }
            else
                comboBox_accountsList.Text = "Не было загружено ни одного аккаунта";

            CaptAns.Visible = ManualCaptBox.Visible = CaptPic.Visible = !CaptchaSolver.Enabled;
            label_ApiCaptcha.Visible = ApiKeyTextBox.Visible = button_GetBalanceCaptcha.Visible = saveCaptcha.Visible = CaptchaSolver.Enabled;
        }
        private void LoadTxtsFiles()
        {
            var txt = new List<string>(Directory.GetFiles("Txts\\Phrases"));
            txt = txt.ConvertAll(Path.GetFileName);
            comboBox_SelectPhrases.Items.Clear();
            comboBox_SelectPhrases.Items.AddRange(txt.ToArray());
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
            => Application.Exit();

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            while (Log.Logs.Count > 0)
                textBoxLogger.Text =
                    $"{Log.Logs.Dequeue()}\r\n{textBoxLogger.Text}";

        }

        private void pictureBox_Title_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void ManualCaptTimer_Tick(object sender, EventArgs e)
        {
            if (CaptPic.Image == null && CaptchaSolver.toSolve.Count != 0)
            {
                currManId = CaptchaSolver.toSolve.Dequeue();
                FileStream fileStream = new FileStream(currManId + ".png", FileMode.Open);
                CaptPic.Image = Image.FromStream(fileStream);
                fileStream.Close();
                File.Delete(currManId + ".png");
            }
        }

        private void RucaptEnabled_Click(object sender, EventArgs e)
        {
            CaptchaSolver.Enabled = RucaptEnabled.Checked;
            CaptAns.Visible = ManualCaptBox.Visible = CaptPic.Visible = !CaptchaSolver.Enabled;
            label_ApiCaptcha.Visible = ApiKeyTextBox.Visible = button_GetBalanceCaptcha.Visible = saveCaptcha.Visible = CaptchaSolver.Enabled;
        }

        private void saveCaptcha_Click(object sender, EventArgs e)
        {
            File.WriteAllText("Txts\\captcha.txt", ApiKeyTextBox.Text);
            if (ApiKeyTextBox.Text.Trim() != "")
                CaptchaSolver.SetKey(ApiKeyTextBox.Text.Trim());
        }

        private void button_GetBalanceCaptcha_Click(object sender, EventArgs e)
        {
            try
            {
                Log.Push($"Баланс капчи: {CaptchaSolver.GetBalance()}");
            }
            catch (Exception ex)
            {
                Log.Push($"[Ошибка запроса баланса]: {ex.Message}");
            }
        }

        private void CaptAns_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ManualCaptBox.Text) || currManId == null)
            {
                CaptchaSolver.RegisterManual(currManId, ManualCaptBox.Text);
                CaptPic.Image.Dispose();
                CaptPic.Image = null;
                currManId = null;
                ManualCaptBox.Text = "";
            }
        }

        private void comboBox_accountsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var account = accounts[comboBox_accountsList.SelectedIndex];
                numericDelayMinFlooder.Value = account.DelayMin == 0 ? 333 : account.DelayMin;
                targetGridFlooder.Rows.Clear();
                targetGridAutoans.Rows.Clear();
                for (var i = 0; i < account.AutoansTarget.Count; i++)
                    targetGridAutoans.Rows.Add(
                        account.AutoansTarget[i].Name,
                        account.AutoansTarget[i].Target,
                        account.AutoansTarget[i].Ids,
                        account.AutoansTarget[i].Content);

                for (var i = 0; i < account.FlooderTarget.Count; i++)
                    targetGridFlooder.Rows.Add(
                        account.FlooderTarget[i].Name,
                        account.FlooderTarget[i].Target,
                        account.FlooderTarget[i].NamePlace,
                        account.FlooderTarget[i].Content);


                checkBox_setActivityFlooder.Checked = account.setActive;
                checkBox_EnabledFlooder.Checked = account.Enabled;
                numericDelayMinAutoans.Value = account.DelayAuto == 0 ? 333 : account.DelayAuto;
                checkBox_setActivityAutoans.Checked = account.SetActibityAuto;

                checkBox_EnabledAutoans.Checked = account.enabledAutoans;
                checkBox_ReplyToMessage.Checked = account.ReplyToMessage;

                comboBox_TypeDelay.Text = account.TimeDelay;

                numericDelayMaxFlooder.Value = account.DelayMax == 0 ? 333 : account.DelayMax;
                comboBox_SelectPhrases.Text = account.PhrasesFile;

                textbox_IdBound.Text = account.IdBound;
                textBox_FromId.Text = account.IdUser;
                numericWatch.Text = account.numericWatch;
                EnabledStopWatch.Checked = account.EnabledStopWatch;

                isMarkSms.Checked = account.IsMarkSms;
                isMarkSmsAnswer.Checked = account.IsMarkSmsAnswer;

                isRandomAnswer.Checked = account.IsRandomAnswer;

                phrases = !string.IsNullOrEmpty(comboBox_SelectPhrases.Text)
                    ? File.ReadAllLines(
                        Path.Combine("Txts\\Phrases", comboBox_SelectPhrases.Text),
                        Encoding.UTF8)
                    : null;
            }
            catch
            {
            }
        }

        private void button_GetToken_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(delegate ()
                {
                    foreach (var account in accounts)
                    {
                        var token = account.GetToken();

                        if (token != -1)
                        {
                            if (token == 0)
                            {
                                comboBox_accountsList.Invoke(new Action(() =>
                                {
                                    comboBox_accountsList.Items.Clear();
                                    comboBox_accountsList.Items.Add($"{account.Login} ({account.AccInfo})");
                                    comboBox_accountsList.SelectedIndex = 0;
                                }));
                            //tabControl.Invoke(new Action(() => { tabControl.Enabled = true; }));
                            Log.Push("Авторизация прошла успешно");
                            }
                            else
                                Log.Push("Ошибка авторизации");
                        }

                        foreach (var acc in accounts)
                            acc.SaveToDisk();
                    }
                }).Start();
            }
            catch { }
        }
        private void MainRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (phrases.Length == 0)
                    Log.Push("Отсутствуют фразы");
                else
                {
                    if (Account.Works)
                    {
                        Account.Works = false;
                        Log.Push("Бот остановлен");
                        MainRun.Text = "Старт";
                    }
                    else
                    {
                        MainRun.Text = "Стоп";
                        Log.Push("Бот запущен");
                        Account.Works = true;
                        foreach (var account in accounts)
                        {
                            if (account.Enabled || account.enabledAutoans)
                            {
                                if (account.Status)
                                    account.AsyncWorker();
                                else
                                    Log.Push($"[{account.Login}]: запуск невозможен, неверный токен");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Push("[Ошибка запуска]: " + ex.Message);
            }
        }

        private void button_SavrFuncFlooder_Click(object sender, EventArgs e)
        {
            if (comboBox_accountsList.SelectedIndex == -1)
                return;

            var account = accounts[comboBox_accountsList.SelectedIndex];

            account.DelayMin = (int)numericDelayMinFlooder.Value;
            account.SetTargetFlooder(targetGridFlooder.Rows);
            account.setActive = checkBox_setActivityFlooder.Checked;
            account.Enabled = checkBox_EnabledFlooder.Checked;
            account.DelayMax = (int)numericDelayMaxFlooder.Value;
            account.TimeDelay = comboBox_TypeDelay.Text;
            account.PhrasesFile = comboBox_SelectPhrases.Text;
            account.IsMarkSms = isMarkSms.Checked;
            phrases = File.ReadAllLines($"Txts\\Phrases\\{comboBox_SelectPhrases.Text}",
                Encoding.UTF8);
            account.SaveToDisk();
        }

        private void button_SaveFuncAutoans_Click(object sender, EventArgs e)
        {
            if (comboBox_accountsList.SelectedIndex == -1)
                return;

            var account = accounts[comboBox_accountsList.SelectedIndex];
            account.SetTargetAutoans(targetGridAutoans.Rows);
            account.DelayAuto = (int)numericDelayMinAutoans.Value;
            account.enabledAutoans = checkBox_EnabledAutoans.Checked;
            account.SetActibityAuto = checkBox_setActivityAutoans.Checked;
            account.ReplyToMessage = checkBox_ReplyToMessage.Checked;
            account.IsMarkSmsAnswer = isMarkSmsAnswer.Checked;
            account.IsRandomAnswer = isRandomAnswer.Checked;
            account.SaveToDisk();
        }

        private void comboBox_accountsList_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_SaveWatchIgnored_Click(object sender, EventArgs e)
        {
            if (comboBox_accountsList.SelectedIndex != -1)
            {
                var account = accounts[comboBox_accountsList.SelectedIndex];
                account.IdBound = textbox_IdBound.Text;
                account.IdUser = textBox_FromId.Text;
                account.numericWatch = numericWatch.Text;
                account.EnabledStopWatch = EnabledStopWatch.Checked;
                account.SaveToDisk();
            }
        }

        private void comboBox_TypeDelay_TextChanged(object sender, EventArgs e)
        {
            if (comboBox_TypeDelay.Text == "Обычная")
                numericDelayMaxFlooder.Enabled = false;
            else
                numericDelayMaxFlooder.Enabled = true;
        }

        private void timerForLogs_Tick(object sender, EventArgs e)
        {
        }
    }
}
