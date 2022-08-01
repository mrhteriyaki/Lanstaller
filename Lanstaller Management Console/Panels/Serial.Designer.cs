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
            this.groupBox5.SuspendLayout();
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
            this.groupBox5.Size = new System.Drawing.Size(400, 137);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Serial Numbers";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(59, 70);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(105, 13);
            this.label21.TabIndex = 24;
            this.label21.Text = "Lookup Registry Val:";
            // 
            // txtRegVal
            // 
            this.txtRegVal.Location = new System.Drawing.Point(170, 67);
            this.txtRegVal.Name = "txtRegVal";
            this.txtRegVal.Size = new System.Drawing.Size(224, 20);
            this.txtRegVal.TabIndex = 23;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(59, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 13);
            this.label20.TabIndex = 22;
            this.label20.Text = "Lookup Registry Key:";
            // 
            // txtRegKey
            // 
            this.txtRegKey.Location = new System.Drawing.Point(170, 41);
            this.txtRegKey.Name = "txtRegKey";
            this.txtRegKey.Size = new System.Drawing.Size(224, 20);
            this.txtRegKey.TabIndex = 21;
            // 
            // btnAddSerial
            // 
            this.btnAddSerial.Enabled = false;
            this.btnAddSerial.Location = new System.Drawing.Point(294, 101);
            this.btnAddSerial.Name = "btnAddSerial";
            this.btnAddSerial.Size = new System.Drawing.Size(100, 28);
            this.btnAddSerial.TabIndex = 19;
            this.btnAddSerial.Text = "Add to Database";
            this.btnAddSerial.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(74, 96);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(91, 13);
            this.label19.TabIndex = 20;
            this.label19.Text = "Instance Number:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(129, 18);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(38, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "Name:";
            // 
            // txtSerialInstance
            // 
            this.txtSerialInstance.Location = new System.Drawing.Point(170, 93);
            this.txtSerialInstance.Name = "txtSerialInstance";
            this.txtSerialInstance.Size = new System.Drawing.Size(44, 20);
            this.txtSerialInstance.TabIndex = 11;
            this.txtSerialInstance.Text = "1";
            // 
            // txtSerialName
            // 
            this.txtSerialName.Location = new System.Drawing.Point(170, 15);
            this.txtSerialName.Name = "txtSerialName";
            this.txtSerialName.Size = new System.Drawing.Size(224, 20);
            this.txtSerialName.TabIndex = 10;
            // 
            // Serial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Name = "Serial";
            this.Size = new System.Drawing.Size(419, 150);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
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
    }
}
