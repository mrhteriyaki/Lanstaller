namespace Lanstaller_Management_Console.Panels
{
    partial class Windows_Settings
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.btnFirewallRuleAdd = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.txtFirewallPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbxRedist = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddRedist = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txtRuleName);
            this.groupBox4.Controls.Add(this.btnFirewallRuleAdd);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.txtFirewallPath);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 156);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Firewall Rule";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Rule name:";
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(75, 49);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(314, 20);
            this.txtRuleName.TabIndex = 19;
            // 
            // btnFirewallRuleAdd
            // 
            this.btnFirewallRuleAdd.Location = new System.Drawing.Point(289, 111);
            this.btnFirewallRuleAdd.Name = "btnFirewallRuleAdd";
            this.btnFirewallRuleAdd.Size = new System.Drawing.Size(100, 28);
            this.btnFirewallRuleAdd.TabIndex = 17;
            this.btnFirewallRuleAdd.Text = "Add to Database";
            this.btnFirewallRuleAdd.UseVisualStyleBackColor = true;
            this.btnFirewallRuleAdd.Click += new System.EventHandler(this.btnFirewallRuleAdd_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 13);
            this.label17.TabIndex = 18;
            this.label17.Text = "File Path:";
            // 
            // txtFirewallPath
            // 
            this.txtFirewallPath.Location = new System.Drawing.Point(75, 23);
            this.txtFirewallPath.Name = "txtFirewallPath";
            this.txtFirewallPath.Size = new System.Drawing.Size(314, 20);
            this.txtFirewallPath.TabIndex = 17;
            this.txtFirewallPath.Text = "%INSTALLPATH%\\";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbxRedist);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnAddRedist);
            this.groupBox1.Location = new System.Drawing.Point(3, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 88);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Redistributable Installers";
            // 
            // cmbxRedist
            // 
            this.cmbxRedist.FormattingEnabled = true;
            this.cmbxRedist.Location = new System.Drawing.Point(80, 19);
            this.cmbxRedist.Name = "cmbxRedist";
            this.cmbxRedist.Size = new System.Drawing.Size(314, 21);
            this.cmbxRedist.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Redist:";
            // 
            // btnAddRedist
            // 
            this.btnAddRedist.Location = new System.Drawing.Point(294, 46);
            this.btnAddRedist.Name = "btnAddRedist";
            this.btnAddRedist.Size = new System.Drawing.Size(100, 28);
            this.btnAddRedist.TabIndex = 17;
            this.btnAddRedist.Text = "Add to Database";
            this.btnAddRedist.UseVisualStyleBackColor = true;
            this.btnAddRedist.Click += new System.EventHandler(this.btnAddRedist_Click);
            // 
            // Windows_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Name = "Windows_Settings";
            this.Size = new System.Drawing.Size(414, 502);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox txtFirewallPath;
        public System.Windows.Forms.Button btnFirewallRuleAdd;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnAddRedist;
        public System.Windows.Forms.ComboBox cmbxRedist;
    }
}
