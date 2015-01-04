using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace IceChat
{
    public class IceTabPageDCC : IceTabPage
    {
        private List<DccFileStruct> dccFiles = new List<DccFileStruct>();
        private delegate void AddDCCFileDelegate(DccFileStruct dcc);
        private delegate void UpdateDCCFileProgressDelegate(DccFileStruct dcc);
        private FlickerFreeListView dccFileList;

        public struct DccFileStruct
        {
            public Thread Thread;
            public TcpClient Socket;
            public FileStream FileStream;
            public string Nick;
            public string Host;
            public string FileName;
            public string Path;
            public string FileSize;
            public int TotalBytesRead;
            public IPAddress IPAddress;
            public bool Finished;
            public string Style;
        }

        public IceTabPageDCC(WindowType windowType, string sCaption) : base(windowType, sCaption)
        {
            this.dccFileList = new FlickerFreeListView();
            this.dccFileList.SuspendLayout();
            this.SuspendLayout();

            Panel dccPanel = new Panel();
            dccPanel.BackColor = Color.LightGray;
            dccPanel.Size = new Size(this.Width, 55);
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

            this.dccFileList.Dock = DockStyle.Fill;
            this.dccFileList.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dccFileList.View = View.Details;
            this.dccFileList.MultiSelect = false;
            this.dccFileList.FullRowSelect = true;
            this.dccFileList.HideSelection = false;
            this.dccFileList.DoubleClick += new EventHandler(dccFileList_DoubleClick);

            ColumnHeader fn = new ColumnHeader();
            fn.Text = "File Name";
            fn.Width = 250;
            fn.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.dccFileList.Columns.Add(fn);

            ColumnHeader n = new ColumnHeader();
            n.Text = "Nick";
            n.Width = 100;
            this.dccFileList.Columns.Add(n);

            ColumnHeader fs = new ColumnHeader();
            fs.Text = "File Size";
            fs.Width = 200;
            this.dccFileList.Columns.Add(fs);

            ColumnHeader s = new ColumnHeader();
            s.Text = "Status";
            s.Width = 150;
            s.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            this.dccFileList.Columns.Add(s);

            //store the dcc file style (upload/download)
            ColumnHeader st = new ColumnHeader();
            st.Text = "Style";
            st.Width = 0;
            st.AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.dccFileList.Columns.Add(st);
            this.dccFileList.Columns[4].Width = 0;

            //store the path/folder for the dcc file
            ColumnHeader pa = new ColumnHeader();
            pa.Text = "Path";
            pa.Width = 0;
            pa.AutoResize(ColumnHeaderAutoResizeStyle.None);
            this.dccFileList.Columns.Add(pa);
            this.dccFileList.Columns[5].Width = 0;

            this.Controls.Add(dccFileList);
            this.Controls.Add(dccPanel);
            this.dccFileList.ResumeLayout(false);
            this.ResumeLayout(false);
            
            
            dccFiles = new List<DccFileStruct>();

        }
        
        protected override void Dispose(bool disposing)
        {
            foreach (DccFileStruct dcc in dccFiles)
            {
                if (dcc.Thread.IsAlive)
                    dcc.Thread.Abort();

                if (dcc.Socket.Connected)
                    dcc.Socket.Close();

                if (!dcc.Finished)
                {
                    try
                    {
                        if (dcc.FileStream != null)
                        {
                            dcc.FileStream.Flush();
                            dcc.FileStream.Close();
                        }
                    }
                    catch { }
                }
            }

            dccFiles.Clear();

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
                        if (dcc.Thread.ManagedThreadId.ToString() == lvi.Tag.ToString())
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dccRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //check if it is finished
                    if (lvi.SubItems[3].Text == "Completed" || lvi.SubItems[3].Text == "Cancelled")
                    {
                        //we can remove this item
                        dccFileList.Items.Remove(lvi);
                        return;
                    }
                    else
                    {
                        //it is not finished, do you wish to cancel it?
                        //find the appropriate matching item                        
                        foreach (DccFileStruct dcc in dccFiles)
                        {
                            if (dcc.Thread.ManagedThreadId.ToString() == lvi.Tag.ToString())
                            {
                                DialogResult dialog = MessageBox.Show("The file is still in progress, do you wish to cancel it?", "Cancel File", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.No)
                                    return;

                                dcc.Socket.Close();
                                lvi.SubItems[3].Text = "Cancelled";
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dccOpen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //find the item, and open the folder
                    foreach (DccFileStruct dcc in dccFiles)
                    {
                        if (dcc.Thread.ManagedThreadId.ToString() == lvi.Tag.ToString())
                        {
                            System.Diagnostics.Process.Start(dcc.Path);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cancel the File Transfer for the File Selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dccCancel_Click(object sender, EventArgs e)
        {
            //find which item is being asked to be canceled
            foreach (ListViewItem lvi in dccFileList.Items)
            {
                if (lvi.Selected)
                {
                    //check if it is finished
                    if (lvi.SubItems[3].Text == "Completed" || lvi.SubItems[3].Text == "Cancelled")
                    {
                        //do nothing
                        return;
                    }
                    else
                    {
                        //it is not finished, do you wish to cancel it?
                        //find the appropriate matching item                        
                        foreach (DccFileStruct dcc in dccFiles)
                        {
                            if (dcc.Thread.ManagedThreadId.ToString() == lvi.Tag.ToString())
                            {
                                DialogResult dialog = MessageBox.Show("The file is still in progress, do you wish to cancel it?", "Cancel File", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.No)
                                    return;

                                dcc.Socket.Close();
                                lvi.SubItems[3].Text = "Cancelled";
                                return;
                            }
                        }
                    }
                }
            }
        }

        internal void StartDCCFile(IRCConnection connection, string nick, string host, string ip, string port, string file, string fileSize)
        {
            DccFileStruct dcc = new DccFileStruct();
            dcc.FileName = file;
            dcc.FileSize = fileSize;
            dcc.Socket = new TcpClient();
            dcc.Nick = nick;
            dcc.Host = host;
            dcc.Style = "Download";

            IPAddress ipAddr = LongToIPAddress(ip);
            IPEndPoint ep = new IPEndPoint(ipAddr, Convert.ToInt32(port));

            dcc.IPAddress = ipAddr;

            try
            {
                dcc.Socket.Connect(ep);
                if (dcc.Socket.Connected)
                {
                    string dccPath = FormMain.Instance.IceChatOptions.DCCReceiveFolder;
                    //check if the file exists
                    if (File.Exists(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName))
                    {
                        //file exists // set a new filename adding [#] to the end of the fielname
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

                    dcc.FileStream = new FileStream(dccPath + System.IO.Path.DirectorySeparatorChar + dcc.FileName, FileMode.Create);
                    dcc.Path = dccPath;

                    //start the thread to get the data
                    dcc.Thread = new Thread(new ParameterizedThreadStart(GetDCCData));
                    dcc.Thread.Start(dcc);
                }
            }
            catch (SocketException se)
            {
                System.Diagnostics.Debug.WriteLine("dcc file connection error:" + se.Message);
            }
        }

        /// <summary>
        /// Add the specific Download File/Data to the DCC File List
        /// </summary>
        /// <param name="dcc"></param>
        private void AddDCCFile(DccFileStruct dcc)
        {
            if (this.InvokeRequired)
            {
                AddDCCFileDelegate add = new AddDCCFileDelegate(AddDCCFile);
                this.Invoke(add, new object[] { dcc });
            }
            else
            {
                ListViewItem lvi = new ListViewItem(dcc.FileName);
                lvi.SubItems.Add(dcc.Nick);
                lvi.SubItems.Add(dcc.FileSize);
                lvi.SubItems.Add("Status");
                lvi.SubItems.Add(dcc.Style);
                lvi.Tag = dcc.Thread.ManagedThreadId;
                dccFileList.Items.Add(lvi);
            }
        }

        /// <summary>
        /// Get the DCC File Data for the Specified DCC Object
        /// </summary>
        /// <param name="dcc"></param>
        private void GetDCCData(object dccObject)
        {
            DccFileStruct dcc = (DccFileStruct)dccObject;
            dcc.TotalBytesRead = 0;
            //add it to the Download List
            AddDCCFile(dcc);
            dccFiles.Add(dcc);

            while (true)
            {
                try
                {
                    int buffSize = 0;
                    byte[] buffer = new byte[8192];
                    NetworkStream ns = dcc.Socket.GetStream();
                    buffSize = dcc.Socket.ReceiveBufferSize;
                    int bytesRead = ns.Read(buffer, 0, buffSize);
                    //dcc file data
                    //System.Diagnostics.Debug.WriteLine("dcc file buffer:" + bytesRead);
                    if (bytesRead == 0)
                    {
                        //we have a disconnection/error
                        break;
                    }
                    //write it to the file
                    if (dcc.FileStream != null)
                    {
                        dcc.FileStream.Write(buffer, 0, bytesRead);
                        dcc.FileStream.Flush();
                        dcc.TotalBytesRead += bytesRead;

                        //update the UI progress bar accordingly
                        UpdateDCCFileProgress(dcc);
                        if (dcc.TotalBytesRead.ToString() == dcc.FileSize)
                        {
                            System.Diagnostics.Debug.WriteLine("should be finished");
                            dcc.Finished = true;
                            break;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("null filestream");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //we have an error
                    System.Diagnostics.Debug.WriteLine("GetDCCData Error:" + ex.Message);
                    break;
                }

            }

            System.Diagnostics.Debug.WriteLine("dcc file disconnected:" + dcc.TotalBytesRead + "/" + dcc.FileSize);
            dcc.FileStream.Flush();
            dcc.FileStream.Close();
            dcc.Socket.Close();

        }

        private IPAddress LongToIPAddress(string longIP)
        {
            byte[] quads = BitConverter.GetBytes(long.Parse(longIP, System.Globalization.CultureInfo.InvariantCulture));
            return IPAddress.Parse(quads[3] + "." + quads[2] + "." + quads[1] + "." + quads[0]);
        }

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
                    if (lvi.Tag.ToString() == dcc.Thread.ManagedThreadId.ToString())
                    {
                        lvi.SubItems[2].Text = dcc.TotalBytesRead + "/" + dcc.FileSize;
                        if (dcc.TotalBytesRead.ToString() == dcc.FileSize)
                            lvi.SubItems[3].Text = "Completed";
                        else
                            lvi.SubItems[3].Text = "Downloading";

                        return;
                    }
                }
            }

        }


    }
}
