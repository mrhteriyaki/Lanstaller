﻿using Lanstaller_Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class Auth : ControllerBase
    {
        //Provides authentication for NGINX Proxy to access files.
        //[Route("auth")]
        [HttpGet,Authorize]
        public string GET()
        {
            return "";
        }


        //Regcode returns token.
        [HttpPost,Route("newtoken")]
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