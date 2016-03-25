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
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace IceChat
{
    public partial class IRCConnection : IDisposable
    {

        public event OutGoingCommandDelegate OutGoingCommand;

        public event ChannelMessageDelegate ChannelMessage;        
        public event ChannelActionDelegate ChannelAction;
        public event QueryMessageDelegate QueryMessage;
        public event QueryActionDelegate QueryAction;

        public event GenericChannelMessageDelegate GenericChannelMessage;

        public event ChangeNickDelegate ChangeNick;
        public event JoinChannelDelegate JoinChannel;
        public event PartChannelDelegate PartChannel;
        public event QuitServerDelegate QuitServer;
        public event ChannelNoticeDelegate ChannelNotice;

        public event ChannelKickDelegate ChannelKick;
        public event ChannelKickSelfDelegate ChannelKickSelf;

        public event ChannelTopicDelegate ChannelTopic;

        public event ChannelModeChangeDelegate ChannelMode;
        public event UserModeChangeDelegate UserMode;

        public event ChannelInviteDelegate ChannelInvite;
        public event UserHostReplyDelegate UserHostReply;
        public event JoinChannelMyselfDelegate JoinChannelMyself;
        public event PartChannelMyselfDelegate PartChannelMyself;

        public event ServerMessageDelegate ServerMessage;
        public event ServerMOTDDelegate ServerMOTD;
        public event ServerErrorDelegate ServerError;
        public event WhoisDataDelegate WhoisData;
        public event CtcpMessageDelegate CtcpMessage;
        public event CtcpReplyDelegate CtcpReply;
        public event UserNoticeDelegate UserNotice;

        public event ServerNoticeDelegate ServerNotice;

        public event ChannelListStartDelegate ChannelListStart;
        public event ChannelListEndDelegate ChannelListEnd;
        public event ChannelListDelegate ChannelList;

        public event DCCChatDelegate DCCChat;
        public event DCCFileDelegate DCCFile;
        public event DCCPassiveDelegate DCCPassive;

        public event RawServerIncomingDataDelegate RawServerIncomingData;
        public event RawServerOutgoingDataDelegate RawServerOutgoingData;

        public event RawServerIncomingDataOverRideDelegate RawServerIncomingDataOverRide;

        public event IALUserDataDelegate IALUserData;
        public event IALUserDataAwayOnlyDelegate IALUserDataAwayOnly;
        public event IALUserChangeDelegate IALUserChange;
        public event IALUserPartDelegate IALUserPart;
        public event IALUserQuitDelegate IALUserQuit;

        public event BuddyListDelegate BuddyListData;
        public event BuddyListClearDelegate BuddyListClear;
        public event BuddyRemoveDelegate BuddyRemove;
        public event MonitorListDelegate MonitorListData;

        public event AutoJoinDelegate AutoJoin;
        public event AutoRejoinDelegate AutoRejoin;
        public event AutoPerformDelegate AutoPerform;
        public event EndofNamesDelegate EndofNames;
        public event EndofWhoReplyDelegate EndofWhoReply;
        public event WhoReplyDelegate WhoReply;
        public event ChannelUserListDelegate ChannelUserList;

        public event StatusTextDelegate StatusText;

        public event ChannelInfoWindowExistsDelegate ChannelInfoWindowExists;
        public event ChannelInfoAddBanDelegate ChannelInfoAddBan;
        public event ChannelInfoAddExceptionDelegate ChannelInfoAddException;
        public event ChannelInfoTopicSetDelegate ChannelInfoTopicSet;

        public event UserInfoWindowExistsDelegate UserInfoWindowExists;
        public event UserInfoHostFullnameDelegate UserInfoHostFullName;
        public event UserInfoIdleLogonDelegate UserInfoIdleLogon;
        public event UserInfoAddChannelsDelegate UserInfoAddChannels;
        public event UserInfoAwayStatusDelegate UserInfoAwayStatus;
        public event UserInfoServerDelegate UserInfoServer;
        public event UserInfoLoggedInDelegate UserInfoLoggedIn;

        public event RefreshServerTreeDelegate RefreshServerTree;
        public event WriteErrorFileDelegate WriteErrorFile;
        public event ServerReconnectDelegate ServerReconnect;
        public event ServerReconnectDelegate ServerDisconnect;
        public event ServerConnectDelegate ServerConnect;
        public event ServerForceDisconnectDelegate ServerForceDisconnect;
        public event ServerPreConnectDelegate ServerPreConnect;

        public event AutoAwayDelegate AutoAwayTrigger;
        public event ServerForceDisconnectDelegate ServerFullyConnected;

        private bool initialLogon = false;

        public Form UserInfoWindow = null;

        private void ParseData(string data)
        {
            try
            {
                //if using server time, we need to strip this out of ircData
                string[] ircData = data.Split(' ');

                string channel;
                string nick;
                string host;
                string msg;
                string tempValue;
                bool check;
                string serverTimeValue = "";                

                if (RawServerIncomingDataOverRide != null)
                    data = RawServerIncomingDataOverRide(this, data);

                if (RawServerIncomingData != null)
                    RawServerIncomingData(this, data);

                if (serverSetting.UseServerTime)
                {
                    //:@time=2014-03-18T01:55:08.596Z :dickson.freenode.net 
                    if (ircData[0].StartsWith("@time="))
                    {
                        //parse out the server time
                        serverTimeValue = ircData[0].Substring(ircData[0].IndexOf("=") + 1);
                        //remove server time from ircData, and re-split
                        data = data.Substring(ircData[0].Length + 1);
                        ircData = data.Split(' ');

                        //System.Diagnostics.Debug.WriteLine("Time:" + serverTimeValue);

                    }
                }

                if (data.Length > 4)
                {
                    if (data.Substring(0, 4).ToUpper() == "PING")
                    {
                        SendData("PONG " + ircData[1]);
                        pongTimer.Stop();
                        pongTimer.Start();

                        if (serverSetting.ShowPingPong == true)
                            ServerMessage(this, "Ping? Pong!", serverTimeValue);
                        return;
                    }

                    if (data.Substring(0,12) == "AUTHENTICATE")
                    {
                        //need USERNAME and PASSWORD for SASL Auth
                        if (serverSetting.UseSASL)
                        {
                            if (serverSetting.SASLPass.Length > 0 && serverSetting.SASLUser.Length > 0)
                            {
                                string a = serverSetting.SASLUser + "\0" + serverSetting.SASLUser + "\0" + serverSetting.SASLPass + "\0";
                                byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(a);
                                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

                                SendData("AUTHENTICATE " + returnValue);
                            }
                            else
                                SendData("AUTHENTICATE *");
                        }                            
                    }                            
                }

                if (data.IndexOf(' ') > -1)
                {

                    nick = NickFromFullHost(RemoveColon(ircData[0]));
                    host = HostFromFullHost(RemoveColon(ircData[0]));

                    // A great list of IRC Numerics http://www.mirc.net/raws/

                    switch (ircData[1])
                    {
                        case "001":
                            //get the real server name
                            serverSetting.RealServerName = RemoveColon(ircData[0]);

                            if (serverSetting.NickName != ircData[2])
                            {
                                ChangeNick(this, serverSetting.NickName, ircData[2], HostFromFullHost(ircData[0]), "");

                                if (!serverSetting.TriedAltNick)
                                {
                                    //update only if we have not tried the alt nickname
                                    serverSetting.NickName = ircData[2];
                                }
                                serverSetting.CurrentNickName = ircData[2];
                            }
                            else
                                serverSetting.CurrentNickName = serverSetting.NickName;

                            if (serverSetting.UseBNC)
                                StatusText(this, serverSetting.CurrentNickName + " connected to " + serverSetting.RealServerName + " {BNC}");
                            else
                                StatusText(this, serverSetting.CurrentNickName + " connected to " + serverSetting.RealServerName);

                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);

                            SendData("USERHOST " + serverSetting.CurrentNickName);

                            initialLogon = true;
                            break;
                        case "002":
                        case "003":
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;

                        case "004":
                            ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);
                            //<server> <version> <usermode> <chanmode> <chanmode params> <usermode params> <servermode> <servermode params>

                            //System.Diagnostics.Debug.WriteLine("004:" + ircData.Length);
                            //8 == dioswkgxRXInP biklmnopstvrDcCNuMT bklov
                            
                            string chanModes = "";
                            switch (ircData.Length)
                            {
                                case 7:
                                    // <usermode> <chanmode>
                                    chanModes = ircData[5];
                                    break;
                                case 8:
                                    // <usermode> <chanmode> <chanmode params>
                                    chanModes = ircData[6];
                                    //remove the param chars
                                    foreach(char c in ircData[7])
                                        chanModes = chanModes.Replace(c.ToString(),"");

                                    serverSetting.ChannelModeNoParam = chanModes;
                                    //System.Diagnostics.Debug.WriteLine(chanMode);
                                    
                                    break;

                            }
                            break;
                        case "005":
                            ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);

                            //parse out all the 005 data
                            for (int i = 0; i < ircData.Length; i++)
                            {
                                //parse out all the status modes for user prefixes
                                if (ircData[i].Length >= 7)
                                {
                                    if (ircData[i].StartsWith("PREFIX="))
                                    {
                                        if (ircData[i].IndexOf(')') == -1)
                                        {
                                            //PREFIX=@+ -- just force it                                           
                                            serverSetting.StatusModes = new char[2][];
                                            serverSetting.StatusModes[0] = "ov".ToCharArray();
                                            serverSetting.StatusModes[1] = "@+".ToCharArray();                                            
                                        }
                                        else
                                        {
                                            //PREFIX=(ov)@+
                                            string[] modes = ircData[i].Substring(8).Split(')');
                                            serverSetting.StatusModes = new char[2][];
                                            serverSetting.StatusModes[0] = modes[0].ToCharArray();
                                            serverSetting.StatusModes[1] = modes[1].ToCharArray();
                                        }
                                    }
                                }

                                //add all the channel modes that have parameters into a variable
                                if (ircData[i].Length >= 10)
                                {
                                    if (ircData[i].Substring(0, 10) == "CHANMODES=")
                                    {
                                        //CHANMODES=b,k,l,imnpstrDducCNMT
                                        /*
                                        CHANMODES=A,B,C,D

                                        The CHANMODES token specifies the modes that may be set on a channel.
                                        These modes are split into four categories, as follows:

                                        Type A: Modes that add or remove an address to or from a list.
                                        These modes always take a parameter when sent by the server to a
                                        client; when sent by a client, they may be specified without a
                                        parameter, which requests the server to display the current
                                        contents of the corresponding list on the channel to the client.
                                        
                                        Type B: Modes that change a setting on the channel. These modes
                                        always take a parameter.
                                        
                                        Type C: Modes that change a setting on the channel. These modes
                                        take a parameter only when set; the parameter is absent when the
                                        mode is removed both in the client's and server's MODE command.
                                        
                                        Type D: Modes that change a setting on the channel. These modes
                                        never take a parameter.
                                        
                                        */

                                        //CHANMODES=b,k,l,imnpstrDducCNMT
                                        //CHANMODES=bouv,k,lOMN,cdejimnpqrstzAJLRU
                                        string[] modes = ircData[i].Substring(ircData[i].IndexOf("=") + 1).Split(',');
                                        if (modes.Length == 4)
                                        {
                                            serverSetting.ChannelModeAddress = modes[0];
                                            serverSetting.ChannelModeParam = modes[1];
                                            serverSetting.ChannelModeParamNotRemove = modes[2];
                                            serverSetting.ChannelModeNoParam += modes[3];

                                            serverSetting.ChannelModeNoParam = RemoveDuplicates(serverSetting.ChannelModeNoParam);

                                        }
                                    }
                                }
                                //parse MAX MODES set
                                if (ircData[i].Length > 6)
                                {
                                    if (ircData[i].StartsWith("MODES="))
                                        serverSetting.MaxModes = Convert.ToInt32(ircData[i].Substring(6));
                                }

                                //parse STATUSMSG symbols
                                if (ircData[i].Length > 10)
                                {
                                    if (ircData[i].StartsWith("STATUSMSG="))
                                        serverSetting.StatusMSG = ircData[i].Substring(10).ToCharArray();
                                }

                                //extract the network name                            
                                if (ircData[i].Length > 8)
                                {
                                    if (ircData[i].Substring(0, 8) == "NETWORK=")
                                        serverSetting.NetworkName = ircData[i].Substring(8);
                                }

                                //parse CHANTYPES symbols
                                if (ircData[i].Length > 10)
                                {
                                    if (ircData[i].StartsWith("CHANTYPES="))
                                        serverSetting.ChannelTypes = ircData[i].Substring(10).ToCharArray();
                                }

                                if (ircData[i].Length > 8)
                                {
                                    if (ircData[i].Substring(0, 8) == "CHARSET=")
                                    {
                                        //do something about character sets
                                    }
                                }

                                //check max nick length
                                if (ircData[i].Length > 8)
                                {
                                    if (ircData[i].Substring(0, 8) == "NICKLEN=")
                                    {
                                        serverSetting.MaxNickLength = Convert.ToInt32(ircData[i].Substring(8));
                                    }
                                }
                                if (ircData[i].Length > 11)
                                {
                                    if (ircData[i].Substring(0, 11) == "MAXNICKLEN=")
                                    {
                                        serverSetting.MaxNickLength = Convert.ToInt32(ircData[i].Substring(11));
                                    }
                                }

                                //check if MONITOR (IRCv3.2) is supported
                                if (ircData[i].Length > 8)
                                {
                                    if (ircData[i].Substring(0, 8) == "MONITOR=")
                                    {
                                        //use monitor instead of ISON
                                        serverSetting.MonitorSupport = true;
                                    }
                                }

                                //tell server this client supports NAMESX
                                if (ircData[i] == "NAMESX")
                                {
                                    SendData("PROTOCTL NAMESX");
                                }
                                
                                //CLIENTVER=3.0
                                if (ircData[i].Length > 10)
                                {
                                    if (ircData[i].Substring(0, 10) == "CLIENTVER=")
                                    {
                                        //ircv3 or 3.1, etc
                                        serverSetting.IRCV3 = true;
                                    }
                                }
                            }
                            break;
                        case "006": // map data
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);                            
                            break;
                        case "007": //could be end of map
                            if (ircData[3] == ":End")
                            {
                                ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            }
                            else
                            {
                                DateTime date5 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                date5 = date5.AddSeconds(Convert.ToDouble(ircData[4]));

                                msg = ircData[3] + " " + date5.ToShortTimeString() + " " + JoinString(ircData, 5, true);
                                ServerMessage(this, msg, serverTimeValue);
                            }
                            break;
                        case "014":
                            ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);
                            break;
                        case "020": //IRCnet message
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        case "042":
                            msg = JoinString(ircData, 4, true) + " " + ircData[3];
                            ServerMessage(this, msg, serverTimeValue);
                            break;
                        case "219": //end of stats
                            ServerMessage(this, JoinString(ircData, 4, true), serverTimeValue);
                            break;
                        case "221": //:port80b.se.quakenet.org 221 Snerf +i
                            ServerMessage(this, RemoveColon(ircData[0]) + " sets mode for " + ircData[2] + " " + ircData[3], serverTimeValue);
                            break;
                        case "222":
                            //ircData[3] == new encoding
                            //auto switch to the new encoding style
                            foreach (EncodingInfo ei in System.Text.Encoding.GetEncodings())
                                if (ei.Name.ToLower() == ircData[3].ToLower())
                                    this.serverSetting.Encoding = ircData[3].ToLower();

                            ServerMessage(this, ircData[3] + " " + JoinString(ircData, 4, true), serverTimeValue);
                            break;
                        case "251": //there are x users on x servers
                        case "255": //I have x users and x servers
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        case "250": //highest connection count
                            msg = JoinString(ircData, 3, true);
                            ServerMessage(this, msg, serverTimeValue);
                            break;
                        case "252": //operators online
                        case "253": //unknown connections
                        case "254": //channels formed
                            msg = "There are " + ircData[3] + " " + JoinString(ircData, 4, true);
                            ServerMessage(this, msg, serverTimeValue);
                            break;
                        case "265": //current local users / max
                        case "266": //current global users / max
                        case "267": //more global user stuff
                            if (ircData[5].StartsWith(":"))
                                msg = JoinString(ircData, 5, true);
                            else
                                msg = JoinString(ircData, 3, true);
                            ServerMessage(this, msg, serverTimeValue);
                            break;
                        case "271":
                        case "272":
                            ServerMessage(this, JoinString(ircData, 4, true), serverTimeValue);                            
                            break;
                        case "302": //parse out a userhost
                            msg = JoinString(ircData, 3, true).TrimEnd();
                            if (msg.Length == 0) return;                            
                            if (msg.IndexOf(' ') == -1)
                            {
                                //single host
                                host = msg.Substring(msg.IndexOf('@') + 1);
                                if (msg.IndexOf('*') > -1)
                                    nick = msg.Substring(0, msg.IndexOf('*'));
                                else
                                    nick = msg.Substring(0, msg.IndexOf('='));

                                try
                                {
                                    System.Net.IPAddress[] addresslist = System.Net.Dns.GetHostAddresses(host);
                                    foreach (System.Net.IPAddress address in addresslist)
                                    {
                                        OutGoingCommand(this, "/echo " + nick + " resolved to " + address.ToString());
                                        UserHostReply(this, msg);
                                        if (nick == serverSetting.CurrentNickName)
                                        {
                                            serverSetting.LocalIP = address;
                                        }
                                    }
                                }
                                catch
                                {
                                    //this can cause a Socket Exception Error
                                    OutGoingCommand(this, "/echo " + nick + " (" + host + ") can not be resolved");
                                }
                            }
                            else
                            {
                                //multiple hosts
                                string[] hosts = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string h in hosts)
                                    UserHostReply(this, h);
                            }
                            break;
                        case "303": //parse out ISON information (Buddy List)
                            msg = JoinString(ircData, 3, true);

                            //queue up next batch to send
                            buddyListTimer.Start();

                            //if (msg.Length == 0) return;

                            string[] buddies = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            BuddyListData(this, buddies, serverTimeValue);
                            break;
                        case "311":     //whois information username address
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, ircData[3]);
                            if (check)
                            {
                                UserInfoHostFullName(this, nick, ircData[4] + "@" + ircData[5], JoinString(ircData, 7, true));
                            }
                            else
                            {
                                msg = "is " + ircData[4] + "@" + ircData[5] + " (" + JoinString(ircData, 7, true) + ")";
                                WhoisData(this, ircData[3], msg, serverTimeValue);
                                IALUserData(this, nick, ircData[4] + "@" + ircData[5], "");
                            }
                            break;
                        case "312":     //whois information server info
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                            {
                                UserInfoServer(this, nick, ircData[4] + " (" + JoinString(ircData, 5, true) + ")");
                            }
                            else
                            {
                                msg = "using " + ircData[4] + " (" + JoinString(ircData, 5, true) + ")";
                                WhoisData(this, ircData[3], msg, serverTimeValue);
                            }
                            break;
                        case "223":     //whois charset is UTF-8
                        case "264":     //whois using encrypted connection
                        case "307":     //whois information nick ips
                        case "310":     //whois is available for help
                        case "313":     //whois information is an IRC operator
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "317":     //whois information signon time
                            DateTime date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            date1 = date1.AddSeconds(Convert.ToDouble(ircData[5]));
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                            {
                                //UserInfoIdleLogon(this, nick, GetDuration(Convert.ToInt32(ircData[4])) + " " + JoinString(ircData, 6, true), date1.ToShortTimeString() + " " + date1.ToShortDateString());
                                UserInfoIdleLogon(this, nick, GetDuration(Convert.ToInt32(ircData[4])), date1.ToShortTimeString() + " " + date1.ToShortDateString());
                            }
                            else
                            {
                                msg = GetDuration(Convert.ToInt32(ircData[4])) + " " + JoinString(ircData, 6, true) + " " + date1.ToShortTimeString() + " " + date1.ToShortDateString();
                                WhoisData(this, ircData[3], msg, serverTimeValue);
                            }
                            break;
                        case "318":     //whois information end of whois
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, false);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "319":     //whois information channels
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                            {
                                string[] chans = JoinString(ircData, 4, true).Split(' ');
                                UserInfoAddChannels(this, nick, chans);
                            }
                            else
                            {
                                msg = "is on: " + JoinString(ircData, 4, true);
                                WhoisData(this, ircData[3], msg, serverTimeValue);
                            }
                            break;
                        case "320":     //whois information
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;

                        case "326": //whois information
                        case "327":
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 5, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        
                        case "330":     //whois information  -- is authed as..
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                            {
                                //get the last ircData[] value
                                msg = ircData[4];
                                UserInfoLoggedIn(this, nick, msg);
                            }
                            else
                            {
                                msg = JoinString(ircData, 5, true) + " " + ircData[4];
                                WhoisData(this, ircData[3], msg, serverTimeValue);
                            }
                            break;
                        case "334":
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, false);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "335":     //whois information
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = ircData[3] + " " + JoinString(ircData, 4, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "338":     //whois information
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            if (ircData[6].StartsWith(":"))
                                msg = JoinString(ircData, 6, true) + " " + ircData[4] + " " + ircData[5];
                            else
                                msg = JoinString(ircData, 5, true) + " " + ircData[4];
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "378":     //whois information
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = RemoveColon(ircData[4]) + " " + JoinString(ircData, 5, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "379":     //whois information
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = RemoveColon(ircData[4]) + " " + JoinString(ircData, 5, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "314":  //whowas information
                            msg = ircData[4] + "!" + ircData[5] + " " + ircData[6] + " " + JoinString(ircData,7,true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "369":
                        case "406": //whowas information
                            msg = JoinString(ircData, 4, false);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "275":
                        case "671":     //using secure connection
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);
                            break;


                        case "615":
                        case "616":
                            nick = ircData[3];
                            check = UserInfoWindowExists(this, nick);
                            if (check)
                                return;
                            msg = JoinString(ircData, 4, true);
                            WhoisData(this, ircData[3], msg, serverTimeValue);                            
                            //:caliburn.pa.us.irchighway.net 615 Snerf Snerf :is using modes +irx +
                            //:caliburn.pa.us.irchighway.net 616 Snerf Snerf :real hostname S01060014d1352fd9.no.sha wcable.net 96.54.227.190
                            break;
                        
                        
                        case "321":     //start channel list
                            ChannelListStart(this);
                            break;
                        case "322":     //channel list
                            //3 4 rc(5+)
                            ChannelList(this, ircData[3], ircData[4], RemoveColon(JoinString(ircData, 5, true)));
                            break;
                        case "323": //end channel list
                            ChannelListEnd(this);
                            ServerMessage(this, "End of Channel List", serverTimeValue);
                            break;
                        case "324":     //channel modes
                            channel = ircData[3];
                            msg = "Channel modes for " + channel + " are :" + JoinString(ircData, 4, false);
                            ChannelMode(this, channel, "", channel, JoinString(ircData, 4, false), serverTimeValue);
                            GenericChannelMessage(this, channel, msg, serverTimeValue);
                            break;
                        
                        case "328":     //channel url
                            channel = ircData[3];
                            msg = "Channel URL is " + JoinString(ircData, 4, true);
                            GenericChannelMessage(this, channel, msg, serverTimeValue);
                            break;
                        case "329":     //channel creation time
                            channel = ircData[3];
                            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            date = date.AddSeconds(Convert.ToDouble(ircData[4]));
                            msg = "Channel Created on: " + date.ToShortTimeString() + " " + date.ToShortDateString();
                            GenericChannelMessage(this, channel, msg, serverTimeValue);
                            break;
                        case "331":     //no topic is set
                            channel = ircData[3];
                            check = ChannelInfoWindowExists(this, channel);
                            if (!check)
                                GenericChannelMessage(this, channel, "No Topic Set", serverTimeValue);
                            break;
                        case "332":     //channel topic
                            channel = ircData[3];
                            check = ChannelInfoWindowExists(this, channel);
                            if (!check)
                                ChannelTopic(this, channel, "", "", JoinString(ircData, 4, true), serverTimeValue);
                            break;
                        case "333":     //channel time
                            channel = ircData[3];
                            nick = ircData[4];
                            DateTime date2 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                            date2 = date2.AddSeconds(Convert.ToDouble(ircData[5]));

                            check = ChannelInfoWindowExists(this, channel);
                            if (check)
                            {
                                ChannelInfoTopicSet(this, channel, nick, date2.ToShortTimeString() + " " + date2.ToShortDateString());
                            }
                            else
                            {
                                msg = "Channel Topic Set by: " + nick + " on " + date2.ToShortTimeString() + " " + date2.ToShortDateString();
                                GenericChannelMessage(this, channel, msg, serverTimeValue);
                            }

                            break;
                        case "343":
                            ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);
                            break;
                        case "348": //channel exception list
                            channel = ircData[3];
                            //3 is channel
                            //4 is host
                            //5 added by
                            //6 added time
                            check = ChannelInfoWindowExists(this, channel);
                            if (check)
                            {
                                DateTime date4 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(ircData[6]));
                                ChannelInfoAddException(this, channel, ircData[4], NickFromFullHost(ircData[5]) + " on " + date4.ToShortTimeString() + " " + date4.ToShortDateString());
                            }
                            else
                            {
                                ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);
                            }
                            break;
                        case "349": //end of channel exception list
                            break;
                        case "315": //end of who reply
                            channel = ircData[3];
                            EndofWhoReply(this, channel);
                            break;
                        case "352": //who reply
                            channel = ircData[3];
                            //user flags in ircData[8] H-here G-gone(away) * - irc operator x-hiddenhost d-deaf
                            //352 Snerf #test ~PircBot ip24-254-168-144.rn.hr.cox.net *.quakenet.org bluemann138 H :3 PircBot 1.5.0 Java IRC Bot
                            //server hops ircData[9]
                            WhoReply(this, channel, ircData[7], ircData[4] + "@" + ircData[5], ircData[8], JoinString(ircData, 3, false));
                            break;
                        case "353": //channel user list
                            channel = ircData[4];
                            ChannelUserList(this, channel, JoinString(ircData, 5, true).Split(' '), JoinString(ircData, 4, true));
                            break;
                        case "365":  //End of Links
                            ServerMessage(this, JoinString(ircData, 4, true), serverTimeValue);
                            break;
                        case "366":     //end of names
                            channel = ircData[3];
                            //channel is fully joined                            
                            EndofNames(this, channel);
                            break;
                        case "367": //channel ban list
                            channel = ircData[3];
                            //3 is channel
                            //4 is host
                            //5 banned by
                            //6 ban time
                            check = ChannelInfoWindowExists(this, channel);
                            if (check)
                            {
                                DateTime date3 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(ircData[6]));
                                ChannelInfoAddBan(this, channel, ircData[4], ircData[5] + " on " + date3.ToShortTimeString() + " " + date3.ToShortDateString());
                            }
                            else
                            {
                                ServerMessage(this, JoinString(ircData, 3, false), serverTimeValue);
                            }
                            break;
                        case "368": //end of channel ban list
                            break;
                        case "377":
                            ServerMessage(this, JoinString(ircData, 6, true), serverTimeValue);
                            break;
                        case "386":
                            ServerMessage(this, JoinString(ircData, 6, true), serverTimeValue);                            
                            break;
                        case "372": //motd
                        case "375":
                            msg = JoinString(ircData, 3, true);
                            if (serverSetting.ShowMOTD || serverSetting.ForceMOTD)
                                ServerMOTD(this, msg, serverTimeValue);
                            break;
                        case "376": //end of motd
                        case "422": //missing motd
                            if (serverSetting.ForceMOTD)
                            {
                                serverSetting.ForceMOTD = false;
                                return;
                            }

                            if (serverSetting.MonitorSupport)
                            {
                                MonitorListCheck();
                            }
                            else
                            {
                                BuddyListCheck();
                                buddyListTimer.Start();
                            }

                            if (fullyConnected)
                                return;

                            ServerMessage(this, "You have successfully connected to " + serverSetting.RealServerName, serverTimeValue);

                            //create default 005 responses if they are blank
                            if (serverSetting.StatusModes == null)
                            {
                                serverSetting.StatusModes = new char[2][];
                                serverSetting.StatusModes[0] = "ov".ToCharArray();
                                serverSetting.StatusModes[1] = "@+".ToCharArray();
                            }

                            if (serverSetting.ChannelTypes == null)
                                serverSetting.ChannelTypes = "#".ToCharArray();

                            if (serverSetting.ChannelModeAddress == null)
                            {
                                string[] modes = "b,k,l,imnpstrDducCNMT".Split(',');
                                serverSetting.ChannelModeAddress = modes[0];
                                serverSetting.ChannelModeParam = modes[1];
                                serverSetting.ChannelModeParamNotRemove = modes[2];
                                serverSetting.ChannelModeNoParam = modes[3];
                            }

                            if (serverSetting.SetModeI)
                                SendData("MODE " + serverSetting.CurrentNickName + " +i");

                            //run autoperform
                            if (serverSetting.AutoPerformEnable && serverSetting.AutoPerform != null)
                            {
                                ServerMessage(this, "Running AutoPerform command(s)...", serverTimeValue);
                                AutoPerform(this, serverSetting.AutoPerform);
                            }

                            // Nickserv password
                            if (serverSetting.NickservPassword != null && serverSetting.NickservPassword.Length > 0)
                            {
                                OutGoingCommand(this, "/msg NickServ identify " + serverSetting.NickservPassword);
                            }

                            if (serverSetting.RejoinChannels)
                            {
                                //rejoin any channels that are open
                                AutoRejoin(this);
                            }

                            //run autojoins
                            if (serverSetting.AutoJoinEnable && serverSetting.AutoJoinChannels != null)
                            {
                                ServerMessage(this, "Auto-joining Channels", serverTimeValue);
                                AutoJoin(this, serverSetting.AutoJoinChannels);
                            }

                            fullyConnected = true;

                            RefreshServerTree(this);

                            //read the command queue
                            if (commandQueue.Count > 0)
                            {
                                foreach (string command in commandQueue)
                                    SendData(command);
                            }
                            commandQueue.Clear();

                            if (ServerFullyConnected != null)
                                ServerFullyConnected(this);
                            
                            break;
                        case "396":     //mode X message                            
                            msg = ircData[3] + " " + JoinString(ircData, 4, true);
                            ServerMessage(this, msg, serverTimeValue);
                            break;
                        case "439":
                        case "931":
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        case "901":
                            ServerMessage(this, JoinString(ircData, 6, true), serverTimeValue);
                            break;
                        case "PRIVMSG":
                            channel = ircData[2];
                            msg = JoinString(ircData, 3, true);
                            if (serverTimeValue.Length > 0 && this.serverSetting.UseServerTime)
                            {
                                //add the server time for buffer playback??
                                //:@time=2014-03-18T01:55:08.596Z :irc.server.com
                                
                                //serverTimeValue = '2014-03-18T01:55:08.596Z'
                                //serverTimeValue = serverTimeValue.Substring(serverTimeValue.IndexOf('T')+1);                                
                                //make this the NEW timestamp
                                //msg = "[" + serverTimeValue + "] " + msg;
                            }

                            if (CheckIgnoreList(nick, host)) return;

                            if (channel.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                //this is a private message to you
                                //check if it was an notice/action
                                if (msg[0] == (char)1)
                                {
                                    //drop the 1st and last CTCP Character
                                    msg = msg.Trim(new char[] { (char)1 });
                                    //check for action
                                    switch (msg.Split(' ')[0].ToUpper())
                                    {
                                        case "ACTION":
                                            msg = msg.Substring(6);
                                            QueryAction(this, nick, host, msg, serverTimeValue);
                                            IALUserData(this, nick, host, "");
                                            break;
                                        case "VERSION":
                                        case "ICECHAT":
                                        case "REMOVEICECHAT":
                                        case "TIME":
                                        case "PING":
                                        case "USERINFO":
                                        case "CLIENTINFO":
                                        case "SOURCE":
                                        case "FINGER":
                                            CtcpMessage(this, nick, msg.Split(' ')[0].ToUpper(), msg.Substring(msg.IndexOf(" ") + 1), serverTimeValue);
                                            break;
                                        default:
                                            //check for DCC SEND, DCC CHAT, DCC ACCEPT, DCC RESUME
                                            if (msg.ToUpper().StartsWith("DCC SEND"))
                                            {
                                                msg = msg.Substring(8).Trim();
                                                System.Diagnostics.Debug.WriteLine("PRIVMSG:" + msg);

                                                string[] dccData = msg.Split(' ');
                                                //sometimes the filenames can be include in quotes
                                                System.Diagnostics.Debug.WriteLine("length:" + dccData.Length);
                                                //PRIVMSG Snerf :DCC SEND serial.txt 1614209982 20052 71

                                                if (dccData.Length > 4)
                                                {
                                                    uint uresult;
                                                    if (!uint.TryParse(dccData[dccData.Length - 1], out uresult))
                                                    {
                                                        return;
                                                    }

                                                    //there can be a passive dcc request sent
                                                    //PegMan-default(2010-05-07)-OS.zip 4294967295 0 176016 960
                                                    //960 is the passive DCC ID
                                                    //length:5
                                                    //960:176016:0:
                                                    if (dccData[dccData.Length - 3] == "0")
                                                    {
                                                        //passive DCC
                                                        string id = dccData[dccData.Length - 1];
                                                        uint fileSize = uint.Parse(dccData[dccData.Length - 2]);
                                                        string port = dccData[dccData.Length - 3];
                                                        string ip = dccData[dccData.Length - 4];
                                                        string file = "";
                                                        if (msg.Contains("\""))
                                                        {
                                                            string[] words = msg.Split('"');
                                                            if (words.Length == 3)
                                                                file = words[1];
                                                            System.Diagnostics.Debug.WriteLine(words.Length);
                                                            foreach (string w in words)
                                                            {
                                                                System.Diagnostics.Debug.WriteLine(w);
                                                            }
                                                        }
                                                        else
                                                            file = dccData[dccData.Length - 5];

                                                        //start up a listening socket on a specific port and send back to ip
                                                        //http://trout.snt.utwente.nl/ubbthreads/ubbthreads.php?ubb=showflat&Number=139329&site_id=1#import

                                                        System.Diagnostics.Debug.WriteLine("PASSIVE DCC " + id + ":" + fileSize + ":" + port + ":" + ip + ":" + file);
                                                        if (DCCPassive != null)
                                                            DCCPassive(this, nick, host, ip, file, fileSize, 0, false, id);

                                                        return;
                                                    }
                                                    else
                                                    {
                                                        uint fileSize = uint.Parse(dccData[dccData.Length - 1]);
                                                        string port = dccData[dccData.Length - 2];
                                                        string ip = dccData[dccData.Length - 3];
                                                        string file = "";
                                                        if (msg.Contains("\""))
                                                        {
                                                            string[] words = msg.Split('"');
                                                            if (words.Length == 3)
                                                                file = words[1];
                                                            System.Diagnostics.Debug.WriteLine(words.Length);
                                                            foreach (string w in words)
                                                            {
                                                                System.Diagnostics.Debug.WriteLine(w);
                                                            }
                                                        }

                                                        System.Diagnostics.Debug.WriteLine(fileSize + ":" + port + ":" + ip + ":" + file);
                                                        //check that values are numbers

                                                        if (DCCFile != null && file.Length > 0)
                                                            DCCFile(this, nick, host, port, ip, file, fileSize, 0, false);
                                                        return;
                                                    }
                                                }
                                                //string fileName = dccData[0];
                                                //string ip = dccData[1];
                                                //string port = dccData[2];
                                                //string fileSize = dccData[3];
                                                System.Diagnostics.Debug.WriteLine("DCC SEND:" + dccData[0] + "::" + dccData[1] + "::" + dccData[2] + "::" + dccData[3]);

                                                //check if filesize is a valid number
                                                uint result;
                                                if (!uint.TryParse(dccData[3], out result))
                                                    return;

                                                //check if quotes around file name
                                                if (dccData[0].StartsWith("\"") && dccData[0].EndsWith("\""))
                                                {
                                                    dccData[0] = dccData[0].Substring(1, dccData[0].Length - 2);
                                                }

                                                if (DCCFile != null)
                                                    DCCFile(this, nick, host, dccData[2], dccData[1], dccData[0], uint.Parse(dccData[3]), 0, false);
                                                else
                                                    ServerError(this, "Invalid DCC File send from " + nick, true);
                                            }
                                            else if (msg.ToUpper().StartsWith("DCC RESUME"))
                                            {
                                                //dcc resume, other client requests resuming a file
                                                //PRIVMSG User1 :DCC RESUME "filename" port position
                                                System.Diagnostics.Debug.WriteLine("DCC RESUME:" + data);
                                                //send back a DCC ACCEPT MESSAGE

                                            }
                                            else if (msg.ToUpper().StartsWith("DCC ACCEPT"))
                                            {
                                                //dcc accept, other client accepts the dcc resume
                                                //PRIVMSG User2 :DCC ACCEPT file.ext port position
                                                //System.Diagnostics.Debug.WriteLine("DCC ACCEPT:" + data);
                                                msg = msg.Substring(10).Trim();
                                                System.Diagnostics.Debug.WriteLine("ACCEPT:" + msg);
                                                string[] dccData = msg.Split(' ');
                                                System.Diagnostics.Debug.WriteLine("length:" + dccData.Length);
                                                
                                                //ACCEPT:"epson13792.exe" 5010 68513792
                                                //length:3

                                                if (DCCFile != null)
                                                    DCCFile(this, nick, host, dccData[dccData.Length - 2], "ip", "file", 0, uint.Parse(dccData[dccData.Length - 1]), true);


                                            }
                                            else if (msg.ToUpper().StartsWith("DCC CHAT"))
                                            {
                                                string ip = ircData[6];
                                                string port = ircData[7].TrimEnd(new char[] { (char)1 });
                                                if (DCCChat != null)
                                                    DCCChat(this, nick, host, port, ip);
                                            }
                                            else
                                                UserNotice(this, nick, msg, serverTimeValue);
                                            break;
                                    }
                                }
                                else
                                {
                                    QueryMessage(this, nick, host, msg, serverTimeValue);
                                    IALUserData(this, nick, host, "");
                                }
                            }
                            else
                            {
                                if (msg[0] == (char)1)
                                {
                                    //some clients dont end with a CTCP character
                                    if (msg.EndsWith( ((char)1).ToString()))
                                        msg = msg.Substring(1, msg.Length - 2);
                                    else
                                        msg = msg.Substring(1);
                                    
                                    switch (msg.Split(' ')[0].ToUpper())
                                    {
                                        case "ACTION":
                                            msg = msg.Substring(7);
                                            ChannelAction(this, channel, nick, host, msg, serverTimeValue);
                                            IALUserData(this, nick, host, channel);
                                            break;
                                        case "VERSION":
                                        case "TIME":
                                        case "PING":
                                        case "USERINFO":
                                        case "CLIENTINFO":
                                        case "SOURCE":
                                        case "FINGER":
                                            //we need to send a reply
                                            CtcpMessage(this, nick, msg.Split(' ')[0].ToUpper(), msg, serverTimeValue);
                                            break;
                                        default:
                                            if (msg.ToUpper().StartsWith("ACTION "))
                                            {
                                                msg = msg.Substring(7);
                                                ChannelAction(this, channel, nick, host, msg, serverTimeValue);
                                                IALUserData(this, nick, host, channel);
                                            }
                                            else
                                            {
                                                ChannelNotice(this, nick, host, (char)32, channel, msg, serverTimeValue);
                                                IALUserData(this, nick, host, channel);
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    if (ChannelMessage != null)
                                        ChannelMessage(this, channel, nick, host, msg, serverTimeValue);
                                    IALUserData(this, nick, host, channel);

                                }
                            }
                            break;
                        case "INVITE":      //channel invite
                            channel = RemoveColon(ircData[3]);
                            ChannelInvite(this, channel, nick, host, serverTimeValue);
                            break;
                    
                        case "NOTICE":
                            msg = JoinString(ircData, 3, true);
                            //check if its a user notice or a server notice
                            if (nick.ToLower() == serverSetting.RealServerName.ToLower())
                                ServerNotice(this, msg, serverTimeValue);
                            else
                            {
                                if (CheckIgnoreList(nick, host)) return;

                                if (initialLogon && serverSetting.StatusMSG == null && serverSetting.StatusModes != null)
                                {
                                    serverSetting.StatusMSG = new char[serverSetting.StatusModes[1].Length];
                                    for (int j = 0; j < serverSetting.StatusModes[1].Length; j++)
                                        serverSetting.StatusMSG[j] = serverSetting.StatusModes[1][j];
                                }
                                if (initialLogon && serverSetting.ChannelTypes != null && Array.IndexOf(serverSetting.ChannelTypes, ircData[2][0]) != -1)
                                {
                                    ChannelNotice(this, nick, host, '0', ircData[2], msg, serverTimeValue);
                                    IALUserData(this, nick, host, ircData[2]);
                                }
                                else if (initialLogon && serverSetting.StatusMSG != null && Array.IndexOf(serverSetting.StatusMSG, ircData[2][0]) != -1 && Array.IndexOf(serverSetting.ChannelTypes, ircData[2][1]) != -1)
                                {
                                    ChannelNotice(this, nick, host, ircData[2][0], ircData[2].Substring(1), msg, serverTimeValue);
                                    IALUserData(this, nick, host, ircData[2]);
                                }
                                else
                                {
                                    //System.Diagnostics.Debug.WriteLine("NOTICE:" + msg);
                                    if (msg.ToUpper().StartsWith("DCC SEND"))
                                    {
                                        System.Diagnostics.Debug.WriteLine("NOTICE DCC SEND:" + nick + ":" + msg);
                                        UserNotice(this, nick, msg, serverTimeValue);
                                    }
                                    else if (msg.ToUpper().StartsWith("DCC CHAT"))
                                    {
                                        UserNotice(this, nick, msg, serverTimeValue);
                                    }
                                    else
                                    {
                                        if (msg[0] == (char)1)
                                        {
                                            msg = msg.Substring(1, msg.Length - 2);
                                            string ctcp = msg.Split(' ')[0].ToUpper();
                                            msg = msg.Substring(msg.IndexOf(" ") + 1);
                                            switch (ctcp)
                                            {
                                                case "PING":
                                                    int result;
                                                    if (Int32.TryParse(msg, out result))
                                                    {
                                                        int diff = System.Environment.TickCount - Convert.ToInt32(msg);
                                                        
                                                        System.Diagnostics.Debug.WriteLine(msg + ":" + System.Environment.TickCount + ":" + diff);
                                                        
                                                        msg = GetDurationMS(diff);
                                                    }
                                                    if (CtcpReply != null)
                                                        CtcpReply(this, nick, ctcp, msg, serverTimeValue);
                                                    break;
                                                default:
                                                    if (CtcpReply != null)
                                                        CtcpReply(this, nick, ctcp, msg, serverTimeValue);
                                                    break;
                                            }
                                        }
                                        else
                                            UserNotice(this, nick, msg, serverTimeValue);
                                    }
                                }
                            }
                            break;

                        case "MODE":
                            channel = ircData[2];

                            if (channel.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                if (host.IndexOf('@') > -1 && this.serverSetting.LocalIP == null)
                                {
                                    this.serverSetting.LocalHost = host;
                                    /*
                                    try
                                    {
                                        host = host.Substring(host.IndexOf('@') + 1);
                                        System.Net.IPAddress[] addresslist = System.Net.Dns.GetHostAddresses(host);
                                        foreach (System.Net.IPAddress address in addresslist)
                                            this.serverSetting.LocalIP = address;
                                    }
                                    catch (Exception)
                                    {
                                        //can not parse the mode
                                    }
                                    */
                                }
                                //user mode
                                tempValue = JoinString(ircData, 3, true);
                                UserMode(this, channel, tempValue, serverTimeValue);
                            }
                            else
                            {
                                //channel mode
                                tempValue = JoinString(ircData, 3, false);

                                ChannelMode(this, nick, HostFromFullHost(ircData[0]), channel, tempValue, serverTimeValue);
                            }
                            break;

                        case "JOIN":
                            channel = RemoveColon(ircData[2]);
                            //extended-join is below
                            string account = "";
                            if (ircData.Length > 3)
                            {
                                //extended join
                                account = ircData[3];
                            }
                                
                            //this is normal
                            //check if it is our own nickname
                            if (nick.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                JoinChannelMyself(this, channel, host, account, serverTimeValue);
                                SendData("MODE " + channel);
                            }
                            else
                            {
                                IALUserData(this, nick, host, channel);
                                JoinChannel(this, channel, nick, host, account, true, serverTimeValue);
                            }
                            break;

                        case "PART":
                            channel = RemoveColon(ircData[2]);
                            tempValue = JoinString(ircData, 3, true); //part reason
                            //check if it is our own nickname
                            if (nick.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                //part self
                                PartChannelMyself(this, channel, serverTimeValue);
                            }
                            else
                            {
                                tempValue = JoinString(ircData, 3, true);
                                IALUserPart(this, nick, channel);
                                PartChannel(this, channel, nick, host, tempValue, serverTimeValue);
                            }
                            break;

                        case "QUIT":
                            nick = NickFromFullHost(RemoveColon(ircData[0]));
                            host = HostFromFullHost(RemoveColon(ircData[0]));
                            tempValue = JoinString(ircData, 2, true);

                            QuitServer(this, nick, host, tempValue, serverTimeValue);
                            IALUserQuit(this, nick);
                            break;

                        case "NICK":
                            //old nickname
                            nick = NickFromFullHost(RemoveColon(ircData[0]));
                            host = HostFromFullHost(RemoveColon(ircData[0]));

                            //new nickname
                            tempValue = RemoveColon(ircData[2]);

                            if (nick.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                //if it is your own nickname, update it
                                serverSetting.CurrentNickName = tempValue;
                            }

                            IALUserChange(this, nick, tempValue);
                            ChangeNick(this, nick, tempValue, HostFromFullHost(ircData[0]),serverTimeValue);

                            break;

                        case "KICK":
                            msg = JoinString(ircData, 4, true);  //kick message                        
                            channel = ircData[2];
                            //this is WHO got kicked
                            nick = ircData[3];
                            //check if it is our own nickname who got kicked
                            if (nick.ToLower() == serverSetting.CurrentNickName.ToLower())
                            {
                                //we got kicked
                                ChannelKickSelf(this, channel, msg, ircData[0], serverTimeValue);
                            }
                            else
                            {
                                ChannelKick(this, channel, nick, msg, ircData[0], serverTimeValue);
                                IALUserPart(this, nick, channel);
                            }
                            break;
                        case "PONG":
                            pongTimer.Stop();
                            pongTimer.Start();
                            //:servercentral.il.us.quakenet.org PONG servercentral.il.us.quakenet.org :servercentral.il.us.quakenet.org
                            
                            break;
                        case "TOPIC":   //channel topic change
                            channel = ircData[2];
                            msg = JoinString(ircData, 3, true);
                            ChannelTopic(this, channel, nick, host, msg, serverTimeValue);
                            break;
                        case "AUTH":    //NOTICE AUTH
                            ServerMessage(this, JoinString(ircData, 2, true), serverTimeValue);
                            break;

                        /*************  IRCV3 extras *******************/
                        case "AWAY": //IRC v3
                            msg = JoinString(ircData, 2, true);
                            if (msg.Length == 0)
                            {
                                //nick is no longer away
                                ServerMessage(this, nick + " is no longer away", serverTimeValue);
                                IALUserDataAwayOnly(this, nick, false, "");
                            }
                            else
                            {
                                ServerMessage(this, nick + " is set as away (" + msg + ")", serverTimeValue);
                                IALUserDataAwayOnly(this, nick, true, msg);                            
                            }
                            break;

                        case "301": //whois reply, away reason
                            nick = ircData[3];                            
                            check = UserInfoWindowExists(this, nick);
                            msg = JoinString(ircData, 4, true);

                            if (msg.Length > 0)
                                IALUserDataAwayOnly(this, nick, true, msg);
                            else
                                IALUserDataAwayOnly(this, nick, false, "");
                            
                            if (check)
                                UserInfoAwayStatus(this, nick, msg);
                            else
                                WhoisData(this, nick, "is away: " + msg, serverTimeValue);
                            break;
                        
                        case "305": //no longer marked away
                            nick = ircData[2];
                            msg = JoinString(ircData, 3, true);
                            ServerMessage(this, msg, serverTimeValue);
                            
                            IALUserDataAwayOnly(this, nick, false, "");
                            break;
                        case "306": //marked as away
                            nick = ircData[2];
                            msg = JoinString(ircData, 3, true);
                            ServerMessage(this, msg, serverTimeValue);
                            
                            IALUserDataAwayOnly(this, nick, true, msg); 
                            break;
                        
                        case "ACCOUNT":
                            //:nick!user@host ACCOUNT accountname
                            //:nick!user@host ACCOUNT *
                            if (ircData[2] == "*")
                            {
                                //nick has logged out
                                UserNotice(this, nick, "logged out of account", serverTimeValue);
                            }
                            else
                            {
                                UserNotice(this, nick, "logged in with account (" + ircData[2] + ")", serverTimeValue);
                            }
                            break;    
                        case "CAP": //ircv3 
                            //:sendak.freenode.net CAP * LS :account-notify extended-join identify-msg multi-prefix sasl                        
                            //<- :totoro.staticbox.net CAP * LS :account-notify away-notify extended-join multi-prefix sasl tls
                            //-> irc.atheme.org CAP REQ :multi-prefix
                            //<- :totoro.staticbox.net CAP Snerf8 ACK :multi-prefix 
                            //-> irc.atheme.org CAP END
                            //<- :totoro.staticbox.net CAP Snerf8 NAK :multi-prefix 
                            
                            tempValue = JoinString(ircData, 4, true); //Capabilities
                            
                            if (ircData[3] == "LS")
                            {
                                ServerMessage(this, "Capabilities supported: " + tempValue, serverTimeValue);

                                string sendREQ = CapREQ(tempValue);

                                // Server: CAP * LS * :capabailirty .. check for the *    

                                if (sendREQ.Length > 0)
                                {
                                    ServerMessage(this, "Capabilities requested: " + sendREQ.Trim(), serverTimeValue);
                                    SendData("CAP REQ :" + sendREQ.Trim());
                                }
                                else
                                    SendData("CAP END");
                            }
                            else if (ircData[3] == "NAK")
                            {
                                //REJECTED - FAILED

                                SendData("CAP END");
                            }
                            //:irc.dereferenced.org CAP Snerf LIST cap-notify
                            else if (ircData[3] == "LIST")
                            {
                                // check for a *
                                ServerMessage(this, "Capabilities List: " + tempValue, serverTimeValue);                            
                            }
                            else if (ircData[3] == "NEW")
                            {
                                // check for a *
                                ServerMessage(this, "Capabilities New Capability: " + tempValue, serverTimeValue);
                                
                                // send a CAP REQ ?
                                string sendREQ = CapREQ(tempValue);

                                // Server: CAP * LS * :capabailirty .. check for the *    
                                if (sendREQ.Length > 0)
                                {
                                    ServerMessage(this, "Capabilities requested: " + sendREQ.Trim(), serverTimeValue);
                                    SendData("CAP REQ :" + sendREQ.Trim());
                                }
                            }
                            else if (ircData[3] == "DEL")
                            {
                                // check for a *
                                ServerMessage(this, "Capabilities Removed Capability: " + tempValue, serverTimeValue);
                                // split them up, disable settings

                            }
                            else if (ircData[3] == "ACK")
                            {
                                ServerMessage(this, "Capabilities acknowledged: " + tempValue, serverTimeValue);
                                if (tempValue.IndexOf("extended-join") > -1)
                                {
                                    //extended join is enabled
                                    //:nick!user@host JOIN #channelname accountname :Real Name
                                    //:nick!user@host JOIN #channelname * :Real Name
                                }

                                if (tempValue.IndexOf("account-notify") > -1)
                                {
                                    //account-notify is enabled                                    
                                }

                                if (tempValue.IndexOf("away-notify") > -1)
                                {
                                    //extended away is enabled
                                }

                                if (tempValue.IndexOf("cap-notify") > -1)
                                {
                                    //cap notify is enabled - http://ircv3.net/specs/extensions/cap-notify-3.2.html
                                
                                }

                                if (tempValue.IndexOf("userhost-in-names") > -1)
                                {
                                    //userhost is passed along in NAMES (353)
                                    //does not happen with ZNC
                                    if (!serverSetting.UseBNC)                                    
                                        serverSetting.UserhostInName = true;  
                                }
                                
                                /*
                                if (tempValue.IndexOf("tls") > -1)
                                {
                                    //tls will not be enabled
                                    if (serverSetting.UseTLS && !serverSetting.UseSSL) // dont use TLS if alrerady using SSL
                                    {
                                        System.Diagnostics.Debug.WriteLine("Enable tls handshake");
                                        SendData("STARTTLS");
                                    }
                                }
                                */

                                if (tempValue.IndexOf("sasl") > -1)
                                {
                                    //sasl is enabled
                                    //send PLAIN auth
                                    if (serverSetting.UseSASL)
                                        SendData("AUTHENTICATE PLAIN");
                                }
                                else
                                    //dont send a CAP END if we have not authenticated
                                    SendData("CAP END");

                            }
                            break;
                        case "670": //RPL_STARTTLS
                            // totoro.staticbox.net 670 Snerf :STARTTLS successful, proceed with TLS handshake
                            // http://ircv3.net/specs/extensions/tls-3.1.html
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);                   
         
                            // Start the TLS Handshake here
                            // the connection is waiting for a TLS handshake
                            

                            break;
                        case "691": // RPL_STARTTLSFAIL
                            // STARTTLS failure
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);                   

                            break;

                        case "900": //SASL logged in as ..
                            ServerMessage(this, JoinString(ircData, 5, true), serverTimeValue);
                            break;
                        case "903": //SASL authentication successful
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            SendData("CAP END");
                            break;
                        case "904": //SASL authentication failed - ERR_SASLFAIL 
                        case "905": //SASL ERR_SASLTOOLONG
                            //::sendak.freenode.net 904 Snerfus :SASL authentication failed
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            SendData("CAP END");
                            break;
                        case "906": //SASL aborted
                        case "907": //SASL Already authenticated error
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        
                        case "730": //monitor response for ONLINE
                            // :rajaniemi.freenode.net 730 Snerf9 :Bubi!~Bubi@p5DE95D59.dip0.t-ipconnect.de,Snerf!IceCha t9@unaffiliated/Snerf
                            //[02:26.05] ->2 :rajaniemi.freenode.net 731 Snerf9 :madmn,IceCold101
                            
                            MonitorListData(this, JoinString(ircData, 3, true), true, serverTimeValue);
                            break;
                        case "731": //monitor response for OFFLINE
                            MonitorListData(this, JoinString(ircData, 3, true), false, serverTimeValue);                            
                            break;

                        case "732": //monitor list
                        case "733": //monitor list end
                        case "734": //monitor list full
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        
                        case "CHGHOST": // change host
                            // http://ircv3.net/specs/extensions/chghost-3.2.html
                            // :nick!user@host CHGHOST newuser host
                            // change host in IAL
                            nick = ircData[2];
                            // get the ident??
                            //IALUserData(this, nick, ircData[3], "");

                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue); 
                            break;

                        /*************  IRCV3 extras *******************/


                        case "501": //unknown MODE
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        //errors
                        case "404": //can not send to channel
                        case "467": //channel key already set
                        case "482": //not a channel operator
                            msg = JoinString(ircData, 4, true);
                            GenericChannelMessage(this, ircData[3], msg, serverTimeValue);
                            break;
                        case "432": //erroneus nickname
                        case "438": //nick change too fast
                        case "468": //only servers can change mode
                        case "485": //cant join channel
                        case "493": //user doesnt want message
                            msg = ircData[3] + " " + JoinString(ircData, 4, true);                            
                            ServerError(this, msg, true);
                            break;
                        case "401": //no such nick
                        case "402": //no such server
                        case "403": //no such channel
                        case "405": //joined too many channels
                        case "407": //no message delivered
                        case "411": //no recipient given
                        case "412": //no text to send
                        case "421": //unknown command
                        case "431": //no nickname given
                        case "470": //forward to other channel
                        case "471": //can not join channel (limit enforced)
                        case "472": //unknown char to me (channel mode)
                            ServerError(this, JoinString(ircData, 3, false), true);
                            break;
                        case "473": //can not join channel invite only
                            msg = ircData[3] + " " + JoinString(ircData, 4, true);
                            ServerError(this, msg, true);
                            break;
                        case "442": //you're not in that channel
                        case "474": //Cannot join channel (+b)
                        case "475": //Cannot join channel (+k)
                            msg = ircData[3] + " " + JoinString(ircData, 4, true);
                            ServerError(this, msg, true);
                            break;

                        case "433": //nickname in use
                            if (fullyConnected == false)
                                serverSetting.RealServerName = RemoveColon(ircData[0]);

                            ServerMessage(this, JoinString(ircData, 4, true), serverTimeValue);

                            if (!initialLogon)  //001 RAW not gotten yet
                            {
                                if (serverSetting.NickName == serverSetting.CurrentNickName)
                                {
                                    SendData("NICK " + serverSetting.AltNickName);
                                    ChangeNick(this, serverSetting.CurrentNickName, serverSetting.AltNickName, HostFromFullHost(RemoveColon(ircData[0])), serverTimeValue);
                                    serverSetting.TriedAltNick = true;
                                }
                                else if (serverSetting.CurrentNickName != serverSetting.AltNickName)
                                {
                                    if (!serverSetting.TriedAltNick)
                                    {
                                        SendData("NICK " + serverSetting.AltNickName);
                                        ChangeNick(this, serverSetting.CurrentNickName, serverSetting.AltNickName, HostFromFullHost(RemoveColon(ircData[0])), serverTimeValue);
                                        serverSetting.TriedAltNick = true;
                                    }
                                    else
                                    {
                                        ServerMessage(this, "Nick and Alt Nick both in use. Please choose a new nickname", "");
                                    }
                                }
                            }
                            break;
                        case "465": //no open proxies
                        case "513": //if you can not connect, type /quote PONG ...
                            ServerError(this, JoinString(ircData, 3, true), true);
                            break;                        

                        case "742": //freenode
                            ChannelNotice(this, ircData[2], "", (char)32, ircData[3], JoinString(ircData, 6, true), serverTimeValue);
                            break;
                        
                        case"SILENCE":
                            ServerMessage(this, JoinString(ircData, 2, false), serverTimeValue);                        
                            break;
                        default:
                            ServerMessage(this, JoinString(ircData, 3, true), serverTimeValue);
                            break;
                        //                            
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorFile(this, "ParseData:" + data, e);
            }
        }
        
        private bool CheckIgnoreList(string nick, string host)
        {
            if (!this.serverSetting.IgnoreListEnable) return false; //if ignore list is disabled, no match
            if (this.serverSetting.IgnoreList.Length == 0) return false;    //if no items in list, no match

            string onlyHost = host.Substring(host.IndexOf("@") + 1).ToLower();

            foreach (string ignore in serverSetting.IgnoreList)
            {
                if (!ignore.StartsWith(";"))    //check to make sure its not disabled
                {
                    //check for an exact match
                    if (nick.ToLower() == ignore.ToLower()) return true;

                    //check if we are looking for a host match
                    if (ignore.Contains("@"))
                    {
                        //do a host match
                        string hostMatch = ignore.Substring(ignore.IndexOf("@") + 1).ToLower();

                        // exact match
                        if (ignore.ToLower() == onlyHost.ToLower()) return true;

                        if (hostMatch == onlyHost) return true;

                        if (hostMatch.StartsWith("*"))
                        {
                            //match the end
                            if (onlyHost.EndsWith(hostMatch.TrimStart('*'))) return true;
                        }
                        else if (hostMatch.EndsWith("*"))
                        {
                            //match the start
                            if (onlyHost.StartsWith(hostMatch.TrimEnd('*'))) return true;
                        }
                        
                    }
                    else
                    {
                        //check for wildcard/regex match for nick name
                        if (Regex.Match(nick, ignore, RegexOptions.IgnoreCase).Success) return true;
                    }
                }
            }

            return false;
        }

        private string RemoveDuplicates(string input)
        {
            string result = "";
            foreach (char c in input)
            {
                if (!result.Contains(c.ToString()) || c == ' ')
                    result += c;
            }
            return result;
        }


        private string CapREQ(string tempValue)
        {
            string sendREQ = "";

            //identify-msg has been deprecated
            //if (this.serverSetting.UseIdentifyMsg)
            //    if (tempValue.IndexOf("identify-msg") > -1)
            //        sendREQ += "identify-msg ";


            if (tempValue.IndexOf("account-notify") > -1)
                if (serverSetting.AccountNotify)
                    sendREQ += "account-notify ";

            if (tempValue.IndexOf("away-notify") > -1)
                if (serverSetting.AwayNotify)
                    sendREQ += "away-notify ";

            if (tempValue.IndexOf("extended-join") > -1)
                if (serverSetting.ExtendedJoin)
                    sendREQ += "extended-join ";

            if (tempValue.IndexOf("multi-prefix") > -1)
                sendREQ += "multi-prefix ";

            if (tempValue.IndexOf("sasl") > -1)
                if (serverSetting.UseSASL)
                    sendREQ += "sasl ";

            if (tempValue.IndexOf("userhost-in-names") > -1)
            {
                //dont enable if BNC
                //has issues with http://pastebin.com/yh0esuCU
                //if (!serverSetting.UseBNC)
                //sendREQ += "userhost-in-names ";
            }

            if (tempValue.IndexOf("cap-notify") > -1)
            {
                // ircv3.2 
                //sendREQ += "cap-notify ";
            }

            if (tempValue.IndexOf("chghost") > -1)
            {
                // ircv3.2                 
                //sendREQ += "chghost ";
            }

            if (tempValue.IndexOf("account-tag") > -1)
            {
                // ircv3.2                 
                //sendREQ += "account-tag ";
            }

            if (tempValue.IndexOf("echo-message") > -1)
            {
                // ircv3.2                 
                // sends back NOTICE and PRIVMSG back to client that sent them
                // make this an option
                //sendREQ += "echo-message ";
            }

            if (tempValue.IndexOf("znc.in/server-time-iso") > -1)
            {
                //:@time=2014-03-18T01:55:08.596Z :dickson.freenode.net 
                sendREQ += "znc.in/server-time-iso ";
                serverSetting.UseServerTime = true;
            }
            else if (tempValue.IndexOf("server-time") > -1)
            {                
                sendREQ += "server-time ";
                serverSetting.UseServerTime = true;
            }

            // no use for TLS
            // http://ircv3.net/specs/extensions/tls-3.1.html
            if (tempValue.IndexOf("tls") > -1)
            {
            //    sendREQ += "tls ";
            }


            

            return sendREQ;

        }


        #region Parsing Methods

        private string GetDurationMS(int milliSseconds)
        {
            TimeSpan t = new TimeSpan(0, 0, 0, 0, milliSseconds);

            string s = t.Seconds.ToString() + "." + t.Milliseconds.ToString() + " secs";
            if (t.Minutes > 0)
                s = t.Minutes.ToString() + " mins " + s;
            if (t.Hours > 0)
                s = t.Hours.ToString() + " hrs " + s;
            if (t.Days > 0)
                s = t.Days.ToString() + " days " + s;

            return s;
        }

        private string GetDuration(int seconds)
        {
            TimeSpan t = new TimeSpan(0, 0, seconds);

            string s = t.Seconds.ToString() + " secs";
            if (t.Minutes > 0)
                s = t.Minutes.ToString() + " mins " + s;
            if (t.Hours > 0)
                s = t.Hours.ToString() + " hrs " + s;
            if (t.Days > 0)
                s = t.Days.ToString() + " days " + s;

            return s;
        }

        private string HostFromFullHost(string host)
        {
            if (host.IndexOf("!") > -1)
                return host.Substring(host.IndexOf("!") + 1);
            else
                return host;
        }

        private string NickFromFullHost(string host)
        {
            if (host.StartsWith(":"))
                host = host.Substring(1);

            if (host.IndexOf("!") > -1)
                return host.Substring(0, host.IndexOf("!"));
            else
                return host;
        }

        private string RemoveColon(string data)
        {
            if (data.StartsWith(":"))
                return data.Substring(1);
            else
                return data;
        }

        private string JoinString(string[] strData, int startIndex, bool removeColon)
        {
            if (startIndex > strData.GetUpperBound(0)) return "";

            string tempString = String.Join(" ", strData, startIndex, strData.GetUpperBound(0) + 1 - startIndex);
            if (removeColon)
            {
                tempString = RemoveColon(tempString);
            }
            return tempString;
        }

        #endregion

    }
}
