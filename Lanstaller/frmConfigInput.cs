using System;
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
using System.Net;
namespace Lanstaller
{
    
    public partial class frmConfigInput : Form
    {

        public frmConfigInput()
        {
            InitializeComponent();
        }

           

     
        private void btnOK_Click(object sender, EventArgs e)
        {
            //Validate Server & Authorisation.
            string _Server = txtServer.Text;
            string _authkey = txtAuth.Text;

            string URI = _Server + "/System/version";

            WebClient WC = new WebClient();
            WC.Headers.Add("authorization", _authkey);
            try
            {
                string response = WC.DownloadString(URI);
                double server_ver = double.Parse(response);
                //Connection OK, write config file.
                System.IO.StreamWriter SW = new System.IO.StreamWriter("config.ini");
                SW.WriteLine("authkey=" + _authkey);
                SW.WriteLine("apiserver=" + _Server);
                SW.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot connect to server - " + ex.ToString());
            }


        }

        private void frmConfigInput_Load(object sender, EventArgs e)
        {

        }
    }
}
