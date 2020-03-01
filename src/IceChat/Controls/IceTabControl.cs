/******************************************************************************\
 * IceChat 9 Internet Relay Chat Client
 *
 * Copyright (C) 2020 Paul Vanderzee <snerf@icechat.net>
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
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using IceChatPlugin;

namespace IceChat
{
    public partial class IceTabControl : UserControl 
    {

        private int _selectedIndex = -1;

        private Point _dragStartPosition = Point.Empty;

        private IceTabPage _currentTab = null;
        private IceTabPage _previousTab = null;
        private MdiLayout _layout = MdiLayout.Cascade;
        
        public bool windowedMode = false;

        public IceTabControl()
        {
            InitializeComponent();
            this.ControlRemoved+=new ControlEventHandler(OnControlRemoved);
        }

        internal MdiLayout MdiLayout
        {
            get { return _layout; }
            set { this._layout = value; }
        }

        internal void AddTabPage(IceTabPage page)
        {
            page.Dock = DockStyle.Fill;
            page.Location = new Point(0, 1);

            if (!this.Controls.Contains(page))
                this.Controls.Add(page);

            _previousTab = _currentTab;
            _currentTab = page;

        }

        internal void BringFront(IceTabPage page)
        {
            try
            {
                if (page.Parent != null)
                {
                    if (page.Parent.GetType() == typeof(FormWindow))                    
                    {
                        ((FormWindow)page.Parent).SelectTabActivate = false;
                        page.Parent.BringToFront();
                    }
                    else
                    {
                        // it is breaking here in Mono!                        
                        //page.BringToFront();
                        FormMain.Instance.TabMain.Controls.SetChildIndex(page, 0);
                        
                    }
                }
                else
                {
                    page.BringToFront();
                }

                _selectedIndex = page.TabIndex;
                
                FormMain.Instance.ServerTree.Invalidate();
                
                _previousTab = _currentTab;
                
                if (_previousTab.WindowStyle != IceTabPage.WindowType.Console)
                    if (_previousTab.TextWindow != null)
                        _previousTab.TextWindow.ResetUnreadMarker();
                
                _currentTab = page;
            }
            catch (Exception ex)
            {
                FormMain.Instance.WriteErrorFile(FormMain.Instance.InputPanel.CurrentConnection, "TabMain BringtoFront2 Error:", ex);
            }
        }
        
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            //
        }

        private void OnControlRemoved(object sender, ControlEventArgs e) 
        {
            if (e.Control is IceTabPage) 
            {
                if (((IceTabPage)e.Control).DockedForm == false)
                    ((IceTabPage)e.Control).Dispose();
               
            }
            else if (e.Control is IceTabPageDCCFile)
            {
                if (((IceTabPageDCCFile)e.Control).DockedForm == false)
                    ((IceTabPageDCCFile)e.Control).Dispose();
            }

            foreach (IceTabPage page in this.Controls)
            {
                if (this.Controls.GetChildIndex(page) == 0)
                {
                    //this is the current 
                    System.Diagnostics.Debug.WriteLine("Current Index - 0:" + page.TabCaption);
                    _currentTab = page;
                }

                if (this.Controls.GetChildIndex(page) == 1)
                    _previousTab = page;
            }

            //redraw
            FormMain.Instance.ServerTree.SelectTab(_currentTab, false);
            FormMain.Instance.ChannelBar.SelectTab(_currentTab);
            FormMain.Instance.ServerTree.Invalidate();

        }        
    }
}
