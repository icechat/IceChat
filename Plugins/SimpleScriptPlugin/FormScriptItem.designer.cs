namespace IceChatPlugin
{
    partial class FormScriptItem
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
            this.textTextMatch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textCommand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboScriptEvent = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textChannelMatch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to Match";
            // 
            // textTextMatch
            // 
            this.textTextMatch.Location = new System.Drawing.Point(13, 76);
            this.textTextMatch.Margin = new System.Windows.Forms.Padding(4);
            this.textTextMatch.Name = "textTextMatch";
            this.textTextMatch.Size = new System.Drawing.Size(275, 23);
            this.textTextMatch.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 151);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Command to Run";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(12, 213);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(116, 23);
            this.buttonUpdate.TabIndex = 4;
            this.buttonUpdate.Text = "Update Item";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textCommand
            // 
            this.textCommand.Location = new System.Drawing.Point(12, 171);
            this.textCommand.Margin = new System.Windows.Forms.Padding(4);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(275, 23);
            this.textCommand.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Script Event";
            // 
            // comboScriptEvent
            // 
            this.comboScriptEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScriptEvent.FormattingEnabled = true;
            this.comboScriptEvent.Location = new System.Drawing.Point(15, 29);
            this.comboScriptEvent.Name = "comboScriptEvent";
            this.comboScriptEvent.Size = new System.Drawing.Size(273, 24);
            this.comboScriptEvent.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 103);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Channel/Nick Match";
            // 
            // textChannelMatch
            // 
            this.textChannelMatch.Location = new System.Drawing.Point(12, 123);
            this.textChannelMatch.Margin = new System.Windows.Forms.Padding(4);
            this.textChannelMatch.Name = "textChannelMatch";
            this.textChannelMatch.Size = new System.Drawing.Size(275, 23);
            this.textChannelMatch.TabIndex = 2;
            // 
            // FormScriptItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 248);
            this.Controls.Add(this.textChannelMatch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboScriptEvent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textCommand);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textTextMatch);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormScriptItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Script Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textTextMatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textCommand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboScriptEvent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textChannelMatch;
    }
}