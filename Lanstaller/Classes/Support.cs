using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lanstaller.Classes
{
    public class Support
    {

        public static void Check()
        {
            //OpenVPNCheck();

            //Check for Hyper-V.

            //Check for audio input device.

            //add discord detection (interference with some games)


        }


        public static void OpenVPNCheck()
        {
            // Get all network interfaces
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Loop through and display information about each network interface
            foreach (NetworkInterface ni in networkInterfaces)
            {
                if(ni.Name.ToLower().Contains("hyper-v")  || ni.Description.ToLower().Contains("hyper-v") )
                {
                    MessageBox.Show("Hyper-V Network Detected - Recommend Removal");  
                }

                if ( ni.Name.ToLower().Contains("openvpn")  || ni.Description.ToLower().Contains("openvpn"))
                {
                    MessageBox.Show("OpenVPN Detected - Recommend Removal");
                }

            }
        }

    }
}
