/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2019 Paul Vanderzee <snerf@icechat.net>
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IceChatPlugin
{
    public partial class FormScriptItem : Form
    {

        public delegate void SaveScriptItemDelegate(ScriptItem scr, int listIndex);
        public event SaveScriptItemDelegate SaveScriptItem;
        
        private ScriptItem scriptItem;
        private int listIndex = 0;

        public FormScriptItem(ScriptItem src, int index)
        {
            InitializeComponent();
            comboScriptEvent.Items.Add("Channel Message");
            comboScriptEvent.Items.Add("Channel Action");
            comboScriptEvent.Items.Add("Private Message");
            comboScriptEvent.Items.Add("Private Action");
            comboScriptEvent.Items.Add("Channel Join");
            comboScriptEvent.Items.Add("Channel Part");
            comboScriptEvent.Items.Add("Channel Invite");
            comboScriptEvent.Items.Add("Channel Kick");
            comboScriptEvent.Items.Add("New Channel");
            comboScriptEvent.Items.Add("New Private");

            comboScriptEvent.Items.Add("Self Channel Message");
            comboScriptEvent.Items.Add("Self Channel Action");

            comboScriptEvent.Items.Add("Self Query Message");
            comboScriptEvent.Items.Add("Self Query Action");

            comboScriptEvent.Items.Add("IceChat Startup");

            comboScriptEvent.Items.Add("Input Text");


            this.scriptItem = src;
            this.listIndex = index;

            comboScriptEvent.SelectedIndexChanged += ComboScriptEvent_SelectedIndexChanged;

            textTextMatch.Text = scriptItem.TextMatch;
            textCommand.Text = scriptItem.Command;
            textChannelMatch.Text = scriptItem.ChannelMatch;
            comboScriptEvent.Text = scriptItem.ScriptEvent;

            textChannelMatch.KeyDown += OnKeyDown;

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.K)
                {
                    //((TextBox)sender).SelectedText = ((char)3).ToString();
                    ((TextBox)sender).SelectedText = @"\%C";
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.B)
                {
                    //((TextBox)sender).SelectedText = ((char)2).ToString();
                    ((TextBox)sender).SelectedText = @"\%B";
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.R)
                {
                    //((TextBox)sender).SelectedText = ((char)2).ToString();
                    ((TextBox)sender).SelectedText = @"\%R";
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.O)
                {
                    //((TextBox)sender).SelectedText = ((char)2).ToString();
                    ((TextBox)sender).SelectedText = @"\%O";
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                if (e.KeyCode == Keys.U)
                {
                    ((TextBox)sender).SelectedText = @"\%U"; //31 (1F) normally //219    //db
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                if (e.KeyCode == Keys.I)
                {
                    //((TextBox)sender).SelectedText = ((char)206).ToString();     //29 1D for normal 29 //ce
                    ((TextBox)sender).SelectedText = @"\%I";     //29 1D for normal 29 //ce
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                //219 - DB
                //206 - CE
            }


        }

        private void ComboScriptEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            // change the labels
            if (comboScriptEvent.Text == "Input Text")
            {
                label2.Visible = false;
                textCommand.Visible = false;
                label5.Visible = false;
                label4.Text = "Text to Replace";
            }
            else
            {
                label2.Visible = true;
                textCommand.Visible = true;
                label5.Visible = true;
                label4.Text = "Channel/Nick Match";
            }

        }


        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            scriptItem.ChannelMatch = textChannelMatch.Text;
            scriptItem.Command = textCommand.Text;
            scriptItem.ScriptEvent = comboScriptEvent.Text;
            scriptItem.TextMatch = textTextMatch.Text;

            if (comboScriptEvent.Text.Length == 0)
            {
                MessageBox.Show("Select a Script event");
                return;
            }

            if (scriptItem.TextMatch.Length == 0)
                scriptItem.TextMatch = ".*";

            
            //update or add new item
            if (SaveScriptItem != null)
                SaveScriptItem(this.scriptItem, listIndex);

            this.Close();     
        }
    }
   
}
