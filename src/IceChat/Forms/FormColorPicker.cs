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
    public partial class FormColorPicker : Form
    {
        private ColorButtonArray colorPicker;

        public FormColorPicker()
        {
            InitializeComponent();

            colorPicker = new ColorButtonArray(panelColorPicker);
            colorPicker.OnClick += new ColorButtonArray.ColorSelected(colorPicker_OnClick);

            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            //buttonClose.Text = FormMain.Instance.IceChatLanguage.buttonClose;
        }

        private void colorPicker_OnClick(int colorSelected)
        {
            FormMain.Instance.InputPanel.AppendText((char)3 + colorSelected.ToString("00"));
            this.Close();
        }
        
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
