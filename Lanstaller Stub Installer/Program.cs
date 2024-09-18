using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace Lanstaller_Stub_Installer
{
    internal class Program
    {
        static string SL = "https://lanstaller.mrhsystems.com/StaticFiles/";
        static string IL = "C:\\Program Files\\Lanstaller\\";

        static void Main()
        {
            if (!System.IO.Directory.Exists(IL))
            {
                System.IO.Directory.CreateDirectory(IL);
            }
            
            //Reminder to update list of resource check at Lanstaller load.
                       
            DF("Lanstaller.exe");
            DF("Lanstaller Shared.dll");
            DF("Newtonsoft.Json.dll");
            DF("dll");
            DF("7z.exe");
            DF("7z.dll");

            Process.Start(IL + "Lanstaller.exe");

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut("C:\\Users\\Public\\Desktop\\Lanstaller.lnk");

            shortcut.TargetPath = IL + "Lanstaller.exe";
            shortcut.WorkingDirectory = IL;
            shortcut.IconLocation = IL + "Lanstaller.exe";

            shortcut.Save();

        }

        static void DF(string File)
        {
            if (System.IO.File.Exists(IL + File))
            {
                System.IO.File.Delete(IL + File);
            }
            WebClient WC = new WebClient();
            WC.DownloadFile(SL + File, IL + File);
        }

    }
}
