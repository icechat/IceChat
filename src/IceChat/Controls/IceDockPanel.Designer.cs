using System;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace IceChat
{
    partial class IceDockPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tabControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // _tabControl
            // 
            this._tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Margin = new System.Windows.Forms.Padding(0);
            this._tabControl.Multiline = true;
            this._tabControl.Name = "_tabControl";
            this._tabControl.Padding = new System.Drawing.Point(15, 5);
            this._tabControl.SelectedIndex = 0;
            this._tabControl.Size = new System.Drawing.Size(200, 100);
            this._tabControl.TabIndex = 0;
            this._tabControl.TabStop = false;
            this._tabControl.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this._tabControl.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this._tabControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this._tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawItem);
            this._tabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            // 
            // IceDockPanel
            // 
            this.Controls.Add(this._tabControl);
            this.ResumeLayout(false);

        }


        private TabControl _tabControl;
        private bool _docked;
        private int _oldDockWidth;


    }
}
