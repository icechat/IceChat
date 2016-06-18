namespace IceChat
{
    partial class QuickConnect
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
            this.label1 = new System.Windows.Forms.Label();
            this.textServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textNick = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textChannel = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Name";
            // 
            // textServer
            // 
            this.textServer.Location = new System.Drawing.Point(114, 13);
            this.textServer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textServer.Name = "textServer";
            this.textServer.Size = new System.Drawing.Size(146, 23);
            this.textServer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nick Name";
            // 
            // textNick
            // 
            this.textNick.Location = new System.Drawing.Point(114, 44);
            this.textNick.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textNick.Name = "textNick";
            this.textNick.Size = new System.Drawing.Size(146, 23);
            this.textNick.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Channel";
            // 
            // textChannel
            // 
            this.textChannel.Location = new System.Drawing.Point(114, 75);
            this.textChannel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textChannel.Name = "textChannel";
            this.textChannel.Size = new System.Drawing.Size(146, 23);
            this.textChannel.TabIndex = 5;
            // 
            // buttonConnect
            // 
            this.buttonConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonConnect.Location = new System.Drawing.Point(269, 73);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(86, 25);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // QuickConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(365, 103);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textChannel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textNick);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textServer);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickConnect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Quick Connect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textNick;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textChannel;
        private System.Windows.Forms.Button buttonConnect;
    }
}