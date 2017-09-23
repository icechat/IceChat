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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace IceChat
{
    public partial class ChannelList : UserControl
    {
        private string headerCaption = "Favorite Channels";
        private int headerHeight = 23;
        private FormMain _parent;

        public ChannelList(FormMain parent)
        {
            InitializeComponent();
            this._parent = parent;

            this.Paint += new PaintEventHandler(OnHeaderPaint);
            //this.DoubleClick += new EventHandler(OnDoubleClick);
            this.panelButtons.Resize += new EventHandler(panelButtons_Resize);
            this.Resize += new EventHandler(OnResize);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            
            //load channel list from XML File
            ReadSettings();
        }

        private void OnResize(object sender, EventArgs e)
        {
            listChannels.Height = this.Height - (panelButtons.Height + listChannels.Top);
            listChannels.Width = this.Width;
        }

        internal void ApplyLanguage()
        {
            IceChatLanguage iceChatLanguage = _parent.IceChatLanguage;
            headerCaption = iceChatLanguage.favChanHeader;
            buttonAdd.Text = iceChatLanguage.favChanbuttonAdd;
            buttonJoin.Text = iceChatLanguage.favChanbuttonJoin;
            buttonEdit.Text = iceChatLanguage.favChanbuttonEdit;
            buttonRemove.Text = iceChatLanguage.favChanbuttonRemove;
        }

        private void panelButtons_Resize(object sender, EventArgs e)
        {
            this.buttonAdd.Width = (panelButtons.Width / 2) - 4;
            this.buttonJoin.Width = buttonAdd.Width;
            this.buttonEdit.Width = buttonAdd.Width;
            this.buttonRemove.Width = buttonAdd.Width;
            this.buttonJoin.Left = (panelButtons.Width / 2) + 1;
            this.buttonRemove.Left = buttonJoin.Left;
        }

        internal void SetListColors()
        {
            this.listChannels.BackColor = IrcColor.colors[_parent.IceChatColors.ChannelListBackColor];
            this.listChannels.ForeColor = IrcColor.colors[_parent.IceChatColors.ChannelListForeColor];

            this.panelButtons.BackColor = IrcColor.colors[_parent.IceChatColors.TabbarBackColor];

            this.buttonAdd.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonEdit.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonJoin.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonRemove.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];

            this.buttonAdd.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonEdit.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonJoin.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonRemove.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];

        }

        /// <summary>
        /// Paint the header with a Gradient Background
        /// </summary>
        private void OnHeaderPaint(object sender, PaintEventArgs e)
        {
            Bitmap buffer = new Bitmap(this.Width, this.Height, e.Graphics);
            Graphics g = Graphics.FromImage(buffer);

            //draw the header
            Rectangle headerR = new Rectangle(0, 0, this.Width, headerHeight);
            Brush l = new LinearGradientBrush(headerR, IrcColor.colors[_parent.IceChatColors.PanelHeaderBG1], IrcColor.colors[_parent.IceChatColors.PanelHeaderBG2], 300);

            g.FillRectangle(l, headerR);
            if (this.Parent.Parent.GetType() != typeof(FormFloat))
            {
                if (Application.RenderWithVisualStyles)
                {
                    if (System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsElementDefined(System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar.NormalGroupCollapse.Normal))
                    {
                        System.Windows.Forms.VisualStyles.VisualStyleRenderer renderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar.NormalGroupCollapse.Normal);
                        //which side are we docked on
                        Rectangle rect = Rectangle.Empty;
                        if (((IceDockPanel)this.Parent.Parent.Parent.Parent).Dock == DockStyle.Right)
                            rect = new Rectangle(0, 0, 22, 22);
                        else
                            rect = new Rectangle(this.Width - 22, 0, 22, 22);
                        renderer.DrawBackground(g, rect);
                    }
                }
            }
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            Font headerFont = new Font("Verdana", 10);

            Rectangle centered = headerR;
            centered.Offset(0, (int)(headerR.Height - g.MeasureString(headerCaption, headerFont).Height) / 2);

            g.DrawString(headerCaption, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]), centered, sf);

            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
            buffer.Dispose();
            g.Dispose();
            l.Dispose();
            headerFont.Dispose();                    

        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (this.Parent.Parent.GetType() != typeof(FormFloat))
            {
                if (e.Y <= headerHeight)
                {
                    //which side are we docked on
                    if (((IceDockPanel)this.Parent.Parent.Parent.Parent).Dock == DockStyle.Right && e.X < 22)
                    {
                        ((IceDockPanel)this.Parent.Parent.Parent.Parent).DockControl();
                        return;
                    }
                    else if (((IceDockPanel)this.Parent.Parent.Parent.Parent).Dock == DockStyle.Left && e.X > (this.Width - 22))
                    {
                        ((IceDockPanel)this.Parent.Parent.Parent.Parent).DockControl();
                        return;
                    }
                }
            }
        }
       
        /// <summary>
        /// Read in all the Favorite Channels from the XML File
        /// </summary>
        private void ReadSettings()
        {
            if (!File.Exists(_parent.FavoriteChannelsFile)) return;

            FileStream fs = new FileStream(_parent.FavoriteChannelsFile, FileMode.Open);
            XmlTextReader r = new XmlTextReader(fs);
            string currentElement = "";            
            while (r.Read())
            {
                if (r.Name.Length > 0)
                    currentElement = r.Name;
                else if (r.NodeType == XmlNodeType.Text && r.Value.Length > 1)
                    listChannels.Items.Add(r.Value);
            }

            r.Close();
            fs.Close();

            fs.Dispose();
        }
        
        /// <summary>
        /// Write out all the Favorite Servers to the XML File
        /// </summary>
        private void WriteSettings()
        {
            FileStream fs = new FileStream(_parent.FavoriteChannelsFile, FileMode.Create);
            XmlTextWriter w = new XmlTextWriter(fs, System.Text.Encoding.UTF8);
            w.Formatting = Formatting.Indented;

            w.WriteStartDocument();
            w.WriteStartElement("FavoriteChannels");

            //int i = listChannels.Items.Count;
            for (int i = 0; i < listChannels.Items.Count; i++)
                w.WriteElementString("Channel" + i.ToString(),listChannels.Items[i].ToString());

            w.WriteEndElement();
            w.WriteEndDocument();

            w.Flush();
            w.Close();
            fs.Close();

            fs.Dispose();

            _parent.FocusInputBox();
        }
        
        /// <summary>
        /// Join the Channel selected to the Current Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listChannels_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int s = listChannels.SelectedIndex;

            if (s == -1) return;

            IRCConnection c = _parent.InputPanel.CurrentConnection;
            if (c != null)
                _parent.ParseOutGoingCommand(c, "/join " + listChannels.Items[s].ToString());
        }

        /// <summary>
        /// Use a Dialog Box to ask for a New Favorite Channel to Add
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //ask for a new channel to add
            InputBoxDialog i = new InputBoxDialog();
            i.FormCaption = "Add Favorite Channel";
            i.FormPrompt = "Enter a channel to add";

            i.ShowDialog();
            if (i.InputResponse.Length > 0)
                listChannels.Items.Add(i.InputResponse);
            
            //write out the settings file
            WriteSettings();
        }

        /// <summary>
        /// Join the Channel selected to the Current Server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonJoin_Click(object sender, EventArgs e)
        {
            //join the channel selected
            int s = listChannels.SelectedIndex;

            if (s == -1) return;

            IRCConnection c = _parent.InputPanel.CurrentConnection;
            if (c != null)
            {
                _parent.ParseOutGoingCommand(c, "/join " + listChannels.Items[s].ToString());
            }
            _parent.FocusInputBox();
        }

        /// <summary>
        /// Edit a Favorite Channel selected with a Dialog Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int s = listChannels.SelectedIndex;
            
            if (s == -1) return;

            InputBoxDialog i = new InputBoxDialog();
            i.FormCaption = "Edit Favorite Channel";
            i.FormPrompt = "Enter the new channel name";
            i.DefaultValue = listChannels.Items[s].ToString();

            i.ShowDialog();
            if (i.InputResponse.Length > 0)
                listChannels.Items[s] = i.InputResponse;

            WriteSettings();
        }

        /// <summary>
        /// Remove the Selected Favorite Channel from the List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            int s = listChannels.SelectedIndex;

            if (s == -1) return;

            listChannels.Items.RemoveAt(s);

            WriteSettings();

            _parent.FocusInputBox();
        }
    }
}
