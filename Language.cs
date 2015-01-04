using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IceChat
{
    [XmlRoot("IceChatLanguage")]
    public class LanguageItem
    {
        private string _languageFile;
        private string _languageName;

        public LanguageItem()
        {
            LanguageFile = "";
            LanguageName = "English";
        }

        public string LanguageFile
        {
            get { return this._languageFile; }
            set { this._languageFile = value; }
        }

        [XmlElement("Name")]
        public string LanguageName
        {
            get { return this._languageName; }
            set { this._languageName = value; }
        }

        
        public override string ToString()
        {
            return this._languageName;
        }
    }


    [XmlRoot("IceChatLanguage")]
    public class IceChatLanguage
    {
        public IceChatLanguage()
        {
            LanguageName = "English";

            // menu items
            mainToolStripMenuItem = "Main";
            minimizeToTrayToolStripMenuItem = "Minimize to Tray";
            debugWindowToolStripMenuItem = "Debug Window";
            exitToolStripMenuItem = "Exit";
            optionsToolStripMenuItem = "Options";
            iceChatSettingsToolStripMenuItem = "Program Settings";
            iceChatColorsToolStripMenuItem = "Colors Settings";
            iceChatEditorToolStripMenuItem = "IceChat Editor";
            pluginsToolStripMenuItem = "Loaded Plugins";
            viewToolStripMenuItem = "View";
            serverListToolStripMenuItem = "Server List";
            nickListToolStripMenuItem = "Nick List";
            statusBarToolStripMenuItem = "Status Bar";
            toolBarToolStripMenuItem = "Tool Bar";
            helpToolStripMenuItem = "Help";
            codePlexPageToolStripMenuItem = "CodePlex Page";
            iceChatHomePageToolStripMenuItem = "IceChat Home Page";
            forumsToolStripMenuItem = "Forums";
            aboutToolStripMenuItem = "About";

            // toolbar icons
            toolStripQuickConnect = "Quick Connect";
            toolStripSettings = "Settings";
            toolStripColors = "Colors";
            toolStripEditor = "Editor";
            toolStripAway = "Set Away";
            toolStripSystemTray = "System Tray";

            // status bar messages
            toolStripStatus = "Status";

            // main window gui items
            buttonSend = "Send";
            buttonOk = "&OK";
            buttonCancel = "&Cancel";
            buttonSave = "Save";
            buttonClose = "Close";

            favChanHeader = "Favorite Channels";
            favChanbuttonAdd = "Add";
            favChanbuttonJoin = "Join";
            favChanbuttonEdit = "Edit";
            favChanbuttonRemove = "Remove";

            buddyListHeader = "Buddy List";
            buddyListbuttonMessage = "Message";

            serverTreeHeader = "Favorite Servers";
            serverTreeButtonConnect = "Connect";
            serverTreeButtonEdit = "Edit";
            serverTreeButtonDisconnect = "Disconnect";
            serverTreeButtonAdd = "Add";

            consoleTabWelcome = "Welcome";
            consoleTabTitle = "Console";

            tabPageFaveChannels = "Fave Channels";
            tabPageNicks = "Nick List";

            // quick connect dialog
            quickConnectTitle = "Quick Connect - Add Server Details";
            quickConnectLblServer = "Server Name";
            quickConnectLblNick = "Nick Name";
            quickConnectLblChannel = "Channel";
            quickConnectButtonConnect = "Connect";
            quickConnectErrorNoServer = "Please Enter a Server Name";
            quickConnectErrorNoNick = "Please Enter a Nick Name";

            // about dialog
            aboutText = "IceChat 9 is an IRC (Internet Relay Chat) program. It is used to connect to an unlimited amount of IRC Servers simultaneously and chat in channels with other IRC users.  Version 9 is a complete rewrite of the program , using a new tabbed interface, and written in C#. It is also now Open Source.";
        }

        [XmlElement("Name")]
        public string LanguageName
        { get; set; }

        // menu items strings 
        public string minimizeToTrayToolStripMenuItem
        { get; set; }

        public string mainToolStripMenuItem
        { get; set; }

        public string debugWindowToolStripMenuItem
        { get; set; }

        public string exitToolStripMenuItem
        { get; set; }

        public string optionsToolStripMenuItem
        { get; set; }

        public string iceChatSettingsToolStripMenuItem
        { get; set; }

        public string iceChatColorsToolStripMenuItem
        { get; set; }

        public string iceChatEditorToolStripMenuItem
        { get; set; }

        public string pluginsToolStripMenuItem
        { get; set; }

        public string viewToolStripMenuItem
        { get; set; }

        public string serverListToolStripMenuItem
        { get; set; }

        public string nickListToolStripMenuItem
        { get; set; }

        public string statusBarToolStripMenuItem
        { get; set; }

        public string toolBarToolStripMenuItem
        { get; set; }

        public string helpToolStripMenuItem
        { get; set; }

        public string codePlexPageToolStripMenuItem
        { get; set; }

        public string iceChatHomePageToolStripMenuItem
        { get; set; }

        public string forumsToolStripMenuItem
        { get; set; }

        public string aboutToolStripMenuItem
        { get; set; }

        // toolbar icons
        public string toolStripQuickConnect
        { get; set; }

        public string toolStripSettings
        { get; set; }

        public string toolStripColors
        { get; set; }

        public string toolStripEditor
        { get; set; }

        public string toolStripAway
        { get; set; }

        public string toolStripSystemTray
        { get; set; }

        // status bar messages
        public string toolStripStatus
        { get; set; }

        // main window gui items
        public string buttonSend
        { get; set; }

        public string buttonOk
        { get; set; }

        public string buttonCancel
        { get; set; }

        public string buttonSave
        { get; set; }

        public string buttonClose
        { get; set; }

        public string favChanHeader
        { get; set; }

        public string favChanbuttonAdd
        { get; set; }

        public string favChanbuttonJoin
        { get; set; }

        public string favChanbuttonEdit
        { get; set; }

        public string favChanbuttonRemove
        { get; set; }

        public string buddyListHeader
        { get; set; }

        public string buddyListbuttonMessage
        { get; set; }

        public string serverTreeHeader
        { get; set; }

        public string serverTreeButtonConnect
        { get; set; }

        public string serverTreeButtonEdit
        { get; set; }

        public string serverTreeButtonDisconnect
        { get; set; }

        public string serverTreeButtonAdd
        { get; set; }

        public string consoleTabTitle
        { get; set; }

        public string consoleTabWelcome
        { get; set; }

        public string tabPageFaveChannels
        { get; set; }

        public string tabPageNicks
        { get; set; }

        // quick connect dialog strings
        public string quickConnectLblServer
        { get; set; }

        public string quickConnectLblNick
        { get; set; }

        public string quickConnectLblChannel
        { get; set; }

        public string quickConnectButtonConnect
        { get; set; }

        public string quickConnectTitle
        { get; set; }

        public string quickConnectErrorNoServer
        { get; set; }

        public string quickConnectErrorNoNick
        { get; set; }

        // server settings dialog strings
        public string serverSettingsMainSettings
        { get; set; }

        public string serverSettingsServerName
        { get; set; }

        public string serverSettingsDisplayName
        { get; set; }

        public string serverSettingsNickName
        { get; set; }

        public string serverSettingsIdentName
        { get; set; }

        public string serverSettingsFullName
        { get; set; }

        public string serverSettingsQuitMessage
        { get; set; }

        public string serverSettingsServerPort
        { get; set; }

        public string serverSettingsAltNickName
        { get; set; }

        public string serverSettingsAwayNickName
        { get; set; }

        public string serverSettingsRemoveServer
        { get; set; }

        public string serverSettingsExtraSettings
        { get; set; }

        // about dialog
        public string aboutText
        { get; set; }

    }
}
