namespace IceChat
{
    partial class FormEmoticons
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
            this.components = new System.ComponentModel.Container();
            this.imageListEmoticons = new System.Windows.Forms.ImageList(this.components);
            this.pictureEmoticons = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmoticons)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListEmoticons
            // 
            this.imageListEmoticons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListEmoticons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListEmoticons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pictureEmoticons
            // 
            this.pictureEmoticons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pictureEmoticons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEmoticons.Location = new System.Drawing.Point(0, 0);
            this.pictureEmoticons.Name = "pictureEmoticons";
            this.pictureEmoticons.Size = new System.Drawing.Size(471, 119);
            this.pictureEmoticons.TabIndex = 0;
            this.pictureEmoticons.TabStop = false;
            // 
            // FormEmoticons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 119);
            this.Controls.Add(this.pictureEmoticons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FormEmoticons";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Emoticon Picker";
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmoticons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListEmoticons;
        private System.Windows.Forms.PictureBox pictureEmoticons;
    }
}