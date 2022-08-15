using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        int selectedsoftwareid = -1;
        List<SoftwareClass.SoftwareInfo> SoftwareList = new List<SoftwareClass.SoftwareInfo>();
        //static string status = "Status: Ready";

        //Declare Panels.
        Panels.Files FilesPanel = new Panels.Files();
        Panels.Preferences PreferencesPanel = new Panels.Preferences();
        Panels.Registry RegistryPanel = new Panels.Registry();
        Panels.Serial SerialPanel = new Panels.Serial();
        Panels.Shortcuts ShortcutsPanel = new Panels.Shortcuts();
        Panels.Windows_Settings WindowsSettingsPanel = new Panels.Windows_Settings();

        int panel_button_x = 300;



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


            foreach (string val in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller", true).GetValueNames())
            {
                if (val == "management_basefolder")
                {
                    FilesPanel.txtServerShare.Text = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller").GetValue("management_basefolder", "").ToString();
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
            //SerialPanel.txtRegKey.TextChanged += new System.EventHandler(this.
            SerialPanel.txtSerialName.TextChanged += new System.EventHandler(this.txtSerialName_TextChanged);


            //Shortcuts
            ShortcutsPanel.btnAddShortcut.Click += new System.EventHandler(this.btnAddShortcut_Click);
            ShortcutsPanel.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);


            //Firewall
            WindowsSettingsPanel.btnFirewallRuleAdd.Click += new System.EventHandler(this.btnFirewallRuleAdd_Click);
            WindowsSettingsPanel.txtFirewallPath.TextChanged += new System.EventHandler(this.txtFirewallPath_TextChanged);


        }

        void RefreshSoftware()
        {
            SoftwareList = SoftwareClass.LoadSoftware();
            lbxSoftware.Items.Clear();
            foreach (SoftwareClass.SoftwareInfo SW in SoftwareList)
            {
                lbxSoftware.Items.Add(SW.Name);
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
                    lbxSoftware.SelectedIndex = index;
                    return;
                }
                index++;
            }
        }


        string[] filelist;
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
            FilesPanel.lblCopyActionInfo.Text = "Status: Scanned Files: " + filelist.Count();
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

        private void lbxSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSoftware.SelectedIndex == -1)
            {
                return;
            }

            selectedsoftwareid = SoftwareList[lbxSoftware.SelectedIndex].id;

            SerialPanel.txtSerialName.Text = SoftwareList[lbxSoftware.SelectedIndex].Name;
            SerialPanel.txtSerialInstance.Text = "1";

            UpdateInfo();

        }

        void UpdateInfo()
        {
            //Get info for install./


            string info = "";
            //GetScalarInfo("WHERE software_id = @softwareid", selectedsoftwareid);
            int filecount = GetScalarInfo("SELECT COUNT(id) from tblFiles where software_id = @softwareid", selectedsoftwareid);
            info += "File Copies: " + filecount.ToString() + Environment.NewLine;

            int firewallcount = GetScalarInfo("SELECT COUNT(id) from tblFirewallExceptions WHERE software_id = @softwareid", selectedsoftwareid);
            info += "Firewall Rules: " + firewallcount.ToString() + Environment.NewLine;

            int registrycount = GetScalarInfo("SELECT COUNT(id) from tblRegistry WHERE software_id = @softwareid", selectedsoftwareid);
            info += "Registry Operations: " + registrycount.ToString() + Environment.NewLine;

            int shortcutcount = GetScalarInfo("SELECT COUNT(id) from [tblShortcut] WHERE software_id = @softwareid", selectedsoftwareid);
            info += "Shortcuts: " + shortcutcount.ToString() + Environment.NewLine;

            lblInstallInfo.Text = "Installation Info:" + Environment.NewLine + info;
        }

        static int GetScalarInfo(string Query, int softwareid)
        {

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(Query, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            int scalarval = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();
            return scalarval;
        }




        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (selectedsoftwareid == -1)
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
                SoftwareClass.AddFile(src, dst, FI.Length, selectedsoftwareid);

            }
            UpdateInfo(); //label info update
            FilesPanel.btnScan.Enabled = true;

        }

        private void txtServerShare_TextChanged(object sender, EventArgs e)
        {
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller", true).SetValue("management_basefolder", FilesPanel.txtServerShare.Text);
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
            if (selectedsoftwareid == -1)
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
            SoftwareClass.AddRegistry(selectedsoftwareid, hkeyval, RegistryPanel.txtKey.Text, RegistryPanel.txtValue.Text, regtype, RegistryPanel.txtData.Text);
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            RegistryPanel.btnAddReg.Enabled = true;
        }

        private void btnAddShortcut_Click(object sender, EventArgs e)
        {

            if (selectedsoftwareid == -1)
            {
                MessageBox.Show("Select Software");
                return;
            }
            ShortcutsPanel.btnAddShortcut.Enabled = false;
            SoftwareClass.AddShortcut(ShortcutsPanel.txtName.Text, ShortcutsPanel.txtLocation.Text, ShortcutsPanel.txtFilepath.Text, ShortcutsPanel.txtWorking.Text, ShortcutsPanel.txtArguments.Text, ShortcutsPanel.txtIcon.Text, selectedsoftwareid);

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
            SoftwareClass.AddFirewallRule(WindowsSettingsPanel.txtFirewallPath.Text, selectedsoftwareid);
            UpdateInfo();

        }

        private void txtFirewallPath_TextChanged(object sender, EventArgs e)
        {
            WindowsSettingsPanel.btnFirewallRuleAdd.Enabled = true;
        }



        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddSerial_Click(object sender, EventArgs e)
        {
            SoftwareClass.AddSerial(SerialPanel.txtSerialName.Text, int.Parse(SerialPanel.txtSerialInstance.Text), selectedsoftwareid, SerialPanel.txtRegKey.Text, SerialPanel.txtRegVal.Text);
        }

        private void txtSerialName_TextChanged(object sender, EventArgs e)
        {
            if (!SerialPanel.txtSerialName.Text.Equals(""))
                SerialPanel.btnAddSerial.Enabled = true;
        }

        private void btnRescanFileSize_Click(object sender, EventArgs e)
        {
            FilesPanel.btnRescanFileSize.Enabled = false;
            LanstallerManagement.RescanFileSize();
            FilesPanel.btnRescanFileSize.Enabled = true;
        }



        private void btnAddPrefFile_Click(object sender, EventArgs e)
        {
            if (selectedsoftwareid == -1)
            {
                MessageBox.Show("Select Software");
                return;
            }
            SoftwareClass.AddPreferenceFile(PreferencesPanel.txtPrefFilePath.Text, PreferencesPanel.txtTarget.Text, PreferencesPanel.txtReplace.Text, selectedsoftwareid);
        }



        private void btnRescanFileHash_Click(object sender, EventArgs e)
        {
            SoftwareClass.RescanFileHashes(true);
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
            if (lbxSoftware.SelectedIndex == -1)
            {
                return;
            }
            //Delete Software.
            SoftwareClass.DeleteSoftware(SoftwareList[lbxSoftware.SelectedIndex].id);

            RefreshSoftware();

        }

        private void btnSecurity_Click(object sender, EventArgs e)
        {
            frmSecurity fS = new frmSecurity();
            fS.Show();
        }
    }
}
