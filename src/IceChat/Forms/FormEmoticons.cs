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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IceChat
{
    public partial class FormEmoticons : Form
    {
        public FormEmoticons()
        {
            InitializeComponent();

            Bitmap emots = new Bitmap(this.Width, this.Height);
            
            Graphics g = Graphics.FromImage(emots);            
            
            int x = 0;
            int y = 0;

            if (FormMain.Instance.IceChatEmoticons != null)
            {
                foreach (EmoticonItem emot in FormMain.Instance.IceChatEmoticons.listEmoticons)
                {
                    try
                    {
                        Bitmap bm = new Bitmap(FormMain.Instance.EmoticonsFolder + System.IO.Path.DirectorySeparatorChar + emot.EmoticonImage);
                        int i = imageListEmoticons.Images.Add(bm, Color.Fuchsia);

                        //System.Diagnostics.Debug.WriteLine(x + ":" + y + ":" + this.Width + ":" + emot.Trigger);

                        g.DrawImage(imageListEmoticons.Images[i], x, y);

                        x = x + 21;
                        if (x >= (this.Width - 30))
                        {
                            x = 0;
                            y = y + 25;
                        }
                    }
                    catch { }
                }
            }

            pictureEmoticons.Image = emots;
            
            g.Dispose();

            pictureEmoticons.MouseDown += new MouseEventHandler(PictureEmoticons_MouseDown);
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {

        }

        private void PictureEmoticons_MouseDown(object sender, MouseEventArgs e)
        {
            //figure out the index
            int x = e.X / 21;
            int y = e.Y / 25;

            int emot = (y * 22) + x;
            if (emot < FormMain.Instance.IceChatEmoticons.listEmoticons.Count)
            {
                FormMain.Instance.InputPanel.AppendText(FormMain.Instance.IceChatEmoticons.listEmoticons[emot].Trigger);
                this.Close();
            }
        }
    }
}
