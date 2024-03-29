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
 * GNU General Public LiceNnse for more details.
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;

using IceChatPlugin;


namespace IceChat
{
    public partial class TextWindow : UserControl, IDisposable
    {

        private int _totalLines;
        private int _totaldisplayLines;
        private int _oldWidth;
        private int _oldDisplayWidth; 

        private int _showMaxLines;
        private int _lineSize;

        private const char colorChar = '\x03';        // 02
        private const char underlineChar = '\x1F';    // 31
        private const char boldChar = '\x02';         // 02      
        private const char cancelChar = '\x0F';       // 15
        private const char reverseChar = '\x16';      // 22
        private const char italicChar = '\x1D';       // 29

        private const char newColorChar = '\xFF03';
        private const char emotChar = '\xFF0A';        

        private const char urlStart = '\xFF0B';
        private const char urlEnd = '\xFF0C';        

        private const char wrapLine = '\xFF0D';
        private const char endLine = '\xFF0F';
       
        private const char brokeChar = '\x84';


        private DisplayLine[] _displayLines;
        private TextLine[] _textLines;

        private int _backColor = 0;

        private bool _showTimeStamp = true;
        private bool _singleLine = false;
        private bool _noColorMode = false;
        private bool _noEmoticons = false;

        private ContextMenuStrip _popupMenu;
        private string _linkedWord = "";

        //works with www.
        //private string _wwwMatch = @"((www\.|www\d\.|(https?|ftp|irc):((//)|(\\\\)))+[\w\d:#@%/!;$()~_?\+-=\\\.&]*)";       

        //new one 9.01n
        //private string _wwwMatch = @"((www\.|www\d\.|(https?|ftp|irc|connect):((//)|(\\\\)))+[\w\d:#@%!;$~_?'\+-=\\\.&\[\]()]*)";
        
        // match the * character as well 9.08b
        private string _wwwMatch = @"((www\.|www\d\.|(https?|ftp|irc|connect|icechatplugin):((//)|(\\\\)))+[\w\d:#@%!;$~_?'\+-=\\\.\*&\[\]()]*)";



        //older one 9.01m
        //private string _wwwMatch = @"((www\.|www\d\.|(https?|ftp|irc|connect):((//)|(\\\\)))+[\w\d:#@%!;$~_?\+-=\\\.&\[\]]*)";
        //9.0
        //private string _wwwMatch = @"((www\.|www\d\.|(https?|ftp|irc):((//)|(\\\\)))+[\w\d:#@%!;$~_?\+-=\\\.&]*)";
        //private string _wwwMatch = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
        //private string _wwwMatch= @"~(?:\b[a-z\d.-]+://[^<>\s]+|\b(?:(?:(?:[^\s!@#$%^&*()_=+[\]{}\|;:,.<>/?]+)\.)+(?:ac|ad|aero|ae|af|ag|ai|al|am|an|ao|aq|arpa|ar|asia|as|at|au|aw|ax|az|ba|bb|bd|be|bf|bg|bh|biz|bi|bj|bm|bn|bo|br|bs|bt|bv|bw|by|bz|cat|ca|cc|cd|cf|cg|ch|ci|ck|cl|cm|cn|coop|com|co|cr|cu|cv|cx|cy|cz|de|dj|dk|dm|do|dz|ec|edu|ee|eg|er|es|et|eu|fi|fj|fk|fm|fo|fr|ga|gb|gd|ge|gf|gg|gh|gi|gl|gm|gn|gov|gp|gq|gr|gs|gt|gu|gw|gy|hk|hm|hn|hr|ht|hu|id|ie|il|im|info|int|in|io|iq|ir|is|it|je|jm|jobs|jo|jp|ke|kg|kh|ki|km|kn|kp|kr|kw|ky|kz|la|lb|lc|li|lk|lr|ls|lt|lu|lv|ly|ma|mc|md|me|mg|mh|mil|mk|ml|mm|mn|mobi|mo|mp|mq|mr|ms|mt|museum|mu|mv|mw|mx|my|mz|name|na|nc|net|ne|nf|ng|ni|nl|no|np|nr|nu|nz|om|org|pa|pe|pf|pg|ph|pk|pl|pm|pn|pro|pr|ps|pt|pw|py|qa|re|ro|rs|ru|rw|sa|sb|sc|sd|se|sg|sh|si|sj|sk|sl|sm|sn|so|sr|st|su|sv|sy|sz|tc|td|tel|tf|tg|th|tj|tk|tl|tm|tn|to|tp|travel|tr|tt|tv|tw|tz|ua|ug|uk|um|us|uy|uz|va|vc|ve|vg|vi|vn|vu|wf|ws|xn--0zwm56d|xn--11b5bs3a9aj6g|xn--80akhbyknj4f|xn--9t4b11yi5a|xn--deba0ad|xn--g6w251d|xn--hgbk6aj7f53bba|xn--hlcj6aya9esc7a|xn--jxalpdlp|xn--kgbechtv|xn--zckzah|ye|yt|yu|za|zm|zw)|(?:(?:[0-9]|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])\.){3}(?:[0-9]|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5]))(?:[;/][^#?<>\s]*)?(?:\?[^#<>\s]*)?(?:#[^<>\s]*)?(?!\w))~iS";
        
        private string _emotMatch = "";
        private int _startHighLine = -1;
        private int _curHighLine;       
        private int _curHighChar;

        private List<int> _selectedLines;
        private int _startX;
        private int _startY;
            
        public struct TextLine
        {
            public string line;
            public float width;
            public int totalLines;
            public int textColor;
        }

        private struct DisplayLine
        {
            public string line;
            public int textLine;
            public bool wrapped;
            public bool previous;
            public int textColor;
            public int lineHeight;
            public int startSelection;
            public int endSelection;
            public int selectionX1;
            public int selectionX2;
        }

        private delegate void ScrollValueDelegate(int value);
        private delegate void ScrollBottomDelegate();

        private int _maxTextLines = 500;

        private Logging _logClass;

        private int _unreadMarker;  // Unread marker
        private bool _unreadReset; // Unread marker 
        
        private bool _reloadText;
        private bool _reformatLines;

        private Bitmap _backgroundImage = null;
        private string _backgroundImageFile;
        private StringFormat stringFormat;

        private Bitmap _buffer = null;
        
        // add a tooltip, for the topic only
        private ToolTip toolTip = null;
        private string toolTipString;

        //private Object thisLock = new Object();

        public TextWindow()
        {
            InitializeComponent();

            stringFormat = StringFormat.GenericTypographic;
            stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.FontChanged += new EventHandler(OnFontChanged);

            this.vScrollBar.Scroll += new ScrollEventHandler(OnScroll);
            this.DoubleClick += new EventHandler(OnDoubleClick);
            
            this.BorderStyle = BorderStyle.Fixed3D;

            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            this.Resize += new EventHandler(OnResize);

            if (FormMain.Instance != null && FormMain.Instance.IceChatOptions != null)
                this._maxTextLines = FormMain.Instance.IceChatOptions.MaximumTextLines;
            
            _displayLines = new DisplayLine[_maxTextLines * 5];
            _textLines = new TextLine[_maxTextLines];
            
            _oldWidth = this.Width;
            _oldDisplayWidth = this.ClientRectangle.Width - vScrollBar.Width - 10;

            _selectedLines = new List<int>();
            _reloadText = false;

            LoadTextSizes();

            if (FormMain.Instance != null && FormMain.Instance.IceChatEmoticons != null)
            {
                if (FormMain.Instance.IceChatEmoticons.listEmoticons.Count > 0)
                {
                    foreach (EmoticonItem emot in FormMain.Instance.IceChatEmoticons.listEmoticons)
                    {
                        _emotMatch += emot.Trigger + ((char)0);
                    }
                    _emotMatch = _emotMatch.Substring(0, _emotMatch.Length - 1);
                }
            }

            _popupMenu = new ContextMenuStrip
            {
                RenderMode = ToolStripRenderMode.ManagerRenderMode,
                Renderer = new EasyRenderer.EasyRender()
            };

            this.MouseWheel += new MouseEventHandler(TextWindow_MouseWheel);

        }

        private void TextWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            this.ScrollWindow(e.Delta > 0);
        }

        private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //get the current character the mouse is over. 
            _startHighLine = ((this.Height + (_lineSize / 2)) - e.Y) / _lineSize;
            _startHighLine = _totaldisplayLines - _startHighLine;
            _startHighLine = (_startHighLine - (_totaldisplayLines - vScrollBar.Value));

            if (_startHighLine > -1)
            {
                int _startHighChar = ReturnChar(_startHighLine, e.X);
                //string line                 
                _displayLines[_startHighLine].startSelection = _startHighChar;
            }

            _startX = e.X;
            _startY = e.Y;

            //what kind of a popupmenu do we want?
            string popupType = "";
            string windowName = "";
            string _linkedWordNick = StripString(_linkedWord);

            if (e.Button == MouseButtons.Left)
            {
                //first need to see what word we clicked, if not, run a command
                if (_linkedWord.Length > 0)
                {
                    //check if it is a URL
                    Regex re = new Regex(_wwwMatch);
                    MatchCollection matches = re.Matches(_linkedWord);

                    String clickedWord = _linkedWord;
                    if (matches.Count > 0)
                    {
                        clickedWord = matches[0].Value;
                    }
                    
                    if (matches.Count > 0 && !clickedWord.StartsWith("irc://") && !clickedWord.StartsWith("connect://") && !clickedWord.StartsWith("icechatplugin://"))
                    {
                        try
                        {
                            if (clickedWord.ToLower().StartsWith("www"))
                                clickedWord = "http://" + clickedWord;

                            // System.Diagnostics.Debug.WriteLine("ClickedWord:" + clickedWord + ":" + _linkedWord);

                            string channel = "";
                            IRCConnection connection = null;
                            if (this.Parent.GetType() == typeof(IceTabPage))
                            {
                                IceTabPage t = (IceTabPage)this.Parent;
                                channel = t.TabCaption;
                                connection = t.Connection;
                            }
                            else if (this.Parent.GetType() == typeof(ConsoleTab))
                            {
                                ConsoleTab c = (ConsoleTab)this.Parent;
                                connection = c.Connection;
                            }

                            PluginArgs args = new PluginArgs(this, channel, "", "", "")
                            {
                                Connection = connection,
                                Extra = clickedWord
                            };

                            foreach (Plugin p in  FormMain.Instance.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                        args = ipc.plugin.LinkClicked(args);
                                }
                            }
                            
                            //the clicked link can be changed, or removed
                            
                            if (args.Extra.Length > 0)
                                System.Diagnostics.Process.Start(args.Extra);
                            

                        }
                        catch (Exception)
                        {
                        }
                        return;
                    }
                    
                    if (clickedWord.StartsWith("irc://"))
                    {
                        //check if a channel was specified
                        string server = clickedWord.Substring(6).TrimEnd();
                        if (server.IndexOf("/") != -1)
                        {
                            string host = server.Split('/')[0];
                            string channel = server.Split('/')[1];
                            if (channel.StartsWith("#"))
                                FormMain.Instance.ParseOutGoingCommand(null, "/joinserv " + host + " " + channel);
                            else
                                FormMain.Instance.ParseOutGoingCommand(null, "/joinserv " + host + " #" + channel);

                        }
                        else
                            FormMain.Instance.ParseOutGoingCommand(null, "/server " + clickedWord.Substring(6).TrimEnd());
                        return;
                    }                    
                    else if (clickedWord.StartsWith("icechatplugin://"))
                    {
                        //download an icechat plugin
                        System.Diagnostics.Process.Start(clickedWord);

                    }
                    else if (clickedWord.StartsWith("connect://"))
                    {
                        //check if a channel was specified , by the network name
                        //server://network:irc.server.name:6667/#icechat
                        string server = clickedWord.Substring(10).TrimEnd();
                        if (server.IndexOf("/") != -1)
                        {
                            string[] host = server.Split('/')[0].Split(':');
                            string channel = server.Split('/')[1];
                            if (host.Length == 3)
                            {
                                //has to be 3 values
                                //network
                                //server
                                //port

                                //check if we are connected to this network
                                bool match = false;
                                foreach (IRCConnection c in FormMain.Instance.ServerTree.ServerConnections.Values)
                                {
                                    if (c.IsConnected)
                                    {
                                        if (c.ServerSetting.NetworkName.ToLower() == host[0].ToLower())
                                        {
                                            //network match
                                            //ask if we want to use this server, or a new connection
                                            if (channel.StartsWith("#"))
                                                FormMain.Instance.ParseOutGoingCommand(c, "/join " + channel);
                                            else
                                                FormMain.Instance.ParseOutGoingCommand(c, "/join #" + channel);
                                            
                                            match = true;
                                        }
                                    }
                                }
                                if (!match)
                                {
                                    //no match found
                                    if (channel.StartsWith("#"))
                                        FormMain.Instance.ParseOutGoingCommand(null, "/joinserv " + host[1]+":"+host[2] + " " + channel);
                                    else
                                        FormMain.Instance.ParseOutGoingCommand(null, "/joinserv " + host[1]+":"+host[2] + " #" + channel);

                                }
                            }

                        }
                        return;
                    }
                }
            }

            if (this.Parent.GetType() == typeof(IceTabPage))
            {
                IceTabPage t = (IceTabPage)this.Parent;
                if (t.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    //check if over a nick name
                    foreach (User u in t.Nicks.Values)
                    {
                        if (u.NickName == _linkedWordNick)
                        {
                            popupType = "NickList";
                            //highlight the nick in the nick list
                            if (e.Button == MouseButtons.Left)
                                FormMain.Instance.NickList.SelectNick(_linkedWordNick);
                            break;
                        }
                        else if (u.NickName == _linkedWord)
                        {
                            popupType = "NickList";
                            _linkedWordNick = _linkedWord;
                            //highlight the nick in the nick list
                            if (e.Button == MouseButtons.Left)
                                FormMain.Instance.NickList.SelectNick(_linkedWordNick);
                            break;
                        }
                    }
                    if (popupType.Length == 0)
                        popupType = "Channel";
                }
                if (t.WindowStyle == IceTabPage.WindowType.Query)
                    popupType = "Query";

                windowName = t.TabCaption;
            }
            if (this.Parent.GetType() == typeof(ConsoleTab))
            {
                ConsoleTab c = (ConsoleTab)this.Parent;
                popupType = "Console";

                if (c.Connection != null)
                {
                    if (c.Connection.ServerSetting.RealServerName.Length > 0)
                        windowName = c.Connection.ServerSetting.RealServerName;
                    else
                        windowName = c.Connection.ServerSetting.ServerName;
                }
            }

            if (e.Button == MouseButtons.XButton1)  //left button
            {
                //go to previous window
                int prevIndex = FormMain.Instance.ChannelBar.SelectedIndex == 0 ? FormMain.Instance.ChannelBar.TabCount - 1 : FormMain.Instance.ChannelBar.SelectedIndex - 1;
                FormMain.Instance.ChannelBar.SelectTab(FormMain.Instance.ChannelBar.TabPages[prevIndex]);
                FormMain.Instance.ServerTree.SelectTab(FormMain.Instance.ChannelBar.TabPages[prevIndex], false);
                return;
            }
            if (e.Button == MouseButtons.XButton2)
            {
                int nextIndex = FormMain.Instance.ChannelBar.TabCount == FormMain.Instance.ChannelBar.SelectedIndex + 1 ? 0 : FormMain.Instance.ChannelBar.SelectedIndex + 1;
                FormMain.Instance.ChannelBar.SelectTab(FormMain.Instance.ChannelBar.TabPages[nextIndex]);
                FormMain.Instance.ServerTree.SelectTab(FormMain.Instance.ChannelBar.TabPages[nextIndex], false);
                return;
            }

            if (e.Button == MouseButtons.Middle && _linkedWord.Length > 0)
            {
                //check if this is a link. if so, save it to the clipboard
                Regex re = new Regex(_wwwMatch);
                MatchCollection matches = re.Matches(_linkedWord);
                if (matches.Count > 0)
                {
                    try
                    {
                        Clipboard.SetText(_linkedWord);
                    }
                    catch (Exception) { }
                    return;
                }
                
                return;
            }
            
            if (e.Button == MouseButtons.Right && popupType.Length > 0)
            {

                System.Diagnostics.Debug.WriteLine(_linkedWord);
                // have a popup showing for links?
                Regex re = new Regex(_wwwMatch);
                MatchCollection matches = re.Matches(_linkedWord);
                if (matches.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine("URL");

                }

                //show the popup menu
                foreach (PopupMenuItem p in FormMain.Instance.IceChatPopupMenus.listPopups)
                {
                    if (p.PopupType == popupType)
                    {
                        string[] menuItems = p.Menu;                        
                        List<string> list = new List<string>();
                        for (int i = 0; i < menuItems.Length; i++)
                        {
                            if (menuItems[i] != "")
                                list.Add(menuItems[i]);
                        }
                        menuItems = list.ToArray();
                        
                        //build the menu
                        ToolStripItem t;
                        int subMenu = 0;

                        _popupMenu.Items.Clear();
                        //_popupMenu.Items.Add(new toolstr
                        foreach (string menu in menuItems)
                        {
                            string caption;
                            string command;
                            string menuItem = menu;
                            int menuDepth = 0;

                            //get the menu depth
                            while (menuItem.StartsWith("."))
                            {
                                menuItem = menuItem.Substring(1);
                                menuDepth++;
                            }

                            if (menu.IndexOf(':') > 0)
                            {
                                caption = menuItem.Substring(0, menuItem.IndexOf(':'));
                                command = menuItem.Substring(menuItem.IndexOf(':') + 1);
                            }
                            else
                            {
                                caption = menuItem;
                                command = "";
                            }

                            if (caption.Length > 0)
                            {
                                if (popupType == "Channel")
                                {
                                    caption = caption.Replace("$chan", windowName);
                                    command = command.Replace("$chan", windowName);

                                    caption = caption.Replace(" # ", " " + windowName + " ");
                                    command = command.Replace(" # ", " " + windowName + " ");
                                }
                                else if (popupType == "Query")
                                {
                                    caption = caption.Replace("$nick", windowName);
                                    command = command.Replace("$nick", windowName);
                                }
                                else if (popupType == "NickList")
                                {
                                    caption = caption.Replace("$nick", _linkedWordNick);
                                    command = command.Replace("$nick", _linkedWordNick);
                                    caption = caption.Replace("$chan", windowName);
                                    command = command.Replace("$chan", windowName);
                                }
                                else if (popupType == "Console")
                                {
                                    caption = caption.Replace("$server", windowName);
                                    command = command.Replace("$server", windowName);
                                }

                                if (caption == "-")
                                    t = new ToolStripSeparator();
                                else
                                {
                                    t = new ToolStripMenuItem(caption)
                                    {
                                        ForeColor = SystemColors.MenuText,
                                        BackColor = SystemColors.Menu
                                    };

                                    //parse out the command/$identifiers                            
                                    if (popupType == "NickList")
                                        command = command.Replace("$1", _linkedWordNick);
                                    else
                                        command = command.Replace("$1", windowName);

                                    t.Click += new EventHandler(OnPopupMenuClick);
                                    t.Tag = command;
                                }

                                if (menuDepth == 0)
                                    subMenu = _popupMenu.Items.Add(t);
                                else
                                {
                                    //do not allow submenu items for a toolstrip seperator
                                    if (_popupMenu.Items[subMenu].GetType() != typeof(ToolStripSeparator))
                                        ((ToolStripMenuItem)_popupMenu.Items[subMenu]).DropDownItems.Add(t);
                                }
                                t = null;
                            }
                        }

                        if (FormMain.Instance != null && FormMain.Instance.LoadedPlugins != null)
                        {
                            foreach (Plugin pl in FormMain.Instance.LoadedPlugins)
                            {
                                IceChatPlugin ipc = pl as IceChatPlugin;
                                if (pl != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                    {
                                        if (popupType == "Channel")
                                        {
                                            ToolStripItem[] popChan = ipc.plugin.AddChannelPopups();
                                            if (popChan != null && popChan.Length > 0)
                                            {
                                                _popupMenu.Items.AddRange(popChan);
                                            }
                                        }
                                        else if (popupType == "Query")
                                        {
                                            ToolStripItem[] popChan = ipc.plugin.AddQueryPopups();
                                            if (popChan != null && popChan.Length > 0)
                                            {
                                                _popupMenu.Items.AddRange(popChan);
                                            }

                                        }
                                        else if (popupType == "Console")
                                        {
                                            ToolStripItem[] popChan = ipc.plugin.AddServerPopups();
                                            if (popChan != null && popChan.Length > 0)
                                            {
                                                _popupMenu.Items.AddRange(popChan);
                                            }

                                        }
                                    }
                                }
                            }
                        }


                        _popupMenu.Show(this, e.Location);
                    }
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (_startX != e.X)
                {
                    if (_startHighLine > -1)
                    {
                        //copy the selected text to the clipboard
                        if (_selectedLines.Count == 0)
                        {
                            //only copied from a single line
                            string line = StripAllCodes(_displayLines[_startHighLine].line, true);
                            
                            if (_displayLines[_startHighLine].selectionX2 > line.Length)                            
                                line = line.Substring(_displayLines[_startHighLine].selectionX1);
                            else
                                line = line.Substring(_displayLines[_startHighLine].selectionX1, _displayLines[_startHighLine].selectionX2 - _displayLines[_startHighLine].selectionX1);
                            if (line.Length > 0)
                            {
                                //System.Diagnostics.Debug.WriteLine(line);
                                //if control key is down.. dont copy to clipboard
                                if ( (Control.ModifierKeys & Keys.Control) == Keys.Control )
                                {
                                   //control key is down -- dont copy to Clipboard
                                }    
                                else
                                    Clipboard.SetText(line);
                            }

                        }
                        else
                        {
                            if (_selectedLines[0] > _startHighLine)
                            {
                                //selected from the top and down
                                string line = StripAllCodes(_displayLines[_startHighLine].line.Substring(_displayLines[_startHighLine].selectionX1), false); ;

                                int lastLine = _displayLines[_startHighLine].textLine;
                                
                                //System.Diagnostics.Debug.WriteLine("start:" + _startHighLine + ":" + _selectedLines[_selectedLines.Count - 1]);
                                
                                int i = 0;
                                for (i = _startHighLine + 1; i < _selectedLines[_selectedLines.Count - 1]; i++)
                                {
                                    if (_displayLines[i].textLine == lastLine)
                                    {
                                        if (_displayLines[i].wrapped == true)
                                        {
                                            line += StripAllCodes(_displayLines[i].line, false);
                                        }
                                        else
                                        {
                                            line += " " + StripAllCodes(_displayLines[i].line, false);
                                        }
                                    }
                                    else
                                    {
                                        line += "\r\n" + StripAllCodes(_displayLines[i].line, false);
                                        lastLine = _displayLines[i].textLine;
                                    }
                                }
                                //System.Diagnostics.Debug.WriteLine(_displayLines[i].selectionX2 + ":" + _displayLines[i].line);
                                //add the last partial line
                                if (_displayLines[i].selectionX2 > 0)
                                {
                                    if (_displayLines[i].textLine == lastLine)
                                    {
                                        if (_displayLines[i].wrapped == true)
                                        {
                                            line += StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                        else
                                        {
                                            line += " " + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                    }
                                    else
                                    {
                                        line += "\r\n" + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                    }

                                }
                                
                                //System.Diagnostics.Debug.WriteLine("copy1:" + line + ":");                            
                                
                                if (line.Length > 0)
                                {
                                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                                    {
                                        //control key is down -- dopnt copy to Clipboard
                                    }
                                    else
                                        Clipboard.SetText(line);

                                    //Clipboard.SetText(line);
                                }

                            }
                            else
                            {
                                //selected from the bottom and up
                                string line = StripAllCodes(_displayLines[_selectedLines[0]].line, false);
                                line = line.Substring(_displayLines[_selectedLines[0]].selectionX1);

                                int lastLine = _displayLines[_selectedLines[0]].textLine;

                                //System.Diagnostics.Debug.WriteLine("start:" + lastLine + ":" + line);

                                int i = 0;
                                for (i = _selectedLines[0] + 1; i < _startHighLine; i++)
                                {
                                    if (_displayLines[i].textLine == lastLine)
                                    {
                                        if (_displayLines[i].wrapped == true)
                                        {
                                            line += StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                        else
                                        {
                                            line += " " + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                    }
                                    else
                                    {
                                        line += "\r\n" + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                    }

                                }

                                if (_displayLines[i].selectionX2 > 0)
                                {
                                    if (_displayLines[i].textLine == lastLine)
                                    {
                                        if (_displayLines[i].wrapped == true)
                                        {
                                            line += StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                        else
                                        {
                                            line += " " + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                        }
                                    }
                                    else
                                    {
                                        line += "\r\n" + StripAllCodes(_displayLines[i].line, false).Substring(0, _displayLines[i].selectionX2);
                                    }

                                }
                                System.Diagnostics.Debug.WriteLine("copy2:" + line + ":");
                                if (line.Length > 0)
                                {
                                    if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                                    {
                                        //control key is down -- dopnt copy to Clipboard
                                    }
                                    else
                                        Clipboard.SetText(line);

                                    //Clipboard.SetText(line);
                                }

                            }
                        }

                        _displayLines[_startHighLine].selectionX1 = 0;
                        _displayLines[_startHighLine].selectionX2 = 0;

                        for (int i = _selectedLines.Count - 1; i > 0; i--)
                        {
                            _displayLines[_selectedLines[i]].selectionX1 = 0;
                            _displayLines[_selectedLines[i]].selectionX2 = 0;
                        }
                        //remove all the selection lines
                        _selectedLines.Clear();

                    }

                    Invalidate();
                }
            }
            catch(Exception ex)
            {
                //do nada
                System.Diagnostics.Debug.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
            
            FormMain.Instance.FocusInputBox();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            //get the current line mouse is over
            try
            {

                int line = 0;

                if (e.X == _startX && e.Y == _startY) return;

                if (_singleLine && _totaldisplayLines > 1)
                {
                    
                    if (toolTip.GetToolTip(this) != toolTipString)
                        toolTip.SetToolTip(this, toolTipString);
                    
                }


                if (e.Button == MouseButtons.Left)
                {
                    //get the current character the mouse is over. 
                    _curHighLine = ((this.Height + (_lineSize / 2)) - e.Y) / _lineSize;
                    _curHighLine = _totaldisplayLines - _curHighLine;
                    _curHighLine = (_curHighLine - (_totaldisplayLines - vScrollBar.Value));

                    //clear the selected Lines
                    for (int i = _selectedLines.Count - 1; i > 0; i--)
                    {
                        _displayLines[_selectedLines[i]].selectionX1 = 0;
                        _displayLines[_selectedLines[i]].selectionX2 = 0;
                        _selectedLines.RemoveAt(i);
                    }

                    if (_startHighLine < 0) return;
                    if (_curHighLine < 0) return;

                    if (_startHighLine == _curHighLine)
                    {
                        //highlite on same line
                        _curHighChar = ReturnChar(_curHighLine, e.X);

                        _displayLines[_curHighLine].endSelection = _curHighChar;

                        if (_displayLines[_curHighLine].startSelection > _displayLines[_curHighLine].endSelection)
                        {
                            _displayLines[_curHighLine].selectionX1 = _displayLines[_curHighLine].endSelection;
                            _displayLines[_curHighLine].selectionX2 = _displayLines[_curHighLine].startSelection;
                        }
                        else
                        {
                            _displayLines[_curHighLine].selectionX1 = _displayLines[_curHighLine].startSelection;
                            _displayLines[_curHighLine].selectionX2 = _displayLines[_curHighLine].endSelection;
                        }

                        //clear all selected lines
                        _selectedLines.Clear();

                    }
                    else if (_curHighLine > _startHighLine)
                    {
                        _curHighChar = ReturnChar(_curHighLine, e.X);

                        _displayLines[_startHighLine].selectionX2 = StripAllCodes(_displayLines[_startHighLine].line, false).Length;

                        _displayLines[_curHighLine].selectionX2 = _curHighChar;
                        _displayLines[_curHighLine].selectionX1 = 0;

                        for (int i = _startHighLine + 1; i < _curHighLine; i++)
                        {
                            _displayLines[i].selectionX1 = 0;
                            _displayLines[i].selectionX2 = StripAllCodes(_displayLines[i].line, false).Length;
                        }

                        for (int i = _startHighLine + 1; i <= _curHighLine; i++)
                        {
                            _selectedLines.Add(i);
                        }

                    }
                    else
                    {
                        _curHighChar = ReturnChar(_curHighLine, e.X);

                        _displayLines[_startHighLine].selectionX1 = 0;

                        _displayLines[_curHighLine].selectionX1 = _curHighChar;
                        _displayLines[_curHighLine].selectionX2 = StripAllCodes(_displayLines[_curHighLine].line, false).Length;

                        for (int i = _curHighLine + 1; i < _startHighLine; i++)
                        {
                            _displayLines[i].selectionX1 = 0;
                            _displayLines[i].selectionX2 = StripAllCodes(_displayLines[i].line, false).Length;
                        }

                        for (int i = _curHighLine; i < _startHighLine; i++)
                        {
                            _selectedLines.Add(i);
                        }

                    }

                    _selectedLines.Sort();

                    if (_startHighLine > -1)
                        Invalidate();

                }

                if (e.Button == MouseButtons.None)
                {
                    line = ((this.Height + (_lineSize / 2)) - e.Y) / _lineSize;

                    // Then, convert it to count from the top. 
                    line = vScrollBar.Value - line;

                    _linkedWord = ReturnWord(line, e.Location.X).Trim();

                    if (_linkedWord.Length > 0)
                    {
                        Regex re = new Regex(_wwwMatch);
                        MatchCollection matches = re.Matches(_linkedWord);
                        if (matches.Count > 0)
                        {
                            if (this.Cursor != Cursors.Hand)
                                this.Cursor = Cursors.Hand;
                            return;
                        }
                        else if (this.Parent.GetType() == typeof(IceTabPage))
                        {
                            IceTabPage t = (IceTabPage)this.Parent;
                            if (t.WindowStyle != IceTabPage.WindowType.Debug && t.WindowStyle != IceTabPage.WindowType.Window && t.WindowStyle != IceTabPage.WindowType.ChannelList)
                            {
                                //check if we are over a channel name
                                string chan = _linkedWord;
                                if (t.Connection.ServerSetting.StatusModes != null)
                                    //for (int i = 0; i < t.Connection.ServerSetting.StatusModes[1].Length; i++)
                                    //    chan = chan.Replace(t.Connection.ServerSetting.StatusModes[1][i].ToString(), string.Empty);
                                    for (int i = 0; i < t.Connection.ServerSetting.StatusModes[1].Length; i++)
                                        if (chan.StartsWith(t.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                            chan = chan.Substring(1);

                                if (chan.Length > 0 && t.Connection.ServerSetting.ChannelTypes != null && Array.IndexOf(t.Connection.ServerSetting.ChannelTypes, chan[0]) != -1)
                                {
                                    if (this.Cursor != Cursors.Hand)
                                        this.Cursor = Cursors.Hand;
                                    return;
                                }
                                string _linkedWordNick = StripString(_linkedWord);

                                //check if over a nick name
                                if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    foreach (User u in t.Nicks.Values)
                                    {
                                        if (u.NickName == _linkedWordNick)
                                        {
                                            if (this.Cursor != Cursors.Hand)
                                                this.Cursor = Cursors.Hand;
                                            return;
                                        }
                                        else if (u.NickName == _linkedWord)
                                        {
                                            if (this.Cursor != Cursors.Hand)
                                                this.Cursor = Cursors.Hand;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        else if (this.Parent.GetType() == typeof(ConsoleTab))
                        {
                            ConsoleTab c = (ConsoleTab)this.Parent;
                            if (c.Connection != null)
                            {
                                //check if we are over a channel name
                                if (c.Connection.IsFullyConnected)
                                {
                                    string chan = _linkedWord;
                                    if (c.Connection.ServerSetting.StatusModes != null)
                                        //for (int i = 0; i < c.Connection.ServerSetting.StatusModes[1].Length; i++)
                                        //    chan = chan.Replace(c.Connection.ServerSetting.StatusModes[1][i].ToString(), string.Empty);
                                        for (int i = 0; i < c.Connection.ServerSetting.StatusModes[1].Length; i++)
                                            if (chan.StartsWith(c.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                                chan = chan.Substring(1);

                                    if (chan.Length > 0 && c.Connection.ServerSetting.ChannelTypes != null && Array.IndexOf(c.Connection.ServerSetting.ChannelTypes, chan[0]) != -1)
                                    {
                                        if (this.Cursor != Cursors.Hand)
                                            this.Cursor = Cursors.Hand;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default; 
                System.Diagnostics.Debug.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
        }

        private string ReturnWord(int lineNumber, int locationX)
        {            
            try
            {
                if (lineNumber < _totaldisplayLines && lineNumber >= 0)
                {
                    Graphics g = this.CreateGraphics();
                    g.InterpolationMode = InterpolationMode.Low;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                    string line = StripAllCodes(_displayLines[lineNumber].line, false);
                    float width = MeasureString(line, g , false);
                    
                    if (locationX > width)
                        return "";

                    int space = 0;
                    bool foundSpace = false;
                    float lookWidth = 0;
                    string match = "";

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == (char)32)
                        {
                            if (!foundSpace)
                            {
                                if (lookWidth >= locationX)
                                {
                                    //System.Diagnostics.Debug.WriteLine("found space:" + i + ":" + space);
                                    if (space == 0)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(line.Substring(0, i));
                                        match = line.Substring(0, i);
                                    }
                                    else
                                    {
                                        //System.Diagnostics.Debug.WriteLine(line.Substring(space,i-space) + ":");
                                        match = line.Substring(space, i - space);
                                    }
                                    foundSpace = true;
                                    break;
                                }
                                else
                                    space = i + 1;
                            }
                        }

                        lookWidth += (float)MeasureString(line[i].ToString(), g, false);
                                          
                    }

                    // System.Diagnostics.Debug.WriteLine("end:" + foundSpace + ":" + space + ":" + line.Length + ":" + (int)line[line.Length-1] + "::" + match);
                    //char \\FF0D means the line was broken properly, if its not, it was line-wrapped

                    //end:False:0:40:103::
                    //linked:6:aces-shortened-links-with-the-real-thing

                    if (match.Length > 0)
                    {
                        //System.Diagnostics.Debug.WriteLine("match:" + match);
                        return match;
                    }
                    else if (space == 0)
                    {
                        if (_singleLine)
                        {
                            // we are only a single line, return it
                            return line;
                        }
                        else
                        {
                            //check if this is a wrap from a previous line
                            if (_displayLines[lineNumber - 1].textLine == _displayLines[lineNumber].textLine)
                            {
                                //check if it ends with a wrapLine char
                                string prevLine = StripAllCodes(_displayLines[lineNumber - 1].line, true);
                                if (prevLine.EndsWith(wrapLine.ToString()))
                                {
                                    //this line is wrapped - get the last word of prev line
                                    //if there is no space, ERROR
                                    if (lineNumber > 0)
                                    {
                                        string extra = "";
                                        int currentLine = _displayLines[lineNumber].textLine;
                                        while (lineNumber > 0)
                                        {
                                            lineNumber--;
                                            if (_displayLines[lineNumber].textLine != currentLine)
                                                break;

                                            extra += StripAllCodes(_displayLines[lineNumber].line, true);
                                            if (extra.IndexOf(' ') > -1)
                                            {
                                                extra = extra.Substring(0, extra.IndexOf(' '));
                                                break;
                                            }
                                        }
                                        return extra + line.Substring(space);
                                    }
                                }
                                else
                                {
                                    if (line.EndsWith(wrapLine.ToString()))
                                        return line;
                                    else
                                    {
                                        //check next line(s)
                                        if (lineNumber < _totaldisplayLines)
                                        {
                                            string extra = "";
                                            int currentLine = _displayLines[lineNumber].textLine;

                                            while (lineNumber < _totaldisplayLines)
                                            {
                                                lineNumber++;
                                                if (_displayLines[lineNumber].textLine != currentLine)
                                                    break;

                                                extra += StripAllCodes(_displayLines[lineNumber].line, true);
                                                if (extra.IndexOf(' ') > -1)
                                                {
                                                    extra = extra.Substring(0, extra.IndexOf(' '));
                                                    break;
                                                }
                                            }
                                            return line.Substring(space) + extra;
                                        }
                                        else
                                        {
                                            return line.Substring(space);
                                        }

                                    }
                                }
                            }
                            else
                                return line;
                        }
                    }
                    else if (space > 0)
                    {
                        if (foundSpace == false && !line.EndsWith(wrapLine.ToString()))
                        {
                            //go to the next line and get word until 1st space is found
                            //check if we have a next line 
                            //System.Diagnostics.Debug.WriteLine(_displayLines[lineNumber + 1].textLine + ":" + _displayLines[lineNumber].textLine);
                            if (_displayLines[lineNumber + 1].textLine == _displayLines[lineNumber].textLine)
                            {
                                //find 1st space on next line
                                //wrap to the next line
                                if (lineNumber < _totaldisplayLines)
                                {
                                    string extra = "";
                                    int currentLine = _displayLines[lineNumber].textLine;

                                    while (lineNumber < _totaldisplayLines)
                                    {
                                        lineNumber++;
                                        if (_displayLines[lineNumber].textLine != currentLine)
                                            break;

                                        extra += StripAllCodes(_displayLines[lineNumber].line, true);
                                        if (extra.IndexOf(' ') > -1)
                                        {
                                            extra = extra.Substring(0, extra.IndexOf(' '));
                                            break;
                                        }
                                    }
                                    return line.Substring(space) + extra;
                                }
                            }
                            else
                            {
                                return line.Substring(space);
                            }
                        }
                        else if (foundSpace == false && (int)line[line.Length - 1] == wrapLine)
                        {
                            //remove wrap line char
                            string extra = line.Substring(space);
                            return extra.Substring(0, extra.Length - 1);
                        }
                        else
                        {
                            return line.Substring(space);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
            }
            return "";
        }

        private int ReturnChar(int lineNumber, int x)
        {
            try
            {
                if (lineNumber < _totaldisplayLines && lineNumber >= 0)
                {
                    Graphics g = this.CreateGraphics();
                    g.InterpolationMode = InterpolationMode.Low;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                    //string line = StripAllCodes(_displayLines[lineNumber].line, false);
                    //string line = StripColorCodes(_displayLines[lineNumber].line);
                    
                    string line = _displayLines[lineNumber].line;
                    float width = MeasureString(line, g, false);

                   // System.Diagnostics.Debug.WriteLine("line1:" + line.Length + ":" + x + ":" + width);

                    if (x > width)
                    {
                        //System.Diagnostics.Debug.WriteLine("x > width:" + line.Length);
                        //we need to remove the length of stuff
                        //System.Diagnostics.Debug.WriteLine(line);
                        //System.Diagnostics.Debug.WriteLine(StripAllCodes(line, false));
                        //System.Diagnostics.Debug.WriteLine(StripAllCodes(line, false).Length);
                        return StripAllCodes(line, false).Length;
                        //return line.Length;
                    }
                    
                    float lookWidth = 0;

                    int controlChars = 0;
                    bool isBold = false;

                    for (int i = 0; i < line.Length; i++)
                    {
                        //System.Diagnostics.Debug.WriteLine(line[i] + ":" + i + ":" + (int)line[i] + ":" + lookWidth);
                        if (line[i] == boldChar || line[i] == cancelChar || line[i] == italicChar || line[i] == underlineChar || line[i] == urlStart || line[i] == urlEnd || line[i] == endLine || line[i] == wrapLine)
                        {
                            //System.Diagnostics.Debug.WriteLine("control char:" + (int)line[i]);
                            
                            controlChars++;

                            if (line[i] == boldChar || isBold)
                                isBold = !isBold;
                        }
                        else if (line[i] == newColorChar)
                        {
                            controlChars = controlChars + 5;
                            i = i + 4;
                            //System.Diagnostics.Debug.WriteLine("new color char:" + i + ":" + lookWidth);
                        }
                        else
                            lookWidth += (float)MeasureString(line[i].ToString(), g, isBold);

                        if (lookWidth >= x)
                        {
                            //int w = StripAllCodes(line, true).Length;
                            //System.Diagnostics.Debug.WriteLine("return:" + (i - controlChars) + ":" + line[i] + ":" + w);
                            //if ((i - controlChars) > w)
                            //    return w;
                            //System.Diagnostics.Debug.WriteLine(StripAllCodes(line,true).Substring(i-controlChars));
                            //else
                            g.Dispose();
                            return i - controlChars;
                        }

                    }
                    g.Dispose();

                    return line.Length;
                }

                //return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ":" + ex.StackTrace);
            }
            return 0;
        }


        private void OnPopupMenuClick(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null) return;

            string command = ((ToolStripMenuItem)sender).Tag.ToString();
            if (command.Length < 2) return;
            
            if (command.Substring(0, 2) != "//")
                command = "/" + command;

            System.Diagnostics.Debug.WriteLine("NickPopupWindow:" + command);

            if (this.Parent.GetType() == typeof(ConsoleTab))
            {
                ConsoleTab c = (ConsoleTab)this.Parent;
                FormMain.Instance.ParseOutGoingCommand(c.Connection, command);
            }
            else if (this.Parent.GetType() == typeof(IceTabPage))
            {
                IceTabPage t = (IceTabPage)this.Parent;
                FormMain.Instance.ParseOutGoingCommand(t.Connection, command);
            }

        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = e as MouseEventArgs;

            if (me.Button == MouseButtons.Left)
            {
                //first need to see what word we double clicked, if not, run a command
                if (_linkedWord.Length > 0)
                {
                    //check if it is a URL
                    Regex re = new Regex(_wwwMatch);
                    MatchCollection matches = re.Matches(_linkedWord);
                    String clickedWord = _linkedWord;

                    if (this.Parent.GetType() == typeof(IceTabPage))
                    {
                        IceTabPage t = (IceTabPage)this.Parent;
                        //check if it is a channel
                        //remove any user types from the front of the clickedWord
                        string chan = clickedWord;
                        for (int i = 0; i < t.Connection.ServerSetting.StatusModes[1].Length; i++)
                            if (chan.StartsWith(t.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                chan = chan.Substring(1);

                        if (chan.Length>0 && Array.IndexOf(t.Connection.ServerSetting.ChannelTypes, chan[0]) != -1)
                        {
                            FormMain.Instance.ParseOutGoingCommand(t.Connection, "/join " + chan);
                            return;
                        }

                        string clickedWordNick = StripString(clickedWord);

                        //check if it is a nickname in the current channel
                        if (t.WindowStyle == IceTabPage.WindowType.Channel)
                        {
                            if (t.WindowStyle == IceTabPage.WindowType.Channel)
                            {
                                foreach (User u in t.Nicks.Values)
                                {
                                    if (u.NickName == clickedWordNick)
                                    {
                                        FormMain.Instance.ParseOutGoingCommand(t.Connection, "/query " + clickedWordNick);
                                        break;
                                    }
                                    else if (u.NickName == clickedWord)
                                    {
                                        FormMain.Instance.ParseOutGoingCommand(t.Connection, "/query " + clickedWord);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (this.Parent.GetType() == typeof(ConsoleTab))
                    {
                        ConsoleTab c = (ConsoleTab)this.Parent;
                        if (c.Connection != null)
                        {
                            //check if it is a channel
                            if (c.Connection.IsFullyConnected)
                            {
                                string chan = clickedWord;
                                for (int i = 0; i < c.Connection.ServerSetting.StatusModes[1].Length; i++)
                                    if (chan.StartsWith(c.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                        chan = chan.Substring(1);

                                if (chan.Length>0 && Array.IndexOf(c.Connection.ServerSetting.ChannelTypes, chan[0]) != -1)
                                {
                                    FormMain.Instance.ParseOutGoingCommand(c.Connection, "/join " + chan);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            if (this.Parent.GetType() == typeof(ConsoleTab))
            {
                //console
                ConsoleTab c = (ConsoleTab)this.Parent;
                if (c.Connection != null && c.Connection.IsConnected)
                    FormMain.Instance.ParseOutGoingCommand(c.Connection, "/lusers");
            }
            else if (this.Parent.GetType() == typeof(IceTabPage))
            {
                IceTabPage t = (IceTabPage)this.Parent;
                if (t.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    if (t.Connection.IsConnected)
                        FormMain.Instance.ParseOutGoingCommand(t.Connection, "/chaninfo");
                }
            }
            else if (this.Parent.GetType() == typeof(Panel))
            {
                if (this.Parent.Parent.GetType() == typeof(IceTabPage))
                {
                    IceTabPage t = (IceTabPage)this.Parent.Parent;
                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                        if (t.Connection.IsConnected)
                            FormMain.Instance.ParseOutGoingCommand(t.Connection, "/chaninfo");
                }
            }
        }

        internal string BackGroundImage
        {
            get
            {
                return _backgroundImageFile;
            }
            set
            {
                if (value.Length > 0)
                    this._backgroundImage = new Bitmap(value);
                else
                    this._backgroundImage = null;

                this._backgroundImageFile = value;
                
                // save to channel setting
                if (this.Parent != null && this.Parent.GetType() == typeof(IceTabPage))
                {
                    IceTabPage t = (IceTabPage)this.Parent;
                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                    {
                        ChannelSetting cs = FormMain.Instance.ChannelSettings.FindChannel(t.TabCaption, t.Connection.ServerSetting.NetworkName);
                        if (cs != null)
                        {
                            cs.BackgroundImage = value;
                        }
                        else
                        {
                            ChannelSetting cs1 = new ChannelSetting
                            {
                                BackgroundImage = value,
                                NoColorMode = _noColorMode,
                                ChannelName = t.TabCaption,
                                NetworkName = t.Connection.ServerSetting.NetworkName
                            };
                            FormMain.Instance.ChannelSettings.AddChannel(cs1);
                        }

                        FormMain.Instance.SaveChannelSettings();
                    }
                }

                Invalidate();
            }
        }

        internal int IRCBackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                Invalidate();
            }
        }

        public bool SingleLine
        {
            get
            {
                return _singleLine;
            }
            set
            {
                _singleLine = value;
                
                if (_singleLine)
                {
                    toolTip = new ToolTip
                    {
                        ShowAlways = true,
                        AutoPopDelay = 10000,
                        InitialDelay = 2000,
                        ForeColor = System.Drawing.SystemColors.InfoText,
                        BackColor = System.Drawing.SystemColors.Info
                    };
                }

                Invalidate();
            }
        }

        internal bool NoColorMode
        {
            get
            {
                return _noColorMode;
            }
            set
            {
                _noColorMode = value;

                if (this.Parent != null && this.Parent.GetType() == typeof(IceTabPage))
                {
                    IceTabPage t = (IceTabPage)this.Parent;
                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                    {
                        ChannelSetting cs = FormMain.Instance.ChannelSettings.FindChannel(t.TabCaption, t.Connection.ServerSetting.NetworkName);
                        if (cs != null)
                        {
                            cs.NoColorMode = value;
                        }
                        else
                        {
                            ChannelSetting cs1 = new ChannelSetting
                            {
                                NoColorMode = value,
                                ChannelName = t.TabCaption,
                                NetworkName = t.Connection.ServerSetting.NetworkName
                            };
                            FormMain.Instance.ChannelSettings.AddChannel(cs1);
                        }
                        
                        FormMain.Instance.SaveChannelSettings();
                    }
                }
            }
        }

        internal bool NoEmoticons
        {
            get { return _noEmoticons; }
            set { _noEmoticons = value; }
        }

        internal bool ReloadText
        {
            get { return _reloadText; }
            set { _reloadText = value; }
        }

        internal int MaximumTextLines
        {
            get
            {
                return _maxTextLines;
            }
            set { 
                
                _maxTextLines = value;
                
                _displayLines = new DisplayLine[_maxTextLines * 5];
                _textLines = new TextLine[_maxTextLines];
                vScrollBar.Value = 1;
                vScrollBar.Maximum = 1;
                vScrollBar.Minimum = 1;
                _totalLines = 0;
                _totaldisplayLines = 0;

                LoadTextSizes();
                
            }
        }

        internal void SetDebugWindow()
        {
            //disable doubleclick when this is a Debug Text Window
            this.DoubleClick -= OnDoubleClick;
        }


        internal void ClearTextWindow()
        {
            //clear the text window of all its lines
            _displayLines.Initialize();
            _textLines.Initialize();

            _totalLines = 0;
            _totaldisplayLines = 0;
            
            Invalidate();
        }
        /*
        internal void SetLogFile()
        {
            if (this.Parent.GetType() == typeof(IceTabPage))
            {
                IceTabPage t = (IceTabPage)this.Parent;
                _logClass = new Logging(t);
            }
            else if (this.Parent.GetType() == typeof(ConsoleTab))
            {
                ConsoleTab c = (ConsoleTab)this.Parent;
                _logClass = new Logging(c);
            }
        }
        */

        internal void LoadDumpFile(IceTabPage t)
        {
            try
            {

                string dumpFile = _logClass.LogFileLocation + System.IO.Path.DirectorySeparatorChar + t.TabCaption + ".dump.xml";
                System.Diagnostics.Debug.WriteLine("Try load dump file " + dumpFile);

                if (File.Exists(dumpFile))
                {
                    //import the file dump, and add a LINE READ marker
                    System.Diagnostics.Debug.WriteLine("Do Load dump file " + dumpFile);

                    List<TextLine> textLines = new List<TextLine>();
                    XmlSerializer deserializer = new XmlSerializer(textLines.GetType());
                    TextReader textReader = new StreamReader(dumpFile);

                    textLines = (List<TextLine>)deserializer.Deserialize(textReader);

                    textReader.Close();
                    textReader.Dispose();

                    for (int i = 0; i < textLines.Count; i++)
                    {
                        _textLines[i] = textLines[i];
                        //replace the codes
                        _textLines[i].line = _textLines[i].line.Replace("&#x3;", (newColorChar).ToString()).Replace("#x2;", ((char)2).ToString()).Replace("#x0F;", ((char)15).ToString());
                    }

                    //adds a blank line to show loaded - this doesnt log
                    //_textLines[textLines.Count].line = (newColorChar).ToString() + "0499 - Total lines loaded:" + (textLines.Count - 1);
                    //_textLines[textLines.Count].totalLines = 1;
                    //_textLines[textLines.Count].textColor = 4;
                    //_textLines[textLines.Count].width = 300;
                        
                    _totalLines = textLines.Count - 1;

                    AddText("<---- Loaded Previous Log ---->",4,"");
                    
                    System.Diagnostics.Debug.WriteLine(textLines.Count + " lines loaded:" + t.TabCaption);

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("LoadDump:" + ex.StackTrace + ":" + ex.Message);
            }
        }

        internal void SetLogFile(bool reloadText)
        {            
            if (this.Parent.GetType() == typeof(IceTabPage))
            {                
                IceTabPage t = (IceTabPage)this.Parent;
                _logClass = new Logging(t);
                
                //check if a dump file exists                
                try
                {
                    if (t.WindowStyle == IceTabPage.WindowType.Channel && reloadText == true && t.LoggingDisable == false)
                    {
                        _reloadText = true;
                        LoadDumpFile(t);                        
                    }
                }
                catch (NullReferenceException)
                {
                    //MessageBox.Show(ee.Message + ee.Source);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace + ":" + ex.Message);
                }
            }
            else if (this.Parent.GetType() == typeof(ConsoleTab))
            {
                ConsoleTab c = (ConsoleTab)this.Parent;
                _logClass = new Logging(c);
            }
        }

        internal string LogFileLocation
        {
            get
            {
                if (_logClass == null)
                    return "";

                return _logClass.LogFileLocation;
            }
        }

        internal string LogFileName
        {
            get
            {
                if (_logClass == null)
                    return "";

                return _logClass.LogFileName;
            }
        }

        internal void DisableLogFile()
        {
            if (_logClass != null)
            {
                _logClass.Dispose();
                _logClass = null;
            }
            else
                SetLogFile(_reloadText);                
        }

        internal void ResetUnreadMarker()
        {            
            _unreadReset = true;
        }

        internal bool ShowTimeStamp
        {
            get { return _showTimeStamp; }
            set { _showTimeStamp = value; }
        }

        internal int TotalLines
        {
            get { return _totalLines; }
        }

        internal string SaveDumpFile(bool forced)
        {            
            //no dump for console, only channels
            string dumpFile = LogFileLocation;

            if (this.Parent.GetType() == typeof(IceTabPage) && (_reloadText == true || forced == true))
            {
                IceTabPage t = (IceTabPage)this.Parent;
                if (t.WindowStyle == IceTabPage.WindowType.Channel && (t.LoggingDisable == false || forced == true))
                {
                    //replace any illegal characters
                    string tabCaption = t.TabCaption;
                    tabCaption = tabCaption.Replace(":","");
                    
                    dumpFile += System.IO.Path.DirectorySeparatorChar + tabCaption + ".dump.xml";
                    
                    System.Diagnostics.Debug.WriteLine("dump file = " + dumpFile);
                    
                    //create a copy of the _textLines, removing blank
                    try
                    {
                        List<TextLine> temp = new List<TextLine>();
                        foreach (TextLine s in _textLines)
                        {
                            if (s.line != null && s.line.Length > 0 && !s.line.EndsWith("<---- Loaded Previous Log ---->"))
                            {
                                TextLine tl = s;
                                tl.line = tl.line.Replace((newColorChar).ToString(), "&#x3;").Replace(((char)2).ToString(), "#x2;").Replace(((char)15).ToString(), "#xF;").Replace(((char)31).ToString(), "#x1F;");

                                temp.Add(tl);
                            }
                        }

                        XmlSerializer writer = new XmlSerializer(temp.GetType());
                        TextWriter file = new StreamWriter(dumpFile, false);
                        writer.Serialize(file, temp);

                        file.Flush();
                        file.Close();
                    }
                    catch (InvalidOperationException ee)
                    {
                        System.Diagnostics.Debug.WriteLine("Save Dump IOC:" + ee.Message);
                        System.Diagnostics.Debug.WriteLine(ee.Source);
                    }
                    catch (NullReferenceException e)
                    {
                        System.Diagnostics.Debug.WriteLine("SaveDump:" +e.Message);
                        System.Diagnostics.Debug.WriteLine(e.Source); ;
                    }
                }
            }
            return dumpFile;
        }

        internal void AppendText(string newLine, string timeStamp)
        {
            if (newLine.Length == 0)
                return;

            newLine = newLine.Replace("\n", " ");
            // this is for replacing codes in the IRC Messages from Colors
            newLine = newLine.Replace("&#x3;", colorChar.ToString());
            newLine = newLine.Replace("&#x2;", boldChar.ToString());
            newLine = newLine.Replace("&#x1F;", underlineChar.ToString());
            newLine = newLine.Replace("&#x0F;", cancelChar.ToString());
            newLine = newLine.Replace("&#x16;", reverseChar.ToString());
            
            newLine = ParseUrl(newLine);

            int _foreColor = 1;
            if (_backColor == 1)
                _foreColor = 0;

            //get the color from the line
            if (newLine[0] == colorChar)
            {
                if (Char.IsNumber(newLine[1]) && Char.IsNumber(newLine[2]))
                    _foreColor = Convert.ToInt32(newLine[1].ToString() + newLine[2].ToString());
                else if (Char.IsNumber(newLine[1]) && !Char.IsNumber(newLine[2]))
                    _foreColor = Convert.ToInt32(newLine[1].ToString());

            }

            AddText(newLine, _foreColor, timeStamp);
        }

        private void AddText(string newLine, int color, string newTimeStamp)
        {
            int errorNumber = 0;
            
            try
            {
                //adds a new line to the Text Window
                if (this.Disposing)
                    return;
                
                if (newLine.Length == 0)
                    return;

                if (_unreadReset)
                {
                    _unreadMarker = 0;
                    _unreadReset = false;
                }

                ++_unreadMarker;

                string timeStamp = "";
                if (newTimeStamp.Length > 0)
                {
                    DateTime ts;
                    if (DateTime.TryParse(newTimeStamp, out ts))
                        timeStamp = ts.ToString(FormMain.Instance.IceChatOptions.TimeStamp.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()));
                    else
                        timeStamp = newTimeStamp;
                }
                else if (FormMain.Instance.IceChatOptions.ShowTimeStamp)
                    if (!_singleLine && _showTimeStamp)
                        timeStamp = DateTime.Now.ToString(FormMain.Instance.IceChatOptions.TimeStamp.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()));

                if (_logClass != null)
                    _logClass.WriteLogFile(timeStamp + newLine);

                _totalLines++;

                if (_singleLine)
                    toolTipString = ReplaceColorCodes(newLine).Replace(urlStart.ToString(),"").Replace(urlEnd.ToString(),"");

                newLine = ParseEmoticons(newLine);

                if (FormMain.Instance.IceChatOptions.ShowTimeStamp)
                    if (!_singleLine && _showTimeStamp)
                        newLine = timeStamp + newLine;

                if (_noColorMode)
                    newLine = ReplaceColorCodes(newLine);
                else
                    newLine = RedefineColorCodes(newLine);

                int lineDiff = 99;   //how many lines to trim off

                errorNumber = 1;

                if (_totalLines >= (_maxTextLines - 1))
                {
                    int x = 1;
                    
                    //System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
                    //s.Start();
                    System.Diagnostics.Debug.WriteLine("reset lines start");

                    errorNumber = 2;

                    Array.Copy(_textLines, lineDiff, _textLines, 0, _totalLines - lineDiff);

                    _totalLines = _totalLines - (lineDiff + 1);

                    x = 0;
                    for (int i = 1; i <= _totalLines; i++)
                    {
                        x = x + _textLines[i].totalLines;
                    }
                    int removeLines = _totaldisplayLines - x;

                    _totaldisplayLines = x;
                    errorNumber = 3;

                    Array.Copy(_displayLines, removeLines, _displayLines, 0, _totaldisplayLines);

                    
                    UpdateScrollBar(_totaldisplayLines);
                    Invalidate();
                    errorNumber = 4;

                    //System.Diagnostics.Debug.WriteLine("reset lines end:" + s.ElapsedMilliseconds);
                    //s.Stop();

                    _totalLines++;
                }

                errorNumber = 5;

                _textLines[_totalLines].line = newLine;

                Graphics g = this.CreateGraphics();
                //properly measure for bold characters needed
                _textLines[_totalLines].width = MeasureString(StripAllCodes(newLine, false), g, false);

                g.Dispose();

                errorNumber = 6;

                //_textLines[_totalLines].textColor = _foreColor;
                _textLines[_totalLines].textColor = color;

                int addedLines = FormatLines(_totalLines, _totalLines, _totaldisplayLines);
                addedLines -= _totaldisplayLines;

                //if client is minimized.. we get a PROBLEM!
                _textLines[_totalLines].totalLines = addedLines;

                for (int i = _totaldisplayLines + 1; i < _totaldisplayLines + addedLines; i++)
                    _displayLines[i].textLine = _totalLines;

                _totaldisplayLines += addedLines;

                UpdateScrollBar(_totaldisplayLines);

                if (_singleLine)
                    vScrollBar.Value = 1;

                Invalidate();
            }
            catch (OutOfMemoryException)
            {
                //System.Diagnostics.Debug.WriteLine("Out of Memory Exception:" + oe.Message + ":" + oe.StackTrace);
            }
            catch (Exception)
            {
                // Error:AddText:3:Number was less than the array's lower bound in the first dimension. <- this happens now and then from line 1940 Array.Copy
                // FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "AddText:" + errorNumber, e);
            }
        }

        internal int SearchText(string data, int start)
        {
            int totalChars = 0;
            //System.Diagnostics.Debug.WriteLine(_totalLines + ":" + start);
            for (int i = 1; i <= _totalLines; i++)
            {
                string line = StripAllCodes(_textLines[i].line, true);
                if ((line.Length + totalChars) > start)
                {
                    int x = line.IndexOf(data);
                    if (x > -1)
                    {
                        //we have a match
                        //now check to make sure it is past the start position
                        if (x > (totalChars + start))
                        {

                        }
                        //System.Diagnostics.Debug.WriteLine("match:" + (x + totalChars));
                    }
                }
                //System.Diagnostics.Debug.WriteLine(_textLines[i].line.IndexOf(data));
                totalChars += line.Length;
            }
            return -1;
        }


        /// <summary>
        /// Used to scroll the Text Window a Page at a Time
        /// </summary>
        /// <param name="scrollUp"></param>
        internal void ScrollWindowPage(bool scrollUp)
        {
            try
            {
                if (vScrollBar.Enabled == false)
                    return;

                if (scrollUp == true)
                {
                    if (vScrollBar.Value > vScrollBar.LargeChange)
                    {
                        vScrollBar.Value = vScrollBar.Value - (vScrollBar.LargeChange - 1);
                        Invalidate();
                    }
                }
                else
                {                    
                    if (vScrollBar.Value <= vScrollBar.Maximum - (vScrollBar.LargeChange * 2))
                        vScrollBar.Value = vScrollBar.Value + (vScrollBar.LargeChange - 1);
                    else
                        vScrollBar.Value = vScrollBar.Maximum - vScrollBar.LargeChange + 1;
                    Invalidate();
                }
            }
            catch (Exception e)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "ScrollWindowPage", e);
            }

        }
        /// <summary>
        /// Used to scroll the Text Window a Single Line at a Time
        /// </summary>
        /// <param name="scrollUp"></param>
        internal void ScrollWindow(bool scrollUp)
        {
            try
            {
                if (vScrollBar.Enabled == false)
                    return;

                if (scrollUp == true)
                {
                    if (vScrollBar.Value > 1)
                    {
                        vScrollBar.Value--;
                        Invalidate();
                    }
                }
                else
                {
                    if (vScrollBar.Value <= vScrollBar.Maximum - vScrollBar.LargeChange)
                    {
                        vScrollBar.Value++;
                        Invalidate();
                    }
                }
            }
            catch (Exception e)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "ScrollWindow", e);
            }
        }

        private string StripString(string targetString)
        {
            //strip all non-alpha numeric chars from string (for nicknames)
            //only allow chars that are allowed in nicks
            return Regex.Replace(targetString, @"[^A-Za-z0-9_\-|\[\]\\\`\^{}]", "");
        }


        #region TextWindow Events



        private void OnFontChanged(object sender, System.EventArgs e)
        {
            LoadTextSizes();

            _displayLines.Initialize();

            _totaldisplayLines = FormatLines(_totalLines, 1, 0);
            UpdateScrollBar(_totaldisplayLines);

            Invalidate();

        }

        private void OnResize(object sender, System.EventArgs e)
        {

            if (this.Height == 0 || _totalLines == 0)
                return;

            if (_buffer != null)
            {
                _buffer.Dispose();
                _buffer = null;
            }

            if (this.Width == _oldWidth)
            {
                //only width changed,just change the scrollbar
                UpdateScrollBar(_totaldisplayLines);
            }
            else
            {
                _displayLines.Initialize();
                _reformatLines = true;
            }

            Invalidate();

            _oldWidth = this.Width;
            _oldDisplayWidth = this.ClientRectangle.Width - vScrollBar.Width - 10;

        }

        /// <summary>
        /// Updates the scrollbar to the given line. 
        /// </summary>
        /// <param name="newValue">Line number to be displayed</param>
        /// <param name="endLine"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private void UpdateScrollBar(int newValue)
        {
            if (this.Height > 0)
                _showMaxLines = (this.Height / _lineSize) + 1;
            
            if (this.InvokeRequired)
            {
                ScrollValueDelegate s = new ScrollValueDelegate(UpdateScrollBar);
                this.Invoke(s, new object[] { newValue });
            }
            else
            {
                if (_showMaxLines < _totaldisplayLines)
                {
                    vScrollBar.LargeChange = _showMaxLines;
                    vScrollBar.Enabled = true;
                }
                else
                {
                    vScrollBar.LargeChange = _totaldisplayLines;
                    vScrollBar.Enabled = false;
                }

                if (_singleLine && _totaldisplayLines == 1)
                {
                    vScrollBar.Visible = false;
                    vScrollBar.Enabled = true;
                }
                else if (_singleLine)
                {
                    vScrollBar.Visible = true;
                    vScrollBar.Enabled = true;
                }

                if (newValue != 0)
                {
                    vScrollBar.Minimum = 1;
                    vScrollBar.Maximum = newValue + vScrollBar.LargeChange - 1;

                    //System.Diagnostics.Debug.WriteLine(_showMaxLines + ":" + this.Height);

                    //System.Diagnostics.Debug.WriteLine(_showMaxLines + ":" + this.Parent.Name + ":" + newValue + ":" + vScrollBar.Value + ":" + vScrollBar.LargeChange + "::" + (vScrollBar.Value + (_showMaxLines / 2)));
                    //if (newValue <= (vScrollBar.Value + (_showMaxLines / 2)) || vScrollBar.Enabled == false)
                    //System.Diagnostics.Debug.WriteLine(newValue + ":" + vScrollBar.Value + ":" + _showMaxLines + ":"   +( vScrollBar.Value + _showMaxLines));
                    
                    if (newValue <= (vScrollBar.Value + (_showMaxLines *.50)) || vScrollBar.Enabled == false)
                    {
                        if (this.Parent != null)
                            if (this.Parent.Name == "panelTopic")
                                return;
                        
                        vScrollBar.Value = newValue;
                    }
                    else
                    {
                        // :10:1:10::15
                        //System.Diagnostics.Debug.WriteLine("not updated:" + vScrollBar.Enabled + ":" + vScrollBar.Value + ":" + newValue);
                        if (vScrollBar.Enabled == true)
                        {
                            //should we make some kind of an indicator??

                        }
                    }
                }
            }
        }

        internal void ScrollToBottom()
        {
            if (this.InvokeRequired)
            {
                ScrollBottomDelegate s = new ScrollBottomDelegate(ScrollToBottom);
                this.Invoke(s, new object[] { });

            }
            else
            {
                if (_totaldisplayLines > 0)
                {
                    if (_totaldisplayLines <= vScrollBar.Maximum)
                        vScrollBar.Value = _totaldisplayLines;
                }
            }
        }

        private void OnScroll(object sender, EventArgs e)
        {
            if (((VScrollBar)sender).Value < 1)
                ((VScrollBar)sender).Value = 1;


            Invalidate();
        }

        #endregion

        #region Emoticon and Color Parsing

        private string ParseEmoticons(string line)
        {
            if (FormMain.Instance.IceChatOptions.ShowEmoticons && FormMain.Instance.IceChatEmoticons.listEmoticons.Count > 0 && !_noEmoticons)
            {
                if (_emotMatch.Length > 0)
                {
                    string[] eachEmot = _emotMatch.Split((char)0);
                    for (int i = eachEmot.GetLowerBound(0); i <= eachEmot.GetUpperBound(0); i++)
                        line = line.Replace(@eachEmot[i], emotChar + i.ToString("000"));
                }
            }

            return line;

        }

        private string RedefineColorCodes(string line)
        {
            //redefine the irc server colors to own standard
            // go from \x0003xx,xx to \x0003xxxx

            StringBuilder sLine = new StringBuilder();
            sLine.Append(line);

            int oldLen = 0;

            int currentBackColor = -1;

            Match m = StaticMethods.ParseColorCodes.Match(sLine.ToString());
            while (m.Success)
            {
                oldLen = sLine.Length;
                sLine.Remove(m.Index, m.Length);

                if (Regex.Match(m.Value, StaticMethods.ParseBackColor).Success)
                {
                    string rem = m.Value.Remove(0, 1);
                    string[] intstr = rem.Split(new Char[] { ',' });
                    //get the fore color                    
                    int fc = int.Parse(intstr[0]);
                    if (fc > (IrcColor.colors.Length - 1))
                        fc = int.Parse(intstr[0].Substring(1, 1));
                    //get the back color
                    int bc = int.Parse(intstr[1]);
                    if (bc > (IrcColor.colors.Length - 1))
                    {
                        bc = int.Parse(intstr[1].Substring(1, 1));
                        currentBackColor = bc;
                        sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + bc.ToString("00") + intstr[1].Substring(2));
                    }
                    else
                    {
                        currentBackColor = bc;
                        sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + bc.ToString("00"));
                    }
                    oldLen--;
                }
                else if (Regex.Match(m.Value, StaticMethods.ParseForeColor).Success)
                {
                    int fc = int.Parse(m.Value.Remove(0, 1));
                    if (fc > (IrcColor.colors.Length - 1))
                    {
                        fc = int.Parse(m.Value.Substring(1, 1));
                        if (currentBackColor > -1)
                            sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + currentBackColor.ToString("00") + m.Value.Substring(2));
                        else
                            sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + "99" + m.Value.Substring(2));
                    }
                    else
                    {
                        if (currentBackColor > -1)
                            sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + currentBackColor.ToString("00"));
                        else
                            sLine.Insert(m.Index, newColorChar.ToString() + fc.ToString("00") + "99");
                    }
                }
                else if (Regex.Match(m.Value, StaticMethods.ParseColorChar).Success)
                {
                    currentBackColor = -1;
                    //sLine.Insert(m.Index, newColorChar.ToString() + _foreColor.ToString("00") + "99");
                    sLine.Insert(m.Index, newColorChar.ToString() + "9999");
                }
                m = StaticMethods.ParseColorCodes.Match(sLine.ToString(), sLine.Length - oldLen);
            }
            return sLine.ToString();
        }

        private string ReplaceColorCodes(string line)
        {
            //strip out all the color codes,bold,underline and reverse codes
            StringBuilder sLine = new StringBuilder();
            sLine.Append(line);

            Match m = StaticMethods.ParseAllCodes.Match(sLine.ToString());
            
            while (m.Success)
            {                
                sLine.Remove(m.Index, m.Length);                
                m = StaticMethods.ParseAllCodes.Match(sLine.ToString(), m.Index);
            }

            return sLine.ToString();
        }

        #endregion

        /// <summary>
        /// Format the text for each line to show in the Text Window
        /// </summary>
        /// <param name="startLine"></param>
        /// <param name="lastLine"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private int FormatLines(int startLine, int lastLine, int line)
        {
            //this formats each line and breaks it up, to fit onto the current display
            int displayWidth = this.ClientRectangle.Width - vScrollBar.Width - 10;

            if (displayWidth <= 0)
            {
                displayWidth = _oldDisplayWidth;
            }

            if (_totalLines == 0)
                return 0;


            string lastColor = "";
            string nextColor = "";

            bool lineSplit;
            int ii = line;
            Graphics g = this.CreateGraphics();

            g.InterpolationMode = InterpolationMode.Low;
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.PixelOffsetMode = PixelOffsetMode.None;
            g.CompositingQuality = CompositingQuality.HighSpeed;            
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            try
            {

                for (int currentLine = lastLine; currentLine <= startLine; currentLine++)
                {
                    lastColor = "";
                    
                    _displayLines[line].previous = false;
                    _displayLines[line].wrapped = false;

                    //check of the line width is the same or less then the display width            
                    if (_textLines[currentLine].width <= displayWidth)
                    {
                        try
                        {
                            _displayLines[line].line = _textLines[currentLine].line + endLine;
                            _displayLines[line].textLine = currentLine;
                            _displayLines[line].textColor = _textLines[currentLine].textColor;
                            _displayLines[line].lineHeight = _lineSize;

                            line++;


                        }
                        catch (Exception e)
                        {
                            FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "FormatLines Error1:", e);
                        }
                    }
                    else
                    {
                        lastColor = "";
                        nextColor = "";

                        lineSplit = false;
                        string curLine = _textLines[currentLine].line;

                        StringBuilder buildString = new StringBuilder();

                        bool bold = false;
                        bool underline = false;
                        bool italic = false;
                        bool reverse = false;

                        int boldPos = 0;
                        int underlinePos = 0;

                        // https://codepoints.net/U+1F0A0

                        char[] chars = curLine.ToCharArray();
                        
                        for (int i = 0; i < chars.Length; i++)
                        {
                            char ch = chars[i];
                          
                            switch (ch)
                            {
                                case boldChar:
                                    bold = !bold;
                                    buildString.Append(ch);
                                    boldPos = i;
                                    break;
                                case italicChar:
                                    italic = !italic;
                                    buildString.Append(ch);
                                    break;
                                case underlineChar:
                                    underline = !underline;
                                    buildString.Append(ch);
                                    underlinePos = i;
                                    break;
                                case reverseChar:
                                    reverse = !reverse;
                                    buildString.Append(ch);
                                    break;
                                case cancelChar:
                                    underline = false;
                                    italic = false;
                                    bold = false;
                                    reverse = false;
                                    
                                    // reset the colors
                                    lastColor = newColorChar + _textLines[currentLine].textColor.ToString("00") + "99";
                                    nextColor = newColorChar + _textLines[currentLine].textColor.ToString("00") + "99";
                                    
                                    buildString.Append(ch);
                                    boldPos = i;
                                    underlinePos = i;
                                    break;
                                case newColorChar:
                                    buildString.Append(curLine.Substring(i, 5));
                                    if (lastColor.Length == 0)
                                    {
                                        lastColor = curLine.Substring(i, 5);
                                    }
                                    else
                                    {
                                        nextColor = curLine.Substring(i, 5);
                                    }
                                    i = i + 4;
                                    break;
                                case emotChar:
                                    buildString.Append(curLine.Substring(i, 4));
                                    i = i + 3;
                                    break;
                                default:
                                    //check if there needs to be a linewrap                                    
                                    if (MeasureString(StripAllCodes(buildString.ToString(), false), g, false) > displayWidth)
                                    {
                                        //check for line wrapping
                                        int lastSpace = buildString.ToString().LastIndexOf(' ');
                                        //System.Diagnostics.Debug.WriteLine(lastSpace + ":" + buildString.Length + ":" + displayWidth + ":" + buildString.ToString());
                                        if (lastSpace > (buildString.Length * 4 / 5))
                                        {
                                            int intNewPos = i - (buildString.Length - lastSpace) + 1;

                                            if ((buildString.Length - lastSpace) != 1)
                                            {
                                                //System.Diagnostics.Debug.WriteLine("line:" + buildString.ToString());
                                                
                                                buildString.Remove(lastSpace, buildString.Length - lastSpace);
                                                buildString.Append(wrapLine);
                                            }

                                            //check for bold and underline accordingly
                                            i = intNewPos;
                                            //ch = curLine.Substring(i, 1).ToCharArray();
                                            ch = chars[i];

                                            //System.Diagnostics.Debug.WriteLine("new ch:" + (int)ch[0] + ":" + i + ":" + boldPos);
                                            switch (ch)
                                            {
                                                case boldChar:
                                                    if (i < boldPos)
                                                    {
                                                        //do nothing - already parsed
                                                    }
                                                    else
                                                    {
                                                        bold = !bold;
                                                        buildString.Append(ch);
                                                        boldPos = i;
                                                    }
                                                    break;
                                                case italicChar:                                                    
                                                    italic = !italic;
                                                    buildString.Append(ch);
                                                    break;
                                                case underlineChar:
                                                    if (i < underlinePos)
                                                    {
                                                        //do nothing
                                                    }
                                                    else
                                                    {
                                                        underline = !underline;
                                                        buildString.Append(ch);
                                                        underlinePos = i;
                                                    }
                                                    break;
                                                case reverseChar:
                                                    reverse = !reverse;
                                                    buildString.Append(ch);
                                                    break;
                                                case cancelChar:
                                                    underline = false;
                                                    italic = false;
                                                    bold = false;
                                                    reverse = false;
                                                    //reset the color
                                                    lastColor = newColorChar + _textLines[currentLine].textColor.ToString("00") + "99";
                                                    nextColor = newColorChar + _textLines[currentLine].textColor.ToString("00") + "99";

                                                    buildString.Append(ch);
                                                    boldPos = i;
                                                    underlinePos = i;
                                                    break;
                                            }                                            
                                        }

                                        if (lineSplit)
                                        {
                                            _displayLines[line].line = lastColor + buildString.ToString();
                                        }
                                        else
                                        {
                                            _displayLines[line].line = buildString.ToString();
                                        }

                                        _displayLines[line].textLine = currentLine;
                                        _displayLines[line].wrapped = true;
                                        _displayLines[line].textColor = _textLines[currentLine].textColor;
                                        _displayLines[line].lineHeight = _lineSize;

                                        lineSplit = true;
                                        if (nextColor.Length != 0)
                                        {
                                            lastColor = nextColor;
                                            nextColor = "";
                                        }
                                        line++;
                                        if (line == (_maxTextLines * 5))
                                        {
                                            //System.Diagnostics.Debug.WriteLine("break here. window width too narrow");
                                            return line - 1;
                                        }
                                        _displayLines[line].previous = true;

                                        buildString = null;
                                        buildString = new StringBuilder();

                                        if (underline) buildString.Append(underlineChar);
                                        if (bold) buildString.Append(boldChar);
                                        if (italic) buildString.Append(italicChar);
                                        if (reverse) buildString.Append(reverseChar);
                                        buildString.Append(ch);
                                    }
                                    else
                                        buildString.Append(ch);
                                    break;
                            }
                        }
                        //get the remainder

                        if (lineSplit)
                        {
                            _displayLines[line].line = lastColor + buildString.ToString() + endLine;
                        }
                        else
                        {
                            _displayLines[line].line = buildString.ToString() + endLine;
                        }

                        buildString = null;

                        _displayLines[line].textLine = currentLine;
                        _displayLines[line].textColor = _textLines[currentLine].textColor;
                        _displayLines[line].lineHeight = _lineSize;

                        line++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + ":" + ex.StackTrace);
            }


            g.Dispose();

            //System.Diagnostics.Debug.WriteLine("return:" + line);

            return line;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!e.ClipRectangle.IsEmpty)
                OnDisplayText(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing
        }

        /// <summary>
        /// Method used to draw the actual text data for the Control
        /// </summary>
        private void OnDisplayText(PaintEventArgs e)
        {

            if (_reformatLines)
            {
                _totaldisplayLines = FormatLines(_totalLines, 1, 0);
                UpdateScrollBar(_totaldisplayLines);
            }

            try
            {
                int startY = 0;
                float startX = 0;
                int LinesToDraw = 0;

                StringBuilder buildString = new StringBuilder();
                float textSize;

                int curLine;
                int curForeColor, curBackColor, oldForeColor = 0, oldBackColor = 0;
                char[] ch;

                Rectangle displayRect = new Rectangle(0, 0, this.Width, this.Height);

                if (_buffer == null)
                {
                    _buffer = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                }

                Graphics g = Graphics.FromImage(_buffer);

                if (_backgroundImage != null)
                    g.DrawImage((Image)_backgroundImage, displayRect);
                else
                    g.Clear(IrcColor.colors[_backColor]);
                

                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                if (_totalLines == 0)
                {
                    e.Graphics.DrawImageUnscaled(_buffer, 0, 0);
                    g.Dispose();
                    return;
                }

                int val = vScrollBar.Value;

                LinesToDraw = (_showMaxLines > val ? val : _showMaxLines);
                curLine = val - LinesToDraw;

                if (_singleLine)
                {
                    startY = 0;
                    LinesToDraw = 1;
                    curLine = vScrollBar.Value - 1;
                }
                else
                    startY = this.Height - (_lineSize * LinesToDraw) - (_lineSize / 2);

                if (!FormMain.Instance.IceChatOptions.EmoticonsFixedSize)
                {
                    int totalHeight = 0;
                    int newLinesDraw = LinesToDraw;

                    //recalculate if we have different line heights
                    for (int i = 0; i < LinesToDraw; i++)
                    {
                        totalHeight += _displayLines[i].lineHeight;
                        if (_displayLines[i].lineHeight > _lineSize)
                        {
                            if ((this.Height - totalHeight) < (_lineSize * -1))
                            {
                                newLinesDraw--;
                            }
                        }
                    }

                    int x = _totaldisplayLines - 1;
                    int th = 0;
                    int count = 0;

                    while (x >= 0)
                    {
                        count++;
                        th += _displayLines[x].lineHeight;
                        if ((this.Height - th) < (_lineSize * -1))
                        {
                            newLinesDraw = count;
                            totalHeight = th;
                            break;
                        }
                        x--;
                    }

                    startY = ((this.Height - totalHeight) - (_lineSize / 2));

                    curLine = val - newLinesDraw;
                    LinesToDraw = newLinesDraw;
                }


                int lineCounter = 0;

                bool isInUrl = false;
                bool isInSelection = false;
                int curSelectionLine = -1;

                Font font = new Font(this.Font.Name, this.Font.Size, FontStyle.Regular, GraphicsUnit.Point);

                int redline = -1;

                if (FormMain.Instance.IceChatOptions.ShowUnreadLine && !_singleLine)
                {
                    for (int i = _totaldisplayLines - 1, j = 0; i >= 0; --i)
                    {
                        if (!_displayLines[i].previous)
                        {
                            ++j;
                            if (j >= _unreadMarker)
                            {
                                redline = i;
                                break;
                            }
                        }
                    }
                }

                while (lineCounter < LinesToDraw)
                {
                    int i = 0, curChar = 0;

                    bool underline = false;
                    bool reverse = false;
                    bool italic = false;
                    bool bold = false;

                    if (redline == curLine)
                    {
                        Pen p = new Pen(IrcColor.colors[FormMain.Instance.IceChatColors.UnreadTextMarkerColor]);
                        g.DrawLine(p, 0, startY, this.Width, startY);
                    }

                    lineCounter++;

                    curForeColor = _displayLines[curLine].textColor;
                    StringBuilder line = new StringBuilder();
                    line.Append(_displayLines[curLine].line);
                    curBackColor = _backColor;
                    

                    //check if in a url
                    if (!isInUrl)
                    {
                        font = null;
                        font = new Font(this.Font.Name, this.Font.Size, FontStyle.Regular);
                    }
                    else
                    {                        
//                        if (!isInSelection)
//                            curForeColor = FormMain.Instance.IceChatColors.HyperlinkColor;
                    }

                    if (_noColorMode)
                    {
                        if (curForeColor == 0 && curBackColor == 0)
                            curForeColor = 1;
                    }

                    if (line.Length > 0)
                    {
                        do                        
                        {
                            ch = line.ToString().Substring(i, 1).ToCharArray();
                                                        
                            switch (ch[0])
                            {
                                case (char)132:
                                case (char)133:
                                    System.Diagnostics.Debug.WriteLine("132 char");
                                    break;
                                
                                case wrapLine:
                                    break;
                                case endLine:
                                    isInUrl = false;
                                    break;
                                
                                case emotChar:
                                    //draws an emoticon
                                    //[]001
                                    if (line.Length > (i + 3))
                                    {
                                        string emotNum = line.ToString().Substring(i + 1, 3);
                                        int result;
                                        if (int.TryParse(emotNum, out result))
                                        {
                                            int emotNumber = Convert.ToInt32(emotNum);
                                            line.Remove(0, 3);
                                            if (!isInUrl)
                                            {
                                                //select the emoticon here
                                                Bitmap bm = new Bitmap(FormMain.Instance.EmoticonsFolder + System.IO.Path.DirectorySeparatorChar + FormMain.Instance.IceChatEmoticons.listEmoticons[emotNumber].EmoticonImage);

                                                if (curBackColor != _backColor && buildString.Length > 0)
                                                {
                                                    textSize = MeasureString(buildString.ToString(), g, false);
                                                    Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                                    g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                                }

                                                if (FormMain.Instance.IceChatOptions.EmoticonsFixedSize)
                                                {
                                                    g.DrawImage((Image)bm, startX + (int)g.MeasureString(buildString.ToString(), font, 0, stringFormat).Width, startY, _lineSize, _lineSize);
                                                    g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);
                                                    startX += _lineSize + MeasureString(buildString.ToString(), g, false);
                                                }
                                                else
                                                {
                                                    g.DrawImage((Image)bm, startX + (int)g.MeasureString(buildString.ToString(), font, 0, stringFormat).Width + 1, startY, bm.Width, bm.Height);

                                                    if (bm.Height > _lineSize)
                                                    {
                                                        //now how much extra height do we need to add?
                                                        if (_displayLines[curLine].lineHeight < bm.Height)
                                                        {
                                                            _displayLines[curLine].lineHeight = bm.Height;
                                                            //this causes a SCREEN FLASH, need to find out why
                                                            //_buffer.Dispose();
                                                            g.Dispose();
                                                            Invalidate();
                                                            return;
                                                        }
                                                    }
                                                    g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);
                                                    startX += bm.Width + MeasureString(buildString.ToString(), g, false);
                                                }

                                                buildString = null;
                                                buildString = new StringBuilder();
                                            }

                                            else
                                            {
                                                buildString.Append(FormMain.Instance.IceChatEmoticons.listEmoticons[emotNumber].Trigger);
                                            }
                                        }
                                        else
                                        {
                                            //not valid emot
                                            buildString.Append(ch[0]);
                                        }
                                    }
                                    else
                                    {
                                        //not valid emot
                                        buildString.Append(ch[0]);
                                    }
                                    break;
                                case urlStart:
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }
                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    //line.Remove(0, 1);
                                    i = 0;

                                    font = null;
                                    font = new Font(this.Font.Name, this.Font.Size, FontStyle.Underline, GraphicsUnit.Point);
                                    isInUrl = true;

                                    //System.Diagnostics.Debug.WriteLine("start url");

                                    if (!isInSelection)
                                    {
                                    //     oldForeColor = curForeColor;
                                    //     curForeColor = FormMain.Instance.IceChatColors.HyperlinkColor;

                                    //    oldBackColor = curBackColor;
                                    //    curBackColor = _backColor;
                                    }
                                    break;

                                case urlEnd:
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }

                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    //line.Remove(0, 1);

                                    i = 0;


                                    font = null;
                                    font = new Font(this.Font.Name, this.Font.Size, FontStyle.Regular, GraphicsUnit.Point);
                                    isInUrl = false;

                                    if (!isInSelection)
                                    {
                                    //    curForeColor = oldForeColor;
                                    //   curBackColor = oldBackColor;
                                    }
                                    break;
                                case underlineChar:
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }

                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    line.Remove(0, 1);
                                    i = -1;
                                    font = null;
                                    underline = !underline;

                                    FontStyle fs = FontStyle.Regular;
                                    if (underline) fs = FontStyle.Underline;
                                    if (italic) fs = fs | FontStyle.Italic;
                                    if (bold) fs = fs | FontStyle.Bold;

                                    font = new Font(this.Font.Name, this.Font.Size, fs, GraphicsUnit.Point);
                                    break;
                                case italicChar:
                                    //italic character
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }
                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    line.Remove(0, 1);
                                    i = -1;
                                    font = null;
                                    italic = !italic;

                                    FontStyle fsi = FontStyle.Regular;
                                    if (underline) fsi = FontStyle.Underline;
                                    if (italic) fsi = fsi | FontStyle.Italic;
                                    if (bold) fsi = fsi | FontStyle.Bold;

                                    font = new Font(this.Font.Name, this.Font.Size, fsi, GraphicsUnit.Point);
                                    break;
                                case boldChar:
                                    //bold character, currently ignored
                                    if (buildString.Length > 0)
                                    {

                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }
                                    //remove whats drawn from string

                                    line.Remove(0, i);
                                    line.Remove(0, 1);
                                    i = -1;
                                    font = null;
                                    bold = !bold;

                                    FontStyle fsb = FontStyle.Regular;
                                    if (underline) fsb = FontStyle.Underline;
                                    if (italic) fsb = fsb | FontStyle.Italic;
                                    if (bold) fsb = fsb | FontStyle.Bold;

                                    font = new Font(this.Font.Name, this.Font.Size, fsb, GraphicsUnit.Point);

                                    break;
                                case cancelChar:
                                    //draw with the standard fore and back color                                    
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        //System.Diagnostics.Debug.WriteLine(buildString.ToString());
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }

                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    line.Remove(0, 1);

                                    if (!isInSelection)
                                    {
                                        curForeColor = _displayLines[curLine].textColor;
                                        curBackColor = _backColor;
                                    }
                                    else
                                    {
                                        oldForeColor = _displayLines[curLine].textColor;
                                        oldBackColor = _backColor;
                                    }

                                    font = null;
                                    underline = false;
                                    italic = false;
                                    bold = false;

                                    font = new Font(this.Font.Name, this.Font.Size, FontStyle.Regular, GraphicsUnit.Point);

                                    i = -1;
                                    break;
                                case reverseChar:
                                    //reverse the fore and back colors
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }
                                    //remove whats drawn from string
                                    line.Remove(0, i);
                                    line.Remove(0, 1);
                                    if (!isInSelection)
                                    {
                                        if (reverse)
                                        {
                                            curForeColor = _displayLines[curLine].textColor;
                                            curBackColor = _backColor;
                                        }
                                        else
                                        {
                                            curForeColor = _backColor;
                                            curBackColor = _displayLines[curLine].textColor;
                                        }
                                    }
                                    else
                                    {
                                        if (reverse)
                                        {
                                            oldForeColor = _displayLines[curLine].textColor;
                                            oldBackColor = _backColor;
                                        }
                                        else
                                        {
                                            oldForeColor = _backColor;
                                            oldBackColor = _displayLines[curLine].textColor;
                                        }
                                    }
                                    reverse = !reverse;
                                    i = -1;
                                    break;
                                case newColorChar:
                                    //draw whats previously in the string
                                    if (buildString.Length > 0)
                                    {
                                        if (curBackColor != _backColor)
                                        {
                                            textSize = MeasureString(buildString.ToString(), g, bold);
                                            Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                            g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                        }
                                        g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                        startX += MeasureString(buildString.ToString(), g, bold);

                                        buildString = null;
                                        buildString = new StringBuilder();
                                    }
                                    //remove whats drawn from string
                                    line.Remove(0, i);

                                    int newForeColor = 0, newBackColor = 0;
                                    //color # 99 resets it back to normal
                                    //get the new fore and back colors                                                                        
                                    
                                    // check the validity fo the next 4 characters, they need to be numbers
                                    try
                                    {
                                        if (line.ToString().Length < 6)
                                        {
                                            //error.. move on
                                            line.Remove(0, 1); // remove just the color code
                                            buildString.Append(ch[0]);
                                        }
                                        else
                                        {
                                            // check that the 4 characters are numbers
                                            int result;
                                            if (int.TryParse(line.ToString().Substring(1, 4), out result))
                                            {
                                                newForeColor = Convert.ToInt32(line.ToString().Substring(1, 2));
                                                newBackColor = Convert.ToInt32(line.ToString().Substring(3, 2));

                                                //check to make sure that FC and BC are in range
                                                if (newForeColor > (IrcColor.colors.Length - 1))
                                                    newForeColor = _displayLines[curLine].textColor;
                                                if (newBackColor > (IrcColor.colors.Length - 1))
                                                    newBackColor = _backColor;

                                                if (!isInSelection)
                                                {
                                                    curForeColor = newForeColor;
                                                    curBackColor = newBackColor;
                                                }
                                                else
                                                {
                                                    oldForeColor = newForeColor;
                                                    oldBackColor = newBackColor;
                                                }

                                                if (isInUrl)
                                                {
                                                    //oldForeColor = curForeColor;
                                                    //curForeColor = FormMain.Instance.IceChatColors.HyperlinkColor;
                                                    //oldBackColor = curBackColor;
                                                    //curBackColor = _backColor;
                                                }
                                                //remove the color codes from the string
                                                line.Remove(0, 5);
                                            }
                                            else
                                            {
                                                // not numbers
                                                //error.. move on
                                                line.Remove(0, 1); // remove just the color code
                                                buildString.Append(ch[0]);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {                                    
                                        line.Remove(0, 1); // remove just the color code
                                        buildString.Append(ch[0]);
                                    }                                        
                                    i = -1;
                                    break;

                                default:
                                    if (_displayLines[curLine].selectionX1 == _displayLines[curLine].selectionX2)
                                    {
                                        //do nothing, just chill
                                    }
                                    else if (_displayLines[curLine].selectionX1 == curChar)
                                    {
                                        if (buildString.Length > 0)
                                        {
                                            if (curBackColor != _backColor)
                                            {
                                                textSize = MeasureString(buildString.ToString(), g, bold);
                                                Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                                g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                            }
                                            g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                            startX += MeasureString(buildString.ToString(), g, bold);

                                            buildString = null;
                                            buildString = new StringBuilder();
                                        }

                                        isInSelection = true;
                                        curSelectionLine = lineCounter;

                                        oldForeColor = curForeColor;
                                        oldBackColor = curBackColor;

                                        curForeColor = FormMain.Instance.IceChatColors.TextSelectForeColor;
                                        curBackColor = FormMain.Instance.IceChatColors.TextSelectBackColor;

                                    }
                                    else if (_displayLines[curLine].selectionX2 == curChar && isInSelection)
                                    {
                                        if (buildString.Length > 0)
                                        {
                                            if (curBackColor != _backColor)
                                            {
                                                textSize = MeasureString(buildString.ToString(), g, bold);
                                                Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                                g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                                            }
                                            g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);

                                            startX += MeasureString(buildString.ToString(), g, bold);

                                            buildString = null;
                                            buildString = new StringBuilder();
                                        }

                                        isInSelection = false;

                                        //if (isInUrl)
                                        //    curForeColor = FormMain.Instance.IceChatColors.HyperlinkColor;
                                        //else
                                            curForeColor = oldForeColor;

                                        curBackColor = oldBackColor;

                                    }
                               
                                    curChar++;
                                    buildString.Append(ch[0]);
                                    break;

                            }

                            i++;

                        } while (line.Length > 0 && i < line.Length);
                    }

                    //draw anything that is left over                
                    if (i == line.Length && line.Length > 0)
                    {
                        if (buildString.Length > 0)
                        {
                            if (curBackColor != _backColor)
                            {
                                textSize = MeasureString(buildString.ToString(), g, bold);
                                Rectangle r = new Rectangle((int)startX, startY, (int)textSize + 1, _lineSize + 1);
                                g.FillRectangle(new SolidBrush(IrcColor.colors[curBackColor]), r);
                            }
                            g.DrawString(buildString.ToString(), font, new SolidBrush(IrcColor.colors[curForeColor]), startX, startY, stringFormat);
                        }

                        if (_displayLines[curLine].selectionX2 == curChar && isInSelection)
                        {
                            isInSelection = false;

                            //if (isInUrl)
                            //    curForeColor = FormMain.Instance.IceChatColors.HyperlinkColor;
                            //else
                                curForeColor = oldForeColor;

                            curBackColor = oldBackColor;
                            
                        }
                    }

                    startY += _displayLines[curLine].lineHeight;

                    startX = 0;
                    curLine++;
                    buildString = null;
                    buildString = new StringBuilder();

                }
                buildString = null;

                e.Graphics.DrawImageUnscaled(_buffer, 0, 0);
                //buffer.Dispose();

                g.Dispose();

                if (_reformatLines == true)
                {
                    _reformatLines = false;
                    if (!_singleLine)
                        ScrollToBottom();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OnDisplayText Error:" + ex.Message + ":" + ex.StackTrace);
                System.Diagnostics.Debug.WriteLine(ex.Source);
               
                

                //FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "TextWindow OnDisplayText", ee);
            }
        }

        #region TextWidth and TextSizes Methods

        private string StripColorCodes(string line)
        {
            Regex parseStuff = new Regex("\xFF03[0-9]{4}");
            return parseStuff.Replace(line, "");
        }

        private string StripAllCodes(string line, bool stripBolds)
        {
            if (line == null)
                return "";
            if (line.Length > 0)
            {
                
                // we have to replace the emoticon with the text
                string[] eachEmot = _emotMatch.Split((char)0);
                for (int i = eachEmot.GetLowerBound(0); i <= eachEmot.GetUpperBound(0); i++)
                    line = line.Replace( emotChar + i.ToString("000"), @eachEmot[i]);
                
                
                //Regex parseStuff = new Regex("\xFF03[0-9]{4}|"+emotChar+"|"+urlStart+"|"+urlEnd+"|"+cancelChar+"|"+underlineChar+"|"+reverseChar+"|"+italicChar+"|"+wrapLine+"|"+endLine);
                
                Regex parseStuff = new Regex("\xFF03[0-9]{4}|"+urlStart+"|"+urlEnd+"|"+cancelChar+"|"+underlineChar+"|"+reverseChar+"|"+italicChar+"|"+wrapLine+"|"+endLine);
                line = parseStuff.Replace(line, "");
                
                if (stripBolds)
                {
                    Regex parseStuff2 = new Regex("\x0002");
                    line = parseStuff2.Replace(line, "");
                }
                return line;
            }
            else
                return "";
        }

        private void LoadTextSizes()
        {
            Graphics g = this.CreateGraphics();

            _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
            _showMaxLines = (this.Height / _lineSize) + 1;
            vScrollBar.LargeChange = _showMaxLines;

            /*
            System.Diagnostics.Debug.WriteLine("LoadTextSize:" + _lineSize);

            //g.MeasureCharacterRanges(
            float[] chars = new float[65536];
            float[] charsB = new float[65536];
            for (int i = 1; i < 65535; i++)
                chars[i] = g.MeasureString(((char)i).ToString(), this.Font).Width;

            Font boldFont2 = new Font(this.Font, FontStyle.Bold);

            for (int i = 1; i < 65535; i++)
                charsB[i] = g.MeasureString(((char)i).ToString(), boldFont2).Width;

            //set these chars to 0 length
            chars[2] = 0;
            chars[3] = 0;
            chars[15] = 0;
            chars[31] = 0;
            chars[22] = 0;
            //chars[65283] = 0;
            //chars[65290] = 0;
            //chars[65291] = 0;
            //chars[65292] = 0;
            //chars[65293] = 0;
            //chars[65295] = 0;
            */

            g.Dispose();

        }
        #endregion

        private string ParseUrl(string data)
        {
            Regex re = new Regex(_wwwMatch);
            
            data = re.Replace(data, delegate(Match m)
            {                
                return urlStart + StripColorCodes(m.Value) + urlEnd;
            });

           
            return data;
        }


        private float MeasureString(string data, Graphics g, bool startBold)
        {
            Font boldFont2 = new Font(this.Font, FontStyle.Bold);
            string line = "";
            float size = 0;
            bool isBold = startBold;

            try
            {

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == boldChar || (isBold && data[i] == cancelChar))
                    {
                        size += (float)g.MeasureString(StripAllCodes(line, true), isBold ? boldFont2 : this.Font, 0, stringFormat).Width;
                        isBold = !isBold;
                        line = "";
                    }
                    else
                    {
                        line += data[i];
                    }
                }
                if (line.Length > 0)
                {
                    size += (float)g.MeasureString(StripAllCodes(line, true), isBold ? boldFont2 : this.Font, 0, stringFormat).Width;
                }

            }
            catch (Exception ex)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "TextWindow OnMeasureString:" + data.Length, ex);
            }
            
            return size;
        }

        /// <summary>
        /// Replacement for graphics.MeasureString
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        /*
        private int MeasureStringWidth(Graphics graphics, string text)
        {

            if (text.Length == 0)
                return 0;

            Size size = TextRenderer.MeasureText(graphics, text, new Font(this.Font.Name, this.Font.Size, FontStyle.Regular, GraphicsUnit.Point), new Size(1000,1000), TextFormatFlags.NoPadding);
            //Size size = TextRenderer.MeasureText(graphics, text, this.Font);

            return size.Width + 1;

            System.Drawing.StringFormat format = new System.Drawing.StringFormat(StringFormatFlags.MeasureTrailingSpaces);

            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, 1000, 1000);
            System.Drawing.CharacterRange[] ranges = { new System.Drawing.CharacterRange(0, text.Length) };
            System.Drawing.Region[] regions = new System.Drawing.Region[1];

            format.SetMeasurableCharacterRanges(ranges);

            try
            {
                regions = graphics.MeasureCharacterRanges(text, this.Font, rect, format);                
                rect = regions[0].GetBounds(graphics);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("regions:" + regions.Length + ":" + text.Length + ":" + rect.Right + ":" + rect.Left + ":" + text);
            }
            //return Convert.ToInt32(Math.Round(rect.Right - rect.Left)); 
        }
        */
    }

}
