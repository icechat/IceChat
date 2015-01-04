using System;
using System.Collections.Generic;
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


using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormList : Form
    {
        internal delegate void SearchChannelDelegate(string command);
        internal event SearchChannelDelegate SearchChannelCommand;

        public FormList()
        {
            InitializeComponent();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (SearchChannelCommand != null)
            {
                string command = "";
                
                if (textMaximum.Text.Length > 0)
                    command += "<" + textMaximum.Text;
                if (textMinimum.Text.Length > 0)
                {
                    if (command.Length > 0)
                        command += ",";
                    command += ">" + textMinimum.Text;
                }
                if (textMatch.Text.Length > 0)
                {
                    if (command.Length > 0)
                        command += " ";
                    command += textMatch.Text + "*";

                }

                SearchChannelCommand("/list " + command);
            }

            this.Close();
        }
    }
}
