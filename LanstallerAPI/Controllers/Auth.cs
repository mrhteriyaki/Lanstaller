using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class Auth : ControllerBase
    {
        //Provides authentication for NGINX Proxy to access files.
        //[Route("auth")]
        public string GET()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == true)
            {
                return "";
            }
            else
            {
                //Auth Fail.
                Response.StatusCode = 400;
                return "";
            }
        }
    }
}
