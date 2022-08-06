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

        [Route("newtoken")]
        public string Post([FromBody] Security.Registration tokenrequest)
        {
            if (tokenrequest.name == null || tokenrequest.regcode == null)
            {
                return "fail1";
            }
            
            int reg_response = 0;
            reg_response = Security.NewToken(tokenrequest.name, tokenrequest.regcode);

            if (reg_response == 0)
            {
                return "fail2";
            }
             
            return Security.GetToken(reg_response).token;
        }
    }
}
