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
            string ServerUri = _Server + "/auth/newtoken";

            string MsgData = string.Empty;
            try
            {
                MsgData = Newtonsoft.Json.JsonConvert.SerializeObject(Rqst);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to parse request into json: " + ex.Message + "\nServer Entered:\"" + _Server + "\"\nRegistration code:\"" + _regcode + "\"");
                return;
            }

            try
            {
                byte[] responsedata = WC.UploadData(ServerUri, "POST", Encoding.UTF8.GetBytes(MsgData));
                _authkey = Encoding.UTF8.GetString(responsedata);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to authenticate: " + ex.Message + "\nServer Entered:\"" + _Server + "\"\nRegistration code:\"" + _regcode + "\"");
                return;
            }


            ServerUri = _Server + "/System/version";

            
            WC.Headers.Add("authorization", _authkey);
            
            try
            {
                string response = WC.DownloadString(ServerUri);
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
                if(ex.Message.Contains("Unauthorized"))
                {
                    MessageBox.Show("Key is not valid.");
                    return;
                }
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

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            //Validate URI.

            //Check if /test returns ok to confirm server online?

        }

        public static bool IsValidUri(string uriString)
        {
            if (string.IsNullOrWhiteSpace(uriString))
            {
                return false;
            }

            Uri uriResult;
            return Uri.TryCreate(uriString, UriKind.Absolute, out uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}
