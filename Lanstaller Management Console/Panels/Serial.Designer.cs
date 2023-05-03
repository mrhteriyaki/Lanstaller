namespace Lanstaller_Management_Console.Panels
{
    partial class Serial
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtRegVal = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtRegKey = new System.Windows.Forms.TextBox();
            this.btnAddSerial = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSerialInstance = new System.Windows.Forms.TextBox();
            this.txtSerialName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserSerial = new System.Windows.Forms.TextBox();
            this.btnAddUserSerial = new System.Windows.Forms.Button();
            this.btnDelUserSerial = new System.Windows.Forms.Button();
            this.lvUserSerials = new System.Windows.Forms.ListView();
            this.cmbxSerials = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label21);
            this.groupBox5.Controls.Add(this.txtRegVal);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.txtRegKey);
            this.groupBox5.Controls.Add(this.btnAddSerial);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.txtSerialInstance);
            this.groupBox5.Controls.Add(this.txtSerialName);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(353, 137);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Serial Numbers";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(7, 72);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(105, 13);
            this.label21.TabIndex = 24;
            this.label21.Text = "Lookup Registry Val:";
            // 
            // txtRegVal
            // 
            this.txtRegVal.Location = new System.Drawing.Point(118, 69);
            this.txtRegVal.Name = "txtRegVal";
            this.txtRegVal.Size = new System.Drawing.Size(224, 20);
            this.txtRegVal.TabIndex = 23;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(7, 50);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 13);
            this.label20.TabIndex = 22;
            this.label20.Text = "Lookup Registry Key:";
            // 
            // txtRegKey
            // 
            this.txtRegKey.Location = new System.Drawing.Point(118, 43);
            this.txtRegKey.Name = "txtRegKey";
            this.txtRegKey.Size = new System.Drawing.Size(224, 20);
            this.txtRegKey.TabIndex = 21;
            // 
            // btnAddSerial
            // 
            this.btnAddSerial.Enabled = false;
            this.btnAddSerial.Location = new System.Drawing.Point(242, 103);
            this.btnAddSerial.Name = "btnAddSerial";
            this.btnAddSerial.Size = new System.Drawing.Size(100, 28);
            this.btnAddSerial.TabIndex = 19;
            this.btnAddSerial.Text = "Add to Database";
            this.btnAddSerial.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(22, 98);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(91, 13);
            this.label19.TabIndex = 20;
            this.label19.Text = "Instance Number:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(77, 20);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(38, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "Name:";
            // 
            // txtSerialInstance
            // 
            this.txtSerialInstance.Location = new System.Drawing.Point(118, 95);
            this.txtSerialInstance.Name = "txtSerialInstance";
            this.txtSerialInstance.Size = new System.Drawing.Size(44, 20);
            this.txtSerialInstance.TabIndex = 11;
            this.txtSerialInstance.Text = "1";
            // 
            // txtSerialName
            // 
            this.txtSerialName.Location = new System.Drawing.Point(118, 17);
            this.txtSerialName.Name = "txtSerialName";
            this.txtSerialName.Size = new System.Drawing.Size(224, 20);
            this.txtSerialName.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbxSerials);
            this.groupBox1.Controls.Add(this.lvUserSerials);
            this.groupBox1.Controls.Add(this.btnDelUserSerial);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtUserSerial);
            this.groupBox1.Controls.Add(this.btnAddUserSerial);
            this.groupBox1.Location = new System.Drawing.Point(3, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 301);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Pool for Users";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Instance:";
            // 
            // txtUserSerial
            // 
            this.txtUserSerial.Location = new System.Drawing.Point(49, 48);
            this.txtUserSerial.Name = "txtUserSerial";
            this.txtUserSerial.Size = new System.Drawing.Size(177, 20);
            this.txtUserSerial.TabIndex = 26;
            // 
            // btnAddUserSerial
            // 
            this.btnAddUserSerial.Enabled = false;
            this.btnAddUserSerial.Location = new System.Drawing.Point(232, 43);
            this.btnAddUserSerial.Name = "btnAddUserSerial";
            this.btnAddUserSerial.Size = new System.Drawing.Size(59, 28);
            this.btnAddUserSerial.TabIndex = 25;
            this.btnAddUserSerial.Text = "Add";
            this.btnAddUserSerial.UseVisualStyleBackColor = true;
            // 
            // btnDelUserSerial
            // 
            this.btnDelUserSerial.Enabled = false;
            this.btnDelUserSerial.Location = new System.Drawing.Point(294, 43);
            this.btnDelUserSerial.Name = "btnDelUserSerial";
            this.btnDelUserSerial.Size = new System.Drawing.Size(53, 28);
            this.btnDelUserSerial.TabIndex = 28;
            this.btnDelUserSerial.Text = "Delete";
            this.btnDelUserSerial.UseVisualStyleBackColor = true;
            // 
            // lvUserSerials
            // 
            this.lvUserSerials.FullRowSelect = true;
            this.lvUserSerials.GridLines = true;
            this.lvUserSerials.HideSelection = false;
            this.lvUserSerials.Location = new System.Drawing.Point(6, 77);
            this.lvUserSerials.Name = "lvUserSerials";
            this.lvUserSerials.Size = new System.Drawing.Size(341, 210);
            this.lvUserSerials.TabIndex = 29;
            this.lvUserSerials.UseCompatibleStateImageBehavior = false;
            this.lvUserSerials.View = System.Windows.Forms.View.Details;
            // 
            // cmbxSerials
            // 
            this.cmbxSerials.FormattingEnabled = true;
            this.cmbxSerials.Location = new System.Drawing.Point(64, 19);
            this.cmbxSerials.Name = "cmbxSerials";
            this.cmbxSerials.Size = new System.Drawing.Size(283, 21);
            this.cmbxSerials.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Serial:";
            // 
            // Serial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox5);
            this.Name = "Serial";
            this.Size = new System.Drawing.Size(365, 454);
            this.Load += new System.EventHandler(this.Serial_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.TextBox txtRegVal;
        public System.Windows.Forms.TextBox txtRegKey;
        public System.Windows.Forms.TextBox txtSerialInstance;
        public System.Windows.Forms.TextBox txtSerialName;
        public System.Windows.Forms.Button btnAddSerial;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button btnDelUserSerial;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtUserSerial;
        public System.Windows.Forms.Button btnAddUserSerial;
        public System.Windows.Forms.ListView lvUserSerials;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cmbxSerials;
    }
}
