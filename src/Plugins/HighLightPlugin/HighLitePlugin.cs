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
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace IceChatPlugin
{
    public class Plugin : IPluginIceChat
    {
        //declare the standard properties
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
        private ListView listHighLite;

        private ColumnHeader columnMatch;
        private ColumnHeader columnCommand;
        private ColumnHeader columnColor;
        private ColumnHeader columnInclude;
        private ColumnHeader columnExclude;
        private ColumnHeader columnSound;

        private IceChatHighLites iceChatHighLites;
        private string highlitesFile;

        public Plugin()
        {
            //set your default values here
            m_Name = "HighLite Plugin";
            m_Author = "Snerf";
            m_Version = "2.9.1";
        }

        public override void Dispose()
        {
            
        }

        public override void Initialize()
        {
            if (this.CurrentVersion < 9020140221)
            {
                //need a newer version
                PluginArgs a = new PluginArgs
                {
                    Command = "/echo Highlite Plugin v2.9 requires IceChat 9 RC 8.22 or newer"
                };
                OnCommand(a);
                this.Enabled = false;

                return;
            }
            
            highlitesFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatHighLites.xml";
            LoadHighLites();



        }
        
        public override void LoadColorsForm(TabControl OptionsTab)
        {
            //when the Options Form gets loaded, ability to add tabs
            //add the Highlite Tab

            //when the Settings Form gets loaded, ability to add tabs

            tabPageHighlight = new System.Windows.Forms.TabPage();
            buttonAdd = new Button();
            buttonRemove = new Button();
            buttonEdit = new Button();
            listHighLite = new ListView();
            columnMatch = new ColumnHeader();
            columnCommand = new ColumnHeader();
            columnColor = new ColumnHeader();
            columnInclude = new ColumnHeader();
            columnExclude = new ColumnHeader();
            columnSound = new ColumnHeader();

            columnMatch.Text = "Text Highlite";
            columnMatch.Width = 250;
            columnCommand.Width = 0;
            columnColor.Width = 0;
            columnInclude.Width = 0;
            columnExclude.Width = 0;
            columnSound.Width = 0;

            tabPageHighlight.SuspendLayout();

            buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonRemove.Location = new System.Drawing.Point(291, 96);
            buttonRemove.Name = "buttonRemove";
            buttonRemove.Size = new System.Drawing.Size(75, 27);
            buttonRemove.TabIndex = 4;
            buttonRemove.Text = "&Remove";
            buttonRemove.UseVisualStyleBackColor = true;
            buttonRemove.Click += new EventHandler(ButtonRemove_Click);
            // 
            // buttonEdit
            // 
            buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonEdit.Location = new System.Drawing.Point(291, 63);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new System.Drawing.Size(75, 27);
            buttonEdit.TabIndex = 3;
            buttonEdit.Text = "&Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += new EventHandler(ButtonEdit_Click);
            // 
            // buttonAdd
            // 
            buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonAdd.Location = new System.Drawing.Point(291, 30);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(75, 27);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "&Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += new EventHandler(ButtonAdd_Click);
            // listHighLite
            // 
            listHighLite.CheckBoxes = true;
            listHighLite.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnMatch,
            columnCommand,
            columnColor,
            columnInclude,
            columnExclude,
            columnSound});
            
            
            listHighLite.FullRowSelect = true;
            listHighLite.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listHighLite.HideSelection = false;
            listHighLite.LabelWrap = false;
            listHighLite.Location = new System.Drawing.Point(25, 30);
            listHighLite.MultiSelect = false;
            listHighLite.Name = "listHighLite";
            listHighLite.ShowGroups = false;
            listHighLite.Size = new System.Drawing.Size(250, 288);
            listHighLite.TabIndex = 1;
            listHighLite.UseCompatibleStateImageBehavior = false;
            listHighLite.View = System.Windows.Forms.View.Details;
            listHighLite.DoubleClick += new EventHandler(ListHighLite_DoubleClick);
            

            tabPageHighlight.BackColor = System.Drawing.SystemColors.Control;
            tabPageHighlight.Controls.Add(buttonRemove);
            tabPageHighlight.Controls.Add(buttonEdit);
            tabPageHighlight.Controls.Add(buttonAdd);
            tabPageHighlight.Controls.Add(listHighLite);
            tabPageHighlight.Location = new System.Drawing.Point(4, 25);
            tabPageHighlight.Name = "tabPageHighlight2";
            tabPageHighlight.Padding = new System.Windows.Forms.Padding(3);
            tabPageHighlight.Size = new System.Drawing.Size(710, 339);
            
            
            tabPageHighlight.Text = "High Lite";

            tabPageHighlight.ResumeLayout();

            OptionsTab.Controls.Add(tabPageHighlight);

            ShowHighLites();

        }

        private void ListHighLite_DoubleClick(object sender, EventArgs e)
        {
            //double click event unchecks the box.. annoying!
            if (listHighLite.SelectedItems.Count == 1)
            {
                ListViewItem item = listHighLite.SelectedItems[0];

                if (listHighLite.FocusedItem == item)
                {
                    if (listHighLite.FocusedItem.Checked == false)
                        listHighLite.FocusedItem.Checked = true;
                    else
                        listHighLite.FocusedItem.Checked = false;
                }


                HighLiteItem hli = new HighLiteItem
                {
                    Match = item.Text,
                    Command = item.SubItems[1].Text.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()),
                    Color = Convert.ToInt32(item.SubItems[2].Text),
                    FlashTab = Convert.ToBoolean(item.SubItems[3].Text),
                    NicksInclude = item.SubItems[4].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries),
                    NicksExclude = item.SubItems[5].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries),
                    Sound = item.SubItems[6].Text
                };

                FormHighLite fi = new FormHighLite(hli, item.Index);
                fi.SaveHighLite += new FormHighLite.SaveHighLiteDelegate(UpdateHighLite);
                fi.ShowDialog(this.MainForm);


            }
        }

        public override void SaveColorsForm()
        {
            iceChatHighLites.listHighLites.Clear();

            foreach (ListViewItem item in listHighLite.Items)
            {
                HighLiteItem hli = new HighLiteItem
                {
                    Match = item.Text,
                    Command = item.SubItems[1].Text,
                    Color = Convert.ToInt32(item.SubItems[2].Text),
                    Enabled = item.Checked,
                    NicksInclude = item.SubItems[4].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                };
                ;
                hli.NicksExclude = item.SubItems[5].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries); ;

                hli.FlashTab = Convert.ToBoolean(item.SubItems[3].Text);
                hli.Sound = item.SubItems[6].Text;

                iceChatHighLites.AddHighLight(hli);
            }
            
            SaveHighLites();
        }

        private void ShowHighLites()
        {
            foreach (HighLiteItem hli in iceChatHighLites.listHighLites)
            {
                ListViewItem lvi = this.listHighLite.Items.Add(hli.Match);
                lvi.SubItems.Add(hli.Command);
                lvi.SubItems.Add(hli.Color.ToString());                
                lvi.SubItems.Add(hli.FlashTab.ToString());
                
                if (hli.NicksInclude != null)
                    lvi.SubItems.Add(string.Join(" ", hli.NicksInclude));
                else
                    lvi.SubItems.Add("");

                if (hli.NicksExclude != null)
                    lvi.SubItems.Add(string.Join(" ", hli.NicksExclude));
                else
                    lvi.SubItems.Add("");

                lvi.SubItems.Add(hli.Sound);
                
                lvi.ForeColor = IrcColor.colors[hli.Color];
                lvi.Checked = hli.Enabled;
            }
        }

        private void LoadHighLites()
        {
            if (File.Exists(highlitesFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatHighLites));
                TextReader textReader = new StreamReader(highlitesFile);
                iceChatHighLites = (IceChatHighLites)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
                iceChatHighLites = new IceChatHighLites();
        }

        private void SaveHighLites()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatHighLites));
            TextWriter textWriter = new StreamWriter(highlitesFile);
            serializer.Serialize(textWriter, iceChatHighLites);
            textWriter.Close();
            textWriter.Dispose();
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHighLite.SelectedItems)
            {
                HighLiteItem hli = new HighLiteItem
                {
                    Match = item.Text,
                    Command = item.SubItems[1].Text.Replace("&#x3;", ((char)3).ToString()).Replace("&#x2;", ((char)2).ToString()),
                    Color = Convert.ToInt32(item.SubItems[2].Text),
                    FlashTab = Convert.ToBoolean(item.SubItems[3].Text),
                    NicksInclude = item.SubItems[4].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries),
                    NicksExclude = item.SubItems[5].Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries),
                    Sound = item.SubItems[6].Text
                };

                FormHighLite fi = new FormHighLite(hli, item.Index);
                fi.SaveHighLite += new FormHighLite.SaveHighLiteDelegate(UpdateHighLite);                
                fi.ShowDialog(this.MainForm);
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            FormHighLite fi = new FormHighLite(new HighLiteItem(), 0);
            fi.SaveHighLite += new FormHighLite.SaveHighLiteDelegate(SaveNewHighLite);
            fi.ShowDialog(this.MainForm);

        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHighLite.SelectedItems)
            {
                listHighLite.Items.Remove(item);
            }

        }

        private void SaveNewHighLite(HighLiteItem hli, int listIndex)
        {
            if (hli.Match.Length > 0)
            {
                ListViewItem lvi = this.listHighLite.Items.Add(hli.Match);
                lvi.SubItems.Add(hli.Command.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;"));
                lvi.SubItems.Add(hli.Color.ToString());
                lvi.SubItems.Add(hli.FlashTab.ToString());
                if (hli.NicksInclude != null)
                    lvi.SubItems.Add(string.Join(" ", hli.NicksInclude));
                else
                    lvi.SubItems.Add("");

                if (hli.NicksExclude != null)
                    lvi.SubItems.Add(string.Join(" ", hli.NicksExclude));
                else
                    lvi.SubItems.Add("");

                lvi.SubItems.Add(hli.Sound);

                lvi.ForeColor = IrcColor.colors[hli.Color];
                lvi.Checked = true;
            }
        }

        private void UpdateHighLite(HighLiteItem hli, int listIndex)
        {
            foreach (ListViewItem item in listHighLite.SelectedItems)
            {
                if (item.Index == listIndex)
                {
                    item.Text = hli.Match;
                    item.SubItems[1].Text = hli.Command.Replace(((char)3).ToString(), "&#x3;").Replace(((char)2).ToString(), "&#x2;");
                    item.SubItems[2].Text = hli.Color.ToString();
                    item.SubItems[3].Text = hli.FlashTab.ToString();
                    item.SubItems[4].Text = string.Join(" ", hli.NicksInclude);
                    item.SubItems[5].Text = string.Join(" ", hli.NicksExclude);
                    item.SubItems[6].Text = hli.Sound;

                    item.ForeColor = IrcColor.colors[hli.Color];
                    break;
                }
            }
        }

        private string CheckTextHighLite(PluginArgs args)
        {
            //parse out any identifiers for the channel/nick, etc
            string message = args.Message;
            try
            {
                foreach (HighLiteItem hli in iceChatHighLites.listHighLites)
                {
                    if (hli.Enabled)
                    {
                        string match = hli.Match;
                        // if $me is part of the match string substitute with the current nickname. 
                        // Some characters that are allowed in nicknames carry significance in regex.
                        // These are  - \ [ ] { } ^ |
                        // We need to escape them with \ before using the nickname in the regex match

                        string me = args.Connection.ServerSetting.CurrentNickName;

                        me = me.Replace(@"\", @"\\");
                        me = me.Replace(@"[", @"\[");
                        me = me.Replace(@"]", @"\]");
                        me = me.Replace(@"{", @"\{");
                        me = me.Replace(@"}", @"\}");
                        me = me.Replace(@"^", @"\^");
                        me = me.Replace(@"|", @"\|");

                        //me = me.Replace(@"-", @"\-");  Don't need this because it has significance only between [ and ] which are already escaped

                        match = match.Replace("$me", me);

                        if (Regex.IsMatch(message, match, RegexOptions.IgnoreCase))
                        {
                            //check include list
                            if (hli.NicksInclude != null && hli.NicksInclude.Length > 0)
                            {
                                bool nickFound = false;
                                foreach (string ni in hli.NicksInclude)
                                {
                                    //string n = EscapeNick(ni);
                                    //if (Regex.IsMatch(args.Nick, "\\b" + n + "\\b", RegexOptions.IgnoreCase)) 
                                    if (Regex.IsMatch(args.Nick, ni, RegexOptions.IgnoreCase))
                                    {
                                        nickFound = true;
                                    }
                                }
                                if (nickFound == false)
                                    continue;

                            }

                            //check exclude list
                            if (hli.NicksExclude != null && hli.NicksExclude.Length > 0)
                            {
                                bool nickFound = false;
                                foreach (string ni in hli.NicksExclude)
                                {
                                    //string n = EscapeNick(ni);
                                    if (Regex.IsMatch(args.Nick, ni, RegexOptions.IgnoreCase))
                                    {
                                        nickFound = true;
                                    }
                                }
                                if (nickFound == true)
                                    continue;
                            }

                            message = message.Replace(args.Extra, "&#x3;" + hli.Color.ToString("00") + args.Extra);

                            if (hli.FlashTab == true)
                            {
                                if (args.Channel.Length > 0)
                                    args.Command = "/flash " + args.Channel;
                                else if (args.Nick.Length > 0)
                                    args.Command = "/flash " + args.Nick;

                                OnCommand(args);

                            }

                            if (hli.Sound != null)
                            {
                                if (hli.Sound.Length > 0)
                                {
                                    args.Command = "/cplay " + args.Channel + " " + hli.Sound;
                                    OnCommand(args);
                                }
                            }

                            if (hli.Command != null)
                            {
                                if (hli.Command.Length > 0)
                                {
                                    args.Command = hli.Command.Replace("$message", args.Extra);
                                    args.Command = args.Command.Replace("$match", hli.Match);
                                    args.Command = args.Command.Replace("$chan", args.Channel);
                                    args.Command = args.Command.Replace("$nick", args.Nick);

                                    OnCommand(args);
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //message = ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return message;
        }

        public override PluginArgs ChannelMessage(PluginArgs args)
        {
            args.Message = CheckTextHighLite(args);            
            
            return args;
        }

        public override PluginArgs ChannelAction(PluginArgs args)
        {
            args.Message = CheckTextHighLite(args);
            
            return args;
        }

        public override PluginArgs QueryMessage(PluginArgs args)
        {
            args.Message = CheckTextHighLite(args);
            
            return args;
        }

        public override PluginArgs QueryAction(PluginArgs args)
        {
            args.Message = CheckTextHighLite(args);
            
            return args;
        }        
    }

    //seperate file for all the highlite items
    public class IceChatHighLites
    {
        [XmlArray("HighLites")]
        [XmlArrayItem("Item", typeof(HighLiteItem))]
        public ArrayList listHighLites;

        public IceChatHighLites()
        {
            listHighLites = new ArrayList();
        }
        public void AddHighLight(HighLiteItem hli)
        {
            listHighLites.Add(hli);
        }
    }
    
    public class HighLiteItem
    {
        [XmlElement("Match")]
        public string Match
        { get; set; }

        [XmlElement("Color")]
        public int Color
        { get; set; }

        [XmlElement("Command")]
        public string Command
        { get; set; }

        [XmlElement("Sound")]
        public string Sound
        { get; set; }

        [XmlElement("Enabled")]
        public bool Enabled
        { get; set; }

        [XmlElement("FlashTab")]
        public bool FlashTab
        { get; set; }

        [XmlArray("NicksInclude")]
        [XmlArrayItem("Item", typeof(string))]
        public string[] NicksInclude
        { get; set; }

        [XmlArray("NicksExclude")]
        [XmlArrayItem("Item", typeof(string))]
        public string[] NicksExclude
        { get; set; }

    }

}
