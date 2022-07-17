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


namespace Lanstaller_Management_Console
{
    public partial class frmLanstallerMmanager : Form
    {
        int selectedsoftwareid = -1;
        List<SoftwareClass.SoftwareInfo> SoftwareList = new List<SoftwareClass.SoftwareInfo>();
        //static string status = "Status: Ready";

        public frmLanstallerMmanager()
        {
            InitializeComponent();
        }

        private void frmLanstallerMmanager_Load(object sender, EventArgs e)
        {
            
            lblVariable.Text = "%INSTALLPATH% = Base Install Directory (Default C:\\Games)" + Environment.NewLine + "%USERPROFILE% = User Profile Path (Default C:\\Users\\<Username>)" + Environment.NewLine + "%WIDTH% = Resolution Preference X" + Environment.NewLine + "%HEIGHT% = Resolution Preference Y" + Environment.NewLine + "%USERNAME% = Username Preference";
            lblCopyActionInfo.Text = "";

            RefreshSoftware();


            foreach (string val in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller", true).GetValueNames())
            {
                if (val == "management_basefolder")
                {
                    txtServerShare.Text = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller").GetValue("management_basefolder", "").ToString();
                }
            }


            cmbxHiveKey.SelectedIndex = 0;
            cmbxType.SelectedIndex = 0;

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
            if (!(txtScanfolder.Text.EndsWith("\\")))
            {
                MessageBox.Show("Scan path must end with backslash.");
                return;
            }

            if (!(txtSubFolder.Text.EndsWith("\\")))
            {
                MessageBox.Show("Sub folder path must end with backslash.");
                return;
            }
            if (txtSubFolder.Text.StartsWith("\\"))
            {
                MessageBox.Show("Sub Folder path cannot start with backslash");
                return;
            }
           
            if (txtServerShare.Text != "" && !(txtServerShare.Text.EndsWith("\\")))
            {
                txtServerShare.Text = txtServerShare.Text + "\\"; //append backslash to server share location..
            }
            //check server share location matches start of scan path.
            if (!(txtScanfolder.Text.ToLower().StartsWith(txtServerShare.Text.ToLower())))
            {
                MessageBox.Show("Scan folder not located within share path");
                return;
            }

            btnScan.Enabled = false;
            string scanfolder = txtScanfolder.Text;
            
            if (Pri.LongPath.Directory.Exists(scanfolder) == false)
            {
                MessageBox.Show("Scan Folder - Invalid");
                btnScan.Enabled = true;
                return;
            }

            lblCopyActionInfo.Text = "Status: Scanning";
            filelist = Pri.LongPath.Directory.GetFiles(scanfolder, "*", System.IO.SearchOption.AllDirectories);
            lblCopyActionInfo.Text = "Status: Scanned Files: " + filelist.Count();
            btnAddFolder.Enabled = true;
        }

        private void lbxSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxSoftware.SelectedIndex == -1)
            {
                return;
            }

            selectedsoftwareid = SoftwareList[lbxSoftware.SelectedIndex].id;

            txtSerialName.Text = SoftwareList[lbxSoftware.SelectedIndex].Name;
            txtSerialInstance.Text = "1";

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


            btnAddFolder.Enabled = false;

            if (txtScanfolder.Text.ToLower().StartsWith(txtServerShare.Text.ToLower() + txtSubFolder.Text.ToLower()) == false)
            {
                MessageBox.Show("Base folder must be part of scan folder.");
                return;
            }

            string destination = txtDestination.Text;
            if (destination == "")
            {
                destination = "%INSTALLPATH%\\";
            }

            if (!(destination.EndsWith("\\")))
            {
                MessageBox.Show("Destination must end with Backslash (\\)");
                return;
            }



            
            string servershare = txtServerShare.Text;
            string subfolder = servershare + txtSubFolder.Text;

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

                MessageBox.Show(src + Environment.NewLine + dst);

                Pri.LongPath.FileInfo FI = new Pri.LongPath.FileInfo(filename);
                SoftwareClass.AddFile(src, dst, FI.Length, selectedsoftwareid);

            }

        }

        private void txtServerShare_TextChanged(object sender, EventArgs e)
        {
            Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Lanstaller", true).SetValue("management_basefolder", txtServerShare.Text);
        }

        private void txtScanfolder_TextChanged(object sender, EventArgs e)
        {
           
            if (txtScanfolder.Text.Contains("\\") && txtServerShare.Text != "")
            {
                //sub-directory path inbetween server-share and scan folder.
                string scanpath = txtScanfolder.Text;
                if (scanpath.EndsWith("\\"))
                {
                    scanpath = scanpath.Substring(0, scanpath.Length - 1);  
                }

                string scanroot = txtScanfolder.Text.Substring(0, scanpath.LastIndexOf("\\") + 1); //offset +1 to include \
                txtSubFolder.Text = (scanroot).Substring(txtServerShare.Text.Length);

            }

            if (txtSubFolder.Text == "")
            {
                btnScan.Enabled = false;
            }
            else
            {
                btnScan.Enabled = true;
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
            if (cmbxHiveKey.Text == "HKEY_LOCAL_MACHINE")
            {
                hkeyval = 1;
            }
            else if (cmbxHiveKey.Text == "HKEY_CURRENT_USER")
            {
                hkeyval = 2;
            }
            else if (cmbxHiveKey.Text == "HKEY_USERS")
            {
                hkeyval = 3;
            }
            else
            {
                MessageBox.Show("Select Reg Hive Key");
                return;
            }

            int regtype;
            if (cmbxType.Text == "STRING")
            {
                regtype = 1;
            }
            else if (cmbxType.Text == "DWORD")
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
            btnAddReg.Enabled = false;
            SoftwareClass.AddRegistry(selectedsoftwareid, hkeyval, txtKey.Text, txtValue.Text, regtype, txtData.Text);
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            btnAddReg.Enabled = true;
        }

        private void btnAddShortcut_Click(object sender, EventArgs e)
        {

            if (selectedsoftwareid == -1)
            {
                MessageBox.Show("Select Software");
                return;
            }
            btnAddShortcut.Enabled = false;
            SoftwareClass.AddShortcut(txtName.Text, txtLocation.Text, txtFilepath.Text, txtWorking.Text, txtArguments.Text, txtIcon.Text, selectedsoftwareid);

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnAddShortcut.Enabled = true;
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            btnAddReg.Enabled = true;
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            btnAddReg.Enabled = true;
        }

        private void txtFilepath_TextChanged(object sender, EventArgs e)
        {
            txtIcon.Text = txtFilepath.Text;

            if (txtFilepath.Text.Contains("\\"))
            {
                txtWorking.Text = txtFilepath.Text.Substring(0, txtFilepath.Text.LastIndexOf("\\"));
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


        
        void UpdateLabels()
        {
           
            string destdir = txtDestination.Text;
            if (txtDestination.Text.Equals(""))
            {
                destdir = "%INSTALLPATH%\\";
            }

            string dstpath = txtScanfolder.Text.Substring(txtServerShare.TextLength + txtSubFolder.TextLength);

            string srcpath = txtScanfolder.Text + "example.exe";
            string dstfile = destdir + dstpath + "example.exe";


            lblCopyActionInfo.Text = "Copy files from: " + srcpath + Environment.NewLine + 
                "to: " + dstfile;

        }

        private void btnFirewallRuleAdd_Click(object sender, EventArgs e)
        {
            SoftwareClass.AddFirewallRule(txtFirewallPath.Text, selectedsoftwareid);

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddSerial_Click(object sender, EventArgs e)
        {
            SoftwareClass.AddSerial(txtSerialName.Text, int.Parse(txtSerialInstance.Text), selectedsoftwareid, txtRegKey.Text, txtRegVal.Text);
        }

        private void txtSerialName_TextChanged(object sender, EventArgs e)
        {
            if (!txtSerialName.Text.Equals(""))
                btnAddSerial.Enabled = true;
        }

        private void btnRescanFileSize_Click(object sender, EventArgs e)
        {
            btnRescanFileSize.Enabled = false;
            SoftwareClass.RescanFileSize();
            btnRescanFileSize.Enabled = true;
        }

        private void btnAddPrefFile_Click(object sender, EventArgs e)
        {
            if (selectedsoftwareid == -1)
            {
                MessageBox.Show("Select Software");
                return;
            }
            SoftwareClass.AddPreferenceFile(txtPrefFilePath.Text, txtTarget.Text, txtReplace.Text, selectedsoftwareid);
        }

      

        private void btnRescanFileHash_Click(object sender, EventArgs e)
        {
            SoftwareClass.RescanFileHashes(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SoftwareClass.RescanFileHashes(false);
        }
    }
}
