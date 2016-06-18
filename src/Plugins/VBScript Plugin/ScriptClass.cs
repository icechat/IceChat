/* To Do List
 * MesBox "Prompt", "Caption", Buttons
 * MyValue = InpBox("Prompt","Title")
 * Echo "Data","Channel",ServerNumber
 * - GetIceChatFolder - has a trailing backslash
 * RemoveCodes(data) - strip codes
 * - MyNickName(ServerNumber)
 * - ServerName(ServerNumber)
 * - Network(ServerNumber)
 * - Nicks(Channel,ServerNumber) seperated by a space
 * - AwayStatus(ServerNumber)
 * - CurrentServerNumber()
 * - PlaySound(File) - /play $file
 * ShowFolderPicker(Caption) 
 * 
 * 
 * 
 * DIALOG ITEMS
 * - MyDialog = CreateDialog("Caption",x,y,width,height)
 * - CloseDialog MyDialog
 * ChangeCaption MyDialog, "Caption"
 * - AddButton MyDialog,"Caption",x,y,w,h,id
 * - AddEditBox MyDialog,"text",x,y,w,h,id
 * - AddLabel MyDialog,"Caption",x,y,w,h,id
 * - AddListBox MyDialog,x,y,w,h,id,0,Name
 * - AddComboBox MyDialog,x,y,w,h,id,0,Name
 * AddCheckBox MyDialog,"Caption",x,y,w,h,id,0,Name  //type 6
 * AddRadioButton MyDialog,x,y,w,h,id,0,Name         //type 7
 * MyValue = GetListItem(id, Type)  //Type - 4 = listbox , 5 = combobox
 * MyValue = SetListItem(id,index,Type)
 * MyValue = GetText(id)
 * AddItem id, Type, "text"
 * ClearItems id, Type
 * RemoveItem id, Type, index
 * 
 * 
 * 
 * 
*/

using System;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IceChatPlugin
{
    public delegate void SendCommandDelegate(string command, object ServerNumber);
    public delegate void SendCommandCurrentDelegate(string command);
    public delegate string ParseIdentifierDelegate(string setting, object ServerNumber);
    public delegate string ParseIdentifierCurrentDelegate(string setting);

    public delegate string GetIniDelegate(string filename, string section, string key, string defValue);
    public delegate void WriteIniDelegate(string filename, string section, string key, string value);

    public delegate bool IsOpDelegate(string nick, string channel, object ServerNumber);
    public delegate bool IsVoiceDelegate(string nick, string channel, object ServerNumber);
    public delegate bool IsHalfOpDelegate(string nick, string channel, object ServerNumber);

    public delegate void RunScriptDelegate(string command);
    public delegate string GetDataFolderDelegate();

    [Guid("a1cbc364-005c-44f2-b26b-419f288a53a2")]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [System.Runtime.InteropServices.ComVisible(true)] 
    public class ScriptClass
    {
        public event SendCommandDelegate ParseCommand;
        public event ParseIdentifierDelegate ParseIdentifier;
        public event ParseIdentifierCurrentDelegate ParseIdentifierCurrent;

        public event GetIniDelegate GetIni;
        public event WriteIniDelegate WriteIni;

        public event IsOpDelegate CheckIsOp;
        public event IsVoiceDelegate CheckIsVoice;
        public event IsHalfOpDelegate CheckIsHalfOp;

        public event SendCommandCurrentDelegate ParseCommandCurrent;
        public event RunScriptDelegate RunScript;
        public event GetDataFolderDelegate GetDataFolder;

        private List<ClassDialog> dialogs;
        

        public ScriptClass()
        {
            dialogs = new List<ClassDialog>();            
        }


        public string CreateDialog(string caption, int x, int y, int w, int h)
        {
            //create a new dialog            
            ClassDialog cd = new ClassDialog(caption, x, y, w, h);
            dialogs.Add(cd);

            System.Diagnostics.Debug.WriteLine("Created:" + cd.Handle);

            //the dialog handle / ID is the windows handle
            return cd.Handle.ToString();
        }
        
        /*
        public string CreateDialog(string caption, int x, int y, int w, int h, int color)
        {
            //create a new dialog            
            //color is ignored for the time being
            ClassDialog cd = new ClassDialog(caption, x, y, w, h);
            cd.ButtonClicked += new ButtonClickedDelegate(FormButtonClicked);
            dialogs.Add(cd);

            System.Diagnostics.Debug.WriteLine("Created:" + cd.Handle);

            //the dialog handle / ID is the windows handle
            return cd.Handle.ToString();
        }
        */
        
        private void FormButtonClicked(string controlName)
        {
            //throw new NotImplementedException();
            //call RunScript
            if (RunScript != null)
                RunScript(controlName + "_Clicked");
        }

        public void ShowDialog(string dialogID)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.ShowDialog();
        }

        public void RemoveDialog(string dialogID)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                dialogs.Remove(cd);
        }

        public void CloseDialog(string dialogID)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.Close();
        }
        
        public void AddButton(string dialogID, string caption,int x,int y,int w,int h,int id ,int style,string controlName)
        {
            // id == 0 -- no need for it
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(1, caption, x, y, w, h, "", controlName);
        }
        /*
        public void AddButton(string dialogID,string caption,int x, int y, int w, int h, string id)
        {
            //MyDialog,"Caption",x,y,w,h,id
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(1, caption, x, y, w, h, id, "");
        }
        */

        public void AddEditBox(string dialogID, string caption, int x, int y, int w, int h, string id)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(2, caption, x, y, w, h, id, "");
        }

        public void AddLabel(string dialogID, string caption, int x, int y, int w, int h, string id)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(3, caption, x, y, w, h, id, "");
        }

        public void AddListBox(string dialogID, int x, int y, int w, int h, string id, int style, string controlName)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(4, "", x, y, w, h, id, controlName);
        }

        /*
        public void AddComboBox(string dialogID, int x, int y, int w, int h, string id)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(5, "", x, y, w, h, id, "");
        }
        */

        public void AddComboBox(string dialogID, int x, int y, int w, int h, string id, int style, string controlName)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(5, "", x, y, w, h, id, controlName);
        }

        public void AddCheckBox(string dialogID, string caption, int x, int y, int w, int h, int id, int style, string controlName)
        {
            // id == 0 -- no need for it
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(6, caption, x, y, w, h, "", controlName);
        }
        
        /*
        public void AddCheckBox(string dialogID, string caption, int x, int y, int w, int h, string id)
        {
            ClassDialog cd = dialogs.Find(
                delegate(ClassDialog c)
                {
                    return c.Handle.ToString() == dialogID;
                }
            );

            if (cd != null)
                cd.AddControl(6, caption, x, y, w, h, id, "");
        }
        */
        
        public void AddItem(string id, int iType, string value)
        {
            switch (iType)
            {
                case 4:
                    //listbox
                    ListBox l = (ListBox)FindControlById(id);
                    if (l != null)
                    {
                        l.Items.Add(value);
                    }
                    break;
                case 5:
                    //combobox
                    ComboBox c = (ComboBox)FindControlById(id);
                    if (c != null)
                    {
                        c.Items.Add(value);
                    }

                    break;

            }
        }

        public void SetListItem(string id, int index, int iType)
        {
            //
            switch (iType)
            {
                case 4:
                    //listbox
                    ListBox l = (ListBox)FindControlById(id);
                    if (l != null)
                    {
                        l.SelectedIndex = index;
                    }
                    break;


                case 5:
                    //combobox
                    ComboBox c = (ComboBox)FindControlById(id);
                    if (c != null)
                    {
                        c.SelectedIndex = index;
                    }
                    break;
            }
        }

        public string GetListItem(string id, int iType)
        {
            string value = "";

            switch (iType)
            {
                case 4:
                    //listbox
                    ListBox l = (ListBox)FindControlById(id);
                    if (l != null)
                    {
                        value = l.Text;
                    }
                    break;
                case 5:
                    //combobox
                    ComboBox c = (ComboBox)FindControlById(id);
                    if (c != null)
                    {
                        value = c.Text;
                    }

                    break;

            }

            return value;
        }

        public string GetText(string id)
        {
            //search through all dialogs
            //search through all controls of dialog
            // id = tag
            //string handle = "";
            TextBox t = (TextBox)FindControlById(id);
            if (t != null)
            {
                System.Diagnostics.Debug.WriteLine("found:" + id + ":" + t.Name + ":" + t.Text);
                return t.Text;
            }
            return "";
        }

        private Control FindControlById(string id)
        {
            Control c = null;

            foreach (ClassDialog cd in dialogs)
            {
                c = cd.FindControl(id);
                if (c != null)
                    break;
            }

            return c;
        }

        public void PlaySound(string file)
        {
            if (file.Length == 0) return;
            if (ParseCommandCurrent != null)
                ParseCommandCurrent("/play " + file);
        }

        public void RunShell(string command)
        {
            if (command.Length == 0) return;
            if (ParseCommandCurrent != null)
                ParseCommandCurrent("/run " + command);
        }

        public void SendCommand(string command, object ServerNumber)
        {
            System.Diagnostics.Debug.WriteLine("SendCommandSN:" + ServerNumber + ":" + command);
            if (command.Length == 0) return;            
            if (ParseCommand != null)
                ParseCommand(command, ServerNumber);
        }
        /*
        public void SendCommand(string command)
        {
            System.Diagnostics.Debug.WriteLine("SendCommand:" + command);
            if (command.Length == 0) return;
            if (ParseCommandCurrent != null)
                ParseCommandCurrent(command);
        }
        */
        public string GetIceChatFolder()
        {
            //return the data folder
            string result = "";
            if (GetDataFolder != null)
                result = GetDataFolder();
            return result;            
        }

        public string AwayStatus(object ServerNumber)
        {
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier("$away", ServerNumber);
            return result;
        }
        /*
        public string AwayStatus()
        {
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent("$away");
            return result;
        }
        */
        public string MyNickname(object ServerNumber)
        {
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier("$nick", ServerNumber);
            return result;
        }
        /*
        public string MyNickname()
        {
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent("$nick");
            return result;
        }
        */
        public string ServerName(object ServerNumber)
        {
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier("$server", ServerNumber);
            return result;
        }
        /*
        public string ServerName()
        {
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent("$server");
            return result;
        }
        */
        public string Network(object ServerNumber)
        {
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier("$network", ServerNumber);
            return result;
        }
        /*
        public string Network()
        {
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent("$network");
            return result;
        }
        */
        public string CurrentServerNumber()
        {
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent("$currentserverid");
            return result;
        }

        public string Nicks(string channel, object ServerNumber)
        {
            if (channel.Length == 0) return "";
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier("$chan(" +channel + ").nicks", ServerNumber);
            return result;
        }
        /*
        public string Nicks(string channel)
        {
            if (channel.Length == 0) return "";
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifierCurrent("$chan(" + channel + ").nicks");
            return result;
        }
        */
        public string GetIdentifier(string setting, object ServerNumber)
        {
            if (setting.Length == 0) return "";
            string result = "";
            if (ParseIdentifier != null)
                result = ParseIdentifier(setting, ServerNumber);

            return result;
        }
        /*
        public string GetIdentifier(string setting)
        {
            if (setting.Length == 0) return "";
            string result = "";
            if (ParseIdentifierCurrent != null)
                result = ParseIdentifierCurrent(setting);

            return result;
        }
        */

        public string GetIniFile(string filename, string section, string key, string defValue)
        {
            string result = "";
            if (GetIni != null)
                result = GetIni(filename,section, key, defValue);

            return result;
        }

        public void WriteIniFile(string filename, string section, string key, string value)
        {
            if (WriteIni != null)
                WriteIni(filename, section, key, value);            
        }

        public bool IsOp(string nick, string channel, object ServerNumber)
        {
            bool result = false;
            if (CheckIsOp != null)
                result = CheckIsOp(nick, channel, ServerNumber);            
            
            return result;
        }

        public bool IsVoice(string nick, string channel, object ServerNumber)
        {
            bool result = false;
            if (CheckIsVoice != null)
                result = CheckIsVoice(nick, channel, ServerNumber);

            return result;
        }

        public bool IsHalfOp(string nick, string channel, object ServerNumber)
        {
            bool result = false;
            if (CheckIsHalfOp != null)
                result = CheckIsHalfOp(nick, channel, ServerNumber);

            return result;
        }

    }
    
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    interface IScriptClass
    {
        void ShowDialog(int dialogID);

    }
}
