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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

using IceChatPlugin;

namespace IceChat
{
    public partial class NickList : UserControl
    {
        private delegate void AddNickCallBack(User nick);
        private delegate void ClearListDelegate();
        private delegate void UpdateHeaderDelegate(string data);

        private IceTabPage currentWindow;
        
        //the index of the top item in the nick list
        private int topIndex = 0;
        private bool mouseFocus = false;
        private bool controlKeyDown = false;

        private int headerHeight = 23;
        private int selectedIndex = -1;
        private string headerCaption = "";

        private ToolTip toolTip;
        private int toolTipNode = -1;

        private List<Nick> sortedNickNames = null;
        private int totalSelected = 0;

        private ContextMenuStrip popupMenu;

        private Bitmap _backgroundImage = null;
        private string _backgroundImageFile;

        private Bitmap _buffer = null;
        private FormMain _parent;

        private string currentKeySelected = "";
        private int currentNickSelected = 0;


        internal class Nick : IComparable
        {
            public string nick;
            public string host;
            public bool selected;
            public int nickColor;
            public bool[] Level;
            public bool Away;

            public bool CustomColor;

            public int CompareTo(object obj)
            {
                Nick u = (Nick)obj;

                if (FormMain.Instance.IceChatOptions.NickListSort)

                    return this.NickOnly.CompareTo(u.NickOnly);

                else
                {

                    int compareNickValue = 0;
                    int thisNickValue = 0;

                    bool[] userCompareLevel = new bool[u.Level.Length];
                    bool[] thisCompareLevel = new bool[this.Level.Length];

                    for (int i = 0; i < userCompareLevel.Length; i++)
                        userCompareLevel[i] = u.Level[i];

                    for (int i = 0; i < thisCompareLevel.Length; i++)
                        thisCompareLevel[i] = this.Level[i];

                    Array.Reverse(userCompareLevel);
                    Array.Reverse(thisCompareLevel);

                    for (int i = userCompareLevel.Length - 1; i >= 0; i--)
                    {
                        if (userCompareLevel[i])
                        {
                            compareNickValue = i + 1;
                            break;
                        }
                    }

                    for (int i = thisCompareLevel.Length - 1; i >= 0; i--)
                    {
                        if (thisCompareLevel[i])
                        {
                            thisNickValue = i + 1;
                            break;
                        }
                    }

                    if (compareNickValue > thisNickValue)
                        return 1;
                    else if (compareNickValue == thisNickValue)
                        return this.nick.CompareTo(u.nick);
                    else
                        return -1;

                }
            }

            internal string NickOnly
            {
                get
                {

                    string nick = this.nick;
                    string[] modes = new string[] { "@", "!", "+", "%", "&", "~" };

                    for (int i = 0; i < modes.Length; i++)
                        if (nick.StartsWith(modes[i]))
                            nick = nick.Substring(1);

                    return nick;
                }
            }

            public override string ToString()
            {
                return this.nick;
            }
        }

        public NickList(FormMain parent)
        {
            InitializeComponent();
            
            this._parent = parent;
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseWheel += new MouseEventHandler(OnMouseWheel);            
            this.DoubleClick += new EventHandler(OnDoubleClick);
            this.Resize += new EventHandler(OnResize);            
            this.FontChanged += new EventHandler(OnFontChanged);
            this.panelButtons.Resize += new EventHandler(panelButtons_Resize);
            this.panelButtons.VisibleChanged += new EventHandler(panelButtons_VisibleChanged);
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            this.vScrollBar.Scroll += new ScrollEventHandler(OnScroll);
            this.DoubleBuffered = true;

            this.MouseEnter += new EventHandler(OnMouseEnter);
            this.MouseLeave += new EventHandler(OnMouseLeave);

            SetStyle(ControlStyles.ResizeRedraw |  ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 3000;
            toolTip.ForeColor = System.Drawing.SystemColors.InfoText;
            toolTip.BackColor = System.Drawing.SystemColors.Info;

            popupMenu = new ContextMenuStrip();
            popupMenu.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            popupMenu.Renderer = new EasyRenderer.EasyRender();

            this.MouseWheel += new MouseEventHandler(NickList_MouseWheel);
        }

        private void NickList_MouseWheel(object sender, MouseEventArgs e)
        {
            this.ScrollWindow(e.Delta > 0);            
        }

        private void panelButtons_VisibleChanged(object sender, EventArgs e)
        {
            if (this.panelButtons.Visible)
                this.vScrollBar.Height = this.Height - this.headerHeight - this.panelButtons.Height;
            else
                this.vScrollBar.Height = this.Height - this.headerHeight;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            mouseFocus = false;
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            mouseFocus = true;
        }

        internal bool MouseHasFocus
        {
            get { return mouseFocus; }
        }

        //this is to make the arrow keys work in the user control
        protected override bool IsInputKey(Keys AKeyData)
        {
            return true;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
                controlKeyDown = true;

            if (e.KeyCode == Keys.Up)
            {
                selectedIndex--;
                Invalidate();
            }
            else if (e.KeyCode == Keys.Down)
            {
                selectedIndex++;
                Invalidate();
            }
            else if (e.KeyCode == Keys.Apps)
            {
                //right mouse key
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1, 0, 0, 0));
            }
            else
            {
                // find a nickname that starts with letter?
                if (currentWindow != null && sortedNickNames != null)
                {
                    /* this doesnt work properly.. scroll bar disappears for higher letters */

                    int totalMatches = 0;
                    int firstMatch = -1;

                    bool currentKeyMatch = false;

                    for (int x = 0; x < sortedNickNames.Count; x++)
                    {

                        string nick = sortedNickNames[x].nick;

                        //replace any of the modes
                        for (int i = 0; i < currentWindow.Connection.ServerSetting.StatusModes[1].Length; i++)
                            if (nick.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                nick = nick.Substring(1);


                        if (e.KeyCode.ToString().ToLower() == currentKeySelected)
                        {
                            // make it go to the next one
                            currentKeyMatch = true;

                        }


                        if (nick.ToLower().StartsWith(e.KeyCode.ToString().ToLower()))
                        {

                            if (currentKeyMatch == true)
                            {
                                if (totalMatches == 0)
                                {
                                    firstMatch = x;
                                }

                                totalMatches++;

                                if (x <= currentNickSelected)
                                {
                                    continue;
                                }

                            }


                            DeSelectAllNicks();

                            // check if we are in the scroll view
                            //System.Diagnostics.Debug.WriteLine(x + " => " + vScrollBar.Value + "  to " + (vScrollBar.Value + vScrollBar.LargeChange ));

                            if (x >= vScrollBar.Value && x <= (vScrollBar.Value + vScrollBar.LargeChange))
                            {
                                // no need to change
                            }
                            else if (x > (vScrollBar.Maximum - vScrollBar.LargeChange))
                            {
                                this.topIndex = (vScrollBar.Maximum - vScrollBar.LargeChange);
                            }
                            else
                            {
                                this.topIndex = x;
                            }

                            this.vScrollBar.Value = this.topIndex;

                            sortedNickNames[x].selected = true;
                            currentWindow.GetNick(sortedNickNames[x].nick).Selected = true;

                            totalSelected = 1;

                            // System.Diagnostics.Debug.WriteLine(sortedNickNames[x].nick + ": SV=" +  vScrollBar.Value + ": LC=" + vScrollBar.LargeChange + ": TI=" + topIndex + ":scroll to:" + x + ":" + vScrollBar.Maximum + ":" + this.sortedNickNames.Count);

                            Invalidate();


                            currentKeySelected = e.KeyCode.ToString().ToLower();
                            currentNickSelected = x;

                            return;
                        }

                    }

                    if (currentKeyMatch == true)
                    {
                        //System.Diagnostics.Debug.WriteLine("Done:" + totalMatches + ":" + firstMatch);
                        if (totalMatches > 1)
                        {
                            if (firstMatch > -1)
                            {

                                DeSelectAllNicks();

                                //System.Diagnostics.Debug.WriteLine("First- go to:");
                                int x = firstMatch;

                                if (x >= vScrollBar.Value && x <= (vScrollBar.Value + vScrollBar.LargeChange))
                                {
                                    // no need to change
                                }
                                else if (x > (vScrollBar.Maximum - vScrollBar.LargeChange))
                                {
                                    this.topIndex = (vScrollBar.Maximum - vScrollBar.LargeChange);
                                }
                                else
                                {
                                    this.topIndex = x;
                                }

                                this.vScrollBar.Value = this.topIndex;

                                sortedNickNames[x].selected = true;
                                currentWindow.GetNick(sortedNickNames[x].nick).Selected = true;

                                totalSelected = 1;

                                Invalidate();


                                currentNickSelected = x;

                            }

                        }

                    }

                }
            }
        } 

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            controlKeyDown = false;
        }

        private void OnResize(object sender, EventArgs e)
        {
            this.vScrollBar.Left = this.Width - this.vScrollBar.Width;
            if (this.panelButtons.Visible)
                this.vScrollBar.Height = this.Height - this.headerHeight - this.panelButtons.Height;
            else
                this.vScrollBar.Height = this.Height - this.headerHeight;
        }

        internal void ApplyLanguage()
        {
            IceChatLanguage iceChatLanguage = _parent.IceChatLanguage;
            if (_parent.ChannelBar.CurrentTab == _parent.ChannelBar.GetTabPage("Console")) 
                Header = _parent.IceChatLanguage.consoleTabTitle;
            // TODO: add code to load button texts from language class
        }

        private void panelButtons_Resize(object sender, EventArgs e)
        {
            buttonOp.Width = (this.panelButtons.Width / 4) - 4;
            buttonVoice.Width = buttonOp.Width;
            buttonBan.Width = buttonOp.Width;
            buttonInfo.Width = buttonOp.Width;
            buttonHop.Width = buttonOp.Width;
            buttonQuery.Width = buttonOp.Width;
            buttonKick.Width = buttonOp.Width;
            buttonWhois.Width = buttonOp.Width;

            buttonVoice.Left = this.panelButtons.Width / 4 + 1;
            buttonQuery.Left = buttonVoice.Left;
            buttonBan.Left = this.panelButtons.Width / 2;
            buttonKick.Left = buttonBan.Left;
            buttonInfo.Left = this.panelButtons.Width / 4 * 3 + 1;
            buttonWhois.Left = buttonInfo.Left;
        }

        private void OnScroll(object sender, ScrollEventArgs e)
        {
            topIndex = e.NewValue;
            Invalidate();
        }

        internal void ScrollWindow(bool scrollUp)
        {
            try
            {
                if (vScrollBar.Visible)
                {
                    if (scrollUp && (topIndex > 0))
                    {
                        topIndex--;
                        vScrollBar.Value--;
                        Invalidate();
                    }
                    else if (!scrollUp && (topIndex + (vScrollBar.LargeChange - 1)) < vScrollBar.Maximum)
                    {
                        topIndex++;
                        vScrollBar.Value++;
                        Invalidate();
                    }
                }                
            }

            catch (Exception)  { }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (selectedIndex >= 0)
            {
                string nick = sortedNickNames[selectedIndex].nick;

                //replace any of the modes
                for (int i = 0; i < currentWindow.Connection.ServerSetting.StatusModes[1].Length; i++)
                    if (nick.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][i].ToString()))
                        nick = nick.Substring(1);

                _parent.ParseOutGoingCommand(currentWindow.Connection, "/query " + nick);
            }
        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
        
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (vScrollBar.Visible == true)
            {
                if (e.Delta < 0)
                {
                    if (vScrollBar.Maximum < (vScrollBar.Value + 1 + vScrollBar.LargeChange))
                    {
                        vScrollBar.Value = vScrollBar.Maximum - vScrollBar.LargeChange + 1;
                    }
                    else
                    {
                        vScrollBar.Value += 1;
                    }
                }
                else if (e.Delta > 0)
                {
                    if (0 > (vScrollBar.Value - 1))
                    {
                        vScrollBar.Value = 0;
                    }
                    else
                    {
                        vScrollBar.Value -= 1;
                    }
                }

                topIndex = vScrollBar.Value;
                Invalidate();
            }
        }
        
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            int nickNumber = 0;
            try
            {
                if (this.Parent.Parent.GetType() != typeof(FormFloat))
                {
                    if (e.Y <= headerHeight && e.Button == MouseButtons.Left)
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

                if (e.Y <= headerHeight)
                {
                    //de-select any previous items
                    DeSelectAllNicks();
                    totalSelected = 0;

                    Invalidate();

                    return;
                }

                if (currentWindow != null && currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    //do the math
                    Graphics g = this.CreateGraphics();

                    int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
                    //find the nickname number, add 1 to it to make it a non-zero value
                    nickNumber = Convert.ToInt32((e.Location.Y - headerHeight) / _lineSize) + topIndex;

                    if (nickNumber < currentWindow.Nicks.Count && nickNumber < sortedNickNames.Count)
                    {
                        selectedIndex = nickNumber;
                        bool selected = sortedNickNames[selectedIndex].selected;

                        if (selected)
                            totalSelected--;
                        else
                            totalSelected++;

                        //if the CTRL-Key is down, we can do a multi-select
                        if (controlKeyDown)
                        {
                            sortedNickNames[selectedIndex].selected = !selected;
                            currentWindow.GetNick(sortedNickNames[selectedIndex].nick).Selected = !selected;
                        }
                        else
                        {
                            if (totalSelected > 0)
                            {
                                //deselect all the previous ones
                                DeSelectAllNicks();                            
                            }
                            
                            totalSelected = 1;

                            sortedNickNames[selectedIndex].selected = true;
                            currentWindow.GetNick(sortedNickNames[selectedIndex].nick).Selected = true;

                        }
                    }
                    else
                    {
                        DeSelectAllNicks();
                        totalSelected = 0;
                        selectedIndex = -1;
                    }

                    g.Dispose();
                    Invalidate();
                }
            }
            catch (Exception)
            {
                /*
                if (currentWindow != null)
                    _parent.WriteErrorFile(currentWindow.Connection, "NickList OnMouseDown:" + nickNumber + ":" + sortedNickNames.Count + ":" + currentWindow.Nicks.Count, ex);
                else
                    _parent.WriteErrorFile(currentWindow.Connection, "NickList OnMouseDown: null:" + nickNumber + ":" + sortedNickNames.Count, ex);
                */ 
            }
        }

        private void DeSelectAllNicks()
        {
            if (sortedNickNames == null) return;
            if (currentWindow == null) return;

            for (int i = 0; i < sortedNickNames.Count; i++)
            {
                sortedNickNames[i].selected = false;
                currentWindow.GetNick(sortedNickNames[i].nick).Selected = false;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y <= headerHeight)
                return;

            if (currentWindow != null && currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                Graphics g = this.CreateGraphics();

                int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
                //find the nickname number, add 1 to it to make it a non-zero value
                int nickNumber = Convert.ToInt32((e.Location.Y - headerHeight) / _lineSize) + topIndex;

                if (sortedNickNames == null) return;

                if (nickNumber < sortedNickNames.Count)
                {
                    if (toolTipNode != nickNumber)
                    {
                        string nick = sortedNickNames[nickNumber].nick;
                        for (int i = 0; i < currentWindow.Connection.ServerSetting.StatusModes[1].Length; i++)
                            if (nick.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                nick = nick.Substring(1);

                        toolTip.ToolTipTitle = "User Information";
                        if (sortedNickNames[nickNumber].host.Length > 0)
                        {
                            toolTip.SetToolTip(this, nick + Environment.NewLine + sortedNickNames[nickNumber].host);                        
                        }
                        else
                        {
                            toolTip.SetToolTip(this, nick);
                        }
                        toolTipNode = nickNumber;
                    }
                }
                else
                {
                    toolTipNode = -1;
                    toolTip.RemoveAll();
                }
                g.Dispose();
            }
            
        }

        private void OnPopupMenuClick(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null) return;

            string command = ((ToolStripMenuItem)sender).Tag.ToString();
            if (command.Length > 0)
            {
                _parent.ParseOutGoingCommand(currentWindow.Connection, command);
            }
        }

        private void PopulateNicks()
        {
            if (sortedNickNames != null)
            {
                sortedNickNames.Clear();
                sortedNickNames = null;
            }
            
            sortedNickNames = new List<Nick>();
            
            try
            {
                foreach (User nick in currentWindow.Nicks.Values)
                {
                    Nick n = new Nick();
                    n.nick = nick.ToString();
                    n.selected = nick.Selected;
                    n.nickColor = nick.nickColor;
                    n.Level = nick.Level;
                    
                    n.Away = nick.Away;
                    n.CustomColor = nick.CustomColor;

                    if (currentWindow.Connection.ServerSetting.IAL.ContainsKey(nick.NickName))
                    {
                        n.host = ((InternalAddressList)currentWindow.Connection.ServerSetting.IAL[nick.NickName]).Host;
                        n.Away = ((InternalAddressList)currentWindow.Connection.ServerSetting.IAL[nick.NickName]).AwayStatus;
                    }
                    else
                    {
                        n.host = "";
                    }

                    sortedNickNames.Add(n);

                }
                
                sortedNickNames.Sort();
                 
            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.Source);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (_buffer != null)
            {
                _buffer.Dispose();
                _buffer = null;
            }
            
            base.OnSizeChanged(e);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //int howFar = 0;
            int i = 0;
            try
            {
                if (_parent == null)
                    return;

                if (_buffer == null)
                    _buffer = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                
                Graphics g = Graphics.FromImage(_buffer);

                Rectangle listR;
                if (this.panelButtons.Visible)
                    listR = new Rectangle(0, headerHeight, this.Width, this.Height - headerHeight - panelButtons.Height);
                else
                    listR = new Rectangle(0, headerHeight, this.Width, this.Height - headerHeight);

                if (_backgroundImage != null)
                    g.DrawImage((Image)_backgroundImage, listR);
                else
                    g.FillRectangle(new SolidBrush(IrcColor.colors[_parent.IceChatColors.NickListBackColor]), listR);

                //howFar = 1;

                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                //draw the header
                Font headerFont = new Font("Verdana", 10);
                
                Rectangle headerR = new Rectangle(0, 0, this.Width, headerHeight);
                //get the header colors here
                Brush l = new LinearGradientBrush(headerR, IrcColor.colors[_parent.IceChatColors.PanelHeaderBG1], IrcColor.colors[_parent.IceChatColors.PanelHeaderBG2], 300);
                g.FillRectangle(l, headerR);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                
                Rectangle centered = headerR;
                centered.Offset(11, (int)(headerR.Height - e.Graphics.MeasureString(headerCaption, headerFont).Height) / 2);
                //centered.Width = this.Width - 22;

                //see if the headerCaption size is too wide
                SizeF f = g.MeasureString(headerCaption, headerFont);

                //howFar = 2;

                if (f.Width > (this.Width - 24))
                {
                    //too wide, including icon
                    //change the headerCaption
                    if (headerCaption.IndexOf(':') > -1)
                    {   
                        //get the last part of the header (number of users)
                        string endCaption = headerCaption.Substring(headerCaption.LastIndexOf(':'));

                        int totalWidth = (int)g.MeasureString(endCaption, headerFont).Width; 
                        string tempHeader = "";
                        for (int ii = 0; ii < (headerCaption.Length - endCaption.Length); ii++)
                        {
                            SizeF w = g.MeasureString(headerCaption[ii].ToString(), headerFont);
                            totalWidth += (int)w.Width;
                            tempHeader += headerCaption[ii];
                            
                            if (totalWidth > (this.Width - 22))
                            {
                                tempHeader += ".." + endCaption;
                                g.DrawString(tempHeader, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]), centered, sf);
                                break;
                            }
                        }
                    }
                    else
                    {
                        int totalWidth = 0;
                        string tempHeader = "";
                        for (int ii = 0; i < headerCaption.Length; ii++)
                        {
                            SizeF w = g.MeasureString(headerCaption[ii].ToString(), headerFont);
                            totalWidth += (int)w.Width;
                            tempHeader += headerCaption[ii];
                            if (totalWidth > (this.Width - 22))
                            {
                                tempHeader += "..";
                                g.DrawString(tempHeader, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]),centered, sf);
                                break;
                            }
                        }

                    }
                }
                else
                    g.DrawString(headerCaption, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]), centered, sf);

                //howFar = 3;

                if (this.Parent.Parent.GetType() != typeof(FormFloat))
                {
                    if (Application.RenderWithVisualStyles)
                    {
                        //System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar.ThumbRight
                        /*
                        if (System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsElementDefined(System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar.ThumbRight.Normal))
                        {
                            System.Windows.Forms.VisualStyles.VisualStyleRenderer renderer = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ExplorerBar.IEBarMenu.Normal);
                            //which side are we docked on
                            Rectangle rect = Rectangle.Empty;
                            if (((IceDockPanel)this.Parent.Parent.Parent.Parent).Dock == DockStyle.Right)
                                rect = new Rectangle(0, 0, 22, 22);
                            else
                                rect = new Rectangle(this.Width - 22, 0, 22, 22);
                            renderer.DrawBackground(g, rect);
                        }
                        */ 
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

                //howFar = 4;

                //draw the nicks 
                if (currentWindow != null && currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                {
                    PopulateNicks();

                    //howFar = 5;

                    int currentY = listR.Y;
                    int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));

                    int randColor = -1;

                    PluginArgs args = new PluginArgs(currentWindow.Connection);
                    args.Channel = currentWindow.TabCaption;
                    
                    if (sortedNickNames != null)
                    {
                        if (_parent.IceChatColors.RandomizeNickColors == true)
                        {
                            for (int ii = 0; ii < sortedNickNames.Count; ii++)
                            {
                                randColor++;
                                if (randColor > (IrcColor.colors.Length - 1))
                                    randColor = 0;

                                //make sure its not the same color as the background
                                if (randColor == _parent.IceChatColors.NickListBackColor)
                                {
                                    randColor++;

                                    if (randColor > (IrcColor.colors.Length - 1))
                                        randColor = 1;
                                }

                                currentWindow.GetNick(sortedNickNames[ii].nick).nickColor = randColor;
                            }
                        }
                    }
                    
                    //howFar = 6;

                    System.Drawing.Font rFont = this.Font;
                    System.Drawing.Font iFont = new Font(this.Font, FontStyle.Italic);    //for away nicks
                    
                    //this will error if sortedNickNames is null
                    if (sortedNickNames != null)
                    {
                        //howFar = 62;
                        for (i = topIndex; i < sortedNickNames.Count; i++)
                        {
                            Brush b = null;
                            args.Extra = "";

                            User u = currentWindow.GetNick(sortedNickNames[i].nick);
                            //howFar = 72;
                            if (u == null)
                            {
                                //System.Diagnostics.Debug.WriteLine("Null Nick:" + sortedNickNames[i].nick);
                            }
                            if (_parent.IceChatColors.RandomizeNickColors == true || u.CustomColor == true)
                            {
                                b = new SolidBrush(IrcColor.colors[u.nickColor]);
                            }
                            else
                            {
                                //get the correct nickname color for channel status
                                for (int y = 0; y < u.Level.Length; y++)
                                {
                                    if (u.Level[y])
                                    {
                                        switch (currentWindow.Connection.ServerSetting.StatusModes[0][y])
                                        {
                                            case 'q':
                                            case 'y':
                                                args.Extra = _parent.IceChatColors.ChannelOwnerColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelOwnerColor;
                                                break;
                                            case 'a':
                                                args.Extra = _parent.IceChatColors.ChannelAdminColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelAdminColor;
                                                break;
                                            case 'o':
                                                args.Extra = _parent.IceChatColors.ChannelOpColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelOpColor;
                                                break;
                                            case 'h':
                                                args.Extra = _parent.IceChatColors.ChannelHalfOpColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelHalfOpColor;
                                                break;
                                            case 'v':
                                                args.Extra = _parent.IceChatColors.ChannelVoiceColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelVoiceColor;
                                                break;
                                            default:
                                                args.Extra = _parent.IceChatColors.ChannelRegularColor.ToString();
                                                u.nickColor = _parent.IceChatColors.ChannelRegularColor;
                                                break;
                                        }

                                        break;
                                    }
                                }
                                if (args.Extra.Length == 0)
                                {
                                    args.Extra = _parent.IceChatColors.ChannelRegularColor.ToString();
                                    u.nickColor = _parent.IceChatColors.ChannelRegularColor;
                                }
                            }

                            //have ability to override this
                            args.Nick = sortedNickNames[i].nick;
                            args.Host = sortedNickNames[i].host;

                            //howFar = 11;
                            foreach (Plugin p in _parent.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                        args = ipc.plugin.NickListDraw(args);
                                }
                            }

                            //check if selected, if so, draw the selector bar
                            if (sortedNickNames[i].selected == true)
                            {
                                g.FillRectangle(new SolidBrush(SystemColors.Highlight), 0, currentY, this.Width, _lineSize);
                                b = new SolidBrush(SystemColors.HighlightText);
                            }
                            else
                            {
                                //check if args.Extra is numeric and within color range                            
                                int result;
                                if (Int32.TryParse(args.Extra, out result))
                                {
                                    if (result >= 0 && result < 72)
                                    {
                                        b = new SolidBrush(IrcColor.colors[result]);
                                        u.nickColor = result;
                                    }
                                }

                                if (b == null)
                                {
                                    b = new SolidBrush(IrcColor.colors[_parent.IceChatColors.ChannelRegularColor]);
                                    u.nickColor = _parent.IceChatColors.ChannelRegularColor;
                                }
                            }

                            //howFar = 13;

                            if (sortedNickNames[i].Away == true)
                            {
                                //user is away.. italic
                                g.DrawString(args.Nick, iFont, b, 2, currentY);
                            }
                            else
                                g.DrawString(args.Nick, rFont, b, 2, currentY);

                            //howFar = 14;

                            //draw the host
                            if (_parent.IceChatOptions.ShowNickHost)
                            {
                                //howFar = 21;
                                if (currentWindow.Connection.ServerSetting.IAL.ContainsKey(u.NickName))
                                {
                                    //howFar = 22;                                    
                                    if (args.Host != null && args.Host.Length > 0)
                                    {                                        
                                        //howFar = 23;
                                        /*
                                        if (g != null)
                                            howFar = 24;
                                        else
                                            howFar = 25;
                                        */
                                        g.DrawString(args.Host, rFont, b, (rFont.SizeInPoints * 14), currentY);

                                    }
                                }
                            }

                            //howFar = 15;

                            currentY += _lineSize;
                            if (currentY >= (listR.Height + listR.Y))
                            {
                                vScrollBar.Maximum = sortedNickNames.Count - 2;
                                vScrollBar.LargeChange = ((listR.Height - _lineSize) / _lineSize);
                                break;
                            }
                        } //end of for (i)
                    }

                    //howFar = 17;

                    if (currentY >= listR.Height || vScrollBar.Value > 0)
                        vScrollBar.Visible = true;
                    else
                    {
                        vScrollBar.Visible = false;
                    }
                }

                l.Dispose();
                sf.Dispose();

                //howFar = 18;

                //paint the buffer onto the usercontrol
                e.Graphics.DrawImageUnscaled(_buffer, 0, 0);

                //howFar = 19;

                g.Dispose();
                headerFont.Dispose();

                //howFar = 20;

            }
            catch (Exception)
            {
                /*
                if (currentWindow != null)
                    _parent.WriteErrorFile(currentWindow.Connection, "NickList OnPaint:H=" + howFar + ":i=" + i + ":ti=" + topIndex + ":" + headerCaption + ":NickCount=" + currentWindow.Nicks.Values.Count + ":NC=" + sortedNickNames.Count, ee);
                else
                    _parent.WriteErrorFile(null, "NickList OnPaint: null", ee);
                */ 
            }
        }

        /// <summary>
        /// Show the popup Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && selectedIndex != -1)
            {
                //show the popup menu
                foreach (PopupMenuItem p in _parent.IceChatPopupMenus.listPopups)
                {
                    if (p.PopupType == "NickList")
                    {
                        string[] menuItems = p.Menu;
                        List<string> list = new List<string>();
                        for (int i = 0; i < menuItems.Length; i++)
                        {
                            if (menuItems[i] != "")
                                list.Add(menuItems[i]);
                        }
                        menuItems = list.ToArray();

                        //build the menu
                        ToolStripItem t;

                        int subMenu = 0;

                        popupMenu.Items.Clear();
                        
                        string nick = sortedNickNames[selectedIndex].ToString();
                        //replace any of the modes
                        for (int i = 0; i < currentWindow.Connection.ServerSetting.StatusModes[1].Length; i++)
                            if (nick.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][i].ToString()))
                                nick = nick.Substring(1);

                        Nick u = sortedNickNames[selectedIndex];

                        foreach (string menu in menuItems)
                        {
                            string caption;
                            string command;
                            string menuItem = menu;
                            int menuDepth = 0;

                            //get the menu depth
                            while (menuItem.StartsWith("."))
                            {
                                menuItem = menuItem.Substring(1);
                                menuDepth++;
                            }

                            if (menu.IndexOf(':') > 0)
                            {
                                caption = menuItem.Substring(0, menuItem.IndexOf(':'));
                                command = menuItem.Substring(menuItem.IndexOf(':') + 1);
                            }
                            else
                            {
                                caption = menuItem;
                                command = "";
                            }

                            if (caption.Length > 0)
                            {
                                if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    caption = caption.Replace("$chan", currentWindow.TabCaption);
                                    command = command.Replace("$chan", currentWindow.TabCaption);

                                    caption = caption.Replace(" # ", " " + currentWindow.TabCaption + " ");
                                    command = command.Replace(" # ", " " + currentWindow.TabCaption + " ");
                                }
                                caption = caption.Replace("$nick", nick);
                                command = command.Replace("$nick", nick);

                                if (caption == "-")
                                    t = new ToolStripSeparator();
                                else
                                {
                                    t = new ToolStripMenuItem(caption);
                                    t.ForeColor = SystemColors.MenuText;
                                    t.BackColor = SystemColors.Menu;

                                    //parse out the command/$identifiers                            
                                    command = command.Replace("$1", nick);
                                    command = command.Replace("$nick", nick);

                                    if (u.host.Length > 0)
                                        command = command.Replace("$host", nick + "!" + u.host);
                                    else
                                        command = command.Replace("$host", nick);

                                        t.Click += new EventHandler(OnPopupMenuClick);
                                    t.Tag = command;
                                }
                                if (menuDepth == 0)
                                    subMenu = popupMenu.Items.Add(t);
                                else
                                    if (popupMenu.Items[subMenu].GetType() != typeof(ToolStripSeparator))
                                        ((ToolStripMenuItem)popupMenu.Items[subMenu]).DropDownItems.Add(t);

                                t = null;
                            }
                        }
                        popupMenu.Show(this, e.Location);
                    }
                }
            }            
        }

        private Object thisLock = new Object();

        internal void SelectNick(string nick)
        {
            if (sortedNickNames == null) return;

            //select a specific nickname in the nicklist
            lock (thisLock)
            {

                for (int i = 0; i < sortedNickNames.Count; i++)
                {
                    if (nick == sortedNickNames[i].ToString())
                    {
                        //matched
                        DeSelectAllNicks();

                        selectedIndex = i;
                        int p = (selectedIndex / vScrollBar.LargeChange);

                        if ((topIndex + vScrollBar.LargeChange) < selectedIndex && vScrollBar.Visible)
                        {
                            topIndex += (p * vScrollBar.LargeChange);

                            if ((selectedIndex - vScrollBar.LargeChange) < topIndex)
                            {
                                topIndex = selectedIndex;
                            }
                        }
                        else if ((topIndex > selectedIndex) && vScrollBar.Visible)
                        {
                            topIndex = (p * vScrollBar.LargeChange);
                        }

                        //see if it is on the last page
                        if (vScrollBar.Visible && (topIndex > (vScrollBar.Maximum - vScrollBar.LargeChange)))
                        {
                            topIndex = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
                        }

                        sortedNickNames[selectedIndex].selected = true;
                        currentWindow.GetNick(sortedNickNames[selectedIndex].nick).Selected = true;

                        //calculate the new scrollbar value
                        if (vScrollBar.Visible)
                        {
                            try
                            {
                                vScrollBar.Value = topIndex;
                            }
                            catch (Exception)
                            {
                                //System.Diagnostics.Debug.WriteLine("scrollbar error:" + topIndex + ":" + vScrollBar.Maximum);
                            }

                        }

                        Invalidate();

                        return;
                    }
                    else if (nick == sortedNickNames[i].ToString().Substring(1))
                    {
                        //matched
                        DeSelectAllNicks();

                        selectedIndex = i;
                        int p = (selectedIndex / vScrollBar.LargeChange);

                        if ((topIndex + vScrollBar.LargeChange) < selectedIndex && vScrollBar.Visible)
                        {
                            topIndex += (p * vScrollBar.LargeChange);
                            if ((selectedIndex - vScrollBar.LargeChange) < topIndex)
                            {
                                //System.Diagnostics.Debug.WriteLine("fix2:" + topIndex + ":" + (selectedIndex - vScrollBar.LargeChange));
                                topIndex = selectedIndex - vScrollBar.LargeChange;
                            }
                        }
                        else if ((topIndex > selectedIndex) && vScrollBar.Visible)
                        {
                            topIndex = (p * vScrollBar.LargeChange);
                        }

                        sortedNickNames[selectedIndex].selected = true;
                        currentWindow.GetNick(sortedNickNames[selectedIndex].nick).Selected = true;

                        Invalidate();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Erase the Nicklist and change the Header to the Console
        /// </summary>
        internal void RefreshList()
        {
            this.currentWindow = null;
            UpdateHeader("Console");            
        }

        private void DeSelectNick()
        {
            selectedIndex = -1;
            topIndex = 0;
            
            vScrollBar.Value = 0;
            vScrollBar.Visible = false;

            toolTipNode = -1;
            toolTip.RemoveAll();
        }


        /// <summary>
        /// Refresh the Nicklist with the Current Channel/Query Selected
        /// </summary>
        /// <param name="window"></param>
        internal void RefreshList(IceTabPage page)
        {
            if (page.WindowStyle == IceTabPage.WindowType.Channel)
            {
                if (this.currentWindow != page)
                {
                    DeSelectNick();
                }

                this.currentWindow = page;
                UpdateHeader(page.TabCaption + ":" + page.Nicks.Count);

                int i = 0;
                try
                {
                    if (sortedNickNames != null)
                    {
                        for (i = 0; i < sortedNickNames.Count; i++)
                        {
                            if (sortedNickNames[i].selected == true)
                            {
                                selectedIndex = i;
                                //change the topindex if needed

                                int p = (selectedIndex / vScrollBar.LargeChange);

                                if ((topIndex + vScrollBar.LargeChange) < selectedIndex && vScrollBar.Visible)
                                {
                                    topIndex += (p * vScrollBar.LargeChange);

                                    if ((selectedIndex - vScrollBar.LargeChange) < topIndex)
                                    {
                                        topIndex = selectedIndex;
                                    }
                                }
                                else if ((topIndex > selectedIndex) && vScrollBar.Visible)
                                {
                                    topIndex = (p * vScrollBar.LargeChange);
                                }

                                //see if it is on the last page
                                if (vScrollBar.Visible && (topIndex > (vScrollBar.Maximum - vScrollBar.LargeChange)))
                                {
                                    topIndex = (vScrollBar.Maximum - vScrollBar.LargeChange) + 1;
                                }


                                //calculate the new scrollbar value
                                if (vScrollBar.Visible)
                                {
                                    try
                                    {
                                        vScrollBar.Value = topIndex;
                                    }
                                    catch (Exception)
                                    {
                                       // System.Diagnostics.Debug.WriteLine("2scrollbar error:" + topIndex + ":" + vScrollBar.Maximum);
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    /*
                    System.Diagnostics.Debug.WriteLine(ee.Message + ":" + ee.StackTrace);
                    System.Diagnostics.Debug.WriteLine(sortedNickNames != null);
                    System.Diagnostics.Debug.WriteLine("count:" + sortedNickNames.Count + ":" + i + ":" + topIndex);
                    */ 
                }
            }
            else if (page.WindowStyle == IceTabPage.WindowType.Query)
            {
                this.currentWindow = page;
                UpdateHeader(page.TabCaption);
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
            else if (page.WindowStyle == IceTabPage.WindowType.ChannelList)
            {
                this.currentWindow = page;
                UpdateHeader("Channels (" + page.TotalChannels + ")");
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
            else if (page.WindowStyle == IceTabPage.WindowType.DCCChat)
            {
                this.currentWindow = page;
                UpdateHeader("DCC Chat:" + page.TabCaption);
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
            else if (page.WindowStyle == IceTabPage.WindowType.Window)
            {
                this.currentWindow = page;
                UpdateHeader(page.TabCaption);
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
            else if (page.WindowStyle == IceTabPage.WindowType.DCCFile)
            {
                this.currentWindow = page;
                UpdateHeader(page.TabCaption);
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
            else if (page.WindowStyle == IceTabPage.WindowType.Debug)
            {
                this.currentWindow = page;
                UpdateHeader("Debug");
                if (sortedNickNames != null)
                    sortedNickNames.Clear();
                DeSelectNick();
            }
        }

        /// <summary>
        /// Update the Header of the Nick List
        /// </summary>
        /// <param name="data"></param>
        private void UpdateHeader(string data)
        {
            if (this.InvokeRequired)
            {
                UpdateHeaderDelegate u = new UpdateHeaderDelegate(UpdateHeader);
                this.Invoke(u, new object[] { data });
            }
            else
            {
                headerCaption = data;
                Invalidate();
            }
        }
        /// <summary>
        /// Returns total nicknames in NickList
        /// </summary>
        public int TotalNicks
        {
            get
            {
                if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
                    return currentWindow.Nicks.Count;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Returns the Header of the Nicklist
        /// </summary>
        public string Header
        {
            get
            {
                return headerCaption;
            }
            set
            {
                UpdateHeader(value);
            }
        }
        /// <summary>
        /// Set or return the Background image for the nicklist
        /// </summary>
        internal string BackGroundImage
        {
            get
            {
                return _backgroundImageFile;
            }
            set
            {
                if (value.Length > 0)
                    this._backgroundImage = new Bitmap(value);
                else
                    this._backgroundImage = null;

                this._backgroundImageFile = value;

                Invalidate();
            }
        }

        internal void SetListColors()
        {
            this.panelButtons.BackColor = IrcColor.colors[_parent.IceChatColors.TabbarBackColor];

            this.buttonOp.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonVoice.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonBan.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonInfo.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            
            this.buttonHop.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonQuery.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonKick.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonWhois.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];

            
            this.buttonOp.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonVoice.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonBan.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonInfo.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            
            this.buttonHop.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonQuery.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonKick.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonWhois.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];

        }

        /// <summary>
        /// Toggle showing the nicklist buttons
        /// </summary>
        internal bool ShowNickButtons
        {
            get { return this.panelButtons.Visible; }
            set 
            { 
                this.panelButtons.Visible = value;
                if (this.panelButtons.Visible)
                    this.vScrollBar.Height = this.Height - this.headerHeight - this.panelButtons.Height;
                else
                    this.vScrollBar.Height = this.Height - this.headerHeight;

            }
        }
        
        /// <summary>
        /// Returns the TabWindow which is currently being shown in the Nicklist
        /// </summary>
        internal IceTabPage CurrentWindow
        {
            get
            {
                return this.currentWindow;
            }
            set
            {
                this.currentWindow = value;
            }
        }

        private void buttonOp_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    int totalSelected = 0;
                    string addModes = "";
                    string removeModes = "";
                    string addNicks = "";
                    string removeNicks = "";
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            if (totalSelected <= currentWindow.Connection.ServerSetting.MaxModes)
                            {
                                Nick u = sortedNickNames[i];
                                string nickName = u.nick;
                                for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                                {
                                    if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                        nickName = nickName.Substring(1);
                                }

                                if (u != null)
                                {
                                    totalSelected++;

                                    //check if voiced or not
                                    for (int y = 0; y < u.Level.Length; y++)
                                    {
                                        if (currentWindow.Connection.ServerSetting.StatusModes[0][y] == 'o')
                                        {
                                            if (u.Level[y])
                                            {
                                                removeModes += "o";
                                                removeNicks += " " + nickName;
                                            }
                                            else
                                            {
                                                addModes += "o";
                                                addNicks += " " + nickName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    string totalModes = "";
                    if (addModes.Length > 0)
                        totalModes += "+" + addModes;
                    if (removeModes.Length > 0)
                        totalModes += "-" + removeModes;
                    if (addNicks.Length > 0)
                        totalModes += addNicks;
                    if (removeNicks.Length > 0)
                        totalModes += removeNicks;

                    if (totalModes.Length > 0)
                        _parent.ParseOutGoingCommand(currentWindow.Connection, "/mode " + currentWindow.TabCaption + " " + totalModes);

                }
            }

            _parent.FocusInputBox();
        }

        private void buttonVoice_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    int totalSelected = 0;
                    string addModes = "";
                    string removeModes = "";
                    string addNicks = "";
                    string removeNicks = "";
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            if (totalSelected <= currentWindow.Connection.ServerSetting.MaxModes)
                            {
                                Nick u = sortedNickNames[i];
                                string nickName = u.nick;
                                for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                                {
                                    if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                        nickName = nickName.Substring(1);
                                }

                                if (u != null)
                                {
                                    totalSelected++;

                                    //check if voiced or not
                                    for (int y = 0; y < u.Level.Length; y++)
                                    {
                                        if (currentWindow.Connection.ServerSetting.StatusModes[0][y] == 'v')
                                        {
                                            if (u.Level[y])
                                            {
                                                removeModes += "v";
                                                removeNicks += " " + nickName;                                                                            
                                            } 
                                            else
                                            {
                                                addModes += "v";
                                                addNicks += " " + nickName;                                                                            
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    string totalModes = "";
                    if (addModes.Length > 0)
                        totalModes += "+" + addModes;
                    if (removeModes.Length > 0)
                        totalModes += "-" + removeModes;
                    if (addNicks.Length > 0)
                        totalModes += addNicks;
                    if (removeNicks.Length > 0)
                        totalModes += removeNicks;

                    if (totalModes.Length > 0)
                        _parent.ParseOutGoingCommand(currentWindow.Connection, "/mode " + currentWindow.TabCaption + " " + totalModes);
                }
            }
            
            _parent.FocusInputBox();
        }

        private void buttonQuery_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            Nick u = sortedNickNames[i];
                            string nickName = u.nick;
                            for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                            {
                                if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                    nickName = nickName.Substring(1);
                            }

                            if (!_parent.ChannelBar.WindowExists(_parent.CurrentWindow.Connection, nickName, IceTabPage.WindowType.Query))
                                _parent.AddWindow(_parent.CurrentWindow.Connection, nickName, IceTabPage.WindowType.Query);
                            else
                                _parent.ChannelBar.SelectTab(_parent.GetWindow(currentWindow.Connection, nickName, IceTabPage.WindowType.Query));

                            return;
                        }
                    }
                }
            }
            _parent.FocusInputBox();
        }

        private void buttonHop_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    int totalSelected = 0;
                    string addModes = "";
                    string removeModes = "";
                    string addNicks = "";
                    string removeNicks = "";
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            if (totalSelected <= currentWindow.Connection.ServerSetting.MaxModes)
                            {
                                Nick u = sortedNickNames[i];
                                string nickName = u.nick;
                                for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                                {
                                    if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                        nickName = nickName.Substring(1);
                                }

                                if (u != null)
                                {
                                    totalSelected++;

                                    //check if voiced or not
                                    for (int y = 0; y < u.Level.Length; y++)
                                    {
                                        if (currentWindow.Connection.ServerSetting.StatusModes[0][y] == 'h')
                                        {
                                            if (u.Level[y])
                                            {
                                                removeModes += "h";
                                                removeNicks += " " + nickName;
                                            }
                                            else
                                            {
                                                addModes += "h";
                                                addNicks += " " + nickName;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    string totalModes = "";
                    if (addModes.Length > 0)
                        totalModes += "+" + addModes;
                    if (removeModes.Length > 0)
                        totalModes += "-" + removeModes;
                    if (addNicks.Length > 0)
                        totalModes += addNicks;
                    if (removeNicks.Length > 0)
                        totalModes += removeNicks;

                    if (totalModes.Length > 0)
                        _parent.ParseOutGoingCommand(currentWindow.Connection, "/mode " + currentWindow.TabCaption + " " + totalModes);

                }
            }

            _parent.FocusInputBox();
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            string nickName = sortedNickNames[i].nick;
                            for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                            {
                                if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                    nickName = nickName.Substring(1);
                            }


                            _parent.ParseOutGoingCommand(currentWindow.Connection, "/userinfo " + nickName);
                            
                            _parent.FocusInputBox();
                            return;                            
                        }
                    }
                }
            }
            _parent.FocusInputBox();
        }

        private void buttonBan_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            Nick u = sortedNickNames[i];
                            string nickName = u.nick;
                            for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                            {
                                if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                    nickName = nickName.Substring(1);
                            }

                            if (u.host.Length > 0)
                                _parent.ParseOutGoingCommand(currentWindow.Connection, "/mode " + currentWindow.TabCaption + " +b *!" + u.host);
                            else
                                _parent.ParseOutGoingCommand(currentWindow.Connection, "/mode " + currentWindow.TabCaption + " +b " + nickName); ;

                            _parent.FocusInputBox();
                            return;
                        }
                    }
                }
            }
            _parent.FocusInputBox();
        }

        private void buttonKick_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            string nickName = sortedNickNames[i].nick;
                            for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                            {
                                if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                    nickName = nickName.Substring(1);
                            }


                            _parent.ParseOutGoingCommand(currentWindow.Connection, "/kick " + currentWindow.TabCaption + " " + nickName);

                            _parent.FocusInputBox();
                            return;
                        }
                    }
                }
            }
            _parent.FocusInputBox();
        }

        private void buttonWhois_Click(object sender, EventArgs e)
        {
            if (currentWindow == null) return;

            if (currentWindow.WindowStyle == IceTabPage.WindowType.Channel)
            {
                //check for all the selected nicks in the nick list
                if (selectedIndex < sortedNickNames.Count)
                {
                    for (int i = 0; i < sortedNickNames.Count; i++)
                    {
                        if (sortedNickNames[i].selected == true)
                        {
                            string nickName = sortedNickNames[i].nick;
                            for (int x = 0; x < currentWindow.Connection.ServerSetting.StatusModes[1].Length; x++)
                            {
                                if (nickName.StartsWith(currentWindow.Connection.ServerSetting.StatusModes[1][x].ToString()))
                                    nickName = nickName.Substring(1);
                            }


                            _parent.ParseOutGoingCommand(currentWindow.Connection, "/whois " + nickName + " " + nickName); ;

                            _parent.FocusInputBox();
                            return;
                        }
                    }
                }
            }
            
            _parent.FocusInputBox();
        }

    }
}
