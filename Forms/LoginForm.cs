using Newtonsoft.Json;
using QiwiLogic;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using TopFun.Engine.Helpers;
using TopFun.Engine.Helpers.Networks;
using TopFun.Other;

namespace TopFun.Forms
{
    public partial class LoginForm : Form
    {
        #region MouseDownForm
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wmsg, int wparam, int lparam);
        #endregion
        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginForm_Shown(object sender, EventArgs e)
        {
            username.Text = Managment.GetUserName();
            hwidLabel.Text = Managment.GetHWID();
            if (!File.Exists("AuthorizeVK.dll") || !File.Exists("Bunifu.Core.dll") || !File.Exists("Bunifu.UI.WinForms.BunifuFormDock.dll") ||
                !File.Exists("Bunifu_UI_v1.5.3.dll") || !File.Exists("FlatUI.dll") || !File.Exists("MaterialSkin.dll") || !File.Exists("Newtonsoft.Json.dll") || 
                !File.Exists("QiwiWalletInfoAPI.dll") || !File.Exists("Zeroit.Framework.LollipopControls.dll"))
            {
                MessageBox.Show("Отсутствуют необходимые DLL для работы приложения: AuthorizeVK.dll, Bunifu.Core.dll," +
                    " Bunifu.UI.WinForms.BunifuFormDock.dll, Bunifu_UI_v1.5.3.dll, FlatUI.dll, MaterialSkin.dll, Newtonsoft.Json.dll, " +
                    "QiwiWalletInfoAPI.dll, Zeroit.Framework.LollipopControls.dll", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }

            new Thread(() => 
            {
                try
                {
                    dynamic json = JsonConvert.DeserializeObject(
                            Network.POST(Parameters.UrlCheck, $"hwid={Managment.GetHWID()}&username={Managment.GetUserName()}"));
                    var response = Convert.ToString(json["response"]);
                    var checkBanned = Convert.ToString(json["user"]["ban"]);

                    if (json["user"]["count"] != 0)
                    {
                        if (response == "ok")
                        {
                            Invoke(new Action(() =>
                            {
                                var form = new MainForm();
                                form.Show();
                                Hide();
                            }));
                        }
                        else if (checkBanned == "banned")
                            MessageBox.Show("Вы были забанены.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show("Не удалось найти Ваш аккаунт.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при входе: {ex.Message}", 
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }) { IsBackground = true }.Start();
        }
        private void MainRun_Click(object sender, EventArgs e)
        {
            try
            {
                var login = Managment.Generate(Managment.TypeGenerator.Login);
                var password = Managment.Generate(Managment.TypeGenerator.Password);
                var comment = Managment.GenerateComment();

                new Thread(() =>
                {
                    Invoke(new Action(() => { MainRun.Text = "Ожидание платежа"; }));
                    Parameters.ConnectServer();
                    var qa = new QiwiAPI(Parameters.ApiKey);
                    var works = true;
                    while (works)
                    {
                        var pay = qa.GetPayments(long.Parse(Parameters.Phone), 1).Data;

                        foreach (var item in pay)
                        {
                            var sum = Parameters.Sum;
                            Invoke(new Action(() =>
                                { label_text.Text = $"Для оплаты переведите {sum} руб.\nНа QIWI кошелек: {Parameters.Phone}\nи укажите комментарий: #{comment}"; }));
                            if (item.Sum.Amount == sum && item.Comment.ToString() == "#" + comment)
                            {
                                dynamic json = JsonConvert.DeserializeObject(Network.POST(Parameters.UrlReg,
                                            $"login={login}&password={password}&hwid={Managment.GetHWID()}&username={Managment.GetUserName()}&action=reg"));
                                Invoke(new Action(() =>
                                {
                                    label_text.Text = $"Ваш логин: " +
                                  $"{Convert.ToString(json["login"])}\nВаш пароль: {Convert.ToString(json["password"])}";
                                    Clipboard.SetText(label_text.Text);
                                    MainRun.Visible = false;
                                    button_ShowMain.Visible = true;
                                }));
                                works = false;
                            }
                        }

                        Thread.Sleep(2000);
                    }
                })
                { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", Text, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }

        private void button_ShowMain_Click(object sender, EventArgs e)
        {
            new Thread(() => 
            {
                var response = Network.POST(Parameters.UrlLogin, $"login={textbox_login.Text}&password={textBox_password.Text}" +
                    $"&hwid={Managment.GetHWID()}&username={Managment.GetUserName()}");

                if (response.Contains("ok"))
                {
                    Invoke(new Action(() =>
                    {
                        var form = new MainForm();
                        form.Show();
                        Hide();
                    }));
                }
                else
                    MessageBox.Show("Ошибка входа: возможно, ввели неверный пароль или не оплатили лицензию.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }) { IsBackground = true }.Start();
        }

        private void linkLabel_hwidanduserName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var res = MessageBox.Show($"HWID: {Managment.GetHWID()}\nUserName: {Managment.GetUserName()}\n\nЧтобы скопировать нажмите \"да\"",
                     Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
                Clipboard.SetText($"{Managment.GetHWID()}:{Managment.GetUserName()}");
        }
    }
}
