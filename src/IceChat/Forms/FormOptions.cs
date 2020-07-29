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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using IceChatPlugin;

namespace IceChat
{
    public partial class FormSettings : Form
    {
        private IceChatOptions iceChatOptions;
        private IceChatFontSetting iceChatFonts;
        private IceChatEmoticon iceChatEmoticons;
        private IceChatSounds iceChatSounds;

        private ListViewItem listMoveItem = null;

        private System.Media.SoundPlayer player;

        private MP3Player mp3Player;

        internal delegate void SaveOptionsDelegate();
        internal event SaveOptionsDelegate SaveOptions;

        public FormSettings(IceChatOptions Options, IceChatFontSetting Fonts, IceChatEmoticon Emoticons, IceChatSounds Sounds, bool showFonts)
        {
            InitializeComponent();

          this.iceChatOptions = Options;
            this.iceChatFonts = Fonts;
            this.iceChatEmoticons = Emoticons;
            this.iceChatSounds = Sounds;
            
            LoadSettings();

            this.Activated += new EventHandler(OnActivated);

            if (showFonts == true)
                tabControlOptions.SelectedTab = tabFonts;

        }

        public FormSettings(IceChatOptions Options, IceChatFontSetting Fonts, IceChatEmoticon Emoticons, IceChatSounds Sounds)
        {
            InitializeComponent();

            this.iceChatOptions = Options;
            this.iceChatFonts = Fonts;
            this.iceChatEmoticons = Emoticons;
            this.iceChatSounds = Sounds;

            LoadSettings();

            this.Activated += new EventHandler(OnActivated);
            this.textNickComplete.KeyDown += new KeyEventHandler(OnKeyDown);

            pictureTSHelp.Image = StaticMethods.LoadResourceImage("help.png");
            pictureTSHelp.Click += new EventHandler(PictureTSHelp_Click);

        }

        private void PictureTSHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.90).aspx");
        }

        private void LoadSettings()
        {
            this.Load += new EventHandler(OnLoad);
            foreach (IceChatSounds.SoundEntry x in this.iceChatSounds.soundList)
            {
                listBoxSounds.Items.Add(x.Description);
            }

            this.listViewEmot.MouseDown += new MouseEventHandler(ListViewEmot_MouseDown);
            this.listViewEmot.MouseUp += new MouseEventHandler(ListViewEmot_MouseUp);
            this.textNickComplete.KeyDown += new KeyEventHandler(OnKeyDown);

            //populate the font settings
            textConsoleFont.Font = new Font(iceChatFonts.FontSettings[0].FontName, 10);
            textConsoleFont.Text = iceChatFonts.FontSettings[0].FontName;
            textConsoleFontSize.Text = iceChatFonts.FontSettings[0].FontSize.ToString();

            textChannelFont.Font = new Font(iceChatFonts.FontSettings[1].FontName, 10);
            textChannelFont.Text = iceChatFonts.FontSettings[1].FontName;
            textChannelFontSize.Text = iceChatFonts.FontSettings[1].FontSize.ToString();

            textQueryFont.Font = new Font(iceChatFonts.FontSettings[2].FontName, 10);
            textQueryFont.Text = iceChatFonts.FontSettings[2].FontName;
            textQueryFontSize.Text = iceChatFonts.FontSettings[2].FontSize.ToString();

            textNickListFont.Font = new Font(iceChatFonts.FontSettings[3].FontName, 10);
            textNickListFont.Text = iceChatFonts.FontSettings[3].FontName;
            textNickListFontSize.Text = iceChatFonts.FontSettings[3].FontSize.ToString();

            textServerListFont.Font = new Font(iceChatFonts.FontSettings[4].FontName, 10);
            textServerListFont.Text = iceChatFonts.FontSettings[4].FontName;
            textServerListFontSize.Text = iceChatFonts.FontSettings[4].FontSize.ToString();

            textInputFont.Font = new Font(iceChatFonts.FontSettings[5].FontName, 10);
            textInputFont.Text = iceChatFonts.FontSettings[5].FontName;
            textInputFontSize.Text = iceChatFonts.FontSettings[5].FontSize.ToString();

            textDockTabFont.Font = new Font(iceChatFonts.FontSettings[6].FontName, 10);
            textDockTabFont.Text = iceChatFonts.FontSettings[6].FontName;
            textDockTabSize.Text = iceChatFonts.FontSettings[6].FontSize.ToString();

            textMenuBarFont.Font = new Font(iceChatFonts.FontSettings[7].FontName, 10);
            textMenuBarFont.Text = iceChatFonts.FontSettings[7].FontName;
            textMenuBarSize.Text = iceChatFonts.FontSettings[7].FontSize.ToString();

            textChannelBarFont.Font = new Font(iceChatFonts.FontSettings[8].FontName, 10);
            textChannelBarFont.Text = iceChatFonts.FontSettings[8].FontName;
            textChannelBarSize.Text = iceChatFonts.FontSettings[8].FontSize.ToString();

            //populate the settings
            textTimeStamp.Text = iceChatOptions.TimeStamp.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()); ;

            checkTimeStamp.Checked = iceChatOptions.ShowTimeStamp;
            checkSaveWindowPosition.Checked = iceChatOptions.SaveWindowPosition;
            checkWindowedMode.Checked = iceChatOptions.WindowedMode;

            checkFlashChannelMessage.Checked = iceChatOptions.FlashTaskBarChannel;
            checkFlashChannelAction.Checked = iceChatOptions.FlashTaskBarChannelAction;

            checkFlashPrivateMessage.Checked = iceChatOptions.FlashTaskBarPrivate;
            checkFlashPrivateAction.Checked = iceChatOptions.FlashTaskBarPrivateAction;

            checkFlashChannel.Checked = iceChatOptions.FlashServerTreeIcons;
            checkFlashPrivate.Checked = iceChatOptions.FlashServerTreeIconsPrivate;

            textFlashTaskbarNumber.Text = iceChatOptions.FlashTaskBarNumber.ToString();
            checkJoinInvite.Checked = iceChatOptions.AutoJoinInvite;
            
            checkLogConsole.Checked = iceChatOptions.LogConsole;
            checkLogChannel.Checked = iceChatOptions.LogChannel;
            checkLogQuery.Checked = iceChatOptions.LogQuery;
            checkLogWindow.Checked = iceChatOptions.LogWindow;
            checkSeperateLogs.Checked = iceChatOptions.SeperateLogs;
            comboLogFormat.Text = iceChatOptions.LogFormat;
            labelCurrentLogFolder.Text = iceChatOptions.LogFolder;
            checkReloadLogs.Checked = iceChatOptions.LogReload;

            checkExternalPlayCommand.Checked = iceChatOptions.SoundUseExternalCommand;
            textExternalPlayCommand.Text = iceChatOptions.SoundExternalCommand;
            checkPlayActive.Checked = iceChatOptions.SoundPlayActive;
            checkSortNickList.Checked = iceChatOptions.NickListSort;

            //comboLineSpacing.Text = iceChatOptions.LineSpacing.ToString();

            if (iceChatEmoticons != null)
            {
                //load in the emoticons
                foreach (EmoticonItem emot in iceChatEmoticons.listEmoticons)
                {
                    Bitmap bm = new Bitmap(FormMain.Instance.EmoticonsFolder + System.IO.Path.DirectorySeparatorChar + emot.EmoticonImage);
                    int i = imageListEmoticons.Images.Add(bm, Color.Fuchsia);
                    ListViewItem lvi = new ListViewItem(emot.Trigger, i);
                    lvi.SubItems.Add(emot.EmoticonImage);
                    listViewEmot.Items.Add(lvi);
                }
            }

            checkEmoticons.Checked = iceChatOptions.ShowEmoticons;
            checkEmoticonPicker.Checked = iceChatOptions.ShowEmoticonPicker;
            checkColorPicker.Checked = iceChatOptions.ShowColorPicker;
            checkBasicCommands.Checked = iceChatOptions.ShowBasicCommands;
            checkStatusBar.Checked = iceChatOptions.ShowStatusBar;
            checkDisableQueries.Checked = iceChatOptions.DisableQueries;
            checkNewQueryForegound.Checked = iceChatOptions.NewQueryForegound;
            checkWhoisNewQuery.Checked = iceChatOptions.WhoisNewQuery;
            checkShowUnreadLine.Checked = iceChatOptions.ShowUnreadLine;
            checkMinimizeTray.Checked = iceChatOptions.MinimizeToTray;
            checkShowTrayIcon.Checked = iceChatOptions.ShowSytemTrayIcon;
            checkShowTrayNotifications.Checked = iceChatOptions.ShowSytemTrayNotifications;
            checkTrayServerMessage.Checked = iceChatOptions.SystemTrayServerMessage;
            checkTrayBuddyOnline.Checked = iceChatOptions.SystemTrayBuddyOnline;
            textSystemTrayText.Text = iceChatOptions.SystemTrayText;
            textSystemTrayIcon.Text = iceChatOptions.SystemTrayIcon;

            checkShowSend.Checked = iceChatOptions.ShowSendButton;

            checkAskQuit.Checked = iceChatOptions.AskQuit;
            textMaximumLines.Text = iceChatOptions.MaximumTextLines.ToString();
            textNickComplete.Text = iceChatOptions.NickCompleteAfter.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString());
            
            checkShowNickHost.Checked = iceChatOptions.ShowNickHost;
            checkNickShowButtons.Checked = iceChatOptions.ShowNickButtons;
            checkServerShowButtons.Checked = iceChatOptions.ShowServerButtons;
            checkKickChannelOpen.Checked = iceChatOptions.ChannelOpenKick;
            checkTopicBar.Checked = iceChatOptions.ShowTopic;
            checkSingleRowCB.Checked = iceChatOptions.SingleRowTabBar;

            //dcc settings
            checkAutoDCCChat.Checked = iceChatOptions.DCCChatAutoAccept;
            checkAutoDCCFile.Checked = iceChatOptions.DCCFileAutoAccept;
            checkAutoDCCChatBuddy.Checked = iceChatOptions.DCCChatAutoAcceptBuddyOnly;
            checkAutoDCCFileBuddy.Checked = iceChatOptions.DCCFileAutoAcceptBuddyOnly;
            checkIgnoreDCCChat.Checked = iceChatOptions.DCCChatIgnore;
            checkIgnoreDCCFile.Checked = iceChatOptions.DCCFileIgnore;
            textDCCChatTimeout.Text = iceChatOptions.DCCChatTimeOut.ToString();
            textDCCPortLow.Text = iceChatOptions.DCCPortLower.ToString();
            textDCCPortHigh.Text = iceChatOptions.DCCPortUpper.ToString();
            textDCCReceiveFolder.Text = iceChatOptions.DCCReceiveFolder;
            textDCCSendFolder.Text = iceChatOptions.DCCSendFolder;
            textDCCLocalIP.Text = iceChatOptions.DCCLocalIP;
            comboBufferSize.Text = iceChatOptions.DCCBufferSize.ToString();
            checkAutoGetLocalIP.Checked = iceChatOptions.DCCAutogetRouterIP;

            comboBoxLanguage.DataSource = FormMain.Instance.IceChatLanguageFiles;
            comboBoxLanguage.SelectedItem = FormMain.Instance.IceChatCurrentLanguageFile;

            //Event Settings
            comboJoinEvent.SelectedIndex = iceChatOptions.JoinEventLocation;
            comboPartEvent.SelectedIndex = iceChatOptions.PartEventLocation;
            comboQuitEvent.SelectedIndex = iceChatOptions.QuitEventLocation;
            comboModeEvent.SelectedIndex = iceChatOptions.ModeEventLocation;
            comboKickEvent.SelectedIndex = iceChatOptions.KickEventLocation;
            comboTopicEvent.SelectedIndex = iceChatOptions.TopicEventLocation;
            comboChannelMessageEvent.SelectedIndex = iceChatOptions.ChannelMessageEventLocation;
            comboChannelActionEvent.SelectedIndex = iceChatOptions.ChannelActionEventLocation;
            comboChannelNoticeEvent.SelectedIndex = iceChatOptions.ChannelNoticeEventLocation;
            comboWhoisEvent.SelectedIndex = iceChatOptions.WhoisEventLocation;
            comboNickEvent.SelectedIndex = iceChatOptions.NickChangeEventLocation;

            comboServerMessageEvent.SelectedIndex = iceChatOptions.ServerMessageEventLocation;
            comboServerNoticeEvent.SelectedIndex = iceChatOptions.ServerNoticeEventLocation;
            comboServerErrorEvent.SelectedIndex = iceChatOptions.ServerErrorEventLocation;
            comboUserNoticeEvent.SelectedIndex = iceChatOptions.UserNoticeEventLocation;


            //away settings
            textAwayCommand.Text = iceChatOptions.AwayCommand;
            textReturnCommand.Text = iceChatOptions.ReturnCommand;
            checkSendAwayReturn.Checked = iceChatOptions.SendAwayCommands;
            checkAwayMessagePrivate.Checked = iceChatOptions.SendAwayPrivateMessage;
            textAwayPrivateMessage.Text = iceChatOptions.PrivateAwayMessage;
            checkAutoAway.Checked = iceChatOptions.AutoAway;
            checkAutoReturn.Checked = iceChatOptions.AutoReturn;
            textAutoAwayMinutes.Text = iceChatOptions.AutoAwayTime.ToString();
            checkAutoAwayTray.Checked = iceChatOptions.AutoAwaySystemTray;
            textAutoAwayMessage.Text = iceChatOptions.AutoAwayMessage;

            checkAutoPerformStartup.Checked = iceChatOptions.AutoPerformStartupEnable;
            if (iceChatOptions.AutoPerformStartup != null)
            {
                foreach (string command in iceChatOptions.AutoPerformStartup)
                    textAutoPerformStartup.AppendText(command + Environment.NewLine);
            }

            trackTransparency.Value = Convert.ToInt32(FormMain.Instance.Opacity * 100);

            ApplyLanguage();

            if (!StaticMethods.IsRunningOnMono())
                mp3Player = new MP3Player();
            else
                player = new System.Media.SoundPlayer();

        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
        }

        private void OnActivated(object sender, EventArgs e)
        {

            //load any plugin addons
            foreach (Plugin p in FormMain.Instance.LoadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                        ipc.plugin.LoadSettingsForm(this.tabControlOptions);
                }
            }

            this.Activated -= OnActivated;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.K)
                {
                    ((TextBox)sender).SelectedText = ((char)3).ToString();
                    e.Handled = true;
                }
                if (e.KeyCode == Keys.B)
                {
                    ((TextBox)sender).SelectedText = ((char)2).ToString();
                    e.Handled = true;
                }
            }
        }

        private void ApplyLanguage()
        {

        }

        private void ListViewEmot_MouseUp(object sender, MouseEventArgs e)
        {
            if (listMoveItem == null)
                return;

            ListViewItem itemOver = listViewEmot.GetItemAt(e.X, e.Y);
            if (itemOver == null)
                return;
   
            listViewEmot.Items.Remove(listMoveItem);
            listViewEmot.Items.Insert(itemOver.Index + 1, listMoveItem);

            listViewEmot.Invalidate();

            listMoveItem = null;
            listViewEmot.Cursor = Cursors.Default;
        }

        private void ListViewEmot_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (ListViewItem eachItem in listViewEmot.SelectedItems)
            {
                listMoveItem = eachItem;
                listViewEmot.Cursor = Cursors.Hand;
                return;
            }
            listMoveItem = null;
            listViewEmot.Cursor = Cursors.Default;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //set all the options accordingly

            iceChatOptions.SaveWindowPosition = checkSaveWindowPosition.Checked;
            iceChatOptions.TimeStamp = textTimeStamp.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;"); ;
            iceChatOptions.ShowTimeStamp = checkTimeStamp.Checked;
            iceChatOptions.AutoJoinInvite = checkJoinInvite.Checked;
            
            iceChatOptions.LogConsole = checkLogConsole.Checked;
            iceChatOptions.LogChannel = checkLogChannel.Checked;
            iceChatOptions.LogQuery = checkLogQuery.Checked;
            iceChatOptions.LogWindow = checkLogWindow.Checked;
            iceChatOptions.SeperateLogs = checkSeperateLogs.Checked;
            iceChatOptions.LogFormat = comboLogFormat.Text;
            iceChatOptions.LogFolder = labelCurrentLogFolder.Text;
            iceChatOptions.LogReload = checkReloadLogs.Checked;

            iceChatOptions.WindowedMode = checkWindowedMode.Checked;
            iceChatOptions.ShowEmoticons = checkEmoticons.Checked;
            iceChatOptions.ShowEmoticonPicker = checkEmoticonPicker.Checked;
            iceChatOptions.ShowColorPicker = checkColorPicker.Checked;
            iceChatOptions.ShowBasicCommands = checkBasicCommands.Checked;
            iceChatOptions.ShowSendButton = checkShowSend.Checked;
            iceChatOptions.ShowStatusBar = checkStatusBar.Checked;
            iceChatOptions.DisableQueries = checkDisableQueries.Checked;
            iceChatOptions.NewQueryForegound = checkNewQueryForegound.Checked;
            iceChatOptions.WhoisNewQuery = checkWhoisNewQuery.Checked;
            iceChatOptions.AskQuit = checkAskQuit.Checked;
            iceChatOptions.SingleRowTabBar = checkSingleRowCB.Checked;
            iceChatOptions.Transparency = trackTransparency.Value;
            iceChatOptions.NickListSort = checkSortNickList.Checked;
            //iceChatOptions.LineSpacing = (float)Convert.ToDouble( comboLineSpacing.Text );
    
            iceChatOptions.FlashTaskBarChannel = checkFlashChannelMessage.Checked;
            iceChatOptions.FlashTaskBarChannelAction = checkFlashChannelAction.Checked;
            iceChatOptions.FlashTaskBarPrivate = checkFlashPrivateMessage.Checked;
            iceChatOptions.FlashTaskBarPrivateAction = checkFlashPrivateAction.Checked;
            
            iceChatOptions.FlashTaskBarNumber = Convert.ToInt32(textFlashTaskbarNumber.Text);
            iceChatOptions.FlashServerTreeIcons = checkFlashChannel.Checked;
            iceChatOptions.FlashServerTreeIconsPrivate = checkFlashPrivate.Checked;

            iceChatOptions.MaximumTextLines = Convert.ToInt32(textMaximumLines.Text);
            iceChatOptions.NickCompleteAfter = textNickComplete.Text.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;");
            iceChatOptions.ShowNickHost = checkShowNickHost.Checked;
            iceChatOptions.ShowNickButtons = checkNickShowButtons.Checked;
            iceChatOptions.ShowServerButtons = checkServerShowButtons.Checked;
            iceChatOptions.ChannelOpenKick = checkKickChannelOpen.Checked;
            iceChatOptions.ShowUnreadLine = checkShowUnreadLine.Checked;
            iceChatOptions.MinimizeToTray = checkMinimizeTray.Checked;
            iceChatOptions.ShowSytemTrayIcon = checkShowTrayIcon.Checked;
            iceChatOptions.ShowSytemTrayNotifications = checkShowTrayNotifications.Checked;
            iceChatOptions.SystemTrayText = textSystemTrayText.Text;
            iceChatOptions.SystemTrayIcon = textSystemTrayIcon.Text;

            iceChatOptions.SystemTrayServerMessage = checkTrayServerMessage.Checked;
            iceChatOptions.SystemTrayBuddyOnline = checkTrayBuddyOnline.Checked;

            iceChatOptions.Language = ((LanguageItem)comboBoxLanguage.SelectedItem).LanguageName;
            iceChatOptions.ShowTopic = checkTopicBar.Checked;

            //set all the fonts
            iceChatFonts.FontSettings[0].FontName = textConsoleFont.Text;
            iceChatFonts.FontSettings[0].FontSize = float.Parse(textConsoleFontSize.Text);

            iceChatFonts.FontSettings[1].FontName = textChannelFont.Text;
            iceChatFonts.FontSettings[1].FontSize = float.Parse(textChannelFontSize.Text);

            iceChatFonts.FontSettings[2].FontName = textQueryFont.Text;
            iceChatFonts.FontSettings[2].FontSize = float.Parse(textQueryFontSize.Text);

            iceChatFonts.FontSettings[3].FontName = textNickListFont.Text;
            iceChatFonts.FontSettings[3].FontSize = float.Parse(textNickListFontSize.Text);

            iceChatFonts.FontSettings[4].FontName = textServerListFont.Text;
            iceChatFonts.FontSettings[4].FontSize = float.Parse(textServerListFontSize.Text);

            iceChatFonts.FontSettings[5].FontName = textInputFont.Text;
            iceChatFonts.FontSettings[5].FontSize = float.Parse(textInputFontSize.Text);
            
            iceChatFonts.FontSettings[6].FontName = textDockTabFont.Text;
            iceChatFonts.FontSettings[6].FontSize = float.Parse(textDockTabSize.Text);

            iceChatFonts.FontSettings[7].FontName = textMenuBarFont.Text;
            iceChatFonts.FontSettings[7].FontSize = float.Parse(textMenuBarSize.Text);

            iceChatFonts.FontSettings[8].FontName = textChannelBarFont.Text;
            iceChatFonts.FontSettings[8].FontSize = float.Parse(textChannelBarSize.Text);

            //dcc settings
            iceChatOptions.DCCChatAutoAccept = checkAutoDCCChat.Checked;
            iceChatOptions.DCCFileAutoAccept = checkAutoDCCFile.Checked;
            iceChatOptions.DCCChatAutoAcceptBuddyOnly = checkAutoDCCChatBuddy.Checked;
            iceChatOptions.DCCFileAutoAcceptBuddyOnly = checkAutoDCCFileBuddy.Checked;
            
            iceChatOptions.DCCChatIgnore = checkIgnoreDCCChat.Checked;
            iceChatOptions.DCCFileIgnore = checkIgnoreDCCFile.Checked;
            iceChatOptions.DCCChatTimeOut = Convert.ToInt32(textDCCChatTimeout.Text);
            iceChatOptions.DCCPortLower = Convert.ToInt32(textDCCPortLow.Text);
            iceChatOptions.DCCPortUpper = Convert.ToInt32(textDCCPortHigh.Text);
            iceChatOptions.DCCReceiveFolder = textDCCReceiveFolder.Text;
            iceChatOptions.DCCSendFolder = textDCCSendFolder.Text;
            iceChatOptions.DCCLocalIP = textDCCLocalIP.Text;
            iceChatOptions.DCCBufferSize = Convert.ToInt32(comboBufferSize.Text);
            iceChatOptions.DCCAutogetRouterIP = checkAutoGetLocalIP.Checked;

            //save the emoticons
            iceChatEmoticons.listEmoticons.Clear();
            //re-add them all back in
            int i = 0;
            foreach (ListViewItem lvi in listViewEmot.Items)
            {
                EmoticonItem ei = new EmoticonItem
                {
                    EmoticonImage = lvi.SubItems[1].Text,
                    Trigger = lvi.Text,
                    ID = i++
                };
                iceChatEmoticons.AddEmoticon(ei);
            }

            FormMain.Instance.IceChatEmoticons = iceChatEmoticons;

            // apply language change
            FormMain.Instance.IceChatCurrentLanguageFile = (LanguageItem) comboBoxLanguage.SelectedItem;

            //Event Settings
            iceChatOptions.JoinEventLocation = comboJoinEvent.SelectedIndex;
            iceChatOptions.PartEventLocation = comboPartEvent.SelectedIndex;
            iceChatOptions.QuitEventLocation = comboQuitEvent.SelectedIndex;
            iceChatOptions.ModeEventLocation = comboModeEvent.SelectedIndex;
            iceChatOptions.KickEventLocation = comboKickEvent.SelectedIndex;
            iceChatOptions.TopicEventLocation = comboTopicEvent.SelectedIndex;
            iceChatOptions.ChannelMessageEventLocation = comboChannelMessageEvent.SelectedIndex;
            iceChatOptions.ChannelActionEventLocation = comboChannelActionEvent.SelectedIndex;
            iceChatOptions.ChannelNoticeEventLocation = comboChannelNoticeEvent.SelectedIndex;
            iceChatOptions.WhoisEventLocation = comboWhoisEvent.SelectedIndex;
            iceChatOptions.NickChangeEventLocation = comboNickEvent.SelectedIndex;

            iceChatOptions.ServerMessageEventLocation = comboServerMessageEvent.SelectedIndex;
            iceChatOptions.ServerNoticeEventLocation = comboServerNoticeEvent.SelectedIndex;
            iceChatOptions.ServerErrorEventLocation = comboServerErrorEvent.SelectedIndex;
            iceChatOptions.UserNoticeEventLocation = comboUserNoticeEvent.SelectedIndex;

            iceChatOptions.SoundUseExternalCommand = checkExternalPlayCommand.Checked;
            iceChatOptions.SoundExternalCommand = textExternalPlayCommand.Text;
            iceChatOptions.SoundPlayActive = checkPlayActive.Checked;
            
            //away settings
            iceChatOptions.AwayCommand = textAwayCommand.Text;
            iceChatOptions.ReturnCommand = textReturnCommand.Text;
            iceChatOptions.SendAwayCommands = checkSendAwayReturn.Checked;
            iceChatOptions.SendAwayPrivateMessage = checkAwayMessagePrivate.Checked;
            iceChatOptions.PrivateAwayMessage = textAwayPrivateMessage.Text;
            iceChatOptions.AutoAway = checkAutoAway.Checked;
            iceChatOptions.AutoReturn = checkAutoReturn.Checked;
            
            if (textAutoAwayMinutes.Text.Length == 0)
            {
                iceChatOptions.AutoAwayTime = 0;
                iceChatOptions.AutoAway = false;
            }
            else
            {
                //check if value is an integer
                int result;
                if (Int32.TryParse(textAutoAwayMinutes.Text, out result))
                    iceChatOptions.AutoAwayTime = result;
                else
                {
                    iceChatOptions.AutoAwayTime = 0;
                    iceChatOptions.AutoAway = false;
                }
            }
            
            iceChatOptions.AutoAwaySystemTray = checkAutoAwayTray.Checked;
            iceChatOptions.AutoAwayMessage = textAutoAwayMessage.Text;

            iceChatOptions.AutoPerformStartupEnable = checkAutoPerformStartup.Checked;
            iceChatOptions.AutoPerformStartup = textAutoPerformStartup.Text.Trim().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (Plugin p in  FormMain.Instance.LoadedPlugins)
            {
                IceChatPlugin ipc = p as IceChatPlugin;
                if (ipc != null)
                {
                    if (ipc.plugin.Enabled == true)
                        ipc.plugin.SaveSettingsForm();
                }
            }
            if (SaveOptions != null)
                SaveOptions();

            this.Close();
        }

        private void ButtonConsoleFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                //load the current font

                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //fd.FixedPitchOnly = true; monospace

                Font = new Font(textConsoleFont.Text, float.Parse(textConsoleFontSize.Text), FontStyle.Regular)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textConsoleFont.Text = fd.Font.Name;
                    textConsoleFontSize.Text = fd.Font.Size.ToString();
                    textConsoleFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonChannelFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textChannelFont.Text, float.Parse(textChannelFontSize.Text), textChannelFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textChannelFont.Text = fd.Font.Name;
                    textChannelFontSize.Text = fd.Font.Size.ToString();
                    textChannelFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonQueryFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textQueryFont.Text, float.Parse(textQueryFontSize.Text), textQueryFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textQueryFont.Text = fd.Font.Name;
                    textQueryFontSize.Text = fd.Font.Size.ToString();
                    textQueryFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }                
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonNickListFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textNickListFont.Text, float.Parse(textNickListFontSize.Text), textNickListFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textNickListFont.Text = fd.Font.Name;
                    textNickListFontSize.Text = fd.Font.Size.ToString();
                    textNickListFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonServerListFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font            
                Font = new Font(textServerListFont.Text, float.Parse(textServerListFontSize.Text), textServerListFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textServerListFont.Text = fd.Font.Name;
                    textServerListFontSize.Text = fd.Font.Size.ToString();
                    textServerListFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonInputFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textInputFont.Text, float.Parse(textInputFontSize.Text), textInputFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textInputFont.Text = fd.Font.Name;
                    textInputFontSize.Text = fd.Font.Size.ToString();
                    textInputFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonDockTab_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textDockTabFont.Text, float.Parse(textDockTabSize.Text), textDockTabFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textDockTabFont.Text = fd.Font.Name;
                    textDockTabSize.Text = fd.Font.Size.ToString();
                    textDockTabFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonMenuBar_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textMenuBarFont.Text, float.Parse(textMenuBarSize.Text), textMenuBarFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textMenuBarFont.Text = fd.Font.Name;
                    textMenuBarSize.Text = fd.Font.Size.ToString();
                    textMenuBarFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonChannelBar_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog
            {
                ShowEffects = false,
                ShowColor = false,
                FontMustExist = true,
                AllowVectorFonts = false,
                AllowVerticalFonts = false,

                //load the current font
                Font = new Font(textChannelBarFont.Text, float.Parse(textChannelBarSize.Text), textChannelBarFont.Font.Style)
            };
            try
            {
                if (fd.ShowDialog() != DialogResult.Cancel)
                {
                    textChannelBarFont.Text = fd.Font.Name;
                    textChannelBarSize.Text = fd.Font.Size.ToString();
                    textChannelBarFont.Font = new Font(fd.Font.Name, 10, fd.Font.Style);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonAddEmoticon_Click(object sender, EventArgs e)
        {
            //add a new emoticon
            OpenFileDialog ofd = new OpenFileDialog
            {
                AutoUpgradeEnabled = false,

                InitialDirectory = FormMain.Instance.EmoticonsFolder,
                Filter = "PNG Images (*.png)|*.png",
                RestoreDirectory = true,
                Title = "Choose a PNG file for the Emoticon Image"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //check if emoticon exists                
                Bitmap bm = new Bitmap(ofd.FileName);
                int i = imageListEmoticons.Images.Add(bm, Color.Fuchsia);
                ListViewItem lvi = new ListViewItem("<edit>", i);
                lvi.SubItems.Add(ofd.SafeFileName);
                listViewEmot.Items.Add(lvi);
            }
        }

        private void ButtonRemoveEmoticon_Click(object sender, EventArgs e)
        {
            //check if one is selected and remove it
            foreach(ListViewItem eachItem in listViewEmot.SelectedItems)
                listViewEmot.Items.Remove(eachItem);
        }

        private void ButtonEditTrigger_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in listViewEmot.SelectedItems)
                eachItem.BeginEdit();
        }

        private void ButtonBrowseEmoticon_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(FormMain.Instance.EmoticonsFolder);
            }
            catch { }            
        }

        private void ListBoxSounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSounds.SelectedIndex >= 0)
            {
                textSound.Text = iceChatSounds.GetSound(listBoxSounds.SelectedIndex).File;
            }
        }

        private void ButtonChooseSound_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    AutoUpgradeEnabled = false
                };

                if (textSound.Text.Length > 0)
                {
                    //go to the same folder
                    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(textSound.Text);
                }
                else
                    ofd.InitialDirectory = FormMain.Instance.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Sounds";

                ofd.RestoreDirectory = true;
                ofd.Filter = "Sounds (*.wav;*.mp3)|*.wav;*.mp3";
                ofd.Title = "Choose an MP3 or WAV File";

                if (ofd.ShowDialog() == DialogResult.OK)
                    textSound.Text = ofd.FileName;
            }
            catch { }
        }

        private void ButtonTest_Click(object sender, EventArgs e)
        {
            if (textSound.Text.Length > 0)
            {
                try
                {
                    if (StaticMethods.IsRunningOnMono())
                    {
                        player.SoundLocation = @textSound.Text;
                        player.Play();
                    }
                    else
                    {
                        mp3Player.Open(@textSound.Text);
                        mp3Player.Play();

                    }
                }
                catch { }
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (StaticMethods.IsRunningOnMono())
                {
                    player.Stop();
                }
                else
                {
                    mp3Player.Stop();
                }
            }
            catch { }
        }


        private void TextSound_TextChanged(object sender, EventArgs e)
        {
            if (listBoxSounds.SelectedIndex >= 0)
            {
                iceChatSounds.GetSound(listBoxSounds.SelectedIndex).File = textSound.Text;
            }
        }
        /// <summary>
        /// Open the Logs Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBrowseLogs_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(FormMain.Instance.LogsFolder);
            }
            catch { }
        }
        /// <summary>
        /// Choose the default DCC Receive Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDCCReceiveFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = "Select DCC Receive Folder"
                };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textDCCReceiveFolder.Text = fbd.SelectedPath;
                }
            }
            catch { }
        }
        /// <summary>
        /// Choose the default DCC Send Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonDCCSendFolder_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = "Select DCC Send Folder"
                };
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textDCCSendFolder.Text = fbd.SelectedPath;
                }
            }
            catch { }
        }

        private void LinkWhatisMyIP_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.icechat.net/getip.php");
            }
            catch { }
        }

        private void TrackTransparency_Scroll(object sender, EventArgs e)
        {
            FormMain.Instance.Opacity = (double)trackTransparency.Value / 100;
        }

        private void ButtonResetEmoticons_Click(object sender, EventArgs e)
        {
            //reset all the emoticons, re-write the XML file
            //FormMain.Instance.EmoticonsFile
            MessageBox.Show("Not yet implemented");
            //iceChatEmoticons = new IceChatEmoticon();
            /*
            EmoticonItem ei = new EmoticonItem();
            ei.EmoticonImage = lvi.SubItems[1].Text;
            ei.Trigger = lvi.Text;
            ei.ID = i++;
            iceChatEmoticons.AddEmoticon(ei);
            */

            //FormMain.Instance.IceChatEmoticons = iceChatEmoticons;


        }

        private void ButtonChangeLogs_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog
                {
                    Description = "Select new logs folder"
                };

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    labelCurrentLogFolder.Text = fbd.SelectedPath;                    
                }
            }
            catch { }
        }

        private void ButtonTrayIcon_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    AutoUpgradeEnabled = false,

                    CheckFileExists = true,
                    CheckPathExists = true,
                    AddExtension = true,
                    DefaultExt = "ico",
                    Multiselect = false,
                    Title = "Choose Icon File"
                };

                if (textSystemTrayIcon.Text.Length > 0)
                {
                    //go to the same folder
                    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(textSystemTrayIcon.Text);
                }
                else
                    ofd.InitialDirectory = FormMain.Instance.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Pictures";

                ofd.RestoreDirectory = true;
                ofd.Filter = "Icon Files (*.ico)|*.ico";

                if (ofd.ShowDialog() == DialogResult.OK)
                    textSystemTrayIcon.Text = ofd.FileName;
            }
            catch { }

        }

        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            // get the current tab, and go to Wiki
            System.Diagnostics.Process.Start("https://wiki.icechat.net/index.php?title=Settings#" + tabControlOptions.TabPages[tabControlOptions.SelectedIndex].Tag);

        }

    }
}