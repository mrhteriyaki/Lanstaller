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
            return JsonConvert.SerializeObject(SoftwareClass.GetFileServer("web"));
        }

        //GetFiles
        [Route("Files")]
        public string Files(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFiles(id));
        }

        //Get Directories.
        [Route("Directories")]
        public string Directories(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SoftwareClass.GetDirectories(id));
        }

        [Route("Compatibility")]
        public string Compatibility(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetCompatibility(id));
        }


        [Route("Shortcuts")]
        public string Shortcuts(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetShortcuts(id));
        }

        [Route("Registry")]
        public string Registry(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetRegistry(id));

        }


        [Route("Firewall")]
        public string Firewall(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            if (id == null || id == 0)
            {
                return "id missing";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFirewallRules(id));
        }

        [Route("Preferences")]
        public string Preferences(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetPreferenceFiles(id));
        }

        [Route("Redistributables")]
        public string Redistributables(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SoftwareClass.GetRedistributables(id));
        }


        [Route("Serials")]
        public string Serials(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetSerials(id));
        }

        [Route("AvailableSerials")]
        public string AvailableSerials(int id)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return JsonConvert.SerializeObject(SoftwareClass.UserSerial.GetUserSerials(id));
        }

        
    }
}
