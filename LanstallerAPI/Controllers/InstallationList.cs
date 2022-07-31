using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        //Get Installation Size
        [Route("InstallSize")]
        public string InstallSize(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return SoftwareClass.GetInstallSize(swid).ToString();
        }

        //GetFiles
        [Route("Files")]
        public string Files(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFiles(swid));
        }

        [Route("Compatibility")]
        public string Compatibility(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetCompatibility(swid));
        }


        [Route("Shortcuts")]
        public string Shortcuts(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetShortcuts(swid));
        }

        [Route("Registry")]
        public string Registry(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetRegistry(swid));

        }


        [Route("Firewall")]
        public string Firewall(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFirewallRules(swid));
        }

        [Route("Preferences")]
        public string Preferences(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetPreferenceFiles(swid));
        }

        [Route("Redistributables")]
        public string Redistributables(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SoftwareClass.GetRedistributables(swid));
        }


        [Route("Serials")]
        public string Serials(int swid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetSerials(swid));
        }

    }
}
