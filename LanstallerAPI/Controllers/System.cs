using LanstallerShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class System : ControllerBase
    {
        //System version for Client to match.
        [HttpGet,Route("version"), Authorize]
        public string Version()
        {   
            return LanstallerServer.GetSystemData("version");
        }

        


    }
}
