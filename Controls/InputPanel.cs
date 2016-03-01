/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2016 Paul Vanderzee <snerf@icechat.net>
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
    public partial class InputPanel : UserControl
    {
        internal delegate void OnCommandDelegate(object sender, string data);
        internal event OnCommandDelegate OnCommand;

        //which is the current connection
        private IRCConnection currentConnection;
        private bool _showEmoticonPicker;
        private bool _showColorPicker;
        private bool _showSendButton;
        private bool _showBasicCommands;
        private bool _backtoNormal;

        private List<String> _buffer;
        private int _currentHistoryItem = -1;
        private int _maxBufferSize = 100;


        public InputPanel()
        {
            _buffer = new List<String>();

            InitializeComponent();

            this.buttonEmoticonPicker.Image = StaticMethods.LoadResourceImage("Smile.png");
            this.buttonColorPicker.Image = StaticMethods.LoadResourceImage("color.png");

            textInput.OnCommand += new IceInputBox.SendCommand(textInput_OnCommand);
            textBoxWide.OnCommand += new IceInputBox.SendCommand(textBoxWide_OnCommand);

        }

        internal int CurrentHistoryItem
        {
            get { return _currentHistoryItem; }
            set { _currentHistoryItem = value; }
        }

        internal int MaxBufferSize
        {
            get { return _maxBufferSize; }
        }

        internal List<String> Buffer
        {
            get { return _buffer; }
        }

        internal void ApplyLanguage()
        {
            if (FormMain.Instance != null)
            {
                IceChatLanguage iceChatLanguage = FormMain.Instance.IceChatLanguage;
                buttonSend.Text = iceChatLanguage.buttonSend;
            }
        }

        internal IRCConnection CurrentConnection
        {
            get
            {
                return currentConnection;
            }
            set
            {
                currentConnection = value;
            }
        }

        internal bool ShowWideTextPanel
        {
            get { return this.panelWideText.Visible; }
            set
            {
                if (value)
                    this.Height = 96;
                else
                    this.Height = 26;

                this.panelWideText.Visible = value;
                this.textInput.Visible = !value;
                this.buttonHelp.Visible = !value;

                if (this._showEmoticonPicker == false)
                    buttonEmoticonPicker.Visible = false;
                else
                    this.buttonEmoticonPicker.Visible = !value;

                if (this._showColorPicker == false)
                    buttonColorPicker.Visible = false;
                else
                    this.buttonColorPicker.Visible = !value;

                if (this._showBasicCommands == false)
                    buttonHelp.Visible = false;
                else
                    buttonHelp.Visible = !value;


                if (this._showSendButton == false)
                {
                    //this.buttonSend.Visible = false;
                    this.panelSend.Visible = false;
                }
                else
                {
                    this.panelSend.Visible = true;
                    //hide the reset button if not wide
                    buttonReset.Visible = value;

                }

                //if (value == false)
                //{
                //    buttonSend.Visible = this._showSendButton;
                //}
                //else
                //    buttonSend.Visible = true;
                //panelSend.Visible = buttonSend.Visible;

                if (value)
                {
                    this.textBoxWide.Focus();
                    if (this.textInput.Text.Length > 0)
                        textBoxWide.Text = textInput.Text;

                    //panelSend.Visible = true;
                    FormMain.Instance.IceChatOptions.ShowMultilineEditbox = true;
                    FormMain.Instance.multilineEditboxToolStripMenuItem.Checked = true;
                }
                else
                {
                    this.textInput.Focus();
                    if (this.textBoxWide.Text.Length > 0)
                        textInput.Text = textBoxWide.Text;

                    FormMain.Instance.IceChatOptions.ShowMultilineEditbox = false;
                    FormMain.Instance.multilineEditboxToolStripMenuItem.Checked = false;
                }
            }
        }

        internal bool ShowEmoticonPicker
        {
            get { return this._showEmoticonPicker; }
            set { this.buttonEmoticonPicker.Visible = value; this._showEmoticonPicker = value; }
        }

        internal bool ShowColorPicker
        {
            get { return this._showColorPicker; }
            set { this.buttonColorPicker.Visible = value; this._showColorPicker = value; }
        }

        internal bool ShowBasicCommands
        {
            get { return this._showBasicCommands; }
            set { this.buttonHelp.Visible = value; this._showBasicCommands = value; }
        }

        internal bool ShowSendButton
        {
            get { return this._showSendButton; }
            set
            {
                this._showSendButton = value;
                this.panelSend.Visible = value;
            }
        }

        internal Font InputBoxFont
        {
            get { return textInput.Font; }
            set
            {
                textInput.Font = value;
                textBoxWide.Font = value;
            }
        }

        internal void SetInputBoxColors()
        {
            this.textInput.BackColor = IrcColor.colors[FormMain.Instance.IceChatColors.InputboxBackColor];
            this.textInput.ForeColor = IrcColor.colors[FormMain.Instance.IceChatColors.InputboxForeColor];

            this.textBoxWide.BackColor = IrcColor.colors[FormMain.Instance.IceChatColors.InputboxBackColor];
            this.textBoxWide.ForeColor = IrcColor.colors[FormMain.Instance.IceChatColors.InputboxForeColor];

            this.BackColor = IrcColor.colors[FormMain.Instance.IceChatColors.InputboxBackColor];
        }

        internal void AppendText(string data)
        {
            if (textInput.Visible)
            {
                textInput.Text = textInput.Text + data;
                textInput.SelectionStart = textInput.Text.Length;
            }
            else
            {
                textBoxWide.Text = textBoxWide.Text + data;
                textBoxWide.SelectionStart = textBoxWide.Text.Length;
            }
        }

        private void textBoxWide_OnCommand(object sender, string data)
        {
            if (OnCommand != null)
            {
                string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        OnCommand(this, line);
                    }
                }
            }
        }

        private void textInput_OnCommand(object sender, string data)
        {
            if (OnCommand != null)
            {
                string[] lines = data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 1)
                {
                    //just 1 line, add to end of text box
                    //if (lines[0].Length < 350)
                    OnCommand(this, lines[0]);
                    //else
                    //{
                    //}
                }
                else
                {
                    if (lines.Length > 2)
                    {
                        //we are pasting 3 lines or more, lets ask
                        //make a form popup here
                        if (!this.ShowWideTextPanel)
                            _backtoNormal = true;

                        this.ShowWideTextPanel = true;

                        foreach (string line in lines)
                        {
                            if (line.Length > 0)
                            {
                                textBoxWide.AppendText(line + Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        foreach (string line in lines)
                        {
                            if (line.Length > 0)
                            {
                                if (line.Length < 350)
                                {
                                    textInput.addToBuffer(line);
                                    OnCommand(this, line);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        internal void ClearTextBox()
        {
            if (textInput.Visible)
            {
                textInput.Clear();
            }
            else
            {
                textBoxWide.Clear();
            }
        }

        internal void FocusTextBox()
        {
            if (textInput.Visible)
            {
                if (!textInput.Focused)
                    textInput.Focus();
            }
            else
            {
                if (!textBoxWide.Focused)
                    textBoxWide.Focus();
            }

        }

        internal void SendButtonClick()
        {
            //System.Diagnostics.Debug.WriteLine("p clicked");
            //this.buttonSend.PerformClick();
            if (textInput.Visible)
            {
                textInput.OnEnterKey();
                FocusTextBox();
            }
            else
            {
                //send what is in textWide
                string[] lines = textBoxWide.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        //split line at 350 chars
                        if (line.Length < 350)
                        {
                            textBoxWide.addToBuffer(line);

                            OnCommand(this, line);
                        }
                        else
                        {
                            //split it up


                        }
                    }
                }
                textBoxWide.Clear();

                if (_backtoNormal)
                {
                    this.ShowWideTextPanel = false;
                    _backtoNormal = false;
                }
                else
                    FocusTextBox();

            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendButtonClick();
        }

        private void buttonEmoticonPicker_Click(object sender, EventArgs e)
        {
            //show the emoticon picker form
            FormEmoticons fe = new FormEmoticons();
            fe.Top = (FormMain.Instance.Top + FormMain.Instance.Height) - 220;
            fe.Left = FormMain.Instance.Left + 10;
            fe.ShowDialog(this);

            FormMain.Instance.FocusInputBox();
        }

        private void buttonColorPicker_Click(object sender, EventArgs e)
        {
            FormColorPicker fc = new FormColorPicker();
            fc.Top = (FormMain.Instance.Top + FormMain.Instance.Height) - 220;
            fc.Left = FormMain.Instance.Left + 10;
            fc.ShowDialog(this);

            FormMain.Instance.FocusInputBox();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            //find next search
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            //find previous search
        }

        private void buttonHelp_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            contextHelpMenu.Show(buttonHelp, e.Location);
        }

        private void toolStripHelpMenuOnClick(object sender, System.EventArgs e)
        {
            OnCommand(this, "//addtext " + ((System.Windows.Forms.ToolStripMenuItem)sender).Tag.ToString());
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            //go back to a normal panel
            ShowWideTextPanel = false;
        }

    }
}
