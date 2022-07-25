using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace Lanstaller
{
    class WindowsInstallerClass
    {
        static List<InstallItem> InstallItemList = new List<InstallItem>();

        //HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall
        //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall


        public static void CheckProgram()
        {
            //Microsoft.Win32.Registry.GetValue(SN.regKey, SN.regVal, "").ToString();
            GetInstalledItems();
            foreach (InstallItem InstalledItem in InstallItemList)
            {
                //Console.WriteLine(InstalledItem.Name);
                Console.WriteLine("DN: " + InstalledItem.DisplayName);
            }


        }

        public static void GetInstalledItems()
        {
            InstallItemList.Clear();

            //32-Bit App
            string U32 = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\";
            foreach (string key in Registry.LocalMachine.OpenSubKey(U32).GetSubKeyNames())
            {
                AddItem(U32, key, false);
            }

            string U64 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\";
            //64-Bit App
            foreach (string key in Registry.LocalMachine.OpenSubKey(U64).GetSubKeyNames())
            {
                AddItem(U64, key, true);
            }

            foreach (InstallItem II in InstallItemList)
            {
                Console.WriteLine(II.DisplayName);
            }
        }

        static void AddItem(string RegLoc, string keyname, bool Arch64Bit)
        {
            RegistryKey RK = Registry.LocalMachine.OpenSubKey(RegLoc + keyname);

            if (RK.GetValueNames().Count() == 0)
                return; //skip empty.

            InstallItem tmpItm = new InstallItem();
            tmpItm.Arch64bit = Arch64Bit;
            tmpItm.Name = keyname;
            
            tmpItm.DisplayName = RK.GetValue("DisplayName", "").ToString();
            tmpItm.DisplayVersion = RK.GetValue("DisplayVersion", "").ToString();
            tmpItm.UninstallString = RK.GetValue("UninstallString", "").ToString();
            //tmpItm.Version = (int)RK.GetValue("Version");

            InstallItemList.Add(tmpItm); //Add to main list.
        }

       



        class InstallItem
        {
            public string Name;
            public string DisplayName;
            public string DisplayVersion;
            public string UninstallString;
            //public int Version;
            public bool Arch64bit;


        }


    }
}
