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
            this.btnFirewallRuleAdd = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.txtFirewallPath = new System.Windows.Forms.TextBox();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnFirewallRuleAdd);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.txtFirewallPath);
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 95);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Firewall Rule";
            // 
            // btnFirewallRuleAdd
            // 
            this.btnFirewallRuleAdd.Location = new System.Drawing.Point(289, 49);
            this.btnFirewallRuleAdd.Name = "btnFirewallRuleAdd";
            this.btnFirewallRuleAdd.Size = new System.Drawing.Size(100, 28);
            this.btnFirewallRuleAdd.TabIndex = 17;
            this.btnFirewallRuleAdd.Text = "Add to Database";
            this.btnFirewallRuleAdd.UseVisualStyleBackColor = true;
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
            // Windows_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Name = "Windows_Settings";
            this.Size = new System.Drawing.Size(414, 107);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox txtFirewallPath;
        public System.Windows.Forms.Button btnFirewallRuleAdd;
    }
}
