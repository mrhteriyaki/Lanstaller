
namespace Lanstaller
{
    partial class frmLanstaller
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
            this.cmbxSoftware = new System.Windows.Forms.ComboBox();
            this.lbxInstallList = new System.Windows.Forms.ListBox();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtInstallDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxActions = new System.Windows.Forms.GroupBox();
            this.chkRedist = new System.Windows.Forms.CheckBox();
            this.chkWindowsSettings = new System.Windows.Forms.CheckBox();
            this.chkPreferences = new System.Windows.Forms.CheckBox();
            this.chkShortcuts = new System.Windows.Forms.CheckBox();
            this.chkRegistry = new System.Windows.Forms.CheckBox();
            this.chkFiles = new System.Windows.Forms.CheckBox();
            this.gbxPref = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.gbxStatus = new System.Windows.Forms.GroupBox();
            this.pbInstall = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txtChatMessages = new System.Windows.Forms.TextBox();
            this.txtChatSendMessage = new System.Windows.Forms.TextBox();
            this.btnOpenTool = new System.Windows.Forms.Button();
            this.cmbxTool = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSpaceRequired = new System.Windows.Forms.Label();
            this.pbTitleExpanded = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.pbTitle = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.gbxActions.SuspendLayout();
            this.gbxPref.SuspendLayout();
            this.gbxStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitleExpanded)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbxSoftware
            // 
            this.cmbxSoftware.BackColor = System.Drawing.Color.Black;
            this.cmbxSoftware.ForeColor = System.Drawing.Color.White;
            this.cmbxSoftware.FormattingEnabled = true;
            this.cmbxSoftware.Location = new System.Drawing.Point(11, 139);
            this.cmbxSoftware.Name = "cmbxSoftware";
            this.cmbxSoftware.Size = new System.Drawing.Size(302, 21);
            this.cmbxSoftware.TabIndex = 0;
            this.cmbxSoftware.Text = "Select Game...";
            this.cmbxSoftware.SelectedIndexChanged += new System.EventHandler(this.cmbxSoftware_SelectedIndexChanged);
            // 
            // lbxInstallList
            // 
            this.lbxInstallList.BackColor = System.Drawing.Color.Black;
            this.lbxInstallList.ForeColor = System.Drawing.Color.White;
            this.lbxInstallList.FormattingEnabled = true;
            this.lbxInstallList.Location = new System.Drawing.Point(11, 235);
            this.lbxInstallList.Name = "lbxInstallList";
            this.lbxInstallList.Size = new System.Drawing.Size(302, 147);
            this.lbxInstallList.TabIndex = 1;
            // 
            // btnInstall
            // 
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(446, 189);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(174, 139);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(11, 183);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(111, 26);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Queue Install";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnClear
            // 
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(128, 183);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(111, 26);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear Queue";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtInstallDirectory
            // 
            this.txtInstallDirectory.BackColor = System.Drawing.Color.Black;
            this.txtInstallDirectory.ForeColor = System.Drawing.Color.White;
            this.txtInstallDirectory.Location = new System.Drawing.Point(11, 109);
            this.txtInstallDirectory.Name = "txtInstallDirectory";
            this.txtInstallDirectory.Size = new System.Drawing.Size(302, 20);
            this.txtInstallDirectory.TabIndex = 5;
            this.txtInstallDirectory.TextChanged += new System.EventHandler(this.txtInstallDirectory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Install Directory:";
            // 
            // gbxActions
            // 
            this.gbxActions.Controls.Add(this.chkRedist);
            this.gbxActions.Controls.Add(this.chkWindowsSettings);
            this.gbxActions.Controls.Add(this.chkPreferences);
            this.gbxActions.Controls.Add(this.chkShortcuts);
            this.gbxActions.Controls.Add(this.chkRegistry);
            this.gbxActions.Controls.Add(this.chkFiles);
            this.gbxActions.ForeColor = System.Drawing.Color.White;
            this.gbxActions.Location = new System.Drawing.Point(319, 183);
            this.gbxActions.Name = "gbxActions";
            this.gbxActions.Size = new System.Drawing.Size(121, 146);
            this.gbxActions.TabIndex = 8;
            this.gbxActions.TabStop = false;
            this.gbxActions.Text = "Actions:";
            // 
            // chkRedist
            // 
            this.chkRedist.AutoSize = true;
            this.chkRedist.Checked = true;
            this.chkRedist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRedist.ForeColor = System.Drawing.Color.White;
            this.chkRedist.Location = new System.Drawing.Point(5, 120);
            this.chkRedist.Name = "chkRedist";
            this.chkRedist.Size = new System.Drawing.Size(101, 17);
            this.chkRedist.TabIndex = 5;
            this.chkRedist.Text = "Redistributables";
            this.chkRedist.UseVisualStyleBackColor = true;
            // 
            // chkWindowsSettings
            // 
            this.chkWindowsSettings.AutoSize = true;
            this.chkWindowsSettings.Checked = true;
            this.chkWindowsSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsSettings.ForeColor = System.Drawing.Color.White;
            this.chkWindowsSettings.Location = new System.Drawing.Point(5, 100);
            this.chkWindowsSettings.Name = "chkWindowsSettings";
            this.chkWindowsSettings.Size = new System.Drawing.Size(111, 17);
            this.chkWindowsSettings.TabIndex = 4;
            this.chkWindowsSettings.Text = "Windows Settings";
            this.chkWindowsSettings.UseVisualStyleBackColor = true;
            // 
            // chkPreferences
            // 
            this.chkPreferences.AutoSize = true;
            this.chkPreferences.Checked = true;
            this.chkPreferences.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreferences.ForeColor = System.Drawing.Color.White;
            this.chkPreferences.Location = new System.Drawing.Point(5, 80);
            this.chkPreferences.Name = "chkPreferences";
            this.chkPreferences.Size = new System.Drawing.Size(108, 17);
            this.chkPreferences.TabIndex = 3;
            this.chkPreferences.Text = "User Preferences";
            this.chkPreferences.UseVisualStyleBackColor = true;
            // 
            // chkShortcuts
            // 
            this.chkShortcuts.AutoSize = true;
            this.chkShortcuts.Checked = true;
            this.chkShortcuts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShortcuts.ForeColor = System.Drawing.Color.White;
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
            this.chkRegistry.ForeColor = System.Drawing.Color.White;
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
            this.chkFiles.ForeColor = System.Drawing.Color.White;
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
            this.gbxPref.Controls.Add(this.label3);
            this.gbxPref.Controls.Add(this.txtHeight);
            this.gbxPref.Controls.Add(this.txtWidth);
            this.gbxPref.Controls.Add(this.label2);
            this.gbxPref.Controls.Add(this.txtUsername);
            this.gbxPref.ForeColor = System.Drawing.Color.White;
            this.gbxPref.Location = new System.Drawing.Point(319, 103);
            this.gbxPref.Name = "gbxPref";
            this.gbxPref.Size = new System.Drawing.Size(301, 74);
            this.gbxPref.TabIndex = 9;
            this.gbxPref.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(183, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(24, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Display Resolution:";
            // 
            // txtHeight
            // 
            this.txtHeight.BackColor = System.Drawing.Color.Black;
            this.txtHeight.ForeColor = System.Drawing.Color.White;
            this.txtHeight.Location = new System.Drawing.Point(197, 45);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(55, 20);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.BackColor = System.Drawing.Color.Black;
            this.txtWidth.ForeColor = System.Drawing.Color.White;
            this.txtWidth.Location = new System.Drawing.Point(127, 45);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(55, 20);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(63, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.Black;
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location = new System.Drawing.Point(127, 19);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(125, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // gbxStatus
            // 
            this.gbxStatus.Controls.Add(this.pbInstall);
            this.gbxStatus.Controls.Add(this.lblStatus);
            this.gbxStatus.ForeColor = System.Drawing.Color.White;
            this.gbxStatus.Location = new System.Drawing.Point(319, 331);
            this.gbxStatus.Name = "gbxStatus";
            this.gbxStatus.Size = new System.Drawing.Size(301, 90);
            this.gbxStatus.TabIndex = 10;
            this.gbxStatus.TabStop = false;
            this.gbxStatus.Text = "Install Status:";
            // 
            // pbInstall
            // 
            this.pbInstall.Enabled = false;
            this.pbInstall.Location = new System.Drawing.Point(5, 54);
            this.pbInstall.Name = "pbInstall";
            this.pbInstall.Size = new System.Drawing.Size(289, 30);
            this.pbInstall.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(5, 13);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(74, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Ready";
            // 
            // txtChatMessages
            // 
            this.txtChatMessages.BackColor = System.Drawing.Color.Black;
            this.txtChatMessages.ForeColor = System.Drawing.Color.White;
            this.txtChatMessages.Location = new System.Drawing.Point(626, 108);
            this.txtChatMessages.Multiline = true;
            this.txtChatMessages.Name = "txtChatMessages";
            this.txtChatMessages.ReadOnly = true;
            this.txtChatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMessages.Size = new System.Drawing.Size(288, 286);
            this.txtChatMessages.TabIndex = 11;
            // 
            // txtChatSendMessage
            // 
            this.txtChatSendMessage.BackColor = System.Drawing.Color.Black;
            this.txtChatSendMessage.ForeColor = System.Drawing.Color.White;
            this.txtChatSendMessage.Location = new System.Drawing.Point(626, 400);
            this.txtChatSendMessage.Name = "txtChatSendMessage";
            this.txtChatSendMessage.Size = new System.Drawing.Size(288, 20);
            this.txtChatSendMessage.TabIndex = 12;
            this.txtChatSendMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtChatSendMessage_KeyUp);
            // 
            // btnOpenTool
            // 
            this.btnOpenTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenTool.Location = new System.Drawing.Point(238, 388);
            this.btnOpenTool.Name = "btnOpenTool";
            this.btnOpenTool.Size = new System.Drawing.Size(75, 32);
            this.btnOpenTool.TabIndex = 1;
            this.btnOpenTool.Text = "Open";
            this.btnOpenTool.UseVisualStyleBackColor = true;
            this.btnOpenTool.Click += new System.EventHandler(this.btnOpenTool_Click);
            // 
            // cmbxTool
            // 
            this.cmbxTool.BackColor = System.Drawing.Color.Black;
            this.cmbxTool.ForeColor = System.Drawing.Color.White;
            this.cmbxTool.FormattingEnabled = true;
            this.cmbxTool.Location = new System.Drawing.Point(11, 399);
            this.cmbxTool.Name = "cmbxTool";
            this.cmbxTool.Size = new System.Drawing.Size(220, 21);
            this.cmbxTool.TabIndex = 0;
            this.cmbxTool.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbxTool_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(11, 215);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Multiple Install Queue:";
            // 
            // lblSpaceRequired
            // 
            this.lblSpaceRequired.AutoSize = true;
            this.lblSpaceRequired.ForeColor = System.Drawing.Color.White;
            this.lblSpaceRequired.Location = new System.Drawing.Point(8, 165);
            this.lblSpaceRequired.Name = "lblSpaceRequired";
            this.lblSpaceRequired.Size = new System.Drawing.Size(87, 13);
            this.lblSpaceRequired.TabIndex = 16;
            this.lblSpaceRequired.Text = "Space Required:";
            // 
            // pbTitleExpanded
            // 
            this.pbTitleExpanded.BackColor = System.Drawing.Color.Transparent;
            this.pbTitleExpanded.ErrorImage = null;
            this.pbTitleExpanded.InitialImage = null;
            this.pbTitleExpanded.Location = new System.Drawing.Point(-12, -9);
            this.pbTitleExpanded.Name = "pbTitleExpanded";
            this.pbTitleExpanded.Size = new System.Drawing.Size(941, 99);
            this.pbTitleExpanded.TabIndex = 17;
            this.pbTitleExpanded.TabStop = false;
            this.pbTitleExpanded.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            // 
            // btnExit
            // 
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(890, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(30, 29);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pbTitle
            // 
            this.pbTitle.BackgroundImage = global::Lanstaller.Properties.Resources.LanstallerThin;
            this.pbTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbTitle.InitialImage = null;
            this.pbTitle.Location = new System.Drawing.Point(8, 7);
            this.pbTitle.Name = "pbTitle";
            this.pbTitle.Size = new System.Drawing.Size(886, 83);
            this.pbTitle.TabIndex = 7;
            this.pbTitle.TabStop = false;
            this.pbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(624, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Chat:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(8, 385);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Tools:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(316, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "User Preferences:";
            // 
            // frmLanstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(920, 426);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnOpenTool);
            this.Controls.Add(this.cmbxTool);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtChatSendMessage);
            this.Controls.Add(this.txtChatMessages);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pbTitle);
            this.Controls.Add(this.lblSpaceRequired);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.gbxStatus);
            this.Controls.Add(this.gbxPref);
            this.Controls.Add(this.gbxActions);
            this.Controls.Add(this.lbxInstallList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInstallDirectory);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.cmbxSoftware);
            this.Controls.Add(this.pbTitleExpanded);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLanstaller";
            this.Text = "Lanstaller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLanstaller_Closing);
            this.Load += new System.EventHandler(this.frmLanstaller_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.gbxActions.ResumeLayout(false);
            this.gbxActions.PerformLayout();
            this.gbxPref.ResumeLayout(false);
            this.gbxPref.PerformLayout();
            this.gbxStatus.ResumeLayout(false);
            this.gbxStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitleExpanded)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).EndInit();
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
        private System.Windows.Forms.PictureBox pbTitle;
        private System.Windows.Forms.GroupBox gbxActions;
        private System.Windows.Forms.CheckBox chkPreferences;
        private System.Windows.Forms.CheckBox chkShortcuts;
        private System.Windows.Forms.CheckBox chkRegistry;
        private System.Windows.Forms.CheckBox chkFiles;
        private System.Windows.Forms.GroupBox gbxPref;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.GroupBox gbxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txtChatMessages;
        private System.Windows.Forms.TextBox txtChatSendMessage;
        private System.Windows.Forms.Button btnOpenTool;
        private System.Windows.Forms.ComboBox cmbxTool;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSpaceRequired;
        private System.Windows.Forms.CheckBox chkWindowsSettings;
        private System.Windows.Forms.ProgressBar pbInstall;
        private System.Windows.Forms.CheckBox chkRedist;
        private System.Windows.Forms.PictureBox pbTitleExpanded;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

