using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


using System.Windows.Forms;

using System.Text.RegularExpressions; //Regex
using System.Diagnostics;

using System.IO;

using LanstallerShared;

using static Lanstaller.Classes.APIClient;
using System.Threading;
using Lanstaller.Classes;
using System.Collections.Concurrent;

namespace Lanstaller
{
    public class ClientSoftwareClass //Extension of shared Software class with client / windows exclusive functions.
    {
        //List<Servers> ServerList = new List<Servers>();
        public SoftwareInfo SInfo;
        public Status statusInfo;

        //Maximum concurrent small file transfers.
        SemaphoreSlim semaphore = new SemaphoreSlim(12); //Must match DownloadTask Init 

        List<Task> smallDownloadtasks = new List<Task>();
        public static bool WANMode = false;

        public string InstallDir;
        public bool installfiles;
        public bool installregistry;
        public bool installshortcuts;
        public bool apply_windowssettings;
        public bool apply_preferences;
        public bool install_redist;
        bool error_occured = false;


        ConcurrentQueue<FileCopyOperation> VerifyCopyOperations;
        List<FileCopyOperation> FileCopyOperations;
        List<RegistryOperation> RegistryOperations;
        List<ShortcutOperation> ShortcutOperations;
        List<SerialNumber> SerialList;
        List<FirewallRule> FirewallRules;
        List<PreferenceOperation> PreferenceOperations;
        List<string> DirectoryList = new List<string>();



        APIClient InstallAPIClient;
        public ClientSoftwareClass(SoftwareInfo InstallInfo)
        {
            SerialList = new List<SerialNumber>(); //Serials are set before install process.

            InstallAPIClient = new APIClient();

            SInfo = InstallInfo;
            statusInfo = new Status(SInfo);
        }

        public void Install()
        {
            //Run Installation

            if (installregistry)
            {
                statusInfo.SetStage(1);

                //DB
                //RegistryList = GetRegistry(SInfo.id);

                //Web
                RegistryOperations = GetRegistryListFromAPI(SInfo.id);
                GenerateRegistry();
            }


            if (installfiles)
            {
                CopyFiles();
            }


            ShortcutOperations = GetShortcutListFromAPI(SInfo.id);
            foreach (ShortcutOperation SCO in ShortcutOperations)
            {
                SCO.location = ReplaceVariable(SCO.location);
                SCO.filepath = ReplaceVariable(SCO.filepath);
                SCO.runpath = ReplaceVariable(SCO.runpath);
                SCO.arguments = ReplaceVariable(SCO.arguments);
                SCO.icon = ReplaceVariable(SCO.icon);
            }
            if (installshortcuts)
            {
                statusInfo.SetStage(5);
                //DB
                //SCO = GetShortcuts(SInfo.id);

                //WEB  
                GenerateShortcuts();
            }

            //Windows Settings (Firewall rules, Compatibility)
            if (apply_windowssettings)
            {
                statusInfo.SetStage(6);
                //DB
                //FWL = GetFirewallRules(SInfo.id);

                //WEB
                FirewallRules = GetFirewallRulesListFromAPI(SInfo.id);
                GenerateFirewallRules();

                //Compatibility.
                List<CompatabilityMode> CPL = GetCompatibilitiesFromAPI(SInfo.id);
                foreach (CompatabilityMode CP in CPL)
                {
                    GenerateCompatibility(CP.filename, CP.compat_type);
                }

            }


            if (apply_preferences)
            {
                statusInfo.SetStage(7);
                //DB
                //POList = GetPreferenceFiles(SInfo.id);

                //WEB
                PreferenceOperations = GetPreferencesListFromAPI(SInfo.id);

                GeneratePreferenceFiles();
            }

            if (install_redist)
            {
                statusInfo.SetStage(8);
                GenerateRedistributables(SInfo.id);
            }

            statusInfo.SetStage(9);

        }




        void GeneratePreferenceFiles()
        {
            foreach (PreferenceOperation PO in PreferenceOperations)
            {
                ReplacePreferenceFile(PO.filename, PO.target, PO.replace);
            }
        }



        void GenerateFirewallRules()
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

            foreach (FirewallRule fwr in FirewallRules)
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
                foreach (FirewallRule EFW in existingFWList)
                {
                    if (EFW.enabled && EFW.action && EFW.direction) //skip disabled, blocked and outbound rules.
                    {
                        if (EFW.exepath.ToLower().Equals(procpath.ToLower())) //Process path matched.
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
                    }
                    else if (fwr.protocol_value == 17) //IP Protocol value for UDP
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



        public void GenerateRedistributables(int sid) //Incomplete.
        {

            foreach (Redistributable Redist in GetRedistributablesListFromAPI(sid))
            {
                if (Redist.filecheck != "")
                {
                    if (System.IO.File.Exists(Redist.filecheck))
                    {
                        //File check true - skip installation.
                        continue;
                    }
                }

                string temppath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Temp";
                string filename = Redist.path.Substring(Redist.path.Replace("\\", "/").LastIndexOf("/"));
                string destpath = temppath + filename;


                try
                {
                    TransferFile(GetFileServerFromAPI()[0], Redist.path, destpath);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Failed to download redistributable. " + ex.Message);
                    return;
                }

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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to extract redistributable: " + ex.ToString());
                        return;
                    }

                    try
                    {
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

        public bool GenerateSerials() //Updates SerialList with user input.
        {
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
                                SF.txtSerial.Text = SN.UnformatSerial(RK.GetValue(SN.regVal, "").ToString());
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

                SN.serialnumber = SN.FormatSerial(SF.txtSerial.Text);

                //Check if serial has format request, check that length matches.
                if (SN.GetFormattedLength() > 0 && SF.txtSerial.Text.Length != SN.GetFormattedLength())
                {
                    MessageBox.Show("Error - Serial does not match required length.");
                    return false;
                }

                if (SF.aborted == true)
                {
                    return false; //Cancelled.
                }

            }
            return true;
        }


        void VerifyFilesThread(int FileCount)
        {
            try
            {

            
            int CheckCounter = 0;
            FileCopyOperation FVO;
            while (CheckCounter < FileCount) //When all files queued for verification and hashed.
            {
                if (VerifyCopyOperations.Count == 0)
                {
                    if (frmLanstaller.shutdownToken.IsCancellationRequested)
                    {
                        //Exit if main threads have closed.
                        return;
                    }
                    continue;
                }
                else
                {
                    //FVO = VerifyCopyOperations.Dequeue();
                    while (!VerifyCopyOperations.TryDequeue(out FVO))
                    {
                        Thread.Sleep(1); //Delay if unable to dequeue.
                    }

                }
                CheckCounter++;
                if (FVO.verified)
                {
                    continue; //skip already verified.
                }

                //Console.WriteLine("Checking hash for: " + FVO.destination);
                if (File.Exists(FVO.destination))
                {
                    //Possible optimisation
                    //Where some files are missing, split verification of existing files and missing files into threads.


                    //MessageBox.Show("MD5: " + FVO.destination);
                    string check_hash = FileInfoClass.CalculateMD5(FVO.destination);
                    if (FVO.fileinfo.hash.Equals(check_hash))
                    {
                        FVO.verified = true;
                        continue;
                    }
                    Logging.LogToFile("Deleting file - failed validation: " + FVO.destination + " Expected hash: " + FVO.fileinfo.hash + " Check result: " + check_hash);
                    File.Delete(FVO.destination); // Delete file if partially downloaded.
                }
                else
                {
                    Logging.LogToFile("Verification error - missing file: " + FVO.destination);
                }
            }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Verification thread crashed - error message: " + ex.Message);
            }
        }

       
        //Copy files from list provided, directory list for folder generation (Including empty dirs)
        void CopyFiles()
        {
            statusInfo.SetStage(2.1);
            //Get File List from DB Directly.
            //FCL = GetFiles(SInfo.id);

            //Get File List from Web Api
            FileCopyOperations = GetFilesListFromAPI(SInfo.id);
            VerifyCopyOperations = new ConcurrentQueue<FileCopyOperation>();


            //Get Directories, split up all sub directory paths and add to generation list.
            statusInfo.SetStage(2.2);
            foreach (string Dir in GetDirectoriesFromAPI(SInfo.id))
            {
                DirectoryList.Add(ReplaceVariable(Dir));
            }

            //Update destination paths.
            statusInfo.SetStage(2.3);
            foreach (FileCopyOperation FCO in FileCopyOperations)
            {
                FCO.destination = ReplaceVariable(FCO.destination);
                //string filedirectory = FCO.destination.Substring(0, FCO.destination.LastIndexOf("\\"));
                //GetSubFolders(filedirectory);
            }
            

            //Generate any required Directories for Files.
            statusInfo.SetStage(3.1);
            foreach (string dir in DirectoryList)
            {
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
            }
            statusInfo.SetStage(3.2);
            FileServer FileServer = GetFileServerFromAPI()[0];

            statusInfo.SetStage(3.3);

            bool FilesNotValidated = true; //File copy validation - loop on hash failure.
            int FailLoopCount = 0;
            while (FilesNotValidated)
            {
                if (frmLanstaller.shutdownToken.IsCancellationRequested)
                {
                    return;
                }

                int CopyIndex = 0;
                statusInfo.ResetCopyState();

                VerifyCopyOperations = new ConcurrentQueue<FileCopyOperation>();

                Thread VerifyCopyThread = new Thread(() => VerifyFilesThread(FileCopyOperations.Count));
                VerifyCopyThread.Name = "Verify Copy Thread";
                VerifyCopyThread.Start();

                while (CopyIndex < FileCopyOperations.Count)
                {
                    if (frmLanstaller.shutdownToken.IsCancellationRequested)
                    {
                        return;
                    }

                    FileCopyOperation FCO = FileCopyOperations[CopyIndex];

                    //Verify existing files.
                    if (File.Exists(FCO.destination)) //Check directory first, prevent directory exception on file exists.
                    {
                        if (string.IsNullOrEmpty(FCO.fileinfo.hash))
                        {
                            if (MessageBox.Show("Error - file hash not available from server, continue anyway?", "Hash Error", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        else
                        {
                            //Compare server hash value to local, also skip if verified previously.
                            if (FCO.verified || FCO.fileinfo.hash.Equals(FileInfoClass.CalculateMD5(FCO.destination)))
                            {
                                FCO.verified = true;
                                VerifyCopyOperations.Enqueue(FCO);

                                statusInfo.SetCopyState(CopyIndex, FCO.fileinfo.size);
                                CopyIndex++; //increment file counter.
                                continue; //Skip file copy, go to next.
                            }
                        }
                    }

                    //Copy File.
                    try
                    {
                        TransferFile(FileServer, CopyIndex);
                        CopyIndex++;
                    }
                    catch (ThreadAbortException ex)
                    {
                        statusInfo.SetError("File copy stopped - thread Abort Exception: " + ex.Message);
                        continue;
                    }
                    catch (Exception ex)
                    {
                        statusInfo.SetError("File copy failed - thread Abort Exception: " + ex.Message);
                        //MessageBox.Show("Failure to copy file:" + FCO.fileinfo.source + Environment.NewLine + "Destination:" + FCO.destination + Environment.NewLine + "Error:" + ex.ToString());
                        //SetStatus("Download error:" + ex.ToString());
                        Thread.Sleep(5);
                        continue;
                    }
                }

                //Complete validation of downloaded files.
                //restart previous loop if failures exist.

                Task.WhenAll(smallDownloadtasks).Wait();

                statusInfo.SetStage(4);

                VerifyCopyThread.Join();
                FilesNotValidated = false;

                FailLoopCount++;
                foreach (FileCopyOperation FCO2 in FileCopyOperations)
                {
                    if (FCO2.verified == false) //Validation Failure
                    {
                        FilesNotValidated = true;
                        break;
                    }
                }

                if (FailLoopCount > 1)
                {
                    statusInfo.SetError(
                        "Some files failed to download.\nRetrying in " +
                        (FailLoopCount * 1).ToString() + " seconds.");

                    Thread.Sleep((FailLoopCount * 1 * 1000));
                }
            }
        }


        //Public method for transfering file - used by redist.
        public static async void TransferFile(FileServer FileServer, string Source, string Destination)
        {
            if (FileServer.protocol == 1)
            {
                DownloadTask DT = new DownloadTask(FileServer.path + Source, Destination);
                Task Dtask = DT.DownloadAsync();
                Dtask.Wait(); //If stuck here - async issue occurs when running from main thread, use task with await.
            }
            else if (FileServer.protocol == 2) //SMB
            {
                string src = Uri.UnescapeDataString(Source).Replace("/", "\\");
                File.Copy(FileServer.path + src, Destination, true);
            }
        }

        //Used for install file copy with local FileCopyOperations list.
        async void TransferFile(FileServer FileServer, int FileCopyIndex)
        {
            FileCopyOperation FCO = FileCopyOperations[FileCopyIndex];
            
            if (FileServer.protocol == 1) //Web
            {
                if (WANMode || FCO.fileinfo.size < 524288) //Less than 512KB or WAN mode, run concurrent downloads.
                {
                    //Multi Thread smaller files to avoid overhead blocking file transfer.
                    smallDownloadtasks.Add(Task.Run(async () =>
                    {
                        await semaphore.WaitAsync();

                        DownloadTask DT = new DownloadTask(FileServer.path + FCO.fileinfo.source, FCO.destination, FCO.fileinfo.size);
                        Task Dtask = DT.DownloadAsync();
                        Dtask.Wait();
                        statusInfo.SetCopyState(FileCopyIndex, FCO.fileinfo.size);
                        VerifyCopyOperations.Enqueue(FCO);

                        semaphore.Release();
                    }));
                }
                else
                {
                    Task.WhenAll(smallDownloadtasks).Wait(); //Finish all smaller downloads.

                    DownloadTask DT = new DownloadTask(FileServer.path + FCO.fileinfo.source, FCO.destination, FCO.fileinfo.size);
                    Task Dtask = DT.DownloadAsync();
                    while (!Dtask.IsCompleted)
                    {
                        statusInfo.SetPartialState(FileCopyIndex, DT.downloadedbytes);                       
                    }
                    Dtask.Wait();
                    statusInfo.SetCopyState(FileCopyIndex, FCO.fileinfo.size);
                    VerifyCopyOperations.Enqueue(FCO);

                }
            }
            else if (FileServer.protocol == 2) //SMB
            {
                File.Copy(FileServer.path + Uri.UnescapeDataString(FCO.fileinfo.source), FCO.destination, true);
                statusInfo.SetCopyState(FileCopyIndex, FCO.fileinfo.size);
                VerifyCopyOperations.Enqueue(FCO);
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


        void GenerateRegistry()
        {
            foreach (RegistryOperation REGOP in RegistryOperations)
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

                //Apply variables to registry Name.
                if (REGOP.valueName != null)
                {
                    REGOP.valueName = ReplaceVariable(REGOP.valueName);
                }

                if ((RegistryValueKind)REGOP.regtype == RegistryValueKind.String || (RegistryValueKind)REGOP.regtype == RegistryValueKind.ExpandString)
                {
                    //Update data with variable for string.
                    REGOP.data = ReplaceVariable(REGOP.data);

                    //Update data with serials.
                    REGOP.data = ReplaceSerial(REGOP.data);
                }

                //Set registry value.

                try
                {
                    HKEY.SetValue(REGOP.valueName, REGOP.data, (RegistryValueKind)REGOP.regtype);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting registry key " + REGOP.subkey + ":" + REGOP.valueName + "\n" + ex.ToString());

                }
            }
        }

        void GenerateShortcuts()
        {
            foreach (ShortcutOperation SCO in ShortcutOperations)
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(SCO.location + "\\" + SCO.name + ".lnk");
                shortcut.TargetPath = SCO.filepath;
                shortcut.WorkingDirectory = SCO.runpath;
                shortcut.Arguments = SCO.arguments;
                if (!String.IsNullOrEmpty(SCO.icon))
                {
                    shortcut.IconLocation = SCO.icon;
                }
                shortcut.Save();
            }
        }

        public List<ShortcutOperation> GetShortcutOperations()
        {
            return ShortcutOperations;
        }


        public static string ReplaceVariable(string dataline)
        {
            //VARIABLES for FilePath, Shortcut Path, Registry Data.

            //%INSTALLPATH% = Installation Directory.
            string updateline = dataline.Replace("%INSTALLPATH%", UserSettings.InstallDirectory);

            //%USERPROFILE% = User profile directory.
            updateline = updateline.Replace("%USERPROFILE%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            //Resolution.
            updateline = updateline.Replace("%WIDTH%", UserSettings.ScreenWidth.ToString());
            updateline = updateline.Replace("%HEIGHT%", UserSettings.ScreenHeight.ToString());

            //Username
            updateline = updateline.Replace("%USERNAME%", UserSettings.Username);

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

            if (!File.Exists(filename))
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
            File.Move(filename, filename + ".bak");

            //Write New file.
            FileStream FS = new FileStream(filename, FileMode.Create);
            StreamWriter SW = new StreamWriter(FS, EncodingType);

            SW.Write(configfiledata); //Write new data.
            SW.Close();

            //Delete Backup of Existing File.
            File.Delete(filename + ".bak");

        }

        public bool GetErrored()
        {
            return error_occured;
        }

        public void AddSerial(SerialNumber Serial)
        {
            SerialList.Add(Serial);
        }



    }









}
