namespace IceChat
{
    partial class BuddyList
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Disconnected");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Connected");
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonMessage = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.treeBuddies = new System.Windows.Forms.TreeView();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonRemove);
            this.panelButtons.Controls.Add(this.buttonEdit);
            this.panelButtons.Controls.Add(this.buttonMessage);
            this.panelButtons.Controls.Add(this.buttonAdd);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelButtons.Location = new System.Drawing.Point(0, 235);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(175, 57);
            this.panelButtons.TabIndex = 3;
            this.panelButtons.Visible = false;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(62, 28);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(86, 24);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Visible = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(2, 28);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(54, 24);
            this.buttonEdit.TabIndex = 2;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Visible = false;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonMessage
            // 
            this.buttonMessage.Location = new System.Drawing.Point(62, 2);
            this.buttonMessage.Name = "buttonMessage";
            this.buttonMessage.Size = new System.Drawing.Size(86, 24);
            this.buttonMessage.TabIndex = 1;
            this.buttonMessage.Text = "Message";
            this.buttonMessage.UseVisualStyleBackColor = true;
            this.buttonMessage.Click += new System.EventHandler(this.buttonMessage_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdd.Location = new System.Drawing.Point(2, 2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(54, 24);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Visible = false;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // treeBuddies
            // 
            this.treeBuddies.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeBuddies.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeBuddies.HideSelection = false;
            this.treeBuddies.Location = new System.Drawing.Point(0, 20);
            this.treeBuddies.Name = "treeBuddies";
            treeNode1.Name = "nodeDisconnected";
            treeNode1.Text = "Disconnected";
            treeNode2.Name = "nodeConnected";
            treeNode2.Text = "Connected";
            this.treeBuddies.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.treeBuddies.Size = new System.Drawing.Size(175, 212);
            this.treeBuddies.TabIndex = 4;
            // 
            // BuddyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeBuddies);
            this.Controls.Add(this.panelButtons);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "BuddyList";
            this.Size = new System.Drawing.Size(175, 292);
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonMessage;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.TreeView treeBuddies;
    }
}
