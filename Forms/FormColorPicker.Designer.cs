namespace IceChat
{
    partial class FormColorPicker
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
            this.panelColorPicker = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelColorPicker
            // 
            this.panelColorPicker.BackColor = System.Drawing.SystemColors.Control;
            this.panelColorPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.panelColorPicker.Location = new System.Drawing.Point(12, 2);
            this.panelColorPicker.Name = "panelColorPicker";
            this.panelColorPicker.Size = new System.Drawing.Size(221, 123);
            this.panelColorPicker.TabIndex = 21;
            // 
            // FormColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 131);
            this.Controls.Add(this.panelColorPicker);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormColorPicker";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Color Picker";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelColorPicker;
    }
}