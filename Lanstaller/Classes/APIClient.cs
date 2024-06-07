using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

//using static Lanstaller.ClientSoftwareClass;
using static Lanstaller_Shared.SoftwareClass;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Net.Http;

using System.ComponentModel;
using System.Threading;
using Lanstaller_Shared;
using System.Collections.Concurrent;
using Lanstaller_Shared.Models;

namespace Lanstaller.Classes
{
    public class APIClient
    {
        //Consolidate multiple download requests into single api client.
        //Many Webclient objects will cause excessive port usage stuck in time_wait open state.
        //Seperate objects for multi-threading or exception will occur 'WebClient does not support concurrent I/O Operations'

        static string _authkey = "";
        public static string APIServer = "";
        WebClient WC;

        public APIClient()
        {
            WC = new WebClient();
            WC.Headers.Add("authorization", _authkey);
        }

        public static void SetAuth(string authkey)
        {
            _authkey = authkey;
            DownloadTask.SetAuth(authkey);
        }


        public static void DownloadFile(string Source, string Destination)
        {
            APIClient AC = new APIClient();
            AC.Download(Source, Destination);
            AC.WC.Dispose();
        }



        public void Download(string Source, string Destination)
        {
            //File download function.
            try
            {
                WC.DownloadFile(Source, Destination);
            }
            catch (WebException ex)
            {
                Console.WriteLine("Download failed: " + Source + " Error:" + ex.Message);
                DialogResult DR = MessageBox.Show("File Download Error\n" + Source + "\nRetry or Cancel (Skip this file)?\n" + ex.ToString(), "Download Error", MessageBoxButtons.RetryCancel);
                if (DR == DialogResult.Retry)
                {
                    DownloadFile(Source, Destination);
                    return;
                }
                else if (DR == DialogResult.Cancel)
                {
                    //Skip - Return ok.
                    return;
                }
            }
        }

        string GetString(string Uri)
        {
            return WC.DownloadString(Uri);
        }

       
        public static string GetSystemInfo(string setting)
        {
            APIClient AC = new APIClient();
            return AC.GetString(APIServer + "System/" + setting);
        }

        public static List<Server> GetFileServerFromAPI()
        {
            List<Server> Servers = new List<Server>();
            JArray ServerArray = JArray.Parse((new APIClient()).GetString(APIServer + "InstallationList/Server"));
            foreach(var SrvJA in  ServerArray) 
            {
                Server FS = SrvJA.ToObject<Server>();
                if (FS.path.EndsWith("/")) //Trim / from url (getfiles will prepend / to source on copy operation).
                {
                    FS.path = FS.path.Substring(0, FS.path.Length - 1);
                }
                Servers.Add(FS);
            }
            return Servers;
        }

        static JArray GetListFromAPI(string ListName, int SoftwareID)
        {
            string url = APIServer + "InstallationList/" + ListName + "?id=" + SoftwareID.ToString();
            try
            {
                string Reply = (new APIClient()).GetString(url);
                return JArray.Parse(Reply);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to download install list details from URL:" + url + ex.Message + "\nInstall will terminate.");
            }
            Application.Exit();
            return null;
        }

        //Get Software.
        public static List<SoftwareInfo> GetSoftwareListFromAPI()
        {
            List<SoftwareInfo> SWL = new List<SoftwareInfo>();
            JArray SoftwareArray = JArray.Parse((new APIClient()).GetString(APIServer + "InstallationList/Software"));
            foreach (var SW in SoftwareArray)
            {
                SWL.Add(SW.ToObject<SoftwareInfo>());
            }
            return SWL;
        }


        //Get Files.
        public static List<FileCopyOperation> GetFilesListFromAPI(int SoftwareID)
        {
            List<FileCopyOperation> FCL = new List<FileCopyOperation>();
            JArray FileArray = GetListFromAPI("Files", SoftwareID);
            foreach (var FC in FileArray)
            {
                FCL.Add(FC.ToObject<FileCopyOperation>());
            }
            return FCL;
        }

        //Get Directories.
        public static List<string> GetDirectoriesFromAPI(int SoftwareID)
        {
            List<string> DirList = new List<string>();
            JArray DirArray = GetListFromAPI("Directories", SoftwareID);
            foreach (var DO in DirArray)
            {
                DirList.Add(DO.ToObject<string>());
            }
            return DirList;
        }


        //Shortcuts
        public static List<ShortcutOperation> GetShortcutListFromAPI(int SoftwareID)
        {
            List<ShortcutOperation> SCL = new List<ShortcutOperation>();
            JArray SCArray = GetListFromAPI("Shortcuts", SoftwareID);
            foreach (var SC in SCArray)
            {
                SCL.Add(SC.ToObject<ShortcutOperation>());
            }
            return SCL;
        }

        //Registry.
        public static List<RegistryOperation> GetRegistryListFromAPI(int SoftwareID)
        {
            List<RegistryOperation> RGL = new List<RegistryOperation>();
            JArray RGArray = GetListFromAPI("Registry", SoftwareID);
            foreach (var RG in RGArray)
            {
                RGL.Add(RG.ToObject<RegistryOperation>());
            }
            return RGL;
        }

        public static List<Compatibility> GetCompatibilitiesFromAPI(int SoftwareID)
        {
            List<Compatibility> CPL = new List<Compatibility>();
            JArray CPArray = GetListFromAPI("Compatibility", SoftwareID);
            foreach (var CP in CPArray)
            {
                CPL.Add(CP.ToObject<Compatibility>());
            }
            return CPL;
        }


        //Firewall
        public static List<FirewallRule> GetFirewallRulesListFromAPI(int SoftwareID)
        {
            List<FirewallRule> FWL = new List<FirewallRule>();
            JArray FRArray = GetListFromAPI("Firewall", SoftwareID);
            foreach (var FR in FRArray)
            {
                FWL.Add(FR.ToObject<FirewallRule>());
            }
            return FWL;
        }

        //Preferences
        public static List<PreferenceOperation> GetPreferencesListFromAPI(int SoftwareID)
        {
            List<PreferenceOperation> LST = new List<PreferenceOperation>();
            JArray Array = GetListFromAPI("Preferences", SoftwareID);
            foreach (var itm in Array)
            {
                LST.Add(itm.ToObject<PreferenceOperation>());
            }
            return LST;
        }


        //Redistributables
        public static List<Redistributable> GetRedistributablesListFromAPI(int SoftwareID)
        {
            List<Redistributable> LST = new List<Redistributable>();
            JArray Array = GetListFromAPI("Redistributables", SoftwareID);
            foreach (var itm in Array)
            {
                LST.Add(itm.ToObject<Redistributable>());
            }
            return LST;
        }


        //Serials
        public static List<SerialNumber> GetSerialsListFromAPI(int SoftwareID)
        {
            List<SerialNumber> LST = new List<SerialNumber>();
            JArray Array = GetListFromAPI("Serials", SoftwareID);
            foreach (var itm in Array)
            {
                LST.Add(itm.ToObject<SerialNumber>());
            }
            return LST;
        }


        public static List<UserSerial> GetAvailableSerialsFromAPI(int SerialID)
        {
            List<UserSerial> SerialList = new List<UserSerial>();
            JArray Array = GetListFromAPI("AvailableSerials", SerialID);
            foreach (var itm in Array)
            {
                SerialList.Add(itm.ToObject<UserSerial>());
            }
            return SerialList;

        }

        public static void SetAvailableSerialsFromAPI(int UserSerialID)
        {
            if (UserSerialID > 0)
            {
                (new APIClient()).GetString(APIServer + "Serials?id=" + UserSerialID.ToString());
            }
        }




    }
}
