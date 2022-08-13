namespace Lanstaller_Management_Console.Panels
{
    partial class Files
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCopyActionInfo = new System.Windows.Forms.Label();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtSubFolder = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServerShare = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.txtScanfolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGenerateNewFilehashes = new System.Windows.Forms.Button();
            this.btnRescanFileHash = new System.Windows.Forms.Button();
            this.btnRescanFileSize = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCopyActionInfo);
            this.groupBox1.Controls.Add(this.btnAddFolder);
            this.groupBox1.Controls.Add(this.btnScan);
            this.groupBox1.Controls.Add(this.txtSubFolder);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtServerShare);
            this.groupBox1.Controls.Add(this.txtDestination);
            this.groupBox1.Controls.Add(this.txtScanfolder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(752, 291);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files:";
            // 
            // lblCopyActionInfo
            // 
            this.lblCopyActionInfo.AutoSize = true;
            this.lblCopyActionInfo.Location = new System.Drawing.Point(6, 181);
            this.lblCopyActionInfo.Name = "lblCopyActionInfo";
            this.lblCopyActionInfo.Size = new System.Drawing.Size(89, 13);
            this.lblCopyActionInfo.TabIndex = 14;
            this.lblCopyActionInfo.Text = "lblCopyActionInfo";
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Enabled = false;
            this.btnAddFolder.Location = new System.Drawing.Point(646, 257);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(100, 28);
            this.btnAddFolder.TabIndex = 9;
            this.btnAddFolder.Text = "Add to Database";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            // 
            // btnScan
            // 
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(569, 257);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(71, 28);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            // 
            // txtSubFolder
            // 
            this.txtSubFolder.Location = new System.Drawing.Point(9, 119);
            this.txtSubFolder.Name = "txtSubFolder";
            this.txtSubFolder.Size = new System.Drawing.Size(737, 20);
            this.txtSubFolder.TabIndex = 12;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 103);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(275, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Subdirectory (Path under share that contains scan folder)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(206, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Server Share (root share location of scan):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Client destination override (Default is %INSTALLPATH%):";
            // 
            // txtServerShare
            // 
            this.txtServerShare.Location = new System.Drawing.Point(9, 78);
            this.txtServerShare.Name = "txtServerShare";
            this.txtServerShare.Size = new System.Drawing.Size(737, 20);
            this.txtServerShare.TabIndex = 10;
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(9, 158);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(737, 20);
            this.txtDestination.TabIndex = 7;
            // 
            // txtScanfolder
            // 
            this.txtScanfolder.Location = new System.Drawing.Point(9, 37);
            this.txtScanfolder.Name = "txtScanfolder";
            this.txtScanfolder.Size = new System.Drawing.Size(737, 20);
            this.txtScanfolder.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Scan Folder (Get Files from this directory):";
            // 
            // btnGenerateNewFilehashes
            // 
            this.btnGenerateNewFilehashes.Location = new System.Drawing.Point(3, 380);
            this.btnGenerateNewFilehashes.Name = "btnGenerateNewFilehashes";
            this.btnGenerateNewFilehashes.Size = new System.Drawing.Size(159, 34);
            this.btnGenerateNewFilehashes.TabIndex = 26;
            this.btnGenerateNewFilehashes.Text = "Generate File Hashes";
            this.btnGenerateNewFilehashes.UseVisualStyleBackColor = true;
            // 
            // btnRescanFileHash
            // 
            this.btnRescanFileHash.Location = new System.Drawing.Point(3, 340);
            this.btnRescanFileHash.Name = "btnRescanFileHash";
            this.btnRescanFileHash.Size = new System.Drawing.Size(159, 34);
            this.btnRescanFileHash.TabIndex = 25;
            this.btnRescanFileHash.Text = "Rescan hash for all files.";
            this.btnRescanFileHash.UseVisualStyleBackColor = true;
            // 
            // btnRescanFileSize
            // 
            this.btnRescanFileSize.Location = new System.Drawing.Point(3, 300);
            this.btnRescanFileSize.Name = "btnRescanFileSize";
            this.btnRescanFileSize.Size = new System.Drawing.Size(159, 34);
            this.btnRescanFileSize.TabIndex = 24;
            this.btnRescanFileSize.Text = "Rescan size for all files.";
            this.btnRescanFileSize.UseVisualStyleBackColor = true;
            // 
            // Files
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGenerateNewFilehashes);
            this.Controls.Add(this.btnRescanFileHash);
            this.Controls.Add(this.btnRescanFileSize);
            this.Controls.Add(this.groupBox1);
            this.Name = "Files";
            this.Size = new System.Drawing.Size(758, 419);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblCopyActionInfo;
        public System.Windows.Forms.TextBox txtSubFolder;
        public System.Windows.Forms.TextBox txtServerShare;
        public System.Windows.Forms.TextBox txtDestination;
        public System.Windows.Forms.TextBox txtScanfolder;
        public System.Windows.Forms.Button btnAddFolder;
        public System.Windows.Forms.Button btnScan;
        public System.Windows.Forms.Button btnGenerateNewFilehashes;
        public System.Windows.Forms.Button btnRescanFileHash;
        public System.Windows.Forms.Button btnRescanFileSize;
    }
}
