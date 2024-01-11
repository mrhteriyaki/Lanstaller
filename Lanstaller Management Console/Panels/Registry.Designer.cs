namespace Lanstaller_Management_Console.Panels
{
    partial class Registry
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddReg = new System.Windows.Forms.Button();
            this.txtData = new System.Windows.Forms.TextBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.cmbxType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbxHiveKey = new System.Windows.Forms.ComboBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddReg);
            this.groupBox2.Controls.Add(this.txtData);
            this.groupBox2.Controls.Add(this.txtValue);
            this.groupBox2.Controls.Add(this.txtKey);
            this.groupBox2.Controls.Add(this.cmbxType);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbxHiveKey);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(308, 202);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registry";
            // 
            // btnAddReg
            // 
            this.btnAddReg.Location = new System.Drawing.Point(204, 165);
            this.btnAddReg.Name = "btnAddReg";
            this.btnAddReg.Size = new System.Drawing.Size(98, 31);
            this.btnAddReg.TabIndex = 8;
            this.btnAddReg.Text = "Add to Database";
            this.btnAddReg.UseVisualStyleBackColor = true;
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(124, 96);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(178, 20);
            this.txtData.TabIndex = 9;
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(79, 70);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(223, 20);
            this.txtValue.TabIndex = 8;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(65, 42);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(237, 20);
            this.txtKey.TabIndex = 7;
            // 
            // cmbxType
            // 
            this.cmbxType.FormattingEnabled = true;
            this.cmbxType.Items.AddRange(new object[] {
            "STRING",
            "DWORD"});
            this.cmbxType.Location = new System.Drawing.Point(152, 122);
            this.cmbxType.Name = "cmbxType";
            this.cmbxType.Size = new System.Drawing.Size(150, 21);
            this.cmbxType.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(112, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Data (DEC for Dwords):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Name / Value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Key:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Hive Key:";
            // 
            // cmbxHiveKey
            // 
            this.cmbxHiveKey.FormattingEnabled = true;
            this.cmbxHiveKey.Items.AddRange(new object[] {
            "HKEY_LOCAL_MACHINE",
            "HKEY_CURRENT_USER",
            "HKEY_USERS"});
            this.cmbxHiveKey.Location = new System.Drawing.Point(128, 13);
            this.cmbxHiveKey.Name = "cmbxHiveKey";
            this.cmbxHiveKey.Size = new System.Drawing.Size(174, 21);
            this.cmbxHiveKey.TabIndex = 0;
            // 
            // Registry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "Registry";
            this.Size = new System.Drawing.Size(323, 211);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtData;
        public System.Windows.Forms.TextBox txtValue;
        public System.Windows.Forms.TextBox txtKey;
        public System.Windows.Forms.ComboBox cmbxType;
        public System.Windows.Forms.ComboBox cmbxHiveKey;
        public System.Windows.Forms.Button btnAddReg;
    }
}
