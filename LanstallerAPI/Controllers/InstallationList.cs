using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstallationList : ControllerBase
    {

        //List of available software.
        [Route("Software")]
        public string Software()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.LoadSoftware());
        }

        //Get Servers.
        [Route("Server")]
        public string Server()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFileServer());
        }

        //GetFiles
        [Route("Files")]
        public string Files(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetFiles();
            //SoftwareClass.GetFiles(12);

            return JsonConvert.SerializeObject(SWC.FileCopyList);
        }

        [Route("Registry")]
        public string Registry(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetRegistry();
            return JsonConvert.SerializeObject(SWC.RegistryList);

        }

        [Route("Shortcuts")]
        public string Shortcuts(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetShortcuts();
            return JsonConvert.SerializeObject(SWC.ShortcutList);
        }

        [Route("Firewall")]
        public string Firewall(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetFirewallRules();
            return JsonConvert.SerializeObject(SWC.FirewallRuleList);
        }

        [Route("Preferences")]
        public string Preferences(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetPreferenceFiles();
            return JsonConvert.SerializeObject(SWC.PreferenceOperationList);
        }

        [Route("Redistributables")]
        public string Redistributables(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetRedistributables();
            return JsonConvert.SerializeObject(SWC.RedistributableList);
        }


        [Route("Serials")]
        public string Serials(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            SoftwareClass SWC = new SoftwareClass();
            SWC.Identity.id = swid;
            SWC.GetSerials();
            return JsonConvert.SerializeObject(SWC.SerialList);
        }

    }
}
