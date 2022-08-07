using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;

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
        public static string InstallConfig(string IPAddr, string NetworkAddr, string ServerKey, string VPNServer)
        {
            //Generate private key.
            Process WGProc = new Process();
            WGProc.StartInfo.FileName = "C:\\Program Files\\WireGuard\\wg.exe";
            WGProc.StartInfo.WorkingDirectory = "C:\\Program Files\\WireGuard";
            WGProc.StartInfo.Arguments = "genkey";
            WGProc.StartInfo.RedirectStandardOutput = true;
            WGProc.StartInfo.UseShellExecute = false;

            WGProc.Start();
            string privkey = WGProc.StandardOutput.ReadToEnd();


            //Get Public key.
            WGProc.StartInfo.Arguments = "pubkey";
            WGProc.StartInfo.RedirectStandardInput = true;
            WGProc.Start();

            StreamWriter myStreamWriter = WGProc.StandardInput;
            myStreamWriter.Write(privkey);
            myStreamWriter.Close();

            string pubkey = WGProc.StandardOutput.ReadToEnd();


            string wgdir = "C:\\Program Files\\WireGuard\\";
            string cfdir = wgdir + "Lanstaller.conf";

            if (File.Exists(cfdir))
            {
                File.Delete(cfdir);
            }

            StreamWriter SW = new System.IO.StreamWriter(cfdir);
            SW.WriteLine("[Interface]");
            SW.WriteLine("Address = " + IPAddr);
            SW.WriteLine("PrivateKey = " + privkey);
            SW.WriteLine("\n[Peer]");
            SW.WriteLine("PublicKey = " + ServerKey);
            SW.WriteLine("Endpoint = " + VPNServer);
            SW.WriteLine("AllowedIPs = " + NetworkAddr);
            SW.WriteLine("PersistentKeepalive = 25");
            SW.Close();

            //Activate Configuration.
            Process WrGrdProc = new Process();
            WrGrdProc.StartInfo.FileName = "C:\\Program Files\\WireGuard\\wireguard.exe";
            WrGrdProc.StartInfo.WorkingDirectory = "C:\\Program Files\\WireGuard";
            WrGrdProc.StartInfo.UseShellExecute = false;

            //Clear any existing install
            WrGrdProc.StartInfo.Arguments = "/uninstalltunnelservice Lanstaller";

            WrGrdProc.StartInfo.Arguments = "/installtunnelservice \"" + cfdir + "\"";          
            WrGrdProc.Start();
            WrGrdProc.WaitForExit();

            //Set VPN to Manual start.
            Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\WireGuardTunnel$Lanstaller", true).SetValue("Start", "3", RegistryValueKind.DWord);
            


            //Send Public Client Key back.
            return pubkey;


        }

    }
}
