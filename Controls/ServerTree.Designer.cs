namespace IceChat
{
    partial class ServerTree
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

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.contextMenuChannel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reJoinChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToAutoJoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelFontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noColorModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.joinEventLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.partEventLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inChannelToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.inConsoleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.quitEventLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inChannelToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.inConsoleToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openChannelLogFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoPerformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceDisconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemBlank = new System.Windows.Forms.ToolStripSeparator();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.autoJoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLogFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuQuery = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.silenceUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsQueryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSoundsQueryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuDCCChat = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearWindowDCCChat = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWindowDCCChat = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectDCCChat = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuChannelList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeChannenListToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuWindow = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuDebug = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuClearDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuCloseDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.hideJoinPartQuitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelButtons.SuspendLayout();
            this.contextMenuChannel.SuspendLayout();
            this.contextMenuServer.SuspendLayout();
            this.contextMenuQuery.SuspendLayout();
            this.contextMenuDCCChat.SuspendLayout();
            this.contextMenuChannelList.SuspendLayout();
            this.contextMenuWindow.SuspendLayout();
            this.contextMenuDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelButtons.Controls.Add(this.buttonAdd);
            this.panelButtons.Controls.Add(this.buttonEdit);
            this.panelButtons.Controls.Add(this.buttonDisconnect);
            this.panelButtons.Controls.Add(this.buttonConnect);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 266);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(2);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(165, 57);
            this.panelButtons.TabIndex = 0;
            // 
            // buttonAdd
            // 
            this.buttonAdd.AccessibleDescription = "Add a new server";
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAdd.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.buttonAdd.Location = new System.Drawing.Point(86, 28);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(77, 24);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.TabStop = false;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.AccessibleDescription = "Edit selected server settings";
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEdit.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.buttonEdit.Location = new System.Drawing.Point(86, 2);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(77, 24);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.TabStop = false;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.AccessibleDescription = "Disconnect selected server";
            this.buttonDisconnect.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.buttonDisconnect.Location = new System.Drawing.Point(2, 28);
            this.buttonDisconnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(83, 24);
            this.buttonDisconnect.TabIndex = 2;
            this.buttonDisconnect.TabStop = false;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.AccessibleDescription = "Connect to selected server";
            this.buttonConnect.Font = new System.Drawing.Font("Verdana", 6.75F);
            this.buttonConnect.Location = new System.Drawing.Point(2, 2);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(83, 24);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.TabStop = false;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // vScrollBar
            // 
            this.vScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar.LargeChange = 2;
            this.vScrollBar.Location = new System.Drawing.Point(148, 23);
            this.vScrollBar.Maximum = 1;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.vScrollBar.Size = new System.Drawing.Size(15, 216);
            this.vScrollBar.TabIndex = 3;
            this.vScrollBar.Visible = false;
            // 
            // contextMenuChannel
            // 
            this.contextMenuChannel.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuChannel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearChannelToolStripMenuItem,
            this.closeChannelToolStripMenuItem,
            this.reJoinChannelToolStripMenuItem,
            this.addToAutoJoinToolStripMenuItem,
            this.channelInformationToolStripMenuItem,
            this.channelFontToolStripMenuItem,
            this.noColorModeToolStripMenuItem,
            this.eventsToolStripMenuItem,
            this.loggingToolStripMenuItem});
            this.contextMenuChannel.Name = "contextMenuChannel";
            this.contextMenuChannel.Size = new System.Drawing.Size(185, 224);
            // 
            // clearChannelToolStripMenuItem
            // 
            this.clearChannelToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.clearChannelToolStripMenuItem.Name = "clearChannelToolStripMenuItem";
            this.clearChannelToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.clearChannelToolStripMenuItem.Text = "Clear Window";
            this.clearChannelToolStripMenuItem.Click += new System.EventHandler(this.clearChannelToolStripMenuItem_Click);
            // 
            // closeChannelToolStripMenuItem
            // 
            this.closeChannelToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.closeChannelToolStripMenuItem.Name = "closeChannelToolStripMenuItem";
            this.closeChannelToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.closeChannelToolStripMenuItem.Text = "Close Channel";
            this.closeChannelToolStripMenuItem.Click += new System.EventHandler(this.closeChannelToolStripMenuItem_Click);
            // 
            // reJoinChannelToolStripMenuItem
            // 
            this.reJoinChannelToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.reJoinChannelToolStripMenuItem.Name = "reJoinChannelToolStripMenuItem";
            this.reJoinChannelToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.reJoinChannelToolStripMenuItem.Text = "Rejoin Channel";
            this.reJoinChannelToolStripMenuItem.Click += new System.EventHandler(this.reJoinChannelToolStripMenuItem_Click);
            // 
            // addToAutoJoinToolStripMenuItem
            // 
            this.addToAutoJoinToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.addToAutoJoinToolStripMenuItem.Name = "addToAutoJoinToolStripMenuItem";
            this.addToAutoJoinToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.addToAutoJoinToolStripMenuItem.Text = "Toggle AutoJoin";
            this.addToAutoJoinToolStripMenuItem.Click += new System.EventHandler(this.addToAutoJoinToolStripMenuItem_Click);
            // 
            // channelInformationToolStripMenuItem
            // 
            this.channelInformationToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.channelInformationToolStripMenuItem.Name = "channelInformationToolStripMenuItem";
            this.channelInformationToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.channelInformationToolStripMenuItem.Text = "Channel Information";
            this.channelInformationToolStripMenuItem.Click += new System.EventHandler(this.channelInformationToolStripMenuItem_Click);
            // 
            // channelFontToolStripMenuItem
            // 
            this.channelFontToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.channelFontToolStripMenuItem.Name = "channelFontToolStripMenuItem";
            this.channelFontToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.channelFontToolStripMenuItem.Text = "Channel Font";
            this.channelFontToolStripMenuItem.Click += new System.EventHandler(this.channelFontToolStripMenuItem_Click);
            // 
            // noColorModeToolStripMenuItem
            // 
            this.noColorModeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.noColorModeToolStripMenuItem.Name = "noColorModeToolStripMenuItem";
            this.noColorModeToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.noColorModeToolStripMenuItem.Text = "No Color Mode";
            this.noColorModeToolStripMenuItem.Click += new System.EventHandler(this.noColorModeToolStripMenuItem_Click);
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableEventsToolStripMenuItem,
            this.disableSoundsToolStripMenuItem,
            this.joinEventLocationToolStripMenuItem,
            this.partEventLocationToolStripMenuItem,
            this.quitEventLocationToolStripMenuItem,
            this.hideJoinPartQuitToolStripMenuItem});
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.eventsToolStripMenuItem.Text = "Events";
            // 
            // disableEventsToolStripMenuItem
            // 
            this.disableEventsToolStripMenuItem.Name = "disableEventsToolStripMenuItem";
            this.disableEventsToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.disableEventsToolStripMenuItem.Text = "Disable Flashing/Color Changes";
            this.disableEventsToolStripMenuItem.Click += new System.EventHandler(this.disableEventsToolStripMenuItem_Click);
            // 
            // disableSoundsToolStripMenuItem
            // 
            this.disableSoundsToolStripMenuItem.Name = "disableSoundsToolStripMenuItem";
            this.disableSoundsToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.disableSoundsToolStripMenuItem.Text = "Disable Sounds";
            this.disableSoundsToolStripMenuItem.Click += new System.EventHandler(this.disableSoundsToolStripMenuItem_Click);
            // 
            // joinEventLocationToolStripMenuItem
            // 
            this.joinEventLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inChannelToolStripMenuItem,
            this.inConsoleToolStripMenuItem,
            this.hideToolStripMenuItem});
            this.joinEventLocationToolStripMenuItem.Name = "joinEventLocationToolStripMenuItem";
            this.joinEventLocationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.joinEventLocationToolStripMenuItem.Text = "Join Event Location";
            // 
            // inChannelToolStripMenuItem
            // 
            this.inChannelToolStripMenuItem.Name = "inChannelToolStripMenuItem";
            this.inChannelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inChannelToolStripMenuItem.Text = "In Channel";
            this.inChannelToolStripMenuItem.Click += new System.EventHandler(this.inChannelToolStripMenuItem_Click);
            // 
            // inConsoleToolStripMenuItem
            // 
            this.inConsoleToolStripMenuItem.Name = "inConsoleToolStripMenuItem";
            this.inConsoleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.inConsoleToolStripMenuItem.Text = "In Console";
            this.inConsoleToolStripMenuItem.Click += new System.EventHandler(this.inConsoleToolStripMenuItem_Click);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // partEventLocationToolStripMenuItem
            // 
            this.partEventLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inChannelToolStripMenuItem1,
            this.inConsoleToolStripMenuItem1,
            this.hideToolStripMenuItem1});
            this.partEventLocationToolStripMenuItem.Name = "partEventLocationToolStripMenuItem";
            this.partEventLocationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.partEventLocationToolStripMenuItem.Text = "Part Event Location";
            // 
            // inChannelToolStripMenuItem1
            // 
            this.inChannelToolStripMenuItem1.Name = "inChannelToolStripMenuItem1";
            this.inChannelToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.inChannelToolStripMenuItem1.Text = "In Channel";
            this.inChannelToolStripMenuItem1.Click += new System.EventHandler(this.inChannelToolStripMenuItem1_Click);
            // 
            // inConsoleToolStripMenuItem1
            // 
            this.inConsoleToolStripMenuItem1.Name = "inConsoleToolStripMenuItem1";
            this.inConsoleToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.inConsoleToolStripMenuItem1.Text = "In Console";
            this.inConsoleToolStripMenuItem1.Click += new System.EventHandler(this.inConsoleToolStripMenuItem1_Click);
            // 
            // hideToolStripMenuItem1
            // 
            this.hideToolStripMenuItem1.Name = "hideToolStripMenuItem1";
            this.hideToolStripMenuItem1.Size = new System.Drawing.Size(131, 22);
            this.hideToolStripMenuItem1.Text = "Hide";
            this.hideToolStripMenuItem1.Click += new System.EventHandler(this.hideToolStripMenuItem1_Click);
            // 
            // quitEventLocationToolStripMenuItem
            // 
            this.quitEventLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inChannelToolStripMenuItem2,
            this.inConsoleToolStripMenuItem2,
            this.hideToolStripMenuItem2});
            this.quitEventLocationToolStripMenuItem.Name = "quitEventLocationToolStripMenuItem";
            this.quitEventLocationToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.quitEventLocationToolStripMenuItem.Text = "Quit Event Location";
            // 
            // inChannelToolStripMenuItem2
            // 
            this.inChannelToolStripMenuItem2.Name = "inChannelToolStripMenuItem2";
            this.inChannelToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.inChannelToolStripMenuItem2.Text = "In Channel";
            this.inChannelToolStripMenuItem2.Click += new System.EventHandler(this.inChannelToolStripMenuItem2_Click);
            // 
            // inConsoleToolStripMenuItem2
            // 
            this.inConsoleToolStripMenuItem2.Name = "inConsoleToolStripMenuItem2";
            this.inConsoleToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.inConsoleToolStripMenuItem2.Text = "In Console";
            this.inConsoleToolStripMenuItem2.Click += new System.EventHandler(this.inConsoleToolStripMenuItem2_Click);
            // 
            // hideToolStripMenuItem2
            // 
            this.hideToolStripMenuItem2.Name = "hideToolStripMenuItem2";
            this.hideToolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.hideToolStripMenuItem2.Text = "Hide";
            this.hideToolStripMenuItem2.Click += new System.EventHandler(this.hideToolStripMenuItem2_Click);
            // 
            // loggingToolStripMenuItem
            // 
            this.loggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openChannelLogFolderToolStripMenuItem,
            this.disableLoggingToolStripMenuItem});
            this.loggingToolStripMenuItem.Name = "loggingToolStripMenuItem";
            this.loggingToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.loggingToolStripMenuItem.Text = "Logging";
            // 
            // openChannelLogFolderToolStripMenuItem
            // 
            this.openChannelLogFolderToolStripMenuItem.Name = "openChannelLogFolderToolStripMenuItem";
            this.openChannelLogFolderToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.openChannelLogFolderToolStripMenuItem.Text = "Open Log Folder";
            this.openChannelLogFolderToolStripMenuItem.Click += new System.EventHandler(this.openChannelLogFolderToolStripMenuItem_Click);
            // 
            // disableLoggingToolStripMenuItem
            // 
            this.disableLoggingToolStripMenuItem.Name = "disableLoggingToolStripMenuItem";
            this.disableLoggingToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.disableLoggingToolStripMenuItem.Text = "Disable Logging";
            this.disableLoggingToolStripMenuItem.Click += new System.EventHandler(this.disableLoggingToolStripMenuItem_Click);
            // 
            // autoPerformToolStripMenuItem
            // 
            this.autoPerformToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.autoPerformToolStripMenuItem.Name = "autoPerformToolStripMenuItem";
            this.autoPerformToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.autoPerformToolStripMenuItem.Text = "Auto Perform";
            this.autoPerformToolStripMenuItem.Click += new System.EventHandler(this.autoPerformToolStripMenuItem_Click);
            // 
            // contextMenuServer
            // 
            this.contextMenuServer.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.forceDisconnectToolStripMenuItem,
            this.autoStartToolStripMenuItem,
            this.toolStripMenuItemBlank,
            this.editToolStripMenuItem,
            this.removeServerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.autoJoinToolStripMenuItem,
            this.autoPerformToolStripMenuItem,
            this.openLogFolderToolStripMenuItem});
            this.contextMenuServer.Name = "contextMenuServer";
            this.contextMenuServer.Size = new System.Drawing.Size(177, 214);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // forceDisconnectToolStripMenuItem
            // 
            this.forceDisconnectToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.forceDisconnectToolStripMenuItem.Name = "forceDisconnectToolStripMenuItem";
            this.forceDisconnectToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.forceDisconnectToolStripMenuItem.Text = "Force disconnect";
            this.forceDisconnectToolStripMenuItem.Click += new System.EventHandler(this.forceDisconnectToolStripMenuItem_Click);
            // 
            // autoStartToolStripMenuItem
            // 
            this.autoStartToolStripMenuItem.Name = "autoStartToolStripMenuItem";
            this.autoStartToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.autoStartToolStripMenuItem.Text = "Connect on startup";
            this.autoStartToolStripMenuItem.Click += new System.EventHandler(this.autoStartToolStripMenuItem_Click);
            // 
            // toolStripMenuItemBlank
            // 
            this.toolStripMenuItemBlank.Name = "toolStripMenuItemBlank";
            this.toolStripMenuItemBlank.Size = new System.Drawing.Size(173, 6);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.editToolStripMenuItem.Text = "Edit server";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeServerToolStripMenuItem
            // 
            this.removeServerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.removeServerToolStripMenuItem.Name = "removeServerToolStripMenuItem";
            this.removeServerToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.removeServerToolStripMenuItem.Text = "Remove server";
            this.removeServerToolStripMenuItem.Click += new System.EventHandler(this.removeServerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(173, 6);
            // 
            // autoJoinToolStripMenuItem
            // 
            this.autoJoinToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.autoJoinToolStripMenuItem.Name = "autoJoinToolStripMenuItem";
            this.autoJoinToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.autoJoinToolStripMenuItem.Text = "Auto Join";
            this.autoJoinToolStripMenuItem.Click += new System.EventHandler(this.autoJoinToolStripMenuItem_Click);
            // 
            // openLogFolderToolStripMenuItem
            // 
            this.openLogFolderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.openLogFolderToolStripMenuItem.Name = "openLogFolderToolStripMenuItem";
            this.openLogFolderToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.openLogFolderToolStripMenuItem.Text = "Open Log Folder";
            this.openLogFolderToolStripMenuItem.Click += new System.EventHandler(this.openLogFolderToolStripMenuItem_Click);
            // 
            // contextMenuQuery
            // 
            this.contextMenuQuery.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearQueryToolStripMenuItem,
            this.closeQueryToolStripMenuItem,
            this.userInformationToolStripMenuItem,
            this.silenceUserToolStripMenuItem,
            this.eventsQueryMenuItem});
            this.contextMenuQuery.Name = "contextMenuQuery";
            this.contextMenuQuery.Size = new System.Drawing.Size(164, 114);
            // 
            // clearQueryToolStripMenuItem
            // 
            this.clearQueryToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.clearQueryToolStripMenuItem.Name = "clearQueryToolStripMenuItem";
            this.clearQueryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.clearQueryToolStripMenuItem.Text = "Clear Query";
            this.clearQueryToolStripMenuItem.Click += new System.EventHandler(this.clearQueryToolStripMenuItem_Click);
            // 
            // closeQueryToolStripMenuItem
            // 
            this.closeQueryToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.closeQueryToolStripMenuItem.Name = "closeQueryToolStripMenuItem";
            this.closeQueryToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.closeQueryToolStripMenuItem.Text = "Close Query";
            this.closeQueryToolStripMenuItem.Click += new System.EventHandler(this.closeQueryToolStripMenuItem_Click);
            // 
            // userInformationToolStripMenuItem
            // 
            this.userInformationToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.userInformationToolStripMenuItem.Name = "userInformationToolStripMenuItem";
            this.userInformationToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.userInformationToolStripMenuItem.Text = "User Information";
            this.userInformationToolStripMenuItem.Click += new System.EventHandler(this.userInformationToolStripMenuItem_Click);
            // 
            // silenceUserToolStripMenuItem
            // 
            this.silenceUserToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.silenceUserToolStripMenuItem.Name = "silenceUserToolStripMenuItem";
            this.silenceUserToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.silenceUserToolStripMenuItem.Text = "Silence User";
            this.silenceUserToolStripMenuItem.Click += new System.EventHandler(this.silenceUserToolStripMenuItem_Click);
            // 
            // eventsQueryMenuItem
            // 
            this.eventsQueryMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableSoundsQueryMenuItem});
            this.eventsQueryMenuItem.Name = "eventsQueryMenuItem";
            this.eventsQueryMenuItem.Size = new System.Drawing.Size(163, 22);
            this.eventsQueryMenuItem.Text = "Events";
            // 
            // disableSoundsQueryMenuItem
            // 
            this.disableSoundsQueryMenuItem.Name = "disableSoundsQueryMenuItem";
            this.disableSoundsQueryMenuItem.Size = new System.Drawing.Size(154, 22);
            this.disableSoundsQueryMenuItem.Text = "Disable Sounds";
            this.disableSoundsQueryMenuItem.Click += new System.EventHandler(this.disableSoundsQueryMenuItem_Click);
            // 
            // contextMenuDCCChat
            // 
            this.contextMenuDCCChat.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuDCCChat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearWindowDCCChat,
            this.closeWindowDCCChat,
            this.disconnectDCCChat});
            this.contextMenuDCCChat.Name = "contextMenuDCCChat";
            this.contextMenuDCCChat.Size = new System.Drawing.Size(151, 70);
            // 
            // clearWindowDCCChat
            // 
            this.clearWindowDCCChat.ForeColor = System.Drawing.SystemColors.MenuText;
            this.clearWindowDCCChat.Name = "clearWindowDCCChat";
            this.clearWindowDCCChat.Size = new System.Drawing.Size(150, 22);
            this.clearWindowDCCChat.Text = "Clear window";
            this.clearWindowDCCChat.Click += new System.EventHandler(this.clearWindowDCCChat_Click);
            // 
            // closeWindowDCCChat
            // 
            this.closeWindowDCCChat.ForeColor = System.Drawing.SystemColors.MenuText;
            this.closeWindowDCCChat.Name = "closeWindowDCCChat";
            this.closeWindowDCCChat.Size = new System.Drawing.Size(150, 22);
            this.closeWindowDCCChat.Text = "Close Window";
            this.closeWindowDCCChat.Click += new System.EventHandler(this.closeWindowDCCChat_Click);
            // 
            // disconnectDCCChat
            // 
            this.disconnectDCCChat.ForeColor = System.Drawing.SystemColors.MenuText;
            this.disconnectDCCChat.Name = "disconnectDCCChat";
            this.disconnectDCCChat.Size = new System.Drawing.Size(150, 22);
            this.disconnectDCCChat.Text = "Disconnect";
            this.disconnectDCCChat.Click += new System.EventHandler(this.disconnectDCCChat_Click);
            // 
            // contextMenuChannelList
            // 
            this.contextMenuChannelList.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuChannelList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeChannenListToolStripMenuItem1});
            this.contextMenuChannelList.Name = "contextMenuChannelList";
            this.contextMenuChannelList.Size = new System.Drawing.Size(151, 26);
            // 
            // closeChannenListToolStripMenuItem1
            // 
            this.closeChannenListToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.closeChannenListToolStripMenuItem1.Name = "closeChannenListToolStripMenuItem1";
            this.closeChannenListToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.closeChannenListToolStripMenuItem1.Text = "Close Window";
            this.closeChannenListToolStripMenuItem1.Click += new System.EventHandler(this.closeChannenListToolStripMenuItem1_Click);
            // 
            // contextMenuWindow
            // 
            this.contextMenuWindow.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearWindowToolStripMenuItem,
            this.closeWindowToolStripMenuItem});
            this.contextMenuWindow.Name = "contextMenuWindow";
            this.contextMenuWindow.Size = new System.Drawing.Size(151, 48);
            // 
            // clearWindowToolStripMenuItem
            // 
            this.clearWindowToolStripMenuItem.Name = "clearWindowToolStripMenuItem";
            this.clearWindowToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.clearWindowToolStripMenuItem.Text = "Clear Window";
            this.clearWindowToolStripMenuItem.Click += new System.EventHandler(this.clearWindowToolStripMenuItem_Click);
            // 
            // closeWindowToolStripMenuItem
            // 
            this.closeWindowToolStripMenuItem.Name = "closeWindowToolStripMenuItem";
            this.closeWindowToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.closeWindowToolStripMenuItem.Text = "Close Window";
            this.closeWindowToolStripMenuItem.Click += new System.EventHandler(this.closeWindowToolStripMenuItem_Click);
            // 
            // contextMenuDebug
            // 
            this.contextMenuDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuClearDebug,
            this.toolStripMenuCloseDebug});
            this.contextMenuDebug.Name = "contextMenuWindow";
            this.contextMenuDebug.Size = new System.Drawing.Size(151, 48);
            // 
            // toolStripMenuClearDebug
            // 
            this.toolStripMenuClearDebug.Name = "toolStripMenuClearDebug";
            this.toolStripMenuClearDebug.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuClearDebug.Text = "Clear Window";
            this.toolStripMenuClearDebug.Click += new System.EventHandler(this.toolStripMenuClearDebug_Click);
            // 
            // toolStripMenuCloseDebug
            // 
            this.toolStripMenuCloseDebug.Name = "toolStripMenuCloseDebug";
            this.toolStripMenuCloseDebug.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuCloseDebug.Text = "Close Window";
            this.toolStripMenuCloseDebug.Click += new System.EventHandler(this.toolStripMenuCloseDebug_Click);
            // 
            // hideJoinPartQuitToolStripMenuItem
            // 
            this.hideJoinPartQuitToolStripMenuItem.Name = "hideJoinPartQuitToolStripMenuItem";
            this.hideJoinPartQuitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.hideJoinPartQuitToolStripMenuItem.Text = "Hide Join/Part/Quit";
            this.hideJoinPartQuitToolStripMenuItem.Click += new System.EventHandler(this.hideJoinPartQuitToolStripMenuItem_Click);
            // 
            // ServerTree
            // 
            this.AccessibleDescription = "List of servers and channels associated with them once connected";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ServerTree";
            this.Size = new System.Drawing.Size(165, 323);
            this.panelButtons.ResumeLayout(false);
            this.contextMenuChannel.ResumeLayout(false);
            this.contextMenuServer.ResumeLayout(false);
            this.contextMenuQuery.ResumeLayout(false);
            this.contextMenuDCCChat.ResumeLayout(false);
            this.contextMenuChannelList.ResumeLayout(false);
            this.contextMenuWindow.ResumeLayout(false);
            this.contextMenuDebug.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuChannel;
        private System.Windows.Forms.ToolStripMenuItem clearChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reJoinChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoJoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoPerformToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuServer;
        private System.Windows.Forms.ContextMenuStrip contextMenuQuery;
        private System.Windows.Forms.ToolStripMenuItem clearQueryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeQueryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem silenceUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLogFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceDisconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItemBlank;
        private System.Windows.Forms.ToolStripMenuItem channelFontToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuDCCChat;
        private System.Windows.Forms.ToolStripMenuItem clearWindowDCCChat;
        private System.Windows.Forms.ToolStripMenuItem closeWindowDCCChat;
        private System.Windows.Forms.ToolStripMenuItem disconnectDCCChat;
        private System.Windows.Forms.ToolStripMenuItem noColorModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToAutoJoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuChannelList;
        private System.Windows.Forms.ToolStripMenuItem closeChannenListToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem eventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSoundsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuWindow;
        private System.Windows.Forms.ToolStripMenuItem clearWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openChannelLogFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableLoggingToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuDebug;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuClearDebug;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuCloseDebug;
        private System.Windows.Forms.ToolStripMenuItem eventsQueryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSoundsQueryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem joinEventLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inChannelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem partEventLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inChannelToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem inConsoleToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitEventLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inChannelToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem inConsoleToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem hideJoinPartQuitToolStripMenuItem;
    }
}
