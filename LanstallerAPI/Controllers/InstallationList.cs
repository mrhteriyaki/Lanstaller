using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LanstallerAPI.Controllers
{
    public class InstallationList : Controller
    {

        //Provide list of all installations.

        public IActionResult Index()
        {
            return View();
        }
    }
}
