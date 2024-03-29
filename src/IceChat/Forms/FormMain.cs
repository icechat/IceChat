/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2023 Paul Vanderzee <snerf@icechat.net>
 *                                    <www.icechat.net> 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
 *
 * Please consult the LICENSE.txt file included with this project for
 * more details
 *
\******************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Xml;
using System.Diagnostics;

using System.IO.Packaging;

using IceChatPlugin;
using IceChatTheme;

//using Newtonsoft.Json;

namespace IceChat
{
    public partial class FormMain : Form
    {
        public static FormMain Instance;

        private string optionsFile;
        private string messagesFile;
        private string fontsFile;
        private string colorsFile;
        private string soundsFile; 
        private string favoriteChannelsFile;
        private string serversFile;
        private string aliasesFile;
        private string popupsFile;
        private string pluginsFile;
        private string channelSettingsFile;
        private string colorPaletteFile;

        private string currentFolder;
        private string logsFolder;
        private string pluginsFolder;
        private string emoticonsFile;
        private string scriptsFolder;
        private string soundsFolder;
        private string picturesFolder;
        private string backupFolder;

        private List<LanguageItem> languageFiles;
        private LanguageItem currentLanguageFile;

        private StreamWriter errorFile;

        private IceChatOptions iceChatOptions;
        private IceChatColors iceChatColors;
        private IceChatMessageFormat iceChatMessages;
        private IceChatFontSetting iceChatFonts;
        private IceChatAliases iceChatAliases;
        private IceChatPopupMenus iceChatPopups;
        private IceChatEmoticon iceChatEmoticons;
        private IceChatLanguage iceChatLanguage;
        private IceChatSounds iceChatSounds;
        private IceChatPluginFile iceChatPlugins;
        private ChannelSettings channelSettings;
        private IceChatColorPalette colorPalette;


        private bool IsForeGround;

        private List<IThemeIceChat> loadedPluginThemes;        
        private List<Plugin> loadedPlugins;

        private IdentServer identServer;

        private delegate IceTabPage AddWindowDelegate(IRCConnection connection, string windowName, IceTabPage.WindowType windowType);

        private delegate void CurrentWindowDelegate(string data, int color);
        private delegate void WindowMessageDelegate(IRCConnection connection, string name, string data, string timeStamp, bool scrollToBottom);
        private delegate void CurrentWindowMessageDelegate(IRCConnection connection, string data, string timeStamp, bool scrollToBottom);
        private delegate void AddInputpanelTextDelegate(string data);
        private delegate void AddPanelDelegate(Panel panel);

        private System.Timers.Timer flashTrayIconTimer;
        private int flashTrayCount;

        private System.Timers.Timer flashTaskBarIconTimer;
        private int flashTaskBarCount;

        private System.Media.SoundPlayer player;
        private MP3Player mp3Player;

        private bool muteAllSounds;
        private string autoStartCommand = null;
        private bool disableAutoStart = false;
        private bool finishedLoading = false;
        private bool allowClose = false;
        private bool askedClose = false;

        private FormWindowState previousWindowState;

        private ToolStripRenderer menuRenderer = null;
        private ToolStripRenderer toolStripRender = null;

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public Int32 dwFlags;
            public UInt32 uCount;
            public Int32 dwTimeout;
        }

        [DllImport("user32.dll")]
        private static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

        private const long BUFFER_SIZE = 4096;

        public const string ProgramID = "IceChat";
        public const string VersionID = "9.54";
        public const string BuildDate = "Jun 1 2023";
        
        public string BuildNumber = ""; //this gets auto filled with the version # from assembly
        
        private List<string> errorMessages;
        private Variables _variables;
        private List<IrcTimer> _globalTimers;
        private string _args;
        internal List<string> loadErrors = new List<string>();

        public Stack<KeyValuePair<string, IRCConnection>> GlobalLastChannels;

        /// <summary>
        /// All the Window Message Types used for Coloring the Tab Text for Different Events
        /// </summary>
        internal enum ServerMessageType
        {
            Default,
            Message = 1,            
            Action = 2,
            JoinChannel = 3,
            PartChannel = 4,
            QuitServer = 5,
            ServerMessage = 6,
            Other = 7,
            ServerNotice = 8,
            BuddyNotice = 9,
            CustomMessage = 10  // allow for a custom color
        }

        public FormMain(string[] args, Form splash)
        {
            FormMain.Instance = this;
            
            this.GlobalLastChannels = new Stack<KeyValuePair<string, IRCConnection>>();

            System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            BuildNumber = fv.FileVersion;

            if (StaticMethods.IsRunningOnMono())
                player = new System.Media.SoundPlayer();
            else
                mp3Player = new MP3Player();

            bool forceCurrentFolder = false;
            errorMessages = new List<string>();

            _variables = new Variables();
            _globalTimers = new List<IrcTimer>();

            string downloadPluginUrl = "";
            string downloadPluginName = "";

            if (args.Length > 0)
            {
                string prevArg = "";
                foreach (string arg in args)
                {                    
                    if (prevArg.Length == 0)
                    {
                        prevArg = arg;

                        if (arg.StartsWith("icechatplugin://"))
                        {
                            // icechatplugin://icechat.net/plugin/35/GooglePlugin

                            // get the URL of the plugin
                            // needs to be from icechat.net
                            string url = arg.Replace("icechatplugin://", "");

                            if (url.ToLower().StartsWith("icechat.net/plugin/"))
                            {
                                //https://www.icechat.net/site/download.php?did=17

                                string[] pluginURL = url.TrimStart("icechat.net/plugin/".ToCharArray()).Split('/');  // 17/AutoAwayPlugin
                                if (pluginURL.Length == 2)
                                {
                                    downloadPluginUrl = "https://www.icechat.net/site/download.php?did=" + pluginURL[0];
                                    
                                    downloadPluginName = pluginURL[1];

                                }
                            }

                        }

                        if (arg.ToLower().StartsWith("irc://"))
                        {
                            //parse out the server name and channel name
                            string server = arg.Substring(6).TrimEnd();
                            if (server.IndexOf("/") != -1)
                            {
                                string host = server.Split('/')[0];
                                string channel = server.Split('/')[1];
                                if (channel.StartsWith("#"))
                                    autoStartCommand = "/joinserv " + host + " " + channel;
                                else
                                    autoStartCommand = "/joinserv " + host + " #" + channel;

                            }
                            else
                                autoStartCommand = "/server " + arg.Substring(6).TrimEnd();

                        }
                        if (arg.ToLower() == "-disableauto")
                        {
                            disableAutoStart = true;
                        }
                    }
                    else
                    {
                        switch (prevArg.ToLower())
                        {
                            case "-profile":
                                //check if the folder exists, if not, create it
                                // make sure we have access to this folder
                                try
                                {
                                    // Attempt to get a list of security permissions from the folder. 
                                    // This will raise an exception if the path is read only or do not have access to view the permissions. 
                                    System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(arg);

                                    currentFolder = arg;

                                    if (!Directory.Exists(currentFolder))
                                        Directory.CreateDirectory(currentFolder);

                                    forceCurrentFolder = true;


                                }
                                catch (Exception)
                                {
                                    // we dol not have access to the folder
                                }

                                break;
                            case "-disableauto":
                                disableAutoStart = true;
                                break;
                        }

                        prevArg = "";
                    }
                }
            }

            #region Settings Files 


            //check if the xml settings files exist in current folder
            if (currentFolder == null)
                currentFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //check for IceChatPackage.xml in Assembly Folder
            // not this far


            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatPackage.xml"))
            {
                //read the package file
                //create the IceChatServer.xml file from the package, if it doesnt exist
                
                serversFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "Settings" + System.IO.Path.DirectorySeparatorChar + "IceChatServer.xml";
                optionsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "Settings" + System.IO.Path.DirectorySeparatorChar + "IceChatOptions.xml";

                if (!File.Exists(serversFile))
                {
                    if (!Directory.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Settings"))
                        Directory.CreateDirectory(currentFolder + System.IO.Path.DirectorySeparatorChar + "Settings");

                    //ask for a Default Nickname
                    InputBoxDialog i = new InputBoxDialog
                    {
                        FormCaption = "Enter Default Nickname",
                        FormPrompt = "Please enter your Nick name"
                    };

                    i.ShowDialog();
                    if (i.InputResponse.Length > 0)
                    {
                        //changedData[count] = i.InputResponse;

                        System.Diagnostics.Debug.WriteLine("Package Exists - Create Defaults");
                        XmlSerializer deserializer = new XmlSerializer(typeof(IceChatPackage));
                        TextReader textReader = new StreamReader(currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatPackage.xml");
                        IceChatPackage package = (IceChatPackage)deserializer.Deserialize(textReader);
                        textReader.Close();
                        textReader.Dispose();

                        IceChatServers servers = new IceChatServers();
                        foreach (ServerSetting s in package.Servers)
                        {
                            s.NickName = i.InputResponse;
                            s.AltNickName = s.NickName + "_";
                            s.AwayNickName = s.NickName + "[A]";
                            
                            servers.AddServer(s);
                        }

                        //save the server(s) to IceChatServer.xml
                        XmlSerializer serializer = new XmlSerializer(typeof(IceChatServers));
                        TextWriter textWriter = new StreamWriter(serversFile);
                        serializer.Serialize(textWriter, servers);
                        textWriter.Close();
                        textWriter.Dispose();

                        servers.listServers.Clear();
                        serializer = null;
                        textWriter = null;

                        //load the options and save
                        IceChatOptions options = package.Options;

                        serializer = new XmlSerializer(typeof(IceChatOptions));
                        textWriter = new StreamWriter(optionsFile);
                        serializer.Serialize(textWriter, options);
                        textWriter.Close();
                        textWriter.Dispose();

                        currentFolder += System.IO.Path.DirectorySeparatorChar + "Settings";

                    }
                    i.Dispose();

                    //change the currentFolder
                }
            }
            //check for Settings/IceChatServer.xml
            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Settings" + System.IO.Path.DirectorySeparatorChar + "IceChatServer.xml"))
            {
                currentFolder += System.IO.Path.DirectorySeparatorChar + "Settings";            
            }
            else if (!File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatServer.xml") && !forceCurrentFolder)
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "IceChat Networks" + Path.DirectorySeparatorChar + "IceChat"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "IceChat Networks" + Path.DirectorySeparatorChar + "IceChat");

                currentFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "IceChat Networks" + Path.DirectorySeparatorChar + "IceChat";
            }

            //load all files from the Local AppData folder, unless it exist in the current folder
            serversFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatServer.xml";
            optionsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatOptions.xml";
            messagesFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatMessages.xml";
            fontsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatFonts.xml";
            colorsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatColors.xml";
            soundsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatSounds.xml";
            favoriteChannelsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatChannels.xml";
            aliasesFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatAliases.xml";
            popupsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatPopups.xml";
            pluginsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatPlugins.xml";
            emoticonsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "Emoticons" + System.IO.Path.DirectorySeparatorChar + "IceChatEmoticons.xml";
            channelSettingsFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "ChannelSetting.xml";
            colorPaletteFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "ColorPalette.xml";
            
            //set a new logs folder            
            logsFolder = currentFolder + System.IO.Path.DirectorySeparatorChar + "Logs";
            scriptsFolder = currentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts";
            soundsFolder = currentFolder + System.IO.Path.DirectorySeparatorChar + "Sounds";
            picturesFolder = currentFolder + System.IO.Path.DirectorySeparatorChar + "Pictures";
            backupFolder = currentFolder + System.IO.Path.DirectorySeparatorChar + "Backups";

            pluginsFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + System.IO.Path.DirectorySeparatorChar + "Plugins";
                        
            if (!Directory.Exists(pluginsFolder))
                Directory.CreateDirectory(pluginsFolder);

            if (!Directory.Exists(scriptsFolder))
                Directory.CreateDirectory(scriptsFolder);

            if (!Directory.Exists(soundsFolder))
                Directory.CreateDirectory(soundsFolder);

            if (!Directory.Exists(picturesFolder))
                Directory.CreateDirectory(picturesFolder);

            if (!Directory.Exists(currentFolder + Path.DirectorySeparatorChar + "Update"))
                Directory.CreateDirectory(currentFolder + Path.DirectorySeparatorChar + "Update");

            if (!Directory.Exists(backupFolder))
                Directory.CreateDirectory(backupFolder);

            #endregion

            languageFiles = new List<LanguageItem>();
            
            DirectoryInfo languageDirectory = null;

            languageDirectory = new DirectoryInfo(currentFolder + System.IO.Path.DirectorySeparatorChar + "Languages");
            if (!Directory.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Languages"))
                Directory.CreateDirectory(currentFolder + System.IO.Path.DirectorySeparatorChar + "Languages");
            
            if (languageDirectory != null)
            {
                // scan the language directory for xml files and make LanguageItems for each file
                FileInfo[] langFiles = languageDirectory.GetFiles("*.xml");
                foreach (FileInfo fi in langFiles)
                {                    
                    string langFile = languageDirectory.FullName + System.IO.Path.DirectorySeparatorChar + fi.Name;
                    LanguageItem languageItem = LoadLanguageItem(langFile);
                    if (languageItem != null) languageFiles.Add(languageItem);                    
                }

                if (languageFiles.Count == 0)
                {
                    currentLanguageFile = new LanguageItem();
                    languageFiles.Add(currentLanguageFile);     // default language English
                }
            }

            //load the color palette
            LoadColorPalette();

            LoadOptions();
            LoadColors();
            LoadSounds();

            // use the language saved in options if available,
            // if not (e.g. user deleted xml file) default is used
            foreach (LanguageItem li in languageFiles)
            {
                if (li.LanguageName == iceChatOptions.Language)
                {
                    currentLanguageFile = li;
                    break;
                }
            }

            LoadLanguage(); // The language class MUST be loaded before any GUI component is created

            //set the new log folder
            if (iceChatOptions.LogFolder.Length > 0)
            {
                // check if this folder exists?
                if (Directory.Exists(iceChatOptions.LogFolder))
                {
                    logsFolder = iceChatOptions.LogFolder;
                }
                else
                {
                    // folder does not exist.. reset to default
                    iceChatOptions.LogFolder = logsFolder;
                }
            }
            else
            {
                iceChatOptions.LogFolder = logsFolder;
            }

            //check if we have any servers/settings saved, if not, load firstrun
            if (!File.Exists(serversFile))
            {
                FormFirstRun firstRun = new FormFirstRun(currentFolder);
                firstRun.SaveOptions += new FormFirstRun.SaveOptionsDelegate(FirstRunSaveOptions);
                firstRun.ShowDialog(this);
            }

            InitializeMainForm();

            InitializeComponent();

            //load icons from Embedded Resources or Pictures Image
            this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-tray-icon.ico").GetHicon());            
            if (iceChatOptions.SystemTrayIcon == null || iceChatOptions.SystemTrayIcon.Trim().Length == 0)
            {
                this.notifyIcon.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-tray-icon.ico").GetHicon());
            }
            else
            {
                //make sure the image exists and is an ICO file                
                if (File.Exists(iceChatOptions.SystemTrayIcon))
                    this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(iceChatOptions.SystemTrayIcon);
                else
                    this.notifyIcon.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-tray-icon.ico").GetHicon());
            }
            
            this.notifyIcon.Visible = iceChatOptions.ShowSytemTrayIcon;

            //disable this by default
            this.toolStripUpdate.Visible = false;
            this.updateAvailableToolStripMenuItem1.Visible = false;
            
            serverListToolStripMenuItem.Checked = iceChatOptions.ShowServerTree;
            panelDockLeft.Visible = serverListToolStripMenuItem.Checked;
            splitterLeft.Visible = serverListToolStripMenuItem.Checked;

            nickListToolStripMenuItem.Checked = iceChatOptions.ShowNickList;
            panelDockRight.Visible = nickListToolStripMenuItem.Checked;
            panelDockRight.TabControl.Alignment = TabAlignment.Right;
            splitterRight.Visible = nickListToolStripMenuItem.Checked;
            
            statusBarToolStripMenuItem.Checked = iceChatOptions.ShowStatusBar;
            statusStripMain.Visible = statusBarToolStripMenuItem.Checked;

            toolBarToolStripMenuItem.Checked = iceChatOptions.ShowToolBar;
            toolStripMain.Visible = toolBarToolStripMenuItem.Checked;            

            viewChannelBarToolStripMenuItem.Checked = iceChatOptions.ShowTabBar;

            if (iceChatOptions.ShowTabBar == false)
            {
                mainChannelBar.HideBar = true;
            }
            mainChannelBar.SingleRow = iceChatOptions.SingleRowTabBar;

            this.Text = ProgramID + " " + VersionID;

            //this can be customized            
            if (iceChatOptions.SystemTrayText == null || iceChatOptions.SystemTrayText.Trim().Length == 0)
                this.notifyIcon.Text = ProgramID + " " + VersionID;
            else
                this.notifyIcon.Text = iceChatOptions.SystemTrayText;
            
            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);

            try
            {
                errorFile = new StreamWriter(logsFolder + System.IO.Path.DirectorySeparatorChar + "errors.log", true);
            }
            catch (IOException io)
            {
                System.Diagnostics.Debug.WriteLine("Can not create errors.log:" + io.Message);
            }
            catch (Exception eo)
            {
                System.Diagnostics.Debug.WriteLine("Can not create errors.log:" + eo.Message);
            }

            if (iceChatOptions.WindowSize != null)
            {
                if (iceChatOptions.WindowSize.Width > 100 && iceChatOptions.WindowSize.Height > 100)
                {
                    this.Size = iceChatOptions.WindowSize;
                    this.WindowState = iceChatOptions.WindowState;
                }
                else
                {
                    this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                    this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                }
            }
            else
            {
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            }

            if (iceChatOptions.WindowLocation != null)
            {
                //check if the location is valid, could try and place it on a 2nd screen that no longer exists
                if (Screen.AllScreens.Length == 1)
                {
                   if (Screen.PrimaryScreen.Bounds.Contains(iceChatOptions.WindowLocation)) 
                        this.Location = iceChatOptions.WindowLocation;

                }
                else
                {
                    //check if we are in the bounds of the screen location
                    foreach (Screen screen in Screen.AllScreens)
                        if (screen.Bounds.Contains(iceChatOptions.WindowLocation))
                            this.Location = iceChatOptions.WindowLocation;                            
                }                
            }
            
            statusStripMain.Visible = iceChatOptions.ShowStatusBar;

            LoadAliases();
            LoadPopups();
            LoadEmoticons();
            LoadMessageFormat();
            LoadFonts();

            bool fileThemeFound = true;

            if (iceChatOptions.CurrentTheme == null)
            {
                iceChatOptions.CurrentTheme = "Default";
                defaultToolStripMenuItem.Checked = true;

                //reload all the theme files

            }
            else
            {
                //load in the new color theme, if it not Default
                if (iceChatOptions.CurrentTheme != "Default")
                {
                    string themeFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "Colors-" + iceChatOptions.CurrentTheme + ".xml";
                    if (File.Exists(themeFile))
                    {
                        XmlSerializer deserializer = new XmlSerializer(typeof(IceChatColors));
                        TextReader textReader = new StreamReader(themeFile);
                        iceChatColors = (IceChatColors)deserializer.Deserialize(textReader);
                        textReader.Close();
                        textReader.Dispose();
                        
                        colorsFile = themeFile;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Color Theme File not found:" + themeFile);
                        fileThemeFound = false;
                    }

                    themeFile = currentFolder + System.IO.Path.DirectorySeparatorChar + "Messages-" + iceChatOptions.CurrentTheme + ".xml";
                    if (File.Exists(themeFile))
                    {
                        XmlSerializer deserializer = new XmlSerializer(typeof(IceChatMessageFormat));
                        TextReader textReader = new StreamReader(themeFile);
                        iceChatMessages = (IceChatMessageFormat)deserializer.Deserialize(textReader);
                        textReader.Close();
                        textReader.Dispose();

                        messagesFile = themeFile;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Messages Theme File not found:" + themeFile);
                        fileThemeFound = false;
                    }
                }
                else
                    defaultToolStripMenuItem.Checked = true;

            }

            if (iceChatOptions.Theme == null)
            {
                defaultToolStripMenuItem.Checked = true;
                iceChatOptions.CurrentTheme = "Default";

                DirectoryInfo _currentFolder = new DirectoryInfo(currentFolder);
                FileInfo[] xmlFiles = _currentFolder.GetFiles("*.xml");

                int totalThemes = 1;
                foreach (FileInfo fi in xmlFiles)
                {
                    if (fi.Name.StartsWith("Colors-"))
                    {
                        totalThemes++;
                    }
                }

                iceChatOptions.Theme = new ThemeItem[totalThemes];

                iceChatOptions.Theme[0] = new ThemeItem
                {
                    ThemeName = "Default",
                    ThemeType = "XML"
                };

                int t = 1;
                foreach (FileInfo fi in xmlFiles)
                {
                    if (fi.Name.StartsWith("Colors-"))
                    {
                        string themeName = fi.Name.Replace("Colors-", "").Replace(".xml", ""); ;
                        iceChatOptions.Theme[t] = new ThemeItem
                        {
                            ThemeName = themeName,
                            ThemeType = "XML"
                        };
                        t++;
                    }
                }
            }

            channelList = new ChannelList(this)
            {
                Dock = DockStyle.Fill
            };
            buddyList = new BuddyList(this)
            {
                Dock = DockStyle.Fill
            };

            toolStripMain.BackColor = IrcColor.colors[iceChatColors.ToolbarBackColor];
            statusStripMain.BackColor = IrcColor.colors[iceChatColors.StatusbarBackColor];
            toolStripStatus.ForeColor = IrcColor.colors[iceChatColors.StatusbarForeColor];
            menuMainStrip.BackColor = IrcColor.colors[iceChatColors.MenubarBackColor];

            splitterLeft.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];
            splitterRight.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];
            splitterBottom.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];

            inputPanel.SetInputBoxColors();
            channelList.SetListColors();
            buddyList.SetListColors();
            serverTree.SetListColors();
            nickList.SetListColors();

            this.nickList.Header = iceChatLanguage.consoleTabTitle;

            nickListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            nickListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];            

            serverListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            serverListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];

            channelListTab = new TabPage("Favorite Channels");
            Panel channelPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            channelPanel.Controls.Add(channelList);
            channelListTab.Controls.Add(channelPanel);
            channelListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            channelListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];

            buddyListTab = new TabPage("Buddy List");
            Panel buddyPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            buddyPanel.Controls.Add(buddyList);
            buddyListTab.Controls.Add(buddyPanel);
            buddyListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            buddyListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];

            panelDockLeft.Width = iceChatOptions.LeftPanelWidth;
            panelDockRight.Width = iceChatOptions.RightPanelWidth;

            //Load the panel items in order
            if (iceChatOptions.LeftPanels != null)
            {
                foreach (string arrayitem in iceChatOptions.LeftPanels)
                {
                    if (arrayitem == serverListTab.Text)
                        this.panelDockLeft.TabControl.TabPages.Add(serverListTab);
                    else if (arrayitem == channelListTab.Text)
                        this.panelDockLeft.TabControl.TabPages.Add(channelListTab);
                    else if (arrayitem == nickListTab.Text)
                        this.panelDockLeft.TabControl.TabPages.Add(nickListTab);
                    else if (arrayitem == buddyListTab.Text)
                        this.panelDockLeft.TabControl.TabPages.Add(buddyListTab);
                }
            }
            if (iceChatOptions.RightPanels != null)
            {
                foreach (string arrayitem in iceChatOptions.RightPanels)
                {
                    if (arrayitem == serverListTab.Text)
                        this.panelDockRight.TabControl.TabPages.Add(serverListTab);
                    else if (arrayitem == nickListTab.Text)
                        this.panelDockRight.TabControl.TabPages.Add(nickListTab);
                    else if (arrayitem == channelListTab.Text)
                        this.panelDockRight.TabControl.TabPages.Add(channelListTab);
                    else if (arrayitem == buddyListTab.Text)
                        this.panelDockRight.TabControl.TabPages.Add(buddyListTab);
                }
            }

            //If any panels are missing
            if (!panelDockLeft.TabControl.TabPages.Contains(serverListTab) && !panelDockRight.TabControl.TabPages.Contains(serverListTab))
                this.panelDockLeft.TabControl.TabPages.Add(serverListTab);
            if (!panelDockLeft.TabControl.TabPages.Contains(nickListTab) && !panelDockRight.TabControl.TabPages.Contains(nickListTab))
                this.panelDockRight.TabControl.TabPages.Add(nickListTab);
            if (!panelDockLeft.TabControl.TabPages.Contains(channelListTab) && !panelDockRight.TabControl.TabPages.Contains(channelListTab))
                this.panelDockRight.TabControl.TabPages.Add(channelListTab);
            if (!panelDockLeft.TabControl.TabPages.Contains(buddyListTab) && !panelDockRight.TabControl.TabPages.Contains(buddyListTab))
                this.panelDockRight.TabControl.TabPages.Add(buddyListTab);

            this.MinimumSize = new Size(panelDockLeft.Width + panelDockRight.Width + 300, 300);

            //hide the left or right panel if it is empty
            if (panelDockLeft.TabControl.TabPages.Count == 0)
            {
                this.splitterLeft.Visible = false;
                panelDockLeft.Visible = false;
                this.MinimumSize = new Size(panelDockRight.Width + 300, 300);
            }
            if (panelDockRight.TabControl.TabPages.Count == 0)
            {
                this.splitterRight.Visible = false;
                panelDockRight.Visible = false;
                if (panelDockLeft.Visible)
                    this.MinimumSize = new Size(panelDockLeft.Width + 300, 300);
                else
                    this.MinimumSize = new Size(300, 300);
            }

            if (iceChatOptions.LockWindowSize)
            {
                fixWindowSizeToolStripMenuItem.Checked = true;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            }

            nickList.Font = new Font(iceChatFonts.FontSettings[3].FontName, iceChatFonts.FontSettings[3].FontSize);
            serverTree.Font = new Font(iceChatFonts.FontSettings[4].FontName, iceChatFonts.FontSettings[4].FontSize);
            mainChannelBar.TabFont = new Font(iceChatFonts.FontSettings[8].FontName, iceChatFonts.FontSettings[8].FontSize);
            menuMainStrip.Font = new Font(iceChatFonts.FontSettings[7].FontName, iceChatFonts.FontSettings[7].FontSize);
            toolStripMain.Font = new Font(iceChatFonts.FontSettings[7].FontName, iceChatFonts.FontSettings[7].FontSize);

            inputPanel.OnCommand +=new InputPanel.OnCommandDelegate(InputPanel_OnCommand);
            inputPanel.OnHotKey += new InputPanel.OnHotKeyDelegate(InputPanel_OnHotKey);

            inputPanel.InputBoxFont = new Font(iceChatFonts.FontSettings[5].FontName, iceChatFonts.FontSettings[5].FontSize);

            inputPanel.ShowColorPicker = iceChatOptions.ShowColorPicker;
            inputPanel.ShowEmoticonPicker = iceChatOptions.ShowEmoticonPicker;
            inputPanel.ShowBasicCommands = iceChatOptions.ShowBasicCommands;
            inputPanel.ShowSendButton = iceChatOptions.ShowSendButton;

            //inputPanel.ShowWideTextPanel = iceChatOptions.ShowMultilineEditbox;

            mainChannelBar.OnTabClosed += new ChannelBar.TabClosedDelegate(OnTabClosed);
            mainChannelBar.SelectedIndexChanged += new ChannelBar.TabEventHandler(OnTabSelectedIndexChanged);

            panelDockLeft.Initialize();
            panelDockRight.Initialize();

            if (iceChatOptions.DockLeftPanel == true)
                panelDockLeft.DockControl();

            if (iceChatOptions.DockRightPanel == true)
                panelDockRight.DockControl();

            LoadChannelSettings();
            
            CreateDefaultConsoleWindow();

            //****
            WindowMessage(null, "Console", "\x000304Data Folder: " + currentFolder, "", true);
            WindowMessage(null, "Console", "\x000304Plugins Folder: " + pluginsFolder, "", true);
            WindowMessage(null, "Console", "\x000304Logs Folder: " + logsFolder, "", true);

            serverTree.NewServerConnection += new NewServerConnectionDelegate(NewServerConnection);
            serverTree.SaveDefault += new ServerTree.SaveDefaultDelegate(OnDefaultServerSettings);

            loadedPluginThemes = new List<IThemeIceChat>();            
            loadedPlugins = new List<Plugin>();
            


            //load the plugin settings file
            LoadPluginFiles();

            // do we have a new Plugin to Download?
            if (downloadPluginUrl.Length > 0)
            {
                DownloadPlugin(downloadPluginUrl, downloadPluginName);
            }

    
            //load any plugin addons
            LoadPlugins();

            //****
            WindowMessage(null, "Console", "\x00034Using Theme - " + iceChatOptions.CurrentTheme, "", true);

            //set any plugins as disabled
            //add any items to the pluginsFile if they do not exist, or remove any that do not
            
            foreach (Plugin p in loadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
                if (ipc != null)
                {
                    bool found = false;
                    for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
                    {
                        if (iceChatPlugins.listPlugins[i].PluginFile.Equals(ipc.plugin.FileName))
                        {
                            found = true;

                            if (iceChatPlugins.listPlugins[i].Enabled == false)
                            {
                                WindowMessage(null, "Console", "\x000304Disabled Plugin - " + ipc.plugin.Name + " v" + ipc.plugin.Version, "", true);

                                foreach (ToolStripMenuItem t in pluginsToolStripMenuItem.DropDownItems)
                                    if (t.ToolTipText.ToLower() == ipc.plugin.FileName.ToLower())
                                        t.Image = StaticMethods.LoadResourceImage("CloseButton.png");

                                ipc.plugin.Enabled = false;
                            }

                        }
                    }
                    if (found == false)
                    {
                        //plugin file not found in plugin Items file, add it
                        PluginItem item = new PluginItem
                        {
                            Enabled = true,
                            PluginFile = ipc.plugin.FileName
                        };
                        iceChatPlugins.AddPlugin(item);
                        SavePluginFiles();
                    }
                }

                if (iceChatOptions.Transparency >= 25)
                    FormMain.Instance.Opacity = (double)iceChatOptions.Transparency / 100;
             
            }
            

            if (iceChatPlugins != null && iceChatPlugins.listPlugins.Count != loadedPlugins.Count)
            {
                //find the file that is missing
                List<int> removeItems = new List<int>();
                for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
                {
                    bool found = false;
                    foreach (Plugin p in loadedPlugins)
                    {
                        IceChatPlugin ipc = p as IceChatPlugin;
                        if (ipc != null)
                        {

                            if (iceChatPlugins.listPlugins[i].PluginFile.Equals(ipc.plugin.FileName))
                                found = true;
                        }
                    }

                    if (found == false)
                        removeItems.Add(i);
                }

                if (removeItems.Count > 0)
                {
                    try
                    {
                        foreach (int i in removeItems)
                            iceChatPlugins.listPlugins.Remove(iceChatPlugins.listPlugins[i]);
                    }
                    catch { }

                    SavePluginFiles();
                }
            }


            //initialize each of the plugins on its own thread
            foreach (Plugin p in loadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
                if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                    {
                        System.Threading.Thread initPlugin = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InitializePlugin));
                        initPlugin.Start(ipc.plugin);
                    }
                }
            }

            foreach (string s in errorMessages)
            {
                WindowMessage(null, "Console", "\x000304Error: " + s,"", true);
            }
            errorMessages.Clear();
            
            pluginsToolStripMenuItem.DropDownOpening += new EventHandler(PluginsToolStripMenuItem_DropDownOpening);

            if (fileThemeFound == false)
            {
                //check for the Plugin File theme
                foreach (IThemeIceChat theme in IceChatPluginThemes)
                {
                    if (theme.Name == iceChatOptions.CurrentTheme)
                    {
                        //update, this is the match
                        iceChatColors.ChannelAdminColor = theme.ChannelAdminColor;
                        iceChatColors.ChannelBackColor = theme.ChannelBackColor;
                        iceChatColors.ChannelHalfOpColor = theme.ChannelHalfOpColor;
                        iceChatColors.ChannelJoinColorChange = theme.ChannelJoinColorChange;
                        iceChatColors.ChannelListBackColor = theme.ChannelListBackColor;
                        iceChatColors.ChannelListForeColor = theme.ChannelListForeColor;
                        iceChatColors.ChannelOpColor = theme.ChannelOpColor;
                        iceChatColors.ChannelOwnerColor = theme.ChannelOwnerColor;
                        iceChatColors.ChannelPartColorChange = theme.ChannelPartColorChange;
                        iceChatColors.ChannelRegularColor = theme.ChannelRegularColor;
                        iceChatColors.ChannelVoiceColor = theme.ChannelVoiceColor;
                        iceChatColors.ConsoleBackColor = theme.ConsoleBackColor;
                        iceChatColors.InputboxBackColor = theme.InputboxBackColor;
                        iceChatColors.InputboxForeColor = theme.InputboxForeColor;
                        iceChatColors.MenubarBackColor = theme.MenubarBackColor;
                        iceChatColors.NewMessageColorChange = theme.NewMessageColorChange;
                        iceChatColors.NickListBackColor = theme.NickListBackColor;
                        iceChatColors.OtherMessageColorChange = theme.OtherMessageColorChange;
                        iceChatColors.PanelHeaderBG1 = theme.PanelHeaderBG1;
                        iceChatColors.PanelHeaderBG2 = theme.PanelHeaderBG2;
                        iceChatColors.PanelHeaderForeColor = theme.PanelHeaderForeColor;
                        iceChatColors.QueryBackColor = theme.QueryBackColor;
                        iceChatColors.RandomizeNickColors = theme.RandomizeNickColors;
                        iceChatColors.ServerListBackColor = theme.ServerListBackColor;
                        iceChatColors.ServerMessageColorChange = theme.ServerMessageColorChange;
                        iceChatColors.ServerQuitColorChange = theme.ServerQuitColorChange;
                        iceChatColors.StatusbarBackColor = theme.StatusbarBackColor;
                        iceChatColors.StatusbarForeColor = theme.StatusbarForeColor;
                        iceChatColors.TabBarChannelJoin = theme.TabBarChannelJoin;
                        iceChatColors.TabBarChannelPart = theme.TabBarChannelPart;
                        iceChatColors.TabBarCurrent = theme.TabBarCurrent;
                        iceChatColors.TabBarDefault = theme.TabBarDefault;
                        iceChatColors.TabBarNewMessage = theme.TabBarNewMessage;
                        iceChatColors.TabBarOtherMessage = theme.TabBarOtherMessage;
                        iceChatColors.TabBarServerMessage = theme.TabBarServerMessage;
                        iceChatColors.TabBarServerQuit = theme.TabBarServerQuit;
                        iceChatColors.ToolbarBackColor = theme.ToolbarBackColor;
                        iceChatColors.UnreadTextMarkerColor = theme.UnreadTextMarkerColor;

                        inputPanel.SetInputBoxColors();

                        toolStripMain.BackColor = IrcColor.colors[iceChatColors.ToolbarBackColor];
                        menuMainStrip.BackColor = IrcColor.colors[iceChatColors.MenubarBackColor];
                        statusStripMain.BackColor = IrcColor.colors[iceChatColors.StatusbarBackColor];
                        toolStripStatus.ForeColor = IrcColor.colors[iceChatColors.StatusbarForeColor];

                        serverListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
                        serverListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
                        nickListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
                        nickListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
                        channelListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
                        channelListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
                        buddyListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
                        buddyListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];

                        inputPanel.SetInputBoxColors();
                        channelList.SetListColors();
                        buddyList.SetListColors();
                        nickList.SetListColors();
                        serverTree.SetListColors();
                        
                        nickList.Invalidate();
                        mainChannelBar.Invalidate();
                        serverTree.Invalidate();
                    }
                }
            }

            //add the themes to the view menu
            if (iceChatOptions.Theme != null)
            {
                foreach (ThemeItem theme in iceChatOptions.Theme)
                {
                    if (!theme.ThemeName.Equals("Default"))
                    {
                        ToolStripMenuItem t = new ToolStripMenuItem(theme.ThemeName);
                        if (iceChatOptions.CurrentTheme == theme.ThemeName)
                            t.Checked = true;
                        
                        t.Click += new EventHandler(ThemeChoice_Click);
                        themesToolStripMenuItem.DropDownItems.Add(t);
                    }
                }
            }

            this.FormClosing += new FormClosingEventHandler(FormMainClosing);
            this.Resize += new EventHandler(FormMainResize);
            this.ControlRemoved += new ControlEventHandler(OnControlRemoved);


            if (iceChatOptions.IdentServer && !System.Diagnostics.Debugger.IsAttached)
                identServer = new IdentServer();

            if (iceChatLanguage.LanguageName != "English") ApplyLanguage(); // ApplyLanguage can first be called after all child controls are created

            //get a new router ip
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                if (iceChatOptions.DCCAutogetRouterIP == true)
                {
                    System.Threading.Thread dccThread = new System.Threading.Thread(GetLocalIPAddress)
                    {
                        Name = "DCCIPAutoUpdate"
                    };
                    dccThread.Start();
                }
            }

            if (splash != null)
            {
                splash.Close();
                splash.Dispose();
            }

            this.Activated += new EventHandler(FormMainActivated);

            nickList.ShowNickButtons = iceChatOptions.ShowNickButtons;
            serverTree.ShowServerButtons = iceChatOptions.ShowServerButtons;

            showButtonsNickListToolStripMenuItem.Checked = iceChatOptions.ShowNickButtons;
            showButtonsServerTreeToolStripMenuItem1.Checked = iceChatOptions.ShowServerButtons;

            // check for background images for nicklist and server tree
            if (iceChatOptions.NickListImage != null && iceChatOptions.NickListImage.Length > 0)
            {
                if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + iceChatOptions.NickListImage))
                    this.nickList.BackGroundImage = picturesFolder + System.IO.Path.DirectorySeparatorChar + iceChatOptions.NickListImage;
                else if (File.Exists(iceChatOptions.NickListImage))
                    this.nickList.BackGroundImage = iceChatOptions.NickListImage;

            }
            if (iceChatOptions.ServerTreeImage != null && iceChatOptions.ServerTreeImage.Length > 0)
            {
                if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + iceChatOptions.ServerTreeImage))
                    this.serverTree.BackGroundImage = picturesFolder + System.IO.Path.DirectorySeparatorChar + iceChatOptions.ServerTreeImage;
                else if (File.Exists(iceChatOptions.NickListImage))
                    this.serverTree.BackGroundImage = iceChatOptions.ServerTreeImage;
            }

            this.flashTrayIconTimer = new System.Timers.Timer(2000)
            {
                Enabled = false
            };
            this.flashTrayIconTimer.Elapsed += new System.Timers.ElapsedEventHandler(FlashTrayIconTimer_Elapsed);
            this.notifyIcon.Tag = "off";
            this.flashTrayCount = 0;

            this.flashTaskBarIconTimer = new System.Timers.Timer(2000)
            {
                Enabled = false
            };
            this.flashTaskBarIconTimer.Elapsed += new System.Timers.ElapsedEventHandler(FlashTaskBarIconTimer_Elapsed);
            this.Tag = "off";
            this.flashTrayCount = 0;

            // check the menu renderer
            switch (iceChatOptions.MenuRenderer)
            {
                case "VS 2008":
                    this.menuRenderer = new VS2008Renderer.MenuStripRenderer();
                    this.toolStripRender = new VS2008Renderer.ToolStripRenderer();

                    this.menuMainStrip.Renderer = menuRenderer;
                    this.toolStripMain.Renderer = toolStripRender;

                    this.vS2008ToolStripMenuItem.Checked = true;

                    break;

                case "Office 2007":
                    this.menuRenderer = new EasyRenderer.EasyRender();
                    this.toolStripRender = new EasyRenderer.EasyRender();

                    this.menuMainStrip.Renderer = menuRenderer;
                    this.toolStripMain.Renderer = toolStripRender;

                    this.office2007ToolStripMenuItem.Checked = true;

                    break;

                default:
                    this.menuMainStrip.RenderMode = ToolStripRenderMode.System;
                    this.toolStripMain.RenderMode = ToolStripRenderMode.System;
                    
                    this.defaultToolStripMenuItem1.Checked = true;
                    break;

            }


            //setup windowed mode if saved
            if (iceChatOptions.WindowedMode)
            {
                resizeWindowToolStripMenuItem.PerformClick();
            }            
            
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                //check for an update and setup DDE, if NOT in debugger            
                System.Threading.Thread checkThread = new System.Threading.Thread(CheckForUpdate);
                checkThread.Start();
            }

            foreach (string s in args)
            {
                if (s.IndexOf(' ') > -1)
                    _args += " \"" + s + "\"";
                else
                    _args += " " + s;
            }

            Microsoft.Win32.SystemEvents.PowerModeChanged += new Microsoft.Win32.PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
            
            // this causes a crash in wine
            Microsoft.Win32.SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);

            // this.inputPanel.Height = 26;

            System.Diagnostics.Debug.WriteLine("Finished Loading:" + this.InputPanel.Height);

            inputPanel.ShowWideTextPanel = iceChatOptions.ShowMultilineEditbox;

        }


        private void InitializeMainForm()
        {
            // MainTab
            this.mainTabControl = new IceChat.IceTabControl
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point(3, 63),
                Margin = new System.Windows.Forms.Padding(0),
                Name = "mainTabControl",
                Size = new System.Drawing.Size(918, 462),
                TabIndex = 20
            };

            this.Controls.Add(this.mainTabControl);

            // NickList
            this.nickList = new NickList(this)
            {
                AccessibleDescription = "List of Nick Names",
                AccessibleRole = System.Windows.Forms.AccessibleRole.Pane,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Header = "",
                Location = new System.Drawing.Point(0, 0),
                Margin = new System.Windows.Forms.Padding(4),
                Name = "nickList",
                Size = new System.Drawing.Size(192, 454),
                TabIndex = 0
            };

            // nickPanel
            this.nickPanel = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Location = new System.Drawing.Point(0, 0),
                Name = "nickPanel",
                Size = new System.Drawing.Size(192, 454),
                TabIndex = 0
            };
            this.nickPanel.Controls.Add(nickList);

            // NickListTab
            this.nickListTab = new System.Windows.Forms.TabPage();
            this.nickListTab.Controls.Add(this.nickPanel);
            this.nickListTab.Location = new System.Drawing.Point(4, 4);
            this.nickListTab.Name = "nickListTab";
            this.nickListTab.Size = new System.Drawing.Size(192, 454);
            this.nickListTab.TabIndex = 0;
            this.nickListTab.Text = "Nick List";

            // serverTree
            this.serverTree = new ServerTree(this)
            {
                AccessibleDescription = "List of servers and channels associated with them once connected",
                AccessibleRole = System.Windows.Forms.AccessibleRole.Pane,
                Dock = System.Windows.Forms.DockStyle.Fill,
                Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(0, 0),
                Margin = new System.Windows.Forms.Padding(4, 3, 4, 3),
                Name = "serverTree",
                Size = new System.Drawing.Size(172, 454),
                TabIndex = 0
            };

            // ServerPanel
            this.serverPanel = new System.Windows.Forms.Panel();
            this.serverPanel.Controls.Add(this.serverTree);
            this.serverPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverPanel.Location = new System.Drawing.Point(0, 0);
            this.serverPanel.Name = "serverPanel";
            this.serverPanel.Size = new System.Drawing.Size(172, 454);
            this.serverPanel.TabIndex = 0;
            this.serverPanel.Controls.Add(this.serverTree);

            // ServerListTab
            this.serverListTab = new System.Windows.Forms.TabPage();
            this.serverListTab.Controls.Add(this.serverPanel);
            this.serverListTab.Location = new System.Drawing.Point(24, 4);
            this.serverListTab.Name = "serverListTab";
            this.serverListTab.Size = new System.Drawing.Size(172, 454);
            this.serverListTab.TabIndex = 0;
            this.serverListTab.Text = "Favorite Servers";
            this.nickListTab.SuspendLayout();
            this.nickListTab.ResumeLayout(false);
            this.serverListTab.SuspendLayout();
            this.serverListTab.ResumeLayout(false);

            // Panels
            this.panelDockRight = new IceDockPanel(this);
            this.panelDockLeft = new IceDockPanel(this);
            this.mainChannelBar = new ChannelBar(this);

            // panelDockRight
            this.panelDockRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelDockRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDockRight.Location = new System.Drawing.Point(704, 94);
            this.panelDockRight.Name = "panelDockRight";
            this.panelDockRight.Size = new System.Drawing.Size(220, 431);
            this.panelDockRight.TabIndex = 14;

            // panelDockLeft
            this.panelDockLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelDockLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelDockLeft.Location = new System.Drawing.Point(0, 94);
            this.panelDockLeft.Name = "panelDockLeft";
            this.panelDockLeft.Size = new System.Drawing.Size(200, 431);
            this.panelDockLeft.TabIndex = 13;

            // mainChannelBar
            this.mainChannelBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainChannelBar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainChannelBar.Location = new System.Drawing.Point(0, 63);
            this.mainChannelBar.Name = "mainChannelBar";
            this.mainChannelBar.SelectedIndex = -1;
            this.mainChannelBar.Size = new System.Drawing.Size(924, 31);
            this.mainChannelBar.TabIndex = 24;            //

            // 
            // splitterLeft
            // 

            this.splitterLeft = new System.Windows.Forms.Splitter
            {
                //this.splitterLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                Location = new System.Drawing.Point(0, 63),
                Name = "splitterLeft",
                Size = new System.Drawing.Size(3, 462),
                TabIndex = 15,
                TabStop = false
            };

            // 
            // splitterRight
            // 
            this.splitterRight = new System.Windows.Forms.Splitter
            {
                //this.splitterRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                Dock = System.Windows.Forms.DockStyle.Right,
                Location = new System.Drawing.Point(921, 63),
                Name = "splitterRight",
                Size = new System.Drawing.Size(3, 462),
                TabIndex = 16,
                TabStop = false
            };


            this.Controls.Add(this.splitterRight);
            this.Controls.Add(this.splitterLeft);
            
            this.Controls.Add(this.panelDockRight);
            this.Controls.Add(this.panelDockLeft);
            this.Controls.Add(this.mainChannelBar);

        }

        private void SystemEvents_TimeChanged(object sender, EventArgs e)
        {
            // detect when the time, or time zone changed
            System.Globalization.CultureInfo.CurrentCulture.ClearCachedData();
        }

        private void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case Microsoft.Win32.PowerModes.Suspend:
                    // set all connected servers to 'Suspended'
                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                    {
                        c.ServerSetting.Suspended = c.IsConnected;
                    }
                    break;


                case Microsoft.Win32.PowerModes.Resume:
                    System.Threading.Thread reStartThread = new System.Threading.Thread(ReconnectServers);
                    reStartThread.Start();

                    break;
            }
        }

        private void ReconnectServers()
        {
            System.Threading.Thread.Sleep(15000);

            foreach (IRCConnection c in serverTree.ServerConnections.Values)
            {
                if (c.IsConnected == false)
                {
                    if (c.ServerSetting.Suspended == true)
                        c.ConnectSocket();
                }
            }

        }

        private void OnControlRemoved(object sender, ControlEventArgs e)
        {
            try
            {
                if (e.Control is Panel)
                {
                    // could be a bottom panel
                    if (e.Control.Tag.ToString() == "pluginb")
                    {
                        // check if we have any panels left over
                        bool foundTag = false;
                        foreach (Control c in this.Controls)
                        {
                            if (c is Panel)
                            {
                                if (c.Tag.ToString() == "pluginb")
                                {
                                    foundTag = true;
                                }
                            }
                        }

                        if (foundTag == false)
                            this.splitterBottom.Visible = false;


                    }
                }
            }
            catch (Exception) { }
            
        }

        private void FormMainActivated(object sender, EventArgs e)
        {
            if (iceChatOptions.IsOnTray == true)
            {
                MinimizeToTray();
                IsForeGround = false;
            }
            else
                IsForeGround = true;


            //check if we have an autostart command from the irc://
            if (disableAutoStart == false)
            {
                if (autoStartCommand != null && autoStartCommand.Length > 0)
                    ParseOutGoingCommand(null, autoStartCommand);
                else
                {
                    System.Threading.Thread autoStartThread = new System.Threading.Thread(AutoStart);                    
                    autoStartThread.Start();
                }
            }
            
            //remove the event handler, because it only needs to be run once, on startup
            this.Activated -= FormMainActivated;

            this.Activated += new EventHandler(FormMain_Activated);
            this.Deactivate += new EventHandler(FormMain_Deactivate);

            finishedLoading = true;

            // make backups of all the XML files, as we have finished loading successfully
            
            //System.Diagnostics.Debug.WriteLine(currentFolder);
            if (this.loadErrors.Count > 0)
            {
                foreach (string err in this.loadErrors)
                {
                    WindowMessage(null, "Console", "\x000304Error: " + err, "", true);
                }
                
                loadErrors.Clear();
            }

            try
            {

                // backup the main XML files
                System.IO.File.Copy(optionsFile, backupFolder + Path.DirectorySeparatorChar + "IceChatOptions.xml", true);
                System.IO.File.Copy(serversFile, backupFolder + Path.DirectorySeparatorChar + "IceChatServer.xml", true);
                System.IO.File.Copy(aliasesFile, backupFolder + Path.DirectorySeparatorChar + "IceChatAliases.xml", true);
                System.IO.File.Copy(popupsFile, backupFolder + Path.DirectorySeparatorChar + "IceChatPopups.xml", true);
                System.IO.File.Copy(fontsFile, backupFolder + Path.DirectorySeparatorChar + "IceChatFonts.xml", true);
                System.IO.File.Copy(messagesFile, backupFolder + Path.DirectorySeparatorChar + "IceChatMessages.xml", true);
                System.IO.File.Copy(colorsFile, backupFolder + Path.DirectorySeparatorChar + "IceChatColors.xml", true);

            }
            catch(Exception)
            {
                System.Diagnostics.Debug.WriteLine("Error Creating backup");
            }
        }

        private void AutoStart()
        {
           this.Invoke((MethodInvoker)delegate()
           {
               //run any auto perform commands
               if (iceChatOptions.AutoPerformStartup != null)
               {
                   if (iceChatOptions.AutoPerformStartupEnable)
                   {
                       foreach (string command in iceChatOptions.AutoPerformStartup)
                       {
                           if (!command.StartsWith(";"))
                               ParseOutGoingCommand(null, command);
                       }
                   }
               }

            });

            //auto start any Auto Connect Servers
            foreach (ServerSetting s in serverTree.ServersCollection.listServers)
            {
                bool found = false;
                if (s.AutoStart)
                {
                    //add a delay here?
                    //check if we have a connection already..
                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                    {
                        if (c.ServerSetting == s)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                        NewServerConnection(s);

                }
            }
        }

        private void UpdateInstallVersion()
        {
            //need elevated results
            if (CheckElevated())
            {
                Microsoft.Win32.RegistryKey rKey = null;
                rKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\IceChat9_is1", true);
                if (rKey != null)
                {
                    rKey.SetValue("DisplayName", ProgramID + " " + VersionID + " (Build " + BuildNumber + ")", Microsoft.Win32.RegistryValueKind.String);
                    rKey.SetValue("DisplayVersion", VersionID, Microsoft.Win32.RegistryValueKind.String);
                }
            }
        }

        private bool CheckElevated()
        {
            try
            {

                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                
                bool isElevated = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
                
                if (!isElevated)
                {
                    //self elevate
                    DialogResult dr = MessageBox.Show("You need to run IceChat as Admin to Update. Do you want to restart and run as Admin?", "IceChat", MessageBoxButtons.YesNo);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            UseShellExecute = true,
                            FileName = Application.ExecutablePath,
                            WorkingDirectory = Environment.CurrentDirectory,
                            Arguments = _args
                        };

                        if (System.Environment.OSVersion.Version.Major >= 6)  // Windows Vista or higher
                        {
                            processStartInfo.Verb = "runas";
                        }
                        else
                        {
                            // No need to prompt to run as admin
                        }

                        System.Diagnostics.Process.Start(processStartInfo);

                        Application.Exit();


                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SetupIRCDDE()
        {
            //check if we have elevated (admin level)
            String os = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows NT\CurrentVersion", "ProductName", string.Empty).ToString();

            if (os.IndexOf("Windows") > -1 && CheckElevated() == true)
            {
                Microsoft.Win32.RegistryKey rKey = null;
                try
                {
                    rKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("irc", true);
                    if (rKey != null)
                    {
                        Microsoft.Win32.RegistryKey sKey = rKey.OpenSubKey(@"shell\open\command");
                        string key = sKey.GetValue("").ToString();
                        
                        //check if the path is the same
                        if (key.IndexOf(Application.ExecutablePath) != 1)
                        {
                            //path is not the same, delete it
                            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(@"irc\shell");
                            Microsoft.Win32.Registry.ClassesRoot.DeleteSubKey(@"irc");
                        }
                        rKey.Close();
                        rKey = null;
                    }
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBox.Show("SetupDDE Error 1: Cant delete irc: key");
                    rKey.Close();
                    rKey = null;
                    return;
                }
                catch (System.Exception)
                {
                    //MessageBox.Show("SetupDDE Error 2:" + ex.Message + ":" + ex.StackTrace);
                    if (rKey != null)
                        rKey.Close();
                    
                    rKey = null;
                }

                if (rKey == null)
                {
                    try
                    {
                        string user = Environment.UserDomainName + "\\" + Environment.UserName;

                        RegistrySecurity rs = new RegistrySecurity();
                        rs.AddAccessRule(new RegistryAccessRule(user,
                                    RegistryRights.FullControl,
                                    InheritanceFlags.None,
                                    PropagationFlags.None,
                                    AccessControlType.Allow));

                        rKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("irc", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, rs);
                        rKey.SetValue("", "URL:IRC Protocol");
                        rKey.SetValue("URL Protocol", "");

                        rKey = rKey.CreateSubKey(@"shell");
                        rKey.SetValue("", "", Microsoft.Win32.RegistryValueKind.String);
                        
                        rKey = rKey.CreateSubKey(@"open");                        
                        rKey.SetValue("", "", Microsoft.Win32.RegistryValueKind.String);
                        
                        rKey = rKey.CreateSubKey(@"command");
                        
                        rKey.SetValue("", "\"" + Application.ExecutablePath + "\" %1", Microsoft.Win32.RegistryValueKind.String);

                        rKey.Close();

                        // icechat plugin
                        rKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("icechatplugin", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, rs);
                        rKey.SetValue("", "URL:IRC Protocol");
                        rKey.SetValue("URL Protocol", "");

                        rKey = rKey.CreateSubKey(@"shell");
                        rKey.SetValue("", "", Microsoft.Win32.RegistryValueKind.String);

                        rKey = rKey.CreateSubKey(@"open");
                        rKey.SetValue("", "", Microsoft.Win32.RegistryValueKind.String);

                        rKey = rKey.CreateSubKey(@"command");

                        rKey.SetValue("", "\"" + Application.ExecutablePath + "\" %1", Microsoft.Win32.RegistryValueKind.String);

                        rKey.Close();



                        MessageBox.Show("DDE Setup Successfull. irc:// links should now work");


                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        MessageBox.Show("Unauthorized Error");
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Exception Error");
                    }
                }
                else
                {
                    rKey.Close();
                }
            }
        }

        private void InitializePlugin(object pluginObject)
        {
            try
            {
                IPluginIceChat plugin = (IPluginIceChat)pluginObject;
                plugin.Initialize();
                plugin.MainProgramLoaded();
                plugin.MainProgramLoaded(ServerTree.ServerConnections);

                if (plugin.Enabled == true)
                {
                    Panel[] bottomPanels = plugin.AddMainPanel();
                    if (bottomPanels != null && bottomPanels.Length > 0)
                    {
                        foreach (Panel p in bottomPanels)
                        {
                            if (p.Dock == DockStyle.Top)
                            {
                                p.Tag = "plugint";
                                this.Invoke((MethodInvoker)delegate()
                                {
                                    this.Controls.Add(p);
                                });
                            }
                            else if (p.Dock == DockStyle.Bottom)
                            {
                                p.Tag = "pluginb";
                                this.Invoke((MethodInvoker)delegate()
                                {
                                    this.Controls.Add(p);
                                    this.splitterBottom.Visible = true;
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
            }
        }

        private void PluginsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            //check if plugins are disabled or not, in case disabled in the plugin
            foreach (ToolStripMenuItem t in pluginsToolStripMenuItem.DropDownItems)
            {
                if (t.Tag != null)
                {
                    IPluginIceChat plugin = (IPluginIceChat)t.Tag;
                    if (!plugin.Enabled)
                        t.Image = StaticMethods.LoadResourceImage("CloseButton.png");
                    else
                        t.Image = null;
                }
            }
        }

        private void FirstRunSaveOptions(IceChatOptions options, IceChatFontSetting fonts)
        {
            this.iceChatOptions = options;
            this.iceChatFonts = fonts;
            
            SaveFonts();
            SaveOptions();           

        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            //close the current window
            mainChannelBar.CloseCurrentTab();
        }

        private void UpdateIcon(string iconName, string tag)
        {
            if (!this.IsHandleCreated && !this.IsDisposed) return;
            
            this.Invoke((MethodInvoker)delegate()
            {
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage(iconName).GetHicon());
                this.Tag = tag;
            });
        }

        private void UpdateTrayIcon(string iconName, string tag)
        {
            if (!this.IsHandleCreated && !this.IsDisposed) return;
            
            this.Invoke((MethodInvoker)delegate()
            {
                this.notifyIcon.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage(iconName).GetHicon());
                this.notifyIcon.Tag = tag;
            });
        }

        private void FlashTaskBar()
        {
            if (StaticMethods.IsRunningOnMono())
            {
                //cant run flashwindowex
                this.flashTaskBarIconTimer.Enabled = true;
                this.flashTaskBarIconTimer.Start();
            }
            else
            {
                //need to invoke
                if (!this.IsHandleCreated && !this.IsDisposed) return;
                
                this.Invoke((MethodInvoker)delegate()
                {

                    FLASHWINFO fw = new FLASHWINFO
                    {
                        cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO))),
                        hwnd = this.Handle,
                        dwFlags = 3,
                        dwTimeout = 0,
                        uCount = (uint)iceChatOptions.FlashTaskBarNumber
                    };

                    FlashWindowEx(ref fw);
                });
            }
        }

        private void FlashTaskBarIconTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (this.Tag.Equals("on"))
                {
                    UpdateIcon("new-tray-icon.ico", "off");
                }
                else
                {
                    UpdateIcon("tray-icon-flash.ico", "on");
                }

                flashTaskBarCount++;

                if (flashTaskBarCount == iceChatOptions.FlashTaskBarNumber)
                {
                    this.flashTaskBarIconTimer.Stop();
                    UpdateIcon("new-tray-icon.ico", "off");
                    flashTaskBarCount = 0;
                }

            }
            else
            {
                this.flashTaskBarIconTimer.Stop();
                UpdateIcon("new-tray-icon.ico", "off");
            }
        }

        private void FlashTrayIconTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.notifyIcon.Visible == true)
            {
                if (this.notifyIcon.Tag.Equals("on"))
                {
                    UpdateTrayIcon("new-tray-icon.ico", "off");
                }
                else
                {
                    UpdateTrayIcon("tray-icon-flash.ico", "on");
                }
                
                flashTrayCount++;

                if (flashTrayCount == 10)
                {
                    this.flashTrayIconTimer.Stop();
                    UpdateTrayIcon("new-tray-icon.ico", "off");
                    flashTrayCount = 0;
                }
            }
            else
            {
                this.flashTrayIconTimer.Stop();
                UpdateTrayIcon("new-tray-icon.ico", "off");
            }
        }

        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            IsForeGround = false;
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            IsForeGround = true;
            //what is the active window.. make sure it IS active..
            //which tab is on top. or the active mdi child
            if (!mainTabControl.windowedMode)
            {
                //which tab is on top                                
                for (int i = mainTabControl.Controls.Count - 1; i >= 0; i--)
                {
                    if (mainTabControl.Controls[i].GetType() == typeof(IceTabPage))
                    {
                        IceTabPage tab = ((IceTabPage)mainTabControl.Controls[i]);
                        if (mainTabControl.Controls.GetChildIndex(tab) == 0)
                        {
                            //which one is active in the channel bar / server tree?
                            mainChannelBar.SelectTab(tab);
                            serverTree.SelectTab(tab, false);
                            
                            break;
                        }
                    }
                }

            }
            else
            {
                //which child form is active


            }

            FocusInputBox();
        }

        private void FormMainResize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (iceChatOptions.MinimizeToTray)
                {
                    this.notifyIcon.Visible = true;
                    this.Hide();
                }
            }
        }

        private void ToolStripMain_VisibleChanged(object sender, EventArgs e)
        {
            toolBarToolStripMenuItem.Checked = toolStripMain.Visible;
        }
        
        /// <summary>
        /// Save Default Server Settings
        /// </summary>
        private void OnDefaultServerSettings()
        {
            SaveOptions();
        }

        #region Load Language File

        private LanguageItem LoadLanguageItem(string languageFileName)
        {
            if (File.Exists(languageFileName))
            {
                LanguageItem languageItem = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(LanguageItem));
                TextReader textReader = new StreamReader(languageFileName);
                try
                {
                    languageItem = (LanguageItem)deserializer.Deserialize(textReader);
                    languageItem.LanguageFile = languageFileName;
               }
                catch
                {
                    languageItem = null;
                }
                finally
                {
                    textReader.Close();
                    textReader.Dispose();
                }
                return languageItem;
            }
            else
            {
                return null;
            }
        }

        private void LoadLanguage()
        {
            if (File.Exists(currentLanguageFile.LanguageFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatLanguage));
                TextReader textReader = new StreamReader(currentLanguageFile.LanguageFile);
                iceChatLanguage = (IceChatLanguage)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
            {
                iceChatLanguage = new IceChatLanguage();
                //write the language file
                XmlSerializer serializer = new XmlSerializer(typeof(IceChatLanguage));
                TextWriter textWriter = new StreamWriter(currentFolder + Path.DirectorySeparatorChar + "Languages" + Path.DirectorySeparatorChar + "English.xml");
                serializer.Serialize(textWriter,iceChatLanguage);
                textWriter.Close();
                textWriter.Dispose();
            }
        }

        private void ApplyLanguage()
        {
            mainToolStripMenuItem.Text = iceChatLanguage.mainToolStripMenuItem;
            minimizeToTrayToolStripMenuItem.Text = iceChatLanguage.minimizeToTrayToolStripMenuItem;
            debugWindowToolStripMenuItem.Text = iceChatLanguage.debugWindowToolStripMenuItem;
            exitToolStripMenuItem.Text = iceChatLanguage.exitToolStripMenuItem;
            optionsToolStripMenuItem.Text = iceChatLanguage.optionsToolStripMenuItem;
            iceChatSettingsToolStripMenuItem.Text = iceChatLanguage.iceChatSettingsToolStripMenuItem;
            iceChatColorsToolStripMenuItem.Text = iceChatLanguage.iceChatColorsToolStripMenuItem;
            iceChatEditorToolStripMenuItem.Text = iceChatLanguage.iceChatEditorToolStripMenuItem;
            pluginsToolStripMenuItem.Text = iceChatLanguage.pluginsToolStripMenuItem;
            viewToolStripMenuItem.Text = iceChatLanguage.viewToolStripMenuItem;
            serverListToolStripMenuItem.Text = iceChatLanguage.serverListToolStripMenuItem;
            nickListToolStripMenuItem.Text = iceChatLanguage.nickListToolStripMenuItem;
            statusBarToolStripMenuItem.Text = iceChatLanguage.statusBarToolStripMenuItem;
            toolBarToolStripMenuItem.Text = iceChatLanguage.toolBarToolStripMenuItem;
            helpToolStripMenuItem.Text = iceChatLanguage.helpToolStripMenuItem;
            iceChatHomePageToolStripMenuItem.Text = iceChatLanguage.iceChatHomePageToolStripMenuItem;
            //forumsToolStripMenuItem.Text = iceChatLanguage.forumsToolStripMenuItem;
            aboutToolStripMenuItem.Text = iceChatLanguage.aboutToolStripMenuItem;
            toolStripQuickConnect.Text = iceChatLanguage.toolStripQuickConnect;
            toolStripSettings.Text = iceChatLanguage.toolStripSettings;
            toolStripColors.Text = iceChatLanguage.toolStripColors;
            toolStripEditor.Text = iceChatLanguage.toolStripEditor;
            toolStripAway.Text = iceChatLanguage.toolStripAway;
            toolStripSystemTray.Text = iceChatLanguage.toolStripSystemTray;
            toolStripStatus.Text = iceChatLanguage.toolStripStatus;
            
            channelListTab.Text = iceChatLanguage.tabPageFaveChannels;
            nickListTab.Text = iceChatLanguage.tabPageNicks;
            serverListTab.Text = iceChatLanguage.serverTreeHeader;

            channelList.ApplyLanguage();
            nickList.ApplyLanguage();
            serverTree.ApplyLanguage();
            inputPanel.ApplyLanguage();

            mainChannelBar.Invalidate();
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 24:    // WM_SHOWWINDOW
                    // not on the tray anymore!
                    iceChatOptions.IsOnTray = false;
                    this.notifyIcon.Visible = iceChatOptions.ShowSytemTrayIcon;
                    break;

                case 16:    // 0x10 WM_CLOSE
                    //System.Diagnostics.Debug.WriteLine("WM_CLOSE:" + askedClose + ":" + allowClose);
                    //catch it here
                    if (!askedClose && !allowClose)
                    {
                        if (iceChatOptions.AskQuit)
                        {
                            foreach (IRCConnection c in serverTree.ServerConnections.Values)
                            {
                                if (c.IsConnected)
                                {
                                    DialogResult dr = MessageBox.Show("You are connected to a Server(s), are you sure you want to close IceChat?", "Close IceChat", MessageBoxButtons.OKCancel);
                                    if (dr == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    break;
                case 274:   // 0x112 WM_SYSCOMMAND
                    //System.Diagnostics.Debug.WriteLine("WM_SYSCOMMAND:" + m.LParam + ":" + m.WParam + ":" + m.WParam.ToString("X"));
                    if (m.WParam.ToInt32() == 0xF060) // SC_CLOSE
                    {
                        if (!StaticMethods.IsRunningOnMono())
                        {
                            //System.Diagnostics.Debug.WriteLine("close it!");
                            if (iceChatOptions.AskQuit)
                            {
                                foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                {
                                    if (c.IsConnected)
                                    {
                                        DialogResult dr = MessageBox.Show("You are connected to a Server(s), are you sure you want to close IceChat?", "Close IceChat", MessageBoxButtons.OKCancel);
                                        if (dr == DialogResult.Cancel)
                                        {
                                            allowClose = false;
                                            askedClose = false;
                                            return;
                                        }
                                        else
                                        {
                                            allowClose = true;
                                            askedClose = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            askedClose = true;
                            allowClose = true;
                        }
                    }
                    break;
                default:
                    //System.Diagnostics.Debug.WriteLine(m.Msg + ":" + m.Msg.ToString("X"));
                    //
                    break;
            }
            
            base.WndProc(ref m);
        }

        private void FormMainClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine(e.CloseReason);
                /*
                if (iceChatOptions.AskQuit)
                {
                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                    {
                        if (c.IsConnected)
                        {
                            DialogResult dr = MessageBox.Show("You are connected to a Server(s), are you sure you want to close IceChat?", "Close IceChat", MessageBoxButtons.OKCancel);
                            if (e.CloseReason == CloseReason.UserClosing && dr == DialogResult.Cancel)
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                                break;
                        }
                    }
                }
                */

                //check if there are any connections open                    
                if (identServer != null)
                {
                    identServer.Stop();
                    identServer = null;
                }
                
                foreach (IrcTimer t in _globalTimers)
                    t.DisableTimer();
                    
                _globalTimers.Clear();

                //disconnect all the servers
                foreach (IRCConnection c in serverTree.ServerConnections.Values)
                {
                    if (c.IsConnected)
                    {
                        c.AttemptReconnect = false;
                        ParseOutGoingCommand(c, "//quit " + c.ServerSetting.QuitMessage);
                    }
                }
                    
                if (iceChatOptions.SaveWindowPosition)
                {
                        //save the window position , as long as its not minimized
                        if (this.WindowState != FormWindowState.Minimized)
                        {

                            if (Screen.AllScreens.Length > 1)
                            {
                                foreach (Screen screen in Screen.AllScreens)
                                    if (screen.Bounds.Contains(this.Location))
                                        iceChatOptions.WindowLocation = new Point(this.Location.X, this.Location.Y);
                            }
                            else
                                iceChatOptions.WindowLocation = this.Location;

                            iceChatOptions.WindowSize = this.Size;

                            iceChatOptions.WindowState = this.WindowState;


                            if (!panelDockRight.IsDocked)
                                iceChatOptions.RightPanelWidth = panelDockRight.Width;
                            if (!panelDockLeft.IsDocked)
                                iceChatOptions.LeftPanelWidth = panelDockLeft.Width;

                            iceChatOptions.DockLeftPanel = panelDockLeft.IsDocked;
                            iceChatOptions.DockRightPanel = panelDockRight.IsDocked;

                            //Save the side panels
                            string[] leftPanels = new string[panelDockLeft.TabControl.TabPages.Count];
                            for (int i = 0; i < panelDockLeft.TabControl.TabPages.Count; i++)
                            {
                                leftPanels[i] = panelDockLeft.TabControl.TabPages[i].Text;
                            }
                            iceChatOptions.LeftPanels = leftPanels;

                            string[] rightPanels = new string[panelDockRight.TabControl.TabPages.Count];
                            for (int i = 0; i < panelDockRight.TabControl.TabPages.Count; i++)
                            {
                                rightPanels[i] = panelDockRight.TabControl.TabPages[i].Text;
                            }
                            iceChatOptions.RightPanels = rightPanels;

                        }

                        SaveOptions();
                }
                    
                    //unload and dispose of all the plugins
                foreach (Plugin p in loadedPlugins)
                {
                    IceChatPlugin ipc = p as IceChatPlugin;
                    if (ipc != null)
                    {
                        try
                        {
                            ipc.plugin.Dispose();
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                for (int i = 0; i < loadedPlugins.Count; i++)
                    loadedPlugins.RemoveAt(i);

                if (errorFile != null)
                {
                    errorFile.Flush();
                    errorFile.Close();
                    errorFile.Dispose();
                }
                    
            }
            catch (Exception)
            {
                //MessageBox.Show(ee.Message + ":" + ee.StackTrace);                
            }
        }
        
        /// <summary>
        /// Play the specified sound file (currently only supports WAV files)
        /// </summary>
        /// <param name="sound"></param>
        internal void PlaySoundFile(string key)
        {            
            IceChatSounds.SoundEntry sound = IceChatSounds.GetSound(key);
            if (sound != null && !muteAllSounds)
            {
                string file = sound.GetSoundFile();
                if (file != null && file.Length > 0)
                {
                    try
                    {
                        if (iceChatOptions.SoundUseExternalCommand && iceChatOptions.SoundExternalCommand.Length > 0)
                            ParseOutGoingCommand(inputPanel.CurrentConnection, iceChatOptions.SoundExternalCommand + " " + file);
                        else
                            ParseOutGoingCommand(inputPanel.CurrentConnection, "/play " + file);
                    }
                    catch { }
                        
                }
            }
        }

        /// <summary>
        /// Create a Default Tab for showing Welcome Information
        /// </summary>
        private void CreateDefaultConsoleWindow()
        {
            IceTabPage p = new IceTabPage(IceTabPage.WindowType.Console, "Console", this);
            p.AddConsoleTab(iceChatLanguage.consoleTabWelcome);
            
            mainTabControl.AddTabPage(p);
            mainTabControl.BringFront(p);

            mainChannelBar.AddTabPage(ref p);

            //get the window size if set
            ChannelSetting cs = ChannelSettings.FindChannel("Console", "");
            if (cs != null)
            {
                p.WindowLocation = cs.WindowLocation;
                p.WindowSize = cs.WindowSize;
                p.PinnedTab = cs.PinnedTab;
            }

            if (System.Environment.Version.Major >= 4)
            {
                //WindowMessage(null, "Console", "\x000304Welcome to " + ProgramID + " " + VersionID + " - Build " + BuildNumber + " compiled with .NET45", "", false);
                string target = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
                WindowMessage(null, "Console", "\x000304Welcome to " + ProgramID + " " + VersionID + " - Build " + BuildNumber + " compiled with " + target, "", false);

            }
            else
            {
                WindowMessage(null, "Console", "\x000304Welcome to " + ProgramID + " " + VersionID + " - Build " + BuildNumber, "", false);            
            }

            WindowMessage(null, "Console", "\x000304Come to \x00030,4#icechat\x0003 on \x00030,2irc://irc.quakenet.org/icechat\x0003 if you wish to help with this project", "", true);
            //WindowMessage(null, "Console", "\x000304Visit our facebook page at https://www.facebook.com/IceChat","", true);
            //WindowMessage(null, "Console", "\x000304Visit our wiki page at https://wiki.icechat.net", "", true);
            WindowMessage(null, "Console", "\x000304Source code is available at https://github.com/icechat/IceChat", "", true);
            WindowMessage(null, "Console", "\x000304-", "", true);
        }

        #region Internal Properties

        /// <summary>
        /// Gets the instance of the Nick List
        /// </summary>
        internal NickList NickList
        {
            get { return nickList; } 
        }
        /// <summary>
        /// Gets the instance of the Server Tree
        /// </summary>
        internal ServerTree ServerTree
        {
            get { return serverTree; }
        }
        /// <summary>
        /// Gets the instance of the Main Tab Control
        /// </summary>
        internal IceTabControl TabMain
        {
            get { return mainTabControl; }
        }
        internal ChannelBar ChannelBar
        {
            get { return mainChannelBar; }
        }

        /// <summary>
        /// Gets the instance of the InputPanel
        /// </summary>
        internal InputPanel InputPanel
        {
            get
            {
                return this.inputPanel;
            }
        }

        internal IceChatOptions IceChatOptions
        {
            get
            {
                return this.iceChatOptions;
            }
        }

        internal IceChatColorPalette ColorPalette
        {
            get
            {
                return this.colorPalette;
            }
        }

        internal IceChatMessageFormat MessageFormats
        {
            get
            {
                return this.iceChatMessages;
            }
        }

        internal IceChatFontSetting IceChatFonts
        {
            get
            {
                return this.iceChatFonts;
            }
        }
        
        internal IceChatColors IceChatColors
        {
            get
            {
                return this.iceChatColors;
            }
        }

        internal ChannelSettings ChannelSettings
        {
            get
            {
                return this.channelSettings;
            }
        }

        internal IceChatSounds IceChatSounds
        {
            get
            {
                return this.iceChatSounds;
            }
        }

        internal IceChatAliases IceChatAliases
        {
            get
            {
                return iceChatAliases;
            }
            set
            {
                iceChatAliases = value;
                //save the aliases
                SaveAliases();
            }
        }

        internal IceChatPopupMenus IceChatPopupMenus
        {
            get
            {
                return iceChatPopups;
            }
            set
            {
                iceChatPopups = value;
                //save the popups
                SavePopups();
            }

        }

        internal List<Plugin> LoadedPlugins
        {
            get { return loadedPlugins; }
        }
        
        internal IceChatEmoticon IceChatEmoticons
        {
            get
            {
                return iceChatEmoticons;
            }
            set
            {
                iceChatEmoticons = value;
                //save the Emoticons
                SaveEmoticons();
            }
        }

        internal IceChatLanguage IceChatLanguage
        {
            get
            {
                return iceChatLanguage;
            }
        }

        internal List<LanguageItem> IceChatLanguageFiles
        {
            get
            {
                return languageFiles;
            }
        }

        internal LanguageItem IceChatCurrentLanguageFile
        {
            get
            {
                return currentLanguageFile;
            }
            set
            {
                if (currentLanguageFile != value)
                {
                    currentLanguageFile = value;
                    LoadLanguage();
                    ApplyLanguage();
                }
           }
        }

        internal string FavoriteChannelsFile
        {
            get
            {
                return favoriteChannelsFile;
            }
        }

        internal BuddyList BuddyList
        {
            get
            {
                return this.buddyList;
            }
        }

        internal string MessagesFile
        {
            get
            {
                return messagesFile;
            }
            set
            {
                messagesFile = value;
            }
        }

        internal string ColorsFile
        {
            get
            {
                return colorsFile;
            }
            set
            {
                colorsFile = value;
            }
        }

        internal string ServersFile
        {
            get
            {
                return serversFile;
            }
        }

        internal string BackupFolder
        {
            get
            {
                return backupFolder;
            }
        }

        internal string AliasesFile
        {
            get
            {
                return aliasesFile;
            }
        }

        internal string PopupsFile
        {
            get
            {
                return popupsFile;
            }
        }

        internal List<IThemeIceChat> IceChatPluginThemes
        {
            get
            {
                return loadedPluginThemes;
            }
        }

        public string LogsFolder
        {
            get
            {
                return logsFolder;
            }
        }

        public string CurrentFolder
        {
            get
            {
                return currentFolder;
            }
        }

        internal string EmoticonsFolder
        {
            get
            {
                return System.IO.Path.GetDirectoryName(emoticonsFile);
            }
        }

        internal string EmoticonsFile
        {
            get
            {
                return emoticonsFile;
            }
        }

        internal void StatusText(string data)
        {
            try
            {
                if (!this.IsHandleCreated && !this.IsDisposed) return;
                
                this.Invoke((MethodInvoker)delegate()
                {
                    toolStripStatus.Text = "Status: " + data;

                    // flickers on mouseover tooltip
                    //toolStripStatus.ToolTipText = "Status: " + data;
                });
            }
            catch (Exception) 
            {
                //System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }

        #endregion

        #region Private Properties
        /// <summary>
        /// Set focus to the Input Panel
        /// </summary>
        internal void FocusInputBox()
        {
            if (Form.ActiveForm == null || Form.ActiveForm.Name != "FormWindow")
                inputPanel.FocusTextBox();
            else
                ((FormWindow)FormMain.ActiveForm).DockedControl.InputPanel.FocusTextBox();
        }

        /// <summary>
        /// Sends a Message to a Named Window
        /// </summary>
        /// <param name="connection">Which Connection to use</param>
        /// <param name="name">Name of the Window</param>
        /// <param name="data">Message to send</param>
        /// <param name="color">Color number of the message</param>
        internal void WindowMessage(IRCConnection connection, string name, string data, string timeStamp, bool scrollToBottom)
        {
            if (this.InvokeRequired)
            {
                WindowMessageDelegate w = new WindowMessageDelegate(WindowMessage);
                this.Invoke(w, new object[] { connection, name, data, timeStamp, scrollToBottom} );
            }
            else
            {
                if (name == "Console")
                {
                    mainChannelBar.GetTabPage("Console").AddText(connection, data, timeStamp, scrollToBottom, ServerMessageType.Message);
                    if (connection != null)
                        if (connection.IsFullyConnected)
                            if (!connection.ServerSetting.DisableSounds)
                                PlaySoundFile("conmsg");
                }
                else
                {
                    foreach (IceTabPage t in mainChannelBar.TabPages)
                    {
                        if (t.TabCaption == name)
                        {
                            if (t.Connection == connection)
                            {
                                t.TextWindow.AppendText(data, timeStamp);
                                if (scrollToBottom)
                                    t.TextWindow.ScrollToBottom();
                                return;
                            }
                        }
                    }
                    
                    WindowMessage(connection, "Console", data, timeStamp, scrollToBottom);
                }
            }
        }
        /// <summary>
        /// Send a Message to the Current Window
        /// </summary>
        /// <param name="connection">Which Connection to use</param>
        /// <param name="data">Message to send</param>
        /// <param name="color">Color number of the message</param>
        internal void CurrentWindowMessage(IRCConnection connection, string data, string timeStamp, bool scrollToBottom)
        {
            if (this.InvokeRequired)
            {
                CurrentWindowMessageDelegate w = new CurrentWindowMessageDelegate(CurrentWindowMessage);
                this.Invoke(w, new object[] { connection, data, timeStamp, scrollToBottom });
            }
            else
            {
                //check what type the current window is
                if (CurrentWindowStyle == IceTabPage.WindowType.ChannelList)
                {
                    //do nothing, send it to the console
                    mainChannelBar.GetTabPage("Console").AddText(connection, data, timeStamp, false, ServerMessageType.Other);
                }
                else if (CurrentWindowStyle != IceTabPage.WindowType.Console)
                {
                    IceTabPage t = mainChannelBar.CurrentTab;
                    if (t != null)
                    {
                        if (t.Connection == connection)
                        {
                            t.TextWindow.AppendText(data, timeStamp);
                        }
                        else
                        {
                            WindowMessage(connection, "Console", data, timeStamp, scrollToBottom);
                        }
                    }
                }
                else
                {
                    //console window is current window
                    mainChannelBar.GetTabPage("Console").AddText(connection, data, timeStamp, false, ServerMessageType.Other);
                }
            }
        }

        /// <summary>
        /// Gets a Tab Window
        /// </summary>
        /// <param name="connection">Which Connection to use</param>
        /// <param name="name">Name of the Window</param>
        /// <param name="windowType">The Window Type</param>
        /// <returns></returns>
        internal IceTabPage GetWindow(IRCConnection connection, string sCaption, IceTabPage.WindowType windowType)
        {
            foreach (IceTabPage t in mainChannelBar.TabPages)
            {
                if (t.TabCaption.ToLower() == sCaption.ToLower() && t.WindowStyle == windowType)
                {
                    if (t.Connection == null && windowType == IceTabPage.WindowType.DCCFile)
                        return t;
                    else if (t.Connection == null && windowType == IceTabPage.WindowType.Debug)
                        return t;
                    else if (t.Connection == connection)
                        return t;
                }
                else if (t.WindowStyle == windowType && windowType == IceTabPage.WindowType.ChannelList)
                {
                    return t;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Get the Current Tab Window
        /// </summary>
        internal IceTabPage CurrentWindow
        {
            get
            {
                return mainChannelBar.CurrentTab;
            }
        }
        
        /// <summary>
        /// Get the Current Window Type
        /// </summary>
        internal IceTabPage.WindowType CurrentWindowStyle
        {
            get
            {
                if (mainChannelBar.CurrentTab != null)
                {
                    
                    return mainChannelBar.CurrentTab.WindowStyle;
                }
                else
                {
                    return IceTabPage.WindowType.Console;
                }
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Send a Message through the IRC Connection to the Server
        /// </summary>
        /// <param name="connection">Which Connection to use</param>
        /// <param name="data">RAW IRC Message to send</param>
        private void SendData(IRCConnection connection, string data)
        {
            try
            {

                if (connection != null)
                {
                    if (connection.IsConnected)
                    {
                        if (connection.IsFullyConnected)
                            connection.SendData(data);
                        else
                            //add to a command queue, which gets run once fully connected, after autoperform/autojoin
                            connection.AddToCommandQueue(data);
                    }
                    else
                    {
                        if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                            WindowMessage(connection, "Console", "\x000304Error: Not Connected to Server (" + data + ")", "", true);
                        else if (CurrentWindow.WindowStyle != IceTabPage.WindowType.ChannelList && CurrentWindow.WindowStyle != IceTabPage.WindowType.DCCFile)
                        {
                            CurrentWindow.TextWindow.AppendText("\x000304Error: Not Connected to Server (" + data + ")", "");
                            CurrentWindow.TextWindow.ScrollToBottom();
                        }
                        else
                        {
                            WindowMessage(connection, "Console", "\x000304Error: Not Connected to Server (" + data + ")", "", true);
                        }
                    }
                }
            }
            catch (NotSupportedException nse)
            {
                System.Diagnostics.Debug.WriteLine("NSE-SendError:" + nse.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SendError:" + e.Message);
            }
        }

        #endregion
       
        /// <summary>
        /// Create a new Server Connection
        /// </summary>
        /// <param name="serverSetting">Which ServerSetting to use</param>
        private void NewServerConnection(ServerSetting serverSetting)
        {            
            IRCConnection c = new IRCConnection(serverSetting);

            c.ChannelMessage += new ChannelMessageDelegate(OnChannelMessage);
            c.ChannelAction += new ChannelActionDelegate(OnChannelAction);
            c.QueryMessage += new QueryMessageDelegate(OnQueryMessage);
            c.QueryAction += new QueryActionDelegate(OnQueryAction);
            c.ChannelNotice += new ChannelNoticeDelegate(OnChannelNotice);

            c.ChangeNick += new ChangeNickDelegate(OnChangeNick);
            c.ChannelKick += new ChannelKickDelegate(OnChannelKick);

            c.OutGoingCommand += new OutGoingCommandDelegate(OutGoingCommand);
            c.JoinChannel += new JoinChannelDelegate(OnChannelJoin);
            c.PartChannel += new PartChannelDelegate(OnChannelPart);
            c.QuitServer += new QuitServerDelegate(OnServerQuit);

            c.JoinChannelMyself += new JoinChannelMyselfDelegate(OnChannelJoinSelf);
            c.PartChannelMyself += new PartChannelMyselfDelegate(OnChannelPartSelf);
            c.ChannelKickSelf += new ChannelKickSelfDelegate(OnChannelKickSelf);

            c.ChannelTopic += new ChannelTopicDelegate(OnChannelTopic);
            c.ChannelMode += new ChannelModeChangeDelegate(OnChannelMode);
            c.UserMode += new UserModeChangeDelegate(OnUserMode);
            c.ChannelInvite += new ChannelInviteDelegate(OnChannelInvite);

            c.ServerMessage += new ServerMessageDelegate(OnServerMessage);
            c.ServerError += new ServerErrorDelegate(OnServerError);
            c.ServerMOTD += new ServerMOTDDelegate(OnServerMOTD);
            c.WhoisData += new WhoisDataDelegate(OnWhoisData);
            c.UserNotice += new UserNoticeDelegate(OnUserNotice);
            c.CtcpMessage += new CtcpMessageDelegate(OnCtcpMessage);
            c.CtcpReply += new CtcpReplyDelegate(OnCtcpReply);
            c.GenericChannelMessage += new GenericChannelMessageDelegate(OnGenericChannelMessage);
            c.ServerNotice += new ServerNoticeDelegate(OnServerNotice);
            c.ChannelListStart += new ChannelListStartDelegate(OnChannelListStart);
            c.ChannelList += new ChannelListDelegate(OnChannelList);
            c.ChannelListEnd += new ChannelListEndDelegate(OnChannelListEnd);
            c.DCCChat += new DCCChatDelegate(OnDCCChat);
            c.DCCFile += new DCCFileDelegate(OnDCCFile);
            c.DCCPassive += new DCCPassiveDelegate(OnDCCPassive);
            c.UserHostReply += new UserHostReplyDelegate(OnUserHostReply);
            c.IALUserData += new IALUserDataDelegate(OnIALUserData);
            c.IALUserDataAwayOnly += new IALUserDataAwayOnlyDelegate(OnIALUserDataAwayOnly);
            c.IALUserChange += new IALUserChangeDelegate(OnIALUserChange);
            c.IALUserAccount += new IALUserAccountDelegate(OnIALAccountChange);
            c.IALUserPart += new IALUserPartDelegate(OnIALUserPart);
            c.IALUserQuit += new IALUserQuitDelegate(OnIALUserQuit);

            c.BuddyListData += new BuddyListDelegate(OnBuddyList);
            c.BuddyListClear += new BuddyListClearDelegate(OnBuddyListClear);
            c.BuddyRemove +=new BuddyRemoveDelegate(OnBuddyRemove);
            c.MonitorListData += new MonitorListDelegate(OnMonitorListData);
            c.RawServerIncomingData += new RawServerIncomingDataDelegate(OnRawServerData);
            c.RawServerOutgoingData += new RawServerOutgoingDataDelegate(OnRawServerOutgoingData);
            c.RawServerIncomingDataOverRide += new RawServerIncomingDataOverRideDelegate(OnRawServerIncomingDataOverRide);

            c.AutoJoin += new AutoJoinDelegate(OnAutoJoin);
            c.AutoRejoin += new AutoRejoinDelegate(OnAutoRejoin);
            c.AutoPerform += new AutoPerformDelegate(OnAutoPerform);

            c.EndofNames += new EndofNamesDelegate(OnEndofNames);
            c.EndofWhoReply += new EndofWhoReplyDelegate(OnEndofWhoReply);
            c.WhoReply += new WhoReplyDelegate(OnWhoReply);
            c.ChannelUserList += new ChannelUserListDelegate(OnChannelUserList);

            c.StatusText += new StatusTextDelegate(OnStatusText);
            c.RefreshServerTree += new RefreshServerTreeDelegate(OnRefreshServerTree);
            c.ServerReconnect += new ServerReconnectDelegate(OnServerReconnect);
            c.ServerDisconnect += new ServerReconnectDelegate(OnServerDisconnect);
            c.ServerConnect += new ServerConnectDelegate(OnServerConnect);
            c.ServerForceDisconnect += new ServerForceDisconnectDelegate(OnServerForceDisconnect);
            c.ServerPreConnect += new ServerPreConnectDelegate(OnServerPreConnect);
            c.UserInfoWindowExists += new UserInfoWindowExistsDelegate(OnUserInfoWindowExists);
            c.UserInfoHostFullName += new UserInfoHostFullnameDelegate(OnUserInfoHostFullName);
            c.UserInfoIdleLogon += new UserInfoIdleLogonDelegate(OnUserInfoIdleLogon);
            c.UserInfoAddChannels += new UserInfoAddChannelsDelegate(OnUserInfoAddChannels);
            c.UserInfoAwayStatus += new UserInfoAwayStatusDelegate(OnUserInfoAwayStatus);
            c.UserInfoLoggedIn+=new UserInfoLoggedInDelegate(OnUserInfoLoggedIn);
            c.UserInfoServer += new UserInfoServerDelegate(OnUserInfoServer);

            c.ChannelInfoWindowExists += new ChannelInfoWindowExistsDelegate(OnChannelInfoWindowExists);
            c.ChannelInfoAddBan += new ChannelInfoAddBanDelegate(OnChannelInfoAddBan);            
            c.ChannelInfoAddException += new ChannelInfoAddExceptionDelegate(OnChannelInfoAddException);
            c.ChannelInfoAddQuiet +=new ChannelInfoAddQuietDelegate(OnChannelInfoAddQuiet);
            c.ChannelInfoTopicSet += new ChannelInfoTopicSetDelegate(OnChannelInfoTopicSet);

            c.AutoAwayTrigger += new AutoAwayDelegate(OnAutoAwayTrigger);
            c.ServerFullyConnected += new ServerForceDisconnectDelegate(OnServerFullyConnected);

            c.WriteErrorFile += new WriteErrorFileDelegate(OnWriteErrorFile);
            c.ParseIdentifier += new IRCConnection.ParseIdentifierDelegate(OnParseIdentifier); 

            OnAddConsoleTab(c);
            
            serverSetting.CurrentNickName = serverSetting.NickName;
            
            serverSetting.LastAwayMessages = new Dictionary<string, DateTime>(); 

            mainChannelBar.SelectTab(mainChannelBar.GetTabPage("Console"));

            inputPanel.CurrentConnection = c;
            serverTree.AddConnection(c);

            c.ConnectSocket();

        }

        #region Tab Events and Methods

        /// <summary>
        /// Add a new Connection Tab to the Console
        /// </summary>
        /// <param name="connection">Which Connection to add</param>
        private void OnAddConsoleTab(IRCConnection connection)
        {            
            mainChannelBar.GetTabPage("Console").AddConsoleTab(connection);
        }

        /// <summary>
        /// Add a new Tab Window to the Main Tab Control
        /// </summary>
        /// <param name="connection">Which Connection it came from</param>
        /// <param name="windowName">Window Name of the New Tab</param>
        /// <param name="windowType">Window Type of the New Tab</param>
        internal IceTabPage AddWindow(IRCConnection connection, string windowName, IceTabPage.WindowType windowType)
        {
            if (this.InvokeRequired)
            {
                AddWindowDelegate a = new AddWindowDelegate(AddWindow);
                return (IceTabPage)this.Invoke(a, new object[] { connection, windowName, windowType });
            }
            else
            {
                try
                {
                    IceTabPage page;

                    //System.Diagnostics.Debug.WriteLine("Add Page:" + mainChannelBar.CurrentTab.TabCaption);
                    IceTabPage currentTab = mainChannelBar.CurrentTab;

                    if (windowType == IceTabPage.WindowType.DCCFile)
                        page = new IceTabPageDCCFile(IceTabPage.WindowType.DCCFile, windowName, this);
                    else
                    {
                        page = new IceTabPage(windowType, windowName, this)
                        {
                            Connection = connection
                        };
                    }

                    if (page.WindowStyle == IceTabPage.WindowType.Channel)
                    {
                        page.TextWindow.Font = new Font(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                        page.ResizeTopicFont(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                        //send the message
                        string msg = GetMessageFormat("Self Channel Join");
                        
                        msg = msg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$channel", windowName);
                        if (connection.ServerSetting.LocalIP != null && connection.ServerSetting.LocalIP.ToString().Length > 0)
                            msg = msg.Replace("$host", connection.ServerSetting.IdentName + "@" + connection.ServerSetting.LocalIP.ToString());
                        else
                            msg = msg.Replace("$host", connection.ServerSetting.CurrentNickName);
                        
                        if (iceChatOptions.LogChannel && page.LoggingDisable == false && connection.ServerSetting.DisableLogging == false)
                        {
                            page.TextWindow.SetLogFile(iceChatOptions.LogReload);
                        }

                        page.TextWindow.AppendText(msg, "");

                        // check server wide no color mode
                        if (connection.ServerSetting.NoColorMode == true)
                        {
                            page.TextWindow.NoColorMode = true;
                        }

                    }
                    else if (page.WindowStyle == IceTabPage.WindowType.Query)
                    {
                        page.TextWindow.Font = new Font(iceChatFonts.FontSettings[2].FontName, iceChatFonts.FontSettings[2].FontSize);
                        if (iceChatOptions.LogQuery && connection.ServerSetting.DisableLogging == false)
                            page.TextWindow.SetLogFile(false);
                    }
                    else if (page.WindowStyle == IceTabPage.WindowType.Debug)
                    {
                        page.TextWindow.NoColorMode = true;
                        page.TextWindow.Font = new Font(iceChatFonts.FontSettings[0].FontName, iceChatFonts.FontSettings[0].FontSize);
                        page.TextWindow.SetLogFile(false);
                        page.TextWindow.SetDebugWindow();
                    }
                    else if (page.WindowStyle == IceTabPage.WindowType.Window)
                    {
                        page.TextWindow.Font = new Font(iceChatFonts.FontSettings[0].FontName, iceChatFonts.FontSettings[0].FontSize);
                        if (iceChatOptions.LogWindow)                        
                            page.TextWindow.SetLogFile(false);
                    }
                    else if (page.WindowStyle == IceTabPage.WindowType.ChannelList)
                    {
                        page.ChannelList.Font = new Font(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                    }

                    //find the last window index for this connection
                    int index = 0;
                    
                    if (page.WindowStyle == IceTabPage.WindowType.Channel || page.WindowStyle == IceTabPage.WindowType.Query || page.WindowStyle == IceTabPage.WindowType.DCCChat || page.WindowStyle == IceTabPage.WindowType.DCCFile)
                    {
                        for (int i = 1; i < mainChannelBar.TabPages.Count; i++)
                        {
                            if (mainChannelBar.TabPages[i].Connection == connection)
                                index = i + 1;
                        }
                    }

                    if (index == 0)
                    {
                        if (mainTabControl.windowedMode == true)
                        {
                            page.DockedForm = true;

                            FormWindow fw = new FormWindow(page)
                            {
                                Text = page.TabCaption
                            };
                            if (windowType == IceTabPage.WindowType.Channel || windowType == IceTabPage.WindowType.Query)
                            {
                                if (windowType == IceTabPage.WindowType.Query)
                                {
                                    if (connection.ServerSetting.IAL.ContainsKey(page.TabCaption))
                                    {
                                        fw.Text += " ("+ ((InternalAddressList)connection.ServerSetting.IAL[page.TabCaption]).Host +") ";
                                    }
                                }
                                fw.Text += " {" + connection.ServerSetting.NetworkName + "}";
                            }

                            fw.MdiParent = this;
                            fw.Show();

                        }
                        else
                        {
                            page.DockedForm = false;

                            mainTabControl.AddTabPage(page);
                        }
                        
                        mainChannelBar.AddTabPage(ref page);
                    }
                    else
                    {
                        if (mainTabControl.windowedMode == true)
                        {
                            page.DockedForm = true;

                            FormWindow fw = new FormWindow(page)
                            {
                                Text = page.TabCaption
                            };
                            if (windowType == IceTabPage.WindowType.Channel || windowType == IceTabPage.WindowType.Query)
                                fw.Text += " {" + connection.ServerSetting.NetworkName + "}";
                            
                            fw.MdiParent = this;
                            fw.Show();
                        }
                        else
                        {
                            page.DockedForm = false;

                            mainTabControl.AddTabPage(page);
                        }

                        mainChannelBar.InsertTabPage(index, ref page);
                    }

                    if (page.WindowStyle == IceTabPage.WindowType.Channel || page.WindowStyle == IceTabPage.WindowType.Query)
                    {
                        page.ChannelSettings(page.Connection.ServerSetting.NetworkName, !mainTabControl.Visible);
                        page.TextWindow.ScrollToBottom();
                    }
                    
                    if (page.WindowStyle == IceTabPage.WindowType.Debug)
                    {
                        ChannelSetting cs = ChannelSettings.FindChannel("Debug","");
                        if (cs != null)
                        {
                            page.PinnedTab = cs.PinnedTab;
                            page.WindowLocation = cs.WindowLocation;
                            page.WindowSize = cs.WindowSize;
                        }
                    }
                    
                    if (page.WindowStyle == IceTabPage.WindowType.Query && !iceChatOptions.NewQueryForegound)
                    {

                        // System.Diagnostics.Debug.WriteLine("SelectTab:" + mainChannelBar.CurrentTab.TabCaption + ":" + currentTab.TabCaption);
                        
                        mainChannelBar.SelectTab(currentTab);
                        serverTree.SelectTab(currentTab, false);
                    }
                    else if (page.WindowStyle == IceTabPage.WindowType.Window)
                    {
                        ChannelSetting cs = ChannelSettings.FindChannel(page.TabCaption, "");
                        if (cs != null)
                        {
                            page.PinnedTab = cs.PinnedTab;
                            page.WindowLocation = cs.WindowLocation;
                            page.WindowSize = cs.WindowSize;
                        }

                        mainChannelBar.Invalidate(); 
                        serverTree.Invalidate();
                    }
                    else
                    {
                        mainChannelBar.SelectTab(page);
                        nickList.CurrentWindow = page;
                        serverTree.SelectTab(page, false);
                    }

                    if (page.WindowStyle == IceTabPage.WindowType.Query && iceChatOptions.WhoisNewQuery == true)
                    {
                        //dont do a whois IF userinfo window is open
                        if (!OnUserInfoWindowExists(page.Connection, page.TabCaption) && page.TabCaption != "*status")
                            ParseOutGoingCommand(page.Connection, "/whois " + page.TabCaption + " " + page.TabCaption);
                    }

                    PluginArgs args = new PluginArgs(page.TextWindow, page.TabCaption, "", "", "")
                    {
                        Extra = page.WindowStyle.ToString(),
                        Connection = connection
                    };

                    foreach (Plugin p in loadedPlugins)
                    {
                        IceChatPlugin ipc = p as IceChatPlugin;
                        if (ipc != null)
                        {
                            if (ipc.plugin.Enabled == true)
                                ipc.plugin.NewWindow(args);
                        }
                    }

                    return page;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Source + ":" + e.StackTrace);
                }
                return null;
            }
        }

        /// <summary>
        /// Close All Channels/Query Tabs for specified Connection
        /// </summary>
        /// <param name="connection">Which Connection it is for</param>
        internal void CloseAllWindows(IRCConnection connection)
        {
            for (int i = mainChannelBar.TabPages.Count - 1; i > 0; i--)
            {
                if (mainChannelBar.TabPages[i].Connection == connection)
                {
                    // remove window if it is a window, not a tab
                    if (mainChannelBar.TabPages[i].Parent.Name == "FormWindow")
                    {
                        ((FormWindow)mainChannelBar.TabPages[i].Parent).DisableActivate();
                        ((FormWindow)mainChannelBar.TabPages[i].Parent).DisableResize();
                        ((FormWindow)mainChannelBar.TabPages[i].Parent).Close();
                    }

                    mainChannelBar.TabPages.Remove(mainChannelBar.TabPages[i]);
                }

            }
            
            mainChannelBar.Invalidate();
            
        }

        internal string GetMessageFormat(string MessageName)
        {
            foreach (ServerMessageFormatItem msg in iceChatMessages.MessageSettings)
            {
                if (msg.MessageName.ToLower() == MessageName.ToLower())
                    return msg.FormattedMessage;
            }
            return null;
        }
        
        
        /// <summary>
        /// A New Tab was Selected for the Main Tab Control
        /// Update the Input Panel with the Current Connection
        /// Change the Status text for the Status Bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabSelectedIndexChanged(object sender, TabEventArgs e)
        {
            if (!this.IsHandleCreated && !this.IsDisposed) return;

            this.Invoke((MethodInvoker)delegate()
            {
                if (mainChannelBar.CurrentTab.WindowStyle != IceTabPage.WindowType.Console)
                {
                    if (mainChannelBar.CurrentTab != null)
                    {
                        IceTabPage t = mainChannelBar.CurrentTab;

                        PluginArgs args = new PluginArgs(t.Connection)
                        {
                            Channel = t.TabCaption
                        };

                        foreach (Plugin p in this.loadedPlugins)
                        {
                            IceChatPlugin ipc = p as IceChatPlugin;
if (ipc != null)
                            {
                                if (ipc.plugin.Enabled == true)
                                    ipc.plugin.SwitchTab(args);

                            }
                        }


                        nickList.RefreshList(t);
                        inputPanel.CurrentConnection = t.Connection;
                        string network = "";

                        if (CurrentWindowStyle != IceTabPage.WindowType.Debug && CurrentWindowStyle != IceTabPage.WindowType.DCCFile && CurrentWindowStyle != IceTabPage.WindowType.Window && t.Connection.ServerSetting.NetworkName.Length > 0)
                            network = " (" + t.Connection.ServerSetting.NetworkName + ")";

                        string away = "";
                        if (inputPanel.CurrentConnection != null && inputPanel.CurrentConnection.ServerSetting.Away == true)
                            away = " {AWAY}";

                        if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                        {
                            //get the current user status mode for the channel
                            if (t.Connection.IsConnected)
                            {
                                User u = t.GetNick(t.Connection.ServerSetting.CurrentNickName);
                                if (u != null)
                                    StatusText(u.ToString() + " in channel " + t.TabCaption + " [" + t.ChannelModes + "] {" + t.Connection.ServerSetting.RealServerName + "}" + network + away);
                                else
                                    StatusText(t.Connection.ServerSetting.CurrentNickName + " in channel " + t.TabCaption + " [" + t.ChannelModes + "] {" + t.Connection.ServerSetting.RealServerName + "}" + network + away);
                            }
                            else
                            {
                                StatusText("Disconnected from channel " + t.TabCaption + " {" + t.Connection.ServerSetting.RealServerName + "}" + network);
                            }
                        }
                        else if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                            StatusText(t.Connection.ServerSetting.CurrentNickName + " in private chat with " + t.TabCaption + " {" + t.Connection.ServerSetting.RealServerName + "}" + network + away);
                        else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                            StatusText(t.Connection.ServerSetting.CurrentNickName + " in DCC chat with " + t.TabCaption + " {" + t.Connection.ServerSetting.RealServerName + "}" + network);
                        else if (CurrentWindowStyle == IceTabPage.WindowType.ChannelList)
                            StatusText(t.Connection.ServerSetting.CurrentNickName + " in Channel List for {" + t.Connection.ServerSetting.RealServerName + "}" + network);

                        CurrentWindow.LastMessageType = ServerMessageType.Default;
                        t = null;

                        if (e.IsHandled == false)
                        {
                            serverTree.SelectTab(mainChannelBar.CurrentTab, false);
                        }

                    }
                }
                else
                {
                    //make sure the 1st tab is not selected
                    nickList.RefreshList();
                    nickList.Header = iceChatLanguage.consoleTabTitle;
                    
                    if (mainChannelBar.GetTabPage("Console").ConsoleTab.SelectedIndex != 0)
                    {
                        inputPanel.CurrentConnection = mainChannelBar.GetTabPage("Console").CurrentConnection;

                        string network = "";
                        if (inputPanel.CurrentConnection.ServerSetting.NetworkName.Length > 0)
                            network = " (" + inputPanel.CurrentConnection.ServerSetting.NetworkName + ")";

                        if (inputPanel.CurrentConnection.IsConnected)
                        {
                            string ssl = "";
                            if (inputPanel.CurrentConnection.ServerSetting.UseSSL)
                                ssl = " {SSL}";
                            
                            string away = "";
                            if (inputPanel.CurrentConnection.ServerSetting.Away == true)
                                away = " {AWAY}";

                            if (inputPanel.CurrentConnection.ServerSetting.UseBNC)
                                StatusText(inputPanel.CurrentConnection.ServerSetting.CurrentNickName + " connected to " + inputPanel.CurrentConnection.ServerSetting.RealServerName + " {BNC " + inputPanel.CurrentConnection.ServerSetting.BNCIP + "}" + away);
                            else
                                StatusText(inputPanel.CurrentConnection.ServerSetting.CurrentNickName + " connected to " + inputPanel.CurrentConnection.ServerSetting.RealServerName + ssl + network + away);
                        }
                        else
                        {
                            if (inputPanel.CurrentConnection.ServerSetting.UseBNC)
                                StatusText(inputPanel.CurrentConnection.ServerSetting.CurrentNickName + " disconnected from " + inputPanel.CurrentConnection.ServerSetting.BNCIP + " {BNC}");
                            else
                                StatusText(inputPanel.CurrentConnection.ServerSetting.CurrentNickName + " disconnected from " + inputPanel.CurrentConnection.ServerSetting.ServerName + network);

                        }
                        
                        //what is the current server - reset the color
                        foreach (ConsoleTab t in mainChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
                        {
                            if (t.Connection != null && t.Connection.ServerSetting == inputPanel.CurrentConnection.ServerSetting)
                                t.LastMessageType = ServerMessageType.Default;                            
                        }

                        if (e.IsHandled == false)
                        {
                            serverTree.SelectTab(mainChannelBar.GetTabPage("Console").CurrentConnection.ServerSetting, false);
                        }
                    }
                    else
                    {
                        inputPanel.CurrentConnection = null;
                        StatusText("Welcome to " + ProgramID + " " + VersionID);
                    }
                }

                this.FocusInputBox();
            });
        }


       
        /// <summary>
        /// Closes the Tab selected
        /// </summary>
        /// <param name="nIndex">Which tab to Close</param>        
        private void OnTabClosed(int nIndex)
        {
            if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.Channel)
            {
                foreach (IRCConnection c in serverTree.ServerConnections.Values)
                {
                    if (c == mainChannelBar.GetTabPage(nIndex).Connection)
                    {
                        //check if connected
                        if (c.IsConnected)
                        {
                            System.Diagnostics.Debug.WriteLine("Close Tab/Send PART:" + mainChannelBar.GetTabPage(nIndex).TabCaption + ":" + c.ServerSetting.UseBNC);
                            
                            ParseOutGoingCommand(c, "/part " + mainChannelBar.GetTabPage(nIndex).TabCaption);
                        }
                        else
                        {
                            mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                            RemoveWindow(c, mainChannelBar.GetTabPage(nIndex).TabCaption, mainChannelBar.GetTabPage(nIndex).WindowStyle);
                        }
                        return;
                    }
                }
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.Query)
            {
                mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.Query);
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.Console)
            {
                if (mainChannelBar.GetTabPage("Console").CurrentConnection != null)
                {
                    if (mainChannelBar.GetTabPage("Console").CurrentConnection.IsConnected)
                    {
                        ParseOutGoingCommand(mainChannelBar.GetTabPage("Console").CurrentConnection, "/quit");
                    }
                    else
                    {
                        //remove the tab
                        mainChannelBar.GetTabPage("Console").RemoveConsoleTab(mainChannelBar.GetTabPage("Console").ConsoleTab.SelectedIndex);
                    }
                }
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.ChannelList)
            {
                //check if channel list is completed or not, do not close if it has not
                if (mainChannelBar.GetTabPage(nIndex).ChannelListComplete)
                {
                    mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                    RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.ChannelList);
                }
                else
                    System.Media.SystemSounds.Beep.Play();
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.DCCChat)
            {
                mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.DCCChat);
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.DCCFile)
            {
                mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.DCCFile);
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.Window)
            {
                mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.Window);
            }
            else if (mainChannelBar.GetTabPage(nIndex).WindowStyle == IceTabPage.WindowType.Debug)
            {
                mainTabControl.Controls.Remove(mainChannelBar.GetTabPage(nIndex));
                RemoveWindow(mainChannelBar.GetTabPage(nIndex).Connection, mainChannelBar.GetTabPage(nIndex).TabCaption, IceTabPage.WindowType.Debug);
            }
        }

        /// <summary>
        /// Remove a Tab Window from the Main Tab Control
        /// </summary>
        /// <param name="connection">Which Connection it is for</param>
        /// <param name="channel">The Channel/Query Window Name</param>
        internal void RemoveWindow(IRCConnection connection, string windowCaption, IceTabPage.WindowType windowType)
        {
            if (!this.IsHandleCreated && !this.IsDisposed) return;

            this.Invoke((MethodInvoker)delegate()
            {
                if (windowType == IceTabPage.WindowType.Channel)
                {

                    IceTabPage t = GetWindow(connection, windowCaption, IceTabPage.WindowType.Channel);
                    if (t != null)
                    {
                        //System.Diagnostics.Debug.WriteLine("remove:" + t.Parent.Name);
                        if (t.Parent != null && t.Parent.GetType() == typeof(FormWindow))
                        {
                            //save the channel position?                        
                            if (IceChatOptions.SaveWindowPosition == true)
                            {
                                ChannelSetting cs = ChannelSettings.FindChannel(((FormWindow)t.Parent).DockedControl.TabCaption, ((FormWindow)t.Parent).DockedControl.Connection.ServerSetting.NetworkName);
                                if (cs != null)
                                {
                                    cs.WindowLocation = ((FormWindow)t.Parent).Location;
                                    if (((FormWindow)t.Parent).WindowState == FormWindowState.Normal)
                                        cs.WindowSize = ((FormWindow)t.Parent).Size;
                                }
                                else
                                {
                                    ChannelSetting cs1 = new ChannelSetting
                                    {
                                        ChannelName = ((FormWindow)t.Parent).DockedControl.TabCaption,
                                        NetworkName = ((FormWindow)t.Parent).DockedControl.Connection.ServerSetting.NetworkName,
                                        WindowLocation = ((FormWindow)t.Parent).Location
                                    };
                                    if (((FormWindow)t.Parent).WindowState == FormWindowState.Normal)
                                        cs1.WindowSize = ((FormWindow)t.Parent).Size;

                                    this.channelSettings.AddChannel(cs1);
                                }

                                SaveChannelSettings();
                            }

                            ((FormWindow)t.Parent).DisableActivate();
                            ((FormWindow)t.Parent).DisableResize();
                            ((FormWindow)t.Parent).Close();
                        }
                        ChannelBar.TabPages.Remove(t);
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(t);

                        return;
                    }

                }

                if (windowType == IceTabPage.WindowType.Query)
                {
                    IceTabPage c = GetWindow(connection, windowCaption, IceTabPage.WindowType.Query);
                    if (c != null)
                    {
                        if (c.Parent != null && c.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)c.Parent).DisableActivate();
                            ((FormWindow)c.Parent).DisableResize();
                            ((FormWindow)c.Parent).Close();
                        }

                        ChannelBar.TabPages.Remove(c);
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(c);
                        return;
                    }
                }

                if (windowType == IceTabPage.WindowType.DCCChat)
                {
                    IceTabPage dcc = GetWindow(connection, windowCaption, IceTabPage.WindowType.DCCChat);
                    if (dcc != null)
                    {
                        if (dcc.Parent != null && dcc.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)dcc.Parent).DisableActivate();
                            ((FormWindow)dcc.Parent).DisableResize();
                            ((FormWindow)dcc.Parent).Close();
                        }
                        ChannelBar.TabPages.Remove(dcc);
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(dcc);
                        return;
                    }
                }

                if (windowType == IceTabPage.WindowType.ChannelList)
                {
                    IceTabPage cl = GetWindow(connection, "", IceTabPage.WindowType.ChannelList);
                    if (cl != null)
                    {
                        if (cl.Parent != null && cl.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)cl.Parent).DisableActivate();
                            ((FormWindow)cl.Parent).DisableResize();
                            ((FormWindow)cl.Parent).Close();
                        }

                        ChannelBar.TabPages.Remove(cl);
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(cl);
                        return;
                    }
                }

                if (windowType == IceTabPage.WindowType.Debug)
                {
                    if (windowType == IceTabPage.WindowType.Debug)
                    {
                        IceTabPage de = GetWindow(null, "Debug", IceTabPage.WindowType.Debug);
                        if (de != null)
                        {

                            if (de.Parent != null && de.Parent.GetType() == typeof(FormWindow))
                            {
                                ((FormWindow)de.Parent).DisableActivate();
                                ((FormWindow)de.Parent).DisableResize();
                                ((FormWindow)de.Parent).Close();
                            }

                            ChannelBar.TabPages.Remove(de);
                            if (mainTabControl.Controls.Count == 0)
                                this.serverTree.Invalidate();
                            else
                                mainTabControl.Controls.Remove(de);
                            return;
                        }
                    }
                }

                if (windowType == IceTabPage.WindowType.Window)
                {
                    IceTabPage wi = GetWindow(null, windowCaption, IceTabPage.WindowType.Window);
                    if (wi != null)
                    {
                        if (wi.Parent != null && wi.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)wi.Parent).DisableActivate();
                            ((FormWindow)wi.Parent).DisableResize();
                            ((FormWindow)wi.Parent).Close();
                        }
                        ChannelBar.TabPages.Remove(wi);
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(wi);
                        return;
                    }
                }

                if (windowType == IceTabPage.WindowType.DCCFile)
                {
                    IceTabPage df = GetWindow(null, windowCaption, IceTabPage.WindowType.DCCFile);
                    if (df != null)
                    {
                        if (df.Parent != null && df.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)df.Parent).DisableActivate();
                            ((FormWindow)df.Parent).DisableResize();
                            ((FormWindow)df.Parent).Close();
                        }
                        
                        ChannelBar.TabPages.Remove(df);
                        
                        if (mainTabControl.Controls.Count == 0)
                            this.serverTree.Invalidate();
                        else
                            mainTabControl.Controls.Remove(df);
                        return;
                    }
                }
            });
        }

        #endregion
        


        private void CreateTimer(string id, int reps, double interval, string command)
        {
            IrcTimer t = new IrcTimer(id, reps, interval * 1000, command);
            t.OnTimerElapsed += new IrcTimer.TimerElapsed(OnTimerElapsed);            
            _globalTimers.Add(t);            
            t.Start();
        }

        private void OnTimerElapsed(string timerID, string command)
        {
            ParseOutGoingCommand(null, command);
        }

        private void DestroyTimer(string id)
        {    
            IrcTimer timer = _globalTimers.Find(
                delegate(IrcTimer t)
                {
                    return t.TimerID == id;
                }
            );
            
            if (timer != null)
                _globalTimers.Remove(timer);
        }

        
        /// <summary>
        /// Input Panel Text Box had Entered Key Pressed or Send Button Pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void InputPanel_OnCommand(object sender, string data)
        {
            if (data.Length > 0)
            {                
                ParseOutGoingCommand(inputPanel.CurrentConnection, data);
                if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                    mainChannelBar.CurrentTab.CurrentConsoleWindow().ScrollToBottom();
                else if (CurrentWindowStyle != IceTabPage.WindowType.DCCFile && CurrentWindowStyle != IceTabPage.WindowType.ChannelList)
                    CurrentWindow.TextWindow.ScrollToBottom();
            
            
                //auto away settings
                if (inputPanel.CurrentConnection != null)
                {
                    if (data.StartsWith("/") == false)
                    {
                        if (iceChatOptions.AutoReturn == true)
                        {
                            if (inputPanel.CurrentConnection.ServerSetting.Away == true)
                            {
                                //return yourself
                                ParseOutGoingCommand(inputPanel.CurrentConnection, "/away");
                            }
                        }
                    }

                    //reset the auto away timer
                    if (data.StartsWith("/") == false)
                    {
                        if (iceChatOptions.AutoAway == true && iceChatOptions.AutoAwayTime > 0)
                        {
                            inputPanel.CurrentConnection.SetAutoAwayTimer(iceChatOptions.AutoAwayTime);
                        }
                    }
                }
            }
        }

        private void InputPanel_OnHotKey(object sender, KeyEventArgs e)
        {
            PluginArgs args = new PluginArgs(inputPanel.CurrentConnection)
            {
                Extra = ((System.Windows.Forms.TextBox)sender).Text
            };

            foreach (Plugin p in loadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                        args = ipc.plugin.HotKey(args, e);

                }
            }

            inputPanel.ClearTextBox();
            if (args.Extra.Length > 0)
            {
                AddInputPanelText(args.Extra);
            }
        }

        private void AddInputPanelText(string data)
        {
            if (this.InvokeRequired)
            {
                AddInputpanelTextDelegate a = new AddInputpanelTextDelegate(AddInputPanelText);
                this.Invoke(a, new object[] { data });
            }
            else
            {
                inputPanel.AppendText(data);
                FocusInputBox();
            }
        }

        /// <summary>
        /// Get the Host from the Full User Host, including Ident
        /// </summary>
        /// <param name="host">Full User Host (user!ident@host)</param>
        /// <returns></returns>
        private string HostFromFullHost(string host)
        {
            if (host.IndexOf("!") > -1)
                return host.Substring(host.LastIndexOf("!") + 1);
            else
                return host;
        }

        /// <summary>
        /// Return the Nick Name from the Full User Host
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private string NickFromFullHost(string host)
        {
            if (host.StartsWith(":"))
                host = host.Substring(1);

            if (host.IndexOf("!") > 0)
                return host.Substring(0, host.LastIndexOf("!"));
            else
                return host;
        }

        private string OnParseIdentifier(IRCConnection connection, string message)
        {
            return ParseIdentifiers(connection, message, message);
        }
        
        
        //rejoin an arrayed string into a single string, not adding null values
        private string JoinString(string[] joinString)
        {
            string joined = "";
            foreach (string j in joinString)
            {
                if (j != null)
                    joined += j + " ";
            }
            if (joined.Length > 0)
                joined = joined.Substring(0, joined.Length - 1);
            return joined;
        }

        private string ReturnBracketValue(string data)
        {
            //return what is between ( ) brackets
            string d = data.Substring(data.IndexOf('(') + 1);
            return d.Substring(0, d.LastIndexOf(')'));
        }
        
        private string ReturnPropertyValue(string data)
        {
            if (data.IndexOf('.') == -1)
                return "";
            else
                return data.Substring(data.LastIndexOf('.') + 1);
        }

        //replace 1st occurence of a string inside another string
        private string ReplaceFirst(string haystack, string needle, string replacement)
        {
            int pos = haystack.IndexOf(needle);
            if (pos < 0) return haystack;

            return haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
        }

        private string GetDuration(double seconds)
        {
            TimeSpan t = new TimeSpan(0, 0,(int)seconds);

            string s = t.Seconds.ToString() + " secs";
            if (t.Minutes > 0)
                s = t.Minutes.ToString() + " mins " + s;
            if (t.Hours > 0)
                s = t.Hours.ToString() + " hrs " + s;
            if (t.Days > 0)
                s = t.Days.ToString() + " days " + s;

            return s;
        }

        private string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        #region Menu and ToolStrip Items

        /// <summary>
        /// Close the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IceChatChannelStripMenuItem_Click(object sender, System.EventArgs e)
        {
            bool match = false;
            foreach (IRCConnection c in FormMain.Instance.ServerTree.ServerConnections.Values)
            {
                if (c.IsConnected)
                {
                    if (c.ServerSetting.NetworkName.ToLower() == "quakenet")
                    {
                        //network match
                        FormMain.Instance.ParseOutGoingCommand(c, "/join #icechat");

                        match = true;
                    }
                }
            }
            if (!match)            
                ParseOutGoingCommand(null, "/joinserv irc.quakenet.org #icechat");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show the about box
            FormAbout fa = new FormAbout();
            fa.Show(this);
        }

        private void MinimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinimizeToTray();
        }


        private void NotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == true)
            {
                this.Activate();

                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                this.iceChatOptions.IsOnTray = false;
                this.Show();
                this.WindowState = previousWindowState;
                this.notifyIcon.Visible = iceChatOptions.ShowSytemTrayIcon;
            }
        }

        private void IceChatSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //bring up a very basic settings form
            if (Application.OpenForms["FormSettings"] as FormSettings != null)
            {
                System.Diagnostics.Debug.WriteLine("Form Settings Open");
                Application.OpenForms["FormSettings"].BringToFront();
                return;
            }

            FormSettings fs = new FormSettings(iceChatOptions, iceChatFonts, iceChatEmoticons, iceChatSounds);
            fs.SaveOptions += new FormSettings.SaveOptionsDelegate(Fs_SaveOptions);
            fs.Show(this);
        }

        private void Fs_SaveOptions()
        {
            SaveOptions();           
            SaveFonts();
            SaveSounds();

            //implement the new Font Settings
            //System.Diagnostics.Debug.WriteLine("Save options");

            //do all the Console Tabs Windows
            foreach (ConsoleTab c in mainChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
            {
                //System.Diagnostics.Debug.WriteLine("Change Console Font");
                ((TextWindow)c.Controls[0]).Font = new Font(iceChatFonts.FontSettings[0].FontName, iceChatFonts.FontSettings[0].FontSize);
                if (((TextWindow)c.Controls[0]).MaximumTextLines != iceChatOptions.MaximumTextLines)
                    ((TextWindow)c.Controls[0]).MaximumTextLines = iceChatOptions.MaximumTextLines;

                ((TextWindow)c.Controls[0]).Invalidate();
            }
            
            //do all the Channel and Query Tabs Windows
            foreach (IceTabPage t in mainChannelBar.TabPages)
            {
                if (t.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    t.TextWindow.Font = new Font(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                    t.ResizeTopicFont(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                    t.TextWindow.ReloadText = iceChatOptions.LogReload;
                }
                if (t.WindowStyle == IceTabPage.WindowType.Query)
                {
                    t.TextWindow.Font = new Font(iceChatFonts.FontSettings[2].FontName, iceChatFonts.FontSettings[2].FontSize);
                    t.TextWindow.Invalidate();
                }
                if (t.WindowStyle == IceTabPage.WindowType.DCCChat)
                {
                    t.TextWindow.Font = new Font(iceChatFonts.FontSettings[2].FontName, iceChatFonts.FontSettings[2].FontSize);
                    t.TextWindow.Invalidate();
                }
                if (t.WindowStyle == IceTabPage.WindowType.Window)
                {
                    t.TextWindow.Font = new Font(iceChatFonts.FontSettings[1].FontName, iceChatFonts.FontSettings[1].FontSize);
                    t.TextWindow.Invalidate();
                }
                if (t.WindowStyle != IceTabPage.WindowType.Console && t.WindowStyle != IceTabPage.WindowType.DCCFile && t.WindowStyle != IceTabPage.WindowType.ChannelList)
                {
                    //check if value is different
                    if (t.TextWindow.MaximumTextLines != iceChatOptions.MaximumTextLines)
                        t.TextWindow.MaximumTextLines = iceChatOptions.MaximumTextLines;
                }

                

            }
            
            //change the server list
            serverTree.Font = new Font(iceChatFonts.FontSettings[4].FontName, iceChatFonts.FontSettings[4].FontSize);
            serverTree.ShowServerButtons = iceChatOptions.ShowServerButtons;

            //change the nick list
            nickList.Font = new Font(iceChatFonts.FontSettings[3].FontName, iceChatFonts.FontSettings[3].FontSize);
            nickList.ShowNickButtons = iceChatOptions.ShowNickButtons;

            mainChannelBar.TabFont = new Font(iceChatFonts.FontSettings[8].FontName, iceChatFonts.FontSettings[8].FontSize);
            mainChannelBar.SingleRow = iceChatOptions.SingleRowTabBar;

            //change the fonts for the Left and Right Dock Panels
            panelDockLeft.Initialize();
            panelDockRight.Initialize();

            //change system tray text and icon and visibility
            this.notifyIcon.Visible = iceChatOptions.ShowSytemTrayIcon;
            
            if (iceChatOptions.SystemTrayText == null || iceChatOptions.SystemTrayText.Trim().Length == 0)
                this.notifyIcon.Text = ProgramID + " " + VersionID;
            else
                this.notifyIcon.Text = iceChatOptions.SystemTrayText;

            if (iceChatOptions.SystemTrayIcon == null || iceChatOptions.SystemTrayIcon.Trim().Length == 0)
            {
                this.notifyIcon.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-tray-icon.ico").GetHicon());
            }
            else
            {
                //make sure the image exists and is an ICO file                
                if (File.Exists(iceChatOptions.SystemTrayIcon))
                    this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(iceChatOptions.SystemTrayIcon);
                else
                    this.notifyIcon.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-tray-icon.ico").GetHicon());
            }
            
            //this.resizeWindowToolStripMenuItem.Visible = !iceChatOptions.WindowedMode;

            //update the logs folder
            this.logsFolder = iceChatOptions.LogFolder;

            //change the main Menu Bar Font
            menuMainStrip.Font = new Font(iceChatFonts.FontSettings[7].FontName, iceChatFonts.FontSettings[7].FontSize);
            toolStripMain.Font = new Font(iceChatFonts.FontSettings[7].FontName, iceChatFonts.FontSettings[7].FontSize);

            panelDockLeft.TabControl.Font = new Font(iceChatFonts.FontSettings[6].FontName, iceChatFonts.FontSettings[6].FontSize);
            panelDockRight.TabControl.Font = new Font(iceChatFonts.FontSettings[6].FontName, iceChatFonts.FontSettings[6].FontSize);

            //change the inputbox font
            inputPanel.InputBoxFont = new Font(iceChatFonts.FontSettings[5].FontName, iceChatFonts.FontSettings[5].FontSize);

            //set if Emoticon Picker/Color Picker is Visible
            inputPanel.ShowEmoticonPicker = iceChatOptions.ShowEmoticonPicker;
            inputPanel.ShowColorPicker = iceChatOptions.ShowColorPicker;
            inputPanel.ShowBasicCommands = iceChatOptions.ShowBasicCommands; 
            inputPanel.ShowSendButton = iceChatOptions.ShowSendButton;

            if (iceChatOptions.ShowEmoticons == false)
                inputPanel.ShowEmoticonPicker = false;

            //set if Status Bar is Visible
            statusStripMain.Visible = iceChatOptions.ShowStatusBar;

            foreach (IRCConnection c in serverTree.ServerConnections.Values)
            {
                if (c.IsConnected)
                {
                    if (iceChatOptions.AutoAway == true && iceChatOptions.AutoAwayTime > 0)
                        c.SetAutoAwayTimer(iceChatOptions.AutoAwayTime);                    
                    else
                        c.DisableAutoAwayTimer();
                }
            }
        }

        private void ServerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitterLeft.Visible = serverListToolStripMenuItem.Checked;
            panelDockLeft.Visible = serverListToolStripMenuItem.Checked;
            iceChatOptions.ShowServerTree = serverListToolStripMenuItem.Checked;
        }

        private void NickListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitterRight.Visible = nickListToolStripMenuItem.Checked;
            panelDockRight.Visible = nickListToolStripMenuItem.Checked;
            iceChatOptions.ShowNickList = nickListToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStripMain.Visible = statusBarToolStripMenuItem.Checked;
            iceChatOptions.ShowStatusBar = statusBarToolStripMenuItem.Checked;
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMain.Visible = toolBarToolStripMenuItem.Checked;
            iceChatOptions.ShowToolBar = toolBarToolStripMenuItem.Checked;

            menuMainStrip.SendToBack();

        }

        private void GitHubPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://github.com/icechat/IceChat");
            }
            catch { }
        }

        /* Forums are retired
        private void ForumsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://www.icechat.net/forums");
            }
            catch { }
        }
        */

        private void IceChatHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://www.icechat.net/");
            }
            catch { }
        }

        private void FacebookFanPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://www.facebook.com/IceChat");
            }
            catch { }
        }

        private void DownloadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://github.com/icechat/IceChat/tree/master/src/Plugins");
            }
            catch { }
        }

        private void IceChatColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["FormColors"] as FormColors != null)
            {
                Application.OpenForms["FormColors"].BringToFront();
                return;
            }

            //bring up a very basic settings form
            FormColors fc = new FormColors(iceChatMessages, iceChatColors);
            fc.SaveColors += new FormColors.SaveColorsDelegate(Fc_SaveColors);
            fc.StartPosition = FormStartPosition.CenterParent;
            
            fc.Show(this);
        }

        private void Fc_SaveColors(IceChatColors colors, IceChatMessageFormat messages)
        {
            this.iceChatColors = colors;
            SaveColors();
            
            this.iceChatMessages = messages;
            SaveMessageFormat();

            toolStripMain.BackColor = IrcColor.colors[iceChatColors.ToolbarBackColor];
            menuMainStrip.BackColor = IrcColor.colors[iceChatColors.MenubarBackColor];
            statusStripMain.BackColor = IrcColor.colors[iceChatColors.StatusbarBackColor];
            toolStripStatus.ForeColor = IrcColor.colors[iceChatColors.StatusbarForeColor];

            splitterLeft.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];
            splitterRight.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];
            splitterBottom.BackColor = IrcColor.colors[iceChatColors.SideBarSplitter];

            serverListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            serverListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
            nickListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            nickListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
            channelListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            channelListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];
            buddyListTab.BackColor = IrcColor.colors[iceChatColors.PanelHeaderBG1];
            buddyListTab.ForeColor = IrcColor.colors[iceChatColors.PanelHeaderForeColor];

            inputPanel.SetInputBoxColors();
            
            channelList.SetListColors();
            buddyList.SetListColors();
            serverTree.SetListColors();
            nickList.SetListColors();

            nickList.Invalidate();
            mainChannelBar.Invalidate();
            serverTree.Invalidate();
            
            buddyList.Invalidate();
            channelList.Invalidate();

            panelDockLeft.TabControl.Invalidate();
            panelDockRight.TabControl.Invalidate();

            //rebuild the themes menu
            foreach (ToolStripMenuItem t in themesToolStripMenuItem.DropDownItems)
                t.Click -= ThemeChoice_Click;
            
            themesToolStripMenuItem.DropDownItems.Clear();
            this.themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultToolStripMenuItem});

            defaultToolStripMenuItem.Click +=new EventHandler(DefaultToolStripMenuItem_Click);

            foreach (ThemeItem theme in IceChatOptions.Theme)
            {
                if (!theme.ThemeName.Equals("Default"))
                {
                    ToolStripMenuItem t = new ToolStripMenuItem(theme.ThemeName);
                    if (iceChatOptions.CurrentTheme == theme.ThemeName)
                        t.Checked = true;

                    t.Click += new EventHandler(ThemeChoice_Click);
                    themesToolStripMenuItem.DropDownItems.Add(t);
                }
            }

            //save options (for theme values)
            SaveOptions();

            //update all the console windows
            foreach (ConsoleTab c in mainChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
            {
                ((TextWindow)c.Controls[0]).IRCBackColor = iceChatColors.ConsoleBackColor;
            }

            //update all the Channel and Query Tabs Windows
            foreach (IceTabPage t in mainChannelBar.TabPages)
            {
                if (t.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    t.TopicWindow.IRCBackColor = iceChatColors.ChannelBackColor;
                    t.TextWindow.IRCBackColor = iceChatColors.ChannelBackColor;
                }
                if (t.WindowStyle == IceTabPage.WindowType.Query)
                    t.TextWindow.IRCBackColor = iceChatColors.QueryBackColor;

                if (t.WindowStyle == IceTabPage.WindowType.DCCChat)
                    t.TextWindow.IRCBackColor = iceChatColors.QueryBackColor;
            }

        }

        private void ToolStripSettings_Click(object sender, EventArgs e)
        {
            iceChatSettingsToolStripMenuItem.PerformClick();
        }

        private void ToolStripColors_Click(object sender, EventArgs e)
        {
            iceChatColorsToolStripMenuItem.PerformClick();
        }

        private void ToolStripFonts_Click(object sender, EventArgs e)
        {
            fontSettingsToolStripMenuItem.PerformClick();
        }

        private void FontSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["FormSettings"] as FormSettings != null)
            {
                Application.OpenForms["FormSettings"].BringToFront();
                return;
            }

            FormSettings fs = new FormSettings(iceChatOptions, iceChatFonts, iceChatEmoticons, iceChatSounds, true);
            fs.SaveOptions += new FormSettings.SaveOptionsDelegate(Fs_SaveOptions);

            fs.Show(this);
        }

        
        private void ToolStripQuickConnect_Click(object sender, EventArgs e)
        {
            //popup a small dialog asking for basic server settings
            if (Application.OpenForms["QuickConnect"] as QuickConnect != null)
            {
                Application.OpenForms["QuickConnect"].BringToFront();
                return;
            }

            QuickConnect qc = new QuickConnect();            
            qc.QuickConnectServer += new QuickConnect.QuickConnectServerDelegate(OnQuickConnectServer);
            qc.Show(this);
        }

        private void OnQuickConnectServer(ServerSetting s)
        {
            s.AltNickName =  s.NickName + "_";
            s.AwayNickName = s.NickName + "[A]";
            s.FullName = iceChatOptions.DefaultFullName;
            s.QuitMessage = iceChatOptions.DefaultQuitMessage;
            s.IdentName = iceChatOptions.DefaultIdent;
            s.IAL = new Hashtable();

            Random r = new Random();
            s.ID = r.Next(50000, 99999);

            NewServerConnection(s);
        }

        private void ToolStripAway_Click(object sender, EventArgs e)
        {
            //check if away or not
            if (InputPanel.CurrentConnection != null)
            {
                if (inputPanel.CurrentConnection.ServerSetting.Away)
                {
                    ParseOutGoingCommand(inputPanel.CurrentConnection, "/away");
                }
                else
                {
                    //ask for an away reason
                    InputBoxDialog i = new InputBoxDialog
                    {
                        FormCaption = "Enter your away Reason",
                        FormPrompt = "Away Reason"
                    };

                    i.ShowDialog();
                    if (i.InputResponse.Length > 0)
                        ParseOutGoingCommand(inputPanel.CurrentConnection, "/away " + i.InputResponse);
                    
                    i.Dispose();
                }
            }
        }

        private void ToolStripUpdate_Click(object sender, EventArgs e)
        {
            //update is available, start the updater
            DialogResult result = MessageBox.Show("Would you like to update to a newer version of IceChat or IceChat Plugins?", "IceChat/Plugin update(s) available", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                RunUpdater();
            }
        }

        private bool DownloadPlugin(string url, string pluginName)
        {

            string pluginFile = pluginsFolder + System.IO.Path.DirectorySeparatorChar + pluginName + ".dll";
            
            if (File.Exists(pluginFile))
            {
                return false;
            }

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Timeout = 10000;
            request.ReadWriteTimeout = 10000;

            System.Net.WebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            
            using (BinaryReader responseReader = new BinaryReader(response.GetResponseStream()))
            {
                byte[] bytes = responseReader.ReadBytes((int)response.ContentLength);
                using (BinaryWriter sw = new BinaryWriter(File.OpenWrite(pluginFile)))
                {
                    sw.Write(bytes);
                    sw.Flush();
                    sw.Close();
                }
            }
            
            /*

            System.Net.WebClient downloadClient = new System.Net.WebClient();

            Uri uri = new Uri(url);

            downloadClient.DownloadFile(uri, pluginsFolder + System.IO.Path.DirectorySeparatorChar + pluginName + ".dll");
            */

            return true;
        }

        private void RunUpdater()
        {
            // USE_NET_45
            // Download the IceChatUpdater File

            string updaterXML = "updater-45.xml";

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML);

            System.Xml.XmlNodeList updaterFile = xmlDoc.GetElementsByTagName("file");
            System.Net.WebClient webClient = new System.Net.WebClient();

            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(WebClient_DownloadFileCompleted);

            string f = System.IO.Path.GetFileName(updaterFile[0].InnerText);

            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + f))
                File.Delete(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + f);

            Uri uri = new Uri(updaterFile[0].InnerText);

            string localFile = Path.GetFileName(uri.ToString());

            webClient.DownloadFileAsync(uri, currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + localFile);


        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + "IceChatUpdater.exe"))
            {
                System.Diagnostics.Process process = null;
                System.Diagnostics.ProcessStartInfo processStartInfo;

                processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + "IceChatUpdater.exe"
                };
                //System.Diagnostics.Debug.WriteLine(processStartInfo.FileName);

                if (System.Environment.OSVersion.Version.Major >= 6)  // Windows Vista or higher
                {
                    processStartInfo.Verb = "runas";
                }
                else
                {
                    // No need to prompt to run as admin
                }

                // different update files for the 4.5 builds
                // USE_NET_45
                //processStartInfo.Arguments = "\"-45\" " +  "\"" + Application.StartupPath + "\"";

                processStartInfo.Arguments = "\"" + Application.StartupPath + "\"";
                

                try
                {

                    processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    processStartInfo.UseShellExecute = true;

                    process = System.Diagnostics.Process.Start(processStartInfo);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            }           
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripMain.Visible = false;
        }

        private void IceChatEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["FormEditor"] as FormEditor != null)
            {
                Application.OpenForms["FormEditor"].BringToFront();
                return;
            }            

            FormEditor fe = new FormEditor();
            fe.Show(this);
        }

        private void ToolStripSystemTray_Click(object sender, EventArgs e)
        {
            MinimizeToTray();
        }

        private void ToolStripEditor_Click(object sender, EventArgs e)
        {
            iceChatEditorToolStripMenuItem.PerformClick();
        }

        private void DebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //add the debug window, if it does not exist
            if (GetWindow(null, "Debug", IceTabPage.WindowType.Debug) == null)
            {
                AddWindow(null, "Debug", IceTabPage.WindowType.Debug);
                mainChannelBar.SelectTab(mainChannelBar.GetTabPage("Debug"));
            }
            else
            {
                //close the tab
                RemoveWindow(null, "Debug", IceTabPage.WindowType.Debug);
            }
            ChannelBar.Invalidate();
            serverTree.Invalidate();
        }

        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iceChatOptions.IsOnTray = false;
            this.Show();
            this.notifyIcon.Visible = iceChatOptions.ShowSytemTrayIcon;
        }

        private void ExitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.exitToolStripMenuItem.PerformClick();
        }

        private void OnPluginMenuItemClick(object sender, EventArgs e)
        {
            //show plugin information
            FormPluginInfo pi = new FormPluginInfo((IPluginIceChat)((ToolStripMenuItem)sender).Tag, (ToolStripMenuItem)sender);            
            pi.ShowDialog(this);
        }

        private void MinimizeToTray()
        {
            this.previousWindowState = this.WindowState;
            this.Hide();
            this.notifyIcon.Visible = true;
            this.iceChatOptions.IsOnTray = true;

            if (iceChatOptions.AutoAwaySystemTray)
            {
                foreach (IRCConnection c in serverTree.ServerConnections.Values)
                {
                    if (c.IsConnected)
                    {
                        if (c.ServerSetting.Away == false)
                        {
                            string msg = iceChatOptions.AutoAwayMessage;
                            msg = msg.Replace("$autoawaytime", iceChatOptions.AutoAwayTime.ToString());
                            ParseOutGoingCommand(c, "/away " + msg);
                        }
                    }
                }

            }

        }

        #endregion

        //http://www.codeproject.com/KB/cs/dynamicpluginmanager.aspx

        private IPluginIceChat LoadPlugin(string fileName)
        {
            string args = fileName.Substring(fileName.LastIndexOf("\\") + 1);
            args = args.Substring(0, args.Length - 4);

            Type ObjType = null;
            try
            {
                Assembly ass = null;
                ass = Assembly.LoadFrom(fileName);                
                
                if (ass != null)
                {
                    ObjType = ass.GetType("IceChatPlugin.Plugin");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("assembly is null::" + ass.GetType("IceChatTheme.Plugin"));                    
                    return null;
                }
                 
            }
            catch (Exception ex)
            {
                WriteErrorFile(inputPanel.CurrentConnection, "LoadPlugin Error ", ex);
                return null;
            }
            try
            {
                // OK Lets create the object as we have the Report Type
                if (ObjType != null)
                {
                    IPluginIceChat ipi = (IPluginIceChat)Activator.CreateInstance(ObjType);

                    ipi.MainForm = this;
                    ipi.MainMenuStrip = menuMainStrip;
                    ipi.CurrentFolder = currentFolder;
                    
                    ipi.CurrentVersion = Convert.ToDouble(BuildNumber.Replace(".", String.Empty));
                    
                    //serverTree.ServerConnections.Values
                    ipi.FileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    ipi.LeftPanel = panelDockLeft.TabControl;
                    ipi.RightPanel = panelDockRight.TabControl;

                    ipi.Domain = null;
                    ipi.Enabled = true; //enable it by default

                    WindowMessage(null, "Console", "\x000304Loaded Plugin - " + ipi.Name + " v" + ipi.Version + " by " + ipi.Author, "", true);

                    //add the menu items
                    ToolStripMenuItem t = new ToolStripMenuItem(ipi.Name)
                    {
                        BackColor = SystemColors.Menu,
                        ForeColor = SystemColors.MenuText,
                        Tag = ipi,
                        ToolTipText = fileName.Substring(fileName.LastIndexOf("\\") + 1)
                    };
                    t.Click += new EventHandler(OnPluginMenuItemClick);

                    pluginsToolStripMenuItem.DropDownItems.Add(t);

                    ipi.OnCommand += new OutGoingCommandHandler(Plugin_OnCommand);

                    //new way to handle plugins
                    IceChatPlugin ip = new IceChatPlugin
                    {
                        plugin = ipi,
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1)
                    };
                    loadedPlugins.Add(ip);

                    return ipi;
                }
                else
                {
                    Assembly ass;
                    ass = Assembly.LoadFrom(fileName);
                    ObjType = ass.GetType("IceChatTheme.Theme");
                    if (ObjType != null)
                    {
                        //this is an icechat theme
                        IThemeIceChat iTheme = (IThemeIceChat)Activator.CreateInstance(ObjType);
                        WindowMessage(null, "Console", "\x000304Loaded Plugin Theme - " + iTheme.Name + " v" + iTheme.Version + " by " + iTheme.Author, "", true);
                        iTheme.Enabled = true;
                        iTheme.Initialize();

                        iTheme.FileName = fileName;
                        loadedPluginThemes.Add(iTheme);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("obj type is null: " + args);

                        ass = null;



                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorFile(inputPanel.CurrentConnection, "LoadPlugin Error", ex);
            }

            return null;


        }

        private void LoadPlugins()
        {
            string[] pluginFiles = Directory.GetFiles(pluginsFolder, "*.dll");
            
            for (int x = 0; x < pluginFiles.Length; x++)
            {
                string fileName = pluginFiles[x].Substring(pluginFiles[x].LastIndexOf("\\") + 1);
                //System.Diagnostics.Debug.WriteLine("check:" + fileName);
                bool pluginFound = false;

                for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
                {
                    //System.Diagnostics.Debug.WriteLine("m:" + ((PluginItem)iceChatPlugins.listPlugins[i]).PluginFile);
                    if (((PluginItem)iceChatPlugins.listPlugins[i]).PluginFile.Equals(fileName))
                    {
                        pluginFound = true;

                        //check if the plugin was unloaded..
                        if (((PluginItem)iceChatPlugins.listPlugins[i]).Unloaded == false)
                        {
                            //System.Diagnostics.Debug.WriteLine("load:" + fileName);                            
                            LoadPlugin(pluginFiles[x]);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("dont load:" + fileName);
                            //create a blank instance of it

                            NullPlugin np = new NullPlugin
                            {
                                Plugin_Enabled = false,
                                Plugin_Unloaded = true,
                                fileName = fileName
                            };

                            loadedPlugins.Add(np);

                        }
                    }
                }
                
                if (pluginFound == false)
                    LoadPlugin(pluginFiles[x]);                
            }
        }

        private void Plugin_OnCommand(PluginArgs e)
        {
            if (e.Command != null)
            {
                if (e.Connection != null)
                    ParseOutGoingCommand(e.Connection, e.Command);
                else
                {
                    if (e.Extra == "current")
                        ParseOutGoingCommand(inputPanel.CurrentConnection, e.Command);
                    else if (e.Extra == "all")
                    {
                        //send to all open connectionsF
                        //ParseOutGoingCommand(inputPanel.CurrentConnection, e.Command);
                    }
                    else
                        ParseOutGoingCommand(null, e.Command);
                }
            }

        }

        internal void UnloadPlugin(ToolStripMenuItem menuItem)
        {
            ParseOutGoingCommand(null, "/unloadplugin " + menuItem.ToolTipText);
        }
                
        internal void StatusPlugin(ToolStripMenuItem menuItem, bool enable)
        {
            for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
            {
                if (((PluginItem)iceChatPlugins.listPlugins[i]).PluginFile.Equals(menuItem.ToolTipText))
                {
                    ((PluginItem)iceChatPlugins.listPlugins[i]).Enabled = enable;
                    SavePluginFiles();
                }
            }

            ParseOutGoingCommand(null, "/statusplugin " + enable.ToString() + " " + menuItem.ToolTipText);
        }
                
        /// <summary>
        /// Write out to the errors file, specific to the Connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="method"></param>
        /// <param name="e"></param>
        internal void WriteErrorFile(IRCConnection connection, string method, Exception e)
        {
            //System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
            //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
            try
            {
                WindowMessage(connection, "Console", "\x000304Error:" + method + ":" + e.Message + ":" + e.StackTrace, "", true);

                if (errorFile != null)
                {
                    try
                    {
                        errorFile.WriteLine(DateTime.Now.ToString("G") + ":" + e.Message + ":" + e.StackTrace);
                        errorFile.WriteLine(DateTime.Now.ToString("G") + ":" + method + "::" + e.Message);
                    }
                    catch (Exception ee) { System.Diagnostics.Debug.WriteLine("No error file:" + ee.Message); }
                    finally
                    {
                        errorFile.Flush();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Write out to the errors file, not Connection Specific
        /// </summary>
        /// <param name="method"></param>
        /// <param name="e"></param>
        internal void WriteErrorFile(string method, FileNotFoundException e)
        {
            try
            {
                //System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
                WindowMessage(inputPanel.CurrentConnection, "Console", "\x000304Error:" + method + ":" + e.Message + ":" + e.StackTrace + ":" + trace.GetFrame(0).GetFileLineNumber(), "", true);

                if (errorFile != null)
                {
                    errorFile.WriteLine(DateTime.Now.ToString("G") + ":" + method + ":" + e.Message + ":" + e.StackTrace + ":" + trace.GetFrame(0).GetFileLineNumber());
                    errorFile.Flush();
                }
            }
            catch (Exception)
            {
            
            }
        }

        private void GetLocalIPAddress()
        {            
            //find your internet IP Address
            System.Net.WebRequest request = System.Net.WebRequest.Create("https://www.icechat.net/_ipaddress.php");
            try
            {
                System.Net.WebResponse response = request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream());
                string data = stream.ReadToEnd();
                stream.Close();
                response.Close();

                //remove any linefeeds and such
                data = data.Replace("\n", "");
                iceChatOptions.DCCLocalIP = data.Trim();

                //save the settings
                SaveOptions();
            }
            catch (Exception)
            {
                //error
            }
        }


        private void CheckForUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
            // this is now disabled
            return;

            //check for newer version
            double currentVersion = Convert.ToDouble(BuildNumber.Replace(".", String.Empty));

            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            int updateCount = 0;

            try
            {

                // different update files for the 4.5 builds
                // USE_NET_45
                    
                string updaterXML = "updater-45.xml";
                string update9XML = "update9-45.xml";


                if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML))
                    File.Delete(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML);

                System.Diagnostics.Debug.WriteLine("https://www.icechat.net/" + updaterXML );
                System.Diagnostics.Debug.WriteLine(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML);


                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile("https://icechat.net/" + updaterXML, currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML);
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();


                xmlDoc.Load(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + updaterXML);
                System.Xml.XmlNodeList version = xmlDoc.GetElementsByTagName("version");
                System.Xml.XmlNodeList versiontext = xmlDoc.GetElementsByTagName("versiontext");

                if (Convert.ToDouble(version[0].InnerText) > currentVersion)
                {
                    this.toolStripUpdate.Visible = true;

                    if (this.toolStripMain.Visible == false)
                    {
                        this.updateAvailableToolStripMenuItem1.Visible = true;
                    }
                    
                    updateCount = 1;
                    CurrentWindowMessage(inputPanel.CurrentConnection, "\x000304There is an IceChat Update available - " + versiontext[0].InnerText + ". Click the update button on the Tool Bar", "", true);
                }
                else
                {
                    this.toolStripUpdate.Visible = false;
                    this.updateAvailableToolStripMenuItem1.Visible = false;
                    CurrentWindowMessage(inputPanel.CurrentConnection, "\x000304You are running the latest version of IceChat (" + BuildNumber + ") -- Version online = " + versiontext[0].InnerText, "", true);
                }

                webClient.DownloadFile("https://www.icechat.net/" + update9XML, currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + update9XML);
                xmlDoc.Load(currentFolder + System.IO.Path.DirectorySeparatorChar + "Update" + Path.DirectorySeparatorChar + update9XML);
                
                //check for any Plugins to be Updated
                if (Directory.Exists(pluginsFolder))
                {
                    string[] plugins = Directory.GetFiles(pluginsFolder, "*.dll");
                    foreach (string fileName in plugins)
                    {
                        
                        //look for a match to plugins online
                        FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName);
                        XmlNodeList plgs = xmlDoc.GetElementsByTagName("plugin");
                        // System.Diagnostics.Debug.WriteLine(fileName + ":" + plgs.Count);

                        foreach (XmlNode plg in plgs)
                        {
                            //System.Diagnostics.Debug.WriteLine(plg["pluginfile"].InnerText);
                            //System.Diagnostics.Debug.WriteLine(plg["pluginversion"].InnerText);
                            if (Path.GetFileName(plg["pluginfile"].InnerText).ToLower() == fvi.InternalName.ToLower())
                            {
                                //check versions
                                if (Convert.ToSingle(fvi.FileVersion.Replace(".", "")) < Convert.ToSingle(plg["pluginversion"].InnerText.Replace(".", "")))
                                {
                                    this.toolStripUpdate.Visible = true;

                                    if (toolStripMain.Visible == false)
                                    {
                                        this.updateAvailableToolStripMenuItem1.Visible = true;
                                    }                                    

                                    CurrentWindowMessage(inputPanel.CurrentConnection, "\x000304There is an Plugin Update available for " + fvi.FileDescription + ". Click the update button on the Tool Bar", "", true);
                                    updateCount++;
                                }
                            }
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                CurrentWindowMessage(inputPanel.CurrentConnection, "\x000304Error checking for IceChat update :" + ex.Message, "", true);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        
        private void TabControl_DoubleClick(object sender, EventArgs e)
        {
            TabControl t = (TabControl)sender;
            if (t.SelectedTab.Controls[0].GetType() == typeof(Panel))
            {
                Panel p = (Panel)t.SelectedTab.Controls[0];
                UnDockPanel(p);
            }
        }
        /// <summary>
        /// Undock the Specified Panel to a Floating Window
        /// </summary>
        /// <param name="p">The panel to remove and add to a Floating Window</param>
        internal void UnDockPanel(Panel p)
        {
            if (p.Parent.GetType() == typeof(TabPage))
            {
                //System.Diagnostics.Debug.WriteLine(panel1.Parent.Name);
                //remove the tab from the tabStrip
                TabControl t = (TabControl)p.Parent.Parent;
                TabPage tp = (TabPage)p.Parent;
                ((TabControl)p.Parent.Parent).TabPages.Remove((TabPage)p.Parent);
                ((TabPage)p.Parent).Controls.Remove(p);

                if (t.TabPages.Count == 0)
                {
                    //hide the splitter bar along with the panel
                    if (t.Parent == panelDockLeft)
                        splitterLeft.Visible = false;
                    else if (t.Parent == panelDockRight)
                        splitterRight.Visible = false;

                    t.Parent.Visible = false;
                }

                FormFloat formFloat = new FormFloat(ref p, this, tp.Text);
                formFloat.Show();
                if (Cursor.Position.X - (formFloat.Width / 2) > 0)
                    formFloat.Left = Cursor.Position.X - (formFloat.Width / 2);
                else
                    formFloat.Left = 0;

                formFloat.Top = Cursor.Position.Y;
            }
        }

        /// <summary>
        /// Re-Dock the Panel checking whether it is closer to the right or left
        /// </summary>
        /// <param name="panel">The panel to re-dock</param>
        /// <param name="formLocation">Current Location of the Floating Form</param>
        /// <param name="tabName">The panels caption</param>
        internal void SetPanel(ref Panel panel, Point formLocation, string tabName)
        {
            if (formLocation.X < (this.Left + 200))
            {
                TabPage p = new TabPage(tabName);
                p.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                this.panelDockLeft.TabControl.TabPages.Add(p);
                this.panelDockLeft.TabControl.Visible = true;
                panelDockLeft.Visible = true;
                splitterLeft.Visible = true;
                this.panelDockLeft.TabControl.SelectedTab = p;
            }
            else
            {
                TabPage p = new TabPage(tabName);
                p.Controls.Add(panel);
                panel.Dock = DockStyle.Fill;
                this.panelDockRight.TabControl.TabPages.Add(p);
                this.panelDockRight.TabControl.Visible = true;
                panelDockRight.Visible = true;
                splitterRight.Visible = true;
                this.panelDockRight.TabControl.SelectedTab = p;
            }
        }

        private void BrowseDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/run " + currentFolder);
        }

        private void BrowseLogsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/run " + logsFolder);
        }

        private void BrowsePluginsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/run " + pluginsFolder);
        }

        private void CloseCurrentWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the current window
            mainChannelBar.CloseCurrentTab();
        }
        
        private void ServerTreeImageMenu_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/bg serverlist");
        }

        private void NickListImageMenu_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/bg nicklist");
        }

        private void NickListImageRemoveMenu_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/bg nicklist none");
        }

        private void ServerTreeImageRemoveMenu_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/bg serverlist none");
        }

        private void MuteAllSoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //mute all sounds
            muteAllSounds = !muteAllSounds;
            muteAllSoundsToolStripMenuItem.Checked = muteAllSounds;
        }

        private void LoadAPluginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //bring up a dialog box to open a new Plugin DLL File
            FileDialog fd = new OpenFileDialog
            {
                DefaultExt = ".dll",
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                AutoUpgradeEnabled = false,
                Filter = "Plugin file (*.dll)|*.dll",
                Title = "Which plugin file do you want to open?",
                InitialDirectory = pluginsFolder
            };

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //currentScript = fd.FileName;
                //need to make sure the plugin is not already loaded
                foreach (ToolStripItem item in pluginsToolStripMenuItem.DropDownItems)
                {
                    if (item.ToolTipText.ToLower() == System.IO.Path.GetFileName(fd.FileName).ToLower())
                    {
                        return;
                    }
                }
                
                //check it to the loadedPlugins collection, if it isnt already
                Plugin pp = null;
                foreach (Plugin p in loadedPlugins)
                {
                    if (p.fileName.ToLower() == System.IO.Path.GetFileName(fd.FileName).ToLower())
                    {
                        pp = p;
                    }
                }

                if (pp != null)
                    loadedPlugins.Remove(pp);

                for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
                {
                    if (iceChatPlugins.listPlugins[i].PluginFile.Equals(System.IO.Path.GetFileName(fd.FileName)))
                    {
                        iceChatPlugins.listPlugins[i].Enabled = true;
                        iceChatPlugins.listPlugins[i].Unloaded = false;
                    }
                }

                IPluginIceChat ipc = LoadPlugin(fd.FileName);
                                
                //initialize it
                if (ipc != null)
                {
                    
                    System.Threading.Thread initPlugin = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InitializePlugin));
                    initPlugin.Start(ipc);
                }

                SavePluginFiles();

            }
        }

        private void MultilineEditboxToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            multilineEditboxToolStripMenuItem.Checked = !multilineEditboxToolStripMenuItem.Checked;
            if (multilineEditboxToolStripMenuItem.Checked == true)
                inputPanel.ShowWideTextPanel = true;
            else
                inputPanel.ShowWideTextPanel = false;

            iceChatOptions.ShowMultilineEditbox = multilineEditboxToolStripMenuItem.Checked;
        }

        private void ThemeChoice_Click(object sender, EventArgs e)
        {
            string theme = ((ToolStripMenuItem)sender).Text;

            if (File.Exists(CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Colors-" + theme + ".xml"))
            {
                try
                {
                    colorsFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Colors-" + theme + ".xml";
                    messagesFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Messages-" + theme + ".xml";

                    XmlSerializer deserializer = new XmlSerializer(typeof(IceChatMessageFormat));
                    TextReader textReader = new StreamReader(messagesFile);
                    iceChatMessages = (IceChatMessageFormat)deserializer.Deserialize(textReader);
                    textReader.Close();
                    textReader.Dispose();

                    XmlSerializer deserializerC = new XmlSerializer(typeof(IceChatColors));
                    TextReader textReaderC = new StreamReader(colorsFile);
                    iceChatColors = (IceChatColors)deserializerC.Deserialize(textReaderC);
                    textReaderC.Close();
                    textReaderC.Dispose();

                    //save the current theme
                    iceChatOptions.CurrentTheme = theme;
                    SaveOptions();

                    //update the colors
                    Fc_SaveColors(iceChatColors, iceChatMessages);

                    //uncheck all other themes, check this one
                    foreach (ToolStripMenuItem t in themesToolStripMenuItem.DropDownItems)
                    {
                        if (t.Text == theme)
                            t.Checked = true;
                        else
                            t.Checked = false;
                    }
                }
                catch (Exception)
                {
                    WindowMessage(inputPanel.CurrentConnection, "Console", "\x00034Error: Theme Files error for " + theme, "", true);
                }
            }
            else
            {
                WindowMessage(inputPanel.CurrentConnection, "Console", "\x00034Error: Theme Files not found for " + theme, "", true);
            }
        }


        private void DefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //load the default theme
            
            colorsFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatColors.xml";
            messagesFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatMessages.xml";
            
            //check if the default files exist, if not, an exception error occurs
            LoadMessageFormat();
            
            LoadColors();
            
            //save the current theme
            iceChatOptions.CurrentTheme = "Default";
            SaveOptions();

            //update the colors
            Fc_SaveColors(iceChatColors, iceChatMessages);

            foreach (ToolStripMenuItem t in themesToolStripMenuItem.DropDownItems)
                t.Checked = false;
            
            defaultToolStripMenuItem.Checked = true;

        }

        private void ChannelListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList formList = new FormList();
            formList.SearchChannelCommand += new FormList.SearchChannelDelegate(FormList_SearchChannelCommand);
            formList.ShowDialog(this);
        }

        private void FormList_SearchChannelCommand(string command)
        {
            ParseOutGoingCommand(inputPanel.CurrentConnection, command);
        }

        private void VS2008ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.menuRenderer = new VS2008Renderer.MenuStripRenderer();
            this.toolStripRender = new VS2008Renderer.ToolStripRenderer();

            this.menuMainStrip.Renderer = menuRenderer;
            this.toolStripMain.Renderer = toolStripRender;

            foreach (ToolStripMenuItem t in menuStylesToolStripMenuItem.DropDownItems)
                t.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;

            iceChatOptions.MenuRenderer = "VS 2008";

        }

        private void Office2007ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.menuRenderer = new EasyRenderer.EasyRender();
            this.toolStripRender = new EasyRenderer.EasyRender();
                        
            this.menuMainStrip.Renderer = menuRenderer;
            this.toolStripMain.Renderer = toolStripRender;

            foreach (ToolStripMenuItem t in menuStylesToolStripMenuItem.DropDownItems)            
                t.Checked = false;            
            ((ToolStripMenuItem)sender).Checked = true;

            iceChatOptions.MenuRenderer = "Office 2007";

        }

        private void DefaultRendererToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripMain.RenderMode = ToolStripRenderMode.System;
            this.menuMainStrip.RenderMode = ToolStripRenderMode.System;

            this.menuRenderer = null;
            this.toolStripRender = null;
            
            this.toolStripMain.BackColor = IrcColor.colors[iceChatColors.ToolbarBackColor];

            foreach (ToolStripMenuItem t in menuStylesToolStripMenuItem.DropDownItems)
                t.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;

            iceChatOptions.MenuRenderer = "Default";

        }

        private string GetBackgroundImage()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AutoUpgradeEnabled = false,

                InitialDirectory = picturesFolder,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Select Background Image",
                Filter = "Images (*.png;*.jpg)|*.png;*.jpg"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //returns the full path
                return dialog.FileName;
            }
            return "";
        }

        private void ResizeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //switch from tabs to windows
            //get the previous windowed mode
            //use Cascade for Default
            DialogResult ask;
            
            if (finishedLoading)
                ask = MessageBox.Show("Are you sure you want to go to Windowed mode?", "Windowed Mode", MessageBoxButtons.YesNo);
            else
                ask = DialogResult.Yes;

            if (ask == DialogResult.No)
                return;
            
            bool haveSize = false;
            this.IsMdiContainer = true;

            int i = 0;

            try
            {
                for (i = mainTabControl.Controls.Count - 1; i >= 0; i--)
                {
                    if (mainTabControl.Controls[i].GetType() == typeof(IceTabPage))
                    {
                        IceTabPage tab = ((IceTabPage)mainTabControl.Controls[i]);
                        tab.DockedForm = true;

                        FormWindow fw = new FormWindow(tab)
                        {
                            Text = tab.TabCaption
                        };
                        if (tab.WindowStyle == IceTabPage.WindowType.Channel || tab.WindowStyle == IceTabPage.WindowType.Query)
                            fw.Text += " {" + tab.Connection.ServerSetting.NetworkName + "}";

                        fw.MdiParent = this;

                        Point location = tab.WindowLocation;

                        fw.Show();

                        if (location != null)
                        {
                            //set new window location
                            fw.Location = location;
                        }

                        if (tab.WindowSize != null && tab.WindowSize.Height != 0)
                        {
                            fw.Size = tab.WindowSize;
                            haveSize = true;
                        }
                    }
                    else if (mainTabControl.Controls[i].GetType() == typeof(IceTabPageDCCFile))
                    {

                        IceTabPageDCCFile tab = ((IceTabPageDCCFile)mainTabControl.Controls[i]);
                        tab.DockedForm = true;

                        FormWindow fw = new FormWindow(tab)
                        {
                            Text = tab.TabCaption,
                            MdiParent = this
                        };

                        Point location = tab.WindowLocation;

                        fw.Show();

                        if (location != null)
                            fw.Location = location;

                        if (tab.WindowSize != null && tab.WindowSize.Height != 0)
                        {
                            fw.Size = tab.WindowSize;
                            haveSize = true;
                        }

                    }
                }


                //dont set this, if we have actual previous values for windowlocation
                if (!haveSize)
                    this.LayoutMdi(mainTabControl.MdiLayout);


                mainTabControl.Visible = false;
                mainTabControl.windowedMode = true;
                iceChatOptions.WindowedMode = true;

                resizeWindowToolStripMenuItem.Visible = false;

                windowsToolStripMenuItem.Visible = true;

                closeWindow.Visible = false;

            }
            catch (Exception ex)
            {
                WriteErrorFile(inputPanel.CurrentConnection, "Resize Window Error:", ex);
            }
        }

        internal void ReDockTabs()
        {
            //back to tabbed interface
            //gets called once per form
            int howfar = 0;
            
            try
            {
                howfar = 1;
                IceTabPage selected = null;
                foreach (FormWindow child in this.MdiChildren)
                {
                    child.DisableActivate();
                }
                howfar = 2;

                foreach (FormWindow child in this.MdiChildren)
                {
                    IceTabPage tab = child.DockedControl;
                    tab.DockedForm = false;

                    mainTabControl.AddTabPage(tab);

                    //if tab index == 0, its the current tab
                    if (tab.TabIndex == 0)
                        selected = tab;

                    child.Close();
                }
                howfar = 3;
                mainTabControl.Visible = true;
                mainTabControl.windowedMode = false;
                iceChatOptions.WindowedMode = false;
                howfar = 4;

                resizeWindowToolStripMenuItem.Visible = true;
                windowsToolStripMenuItem.Visible = false;
                closeWindow.Visible = true;

                this.IsMdiContainer = false;

                howfar = 5;
                //what is the active tab??
                if (selected != null)
                    mainChannelBar.SelectTab(selected);

                howfar = 6;

            }
            catch (Exception ex)
            {

                WriteErrorFile(inputPanel.CurrentConnection, "RedockTabs Error:" + howfar, ex);

                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
            mainTabControl.MdiLayout = MdiLayout.Cascade;
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
            mainTabControl.MdiLayout = MdiLayout.TileHorizontal;
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
            mainTabControl.MdiLayout = MdiLayout.TileVertical;
        }

        private void ShowButtonsNickListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toggle the option to show nicklist buttons
            iceChatOptions.ShowNickButtons = !iceChatOptions.ShowNickButtons;

            nickList.ShowNickButtons = iceChatOptions.ShowNickButtons;
            showButtonsNickListToolStripMenuItem.Checked = iceChatOptions.ShowNickButtons;
        }

        private void ShowButtonsServerTreeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //toggle the option to show server tree buttons
            iceChatOptions.ShowServerButtons = !iceChatOptions.ShowServerButtons;

            serverTree.ShowServerButtons = iceChatOptions.ShowServerButtons;
            showButtonsServerTreeToolStripMenuItem1.Checked = iceChatOptions.ShowServerButtons;
        }

        private void AlwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void ViewChannelBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iceChatOptions.ShowTabBar = viewChannelBarToolStripMenuItem.Checked;
            //mainChannelBar.Visible = iceChatOptions.ShowTabBar;
            mainChannelBar.HideBar = !iceChatOptions.ShowTabBar;
            if (toolStripMain.Visible)
                toolStripMain.SendToBack();

            menuMainStrip.SendToBack();
            mainChannelBar.Invalidate();

        }

        private void SaveTabOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/saveorder");
        }

        private void RestoreTabOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParseOutGoingCommand(null, "/loadorder");
        }

        private void IceChatWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/");
            }
            catch { }
        }
         
        // IRC Indexer Service
        /*
        private void searchForChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //use ircindexer.net
            //http://ircindexer.net/indexerapi.php?search=
            
            InputBoxDialog i = new InputBoxDialog();
            i.FormCaption = "Search Channels";
            i.FormPrompt = "Enter the channel to search for.";

            i.ShowDialog();
            if (i.InputResponse.Length > 0)
            {
                //output to an @search
                searchChannels(i.InputResponse);    
            }

            i.Dispose();                            


        }

        private void searchForNetworksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBoxDialog i = new InputBoxDialog();
            i.FormCaption = "Search IRC Networks";
            i.FormPrompt = "Enter the IRC Network to search for.";

            i.ShowDialog();
            if (i.InputResponse.Length > 0)
            {
                //output to an @search
                searchNetworks(i.InputResponse);
            }

            i.Dispose();
        }

        private void searchNetworks(string network)
        {

            string url = "http://ircindexer.net/indexerapi.php?network=" + System.Uri.EscapeDataString(network);
            System.Net.WebClient webClient = new System.Net.WebClient();
            try
            {
                string json = webClient.DownloadString(url);

                List<IrcNetworkSearch> searchResults = JsonConvert.DeserializeObject<List<IrcNetworkSearch>>(json);
                if (searchResults.Count == 0)
                {
                    //echo no results
                    ParseOutGoingCommand(null, "/aline @search \x0002No search results for:\x0002 " + network);
                    ParseOutGoingCommand(null, "/aline @search \x0002IRC Network search brought to you by:\x0002 www.ircindexer.net");
                }
                else
                {
                    ParseOutGoingCommand(null, "/aline @search \x0002Network search results for:\x0002 " + network);

                    foreach (IrcNetworkSearch s in searchResults)
                    {
                        ParseOutGoingCommand(null, "/aline @search " + s.network + " - irc://" + s.ircserver + " - " + s.description);
                    }

                    ParseOutGoingCommand(null, "/aline @search \x0002IRC Network search brought to you by:\x0002 www.ircindexer.net");

                }
                mainChannelBar.SelectTab(GetWindow(null, "@search", IceTabPage.WindowType.Window));
                serverTree.SelectTab(mainChannelBar.CurrentTab, false);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error parsing:" + ex.Message);
            }
        }

        private void searchChannels(string channel)
        {
            string url = "http://ircindexer.net/indexerapi.php?search=" + System.Uri.EscapeDataString(channel);
            System.Net.WebClient webClient = new System.Net.WebClient();
            try
            {
                string json = webClient.DownloadString(url);

                List<IrcChannelSearch> searchResults = JsonConvert.DeserializeObject<List<IrcChannelSearch>>(json);
                if (searchResults.Count == 0)
                {
                    //echo no results
                    ParseOutGoingCommand(null, "/aline @search \x0002No search results for:\x0002 " + channel);
                    ParseOutGoingCommand(null, "/aline @search \x0002IRC Channel search brought to you by:\x0002 www.ircindexer.net");
                }
                else
                {
                    ParseOutGoingCommand(null, "/aline @search \x0002Search results for:\x0002 " + channel);

                    foreach (IrcChannelSearch s in searchResults)
                    {
                        if (s.network.Length == 0)
                        {
                            ParseOutGoingCommand(null, "/aline @search irc://" + s.ircserver + "/" + s.channelname + " " + s.channeltopic);
                        }
                        //this will need to be changed to allow for checking if we are connected to a network already
                        //connect://network:irc.server.name:6667/#icechat
                        else
                        {
                            ParseOutGoingCommand(null, "/aline @search connect://" + s.network + ":" + s.ircserver + "/" + s.channelname + " " + s.channeltopic);
                        }
                    }

                    ParseOutGoingCommand(null, "/aline @search \x0002IRC Channel search brought to you by:\x0002 www.ircindexer.net");

                }
                mainChannelBar.SelectTab(GetWindow(null, "@search", IceTabPage.WindowType.Window));
                serverTree.SelectTab(mainChannelBar.CurrentTab, false);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error parsing:" + ex.Message);
                //MessageBox.Show(ex.Message);
            }
        }
        */
        // END of IRC Indexer Service

        private void ImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //import settings zip file
            OpenFileDialog dialog = new OpenFileDialog
            {
                AutoUpgradeEnabled = false,
                //dialog.InitialDirectory = ;
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //returns the full path
                //System.Diagnostics.Debug.WriteLine(dialog.FileName);
                //string outPath = currentFolder;
                using (ZipPackage zip = (ZipPackage)Package.Open(dialog.FileName, FileMode.Open))
                {
                    System.Diagnostics.Debug.WriteLine("open zip:" + dialog.FileName);
                    foreach (PackagePart part in zip.GetParts())
                    {
                        string outFileName = Path.Combine(currentFolder, part.Uri.OriginalString.Substring(1));

                        System.Diagnostics.Debug.WriteLine(outFileName);

                        using (System.IO.FileStream outFileStream = new System.IO.FileStream(outFileName, FileMode.Create))
                        {
                            using (Stream inFileStream = part.GetStream())
                            {
                                CopyStream(inFileStream, outFileStream);
                            }
                        }
                    }
                }
            }
            //set a flag NOT to save settings, and close / restart
            MessageBox.Show("New Settings Imported");
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //save all settings to a zip file
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "ZIP Files (*.zip)|*.zip",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                //this is the full filename
                using (ZipPackage zip = (ZipPackage)Package.Open(sfd.FileName, FileMode.Create))
                {
                    //zip up every file in the IceChat Settings folder
                    DirectoryInfo di = new DirectoryInfo(currentFolder);
                    foreach (System.IO.FileInfo fi in di.GetFiles())
                    {
                        if (!fi.FullName.EndsWith(Path.GetFileName(sfd.FileName)))
                        {
                            Uri uri = PackUriHelper.CreatePartUri(new Uri(Path.GetFileName(fi.FullName), UriKind.Relative));
                            PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
                            using (FileStream file = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read))
                            {
                                using (Stream dest = part.GetStream())
                                {
                                    CopyStream(file, dest);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CopyStream(System.IO.Stream inputStream, System.IO.Stream outputStream)
        {
            long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = 0;
            long bytesWritten = 0;
            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                outputStream.Write(buffer, 0, bytesRead);
                bytesWritten += bufferSize;
            }
        }

        private void SetupDDEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetupIRCDDE();
        }

        private void FixWindowSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fixWindowSizeToolStripMenuItem.Checked)
                this.FormBorderStyle = FormBorderStyle.Sizable;
            else
                this.FormBorderStyle = FormBorderStyle.FixedSingle;

            fixWindowSizeToolStripMenuItem.Checked = !fixWindowSizeToolStripMenuItem.Checked;
            
            iceChatOptions.LockWindowSize = fixWindowSizeToolStripMenuItem.Checked;
        }

        private void UpdateAvailableToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripUpdate.PerformClick();            
        }

        private void CommandsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/index.php?title=Commands");
            }
            catch { }
        }

        private void AliasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/index.php?title=Aliases");
            }
            catch { }
        }

        private void IdentifiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/index.php?title=Identifiers");
            }
            catch { }

        }

        private void PortableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/index.php?title=Portable");
            }
            catch { }
        }

        private void BuildFromSourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ParseOutGoingCommand(null, "/browser https://wiki.icechat.net/index.php?title=Build_from_source_code");
            }
            catch { }
        }
    }
    /*
    public class IrcChannelSearch
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "network")]
        public string network { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ircserver")]
        public string ircserver { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "channelname")]
        public string channelname { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "channeltopic")]
        public string channeltopic { get; set; }
    }

    public class IrcNetworkSearch
    {
        [Newtonsoft.Json.JsonProperty(PropertyName = "network")]
        public string network { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ircserver")]
        public string ircserver { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "usercount")]
        public string usercount { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "status")]
        public string status { get; set; }
    }
    */ 
}