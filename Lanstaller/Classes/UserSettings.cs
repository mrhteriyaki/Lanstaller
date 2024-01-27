using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
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
            Registry.LocalMachine.CreateSubKey(SettingsKey, true);


            //Get Subkey settings.
            RegistryKey LanstallerKey = Registry.LocalMachine.OpenSubKey(SettingsKey, true);
            
            //Install Directory
            InstallDirectory = LanstallerKey.GetValue("installdir", defaultinstalldir).ToString();
            //Screen.PrimaryScreen requires app.manifest settings DpiAwareness and DpiAware to show correct value after Windows scaling.
            ScreenWidth = int.Parse(LanstallerKey.GetValue("screenwidth", Screen.PrimaryScreen.Bounds.Width).ToString());
            ScreenHeight = int.Parse(LanstallerKey.GetValue("screenheight", Screen.PrimaryScreen.Bounds.Height).ToString());
            Username = LanstallerKey.GetValue("username", System.Environment.UserName).ToString();

        }


        public static void SetInstallDirectory(string Directory)
        {
            InstallDirectory = Directory;

            if (Directory.EndsWith("\\"))
            {
                InstallDirectory = Directory.Substring(0, Directory.Length - 1);
            }

            Registry.LocalMachine.OpenSubKey(SettingsKey, true).SetValue("installdir", InstallDirectory);

        }

        public static void SetWidth(int Value)
        {
            Registry.LocalMachine.OpenSubKey(SettingsKey, true).SetValue("screenwidth", Value);
            ScreenWidth = Value;
        }
        public static void SetHeight(int Value)
        {
            Registry.LocalMachine.OpenSubKey(SettingsKey, true).SetValue("screenheight", Value);
            ScreenHeight = Value;
        }

        public static void SetUsername(string Name)
        {
            Registry.LocalMachine.OpenSubKey(SettingsKey, true).SetValue("username", Name);
            Username = Name;
        }


        public static bool CheckInstallDirectoryValid()
        {
            return System.IO.Directory.Exists(InstallDirectory);
        }




    }
}
