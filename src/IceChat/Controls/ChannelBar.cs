/***********************************\
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

using IceChatPlugin;

namespace IceChat
{
    public partial class ChannelBar : UserControl
    {
        private Font _tabFont = new Font("Verdana", 10F);
        private Dictionary<int, Rectangle> _tabSizeRects = new Dictionary<int, Rectangle>();
        private Dictionary<int, Rectangle> _tabTextRects = new Dictionary<int, Rectangle>();

        private List<IceTabPage> _TabPages = new List<IceTabPage>();

        private int _selectedIndex = -1;
        private int _hoveredIndex = -1;

        private int _TabRowHeight = 30;
        private int _TotalTabRows = 1;

        private int _tabStartXPos = 0;

        private Point _dragStartPosition = Point.Empty;

        //which tab will be dragged
        private IceTabPage _dragTab;
        private ContextMenuStrip _popupMenu;

        //private bool showTabs;
        private bool singleRow;
        private bool showScrollItems = false;
        private int _tabStartXtra = 0;
        private bool leftButtonHover = false;
        private bool rightButtonHover = false;

        private ToolTip toolTip;
        private int toolTipTab = -1;        

        private System.Timers.Timer flashTabTimer;
        private int _flashValue = 0;

        internal delegate void TabEventHandler(object sender, TabEventArgs e);
        internal event TabEventHandler SelectedIndexChanged;

        internal delegate void TabClosedDelegate(int nIndex);
        internal event TabClosedDelegate OnTabClosed;
        private FormMain _parent;
        private bool hideBar;

        public ChannelBar(FormMain parent)
        {
            InitializeComponent();
            this._parent = parent;

            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseLeave += new EventHandler(OnMouseLeave);
            this.MouseUp += new MouseEventHandler(OnMouseUp);

            this.AutoSize = false;

            this.DoubleBuffered = true;

            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            _popupMenu = new ContextMenuStrip();

            flashTabTimer = new System.Timers.Timer();
            flashTabTimer.Interval = 1000;
            flashTabTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnFlashTabTimerElapsed);

            toolTip = new ToolTip();
            toolTip.AutoPopDelay = 3000;
            toolTip.ForeColor = System.Drawing.SystemColors.InfoText;
            toolTip.BackColor = System.Drawing.SystemColors.Info;

        }

        internal bool SingleRow
        {
            get
            {
                return this.singleRow;
            }
            set
            {
                this.singleRow = value;
                this.Invalidate();                
            }
        }

        internal bool HideBar
        {
            get
            {
                return this.hideBar;
            }
            set
            {
                this.hideBar = value;
            }

        }

        internal int TabCount
        {
            get
            {
                return this._TabPages.Count;
            }
        }

        internal Font TabFont
        {
            set
            {
                this._tabFont = value;
                this.Invalidate();
            }
        }

        public int SelectedIndex
        {
            set
            {
                this._selectedIndex = value;

                if (this.SelectedIndexChanged != null)
                {
                    TabEventArgs e = new TabEventArgs();
                    e.IsHandled = true;
                    SelectedIndexChanged(this, e);
                }
            }
            get
            {
                return this._selectedIndex;
            }
        }

        internal IceTabPage CurrentTab
        {
            get
            {
                if (_selectedIndex == -1) _selectedIndex = 0;
                if (_selectedIndex > (_TabPages.Count - 1)) _selectedIndex = 0;
                return _TabPages[_selectedIndex];
            }
        }

        internal List<IceTabPage> TabPages
        {
            get
            {
                return this._TabPages;
            }
        }

        internal void AddTabPage(ref IceTabPage page)
        {
            this._TabPages.Add(page);
        }

        internal void InsertTabPage(int index, ref IceTabPage page)
        {
            this._TabPages.Insert(index, page);
        }

        internal void SortPageTabs()
        {
            this._TabPages.Sort();
        }

        private IceTabPage HoverTab(Point pClickLocation)
        {
            for (int i = 0; i < _tabSizeRects.Count; i++)
            {
                Rectangle rectTab = _tabSizeRects[i];
                if ((pClickLocation.X > rectTab.X && pClickLocation.X < rectTab.X + rectTab.Width) && (pClickLocation.Y > rectTab.Y && pClickLocation.Y < rectTab.Bottom))
                    return GetTabPage(i);
            }
            return null;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_parent != null)
                DrawControl(e.Graphics);
        }


        internal void DrawControl(Graphics g)
        {
            try
            {
                //draw the background
                g.Clear(IrcColor.colors[_parent.IceChatColors.TabbarBackColor]);

                if (this._TabPages.Count == 0) return;

                if (this._TabPages.Count != 0 && _selectedIndex == -1)
                    SelectedIndex = 0;

                if (this._selectedIndex > (_TabPages.Count - 1))
                    SelectedIndex = 0;

                CalculateTabSizes(g);

                if (singleRow && !showScrollItems && _tabStartXtra != 0)
                {
                    //recalculate, we do not need extra space , no buttons
                    _tabStartXtra = 0;
                    CalculateTabSizes(g);
                }
                
                //check that right most tab, is right most
                if (showScrollItems && singleRow)
                {
                    //sometimes a blank space is there
                    Rectangle rectTab = _tabSizeRects[_tabSizeRects.Count-1];

                    if ((rectTab.X + rectTab.Width) < (this.Width - 44))
                    {
                        _tabStartXtra = _tabStartXtra + rectTab.Width;
                        CalculateTabSizes(g);                        
                    }
                }

                bool flashTabs = false;

                //check if we have any flashing tabs
                for (int i = 0; i < _TabPages.Count; i++)
                {
                    if (_TabPages[i].FlashTab == true)
                    {
                        flashTabs = true;
                        break;
                    }
                }

                if (flashTabs == true)
                {
                    //enable the flash tab timer
                    flashTabTimer.Enabled = true;
                    flashTabTimer.Start();
                }
                else
                {
                    //disable the flash tab timer
                    flashTabTimer.Stop();
                    flashTabTimer.Enabled = false;
                }
                
                if (singleRow && showScrollItems)
                {
                    //draw the button for left - right if need be
                    if (Application.RenderWithVisualStyles)
                    {
                        //draw the background for the buttons area
                        System.Windows.Forms.VisualStyles.VisualStyleRenderer leftButton;
                        System.Windows.Forms.VisualStyles.VisualStyleRenderer rightButton;

                        if (leftButtonHover)
                            leftButton = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar.ArrowButton.LeftHot);
                        else
                            leftButton = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar.ArrowButton.LeftNormal);

                        Rectangle leftRect = new Rectangle(this.Width - 44, 0, 22, 22);
                        leftButton.DrawBackground(g, leftRect);

                        if (rightButtonHover)
                            rightButton = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar.ArrowButton.RightHot);
                        else
                            rightButton = new System.Windows.Forms.VisualStyles.VisualStyleRenderer(System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar.ArrowButton.RightNormal);

                        Rectangle rightRect = new Rectangle(this.Width - 22, 0, 22, 22);
                        rightButton.DrawBackground(g, rightRect);

                    }
                    //limit drawing area
                    g.Clip = new Region(new Rectangle(0, 0, this.Width - 44, this.Height));
                }

                for (int i = 0; i < _TabPages.Count; i++)
                {
                    if (!hideBar)
                        DrawTab(g, _TabPages[i], i);
                    else
                    {
                        Graphics gg = this.CreateGraphics();                        
                        DrawTab(gg, _TabPages[i], i);
                        gg.Dispose();
                    }
                }

            }
            catch (Exception)
            {
                //System.Diagnostics.Debug.WriteLine("IceChatControl DrawControl Error:" + e.Message);
                //_parent.WriteErrorFile(_parent.InputPanel.CurrentConnection, "IceTabControl DrawControl", e);
            }
        }

        internal void DrawTab(Graphics g, IceTabPage tabPage, int nIndex)
        {
            try
            {
                Rectangle recBounds = _tabSizeRects[nIndex];
                Rectangle tabTextArea = _tabTextRects[nIndex];

                Brush brBack;
                Point[] pt;

                bool bSelected = (this._selectedIndex == nIndex);
                bool bHovered = (this._hoveredIndex == nIndex);

                if (bSelected)
                    brBack = new LinearGradientBrush(recBounds, IrcColor.colors[_parent.IceChatColors.TabBarCurrentBG1], IrcColor.colors[_parent.IceChatColors.TabBarCurrentBG2], 90);
                else if (bHovered)
                    brBack = new LinearGradientBrush(recBounds, IrcColor.colors[_parent.IceChatColors.TabBarHoverBG1], IrcColor.colors[_parent.IceChatColors.TabBarHoverBG2], 90);
                else
                    brBack = new LinearGradientBrush(recBounds, IrcColor.colors[_parent.IceChatColors.TabBarOtherBG1], IrcColor.colors[_parent.IceChatColors.TabBarOtherBG2], 90);

                pt = new Point[7];
                pt[0] = new Point(recBounds.Left + 1, recBounds.Bottom);
                if (bSelected)
                {
                    pt[1] = new Point(recBounds.Left + 1, recBounds.Top + 3);
                    pt[2] = new Point(recBounds.Left + 4, recBounds.Top);
                    pt[3] = new Point(recBounds.Right - 4, recBounds.Top);
                    pt[4] = new Point(recBounds.Right - 1, recBounds.Top + 3);
                }
                else
                {
                    pt[1] = new Point(recBounds.Left + 1, recBounds.Top + 6);
                    pt[2] = new Point(recBounds.Left + 4, recBounds.Top + 3);
                    pt[3] = new Point(recBounds.Right - 4, recBounds.Top + 3);
                    pt[4] = new Point(recBounds.Right - 1, recBounds.Top + 6);
                }
                pt[5] = new Point(recBounds.Right - 1, recBounds.Bottom);
                pt[6] = new Point(recBounds.Left + 1, recBounds.Bottom);

                //paint the background
                if (!tabPage.PinnedTab)
                {
                    g.FillPolygon(brBack, pt);
                    // draw the border around the control
                    g.DrawPolygon(new Pen(Color.Black, 1), pt);
                }

                Image img = null;

                switch (tabPage.WindowStyle)
                {
                    case IceTabPage.WindowType.Console:
                        img = StaticMethods.LoadResourceImage("console.png");
                        break;
                    case IceTabPage.WindowType.Channel:
                        img = StaticMethods.LoadResourceImage("channel.png");
                        break;
                    case IceTabPage.WindowType.Query:
                    case IceTabPage.WindowType.DCCChat:
                        img = StaticMethods.LoadResourceImage("query.png");
                        break;
                    case IceTabPage.WindowType.ChannelList:
                        img = StaticMethods.LoadResourceImage("channellist.png");
                        break;
                    case IceTabPage.WindowType.Window:
                    case IceTabPage.WindowType.Debug:
                        img = StaticMethods.LoadResourceImage("window-icon.ico");
                        break;
                    default:
                        img = StaticMethods.LoadResourceImage("window-icon.ico");
                        break;

                }

                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Center;

                Brush br = null;

                //get the tab text color
                if (bSelected)
                {
                    if (tabPage.PinnedTab)
                        br = brBack;
                    else
                        br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarCurrent]);                    

                    tabPage.LastMessageType = FormMain.ServerMessageType.Default;
                }
                else
                {
                    switch (tabPage.LastMessageType)
                    {
                        case FormMain.ServerMessageType.JoinChannel:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarChannelJoin]);
                            break;
                        case FormMain.ServerMessageType.PartChannel:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarChannelPart]);
                            break;
                        case FormMain.ServerMessageType.Message:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarNewMessage]);
                            break;
                        case FormMain.ServerMessageType.Action:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarNewAction]);
                            break;
                        case FormMain.ServerMessageType.QuitServer:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarServerQuit]);
                            break;
                        case FormMain.ServerMessageType.ServerMessage:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarServerMessage]);
                            break;
                        case FormMain.ServerMessageType.ServerNotice:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarServerNotice]);
                            break;
                        case FormMain.ServerMessageType.BuddyNotice:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarBuddyNotice]);
                            break;
                        case FormMain.ServerMessageType.Other:
                            br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarOtherMessage]);
                            break;
                        case FormMain.ServerMessageType.CustomMessage:
                            br = new SolidBrush(IrcColor.colors[tabPage.CustomForeColor]);
                            break;
                        default:
                            if (tabPage.PinnedTab)
                                br = brBack;
                            else
                                br = new SolidBrush(IrcColor.colors[_parent.IceChatColors.TabBarDefault]);                            
                            break;
                    }
                }

                if (!tabPage.PinnedTab)
                {
                    if (tabPage.WindowStyle != IceTabPage.WindowType.Console)
                    {
                        if (tabPage.WindowStyle != IceTabPage.WindowType.ChannelList)
                            g.DrawString(tabPage.TabCaption, _tabFont, br, tabTextArea, stringFormat);
                        else
                            g.DrawString(tabPage.TabCaption + "(" + tabPage.TotalChannels + ")", _tabFont, br, tabTextArea, stringFormat);
                    }
                    else
                    {
                        g.DrawString(_parent.IceChatLanguage.consoleTabTitle, _tabFont, br, tabTextArea, stringFormat);
                    }
                }
                else
                {
                    //pinned tab
                    g.FillPolygon(br, pt);
                    // draw the border around the control
                    g.DrawPolygon(new Pen(Color.Black, 1), pt);
                }
                
                brBack.Dispose();
                br.Dispose();
                
                //draw the icon last now
                Rectangle rimage = new Rectangle(recBounds.X, recBounds.Y, img.Width, img.Height);

                int imgY = ((recBounds.Bottom - recBounds.Top) - img.Height) / 2;
                int imgX = 4;
                if (tabPage.PinnedTab)
                    imgX = 6;

                tabPage.FlashValue = _flashValue;

                if (bSelected)
                {
                    //set the image in the verical center of the tab
                    rimage.Offset(imgX, imgY);
                    g.DrawImage(img, rimage);

                    //disable flashing, since it is the current page
                    tabPage.FlashTab = false;
                }
                else
                {
                    rimage.Offset(imgX, imgY + 2);
                    if (tabPage.FlashTab == true)
                    {                        
                        if (tabPage.FlashValue == 1)
                            g.DrawImage(img, rimage);
                    }
                    else
                        g.DrawImage(img, rimage);
                }
            }
            catch (Exception) { }
        }


        private void CalculateTabSizes(Graphics g)
        {
            try
            {
                _tabSizeRects.Clear();
                _tabTextRects.Clear();

                _TotalTabRows = 1;

                int totalWidth = 0;
                int xPos = _tabStartXPos;
                
                //add extra to start position if needed 
                if (singleRow)
                    xPos += _tabStartXtra;

                int yPos = 0;
                
                showScrollItems = false;

                _TabRowHeight = (int)g.MeasureString("0", _tabFont).Height + 4;

                if ((_TabRowHeight / 2) * 2 == _TabRowHeight)
                    _TabRowHeight++;

                for (int i = 0; i < _TabPages.Count; i++)
                {
                    Rectangle recBounds = new Rectangle();
                    Rectangle recTextArea = new Rectangle();

                    //caclulate the width of the text
                    int textWidth = (int)g.MeasureString(_TabPages[i].TabCaption, _tabFont).Width;
                    if (_TabPages[i].WindowStyle == IceTabPage.WindowType.ChannelList)
                    {
                        textWidth += (int)g.MeasureString(" (" + _TabPages[i].TotalChannels + ") ", _tabFont).Width;
                    }
                    
                    if (_TabPages[i].PinnedTab)
                    {
                        //icon only
                        recBounds.Width = 28;
                        recTextArea.Width = 0;
                    }
                    else
                    {
                        recBounds.Width = textWidth + 26;
                        recTextArea.Width = textWidth + 1;
                    }
                    
                    recBounds.Height = _TabRowHeight + 5;
                    recTextArea.Height = (int)g.MeasureString(_TabPages[i].TabCaption, _tabFont).Height + 10;
                    
                    //check if we should go to the next row
                    if (totalWidth > 0 && ((totalWidth + recBounds.Width) > (this.Width)))
                    {
                        totalWidth = 0;
                        if (singleRow == false)
                        {
                            _TotalTabRows++;
                            xPos = _tabStartXPos;
                            yPos = yPos + _TabRowHeight + 5;
                        }
                        else
                            showScrollItems = true;
                    }

                    recBounds.X = xPos;
                    recBounds.Y = yPos;
                
                    recTextArea.X = xPos + 21;  //add area for image and a little extra
                    recTextArea.Y = yPos;
                    
                    if (i != this._selectedIndex)
                        recTextArea.Y = yPos + 2;

                    _tabSizeRects.Add(i, recBounds);
                    _tabTextRects.Add(i, recTextArea);

                    xPos = xPos + recBounds.Width;
                    totalWidth = totalWidth + recBounds.Width;
                }
                if (this.hideBar)
                    this.Height = 1;
                else
                    this.Height = (_TabRowHeight + 6) * _TotalTabRows;

            }
            catch (Exception)
            {
                //_parent.WriteErrorFile(_parent.InputPanel.CurrentConnection, "CalculateTabSizes", e);
            }
        }

        internal void SelectTab(IceTabPage page)
        {
            try
            {
                
                _parent.TabMain.BringFront(page);
                
                //System.Diagnostics.Debug.WriteLine("SelectTab ChannelBar:" + page.TabCaption);

                for (int i = 0; i < _TabPages.Count; i++)
                {
                    if (_TabPages[i] == page)
                    {
                        SelectedIndex = i;

                        //for single row.. scroll into view. if needed
                        checkTabLocation(i);

                        this.Invalidate();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                _parent.WriteErrorFile(_parent.InputPanel.CurrentConnection, "ChannelBar SelectTab:" , ex);

            }
        }

        private void checkTabLocation(int i)
        {
            if (singleRow && i < _tabSizeRects.Count)
            {
                Rectangle rectTab = _tabSizeRects[i];
                int width = (rectTab.X + rectTab.Width);
                int checkWidth = this.Width;
                if (showScrollItems)
                    checkWidth = checkWidth - 44;

                if (width > checkWidth)
                {
                    //scroll into view
                    //how much is it out of view
                    int diff = checkWidth - width + _tabStartXtra;

                    _tabStartXtra = diff;

                }
                else if (rectTab.X < 0)
                {
                    //out of view on the left
                    _tabStartXtra = _tabStartXtra - rectTab.X;
                }
            }

        }

        private IceTabPage setSelectedByClickLocation(Point pClickLocation)
        {
            if (_tabSizeRects.Count == 0) return null;

            for (int i = 0; i < _tabSizeRects.Count; i++)
            {
                Rectangle rectTab = _tabSizeRects[i];
                if ((pClickLocation.X > rectTab.X && pClickLocation.X < rectTab.X + rectTab.Width) && (pClickLocation.Y > rectTab.Y && pClickLocation.Y < rectTab.Bottom))
                {
                    if (this._selectedIndex != i)
                    {
                        this._selectedIndex = i;

                        checkTabLocation(i);
                    }

                    break;
                }
            }

            if (GetTabPage(_selectedIndex) != null)
            {
                if (this.SelectedIndexChanged != null)
                {
                    _parent.TabMain.BringFront(_TabPages[_selectedIndex]);

                    TabEventArgs e = new TabEventArgs();
                    SelectedIndexChanged(this, e);
                }
            }

            this.Invalidate();

            return GetTabPage(_selectedIndex);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_tabSizeRects.Count == 0)
                return;

            int i = 0;

            try
            {

                if (singleRow && showScrollItems)
                {
                    //check if we are over next or previous buttons
                    if (e.X >= (this.Width - 44))
                    {
                        _hoveredIndex = -1;
                        if (e.X > (this.Width - 22))
                        {
                            //right/next button
                            rightButtonHover = true;
                            leftButtonHover = false;
                        }
                        else
                        {
                            //left/prev button
                            rightButtonHover = false;
                            leftButtonHover = true;
                        }

                        this.Invalidate();
                        return;
                    }
                    rightButtonHover = false;
                    leftButtonHover = false;
                }

                //show the tooltip
                if (e.Button == MouseButtons.Left)
                {
                    Rectangle r = new Rectangle(_dragStartPosition, Size.Empty);

                    r.Inflate(SystemInformation.DragSize);

                    if (_dragTab != null)
                    {
                        if (!r.Contains(e.X, e.Y))
                        {
                            IceTabPage hover_Tab = HoverTab(e.Location);
                            if (hover_Tab != null)
                            {
                                SwapTabPages(_dragTab, hover_Tab);
                                _dragTab = setSelectedByClickLocation(e.Location);
                                this.Invalidate();
                            }
                        }
                    }

                    _dragStartPosition = Point.Empty;
                    return;
                }

                if (e.Y < _tabSizeRects[0].Y + 3 || e.Y > _tabSizeRects[_tabSizeRects.Count - 1].Y + _tabSizeRects[0].Height)
                {
                    _hoveredIndex = -1;
                    this.Invalidate();
                    return;
                }
                
                int iHoveredIndexBeforeClick = _hoveredIndex;

                for (i = 0; i < _tabSizeRects.Count; i++)
                {
                    Rectangle rectTab = _tabSizeRects[i];
                    if ((e.X > rectTab.X && e.X < (rectTab.X + rectTab.Width)) && (e.Y > rectTab.Y && e.Y < (rectTab.Y + rectTab.Height)))
                    {
                        if (this._hoveredIndex != i)
                            this._hoveredIndex = i;

                        //show the tooltip
                        if (toolTipTab != i)
                        {
                            if (_TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                            {
                                toolTip.ToolTipTitle = _TabPages[i].TabCaption;
                                toolTip.SetToolTip(this, "{" + _TabPages[i].Nicks.Count + "} " + "[" + _TabPages[i].ChannelModes + "] {" + _TabPages[i].Connection.ServerSetting.NetworkName + "}");
                            }
                            else if (_TabPages[i].WindowStyle == IceTabPage.WindowType.Query || _TabPages[i].WindowStyle == IceTabPage.WindowType.DCCChat)
                            {
                                toolTip.ToolTipTitle = "User Information";
                                toolTip.SetToolTip(this, _TabPages[i].TabCaption);
                            }
                            else if (_TabPages[i].WindowStyle == IceTabPage.WindowType.Debug || _TabPages[i].WindowStyle == IceTabPage.WindowType.Window || _TabPages[i].WindowStyle == IceTabPage.WindowType.ChannelList || _TabPages[i].WindowStyle == IceTabPage.WindowType.DCCFile)
                            {
                                toolTip.ToolTipTitle = "";
                                toolTip.SetToolTip(this, _TabPages[i].TabCaption);
                            }
                            else if (_TabPages[i].WindowStyle == IceTabPage.WindowType.Console)
                            {
                                int x = 0;
                                foreach (IRCConnection c in _parent.ServerTree.ServerConnections.Values)
                                {
                                    if (c.IsConnected)
                                    {
                                        x++;
                                    }
                                }
                                toolTip.ToolTipTitle = "Console";
                                toolTip.SetToolTip(this, x.ToString() + " servers connected");
                            }

                            toolTipTab = i;
                        }

                        break;
                    }
                }

                if (_hoveredIndex == iHoveredIndexBeforeClick)
                    return;

                this.Invalidate();
            }
            catch (Exception)
            {
                //_parent.WriteErrorFile(_parent.InputPanel.CurrentConnection, "ChannelBar MouseMove:" + e.Y + ":" + e.X + ":i=" + i + ":HoverIndex=" + _hoveredIndex + ":TabRectCount=" + _tabSizeRects.Count + ":TabCount=" + _TabPages.Count + ":" + ex.Message, ex);
            }

        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            _hoveredIndex = -1;
            toolTipTab = -1;

            rightButtonHover = false;
            leftButtonHover = false;
            
            this.Invalidate();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && singleRow && showScrollItems)
            {
                //check if we have clicked next or previous
                if (e.X >= (this.Width - 44))
                {
                    if (e.X > (this.Width - 22))
                    {
                        //right/next button
                        if (_selectedIndex == _TabPages.Count-1)
                            return;

                        _selectedIndex++;
                        SelectTab(GetTabPage(_selectedIndex));
                    }
                    else
                    {
                        //left/prev button
                        //what position are we
                        if (_selectedIndex == 0)
                            return; //no need to scroll left -all the way there

                        _selectedIndex--;
                        SelectTab(GetTabPage(_selectedIndex));
                    }

                    TabEventArgs te = new TabEventArgs();
                    te.IsHandled = false;
                    SelectedIndexChanged(this, te);

                    return;
                }
            }

            _dragStartPosition = new Point(e.X, e.Y);
            _dragTab = setSelectedByClickLocation(e.Location);

            if (e.Button == MouseButtons.Middle)
            {
                if (_dragTab != null)
                {
                    if (this.OnTabClosed != null)
                        OnTabClosed(SelectedIndex);
                }
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //show the proper popup menu according to what kind of tab
                if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Console)
                {
                    //console tab
                    _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                    _popupMenu.Items.Clear();

                    if (GetTabPage(_selectedIndex).PinnedTab)
                        _popupMenu.Items.Add(NewMenuItem("Unpin Tab","/unpin $1"));
                    else
                        _popupMenu.Items.Add(NewMenuItem("Pin Tab", "/pin $1"));

                    _popupMenu.Items.Add(new ToolStripSeparator());

                    _popupMenu.Items.Add(NewMenuItem("Clear", "/clear $1"));
                    _popupMenu.Items.Add(NewMenuItem("Clear All", "/clear all console"));
                    _popupMenu.Items.Add(new ToolStripSeparator());
                    _popupMenu.Items.Add(NewMenuItem("Quit Server", "//quit"));
                    _popupMenu.Items.Add(NewMenuItem("Auto Join", "/autojoin"));
                    _popupMenu.Items.Add(NewMenuItem("Auto Perform", "/autoperform"));

                    AddPopupMenu("Console", _popupMenu);

                    //add dynamic menu items
                    if (_parent != null && _parent.LoadedPlugins != null)
                    {
                        foreach (Plugin p in _parent.LoadedPlugins)
                        {
                            IceChatPlugin ipc = p as IceChatPlugin;
                            if (ipc != null)
                            {
                                if (ipc.plugin.Enabled == true)
                                {
                                    ToolStripItem[] popServer = ipc.plugin.AddServerPopups();
                                    if (popServer != null && popServer.Length > 0)
                                    {
                                        _popupMenu.Items.AddRange(popServer);
                                    }
                                }
                            }
                        }
                    }
                    
                    _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                    _popupMenu.Show(this, e.Location);
                }
                else
                {

                    if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Channel)
                    {
                        _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Items.Clear();

                        if (GetTabPage(_selectedIndex).PinnedTab)
                            _popupMenu.Items.Add(NewMenuItem("Unpin Tab", "/unpin $1"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Pin Tab", "/pin $1"));

                        if (GetTabPage(_selectedIndex).Detached)
                            _popupMenu.Items.Add(NewMenuItem("Attach Tab", "/attach"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Detach Tab", "/detach"));

                        _popupMenu.Items.Add(new ToolStripSeparator());

                        _popupMenu.Items.Add(NewMenuItem("Clear Window", "/clear $1"));
                        _popupMenu.Items.Add(NewMenuItem("Close Channel", "/part $1"));
                        _popupMenu.Items.Add(NewMenuItem("Rejoin Channel", "/hop $1"));
                        _popupMenu.Items.Add(NewMenuItem("Toggle Autojoin Channel", "/autojoin $1"));
                        _popupMenu.Items.Add(NewMenuItem("Channel Information", "/chaninfo $1"));
                        _popupMenu.Items.Add(NewMenuItem("Change Font", "/font $1"));


                        ToolStripMenuItem colorMode = new ToolStripMenuItem("No Color Mode");
                        colorMode.Tag = "/colormode $1";
                        colorMode.Checked = GetTabPage(_selectedIndex).TextWindow.NoColorMode;
                        
                        ToolStripMenuItem log = new ToolStripMenuItem("Logging");
                        ToolStripMenuItem openLog = new ToolStripMenuItem("Open Log Folder");
                        openLog.Tag = "/run " + GetTabPage(_selectedIndex).TextWindow.LogFileLocation;
                        ToolStripMenuItem disableLog = new ToolStripMenuItem("Disable Logging");
                        disableLog.Tag = "/logging $1";

                        //add a way to disable channel log reload                        
                        log.DropDownItems.Add(openLog);
                        log.DropDownItems.Add(disableLog);
                        
                        ToolStripMenuItem events = new ToolStripMenuItem("Events");
                        ToolStripMenuItem disableFlash = new ToolStripMenuItem("Disable Flashing/Color Changes");
                        disableFlash.Tag = "/flashing $1";
                        disableFlash.Checked = GetTabPage(_selectedIndex).EventOverLoad;

                        ToolStripMenuItem disableSounds = new ToolStripMenuItem("Disable Sounds");
                        disableSounds.Tag = "/sounds $1";
                        disableSounds.Checked = GetTabPage(_selectedIndex).DisableSounds;

                        events.DropDownItems.Add(disableFlash);
                        events.DropDownItems.Add(disableSounds);

                        colorMode.Click += new EventHandler(OnPopupExtraMenuClick);
                        disableSounds.Click += new EventHandler(OnPopupExtraMenuClick);
                        disableFlash.Click += new EventHandler(OnPopupExtraMenuClick);
                        disableLog.Click += new EventHandler(OnPopupExtraMenuClick);
                        openLog.Click += new EventHandler(OnPopupExtraMenuClick);                        

                        _popupMenu.Items.Add(colorMode);
                        _popupMenu.Items.Add(log);
                        _popupMenu.Items.Add(events);

                        //add dynamic menu items                        
                        if (_parent != null && _parent.LoadedPlugins != null)
                         {
                            foreach (Plugin p in _parent.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                    {
                                        ToolStripItem[] popChan = ipc.plugin.AddChannelPopups();
                                        if (popChan != null && popChan.Length > 0)
                                        {
                                            _popupMenu.Items.AddRange(popChan);
                                        }
                                    }
                                }
                            }
                         }

                         //add then channel popup menu
                         AddPopupMenu("Channel", _popupMenu);

                        _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Show(this, e.Location);

                    }
                    else if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Query)
                    {
                        _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Items.Clear();

                        if (GetTabPage(_selectedIndex).PinnedTab)
                            _popupMenu.Items.Add(NewMenuItem("Unpin Tab", "/unpin $1"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Pin Tab", "/pin $1"));

                        if (GetTabPage(_selectedIndex).Detached)
                            _popupMenu.Items.Add(NewMenuItem("Attach Tab", "/attach"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Detach Tab", "/detach"));

                        _popupMenu.Items.Add(new ToolStripSeparator());

                        _popupMenu.Items.Add(NewMenuItem("Clear Query", "/clear $1"));
                        _popupMenu.Items.Add(NewMenuItem("Close Query", "/part $1"));
                        _popupMenu.Items.Add(NewMenuItem("User Information", "/userinfo $1"));
                        _popupMenu.Items.Add(NewMenuItem("Silence User", "/silence +$1"));

                        if (_parent != null && _parent.LoadedPlugins != null)
                        {
                            foreach (Plugin p in _parent.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                    {
                                        ToolStripItem[] popServer = ipc.plugin.AddQueryPopups();
                                        if (popServer != null && popServer.Length > 0)
                                        {
                                            _popupMenu.Items.AddRange(popServer);
                                        }
                                    }
                                }
                            }
                        }

                        //add then channel popup menu
                        AddPopupMenu("Query", _popupMenu);

                        _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Show(this, e.Location);
                    }
                    else if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.ChannelList)
                    {
                        _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Items.Clear();
                        _popupMenu = ChannelListPopupMenu();
                        _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Show(this, e.Location);
                    }
                    else if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.DCCChat)
                    {
                        _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Items.Clear();
                        _popupMenu = DCCChatPopupMenu();
                        _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Show(this, e.Location);
                    }
                    else if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Window || GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Debug)
                    {
                        _popupMenu.ItemClicked -= new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Items.Clear();

                        if (GetTabPage(_selectedIndex).PinnedTab)
                            _popupMenu.Items.Add(NewMenuItem("Unpin Tab", "/unpin $1"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Pin Tab", "/pin $1"));

                        if (GetTabPage(_selectedIndex).Detached)
                            _popupMenu.Items.Add(NewMenuItem("Attach Tab", "/attach"));
                        else
                            _popupMenu.Items.Add(NewMenuItem("Detach Tab", "/detach"));

                        _popupMenu.Items.Add(new ToolStripSeparator());

                        _popupMenu.Items.Add(NewMenuItem("Clear Window", "/clear $1"));
                        _popupMenu.Items.Add(NewMenuItem("Close Window", "/close $1"));

                        if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Debug)
                        {
                            _popupMenu.Items.Add(new ToolStripSeparator());

                            //add extra items
                            foreach (IRCConnection c in _parent.ServerTree.ServerConnections.Values)
                            {
                                if (c.IsConnected)
                                {
                                    ToolStripMenuItem t = new ToolStripMenuItem();
                                    if (c.ServerSetting.RealServerName.Length > 0)
                                        t.Text = c.ServerSetting.RealServerName;
                                    else
                                        t.Text = c.ServerSetting.ServerName;

                                    if (c.ShowDebug)
                                    {
                                        t.Tag = "/debug disable " + +c.ServerSetting.ID;
                                        t.Checked = true;
                                        _popupMenu.Items.Add(t);
                                    }
                                    else
                                    {
                                        t.Tag = "/debug enable " + +c.ServerSetting.ID;
                                        _popupMenu.Items.Add(t);
                                    }
                                }
                            }
                        }
                        _popupMenu.ItemClicked += new ToolStripItemClickedEventHandler(OnPopupMenu_ItemClicked);
                        _popupMenu.Show(this, e.Location);
                    }
                }
            }

            _dragTab = null;
            _parent.FocusInputBox();
        }

        private void SwapTabPages(IceTabPage drag, IceTabPage hover)
        {
            int Index1 = _TabPages.IndexOf(drag);
            int Index2 = _TabPages.IndexOf(hover);

            if (Index1 == Index2) return;

            _TabPages.Remove(drag);
            _TabPages.Insert(Index2, drag);

        }


        private void OnPopupMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //send the command to the proper window
            if (e.ClickedItem.Tag == null) return;

            string command = e.ClickedItem.Tag.ToString();
            
            ((ContextMenuStrip)(sender)).Close();

            if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Console)
            {
                //a console command, find out which is the current tab
                command = command.Replace("$1", "Console");
                _parent.ParseOutGoingCommand(GetTabPage("Console").CurrentConnection, command);
            }
            else
            {
                IceTabPage t = _parent.ChannelBar.TabPages[_selectedIndex];
                if (t != null)
                {
                    command = command.Replace("$1", t.TabCaption);
                    command = command.Replace("$chan", t.TabCaption);
                    _parent.ParseOutGoingCommand(t.Connection, command);
                }
            }
        }

        private ContextMenuStrip DCCChatPopupMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(NewMenuItem("Clear Window", "/clear $1"));
            menu.Items.Add(NewMenuItem("Close Window", "/close $1"));
            menu.Items.Add(NewMenuItem("Disconnect", "/disconnect $1"));

            return menu;
        }

        private ContextMenuStrip ChannelListPopupMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(NewMenuItem("Close Window", "/close $1"));
            menu.Items.Add(NewMenuItem("Export List", "/export"));
            AddPopupMenu("ChannelList", menu);
            return menu;
        }

        private ToolStripMenuItem NewMenuItem(string caption, string command)
        {
            ToolStripMenuItem t = new ToolStripMenuItem(caption);
            t.Tag = command;
            return t;
        }

        internal IceTabPage GetTabPage(string sCaption)
        {
            for (int i = 0; i < _TabPages.Count; i++)
            {
                if (_TabPages[i].TabCaption.Equals(sCaption))
                    return _TabPages[i];
            }
            
            return null;
        }
        //just return for channel - tab order
        internal int GetTabPage(string sCaption, IRCConnection connection)
        {
            for (int i = 0; i < _TabPages.Count; i++)
            {
                if (_TabPages[i].TabCaption.ToLower() == sCaption && _TabPages[i].Connection == connection && _TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                    return i;
            }
            
            return 0;
        }

        internal IceTabPage GetTabPage(int iTabIndex)
        {
            if (iTabIndex < _TabPages.Count)
                return _TabPages[iTabIndex];
            
            return null;
        }

        internal void CloseCurrentTab()
        {
            IceTabPage current = GetTabPage(_selectedIndex);
            if (current != null)
            {

                if (current.WindowStyle == IceTabPage.WindowType.Console)
                {
                    return;
                }
                
                if (this.OnTabClosed != null)
                    OnTabClosed(SelectedIndex);

                _TabPages.Remove(current);

                this.Invalidate();
            }
        }


        private void AddPopupMenu(string PopupType, ContextMenuStrip mainMenu)
        {
            //add the console menu popup
            if (_parent == null) return;

            if (_parent.IceChatPopupMenus == null) return;

            foreach (PopupMenuItem p in _parent.IceChatPopupMenus.listPopups)
            {
                if (p.PopupType == PopupType && p.Menu.Length > 0)
                {
                    //add a break
                    ToolStripItem sep = new ToolStripSeparator();
                    mainMenu.Items.Add(sep);

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
                            //parse out $identifiers
                            IceTabPage tw = null;
                            if (GetTabPage(_selectedIndex).WindowStyle != IceTabPage.WindowType.Console)
                            {
                                tw = GetTabPage(_selectedIndex);
                            }

                            if (p.PopupType == "Channel")
                            {
                                if (tw != null)
                                {
                                    command = command.Replace("$chanlogdir", tw.TextWindow.LogFileLocation);
                                    caption = caption.Replace("$chan", tw.TabCaption);
                                    command = command.Replace("$chan", tw.TabCaption);
                                    caption = caption.Replace("$1", tw.TabCaption);
                                    command = command.Replace("$1", tw.TabCaption);

                                }
                            }

                            if (p.PopupType == "Query")
                            {
                                if (tw != null)
                                {
                                    caption = caption.Replace("$nick", tw.TabCaption);
                                    command = command.Replace("$nick", tw.TabCaption);
                                    caption = caption.Replace("$1", tw.TabCaption);
                                    command = command.Replace("$1", tw.TabCaption);
                                    command = command.Replace("$querylogdir", tw.TextWindow.LogFileLocation);
                                }
                            }

                            if (p.PopupType == "ChannelList")
                            {
                                if (tw != null)
                                {
                                    caption = caption.Replace("$1", tw.WindowStyle.ToString());
                                    command = command.Replace("$1", tw.WindowStyle.ToString());
                                }
                            }

                            if (caption == "-")
                                t = new ToolStripSeparator();
                            else
                            {
                                t = new ToolStripMenuItem(caption);

                                t.Click += new EventHandler(OnPopupExtraMenuClick);
                                t.Tag = command;
                            }

                            if (menuDepth == 0)
                                subMenu = mainMenu.Items.Add(t);
                            else
                            {
                                if (mainMenu.Items[subMenu].GetType() == typeof(ToolStripMenuItem))
                                    ((ToolStripMenuItem)mainMenu.Items[subMenu]).DropDownItems.Add(t);
                            }

                            t = null;
                        }
                    }
                }
            }
        }

        private void OnPopupExtraMenuClick(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null) return;

            string command = ((ToolStripMenuItem)sender).Tag.ToString();

            if (GetTabPage(_selectedIndex).WindowStyle == IceTabPage.WindowType.Console)
            {
                //a console command, find out which is the current tab
                command = command.Replace("$1", "Console");
                System.Diagnostics.Debug.WriteLine("Parse1:" + command);
                _parent.ParseOutGoingCommand(GetTabPage("Console").CurrentConnection, command);
            }
            else
            {
                IceTabPage t = GetTabPage(_selectedIndex);
                if (t != null)
                {
                    command = command.Replace("$1", t.TabCaption);
                    System.Diagnostics.Debug.WriteLine("Parse2:" + command);
                    _parent.ParseOutGoingCommand(t.Connection, command);
                }
            }
        }

        private void OnFlashTabTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //syncs all the flashing
            _flashValue++;
            if (_flashValue == 2)
                _flashValue = 0;

            this.Invalidate();
            _parent.ServerTree.Invalidate();
        }

        internal bool WindowExists(IRCConnection connection, string windowName, IceTabPage.WindowType windowType)
        {
            foreach (IceTabPage t in this._TabPages)
            {
                if (t.Connection == null)
                {
                    if (t.WindowStyle == IceTabPage.WindowType.DCCFile)
                    {
                        if (t.TabCaption.ToLower() == windowName.ToLower())
                            return true;
                    }
                }
                else if (t.Connection == connection)
                {
                    if (t.WindowStyle == windowType)
                    {
                        if (t.TabCaption.ToLower() == windowName.ToLower())
                            return true;
                    }
                }
            }
            return false;
        }
    }

    internal class TabEventArgs : System.EventArgs
    {
        public bool IsHandled;
    }
}