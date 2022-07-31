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
            return SoftwareClass.GetSystemVersion().ToString();
        }
    }
}
