using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

using IWshRuntimeLibrary;
using System.Windows.Forms;

using System.Text.RegularExpressions; //Regex
using System.Diagnostics;

using System.IO;

using Lanstaller_Shared;


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




        public static void Install(SoftwareClass SWI, bool installfiles, bool installregistry, bool installshortcuts, bool apply_windowssettings, bool apply_preferences, bool install_redist)
        {
            string ServerAddress = GetServers();
            
            //Run Installation

            if (installregistry)
            {
                SetStatus("Applying Registry - " + SWI.Name);
                GetRegistry(SWI.id);
                GenerateRegistry();
            }


            if (installfiles)
            {
                SetStatus("Indexing - " + SWI.Name);
                GetFiles(SWI.id, ServerAddress);

                InstalledSize = 0; //reset install size.
                InstallSize = GetInstallSize(SWI.id);

                SetStatus("Copying Files - " + SWI.Name);
                CopyFiles(SWI.id);
            }

            if (installshortcuts)
            {
                SetStatus("Generating Shortcuts - " + SWI.Name);
                GetShortcuts(SWI.id);
                GenerateShortcuts();
            }

            SetStatus("Adding firewall rules - " + SWI.Name);
            //firewall rules.
            if (apply_windowssettings)
            {
                GetFirewallRules(SWI.id);
                GenerateFirewallRules();

                GenerateCompatibility(SWI.id);
            }

            SetStatus("Applying Preferences - " + SWI.Name);
            if (apply_preferences)
            {
                GetPreferenceFiles(SWI.id);
                GeneratePreferenceFiles();
            }

            SetStatus("Installing Redistributables - " + SWI.Name);
            if (install_redist)
            {
                GetRedistributables(SWI.id);
                GenerateRedistributables();
            }

            SetStatus("Install Complete:" + Environment.NewLine + SWI.Name);

        }


        static void SetStatus(string message)
        {
            lock (_statuslock)
            {
                status = "Status: " + message.Replace("&","&&");

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

       

        
        static void GeneratePreferenceFiles()
        {
            foreach(PreferenceOperation PO in PreferenceOperationList)
            {
                ReplacePreferenceFile(PO.filename, PO.target, PO.replace);
            }
        }



        static void GenerateFirewallRules()
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

        static void GenerateCompatibility(int softwareid)
        {
            //Set compatibility flags in registry.

            string QueryString = "select filename,compat_type from tblCompatibility where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();

            while (SQLOutput.Read())
            {
                GenerateCompatiblity(SQLOutput[0].ToString(), (int)SQLOutput[1]);
            }

            SQLConn.Close();

        }

        public static void GenerateRedistributables() //Incomplete.
        {
            foreach (Redistributable Redist in RedistributableList)
            {

            }

            //Check filecheck, if blank, check name against windows installations.
        }


        static void CopyFiles(int softwareid)
        {
            //Get Software Name.
            string softwarename = GetSoftwareName(softwareid);
            
            //Calculate total copy size.
            long totalbytes = GetInstallSize(softwareid);
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


            SetStatus("Installing: " + softwarename + Environment.NewLine + "Status: Generating Directories");

            foreach (string dir in DirectoryList)
            {
                if (Pri.LongPath.Directory.Exists(dir) == false)
                {
                    Pri.LongPath.Directory.CreateDirectory(dir);
                }

            }

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
                    sizestring = " (Current File Size " +  Math.Round(currentmbsize,0).ToString() + "MB)";
                }

                SetStatus("Installing: " + softwarename + Environment.NewLine + 
                    "Copying File:" + copycount2 + " / " + FileCopyList.Count + sizestring + Environment.NewLine +
                    "Copied (GB): " + Math.Round(gbsize,2) + " / " + Math.Round(totalgbytes,2));
                try
                {
                    Pri.LongPath.File.Copy(FCO.fileinfo.source, FCO.destination, true);
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


     

        public static void GenerateCompatiblity(string filename, int compat_type)
        {
            //compat_types:
            if (compat_type == 1)
            {
                //1 = Run as admin.
                Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true).SetValue(ReplaceVariable(filename), "~ RUNASADMIN");
            }

        }

        //Gets Serial Requirements for Queued Installs.
        public static void GetSerials(List<int> SoftwareIDList)
        {
            SerialList.Clear();
            foreach (int SoftwareID in SoftwareIDList)
            {


                string QueryString = "select [name],[instance],[regKey],[regVal] from [tblSerials] WHERE software_id = @softwareid";

                SqlConnection SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("softwareid", SoftwareID);
                SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
                while (SQLOutput.Read())
                {
                    SerialNumber tSerial = new SerialNumber();
                    tSerial.softwareid = SoftwareID;
                    tSerial.name = SQLOutput[0].ToString();
                    tSerial.instancenumber = (int)SQLOutput[1];
                    tSerial.regKey = SQLOutput[2].ToString();
                    tSerial.regVal = SQLOutput[3].ToString();
                    SerialList.Add(tSerial);
                }
                SQLConn.Close();
            }

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
                    }catch (Exception ex)
                    {
                        //occurs on null exception due to missing key.
                        Console.WriteLine(ex.Message);  
                    }
                    
                }
                SF.ShowDialog();

               

                SN.serialnumber = SF.txtSerial.Text;


            }

        }

       

        static void GetRegistry(int SoftwareID)
        {
            RegistryList.Clear();

            string QueryString = "select [hkey],[subkey],[value],[type],[data] from [tblRegistry] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                RegistryOperation tReg = new RegistryOperation();
                tReg.hkey = ((int)SQLOutput[0]); //Hive Key.
                tReg.subkey = SQLOutput[1].ToString(); //Sub Key.
                tReg.value = SQLOutput[2].ToString();
                tReg.regtype = ((int)SQLOutput[3]);
                tReg.data = ReplaceSerial(ReplaceVariable(SQLOutput[4].ToString()), SoftwareID); //Includes ReplaceSerial to check for Serial number.

                RegistryList.Add(tReg);
            }
            SQLConn.Close();
        }

        static void GenerateRegistry()
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
                HKEY.SetValue(REGOP.value, REGOP.data, (RegistryValueKind)REGOP.regtype);
            }
        }

        
               

        

        static void GenerateShortcuts()
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

        public static long GetInstallSize(int softwareid)
        {
            string QueryString = "SELECT SUM(filesize) FROM tblFiles where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
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


        static string ReplaceSerial(string data, int softwareid)
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

                    if (SN.softwareid == softwareid)
                    {
                        if (SN.instancenumber == SInstance)
                        {
                            newdata = newdata.Replace(replacestring, SN.serialnumber); //update serial number.
                        }

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
