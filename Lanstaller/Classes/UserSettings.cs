using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lanstaller.Classes;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
namespace Lanstaller
{
    class UserSettings
    {
        static string SettingsKey = "Software\\Lanstaller";
        public static readonly string defaultinstalldir = "C:\\Games";
        public static string InstallDirectory;

        public static int ScreenWidth;
        public static int ScreenHeight;
        public static string Username;

        public static void GetSettings()
        {
            //Generate base key if missing.
            Registry.CurrentUser.CreateSubKey(SettingsKey, true);


            //Get Subkey settings.
            RegistryKey LanstallerKey = Registry.CurrentUser.OpenSubKey(SettingsKey, true);

            //Install Directory
            InstallDirectory = LanstallerKey.GetValue("installdir", defaultinstalldir).ToString();
            //Screen.PrimaryScreen requires app.manifest settings DpiAwareness and DpiAware to show correct value after Windows scaling.
            Resolutions.Resolution MonitorRes = Resolutions.GetResolution();
            ScreenWidth = int.Parse(LanstallerKey.GetValue("screenwidth", MonitorRes.Width).ToString());
            ScreenHeight = int.Parse(LanstallerKey.GetValue("screenheight", MonitorRes.Height).ToString());
            Username = LanstallerKey.GetValue("username", System.Environment.UserName).ToString();
        }

        public static void SetInstallDirectory(string Directory)
        {
            InstallDirectory = Directory;

            if (Directory.EndsWith("\\"))
            {
                InstallDirectory = Directory.Substring(0, Directory.Length - 1);
            }

            Registry.CurrentUser.OpenSubKey(SettingsKey, true).SetValue("installdir", InstallDirectory);

        }

        public static void SetWidth(int Value)
        {
            var key = Registry.CurrentUser.OpenSubKey(SettingsKey, true);
            if (Value == Resolutions.GetResolution().Width)
            {
                key.DeleteValue("screenwidth", false); // Delete if value matches screen width
            }
            else
            {
                key.SetValue("screenwidth", Value);
                ScreenWidth = Value;
            }
        }
        public static void SetHeight(int Value)
        {
            var key = Registry.CurrentUser.OpenSubKey(SettingsKey, true);
            if (Value == Resolutions.GetResolution().Height)
            {
                key.DeleteValue("screenheight", false); // Delete if value matches screen width
            }
            else
            {
                key.SetValue("screenheight", Value);
                ScreenHeight = Value;
            }
        }

        public static void SetUsername(string Name)
        {
            Registry.CurrentUser.OpenSubKey(SettingsKey, true).SetValue("username", Name);
            Username = Name;
        }


        public static bool CheckInstallDirectoryValid()
        {
            return System.IO.Directory.Exists(InstallDirectory);
        }




    }
}
