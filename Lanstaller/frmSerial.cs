﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Threading;
using Lanstaller.Classes;
using LanstallerShared;
using static LanstallerShared.LanstallerServer;

namespace Lanstaller
{

    public partial class frmSerial : Form
    {
        public int serialid;
        List<UserSerial> serial_pool = new List<UserSerial>();
        public bool aborted = true;

        public frmSerial()
        {
            InitializeComponent();
        }

       
        private void txtSerial_TextChanged(object sender, EventArgs e)
        {
            txtSerial.Text = SerialNumber.FilterSerial(txtSerial.Text);
        }

     
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbxServerSerials.SelectedIndex != -1)
            {
                //Send back Server Serial of confirmation for use.
                APIClient.SetAvailableSerials(serial_pool[cmbxServerSerials.SelectedIndex].id);
            }

            if (!String.IsNullOrWhiteSpace(txtSerial.Text))
            {
                aborted = false;
                this.Close();
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            txtSerial.Text = GetClipboard();
        }


        private string GetClipboard()
        {
            try
            {
                string clipboardData = null;
                Exception threadEx = null;
                Thread staThread = new Thread(
                    delegate ()
                    {
                        try
                        {
                            clipboardData = Clipboard.GetText(TextDataFormat.Text);
                        }

                        catch (Exception ex)
                        {
                            threadEx = ex;
                        }
                    });
                staThread.Name = "Clipboard Thread";
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
                return clipboardData;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return string.Empty;
            }
        }

        private void frmSerial_Load(object sender, EventArgs e)
        {
            serial_pool = APIClient.GetAvailableSerials(serialid);

            foreach (UserSerial serial in serial_pool)
            {
                cmbxServerSerials.Items.Add(serial.serial);

            }
            if (serial_pool.Count == 0)
            {
                cmbxServerSerials.Visible = false;
                lblServerSerials.Visible = false;
            }

        }

        private void cmbxServerSerials_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxServerSerials.SelectedIndex != -1)
            {
                txtSerial.Text = cmbxServerSerials.Text;
                btnPaste.Enabled = false;
                btnPaste.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
