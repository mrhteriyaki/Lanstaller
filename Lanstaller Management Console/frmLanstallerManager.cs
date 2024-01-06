using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//using Lanstaller;
using Lanstaller_Shared;
using Microsoft.VisualBasic;
using Microsoft.Win32;

using System.IO;

namespace Lanstaller_Management_Console
{
    public partial class frmLanstallerManager : Form
    {
        List<SoftwareClass.SoftwareInfo> SoftwareList = new List<SoftwareClass.SoftwareInfo>();
        SoftwareClass.SoftwareInfo CurrentSelectedSoftware;

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
            lvSoftware.Columns.Add("Software",260);
            lvSoftware.Columns.Add("Files",60);
            lvSoftware.Columns.Add("Registry",70);
            lvSoftware.Columns.Add("Shortcuts", 70);
            lvSoftware.Columns.Add("Firewall Rules",80);

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
                    SoftwareClass.ConnectionString = line;
                }
            }


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


            foreach (string val in Registry.CurrentUser.OpenSubKey("SOFTWARE\\Lanstaller", true).GetValueNames())
            {
                if (val == "management_basefolder")
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


            //Firewall
            WindowsSettingsPanel.btnFirewallRuleAdd.Click += new System.EventHandler(this.btnFirewallRuleAdd_Click);
            WindowsSettingsPanel.txtFirewallPath.TextChanged += new System.EventHandler(this.txtFirewallPath_TextChanged);


        }

        void RefreshSoftware()
        {
            //Full software reload.
            SoftwareList = SoftwareClass.LoadSoftware();

            //for updating listview from local SoftwareList counts.
            lvSoftware.Items.Clear();
            foreach (SoftwareClass.SoftwareInfo SW in SoftwareList)
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
            }
            int newid = SoftwareClass.AddSoftware(softwarename);

            RefreshSoftware();
            int index = 0;
            foreach (SoftwareClass.SoftwareInfo SW in SoftwareList)
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


        string[] filelist;
        string[] directories;

        private void btnScan_Click(object sender, EventArgs e)
        {
            //Cleanup path ends.
            if (!(FilesPanel.txtScanfolder.Text.EndsWith("\\")))
            {
                MessageBox.Show("Scan path must end with backslash.");
                return;
            }

            if (!(FilesPanel.txtSubFolder.Text.Equals("")))
            {
                if (!(FilesPanel.txtSubFolder.Text.EndsWith("\\")))
                {
                    MessageBox.Show("Sub folder path must end with backslash.");
                    return;
                }
            }

            if (FilesPanel.txtSubFolder.Text.StartsWith("\\"))
            {
                MessageBox.Show("Sub Folder path cannot start with backslash");
                return;
            }

            if (FilesPanel.txtServerShare.Text != "" && !(FilesPanel.txtServerShare.Text.EndsWith("\\")))
            {
                FilesPanel.txtServerShare.Text = FilesPanel.txtServerShare.Text + "\\"; //append backslash to server share location..
            }

            //check server share location matches start of scan path.
            if (!(FilesPanel.txtScanfolder.Text.ToLower().StartsWith(FilesPanel.txtServerShare.Text.ToLower())))
            {
                MessageBox.Show("Scan folder not located within share path");
                return;
            }

            FilesPanel.btnScan.Enabled = false;
            string scanfolder = FilesPanel.txtScanfolder.Text;

            if (Pri.LongPath.Directory.Exists(scanfolder) == false)
            {
                MessageBox.Show("Scan Folder - Invalid");
                FilesPanel.btnScan.Enabled = true;
                return;
            }

            FilesPanel.lblCopyActionInfo.Text = "Status: Scanning";
            filelist = Pri.LongPath.Directory.GetFiles(scanfolder, "*", System.IO.SearchOption.AllDirectories);
            directories = Pri.LongPath.Directory.GetDirectories(scanfolder, "*", SearchOption.AllDirectories);

            FilesPanel.lblCopyActionInfo.Text = "Status: Scanned Files: " + filelist.Count() + "\nDirectories: " + directories.Count();
            FilesPanel.btnAddFolder.Enabled = true;
        }

        private void btnCheckAllFiles_Click(object sender, EventArgs e)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();
            SQLCmd.CommandText = "SELECT address from tblServers where [type] = 'smb'";
            string serveradd = SQLCmd.ExecuteScalar().ToString();
            if (!serveradd.EndsWith("\\"))
            {
                serveradd = serveradd + "\\";
            }

            SQLCmd.CommandText = "SELECT id,source from tblFiles";
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

            if (FilesPanel.txtScanfolder.Text.ToLower().StartsWith(FilesPanel.txtServerShare.Text.ToLower() + FilesPanel.txtSubFolder.Text.ToLower()) == false)
            {
                MessageBox.Show("Base folder must be part of scan folder.");
                return;
            }

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
                SoftwareClass.AddDirectory(dst, CurrentSelectedSoftware.id);

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

                Pri.LongPath.FileInfo FI = new Pri.LongPath.FileInfo(filename);
                SoftwareClass.AddFile(src, dst, FI.Length, CurrentSelectedSoftware.id);

            }
            RefreshSoftware(); 
            FilesPanel.btnScan.Enabled = true;

        }

        void RefreshStatusInfo()
        {
            SoftwareClass.LoadSoftwareCounts(CurrentSelectedSoftware); //Update counts.
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
            if (Directory.Exists(FilesPanel.txtScanfolder.Text) == true)
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
            SoftwareClass.AddRegistry(CurrentSelectedSoftware.id, hkeyval, RegistryPanel.txtKey.Text, RegistryPanel.txtValue.Text, regtype, RegistryPanel.txtData.Text);

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
            SoftwareClass.AddShortcut(ShortcutsPanel.txtName.Text, ShortcutsPanel.txtLocation.Text, ShortcutsPanel.txtFilepath.Text, ShortcutsPanel.txtWorking.Text, ShortcutsPanel.txtArguments.Text, ShortcutsPanel.txtIcon.Text, CurrentSelectedSoftware.id);

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

            string destdir = FilesPanel.txtDestination.Text;
            if (FilesPanel.txtDestination.Text.Equals(""))
            {
                destdir = "%INSTALLPATH%\\";
            }

            int length = FilesPanel.txtServerShare.TextLength + FilesPanel.txtSubFolder.TextLength;

            if (length <= FilesPanel.txtScanfolder.Text.Length)
            {
                string dstpath = FilesPanel.txtScanfolder.Text.Substring(length);
                string srcpath = FilesPanel.txtScanfolder.Text + "example.exe";
                string dstfile = destdir + dstpath + "example.exe";
                FilesPanel.lblCopyActionInfo.Text = "Copy files from: " + srcpath + Environment.NewLine +
                    "to: " + dstfile;
            }
            else
            {
                FilesPanel.lblCopyActionInfo.Text = "Error";
            }




        }

        private void btnFirewallRuleAdd_Click(object sender, EventArgs e)
        {
            WindowsSettingsPanel.btnFirewallRuleAdd.Enabled = false;
            SoftwareClass.AddFirewallRule(WindowsSettingsPanel.txtFirewallPath.Text, CurrentSelectedSoftware.id);
            RefreshSoftware();

        }

        private void txtFirewallPath_TextChanged(object sender, EventArgs e)
        {
            WindowsSettingsPanel.btnFirewallRuleAdd.Enabled = true;
        }




        private void btnAddSerial_Click(object sender, EventArgs e)
        {
            string filtered_serial = SoftwareClass.SerialNumber.FilterSerial(SerialPanel.txtSerialName.Text);
            SerialPanel.txtSerialName.Text = filtered_serial;
            SoftwareClass.AddSerial(filtered_serial, int.Parse(SerialPanel.txtSerialInstance.Text), CurrentSelectedSoftware.id, SerialPanel.txtRegKey.Text, SerialPanel.txtRegVal.Text, SerialPanel.txtFormat.Text);
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
            if (SerialPanel.cmbxSerialPoolInstance.SelectedIndex == 1) return; //No serials to select for software, exit.

            //Load serial list for selected serial.
            int instance_index = SerialPanel.cmbxSerialPoolInstance.SelectedIndex;
            serialid_pool_selected = SoftwareClass.GetSerials(CurrentSelectedSoftware.id)[instance_index].serialid;
            RefreshSerialPoolList(); //Load serial pool into listview.
            SerialPanel.btnAddUserSerial.Enabled = true;
           
        }

        private void RefreshSerialPoolList()
        {
            SerialPanel.lvUserSerials.Items.Clear();
            foreach (SoftwareClass.UserSerial SN in SoftwareClass.UserSerial.GetUserSerials(serialid_pool_selected))
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
            string serial_value = SoftwareClass.SerialNumber.FilterSerial(SerialPanel.txtUserSerial.Text);
            
            SoftwareClass.UserSerial.AddAvailableSerial(serialid_pool_selected, serial_value);
            SerialPanel.txtUserSerial.Text = serial_value;
            RefreshSerialPoolList();
        }                    

        private void btnDelUserSerial_Click(object sender, EventArgs e)
        {
            if (SerialPanel.lvUserSerials.SelectedItems.Count == 0) return;
            //Delete serial from pool.
            int userserialid = (int)SerialPanel.lvUserSerials.SelectedItems[0].Tag;
            SoftwareClass.UserSerial.DeleteAvailableSerial(userserialid);
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
            LanstallerManagement.RescanFileSize();
            FilesPanel.btnRescanFileSize.Enabled = true;
        }



        private void btnAddPrefFile_Click(object sender, EventArgs e)
        {
            if (CurrentSelectedSoftware == null)
            {
                MessageBox.Show("Select Software");
                return;
            }
            SoftwareClass.AddPreferenceFile(PreferencesPanel.txtPrefFilePath.Text, PreferencesPanel.txtTarget.Text, PreferencesPanel.txtReplace.Text, CurrentSelectedSoftware.id);
        }



        private void btnRescanFileHash_Click(object sender, EventArgs e)
        {
            FilesPanel.btnRescanFileHash.Enabled = false;
            
            //GetUnhashedFileCount
            Thread ST = new Thread(GenerateFileHash);
            ST.Name = "File Hashing";
            ST.Start();

        }

        void GenerateFileHash()
        {
            if (MessageBox.Show("Also verify existing hashes?", "Scan Option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SoftwareClass.RescanFileHashes(true);
            }
            else
            {
                SoftwareClass.RescanFileHashes(false);
            }

            FilesPanel.lblCopyActionInfo.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                FilesPanel.lblCopyActionInfo.Visible = false;
            });

            FilesPanel.btnRescanFileHash.Invoke((MethodInvoker)delegate
            {
                FilesPanel.btnRescanFileHash.Enabled = true;
            });


        }

        private void button1_Click(object sender, EventArgs e)
        {

            SoftwareClass.RescanFileHashes(false);
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
            SoftwareClass.DeleteSoftware(SoftwareList[lvSoftware.SelectedItems[0].Index].id);

            RefreshSoftware();

        }

        private void btnSecurity_Click(object sender, EventArgs e)
        {
            frmSecurity fS = new frmSecurity();
            fS.Show();
        }

        private void tmrProgress_Tick(object sender, EventArgs e)
        {
            FilesPanel.lblUnhashedFiles.Text = "Unhashed Files:" + SoftwareClass.GetUnhashedFileCount();
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
            foreach (SoftwareClass.SerialNumber SN in SoftwareClass.GetSerials(CurrentSelectedSoftware.id))
            {
                SerialPanel.cmbxSerialPoolInstance.Items.Add(SN.name);
            }

        }
    }
}
