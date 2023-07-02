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

using IceChatPlugin;

namespace IceChat
{
    public partial class FormPluginInfo : Form
    {
        IPluginIceChat plugin;
        ToolStripMenuItem menuItem;

        public FormPluginInfo(IPluginIceChat plugin, ToolStripMenuItem menuItem)
        {
            InitializeComponent();

            labelName.Text = plugin.Name;
            labelAuthor.Text = plugin.Author;
            labelVersion.Text = plugin.Version;
            labelFileName.Text = plugin.FileName;

            this.plugin = plugin;
            this.menuItem = menuItem;
            
            if (this.plugin.Enabled)
                this.buttonEnable.Text = "Disable";
            else
                this.buttonEnable.Text = "Enable";
            
            if (plugin.HaveSettingsForm())
                this.buttonSettings.Visible = true;
            else
                this.buttonSettings.Visible = false;

            ApplyLanguage();
        }

        private void ApplyLanguage()
        {

        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonUnload_Click(object sender, EventArgs e)
        {
            FormMain.Instance.UnloadPlugin(menuItem);
            this.Close();
        }

        private void ButtonEnable_Click(object sender, EventArgs e)
        {
            if (this.plugin.Enabled)
                this.buttonEnable.Text = "Enable";
            else
                this.buttonEnable.Text = "Disable";

            this.plugin.Enabled = !this.plugin.Enabled;

            if (this.plugin.Enabled == false)
            {
                this.plugin.Dispose();
            }
            else
            {
                this.plugin.Initialize();
            }

            FormMain.Instance.StatusPlugin(menuItem, this.plugin.Enabled);
            this.Close();
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            plugin.ShowSettingsForm();
        }
    }
}
