/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2017 Paul Vanderzee <snerf@icechat.net>
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
using System.Collections;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Reflection;

using IceChat.Properties;
using IceChatPlugin;

namespace IceChat
{
    public partial class FormMain
    {

        // could make this a setting
        int maxMessageLength = 300;
        
        private string[] SplitLongMessage(string msg)
        {
            string[] words = msg.Split(' ');
            List<string> parts = new List<string>();
            string part = string.Empty;
            	
            // small change if first word is longer than the partLength: (!string.IsNullOrEmpty(part)) parts.Add(partCounter, part)
            foreach (string word in words)
            {
                if (part.Length + word.Length < maxMessageLength)
                {
                    part += string.IsNullOrEmpty(part) ? word : " " + word;
                }
                else
                {
                    parts.Add(part);
                    part = word;
                }
            }

            parts.Add(part);
            
            return parts.ToArray();
        }
        

        /// <summary>
        /// Parse out command written in Input Box or sent from Plugin
        /// </summary>
        /// <param name="connection">Which Connection it is for</param>
        /// <param name="data">The Message to Parse</param>
        public void ParseOutGoingCommand(IRCConnection connection, string commandData)
        {
            int error = 0;

            try
            {
                string data = commandData.Replace("&#x3;", "\x0003");
                data = data.Replace("&#x2;", "\x0002");
                data = data.Replace("&#x1F;", "\x001F");
                data = data.Replace("&#x1D;", "\x001D");
                data = data.Replace("&#x16;", "\x0016");
                data = data.Replace("&#x0F;", "\x000F");

                data = data.Replace(@"\%C", "\x0003");
                data = data.Replace(@"\%B", "\x0002");
                data = data.Replace(@"\%U", "\x001F");
                data = data.Replace(@"\%I", "\x001D");
                data = data.Replace(@"\%R", "\x0016");
                data = data.Replace(@"\%O", "\x000F");

                PluginArgs args = new PluginArgs(connection);
                args.Command = data;
                //pass the channel or query/chat if either is active window               
                if (CurrentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    args.Channel = CurrentWindow.TabCaption;
                    args.Extra = IceTabPage.WindowType.Channel.ToString();
                }
                else if (CurrentWindow.WindowStyle == IceTabPage.WindowType.Query)
                {
                    args.Nick = CurrentWindow.TabCaption;
                    args.Extra = IceTabPage.WindowType.Query.ToString();
                }
                else if (CurrentWindow.WindowStyle == IceTabPage.WindowType.DCCChat)
                {
                    args.Nick = CurrentWindow.TabCaption;
                    args.Extra = IceTabPage.WindowType.DCCChat.ToString();
                }
                else if (CurrentWindow.WindowStyle == IceTabPage.WindowType.Console)
                {
                    args.Nick = "Console";
                    args.Extra = "Console";
                }

                foreach (Plugin p in loadedPlugins)
                {
                    IceChatPlugin ipc = p as IceChatPlugin;
                    if (ipc != null)
                    {
                        if (((IceChatPlugin)ipc).plugin.Enabled == true)
                            args = ((IceChatPlugin)ipc).plugin.InputText(args);
                    }
                }

                data = args.Command;

                // System.Diagnostics.Debug.WriteLine("ParseCommand=" + data);

                if (data.StartsWith("//"))
                {
                    //parse out identifiers
                    ParseOutGoingCommand(connection, ParseIdentifiers(connection, data, data));
                    return;
                }

                if (data.Length == 0)
                    return;


                if (data.StartsWith("/"))
                {
                    int indexOfSpace = data.IndexOf(" ");
                    string command = "";
                    string temp = "";

                    if (indexOfSpace > 0)
                    {
                        command = data.Substring(0, indexOfSpace);
                        data = data.Substring(command.Length + 1);
                    }
                    else
                    {
                        command = data;
                        data = "";
                    }

                    //check for aliases
                    foreach (AliasItem a in iceChatAliases.listAliases)
                    {
                        if (a.AliasName == command)
                        {
                            if (a.Command.Length == 1)
                            {
                                data = ParseIdentifierValue(a.Command[0], data);
                                ParseOutGoingCommand(connection, ParseIdentifiers(connection, data, data));
                            }
                            else
                            {
                                //it is a multilined alias, run multiple commands
                                string oldData = data;
                                foreach (string c in a.Command)
                                {
                                    //System.Diagnostics.Debug.WriteLine("a1:" + c + ":" + data + "::" + oldData);
                                    //data = ParseIdentifierValue(c, data);
                                    string data2 = ParseIdentifierValue(c, oldData);
                                    //System.Diagnostics.Debug.WriteLine("a2:" + data2);                                    
                                    ParseOutGoingCommand(connection, ParseIdentifiers(connection, data2, oldData));
                                }
                            }
                            return;
                        }
                    }

                    switch (command.ToLower())
                    {
                        case "/makeexception":
                            throw new Exception("IceChat 9 Test Exception Error");

                        case "/testchars":
                            for (int x = 1; x < 255; x++)
                            {
                                ParseOutGoingCommand(connection, x.ToString() + " = " + ((char)x).ToString());
                            }
                            ParseOutGoingCommand(connection, "done test");
                            break;
                        case "/setupdde":
                            SetupIRCDDE();
                            break;
                        case "/loadpalette":
                            LoadColorPalette();
                            break;
                        case "/updater":
                            RunUpdater();
                            break;
                        case "/updateversion":
                            UpdateInstallVersion();
                            break;
                        case "/totray":
                            minimizeToTray();
                            break;
                        case "/searchchannel":
                        case "/searchchannels":
                            if (data.Length > 0)
                            {
                                //searchChannels(data);
                            }
                            break;
                        case "/searchnetwork":
                        case "/searchnetworks":
                            if (data.Length > 0)
                            {
                                //searchNetworks(data);
                            }
                            break;
                        case "/connectionstatus":
                            if (connection != null)
                            {
                                System.Diagnostics.Debug.WriteLine(connection.IsConnected);
                            }
                            break;
                        case "/debug":
                            if (data.Length == 0)
                                debugWindowToolStripMenuItem.PerformClick();
                            else
                            {
                                string[] dt = data.Split(' ');
                                if (dt[0].Equals("disable", StringComparison.OrdinalIgnoreCase))
                                {
                                    //disable this server from debug window/popup
                                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                    {
                                        if (c.ServerSetting.ID.ToString() == dt[1])
                                        {
                                            c.ShowDebug = false;
                                        }
                                    }

                                }
                                else if (dt[0].Equals("enable", StringComparison.OrdinalIgnoreCase))
                                {
                                    //disable this server from debug window/popup
                                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                    {
                                        if (c.ServerSetting.ID.ToString() == dt[1])
                                        {
                                            c.ShowDebug = true;
                                        }
                                    }
                                }

                            }
                            break;
                        case "/addlines":
                            if (data.Length == 0)
                            {
                                for (int i = 0; i < 250; i++)
                                {
                                    //pick a random color
                                    int randColor = new Random().Next(0, 71);

                                    string msg = "\x000304 " + i.ToString() + ":The quick brown \x0003" + randColor.ToString("00") + ",4fox jumps over the\x0003 www.icechat.net lazy dog and gets away with it at https://github.com/icechat/IceChat";
                                    CurrentWindowMessage(connection, msg, "", true);
                                }
                            }
                            else
                            {
                                int c = Convert.ToInt32(data);
                                for (int i = 0; i < c; i++)
                                {
                                    //pick a random color
                                    int randColor = new Random().Next(0, 71);

                                    string msg = "\x000304 " + i.ToString() + ":The quick brown \x0003" + randColor.ToString("00") + ",4fox jumps over the\x0003 www.icechat.net lazy dog and gets away with it at https://github.com/icechat/IceChat";
                                    CurrentWindowMessage(connection, msg, "", true);
                                }
                            }
                            break;

                        case "/ipsum":
                            string[] words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};
                            Random rn = new Random();
                            string rs = string.Empty;
                            int maxWords = 75;
                            for (int w = 0; w < maxWords; w++)
                            {
                                if (w > 0) { rs += " "; }
                                rs += words[rn.Next(words.Length)];
                            }

                            CurrentWindowMessage(connection, rs, "", true);
                            break;

                        case "/colormode":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                    CurrentWindow.TextWindow.NoColorMode = !CurrentWindow.TextWindow.NoColorMode;
                            }
                            else
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    System.Diagnostics.Debug.WriteLine(data);
                                    t.TextWindow.NoColorMode = !t.TextWindow.NoColorMode;
                                }
                            }
                            break;

                        case "/colornick":
                            if (data.Length > 0 && CurrentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                            {
                                // /colornick <nick> [color]
                                string[] split = data.Split(' ');

                                User u = CurrentWindow.GetNick(split[0]);
                                if (u != null)
                                {
                                    if (split.Length == 1)
                                    {
                                        // reset the nick color
                                        u.CustomColor = false;
                                    }
                                    else
                                    {                                        
                                        // make sure 2nd value is an integer
                                        int intColor = 0;
                                        if (Int32.TryParse(split[1], out intColor))
                                        {
                                            if (intColor < 72 && intColor > -1)
                                            {
                                                u.CustomColor = true;
                                                u.nickColor = intColor;
                                            }
                                        }
                                    }

                                    nickList.Invalidate();
                                }
                            }
                            break;

                        case "/sounds":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                    CurrentWindow.DisableSounds = !CurrentWindow.DisableSounds;
                            }
                            else
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                    t.DisableSounds = !t.DisableSounds;
                            }
                            break;

                        case "/flashing":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                    CurrentWindow.EventOverLoad = !CurrentWindow.EventOverLoad;
                            }
                            else
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                    t.EventOverLoad = !t.EventOverLoad;
                            }
                            break;

                        case "/logging":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    CurrentWindow.LoggingDisable = !CurrentWindow.LoggingDisable;
                                    CurrentWindow.TextWindow.DisableLogFile();
                                }
                            }
                            else
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    t.LoggingDisable = !t.LoggingDisable;
                                    t.TextWindow.DisableLogFile();
                                }
                            }
                            break;

                        case "/dump":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    CurrentWindow.TextWindow.SaveDumpFile(true);
                                }
                            }
                            break;
                        case "/loaddump":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    CurrentWindow.TextWindow.LoadDumpFile(CurrentWindow);
                                    CurrentWindow.TextWindow.Invalidate();
                                }
                            }
                            break;

                        case "/background":
                        case "/bg": //change background image for a window(s)
                            if (data.Length > 0)
                            {
                                //bg windowtype imagefile
                                //bg windowtype windowname imagefile
                                //if imagefile is 'none', erase background image
                                string window = data.Split(' ')[0];
                                string file = "";
                                if (data.IndexOf(' ') > -1)
                                    file = data.Substring(window.Length + 1);

                                switch (window.ToLower())
                                {
                                    case "nicklist":
                                        if (file.Length == 0)
                                        {
                                            //ask for a file/picture
                                            file = GetBackgroundImage();
                                            if (file.Length > 0)
                                            {
                                                this.nickList.BackGroundImage = file;
                                                //save the image file
                                                if (System.IO.Path.GetDirectoryName(file).ToLower().CompareTo(picturesFolder.ToLower()) == 0)
                                                    iceChatOptions.NickListImage = System.IO.Path.GetFileName(file);
                                                else
                                                    iceChatOptions.NickListImage = file;

                                            }
                                            return;
                                        }
                                        else if (file.ToLower() == "none" || file.ToLower() == "remove")
                                        {
                                            this.nickList.BackGroundImage = "";
                                            iceChatOptions.NickListImage = "";
                                        }
                                        else
                                        {
                                            //check if it is a full path or just a pic in the pictures folder
                                            if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                            {
                                                this.nickList.BackGroundImage = picturesFolder + System.IO.Path.DirectorySeparatorChar + file;
                                                iceChatOptions.NickListImage = file;
                                            }
                                            else if (File.Exists(file))
                                            {
                                                this.nickList.BackGroundImage = file;
                                                iceChatOptions.NickListImage = file;
                                            }
                                        }

                                        break;
                                    case "serverlist":
                                    case "servertree":
                                        if (file.Length == 0)
                                        {
                                            //ask for a file/picture
                                            file = GetBackgroundImage();
                                            if (file.Length > 0)
                                            {
                                                this.serverTree.BackGroundImage = file;
                                                //save the image file
                                                if (System.IO.Path.GetDirectoryName(file).ToLower().CompareTo(picturesFolder.ToLower()) == 0)
                                                    iceChatOptions.ServerTreeImage = System.IO.Path.GetFileName(file);
                                                else
                                                    iceChatOptions.ServerTreeImage = file;

                                            }
                                            return;
                                        }
                                        else if (file.ToLower() == "none" || file.ToLower() == "remove")
                                        {
                                            this.serverTree.BackGroundImage = "";
                                            iceChatOptions.ServerTreeImage = "";
                                        }
                                        else
                                        {
                                            //check if it is a full path or just a pic in the pictures folder
                                            if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                            {
                                                this.serverTree.BackGroundImage = picturesFolder + System.IO.Path.DirectorySeparatorChar + file;
                                                iceChatOptions.ServerTreeImage = file;
                                            }
                                            else if (File.Exists(file))
                                            {
                                                this.serverTree.BackGroundImage = file;
                                                iceChatOptions.ServerTreeImage = file;
                                            }
                                        }
                                        break;
                                    case "console":
                                        //check if the file is a URL
                                        if (file.Length > 0)
                                        {
                                            if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                                mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().BackGroundImage = picturesFolder + System.IO.Path.DirectorySeparatorChar + file;
                                            else
                                            {
                                                //check if this is a full path to the file
                                                if (File.Exists(file))
                                                    mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().BackGroundImage = file;
                                                else
                                                    mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().BackGroundImage = "";
                                            }
                                        }
                                        break;
                                    case "channel":
                                        //get the channel name
                                        if (file.IndexOf(' ') > -1)
                                        {
                                            string channel = file.Split(' ')[0];
                                            //if channel == "all" do it for all

                                            file = file.Substring(channel.Length + 1);
                                            if (channel.ToLower() == "all")
                                            {
                                                foreach (IceTabPage t in mainChannelBar.TabPages)
                                                {
                                                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                                    {
                                                        if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                                            t.TextWindow.BackGroundImage = (picturesFolder + System.IO.Path.DirectorySeparatorChar + file);
                                                        else
                                                            t.TextWindow.BackGroundImage = "";

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                IceTabPage t = GetWindow(connection, channel, IceTabPage.WindowType.Channel);
                                                if (t != null)
                                                {
                                                    if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                                        t.TextWindow.BackGroundImage = (picturesFolder + System.IO.Path.DirectorySeparatorChar + file);
                                                    else
                                                        t.TextWindow.BackGroundImage = "";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //only a channel name specified, no file, erase the image
                                            //if file == "all" clear em all
                                            if (file.ToLower() == "all")
                                            {
                                                foreach (IceTabPage t in mainChannelBar.TabPages)
                                                {
                                                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                                        t.TextWindow.BackGroundImage = "";
                                                }
                                            }
                                            else
                                            {
                                                IceTabPage t = GetWindow(connection, file, IceTabPage.WindowType.Channel);
                                                if (t != null)
                                                    t.TextWindow.BackGroundImage = "";
                                            }
                                        }
                                        break;
                                    case "query":

                                        break;
                                    case "window":
                                        if (file.IndexOf(' ') > -1)
                                        {
                                            string windowName = file.Split(' ')[0];

                                            file = file.Substring(windowName.Length + 1);
                                            IceTabPage t = GetWindow(connection, windowName, IceTabPage.WindowType.Window);
                                            if (t != null)
                                            {
                                                if (File.Exists(picturesFolder + System.IO.Path.DirectorySeparatorChar + file))
                                                    t.TextWindow.BackGroundImage = (picturesFolder + System.IO.Path.DirectorySeparatorChar + file);
                                                else
                                                    t.TextWindow.BackGroundImage = "";
                                            }

                                        }
                                        else
                                        {
                                            IceTabPage t = GetWindow(connection, file, IceTabPage.WindowType.Window);
                                            if (t != null)
                                                t.TextWindow.BackGroundImage = "";

                                        }
                                        break;
                                }
                            }
                            break;
                        case "/bgcolor":
                            //change the background color for the current or selected window
                            if (data.Length > 0)
                            {
                                int result;
                                if (Int32.TryParse(data, out result))
                                    if (result >= 0 && result < 72)
                                        if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                                            mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().IRCBackColor = result;
                                        else if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.Query || CurrentWindowStyle == IceTabPage.WindowType.Window)
                                            CurrentWindow.TextWindow.IRCBackColor = result;
                            }
                            break;
                        case "/unloadplugin":
                            if (data.Length > 0)
                            {
                                //get the plugin name, and look for it in the menu items
                                ToolStripMenuItem menuItem = null;
                                foreach (ToolStripMenuItem t in pluginsToolStripMenuItem.DropDownItems)
                                    if (t.ToolTipText.ToLower() == data.ToLower())
                                        menuItem = t;

                                if (menuItem != null)
                                {
                                    IPluginIceChat plugin = (IPluginIceChat)menuItem.Tag;

                                    plugin.Enabled = false;
                                    plugin.Unloaded = true;

                                    //remove any panels added to the main form
                                    Panel[] addedPanels = plugin.AddMainPanel();
                                    if (addedPanels != null && addedPanels.Length > 0)
                                    {
                                        bool foundOther = false;
                                        foreach (Panel p in addedPanels)
                                        {
                                            //fix the bottom panel / splitter
                                            if (p != null)
                                            {
                                                if (p.Dock == DockStyle.Bottom)
                                                {
                                                    //are there any other 
                                                    foreach (Control cp in this.Controls)
                                                    {
                                                        if (cp != p)
                                                        {
                                                            if (cp.GetType() == typeof(Panel))
                                                                if (cp.Dock == DockStyle.Bottom)
                                                                    foundOther = true;
                                                        }
                                                    }
                                                }
                                            }
                                            this.Controls.Remove(p);
                                        }

                                        if (!foundOther)
                                        {
                                            this.Invoke((MethodInvoker)delegate()
                                            {
                                                this.splitterBottom.Visible = false;
                                            });
                                        }
                                    }

                                    plugin.OnCommand -= new OutGoingCommandHandler(Plugin_OnCommand);
                                    menuItem.Click -= new EventHandler(OnPluginMenuItemClick);
                                    pluginsToolStripMenuItem.DropDownItems.Remove(menuItem);

                                    for (int i = 0; i < iceChatPlugins.listPlugins.Count; i++)
                                    {
                                        if (((PluginItem)iceChatPlugins.listPlugins[i]).PluginFile.Equals(menuItem.ToolTipText))
                                        {
                                            ((PluginItem)iceChatPlugins.listPlugins[i]).Enabled = false;
                                            ((PluginItem)iceChatPlugins.listPlugins[i]).Unloaded = true;
                                            SavePluginFiles();
                                        }
                                    }

                                    WindowMessage(null, "Console", "\x000304Unloaded Plugin - " + plugin.Name, "", true);
                                }
                            }
                            break;
                        case "/statusplugin":
                            if (data.Length > 0 && data.IndexOf(' ') > 0)
                            {
                                string[] values = data.Split(new char[] { ' ' }, 2);

                                ToolStripMenuItem menuItem = null;
                                foreach (ToolStripMenuItem t in pluginsToolStripMenuItem.DropDownItems)
                                    if (t.ToolTipText.ToLower() == values[1].ToLower())
                                        menuItem = t;

                                if (menuItem != null)
                                {
                                    //match
                                    IPluginIceChat plugin = (IPluginIceChat)menuItem.Tag;
                                    plugin.Enabled = Convert.ToBoolean(values[0]);

                                    if (plugin.Enabled == true)
                                    {
                                        WindowMessage(null, "Console", "\x000304Enabled Plugin - " + plugin.Name + " v" + plugin.Version, "", true);

                                        //init the plugin
                                        System.Threading.Thread initPlugin = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InitializePlugin));
                                        initPlugin.Start(plugin);

                                        //remove the icon
                                        menuItem.Image = null;
                                    }
                                    else
                                    {
                                        WindowMessage(null, "Console", "\x000304Disabled Plugin - " + plugin.Name + " v" + plugin.Version, "", true);
                                        menuItem.Image = StaticMethods.LoadResourceImage("CloseButton.png");

                                    }
                                }
                            }
                            break;
                        case "/loadplugin":
                            if (data.Length > 0)
                            {
                                IPluginIceChat ipc = loadPlugin(pluginsFolder + System.IO.Path.DirectorySeparatorChar + data);
                                if (ipc != null)
                                {
                                    System.Threading.Thread initPlugin = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(InitializePlugin));
                                    initPlugin.Start(ipc);
                                }
                            }
                            break;
                        case "/reload":
                            if (data.Length > 0)
                            {
                                switch (data)
                                {
                                    case "alias":
                                    case "aliases":
                                        CurrentWindowMessage(connection, "\x000304 Aliases file reloaded", "", true);
                                        LoadAliases();
                                        break;
                                    case "popup":
                                    case "popups":
                                        CurrentWindowMessage(connection, "\x000304 Popups file reloaded", "", true);
                                        LoadPopups();
                                        break;
                                    case "emoticon":
                                    case "emoticons":
                                        CurrentWindowMessage(connection, "\x000304 Emoticons file reloaded", "", true);
                                        LoadEmoticons();
                                        break;
                                    case "sound":
                                    case "sounds":
                                        CurrentWindowMessage(connection, "\x000304 Sounds file reloaded", "", true);
                                        LoadSounds();
                                        break;
                                    case "color":
                                    case "colors":
                                        CurrentWindowMessage(connection, "\x00034 Colors file reloaded", "", true);
                                        LoadColors();
                                        toolStripMain.BackColor = IrcColor.colors[iceChatColors.ToolbarBackColor];
                                        menuMainStrip.BackColor = IrcColor.colors[iceChatColors.MenubarBackColor];
                                        statusStripMain.BackColor = IrcColor.colors[iceChatColors.StatusbarBackColor];
                                        toolStripStatus.ForeColor = IrcColor.colors[iceChatColors.StatusbarForeColor];
                                        inputPanel.SetInputBoxColors();
                                        channelList.SetListColors();
                                        buddyList.SetListColors();
                                        serverTree.SetListColors();
                                        nickList.SetListColors();
                                        break;
                                    case "font":
                                    case "fonts":
                                        CurrentWindowMessage(connection, "\x00034 Fonts file reloaded", "", true);
                                        LoadFonts();
                                        nickList.Font = new Font(iceChatFonts.FontSettings[3].FontName, iceChatFonts.FontSettings[3].FontSize);
                                        serverTree.Font = new Font(iceChatFonts.FontSettings[4].FontName, iceChatFonts.FontSettings[4].FontSize);
                                        menuMainStrip.Font = new Font(iceChatFonts.FontSettings[7].FontName, iceChatFonts.FontSettings[7].FontSize);
                                        break;
                                }
                            }
                            break;
                        case "/addtext":
                            if (data.Length > 0)
                                AddInputPanelText(data);
                            break;
                        case "/beep":
                            System.Media.SystemSounds.Beep.Play();
                            break;
                        case "/size":
                            CurrentWindowMessage(connection, "\x00034Window Size is: " + this.Width + ":" + this.Height, "", true);
                            break;
                        case "/ame":    //me command for all channels
                            if (connection != null && data.Length > 0)
                            {
                                foreach (IceTabPage t in mainChannelBar.TabPages)
                                {
                                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                    {
                                        if (t.Connection == connection)
                                        {
                                            //SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + data + "");

                                            if (data.Length > maxMessageLength)
                                            {
                                                var lines = SplitLongMessage(data);
                                                foreach (var line in lines)
                                                {
                                                    SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + line + "");
                                                }
                                            }
                                            else
                                            {
                                                SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + data + "");
                                            } 

                                            string msg = GetMessageFormat("Self Channel Action");
                                            msg = msg.Replace("$nick", t.Connection.ServerSetting.CurrentNickName).Replace("$channel", t.TabCaption);
                                            msg = msg.Replace("$message", data);

                                            t.TextWindow.AppendText(msg, "");
                                            t.TextWindow.ScrollToBottom();
                                            t.LastMessageType = ServerMessageType.Action;
                                        }
                                    }
                                }
                            }
                            break;

                        case "/aquery": // send a message to all queries
                            if (connection != null && data.Length > 0)
                            {
                                foreach (IceTabPage t in mainChannelBar.TabPages)
                                {
                                    if (t.WindowStyle == IceTabPage.WindowType.Query)
                                    {
                                        if (t.Connection == connection)
                                        {
                                            // message this query/nick
                                            ParseOutGoingCommand(connection, "/msg " + t.TabCaption + " " + data);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/amsg":   //send a message to all channels 
                            if (connection != null && data.Length > 0)
                            {
                                foreach (IceTabPage t in mainChannelBar.TabPages)
                                {
                                    if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                    {
                                        if (t.Connection == connection)
                                        {
                                            //SendData(connection, "PRIVMSG " + t.TabCaption + " :" + data);

                                            if (data.Length > maxMessageLength)
                                            {
                                                var lines = SplitLongMessage(data);
                                                foreach (var line in lines)
                                                {
                                                    SendData(connection, "PRIVMSG " + t.TabCaption + " :" + line);
                                                }
                                            }
                                            else
                                            {
                                                SendData(connection, "PRIVMSG " + t.TabCaption + " :" + data);
                                            } 

                                            string msg = GetMessageFormat("Self Channel Message");
                                            msg = msg.Replace("$nick", t.Connection.ServerSetting.CurrentNickName).Replace("$channel", t.TabCaption);

                                            //assign $color to the nickname 
                                            if (msg.Contains("$color"))
                                            {
                                                User u = CurrentWindow.GetNick(t.Connection.ServerSetting.CurrentNickName);
                                                //get the nick color
                                                if (u.nickColor == -1)
                                                {
                                                    if (IceChatColors.RandomizeNickColors == true)
                                                    {
                                                        int randColor = new Random().Next(0, 71);
                                                        if (randColor == IceChatColors.NickListBackColor)
                                                            randColor = new Random().Next(0, 71);
                                                        u.nickColor = randColor;
                                                    }
                                                    else
                                                    {
                                                        //get the correct nickname color for channel status
                                                        for (int y = 0; y < u.Level.Length; y++)
                                                        {
                                                            if (u.Level[y])
                                                            {
                                                                switch (connection.ServerSetting.StatusModes[0][y])
                                                                {
                                                                    case 'q':
                                                                    case 'y':
                                                                        u.nickColor = IceChatColors.ChannelOwnerColor;
                                                                        break;
                                                                    case 'a':
                                                                        u.nickColor = IceChatColors.ChannelAdminColor;
                                                                        break;
                                                                    case 'o':
                                                                        u.nickColor = IceChatColors.ChannelOpColor;
                                                                        break;
                                                                    case 'h':
                                                                        u.nickColor = IceChatColors.ChannelHalfOpColor;
                                                                        break;
                                                                    case 'v':
                                                                        u.nickColor = IceChatColors.ChannelVoiceColor;
                                                                        break;
                                                                    default:
                                                                        u.nickColor = IceChatColors.ChannelRegularColor;
                                                                        break;
                                                                }
                                                                break;
                                                            }
                                                        }

                                                    }
                                                    if (u.nickColor == -1)
                                                        u.nickColor = IceChatColors.ChannelRegularColor;

                                                }

                                                msg = msg.Replace("$color", "\x0003" + u.nickColor.ToString("00"));
                                            }

                                            msg = msg.Replace("$status", CurrentWindow.GetNick(t.Connection.ServerSetting.CurrentNickName).ToString().Replace(t.Connection.ServerSetting.CurrentNickName, ""));
                                            msg = msg.Replace("$message", data);

                                            t.TextWindow.AppendText(msg, "");
                                            t.TextWindow.ScrollToBottom();
                                            t.LastMessageType = ServerMessageType.Message;

                                        }
                                    }
                                }
                            }
                            break;
                        case "/anick":
                            if (data.Length > 0)
                            {
                                foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                    if (c.IsConnected)
                                    {
                                        SendData(c, "NICK " + data);
                                        c.ServerSetting.SendNickServPassword = false;
                                    }
                            }
                            break;
                        case "/autojoin":
                            if (connection != null)
                            {
                                if (data.Length == 0)
                                {
                                    if (connection.ServerSetting.AutoJoinChannels != null)
                                    {
                                        foreach (string chan in connection.ServerSetting.AutoJoinChannels)
                                        {
                                            if (chan != null)
                                            {
                                                if (!chan.StartsWith(";"))
                                                    SendData(connection, "JOIN " + chan);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (connection.ServerSetting.AutoJoinChannels == null)
                                    {
                                        //we have no autojoin channels, so just add it
                                        connection.ServerSetting.AutoJoinChannels = new string[1];
                                        connection.ServerSetting.AutoJoinChannels[0] = data;
                                        CurrentWindowMessage(connection, "\x000307" + data + " is added to the Autojoin List", "", true);
                                        connection.ServerSetting.AutoJoinEnable = true;

                                        serverTree.SaveServers(serverTree.ServersCollection);
                                    }
                                    else
                                    {
                                        //check if it is in the list first
                                        bool Exists = false;
                                        bool Disabled = false;

                                        string[] oldAutoJoin = new string[connection.ServerSetting.AutoJoinChannels.Length];
                                        int i = 0;
                                        foreach (string chan in connection.ServerSetting.AutoJoinChannels)
                                        {
                                            if (chan != null)
                                            {
                                                if (chan.ToLower() == data.ToLower())
                                                {
                                                    //already in the list
                                                    Exists = true;
                                                    Disabled = true;
                                                    oldAutoJoin[i] = ";" + chan;
                                                    CurrentWindowMessage(connection, "\x000307" + data + " is now disabled in the Autojoin List", "", true);
                                                }
                                                else if (chan.ToLower() == (";" + data.ToLower()))
                                                {
                                                    //already in the list, but disabled
                                                    //so lets enable it
                                                    Disabled = true;
                                                    oldAutoJoin[i] = chan.Substring(1);
                                                    Exists = true;
                                                    CurrentWindowMessage(connection, "\x000307" + data + " is enabled in the Autojoin List", "", true);
                                                }
                                                else
                                                    oldAutoJoin[i] = chan;
                                            }
                                            i++;
                                        }

                                        if (!Exists)
                                        {
                                            //add a new item
                                            connection.ServerSetting.AutoJoinChannels = new string[connection.ServerSetting.AutoJoinChannels.Length + 1];
                                            i = 0;
                                            foreach (string chan in oldAutoJoin)
                                            {
                                                connection.ServerSetting.AutoJoinChannels[i] = chan;
                                                i++;
                                            }
                                            connection.ServerSetting.AutoJoinChannels[i] = data;
                                            CurrentWindowMessage(connection, "\x000307" + data + " is added to the Autojoin List", "", true);
                                            connection.ServerSetting.AutoJoinEnable = true;

                                            serverTree.SaveServers(serverTree.ServersCollection);
                                        }
                                        else if (Disabled)
                                        {
                                            connection.ServerSetting.AutoJoinChannels = new string[connection.ServerSetting.AutoJoinChannels.Length];
                                            i = 0;
                                            foreach (string chan in oldAutoJoin)
                                            {
                                                connection.ServerSetting.AutoJoinChannels[i] = chan;
                                                i++;
                                            }
                                            serverTree.SaveServers(serverTree.ServersCollection);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/autoperform":
                            if (connection != null)
                            {
                                if (connection.ServerSetting.AutoPerform != null)
                                {
                                    foreach (string ap in connection.ServerSetting.AutoPerform)
                                    {
                                        string autoCommand = ap.Replace("\r", String.Empty);
                                        if (!autoCommand.StartsWith(";"))
                                            ParseOutGoingCommand(connection, autoCommand);
                                    }
                                }
                            }
                            break;
                        case "/autostart":
                            if (connection != null)
                            {
                                connection.ServerSetting.AutoStart = !connection.ServerSetting.AutoStart;
                                serverTree.SaveServers(serverTree.ServersCollection);
                            }
                            break;
                        case "/aaway":
                            foreach (IRCConnection c in serverTree.ServerConnections.Values)
                            {
                                if (c.IsConnected)
                                    ParseOutGoingCommand(c, "/away " + data);
                            }
                            break;
                        case "/away":
                            if (connection != null)
                            {
                                if (connection.ServerSetting.Away)
                                {
                                    connection.ServerSetting.Away = false;

                                    if (connection.ServerSetting.AwayNickName.Length > 0)
                                    {
                                        SendData(connection, "NICK " + connection.ServerSetting.DefaultNick);
                                        connection.ServerSetting.SendNickServPassword = false;
                                    }

                                    TimeSpan t = DateTime.Now.Subtract(connection.ServerSetting.AwayStart);

                                    string s = t.Seconds.ToString() + " secs";
                                    if (t.Minutes > 0)
                                        s = t.Minutes.ToString() + " mins " + s;
                                    if (t.Hours > 0)
                                        s = t.Hours.ToString() + " hrs " + s;
                                    if (t.Days > 0)
                                        s = t.Days.ToString() + " days " + s;

                                    string msg = iceChatOptions.ReturnCommand;
                                    msg = msg.Replace("$awaytime", s);
                                    // show the away reason
                                    msg = msg.Replace("$awayreason", connection.ServerSetting.AwayReason);

                                    if (iceChatOptions.SendAwayCommands == true && !connection.ServerSetting.DisableAwayMessages)
                                        ParseOutGoingCommand(connection, msg);

                                }
                                else
                                {
                                    connection.ServerSetting.Away = true;
                                    connection.ServerSetting.DefaultNick = connection.ServerSetting.CurrentNickName;
                                    connection.ServerSetting.AwayStart = System.DateTime.Now;

                                    if (connection.ServerSetting.AwayNickName.Length > 0)
                                    {
                                        SendData(connection, "NICK " + connection.ServerSetting.AwayNickName);
                                        connection.ServerSetting.SendNickServPassword = false;
                                    }

                                    string msg = iceChatOptions.AwayCommand;
                                    msg = msg.Replace("$awayreason", data);
                                    connection.ServerSetting.AwayReason = data;

                                    if (iceChatOptions.SendAwayCommands == true && !connection.ServerSetting.DisableAwayMessages)
                                        ParseOutGoingCommand(connection, msg);
                                }
                            }
                            break;
                        case "/ban":  // /ban #channel nick|address   /mode #channel +b host
                            if (connection != null && data.IndexOf(' ') > 0)
                            {
                                string channel = data.Split(' ')[0];
                                string host = data.Split(' ')[1];
                                ParseOutGoingCommand(connection, "/mode " + channel + " +b " + host);
                            }
                            break;
                        case "/browser":
                            if (data.Length > 0)
                            {
                                if (data.StartsWith("http"))
                                    System.Diagnostics.Process.Start(data);
                                else
                                    System.Diagnostics.Process.Start("http://" + data);
                            }
                            break;
                        case "/buddylist":
                        case "/notify":
                            //add a nickname to the buddy list
                            if (connection != null && data.Length > 0 && data.IndexOf(" ") == -1)
                            {
                                //check if the nickname is already in the buddy list                                
                                if (connection.ServerSetting.BuddyList != null)
                                {
                                    foreach (BuddyListItem buddy in connection.ServerSetting.BuddyList)
                                    {
                                        if (buddy != null)
                                        {
                                            if (!buddy.Nick.StartsWith(";"))
                                                if (buddy.Nick.ToLower() == data.ToLower())
                                                    return;
                                                else
                                                    if (buddy.Nick.Substring(1).ToLower() == data.ToLower())
                                                        return;
                                        }
                                    }
                                }

                                //add in the new buddy list item
                                BuddyListItem b = new BuddyListItem();
                                b.Nick = data;

                                BuddyListItem[] buddies = connection.ServerSetting.BuddyList;
                                Array.Resize(ref buddies, buddies.Length + 1);
                                buddies[buddies.Length - 1] = b;

                                connection.ServerSetting.BuddyList = buddies;

                                serverTree.SaveServers(serverTree.ServersCollection);

                                connection.BuddyListCheck();
                            }
                            break;
                        case "/chaninfo":
                            if (connection != null)
                            {
                                if (data.Length > 0)
                                {
                                    IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                    if (t != null)
                                    {
                                        FormChannelInfo fci = new FormChannelInfo(t);
                                        SendData(connection, "MODE " + t.TabCaption + " +b");

                                        //check if mode (e) exists for Exception List

                                        if (connection.ServerSetting.ChannelModeParam.Contains("e"))
                                            SendData(connection, "MODE " + t.TabCaption + " +e");
                                        else if (connection.ServerSetting.ChannelModeAddress.Contains("e"))
                                            SendData(connection, "MODE " + t.TabCaption + " +e");

                                        //check if mode (q) exists for Quiet List
                                        if (connection.ServerSetting.ChannelModeParam.Contains("q"))
                                            SendData(connection, "MODE " + t.TabCaption + " +q");
                                        else if (connection.ServerSetting.ChannelModeAddress.Contains("q"))
                                            SendData(connection, "MODE " + t.TabCaption + " +q");

                                        SendData(connection, "TOPIC :" + t.TabCaption);
                                        fci.Show(this);
                                    }
                                }
                                else
                                {
                                    //check if current window is channel
                                    if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                    {
                                        FormChannelInfo fci = new FormChannelInfo(CurrentWindow);
                                        SendData(connection, "MODE " + CurrentWindow.TabCaption + " +b");
                                        //check if mode (e) exists for Exception List
                                        if (connection.ServerSetting.ChannelModeParam.Contains("e"))
                                            SendData(connection, "MODE " + CurrentWindow.TabCaption + " +e");
                                        else if (connection.ServerSetting.ChannelModeAddress.Contains("e"))
                                            SendData(connection, "MODE " + CurrentWindow.TabCaption + " +e");

                                        //check if mode (q) exists for Quiet List
                                        if (connection.ServerSetting.ChannelModeParam.Contains("q"))
                                            SendData(connection, "MODE " + CurrentWindow.TabCaption + " +q");
                                        else if (connection.ServerSetting.ChannelModeAddress.Contains("q"))
                                            SendData(connection, "MODE " + CurrentWindow.TabCaption + " +q");

                                        SendData(connection, "TOPIC :" + CurrentWindow.TabCaption);
                                        fci.Show(this);
                                    }
                                }
                            }
                            break;
                        case "/pin":
                            if (data.Length > 0)
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    t.PinnedTab = true;
                                }
                                else
                                {
                                    IceTabPage c = GetWindow(null, data, IceTabPage.WindowType.Console);
                                    if (c != null)
                                        c.PinnedTab = true;
                                    else
                                    {
                                        //debug window?
                                        IceTabPage d = GetWindow(null, data, IceTabPage.WindowType.Debug);
                                        if (d != null)
                                            d.PinnedTab = true;
                                        else
                                        {
                                            IceTabPage w = GetWindow(null, data, IceTabPage.WindowType.Window);
                                            if (w != null)
                                                w.PinnedTab = true;
                                            else
                                            {
                                                IceTabPage q = GetWindow(connection, data, IceTabPage.WindowType.Query);
                                                if (q != null)
                                                    q.PinnedTab = true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "/unpin":
                            if (data.Length > 0)
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    t.PinnedTab = false;
                                }
                                else
                                {
                                    IceTabPage c = GetWindow(null, data, IceTabPage.WindowType.Console);
                                    if (c != null)
                                        c.PinnedTab = false;
                                    else
                                    {
                                        //debug window?
                                        IceTabPage d = GetWindow(null, data, IceTabPage.WindowType.Debug);
                                        if (d != null)
                                            d.PinnedTab = false;
                                        else
                                        {
                                            IceTabPage w = GetWindow(null, data, IceTabPage.WindowType.Window);
                                            if (w != null)
                                                w.PinnedTab = false;
                                            else
                                            {
                                                IceTabPage q = GetWindow(connection, data, IceTabPage.WindowType.Query);
                                                if (q != null)
                                                    q.PinnedTab = false;

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "/attach":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.Query || CurrentWindowStyle == IceTabPage.WindowType.Window)
                                {
                                    //are we in windowed mode or not?
                                    if (!mainTabControl.windowedMode)
                                    {
                                        //need to get the FormWindow
                                        FormWindow child = (FormWindow)CurrentWindow.Parent;
                                        child.MainMenu.Hide();
                                        child.DisableActivate();

                                        IceTabPage tab = child.DockedControl;
                                        tab.DockedForm = false;
                                        tab.Detached = false;

                                        mainTabControl.AddTabPage(tab);
                                        mainChannelBar.SelectTab(tab);

                                        //close the window
                                        child.Close();
                                    }
                                    else
                                    {
                                        //we are already in windowed mode.. back to the parent
                                        FormWindow child = (FormWindow)CurrentWindow.Parent;
                                        child.MdiParent = this;
                                        child.CreateDetachMenu();

                                        child.MainMenu.Hide();
                                        CurrentWindow.Detached = false;
                                    }
                                }
                            }
                            break;
                        case "/detach":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.Query || CurrentWindowStyle == IceTabPage.WindowType.Window)
                                {
                                    //are we in windowed mode or not?
                                    if (!mainTabControl.windowedMode)
                                    {
                                        IceTabPage tab = CurrentWindow;
                                        tab.Detached = true;
                                        tab.DockedForm = true;

                                        FormWindow fw = new FormWindow(tab);

                                        fw.Text = tab.TabCaption;
                                        if (tab.WindowStyle == IceTabPage.WindowType.Channel || tab.WindowStyle == IceTabPage.WindowType.Query)
                                            fw.Text += " {" + tab.Connection.ServerSetting.NetworkName + "}";

                                        Point location = tab.WindowLocation;

                                        fw.Show();

                                        if (location != null)
                                        {
                                            //set new window location
                                            fw.Location = location;
                                        }

                                        if (tab.WindowSize != null && tab.WindowSize.Height != 0)
                                        {
                                            fw.Size = tab.WindowSize;
                                        }
                                    }
                                    else
                                    {
                                        //we are already in windowed mode.. remove the parent
                                        FormWindow child = (FormWindow)CurrentWindow.Parent;
                                        child.MdiParent = null;
                                        child.CreateAttachMenu();

                                        CurrentWindow.Detached = true;

                                    }
                                }
                            }
                            else
                            {
                                //detach a specific window

                            }
                            break;
                        case "/loadorder":
                            mainChannelBar.SortPageTabs();
                            mainChannelBar.Invalidate();
                            break;

                        case "/saveorder":
                            int curWindow = 1;  //window #
                            for (int i = 0; i < mainChannelBar.TabPages.Count; i++)
                            {
                                if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    curWindow++;
                                    mainChannelBar.TabPages[i].WindowIndex = curWindow;
                                }
                            }
                            //save the channel settings
                            SaveChannelSettings();
                            break;

                        case "/switch":
                            //switch to a specific channel / query on a server
                            //  /switch #channel serverID
                            if (data.IndexOf(' ') > -1)
                            {
                                string channel = data.Split(' ')[0];
                                string server = data.Split(' ')[1];
                                int serverID;
                                if (Int32.TryParse(server, out serverID))
                                {
                                    //switch to this window
                                    for (int i = 0; i < mainChannelBar.TabPages.Count; i++)
                                    {
                                        if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel || mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Query)
                                        {
                                            if (mainChannelBar.TabPages[i].Connection.ServerSetting.ID == serverID)
                                            {
                                                if (mainChannelBar.TabPages[i].TabCaption.ToLower() == channel.ToLower())
                                                    mainChannelBar.SelectTab(mainChannelBar.TabPages[i]);
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "/clear":
                            if (data.Length == 0)
                            {
                                if (CurrentWindowStyle != IceTabPage.WindowType.Console)
                                {
                                    CurrentWindow.TextWindow.ClearTextWindow();
                                }
                                else
                                {
                                    //find the current console tab window
                                    mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().ClearTextWindow();
                                }
                            }
                            else
                            {
                                //find a match
                                if (data == "Console")
                                {
                                    mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().ClearTextWindow();
                                    return;
                                }
                                else if (data == "Debug")
                                {
                                    IceTabPage db = GetWindow(null, "Debug", IceTabPage.WindowType.Debug);
                                    if (db != null)
                                    {
                                        db.TextWindow.ClearTextWindow();
                                    }
                                }
                                else if (data.ToLower() == "all console")
                                {
                                    //clear all the console windows and channel/queries
                                    foreach (ConsoleTab c in mainChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
                                        ((TextWindow)c.Controls[0]).ClearTextWindow();
                                }
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                    t.TextWindow.ClearTextWindow();
                                else
                                {
                                    IceTabPage q = GetWindow(connection, data, IceTabPage.WindowType.Query);
                                    if (q != null)
                                    {
                                        q.TextWindow.ClearTextWindow();
                                        return;
                                    }
                                    IceTabPage dcc = GetWindow(connection, data, IceTabPage.WindowType.DCCChat);
                                    if (dcc != null)
                                    {
                                        dcc.TextWindow.ClearTextWindow();
                                        return;
                                    }
                                    IceTabPage win = GetWindow(null, data, IceTabPage.WindowType.Window);
                                    if (win != null)
                                    {
                                        win.TextWindow.ClearTextWindow();
                                        return;
                                    }
                                }
                            }
                            break;
                        case "/clearall":
                            //clear all the text windows
                            for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                            {
                                if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel || mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Query)
                                {
                                    mainChannelBar.TabPages[i].TextWindow.ClearTextWindow();
                                }
                                else if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Console)
                                {
                                    //clear all console windows
                                    foreach (ConsoleTab c in mainChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
                                    {
                                        ((TextWindow)c.Controls[0]).ClearTextWindow();
                                    }
                                }
                            }
                            break;
                        case "/closeall":
                            for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                            {
                                if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Window)
                                {
                                    RemoveWindow(connection, mainChannelBar.TabPages[i].TabCaption, IceTabPage.WindowType.Window);
                                }
                            }
                            break;
                        case "/close":
                            if (connection != null && data.Length > 0)
                            {
                                //check if it is a channel list window
                                if (data == "Channels")
                                {
                                    IceTabPage c = GetWindow(connection, "", IceTabPage.WindowType.ChannelList);
                                    if (c != null)
                                        RemoveWindow(connection, "", IceTabPage.WindowType.ChannelList);
                                    return;
                                }
                                //check if it is a query window
                                IceTabPage q = GetWindow(connection, data, IceTabPage.WindowType.Query);
                                if (q != null)
                                {
                                    RemoveWindow(connection, q.TabCaption, IceTabPage.WindowType.Query);
                                    return;
                                }

                                //check if it is a dcc chat window
                                IceTabPage dcc = GetWindow(connection, data, IceTabPage.WindowType.DCCChat);
                                if (dcc != null)
                                {
                                    RemoveWindow(connection, dcc.TabCaption, IceTabPage.WindowType.DCCChat);
                                    return;
                                }
                            }
                            else if (connection != null)
                            {
                                //check if current window is channel/query/dcc chat
                                if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                                    RemoveWindow(connection, CurrentWindow.TabCaption, CurrentWindow.WindowStyle);
                                else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                                    RemoveWindow(connection, CurrentWindow.TabCaption, CurrentWindow.WindowStyle);
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    SendData(connection, "PART " + CurrentWindow.TabCaption);
                                    RemoveWindow(connection, CurrentWindow.TabCaption, CurrentWindow.WindowStyle);
                                }
                            }
                            else
                            {
                                //check if the current window is the debug window
                                if (data.Length == 0)
                                {
                                    if (CurrentWindowStyle == IceTabPage.WindowType.Window)
                                        RemoveWindow(null, CurrentWindow.TabCaption, CurrentWindow.WindowStyle);
                                    else if (CurrentWindowStyle == IceTabPage.WindowType.Debug)
                                        RemoveWindow(null, "Debug", IceTabPage.WindowType.Debug);
                                    else if (CurrentWindowStyle == IceTabPage.WindowType.DCCFile)
                                        RemoveWindow(null, "DCC Files", IceTabPage.WindowType.DCCFile);
                                }
                                else if (data.ToLower() == "debug")
                                {
                                    RemoveWindow(null, "Debug", IceTabPage.WindowType.Debug);
                                }
                                else if (data.ToLower() == "dcc files")
                                {
                                    RemoveWindow(null, "DCC Files", IceTabPage.WindowType.DCCFile);
                                }
                                else
                                {
                                    if (data.StartsWith("@"))
                                        RemoveWindow(null, data, IceTabPage.WindowType.Window);
                                }
                            }
                            break;
                        case "/closequery":
                            System.Diagnostics.Debug.WriteLine("/closequery");
                            if (connection != null)
                            {
                                System.Diagnostics.Debug.WriteLine("valid connection");
                                for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                                {
                                    if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Query)
                                    {
                                        if (mainChannelBar.TabPages[i].Connection == connection)
                                        {
                                            RemoveWindow(connection, mainChannelBar.TabPages[i].TabCaption, IceTabPage.WindowType.Query);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/closeallquery":
                            if (connection != null)
                            {
                                for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                                {
                                    if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Query)
                                    {
                                        RemoveWindow(connection, mainChannelBar.TabPages[i].TabCaption, IceTabPage.WindowType.Query);
                                    }
                                }
                            }
                            break;
                        case "/ctcp":
                            if (connection != null && data.IndexOf(' ') > 0)
                            {
                                //ctcp nick ctcptype
                                string nick = data.Substring(0, data.IndexOf(' '));
                                //get the message
                                string ctcp = data.Substring(data.IndexOf(' ') + 1);

                                string msg = GetMessageFormat("Ctcp Send");
                                msg = msg.Replace("$nick", nick); ;
                                msg = msg.Replace("$ctcp", ctcp.ToUpper());
                                CurrentWindowMessage(connection, msg, "", true);
                                if (ctcp.ToUpper() == "PING")
                                    SendData(connection, "PRIVMSG " + nick + " :" + ctcp.ToUpper() + " " + System.Environment.TickCount.ToString() + "");
                                else
                                    SendData(connection, "PRIVMSG " + nick + " " + ctcp.ToUpper() + "");
                            }
                            break;
                        case "/dcc":
                            if (connection != null && data.IndexOf(' ') > 0)
                            {
                                //get the type of dcc
                                string dccType = data.Substring(0, data.IndexOf(' ')).ToUpper();
                                //get who it is being sent to
                                string nick = data.Substring(data.IndexOf(' ') + 1);

                                switch (dccType)
                                {
                                    case "CHAT":
                                        //start a dcc chat
                                        if (nick.IndexOf(' ') == -1)    //make sure no space in the nick name
                                        {
                                            //check if we already have a dcc chat open with this person
                                            if (!mainChannelBar.WindowExists(connection, nick, IceTabPage.WindowType.DCCChat))
                                            {
                                                //create a new window
                                                AddWindow(connection, nick, IceTabPage.WindowType.DCCChat);
                                                IceTabPage t = GetWindow(connection, nick, IceTabPage.WindowType.DCCChat);
                                                if (t != null)
                                                {
                                                    t.RequestDCCChat();
                                                    string msg = GetMessageFormat("DCC Chat Outgoing");
                                                    msg = msg.Replace("$nick", nick);
                                                    t.TextWindow.AppendText(msg, "");
                                                    t.TextWindow.ScrollToBottom();
                                                }
                                            }
                                            else
                                            {
                                                mainChannelBar.SelectTab(GetWindow(connection, nick, IceTabPage.WindowType.DCCChat));
                                                serverTree.SelectTab(mainChannelBar.CurrentTab, false);

                                                //see if it is connected or not
                                                IceTabPage dcc = GetWindow(connection, nick, IceTabPage.WindowType.DCCChat);
                                                if (dcc != null)
                                                {
                                                    if (!dcc.IsConnected)
                                                    {
                                                        dcc.RequestDCCChat();
                                                        string msg = GetMessageFormat("DCC Chat Outgoing");
                                                        msg = msg.Replace("$nick", nick);
                                                        dcc.TextWindow.AppendText(msg, "");
                                                        dcc.TextWindow.ScrollToBottom();
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "SEND":
                                        //was a filename specified, if not try and select one
                                        string file;
                                        if (nick.IndexOf(' ') > 0)
                                        {
                                            file = nick.Substring(nick.IndexOf(' ') + 1);
                                            nick = nick.Substring(0, nick.IndexOf(' '));

                                            //see if the file exists
                                            if (!File.Exists(file))
                                            {
                                                //file does not exists, just quit
                                                //try from the dccsend folder
                                                if (File.Exists(iceChatOptions.DCCSendFolder + Path.DirectorySeparatorChar + file))
                                                    file = iceChatOptions.DCCSendFolder + Path.DirectorySeparatorChar + file;
                                                else
                                                    return;
                                            }
                                        }
                                        else
                                        {
                                            //ask for a file name
                                            OpenFileDialog dialog = new OpenFileDialog();
                                            dialog.InitialDirectory = iceChatOptions.DCCSendFolder;
                                            dialog.CheckFileExists = true;
                                            dialog.CheckPathExists = true;
                                            if (dialog.ShowDialog() == DialogResult.OK)
                                            {
                                                //returns the full path
                                                System.Diagnostics.Debug.WriteLine(dialog.FileName);
                                                file = dialog.FileName;
                                            }
                                            else
                                                return;

                                        }

                                        //more to it, maybe a file to send                                            
                                        if (!mainChannelBar.WindowExists(null, "DCC Files", IceTabPage.WindowType.DCCFile))
                                            AddWindow(null, "DCC Files", IceTabPage.WindowType.DCCFile);

                                        IceTabPage tt = GetWindow(null, "DCC Files", IceTabPage.WindowType.DCCFile);
                                        if (tt != null)
                                            ((IceTabPageDCCFile)tt).RequestDCCFile(connection, nick, file);

                                        break;
                                }
                            }
                            break;
                        case "/describe":   //me command for a specific channel
                            if (connection != null && data.IndexOf(' ') > 0)
                            {
                                //get the channel name
                                string channel = data.Substring(0, data.IndexOf(' '));
                                //get the message
                                string message = data.Substring(data.IndexOf(' ') + 1);
                                //check for the channel
                                IceTabPage t = GetWindow(connection, channel, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    //SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + message + "");

                                    if (message.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(message);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + line + "");
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + t.TabCaption + " :ACTION " + message + "");
                                    } 

                                    string msg = GetMessageFormat("Self Channel Action");
                                    msg = msg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName).Replace("$channel", t.TabCaption);
                                    msg = msg.Replace("$message", message);
                                    t.TextWindow.AppendText(msg, "");
                                    t.TextWindow.ScrollToBottom();
                                    t.LastMessageType = ServerMessageType.Action;
                                }
                            }
                            break;
                        case "/dns":
                            if (data.Length > 0)
                            {
                                if (data.IndexOf(".") > 0)
                                {
                                    //dns a host
                                    try
                                    {
                                        System.Net.IPAddress address;
                                        if (System.Net.IPAddress.TryParse(data, out address))
                                        {
                                            // do a reverse dns
                                            System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry(data);
                                            ParseOutGoingCommand(connection, "/echo " + data + " resolved to " + host.HostName);
                                            foreach (string alias in host.Aliases)
                                            {
                                                //    System.Diagnostics.Debug.WriteLine(alias);
                                            }
                                        }
                                        else
                                        {

                                            args.Extra = data;
                                            foreach (Plugin p in loadedPlugins)
                                            {
                                                IceChatPlugin ipc = p as IceChatPlugin;
                                                if (ipc != null)
                                                {
                                                    if (ipc.plugin.Enabled == true)
                                                        args = ipc.plugin.DNSResolve(args);
                                                }
                                            }

                                            if (args.Extra.Length > 0)
                                            {

                                                System.Net.IPAddress[] addresslist = System.Net.Dns.GetHostAddresses(data);
                                                ParseOutGoingCommand(connection, "/echo " + data + " resolved to " + addresslist.Length + " address(es)");

                                                foreach (System.Net.IPAddress ipa in addresslist)
                                                    ParseOutGoingCommand(connection, "/echo -> " + ipa.ToString());
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ParseOutGoingCommand(connection, "/echo " + data + " does not resolve (unknown address)");
                                    }
                                }
                                else
                                {
                                    //dns a nickname (send a userhost)
                                    SendData(connection, "USERHOST " + data);
                                }
                            }
                            break;
                        case "/echo":
                            if (data.Length > 0)
                            {
                                //check if we are on the current server or not, otherwise , echo to console
                                if (CurrentWindow.Connection != connection)
                                {
                                    //echo to the console
                                    string msg = GetMessageFormat("User Echo");
                                    msg = msg.Replace("$message", "\x000F" + data);
                                    WindowMessage(connection, "Console", msg, "", true);
                                }
                                else
                                {
                                    if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.Query)
                                    {
                                        string msg = GetMessageFormat("User Echo");
                                        msg = msg.Replace("$message", "\x000F" + data);

                                        CurrentWindow.TextWindow.AppendText(msg, "");
                                    }
                                    else if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                                    {
                                        string msg = GetMessageFormat("User Echo");
                                        msg = msg.Replace("$message", "\x000F" + data);

                                        mainChannelBar.GetTabPage("Console").CurrentConsoleWindow().AppendText(msg, "");
                                    }
                                    else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                                    {
                                        string msg = GetMessageFormat("User Echo");
                                        msg = msg.Replace("$message", "\x000F" + data);

                                        CurrentWindow.TextWindow.AppendText(msg, "");
                                    }
                                    else if (CurrentWindowStyle == IceTabPage.WindowType.Window)
                                    {
                                        string msg = GetMessageFormat("User Echo");
                                        msg = msg.Replace("$message", "\x000F" + data);

                                        CurrentWindow.TextWindow.AppendText(msg, "");
                                    }
                                }
                            }
                            break;
                        case "/export":
                            if (connection != null)
                            {
                                //export the channel list to a file
                                if (CurrentWindowStyle == IceTabPage.WindowType.ChannelList)
                                {
                                    System.Diagnostics.Debug.WriteLine("Total:" + CurrentWindow.ChannelList.Items.Count);
                                    if (CurrentWindow.ChannelList.Items.Count > 0)
                                    {
                                        //ask for a file name
                                        SaveFileDialog sfd = new SaveFileDialog();

                                        sfd.Filter = "TXT Files (*.txt)|*.txt";
                                        sfd.FilterIndex = 2;
                                        sfd.RestoreDirectory = true;
                                        if (sfd.ShowDialog() == DialogResult.OK)
                                        {
                                            //this is the full filename
                                            StreamWriter writer = new StreamWriter(sfd.OpenFile());

                                            for (int i = 1; i < CurrentWindow.ChannelList.Items.Count; i++)
                                            {
                                                writer.WriteLine(CurrentWindow.ChannelList.Items[i].Text + " : " + CurrentWindow.ChannelList.Items[i].SubItems[1].Text + " : " + CurrentWindow.ChannelList.Items[i].SubItems[2].Text);
                                            }
                                            writer.Flush();
                                            writer.Close();
                                            writer.Dispose();

                                            MessageBox.Show("Channel List Exported");
                                        }
                                    }
                                }

                            }
                            break;
                        case "/emoticons":
                            // used to set emoticons on or off for a channel
                            if (connection != null && data.Length > 0)
                            {
                                if (data.IndexOf(" ") > 0)
                                {
                                    // /emoticons #channel [on/off]
                                    string window = data.Substring(0, data.IndexOf(' '));
                                    string co = data.Substring(data.IndexOf(' ') + 1).ToLower();
                                    if (co.IndexOf(' ') == -1)
                                    {
                                        IceTabPage c = GetWindow(connection, window, IceTabPage.WindowType.Channel);
                                        if (c != null)
                                        {
                                            if (co == "on")
                                                c.TextWindow.NoEmoticons = false;
                                            else if (co == "off")
                                                c.TextWindow.NoEmoticons = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "/colortab":
                            // used to change the color of the text of a specific channel or query
                            if (connection != null && data.Length > 0)
                            {
                                if (data.IndexOf(" ") > 0)
                                {
                                    // /colortab #channel [forecolor<,back>]
                                    string window = data.Substring(0, data.IndexOf(' '));
                                    string co = data.Substring(data.IndexOf(' ') + 1);
                                    if (co.IndexOf(' ') == -1)
                                    {
                                        // no space in the color, only forecolor
                                        IceTabPage c = GetWindow(connection, window, IceTabPage.WindowType.Channel);
                                        if (c != null)
                                        {
                                            // set the new custom color
                                            c.CustomForeColor = Convert.ToInt32(co);

                                            //set the message type to custom
                                            c.LastMessageType = ServerMessageType.CustomMessage;

                                        }
                                        else
                                        {
                                            IceTabPage q = GetWindow(connection, window, IceTabPage.WindowType.Query);
                                            if (q != null)
                                            {
                                                // set the new custom color
                                                q.CustomForeColor = Convert.ToInt32(co);

                                                //set the message type to custom
                                                q.LastMessageType = ServerMessageType.CustomMessage;

                                            }
                                        }

                                    }
                                    else
                                    {
                                        // foreground / background color
                                        // currently not implemented

                                    }
                                }
                            }
                            break;
                        case "/flash":
                        case "/flashtab":
                            //used to flash a specific channel or query
                            if (connection != null && data.Length > 0)
                            {
                                string window = data;
                                bool flashWindow = true;
                                if (data.IndexOf(" ") > 0)
                                {
                                    window = data.Substring(0, data.IndexOf(' '));
                                    string t = data.Substring(data.IndexOf(' ') + 1);
                                    if (t.ToLower() == "off")
                                        flashWindow = false;
                                }

                                //check if it is a channel window
                                IceTabPage c = GetWindow(connection, window, IceTabPage.WindowType.Channel);
                                if (c != null)
                                {
                                    if (!c.EventOverLoad)
                                    {
                                        c.FlashTab = flashWindow;
                                        mainChannelBar.Invalidate();
                                        serverTree.Invalidate();
                                    }
                                }
                                else
                                {
                                    //check if it is a query
                                    IceTabPage q = GetWindow(connection, window, IceTabPage.WindowType.Query);
                                    if (q != null)
                                    {
                                        q.FlashTab = flashWindow;
                                        mainChannelBar.Invalidate();
                                        serverTree.Invalidate();
                                    }
                                }

                            }
                            break;
                        case "/flashtask":
                        case "/flashtaskbar":
                            FlashTaskBar();
                            break;

                        case "/flashtray":
                            //check if we are minimized                            
                            if (this.notifyIcon.Visible == true)
                            {
                                this.flashTrayIconTimer.Enabled = true;
                                this.flashTrayIconTimer.Start();
                                //show a message in a balloon
                                if (data.Length > 0)
                                {
                                    this.notifyIcon.BalloonTipTitle = "IceChat 9";
                                    this.notifyIcon.BalloonTipText = StripColorCodes(data);
                                    this.notifyIcon.ShowBalloonTip(1000);
                                }
                            }
                            break;

                        case "/sound":
                            //change the sound of the current window
                            //check if data is a channel
                            if (connection != null && data.Length > 0)
                            {
                                if (data.IndexOf(' ') == -1)
                                {
                                    IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                    if (t != null)
                                    {
                                        t.DisableSounds = !t.DisableSounds;
                                    }
                                }
                            }
                            break;
                        case "/font":
                            //change the font of the current window
                            //check if data is a channel
                            if (connection != null && data.Length > 0)
                            {
                                if (data.IndexOf(' ') == -1)
                                {
                                    IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                    if (t != null)
                                    {
                                        //bring up a font dialog
                                        FontDialog fd = new FontDialog();
                                        //load the current font
                                        fd.Font = t.TextWindow.Font;

                                        fd.ShowEffects = false;
                                        fd.ShowColor = false;
                                        fd.FontMustExist = true;
                                        fd.AllowVectorFonts = false;
                                        fd.AllowVerticalFonts = false;
                                        try
                                        {
                                            if (fd.ShowDialog() != DialogResult.Cancel && fd.Font.Style == FontStyle.Regular)
                                            {
                                                t.TextWindow.Font = fd.Font;
                                            }
                                            else
                                            {
                                                if (fd.Font.Style != FontStyle.Regular)
                                                {
                                                    MessageBox.Show("IceChat only supports 'Regular' font styles", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show("IceChat only supports TrueType fonts", "Font Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/forcequit":
                            if (connection != null)
                            {
                                connection.AttemptReconnect = false;
                                connection.ForceDisconnect();
                            }
                            break;
                        case "/google":
                            if (data.Length > 0)
                                System.Diagnostics.Process.Start("http://www.google.com/search?q=" + data);
                            else
                                System.Diagnostics.Process.Start("http://www.google.com");
                            break;
                        case "/hop":
                        case "/cycle":
                            if (connection != null && data.Length == 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    CurrentWindow.ChannelHop = true;
                                    string key = CurrentWindow.ChannelKey;

                                    temp = CurrentWindow.TabCaption;
                                    SendData(connection, "PART " + temp);
                                    if (key.Length > 0 && key != "*")
                                        ParseOutGoingCommand(connection, "/timer joinhop 1 1 /join " + temp + " " + key);
                                    else
                                        ParseOutGoingCommand(connection, "/timer joinhop 1 1 /join " + temp);

                                }
                            }
                            else
                            {
                                IceTabPage t = GetWindow(connection, data, IceTabPage.WindowType.Channel);
                                if (t != null)
                                {
                                    t.ChannelHop = true;
                                    string key = CurrentWindow.ChannelKey;

                                    SendData(connection, "PART " + t.TabCaption);
                                    if (key.Length > 0 && key != "*")
                                        ParseOutGoingCommand(connection, "/timer joinhop 1 1 /join " + t.TabCaption + " " + key);
                                    else
                                        ParseOutGoingCommand(connection, "/timer joinhop 1 1 /join " + t.TabCaption);

                                }
                            }
                            break;
                        case "/rejoin":
                            if (connection != null)
                            {
                                if (connection.ServerSetting.LastChannelsParted.Count > 0)
                                {
                                    string channel = connection.ServerSetting.LastChannelsParted.Pop();
                                    ParseOutGoingCommand(connection, "/join " + channel);
                                }
                            }
                            break;
                        case "/rejoinall":
                            if (this.GlobalLastChannels.Count > 0)
                            {
                                KeyValuePair<string, IRCConnection> keyValue = GlobalLastChannels.Pop();
                                ParseOutGoingCommand(keyValue.Value, "/join " + keyValue.Key);
                            }
                            break;
                        case "/rejoinlist":
                            if (connection != null)
                            {
                                if (connection.ServerSetting.LastChannelsParted.Count > 0)
                                {
                                    foreach (string item in connection.ServerSetting.LastChannelsParted)
                                    {
                                        ParseOutGoingCommand(connection, "/echo " + item);
                                    }
                                }
                                else
                                {
                                    // show its empty
                                    ParseOutGoingCommand(connection, "/echo Rejoin list is empty");
                                }
                            }
                            break;

                        case "/rejoinlistall":
                            if (this.GlobalLastChannels.Count > 0)
                            {
                                foreach (KeyValuePair<string, IRCConnection> keyValue in GlobalLastChannels)
                                {
                                    ParseOutGoingCommand(connection, "/echo " + keyValue.Key + " on " + keyValue.Value.ServerSetting.ServerName);
                                }
                            }
                            break;
                        case "/icechat":
                            if (connection != null)
                                ParseOutGoingCommand(connection, "/me is using " + ProgramID + " " + VersionID + " - Build " + BuildNumber);
                            else
                                ParseOutGoingCommand(connection, "/echo you are using " + ProgramID + " " + VersionID + " - Build " + BuildNumber);
                            break;
                        case "/icepath":
                            //To get current Folder and paste it into /me
                            if (connection != null)
                                ParseOutGoingCommand(connection, "/me Build Path = " + Directory.GetCurrentDirectory());
                            else
                                ParseOutGoingCommand(connection, "/echo Build Path = " + Directory.GetCurrentDirectory());
                            break;
                        case "/ignore":
                            if (connection != null)
                            {
                                if (data.Length > 0)
                                {
                                    //check if just a nick/host , no extra params
                                    if (data.IndexOf(" ") == -1)
                                    {
                                        if (data.ToLower() == "enable")
                                        {
                                            connection.ServerSetting.IgnoreListEnable = true;
                                            ParseOutGoingCommand(connection, "/echo ignore list enabled");
                                            return;
                                        }
                                        else if (data.ToLower() == "disable")
                                        {
                                            connection.ServerSetting.IgnoreListEnable = false;
                                            ParseOutGoingCommand(connection, "/echo ignore list disabled");
                                            return;
                                        }
                                    }

                                    // do a -l to list them
                                    if (data.ToLower() == "-l")
                                    {
                                        System.Diagnostics.Debug.WriteLine("list ignores");
                                        if (connection.ServerSetting.Ignores != null || connection.ServerSetting.Ignores.Length > 0)
                                        {
                                            for (int i = 0; i < connection.ServerSetting.Ignores.Length; i++)
                                            {
                                                if (connection.ServerSetting.Ignores[i].Enabled)
                                                {
                                                    //connection.ServerSetting.Ignores[i].IgnoreType
                                                    ParseOutGoingCommand(connection, "/echo -> " + connection.ServerSetting.Ignores[i].Item);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // empty list
                                            ParseOutGoingCommand(connection, "/echo Ignore list is empty for this server");

                                        }

                                        return;
                                    }

                                    string ignoreNick = data;
                                    IgnoreType ignoreType = new IgnoreType(0);

                                    bool removeIgnore = false;

                                    if (data.StartsWith("-"))
                                    {
                                        // we have a param
                                        // [-pcn]
                                        // -p - ignores the private messages
                                        // -c - ignores the channel messages
                                        // -n - ignores the notice messages
                                        // -t - ctcp
                                        // -i - invite
                                        // -d - dcc

                                        string param = data.Substring(1).Split(' ')[0];
                                        ignoreNick = data.Split(' ')[1];

                                        foreach (char p in param)
                                        {
                                            switch (p)
                                            {
                                                case 'p':   // ignore in private
                                                    ignoreType.All = false;
                                                    ignoreType.Private = true;
                                                    break;
                                                case 'c':   // ignore in channel
                                                    ignoreType.All = false; 
                                                    ignoreType.Channel = true;
                                                    break;
                                                case 'n':   // ignore in notice
                                                    ignoreType.All = false;
                                                    ignoreType.Notice = true;
                                                    break;
                                                case 't':   // ignore in ctcp
                                                    ignoreType.All = false;
                                                    ignoreType.Ctcp = true;
                                                    break;
                                                case 'd':   // ignore dcc
                                                    ignoreType.All = false;
                                                    ignoreType.DCC = true;
                                                    break;
                                                case 'i':   // ignore invite
                                                    ignoreType.All = false;
                                                    ignoreType.Invite = true;
                                                    break;
                                                case 'r':   // remove ignore from list
                                                    removeIgnore = true;
                                                    break;
                                            }
                                        }
                                    }


                                    //check if already in ignore list or not
                                    if (connection.ServerSetting.Ignores != null)
                                    {
                                        if (removeIgnore == false)
                                        {

                                            for (int i = 0; i < connection.ServerSetting.Ignores.Length; i++)
                                            {
                                                string checkNick = connection.ServerSetting.Ignores[i].Item;

                                                if (checkNick.ToLower() == ignoreNick.ToLower())
                                                {
                                                    if (!connection.ServerSetting.Ignores[i].Enabled)
                                                    {
                                                        connection.ServerSetting.Ignores[i].Enabled = true;
                                                        connection.ServerSetting.Ignores[i].IgnoreType.MergeIgnore(ignoreType);

                                                        ParseOutGoingCommand(connection, "/echo " + checkNick + " enabled to ignore list");
                                                    }
                                                    else
                                                    {
                                                        // is the ignoreType set?
                                                        if (connection.ServerSetting.Ignores[i].IgnoreType.ToString() == ignoreType.ToString())
                                                        {
                                                            connection.ServerSetting.Ignores[i].Enabled = false;
                                                            ParseOutGoingCommand(connection, "/echo " + checkNick + " disabled in ignore list");
                                                        }
                                                        else
                                                        {
                                                            connection.ServerSetting.Ignores[i].IgnoreType.MergeIgnore(ignoreType);
                                                            ParseOutGoingCommand(connection, "/echo " + checkNick + " updated in ignore list");
                                                        }
                                                    }

                                                    serverTree.SaveServers(serverTree.ServersCollection);
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // remove from the ignore list
                                            bool foundMatch = false;
                                            System.Diagnostics.Debug.WriteLine("Delete from List:" + ignoreNick);
                                            //IgnoreListItem[] ignores; ;
                                            IgnoreListItem[] ignores = new IgnoreListItem[connection.ServerSetting.Ignores.Length];
                                            int counter = 0;
                                            for (int i = 0; i < connection.ServerSetting.Ignores.Length; i++)
                                            {
                                                string checkNick = connection.ServerSetting.Ignores[i].Item;

                                                if (checkNick.ToLower() == ignoreNick.ToLower())
                                                {
                                                    foundMatch = true;
                                                    ParseOutGoingCommand(connection, "/echo " + checkNick + " removed from ignore list");
                                                }
                                                else
                                                {

                                                    ignores[counter] = new IgnoreListItem();
                                                    ignores[counter].IgnoreType = new IgnoreType();

                                                    ignores[counter].Item = connection.ServerSetting.Ignores[i].Item;
                                                    ignores[counter].Enabled = connection.ServerSetting.Ignores[i].Enabled;

                                                    ignores[counter].IgnoreType = connection.ServerSetting.Ignores[i].IgnoreType;
                                                    
                                                    counter++;

                                                }
                                            }
                                            if (foundMatch == true)
                                            {
                                                // resize the array
                                                Array.Resize(ref ignores, ignores.Length - 1);
                                                connection.ServerSetting.Ignores = ignores;
                                                serverTree.SaveServers(serverTree.ServersCollection);
                                            }
                                        }
                                    }

                                    //no match found, add the new item to the IgnoreList
                                    // only add if removeIgnore flag (-r) is not set
                                    if (removeIgnore == false)
                                    {
                                        
                                        IgnoreListItem[] ignores;

                                        if (connection.ServerSetting.Ignores != null)
                                        {
                                            ignores = connection.ServerSetting.Ignores;
                                            Array.Resize(ref ignores, ignores.Length + 1);
                                        }
                                        else
                                        {
                                            ignores = new IgnoreListItem[1];
                                        }

                                        ignores[ignores.Length - 1] = new IgnoreListItem();

                                        ignores[ignores.Length - 1].Item = ignoreNick;
                                        ignores[ignores.Length - 1].Enabled = true;

                                        ignores[ignores.Length - 1].IgnoreType = ignoreType;

                                        connection.ServerSetting.Ignores = ignores;
                                        connection.ServerSetting.IgnoreListEnable = true;

                                        ParseOutGoingCommand(connection, "/echo " + ignoreNick + " added to ignore list");

                                        serverTree.SaveServers(serverTree.ServersCollection);

                                    }
                                }
                            }
                            break;
                        case "/join":
                            if (connection != null && data.Length > 0)
                            {
                                if (connection.ServerSetting.ChannelTypes != null && Array.IndexOf(connection.ServerSetting.ChannelTypes, data[0]) == -1)
                                {
                                    data = connection.ServerSetting.ChannelTypes[0] + data;
                                }
                                error = 1;
                                if (data.IndexOf(' ') > -1)
                                {
                                    string[] c = data.Split(new char[] { ' ' }, 2);
                                    error = 2;
                                    if (connection.ServerSetting.ChannelJoins.ContainsKey(c[0].ToLower()))
                                    {
                                        error = 3;
                                        connection.ServerSetting.ChannelJoins[c[0].ToLower()] = c[1];
                                    }
                                    else
                                    {
                                        error = 4;
                                        connection.ServerSetting.ChannelJoins.Add(c[0].ToLower(), c[1]);
                                    }
                                    error = 5;
                                }
                                else
                                {
                                    error = 6;
                                    if (!connection.ServerSetting.ChannelJoins.ContainsKey(data.ToLower()))
                                    {
                                        error = 7;
                                        connection.ServerSetting.ChannelJoins.Add(data.ToLower(), "");
                                    }
                                    else
                                    {
                                        error = 8;
                                        connection.ServerSetting.ChannelJoins[data.ToLower()] = "";
                                    }
                                    error = 9;
                                }
                                error = 10;
                                SendData(connection, "JOIN " + data);
                            }
                            break;
                        case "/kick":
                            if (connection != null && data.Length > 0)
                            {
                                //kick #channel nick reason
                                if (data.IndexOf(' ') > 0)
                                {
                                    //get the channel
                                    temp = data.Substring(0, data.IndexOf(' '));
                                    //check if temp is a channel or not
                                    if (Array.IndexOf(connection.ServerSetting.ChannelTypes, temp[0]) == -1)
                                    {
                                        //temp is not a channel, substitute with current channel
                                        //make sure we are in a channel
                                        if (CurrentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                                        {
                                            temp = CurrentWindow.TabCaption;
                                            if (data.IndexOf(' ') > 0)
                                            {
                                                //there is a kick reason
                                                string msg = data.Substring(data.IndexOf(' ') + 1);
                                                data = data.Substring(0, data.IndexOf(' '));
                                                SendData(connection, "KICK " + temp + " " + data + " :" + msg);
                                            }
                                            else
                                            {
                                                SendData(connection, "KICK " + temp + " " + data);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        data = data.Substring(temp.Length + 1);
                                        if (data.IndexOf(' ') > 0)
                                        {
                                            //there is a kick reason
                                            string msg = data.Substring(data.IndexOf(' ') + 1);
                                            data = data.Substring(0, data.IndexOf(' '));
                                            SendData(connection, "KICK " + temp + " " + data + " :" + msg);
                                        }
                                        else
                                        {
                                            SendData(connection, "KICK " + temp + " " + data);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/me":
                            //check if in channel, query, etc
                            if (connection != null && data.Length > 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.Query)
                                {
                                    //SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :ACTION " + data + "");

                                    if (data.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(data);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :ACTION " + line + "");
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :ACTION " + data + "");
                                    } 


                                    string msg = GetMessageFormat("Self Channel Action");
                                    msg = msg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName).Replace("$channel", CurrentWindow.TabCaption);
                                    msg = msg.Replace("$message", data);

                                    CurrentWindow.TextWindow.AppendText(msg, "");
                                    CurrentWindow.TextWindow.ScrollToBottom();
                                    CurrentWindow.LastMessageType = ServerMessageType.Action;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                                {

                                    IceTabPage c = GetWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.DCCChat);
                                    if (c != null)
                                    {
                                        c.SendDCCData("ACTION " + data + "");

                                        string msg = GetMessageFormat("DCC Chat Action");
                                        msg = msg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName);
                                        msg = msg.Replace("$message", data);

                                        CurrentWindow.TextWindow.AppendText(msg, "");
                                        CurrentWindow.TextWindow.ScrollToBottom();
                                        CurrentWindow.LastMessageType = ServerMessageType.Action;

                                    }
                                }
                            }
                            break;
                        case "/umode":
                            if (connection != null && data.Length > 0)
                                SendData(connection, "MODE " + connection.ServerSetting.CurrentNickName + " " + data);
                            break;
                        case "/mode":
                            if (connection != null && data.Length > 0)
                                SendData(connection, "MODE " + data);
                            break;
                        case "/modex":
                            if (connection != null)
                                SendData(connection, "MODE " + connection.ServerSetting.CurrentNickName + " +x");
                            break;
                        case "/motd":
                            if (connection != null)
                            {
                                connection.ServerSetting.ForceMOTD = true;
                                SendData(connection, "MOTD");
                            }
                            break;
                        case "/msg":
                        case "/msgsec":
                            if (connection != null && data.IndexOf(' ') > -1)
                            {
                                string nick = data.Substring(0, data.IndexOf(' '));
                                string msg2 = data.Substring(data.IndexOf(' ') + 1);
                                if (nick.StartsWith("="))
                                {
                                    //send to a dcc chat window
                                    nick = nick.Substring(1);

                                    IceTabPage c = GetWindow(connection, nick, IceTabPage.WindowType.DCCChat);
                                    if (c != null)
                                    {
                                        c.SendDCCData(data);
                                        string msg = GetMessageFormat("Self DCC Chat Message");
                                        if (command.ToLower() == "/msgsec")
                                            msg = msg.Replace("$nick", c.Connection.ServerSetting.CurrentNickName).Replace("$message", "*");
                                        else
                                            msg = msg.Replace("$nick", c.Connection.ServerSetting.CurrentNickName).Replace("$message", data);

                                        c.TextWindow.AppendText(msg, "");

                                    }
                                }
                                else
                                {
                                    //SendData(connection, "PRIVMSG " + nick + " :" + msg2);

                                    if (msg2.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(msg2);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + nick + " :" + line);
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + nick + " :" + msg2);
                                    } 


                                    //get the color for the private message
                                    string msg = GetMessageFormat("Self Channel Message");
                                    msg = msg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$channel", nick);

                                    //check if the nick has a query window open
                                    IceTabPage q = GetWindow(connection, nick, IceTabPage.WindowType.Query);
                                    if (q != null)
                                    {
                                        string nmsg = GetMessageFormat("Self Private Message");
                                        if (command.ToLower() == "/msgsec")
                                            nmsg = nmsg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$message", "*");
                                        else
                                            nmsg = nmsg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$message", msg2);

                                        q.TextWindow.AppendText(nmsg, "");
                                        q.LastMessageType = ServerMessageType.Message;

                                    }
                                    else
                                    {
                                        IceTabPage t = GetWindow(connection, nick, IceTabPage.WindowType.Channel);
                                        if (t != null)
                                        {
                                            if (msg.Contains("$color"))
                                            {
                                                User u = t.GetNick(connection.ServerSetting.CurrentNickName);

                                                System.Diagnostics.Debug.WriteLine(u.nickColor + ":" + u.NickName);

                                                //get the nick color
                                                if (u.nickColor == -1)
                                                {
                                                    if (IceChatColors.RandomizeNickColors == true)
                                                    {
                                                        int randColor = new Random().Next(0, 71);
                                                        if (randColor == IceChatColors.NickListBackColor)
                                                            randColor = new Random().Next(0, 71);
                                                        u.nickColor = randColor;
                                                    }
                                                    else
                                                    {
                                                        //get the correct nickname color for channel status
                                                        for (int y = 0; y < u.Level.Length; y++)
                                                        {
                                                            if (u.Level[y])
                                                            {
                                                                switch (connection.ServerSetting.StatusModes[0][y])
                                                                {
                                                                    case 'q':
                                                                    case 'y':
                                                                        u.nickColor = IceChatColors.ChannelOwnerColor;
                                                                        break;
                                                                    case 'a':
                                                                        u.nickColor = IceChatColors.ChannelAdminColor;
                                                                        break;
                                                                    case 'o':
                                                                        u.nickColor = IceChatColors.ChannelOpColor;
                                                                        break;
                                                                    case 'h':
                                                                        u.nickColor = IceChatColors.ChannelHalfOpColor;
                                                                        break;
                                                                    case 'v':
                                                                        u.nickColor = IceChatColors.ChannelVoiceColor;
                                                                        break;
                                                                    default:
                                                                        u.nickColor = IceChatColors.ChannelRegularColor;
                                                                        break;
                                                                }
                                                                break;
                                                            }
                                                        }

                                                    }

                                                    System.Diagnostics.Debug.WriteLine(u.nickColor);

                                                    if (u.nickColor == -1)
                                                        u.nickColor = IceChatColors.ChannelRegularColor;
                                                }

                                                msg = msg.Replace("$color", "\x0003" + u.nickColor.ToString("00"));
                                            }

                                            if (t.GetNick(connection.ServerSetting.CurrentNickName) != null)
                                                msg = msg.Replace("$status", t.GetNick(connection.ServerSetting.CurrentNickName).ToString().Replace(connection.ServerSetting.CurrentNickName, ""));

                                            if (command.ToLower() == "/msgsec")
                                                msg = msg.Replace("$message", "*");
                                            else
                                                msg = msg.Replace("$message", msg2);

                                            t.TextWindow.AppendText(msg, "");
                                            t.LastMessageType = ServerMessageType.Message;
                                        }
                                        else
                                        {
                                            //send to the current window
                                            if (msg.StartsWith("&#x3;"))
                                            {
                                                //get the color
                                                string color = msg.Substring(0, 6);
                                                int result;
                                                if (Int32.TryParse(msg.Substring(6, 1), out result))
                                                    color += msg.Substring(6, 1);
                                                if (command.ToLower() == "/msgsec")
                                                    msg = color + "*" + nick + "* *";
                                                else
                                                    msg = color + "*" + nick + "* " + data.Substring(data.IndexOf(' ') + 1);
                                            }
                                            CurrentWindowMessage(connection, msg, "", true);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/names":
                            if (connection != null && data.Length > 0)
                            {
                                SendData(connection, "NAMES " + data);
                            }
                            break;
                        case "/nick":
                            if (connection != null && data.Length > 0)
                            {
                                connection.SendData("NICK " + data);
                                connection.ServerSetting.SendNickServPassword = false;

                                if (data.IndexOf(' ') == -1)
                                {
                                    if (connection.ServerSetting.NickName.CompareTo(data) != 0)
                                        connection.ServerSetting.NickName = data;
                                }
                                else
                                {
                                    //has a space
                                    string nick = data.Substring(0, data.IndexOf(' '));
                                    if (connection.ServerSetting.NickName.CompareTo(nick) != 0)
                                        connection.ServerSetting.NickName = nick;
                                }
                            }
                            break;
                        case "/notice":
                            if (connection != null && data.IndexOf(' ') > -1)
                            {
                                string nick = data.Substring(0, data.IndexOf(' '));
                                string msg = data.Substring(data.IndexOf(' ') + 1);
                                SendData(connection, "NOTICE " + nick + " :" + msg);

                                string nmsg = GetMessageFormat("Self Notice");
                                nmsg = nmsg.Replace("$nick", nick).Replace("$message", msg);

                                CurrentWindowMessage(connection, nmsg, "", true);
                            }
                            break;
                        case "/onotice":
                            if (connection != null && data.IndexOf(' ') > -1)
                            {
                                string nick = data.Substring(0, data.IndexOf(' '));
                                string msg = data.Substring(data.IndexOf(' ') + 1);
                                SendData(connection, "NOTICE @" + nick + " :" + msg);

                                string nmsg = GetMessageFormat("Self Notice");
                                nmsg = nmsg.Replace("$nick", nick).Replace("$message", msg);

                                CurrentWindowMessage(connection, nmsg, "", true);
                            }
                            break;
                        case "/parse":
                            //if (data.Length == 0)
                            {
                                /*
                                string pattern2 = @"\[style.*\](.+?)\[/style\]";
                                Regex regex = new Regex(pattern2, RegexOptions.IgnoreCase);
                                //[br][Style ff:Tahoma;bgco:blue;co:blue;b;]___.[/style][Style ff:Tahoma;bgco:blue;co:yellow;b;]The Script－Testing Room[/style][Style ff:Tahoma;bgco:blue;co:blue;b;].___[/style][br][Style co:gold;b;]C[Style co:red;b;]RI[Style－co:blue;b;]m[Style co:green;b;]-[Style co:blue;b;]m[Style co:red;b;]IR[Style co:gold;b;]C [Style co:black;]http://crim-mirc.com[br][Style ff:Tahoma;bgco:red;co:red;b;]___.[/style][Style ff:Tahoma;bgco:red;co:white;b;]The Script Testing－Room[/style]
                                
                                MatchCollection m = regex.Matches(@data);
                                System.Diagnostics.Debug.WriteLine(m.Count);
                                string s = Regex.Replace(data, @"\[[^]]+\]", "");
                                
                                string p = regex.Replace(data, "$1");

                                System.Diagnostics.Debug.WriteLine(p);
                                System.Diagnostics.Debug.WriteLine(s);
                                */

                                //string message = @"&#x3;0<04@Snerf&#x3;> heya \test how are ya";
                                string message = "123 \test nick|name 123";
                                string nick = "\test";
                                nick = "nick|name";

                                // \t = tab
                                // \d = digit
                                // \b = boundary

                                if (Regex.IsMatch(message, Regex.Escape(nick), RegexOptions.IgnoreCase))
                                {
                                    System.Diagnostics.Debug.WriteLine("match");
                                }
                                else
                                    System.Diagnostics.Debug.WriteLine("no match");


                            }
                            break;
                        case "/part":
                            if (connection != null && data.Length > 0)
                            {
                                //check if it is a query window
                                IceTabPage q = GetWindow(connection, data, IceTabPage.WindowType.Query);
                                if (q != null)
                                {
                                    RemoveWindow(connection, q.TabCaption, IceTabPage.WindowType.Query);
                                    return;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                                {
                                    RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.Query);
                                    return;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                                {
                                    RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.DCCChat);
                                    return;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Window)
                                {
                                    RemoveWindow(null, CurrentWindow.TabCaption, IceTabPage.WindowType.Window);
                                    return;
                                }
                                //is there a part message
                                if (data.IndexOf(' ') > -1)
                                {
                                    //check if channel is a valid channel
                                    if (Array.IndexOf(connection.ServerSetting.ChannelTypes, data[0]) != -1)
                                    {
                                        SendData(connection, "PART " + data.Substring(0, data.IndexOf(' ')) + " :" + data.Substring(data.IndexOf(' ') + 1));
                                        RemoveWindow(connection, data.Substring(0, data.IndexOf(' ')), IceTabPage.WindowType.Channel);
                                    }
                                    else
                                    {
                                        //not a valid channel, use the current window
                                        if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                        {
                                            SendData(connection, "PART " + CurrentWindow.TabCaption + " :" + data);
                                            RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.Channel);
                                        }
                                    }
                                }
                                else
                                {
                                    //see if data is a valid channel;
                                    if (Array.IndexOf(connection.ServerSetting.ChannelTypes, data[0]) != -1)
                                    {
                                        SendData(connection, "PART " + data);
                                        RemoveWindow(connection, data, IceTabPage.WindowType.Channel);
                                    }
                                    else
                                    {
                                        if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                        {
                                            SendData(connection, "PART " + CurrentWindow.TabCaption + " :" + data);
                                            RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.Channel);
                                        }
                                    }
                                }
                            }
                            else if (connection != null)
                            {
                                //check if current window is channel
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    SendData(connection, "PART " + CurrentWindow.TabCaption);
                                    RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.Channel);
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                                {
                                    RemoveWindow(connection, CurrentWindow.TabCaption, IceTabPage.WindowType.Query);
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Window)
                                {
                                    RemoveWindow(null, CurrentWindow.TabCaption, IceTabPage.WindowType.Window);
                                }
                            }
                            break;
                        case "/partall":
                            if (connection != null)
                            {
                                for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                                {
                                    if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                                    {
                                        if (mainChannelBar.TabPages[i].Connection == connection)
                                        {
                                            if (connection.IsConnected)
                                                SendData(connection, "PART " + mainChannelBar.TabPages[i].TabCaption);

                                            RemoveWindow(connection, mainChannelBar.TabPages[i].TabCaption, IceTabPage.WindowType.Channel);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/spartall":
                            for (int i = mainChannelBar.TabPages.Count - 1; i >= 0; i--)
                            {
                                if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    if (mainChannelBar.TabPages[i].Connection.IsConnected)
                                        SendData(mainChannelBar.TabPages[i].Connection, "PART " + mainChannelBar.TabPages[i].TabCaption);

                                    RemoveWindow(mainChannelBar.TabPages[i].Connection, mainChannelBar.TabPages[i].TabCaption, IceTabPage.WindowType.Channel);
                                }
                            }
                            break;
                        case "/ping":
                            if (connection != null && data.Length > 0 && data.IndexOf(' ') == -1)
                            {
                                //ctcp nick ping
                                string msg = GetMessageFormat("Ctcp Send");
                                msg = msg.Replace("$nick", data); ;
                                msg = msg.Replace("$ctcp", "PING");
                                CurrentWindowMessage(connection, msg, "", true);
                                SendData(connection, "PRIVMSG " + data + " :PING " + System.Environment.TickCount.ToString() + "");
                            }
                            break;
                        case "/cplay":  //play a file in a specific channel /cplay #channel test.wav
                            if (connection != null && data.Length > 4 && data.IndexOf(' ') > 0)
                            {
                                string chan = data.Substring(0, data.IndexOf(' '));
                                string soundFile = data.Substring(data.IndexOf(' ') + 1);
                                //check if the channel is muted or not
                                ChannelSetting cs = ChannelSettings.FindChannel(chan, connection.ServerSetting.NetworkName);
                                if (cs != null)
                                {
                                    if (cs.SoundsDisable)
                                        return;
                                }

                                ParseOutGoingCommand(connection, "/play " + soundFile);

                            }
                            break;

                        case "/play":   //play a WAV sound or MP3
                            if (data.Length > 4 && (data.ToLower().EndsWith(".wav") || data.ToLower().EndsWith(".mp3")))
                            {
                                //check if the WAV file exists in the Sounds Folder                                
                                //check if the entire path was passed for the sound file
                                if (File.Exists(data))
                                {
                                    try
                                    {
                                        if (StaticMethods.IsRunningOnMono())
                                        {
                                            player.SoundLocation = @data;
                                            player.Play();
                                        }
                                        else
                                        {
                                            mp3Player.Open(data);
                                            mp3Player.Play();
                                        }
                                    }
                                    catch { }
                                }
                                else if (File.Exists(soundsFolder + System.IO.Path.DirectorySeparatorChar + data))
                                {
                                    try
                                    {
                                        if (StaticMethods.IsRunningOnMono())
                                        {
                                            player.SoundLocation = soundsFolder + System.IO.Path.DirectorySeparatorChar + data;
                                            player.Play();
                                        }
                                        else
                                        {
                                            mp3Player.Open(soundsFolder + System.IO.Path.DirectorySeparatorChar + data);
                                            mp3Player.Play();
                                        }
                                    }
                                    catch { }
                                }
                            }
                            break;
                        case "/query":
                            if (connection != null && data.Length > 0)
                            {
                                string nick = "";
                                string msg = "";

                                if (data.IndexOf(" ") > 0)
                                {
                                    //check if there is a message added
                                    nick = data.Substring(0, data.IndexOf(' '));
                                    msg = data.Substring(data.IndexOf(' ') + 1);
                                }
                                else
                                    nick = data;

                                if (!mainChannelBar.WindowExists(connection, nick, IceTabPage.WindowType.Query))
                                    AddWindow(connection, nick, IceTabPage.WindowType.Query);

                                mainChannelBar.SelectTab(GetWindow(connection, nick, IceTabPage.WindowType.Query));
                                serverTree.SelectTab(mainChannelBar.CurrentTab, false);

                                if (msg.Length > 0)
                                {
                                    // SendData(connection, "PRIVMSG " + nick + " :" + msg);

                                    if (msg.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(msg);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + nick + " :" + line);
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + nick + " :" + msg);
                                    } 

                                    string nmsg = GetMessageFormat("Self Private Message");
                                    nmsg = nmsg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName).Replace("$message", msg);

                                    CurrentWindow.TextWindow.AppendText(nmsg, "");
                                    CurrentWindow.LastMessageType = ServerMessageType.Message;
                                }
                            }
                            break;
                        case "/quit":
                            if (connection != null)
                            {
                                connection.AttemptReconnect = false;

                                if (data.Length > 0)
                                    SendData(connection, "QUIT :" + data);
                                else
                                    SendData(connection, "QUIT :" + ParseIdentifiers(connection, connection.ServerSetting.QuitMessage, ""));
                            }
                            break;
                        case "/aquit":
                        case "/quitall":
                            foreach (IRCConnection c in serverTree.ServerConnections.Values)
                            {
                                if (c.IsConnected)
                                {
                                    c.AttemptReconnect = false;

                                    if (data.Length > 0)
                                        SendData(c, "QUIT :" + data);
                                    else
                                        SendData(c, "QUIT :" + ParseIdentifiers(connection, c.ServerSetting.QuitMessage, ""));
                                }
                            }
                            break;
                        case "/redrawtree":
                            System.Diagnostics.Debug.WriteLine(mainChannelBar.CurrentTab.TabCaption);
                            this.serverTree.Invalidate();
                            break;
                        case "/run":
                            if (data.Length > 0)
                            {
                                try
                                {
                                    if (data.IndexOf("'") == -1)
                                        System.Diagnostics.Process.Start(data);
                                    else
                                    {
                                        string cmd = data.Substring(0, data.IndexOf("'"));
                                        string arg = data.Substring(data.IndexOf("'") + 1);
                                        System.Diagnostics.Process p = System.Diagnostics.Process.Start(cmd, arg);
                                    }
                                }
                                catch { }
                            }
                            break;
                        case "/say":
                            if (connection != null && data.Length > 0)
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                {                                    
                                    if (data.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(data);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + line);
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);
                                    } 

                                    string msg = GetMessageFormat("Self Channel Message");
                                    string nick = inputPanel.CurrentConnection.ServerSetting.CurrentNickName;

                                    msg = msg.Replace("$nick", nick).Replace("$channel", CurrentWindow.TabCaption);

                                    //assign $color to the nickname 
                                    if (msg.Contains("$color"))
                                    {
                                        User u = CurrentWindow.GetNick(nick);

                                        //get the nick color
                                        if (u.nickColor == -1)
                                        {
                                            if (IceChatColors.RandomizeNickColors == true)
                                            {
                                                int randColor = new Random().Next(0, 71);
                                                if (randColor == IceChatColors.NickListBackColor)
                                                    randColor = new Random().Next(0, 71);
                                                u.nickColor = randColor;
                                            }
                                            else
                                            {
                                                //get the correct nickname color for channel status
                                                for (int y = 0; y < u.Level.Length; y++)
                                                {
                                                    if (u.Level[y])
                                                    {
                                                        switch (connection.ServerSetting.StatusModes[0][y])
                                                        {
                                                            case 'q':
                                                            case 'y':
                                                                u.nickColor = IceChatColors.ChannelOwnerColor;
                                                                break;
                                                            case 'a':
                                                                u.nickColor = IceChatColors.ChannelAdminColor;
                                                                break;
                                                            case 'o':
                                                                u.nickColor = IceChatColors.ChannelOpColor;
                                                                break;
                                                            case 'h':
                                                                u.nickColor = IceChatColors.ChannelHalfOpColor;
                                                                break;
                                                            case 'v':
                                                                u.nickColor = IceChatColors.ChannelVoiceColor;
                                                                break;
                                                            default:
                                                                u.nickColor = IceChatColors.ChannelRegularColor;
                                                                break;
                                                        }
                                                        break;
                                                    }
                                                }

                                            }
                                            if (u.nickColor == -1)
                                                u.nickColor = IceChatColors.ChannelRegularColor;

                                        }
                                        msg = msg.Replace("$color", "\x0003" + u.nickColor.ToString("00"));
                                    }

                                    msg = msg.Replace("$status", CurrentWindow.GetNick(nick).ToString().Replace(nick, ""));
                                    msg = msg.Replace("$message", data);

                                    CurrentWindow.TextWindow.AppendText(msg, "");
                                    CurrentWindow.LastMessageType = ServerMessageType.Message;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                                {
                                    //SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);

                                    if (data.Length > maxMessageLength)
                                    {
                                        var lines = SplitLongMessage(data);
                                        foreach (var line in lines)
                                        {
                                            SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + line);
                                        }
                                    }
                                    else
                                    {
                                        SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);
                                    } 

                                    string msg = GetMessageFormat("Self Private Message");
                                    msg = msg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName).Replace("$message", data);

                                    CurrentWindow.TextWindow.AppendText(msg, "");
                                    CurrentWindow.LastMessageType = ServerMessageType.Message;
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                                {
                                    CurrentWindow.SendDCCData(data);

                                    string msg = GetMessageFormat("Self DCC Chat Message");
                                    msg = msg.Replace("$nick", inputPanel.CurrentConnection.ServerSetting.CurrentNickName).Replace("$message", data);
                                    CurrentWindow.TextWindow.AppendText(msg, "");
                                }
                                else if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                                {
                                    WindowMessage(connection, "Console", data, "", true);
                                }
                            }
                            break;
                        case "/joinserv":       //joinserv irc.server.name #channel
                            if (data.Length > 0 && data.IndexOf(' ') > 0)
                            {
                                //check if default nick name has been set
                                if (iceChatOptions.DefaultNick == null || iceChatOptions.DefaultNick.Length == 0)
                                {
                                    CurrentWindowMessage(connection, "\x000304No Default Nick Name Assigned. Go to Server Settings and set one under the Default Server Settings section.", "", false);
                                }
                                else
                                {
                                    ServerSetting s = new ServerSetting();
                                    //get the server name
                                    //if there is a port name. extract it
                                    string server = data.Substring(0, data.IndexOf(' '));
                                    string channel = data.Substring(data.IndexOf(' ') + 1);
                                    if (server.Contains(":"))
                                    {
                                        s.ServerName = server.Substring(0, server.IndexOf(':'));
                                        s.ServerPort = server.Substring(server.IndexOf(':') + 1);
                                        if (s.ServerPort.IndexOf(' ') > 0)
                                        {
                                            s.ServerPort = s.ServerPort.Substring(0, s.ServerPort.IndexOf(' '));
                                        }
                                        //check for + in front of port, SSL Connection
                                        if (s.ServerPort.StartsWith("+"))
                                        {
                                            s.ServerPort = s.ServerPort.Substring(1);
                                            s.UseSSL = true;
                                        }
                                    }
                                    else
                                    {
                                        s.ServerName = server;
                                        s.ServerPort = "6667";
                                    }

                                    s.NickName = iceChatOptions.DefaultNick;
                                    s.AltNickName = iceChatOptions.DefaultNick + "_";
                                    s.AwayNickName = iceChatOptions.DefaultNick + "[A]";
                                    s.FullName = iceChatOptions.DefaultFullName;
                                    s.QuitMessage = iceChatOptions.DefaultQuitMessage;
                                    s.IdentName = iceChatOptions.DefaultIdent;
                                    s.IAL = new Hashtable();
                                    s.AutoJoinChannels = new string[] { channel };
                                    s.AutoJoinEnable = true;
                                    Random r = new Random();
                                    s.ID = r.Next(50000, 99999);
                                    NewServerConnection(s);
                                }
                            }
                            break;
                        case "/scid": //scid <ServerNumber/NetworkName> /command [parameters]
                            if (data.Length > 0 && data.IndexOf(' ') > -1)
                            {
                                string[] param = data.Split(new char[] { ' ' }, 2);
                                int result;
                                if (Int32.TryParse(param[0], out result))
                                {
                                    //result is the server id
                                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                    {
                                        if (c.ServerSetting.ID == result)
                                        {
                                            ParseOutGoingCommand(c, param[1]);
                                        }
                                    }
                                }
                                else
                                {
                                    //check for a network name match
                                    foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                    {
                                        if (c.ServerSetting.NetworkName.Equals(param[0], StringComparison.OrdinalIgnoreCase))
                                        {
                                            ParseOutGoingCommand(c, param[1]);
                                        }
                                    }
                                }
                            }
                            break;
                        case "/server":
                            if (data.Length > 0)
                            {
                                //check if default nick name has been set
                                if (iceChatOptions.DefaultNick == null || iceChatOptions.DefaultNick.Length == 0)
                                {
                                    CurrentWindowMessage(connection, "\x000304No Default Nick Name Assigned. Go to Server Settings and set one under the Default Server Settings section.", "", false);
                                }
                                else if (data.StartsWith("id="))
                                {
                                    string serverID = data.Substring(3);
                                    foreach (ServerSetting s in serverTree.ServersCollection.listServers)
                                    {
                                        // have we connected already?                                        
                                        foreach (IRCConnection c in serverTree.ServerConnections.Values)
                                        {
                                            if (c.ServerSetting == s && s.ID.ToString() == serverID) 
                                            {
                                                if (!c.IsConnected)
                                                {
                                                    // reconnect it
                                                    c.ConnectSocket();
                                                }
                                                // we already have this connection, ignore it
                                                return;
                                            }
                                        }

                                        if (s.ID.ToString() == serverID)
                                        {
                                            NewServerConnection(s);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    ServerSetting s = new ServerSetting();
                                    s.NickName = "";
                                    // get the server name
                                    // if there is a port name. extract it

                                    // server [-6e] <server:port> [port] [password] [-i nick anick email name] [-j #channel pass]

                                    if (data.Contains(" "))
                                    {
                                        if (data.StartsWith("-"))
                                        {
                                            //parameters
                                            // [-46epoc] - poc not used
                                            string switches = data.Substring(0, data.IndexOf(' '));
                                            data = data.Substring(switches.Length + 1);
                                            if (switches.IndexOf('e') > -1)
                                            {
                                                //enable ssl
                                                s.UseSSL = true;
                                                s.SSLAcceptInvalidCertificate = true;
                                            }
                                            //6 is ipv6
                                            if (switches.IndexOf('6') > -1)
                                            {
                                                //enable ssl
                                                s.UseIPv6 = true;
                                            }
                                        }
                                    }
                                    //data is now w/o the starting switches
                                    if (data.Contains(" "))
                                    {
                                        s.ServerName = data.Substring(0, data.IndexOf(' '));
                                        string sp = data.Substring(data.IndexOf(' ') + 1);
                                        //server address [port] [password]
                                        if (sp.IndexOf(' ') > 0)
                                        {
                                            if (sp.StartsWith("-"))
                                            {
                                                // -j or -i or both
                                                string[] sections = sp.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string section in sections)
                                                {
                                                    string switches = section.Substring(0, section.IndexOf(' '));
                                                    sp = section.Substring(switches.Length + 1);

                                                    if (switches.IndexOf('j') > -1)
                                                    {
                                                        //auto join this channel
                                                        //could have a channel pass
                                                        s.AutoJoinChannels = new string[1];
                                                        s.AutoJoinChannels[0] = sp;
                                                        s.AutoJoinEnable = true;
                                                    }

                                                    if (switches.IndexOf('i') > -1)
                                                    {
                                                        //use this nick                                                        
                                                        s.NickName = sp;
                                                        s.AltNickName = sp + "_";
                                                        s.AwayNickName = sp + "[A]";
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                s.ServerPort = sp.Substring(0, sp.IndexOf(' '));
                                                if (s.ServerPort.StartsWith("+"))
                                                {
                                                    s.ServerPort = s.ServerPort.Substring(1);
                                                    s.UseSSL = true;
                                                    s.SSLAcceptInvalidCertificate = true;
                                                }
                                                s.Password = sp.Substring(sp.IndexOf(' ') + 1);
                                            }
                                        }
                                        else
                                        {
                                            //no space, check if value is a number or not                                            
                                            int result;
                                            if (int.TryParse(sp, out result))
                                            {
                                                s.ServerPort = sp;
                                            }
                                            else
                                            {
                                                //check for + in front of port, SSL Connection
                                                if (sp.StartsWith("+"))
                                                {
                                                    s.ServerPort = sp.Substring(1);
                                                    s.UseSSL = true;
                                                    s.SSLAcceptInvalidCertificate = true;
                                                }
                                                else
                                                {
                                                    s.ServerPort = "6667";
                                                    s.Password = sp;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        s.ServerName = data;
                                        s.ServerPort = "6667";
                                    }


                                    //check if server name has : or :+ port
                                    if (s.ServerName.IndexOf(":") > -1)
                                    {
                                        string server = s.ServerName.Substring(0, s.ServerName.IndexOf(':'));
                                        s.ServerPort = s.ServerName.Substring(s.ServerName.IndexOf(':') + 1);
                                        s.ServerName = server;

                                        if (s.ServerPort.StartsWith("+"))
                                        {
                                            s.ServerPort = s.ServerPort.Substring(1);
                                            s.UseSSL = true;
                                            s.SSLAcceptInvalidCertificate = true;
                                        }
                                    }


                                    //nick could be set above
                                    if (s.NickName.Length == 0)
                                    {
                                        s.NickName = iceChatOptions.DefaultNick;
                                        s.AltNickName = iceChatOptions.DefaultNick + "_";
                                        s.AwayNickName = iceChatOptions.DefaultNick + "[A]";
                                    }

                                    s.FullName = iceChatOptions.DefaultFullName;
                                    s.QuitMessage = iceChatOptions.DefaultQuitMessage;
                                    s.IdentName = iceChatOptions.DefaultIdent;
                                    s.IAL = new Hashtable();

                                    Random r = new Random();
                                    s.ID = r.Next(50000, 99999);

                                    NewServerConnection(s);
                                }
                            }
                            break;

                        case "/set":
                            // set an internal variable
                            // set -uNg <%var> [value]
                            // -u - unset in N amount of seconds
                            // -g for global

                            if (data.IndexOf(' ') > -1)
                            {
                                bool global = false;
                                string name = "";

                                if (data.StartsWith("-"))
                                {
                                    string switches = data.Substring(0, data.IndexOf(' '));
                                    data = data.Substring(switches.Length + 1);
                                    if (switches.Contains("g"))
                                        global = true;
                                    if (switches.Contains("u"))
                                    {
                                        // set a timed variable
                                        //need to get the numeric value
                                        string delay = switches.Substring(switches.IndexOf('u') + 1);
                                        if (delay.Length > 0)
                                        {
                                            int result = 0;
                                            int z = 1;
                                            bool number = false;
                                            //get the numeric value out of it
                                            while (z < delay.Length)
                                            {
                                                if (Int32.TryParse(delay.Substring(z, 1), out result))
                                                {
                                                    z++;
                                                    number = true;
                                                }
                                                else
                                                    break;
                                            }

                                            System.Diagnostics.Debug.WriteLine(switches + ":" + delay + ":" + delay.Substring(0, z) + ":" + z + ":" + number);

                                            if (connection != null && global == false)
                                            {
                                                name = data.Substring(0, data.IndexOf(' '));
                                                //add the /timer command globally
                                                connection.CreateTimer("unset", 1, 1, "/unset " + name);
                                            }
                                        }

                                    }
                                }
                                //needs to have a space  <%var value>
                                if (data.IndexOf(' ') > -1)
                                {
                                    name = data.Substring(0, data.IndexOf(' '));
                                    object val = data.Substring(data.IndexOf(' ') + 1);

                                    if (connection != null && global == false)
                                        connection.ServerSetting.Variables.AddVariable(name, val);
                                    else
                                    {
                                        //add it as a global variable
                                        _variables.AddVariable(name, val);
                                    }
                                }
                            }
                            break;
                        case "/unset":
                            //unset <%var>
                            if (data.Length > 0)
                            {
                                if (connection != null)
                                    connection.ServerSetting.Variables.RemoveVariable(data);
                                else
                                {
                                    //remove it as a global variable
                                    _variables.RemoveVariable(data);
                                }
                            }
                            break;

                        case "/tab":
                            if (data.Length > 0)
                            {
                                //activate a specific tab
                                if (data.ToLower().Equals("buddylist"))
                                    ((TabControl)buddyListTab.Parent).SelectedTab = buddyListTab;
                                else if (data.ToLower().Equals("serverlist") || data.ToLower().Equals("servertree"))
                                    ((TabControl)serverListTab.Parent).SelectedTab = serverListTab;
                                else if (data.ToLower().Equals("nicklist"))
                                    ((TabControl)nickListTab.Parent).SelectedTab = nickListTab;
                                else if (data.ToLower().Equals("channellist"))
                                    ((TabControl)channelListTab.Parent).SelectedTab = channelListTab;

                                FocusInputBox();
                            }
                            break;
                        case "/timers":
                            if (connection != null)
                            {
                                if (connection.IRCTimers.Count == 0)
                                    OnServerMessage(connection, "No Timers", "");
                                else
                                {
                                    foreach (IrcTimer timer in connection.IRCTimers)
                                        OnServerMessage(connection, "[ID=" + timer.TimerID + "] [Interval=" + timer.TimerInterval + "] [Reps=" + timer.TimerRepetitions + "] [Count=" + timer.TimerCounter + "] [Command=" + timer.TimerCommand + "]", "");
                                }
                            }
                            else
                            {
                                if (this._globalTimers.Count == 0)
                                    ParseOutGoingCommand(null, "/echo No Global Timers");
                                else
                                {
                                    foreach (IrcTimer timer in this._globalTimers)
                                        ParseOutGoingCommand(null, "/echo [Global ID=" + timer.TimerID + "] [Interval=" + timer.TimerInterval + "] [Reps=" + timer.TimerRepetitions + "] [Count=" + timer.TimerCounter + "] [Command=" + timer.TimerCommand + "]");
                                }
                            }
                            break;
                        case "/timer":
                            if (connection != null)
                            {
                                if (data.Length != 0)
                                {
                                    string[] param = data.Split(new char[] { ' ' }, 4);
                                    if (param.Length == 2)
                                    {
                                        //check for /timer ID off
                                        if (param[1].ToLower() == "off")
                                        {
                                            connection.DestroyTimer(param[0]);
                                            break;
                                        }
                                    }
                                    else if (param.Length == 4)
                                    {
                                        // param[0] = TimerID
                                        // param[1] = Repetitions
                                        // param[2] = Interval
                                        // param[3+] = Command
                                        if (param[0].StartsWith("-g"))
                                            this.CreateTimer(param[0], Convert.ToInt32(param[1]), Convert.ToDouble(param[2]), param[3]);
                                        else
                                            connection.CreateTimer(param[0], Convert.ToInt32(param[1]), Convert.ToDouble(param[2]), param[3]);
                                    }
                                    else
                                    {
                                        string msg = GetMessageFormat("User Error");
                                        msg = msg.Replace("$message", "/timer [ID] [REPS] [INTERVAL] [COMMAND]");
                                        CurrentWindowMessage(connection, msg, "", true);
                                    }
                                }
                                else
                                {
                                    string msg = GetMessageFormat("User Error");
                                    msg = msg.Replace("$message", "/timer [ID] [REPS] [INTERVAL] [COMMAND]");
                                    CurrentWindowMessage(connection, msg, "", true);
                                }
                            }
                            else
                            {
                                //add it to a global timer
                                if (data.Length != 0)
                                {
                                    string[] param = data.Split(new char[] { ' ' }, 4);
                                    if (param.Length == 2)
                                    {
                                        //check for /timer ID off
                                        if (param[1].ToLower() == "off")
                                        {
                                            this.DestroyTimer(param[0]);
                                            break;
                                        }
                                    }
                                    else if (param.Length == 4)
                                    {
                                        this.CreateTimer(param[0], Convert.ToInt32(param[1]), Convert.ToDouble(param[2]), param[3]);
                                    }
                                    else
                                    {
                                        string msg = GetMessageFormat("User Error");
                                        msg = msg.Replace("$message", "/timer [ID] [REPS] [INTERVAL] [COMMAND]");
                                        CurrentWindowMessage(null, msg, "", true);
                                    }
                                }
                                else
                                {
                                    string msg = GetMessageFormat("User Error");
                                    msg = msg.Replace("$message", "/timer [ID] [REPS] [INTERVAL] [COMMAND]");
                                    CurrentWindowMessage(null, msg, "", true);
                                }
                            }
                            break;
                        case "/topicbar":
                            if (connection != null)
                            {
                                if (data.Length > 0)
                                {
                                    if (data.IndexOf(' ') == -1)
                                    {
                                        if (CurrentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                                        {
                                            //topicbar show //topicbar hide for current channel
                                            if (data.ToLower() == "show" || data.ToLower() == "on")
                                                CurrentWindow.ShowTopicBar = true;
                                            if (data.ToLower() == "hide" || data.ToLower() == "off")
                                                CurrentWindow.ShowTopicBar = false;

                                            ChannelSetting cs = ChannelSettings.FindChannel(CurrentWindow.TabCaption, connection.ServerSetting.NetworkName);
                                            if (cs != null)
                                            {
                                                cs.HideTopicBar = !CurrentWindow.ShowTopicBar;
                                            }
                                            else
                                            {
                                                ChannelSetting cs1 = new ChannelSetting();
                                                cs1.HideTopicBar = !CurrentWindow.ShowTopicBar;
                                                cs1.ChannelName = CurrentWindow.TabCaption;
                                                cs1.NetworkName = connection.ServerSetting.NetworkName;
                                                ChannelSettings.AddChannel(cs1);
                                            }

                                            SaveChannelSettings();

                                        }
                                    }
                                    else
                                    {
                                        //topicbar #channel show
                                        //string[] words = data.Split(' ');

                                    }
                                }
                            }
                            break;
                        case "/topic":
                            if (connection != null)
                            {
                                if (data.Length == 0)
                                {
                                    if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                        SendData(connection, "TOPIC :" + CurrentWindow.TabCaption);
                                }
                                else
                                {
                                    //check if a channel name was passed                            
                                    string word = "";

                                    if (data.IndexOf(' ') > -1)
                                        word = data.Substring(0, data.IndexOf(' '));
                                    else
                                        word = data;

                                    if (Array.IndexOf(connection.ServerSetting.ChannelTypes, word[0]) != -1)
                                    {
                                        IceTabPage t = GetWindow(connection, word, IceTabPage.WindowType.Channel);
                                        if (t != null)
                                        {
                                            if (data.IndexOf(' ') > -1)
                                                SendData(connection, "TOPIC " + t.TabCaption + " :" + data.Substring(data.IndexOf(' ') + 1));
                                            else
                                                SendData(connection, "TOPIC :" + t.TabCaption);
                                        }
                                    }
                                    else
                                    {
                                        if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                                            SendData(connection, "TOPIC " + CurrentWindow.TabCaption + " :" + data);
                                    }
                                }
                            }
                            break;
                        case "/update":
                            checkForUpdate();
                            break;
                        case "/userinfo":
                            if (connection != null && data.Length > 0)
                            {
                                FormUserInfo fui = new FormUserInfo(connection);
                                //find the user
                                fui.NickName(data);
                                fui.Show(this);
                            }
                            break;
                        case "/version":
                            if (connection != null && data.Length > 0)
                            {
                                string msg = GetMessageFormat("Ctcp Send");
                                msg = msg.Replace("$nick", data); ;
                                msg = msg.Replace("$ctcp", "VERSION");
                                CurrentWindowMessage(connection, msg, "", true);
                                SendData(connection, "PRIVMSG " + data + " VERSION");
                            }
                            else
                                SendData(connection, "VERSION");
                            break;
                        case "/who":
                            if (connection != null && data.Length > 0)
                            {
                                SendData(connection, "WHO " + data);
                            }
                            break;
                        case "/whois":
                            if (connection != null && data.Length > 0)
                                SendData(connection, "WHOIS " + data);
                            break;
                        case "/aline":  //for adding lines to @windows
                            if (data.Length > 0 && data.IndexOf(" ") > -1)
                            {
                                string window = data.Substring(0, data.IndexOf(' '));
                                string msg = data.Substring(data.IndexOf(' ') + 1);
                                if (GetWindow(null, window, IceTabPage.WindowType.Window) == null)
                                    AddWindow(null, window, IceTabPage.WindowType.Window);

                                IceTabPage t = GetWindow(null, window, IceTabPage.WindowType.Window);
                                if (t != null)
                                {
                                    t.TextWindow.AppendText(msg, "");
                                    t.LastMessageType = ServerMessageType.Message;
                                }
                            }
                            break;
                        case "/window":
                            if (data.Length > 0)
                            {
                                if (data.StartsWith("@") && data.IndexOf(" ") == -1)
                                {
                                    if (GetWindow(null, data, IceTabPage.WindowType.Window) == null)
                                        AddWindow(null, data, IceTabPage.WindowType.Window);
                                    else
                                    {
                                        //switch to this window
                                        for (int i = 0; i < mainChannelBar.TabPages.Count; i++)
                                        {
                                            if (mainChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Window)
                                            {
                                                if (mainChannelBar.TabPages[i].TabCaption == data)
                                                {
                                                    mainChannelBar.SelectTab(mainChannelBar.TabPages[i]);
                                                    return;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            break;
                        case "/quote":
                        case "/raw":
                            if (connection != null && connection.IsConnected)
                                connection.SendData(data);
                            break;
                        default:
                            //parse the outgoing data
                            if (connection != null)
                                SendData(connection, command.Substring(1) + " " + data);
                            break;
                    }
                }
                else
                {
                    //sends a message to the channel
                    error = 1;
                    if (inputPanel.CurrentConnection != null && connection != null)
                    {
                        if (connection.IsConnected)
                        {
                            error = 2;
                            //check if the current window is a Channel/Query, etc
                            if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                            {

                                // SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);

                                if (data.Length > maxMessageLength)
                                {
                                    var lines = SplitLongMessage(data);
                                    foreach (var line in lines)
                                    {
                                        SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + line);
                                    }
                                }
                                else
                                {
                                    SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);
                                } 


                                //check if we got kicked out of the channel or not, and the window is still open
                                if (CurrentWindow.IsFullyJoined)
                                {
                                    error = 3;
                                    string msg = GetMessageFormat("Self Channel Message");
                                    string nick = connection.ServerSetting.CurrentNickName;
                                    msg = msg.Replace("$nick", nick).Replace("$channel", CurrentWindow.TabCaption);
                                    error = 4;

                                    //assign $color to the nickname 
                                    if (msg.Contains("$color"))
                                    {
                                        error = 5;
                                        User u = CurrentWindow.GetNick(nick);
                                        //User u = t.GetNick(connection.ServerSetting.CurrentNickName);

                                        System.Diagnostics.Debug.WriteLine(u.NickName + ":" + u.nickColor);

                                        //get the nick color
                                        if (u != null && u.nickColor == -1)
                                        {
                                            error = 6;
                                            if (IceChatColors.RandomizeNickColors == true)
                                            {
                                                int randColor = new Random().Next(0, 71);
                                                if (randColor == IceChatColors.NickListBackColor)
                                                    randColor = new Random().Next(0, 71);
                                                u.nickColor = randColor;
                                                error = 7;
                                            }
                                            else
                                            {
                                                //get the correct nickname color for channel status
                                                error = 8;
                                                for (int y = 0; y < u.Level.Length; y++)
                                                {
                                                    if (u.Level[y])
                                                    {
                                                        System.Diagnostics.Debug.WriteLine(u.Level[y]);
                                                        switch (connection.ServerSetting.StatusModes[0][y])
                                                        {
                                                            case 'q':
                                                            case 'y':
                                                                u.nickColor = IceChatColors.ChannelOwnerColor;
                                                                break;
                                                            case 'a':
                                                                u.nickColor = IceChatColors.ChannelAdminColor;
                                                                break;
                                                            case 'o':
                                                                u.nickColor = IceChatColors.ChannelOpColor;
                                                                break;
                                                            case 'h':
                                                                u.nickColor = IceChatColors.ChannelHalfOpColor;
                                                                break;
                                                            case 'v':
                                                                u.nickColor = IceChatColors.ChannelVoiceColor;
                                                                break;
                                                            default:
                                                                u.nickColor = IceChatColors.ChannelRegularColor;
                                                                break;
                                                        }
                                                        break;
                                                    }
                                                }
                                                error = 9;
                                            }

                                            if (u.nickColor == -1)
                                                u.nickColor = IceChatColors.ChannelRegularColor;

                                            error = 10;

                                        }

                                        msg = msg.Replace("$color", "\x0003" + u.nickColor.ToString("00"));

                                        error = 11;
                                    }
                                    else
                                        msg = msg.Replace("$color", "");


                                    error = 12; //this errors, losing a nickname for some reason!!

                                    if (CurrentWindow.GetNick(nick) != null)
                                        msg = msg.Replace("$status", CurrentWindow.GetNick(nick).ToString().Replace(nick, ""));
                                    else
                                        msg = msg.Replace("$status", "");

                                    error = 13;
                                    msg = msg.Replace("$message", data);
                                    error = 14;
                                    CurrentWindow.TextWindow.AppendText(msg, "");
                                    CurrentWindow.LastMessageType = ServerMessageType.Message;
                                    error = 15;
                                }
                            }
                            else if (CurrentWindowStyle == IceTabPage.WindowType.Query)
                            {
                                // SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);

                                if (data.Length > maxMessageLength)
                                {
                                    var lines = SplitLongMessage(data);
                                    foreach (var line in lines)
                                    {
                                        SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + line);
                                    }
                                }
                                else
                                {
                                    SendData(connection, "PRIVMSG " + CurrentWindow.TabCaption + " :" + data);
                                } 

                                string msg = GetMessageFormat("Self Private Message");
                                msg = msg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$message", data);

                                CurrentWindow.TextWindow.AppendText(msg, "");
                                CurrentWindow.LastMessageType = ServerMessageType.Message;
                            }
                            else if (CurrentWindowStyle == IceTabPage.WindowType.DCCChat)
                            {
                                CurrentWindow.SendDCCData(data);

                                string msg = GetMessageFormat("Self DCC Chat Message");
                                msg = msg.Replace("$nick", connection.ServerSetting.CurrentNickName).Replace("$message", data);

                                CurrentWindow.TextWindow.AppendText(msg, "");
                            }
                            else if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                            {
                                WindowMessage(connection, "Console", "\x000304" + data, "", true);
                            }
                        }
                        else
                        {
                            WindowMessage(connection, "Console", "\x000304Error: Not Connected", "", true);
                            WindowMessage(connection, "Console", "\x000304" + data, "", true);
                        }
                    }
                    else
                    {
                        if (CurrentWindowStyle == IceTabPage.WindowType.Window)
                            CurrentWindow.TextWindow.AppendText("\x000301" + data, "");
                        else
                            WindowMessage(null, "Console", "\x000304" + data, "", true);
                    }
                }
            }
            catch (NotSupportedException nse)
            {
                System.Diagnostics.Debug.WriteLine("NS Error:" + nse.StackTrace);
                System.Diagnostics.Debug.WriteLine("NS Error:" + nse.InnerException);
                System.Diagnostics.Debug.WriteLine("NS Error:" + nse.Source);
                System.Diagnostics.Debug.WriteLine("NS Error:" + nse.Message);
            }
            catch (Exception e)
            {
                WriteErrorFile(connection, "ParseOutGoingCommand:" + CurrentWindowStyle.ToString() + ":" + error + ":" + commandData, e);
            }
        }



        private string ParseIdentifier(IRCConnection connection, string data)
        {
            //match all words starting with a $
            try
            {
                string identMatch = "\\$\\b[a-zA-Z_0-9.]+\\b";
                Regex ParseIdent = new Regex(identMatch);
                Match m = ParseIdent.Match(data);

                while (m.Success)
                {
                    switch (m.Value)
                    {
                        case "$away":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, "$" + connection.ServerSetting.Away.ToString().ToLower());
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$me":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.CurrentNickName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$cme":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.NickName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$altnick":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.AltNickName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$ident":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.IdentName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$host":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.LocalHost);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$fullhost":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.CurrentNickName + "!" + connection.ServerSetting.LocalHost);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$fullname":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.FullName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$ip":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.LocalIP.ToString());
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$network":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.NetworkName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$port":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.ServerPort);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$encoding":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.Encoding);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$quitmessage":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.QuitMessage);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$chantypes":
                            break;
                        case "$chanmodes":
                            break;
                        case "$servermode":
                            data = ReplaceFirst(data, m.Value, string.Empty);
                            //connection.ServerSetting.ChannelModeParam
                            break;
                        case "$currentserverid":
                            if (inputPanel.CurrentConnection != null)
                                data = ReplaceFirst(data, m.Value, inputPanel.CurrentConnection.ServerSetting.ID.ToString());
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$serverid":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.ID.ToString());
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$serverip":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.ServerIP);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$serversetting":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.ServerName);
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$server":
                            if (connection != null)
                            {
                                if (connection.ServerSetting.RealServerName.Length > 0)
                                    data = ReplaceFirst(data, m.Value, connection.ServerSetting.RealServerName);
                                else
                                    data = ReplaceFirst(data, m.Value, connection.ServerSetting.ServerName);
                            }
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$online":
                            if (connection != null)
                            {
                                //check the datediff
                                TimeSpan online = DateTime.Now.Subtract(connection.ServerSetting.ConnectedTime);
                                data = ReplaceFirst(data, m.Value, GetDuration((int)online.TotalSeconds));
                            }
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;
                        case "$localip":
                            if (connection != null)
                                data = ReplaceFirst(data, m.Value, connection.ServerSetting.LocalIP.ToString());
                            else
                                data = ReplaceFirst(data, m.Value, "$null");
                            break;

                        //identifiers that do not require a connection                                
                        case "$theme":
                            data = ReplaceFirst(data, m.Value, iceChatOptions.CurrentTheme);
                            break;
                        case "$colors":
                            data = ReplaceFirst(data, m.Value, colorsFile);
                            break;
                        case "$appdata":
                            data = ReplaceFirst(data, m.Value, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).ToString());
                            break;
                        case "$ossp":
                            data = ReplaceFirst(data, m.Value, Environment.OSVersion.ServicePack.ToString());
                            break;
                        case "$osbuild":
                            data = ReplaceFirst(data, m.Value, Environment.OSVersion.Version.Build.ToString());
                            break;
                        case "$osplatform":
                            data = ReplaceFirst(data, m.Value, Environment.OSVersion.Platform.ToString());
                            break;
                        case "$osbits":
                            //8 on 64bit -- AMD64
                            string arch = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                            switch (arch)
                            {
                                case "x86":
                                    string arch2 = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432");
                                    if (arch2 == "AMD64")
                                        data = ReplaceFirst(data, m.Value, "64bit");
                                    else
                                        data = ReplaceFirst(data, m.Value, "32bit");
                                    break;
                                case "AMD64":
                                case "IA64":
                                    data = ReplaceFirst(data, m.Value, "64bit");
                                    break;

                            }
                            break;
                        case "$os":
                            data = ReplaceFirst(data, m.Value, GetOperatingSystemName());
                            break;
                        case "$time":
                            data = ReplaceFirst(data, m.Value, DateTime.Now.ToShortTimeString());
                            break;
                        case "$date":
                            data = ReplaceFirst(data, m.Value, DateTime.Now.ToShortDateString());
                            break;
                        case "$icepath":
                        case "$icechatexedir":
                            data = ReplaceFirst(data, m.Value, Directory.GetCurrentDirectory());
                            break;
                        case "$scriptdir":
                            data = ReplaceFirst(data, m.Value, scriptsFolder + Path.DirectorySeparatorChar);
                            break;
                        case "$plugindir":
                            data = ReplaceFirst(data, m.Value, pluginsFolder);
                            break;
                        case "$aliasfile":
                            data = ReplaceFirst(data, m.Value, aliasesFile);
                            break;
                        case "$serverfile":
                            data = ReplaceFirst(data, m.Value, serversFile);
                            break;
                        case "$popupfile":
                            data = ReplaceFirst(data, m.Value, popupsFile);
                            break;
                        case "$icechatver":
                            data = ReplaceFirst(data, m.Value, VersionID);
                            break;
                        case "$version":
                            data = ReplaceFirst(data, m.Value, ProgramID + " " + VersionID);
                            break;
                        case "$icechatdir":
                            data = ReplaceFirst(data, m.Value, currentFolder);
                            break;
                        case "$icechathandle":
                            data = ReplaceFirst(data, m.Value, this.Handle.ToString());
                            break;
                        case "$icechat":
                            data = ReplaceFirst(data, m.Value, ProgramID + " " + VersionID + " http://www.icechat.net");
                            break;
                        case "$logdir":
                            data = ReplaceFirst(data, m.Value, logsFolder);
                            break;
                        case "$randquit":
                            Random rand = new Random();
                            int rq = rand.Next(0, QuitMessages.RandomQuitMessages.Length);
                            data = ReplaceFirst(data, m.Value, QuitMessages.RandomQuitMessages[rq]);
                            break;
                        case "$randcolor":
                            Random randcolor = new Random();
                            int rc = randcolor.Next(0, (IrcColor.colors.Length - 1));
                            data = ReplaceFirst(data, m.Value, rc.ToString());
                            break;
                        case "$tickcount":
                        case "$ticks":
                            data = ReplaceFirst(data, m.Value, System.Environment.TickCount.ToString());
                            break;
                        case "$totalwindows":
                            data = ReplaceFirst(data, m.Value, mainChannelBar.TabCount.ToString());
                            break;
                        case "$totalscreens":
                            data = ReplaceFirst(data, m.Value, System.Windows.Forms.Screen.AllScreens.Length.ToString());
                            break;
                        case "$currentwindow":
                        case "$active":
                            data = ReplaceFirst(data, m.Value, CurrentWindow.TabCaption);
                            break;
                        case "$chanlogdir":
                            data = ReplaceFirst(data, m.Value, CurrentWindow.TextWindow.LogFileLocation);
                            break;
                        case "$totallines":
                            if (CurrentWindowStyle == IceTabPage.WindowType.Console)
                            {
                                data = ReplaceFirst(data, m.Value, ((TextWindow)mainChannelBar.GetTabPage("Console").ConsoleTab.SelectedTab.Controls[0]).TotalLines.ToString());
                            }
                            else
                            {
                                if (CurrentWindowStyle == IceTabPage.WindowType.Channel || CurrentWindowStyle == IceTabPage.WindowType.DCCChat || CurrentWindowStyle == IceTabPage.WindowType.Query || CurrentWindowStyle == IceTabPage.WindowType.Window)
                                {
                                    data = ReplaceFirst(data, m.Value, CurrentWindow.TextWindow.TotalLines.ToString());
                                }
                            }
                            break;
                        case "$framework":
                            #if USE_NET_45
                                data = ReplaceFirst(data, m.Value, System.Environment.Version.ToString()) + " +4.5";
                            #else
                                data = ReplaceFirst(data, m.Value, System.Environment.Version.ToString());
                            #endif
                            break;
                        case "$totalplugins":
                            data = ReplaceFirst(data, m.Value, loadedPlugins.Count.ToString());
                            break;
                        case "$plugins":
                            string plugins = "";
                            foreach (Plugin p in loadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                        plugins += ipc.plugin.Name + " : ";
                                }
                            }
                            if (plugins.EndsWith(" : "))
                                plugins = plugins.Substring(0, plugins.Length - 3);
                            data = ReplaceFirst(data, m.Value, plugins);
                            break;
                        case "$uptime2":
                            int systemUpTime = System.Environment.TickCount / 1000;
                            TimeSpan ts = TimeSpan.FromSeconds(systemUpTime);
                            data = ReplaceFirst(data, m.Value, GetDuration(ts.TotalSeconds));
                            break;
                        case "$uptime":
                            System.Diagnostics.PerformanceCounter pc = new System.Diagnostics.PerformanceCounter("System", "System Up Time");
                            pc.NextValue();
                            TimeSpan ts2 = TimeSpan.FromSeconds(pc.NextValue());
                            data = ReplaceFirst(data, m.Value, GetDuration(ts2.TotalSeconds));
                            break;
                        case "$mono":
                            if (StaticMethods.IsRunningOnMono())
                                data = ReplaceFirst(data, m.Value, (string)typeof(object).Assembly.GetType("Mono.Runtime").InvokeMember("GetDisplayName", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.ExactBinding, null, null, null));
                            else
                                data = ReplaceFirst(data, m.Value, "Mono.Runtime not detected");
                            break;
                    }
                    m = m.NextMatch();
                }
            }
            catch (Exception e)
            {
                WriteErrorFile(connection, "ParseIdentifier:" + data, e);
            }
            return data;
        }


        private string ParseIdentifierValue(string data, string dataPassed)
        {
            //split up the data into words
            string[] parsedData = data.Split(' ');

            //the data that was passed for parsing identifiers
            string[] passedData = dataPassed.Split(' ');

            //will hold the updates message/data after identifiers are parsed
            string[] changedData = data.Split(' ');

            int count = -1;

            try
            {
                //search for identifiers that are numbers
                //used for replacing values passed to the function
                foreach (string word in parsedData)
                {
                    count++;

                    if (word.StartsWith("//") && count == 0)
                        changedData[count] = word.Substring(1);

                    //parse out identifiers (start with a $)
                    if (word.StartsWith("$"))
                    {
                        switch (word)
                        {
                            case "$+":
                                break;

                            default:
                                //search for identifiers that are numbers
                                //used for replacing values passed to the function
                                int result;
                                int z = 1;

                                while (z < word.Length)
                                {
                                    if (Int32.TryParse(word.Substring(z, 1), out result))
                                        z++;
                                    else
                                        break;
                                }

                                //check for - after numbered identifier
                                if (z > 1)
                                {
                                    //get the numbered identifier value
                                    int identVal = Int32.Parse(word.Substring(1, z - 1));

                                    if (identVal <= passedData.Length)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(identVal + ":" +  passedData[identVal - 1]);
                                        //System.Diagnostics.Debug.WriteLine(z + ":" + word.Length);
                                        //System.Diagnostics.Debug.WriteLine(word.Substring(z,1));
                                        if (word.Length > z)
                                            if (word.Substring(z, 1) == "-")
                                            {
                                                //System.Diagnostics.Debug.WriteLine("change - " + identVal + ":" + passedData.Length);
                                                changedData[count] = String.Join(" ", passedData, identVal - 1, passedData.Length - identVal + 1);
                                                continue;
                                            }
                                        //System.Diagnostics.Debug.WriteLine("change normal ");
                                        changedData[count] = passedData[identVal - 1];
                                    }
                                    else
                                        changedData[count] = "";
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteErrorFile(null, "ParseIdentifierValue" + data + ":" + dataPassed, e);
            }

            return String.Join(" ", changedData);
        }
        /// <summary>
        /// Parse out $identifiers for outgoing commands
        /// </summary>
        /// <param name="connection">Which Connection it is for</param>
        /// <param name="data">The data to be parsed</param>
        /// <returns></returns>
        private string ParseIdentifiers(IRCConnection connection, string data, string dataPassed)
        {
            string[] changedData = null;

            try
            {
                //parse the initial identifiers
                data = ParseIdentifier(connection, data);

                //parse out the $1,$2.. identifiers
                data = ParseIdentifierValue(data, dataPassed);

                //$+ is a joiner identifier, great for joining 2 words together
                data = data.Replace(" $+ ", string.Empty);

                //parse out the current channel #
                if (CurrentWindowStyle == IceTabPage.WindowType.Channel)
                {
                    data = data.Replace(" # ", " " + CurrentWindow.TabCaption + " ");
                }

                //split up the data into words
                string[] parsedData = data.Split(' ');

                //the data that was passed for parsing identifiers
                string[] passedData = dataPassed.Split(' ');

                //will hold the updates message/data after identifiers are parsed
                changedData = data.Split(' ');

                int count = -1;
                string extra = "";
                bool askExtra = false;
                bool askSecure = false;

                foreach (string word in parsedData)
                {
                    count++;

                    if (word.StartsWith("//") && count == 0)
                        changedData[count] = word.Substring(1);

                    if (askExtra)
                    {
                        //continueing a $?= 
                        extra += " " + word;
                        changedData[count] = null;
                        if (extra[extra.Length - 1] == extra[0])
                        {
                            askExtra = false;
                            //ask the question
                            InputBoxDialog i = new InputBoxDialog();
                            i.PasswordChar = askSecure;
                            i.FormCaption = "Enter Value";
                            i.FormPrompt = extra.Substring(1, extra.Length - 2);

                            i.ShowDialog();
                            if (i.InputResponse.Length > 0)
                                changedData[count] = i.InputResponse;
                            i.Dispose();
                        }
                    }

                    //parse out identifiers (start with a $)
                    if (word.StartsWith("$"))
                    {
                        switch (word)
                        {

                            default:
                                int result;
                                if (word.StartsWith("$?=") && word.Length > 5)
                                {
                                    //check for 2 quotes (single or double)
                                    string ask = word.Substring(3);
                                    //check what kind of a quote it is
                                    char quote = ask[0];
                                    if (quote == ask[ask.Length - 1])
                                    {
                                        //ask the question
                                        extra = ask;
                                        InputBoxDialog i = new InputBoxDialog();
                                        i.FormCaption = "Enter Value";
                                        i.FormPrompt = extra.Substring(1, extra.Length - 2);

                                        i.ShowDialog();
                                        if (i.InputResponse.Length > 0)
                                            changedData[count] = i.InputResponse;
                                        else
                                            changedData[count] = null;
                                        i.Dispose();
                                    }
                                    else
                                    {
                                        //go to the next word until we find a quote at the end
                                        extra = ask;
                                        askExtra = true;
                                        changedData[count] = null;
                                    }
                                }

                                //check for $?*="" // password char
                                if (word.StartsWith("$?*=") && word.Length > 6)
                                {
                                    //check for 2 quotes (single or double)
                                    string ask = word.Substring(4);
                                    //check what kind of a quote it is
                                    char quote = ask[0];
                                    if (quote == ask[ask.Length - 1])
                                    {
                                        //ask the question
                                        extra = ask;
                                        InputBoxDialog i = new InputBoxDialog();
                                        i.PasswordChar = true;
                                        i.FormCaption = "Enter Value";
                                        i.FormPrompt = extra.Substring(1, extra.Length - 2);

                                        i.ShowDialog();
                                        if (i.InputResponse.Length > 0)
                                            changedData[count] = i.InputResponse;
                                        else
                                            changedData[count] = null;
                                        i.Dispose();
                                    }
                                    else
                                    {
                                        //go to the next word until we find a quote at the end
                                        extra = ask;
                                        askExtra = true;
                                        askSecure = true;
                                        changedData[count] = null;
                                    }
                                }


                                if (word.StartsWith("$md5(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    string input = ReturnBracketValue(word);
                                    changedData[count] = MD5(input);
                                }

                                if (word.StartsWith("$net(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    string input = ReturnBracketValue(word);
                                    try
                                    {
                                        // //echo $net(System.Environment.CurrentDirectory)                                        
                                        string[] thevalue = input.Split('.');
                                        if (thevalue.Length > 1)
                                        {
                                            string Class = String.Join(".", thevalue, 0, thevalue.Length - 1);
                                            string value = input.Substring(Class.Length + 1);
                                            //System.Diagnostics.Debug.WriteLine(Class + "::" + value);
                                            if (Class.Length > 0)
                                            {
                                                //Type.GetType(string for the type).GetProperty(n ame of the property).GetValue(null)
                                                Type t = Type.GetType(Class);

                                                PropertyInfo info = t.GetProperty(value);
                                                if (info != null)
                                                {
                                                    //System.Diagnostics.Debug.WriteLine("name=" +info.Name + ":" + info.GetValue(t, null));
                                                    changedData[count] = info.GetValue(t, null).ToString();
                                                }
                                                else
                                                {
                                                    //System.Diagnostics.Debug.WriteLine("null info");
                                                    changedData[count] = "$null";
                                                }

                                                //System.Diagnostics.Debug.WriteLine(t.ToString());
                                            }
                                            else
                                                changedData[count] = "$null";
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        changedData[count] = "$null";
                                    }
                                }

                                if (word.StartsWith("$rand(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    string input = ReturnBracketValue(word);
                                    //look for a comma (,)
                                    if (input.Split(',').Length == 2)
                                    {
                                        string lownum = input.Split(',')[0];
                                        string hinum = input.Split(',')[1];

                                        int lowNum, hiNum;
                                        if (Int32.TryParse(lownum, out lowNum) && Int32.TryParse(hinum, out hiNum))
                                        {
                                            //valid numbers
                                            Random r = new Random();
                                            int randNumber = r.Next(lowNum, hiNum);

                                            changedData[count] = randNumber.ToString();
                                        }
                                        else
                                            changedData[count] = "$null";
                                        Variable v = new Variable();


                                    }
                                    else if (input.IndexOf(',') == -1)
                                    {
                                        //make it a value from 1 - value
                                        int hiNum;
                                        if (Int32.TryParse(input, out hiNum))
                                        {
                                            //valid number
                                            Random r = new Random();
                                            int randNumber = r.Next(1, hiNum);

                                            changedData[count] = randNumber.ToString();
                                        }
                                        else
                                            changedData[count] = "$null";

                                    }
                                    else
                                        changedData[count] = "$null";
                                }

                                if (word.StartsWith("$read(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    string file = ReturnBracketValue(word);
                                    //check if we have passed a path or just a filename
                                    if (file.IndexOf(System.IO.Path.DirectorySeparatorChar) > -1)
                                    {
                                        //its a full folder
                                        if (File.Exists(file))
                                        {
                                            //count the number of lines in the file                                            
                                            //load the file in and read a random line from it
                                            string[] lines = File.ReadAllLines(file);
                                            if (lines.Length > 0)
                                            {
                                                //pick a random line
                                                Random r = new Random();
                                                int line = r.Next(0, lines.Length - 1);
                                                changedData[count] = lines[line];
                                            }
                                            else
                                                changedData[count] = "$null";

                                        }
                                        else
                                        {
                                            changedData[count] = "$null";
                                        }
                                    }
                                    else
                                    {
                                        //just check in the Scripts Folder
                                        if (File.Exists(scriptsFolder + System.IO.Path.DirectorySeparatorChar + file))
                                        {
                                            //load the file in and read a random line from it
                                            string[] lines = File.ReadAllLines(scriptsFolder + System.IO.Path.DirectorySeparatorChar + file);
                                            if (lines.Length > 0)
                                            {
                                                //pick a random line
                                                Random r = new Random();
                                                int line = r.Next(0, lines.Length - 1);
                                                changedData[count] = lines[line];
                                            }
                                            else
                                                changedData[count] = "$null";
                                        }
                                        else
                                        {
                                            changedData[count] = "$null";
                                        }
                                    }
                                }

                                if (word.StartsWith("$var(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    //get the value between and after the brackets
                                    string variable = ReturnBracketValue(word);
                                    string prop = ReturnPropertyValue(word);

                                    System.Diagnostics.Debug.WriteLine(variable);
                                    //check if we have a connection or not
                                    if (connection == null)
                                    {
                                        changedData[count] = _variables.ReturnValue(variable).ToString();

                                    }
                                }

                                if (word.StartsWith("$plugin(") && word.IndexOf(')') > word.IndexOf('('))
                                {
                                    //get the plugin information
                                    string pluginid = ReturnBracketValue(word);
                                    string prop = ReturnPropertyValue(word);

                                    //tryparse
                                    if (Int32.TryParse(pluginid, out result))
                                    {
                                        for (int i = 0; i < loadedPlugins.Count; i++)
                                        {
                                            if (i == result)
                                            {
                                                IPluginIceChat ipc = ((IceChatPlugin)loadedPlugins[i]).plugin;

                                                switch (prop.ToLower())
                                                {
                                                    case "id":
                                                        changedData[count] = i.ToString();
                                                        break;
                                                    case "name":
                                                        changedData[count] = ipc.Name;
                                                        break;
                                                    case "version":
                                                        changedData[count] = ipc.Version;
                                                        break;
                                                    case "author":
                                                        changedData[count] = ipc.Author;
                                                        break;
                                                    case "enabled":
                                                        changedData[count] = ipc.Enabled.ToString();
                                                        break;
                                                    case "filename":
                                                        changedData[count] = ipc.FileName;
                                                        break;
                                                    default:
                                                        changedData[count] = ipc.Name;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //go by plugin filename, not number
                                        for (int i = 0; i < loadedPlugins.Count; i++)
                                        {
                                            if (((IceChatPlugin)loadedPlugins[i]).plugin.FileName.ToLower() == pluginid.ToLower())
                                            {
                                                IPluginIceChat ipc = ((IceChatPlugin)loadedPlugins[i]).plugin;

                                                switch (prop.ToLower())
                                                {
                                                    case "id":
                                                        changedData[count] = i.ToString();
                                                        break;
                                                    case "name":
                                                        changedData[count] = ipc.Name;
                                                        break;
                                                    case "version":
                                                        changedData[count] = ipc.Version;
                                                        break;
                                                    case "author":
                                                        changedData[count] = ipc.Author;
                                                        break;
                                                    case "enabled":
                                                        changedData[count] = ipc.Enabled.ToString();
                                                        break;
                                                    case "filename":
                                                        changedData[count] = ipc.FileName;
                                                        break;
                                                    default:
                                                        changedData[count] = ipc.Name;
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }


                                if (connection != null)
                                {
                                    if (word.StartsWith("$ial(") && word.IndexOf(')') > word.IndexOf('('))
                                    {
                                        string nick = ReturnBracketValue(word);
                                        string prop = ReturnPropertyValue(word);

                                        InternalAddressList ial = (InternalAddressList)connection.ServerSetting.IAL[nick];
                                        if (ial != null)
                                        {
                                            if (prop.Length == 0)
                                                changedData[count] = ial.Nick;
                                            else
                                            {
                                                switch (prop.ToLower())
                                                {
                                                    case "nick":
                                                        changedData[count] = ial.Nick;
                                                        break;
                                                    case "host":
                                                        changedData[count] = ial.Host;
                                                        break;
                                                    case "account":
                                                        changedData[count] = ial.Account;
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                            changedData[count] = "$null";
                                    }

                                    if (word.StartsWith("$nick(") && word.IndexOf(')') > word.IndexOf('('))
                                    {
                                        //get the value between and after the brackets
                                        string values = ReturnBracketValue(word);
                                        if (values.Split(',').Length == 2)
                                        {
                                            string channel = values.Split(',')[0];
                                            string nickvalue = values.Split(',')[1];

                                            string prop = ReturnPropertyValue(word);

                                            // $nick(#,N)     
                                            //find then channel
                                            IceTabPage t = GetWindow(connection, channel, IceTabPage.WindowType.Channel);
                                            if (t != null)
                                            {
                                                User u = null;
                                                if (Int32.TryParse(nickvalue, out result))
                                                {
                                                    if (Convert.ToInt32(nickvalue) == 0)
                                                        changedData[count] = t.Nicks.Count.ToString();
                                                    else
                                                        u = t.GetNick(Convert.ToInt32(nickvalue));
                                                }
                                                else
                                                {
                                                    u = t.GetNick(nickvalue);
                                                }

                                                if (prop.Length == 0 && u != null)
                                                {
                                                    changedData[count] = u.NickName;
                                                }
                                                else if (u != null)
                                                {
                                                    //$nick(#channel,1).op , .voice, .halfop, .admin,.owner. 
                                                    //.mode, .host, .nick, .ident
                                                    InternalAddressList ial = (InternalAddressList)connection.ServerSetting.IAL[u.NickName];
                                                    switch (prop.ToLower())
                                                    {
                                                        case "host":
                                                            if (ial != null && ial.Host != null && ial.Host.Length > 0)
                                                                changedData[count] = ial.Host.Substring(ial.Host.IndexOf('@') + 1);
                                                            break;
                                                        case "ident":
                                                            if (ial != null && ial.Host != null && ial.Host.Length > 0)
                                                                changedData[count] = ial.Host.Substring(0, ial.Host.IndexOf('@'));
                                                            break;
                                                        case "account":
                                                            if (ial != null && ial.Account != null && ial.Account.Length > 0)
                                                                changedData[count] = ial.Account;
                                                            break;
                                                        case "nick":
                                                            changedData[count] = u.NickName;
                                                            break;
                                                        case "mode":
                                                            changedData[count] = u.ToString().Replace(u.NickName, "");
                                                            break;
                                                        case "op":
                                                            for (int i = 0; i < u.Level.Length; i++)
                                                            {
                                                                if (connection.ServerSetting.StatusModes[0][i] == 'o')
                                                                {
                                                                    if (u.Level[i] == true)
                                                                        changedData[count] = "$true";
                                                                    else
                                                                        changedData[count] = "$false";
                                                                }
                                                            }
                                                            break;
                                                        case "halfop":
                                                            for (int i = 0; i < u.Level.Length; i++)
                                                            {
                                                                if (connection.ServerSetting.StatusModes[0][i] == 'h')
                                                                {
                                                                    if (u.Level[i] == true)
                                                                        changedData[count] = "$true";
                                                                    else
                                                                        changedData[count] = "$false";
                                                                }
                                                            }
                                                            break;
                                                        case "voice":
                                                            for (int i = 0; i < u.Level.Length; i++)
                                                            {
                                                                if (connection.ServerSetting.StatusModes[0][i] == 'v')
                                                                {
                                                                    if (u.Level[i] == true)
                                                                        changedData[count] = "$true";
                                                                    else
                                                                        changedData[count] = "$false";
                                                                }
                                                            }
                                                            break;
                                                    }
                                                    ial = null;
                                                }
                                            }
                                        }
                                    }

                                    if (word.StartsWith("$chan(") && word.IndexOf(')') > word.IndexOf('('))
                                    {
                                        //get the value between and after the brackets
                                        string channel = ReturnBracketValue(word);
                                        string prop = ReturnPropertyValue(word);

                                        //find then channel
                                        IceTabPage t = GetWindow(connection, channel, IceTabPage.WindowType.Channel);
                                        if (t != null)
                                        {
                                            if (prop.Length == 0)
                                            {
                                                //replace with channel name
                                                changedData[count] = t.TabCaption;
                                            }
                                            else
                                            {
                                                switch (prop.ToLower())
                                                {
                                                    case "mode":
                                                        changedData[count] = t.ChannelModes;
                                                        break;
                                                    case "count":
                                                        changedData[count] = t.Nicks.Count.ToString();
                                                        break;
                                                    case "nicks":
                                                        //return all the nicks seperated by a space
                                                        string nicks = "";
                                                        foreach (string n in t.Nicks.Keys)
                                                            nicks += n + " ";
                                                        changedData[count] = nicks.Trim();
                                                        break;
                                                    case "log":
                                                        changedData[count] = t.TextWindow.LogFileName;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    if (word.StartsWith("$timer(") && word.IndexOf(')') > word.IndexOf('('))
                                    {
                                        //get the value between and after the brackets
                                        string timerid = ReturnBracketValue(word);
                                        string prop = ReturnPropertyValue(word);

                                        //find the timer
                                        foreach (IrcTimer timer in connection.IRCTimers)
                                        {
                                            if (timer.TimerID == timerid)
                                            {
                                                if (prop.Length == 0)
                                                {
                                                    //replace with timer id
                                                    changedData[count] = timer.TimerID;
                                                }
                                                else
                                                {
                                                    switch (prop.ToLower())
                                                    {
                                                        case "id":
                                                            changedData[count] = timer.TimerID;
                                                            break;
                                                        case "reps":
                                                            changedData[count] = timer.TimerRepetitions.ToString();
                                                            break;
                                                        case "count":
                                                            changedData[count] = timer.TimerCounter.ToString();
                                                            break;
                                                        case "command":
                                                            changedData[count] = timer.TimerCommand;
                                                            break;
                                                        case "interval":
                                                            changedData[count] = timer.TimerInterval.ToString();
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }


                                    if (word.StartsWith("$mask(") && word.IndexOf(')') > word.IndexOf('('))
                                    {
                                        //$mask($host,2)
                                        //get the value between and after the brackets
                                        string values = ReturnBracketValue(word);
                                        string prop = ReturnPropertyValue(word);

                                        if (values.Split(',').Length == 2)
                                        {
                                            string full_host = values.Split(',')[0];
                                            string mask_value = values.Split(',')[1];

                                            if (full_host.Length == 0) break;
                                            if (mask_value.Length == 0) break;

                                            if (full_host.IndexOf("@") == -1) break;
                                            if (full_host.IndexOf("!") == -1) break;

                                            switch (mask_value)
                                            {
                                                case "0":   // *!user@host
                                                    changedData[count] = "*!" + full_host.Substring(full_host.IndexOf("!") + 1);
                                                    break;

                                                case "1":   // *!*user@host
                                                    changedData[count] = "*!*" + full_host.Substring(full_host.IndexOf("!") + 1);
                                                    break;

                                                case "2":   // *!*user@*.host
                                                    changedData[count] = "*!*" + full_host.Substring(full_host.IndexOf("@"));
                                                    break;

                                                case "3":   // *!*user@*.host
                                                    break;

                                                case "4":   // *!*@*.host
                                                    break;

                                                case "5":   // nick!user@host
                                                    changedData[count] = full_host;
                                                    break;

                                                case "6":   // nick!*user@host
                                                    break;

                                                case "7":   // nick!*@host
                                                    break;

                                                case "8":   // nick!*user@*.host
                                                    break;

                                                case "9":   // nick!*@*.host
                                                    break;

                                                case "10":  // nick!*@*
                                                    changedData[count] = full_host.Substring(0, full_host.IndexOf("!")) + "!*@*";
                                                    break;

                                                case "11":  // *!user@*
                                                    break;
                                            }



                                        }

                                    }


                                }
                                break;
                        }


                    }

                }
            }
            catch (Exception e)
            {
                WriteErrorFile(connection, "ParseIdentifiers" + data, e);
            }
            //return String.Join(" ", changedData);
            return JoinString(changedData);
        }
    }
}