namespace IceChat
{
    partial class FormDCCFileAccept
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDCCFileAccept));
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonReject = new System.Windows.Forms.Button();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonAccept
            // 
            this.buttonAccept.AutoSize = true;
            this.buttonAccept.Location = new System.Drawing.Point(12, 62);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonAccept.Size = new System.Drawing.Size(77, 26);
            this.buttonAccept.TabIndex = 0;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonReject
            // 
            this.buttonReject.AutoSize = true;
            this.buttonReject.Location = new System.Drawing.Point(93, 62);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(75, 26);
            this.buttonReject.TabIndex = 1;
            this.buttonReject.Text = "Reject";
            this.buttonReject.UseVisualStyleBackColor = true;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // buttonIgnore
            // 
            this.buttonIgnore.AutoSize = true;
            this.buttonIgnore.Location = new System.Drawing.Point(174, 62);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(76, 26);
            this.buttonIgnore.TabIndex = 2;
            this.buttonIgnore.Text = "Ignore";
            this.buttonIgnore.UseVisualStyleBackColor = true;
            this.buttonIgnore.Click += new System.EventHandler(this.buttonIgnore_Click);
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(13, 10);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(214, 16);
            this.labelUser.TabIndex = 3;
            this.labelUser.Text = "Nick is trying to send you a file";
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(12, 31);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(127, 16);
            this.labelFile.TabIndex = 4;
            this.labelFile.Text = "FileName (filesize)";
            // 
            // FormDCCFileAccept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 97);
            this.ControlBox = false;
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.buttonIgnore);
            this.Controls.Add(this.buttonReject);
            this.Controls.Add(this.buttonAccept);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDCCFileAccept";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DCC File Accept";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelFile;
    }
}