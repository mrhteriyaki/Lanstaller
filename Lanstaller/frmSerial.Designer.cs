
namespace Lanstaller
{
    partial class frmSerial
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
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnPaste = new System.Windows.Forms.Button();
            this.cmbxServerSerials = new System.Windows.Forms.ComboBox();
            this.lblServerSerials = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSerial
            // 
            this.txtSerial.BackColor = System.Drawing.Color.White;
            this.txtSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.ForeColor = System.Drawing.Color.Black;
            this.txtSerial.Location = new System.Drawing.Point(12, 34);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(604, 38);
            this.txtSerial.TabIndex = 0;
            this.txtSerial.TextChanged += new System.EventHandler(this.txtSerial_TextChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(76, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Serial Number:";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(516, 78);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.BackColor = System.Drawing.Color.Black;
            this.btnPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPaste.ForeColor = System.Drawing.Color.White;
            this.btnPaste.Location = new System.Drawing.Point(410, 78);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(100, 30);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = false;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // cmbxServerSerials
            // 
            this.cmbxServerSerials.FormattingEnabled = true;
            this.cmbxServerSerials.Location = new System.Drawing.Point(166, 82);
            this.cmbxServerSerials.Margin = new System.Windows.Forms.Padding(2);
            this.cmbxServerSerials.Name = "cmbxServerSerials";
            this.cmbxServerSerials.Size = new System.Drawing.Size(238, 21);
            this.cmbxServerSerials.TabIndex = 4;
            this.cmbxServerSerials.SelectedIndexChanged += new System.EventHandler(this.cmbxServerSerials_SelectedIndexChanged);
            // 
            // lblServerSerials
            // 
            this.lblServerSerials.AutoSize = true;
            this.lblServerSerials.ForeColor = System.Drawing.Color.White;
            this.lblServerSerials.Location = new System.Drawing.Point(89, 85);
            this.lblServerSerials.Name = "lblServerSerials";
            this.lblServerSerials.Size = new System.Drawing.Size(75, 13);
            this.lblServerSerials.TabIndex = 5;
            this.lblServerSerials.Text = "Server Serials:";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(12, 78);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmSerial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(628, 120);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblServerSerials);
            this.Controls.Add(this.cmbxServerSerials);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtSerial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSerial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SerialForm";
            this.Load += new System.EventHandler(this.frmSerial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.TextBox txtSerial;
        public System.Windows.Forms.Button btnPaste;
        public System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Label lblServerSerials;
        private System.Windows.Forms.ComboBox cmbxServerSerials;
        public System.Windows.Forms.Button btnClose;
    }
}