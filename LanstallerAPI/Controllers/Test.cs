using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("test")]
    public class Test : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "ok";
        }
    }

    //Test function to check if API is online.
}



