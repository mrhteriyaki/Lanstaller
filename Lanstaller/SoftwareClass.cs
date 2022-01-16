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


namespace Lanstaller
{
    public class SoftwareClass
    {
        public static string ConnectionString = "Data Source=192.168.88.3,1433;Initial Catalog=lanstaller;user=lanstaller;password=LanJoekf192!";

        public int id;
        public string Name;
        static string status = "Status: Ready";

        static long InstallSize; //Size of current install.
        static long InstalledSize; //Progres of current install.

        //List<Servers> ServerList = new List<Servers>();
        static List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();
        static List<RegistryOperation> RegistryList = new List<RegistryOperation>();
        static List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();
        public static List<SerialNumber> SerialList = new List<SerialNumber>();


        //Locks.
        static readonly object _statuslock = new object();
        static readonly object _progresslock = new object();

        public static List<SoftwareClass> LoadSoftware()
        {
            List<SoftwareClass> tmpList = new List<SoftwareClass>();

            //Get List of Software from Server
            string QueryString = "SELECT [id],[name] from tblSoftware order by [name]";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SoftwareClass tmpSoftware = new SoftwareClass();
                tmpSoftware.id = (int)SQLOutput[0];
                tmpSoftware.Name = SQLOutput[1].ToString();
                tmpList.Add(tmpSoftware);
            }
            SQLConn.Close();

            return tmpList;

        }

        public static int AddSoftware(string softwarename)
        {
            string QueryString = "INSERT into tblSoftware ([name]) VALUES (@softname); SELECT SCOPE_IDENTITY();";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softname", softwarename);
            object output = SQLCmd.ExecuteScalar();
            int idval = int.Parse(output.ToString());
            SQLConn.Close();
            return idval;
        }

        public static void Install(SoftwareClass SWI, bool installfiles, bool installregistry, bool installshortcuts, bool applyfirewallrules, bool apply_preferences, bool install_redist)
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
                InstallSize = GetInstallSize(SWI.id);

                SetStatus("Copying Files - " + SWI.Name);
                GenerateFiles(SWI.Name);
            }

            if (installshortcuts)
            {
                SetStatus("Generating Shortcuts - " + SWI.Name);
                GetShortcuts(SWI.id);
                GenerateShortcuts();
            }

            SetStatus("Adding firewall rules - " + SWI.Name);
            //firewall rules.
            if (applyfirewallrules)
            {
                GenerateFirewallRules(SWI.id, SWI.Name);
            }

            SetStatus("Applying Preferences - " + SWI.Name);
            if (apply_preferences)
            {
                GeneratePreferenceFiles(SWI.id);
            }

            SetStatus("Installing Redistributables - " + SWI.Name);
            if (install_redist)
            {
                CheckRedistributables(SWI.id);
            }

            SetStatus("Install Complete - " + SWI.Name);

        }


        static void SetStatus(string message)
        {
            lock (_statuslock)
            {
                status = "Status: " + message;

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

        static string GetServers()
        {
            //ServerList.Clear();
            string QueryString = "SELECT TOP(1) [address] FROM [tblServers]";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            string servadd = SQLCmd.ExecuteScalar().ToString();

            SQLConn.Close();

            return servadd;

        }

        static void GetFiles(int softwareid, string ServerAddress)
        {
            FileCopyList.Clear();

            string QueryString = "select [filename],[source],[destination],[filesize] from tblFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                //Prepare full source and destination locations for transfer process.
                string Sfilename = SQLOutput[0].ToString();
                string Ssource = SQLOutput[1].ToString();
                string Sdestination = SQLOutput[2].ToString();
                long filesize = (long)SQLOutput[3];

                FileCopyOperation tFCO = new FileCopyOperation();
                tFCO.source = ServerAddress + "\\" + Ssource;
                tFCO.size = filesize;

                //Determine file destination.
                if (SQLOutput[2].ToString().Equals(""))
                {
                    //Empty Destination - Uses Install Directory.
                    tFCO.destination = LanstallerSettings.InstallDirectory + "\\" + Sfilename;
                }
                else
                {
                    //Use static path.
                    string destination = Sdestination + "\\" + Sfilename;
                    //Replace any variables in static destination path.
                    tFCO.destination = ReplaceVariable(destination);
                }

                //Add copy operation to list.
                FileCopyList.Add(tFCO);

            }
            SQLConn.Close();
        }

        static void GeneratePreferenceFiles(int softwareid)
        {
            string QueryString = "select [filepath],[target],[replace] from tblPreferenceFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                ReplacePreferenceFile(SQLOutput[0].ToString(), SQLOutput[1].ToString(), SQLOutput[2].ToString());
            }

            SQLConn.Close();
        }

        static void GenerateFirewallRules(int softwareid, string softwarename)
        {
            //Software name used for Windows Firewall rule.
            FileCopyList.Clear();

            string QueryString = "select [filepath] from tblFirewallExceptions WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                string firewallpath = ReplaceVariable(SQLOutput[0].ToString()).ToLower();
                AddFirewallRule(softwarename, firewallpath);
            }

            SQLConn.Close();
        }

        static void CheckRedistributables(int softwareid)
        {

            //Get Required Redist ID for install.
            string QueryString = "SELECT [redist_id] FROM tblRedistUsage WHERE software_id = @softwareid ORDER BY [install_order] ASC";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            List<int> RedistList = new List<int>();
            while (SQLOutput.Read())
            {
                RedistList.Add((int)SQLOutput[0]);
            }
            SQLConn.Close();

            foreach (int i in RedistList)
            {

                QueryString = "SELECT [name],[path],[filecheck],[version] from tblRedist WHERE id = @softwareid";
                SQLConn.Open();
                SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
                SQLOutput = SQLCmd.ExecuteReader();
                while (SQLOutput.Read())
                {
                    

                }
                SQLConn.Close();

            }

            //Check filecheck, if blank, check name against windows installations.


        }

        class Redistributable
        {
            public string name;
            public string path;
            public string filecheck;
            public string version;


        }

       
        public static void AddFile(string filename, string fullsource, string destination, long filesize, int softwareid)
        {
            string QueryString = "INSERT into tblFiles ([filename],[source],[destination],[filesize],[software_id]) VALUES (@filename,@sourcefile,@destinationfile,@filesize,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("filename", filename);
            SQLCmd.Parameters.AddWithValue("sourcefile", fullsource);
            SQLCmd.Parameters.AddWithValue("destinationfile", destination);
            SQLCmd.Parameters.AddWithValue("filesize", filesize);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }



        public static void RescanFileSize()
        {
            string SA = GetServers();

            string QueryString = "SELECT [id],[source] from tblFiles";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SR = SQLCmd.ExecuteReader();

            List<FileCopyOperation> FileList = new List<FileCopyOperation>();

            while (SR.Read())
            {
                FileCopyOperation tmpFCO = new FileCopyOperation();
                tmpFCO.id = (int)SR[0];
                tmpFCO.source = SR[1].ToString();
                Pri.LongPath.FileInfo FI = new Pri.LongPath.FileInfo(SA + "\\" + SR[1].ToString());
                tmpFCO.size = FI.Length;
                FileList.Add(tmpFCO);
            }
            SQLConn.Close();

            QueryString = "UPDATE tblFiles SET filesize = @filesize WHERE id = @fileid";
            foreach (FileCopyOperation FCO in FileList)
            {
                SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("filesize", FCO.size);
                SQLCmd.Parameters.AddWithValue("fileid", FCO.id);
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }


        }



        public static void AddRegistry(int softwareid, int hkey, string subkey, string value, int regtype, string data)
        {
            string QueryString = "INSERT into tblRegistry ([hkey],[subkey],[value],[type],[data],[software_id]) VALUES (@hkey,@subkey,@value,@type,@data,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("hkey", hkey);
            SQLCmd.Parameters.AddWithValue("subkey", subkey);
            SQLCmd.Parameters.AddWithValue("value", value);
            SQLCmd.Parameters.AddWithValue("type", regtype);
            SQLCmd.Parameters.AddWithValue("data", data);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();

        }

        public static void AddShortcut(string name, string location, string filepath, string runpath, string arguments, string icon, int softwareid)
        {

            string QueryString = "INSERT into tblShortcut ([name],[location],[filepath],[runpath],[arguments],[icon],[software_id]) VALUES (@name,@location,@filepath,@runpath,@arguments,@icon,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("name", name);
            SQLCmd.Parameters.AddWithValue("location", location);
            SQLCmd.Parameters.AddWithValue("filepath", filepath);
            SQLCmd.Parameters.AddWithValue("runpath", runpath);
            SQLCmd.Parameters.AddWithValue("arguments", arguments);
            SQLCmd.Parameters.AddWithValue("icon", icon);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddFirewallRule(string filepath, int softwareid)
        {
            string QueryString = "INSERT into tblFirewallExceptions ([filepath],[software_id]) VALUES (@filepath,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("filepath", filepath);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddPreferenceFile(string filepath, string target, string replace, int softwareid)
        {
            string QueryString = "INSERT into tblPreferenceFiles ([filepath],[target],[replace],[software_id]) VALUES (@filepath,@target,@replace,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("filepath", filepath);
            SQLCmd.Parameters.AddWithValue("target", target);
            SQLCmd.Parameters.AddWithValue("replace", replace);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddSerial(string name, int instancenumber, int softwareid, string regKey, string regVal)
        {
            //Check no existing serial present with same software id and instance number.
            string QueryString = "SELECT COUNT(instance) from tblSerials where [instance] = @instancenumb and [software_id] = @softwareid";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            int counter = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();

            if (counter != 0)
            {
                MessageBox.Show("A serial with the same instancen number already exists for this software.");
                return;
            }


            QueryString = "INSERT into tblSerials ([name],[instance],[regKey],[regVal],[software_id]) VALUES (@name,@instancenumb,@regKey,@regVal,@softwareid)";

            SQLConn.Open();
            SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("name", name);
            SQLCmd.Parameters.AddWithValue("instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SQLCmd.Parameters.AddWithValue("regKey", regKey);
            SQLCmd.Parameters.AddWithValue("regVal", regVal);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }


        static void GenerateFiles(string softwarename)
        {
            //Generate Directories.
            List<string> DirectoryList = new List<string>();
            foreach (FileCopyOperation FCO in FileCopyList)
            {
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

            int statuscount = 0;
            long bytecounter = 0;
            //Copy Files.
            foreach (FileCopyOperation FCO in FileCopyList)
            {
                Pri.LongPath.File.Copy(FCO.source, FCO.destination, true);
                statuscount++;
                SetStatus("Installing: " + softwarename + Environment.NewLine + "Copying Files:" + statuscount + " / " + FileCopyList.Count);

                bytecounter += FCO.size;
                SetProgress(bytecounter);
                //Provision for hashing has been put into database table.
            }
        }


        public static void AddFirewallRule(string RuleName, string EXEPath)
        {
            //netsh advfirewall firewall add rule name="My Application" dir=in action=allow program="C:\games\The Call of Duty\CoDMP.exe" enable=yes
            Process FWNetSHProc = new Process();
            FWNetSHProc.StartInfo.FileName = "c:\\windows\\system32\\netsh.exe";
            FWNetSHProc.StartInfo.Arguments = "advfirewall firewall add rule name=\"" + RuleName + "\" dir=in action=allow program=\"" + ReplaceVariable(EXEPath) + "\" enable=yes";
            FWNetSHProc.StartInfo.UseShellExecute = false;
            FWNetSHProc.StartInfo.RedirectStandardOutput = true;
            FWNetSHProc.StartInfo.CreateNoWindow = true;
            FWNetSHProc.Start();


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
                SF.Text = "Serial: " + SN.name;
                SF.FormBorderStyle = FormBorderStyle.FixedSingle;

                if (!SN.regKey.Equals(""))
                {
                    SF.txtSerial.Text = Microsoft.Win32.Registry.GetValue(SN.regKey, SN.regVal, "").ToString();
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
                tReg.SetHiveKey((int)SQLOutput[0]); //Hive Key.
                tReg.subkey = SQLOutput[1].ToString(); //Sub Key.
                tReg.value = SQLOutput[2].ToString();
                tReg.SetRegType((int)SQLOutput[3]);
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

                if (REGOP.GetHiveKey() == RegistryHive.LocalMachine)
                {
                    Registry.LocalMachine.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.LocalMachine.OpenSubKey(REGOP.subkey, true);
                }
                else if (REGOP.GetHiveKey() == RegistryHive.CurrentUser)
                {
                    Registry.CurrentUser.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.CurrentUser.OpenSubKey(REGOP.subkey, true);
                }
                else if (REGOP.GetHiveKey() == RegistryHive.Users)
                {
                    Registry.Users.CreateSubKey(REGOP.subkey, true);
                    HKEY = Registry.Users.OpenSubKey(REGOP.subkey, true);
                }

                HKEY.SetValue(REGOP.value, REGOP.data, REGOP.GetRegType());
            }
        }

        static void GetShortcuts(int SoftwareID)
        {
            ShortcutList.Clear();

            string QueryString = "SELECT [name],[location],[filepath],[runpath],[arguments],[icon] FROM [tblShortcut] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                ShortcutOperation tScut = new ShortcutOperation();
                tScut.name = SQLOutput[0].ToString();
                tScut.location = ReplaceVariable(SQLOutput[1].ToString());
                tScut.filepath = ReplaceVariable(SQLOutput[2].ToString());
                tScut.runpath = ReplaceVariable(SQLOutput[3].ToString());
                tScut.arguments = ReplaceVariable(SQLOutput[4].ToString());
                tScut.icon = ReplaceVariable(SQLOutput[5].ToString());

                ShortcutList.Add(tScut);
            }
            SQLConn.Close();
        }

        static void GenerateShortcuts()
        {
            foreach (ShortcutOperation SCO in ShortcutList)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(SCO.location + "\\" + SCO.name + ".lnk");

                shortcut.TargetPath = SCO.filepath;
                shortcut.WorkingDirectory = SCO.runpath;
                shortcut.Arguments = SCO.arguments;
                shortcut.IconLocation = SCO.icon;

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



    class FileCopyOperation
    {
        public int id;
        public string source;
        public string destination;
        public string filename;
        public long size;
    }




    class RegistryOperation
    {
        RegistryHive hkey;
        RegistryValueKind regtype;
        public string subkey;
        public string value;
        public string data;


        public void SetHiveKey(int hkey_val)
        {
            // 1 = Local Machine.
            //2 = Current User.
            //3 = Users.

            if (hkey_val == 1)
            {
                hkey = RegistryHive.LocalMachine;
            }
            else if (hkey_val == 2)
            {
                hkey = RegistryHive.CurrentUser;
            }
            else if (hkey_val == 3)
            {
                hkey = RegistryHive.Users;
            }

        }

        public RegistryHive GetHiveKey()
        {
            return hkey;
        }

        public void SetRegType(int type_val)
        {
            regtype = (RegistryValueKind)type_val;
            //string = 1
            //binary = 3
            //dword = 4
            //expanded string = 2
            //multi string = 7
            //qword = 11
        }

        public RegistryValueKind GetRegType()
        {
            return regtype;
        }




    }

    class ShortcutOperation
    {
        public string name;
        public string location;
        public string filepath;
        public string runpath;
        public string icon;
        public string arguments;
    }

    public class SerialNumber
    {
        public string name;
        public int instancenumber;
        public string serialnumber;
        public string regKey;
        public string regVal;
        public int softwareid;

    }
}
