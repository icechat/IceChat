/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2015 Paul Vanderzee <snerf@icechat.net>
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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;


namespace IceChatUpdater
{
    public partial class FormUpdater : Form
    {
        private string currentFolder;
        private System.Net.WebClient webClient;
        private string currentFile;
        
        private Stack<Uri> localFiles = new Stack<Uri>();
        private Stack<Uri> moveFiles = new Stack<Uri>();

        private bool useNet45 = false;

        public FormUpdater(string[] args)
        {
            InitializeComponent();

            this.TopMost = true;

            if (args.Length > 0)
            {

                if (args.Length == 2)
                    useNet45 = true;

                foreach (string arg in args)
                    currentFolder = arg;
            }
            else
                currentFolder = Application.StartupPath;
           


            labelFolder.Text = currentFolder;
            labelCurrentFile.Text = "";
            label3.Text = Application.StartupPath;

            CheckForUpdate();
            
            //check for Framework version (no not check in Mono)
            if (Type.GetType("Mono.Runtime") == null)
                Get45or451FromRegistry();

            //string remoteUri = "http://download-codeplex.sec.s-msft.com/Download/SourceControlFileDownload.ashx?ProjectName=icechat&changeSetId=" + revision;

        }

        private void CheckForUpdate()
        {

            //get the current version of IceChat 2009 in the Same Folder

            // are we checking for .net 45 or not
            string update9XML = "update9.xml";

            if (useNet45 == true)
            {
                update9XML = "update9-45.xml";
                label1.Text = "Files to Update: using .NET 4.5";
            }

            System.Diagnostics.FileVersionInfo fv;
            double currentVersion;
            try
            {
                fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(currentFolder + Path.DirectorySeparatorChar + "IceChat2009.exe");
                System.Diagnostics.Debug.WriteLine(fv.FileVersion);
                labelCurrent.Text = "Current Version: " + fv.FileVersion;
                currentVersion = Convert.ToDouble(fv.FileVersion.Replace(".", String.Empty));
            }
            catch(Exception)
            {
                currentVersion = 000000000;
                labelCurrent.Text = "Current Version: 0.0.0000.0000";
                System.Diagnostics.Debug.WriteLine("IceChat EXE not found");
            }
            
            //delete the current update.xml file if it exists
            if (File.Exists(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + update9XML))
                File.Delete(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + update9XML);

            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadFile("http://www.icechat.net/" + update9XML, Application.StartupPath + System.IO.Path.DirectorySeparatorChar + update9XML);
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + update9XML);
            
            System.Xml.XmlNodeList version = xmlDoc.GetElementsByTagName("version");
            System.Xml.XmlNodeList versiontext = xmlDoc.GetElementsByTagName("versiontext");

            labelLatest.Text = "Latest Version: " + versiontext[0].InnerText;

            if (Convert.ToDouble(version[0].InnerText) > currentVersion)
            {
                XmlNodeList files = xmlDoc.GetElementsByTagName("file");
                foreach (XmlNode node in files)
                {
                    DownloadItem dl = new DownloadItem();
                    dl.FileName = node.InnerText;
                    dl.ShortName = Path.GetFileName(node.InnerText);
                    dl.FileType = "core";
                    listFiles.Items.Add(dl);                 
                }

                buttonDownload.Visible = true;
                labelUpdate.Visible = true;
            }
            else
                labelNoUpdate.Visible = true;

            //return;

            //check plugins that need to be updated as well
            //check the plugins folder
            if (Directory.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Plugins"))
            {
                string[] plugins = Directory.GetFiles(currentFolder + System.IO.Path.DirectorySeparatorChar + "Plugins", "*.dll");
                foreach (string fileName in plugins)
                {
                                        
                    //System.Diagnostics.Debug.WriteLine(fileName);
                    //look for a match to plugins online
                    FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(fileName);

                    XmlNodeList plgs = xmlDoc.GetElementsByTagName("plugin");
                    
                    foreach (XmlNode plg in plgs)
                    {
                        //System.Diagnostics.Debug.WriteLine(plg["file"].InnerText);
                        //System.Diagnostics.Debug.WriteLine(plg["version"].InnerText);
                        if (Path.GetFileName(plg["pluginfile"].InnerText).ToLower() == fvi.InternalName.ToLower())
                        {
                            //check versions
                            //System.Diagnostics.Debug.WriteLine(fvi.FileVersion + ":" + plg["pluginversion"].InnerText + ":" + plg["pluginfile"].InnerText);
                            //System.Diagnostics.Debug.WriteLine(Convert.ToSingle(fvi.FileVersion));
                            if (Convert.ToSingle(fvi.FileVersion.Replace(".", "")) < Convert.ToSingle(plg["pluginversion"].InnerText.Replace(".", "")))
                            {
                                System.Diagnostics.Debug.WriteLine("Upgrade needed for " + fvi.InternalName);

                                DownloadItem dl = new DownloadItem();
                                dl.FileName = plg["pluginfile"].InnerText;
                                dl.ShortName = Path.GetFileName(plg["pluginfile"].InnerText);
                                dl.FileType = "plugin";
                                listFiles.Items.Add(dl);

                                buttonDownload.Visible = true;
                                labelUpdate.Visible = true;

                            }
                        }
                    }
                    
                }
            }

        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            //download the files in the File List box
            //check to make sure icechat 9 is not running
            Process[] pArry = Process.GetProcesses();

            foreach (Process p in pArry)
            {
                string s = p.ProcessName;
                s = s.ToLower();
                //System.Diagnostics.Debug.WriteLine(s);

                if (s.Equals("icechat2009"))
                {
                    System.Diagnostics.Debug.WriteLine(Path.GetDirectoryName(p.Modules[0].FileName).ToLower() + ":" + currentFolder.ToLower());
                    if (Path.GetDirectoryName(p.Modules[0].FileName).ToLower() == currentFolder.ToLower())
                    {
                        MessageBox.Show("Please Close IceChat 9 before updating.");
                        return;
                    }
               }
            }
            //MessageBox.Show("no match");
            //return;

            webClient = new System.Net.WebClient();
            this.Cursor = Cursors.WaitCursor;
            
            this.labelSize.Visible = true;
            this.progressBar.Visible = true;

            this.buttonDownload.Enabled = false;
            //System.Collections.ArrayList localFiles = new System.Collections.ArrayList();
            
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);            
            webClient.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            
            foreach (DownloadItem item in listFiles.Items)
            {
                //if (item.FileType == "core")
                {
                    string f = System.IO.Path.GetFileName(item.FileName);

                    //delete any previous downloaded versions of the file
                    try
                    {
                        if (File.Exists(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + f))
                            File.Delete(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + f);
                    }
                    catch (Exception) { }
                    
                    System.Diagnostics.Debug.WriteLine("core:" + item.FileName);

                    Uri uri = new Uri(item.FileName);                    

                    localFiles.Push(uri);
                    moveFiles.Push(uri);
                }   
            }
            
            Uri u = localFiles.Pop();
            string localFile = Path.GetFileName(u.ToString());
            currentFile = u.ToString();
            labelCurrentFile.Text = currentFile;

            webClient.DownloadFileAsync(u, Application.StartupPath + System.IO.Path.DirectorySeparatorChar + localFile);
           
        }

        private void webClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(e.ProgressPercentage + ":" + e.BytesReceived + ":" + e.TotalBytesToReceive);
            this.progressBar.Value = e.ProgressPercentage;
            labelSize.Text = e.BytesReceived + "/" + e.TotalBytesToReceive + " (" + e.ProgressPercentage + "%)";            
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("download done:" + e.UserState + ":" + currentFile);

            //go to the next file in the list
            if (localFiles.Count > 0)
            {
                Uri u = localFiles.Pop();

                string localFile = Path.GetFileName(u.ToString());
                currentFile = u.ToString();
                labelCurrentFile.Text = currentFile;
                
                System.Diagnostics.Debug.WriteLine("downloaded:" + localFile);

                webClient.DownloadFileAsync(u, Application.StartupPath + System.IO.Path.DirectorySeparatorChar + localFile);
            }
            else
            {

                this.Cursor = Cursors.Default;
                MessageBox.Show("Completed Download");

                
                buttonDownload.Enabled = false;
                
                foreach (Uri f in moveFiles)
                {
                    //check where to place the file

                    if (f.ToString().Contains("www.icechat.net/beta/"))
                    {
                        System.Diagnostics.Debug.WriteLine("Update core:" + f.ToString());
                        try
                        {
                            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString())))
                                File.Delete(currentFolder + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()));

                        }
                        catch (FileNotFoundException)
                        {
                            System.Diagnostics.Debug.WriteLine("Error 1");
                        }

                        System.Threading.Thread.Sleep(500);

                        File.Copy(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()), currentFolder + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()), true);

                    }
                    else
                    {
                        //its a plugin file
                        System.Diagnostics.Debug.WriteLine("Update plugin:" + f.ToString());
                        try
                        {
                            if (File.Exists(currentFolder + System.IO.Path.DirectorySeparatorChar + "Plugins" + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString())))
                                File.Delete(currentFolder + System.IO.Path.DirectorySeparatorChar + "Plugins" + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()));

                        }
                        catch (FileNotFoundException)
                        {
                            System.Diagnostics.Debug.WriteLine("Error 2");
                        }
                        System.Threading.Thread.Sleep(500);

                        File.Copy(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()), currentFolder + System.IO.Path.DirectorySeparatorChar + "Plugins" + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()), true);

                    }
                    //delete the files out of the update folder
                    File.Delete(Application.StartupPath + System.IO.Path.DirectorySeparatorChar + Path.GetFileName(f.ToString()));

                }


                MessageBox.Show("Files updated, you are welcome to restart IceChat");

            }

        }


        private void Get45or451FromRegistry()
        {
            using (RegistryKey rkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\", false))
            {
                int releaseKey = Convert.ToInt32(rkey.GetValue("Release"));
                labelFramework.Text = CheckFor45DotVersion(releaseKey);

                if (releaseKey >= 378389)
                {
                    //4.5 installed, no need to download

                }
                else
                {
                    //need to install .net 4.5!!
                    // http://www.microsoft.com/en-ca/download/details.aspx?id=42643

                }
            }
        }

        private string CheckFor45DotVersion(int releaseKey)
        {
            // https://msdn.microsoft.com/en-us/library/hh925568%28v=vs.110%29.aspx

            if (releaseKey >= 393273)
            {
                return ".Net Framework 4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return ".Net Framework 4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return ".Net Framework 4.5.1";
            }
            if ((releaseKey >= 378389))
            {
                return ".Net Framework 4.5";
            }
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 
            return "No .Net Framework 4.5 or later version detected";
        }

    }
    public class DownloadItem
    {

        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ShortName { get; set; }

        public override string ToString()
        {
            return ShortName;
        }

    }    

}
