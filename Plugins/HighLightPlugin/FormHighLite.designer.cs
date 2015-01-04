namespace IceChatPlugin
{
    partial class FormHighLite
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
            this.textHiLite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textCommand = new System.Windows.Forms.TextBox();
            this.panelColorPicker = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.checkFlashTab = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textInclude = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textExclude = new System.Windows.Forms.TextBox();
            this.textPlaySound = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to highlight";
            // 
            // textHiLite
            // 
            this.textHiLite.Location = new System.Drawing.Point(13, 29);
            this.textHiLite.Margin = new System.Windows.Forms.Padding(4);
            this.textHiLite.Name = "textHiLite";
            this.textHiLite.Size = new System.Drawing.Size(275, 23);
            this.textHiLite.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Command to run  (optional)";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(12, 471);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(116, 23);
            this.buttonUpdate.TabIndex = 3;
            this.buttonUpdate.Text = "Update Item";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textCommand
            // 
            this.textCommand.Location = new System.Drawing.Point(13, 76);
            this.textCommand.Margin = new System.Windows.Forms.Padding(4);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(275, 23);
            this.textCommand.TabIndex = 4;
            // 
            // panelColorPicker
            // 
            this.panelColorPicker.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.panelColorPicker.Location = new System.Drawing.Point(16, 339);
            this.panelColorPicker.Name = "panelColorPicker";
            this.panelColorPicker.Size = new System.Drawing.Size(288, 122);
            this.panelColorPicker.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 320);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "Select highlight color";
            // 
            // checkFlashTab
            // 
            this.checkFlashTab.AutoSize = true;
            this.checkFlashTab.Location = new System.Drawing.Point(15, 297);
            this.checkFlashTab.Name = "checkFlashTab";
            this.checkFlashTab.Size = new System.Drawing.Size(225, 20);
            this.checkFlashTab.TabIndex = 23;
            this.checkFlashTab.Text = "Flash channel tab on highlight";
            this.checkFlashTab.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 155);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(218, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "Highlight only for selected nicks";
            // 
            // textInclude
            // 
            this.textInclude.Location = new System.Drawing.Point(12, 175);
            this.textInclude.Margin = new System.Windows.Forms.Padding(4);
            this.textInclude.Name = "textInclude";
            this.textInclude.Size = new System.Drawing.Size(275, 23);
            this.textInclude.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 202);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "Highlight for all except selected nicks";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 249);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(209, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "(seperate above nicks with spaces)";
            // 
            // textExclude
            // 
            this.textExclude.Location = new System.Drawing.Point(13, 222);
            this.textExclude.Margin = new System.Windows.Forms.Padding(4);
            this.textExclude.Name = "textExclude";
            this.textExclude.Size = new System.Drawing.Size(275, 23);
            this.textExclude.TabIndex = 28;
            // 
            // textPlaySound
            // 
            this.textPlaySound.Location = new System.Drawing.Point(13, 126);
            this.textPlaySound.Margin = new System.Windows.Forms.Padding(4);
            this.textPlaySound.Name = "textPlaySound";
            this.textPlaySound.Size = new System.Drawing.Size(275, 23);
            this.textPlaySound.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 106);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(257, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "Sound to play  (files in sounds folder)";
            // 
            // FormHighLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 507);
            this.Controls.Add(this.textPlaySound);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textExclude);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textInclude);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkFlashTab);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelColorPicker);
            this.Controls.Add(this.textCommand);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textHiLite);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHighLite";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Highlight Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textHiLite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textCommand;
        private System.Windows.Forms.Panel panelColorPicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkFlashTab;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textInclude;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textExclude;
        private System.Windows.Forms.TextBox textPlaySound;
        private System.Windows.Forms.Label label7;
    }
}