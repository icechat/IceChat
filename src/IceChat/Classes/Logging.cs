using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace IceChat
{
    public class Logging : IDisposable
    {
        //a logging class to handle window logging        
        private System.IO.FileStream logFile;
        private int lastDayWritten;
        private ConsoleTab _consoleTab = null;
        private IceTabPage _tabPage = null;
        private string logFileLocation;
        private string fileExtension;
        
        internal string LogFileLocation
        {
            get { return logFileLocation; }
        }

        internal string LogFileName
        {
            get { return Path.GetFileName(logFile.Name); }
        }

        public Logging(ConsoleTab consoleTab)
        {
            this._consoleTab = consoleTab;

            CreateConsoleLog();
        }

        private void CreateConsoleLog()
        {
            string logFolder = FormMain.Instance.LogsFolder;

            fileExtension = null;

            if (FormMain.Instance.IceChatOptions.LogFormat == "Plain Text" || FormMain.Instance.IceChatOptions.LogFormat == "Colored Text")
                fileExtension = ".log";
            else if (FormMain.Instance.IceChatOptions.LogFormat == "HTML")
                fileExtension = ".html";

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            logFolder += Path.DirectorySeparatorChar + _consoleTab.Connection.ServerSetting.ServerName;

            //replace any invalid characters in the log folder
            foreach (char c in Path.GetInvalidPathChars())
                logFolder = logFolder.Replace(c, '_');

            string date = "-" + System.DateTime.Now.ToString("yyyy-MM-dd");

            try
            {
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);
            }
            catch (NotSupportedException)
            {
                //invalid directory name, try display name instead
                logFolder = FormMain.Instance.LogsFolder + Path.DirectorySeparatorChar + _consoleTab.Connection.ServerSetting.DisplayName;
                foreach (char c in Path.GetInvalidPathChars())
                    logFolder = logFolder.Replace(c, '_');
                
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

            }

            if (FormMain.Instance.IceChatOptions.SeperateLogs)
            {
                logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + "Console" + date + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            }
            else
            {
                logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + "Console" + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            }
            
            lastDayWritten = DateTime.Now.Day;

            logFileLocation = logFolder;
            
            if (logFile.Length == 0)
                AddFileHeader();

        }

        public Logging(IceTabPage tabPage)
        {
            this._tabPage = tabPage;

            CreateStandardLog();            
        }

        private void CreateStandardLog()
        {
            string logFolder = FormMain.Instance.LogsFolder;

            string date = "-" + System.DateTime.Now.ToString("yyyy-MM-dd");

            string fileName = _tabPage.TabCaption;
            fileExtension = null;

            if (FormMain.Instance.IceChatOptions.LogFormat == "Plain Text" || FormMain.Instance.IceChatOptions.LogFormat == "Colored Text")
                fileExtension = ".log";
            else if (FormMain.Instance.IceChatOptions.LogFormat == "HTML")
                fileExtension = ".html";

            //replace any invalid characters in the file name
            foreach (char c in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(c, '_');
            
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            //set the log folder to the server name
            if (_tabPage.WindowStyle != IceTabPage.WindowType.Debug && _tabPage.WindowStyle != IceTabPage.WindowType.Window)
                logFolder += Path.DirectorySeparatorChar + _tabPage.Connection.ServerSetting.ServerName;

            //replace any invalid characters in the log folder
            foreach (char c in Path.GetInvalidPathChars())
                logFolder = logFolder.Replace(c, '_');


            if (_tabPage.WindowStyle == IceTabPage.WindowType.Channel)
            {
                logFolder += Path.DirectorySeparatorChar + "Channel";
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                if (FormMain.Instance.IceChatOptions.SeperateLogs)
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + date + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                else
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            }
            else if (_tabPage.WindowStyle == IceTabPage.WindowType.Query)
            {
                logFolder += Path.DirectorySeparatorChar + "Query";
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                if (FormMain.Instance.IceChatOptions.SeperateLogs)
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + date + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                else
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            }
            else if (_tabPage.WindowStyle == IceTabPage.WindowType.Window)
            {

                logFolder += Path.DirectorySeparatorChar + "Window";
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                if (FormMain.Instance.IceChatOptions.SeperateLogs)
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + date + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                else
                    logFile = new FileStream(logFolder + Path.DirectorySeparatorChar + fileName + fileExtension, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);

            }
            else if (_tabPage.WindowStyle == IceTabPage.WindowType.Debug)
            {
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                logFile = new FileStream(FormMain.Instance.LogsFolder + Path.DirectorySeparatorChar + "debug.log", System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
            }
            
            logFileLocation = logFolder;

            lastDayWritten = DateTime.Now.Day;

            if (logFile.Length == 0)
                AddFileHeader();

        }

        internal void WriteLogFile(string message)
        {
            if (logFile != null && logFile.CanWrite == true)
            {
                //check if we need to make a new log file for a new day
                if (DateTime.Now.Day != lastDayWritten && FormMain.Instance.IceChatOptions.SeperateLogs)
                {
                    logFile.Close();
                    logFile.Dispose();

                    if (_consoleTab != null)
                        CreateConsoleLog();
                    else if (_tabPage != null)
                        CreateStandardLog();
                }
                //Check if log format has changed
                else if (((FormMain.Instance.IceChatOptions.LogFormat == "Plain Text" || FormMain.Instance.IceChatOptions.LogFormat == "Colored Text") && fileExtension == ".html") || (FormMain.Instance.IceChatOptions.LogFormat == "HTML" && fileExtension == ".log"))
                {
                    logFile.Close();
                    logFile.Dispose();

                    if (_consoleTab != null)
                        CreateConsoleLog();
                    else if (_tabPage != null)
                        CreateStandardLog();
                }

                if (FormMain.Instance.IceChatOptions.LogFormat == "Plain Text")
                {
                    System.Diagnostics.Debug.WriteLine("1:"+message);
                    message = StripCodes(message);
                    System.Diagnostics.Debug.WriteLine("2:"+message);
                    message += "\r\n";
                    logFile.Write(System.Text.Encoding.UTF8.GetBytes(message), 0, System.Text.Encoding.UTF8.GetBytes(message).Length);
                }
                else if (FormMain.Instance.IceChatOptions.LogFormat == "Colored Text")
                {
                    message += "\r\n";
                    logFile.Write(System.Text.Encoding.UTF8.GetBytes(message), 0, System.Text.Encoding.UTF8.GetBytes(message).Length);
                }
                else if (FormMain.Instance.IceChatOptions.LogFormat == "HTML")
                {
                    message = ReplaceColorCodes(message);
                    logFile.Write(System.Text.Encoding.UTF8.GetBytes(message), 0, System.Text.Encoding.UTF8.GetBytes(message).Length);
                }
                
                logFile.Flush();
            }
        }

        internal void AddFileHeader()
        {
            if (_tabPage != null)
            {
                if (FormMain.Instance.IceChatOptions.LogFormat == "HTML")
                {
                    string message = "<meta charset='utf-8'>\r\n";
                    logFile.Write(System.Text.Encoding.UTF8.GetBytes(message), 0, message.Length);
                }

                WriteLogFile("Session Start: " + DateTime.Now.ToString("ddd MMM dd hh:mm:ss yyyy"));
                if (_tabPage.WindowStyle == IceTabPage.WindowType.Channel)
                    WriteLogFile("Session Ident: " + _tabPage.TabCaption);
            }
            else if (_consoleTab != null)
            {
                if (FormMain.Instance.IceChatOptions.LogFormat == "HTML")
                {
                    string message = "<meta charset='utf-8'>\r\n";
                    logFile.Write(System.Text.Encoding.UTF8.GetBytes(message), 0, System.Text.Encoding.UTF8.GetBytes(message).Length);
                }
                WriteLogFile("Session Start: " + DateTime.Now.ToString("ddd MMM dd hh:mm:ss yyyy"));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)        
        {
            //if (_tabPage != null)
            //    System.Diagnostics.Debug.WriteLine("Dispose Logging Class TabPage:" + _tabPage.TabCaption);
            //else if (_consoleTab != null)
            //    System.Diagnostics.Debug.WriteLine("Dispose Logging Class Console:" + _consoleTab.Connection.ServerSetting.ServerName);
            
            if (logFile != null)
            {
                try
                {
                    if (logFile.CanWrite)
                    {
                        logFile.Flush();
                        logFile.Close();
                    }
                    logFile.Dispose();
                }
                catch
                {
                }
            }
        }
        
        //replace color codes with HTML codes
        private string ReplaceColorCodes(string line)
        {
            //replace certain characters with HTML friendly chars

            line = Regex.Replace(line, "&", "&amp;");
            line = Regex.Replace(line, "'", "&apos;");
            line = Regex.Replace(line, "\"", "&quot;");
            line = Regex.Replace(line, @"<", "&lt;");
            line = Regex.Replace(line, @">", "&gt;");

            //replace url chars
            line = line.Replace('\xFF0B'.ToString(), "");
            line = line.Replace('\xFF0C'.ToString(), "");
            
            //replace line wrapping chars
            line = line.Replace('\xFF0D'.ToString(), "");
            line = line.Replace('\xFF0F'.ToString(), "");

            line = "<div style='float:left; display:block; clear:both;'>" + line;

            bool inColor = false;

            //parse color codes
            Match m = StaticMethods.ParseColorCodes.Match(line);
            while (m.Success)
            {
                if (Regex.Match(m.Value, StaticMethods.ParseBackColor).Success)
                {
                    //find the comma
                    int color = 0;
                    color = Convert.ToInt32(m.Value.Substring(1, m.Value.IndexOf(",") - 1));
                    if (color == 0) color = 1;
                    if (color > 71) color = 1;

                    string c = System.Drawing.ColorTranslator.ToHtml(IrcColor.colors[color]);
                    if (inColor)
                        line = line.Replace(m.Value, "</span><span style='color:" + c + ";'>");
                    else
                        line = line.Replace(m.Value, "<span style='color:" + c + ";'>");
                    
                    inColor = true;
                }
                else if (Regex.Match(m.Value, StaticMethods.ParseForeColor).Success)
                {
                    //System.Diagnostics.Debug.WriteLine("fc:" + m.Value);
                    int color = 0;
                    if (m.Value.Length > 1) //just a color code
                        color = Convert.ToInt32(m.Value.Substring(1, m.Value.Length - 1));

                    if (color == 0) color = 1;
                    if (color > 71) color = 1;

                    string c = System.Drawing.ColorTranslator.ToHtml(IrcColor.colors[color]);
                    if (inColor)
                        line = line.Replace(m.Value, "</span><span style='color:" + c + ";'>");
                    else
                        line = line.Replace(m.Value, "<span style='color:" + c + ";'>");
                    
                    inColor = true;
                }
                else if (Regex.Match(m.Value, StaticMethods.ParseColorChar).Success)
                {
                    //System.Diagnostics.Debug.WriteLine("c:" + m.Value + ":" + inColor);
                    if (inColor)
                    {
                        line = line.Replace(m.Value, "</span>");
                        inColor = false;
                    }
                }
                /*
                int color = 0;
                if (m.Value.Length > 1) //just a color code
                    color = Convert.ToInt32(m.Value.Substring(1,m.Value.Length -1));                                

                if (color == 0) color = 1;
                string c = System.Drawing.ColorTranslator.ToHtml(IrcColor.colors[color]);
                if (inColor)                    
                    line = line.Replace(m.Value, "</span><span style='color:" + c + ";'>");
                else
                    line = line.Replace(m.Value, "<span style='color:" + c + ";'>");
                */

                m = m.NextMatch();
                //inColor = true;

            }

            //parse bold codes
            Regex parseBold = new Regex(((char)2).ToString());
            Match b = parseBold.Match(line);
            bool isBold = false;
            while (b.Success)
            {
                //problem here, replaces all the bold chars, not 1 by 1
                if (isBold)
                    line = line.Replace(b.Value, "</b>");
                else
                    line = line.Replace(b.Value, "<b>");
                isBold = !isBold;
                
                b = b.NextMatch();
            }

            //parse underline codes
            Regex parseUnderline = new Regex(((char)3).ToString());
            Match u = parseUnderline.Match(line);
            bool isUnderline = false;
            while (u.Success)
            {
                if (isUnderline)
                    line = line.Replace(u.Value, "</u>");
                else
                    line = line.Replace(u.Value, "<u>");
                isUnderline = !isUnderline;

                u = u.NextMatch();
            }
            if (isBold)
                line = line + "</b>";
            if (isUnderline)
                line = line + "</u>";

            if (inColor)
                line = line + "</span>";

            return line + "</div><br />\r\n";
        }
        
        //strip all the colors codes out
        private string StripCodes(string line)
        {
            return StaticMethods.ParseAllCodes.Replace(line, "");            
        }

    }
}
