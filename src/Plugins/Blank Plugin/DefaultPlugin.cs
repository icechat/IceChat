/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2011 Paul Vanderzee <snerf@icechat.net>
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
using System.Windows.Forms;

namespace IceChatPlugin
{
    public class Plugin : IPluginIceChat
    {
        
        //all the events get declared here
        public override event OutGoingCommandHandler OnCommand;

        //declare the standard properties
        private string m_Name;
        private string m_Author;
        private string m_Version;

        public override string Name { get { return m_Name; } }
        public override string Version { get { return m_Version; } }
        public override string Author { get { return m_Author; } }

        public Plugin()
        {
            //set your default values here
            m_Name = "Default Plugin";
            m_Author = "Default Author";
            m_Version = "1.0";
        }

        //declare the standard methods

        public override void Dispose()
        {

        }

        public override void Initialize()
        {

        }

        //if you want to add a new method to override, use public override
    }
}
