using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

//using static Lanstaller.ClientSoftwareClass;
using static LanstallerShared.LanstallerServer;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Net.Http;

using System.ComponentModel;
using System.Threading;
using LanstallerShared;
using System.Collections.Concurrent;

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


   
        string GetString(string Uri)
        {
            string response = "";
            while (string.IsNullOrEmpty(response))
            {
                try
                {
                    response = WC.DownloadString(Uri); 
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Failed to query server: " + ex.Message);
                    Thread.Sleep(1000);
                }
            }
            return response;
        }

       
        public static string GetSystemInfo(string setting)
        {
            APIClient AC = new APIClient();
            return AC.GetString(APIServer + "System/" + setting);
        }

        public static List<FileServer> GetFileServerFromAPI()
        {
            List<FileServer> Servers = new List<FileServer>();
            JArray ServerArray = JArray.Parse((new APIClient()).GetString(APIServer + "InstallationList/Server"));
            foreach(JToken SrvJA in ServerArray) 
            {
                FileServer FS = SrvJA.ToObject<FileServer>();
                if (FS.protocol == 1 && !(FS.path.EndsWith("/"))) //Trim / from url (getfiles will prepend / to source on copy operation).
                {
                    FS.path = FS.path + "/";
                }
                else if (FS.protocol == 2 && !(FS.path.EndsWith("\\")))
                {
                    FS.path = FS.path + "\\";
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

        public static List<CompatabilityMode> GetCompatibilitiesFromAPI(int SoftwareID)
        {
            List<CompatabilityMode> CPL = new List<CompatabilityMode>();
            JArray CPArray = GetListFromAPI("Compatibility", SoftwareID);
            foreach (var CP in CPArray)
            {
                CPL.Add(CP.ToObject<CompatabilityMode>());
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
