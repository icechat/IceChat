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
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormFirstRun : Form
    {
        private string[] Default_Servers = new string[] { "irc.quakenet.org", "irc.libera.chat", "irc.ircnet.com", "irc.undernet.org", "irc.rizon.net", "irc.efnet.org" };
        private string[] Default_Aliases = new string[] { "/op /mode # +o $1", "/deop /mode # -o $1", "/voice /mode # +v $1", "/devoice /mode # -v $1", "/b /ban # $1", "/j /join $1 $2-", "/n /names $1", "/w /whois $1 $1", "/k /kick # $1 $2-", "/q /query $1", "/v /version $1", "/about //say Operating System  [$os Build No. $osbuild]  - Uptime [$uptime]  - $icechat" };

        private string[] Nicklist_Popup = new string[] { "Information", ".Display User Info:/userinfo $nick", ".Whois user:/whois $nick $nick", ".DNS User:/dns $nick", "Commands", ".Query user:/query $nick", "Op Commands", ".Voice user:/mode # +v $nick", ".DeVoice user:/mode # -v $nick", ".Op user:/mode # +o $nick", ".Deop user:/mode # -o $nick", ".Kick:/kick # $nick", ".Ban:/ban # $mask($host,2)", "CTCP", ".Ping:/ping $nick", ".Version:/version $nick", "DCC", ".Send:/dcc send $nick", ".Chat:/dcc chat $nick", "Slaps", ".Brick:/me slaps $nick with a big red brick", ".Trout:/me slaps $nick with a &#x3;4r&#x3;8a&#x3;9i&#x3;11n&#x3;13b&#x3;17o&#x3;26w trout" };
        private string[] Channel_Popup = new string[] { "Information", ".Channel Info:/chaninfo" };
        private string[] Console_Popup = new string[] { "Server Commands", ".Server Links Here:/links", ".Message of the Day:/motd", "AutoPerform:/autoperform", "Autojoin:/autojoin" };
        private string[] Query_Popup = new string[] { "Info:/userinfo $1", "Whois:/whois $nick", "-", "Ignore:/ignore $1", "-", ".Ping:/ctcp $1 ping", ".Time:/ctcp $1 time", ".Version:/ctcp $1 version", "DCC", ".Send:/dcc send $1", ".Chat:/dcc chat $1" };
        private string[] Buddy_Popup = new string[] { "Query:/query $1", "Whois:/whois $1" };

        private int _currentStep;

        private string _nickName;
        private string _currentFolder;

        private IceChatOptions icechatOptions;
        private IceChatFontSetting icechatFonts;
        private IceChatColors icechatColors;
        private IceChatMessageFormat icechatMessages;

        internal delegate void SaveOptionsDelegate(IceChatOptions options, IceChatFontSetting fonts);
        internal event SaveOptionsDelegate SaveOptions;

        public FormFirstRun(string currentFolder)
        {
            InitializeComponent();
            this.icechatOptions = new IceChatOptions();
            this.icechatFonts = new IceChatFontSetting();
            this.icechatColors = new IceChatColors();
            this.icechatMessages = new IceChatMessageFormat();

            this.buttonNext.Image = StaticMethods.LoadResourceImage("next.svg.png");
            this.buttonBack.Image = StaticMethods.LoadResourceImage("previous.svg.png");

            _nickName = "Default";
            _currentFolder = currentFolder;

            foreach (string s in Default_Servers)
            {
                comboData.Items.Add(s);
            }

            comboData.Text = "irc.quakenet.org";

            CurrentStep = 0;
        }

        private void FormFirstRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _currentStep != 3)
                e.Cancel = true;
        }
        
        private int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                _currentStep = value;
                
                switch (value)
                {
                    case 0:
                        textData.Text = _nickName;
                        labelHeader.Text = "Nick Name";
                        labelDesc.Text = "Enter a nickname in the field below." + Environment.NewLine + "A nickname (or handle) is what you will be known as on IRC. It's similar to your real life name except on IRC, your nickname will be unique. You will be the only person on the network with your chosen nickname. Usually nicknames are limited to 9 characters in length.";
                        labelTip.Text = "Tip: You can change your nickname while connected by typing'/nick NewNick'";
                        labelField.Text = "Nickname:";
                        
                        comboData.Visible = false;
                        textData.Visible = true;
                        buttonBack.Visible = false;
                        buttonNext.Visible = true;
                        buttonImport.Visible = true;

                        break;

                    case 1:
                        _nickName = textData.Text;
                        labelHeader.Text = "Server Name";
                        labelDesc.Text = "Enter a server in the field below." + Environment.NewLine + "IRC is made up of several things called 'servers'. You can think of a server like a building which contains people and rooms (channels). There are hundreds of servers which you can connect to on IRC. Some servers make up single networks. These could be thought of as several buildings all joined together to make one big one.";
                        labelTip.Text = "Tip: You can use the server editor located at the bottom right or type '/server Address' to connect";
                        labelField.Text = "Server Address:";

                        textData.Visible = false;
                        comboData.Visible = true;
                        buttonBack.Visible = true;
                        buttonNext.Visible = true;
                        buttonImport.Visible = false;

                        break;

                    case 2:
                        labelHeader.Text = "Done";
                        labelDesc.Text = "Your information has been saved. Simply select a server from the Favorite Server List, and click the 'Connect' button.";
                        labelTip.Text = "Tip: Use the '?' in the bottom left corner beside the Input Box for all your basic needs";
                        labelDesc.Text += Environment.NewLine + Environment.NewLine + "Default Nick Name: " + _nickName ;
                        if (comboData.Text.Length > 0)
                            labelDesc.Text += Environment.NewLine + "Default Server: " + comboData.Text;

                        labelField.Text = "";
                        textData.Visible = false;
                        comboData.Visible = false;
                        buttonBack.Visible = true;
                        buttonNext.Visible = true;
                        buttonImport.Visible = false;

                        break;

                    case 3:
                        //save the information
                        MakeDefaultFiles();

                        this.Close();
                        break;
                }
                
            }
            
        }
        
        private void MakeDefaultFiles()
        {
            //make the server file
            string serversFile = _currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatServer.xml";

            IceChatServers servers = new IceChatServers();

            int ID = 1;

            _nickName = _nickName.Replace(" ", "");
            _nickName = _nickName.Replace("#", "");
            _nickName = _nickName.Replace(",", "");
            _nickName = _nickName.Replace("`", "");

            FormMain.Instance.IceChatOptions.DefaultNick = _nickName;

            //check for other theme files
            int totalThemes = 1;
            
            DirectoryInfo currentFolder = new DirectoryInfo(_currentFolder);
            FileInfo[] xmlFiles = currentFolder.GetFiles("*.xml"); 
            foreach (FileInfo fi in xmlFiles)
            {
                if (fi.Name.StartsWith("Colors-"))
                {
                    totalThemes++;
                }
            }


            FormMain.Instance.IceChatOptions.Theme = new ThemeItem[totalThemes];

            FormMain.Instance.IceChatOptions.Theme[0] = new ThemeItem
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
                    FormMain.Instance.IceChatOptions.Theme[t] = new ThemeItem
                    {
                        ThemeName = themeName,
                        ThemeType = "XML"
                    };
                    t++;
                }
            }

            FormMain.Instance.IceChatOptions.CurrentTheme = "Default";

            if (comboData.Text.Length > 0)
            {
                ServerSetting s = new ServerSetting
                {
                    ID = ID,
                    ServerName = comboData.Text,
                    NickName = _nickName,
                    AltNickName = _nickName + "_",
                    ServerPort = "6667",
                    FullName = FormMain.Instance.IceChatOptions.DefaultFullName,
                    IdentName = FormMain.Instance.IceChatOptions.DefaultIdent,
                    QuitMessage = FormMain.Instance.IceChatOptions.DefaultQuitMessage,
                    SetModeI = true
                };

                if (comboData.Text.ToLower() == "irc.quakenet.org")
                {
                    s.AutoJoinChannels = new string[] { "#icechat" };
                    s.AutoJoinEnable = true;
                }

                ID++;

                servers.AddServer(s);
            }
            
            foreach (string server in comboData.Items)
            {
                if (server != comboData.Text && server.Length > 0)
                {
                    ServerSetting ss = new ServerSetting
                    {
                        ID = ID,
                        ServerName = server,
                        NickName = _nickName,
                        AltNickName = _nickName + "_",
                        ServerPort = "6667",
                        SetModeI = true,
                        FullName = FormMain.Instance.IceChatOptions.DefaultFullName,
                        IdentName = FormMain.Instance.IceChatOptions.DefaultIdent,
                        QuitMessage = FormMain.Instance.IceChatOptions.DefaultQuitMessage
                    };

                    if (server.ToLower() == "irc.quakenet.org")
                    {
                        ss.AutoJoinChannels = new string[] { "#icechat" };
                        ss.AutoJoinEnable = true;
                    }

                    ID++;

                    servers.AddServer(ss);
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(IceChatServers));
            TextWriter textWriter = new StreamWriter(FormMain.Instance.ServersFile);
            serializer.Serialize(textWriter, servers);
            textWriter.Close();
            textWriter.Dispose();


            //make the default aliases file
            string aliasesFile = _currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatAliases.xml";
            IceChatAliases aliasList = new IceChatAliases();
            
            foreach (string alias in Default_Aliases)
            {
                AliasItem a = new AliasItem();
                string name = alias.Substring(0,alias.IndexOf(" ")).Trim();
                string command = alias.Substring(alias.IndexOf(" ") + 1);
                a.AliasName = name;
                a.Command = new String[] { command };

                aliasList.AddAlias(a);
            }

            XmlSerializer serializerA = new XmlSerializer(typeof(IceChatAliases));
            TextWriter textWriterA = new StreamWriter(aliasesFile);
            serializerA.Serialize(textWriterA, aliasList);
            textWriterA.Close();
            textWriterA.Dispose();

            
            //make the default popups file
            string popupsFile = _currentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatPopups.xml";
            IceChatPopupMenus popupList = new IceChatPopupMenus();

            popupList.AddPopup(NewPopupMenu("Console", Console_Popup));
            popupList.AddPopup(NewPopupMenu("Channel", Channel_Popup));
            popupList.AddPopup(NewPopupMenu("Query", Query_Popup));
            popupList.AddPopup(NewPopupMenu("NickList", Nicklist_Popup));

            XmlSerializer serializerP = new XmlSerializer(typeof(IceChatPopupMenus));
            TextWriter textWriterP = new StreamWriter(popupsFile);
            serializerP.Serialize(textWriterP, popupList);
            textWriterP.Close();
            textWriterP.Dispose();

        }

        private PopupMenuItem NewPopupMenu(string type, string[] menu)
        {
            PopupMenuItem p = new PopupMenuItem
            {
                PopupType = type,
                Menu = menu
            };
            return p;
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            //check if a nickname has been set
            if (_currentStep == 0)
            {
                if (textData.Text == "Default")
                {
                    MessageBox.Show("Please Choose a Default Nick Name");
                    return;
                }
            }
            CurrentStep++;
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            CurrentStep--;
        }

        private void ButtonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = ".ini",
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                AutoUpgradeEnabled = true,
                Filter = "IceChat Setting (*.ini)|icechat.ini",
                Title = "Locate the IceChat.ini settings file?"
            };

            string directory = Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + "IceChat";

            ofd.InitialDirectory = directory;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //make sure it is icechat.ini
                //System.Diagnostics.Debug.WriteLine(ofd.FileName + ":" + Path.GetFileName(ofd.FileName));

                if (Path.GetFileName(ofd.FileName).ToLower().Equals("icechat.ini"))
                {
                    //we have a match
                    IniParser parser = new IniParser(ofd.FileName);

                    //System.Diagnostics.Debug.WriteLine(parser.GetSetting("Main Settings","Color",""));

                    //1,6,21,5,3,7,26,3,3,15,15,5,4,15,21,2,28,4,10,22,3,2,10,3,1,1,4,12,12,4,12,10,13,7,1,10,29,12,4,4,4,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,51,0,0,66,4,40,51,0,0,53,0,4,51,0,51,0,53,0,52,0,51,0,0,51,51,0,0,1,4,2,1,1,1,1,51
                    string[] mainColors = parser.GetSetting("Main Settings", "Color", "").Split(',');

                    icechatColors.ChannelAdminColor = Convert.ToInt32(mainColors[38]);
                    icechatColors.ChannelBackColor = Convert.ToInt32(mainColors[43]);
                    //icechatColors.ChannelJoinColorChange = Convert.ToInt32(mainColors[30]);
                    icechatColors.ChannelListBackColor = Convert.ToInt32(mainColors[63]);
                    icechatColors.ChannelListForeColor = Convert.ToInt32(mainColors[86]);
                    icechatColors.ChannelOwnerColor = Convert.ToInt32(mainColors[39]);
                    //icechatColors.ChannelPartColorChange
                    icechatColors.ChannelRegularColor = Convert.ToInt32(mainColors[34]);
                    icechatColors.ChannelVoiceColor = Convert.ToInt32(mainColors[35]);
                    icechatColors.ChannelHalfOpColor = Convert.ToInt32(mainColors[36]);
                    icechatColors.ChannelOpColor = Convert.ToInt32(mainColors[37]);
                    icechatColors.ConsoleBackColor = Convert.ToInt32(mainColors[42]);

                    icechatColors.InputboxBackColor = Convert.ToInt32(mainColors[52]);
                    icechatColors.InputboxForeColor = Convert.ToInt32(mainColors[25]);
                    icechatColors.NickListBackColor = Convert.ToInt32(mainColors[53]);
                    icechatColors.PanelHeaderBG1 = Convert.ToInt32(mainColors[70]);
                    icechatColors.PanelHeaderBG2 = Convert.ToInt32(mainColors[71]);
                    icechatColors.PanelHeaderForeColor = Convert.ToInt32(mainColors[88]);
                    icechatColors.QueryBackColor = Convert.ToInt32(mainColors[44]);
                    icechatColors.RandomizeNickColors = false;
                    icechatColors.ServerListBackColor = Convert.ToInt32(mainColors[54]);
                    icechatColors.StatusbarBackColor = Convert.ToInt32(mainColors[90]);
                    icechatColors.StatusbarForeColor = Convert.ToInt32(mainColors[89]);
                    icechatColors.TabBarChannelJoin = Convert.ToInt32(mainColors[30]);
                    icechatColors.TabBarChannelPart = Convert.ToInt32(mainColors[31]);
                    icechatColors.TabBarCurrent = Convert.ToInt32(mainColors[28]);
                    icechatColors.TabBarDefault = Convert.ToInt32(mainColors[41]);
                    icechatColors.TabBarNewMessage = Convert.ToInt32(mainColors[29]);
                    icechatColors.TabBarOtherMessage = Convert.ToInt32(mainColors[32]);
                    icechatColors.TabBarServerMessage = Convert.ToInt32(mainColors[32]);
                    icechatColors.TabBarServerQuit = Convert.ToInt32(mainColors[32]);
                    icechatColors.ToolbarBackColor = Convert.ToInt32(mainColors[68]);
                    icechatColors.UnreadTextMarkerColor = Convert.ToInt32(mainColors[67]);

                    XmlSerializer serializer = new XmlSerializer(typeof(IceChatColors));
                    TextWriter textWriter = new StreamWriter(FormMain.Instance.ColorsFile);
                    serializer.Serialize(textWriter, icechatColors);
                    textWriter.Close();
                    textWriter.Dispose();


                    icechatMessages.MessageSettings = new ServerMessageFormatItem[49];

                    icechatMessages.MessageSettings[0] = NewMessageFormat("Server Connect", "&#x3;" + mainColors[18] + "*** Attempting to connect to $server ($serverip) on port $port");
                    icechatMessages.MessageSettings[1] = NewMessageFormat("Server Disconnect", "&#x3;" + mainColors[19] + "*** Server disconnected on $server");
                    icechatMessages.MessageSettings[2] = NewMessageFormat("Server Reconnect", "&#x3;" + mainColors[11] + "*** Attempting to re-connect to $server");
                    icechatMessages.MessageSettings[3] = NewMessageFormat("Channel Invite", "&#x3;" + mainColors[22] + "* $nick invites you to $channel");
                    icechatMessages.MessageSettings[4] = NewMessageFormat("Ctcp Reply", "&#x3;" + mainColors[27] + "[$nick $ctcp Reply] : $reply");
                    icechatMessages.MessageSettings[5] = NewMessageFormat("Ctcp Send", "&#x3;" + mainColors[26] + "--> [$nick] $ctcp");
                    icechatMessages.MessageSettings[6] = NewMessageFormat("Ctcp Request", "&#x3;" + mainColors[11] + "[$nick] $ctcp");
                    icechatMessages.MessageSettings[7] = NewMessageFormat("Channel Mode", "&#x3;" + mainColors[7] + "* $nick sets mode $mode $modeparam for $channel");
                    icechatMessages.MessageSettings[8] = NewMessageFormat("Server Mode", "&#x3;" + mainColors[16] + "* Your mode is now $mode");
                    icechatMessages.MessageSettings[9] = NewMessageFormat("Server Notice", "&#x3;" + mainColors[24] + "*** $server $message");
                    icechatMessages.MessageSettings[10] = NewMessageFormat("Server Message", "&#x3;" + mainColors[11] + "-$server- $message");
                    icechatMessages.MessageSettings[11] = NewMessageFormat("User Notice", "&#x3;" + mainColors[3] + "--$nick-- $message");
                    icechatMessages.MessageSettings[12] = NewMessageFormat("Channel Message", "&#x3;" + mainColors[0] + "<$color$status$nick&#x3;> $message");
                    icechatMessages.MessageSettings[13] = NewMessageFormat("Self Channel Message", "&#x3;" + mainColors[2] + "<$nick&#x3;> $message");
                    icechatMessages.MessageSettings[14] = NewMessageFormat("Channel Action", "&#x3;" + mainColors[1] + "* $nick $message");
                    icechatMessages.MessageSettings[15] = NewMessageFormat("Self Channel Action", "&#x3;" + mainColors[2] + "* $nick $message");
                    icechatMessages.MessageSettings[16] = NewMessageFormat("Channel Join", "&#x3;" + mainColors[5] + "* $nick ($host) has joined channel $channel");
                    icechatMessages.MessageSettings[17] = NewMessageFormat("Self Channel Join", "&#x3;" + mainColors[5] + "* You have joined $channel");
                    icechatMessages.MessageSettings[18] = NewMessageFormat("Channel Part", "&#x3;" + mainColors[4] + "* $nick ($host) has left $channel $reason");
                    icechatMessages.MessageSettings[19] = NewMessageFormat("Self Channel Part", "&#x3;" + mainColors[4] + "* You have left $channel - You will be missed &#x3;10 $reason");
                    icechatMessages.MessageSettings[20] = NewMessageFormat("Server Quit", "&#x3;" + mainColors[21] + "* $nick ($host) Quit ($reason)");
                    icechatMessages.MessageSettings[21] = NewMessageFormat("Channel Nick Change", "&#x3;" + mainColors[20] + "* $nick is now known as $newnick");
                    icechatMessages.MessageSettings[22] = NewMessageFormat("Self Nick Change", "&#x3;" + mainColors[20] + "* You are now known as $newnick");
                    icechatMessages.MessageSettings[23] = NewMessageFormat("Channel Kick", "&#x3;" + mainColors[6] + "* $kickee was kicked by $nick($host) &#x3;3 - Reason ($reason)");
                    icechatMessages.MessageSettings[24] = NewMessageFormat("Self Channel Kick", "&#x3;" + mainColors[6] + "* You were kicked from $channel by $kicker (&#x3;3$reason)");
                    icechatMessages.MessageSettings[25] = NewMessageFormat("Private Message", "&#x3;" + mainColors[0] + "<$nick> $message");
                    icechatMessages.MessageSettings[26] = NewMessageFormat("Self Private Message", "&#x3;" + mainColors[2] + "<$nick>&#x3;1 $message");
                    icechatMessages.MessageSettings[27] = NewMessageFormat("Private Action", "&#x3;" + mainColors[1] + "* $nick $message");
                    icechatMessages.MessageSettings[28] = NewMessageFormat("Self Private Action", "&#x3;" + mainColors[1] + "* $nick $message");
                    icechatMessages.MessageSettings[29] = NewMessageFormat("DCC Chat Action", "&#x3;" + mainColors[1] + "* $nick $message");
                    icechatMessages.MessageSettings[30] = NewMessageFormat("Self DCC Chat Action", "&#x3;" + mainColors[1] + "* $nick $message");
                    icechatMessages.MessageSettings[31] = NewMessageFormat("DCC Chat Message", "&#x3;" + mainColors[0] + "<$nick> $message");
                    icechatMessages.MessageSettings[32] = NewMessageFormat("Self DCC Chat Message", "&#x3;" + mainColors[2] + "<$nick> $message");
                    icechatMessages.MessageSettings[33] = NewMessageFormat("DCC Chat Request", "&#x3;" + mainColors[11] + "* $nick ($host) is requesting a DCC Chat");
                    icechatMessages.MessageSettings[34] = NewMessageFormat("DCC File Send", "&#x3;" + mainColors[11] + "* $nick ($host) is trying to send you a file ($file) [$filesize bytes]");
                    icechatMessages.MessageSettings[35] = NewMessageFormat("Channel Topic Change", "&#x3;" + mainColors[8] + "* $nick changes topic to: $topic");
                    icechatMessages.MessageSettings[36] = NewMessageFormat("Channel Topic Text", "&#x3;" + mainColors[8] + " Topic: $topic");
                    icechatMessages.MessageSettings[37] = NewMessageFormat("Server MOTD", "&#x3;" + mainColors[10] + "$message");
                    icechatMessages.MessageSettings[38] = NewMessageFormat("Channel Notice", "&#x3;" + mainColors[3] + "-$nick:$status$channel- $message");
                    icechatMessages.MessageSettings[39] = NewMessageFormat("Channel Other", "&#x3;" + mainColors[9] + "$message");
                    icechatMessages.MessageSettings[40] = NewMessageFormat("User Echo", "&#x3;" + mainColors[15] + "$message");
                    icechatMessages.MessageSettings[41] = NewMessageFormat("Server Error", "&#x3;" + mainColors[17] + "ERROR: $message");
                    icechatMessages.MessageSettings[42] = NewMessageFormat("User Whois", "&#x3;" + mainColors[14] + "->> $nick $data");
                    icechatMessages.MessageSettings[43] = NewMessageFormat("User Error", "&#x3;" + mainColors[12] + "ERROR: $message");
                    icechatMessages.MessageSettings[44] = NewMessageFormat("DCC Chat Connect", "&#x3;" + mainColors[18] + "* DCC Chat Connection Established with $nick");
                    icechatMessages.MessageSettings[45] = NewMessageFormat("DCC Chat Disconnect", "&#x3;" + mainColors[19] + "* DCC Chat Disconnected from $nick");
                    icechatMessages.MessageSettings[46] = NewMessageFormat("DCC Chat Outgoing", "&#x3;" + mainColors[11] + "* DCC Chat Requested with $nick");
                    icechatMessages.MessageSettings[47] = NewMessageFormat("DCC Chat Timeout", "&#x3;" + mainColors[11] + "* DCC Chat with $nick timed out");
                    icechatMessages.MessageSettings[48] = NewMessageFormat("Self Notice", "&#x3;" + mainColors[24] + "--> $nick - $message");


                    XmlSerializer serializerM = new XmlSerializer(typeof(IceChatMessageFormat));
                    TextWriter textWriterM = new StreamWriter(FormMain.Instance.MessagesFile);
                    serializerM.Serialize(textWriterM, icechatMessages);
                    textWriterM.Close();
                    textWriterM.Dispose();

                    string[] themes = parser.EnumSectionTheme("Color Themes");
                    foreach (string theme in themes)
                    {
                        //System.Diagnostics.Debug.WriteLine("theme:" + theme);
                        string name = theme.Substring(0, theme.IndexOf((char)255));
                        string value = theme.Substring(theme.IndexOf((char)255) + 1);
                        string[] colors = value.Split(',');

                        if (name == "Default")
                            name = "DefaultIce7";

                        IceChatColors ic = new IceChatColors
                        {
                            ChannelAdminColor = Convert.ToInt32(colors[38]),
                            ChannelBackColor = Convert.ToInt32(colors[43]),
                            ChannelListBackColor = Convert.ToInt32(colors[63]),
                            ChannelListForeColor = Convert.ToInt32(colors[86]),
                            ChannelOwnerColor = Convert.ToInt32(colors[39]),
                            ChannelRegularColor = Convert.ToInt32(colors[34]),
                            ChannelVoiceColor = Convert.ToInt32(colors[35]),
                            ChannelHalfOpColor = Convert.ToInt32(colors[36]),
                            ChannelOpColor = Convert.ToInt32(colors[37]),
                            ConsoleBackColor = Convert.ToInt32(colors[42]),
                            InputboxBackColor = Convert.ToInt32(colors[52]),
                            InputboxForeColor = Convert.ToInt32(colors[25]),
                            NickListBackColor = Convert.ToInt32(colors[53]),
                            PanelHeaderBG1 = Convert.ToInt32(colors[70]),
                            PanelHeaderBG2 = Convert.ToInt32(colors[71]),
                            PanelHeaderForeColor = Convert.ToInt32(colors[88]),
                            QueryBackColor = Convert.ToInt32(colors[44]),
                            RandomizeNickColors = false,
                            ServerListBackColor = Convert.ToInt32(colors[54]),
                            StatusbarBackColor = Convert.ToInt32(colors[90]),
                            StatusbarForeColor = Convert.ToInt32(colors[89]),
                            TabBarChannelJoin = Convert.ToInt32(colors[30]),
                            TabBarChannelPart = Convert.ToInt32(colors[31]),
                            TabBarCurrent = Convert.ToInt32(colors[28]),
                            TabBarDefault = Convert.ToInt32(colors[41]),
                            TabBarNewMessage = Convert.ToInt32(colors[29]),
                            TabBarOtherMessage = Convert.ToInt32(colors[32]),
                            TabBarServerMessage = Convert.ToInt32(colors[32]),
                            TabBarServerQuit = Convert.ToInt32(colors[32]),
                            ToolbarBackColor = Convert.ToInt32(colors[68]),
                            UnreadTextMarkerColor = Convert.ToInt32(colors[67])
                        };

                        XmlSerializer serializerC = new XmlSerializer(typeof(IceChatColors));
                        string themeFile = FormMain.Instance.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Colors-" + name + ".xml";
                        TextWriter textWriterC = new StreamWriter(themeFile);
                        serializerC.Serialize(textWriterC, ic);
                        textWriterC.Close();
                        textWriterC.Dispose();


                        IceChatMessageFormat im = new IceChatMessageFormat
                        {
                            MessageSettings = new ServerMessageFormatItem[49]
                        };

                        im.MessageSettings[0] = NewMessageFormat("Server Connect", "&#x3;" + colors[18] + "*** Attempting to connect to $server ($serverip) on port $port");
                        im.MessageSettings[1] = NewMessageFormat("Server Disconnect", "&#x3;" + colors[19] + "*** Server disconnected on $server");
                        im.MessageSettings[2] = NewMessageFormat("Server Reconnect", "&#x3;" + colors[11] + "*** Attempting to re-connect to $server");
                        im.MessageSettings[3] = NewMessageFormat("Channel Invite", "&#x3;" + colors[22] + "* $nick invites you to $channel");
                        im.MessageSettings[4] = NewMessageFormat("Ctcp Reply", "&#x3;" + colors[27] + "[$nick $ctcp Reply] : $reply");
                        im.MessageSettings[5] = NewMessageFormat("Ctcp Send", "&#x3;" + colors[26] + "--> [$nick] $ctcp");
                        im.MessageSettings[6] = NewMessageFormat("Ctcp Request", "&#x3;" + colors[11] + "[$nick] $ctcp");
                        im.MessageSettings[7] = NewMessageFormat("Channel Mode", "&#x3;" + colors[7] + "* $nick sets mode $mode $modeparam for $channel");
                        im.MessageSettings[8] = NewMessageFormat("Server Mode", "&#x3;" + colors[16] + "* Your mode is now $mode");
                        im.MessageSettings[9] = NewMessageFormat("Server Notice", "&#x3;" + colors[24] + "*** $server $message");
                        im.MessageSettings[10] = NewMessageFormat("Server Message", "&#x3;" + colors[11] + "-$server- $message");
                        im.MessageSettings[11] = NewMessageFormat("User Notice", "&#x3;" + colors[3] + "--$nick-- $message");
                        im.MessageSettings[12] = NewMessageFormat("Channel Message", "&#x3;" + colors[0] + "<$color$status$nick&#x3;> $message");
                        im.MessageSettings[13] = NewMessageFormat("Self Channel Message", "&#x3;" + colors[2] + "<$nick&#x3;> $message");
                        im.MessageSettings[14] = NewMessageFormat("Channel Action", "&#x3;" + colors[1] + "* $nick $message");
                        im.MessageSettings[15] = NewMessageFormat("Self Channel Action", "&#x3;" + colors[2] + "* $nick $message");
                        im.MessageSettings[16] = NewMessageFormat("Channel Join", "&#x3;" + colors[5] + "* $nick ($host) has joined channel $channel");
                        im.MessageSettings[17] = NewMessageFormat("Self Channel Join", "&#x3;" + colors[5] + "* You have joined $channel");
                        im.MessageSettings[18] = NewMessageFormat("Channel Part", "&#x3;" + colors[4] + "* $nick ($host) has left $channel $reason");
                        im.MessageSettings[19] = NewMessageFormat("Self Channel Part", "&#x3;" + colors[4] + "* You have left $channel - You will be missed &#x3;10 $reason");
                        im.MessageSettings[20] = NewMessageFormat("Server Quit", "&#x3;" + colors[21] + "* $nick ($host) Quit ($reason)");
                        im.MessageSettings[21] = NewMessageFormat("Channel Nick Change", "&#x3;" + colors[20] + "* $nick is now known as $newnick");
                        im.MessageSettings[22] = NewMessageFormat("Self Nick Change", "&#x3;" + colors[20] + "* You are now known as $newnick");
                        im.MessageSettings[23] = NewMessageFormat("Channel Kick", "&#x3;" + colors[6] + "* $kickee was kicked by $nick($host) &#x3;3 - Reason ($reason)");
                        im.MessageSettings[24] = NewMessageFormat("Self Channel Kick", "&#x3;" + colors[6] + "* You were kicked from $channel by $kicker (&#x3;3$reason)");
                        im.MessageSettings[25] = NewMessageFormat("Private Message", "&#x3;" + colors[0] + "<$nick> $message");
                        im.MessageSettings[26] = NewMessageFormat("Self Private Message", "&#x3;" + colors[2] + "<$nick>&#x3;1 $message");
                        im.MessageSettings[27] = NewMessageFormat("Private Action", "&#x3;" + colors[1] + "* $nick $message");
                        im.MessageSettings[28] = NewMessageFormat("Self Private Action", "&#x3;" + colors[1] + "* $nick $message");
                        im.MessageSettings[29] = NewMessageFormat("DCC Chat Action", "&#x3;" + colors[1] + "* $nick $message");
                        im.MessageSettings[30] = NewMessageFormat("Self DCC Chat Action", "&#x3;" + colors[1] + "* $nick $message");
                        im.MessageSettings[31] = NewMessageFormat("DCC Chat Message", "&#x3;" + colors[0] + "<$nick> $message");
                        im.MessageSettings[32] = NewMessageFormat("Self DCC Chat Message", "&#x3;" + colors[2] + "<$nick> $message");
                        im.MessageSettings[33] = NewMessageFormat("DCC Chat Request", "&#x3;" + colors[11] + "* $nick ($host) is requesting a DCC Chat");
                        im.MessageSettings[34] = NewMessageFormat("DCC File Send", "&#x3;" + colors[11] + "* $nick ($host) is trying to send you a file ($file) [$filesize bytes]");
                        im.MessageSettings[35] = NewMessageFormat("Channel Topic Change", "&#x3;" + colors[8] + "* $nick changes topic to: $topic");
                        im.MessageSettings[36] = NewMessageFormat("Channel Topic Text", "&#x3;" + colors[8] + " Topic: $topic");
                        im.MessageSettings[37] = NewMessageFormat("Server MOTD", "&#x3;" + colors[10] + "$message");
                        im.MessageSettings[38] = NewMessageFormat("Channel Notice", "&#x3;" + colors[3] + "-$nick:$status$channel- $message");
                        im.MessageSettings[39] = NewMessageFormat("Channel Other", "&#x3;" + colors[9] + "$message");
                        im.MessageSettings[40] = NewMessageFormat("User Echo", "&#x3;" + colors[15] + "$message");
                        im.MessageSettings[41] = NewMessageFormat("Server Error", "&#x3;" + colors[17] + "ERROR: $message");
                        im.MessageSettings[42] = NewMessageFormat("User Whois", "&#x3;" + colors[14] + "->> $nick $data");
                        im.MessageSettings[43] = NewMessageFormat("User Error", "&#x3;" + colors[12] + "ERROR: $message");
                        im.MessageSettings[44] = NewMessageFormat("DCC Chat Connect", "&#x3;" + colors[18] + "* DCC Chat Connection Established with $nick");
                        im.MessageSettings[45] = NewMessageFormat("DCC Chat Disconnect", "&#x3;" + colors[19] + "* DCC Chat Disconnected from $nick");
                        im.MessageSettings[46] = NewMessageFormat("DCC Chat Outgoing", "&#x3;" + colors[11] + "* DCC Chat Requested with $nick");
                        im.MessageSettings[47] = NewMessageFormat("DCC Chat Timeout", "&#x3;" + colors[11] + "* DCC Chat with $nick timed out");
                        im.MessageSettings[48] = NewMessageFormat("Self Notice", "&#x3;" + colors[24] + "--> $nick - $message");


                        XmlSerializer serializerIM = new XmlSerializer(typeof(IceChatMessageFormat));
                        string messFile = FormMain.Instance.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Messages-" + name + ".xml";
                        TextWriter textWriterIM = new StreamWriter(messFile);
                        serializerIM.Serialize(textWriterIM, im);
                        textWriterIM.Close();
                        textWriterIM.Dispose();

                    }

                    icechatOptions.Theme = new ThemeItem[themes.Length + 1];

                    icechatOptions.Theme[0] = new ThemeItem
                    {
                        ThemeName = "Default",
                        ThemeType = "XML"
                    };

                    int t = 1;
                    foreach (string theme in themes)
                    {
                        string name = theme.Substring(0, theme.IndexOf((char)255));
                        if (name == "Default")
                            name = "DefaultIce7";

                        icechatOptions.Theme[t] = new ThemeItem
                        {
                            ThemeName = name,
                            ThemeType = "XML"
                        };

                        t++;
                    }

                    icechatOptions.DefaultNick = parser.GetSetting("Default Server Settings", "NickName", "");
                    icechatOptions.DefaultFullName = parser.GetSetting("Default Server Settings", "FullName", "The Chat Cool People Use");
                    icechatOptions.DefaultIdent = parser.GetSetting("Default Server Settings", "Ident", "IceChat95");
                    icechatOptions.DefaultQuitMessage = parser.GetSetting("Default Server Settings", "QuitMessage", "$randquit");

                    icechatOptions.TimeStamp = parser.GetSetting("Main Settings", "TimeStampFormat", "").Replace("n", "m");
                    icechatOptions.ShowTimeStamp = parser.GetSettingBool("Main Settings", "UseTimeStamp", true);
                    icechatOptions.AskQuit = parser.GetSettingBool("Main Settings", "CheckBeforeClose", true);

                    //icechatOptions.FlashTaskBar = parser.GetSettingBool("Main Settings", "FlashTaskBar", false);
                    //icechatOptions.FlashTaskBarNumber = Convert.ToInt32(parser.GetSetting("Main Settings", "FlashCount", "5"));
                    icechatOptions.NewQueryForegound = parser.GetSettingBool("Main Settings", "QueryForeground", true);
                    icechatOptions.WhoisNewQuery = parser.GetSettingBool("Main Settings", "WhoisOnQuery", true);
                    icechatOptions.SaveWindowPosition = parser.GetSettingBool("Main Settings", "SaveWindowPosition", true);
                    icechatOptions.DisableQueries = parser.GetSettingBool("Main Settings", "NoQueries", false);

                    icechatOptions.ShowEmoticons = parser.GetSettingBool("Main Settings", "Emoticons", true);
                    icechatOptions.ShowEmoticonPicker = parser.GetSettingBool("View Settings", "ShowEmoticonButton", true);
                    icechatOptions.ShowColorPicker = parser.GetSettingBool("View Settings", "ShowColorButton", true);

                    icechatFonts.FontSettings = new FontSettingItem[8];

                    icechatFonts.FontSettings[0] = NewFontSetting("Console", parser.GetSetting("Fonts", "ConsoleFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "ConsoleFontSize", "12")));
                    icechatFonts.FontSettings[1] = NewFontSetting("Channel", parser.GetSetting("Fonts", "ChannelFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "ChannelFontSize", "12")));
                    icechatFonts.FontSettings[2] = NewFontSetting("Query", parser.GetSetting("Fonts", "QueryFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "QueryFontSize", "12")));
                    icechatFonts.FontSettings[3] = NewFontSetting("Nicklist", parser.GetSetting("Fonts", "NickListFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "NickListFontSize", "12")));
                    icechatFonts.FontSettings[4] = NewFontSetting("Serverlist", parser.GetSetting("Fonts", "ServerListFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "ServerListFontSize", "12")));
                    icechatFonts.FontSettings[5] = NewFontSetting("InputBox", parser.GetSetting("Fonts", "InputBoxFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "InputBoxFontSize", "12")));
                    icechatFonts.FontSettings[6] = NewFontSetting("DockTabs", parser.GetSetting("Fonts", "TabFontName", ""), Convert.ToInt32(parser.GetSetting("Fonts", "TabFontSize", "12")));
                    icechatFonts.FontSettings[7] = NewFontSetting("MenuBar", "Verdana", 10);


                    IceChatAliases aliasList = new IceChatAliases();

                    //check for any alias files
                    if (parser.GetSetting("AliasFiles", "n0", "").Length > 0)
                    {
                        //loop while not blank
                        int i = 0;
                        while (parser.GetSetting("AliasFiles", "n" + i.ToString(), "").Length > 0)
                        {
                            string aliasFile = parser.GetSetting("AliasFiles", "n" + i.ToString(), "");
                            string aliasPath = Path.GetDirectoryName(ofd.FileName) + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + aliasFile;
                            //System.Diagnostics.Debug.WriteLine("load " + aliasPath);
                            if (File.Exists(aliasPath))
                            {
                                System.IO.StreamReader file = new System.IO.StreamReader(aliasPath);
                                string alias;
                                while ((alias = file.ReadLine()) != null)
                                {
                                    if (alias.Length > 0)
                                    {
                                        AliasItem a = new AliasItem();
                                        string name = alias.Substring(0, alias.IndexOf(" ")).Trim();
                                        string command = alias.Substring(alias.IndexOf(" ") + 1);
                                        if (!command.StartsWith("/"))
                                            command = "/" + command;

                                        a.AliasName = name;
                                        a.Command = new String[] { command };

                                        aliasList.AddAlias(a);
                                    }
                                }

                                file.Close();

                            }
                            i++;
                        }
                    }
                    //write out the alias file                    
                    XmlSerializer serializerA = new XmlSerializer(typeof(IceChatAliases));
                    TextWriter textWriterA = new StreamWriter(FormMain.Instance.AliasesFile);
                    serializerA.Serialize(textWriterA, aliasList);
                    textWriterA.Close();
                    textWriterA.Dispose();

                    //check for popups file
                    IniParser popupParser = new IniParser(Path.GetDirectoryName(ofd.FileName) + Path.DirectorySeparatorChar + "popups.ini");

                    string[] queryPopup = popupParser.EnumSection("qpopup");
                    string[] nickPopup = popupParser.EnumSection("lpopup");
                    string[] consolePopup = popupParser.EnumSection("mpopup");
                    string[] channelPopup = popupParser.EnumSection("cpopup");
                    string[] buddyPopup = popupParser.EnumSection("npopup");

                    IceChatPopupMenus popupList = new IceChatPopupMenus();

                    popupList.AddPopup(NewPopupMenu("NickList", nickPopup));
                    popupList.AddPopup(NewPopupMenu("Console", consolePopup));
                    popupList.AddPopup(NewPopupMenu("Channel", channelPopup));
                    popupList.AddPopup(NewPopupMenu("Query", queryPopup));

                    XmlSerializer serializerP = new XmlSerializer(typeof(IceChatPopupMenus));
                    TextWriter textWriterP = new StreamWriter(FormMain.Instance.PopupsFile);
                    serializerP.Serialize(textWriterP, popupList);
                    textWriterP.Close();
                    textWriterP.Dispose();

                    //import servers.ini file
                    IniParser sParser = new IniParser(Path.GetDirectoryName(ofd.FileName) + Path.DirectorySeparatorChar + "servers.ini");

                    IceChatServers servers = new IceChatServers();
                    int totalServers = Convert.ToInt32(sParser.GetSetting("ServerList", "List", "0"));

                    for (int i = 1; i <= totalServers; i++)
                    {
                        ServerSetting s = new ServerSetting
                        {
                            ID = i,
                            ServerName = sParser.GetSetting(i.ToString(), "ServerName", ""),
                            NickName = sParser.GetSetting(i.ToString(), "Nickname", ""),
                            AltNickName = sParser.GetSetting(i.ToString(), "AltNickName", ""),
                            AwayNickName = sParser.GetSetting(i.ToString(), "AwayNick", ""),

                            ServerPort = sParser.GetSetting(i.ToString(), "ServerPort", "6667"),
                            FullName = sParser.GetSetting(i.ToString(), "FullName", "The Chat Cool People Use"),
                            IdentName = sParser.GetSetting(i.ToString(), "Ident", "IceChat95"),
                            QuitMessage = sParser.GetSetting(i.ToString(), "QuitMessage", "$randquit"),
                            SetModeI = sParser.GetSettingBool(i.ToString(), "AutoModeI", true),
                            ShowMOTD = sParser.GetSettingBool(i.ToString(), "ShowMOTD", true),
                            ShowPingPong = sParser.GetSettingBool(i.ToString(), "ShowPingPong", false),
                            RejoinChannels = sParser.GetSettingBool(i.ToString(), "RejoinChannels", true),
                            AutoStart = sParser.GetSettingBool(i.ToString(), "AutoStartup", false),
                            UseSSL = sParser.GetSettingBool(i.ToString(), "UseSSL", false),
                            Password = sParser.GetSetting(i.ToString(), "ServerPassword", ""),
                            NickservPassword = sParser.GetSetting(i.ToString(), "NickServPass", ""),

                            AutoJoinEnable = sParser.GetSettingBool(i.ToString(), "AutoJoinListEnable", true),
                            AutoJoinDelay = sParser.GetSettingBool(i.ToString(), "AutoJoinDelay", false),
                            AutoPerformEnable = sParser.GetSettingBool(i.ToString(), "AutoPerformEnable", true),

                            BNCIP = sParser.GetSetting(i.ToString(), "BNCServer", ""),
                            BNCPort = sParser.GetSetting(i.ToString(), "BNCPort", ""),
                            BNCUser = sParser.GetSetting(i.ToString(), "BNCUserName", ""),
                            BNCPass = sParser.GetSetting(i.ToString(), "BNCPassword", "")
                        };


                        if (sParser.GetSetting(i.ToString(), "AutoJoin0", "").Length > 0)
                        {
                            string a = "";
                            int c = 0;
                            List<String> channels = new List<string>();
                            do
                            {
                                a = sParser.GetSetting(i.ToString(), "AutoJoin" + c.ToString(), "").Replace(":", " ");
                                if (a.Length > 0)
                                    channels.Add(a);
                                c++;
                            }
                            while (a.Length > 0);

                            s.AutoJoinChannels = new string[channels.Count];
                            for (c = 0; c < channels.Count; c++)
                                s.AutoJoinChannels[c] = channels[c];

                        }

                        if (sParser.GetSetting(i.ToString(), "AutoPerform0", "").Length > 0)
                        {
                            string a = "";
                            int c = 0;
                            List<String> commands = new List<string>();
                            do
                            {
                                a = sParser.GetSetting(i.ToString(), "AutoPerform" + c.ToString(), "");
                                if (a.Length > 0)
                                    commands.Add(a);
                                c++;
                            }
                            while (a.Length > 0);

                            s.AutoPerform = new string[commands.Count];
                            for (c = 0; c < commands.Count; c++)
                                s.AutoPerform[c] = commands[c];

                        }

                        servers.AddServer(s);
                    }

                    XmlSerializer serializerS = new XmlSerializer(typeof(IceChatServers));
                    TextWriter textWriterS = new StreamWriter(FormMain.Instance.ServersFile);
                    serializerS.Serialize(textWriterS, servers);
                    textWriterS.Close();
                    textWriterS.Dispose();
                    if (SaveOptions != null)
                        SaveOptions(icechatOptions, icechatFonts);


                    this.Close();

                }
                else
                {
                    MessageBox.Show("You need to choose the icechat.ini file.");
                }
            }            
        }

        private ServerMessageFormatItem NewMessageFormat(string messageName, string message)
        {
            ServerMessageFormatItem m = new ServerMessageFormatItem
            {
                MessageName = messageName,
                FormattedMessage = message
            };
            return m;
        }

        private FontSettingItem NewFontSetting(string windowType, string fontName, int fontSize)
        {
            FontSettingItem f = new FontSettingItem
            {
                WindowType = windowType,
                FontName = fontName,
                FontSize = fontSize
            };
            return f;
        }

    }
    
    public class IniParser
    {
        private String iniFilePath;
        private Dictionary<SectionPair, string> keyPairs = new Dictionary<SectionPair,string>();

        private struct SectionPair
        {
            public string Section;
            public string Key;
        }

        public IniParser(String iniPath)
        {
            TextReader iniFile = null;
            String strLine = null;
            String currentRoot = null;
            String[] keyPair = null;

            iniFilePath = iniPath;

            if (File.Exists(iniPath))
            {
                try
                {
                    iniFile = new StreamReader(iniPath);

                    strLine = iniFile.ReadLine();
                    
                    while (strLine != null)
                    {
                        //strLine = strLine.Trim().ToUpper();
                        strLine = strLine.Trim();

                        if (strLine != "")
                        {
                            if (strLine.StartsWith("[") && strLine.EndsWith("]"))
                            {                                
                                currentRoot = strLine.Substring(1, strLine.Length - 2);
                            }
                            else
                            {
                                keyPair = strLine.Split(new char[] { '=' }, 2);

                                SectionPair sectionPair;
                                String value = null;

                                if (currentRoot == null)
                                    currentRoot = "ROOT";

                                sectionPair.Section = currentRoot;
                                sectionPair.Key = keyPair[0];

                                if (keyPair.Length > 1)
                                    value = keyPair[1];
                                
                                keyPairs.Add(sectionPair, value);
                            }
                        }

                        strLine = iniFile.ReadLine();
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    throw ex;
                }
                finally
                {
                    if (iniFile != null)
                        iniFile.Close();
                }
            }
            else
                throw new FileNotFoundException("Unable to locate " + iniPath);

        }

        public String GetSetting(String sectionName, String settingName, String defaultValue)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;
            try
            {
                if (keyPairs[sectionPair] == null)
                    return defaultValue;
                else
                    return (string)keyPairs[sectionPair];
            }
            catch(Exception)
            {
                return defaultValue;
            }
        }

        public bool GetSettingBool(String sectionName, String settingName, bool defaultValue)
        {
            SectionPair sectionPair;
            sectionPair.Section = sectionName;
            sectionPair.Key = settingName;

            bool value = defaultValue;
            try
            {
                if (keyPairs[sectionPair].ToString().Equals("1"))
                    value = true;
            }
            catch { }
            return value;
        }

        public String[] EnumSection(String sectionName)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Section.ToUpper() == sectionName.ToUpper())
                {
                    //these are added out of order, we need to sort them
                    string keyPair = keyPairs[pair].ToString();
                    //see if there is a / after the : key
                    if (keyPair.IndexOf(":") > -1)
                    {
                        int colon = keyPair.IndexOf(":");
                        //check if next char is a /
                        if (colon < keyPair.Length)
                        {
                            if (keyPair.Substring(colon + 1, 1) != "/")
                            {
                                //add the // in front of the command
                                keyPair = keyPair.Substring(0, colon) + "://" + keyPair.Substring(colon + 1);
                            }
                        }
                    }
                    
                    keyPair = keyPair.Replace("$$?", "$?");
                    tmpArray.Add(keyPair.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;").Replace( ((char)0).ToString(), ""));
                }
            }
            
            return (String[])tmpArray.ToArray(typeof(String));
        }

        public String[] EnumSectionTheme(String sectionName)
        {
            ArrayList tmpArray = new ArrayList();

            foreach (SectionPair pair in keyPairs.Keys)
            {
                if (pair.Section.ToUpper() == sectionName.ToUpper())
                {
                    //these are added out of order, we need to sort them
                    tmpArray.Add(pair.Key + (char)255 + keyPairs[pair].ToString().Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;").Replace(((char)0).ToString(), ""));
                }
            }

            return (String[])tmpArray.ToArray(typeof(String));
        }


    }

   
}
