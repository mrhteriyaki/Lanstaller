using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lanstaller_Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstallationList : ControllerBase
    {

        //List of available software.
        [HttpGet,Route("Software"), Authorize]
        public string Software()
        {
            return JsonConvert.SerializeObject(SoftwareClass.LoadSoftware());
        }

        //Get Servers.
        [HttpGet,Route("Server"), Authorize]
        public string Server()
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetFileServer());
        }

        //GetFiles
        [HttpGet,Route("Files"), Authorize]
        public string Files(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetFiles(id));
        }

        //Get Directories.
        [HttpGet,Route("Directories"), Authorize]
        public string Directories(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetDirectories(id));
        }

        [HttpGet,Route("Compatibility"), Authorize]
        public string Compatibility(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetCompatibility(id));
        }


        [HttpGet,Route("Shortcuts"), Authorize]
        public string Shortcuts(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetShortcuts(id));
        }

        [HttpGet,Route("Registry"), Authorize]
        public string Registry(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetRegistry(id));

        }


        [HttpGet,Route("Firewall"), Authorize]
        public string Firewall(int id)
        {
            if (id == null || id == 0)
            {
                return "id missing";
            }
            return JsonConvert.SerializeObject(SoftwareClass.GetFirewallRules(id));
        }

        [HttpGet,Route("Preferences"), Authorize]
        public string Preferences(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetPreferenceFiles(id));
        }

        [HttpGet,Route("Redistributables"), Authorize]
        public string Redistributables(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetRedistributables(id));
        }


        [HttpGet,Route("Serials"), Authorize]
        public string Serials(int id)
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetSerials(id));
        }

        [HttpGet,Route("AvailableSerials"), Authorize]
        public string AvailableSerials(int id)
        {
            return JsonConvert.SerializeObject(UserSerial.GetUserSerials(id));
        }

        
    }
}
