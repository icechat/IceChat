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

            RemoveAdvancedTabs();

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

            LoadDefaultServerSettings();

            this.textNickName.Text = textDefaultNick.Text;
            this.textIdentName.Text = textDefaultIdent.Text;
            this.textFullName.Text = textDefaultFullName.Text;
            this.textQuitMessage.Text = textDefaultQuitMessage.Text;
            this.textPingTimer.Text = "5";
            this.textServerPort.Text = "6667";

            this.checkAccountNotify.Checked = true;
            this.checkAwayNotify.Checked = true;
            this.checkExtendedJoin.Checked = true;
            this.checkRejoinChannel.Checked = true;
            this.checkModeI.Checked = true;


            this.checkAdvancedSettings.CheckedChanged += new System.EventHandler(this.checkAdvancedSettings_CheckedChanged);

            ApplyLanguage();
        }

        private void FormServers_Load(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
            
        }

        private void textDisplayName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.K)
                {
                    textDisplayName.SelectedText = ((char)3).ToString();
                    e.Handled = true;
                }
            }
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

            this.Text = "Server Editor: " + s.ServerName;
            
            LoadDefaultServerSettings();

            if (!s.AdvancedSettings)
                RemoveAdvancedTabs();
            else
                checkAdvancedSettings.Checked = true;

            this.checkAdvancedSettings.CheckedChanged += new System.EventHandler(this.checkAdvancedSettings_CheckedChanged);

            ApplyLanguage();
        }

        private void RemoveAdvancedTabs()
        {
            this.tabControlSettings.TabPages.Remove(tabPageProxy);
            this.tabControlSettings.TabPages.Remove(tabPageBNC);
            this.tabControlSettings.TabPages.Remove(tabPageNotes);
            this.tabControlSettings.TabPages.Remove(tabPageIgnore);
        }

        private void AddAdvancedTabs()
        {
            this.tabControlSettings.TabPages.Remove(tabPageDefault);

            this.tabControlSettings.TabPages.Add(tabPageIgnore);
            this.tabControlSettings.TabPages.Add(tabPageNotes);
            this.tabControlSettings.TabPages.Add(tabPageProxy);
            this.tabControlSettings.TabPages.Add(tabPageBNC);

            this.tabControlSettings.TabPages.Add(tabPageDefault);
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
            this.textDisplayName.Text = serverSetting.DisplayName.Replace("&#x3;",((char)3).ToString());
            
            this.textIdentName.Text = serverSetting.IdentName;
            this.textFullName.Text = serverSetting.FullName;
            this.textQuitMessage.Text = serverSetting.QuitMessage;
            
            this.checkAutoJoin.Checked = serverSetting.AutoJoinEnable;
            this.checkAutoJoinDelay.Checked = serverSetting.AutoJoinDelay;
            this.checkAutoJoinDelayBetween.Checked = serverSetting.AutoJoinDelayBetween;
            this.checkAutoPerform.Checked = serverSetting.AutoPerformEnable;
            this.checkIgnore.Checked = serverSetting.IgnoreListEnable;
            this.checkBuddyList.Checked = serverSetting.BuddyListEnable;

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


            this.checkUseSASL.Checked = serverSetting.UseSASL;
            this.textSASLUser.Text = serverSetting.SASLUser;
            this.textSASLPass.Text = serverSetting.SASLPass;
            this.checkExtendedJoin.Checked = serverSetting.ExtendedJoin;
            this.checkAccountNotify.Checked = serverSetting.AccountNotify;
            this.checkAwayNotify.Checked = serverSetting.AwayNotify;

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
                                ListViewItem lvi = new ListViewItem(chan);
                                lvi.Checked = true;
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
                    textAutoPerform.AppendText(command + Environment.NewLine);
            }

            if (serverSetting.IgnoreList != null)
            {
                foreach (string ignore in serverSetting.IgnoreList)
                {
                    if (!ignore.StartsWith(";"))
                    {
                        ListViewItem lvi = new ListViewItem(ignore);
                        lvi.Checked = true;
                        listIgnore.Items.Add(lvi);
                    }
                    else
                        listIgnore.Items.Add(ignore.Substring(1));
                }
            }

            if (serverSetting.BuddyList != null)
            {
                foreach (BuddyListItem buddy in serverSetting.BuddyList)
                {
                    if (!buddy.Nick.StartsWith(";"))
                    {
                        ListViewItem lvi = new ListViewItem(buddy.Nick);
                        lvi.Checked = true;
                        listBuddyList.Items.Add(lvi);
                    }
                    else
                        listBuddyList.Items.Add(buddy.Nick.Substring(1));
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

            if (textServername.Text.Length == 0)
            {
                MessageBox.Show("You do not have a server name");
                return false;
            }
            if (textNickName.Text.Length == 0)
            {
                MessageBox.Show("You do not have a nick name");
                return false;
            }
            
            serverSetting.NickName = textNickName.Text;
            if (textAltNickName.Text.Length > 0)
                serverSetting.AltNickName = textAltNickName.Text;
            else
                serverSetting.AltNickName = textNickName.Text + "_";

            if (textAwayNick.Text.Length > 0)
                serverSetting.AwayNickName = textAwayNick.Text;
            else
                serverSetting.AwayNickName = textNickName.Text + "[A]";

            serverSetting.ServerName = textServername.Text;
            serverSetting.DisplayName = textDisplayName.Text.Replace(((char)3).ToString(), "&#x3;");

            serverSetting.Password = textServerPassword.Text;
            serverSetting.NickservPassword = textNickservPassword.Text;

            if (textServerPort.Text.Length == 0)
                textServerPort.Text = "6667";
            serverSetting.ServerPort = textServerPort.Text;

            if (textIdentName.Text.Length == 0)
                textIdentName.Text = textDefaultIdent.Text;
            serverSetting.IdentName = textIdentName.Text;

            if (textFullName.Text.Length == 0)
                textFullName.Text = textDefaultFullName.Text;
            serverSetting.FullName = textFullName.Text;

            if (textQuitMessage.Text.Length == 0)
                textQuitMessage.Text = textDefaultQuitMessage.Text;

            serverSetting.QuitMessage = textQuitMessage.Text;

            serverSetting.AutoJoinEnable = checkAutoJoin.Checked;
            serverSetting.AutoJoinDelay = checkAutoJoinDelay.Checked;
            serverSetting.AutoJoinDelayBetween = checkAutoJoinDelayBetween.Checked;
            serverSetting.AutoPerformEnable = checkAutoPerform.Checked;
            serverSetting.IgnoreListEnable = checkIgnore.Checked;
            serverSetting.BuddyListEnable = checkBuddyList.Checked;

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
            
            serverSetting.IgnoreList = new string[listIgnore.Items.Count];
            for (int i = 0; i < listIgnore.Items.Count; i++)
            {
                if (listIgnore.Items[i].Checked == false)
                    serverSetting.IgnoreList[i] = ";" + listIgnore.Items[i].Text;
                else
                    serverSetting.IgnoreList[i] = listIgnore.Items[i].Text;
            }

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
                //b.Note = serverSetting.ServerName;
                serverSetting.BuddyList[i] = b;
            }

            serverSetting.AutoPerform = textAutoPerform.Text.Trim().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            
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
            if (Int32.TryParse(textPingTimer.Text, out result)) {
                serverSetting.PingTimerMinutes = Convert.ToInt32(textPingTimer.Text);
            }
            
            serverSetting.UseSASL = checkUseSASL.Checked; 
            serverSetting.SASLUser = textSASLUser.Text;
            serverSetting.SASLPass = textSASLPass.Text;
            serverSetting.ExtendedJoin = checkExtendedJoin.Checked;
            serverSetting.AccountNotify = checkAccountNotify.Checked;
            serverSetting.AwayNotify = checkAwayNotify.Checked;

            serverSetting.UseProxy = checkUseProxy.Checked;
            serverSetting.ProxyIP = textProxyIP.Text;
            serverSetting.ProxyPort = textProxyPort.Text;
            serverSetting.ProxyUser = textProxyUser.Text;
            serverSetting.ProxyPass = textProxyPass.Text;

            if (radioSocksHTTP.Checked)
                serverSetting.ProxyType = 1;
            else if (radioSocks4.Checked)
                serverSetting.ProxyType = 2;
            else if (radioSocks5.Checked)
                serverSetting.ProxyType = 3;

            serverSetting.UseBNC = checkUseBNC.Checked;
            serverSetting.BNCIP = textBNCIP.Text;
            serverSetting.BNCPort = textBNCPort.Text;
            serverSetting.BNCUser = textBNCUser.Text;
            serverSetting.BNCPass = textBNCPass.Text;

            serverSetting.ServerNotes = textNotes.Text;

            serverSetting.AdvancedSettings = checkAdvancedSettings.Checked;

            if (newServer == true)
            {
                //add in the server to the server collection
                if (NewServer != null)
                    NewServer(serverSetting);
            }
            else
                if (SaveServer != null)
                    SaveServer(serverSetting, false);

            return true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //save the default server settings
            SaveDefaultServerSettings();

            //save all the server settings first
            if (SaveSettings() == true)
                this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
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

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem eachItem in listChannel.SelectedItems)
                listChannel.Items.Remove(eachItem);
        }

        private void textChannel_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                buttonAddAutoJoin.PerformClick();
            }
        }
        
        private void textIgnore_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (textIgnore.Text.Length > 0)
                {
                    ListViewItem lvi = new ListViewItem(textIgnore.Text);
                    lvi.Checked = true;
                    listIgnore.Items.Add(lvi);
                    textIgnore.Text = "";
                    textIgnore.Focus();

                    if (listIgnore.Items.Count == 1)
                        checkIgnore.Checked = true;

                }
            }        
        }

        private void textBuddy_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (textBuddy.Text.Length > 0)
                {
                    ListViewItem lvi = new ListViewItem(textBuddy.Text);
                    lvi.Checked = true;
                    listBuddyList.Items.Add(lvi);
                    textBuddy.Text = "";
                    textBuddy.Focus();
                    
                    if (listBuddyList.Items.Count == 1)
                        checkBuddyList.Checked = true;

                }
            }
        }


        private void buttonEdit_Click(object sender, EventArgs e)
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

        private void buttonRemoveServer_Click(object sender, EventArgs e)
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

        private void buttonDuplicateServer_Click(object sender, EventArgs e)
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
                ServerSetting dupe = new ServerSetting();
                
                dupe.AccountNotify = serverSetting.AccountNotify;
                dupe.AdvancedSettings = serverSetting.AdvancedSettings;
                dupe.AltNickName = serverSetting.AltNickName;
                dupe.AutoJoinChannels = serverSetting.AutoJoinChannels;
                dupe.AutoJoinDelay = serverSetting.AutoJoinDelay;
                dupe.AutoJoinDelayBetween = serverSetting.AutoJoinDelayBetween;
                dupe.AutoJoinEnable = serverSetting.AutoJoinEnable;
                dupe.AutoPerform = serverSetting.AutoPerform;
                dupe.AutoPerformEnable = serverSetting.AutoPerformEnable;
                dupe.AutoStart = serverSetting.AutoStart;
                dupe.Away = serverSetting.Away;
                dupe.AwayNickName = serverSetting.AwayNickName;
                dupe.AwayNotify = serverSetting.AwayNotify;
                dupe.AwayStart = serverSetting.AwayStart;
                dupe.BNCIP = serverSetting.BNCIP;
                dupe.BNCPass = serverSetting.BNCPass;
                dupe.BNCPort = serverSetting.BNCPort;
                dupe.BNCUser = serverSetting.BNCUser;
                dupe.BuddyList = serverSetting.BuddyList;
                dupe.BuddyListEnable = serverSetting.BuddyListEnable;
                dupe.DisableAwayMessages = serverSetting.DisableAwayMessages;
                dupe.DisableCTCP = serverSetting.DisableCTCP;
                dupe.DisableSounds = serverSetting.DisableSounds;
                dupe.DisplayName = serverSetting.DisplayName;
                dupe.Encoding = serverSetting.Encoding;
                dupe.ExtendedJoin = serverSetting.ExtendedJoin;
                dupe.FullName = serverSetting.FullName;
                dupe.IdentName = serverSetting.IdentName;
                dupe.IgnoreList = serverSetting.IgnoreList;
                dupe.IgnoreListEnable = serverSetting.IgnoreListEnable;
                dupe.IRCV3 = serverSetting.IRCV3;
                dupe.NickName = serverSetting.NickName;
                dupe.NickservPassword = serverSetting.NickservPassword;
                dupe.Password = serverSetting.Password;
                dupe.PingTimerMinutes = serverSetting.PingTimerMinutes;
                dupe.ProxyIP = serverSetting.ProxyIP;
                dupe.ProxyPass = serverSetting.ProxyPass;
                dupe.ProxyPort = serverSetting.ProxyPort;
                dupe.ProxyType = serverSetting.ProxyType;
                dupe.ProxyUser = serverSetting.ProxyUser;
                dupe.QuitMessage = serverSetting.QuitMessage;
                dupe.RejoinChannels = serverSetting.RejoinChannels;
                dupe.SASLPass = serverSetting.SASLPass;
                dupe.SASLUser = serverSetting.SASLUser;
                dupe.ServerName = serverSetting.ServerName;
                dupe.ServerNotes = serverSetting.ServerNotes;
                dupe.ServerPort = serverSetting.ServerPort;
                dupe.SetModeI = serverSetting.SetModeI;
                dupe.ShowMOTD = serverSetting.ShowMOTD;
                dupe.ShowPingPong = serverSetting.ShowPingPong;
                dupe.SSLAcceptInvalidCertificate = serverSetting.SSLAcceptInvalidCertificate;
                dupe.UseBNC = serverSetting.UseBNC;
                dupe.UseIPv6 = serverSetting.UseIPv6;
                dupe.UseProxy = serverSetting.UseProxy;
                dupe.UseSASL = serverSetting.UseSASL;
                dupe.UseSSL = serverSetting.UseSSL;

                if (NewServer != null)
                    NewServer(dupe);

                this.Close();

            }
        }

        private void buttonAddIgnore_Click(object sender, EventArgs e)
        {
            if (textIgnore.Text.Length > 0)
            {
                ListViewItem lvi = new ListViewItem(textIgnore.Text);
                lvi.Checked = true;
                listIgnore.Items.Add(lvi);
                textIgnore.Text = "";
                textIgnore.Focus();

                if (listIgnore.Items.Count == 1)
                    checkIgnore.Checked = true;

            }
        }

        private void buttonRemoveIgnore_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listIgnore.SelectedItems)
                listIgnore.Items.Remove(eachItem);

        }

        private void buttonEditIgnore_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listIgnore.SelectedItems)
            {
                textIgnore.Text = eachItem.Text;
                listIgnore.Items.Remove(eachItem);
            }
        }

        private void buttonAddBuddy_Click(object sender, EventArgs e)
        {
            if (textBuddy.Text.Length > 0)
            {
                ListViewItem lvi = new ListViewItem(textBuddy.Text);
                lvi.Checked = true;
                listBuddyList.Items.Add(lvi);
                textBuddy.Text = "";
                textBuddy.Focus();

                if (listBuddyList.Items.Count == 1)
                    checkBuddyList.Checked = true;

            }

        }

        private void buttonRemoveBuddy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
                listBuddyList.Items.Remove(eachItem);

        }

        private void buttonEditBuddy_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
            {
                textBuddy.Text = eachItem.Text;
                listBuddyList.Items.Remove(eachItem);
            }
        }

        private void checkAdvancedSettings_CheckedChanged(object sender, EventArgs e)
        {
            //add or remote BNC/Proxy/Server Notes
            if (checkAdvancedSettings.Checked)
                AddAdvancedTabs();
            else
                RemoveAdvancedTabs();
        }

        private void listChannel_DoubleClick(object sender, System.EventArgs e)
        {
            buttonEditAutoJoin.PerformClick();
        }

        private void listBuddyList_DoubleClick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem eachItem in listBuddyList.SelectedItems)
            {
                textBuddy.Text = eachItem.Text;
                listBuddyList.Items.Remove(eachItem);
            }
        }

        private void listIgnore_DoubleClick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem eachItem in listIgnore.SelectedItems)
            {
                textIgnore.Text = eachItem.Text;
                listIgnore.Items.Remove(eachItem);
            }
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
                    System.Diagnostics.Debug.WriteLine(f);
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


    }
}
