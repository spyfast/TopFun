namespace TopFun.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.flatClose1 = new FlatUI.FlatClose();
            this.flatMini1 = new FlatUI.FlatMini();
            this.textbox_login = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopTextBox();
            this.textBox_password = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopTextBox();
            this.zeroitLollipopLabel6 = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel();
            this.zeroitLollipopLabel1 = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel();
            this.MainRun = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopButton();
            this.label_text = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel();
            this.button_ShowMain = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopButton();
            this.linkLabel_hwidanduserName = new System.Windows.Forms.LinkLabel();
            this.username = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel();
            this.hwidLabel = new Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(113, 45);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(95, 85);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton1.TabIndex = 0;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            // 
            // flatClose1
            // 
            this.flatClose1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flatClose1.BackColor = System.Drawing.Color.White;
            this.flatClose1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.flatClose1.Font = new System.Drawing.Font("Marlett", 10F);
            this.flatClose1.Location = new System.Drawing.Point(311, 7);
            this.flatClose1.Name = "flatClose1";
            this.flatClose1.Size = new System.Drawing.Size(18, 18);
            this.flatClose1.TabIndex = 3;
            this.flatClose1.Text = "flatClose1";
            this.flatClose1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // flatMini1
            // 
            this.flatMini1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flatMini1.BackColor = System.Drawing.Color.White;
            this.flatMini1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatMini1.Font = new System.Drawing.Font("Marlett", 12F);
            this.flatMini1.Location = new System.Drawing.Point(287, 7);
            this.flatMini1.Name = "flatMini1";
            this.flatMini1.Size = new System.Drawing.Size(18, 18);
            this.flatMini1.TabIndex = 2;
            this.flatMini1.Text = "flatMini1";
            this.flatMini1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // textbox_login
            // 
            this.textbox_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textbox_login.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textbox_login.DisabledUnFocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textbox_login.EnabledUnFocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(142)))), ((int)(((byte)(245)))));
            this.textbox_login.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(234)))), ((int)(((byte)(0)))));
            this.textbox_login.ForeColor = System.Drawing.Color.White;
            this.textbox_login.IsEnabled = true;
            this.textbox_login.Location = new System.Drawing.Point(70, 162);
            this.textbox_login.MaxLength = 32767;
            this.textbox_login.Multiline = false;
            this.textbox_login.Name = "textbox_login";
            this.textbox_login.PasswordChar = '\0';
            this.textbox_login.ReadOnly = false;
            this.textbox_login.Size = new System.Drawing.Size(204, 24);
            this.textbox_login.TabIndex = 4;
            this.textbox_login.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.textbox_login.TextBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textbox_login.UseSystemPasswordChar = false;
            this.textbox_login.WordWrap = true;
            // 
            // textBox_password
            // 
            this.textBox_password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textBox_password.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textBox_password.DisabledUnFocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textBox_password.EnabledUnFocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(142)))), ((int)(((byte)(245)))));
            this.textBox_password.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(234)))), ((int)(((byte)(0)))));
            this.textBox_password.ForeColor = System.Drawing.Color.White;
            this.textBox_password.IsEnabled = true;
            this.textBox_password.Location = new System.Drawing.Point(70, 219);
            this.textBox_password.MaxLength = 32767;
            this.textBox_password.Multiline = false;
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '\0';
            this.textBox_password.ReadOnly = false;
            this.textBox_password.Size = new System.Drawing.Size(204, 24);
            this.textBox_password.TabIndex = 5;
            this.textBox_password.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBox_password.TextBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.textBox_password.UseSystemPasswordChar = false;
            this.textBox_password.WordWrap = true;
            // 
            // zeroitLollipopLabel6
            // 
            this.zeroitLollipopLabel6.AllowTransparency = true;
            this.zeroitLollipopLabel6.AutoSize = true;
            this.zeroitLollipopLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.zeroitLollipopLabel6.ForeColor = System.Drawing.Color.White;
            this.zeroitLollipopLabel6.Location = new System.Drawing.Point(140, 142);
            this.zeroitLollipopLabel6.Name = "zeroitLollipopLabel6";
            this.zeroitLollipopLabel6.Size = new System.Drawing.Size(47, 17);
            this.zeroitLollipopLabel6.TabIndex = 17;
            this.zeroitLollipopLabel6.Text = "Логин";
            // 
            // zeroitLollipopLabel1
            // 
            this.zeroitLollipopLabel1.AllowTransparency = true;
            this.zeroitLollipopLabel1.AutoSize = true;
            this.zeroitLollipopLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.zeroitLollipopLabel1.ForeColor = System.Drawing.Color.White;
            this.zeroitLollipopLabel1.Location = new System.Drawing.Point(140, 199);
            this.zeroitLollipopLabel1.Name = "zeroitLollipopLabel1";
            this.zeroitLollipopLabel1.Size = new System.Drawing.Size(57, 17);
            this.zeroitLollipopLabel1.TabIndex = 18;
            this.zeroitLollipopLabel1.Text = "Пароль";
            // 
            // MainRun
            // 
            this.MainRun.AllowTransparency = true;
            this.MainRun.AnimationInterval = 1;
            this.MainRun.BackColor = System.Drawing.Color.Transparent;
            this.MainRun.BGColor = "#508ef5";
            this.MainRun.Corners = 1F;
            this.MainRun.DisabledBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(178)))), ((int)(((byte)(181)))));
            this.MainRun.DoubleRipple = false;
            this.MainRun.EnabledBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(142)))), ((int)(((byte)(245)))));
            this.MainRun.FontColor = "#ffffff";
            this.MainRun.ForeColor = System.Drawing.Color.White;
            this.MainRun.Location = new System.Drawing.Point(70, 254);
            this.MainRun.Name = "MainRun";
            this.MainRun.RippleEffectColor = System.Drawing.Color.Black;
            this.MainRun.RippleOpacity = 25;
            this.MainRun.Size = new System.Drawing.Size(204, 21);
            this.MainRun.Smoothing = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.MainRun.TabIndex = 22;
            this.MainRun.Text = "Перейти к оплате";
            this.MainRun.TextRendering = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.MainRun.Click += new System.EventHandler(this.MainRun_Click);
            // 
            // label_text
            // 
            this.label_text.AllowTransparency = true;
            this.label_text.AutoSize = true;
            this.label_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label_text.ForeColor = System.Drawing.Color.White;
            this.label_text.Location = new System.Drawing.Point(71, 281);
            this.label_text.Name = "label_text";
            this.label_text.Size = new System.Drawing.Size(195, 34);
            this.label_text.TabIndex = 23;
            this.label_text.Text = "После оплаты Вам выдадут \r\nлогин и пароль для входа";
            this.label_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ShowMain
            // 
            this.button_ShowMain.AllowTransparency = true;
            this.button_ShowMain.AnimationInterval = 1;
            this.button_ShowMain.BackColor = System.Drawing.Color.Transparent;
            this.button_ShowMain.BGColor = "#508ef5";
            this.button_ShowMain.Corners = 1F;
            this.button_ShowMain.DisabledBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(178)))), ((int)(((byte)(181)))));
            this.button_ShowMain.DoubleRipple = false;
            this.button_ShowMain.EnabledBGColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(142)))), ((int)(((byte)(245)))));
            this.button_ShowMain.FontColor = "#ffffff";
            this.button_ShowMain.ForeColor = System.Drawing.Color.White;
            this.button_ShowMain.Location = new System.Drawing.Point(70, 254);
            this.button_ShowMain.Name = "button_ShowMain";
            this.button_ShowMain.RippleEffectColor = System.Drawing.Color.Black;
            this.button_ShowMain.RippleOpacity = 25;
            this.button_ShowMain.Size = new System.Drawing.Size(204, 21);
            this.button_ShowMain.Smoothing = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.button_ShowMain.TabIndex = 24;
            this.button_ShowMain.Text = "Войти";
            this.button_ShowMain.TextRendering = System.Drawing.Text.TextRenderingHint.AntiAlias;
            this.button_ShowMain.Visible = false;
            this.button_ShowMain.Click += new System.EventHandler(this.button_ShowMain_Click);
            // 
            // linkLabel_hwidanduserName
            // 
            this.linkLabel_hwidanduserName.AutoSize = true;
            this.linkLabel_hwidanduserName.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(242)))), ((int)(((byte)(32)))));
            this.linkLabel_hwidanduserName.Location = new System.Drawing.Point(86, 7);
            this.linkLabel_hwidanduserName.Name = "linkLabel_hwidanduserName";
            this.linkLabel_hwidanduserName.Size = new System.Drawing.Size(151, 13);
            this.linkLabel_hwidanduserName.TabIndex = 25;
            this.linkLabel_hwidanduserName.TabStop = true;
            this.linkLabel_hwidanduserName.Text = "Показать HWID и UserName";
            this.linkLabel_hwidanduserName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_hwidanduserName_LinkClicked);
            // 
            // username
            // 
            this.username.AllowTransparency = true;
            this.username.AutoSize = true;
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.username.ForeColor = System.Drawing.Color.White;
            this.username.Location = new System.Drawing.Point(12, 354);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(73, 17);
            this.username.TabIndex = 26;
            this.username.Text = "Username";
            // 
            // hwidLabel
            // 
            this.hwidLabel.AllowTransparency = true;
            this.hwidLabel.AutoSize = true;
            this.hwidLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.hwidLabel.ForeColor = System.Drawing.Color.White;
            this.hwidLabel.Location = new System.Drawing.Point(193, 354);
            this.hwidLabel.Name = "hwidLabel";
            this.hwidLabel.Size = new System.Drawing.Size(44, 17);
            this.hwidLabel.TabIndex = 27;
            this.hwidLabel.Text = "HWID";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(336, 385);
            this.Controls.Add(this.hwidLabel);
            this.Controls.Add(this.username);
            this.Controls.Add(this.linkLabel_hwidanduserName);
            this.Controls.Add(this.button_ShowMain);
            this.Controls.Add(this.label_text);
            this.Controls.Add(this.MainRun);
            this.Controls.Add(this.zeroitLollipopLabel1);
            this.Controls.Add(this.zeroitLollipopLabel6);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.textbox_login);
            this.Controls.Add(this.flatClose1);
            this.Controls.Add(this.flatMini1);
            this.Controls.Add(this.bunifuImageButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TopFun";
            this.Shown += new System.EventHandler(this.LoginForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton1;
        private FlatUI.FlatClose flatClose1;
        private FlatUI.FlatMini flatMini1;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopTextBox textbox_login;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopTextBox textBox_password;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel zeroitLollipopLabel6;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel zeroitLollipopLabel1;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopButton MainRun;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel label_text;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopButton button_ShowMain;
        private System.Windows.Forms.LinkLabel linkLabel_hwidanduserName;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel username;
        private Zeroit.Framework.LollipopControls.Controls.ZeroitLollipopLabel hwidLabel;
    }
}