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
            components = new System.ComponentModel.Container();

            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;
            _tabControl.Multiline = true;
            _tabControl.TabStop = false;
            //_tabControl.AllowDrop = true;
            //_tabControl.SizeMode = TabSizeMode.Fixed;
            _tabControl.Padding = new Point(15, 5);
            _tabControl.SizeMode = TabSizeMode.Normal;
           
            _tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            _tabControl.Alignment = TabAlignment.Left;
            _tabControl.DrawItem += new DrawItemEventHandler(OnDrawItem);
            _tabControl.DoubleClick += new EventHandler(OnDoubleClick);
            _tabControl.MouseDown += new MouseEventHandler(OnMouseDown);
            _tabControl.MouseMove += new MouseEventHandler(OnMouseMove);
            _tabControl.DragOver += new DragEventHandler(OnDragOver);
            
            _docked = false;

            this.Controls.Add(_tabControl);
        
        }


        private TabControl _tabControl;
        private bool _docked;
        private int _oldDockWidth;


    }
}
