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
            this.textNickMatch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelColorPicker = new System.Windows.Forms.Panel();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.textHostMatch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textNickMatch
            // 
            this.textNickMatch.Location = new System.Drawing.Point(13, 29);
            this.textNickMatch.Margin = new System.Windows.Forms.Padding(4);
            this.textNickMatch.Name = "textNickMatch";
            this.textNickMatch.Size = new System.Drawing.Size(275, 23);
            this.textNickMatch.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nick to Highlight (leave blank to match all)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Select Highlight Color";
            // 
            // panelColorPicker
            // 
            this.panelColorPicker.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.panelColorPicker.Location = new System.Drawing.Point(13, 130);
            this.panelColorPicker.Name = "panelColorPicker";
            this.panelColorPicker.Size = new System.Drawing.Size(288, 122);
            this.panelColorPicker.TabIndex = 23;
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(13, 267);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(116, 23);
            this.buttonUpdate.TabIndex = 25;
            this.buttonUpdate.Text = "Update Item";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // textHostMatch
            // 
            this.textHostMatch.Location = new System.Drawing.Point(16, 84);
            this.textHostMatch.Margin = new System.Windows.Forms.Padding(4);
            this.textHostMatch.Name = "textHostMatch";
            this.textHostMatch.Size = new System.Drawing.Size(275, 23);
            this.textHostMatch.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "Host to Highlight (leave blank to match all)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(138, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "use .* for wildcard matching";
            // 
            // FormHighLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 298);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textHostMatch);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panelColorPicker);
            this.Controls.Add(this.textNickMatch);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHighLite";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Highlite Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textNickMatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panelColorPicker;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.TextBox textHostMatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}