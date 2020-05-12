/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2020 Paul Vanderzee <snerf@icechat.net>
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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace IceChat
{
    public partial class FormServers : Form
    {
        private ServerSetting serverSetting;
        
        private bool newServer;

        internal delegate void NewServerDelegate(ServerSetting s);
        internal event NewServerDelegate NewServer;

        internal delegate void SaveServerDelegate(ServerSetting s, bool removeServer);
        internal event SaveServerDelegate SaveServer;

        internal delegate void SaveDefaultServerDelegate();
        internal event SaveDefaultServerDelegate SaveDefaultServer;

        public FormServers()
        {
            InitializeComponent();

            newServer = true;
            this.Load += new EventHandler(FormServers_Load);
            this.Text = "Server Editor: New Server";

            foreach (EncodingInfo ei in System.Text.Encoding.GetEncodings())
            {
                try
                {
                    comboEncoding.Items.Add(ei.Name);
                }
                catch { }
            }
            
            comboEncoding.Text = "utf-8";
            buttonRemoveServer.Enabled = false;
            buttonDuplicateServer.Enabled = false;

            LoadDefaultServerSettings();

            this.textNickName.Text = textDefaultNick.Text;
            this.textIdentName.Text = textDefaultIdent.Text;
            this.textFullName.Text = textDefaultFullName.Text;
            this.textQuitMessage.Text = textDefaultQuitMessage.Text;
            this.textPingTimer.Text = "5";
            this.textServerPort.Text = "6667";
            this.textReconnectTime.Text = "60";

            this.checkAccountNotify.Checked = false;
            this.checkAwayNotify.Checked = false;
            this.checkExtendedJoin.Checked = false;
            this.checkRejoinChannel.Checked = true;
            this.checkModeI.Checked = true;
            this.checkNoColorMode.Checked = false;

            this.SetDefaultHandlers();

            ApplyLanguage();
        }

        private void FormServers_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
            
        }

        private void Text_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.K)
                {
                    TextBox txt = sender as TextBox;
                    txt.SelectedText = ((char)3).ToString();
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.B)
                {
                    TextBox txt = sender as TextBox;
                    txt.SelectedText = ((char)2).ToString();
                    e.Handled = true;
                }
            }
        }

        private void SetDefaultHandlers()
        {

            this.textDisplayName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_KeyDown);
            this.textQuitMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_KeyDown);
            this.textFullName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_KeyDown);
            this.textAutoPerform.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_KeyDown);


            this.textServerPassword.MouseEnter += Password_MouseEnter;
            this.textServerPassword.MouseLeave += Password_MouseLeave;
            this.textServerPassword.GotFocus += Password_MouseEnter;
            this.textServerPassword.LostFocus += Password_MouseLeave;

            this.textNickservPassword.MouseEnter += Password_MouseEnter;
            this.textNickservPassword.MouseLeave += Password_MouseLeave;
            this.textNickservPassword.GotFocus += Password_MouseEnter;
            this.textNickservPassword.LostFocus += Password_MouseLeave;

            this.textSASLPass.MouseEnter += Password_MouseEnter;
            this.textSASLPass.MouseLeave += Password_MouseLeave;
            this.textSASLPass.GotFocus += Password_MouseEnter;
            this.textSASLPass.LostFocus += Password_MouseLeave;

            this.textBNCPass.MouseEnter += Password_MouseEnter;
            this.textBNCPass.MouseLeave += Password_MouseLeave;
            this.textBNCPass.GotFocus += Password_MouseEnter;
            this.textBNCPass.LostFocus += Password_MouseLeave;

            this.textProxyPass.MouseEnter += Password_MouseEnter;
            this.textProxyPass.MouseLeave += Password_MouseLeave;
            this.textProxyPass.GotFocus += Password_MouseEnter;
            this.textProxyPass.LostFocus += Password_MouseLeave;


        }

        public FormServers(ServerSetting s)
        {
            InitializeComponent();

            this.Load += new EventHandler(FormServers_Load);
            serverSetting = s;

            foreach (EncodingInfo ei in System.Text.Encoding.GetEncodings())
            {
                try
                {
                    comboEncoding.Items.Add(ei.Name);
                }
                catch { }
            }

            newServer = false;
            LoadSettings();

            this.Text = "Server Editor: " + s.ServerName + " - " + s.ID;
            
            LoadDefaultServerSettings();

            this.SetDefaultHandlers();


            //if (s.conn
            foreach (IRCConnection c in FormMain.Instance.ServerTree.ServerConnections.Values)
            {
                //see if the server is connected
                if (c.ServerSetting == s)
                {
                    if (c.IsConnected)
                    {
                        this.checkUseIPv6.Enabled = false;
                        this.checkUseSSL.Enabled = false;
                        this.checkInvalidCertificate.Enabled = false;
                        this.checkUseProxy.Enabled = false;
                        this.checkUseBNC.Enabled = false;
                        this.checkDisableLogging.Enabled = false;
                    }
                }
            }

            ApplyLanguage();
        }

        private void Password_MouseLeave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Focused == false)
            {
                ((TextBox)sender).PasswordChar = '*';
            }
        }

        private void Password_MouseEnter(object sender, EventArgs e)
        {
            ((TextBox)sender).PasswordChar = '\0'; ;
        }

        private void ApplyLanguage()
        {

        }
        
        /// <summary>
        /// Load the Default Server Settings
        /// </summary>
        private void LoadDefaultServerSettings()
        {
            textDefaultNick.Text = FormMain.Instance.IceChatOptions.DefaultNick;
            textDefaultIdent.Text = FormMain.Instance.IceChatOptions.DefaultIdent;
            textDefaultFullName.Text = FormMain.Instance.IceChatOptions.DefaultFullName;
            textDefaultQuitMessage.Text = FormMain.Instance.IceChatOptions.DefaultQuitMessage;

            checkIdentServer.Checked = FormMain.Instance.IceChatOptions.IdentServer;
            checkServerReconnect.Checked = FormMain.Instance.IceChatOptions.ReconnectServer;

        }
        
        /// <summary>
        /// Save the Default Server Settings
        /// </summary>
        private void SaveDefaultServerSettings()
        {
            FormMain.Instance.IceChatOptions.DefaultNick = textDefaultNick.Text;
            FormMain.Instance.IceChatOptions.DefaultIdent = textDefaultIdent.Text;
            FormMain.Instance.IceChatOptions.DefaultFullName = textDefaultFullName.Text;
            FormMain.Instance.IceChatOptions.DefaultQuitMessage = textDefaultQuitMessage.Text;

            FormMain.Instance.IceChatOptions.IdentServer = checkIdentServer.Checked;
            FormMain.Instance.IceChatOptions.ReconnectServer = checkServerReconnect.Checked;

            if (SaveDefaultServer != null)
                SaveDefaultServer();
        }

        /// <summary>
        /// Load the Server Settings into the text boxes
        /// </summary>
        private void LoadSettings()
        {
            this.textNickName.Text = serverSetting.NickName;
            this.textAltNickName.Text = serverSetting.AltNickName;
            this.textAwayNick.Text = serverSetting.AwayNickName;
            this.textServername.Text = serverSetting.ServerName;
            this.textServerPort.Text = serverSetting.ServerPort;
            
            this.textDisplayName.Text = serverSetting.DisplayName.Replace("&#x3;",((char)3).ToString()).Replace("&#x2;",((char)2).ToString());
            this.textFullName.Text = serverSetting.FullName.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString());
            this.textQuitMessage.Text = serverSetting.QuitMessage.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString());
            
            this.textIdentName.Text = serverSetting.IdentName;
            
            this.checkAutoJoin.Checked = serverSetting.AutoJoinEnable;
            this.checkAutoJoinDelay.Checked = serverSetting.AutoJoinDelay;
            this.checkAutoJoinDelayBetween.Checked = serverSetting.AutoJoinDelayBetween;
            this.checkAutoPerform.Checked = serverSetting.AutoPerformEnable;
            this.checkIgnore.Checked = serverSetting.IgnoreListEnable;
            this.checkBuddyList.Checked = serverSetting.BuddyListEnable;
            this.checkDisableLogging.Checked = serverSetting.DisableLogging;
            this.checkDisableQueries.Checked = serverSetting.DisableQueries;

            this.checkNoColorMode.Checked = serverSetting.NoColorMode;

            this.checkModeI.Checked = serverSetting.SetModeI;
            this.checkMOTD.Checked = serverSetting.ShowMOTD;
            this.checkPingPong.Checked = serverSetting.ShowPingPong;
            this.checkRejoinChannel.Checked = serverSetting.RejoinChannels;
            this.checkDisableCTCP.Checked = serverSetting.DisableCTCP;
            this.checkDisableAwayMessages.Checked = serverSetting.DisableAwayMessages;
            this.comboEncoding.Text = serverSetting.Encoding;
            this.textServerPassword.Text = serverSetting.Password;
            this.textNickservPassword.Text = serverSetting.NickservPassword;
            this.checkAutoStart.Checked = serverSetting.AutoStart;
            this.checkUseSSL.Checked = serverSetting.UseSSL;
            this.checkUseIPv6.Checked = serverSetting.UseIPv6;
            this.checkInvalidCertificate.Checked = serverSetting.SSLAcceptInvalidCertificate;
            this.textPingTimer.Text = serverSetting.PingTimerMinutes.ToString();
            this.textReconnectTime.Text = serverSetting.ReconnectTime.ToString();

            this.checkUseSASL.Checked = serverSetting.UseSASL;
            this.textSASLUser.Text = serverSetting.SASLUser;
            this.textSASLPass.Text = serverSetting.SASLPass;
            this.checkExtendedJoin.Checked = serverSetting.ExtendedJoin;
            this.checkAccountNotify.Checked = serverSetting.AccountNotify;
            this.checkAwayNotify.Checked = serverSetting.AwayNotify;
            this.checkChgHost.Checked = serverSetting.ChangeHost;
            this.checkEchoMessage.Checked = serverSetting.EchoMessage;
            
            if (serverSetting.AutoJoinChannels != null)
            {
                foreach (string chan in serverSetting.AutoJoinChannels)
                {
                    if (chan != null)
                    {
                        if (!chan.StartsWith(";"))
                        {
                            if (chan.IndexOf(' ') > -1)
                            {
                                string channel = chan.Substring(0, chan.IndexOf(' '));
                                string key = chan.Substring(chan.IndexOf(' ') + 1);
                                ListViewItem lvi = new ListViewItem(channel);
                                lvi.SubItems.Add(key);
                                lvi.Checked = true;
                                listChannel.Items.Add(lvi);
                            }
                            else
                            {
                                ListViewItem lvi = new ListViewItem(chan)
                                {
                                    Checked = true
                                };
                                listChannel.Items.Add(lvi);
                            }
                        }
                        else
                        {
                            if (chan.IndexOf(' ') > -1)
                            {
                                string channel = chan.Substring(1, chan.IndexOf(' '));
                                string key = chan.Substring(chan.IndexOf(' ') + 1);
                                ListViewItem lvi = new ListViewItem(channel);
                                lvi.SubItems.Add(key);
                                listChannel.Items.Add(lvi);
                            }
                            else
                            {
                                ListViewItem lvi = new ListViewItem(chan.Substring(1));
                                listChannel.Items.Add(lvi);
                            }
                        }
                    }
                }
            }

            if (serverSetting.AutoPerform != null)
            {
                foreach (string command in serverSetting.AutoPerform)
                    textAutoPerform.AppendText(command.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()) + Environment.NewLine);
            }

            if (serverSetting.Ignores != null)
            {
                foreach (IgnoreListItem ignore in serverSetting.Ignores)
                {
                    ListViewItem lvi = new ListViewItem(ignore.Item)
                    {
                        Checked = ignore.Enabled
                    };

                    lvi.SubItems.Add(ignore.IgnoreType.ToString() );

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.Channel == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.Private == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.Notice == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.Ctcp == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.Invite == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");

                    if (ignore.IgnoreType.All == true || ignore.IgnoreType.DCC == true)
                        lvi.SubItems.Add("x");
                    else
                        lvi.SubItems.Add("");


                    listIgnore.Items.Add(lvi);

                }
            }

            if (serverSetting.BuddyList != null)
            {
                foreach (BuddyListItem buddy in serverSetting.BuddyList)
                {
                    if (buddy != null)
                    {
                        if (!buddy.Nick.StartsWith(";"))
                        {
                            ListViewItem lvi = new ListViewItem(buddy.Nick)
                            {
                                Checked = true
                            };
                            listBuddyList.Items.Add(lvi);
                        }
                        else
                            listBuddyList.Items.Add(buddy.Nick.Substring(1));
                    }
                }
            }

            checkUseProxy.Checked = serverSetting.UseProxy;
            textProxyIP.Text = serverSetting.ProxyIP;
            textProxyPort.Text = serverSetting.ProxyPort;
            textProxyUser.Text = serverSetting.ProxyUser;
            textProxyPass.Text = serverSetting.ProxyPass;

            if (serverSetting.ProxyType == 1)
                radioSocksHTTP.Checked = true;
            else if (serverSetting.ProxyType == 2)
                radioSocks4.Checked = true;
            else if (serverSetting.ProxyType == 3)
                radioSocks5.Checked = true;

            checkUseBNC.Checked = serverSetting.UseBNC;
            textBNCIP.Text = serverSetting.BNCIP;
            textBNCPort.Text = serverSetting.BNCPort;
            textBNCUser.Text = serverSetting.BNCUser;
            textBNCPass.Text = serverSetting.BNCPass;
            
            this.textNotes.Text = serverSetting.ServerNotes;

        }

        /// <summary>
        /// Update the Server Settings
        /// </summary>
        private bool SaveSettings()
        {
            if (serverSetting == null)
                serverSetting = new ServerSetting();

            if (textServername.Text.Trim().Length == 0)
            {
                MessageBox.Show("You do not have a server name");
                return false;
            }
            if (textNickName.Text.Trim().Length == 0)
            {
                MessageBox.Show("You do not have a nick name");
                return false;
            }
            
            serverSetting.NickName = textNickName.Text.Trim();
            if (textAltNickName.Text.Trim().Length > 0)
                serverSetting.AltNickName = textAltNickName.Text;
            else
                serverSetting.AltNickName = textNickName.Text.Trim() + "_";

            serverSetting.AwayNickName = textAwayNick.Text.Trim();

            serverSetting.ServerName = textServername.Text.Trim();
            serverSetting.DisplayName = textDisplayName.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;");

            if (textFullName.Text.Trim().Length == 0)
                textFullName.Text = textDefaultFullName.Text.Trim();
            serverSetting.FullName = textFullName.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;");

            if (textQuitMessage.Text.Trim().Length == 0)
                textQuitMessage.Text = textDefaultQuitMessage.Text.Trim();
            serverSetting.QuitMessage = textQuitMessage.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;");

            serverSetting.Password = textServerPassword.Text.Trim();
            serverSetting.NickservPassword = textNickservPassword.Text.Trim();

            if (textServerPort.Text.Trim().Length == 0)
                textServerPort.Text = "6667";
            serverSetting.ServerPort = textServerPort.Text.Trim();

            if (textIdentName.Text.Trim().Length == 0)
                textIdentName.Text = textDefaultIdent.Text;
            serverSetting.IdentName = textIdentName.Text.Trim();

            serverSetting.AutoJoinEnable = checkAutoJoin.Checked;
            serverSetting.AutoJoinDelay = checkAutoJoinDelay.Checked;
            serverSetting.AutoJoinDelayBetween = checkAutoJoinDelayBetween.Checked;
            serverSetting.AutoPerformEnable = checkAutoPerform.Checked;
            serverSetting.IgnoreListEnable = checkIgnore.Checked;
            serverSetting.BuddyListEnable = checkBuddyList.Checked;
            serverSetting.DisableLogging = checkDisableLogging.Checked;
            serverSetting.DisableQueries = checkDisableQueries.Checked;
            
            serverSetting.NoColorMode = checkNoColorMode.Checked;

            serverSetting.AutoJoinChannels = new string[listChannel.Items.Count];
            for (int i = 0; i < listChannel.Items.Count; i++)
            {
                if (listChannel.Items[i].Checked == false)
                {
                    if (listChannel.Items[i].SubItems.Count > 1)
                        serverSetting.AutoJoinChannels[i] = ";" + listChannel.Items[i].Text + " " + listChannel.Items[i].SubItems[1].Text;
                    else
                        serverSetting.AutoJoinChannels[i] = ";" + listChannel.Items[i].Text;
                }
                else
                {
                    if (listChannel.Items[i].SubItems.Count > 1)
                        serverSetting.AutoJoinChannels[i] = listChannel.Items[i].Text + " " + listChannel.Items[i].SubItems[1].Text;
                    else
                        serverSetting.AutoJoinChannels[i] = listChannel.Items[i].Text;
                }
            }
            
            // save the new ignore list
            serverSetting.Ignores = new IgnoreListItem[listIgnore.Items.Count];
            for (int i = 0; i < listIgnore.Items.Count; i++)
            {
                serverSetting.Ignores[i] = new IgnoreListItem
                {
                    IgnoreType = new IgnoreType(),
                    Item = listIgnore.Items[i].Text,
                    Enabled = listIgnore.Items[i].Checked
                };

                // Resave the Ignore Types                
                serverSetting.Ignores[i].IgnoreType.SetIgnore( Convert.ToInt32 ( listIgnore.Items[i].SubItems[1].Text ) );

            }

            BuddyListItem[] oldList = serverSetting.BuddyList;

            serverSetting.BuddyList = new BuddyListItem[listBuddyList.Items.Count];
            for (int i = 0; i < listBuddyList.Items.Count; i++)
            {
                BuddyListItem b = new BuddyListItem();
                
                if (listBuddyList.Items[i].Checked == false)
                {
                    b.Nick = ";" + listBuddyList.Items[i].Text;
                }
                else
                {
                    b.Nick = listBuddyList.Items[i].Text;
                }
                
                //check if was sent on old list
                foreach (BuddyListItem bo in oldList)
                {
                    if (bo != null)
                    {
                        if (bo.IsOnSent)    //was sent, so was used
                        {
                            //now check for match
                            if (bo.Nick == b.Nick)
                            {
                                b.IsOnSent = true;
                                b.IsOnReceived = bo.IsOnReceived;
                                b.Connected = bo.Connected;

                                System.Diagnostics.Debug.WriteLine("matched:" + bo.Nick);
                            }
                            else if (b.Nick.StartsWith(";") && bo.Nick == b.Nick.Substring(1))
                            {
                                //nick is now disabled
                                b.Connected = bo.Connected;
                                b.IsOnReceived = bo.IsOnReceived;
                                b.IsOnSent = false;

                                System.Diagnostics.Debug.WriteLine("matched DIS:" + bo.Nick);
                            }
                        }
                    }
                }

                serverSetting.BuddyList[i] = b;
            }

            serverSetting.AutoPerform = textAutoPerform.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;").Trim().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            
            serverSetting.SetModeI = checkModeI.Checked;
            serverSetting.ShowMOTD = checkMOTD.Checked;
            serverSetting.ShowPingPong = checkPingPong.Checked;
            serverSetting.RejoinChannels = checkRejoinChannel.Checked;
            serverSetting.DisableCTCP = checkDisableCTCP.Checked;
            serverSetting.DisableAwayMessages = checkDisableAwayMessages.Checked;
            serverSetting.AutoStart = checkAutoStart.Checked;
            serverSetting.UseIPv6 = checkUseIPv6.Checked;
            serverSetting.UseSSL = checkUseSSL.Checked;
            serverSetting.SSLAcceptInvalidCertificate = checkInvalidCertificate.Checked;
            serverSetting.Encoding = comboEncoding.Text;

            int result;
            if (Int32.TryParse(textPingTimer.Text, out result))
            {
                serverSetting.PingTimerMinutes = Convert.ToInt32(textPingTimer.Text);
            }
            else
                serverSetting.PingTimerMinutes = 5;

            if (Int32.TryParse(textReconnectTime.Text, out result))
            {
                serverSetting.ReconnectTime = Convert.ToInt32(textReconnectTime.Text);
            }
            else
                serverSetting.ReconnectTime = 60;


            serverSetting.UseSASL = checkUseSASL.Checked; 
            serverSetting.SASLUser = textSASLUser.Text;
            serverSetting.SASLPass = textSASLPass.Text;
            serverSetting.ExtendedJoin = checkExtendedJoin.Checked;
            serverSetting.AccountNotify = checkAccountNotify.Checked;
            serverSetting.AwayNotify = checkAwayNotify.Checked;
            serverSetting.ChangeHost = checkChgHost.Checked;
            serverSetting.EchoMessage = checkEchoMessage.Checked;

            serverSetting.UseProxy = checkUseProxy.Checked;
            serverSetting.ProxyIP = textProxyIP.Text.Trim();
            serverSetting.ProxyPort = textProxyPort.Text.Trim();
            serverSetting.ProxyUser = textProxyUser.Text.Trim();
            serverSetting.ProxyPass = textProxyPass.Text.Trim();

            if (radioSocksHTTP.Checked)
                serverSetting.ProxyType = 1;
            else if (radioSocks4.Checked)
                serverSetting.ProxyType = 2;
            else if (radioSocks5.Checked)
                serverSetting.ProxyType = 3;

            serverSetting.UseBNC = checkUseBNC.Checked;
            serverSetting.BNCIP = textBNCIP.Text.Trim();
            serverSetting.BNCPort = textBNCPort.Text.Trim();
            serverSetting.BNCUser = textBNCUser.Text.Trim();
            serverSetting.BNCPass = textBNCPass.Text.Trim();

            serverSetting.ServerNotes = textNotes.Text;

            if (newServer == true)
            {
                //add in the server to the server collection
                if (NewServer != null)
                    NewServer(serverSetting);
            }
            else
            {
                if (SaveServer != null)
                    SaveServer(serverSetting, false);
            }

            return true;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //save the default server settings
            SaveDefaultServerSettings();

            //save all the server settings first
            if (SaveSettings() == true)
                this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (textChannel.Text.Length > 0)
            {
                ListViewItem lvi = null;
                if (textChannel.Text.IndexOf(" ") > -1)
                {
                    string channel = textChannel.Text.Substring(0, textChannel.Text.IndexOf(' '));
                    string key = textChannel.Text.Substring(textChannel.Text.IndexOf(' ') + 1);
                    lvi = new ListViewItem(channel);
                    lvi.SubItems.Add(key);
                }
                else if (textChannel.Text.IndexOf(":") > -1)
                {
                    string channel = textChannel.Text.Substring(0, textChannel.Text.IndexOf(':'));
                    string key = textChannel.Text.Substring(textChannel.Text.IndexOf(':') + 1);
                    lvi = new ListViewItem(channel);
                    lvi.SubItems.Add(key);
                }
                else
                {
                    lvi = new ListViewItem(textChannel.Text);
                }
                
                lvi.Checked = true;                
                listChannel.Items.Add(lvi);
                textChannel.Text = "";
                textChannel.Focus();

                if (listChannel.Items.Count == 1)
                    checkAutoJoin.Checked = true;
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem eachItem in listChannel.SelectedItems)
                listChannel.Items.Remove(eachItem);
        }

        private void TextChannel_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                buttonAddAutoJoin.PerformClick();
            }
        }
        
        private void TextIgnore_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {                
                if (textIgnore.Text.Length > 0)
                {
                    buttonAddIgnore.PerformClick();
                }
            }        
        }

        private void TextBuddy_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (textBuddy.Text.Length > 0)
                {
                    buttonAddBuddy.PerformClick();
                }
            }
        }


        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listChannel.SelectedItems)
            {
                textChannel.Text = eachItem.Text;
                
                if (eachItem.SubItems.Count == 2 && eachItem.SubItems[1].Text.Length > 0)
                    textChannel.AppendText(" " + eachItem.SubItems[1].Text);
                
                listChannel.Items.Remove(eachItem);
            }
            textChannel.Focus();
        }

        private void ButtonRemoveServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Remove this Server from the Server Tree?","Remove Server", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (SaveServer != null)
                {
                    SaveServer(serverSetting, true);
                    this.Close();
                }
            }
        }

        private void ButtonDuplicateServer_Click(object sender, EventArgs e)
        {
            //duplicate the server
            
            if (MessageBox.Show("Are you sure you want to Duplicate this Server?", "Duplicate Server", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {

                if (textServername.Text.Length == 0)
                {
                    MessageBox.Show("You do not have a server name");
                    return;
                }
                if (textNickName.Text.Length == 0)
                {
                    MessageBox.Show("You do not have a nick name");
                    return;
                }

                //make a copy of the server settings
                ServerSetting dupe = new ServerSetting
                {
                    AccountNotify = serverSetting.AccountNotify,
                    AltNickName = serverSetting.AltNickName,
                    AutoJoinChannels = serverSetting.AutoJoinChannels,
                    AutoJoinDelay = serverSetting.AutoJoinDelay,
                    AutoJoinDelayBetween = serverSetting.AutoJoinDelayBetween,
                    AutoJoinEnable = serverSetting.AutoJoinEnable,
                    AutoPerform = serverSetting.AutoPerform,
                    AutoPerformEnable = serverSetting.AutoPerformEnable,
                    AutoStart = serverSetting.AutoStart,
                    Away = serverSetting.Away,
                    AwayNickName = serverSetting.AwayNickName,
                    AwayNotify = serverSetting.AwayNotify,
                    AwayStart = serverSetting.AwayStart,
                    BNCIP = serverSetting.BNCIP,
                    BNCPass = serverSetting.BNCPass,
                    BNCPort = serverSetting.BNCPort,
                    BNCUser = serverSetting.BNCUser,
                    BuddyList = serverSetting.BuddyList,
                    BuddyListEnable = serverSetting.BuddyListEnable,
                    DisableAwayMessages = serverSetting.DisableAwayMessages,
                    DisableCTCP = serverSetting.DisableCTCP,
                    DisableSounds = serverSetting.DisableSounds,
                    DisplayName = serverSetting.DisplayName,
                    Encoding = serverSetting.Encoding,
                    ExtendedJoin = serverSetting.ExtendedJoin,
                    FullName = serverSetting.FullName,
                    IdentName = serverSetting.IdentName,
                    //dupe.IgnoreList = serverSetting.IgnoreList;
                    Ignores = serverSetting.Ignores,
                    IgnoreListEnable = serverSetting.IgnoreListEnable,
                    IgnoreListUpdated = serverSetting.IgnoreListUpdated,
                    IRCV3 = serverSetting.IRCV3,
                    NickName = serverSetting.NickName,
                    NickservPassword = serverSetting.NickservPassword,
                    Password = serverSetting.Password,
                    PingTimerMinutes = serverSetting.PingTimerMinutes,
                    ProxyIP = serverSetting.ProxyIP,
                    ProxyPass = serverSetting.ProxyPass,
                    ProxyPort = serverSetting.ProxyPort,
                    ProxyType = serverSetting.ProxyType,
                    ProxyUser = serverSetting.ProxyUser,
                    QuitMessage = serverSetting.QuitMessage,
                    RejoinChannels = serverSetting.RejoinChannels,
                    SASLPass = serverSetting.SASLPass,
                    SASLUser = serverSetting.SASLUser,
                    ServerName = serverSetting.ServerName,
                    ServerNotes = serverSetting.ServerNotes,
                    ServerPort = serverSetting.ServerPort,
                    SetModeI = serverSetting.SetModeI,
                    ShowMOTD = serverSetting.ShowMOTD,
                    ShowPingPong = serverSetting.ShowPingPong,
                    SSLAcceptInvalidCertificate = serverSetting.SSLAcceptInvalidCertificate,
                    UseBNC = serverSetting.UseBNC,
                    UseIPv6 = serverSetting.UseIPv6,
                    UseProxy = serverSetting.UseProxy,
                    UseSASL = serverSetting.UseSASL,
                    UseSSL = serverSetting.UseSSL
                };

                if (NewServer != null)
                    NewServer(dupe);

                this.Close();

            }
        }

        private void ButtonAddIgnore_Click(object sender, EventArgs e)
        {
            if (textIgnore.Text.Length > 0)
            {
                ListViewItem lvi = new ListViewItem
                {
                    Text = textIgnore.Text,
                    Checked = true
                };

                lvi.SubItems.Add("0");  // add default Ignore All
                
                lvi.SubItems.Add("x");
                lvi.SubItems.Add("x");
                lvi.SubItems.Add("x");
                lvi.SubItems.Add("x");
                lvi.SubItems.Add("x");
                lvi.SubItems.Add("x");

                listIgnore.Items.Add(lvi);

                textIgnore.Text = "";
                textIgnore.Focus();

                if (listIgnore.Items.Count == 1)
                    checkIgnore.Checked = true;

            }
        }

        private void ButtonRemoveIgnore_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listIgnore.SelectedItems)
                listIgnore.Items.Remove(eachItem);

        }

        private void ButtonEditIgnore_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listIgnore.SelectedItems)
            {
                FormIgnoreItem fii = new FormIgnoreItem(eachItem);
                fii.UpdateIgnoreList += new FormIgnoreItem.UpdateIgnoreListDelegate(UpdateIgnoreList);
                fii.ShowDialog(this);
            }
        }

        private void UpdateIgnoreList(ListViewItem lvi)
        {
            foreach (ListViewItem eachItem in listIgnore.Items)
            {
                if (eachItem == lvi)
                {
                    eachItem.Text = lvi.Text;
                    
                    eachItem.SubItems[1].Text = lvi.SubItems[1].Text;
                    eachItem.SubItems[2].Text = lvi.SubItems[2].Text;
                    eachItem.SubItems[3].Text = lvi.SubItems[3].Text;
                    eachItem.SubItems[4].Text = lvi.SubItems[4].Text;
                    eachItem.SubItems[5].Text = lvi.SubItems[5].Text;
                    eachItem.SubItems[6].Text = lvi.SubItems[6].Text;
                    eachItem.SubItems[7].Text = lvi.SubItems[7].Text;

                }

            }
        }

        private void ButtonAddBuddy_Click(object sender, EventArgs e)
        {
            if (textBuddy.Text.Length > 0)
            {
                ListViewItem lvi = new ListViewItem(textBuddy.Text)
                {
                    Checked = true
                };
                listBuddyList.Items.Add(lvi);
                textBuddy.Text = "";
                textBuddy.Focus();

                if (listBuddyList.Items.Count == 1)
                    checkBuddyList.Checked = true;

            }

        }

        private void ButtonRemoveBuddy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
                listBuddyList.Items.Remove(eachItem);

        }

        private void ButtonEditBuddy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
            {
                textBuddy.Text = eachItem.Text;
                listBuddyList.Items.Remove(eachItem);
            }
        }

        private void ListChannel_DoubleClick(object sender, System.EventArgs e)
        {
            buttonEditAutoJoin.PerformClick();
        }

        private void ListBuddyList_DoubleClick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
            {
                textBuddy.Text = eachItem.Text;
                listBuddyList.Items.Remove(eachItem);
            }
        }

        private void ListIgnore_DoubleClick(object sender, System.EventArgs e)
        {
            buttonEditIgnore.PerformClick();
        }


        private void DetectTor()
        {
            System.Diagnostics.Process[] pArry = System.Diagnostics.Process.GetProcesses();

            foreach (System.Diagnostics.Process p in pArry)
            {
                string s = p.ProcessName;
                s = s.ToLower();

                if (s.CompareTo("vidalia") == 0)
                {
                    //tor client found
                    //get the folder
                    string f = System.IO.Path.GetDirectoryName(p.Modules[0].FileName);
                    //System.Diagnostics.Debug.WriteLine(f);
                    if (f.Length > 0)
                    {
                        //f = Directory.GetParent(f) + Path.DirectorySeparatorChar.ToString() + "Data" + Path.DirectorySeparatorChar.ToString() + "Vidalia";
                        string vidaliaConf = Directory.GetParent(f) + Path.DirectorySeparatorChar.ToString() + "Data" + Path.DirectorySeparatorChar.ToString() + "Vidalia" + Path.DirectorySeparatorChar.ToString() + "vidalia.conf";

                        if (File.Exists(vidaliaConf))
                        {
                            string torrcConf = "";
                            using (StreamReader sr = new StreamReader(vidaliaConf))
                            {
                                String line;
                                // Read and display lines from the file until the end of 
                                // the file is reached.
                                while ((line = sr.ReadLine()) != null)
                                {
                                    //Console.WriteLine(line);
                                    if (line.StartsWith("Torrc="))
                                    {
                                        //System.Diagnostics.Debug.WriteLine(line.Substring(6));
                                        string torrc = line.Substring(6);
                                        //System.Diagnostics.Debug.WriteLine(torrc);
                                        //string path = Path.Combine(@f,@torrc);                                                        
                                        //System.Diagnostics.Debug.WriteLine(new DirectoryInfo(path).FullName);
                                        //System.Diagnostics.Debug.WriteLine(Path.GetFullPath(path));
                                        //System.Diagnostics.Debug.WriteLine(Path.GetFullPath(Path.Combine(f + Path.DirectorySeparatorChar.ToString(), line.Substring(6))));

                                        DirectoryInfo dir = new DirectoryInfo(f);
                                        //System.Diagnostics.Debug.WriteLine(dir.FullName);
                                        while (torrc.StartsWith("..\\"))
                                        {
                                            dir = dir.Parent;
                                            torrc = torrc.Substring(3);
                                        }
                                        //System.Diagnostics.Debug.WriteLine(Path.GetFullPath(dir.FullName + torrc));                                                        
                                        //System.Diagnostics.Debug.WriteLine(Path.Combine(dir.FullName, torrc));
                                        torrcConf = Path.GetFullPath(dir.FullName + torrc);


                                        //return Path.Combine(dir.FullName, relativePath);

                                    }

                                }
                                sr.Close();
                                sr.Dispose();

                                if (torrcConf.Length > 0)
                                {
                                    using (StreamReader sr2 = new StreamReader(torrcConf))
                                    {
                                        String line2;
                                        // Read and display lines from the file until the end of 
                                        // the file is reached.
                                        while ((line2 = sr2.ReadLine()) != null)
                                        {
                                            System.Diagnostics.Debug.WriteLine(line2);

                                            //SocksListenAddress 127.0.0.1
                                            //SocksPort 9050

                                            //CurrentWindowMessage(connection, line2, 4, true);


                                        }

                                        sr2.Close();
                                        sr2.Dispose();
                                    }



                                }

                            }
                        }

                        //[Tor]
                        //ControlPort=
                        //Torrc=..\\Data\\Tor\\torrc
                        //find the tor config file
                        //System.Diagnostics.Debug.WriteLine("tor client found:" + f);


                    }
                    //System.Diagnostics.Debug.WriteLine(Directory.GetParent(f));
                    //C:\Users\Snerf\Downloads\Tor Browser\App
                    //C:\Users\Snerf\Downloads\Tor Browser\Data\Vidalia\vidalia.conf


                }
            }
        }

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            //move item up the list, if not at the top
            if (listChannel.SelectedItems.Count == 1 && listChannel.Items.Count > 1)
            {
                if (listChannel.SelectedItems[0].Index > 0)
                {
                    int newIndex = listChannel.SelectedItems[0].Index - 1;
                    ListViewItem item = listChannel.SelectedItems[0];
                    listChannel.Items.RemoveAt(item.Index);
                    listChannel.Items.Insert(newIndex, item);
                }
            }
            listChannel.Focus();

        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            //move the item down the list, if not at bottom
            if (listChannel.SelectedItems.Count == 1 && listChannel.Items.Count > 1)
            {
                if (listChannel.SelectedItems[0].Index < (listChannel.Items.Count - 1))
                {
                    int newIndex = listChannel.SelectedItems[0].Index + 1;
                    ListViewItem item = listChannel.SelectedItems[0];
                    listChannel.Items.RemoveAt(item.Index);
                    listChannel.Items.Insert(newIndex, item);
                }
            }
            listChannel.Focus();
        }

        private void ButtonClearIgnores_Click(object sender, EventArgs e)
        {
            listIgnore.Items.Clear();
        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            // go to help
            switch (tabControlSettings.TabPages[tabControlSettings.SelectedIndex].Text)
            {
                case "Main Settings":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server");
                    break;

                case "Extra Settings":
                case "IRCv3":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Extra_Settings_tab");
                    break;

                case "AutoJoin":
                    System.Diagnostics.Process.Start("http://wiki.icechat.net/index.php?title=New_Server#Auto_Join_tab");
                    break;

                case "AutoPerform":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#AutoPerform_tab");
                    break;

                case "Ignore List":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Ignore_List_tab");
                    break;

                case "Buddy List":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Buddy_List_tab");
                    break;

                case "Notes":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Notes_tab");
                    break;

                case "Proxy Settings":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Proxy_Settings_tab");
                    break;

                case "BNC Settings":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#BNC_Settings_tab");
                    break;

                case "Default Server Settings":
                    System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=New_Server#Default_Server_Settings");
                    break;
            
            }


        }


    }
}
