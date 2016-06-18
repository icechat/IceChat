namespace IceChat
{
    partial class FormUserInfo
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
            this.labelTopicSetBy = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listChannels = new System.Windows.Forms.ListBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textNick = new System.Windows.Forms.TextBox();
            this.textHost = new System.Windows.Forms.TextBox();
            this.textFullName = new System.Windows.Forms.TextBox();
            this.textIdleTime = new System.Windows.Forms.TextBox();
            this.textLogonTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textAwayStatus = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textServer = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textCtcpReply = new System.Windows.Forms.TextBox();
            this.buttonPing = new System.Windows.Forms.Button();
            this.buttonVersion = new System.Windows.Forms.Button();
            this.textLoggedIn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textSendMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelTopicSetBy
            // 
            this.labelTopicSetBy.AutoSize = true;
            this.labelTopicSetBy.Location = new System.Drawing.Point(12, 9);
            this.labelTopicSetBy.Name = "labelTopicSetBy";
            this.labelTopicSetBy.Size = new System.Drawing.Size(82, 16);
            this.labelTopicSetBy.TabIndex = 2;
            this.labelTopicSetBy.Text = "Nick Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Host:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Full Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Idle Time:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Logged on at:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "On Channels:";
            // 
            // listChannels
            // 
            this.listChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listChannels.FormattingEnabled = true;
            this.listChannels.ItemHeight = 16;
            this.listChannels.Location = new System.Drawing.Point(12, 268);
            this.listChannels.Name = "listChannels";
            this.listChannels.Size = new System.Drawing.Size(407, 180);
            this.listChannels.TabIndex = 9;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSize = true;
            this.buttonClose.Location = new System.Drawing.Point(354, 538);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 26);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textNick
            // 
            this.textNick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textNick.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textNick.Location = new System.Drawing.Point(144, 6);
            this.textNick.Name = "textNick";
            this.textNick.ReadOnly = true;
            this.textNick.Size = new System.Drawing.Size(275, 22);
            this.textNick.TabIndex = 11;
            // 
            // textHost
            // 
            this.textHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textHost.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textHost.Location = new System.Drawing.Point(144, 35);
            this.textHost.Name = "textHost";
            this.textHost.ReadOnly = true;
            this.textHost.Size = new System.Drawing.Size(275, 22);
            this.textHost.TabIndex = 12;
            // 
            // textFullName
            // 
            this.textFullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textFullName.Location = new System.Drawing.Point(144, 93);
            this.textFullName.Name = "textFullName";
            this.textFullName.ReadOnly = true;
            this.textFullName.Size = new System.Drawing.Size(275, 23);
            this.textFullName.TabIndex = 13;
            // 
            // textIdleTime
            // 
            this.textIdleTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textIdleTime.Location = new System.Drawing.Point(144, 122);
            this.textIdleTime.Name = "textIdleTime";
            this.textIdleTime.ReadOnly = true;
            this.textIdleTime.Size = new System.Drawing.Size(275, 23);
            this.textIdleTime.TabIndex = 14;
            // 
            // textLogonTime
            // 
            this.textLogonTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textLogonTime.Location = new System.Drawing.Point(144, 180);
            this.textLogonTime.Name = "textLogonTime";
            this.textLogonTime.ReadOnly = true;
            this.textLogonTime.Size = new System.Drawing.Size(275, 23);
            this.textLogonTime.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 16);
            this.label7.TabIndex = 17;
            this.label7.Text = "Away Status:";
            // 
            // textAwayStatus
            // 
            this.textAwayStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textAwayStatus.Location = new System.Drawing.Point(144, 209);
            this.textAwayStatus.Name = "textAwayStatus";
            this.textAwayStatus.ReadOnly = true;
            this.textAwayStatus.Size = new System.Drawing.Size(275, 23);
            this.textAwayStatus.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 16);
            this.label8.TabIndex = 19;
            this.label8.Text = "Server:";
            // 
            // textServer
            // 
            this.textServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textServer.Location = new System.Drawing.Point(144, 151);
            this.textServer.Name = "textServer";
            this.textServer.ReadOnly = true;
            this.textServer.Size = new System.Drawing.Size(275, 23);
            this.textServer.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 510);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "Ctcp Reply:";
            // 
            // textCtcpReply
            // 
            this.textCtcpReply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textCtcpReply.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textCtcpReply.Location = new System.Drawing.Point(144, 508);
            this.textCtcpReply.Name = "textCtcpReply";
            this.textCtcpReply.ReadOnly = true;
            this.textCtcpReply.Size = new System.Drawing.Size(275, 22);
            this.textCtcpReply.TabIndex = 22;
            // 
            // buttonPing
            // 
            this.buttonPing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPing.AutoSize = true;
            this.buttonPing.Location = new System.Drawing.Point(15, 535);
            this.buttonPing.Name = "buttonPing";
            this.buttonPing.Size = new System.Drawing.Size(75, 26);
            this.buttonPing.TabIndex = 23;
            this.buttonPing.Text = "Ping";
            this.buttonPing.UseVisualStyleBackColor = true;
            this.buttonPing.Click += new System.EventHandler(this.buttonPing_Click);
            // 
            // buttonVersion
            // 
            this.buttonVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVersion.AutoSize = true;
            this.buttonVersion.Location = new System.Drawing.Point(96, 535);
            this.buttonVersion.Name = "buttonVersion";
            this.buttonVersion.Size = new System.Drawing.Size(75, 26);
            this.buttonVersion.TabIndex = 24;
            this.buttonVersion.Text = "Version";
            this.buttonVersion.UseVisualStyleBackColor = true;
            this.buttonVersion.Click += new System.EventHandler(this.buttonVersion_Click);
            // 
            // textLoggedIn
            // 
            this.textLoggedIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textLoggedIn.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textLoggedIn.Location = new System.Drawing.Point(144, 64);
            this.textLoggedIn.Name = "textLoggedIn";
            this.textLoggedIn.ReadOnly = true;
            this.textLoggedIn.Size = new System.Drawing.Size(275, 22);
            this.textLoggedIn.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Logged in as:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 471);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 16);
            this.label10.TabIndex = 27;
            this.label10.Text = "Send Message:";
            // 
            // textSendMessage
            // 
            this.textSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textSendMessage.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textSendMessage.Location = new System.Drawing.Point(144, 469);
            this.textSendMessage.Name = "textSendMessage";
            this.textSendMessage.Size = new System.Drawing.Size(275, 22);
            this.textSendMessage.TabIndex = 28;
            this.textSendMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textSendMessage_KeyPress);
            // 
            // FormUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 573);
            this.Controls.Add(this.textSendMessage);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textLoggedIn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonVersion);
            this.Controls.Add(this.buttonPing);
            this.Controls.Add(this.textCtcpReply);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textServer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textAwayStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textLogonTime);
            this.Controls.Add(this.textIdleTime);
            this.Controls.Add(this.textFullName);
            this.Controls.Add(this.textHost);
            this.Controls.Add(this.textNick);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.listChannels);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTopicSetBy);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUserInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTopicSetBy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listChannels;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textNick;
        private System.Windows.Forms.TextBox textHost;
        private System.Windows.Forms.TextBox textFullName;
        private System.Windows.Forms.TextBox textIdleTime;
        private System.Windows.Forms.TextBox textLogonTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textAwayStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textServer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textCtcpReply;
        private System.Windows.Forms.Button buttonPing;
        private System.Windows.Forms.Button buttonVersion;
        private System.Windows.Forms.TextBox textLoggedIn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textSendMessage;
    }
}