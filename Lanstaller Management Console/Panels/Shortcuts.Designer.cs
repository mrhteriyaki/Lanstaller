namespace Lanstaller_Management_Console.Panels
{
    partial class Shortcuts
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddShortcut = new System.Windows.Forms.Button();
            this.txtIcon = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtWorking = new System.Windows.Forms.TextBox();
            this.txtFilepath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAddShortcut);
            this.groupBox3.Controls.Add(this.txtIcon);
            this.groupBox3.Controls.Add(this.txtArguments);
            this.groupBox3.Controls.Add(this.txtWorking);
            this.groupBox3.Controls.Add(this.txtFilepath);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtName);
            this.groupBox3.Controls.Add(this.txtLocation);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(358, 202);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shortcut";
            // 
            // btnAddShortcut
            // 
            this.btnAddShortcut.Location = new System.Drawing.Point(235, 162);
            this.btnAddShortcut.Name = "btnAddShortcut";
            this.btnAddShortcut.Size = new System.Drawing.Size(110, 31);
            this.btnAddShortcut.TabIndex = 20;
            this.btnAddShortcut.Text = "Add to Database";
            this.btnAddShortcut.UseVisualStyleBackColor = true;
            this.btnAddShortcut.Click += new System.EventHandler(this.btnAddShortcut_Click);
            // 
            // txtIcon
            // 
            this.txtIcon.Location = new System.Drawing.Point(108, 137);
            this.txtIcon.Name = "txtIcon";
            this.txtIcon.Size = new System.Drawing.Size(237, 20);
            this.txtIcon.TabIndex = 19;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(108, 113);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(237, 20);
            this.txtArguments.TabIndex = 18;
            // 
            // txtWorking
            // 
            this.txtWorking.Location = new System.Drawing.Point(108, 91);
            this.txtWorking.Name = "txtWorking";
            this.txtWorking.Size = new System.Drawing.Size(237, 20);
            this.txtWorking.TabIndex = 17;
            // 
            // txtFilepath
            // 
            this.txtFilepath.Location = new System.Drawing.Point(108, 68);
            this.txtFilepath.Name = "txtFilepath";
            this.txtFilepath.Size = new System.Drawing.Size(237, 20);
            this.txtFilepath.TabIndex = 16;
            this.txtFilepath.Text = "%INSTALLPATH%\\";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(71, 140);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Icon:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(42, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Arguments:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 13);
            this.label13.TabIndex = 13;
            this.label13.Text = "Working Path:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(55, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Filepath:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(64, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(108, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 20);
            this.txtName.TabIndex = 10;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(108, 45);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(237, 20);
            this.txtLocation.TabIndex = 1;
            this.txtLocation.Text = "C:\\Users\\Public\\Desktop";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Shortcut Location:";
            // 
            // Shortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Name = "Shortcuts";
            this.Size = new System.Drawing.Size(377, 218);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox txtIcon;
        public System.Windows.Forms.TextBox txtArguments;
        public System.Windows.Forms.TextBox txtWorking;
        public System.Windows.Forms.TextBox txtFilepath;
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.TextBox txtLocation;
        public System.Windows.Forms.Button btnAddShortcut;
    }
}
