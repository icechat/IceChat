using System.Windows.Forms;
using System;
using System.Drawing;

namespace IceChat
{
    partial class FormMain
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

        private IceTabControl mainTabControl;
        private InputPanel inputPanel;
        private System.Windows.Forms.MenuStrip menuMainStrip;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;

        internal System.Windows.Forms.ToolStripMenuItem iceChatColorsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem iceChatEditorToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem iceChatSettingsToolStripMenuItem;

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripQuickConnect;
        private System.Windows.Forms.ToolStripButton toolStripSettings;
        private System.Windows.Forms.ToolStripButton toolStripColors;
        private System.Windows.Forms.ToolStripButton toolStripSystemTray;
        private System.Windows.Forms.ContextMenuStrip contextMenuToolBar;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripEditor;
        private System.Windows.Forms.ToolStripMenuItem forumsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iceChatChannelStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizeToTrayToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuMainStrip = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fixWindowSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeCurrentWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteAllSoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAPluginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStylesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.vS2008ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.office2007ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nickListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewChannelBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTabOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreTabOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multilineEditboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.selectNickListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nickListImageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.nickListImageRemoveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.showButtonsNickListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectServerTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverTreeImageMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.serverTreeImageRemoveMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.showButtonsServerTreeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchForNetworksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatHomePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatWikiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aliasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.identifiersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buildFromSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forumsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codePlexPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookFanPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseDataFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseLogsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browsePluginsFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iceChatChannelStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateAvailableToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.splitterRight = new System.Windows.Forms.Splitter();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.contextMenuToolBar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripQuickConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripFonts = new System.Windows.Forms.ToolStripButton();
            this.toolStripColors = new System.Windows.Forms.ToolStripButton();
            this.toolStripEditor = new System.Windows.Forms.ToolStripButton();
            this.toolStripAway = new System.Windows.Forms.ToolStripButton();
            this.toolStripSystemTray = new System.Windows.Forms.ToolStripButton();
            this.toolStripUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.splitterBottom = new System.Windows.Forms.Splitter();
            this.nickListTab = new System.Windows.Forms.TabPage();
            this.nickPanel = new System.Windows.Forms.Panel();
            this.nickList = new NickList(this);

            this.serverListTab = new System.Windows.Forms.TabPage();
            this.serverPanel = new System.Windows.Forms.Panel();
            this.serverTree = new ServerTree(this);

            this.panelDockRight = new IceDockPanel(this);
            this.panelDockLeft = new IceDockPanel(this);
            this.mainChannelBar = new ChannelBar(this);

            this.mainTabControl = new IceChat.IceTabControl();
            this.inputPanel = new IceChat.InputPanel();
            this.menuMainStrip.SuspendLayout();
            this.contextMenuNotify.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.contextMenuToolBar.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            this.nickListTab.SuspendLayout();
            this.serverListTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMainStrip
            // 
            this.menuMainStrip.AccessibleDescription = "Main Menu Bar";
            this.menuMainStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.menuMainStrip.AllowItemReorder = true;
            this.menuMainStrip.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuMainStrip.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuMainStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.viewToolStripMenuItem,
            //this.searchToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.windowsToolStripMenuItem,
            this.closeWindow,
            this.resizeWindowToolStripMenuItem,
            this.updateAvailableToolStripMenuItem1});
            this.menuMainStrip.Location = new System.Drawing.Point(0, 0);
            this.menuMainStrip.MdiWindowListItem = this.windowsToolStripMenuItem;
            this.menuMainStrip.Name = "menuMainStrip";
            this.menuMainStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuMainStrip.ShowItemToolTips = true;
            this.menuMainStrip.Size = new System.Drawing.Size(924, 24);
            this.menuMainStrip.TabIndex = 12;
            this.menuMainStrip.Text = "menuStripMain";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.mainToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minimizeToTrayToolStripMenuItem,
            this.debugWindowToolStripMenuItem,
            this.alwaysOnTopToolStripMenuItem,
            this.fixWindowSizeToolStripMenuItem,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.closeCurrentWindowToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // minimizeToTrayToolStripMenuItem
            // 
            this.minimizeToTrayToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.minimizeToTrayToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.minimizeToTrayToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("minimizeToTrayToolStripMenuItem.Image")));
            this.minimizeToTrayToolStripMenuItem.Name = "minimizeToTrayToolStripMenuItem";
            this.minimizeToTrayToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.minimizeToTrayToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.minimizeToTrayToolStripMenuItem.Text = "Minimize to Tray";
            this.minimizeToTrayToolStripMenuItem.Click += new System.EventHandler(this.minimizeToTrayToolStripMenuItem_Click);
            // 
            // debugWindowToolStripMenuItem
            // 
            this.debugWindowToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.debugWindowToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.debugWindowToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("debugWindowToolStripMenuItem.Image")));
            this.debugWindowToolStripMenuItem.Name = "debugWindowToolStripMenuItem";
            this.debugWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.debugWindowToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.debugWindowToolStripMenuItem.Text = "Debug Window";
            this.debugWindowToolStripMenuItem.Click += new System.EventHandler(this.debugWindowToolStripMenuItem_Click);
            // 
            // alwaysOnTopToolStripMenuItem
            // 
            this.alwaysOnTopToolStripMenuItem.CheckOnClick = true;
            this.alwaysOnTopToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.alwaysOnTopToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.alwaysOnTopToolStripMenuItem.Name = "alwaysOnTopToolStripMenuItem";
            this.alwaysOnTopToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.alwaysOnTopToolStripMenuItem.Text = "Always on Top";
            this.alwaysOnTopToolStripMenuItem.Click += new System.EventHandler(this.alwaysOnTopToolStripMenuItem_Click);
            // 
            // fixWindowSizeToolStripMenuItem
            // 
            this.fixWindowSizeToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.fixWindowSizeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.fixWindowSizeToolStripMenuItem.Name = "fixWindowSizeToolStripMenuItem";
            this.fixWindowSizeToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.fixWindowSizeToolStripMenuItem.Text = "Lock Window Size";
            this.fixWindowSizeToolStripMenuItem.Click += new System.EventHandler(this.fixWindowSizeToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Visible = false;
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Visible = false;
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // closeCurrentWindowToolStripMenuItem
            // 
            this.closeCurrentWindowToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.closeCurrentWindowToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.closeCurrentWindowToolStripMenuItem.Name = "closeCurrentWindowToolStripMenuItem";
            this.closeCurrentWindowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeCurrentWindowToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.closeCurrentWindowToolStripMenuItem.Text = "Close Current Window";
            this.closeCurrentWindowToolStripMenuItem.Click += new System.EventHandler(this.closeCurrentWindowToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(271, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.exitToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(274, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iceChatSettingsToolStripMenuItem,
            this.fontSettingsToolStripMenuItem,
            this.iceChatColorsToolStripMenuItem,
            this.iceChatEditorToolStripMenuItem,
            this.muteAllSoundsToolStripMenuItem,
            this.pluginsToolStripMenuItem});
            this.optionsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // iceChatSettingsToolStripMenuItem
            // 
            this.iceChatSettingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatSettingsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("iceChatSettingsToolStripMenuItem.Image")));
            this.iceChatSettingsToolStripMenuItem.Name = "iceChatSettingsToolStripMenuItem";
            this.iceChatSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.iceChatSettingsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.iceChatSettingsToolStripMenuItem.Text = "Program Settings...";
            this.iceChatSettingsToolStripMenuItem.Click += new System.EventHandler(this.iceChatSettingsToolStripMenuItem_Click);
            // 
            // fontSettingsToolStripMenuItem
            // 
            this.fontSettingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.fontSettingsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.fontSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fontSettingsToolStripMenuItem.Image")));
            this.fontSettingsToolStripMenuItem.Name = "fontSettingsToolStripMenuItem";
            this.fontSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fontSettingsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.fontSettingsToolStripMenuItem.Text = "Font Settings...";
            this.fontSettingsToolStripMenuItem.Click += new System.EventHandler(this.fontSettingsToolStripMenuItem_Click);
            // 
            // iceChatColorsToolStripMenuItem
            // 
            this.iceChatColorsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatColorsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatColorsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("iceChatColorsToolStripMenuItem.Image")));
            this.iceChatColorsToolStripMenuItem.Name = "iceChatColorsToolStripMenuItem";
            this.iceChatColorsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.iceChatColorsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.iceChatColorsToolStripMenuItem.Text = "Colors Settings...";
            this.iceChatColorsToolStripMenuItem.Click += new System.EventHandler(this.iceChatColorsToolStripMenuItem_Click);
            // 
            // iceChatEditorToolStripMenuItem
            // 
            this.iceChatEditorToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatEditorToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatEditorToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("iceChatEditorToolStripMenuItem.Image")));
            this.iceChatEditorToolStripMenuItem.Name = "iceChatEditorToolStripMenuItem";
            this.iceChatEditorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.iceChatEditorToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.iceChatEditorToolStripMenuItem.Text = "IceChat Editor...";
            this.iceChatEditorToolStripMenuItem.Click += new System.EventHandler(this.iceChatEditorToolStripMenuItem_Click);
            // 
            // muteAllSoundsToolStripMenuItem
            // 
            this.muteAllSoundsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.muteAllSoundsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.muteAllSoundsToolStripMenuItem.Name = "muteAllSoundsToolStripMenuItem";
            this.muteAllSoundsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.muteAllSoundsToolStripMenuItem.Text = "Mute all Sounds";
            this.muteAllSoundsToolStripMenuItem.Click += new System.EventHandler(this.muteAllSoundsToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAPluginToolStripMenuItem});
            this.pluginsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.pluginsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pluginsToolStripMenuItem.Image")));
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.pluginsToolStripMenuItem.Text = "Loaded Plugins";
            // 
            // loadAPluginToolStripMenuItem
            // 
            this.loadAPluginToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.loadAPluginToolStripMenuItem.Name = "loadAPluginToolStripMenuItem";
            this.loadAPluginToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.loadAPluginToolStripMenuItem.Text = "Load a Plugin...";
            this.loadAPluginToolStripMenuItem.ToolTipText = "Load a new Plugin";
            this.loadAPluginToolStripMenuItem.Click += new System.EventHandler(this.loadAPluginToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themesToolStripMenuItem,
            this.menuStylesToolStripMenuItem,
            this.serverListToolStripMenuItem,
            this.nickListToolStripMenuItem,
            this.statusBarToolStripMenuItem,
            this.toolBarToolStripMenuItem,
            this.channelBarToolStripMenuItem,
            this.multilineEditboxToolStripMenuItem,
            this.channelListToolStripMenuItem,
            this.toolStripMenuItem3,
            this.selectNickListToolStripMenuItem,
            this.selectServerTreeToolStripMenuItem});
            this.viewToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // themesToolStripMenuItem
            // 
            this.themesToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem});
            this.themesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            this.themesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.themesToolStripMenuItem.Text = "Themes";
            // 
            // defaultToolStripMenuItem
            // 
            this.defaultToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.defaultToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.defaultToolStripMenuItem.Name = "defaultToolStripMenuItem";
            this.defaultToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.defaultToolStripMenuItem.Text = "Default";
            this.defaultToolStripMenuItem.Click += new System.EventHandler(this.defaultToolStripMenuItem_Click);
            // 
            // menuStylesToolStripMenuItem
            // 
            this.menuStylesToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.menuStylesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem1,
            this.vS2008ToolStripMenuItem,
            this.office2007ToolStripMenuItem});
            this.menuStylesToolStripMenuItem.Name = "menuStylesToolStripMenuItem";
            this.menuStylesToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.menuStylesToolStripMenuItem.Text = "Menu Styles";
            // 
            // defaultToolStripMenuItem1
            // 
            this.defaultToolStripMenuItem1.BackColor = System.Drawing.SystemColors.Menu;
            this.defaultToolStripMenuItem1.Name = "defaultToolStripMenuItem1";
            this.defaultToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.defaultToolStripMenuItem1.Text = "Default";
            this.defaultToolStripMenuItem1.Click += new System.EventHandler(this.DefaultRendererToolStripMenuItem_Click);
            // 
            // vS2008ToolStripMenuItem
            // 
            this.vS2008ToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.vS2008ToolStripMenuItem.Name = "vS2008ToolStripMenuItem";
            this.vS2008ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.vS2008ToolStripMenuItem.Text = "VS 2008";
            this.vS2008ToolStripMenuItem.Click += new System.EventHandler(this.VS2008ToolStripMenuItem_Click);
            // 
            // office2007ToolStripMenuItem
            // 
            this.office2007ToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.office2007ToolStripMenuItem.Name = "office2007ToolStripMenuItem";
            this.office2007ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.office2007ToolStripMenuItem.Text = "Office 2007";
            this.office2007ToolStripMenuItem.Click += new System.EventHandler(this.Office2007ToolStripMenuItem_Click);
            // 
            // serverListToolStripMenuItem
            // 
            this.serverListToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.serverListToolStripMenuItem.Checked = true;
            this.serverListToolStripMenuItem.CheckOnClick = true;
            this.serverListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serverListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.serverListToolStripMenuItem.Name = "serverListToolStripMenuItem";
            this.serverListToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.serverListToolStripMenuItem.Text = "Left Panel";
            this.serverListToolStripMenuItem.Click += new System.EventHandler(this.serverListToolStripMenuItem_Click);
            // 
            // nickListToolStripMenuItem
            // 
            this.nickListToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.nickListToolStripMenuItem.Checked = true;
            this.nickListToolStripMenuItem.CheckOnClick = true;
            this.nickListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nickListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.nickListToolStripMenuItem.Name = "nickListToolStripMenuItem";
            this.nickListToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.nickListToolStripMenuItem.Text = "Right Panel";
            this.nickListToolStripMenuItem.Click += new System.EventHandler(this.nickListToolStripMenuItem_Click);
            // 
            // statusBarToolStripMenuItem
            // 
            this.statusBarToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.statusBarToolStripMenuItem.Checked = true;
            this.statusBarToolStripMenuItem.CheckOnClick = true;
            this.statusBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statusBarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.statusBarToolStripMenuItem.Name = "statusBarToolStripMenuItem";
            this.statusBarToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.statusBarToolStripMenuItem.Text = "Status Bar";
            this.statusBarToolStripMenuItem.Click += new System.EventHandler(this.statusBarToolStripMenuItem_Click);
            // 
            // toolBarToolStripMenuItem
            // 
            this.toolBarToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.toolBarToolStripMenuItem.Checked = true;
            this.toolBarToolStripMenuItem.CheckOnClick = true;
            this.toolBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolBarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.toolBarToolStripMenuItem.Name = "toolBarToolStripMenuItem";
            this.toolBarToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.toolBarToolStripMenuItem.Text = "Tool Bar";
            this.toolBarToolStripMenuItem.Click += new System.EventHandler(this.toolBarToolStripMenuItem_Click);
            // 
            // channelBarToolStripMenuItem
            // 
            this.channelBarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewChannelBarToolStripMenuItem,
            this.saveTabOrderToolStripMenuItem,
            this.restoreTabOrderToolStripMenuItem});
            this.channelBarToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.channelBarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.channelBarToolStripMenuItem.Name = "channelBarToolStripMenuItem";
            this.channelBarToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.channelBarToolStripMenuItem.Text = "Channel Bar";
            // 
            // viewChannelBarToolStripMenuItem
            // 
            this.viewChannelBarToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.viewChannelBarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.viewChannelBarToolStripMenuItem.Checked = true;
            this.viewChannelBarToolStripMenuItem.CheckOnClick = true;
            this.viewChannelBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewChannelBarToolStripMenuItem.Name = "viewChannelBarToolStripMenuItem";
            this.viewChannelBarToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewChannelBarToolStripMenuItem.Text = "View Channel Bar";
            this.viewChannelBarToolStripMenuItem.Click += new System.EventHandler(this.viewChannelBarToolStripMenuItem_Click);
            // 
            // saveTabOrderToolStripMenuItem
            // 
            this.saveTabOrderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.saveTabOrderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.saveTabOrderToolStripMenuItem.Name = "saveTabOrderToolStripMenuItem";
            this.saveTabOrderToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.saveTabOrderToolStripMenuItem.Text = "Save Tab Order";
            this.saveTabOrderToolStripMenuItem.Click += new System.EventHandler(this.saveTabOrderToolStripMenuItem_Click);
            // 
            // restoreTabOrderToolStripMenuItem
            // 
            this.restoreTabOrderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.restoreTabOrderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.restoreTabOrderToolStripMenuItem.Name = "restoreTabOrderToolStripMenuItem";
            this.restoreTabOrderToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.restoreTabOrderToolStripMenuItem.Text = "Restore Tab Order";
            this.restoreTabOrderToolStripMenuItem.Click += new System.EventHandler(this.restoreTabOrderToolStripMenuItem_Click);
            // 
            // multilineEditboxToolStripMenuItem
            // 
            this.multilineEditboxToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.multilineEditboxToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.multilineEditboxToolStripMenuItem.Name = "multilineEditboxToolStripMenuItem";
            this.multilineEditboxToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.multilineEditboxToolStripMenuItem.Text = "Multiline Editbox";
            this.multilineEditboxToolStripMenuItem.Click += new System.EventHandler(this.multilineEditboxToolStripMenuItem_Click);
            // 
            // channelListToolStripMenuItem
            // 
            this.channelListToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.channelListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.channelListToolStripMenuItem.Name = "channelListToolStripMenuItem";
            this.channelListToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.channelListToolStripMenuItem.Text = "Channel List";
            this.channelListToolStripMenuItem.Click += new System.EventHandler(this.channelListToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.BackColor = System.Drawing.SystemColors.Menu;
            this.toolStripMenuItem3.ForeColor = System.Drawing.SystemColors.MenuText;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(179, 6);
            // 
            // selectNickListToolStripMenuItem
            // 
            this.selectNickListToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.selectNickListToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nickListImageMenu,
            this.nickListImageRemoveMenu,
            this.showButtonsNickListToolStripMenuItem});
            this.selectNickListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.selectNickListToolStripMenuItem.Name = "selectNickListToolStripMenuItem";
            this.selectNickListToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.selectNickListToolStripMenuItem.Text = "Nick List";
            // 
            // nickListImageMenu
            // 
            this.nickListImageMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.nickListImageMenu.ForeColor = System.Drawing.SystemColors.MenuText;
            this.nickListImageMenu.Name = "nickListImageMenu";
            this.nickListImageMenu.Size = new System.Drawing.Size(253, 22);
            this.nickListImageMenu.Text = "Background Image...";
            this.nickListImageMenu.Click += new System.EventHandler(this.nickListImageMenu_Click);
            // 
            // nickListImageRemoveMenu
            // 
            this.nickListImageRemoveMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.nickListImageRemoveMenu.ForeColor = System.Drawing.SystemColors.MenuText;
            this.nickListImageRemoveMenu.Name = "nickListImageRemoveMenu";
            this.nickListImageRemoveMenu.Size = new System.Drawing.Size(253, 22);
            this.nickListImageRemoveMenu.Text = "Remove Background Image";
            this.nickListImageRemoveMenu.Click += new System.EventHandler(this.nickListImageRemoveMenu_Click);
            // 
            // showButtonsNickListToolStripMenuItem
            // 
            this.showButtonsNickListToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.showButtonsNickListToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.showButtonsNickListToolStripMenuItem.Name = "showButtonsNickListToolStripMenuItem";
            this.showButtonsNickListToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.showButtonsNickListToolStripMenuItem.Text = "Show Buttons";
            this.showButtonsNickListToolStripMenuItem.Click += new System.EventHandler(this.showButtonsNickListToolStripMenuItem_Click);
            // 
            // selectServerTreeToolStripMenuItem
            // 
            this.selectServerTreeToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.selectServerTreeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverTreeImageMenu,
            this.serverTreeImageRemoveMenu,
            this.showButtonsServerTreeToolStripMenuItem1});
            this.selectServerTreeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.selectServerTreeToolStripMenuItem.Name = "selectServerTreeToolStripMenuItem";
            this.selectServerTreeToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.selectServerTreeToolStripMenuItem.Text = "Server Tree";
            // 
            // serverTreeImageMenu
            // 
            this.serverTreeImageMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.serverTreeImageMenu.ForeColor = System.Drawing.SystemColors.MenuText;
            this.serverTreeImageMenu.Name = "serverTreeImageMenu";
            this.serverTreeImageMenu.Size = new System.Drawing.Size(253, 22);
            this.serverTreeImageMenu.Text = "Background Image...";
            this.serverTreeImageMenu.Click += new System.EventHandler(this.serverTreeImageMenu_Click);
            // 
            // serverTreeImageRemoveMenu
            // 
            this.serverTreeImageRemoveMenu.BackColor = System.Drawing.SystemColors.Menu;
            this.serverTreeImageRemoveMenu.ForeColor = System.Drawing.SystemColors.MenuText;
            this.serverTreeImageRemoveMenu.Name = "serverTreeImageRemoveMenu";
            this.serverTreeImageRemoveMenu.Size = new System.Drawing.Size(253, 22);
            this.serverTreeImageRemoveMenu.Text = "Remove Background Image";
            this.serverTreeImageRemoveMenu.Click += new System.EventHandler(this.serverTreeImageRemoveMenu_Click);
            // 
            // showButtonsServerTreeToolStripMenuItem1
            // 
            this.showButtonsServerTreeToolStripMenuItem1.BackColor = System.Drawing.SystemColors.Menu;
            this.showButtonsServerTreeToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.showButtonsServerTreeToolStripMenuItem1.Name = "showButtonsServerTreeToolStripMenuItem1";
            this.showButtonsServerTreeToolStripMenuItem1.Size = new System.Drawing.Size(253, 22);
            this.showButtonsServerTreeToolStripMenuItem1.Text = "Show Buttons";
            this.showButtonsServerTreeToolStripMenuItem1.Click += new System.EventHandler(this.showButtonsServerTreeToolStripMenuItem1_Click);
            /*
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchForChannelsToolStripMenuItem,
            this.searchForNetworksToolStripMenuItem});
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.searchToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.searchToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.searchToolStripMenuItem.Text = "Search";
            
            // 
            // searchForChannelsToolStripMenuItem
            // 
            this.searchForChannelsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.searchForChannelsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.searchForChannelsToolStripMenuItem.Name = "searchForChannelsToolStripMenuItem";
            this.searchForChannelsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.searchForChannelsToolStripMenuItem.Text = "Search for Channels";
            this.searchForChannelsToolStripMenuItem.Click += new System.EventHandler(this.searchForChannelsToolStripMenuItem_Click);
            // 
            // searchForNetworksToolStripMenuItem
            // 
            this.searchForNetworksToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.searchForNetworksToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.searchForNetworksToolStripMenuItem.Name = "searchForNetworksToolStripMenuItem";
            this.searchForNetworksToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.searchForNetworksToolStripMenuItem.Text = "Search for Networks";
            this.searchForNetworksToolStripMenuItem.Click += new System.EventHandler(this.searchForNetworksToolStripMenuItem_Click);
            */
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuBar;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iceChatHomePageToolStripMenuItem,
            this.iceChatWikiToolStripMenuItem,
            this.forumsToolStripMenuItem,
            //this.codePlexPageToolStripMenuItem,
            this.gitHubPageToolStripMenuItem,
            this.facebookFanPageToolStripMenuItem,
            this.downloadPluginsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.checkForUpdateToolStripMenuItem,
            this.browseDataFolderToolStripMenuItem,
            this.browseLogsFolderToolStripMenuItem,
            this.browsePluginsFolderToolStripMenuItem,
            this.iceChatChannelStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // iceChatHomePageToolStripMenuItem
            // 
            this.iceChatHomePageToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatHomePageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatHomePageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("iceChatHomePageToolStripMenuItem.Image")));
            this.iceChatHomePageToolStripMenuItem.Name = "iceChatHomePageToolStripMenuItem";
            this.iceChatHomePageToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.iceChatHomePageToolStripMenuItem.Text = "IceChat Home Page";
            this.iceChatHomePageToolStripMenuItem.Click += new System.EventHandler(this.iceChatHomePageToolStripMenuItem_Click);
            // 
            // iceChatWikiToolStripMenuItem
            // 
            this.iceChatWikiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commandsToolStripMenuItem,
            this.aliasesToolStripMenuItem,
            this.identifiersToolStripMenuItem,
            this.portableToolStripMenuItem,
            this.buildFromSourceToolStripMenuItem});
            this.iceChatWikiToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatWikiToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatWikiToolStripMenuItem.Name = "iceChatWikiToolStripMenuItem";
            this.iceChatWikiToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.iceChatWikiToolStripMenuItem.Text = "IceChat Wiki";
            this.iceChatWikiToolStripMenuItem.Click += new System.EventHandler(this.iceChatWikiToolStripMenuItem_Click);
            // 
            // commandsToolStripMenuItem
            // 
            this.commandsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.commandsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.commandsToolStripMenuItem.Name = "commandsToolStripMenuItem";
            this.commandsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.commandsToolStripMenuItem.Text = "Commands";
            this.commandsToolStripMenuItem.Click += new System.EventHandler(this.commandsToolStripMenuItem_Click);
            // 
            // aliasesToolStripMenuItem
            // 
            this.aliasesToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.aliasesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.aliasesToolStripMenuItem.Name = "aliasesToolStripMenuItem";
            this.aliasesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.aliasesToolStripMenuItem.Text = "Aliases";
            this.aliasesToolStripMenuItem.Click += new System.EventHandler(this.aliasesToolStripMenuItem_Click);
            // 
            // identifiersToolStripMenuItem
            // 
            this.identifiersToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.identifiersToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.identifiersToolStripMenuItem.Name = "identifiersToolStripMenuItem";
            this.identifiersToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.identifiersToolStripMenuItem.Text = "Identifiers";
            this.identifiersToolStripMenuItem.Click += new System.EventHandler(this.identifiersToolStripMenuItem_Click);
            // 
            // portableToolStripMenuItem
            // 
            this.portableToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.portableToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.portableToolStripMenuItem.Name = "portableToolStripMenuItem";
            this.portableToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.portableToolStripMenuItem.Text = "IceChat 9 Portable";
            this.portableToolStripMenuItem.Click += new System.EventHandler(this.portableToolStripMenuItem_Click);
            // 
            // buildFromSourceToolStripMenuItem
            // 
            this.buildFromSourceToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.buildFromSourceToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.buildFromSourceToolStripMenuItem.Name = "buildFromSourceToolStripMenuItem";
            this.buildFromSourceToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.buildFromSourceToolStripMenuItem.Text = "Build from Source";
            this.buildFromSourceToolStripMenuItem.Click += new System.EventHandler(this.buildFromSourceToolStripMenuItem_Click);
            // 
            // forumsToolStripMenuItem
            // 
            this.forumsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.forumsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.forumsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("forumsToolStripMenuItem.Image")));
            this.forumsToolStripMenuItem.Name = "forumsToolStripMenuItem";
            this.forumsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.forumsToolStripMenuItem.Text = "IceChat Forums";
            this.forumsToolStripMenuItem.Click += new System.EventHandler(this.forumsToolStripMenuItem_Click);
            // 
            // codePlexPageToolStripMenuItem
            // 
            this.codePlexPageToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.codePlexPageToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.codePlexPageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.codePlexPageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("codePlexPageToolStripMenuItem.Image")));
            this.codePlexPageToolStripMenuItem.Name = "codePlexPageToolStripMenuItem";
            this.codePlexPageToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.codePlexPageToolStripMenuItem.Text = "CodePlex Page";
            this.codePlexPageToolStripMenuItem.Click += new System.EventHandler(this.codePlexPageToolStripMenuItem_Click);
            // 
            // gitHubPageToolStripMenuItem
            // 
            this.gitHubPageToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.gitHubPageToolStripMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gitHubPageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.gitHubPageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("gitHubPageToolStripMenuItem.Image")));
            this.gitHubPageToolStripMenuItem.Name = "gitHubPageToolStripMenuItem";
            this.gitHubPageToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.gitHubPageToolStripMenuItem.Text = "IceChat on GitHub";
            this.gitHubPageToolStripMenuItem.Click += new System.EventHandler(this.gitHubPageToolStripMenuItem_Click);
            // 
            // facebookFanPageToolStripMenuItem
            // 
            this.facebookFanPageToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.facebookFanPageToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.facebookFanPageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("facebookFanPageToolStripMenuItem.Image")));
            this.facebookFanPageToolStripMenuItem.Name = "facebookFanPageToolStripMenuItem";
            this.facebookFanPageToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.facebookFanPageToolStripMenuItem.Text = "Facebook Fan page";
            this.facebookFanPageToolStripMenuItem.Click += new System.EventHandler(this.facebookFanPageToolStripMenuItem_Click);
            // 
            // downloadPluginsToolStripMenuItem
            // 
            this.downloadPluginsToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.downloadPluginsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.downloadPluginsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("downloadPluginsToolStripMenuItem.Image")));
            this.downloadPluginsToolStripMenuItem.Name = "downloadPluginsToolStripMenuItem";
            this.downloadPluginsToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.downloadPluginsToolStripMenuItem.Text = "Download Plugins";
            this.downloadPluginsToolStripMenuItem.Click += new System.EventHandler(this.downloadPluginsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.BackColor = System.Drawing.SystemColors.Menu;
            this.toolStripMenuItem1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(215, 6);
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.checkForUpdateToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.checkForUpdateToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("checkForUpdateToolStripMenuItem.Image")));
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.checkForUpdateToolStripMenuItem.Text = "Check for Update";
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // browseDataFolderToolStripMenuItem
            // 
            this.browseDataFolderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.browseDataFolderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.browseDataFolderToolStripMenuItem.Name = "browseDataFolderToolStripMenuItem";
            this.browseDataFolderToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.browseDataFolderToolStripMenuItem.Text = "Browse Data Folder";
            this.browseDataFolderToolStripMenuItem.Click += new System.EventHandler(this.browseDataFolderToolStripMenuItem_Click);
            // 
            // browseLogsFolderToolStripMenuItem
            // 
            this.browseLogsFolderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.browseLogsFolderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.browseLogsFolderToolStripMenuItem.Name = "browseLogsFolderToolStripMenuItem";
            this.browseLogsFolderToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.browseLogsFolderToolStripMenuItem.Text = "Browse Logs Folder";
            this.browseLogsFolderToolStripMenuItem.Click += new System.EventHandler(this.browseLogsFolderToolStripMenuItem_Click);
            // 
            // browsePluginsFolderToolStripMenuItem
            // 
            this.browsePluginsFolderToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.browsePluginsFolderToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.browsePluginsFolderToolStripMenuItem.Name = "browsePluginsFolderToolStripMenuItem";
            this.browsePluginsFolderToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.browsePluginsFolderToolStripMenuItem.Text = "Browse Plugins Folder";
            this.browsePluginsFolderToolStripMenuItem.Click += new System.EventHandler(this.browsePluginsFolderToolStripMenuItem_Click);
            // 
            // iceChatChannelStripMenuItem
            // 
            this.iceChatChannelStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.iceChatChannelStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.iceChatChannelStripMenuItem.Name = "iceChatChannelStripMenuItem";
            this.iceChatChannelStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.iceChatChannelStripMenuItem.Text = "Developer Channel";
            this.iceChatChannelStripMenuItem.Click += new System.EventHandler(this.iceChatChannelStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.aboutToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // windowsToolStripMenuItem
            // 
            this.windowsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem,
            this.tileVerticalToolStripMenuItem});
            this.windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            this.windowsToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.windowsToolStripMenuItem.Text = "Windows";
            this.windowsToolStripMenuItem.Visible = false;
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.cascadeToolStripMenuItem.Text = "Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.cascadeToolStripMenuItem_Click);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.tileHorizontalToolStripMenuItem.Text = "Tile Horizontal";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.tileHorizontalToolStripMenuItem_Click);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.tileVerticalToolStripMenuItem.Text = "Tile Vertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.tileVerticalToolStripMenuItem_Click);
            // 
            // closeWindow
            // 
            this.closeWindow.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.closeWindow.AutoToolTip = true;
            this.closeWindow.BackColor = System.Drawing.SystemColors.MenuBar;
            this.closeWindow.Image = ((System.Drawing.Image)(resources.GetObject("closeWindow.Image")));
            this.closeWindow.Name = "closeWindow";
            this.closeWindow.Size = new System.Drawing.Size(28, 20);
            this.closeWindow.Text = null;
            this.closeWindow.ToolTipText = "Close Current Window";
            this.closeWindow.Click += new System.EventHandler(this.closeWindow_Click);
            // 
            // resizeWindowToolStripMenuItem
            // 
            this.resizeWindowToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.resizeWindowToolStripMenuItem.Image = global::IceChat.Properties.Resources.restore;
            this.resizeWindowToolStripMenuItem.Name = "resizeWindowToolStripMenuItem";
            this.resizeWindowToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.resizeWindowToolStripMenuItem.ToolTipText = "Resize Current Window";
            this.resizeWindowToolStripMenuItem.Click += new System.EventHandler(this.resizeWindowToolStripMenuItem_Click);
            // 
            // updateAvailableToolStripMenuItem1
            // 
            this.updateAvailableToolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.updateAvailableToolStripMenuItem1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.updateAvailableToolStripMenuItem1.ForeColor = System.Drawing.Color.Red;
            //this.updateAvailableToolStripMenuItem1.Font.Bold = true;
            this.updateAvailableToolStripMenuItem1.Name = "updateAvailableToolStripMenuItem1";
            this.updateAvailableToolStripMenuItem1.Size = new System.Drawing.Size(130, 20);
            this.updateAvailableToolStripMenuItem1.Text = "Update Available";
            this.updateAvailableToolStripMenuItem1.Visible = false;
            this.updateAvailableToolStripMenuItem1.Click += new EventHandler(updateAvailableToolStripMenuItem1_Click);
            // 
            // splitterLeft
            // 
            this.splitterLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitterLeft.Location = new System.Drawing.Point(0, 63);
            this.splitterLeft.Name = "splitterLeft";
            this.splitterLeft.Size = new System.Drawing.Size(3, 462);
            this.splitterLeft.TabIndex = 15;
            this.splitterLeft.TabStop = false;
            // 
            // splitterRight
            // 
            this.splitterRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterRight.Location = new System.Drawing.Point(921, 63);
            this.splitterRight.Name = "splitterRight";
            this.splitterRight.Size = new System.Drawing.Size(3, 462);
            this.splitterRight.TabIndex = 16;
            this.splitterRight.TabStop = false;
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipTitle = "IceChat";
            this.notifyIcon.ContextMenuStrip = this.contextMenuNotify;
            this.notifyIcon.Text = "IceChat";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIconMouseDoubleClick);
            // 
            // contextMenuNotify
            // 
            this.contextMenuNotify.BackColor = System.Drawing.SystemColors.MenuBar;
            this.contextMenuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.exitToolStripMenuItem1});
            this.contextMenuNotify.Name = "contextMenuNotify";
            this.contextMenuNotify.Size = new System.Drawing.Size(114, 48);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.BackColor = System.Drawing.SystemColors.Menu;
            this.restoreToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.restoreToolStripMenuItem.Text = "Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.BackColor = System.Drawing.SystemColors.Menu;
            this.exitToolStripMenuItem1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.AccessibleDescription = "Main Tool bar";
            this.toolStripMain.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.toolStripMain.AllowItemReorder = true;
            this.toolStripMain.ContextMenuStrip = this.contextMenuToolBar;
            this.toolStripMain.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripQuickConnect,
            this.toolStripSettings,
            this.toolStripFonts,
            this.toolStripColors,
            this.toolStripEditor,
            this.toolStripAway,
            this.toolStripSystemTray,
            this.toolStripUpdate});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripMain.Size = new System.Drawing.Size(924, 39);
            this.toolStripMain.TabIndex = 17;
            this.toolStripMain.Text = "toolStripMain";
            this.toolStripMain.VisibleChanged += new System.EventHandler(this.toolStripMain_VisibleChanged);
            // 
            // contextMenuToolBar
            // 
            this.contextMenuToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hideToolStripMenuItem});
            this.contextMenuToolBar.Name = "contextMenuToolBar";
            this.contextMenuToolBar.Size = new System.Drawing.Size(100, 26);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // toolStripQuickConnect
            // 
            this.toolStripQuickConnect.AccessibleDescription = "Bring up Quick Connect Window";
            this.toolStripQuickConnect.BackColor = System.Drawing.Color.Transparent;
            this.toolStripQuickConnect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripQuickConnect.Image")));
            this.toolStripQuickConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripQuickConnect.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripQuickConnect.Name = "toolStripQuickConnect";
            this.toolStripQuickConnect.Size = new System.Drawing.Size(133, 39);
            this.toolStripQuickConnect.Text = "Quick Connect";
            this.toolStripQuickConnect.Click += new System.EventHandler(this.toolStripQuickConnect_Click);
            // 
            // toolStripSettings
            // 
            this.toolStripSettings.AccessibleDescription = "Open settings window";
            this.toolStripSettings.BackColor = System.Drawing.Color.Transparent;
            this.toolStripSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSettings.Image")));
            this.toolStripSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSettings.Name = "toolStripSettings";
            this.toolStripSettings.Size = new System.Drawing.Size(95, 36);
            this.toolStripSettings.Text = "Settings";
            this.toolStripSettings.Click += new System.EventHandler(this.toolStripSettings_Click);
            // 
            // toolStripFonts
            // 
            this.toolStripFonts.AccessibleDescription = "Open Fonts settings window";
            this.toolStripFonts.BackColor = System.Drawing.Color.Transparent;
            this.toolStripFonts.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFonts.Image")));
            this.toolStripFonts.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFonts.Name = "toolStripFonts";
            this.toolStripFonts.Size = new System.Drawing.Size(78, 36);
            this.toolStripFonts.Text = "Fonts";
            this.toolStripFonts.Click += new System.EventHandler(this.toolStripFonts_Click);
            // 
            // toolStripColors
            // 
            this.toolStripColors.AccessibleDescription = "Open color settings window";
            this.toolStripColors.BackColor = System.Drawing.Color.Transparent;
            this.toolStripColors.Image = ((System.Drawing.Image)(resources.GetObject("toolStripColors.Image")));
            this.toolStripColors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripColors.Name = "toolStripColors";
            this.toolStripColors.Size = new System.Drawing.Size(83, 36);
            this.toolStripColors.Text = "Colors";
            this.toolStripColors.Click += new System.EventHandler(this.toolStripColors_Click);
            // 
            // toolStripEditor
            // 
            this.toolStripEditor.AccessibleDescription = "Open IceChat Editor";
            this.toolStripEditor.BackColor = System.Drawing.Color.Transparent;
            this.toolStripEditor.Image = ((System.Drawing.Image)(resources.GetObject("toolStripEditor.Image")));
            this.toolStripEditor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripEditor.Name = "toolStripEditor";
            this.toolStripEditor.Size = new System.Drawing.Size(80, 36);
            this.toolStripEditor.Text = "Editor";
            this.toolStripEditor.Click += new System.EventHandler(this.toolStripEditor_Click);
            // 
            // toolStripAway
            // 
            this.toolStripAway.AccessibleDescription = "Set yourself as away, or return";
            this.toolStripAway.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAway.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAway.Image")));
            this.toolStripAway.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAway.Name = "toolStripAway";
            this.toolStripAway.Size = new System.Drawing.Size(102, 36);
            this.toolStripAway.Text = "Set Away";
            this.toolStripAway.ToolTipText = "Set Away";
            this.toolStripAway.Click += new System.EventHandler(this.toolStripAway_Click);
            // 
            // toolStripSystemTray
            // 
            this.toolStripSystemTray.AccessibleDescription = "Put IceChat on the System Tray";
            this.toolStripSystemTray.BackColor = System.Drawing.Color.Transparent;
            this.toolStripSystemTray.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSystemTray.Image")));
            this.toolStripSystemTray.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSystemTray.Name = "toolStripSystemTray";
            this.toolStripSystemTray.Size = new System.Drawing.Size(120, 36);
            this.toolStripSystemTray.Text = "System Tray";
            this.toolStripSystemTray.Click += new System.EventHandler(this.toolStripSystemTray_Click);
            // 
            // toolStripUpdate
            // 
            this.toolStripUpdate.AccessibleDescription = "IceChat Update Available";
            this.toolStripUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripUpdate.BackColor = System.Drawing.Color.Transparent;
            this.toolStripUpdate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripUpdate.Image")));
            this.toolStripUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripUpdate.Name = "toolStripUpdate";
            this.toolStripUpdate.Size = new System.Drawing.Size(149, 36);
            this.toolStripUpdate.Text = "Update Available";
            this.toolStripUpdate.Click += new System.EventHandler(this.toolStripUpdate_Click);
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatus.Text = "Status:";
            this.toolStripStatus.ToolTipText = "hey hey";
            // 
            // statusStripMain
            // 
            this.statusStripMain.AccessibleDescription = "Main status bar";
            this.statusStripMain.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar;
            this.statusStripMain.CanOverflow = true;
            this.statusStripMain.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 551);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStripMain.ShowItemToolTips = true;
            this.statusStripMain.Size = new System.Drawing.Size(924, 22);
            this.statusStripMain.SizingGrip = false;
            this.statusStripMain.TabIndex = 18;
            // 
            // splitterBottom
            // 
            this.splitterBottom.BackColor = System.Drawing.Color.Red;
            this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterBottom.Location = new System.Drawing.Point(0, 573);
            this.splitterBottom.Name = "splitterBottom";
            this.splitterBottom.Size = new System.Drawing.Size(924, 3);
            this.splitterBottom.TabIndex = 0;
            this.splitterBottom.TabStop = false;
            this.splitterBottom.Visible = false;
            // 
            // nickListTab
            // 
            this.nickListTab.Controls.Add(this.nickPanel);
            this.nickListTab.Location = new System.Drawing.Point(4, 4);
            this.nickListTab.Name = "nickListTab";
            this.nickListTab.Size = new System.Drawing.Size(192, 454);
            this.nickListTab.TabIndex = 0;
            this.nickListTab.Text = "Nick List";
            // 
            // nickList
            // 
            this.nickList.AccessibleDescription = "List of Nick Names";
            this.nickList.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.nickList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nickList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nickList.Header = "";
            this.nickList.Location = new System.Drawing.Point(0, 0);
            this.nickList.Margin = new System.Windows.Forms.Padding(4);
            this.nickList.Name = "nickList";
            this.nickList.Size = new System.Drawing.Size(192, 454);
            this.nickList.TabIndex = 0;
            // 
            // nickPanel
            // 
            this.nickPanel.Controls.Add(this.nickList);
            this.nickPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nickPanel.Location = new System.Drawing.Point(0, 0);
            this.nickPanel.Name = "nickPanel";
            this.nickPanel.Size = new System.Drawing.Size(192, 454);
            this.nickPanel.TabIndex = 0;
            // 
            // serverListTab
            // 
            this.serverListTab.Controls.Add(this.serverPanel);
            this.serverListTab.Location = new System.Drawing.Point(24, 4);
            this.serverListTab.Name = "serverListTab";
            this.serverListTab.Size = new System.Drawing.Size(172, 454);
            this.serverListTab.TabIndex = 0;
            this.serverListTab.Text = "Favorite Servers";
            // 
            // serverPanel
            // 
            this.serverPanel.Controls.Add(this.serverTree);
            this.serverPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverPanel.Location = new System.Drawing.Point(0, 0);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(172, 454);
            this.serverPanel.TabIndex = 0;
            // 
            // serverTree
            // 
            this.serverTree.AccessibleDescription = "List of servers and channels associated with them once connected";
            this.serverTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.serverTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverTree.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverTree.Location = new System.Drawing.Point(0, 0);
            this.serverTree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.serverTree.Name = "serverTree";
            this.serverTree.Size = new System.Drawing.Size(172, 454);
            this.serverTree.TabIndex = 0;
            // 
            // mainTabControl
            // 
            this.mainTabControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(3, 63);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.Size = new System.Drawing.Size(918, 462);
            this.mainTabControl.TabIndex = 20;
            // 
            // panelDockRight
            // 
            this.panelDockRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDockRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDockRight.Location = new System.Drawing.Point(704, 94);
            this.panelDockRight.Name = "panelDockRight";
            this.panelDockRight.Size = new System.Drawing.Size(220, 431);
            this.panelDockRight.TabIndex = 14;
            // 
            // panelDockLeft
            // 
            this.panelDockLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDockLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDockLeft.Location = new System.Drawing.Point(0, 94);
            this.panelDockLeft.Name = "panelDockLeft";
            this.panelDockLeft.Size = new System.Drawing.Size(200, 431);
            this.panelDockLeft.TabIndex = 13;
            // 
            // inputPanel
            // 
            this.inputPanel.AccessibleDescription = "";
            this.inputPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputPanel.Location = new System.Drawing.Point(0, 525);
            this.inputPanel.Margin = new System.Windows.Forms.Padding(4);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(924, 26);
            this.inputPanel.TabIndex = 0;
            // 
            // mainChannelBar
            // 
            this.mainChannelBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainChannelBar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainChannelBar.Location = new System.Drawing.Point(0, 63);
            this.mainChannelBar.Name = "mainChannelBar";
            this.mainChannelBar.SelectedIndex = -1;
            this.mainChannelBar.Size = new System.Drawing.Size(924, 31);
            this.mainChannelBar.TabIndex = 24;            // 
            //
            // FormMain
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(924, 576);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.splitterRight);
            this.Controls.Add(this.splitterLeft);
            this.Controls.Add(this.panelDockRight);
            this.Controls.Add(this.panelDockLeft);
            this.Controls.Add(this.inputPanel);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.mainChannelBar);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuMainStrip);
            this.Controls.Add(this.splitterBottom);
            this.MainMenuStrip = this.menuMainStrip;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "IceChat";
            this.menuMainStrip.ResumeLayout(false);
            this.menuMainStrip.PerformLayout();
            this.contextMenuNotify.ResumeLayout(false);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.contextMenuToolBar.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.nickListTab.ResumeLayout(false);
            this.serverListTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.ToolStripMenuItem debugWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codePlexPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iceChatHomePageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotify;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nickListToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripAway;
        private System.Windows.Forms.ToolStripMenuItem statusBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facebookFanPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem browseDataFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browsePluginsFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem closeCurrentWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem selectNickListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectServerTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem channelBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripUpdate;
        private System.Windows.Forms.ToolStripMenuItem muteAllSoundsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAPluginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.Splitter splitterBottom;
        public System.Windows.Forms.ToolStripMenuItem multilineEditboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStylesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vS2008ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem office2007ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultToolStripMenuItem1;


        //added
        private System.Windows.Forms.ToolStripMenuItem nickListImageMenu;
        private System.Windows.Forms.ToolStripMenuItem serverTreeImageMenu;
        private System.Windows.Forms.ToolStripMenuItem nickListImageRemoveMenu;
        private System.Windows.Forms.ToolStripMenuItem serverTreeImageRemoveMenu;


        internal System.Windows.Forms.Splitter splitterLeft;
        internal System.Windows.Forms.Splitter splitterRight;

        private IceDockPanel panelDockLeft;
        private IceDockPanel panelDockRight;
        private ServerTree serverTree;
        private ChannelList channelList;
        private BuddyList buddyList;
        private NickList nickList;

        private Panel serverPanel;
        private Panel nickPanel;

        private TabPage nickListTab;
        private TabPage serverListTab;
        private TabPage channelListTab;
        private TabPage buddyListTab;
        private ToolStripMenuItem closeWindow;
        private ChannelBar mainChannelBar;
        private ToolStripButton toolStripFonts;
        internal ToolStripMenuItem fontSettingsToolStripMenuItem;
        private ToolStripMenuItem resizeWindowToolStripMenuItem;
        private ToolStripMenuItem windowsToolStripMenuItem;
        private ToolStripMenuItem cascadeToolStripMenuItem;
        private ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private ToolStripMenuItem tileVerticalToolStripMenuItem;
        private ToolStripMenuItem browseLogsFolderToolStripMenuItem;
        private ToolStripMenuItem showButtonsNickListToolStripMenuItem;
        private ToolStripMenuItem showButtonsServerTreeToolStripMenuItem1;
        private ToolStripMenuItem alwaysOnTopToolStripMenuItem;
        private ToolStripMenuItem viewChannelBarToolStripMenuItem;
        private ToolStripMenuItem saveTabOrderToolStripMenuItem;
        private ToolStripMenuItem restoreTabOrderToolStripMenuItem;
        private ToolStripMenuItem iceChatWikiToolStripMenuItem;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripMenuItem searchForChannelsToolStripMenuItem;
        private ToolStripMenuItem searchForNetworksToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem exportToolStripMenuItem;
        private ToolStripMenuItem fixWindowSizeToolStripMenuItem;
        private ToolStripMenuItem commandsToolStripMenuItem;
        private ToolStripMenuItem aliasesToolStripMenuItem;
        private ToolStripMenuItem identifiersToolStripMenuItem;
        private ToolStripMenuItem portableToolStripMenuItem;
        private ToolStripMenuItem buildFromSourceToolStripMenuItem;
        private ToolStripMenuItem updateAvailableToolStripMenuItem1;

    }

}

