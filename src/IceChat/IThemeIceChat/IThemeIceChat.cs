/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2022 Paul Vanderzee <snerf@icechat.net>
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

namespace IceChatTheme
{
    public abstract class IThemeIceChat
    {
        private int _channelOwnerColor = 4;
        private int _channelAdminColor = 4;
        private int _channelOpColor = 12;
        private int _channelHalfOpColor = 7;
        private int _channelVoiceColor = 9;
        private int _channelRegularColor = 1;

        private int _tabBarCurrent = 1;
        private int _tabBarNewMessage = 4;
        private int _tabBarChannelJoin = 3;
        private int _tabBarChannelPart = 7;
        private int _tabBarServerQuit = 10;
        private int _tabBarServerMessage = 13;
        private int _tabBarOtherMessage = 6;
        private int _tabBarDefault = 1;
        private int _tabBarNewAction = 4;
        private int _tabBarServerNotice = 13;
        private int _tabBarBuddyNotice = 4;

        private int _panelHeaderBGColor1 = 15;
        private int _panelHeaderBGColor2 = 0;
        private int _panelHeaderForeColor = 1;

        private int _consoleBack = 0;
        private int _channelBack = 0;
        private int _queryBack = 0;
        private int _nicklistBack = 0;
        private int _serverlistBack = 0;
        private int _inputboxBack = 0;
        private int _inputboxFore = 1;
        private int _channellistBack = 0;
        private int _channellistFore = 1;
        private int _menubarBack = 20;
        private int _toolbarBack = 20;
        private int _statusbarBack = 20;
        private int _statusbarFore = 1;
        private int _unreadTextMarker = 4;

        private int _sideBarButtons = 67;
        private int _sideBarForeButtons = 1;

        private bool _randomizeNickColors = false;
        private bool _newMessageEnabled = true;
        private bool _channelJoinEnabled = true;
        private bool _channelPartEnabled = true;
        private bool _serverQuitEnabled = true;
        private bool _serverMessageEnabled = true;
        private bool _otherMessageEnabled = true;

        //private IceChatMessageFormat iceChatMessages = new IceChatMessageFormat();
        
        public void Initialize()
        {
            /*
            iceChatMessages.MessageSettings = new ServerMessageFormatItem[49];
            iceChatMessages.MessageSettings[0] = NewMessageFormat("Server Connect", "&#x3;1*** Attempting to connect to $server ($serverip) on port $port");
            iceChatMessages.MessageSettings[1] = NewMessageFormat("Server Disconnect", "&#x3;1*** Server disconnected on $server");
            iceChatMessages.MessageSettings[2] = NewMessageFormat("Server Reconnect", "&#x3;1*** Attempting to re-connect to $server");
            iceChatMessages.MessageSettings[3] = NewMessageFormat("Channel Invite", "&#x3;3* $nick invites you to $channel");
            iceChatMessages.MessageSettings[7] = NewMessageFormat("Channel Mode", "&#x3;9* $nick sets mode $mode $modeparam for $channel");
            iceChatMessages.MessageSettings[8] = NewMessageFormat("Server Mode", "&#x3;6* Your mode is now $mode");
            iceChatMessages.MessageSettings[9] = NewMessageFormat("Server Notice", "&#x3;4*** $server $message");
            iceChatMessages.MessageSettings[10] = NewMessageFormat("Server Message", "&#x3;4-$server- $message");
            iceChatMessages.MessageSettings[11] = NewMessageFormat("User Notice", "&#x3;4--$nick-- $message");
            iceChatMessages.MessageSettings[12] = NewMessageFormat("Channel Message", "&#x3;1<$color$status$nick&#x3;> $message");
            iceChatMessages.MessageSettings[13] = NewMessageFormat("Self Channel Message", "&#x3;1<$nick&#x3;> $message");
            iceChatMessages.MessageSettings[14] = NewMessageFormat("Channel Action", "&#x3;5* $nick $message");
            iceChatMessages.MessageSettings[15] = NewMessageFormat("Self Channel Action", "&#x3;4* $nick $message");
            iceChatMessages.MessageSettings[16] = NewMessageFormat("Channel Join", "&#x3;7* $nick ($host) has joined channel $channel");
            iceChatMessages.MessageSettings[17] = NewMessageFormat("Self Channel Join", "&#x3;4* You have joined $channel");
            iceChatMessages.MessageSettings[18] = NewMessageFormat("Channel Part", "&#x3;3* $nick ($host) has left $channel ($reason)");
            iceChatMessages.MessageSettings[19] = NewMessageFormat("Self Channel Part", "&#x3;4* You have left $channel - You will be missed &#x3;10($reason)");
            iceChatMessages.MessageSettings[20] = NewMessageFormat("Server Quit", "&#x3;2* $nick ($host) Quit ($reason)");
            iceChatMessages.MessageSettings[21] = NewMessageFormat("Channel Nick Change", "&#x3;3* $nick is now known as $newnick");
            iceChatMessages.MessageSettings[22] = NewMessageFormat("Self Nick Change", "&#x3;4* You are now known as $newnick");
            iceChatMessages.MessageSettings[23] = NewMessageFormat("Channel Kick", "&#x3;4* $kickee was kicked by $nick($host) &#x3;3 - Reason ($reason)");
            iceChatMessages.MessageSettings[24] = NewMessageFormat("Self Channel Kick", "&#x3;4* You were kicked from $channel by $kicker (&#x3;3$reason)");
            iceChatMessages.MessageSettings[25] = NewMessageFormat("Private Message", "&#x3;1<$nick> $message");
            iceChatMessages.MessageSettings[26] = NewMessageFormat("Self Private Message", "&#x3;4<$nick>&#x3;1 $message");
            iceChatMessages.MessageSettings[27] = NewMessageFormat("Private Action", "&#x3;13* $nick $message");
            iceChatMessages.MessageSettings[28] = NewMessageFormat("Self Private Action", "&#x3;12* $nick $message");
            iceChatMessages.MessageSettings[35] = NewMessageFormat("Channel Topic Change", "&#x3;3* $nick changes topic to: $topic");
            iceChatMessages.MessageSettings[36] = NewMessageFormat("Channel Topic Text", "&#x3;3Topic: $topic");
            iceChatMessages.MessageSettings[37] = NewMessageFormat("Server MOTD", "&#x3;3$message");
            iceChatMessages.MessageSettings[38] = NewMessageFormat("Channel Notice", "&#x3;5-$nick:$status$channel- $message");
            iceChatMessages.MessageSettings[39] = NewMessageFormat("Channel Other", "&#x3;1$message");
            iceChatMessages.MessageSettings[40] = NewMessageFormat("User Echo", "&#x3;7$message");
            iceChatMessages.MessageSettings[41] = NewMessageFormat("Server Error", "&#x3;4ERROR: $message");
            iceChatMessages.MessageSettings[42] = NewMessageFormat("User Whois", "&#x3;12->> $nick $data");
            iceChatMessages.MessageSettings[43] = NewMessageFormat("User Error", "&#x3;4ERROR: $message");
            iceChatMessages.MessageSettings[44] = NewMessageFormat("DCC Chat Connect", "&#x3;1* DCC Chat Connection Established with $nick");
            iceChatMessages.MessageSettings[45] = NewMessageFormat("DCC Chat Disconnect", "&#x3;1* DCC Chat Disconnected from $nick");
            iceChatMessages.MessageSettings[48] = NewMessageFormat("Self Notice", "&#x3;1--> $nick - $message");

            iceChatMessages.MessageSettings[4] = NewMessageFormat("Ctcp Reply", "&#x3;12[$nick $ctcp Reply] : $reply");
            iceChatMessages.MessageSettings[5] = NewMessageFormat("Ctcp Send", "&#x3;10--> [$nick] $ctcp");
            iceChatMessages.MessageSettings[6] = NewMessageFormat("Ctcp Request", "&#x3;7[$nick] $ctcp");

            iceChatMessages.MessageSettings[29] = NewMessageFormat("DCC Chat Action", "&#x3;5* $nick $message");
            iceChatMessages.MessageSettings[30] = NewMessageFormat("Self DCC Chat Action", "&#x3;5* $nick $message");
            iceChatMessages.MessageSettings[31] = NewMessageFormat("DCC Chat Message", "&#x3;1<$nick> $message");
            iceChatMessages.MessageSettings[32] = NewMessageFormat("Self DCC Chat Message", "&#x3;4<$nick> $message");

            iceChatMessages.MessageSettings[33] = NewMessageFormat("DCC Chat Request", "&#x3;4* $nick ($host) is requesting a DCC Chat");
            iceChatMessages.MessageSettings[34] = NewMessageFormat("DCC File Send", "&#x3;4* $nick ($host) is trying to send you a file ($file) [$filesize bytes]");

            iceChatMessages.MessageSettings[46] = NewMessageFormat("DCC Chat Outgoing", "&#x3;1* DCC Chat Requested with $nick");
            iceChatMessages.MessageSettings[47] = NewMessageFormat("DCC Chat Timeout", "&#x3;1* DCC Chat with $nick timed out");
            */
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


        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract string Author { get; }
        
        //if the plugin is enabled or disabled
        public bool Enabled { get; set; }
        public string FileName { get; set; }

        //public virtual IceChatMessageFormat MessageFormat
        //{ get { return iceChatMessages; } set { iceChatMessages = value; } }

        public virtual int ConsoleBackColor
        { get { return _consoleBack; } set { _consoleBack = value; } }

        public virtual int ChannelBackColor
        { get { return _channelBack; } set { _channelBack = value; } }

        public virtual int QueryBackColor
        { get { return _queryBack; } set { _queryBack = value; } }

        public virtual int NickListBackColor
        { get { return _nicklistBack; } set { _nicklistBack = value; } }

        public virtual int ServerListBackColor
        { get { return _serverlistBack; } set { _serverlistBack = value; } }

        public virtual int ChannelListBackColor
        { get { return _channellistBack; } set { _channellistBack = value; } }

        public virtual int ChannelListForeColor
        { get { return _channellistFore; } set { _channellistFore = value; } }

        public virtual int InputboxBackColor
        { get { return _inputboxBack; } set { _inputboxBack = value; } }

        public virtual int InputboxForeColor
        { get { return _inputboxFore; } set { _inputboxFore = value; } }

        public virtual int MenubarBackColor
        { get { return _menubarBack; } set { _menubarBack = value; } }

        public virtual int ToolbarBackColor
        { get { return _toolbarBack; } set { _toolbarBack = value; } }

        public virtual int StatusbarBackColor
        { get { return _statusbarBack; } set { _statusbarBack = value; } }

        public virtual int StatusbarForeColor
        { get { return _statusbarFore; } set { _statusbarFore = value; } }

        public virtual int SideBarButtonsBackColor
        { get { return _sideBarButtons; } set { _sideBarButtons = value; } }

        public virtual int SideBarButtonsForeColor
        { get { return _sideBarForeButtons; } set { _sideBarForeButtons = value; } }

        public virtual int ChannelOwnerColor
        { get { return _channelOwnerColor; } set { _channelOwnerColor = value; } }

        public virtual int ChannelAdminColor
        { get { return this._channelAdminColor; } set { this._channelAdminColor = value; } }

        public virtual int ChannelOpColor
        { get { return this._channelOpColor; } set { this._channelOpColor = value; } }

        public virtual int ChannelHalfOpColor
        { get { return this._channelHalfOpColor; } set { this._channelHalfOpColor = value; } }

        public virtual int ChannelVoiceColor
        { get { return this._channelVoiceColor; } set { this._channelVoiceColor = value; } }

        public virtual int ChannelRegularColor
        { get { return this._channelRegularColor; } set { this._channelRegularColor = value; } }

        public virtual int TabBarCurrent
        { get { return _tabBarCurrent; } set { _tabBarCurrent = value; } }

        //new messages and actions
        public virtual int TabBarNewMessage
        { get { return _tabBarNewMessage; } set { _tabBarNewMessage = value; } }

        public virtual int TabBarChannelJoin
        { get { return _tabBarChannelJoin; } set { _tabBarChannelJoin = value; } }

        public virtual int TabBarChannelPart
        { get { return _tabBarChannelPart; } set { _tabBarChannelPart = value; } }

        public virtual int TabBarServerQuit
        { get { return _tabBarServerQuit; } set { _tabBarServerQuit = value; } }

        public virtual int TabBarServerMessage
        { get { return _tabBarServerMessage; } set { _tabBarServerMessage = value; } }

        public virtual int TabBarOtherMessage
        { get { return _tabBarOtherMessage; } set { _tabBarOtherMessage = value; } }

        public virtual int TabBarNewAction
        { get { return _tabBarNewAction; } set { _tabBarNewAction = value; } }

        public virtual int TabBarServerNotice
        { get { return _tabBarServerNotice; } set { _tabBarServerNotice = value; } }

        public virtual int TabBarBuddyNotice
        { get { return _tabBarBuddyNotice; } set { _tabBarBuddyNotice = value; } }

        public virtual int TabBarDefault
        { get { return _tabBarDefault; } set { _tabBarDefault = value; } }

        public virtual int PanelHeaderBG1
        { get { return _panelHeaderBGColor1; } set { _panelHeaderBGColor1 = value; } }

        public virtual int PanelHeaderBG2
        { get { return _panelHeaderBGColor2; } set { _panelHeaderBGColor2 = value; } }

        public virtual int PanelHeaderForeColor
        { get { return _panelHeaderForeColor; } set { _panelHeaderForeColor = value; } }

        public virtual int UnreadTextMarkerColor
        { get { return this._unreadTextMarker; } set { this._unreadTextMarker = value; } }

        public virtual bool RandomizeNickColors
        { get { return this._randomizeNickColors; } set { this._randomizeNickColors = value; } }

        public virtual bool NewMessageColorChange
        { get { return this._newMessageEnabled; } set { this._newMessageEnabled = value; } }

        public virtual bool ChannelJoinColorChange
        { get { return this._channelJoinEnabled; } set { this._channelJoinEnabled = value; } }

        public virtual bool ChannelPartColorChange
        { get { return this._channelPartEnabled; } set { this._channelPartEnabled = value; } }

        public virtual bool ServerQuitColorChange
        { get { return this._serverQuitEnabled; } set { this._serverQuitEnabled = value; } }

        public virtual bool ServerMessageColorChange
        { get { return this._serverMessageEnabled; } set { this._serverMessageEnabled = value; } }

        public virtual bool OtherMessageColorChange
        { get { return this._otherMessageEnabled; } set { this._otherMessageEnabled = value; } }




    }

    public class IceChatMessageFormat
    {
        public ServerMessageFormatItem[] MessageSettings
        { get; set; }

    }

    public class ServerMessageFormatItem
    {
        public string MessageName;
        public string FormattedMessage;
    }

}
