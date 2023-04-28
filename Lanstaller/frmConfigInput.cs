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

           
        class RegistrationRequest
        {
            public string regcode { get; set; }
            public string name { get; set; }
        }
     
        private void btnOK_Click(object sender, EventArgs e)
        {
            //Validate Server & Authorisation.
            string _Server = txtServer.Text;
            string _regcode = txtAuth.Text;

            if (!_Server.StartsWith("https://"))
            {
                _Server = "https://" + _Server;
            }


            WebClient WC = new WebClient();
            WC.Headers.Add("Content-Type", "application/json");

            RegistrationRequest Rqst = new RegistrationRequest();
            Rqst.name = Environment.MachineName + ":" + Environment.UserName;
            Rqst.regcode = _regcode;
            
            string _authkey;
            string URI = _Server + "/auth/newtoken";

            try
            {
                string MsgData = Newtonsoft.Json.JsonConvert.SerializeObject(Rqst);
                byte[] responsedata = WC.UploadData(URI, "POST", Encoding.UTF8.GetBytes(MsgData));
                _authkey = Encoding.UTF8.GetString(responsedata);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.ToString());
                return;
            }


            URI = _Server + "/System/version";

            
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
                MessageBox.Show("Cannot connect to server. Error:" + ex.ToString());
            }


        }

        private void frmConfigInput_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
