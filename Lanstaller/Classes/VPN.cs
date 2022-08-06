using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lanstaller.Classes
{
    internal class VPN
    {
        //Wireguard VPN Installation
        public static void InstallWireguard()
        {          
            string wiresharkurl = APIClient.GetSystemInfo("wireguard_msi_url");
            string tmpsave = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\wireguard.msi";

            if (System.IO.File.Exists(tmpsave))
            {
                System.IO.File.Delete(tmpsave);
            }

            APIClient.DownloadFile(wiresharkurl, tmpsave);

            Process InstallProc = new Process();
            InstallProc.StartInfo.FileName = "C:\\Windows\\System32\\msiexec.exe";
            InstallProc.StartInfo.Arguments = "/i \"" + tmpsave + "\" DO_NOT_LAUNCH=1 /qn";
            InstallProc.Start();
            InstallProc.WaitForExit();



        }

        //Fetch Configuration from API?


        //VPN Client Configuration


    }
}
