using Microsoft.AspNetCore.Mvc;
using LanstallerShared;
using System.IO.Compression;


namespace LanstallerWeb.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DownloadController : Controller
    {
        [HttpGet]
        public IActionResult Get(string regcode = "")
        {
            Maintenance.CleanupTemp(); //Cleanup existing tmp data.

            if (string.IsNullOrWhiteSpace(regcode))
            {
                throw new Exception("Empty registration code.");
            }

            //Get a new token code, build zip file with it configured in the config.ini file.
            //then provide zip as download to user.

            int newTokenid = Security.NewToken(regcode);
            string tokenStr = Security.GetToken(newTokenid).token;

            string tmppath = "./tmp/" + Guid.NewGuid().ToString() + "/";
            Directory.CreateDirectory(tmppath);
            //Write tmp config file.
            string tmpConfig = tmppath + "config.ini";
            StreamWriter SW = new StreamWriter(tmpConfig);
            SW.WriteLine("authkey=" + tokenStr);
            SW.WriteLine("apiserver=https://" + LanstallerWebSettings.serverAddress);
            SW.Close();

            string[] files = Directory.GetFiles("./LanstallerApp/");

            string zipFilePath = tmppath + "/Lanstaller.zip";

            using (FileStream zipFS = new FileStream(zipFilePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipFS, ZipArchiveMode.Create))
                {
                    foreach (string file in files)
                    {
                        archive.CreateEntryFromFile(file, Path.GetFileName(file));
                    }
                    archive.CreateEntryFromFile(tmpConfig, "config.ini");
                }
            }

            // Path to the file you want to download
            var fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
            return File(fileBytes, "application/octet-stream", "Lanstaller.zip");

        }


    }
}
