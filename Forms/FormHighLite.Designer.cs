namespace IceChat2009
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Text to Highlight";
            // 
            // textHiLite
            // 
            this.textHiLite.Location = new System.Drawing.Point(13, 29);
            this.textHiLite.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.label2.Text = "Command to Run (Optional)";
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(15, 201);
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
            this.panelColorPicker.Location = new System.Drawing.Point(16, 122);
            this.panelColorPicker.Name = "panelColorPicker";
            this.panelColorPicker.Size = new System.Drawing.Size(288, 40);
            this.panelColorPicker.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 103);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "Select Highlight Color";
            // 
            // FormHighLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 236);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelColorPicker);
            this.Controls.Add(this.textCommand);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textHiLite);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
    }
}