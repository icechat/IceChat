/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2019 Paul Vanderzee <snerf@icechat.net>
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
    public class InternalAddressList
    {
        private string _nick;
        private string _host;
        private bool _away;
        private string _awayMessage;
        private string _account;

        private List<String> _channels;

        public InternalAddressList(string nick, string host, string channel)
        {
            _nick = nick;
            _host = host;
            
            _channels = new List<String>();
            _channels.Add(channel);
        }

        public InternalAddressList(string nick, string host, string channel, string account)
        {
            _nick = nick;
            _host = host;
            _account = account;

            _channels = new List<String>();
            _channels.Add(channel);
        }

        public InternalAddressList(string nick, bool away, string awayMessage)
        {
            _nick = nick;
            _away = away;
            _awayMessage = awayMessage;
            
            _channels = new List<String>();
        }

        public string Nick
        {
            get { return _nick; }
            set { _nick = value; }
        }

        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public bool AwayStatus
        {
            get { return _away; }
            set { _away = value; }
        }

        public string AwayMessage
        {
            get { return _awayMessage; }
            set { _awayMessage = value; }
        }

        public List<String> Channels
        {
            get { return this._channels; }
        }

        public void AddChannel(string channel)
        {
            if (_channels.IndexOf(channel) == -1)
                _channels.Add(channel);
        }

        public void RemoveChannel(string channel)
        {
            if (_channels.IndexOf(channel) != -1)
                _channels.Remove(channel);
        }
    }
}
