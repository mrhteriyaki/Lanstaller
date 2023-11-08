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
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Web;

namespace Lanstaller
{
    //Lanstaller Project
    


    public partial class frmLanstaller : Form
    {
        Double Version = 0.2;
        List<ClientSoftwareClass> InstallList = new List<ClientSoftwareClass>();
        List<ClientSoftwareClass.SoftwareInfo> SList; //List of Software.

        Thread MThread; //Status Monitor Thread
        Thread CThread; //Chat Thread
        Thread InsTrd; //installer thread.

        bool shutdown = false;

        Size StartSize;

        public frmLanstaller()
        {
            InitializeComponent();
        }



        bool CheckResources()
        {
            if (!CheckFile("7z.exe")) return false;
            if (!CheckFile("7z.dll")) return false;
            if (!CheckFile("Lanstaller Shared.dll")) return false;
            if (!CheckFile("Newtonsoft.Json.dll")) return false;
            if (!CheckFile("Pri.LongPath.dll")) return false;
            return true;
        }

        bool CheckFile(string cfpath)
        {
            //Check if file exists, return false if not.
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + cfpath))
            {
                MessageBox.Show("Missing resource file: " + cfpath);
                return false;
            }
            return true;
        }

        private void frmLanstaller_Load(object sender, EventArgs e)
        {
            if (!CheckResources())
            {
                //Missing files, exit.
                Application.Exit();
                return;
            }

            //Load Config File.
            if (!File.Exists("config.ini"))
            {
                //Generate Config File if missing.
                frmConfigInput CI = new frmConfigInput();
                CI.ShowDialog();
            }
            
            //Check if config file exists.
            if (!File.Exists("config.ini")) Application.Exit();
                        

            //Load Config.
            foreach (string line in System.IO.File.ReadAllLines("config.ini"))
            {
                if (line.StartsWith("authkey="))
                {
                    string auth = line.Split('=')[1];
                    APIClient.Setup(auth);
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
            
            //Record original window size.
            StartSize = this.Size;

            //Init threads.
            MThread = new Thread(StatusMonitorThread);
            MThread.Name = "Status Monitor";
            CThread = new Thread(ChatThread);
            CThread.Name = "Chat Thread";

            //Check Server Version against local client.
            VersionCheck();

            //Set initial status.
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

            //Load software list from Server.
            lvSoftware.View = View.Details;
            lvSoftware.HeaderStyle = ColumnHeaderStyle.None;
            lvSoftware.FullRowSelect = true;
            lvSoftware.Columns.Add("Name",190);
            LoadSoftwareList();

            //Start installation progress bar thread.
            MThread.Start();

            //Start Chat thread.
            CThread.Start(); //Disabled until code update.

        }

        void LoadSoftwareList()
        {
            //Direct to database.
            //SList = SoftwareClass.LoadSoftware();


            lvSoftware.Items.Clear();


            string tmpImages = Path.GetTempPath() + "Lanstaller\\Images";
            if (!Directory.Exists(tmpImages))
            {
                Directory.CreateDirectory(tmpImages);
            }

            
            //Get a file server path.
            SoftwareClass.Server FileServer = APIClient.GetFileServerFromAPI();
            
            //Load from web api.
            SList = APIClient.GetSoftwareListFromAPI();
            ImageList SmallImageList = new ImageList();
            lvSoftware.SmallImageList = SmallImageList;
            foreach(SoftwareClass.SoftwareInfo SWI in SList)
            {
                ListViewItem LVI;
                if (!String.IsNullOrEmpty(SWI.image_small))
                {
                    LVI = new ListViewItem(SWI.Name, SmallImageList.Images.Count);
                    string imgFilename = Path.GetFileName(SWI.image_small);
                    string imgDst = tmpImages + "\\" + imgFilename;                  
                    string imgSrc = FileServer.path + SWI.image_small;
                    APIClient.DownloadFile(imgSrc, imgDst);
                    SmallImageList.Images.Add(Image.FromFile(imgDst));

                    //add caching here later if load speed is slow from images.
                }
                else
                {
                    LVI = new ListViewItem(SWI.Name);
                }

                lvSoftware.Items.Add(LVI);
            }

           
        }

        void VersionCheck()
        {
            try
            {
                double server_version = double.Parse(APIClient.GetSystemInfo("version"));
                if (server_version != Version)
                {
                    if (MessageBox.Show("Lanstaller Update Required.", "Update Required", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        string upath = AppDomain.CurrentDomain.BaseDirectory;
                        string updaterpath = upath + "\\Lanstaller.Updater.exe";

                        //Update.
                        if (File.Exists(updaterpath))
                        {
                            File.Delete(updaterpath);
                        }
                        //Download file.
                        APIClient.DownloadFile(APIClient.APIServer + "StaticFiles/Lanstaller.Updater.exe", updaterpath);

                        //Run with wscript.
                        Process UProc = new Process();
                        UProc.StartInfo.FileName = updaterpath;
                        UProc.StartInfo.WorkingDirectory = upath;
                        UProc.StartInfo.Arguments = APIClient.APIServer;
                        UProc.Start();
                        UProc.WaitForExit();

                        Thread.Sleep(10000);
                        MessageBox.Show("Update timeout");
                        this.BeginInvoke(new MethodInvoker(this.Close));
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(this.Close));
                        //return;
                    }
                }
            }
            catch (Exception ex)
            {
                //Exit on failure.
                MessageBox.Show("Version Check / Update Failure" + ex.ToString() + "\nApplication will now close.", "Update Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.BeginInvoke(new MethodInvoker(this.Close));
                return;
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

        long lastid = 0; //last message ID number.
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

            lvSoftware.Enabled = state;

            txtInstallDirectory.Enabled = state;

            gbxActions.Visible = state;
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
                if (lvSoftware.SelectedItems.Count != 1)
                {
                    MessageBox.Show("Nothing Selected");
                    return;
                }

                //Nothing queued in gui, add currently selected software to queue for install (single entry).
                ClientSoftwareClass currentsw = new ClientSoftwareClass();
                currentsw.Identity = SList[lvSoftware.SelectedItems[0].Index];
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
                    foreach (ClientSoftwareClass.SerialNumber SN in APIClient.GetSerialsListFromAPI(CSW.Identity.id))
                    {
                        CSW.SerialList.Add(SN);
                    }
                    ClientSoftwareClass.GenerateSerials(CSW.SerialList);
                }

            }

            //Enable progress bar.
            this.BeginInvoke((MethodInvoker)(() => pbInstall.Visible = true));


            //Run Through install list and install software.
            foreach (ClientSoftwareClass CSW in InstallList)
            {
                CSW.Install(install_files, install_reg, install_shortcut, apply_windowssettings, apply_preferences, install_redist);
            }

            //Reset install list. (begin invoke is async).
            this.BeginInvoke((MethodInvoker)(() => lbxInstallList.Items.Clear()));

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

            if (lvSoftware.SelectedItems.Count != 1)
            {
                MessageBox.Show("Nothing Selected");
                return;
            }

            int index = lvSoftware.SelectedItems[0].Index;
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
            if (MThread != null)
            {
                MThread.Abort();
            }
            if (CThread != null)
            {
                CThread.Abort();
            }
            
            if (InsTrd != null)
            {
                InsTrd.Abort();
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
            catch (Exception ex)
            {
                //Failed to send.
                MessageBox.Show(ex.ToString());
            }


            txtChatSendMessage.Text = "";
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

        
        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            //If game force shrinks window, reset to normal size.
            
            if(this.Size.Width < StartSize.Width)
            {
                int monwidth = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                if (monwidth > StartSize.Width)
                {
                    this.Width = StartSize.Width;
                    
                }
            }
            if(this.Size.Height < StartSize.Height)
            {
                int monheight = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
                if (monheight > StartSize.Height)
                {
                    this.Height = StartSize.Height;
                }
            }
            this.Invalidate();
        }

        private void lvSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSoftware.SelectedItems.Count != 1) return;
            

            long filesize = APIClient.GetInstallSizeFromAPI(SList[lvSoftware.SelectedItems[0].Index].id);

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
    }
}
