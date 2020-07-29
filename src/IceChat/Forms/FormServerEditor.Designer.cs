namespace IceChat
{
    partial class FormServers
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
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.labelServerPassword = new System.Windows.Forms.Label();
            this.textServerPassword = new System.Windows.Forms.TextBox();
            this.textNickservPassword = new System.Windows.Forms.TextBox();
            this.labelNickservPassword = new System.Windows.Forms.Label();
            this.checkUseSSL = new System.Windows.Forms.CheckBox();
            this.checkInvalidCertificate = new System.Windows.Forms.CheckBox();
            this.checkUseIPv6 = new System.Windows.Forms.CheckBox();
            this.textAwayNick = new System.Windows.Forms.TextBox();
            this.labelAwayNickName = new System.Windows.Forms.Label();
            this.textAltNickName = new System.Windows.Forms.TextBox();
            this.labelAltNickName = new System.Windows.Forms.Label();
            this.textDisplayName = new System.Windows.Forms.TextBox();
            this.labelDisplayName = new System.Windows.Forms.Label();
            this.textQuitMessage = new System.Windows.Forms.TextBox();
            this.labelQuitMessage = new System.Windows.Forms.Label();
            this.labelFullName = new System.Windows.Forms.Label();
            this.textFullName = new System.Windows.Forms.TextBox();
            this.textIdentName = new System.Windows.Forms.TextBox();
            this.labelIdentName = new System.Windows.Forms.Label();
            this.textServerPort = new System.Windows.Forms.TextBox();
            this.textServername = new System.Windows.Forms.TextBox();
            this.textNickName = new System.Windows.Forms.TextBox();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.labelServerName = new System.Windows.Forms.Label();
            this.labelNickName = new System.Windows.Forms.Label();
            this.tabPageExtra = new System.Windows.Forms.TabPage();
            this.checkNoColorMode = new System.Windows.Forms.CheckBox();
            this.checkDisableQueries = new System.Windows.Forms.CheckBox();
            this.checkDisableLogging = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textReconnectTime = new System.Windows.Forms.TextBox();
            this.checkDisableAwayMessages = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textPingTimer = new System.Windows.Forms.TextBox();
            this.checkAutoStart = new System.Windows.Forms.CheckBox();
            this.checkDisableCTCP = new System.Windows.Forms.CheckBox();
            this.labelEncoding = new System.Windows.Forms.Label();
            this.comboEncoding = new System.Windows.Forms.ComboBox();
            this.checkRejoinChannel = new System.Windows.Forms.CheckBox();
            this.checkPingPong = new System.Windows.Forms.CheckBox();
            this.checkMOTD = new System.Windows.Forms.CheckBox();
            this.checkModeI = new System.Windows.Forms.CheckBox();
            this.tabPageIRCV3 = new System.Windows.Forms.TabPage();
            this.checkEchoMessage = new System.Windows.Forms.CheckBox();
            this.checkChgHost = new System.Windows.Forms.CheckBox();
            this.checkAccountNotify = new System.Windows.Forms.CheckBox();
            this.checkAwayNotify = new System.Windows.Forms.CheckBox();
            this.checkExtendedJoin = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textSASLUser = new System.Windows.Forms.TextBox();
            this.textSASLPass = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkUseSASL = new System.Windows.Forms.CheckBox();
            this.tabPageAutoJoin = new System.Windows.Forms.TabPage();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkAutoJoinDelayBetween = new System.Windows.Forms.CheckBox();
            this.checkAutoJoinDelay = new System.Windows.Forms.CheckBox();
            this.checkAutoJoin = new System.Windows.Forms.CheckBox();
            this.buttonEditAutoJoin = new System.Windows.Forms.Button();
            this.listChannel = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRemoveAutoJoin = new System.Windows.Forms.Button();
            this.buttonAddAutoJoin = new System.Windows.Forms.Button();
            this.textChannel = new System.Windows.Forms.TextBox();
            this.labelChannel = new System.Windows.Forms.Label();
            this.tabPageAutoPerform = new System.Windows.Forms.TabPage();
            this.textAutoPerform = new System.Windows.Forms.TextBox();
            this.checkAutoPerform = new System.Windows.Forms.CheckBox();
            this.tabPageIgnore = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.buttonClearIgnores = new System.Windows.Forms.Button();
            this.labelIgnoreNote = new System.Windows.Forms.Label();
            this.listIgnore = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnI = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkIgnore = new System.Windows.Forms.CheckBox();
            this.textIgnore = new System.Windows.Forms.TextBox();
            this.labelNickHost = new System.Windows.Forms.Label();
            this.buttonEditIgnore = new System.Windows.Forms.Button();
            this.buttonRemoveIgnore = new System.Windows.Forms.Button();
            this.buttonAddIgnore = new System.Windows.Forms.Button();
            this.tabBuddyList = new System.Windows.Forms.TabPage();
            this.listBuddyList = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBuddyList = new System.Windows.Forms.CheckBox();
            this.textBuddy = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonEditBuddy = new System.Windows.Forms.Button();
            this.buttonRemoveBuddy = new System.Windows.Forms.Button();
            this.buttonAddBuddy = new System.Windows.Forms.Button();
            this.tabPageNotes = new System.Windows.Forms.TabPage();
            this.textNotes = new System.Windows.Forms.TextBox();
            this.tabPageProxy = new System.Windows.Forms.TabPage();
            this.textProxyPass = new System.Windows.Forms.TextBox();
            this.labelProxyPass = new System.Windows.Forms.Label();
            this.textProxyUser = new System.Windows.Forms.TextBox();
            this.labelProxyUser = new System.Windows.Forms.Label();
            this.radioSocksHTTP = new System.Windows.Forms.RadioButton();
            this.radioSocks4 = new System.Windows.Forms.RadioButton();
            this.radioSocks5 = new System.Windows.Forms.RadioButton();
            this.textProxyPort = new System.Windows.Forms.TextBox();
            this.labelProxyPort = new System.Windows.Forms.Label();
            this.textProxyIP = new System.Windows.Forms.TextBox();
            this.labelProxyIP = new System.Windows.Forms.Label();
            this.checkUseProxy = new System.Windows.Forms.CheckBox();
            this.tabPageBNC = new System.Windows.Forms.TabPage();
            this.textBNCPass = new System.Windows.Forms.TextBox();
            this.labelBNCPass = new System.Windows.Forms.Label();
            this.textBNCUser = new System.Windows.Forms.TextBox();
            this.labelBNCUser = new System.Windows.Forms.Label();
            this.textBNCPort = new System.Windows.Forms.TextBox();
            this.labelBNCPort = new System.Windows.Forms.Label();
            this.textBNCIP = new System.Windows.Forms.TextBox();
            this.labelBNCIP = new System.Windows.Forms.Label();
            this.checkUseBNC = new System.Windows.Forms.CheckBox();
            this.tabPageDefault = new System.Windows.Forms.TabPage();
            this.textDefaultQuitMessage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textDefaultFullName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textDefaultIdent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkServerReconnect = new System.Windows.Forms.CheckBox();
            this.checkIdentServer = new System.Windows.Forms.CheckBox();
            this.textDefaultNick = new System.Windows.Forms.TextBox();
            this.labelDefaultNickName = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonDuplicateServer = new System.Windows.Forms.Button();
            this.buttonRemoveServer = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.tabControlSettings.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageExtra.SuspendLayout();
            this.tabPageIRCV3.SuspendLayout();
            this.tabPageAutoJoin.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageAutoPerform.SuspendLayout();
            this.tabPageIgnore.SuspendLayout();
            this.tabBuddyList.SuspendLayout();
            this.tabPageNotes.SuspendLayout();
            this.tabPageProxy.SuspendLayout();
            this.tabPageBNC.SuspendLayout();
            this.tabPageDefault.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSettings
            // 
            this.tabControlSettings.AccessibleDescription = "";
            this.tabControlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSettings.Controls.Add(this.tabPageMain);
            this.tabControlSettings.Controls.Add(this.tabPageExtra);
            this.tabControlSettings.Controls.Add(this.tabPageIRCV3);
            this.tabControlSettings.Controls.Add(this.tabPageAutoJoin);
            this.tabControlSettings.Controls.Add(this.tabPageAutoPerform);
            this.tabControlSettings.Controls.Add(this.tabPageIgnore);
            this.tabControlSettings.Controls.Add(this.tabBuddyList);
            this.tabControlSettings.Controls.Add(this.tabPageNotes);
            this.tabControlSettings.Controls.Add(this.tabPageProxy);
            this.tabControlSettings.Controls.Add(this.tabPageBNC);
            this.tabControlSettings.Controls.Add(this.tabPageDefault);
            this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
            this.tabControlSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlSettings.Multiline = true;
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(681, 298);
            this.tabControlSettings.TabIndex = 0;
            // 
            // tabPageMain
            // 
            this.tabPageMain.BackColor = System.Drawing.Color.Transparent;
            this.tabPageMain.Controls.Add(this.labelServerPassword);
            this.tabPageMain.Controls.Add(this.textServerPassword);
            this.tabPageMain.Controls.Add(this.textNickservPassword);
            this.tabPageMain.Controls.Add(this.labelNickservPassword);
            this.tabPageMain.Controls.Add(this.checkUseSSL);
            this.tabPageMain.Controls.Add(this.checkInvalidCertificate);
            this.tabPageMain.Controls.Add(this.checkUseIPv6);
            this.tabPageMain.Controls.Add(this.textAwayNick);
            this.tabPageMain.Controls.Add(this.labelAwayNickName);
            this.tabPageMain.Controls.Add(this.textAltNickName);
            this.tabPageMain.Controls.Add(this.labelAltNickName);
            this.tabPageMain.Controls.Add(this.textDisplayName);
            this.tabPageMain.Controls.Add(this.labelDisplayName);
            this.tabPageMain.Controls.Add(this.textQuitMessage);
            this.tabPageMain.Controls.Add(this.labelQuitMessage);
            this.tabPageMain.Controls.Add(this.labelFullName);
            this.tabPageMain.Controls.Add(this.textFullName);
            this.tabPageMain.Controls.Add(this.textIdentName);
            this.tabPageMain.Controls.Add(this.labelIdentName);
            this.tabPageMain.Controls.Add(this.textServerPort);
            this.tabPageMain.Controls.Add(this.textServername);
            this.tabPageMain.Controls.Add(this.textNickName);
            this.tabPageMain.Controls.Add(this.labelServerPort);
            this.tabPageMain.Controls.Add(this.labelServerName);
            this.tabPageMain.Controls.Add(this.labelNickName);
            this.tabPageMain.ImageIndex = 0;
            this.tabPageMain.Location = new System.Drawing.Point(4, 46);
            this.tabPageMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Size = new System.Drawing.Size(673, 248);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "Main Settings";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // labelServerPassword
            // 
            this.labelServerPassword.AutoSize = true;
            this.labelServerPassword.Location = new System.Drawing.Point(9, 170);
            this.labelServerPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelServerPassword.Name = "labelServerPassword";
            this.labelServerPassword.Size = new System.Drawing.Size(118, 16);
            this.labelServerPassword.TabIndex = 59;
            this.labelServerPassword.Text = "Server password";
            // 
            // textServerPassword
            // 
            this.textServerPassword.Location = new System.Drawing.Point(165, 165);
            this.textServerPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textServerPassword.Name = "textServerPassword";
            this.textServerPassword.PasswordChar = '*';
            this.textServerPassword.Size = new System.Drawing.Size(171, 23);
            this.textServerPassword.TabIndex = 57;
            // 
            // textNickservPassword
            // 
            this.textNickservPassword.Location = new System.Drawing.Point(165, 190);
            this.textNickservPassword.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textNickservPassword.Name = "textNickservPassword";
            this.textNickservPassword.PasswordChar = '*';
            this.textNickservPassword.Size = new System.Drawing.Size(171, 23);
            this.textNickservPassword.TabIndex = 58;
            // 
            // labelNickservPassword
            // 
            this.labelNickservPassword.AutoSize = true;
            this.labelNickservPassword.Location = new System.Drawing.Point(9, 195);
            this.labelNickservPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNickservPassword.Name = "labelNickservPassword";
            this.labelNickservPassword.Size = new System.Drawing.Size(130, 16);
            this.labelNickservPassword.TabIndex = 60;
            this.labelNickservPassword.Text = "Nickserv password";
            // 
            // checkUseSSL
            // 
            this.checkUseSSL.AutoSize = true;
            this.checkUseSSL.Location = new System.Drawing.Point(400, 132);
            this.checkUseSSL.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseSSL.Name = "checkUseSSL";
            this.checkUseSSL.Size = new System.Drawing.Size(176, 20);
            this.checkUseSSL.TabIndex = 54;
            this.checkUseSSL.Text = "Connect with SSL/TLS";
            this.checkUseSSL.UseVisualStyleBackColor = true;
            // 
            // checkInvalidCertificate
            // 
            this.checkInvalidCertificate.AutoSize = true;
            this.checkInvalidCertificate.Location = new System.Drawing.Point(400, 158);
            this.checkInvalidCertificate.Margin = new System.Windows.Forms.Padding(2);
            this.checkInvalidCertificate.Name = "checkInvalidCertificate";
            this.checkInvalidCertificate.Size = new System.Drawing.Size(230, 20);
            this.checkInvalidCertificate.TabIndex = 53;
            this.checkInvalidCertificate.Text = "Accept invalid SSL certificates";
            this.checkInvalidCertificate.UseVisualStyleBackColor = true;
            // 
            // checkUseIPv6
            // 
            this.checkUseIPv6.AutoSize = true;
            this.checkUseIPv6.Location = new System.Drawing.Point(400, 107);
            this.checkUseIPv6.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseIPv6.Name = "checkUseIPv6";
            this.checkUseIPv6.Size = new System.Drawing.Size(149, 20);
            this.checkUseIPv6.TabIndex = 52;
            this.checkUseIPv6.Text = "Connect with IPv6";
            this.checkUseIPv6.UseVisualStyleBackColor = true;
            // 
            // textAwayNick
            // 
            this.textAwayNick.Location = new System.Drawing.Point(502, 80);
            this.textAwayNick.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textAwayNick.Name = "textAwayNick";
            this.textAwayNick.Size = new System.Drawing.Size(121, 23);
            this.textAwayNick.TabIndex = 50;
            // 
            // labelAwayNickName
            // 
            this.labelAwayNickName.AutoSize = true;
            this.labelAwayNickName.Location = new System.Drawing.Point(398, 85);
            this.labelAwayNickName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAwayNickName.Name = "labelAwayNickName";
            this.labelAwayNickName.Size = new System.Drawing.Size(76, 16);
            this.labelAwayNickName.TabIndex = 49;
            this.labelAwayNickName.Text = "Away Nick";
            // 
            // textAltNickName
            // 
            this.textAltNickName.Location = new System.Drawing.Point(502, 55);
            this.textAltNickName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textAltNickName.Name = "textAltNickName";
            this.textAltNickName.Size = new System.Drawing.Size(121, 23);
            this.textAltNickName.TabIndex = 48;
            // 
            // labelAltNickName
            // 
            this.labelAltNickName.AutoSize = true;
            this.labelAltNickName.Location = new System.Drawing.Point(398, 60);
            this.labelAltNickName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAltNickName.Name = "labelAltNickName";
            this.labelAltNickName.Size = new System.Drawing.Size(63, 16);
            this.labelAltNickName.TabIndex = 47;
            this.labelAltNickName.Text = "Alt. Nick";
            // 
            // textDisplayName
            // 
            this.textDisplayName.Location = new System.Drawing.Point(116, 30);
            this.textDisplayName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textDisplayName.Name = "textDisplayName";
            this.textDisplayName.Size = new System.Drawing.Size(220, 23);
            this.textDisplayName.TabIndex = 2;
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.AutoSize = true;
            this.labelDisplayName.Location = new System.Drawing.Point(10, 35);
            this.labelDisplayName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(94, 16);
            this.labelDisplayName.TabIndex = 46;
            this.labelDisplayName.Text = "Display name";
            // 
            // textQuitMessage
            // 
            this.textQuitMessage.Location = new System.Drawing.Point(116, 130);
            this.textQuitMessage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textQuitMessage.Name = "textQuitMessage";
            this.textQuitMessage.Size = new System.Drawing.Size(220, 23);
            this.textQuitMessage.TabIndex = 6;
            // 
            // labelQuitMessage
            // 
            this.labelQuitMessage.AutoSize = true;
            this.labelQuitMessage.Location = new System.Drawing.Point(9, 135);
            this.labelQuitMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelQuitMessage.Name = "labelQuitMessage";
            this.labelQuitMessage.Size = new System.Drawing.Size(97, 16);
            this.labelQuitMessage.TabIndex = 45;
            this.labelQuitMessage.Text = "Quit message";
            // 
            // labelFullName
            // 
            this.labelFullName.AutoSize = true;
            this.labelFullName.Location = new System.Drawing.Point(10, 110);
            this.labelFullName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFullName.Name = "labelFullName";
            this.labelFullName.Size = new System.Drawing.Size(65, 16);
            this.labelFullName.TabIndex = 43;
            this.labelFullName.Text = "Fullname";
            // 
            // textFullName
            // 
            this.textFullName.Location = new System.Drawing.Point(116, 105);
            this.textFullName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textFullName.Name = "textFullName";
            this.textFullName.Size = new System.Drawing.Size(220, 23);
            this.textFullName.TabIndex = 5;
            // 
            // textIdentName
            // 
            this.textIdentName.Location = new System.Drawing.Point(116, 80);
            this.textIdentName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textIdentName.Name = "textIdentName";
            this.textIdentName.Size = new System.Drawing.Size(121, 23);
            this.textIdentName.TabIndex = 4;
            // 
            // labelIdentName
            // 
            this.labelIdentName.AutoSize = true;
            this.labelIdentName.Location = new System.Drawing.Point(10, 85);
            this.labelIdentName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIdentName.Name = "labelIdentName";
            this.labelIdentName.Size = new System.Drawing.Size(83, 16);
            this.labelIdentName.TabIndex = 41;
            this.labelIdentName.Text = "Ident name";
            // 
            // textServerPort
            // 
            this.textServerPort.Location = new System.Drawing.Point(502, 5);
            this.textServerPort.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textServerPort.Name = "textServerPort";
            this.textServerPort.Size = new System.Drawing.Size(51, 23);
            this.textServerPort.TabIndex = 1;
            // 
            // textServername
            // 
            this.textServername.Location = new System.Drawing.Point(116, 5);
            this.textServername.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textServername.Name = "textServername";
            this.textServername.Size = new System.Drawing.Size(220, 23);
            this.textServername.TabIndex = 0;
            // 
            // textNickName
            // 
            this.textNickName.Location = new System.Drawing.Point(116, 55);
            this.textNickName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textNickName.Name = "textNickName";
            this.textNickName.Size = new System.Drawing.Size(121, 23);
            this.textNickName.TabIndex = 3;
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.Location = new System.Drawing.Point(398, 10);
            this.labelServerPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(83, 16);
            this.labelServerPort.TabIndex = 39;
            this.labelServerPort.Text = "Server port";
            // 
            // labelServerName
            // 
            this.labelServerName.AutoSize = true;
            this.labelServerName.ForeColor = System.Drawing.Color.Red;
            this.labelServerName.Location = new System.Drawing.Point(10, 10);
            this.labelServerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelServerName.Name = "labelServerName";
            this.labelServerName.Size = new System.Drawing.Size(51, 16);
            this.labelServerName.TabIndex = 38;
            this.labelServerName.Text = "Server";
            // 
            // labelNickName
            // 
            this.labelNickName.AutoSize = true;
            this.labelNickName.ForeColor = System.Drawing.Color.Red;
            this.labelNickName.Location = new System.Drawing.Point(9, 60);
            this.labelNickName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNickName.Name = "labelNickName";
            this.labelNickName.Size = new System.Drawing.Size(35, 16);
            this.labelNickName.TabIndex = 36;
            this.labelNickName.Text = "Nick";
            // 
            // tabPageExtra
            // 
            this.tabPageExtra.BackColor = System.Drawing.Color.Transparent;
            this.tabPageExtra.Controls.Add(this.checkNoColorMode);
            this.tabPageExtra.Controls.Add(this.checkDisableQueries);
            this.tabPageExtra.Controls.Add(this.checkDisableLogging);
            this.tabPageExtra.Controls.Add(this.label13);
            this.tabPageExtra.Controls.Add(this.textReconnectTime);
            this.tabPageExtra.Controls.Add(this.checkDisableAwayMessages);
            this.tabPageExtra.Controls.Add(this.label11);
            this.tabPageExtra.Controls.Add(this.textPingTimer);
            this.tabPageExtra.Controls.Add(this.checkAutoStart);
            this.tabPageExtra.Controls.Add(this.checkDisableCTCP);
            this.tabPageExtra.Controls.Add(this.labelEncoding);
            this.tabPageExtra.Controls.Add(this.comboEncoding);
            this.tabPageExtra.Controls.Add(this.checkRejoinChannel);
            this.tabPageExtra.Controls.Add(this.checkPingPong);
            this.tabPageExtra.Controls.Add(this.checkMOTD);
            this.tabPageExtra.Controls.Add(this.checkModeI);
            this.tabPageExtra.ImageIndex = 1;
            this.tabPageExtra.Location = new System.Drawing.Point(4, 46);
            this.tabPageExtra.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageExtra.Name = "tabPageExtra";
            this.tabPageExtra.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageExtra.Size = new System.Drawing.Size(673, 248);
            this.tabPageExtra.TabIndex = 3;
            this.tabPageExtra.Text = "Extra Settings";
            this.tabPageExtra.UseVisualStyleBackColor = true;
            // 
            // checkNoColorMode
            // 
            this.checkNoColorMode.AutoSize = true;
            this.checkNoColorMode.Location = new System.Drawing.Point(312, 106);
            this.checkNoColorMode.Margin = new System.Windows.Forms.Padding(2);
            this.checkNoColorMode.Name = "checkNoColorMode";
            this.checkNoColorMode.Size = new System.Drawing.Size(198, 20);
            this.checkNoColorMode.TabIndex = 72;
            this.checkNoColorMode.Text = "Disable Colors in Channels";
            this.checkNoColorMode.UseVisualStyleBackColor = true;
            // 
            // checkDisableQueries
            // 
            this.checkDisableQueries.AutoSize = true;
            this.checkDisableQueries.Location = new System.Drawing.Point(312, 34);
            this.checkDisableQueries.Margin = new System.Windows.Forms.Padding(2);
            this.checkDisableQueries.Name = "checkDisableQueries";
            this.checkDisableQueries.Size = new System.Drawing.Size(127, 20);
            this.checkDisableQueries.TabIndex = 71;
            this.checkDisableQueries.Text = "Disable Queries";
            this.checkDisableQueries.UseVisualStyleBackColor = true;
            // 
            // checkDisableLogging
            // 
            this.checkDisableLogging.AutoSize = true;
            this.checkDisableLogging.Location = new System.Drawing.Point(312, 10);
            this.checkDisableLogging.Margin = new System.Windows.Forms.Padding(2);
            this.checkDisableLogging.Name = "checkDisableLogging";
            this.checkDisableLogging.Size = new System.Drawing.Size(128, 20);
            this.checkDisableLogging.TabIndex = 68;
            this.checkDisableLogging.Text = "Disable Logging";
            this.checkDisableLogging.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 209);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(190, 16);
            this.label13.TabIndex = 67;
            this.label13.Text = "Reconnect Timer (seconds)";
            // 
            // textReconnectTime
            // 
            this.textReconnectTime.Location = new System.Drawing.Point(223, 205);
            this.textReconnectTime.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textReconnectTime.Name = "textReconnectTime";
            this.textReconnectTime.Size = new System.Drawing.Size(37, 23);
            this.textReconnectTime.TabIndex = 66;
            // 
            // checkDisableAwayMessages
            // 
            this.checkDisableAwayMessages.AutoSize = true;
            this.checkDisableAwayMessages.Location = new System.Drawing.Point(312, 58);
            this.checkDisableAwayMessages.Margin = new System.Windows.Forms.Padding(2);
            this.checkDisableAwayMessages.Name = "checkDisableAwayMessages";
            this.checkDisableAwayMessages.Size = new System.Drawing.Size(228, 20);
            this.checkDisableAwayMessages.TabIndex = 61;
            this.checkDisableAwayMessages.Text = "Disable away/return messages";
            this.checkDisableAwayMessages.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 181);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 16);
            this.label11.TabIndex = 60;
            this.label11.Text = "Ping Timer (minutes)";
            // 
            // textPingTimer
            // 
            this.textPingTimer.Location = new System.Drawing.Point(223, 177);
            this.textPingTimer.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textPingTimer.Name = "textPingTimer";
            this.textPingTimer.Size = new System.Drawing.Size(37, 23);
            this.textPingTimer.TabIndex = 59;
            // 
            // checkAutoStart
            // 
            this.checkAutoStart.AutoSize = true;
            this.checkAutoStart.Location = new System.Drawing.Point(12, 154);
            this.checkAutoStart.Margin = new System.Windows.Forms.Padding(2);
            this.checkAutoStart.Name = "checkAutoStart";
            this.checkAutoStart.Size = new System.Drawing.Size(156, 20);
            this.checkAutoStart.TabIndex = 5;
            this.checkAutoStart.Text = "Connect on startup";
            this.checkAutoStart.UseVisualStyleBackColor = true;
            // 
            // checkDisableCTCP
            // 
            this.checkDisableCTCP.AutoSize = true;
            this.checkDisableCTCP.Location = new System.Drawing.Point(312, 82);
            this.checkDisableCTCP.Margin = new System.Windows.Forms.Padding(2);
            this.checkDisableCTCP.Name = "checkDisableCTCP";
            this.checkDisableCTCP.Size = new System.Drawing.Size(160, 20);
            this.checkDisableCTCP.TabIndex = 4;
            this.checkDisableCTCP.Text = "Disable CTCP replies";
            this.checkDisableCTCP.UseVisualStyleBackColor = true;
            // 
            // labelEncoding
            // 
            this.labelEncoding.AutoSize = true;
            this.labelEncoding.Location = new System.Drawing.Point(309, 209);
            this.labelEncoding.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEncoding.Name = "labelEncoding";
            this.labelEncoding.Size = new System.Drawing.Size(67, 16);
            this.labelEncoding.TabIndex = 5;
            this.labelEncoding.Text = "Encoding";
            // 
            // comboEncoding
            // 
            this.comboEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboEncoding.FormattingEnabled = true;
            this.comboEncoding.Location = new System.Drawing.Point(397, 206);
            this.comboEncoding.Margin = new System.Windows.Forms.Padding(2);
            this.comboEncoding.Name = "comboEncoding";
            this.comboEncoding.Size = new System.Drawing.Size(131, 24);
            this.comboEncoding.TabIndex = 7;
            // 
            // checkRejoinChannel
            // 
            this.checkRejoinChannel.AutoSize = true;
            this.checkRejoinChannel.Checked = true;
            this.checkRejoinChannel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkRejoinChannel.Location = new System.Drawing.Point(12, 82);
            this.checkRejoinChannel.Margin = new System.Windows.Forms.Padding(2);
            this.checkRejoinChannel.Name = "checkRejoinChannel";
            this.checkRejoinChannel.Size = new System.Drawing.Size(209, 20);
            this.checkRejoinChannel.TabIndex = 3;
            this.checkRejoinChannel.Text = "Rejoin channels on connect";
            this.checkRejoinChannel.UseVisualStyleBackColor = true;
            // 
            // checkPingPong
            // 
            this.checkPingPong.AutoSize = true;
            this.checkPingPong.Location = new System.Drawing.Point(12, 58);
            this.checkPingPong.Margin = new System.Windows.Forms.Padding(2);
            this.checkPingPong.Name = "checkPingPong";
            this.checkPingPong.Size = new System.Drawing.Size(209, 20);
            this.checkPingPong.TabIndex = 2;
            this.checkPingPong.Text = "Show PING PONG messages";
            this.checkPingPong.UseVisualStyleBackColor = true;
            // 
            // checkMOTD
            // 
            this.checkMOTD.AutoSize = true;
            this.checkMOTD.Location = new System.Drawing.Point(12, 34);
            this.checkMOTD.Margin = new System.Windows.Forms.Padding(2);
            this.checkMOTD.Name = "checkMOTD";
            this.checkMOTD.Size = new System.Drawing.Size(187, 20);
            this.checkMOTD.TabIndex = 1;
            this.checkMOTD.Text = "Show MOTD on connect";
            this.checkMOTD.UseVisualStyleBackColor = true;
            // 
            // checkModeI
            // 
            this.checkModeI.AutoSize = true;
            this.checkModeI.Checked = true;
            this.checkModeI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkModeI.Location = new System.Drawing.Point(12, 10);
            this.checkModeI.Margin = new System.Windows.Forms.Padding(2);
            this.checkModeI.Name = "checkModeI";
            this.checkModeI.Size = new System.Drawing.Size(187, 20);
            this.checkModeI.TabIndex = 0;
            this.checkModeI.Text = "Set mode +i on connect";
            this.checkModeI.UseVisualStyleBackColor = true;
            // 
            // tabPageIRCV3
            // 
            this.tabPageIRCV3.Controls.Add(this.checkEchoMessage);
            this.tabPageIRCV3.Controls.Add(this.checkChgHost);
            this.tabPageIRCV3.Controls.Add(this.checkAccountNotify);
            this.tabPageIRCV3.Controls.Add(this.checkAwayNotify);
            this.tabPageIRCV3.Controls.Add(this.checkExtendedJoin);
            this.tabPageIRCV3.Controls.Add(this.label9);
            this.tabPageIRCV3.Controls.Add(this.textSASLUser);
            this.tabPageIRCV3.Controls.Add(this.textSASLPass);
            this.tabPageIRCV3.Controls.Add(this.label10);
            this.tabPageIRCV3.Controls.Add(this.checkUseSASL);
            this.tabPageIRCV3.Location = new System.Drawing.Point(4, 46);
            this.tabPageIRCV3.Name = "tabPageIRCV3";
            this.tabPageIRCV3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageIRCV3.Size = new System.Drawing.Size(673, 248);
            this.tabPageIRCV3.TabIndex = 10;
            this.tabPageIRCV3.Text = "SASL / IRCv3";
            this.tabPageIRCV3.UseVisualStyleBackColor = true;
            // 
            // checkEchoMessage
            // 
            this.checkEchoMessage.AutoSize = true;
            this.checkEchoMessage.Location = new System.Drawing.Point(12, 130);
            this.checkEchoMessage.Margin = new System.Windows.Forms.Padding(2);
            this.checkEchoMessage.Name = "checkEchoMessage";
            this.checkEchoMessage.Size = new System.Drawing.Size(121, 20);
            this.checkEchoMessage.TabIndex = 81;
            this.checkEchoMessage.Text = "Echo Message";
            this.checkEchoMessage.UseVisualStyleBackColor = true;
            // 
            // checkChgHost
            // 
            this.checkChgHost.AutoSize = true;
            this.checkChgHost.Location = new System.Drawing.Point(165, 106);
            this.checkChgHost.Margin = new System.Windows.Forms.Padding(2);
            this.checkChgHost.Name = "checkChgHost";
            this.checkChgHost.Size = new System.Drawing.Size(111, 20);
            this.checkChgHost.TabIndex = 80;
            this.checkChgHost.Text = "Change Host";
            this.checkChgHost.UseVisualStyleBackColor = true;
            // 
            // checkAccountNotify
            // 
            this.checkAccountNotify.AutoSize = true;
            this.checkAccountNotify.Location = new System.Drawing.Point(165, 82);
            this.checkAccountNotify.Margin = new System.Windows.Forms.Padding(2);
            this.checkAccountNotify.Name = "checkAccountNotify";
            this.checkAccountNotify.Size = new System.Drawing.Size(126, 20);
            this.checkAccountNotify.TabIndex = 79;
            this.checkAccountNotify.Text = "Account Notify";
            this.checkAccountNotify.UseVisualStyleBackColor = true;
            // 
            // checkAwayNotify
            // 
            this.checkAwayNotify.AutoSize = true;
            this.checkAwayNotify.Location = new System.Drawing.Point(12, 106);
            this.checkAwayNotify.Margin = new System.Windows.Forms.Padding(2);
            this.checkAwayNotify.Name = "checkAwayNotify";
            this.checkAwayNotify.Size = new System.Drawing.Size(107, 20);
            this.checkAwayNotify.TabIndex = 78;
            this.checkAwayNotify.Text = "Away Notify";
            this.checkAwayNotify.UseVisualStyleBackColor = true;
            // 
            // checkExtendedJoin
            // 
            this.checkExtendedJoin.AutoSize = true;
            this.checkExtendedJoin.Location = new System.Drawing.Point(12, 82);
            this.checkExtendedJoin.Margin = new System.Windows.Forms.Padding(2);
            this.checkExtendedJoin.Name = "checkExtendedJoin";
            this.checkExtendedJoin.Size = new System.Drawing.Size(118, 20);
            this.checkExtendedJoin.TabIndex = 77;
            this.checkExtendedJoin.Text = "Extended Join";
            this.checkExtendedJoin.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 34);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 16);
            this.label9.TabIndex = 74;
            this.label9.Text = "SASL user";
            // 
            // textSASLUser
            // 
            this.textSASLUser.Location = new System.Drawing.Point(165, 32);
            this.textSASLUser.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textSASLUser.Name = "textSASLUser";
            this.textSASLUser.Size = new System.Drawing.Size(115, 23);
            this.textSASLUser.TabIndex = 72;
            // 
            // textSASLPass
            // 
            this.textSASLPass.Location = new System.Drawing.Point(165, 57);
            this.textSASLPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textSASLPass.Name = "textSASLPass";
            this.textSASLPass.PasswordChar = '*';
            this.textSASLPass.Size = new System.Drawing.Size(115, 23);
            this.textSASLPass.TabIndex = 73;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 58);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 16);
            this.label10.TabIndex = 75;
            this.label10.Text = "SASL password";
            // 
            // checkUseSASL
            // 
            this.checkUseSASL.AutoSize = true;
            this.checkUseSASL.Location = new System.Drawing.Point(12, 10);
            this.checkUseSASL.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseSASL.Name = "checkUseSASL";
            this.checkUseSASL.Size = new System.Drawing.Size(154, 20);
            this.checkUseSASL.TabIndex = 71;
            this.checkUseSASL.Text = "Connect with SASL";
            this.checkUseSASL.UseVisualStyleBackColor = true;
            // 
            // tabPageAutoJoin
            // 
            this.tabPageAutoJoin.BackColor = System.Drawing.Color.Transparent;
            this.tabPageAutoJoin.Controls.Add(this.buttonMoveDown);
            this.tabPageAutoJoin.Controls.Add(this.buttonMoveUp);
            this.tabPageAutoJoin.Controls.Add(this.panel1);
            this.tabPageAutoJoin.Controls.Add(this.buttonEditAutoJoin);
            this.tabPageAutoJoin.Controls.Add(this.listChannel);
            this.tabPageAutoJoin.Controls.Add(this.buttonRemoveAutoJoin);
            this.tabPageAutoJoin.Controls.Add(this.buttonAddAutoJoin);
            this.tabPageAutoJoin.Controls.Add(this.textChannel);
            this.tabPageAutoJoin.Controls.Add(this.labelChannel);
            this.tabPageAutoJoin.ImageIndex = 2;
            this.tabPageAutoJoin.Location = new System.Drawing.Point(4, 46);
            this.tabPageAutoJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPageAutoJoin.Name = "tabPageAutoJoin";
            this.tabPageAutoJoin.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPageAutoJoin.Size = new System.Drawing.Size(673, 248);
            this.tabPageAutoJoin.TabIndex = 1;
            this.tabPageAutoJoin.Text = "AutoJoin";
            this.tabPageAutoJoin.UseVisualStyleBackColor = true;
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Location = new System.Drawing.Point(425, 130);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(95, 25);
            this.buttonMoveDown.TabIndex = 37;
            this.buttonMoveDown.Text = "Move Down";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.ButtonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Location = new System.Drawing.Point(425, 99);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(95, 25);
            this.buttonMoveUp.TabIndex = 36;
            this.buttonMoveUp.Text = "Move Up";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.ButtonMoveUp_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkAutoJoinDelayBetween);
            this.panel1.Controls.Add(this.checkAutoJoinDelay);
            this.panel1.Controls.Add(this.checkAutoJoin);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 223);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 22);
            this.panel1.TabIndex = 35;
            // 
            // checkAutoJoinDelayBetween
            // 
            this.checkAutoJoinDelayBetween.AutoSize = true;
            this.checkAutoJoinDelayBetween.Location = new System.Drawing.Point(420, 0);
            this.checkAutoJoinDelayBetween.Margin = new System.Windows.Forms.Padding(2);
            this.checkAutoJoinDelayBetween.Name = "checkAutoJoinDelayBetween";
            this.checkAutoJoinDelayBetween.Size = new System.Drawing.Size(213, 20);
            this.checkAutoJoinDelayBetween.TabIndex = 36;
            this.checkAutoJoinDelayBetween.Text = "Delay between joins (1 sec)";
            this.checkAutoJoinDelayBetween.UseVisualStyleBackColor = true;
            // 
            // checkAutoJoinDelay
            // 
            this.checkAutoJoinDelay.AutoSize = true;
            this.checkAutoJoinDelay.Location = new System.Drawing.Point(180, 1);
            this.checkAutoJoinDelay.Margin = new System.Windows.Forms.Padding(2);
            this.checkAutoJoinDelay.Name = "checkAutoJoinDelay";
            this.checkAutoJoinDelay.Size = new System.Drawing.Size(183, 20);
            this.checkAutoJoinDelay.TabIndex = 35;
            this.checkAutoJoinDelay.Text = "AutoJoin delay (5 secs)";
            this.checkAutoJoinDelay.UseVisualStyleBackColor = true;
            // 
            // checkAutoJoin
            // 
            this.checkAutoJoin.AutoSize = true;
            this.checkAutoJoin.Location = new System.Drawing.Point(6, 2);
            this.checkAutoJoin.Margin = new System.Windows.Forms.Padding(2);
            this.checkAutoJoin.Name = "checkAutoJoin";
            this.checkAutoJoin.Size = new System.Drawing.Size(131, 20);
            this.checkAutoJoin.TabIndex = 34;
            this.checkAutoJoin.Text = "Enable AutoJoin";
            this.checkAutoJoin.UseVisualStyleBackColor = true;
            // 
            // buttonEditAutoJoin
            // 
            this.buttonEditAutoJoin.Location = new System.Drawing.Point(425, 68);
            this.buttonEditAutoJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonEditAutoJoin.Name = "buttonEditAutoJoin";
            this.buttonEditAutoJoin.Size = new System.Drawing.Size(70, 25);
            this.buttonEditAutoJoin.TabIndex = 31;
            this.buttonEditAutoJoin.Text = "Edit";
            this.buttonEditAutoJoin.UseVisualStyleBackColor = true;
            this.buttonEditAutoJoin.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // listChannel
            // 
            this.listChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listChannel.CheckBoxes = true;
            this.listChannel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listChannel.FullRowSelect = true;
            this.listChannel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listChannel.Location = new System.Drawing.Point(5, 31);
            this.listChannel.Margin = new System.Windows.Forms.Padding(2);
            this.listChannel.MultiSelect = false;
            this.listChannel.Name = "listChannel";
            this.listChannel.Size = new System.Drawing.Size(370, 180);
            this.listChannel.TabIndex = 32;
            this.listChannel.UseCompatibleStateImageBehavior = false;
            this.listChannel.View = System.Windows.Forms.View.Details;
            this.listChannel.DoubleClick += new System.EventHandler(this.ListChannel_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Channel";
            this.columnHeader1.Width = 246;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ChannelKey";
            this.columnHeader2.Width = 115;
            // 
            // buttonRemoveAutoJoin
            // 
            this.buttonRemoveAutoJoin.Location = new System.Drawing.Point(425, 37);
            this.buttonRemoveAutoJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonRemoveAutoJoin.Name = "buttonRemoveAutoJoin";
            this.buttonRemoveAutoJoin.Size = new System.Drawing.Size(70, 25);
            this.buttonRemoveAutoJoin.TabIndex = 30;
            this.buttonRemoveAutoJoin.Text = "Remove";
            this.buttonRemoveAutoJoin.UseVisualStyleBackColor = true;
            this.buttonRemoveAutoJoin.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // buttonAddAutoJoin
            // 
            this.buttonAddAutoJoin.Location = new System.Drawing.Point(425, 6);
            this.buttonAddAutoJoin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonAddAutoJoin.Name = "buttonAddAutoJoin";
            this.buttonAddAutoJoin.Size = new System.Drawing.Size(70, 25);
            this.buttonAddAutoJoin.TabIndex = 29;
            this.buttonAddAutoJoin.Text = "Add";
            this.buttonAddAutoJoin.UseVisualStyleBackColor = true;
            this.buttonAddAutoJoin.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // textChannel
            // 
            this.textChannel.Location = new System.Drawing.Point(74, 5);
            this.textChannel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textChannel.Name = "textChannel";
            this.textChannel.Size = new System.Drawing.Size(301, 23);
            this.textChannel.TabIndex = 26;
            this.textChannel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextChannel_KeyDown);
            // 
            // labelChannel
            // 
            this.labelChannel.AutoSize = true;
            this.labelChannel.Location = new System.Drawing.Point(6, 7);
            this.labelChannel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelChannel.Name = "labelChannel";
            this.labelChannel.Size = new System.Drawing.Size(60, 16);
            this.labelChannel.TabIndex = 27;
            this.labelChannel.Text = "Channel";
            // 
            // tabPageAutoPerform
            // 
            this.tabPageAutoPerform.Controls.Add(this.textAutoPerform);
            this.tabPageAutoPerform.Controls.Add(this.checkAutoPerform);
            this.tabPageAutoPerform.ImageIndex = 3;
            this.tabPageAutoPerform.Location = new System.Drawing.Point(4, 46);
            this.tabPageAutoPerform.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPageAutoPerform.Name = "tabPageAutoPerform";
            this.tabPageAutoPerform.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPageAutoPerform.Size = new System.Drawing.Size(673, 248);
            this.tabPageAutoPerform.TabIndex = 2;
            this.tabPageAutoPerform.Text = "AutoPerform";
            this.tabPageAutoPerform.UseVisualStyleBackColor = true;
            // 
            // textAutoPerform
            // 
            this.textAutoPerform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textAutoPerform.Location = new System.Drawing.Point(2, 3);
            this.textAutoPerform.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textAutoPerform.Multiline = true;
            this.textAutoPerform.Name = "textAutoPerform";
            this.textAutoPerform.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textAutoPerform.Size = new System.Drawing.Size(669, 222);
            this.textAutoPerform.TabIndex = 28;
            // 
            // checkAutoPerform
            // 
            this.checkAutoPerform.AutoSize = true;
            this.checkAutoPerform.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkAutoPerform.Location = new System.Drawing.Point(2, 225);
            this.checkAutoPerform.Margin = new System.Windows.Forms.Padding(2);
            this.checkAutoPerform.Name = "checkAutoPerform";
            this.checkAutoPerform.Size = new System.Drawing.Size(669, 20);
            this.checkAutoPerform.TabIndex = 29;
            this.checkAutoPerform.Text = "Enable AutoPerform";
            this.checkAutoPerform.UseVisualStyleBackColor = true;
            // 
            // tabPageIgnore
            // 
            this.tabPageIgnore.Controls.Add(this.label14);
            this.tabPageIgnore.Controls.Add(this.buttonClearIgnores);
            this.tabPageIgnore.Controls.Add(this.labelIgnoreNote);
            this.tabPageIgnore.Controls.Add(this.listIgnore);
            this.tabPageIgnore.Controls.Add(this.checkIgnore);
            this.tabPageIgnore.Controls.Add(this.textIgnore);
            this.tabPageIgnore.Controls.Add(this.labelNickHost);
            this.tabPageIgnore.Controls.Add(this.buttonEditIgnore);
            this.tabPageIgnore.Controls.Add(this.buttonRemoveIgnore);
            this.tabPageIgnore.Controls.Add(this.buttonAddIgnore);
            this.tabPageIgnore.Location = new System.Drawing.Point(4, 46);
            this.tabPageIgnore.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageIgnore.Name = "tabPageIgnore";
            this.tabPageIgnore.Size = new System.Drawing.Size(673, 248);
            this.tabPageIgnore.TabIndex = 4;
            this.tabPageIgnore.Text = "Ignore List";
            this.tabPageIgnore.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(564, 31);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 124);
            this.label14.TabIndex = 44;
            this.label14.Text = "C = Channel P = Private N = Notice T = CTCP    I = Invite   D = DCC";
            // 
            // buttonClearIgnores
            // 
            this.buttonClearIgnores.Location = new System.Drawing.Point(440, 101);
            this.buttonClearIgnores.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonClearIgnores.Name = "buttonClearIgnores";
            this.buttonClearIgnores.Size = new System.Drawing.Size(70, 25);
            this.buttonClearIgnores.TabIndex = 43;
            this.buttonClearIgnores.Text = "Clear All";
            this.buttonClearIgnores.UseVisualStyleBackColor = true;
            this.buttonClearIgnores.Click += new System.EventHandler(this.ButtonClearIgnores_Click);
            // 
            // labelIgnoreNote
            // 
            this.labelIgnoreNote.Location = new System.Drawing.Point(437, 182);
            this.labelIgnoreNote.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelIgnoreNote.Name = "labelIgnoreNote";
            this.labelIgnoreNote.Size = new System.Drawing.Size(207, 38);
            this.labelIgnoreNote.TabIndex = 41;
            this.labelIgnoreNote.Text = "Note: For wildcards, use the . character for nick names";
            // 
            // listIgnore
            // 
            this.listIgnore.CheckBoxes = true;
            this.listIgnore.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader5,
            this.columnC,
            this.columnP,
            this.columnN,
            this.columnT,
            this.columnI,
            this.columnD});
            this.listIgnore.FullRowSelect = true;
            this.listIgnore.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listIgnore.Location = new System.Drawing.Point(5, 31);
            this.listIgnore.Margin = new System.Windows.Forms.Padding(2);
            this.listIgnore.Name = "listIgnore";
            this.listIgnore.Size = new System.Drawing.Size(413, 184);
            this.listIgnore.TabIndex = 41;
            this.listIgnore.UseCompatibleStateImageBehavior = false;
            this.listIgnore.View = System.Windows.Forms.View.Details;
            this.listIgnore.DoubleClick += new System.EventHandler(this.ListIgnore_DoubleClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Nick / Host";
            this.columnHeader3.Width = 255;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "IgnoreType";
            this.columnHeader5.Width = 0;
            // 
            // columnC
            // 
            this.columnC.Text = "C";
            this.columnC.Width = 25;
            // 
            // columnP
            // 
            this.columnP.Text = "P";
            this.columnP.Width = 25;
            // 
            // columnN
            // 
            this.columnN.Text = "N";
            this.columnN.Width = 25;
            // 
            // columnT
            // 
            this.columnT.Text = "T";
            this.columnT.Width = 25;
            // 
            // columnI
            // 
            this.columnI.Text = "I";
            this.columnI.Width = 25;
            // 
            // columnD
            // 
            this.columnD.Text = "D";
            this.columnD.Width = 25;
            // 
            // checkIgnore
            // 
            this.checkIgnore.AutoSize = true;
            this.checkIgnore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkIgnore.Location = new System.Drawing.Point(0, 228);
            this.checkIgnore.Margin = new System.Windows.Forms.Padding(2);
            this.checkIgnore.Name = "checkIgnore";
            this.checkIgnore.Size = new System.Drawing.Size(673, 20);
            this.checkIgnore.TabIndex = 42;
            this.checkIgnore.Text = "Enable Ignore list";
            this.checkIgnore.UseVisualStyleBackColor = true;
            // 
            // textIgnore
            // 
            this.textIgnore.Location = new System.Drawing.Point(86, 5);
            this.textIgnore.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textIgnore.Name = "textIgnore";
            this.textIgnore.Size = new System.Drawing.Size(332, 23);
            this.textIgnore.TabIndex = 37;
            this.textIgnore.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextIgnore_KeyDown);
            // 
            // labelNickHost
            // 
            this.labelNickHost.AutoSize = true;
            this.labelNickHost.Location = new System.Drawing.Point(6, 7);
            this.labelNickHost.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNickHost.Name = "labelNickHost";
            this.labelNickHost.Size = new System.Drawing.Size(71, 16);
            this.labelNickHost.TabIndex = 38;
            this.labelNickHost.Text = "Nick/Host";
            // 
            // buttonEditIgnore
            // 
            this.buttonEditIgnore.Location = new System.Drawing.Point(440, 70);
            this.buttonEditIgnore.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonEditIgnore.Name = "buttonEditIgnore";
            this.buttonEditIgnore.Size = new System.Drawing.Size(70, 25);
            this.buttonEditIgnore.TabIndex = 40;
            this.buttonEditIgnore.Text = "Edit";
            this.buttonEditIgnore.UseVisualStyleBackColor = true;
            this.buttonEditIgnore.Click += new System.EventHandler(this.ButtonEditIgnore_Click);
            // 
            // buttonRemoveIgnore
            // 
            this.buttonRemoveIgnore.Location = new System.Drawing.Point(440, 39);
            this.buttonRemoveIgnore.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonRemoveIgnore.Name = "buttonRemoveIgnore";
            this.buttonRemoveIgnore.Size = new System.Drawing.Size(70, 25);
            this.buttonRemoveIgnore.TabIndex = 39;
            this.buttonRemoveIgnore.Text = "Remove";
            this.buttonRemoveIgnore.UseVisualStyleBackColor = true;
            this.buttonRemoveIgnore.Click += new System.EventHandler(this.ButtonRemoveIgnore_Click);
            // 
            // buttonAddIgnore
            // 
            this.buttonAddIgnore.Location = new System.Drawing.Point(440, 7);
            this.buttonAddIgnore.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonAddIgnore.Name = "buttonAddIgnore";
            this.buttonAddIgnore.Size = new System.Drawing.Size(70, 25);
            this.buttonAddIgnore.TabIndex = 38;
            this.buttonAddIgnore.Text = "Add";
            this.buttonAddIgnore.UseVisualStyleBackColor = true;
            this.buttonAddIgnore.Click += new System.EventHandler(this.ButtonAddIgnore_Click);
            // 
            // tabBuddyList
            // 
            this.tabBuddyList.Controls.Add(this.listBuddyList);
            this.tabBuddyList.Controls.Add(this.checkBuddyList);
            this.tabBuddyList.Controls.Add(this.textBuddy);
            this.tabBuddyList.Controls.Add(this.label4);
            this.tabBuddyList.Controls.Add(this.buttonEditBuddy);
            this.tabBuddyList.Controls.Add(this.buttonRemoveBuddy);
            this.tabBuddyList.Controls.Add(this.buttonAddBuddy);
            this.tabBuddyList.Location = new System.Drawing.Point(4, 46);
            this.tabBuddyList.Margin = new System.Windows.Forms.Padding(2);
            this.tabBuddyList.Name = "tabBuddyList";
            this.tabBuddyList.Padding = new System.Windows.Forms.Padding(2);
            this.tabBuddyList.Size = new System.Drawing.Size(673, 248);
            this.tabBuddyList.TabIndex = 7;
            this.tabBuddyList.Text = "Buddy List";
            this.tabBuddyList.UseVisualStyleBackColor = true;
            // 
            // listBuddyList
            // 
            this.listBuddyList.CheckBoxes = true;
            this.listBuddyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this.listBuddyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listBuddyList.Location = new System.Drawing.Point(5, 31);
            this.listBuddyList.Margin = new System.Windows.Forms.Padding(2);
            this.listBuddyList.Name = "listBuddyList";
            this.listBuddyList.Size = new System.Drawing.Size(281, 184);
            this.listBuddyList.TabIndex = 47;
            this.listBuddyList.UseCompatibleStateImageBehavior = false;
            this.listBuddyList.View = System.Windows.Forms.View.Details;
            this.listBuddyList.DoubleClick += new System.EventHandler(this.ListBuddyList_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Nick";
            this.columnHeader4.Width = 339;
            // 
            // checkBuddyList
            // 
            this.checkBuddyList.AutoSize = true;
            this.checkBuddyList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBuddyList.Location = new System.Drawing.Point(2, 226);
            this.checkBuddyList.Margin = new System.Windows.Forms.Padding(2);
            this.checkBuddyList.Name = "checkBuddyList";
            this.checkBuddyList.Size = new System.Drawing.Size(669, 20);
            this.checkBuddyList.TabIndex = 46;
            this.checkBuddyList.Text = "Enable Buddy list";
            this.checkBuddyList.UseVisualStyleBackColor = true;
            // 
            // textBuddy
            // 
            this.textBuddy.Location = new System.Drawing.Point(47, 5);
            this.textBuddy.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBuddy.Name = "textBuddy";
            this.textBuddy.Size = new System.Drawing.Size(238, 23);
            this.textBuddy.TabIndex = 44;
            this.textBuddy.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBuddy_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 7);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 16);
            this.label4.TabIndex = 45;
            this.label4.Text = "Nick";
            // 
            // buttonEditBuddy
            // 
            this.buttonEditBuddy.Location = new System.Drawing.Point(351, 68);
            this.buttonEditBuddy.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonEditBuddy.Name = "buttonEditBuddy";
            this.buttonEditBuddy.Size = new System.Drawing.Size(70, 25);
            this.buttonEditBuddy.TabIndex = 43;
            this.buttonEditBuddy.Text = "Edit";
            this.buttonEditBuddy.UseVisualStyleBackColor = true;
            this.buttonEditBuddy.Click += new System.EventHandler(this.ButtonEditBuddy_Click);
            // 
            // buttonRemoveBuddy
            // 
            this.buttonRemoveBuddy.Location = new System.Drawing.Point(351, 37);
            this.buttonRemoveBuddy.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonRemoveBuddy.Name = "buttonRemoveBuddy";
            this.buttonRemoveBuddy.Size = new System.Drawing.Size(70, 25);
            this.buttonRemoveBuddy.TabIndex = 42;
            this.buttonRemoveBuddy.Text = "Remove";
            this.buttonRemoveBuddy.UseVisualStyleBackColor = true;
            this.buttonRemoveBuddy.Click += new System.EventHandler(this.ButtonRemoveBuddy_Click);
            // 
            // buttonAddBuddy
            // 
            this.buttonAddBuddy.Location = new System.Drawing.Point(351, 5);
            this.buttonAddBuddy.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonAddBuddy.Name = "buttonAddBuddy";
            this.buttonAddBuddy.Size = new System.Drawing.Size(70, 25);
            this.buttonAddBuddy.TabIndex = 41;
            this.buttonAddBuddy.Text = "Add";
            this.buttonAddBuddy.UseVisualStyleBackColor = true;
            this.buttonAddBuddy.Click += new System.EventHandler(this.ButtonAddBuddy_Click);
            // 
            // tabPageNotes
            // 
            this.tabPageNotes.Controls.Add(this.textNotes);
            this.tabPageNotes.Location = new System.Drawing.Point(4, 46);
            this.tabPageNotes.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageNotes.Name = "tabPageNotes";
            this.tabPageNotes.Size = new System.Drawing.Size(673, 248);
            this.tabPageNotes.TabIndex = 8;
            this.tabPageNotes.Text = "Notes";
            this.tabPageNotes.UseVisualStyleBackColor = true;
            // 
            // textNotes
            // 
            this.textNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textNotes.Location = new System.Drawing.Point(0, 0);
            this.textNotes.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textNotes.Multiline = true;
            this.textNotes.Name = "textNotes";
            this.textNotes.Size = new System.Drawing.Size(673, 248);
            this.textNotes.TabIndex = 29;
            // 
            // tabPageProxy
            // 
            this.tabPageProxy.Controls.Add(this.textProxyPass);
            this.tabPageProxy.Controls.Add(this.labelProxyPass);
            this.tabPageProxy.Controls.Add(this.textProxyUser);
            this.tabPageProxy.Controls.Add(this.labelProxyUser);
            this.tabPageProxy.Controls.Add(this.radioSocksHTTP);
            this.tabPageProxy.Controls.Add(this.radioSocks4);
            this.tabPageProxy.Controls.Add(this.radioSocks5);
            this.tabPageProxy.Controls.Add(this.textProxyPort);
            this.tabPageProxy.Controls.Add(this.labelProxyPort);
            this.tabPageProxy.Controls.Add(this.textProxyIP);
            this.tabPageProxy.Controls.Add(this.labelProxyIP);
            this.tabPageProxy.Controls.Add(this.checkUseProxy);
            this.tabPageProxy.Location = new System.Drawing.Point(4, 46);
            this.tabPageProxy.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageProxy.Name = "tabPageProxy";
            this.tabPageProxy.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageProxy.Size = new System.Drawing.Size(673, 248);
            this.tabPageProxy.TabIndex = 6;
            this.tabPageProxy.Text = "Proxy Settings";
            this.tabPageProxy.UseVisualStyleBackColor = true;
            // 
            // textProxyPass
            // 
            this.textProxyPass.Location = new System.Drawing.Point(141, 121);
            this.textProxyPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textProxyPass.Name = "textProxyPass";
            this.textProxyPass.PasswordChar = '*';
            this.textProxyPass.Size = new System.Drawing.Size(255, 23);
            this.textProxyPass.TabIndex = 56;
            // 
            // labelProxyPass
            // 
            this.labelProxyPass.AutoSize = true;
            this.labelProxyPass.Location = new System.Drawing.Point(10, 124);
            this.labelProxyPass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProxyPass.Name = "labelProxyPass";
            this.labelProxyPass.Size = new System.Drawing.Size(111, 16);
            this.labelProxyPass.TabIndex = 57;
            this.labelProxyPass.Text = "Proxy password";
            // 
            // textProxyUser
            // 
            this.textProxyUser.Location = new System.Drawing.Point(141, 96);
            this.textProxyUser.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textProxyUser.Name = "textProxyUser";
            this.textProxyUser.Size = new System.Drawing.Size(255, 23);
            this.textProxyUser.TabIndex = 54;
            // 
            // labelProxyUser
            // 
            this.labelProxyUser.AutoSize = true;
            this.labelProxyUser.Location = new System.Drawing.Point(10, 99);
            this.labelProxyUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProxyUser.Name = "labelProxyUser";
            this.labelProxyUser.Size = new System.Drawing.Size(77, 16);
            this.labelProxyUser.TabIndex = 55;
            this.labelProxyUser.Text = "Proxy user";
            // 
            // radioSocksHTTP
            // 
            this.radioSocksHTTP.AutoSize = true;
            this.radioSocksHTTP.Location = new System.Drawing.Point(10, 198);
            this.radioSocksHTTP.Margin = new System.Windows.Forms.Padding(2);
            this.radioSocksHTTP.Name = "radioSocksHTTP";
            this.radioSocksHTTP.Size = new System.Drawing.Size(61, 20);
            this.radioSocksHTTP.TabIndex = 53;
            this.radioSocksHTTP.TabStop = true;
            this.radioSocksHTTP.Text = "HTTP";
            this.radioSocksHTTP.UseVisualStyleBackColor = true;
            // 
            // radioSocks4
            // 
            this.radioSocks4.AutoSize = true;
            this.radioSocks4.Location = new System.Drawing.Point(10, 177);
            this.radioSocks4.Margin = new System.Windows.Forms.Padding(2);
            this.radioSocks4.Name = "radioSocks4";
            this.radioSocks4.Size = new System.Drawing.Size(86, 20);
            this.radioSocks4.TabIndex = 52;
            this.radioSocks4.TabStop = true;
            this.radioSocks4.Text = "Socks v4";
            this.radioSocks4.UseVisualStyleBackColor = true;
            // 
            // radioSocks5
            // 
            this.radioSocks5.AutoSize = true;
            this.radioSocks5.Location = new System.Drawing.Point(10, 156);
            this.radioSocks5.Margin = new System.Windows.Forms.Padding(2);
            this.radioSocks5.Name = "radioSocks5";
            this.radioSocks5.Size = new System.Drawing.Size(251, 20);
            this.radioSocks5.TabIndex = 51;
            this.radioSocks5.TabStop = true;
            this.radioSocks5.Text = "Socks v5 (commonly used by Tor)";
            this.radioSocks5.UseVisualStyleBackColor = true;
            // 
            // textProxyPort
            // 
            this.textProxyPort.Location = new System.Drawing.Point(141, 60);
            this.textProxyPort.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textProxyPort.Name = "textProxyPort";
            this.textProxyPort.Size = new System.Drawing.Size(51, 23);
            this.textProxyPort.TabIndex = 49;
            // 
            // labelProxyPort
            // 
            this.labelProxyPort.AutoSize = true;
            this.labelProxyPort.Location = new System.Drawing.Point(10, 62);
            this.labelProxyPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProxyPort.Name = "labelProxyPort";
            this.labelProxyPort.Size = new System.Drawing.Size(76, 16);
            this.labelProxyPort.TabIndex = 50;
            this.labelProxyPort.Text = "Proxy port";
            // 
            // textProxyIP
            // 
            this.textProxyIP.Location = new System.Drawing.Point(141, 34);
            this.textProxyIP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textProxyIP.Name = "textProxyIP";
            this.textProxyIP.Size = new System.Drawing.Size(255, 23);
            this.textProxyIP.TabIndex = 47;
            // 
            // labelProxyIP
            // 
            this.labelProxyIP.AutoSize = true;
            this.labelProxyIP.Location = new System.Drawing.Point(10, 36);
            this.labelProxyIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProxyIP.Name = "labelProxyIP";
            this.labelProxyIP.Size = new System.Drawing.Size(108, 16);
            this.labelProxyIP.TabIndex = 48;
            this.labelProxyIP.Text = "Proxy server IP";
            // 
            // checkUseProxy
            // 
            this.checkUseProxy.AutoSize = true;
            this.checkUseProxy.Location = new System.Drawing.Point(10, 9);
            this.checkUseProxy.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseProxy.Name = "checkUseProxy";
            this.checkUseProxy.Size = new System.Drawing.Size(190, 20);
            this.checkUseProxy.TabIndex = 1;
            this.checkUseProxy.Text = "Enable Proxy Connection";
            this.checkUseProxy.UseVisualStyleBackColor = true;
            // 
            // tabPageBNC
            // 
            this.tabPageBNC.Controls.Add(this.textBNCPass);
            this.tabPageBNC.Controls.Add(this.labelBNCPass);
            this.tabPageBNC.Controls.Add(this.textBNCUser);
            this.tabPageBNC.Controls.Add(this.labelBNCUser);
            this.tabPageBNC.Controls.Add(this.textBNCPort);
            this.tabPageBNC.Controls.Add(this.labelBNCPort);
            this.tabPageBNC.Controls.Add(this.textBNCIP);
            this.tabPageBNC.Controls.Add(this.labelBNCIP);
            this.tabPageBNC.Controls.Add(this.checkUseBNC);
            this.tabPageBNC.Location = new System.Drawing.Point(4, 46);
            this.tabPageBNC.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageBNC.Name = "tabPageBNC";
            this.tabPageBNC.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageBNC.Size = new System.Drawing.Size(673, 248);
            this.tabPageBNC.TabIndex = 9;
            this.tabPageBNC.Text = "BNC Settings";
            this.tabPageBNC.UseVisualStyleBackColor = true;
            // 
            // textBNCPass
            // 
            this.textBNCPass.Location = new System.Drawing.Point(141, 121);
            this.textBNCPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBNCPass.Name = "textBNCPass";
            this.textBNCPass.PasswordChar = '*';
            this.textBNCPass.Size = new System.Drawing.Size(256, 23);
            this.textBNCPass.TabIndex = 64;
            // 
            // labelBNCPass
            // 
            this.labelBNCPass.AutoSize = true;
            this.labelBNCPass.Location = new System.Drawing.Point(10, 123);
            this.labelBNCPass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBNCPass.Name = "labelBNCPass";
            this.labelBNCPass.Size = new System.Drawing.Size(101, 16);
            this.labelBNCPass.TabIndex = 65;
            this.labelBNCPass.Text = "BNC password";
            // 
            // textBNCUser
            // 
            this.textBNCUser.Location = new System.Drawing.Point(141, 96);
            this.textBNCUser.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBNCUser.Name = "textBNCUser";
            this.textBNCUser.Size = new System.Drawing.Size(256, 23);
            this.textBNCUser.TabIndex = 62;
            // 
            // labelBNCUser
            // 
            this.labelBNCUser.AutoSize = true;
            this.labelBNCUser.Location = new System.Drawing.Point(10, 99);
            this.labelBNCUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBNCUser.Name = "labelBNCUser";
            this.labelBNCUser.Size = new System.Drawing.Size(67, 16);
            this.labelBNCUser.TabIndex = 63;
            this.labelBNCUser.Text = "BNC user";
            // 
            // textBNCPort
            // 
            this.textBNCPort.Location = new System.Drawing.Point(141, 60);
            this.textBNCPort.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBNCPort.Name = "textBNCPort";
            this.textBNCPort.Size = new System.Drawing.Size(51, 23);
            this.textBNCPort.TabIndex = 60;
            // 
            // labelBNCPort
            // 
            this.labelBNCPort.AutoSize = true;
            this.labelBNCPort.Location = new System.Drawing.Point(10, 62);
            this.labelBNCPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBNCPort.Name = "labelBNCPort";
            this.labelBNCPort.Size = new System.Drawing.Size(66, 16);
            this.labelBNCPort.TabIndex = 61;
            this.labelBNCPort.Text = "BNC port";
            // 
            // textBNCIP
            // 
            this.textBNCIP.Location = new System.Drawing.Point(141, 34);
            this.textBNCIP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBNCIP.Name = "textBNCIP";
            this.textBNCIP.Size = new System.Drawing.Size(256, 23);
            this.textBNCIP.TabIndex = 58;
            // 
            // labelBNCIP
            // 
            this.labelBNCIP.AutoSize = true;
            this.labelBNCIP.Location = new System.Drawing.Point(10, 36);
            this.labelBNCIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelBNCIP.Name = "labelBNCIP";
            this.labelBNCIP.Size = new System.Drawing.Size(80, 16);
            this.labelBNCIP.TabIndex = 59;
            this.labelBNCIP.Text = "BNC server";
            // 
            // checkUseBNC
            // 
            this.checkUseBNC.AutoSize = true;
            this.checkUseBNC.Location = new System.Drawing.Point(10, 9);
            this.checkUseBNC.Margin = new System.Windows.Forms.Padding(2);
            this.checkUseBNC.Name = "checkUseBNC";
            this.checkUseBNC.Size = new System.Drawing.Size(179, 20);
            this.checkUseBNC.TabIndex = 2;
            this.checkUseBNC.Text = "Enable BNC connection";
            this.checkUseBNC.UseVisualStyleBackColor = true;
            // 
            // tabPageDefault
            // 
            this.tabPageDefault.Controls.Add(this.textDefaultQuitMessage);
            this.tabPageDefault.Controls.Add(this.label3);
            this.tabPageDefault.Controls.Add(this.textDefaultFullName);
            this.tabPageDefault.Controls.Add(this.label2);
            this.tabPageDefault.Controls.Add(this.textDefaultIdent);
            this.tabPageDefault.Controls.Add(this.label1);
            this.tabPageDefault.Controls.Add(this.checkServerReconnect);
            this.tabPageDefault.Controls.Add(this.checkIdentServer);
            this.tabPageDefault.Controls.Add(this.textDefaultNick);
            this.tabPageDefault.Controls.Add(this.labelDefaultNickName);
            this.tabPageDefault.Location = new System.Drawing.Point(4, 46);
            this.tabPageDefault.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageDefault.Name = "tabPageDefault";
            this.tabPageDefault.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageDefault.Size = new System.Drawing.Size(673, 248);
            this.tabPageDefault.TabIndex = 5;
            this.tabPageDefault.Text = "Default Server Settings";
            this.tabPageDefault.UseVisualStyleBackColor = true;
            // 
            // textDefaultQuitMessage
            // 
            this.textDefaultQuitMessage.Location = new System.Drawing.Point(117, 86);
            this.textDefaultQuitMessage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textDefaultQuitMessage.Name = "textDefaultQuitMessage";
            this.textDefaultQuitMessage.Size = new System.Drawing.Size(183, 23);
            this.textDefaultQuitMessage.TabIndex = 53;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(8, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "Quit message";
            // 
            // textDefaultFullName
            // 
            this.textDefaultFullName.Location = new System.Drawing.Point(117, 59);
            this.textDefaultFullName.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textDefaultFullName.Name = "textDefaultFullName";
            this.textDefaultFullName.Size = new System.Drawing.Size(183, 23);
            this.textDefaultFullName.TabIndex = 51;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 52;
            this.label2.Text = "Fullname";
            // 
            // textDefaultIdent
            // 
            this.textDefaultIdent.Location = new System.Drawing.Point(117, 32);
            this.textDefaultIdent.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textDefaultIdent.Name = "textDefaultIdent";
            this.textDefaultIdent.Size = new System.Drawing.Size(121, 23);
            this.textDefaultIdent.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "Ident name";
            // 
            // checkServerReconnect
            // 
            this.checkServerReconnect.AutoSize = true;
            this.checkServerReconnect.Location = new System.Drawing.Point(10, 144);
            this.checkServerReconnect.Margin = new System.Windows.Forms.Padding(2);
            this.checkServerReconnect.Name = "checkServerReconnect";
            this.checkServerReconnect.Size = new System.Drawing.Size(207, 20);
            this.checkServerReconnect.TabIndex = 48;
            this.checkServerReconnect.Text = "Reconnect servers on error";
            this.checkServerReconnect.UseVisualStyleBackColor = true;
            // 
            // checkIdentServer
            // 
            this.checkIdentServer.AutoSize = true;
            this.checkIdentServer.Location = new System.Drawing.Point(10, 121);
            this.checkIdentServer.Margin = new System.Windows.Forms.Padding(2);
            this.checkIdentServer.Name = "checkIdentServer";
            this.checkIdentServer.Size = new System.Drawing.Size(108, 20);
            this.checkIdentServer.TabIndex = 47;
            this.checkIdentServer.Text = "Ident server";
            this.checkIdentServer.UseVisualStyleBackColor = true;
            // 
            // textDefaultNick
            // 
            this.textDefaultNick.Location = new System.Drawing.Point(117, 6);
            this.textDefaultNick.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textDefaultNick.Name = "textDefaultNick";
            this.textDefaultNick.Size = new System.Drawing.Size(121, 23);
            this.textDefaultNick.TabIndex = 45;
            // 
            // labelDefaultNickName
            // 
            this.labelDefaultNickName.AutoSize = true;
            this.labelDefaultNickName.ForeColor = System.Drawing.Color.Black;
            this.labelDefaultNickName.Location = new System.Drawing.Point(10, 9);
            this.labelDefaultNickName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDefaultNickName.Name = "labelDefaultNickName";
            this.labelDefaultNickName.Size = new System.Drawing.Size(35, 16);
            this.labelDefaultNickName.TabIndex = 46;
            this.labelDefaultNickName.Text = "Nick";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(413, 73);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 20);
            this.textBox1.TabIndex = 56;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 57;
            this.label5.Text = "Proxy Password";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(413, 42);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(150, 20);
            this.textBox2.TabIndex = 54;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Proxy User";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(15, 161);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(54, 17);
            this.radioButton1.TabIndex = 53;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "HTTP";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(15, 135);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(70, 17);
            this.radioButton2.TabIndex = 52;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Socks v4";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(15, 109);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(70, 17);
            this.radioButton3.TabIndex = 51;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Socks v5";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(128, 73);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(150, 20);
            this.textBox3.TabIndex = 49;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Proxy Port";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(128, 42);
            this.textBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(150, 20);
            this.textBox4.TabIndex = 47;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 48;
            this.label8.Text = "Proxy Server IP";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 11);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(145, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Enable Proxy Connection";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonDuplicateServer
            // 
            this.buttonDuplicateServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDuplicateServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonDuplicateServer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDuplicateServer.Location = new System.Drawing.Point(224, 302);
            this.buttonDuplicateServer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDuplicateServer.Name = "buttonDuplicateServer";
            this.buttonDuplicateServer.Size = new System.Drawing.Size(114, 25);
            this.buttonDuplicateServer.TabIndex = 55;
            this.buttonDuplicateServer.Text = "Duplicate";
            this.buttonDuplicateServer.UseVisualStyleBackColor = true;
            this.buttonDuplicateServer.Click += new System.EventHandler(this.ButtonDuplicateServer_Click);
            // 
            // buttonRemoveServer
            // 
            this.buttonRemoveServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonRemoveServer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveServer.Location = new System.Drawing.Point(114, 302);
            this.buttonRemoveServer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonRemoveServer.Name = "buttonRemoveServer";
            this.buttonRemoveServer.Size = new System.Drawing.Size(100, 25);
            this.buttonRemoveServer.TabIndex = 37;
            this.buttonRemoveServer.Text = "Remove";
            this.buttonRemoveServer.UseVisualStyleBackColor = true;
            this.buttonRemoveServer.Click += new System.EventHandler(this.ButtonRemoveServer_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(469, 302);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 25);
            this.buttonCancel.TabIndex = 19;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSave.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(577, 302);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 25);
            this.buttonSave.TabIndex = 17;
            this.buttonSave.Text = "OK";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonHelp.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHelp.Location = new System.Drawing.Point(4, 302);
            this.buttonHelp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 25);
            this.buttonHelp.TabIndex = 56;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // FormServers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(681, 332);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonDuplicateServer);
            this.Controls.Add(this.buttonRemoveServer);
            this.Controls.Add(this.tabControlSettings);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormServers";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Server Editor";
            this.tabControlSettings.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageExtra.ResumeLayout(false);
            this.tabPageExtra.PerformLayout();
            this.tabPageIRCV3.ResumeLayout(false);
            this.tabPageIRCV3.PerformLayout();
            this.tabPageAutoJoin.ResumeLayout(false);
            this.tabPageAutoJoin.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageAutoPerform.ResumeLayout(false);
            this.tabPageAutoPerform.PerformLayout();
            this.tabPageIgnore.ResumeLayout(false);
            this.tabPageIgnore.PerformLayout();
            this.tabBuddyList.ResumeLayout(false);
            this.tabBuddyList.PerformLayout();
            this.tabPageNotes.ResumeLayout(false);
            this.tabPageNotes.PerformLayout();
            this.tabPageProxy.ResumeLayout(false);
            this.tabPageProxy.PerformLayout();
            this.tabPageBNC.ResumeLayout(false);
            this.tabPageBNC.PerformLayout();
            this.tabPageDefault.ResumeLayout(false);
            this.tabPageDefault.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TextBox textQuitMessage;
        private System.Windows.Forms.Label labelQuitMessage;
        private System.Windows.Forms.Label labelFullName;
        private System.Windows.Forms.TextBox textFullName;
        private System.Windows.Forms.TextBox textIdentName;
        private System.Windows.Forms.Label labelIdentName;
        private System.Windows.Forms.TextBox textServerPort;
        private System.Windows.Forms.TextBox textServername;
        private System.Windows.Forms.TextBox textNickName;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.Label labelNickName;
        private System.Windows.Forms.TabPage tabPageAutoJoin;
        private System.Windows.Forms.TextBox textChannel;
        private System.Windows.Forms.Label labelChannel;
        private System.Windows.Forms.TabPage tabPageAutoPerform;
        private System.Windows.Forms.TextBox textAutoPerform;
        private System.Windows.Forms.Button buttonAddAutoJoin;
        private System.Windows.Forms.Button buttonRemoveAutoJoin;
        private System.Windows.Forms.TextBox textDisplayName;
        private System.Windows.Forms.Label labelDisplayName;
        private System.Windows.Forms.CheckBox checkAutoPerform;
        private System.Windows.Forms.ListView listChannel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabPage tabPageExtra;
        private System.Windows.Forms.CheckBox checkPingPong;
        private System.Windows.Forms.CheckBox checkMOTD;
        private System.Windows.Forms.CheckBox checkModeI;
        private System.Windows.Forms.TextBox textAltNickName;
        private System.Windows.Forms.Label labelAltNickName;
        private System.Windows.Forms.CheckBox checkRejoinChannel;
        private System.Windows.Forms.ComboBox comboEncoding;
        private System.Windows.Forms.Label labelEncoding;
        private System.Windows.Forms.Button buttonEditAutoJoin;
        private System.Windows.Forms.Button buttonRemoveServer;
        private System.Windows.Forms.TextBox textAwayNick;
        private System.Windows.Forms.Label labelAwayNickName;
        private System.Windows.Forms.CheckBox checkDisableCTCP;
        private System.Windows.Forms.TabPage tabPageIgnore;
        private System.Windows.Forms.ListView listIgnore;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckBox checkIgnore;
        private System.Windows.Forms.TextBox textIgnore;
        private System.Windows.Forms.Label labelNickHost;
        private System.Windows.Forms.Button buttonEditIgnore;
        private System.Windows.Forms.Button buttonRemoveIgnore;
        private System.Windows.Forms.Button buttonAddIgnore;
        private System.Windows.Forms.Label labelIgnoreNote;
        private System.Windows.Forms.CheckBox checkAutoStart;
        private System.Windows.Forms.TabPage tabPageDefault;
        private System.Windows.Forms.CheckBox checkServerReconnect;
        private System.Windows.Forms.CheckBox checkIdentServer;
        private System.Windows.Forms.TextBox textDefaultNick;
        private System.Windows.Forms.Label labelDefaultNickName;
        private System.Windows.Forms.TextBox textDefaultIdent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textDefaultQuitMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textDefaultFullName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPageProxy;
        private System.Windows.Forms.RadioButton radioSocksHTTP;
        private System.Windows.Forms.RadioButton radioSocks4;
        private System.Windows.Forms.RadioButton radioSocks5;
        private System.Windows.Forms.TextBox textProxyPort;
        private System.Windows.Forms.Label labelProxyPort;
        private System.Windows.Forms.TextBox textProxyIP;
        private System.Windows.Forms.Label labelProxyIP;
        private System.Windows.Forms.CheckBox checkUseProxy;
        private System.Windows.Forms.TextBox textProxyPass;
        private System.Windows.Forms.Label labelProxyPass;
        private System.Windows.Forms.TextBox textProxyUser;
        private System.Windows.Forms.Label labelProxyUser;
        private System.Windows.Forms.TabPage tabBuddyList;
        private System.Windows.Forms.ListView listBuddyList;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.CheckBox checkBuddyList;
        private System.Windows.Forms.TextBox textBuddy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonEditBuddy;
        private System.Windows.Forms.Button buttonRemoveBuddy;
        private System.Windows.Forms.Button buttonAddBuddy;
        private System.Windows.Forms.CheckBox checkUseIPv6;
        private System.Windows.Forms.TabPage tabPageNotes;
        private System.Windows.Forms.TextBox textNotes;
        private System.Windows.Forms.TabPage tabPageBNC;
        private System.Windows.Forms.TextBox textBNCPass;
        private System.Windows.Forms.Label labelBNCPass;
        private System.Windows.Forms.TextBox textBNCUser;
        private System.Windows.Forms.Label labelBNCUser;
        private System.Windows.Forms.TextBox textBNCPort;
        private System.Windows.Forms.Label labelBNCPort;
        private System.Windows.Forms.TextBox textBNCIP;
        private System.Windows.Forms.Label labelBNCIP;
        private System.Windows.Forms.CheckBox checkUseBNC;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkAutoJoinDelay;
        private System.Windows.Forms.CheckBox checkAutoJoin;
        private System.Windows.Forms.CheckBox checkAutoJoinDelayBetween;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textPingTimer;
        private System.Windows.Forms.CheckBox checkDisableAwayMessages;
        private System.Windows.Forms.CheckBox checkUseSSL;
        private System.Windows.Forms.CheckBox checkInvalidCertificate;
        private System.Windows.Forms.Button buttonDuplicateServer;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textReconnectTime;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Label labelServerPassword;
        private System.Windows.Forms.TextBox textServerPassword;
        private System.Windows.Forms.TextBox textNickservPassword;
        private System.Windows.Forms.Label labelNickservPassword;
        private System.Windows.Forms.CheckBox checkDisableLogging;
        private System.Windows.Forms.CheckBox checkDisableQueries;
        private System.Windows.Forms.Button buttonClearIgnores;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnC;
        private System.Windows.Forms.ColumnHeader columnP;
        private System.Windows.Forms.ColumnHeader columnN;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ColumnHeader columnT;
        private System.Windows.Forms.ColumnHeader columnI;
        private System.Windows.Forms.ColumnHeader columnD;
        private System.Windows.Forms.TabPage tabPageIRCV3;
        private System.Windows.Forms.CheckBox checkEchoMessage;
        private System.Windows.Forms.CheckBox checkChgHost;
        private System.Windows.Forms.CheckBox checkAccountNotify;
        private System.Windows.Forms.CheckBox checkAwayNotify;
        private System.Windows.Forms.CheckBox checkExtendedJoin;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textSASLUser;
        private System.Windows.Forms.TextBox textSASLPass;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkUseSASL;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.CheckBox checkNoColorMode;
    }
}