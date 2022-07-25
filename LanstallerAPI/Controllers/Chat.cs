using Microsoft.AspNetCore.Mvc;
using Lanstaller_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("chat")]
    public class Chat : ControllerBase
    {

        //untested and incomplete.

        public string GetFullChat()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SharedChat.GetFullChat);
        }

        public string GetChat(string lastcheck)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SharedChat.GetChat(DateTime.Parse(lastcheck)));
        }

        public string Post(string jsondata)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }
            
            JsonConvert.DeserializeObject(jsondata);

            SharedChat.ChatMessage tmpmsg;
            tmpmsg = (SharedChat.ChatMessage)JsonConvert.DeserializeObject(jsondata);

            SharedChat.SendMessage(tmpmsg.message, tmpmsg.sender);

            return "ok";
        }
    }
}
