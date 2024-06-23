
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLanstaller));
            this.btnInstall = new System.Windows.Forms.Button();
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
            this.lblSpaceRequired = new System.Windows.Forms.Label();
            this.pbTitleExpanded = new System.Windows.Forms.PictureBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.pbTitle = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.lvSoftware = new System.Windows.Forms.ListView();
            this.pbBottomright = new System.Windows.Forms.PictureBox();
            this.btnMin = new System.Windows.Forms.Button();
            this.csmSoftware = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.reinstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbxActions.SuspendLayout();
            this.gbxPref.SuspendLayout();
            this.gbxStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitleExpanded)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomright)).BeginInit();
            this.csmSoftware.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInstall
            // 
            this.btnInstall.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnInstall.BackgroundImage")));
            this.btnInstall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstall.ForeColor = System.Drawing.Color.White;
            this.btnInstall.Location = new System.Drawing.Point(351, 391);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(180, 60);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // txtInstallDirectory
            // 
            this.txtInstallDirectory.BackColor = System.Drawing.Color.Black;
            this.txtInstallDirectory.ForeColor = System.Drawing.Color.White;
            this.txtInstallDirectory.Location = new System.Drawing.Point(339, 110);
            this.txtInstallDirectory.Name = "txtInstallDirectory";
            this.txtInstallDirectory.Size = new System.Drawing.Size(203, 20);
            this.txtInstallDirectory.TabIndex = 5;
            this.txtInstallDirectory.TextChanged += new System.EventHandler(this.txtInstallDirectory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(336, 94);
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
            this.gbxActions.Location = new System.Drawing.Point(339, 149);
            this.gbxActions.Name = "gbxActions";
            this.gbxActions.Size = new System.Drawing.Size(203, 146);
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
            this.gbxPref.Location = new System.Drawing.Point(339, 301);
            this.gbxPref.Name = "gbxPref";
            this.gbxPref.Size = new System.Drawing.Size(203, 74);
            this.gbxPref.TabIndex = 9;
            this.gbxPref.TabStop = false;
            this.gbxPref.Text = "User Preferences";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(124, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Resolution:";
            // 
            // txtHeight
            // 
            this.txtHeight.BackColor = System.Drawing.Color.Black;
            this.txtHeight.ForeColor = System.Drawing.Color.White;
            this.txtHeight.Location = new System.Drawing.Point(138, 45);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(55, 20);
            this.txtHeight.TabIndex = 3;
            this.txtHeight.TextChanged += new System.EventHandler(this.txtHeight_TextChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.BackColor = System.Drawing.Color.Black;
            this.txtWidth.ForeColor = System.Drawing.Color.White;
            this.txtWidth.Location = new System.Drawing.Point(68, 45);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(55, 20);
            this.txtWidth.TabIndex = 2;
            this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.Black;
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location = new System.Drawing.Point(68, 19);
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
            this.gbxStatus.Location = new System.Drawing.Point(305, 457);
            this.gbxStatus.Name = "gbxStatus";
            this.gbxStatus.Size = new System.Drawing.Size(260, 122);
            this.gbxStatus.TabIndex = 10;
            this.gbxStatus.TabStop = false;
            this.gbxStatus.Text = "Status";
            // 
            // pbInstall
            // 
            this.pbInstall.Enabled = false;
            this.pbInstall.Location = new System.Drawing.Point(6, 94);
            this.pbInstall.Name = "pbInstall";
            this.pbInstall.Size = new System.Drawing.Size(248, 22);
            this.pbInstall.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(5, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(74, 13);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Ready";
            // 
            // txtChatMessages
            // 
            this.txtChatMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChatMessages.BackColor = System.Drawing.Color.Black;
            this.txtChatMessages.ForeColor = System.Drawing.Color.White;
            this.txtChatMessages.Location = new System.Drawing.Point(574, 96);
            this.txtChatMessages.Multiline = true;
            this.txtChatMessages.Name = "txtChatMessages";
            this.txtChatMessages.ReadOnly = true;
            this.txtChatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChatMessages.Size = new System.Drawing.Size(366, 457);
            this.txtChatMessages.TabIndex = 11;
            // 
            // txtChatSendMessage
            // 
            this.txtChatSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChatSendMessage.BackColor = System.Drawing.Color.Black;
            this.txtChatSendMessage.ForeColor = System.Drawing.Color.White;
            this.txtChatSendMessage.Location = new System.Drawing.Point(609, 559);
            this.txtChatSendMessage.Name = "txtChatSendMessage";
            this.txtChatSendMessage.Size = new System.Drawing.Size(322, 20);
            this.txtChatSendMessage.TabIndex = 12;
            this.txtChatSendMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtChatSendMessage_KeyUp);
            // 
            // lblSpaceRequired
            // 
            this.lblSpaceRequired.AutoSize = true;
            this.lblSpaceRequired.ForeColor = System.Drawing.Color.White;
            this.lblSpaceRequired.Location = new System.Drawing.Point(336, 133);
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
            this.pbTitleExpanded.Location = new System.Drawing.Point(0, 0);
            this.pbTitleExpanded.Name = "pbTitleExpanded";
            this.pbTitleExpanded.Size = new System.Drawing.Size(967, 90);
            this.pbTitleExpanded.TabIndex = 17;
            this.pbTitleExpanded.TabStop = false;
            this.pbTitleExpanded.Click += new System.EventHandler(this.pbTitleExpanded_Click);
            this.pbTitleExpanded.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(922, 0);
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
            this.pbTitle.Location = new System.Drawing.Point(90, 5);
            this.pbTitle.Name = "pbTitle";
            this.pbTitle.Size = new System.Drawing.Size(748, 83);
            this.pbTitle.TabIndex = 7;
            this.pbTitle.TabStop = false;
            this.pbTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(571, 562);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Chat:";
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Interval = 4000;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // lvSoftware
            // 
            this.lvSoftware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvSoftware.BackColor = System.Drawing.Color.Black;
            this.lvSoftware.ForeColor = System.Drawing.Color.White;
            this.lvSoftware.HideSelection = false;
            this.lvSoftware.Location = new System.Drawing.Point(0, 93);
            this.lvSoftware.Margin = new System.Windows.Forms.Padding(2);
            this.lvSoftware.Name = "lvSoftware";
            this.lvSoftware.Size = new System.Drawing.Size(300, 489);
            this.lvSoftware.TabIndex = 21;
            this.lvSoftware.UseCompatibleStateImageBehavior = false;
            this.lvSoftware.SelectedIndexChanged += new System.EventHandler(this.lvSoftware_SelectedIndexChanged);
            this.lvSoftware.DoubleClick += new System.EventHandler(this.lvSoftware_DoubleClick);
            this.lvSoftware.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvSoftware_MouseClick);
            // 
            // pbBottomright
            // 
            this.pbBottomright.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBottomright.BackColor = System.Drawing.Color.Transparent;
            this.pbBottomright.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pbBottomright.BackgroundImage")));
            this.pbBottomright.ErrorImage = null;
            this.pbBottomright.InitialImage = null;
            this.pbBottomright.Location = new System.Drawing.Point(935, 566);
            this.pbBottomright.Name = "pbBottomright";
            this.pbBottomright.Size = new System.Drawing.Size(15, 15);
            this.pbBottomright.TabIndex = 22;
            this.pbBottomright.TabStop = false;
            this.pbBottomright.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbBottomright_MouseDown);
            this.pbBottomright.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbBottomright_MouseMove);
            this.pbBottomright.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbBottomright_MouseUp);
            // 
            // btnMin
            // 
            this.btnMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMin.ForeColor = System.Drawing.Color.White;
            this.btnMin.Location = new System.Drawing.Point(891, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(30, 29);
            this.btnMin.TabIndex = 23;
            this.btnMin.Text = "_";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // csmSoftware
            // 
            this.csmSoftware.BackColor = System.Drawing.SystemColors.Control;
            this.csmSoftware.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reinstallToolStripMenuItem});
            this.csmSoftware.Name = "csmSoftware";
            this.csmSoftware.Size = new System.Drawing.Size(181, 48);
            // 
            // reinstallToolStripMenuItem
            // 
            this.reinstallToolStripMenuItem.Name = "reinstallToolStripMenuItem";
            this.reinstallToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reinstallToolStripMenuItem.Text = "Reinstall";
            this.reinstallToolStripMenuItem.Click += new System.EventHandler(this.reinstallToolStripMenuItem_Click);
            // 
            // frmLanstaller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(952, 583);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.pbBottomright);
            this.Controls.Add(this.lvSoftware);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.txtChatSendMessage);
            this.Controls.Add(this.txtChatMessages);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pbTitle);
            this.Controls.Add(this.lblSpaceRequired);
            this.Controls.Add(this.gbxStatus);
            this.Controls.Add(this.gbxPref);
            this.Controls.Add(this.gbxActions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInstallDirectory);
            this.Controls.Add(this.pbTitleExpanded);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            ((System.ComponentModel.ISupportInitialize)(this.pbBottomright)).EndInit();
            this.csmSoftware.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnInstall;
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
        private System.Windows.Forms.Label lblSpaceRequired;
        private System.Windows.Forms.CheckBox chkWindowsSettings;
        private System.Windows.Forms.ProgressBar pbInstall;
        private System.Windows.Forms.CheckBox chkRedist;
        private System.Windows.Forms.PictureBox pbTitleExpanded;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.ListView lvSoftware;
        private System.Windows.Forms.PictureBox pbBottomright;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.ContextMenuStrip csmSoftware;
        private System.Windows.Forms.ToolStripMenuItem reinstallToolStripMenuItem;
    }
}

