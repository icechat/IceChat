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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormUserInfo : Form
    {
        private string _nick;
        private IRCConnection _connection;

        private delegate void ChangeValueDelegate(string text);

        public FormUserInfo(IRCConnection connection)
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(OnFormClosing);
            this.Load+=new EventHandler(OnLoad);
            
            _connection = connection;
            _connection.UserInfoWindow = this;

            ApplyLanguage();
        }

        private void ApplyLanguage()
        {

        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            _connection.UserInfoWindow = null;
        }
        
        public void NickName(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(NickName);
                this.Invoke(c, new object[] { value });
            }
            else
            {
                _nick = value;
                this.Text = "User Information: " + _nick;
                this.textNick.Text = value;
                FormMain.Instance.ParseOutGoingCommand(_connection, "/whois " + _nick + " " + _nick);
            }
        }

        public string Nick
        {
            get { return _nick; }
        }

        public void HostName(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(HostName);
                this.Invoke(c, new object[] { value });
            }
            else
            {
                this.listChannels.Items.Clear();
                this.textHost.Text = value;
            }
        }

        public void LoggedIn(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(LoggedIn);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textLoggedIn.Text = value;
        }

        public void FullName(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(FullName);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textFullName.Text = value;
        }

        public void IdleTime(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(IdleTime);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textIdleTime.Text = value;
        }

        public void LogonTime(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(LogonTime);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textLogonTime.Text = value;
        }

        public void Channel(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(Channel);
                this.Invoke(c, new object[] { value });
            }
            else
                this.listChannels.Items.Add(value);
        }

        public void AwayStatus(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(AwayStatus);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textAwayStatus.Text = value;

        }

        public void Server(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(Server);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textServer.Text = value;

        }

        public void CtcpReply(string value)
        {
            if (this.InvokeRequired)
            {
                ChangeValueDelegate c = new ChangeValueDelegate(CtcpReply);
                this.Invoke(c, new object[] { value });
            }
            else
                this.textCtcpReply.Text = value;

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPing_Click(object sender, EventArgs e)
        {
            textCtcpReply.Text = "Waiting for reply...";
            FormMain.Instance.ParseOutGoingCommand(_connection, "/ping " + _nick);
        }

        private void buttonVersion_Click(object sender, EventArgs e)
        {
            textCtcpReply.Text = "Waiting for reply...";
            FormMain.Instance.ParseOutGoingCommand(_connection, "/version " + _nick);
        }

        private void textSendMessage_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //send the message
                FormMain.Instance.ParseOutGoingCommand(_connection, "/msg " + _nick + " " + textSendMessage.Text);
                textSendMessage.Text = "";
            }
        }
    }
}
