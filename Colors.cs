/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2014 Paul Vanderzee <snerf@icechat.net>
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
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace IceChat
{

    [XmlRoot("IceChatColors")]
    public class IceChatColors
    {
        //default color values
        private int _channelOwnerColor = 4;
        private int _channelAdminColor = 4;
        private int _channelOpColor = 4;
        private int _channelHalfOpColor = 7;
        private int _channelVoiceColor = 9;
        private int _channelRegularColor = 0;
        private int _channelAwayColor = 8;

        private int _tabBarCurrent = 0;
        private int _tabBarNewMessage = 4;
        private int _tabBarChannelJoin = 29;
        private int _tabBarChannelPart = 46;
        private int _tabBarServerQuit = 9;
        private int _tabBarServerMessage = 13;
        private int _tabBarOtherMessage = 6;
        private int _tabBarDefault = 0;

        private int _tabBarNewAction = 4;
        private int _tabBarServerNotice = 13;
        private int _tabBarBuddyNotice = 4;

        private int _tabBarCurrentBG1 = 29;
        private int _tabBarCurrentBG2 = 0;
        private int _tabBarOtherBG1 = 14;
        private int _tabBarOtherBG2 = 0;
        private int _tabBarHoverBG1 = 0;
        private int _tabBarHoverBG2 = 7;
        private int _tabbarBack = 20;

        private int _panelHeaderBGColor1 = 15;
        private int _panelHeaderBGColor2 = 0;
        private int _panelHeaderForeColor = 1;

        private int _consoleBack = 1;
        private int _channelBack = 1;
        private int _queryBack = 1;
        private int _nicklistBack = 1;
        private int _serverlistBack = 1;
        private int _inputboxBack = 1;
        private int _inputboxFore = 0;
        private int _channellistBack = 1;
        private int _channellistFore = 0;
        private int _menubarBack = 20;
        private int _toolbarBack = 20;
        private int _statusbarBack = 20;
        private int _statusbarFore = 1;
        private int _unreadTextMarker = 4;
        private int _hyperLinkColor = 4;
        private int _textSelectFore = 0;
        private int _textSelectBack = 2;


        private bool _randomizeNickColors = false;
        private bool _newMessageEnabled = true;
        private bool _channelJoinEnabled = true;
        private bool _channelPartEnabled = true;
        private bool _serverQuitEnabled = true;
        private bool _serverMessageEnabled = true;
        private bool _otherMessageEnabled = true;

        [XmlElement("ConsoleBackColor")]
        public int ConsoleBackColor
        { get { return _consoleBack; } set { _consoleBack = value; } }

        [XmlElement("ChannelBackColor")]
        public int ChannelBackColor
        { get { return _channelBack; } set { _channelBack = value; } }

        [XmlElement("QueryBackColor")]
        public int QueryBackColor
        { get { return _queryBack; } set { _queryBack = value; } }

        [XmlElement("NickListBackColor")]
        public int NickListBackColor
        { get { return _nicklistBack; } set { _nicklistBack = value; } }

        [XmlElement("ServerListBackColor")]
        public int ServerListBackColor
        { get { return _serverlistBack; } set { _serverlistBack = value; } }

        [XmlElement("ChannelListBackColor")]
        public int ChannelListBackColor
        { get { return _channellistBack; } set { _channellistBack = value; } }

        [XmlElement("ChannelListForeColor")]
        public int ChannelListForeColor
        { get { return _channellistFore; } set { _channellistFore = value; } }

        [XmlElement("InputboxBackColor")]
        public int InputboxBackColor
        { get { return _inputboxBack; } set { _inputboxBack = value; } }

        [XmlElement("InputboxForeColor")]
        public int InputboxForeColor
        { get { return _inputboxFore; } set { _inputboxFore = value; } }

        [XmlElement("TabbarBackColor")]
        public int TabbarBackColor
        { get { return _tabbarBack; } set { _tabbarBack = value; } }

        [XmlElement("MenubarBackColor")]
        public int MenubarBackColor
        { get { return _menubarBack; } set { _menubarBack = value; } }

        [XmlElement("ToolbarBackColor")]
        public int ToolbarBackColor
        { get { return _toolbarBack; } set { _toolbarBack = value; } }

        [XmlElement("StatusbarBackColor")]
        public int StatusbarBackColor
        { get { return _statusbarBack; } set { _statusbarBack = value; } }

        [XmlElement("StatusbarForeColor")]
        public int StatusbarForeColor
        { get { return _statusbarFore; } set { _statusbarFore = value; } }

        [XmlElement("ChannelOwnerColor")]
        public int ChannelOwnerColor
        { get { return _channelOwnerColor; } set { _channelOwnerColor = value; } }

        [XmlElement("ChannelAdminColor")]
        public int ChannelAdminColor
        { get { return this._channelAdminColor; } set { this._channelAdminColor = value; } }

        [XmlElement("ChannelOpColor")]
        public int ChannelOpColor
        { get { return this._channelOpColor; } set { this._channelOpColor = value; } }

        [XmlElement("ChannelHalfOpColor")]
        public int ChannelHalfOpColor
        { get { return this._channelHalfOpColor; } set { this._channelHalfOpColor = value; } }

        [XmlElement("ChannelVoiceColor")]
        public int ChannelVoiceColor
        { get { return this._channelVoiceColor; } set { this._channelVoiceColor = value; } }

        [XmlElement("ChannelRegularColor")]
        public int ChannelRegularColor
        { get { return this._channelRegularColor; } set { this._channelRegularColor = value; } }

        [XmlElement("ChannelAwayColor")]
        public int ChannelAwayColor
        { get { return this._channelAwayColor; } set { this._channelAwayColor = value; } }

        [XmlElement("TabBarCurrent")]
        public int TabBarCurrent
        { get { return _tabBarCurrent; } set { _tabBarCurrent = value; } }

        //new messages and actions
        [XmlElement("TabBarNewMessage")]
        public int TabBarNewMessage
        { get { return _tabBarNewMessage; } set { _tabBarNewMessage = value; } }

        [XmlElement("TabBarChannelJoin")]
        public int TabBarChannelJoin
        { get { return _tabBarChannelJoin; } set { _tabBarChannelJoin = value; } }

        [XmlElement("TabBarChannelPart")]
        public int TabBarChannelPart
        { get { return _tabBarChannelPart; } set { _tabBarChannelPart = value; } }

        [XmlElement("TabBarServerQuit")]
        public int TabBarServerQuit
        { get { return _tabBarServerQuit; } set { _tabBarServerQuit = value; } }

        [XmlElement("TabBarServerMessage")]
        public int TabBarServerMessage
        { get { return _tabBarServerMessage; } set { _tabBarServerMessage = value; } }

        [XmlElement("TabBarOtherMessage")]
        public int TabBarOtherMessage
        { get { return _tabBarOtherMessage; } set { _tabBarOtherMessage = value; } }

        [XmlElement("TabBarNewAction")]
        public int TabBarNewAction
        { get { return _tabBarNewAction; } set { _tabBarNewAction = value; } }

        [XmlElement("TabBarServerNotice")]
        public int TabBarServerNotice
        { get { return _tabBarServerNotice; } set { _tabBarServerNotice = value; } }

        [XmlElement("TabBarBuddyNotice")]
        public int TabBarBuddyNotice
        { get { return _tabBarBuddyNotice; } set { _tabBarBuddyNotice = value; } }

        [XmlElement("TabBarDefault")]
        public int TabBarDefault
        { get { return _tabBarDefault; } set { _tabBarDefault = value; } }

        [XmlElement("TabBarCurrentBG1")]
        public int TabBarCurrentBG1
        { get { return _tabBarCurrentBG1; } set { _tabBarCurrentBG1 = value; } }

        [XmlElement("TabBarCurrentBG2")]
        public int TabBarCurrentBG2
        { get { return _tabBarCurrentBG2; } set { _tabBarCurrentBG2 = value; } }

        [XmlElement("TabBarOtherBG1")]
        public int TabBarOtherBG1
        { get { return _tabBarOtherBG1; } set { _tabBarOtherBG1 = value; } }

        [XmlElement("TabBarOtherBG2")]
        public int TabBarOtherBG2
        { get { return _tabBarOtherBG2; } set { _tabBarOtherBG2 = value; } }

        [XmlElement("TabBarHoverBG1")]
        public int TabBarHoverBG1
        { get { return _tabBarHoverBG1; } set { _tabBarHoverBG1 = value; } }

        [XmlElement("TabBarHoverBG2")]
        public int TabBarHoverBG2
        { get { return _tabBarHoverBG2; } set { _tabBarHoverBG2 = value; } }

        [XmlElement("PanelHeaderBG1")]
        public int PanelHeaderBG1
        { get { return _panelHeaderBGColor1; } set { _panelHeaderBGColor1 = value; } }

        [XmlElement("PanelHeaderBG2")]
        public int PanelHeaderBG2
        { get { return _panelHeaderBGColor2; } set { _panelHeaderBGColor2 = value; } }

        [XmlElement("PanelHeaderForeColor")]
        public int PanelHeaderForeColor
        { get { return _panelHeaderForeColor; } set { _panelHeaderForeColor = value; } }

        [XmlElement("UnreadTextMarker")]
        public int UnreadTextMarkerColor
        { get { return this._unreadTextMarker; } set { this._unreadTextMarker = value; } }

        [XmlElement("HyperlinkColor")]
        public int HyperlinkColor
        { get { return this._hyperLinkColor; } set { this._hyperLinkColor = value; } }

        [XmlElement("TextSelectForeColor")]
        public int TextSelectForeColor
        { get { return this._textSelectFore; } set { this._textSelectFore = value; } }

        [XmlElement("TextSelectBackColor")]
        public int TextSelectBackColor
        { get { return this._textSelectBack; } set { this._textSelectBack = value; } }

        [XmlElement("RandomizeNickColors")]
        public bool RandomizeNickColors
        { get { return this._randomizeNickColors; } set { this._randomizeNickColors = value; } }

        [XmlElement("NewMessageColorChange")]
        public bool NewMessageColorChange
        { get { return this._newMessageEnabled; } set { this._newMessageEnabled = value; } }

        [XmlElement("ChannelJoinColorChange")]
        public bool ChannelJoinColorChange
        { get { return this._channelJoinEnabled; } set { this._channelJoinEnabled = value; } }

        [XmlElement("ChannePartColorChange")]
        public bool ChannelPartColorChange
        { get { return this._channelPartEnabled; } set { this._channelPartEnabled = value; } }

        [XmlElement("ServerQuitColorChange")]
        public bool ServerQuitColorChange
        { get { return this._serverQuitEnabled; } set { this._serverQuitEnabled = value; } }

        [XmlElement("ServerMessageColorChange")]
        public bool ServerMessageColorChange
        { get { return this._serverMessageEnabled; } set { this._serverMessageEnabled = value; } }

        [XmlElement("OtherMessageColorChange")]
        public bool OtherMessageColorChange
        { get { return this._otherMessageEnabled; } set { this._otherMessageEnabled = value; } }

    }
   
    [XmlRoot("IceChatColorPalette")]
    public class IceChatColorPalette
    {
        [XmlArray("Colors")]
        [XmlArrayItem("HTMLColor",typeof(String))]
        public List<String> listColors;

        public IceChatColorPalette() 
        {
            listColors = new List<String>();
        }
    }
    
    public static class IrcColor
    {
        public static Color[] colors;

        static IrcColor()
        {
            //Color color;
            colors = new Color[72];

            //load color settings from color xml file
            if (FormMain.Instance.ColorPalette.listColors.Count == 72)
            {
                int i = 0;
                foreach (String color in FormMain.Instance.ColorPalette.listColors)
                {
                    colors[i] = System.Drawing.ColorTranslator.FromHtml(color);
                    i++;
                }
            }
            else
            {
                //load default colors if 72 colors are not defined
                colors[0] = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                colors[1] = System.Drawing.ColorTranslator.FromHtml("#000000");
                colors[2] = System.Drawing.ColorTranslator.FromHtml("#00007F");
                colors[3] = System.Drawing.ColorTranslator.FromHtml("#009300");
                colors[4] = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                colors[5] = System.Drawing.ColorTranslator.FromHtml("#7F0000");
                colors[6] = System.Drawing.ColorTranslator.FromHtml("#9C009C");
                colors[7] = System.Drawing.ColorTranslator.FromHtml("#FC7F00");

                colors[8] = System.Drawing.ColorTranslator.FromHtml("#FFFF00");
                colors[9] = System.Drawing.ColorTranslator.FromHtml("#00FC00");
                colors[10] = System.Drawing.ColorTranslator.FromHtml("#009393");
                colors[11] = System.Drawing.ColorTranslator.FromHtml("#00FFFF");
                colors[12] = System.Drawing.ColorTranslator.FromHtml("#0000FC");
                colors[13] = System.Drawing.ColorTranslator.FromHtml("#FF00FF");
                colors[14] = System.Drawing.ColorTranslator.FromHtml("#7F7F7F");
                colors[15] = System.Drawing.ColorTranslator.FromHtml("#D2D2D2");
                
                
                colors[16] = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");
                colors[17] = System.Drawing.ColorTranslator.FromHtml("#0066FF");
                colors[18] = System.Drawing.ColorTranslator.FromHtml("#FAEBD7");
                colors[19] = System.Drawing.ColorTranslator.FromHtml("#FFD700");
                colors[20] = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
                colors[21] = System.Drawing.ColorTranslator.FromHtml("#4682B4");
                colors[22] = System.Drawing.ColorTranslator.FromHtml("#993333");
                colors[23] = System.Drawing.ColorTranslator.FromHtml("#FF99FF");

                colors[24] = System.Drawing.ColorTranslator.FromHtml("#DDA0DD");
                colors[25] = System.Drawing.ColorTranslator.FromHtml("#8B4513");
                colors[26] = System.Drawing.ColorTranslator.FromHtml("#CC0000");
                colors[27] = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                colors[28] = System.Drawing.ColorTranslator.FromHtml("#339900");
                colors[29] = System.Drawing.ColorTranslator.FromHtml("#FF9900");

                colors[30] = System.Drawing.ColorTranslator.FromHtml("#FFDAB9");
                colors[31] = System.Drawing.ColorTranslator.FromHtml("#2F4F4F");
                colors[32] = System.Drawing.ColorTranslator.FromHtml("#ECE9D8");
                colors[33] = System.Drawing.ColorTranslator.FromHtml("#5FDAEE");
                colors[34] = System.Drawing.ColorTranslator.FromHtml("#E2FF00");
                colors[35] = System.Drawing.ColorTranslator.FromHtml("#00009E");

                colors[36] = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                colors[37] = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                colors[38] = System.Drawing.ColorTranslator.FromHtml("#FFFF66");
                colors[39] = System.Drawing.ColorTranslator.FromHtml("#FFCC33");
                colors[40] = System.Drawing.ColorTranslator.FromHtml("#FF9933");
                colors[41] = System.Drawing.ColorTranslator.FromHtml("#FF6633");

                colors[42] = System.Drawing.ColorTranslator.FromHtml("#c6ffc6");
                colors[43] = System.Drawing.ColorTranslator.FromHtml("#84ff84");
                colors[44] = System.Drawing.ColorTranslator.FromHtml("#00ff00");
                colors[45] = System.Drawing.ColorTranslator.FromHtml("#00c700");
                colors[46] = System.Drawing.ColorTranslator.FromHtml("#008600");
                colors[47] = System.Drawing.ColorTranslator.FromHtml("#004100");

                //blues
                colors[48] = System.Drawing.ColorTranslator.FromHtml("#C6FFFF");
                colors[49] = System.Drawing.ColorTranslator.FromHtml("#84FFFF");
                colors[50] = System.Drawing.ColorTranslator.FromHtml("#00FFFF");
                colors[51] = System.Drawing.ColorTranslator.FromHtml("#6699FF");
                colors[52] = System.Drawing.ColorTranslator.FromHtml("#6666FF");
                colors[53] = System.Drawing.ColorTranslator.FromHtml("#3300FF");



                //reds
                colors[54] = System.Drawing.ColorTranslator.FromHtml("#FFCC99");
                colors[55] = System.Drawing.ColorTranslator.FromHtml("#FF9966");
                colors[56] = System.Drawing.ColorTranslator.FromHtml("#ff6633");
                colors[57] = System.Drawing.ColorTranslator.FromHtml("#FF0033");
                colors[58] = System.Drawing.ColorTranslator.FromHtml("#CC0000");
                colors[59] = System.Drawing.ColorTranslator.FromHtml("#AA0000");


                //pink / purple
                colors[60] = System.Drawing.ColorTranslator.FromHtml("#ffc7ff");
                colors[61] = System.Drawing.ColorTranslator.FromHtml("#ff86ff");
                colors[62] = System.Drawing.ColorTranslator.FromHtml("#ff00ff");
                colors[63] = System.Drawing.ColorTranslator.FromHtml("#FF00CC");
                colors[64] = System.Drawing.ColorTranslator.FromHtml("#CC0099");
                colors[65] = System.Drawing.ColorTranslator.FromHtml("#660099");


                //gray scale
                colors[66] = System.Drawing.ColorTranslator.FromHtml("#EEEEEE");
                colors[67] = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                colors[68] = System.Drawing.ColorTranslator.FromHtml("#AAAAAA");
                colors[69] = System.Drawing.ColorTranslator.FromHtml("#888888");
                colors[70] = System.Drawing.ColorTranslator.FromHtml("#666666");
                colors[71] = System.Drawing.ColorTranslator.FromHtml("#444444");
            }
        }
    }
}
