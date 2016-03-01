/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2016 Paul Vanderzee <snerf@icechat.net>
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
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using IceChatPlugin;


namespace IceChat
{
    public class IceTabPageDCCFile : IceTabPage
    {
        private List<DccFileStruct> dccFiles = new List<DccFileStruct>();
        private delegate void AddDCCFileDelegate(DccFileStruct dcc);
        private delegate void RemoveDCCFileDelegate(DccFileStruct dcc);
        private delegate void UpdateDCCFileProgressDelegate(DccFileStruct dcc);
        private delegate void UpdateDCCFileStatusDelegate(DccFileStruct dcc, string value);
        private FlickerFreeListView dccFileList;
        private FormMain _parent;

        private void InitializeComponent()
        {
            this.dccFileList = new FlickerFreeListView();
            this.dccFileList.SuspendLayout();
            this.SuspendLayout();
            
            Panel dccPanel = new Panel();
            dccPanel.BackColor = Color.LightGray;
            dccPanel.Size = new Size(this.Width, 45);
            dccPanel.Dock = DockStyle.Bottom;

            Button dccCancel = new Button();
            dccCancel.Name = "dccCancel";
            dccCancel.Click += new EventHandler(dccCancel_Click);
            dccCancel.Location = new Point(5, 5);
            dccCancel.Size = new Size(100, 35);
            dccCancel.Text = "Cancel";
            dccCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dccCancel.UseVisualStyleBackColor = true;
            dccPanel.Controls.Add(dccCancel);

            Button dccOpen = new Button();
            dccOpen.Name = "dccOpen";
            dccOpen.Click += new EventHandler(dccOpen_Click);
            dccOpen.Location = new Point(110, 5);
            dccOpen.Size = new Size(100, 35);
            dccOpen.Text = "Open Folder";
            dccOpen.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dccOpen.UseVisualStyleBackColor = true;
            dccPanel.Controls.Add(dccOpen);

            Button dccRemove = new Button();
            dccRemove.Name = "dccRemove";
            dccRemove.Click += new EventHandler(dccRemove_Click);
            dccRemove.Location = new Point(220, 5);
            dccRemove.Size = new Size(100, 35);
            dccRemove.Text = "Remove";
            dccRemove.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dccRemove.UseVisualStyleBackColor = true;
            dccPanel.Controls.Add(dccRemove);

            Button dccResend = new Button();
            dccResend.Name = "dccResend";
            dccResend.Click += new EventHandler(dccResend_Click);
            dccResend.Location = new Point(330, 5);
            dccResend.Size = new Size(100, 35);
            dccResend.Text = "Resend";
            dccResend.Visible = false;
            dccResend.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dccResend.UseVisualStyleBackColor = true;
            dccPanel.Controls.Add(dccResend);


            this.dccFileList.Dock = DockStyle.Fill;
            this.dccFileList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dccFileList.View = View.Details;
            this.dccFileList.ShowItemToolTips = true;
            this.dccFileList.MultiSelect = false;
            this.dccFileList.FullRowSelect = true;
            this.dccFileList.HideSelection = false;
            this.dccFileList.DoubleClick += new EventHandler(dccFileList_DoubleClick);

            ColumnHeader fn = new ColumnHeader();
            fn.Text = "File Name";
            fn.Width = 200;
            fn.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.dccFileList.Columns.Add(fn);

            ColumnHeader n = new ColumnHeader();
            n.Text = "Nick";
            n.Width = 80;
            this.dccFileList.Columns.Add(n);

            ColumnHeader fs = new ColumnHeader();
            fs.Text = "File Size";
            fs.Width = 200;
            this.dccFileList.Columns.Add(fs);

            ColumnHeader sp = new ColumnHeader();
            sp.Text = "Speed";
            sp.Width = 100;
            this.dccFileList.Columns.Add(sp);

            ColumnHeader el = new ColumnHeader();
            el.Text = "Elapsed";
            el.Width = 100;
            this.dccFileList.Columns.Add(el);

            ColumnHeader s = new ColumnHeader();
            s.Text = "Status";
            s.Width = 100;
            s.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.dccFileList.Columns.Add(s);


            //store the dcc file style (upload/download)
            ColumnHeader st = new ColumnHeader();
            st.Text = "Style";
            st.Width = 0;
            st.AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.dccFileList.Columns.Add(st);
            this.dccFileList.Columns[6].Width = 0;

            //store the server id of the connection
            ColumnHeader sid = new ColumnHeader();
            sid.Text = "ServerID";
            sid.Width = 0;
            sid.AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.dccFileList.Columns.Add(sid);
            this.dccFileList.Columns[7].Width = 0;


            //store the path/folder for the dcc file
            ColumnHeader pa = new ColumnHeader();
            pa.Text = "Path";
            pa.Width = 0;
            pa.AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.dccFileList.Columns.Add(pa);
            this.dccFileList.Columns[8].Width = 0;

            this.Controls.Add(dccFileList);
            this.Controls.Add(dccPanel);
            this.dccFileList.ResumeLayout(false);
            this.ResumeLayout(false);
        }


        private void dccResend_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public IceTabPageDCCFile(WindowType windowType, string sCaption, FormMain parent) : base(windowType, sCaption, parent)
        {

            InitializeComponent();
            this._parent = parent;

            dccFiles = new List<DccFileStruct>();

        }
        
        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("disposing:" + dccFiles.Count);
            foreach (DccFileStruct dcc in dccFiles)
            {
                try
                {
                    if (dcc.Thread != null)
                        if (dcc.Thread.IsAlive == true)
                            dcc.Thread.Abort();

                    if (dcc.Socket != null)
                        if (dcc.Socket.Connected == true)
                            dcc.Socket.Close();

                    dcc.Socket = null;
                    dcc.Thread = null;

                    if (dcc.ListenerSocket != null)
                    {
                        dcc.ListenerSocket.Stop();
                        dcc.ListenerSocket = null;
                    }

                    if (dcc.ListenerThread != null)
                    {
                        dcc.keepListening = false;
                    }

                    if (dcc.timeoutTimer != null)
                    {
                        dcc.timeoutTimer.Stop();
                        dcc.timeoutTimer = null;
                    }
                }
                catch (ThreadAbortException ta)
                {
                    System.Diagnostics.Debug.WriteLine(ta.Message + ":" + ta.StackTrace);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message + ":" + e.StackTrace);
                }

                if (!dcc.Finished)
                {
                    try
                    {
                        if (dcc.FileStream != null)
                        {
                            System.Diagnostics.Debug.WriteLine(dcc.FileStream.SafeFileHandle);
                            System.Diagnostics.Debug.WriteLine(dcc.FileName);
                            //dcc.FileStream.Flush();
                            dcc.FileStream.Close();
                        }
                    }
                    catch { }
                }
            }
            for (int i = dccFiles.Count; i > 0; i--)
                dccFiles.RemoveAt(0);

            dccFiles.Clear();
            dccFiles = null;

        }

        /// <summary>
        /// Open the folder for the File Double Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dccFileList_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //find the item, and open the folder
                    foreach (DccFileStruct dcc in dccFiles)
                    {
                        if (dcc.ListingTag.ToString() == lvi.Tag.ToString())
                        {
                            System.Diagnostics.Process.Start(dcc.Path);
                            return;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Remove a cancelled or completed item from the Dcc File List
        /// </summary>
        private void dccRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //check if it is finished
                    if (lvi.SubItems[5].Text == "Completed" || lvi.SubItems[5].Text == "Cancelled" || lvi.SubItems[5].Text == "ERROR")
                    {
                        //we can remove this item
                        int x = -1;
                        for (int i = 0; i < dccFiles.Count; i++)
                        {
                            if (dccFiles[i].ListingTag.ToString() == lvi.Tag.ToString())
                                x = i;
                        }
                        
                        if (x > -1)
                            RemoveDCCFile(dccFiles[x]);
                        return;
                    }
                    else
                    {
                        //it is not finished, do you wish to cancel it?
                        //find the appropriate matching item                        
                        foreach (DccFileStruct dcc in dccFiles)
                        {
                            if (dcc.ListingTag.ToString() == lvi.Tag.ToString())
                            {
                                DialogResult dialog = MessageBox.Show("The file is still in progress, do you wish to cancel it?", "Cancel File", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.No)
                                    return;
                                
                                if (dcc.Socket != null)
                                    dcc.Socket.Close();

                                if (dcc.Thread != null)
                                    dcc.Thread.Abort();
                                
                                lvi.SubItems[5].Text = "Cancelled";
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Open the folder for the File Selected
        /// </summary>
        private void dccOpen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //find the item, and open the folder
                    foreach (DccFileStruct dcc in dccFiles)
                    {
                        if (dcc.ListingTag.ToString() == lvi.Tag.ToString())
                        {
                            System.Diagnostics.Process.Start(dcc.Path);
                            return;
                        }
                    }
                }
            }
            
            System.Diagnostics.Process.Start(FormMain.Instance.IceChatOptions.DCCReceiveFolder);
        }

        /// <summary>
        /// Cancel the File Transfer for the File Selected
        /// </summary>
        private void dccCancel_Click(object sender, EventArgs e)
        {
            //find which item is being asked to be canceled
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //check if it is finished
                    if (lvi.SubItems[5].Text == "Completed" || lvi.SubItems[5].Text == "Cancelled" || lvi.SubItems[5].Text == "ERROR")
                    {
                        //do nothing
                        return;
                    }
                    else
                    {
                        //it is not finished, do you wish to cancel it?
                        //find the appropriate matching item                        
                        System.Diagnostics.Debug.WriteLine("selected:" + lvi.Tag.ToString() + ":" + dccFiles.Count);
                        foreach (DccFileStruct dcc in dccFiles)
                        {
                            System.Diagnostics.Debug.WriteLine("checking:" + dcc.ListingTag.ToString());
                            if (dcc.ListingTag.ToString() == lvi.Tag.ToString())
                            {
                                DialogResult dialog = MessageBox.Show("The file is still in progress, do you wish to cancel it?", "Cancel File", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.No)
                                    return;

                                if (dcc.Socket != null)
                                {
                                    if (dcc.Socket.Connected)
                                        dcc.Socket.Close();
                                }
                                //dcc.FileStream.Flush();
                                if (dcc.FileStream != null)
                                    dcc.FileStream.Close();

                                lvi.SubItems[5].Text = "Cancelled";
                                return;
                            }
                        }
                    }
                }
            }
        }

        internal void ResumeDCCFile(IRCConnection connection, string port, uint filePos)
        {
            for (int i = 0; i < dccFiles.Count; i++)
            {
                if (dccFiles[i].Port == port && dccFiles[i].Connection == connection)
                {

                    System.Diagnostics.Debug.WriteLine("resume the file in here:" + dccFiles[i].FileName + ":" + dccFiles[i].Path);
                    System.Diagnostics.Debug.WriteLine("found a match on port " + port);

                    dccFiles[i].Resume = true;
                    dccFiles[i].TotalBytesRead = filePos;
                    dccFiles[i].StartFileSize = filePos;

                    dccFiles[i].Socket = null;
                    dccFiles[i].Socket = new TcpClient();

                    dccFiles[i].FileStream.Close();
                    dccFiles[i].FileStream = null;
                    dccFiles[i].FileStream = new FileStream(dccFiles[i].Path + System.IO.Path.DirectorySeparatorChar + dccFiles[i].FileName, FileMode.Append);
                    
                    UpdateDCCFileStatus(dccFiles[i], "W-RESUME");

                    dccFiles[i].Thread = new Thread(new ParameterizedThreadStart(ResumeDCC));
                    dccFiles[i].Thread.Name = "DCCFileThreadResume";
                    dccFiles[i].Thread.Start(dccFiles[i]);
                    
                    break;

                }
            }
        }

        internal void StartDCCPassive(IRCConnection connection, string nick, string host, string ip, string file, uint fileSize, string id)
        {
            //open a new dcc listening port, and send back to the client
            System.Diagnostics.Debug.WriteLine("start passive dcc - open listener:" + id);
            DccFileStruct dcc = new DccFileStruct();
            dcc.FileName = file;
            dcc.FileSize = fileSize;
            dcc.StartFileSize = 0;
            dcc.Nick = nick;
            dcc.Host = host;
            dcc.Connection = connection;
            dcc.Ip = ip;
            dcc.passiveID = id;

            dcc.Style = "Passive";

            //pick a random incoming port
            Random port = new Random();
            int p = port.Next(FormMain.Instance.IceChatOptions.DCCPortLower, FormMain.Instance.IceChatOptions.DCCPortUpper);
            dcc.Port = p.ToString();

            //create a random number for a tag
            dcc.ListingTag = RandomListingTag();

            try
            {
                string dccPath = FormMain.Instance.IceChatOptions.DCCReceiveFolder;
                //check to make sure the folder exists
                if (!Directory.Exists(dccPath))
                {
                    //add a folder browsing dialog here
                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    if (fbd.ShowDialog() == DialogResult.OK)
                        dccPath = fbd.SelectedPath;
                    else
                    {
                        //no folder selected, out we go
                        System.Diagnostics.Debug.WriteLine("PASSIVE No folder selected, non-existant dcc receive folder");
                        FormMain.Instance.WindowMessage(connection, "Console", "\x000304DCC Passive File Received Failed : DCC Receive Path does not exists", "", true);
                        return;
                    }
                }

                //check if the file exists
                if (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName))
                {
                    //check the local file size and compare to what is being sent
                    FileInfo fi = new FileInfo(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName);
                    if (fi.Length <= dcc.FileSize)
                    {
                        
                        System.Diagnostics.Debug.WriteLine("PASSIVE appending file:" + fi.Length + ":" + dcc.FileSize + ":" + connection.IsFullyConnected);
                        //send DCC RESUME
                        //wait for a DCC ACCEPT from client, and start resume on this port
                        connection.SendData("PRIVMSG " + nick + " :\x0001DCC RESUME \"" + dcc.FileName + "\" " + dcc.Port + " " + fi.Length.ToString() + "\x0001");
                        dcc.Resume = true;
                        dcc.TotalBytesRead = (uint)fi.Length;
                        dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Append);
                        dcc.Path = dccPath;
                        dcc.StartFileSize = dcc.TotalBytesRead;
                        System.Diagnostics.Debug.WriteLine(dccFiles.Count + ":" + dccFileList.Items.Count);
                        
                        dccFiles.Add(dcc);
                        return;
                    }
                    else
                    {
                        //file exists, and already complete // set a new filename adding [#] to the end of the fielname
                        int extPos = dcc.FileName.LastIndexOf('.');
                        if (extPos == -1)
                        {
                            int i = 0;
                            do
                            {
                                i++;
                            } while (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName + "(" + i.ToString() + ")"));
                            dcc.FileName += "(" + i.ToString() + ")";
                        }
                        else
                        {
                            string fileName = dcc.FileName.Substring(0, extPos);
                            string ext = dcc.FileName.Substring(extPos + 1);
                            int i = 0;
                            do
                            {
                                i++;
                            } while (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + fileName + "(" + i.ToString() + ")." + ext));
                            dcc.FileName = fileName + "(" + i.ToString() + ")." + ext;
                        }
                    }
                }

                dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Create);
                dcc.Path = dccPath;

                dcc.PassiveSocket = new TcpListener(new IPEndPoint(IPAddress.Any, Convert.ToInt32(p)));
                dcc.PassiveThread = new Thread(new ParameterizedThreadStart(StartPassiveSocket));
                dcc.PassiveThread.Name = "DCCPassiveThread";
                dcc.PassiveThread.Start(dcc);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("passive dcc file error:" + ex.Message);
                
                dcc.FileStream.Close();
            }
        }

        private void timeoutTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //dcc send has timed out
            foreach (DccFileStruct dcc in dccFiles)
            {
                if (dcc.timeoutTimer == (System.Timers.Timer)sender)
                {
                    System.Diagnostics.Debug.WriteLine("found time out timer");
                    UpdateDCCFileStatus(dcc, "Timed out");
                    try
                    {
                        dcc.ListenerSocket.Stop();
                    }
                    catch (SocketException se)
                    {
                        System.Diagnostics.Debug.WriteLine("stop listener error:" + se.Message);
                    }
                }
            }
        }


        internal void RequestDCCFile(IRCConnection connection , string nick, string file)
        {
            //send out a dccfile request
            string localIP = "";
            if (FormMain.Instance.IceChatOptions.DCCLocalIP != null && FormMain.Instance.IceChatOptions.DCCLocalIP.Length > 0)
            {
                localIP = IPAddressToLong(IPAddress.Parse(FormMain.Instance.IceChatOptions.DCCLocalIP)).ToString();
            }
            else
            {
                if (connection.ServerSetting.LocalIP == null || connection.ServerSetting.LocalIP.ToString().Length == 0)
                {
                    //error. no local IP found
                    FormMain.Instance.WindowMessage(connection, "Console", "\x000304DCC ERROR, no Router/Firewall IP Address specified in DCC Settings", "", true);
                    return;
                }
                else
                {
                    localIP = IPAddressToLong(connection.ServerSetting.LocalIP).ToString();
                }
            }

            
            Random port = new Random();
            int p = port.Next(FormMain.Instance.IceChatOptions.DCCPortLower, FormMain.Instance.IceChatOptions.DCCPortUpper);

            //DccFileStruct dcc = new DccFileStruct();
            DccFileStruct dcc = new DccFileStruct();
            dcc.FileStream = new FileStream(file, FileMode.Open);

            FileInfo f = new FileInfo(file);
            dcc.FileSize = (uint)f.Length;
            dcc.StartFileSize = 0;

            dcc.FileName = file;
            dcc.Nick = nick;
            dcc.Style = "Upload";
            dcc.Connection = connection;
            dcc.Port = p.ToString();
            dcc.LocalIP = localIP;

            dcc.ListingTag = RandomListingTag();
            string fname = dcc.FileName.Replace(' ', '_'); // strip spaces from filename
            //get the file from the path
            fname = Path.GetFileName(fname);
            dcc.SendFileName = fname;

            dcc.ListenerSocket = new TcpListener(new IPEndPoint(IPAddress.Any, Convert.ToInt32(p)));
            dcc.ListenerThread = new Thread(new ParameterizedThreadStart(ListenForConnection));
            dcc.ListenerThread.Name = "DCCListenerThread";
            dcc.ListenerThread.Start(dcc);
                

            //dcc.Connection.SendData("PRIVMSG " + dcc.Nick + " :DCC SEND " + fname + " " + localIP + " " + p.ToString() + " " + dcc.FileSize.ToString() + "");
             
            dcc.timeoutTimer = new System.Timers.Timer();
            dcc.timeoutTimer.Interval = 1000 * FormMain.Instance.IceChatOptions.DCCChatTimeOut;
            dcc.timeoutTimer.Elapsed += new System.Timers.ElapsedEventHandler(timeoutTimer_Elapsed);
            dcc.timeoutTimer.Start();
            
            AddDCCFile(dcc);

        }

        private void ListenForConnection(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;
            dcc.ListenerSocket.Start();
            
            dcc.keepListening = true;

            dcc.Connection.SendData("PRIVMSG " + dcc.Nick + " :DCC SEND " + dcc.SendFileName + " " + dcc.LocalIP + " " + dcc.Port + " " + dcc.FileSize.ToString() + "");

            while (dcc.keepListening)
            {
                try
                {
                    dcc.Socket = dcc.ListenerSocket.AcceptTcpClient();
                    //System.Diagnostics.Debug.WriteLine("dcc file connected:" + dcc.Socket.Client.RemoteEndPoint.ToString());
                    dcc.Ip = dcc.Socket.Client.RemoteEndPoint.ToString();
                    dcc.ListenerSocket.Stop();
                    dcc.Thread = new Thread(new ParameterizedThreadStart(GetDCCData));
                    dcc.Thread.Name = "DCCListenThread";
                    dcc.Thread.Start(dcc);

                    //dcc.keepListening = false;
                    break;

                }
                catch (Exception)
                {
                    
                    //dcc.keepListening = false;
                }
            }
        }


        internal void StartDCCFile(IRCConnection connection, string nick, string host, string ip, string port, string file, uint fileSize)
        {
            DccFileStruct dcc = new DccFileStruct();
            dcc.FileName = file;
            dcc.FileSize = fileSize;
            dcc.StartFileSize = 0;
            dcc.Socket = new TcpClient();
            dcc.Nick = nick;
            dcc.Host = host;
            dcc.Style = "Download";
            dcc.Connection = connection;
            dcc.Port = port;
            dcc.Ip = ip;

            //create a random number for a tag
            dcc.ListingTag = RandomListingTag();

            try
            {
                string dccPath = FormMain.Instance.IceChatOptions.DCCReceiveFolder;
                //check to make sure the folder exists
                if (!Directory.Exists(dccPath))
                {
                    //add a folder browsing dialog here
                    FolderBrowserDialog fbd = new FolderBrowserDialog();

                    if (fbd.ShowDialog() == DialogResult.OK)
                        dccPath = fbd.SelectedPath;
                    else
                    {
                        //no folder selected, out we go
                        System.Diagnostics.Debug.WriteLine("No folder selected, non-existant dcc receive folder");
                        FormMain.Instance.WindowMessage(connection, "Console", "\x000304DCC File Received Failed : DCC Receive Path does not exists", "", true);
                        return;
                    }
                }

                dcc.Path = dccPath;

                //check if the file exists
                if (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName))
                {
                    //check the local file size and compare to what is being sent
                    FileInfo fi = new FileInfo(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName);
                    if (fi.Length < dcc.FileSize)
                    {
                        try
                        {
                            //appending file:12320768:87618872:True
                            System.Diagnostics.Debug.WriteLine("appending file:" + fi.Length + ":" + dcc.FileSize + ":" + connection.IsFullyConnected);
                            //send DCC RESUME
                            //wait for a DCC ACCEPT from client, and start resume on this port
                            //System.Diagnostics.Debug.WriteLine("PRIVMSG " + nick + " :DCC RESUME \"" + dcc.FileName + "\" " + port + " " + fi.Length.ToString());
                            //connection.SendData("PRIVMSG " + nick + " :\x0001DCC RESUME \"" + dcc.FileName + "\" " + port + " " + fi.Length.ToString() + "\x0001");
                            connection.SendData("PRIVMSG " + nick + " :\x0001DCC RESUME file.ext " + port + " " + fi.Length.ToString() + "\x0001");


                            bool foundMatch = false;
                            
                            if (dccFiles.Count > 0)
                            {
                                foreach (DccFileStruct d in dccFiles)
                                {
                                    //System.Diagnostics.Debug.WriteLine(d.FileName);
                                    if (d.Port == port)
                                    {
                                        System.Diagnostics.Debug.WriteLine("found a match on port " + port);
                                        foundMatch = true;
                                    }
                                }
                            }
                            if (!foundMatch)
                            {
                                dcc.Resume = true;
                                dcc.TotalBytesRead = (uint)fi.Length;
                                dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Append);
                                dcc.StartFileSize = dcc.TotalBytesRead;

                                System.Diagnostics.Debug.WriteLine(dccFiles.Count + ":" + dccFileList.Items.Count);

                                dccFiles.Add(dcc);

                                AddDCCFile(dcc);
                                UpdateDCCFileStatus(dcc, "W-RESUME");

                            }

                            /*
                            
                            bool foundMatch = false;
                            if (dccFiles.Count > 0)
                            {
                                foreach (DccFileStruct d in dccFiles)
                                {
                                    //System.Diagnostics.Debug.WriteLine(d.FileName);
                                    if (d.Port == port)
                                    {
                                        System.Diagnostics.Debug.WriteLine("found a match on port " + port);
                                        foundMatch = true;
                                        d.Resume = true;
                                        d.TotalBytesRead = (uint)fi.Length;
                                        d.StartFileSize = (uint)fi.Length;

                                        d.Socket = null;
                                        d.Socket = new TcpClient();

                                        d.FileStream.Close();
                                        d.FileStream = null;                                        
                                        d.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + file, FileMode.Append);
                                        UpdateDCCFileStatus(d, "W-RESUME");

                                        d.Thread = new Thread(new ParameterizedThreadStart(ConnectDCC));
                                        d.Thread.Name = "StartDCCFileThreadResume";
                                        d.Thread.Start(d);
                                        
                                        break;
                                    }
                                }

                            }

                            if (!foundMatch)
                            {
                                dcc.Resume = true;
                                dcc.TotalBytesRead = (uint)fi.Length;
                                dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Append);
                                dcc.StartFileSize = dcc.TotalBytesRead;

                                System.Diagnostics.Debug.WriteLine(dccFiles.Count + ":" + dccFileList.Items.Count);
                                
                                dccFiles.Add(dcc);

                                AddDCCFile(dcc);
                                UpdateDCCFileStatus(dcc, "W-RESUME");

                                dcc.Thread = new Thread(new ParameterizedThreadStart(ConnectDCC));
                                dcc.Thread.Name = "StartDCCFileThread";
                                dcc.Thread.Start(dcc);
                            }

                            //appending file:62103552:87618872:True
                            //'PRIVMSG Snerf8 :DCC RESUME "epson13792.exe" 5010 62103552
                            //2:1
                            //A first chance exception of type 'System.ArgumentOutOfRangeException' occurred in System.Windows.Forms.dll
                            //InvalidArgument=Value of '7' is not valid for 'index'.
                            //Parameter name: index
                            */


                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                        }
                        return;
                    }
                    else
                    {
                        //file exists, and already complete // set a new filename adding [#] to the end of the fielname
                        int extPos = dcc.FileName.LastIndexOf('.');
                        if (extPos == -1)
                        {
                            int i = 0;
                            do
                            {
                                i++;
                            } while (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName + "(" + i.ToString() + ")"));
                            dcc.FileName += "(" + i.ToString() + ")";
                        }
                        else
                        {
                            string fileName = dcc.FileName.Substring(0, extPos);
                            string ext = dcc.FileName.Substring(extPos + 1);
                            int i = 0;
                            do
                            {
                                i++;
                            } while (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + fileName + "(" + i.ToString() + ")." + ext));
                            dcc.FileName = fileName + "(" + i.ToString() + ")." + ext;
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine("next part of start dcc:" + dcc.FileName);
                AddDCCFile(dcc);
                UpdateDCCFileStatus(dcc, "Waiting");

                dcc.Thread = new Thread(new ParameterizedThreadStart(ConnectDCC));
                dcc.Thread.Name = "StartDCCFileThread";
                dcc.Thread.Start(dcc);
                
                /*
                dcc.Socket.Connect(ep);
                if (dcc.Socket.Connected)
                {
                    dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Create);
                    dcc.Path = dccPath;

                    //start the thread to get the data
                    dcc.Thread = new Thread(new ParameterizedThreadStart(GetDCCData));
                    dcc.Thread.Start(dcc);
                    
                }
                */ 
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("DCC file connection error:" + se.Message);
                FormMain.Instance.WindowMessage(connection, "Console", "\x000304DCC File Receive Error from " + dcc.Nick + " for file " + dcc.FileName + " : " + se.Message, "", true);

                //AddDCCFile(dcc);
                dcc.Errored = true;
                dcc.Finished = false;
                dcc.StartTime = 0;
                UpdateDCCFileProgress(dcc);

            }
        }

        private void ResumeDCC(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;

            IPAddress ipAddr = LongToIPAddress(dcc.Ip);
            IPEndPoint ep = new IPEndPoint(ipAddr, Convert.ToInt32(dcc.Port));
            dcc.IPAddress = ipAddr;
            
            //dcc.Socket = null;
            //dcc.Socket = new TcpClient();

            System.Diagnostics.Debug.WriteLine("start dcc resume thread:" + dcc.Ip);

            try
            {
                dcc.Socket.Connect(ep);
                if (dcc.Socket.Connected)
                {
                    System.Diagnostics.Debug.WriteLine("dcc resume connected");
                    UpdateDCCFileStatus(dcc, "Resuming");
                    GetDCCData(dcc);
                }
                else
                {
                    //dcc did not connect
                    System.Diagnostics.Debug.WriteLine("dcc resume NOT connected");

                }
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("DCC Resume Connect Error " + se.ErrorCode + ":" + se.StackTrace);
                FormMain.Instance.WindowMessage(dcc.Connection, "Console", "\x000304DCC File Resume Error from " + dcc.Nick + " for file " + dcc.FileName + " : " + se.Message, "", true);

                dcc.FileStream.Close();
                dcc.Errored = true;
                dcc.Finished = false;
                dcc.StartTime = 0;
                UpdateDCCFileProgress(dcc);

            }
        }

        private void ConnectDCC(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;

            //IPAddress ipAddr = IPAddress.Parse(dcc.Ip);
            
            IPAddress ipAddr = LongToIPAddress(dcc.Ip);
            IPEndPoint ep = new IPEndPoint(ipAddr, Convert.ToInt32(dcc.Port));
            
            dcc.IPAddress = ipAddr;
            System.Diagnostics.Debug.WriteLine("ConnectDCC attempt connect to :" + dcc.Ip + ":" + ipAddr.ToString() + ":" + ipAddr + ":" + dcc.Port);
            if (dcc.Socket != null)
            {
                System.Diagnostics.Debug.WriteLine("not null:" + dcc.Socket.Available);
            }
            try
            {
                dcc.Socket.Connect(ep);

                if (dcc.Socket.Connected)
                {
                    if (dcc.FileStream == null)
                        dcc.FileStream = new FileStream(dcc.Path + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Create);

                    System.Diagnostics.Debug.WriteLine("connected with dcc:" + dcc.Socket.Connected);
                    UpdateDCCFileStatus(dcc, "Connected");
                    GetDCCData(dcc);
                }
                else
                {
                    //did not connect
                }
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("DCC Connect Error " + se.ErrorCode + ":" + se.StackTrace);
                FormMain.Instance.WindowMessage(dcc.Connection, "Console", "\x000304DCC File Receive Error from " + dcc.Nick + " for file " + dcc.FileName + " : " + se.Message, "", true);
                if (dcc.FileStream != null)
                    dcc.FileStream.Close();
                
                //AddDCCFile(dcc);
                dcc.Errored = true;
                dcc.Finished = false;
                dcc.StartTime = 0;
                UpdateDCCFileProgress(dcc);


            }
        }

         /// <summary>
        /// Remove the specific Download File/Data from the DCC File List
        /// </summary>
        private void RemoveDCCFile(DccFileStruct dcc)
        {
            if (this.InvokeRequired)
            {
                RemoveDCCFileDelegate remove = new RemoveDCCFileDelegate(RemoveDCCFile);
                this.Invoke(remove, new object[] { dcc });
            }
            else
            {
                foreach (ListViewItem l in dccFileList.Items)
                {
                    if (l.Tag.ToString() == dcc.ListingTag.ToString())
                    {
                        //close enough for a match, use this file
                        dccFileList.Items.Remove(l);
                        //remove the item from dccFiles
                        try
                        {
                            if (dcc.FileStream != null)
                            {
                                dcc.FileStream.Close();
                            }

                            if (dcc.Thread != null)
                                if (dcc.Thread.IsAlive)
                                    dcc.Thread.Abort();

                            if (dcc.Socket != null)
                            {
                                if (dcc.Socket.Connected)
                                    dcc.Socket.Close();

                                dcc.Socket = null;
                            }

                            if (dcc.ListenerSocket != null)
                            {
                                dcc.ListenerSocket.Stop();
                                dcc.ListenerSocket = null;
                            }

                            if (dcc.ListenerThread != null)
                            {
                                //dcc.keepListening = false;
                            }

                            if (dcc.timeoutTimer != null)
                            {
                                dcc.timeoutTimer.Stop();
                                dcc.timeoutTimer = null;
                            }

                            dccFiles.Remove(dcc);           

                        }
                        catch (SocketException se)
                        {
                            System.Diagnostics.Debug.WriteLine("remove dcc socket error:" + se.Message);
                        }
                        catch (Exception ee)
                        {
                            System.Diagnostics.Debug.WriteLine("remove dcc error:" + ee.Message);
                        }

             
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Add the specific Download File/Data to the DCC File List
        /// </summary>
        private void AddDCCFile(DccFileStruct dcc)
        {
            if (this.InvokeRequired)
            {
                AddDCCFileDelegate add = new AddDCCFileDelegate(AddDCCFile);
                this.Invoke(add, new object[] { dcc });
            }
            else
            {
                if (dcc.Resume)
                {
                    //try and find a match and continue on with that one
                    foreach (ListViewItem l in dccFileList.Items)
                    {
                        if (l.Text == dcc.FileName && l.SubItems[1].Text == dcc.Nick && l.SubItems[7].Text == dcc.Connection.ServerSetting.ID.ToString())
                        {
                            //close enough for a match, use this file
                            l.Tag = dcc.ListingTag;
                            return;
                        }
                    }
                }
                
                ListViewItem lvi = new ListViewItem(dcc.FileName);
                lvi.SubItems.Add(dcc.Nick);
                lvi.SubItems.Add(dcc.FileSize.ToString());
                lvi.SubItems.Add("");   //speed blank initially
                
                if (dcc.Resume)
                    lvi.SubItems.Add("Resuming");
                else
                    lvi.SubItems.Add("Status");
                
                lvi.SubItems.Add(dcc.Style);
                lvi.SubItems.Add(dcc.Connection.ServerSetting.ID.ToString());
                lvi.Tag = dcc.ListingTag;
                lvi.ToolTipText = dcc.FileName;
                
                dccFiles.Add(dcc);
                dccFileList.Items.Add(lvi);
            }
        }

        private void StartPassiveSocket(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;

            dcc.PassiveSocket.Start();
            bool keepListeningPassive = true;

            string localIP = "";
            if (FormMain.Instance.IceChatOptions.DCCLocalIP != null && FormMain.Instance.IceChatOptions.DCCLocalIP.Length > 0)
            {
                localIP = IPAddressToLong(IPAddress.Parse(FormMain.Instance.IceChatOptions.DCCLocalIP)).ToString();
            }
            else
            {
                if (dcc.Connection.ServerSetting.LocalIP == null)
                {
                    //error. no local IP found
                    FormMain.Instance.WindowMessage(dcc.Connection, "Console", "\x000304DCC ERROR, no Router/Firewall IP Address specified in DCC Settings", "", true);
                    return;
                }
                else
                {
                    localIP = IPAddressToLong(dcc.Connection.ServerSetting.LocalIP).ToString();
                }
            }

            dcc.Connection.SendData("PRIVMSG " + dcc.Nick + " :DCC SEND " + dcc.FileName + " " + localIP + " " + dcc.Port + " " + dcc.FileSize + " " + dcc.passiveID + "");
            System.Diagnostics.Debug.WriteLine("PRIVMSG " + dcc.Nick + " :DCC SEND " + dcc.FileName + " " + localIP + " " + dcc.Port + " " + dcc.FileSize + " " + dcc.passiveID + "");

            while (keepListeningPassive)
            {
                dcc.Socket = dcc.PassiveSocket.AcceptTcpClient();
                dcc.PassiveSocket.Stop();

                System.Diagnostics.Debug.WriteLine("dcc passive socket connected with " + dcc.Nick);
                dcc.Thread = new Thread(new ParameterizedThreadStart(GetDCCData));
                dcc.Thread.Name = "DCCPassiveThread";
                dcc.Thread.Start(dcc);
                
                keepListeningPassive = false;
            }
            
            System.Diagnostics.Debug.WriteLine("startpassive socket completed");

        }
        
        /// <summary>
        /// Get the DCC File Data for the Specified DCC Object
        /// </summary>
        private void GetDCCData(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;

            byte[] buffer;

            dcc.StartTime = DateTime.Now.Ticks;

            while (true)
            {
                try
                {
                    if (dcc.Style == "Download")
                    {
                        //receieve the file data
                        int buffSize = 0;
                        NetworkStream ns = dcc.Socket.GetStream();
                        buffSize = dcc.Socket.ReceiveBufferSize;
                        buffer = new byte[buffSize];

                        int bytesRead;
                        uint rxTotal = 0;
                        
                        if (dcc.Resume)
                            rxTotal = dcc.TotalBytesRead;

                        //no timeout timer with download
                        //dcc.timeoutTimer.Stop();
                        
                        //System.Diagnostics.Debug.WriteLine("can read:" + ns.CanRead + ":" + ns.CanSeek + ":" + ns.CanWrite + ":" + buffSize + ":" + dcc.Socket.ReceiveTimeout);
                        //System.Diagnostics.Debug.WriteLine(ns.DataAvailable);

                        while (true)
                        {
                            bytesRead = ns.Read(buffer, 0, buffSize);
                            if (bytesRead == 0)
                            {
                                //we have a disconnection/error
                                System.Diagnostics.Debug.WriteLine("0 bytes, disconnect");
                                break;
                            }

                            //write it to the file
                            if (dcc.FileStream != null)
                            {
                                dcc.FileStream.Write(buffer, 0, bytesRead);
                                dcc.FileStream.Flush();
                                dcc.TotalBytesRead += (uint)bytesRead;
                                rxTotal += (uint)bytesRead;

                                //update the UI progress bar accordingly
                                UpdateDCCFileProgress(dcc);
                                if (dcc.TotalBytesRead == dcc.FileSize)
                                {
                                    System.Diagnostics.Debug.WriteLine("should be finished");
                                    dcc.Finished = true;
                                    dcc.FileStream.Flush();
                                    dcc.FileStream.Close();
                                    break;
                                }
                            }

                            //System.Diagnostics.Debug.WriteLine("received:" + rxTotal + ":" + dcc.TotalBytesRead + " / " + dcc.FileSize);

                            buffer[0] = (byte)(rxTotal / 16777216);
                            buffer[1] = (byte)((rxTotal - (16777216 * buffer[0])) / 65536);
                            buffer[2] = (byte)((rxTotal - ((16777216 * buffer[0]) + (65536 * buffer[1]))) / 256);
                            buffer[3] = (byte)(rxTotal - ((16777216 * buffer[0]) + (65536 * buffer[1]) + 256 * buffer[2]));
                            
                            //this will error out if the connection is closed
                            ns.Write(buffer, 0, 4);
                        }

                        System.Diagnostics.Debug.WriteLine("no more data avail:" + dcc.Socket.Connected + ":" + ns.DataAvailable);

                        break;
                    }
                    else if (dcc.Style == "Upload")
                    {
                        //send out the file data
                        //receieve the file data
                        long left = dcc.FileStream.Length;
                        NetworkStream ns = dcc.Socket.GetStream();
                        int buffSize = dcc.Socket.SendBufferSize;
                        int bytesToSend ;
                        
                        dcc.timeoutTimer.Stop();
                        
                        //System.Diagnostics.Debug.WriteLine("GetDCC Data Upload:" + ns.DataAvailable + ":" + ns.CanWrite + ":" + ns.CanRead);

                        int buffLimit = FormMain.Instance.IceChatOptions.DCCBufferSize;
                        buffer = new byte[buffLimit];

                        dcc.TotalBytesRead = 0;

                        while(left > 0)
                        {
                            if (left > buffLimit)
                                bytesToSend = buffLimit;
                            else
                                bytesToSend = (int)left;

                            try
                            {
                                dcc.FileStream.Read(buffer, 0, bytesToSend); // read the file into the buffer for sending

                                ns.Write(buffer,0,bytesToSend) ; // blocks until sent, or exception
                                dcc.TotalBytesRead += (uint)bytesToSend;
                                left -= bytesToSend;
                                
                                /*
                                if (ns.Read(buffer, 0, 4) == 4) // handshake
                                {
                                    bytesReceived = (buffer[0] * 16777216) + (buffer[1] * 65536) + (buffer[2] * 256) + buffer[3];
                                    if (bytesReceived != dcc.TotalBytesRead)
                                    {
                                        System.Diagnostics.Debug.WriteLine("Handshake response was " + bytesReceived + " bytes, should have been " + dcc.TotalBytesRead + " " + buffer[0].ToString() + "-" + buffer[1].ToString() + "-" + buffer[2].ToString() + "-" + buffer[3].ToString() + "-");
                                        left = 0;
                                    }
                                }
                                */ 
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Debug.WriteLine(e.Message) ;
                                break ;
                            }

                            UpdateDCCFileProgress(dcc);
                        }

                        // should be completed
                        dcc.Finished = true;
                        break ;


                    }
                }
                catch (SocketException se)
                {
                    System.Diagnostics.Debug.WriteLine("GetDCCData Socket Exception:" + se.Message);
                    dcc.Errored = true;
                    break;
                }
                catch (Exception ex)
                {
                    //we have an error
                    System.Diagnostics.Debug.WriteLine("GetDCCData Error:" + ex.Message + ":" + ex.StackTrace);
                    dcc.Errored = true;
                    break;
                }

            }

            System.Diagnostics.Debug.WriteLine("dcc file disconnected:" + dcc.Style + ":"  + dcc.TotalBytesRead + "/" + dcc.FileSize);
            
            if (dcc.TotalBytesRead != dcc.FileSize)
            {
                dcc.Finished = false;
                dcc.Errored = true;
                dcc.Incomplete = true;
            }

            UpdateDCCFileProgress(dcc);

            try
            {
                if (dcc.FileStream != null)
                {
                    //System.Diagnostics.Debug.WriteLine("closing file stream");
                    //dcc.FileStream.Flush();
                    dcc.FileStream.Close();

                    //check if we have a zero byte file
                    if (File.Exists(dcc.Path + System.IO.Path.DirectorySeparatorChar + dcc.FileName))
                    {
                        //check the local file size and compare to what is being sent
                        FileInfo fi = new FileInfo(dcc.Path + System.IO.Path.DirectorySeparatorChar + dcc.FileName);
                        //System.Diagnostics.Debug.WriteLine("file size:" + fi.Length);
                        if (fi.Length == 0)
                        {
                            //delete the file
                            fi.Delete();
                        }
                    }
                }

                dcc.Socket.Close();
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("GetDCCData Error:" + se.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }            

        }

        private IPAddress LongToIPAddress(string longIP)
        {
            //this could be an IPV6 address parsed...
            try
            {
                byte[] quads = BitConverter.GetBytes(long.Parse(longIP, System.Globalization.CultureInfo.InvariantCulture));
                return IPAddress.Parse(quads[3] + "." + quads[2] + "." + quads[1] + "." + quads[0]);
            }
            catch (Exception)
            {
                return null;
            }

            //byte[] quads = BitConverter.GetBytes(long.Parse(longIP, System.Globalization.CultureInfo.InvariantCulture));
            //return IPAddress.Parse(quads[3] + "." + quads[2] + "." + quads[1] + "." + quads[0]);


        }

        private long IPAddressToLong(IPAddress ip)
        {
            byte[] bytes = ip.GetAddressBytes();
            return (long)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
        }

        private int RandomListingTag()
        {
            Random r = new Random();
            int t = r.Next(10000, 9999999);
            //check if currently in use by looking through dccFiles list


            return t;
        }

        private void UpdateDCCFileStatus(DccFileStruct dcc, string value)
        {
            if (this.InvokeRequired)
            {
                UpdateDCCFileStatusDelegate u = new UpdateDCCFileStatusDelegate(UpdateDCCFileStatus);
                this.Invoke(u, new object[] { dcc, value });
            }
            else
            {
                foreach (ListViewItem lvi in dccFileList.Items)
                {
                    if (lvi.Tag.ToString() == dcc.ListingTag.ToString())
                        lvi.SubItems[5].Text = value;
                }
            }
        }

        /// <summary>
        /// Show the updated file progress in the DCC File List
        /// </summary>
        private void UpdateDCCFileProgress(DccFileStruct dcc)
        {
            if (this.InvokeRequired)
            {
                UpdateDCCFileProgressDelegate u = new UpdateDCCFileProgressDelegate(UpdateDCCFileProgress);
                this.Invoke(u, new object[] { dcc });
            }
            else
            {
                foreach (ListViewItem lvi in dccFileList.Items)
                {
                    if (lvi.Tag.ToString() == dcc.ListingTag.ToString())
                    {
                        lvi.SubItems[2].Text = FormatBytes(dcc.TotalBytesRead) + "/" + FormatBytes(dcc.FileSize);
                        
                        //calculate the bp/sec
                        long elasped = DateTime.Now.Ticks - dcc.StartTime;
                        
                        if (elasped > 0 && (dcc.TotalBytesRead > dcc.StartFileSize))
                        {
                            float b = (elasped / 10000000f);
                            float bps = (dcc.TotalBytesRead - dcc.StartFileSize) / b;

                            lvi.SubItems[3].Text = FormatBytes(bps) +"/s";
                        }
                        else
                            lvi.SubItems[3].Text = "0 Bytes/s";
                        
                        if (dcc.StartTime == 0)
                            lvi.SubItems[4].Text = "";
                        else
                            lvi.SubItems[4].Text = GetDurationTicks(elasped);

                        if (dcc.TotalBytesRead == dcc.FileSize || dcc.Finished)
                        {
                            lvi.SubItems[5].Text = "Completed";
                            
                            //the dcc file download is completed
                            PluginArgs args = new PluginArgs(dcc.Connection);
                            args.fileName = dcc.FileName;
                            args.fileSize = dcc.FileSize;
                            args.dccPort = dcc.Ip;

                            foreach (Plugin p in FormMain.Instance.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                        ipc.plugin.DCCFileComplete(args);
                                }
                            }
                        }
                        else if (dcc.Errored)
                        {
                            if (dcc.Incomplete == true)
                                lvi.SubItems[5].Text = "INCOMPLETE";
                            else
                                lvi.SubItems[5].Text = "ERROR";

                            //the dcc file download has errored
                            PluginArgs args = new PluginArgs(dcc.Connection);
                            args.fileName = dcc.FileName;
                            args.fileSize = dcc.FileSize;
                            args.dccPort = dcc.Ip;

                            foreach (Plugin p in  FormMain.Instance.LoadedPlugins)
                            {
                                IceChatPlugin ipc = p as IceChatPlugin;
                                if (ipc != null)
                                {
                                    if (ipc.plugin.Enabled == true)
                                        ipc.plugin.DCCFileError(args);
                                }
                            }
                        }
                        else if (dcc.Resume)
                            lvi.SubItems[5].Text = "Resuming";
                        else
                            lvi.SubItems[5].Text = "Downloading";

                        return;
                    }
                }
            }

        }

        private string FormatBytes(float bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}",  decimal.Divide((long)bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        private string GetDurationTicks(long ticks)
        {
            TimeSpan t = new TimeSpan(ticks);

            string s = t.Seconds.ToString() + "." + t.Milliseconds.ToString() + " secs";
            if (t.Minutes > 0)
                s = t.Minutes.ToString() + " mins " + s;
            if (t.Hours > 0)
                s = t.Hours.ToString() + " hrs " + s;
            if (t.Days > 0)
                s = t.Days.ToString() + " days " + s;

            return s;
        }

    }
    
    internal class DccFileStruct
    {
        internal Thread Thread = null;
        internal Thread ListenerThread = null;
        internal TcpClient Socket = null;
        internal TcpListener ListenerSocket = null;
        internal FileStream FileStream = null;
        internal IPAddress IPAddress;
        internal IRCConnection Connection = null;

        internal System.Timers.Timer timeoutTimer;

        internal TcpListener PassiveSocket;
        internal Thread PassiveThread;
        internal string passiveID;

        internal string LocalIP;
        internal string Nick;
        internal string Host;
        internal string Ip;
        internal string FileName;
        internal string Path;
        internal string Port;
        internal string SendFileName;

        internal uint FileSize;
        internal uint TotalBytesRead;
        internal uint StartFileSize;
        internal bool Finished;
        internal bool Resume;
        internal long StartTime;
        internal bool Errored;
        internal bool Incomplete;

        internal string Style;        //upload or download
        internal object ListingTag;
        internal bool keepListening;
    }
}
