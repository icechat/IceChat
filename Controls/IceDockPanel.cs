using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace IceChat
{
    public partial class IceDockPanel : Panel
    {
        private Point DragStartPosition = Point.Empty;
        private int dockWidth = 24;

        public IceDockPanel()
        {
            InitializeComponent();
        }

        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle tabRect = _tabControl.GetTabRect(e.Index);
            Brush textBrush;
            
            Graphics g = e.Graphics;

            //Pen p = new Pen(Color.Gray);            
            //g.DrawRectangle(p, tabRect);

            //reset dockWidth to proper tab width
            dockWidth = tabRect.Width + 2;

            Brush br;

            //g.Clear(IrcColor.colors[FormMain.Instance.IceChatColors.TabbarBackColor]);
            
            if (e.State == DrawItemState.Selected)
                textBrush = new SolidBrush(IrcColor.colors[FormMain.Instance.IceChatColors.TabBarCurrent]);
            else
                textBrush = new SolidBrush(IrcColor.colors[FormMain.Instance.IceChatColors.TabBarDefault]);

            if (_tabControl.Alignment == TabAlignment.Left)
            {
                if (e.State == DrawItemState.Selected)
                    br = new LinearGradientBrush(tabRect, IrcColor.colors[FormMain.Instance.IceChatColors.TabBarCurrentBG1], IrcColor.colors[FormMain.Instance.IceChatColors.TabBarCurrentBG2], 360);
                else
                    br = new LinearGradientBrush(tabRect, IrcColor.colors[FormMain.Instance.IceChatColors.TabBarOtherBG1], IrcColor.colors[FormMain.Instance.IceChatColors.TabBarOtherBG2], 360);

                g.FillRectangle(br, tabRect);

                g.TranslateTransform(tabRect.Left, tabRect.Height + tabRect.Top);
                g.RotateTransform(270);
                g.DrawString(_tabControl.TabPages[e.Index].Text, _tabControl.Font, textBrush, 10, 4);
            }
            else
            {

                if (e.State == DrawItemState.Selected)
                    br = new LinearGradientBrush(tabRect, IrcColor.colors[FormMain.Instance.IceChatColors.TabBarCurrentBG1], IrcColor.colors[FormMain.Instance.IceChatColors.TabBarCurrentBG2], 180);
                else
                    br = new LinearGradientBrush(tabRect, IrcColor.colors[FormMain.Instance.IceChatColors.TabBarOtherBG1], IrcColor.colors[FormMain.Instance.IceChatColors.TabBarOtherBG2], 180);

                g.FillRectangle(br, tabRect);

                g.TranslateTransform(tabRect.Left, tabRect.Top);
                g.RotateTransform(90);                
                g.DrawString(_tabControl.TabPages[e.Index].Text, _tabControl.Font, textBrush, 10, (dockWidth  * -1) + 6);
            }
            
            br.Dispose();

            g.ResetTransform();
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            DragStartPosition = new Point(e.X, e.Y);

            UnDockControl();
        }

        public void UnDockControl()
        {
            if (_docked)
            {
                this.Width = _oldDockWidth;

                if (this.Dock == DockStyle.Left)
                    FormMain.Instance.splitterLeft.Visible = true;
                else
                    FormMain.Instance.splitterRight.Visible = true;

                _docked = false;

            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            TabPage hover_Tab = HoverTab();
            if (hover_Tab == null)
                e.Effect = DragDropEffects.None;
            else
            {
                if (e.Data.GetDataPresent(typeof(TabPage)))
                {
                    e.Effect = DragDropEffects.Move;
                    TabPage drag_tab = (TabPage)e.Data.GetData(typeof(TabPage));

                    if (hover_Tab == drag_tab) return;

                    Rectangle TabRect = _tabControl.GetTabRect(_tabControl.TabPages.IndexOf(hover_Tab));
                    TabRect.Inflate(-3, -3);
                    if (TabRect.Contains(_tabControl.PointToClient(new Point(e.X, e.Y))))
                    {
                        SwapTabPages(drag_tab, hover_Tab);
                        _tabControl.SelectedTab = drag_tab;
                    }
                }
            }
        }

        private void SwapTabPages(TabPage tp1, TabPage tp2)
        {
            int Index1 = _tabControl.TabPages.IndexOf(tp1);
            int Index2 = _tabControl.TabPages.IndexOf(tp2);
            
            _tabControl.TabPages[Index1] = tp2;
            _tabControl.TabPages[Index2] = tp1;
        }

        private TabPage HoverTab()
        {
            for (int index = 0; index <= _tabControl.TabCount - 1; index++)
            {
                if (_tabControl.GetTabRect(index).Contains(_tabControl.PointToClient(Cursor.Position)))
                    return _tabControl.TabPages[index];
            }
            return null;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            Rectangle r = new Rectangle(DragStartPosition, Size.Empty);
            r.Inflate(SystemInformation.DragSize);

            TabPage tp = HoverTab();

            if (tp != null)
            {
                if (!r.Contains(e.X, e.Y))
                    _tabControl.DoDragDrop(tp, DragDropEffects.All);
            }
            DragStartPosition = Point.Empty;
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            //
        }

        private void OnMouseHover(object sender, EventArgs e)
        {
            //
        }

        public bool IsDocked
        {
            get { return _docked; }
        }

        public void DockControl()
        {
            if (!_docked)
            {
                _docked = true;
                _oldDockWidth = this.Width;

                //get the proper header height/width

                this.Width = dockWidth;

                if (this.Dock == DockStyle.Left)
                    FormMain.Instance.splitterLeft.Visible = false;
                else
                    FormMain.Instance.splitterRight.Visible = false;
            }
        }

        public TabControl TabControl
        {
            get { return _tabControl; }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            if (_tabControl.SelectedTab.Controls[0].GetType() == typeof(Panel))
            {
                Panel p = (Panel)_tabControl.SelectedTab.Controls[0];
                UnDockPanel(p);
            }
        }

        /// <summary>
        /// Setup the Tabs Font to the setting
        /// </summary>
        public void Initialize()
        {
            _tabControl.Font = new Font(FormMain.Instance.IceChatFonts.FontSettings[6].FontName, FormMain.Instance.IceChatFonts.FontSettings[6].FontSize);
        }

        /// <summary>
        /// Undock the Specified Panel to a Floating Window
        /// </summary>
        /// <param name="p">The panel to remove and add to a Floating Window</param>
        public void UnDockPanel(Panel p)
        {
            if (p.Parent.GetType() == typeof(TabPage))
            {
                //remove the tab from the tabStrip
                TabPage tp = (TabPage)p.Parent;
                _tabControl.TabPages.Remove((TabPage)p.Parent);
                ((TabPage)p.Parent).Controls.Remove(p);

                if (_tabControl.TabPages.Count == 0)
                {
                    //hide the splitter bar along with the panel
                    if (this.Dock == DockStyle.Left)
                        FormMain.Instance.splitterLeft.Visible = false;
                    else
                        FormMain.Instance.splitterRight.Visible = false;

                    this.Visible = false;
                }

                FormFloat formFloat = new FormFloat(ref p, FormMain.Instance, tp.Text);
                formFloat.Show();
                formFloat.Left = Cursor.Position.X - (formFloat.Width / 2);
                formFloat.Top = Cursor.Position.Y;
            }
        }
    }
}
