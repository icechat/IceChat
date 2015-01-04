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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace IceChat
{
    [XmlRoot("IceChatServers")]
    public class IceChatServers
    {
        [XmlArray("Servers")]
        [XmlArrayItem("Item",typeof(ServerSetting))]
        public List<ServerSetting> listServers;

        public IceChatServers() 
        {
            listServers = new List<ServerSetting>();
        }

        public void AddServer(ServerSetting server)
        {
            listServers.Add(server);
        }

        public void RemoveServer(ServerSetting server)
        {
            listServers.Remove(server);
        }

        public int GetNextID()
        {
            if (listServers.Count ==0)
                return 1;
            return listServers[listServers.Count-1].ID+1;
        }
    }   
}
