namespace PDV.UTIL.FORMS.Forms
{
    partial class Login
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.ovTXT_Login = new System.Windows.Forms.TextBox();
            this.ovTXT_Senha = new System.Windows.Forms.TextBox();
            this.ovTXT_StatusLogin = new System.Windows.Forms.Label();
            this.ovCKB_Lembrar = new System.Windows.Forms.CheckBox();
            this.ovTXT_Versao = new System.Windows.Forms.Label();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnLogar = new System.Windows.Forms.Button();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.pictureBox1DueERP = new System.Windows.Forms.PictureBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1DuePDV = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new PDV.UTIL.Components.Custom.PictureBox();
            this.pictureBox2 = new PDV.UTIL.Components.Custom.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblShortDate = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1DueERP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1DuePDV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // ovTXT_Login
            // 
            this.ovTXT_Login.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ovTXT_Login.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovTXT_Login.Location = new System.Drawing.Point(428, 161);
            this.ovTXT_Login.Name = "ovTXT_Login";
            this.ovTXT_Login.Size = new System.Drawing.Size(177, 23);
            this.ovTXT_Login.TabIndex = 3;
            // 
            // ovTXT_Senha
            // 
            this.ovTXT_Senha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ovTXT_Senha.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovTXT_Senha.Location = new System.Drawing.Point(428, 210);
            this.ovTXT_Senha.Name = "ovTXT_Senha";
            this.ovTXT_Senha.PasswordChar = '*';
            this.ovTXT_Senha.Size = new System.Drawing.Size(177, 23);
            this.ovTXT_Senha.TabIndex = 4;
            this.ovTXT_Senha.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ovTXT_Senha_KeyUp);
            // 
            // ovTXT_StatusLogin
            // 
            this.ovTXT_StatusLogin.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ovTXT_StatusLogin.BackColor = System.Drawing.Color.Transparent;
            this.ovTXT_StatusLogin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovTXT_StatusLogin.ForeColor = System.Drawing.Color.White;
            this.ovTXT_StatusLogin.Location = new System.Drawing.Point(288, -28);
            this.ovTXT_StatusLogin.Name = "ovTXT_StatusLogin";
            this.ovTXT_StatusLogin.Size = new System.Drawing.Size(120, 23);
            this.ovTXT_StatusLogin.TabIndex = 9;
            this.ovTXT_StatusLogin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ovCKB_Lembrar
            // 
            this.ovCKB_Lembrar.AutoSize = true;
            this.ovCKB_Lembrar.BackColor = System.Drawing.Color.Transparent;
            this.ovCKB_Lembrar.Checked = true;
            this.ovCKB_Lembrar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ovCKB_Lembrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovCKB_Lembrar.ForeColor = System.Drawing.Color.Black;
            this.ovCKB_Lembrar.Location = new System.Drawing.Point(478, 239);
            this.ovCKB_Lembrar.Name = "ovCKB_Lembrar";
            this.ovCKB_Lembrar.Size = new System.Drawing.Size(127, 20);
            this.ovCKB_Lembrar.TabIndex = 13;
            this.ovCKB_Lembrar.Text = "Lembrar Usuário";
            this.ovCKB_Lembrar.UseVisualStyleBackColor = false;
            // 
            // ovTXT_Versao
            // 
            this.ovTXT_Versao.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ovTXT_Versao.AutoSize = true;
            this.ovTXT_Versao.BackColor = System.Drawing.Color.Transparent;
            this.ovTXT_Versao.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ovTXT_Versao.ForeColor = System.Drawing.Color.White;
            this.ovTXT_Versao.Location = new System.Drawing.Point(437, 421);
            this.ovTXT_Versao.Name = "ovTXT_Versao";
            this.ovTXT_Versao.Size = new System.Drawing.Size(40, 13);
            this.ovTXT_Versao.TabIndex = 17;
            this.ovTXT_Versao.Text = "Versão";
            this.ovTXT_Versao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(9)))), ((int)(((byte)(96)))));
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.ForeColor = System.Drawing.Color.White;
            this.btnSair.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSair.Location = new System.Drawing.Point(428, 262);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(74, 30);
            this.btnSair.TabIndex = 23;
            this.btnSair.Text = "&Sair";
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnLogar
            // 
            this.btnLogar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(9)))), ((int)(((byte)(96)))));
            this.btnLogar.FlatAppearance.BorderSize = 0;
            this.btnLogar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogar.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogar.ForeColor = System.Drawing.Color.White;
            this.btnLogar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogar.Location = new System.Drawing.Point(532, 262);
            this.btnLogar.Name = "btnLogar";
            this.btnLogar.Size = new System.Drawing.Size(73, 30);
            this.btnLogar.TabIndex = 19;
            this.btnLogar.Text = "&Acessar";
            this.btnLogar.UseVisualStyleBackColor = false;
            this.btnLogar.Click += new System.EventHandler(this.button1_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton1.ImageOptions.SvgImage")));
            this.simpleButton1.Location = new System.Drawing.Point(12, 421);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton1.Size = new System.Drawing.Size(35, 23);
            this.simpleButton1.TabIndex = 28;
            this.simpleButton1.Text = "Configuração";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton2.ImageOptions.SvgImage")));
            this.simpleButton2.Location = new System.Drawing.Point(650, 421);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton2.Size = new System.Drawing.Size(38, 23);
            this.simpleButton2.TabIndex = 29;
            this.simpleButton2.Text = "Licença";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // pictureBox1DueERP
            // 
            this.pictureBox1DueERP.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1DueERP.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1DueERP.Image")));
            this.pictureBox1DueERP.Location = new System.Drawing.Point(69, 142);
            this.pictureBox1DueERP.Name = "pictureBox1DueERP";
            this.pictureBox1DueERP.Size = new System.Drawing.Size(242, 171);
            this.pictureBox1DueERP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1DueERP.TabIndex = 30;
            this.pictureBox1DueERP.TabStop = false;
            this.pictureBox1DueERP.Visible = false;
            this.pictureBox1DueERP.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(428, 142);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 16);
            this.labelControl1.TabIndex = 31;
            this.labelControl1.Text = "Informe o Login";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(428, 191);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 16);
            this.labelControl2.TabIndex = 32;
            this.labelControl2.Text = "Informe a Senha";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(9, 355);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(88, 16);
            this.linkLabel1.TabIndex = 35;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Mudar Senha";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pictureBox1DuePDV
            // 
            this.pictureBox1DuePDV.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1DuePDV.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1DuePDV.Image")));
            this.pictureBox1DuePDV.Location = new System.Drawing.Point(69, 142);
            this.pictureBox1DuePDV.Name = "pictureBox1DuePDV";
            this.pictureBox1DuePDV.Size = new System.Drawing.Size(242, 171);
            this.pictureBox1DuePDV.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1DuePDV.TabIndex = 37;
            this.pictureBox1DuePDV.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.IDItemMenu = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(393, 210);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(29, 23);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 34;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.IDItemMenu = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(393, 161);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 23);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(58, 43);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 424;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(65, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(204, 24);
            this.label5.TabIndex = 431;
            this.label5.Text = "GESTÃO COMERCIAL";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(542, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 24);
            this.label7.TabIndex = 433;
            this.label7.Text = "Seja Bem Vindo";
            // 
            // lblShortDate
            // 
            this.lblShortDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblShortDate.AutoSize = true;
            this.lblShortDate.BackColor = System.Drawing.Color.Transparent;
            this.lblShortDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShortDate.ForeColor = System.Drawing.Color.Black;
            this.lblShortDate.Location = new System.Drawing.Point(212, 407);
            this.lblShortDate.Name = "lblShortDate";
            this.lblShortDate.Size = new System.Drawing.Size(283, 14);
            this.lblShortDate.TabIndex = 434;
            this.lblShortDate.Text = "Fortaleza , 11 de Fevereiro de 2016 00:00:00";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(584, 355);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(104, 60);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 435;
            this.pictureBox4.TabStop = false;
            // 
            // button3
            // 
            this.button3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.Location = new System.Drawing.Point(12, 388);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(34, 27);
            this.button3.TabIndex = 436;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.simpleButton3.Appearance.Options.UseFont = true;
            this.simpleButton3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButton3.ImageOptions.SvgImage")));
            this.simpleButton3.Location = new System.Drawing.Point(293, 355);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.simpleButton3.Size = new System.Drawing.Size(144, 46);
            this.simpleButton3.TabIndex = 437;
            this.simpleButton3.Text = "Atualizar Agora!";
            this.simpleButton3.Visible = false;
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // Login
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = global::PDV.UTIL.FORMS.Properties.Resources.White_abstract_background_with_wave_vector_illustration_01;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.lblShortDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox1DuePDV);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnLogar);
            this.Controls.Add(this.ovTXT_Versao);
            this.Controls.Add(this.ovCKB_Lembrar);
            this.Controls.Add(this.ovTXT_StatusLogin);
            this.Controls.Add(this.ovTXT_Senha);
            this.Controls.Add(this.ovTXT_Login);
            this.Controls.Add(this.pictureBox1DueERP);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.InactiveGlowColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1DueERP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1DuePDV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ovTXT_StatusLogin;
        public System.Windows.Forms.TextBox ovTXT_Login;
        public System.Windows.Forms.TextBox ovTXT_Senha;
        public System.Windows.Forms.CheckBox ovCKB_Lembrar;
        public System.Windows.Forms.Label ovTXT_Versao;
        private System.Windows.Forms.Button btnLogar;
        private System.Windows.Forms.Button btnSair;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private System.Windows.Forms.PictureBox pictureBox1DueERP;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private Components.Custom.PictureBox pictureBox2;
        private Components.Custom.PictureBox pictureBox3;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.PictureBox pictureBox1DuePDV;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.Label lblShortDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox4;
        internal System.Windows.Forms.Button button3;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
    }
}