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

namespace Lanstaller.Classes
{
    public class APIClient
    {
        //Client side functions to access Lanstaller API.
        
        public static WebClient WC = new System.Net.WebClient();
        public static string APIServer = "";
        static string _authkey = "";
        public static void SetAuthKey(string authkey)
        {
            _authkey = authkey;
            WC.Headers.Add("authorization", _authkey);
        }
       
        public static Server GetFileServerFromAPI()
        {
            return (Server)JsonConvert.DeserializeObject(WC.DownloadString(APIServer + "InstallationList/Server"));
        }
        
        static JArray GetInstallationList(string ListName, int SoftwareID)
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
        
        //Get Tools
        public static List<Tool> GetToolsListFromAPI()
        {
            //List<Tool> GetTools
            List<Tool> TLL = new List<Tool>();
            JArray ToolArray = JArray.Parse(WC.DownloadString(APIServer + "Tools"));
            foreach (var TL in ToolArray)
            {
                TLL.Add(TL.ToObject<Tool>());
            }
            return TLL;
        }


        //Get Files.
        public static List<FileCopyOperation> GetFilesListFromAPI(int SoftwareID)
        {
            List<FileCopyOperation> FCL = new List<FileCopyOperation>();
            JArray FileArray = GetInstallationList("Files", SoftwareID);
            foreach (var FC in FileArray)
            {
                FCL.Add(FC.ToObject<FileCopyOperation>());
            }
            return FCL;
        }


        //Shortcuts
        public static List<ShortcutOperation> GetShortcutListFromAPI(int SoftwareID)
        {
            List<ShortcutOperation> SCL = new List<ShortcutOperation>();
            JArray SCArray = GetInstallationList("Shortcuts", SoftwareID);
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
            JArray RGArray = GetInstallationList("Registry",SoftwareID);
            foreach(var RG in RGArray)
            {
                RGL.Add(RG.ToObject<RegistryOperation>());
            }
            return RGL;
        }

        //Firewall
        public static List<FirewallRule> GetFirewallRulesListFromAPI(int SoftwareID)
        {
            List<FirewallRule> FWL = new List<FirewallRule>();
            JArray FRArray = GetInstallationList("Firewall",SoftwareID);
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
            JArray Array = GetInstallationList("Preferences",SoftwareID);
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
            JArray Array = GetInstallationList("Redistributables",SoftwareID);
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
            JArray Array = GetInstallationList("Serials",SoftwareID);
            foreach (var itm in Array)
            {
                LST.Add(itm.ToObject<SerialNumber>());
            }
            return LST;
        }
    }
}
