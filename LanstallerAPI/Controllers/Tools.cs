using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("tools")]
    public class Tools : ControllerBase
    {
        public string GET()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }     
            return JsonConvert.SerializeObject(SoftwareClass.GetTools());
        }

    }
}
