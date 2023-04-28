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
using System.Net.Http.Headers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.ComponentModel;
using System.Threading;

namespace Lanstaller.Classes
{
    public class APIClient
    {
        //Client side functions to access Lanstaller API.

        static WebClient WC = new System.Net.WebClient(); //Static webclient to reduce time_wait open ports.
        public static string APIServer = "";
        static string _authkey = "";


        public static void Setup(string authkey)
        {
            _authkey = authkey;
            WC.Headers.Clear();
            WC.Headers.Add("authorization", _authkey);
        }

        public static void DownloadFile(string Source, string Destination)
        {
            //File download function.
            try
            {
                WC.DownloadFile(Source, Destination);
            }
            catch (WebException ex)
            {
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

        public class DownloadWithProgress
        {
            public long downloadedbytes = 0;
            string _source;
            string _destination;
            public bool completed = false;

            public DownloadWithProgress(string Source, string Destination) { _source = Source; _destination = Destination; }
            public void Download()
            {
                WC.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                WC.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                WC.DownloadFileAsync(new Uri(_source),_destination);
            }
            void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                downloadedbytes = e.BytesReceived;
                //double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());   

            }
            void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                completed = true;
            }

        }


        static string DownloadString(string ListName, int SoftwareID)
        {
            return WC.DownloadString(APIServer + "InstallationList/" + ListName + "?swid=" + SoftwareID.ToString());
        }

        public static string GetSystemInfo(string setting)
        {
            return WC.DownloadString(APIServer + "System/" + setting);
        }

        public static Server GetFileServerFromAPI()
        {
            //return (Server)JsonConvert.DeserializeObject(WC.DownloadString(APIServer + "InstallationList/Server"));
            Server FS = JObject.Parse(WC.DownloadString(APIServer + "InstallationList/Server")).ToObject<Server>();
            return FS;

        }

        static JArray GetListFromAPI(string ListName, int SoftwareID)
        {
            string Reply = WC.DownloadString(APIServer + "InstallationList/" + ListName + "?swid=" + SoftwareID.ToString());
            return JArray.Parse(Reply);
        }

        //Get Software.
        public static List<SoftwareInfo> GetSoftwareListFromAPI()
        {
            List<SoftwareInfo> SWL = new List<SoftwareInfo>();
            JArray SoftwareArray = JArray.Parse(WC.DownloadString(APIServer + "InstallationList/Software"));
            foreach (var SW in SoftwareArray)
            {
                SWL.Add(SW.ToObject<SoftwareInfo>());
            }
            return SWL;
        }

        //Get Installation Size.
        public static long GetInstallSizeFromAPI(int SoftwareID)
        {
            List<FileCopyOperation> FCL = new List<FileCopyOperation>();
            string returnstr = DownloadString("InstallSize", SoftwareID);
            return long.Parse(returnstr);
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


        public static List<string> GetAvailableSerialsFromAPI(int SerialID)
        {
            List<string> SerialList = new List<string>();
            JArray Array = GetListFromAPI("AvailableSerials", SerialID);
            foreach (var itm in Array)
            {
                SerialList.Add(itm.ToObject<string>());
            }
            return SerialList;

        }

        public static void SetAvailableSerialsFromAPI(int SerialID, string Serial)
        {
            string _serial = HttpUtility.UrlEncode(Serial);
            WC.DownloadString(APIServer + "Serials?id=" + SerialID.ToString() + "&serial=" + _serial);
        }




    }
}
