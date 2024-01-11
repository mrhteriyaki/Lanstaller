using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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

            //Shutdown any open Lanstaller.
            Process LSTKill = new Process();
            LSTKill.StartInfo.FileName = "C:\\Windows\\System32\\taskkill.exe";
            LSTKill.StartInfo.Arguments = "/f /im Lanstaller.exe";
            LSTKill.Start();
            LSTKill.WaitForExit();

            Thread.Sleep(200);

            //Files to download and replace.
            List<string> FileList = new List<string>();
            FileList.Add("Lanstaller.exe");
            FileList.Add("Lanstaller Shared.dll");
            FileList.Add("Newtonsoft.Json.dll");
            FileList.Add("Pri.LongPath.dll");
            FileList.Add("7z.exe");
            FileList.Add("7z.dll");

            foreach(string file in FileList)
            {
                File.Move(file, file + ".bak"); //Backup files.
            }

            foreach (string file in FileList)
            {
                try
                {
                    DF(file); //Download files.
                }
                catch(Exception ex)
                {
                    //Restore files - delete any already downloaded.
                    foreach(string file2 in FileList)
                    {
                        if (File.Exists(file2))
                        {
                            File.Delete(file2);
                        }
                        File.Move(file2 + ".bak", file2); //restore backup.
                    }
                    break;
                }
            }


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
