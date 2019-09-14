/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2019 Paul Vanderzee <snerf@icechat.net>
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
using System.Collections.Generic;
using System.Collections;


namespace IceChatPlugin
{  

    public delegate void OutGoingCommandHandler(PluginArgs e);

    public abstract class IPluginIceChat
    {
        public abstract void Initialize();
        public abstract void Dispose();        
        
        public abstract string Name { get; }
        public abstract string Version { get; }
        public abstract string Author { get; }

        //sets the MainForm for IceChat
        public Form MainForm { get; set; }
        public string CurrentFolder { get; set; }
        public string FileName { get; set; }

        public virtual void ShowInfo()
        {
            MessageBox.Show(Name + " Loaded", Name + " " + Author);
        }

        public AppDomain domain { get; set; }
        
        //if the plugin is enabled or disabled
        public bool Enabled { get; set; }
        public bool Unloaded { get; set; }

        //the mainform menu
        public MenuStrip MainMenuStrip { get; set; }
        //the bottom panel of the main window
        public Panel BottomPanel { get; set; }
        
        public TabControl LeftPanel { get; set; }
        public TabControl RightPanel { get; set; }
        public double CurrentVersion { get; set; }


        public virtual bool HaveSettingsForm() { return false; }
        public virtual void ShowSettingsForm() { }

        public virtual void LoadSettingsForm(System.Windows.Forms.TabControl SettingsTab) { }
        public virtual void LoadColorsForm(System.Windows.Forms.TabControl ColorsTab) { }
        public virtual void LoadEditorForm(System.Windows.Forms.TabControl ScriptsTab, System.Windows.Forms.MenuStrip MainMenu) { }
        public virtual void SaveColorsForm() { }
        public virtual void SaveSettingsForm() { }
        public virtual void SaveEditorForm() { }

        public virtual ToolStripItem[] AddChannelPopups() { return null; }
        public virtual ToolStripItem[] AddQueryPopups() { return null; }
        public virtual ToolStripItem[] AddServerPopups() { return null; }
        public virtual Panel[] AddMainPanel() { return null; }

        public IceChat.IRCConnection ServerTreeCurrentConnection { get; set; }
        //public IceChat.IceChatServers Connections { get; set; }
        public string ServerTreeCurrentTab { get; set; }

        public virtual void MainProgramLoaded() { }                                   //the main icechat form/program has loaded
        public virtual void MainProgramLoaded(SortedList ServerConnections) { }       //the main icechat form/program has loaded

        public virtual PluginArgs ChannelMessage(PluginArgs args) { return args; }
        public virtual PluginArgs ChannelAction(PluginArgs args) { return args; }
        public virtual PluginArgs ChannelKick(PluginArgs args) { return args; }
        public virtual PluginArgs QueryMessage(PluginArgs args) { return args; }
        public virtual PluginArgs QueryAction(PluginArgs args) { return args; }

        public virtual PluginArgs ChannelJoin(PluginArgs args) { return args; }
        public virtual PluginArgs ChannelPart(PluginArgs args) { return args; }
        public virtual PluginArgs ServerQuit(PluginArgs args) { return args; }
        public virtual PluginArgs ServerMessage(PluginArgs args) { return args; }
        public virtual PluginArgs ServerNotice(PluginArgs args) { return args; }
        public virtual PluginArgs InputText(PluginArgs args) { return args; }
        public virtual PluginArgs UserNotice(PluginArgs args) { return args; }
        public virtual PluginArgs CtcpMessage(PluginArgs args) { return args; }
        public virtual PluginArgs CtcpReply(PluginArgs args) { return args; }
        public virtual PluginArgs ChannelNotice(PluginArgs args) { return args; }

        public virtual PluginArgs DCCChatOpen(PluginArgs args) { return args; }
        public virtual PluginArgs DCCChatClosed(PluginArgs args) { return args; }
        public virtual PluginArgs DCCChatMessage(PluginArgs args) { return args; }
        public virtual PluginArgs DCCChatTimeOut(PluginArgs args) { return args; }
        public virtual PluginArgs DCCChatConnected(PluginArgs args) { return args; }

        public virtual PluginArgs NickListDraw(PluginArgs args) { return args; }
        public virtual PluginArgs ServerListDraw(PluginArgs args) { return args; }

        public virtual PluginArgs ChannelTopic(PluginArgs args) { return args; }

        public virtual PluginArgs AutoJoinChannel(PluginArgs args) { return args; }
        public virtual PluginArgs AutoPerformCommand(PluginArgs args) { return args; }
        public virtual PluginArgs RejoinChannel(PluginArgs args) { return args; }
        public virtual PluginArgs DNSResolve(PluginArgs args) { return args; }
        public virtual PluginArgs LinkClicked(PluginArgs args) { return args; }

        public virtual PluginArgs ServerRawOverride(PluginArgs args) { return args; }

        public virtual PluginArgs ChannelInvite(PluginArgs args) { return args; }

        public virtual ToolStripMenuItem MenuItemShow(ToolStripMenuItem menu) { return menu; }
        
        public virtual void DCCFileStart(PluginArgs args) { }
        public virtual void DCCFileResume(PluginArgs args) { }
        public virtual void DCCFileComplete(PluginArgs args) { }
        public virtual void DCCFileError(PluginArgs args) { }

        public virtual void ServerConnect(PluginArgs args) { }
        public virtual void ServerDisconnect(PluginArgs args) { }
        public virtual void ServerPreConnect(PluginArgs args) {  }

        public virtual void WhoisUser(PluginArgs args) { }
        public virtual void BuddyList(PluginArgs args) { }
        public virtual void ChannelMode(PluginArgs args) { }
        public virtual void WhoReply(PluginArgs args) { }

        public virtual void NickChange(PluginArgs args) { }
        public virtual void ServerError(PluginArgs args) { }
        public virtual void ServerRaw(PluginArgs args) { }

        public virtual void NewWindow(PluginArgs args) { }

        public virtual void ChannelNames(PluginArgs args) { }
        public virtual void EndChannelNames(PluginArgs args) { }

        // when a hotkey is pressed
        public virtual PluginArgs HotKey(PluginArgs args, KeyEventArgs e) { return args; }
        
        // when we have switched to a new tab
        public virtual void SwitchTab(PluginArgs args) { }


        public void SendCommand(PluginArgs args)
        {
            OnCommand(args);
        }

        public virtual event OutGoingCommandHandler OnCommand;

    }

    public class PluginArgs : EventArgs
    {
        public string Message;  
        public string Nick;     
        public string Host;
        public string Channel;
        public string Extra;
        public Form Form;           
        public Object TextWindow;
        public IceChat.IRCConnection Connection;
        public uint fileSize;
        public uint filePos;
        public string fileName;
        public string dccPort;
        public string dccIP;
        public bool DisableEvent;
        public bool DisableNicklist;

        public string Command;      //if you wish to return back a command

        public PluginArgs()
        {
            //nada
        }

        public PluginArgs(string command)
        {
            this.Command = command;
        }

        public PluginArgs(Object textwindow, string channel, string nick, string host, string message)
        {
            this.Channel = channel;
            this.Nick = nick;
            this.Host = host;
            this.Message = message;
            this.TextWindow = textwindow;
        }

        public PluginArgs(Form form)
        {
            this.Form = form;    
        }

        public PluginArgs(IceChat.IRCConnection connection)
        {
            this.Connection = connection;
        }
    }
}
