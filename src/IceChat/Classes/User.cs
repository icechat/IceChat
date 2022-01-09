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
using System.Collections.Generic;
using System.Text;

namespace IceChat
{
    public class User
    {
        private string nickName;
        private IRCConnection connection = null;
        
        public bool[] Level;
        public int nickColor = -1;
        public bool Selected;
        public bool Away;

        public bool CustomColor = false;

        public User(string nick, IRCConnection connection)
        {
            if (connection != null)
            {
                this.connection = connection;
                Level = new bool[connection.ServerSetting.StatusModes[0].Length];
                for (int i = 0; i < this.Level.Length; i++)
                {
                    if (nick.StartsWith(connection.ServerSetting.StatusModes[1][i].ToString()))
                    {
                        this.Level[i] = true;
                        nick = nick.Substring(1);
                    }
                }
            }
            nickName = nick;
        }

        public override string ToString()
        {
            if (connection != null)
            {
                for (int i = 0; i < this.Level.Length; i++)
                {
                    if (this.Level[i] == true)
                        return connection.ServerSetting.StatusModes[1][i] + nickName;
                }
            }
            return nickName;
        }

        public string NickName
        {
            get
            {
                return this.nickName;
            }
            set
            {
                string n = value;
                if (connection != null)
                {
                    for (int i = 0; i < connection.ServerSetting.StatusModes[1].Length; i++)
                    {
                        if (n.StartsWith(connection.ServerSetting.StatusModes[1][i].ToString()))
                        {
                            this.Level[i] = true;
                            n = n.Substring(1);
                        }
                    }
                }
                nickName = n;

            }
        }
    }
}