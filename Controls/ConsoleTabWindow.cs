/******************************************************************************\
 * IceChat 2009 Internet Relay Chat Client
 *
 * Copyright (C) 2009 Paul Vanderzee <snerf@icechat.net>
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
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;

namespace IceChat
{
    public class ConsoleTabWindow : System.Windows.Forms.TabPage
    {
        private TabControl consoleTab;
        private FormMain.ServerMessageType lastMessageType;

        private delegate TextWindow CurrentWindowDelegate();


        public ConsoleTabWindow() : base()
        {
            InitializeComponent();
            /*
            consoleTab.DrawMode = TabDrawMode.OwnerDrawFixed;
            consoleTab.SizeMode = TabSizeMode.Normal;
            consoleTab.SelectedIndexChanged += new EventHandler(OnSelectedIndexChanged);
            consoleTab.MouseUp += new MouseEventHandler(OnMouseUp);
            consoleTab.MouseDown += new MouseEventHandler(OnMouseDown);
            consoleTab.DrawItem += new DrawItemEventHandler(OnDrawItem);
            consoleTab.ControlRemoved += new ControlEventHandler(OnControlRemoved);
            consoleTab.Selecting += new TabControlCancelEventHandler(OnSelectingTab);
            */
        }


        /// <summary>
        /// Add a message to the Text Window for Selected Console Tab Connection
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        /// <param name="color"></param>
        internal void AddText(IRCConnection connection, string data, int color, bool scrollToBottom)
        {
            foreach (ConsoleTab t in consoleTab.TabPages)
            {
                if (t.Connection == connection)
                {
                    ((TextWindow)t.Controls[0]).AppendText(data, color);
                    if (scrollToBottom)
                        ((TextWindow)t.Controls[0]).ScrollToBottom();
                    return;
                }
            }
        }
        
        /// <summary>
        /// Return the Console Tab Control
        /// </summary>
        internal TabControl ConsoleTab
        {
            get { return consoleTab; }
        }
        
        /// <summary>
        /// Return the Text Window for the Current Selected Tab in the Console Tab Control
        /// </summary>
        internal TextWindow CurrentWindow()
        {
            if (this.InvokeRequired)
            {
                CurrentWindowDelegate cwd = new CurrentWindowDelegate(CurrentWindow);
                return (TextWindow)this.Invoke(cwd, new object[] { });
            }
            else
            {
                return (TextWindow)consoleTab.SelectedTab.Controls[0];
            }
        }
        
        /// <summary>
        /// Return the Connection for the Current Selected in the Console Tab Control
        /// </summary>
        internal IRCConnection CurrentConnection
        {
            get
            {
                return ((ConsoleTab)consoleTab.SelectedTab).Connection;
            }
        }
        
        /// <summary>
        /// Get/Set the last message type
        /// </summary>
        internal FormMain.ServerMessageType LastMessageType
        {
            get
            {
                return lastMessageType;
            }
            set
            {
                if (lastMessageType != value)
                {
                    lastMessageType = value;
                    //repaint the tab
                    ((IceTabControl)this.Parent).Invalidate(); 
                    FormMain.Instance.ServerTree.Invalidate();
                }
            }
        }

        /// <summary>
        /// Add a new Tab/Connection to the Console Tab Control
        /// </summary>
        /// <param name="connection"></param>
        internal void AddConsoleTab(IRCConnection connection)
        {
            ConsoleTab t = new ConsoleTab(connection.ServerSetting.ServerName);
            t.Connection = connection;

            TextWindow w = new TextWindow();
            w.Dock = DockStyle.Fill;
            w.Font = new System.Drawing.Font(FormMain.Instance.IceChatFonts.FontSettings[0].FontName, FormMain.Instance.IceChatFonts.FontSettings[0].FontSize);
            w.IRCBackColor = FormMain.Instance.IceChatColors.ConsoleBackColor;
            w.NoEmoticons = true;

            t.Controls.Add(w);
            if (FormMain.Instance.IceChatOptions.LogConsole)
                w.SetLogFile(FormMain.Instance.LogsFolder + System.IO.Path.DirectorySeparatorChar + connection.ServerSetting.ServerName);
            
            consoleTab.TabPages.Add(t);
            consoleTab.SelectedTab = t;

        }
        /// <summary>
        /// Temporary Method to create a NULL Connection for the Welcome Tab in the Console
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="serverName"></param>
        internal void AddConsoleTab(string serverName)
        {
            //this is only used for now, to show the "Welcome" Tab
            ConsoleTab t = new ConsoleTab(serverName);

            TextWindow w = new TextWindow();
            w.Dock = DockStyle.Fill;
            w.Font = new System.Drawing.Font(FormMain.Instance.IceChatFonts.FontSettings[0].FontName, FormMain.Instance.IceChatFonts.FontSettings[0].FontSize);
            w.IRCBackColor = FormMain.Instance.IceChatColors.ConsoleBackColor;

            t.Controls.Add(w);
            consoleTab.TabPages.Add(t);
            consoleTab.SelectedTab = t;

        }

        /// <summary>
        /// Console Tab Has New Tab Selected
        /// Update the Status Text accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (consoleTab.TabPages.IndexOf(consoleTab.SelectedTab) != 0)
            {
                FormMain.Instance.InputPanel.CurrentConnection = ((ConsoleTab)consoleTab.SelectedTab).Connection;

                if (((ConsoleTab)consoleTab.SelectedTab).Connection.IsConnected)
                {
                    if (((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.RealServerName != null)
                        FormMain.Instance.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NickName + " connected to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.RealServerName);
                    else
                        FormMain.Instance.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NickName + " connected to " + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.ServerName);
                }
                else
                {
                    FormMain.Instance.StatusText(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.NickName + " disconnected (" + ((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting.ServerName + ")");
                }

                //highlite the proper item in the server tree
                FormMain.Instance.ServerTree.SelectTab(((ConsoleTab)consoleTab.SelectedTab).Connection.ServerSetting);
            }
            else
            {
                FormMain.Instance.ServerTree.SelectTab(this);
                FormMain.Instance.InputPanel.CurrentConnection = null;
                FormMain.Instance.StatusText("Welcome to IceChat 2009");
            }
        }

        private void OnControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control.GetType() == typeof(ConsoleTab))
                ((TextWindow)((ConsoleTab)e.Control).Controls[0]).CloseLogFile(); ;
        }


        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            string name = consoleTab.TabPages[e.Index].Text;
            Rectangle bounds = e.Bounds;
            e.Graphics.FillRectangle(new SolidBrush(Color.White), bounds);

            if (e.Index == consoleTab.SelectedIndex)
            {
                bounds.Offset(4, 2);
                e.Graphics.DrawString(name, this.Font, new SolidBrush(Color.Red), bounds);
                bounds.Offset(0, -1);
            }
            else
            {
                bounds.Offset(2, 3);
                e.Graphics.DrawString(name, this.Font, new SolidBrush(Color.Black), bounds);
                bounds.Offset(4, -2);
            }
            if (e.Index != 0 && e.Index == consoleTab.SelectedIndex)
            {
                System.Drawing.Image icon = new Bitmap(Properties.Resources.CloseButton);
                e.Graphics.DrawImage(icon, bounds.Right - 20, bounds.Top + 4, 12, 12);
                icon.Dispose();
            }

        }

        private void OnSelectingTab(object sender, TabControlCancelEventArgs e)
        {            
            if (consoleTab.GetTabRect(e.TabPageIndex).Contains(consoleTab.PointToClient(Cursor.Position)) && e.TabPageIndex != 0)
            {
                if (this.PointToClient(Cursor.Position).X > consoleTab.GetTabRect(e.TabPageIndex).Right - 14)
                    e.Cancel = true;
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            FormMain.Instance.FocusInputBox();
        }

        /// <summary>
        /// Checks if Middle Mouse Button is Pressed
        /// Quits Server if Server is Connected
        /// Closes Server Tab if Server is Disconnected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = consoleTab.TabPages.Count - 1; i >= 0; i--)
                {
                    if (consoleTab.GetTabRect(i).Contains(e.Location) && i == consoleTab.SelectedIndex)
                    {
                        if (((ConsoleTab)consoleTab.TabPages[i]).Connection != null)
                        {
                            if (e.Location.X > consoleTab.GetTabRect(i).Right - 14)
                            {
                                if (((ConsoleTab)consoleTab.TabPages[i]).Connection.IsConnected)
                                {
                                    if (((ConsoleTab)consoleTab.TabPages[i]).Connection.IsFullyConnected)
                                    {
                                        ((ConsoleTab)consoleTab.TabPages[i]).Connection.AttemptReconnect = false;
                                        ((ConsoleTab)consoleTab.TabPages[i]).Connection.SendData("QUIT :" + ((ConsoleTab)consoleTab.TabPages[i]).Connection.ServerSetting.QuitMessage);
                                        return;
                                    }
                                }

                                //close all the windows related to this tab
                                FormMain.Instance.CloseAllWindows(((ConsoleTab)consoleTab.TabPages[i]).Connection);
                                //remove the server connection from the collection
                                ((ConsoleTab)consoleTab.TabPages[i]).Connection.Dispose();
                                FormMain.Instance.ServerTree.ServerConnections.Remove(((ConsoleTab)consoleTab.TabPages[i]).Connection.ServerSetting.ID);
                                consoleTab.TabPages.RemoveAt(consoleTab.TabPages.IndexOf(consoleTab.TabPages[i]));
                                return;
                            }
                        }
                    }
                }
            }
        }
		
        private void InitializeComponent()
        {
            this.consoleTab = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // consoleTab
            // 
            this.consoleTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTab.Font = new System.Drawing.Font("Verdana", 10F);
            this.consoleTab.Location = new System.Drawing.Point(0, 0);
            this.consoleTab.Name = "consoleTab";
            this.consoleTab.SelectedIndex = 0;
            this.consoleTab.Size = new System.Drawing.Size(200, 100);
            this.consoleTab.TabIndex = 0;
            // 
            // ConsoleTabWindow
            // 
            this.Controls.Add(this.consoleTab);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImageIndex = 0;
            this.Text = "Console";
            this.ResumeLayout(false);

        }
    
    }

    public class ConsoleTab : System.Windows.Forms.TabPage
    {
        public IRCConnection Connection;

        public ConsoleTab(string serverName) : base()         
        {
            base.Text = serverName;
        }
    }
}
