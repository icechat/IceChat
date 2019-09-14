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
// http://www.vbforums.com/showthread.php?t=614972

// http://www.vbforums.com/showthread.php?p=3685172  -- other multiple one

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IceChat
{
    public class VS2008Renderer
    {
        private static readonly Color ColorHorBG_GrayBlue = Color.FromArgb(255, 233, 236, 250);
        private static readonly Color ColorHorBG_White = Color.FromArgb(255, 244, 247, 252);
        private static readonly Color ColorSubmenuBG = Color.FromArgb(255, 240, 240, 240);
        private static readonly Color ColorImageMarginBlue = Color.FromArgb(255, 212, 216, 230);
        private static readonly Color ColorImageMarginWhite = Color.FromArgb(255, 244, 247, 252);
        private static readonly Color ColorImageMarginLine = Color.FromArgb(255, 160, 160, 180);
        private static readonly Color ColorSelectedBG_Blue = Color.FromArgb(255, 186, 228, 246);
        private static readonly Color ColorSelectedBG_Header_Blue = Color.FromArgb(255, 146, 202, 230);
        private static readonly Color ColorSelectedBG_White = Color.FromArgb(255, 241, 248, 251);
        private static readonly Color ColorSelectedBG_Border = Color.FromArgb(255, 150, 217, 249);
        private static readonly Color ColorSelectedBG_Drop_Blue = Color.FromArgb(255, 139, 195, 225);
        private static readonly Color ColorSelectedBG_Drop_Border = Color.FromArgb(255, 48, 127, 177);
        private static readonly Color ColorMenuBorder = Color.FromArgb(255, 160, 160, 160);
        private static readonly Color ColorCheckBG = Color.FromArgb(255, 206, 237, 250);

        private static readonly Color ColorVerBG_GrayBlue = Color.FromArgb(255, 196, 203, 219);
        private static readonly Color ColorVerBG_White = Color.FromArgb(255, 250, 250, 253);
        private static readonly Color ColorVerBG_Shadow = Color.FromArgb(255, 181, 190, 206);

        private static readonly Color ColorToolstripBtnGrad_Blue = Color.FromArgb(255, 129, 192, 224);
        private static readonly Color ColorToolstripBtnGrad_White = Color.FromArgb(255, 237, 248, 253);
        private static readonly Color ColorToolstripBtn_Border = Color.FromArgb(255, 41, 153, 255);
        private static readonly Color ColorToolstripBtnGrad_Blue_Pressed = Color.FromArgb(255, 124, 177, 204);
        private static readonly Color ColorToolstripBtnGrad_White_Pressed = Color.FromArgb(255, 228, 245, 252);

        private static void DrawRoundedRectangle(Graphics graphics, int xAxis, int yAxis, int width, int height, int diameter, Color color)
        {
            Pen pen = new Pen(color);

            var BaseRect = new RectangleF(xAxis, yAxis, width, height);
            var ArcRect = new RectangleF(BaseRect.Location, new SizeF(diameter, diameter));

            graphics.DrawArc(pen, ArcRect, 180, 90);
            graphics.DrawLine(pen, xAxis + (int)(diameter / 2), yAxis, xAxis + width - (int)(diameter / 2), yAxis);

            ArcRect.X = BaseRect.Right - diameter;
            graphics.DrawArc(pen, ArcRect, 270, 90);
            graphics.DrawLine(pen, xAxis + width, yAxis + (int)(diameter / 2), xAxis + width, yAxis + height - (int)(diameter / 2));

            ArcRect.Y = BaseRect.Bottom - diameter;
            graphics.DrawArc(pen, ArcRect, 0, 90);
            graphics.DrawLine(pen, xAxis + (int)(diameter / 2), yAxis + height, xAxis + width - (int)(diameter / 2), yAxis + height);

            ArcRect.X = BaseRect.Left;
            graphics.DrawArc(pen, ArcRect, 90, 90);
            graphics.DrawLine(pen, xAxis, yAxis + (int)(diameter / 2), xAxis, yAxis + height - (int)(diameter / 2));
        }        

        public class MenuStripRenderer : System.Windows.Forms.ToolStripRenderer
        {
            protected override void InitializeItem(System.Windows.Forms.ToolStripItem item)
            {
                base.InitializeItem(item);
                item.ForeColor = Color.Black;
            }

            protected override void Initialize(System.Windows.Forms.ToolStrip toolStrip)
            {
                base.Initialize(toolStrip);
                toolStrip.ForeColor = Color.Black;
            }

            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                base.OnRenderToolStripBackground(e);

                var b = new LinearGradientBrush(e.AffectedBounds, ColorHorBG_GrayBlue, ColorHorBG_White,
                    LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(b, e.AffectedBounds);
            }

            protected override void OnRenderImageMargin(System.Windows.Forms.ToolStripRenderEventArgs e)
            {
                base.OnRenderImageMargin(e);

                var b = new LinearGradientBrush(e.AffectedBounds, ColorImageMarginWhite, ColorImageMarginBlue,
                    LinearGradientMode.Horizontal);

                var DarkLine = new SolidBrush(ColorImageMarginLine);
                var WhiteLine = new SolidBrush(Color.White);
                var rect = new Rectangle(e.AffectedBounds.Width, 2, 1, e.AffectedBounds.Height);
                var rect2 = new Rectangle(e.AffectedBounds.Width + 1, 2, 1, e.AffectedBounds.Height);

                var SubmenuBGbrush = new SolidBrush(ColorSubmenuBG);
                var rect3 = new Rectangle(0, 0, e.ToolStrip.Width, e.ToolStrip.Height);

                var borderPen = new Pen(ColorMenuBorder);
                var rect4 = new Rectangle(0, 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 2);

                e.Graphics.FillRectangle(SubmenuBGbrush, rect3);
                e.Graphics.FillRectangle(b, e.AffectedBounds);
                e.Graphics.FillRectangle(DarkLine, rect);
                e.Graphics.FillRectangle(WhiteLine, rect2);
                e.Graphics.DrawRectangle(borderPen, rect4);
            }

            protected override void OnRenderItemCheck(System.Windows.Forms.ToolStripItemImageRenderEventArgs e)
            {
                base.OnRenderItemCheck(e);

                if (e.Item.Selected)
                {
                    var rect = new Rectangle(3, 1, 20, 20);
                    var rect2 = new Rectangle(4, 2, 18, 18);
                    var b = new SolidBrush(ColorToolstripBtn_Border);
                    var b2 = new SolidBrush(ColorCheckBG);

                    e.Graphics.FillRectangle(b, rect);
                    e.Graphics.FillRectangle(b2, rect2);
                    e.Graphics.DrawImage(e.Image, new Point(5, 3));
                }
                else
                {
                    var rect = new Rectangle(3, 1, 20, 20);
                    var rect2 = new Rectangle(4, 2, 18, 18);
                    var b = new SolidBrush(ColorSelectedBG_Drop_Border);
                    var b2 = new SolidBrush(ColorCheckBG);

                    e.Graphics.FillRectangle(b, rect);
                    e.Graphics.FillRectangle(b2, rect2);
                    e.Graphics.DrawImage(e.Image, new Point(5, 3));
                }
            }

            protected override void OnRenderSeparator(System.Windows.Forms.ToolStripSeparatorRenderEventArgs e)
            {
                base.OnRenderSeparator(e);

                var DarkLine = new SolidBrush(ColorImageMarginLine);
                var WhiteLine = new SolidBrush(Color.White);
                var rect = new Rectangle(32, 3, e.Item.Width - 32, 1);
                var rect2 = new Rectangle(32, 4, e.Item.Width - 32, 1);
                e.Graphics.FillRectangle(DarkLine, rect);
                e.Graphics.FillRectangle(WhiteLine, rect2);
            }

            protected override void OnRenderArrow(System.Windows.Forms.ToolStripArrowRenderEventArgs e)
            {
                e.ArrowColor = Color.Black;

                base.OnRenderArrow(e);
            }

            protected override void OnRenderMenuItemBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
            {
                base.OnRenderMenuItemBackground(e);

                if (e.Item.Enabled)
                {
                    if (!e.Item.IsOnDropDown && e.Item.Selected)
                    {
                        var rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                        var b = new LinearGradientBrush(rect, ColorSelectedBG_White, ColorSelectedBG_Header_Blue, LinearGradientMode.Vertical);
                        var b2 = new SolidBrush(ColorToolstripBtn_Border);

                        e.Graphics.FillRectangle(b, rect);
                        DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, ColorToolstripBtn_Border);
                        DrawRoundedRectangle(e.Graphics, rect.Left - 2, rect.Top - 2, rect.Width + 2, rect.Height + 3, 4, Color.White);
                        e.Item.ForeColor = Color.Black;
                    }
                    else if (e.Item.IsOnDropDown && e.Item.Selected)
                    {
                        var rect = new Rectangle(4, 2, e.Item.Width - 6, e.Item.Height - 4);
                        var b = new LinearGradientBrush(rect, ColorSelectedBG_White, ColorSelectedBG_Blue, LinearGradientMode.Vertical);
                        var b2 = new SolidBrush(ColorSelectedBG_Border);

                        e.Graphics.FillRectangle(b, rect);
                        DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 6, ColorSelectedBG_Border);
                        e.Item.ForeColor = Color.Black;
                    }

                    if (((ToolStripMenuItem)e.Item).DropDown.Visible && !e.Item.IsOnDropDown)
                    {
                        var rect = new Rectangle(3, 2, e.Item.Width - 6, e.Item.Height - 4);
                        var b = new LinearGradientBrush(rect, Color.White, ColorSelectedBG_Drop_Blue, LinearGradientMode.Vertical);
                        var b2 = new SolidBrush(ColorSelectedBG_Drop_Border);

                        e.Graphics.FillRectangle(b, rect);
                        DrawRoundedRectangle(e.Graphics, rect.Left - 1, rect.Top - 1, rect.Width, rect.Height + 1, 4, ColorSelectedBG_Drop_Border);
                        DrawRoundedRectangle(e.Graphics, rect.Left - 2, rect.Top - 2, rect.Width + 2, rect.Height + 3, 4, Color.White);
                        e.Item.ForeColor = Color.Black;
                    }
                }
            }
        }

        public class ToolStripRenderer : System.Windows.Forms.ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
            {
                base.OnRenderToolStripBackground(e);

                var b = new LinearGradientBrush(e.AffectedBounds, ColorVerBG_White, ColorVerBG_GrayBlue,
                    LinearGradientMode.Vertical);
                var shadow = new SolidBrush(ColorVerBG_Shadow);
                var rect = new Rectangle(0, e.ToolStrip.Height - 2, e.ToolStrip.Width, 1);
                e.Graphics.FillRectangle(b, e.AffectedBounds);
                e.Graphics.FillRectangle(shadow, rect);
            }

            protected override void OnRenderButtonBackground(System.Windows.Forms.ToolStripItemRenderEventArgs e)
            {
                base.OnRenderButtonBackground(e);
                if (e.Item.Selected || ((ToolStripButton)e.Item).Checked)
                {
                    var rectBorder = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                    var rect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                    var b = new LinearGradientBrush(rect, ColorToolstripBtnGrad_White, ColorToolstripBtnGrad_Blue,
                        LinearGradientMode.Vertical);
                    var b2 = new SolidBrush(ColorToolstripBtn_Border);

                    e.Graphics.FillRectangle(b2, rectBorder);
                    e.Graphics.FillRectangle(b, rect);
                }

                if (e.Item.Pressed)
                {
                    var rectBorder = new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1);
                    var rect = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
                    var b = new LinearGradientBrush(rect, ColorToolstripBtnGrad_White_Pressed, ColorToolstripBtnGrad_Blue_Pressed,
                        LinearGradientMode.Vertical);
                    var b2 = new SolidBrush(ColorToolstripBtn_Border);

                    e.Graphics.FillRectangle(b2, rectBorder);
                    e.Graphics.FillRectangle(b, rect);
                }
            }
        }
    } 


}
