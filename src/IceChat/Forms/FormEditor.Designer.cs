namespace IceChat
{
    partial class FormEditor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.popupTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nickListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.channelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textPopups = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textAliases = new System.Windows.Forms.TextBox();
            this.tabControlEditor = new System.Windows.Forms.TabControl();
            this.panelBottom.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControlEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonHelp);
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.buttonSave);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 322);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(2);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(632, 33);
            this.panelBottom.TabIndex = 1;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonHelp.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonHelp.Location = new System.Drawing.Point(6, 4);
            this.buttonHelp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(100, 25);
            this.buttonHelp.TabIndex = 26;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            this.buttonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(420, 4);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 25);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSave.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSave.Location = new System.Drawing.Point(528, 4);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 25);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuStripMain.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem2,
            this.popupTypeToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(632, 24);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // mainToolStripMenuItem2
            // 
            this.mainToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.closeToolStripMenuItem1});
            this.mainToolStripMenuItem2.Name = "mainToolStripMenuItem2";
            this.mainToolStripMenuItem2.Size = new System.Drawing.Size(42, 20);
            this.mainToolStripMenuItem2.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem1
            // 
            this.closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            this.closeToolStripMenuItem1.Size = new System.Drawing.Size(109, 22);
            this.closeToolStripMenuItem1.Text = "Exit";
            this.closeToolStripMenuItem1.Click += new System.EventHandler(this.CloseToolStripMenuItem1_Click);
            // 
            // popupTypeToolStripMenuItem
            // 
            this.popupTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nickListToolStripMenuItem,
            this.consoleToolStripMenuItem,
            this.channelToolStripMenuItem,
            this.queryToolStripMenuItem});
            this.popupTypeToolStripMenuItem.Name = "popupTypeToolStripMenuItem";
            this.popupTypeToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.popupTypeToolStripMenuItem.Text = "PopupType";
            // 
            // nickListToolStripMenuItem
            // 
            this.nickListToolStripMenuItem.Name = "nickListToolStripMenuItem";
            this.nickListToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.nickListToolStripMenuItem.Text = "NickList";
            this.nickListToolStripMenuItem.Click += new System.EventHandler(this.NickListToolStripMenuItem_Click);
            // 
            // consoleToolStripMenuItem
            // 
            this.consoleToolStripMenuItem.Name = "consoleToolStripMenuItem";
            this.consoleToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.consoleToolStripMenuItem.Text = "Console";
            this.consoleToolStripMenuItem.Click += new System.EventHandler(this.ConsoleToolStripMenuItem_Click);
            // 
            // channelToolStripMenuItem
            // 
            this.channelToolStripMenuItem.Name = "channelToolStripMenuItem";
            this.channelToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.channelToolStripMenuItem.Text = "Channel";
            this.channelToolStripMenuItem.Click += new System.EventHandler(this.ChannelToolStripMenuItem_Click);
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.queryToolStripMenuItem.Text = "Query";
            this.queryToolStripMenuItem.Click += new System.EventHandler(this.QueryToolStripMenuItem_Click);
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.mainToolStripMenuItem.Text = "Main";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // mainToolStripMenuItem1
            // 
            this.mainToolStripMenuItem1.Name = "mainToolStripMenuItem1";
            this.mainToolStripMenuItem1.Size = new System.Drawing.Size(32, 19);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textPopups);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(624, 269);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PopupMenus";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textPopups
            // 
            this.textPopups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textPopups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textPopups.Location = new System.Drawing.Point(2, 2);
            this.textPopups.Margin = new System.Windows.Forms.Padding(2);
            this.textPopups.Multiline = true;
            this.textPopups.Name = "textPopups";
            this.textPopups.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textPopups.Size = new System.Drawing.Size(620, 265);
            this.textPopups.TabIndex = 1;
            this.textPopups.WordWrap = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textAliases);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(624, 269);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Aliases";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textAliases
            // 
            this.textAliases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textAliases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textAliases.Location = new System.Drawing.Point(2, 2);
            this.textAliases.Margin = new System.Windows.Forms.Padding(2);
            this.textAliases.Multiline = true;
            this.textAliases.Name = "textAliases";
            this.textAliases.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textAliases.Size = new System.Drawing.Size(620, 265);
            this.textAliases.TabIndex = 0;
            this.textAliases.WordWrap = false;
            // 
            // tabControlEditor
            // 
            this.tabControlEditor.Controls.Add(this.tabPage1);
            this.tabControlEditor.Controls.Add(this.tabPage2);
            this.tabControlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlEditor.Location = new System.Drawing.Point(0, 24);
            this.tabControlEditor.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlEditor.Name = "tabControlEditor";
            this.tabControlEditor.SelectedIndex = 0;
            this.tabControlEditor.Size = new System.Drawing.Size(632, 298);
            this.tabControlEditor.TabIndex = 0;
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(632, 355);
            this.Controls.Add(this.tabControlEditor);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "IceChat Editor";
            this.panelBottom.ResumeLayout(false);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControlEditor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem popupTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nickListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem channelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textPopups;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textAliases;
        private System.Windows.Forms.TabControl tabControlEditor;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button buttonHelp;
    }
}