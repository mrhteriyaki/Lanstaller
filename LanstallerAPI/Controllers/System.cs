using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class System : ControllerBase
    {
        //System version for Client to match.
        [Route("version")]
        public string Version()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }         
            return SoftwareClass.GetSystemData("version");
        }


        //Wireguard software install.
        [Route("wireguard_msi_url")]
        public string WireguardMSI()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return SoftwareClass.GetSystemData("wireguard_msi_url");
        }

    }
}
