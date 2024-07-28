using Microsoft.AspNetCore.Mvc;

namespace LanstallerWeb.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DownloadController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Download Test.";
        }

    }
}
