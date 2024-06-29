using Microsoft.AspNetCore.Mvc;
using Lanstaller_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Lanstaller_Shared.SharedChat;
using Microsoft.AspNetCore.Authorization;


namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("chat")]
    public class Chat : ControllerBase
    {
        [HttpGet, Authorize]
        public string Get()
        {
            return JsonConvert.SerializeObject(SharedChat.GetFullChat());
        }

        [HttpPost,Route("send"), Authorize]
        public string Post([FromBody] ChatMessage cm)
        {
            SendMessage(cm.message,cm.sender);
            return "";
        }

        [HttpGet,Route("check"), Authorize]
        public string GetNewCount(int lastid)
        {
            return SharedChat.GetMessageCount(lastid).ToString();
        }


       
    }
}
