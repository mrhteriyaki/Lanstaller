
namespace Lanstaller_Management_Console
{
    partial class frmLanstallerMmanager
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
            this.lbxSoftware = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.txtScanfolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFolderStatus = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblDestination = new System.Windows.Forms.Label();
            this.lblFilename = new System.Windows.Forms.Label();
            this.lblSource = new System.Windows.Forms.Label();
            this.txtBaseFolder = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServerShare = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAddShortcut = new System.Windows.Forms.Button();
            this.txtIcon = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtWorking = new System.Windows.Forms.TextBox();
            this.txtFilepath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnFirewallRuleAdd = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.txtFirewallPath = new System.Windows.Forms.TextBox();
            this.txtSerialName = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtRegVal = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtRegKey = new System.Windows.Forms.TextBox();
            this.btnAddSerial = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSerialInstance = new System.Windows.Forms.TextBox();
            this.btnRescanFileSize = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnAddPrefFile = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.txtPrefFilePath = new System.Windows.Forms.TextBox();
            this.lblVariable = new System.Windows.Forms.Label();
            this.lblInstallInfo = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxSoftware
            // 
            this.lbxSoftware.FormattingEnabled = true;
            this.lbxSoftware.Location = new System.Drawing.Point(12, 45);
            this.lbxSoftware.Name = "lbxSoftware";
            this.lbxSoftware.Size = new System.Drawing.Size(171, 836);
            this.lbxSoftware.TabIndex = 0;
            this.lbxSoftware.SelectedIndexChanged += new System.EventHandler(this.lbxSoftware_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Software List:";
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(108, 21);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "Add New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // txtScanfolder
            // 
            this.txtScanfolder.Location = new System.Drawing.Point(83, 21);
            this.txtScanfolder.Name = "txtScanfolder";
            this.txtScanfolder.Size = new System.Drawing.Size(476, 20);
            this.txtScanfolder.TabIndex = 3;
            this.txtScanfolder.TextChanged += new System.EventHandler(this.txtScanfolder_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Scan Folder:";
            // 
            // lblFolderStatus
            // 
            this.lblFolderStatus.AutoSize = true;
            this.lblFolderStatus.Location = new System.Drawing.Point(54, 205);
            this.lblFolderStatus.Name = "lblFolderStatus";
            this.lblFolderStatus.Size = new System.Drawing.Size(40, 13);
            this.lblFolderStatus.TabIndex = 5;
            this.lblFolderStatus.Text = "Status:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.btnAddFolder);
            this.groupBox1.Controls.Add(this.btnScan);
            this.groupBox1.Controls.Add(this.lblDestination);
            this.groupBox1.Controls.Add(this.lblFilename);
            this.groupBox1.Controls.Add(this.lblSource);
            this.groupBox1.Controls.Add(this.txtBaseFolder);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtServerShare);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDestination);
            this.groupBox1.Controls.Add(this.txtScanfolder);
            this.groupBox1.Controls.Add(this.lblFolderStatus);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(189, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 228);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Folder";
            // 
            // btnAddFolder
            // 
            this.btnAddFolder.Enabled = false;
            this.btnAddFolder.Location = new System.Drawing.Point(459, 182);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(100, 28);
            this.btnAddFolder.TabIndex = 9;
            this.btnAddFolder.Text = "Add to Database";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            // 
            // btnScan
            // 
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(459, 150);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(100, 28);
            this.btnScan.TabIndex = 6;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(12, 180);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(82, 13);
            this.lblDestination.TabIndex = 16;
            this.lblDestination.Text = "Full Destination:";
            // 
            // lblFilename
            // 
            this.lblFilename.AutoSize = true;
            this.lblFilename.Location = new System.Drawing.Point(42, 165);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(52, 13);
            this.lblFilename.TabIndex = 15;
            this.lblFilename.Text = "Filename:";
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(50, 150);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(44, 13);
            this.lblSource.TabIndex = 14;
            this.lblSource.Text = "Source:";
            // 
            // txtBaseFolder
            // 
            this.txtBaseFolder.Location = new System.Drawing.Point(83, 44);
            this.txtBaseFolder.Name = "txtBaseFolder";
            this.txtBaseFolder.Size = new System.Drawing.Size(476, 20);
            this.txtBaseFolder.TabIndex = 12;
            this.txtBaseFolder.TextChanged += new System.EventHandler(this.txtBaseFolder_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(16, 47);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 13);
            this.label16.TabIndex = 13;
            this.label16.Text = "Base Folder:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Server Share:";
            // 
            // txtServerShare
            // 
            this.txtServerShare.Location = new System.Drawing.Point(79, 93);
            this.txtServerShare.Name = "txtServerShare";
            this.txtServerShare.Size = new System.Drawing.Size(302, 20);
            this.txtServerShare.TabIndex = 10;
            this.txtServerShare.TextChanged += new System.EventHandler(this.txtServerShare_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Destination:";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(79, 117);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(480, 20);
            this.txtDestination.TabIndex = 7;
            this.txtDestination.TextChanged += new System.EventHandler(this.txtDestination_TextChanged);
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
            this.groupBox2.Location = new System.Drawing.Point(187, 506);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(325, 158);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registry";
            // 
            // btnAddReg
            // 
            this.btnAddReg.Location = new System.Drawing.Point(221, 111);
            this.btnAddReg.Name = "btnAddReg";
            this.btnAddReg.Size = new System.Drawing.Size(98, 31);
            this.btnAddReg.TabIndex = 8;
            this.btnAddReg.Text = "Add to Database";
            this.btnAddReg.UseVisualStyleBackColor = true;
            this.btnAddReg.Click += new System.EventHandler(this.btnAddReg_Click);
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(65, 97);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(150, 20);
            this.txtData.TabIndex = 9;
            this.txtData.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(65, 70);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(237, 20);
            this.txtValue.TabIndex = 8;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(65, 42);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(237, 20);
            this.txtKey.TabIndex = 7;
            this.txtKey.TextChanged += new System.EventHandler(this.txtKey_TextChanged);
            // 
            // cmbxType
            // 
            this.cmbxType.FormattingEnabled = true;
            this.cmbxType.Items.AddRange(new object[] {
            "STRING",
            "DWORD"});
            this.cmbxType.Location = new System.Drawing.Point(65, 121);
            this.cmbxType.Name = "cmbxType";
            this.cmbxType.Size = new System.Drawing.Size(150, 21);
            this.cmbxType.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "Type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Data:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Value:";
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
            this.label5.Location = new System.Drawing.Point(6, 20);
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
            this.cmbxHiveKey.Location = new System.Drawing.Point(65, 17);
            this.cmbxHiveKey.Name = "cmbxHiveKey";
            this.cmbxHiveKey.Size = new System.Drawing.Size(174, 21);
            this.cmbxHiveKey.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAddShortcut);
            this.groupBox3.Controls.Add(this.txtIcon);
            this.groupBox3.Controls.Add(this.txtArguments);
            this.groupBox3.Controls.Add(this.txtWorking);
            this.groupBox3.Controls.Add(this.txtFilepath);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtName);
            this.groupBox3.Controls.Add(this.txtLocation);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(187, 670);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(358, 202);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Shortcut";
            // 
            // btnAddShortcut
            // 
            this.btnAddShortcut.Location = new System.Drawing.Point(235, 163);
            this.btnAddShortcut.Name = "btnAddShortcut";
            this.btnAddShortcut.Size = new System.Drawing.Size(110, 31);
            this.btnAddShortcut.TabIndex = 20;
            this.btnAddShortcut.Text = "Add to Database";
            this.btnAddShortcut.UseVisualStyleBackColor = true;
            this.btnAddShortcut.Click += new System.EventHandler(this.btnAddShortcut_Click);
            // 
            // txtIcon
            // 
            this.txtIcon.Location = new System.Drawing.Point(108, 137);
            this.txtIcon.Name = "txtIcon";
            this.txtIcon.Size = new System.Drawing.Size(237, 20);
            this.txtIcon.TabIndex = 19;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(108, 113);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(237, 20);
            this.txtArguments.TabIndex = 18;
            // 
            // txtWorking
            // 
            this.txtWorking.Location = new System.Drawing.Point(108, 91);
            this.txtWorking.Name = "txtWorking";
            this.txtWorking.Size = new System.Drawing.Size(237, 20);
            this.txtWorking.TabIndex = 17;
            // 
            // txtFilepath
            // 
            this.txtFilepath.Location = new System.Drawing.Point(108, 68);
            this.txtFilepath.Name = "txtFilepath";
            this.txtFilepath.Size = new System.Drawing.Size(237, 20);
            this.txtFilepath.TabIndex = 16;
            this.txtFilepath.Text = "%INSTALLPATH%\\";
            this.txtFilepath.TextChanged += new System.EventHandler(this.txtFilepath_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(71, 140);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(31, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Icon:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(42, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Arguments:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 13);
            this.label13.TabIndex = 13;
            this.label13.Text = "Working Path:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(55, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Filepath:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(64, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(108, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 20);
            this.txtName.TabIndex = 10;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(108, 45);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(237, 20);
            this.txtLocation.TabIndex = 1;
            this.txtLocation.Text = "C:\\Users\\Public\\Desktop";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Shortcut Location:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnFirewallRuleAdd);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.txtFirewallPath);
            this.groupBox4.Location = new System.Drawing.Point(187, 255);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 95);
            this.groupBox4.TabIndex = 9;
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
            // txtSerialName
            // 
            this.txtSerialName.Location = new System.Drawing.Point(170, 15);
            this.txtSerialName.Name = "txtSerialName";
            this.txtSerialName.Size = new System.Drawing.Size(224, 20);
            this.txtSerialName.TabIndex = 10;
            this.txtSerialName.TextChanged += new System.EventHandler(this.txtSerialName_TextChanged);
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
            this.groupBox5.Location = new System.Drawing.Point(187, 356);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(400, 144);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Serial Numbers";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
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
            this.btnAddSerial.Location = new System.Drawing.Point(294, 106);
            this.btnAddSerial.Name = "btnAddSerial";
            this.btnAddSerial.Size = new System.Drawing.Size(100, 28);
            this.btnAddSerial.TabIndex = 19;
            this.btnAddSerial.Text = "Add to Database";
            this.btnAddSerial.UseVisualStyleBackColor = true;
            this.btnAddSerial.Click += new System.EventHandler(this.btnAddSerial_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(74, 109);
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
            this.txtSerialInstance.Location = new System.Drawing.Point(170, 106);
            this.txtSerialInstance.Name = "txtSerialInstance";
            this.txtSerialInstance.Size = new System.Drawing.Size(44, 20);
            this.txtSerialInstance.TabIndex = 11;
            this.txtSerialInstance.Text = "1";
            this.txtSerialInstance.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // btnRescanFileSize
            // 
            this.btnRescanFileSize.Location = new System.Drawing.Point(634, 262);
            this.btnRescanFileSize.Name = "btnRescanFileSize";
            this.btnRescanFileSize.Size = new System.Drawing.Size(120, 51);
            this.btnRescanFileSize.TabIndex = 12;
            this.btnRescanFileSize.Text = "Rescan File Size";
            this.btnRescanFileSize.UseVisualStyleBackColor = true;
            this.btnRescanFileSize.Click += new System.EventHandler(this.btnRescanFileSize_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtReplace);
            this.groupBox6.Controls.Add(this.txtTarget);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.btnAddPrefFile);
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.txtPrefFilePath);
            this.groupBox6.Location = new System.Drawing.Point(518, 506);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(400, 158);
            this.groupBox6.TabIndex = 19;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Preference File:";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(75, 77);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(237, 20);
            this.txtReplace.TabIndex = 22;
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(75, 49);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(237, 20);
            this.txtTarget.TabIndex = 21;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(13, 80);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(50, 13);
            this.label23.TabIndex = 20;
            this.label23.Text = "Replace:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(22, 52);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(41, 13);
            this.label24.TabIndex = 19;
            this.label24.Text = "Target:";
            // 
            // btnAddPrefFile
            // 
            this.btnAddPrefFile.Location = new System.Drawing.Point(289, 112);
            this.btnAddPrefFile.Name = "btnAddPrefFile";
            this.btnAddPrefFile.Size = new System.Drawing.Size(100, 28);
            this.btnAddPrefFile.TabIndex = 17;
            this.btnAddPrefFile.Text = "Add to Database";
            this.btnAddPrefFile.UseVisualStyleBackColor = true;
            this.btnAddPrefFile.Click += new System.EventHandler(this.btnAddPrefFile_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 26);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(51, 13);
            this.label22.TabIndex = 18;
            this.label22.Text = "File Path:";
            // 
            // txtPrefFilePath
            // 
            this.txtPrefFilePath.Location = new System.Drawing.Point(75, 23);
            this.txtPrefFilePath.Name = "txtPrefFilePath";
            this.txtPrefFilePath.Size = new System.Drawing.Size(314, 20);
            this.txtPrefFilePath.TabIndex = 17;
            this.txtPrefFilePath.Text = "%INSTALLPATH%\\";
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(773, 30);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(69, 13);
            this.lblVariable.TabIndex = 20;
            this.lblVariable.Text = "Variable Info:";
            // 
            // lblInstallInfo
            // 
            this.lblInstallInfo.AutoSize = true;
            this.lblInstallInfo.Location = new System.Drawing.Point(776, 141);
            this.lblInstallInfo.Name = "lblInstallInfo";
            this.lblInstallInfo.Size = new System.Drawing.Size(81, 13);
            this.lblInstallInfo.TabIndex = 21;
            this.lblInstallInfo.Text = "Installation Info:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(387, 96);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(146, 13);
            this.label25.TabIndex = 22;
            this.label25.Text = "(Base share location of Scan)";
            // 
            // frmLanstallerMmanager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 984);
            this.Controls.Add(this.lblInstallInfo);
            this.Controls.Add(this.lblVariable);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.btnRescanFileSize);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbxSoftware);
            this.Name = "frmLanstallerMmanager";
            this.Text = "Lanstaller Management";
            this.Load += new System.EventHandler(this.frmLanstallerMmanager_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbxSoftware;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.TextBox txtScanfolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFolderStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerShare;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbxType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbxHiveKey;
        private System.Windows.Forms.Button btnAddReg;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAddShortcut;
        private System.Windows.Forms.TextBox txtIcon;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.TextBox txtWorking;
        private System.Windows.Forms.TextBox txtFilepath;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtBaseFolder;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFirewallRuleAdd;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtFirewallPath;
        private System.Windows.Forms.TextBox txtSerialName;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtSerialInstance;
        private System.Windows.Forms.Button btnAddSerial;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtRegKey;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtRegVal;
        private System.Windows.Forms.Button btnRescanFileSize;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnAddPrefFile;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtPrefFilePath;
        private System.Windows.Forms.Label lblVariable;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblInstallInfo;
        private System.Windows.Forms.Label label25;
    }
}

