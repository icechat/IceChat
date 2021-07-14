/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2021 Paul Vanderzee <snerf@icechat.net>
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
    public partial class QuickConnect : Form
    {
        internal delegate void QuickConnectServerDelegate(ServerSetting s);
        internal event QuickConnectServerDelegate QuickConnectServer;
        
        public QuickConnect()
        {
            InitializeComponent();
            this.Load += new EventHandler(OnLoad);
            ApplyLanguage();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
        }

        public void ApplyLanguage()
        {
            IceChatLanguage iceChatLanguage = FormMain.Instance.IceChatLanguage;

            label1.Text = iceChatLanguage.quickConnectLblServer;
            label2.Text = iceChatLanguage.quickConnectLblNick;
            label3.Text = iceChatLanguage.quickConnectLblChannel;
            buttonConnect.Text = iceChatLanguage.quickConnectButtonConnect;
            this.Text = iceChatLanguage.quickConnectTitle;
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            if (textServer.Text.Trim().Length == 0)
            {
                MessageBox.Show(FormMain.Instance.IceChatLanguage.quickConnectErrorNoServer, FormMain.Instance.IceChatLanguage.quickConnectTitle);
                return;
            }
            if (textNick.Text.Trim().Length == 0)
            {
                MessageBox.Show(FormMain.Instance.IceChatLanguage.quickConnectErrorNoNick, FormMain.Instance.IceChatLanguage.quickConnectTitle);
                return;
            }

            if (QuickConnectServer != null)
            {
                ServerSetting s = new ServerSetting();
                //check if a server:port was entered
                if (textServer.Text.IndexOf(':') != -1)
                {
                    s.ServerName = textServer.Text.Substring(0, textServer.Text.IndexOf(':'));
                    s.ServerPort = textServer.Text.Substring(textServer.Text.IndexOf(':') + 1);
                }
                else if (textServer.Text.IndexOf(' ') != -1)
                {
                    s.ServerName = textServer.Text.Substring(0, textServer.Text.IndexOf(' '));
                    s.ServerPort = textServer.Text.Substring(textServer.Text.IndexOf(' ') + 1);
                }
                else
                {
                    s.ServerName = textServer.Text;
                    s.ServerPort = "6667";
                }

                s.NickName = textNick.Text;

                if (textChannel.Text.Trim().Length > 0)
                {
                    s.AutoJoinEnable = true;
                    s.AutoJoinChannels = new string[] { textChannel.Text };
                }

                QuickConnectServer(s);
            }

            this.Close();
        }
    }
}
