using Lanstaller_Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LanstallerAPI.Controllers
{
    [ApiController]
    [Route("tools")]
    public class Tools : ControllerBase
    {
        public string GET()
        {
            return JsonConvert.SerializeObject(SoftwareClass.GetTools());
        }

    }
}
