using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace Lanstaller
{
    //Lanstaller Project - mrhsystems.com


    public partial class frmLanstaller : Form
    {
        List<SoftwareClass> SList; //List of Software.
        List<int> InstallList = new List<int>();

        List<Tool> ToolList = new List<Tool>();

        Thread MThread; //Status Monitor Thread
        Thread CThread; //Chat Thread
        Thread InsTrd; //installer thread.

        bool shutdown = false;

        public frmLanstaller()
        {
            InitializeComponent();
        }


        private void frmLanstaller_Load(object sender, EventArgs e)
        {
            //Load Config File.
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("Missing config file - no database information");
            }


            foreach (string line in System.IO.File.ReadAllLines("config.ini"))
            {
                if (line.StartsWith("Data Source="))
                {
                    SoftwareClass.ConnectionString = line;
                }
            }

            btnInstall.Text = "Start" + Environment.NewLine + "Install";
            lblSpaceRequired.Text = "";

            //Hide progress bar.
            pbInstall.Visible = false;

            //Get list of installed programs.
            WindowsInstallerClass.CheckProgram();


            //Lanstaller Settings
            LanstallerSettings.GetSettings(); //Refresh settings from Registry.
            txtInstallDirectory.Text = LanstallerSettings.InstallDirectory;
            txtUsername.Text = LanstallerSettings.Username;
            txtWidth.Text = LanstallerSettings.ScreenWidth.ToString();
            txtHeight.Text = LanstallerSettings.ScreenHeight.ToString();


            //Test Web Download with Long File Length?


            //Load software list from Server.
            SList = SoftwareClass.LoadSoftware();
            foreach (SoftwareClass Sw in SList)
            {
                cmbxSoftware.Items.Add(Sw.Name);
            }
            
            //Start installation progress bar thread.
            MThread = new Thread(StatusMonitorThread);
            MThread.Start();

            //Start Chat thread.
            CThread = new Thread(ChatThread);
            CThread.Start();

            //Update list of tools.
            GetTools();


           
            

        }

        class Tool
        {
            public string name;
            public string filepath;
        }

        void GetTools()
        {
            string QueryString = "SELECT [name],[path] FROM tblTools";

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SR = SQLCmd.ExecuteReader();
            while (SR.Read())
            {
                Tool tmpTool = new Tool();
                tmpTool.name = SR[0].ToString();
                tmpTool.filepath = SR[1].ToString();
                ToolList.Add(tmpTool);

                cmbxTool.Items.Add(tmpTool.name);
            }
            SQLConn.Close();
        }



        void StatusMonitorThread()
        {
            while (shutdown == false)
            {
                
                lblStatus.Invoke((MethodInvoker)delegate
                 {
                     lblStatus.Text = SoftwareClass.GetStatus();
                 });

                pbInstall.Invoke((MethodInvoker)delegate
                {
                    pbInstall.Value = SoftwareClass.GetProgressPercentage();
                });

                //Wait 100 ms before next update.
                System.Threading.Thread.Sleep(100);

            }

        }


        void ChatThread()
        {
            string QueryString = "SELECT [id],[timestamp],[message] from tblMessages WHERE [timestamp] > DATEADD(HOUR, -1, GETDATE()) ORDER BY [timestamp] ASC";
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            int lastid = 0;
            int currentid = 0;
            bool newmsg = false;

            while (shutdown == false)
            {
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
                SqlDataReader SR = SQLCmd.ExecuteReader();
                string message = "";
                while (SR.Read())
                {
                    message = message + DateTime.Parse(SR[1].ToString()).ToString("HH:mm:ss") + ": " + SR[2].ToString() + Environment.NewLine;
                    lastid = (int)SR[0];
                }
                SQLConn.Close();

                if (lastid != currentid)
                {
                    currentid = lastid;
                        txtChatMessages.Invoke((MethodInvoker)delegate
                        {
                            txtChatMessages.Text = message;
                            txtChatMessages.SelectionStart = txtChatMessages.Text.Length;
                            txtChatMessages.ScrollToCaret();
                        });
                }
                Thread.Sleep(300);
            }



        }

        void EnableInstallControls(bool state)
        {
            btnInstall.Enabled = state;
            btnInstall.Visible = state;
            
            btnAdd.Enabled = state;
            btnAdd.Visible = state;

            btnClear.Enabled = state;
            btnClear.Visible = state;

            lbxInstallList.Enabled = state;

            /*
            chkFiles.Enabled = state;
            chkRegistry.Enabled = state;
            chkShortcuts.Enabled = state;
            chkPreferences.Enabled = state;
            chkWindowsSettings.Enabled = state;
            chkRedist.Enabled = state;
            */

            if (state == false)
            {
                //Install running.
                lbxInstallList.Location = new Point(lbxInstallList.Location.X, lbxInstallList.Location.Y - 30);
                lbxInstallList.Size = new Size(lbxInstallList.Width, lbxInstallList.Height + 30);
                lblMIQ.Location = new Point(lblMIQ.Location.X,lblMIQ.Location.Y - 30);
            }
            else if (state == true)
            {
                //install normal.
                
                lbxInstallList.Location = new Point(lbxInstallList.Location.X, lbxInstallList.Location.Y + 30);
                lbxInstallList.Size = new Size(lbxInstallList.Width, lbxInstallList.Height - 30);
                lblMIQ.Location = new Point(lblMIQ.Location.X, lblMIQ.Location.Y + 30);
            }

       }


        private void btnInstall_Click(object sender, EventArgs e)
        {


            //Check for single install, add selected item to install list.
            if (lbxInstallList.Items.Count == 0)
            {
                //Single install - Use CMBX Selection.
                if (cmbxSoftware.SelectedIndex == -1)
                {
                    MessageBox.Show("Nothing Selected");
                    return;
                }

                InstallList.Add(cmbxSoftware.SelectedIndex);
            }
            EnableInstallControls(false);

            //Run Installation.
            InsTrd = new Thread(InstallThread);
            InsTrd.Start();


        }



        void InstallThread()
        {

            //Check Install Directory Valid.
            if (LanstallerSettings.CheckInstallDirectoryValid() == false)
            {
                System.IO.Directory.CreateDirectory(LanstallerSettings.InstallDirectory); //Generate
            }

            bool install_files = chkFiles.Checked;
            bool install_reg = chkRegistry.Checked;
            bool install_shortcut = chkShortcuts.Checked;
            bool apply_windowssettings = chkWindowsSettings.Checked;
            bool apply_preferences = chkPreferences.Checked;
            bool install_redist = chkRedist.Checked;


            //Get Serials for All Software
            if (install_reg)
            {
                List<int> IDList = new List<int>();
                foreach (int index in InstallList)
                {
                    IDList.Add(SList[index].id);
                }
                SoftwareClass.GetSerials(IDList);
            }

            //Enable progress bar.
            pbInstall.Invoke((MethodInvoker)delegate
            {
                pbInstall.Visible = true;
            });

            //Run Through install list and install software.
            foreach (int index in InstallList)
            {
                SoftwareClass.Install(SList[index], install_files, install_reg, install_shortcut, apply_windowssettings, apply_preferences, install_redist);
            }

            //Reset install list.
            lbxInstallList.Invoke((MethodInvoker)delegate
            {
                lbxInstallList.Items.Clear();
            });

            InstallList.Clear();


            frmComplete CF = new frmComplete();
            CF.TopMost = true;
            CF.FormBorderStyle = FormBorderStyle.None;
            CF.ShowDialog();
            
            btnInstall.Invoke((MethodInvoker)delegate
            {
                EnableInstallControls(true);
            });

            pbInstall.Invoke((MethodInvoker)delegate
            {
                pbInstall.Visible = false;
            });

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            if (cmbxSoftware.SelectedIndex == -1)
            {
                MessageBox.Show("Nothing Selected");
                return;
            }

            int index = cmbxSoftware.SelectedIndex;
            foreach (int SoftwareID in InstallList)
            {
                if (SList[index].id == SoftwareID)
                {
                    //Duplicate entry.
                    MessageBox.Show("Entry already in install list.");
                    return;
                }

            }

            //add item to installation list.
            lbxInstallList.Items.Add(SList[index].Name);
            InstallList.Add(index);

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Reset install list.
            lbxInstallList.Items.Clear();
            InstallList.Clear();

        }



        private void txtInstallDirectory_TextChanged(object sender, EventArgs e)
        {
            LanstallerSettings.SetInstallDirectory(txtInstallDirectory.Text);
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            LanstallerSettings.SetWidth(int.Parse(txtWidth.Text));
        }

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {
            LanstallerSettings.SetHeight(int.Parse(txtHeight.Text));
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            LanstallerSettings.SetUsername(txtUsername.Text);
        }

        private void frmLanstaller_Closing(object sender, FormClosingEventArgs e)
        {
            shutdown = true;
            //Shutdown Threads.
            MThread.Abort();
            CThread.Abort();

            if (InsTrd != null)
            {
                InsTrd.Abort();
            }
            

            
        }

        private void btnOpenTool_Click(object sender, EventArgs e)
        {
            if (cmbxTool.SelectedIndex == -1)
            {
                return;
            }
            try
            {
                Process.Start(ToolList[cmbxTool.SelectedIndex].filepath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open tool, error:" + ex.ToString());
            }


        }

        private void txtChatSendMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;

            SendMessage(txtChatSendMessage.Text);

        }

        void SendMessage(string Message)
        {
            string QueryString = "INSERT INTO tblMessages ([message],[timestamp]) VALUES (@message,GETDATE())";

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("message", txtUsername.Text + " - " + Message);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();

            txtChatSendMessage.Text = "";
        }

        private void cmbxSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxSoftware.SelectedIndex == -1)
            {
                return;
            }


            long filesize = SoftwareClass.GetInstallSize(SList[cmbxSoftware.SelectedIndex].id);

            double mbfilesize = (double)filesize / 1048576;
            if (mbfilesize < 1000)
            {
                lblSpaceRequired.Text = "Space Required: " + Math.Round(mbfilesize, 2).ToString() + "MB";
            }
            else
            {
                double gbfilesize = filesize / 1073741824;
                lblSpaceRequired.Text = "Space Required: " + Math.Round(gbfilesize, 2).ToString() + "GB";
            }


        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Caption_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((e.Clicks == 1) && (this.WindowState != FormWindowState.Maximized))
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

       

        private void cmbxTool_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                string debuginfo = "Debug Info: ";

                if (InsTrd == null)
                { 
                    debuginfo = "Installer Thread status: not initalised"; //null reference - no install started.
                }
                else if (InsTrd.IsAlive)
                {
                    debuginfo += "Installer Thread status: alive"; //install thread running.
                }
                else
                {
                    debuginfo += "Installer Thread status: not running"; //thread not running / crashed.
                }

                MessageBox.Show(debuginfo);

            }
        }

      
    }
}
