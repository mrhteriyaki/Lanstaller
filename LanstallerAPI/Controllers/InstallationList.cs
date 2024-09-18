using LanstallerShared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            return JsonConvert.SerializeObject(SoftwareInfo.LoadSoftware());
        }

        //Get Servers.
        [HttpGet,Route("Server"), Authorize]
        public string Server()
        {
            return JsonConvert.SerializeObject(FileServer.GetFileServer());
        }

        //GetFiles
        [HttpGet,Route("Files"), Authorize]
        public string Files(int id)
        {
            return JsonConvert.SerializeObject(FileCopyOperation.GetFiles(id));
        }

        //Get Directories.
        [HttpGet,Route("Directories"), Authorize]
        public string Directories(int id)
        {
            return JsonConvert.SerializeObject(FileCopyOperation.GetDirectories(id));
        }

        [HttpGet,Route("Compatibility"), Authorize]
        public string Compatibility(int id)
        {
            return JsonConvert.SerializeObject(CompatabilityMode.GetCompatibility(id));
        }


        [HttpGet,Route("Shortcuts"), Authorize]
        public string Shortcuts(int id)
        {
            return JsonConvert.SerializeObject(ShortcutOperation.GetShortcuts(id));
        }

        [HttpGet,Route("Registry"), Authorize]
        public string Registry(int id)
        {
            return JsonConvert.SerializeObject(RegistryOperation.GetRegistry(id));

        }


        [HttpGet,Route("Firewall"), Authorize]
        public string Firewall(int id)
        {
            if (id == null || id == 0)
            {
                return "id missing";
            }
            return JsonConvert.SerializeObject(FirewallRule.GetFirewallRules(id));
        }

        [HttpGet,Route("Preferences"), Authorize]
        public string Preferences(int id)
        {
            return JsonConvert.SerializeObject(PreferenceOperation.GetPreferenceFiles(id));
        }

        [HttpGet,Route("Redistributables"), Authorize]
        public string Redistributables(int id)
        {
            return JsonConvert.SerializeObject(Redistributable.GetRedistributables(id));
        }


        [HttpGet,Route("Serials"), Authorize]
        public string Serials(int id)
        {
            return JsonConvert.SerializeObject(SerialNumber.GetSerials(id));
        }

        [HttpGet,Route("AvailableSerials"), Authorize]
        public string AvailableSerials(int id)
        {
            return JsonConvert.SerializeObject(UserSerial.GetUserSerials(id));
        }

        
    }
}
