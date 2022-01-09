/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2022 Paul Vanderzee <snerf@icechat.net>
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IceChat
{
    public partial class FormWindow : Form
    {
        private IceTabPage dockedControl;
        private bool kickedChannel;
        private bool selectTabActivate = true;
        private bool allowClose = false;

        private int SYSMENU_ATTACH_MENU = 0x1;
        private int SYSMENU_DETACH_MENU = 0x2;

        // P/Invoke constants
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;

        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);


        public FormWindow(IceTabPage tab)
        {
            //used to hold each tab in MDI
            InitializeComponent();

            this.Controls.Add(tab);
            tab.Dock = DockStyle.Fill;
            dockedControl = tab;

            this.menuStrip.Visible = false;

            ToolStripMenuItemTB trackBar = new ToolStripMenuItemTB();
            trackBar.ValueChanged += new EventHandler(TrackBar_ValueChanged);
            viewToolStripMenuItem.DropDownItems.Add(trackBar);

            this.Activated += new EventHandler(OnActivated);
            this.Resize += new EventHandler(OnResize);
            this.Move += new EventHandler(OnMove);

            this.MouseDown += new MouseEventHandler(FormWindow_MouseDown);
            dockedControl.MouseDown+=new MouseEventHandler(FormWindow_MouseDown);

            this.FormClosing += new FormClosingEventHandler(OnFormClosing);

            if (tab.WindowStyle == IceTabPage.WindowType.Console)
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("console.png").GetHicon());
            else if (tab.WindowStyle == IceTabPage.WindowType.Channel)
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("channel.png").GetHicon());
            else if (tab.WindowStyle == IceTabPage.WindowType.Query)
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("query.png").GetHicon());
            else if (tab.WindowStyle == IceTabPage.WindowType.ChannelList)
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("channellist.png").GetHicon());
            else //for the rest
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("window-icon.ico").GetHicon());


        }

        internal void CreateAttachMenu()
        {
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ATTACH_MENU, "Attach Tab");
        }

        internal void CreateDetachMenu()
        {
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_DETACH_MENU, "Detach Tab");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_SYSCOMMAND) {
                if ((int)m.WParam == SYSMENU_ATTACH_MENU)
                {
                    FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/attach");
                }
                if ((int)m.WParam == SYSMENU_DETACH_MENU)
                {
                    FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/detach");
                }
            }
        }

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            int valueB = ((TrackBar)sender).Value;
            this.Opacity = (double)valueB / 100;
        }

        internal void DisableActivate()
        {
            this.Activated -= OnActivated;
            this.FormClosing -= OnFormClosing;
        }

        internal void DisableResize()
        {
            this.Resize -= OnResize;
            this.Move -= OnMove;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            //need to send the part message
            //System.Diagnostics.Debug.WriteLine("closing form:" + this.Text + ":" + this.Controls.Count + ":" + e.CloseReason);
            
            try
            {
                if (this.Controls.Count == 2)
                {
                    if (dockedControl.WindowStyle == IceTabPage.WindowType.Console)
                    {
                        if (e.CloseReason == CloseReason.UserClosing)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (FormMain.Instance.IceChatOptions.SaveWindowPosition == true)
                            {
                                ChannelSetting cs = FormMain.Instance.ChannelSettings.FindChannel("Console", "");
                                if (cs != null)
                                {
                                    cs.WindowLocation = this.Location;
                                    if (this.WindowState == FormWindowState.Normal)
                                        cs.WindowSize = this.Size;
                                }
                                else
                                {
                                    ChannelSetting cs1 = new ChannelSetting
                                    {
                                        ChannelName = "Console",
                                        NetworkName = "",
                                        WindowLocation = this.Location
                                    };
                                    if (this.WindowState == FormWindowState.Normal)
                                        cs1.WindowSize = this.Size;

                                    FormMain.Instance.ChannelSettings.AddChannel(cs1);

                                }
                                
                                FormMain.Instance.SaveChannelSettings();
                            }
                        }
                    }

                    if (dockedControl.WindowStyle == IceTabPage.WindowType.Channel)
                    {
                        if (FormMain.Instance.IceChatOptions.SaveWindowPosition == true)
                        {
                            ChannelSetting cs = FormMain.Instance.ChannelSettings.FindChannel(dockedControl.TabCaption, dockedControl.Connection.ServerSetting.NetworkName);
                            if (cs != null)
                            {
                                cs.WindowLocation = this.Location;
                                if (this.WindowState == FormWindowState.Normal)
                                    cs.WindowSize = this.Size;
                            }
                            else
                            {
                                ChannelSetting cs1 = new ChannelSetting
                                {
                                    ChannelName = dockedControl.TabCaption,
                                    NetworkName = dockedControl.Connection.ServerSetting.NetworkName,
                                    WindowLocation = this.Location
                                };
                                if (this.WindowState == FormWindowState.Normal)
                                    cs1.WindowSize = this.Size;

                                FormMain.Instance.ChannelSettings.AddChannel(cs1);
                            }

                            FormMain.Instance.SaveChannelSettings();
                        }

                        if (kickedChannel == false)
                        {
                            bool dontPart = true;                            
                            if (e.CloseReason == CloseReason.MdiFormClosing)
                            {
                                if (dockedControl.Connection.ServerSetting.UseBNC == true)
                                    dontPart = false;
                            }
                            
                            if (dontPart)
                                FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/part " + dockedControl.TabCaption);
                        }

                    }
                    else if (dockedControl.WindowStyle == IceTabPage.WindowType.Query)
                        FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/close " + dockedControl.TabCaption);
                    else
                    {
                        //remove this from the channel bar
                        FormMain.Instance.RemoveWindow(dockedControl.Connection, dockedControl.TabCaption, dockedControl.WindowStyle);
                    }
                    
                }
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.Message);
                System.Diagnostics.Debug.WriteLine(ee.StackTrace);
            }
        }

        private void OnMove(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("move window:" + this.Location.X);
            if (this.WindowState == FormWindowState.Normal)
            {
                dockedControl.WindowLocation = this.Location;
            }
        }

        private void OnResize(object sender, EventArgs e)
        {
            int howfar = 0;
            try
            {
                howfar = 1;
                //change back to tabbed view if maximized
                if (this.WindowState == FormWindowState.Maximized)
                {
                    howfar = 2;
                    if (dockedControl.Detached == false)
                    {
                        //this gets called for all forms, disable them all
                        foreach (FormWindow child in FormMain.Instance.MdiChildren)
                        {
                            child.DisableResize();
                        }

                        FormMain.Instance.ReDockTabs();
                    }
                    howfar = 3;
                }
                else
                    dockedControl.WindowSize = this.Size;

                howfar = 4;
            }
            catch (NullReferenceException nre)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "Resize NRE FormWindow Error:" + howfar, nre);
            }
            catch (Exception ex)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "Resize FormWindow Error:" + howfar, ex);
            }
        }

        private void FormWindow_MouseDown(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("FormWindow MouseDown:" + this.Text);
        }


        private void OnActivated(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Form Window OnActivated:" + this.Visible);
            try
            {
                FormMain.Instance.ChannelBar.SelectTab(dockedControl);
                if (dockedControl.WindowStyle == IceTabPage.WindowType.Query)
                {
                    if (dockedControl.Connection.ServerSetting.IAL.ContainsKey(dockedControl.TabCaption))
                    {

                        this.UIThread(delegate
                        {
                            this.Text = dockedControl.TabCaption + " (" + ((InternalAddressList)dockedControl.Connection.ServerSetting.IAL[dockedControl.TabCaption]).Host + ") {" + dockedControl.Connection.ServerSetting.NetworkName + "}";
                        });
                    }
                }

                if (this.Text == "Console")
                {
                    //dont do anything, or ya get the BUGZ!    
                    // FormMain.Instance.ServerTree.SelectTab(dockedControl, true);
                }
                else
                    FormMain.Instance.ServerTree.SelectTab(dockedControl, false);

                
                //set the tabindex to 0
                dockedControl.TabIndex = 0;

                //set the rest to 1
                foreach (FormWindow child in FormMain.Instance.MdiChildren)
                {
                    if (child != this)
                    {
                        IceTabPage tab = child.DockedControl;
                        tab.TabIndex = 1;
                    }
                }


                if (dockedControl.Detached)
                    dockedControl.InputPanel.FocusTextBox();
                else
                    FormMain.Instance.FocusInputBox();

            }
            catch (Exception ex)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "Activate FormWindow Error:", ex);
            }
        }

        internal bool AllowClose
        {
            get { return allowClose; }
            set { allowClose = value;}
        }

        internal MenuStrip MainMenu
        {
            get { return menuStrip; }
        }

        internal IceTabPage DockedControl
        {
            get { return dockedControl; }
        }

        internal bool KickedChannel
        {
            get { return kickedChannel; }
            set { kickedChannel = value; }
        }

        internal bool SelectTabActivate
        {
            get { return selectTabActivate; }
            set { selectTabActivate = value; }
        }

        private void AttachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //re-attach the tab
            FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/attach");
        }

        private void AlwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (alwaysOnTopToolStripMenuItem.Checked)
            {
                //remove always on top
                this.TopMost = false;
            }
            else
            {
                //make window always on top
                this.TopMost = true;
            }

            alwaysOnTopToolStripMenuItem.Checked = !alwaysOnTopToolStripMenuItem.Checked;

        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close this window
            this.Close();
        }

        private void NickListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show or hide the nick list

        }

        private void HideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuStrip.Hide();
        }
    }
    
    [System.ComponentModel.DesignerCategory("code")]
    [System.Windows.Forms.Design.ToolStripItemDesignerAvailability(System.Windows.Forms.Design.ToolStripItemDesignerAvailability.ContextMenuStrip | System.Windows.Forms.Design.ToolStripItemDesignerAvailability.MenuStrip)]
    public partial class ToolStripMenuItemTB : ToolStripControlHost
    {
        public ToolStripMenuItemTB()
            : base(CreateControlInstance())
        {
        }
        /// <summary>
        /// Create a strongly typed property called TrackBar - handy to prevent casting everywhere.
        /// </summary>
        public TrackBar TrackBar
        {
            get
            {
                return Control as TrackBar;
            }
        }
        /// <summary>
        /// Create the actual control, note this is static so it can be called from the
        /// constructor.
        ///
        /// </summary>
        /// <returns></returns>
        private static Control CreateControlInstance()
        {
            TrackBar t = new TrackBar
            {
                AutoSize = false,
                Minimum = 25,
                Maximum = 100,
                SmallChange = 1,
                LargeChange = 5,
                TickStyle = TickStyle.None,
                Value = 100
            };

            // Add other initialization code here.
            return t;
        }
        [DefaultValue(0)]
        public int Value
        {
            get { return TrackBar.Value; }
            set { TrackBar.Value = value; }
        }
        /// <summary>
        /// Attach to events we want to re-wrap
        /// </summary>
        /// <param name="control"></param>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            trackBar.ValueChanged += new EventHandler(TrackBar_ValueChanged);
        }
        /// <summary>
        /// Detach from events.
        /// </summary>
        /// <param name="control"></param>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            trackBar.ValueChanged -= new EventHandler(TrackBar_ValueChanged);
        }
        /// <summary>
        /// Routing for event
        /// TrackBar.ValueChanged -> ToolStripTrackBar.ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            // when the trackbar value changes, fire an event.
            if (this.ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }
        // add an event that is subscribable from the designer.
        public event EventHandler ValueChanged;
        // set other defaults that are interesting
        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 10);
            }
        }
    }
}
