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

        //untested and incomplete.
        [Route("send")]
        public string Post([FromBody] JObject MessageData)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            testclass CM = MessageData.ToObject<testclass>();

            SharedChat.SendMessage(CM.message, CM.sender);

            return "ok";
        }


        class testclass
        {
            public string message;
            public string sender;

        }

        public string Get()
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SharedChat.GetFullChat());
        }

        [Route("update")]
        public string Update(string lastcheck)
        {
            if (Authentication.CheckLogon(HttpContext.Request) == false)
            {
                return "auth fail";
            }

            return JsonConvert.SerializeObject(SharedChat.GetChat(DateTime.Parse(lastcheck)));
        }
        

       
    }
}
