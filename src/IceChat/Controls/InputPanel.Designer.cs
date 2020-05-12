namespace IceChat
{
    partial class InputPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonEmoticonPicker = new System.Windows.Forms.Button();
            this.buttonColorPicker = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.contextHelpMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuNickName = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChannel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelWideText = new System.Windows.Forms.Panel();
            this.textBoxWide = new IceInputBox(this);
            this.panelSend = new System.Windows.Forms.Panel();

            this.panelSearch = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.searchText = new System.Windows.Forms.TextBox();

            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.textInput = new IceInputBox(this);
            this.contextHelpMenu.SuspendLayout();
            this.panelWideText.SuspendLayout();
            this.panelSend.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSearch.SuspendLayout();

            this.SuspendLayout();
            // 
            // buttonEmoticonPicker
            // 
            this.buttonEmoticonPicker.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonEmoticonPicker.Location = new System.Drawing.Point(0, 0);
            this.buttonEmoticonPicker.Name = "buttonEmoticonPicker";
            this.buttonEmoticonPicker.Size = new System.Drawing.Size(28, 94);
            this.buttonEmoticonPicker.TabIndex = 2;
            this.buttonEmoticonPicker.UseVisualStyleBackColor = true;
            this.buttonEmoticonPicker.Click += new System.EventHandler(this.ButtonEmoticonPicker_Click);
            // 
            // buttonColorPicker
            // 
            this.buttonColorPicker.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonColorPicker.Location = new System.Drawing.Point(28, 0);
            this.buttonColorPicker.Name = "buttonColorPicker";
            this.buttonColorPicker.Size = new System.Drawing.Size(28, 94);
            this.buttonColorPicker.TabIndex = 3;
            this.buttonColorPicker.UseVisualStyleBackColor = true;
            this.buttonColorPicker.Click += new System.EventHandler(this.ButtonColorPicker_Click);
            // 
            // buttonHelp
            // 
            this.buttonHelp.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonHelp.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelp.Location = new System.Drawing.Point(56, 0);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(28, 94);
            this.buttonHelp.TabIndex = 5;
            this.buttonHelp.Text = "?";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonHelp_MouseDown);
            // 
            // contextHelpMenu
            // 
            this.contextHelpMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.contextHelpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNickName,
            this.menuChannel,
            this.menuServer,
            this.toolStripMenuItem3});
            this.contextHelpMenu.Name = "contextMenuStrip1";
            this.contextHelpMenu.Size = new System.Drawing.Size(194, 92);
            // 
            // menuNickName
            // 
            this.menuNickName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5});
            this.menuNickName.Name = "menuNickName";
            this.menuNickName.Size = new System.Drawing.Size(193, 22);
            this.menuNickName.Text = "Nickname Commands";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(266, 22);
            this.toolStripMenuItem1.Tag = "/nick $?=\'Choose new Nick name\'";
            this.toolStripMenuItem1.Text = "Change Your Nick name";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(266, 22);
            this.toolStripMenuItem2.Tag = "/msg $?=\'Insert Nick name\' message";
            this.toolStripMenuItem2.Text = "Send a private message to some one";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(266, 22);
            this.toolStripMenuItem4.Tag = "/whois $?=\'Insert Nick name\'";
            this.toolStripMenuItem4.Text = "Perform a whois";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(266, 22);
            this.toolStripMenuItem5.Tag = "/ping $?=\'Insert Nick name\'";
            this.toolStripMenuItem5.Text = "Ping some one";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // menuChannel
            // 
            this.menuChannel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8});
            this.menuChannel.Name = "menuChannel";
            this.menuChannel.Size = new System.Drawing.Size(193, 22);
            this.menuChannel.Text = "Channel Commands";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem6.Tag = "/join $?=\'Insert Channel name\'";
            this.toolStripMenuItem6.Text = "Join a channel";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem7.Tag = "/part";
            this.toolStripMenuItem7.Text = "Leave a channel";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem8.Tag = "/me action";
            this.toolStripMenuItem8.Text = "Send an action";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // menuServer
            // 
            this.menuServer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.toolStripMenuItem10});
            this.menuServer.Name = "menuServer";
            this.menuServer.Size = new System.Drawing.Size(193, 22);
            this.menuServer.Text = "Server Commands";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(176, 22);
            this.toolStripMenuItem9.Tag = "/server $?=\'Insert Server name\'";
            this.toolStripMenuItem9.Text = "Connect to a server";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(176, 22);
            this.toolStripMenuItem10.Tag = "/quit";
            this.toolStripMenuItem10.Text = "Quit a server";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(193, 22);
            this.toolStripMenuItem3.Tag = "/help";
            this.toolStripMenuItem3.Text = "Get More Help";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripHelpMenuOnClick);
            // 
            // panelWideText
            // 
            this.panelWideText.Controls.Add(this.textBoxWide);
            this.panelWideText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWideText.Location = new System.Drawing.Point(84, 0);
            this.panelWideText.Name = "panelWideText";
            this.panelWideText.Size = new System.Drawing.Size(475, 94);
            this.panelWideText.TabIndex = 6;
            this.panelWideText.Visible = false;
            // 
            // textBoxWide
            // 
            this.textBoxWide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxWide.Location = new System.Drawing.Point(0, 0);
            this.textBoxWide.Multiline = true;
            this.textBoxWide.Name = "textBoxWide";
            //this.textBoxWide.MaxLength = 440;     // changed from 512
            this.textBoxWide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxWide.Size = new System.Drawing.Size(475, 94);
            this.textBoxWide.TabIndex = 0;
            // 
            // panelSend
            // 
            this.panelSend.Controls.Add(this.buttonSend);
            this.panelSend.Controls.Add(this.buttonReset);
            this.panelSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSend.Location = new System.Drawing.Point(559, 0);
            this.panelSend.Name = "panelSend";
            this.panelSend.Size = new System.Drawing.Size(72, 94);
            this.panelSend.TabIndex = 8;
            // 
            // buttonSend
            // 
            this.buttonSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSend.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSend.Location = new System.Drawing.Point(0, 0);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(72, 71);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.ButtonSend_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonReset.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonReset.Location = new System.Drawing.Point(0, 71);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(72, 23);
            this.buttonReset.TabIndex = 8;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // textInput
            // 
            this.textInput.AccessibleDescription = "Main input area";
            this.textInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textInput.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textInput.HideSelection = false;
            this.textInput.Location = new System.Drawing.Point(84, 0);
            this.textInput.MaxLength = 1024;
            this.textInput.Name = "textInput";
            this.textInput.Size = new System.Drawing.Size(475, 16);
            this.textInput.TabIndex = 0;
            // 
            // InputPanel
            // 
            // searchPanel
            // buttonsPanel
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Height = 20;
            this.panelSearch.BackColor = System.Drawing.Color.White;
            this.panelSearch.Visible = false;

            this.searchText.Text = "Search Text Here";
            this.searchText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchText.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.searchText.Margin = new System.Windows.Forms.Padding(0);            

            this.buttonSearch.Text = "Search";
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonSearch.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Size = new System.Drawing.Size(72, 71);
            this.buttonSearch.Visible = false;

            this.panelSearch.Controls.Add(searchText);
           // this.panelSearch.Controls.Add(buttonSearch);

            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Size = new System.Drawing.Size(631, 94);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.panelButtons.Controls.Add(this.textInput);
            this.panelButtons.Controls.Add(this.panelWideText);
            this.panelButtons.Controls.Add(this.panelSend);
            this.panelButtons.Controls.Add(this.buttonHelp);
            this.panelButtons.Controls.Add(this.buttonColorPicker);
            this.panelButtons.Controls.Add(this.buttonEmoticonPicker);

            this.Controls.Add(panelButtons);
            this.Controls.Add(panelSearch);

            this.Name = "InputPanel";

            this.contextHelpMenu.ResumeLayout(false);
            this.panelWideText.ResumeLayout(false);
            this.panelWideText.PerformLayout();
            this.panelSend.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);

            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private IceInputBox textInput;
        private System.Windows.Forms.Button buttonEmoticonPicker;
        private System.Windows.Forms.Button buttonColorPicker;
        private System.Windows.Forms.Button buttonHelp;
        private System.Windows.Forms.ContextMenuStrip contextHelpMenu;
        private System.Windows.Forms.ToolStripMenuItem menuNickName;
        private System.Windows.Forms.ToolStripMenuItem menuChannel;
        private System.Windows.Forms.ToolStripMenuItem menuServer;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.Panel panelWideText;

        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Panel panelButtons;

        private System.Windows.Forms.TextBox searchText;
        private IceInputBox textBoxWide;

        private System.Windows.Forms.Panel panelSend;

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSearch;
    }
}
