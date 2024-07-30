using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

namespace Lanstaller.Updater
{
    internal class Program
    {
        static string serveraddr;

        static void Main(string[] args)
        {
            Console.WriteLine("Lanstaller Updater");

            if (args.Length == 0)
            {
                return;
            }
            else
            {
                if (args[0].ToString().EndsWith("/"))
                {
                    Console.WriteLine("Server address cannot end with /");
                    return;
                }
                serveraddr = args[0].ToString() + "/StaticFiles/";
            }



            //update this to read from local config file.

            //Shutdown any open Lanstaller.
            try
            {
                Console.WriteLine("Stopping Lanstaller process.");
                Process LSTKill = new Process();
                LSTKill.StartInfo.FileName = "C:\\Windows\\System32\\taskkill.exe";
                LSTKill.StartInfo.Arguments = "/f /im Lanstaller.exe";
                LSTKill.Start();
                LSTKill.WaitForExit();

                Thread.Sleep(500);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }

            //Files to download and replace.
            string[] FileList = {
                "Lanstaller.exe",
                "Lanstaller Shared.dll",
                "Newtonsoft.Json.dll",
                "Pri.LongPath.dll",
                "7z.exe",
                "7z.dll"
            };


            RemoveBackups(FileList);

            Console.WriteLine("Renaming files for backup.");

            while (CheckFilesExist(FileList))
            {
                foreach (string file in FileList)
                {
                    try
                    {
                        File.Move(file, file + ".bak"); //Backup files.
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error moving file: " + ex.ToString());
                    }
                }
                Thread.Sleep(1000);
            }



            foreach (string file in FileList)
            {
                try
                {
                    DF(file); //Download files.
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to download file: " + ex.Message);
                    Console.ReadLine();
                    //Restore files - delete any already downloaded.
                    foreach (string file2 in FileList)
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

            RemoveBackups(FileList);


            //Launch.
            Process LST = new Process();
            LST.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "/Lanstaller.exe";
            LST.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            LST.Start();

        }

        static bool CheckFilesExist(string[] FileList)
        {
            foreach (string file in FileList)
            {
                if (File.Exists(file))
                {
                    return true;
                }
            }
            return false;
        }

        static void RemoveBackups(string[] FileList)
        {
            foreach (string file in FileList)
            {
                if (File.Exists(file + ".bak"))
                {
                    File.Delete(file + ".bak");
                }
            }
            Thread.Sleep(500); //Allow file system completion time.
        }

        static void DF(string File)
        {
            WebClient WC = new WebClient();
            string destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, File);
            string source = serveraddr + File;

            try
            {
                WC.DownloadFile(source, destination);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to download: " + serveraddr + File + " -> " + destination);
                throw ex;
            }
        }
    }
}
