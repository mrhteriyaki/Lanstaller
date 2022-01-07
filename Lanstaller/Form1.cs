using System;
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

namespace Lanstaller
{



    public partial class Form1 : Form
    {
        List<SoftwareClass> SList; //List of Software.
        List<int> InstallList = new List<int>();

        List<Tool> ToolList = new List<Tool>();

        Thread MThread; //Status Monitor Thread
        Thread CThread; //Chat Thread

        bool shutdown = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
            
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

            MThread = new Thread(StatusMonitorThread);
            MThread.Start();


            CThread = new Thread(ChatThread);
            CThread.Start();

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
                //Wait 300 ms before next update.
                System.Threading.Thread.Sleep(100);

                try
                {
                    lblStatus.Invoke((MethodInvoker)delegate
                    {
                        lblStatus.Text = SoftwareClass.GetStatus();
                    });
                }
                catch (Exception ex)
                {
                    return;
                }

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
                    try
                    {
                        txtChatMessages.Invoke((MethodInvoker)delegate
                        {
                            txtChatMessages.Text = message;
                            txtChatMessages.SelectionStart = txtChatMessages.Text.Length;
                            txtChatMessages.ScrollToCaret();
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                Thread.Sleep(1000);
            }



        }

        void EnableInstallControls(bool state)
        {
            btnInstall.Enabled = state;
            btnAdd.Enabled = state;
            btnClear.Enabled = state;
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

            Thread InsTrd = new Thread(InstallThread);
            InsTrd.Start();

            

        }



        void InstallThread()
        {

            //Check Install Directory Valid.
            if (LanstallerSettings.CheckInstallDirectoryValid() == false)
            {
                if (LanstallerSettings.InstallDirectory == LanstallerSettings.defaultinstalldir)
                {
                    System.IO.Directory.CreateDirectory(LanstallerSettings.defaultinstalldir); //Generate Default C:\Install directory.
                }
                else
                {
                    MessageBox.Show("Invalid Install Path."); // Prompt if custom directory does not exist.
                }

                return;
            }
            bool install_files = chkFiles.Checked;
            bool install_reg = chkRegistry.Checked;
            bool install_shortcut = chkShortcuts.Checked;


            //Get Serials for All Software
            List<int> IDList = new List<int>();
            foreach (int index in InstallList)
            {
                IDList.Add(SList[index].id);
            }
            SoftwareClass.GetSerials(IDList);

           
            //Run Through install list and install software.
            foreach (int index in InstallList)
            {
                SoftwareClass.Install(SList[index], install_files, install_reg, install_shortcut);
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

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            shutdown = true;
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
    }
}
