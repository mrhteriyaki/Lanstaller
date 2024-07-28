using Microsoft.AspNetCore.Mvc;
using Lanstaller_Shared;
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
            if (string.IsNullOrWhiteSpace(regcode))
            {
                throw new Exception("Empty registration code.");
            }

            //Get a new token code, build zip file with it configured in the config.ini file.
            //then provide zip as download to user.
            
            int newTokenid = Security.NewToken(regcode);

            string[] files = [];

            //
            // Create the zip file
            string zipFilePath =  "./tmp/zipfile.zip";
            using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
                {
                    foreach (string file in files)
                    {
                        // Ensure the file exists
                        if (System.IO.File.Exists(file))
                        {
                            // Add the file to the zip archive
                            archive.CreateEntryFromFile(file, Path.GetFileName(file));
                        }
                        else
                        {
                            Console.WriteLine($"File not found: {file}");
                        }
                    }
                }
            }

            // Path to the file you want to download
            var filePath = "path/to/your/file.ext";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = "downloadedFile.ext";

            return File(fileBytes, "application/octet-stream", fileName);

            //return "Download Test: " + regcode;
        }
       

    }
}
