
/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2021 Paul Vanderzee <snerf@icechat.net>
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
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Text;

namespace IceChat
{

    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(ShowAssemblyLoad);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            bool noSplash = false;
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.ToLower() == "-nosplash")
                    {
                        noSplash = true;
                    }
                }
            }

            if (noSplash)
            {
                Application.Run(new FormMain(args, null));
            }
            else
            {
                FormSplash splash = new FormSplash();
                splash.Show();
                Application.Run(new FormMain(args, splash));
            }

        }

        static void ShowAssemblyLoad(object sender, AssemblyLoadEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.LoadedAssembly.GetName().Name + ":" + e.LoadedAssembly.Location);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                System.IO.StreamWriter io = new System.IO.StreamWriter(FormMain.Instance.CurrentFolder + System.IO.Path.DirectorySeparatorChar + "Logs" + System.IO.Path.DirectorySeparatorChar + "exception.log", true);
                io.WriteLine(DateTime.Now.ToString() + "-" + ex.Message + ":" + ex.StackTrace);
                io.WriteLine();
                io.Flush();
                io.Close();
                io.Dispose();
                MessageBox.Show("IceChat 9 Unhandled Exception Error\n\n" + ex.Message + ex.StackTrace, "Fatal Error - Written to Exception.log", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                //Application.Exit();
            }
        }
    }
}
