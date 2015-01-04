/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2011 Paul Vanderzee <snerf@icechat.net>
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
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;


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
        public override event OutGoingCommandHandler OnCommand;

        private TabPage tabPageHighlight;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonEdit;
        private ListView listScripts;

        private ColumnHeader columnTextMatch;
        private ColumnHeader columnCommand;
        private ColumnHeader columnChannelMatch;
        private ColumnHeader columnEventType;

        private IceChatScripts iceChatScripts;
        private string scriptsFile;

        public Plugin()
        {
            //set your default values here
            m_Name = "Simple Script Plugin (Beta)";
            m_Author = "Snerf";
            m_Version = "1.0";
        }

        public override void Dispose()
        {

        }

        public override void Initialize()
        {
            scriptsFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatSimpleScript.xml";
            LoadScriptSettings();

        }

        public override void LoadEditorForm(TabControl ScriptsTab, MenuStrip MainMenu)
        {
            //when the Editor Form gets loaded, ability to add tabs            
            tabPageHighlight = new System.Windows.Forms.TabPage();
            buttonAdd = new Button();
            buttonRemove = new Button();
            buttonEdit = new Button();
            listScripts = new ListView();
            columnTextMatch = new ColumnHeader();
            columnCommand = new ColumnHeader();
            columnChannelMatch = new ColumnHeader();
            columnEventType = new ColumnHeader();

            columnTextMatch.Width = 200;
            columnChannelMatch.Width = 200;
            columnEventType.Text = "Script Event";
            columnEventType.Width = 200;
            columnCommand.Width = 0;

            tabPageHighlight.SuspendLayout();

            buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonRemove.Location = new System.Drawing.Point(460, 96);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new System.Drawing.Size(75, 27);
            buttonRemove.TabIndex = 4;
            buttonRemove.Text = "Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += new EventHandler(buttonRemove_Click);
            // 
            // buttonEdit
            // 
            buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonEdit.Location = new System.Drawing.Point(460, 63);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new System.Drawing.Size(75, 27);
            buttonEdit.TabIndex = 3;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += new EventHandler(buttonEdit_Click);
            // 
            // buttonAdd
            // 
            buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonAdd.Location = new System.Drawing.Point(460, 30);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(75, 27);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += new EventHandler(buttonAdd_Click);
            // listScripts
            // 
            listScripts.CheckBoxes = true;
            listScripts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnEventType,
            columnCommand,
            columnTextMatch, 
            columnChannelMatch });


            listScripts.FullRowSelect = true;
            listScripts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listScripts.HideSelection = false;
            listScripts.LabelWrap = false;
            listScripts.Location = new System.Drawing.Point(25, 30);
            listScripts.MultiSelect = false;
            listScripts.Name = "listScripts";
            listScripts.ShowGroups = false;
            listScripts.Size = new System.Drawing.Size(450, 288);
            listScripts.Dock = DockStyle.Left;
            listScripts.TabIndex = 1;
            listScripts.UseCompatibleStateImageBehavior = false;
            listScripts.View = System.Windows.Forms.View.Details;

            tabPageHighlight.BackColor = System.Drawing.SystemColors.Control;
            tabPageHighlight.Controls.Add(buttonRemove);
            tabPageHighlight.Controls.Add(buttonEdit);
            tabPageHighlight.Controls.Add(buttonAdd);
            tabPageHighlight.Controls.Add(listScripts);
            tabPageHighlight.Location = new System.Drawing.Point(4, 25);
            tabPageHighlight.Name = "tabPageHighlight2";
            tabPageHighlight.Padding = new System.Windows.Forms.Padding(3);
            

            tabPageHighlight.Text = "Simple Script";

            tabPageHighlight.ResumeLayout();

            ScriptsTab.Controls.Add(tabPageHighlight);

            ShowScriptItems();


        }

        public override void SaveEditorForm()
        {
            iceChatScripts.listScripts.Clear();

            foreach (ListViewItem item in listScripts.Items)
            {
                ScriptItem scr = new ScriptItem();
                scr.ScriptEvent = item.Text;
                scr.Command = item.SubItems[1].Text;
                scr.TextMatch = item.SubItems[2].Text;
                scr.ChannelMatch = item.SubItems[3].Text;
                scr.Enabled = item.Checked;
                iceChatScripts.AddScriptItem(scr);
            }

            SaveScriptSettings();
        }

        private void ShowScriptItems()
        {
            foreach (ScriptItem scr in iceChatScripts.listScripts)
            {
                ListViewItem lvi = this.listScripts.Items.Add(scr.ScriptEvent);
                lvi.SubItems.Add(scr.Command);
                lvi.SubItems.Add(scr.TextMatch);
                lvi.SubItems.Add(scr.ChannelMatch);
                lvi.Checked = scr.Enabled;
            }
        }

        private void LoadScriptSettings()
        {
            if (File.Exists(scriptsFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatScripts));
                TextReader textReader = new StreamReader(scriptsFile);
                iceChatScripts = (IceChatScripts)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
                iceChatScripts = new IceChatScripts();
        }

        private void SaveScriptSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatScripts));
            TextWriter textWriter = new StreamWriter(scriptsFile);
            serializer.Serialize(textWriter, iceChatScripts);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listScripts.SelectedItems)
            {
                ScriptItem scr = new ScriptItem();

                scr.ScriptEvent = item.Text;
                scr.Command = item.SubItems[1].Text;
                scr.TextMatch = item.SubItems[2].Text;
                scr.ChannelMatch = item.SubItems[3].Text;
                scr.Enabled = item.Checked;

                FormScriptItem fi = new FormScriptItem(scr, item.Index);
                fi.SaveScriptItem += new FormScriptItem.SaveScriptItemDelegate(UpdateScriptItem);                
                fi.ShowDialog(MainForm);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormScriptItem fsi = new FormScriptItem(new ScriptItem(), 0);
            fsi.SaveScriptItem += new FormScriptItem.SaveScriptItemDelegate(SaveNewScriptItem);
            fsi.ShowDialog(MainForm);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listScripts.SelectedItems)
            {
                listScripts.Items.Remove(item);
            }

        }

        private void SaveNewScriptItem(ScriptItem scr, int listIndex)
        {
            if (scr.TextMatch.Length > 0)
            {
                ListViewItem lvi = this.listScripts.Items.Add(scr.ScriptEvent);
                lvi.SubItems.Add(scr.Command);
                lvi.SubItems.Add(scr.TextMatch);
                lvi.SubItems.Add(scr.ChannelMatch);
                lvi.Checked = true;
            }
        }

        private void UpdateScriptItem(ScriptItem scr, int listIndex)
        {
            foreach (ListViewItem item in listScripts.SelectedItems)
            {
                if (item.Index == listIndex)
                {
                    item.Text= scr.ScriptEvent;
                    item.SubItems[1].Text = scr.Command;
                    item.SubItems[2].Text = scr.TextMatch;
                    item.SubItems[3].Text = scr.ChannelMatch;
                    item.Checked = scr.Enabled;
                    break;
                }
            }
        }

        private PluginArgs CheckScripts(PluginArgs args, string eventType)
        {
            foreach (ScriptItem scr in iceChatScripts.listScripts)
            {
                string command = "";
                if (scr.Enabled && eventType == scr.ScriptEvent)
                {
                    switch (scr.ScriptEvent)
                    {

                        case "Channel Message":
                        case "Channel Action":
                            //use a regex match of sorts down the road
                            Regex regChannel = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);
                            if (args.Channel != null && regChannel.IsMatch(args.Channel))
                            {
                                Regex regMatch = new Regex(scr.TextMatch, RegexOptions.IgnoreCase);

                                if (regMatch.IsMatch(args.Extra))                                
                                {
                                    command = scr.Command.Replace("$chan", args.Channel);
                                    command = command.Replace("$match", scr.TextMatch);
                                    command = command.Replace("$message", args.Extra);
                                    command = command.Replace("$nick", args.Nick);

                                    args.Command = command;
                                    
                                    if (OnCommand != null)
                                        OnCommand(args);

                                }
                            }                            
                            break;
                        case "Private Message":
                        case "Private Action":
                            Regex regChannel2 = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);
                            if (args.Channel != null && regChannel2.IsMatch(args.Channel))
                            {
                                Regex regMatch = new Regex(scr.TextMatch, RegexOptions.IgnoreCase);
                                if (regMatch.IsMatch(args.Extra))
                                {
                                    command = scr.Command.Replace("$chan",args.Channel);
                                    command = command.Replace("$nick", args.Nick);
                                    command = command.Replace("$match", scr.TextMatch);
                                    command = command.Replace("$message", args.Extra);

                                    args.Command = command;

                                    if (OnCommand != null)
                                        OnCommand(args);

                                }
                            }
                        
                            break;                        
                        case "Channel Join":
                            Regex regChannel3 = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);                           
                            if (args.Channel != null && regChannel3.IsMatch(args.Channel))
                            {
                                command = scr.Command.Replace("$chan", args.Channel);
                                command = command.Replace("$nick", args.Nick);
                                command = command.Replace("$match", scr.TextMatch);

                                args.Command = command;

                                if (OnCommand != null)
                                    OnCommand(args);
                            
                            } 
                            break;
                        case "Channel Kick":
                            Regex regChannel4 = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);
                            if (args.Channel != null && regChannel4.IsMatch(args.Channel))
                            {
                                command = scr.Command.Replace("$chan", args.Channel);
                                command = command.Replace("$nick", args.Nick);
                                command = command.Replace("$match", scr.TextMatch);
                                command = command.Replace("$message", args.Extra);

                                args.Command = command;

                                if (OnCommand != null)
                                    OnCommand(args);

                            }
                            break;

                        case "Channel Invite":
                            Regex regChannel5 = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);
                            if (args.Channel != null && regChannel5.IsMatch(args.Channel))
                            {
                                command = scr.Command.Replace("$chan", args.Channel);
                                command = command.Replace("$nick", args.Nick);
                                command = command.Replace("$match", scr.TextMatch);

                                args.Command = command;

                                if (OnCommand != null)
                                    OnCommand(args);

                            }
                            break;

                        case "Channel Part":
                            Regex regChannel6 = new Regex(scr.ChannelMatch, RegexOptions.IgnoreCase);
                            if (args.Channel != null && regChannel6.IsMatch(args.Channel))
                            {
                                command = scr.Command.Replace("$chan", args.Channel);
                                command = command.Replace("$nick", args.Nick);
                                command = command.Replace("$match", scr.TextMatch);
                                command = command.Replace("$message", args.Extra);

                                args.Command = command;

                                if (OnCommand != null)
                                    OnCommand(args);

                            }
                            break;

                        case "IceChat Startup":
                            args.Command = scr.Command;
                            if (OnCommand != null)
                                OnCommand(args);                            
                            
                            break;
                    }

                }
            }
           
            return args;
        }

        public override PluginArgs ChannelMessage(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Message");
            return args;
        }

        public override PluginArgs ChannelAction(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Action");
            return args;
        }

        public override PluginArgs QueryMessage(PluginArgs args)
        {
            args = CheckScripts(args, "Private Message");
            return args;
        }

        public override PluginArgs QueryAction(PluginArgs args)
        {
            args = CheckScripts(args, "Private Action");
            return args;
        }

        public override PluginArgs ChannelJoin(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Join");
            return args;
        }

        public override PluginArgs ChannelPart(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Part");
            return args;            
        }

        public override void ChannelInvite(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Invite");
        }

        public override PluginArgs ChannelKick(PluginArgs args)
        {
            args = CheckScripts(args, "Channel Kick");
            return args;
        }

        public override void MainProgramLoaded()
        {
            PluginArgs args = new PluginArgs();            
            CheckScripts(args, "IceChat Startup");            
        }

    }

    //seperate file for all the highlite items
    public class IceChatScripts
    {
        [XmlArray("ScriptItems")]
        [XmlArrayItem("Item", typeof(ScriptItem))]
        public ArrayList listScripts;

        public IceChatScripts()
        {
            listScripts = new ArrayList();
        }
        public void AddScriptItem(ScriptItem scr)
        {
            listScripts.Add(scr);
        }
    }
    
    public class ScriptItem
    {
        [XmlElement("ScriptEvent")]
        public string ScriptEvent
        { get; set; }
        
        //watch text to match
        [XmlElement("TextMatch")]
        public string TextMatch
        { get; set; }

        //match a user or a nick
        [XmlElement("ChannelMatch")]
        public string ChannelMatch
        { get; set; }

        //the command to run on a match
        [XmlElement("Command")]
        public string Command
        { get; set; }

        //whether the script item is enabled
        [XmlElement("Enabled")]
        public bool Enabled
        { get; set; }
    }

}
