using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//using Lanstaller;
using LanstallerShared;

using Microsoft.VisualBasic;
using Microsoft.Win32;

using System.IO;

namespace Lanstaller_Management_Console
{
    public partial class frmLanstallerManager : Form
    {
        List<SoftwareInfo> SoftwareList = new List<SoftwareInfo>();
        SoftwareInfo CurrentSelectedSoftware;

        List<Redistributable> RedistList = new List<Redistributable>();

        //static string status = "Status: Ready";

        //Declare Panels.
        Panels.Files FilesPanel = new Panels.Files();
        Panels.Preferences PreferencesPanel = new Panels.Preferences();
        Panels.Registry RegistryPanel = new Panels.Registry();
        Panels.Serial SerialPanel = new Panels.Serial();
        Panels.Shortcuts ShortcutsPanel = new Panels.Shortcuts();
        Panels.Windows_Settings WindowsSettingsPanel = new Panels.Windows_Settings();

        int panel_button_x = 150;
        int serialid_pool_selected = 0;

        string[] filelist;
        string[] directories;

        public frmLanstallerManager()
        {
            InitializeComponent();
        }

        List<UserControl> PanelList = new List<UserControl>();

        void AddPanel(UserControl NewPanel)
        {
            //Generate Button for Panel.
            Button NewButton = new Button();
            NewButton.Text = NewPanel.Name;
            NewButton.Location = new Point(panel_button_x, 4);
            NewButton.Width = 120;
            panel_button_x += 120;

            NewButton.Click += (sender2, e2) => PanelButtonClick(sender2, e2, NewPanel);

            this.Controls.Add(NewButton);

            NewPanel.Hide();
            NewPanel.Location = new Point(gbxInfo.Location.X, (gbxInfo.Location.Y + gbxInfo.Height + 10));
            this.Controls.Add(NewPanel);
            PanelList.Add(NewPanel);
        }

        void PanelButtonClick(object sender, EventArgs e, UserControl Panel)
        {
            foreach (UserControl UC in PanelList)
            {
                UC.Hide();
            }

            //Button button = sender as Button;

            Panel.Show();
        }


        private void frmLanstallerMmanager_Load(object sender, EventArgs e)
        {
            lvSoftware.Columns.Add("Software", 260);
            lvSoftware.Columns.Add("Files", 60);
            lvSoftware.Columns.Add("Registry", 70);
            lvSoftware.Columns.Add("Shortcuts", 70);
            lvSoftware.Columns.Add("Firewall Rules", 80);



            LanstallerServer.ConnectionString = "Data Source=192.168.88.3,1433;Initial Catalog=lanstaller;Integrated Security=true;TrustServerCertificate=True";
            
            
            /*
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("missing config.ini");
                this.Close();
                return;
            }

            foreach (string line in System.IO.File.ReadAllLines("config.ini"))
            {
                if (line.StartsWith("Data Source="))
                {
                    LanstallerServer.ConnectionString = line;
                }
            }
            */

            AddPanel(FilesPanel);
            AddPanel(PreferencesPanel);
            AddPanel(RegistryPanel);
            AddPanel(SerialPanel);
            AddPanel(ShortcutsPanel);
            AddPanel(WindowsSettingsPanel);


            lblVariable.Text = "%INSTALLPATH% = Base Install Directory (Default C:\\Games)" + Environment.NewLine +
                               "%USERPROFILE% = User Profile Path (Default C:\\Users\\<Username>)" + Environment.NewLine +
                               "%WIDTH% = Resolution Preference X" + Environment.NewLine +
                               "%HEIGHT% = Resolution Preference Y" + Environment.NewLine +
                               "%USERNAME% = Username Preference";


            FilesPanel.lblCopyActionInfo.Text = "";

            RefreshSoftware();

            Registry.CurrentUser.CreateSubKey("SOFTWARE\\Lanstaller");
            foreach (string ValueName in Registry.CurrentUser.OpenSubKey("SOFTWARE\\Lanstaller", true).GetValueNames())
            {
                if (ValueName == "management_basefolder")
                {
                    FilesPanel.txtServerShare.Text = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Lanstaller").GetValue("management_basefolder", "").ToString();
                }
            }


            RegistryPanel.cmbxHiveKey.SelectedIndex = 0;
            RegistryPanel.cmbxType.SelectedIndex = 0;



            //Files.
            FilesPanel.txtScanfolder.TextChanged += new System.EventHandler(this.txtScanfolder_TextChanged);
            FilesPanel.txtDestination.TextChanged += new System.EventHandler(this.txtDestination_TextChanged);
            FilesPanel.txtSubFolder.TextChanged += new System.EventHandler(this.txtSubFolder_TextChanged);
            FilesPanel.txtServerShare.TextChanged += new System.EventHandler(this.txtServerShare_TextChanged);
            FilesPanel.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            FilesPanel.btnAddFolder.Click += new System.EventHandler(this.btnAddFolder_Click);
            FilesPanel.btnClear.Click += new EventHandler(this.btnClear_Click);


            //FilesPanel.btnGenerateNewFilehashes.Click += new System.EventHandler();
            FilesPanel.btnRescanFileHash.Click += new System.EventHandler(this.btnRescanFileHash_Click);
            FilesPanel.btnRescanFileSize.Click += new System.EventHandler(this.btnRescanFileSize_Click);
            FilesPanel.btnCheckAllFiles.Click += new System.EventHandler(this.btnCheckAllFiles_Click);


            //Preferences
            PreferencesPanel.btnAddPrefFile.Click += new System.EventHandler(this.btnAddPrefFile_Click);


            //Registry
            RegistryPanel.btnAddReg.Click += new System.EventHandler(this.btnAddReg_Click);
            RegistryPanel.txtData.TextChanged += new System.EventHandler(this.txtData_TextChanged);
            RegistryPanel.txtKey.TextChanged += new System.EventHandler(this.txtKey_TextChanged);
            RegistryPanel.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);

            //Serials
            SerialPanel.btnAddSerial.Click += new System.EventHandler(this.btnAddSerial_Click);
            SerialPanel.txtSerialName.TextChanged += new System.EventHandler(this.txtSerialName_TextChanged);

            //User Serials - Serial Pool.
            SerialPanel.cmbxSerialPoolInstance.SelectedIndexChanged += new System.EventHandler(this.cmbxSerialPoolInstance_SelectedIndexChanged);
            SerialPanel.btnAddUserSerial.Click += new System.EventHandler(this.btnAddUserSerial_Click);
            SerialPanel.btnDelUserSerial.Click += new System.EventHandler(this.btnDelUserSerial_Click);
            SerialPanel.lvUserSerials.SelectedIndexChanged += new System.EventHandler(this.lvUserSerials_SelectedIndexChanged);

            //Shortcuts
            ShortcutsPanel.btnAddShortcut.Click += new System.EventHandler(this.btnAddShortcut_Click);
            ShortcutsPanel.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);


            //Windows:
            //Firewall
            WindowsSettingsPanel.btnFirewallRuleAdd.Click += new System.EventHandler(this.btnFirewallRuleAdd_Click);
            WindowsSettingsPanel.txtFirewallPath.TextChanged += new System.EventHandler(this.txtFirewallPath_TextChanged);

            //Redist
            LoadRedist();
            WindowsSettingsPanel.btnAddRedist.Click += new System.EventHandler(this.btnAddRedist_Click);



        }


        void LoadRedist()
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLCmd.CommandText = "SELECT ID,Name FROM [tblRedist]";

            SQLConn.Open();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            while (SR.Read())
            {
                Redistributable RED = new Redistributable();
                RED.id = (int)SR[0];
                RED.name = SR[1].ToString();
                RedistList.Add(RED);
                WindowsSettingsPanel.cmbxRedist.Items.Add(RED.name);
            }
            SQLConn.Close();

        }

        void RefreshSoftware()
        {
            //Full software reload.
            SoftwareList = SoftwareInfo.LoadSoftware();

            //for updating listview from local SoftwareList counts.
            lvSoftware.Items.Clear();
            foreach (SoftwareInfo SW in SoftwareList)
            {
                ListViewItem lvi = new ListViewItem(SW.Name);
                lvi.SubItems.Add(SW.file_count.ToString());
                lvi.SubItems.Add(SW.registry_count.ToString());
                lvi.SubItems.Add(SW.shortcut_count.ToString());
                lvi.SubItems.Add(SW.firewall_count.ToString());

                lvSoftware.Items.Add(lvi);
            }



        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            string softwarename = Interaction.InputBox("Name:");
            if (softwarename.Equals(""))
            {
                MessageBox.Show("Error - Nothing in name.");
                return;
            }
            int newid = SoftwareInfo.AddSoftware(softwarename);

            RefreshSoftware();
            int index = 0;
            foreach (SoftwareInfo SW in SoftwareList)
            {
                if (SW.id == newid)
                {
                    lvSoftware.SelectedItems.Clear();
                    lvSoftware.Items[index].Selected = true;
                    return;
                }
                index++;
            }
        }


        private void btnClear_Click(object sender,EventArgs e)
        {
            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("No software selected");
                return;
            }

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            
            SQLCmd.Parameters.AddWithValue("@swid", CurrentSelectedSoftware.id);

            SQLConn.Open();
            SQLCmd.CommandText = "DELETE from tblFiles WHERE software_id = @swid";
            SQLCmd.ExecuteNonQuery();
            SQLCmd.CommandText = "DELETE from tblDirectories WHERE software_id = @swid";
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();

            RefreshSoftware();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            //Cleanup path ends.
            if (!FilesPanel.txtServerShare.Text.EndsWith("\\"))
            {
                FilesPanel.txtServerShare.Text += "\\";
            }
            if (!FilesPanel.txtScanfolder.Text.EndsWith("\\"))
            {
                FilesPanel.txtScanfolder.Text += "\\";
            }
            if (!FilesPanel.txtSubFolder.Text.Equals("") && !FilesPanel.txtSubFolder.Text.EndsWith("\\"))
            {
                FilesPanel.txtSubFolder.Text += "\\";
            }

            string scanfolder = Path.Combine(FilesPanel.txtServerShare.Text, FilesPanel.txtScanfolder.Text);
            if (Directory.Exists(scanfolder) == false)
            {
                MessageBox.Show("Scan Folder - Invalid");
                return;
            }

            FilesPanel.btnScan.Enabled = false;

            FilesPanel.lblCopyActionInfo.Text = "Status: Scanning";
            filelist = Directory.GetFiles(scanfolder, "*", SearchOption.AllDirectories);
            directories = Directory.GetDirectories(scanfolder, "*", SearchOption.AllDirectories);
            
            FilesPanel.lblCopyActionInfo.Text = "Status: Scanned Files: " + filelist.Count() + "\nDirectories: " + directories.Count();
            FilesPanel.btnAddFolder.Enabled = true;

            FilesPanel.btnScan.Enabled = true;
        }

        private void btnCheckAllFiles_Click(object sender, EventArgs e)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();
            SQLCmd.CommandText = "SELECT address from tblServers where [type] = 'smb'";
            string serveradd = SQLCmd.ExecuteScalar().ToString();
            if (!serveradd.EndsWith("\\"))
            {
                serveradd = serveradd + "\\";
            }

            SQLCmd.CommandText = "SELECT id,source from tblFiles WHERE software_id = @swid";
            SQLCmd.Parameters.AddWithValue("@swid", CurrentSelectedSoftware.id);
            SqlDataReader SR = SQLCmd.ExecuteReader();
            List<string> OutputList = new List<string>();
            int filecount = 0;
            while (SR.Read())
            {
                filecount++;
                if (File.Exists(serveradd + SR["source"].ToString()) == false)
                {
                    OutputList.Add(SR["id"].ToString() + "," + SR["source"].ToString());
                }
            }
            SQLConn.Close();


            if (OutputList.Count > 0)
            {
                StreamWriter SW = new StreamWriter("missingfiles.txt");
                foreach (string Output in OutputList)
                {
                    SW.WriteLine(Output);
                }
                SW.Close();
                MessageBox.Show("Missing files found - logged list to missingfiles.txt");
            }
            else
            {
                MessageBox.Show("No missing files - checked: " + filecount.ToString());
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("No software ID.");
                return;
            }

            FilesPanel.btnAddFolder.Enabled = false;

            string destination = FilesPanel.txtDestination.Text;
            if (destination == "")
            {
                destination = "%INSTALLPATH%\\";
            }

            if (!(destination.EndsWith("\\")))
            {
                MessageBox.Show("Destination must end with Backslash (\\)");
                return;
            }


            string servershare = FilesPanel.txtServerShare.Text;
            string subfolder = servershare + FilesPanel.txtSubFolder.Text;


            foreach (string dirname in directories)
            {
                string dst = destination + dirname.Substring(subfolder.Length);
                FileInfoClass.AddDirectory(dst, CurrentSelectedSoftware.id);

            }


            //Loop through each file from .getfiles.
            foreach (string filename in filelist)
            {
                //Trim Base Line + last \


                //Scan folder: \\mrh-nas1\games\Games Source\CNC3\CNC3KW
                //Base folder: \\mrh-nas1\games\Games Source\CNC3
                //Server share: \\mrh-nas1\Games\Games Source

                //Example:
                //filename = \\mrh-nas1\games\Games Source\CNC3\CNC3KW\test.exe
                //dst = CNC3KW\test.exe
                //src = CNC3\CNC3KW\test.exe

                string src = filename.Substring(servershare.Length);
                string dst = destination + filename.Substring(subfolder.Length);


                //Messagebox for each file:
                //MessageBox.Show(src + Environment.NewLine + dst);

                FileInfo FI = new FileInfo(filename);
                FileInfoClass.AddFile(src, dst, FI.Length, CurrentSelectedSoftware.id);

            }
            RefreshSoftware();

            RunFilehashThread(false, CurrentSelectedSoftware.id);

            FilesPanel.btnScan.Enabled = true;

        }

        void RefreshStatusInfo()
        {
            SoftwareInfo.UpdateSoftwareCounts(CurrentSelectedSoftware); //Update counts.
            string info = "";
            info += "File Copies: " + CurrentSelectedSoftware.file_count + Environment.NewLine;
            info += "Registry Operations: " + CurrentSelectedSoftware.registry_count + Environment.NewLine;
            info += "Shortcuts: " + CurrentSelectedSoftware.shortcut_count + Environment.NewLine;
            info += "Firewall Rules: " + CurrentSelectedSoftware.firewall_count + Environment.NewLine;
            lblInstallInfo.Text = "Installation Info:" + Environment.NewLine + info;
        }

        private void txtServerShare_TextChanged(object sender, EventArgs e)
        {
            Registry.CurrentUser.OpenSubKey("SOFTWARE\\Lanstaller", true).SetValue("management_basefolder", FilesPanel.txtServerShare.Text);
        }

        public void txtScanfolder_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(FilesPanel.txtServerShare.Text, FilesPanel.txtScanfolder.Text)) == true)
            {
                FilesPanel.txtScanfolder.BackColor = Color.White;
            }
            else
            {
                FilesPanel.txtScanfolder.BackColor = Color.Yellow;
                return;
            }

            if (FilesPanel.txtScanfolder.Text.Contains("\\") && FilesPanel.txtServerShare.Text != "")
            {
                //sub-directory path inbetween server-share and scan folder.
                string scanpath = FilesPanel.txtScanfolder.Text;
                if (scanpath.EndsWith("\\"))
                {
                    scanpath = scanpath.Substring(0, scanpath.Length - 1);
                }

                string scanroot = FilesPanel.txtScanfolder.Text.Substring(0, scanpath.LastIndexOf("\\") + 1); //offset +1 to include \

                if (FilesPanel.txtServerShare.Text.Length < scanroot.Length)
                {
                    FilesPanel.txtSubFolder.Text = scanroot.Substring(FilesPanel.txtServerShare.Text.Length);
                }

            }

            UpdateLabels();

        }

        private void btnAddReg_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("Invalid software id");
                return;
            }

            //HKEY SQL Values:
            //1 = Local Machine.
            //2 = Current User.
            //3 = Users.
            int hkeyval;
            if (RegistryPanel.cmbxHiveKey.Text == "HKEY_LOCAL_MACHINE")
            {
                hkeyval = 1;
            }
            else if (RegistryPanel.cmbxHiveKey.Text == "HKEY_CURRENT_USER")
            {
                hkeyval = 2;
            }
            else if (RegistryPanel.cmbxHiveKey.Text == "HKEY_USERS")
            {
                hkeyval = 3;
            }
            else
            {
                MessageBox.Show("Select Reg Hive Key");
                return;
            }

            int regtype;
            if (RegistryPanel.cmbxType.Text == "STRING")
            {
                regtype = 1;
            }
            else if (RegistryPanel.cmbxType.Text == "DWORD")
            {
                regtype = 4;
            }
            else
            {
                MessageBox.Show("Select Reg Type");
                return;
            }
            //Registry Type SQL Values:
            //string = 1
            //binary = 3
            //dword = 4
            //expanded string = 2
            //multi string = 7
            //qword = 11
            RegistryPanel.btnAddReg.Enabled = false;
            RegistryOperation.AddRegistry(CurrentSelectedSoftware.id, hkeyval, RegistryPanel.txtKey.Text, RegistryPanel.txtValue.Text, regtype, RegistryPanel.txtData.Text);

            RefreshSoftware();

        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            RegistryPanel.btnAddReg.Enabled = true;
        }

        private void btnAddShortcut_Click(object sender, EventArgs e)
        {

            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("Select Software");
                return;
            }
            ShortcutsPanel.btnAddShortcut.Enabled = false;
            ShortcutOperation.AddShortcut(ShortcutsPanel.txtName.Text, ShortcutsPanel.txtLocation.Text, ShortcutsPanel.txtFilepath.Text, ShortcutsPanel.txtWorking.Text, ShortcutsPanel.txtArguments.Text, ShortcutsPanel.txtIcon.Text, CurrentSelectedSoftware.id);

            RefreshSoftware();

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            ShortcutsPanel.btnAddShortcut.Enabled = true;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            RegistryPanel.btnAddReg.Enabled = true;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            RegistryPanel.btnAddReg.Enabled = true;
        }

        private void txtFilepath_TextChanged(object sender, EventArgs e)
        {
            ShortcutsPanel.txtIcon.Text = ShortcutsPanel.txtFilepath.Text;

            if (ShortcutsPanel.txtFilepath.Text.Contains("\\"))
            {
                ShortcutsPanel.txtWorking.Text = ShortcutsPanel.txtFilepath.Text.Substring(0, ShortcutsPanel.txtFilepath.Text.LastIndexOf("\\"));
            }
        }

        private void txtBaseFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }



        private void txtDestination_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void txtSubFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }


        void UpdateLabels()
        {
            if (!FilesPanel.txtServerShare.Text.EndsWith("\\"))
            {
                FilesPanel.lblCopyActionInfo.Text = "Server Share must end with \\";
                return;
            }
            else if (!FilesPanel.txtScanfolder.Text.EndsWith("\\"))
            {
                FilesPanel.lblCopyActionInfo.Text = "Scan folder must end with \\";
                return;
            }
            else if (!FilesPanel.txtSubFolder.Text.Equals("") && !FilesPanel.txtSubFolder.Text.EndsWith("\\"))
            {
                FilesPanel.lblCopyActionInfo.Text = "Subfolder must end with \\";
                return;
            }

            string destdir = FilesPanel.txtDestination.Text;
            if (FilesPanel.txtDestination.Text.Equals(""))
            {
                destdir = "%INSTALLPATH%\\";
            }

            try
            {
                string srcpath = Path.Combine(FilesPanel.txtServerShare.Text, FilesPanel.txtScanfolder.Text) + "example.exe";
                string dstfile = Path.Combine(destdir, FilesPanel.txtScanfolder.Text.Substring(FilesPanel.txtSubFolder.Text.Length)) + "example.exe";
                FilesPanel.lblCopyActionInfo.Text = "Copy files from: " + srcpath + Environment.NewLine +
                    "to: " + dstfile;
            }
            catch
            {
                FilesPanel.lblCopyActionInfo.Text = "Error";
            }
           
        }

        private void btnFirewallRuleAdd_Click(object sender, EventArgs e)
        {
            WindowsSettingsPanel.btnFirewallRuleAdd.Enabled = false;
            FirewallRule.AddFirewallRule(WindowsSettingsPanel.txtFirewallPath.Text, WindowsSettingsPanel.txtRuleName.Text, CurrentSelectedSoftware.id);
            RefreshSoftware();

        }

        private void txtFirewallPath_TextChanged(object sender, EventArgs e)
        {
            WindowsSettingsPanel.btnFirewallRuleAdd.Enabled = true;
        }


        private void btnAddRedist_Click(object sender, EventArgs e)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLCmd.CommandText = "INSERT into tblRedistUsage ([redist_id],[software_id]) VALUES (@redistid,@softwareid)";
            SQLConn.Open();
            SQLCmd.Parameters.AddWithValue("@redistid", RedistList[WindowsSettingsPanel.cmbxRedist.SelectedIndex].id);
            SQLCmd.Parameters.AddWithValue("@softwareid", CurrentSelectedSoftware.id);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }


        private void btnAddSerial_Click(object sender, EventArgs e)
        {
            SerialNumber.AddSerial(SerialPanel.txtSerialName.Text, int.Parse(SerialPanel.txtSerialInstance.Text), CurrentSelectedSoftware.id, SerialPanel.txtRegKey.Text, SerialPanel.txtRegVal.Text, SerialPanel.txtFormat.Text);
        }

        private void txtSerialName_TextChanged(object sender, EventArgs e)
        {
            if (!SerialPanel.txtSerialName.Text.Equals(""))
                SerialPanel.btnAddSerial.Enabled = true;
        }


        private void cmbxSerialPoolInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            SerialPanel.btnAddUserSerial.Enabled = false;
            SerialPanel.btnDelUserSerial.Enabled = false;
            serialid_pool_selected = 0;
            if (SerialPanel.cmbxSerialPoolInstance.SelectedIndex == -1) return; //No serials to select for software, exit.

            //Load serial list for selected serial.
            int instance_index = SerialPanel.cmbxSerialPoolInstance.SelectedIndex;
            serialid_pool_selected = SerialNumber.GetSerials(CurrentSelectedSoftware.id)[instance_index].serialid;
            RefreshSerialPoolList(); //Load serial pool into listview.
            SerialPanel.btnAddUserSerial.Enabled = true;

        }

        private void RefreshSerialPoolList()
        {
            SerialPanel.lvUserSerials.Items.Clear();
            foreach (UserSerial SN in UserSerial.GetUserSerials(serialid_pool_selected))
            {
                //MessageBox.Show(SN.name);
                ListViewItem LVI = new ListViewItem(SN.serial);
                LVI.SubItems.Add(SN.used_timestamp);
                LVI.Tag = SN.id;

                SerialPanel.lvUserSerials.Items.Add(LVI);
            }
        }


        private void btnAddUserSerial_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(SerialPanel.txtUserSerial.Text)) return; //Skip empty input.
            //add serial to pool.
            string serial_value = SerialNumber.FilterSerial(SerialPanel.txtUserSerial.Text);

            UserSerial.AddAvailableSerial(serialid_pool_selected, serial_value);
            SerialPanel.txtUserSerial.Text = serial_value;
            RefreshSerialPoolList();
            SerialPanel.txtUserSerial.Text = string.Empty;
        }

        private void btnDelUserSerial_Click(object sender, EventArgs e)
        {
            if (SerialPanel.lvUserSerials.SelectedItems.Count == 0) return;
            //Delete serial from pool.
            int userserialid = (int)SerialPanel.lvUserSerials.SelectedItems[0].Tag;
            UserSerial.DeleteAvailableSerial(userserialid);
            RefreshSerialPoolList();
        }



        private void lvUserSerials_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SerialPanel.lvUserSerials.SelectedItems.Count == 0) return;

            SerialPanel.btnDelUserSerial.Enabled = true;

        }

        private void btnRescanFileSize_Click(object sender, EventArgs e)
        {
            FilesPanel.btnRescanFileSize.Enabled = false;
            LanstallerManagement.RescanFileSize(CurrentSelectedSoftware.id);
            FilesPanel.btnRescanFileSize.Enabled = true;
        }



        private void btnAddPrefFile_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("Select Software");
                return;
            }
            PreferenceOperation.AddPreferenceFile(PreferencesPanel.txtPrefFilePath.Text, PreferencesPanel.txtTarget.Text, PreferencesPanel.txtReplace.Text, CurrentSelectedSoftware.id);
        }



        private void btnRescanFileHash_Click(object sender, EventArgs e)
        {
            FilesPanel.btnRescanFileHash.Enabled = false;

            int scanid = 0;
            if (MessageBox.Show("All software?", "Scan Option", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                if (CurrentSelectedSoftware == null)
                {
                    MessageBox.Show("No software selected.");

                    //re-enable button
                    FilesPanel.btnRescanFileHash.Invoke((MethodInvoker)delegate
                    {
                        FilesPanel.btnRescanFileHash.Enabled = true;
                    });

                    return;
                }
                scanid = CurrentSelectedSoftware.id;
            }

            if (MessageBox.Show("Verify only the unhashed files?", "Scan Option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RunFilehashThread(false, scanid);
            }
            else
            {
                RunFilehashThread(true, scanid);
            }
        }

        void RunFilehashThread(bool fullRescan, int ScanId)
        {
            //GetUnhashedFileCount
            Thread ST = new Thread(t => GenerateFileHash(fullRescan, ScanId));
            ST.Name = "File Hashing";
            ST.Start();
        }

        void GenerateFileHash(bool fullRescan, int scanid)
        {
            FileInfoClass.RescanFileHashes(fullRescan, scanid);

            FilesPanel.lblCopyActionInfo.Invoke((MethodInvoker)delegate
            {
                // Running on the UI thread
                FilesPanel.lblCopyActionInfo.Visible = false;
            });

            FilesPanel.btnRescanFileHash.Invoke((MethodInvoker)delegate
            {
                FilesPanel.btnRescanFileHash.Enabled = true;
            });
        }


        private void btnAuthNew_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvSoftware.SelectedItems[0].Index == -1)
            {
                return;
            }
            //Delete Software.
            SoftwareInfo.DeleteSoftware(SoftwareList[lvSoftware.SelectedItems[0].Index].id);

            RefreshSoftware();

        }

        private void btnSecurity_Click(object sender, EventArgs e)
        {
            frmSecurity fS = new frmSecurity();
            fS.Show();
        }

        private void tmrProgress_Tick(object sender, EventArgs e)
        {
            FilesPanel.lblUnhashedFiles.Text = "Unhashed Files:" + FileInfoClass.GetUnhashedFileCount();
        }

        private void lvSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSoftware.SelectedItems.Count == 0)
            {
                return;
            }
            CurrentSelectedSoftware = SoftwareList[lvSoftware.SelectedItems[0].Index];

            SerialPanel.txtSerialName.Text = SoftwareList[lvSoftware.SelectedItems[0].Index].Name;
            SerialPanel.txtSerialInstance.Text = "1";

            RefreshStatusInfo();

            //load serial panel details.
            SerialPanel.cmbxSerialPoolInstance.Items.Clear();
            foreach (SerialNumber SN in SerialNumber.GetSerials(CurrentSelectedSoftware.id))
            {
                SerialPanel.cmbxSerialPoolInstance.Items.Add(SN.name);
            }

            foreach (Redistributable RS in Redistributable.GetRedistributables(CurrentSelectedSoftware.id))
            {
                WindowsSettingsPanel.cmbxRedist.Items.Add(RS.name);
            }
        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            if (lvSoftware.SelectedItems.Count != 1)
            {
                MessageBox.Show("Select Software");
                return;
            }

            string baseshare = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Lanstaller").GetValue("management_basefolder", "").ToString();
            if (string.IsNullOrEmpty(baseshare))
            {
                MessageBox.Show("Images directory not found, Set server path in files section.");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*"
            };
            openFileDialog.ShowDialog();

            string tmpFile = Path.GetTempPath() + "\\lanstallericontmp.exe";
            if (File.Exists(tmpFile))
            {
                File.Delete(tmpFile);
            }
            File.Copy(openFileDialog.FileName, tmpFile);
            Icon icon = Icon.ExtractAssociatedIcon(tmpFile);
            Bitmap bitmap = icon.ToBitmap();
            string saveFile = baseshare + "\\Images\\" + CurrentSelectedSoftware.Name + ".png";
            if (File.Exists(saveFile))
            {
                File.Delete(saveFile);
            }
            try
            {
                bitmap.Save(saveFile, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem saving image file: " + ex.Message);
                return;
            }

            File.Delete(tmpFile);

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLCmd.CommandText = "IF EXISTS (SELECT software_id FROM tblImages WHERE software_id = @swid) " +
                "UPDATE tblImages SET small_image = @savefile WHERE software_id = @swid " +
                "ELSE INSERT INTO tblImages (software_id,small_image) VALUES (@swid,@savefile)";
            SQLCmd.Parameters.AddWithValue("@swid", CurrentSelectedSoftware.id);
            SQLCmd.Parameters.AddWithValue("@savefile", "Images/" + CurrentSelectedSoftware.Name + ".png");

            SQLConn.Open();
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();


        }
    }
}
