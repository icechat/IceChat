using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace IceChat
{
    public partial class ColorPicker : UserControl
    {

        public delegate void ColorSelected(string Code, int color);

        public event ColorSelected OnClick;

        private int selectedColor = 100;

        private bool showButtons = false;

        private int buttonSpacing = 20;

        private Pen buttonShadow = new Pen(ColorTranslator.FromHtml("#a0a0a0"));


        public ColorPicker(bool showExtraButtons)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(OnPaint);
            this.MouseUp += new MouseEventHandler(OnMouseUp);

            this.MouseMove += ColorPicker_MouseMove;

            // draw the B , U, I buttons
            this.showButtons = showExtraButtons;

            this.DoubleBuffered = true;


        }

        private void ColorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            // hover effect ??
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {

            if (this.showButtons == true)
            {
                // Add Bold  / Underline / Italic Buttons

                e.Graphics.DrawRectangle(buttonShadow, new Rectangle(181, 1, 15, 15));
                e.Graphics.DrawRectangle(new Pen(Color.Gray), 180, 0, 15, 15);

                e.Graphics.DrawString("B", new Font("Verdana", 10F), new SolidBrush(Color.Black), 182, 0);

                e.Graphics.DrawRectangle(buttonShadow, 201, 1, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), 200, 0, 15, 15);
                e.Graphics.DrawString("U", new Font("Verdana", 10F), new SolidBrush(Color.Black), 202, 0);

                e.Graphics.DrawRectangle(buttonShadow, 221, 1, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), 220, 0, 15, 15);
                e.Graphics.DrawString("I", new Font("Verdana", 10F), new SolidBrush(Color.Black), 224, 0);
            }

            // draw 2 rows of 8 first
            for (int i = 0; i <= 7; i++)
            {

                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 1, 15, 15);
                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 21, 15, 15);

                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i]), (i * buttonSpacing), 0, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 0, 15, 15);

                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 8]), (i * buttonSpacing), 20, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 20, 15, 15);

            }

            // now rows if 12
            for (int i = 0; i <= 11; i++)
            {
                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 41, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 16]), (i * buttonSpacing), 40, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 40, 15, 15);


                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 61, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 28]), (i * buttonSpacing), 60, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 60, 15, 15);

                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 81, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 40]), (i * buttonSpacing), 80, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 80, 15, 15);

                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 101, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 52]), (i * buttonSpacing), 100, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 100, 15, 15);

                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 121, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 64]), (i * buttonSpacing), 120, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 120, 15, 15);

                e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 141, 15, 15);
                e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 76]), (i * buttonSpacing), 140, 15, 15);
                e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 140, 15, 15);

                if (i < 11)
                {
                    e.Graphics.DrawRectangle(buttonShadow, (i * buttonSpacing) + 1, 161, 15, 15);
                    e.Graphics.FillRectangle(new SolidBrush(IrcColor.colors[i + 88]), (i * buttonSpacing), 160, 15, 15);
                    e.Graphics.DrawRectangle(new Pen(Color.Gray), (i * buttonSpacing), 160, 15, 15);
                }

                if (i == selectedColor)
                {
                    //draw a selection rectangle
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 0, 15, 15);
                }
                if (i + 8 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 20, 15, 15);
                }
                if (i + 16 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 40, 15, 15);
                }
                if (i + 28 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 60, 15, 15);
                }
                if (i + 40 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 80, 15, 15);
                }
                if (i + 52 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 100, 15, 15);
                }
                if (i + 64 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 120, 15, 15);
                }
                if (i + 76 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 140, 15, 15);
                }
                if (i + 88 == selectedColor)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Black, 3), (i * buttonSpacing), 160, 15, 15);
                }


            }

        }

        private void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (OnClick != null)
            {

                int xPos = e.X / buttonSpacing;


                if (e.Y < 18)
                {
                    if (xPos < 8)
                    {
                        selectedColor = xPos;
                    }

                    if (showButtons == true)
                    {
                        // 9 == BOLD  
                        // 10 == Underline
                        // 11 == Italic
                        if (xPos == 9)
                        {
                            OnClick('\x02'.ToString(), 0);
                            return;
                        }
                        else if (xPos == 10)
                        {
                            OnClick('\x1F'.ToString(), 0);
                            return;
                        }
                        else if (xPos == 11)
                        {
                            OnClick('\x1D'.ToString(), 0);
                            return;
                        }
                    }


                }
                else if ((e.Y > 19) && e.Y < 38)
                {
                    if (xPos < 8)
                    {
                        selectedColor = xPos + 8;
                    }
                }
                else if ((e.Y > 39) && e.Y < 58)
                {
                    selectedColor = xPos + 16;
                }
                else if ((e.Y > 59) && e.Y < 79)
                {
                    selectedColor = xPos + 28;
                }
                else if ((e.Y > 79) && e.Y < 99)
                {
                    selectedColor = xPos + 40;
                }
                else if ((e.Y > 99) && e.Y < 119)
                {
                    selectedColor = xPos + 52;
                }
                else if ((e.Y > 119) && e.Y < 139)
                {
                    selectedColor = xPos + 64;
                }
                else if ((e.Y > 139) && e.Y < 159)
                {
                    selectedColor = xPos + 76;
                }
                else if ((e.Y > 159) && e.Y < 179)
                {
                    if (xPos < 11)
                    {
                        selectedColor = xPos + 88;
                    }
                }

                this.Invalidate();

                if (selectedColor < 100)
                {
                    OnClick((char)3 + selectedColor.ToString("00"), selectedColor);
                }
            }
        }

        public int SelectedColor
        {
            get { return selectedColor; }
            set
            {
                selectedColor = value;
                this.Invalidate();
            }
        }
    }
}
