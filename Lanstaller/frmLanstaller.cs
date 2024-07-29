using System;
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
using static Lanstaller_Shared.LanstallerShared;
using System.Runtime.InteropServices.ComTypes;
using Pri.LongPath;
using System.Reflection;
using static Lanstaller.Classes.LocalDatabase;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Lanstaller
{
    //Lanstaller Project

    public partial class frmLanstaller : Form
    {
        static readonly Double Version = 0.22; //Increment Version in tblSystem when changed.
        readonly static string LanstallerDataDir = "C:\\ProgramData\\Lanstaller\\";
        LocalDatabase LocalDB;

        private static object lock_InstallQueue = new object();
        Queue<ClientSoftwareClass> InstallQueue = new Queue<ClientSoftwareClass>();
        ClientSoftwareClass CurrentCSW = new ClientSoftwareClass();

        List<SoftwareInfo> SList; //List of Software.

        Thread MThread; //Status Monitor Thread
        Thread CThread; //Chat Thread
        Thread InsTrd; //installer thread.
        Thread sCheck; //support checks.

        static bool install_option = true;
        public static bool shutdown = false;
        static bool InstallThreadRunning = false;
        private static object lock_InstallThreadRunning = new object();

        Size WindowStartSize;

        List<ShortcutOperation> currentSoftwareShortcuts;

        public frmLanstaller()
        {
            InitializeComponent();
        }

        void CheckCoreFilesExist()
        {
            string[] core_files = {
                "7z.exe",
                "7z.dll",
                "Lanstaller Shared.dll",
                "Newtonsoft.Json.dll",
                "Pri.LongPath.dll"
            };

            foreach (string cfile in core_files)
            {
                if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + cfile))
                {
                    MessageBox.Show("Missing resource file: " + cfile);
                    Application.Exit();
                }
            }
        }

        private void frmLanstaller_Load(object sender, EventArgs e)
        {
            CheckCoreFilesExist();
            WindowStartSize = this.Size;
            if (!LoadConfigFile())
            {
                Application.Exit();
                return;
            }
            
            if (!ClientUpdateCheck())
            {
                Application.Exit();
                return;
            }

            SetupThreads(); //Put other supporting threads her

            //Get list of installed programs - future use to skip redist.
            //WindowsInstallerClass.CheckProgram();

            //Need to put auth/connection check - invalid auth response should re-prompt with ConfigInput.

            LoadClientSettings();
           
            InitialFormSetup();
            
            try
            {
                LoadSoftwareList(); //causing 2 second load delay, add load spash in future.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to server. Error: " + ex.Message);
                Application.Exit();
            }
            Logging.ClearLog();

        }

        void InitialFormSetup()
        {
            lblSpaceRequired.Text = "";
            gbxStatus.Visible = false;
            lvSoftware.View = View.Details;
            lvSoftware.HeaderStyle = ColumnHeaderStyle.None;
            lvSoftware.FullRowSelect = true;
            lvSoftware.Columns.Add("Name", 275);
        }


        void SetupThreads()
        {
            MThread = new Thread(StatusMonitorThread);
            MThread.Name = "Status Monitor";
            MThread.Start();

            CThread = new Thread(ChatThread);
            CThread.Name = "Chat Thread";
            CThread.Start();

            sCheck = new Thread(Support.Check);
            sCheck.Name = "Support Checks Thread";
            sCheck.Start();

        }

        bool LoadConfigFile()
        {
            if (!File.Exists("config.ini"))
            {
                //Generate Config File if missing.
                frmConfigInput CI = new frmConfigInput();
                CI.ShowDialog();
            }
            if (!File.Exists("config.ini"))
            {
                MessageBox.Show("Config File Missing.");
                return false;
            }
            foreach (string line in System.IO.File.ReadAllLines("config.ini"))
            {
                if (line.StartsWith("authkey="))
                {
                    string auth = line.Split('=')[1];
                    APIClient.SetAuth(auth);
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
            return true;
        }

        void LoadClientSettings()
        {
            UserSettings.GetSettings(); //Refresh settings from Registry.
            txtInstallDirectory.Text = UserSettings.InstallDirectory;
            txtUsername.Text = UserSettings.Username;
            txtWidth.Text = UserSettings.ScreenWidth.ToString();
            txtHeight.Text = UserSettings.ScreenHeight.ToString();
        }

        async void LoadSoftwareList()
        {
            //For direct link to database.
            //SList = SoftwareClass.LoadSoftware();

            SList = APIClient.GetSoftwareListFromAPI();
            FileServer FileServer;
            try
            {
                FileServer = APIClient.GetFileServerFromAPI()[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to get server information, Error: " + ex.Message);
                return;
            }

            ClientSoftwareClass.WANMode = FileServer.IsWAN();
            if(ClientSoftwareClass.WANMode)
            {
                lblWANMode.Visible = true;
            }
            

            string ImageDir = LanstallerDataDir + "Images";
            if (!Directory.Exists(ImageDir))
            {
                Directory.CreateDirectory(ImageDir);
            }

            //Load software already installed.
            LocalDB = new LocalDatabase(LanstallerDataDir + "installed.json");
            List<int> InstalledIDs = LocalDB.GetSoftwareIDs();
            lvSoftware.SmallImageList = new ImageList();
            FileServer FS = APIClient.GetFileServerFromAPI()[0];
            foreach (SoftwareInfo SWI in SList)
            {
                ListViewItem LVI = new ListViewItem(SWI.Name);
                lvSoftware.Items.Add(LVI);

                //Add icon image to listview
                if (!String.IsNullOrEmpty(SWI.image_small))
                {
                    LVI.ImageIndex = lvSoftware.SmallImageList.Images.Count;
                    string imgFilename = Path.GetFileName(SWI.image_small);
                    string imgDst = ImageDir + "\\" + imgFilename;
                    if (!File.Exists(imgDst))
                    {
                        Task task = Task.Run(() => ClientSoftwareClass.TransferFile(FS, Uri.EscapeUriString(SWI.image_small), imgDst));
                        await task;
                        task.Wait();
                        //Will need cache invalidation in future for refresh / updates.
                    }
                    lvSoftware.SmallImageList.Images.Add(Image.FromFile(imgDst));
                }

                //Highlight installed items with White text.
                LVI.ForeColor = Color.Gray;
                foreach (int LSWID in InstalledIDs)
                {
                    if (LSWID == SWI.id)
                    {
                        LVI.ForeColor = Color.White;
                        break;
                    }
                }
            }
        }

        bool ClientUpdateCheck()
        {
            double server_version = 0;
            try
            {
                 server_version = double.Parse(APIClient.GetSystemInfo("version"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to check version Error:" +  ex.Message);
                return false;
            }

            try
            {
                
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
                        FileServer FS = APIClient.GetFileServerFromAPI()[0];
                        try
                        {
                            WebClient UWC = new WebClient();
                            UWC.DownloadFile(APIClient.APIServer + "StaticFiles/Lanstaller.Updater.exe", updaterpath);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Failed to get updater " + ex.Message);
                        }
                        

                        //Run with wscript.
                        Process UProc = new Process();
                        UProc.StartInfo.FileName = updaterpath;
                        UProc.StartInfo.WorkingDirectory = upath;
                        UProc.StartInfo.Arguments = APIClient.APIServer;
                        UProc.Start();
                        UProc.WaitForExit();

                        //Wait for updater to kill process.
                        Thread.Sleep(10000);
                        MessageBox.Show("Update timeout");
                        this.BeginInvoke(new MethodInvoker(this.Close));
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(this.Close));
                        
                    }
                    return false;
                }
                else
                {
                    return true; //Version OK.
                }

            }
            catch (Exception ex)
            {
                //Exit on failure.
                MessageBox.Show("Version Check / Update Failure" + ex.ToString() + "\nApplication will now close.", "Update Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.BeginInvoke(new MethodInvoker(this.Close));
                return false;
            }
        }

        void StatusMonitorThread()
        {
            while (shutdown == false)
            {

                lblStatus.Invoke((MethodInvoker)delegate
                 {
                     lblStatus.Text = CurrentCSW.GetStatus();
                 });

                pbInstall.Invoke((MethodInvoker)delegate
                {
                    pbInstall.Value = CurrentCSW.GetProgressPercentage();
                });

                //Wait 100 ms before next update.
                Thread.Sleep(100);
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

        private void lvSoftware_DoubleClick(object sender, EventArgs e)
        {
            SoftwareAction();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            SoftwareAction();
        }

        void SoftwareAction()
        {
            if (install_option)
            {
                //Install Software.
                foreach (ListViewItem sItm in lvSoftware.SelectedItems)
                {
                    QueueInstall(sItm.Index);
                }
            }
            else
            {
                //Run Software.
                if (currentSoftwareShortcuts.Count == 1)
                {
                    runShortcut(currentSoftwareShortcuts[0]);
                }
                else
                {
                    frmRunSelection runOptionsWindow = new frmRunSelection();
                    runOptionsWindow.SetOptions(currentSoftwareShortcuts);
                    runOptionsWindow.Show();
                }
            }
        }

        public static void runShortcut(ShortcutOperation Shortcut)
        {
            if (!File.Exists(Shortcut.filepath))
            {
                MessageBox.Show("Missing EXE to launch");
                return;
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Shortcut.filepath;
            startInfo.WorkingDirectory = Shortcut.runpath;
            startInfo.Arguments = Shortcut.arguments;
            startInfo.UseShellExecute = true;
            try
            {
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to launch: " + ex.ToString());
            }
        }

        void QueueInstall(int SoftwareListIndex)
        {
            ClientSoftwareClass InstallSW = new ClientSoftwareClass();
            InstallSW.Identity = SList[SoftwareListIndex];

            //Check if already in queue.
            if (CurrentCSW.Identity.id == InstallSW.Identity.id)
            {
                MessageBox.Show("Installation already running.");
                return;
            }

            lock (lock_InstallQueue)
            {
                if (InstallQueue.Any(inst => inst.Identity.id == InstallSW.Identity.id))
                {
                    MessageBox.Show("Installation already queued.");
                    return;
                }
            }




            InstallSW.InstallDir = UserSettings.InstallDirectory;
            InstallSW.installfiles = chkFiles.Checked;
            InstallSW.installregistry = chkRegistry.Checked;
            InstallSW.installshortcuts = chkShortcuts.Checked;
            InstallSW.apply_windowssettings = chkWindowsSettings.Checked;
            InstallSW.apply_preferences = chkPreferences.Checked;
            InstallSW.install_redist = chkRedist.Checked;

            //Get serial keys from user - may need to put on another thread to stop gui block.
            if (InstallSW.installregistry) //Only request serials if registry checked.
            {
                foreach (SerialNumber SN in APIClient.GetSerialsListFromAPI(InstallSW.Identity.id))
                {
                    InstallSW.AddSerial(SN);
                }
                if (!InstallSW.GenerateSerials()) //Prompt for serials, if cancelled, abort install.
                {
                    return;
                }
            }

            lvSoftware.Items[SoftwareListIndex].Text = lvSoftware.Items[SoftwareListIndex].Text + " (Install Queued)";
            lvSoftware.Items[SoftwareListIndex].ForeColor = Color.DarkGray;

            //Queue installation request.
            lock (lock_InstallQueue)
            {
                InstallQueue.Enqueue(InstallSW);
            }

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

        void InstallThread()
        {
            while (InstallQueue.Count > 0)
            {
                lock (lock_InstallQueue)
                {
                    CurrentCSW = InstallQueue.Dequeue();
                }

                //Check Install Directory Valid.
                if (Directory.Exists(CurrentCSW.InstallDir) == false)
                {
                    Directory.CreateDirectory(CurrentCSW.InstallDir); //Generate installation path.
                }

                this.BeginInvoke((MethodInvoker)(() => gbxStatus.Visible = true));

                int SListIndex = SList.FindIndex(swi => swi.id == CurrentCSW.Identity.id);

                this.BeginInvoke((MethodInvoker)(() => lvSoftware.Items[SListIndex].Text = CurrentCSW.Identity.Name + " (Installing)"));

                CurrentCSW.Install();

                this.BeginInvoke((MethodInvoker)(() => lvSoftware.Items[SListIndex].Text = CurrentCSW.Identity.Name));
                if (CurrentCSW.GetErrored())
                {
                    MessageBox.Show("Some or all files failed to download and pass verification.");
                }
                else
                {
                    //if (ShortcutAndFileExist(CurrentCSW.Identity.id))
                    LocalDB.AddLocalInstall(CurrentCSW.Identity.id, CurrentCSW.GetShortcutOperations());
                    this.BeginInvoke((MethodInvoker)(() => lvSoftware.Items[SListIndex].ForeColor = Color.White));
                    this.BeginInvoke((MethodInvoker)(() => CheckInstalled()));
                }


            } //End of installer queue.

            CurrentCSW = new ClientSoftwareClass(); //Clear current CSW object.

            //Disable progress bar while no installs running.
            this.BeginInvoke((MethodInvoker)(() => gbxStatus.Visible = false));

            //Mark installer thread as not running.
            lock (lock_InstallThreadRunning)
            {
                InstallThreadRunning = false;
            }

        }

        bool warningShown = false;
        private void txtInstallDirectory_TextChanged(object sender, EventArgs e)
        {
            UserSettings.SetInstallDirectory(txtInstallDirectory.Text);

            if (InstallThreadRunning && !warningShown)
            {
                warningShown = true;
                MessageBox.Show("Changes to installation directory while running will not apply until the next installation starts.");
            }
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            UserSettings.SetWidth(int.Parse(txtWidth.Text));
        }

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {
            UserSettings.SetHeight(int.Parse(txtHeight.Text));
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            UserSettings.SetUsername(txtUsername.Text);
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

        private void Caption_MouseDown(object sender, MouseEventArgs e) //Used by MouseDown event on pictureboxes to move program window.
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

            if (this.Size.Width < WindowStartSize.Width)
            {
                int monwidth = SystemInformation.PrimaryMonitorSize.Width;
                if (monwidth > WindowStartSize.Width)
                {
                    this.Width = WindowStartSize.Width;

                }
            }
            if (this.Size.Height < WindowStartSize.Height)
            {
                int monheight = SystemInformation.PrimaryMonitorSize.Height;
                if (monheight > WindowStartSize.Height)
                {
                    this.Height = WindowStartSize.Height;
                }
            }
            this.Invalidate();
        }

        private void lvSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInstallOptions();
            CheckInstalled();
        }




        private void lvSoftware_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateSpaceRequired();

            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = lvSoftware.HitTest(e.Location);
                if (hitTestInfo.Item != null)
                {
                    csmSoftware.Show(lvSoftware, e.Location);
                }
            }
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


        void CheckInstalled()
        {
            install_option = true;
            if (lvSoftware.SelectedItems.Count == 1)
            {
                currentSoftwareShortcuts = LocalDB.GetShortcuts(SList[lvSoftware.SelectedItems[0].Index].id);
                if (currentSoftwareShortcuts.Count > 0)
                {
                    foreach (ShortcutOperation SC in currentSoftwareShortcuts)
                    {
                        if (File.Exists(SC.filepath))
                        {
                            install_option = false;
                            btnInstall.Text = "Start";
                            return;
                        }
                    }
                }
            }
            btnInstall.Text = "Install";
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
            active_chk = 0;
            if (fil_ops == 0)
            {
                CheckboxSet(chkFiles, false);
            }
            else
            {
                CheckboxSet(chkFiles, true);
            }
            if (reg_ops == 0)
            {
                CheckboxSet(chkRegistry, false);
            }
            else
            {
                CheckboxSet(chkRegistry, true);
            }
            if (sht_ops == 0)
            {
                CheckboxSet(chkShortcuts, false);
            }
            else
            {
                CheckboxSet(chkShortcuts, true);
            }
            if (usr_ops == 0)
            {
                CheckboxSet(chkPreferences, false);
            }
            else
            {
                CheckboxSet(chkPreferences, true);
            }
            if (set_ops == 0)
            {
                CheckboxSet(chkWindowsSettings, false);
            }
            else
            {
                CheckboxSet(chkWindowsSettings, true);
            }
            if (dis_ops == 0)
            {
                CheckboxSet(chkRedist, false);
            }
            else
            {
                CheckboxSet(chkRedist, true);
            }





        }

        int active_chk = 0;
        void CheckboxSet(Control _control, bool State)
        {
            _control.Enabled = State;
            _control.Visible = State;
            if (State)
            {
                active_chk++;
                _control.Location = new Point(_control.Location.X, (active_chk * 20));
            }
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

        private void reinstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Re-Install selected software.
            foreach (ListViewItem sItm in lvSoftware.SelectedItems)
            {
                QueueInstall(sItm.Index);
            }
        }
    }
}
