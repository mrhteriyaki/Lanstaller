namespace Lanstaller_Management_Console
{
    partial class frmSecurity
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
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAuthDelete = new System.Windows.Forms.Button();
            this.lbxAuth = new System.Windows.Forms.ListBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRegDel = new System.Windows.Forms.Button();
            this.btnRegAdd = new System.Windows.Forms.Button();
            this.lbxRegCodes = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRegName = new System.Windows.Forms.TextBox();
            this.txtRegCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtExpiry = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(350, 339);
            this.txtToken.Multiline = true;
            this.txtToken.Name = "txtToken";
            this.txtToken.ReadOnly = true;
            this.txtToken.Size = new System.Drawing.Size(176, 210);
            this.txtToken.TabIndex = 39;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 297);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Security Tokens:";
            // 
            // btnAuthDelete
            // 
            this.btnAuthDelete.Location = new System.Drawing.Point(213, 287);
            this.btnAuthDelete.Name = "btnAuthDelete";
            this.btnAuthDelete.Size = new System.Drawing.Size(75, 23);
            this.btnAuthDelete.TabIndex = 37;
            this.btnAuthDelete.Text = "Delete";
            this.btnAuthDelete.UseVisualStyleBackColor = true;
            this.btnAuthDelete.Click += new System.EventHandler(this.btnAuthDelete_Click);
            // 
            // lbxAuth
            // 
            this.lbxAuth.FormattingEnabled = true;
            this.lbxAuth.Location = new System.Drawing.Point(10, 313);
            this.lbxAuth.Name = "lbxAuth";
            this.lbxAuth.Size = new System.Drawing.Size(278, 238);
            this.lbxAuth.TabIndex = 35;
            this.lbxAuth.SelectedIndexChanged += new System.EventHandler(this.lbxAuth_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(350, 313);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(176, 20);
            this.txtName.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(306, 316);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(303, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Token:";
            // 
            // btnRegDel
            // 
            this.btnRegDel.Location = new System.Drawing.Point(132, 11);
            this.btnRegDel.Name = "btnRegDel";
            this.btnRegDel.Size = new System.Drawing.Size(75, 23);
            this.btnRegDel.TabIndex = 45;
            this.btnRegDel.Text = "Delete";
            this.btnRegDel.UseVisualStyleBackColor = true;
            this.btnRegDel.Click += new System.EventHandler(this.btnRegDel_Click);
            // 
            // btnRegAdd
            // 
            this.btnRegAdd.Location = new System.Drawing.Point(213, 11);
            this.btnRegAdd.Name = "btnRegAdd";
            this.btnRegAdd.Size = new System.Drawing.Size(75, 23);
            this.btnRegAdd.TabIndex = 44;
            this.btnRegAdd.Text = "Add New";
            this.btnRegAdd.UseVisualStyleBackColor = true;
            this.btnRegAdd.Click += new System.EventHandler(this.btnRegAdd_Click);
            // 
            // lbxRegCodes
            // 
            this.lbxRegCodes.FormattingEnabled = true;
            this.lbxRegCodes.Location = new System.Drawing.Point(10, 40);
            this.lbxRegCodes.Name = "lbxRegCodes";
            this.lbxRegCodes.Size = new System.Drawing.Size(278, 238);
            this.lbxRegCodes.TabIndex = 43;
            this.lbxRegCodes.SelectedIndexChanged += new System.EventHandler(this.lbxRegCodes_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Registration Codes:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Reg code:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(316, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "Name:";
            // 
            // txtRegName
            // 
            this.txtRegName.Location = new System.Drawing.Point(360, 40);
            this.txtRegName.Name = "txtRegName";
            this.txtRegName.ReadOnly = true;
            this.txtRegName.Size = new System.Drawing.Size(166, 20);
            this.txtRegName.TabIndex = 48;
            // 
            // txtRegCode
            // 
            this.txtRegCode.Location = new System.Drawing.Point(360, 66);
            this.txtRegCode.Name = "txtRegCode";
            this.txtRegCode.ReadOnly = true;
            this.txtRegCode.Size = new System.Drawing.Size(166, 20);
            this.txtRegCode.TabIndex = 47;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(313, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "Expiry:";
            // 
            // txtExpiry
            // 
            this.txtExpiry.Location = new System.Drawing.Point(360, 92);
            this.txtExpiry.Name = "txtExpiry";
            this.txtExpiry.ReadOnly = true;
            this.txtExpiry.Size = new System.Drawing.Size(166, 20);
            this.txtExpiry.TabIndex = 51;
            // 
            // frmSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 558);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtExpiry);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRegName);
            this.Controls.Add(this.txtRegCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnRegDel);
            this.Controls.Add(this.btnRegAdd);
            this.Controls.Add(this.lbxRegCodes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtToken);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAuthDelete);
            this.Controls.Add(this.lbxAuth);
            this.Name = "frmSecurity";
            this.Text = "frmSecurity";
            this.Load += new System.EventHandler(this.frmSecurity_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAuthDelete;
        private System.Windows.Forms.ListBox lbxAuth;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRegDel;
        private System.Windows.Forms.Button btnRegAdd;
        private System.Windows.Forms.ListBox lbxRegCodes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRegName;
        private System.Windows.Forms.TextBox txtRegCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtExpiry;
    }
}