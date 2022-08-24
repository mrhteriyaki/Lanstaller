using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lanstaller.Updater
{
    internal class Program
    {
        static string serveraddr;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            serveraddr = args[0].ToString() + "StaticFiles/";

            //Download Update.
            Process LSTKill = new Process();
            LSTKill.StartInfo.FileName = "C:\\Windows\\System32\\taskkill.exe";
            LSTKill.StartInfo.Arguments = "/f /im Lanstaller.exe";
            LSTKill.Start();
            LSTKill.WaitForExit();

            Thread.Sleep(200);

            DF("Lanstaller.exe");
            DF("Lanstaller Shared.dll");
            DF("Newtonsoft.Json.dll");
            DF("Pri.LongPath.dll");
            DF("7z.exe");
            DF("7z.dll");


            //Launch.
            Process LST = new Process();
            LST.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "/Lanstaller.exe";
            LST.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            LST.Start();

        }

        static void DF(string File)
        {
            WebClient WC = new WebClient();
            string destination = AppDomain.CurrentDomain.BaseDirectory + "/" + File;
            int retry = 0;
            while (System.IO.File.Exists(destination))
            {
                try
                {
                    System.IO.File.Delete(destination);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                    retry += 1;
                    if (retry == 5)
                    {
                        throw new Exception("Unable to remove file: " + File + "\n" + ex.ToString());
                    }
                }
            }
            try
            {
                WC.DownloadFile(serveraddr + File, destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to download: " + serveraddr + File);
                throw ex;
            }
        }
    }
}
