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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using System.Runtime.InteropServices;

using MSScriptControl;

namespace IceChatPlugin
{
    public class Plugin : IPluginIceChat
    {

        //all the events get declared here
        public override event OutGoingCommandHandler OnCommand;

        //declare the standard properties
        private string m_Name;
        private string m_Author;
        private string m_Version;

        public override string Name { get { return m_Name; } }
        public override string Version { get { return m_Version; } }
        public override string Author { get { return m_Author; } }

        private ScriptControlClass scriptObject;
        private TextBox editorText;

        private ToolStripMenuItem scriptMenu;
        private ToolStripMenuItem loadScript;
        private ToolStripMenuItem newScript;
        private ToolStripMenuItem unloadScript;
        private ToolStripMenuItem saveScript;
        private ToolStripMenuItem loadedScripts;

        private List<string> scriptFiles = new List<string>();
        private string scriptSettingsFile;
        private IceChatVBScripts icechatVBScripts;
        private SortedList Connections;


        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
                int size, string filePath);


        public Plugin()
        {
            //set your default values here
            m_Name = "VBScript Plugin (Beta)";
            m_Author = "Snerf";
            m_Version = "1.0";
            scriptObject = new ScriptControlClass();

            scriptObject.Language = "VBScript";

        }

        public override void Initialize()
        {
            scriptMenu = new ToolStripMenuItem("VBScript");            
            loadScript = new ToolStripMenuItem("Load Script");
            unloadScript = new ToolStripMenuItem("Unload Script");
            newScript = new ToolStripMenuItem("New Script");
            saveScript = new ToolStripMenuItem("Save Script");
            
            loadedScripts = new ToolStripMenuItem("Scripts");

            loadScript.Click += new EventHandler(loadScript_Click);
            unloadScript.Click += new EventHandler(unloadScript_Click);
            saveScript.Click += new EventHandler(saveScript_Click);
            newScript.Click += new EventHandler(newScript_Click);
            
            scriptSettingsFile = CurrentFolder + System.IO.Path.DirectorySeparatorChar + "IceChatVBScripts.xml";

            LoadScripts();
        }

        public override void MainProgramLoaded(SortedList ServerConnections)
        {
            this.Connections = ServerConnections;
        }

        private void editorText_KeyPress(object sender, KeyPressEventArgs e)
        {
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                if (item.Checked)
                {
                    if (item.Tag.ToString() == "saved")
                    {
                        item.Tag = "unsaved";
                    }
                }
            }

        }

        public override void Dispose()
        {

        }

        private void LoadScripts()
        {
            //load all the scripts into the Script Object
            scriptObject.Reset();

            ScriptClass sc = new ScriptClass();

            sc.ParseCommand += new SendCommandDelegate(Script_ParseCommand);
            sc.ParseCommandCurrent+=new SendCommandCurrentDelegate(Script_ParseCommandCurrent);
            sc.ParseIdentifier += new ParseIdentifierDelegate(Script_ParseIdentifier);
            sc.ParseIdentifierCurrent += new ParseIdentifierCurrentDelegate(Script_ParseIdentifierCurrent);
            sc.GetIni += new GetIniDelegate(Script_GetIni);
            sc.WriteIni += new WriteIniDelegate(Script_WriteIni);
            sc.CheckIsOp += new IsOpDelegate(Script_CheckIsOp);
            sc.CheckIsVoice += new IsVoiceDelegate(Script_CheckIsVoice);

            sc.RunScript += new RunScriptDelegate(Script_RunScript);
            sc.GetDataFolder += new GetDataFolderDelegate(Script_GetDataFolder);

            scriptObject.AddObject("irc", (object)sc, true);
            scriptObject.AllowUI = true;
            scriptObject.State = ScriptControlStates.Connected;

            //read in all the loaded script files
            if(File.Exists(scriptSettingsFile))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(IceChatVBScripts));
                TextReader textReader = new StreamReader(scriptSettingsFile);
                icechatVBScripts = (IceChatVBScripts)deserializer.Deserialize(textReader);
                textReader.Close();
                textReader.Dispose();
            }
            else
                icechatVBScripts = new IceChatVBScripts();
            


            foreach (String file in icechatVBScripts.listScripts)
            {
                if (File.Exists(this.CurrentFolder + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + file))
                {
                    //System.Diagnostics.Debug.WriteLine("loading script file into engine: " + file);
                    
                    StreamReader sr = new StreamReader(this.CurrentFolder + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + file);
                    //add the script file into its own module
                    object o = new object();                    
                    Module m = scriptObject.Modules.Add(file,ref o);
                    m.AddCode(sr.ReadToEnd());
                    
                    sr.Close();                    
                }
            }
        }

        public override void SaveEditorForm()
        {
            //check if the current script has been edited, and if saved?
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                if (item.Checked)
                {
                    if (item.Tag.ToString() == "unsaved")
                    {
                        //save it
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts" + System.IO.Path.DirectorySeparatorChar + item.Name);
                        sw.Write(editorText.Text);
                        sw.Flush();
                        sw.Close();

                        LoadScripts();
                    }
                }
            }
        }

        private void SaveScriptSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(IceChatVBScripts));
            TextWriter textWriter = new StreamWriter(scriptSettingsFile);
            serializer.Serialize(textWriter, icechatVBScripts);
            textWriter.Close();
            textWriter.Dispose();
        }

        public override void LoadEditorForm(TabControl ScriptsTab, MenuStrip mainMenu)
        {
            ScriptsTab.TabPages.Add(AddEditorTab());
            this.editorText.KeyPress += new KeyPressEventHandler(editorText_KeyPress);

            mainMenu.Invoke((MethodInvoker)delegate()
            {                    
                scriptMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    loadScript, unloadScript, newScript, saveScript});

                
                int i = 1;

                foreach (String file in icechatVBScripts.listScripts)
                {
                    if (File.Exists(this.CurrentFolder + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + file))
                    {
                        System.Diagnostics.Debug.WriteLine("loading script file into editor and add to menu: " + file);

                        ToolStripMenuItem s = new ToolStripMenuItem(file);
                        s.Name = file;
                        s.Click += new EventHandler(loadedScript_Click);
                        s.Tag = "saved";
                        
                        //load the first one in the editor
                        if (i == 1)
                        {
                            s.Checked = true;

                            StreamReader sr = new StreamReader(this.CurrentFolder + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + file);
                            editorText.Text = sr.ReadToEnd();
                            sr.Close();
                        }

                        loadedScripts.DropDownItems.AddRange(new ToolStripItem[] {
                            s
                        });
                            
                        i++;
                    }
                }

                mainMenu.Items.Add(scriptMenu);
                mainMenu.Items.Add(loadedScripts);

                //System.Diagnostics.Debug.WriteLine("script items:" + loadedScripts.DropDownItems.Count);

            });

        }

        private void saveScript_Click(object sender, EventArgs e)
        {
            //get the current menu item-- its the current script
            System.Diagnostics.Debug.WriteLine("save the script");
            bool saved = false;
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                if (item.Checked)
                {
                    //save the file
                    item.Tag = "saved";

                    System.IO.StreamWriter sw = new System.IO.StreamWriter(this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts" + System.IO.Path.DirectorySeparatorChar + item.Name);
                    sw.Write(editorText.Text);
                    sw.Flush();
                    sw.Close();
                    saved = true;
                }
            }

            if (!saved)
            {
                //ask for a file name
                SaveFileDialog fd = new SaveFileDialog();
                fd.DefaultExt = ".ice";
                fd.AddExtension = true;
                fd.AutoUpgradeEnabled = true;
                fd.Filter = "Script file (*.ice)|*.ice";
                
                fd.Title = "Save the script file";
                fd.InitialDirectory = this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts";

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter writer = new StreamWriter(fd.OpenFile());
                    writer.Write(editorText.Text);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();

                    //add the menu item
                    ToolStripMenuItem s = new ToolStripMenuItem(System.IO.Path.GetFileName(fd.FileName));
                    s.Name = System.IO.Path.GetFileName(fd.FileName);
                    s.Click += new EventHandler(loadedScript_Click);
                    s.Tag = "saved";

                    s.Checked = true;

                    loadedScripts.DropDownItems.AddRange(new ToolStripItem[] {
                        s
                    });


                    icechatVBScripts.AddScript(s.Name);

                    SaveScriptSettings();

                }

            }

            LoadScripts();

        }


        private void loadedScript_Click(object sender, EventArgs e)
        {
            //show the selected script
            //first uncheck all items
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                item.Checked = false;
            }
            
            //check selected item
            ToolStripMenuItem it = ((ToolStripMenuItem)sender);
            it.Checked = true;

            StreamReader sr = new StreamReader(this.CurrentFolder + Path.DirectorySeparatorChar + "Scripts" + Path.DirectorySeparatorChar + it.Name);
            editorText.Text = sr.ReadToEnd();
            sr.Close();

        }

        private void newScript_Click(object sender, EventArgs e)
        {            
            editorText.Text = "";
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                item.Checked = false;
            }
        }

        private void unloadScript_Click(object sender, EventArgs e)
        {
            //unload the selected script
            foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
            {
                if (item.Checked)
                {
                    System.Diagnostics.Debug.WriteLine("unload:" + item.Text);
                    icechatVBScripts.listScripts.Remove(item.Text);
                    loadedScripts.DropDownItems.Remove(item);

                    this.editorText.Clear();

                    SaveScriptSettings();

                    return;
                }
            }
        }

        private void loadScript_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".ice";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.AddExtension = true;
            fd.AutoUpgradeEnabled = true;
            fd.Filter = "Script file (*.ice)|*.ice";
            fd.Title = "Which script file do you want to open?";
            fd.InitialDirectory = this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //check if it already loaded
                foreach (ToolStripItem item in loadedScripts.DropDownItems)
                {
                    if (item.Name.ToLower() == System.IO.Path.GetFileName(fd.FileName).ToLower())
                    {
                        MessageBox.Show(System.IO.Path.GetFileName(fd.FileName) + " is already loaded");
                        return;
                    }
                }

                foreach (ToolStripMenuItem item in loadedScripts.DropDownItems)
                {
                    item.Checked = false;
                }

                //load the file into the editor
                System.IO.StreamReader sr = new System.IO.StreamReader(fd.FileName);
                editorText.Text = sr.ReadToEnd();
                sr.Close();

                ToolStripMenuItem s = new ToolStripMenuItem(System.IO.Path.GetFileName(fd.FileName));
                s.Checked = true;
                s.Tag = "saved";

                loadedScripts.DropDownItems.AddRange(new ToolStripItem[] {
                    s
                });

                s.Name = System.IO.Path.GetFileName(fd.FileName);
                s.Click += new EventHandler(loadedScript_Click);

                icechatVBScripts.AddScript(s.Name);

                SaveScriptSettings();
            }
        }

        private TabPage AddEditorTab()
        {
            TabPage editorTab = new TabPage();
            editorTab.Text = "VB Scripts";
            editorText = new TextBox();
            editorText.Multiline = true;
            editorText.ScrollBars = ScrollBars.Both;
            editorText.WordWrap = false;
            editorText.Dock = DockStyle.Fill;
            editorTab.Controls.Add(editorText);

            return editorTab;
        }

        public override PluginArgs InputText(PluginArgs args)
        {
            if (args.Command.StartsWith("/?"))
            {
                string command = args.Command.Substring(3);
                scriptObject.ExecuteStatement(command);
                args.Command = "";
            }
            else if (args.Command.StartsWith("/!"))
            {
                string command = args.Command.Substring(3);
                
                if (command.IndexOf(' ') == -1)
                {
                    object[] param = { };
                    RunScript(command, param);
                }
                else
                {
                    //parse the params
                    string ps = command.Split(new char[] {' '}, 2)[1];
                    command = command.Split(new char[] { ' ' }, 2)[0];
                    
                    string[] param = ps.Split(new char[] {'|'});
                    object[] p = new object[param.Length];

                    for (int i = 0; i < param.Length; i++)
                    {
                        System.Diagnostics.Debug.WriteLine(i +":" + param[i]);
                        //param[i] = (object)param[i];
                        //convert to type Integer if possible
                        int result;
                        if (Int32.TryParse(param[i].ToString(), out result))
                        {
                            //param[i] = (Int32)param[i];
                            p[i] = Convert.ToInt32( param[i] );
                        }
                        else
                            p[i] = param[i];

                    }

                    System.Diagnostics.Debug.WriteLine("/! parse:" + command);
                    System.Diagnostics.Debug.WriteLine(p.GetType());
                    System.Diagnostics.Debug.WriteLine(p);
                    
                    RunScript(command, p);

                }
                args.Command = "";
            }
            else
            {
                object[] param = { args.Command };
                RunScript("OUTTEXT", param);
            }
            return args;
        }

        public override PluginArgs ChannelMessage(PluginArgs args)
        {
            object[] param = { args.Extra, args.Channel, args.Nick, args.Host, args.Connection.ServerSetting.ID };
            RunScript("ONTEXT", param);
            return args;
        }

        public override PluginArgs ChannelAction(PluginArgs args)
        {
            object[] param = { args.Extra, args.Channel, args.Nick, args.Host, args.Connection.ServerSetting.ID };
            RunScript("ONACTION", param);
            return args;
        }

        public override PluginArgs QueryMessage(PluginArgs args)
        {
            object[] param = { args.Extra, args.Nick, args.Host, args.Connection.ServerSetting.ID };
            RunScript("ONQUERY", param);
            return args;
        }

        public override PluginArgs QueryAction(PluginArgs args)
        {
            object[] param = { args.Extra, args.Nick, args.Host, args.Connection.ServerSetting.ID };
            RunScript("ONQUERYACTION", param);
            return args;
        }

        public override PluginArgs ChannelJoin(PluginArgs args)
        {
            object[] param = { args.Nick, args.Host, args.Channel, args.Connection.ServerSetting.ID };
            RunScript("ONJOIN", param);
            return args;
        }

        public override PluginArgs ChannelPart(PluginArgs args)
        {
            object[] param = { args.Nick, args.Host, args.Channel, args.Extra, args.Connection.ServerSetting.ID };
            RunScript("ONPART", param);
            return args;
        }

        public override PluginArgs ServerQuit(PluginArgs args)
        {
            object[] param = { args.Nick, args.Host, args.Extra, args.Connection.ServerSetting.ID };
            RunScript("ONQUIT", param);
            return args;
        }

        private void Script_RunScript(string command)
        {
            object[] param = { };
            RunScript(command, param);
        }

        //if you want to add a new method to override, use public override
        private void RunScript(String method, object [] param)
        {
            //System.Diagnostics.Debug.WriteLine("modules:" + scriptObject.Modules.Count + ":" + method);
            
            foreach (Module module in scriptObject.Modules)
            {
                foreach (Procedure proc in module.Procedures)
                {
                    //System.Diagnostics.Debug.WriteLine("proc:" + proc.Name);
                    if (proc.Name.ToLower() == method.ToLower())
                    {
                        //if (param.Length == 5)
                        //    System.Diagnostics.Debug.WriteLine("Run:" + method + ":" + param.Length + ":" + param[0] + ":" + param[1] + ":" + param[2] + ":" + param[3] + ":" + param[4]);
                        //else if (param.Length == 4)
                        //    System.Diagnostics.Debug.WriteLine("Run:" + method + ":" + param.Length + ":" + param[0] + ":" + param[1] + ":" + param[2] + ":" + param[3]);
                        //else
                        System.Diagnostics.Debug.WriteLine("Run:" + method + ":" + param.Length);
                        
                        try
                        {
                            module.Run(method, ref param);
                        }
                        catch (COMException comx)
                        {
                            System.Diagnostics.Debug.WriteLine(comx.Message);
                            //System.Diagnostics.Debug.WriteLine(comx.StackTrace);
                            //System.Diagnostics.Debug.WriteLine(comx.ErrorCode);

                            //return this message back to icechat
                            Script_ParseCommandCurrent("/echo VBScript Error:" + comx.Message);


                        }
                        catch (Exception ex)
                        {
                            //do nothing
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                        /*
                        switch (param.Length)
                        {
                            case 0:
                                break;
                            default:
                                module.Run(method,ref param);
                                break;
                        }
                        */
                    }
                }
            }

        }
        
        //all the different script commands
        
        private string Script_ParseIdentifierCurrent(string setting)
        {
            System.Diagnostics.Debug.WriteLine("GetIdentifier:" + setting);
            string result = ServerTreeCurrentConnection.Parse((string)setting);
            return result;
        }
        
        private string Script_ParseIdentifier(string setting, object ServerNumber)
        {
            System.Diagnostics.Debug.WriteLine("GetIdentifierS:" + setting);
            string result = "";
            if (Connections.ContainsKey(ServerNumber))
            {
                IceChat.IRCConnection c = (IceChat.IRCConnection)Connections[ServerNumber];
                result = c.Parse(setting);
            }
            
            return result;
        }
        
        private void Script_ParseCommandCurrent(string command)
        {
            System.Diagnostics.Debug.WriteLine("parsecc:" + command);
            IceChat.IRCConnection c = null;
            PluginArgs args = new PluginArgs(c);
            args.Extra = "current";
            args.Command = (String)command;
            OnCommand(args);
        }
        

        private void Script_ParseCommand(string command, object ServerNumber)
        {
            System.Diagnostics.Debug.WriteLine("parsec:" + command);
            //how do I get the connection from a server ID
            
            if (Connections.ContainsKey(ServerNumber))
            {
                IceChat.IRCConnection c = (IceChat.IRCConnection)Connections[ServerNumber];
                PluginArgs args = new PluginArgs(c);
                args.Command = command;
                OnCommand(args);
            }

        }

        private bool Script_CheckIsOp(string nick, string channel, object ServerNumber)
        {            
            bool result = false;
            //get the IAL
            //IceChat.InternalAddressList u = (IceChat.InternalAddressList)((IceChat.IRCConnection)ServerNumber).ServerSetting.IAL[nick];
            if (Connections.ContainsKey(ServerNumber))
            {
                IceChat.IRCConnection c = (IceChat.IRCConnection)Connections[ServerNumber];
                string op = c.Parse("$nick(" + channel + "," + nick + ").op");
                if (op == "$true")
                    result = true;
            }
            return result;
        }

        private bool Script_CheckHalfIsOp(string nick, string channel, object ServerNumber)
        {
            bool result = false;
            if (Connections.ContainsKey(ServerNumber))
            {
                IceChat.IRCConnection c = (IceChat.IRCConnection)Connections[ServerNumber];
                string op = c.Parse("$nick(" + channel + "," + nick + ").halfop");
                if (op == "$true")
                    result = true;
            }
            return result;
        }

        private bool Script_CheckIsVoice(string nick, string channel, object ServerNumber)
        {
            bool result = false;
            if (Connections.ContainsKey(ServerNumber))
            {
                IceChat.IRCConnection c = (IceChat.IRCConnection)Connections[ServerNumber];
                string voice = c.Parse("$nick(" + channel + "," + nick + ").voice");
                if (voice == "$true")
                    result = true;
            }
            return result;
        }


        private void Script_WriteIni(string filename, string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts" + System.IO.Path.DirectorySeparatorChar + filename);
        }

        private string Script_GetIni(string filename, string section, string key, string defValue)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, defValue, temp, 255, this.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Scripts" + System.IO.Path.DirectorySeparatorChar + filename);
            return temp.ToString();
        }

        private string Script_GetDataFolder()
        {
            return base.CurrentFolder + Path.DirectorySeparatorChar;
        }

    }

    public class IceChatVBScripts
    {
        [XmlArray("Scripts")]
        [XmlArrayItem("File", typeof(string))]
        public ArrayList listScripts;

        public IceChatVBScripts()
        {
            listScripts = new ArrayList();
        }
        public void AddScript(string file)
        {
            listScripts.Add(file);
        }
    }

}