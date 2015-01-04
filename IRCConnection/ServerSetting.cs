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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IceChat
{
    public class ServerSetting
    {

        //set the default values (only for specific settings)
        private string _serverPort = "6667";
        private string _displayName = "";
        private string _realServerName = "";
        private string _networkName = "";
        private string _encoding = System.Text.Encoding.UTF8.WebName.ToString();
        private bool _setModeI = true;
        private bool _rejoinChannels = true;
        private int _pingTimerMinutes = 5;
        private int _maxNickLength = 15;
        private int _maxModes = 5;
        private bool _ircv3 = false;
        private bool _extendedJoin = true;
        private bool _awayNotify = true;
        private bool _accountNotify = true;

        private List<string> _tabs = new List<string>();

        [XmlAttribute("ServerID")]
        public int ID
        { get; set; }

        [XmlElement("ServerName")]
        public string ServerName
        { get; set; }

        [XmlElement("DisplayName")]
        public string DisplayName
        { get { return this._displayName; } set { this._displayName = value; } }

        [XmlElement("ServerPort")]
        public string ServerPort
        { get { return this._serverPort; } set { this._serverPort = value; } }

        [XmlElement("NickName")]
        public string NickName
        { get; set; }

        [XmlElement("Password")]
        public string Password
        { get; set; }

        [XmlElement("NickservPassword")]
        public string NickservPassword
        { get; set; }

        [XmlElement("AltNickName")]
        public string AltNickName
        { get; set; }

        [XmlElement("AwayNickName")]
        public string AwayNickName
        { get; set; }

        [XmlElement("QuitMessage")]
        public string QuitMessage
        { get; set; }

        [XmlElement("FullName")]
        public string FullName
        { get; set; }

        [XmlElement("IdentName")]
        public string IdentName
        { get; set; }

        [XmlElement("SetModeI")]
        public bool SetModeI
        { get { return this._setModeI; } set { this._setModeI = value; } }

        [XmlElement("ShowMOTD")]
        public bool ShowMOTD
        { get; set; }

        [XmlElement("AutoStart")]
        public bool AutoStart
        { get; set; }

        [XmlElement("ShowPingPong")]
        public bool ShowPingPong
        { get; set; }

        [XmlElement("AutoJoinDelay")]
        public bool AutoJoinDelay
        { get; set; }

        [XmlElement("AutoJoinDelayBetween")]
        public bool AutoJoinDelayBetween
        { get; set; }

        [XmlArray("AutoPerform")]
        [XmlArrayItem("Item")]
        public string[] AutoPerform
        { get; set; }

        [XmlElement("AutoPerformEnable")]
        public bool AutoPerformEnable
        { get; set; }

        [XmlArray("AutoJoin")]
        [XmlArrayItem("Item", typeof(String))]
        public string[] AutoJoinChannels
        { get; set; }

        [XmlElement("AutoJoinEnable")]
        public bool AutoJoinEnable
        { get; set; }

        [XmlElement("RejoinChannels")]
        public bool RejoinChannels
        { get { return this._rejoinChannels; } set { this._rejoinChannels= value; } }

        [XmlElement("Encoding")]
        public string Encoding
        { get { return this._encoding; } set { this._encoding = value; } }

        [XmlElement("DisableCTCP")]
        public bool DisableCTCP
        { get; set; }

        [XmlElement("DisableAwayMessages")]
        public bool DisableAwayMessages
        { get; set; }

        [XmlArray("IgnoreList")]
        [XmlArrayItem("Item")]
        public string[] IgnoreList
        { get; set; }

        [XmlElement("IgnoreListEnable")]
        public bool IgnoreListEnable
        { get; set; }

        [XmlArray("BuddyList")]
        [XmlArrayItem("Item", typeof(BuddyListItem))]
        public BuddyListItem[] BuddyList
        { get; set; }

        [XmlElement("BuddyListEnable")]
        public bool BuddyListEnable
        { get; set; }

        [XmlElement("PingTimerMinutes")]
        public int PingTimerMinutes
        { get { return this._pingTimerMinutes; } set { this._pingTimerMinutes = value; } }

        [XmlElement("UseSSL")]
        public bool UseSSL
        { get; set; }

        [XmlElement("SSLAcceptInvalidCertificate")]
        public bool SSLAcceptInvalidCertificate
        { get; set; }

        [XmlElement("UseIPv6")]
        public bool UseIPv6
        { get; set; }

        [XmlElement("UseProxy")]
        public bool UseProxy
        { get; set; }

        [XmlElement("ProxyType")]
        //1 = HTTP ; 2 = SOCKS4 ; 3 = SOCKS5
        public int ProxyType
        { get; set; }

        [XmlElement("ProxyIP")]
        public string ProxyIP
        { get; set; }

        [XmlElement("ProxyPort")]
        public string ProxyPort
        { get; set; }

        [XmlElement("ProxyUser")]
        public string ProxyUser
        { get; set; }

        [XmlElement("ProxyPass")]
        public string ProxyPass
        { get; set; }

        [XmlElement("UseBNC")]
        public bool UseBNC
        { get; set; }

        [XmlElement("BNCIP")]
        public string BNCIP
        { get; set; }

        [XmlElement("BNCPort")]
        public string BNCPort
        { get; set; }

        [XmlElement("BNCUser")]
        public string BNCUser
        { get; set; }

        [XmlElement("BNCPass")]
        public string BNCPass
        { get; set; }

        [XmlElement("ServerNotes")]
        public string ServerNotes
        { get; set; }

        [XmlElement("AdvancedSettings")]
        public bool AdvancedSettings
        { get; set; }

        //IRCv3 specs

        [XmlElement("UseSASL")]
        public bool UseSASL
        { get; set; }

        [XmlElement("SASLUser")]
        public string SASLUser
        { get; set; }

        [XmlElement("SASLPass")]
        public string SASLPass
        { get; set; }

        [XmlElement("AccountNotify")]
        public bool AccountNotify
        { get { return this._accountNotify; } set { this._accountNotify= value; } }

        [XmlElement("ExtendedJoin")]
        public bool ExtendedJoin
        { get { return this._extendedJoin; } set { this._extendedJoin = value; } }

        [XmlElement("AwayNotify")]
        public bool AwayNotify
        { get { return this._awayNotify; } set { this._awayNotify = value; } }
        
        //these are all temporary server settings, not saved to the XML file

        [XmlIgnore()]
        public string RealServerName
        { get { return this._realServerName; } set { this._realServerName = value; } }

        //server settings not stored in the XML File
        [XmlIgnore()]
        public string ServerIP
        { get; set; }

        //the NetWork name obtained from NETWORK 005 Reply
        [XmlIgnore()]
        public string NetworkName
        { 
            get 
            {            
                if (_networkName.Length == 0)
                    return this._realServerName; 
                else
                    return this._networkName; 
            }
            set 
            { 
                this._networkName = value; 
            } 
        }

        //the channel modes which have parameters from CHANMODES 005 Reply
        [XmlIgnore()]
        public string ChannelModeParam
        { get; set; }

        [XmlIgnore()]
        public string ChannelModeNoParam
        { get; set; }

        [XmlIgnore()]
        public string ChannelModeAddress
        { get; set; }

        [XmlIgnore()]
        public string ChannelModeParamNotRemove
        { get; set; }

        //the STATUSMG 005 Reply
        [XmlIgnore()]
        public char[] StatusMSG
        { get; set; }

        //the user status modes from PREFIX 005 Reply
        [XmlIgnore()]
        public char[][] StatusModes
        { get; set; }

        //which channel prefixes are allowed
        [XmlIgnore()]
        public char[] ChannelTypes
        { get; set; }

        //maximum setable modes
        public int MaxModes
        { get { return this._maxModes; } set { this._maxModes = value; } }

        //allow the MOTD to show if /motd command is used, but ShowMOTD is disabled
        [XmlIgnore()]
        public bool ForceMOTD
        { get; set; }

        [XmlIgnore()]
        public Hashtable IAL
        { get; set; }

        //whether you are away or not
        [XmlIgnore()]
        public bool Away
        { get; set; }

        //ircv3 userhost-in-name
        [XmlIgnore()]
        public bool UserhostInName
        { get; set; }

        [XmlIgnore()]
        public bool UseServerTime
        { get; set; }
        
        //remember your nickname before you set yourself away
        [XmlIgnore()]
        public string DefaultNick
        { get; set; }

        [XmlIgnore()]
        public string CurrentNickName
        { get; set; }

        [XmlIgnore()]
        public DateTime AwayStart
        { get; set; }

        [XmlIgnore()]
        public DateTime ConnectedTime
        { get; set; }

        [XmlIgnore()]
        public System.Net.IPAddress LocalIP
        { get; set; }

        [XmlIgnore()]
        public string LocalHost
        { get; set; }

        [XmlIgnore()]
        public int MaxNickLength
        { get { return this._maxNickLength; } set { this._maxNickLength = value; } }

        [XmlIgnore()]
        public bool DisableSounds
        { get; set; }

        [XmlIgnore()]
        public int TreeCollapse
        { get; set; }

        [XmlIgnore()]
        public bool TriedAltNick
        { get; set; }

        [XmlIgnore()]
        public bool IRCV3
        { get { return this._ircv3; } set { this._ircv3 = value; } }

        [XmlIgnore()]
        public bool MonitorSupport
        { get; set; }

        [XmlIgnore()]
        [XmlArray("TabOrder")]
        [XmlArrayItem("Tab", typeof(String))]
        public List<String> TabOrder
        { get { return this._tabs; } set { this._tabs = value; } }

    }

}
