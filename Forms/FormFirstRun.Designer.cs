namespace IceChat
{
    partial class FormFirstRun
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
            this.labelDesc = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelField = new System.Windows.Forms.Label();
            this.labelTip = new System.Windows.Forms.Label();
            this.textData = new System.Windows.Forms.TextBox();
            this.comboData = new System.Windows.Forms.ComboBox();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelDesc
            // 
            this.labelDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDesc.Location = new System.Drawing.Point(16, 46);
            this.labelDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(499, 170);
            this.labelDesc.TabIndex = 0;
            this.labelDesc.Text = "Description";
            // 
            // labelHeader
            // 
            this.labelHeader.BackColor = System.Drawing.Color.White;
            this.labelHeader.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(11, 9);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(504, 28);
            this.labelHeader.TabIndex = 1;
            this.labelHeader.Text = "Header";
            // 
            // labelField
            // 
            this.labelField.AutoSize = true;
            this.labelField.Location = new System.Drawing.Point(13, 235);
            this.labelField.Name = "labelField";
            this.labelField.Size = new System.Drawing.Size(44, 16);
            this.labelField.TabIndex = 2;
            this.labelField.Text = "Field:";
            // 
            // labelTip
            // 
            this.labelTip.ForeColor = System.Drawing.Color.Red;
            this.labelTip.Location = new System.Drawing.Point(13, 264);
            this.labelTip.Name = "labelTip";
            this.labelTip.Size = new System.Drawing.Size(367, 62);
            this.labelTip.TabIndex = 3;
            this.labelTip.Text = "Tip:";
            // 
            // textData
            // 
            this.textData.Location = new System.Drawing.Point(176, 232);
            this.textData.Name = "textData";
            this.textData.Size = new System.Drawing.Size(252, 23);
            this.textData.TabIndex = 4;
            // 
            // comboData
            // 
            this.comboData.FormattingEnabled = true;
            this.comboData.Location = new System.Drawing.Point(176, 232);
            this.comboData.Name = "comboData";
            this.comboData.Size = new System.Drawing.Size(252, 24);
            this.comboData.TabIndex = 5;
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(463, 275);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(55, 51);
            this.buttonNext.TabIndex = 6;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(402, 275);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(55, 51);
            this.buttonBack.TabIndex = 7;
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonImport
            // 
            this.buttonImport.AutoSize = true;
            this.buttonImport.Location = new System.Drawing.Point(23, 181);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(247, 30);
            this.buttonImport.TabIndex = 8;
            this.buttonImport.Text = "Import IceChat 7 Settings";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // FormFirstRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 336);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.textData);
            this.Controls.Add(this.labelTip);
            this.Controls.Add(this.labelField);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.comboData);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFirstRun";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IceChat First Run";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelField;
        private System.Windows.Forms.Label labelTip;
        private System.Windows.Forms.TextBox textData;
        private System.Windows.Forms.ComboBox comboData;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonImport;
    }
}