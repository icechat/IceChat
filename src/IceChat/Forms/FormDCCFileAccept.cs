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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormDCCFileAccept : Form
    {
        private IRCConnection _connection;
        private string _nick;
        private string _host;
        private string _port;
        private string _ip;
        private string _file;
        private uint _fileSize;
        private uint _filePos;
        private bool _resume;
        private string _id;

        internal delegate void DCCFileAcceptDelegate(DialogResult result, IRCConnection connection, string nick, string host, string port, string ip, string file, uint fileSize, uint filePos, bool resume,  string id);
        internal event DCCFileAcceptDelegate DCCFileAcceptResult;

        public FormDCCFileAccept(IRCConnection connection, string nick, string host, string port, string ip, string file, uint fileSize, bool resume, uint filePos)
        {
            //for normal dcc connections
            InitializeComponent();

            labelUser.Text = nick + "@" + host + " is trying to send you a file";
            labelFile.Text = file + " (" + fileSize.ToString() + " bytes)";

            _connection = connection;
            _nick = nick;
            _host = host;
            _port = port;
            _ip = ip;
            _file = file;
            _fileSize = fileSize;
            _filePos = filePos;
            _resume = resume;
        }

        public FormDCCFileAccept(IRCConnection connection, string nick, string host, string port, string ip, string file, uint fileSize, uint filePos, bool resume, string id)
        {
            //for passive dcc connections
            InitializeComponent();

            labelUser.Text = nick + "@" + host + " is trying to send you a file";
            labelFile.Text = file + " (" + fileSize.ToString() + " bytes)";

            _connection = connection;
            _nick = nick;
            _host = host;
            _port = port;
            _ip = ip;
            _file = file;
            _fileSize = fileSize;
            _filePos = filePos;
            _resume = resume;
            _id = id;
        }

        private void ButtonAccept_Click(object sender, EventArgs e)
        {
            if (DCCFileAcceptResult != null)
                DCCFileAcceptResult(DialogResult.Yes, _connection, _nick, _host, _port, _ip, _file, _fileSize, _filePos, _resume, _id);

            this.Close();
        }

        private void ButtonReject_Click(object sender, EventArgs e)
        {
            if (DCCFileAcceptResult != null)
                DCCFileAcceptResult(DialogResult.No, _connection, _nick, _host, _port, _ip, _file, _fileSize, _filePos, _resume, _id);

            this.Close();
        }

        private void ButtonIgnore_Click(object sender, EventArgs e)
        {
            if (DCCFileAcceptResult != null)
                DCCFileAcceptResult(DialogResult.Ignore, _connection, _nick, _host, _port, _ip, _file, _fileSize, _filePos, _resume, _id);

            this.Close();

        }
    }
}
