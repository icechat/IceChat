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
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;

namespace IceChatPlugin
{
    public class Plugin : IPluginIceChat
    {

        private string m_Name;
        private string m_Author;
        private string m_Version;

        public override string Name { get { return m_Name; } }
        public override string Version { get { return m_Version; } }
        public override string Author { get { return m_Author; } }

        //all the events get declared here, do not change
        //public override event OutGoingCommandHandler OnCommand;

        private ToolStripMenuItem m_EnableMonitor;

        private struct cMonitor
        {
            public IceChat.IRCConnection connection;
            public string channel;
            public cMonitor(IceChat.IRCConnection connection, string channel)
            {
                this.connection = connection;
                this.channel = channel;
            }
        }

        List<cMonitor> monitoredChannels = new List<cMonitor>();

        private ListView listMonitor = null;
        private ColumnHeader columnTime;
        private ColumnHeader columnChannel;
        private ColumnHeader columnMessage;
        private ColumnHeader columnServerID;

        private delegate void UpdateMonitorDelegate(string Channel, string Message, bool highlight, int serverID);
        public override event OutGoingCommandHandler OnCommand;

        private const char colorChar = (char)3;
        private const char underlineChar = (char)31;
        private const char boldChar = (char)2;
        private const char plainChar = (char)15;
        private const char reverseChar = (char)22;
        private const char italicChar = (char)29;

        Panel panel = null;

        private IceChatChannelMonitor monitorChannels;
        private string settingsFile;
    

        //requires icechat 9 8.22 (20140221)

        public Plugin()
        {
            //set your default values here
            m_Name = "Channel Monitor Plugin";
            m_Author = "Snerf";
            m_Version = "1.5.2";
        }

        public override void Dispose()
        {
            //remove the listview/panel
            if (panel != null)
            {
                listMonitor.Dispose();
                panel.Dispose();
            }

        }

        public override void Initialize()
        {

            if (CurrentVersion < 90020140221)
            {
                //send back a message that we need to update!
                PluginArgs a = new PluginArgs
                {
                    Command = "/echo Channel Monitor Plugin v1.3 requires IceChat 9 RC 8.22 or newer (" + CurrentVersion + ")"
                };
                OnCommand(a);
                this.Enabled = false;
                return;
            }            
            
            settingsFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatChannelMonitor.xml";
            LoadSettings();

            panel = new Panel
            {
                Height = 150,
                Dock = DockStyle.Bottom
            };

            listMonitor = new ListView();
            columnTime = new ColumnHeader();
            columnChannel = new ColumnHeader();
            columnMessage = new ColumnHeader();
            columnServerID = new ColumnHeader();

            columnTime.Width = 175;
            columnTime.Text = "Time";

            columnChannel.Width = 150;
            columnChannel.Text = "Channel/Nick";

            columnMessage.Width = 1000;
            columnMessage.Text = "Message";

            columnServerID.Width = 0;   //hidden

            listMonitor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnTime,
            columnChannel,
            columnMessage,
            columnServerID});

            listMonitor.View = System.Windows.Forms.View.Details;
            listMonitor.FullRowSelect = true;
            listMonitor.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            listMonitor.Dock = DockStyle.Fill;
            panel.Controls.Add(listMonitor);

            if (CurrentVersion > 90220150213)
            {
                listMonitor.DoubleClick += new EventHandler(ListMonitor_DoubleClick);
            }

            m_EnableMonitor = new ToolStripMenuItem
            {
                Text = "Toggle Monitor",
                Checked = true
            };
            m_EnableMonitor.Click += new EventHandler(OnEnableMonitor_Click);

        }

        public override ToolStripMenuItem MenuItemShow(ToolStripMenuItem menu)
        {
            if (menu == m_EnableMonitor)
            {
                // check if this needs to be checked or not
                cMonitor newChan = new cMonitor(ServerTreeCurrentConnection, ServerTreeCurrentTab);
                if (monitoredChannels.IndexOf(newChan) > -1)
                {
                    menu.Checked = true;
                }
                else
                {
                    menu.Checked = false;
                }
            }
            
            return menu;
        }

        private void ListMonitor_DoubleClick(object sender, EventArgs e)
        {
            //use the /switch #channel serverID to open that channel
            //will require icechat 9.03+
            if (listMonitor.SelectedItems.Count == 1)
            {
                ListViewItem lvi = listMonitor.SelectedItems[0];
                PluginArgs a = new PluginArgs
                {
                    Command = "/switch " + lvi.SubItems[1].Text + " " + lvi.SubItems[3].Text
                };
                OnCommand(a);
            }
        }

        public override void MainProgramLoaded(SortedList ServerConnections)
        {
            //automatically add in the opened channels
            foreach (IceChat.IRCConnection c in ServerConnections.Values)
            {
                if (c.IsConnected)
                {
                    //get all the open channels
                    foreach (string chan in c.OpenChannels)
                    {
                        cMonitor newChan = new cMonitor(c, chan);
                        if (monitoredChannels.IndexOf(newChan) == -1)
                        {
                            monitoredChannels.Add(newChan);
                            AddMonitorMessage(newChan.channel, "Started Monitoring channel:" + monitoredChannels.Count, false, c.ServerSetting.ID);
                        }
                    }                
                }
            }

            
        }
        //declare the standard properties


        public override ToolStripItem[] AddChannelPopups()
        {
            return (new System.Windows.Forms.ToolStripItem[] { m_EnableMonitor });
        }

        public override Panel[] AddMainPanel()
        {
            return (new Panel[] { panel });
        }

        private void OnEnableMonitor_Click(object sender, EventArgs e)
        {
            //get the current selected item for the popup menu
            cMonitor newChan = new cMonitor(ServerTreeCurrentConnection, ServerTreeCurrentTab);
            bool mEnabled = false;
            if (((ToolStripMenuItem)sender).CheckState == CheckState.Checked)
            {
                //remove the channel from being monitored
                if (monitoredChannels.IndexOf(newChan) > -1)
                {
                    monitoredChannels.Remove(newChan);
                    AddMonitorMessage(newChan.channel, "Stopped Monitoring channel:" + monitoredChannels.Count, false, ServerTreeCurrentConnection.ServerSetting.ID);
                }
                ((ToolStripMenuItem)sender).CheckState = CheckState.Unchecked;


            }
            else
            {
                //add the channel for monitoring
                if (monitoredChannels.IndexOf(newChan) == -1)
                {
                    monitoredChannels.Add(newChan);
                    AddMonitorMessage(newChan.channel, "Started Monitoring channel:" + monitoredChannels.Count, false, ServerTreeCurrentConnection.ServerSetting.ID);
                }
                ((ToolStripMenuItem)sender).CheckState = CheckState.Checked;
                mEnabled = true;
            }

            //save this setting in an XML file
            if (monitorChannels.ChannelExist(newChan.channel) != null)
            {
                monitorChannels.UpdateChannel(newChan.channel, mEnabled);
            }
            else
            {
                MonitorItem c = new MonitorItem
                {
                    Channel = newChan.channel,
                    Enabled = mEnabled
                };

                monitorChannels.AddChannel(c);
            }

            SaveSettings();

        }

       
        private void AddMonitorMessage(string Channel, string Message, bool highlight, int serverID)
        {
            if (panel.InvokeRequired)
            {
                UpdateMonitorDelegate umd = new UpdateMonitorDelegate(AddMonitorMessage);
                panel.Invoke(umd, new object[] { Channel, Message, highlight, serverID });
            }
            else
            {
                DateTime now = DateTime.Now;
                ListViewItem lvi = new ListViewItem(now.ToString());
                
                lvi.SubItems.Add(Channel);
                lvi.SubItems.Add(Message);
                lvi.SubItems.Add(serverID.ToString());

                if (highlight)
                    lvi.ForeColor = System.Drawing.Color.Red;
                
                listMonitor.Items.Add(lvi);



                //scroll the listview to the bottom
                listMonitor.EnsureVisible(listMonitor.Items.Count - 1);
                
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatChannelMonitor));
                TextReader textReader = new StreamReader(settingsFile);
                monitorChannels = (IceChatChannelMonitor)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
            {
                //create default settings
                monitorChannels = new IceChatChannelMonitor();
                SaveSettings();
            }
        }

        private void SaveSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatChannelMonitor));
            TextWriter textWriter = new StreamWriter(settingsFile);
            serializer.Serialize(textWriter, monitorChannels);
            textWriter.Close();
            textWriter.Dispose();
        }


        private string StripColorCodes(string line)
        {
            //strip out all the color codes, bold , underline and reverse codes
            string ParseBackColor = @"\x03([0-9]{1,2}),([0-9]{1,2})";
            string ParseForeColor = @"\x03[0-9]{1,2}";
            string ParseColorChar = @"\x03";
            string ParseBoldChar = @"\x02";
            string ParseUnderlineChar = @"\x1F";    //code 31
            string ParseReverseChar = @"\x16";      //code 22
            string ParseItalicChar = @"\x1D";      //code 29

            line = line.Replace("&#x3;", colorChar.ToString());

            StringBuilder sLine = new StringBuilder();
            sLine.Append(line);

            Regex ParseIRCCodes = new Regex(ParseBackColor + "|" + ParseForeColor + "|" + ParseColorChar + "|" + ParseBoldChar + "|" + ParseUnderlineChar + "|" + ParseReverseChar + "|" + ParseItalicChar);

            Match m = ParseIRCCodes.Match(sLine.ToString());

            while (m.Success)
            {
                sLine.Remove(m.Index, m.Length);
                m = ParseIRCCodes.Match(sLine.ToString(), m.Index);
            }

            return sLine.ToString();
        }

        //declare all the necessary events

        public override PluginArgs ChannelMessage(PluginArgs args)
        {
            //check if monitoring is enabled for this channel
            cMonitor newChan = new cMonitor(args.Connection, args.Channel);
            if (monitoredChannels.IndexOf(newChan) > -1)
            {
                //check if nick is said
                string message = StripColorCodes(args.Message);
                if (message.IndexOf(args.Connection.ServerSetting.CurrentNickName) > -1)
                    AddMonitorMessage(args.Channel, message, true, args.Connection.ServerSetting.ID);
                else
                    AddMonitorMessage(args.Channel, message, false, args.Connection.ServerSetting.ID);
            }
            return args;
        }

        public override PluginArgs ChannelAction(PluginArgs args)
        {
            cMonitor newChan = new cMonitor(args.Connection, args.Channel);
            if (monitoredChannels.IndexOf(newChan) > -1)
            {
                string message = StripColorCodes(args.Message);
                if (message.IndexOf(args.Connection.ServerSetting.CurrentNickName) > -1)
                    AddMonitorMessage(args.Channel, message, true, args.Connection.ServerSetting.ID);
                else
                    AddMonitorMessage(args.Channel, message, false, args.Connection.ServerSetting.ID);
            }
            return args;
        }
        
        public override PluginArgs ChannelJoin(PluginArgs args)
        {
            if (args.Nick == args.Connection.ServerSetting.NickName)
            {                
                //add the channel to the list
                //check if this channel is on Monitor settings
                bool disabled = false;
                if (monitorChannels.ChannelExist(args.Channel) != null)
                {
                    if (monitorChannels.ChannelExist(args.Channel).Enabled == false)
                    {
                        disabled = true;
                    }
                }
                
                if (disabled == false)
                {
                    cMonitor newChan = new cMonitor(args.Connection, args.Channel);
                    monitoredChannels.Add(newChan);

                    AddMonitorMessage(args.Channel, "Started Monitoring channel:" + monitoredChannels.Count, false, args.Connection.ServerSetting.ID);
                }

                if (monitorChannels.ChannelExist(args.Channel) != null)
                {
                    monitorChannels.UpdateChannel(args.Channel, !disabled);
                }
                else
                {
                    MonitorItem c = new MonitorItem
                    {
                        Channel = args.Channel,
                        Enabled = !disabled
                    };

                    monitorChannels.AddChannel(c);
                }

                SaveSettings();
            
            }
            return args;
        }
        
        public override PluginArgs ChannelPart(PluginArgs args)
        {
            if (args.Nick == args.Connection.ServerSetting.NickName)
            {
                //remove the channel from the list
                cMonitor newChan = new cMonitor(args.Connection, args.Channel);
                if (monitoredChannels.IndexOf(newChan) > -1)
                {
                    monitoredChannels.Remove(newChan);
                    AddMonitorMessage(args.Channel, "Stopped Monitoring channel:" + monitoredChannels.Count, false, args.Connection.ServerSetting.ID);
                }

            }
            return args;
        }


        public override PluginArgs CtcpMessage(PluginArgs args)
        {
            //args.Extra        -- ctcp message 
            AddMonitorMessage(args.Nick, "CTCP : " + args.Extra, false, args.Connection.ServerSetting.ID);

            return args;
        }


    }
    
    public class IceChatChannelMonitor
    {
        [XmlArray("Channels")]
        [XmlArrayItem("Item", typeof(MonitorItem))]
        public ArrayList listChannels;

        public IceChatChannelMonitor()
        {
            listChannels = new ArrayList();
        }
        
        public void AddChannel(MonitorItem item)
        {
            listChannels.Add(item);
        }

        public void UpdateChannel(string channel, bool enabled)
        {
            foreach (MonitorItem item in listChannels)
            {
                if (item.Channel.Equals(channel))
                    item.Enabled = enabled;
            }
        }

        public MonitorItem ChannelExist(string channel)
        {
            MonitorItem item = null;
            foreach (MonitorItem mi in listChannels)
            {
                if (mi.Channel.Equals(channel))
                    item = mi;
            }

            return item;
        }
    }

    public class MonitorItem
    {
        [XmlElement("Channel")]
        public string Channel
        { get; set; }

        [XmlElement("Enabled")]
        public bool Enabled
        { get; set; }


    }

}
