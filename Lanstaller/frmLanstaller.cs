using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Lanstaller_Shared;
using Lanstaller.Classes;

namespace Lanstaller
{
    //Lanstaller Project

    public partial class frmLanstaller : Form
    {
        Double Version = 0.1;
        List<ClientSoftwareClass> InstallList = new List<ClientSoftwareClass>();
        List<ClientSoftwareClass.SoftwareInfo> SList; //List of Software.
        List<ClientSoftwareClass.Tool> ToolList = new List<ClientSoftwareClass.Tool>();

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
                //Generate Config File if missing.
                frmConfigInput CI = new frmConfigInput();
                CI.ShowDialog();
            }


            foreach (string line in System.IO.File.ReadAllLines("config.ini"))
            {
                if (line.StartsWith("authkey="))
                {
                    string auth = line.Split('=')[1];
                    APIClient.SetAuthKey(auth);
                    ChatClient.InitChatWC(auth);

                }
                else if (line.StartsWith("apiserver="))
                {
                    APIClient.APIServer = line.Split('=')[1];
                    if (!APIClient.APIServer.EndsWith("/"))
                    {
                        APIClient.APIServer = APIClient.APIServer + "/";
                        
                    }
                    //Set chat server to match api.
                    ChatClient.ChatServer = APIClient.APIServer;
                }
            }

            //Init threads.
            MThread = new Thread(StatusMonitorThread);
            CThread = new Thread(ChatThread);

            //Check Server Version
            try
            {
                double server_version = double.Parse(APIClient.GetSystemInfo("version"));
                if (server_version != Version)
                {
                    MessageBox.Show("Server version does not match client.\nVersion Check Failure\nApplication will now close.");
                    Application.Exit();
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Version Check Failure" + ex.ToString() + "\nApplication will now close.");
                Application.Exit();
                return;
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

            //Direct to database.
            //SList = SoftwareClass.LoadSoftware();


            //Web API
            SList = APIClient.GetSoftwareListFromAPI();
            foreach (ClientSoftwareClass.SoftwareInfo Sw in SList)
            {
                cmbxSoftware.Items.Add(Sw.Name);
            }

            //Start installation progress bar thread.
            MThread.Start();

            //Start Chat thread.
            CThread.Start(); //Disabled until code update.



            //Update list of tools.
            /*
            foreach (SoftwareClass.Tool TL in SoftwareClass.GetTools())
            {
                ToolList.Add(TL);
                cmbxTool.Items.Add(TL.Name);
            }
            */

            foreach (ClientSoftwareClass.Tool TL in APIClient.GetToolsListFromAPI())
            {
                ToolList.Add(TL);
                cmbxTool.Items.Add(TL.Name);
            }
        }



        void StatusMonitorThread()
        {
            while (shutdown == false)
            {

                lblStatus.Invoke((MethodInvoker)delegate
                 {
                     lblStatus.Text = ClientSoftwareClass.GetStatus();
                 });

                pbInstall.Invoke((MethodInvoker)delegate
                {
                    pbInstall.Value = ClientSoftwareClass.GetProgressPercentage();
                });

                //Wait 100 ms before next update.
                System.Threading.Thread.Sleep(100);

            }

        }

        long lastid = 0;
        
        void ChatThread()
        {
            UpdateChatMessages();
            //lastcheck = CM.timestamp;
            while (shutdown == false)
            { 
                if (ChatClient.GetMessageCount(lastid) != 0)
                {
                    UpdateChatMessages();
                }
                Thread.Sleep(300);
            }
            
        }

        void UpdateChatMessages()
        {
            StringBuilder MessageData = new StringBuilder();
            foreach (SharedChat.ChatMessage CM in ChatClient.GetMessagesAPI())
            {
                //Get last hour of chat messages.
                if (CM.timestamp > DateTime.Today)
                {
                    MessageData.AppendLine(CM.timestamp.ToString("HH:mm") + " " + CM.sender + ": " + CM.message);
                }
                else
                {
                    MessageData.AppendLine("(" + CM.timestamp.ToString("m") + " " + CM.timestamp.ToString("yyyy") + ") " + CM.timestamp.ToString("HH:mm") + " " + CM.sender + ": " + CM.message);
                }
                lastid = CM.id;
            }
            
            if (MessageData.Length == 0)
            {
                return;
            }

            //Trim empty lastline.
            string MessageDataStr = MessageData.ToString().Substring(0, (MessageData.Length - 1)); 
            
            //Update GUI
            txtChatMessages.Invoke((MethodInvoker)delegate
            {
                txtChatMessages.Text = MessageDataStr.ToString();
                txtChatMessages.SelectionStart = txtChatMessages.Text.Length;
                txtChatMessages.ScrollToCaret();
            });
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
                lblMIQ.Location = new Point(lblMIQ.Location.X, lblMIQ.Location.Y - 30);
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

                //Nothing queued in gui, add currently selected software to queue for install (single entry).
                ClientSoftwareClass currentsw = new ClientSoftwareClass();
                currentsw.Identity = SList[cmbxSoftware.SelectedIndex];
                InstallList.Add(currentsw);
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
                //Get serial keys for all queued installs.
                foreach (ClientSoftwareClass CSW in InstallList)
                {
                    foreach(ClientSoftwareClass.SerialNumber SN in APIClient.GetSerialsListFromAPI(CSW.Identity.id))
                    {
                        CSW.SerialList.Add(SN);
                    }
                    ClientSoftwareClass.GenerateSerials(CSW.SerialList);
                }
               
            }

            //Enable progress bar.
            pbInstall.Invoke((MethodInvoker)delegate
            {
                pbInstall.Visible = true;
            });

            //Run Through install list and install software.
            foreach (ClientSoftwareClass CSW in InstallList)
            {
                CSW.Install(install_files, install_reg, install_shortcut, apply_windowssettings, apply_preferences, install_redist);
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
            foreach (ClientSoftwareClass CSW in InstallList)
            {
                if (SList[index].id == CSW.Identity.id)
                {
                    //Duplicate entry.
                    MessageBox.Show("Entry already in install list.");
                    return;
                }

            }

            //add item to installation list.
            lbxInstallList.Items.Add(SList[index].Name);

            ClientSoftwareClass currentsw = new ClientSoftwareClass();
            currentsw.Identity = SList[index];
            InstallList.Add(currentsw);

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Reset install list.
            lbxInstallList.Items.Clear();
            InstallList.Clear();



            //returns public client cert as string.
            //Console.WriteLine(VPN.InstallConfig("192.168.90.2/32","192.168.90.0/24", "pADjnbdB1mTsopMmmvOdkzKo56voLeWYBHEz6tyoHgQ=", "192.168.88.1:13231"));
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
                //SMB
                //Process.Start(ToolList[cmbxTool.SelectedIndex].path);

                //WEB
                string toolpath = ToolList[cmbxTool.SelectedIndex].path;
                int pathfinishpoint = toolpath.LastIndexOf("\\");
                string destpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\" + toolpath.Substring(pathfinishpoint);

                APIClient.DownloadFile(ToolList[cmbxTool.SelectedIndex].path, destpath);
                Process.Start(destpath);
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

            try
            {
                SharedChat.ChatMessage tMsg = new SharedChat.ChatMessage();
                tMsg.message = txtChatSendMessage.Text;
                tMsg.sender = txtUsername.Text;
                tMsg.timestamp = DateTime.Now;

                ChatClient.SendMessageAPI(tMsg);

                string MessageData = txtChatMessages.Text.ToString() + Environment.NewLine + tMsg.timestamp.ToString("HH:mm") + " " + tMsg.sender + ": " + tMsg.message;
                txtChatMessages.Text = MessageData;
                txtChatMessages.SelectionStart = MessageData.Length;
                txtChatMessages.ScrollToCaret();

            }
            catch(Exception ex)
            {
                //Failed to send.
                MessageBox.Show(ex.ToString());
            }
            
            
            txtChatSendMessage.Text = "";
        }


        private void cmbxSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxSoftware.SelectedIndex == -1)
            {
                return;
            }

            
            long filesize = APIClient.GetInstallSizeFromAPI(SList[cmbxSoftware.SelectedIndex].id);

            double mbfilesize = (double)filesize / (double)1048576;
            if (mbfilesize < 1000)
            {
                lblSpaceRequired.Text = "Space Required: " + Math.Round(mbfilesize, 2).ToString() + "MB";
            }
            else
            {
                double gbfilesize = filesize / (double)1073741824;
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
