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
        
        //all the events get declared here
        public override event OutGoingCommandHandler OnCommand;

        //declare the standard properties
        private string m_Name;
        private string m_Author;
        private string m_Version;

        public override string Name { get { return m_Name; } }
        public override string Version { get { return m_Version; } }
        public override string Author { get { return m_Author; } }

        private TabPage tabPageHighlight;
        private Button buttonAdd;
        private Button buttonRemove;
        private Button buttonEdit;
        private ListView listHighLite;

        private ColumnHeader columnMatch;
        private ColumnHeader columnColor;
        private ColumnHeader columnMatchHost;

        private IceChatHighLites iceChatNickHighLites;
        private string nickhighlitesFile;

        public Plugin()
        {
            //set your default values here
            m_Name = "Nick Highlite Plugin";
            m_Author = "Snerf";
            m_Version = "1.3";
        }

        //declare the standard methods

        public override void Dispose()
        {

        }

        public override void Initialize()
        {
            nickhighlitesFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatNickHighLites.xml";
            LoadHighLites();

        }

        public override PluginArgs NickListDraw(PluginArgs args)
        {
            //args.Nick == nickname
            //args.Host == nickhighlitesFile host
            
            //args.Extra == color number to change to
            args.Extra = CheckNickHighlite(args);

            return args;
        }

        private string CheckNickHighlite(PluginArgs args)
        {
            string nick = args.Nick;
            string host = args.Host;
            string color = args.Extra;

            foreach (HighLiteItem hli in iceChatNickHighLites.listHighLites)
            {
                if (hli.Enabled)
                {
                    string match = hli.Match;
                    string hostMatch = hli.MatchHost;

                    //match = match.Replace("$me", args.Connection.ServerSetting.NickName);
                    
                    string me = args.Connection.ServerSetting.NickName;
                    me = me.Replace(@"\", @"\\");
                    me = me.Replace(@"[", @"\[");
                    me = me.Replace(@"]", @"\]");
                    me = me.Replace(@"{", @"\{");
                    me = me.Replace(@"}", @"\}");
                    me = me.Replace(@"^", @"\^");
                    me = me.Replace(@"|", @"\|");

                    match = match.Replace("$me", me);
                                        
                    hostMatch = hostMatch.Replace("$host", args.Connection.ServerSetting.LocalHost);
                    hostMatch = hostMatch.Replace("$ident", args.Connection.ServerSetting.IdentName);
                    hostMatch = hostMatch.Replace("$fullhost", args.Connection.ServerSetting.CurrentNickName + "!" + args.Connection.ServerSetting.LocalHost);

                    if (match.Length > 0)
                    {
                        try
                        {
                            if (Regex.IsMatch(nick, match, RegexOptions.IgnoreCase))
                            {
                                color = hli.Color.ToString();
                            }
                        }
                        catch {

                        }
                    }
                    else if (hostMatch.Length > 0)
                    {
                        try
                        {
                            if (Regex.IsMatch(host, hostMatch, RegexOptions.IgnoreCase))
                            {
                                color = hli.Color.ToString();
                            }
                        }
                        catch {

                        }
                    }
                }
            }

            return color;
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
            columnColor = new ColumnHeader();
            columnMatchHost = new ColumnHeader();

            columnMatch.Text = "Nick Match";
            columnMatch.Width = 100;
            columnColor.Width = 0;
            columnMatchHost.Text = "Host Match";
            columnMatchHost.Width = 100;

            tabPageHighlight.SuspendLayout();

            buttonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonRemove.Location = new System.Drawing.Point(391, 96);
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
            buttonEdit.Location = new System.Drawing.Point(391, 63);
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
            buttonAdd.Location = new System.Drawing.Point(391, 30);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new System.Drawing.Size(75, 27);
            buttonAdd.TabIndex = 2;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += new EventHandler(buttonAdd_Click);
            // listHighLite
            // 
            listHighLite.CheckBoxes = true;
            listHighLite.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnMatch,
            columnColor,
            columnMatchHost});

            listHighLite.FullRowSelect = true;
            listHighLite.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listHighLite.HideSelection = false;
            listHighLite.LabelWrap = false;
            listHighLite.Location = new System.Drawing.Point(25, 30);
            listHighLite.MultiSelect = false;
            listHighLite.Name = "listHighLite";
            listHighLite.ShowGroups = false;
            listHighLite.Size = new System.Drawing.Size(350, 288);
            listHighLite.TabIndex = 1;
            listHighLite.UseCompatibleStateImageBehavior = false;
            listHighLite.View = System.Windows.Forms.View.Details;

            tabPageHighlight.BackColor = System.Drawing.SystemColors.Control;
            tabPageHighlight.Controls.Add(buttonRemove);
            tabPageHighlight.Controls.Add(buttonEdit);
            tabPageHighlight.Controls.Add(buttonAdd);
            tabPageHighlight.Controls.Add(listHighLite);
            tabPageHighlight.Location = new System.Drawing.Point(4, 25);
            tabPageHighlight.Name = "tabPageHighlight2";
            tabPageHighlight.Padding = new System.Windows.Forms.Padding(3);
            tabPageHighlight.Size = new System.Drawing.Size(710, 339);


            tabPageHighlight.Text = "Nick Hilite";

            tabPageHighlight.ResumeLayout();

            OptionsTab.Controls.Add(tabPageHighlight);

            ShowHighLites();

        }

        public override void SaveColorsForm()
        {
            iceChatNickHighLites.listHighLites.Clear();

            foreach (ListViewItem item in listHighLite.Items)
            {
                HighLiteItem hli = new HighLiteItem();
                hli.Match = item.Text;
                hli.Color = Convert.ToInt32(item.SubItems[1].Text);
                hli.MatchHost = item.SubItems[2].Text;

                hli.Enabled = item.Checked;

                iceChatNickHighLites.AddHighLight(hli);
            }

            SaveHighLites();
        }

        private void ShowHighLites()
        {
            foreach (HighLiteItem hli in iceChatNickHighLites.listHighLites)
            {
                ListViewItem lvi = this.listHighLite.Items.Add(hli.Match);
                lvi.SubItems.Add(hli.Color.ToString());
                lvi.ForeColor = IrcColor.colors[hli.Color];
                lvi.SubItems.Add(hli.MatchHost);
                
                lvi.Checked = hli.Enabled;
            }
        }

        private void LoadHighLites()
        {
            if (File.Exists(nickhighlitesFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatHighLites));
                TextReader textReader = new StreamReader(nickhighlitesFile);
                iceChatNickHighLites = (IceChatHighLites)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
                iceChatNickHighLites = new IceChatHighLites();
        }

        private void SaveHighLites()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatHighLites));
            TextWriter textWriter = new StreamWriter(nickhighlitesFile);
            serializer.Serialize(textWriter, iceChatNickHighLites);
            textWriter.Close();
            textWriter.Dispose();
        }
        
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHighLite.SelectedItems)
            {
                HighLiteItem hli = new HighLiteItem();

                hli.Match = item.Text;
                hli.Color = Convert.ToInt32(item.SubItems[1].Text);
                hli.MatchHost = item.SubItems[2].Text;

                FormHighLite fi = new FormHighLite(hli, item.Index);
                fi.SaveHighLite += new FormHighLite.SaveHighLiteDelegate(UpdateHighLite);
                fi.ShowDialog(this.MainForm);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormHighLite fi = new FormHighLite(new HighLiteItem(), 0);
            fi.SaveHighLite += new FormHighLite.SaveHighLiteDelegate(SaveNewHighLite);
            fi.ShowDialog(this.MainForm);

        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listHighLite.SelectedItems)
            {
                listHighLite.Items.Remove(item);
            }
        }


        private void SaveNewHighLite(HighLiteItem hli, int listIndex)
        {
            if (hli.Match.Length > 0 || hli.MatchHost.Length > 0)
            {
                ListViewItem lvi = this.listHighLite.Items.Add(hli.Match);
                lvi.SubItems.Add(hli.Color.ToString());
                lvi.SubItems.Add(hli.MatchHost);

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
                    item.SubItems[1].Text = hli.Color.ToString();
                    item.SubItems[2].Text = hli.MatchHost;

                    item.ForeColor = IrcColor.colors[hli.Color];
                    break;
                }
            }
        }


    }
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

        [XmlElement("MatchHost")]
        public string MatchHost
        { get; set; }

        [XmlElement("Color")]
        public int Color
        { get; set; }

        [XmlElement("Enabled")]
        public bool Enabled
        { get; set; }

    }
}
