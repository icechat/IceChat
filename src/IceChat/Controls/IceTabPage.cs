/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2016 Paul Vanderzee <snerf@icechat.net>
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
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

using System.Runtime.InteropServices;

using IceChatPlugin;

namespace IceChat
{
    public class IceTabPage : Panel, IDisposable, IComparable<IceTabPage>
    {
        private IRCConnection connection = null;

        private Hashtable nicks;

        //channel settings
        private string channelTopic = "";
        private string fullChannelMode = "";
        private string channelKey = "";

        internal struct channelMode
        {
            public char mode;
            public bool set;
            public string param;
        }
        private Hashtable channelModes;

        private bool isFullyJoined = false;
        private bool hasChannelInfo = false;
        private FormChannelInfo channelInfoForm = null;

        private delegate TextWindow CurrentWindowDelegate();
        private delegate void ChangeTopicDelegate(string topic);
        private delegate void ChangeTextDelegate(string text);
        private delegate void AddDccChatDelegate(string message);
        private delegate void AddConsoleTabDelegate(IRCConnection connection);
        
        private delegate void AddChannelListDelegate(string channel, int users, string topic);
        private delegate void ClearChannelListDelegate();
        private delegate void AddConsoleDelegate(ConsoleTab tab, TextWindow window);

        private Panel panelTopic;
        private Splitter topicSplitter;
        
        private TextWindow textWindow;
        private TextWindow textTopic;
        private WindowType windowType;
        private FlickerFreeListView channelList;
        private TextBox searchText;
        private List<ListViewItem> listItems;
        private TabControl consoleTab;

        private FormMain.ServerMessageType lastMessageType;
        
        private int customForeColor;
        private int customBackColor;

        private bool _disableConsoleSelectChangedEvent = false;
        private bool _disableSounds = false;

        private bool gotWhoList = false;
        private bool gotNamesList = false;
        private bool channelHop = false;
        private bool channelListComplete = false;
        private bool pinnedTab = false;

        private TcpClient dccSocket;
        private TcpListener dccSocketListener;
        private Thread dccThread;
        private Thread listenThread;
        private bool keepListening;

        private System.Timers.Timer dccTimeOutTimer;

        private bool _flashTab;
        private int _flashValue;
        private bool _eventOverLoad;
        private bool _dockControl;
        private bool _disableLogs;

        private bool _joinEventOverload;
        private int _joinEventLocation;
        private bool _partEventOverload;
        private int _partEventLocation;
        private bool _quitEventOverload;
        private int _quitEventLocation;


        private System.Drawing.Size windowSize;
        private System.Drawing.Point windowLocation;

        private InputPanel inputPanel;
        private bool isDetached;
        private string _tabCaption;
        private int _windowIndex;
        private Panel searchPanel = null;
        private FormMain _parent;

        public enum WindowType
        {
            Console = 1,
            Channel = 2,
            Query = 3,
            ChannelList = 4,
            DCCChat = 5,
            DCCFile = 6,
            Window = 7,
            Debug = 99
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam);

        private const uint SB_VERT = 1;
        private const uint ESB_DISABLE_BOTH = 0x3;
        private const uint ESB_ENABLE_BOTH = 0x0;
        private const int WM_VSCROLL = 0x115;
 

        public IceTabPage(WindowType windowType, string sCaption, FormMain parent) 
        {
            this._parent = parent;

            if (windowType == WindowType.Channel)
            {
                InitializeChannel();
                textTopic.NoEmoticons = true;
                textTopic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.textTopic.AppendText("\x000303Topic:", "");
            }
            else if (windowType == WindowType.Query)
            {
                InitializeChannel();
                panelTopic.Visible = false;
            }
            else if (windowType == WindowType.Console)
            {
                InitializeConsole();
            }
            else if (windowType == WindowType.ChannelList)
            {
                InitializeChannelList();
            }
            else if (windowType == WindowType.DCCChat)
            {
                InitializeChannel();
                panelTopic.Visible = false;
            }
            else if (windowType == WindowType.Window)
            {
                InitializeChannel();
                panelTopic.Visible = false;
                textWindow.NoEmoticons = true;
            }
            else if (windowType == WindowType.Debug)
            {
                InitializeChannel();
                panelTopic.Visible = false;
                textWindow.NoEmoticons = true;
            }

            this.inputPanel = new InputPanel();
            this.inputPanel.Dock = DockStyle.Bottom;

            inputPanel.OnCommand += new InputPanel.OnCommandDelegate(inputPanel_OnCommand);
            inputPanel.InputBoxFont = new Font(_parent.IceChatFonts.FontSettings[5].FontName, _parent.IceChatFonts.FontSettings[5].FontSize);
            inputPanel.ShowColorPicker = _parent.IceChatOptions.ShowColorPicker;
            inputPanel.ShowEmoticonPicker = _parent.IceChatOptions.ShowEmoticonPicker;
            inputPanel.ShowBasicCommands = _parent.IceChatOptions.ShowBasicCommands;
            inputPanel.ShowSendButton = _parent.IceChatOptions.ShowSendButton;

            inputPanel.ShowWideTextPanel = _parent.IceChatOptions.ShowMultilineEditbox;

            inputPanel.SetInputBoxColors();

            if (_parent.IceChatOptions.ShowEmoticons == false)
                inputPanel.ShowEmoticonPicker = false;

            this.Controls.Add(this.inputPanel);
            this.inputPanel.Visible = false;


            _tabCaption = sCaption;
            this.WindowStyle = windowType;
            //this.inputPanel.Visible = false;
            this.Name = "IceTabPage";

            nicks = new Hashtable();
            channelModes = new Hashtable();
            
            _flashTab = false;
            _flashValue = 0;
            _eventOverLoad = false;
            
            lastMessageType = FormMain.ServerMessageType.Default;
        }

        public int CompareTo(IceTabPage page)
        {
            if (page == null) return 0;            
            
            if (page.WindowStyle == WindowType.Console && this.WindowStyle != WindowType.Console)   //console always first
                return 1;


            if (page.WindowStyle == WindowType.Channel)
            {
                ChannelSetting cs = _parent.ChannelSettings.FindChannel(page.TabCaption, page.connection.ServerSetting.NetworkName);
                if (cs != null)
                {
                    if (page.WindowIndex > _windowIndex)
                        return -1;
                    else if (page._windowIndex < _windowIndex)
                        return 1;
                }
            }

            return 0;
        }

        protected override void Dispose(bool disposing)
        {
            //this will dispose the TextWindow, making it close the log file
            this.UIThread(delegate
            {
                if (this.windowType == WindowType.Channel || this.windowType == WindowType.Query)
                {
                    //System.Diagnostics.Debug.WriteLine("dispose text window for:" + this.TabCaption);
                    //textWindow.SaveDumpFile(false);
                    textWindow.Dispose();
                }

                if (this.windowType == WindowType.DCCChat)
                {
                    System.Diagnostics.Debug.WriteLine("disposing dcc chat");
                    if (dccSocket != null)
                    {
                        if (dccSocket.Connected)
                            dccSocket.Close();
                    }

                    if (dccSocketListener != null)
                        dccSocketListener.Stop();

                    if (dccThread != null)
                    {
                        if (dccThread.IsAlive)
                            dccThread.Abort();
                    }
                    if (listenThread != null)
                    {
                        listenThread.Abort();
                        if (listenThread.IsAlive)
                        {
                            System.Diagnostics.Debug.WriteLine("abort listen thread JOIN");
                            listenThread.Join();
                            System.Diagnostics.Debug.WriteLine("abort listen thread JOIN DONE");
                        }
                    }
                }

            });
        }

        /// <summary>
        /// Add a message to the Text Window for Selected Console Tab Connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        /// <param name="color"></param>
        internal void AddText(IRCConnection connection, string data, string timeStamp, bool scrollToBottom, FormMain.ServerMessageType lastMessageType)
        {
            foreach (ConsoleTab t in consoleTab.TabPages)
            {
                if (t.Connection == connection)
                {
                    t.LastMessageType = lastMessageType;
                    
                    ((TextWindow)t.Controls[0]).AppendText(data, timeStamp);
                    if (scrollToBottom)
                        ((TextWindow)t.Controls[0]).ScrollToBottom();
                    return;
                }
            }
        }

        /// <summary>
        /// Add a new Tab/Connection to the Console Tab Control
        /// </summary>
        /// <param name="connection"></param>
        internal void AddConsoleTab(IRCConnection connection)
        {
            this.UIThread(delegate
            {
                ConsoleTab t;
                if (connection.ServerSetting.UseBNC)
                    t = new ConsoleTab(connection.ServerSetting.BNCIP + " {BNC}");
                else
                    t = new ConsoleTab(connection.ServerSetting.ServerName);

                t.Connection = connection;

                TextWindow w = new TextWindow();
                w.Dock = DockStyle.Fill;
                w.Font = new System.Drawing.Font(_parent.IceChatFonts.FontSettings[0].FontName, _parent.IceChatFonts.FontSettings[0].FontSize);
                w.IRCBackColor = _parent.IceChatColors.ConsoleBackColor;
                w.NoEmoticons = true;

                AddConsole(t, w);
            });
        }

        private void AddConsole(ConsoleTab tab, TextWindow window)
        {
            this.UIThread(delegate
            {
                tab.Controls.Add(window);
                if (_parent.IceChatOptions.LogConsole && tab.Connection.ServerSetting.DisableLogging == false)
                    window.SetLogFile(false);

                consoleTab.TabPages.Add(tab);
                consoleTab.SelectedTab = tab;
            });
        }

        /// <summary>
        /// Temporary Method to create a NULL Connection for the Welcome Tab in the Console
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="serverName"></param>
        internal void AddConsoleTab(string serverName)
        {
            //this is only used for now, to show the "Welcome" Tab
            ConsoleTab t = new ConsoleTab(serverName);

            TextWindow w = new TextWindow();
            w.Dock = DockStyle.Fill;
            w.Font = new System.Drawing.Font(_parent.IceChatFonts.FontSettings[0].FontName, _parent.IceChatFonts.FontSettings[0].FontSize);
            w.IRCBackColor = _parent.IceChatColors.ConsoleBackColor;

            t.Controls.Add(w);
            consoleTab.TabPages.Add(t);
            consoleTab.SelectedTab = t;

        }

        internal Size WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }

        internal System.Drawing.Point WindowLocation
        {
            get { return windowLocation; }
            set { windowLocation = value; }
        }

        internal bool DockedForm
        {
            get { return _dockControl; }
            set { _dockControl = value; }
        }
        
        internal InputPanel InputPanel
        {
            get { return inputPanel; }
        }

        internal bool Detached
        {
            get { return this.isDetached; }
            set
            {
                isDetached = value;
                inputPanel.Visible = isDetached;
            }
        }

        internal bool NickExists(string nick)
        {
            return nicks.ContainsKey(nick);
        }

        internal void UpdateChannelMode(char mode, bool addMode)
        {
            try
            {
                channelMode c = new channelMode();
                c.mode = mode;
                c.set = addMode;
                c.param = null;

                if (channelModes.Contains(mode))
                {
                    if (addMode)
                        channelModes[mode] = c;
                    else
                        channelModes.Remove(mode);
                }
                else
                {
                    channelModes.Add(mode, c);
                }

                string modes = "";
                string prms = " ";
                foreach (channelMode cm in channelModes.Values)
                {
                    modes += cm.mode.ToString();
                    if (cm.param != null)
                        prms += cm.param + " ";
                }

                if (modes.Trim().Length > 0)
                    ChannelModes = "+" + modes.Trim() + prms.TrimEnd();
                else
                    ChannelModes = "";
            }
            catch (Exception e)
            {
                _parent.WriteErrorFile(_parent.InputPanel.CurrentConnection,"IceTabPage UpdateChannelMode", e);
            }
        }

        internal void UpdateChannelMode(char mode, string param, bool addMode)
        {
            try
            {
                channelMode c = new channelMode();
                c.mode = mode;
                c.set = addMode;
                c.param = param;

                if (channelModes.Contains(mode))
                {
                    if (addMode)
                        channelModes[mode] = c;
                    else
                        channelModes.Remove(mode);
                }
                else
                {
                    if (addMode)
                        channelModes.Add(mode, c);
                }

                if (mode == 'k' && param != "*")
                {
                    if (addMode)
                        this.channelKey = param;
                    else
                        this.channelKey = "";
                }

                string modes = "";
                string prms = " ";
                foreach (channelMode cm in channelModes.Values)
                {
                    modes += cm.mode.ToString();
                    if (cm.param != null)
                        prms += cm.param + " ";
                }
                
                if (modes.Trim().Length > 0)
                    ChannelModes = "+" + modes.Trim() + prms.TrimEnd();
                else
                    ChannelModes = "";
            }
            catch (Exception e)
            {
                _parent.WriteErrorFile(_parent.InputPanel.CurrentConnection,"IceTabPage UpdateChannelMode2", e);
            }
        }

        internal void UpdateNick(string nick, string mode, bool addMode)
        {
            string justNick = nick;

            //for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
            //    justNick = justNick.Replace(connection.ServerSetting.StatusModes[1][i].ToString(), string.Empty);

            for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
                if (justNick.StartsWith(connection.ServerSetting.StatusModes[1][i].ToString()))
                    justNick = justNick.Substring(1);


            if (nicks.ContainsKey(justNick))
            {
                User u = (User)nicks[justNick];
                
                for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
                    if (mode == connection.ServerSetting.StatusModes[1][i].ToString())
                    {
                        u.Level[i] = addMode;
                        //update the nick color
                        if (u.Level[i])
                        {
                            switch (connection.ServerSetting.StatusModes[0][i])
                            {
                                case 'q':
                                    u.nickColor = _parent.IceChatColors.ChannelOwnerColor;
                                    break;
                                case 'a':
                                    u.nickColor = _parent.IceChatColors.ChannelAdminColor;
                                    break;
                                case 'o':
                                    u.nickColor = _parent.IceChatColors.ChannelOpColor;
                                    break;
                                case 'h':
                                    u.nickColor = _parent.IceChatColors.ChannelHalfOpColor;
                                    break;
                                case 'v':
                                    u.nickColor = _parent.IceChatColors.ChannelVoiceColor;
                                    break;
                                default:
                                    u.nickColor = _parent.IceChatColors.ChannelRegularColor;
                                    break;
                             }
                             break;
                        }                
                    }
                if (_parent.CurrentWindow == this)
                    _parent.NickList.RefreshList(this);
            }
        }

        internal User GetNick(string nick)
        {
            for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
                if (nick.StartsWith(connection.ServerSetting.StatusModes[1][i].ToString()))
                    nick = nick.Substring(1);

            if (nicks.ContainsKey(nick))
                return (User)nicks[nick];

            return null;
        }

        internal User GetNick(int nickNumber)
        {
            if (nickNumber <= nicks.Count)
            {
                int i = 1;
                foreach (User u in nicks.Values)
                {
                    if (nickNumber == i)
                        return u;
                    i++;
                }
            }
            return null;
        }

        internal User AddNick(string nick, bool refresh)
        {
            //replace any user modes from the nick
            string justNick = nick;
            if (connection != null)            
                for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
                    if (justNick.StartsWith(connection.ServerSetting.StatusModes[1][i].ToString()))
                        justNick = justNick.Substring(1);

            if (nicks.ContainsKey(justNick))
                return null;

            User u = new User(nick, connection);

            nicks.Add(justNick, u);
            if (refresh)
            {
                if (_parent.CurrentWindow == this)
                    _parent.NickList.RefreshList(this);
            }

            return u;
        }

        internal void RemoveNick(string nick)
        {
            nicks.Remove(nick);
            if (_parent.CurrentWindow == this)
                _parent.NickList.RefreshList(this);
            
        }

        internal void ClearNicks()
        {
            nicks.Clear();
        }

        internal Hashtable Nicks
        {
            get { return nicks; }
        }

        internal TextWindow TextWindow
        {
            get { return this.textWindow; }
        }

        internal TextWindow TopicWindow
        {
            get { return this.textTopic; }
        }

        internal bool JoinEventLocationOverload
        {
            get { return _joinEventOverload; }
            set
            {
                _joinEventOverload = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.JoinEventOverload = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.JoinEventOverload = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        internal int JoinEventLocation
        {
            get { return _joinEventLocation; }
            set
            {
                _joinEventLocation = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.JoinEventLocation = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.JoinEventLocation = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        internal bool PartEventLocationOverload
        {
            get { return _partEventOverload; }
            set
            {
                _partEventOverload = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.PartEventOverload = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.PartEventOverload = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        internal int PartEventLocation
        {
            get { return _partEventLocation; }
            set
            {
                _partEventLocation = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.PartEventLocation = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.PartEventLocation = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        internal bool QuitEventLocationOverload
        {
            get { return _quitEventOverload; }
            set
            {
                _quitEventOverload = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.QuitEventOverload = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.QuitEventOverload = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        internal int QuitEventLocation
        {
            get { return _quitEventLocation; }
            set
            {
                _quitEventLocation = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.QuitEventLocation = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.QuitEventLocation = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        //for disabling flashing
        internal bool EventOverLoad
        {
            get { return _eventOverLoad; }
            set { 
                _eventOverLoad = value;
                if (this.windowType == WindowType.Channel)
                {                    
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.EventsDisable = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.EventsDisable = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();
                }
            }
        }

        //whether to play sound events for this window
        internal bool DisableSounds
        {
            get { return _disableSounds; }
            set
            {
                _disableSounds = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.SoundsDisable = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.SoundsDisable = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;

                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    _parent.SaveChannelSettings();

                }

            }
        }

        internal bool LoggingDisable
        {
            get { return _disableLogs; }
            set {
                _disableLogs = value; 
                //do something??
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.LoggingDisable = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.LoggingDisable = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;

                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                    
                    _parent.SaveChannelSettings();
                }

            }
        }
        
        internal bool IsConnected
        {
            get
            {
                if (dccSocket != null)
                    return dccSocket.Connected;
                else
                    return false;
            }
        }

        internal bool GotWhoList
        {
            get { return gotWhoList; }
            set { gotWhoList = value; }
        }

        internal bool ChannelHop
        {
            get { return channelHop; }
            set { channelHop = value; }
        }

        internal bool GotNamesList
        {
            get { return gotNamesList; }
            set { gotNamesList = value; }
        }

        internal bool FlashTab
        {
            get { return _flashTab; }
            set { _flashTab = value; }
        }

        internal void ResetFlash()
        {
            _flashValue = 0;
        }

        internal int FlashValue
        {
            get
            {
                return _flashValue;
            }
            set
            {
                _flashValue = value;
            }
        }

        internal void DisconnectDCC()
        {
            if (dccSocket != null)
                dccSocket.Close();
            else if (dccSocketListener != null)
                dccSocketListener.Stop();
        }

        internal void ChannelSettings(string network, bool setWindowSize)
        {
            if (windowType == WindowType.Channel)
            {
                ChannelSetting cs = _parent.ChannelSettings.FindChannel(_tabCaption, network);
                if (cs != null)
                {
                    _eventOverLoad = cs.EventsDisable;
                    _disableSounds = cs.SoundsDisable;
                    textWindow.NoColorMode = cs.NoColorMode;
                    _disableLogs = cs.LoggingDisable;
                    _windowIndex = cs.WindowIndex;
                    _joinEventOverload = cs.JoinEventOverload;
                    _joinEventLocation = cs.JoinEventLocation;
                    _partEventOverload = cs.PartEventOverload;
                    _partEventLocation = cs.PartEventLocation;
                    _quitEventOverload = cs.QuitEventOverload;
                    _quitEventLocation = cs.QuitEventLocation;

                    ShowTopicBar = !cs.HideTopicBar;
                    pinnedTab = cs.PinnedTab;

                    //set the window size
                    if (setWindowSize == true && _parent.IceChatOptions.SaveWindowPosition == true)
                    {
                        if (this.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)this.Parent).Size = cs.WindowSize;
                            ((FormWindow)this.Parent).Location = cs.WindowLocation;
                        }
                    }
                }
            }
            else if (windowType == WindowType.Query)
            {
                ChannelSetting cs = _parent.ChannelSettings.FindChannel(_tabCaption, network);
                if (cs != null)
                {
                    pinnedTab = cs.PinnedTab;

                    //set the window size
                    if (setWindowSize == true && _parent.IceChatOptions.SaveWindowPosition == true)
                    {
                        if (this.Parent.GetType() == typeof(FormWindow))
                        {
                            ((FormWindow)this.Parent).Size = cs.WindowSize;
                            ((FormWindow)this.Parent).Location = cs.WindowLocation;
                        }
                    }
                }
            }
        }

        internal void dccTimeOutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string msg = _parent.GetMessageFormat("DCC Chat Timeout");
            msg = msg.Replace("$nick", _tabCaption);

            PluginArgs args = new PluginArgs(this.textWindow, "", _tabCaption, "", msg);
            args.Connection = this.connection;

            foreach (Plugin p in  _parent.LoadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
                if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                        args = ipc.plugin.DCCChatTimeOut(args);
                }
            }

            textWindow.AppendText(args.Message, "");
            this.LastMessageType = FormMain.ServerMessageType.ServerMessage;

            System.Diagnostics.Debug.WriteLine("timed out");

            dccTimeOutTimer.Stop();
            
            System.Diagnostics.Debug.WriteLine("timed out 1");
            dccSocketListener.Stop();
            
            System.Diagnostics.Debug.WriteLine("timed out 2");
            keepListening = false;

            System.Diagnostics.Debug.WriteLine("timed out 3");

            //listenThread.Abort();
        }


        internal void RequestDCCChat()
        {
            //send out a dcc chat request
            string localIP = "";
            if (_parent.IceChatOptions.DCCLocalIP != null && _parent.IceChatOptions.DCCLocalIP.Length > 0)
            {
                localIP = IPAddressToLong(IPAddress.Parse(_parent.IceChatOptions.DCCLocalIP)).ToString();
            }
            else
            {
                if (connection.ServerSetting.LocalIP == null || connection.ServerSetting.LocalIP.ToString().Length == 0)
                {
                    //error. no local IP found
                    _parent.WindowMessage(connection, _tabCaption, "\x000304DCC ERROR, no Router/Firewall IP Address specified in DCC Settings", "", true);
                    this.LastMessageType = FormMain.ServerMessageType.ServerMessage; 
                    return;
                }
                else
                {
                    localIP = IPAddressToLong(connection.ServerSetting.LocalIP).ToString();
                }
            }
            
            
            Random port = new Random();
            int p = port.Next(_parent.IceChatOptions.DCCPortLower, _parent.IceChatOptions.DCCPortUpper);

            dccSocketListener = new TcpListener(new IPEndPoint(IPAddress.Any, Convert.ToInt32(p)));

            object args = new object[2] { localIP, p };

            listenThread = new Thread(new ParameterizedThreadStart(ListenForConnection));
            listenThread.Name = "DCCListenThread";
            listenThread.Start(args);
            
            System.Diagnostics.Debug.WriteLine("dcc chat outgoing :" + localIP.ToString() + ":port:" + p.ToString());
            //connection.SendData("PRIVMSG " + _tabCaption + " :DCC CHAT chat " + localIP + " " + p.ToString() + "");
            dccTimeOutTimer = new System.Timers.Timer();
            dccTimeOutTimer.Interval = 1000 * _parent.IceChatOptions.DCCChatTimeOut;
            dccTimeOutTimer.Elapsed += new System.Timers.ElapsedEventHandler(dccTimeOutTimer_Elapsed);
            dccTimeOutTimer.Start();
        }

        private void ListenForConnection(object portIP)
        {
            this.dccSocketListener.Start();

            Array argArray = new object[2];
            argArray = (Array)portIP;

            string localIP = (string)argArray.GetValue(0);
            int port = (int)argArray.GetValue(1);

            connection.SendData("PRIVMSG " + _tabCaption + " :\x0001DCC CHAT chat " + localIP + " " + port.ToString() + "\x0001");
            System.Diagnostics.Debug.WriteLine("PRIVMSG " + _tabCaption + " :\x0001DCC CHAT chat " + localIP + " " + port.ToString() + "\x0001");
            System.Diagnostics.Debug.WriteLine("start listening:" + dccSocketListener.Pending());
            keepListening = true;

            while (keepListening)
            {
                try
                {
                    dccSocket = dccSocketListener.AcceptTcpClient();
                    
                    keepListening = false;
                    System.Diagnostics.Debug.WriteLine("accepted");
                    dccSocketListener.Stop();
                    dccTimeOutTimer.Stop();
                    

                    string msg = _parent.GetMessageFormat("DCC Chat Connect");
                    msg = msg.Replace("$nick", _tabCaption);

                    PluginArgs args = new PluginArgs(this.textWindow, "", this.connection.ServerSetting.CurrentNickName, "", msg);
                    args.Connection = this.connection;

                    foreach (Plugin p in _parent.LoadedPlugins)
                    {
                        IceChatPlugin ipc = p as IceChatPlugin;
                        if (ipc != null)
                        {
                            if (ipc.plugin.Enabled == true)
                                args = ipc.plugin.DCCChatConnected(args);
                        }
                    }

                    textWindow.AppendText(args.Message, "");
                    this.LastMessageType = FormMain.ServerMessageType.ServerMessage;

                    dccThread = new Thread(new ThreadStart(GetDCCData));
                    dccThread.Name = "DCCDataThread";
                    dccThread.Start();
                    break;

                }
                catch (ThreadAbortException tx)
                {
                    System.Diagnostics.Debug.WriteLine("listen thread exception:" + tx.Message + ":" + tx.StackTrace);
                }
                catch (SocketException se)
                {
                    System.Diagnostics.Debug.WriteLine("listen thread socket exception:" + se.Message + ":" + se.StackTrace);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("listen exception:" + ex.Message + ":" + ex.StackTrace);
                }
            }
            System.Diagnostics.Debug.WriteLine("finished listening on port " + port + ":" + keepListening);
        }

        internal void StartDCCChat(string nick, string ip, string port)
        {
            dccSocket = new TcpClient();
            System.Diagnostics.Debug.WriteLine("start dcc chat " + ip + " on port " + port);
            try
            {
                IPAddress ipAddr = LongToIPAddress(ip);
                IPEndPoint ep = new IPEndPoint(ipAddr, Convert.ToInt32(port));
                System.Diagnostics.Debug.WriteLine("attempting to connect on port " + port + " from address " + ipAddr.ToString() + ":" + ipAddr);

                dccSocket.Connect(ep);
                if (dccSocket.Connected)
                {
                    string msg = _parent.GetMessageFormat("DCC Chat Connect");
                    msg = msg.Replace("$nick", nick).Replace("$ip", ip).Replace("$port", port);

                    PluginArgs args = new PluginArgs(this.textWindow, "", nick, ip, msg);
                    args.Extra = port;
                    args.Connection = this.connection;

                    foreach (Plugin p in  _parent.LoadedPlugins)
                    {
                        IceChatPlugin ipc = p as IceChatPlugin;
                        if (ipc != null)
                        {
                            if (ipc.plugin.Enabled == true)
                                args = ipc.plugin.DCCChatConnected(args);
                        }
                    }
                    
                    if (dccTimeOutTimer != null)
                        dccTimeOutTimer.Stop();

                    textWindow.AppendText(args.Message, "");
                    this.LastMessageType = FormMain.ServerMessageType.ServerMessage;

                    dccThread = new Thread(new ThreadStart(GetDCCData));
                    dccThread.Name = "DCCChatThread_" + nick;
                    dccThread.Start();
                }
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine(se.Message + ":" + se.StackTrace);
                textWindow.AppendText("\x000304" +se.Message, "");
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
                textWindow.AppendText("\x000304" + e.Message + ":" + e.StackTrace, "");
            }
        }

        internal void SendDCCData(string message)
        {
            if (dccSocket != null)
            {
                if (dccSocket.Connected)
                {
                    NetworkStream ns = dccSocket.GetStream();
                    ASCIIEncoding encoder = new ASCIIEncoding();
                    byte[] buffer = encoder.GetBytes(message + "\n");
                    try
                    {
                        ns.Write(buffer, 0, buffer.Length);
                        ns.Flush();
                    }
                    catch { }
                }
            }
        }

        private void GetDCCData()
        {
            while (true)
            {
                try
                {
                    int buffSize = 0;
                    //byte[] buffer = new byte[8192];
                    NetworkStream ns = dccSocket.GetStream();
                    buffSize = dccSocket.ReceiveBufferSize;
                    byte[] buffer = new byte[buffSize];
                    int bytesRead = ns.Read(buffer, 0, buffSize);
                    Decoder d = Encoding.GetEncoding(this.connection.ServerSetting.Encoding).GetDecoder();
                    char[] chars = new char[buffSize];
                    int charLen = d.GetChars(buffer, 0, buffSize, chars, 0);

                    System.String strData = new System.String(chars);
                    if (bytesRead == 0)
                    {
                        //we have a disconnection
                        break;
                    }
                    //cut off the null chars
                    strData = strData.Substring(0, strData.IndexOf(Convert.ToChar(0x0).ToString()));

                    AddDCCMessage(strData);
                }
                catch (Exception)
                {
                    //we have an error
                    break;
                }
            }

            string msg = _parent.GetMessageFormat("DCC Chat Disconnect");
            msg = msg.Replace("$nick", _tabCaption);

            PluginArgs args = new PluginArgs(this.textWindow, "", _tabCaption, "", msg);
            args.Connection = this.connection;

            foreach (Plugin p in  _parent.LoadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
                if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                        args = ipc.plugin.DCCChatClosed(args);
                }
            }
            
            textWindow.AppendText(msg, "");
            this.LastMessageType = FormMain.ServerMessageType.ServerMessage;

            System.Diagnostics.Debug.WriteLine("dcc chat disconnect");
            dccSocket.Close();
        }

        private void AddDCCMessage(string message)
        {
            if (this.InvokeRequired)
            {
                AddDccChatDelegate a = new AddDccChatDelegate(AddDCCMessage);
                this.Invoke(a, new object[] { message });
            }
            else
            {
                string[] lines = message.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line[0] != (char)0 && line[0] != (char)1)
                    {
                        string msg = _parent.GetMessageFormat("DCC Chat Message");
                        msg = msg.Replace("$nick", _tabCaption);
                        msg = msg.Replace("$message", line);

                        PluginArgs args = new PluginArgs(this.textWindow, "", _tabCaption, "", msg);
                        args.Connection = this.connection;

                        foreach (Plugin p in  _parent.LoadedPlugins)
                        {
		                    IceChatPlugin ipc = p as IceChatPlugin;
                            if (ipc != null)
                            {
                                if (ipc.plugin.Enabled == true)
                                    args = ipc.plugin.DCCChatMessage(args);
                            }
                        }

                        textWindow.AppendText(args.Message, "");
                        this.LastMessageType = FormMain.ServerMessageType.Message;

                    }
                    else if (line[0] == (char)1)
                    {
                        //action
                        string action = line.Substring(8);
                        action = action.Substring(0, action.Length - 1);
                        
                        string msg = _parent.GetMessageFormat("DCC Chat Action");
                        msg = msg.Replace("$nick", _tabCaption);
                        msg = msg.Replace("$message", action);

                        PluginArgs args = new PluginArgs(this.textWindow, "", _tabCaption, "", msg);
                        args.Connection = this.connection;

                        foreach (Plugin p in _parent.LoadedPlugins)
                        {
                            IceChatPlugin ipc = p as IceChatPlugin;
                            if (ipc != null)
                            {
                                if (ipc.plugin.Enabled == true)
                                    args = ipc.plugin.DCCChatMessage(args);
                            }
                        }

                        textWindow.AppendText(args.Message, "");
                        this.LastMessageType = FormMain.ServerMessageType.Action;

                    }
                }
            }
        }

        private long IPAddressToLong(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            return (long)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]); 
        }

        private IPAddress LongToIPAddress(string longIP)
        {
            byte[] quads = BitConverter.GetBytes(long.Parse(longIP, System.Globalization.CultureInfo.InvariantCulture));
            return IPAddress.Parse(quads[3] + "." + quads[2] + "." + quads[1] + "." + quads[0]);
        }

        private long NetworkUnsignedLong(long hostOrderLong)
        {
            long networkLong = IPAddress.HostToNetworkOrder(hostOrderLong);
            //Network order has the octets in reverse order starting with byte 7
            //To get the correct string simply shift them down 4 bytes
            //and zero out the first 4 bytes.
            return (networkLong >> 32) & 0x00000000ffffffff;
        }

        /// <summary>
        /// Return the Connection for the Current Selected in the Console Tab Control
        /// </summary>
        internal IRCConnection CurrentConnection
        {
            get
            {
                return ((ConsoleTab)consoleTab.SelectedTab).Connection;
            }
        }

        internal IRCConnection Connection
        {
            get
            {
                return this.connection;
            }
            set
            {
                this.connection = value;
                inputPanel.CurrentConnection = this.connection;
            }
        }

        private IRCConnection ActiveConnection
        {
            get
            {
                if (this.windowType == WindowType.Console)
                    return ((ConsoleTab)consoleTab.SelectedTab).Connection;
                else
                    return this.connection;
            }

        }

        internal WindowType WindowStyle
        {
            get
            {
                return windowType;
            }
            set
            {
                windowType = value;
                if (windowType == WindowType.Console)
                {
                    //nada
                }
                else if (windowType == WindowType.Channel)
                {
                    panelTopic.Visible = true;
                    textWindow.IRCBackColor = _parent.IceChatColors.ChannelBackColor;
                    textTopic.IRCBackColor = _parent.IceChatColors.ChannelBackColor;
                }
                else if (windowType == WindowType.Query)
                {
                    textWindow.IRCBackColor = _parent.IceChatColors.QueryBackColor;
                }
                else if (windowType == WindowType.ChannelList)
                {
                    //nada
                }
                else if (windowType == WindowType.DCCChat)
                {
                    textWindow.IRCBackColor = _parent.IceChatColors.QueryBackColor;
                }
                else if (windowType == WindowType.DCCFile)
                {
                    //nada
                }
                else if (windowType == WindowType.Window)
                {
                    //nada
                }
                else if (windowType == WindowType.Debug)
                {
                    //nada
                }

            }
        }

        internal bool PinnedTab
        {
            get
            {
                return pinnedTab;
            }
            set
            {
                pinnedTab = value;
                
                if (this.windowType == WindowType.Channel || this.windowType == WindowType.Query)
                {                    
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.PinnedTab = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.PinnedTab = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                }
                else if (this.windowType == WindowType.Console || this.windowType == WindowType.Debug || this.windowType == WindowType.Window)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, "");

                    if (cs != null)
                    {
                        cs.PinnedTab = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.PinnedTab = value;
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = "";

                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                }

                _parent.SaveChannelSettings();
                _parent.ChannelBar.Invalidate();
            }
        }

        internal int WindowIndex
        {
            get { return _windowIndex; }
            set
            {
                _windowIndex = value;
                if (this.windowType == WindowType.Channel)
                {
                    ChannelSetting cs = _parent.ChannelSettings.FindChannel(this.TabCaption, this.Connection.ServerSetting.NetworkName);
                    if (cs != null)
                    {
                        cs.WindowIndex = value;
                    }
                    else
                    {
                        ChannelSetting cs1 = new ChannelSetting();
                        cs1.ChannelName = this.TabCaption;
                        cs1.NetworkName = this.connection.ServerSetting.NetworkName;
                        cs1.WindowIndex = value;
                        _parent.ChannelSettings.AddChannel(cs1);
                    }
                }
            }            
        }

        internal string ChannelModes
        {
            get
            {
                return fullChannelMode;
            }
            set
            {
                fullChannelMode = value;
            }
        }

        internal Hashtable ChannelModesHash
        {
            get
            {
                return channelModes;
            }
        }

        internal string ChannelKey
        {
            get
            {
                return channelKey;
            }
            set
            {
                channelKey = value;
            }
        }

        internal string ChannelTopic
        {
            get
            {
                return channelTopic;
            }
            set
            {
                channelTopic = value;
                UpdateTopic(value);
            }
        }

        internal bool IsFullyJoined
        {
            get
            {
                return isFullyJoined;
            }
            set
            {
                isFullyJoined = value;
            }
        }

        internal bool HasChannelInfo
        {
            get
            {
                return hasChannelInfo;
            }
            set
            {
                hasChannelInfo = value;
            }
        }

        internal FormChannelInfo ChannelInfoForm
        {
            get
            {
                return channelInfoForm;
            }
            set
            {
                channelInfoForm = value;
            }
        }

        internal FlickerFreeListView ChannelList
        {
            get
            {
                return channelList;
            }
        }

        internal bool ChannelListComplete
        {
            get
            {
                return channelListComplete;
            }
            set
            {
                channelListComplete = value;
                ShowSearchPanel();
            }
        }

        internal bool ShowTopicBar
        {
            get
            {
                return this.panelTopic.Visible;
            }
            set
            {
                if (_parent.IceChatOptions.ShowTopic == false)
                {
                    //dont allow to update

                }
                else
                {
                    this.panelTopic.Visible = value;
                }
            }
        }

        internal TabControl ConsoleTab
        {
            get { return consoleTab; }
        }

        internal FormMain.ServerMessageType LastMessageType
        {
            get
            {
                return lastMessageType;
            }
            set
            {
                if (lastMessageType != value || value == FormMain.ServerMessageType.CustomMessage)
                {
                    //check if we are the current window or not
                    if (this == _parent.CurrentWindow)
                    {
                        lastMessageType = FormMain.ServerMessageType.Default;
                        return;
                    }
                    
                    // do not change if already a New Message (or override if it is a custom color)
                    
                    if (lastMessageType != FormMain.ServerMessageType.Message || value == FormMain.ServerMessageType.CustomMessage)
                    {
                        if (this._eventOverLoad == false)
                        {
                            if (lastMessageType == FormMain.ServerMessageType.CustomMessage)
                            {
                                // dont over write
                            }
                            else if (lastMessageType == FormMain.ServerMessageType.Action)
                            {
                                if (value == FormMain.ServerMessageType.Message)
                                {
                                    //only allow action to be over riden by message
                                    lastMessageType = value;
                                    _parent.ChannelBar.Invalidate();
                                    _parent.ServerTree.Invalidate();
                                }
                            }
                            else
                            {
                                lastMessageType = value;
                                _parent.ChannelBar.Invalidate();
                                _parent.ServerTree.Invalidate();
                            }
                        } // allow event if DisableFlashing is enabled
                        else if (value == FormMain.ServerMessageType.CustomMessage)
                        {
                            lastMessageType = value;
                            _parent.ChannelBar.Invalidate();
                            _parent.ServerTree.Invalidate();
                        }
                    }
                }
            }
        }

        internal int CustomForeColor
        {
            get 
            {
                return this.customForeColor;   
            }
            set
            {
                this.customForeColor = value;
            }
        }

        internal int CustomBackColor
        {
            get
            {
                return this.customBackColor;
            }
            set
            {
                this.customBackColor = value;
            }
        }

        internal TextWindow CurrentConsoleWindow()
        {
            if (this.InvokeRequired)
            {
                CurrentWindowDelegate cwd = new CurrentWindowDelegate(CurrentConsoleWindow);
                return (TextWindow)this.Invoke(cwd, new object[] { });
            }
            else
            {
                return (TextWindow)consoleTab.SelectedTab.Controls[0];
            }
        }

        internal int TotalChannels
        {
            get { return this.channelList.Items.Count; }
        }



        internal void ClearChannelList()
        {
            if (this.InvokeRequired)
            {
                ClearChannelListDelegate c = new ClearChannelListDelegate(ClearChannelList);
                this.Invoke(c, new object[] { });
            }
            else
                this.channelList.Items.Clear();        
        }

        private void UpdateText(string text)
        {
            if (this.InvokeRequired)
            {
                ChangeTextDelegate c = new ChangeTextDelegate(UpdateText);
                this.Invoke(c, new object[] { text });
            }
            else
            {
                this.Text = text;
                this.Update();
            }
        }

        private void UpdateTopic(string topic)
        {
            if (this.InvokeRequired)
            {
                ChangeTopicDelegate c = new ChangeTopicDelegate(UpdateTopic);
                this.Invoke(c, new object[] { topic });
            }
            else
            {
                channelTopic = topic;
                textTopic.ClearTextWindow();
                string msgt = _parent.GetMessageFormat("Channel Topic Text");
                msgt = msgt.Replace("$channel", this.TabCaption);
                msgt = msgt.Replace("$topic", topic);
                textTopic.AppendText(msgt, "");
            }   
        }

        /// <summary>
        /// Add the specified channel list data to the ListView
        /// </summary>
        /// <param name="channel">Channel Name</param>
        /// <param name="users">The number of users</param>
        /// <param name="topic">The channel topic</param>
        internal void AddChannelList(string channel, int users, string topic)
        {
            if (this.InvokeRequired)
            {
                AddChannelListDelegate a = new AddChannelListDelegate(AddChannelList);
                this.Invoke(a, new object[] { channel, users, topic });
            }
            else
            {
                channelList.BeginUpdate();
                
                ListViewItem lvi = new ListViewItem(channel);
                lvi.ToolTipText = topic;
                lvi.SubItems.Add(users.ToString());
                lvi.SubItems.Add(topic);
                channelList.Items.Add(lvi);

                channelList.EndUpdate();
            }  
        }

        private void OnControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.Dock = DockStyle.Fill;
        }

        public string TabCaption
        {
            get { return _tabCaption; }
            set { this._tabCaption = value; }
        }

        internal void SelectConsoleTab(ConsoleTab c)
        {
            _disableConsoleSelectChangedEvent = true;
            consoleTab.SelectedTab = c;
            StatusChange();
            _disableConsoleSelectChangedEvent = false;
        }

        internal void ResizeTopicFont(string fontName, float fontSize)
        {
            //resize the font for the topic, and make the box size accordingly
            textTopic.Font = new Font(fontName, fontSize);
            this.panelTopic.Size = new System.Drawing.Size(panelTopic.Width,(int) fontSize * 2);
            this.panelTopic.MinimumSize = new System.Drawing.Size(panelTopic.Width, (int)fontSize );
            
            this.panelTopic.Visible = _parent.IceChatOptions.ShowTopic;
        }

        private void StatusChange()
        {
            _parent.InputPanel.CurrentConnection = ((ConsoleTab)consoleTab.SelectedTab).Connection;

            string network = "";
            if (((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NetworkName.Length > 0)
                network = " (" + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NetworkName + ")";

            if (((ConsoleTab)consoleTab.SelectedTab).Connection.IsConnected)
            {
                string ssl = "";
                if (((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.UseSSL)
                    ssl = " {SSL}";

                if (((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.UseBNC)
                    //_parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.CurrentNickName + " connected to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.BNCIP +  " {BNC}" + ssl);
                    _parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.CurrentNickName + " connected to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.RealServerName +  " {BNC " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.BNCIP + "} " + ssl);
                else
                {
                    if (((ConsoleTab)consoleTab.SelectedTab).Connection.IsFullyConnected == true)
                        _parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.CurrentNickName + " connected to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.RealServerName + ssl + network);
                    else
                        _parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.CurrentNickName + " connecting to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.ServerName + ssl);
                }
            }
            else
            {
                if (((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.UseBNC)
                    _parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NickName + " disconnected from " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.BNCIP);
                else
                    _parent.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NickName + " disconnected from " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.ServerName + network);
            }
        }

        private void OnTabConsoleSelectedIndexChanged(object sender, EventArgs e)
        {
            ((TextWindow)(consoleTab.SelectedTab.Controls[0])).resetUnreadMarker(); 
            
            if (consoleTab.TabPages.IndexOf(consoleTab.SelectedTab) != 0 && !_disableConsoleSelectChangedEvent)
            {
                StatusChange();
                
                //highlite the proper item in the server tree
                _parent.ServerTree.SelectTab(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting, false);                
            }
            else
            {
                _parent.InputPanel.CurrentConnection = null;
                _parent.StatusText("Welcome to " + FormMain.ProgramID + " " + FormMain.VersionID);
            }            
        }

        private void OnTabConsoleMouseUp(object sender, MouseEventArgs e)
        {
            _parent.FocusInputBox();
        }

        /// <summary>
        /// Checks if Left Mouse Button is Pressed by the "X" button
        /// Quits Server if Server is Connected
        /// Closes Server Tab if Server is Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabConsoleMouseDown(object sender, MouseEventArgs e)
        {                        
            if (e.Button == MouseButtons.Left)
            {
                for (int i = consoleTab.TabPages.Count - 1; i >= 0; i--)
                {
                    if (consoleTab.GetTabRect(i).Contains(e.Location) && i == consoleTab.SelectedIndex)
                    {
                        if (((ConsoleTab)consoleTab.TabPages[i]).Connection != null)
                        {
                            if (e.Location.X > consoleTab.GetTabRect(i).Right - 20)
                            {
                                if (((ConsoleTab)consoleTab.TabPages[i]).Connection.IsConnected)
                                {
                                    if (((ConsoleTab)consoleTab.TabPages[i]).Connection.IsFullyConnected)
                                    {
                                        ((ConsoleTab)consoleTab.TabPages[i]).Connection.AttemptReconnect = false;
                                        _parent.ParseOutGoingCommand(((ConsoleTab)consoleTab.TabPages[i]).Connection, "//quit " + ((ConsoleTab)consoleTab.TabPages[i]).Connection.ServerSetting.QuitMessage);
                                        return;
                                    }
                                }
                                //close all the windows related to this tab
                                RemoveConsoleTab(i);
                                return;
                            }
                            ((ConsoleTab)consoleTab.TabPages[i]).LastMessageType = FormMain.ServerMessageType.Default;
                        }
                    }
                }
            }
        }

        internal void RemoveConsoleTab(int index)
        {
            _parent.CloseAllWindows(((ConsoleTab)consoleTab.TabPages[index]).Connection);            
            //remove the server connection from the collection
            ((ConsoleTab)consoleTab.TabPages[index]).Connection.Dispose();
            _parent.ServerTree.ServerConnections.Remove(((ConsoleTab)consoleTab.TabPages[index]).Connection.ServerSetting.ID);
            
            consoleTab.TabPages.RemoveAt(consoleTab.TabPages.IndexOf(consoleTab.TabPages[index]));
        }


        private void OnControlRemoved(object sender, ControlEventArgs e)
        {
            //this will close the log file for the particular server tab closed
            try
            {
                if (e.Control.GetType() == typeof(ConsoleTab))
                {
                    if (((ConsoleTab)e.Control).Connection.ServerSetting.ID > 50000)
                    {
                        //temporary server, remove it from ServerTree
                        _parent.ServerTree.ServersCollection.RemoveServer(((ConsoleTab)e.Control).Connection.ServerSetting);
                    }
                    ((TextWindow)((ConsoleTab)e.Control).Controls[0]).Dispose();
                    _parent.ServerTree.Invalidate();
                }
            }
            catch { }
        }

        private void OnTabConsoleDrawItem(object sender, DrawItemEventArgs e)
        {
            string tabName = consoleTab.TabPages[e.Index].Text;
            Rectangle bounds = consoleTab.GetTabRect(e.Index);

            Rectangle textBounds = bounds;
            textBounds.X = textBounds.X + 5;
            textBounds.Y = textBounds.Y + 5;

            Brush br;
            if (e.State == DrawItemState.Selected)
                br = new LinearGradientBrush(bounds, IrcColor.colors[_parent.IceChatColors.TabBarCurrentBG1], IrcColor.colors[_parent.IceChatColors.TabBarCurrentBG2], 90);
            else
            {
                // get the last message value
                if (((ConsoleTab)consoleTab.TabPages[e.Index]).LastMessageType == FormMain.ServerMessageType.Default || tabName == "Welcome")
                    br = new LinearGradientBrush(bounds, IrcColor.colors[_parent.IceChatColors.TabBarOtherBG1], IrcColor.colors[_parent.IceChatColors.TabBarOtherBG2], 90);
                else
                    br = new LinearGradientBrush(bounds, IrcColor.colors[_parent.IceChatColors.ConsoleTabHighlite], IrcColor.colors[_parent.IceChatColors.ConsoleTabHighlite], 90);
                    //br = new SolidBrush( IrcColor.colors[_parent.IceChatColors.ConsoleTabHighlite]);
            }

            e.Graphics.FillRectangle(br, bounds);

            if (e.Index == consoleTab.SelectedIndex)
            {
                e.Graphics.DrawString(tabName, consoleTab.Font, new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarCurrent]), textBounds);
            }
            else
            {
                textBounds.Y = textBounds.Y + 2;
                e.Graphics.DrawString(tabName, consoleTab.Font, new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarDefault]), textBounds);
            }
            
            if (e.Index != 0 && e.Index == consoleTab.SelectedIndex)
            {
                System.Drawing.Image icon = StaticMethods.LoadResourceImage("CloseButton.png");
                e.Graphics.DrawImage(icon, bounds.Right - 20, bounds.Top + 4, 12, 12);
                icon.Dispose();
            }
            
            br.Dispose();

        }

        private void OnTabConsoleSelectingTab(object sender, TabControlCancelEventArgs e)
        {
            if (consoleTab.GetTabRect(e.TabPageIndex).Contains(consoleTab.PointToClient(Cursor.Position)) && e.TabPageIndex != 0)
            {
                if (this.PointToClient(Cursor.Position).X > consoleTab.GetTabRect(e.TabPageIndex).Right - 14)
                    e.Cancel = true;
            }
        }
        /// <summary>
        /// Create the Console Tab
        /// </summary>
        private void InitializeConsole()
        {
            this.SuspendLayout();
            // 
            // consoleTab
            // 
            this.consoleTab = new TabControl();
            this.consoleTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputPanel = new InputPanel();
            
            this.consoleTab.Font = new Font(_parent.IceChatFonts.FontSettings[6].FontName, _parent.IceChatFonts.FontSettings[6].FontSize);

            this.consoleTab.Location = new System.Drawing.Point(0, 0);
            this.consoleTab.Name = "consoleTab";
            this.consoleTab.SelectedIndex = 0;
            this.consoleTab.Size = new System.Drawing.Size(200, 100);
            this.consoleTab.TabIndex = 0;
            
            this.consoleTab.DrawMode = TabDrawMode.OwnerDrawFixed;
            this.consoleTab.SizeMode = TabSizeMode.Normal;
            this.consoleTab.Padding = new Point(15, 5);

            // 
            // ConsoleTabWindow
            // 
            this.Controls.Add(this.consoleTab);

            this.ResumeLayout(false);
            
            consoleTab.SelectedIndexChanged += new EventHandler(OnTabConsoleSelectedIndexChanged);
            consoleTab.MouseUp += new MouseEventHandler(OnTabConsoleMouseUp);
            consoleTab.MouseDown += new MouseEventHandler(OnTabConsoleMouseDown);
            consoleTab.DrawItem += new DrawItemEventHandler(OnTabConsoleDrawItem);
            consoleTab.Selecting += new TabControlCancelEventHandler(OnTabConsoleSelectingTab);

            consoleTab.ControlRemoved += new ControlEventHandler(OnControlRemoved);

        }

        /// <summary>
        /// Create the Channel List
        /// </summary>
        private void InitializeChannelList()
        {
            this.channelList = new FlickerFreeListView();
            this.channelList.SuspendLayout();
            this.SuspendLayout();

            this.channelList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            
            this.channelList.Location = new System.Drawing.Point(0, 0);
            this.channelList.Name = "channelList";
            this.channelList.DoubleClick += new EventHandler(channelList_DoubleClick);
            this.channelList.ColumnClick += new ColumnClickEventHandler(channelList_ColumnClick);
            this.channelList.MouseDown += new MouseEventHandler(channelList_MouseDown);
            
            this.channelList.BackColor = IrcColor.colors[FormMain.Instance.IceChatColors.ServerListBackColor];
            this.channelList.ForeColor = IrcColor.colors[FormMain.Instance.IceChatColors.TabBarDefault];

            this.channelList.ShowItemToolTips = true;

            ColumnHeader c = new ColumnHeader();
            c.Text = "Channel";
            c.Width = 200;
            this.channelList.Columns.Add(c);
            
            this.channelList.Columns.Add("Users");
            ColumnHeader t = new ColumnHeader();
            t.Text = "Topic";
            t.Width = 2000;
            this.channelList.Columns.Add(t);

            this.channelList.View = View.Details;
            this.channelList.MultiSelect = false;
            this.channelList.FullRowSelect = true;

            searchPanel = new Panel();
            searchPanel.Visible = false;
            searchPanel.Height = 30;

            Label searchLabel = new Label();
            searchLabel.Text = "Search Channels:";
            searchLabel.Dock = DockStyle.Left;
            searchLabel.AutoSize = true;
            searchLabel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            
            searchText = new TextBox();
            searchText.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            searchText.Dock = DockStyle.Fill;
            searchText.BorderStyle = BorderStyle.Fixed3D;
            searchText.KeyDown += new KeyEventHandler(searchText_KeyDown);

            Button searchButton = new Button();
            searchButton.Text = "Search";
            searchButton.Dock = DockStyle.Right;
            searchButton.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            searchButton.Click += new EventHandler(OnSearchButtonClick);
            
            searchPanel.Controls.Add(searchText);
            searchPanel.Controls.Add(searchLabel);
            searchPanel.Controls.Add(searchButton);
            
            searchPanel.Dock =  DockStyle.Bottom;
            this.channelList.Dock = DockStyle.Fill;

            this.Controls.Add(channelList);
            this.Controls.Add(searchPanel);
            this.channelList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ShowSearchPanel()
        {
            this.Invoke((MethodInvoker)delegate()
            {
                if (searchPanel != null)
                    searchPanel.Show();
            });

        }

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OnSearchButtonClick(sender, new EventArgs());
        }

        private void inputPanel_OnCommand(object sender, string data)
        {
            if (data.Length > 0)
            {
                if (WindowStyle == IceTabPage.WindowType.Console)
                {
                    _parent.ParseOutGoingCommand(ActiveConnection, data);
                    CurrentConsoleWindow().ScrollToBottom();
                }
                else if (WindowStyle != IceTabPage.WindowType.DCCFile && WindowStyle != IceTabPage.WindowType.ChannelList)
                {
                    _parent.ParseOutGoingCommand(ActiveConnection, data);
                    textWindow.ScrollToBottom();
                }

                //auto away settings
                if (ActiveConnection != null)
                {
                    if (data.StartsWith("/") == false)
                    {
                        if (_parent.IceChatOptions.AutoReturn == true)
                        {
                            if (ActiveConnection.ServerSetting.Away == true)
                            {
                                //return yourself
                                _parent.ParseOutGoingCommand(ActiveConnection, "/away");
                            }
                        }
                    }

                    //reset the auto away timer
                    if (data.StartsWith("/") == false)
                    {
                        if (_parent.IceChatOptions.AutoAway == true && _parent.IceChatOptions.AutoAwayTime > 0)
                        {
                            ActiveConnection.SetAutoAwayTimer(_parent.IceChatOptions.AutoAwayTime);
                        }
                    }
                }
            }
        }

        private void OnSearchButtonClick(object sender, EventArgs e)
        {
            //filter the channel list according to the specified text
            if (searchText.Text.Length == 0)
            {
                //restore the original list
                if (listItems != null)
                {                    
                    channelList.Items.Clear();
                    ListViewItem[] items = new ListViewItem[listItems.Count];
                    listItems.CopyTo(items);
                    channelList.Items.AddRange(items);
                }
                listItems = null;
            }
            else
            {
                if (listItems == null)
                {
                    listItems = new List<ListViewItem>();
                    ListViewItem[] items = new ListViewItem[channelList.Items.Count];
                    channelList.Items.CopyTo(items, 0);
                    listItems.AddRange(items);
                }
                
                channelList.Items.Clear();
                
                foreach (ListViewItem item in listItems)
                {
                    if (item.Text.Contains(searchText.Text))
                        channelList.Items.Add(item);        
                    else if (item.SubItems[2].Text.Contains(searchText.Text))
                        channelList.Items.Add(item);        
                }

            }


        }

        private void channelList_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            this.UIThread(delegate
            {
                channelList.BeginUpdate();

                // put this in a thread
                ListViewSorter Sorter = new ListViewSorter();
                channelList.ListViewItemSorter = Sorter;
                if (!(channelList.ListViewItemSorter is ListViewSorter))
                    return;

                Sorter = (ListViewSorter)channelList.ListViewItemSorter;
                if (Sorter.LastSort == e.Column)
                {
                    if (channelList.Sorting == SortOrder.Descending)
                        channelList.Sorting = SortOrder.Ascending;
                    else
                        channelList.Sorting = SortOrder.Descending;
                }
                else
                {
                    channelList.Sorting = SortOrder.Ascending;
                }
                Sorter.ByColumn = e.Column;

                channelList.Sort();

                channelList.EndUpdate();
            });
        }

        private void channelList_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in channelList.SelectedItems)
                _parent.ParseOutGoingCommand(this.connection, "/join " + eachItem.Text);
        }

        private void channelList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {                
                //show a popup menu
                ContextMenuStrip menu = new ContextMenuStrip();
                ToolStripMenuItem joinChannel = new System.Windows.Forms.ToolStripMenuItem();
                ToolStripMenuItem autoJoinChannel = new System.Windows.Forms.ToolStripMenuItem();
                
                menu.BackColor = SystemColors.Menu;
                joinChannel.ForeColor = SystemColors.MenuText;
                autoJoinChannel.ForeColor = SystemColors.MenuText;

                joinChannel.Text = "Join Channel";
                autoJoinChannel.Text = "Autojoin Channel";
                
                joinChannel.Size = new System.Drawing.Size(165, 22);
                autoJoinChannel.Size = new System.Drawing.Size(165, 22);

                joinChannel.Click += new EventHandler(joinChannel_Click);
                autoJoinChannel.Click += new EventHandler(autoJoinChannel_Click);
                menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    joinChannel,
                    autoJoinChannel});

                menu.Show(channelList, new Point(e.X, e.Y));
            }
        }

        private void autoJoinChannel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in channelList.SelectedItems)
                _parent.ParseOutGoingCommand(this.connection, "/autojoin " + eachItem.Text);
            
        }

        private void joinChannel_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in channelList.SelectedItems)
                _parent.ParseOutGoingCommand(this.connection, "/join " + eachItem.Text);            
        }


        internal void ScrollListWindow(bool scrollUp)
        {
            try
            {

                if (scrollUp)
                {
                    SendMessage(this.channelList.Handle, (uint)WM_VSCROLL, (System.UIntPtr)ScrollEventType.SmallDecrement, (System.IntPtr)0);
                }
                else
                {
                    SendMessage(this.channelList.Handle, (uint)WM_VSCROLL, (System.UIntPtr)ScrollEventType.SmallIncrement, (System.IntPtr)0);
                }
            }
            catch (Exception)
            {
            }
        }



        /// <summary>
        /// Create the Channel Window and items needed
        /// </summary>
        private void InitializeChannel()
        {
            this.panelTopic = new System.Windows.Forms.Panel();
            this.textTopic = new TextWindow();
            this.textWindow = new TextWindow();
            this.inputPanel = new InputPanel();
            this.panelTopic.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTopic
            // 
            this.panelTopic.Controls.Add(this.textTopic);
            
            this.panelTopic.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopic.Location = new System.Drawing.Point(0, 0);
            this.panelTopic.Name = "panelTopic";
            
            this.panelTopic.TabIndex = 1;
            this.panelTopic.Visible = false;
            // 
            // textTopic
            // 
            this.textTopic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textTopic.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textTopic.IRCBackColor = 0;
            this.textTopic.Location = new System.Drawing.Point(0, 0);
            this.textTopic.Name = "textTopic";
            this.textTopic.NoColorMode = false;
            this.textTopic.ShowTimeStamp = true;
            this.textTopic.SingleLine = true;
            this.textTopic.Size = new System.Drawing.Size(304, 22);
            this.textTopic.TabIndex = 0;
            // 
            // textWindow
            // 
            this.textWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textWindow.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textWindow.IRCBackColor = 0;
            this.textWindow.Location = new System.Drawing.Point(0, 22);
            this.textWindow.Name = "textWindow";
            this.textWindow.NoColorMode = false;
            this.textWindow.ShowTimeStamp = true;
            this.textWindow.SingleLine = false;
            this.textWindow.Size = new System.Drawing.Size(304, 166);
            this.textWindow.TabIndex = 0;

            this.topicSplitter = new Splitter();
            this.topicSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.topicSplitter.Size = new System.Drawing.Size(3, 3);
            this.topicSplitter.Dock = DockStyle.Top;
            this.topicSplitter.TabStop = false;
            this.topicSplitter.Visible = true;

            this.Controls.Add(this.textWindow);
            //this.Controls.Add(this.topicSplitter);
            this.Controls.Add(this.panelTopic);
            
            this.Size = new System.Drawing.Size(300, 180);
            this.panelTopic.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }

    public class ConsoleTab : TabPage
    {
        internal IRCConnection Connection;
        internal FormMain.ServerMessageType LastMessageType;
        
        public ConsoleTab(string serverName)
        {
            base.Text = serverName;
        }
    }

    //http://www.daniweb.com/forums/thread86620.html

    //flicker free listview for channel list/dcc file list
    internal class FlickerFreeListView : ListView
    {
        private ToolTip toolTip = new ToolTip();
        
        public FlickerFreeListView()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            
            toolTip.AutoPopDelay = 7000;
            toolTip.InitialDelay = 450;
            toolTip.ReshowDelay = 450;
        }

        protected override void OnNotifyMessage(Message m)
        {
            // filter WM_ERASEBKGND
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            ListViewItem item = this.GetItemAt(e.X, e.Y);
            ListViewHitTestInfo info = this.HitTest(e.X, e.Y);

            if ((item != null) && (info.SubItem != null))
            {
                toolTip.SetToolTip(this.Parent, info.Item.ToolTipText);
            }
            else
            {
                toolTip.SetToolTip(this.Parent, null);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pea)
        {
            // do nothing here since this event is now handled by OnPaint
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            base.OnPaint(pea);
        }


    }

    internal class ListViewSorter : System.Collections.IComparer
    {
        public int Compare(object o1, object o2)
        {
            if (!(o1 is ListViewItem))
                return (0);
            if (!(o2 is ListViewItem))
                return (0);

            ListViewItem lvi1 = (ListViewItem)o2;
            string str1 = lvi1.SubItems[ByColumn].Text;
            ListViewItem lvi2 = (ListViewItem)o1;
            string str2 = lvi2.SubItems[ByColumn].Text;

            int result;
            if (lvi1.ListView.Sorting == SortOrder.Ascending)
            {
                int r1;
                int r2;
                if (int.TryParse(str1, out r1) && int.TryParse(str2, out r2))
                {
                    //check if numeric
                    if (Convert.ToInt32(str1) > Convert.ToInt32(str2))
                        result = 1;
                    else
                        result = -1;
                }
                else
                    result = String.Compare(str1, str2);
            }
            else
            {
                int r3;
                int r4;
                if (int.TryParse(str1, out r3) && int.TryParse(str2, out r4))
                {
                    //check if numeric
                    if (Convert.ToInt32(str1) < Convert.ToInt32(str2))
                        result = 1;
                    else
                        result = -1;
                }
                else
                    result = String.Compare(str2, str1);
            }
            LastSort = ByColumn;

            return (result);
        }


        public int ByColumn
        {
            get { return Column; }
            set { Column = value; }
        }
        int Column = 0;

        public int LastSort
        {
            get { return LastColumn; }
            set { LastColumn = value; }
        }
        int LastColumn = 0;
    }
}
