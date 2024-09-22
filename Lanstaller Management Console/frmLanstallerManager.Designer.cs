
namespace Lanstaller_Management_Console
{
    partial class frmLanstallerManager
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
            components = new System.ComponentModel.Container();
            label1 = new System.Windows.Forms.Label();
            btnNew = new System.Windows.Forms.Button();
            lblVariable = new System.Windows.Forms.Label();
            lblInstallInfo = new System.Windows.Forms.Label();
            gbxInfo = new System.Windows.Forms.GroupBox();
            btnRemove = new System.Windows.Forms.Button();
            btnSecurity = new System.Windows.Forms.Button();
            tmrProgress = new System.Windows.Forms.Timer(components);
            lvSoftware = new System.Windows.Forms.ListView();
            btnSetImage = new System.Windows.Forms.Button();
            btnLLA = new System.Windows.Forms.Button();
            gbxInfo.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 51);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 15);
            label1.TabIndex = 1;
            label1.Text = "Software List:";
            // 
            // btnNew
            // 
            btnNew.Location = new System.Drawing.Point(587, 45);
            btnNew.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnNew.Name = "btnNew";
            btnNew.Size = new System.Drawing.Size(88, 27);
            btnNew.TabIndex = 2;
            btnNew.Text = "Add New";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // lblVariable
            // 
            lblVariable.AutoSize = true;
            lblVariable.Location = new System.Drawing.Point(251, 18);
            lblVariable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblVariable.Name = "lblVariable";
            lblVariable.Size = new System.Drawing.Size(75, 15);
            lblVariable.TabIndex = 20;
            lblVariable.Text = "Variable Info:";
            // 
            // lblInstallInfo
            // 
            lblInstallInfo.AutoSize = true;
            lblInstallInfo.Location = new System.Drawing.Point(7, 18);
            lblInstallInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblInstallInfo.Name = "lblInstallInfo";
            lblInstallInfo.Size = new System.Drawing.Size(92, 15);
            lblInstallInfo.TabIndex = 21;
            lblInstallInfo.Text = "Installation Info:";
            // 
            // gbxInfo
            // 
            gbxInfo.Controls.Add(lblVariable);
            gbxInfo.Controls.Add(lblInstallInfo);
            gbxInfo.Location = new System.Drawing.Point(681, 72);
            gbxInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbxInfo.Name = "gbxInfo";
            gbxInfo.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbxInfo.Size = new System.Drawing.Size(880, 127);
            gbxInfo.TabIndex = 24;
            gbxInfo.TabStop = false;
            gbxInfo.Text = "Info";
            // 
            // btnRemove
            // 
            btnRemove.Location = new System.Drawing.Point(492, 45);
            btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new System.Drawing.Size(88, 27);
            btnRemove.TabIndex = 25;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnSecurity
            // 
            btnSecurity.Location = new System.Drawing.Point(9, 5);
            btnSecurity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSecurity.Name = "btnSecurity";
            btnSecurity.Size = new System.Drawing.Size(127, 27);
            btnSecurity.TabIndex = 26;
            btnSecurity.Text = "Security";
            btnSecurity.UseVisualStyleBackColor = true;
            btnSecurity.Click += btnSecurity_Click;
            // 
            // tmrProgress
            // 
            tmrProgress.Enabled = true;
            tmrProgress.Interval = 1000;
            tmrProgress.Tick += tmrProgress_Tick;
            // 
            // lvSoftware
            // 
            lvSoftware.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lvSoftware.FullRowSelect = true;
            lvSoftware.GridLines = true;
            lvSoftware.Location = new System.Drawing.Point(9, 78);
            lvSoftware.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lvSoftware.Name = "lvSoftware";
            lvSoftware.Size = new System.Drawing.Size(664, 718);
            lvSoftware.TabIndex = 27;
            lvSoftware.UseCompatibleStateImageBehavior = false;
            lvSoftware.View = System.Windows.Forms.View.Details;
            lvSoftware.SelectedIndexChanged += lvSoftware_SelectedIndexChanged;
            // 
            // btnSetImage
            // 
            btnSetImage.Location = new System.Drawing.Point(361, 45);
            btnSetImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSetImage.Name = "btnSetImage";
            btnSetImage.Size = new System.Drawing.Size(105, 27);
            btnSetImage.TabIndex = 28;
            btnSetImage.Text = "Set Icon Image";
            btnSetImage.UseVisualStyleBackColor = true;
            btnSetImage.Click += btnSetImage_Click;
            // 
            // btnLLA
            // 
            btnLLA.Location = new System.Drawing.Point(234, 45);
            btnLLA.Name = "btnLLA";
            btnLLA.Size = new System.Drawing.Size(120, 27);
            btnLLA.TabIndex = 29;
            btnLLA.Text = "LLA Check";
            btnLLA.UseVisualStyleBackColor = true;
            btnLLA.Click += btnLLA_Click;
            // 
            // frmLanstallerManager
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1574, 811);
            Controls.Add(btnLLA);
            Controls.Add(btnSetImage);
            Controls.Add(lvSoftware);
            Controls.Add(btnSecurity);
            Controls.Add(btnRemove);
            Controls.Add(gbxInfo);
            Controls.Add(btnNew);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmLanstallerManager";
            Text = "Lanstaller Management";
            Load += frmLanstallerMmanager_Load;
            gbxInfo.ResumeLayout(false);
            gbxInfo.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.Label lblInstallInfo;
        private System.Windows.Forms.GroupBox gbxInfo;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnSecurity;
        private System.Windows.Forms.Timer tmrProgress;
        private System.Windows.Forms.ListView lvSoftware;
        private System.Windows.Forms.Button btnSetImage;
        private System.Windows.Forms.Button btnLLA;
    }
}

