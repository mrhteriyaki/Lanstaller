using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanstallerShared
{
    public class Logging
    {
        static readonly string logfile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp\\lanstaller.log";
        
        static object lockMessages = new object();

        public static void LogToFile(string message)
        {
            lock(lockMessages)
            {
                StreamWriter SW = new StreamWriter(logfile, true);
                Console.WriteLine("Logging: " + message);
                SW.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "," + message);
                SW.Close();
            }           
        }

        public static void ClearLog()
        {
            if (File.Exists(logfile))
            {
                File.Delete(logfile);
            }
        }



    }
}
