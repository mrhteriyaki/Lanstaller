
namespace Lanstaller
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.cmbxSoftware = new System.Windows.Forms.ComboBox();
            this.lbxInstallList = new System.Windows.Forms.ListBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtInstallDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gbxActions = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.chkShortcuts = new System.Windows.Forms.CheckBox();
            this.chkRegistry = new System.Windows.Forms.CheckBox();
            this.chkFiles = new System.Windows.Forms.CheckBox();
            this.gbxPref = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.gbxStatus = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtChatMessages = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtChatSendMessage = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOpenTool = new System.Windows.Forms.Button();
            this.cmbxTool = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.gbxActions.SuspendLayout();
            this.gbxPref.SuspendLayout();
            this.gbxStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbxSoftware
            // 
            this.cmbxSoftware.FormattingEnabled = true;
            this.cmbxSoftware.Location = new System.Drawing.Point(11, 181);
            this.cmbxSoftware.Name = "cmbxSoftware";
            this.cmbxSoftware.Size = new System.Drawing.Size(302, 21);
            this.cmbxSoftware.TabIndex = 0;
            this.cmbxSoftware.Text = "Select Game...";
            // 
            // lbxInstallList
            // 
            this.lbxInstallList.FormattingEnabled = true;
            this.lbxInstallList.Location = new System.Drawing.Point(11, 260);
            this.lbxInstallList.Name = "lbxInstallList";
            this.lbxInstallList.Size = new System.Drawing.Size(302, 134);
            this.lbxInstallList.TabIndex = 1;
            // 
            // btnInstall
            // 
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.Location = new System.Drawing.Point(446, 248);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(174, 94);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(11, 208);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(111, 26);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Queue Install";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(128, 208);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(111, 26);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear Queue";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtInstallDirectory
            // 
            this.txtInstallDirectory.Location = new System.Drawing.Point(11, 151);
            this.txtInstallDirectory.Name = "txtInstallDirectory";
            this.txtInstallDirectory.Size = new System.Drawing.Size(302, 20);
            this.txtInstallDirectory.TabIndex = 5;
            this.txtInstallDirectory.TextChanged += new System.EventHandler(this.txtInstallDirectory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Install Directory:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(11, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(905, 118);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // gbxActions
            // 
            this.gbxActions.Controls.Add(this.checkBox4);
            this.gbxActions.Controls.Add(this.chkShortcuts);
            this.gbxActions.Controls.Add(this.chkRegistry);
            this.gbxActions.Controls.Add(this.chkFiles);
            this.gbxActions.Location = new System.Drawing.Point(319, 242);
            this.gbxActions.Name = "gbxActions";
            this.gbxActions.Size = new System.Drawing.Size(121, 100);
            this.gbxActions.TabIndex = 8;
            this.gbxActions.TabStop = false;
            this.gbxActions.Text = "Actions:";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Enabled = false;
            this.checkBox4.Location = new System.Drawing.Point(5, 80);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(108, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "User Preferences";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // chkShortcuts
            // 
            this.chkShortcuts.AutoSize = true;
            this.chkShortcuts.Checked = true;
            this.chkShortcuts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShortcuts.Location = new System.Drawing.Point(5, 60);
            this.chkShortcuts.Name = "chkShortcuts";
            this.chkShortcuts.Size = new System.Drawing.Size(71, 17);
            this.chkShortcuts.TabIndex = 2;
            this.chkShortcuts.Text = "Shortcuts";
            this.chkShortcuts.UseVisualStyleBackColor = true;
            // 
            // chkRegistry
            // 
            this.chkRegistry.AutoSize = true;
            this.chkRegistry.Checked = true;
            this.chkRegistry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRegistry.Location = new System.Drawing.Point(5, 40);
            this.chkRegistry.Name = "chkRegistry";
            this.chkRegistry.Size = new System.Drawing.Size(64, 17);
            this.chkRegistry.TabIndex = 1;
            this.chkRegistry.Text = "Registry";
            this.chkRegistry.UseVisualStyleBackColor = true;
            // 
            // chkFiles
            // 
            this.chkFiles.AutoSize = true;
            this.chkFiles.Checked = true;
            this.chkFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFiles.Location = new System.Drawing.Point(5, 20);
            this.chkFiles.Name = "chkFiles";
            this.chkFiles.Size = new System.Drawing.Size(74, 17);
            this.chkFiles.TabIndex = 0;
            this.chkFiles.Text = "Copy Files";
            this.chkFiles.UseVisualStyleBackColor = true;
            // 
            // gbxPref
            // 
            this.gbxPref.Controls.Add(this.label5);
            this.gbxPref.Controls.Add(this.label4);
            this.gbxPref.Controls.Add(this.label3);
            this.gbxPref.Controls.Add(this.txtHeight);
            this.gbxPref.Controls.Add(this.txtWidth);
            this.gbxPref.Controls.Add(this.label2);
            this.gbxPref.Controls.Add(this.txtUsername);
            this.gbxPref.Location = new System.Drawing.Point(319, 136);
            this.gbxPref.Name = "gbxPref";
            this.gbxPref.Size = new System.Drawing.Size(301, 100);
            this.gbxPref.TabIndex = 9;
            this.gbxPref.TabStop = false;
            this.gbxPref.Text = "User Preferences";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(109, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Height:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Width:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Display Resolution:";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(156, 56);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(55, 20);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(48, 56);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(55, 20);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(66, 14);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(229, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // gbxStatus
            // 
            this.gbxStatus.Controls.Add(this.lblStatus);
            this.gbxStatus.Location = new System.Drawing.Point(319, 348);
            this.gbxStatus.Name = "gbxStatus";
            this.gbxStatus.Size = new System.Drawing.Size(301, 98);
            this.gbxStatus.TabIndex = 10;
            this.gbxStatus.TabStop = false;
            this.gbxStatus.Text = "Install Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(6, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(74, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Ready";
            // 
            // txtChatMessages
            // 
            this.txtChatMessages.Location = new System.Drawing.Point(6, 19);
            this.txtChatMessages.Multiline = true;
            this.txtChatMessages.Name = "txtChatMessages";
            this.txtChatMessages.ReadOnly = true;
            this.txtChatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMessages.Size = new System.Drawing.Size(278, 258);
            this.txtChatMessages.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtChatSendMessage);
            this.groupBox1.Controls.Add(this.txtChatMessages);
            this.groupBox1.Location = new System.Drawing.Point(626, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 312);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat";
            // 
            // txtChatSendMessage
            // 
            this.txtChatSendMessage.Location = new System.Drawing.Point(6, 283);
            this.txtChatSendMessage.Name = "txtChatSendMessage";
            this.txtChatSendMessage.Size = new System.Drawing.Size(278, 20);
            this.txtChatSendMessage.TabIndex = 12;
            this.txtChatSendMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtChatSendMessage_KeyUp);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnOpenTool);
            this.groupBox2.Controls.Add(this.cmbxTool);
            this.groupBox2.Location = new System.Drawing.Point(11, 398);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 48);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tools:";
            // 
            // btnOpenTool
            // 
            this.btnOpenTool.Location = new System.Drawing.Point(220, 17);
            this.btnOpenTool.Name = "btnOpenTool";
            this.btnOpenTool.Size = new System.Drawing.Size(75, 23);
            this.btnOpenTool.TabIndex = 1;
            this.btnOpenTool.Text = "Open";
            this.btnOpenTool.UseVisualStyleBackColor = true;
            this.btnOpenTool.Click += new System.EventHandler(this.btnOpenTool_Click);
            // 
            // cmbxTool
            // 
            this.cmbxTool.FormattingEnabled = true;
            this.cmbxTool.Location = new System.Drawing.Point(6, 19);
            this.cmbxTool.Name = "cmbxTool";
            this.cmbxTool.Size = new System.Drawing.Size(208, 21);
            this.cmbxTool.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Install Queue:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbxStatus);
            this.Controls.Add(this.gbxPref);
            this.Controls.Add(this.gbxActions);
            this.Controls.Add(this.lbxInstallList);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInstallDirectory);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.cmbxSoftware);
            this.Name = "Form1";
            this.Text = " Lanstaller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.gbxActions.ResumeLayout(false);
            this.gbxActions.PerformLayout();
            this.gbxPref.ResumeLayout(false);
            this.gbxPref.PerformLayout();
            this.gbxStatus.ResumeLayout(false);
            this.gbxStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbxSoftware;
        private System.Windows.Forms.ListBox lbxInstallList;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtInstallDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox gbxActions;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox chkShortcuts;
        private System.Windows.Forms.CheckBox chkRegistry;
        private System.Windows.Forms.CheckBox chkFiles;
        private System.Windows.Forms.GroupBox gbxPref;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.GroupBox gbxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtChatMessages;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtChatSendMessage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnOpenTool;
        private System.Windows.Forms.ComboBox cmbxTool;
        private System.Windows.Forms.Label label6;
    }
}

