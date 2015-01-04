namespace IceChat
{
    partial class FormBuddyItem
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
            this.labelBuddyNick = new System.Windows.Forms.Label();
            this.labelBuddyNetwork = new System.Windows.Forms.Label();
            this.textBuddyNick = new System.Windows.Forms.TextBox();
            this.textBuddyNetwork = new System.Windows.Forms.TextBox();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelBuddyNick
            // 
            this.labelBuddyNick.AutoSize = true;
            this.labelBuddyNick.Location = new System.Drawing.Point(13, 9);
            this.labelBuddyNick.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBuddyNick.Name = "labelBuddyNick";
            this.labelBuddyNick.Size = new System.Drawing.Size(76, 16);
            this.labelBuddyNick.TabIndex = 0;
            this.labelBuddyNick.Text = "Nick Name";
            // 
            // labelBuddyNetwork
            // 
            this.labelBuddyNetwork.AutoSize = true;
            this.labelBuddyNetwork.Location = new System.Drawing.Point(13, 41);
            this.labelBuddyNetwork.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBuddyNetwork.Name = "labelBuddyNetwork";
            this.labelBuddyNetwork.Size = new System.Drawing.Size(62, 16);
            this.labelBuddyNetwork.TabIndex = 1;
            this.labelBuddyNetwork.Text = "Network";
            // 
            // textBuddyNick
            // 
            this.textBuddyNick.Location = new System.Drawing.Point(122, 6);
            this.textBuddyNick.Name = "textBuddyNick";
            this.textBuddyNick.Size = new System.Drawing.Size(144, 23);
            this.textBuddyNick.TabIndex = 2;
            // 
            // textBuddyNetwork
            // 
            this.textBuddyNetwork.Location = new System.Drawing.Point(121, 38);
            this.textBuddyNetwork.Name = "textBuddyNetwork";
            this.textBuddyNetwork.Size = new System.Drawing.Size(144, 23);
            this.textBuddyNetwork.TabIndex = 3;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.AutoSize = true;
            this.buttonUpdate.Location = new System.Drawing.Point(140, 80);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(122, 26);
            this.buttonUpdate.TabIndex = 4;
            this.buttonUpdate.Text = "Add/Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // FormBuddyItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 113);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.textBuddyNetwork);
            this.Controls.Add(this.textBuddyNick);
            this.Controls.Add(this.labelBuddyNetwork);
            this.Controls.Add(this.labelBuddyNick);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "FormBuddyItem";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Buddy List Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBuddyNick;
        private System.Windows.Forms.Label labelBuddyNetwork;
        private System.Windows.Forms.TextBox textBuddyNick;
        private System.Windows.Forms.TextBox textBuddyNetwork;
        private System.Windows.Forms.Button buttonUpdate;
    }
}