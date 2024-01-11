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
using System.Collections.Concurrent;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Net.Http;
using static Lanstaller_Shared.SoftwareClass;
using System.Runtime.InteropServices.ComTypes;

namespace Lanstaller
{
    //Lanstaller Project

    public partial class frmLanstaller : Form
    {
        Double Version = 0.22; //Increment Version in tblSystem when changed.

        ConcurrentQueue<ClientSoftwareClass> InstallQueue = new ConcurrentQueue<ClientSoftwareClass>();

        List<int> InstallQueueIDs = new List<int>();
        private static object lock_InstallQueueIDs = new object();


        List<ClientSoftwareClass.SoftwareInfo> SList; //List of Software.

        Thread MThread; //Status Monitor Thread
        Thread CThread; //Chat Thread
        Thread InsTrd; //installer thread.

        bool shutdown = false;
        bool InstallThreadRunning = false;
        private static object lock_InstallThreadRunning = new object();

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
                    ChatClient.SetAuth(auth);

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
            lblSpaceRequired.Text = "";
            //Hide status box.
            gbxStatus.Visible = false;

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
            lvSoftware.Columns.Add("Name", 275);
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

            //Load Software list from web api.
            SList = APIClient.GetSoftwareListFromAPI();

            //Load Icons.
            ImageList SmallImageList = new ImageList();
            lvSoftware.SmallImageList = SmallImageList;
            foreach (SoftwareClass.SoftwareInfo SWI in SList)
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


        private void btnInstall_Click(object sender, EventArgs e)
        {
            InstallSelected();
        }

        void InstallSelected()
        {
            foreach (ListViewItem sItm in lvSoftware.SelectedItems)
            {
                //Prepare install request.
                ClientSoftwareClass InstallSW = new ClientSoftwareClass();
                InstallSW.Identity = SList[sItm.Index];

                //Check if already in queue.
                lock (lock_InstallQueueIDs)
                {
                    foreach (int qid in InstallQueueIDs)
                    {
                        if (InstallSW.Identity.id == qid)
                        {
                            MessageBox.Show("Installation already queued.");
                            return; //skip installation.
                        }

                    }
                    InstallQueueIDs.Add(InstallSW.Identity.id);
                }

                InstallSW.InstallDir = LanstallerSettings.InstallDirectory;
                //Set current selections for installation.
                InstallSW.installfiles = chkFiles.Checked;
                InstallSW.installregistry = chkRegistry.Checked;
                InstallSW.installshortcuts = chkShortcuts.Checked;
                InstallSW.apply_windowssettings = chkWindowsSettings.Checked;
                InstallSW.apply_preferences = chkPreferences.Checked;
                InstallSW.install_redist = chkRedist.Checked;

                //Get serial keys from user - may need to put on another thread to stop gui block.
                if (InstallSW.installregistry) //Only request serials if registry checked.
                {
                    foreach (ClientSoftwareClass.SerialNumber SN in APIClient.GetSerialsListFromAPI(InstallSW.Identity.id))
                    {
                        InstallSW.SerialList.Add(SN);
                    }
                    ClientSoftwareClass.GenerateSerials(InstallSW.SerialList);
                }


                //Queue installation request.
                InstallQueue.Enqueue(InstallSW);

                //Start Installation thread if not running.
                lock (lock_InstallThreadRunning)
                {
                    if (!InstallThreadRunning)
                    {
                        InstallThreadRunning = true;
                        InsTrd = new Thread(InstallThread);
                        InsTrd.Name = "Installer Thread";
                        InsTrd.Start();
                    }
                }
            }

        }

        void InstallThread()
        {
            while (InstallQueue.Count > 0)
            {
                ClientSoftwareClass CSW;
                if (InstallQueue.TryDequeue(out CSW))
                {
                    //Check Install Directory Valid.
                    if (Directory.Exists(CSW.InstallDir) == false) Directory.CreateDirectory(CSW.InstallDir); //Generate installation path.

                    //Enable progress bar.
                    this.BeginInvoke((MethodInvoker)(() => gbxStatus.Visible = true));

                    //Run Installation.
                    CSW.Install();

                    //Disabled complete notification - swapping to queue system.
                    /*
                    frmComplete CF = new frmComplete();
                    CF.TopMost = true;
                    CF.FormBorderStyle = FormBorderStyle.None;
                    CF.ShowDialog();
                    */

                    lock (lock_InstallQueueIDs)
                    {
                        InstallQueueIDs.Remove(CSW.Identity.id);
                    }
                }
            } //End of installer queue.

            //Disable progress bar while no installs running.
            this.BeginInvoke((MethodInvoker)(() => gbxStatus.Visible = false));

            //Mark installer thread as not running.
            lock (lock_InstallThreadRunning)
            {
                InstallThreadRunning = false;
            }

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
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
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

            if (this.Size.Width < StartSize.Width)
            {
                int monwidth = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                if (monwidth > StartSize.Width)
                {
                    this.Width = StartSize.Width;

                }
            }
            if (this.Size.Height < StartSize.Height)
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
            UpdateInstallOptions();
        }

        private void lvSoftware_DoubleClick(object sender, EventArgs e)
        {
            InstallSelected();
        }


        private void lvSoftware_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateSpaceRequired();
        }

        void UpdateSpaceRequired()
        {
            long filesize = 0;
            foreach (ListViewItem lvi in lvSoftware.SelectedItems)
            {
                filesize += SList[lvi.Index].install_size;
            }

            string space_req_str = String.Empty;

            if (filesize > 0)
            {
                double mbfilesize = (double)filesize / (double)1048576;

                if (mbfilesize < 1000)
                {
                    space_req_str = Math.Round(mbfilesize, 2).ToString() + "MB";
                }
                else
                {
                    double gbfilesize = filesize / (double)1073741824;
                    space_req_str = Math.Round(gbfilesize, 2).ToString() + "GB";
                }
            }

            lblSpaceRequired.Text = "Space required: " + space_req_str;
        }

        void UpdateInstallOptions()
        {
            //Only show relevant options for installation.
            int fil_ops = 0;
            int reg_ops = 0;
            int sht_ops = 0;
            int usr_ops = 0;
            int set_ops = 0;
            int dis_ops = 0;

            foreach (ListViewItem lvi in lvSoftware.SelectedItems)
            {
                SoftwareInfo SI = SList[lvi.Index];
                fil_ops += SI.file_count;
                reg_ops += SI.registry_count;
                sht_ops += SI.shortcut_count;
                usr_ops += SI.preference_count;
                set_ops += SI.firewall_count;
                dis_ops += SI.redist_count;
            }

            if (fil_ops == 0)
            {
                CVis(chkFiles, false);
            }
            else
            {
                CVis(chkFiles,true);
            }
            if (reg_ops == 0)
            {
                CVis(chkRegistry,false);
            }
            else
            {
                CVis(chkRegistry,true);
            }
            if (sht_ops == 0)
            {
                CVis(chkShortcuts,false);
            }
            else
            {
                CVis(chkShortcuts,true);
            }
            if (usr_ops == 0)
            {
                CVis(chkPreferences, false);
            }
            else
            {
                CVis(chkPreferences,true);
            }
            if (set_ops == 0)
            {
                CVis(chkWindowsSettings,false);
            }
            else
            {
                CVis(chkWindowsSettings,true);
            }
            if (dis_ops == 0)
            {
                CVis(chkRedist,false);
            }
            else
            {
                CVis(chkRedist,true);
            }
        }

        void CVis(Control _control,bool State)
        {
            _control.Enabled = State;
            _control.Visible = State;
        }




        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private bool isResizing = false;
        private Point resizeStartPoint;


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Handle the WM_NCHITTEST message to allow resizing the form
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)
            {
                m.Result = (IntPtr)HTCAPTION;
            }
        }

        private void pbTitleExpanded_Click(object sender, EventArgs e)
        {

        }

        private void pbBottomright_MouseDown(object sender, MouseEventArgs e)
        {
            isResizing = true;
            resizeStartPoint = Cursor.Position;
        }

        private void pbBottomright_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                // Get the cursor position relative to the screen
                Point cursorPos = Cursor.Position;

                // Convert cursor position to client coordinates
                Point clientCursorPos = PointToClient(cursorPos);

                // Set the new size of the form based on the cursor position
                int newWidth = clientCursorPos.X + pbBottomright.Width;
                int newHeight = clientCursorPos.Y + pbBottomright.Height;

                // Adjust the size to ensure it's not too small
                newWidth = Math.Max(newWidth, MinimumSize.Width);
                newHeight = Math.Max(newHeight, MinimumSize.Height);

                // Set the new size of the form
                Size = new Size(newWidth, newHeight);

                // Update the resize start point for the next iteration
                resizeStartPoint = Cursor.Position;

                //redraw border.
                this.Invalidate();
                this.Update();
                this.Refresh();


            }
        }

        private void pbBottomright_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
