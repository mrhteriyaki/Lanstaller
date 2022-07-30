using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

using IWshRuntimeLibrary;
using System.Windows.Forms;

using System.Text.RegularExpressions; //Regex
using System.Diagnostics;

using System.IO;

using Lanstaller_Shared;

using static Lanstaller.Classes.APIClient;

namespace Lanstaller
{
    public class ClientSoftwareClass : SoftwareClass //Extension of shared Software class with client / windows exclusive functions.
    {
        static string status = "Status: Ready"; //Used for status label.
        static long InstallSize; //Size of current install.
        static long InstalledSize; //Progres of current install.

        //List<Servers> ServerList = new List<Servers>();

        //Locks.
        static readonly object _statuslock = new object();
        static readonly object _progresslock = new object();

        public List<SerialNumber> SerialList = new List<SerialNumber>();

        


        public void Install(bool installfiles, bool installregistry, bool installshortcuts, bool apply_windowssettings, bool apply_preferences, bool install_redist)
        {
            //Run Installation

            if (installregistry)
            {
                SetStatus("Applying Registry - " + Identity.Name);
                List<RegistryOperation> RegistryList;

                //DB
                //RegistryList = GetRegistry(Identity.id);
                
                //Web
                RegistryList = GetRegistryListFromAPI(Identity.id);
                GenerateRegistry(RegistryList, SerialList);
            }


            if (installfiles)
            {
                SetStatus("Indexing - " + Identity.Name);
                List<FileCopyOperation> FCL;

                //DB
                //FCL = GetFiles(Identity.id);

                //Web
                FCL = GetFilesListFromAPI(Identity.id);

                InstalledSize = 0; //reset install size.
                InstallSize = GetInstallSize(Identity.id);

                SetStatus("Copying Files - " + Identity.Name);
                CopyFiles(FCL);
            }

            if (installshortcuts)
            {
                SetStatus("Generating Shortcuts - " + Identity.Name);
                List<ShortcutOperation> SCO;

                //DB
                //SCO = GetShortcuts(Identity.id);

                //WEB
                SCO = GetShortcutListFromAPI(Identity.id);

                GenerateShortcuts(SCO);
            }

            SetStatus("Adding firewall rules - " + Identity.Name);
            //firewall rules.
            if (apply_windowssettings)
            {
                List<FirewallRule> FWL;

                //DB
                //FWL = GetFirewallRules(Identity.id);

                //WEB
                FWL = GetFirewallRulesListFromAPI(Identity.id);

                GenerateFirewallRules(FWL);
                GenerateCompatibility();
            }

            SetStatus("Applying Preferences - " + Identity.Name);
            if (apply_preferences)
            {
                List<PreferenceOperation> POList;
                
                //DB
                //POList = GetPreferenceFiles(Identity.id);
                
                //WEB
                POList = GetPreferencesListFromAPI(Identity.id);

                GeneratePreferenceFiles(POList);
            }

            SetStatus("Installing Redistributables - " + Identity.Name);
            if (install_redist)
            {
                List<Redistributable> RL;

                //DB
                //RL = GetRedistributables(Identity.id);

                //WEB
                RL = GetRedistributablesListFromAPI(Identity.id);

                GenerateRedistributables(RL);
            }

            SetStatus("Install Complete:" + Environment.NewLine + Identity.Name);

        }


        static void SetStatus(string message)
        {
            lock (_statuslock)
            {
                status = "Status: " + message.Replace("&", "&&");

            }
        }

        public static string GetStatus()
        {
            lock (_statuslock)
            {
                return status;
            }
        }

        public static void SetProgress(long currentbytes)
        {
            lock (_progresslock)
            {
                InstalledSize = currentbytes;
            }
        }

        public static int GetProgressPercentage()
        {
            lock (_progresslock)
            {
                if (InstallSize == 0 || InstalledSize == 0)
                {
                    return 0;
                }
                double perc = ((double)InstalledSize / (double)InstallSize) * 100;

                return Convert.ToInt32(perc);
            }
        }




        void GeneratePreferenceFiles(List<PreferenceOperation> PreferenceOperationList)
        {
            foreach (PreferenceOperation PO in PreferenceOperationList)
            {
                ReplacePreferenceFile(PO.filename, PO.target, PO.replace);
            }
        }



        void GenerateFirewallRules(List<FirewallRule> FirewallRuleList)
        {
            //FirewallRule
            foreach (FirewallRule fwr in FirewallRuleList)
            {
                //netsh advfirewall firewall add rule name="My Application" dir=in action=allow program="C:\games\The Call of Duty\CoDMP.exe" enable=yes
                Process FWNetSHProc = new Process();
                FWNetSHProc.StartInfo.FileName = "c:\\windows\\system32\\netsh.exe";
                FWNetSHProc.StartInfo.Arguments = "advfirewall firewall add rule name=\"" + fwr.softwarename + "\" dir=in action=allow program=\"" + ReplaceVariable(fwr.exepath) + "\" enable=yes";
                FWNetSHProc.StartInfo.UseShellExecute = false;
                FWNetSHProc.StartInfo.RedirectStandardOutput = true;
                FWNetSHProc.StartInfo.CreateNoWindow = true;
                FWNetSHProc.Start();
            }
        }

        void GenerateCompatibility()
        {
            //Set compatibility flags in registry.

            string QueryString = "select filename,compat_type from tblCompatibility where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();

            while (SQLOutput.Read())
            {
                GenerateCompatiblity(SQLOutput[0].ToString(), (int)SQLOutput[1]);
            }

            SQLConn.Close();

        }

        public void GenerateRedistributables(List<Redistributable> RedistributableList) //Incomplete.
        {
            foreach (Redistributable Redist in RedistributableList)
            {

            }

            //Check filecheck, if blank, check name against windows installations.
        }

        public static void GenerateSerials(List<SerialNumber> SerialList)
        {
            //Updates SerialList with user input.

            foreach (SerialNumber SN in SerialList)
            {
                //Prompt user to enter serial numbers.
                //string prom = SN.name + Environment.NewLine + "(Note: spaces and dashes will be removed automatically.)";
                //SN.serialnumber = Microsoft.VisualBasic.Interaction.InputBox(prom, SN.name).Replace(" ","").Replace("-",""); //Strip whitespace.

                frmSerial SF = new frmSerial();
                SF.TopMost = true;
                SF.Text = "Serial Number Prompt";
                SF.lblTitle.Text = "Input Serial Number for " + SN.name;
                SF.FormBorderStyle = FormBorderStyle.FixedSingle;

                if (!SN.regKey.Equals(""))
                {
                    try
                    {
                        SF.txtSerial.Text = Microsoft.Win32.Registry.GetValue(SN.regKey, SN.regVal, "").ToString();
                    }
                    catch (Exception ex)
                    {
                        //occurs on null exception due to missing key.
                        Console.WriteLine(ex.Message);
                    }

                }
                SF.ShowDialog();
                SN.serialnumber = SF.txtSerial.Text;
            }
        }



        void CopyFiles(List<FileCopyOperation> FileCopyList)
        {

            //Calculate total copy size.
            long totalbytes = GetInstallSize();
            double totalgbytes = (double)totalbytes / 1073741824;


            //Generate Directories.
            List<string> DirectoryList = new List<string>();
            foreach (FileCopyOperation FCO in FileCopyList)
            {
                //Update destination variables.
                FCO.destination = ReplaceVariable(FCO.destination);

                //Get File Directory, trim end filename.
                string filedirectory = FCO.destination.Substring(0, FCO.destination.LastIndexOf("\\"));
                string checkdir = "";
                //Split up file directory for parent folders.
                int count = 0;
                foreach (string FileDirSection in filedirectory.Split(Char.Parse("\\")))
                {
                    //Check each directory including parents against list and add if missing.
                    checkdir = checkdir + FileDirSection + "\\";

                    count++;
                    if (count == 1)
                    {
                        //Skip Root of drive.
                        continue;
                    }

                    bool missing = true;
                    foreach (string existingdir in DirectoryList)
                    {
                        if (existingdir.Equals(checkdir))
                        {
                            missing = false;
                            break;
                        }
                    }
                    if (missing == true)
                    {

                        DirectoryList.Add(checkdir);
                    }

                }

            }


            SetStatus("Installing: " + Identity.Name + Environment.NewLine + "Status: Generating Directories");

            foreach (string dir in DirectoryList)
            {
                if (Pri.LongPath.Directory.Exists(dir) == false)
                {
                    Pri.LongPath.Directory.CreateDirectory(dir);
                }

            }

            Server FileServer = GetFileServer();


            int copycount = 0;
            long bytecounter = 0;
            //Copy Files.
            while (copycount < FileCopyList.Count)
            {
                FileCopyOperation FCO = FileCopyList[copycount];
                int copycount2 = (copycount + 1);

                //double mbfilesize = (double)FCO.size / 1048576;
                double gbsize = (double)bytecounter / 1073741824;

                string sizestring = "";
                if (FCO.fileinfo.size > 1073741824) //Larger than 1GB.
                {
                    double currentgbsize = (double)FCO.fileinfo.size / 1073741824;
                    sizestring = " (Current File Size " + Math.Round(currentgbsize, 2).ToString() + "GB)";
                }
                else if (FCO.fileinfo.size > 1048576) //larger than 1MB
                {
                    double currentmbsize = (double)FCO.fileinfo.size / 1048576;
                    sizestring = " (Current File Size " + Math.Round(currentmbsize, 0).ToString() + "MB)";
                }

                SetStatus("Installing: " + Identity.Name + Environment.NewLine +
                    "Copying File:" + copycount2 + " / " + FileCopyList.Count + sizestring + Environment.NewLine +
                    "Copied (GB): " + Math.Round(gbsize, 2) + " / " + Math.Round(totalgbytes, 2));
                try
                {
                    if (FileServer.protocol == "web")
                    {
                        //Web mode.
                        //MessageBox.Show("http://lanstallerfiles.mrhsystems.com/Games%20Source/" + FCO.fileinfo.source);
                        WC.DownloadFile(FileServer.path + FCO.fileinfo.source, FCO.destination);
                    }
                    else if (FileServer.protocol == "smb")
                    {
                        //SMB mode.
                        Pri.LongPath.File.Copy(FileServer.path + "\\" + FCO.fileinfo.source, FCO.destination, true);
                        //System.IO.File.Copy(ServerAddress + "\\" + FCO.fileinfo.source, FCO.destination, true);
                    }
                    copycount++;
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    Console.WriteLine(ex.ToString());
                    SetStatus("File copy stopped - thread Abort Exception");
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failure to copy file:" + FCO.fileinfo.source + Environment.NewLine + "TO:" + FCO.destination + Environment.NewLine + "Error:" + ex.ToString());
                    SetStatus("Error:" + ex.ToString());
                    return; //Exit - terminate.
                }


                bytecounter += FCO.fileinfo.size;
                SetProgress(bytecounter);

            }

        }

        public static void DownloadFile(string Source, string Destination)
        {
            //File download function for Tools.
            WC.DownloadFile(Source, Destination);
        }



        public static void GenerateCompatiblity(string filename, int compat_type)
        {
            //compat_types:
            if (compat_type == 1)
            {
                //1 = Run as admin.
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true).SetValue(ReplaceVariable(filename), "~ RUNASADMIN");
            }

        }


        void GenerateRegistry(List<RegistryOperation> RegistryList, List<SerialNumber> SerialList)
        {
            foreach (RegistryOperation REGOP in RegistryList)
            {
                RegistryKey HKEY = null;

                if (REGOP.hkey == 1)
                {
                    Registry.LocalMachine.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.LocalMachine.OpenSubKey(REGOP.subkey, true);
                }
                else if (REGOP.hkey == 2)
                {
                    Registry.CurrentUser.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.CurrentUser.OpenSubKey(REGOP.subkey, true);
                }
                else if (REGOP.hkey == 3)
                {
                    Registry.Users.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.Users.OpenSubKey(REGOP.subkey, true);
                }
                if ((RegistryValueKind)REGOP.regtype == RegistryValueKind.String || (RegistryValueKind)REGOP.regtype == RegistryValueKind.ExpandString)
                {
                    //Update data with variable for string.
                    REGOP.data = ReplaceVariable(REGOP.data);

                    //Update data with serials.
                    REGOP.data = ReplaceSerial(REGOP.data);
                }
                HKEY.SetValue(REGOP.value, REGOP.data, (RegistryValueKind)REGOP.regtype);
            }
        }

        void GenerateShortcuts(List<ShortcutOperation>ShortcutList)
        {
            foreach (ShortcutOperation SCO in ShortcutList)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(ReplaceVariable(SCO.location) + "\\" + SCO.name + ".lnk");

                shortcut.TargetPath = ReplaceVariable(SCO.filepath);
                shortcut.WorkingDirectory = ReplaceVariable(SCO.runpath);
                shortcut.Arguments = ReplaceVariable(SCO.arguments);
                shortcut.IconLocation = ReplaceVariable(SCO.icon);

                shortcut.Save();
            }


        }

        public long GetInstallSize()
        {
            return GetInstallSize(Identity.id);
        }

        public static long GetInstallSize(int SoftwareID)
        {
            string QueryString = "SELECT SUM(filesize) FROM tblFiles where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", SoftwareID);
            long filesize = (long)SQLCmd.ExecuteScalar();
            SQLConn.Close();
            return filesize;
        }


        public static string ReplaceVariable(string dataline)
        {
            //VARIABLES for FilePath, Shortcut Path, Registry Data.

            //%INSTALLPATH% = Installation Directory.
            string updateline = dataline.Replace("%INSTALLPATH%", LanstallerSettings.InstallDirectory);

            //%USERPROFILE% = User profile directory.
            updateline = updateline.Replace("%USERPROFILE%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            //Resolution.
            updateline = updateline.Replace("%WIDTH%", LanstallerSettings.ScreenWidth.ToString());
            updateline = updateline.Replace("%HEIGHT%", LanstallerSettings.ScreenHeight.ToString());

            //Username
            updateline = updateline.Replace("%USERNAME%", LanstallerSettings.Username);

            return updateline;

        }


        string ReplaceSerial(string data)
        {
            //Serial Numbers.
            //Check SerialList for matching serial number.
            string newdata = data;
            //check if %SERIALx%
            Regex rx = new Regex("[%SERIAL]\\d[%]");

            if (rx.Matches(data).Count > 0)
            {
                int SInstance = int.Parse(rx.Match(data).Value.ToString().Substring(1, 1)); //Get Serial Instance Number.
                string replacestring = "%SERIAL" + SInstance.ToString() + "%";

                foreach (SerialNumber SN in SerialList)
                {
                    if (SN.instancenumber == SInstance)
                    {
                        newdata = newdata.Replace(replacestring, SN.serialnumber); //update serial number.
                    }
                }

            }
            return newdata;
        }


        public static void ReplacePreferenceFile(string filename, string target, string replace)
        {
            filename = ReplaceVariable(filename); //Adjust for any variables in filename.

            if (!Pri.LongPath.File.Exists(filename))
            {
                MessageBox.Show("Preference File Missing  - " + filename);
                return;
            }



            //Read existing file.
            StreamReader SR = new StreamReader(filename, true);
            Encoding EncodingType = SR.CurrentEncoding; //Record encoding type.
            string configfiledata = SR.ReadToEnd();
            SR.Close();


            if (EncodingType == Encoding.UTF8)
            {
                //If UTF-8 Check for Byte Order Mark.
                FileStream fs = new FileStream(filename, FileMode.Open); //Open File for read.
                byte[] bits = new byte[3];
                fs.Read(bits, 0, 3); //Read 3 bytes.
                fs.Close();
                // UTF8 byte order mark is: 0xEF,0xBB,0xBF
                if (bits[0] != 0xEF && bits[1] != 0xBB && bits[2] != 0xBF)
                {
                    //No Byte Order Mark.
                    EncodingType = new UTF8Encoding(false);
                }

            }



            //Process config file data.
            replace = ReplaceVariable(replace); //Update replacement variables with user preferences.
            configfiledata = configfiledata.Replace(target, replace); //Apply replacement to target in file data.

            //Move Existing File.
            Pri.LongPath.File.Move(filename, filename + ".bak");

            //Write New file.
            FileStream FS = new FileStream(filename, FileMode.Create);
            StreamWriter SW = new StreamWriter(FS, EncodingType);

            SW.Write(configfiledata); //Write new data.
            SW.Close();


            //Delete Backup of Existing File.
            Pri.LongPath.File.Delete(filename + ".bak");

        }




    }









}
