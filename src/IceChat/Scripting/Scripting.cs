/******************************************************************************\
 * IceChat 2009 Internet Relay Chat Client
 *
 * Copyright (C) 2010 Paul Vanderzee <snerf@icechat.net>
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
//using System.Text;
//using System.Windows.Forms;

namespace IceChatScript
{
    public class IceChatScript
    {
        public delegate void OutGoingCommandDelegate(string command, object connection);
        public event OutGoingCommandDelegate OutGoingCommand;
        
        public IceChatScript()
        {            
            this.OutGoingCommand += new OutGoingCommandDelegate(IceChatScript_OutGoingCommand);
        }
        
        public void SendCommand(string command, object connection)
        {
            System.Diagnostics.Debug.WriteLine("fire send command from IceChatScript Class");
            if (OutGoingCommand != null)
            {
                System.Diagnostics.Debug.WriteLine("run outgoingcommand");
                OutGoingCommand(command, connection);
            }
        }

        void IceChatScript_OutGoingCommand(string command, object connection)
        {
            System.Diagnostics.Debug.WriteLine("put it out:" + command);
        }
    }
}
