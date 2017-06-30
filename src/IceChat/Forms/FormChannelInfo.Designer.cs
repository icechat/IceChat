namespace IceChat
{
    partial class FormChannelInfo
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textChannelKey = new System.Windows.Forms.TextBox();
            this.checkModek = new System.Windows.Forms.CheckBox();
            this.textMaxUsers = new System.Windows.Forms.TextBox();
            this.checkModel = new System.Windows.Forms.CheckBox();
            this.checkModes = new System.Windows.Forms.CheckBox();
            this.checkModem = new System.Windows.Forms.CheckBox();
            this.checkModei = new System.Windows.Forms.CheckBox();
            this.checkModen = new System.Windows.Forms.CheckBox();
            this.checkModet = new System.Windows.Forms.CheckBox();
            this.labelTopicSetBy = new System.Windows.Forms.Label();
            this.textTopic = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonRemoveBan = new System.Windows.Forms.Button();
            this.listViewBans = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStripBans = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonRemoveException = new System.Windows.Forms.Button();
            this.listViewExceptions = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.listViewQuiet = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStripBans.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(445, 339);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textChannelKey);
            this.tabPage1.Controls.Add(this.checkModek);
            this.tabPage1.Controls.Add(this.textMaxUsers);
            this.tabPage1.Controls.Add(this.checkModel);
            this.tabPage1.Controls.Add(this.checkModes);
            this.tabPage1.Controls.Add(this.checkModem);
            this.tabPage1.Controls.Add(this.checkModei);
            this.tabPage1.Controls.Add(this.checkModen);
            this.tabPage1.Controls.Add(this.checkModet);
            this.tabPage1.Controls.Add(this.labelTopicSetBy);
            this.tabPage1.Controls.Add(this.textTopic);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(437, 310);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Channel Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textChannelKey
            // 
            this.textChannelKey.Location = new System.Drawing.Point(185, 277);
            this.textChannelKey.Name = "textChannelKey";
            this.textChannelKey.Size = new System.Drawing.Size(128, 23);
            this.textChannelKey.TabIndex = 11;
            // 
            // checkModek
            // 
            this.checkModek.AutoSize = true;
            this.checkModek.Location = new System.Drawing.Point(6, 280);
            this.checkModek.Name = "checkModek";
            this.checkModek.Size = new System.Drawing.Size(141, 20);
            this.checkModek.TabIndex = 10;
            this.checkModek.Text = "Channel Key (+k)";
            this.checkModek.UseVisualStyleBackColor = true;
            // 
            // textMaxUsers
            // 
            this.textMaxUsers.Location = new System.Drawing.Point(185, 248);
            this.textMaxUsers.Name = "textMaxUsers";
            this.textMaxUsers.Size = new System.Drawing.Size(48, 23);
            this.textMaxUsers.TabIndex = 9;
            // 
            // checkModel
            // 
            this.checkModel.AutoSize = true;
            this.checkModel.Location = new System.Drawing.Point(6, 251);
            this.checkModel.Name = "checkModel";
            this.checkModel.Size = new System.Drawing.Size(156, 20);
            this.checkModel.TabIndex = 8;
            this.checkModel.Text = "Maximum Users (+l)";
            this.checkModel.UseVisualStyleBackColor = true;
            // 
            // checkModes
            // 
            this.checkModes.AutoSize = true;
            this.checkModes.Location = new System.Drawing.Point(6, 225);
            this.checkModes.Name = "checkModes";
            this.checkModes.Size = new System.Drawing.Size(104, 20);
            this.checkModes.TabIndex = 7;
            this.checkModes.Text = "Secret (+s)";
            this.checkModes.UseVisualStyleBackColor = true;
            // 
            // checkModem
            // 
            this.checkModem.AutoSize = true;
            this.checkModem.Location = new System.Drawing.Point(6, 199);
            this.checkModem.Name = "checkModem";
            this.checkModem.Size = new System.Drawing.Size(134, 20);
            this.checkModem.TabIndex = 6;
            this.checkModem.Text = "Moderated (+m)";
            this.checkModem.UseVisualStyleBackColor = true;
            // 
            // checkModei
            // 
            this.checkModei.AutoSize = true;
            this.checkModei.Location = new System.Drawing.Point(6, 173);
            this.checkModei.Name = "checkModei";
            this.checkModei.Size = new System.Drawing.Size(128, 20);
            this.checkModei.TabIndex = 5;
            this.checkModei.Text = "Invite Only (+i)";
            this.checkModei.UseVisualStyleBackColor = true;
            // 
            // checkModen
            // 
            this.checkModen.AutoSize = true;
            this.checkModen.Location = new System.Drawing.Point(6, 147);
            this.checkModen.Name = "checkModen";
            this.checkModen.Size = new System.Drawing.Size(205, 20);
            this.checkModen.TabIndex = 4;
            this.checkModen.Text = "No external messages (+n)";
            this.checkModen.UseVisualStyleBackColor = true;
            // 
            // checkModet
            // 
            this.checkModet.AutoSize = true;
            this.checkModet.Location = new System.Drawing.Point(6, 121);
            this.checkModet.Name = "checkModet";
            this.checkModet.Size = new System.Drawing.Size(236, 20);
            this.checkModet.TabIndex = 3;
            this.checkModet.Text = "Only ops can change topic (+t)";
            this.checkModet.UseVisualStyleBackColor = true;
            // 
            // labelTopicSetBy
            // 
            this.labelTopicSetBy.Location = new System.Drawing.Point(3, 82);
            this.labelTopicSetBy.Name = "labelTopicSetBy";
            this.labelTopicSetBy.Size = new System.Drawing.Size(428, 31);
            this.labelTopicSetBy.TabIndex = 1;
            this.labelTopicSetBy.Text = "Topic Set By:";
            // 
            // textTopic
            // 
            this.textTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textTopic.Location = new System.Drawing.Point(6, 6);
            this.textTopic.Multiline = true;
            this.textTopic.Name = "textTopic";
            this.textTopic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textTopic.Size = new System.Drawing.Size(425, 70);
            this.textTopic.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonRemoveBan);
            this.tabPage2.Controls.Add(this.listViewBans);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(437, 310);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Ban List";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveBan
            // 
            this.buttonRemoveBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveBan.AutoSize = true;
            this.buttonRemoveBan.Location = new System.Drawing.Point(304, 278);
            this.buttonRemoveBan.Name = "buttonRemoveBan";
            this.buttonRemoveBan.Size = new System.Drawing.Size(127, 26);
            this.buttonRemoveBan.TabIndex = 1;
            this.buttonRemoveBan.Text = "Remove Ban";
            this.buttonRemoveBan.UseVisualStyleBackColor = true;
            this.buttonRemoveBan.Click += new System.EventHandler(this.buttonRemoveBan_Click);
            // 
            // listViewBans
            // 
            this.listViewBans.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewBans.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewBans.ContextMenuStrip = this.contextMenuStripBans;
            this.listViewBans.Location = new System.Drawing.Point(8, 6);
            this.listViewBans.MultiSelect = false;
            this.listViewBans.Name = "listViewBans";
            this.listViewBans.Size = new System.Drawing.Size(419, 266);
            this.listViewBans.TabIndex = 0;
            this.listViewBans.UseCompatibleStateImageBehavior = false;
            this.listViewBans.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Banned Host";
            this.columnHeader1.Width = 191;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Added By";
            this.columnHeader2.Width = 400;
            // 
            // contextMenuStripBans
            // 
            this.contextMenuStripBans.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToClipboardToolStripMenuItem});
            this.contextMenuStripBans.Name = "contextMenuStripBans";
            this.contextMenuStripBans.Size = new System.Drawing.Size(172, 26);
            // 
            // copyToClipboardToolStripMenuItem
            // 
            this.copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            this.copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            this.copyToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyToClipboardToolStripMenuItem_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonRemoveException);
            this.tabPage3.Controls.Add(this.listViewExceptions);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(437, 310);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Exception List";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonRemoveException
            // 
            this.buttonRemoveException.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemoveException.AutoSize = true;
            this.buttonRemoveException.Location = new System.Drawing.Point(253, 278);
            this.buttonRemoveException.Name = "buttonRemoveException";
            this.buttonRemoveException.Size = new System.Drawing.Size(178, 26);
            this.buttonRemoveException.TabIndex = 2;
            this.buttonRemoveException.Text = "Remove Exception";
            this.buttonRemoveException.UseVisualStyleBackColor = true;
            this.buttonRemoveException.Click += new System.EventHandler(this.buttonRemoveException_Click);
            // 
            // listViewExceptions
            // 
            this.listViewExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewExceptions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.listViewExceptions.Location = new System.Drawing.Point(8, 6);
            this.listViewExceptions.MultiSelect = false;
            this.listViewExceptions.Name = "listViewExceptions";
            this.listViewExceptions.Size = new System.Drawing.Size(419, 266);
            this.listViewExceptions.TabIndex = 1;
            this.listViewExceptions.UseCompatibleStateImageBehavior = false;
            this.listViewExceptions.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Exception Host";
            this.columnHeader3.Width = 191;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Added By";
            this.columnHeader4.Width = 400;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSize = true;
            this.buttonClose.Location = new System.Drawing.Point(348, 347);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(93, 26);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.AutoSize = true;
            this.buttonApply.Location = new System.Drawing.Point(249, 347);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(93, 26);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.listViewQuiet);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(437, 310);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Quiet List";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // listViewQuiet
            // 
            this.listViewQuiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewQuiet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.listViewQuiet.Location = new System.Drawing.Point(8, 6);
            this.listViewQuiet.MultiSelect = false;
            this.listViewQuiet.Name = "listViewQuiet";
            this.listViewQuiet.Size = new System.Drawing.Size(419, 266);
            this.listViewQuiet.TabIndex = 2;
            this.listViewQuiet.UseCompatibleStateImageBehavior = false;
            this.listViewQuiet.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Exception Host";
            this.columnHeader5.Width = 191;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Added By";
            this.columnHeader6.Width = 400;
            // 
            // FormChannelInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 382);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChannelInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Channel Information";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStripBans.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textTopic;
        private System.Windows.Forms.Label labelTopicSetBy;
        private System.Windows.Forms.CheckBox checkModem;
        private System.Windows.Forms.CheckBox checkModei;
        private System.Windows.Forms.CheckBox checkModen;
        private System.Windows.Forms.CheckBox checkModet;
        private System.Windows.Forms.TextBox textMaxUsers;
        private System.Windows.Forms.CheckBox checkModel;
        private System.Windows.Forms.CheckBox checkModes;
        private System.Windows.Forms.ListView listViewBans;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listViewExceptions;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button buttonRemoveBan;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textChannelKey;
        private System.Windows.Forms.CheckBox checkModek;
        private System.Windows.Forms.Button buttonRemoveException;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBans;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView listViewQuiet;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
    }
}