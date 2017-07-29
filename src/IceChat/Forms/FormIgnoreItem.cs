/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2017 Paul Vanderzee <snerf@icechat.net>
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
    public partial class FormIgnoreItem : Form
    {

        internal delegate void UpdateIgnoreListDelegate(ListViewItem lvi);
        internal event UpdateIgnoreListDelegate UpdateIgnoreList;        

        private int ignoreType;
        private ListViewItem _lvi;

        public FormIgnoreItem(ListViewItem lvi)
        {
            InitializeComponent();

            textNick.Text = lvi.Text;
            this._lvi = lvi;

            ignoreType = Convert.ToInt32(lvi.SubItems[1].Text);

            // set the checkboxes
            if (ignoreType ==  0)
            {
                checkChannel.Checked = true;
                checkPrivate.Checked = true;
                checkNotice.Checked = true;
                checkCTCP.Checked = true;
                checkDCC.Checked = true;
                checkInvite.Checked = true;
            }
            else
            {
                int val = ignoreType;
                
                if (val > 31)
                {
                    checkInvite.Checked = true;
                    val -= 32;
                }

                if (val > 15)
                {
                    checkDCC.Checked = true;
                    val -= 16;
                }
                if (val > 7)
                {
                    checkCTCP.Checked = true;
                    val -= 8;
                }

                if (val > 3)
                {
                    checkNotice.Checked = true;
                    val -= 4;
                }

                if (val > 1)
                {
                    checkPrivate.Checked = true;
                    val -= 2;
                }

                if (val > 0)
                {
                    checkChannel.Checked = true;
                    val -= 1;
                }
            }

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            // save the settings, and return the list item
            _lvi.Text = textNick.Text;

            if (checkChannel.Checked == true && checkPrivate.Checked == true && checkNotice.Checked == true && checkCTCP.Checked == true && checkInvite.Checked == true && checkDCC.Checked == true)
            {
                // all are checked

                _lvi.SubItems[1].Text = "0";

                _lvi.SubItems[2].Text = "x";
                _lvi.SubItems[3].Text = "x";
                _lvi.SubItems[4].Text = "x";
                _lvi.SubItems[5].Text = "x";
                _lvi.SubItems[6].Text = "x";
                _lvi.SubItems[7].Text = "x";

            }
            else
            {
                // get the value
                int val = 0;

                if (this.checkChannel.Checked)
                {
                    val += 1;
                    _lvi.SubItems[2].Text = "x";
                }
                else
                {
                    _lvi.SubItems[2].Text = "";
                }

                if (this.checkPrivate.Checked)
                {
                    val += 2;
                    _lvi.SubItems[3].Text = "x";
                }
                else
                {
                    _lvi.SubItems[3].Text = "";
                }
                
                if (this.checkNotice.Checked)
                {
                    val += 4;
                    _lvi.SubItems[4].Text = "x";
                }
                else
                {
                    _lvi.SubItems[4].Text = "";                
                }
                
                if (this.checkCTCP.Checked)
                {
                    val += 8;
                    _lvi.SubItems[5].Text = "x";
                }
                else
                {
                    _lvi.SubItems[5].Text = "";

                }
                if (this.checkDCC.Checked)
                {
                    _lvi.SubItems[6].Text = "x";
                    val += 16;
                }
                else
                {
                    _lvi.SubItems[6].Text = "";
                }

                if (this.checkInvite.Checked)
                {
                    val += 32;
                    _lvi.SubItems[7].Text = "x";
                }
                else
                {
                    _lvi.SubItems[7].Text = "";

                }

                _lvi.SubItems[1].Text = val.ToString();


            }

            if (UpdateIgnoreList != null)
                UpdateIgnoreList(_lvi);

            this.Close();

        }
    }
}

