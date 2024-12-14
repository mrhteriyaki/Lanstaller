
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLanstaller));
            btnInstall = new System.Windows.Forms.Button();
            txtInstallDirectory = new System.Windows.Forms.TextBox();
            lblinstDir = new System.Windows.Forms.Label();
            gbxActions = new System.Windows.Forms.GroupBox();
            chkRedist = new System.Windows.Forms.CheckBox();
            chkWindowsSettings = new System.Windows.Forms.CheckBox();
            chkPreferences = new System.Windows.Forms.CheckBox();
            chkShortcuts = new System.Windows.Forms.CheckBox();
            chkRegistry = new System.Windows.Forms.CheckBox();
            chkFiles = new System.Windows.Forms.CheckBox();
            gbxPref = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtHeight = new System.Windows.Forms.TextBox();
            txtWidth = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            txtUsername = new System.Windows.Forms.TextBox();
            gbxStatus = new System.Windows.Forms.GroupBox();
            pbInstall = new System.Windows.Forms.ProgressBar();
            lblStatus = new System.Windows.Forms.Label();
            txtChatMessages = new System.Windows.Forms.TextBox();
            txtChatSendMessage = new System.Windows.Forms.TextBox();
            lblSpaceRequired = new System.Windows.Forms.Label();
            pbTitleExpanded = new System.Windows.Forms.PictureBox();
            btnExit = new System.Windows.Forms.Button();
            pbTitle = new System.Windows.Forms.PictureBox();
            label4 = new System.Windows.Forms.Label();
            tmrRefresh = new System.Windows.Forms.Timer(components);
            lvSoftware = new System.Windows.Forms.ListView();
            pbBottomright = new System.Windows.Forms.PictureBox();
            btnMin = new System.Windows.Forms.Button();
            csmSoftware = new System.Windows.Forms.ContextMenuStrip(components);
            reinstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            lblWANMode = new System.Windows.Forms.Label();
            txtCompat = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            gbxActions.SuspendLayout();
            gbxPref.SuspendLayout();
            gbxStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbTitleExpanded).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbTitle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbBottomright).BeginInit();
            csmSoftware.SuspendLayout();
            SuspendLayout();
            // 
            // btnInstall
            // 
            btnInstall.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnInstall.BackgroundImage");
            btnInstall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnInstall.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnInstall.ForeColor = System.Drawing.Color.White;
            btnInstall.Location = new System.Drawing.Point(407, 587);
            btnInstall.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            btnInstall.Name = "btnInstall";
            btnInstall.Size = new System.Drawing.Size(273, 85);
            btnInstall.TabIndex = 2;
            btnInstall.Text = "Install";
            btnInstall.UseVisualStyleBackColor = true;
            btnInstall.Click += btnInstall_Click;
            // 
            // txtInstallDirectory
            // 
            txtInstallDirectory.BackColor = System.Drawing.Color.Black;
            txtInstallDirectory.ForeColor = System.Drawing.Color.White;
            txtInstallDirectory.Location = new System.Drawing.Point(407, 169);
            txtInstallDirectory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtInstallDirectory.Name = "txtInstallDirectory";
            txtInstallDirectory.Size = new System.Drawing.Size(273, 27);
            txtInstallDirectory.TabIndex = 5;
            txtInstallDirectory.TextChanged += txtInstallDirectory_TextChanged;
            // 
            // lblinstDir
            // 
            lblinstDir.AutoSize = true;
            lblinstDir.ForeColor = System.Drawing.Color.White;
            lblinstDir.Location = new System.Drawing.Point(402, 144);
            lblinstDir.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblinstDir.Name = "lblinstDir";
            lblinstDir.Size = new System.Drawing.Size(116, 20);
            lblinstDir.TabIndex = 6;
            lblinstDir.Text = "Install Directory:";
            // 
            // gbxActions
            // 
            gbxActions.Controls.Add(chkRedist);
            gbxActions.Controls.Add(chkWindowsSettings);
            gbxActions.Controls.Add(chkPreferences);
            gbxActions.Controls.Add(chkShortcuts);
            gbxActions.Controls.Add(chkRegistry);
            gbxActions.Controls.Add(chkFiles);
            gbxActions.ForeColor = System.Drawing.Color.White;
            gbxActions.Location = new System.Drawing.Point(407, 229);
            gbxActions.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxActions.Name = "gbxActions";
            gbxActions.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxActions.Size = new System.Drawing.Size(273, 224);
            gbxActions.TabIndex = 8;
            gbxActions.TabStop = false;
            gbxActions.Text = "Actions:";
            // 
            // chkRedist
            // 
            chkRedist.AutoSize = true;
            chkRedist.Checked = true;
            chkRedist.CheckState = System.Windows.Forms.CheckState.Checked;
            chkRedist.ForeColor = System.Drawing.Color.White;
            chkRedist.Location = new System.Drawing.Point(7, 184);
            chkRedist.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkRedist.Name = "chkRedist";
            chkRedist.Size = new System.Drawing.Size(138, 24);
            chkRedist.TabIndex = 5;
            chkRedist.Text = "Redistributables";
            chkRedist.UseVisualStyleBackColor = true;
            // 
            // chkWindowsSettings
            // 
            chkWindowsSettings.AutoSize = true;
            chkWindowsSettings.Checked = true;
            chkWindowsSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            chkWindowsSettings.ForeColor = System.Drawing.Color.White;
            chkWindowsSettings.Location = new System.Drawing.Point(7, 153);
            chkWindowsSettings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkWindowsSettings.Name = "chkWindowsSettings";
            chkWindowsSettings.Size = new System.Drawing.Size(149, 24);
            chkWindowsSettings.TabIndex = 4;
            chkWindowsSettings.Text = "Windows Settings";
            chkWindowsSettings.UseVisualStyleBackColor = true;
            // 
            // chkPreferences
            // 
            chkPreferences.AutoSize = true;
            chkPreferences.Checked = true;
            chkPreferences.CheckState = System.Windows.Forms.CheckState.Checked;
            chkPreferences.ForeColor = System.Drawing.Color.White;
            chkPreferences.Location = new System.Drawing.Point(7, 123);
            chkPreferences.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkPreferences.Name = "chkPreferences";
            chkPreferences.Size = new System.Drawing.Size(140, 24);
            chkPreferences.TabIndex = 3;
            chkPreferences.Text = "User Preferences";
            chkPreferences.UseVisualStyleBackColor = true;
            // 
            // chkShortcuts
            // 
            chkShortcuts.AutoSize = true;
            chkShortcuts.Checked = true;
            chkShortcuts.CheckState = System.Windows.Forms.CheckState.Checked;
            chkShortcuts.ForeColor = System.Drawing.Color.White;
            chkShortcuts.Location = new System.Drawing.Point(7, 92);
            chkShortcuts.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkShortcuts.Name = "chkShortcuts";
            chkShortcuts.Size = new System.Drawing.Size(92, 24);
            chkShortcuts.TabIndex = 2;
            chkShortcuts.Text = "Shortcuts";
            chkShortcuts.UseVisualStyleBackColor = true;
            // 
            // chkRegistry
            // 
            chkRegistry.AutoSize = true;
            chkRegistry.Checked = true;
            chkRegistry.CheckState = System.Windows.Forms.CheckState.Checked;
            chkRegistry.ForeColor = System.Drawing.Color.White;
            chkRegistry.Location = new System.Drawing.Point(7, 61);
            chkRegistry.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkRegistry.Name = "chkRegistry";
            chkRegistry.Size = new System.Drawing.Size(84, 24);
            chkRegistry.TabIndex = 1;
            chkRegistry.Text = "Registry";
            chkRegistry.UseVisualStyleBackColor = true;
            // 
            // chkFiles
            // 
            chkFiles.AutoSize = true;
            chkFiles.Checked = true;
            chkFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            chkFiles.ForeColor = System.Drawing.Color.White;
            chkFiles.Location = new System.Drawing.Point(7, 31);
            chkFiles.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            chkFiles.Name = "chkFiles";
            chkFiles.Size = new System.Drawing.Size(98, 24);
            chkFiles.TabIndex = 0;
            chkFiles.Text = "Copy Files";
            chkFiles.UseVisualStyleBackColor = true;
            // 
            // gbxPref
            // 
            gbxPref.Controls.Add(label5);
            gbxPref.Controls.Add(label3);
            gbxPref.Controls.Add(txtHeight);
            gbxPref.Controls.Add(txtWidth);
            gbxPref.Controls.Add(label2);
            gbxPref.Controls.Add(txtUsername);
            gbxPref.ForeColor = System.Drawing.Color.White;
            gbxPref.Location = new System.Drawing.Point(407, 463);
            gbxPref.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxPref.Name = "gbxPref";
            gbxPref.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxPref.Size = new System.Drawing.Size(273, 113);
            gbxPref.TabIndex = 9;
            gbxPref.TabStop = false;
            gbxPref.Text = "User Preferences";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = System.Drawing.Color.White;
            label5.Location = new System.Drawing.Point(170, 71);
            label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(18, 20);
            label5.TabIndex = 6;
            label5.Text = "X";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(14, 71);
            label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(82, 20);
            label3.TabIndex = 4;
            label3.Text = "Resolution:";
            // 
            // txtHeight
            // 
            txtHeight.BackColor = System.Drawing.Color.Black;
            txtHeight.ForeColor = System.Drawing.Color.White;
            txtHeight.Location = new System.Drawing.Point(189, 67);
            txtHeight.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtHeight.Name = "txtHeight";
            txtHeight.Size = new System.Drawing.Size(71, 27);
            txtHeight.TabIndex = 3;
            txtHeight.TextChanged += txtHeight_TextChanged;
            // 
            // txtWidth
            // 
            txtWidth.BackColor = System.Drawing.Color.Black;
            txtWidth.ForeColor = System.Drawing.Color.White;
            txtWidth.Location = new System.Drawing.Point(95, 67);
            txtWidth.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtWidth.Name = "txtWidth";
            txtWidth.Size = new System.Drawing.Size(71, 27);
            txtWidth.TabIndex = 2;
            txtWidth.TextChanged += txtWidth_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = System.Drawing.Color.White;
            label2.Location = new System.Drawing.Point(10, 29);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 20);
            label2.TabIndex = 1;
            label2.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = System.Drawing.Color.Black;
            txtUsername.ForeColor = System.Drawing.Color.White;
            txtUsername.Location = new System.Drawing.Point(95, 27);
            txtUsername.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(165, 27);
            txtUsername.TabIndex = 0;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // gbxStatus
            // 
            gbxStatus.Controls.Add(pbInstall);
            gbxStatus.Controls.Add(lblStatus);
            gbxStatus.ForeColor = System.Drawing.Color.White;
            gbxStatus.Location = new System.Drawing.Point(407, 673);
            gbxStatus.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxStatus.Name = "gbxStatus";
            gbxStatus.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            gbxStatus.Size = new System.Drawing.Size(273, 221);
            gbxStatus.TabIndex = 10;
            gbxStatus.TabStop = false;
            gbxStatus.Text = "Status";
            // 
            // pbInstall
            // 
            pbInstall.Enabled = false;
            pbInstall.Location = new System.Drawing.Point(7, 179);
            pbInstall.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pbInstall.Name = "pbInstall";
            pbInstall.Size = new System.Drawing.Size(256, 33);
            pbInstall.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(7, 27);
            lblStatus.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(97, 20);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Status: Ready";
            // 
            // txtChatMessages
            // 
            txtChatMessages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtChatMessages.BackColor = System.Drawing.Color.Black;
            txtChatMessages.ForeColor = System.Drawing.Color.White;
            txtChatMessages.Location = new System.Drawing.Point(688, 548);
            txtChatMessages.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtChatMessages.Multiline = true;
            txtChatMessages.Name = "txtChatMessages";
            txtChatMessages.ReadOnly = true;
            txtChatMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtChatMessages.Size = new System.Drawing.Size(462, 285);
            txtChatMessages.TabIndex = 11;
            // 
            // txtChatSendMessage
            // 
            txtChatSendMessage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtChatSendMessage.BackColor = System.Drawing.Color.Black;
            txtChatSendMessage.ForeColor = System.Drawing.Color.White;
            txtChatSendMessage.Location = new System.Drawing.Point(689, 863);
            txtChatSendMessage.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtChatSendMessage.Name = "txtChatSendMessage";
            txtChatSendMessage.Size = new System.Drawing.Size(438, 27);
            txtChatSendMessage.TabIndex = 12;
            txtChatSendMessage.KeyUp += txtChatSendMessage_KeyUp;
            // 
            // lblSpaceRequired
            // 
            lblSpaceRequired.AutoSize = true;
            lblSpaceRequired.ForeColor = System.Drawing.Color.White;
            lblSpaceRequired.Location = new System.Drawing.Point(402, 204);
            lblSpaceRequired.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblSpaceRequired.Name = "lblSpaceRequired";
            lblSpaceRequired.Size = new System.Drawing.Size(116, 20);
            lblSpaceRequired.TabIndex = 16;
            lblSpaceRequired.Text = "Space Required:";
            // 
            // pbTitleExpanded
            // 
            pbTitleExpanded.BackColor = System.Drawing.Color.Transparent;
            pbTitleExpanded.ErrorImage = null;
            pbTitleExpanded.InitialImage = null;
            pbTitleExpanded.Location = new System.Drawing.Point(0, 0);
            pbTitleExpanded.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pbTitleExpanded.Name = "pbTitleExpanded";
            pbTitleExpanded.Size = new System.Drawing.Size(1173, 139);
            pbTitleExpanded.TabIndex = 17;
            pbTitleExpanded.TabStop = false;
            pbTitleExpanded.Click += pbTitleExpanded_Click;
            pbTitleExpanded.MouseDown += Caption_MouseDown;
            // 
            // btnExit
            // 
            btnExit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnExit.ForeColor = System.Drawing.Color.White;
            btnExit.Location = new System.Drawing.Point(1120, 0);
            btnExit.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(40, 44);
            btnExit.TabIndex = 18;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // pbTitle
            // 
            pbTitle.BackgroundImage = Properties.Resources.LanstallerThin;
            pbTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            pbTitle.InitialImage = null;
            pbTitle.Location = new System.Drawing.Point(23, 8);
            pbTitle.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pbTitle.Name = "pbTitle";
            pbTitle.Size = new System.Drawing.Size(998, 128);
            pbTitle.TabIndex = 7;
            pbTitle.TabStop = false;
            pbTitle.MouseDown += Caption_MouseDown;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.ForeColor = System.Drawing.Color.White;
            label4.Location = new System.Drawing.Point(688, 839);
            label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(42, 20);
            label4.TabIndex = 7;
            label4.Text = "Chat:";
            // 
            // tmrRefresh
            // 
            tmrRefresh.Enabled = true;
            tmrRefresh.Interval = 4000;
            tmrRefresh.Tick += tmrRefresh_Tick;
            // 
            // lvSoftware
            // 
            lvSoftware.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lvSoftware.BackColor = System.Drawing.Color.Black;
            lvSoftware.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            lvSoftware.ForeColor = System.Drawing.Color.White;
            lvSoftware.Location = new System.Drawing.Point(0, 143);
            lvSoftware.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            lvSoftware.Name = "lvSoftware";
            lvSoftware.Size = new System.Drawing.Size(398, 751);
            lvSoftware.TabIndex = 21;
            lvSoftware.UseCompatibleStateImageBehavior = false;
            lvSoftware.SelectedIndexChanged += lvSoftware_SelectedIndexChanged;
            lvSoftware.DoubleClick += lvSoftware_DoubleClick;
            lvSoftware.MouseClick += lvSoftware_MouseClick;
            // 
            // pbBottomright
            // 
            pbBottomright.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            pbBottomright.BackColor = System.Drawing.Color.Transparent;
            pbBottomright.BackgroundImage = (System.Drawing.Image)resources.GetObject("pbBottomright.BackgroundImage");
            pbBottomright.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            pbBottomright.ErrorImage = null;
            pbBottomright.InitialImage = null;
            pbBottomright.Location = new System.Drawing.Point(1137, 871);
            pbBottomright.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            pbBottomright.Name = "pbBottomright";
            pbBottomright.Size = new System.Drawing.Size(21, 23);
            pbBottomright.TabIndex = 22;
            pbBottomright.TabStop = false;
            pbBottomright.MouseDown += pbBottomright_MouseDown;
            pbBottomright.MouseMove += pbBottomright_MouseMove;
            pbBottomright.MouseUp += pbBottomright_MouseUp;
            // 
            // btnMin
            // 
            btnMin.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnMin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnMin.ForeColor = System.Drawing.Color.White;
            btnMin.Location = new System.Drawing.Point(1079, 0);
            btnMin.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            btnMin.Name = "btnMin";
            btnMin.Size = new System.Drawing.Size(40, 44);
            btnMin.TabIndex = 23;
            btnMin.Text = "_";
            btnMin.UseVisualStyleBackColor = true;
            btnMin.Click += btnMin_Click;
            // 
            // csmSoftware
            // 
            csmSoftware.BackColor = System.Drawing.SystemColors.Control;
            csmSoftware.ImageScalingSize = new System.Drawing.Size(20, 20);
            csmSoftware.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { reinstallToolStripMenuItem });
            csmSoftware.Name = "csmSoftware";
            csmSoftware.Size = new System.Drawing.Size(135, 28);
            // 
            // reinstallToolStripMenuItem
            // 
            reinstallToolStripMenuItem.Name = "reinstallToolStripMenuItem";
            reinstallToolStripMenuItem.Size = new System.Drawing.Size(134, 24);
            reinstallToolStripMenuItem.Text = "Reinstall";
            reinstallToolStripMenuItem.Click += reinstallToolStripMenuItem_Click;
            // 
            // lblWANMode
            // 
            lblWANMode.AutoSize = true;
            lblWANMode.Location = new System.Drawing.Point(1024, 104);
            lblWANMode.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            lblWANMode.Name = "lblWANMode";
            lblWANMode.Size = new System.Drawing.Size(144, 20);
            lblWANMode.TabIndex = 24;
            lblWANMode.Text = "WAN Mode Enabled";
            lblWANMode.Visible = false;
            // 
            // txtCompat
            // 
            txtCompat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtCompat.BackColor = System.Drawing.Color.Black;
            txtCompat.ForeColor = System.Drawing.Color.White;
            txtCompat.Location = new System.Drawing.Point(688, 169);
            txtCompat.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            txtCompat.Multiline = true;
            txtCompat.Name = "txtCompat";
            txtCompat.ReadOnly = true;
            txtCompat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtCompat.Size = new System.Drawing.Size(462, 369);
            txtCompat.TabIndex = 25;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = System.Drawing.Color.White;
            label1.Location = new System.Drawing.Point(527, 439);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(116, 20);
            label1.TabIndex = 26;
            label1.Text = "Install Directory:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = System.Drawing.Color.White;
            label6.Location = new System.Drawing.Point(689, 144);
            label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(102, 20);
            label6.TabIndex = 27;
            label6.Text = "Compatibility:";
            // 
            // frmLanstaller
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Black;
            ClientSize = new System.Drawing.Size(1160, 897);
            Controls.Add(label6);
            Controls.Add(label1);
            Controls.Add(txtCompat);
            Controls.Add(lblWANMode);
            Controls.Add(btnMin);
            Controls.Add(pbBottomright);
            Controls.Add(lvSoftware);
            Controls.Add(label4);
            Controls.Add(btnInstall);
            Controls.Add(txtChatSendMessage);
            Controls.Add(txtChatMessages);
            Controls.Add(btnExit);
            Controls.Add(pbTitle);
            Controls.Add(lblSpaceRequired);
            Controls.Add(gbxStatus);
            Controls.Add(gbxPref);
            Controls.Add(gbxActions);
            Controls.Add(lblinstDir);
            Controls.Add(txtInstallDirectory);
            Controls.Add(pbTitleExpanded);
            ForeColor = System.Drawing.Color.White;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            Name = "frmLanstaller";
            Text = "Lanstaller";
            FormClosing += frmLanstaller_Closing;
            Load += frmLanstaller_Load;
            Paint += OnPaint;
            gbxActions.ResumeLayout(false);
            gbxActions.PerformLayout();
            gbxPref.ResumeLayout(false);
            gbxPref.PerformLayout();
            gbxStatus.ResumeLayout(false);
            gbxStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbTitleExpanded).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbTitle).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbBottomright).EndInit();
            csmSoftware.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.TextBox txtInstallDirectory;
        private System.Windows.Forms.Label lblinstDir;
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
        private System.Windows.Forms.Label lblWANMode;
        private System.Windows.Forms.TextBox txtCompat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
    }
}

