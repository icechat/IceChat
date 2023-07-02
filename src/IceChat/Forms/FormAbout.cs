/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2023 Paul Vanderzee <snerf@icechat.net>
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

namespace IceChat
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
            this.Text = FormMain.ProgramID + " " + FormMain.VersionID + " | " + FormMain.BuildDate;
            labelVersion.Text = FormMain.ProgramID + " " + FormMain.VersionID + " | " + FormMain.BuildDate;
            this.Load += new EventHandler(OnLoad);
            this.Click += new EventHandler(OnClick);
            ApplyLanguage();
        }

        private void OnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.Owner != null)
                this.Location = new Point(this.Owner.Location.X + this.Owner.Width / 2 - this.Width / 2,
                    this.Owner.Location.Y + this.Owner.Height / 2 - this.Height / 2);
        }

        private void ApplyLanguage()
        {
            //labelAbout.Text = FormMain.Instance.IceChatLanguage.aboutText;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.icechat.net");
            }
            catch {}
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/icechat/IceChat");
            }
            catch { }
        }
    }
}
