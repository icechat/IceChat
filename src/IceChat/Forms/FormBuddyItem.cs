﻿/******************************************************************************\
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
    public partial class FormBuddyItem : Form
    {
        internal delegate void AddBuddyListDelegate(string buddy, string network, TreeNode newItem);
        internal event AddBuddyListDelegate AddBuddyList;
        
        private TreeNode _newItem;

        public FormBuddyItem(string buddy, string network, TreeNode newItem)
        {
            InitializeComponent();

            this.textBuddyNick.Text = buddy;
            this.textBuddyNetwork.Text = network;
            _newItem = newItem;
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (AddBuddyList != null)
                AddBuddyList(textBuddyNick.Text, textBuddyNetwork.Text, _newItem);

            this.Close();
        }
    }
}
