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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using IceChatPlugin;

namespace IceChat
{
    public partial class ServerTree : UserControl
    {

        private FormServers f;
        private SortedList serverConnections;

        private IceChatServers serversCollection;
        private int topIndex = 0;
        private int headerHeight = 23;
        private int selectedNodeIndex = 0;
        private int selectedServerID = 0;

        private ServerSetting selectedDragID = null;

        private string headerCaption = "";
        private ToolTip toolTip;
        private int toolTipNode = -1;

        private List<KeyValuePair<string,object>> serverNodes;
        private System.Timers.Timer flashTabTimer;

        internal event NewServerConnectionDelegate NewServerConnection;

        internal delegate void SaveDefaultDelegate();
        internal event SaveDefaultDelegate SaveDefault;

        private Bitmap _backgroundImage = null;
        private string _backgroundImageFile;
        private FormMain _parent;

        private bool mouseFocus = false;

        public ServerTree(FormMain parent)
        {
            InitializeComponent();
            this._parent = parent;

            if (_parent == null)
                return;

            headerCaption = "Favorite Servers";
            
            this.MouseUp += new MouseEventHandler(OnMouseUp);
            this.MouseDown += new MouseEventHandler(OnMouseDown);
            this.MouseMove += new MouseEventHandler(OnMouseMove);
            this.MouseWheel += new MouseEventHandler(OnMouseWheel);
            this.DoubleClick += new EventHandler(OnDoubleClick);
            this.FontChanged += new EventHandler(OnFontChanged);
            this.Resize += new EventHandler(OnResize);
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            this.panelButtons.Resize += new EventHandler(PanelButtons_Resize);
            this.panelButtons.VisibleChanged += new EventHandler(PanelButtons_VisibleChanged);
            this.vScrollBar.Scroll += new ScrollEventHandler(OnScroll);
            this.DoubleBuffered = true;

            this.MouseEnter += new EventHandler(OnMouseEnter);
            this.MouseLeave += new EventHandler(OnMouseLeave);

            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            
            this.UpdateStyles();

            this.contextMenuServer.Renderer = new EasyRenderer.EasyRender();
            this.contextMenuChannel.Renderer = new EasyRenderer.EasyRender();
            this.contextMenuQuery.Renderer = new EasyRenderer.EasyRender();
            this.contextMenuDCCChat.Renderer = new EasyRenderer.EasyRender();
            this.contextMenuChannelList.Renderer = new EasyRenderer.EasyRender();

            serverConnections = new SortedList();

            serverNodes = new List<KeyValuePair<string,object>>();

            serversCollection = LoadServers();

            //renumber the server ID's if needed
            int serverID = 1;

            bool updateIgnoreList = false;
            
            foreach (ServerSetting s in serversCollection.listServers)
            {
                if (s.AltNickName == null)
                    s.AltNickName = s.NickName + "_";
                

                // have we updated to the new ignore list
                if (s.IgnoreListUpdated == false)
                {
                    if (s.IgnoreList != null)
                    {
                        s.Ignores = new IgnoreListItem[s.IgnoreList.Length];
                        
                        int i = 0;
                        
                        foreach (string item in s.IgnoreList)
                        {
                            s.Ignores[i] = new IgnoreListItem
                            {
                                IgnoreType = new IgnoreType()
                            };

                            if (item.StartsWith(";")) {
                                s.Ignores[i].Item = item.Substring(1);
                                s.Ignores[i].Enabled = false;
                            } else {
                                s.Ignores[i].Item = item;
                                s.Ignores[i].Enabled = true;
                            }
                            
                            s.Ignores[i].IgnoreType.SetIgnore(0);
                            
                            i++;
                        }
                    }
                    
                    s.IgnoreListUpdated = true;
                    s.IgnoreList = new string[0];


                    updateIgnoreList = true;
                }


                s.IAL = new Hashtable();
                s.ID = serverID;
                serverID++;
            }

            flashTabTimer = new System.Timers.Timer
            {
                Interval = 1000
            };
            flashTabTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnFlashTabTimerElapsed);

            toolTip = new ToolTip
            {
                AutoPopDelay = 3000,
                ForeColor = System.Drawing.SystemColors.InfoText,
                BackColor = System.Drawing.SystemColors.Info
            };

            Invalidate();

            // have we created the new ignore list?
            if (updateIgnoreList)
            {                
                SaveServers(serversCollection);
            }


            this.MouseWheel += new MouseEventHandler(ServerTree_MouseWheel);

        }

        private void ServerTree_MouseWheel(object sender, MouseEventArgs e)
        {
            this.ScrollWindow(e.Delta > 0);            
        }

        private void OnFlashTabTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invalidate();
        }

        private void PanelButtons_VisibleChanged(object sender, EventArgs e)
        {
            if (this.panelButtons.Visible == true)
                this.vScrollBar.Height = this.Height - (this.headerHeight + this.panelButtons.Height);
            else
                this.vScrollBar.Height = this.Height - this.headerHeight;
        }

        //this is to make the arrow keys work in the user control
        protected override bool IsInputKey(Keys AKeyData)
        {
            return true;
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                selectedNodeIndex--;
                SelectNodeByIndex(selectedNodeIndex, false);
            }
            else if (e.KeyCode == Keys.Down)
            {
                selectedNodeIndex++;
                SelectNodeByIndex(selectedNodeIndex, false);
            }
            else if (e.KeyCode == Keys.Apps)
            {
                //right mouse key
                this.OnMouseUp(new MouseEventArgs(MouseButtons.Right, 1,0,0,0));
            }
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
                    else if (!scrollUp && (topIndex + vScrollBar.LargeChange) < vScrollBar.Maximum)
                    {
                        topIndex++;
                        vScrollBar.Value++;
                        Invalidate();
                    }
                }
            }

            catch (Exception)  { }
        }

        private void OnResize(object sender, EventArgs e)
        {
            this.vScrollBar.Left = this.Width - this.vScrollBar.Width;

            if (this.panelButtons.Visible == true)
                this.vScrollBar.Height = this.Height - this.headerHeight - this.panelButtons.Height;
            else
                this.vScrollBar.Height = this.Height - this.headerHeight;
        }

        internal void ApplyLanguage()
        {
            IceChatLanguage iceChatLanguage = _parent.IceChatLanguage;
            buttonConnect.Text = iceChatLanguage.serverTreeButtonConnect;
            buttonEdit.Text = iceChatLanguage.serverTreeButtonEdit;
            buttonDisconnect.Text = iceChatLanguage.serverTreeButtonDisconnect;
            buttonAdd.Text = iceChatLanguage.serverTreeButtonAdd;
            headerCaption = iceChatLanguage.serverTreeHeader;
            Invalidate();
        }

        private void PanelButtons_Resize(object sender, EventArgs e)
        {
            buttonConnect.Width = (panelButtons.Width / 2) - 4;
            buttonDisconnect.Width = buttonConnect.Width;

            buttonAdd.Width = buttonConnect.Width;
            buttonEdit.Width = buttonConnect.Width;
            
            buttonEdit.Left = (panelButtons.Width / 2) + 1;
            buttonAdd.Left = (panelButtons.Width / 2) + 1;
            
        }


        private void OnScroll(object sender, ScrollEventArgs e)
        {
            topIndex = ((VScrollBar)sender).Value;
            Invalidate();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (vScrollBar.Visible == true)
            {
                if (e.Delta < 0)
                {
                    if (vScrollBar.Maximum < (vScrollBar.Value + 1))
                    {
                        vScrollBar.Value = vScrollBar.Maximum;
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

        private void OnDoubleClick(object sender, EventArgs e)
        {
            Point p = this.PointToClient(Cursor.Position);

            if (selectedServerID == 0)
                return;

            if (p.X < 16)
                return;

            //only disconnect/connect if an actual server is selected, not just any window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode != null)
            {
                if (findNode.GetType() == typeof(ServerSetting))
                {
                    IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
                    if (c != null)
                    {
                        if (c.IsConnected)
                        {
                            _parent.ParseOutGoingCommand(c, "//quit " + c.ServerSetting.QuitMessage);
                        }
                        else
                        {
                            //switch to Console
                            _parent.ChannelBar.SelectedIndex = 0;
                            c.ConnectSocket();
                        }
                        return;
                    }
                    if (NewServerConnection != null)
                        NewServerConnection(GetServerSetting(selectedServerID));
                }
            }
        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            Invalidate();
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

            if (e.Y <= headerHeight)
            {
                //de-select any previous items
                selectedNodeIndex = 0;
                Invalidate();
                return;
            }

            Graphics g = this.CreateGraphics();
            
            int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
            //find the server number, add 1 to it to make it a non-zero value
            int nodeNumber = Convert.ToInt32((e.Location.Y - headerHeight) / _lineSize) + 1 + topIndex;

            g.Dispose();
            
            //check if we have clicked the + or - to collapse or not collapse the tree
            if (e.Button == MouseButtons.Left && serverNodes.Count > 0 && e.X < 16)
            {
                object findNode = FindNodeValue(nodeNumber);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(ServerSetting))
                    {
                        int t = ((ServerSetting)findNode).TreeCollapse;
                        if (t == 0)
                            return;
                        else if (t == 1)
                            ((ServerSetting)findNode).TreeCollapse = 2;
                        else
                            ((ServerSetting)findNode).TreeCollapse = 1;

                        this.Invalidate();
                        return;
                    }
                }
            }

            SelectNodeByIndex(nodeNumber, true);

            if (e.Button == MouseButtons.Left && serverNodes.Count > 1)
            {
                object findNode = FindNodeValue(nodeNumber);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(ServerSetting))
                    {
                        selectedDragID = (ServerSetting)findNode;
                    }
                }
            }

            if (e.Button == MouseButtons.Middle)
            {
                object findNode = FindNodeValue(nodeNumber);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(IceTabPage))
                    {
                        if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Channel || ((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Query || ((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.DCCChat)
                        {
                            //part the channel/close the query window
                            _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/part " + ((IceTabPage)findNode).TabCaption);
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Window)                            
                        {
                            //just close the window
                            _parent.ParseOutGoingCommand(null, "/close " + ((IceTabPage)findNode).TabCaption);
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Debug)
                        {
                            _parent.ParseOutGoingCommand(null, "/close debug");
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.ChannelList)
                        {
                            System.Diagnostics.Debug.WriteLine("found channel list");
                            _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/close Channels");
                        }
                    }
                    else if (findNode.GetType() == typeof(IceTabPageDCCFile))
                    {
                        //close dcc file/send window

                    }
                }
            }

        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Y <= headerHeight)
                return;

            Graphics g = this.CreateGraphics();

            int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
            //find the server number, add 1 to it to make it a non-zero value
            int nodeNumber = Convert.ToInt32((e.Location.Y - headerHeight) / _lineSize) + 1 + topIndex;
            
            //dont bother moving anything if we have 1 or less servers
            if (e.Button == MouseButtons.Left && serverNodes.Count > 1)
            {
                //try dragging a node
                if (nodeNumber <= serverNodes.Count)
                {
                    object findNode = FindNodeValue(nodeNumber);
                    if (findNode != null)
                    {
                        if (findNode.GetType() == typeof(ServerSetting) && selectedDragID != null)
                        {
                            //can only drag a server node
                            ServerSetting index1 = (ServerSetting)findNode;
                            if (index1 == selectedDragID) return;

                            serversCollection.listServers.Remove(selectedDragID);
                            serversCollection.listServers.Insert(index1.ID - 1, selectedDragID);

                            //rename all the servers ID's in the list
                            int count = 1;
                            foreach (ServerSetting s in serversCollection.listServers)
                            {
                                s.ID = count;
                                count++;
                            }

                            //re-select the proper node (NOT QUITE RIGHT)                            
                            selectedNodeIndex = nodeNumber;

                            this.Invalidate();

                        }
                    }
                }

                return;
            }


            if (nodeNumber <= serverNodes.Count)
            {
                object findNode = FindNodeValue(nodeNumber);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(ServerSetting))
                    {
                        if (toolTipNode != nodeNumber)
                        {
                            string t = "";
                            if (((ServerSetting)findNode).RealServerName.Length > 0 )
                                t = ((ServerSetting)findNode).RealServerName + ":" + ((ServerSetting)findNode).ServerPort;
                            else
                                t = ((ServerSetting)findNode).ServerName + ":" + ((ServerSetting)findNode).ServerPort;

                            toolTip.ToolTipTitle = t; 
                            toolTip.SetToolTip(this, ((ServerSetting)findNode).NickName);
                            
                            toolTipNode = nodeNumber;
                        }
                    }
                    else if (findNode.GetType() == typeof(IceTabPage))
                    {
                        //this is a window, switch to this channel/query
                        if (toolTipNode != nodeNumber)
                        {
                            if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Channel)
                            {
                                toolTip.ToolTipTitle = ((IceTabPage)findNode).TabCaption;
                                toolTip.SetToolTip(this, "{" + ((IceTabPage)findNode).Nicks.Count + "} " + "[" + ((IceTabPage)findNode).ChannelModes + "]");
                            }
                            else
                            {
                                toolTip.ToolTipTitle = "User Information";
                                toolTip.SetToolTip(this, ((IceTabPage)findNode).TabCaption);
                            }
                            toolTipNode = nodeNumber;
                        }
                    }
                    else if (findNode.GetType() == typeof(IceTabPageDCCFile))
                    {


                    }
                }
            }
            else
            {
                toolTip.RemoveAll();
            }

            g.Dispose();
        }

        internal void SelectNodeByIndex(int nodeNumber, bool RefreshMainTab)
        {
            try
            {
                if (nodeNumber < 0)
                    selectedNodeIndex = 0;
                else if (nodeNumber <= serverNodes.Count)
                    selectedNodeIndex = nodeNumber;
                else
                    selectedNodeIndex = 0;

                selectedServerID = 0;

                this.Invalidate();
                
                object findNode = FindNodeValue(selectedNodeIndex);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(ServerSetting))
                    {
                        //this is a server, switch to console
                        if (RefreshMainTab)
                            _parent.ChannelBar.SelectTab(_parent.ChannelBar.GetTabPage("Console"));

                        //find the correct tab for the server tab
                        foreach (ConsoleTab c in _parent.ChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
                        {
                            if (c.Connection != null)
                            {
                                if (c.Connection.ServerSetting == ((ServerSetting)findNode))
                                {
                                    //found the connection, switch to this tab in the Console Tab Window
                                    selectedServerID = c.Connection.ServerSetting.ID;
                                    _parent.ChannelBar.GetTabPage("Console").SelectConsoleTab(c);
                                    return;
                                }
                            }
                        }
                        
                        //select the default console window
                        _parent.ChannelBar.GetTabPage("Console").ConsoleTab.SelectedIndex = 0;
                        return;
                    }

                    else if (findNode.GetType() == typeof(IceTabPage))
                    {
                        //this is a window, switch to this channel/query
                        if (RefreshMainTab)
                            _parent.ChannelBar.SelectTab((IceTabPage)findNode);
                        return;
                    }
                    else if (findNode.GetType() == typeof(IceTabPageDCCFile))
                    {
                        if (RefreshMainTab)
                            _parent.ChannelBar.SelectTab((IceTabPageDCCFile)findNode);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                _parent.WriteErrorFile(_parent.InputPanel.CurrentConnection,"SelectNodeByIndex", e);
            }
        }

        internal void SelectTab(object selectedNode, bool RefreshMainTab)
        {
            BuildServerNodes();

            if (selectedNode.GetType() == typeof(ServerSetting))
            {
                //this is a console tab
                int node = FindServerNodeMatch(selectedNode);

                SelectNodeByIndex(node, RefreshMainTab);
            }
            else if (selectedNode.GetType() == typeof(IceTabPage))
            {
                //this is a window tab
                //check if it is a console tab or not
                
                if (((IceTabPage)selectedNode).WindowStyle == IceTabPage.WindowType.Console)
                {
                    if (((ConsoleTab)((IceTabPage)selectedNode).ConsoleTab.SelectedTab).Connection != null)
                        SelectNodeByIndex(FindServerNodeMatch(((ConsoleTab)((IceTabPage)selectedNode).ConsoleTab.SelectedTab).Connection.ServerSetting), RefreshMainTab);
                    else
                        SelectNodeByIndex(FindWindowNodeMatch(selectedNode), RefreshMainTab);
                }
                else
                    SelectNodeByIndex(FindWindowNodeMatch(selectedNode), RefreshMainTab);

            }
            
            this.Invalidate();                
        }
        
        private int FindServerNodeMatch(object nodeMatch)
        {
            int nodeCount = 0;
            foreach (KeyValuePair<string, object> de in serverNodes)
            {
                nodeCount++;
                if (de.Value == (ServerSetting)nodeMatch)
                {
                    return nodeCount;
                }
            }
            return 0;
        }

        private int FindWindowNodeMatch(object nodeMatch)
        {
            int nodeCount = 0;
            foreach (KeyValuePair<string, object> de in serverNodes)
            {
                nodeCount++;
                if (de.Value == (IceTabPage)nodeMatch)
                {
                    return nodeCount;
                }
            }
            return 0;
        }

        /// <summary>
        /// Find a node by the index and return its value (node type)
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <returns></returns>
        private object FindNodeValue(int nodeIndex)
        {
            int nodeCount = 0;
            foreach (KeyValuePair<string, object> de in serverNodes)
            {
                nodeCount++;
                if (nodeCount == nodeIndex)
                {
                    return de.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Return focus back to the InputText Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            //see what menu to popup according to the nodetype
            if (selectedDragID != null)
            {
                //save the server list
                SaveServerSettings();
                selectedDragID = null;
            }

            if (e.Button == MouseButtons.Right)
            {
                //see what kind of a node we right clicked
                object findNode = FindNodeValue(selectedNodeIndex);
                if (findNode != null)
                {
                    if (findNode.GetType() == typeof(ServerSetting))
                    {
                        //make the default menu
                        this.contextMenuServer.Items.Clear();
                        
                        this.contextMenuServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                            this.connectToolStripMenuItem,
                            this.disconnectToolStripMenuItem,
                            this.forceDisconnectToolStripMenuItem,
                            this.autoStartToolStripMenuItem,
                            this.toolStripMenuItemBlank,
                            this.editToolStripMenuItem,
                            this.removeServerToolStripMenuItem,
                            this.toolStripMenuItem1,
                            this.autoJoinToolStripMenuItem,
                            this.autoPerformToolStripMenuItem,
                            this.openLogFolderToolStripMenuItem});

                        this.autoStartToolStripMenuItem.Checked = ((ServerSetting)findNode).AutoStart;

                        //add the menu's created by plugins
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
                                        List<ToolStripItem> col = new List<ToolStripItem>();

                                        foreach (ToolStripMenuItem t in popServer)
                                        {
                                            ToolStripMenuItem ti = ipc.plugin.MenuItemShow(t);
                                            col.Add(ti);
                                        }

                                        this.contextMenuServer.Items.AddRange(col.ToArray());

                                    }
                                }
                            }
                        }

                        //add in the popup menu
                        AddPopupMenu("Console", contextMenuServer);

                        this.contextMenuServer.Show(this, new Point(e.X, e.Y));
                    }
                    else if (findNode.GetType() == typeof(IceTabPage))
                    {
                        //check if it is a channel or query window
                        if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Channel)
                        {
                            contextMenuChannel.Items.Clear();

                            ToolStripMenuItem attachMenu; // = new ToolStripMenuItem(caption);
                            //t.Tag = command;
                            if (((IceTabPage)findNode).Detached)
                            {
                                attachMenu = new ToolStripMenuItem("Attach Tab")
                                {
                                    Tag = "/attach"
                                };
                            }
                            else
                            {
                                attachMenu = new ToolStripMenuItem("Detach Tab")
                                {
                                    Tag = "/detach"
                                };
                            }
                            
                            attachMenu.Click += new EventHandler(OnPopupMenuClick);

                            this.contextMenuChannel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                            this.clearChannelToolStripMenuItem,
                            this.closeChannelToolStripMenuItem,
                            this.reJoinChannelToolStripMenuItem,
                            this.addToAutoJoinToolStripMenuItem,
                            this.channelInformationToolStripMenuItem,
                            this.channelFontToolStripMenuItem,
                            this.noColorModeToolStripMenuItem, 
                            this.loggingToolStripMenuItem,
                            this.eventsToolStripMenuItem,
                            attachMenu});



                            this.noColorModeToolStripMenuItem.Checked = ((IceTabPage)findNode).TextWindow.NoColorMode;
                            this.disableEventsToolStripMenuItem.Checked = ((IceTabPage)findNode).EventOverLoad;
                            this.disableSoundsToolStripMenuItem.Checked = ((IceTabPage)findNode).DisableSounds;
                            this.disableLoggingToolStripMenuItem.Checked = ((IceTabPage)findNode).LoggingDisable;

                            if (((IceTabPage)findNode).JoinEventLocationOverload)
                            {
                                switch (((IceTabPage)findNode).JoinEventLocation)
                                {
                                    case 0:
                                        this.inChannelToolStripMenuItem.Checked = true;
                                        this.inConsoleToolStripMenuItem.Checked = false;
                                        this.hideToolStripMenuItem.Checked = false;
                                        break;
                                    case 1:
                                        this.inConsoleToolStripMenuItem.Checked = true;
                                        this.inChannelToolStripMenuItem.Checked = false;
                                        this.hideToolStripMenuItem.Checked = false;
                                        break;
                                    case 2:
                                        this.hideToolStripMenuItem.Checked = true;
                                        this.inChannelToolStripMenuItem.Checked = false;
                                        this.inConsoleToolStripMenuItem.Checked = false;
                                        break;
                                }
                            }
                            else
                            {
                                this.inChannelToolStripMenuItem.Checked = false;
                                this.inConsoleToolStripMenuItem.Checked = false;
                                this.hideToolStripMenuItem.Checked = false;
                            }


                            if (((IceTabPage)findNode).PartEventLocationOverload)
                            {
                                switch (((IceTabPage)findNode).PartEventLocation)
                                {
                                    case 0:
                                        this.inChannelToolStripMenuItem1.Checked = true;
                                        this.inConsoleToolStripMenuItem1.Checked = false;
                                        this.hideToolStripMenuItem1.Checked = false;
                                        break;
                                    case 1:
                                        this.inConsoleToolStripMenuItem1.Checked = true;
                                        this.inChannelToolStripMenuItem1.Checked = false;
                                        this.hideToolStripMenuItem1.Checked = false;
                                        break;
                                    case 2:
                                        this.hideToolStripMenuItem1.Checked = true;
                                        this.inChannelToolStripMenuItem1.Checked = false;
                                        this.inConsoleToolStripMenuItem1.Checked = false;
                                        break;
                                }
                            }
                            else
                            {
                                this.inChannelToolStripMenuItem1.Checked = false;
                                this.inConsoleToolStripMenuItem1.Checked = false;
                                this.hideToolStripMenuItem1.Checked = false;
                            }

                            if (((IceTabPage)findNode).QuitEventLocationOverload)
                            {
                                switch (((IceTabPage)findNode).QuitEventLocation)
                                {
                                    case 0:
                                        this.inChannelToolStripMenuItem2.Checked = true;
                                        this.inConsoleToolStripMenuItem2.Checked = false;
                                        this.hideToolStripMenuItem2.Checked = false;
                                        break;
                                    case 1:
                                        this.inConsoleToolStripMenuItem2.Checked = true;
                                        this.inChannelToolStripMenuItem2.Checked = false;
                                        this.hideToolStripMenuItem2.Checked = false;
                                        break;
                                    case 2:
                                        this.hideToolStripMenuItem2.Checked = true;
                                        this.inChannelToolStripMenuItem2.Checked = false;
                                        this.inConsoleToolStripMenuItem2.Checked = false;
                                        break;
                                }
                            }
                            else
                            {
                                this.inChannelToolStripMenuItem2.Checked = false;
                                this.inConsoleToolStripMenuItem2.Checked = false;
                                this.hideToolStripMenuItem2.Checked = false;
                            }

                            // are all events disabled ?
                            bool allDisabled = false;
                            if (((IceTabPage)findNode).JoinEventLocationOverload)
                            {
                                if (((IceTabPage)findNode).JoinEventLocation == 2)
                                {
                                    if (((IceTabPage)findNode).PartEventLocationOverload)
                                    {
                                        if (((IceTabPage)findNode).PartEventLocation == 2)
                                        {
                                            if (((IceTabPage)findNode).QuitEventLocationOverload)
                                            {
                                                if (((IceTabPage)findNode).QuitEventLocation == 2)
                                                {
                                                    hideJoinPartQuitToolStripMenuItem.Checked = true;
                                                    allDisabled = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            // they are not all disabled, uncheck it
                            if (!allDisabled)
                                hideJoinPartQuitToolStripMenuItem.Checked = false;

                            //add the menu's created by plugins
                            foreach (Plugin p in _parent.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                    {
                                        ipc.plugin.ServerTreeCurrentConnection = ((IceTabPage)findNode).Connection;
                                        ipc.plugin.ServerTreeCurrentTab = ((IceTabPage)findNode).TabCaption;

                                        ToolStripItem[] popChan = ipc.plugin.AddChannelPopups();
                                        if (popChan != null && popChan.Length > 0)
                                        {
                                            // run a refresh on the popup item
                                            List<ToolStripItem> col = new List<ToolStripItem>();

                                            foreach (ToolStripMenuItem t in popChan)
                                            {
                                                ToolStripMenuItem ti = ipc.plugin.MenuItemShow(t);
                                                col.Add(ti);
                                            }

                                            this.contextMenuChannel.Items.AddRange(col.ToArray());
                                        }
                                    }
                                }
                            }

                            //add in the popup menu
                            AddPopupMenu("Channel", contextMenuChannel);

                            this.contextMenuChannel.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Query)
                        {
                            contextMenuQuery.Items.Clear();
                            
                            this.contextMenuQuery.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                            this.clearQueryToolStripMenuItem,
                            this.closeQueryToolStripMenuItem,
                            this.userInformationToolStripMenuItem,
                            this.silenceUserToolStripMenuItem,
                            this.eventsQueryMenuItem});

                            this.disableSoundsQueryMenuItem.Checked = ((IceTabPage)findNode).DisableSounds;

                            //add the menu's created by plugins
                            foreach (Plugin p in _parent.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                    {
                                        ipc.plugin.ServerTreeCurrentConnection = ((IceTabPage)findNode).Connection;
                                        ipc.plugin.ServerTreeCurrentTab = ((IceTabPage)findNode).TabCaption;

                                        ToolStripItem[] popQuery = ipc.plugin.AddQueryPopups();
                                        if (popQuery != null && popQuery.Length > 0)
                                        {
                                            //this.contextMenuQuery.Items.AddRange(popQuery);
                                            List<ToolStripItem> col = new List<ToolStripItem>();

                                            foreach (ToolStripMenuItem t in popQuery)
                                            {
                                                ToolStripMenuItem ti = ipc.plugin.MenuItemShow(t);
                                                col.Add(ti);
                                            }

                                            this.contextMenuQuery.Items.AddRange(col.ToArray());
                                        }
                                    }
                                }
                            }

                            //add in the popup menu
                            AddPopupMenu("Query", contextMenuQuery);

                            this.contextMenuQuery.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.DCCChat)
                        {
                            this.contextMenuDCCChat.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.ChannelList)
                        {
                            this.contextMenuChannelList.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.DCCFile)
                        {
                            this.contextMenuDCCFiles.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Window)
                        {
                            this.contextMenuWindow.Show(this, new Point(e.X, e.Y));
                        }
                        else if (((IceTabPage)findNode).WindowStyle == IceTabPage.WindowType.Debug)
                        {
                            contextMenuDebug.Items.Clear();
                            this.contextMenuDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                this.toolStripMenuClearDebug, 
                                this.toolStripMenuCloseDebug
                            });

                            contextMenuDebug.Items.Add(new ToolStripSeparator());
                            //add the servers to the list
                            foreach (IRCConnection c in _parent.ServerTree.ServerConnections.Values)
                            {
                                if (c.IsConnected)
                                {
                                    ToolStripMenuItem t = new ToolStripMenuItem();
                                    t.Click += new EventHandler(DebugMenuItemClick);
                                    if (c.ServerSetting.RealServerName.Length > 0)
                                        t.Text = c.ServerSetting.RealServerName;
                                    else
                                        t.Text = c.ServerSetting.ServerName;

                                    if (c.ShowDebug)
                                    {
                                        t.Tag = "/debug disable " + c.ServerSetting.ID;
                                        t.Checked = true;
                                        this.contextMenuDebug.Items.Add(t);
                                    }
                                    else
                                    {
                                        t.Tag = "/debug enable " + c.ServerSetting.ID;
                                        this.contextMenuDebug.Items.Add(t);
                                    }
                                }
                            }

                            this.contextMenuDebug.Show(this, new Point(e.X, e.Y));
                        }
                    }
                }
            }
        }

        private void DebugMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem t = sender as ToolStripMenuItem;
            if (t.Tag == null) return;
            _parent.ParseOutGoingCommand(null, t.Tag.ToString());
        }

        private void AddPopupMenu(string PopupType, ContextMenuStrip mainMenu)
        {
            //add the console menu popup
            foreach (PopupMenuItem p in _parent.IceChatPopupMenus.listPopups)
            {
                if (p.PopupType == PopupType && p.Menu.Length > 0)
                {
                    //add a break
                    mainMenu.Items.Add(new ToolStripSeparator());
                    
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
                            object findNode = FindNodeValue(selectedNodeIndex);
                            if (findNode != null)
                            {
                                if (p.PopupType == "Channel")
                                {
                                    if (findNode.GetType() == typeof(IceTabPage))
                                    {
                                        command = command.Replace("$chanlogdir", ((IceTabPage)findNode).TextWindow.LogFileLocation);
                                        caption = caption.Replace("$chan", ((IceTabPage)findNode).TabCaption);
                                        command = command.Replace("$chan", ((IceTabPage)findNode).TabCaption);
                                        caption = caption.Replace("$1", ((IceTabPage)findNode).TabCaption);
                                        command = command.Replace("$1", ((IceTabPage)findNode).TabCaption);
                                    }
                                }

                                if (p.PopupType == "Query")
                                {
                                    if (findNode.GetType() == typeof(IceTabPage))
                                    {
                                        caption = caption.Replace("$nick", ((IceTabPage)findNode).TabCaption);
                                        command = command.Replace("$nick", ((IceTabPage)findNode).TabCaption);
                                        caption = caption.Replace("$1", ((IceTabPage)findNode).TabCaption);
                                        command = command.Replace("$1", ((IceTabPage)findNode).TabCaption);
                                        command = command.Replace("$querylogdir", ((IceTabPage)findNode).TextWindow.LogFileLocation);
                                    }
                                }

                                if (p.PopupType == "Console")
                                {
                                    if (findNode.GetType() == typeof(ServerSetting))
                                    {
                                        if (((ServerSetting)findNode).RealServerName.Length > 0)
                                        {
                                            caption = caption.Replace("$server", ((ServerSetting)findNode).RealServerName);
                                            command = command.Replace("$server", ((ServerSetting)findNode).RealServerName);
                                        }
                                        else
                                        {
                                            caption = caption.Replace("$server", ((ServerSetting)findNode).ServerName);
                                            command = command.Replace("$server", ((ServerSetting)findNode).ServerName);
                                        }
                                    }
                                }
                            }
                            
                            if (caption == "-")
                                t = new ToolStripSeparator();
                            else
                            {
                                t = new ToolStripMenuItem(caption)
                                {
                                    ForeColor = SystemColors.MenuText,
                                    BackColor = SystemColors.Menu
                                };

                                t.Click += new EventHandler(OnPopupMenuClick);
                                t.Tag = command;
                            }

                            if (menuDepth == 0)
                                subMenu = mainMenu.Items.Add(t);
                            else
                            {
                                if (mainMenu.Items[subMenu].GetType() != typeof(ToolStripSeparator))
                                    ((ToolStripMenuItem)mainMenu.Items[subMenu]).DropDownItems.Add(t);
                            }
                            t = null;
                        }
                    }
                }
            }

        }

        private void OnPopupMenuClick(object sender, EventArgs e)
        {
            if (((ToolStripMenuItem)sender).Tag == null) return;

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            string command = ((ToolStripMenuItem)sender).Tag.ToString();
            if (command.Length == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                _parent.ParseOutGoingCommand(c, command);
                return;
            }

        }

        private void PanelTop_Paint(object sender, PaintEventArgs e)
        {
            Bitmap buffer = new Bitmap(this.Width, headerHeight, e.Graphics);
            Graphics g = Graphics.FromImage(buffer);
            
            Font headerFont = new Font("Verdana", 10);
            Rectangle headerR = new Rectangle(0, 0, this.Width, headerHeight);
            Brush l = new LinearGradientBrush(headerR, IrcColor.colors[_parent.IceChatColors.PanelHeaderBG1], IrcColor.colors[_parent.IceChatColors.PanelHeaderBG2], 300);
            g.FillRectangle(l, headerR);

            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center
            };
            Rectangle centered = headerR;
            centered.Offset(0, (int)(headerR.Height - e.Graphics.MeasureString(headerCaption, headerFont).Height) / 2);

            g.DrawString(headerCaption, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]), centered, sf);

            e.Graphics.DrawImageUnscaled(buffer, 0, 0);

            buffer.Dispose();
            headerFont.Dispose();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (_parent == null)
                    return;
                
                //make the buffer we draw this all to
                Bitmap buffer = new Bitmap(this.Width, this.Height, e.Graphics);
                Graphics g = Graphics.FromImage(buffer);

                g.InterpolationMode = InterpolationMode.Low;
                g.SmoothingMode = SmoothingMode.HighSpeed;
                g.PixelOffsetMode = PixelOffsetMode.None;
                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

                //draw the header
                Font headerFont = new Font("Verdana", 10);

                Rectangle headerR = new Rectangle(0, 0, this.Width, headerHeight);
                Brush l = new LinearGradientBrush(headerR, IrcColor.colors[_parent.IceChatColors.PanelHeaderBG1], IrcColor.colors[_parent.IceChatColors.PanelHeaderBG2], 300);
                g.FillRectangle(l, headerR);

                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };
                Rectangle centered = headerR;
                centered.Offset(0, (int)(headerR.Height - e.Graphics.MeasureString(headerCaption, headerFont).Height) / 2);

                g.DrawString(headerCaption, headerFont, new SolidBrush(IrcColor.colors[_parent.IceChatColors.PanelHeaderForeColor]), centered, sf);
                    
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
                //draw each individual server
                Rectangle listR;
                if (this.panelButtons.Visible == true)
                    listR = new Rectangle(0, headerHeight, this.Width, this.Height - headerHeight - panelButtons.Height);
                else
                    listR = new Rectangle(0, headerHeight, this.Width, this.Height - headerHeight);

                if (_backgroundImage != null)
                    g.DrawImage((Image)_backgroundImage, listR);
                else
                    g.FillRectangle(new SolidBrush(IrcColor.colors[_parent.IceChatColors.ServerListBackColor]), listR);

                int currentY = listR.Y;
                int _lineSize = Convert.ToInt32(this.Font.GetHeight(g));
                
                BuildServerNodes();

                int nodeCount = 0;

                bool flashTabs = false;
                
                //check if we have any flashing tabs
                if (_parent.IceChatOptions.FlashServerTreeIcons == true)
                    flashTabs = true;
                else
                {
                    for (int i = 0; i < _parent.ChannelBar.TabPages.Count; i++)
                    {
                        if (_parent.ChannelBar.TabPages[i].FlashTab == true && _parent.ChannelBar.TabPages[i].EventOverLoad == false)
                        {
                            flashTabs = true;
                            break;
                        }
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


                if ((serverNodes.Count * _lineSize) > listR.Height)
                    vScrollBar.Visible = true;
                else
                {
                    vScrollBar.Visible = false;
                    // set the top to 0
                    topIndex = 0;
                }

                foreach (KeyValuePair<string, object> de in serverNodes)
                {
                    //get the object type for this node
                    string node = (string)de.Key;
                    string[] nodes = node.Split(',');
                    
                    //get a color out of the nodes[4]
                    string color = "";
                    if (nodes[4].StartsWith("&#x3;"))
                    {
                        //get the color code, 1 digit or 2
                        color = nodes[4].Substring(5, 1);
                        int result;
                        if (Int32.TryParse(nodes[4].Substring(6, 1), out result))
                            color += nodes[4].Substring(6, 1);
                        nodes[4] = nodes[4].Substring(5 + color.Length);
                    }
                    
                    object value = de.Value;

                    int x = 16;
                    Brush b;
                    //for drawing the tree + -        
                    Pen p = new Pen(IrcColor.colors[_parent.IceChatColors.TabBarDefault]);
                    
                    nodeCount++;                    
                    if (nodeCount <= topIndex)
                        continue;

                    if (nodeCount == selectedNodeIndex)
                    {
                        g.FillRectangle(new SolidBrush(SystemColors.Highlight), 0, currentY, this.Width, _lineSize);
                        b = new SolidBrush(SystemColors.HighlightText);
                    }
                    else
                    {
                        if (color.Length > 0)   //use the custom color
                        {
                            int d = Convert.ToInt32(nodes[2]);
                            if (d == _parent.IceChatColors.TabBarDefault)
                                b = new SolidBrush(IrcColor.colors[Convert.ToInt32(color)]);
                            else
                                b = new SolidBrush(IrcColor.colors[d]);                            
                        }
                        else
                            b = new SolidBrush(IrcColor.colors[Convert.ToInt32(nodes[2])]);
                    }

                    if (value.GetType() == typeof(ServerSetting))
                    {
                        if (nodeCount == selectedNodeIndex)
                        {
                            selectedServerID = ((ServerSetting)value).ID;
                        }
                        //TreeCollapse -- 0 - no items :: 1- show + sign (not collapsed) :: 2- show - sign (collapsed)

                        if (((ServerSetting)value).TreeCollapse > 0)
                            g.DrawRectangle(p, x - 12, currentY + 2, 8, 8);
                        
                        //now draw the + or - if it is collapsed or not

                        if (((ServerSetting)value).TreeCollapse == 1)
                        {
                            //the plus sign
                            g.DrawLine(p, x - 12, currentY + 6, x - 4, currentY + 6);
                            g.DrawLine(p, x - 8, currentY + 2, x - 8, currentY + 10);
                        }
                        else if (((ServerSetting)value).TreeCollapse == 2)
                        {
                            //the minus sign
                            g.DrawLine(p, x - 12, currentY + 6, x - 4, currentY + 6);
                        }
                       
                        x = 16;
                    }

                    if (value.GetType() == typeof(IceTabPage))
                    {
                        x = 32;
                        if (((IceTabPage)value).WindowStyle == IceTabPage.WindowType.Channel || ((IceTabPage)value).WindowStyle == IceTabPage.WindowType.Query || ((IceTabPage)value).WindowStyle == IceTabPage.WindowType.DCCChat)
                        {
                            if (nodeCount == selectedNodeIndex)
                            {
                                selectedServerID = ((IceTabPage)value).Connection.ServerSetting.ID;
                                ((IceTabPage)value).FlashTab = false;
                            }
                        }
                        else if (((IceTabPage)value).WindowStyle == IceTabPage.WindowType.Window)
                        {
                            if (((IceTabPage)value).Connection == null)
                                x = 16;
                        }
                        else if (((IceTabPage)value).WindowStyle == IceTabPage.WindowType.Debug)
                            x = 16;
                    }
                    switch (nodes[1])
                    {
                        case "0":   //disconnected
                            g.DrawImage(StaticMethods.LoadResourceImage("disconnect-icon.png"), x, currentY, 16, 16);
                            break;
                        case "0a":   //disconnected auto start
                            g.DrawImage(StaticMethods.LoadResourceImage("disconnect-icon-auto.png"), x, currentY, 16, 16);
                            break;
                        case "1":   //connected
                            g.DrawImage(StaticMethods.LoadResourceImage("connect-icon.png"), x, currentY, 16, 16);
                            break;
                        case "1a":   //connected - auto start
                            g.DrawImage(StaticMethods.LoadResourceImage("connect-icon-auto.png"), x, currentY, 16, 16);
                            break;
                        case "2":   //connecting
                        case "2a":   //connecting
                            g.DrawImage(StaticMethods.LoadResourceImage("refresh-icon.png"), x, currentY, 16, 16);
                            break;
                        case "3":   //channel
                            //check if we are flashing or not
                            if (((IceTabPage)value).FlashTab == true && ((IceTabPage)value).EventOverLoad == false)
                            {
                                if (((IceTabPage)value).FlashValue == 1)
                                    g.DrawImage(StaticMethods.LoadResourceImage("channel.png"), x, currentY, 16, 16);
                            }
                            else
                                g.DrawImage(StaticMethods.LoadResourceImage("channel.png"), x, currentY, 16, 16);
                            break;
                        case "4":   //query
                        case "5":   //dcc chat
                            if (((IceTabPage)value).FlashTab == true)
                            {
                                if (((IceTabPage)value).FlashValue == 1)
                                    g.DrawImage(StaticMethods.LoadResourceImage("query.png"), x, currentY, 16, 16);
                            }
                            else
                                g.DrawImage(StaticMethods.LoadResourceImage("query.png"), x, currentY, 16, 16);
                            break;
                        case "6":   //channel list
                            g.DrawImage(StaticMethods.LoadResourceImage("channellist.png"), x, currentY, 16, 16);
                            break;
                        case "7":
                            g.DrawImage(StaticMethods.LoadResourceImage("window-icon.ico"), x, currentY, 16, 16);
                            break;
                    }



                    g.DrawString(nodes[4], this.Font, b, x + 16, currentY);

                    b.Dispose();
                    
                    if (currentY >= (listR.Height))
                    {
                        //System.Diagnostics.Debug.WriteLine("redo scroll bar:" + serverNodes.Count);
                        vScrollBar.Maximum = serverNodes.Count - 2;
                        vScrollBar.LargeChange = ((listR.Height - _lineSize) / _lineSize);
                        break;
                    }

                    currentY += _lineSize;
                }

                l.Dispose();
                sf.Dispose();

                //paint the buffer onto the usercontrol
                e.Graphics.DrawImageUnscaled(buffer, 0, 0);

                buffer.Dispose();
                headerFont.Dispose();
                g.Dispose();
            }
            catch (Exception)
            {
                //_parent.WriteErrorFile(_parent.InputPanel.CurrentConnection,"ServerTree OnPaint", ee);
            }
        }

        private void BuildServerNodes()
        {
            try
            {
                serverNodes.Clear();

                int nodeCount = 0;

                //reset all the servers open channels
                foreach (IRCConnection c in serverConnections.Values)
                {
                    c.OpenChannels.Clear();
                }

                foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                {
                    if (t.Connection == null)
                    {
                        if (t.WindowStyle == IceTabPage.WindowType.Window)
                        {
                            //get the color
                            int colorQ = _parent.IceChatColors.TabBarDefault;

                            if (t.LastMessageType == FormMain.ServerMessageType.Default)
                                colorQ = _parent.IceChatColors.TabBarDefault;
                            else if (t.LastMessageType == FormMain.ServerMessageType.Message)
                                colorQ = _parent.IceChatColors.TabBarNewMessage;
                            else if (t.LastMessageType == FormMain.ServerMessageType.Action)
                                colorQ = _parent.IceChatColors.TabBarNewAction;
                            else
                                colorQ = _parent.IceChatColors.TabBarDefault;

                            nodeCount++;
                            //check if it is collapsed or has any sub items

                            serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",7," + colorQ.ToString() + ",0," + t.TabCaption, t));
                        }
                        else if (t.WindowStyle == IceTabPage.WindowType.Debug)
                        {
                            nodeCount++;
                            //check if it is collapsed or has any sub items
                            int colorQ = _parent.IceChatColors.TabBarDefault;
                            serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",7," + colorQ.ToString() + ",0," + t.TabCaption, t));
                        }
                        else if (t.WindowStyle == IceTabPage.WindowType.DCCFile)
                        {
                            nodeCount++;
                            //check if it is collapsed or has any sub items
                            int colorQ = _parent.IceChatColors.TabBarDefault;
                            serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",7," + colorQ.ToString() + ",0," + t.TabCaption, t));
                        }
                    }
                }

                //make a list of all the servers/windows open
                foreach (ServerSetting s in serversCollection.listServers)
                {
                    nodeCount++;
                    //icon_number:color:text
                    //1st check for server name/connected
                    int windowCount = 0;
                  
                    foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                    {
                        if (t.Connection != null && t.Connection.ServerSetting == s)
                        {
                            if (t.WindowStyle == IceTabPage.WindowType.Channel)
                                windowCount++;
                            else if (t.WindowStyle == IceTabPage.WindowType.Query)
                                windowCount++;
                            else if (t.WindowStyle == IceTabPage.WindowType.DCCChat)
                                windowCount++;
                            else if (t.WindowStyle == IceTabPage.WindowType.ChannelList)
                                windowCount++;
                        }
                    }

                    if (windowCount == 0)
                        s.TreeCollapse = 0;
                    else if (windowCount > 0 && s.TreeCollapse == 0)
                        s.TreeCollapse = 2;

                    //change the color                    
                    int colorS = GetLastMessageColor(s);

                    if (s.DisplayName.Length > 0)
                    {
                        serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + "," + IsServerConnected(s) + "," + colorS +"," + s.TreeCollapse + "," + s.DisplayName, s));
                    }
                    else
                    {
                        serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + "," + IsServerConnected(s) + "," + colorS + "," + s.TreeCollapse + "," + s.ServerName, s));
                    }

                    //find all open windows for this server                
                    //add the channels 1st
                    if (s.TreeCollapse == 2)
                    {
                        foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                        {
                            if (t.Connection != null)
                            {
                                if (t.Connection.ServerSetting == s && t.WindowStyle == IceTabPage.WindowType.Channel)
                                {
                                    int color = _parent.IceChatColors.TabBarDefault;
                                    if (t.LastMessageType == FormMain.ServerMessageType.CustomMessage)
                                        color = t.CustomForeColor;                                    
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Default)
                                        color = _parent.IceChatColors.TabBarDefault;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.JoinChannel)
                                        color = _parent.IceChatColors.TabBarChannelJoin;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.PartChannel)
                                        color = _parent.IceChatColors.TabBarChannelPart;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Message)
                                    {
                                        if (_parent.IceChatOptions.FlashServerTreeIcons && _parent.CurrentWindow != t)
                                            t.FlashTab = true;
                                        color = _parent.IceChatColors.TabBarNewMessage;
                                    }
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Action)
                                    {
                                        if (_parent.IceChatOptions.FlashServerTreeIcons && _parent.CurrentWindow != t)
                                            t.FlashTab = true;
                                        color = _parent.IceChatColors.TabBarNewAction;
                                    }
                                    else if (t.LastMessageType == FormMain.ServerMessageType.ServerMessage)
                                        color = _parent.IceChatColors.TabBarServerMessage;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.ServerNotice)
                                        color = _parent.IceChatColors.TabBarServerNotice;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.BuddyNotice)
                                        color = _parent.IceChatColors.TabBarBuddyNotice;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.QuitServer)
                                        color = _parent.IceChatColors.TabBarServerQuit;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Other)
                                        color = _parent.IceChatColors.TabBarOtherMessage;
                                    else
                                        color = _parent.IceChatColors.TabBarDefault;

                                    nodeCount++;
                                    //check if it is collapsed or has any sub items

                                    t.Connection.OpenChannels.Add(t.TabCaption);

                                    serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",3," + color.ToString() + ",0," + t.TabCaption, t));
                                }
                            }
                        }

                        //add the queries next
                        foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                        {
                            if (t.Connection != null)
                            {
                                if (t.Connection.ServerSetting == s && t.WindowStyle == IceTabPage.WindowType.Query)
                                {
                                    //get the color
                                    int colorQ = _parent.IceChatColors.TabBarDefault;
                                    if (t.LastMessageType == FormMain.ServerMessageType.Default)
                                        colorQ = _parent.IceChatColors.TabBarDefault;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Message)
                                    {
                                        if (_parent.IceChatOptions.FlashServerTreeIconsPrivate && _parent.CurrentWindow != t)
                                            t.FlashTab = true;
                                        colorQ = _parent.IceChatColors.TabBarNewMessage;
                                    }
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Action)
                                    {
                                        if (_parent.IceChatOptions.FlashServerTreeIconsPrivate && _parent.CurrentWindow != t)
                                            t.FlashTab = true;
                                        colorQ = _parent.IceChatColors.TabBarNewAction;
                                    }
                                    else if (t.LastMessageType == FormMain.ServerMessageType.CustomMessage)
                                    {
                                        if (_parent.IceChatOptions.FlashServerTreeIconsPrivate && _parent.CurrentWindow != t)
                                            t.FlashTab = true;
                                        colorQ = t.CustomForeColor;
                                    }
                                    else
                                        colorQ = _parent.IceChatColors.TabBarDefault;

                                    nodeCount++;
                                    serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",4," + colorQ.ToString() + ",0," + t.TabCaption, t));
                                }
                            }
                        }

                        //add dcc chat windows
                        foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                        {
                            if (t.Connection != null)
                            {
                                if (t.Connection.ServerSetting == s && t.WindowStyle == IceTabPage.WindowType.DCCChat)
                                {
                                    //get the color
                                    int colorQ = _parent.IceChatColors.TabBarDefault;
                                    if (t.LastMessageType == FormMain.ServerMessageType.Default)
                                        colorQ = _parent.IceChatColors.TabBarDefault;
                                    else if (t.LastMessageType == FormMain.ServerMessageType.Message || t.LastMessageType == FormMain.ServerMessageType.Action)
                                        colorQ = _parent.IceChatColors.TabBarNewMessage;
                                    else
                                        colorQ = _parent.IceChatColors.TabBarDefault;

                                    nodeCount++;
                                    serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",5," + colorQ.ToString() + ",0," + t.TabCaption, t));
                                }
                            }
                        }
                        //add any channel lists
                        foreach (IceTabPage t in _parent.ChannelBar.TabPages)
                        {
                            if (t.Connection != null)
                            {
                                if (t.Connection.ServerSetting == s && t.WindowStyle == IceTabPage.WindowType.ChannelList)
                                {
                                    //get the color
                                    int colorQ = _parent.IceChatColors.TabBarDefault;

                                    nodeCount++;
                                    serverNodes.Add(new KeyValuePair<string, object>(nodeCount.ToString() + ",6," + colorQ.ToString() + ",0," + t.TabCaption + " (" + t.TotalChannels + ")", t));
                                }
                            }
                        }
                    }


                }
            }
            catch (Exception e)
            {
                _parent.WriteErrorFile(_parent.InputPanel.CurrentConnection,"BuildServerNodes", e);
            }
        }

        private int GetLastMessageColor(ServerSetting s)
        {
            int color = _parent.IceChatColors.TabBarDefault;

            foreach (ConsoleTab t in _parent.ChannelBar.GetTabPage("Console").ConsoleTab.TabPages)
            {
                if (t.Connection != null && t.Connection.ServerSetting == s)
                {
                    if (t.LastMessageType == FormMain.ServerMessageType.Default)
                            color = _parent.IceChatColors.TabBarDefault;
                        else if (t.LastMessageType == FormMain.ServerMessageType.JoinChannel)
                            color = _parent.IceChatColors.TabBarChannelJoin;
                        else if (t.LastMessageType == FormMain.ServerMessageType.PartChannel)
                            color = _parent.IceChatColors.TabBarChannelPart;
                        else if (t.LastMessageType == FormMain.ServerMessageType.Message)
                            color = _parent.IceChatColors.TabBarNewMessage;
                        else if (t.LastMessageType == FormMain.ServerMessageType.Action)
                            color = _parent.IceChatColors.TabBarNewAction;
                        else if (t.LastMessageType == FormMain.ServerMessageType.ServerMessage)
                            color = _parent.IceChatColors.TabBarServerMessage;
                        else if (t.LastMessageType == FormMain.ServerMessageType.ServerNotice)
                            color = _parent.IceChatColors.TabBarServerNotice;
                        else if (t.LastMessageType == FormMain.ServerMessageType.BuddyNotice)
                            color = _parent.IceChatColors.TabBarBuddyNotice;
                        else if (t.LastMessageType == FormMain.ServerMessageType.QuitServer)
                            color = _parent.IceChatColors.TabBarServerQuit;
                    else if (t.LastMessageType == FormMain.ServerMessageType.Other)
                        color = _parent.IceChatColors.TabBarOtherMessage;
                    else
                        color = _parent.IceChatColors.TabBarDefault;

                    return color;
                }
            }                
            return color;
        }

        private string IsServerConnected(ServerSetting s)
        {
            foreach (IRCConnection c in serverConnections.Values)
            {
                //see if the server is connected
                if (c.ServerSetting == s)
                {
                    if (c.ServerSetting.AutoStart)
                    {
                        if (c.IsFullyConnected)
                            return "1a";
                        else if (c.IsConnected)
                            return "2a";
                    }
                    else
                    {
                        if (c.IsFullyConnected)
                            return "1";
                        else if (c.IsConnected)
                            return "2";
                    }
                }
            }
            if (s.AutoStart)
                return "0a";
            else
                return "0";

        }

        private ServerSetting GetServerSetting(int id)
        {
            ServerSetting ss = null;
            foreach (ServerSetting s in serversCollection.listServers)
            {
                if (s.ID == id)
                    ss = s;
            }
            return ss;
        }

        internal void SaveServers(IceChatServers servers)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatServers));
            TextWriter textWriter = new StreamWriter(_parent.ServersFile);
            serializer.Serialize(textWriter, servers);
            textWriter.Close();
            textWriter.Dispose();

            // validate the server settings.. if good.. Save to backup
            System.IO.File.Copy(_parent.ServersFile,_parent.BackupFolder + Path.DirectorySeparatorChar + "IceChatServer.xml", true);

        }

        private IceChatServers LoadServers()
        {
            IceChatServers servers = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(IceChatServers));
            TextReader textReader = null;
            try
            {                

                if (File.Exists(_parent.ServersFile))
                {
                    textReader = new StreamReader(_parent.ServersFile);
                    servers = (IceChatServers)deserializer.Deserialize(textReader);
                    textReader.Close();
                    textReader.Dispose();
                }
                else
                {
                    //create default server settings
                    servers = new IceChatServers();
                    SaveServers(servers);
                }
            }
            catch (InvalidOperationException)
            {
                // we have an error!  -- lets see if we have a backup!
                if (textReader != null)
                    textReader.Close();

                try
                {

                    string backupFile = _parent.CurrentFolder + Path.DirectorySeparatorChar + "Backups" + Path.DirectorySeparatorChar + "IceChatServer.xml";
                    if (File.Exists(backupFile))
                    {
                        textReader = new StreamReader(backupFile);
                        servers = (IceChatServers)deserializer.Deserialize(textReader);
                        textReader.Close();
                        textReader.Dispose();
                        
                        _parent.loadErrors.Add("There was a problem with IceChatServer.xml, restored from backup");

                        File.Copy(backupFile, _parent.ServersFile, true);

                    }
                    else
                    {
                        //create default server settings
                        servers = new IceChatServers();
                        SaveServers(servers);

                        _parent.loadErrors.Add("There was a problem with IceChatServer.xml, no backup to restore");

                    }
                }
                catch (InvalidOperationException)
                {
                    servers = new IceChatServers();
                    SaveServers(servers);

                    _parent.loadErrors.Add("There was a problem with IceChatServer.xml, no backup to restore");

                }
            }

            return servers;

        }
        
        internal SortedList ServerConnections
        {
            get
            {
                return serverConnections;
            }
        }

        internal IceChatServers ServersCollection
        {
            get
            {
                return serversCollection;
            }
        }

        internal bool Docked
        {
            get { return false; }
        }

        internal bool ShowServerButtons
        {
            get { return this.panelButtons.Visible; }
            set 
            { 
                this.panelButtons.Visible = value;
            }
        }

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


            this.buttonConnect.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonDisconnect.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonEdit.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];
            this.buttonAdd.BackColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsBackColor];


            this.buttonConnect.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonDisconnect.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonEdit.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];
            this.buttonAdd.ForeColor = IrcColor.colors[_parent.IceChatColors.SideBarButtonsForeColor];

        }


        #region Server Tree Buttons
        
        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                if (!c.IsConnected)
                {
                    //switch to the Console
                    //_parent.ChannelBar.SelectedIndex = 0;
                    c.ConnectSocket();
                }
                return;
            }

            if (NewServerConnection != null)
                NewServerConnection(GetServerSetting(selectedServerID));
        }

        private void ButtonDisconnect_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                c.AttemptReconnect = false;
                if (c.IsConnected)
                {
                    _parent.ParseOutGoingCommand(c, "//quit " + c.ServerSetting.QuitMessage);                    
                }
                return;
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            //open up the Server Editor
            //check if a server is selected or not
            if (Application.OpenForms["FormServers"] as FormServers != null)
            {
                Application.OpenForms["FormServers"].BringToFront();
                return;
            }            
            
            if (selectedServerID > 0)
            {
                f = new FormServers(GetServerSetting(selectedServerID));
                f.SaveServer += new FormServers.SaveServerDelegate(OnSaveServer);
                f.NewServer += new FormServers.NewServerDelegate(OnNewServer);
                f.SaveDefaultServer += new FormServers.SaveDefaultServerDelegate(OnSaveDefaultServer);
            }
            else
            {
                f = new FormServers();
                f.NewServer += new FormServers.NewServerDelegate(OnNewServer);
                f.SaveDefaultServer += new FormServers.SaveDefaultServerDelegate(OnSaveDefaultServer);
            }

            f.Show(this.Parent);
        }
        
        /// <summary>
        /// Save the Default Server Settings
        /// </summary>
        private void OnSaveDefaultServer()
        {
            if (SaveDefault != null)
                SaveDefault();
        }
        
        private void OnSaveServer(ServerSetting s, bool removeServer)
        {
            //check if the server needs to be removed
            if (removeServer)
            {
                serversCollection.RemoveServer(s);
            }

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                if (c.IsConnected)
                {
                    if (s.NickName != c.ServerSetting.CurrentNickName && s.TriedAltNick == false)
                        _parent.ParseOutGoingCommand(c, "/nick " + s.NickName);

                    //check for buddy list
                    if (c.ServerSetting.BuddyListEnable)
                        c.BuddyListCheck();
                    
                    // find all windows/channels for nocolormode
                    for (int i = FormMain.Instance.ChannelBar.TabPages.Count - 1; i >= 0; i--)
                    {
                        if (FormMain.Instance.ChannelBar.TabPages[i].WindowStyle == IceTabPage.WindowType.Channel)
                        {
                            if (FormMain.Instance.ChannelBar.TabPages[i].Connection == c)
                            {
                                FormMain.Instance.ChannelBar.TabPages[i].TextWindow.NoColorMode = s.NoColorMode;
                            }
                        }
                    }


                }
            }

            SaveServerSettings();
            f = null;
        }

        private void OnNewServer(ServerSetting s)
        {
            s.ID = serversCollection.GetNextID();
            s.IAL = new Hashtable();
            
            serversCollection.AddServer(s);
            SaveServerSettings();
            f = null;
        }

        public void AddConnection(IRCConnection c)
        {
            if (ServerConnections.ContainsKey(c.ServerSetting.ID))
            {
                Random r = new Random();
                do
                {
                    c.ServerSetting.ID = r.Next(10000, 49999);
                } while (ServerConnections.ContainsKey(c.ServerSetting.ID));
            }
            ServerConnections.Add(c.ServerSetting.ID, c);
            //check if it exists in the servers collection
            foreach (ServerSetting s in serversCollection.listServers)
            {
                if (s.ID == c.ServerSetting.ID)
                    return;
            }
            serversCollection.AddServer(c.ServerSetting);
        } 

        private void SaveServerSettings()
        {
            //save the XML File
            SaveServers(serversCollection);

            //update the Server Tree
            Invalidate();            

            _parent.FocusInputBox();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["FormServers"] as FormServers != null)
            {
                Application.OpenForms["FormServers"].BringToFront();
                return;
            }            

            FormServers f = new FormServers();
            f.NewServer += new FormServers.NewServerDelegate(OnNewServer);
            f.SaveDefaultServer += new FormServers.SaveDefaultServerDelegate(OnSaveDefaultServer);            
            f.Show(this.Parent);
        }


        #endregion

        #region Server Popup Menus
        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                if (!c.IsConnected)
                {
                    c.ConnectSocket();
                }
                return;
            }

            if (NewServerConnection != null)
                NewServerConnection(GetServerSetting(selectedServerID));

        }

        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                c.AttemptReconnect = false;
                if (c.IsConnected)
                {
                    _parent.ParseOutGoingCommand(c, "//quit " + c.ServerSetting.QuitMessage);
                }
                return;
            }
        }

        private void AutoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            GetServerSetting(selectedServerID).AutoStart = !GetServerSetting(selectedServerID).AutoStart; 
            
            //save the settings
            SaveServerSettings();

        }



        private void ForceDisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c != null)
            {
                c.AttemptReconnect = false;
                c.ForceDisconnect();
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedServerID > 0)
            {
                f = new FormServers(GetServerSetting(selectedServerID));
                f.SaveServer += new FormServers.SaveServerDelegate(OnSaveServer);
                f.SaveDefaultServer += new FormServers.SaveDefaultServerDelegate(OnSaveDefaultServer);
            }
            else
            {
                f = new FormServers();
                f.NewServer += new FormServers.NewServerDelegate(OnNewServer);
                f.SaveDefaultServer += new FormServers.SaveDefaultServerDelegate(OnSaveDefaultServer);
            }

            f.Show(this.Parent);

        }

        private void AutoJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            if (c!=null)
            {
                if (c.IsConnected)
                    _parent.ParseOutGoingCommand(c, "/autojoin");
                return;
            }
        }

        private void AutoPerformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            IRCConnection c = (IRCConnection)serverConnections[selectedServerID];
            {
                if (c != null && c.IsConnected)
                    _parent.ParseOutGoingCommand(c, "/autoperform");
                return;
            }

        }

        private void OpenLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            ServerSetting s = GetServerSetting(selectedServerID);
            if (s != null)
            {
                if (Directory.Exists(_parent.LogsFolder + System.IO.Path.DirectorySeparatorChar + s.ServerName))
                    System.Diagnostics.Process.Start(_parent.LogsFolder + System.IO.Path.DirectorySeparatorChar + s.ServerName);
                else
                    MessageBox.Show("Log folder not found", "IceChat");
                return;
            }
        }

        private void ClearChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear the channel window for the selected channel
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                ((IceTabPage)findNode).TextWindow.ClearTextWindow();

        }

        private void CloseChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close the channel
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/part " + ((IceTabPage)findNode).TabCaption);
        }

        private void ReJoinChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //do a channel hop
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/hop " + ((IceTabPage)findNode).TabCaption);                
        }

        private void AddToAutoJoinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //add channel to autojoin
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/autojoin " + ((IceTabPage)findNode).TabCaption);                
        }



        private void ChannelInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //popup channel information window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/chaninfo " + ((IceTabPage)findNode).TabCaption);
        }

        private void ChannelFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //popup change font window for channel
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/font " + ((IceTabPage)findNode).TabCaption);

        }

        private void NoColorModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //change the channel to No Color Mode or back again
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                ((IceTabPage)findNode).TextWindow.NoColorMode = !((IceTabPage)findNode).TextWindow.NoColorMode;
            }
        }

        private void OpenChannelLogFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //open the log folder for the channel
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                string logFolder = ((IceTabPage)findNode).TextWindow.LogFileLocation;

                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/run " + logFolder);
            }

        }
        private void DisableLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                //System.Diagnostics.Debug.WriteLine("DisableLogging:" + disableLoggingToolStripMenuItem.Checked);
                if (disableLoggingToolStripMenuItem.Checked)
                {
                    //set it to false
                    ((IceTabPage)findNode).LoggingDisable = false;
                }
                else
                    ((IceTabPage)findNode).LoggingDisable = true;

                ((IceTabPage)findNode).TextWindow.DisableLogFile();
            }
        }

        private void DisableEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                ((IceTabPage)findNode).EventOverLoad = !((IceTabPage)findNode).EventOverLoad;
            }
        }

        private void DisableSoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                ((IceTabPage)findNode).DisableSounds = !((IceTabPage)findNode).DisableSounds;
            }
        }

        private void InChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //channel joins in channel
            object findNode = FindNodeValue(selectedNodeIndex);

            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).JoinEventLocationOverload)
                {
                    if (((IceTabPage)findNode).JoinEventLocation == 0)
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = true;
                        ((IceTabPage)findNode).JoinEventLocation = 0;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).JoinEventLocationOverload = true;
                    ((IceTabPage)findNode).JoinEventLocation = 0;
                }
            }

        }

        private void InConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //channel joins in console
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).JoinEventLocationOverload)
                {
                    if (((IceTabPage)findNode).JoinEventLocation == 1)
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = false;
                        //((ToolStripMenuItem)sender).Checked = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = true;
                        ((IceTabPage)findNode).JoinEventLocation = 1;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).JoinEventLocationOverload = true;
                    ((IceTabPage)findNode).JoinEventLocation = 1;
                }

            }
        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //hide channel joins1
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).JoinEventLocationOverload)
                {
                    if (((IceTabPage)findNode).JoinEventLocation == 2)
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).JoinEventLocationOverload = true;
                        ((IceTabPage)findNode).JoinEventLocation = 2;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).JoinEventLocationOverload = true;
                    ((IceTabPage)findNode).JoinEventLocation = 2;
                }

            }

        }

        private void InChannelToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //channel parts in channel
            object findNode = FindNodeValue(selectedNodeIndex);

            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).PartEventLocationOverload)
                {
                    if (((IceTabPage)findNode).PartEventLocation == 0)
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = true;
                        ((IceTabPage)findNode).PartEventLocation = 0;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).PartEventLocationOverload = true;
                    ((IceTabPage)findNode).PartEventLocation = 0;
                }
            }

        }

        private void InConsoleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //channel parts in console
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).PartEventLocationOverload)
                {
                    if (((IceTabPage)findNode).PartEventLocation == 1)
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = true;
                        ((IceTabPage)findNode).PartEventLocation = 1;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).PartEventLocationOverload = true;
                    ((IceTabPage)findNode).PartEventLocation = 1;
                }

            }

        }

        private void HideToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //channel parts hide
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).PartEventLocationOverload)
                {
                    if (((IceTabPage)findNode).PartEventLocation == 2)
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).PartEventLocationOverload = true;
                        ((IceTabPage)findNode).PartEventLocation = 2;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).PartEventLocationOverload = true;
                    ((IceTabPage)findNode).PartEventLocation = 2;
                }

            }
        }

        private void InChannelToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //join events in channel
            hideJoinPartQuitToolStripMenuItem.Checked = false;

            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).QuitEventLocationOverload)
                {
                    if (((IceTabPage)findNode).QuitEventLocation == 1)
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = true;
                        ((IceTabPage)findNode).QuitEventLocation = 1;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).QuitEventLocationOverload = true;
                    ((IceTabPage)findNode).QuitEventLocation = 1;
                }

            }


        }

        private void InConsoleToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //part events in console
            hideJoinPartQuitToolStripMenuItem.Checked = false;

            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).QuitEventLocationOverload)
                {
                    if (((IceTabPage)findNode).QuitEventLocation == 1)
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = true;
                        ((IceTabPage)findNode).QuitEventLocation = 1;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).QuitEventLocationOverload = true;
                    ((IceTabPage)findNode).QuitEventLocation = 1;
                }
            }
        }

        private void HideToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //quit events hide
            hideJoinPartQuitToolStripMenuItem.Checked = false;
            
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                if (((IceTabPage)findNode).QuitEventLocationOverload)
                {
                    if (((IceTabPage)findNode).QuitEventLocation == 2)
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = false;
                    }
                    else
                    {
                        ((IceTabPage)findNode).QuitEventLocationOverload = true;
                        ((IceTabPage)findNode).QuitEventLocation = 2;
                    }
                }
                else
                {
                    ((IceTabPage)findNode).QuitEventLocationOverload = true;
                    ((IceTabPage)findNode).QuitEventLocation = 2;
                }

            }
        }

        private void HideJoinPartQuitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // hide all joins / parts / quits with 1 click
            if (hideJoinPartQuitToolStripMenuItem.Checked)
                hideJoinPartQuitToolStripMenuItem.Checked = false;
            else
                hideJoinPartQuitToolStripMenuItem.Checked = true;

            hideToolStripMenuItem.PerformClick();
            hideToolStripMenuItem1.PerformClick();
            hideToolStripMenuItem2.PerformClick();

        }

        private void ClearQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear query window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                ((IceTabPage)findNode).TextWindow.ClearTextWindow();
        }

        private void CloseQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close query window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/part " + ((IceTabPage)findNode).TabCaption);

        }

        private void UserInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //user information for query nick
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/userinfo " + ((IceTabPage)findNode).TabCaption);
        }

        private void SilenceUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //silence query user
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/silence +" + ((IceTabPage)findNode).TabCaption);
        }

        private void DisableSoundsQueryMenuItem_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
            {
                ((IceTabPage)findNode).DisableSounds = !((IceTabPage)findNode).DisableSounds;
            }
        }

        private void ClearWindowDCCChat_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                ((IceTabPage)findNode).TextWindow.ClearTextWindow();
        }

        private void CloseWindowDCCChat_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/close " + ((IceTabPage)findNode).TabCaption);
        }

        private void CloseChannenListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //close the channel list window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ParseOutGoingCommand(((IceTabPage)findNode).Connection, "/close " + ((IceTabPage)findNode).TabCaption);

        }

        private void ToolStripMenuClearDebug_Click(object sender, EventArgs e)
        {
            //clear debug window
            _parent.ParseOutGoingCommand(null, "/clear Debug");
        }

        private void ToolStripMenuCloseDebug_Click(object sender, EventArgs e)
        {
            //close debug window
            _parent.ParseOutGoingCommand(null, "/close debug");
        }

        private void ToolStripMenuCloseDCC_Click(object sender, EventArgs e)
        {
            _parent.ParseOutGoingCommand(null, "/close DCC Files");
        }

        private void DisconnectDCCChat_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                ((IceTabPage)findNode).DisconnectDCC();
        }

        private void ClearWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clear the window for the selected @window
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                ((IceTabPage)findNode).TextWindow.ClearTextWindow();

        }

        private void CloseWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            object findNode = FindNodeValue(selectedNodeIndex);
            if (findNode.GetType() == typeof(IceTabPage))
                _parent.ChannelBar.Controls.Remove(((IceTabPage)findNode));
        }


        private void RemoveServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _parent.FocusInputBox();

            if (selectedNodeIndex == 0 || selectedServerID == 0) return;

            ServerSetting s = GetServerSetting(selectedServerID);
            if (s != null)
            {                
                DialogResult ask = MessageBox.Show("Are you sure you want to remove  " + s.ServerName + " from the Server Tree?", "Remove Server", MessageBoxButtons.OKCancel);
                if (ask == DialogResult.OK)
                {
                    serversCollection.RemoveServer(s);
                    SaveServerSettings();
                }
                return;
            }
        }


        #endregion        


    }
}
