using Lanstaller_Shared;
using Lanstaller_Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("Serials")]
    public class Serials : ControllerBase
    {
        [HttpGet, Authorize]
        public string Index(int id)
        {
            UserSerial.SetAvailableSerial(id);
            return "ok";
        }
    }
}
