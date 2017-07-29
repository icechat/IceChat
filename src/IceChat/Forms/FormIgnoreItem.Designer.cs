namespace IceChat
{
    partial class FormIgnoreItem
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
            this.textNick = new System.Windows.Forms.TextBox();
            this.labelBuddyNick = new System.Windows.Forms.Label();
            this.checkChannel = new System.Windows.Forms.CheckBox();
            this.checkPrivate = new System.Windows.Forms.CheckBox();
            this.checkNotice = new System.Windows.Forms.CheckBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.checkCTCP = new System.Windows.Forms.CheckBox();
            this.checkInvite = new System.Windows.Forms.CheckBox();
            this.checkDCC = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textNick
            // 
            this.textNick.Location = new System.Drawing.Point(122, 6);
            this.textNick.Name = "textNick";
            this.textNick.Size = new System.Drawing.Size(172, 23);
            this.textNick.TabIndex = 4;
            // 
            // labelBuddyNick
            // 
            this.labelBuddyNick.AutoSize = true;
            this.labelBuddyNick.Location = new System.Drawing.Point(13, 9);
            this.labelBuddyNick.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBuddyNick.Name = "labelBuddyNick";
            this.labelBuddyNick.Size = new System.Drawing.Size(71, 16);
            this.labelBuddyNick.TabIndex = 3;
            this.labelBuddyNick.Text = "Nick/Host";
            // 
            // checkChannel
            // 
            this.checkChannel.AutoSize = true;
            this.checkChannel.Location = new System.Drawing.Point(16, 43);
            this.checkChannel.Name = "checkChannel";
            this.checkChannel.Size = new System.Drawing.Size(79, 20);
            this.checkChannel.TabIndex = 6;
            this.checkChannel.Text = "Channel";
            this.checkChannel.UseVisualStyleBackColor = true;
            // 
            // checkPrivate
            // 
            this.checkPrivate.AutoSize = true;
            this.checkPrivate.Location = new System.Drawing.Point(16, 69);
            this.checkPrivate.Name = "checkPrivate";
            this.checkPrivate.Size = new System.Drawing.Size(73, 20);
            this.checkPrivate.TabIndex = 7;
            this.checkPrivate.Text = "Private";
            this.checkPrivate.UseVisualStyleBackColor = true;
            // 
            // checkNotice
            // 
            this.checkNotice.AutoSize = true;
            this.checkNotice.Location = new System.Drawing.Point(16, 95);
            this.checkNotice.Name = "checkNotice";
            this.checkNotice.Size = new System.Drawing.Size(69, 20);
            this.checkNotice.TabIndex = 8;
            this.checkNotice.Text = "Notice";
            this.checkNotice.UseVisualStyleBackColor = true;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.AutoSize = true;
            this.buttonUpdate.Location = new System.Drawing.Point(171, 121);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(122, 26);
            this.buttonUpdate.TabIndex = 9;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // checkCTCP
            // 
            this.checkCTCP.AutoSize = true;
            this.checkCTCP.Location = new System.Drawing.Point(215, 43);
            this.checkCTCP.Name = "checkCTCP";
            this.checkCTCP.Size = new System.Drawing.Size(62, 20);
            this.checkCTCP.TabIndex = 10;
            this.checkCTCP.Text = "CTCP";
            this.checkCTCP.UseVisualStyleBackColor = true;
            // 
            // checkInvite
            // 
            this.checkInvite.AutoSize = true;
            this.checkInvite.Location = new System.Drawing.Point(215, 69);
            this.checkInvite.Name = "checkInvite";
            this.checkInvite.Size = new System.Drawing.Size(65, 20);
            this.checkInvite.TabIndex = 11;
            this.checkInvite.Text = "Invite";
            this.checkInvite.UseVisualStyleBackColor = true;
            // 
            // checkDCC
            // 
            this.checkDCC.AutoSize = true;
            this.checkDCC.Location = new System.Drawing.Point(215, 95);
            this.checkDCC.Name = "checkDCC";
            this.checkDCC.Size = new System.Drawing.Size(54, 20);
            this.checkDCC.TabIndex = 12;
            this.checkDCC.Text = "DCC";
            this.checkDCC.UseVisualStyleBackColor = true;
            // 
            // FormIgnoreItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 158);
            this.Controls.Add(this.checkDCC);
            this.Controls.Add(this.checkInvite);
            this.Controls.Add(this.checkCTCP);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.checkNotice);
            this.Controls.Add(this.checkPrivate);
            this.Controls.Add(this.checkChannel);
            this.Controls.Add(this.textNick);
            this.Controls.Add(this.labelBuddyNick);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormIgnoreItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ignore Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textNick;
        private System.Windows.Forms.Label labelBuddyNick;
        private System.Windows.Forms.CheckBox checkChannel;
        private System.Windows.Forms.CheckBox checkPrivate;
        private System.Windows.Forms.CheckBox checkNotice;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.CheckBox checkCTCP;
        private System.Windows.Forms.CheckBox checkInvite;
        private System.Windows.Forms.CheckBox checkDCC;
    }
}