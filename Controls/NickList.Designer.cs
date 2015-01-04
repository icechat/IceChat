namespace IceChat
{
    partial class NickList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonWhois = new System.Windows.Forms.Button();
            this.buttonKick = new System.Windows.Forms.Button();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.buttonHop = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonBan = new System.Windows.Forms.Button();
            this.buttonVoice = new System.Windows.Forms.Button();
            this.buttonOp = new System.Windows.Forms.Button();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.contextMenuNickList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.opToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelButtons.SuspendLayout();
            this.contextMenuNickList.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonWhois);
            this.panelButtons.Controls.Add(this.buttonKick);
            this.panelButtons.Controls.Add(this.buttonQuery);
            this.panelButtons.Controls.Add(this.buttonHop);
            this.panelButtons.Controls.Add(this.buttonInfo);
            this.panelButtons.Controls.Add(this.buttonBan);
            this.panelButtons.Controls.Add(this.buttonVoice);
            this.panelButtons.Controls.Add(this.buttonOp);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(0, 281);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(4);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(200, 57);
            this.panelButtons.TabIndex = 0;
            // 
            // buttonWhois
            // 
            this.buttonWhois.AccessibleDescription = "Perform whois on user";
            this.buttonWhois.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonWhois.Location = new System.Drawing.Point(147, 28);
            this.buttonWhois.Margin = new System.Windows.Forms.Padding(2);
            this.buttonWhois.Name = "buttonWhois";
            this.buttonWhois.Size = new System.Drawing.Size(46, 24);
            this.buttonWhois.TabIndex = 7;
            this.buttonWhois.TabStop = false;
            this.buttonWhois.Text = "Whois";
            this.buttonWhois.UseVisualStyleBackColor = true;
            this.buttonWhois.Click += new System.EventHandler(this.buttonWhois_Click);
            // 
            // buttonKick
            // 
            this.buttonKick.AccessibleDescription = "Kick a user";
            this.buttonKick.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonKick.Location = new System.Drawing.Point(99, 28);
            this.buttonKick.Margin = new System.Windows.Forms.Padding(2);
            this.buttonKick.Name = "buttonKick";
            this.buttonKick.Size = new System.Drawing.Size(46, 24);
            this.buttonKick.TabIndex = 6;
            this.buttonKick.TabStop = false;
            this.buttonKick.Text = "Kick";
            this.buttonKick.UseVisualStyleBackColor = true;
            this.buttonKick.Click += new System.EventHandler(this.buttonKick_Click);
            // 
            // buttonQuery
            // 
            this.buttonQuery.AccessibleDescription = "Open a private message window with user";
            this.buttonQuery.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuery.Location = new System.Drawing.Point(51, 28);
            this.buttonQuery.Margin = new System.Windows.Forms.Padding(2);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(46, 24);
            this.buttonQuery.TabIndex = 5;
            this.buttonQuery.TabStop = false;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // buttonHop
            // 
            this.buttonHop.AccessibleDescription = "Give/Take Half Operator Status";
            this.buttonHop.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHop.Location = new System.Drawing.Point(2, 28);
            this.buttonHop.Margin = new System.Windows.Forms.Padding(2);
            this.buttonHop.Name = "buttonHop";
            this.buttonHop.Size = new System.Drawing.Size(44, 24);
            this.buttonHop.TabIndex = 4;
            this.buttonHop.TabStop = false;
            this.buttonHop.Text = "H-Op";
            this.buttonHop.UseVisualStyleBackColor = true;
            this.buttonHop.Click += new System.EventHandler(this.buttonHop_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.AccessibleDescription = "Show user information";
            this.buttonInfo.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInfo.Location = new System.Drawing.Point(147, 2);
            this.buttonInfo.Margin = new System.Windows.Forms.Padding(2);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(46, 24);
            this.buttonInfo.TabIndex = 3;
            this.buttonInfo.TabStop = false;
            this.buttonInfo.Text = "Info";
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // buttonBan
            // 
            this.buttonBan.AccessibleDescription = "Ban a user";
            this.buttonBan.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBan.Location = new System.Drawing.Point(99, 2);
            this.buttonBan.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBan.Name = "buttonBan";
            this.buttonBan.Size = new System.Drawing.Size(46, 24);
            this.buttonBan.TabIndex = 2;
            this.buttonBan.TabStop = false;
            this.buttonBan.Text = "Ban";
            this.buttonBan.UseVisualStyleBackColor = true;
            this.buttonBan.Click += new System.EventHandler(this.buttonBan_Click);
            // 
            // buttonVoice
            // 
            this.buttonVoice.AccessibleDescription = "Give/Take Voice Status";
            this.buttonVoice.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVoice.Location = new System.Drawing.Point(51, 2);
            this.buttonVoice.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVoice.Name = "buttonVoice";
            this.buttonVoice.Size = new System.Drawing.Size(46, 24);
            this.buttonVoice.TabIndex = 1;
            this.buttonVoice.TabStop = false;
            this.buttonVoice.Text = "Voice";
            this.buttonVoice.UseVisualStyleBackColor = true;
            this.buttonVoice.Click += new System.EventHandler(this.buttonVoice_Click);
            // 
            // buttonOp
            // 
            this.buttonOp.AccessibleDescription = "Give/Take Operator Status";
            this.buttonOp.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOp.Location = new System.Drawing.Point(2, 2);
            this.buttonOp.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOp.Name = "buttonOp";
            this.buttonOp.Size = new System.Drawing.Size(44, 24);
            this.buttonOp.TabIndex = 0;
            this.buttonOp.TabStop = false;
            this.buttonOp.Text = "Op";
            this.buttonOp.UseVisualStyleBackColor = true;
            this.buttonOp.Click += new System.EventHandler(this.buttonOp_Click);
            // 
            // vScrollBar
            // 
            this.vScrollBar.LargeChange = 2;
            this.vScrollBar.Location = new System.Drawing.Point(183, 23);
            this.vScrollBar.Maximum = 1;
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Padding = new System.Windows.Forms.Padding(0, 23, 0, 0);
            this.vScrollBar.Size = new System.Drawing.Size(17, 258);
            this.vScrollBar.TabIndex = 2;
            this.vScrollBar.Value = 1;
            this.vScrollBar.Visible = false;
            // 
            // contextMenuNickList
            // 
            this.contextMenuNickList.BackColor = System.Drawing.SystemColors.Menu;
            this.contextMenuNickList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opToolStripMenuItem});
            this.contextMenuNickList.Name = "contextMenuNickList";
            this.contextMenuNickList.Size = new System.Drawing.Size(153, 48);
            // 
            // opToolStripMenuItem
            // 
            this.opToolStripMenuItem.ForeColor = System.Drawing.SystemColors.MenuText;
            this.opToolStripMenuItem.Name = "opToolStripMenuItem";
            this.opToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.opToolStripMenuItem.Text = "Op";
            // 
            // NickList
            // 
            this.AccessibleDescription = "List of Nick Names";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NickList";
            this.Size = new System.Drawing.Size(200, 338);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.panelButtons.ResumeLayout(false);
            this.contextMenuNickList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonOp;
        private System.Windows.Forms.Button buttonVoice;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.ContextMenuStrip contextMenuNickList;
        private System.Windows.Forms.ToolStripMenuItem opToolStripMenuItem;
        private System.Windows.Forms.Button buttonWhois;
        private System.Windows.Forms.Button buttonKick;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Button buttonHop;
        private System.Windows.Forms.Button buttonInfo;
        private System.Windows.Forms.Button buttonBan;

    }
}
