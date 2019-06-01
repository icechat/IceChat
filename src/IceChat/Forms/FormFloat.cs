/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2018 Paul Vanderzee <snerf@icechat.net>
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

namespace IceChat
{
    public partial class FormFloat : Form
    {
        private Panel _panel;
        private FormMain _parentForm;
        private string _tabName;
        private bool ableDock;

        private const int WM_NCMOUSEMOVE = 0x00A0;

        public FormFloat(ref Panel panel, FormMain parent, string tabName)
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(FormFloat_FormClosing);
            this.Move += new EventHandler(FormFloat_Move);

            this._panel = panel;
            this.Controls.Add(_panel);
            _panel.Dock = DockStyle.Fill;
            _parentForm = parent;
            _tabName = tabName;
            this.Text = tabName;

        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCMOUSEMOVE:
                    if (!StaticMethods.IsRunningOnMono())                    
                        if (ableDock)
                            this.Close();
                    break;
            }
            base.WndProc(ref m);
        }

        private void FormFloat_Move(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(this.Left + ":" + _parentForm.Left + ":" + _parentForm.Right);
            //check if auto-dock is enabled
            if ((this.Left > _parentForm.Left - 10) && (this.Left < _parentForm.Left + 10))
            {
                //oldSize = this.Size;
                this.Height = _parentForm.Height - 50;
                ableDock = true;
            }
            else if ((this.Right > _parentForm.Right - 10) && (this.Right < _parentForm.Right + 10))
            {
                this.Height = _parentForm.Height - 50;
                ableDock = true;
            }
            else
            {
                ableDock = false;
            }
        }

        private void FormFloat_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parentForm.SetPanel(ref this._panel, this.Location, _tabName);
            this.Controls.Remove(_panel);
        }
    }
}
