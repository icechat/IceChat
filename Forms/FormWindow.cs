using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormWindow : Form
    {
        private IceTabPage dockedControl;
        private bool kickedChannel;
        private bool selectTabActivate = true;
        private bool allowClose = false;

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
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("new-query.ico").GetHicon());
            else if (tab.WindowStyle == IceTabPage.WindowType.ChannelList)
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("channellist.png").GetHicon());
            else //for the rest
                this.Icon = System.Drawing.Icon.FromHandle(StaticMethods.LoadResourceImage("window-icon.ico").GetHicon());

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
            
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                //e.Cancel = true;
                //return;
            }

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
                                    ChannelSetting cs1 = new ChannelSetting();
                                    cs1.ChannelName = "Console";
                                    cs1.NetworkName = "";
                                    cs1.WindowLocation = this.Location;
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
                            //System.Diagnostics.Debug.WriteLine("closing channel:" + this.Location);
                            
                            ChannelSetting cs = FormMain.Instance.ChannelSettings.FindChannel(dockedControl.TabCaption, dockedControl.Connection.ServerSetting.NetworkName);
                            if (cs != null)
                            {
                                cs.WindowLocation = this.Location;
                                if (this.WindowState == FormWindowState.Normal)
                                    cs.WindowSize = this.Size;
                            }
                            else
                            {
                                ChannelSetting cs1 = new ChannelSetting();
                                cs1.ChannelName = dockedControl.TabCaption;
                                cs1.NetworkName = dockedControl.Connection.ServerSetting.NetworkName;
                                cs1.WindowLocation = this.Location;
                                if (this.WindowState == FormWindowState.Normal)
                                    cs1.WindowSize = this.Size;

                                FormMain.Instance.ChannelSettings.AddChannel(cs1);
                            }

                            FormMain.Instance.SaveChannelSettings();
                        }
                        
                        if (kickedChannel == false)
                            FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/part " + dockedControl.TabCaption);

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

        private void OnControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is IceTabPage)
            {
                //System.Diagnostics.Debug.WriteLine("remove tab (form):" + ((IceTabPage)e.Control).TabCaption + ":" + this.Controls.Count);
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
            //change back to tabbed view if maximized
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (dockedControl.Detached == false)
                {
                    //this gets called for all forms, disable them all
                    foreach (FormWindow child in FormMain.Instance.MdiChildren)
                    {
                        child.DisableResize();
                    }

                    FormMain.Instance.ReDockTabs();
                }
            }        
            else
                dockedControl.WindowSize = this.Size;

        }

        private void FormWindow_MouseDown(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("FormWindow MouseDown:" + this.Text);
        }


        private void OnActivated(object sender, EventArgs e)
        {
            FormMain.Instance.ChannelBar.SelectTab(dockedControl);

            if (this.Text == "Console")
            {
                //dont do anything, or ya get the BUGZ!
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

        private void attachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //re-attach the tab
            FormMain.Instance.ParseOutGoingCommand(dockedControl.Connection, "/attach");
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //close this window
            this.Close();
        }

        private void nickListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //show or hide the nick list

        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
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
            TrackBar t = new TrackBar();
            t.AutoSize = false;

            t.Minimum = 25;
            t.Maximum = 100;
            t.SmallChange = 1;
            t.LargeChange = 5;
            t.TickStyle = TickStyle.None;
            t.Value = 100;

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
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
        }
        /// <summary>
        /// Detach from events.
        /// </summary>
        /// <param name="control"></param>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            TrackBar trackBar = control as TrackBar;
            trackBar.ValueChanged -= new EventHandler(trackBar_ValueChanged);
        }
        /// <summary>
        /// Routing for event
        /// TrackBar.ValueChanged -> ToolStripTrackBar.ValueChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trackBar_ValueChanged(object sender, EventArgs e)
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
