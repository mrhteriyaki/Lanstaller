using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("Serials")]
    public class Serials : ControllerBase
    {
        public string Index(int id, string serial)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            SoftwareClass.SetAvailableSerial(id, serial);

            return "ok";
        }
    }
}
