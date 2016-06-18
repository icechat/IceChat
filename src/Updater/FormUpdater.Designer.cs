namespace IceChatUpdater
{
    partial class FormUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUpdater));
            this.labelCurrent = new System.Windows.Forms.Label();
            this.labelLatest = new System.Windows.Forms.Label();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFolder = new System.Windows.Forms.Label();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.labelUpdate = new System.Windows.Forms.Label();
            this.labelNoUpdate = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelCurrentFile = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelFramework = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCurrent
            // 
            this.labelCurrent.AutoSize = true;
            this.labelCurrent.Location = new System.Drawing.Point(13, 9);
            this.labelCurrent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(116, 16);
            this.labelCurrent.TabIndex = 0;
            this.labelCurrent.Text = "Current Version:";
            // 
            // labelLatest
            // 
            this.labelLatest.AutoSize = true;
            this.labelLatest.Location = new System.Drawing.Point(13, 37);
            this.labelLatest.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLatest.Name = "labelLatest";
            this.labelLatest.Size = new System.Drawing.Size(109, 16);
            this.labelLatest.TabIndex = 1;
            this.labelLatest.Text = "Latest Version:";
            // 
            // listFiles
            // 
            this.listFiles.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listFiles.FormattingEnabled = true;
            this.listFiles.Location = new System.Drawing.Point(16, 82);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(386, 134);
            this.listFiles.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Files to Update:";
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.Location = new System.Drawing.Point(13, 299);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(46, 16);
            this.labelFolder.TabIndex = 4;
            this.labelFolder.Text = "label2";
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(408, 124);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(173, 23);
            this.buttonDownload.TabIndex = 5;
            this.buttonDownload.Text = "Download Updates";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Visible = false;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // labelUpdate
            // 
            this.labelUpdate.AutoSize = true;
            this.labelUpdate.ForeColor = System.Drawing.Color.Red;
            this.labelUpdate.Location = new System.Drawing.Point(434, 96);
            this.labelUpdate.Name = "labelUpdate";
            this.labelUpdate.Size = new System.Drawing.Size(125, 16);
            this.labelUpdate.TabIndex = 6;
            this.labelUpdate.Text = "Updates Available";
            this.labelUpdate.Visible = false;
            // 
            // labelNoUpdate
            // 
            this.labelNoUpdate.AutoSize = true;
            this.labelNoUpdate.ForeColor = System.Drawing.Color.Red;
            this.labelNoUpdate.Location = new System.Drawing.Point(364, 9);
            this.labelNoUpdate.Name = "labelNoUpdate";
            this.labelNoUpdate.Size = new System.Drawing.Size(217, 16);
            this.labelNoUpdate.TabIndex = 7;
            this.labelNoUpdate.Text = "Your are running Latest Version";
            this.labelNoUpdate.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(16, 226);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(386, 23);
            this.progressBar.TabIndex = 8;
            this.progressBar.Visible = false;
            // 
            // labelCurrentFile
            // 
            this.labelCurrentFile.AutoSize = true;
            this.labelCurrentFile.Location = new System.Drawing.Point(13, 263);
            this.labelCurrentFile.Name = "labelCurrentFile";
            this.labelCurrentFile.Size = new System.Drawing.Size(90, 16);
            this.labelCurrentFile.TabIndex = 9;
            this.labelCurrentFile.Text = "Current File:";
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(408, 233);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(35, 16);
            this.labelSize.TabIndex = 10;
            this.labelSize.Text = "Size";
            this.labelSize.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 338);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(337, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "PLEASE CLOSE ICECHAT BEFORE YOU UPDATE";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 281);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "label3";
            // 
            // labelFramework
            // 
            this.labelFramework.AutoSize = true;
            this.labelFramework.Location = new System.Drawing.Point(13, 318);
            this.labelFramework.Name = "labelFramework";
            this.labelFramework.Size = new System.Drawing.Size(79, 16);
            this.labelFramework.TabIndex = 13;
            this.labelFramework.Text = "Framework";
            // 
            // FormUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(593, 363);
            this.Controls.Add(this.labelFramework);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelCurrentFile);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelNoUpdate);
            this.Controls.Add(this.labelUpdate);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.labelFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.labelLatest);
            this.Controls.Add(this.labelCurrent);
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IceChat 9 Updater v2.01";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label labelLatest;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Label labelUpdate;
        private System.Windows.Forms.Label labelNoUpdate;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelCurrentFile;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelFramework;
    }
}

