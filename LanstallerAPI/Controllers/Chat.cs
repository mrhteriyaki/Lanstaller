using Microsoft.AspNetCore.Mvc;
using Lanstaller_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Lanstaller_Shared.SharedChat;


namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("chat")]
    public class Chat : ControllerBase
    {
        public string Get()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SharedChat.GetFullChat());
        }

        [Route("send")]
        public string Post([FromBody] ChatMessage cm)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            SendMessage(cm.message,cm.sender);
            return "";
        }

       

        [Route("check")]
        public string GetNewCount(int lastid)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            return SharedChat.GetMessageCount(lastid).ToString();
        }


       
    }
}
