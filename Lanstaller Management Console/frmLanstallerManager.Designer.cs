
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.lblVariable = new System.Windows.Forms.Label();
            this.lblInstallInfo = new System.Windows.Forms.Label();
            this.gbxInfo = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnSecurity = new System.Windows.Forms.Button();
            this.tmrProgress = new System.Windows.Forms.Timer(this.components);
            this.lvSoftware = new System.Windows.Forms.ListView();
            this.gbxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Software List:";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(503, 39);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "Add New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(215, 16);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(69, 13);
            this.lblVariable.TabIndex = 20;
            this.lblVariable.Text = "Variable Info:";
            // 
            // lblInstallInfo
            // 
            this.lblInstallInfo.AutoSize = true;
            this.lblInstallInfo.Location = new System.Drawing.Point(6, 16);
            this.lblInstallInfo.Name = "lblInstallInfo";
            this.lblInstallInfo.Size = new System.Drawing.Size(81, 13);
            this.lblInstallInfo.TabIndex = 21;
            this.lblInstallInfo.Text = "Installation Info:";
            // 
            // gbxInfo
            // 
            this.gbxInfo.Controls.Add(this.lblVariable);
            this.gbxInfo.Controls.Add(this.lblInstallInfo);
            this.gbxInfo.Location = new System.Drawing.Point(584, 62);
            this.gbxInfo.Name = "gbxInfo";
            this.gbxInfo.Size = new System.Drawing.Size(754, 110);
            this.gbxInfo.TabIndex = 24;
            this.gbxInfo.TabStop = false;
            this.gbxInfo.Text = "Info";
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(422, 39);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 25;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnSecurity
            // 
            this.btnSecurity.Location = new System.Drawing.Point(8, 4);
            this.btnSecurity.Name = "btnSecurity";
            this.btnSecurity.Size = new System.Drawing.Size(109, 23);
            this.btnSecurity.TabIndex = 26;
            this.btnSecurity.Text = "Security";
            this.btnSecurity.UseVisualStyleBackColor = true;
            this.btnSecurity.Click += new System.EventHandler(this.btnSecurity_Click);
            // 
            // tmrProgress
            // 
            this.tmrProgress.Enabled = true;
            this.tmrProgress.Interval = 1000;
            this.tmrProgress.Tick += new System.EventHandler(this.tmrProgress_Tick);
            // 
            // lvSoftware
            // 
            this.lvSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSoftware.FullRowSelect = true;
            this.lvSoftware.GridLines = true;
            this.lvSoftware.HideSelection = false;
            this.lvSoftware.Location = new System.Drawing.Point(8, 68);
            this.lvSoftware.Name = "lvSoftware";
            this.lvSoftware.Size = new System.Drawing.Size(570, 606);
            this.lvSoftware.TabIndex = 27;
            this.lvSoftware.UseCompatibleStateImageBehavior = false;
            this.lvSoftware.View = System.Windows.Forms.View.Details;
            this.lvSoftware.SelectedIndexChanged += new System.EventHandler(this.lvSoftware_SelectedIndexChanged);
            // 
            // frmLanstallerManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 686);
            this.Controls.Add(this.lvSoftware);
            this.Controls.Add(this.btnSecurity);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.gbxInfo);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label1);
            this.Name = "frmLanstallerManager";
            this.Text = "Lanstaller Management";
            this.Load += new System.EventHandler(this.frmLanstallerMmanager_Load);
            this.gbxInfo.ResumeLayout(false);
            this.gbxInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

