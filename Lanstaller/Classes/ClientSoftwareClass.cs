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
using System.Drawing;
using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualBasic;

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

        public string InstallDir;
        public bool installfiles;
        public bool installregistry;
        public bool installshortcuts;
        public bool apply_windowssettings;
        public bool apply_preferences;
        public bool install_redist;


        public void Install()
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
                InstallSize = Identity.install_size;

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

            SetStatus(Identity.Name + Environment.NewLine + "Updating Windows Settings" + Environment.NewLine + "(Firewall Rules and Compatibility)");
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
                status =  message.Replace("&", "&&");

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
            //Add Firewall rules to Windows.
            string netsh_path = "c:\\windows\\system32\\netsh.exe";
            if (!System.IO.File.Exists(netsh_path))
            {
                MessageBox.Show("netsh.exe missing - unable to add firewall rules"); //just incase.
                return;
            }

            Process FWNetSHProc = new Process();
            FWNetSHProc.StartInfo.FileName = netsh_path;
            FWNetSHProc.StartInfo.UseShellExecute = false;
            FWNetSHProc.StartInfo.RedirectStandardOutput = true;
            FWNetSHProc.StartInfo.CreateNoWindow = true;

            foreach (FirewallRule fwr in FirewallRuleList)
            {
                string rulename = fwr.rulename;
                if (string.IsNullOrEmpty(rulename)) rulename = fwr.softwarename;

                //Check if rule exists.
                FWNetSHProc.StartInfo.Arguments = "advfirewall firewall show rule name=\"" + rulename + "\" verbose";
                FWNetSHProc.Start();
                
                //Get existing firewall rules.
                List<FirewallRule> existingFWList = new List<FirewallRule>();
                while (!FWNetSHProc.StandardOutput.EndOfStream)
                {
                    string line = FWNetSHProc.StandardOutput.ReadLine();
                    if (line.StartsWith("Enabled:"))
                    {
                        existingFWList.Add(new FirewallRule());
                        existingFWList[existingFWList.Count - 1].enabled = false;
                        if (line.EndsWith("Yes"))
                        {
                            existingFWList[existingFWList.Count - 1].enabled = true;
                        }
                    }
                    else if (line.StartsWith("Program:"))
                    {
                        string program_path = line.Substring(8).Trim();
                        existingFWList[existingFWList.Count - 1].exepath = program_path;
                    }
                    else if (line.StartsWith("Direction:"))
                    {
                        existingFWList[existingFWList.Count - 1].direction = false;
                        if (line.EndsWith("In"))
                        {
                            existingFWList[existingFWList.Count - 1].direction = true;
                        }
                    }
                    else if (line.StartsWith("LocalPort:"))
                    {
                        if (!line.EndsWith("Any"))
                        {
                            existingFWList[existingFWList.Count - 1].port_number = int.Parse(line.Substring(11).Trim());
                        }
                    }
                    else if (line.StartsWith("Protocol:"))
                    {
                        existingFWList[existingFWList.Count - 1].protocol_value = 0;
                        if (line.EndsWith("TCP"))
                        {
                            existingFWList[existingFWList.Count - 1].protocol_value = 6;
                        }
                        else if (line.EndsWith("UDP"))
                        {
                            existingFWList[existingFWList.Count - 1].protocol_value = 17;
                        }
                    }
                    else if (line.StartsWith("Action:"))
                    {
                        existingFWList[existingFWList.Count - 1].action = false;
                        if (line.EndsWith("Allow"))
                        {
                            existingFWList[existingFWList.Count - 1].action = true;
                        }
                    }
                }

                string procpath = ReplaceVariable(fwr.exepath);
                bool rule_required = true;
                //Check if any existing rule matches properties of requested.
                foreach(FirewallRule EFW in existingFWList)
                {
                    if (EFW.enabled && EFW.action && EFW.direction) //skip disabled, blocked and outbound rules.
                    {
                        if (procpath.ToLower().Equals(procpath.ToLower())) //Process path matched.
                        {
                            if (EFW.protocol_value == fwr.protocol_value && EFW.port_number == fwr.port_number) //Port and IP Protocol matched.
                            {
                                rule_required = false;
                                break;
                            }
                        }
                    }
                }
                
                
                //Generates firewall rule using netsh, eg:
                //netsh advfirewall firewall add rule name="My Application" dir=in action=allow program="C:\games\The Call of Duty\CoDMP.exe" enable=yes
                if (rule_required)
                {
                    string protocol_str = String.Empty;
                    if (fwr.protocol_value == 6) //IP Protocol value for TCP
                    {
                        protocol_str = " protocol=tcp";
                    }else if (fwr.protocol_value == 17) //IP Protocol value for UDP
                    {
                        protocol_str = " protocol=udp";
                    }
                    string port_str = String.Empty;
                    if (fwr.port_number > 0)
                    {
                        port_str = " localport=" + fwr.port_number.ToString();
                    }

                    FWNetSHProc.StartInfo.Arguments = "advfirewall firewall add rule name=\"" + rulename + "\" dir=in action=allow" + protocol_str + port_str + " program=\"" + procpath + "\" enable=yes";
                    FWNetSHProc.Start();
                }
                
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

                if (!System.IO.File.Exists(destpath))
                {
                    MessageBox.Show("Missing file for redistributable installation:" + destpath + " Install will skip.");
                    return;
                }

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
                    try
                    {
                        SevenZipProc.Start();
                        SevenZipProc.WaitForExit();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Failed to extract redistributable: " + ex.ToString());
                        return;
                    }
                    
                    try
                    {
                        //Run
                        RDProc.StartInfo.FileName = extractpath + Redist.compressed_path;
                        RDProc.StartInfo.Arguments = Redist.args;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to run redistributable installer:" + ex.ToString());
                        return;
                    }
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
                SF.serialid = SN.serialid;

                if (!string.IsNullOrEmpty(SN.regKey))
                {
                    try
                    {

                        if (SN.regKey.StartsWith("HKEY_LOCAL_MACHINE"))
                        { 
                            RegistryKey RK = Registry.LocalMachine.OpenSubKey(SN.regKey.Substring(19));
                            if (RK != null)
                            {
                                string serialkey = SerialNumber.UnformatSerial(SN.regVal,SN.format);
                                SF.txtSerial.Text = RK.GetValue(SN.regVal, serialkey).ToString();
                            }
                        }

                       
                    }
                    catch (Exception ex)
                    {
                        //occurs on null exception due to missing key.
                        MessageBox.Show("Error in GenerateSerials() : " + ex.Message);
                    }

                }
                SF.ShowDialog();
                SN.serialnumber = SerialNumber.FormatSerial(SF.txtSerial.Text,SN.format);
            }
        }

        

        //Copy files from list provided, directory list for folder generation (Including empty dirs)
        void CopyFiles(List<FileCopyOperation> FileCopyList, List<string> DirectoryList)
        {

            //Calculate total copy size.
            long totalbytes = Identity.install_size;
            double totalgbytes = (double)totalbytes / 1073741824;


            SetStatus("Status: Installing " + Identity.Name + Environment.NewLine + " - Generating Directories");
            //Generate any required Directories for Files.
            foreach (string dir in DirectoryList)
            {
                /*
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
                */
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
                //Info for status update.
                FileCopyOperation FCO = FileCopyList[copycount];
                int copycount2 = (copycount + 1);
                string sizestring = "";
                if (FCO.fileinfo.size > 1073741824) //Larger than 1GB.
                {
                    double currentgbsize = (double)FCO.fileinfo.size / 1073741824;
                    sizestring = "Current File Size: " + Math.Round(currentgbsize, 2).ToString() + "GB";
                }
                else if (FCO.fileinfo.size > 1048576) //larger than 1MB
                {
                    double currentmbsize = (double)FCO.fileinfo.size / 1048576;
                    sizestring = "Current File Size: " + Math.Round(currentmbsize, 0).ToString() + "MB";
                }

                //Calculate Gigabyte count of transfered files + current progress.
                //double mbfilesize = (double)FCO.size / 1048576;
                double gbsize = (double)bytecounter / 1073741824;
                

                //Check file exists, verify hash.
                if (System.IO.File.Exists(FCO.destination))
                {
                    if (string.IsNullOrEmpty(FCO.fileinfo.hash))
                    {
                        MessageBox.Show("Error - file hash not available from server - alert Lanlord.");
                    }
                    SetStatus("Installing: \n" + Identity.Name + 
                            "\nVerifying File:" + copycount2 + " / " + FileCopyList.Count + 
                            "\nProgress (GB): " + Math.Round(gbsize, 2) + " / " + Math.Round(totalgbytes, 2) +
                            "\n" + sizestring);

                    string check_hash = CalculateMD5(FCO.destination);
                    //MessageBox.Show(check_hash);
                    //MessageBox.Show("CH:" + check_hash + "\nFH:" + FCO.fileinfo.hash);
                   
                    if (!String.IsNullOrEmpty(FCO.fileinfo.hash)) //Check hash value has been scanned into server.
                    {
                        if (FCO.fileinfo.hash.Equals(check_hash)) //Compare server hash value to local.
                        {
                            //File exists and matches correct hash.
                            bytecounter += FCO.fileinfo.size;
                            SetProgress(bytecounter);
                            copycount++; //increment file counter.
                            continue; //Skip file copy, go to next.
                        }
                    }
                }
               

                //Copy File.
                try
                {
                    if (FileServer.protocol == "web")
                    {
                        //Web mode.

                        //OLD Download.
                        //DownloadFile(FileServer.path + FCO.fileinfo.source, FCO.destination);

                        //Download File - ASYNC
                        DownloadWithProgress DLP = new DownloadWithProgress(FileServer.path + FCO.fileinfo.source, FCO.destination);
                        DLP.Download();
                        //Need to test retry / verification process.


                        while(!DLP.completed) //Do until download is completed.
                        {
                            gbsize = ((double)bytecounter + DLP.downloadedbytes) / 1073741824;
                            SetProgress(bytecounter + DLP.downloadedbytes);

                            SetStatus("Installing: \n" + Identity.Name + 
                            "\nCopying File:" + copycount2 + " / " + FileCopyList.Count +
                            "\nProgress (GB): " + Math.Round(gbsize, 2) + " / " + Math.Round(totalgbytes, 2) +
                            "\n" + sizestring);
                        }

                       
                    }
                    else if (FileServer.protocol == "smb")
                    {
                        //SMB mode.
                        Pri.LongPath.File.Copy(FileServer.path + "\\" + FCO.fileinfo.source, FCO.destination, true);
                        //System.IO.File.Copy(ServerAddress + "\\" + FCO.fileinfo.source, FCO.destination, true);
                    }

                    //Update copy index once complete, failure will cause copy retry.
                    copycount++;
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    //MessageBox.Show(ex.ToString());
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
                try
                {
                    HKEY.SetValue(REGOP.value, REGOP.data, (RegistryValueKind)REGOP.regtype);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting registry key " + REGOP.subkey + ":" + REGOP.value + "\n" + ex.ToString());
                    
                }
                
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
