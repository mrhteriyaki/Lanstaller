namespace Lanstaller_Management_Console.Panels
{
    partial class Preferences
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnAddPrefFile = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.txtPrefFilePath = new System.Windows.Forms.TextBox();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtReplace);
            this.groupBox6.Controls.Add(this.txtTarget);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.btnAddPrefFile);
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.txtPrefFilePath);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(377, 163);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Preference Files (Replace text pattern)";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(75, 101);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(296, 20);
            this.txtReplace.TabIndex = 22;
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(75, 73);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(296, 20);
            this.txtTarget.TabIndex = 21;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(13, 104);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(50, 13);
            this.label23.TabIndex = 20;
            this.label23.Text = "Replace:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(22, 76);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 13);
            this.label24.TabIndex = 19;
            this.label24.Text = "Target:";
            // 
            // btnAddPrefFile
            // 
            this.btnAddPrefFile.Location = new System.Drawing.Point(271, 127);
            this.btnAddPrefFile.Name = "btnAddPrefFile";
            this.btnAddPrefFile.Size = new System.Drawing.Size(100, 28);
            this.btnAddPrefFile.TabIndex = 17;
            this.btnAddPrefFile.Text = "Add to Database";
            this.btnAddPrefFile.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 26);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(51, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "File Path:";
            // 
            // txtPrefFilePath
            // 
            this.txtPrefFilePath.Location = new System.Drawing.Point(75, 23);
            this.txtPrefFilePath.Name = "txtPrefFilePath";
            this.txtPrefFilePath.Size = new System.Drawing.Size(296, 20);
            this.txtPrefFilePath.TabIndex = 17;
            this.txtPrefFilePath.Text = "%INSTALLPATH%\\";
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Name = "Preferences";
            this.Size = new System.Drawing.Size(395, 173);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.Button btnAddPrefFile;
        public System.Windows.Forms.TextBox txtReplace;
        public System.Windows.Forms.TextBox txtTarget;
        public System.Windows.Forms.TextBox txtPrefFilePath;
    }
}
