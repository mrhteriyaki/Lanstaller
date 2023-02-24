using System;
using System.Collections.Generic;
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
                List<string> DirectoryList = new List<string>();

                //DB
                //FCL = GetFiles(Identity.id);

                //Web
                FCL = GetFilesListFromAPI(Identity.id);
                
                //Get Directories, split up all sub directory paths and add to generation list.
                foreach (string Dir in GetDirectoriesFromAPI(Identity.id))
                {
                    Console.WriteLine(Dir);
                    GetSubFolders(ReplaceVariable(Dir), DirectoryList);
                }

                foreach (FileCopyOperation FCO in FCL)
                {
                    FCO.destination = ReplaceVariable(FCO.destination);
                    //Remove filename from path.
                    string filedirectory = FCO.destination.Substring(0, FCO.destination.LastIndexOf("\\"));
                    GetSubFolders(filedirectory, DirectoryList);
                }

                InstalledSize = 0; //reset install size.
                InstallSize = GetInstallSizeFromAPI(Identity.id);

                SetStatus("Copying Files - " + Identity.Name);
                CopyFiles(FCL, DirectoryList);
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

            SetStatus("Updating Windows Settings (Firewall, Compatibility) - " + Identity.Name);
            //Windows Settings (Firewall rules, Compatibility)
            if (apply_windowssettings)
            {
                //Firewall
                List<FirewallRule> FWL;
                
                //DB
                //FWL = GetFirewallRules(Identity.id);

                //WEB
                FWL = GetFirewallRulesListFromAPI(Identity.id);
                GenerateFirewallRules(FWL);

                //Compatibility.
                List<Compatibility> CPL = GetCompatibilitiesFromAPI(Identity.id);
                foreach(Compatibility CP in CPL)
                {
                    GenerateCompatibility(CP.filename,CP.compat_type);
                }
                
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

        //Gets all sub folders for given directory, adds to list.
        void GetSubFolders(string directory, List<string> DirectoryList)
        {
            //Check if already in list, if so skip.
            foreach (string dir in DirectoryList)
            {
                if (dir == directory)
                {
                    return;
                }
            }

            //Split up file directory for parent folders.
            int count = 0;
            string directorypath = "";
            foreach (string FileDirSection in directory.Split(Char.Parse("\\")))
            {
                //Check each directory including parents against list and add if missing.
                directorypath = directorypath + FileDirSection + "\\";

                count++;
                if (count == 1)
                {
                    //Skip Drive Root (Eg: C:).
                    continue;
                }

                //Check if path already added to list.
                bool missing = true;
                foreach (string existingdir in DirectoryList)
                {
                    if (existingdir.Equals(directorypath))
                    {
                        missing = false;
                        break;
                    }
                }
                if (missing == true)
                {
                    DirectoryList.Add(directorypath);
                }
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

       

        public void GenerateRedistributables(List<Redistributable> RedistributableList) //Incomplete.
        {
            foreach (Redistributable Redist in RedistributableList)
            {
                if (Redist.filecheck != "")
                {
                    if (System.IO.File.Exists(Redist.filecheck))
                    {
                        //File check true - skip installation.
                        continue;
                    }
                }


                string temppath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp" ;
                
                string filename = Redist.path.Substring(Redist.path.Replace("\\", "/").LastIndexOf("/"));
                string destpath = temppath + filename;



                DownloadFile(Redist.path, destpath);

                Process RDProc = new Process();

                if (Redist.compressed == "0" || Redist.compressed == "") //Direct file, run.
                {
                    RDProc.StartInfo.FileName = destpath;
                    RDProc.StartInfo.Arguments = Redist.args;
                   
                }
                else if (Redist.compressed == "1")
                {
                    string extractpath = temppath + "\\" + Guid.NewGuid().ToString() + "\\";
                    Directory.CreateDirectory(extractpath);

                    //Extract.
                    Process SevenZipProc = new Process();                   
                    SevenZipProc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "7z.exe";
                    SevenZipProc.StartInfo.Arguments = "x \"" + destpath + "\" -o\"" + extractpath + "\"";
                    SevenZipProc.StartInfo.UseShellExecute = false;
                    SevenZipProc.Start();
                    SevenZipProc.WaitForExit();

                    //Run
                    RDProc.StartInfo.FileName = extractpath + Redist.compressed_path;
                    RDProc.StartInfo.Arguments = Redist.args;
                }
                else
                {
                    MessageBox.Show("Redist Installation Error - check configuration.");
                    return;
                }

                RDProc.Start();
                RDProc.WaitForExit();

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

                        if (SN.regKey.StartsWith("HKEY_LOCAL_MACHINE"))
                        { 
                            RegistryKey RK = Registry.LocalMachine.OpenSubKey(SN.regKey.Substring(19));
                            if (RK != null)
                            {
                                SF.txtSerial.Text = RK.GetValue(SN.regVal, "").ToString();
                            }
                        }

                       
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

        

        void CopyFiles(List<FileCopyOperation> FileCopyList, List<string> DirectoryList)
        {

            //Calculate total copy size.
            long totalbytes = GetInstallSizeFromAPI(Identity.id);
            double totalgbytes = (double)totalbytes / 1073741824;


            SetStatus("Status: Installing " + Identity.Name + Environment.NewLine + " - Generating Directories");
            //Generate any required Directories for Files.
            foreach (string dir in DirectoryList)
            {
                if (Pri.LongPath.Directory.Exists(dir) == true)
                {
                    //Prompt to remove any existing game folder.
                    if (dir.StartsWith(LanstallerSettings.InstallDirectory + "\\") && dir != LanstallerSettings.InstallDirectory + "\\")
                    {
                        if (MessageBox.Show("Delete existing folder?\n" + dir, "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            while(Pri.LongPath.Directory.Exists(dir) == true)
                            {
                                Pri.LongPath.Directory.Delete(dir, true);
                                if (Pri.LongPath.Directory.Exists(dir))
                                {
                                    MessageBox.Show("Removal failed, try again?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                }
                            }
                            
                        }
                    }
                }

                if (Pri.LongPath.Directory.Exists(dir) == false)
                {
                    Pri.LongPath.Directory.CreateDirectory(dir);
                }
            }

            Server FileServer = GetFileServerFromAPI();
            
            //Trim / from url (getfiles will prepend / to source on copy operation).
            if (FileServer.path.EndsWith("/"))
            {
                FileServer.path = FileServer.path.Substring(0, FileServer.path.Length - 1);
            }

            //Copy Files.
            int copycount = 0;
            long bytecounter = 0;
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
                        DownloadFile(FileServer.path + FCO.fileinfo.source, FCO.destination);
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
    



        public static void GenerateCompatibility(string filename, int compat_type)
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
                if (!String.IsNullOrEmpty(SCO.icon))
                {
                    shortcut.IconLocation = ReplaceVariable(SCO.icon);
                }
                shortcut.Save();
            }


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
