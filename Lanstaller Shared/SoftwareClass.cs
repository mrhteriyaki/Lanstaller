

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions; //Regex
using System.Diagnostics;

using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Data.SqlTypes;
using System.Threading;
using Lanstaller_Shared.Models;


//Shared library of functions for client and API (Server).


namespace Lanstaller_Shared
{
    public class SoftwareClass
    {
        public static string ConnectionString;
        public SoftwareInfo Identity = new SoftwareInfo();


        public static string GetSystemData(string setting)
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT [data] from tblSystem WHERE setting = @setval", SQLConn);
            SQLCmd.Parameters.AddWithValue("@setval", setting);
            SQLConn.Open();
            string data = SQLCmd.ExecuteScalar().ToString();
            SQLConn.Close();
            return data;
        }

        public static void LoadSoftwareCounts(SoftwareInfo tmpSoftwareInfo)
        {
            //Get counts for install items.

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();

            SQLCmd.Parameters.Clear();
            SQLCmd.Parameters.AddWithValue("@softwareid", tmpSoftwareInfo.id);

            //File count.
            SQLCmd.CommandText = "SELECT COUNT(id) from tblFiles where software_id = @softwareid";
            tmpSoftwareInfo.file_count = (int)SQLCmd.ExecuteScalar();

            //registry count
            SQLCmd.CommandText = "SELECT COUNT(id) from tblRegistry WHERE software_id = @softwareid";
            tmpSoftwareInfo.registry_count = (int)SQLCmd.ExecuteScalar();

            //shortcut count
            SQLCmd.CommandText = "SELECT COUNT(id) from [tblShortcut] WHERE software_id = @softwareid";
            tmpSoftwareInfo.shortcut_count = (int)SQLCmd.ExecuteScalar();

            //Firewall rule count.
            SQLCmd.CommandText = "SELECT COUNT(id) from tblFirewallExceptions WHERE software_id = @softwareid";
            tmpSoftwareInfo.firewall_count = (int)SQLCmd.ExecuteScalar();

            //Preference files.
            SQLCmd.CommandText = "SELECT COUNT(id) from tblPreferenceFiles WHERE software_id = @softwareid";
            tmpSoftwareInfo.preference_count = (int)SQLCmd.ExecuteScalar();

            //Redist 
            SQLCmd.CommandText = "SELECT COUNT(redist_id) from tblRedistUsage WHERE software_id = @softwareid";
            tmpSoftwareInfo.redist_count = (int)SQLCmd.ExecuteScalar();

            SQLConn.Close();
        }

        public static List<SoftwareInfo> LoadSoftware()
        {
            List<SoftwareInfo> tmpList = new List<SoftwareInfo>();

            //Get List of Software from Server
            string QueryString = "SELECT tblSoftware.[id],tblSoftware.[name],tblImages.small_image FROM tblSoftware LEFT JOIN tblImages on tblSoftware.id = tblImages.software_id order by [name]";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SoftwareInfo tmpSoftware = new SoftwareInfo();
                tmpSoftware.id = (int)SQLOutput[0];
                tmpSoftware.Name = SQLOutput[1].ToString();
                tmpSoftware.image_small = SQLOutput[2].ToString();
                tmpList.Add(tmpSoftware);
            }
            SQLOutput.Close();
            SQLConn.Close();

            foreach (SoftwareInfo SW in tmpList)
            {
                LoadSoftwareCounts(SW);
                SW.install_size = GetInstallSize(SW.id);
            }

            return tmpList;
        }


        public static int AddSoftware(string softwarename)
        {
            string QueryString = "INSERT into tblSoftware ([name]) VALUES (@softname); SELECT SCOPE_IDENTITY();";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softname", softwarename);
            object output = SQLCmd.ExecuteScalar();
            int idval = int.Parse(output.ToString());
            SQLConn.Close();
            return idval;
        }

        public static void DeleteSoftware(int software_id)
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLCmd.Parameters.AddWithValue("@softid", software_id);

            SQLConn.Open();

            //Delete Files.
            SQLCmd.CommandText = "DELETE FROM tblFiles WHERE software_id = @softid" +
            "DELETE FROM tblRegistry WHERE software_id = @softid;" +
            "DELETE FROM tblCompatibility WHERE software_id = @softid;" +
            "DELETE FROM tblFirewallExceptions WHERE software_id = @softid;" +
            "DELETE FROM tblPreferenceFiles WHERE software_id = @softid;" +
            "DELETE FROM tblSerialsAvailable WHERE serial_id = (SELECT id FROM tblSerials WHERE software_id = @softid);" +
            "DELETE FROM tblSerials WHERE software_id = @softid;" +
            "DELETE FROM tblShortcut WHERE software_id = @softid;" +
            "DELETE FROM tblRedistUsage WHERE software_id = @softid;" +
            "DELETE FROM tblSoftware WHERE id = @softid;";
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }


        public static List<Server> GetFileServer() //web or smb for type.
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLCmd.CommandText = "SELECT [address],[protocol],[priority] FROM [tblServers] ORDER BY [Priority] ASC";

            SQLConn.Open();
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            List<Server> tmpList = new List<Server>();
            while (SQLOutput.Read())
            {
                tmpList.Add(new Server()
                {
                    path = SQLOutput[0].ToString(),
                    protocol = (int)SQLOutput[1],
                    priority = (int)SQLOutput[2]
                });
            }
            SQLConn.Close();
            return tmpList;
        }

        public static string GetSoftwareName(int softwareid)
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand()
            {
                CommandText = "SELECT TOP(1) [name] FROM [tblSoftware] WHERE id = @sid",
                Connection = SQLConn
            };
            SQLCmd.Parameters.AddWithValue("@sid", softwareid);
            string softwarename = SQLCmd.ExecuteScalar().ToString();
            SQLConn.Close();
            return softwarename;
        }

        public static List<FileCopyOperation> GetFiles(int SoftwareID)
        {
            List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();

            string QueryString = "SELECT [id],[source],[destination],[filesize],[hash_md5] from tblFiles WHERE software_id = @softwareid ORDER BY filesize ASC";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID); //Identity.id
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                //Prepare full source and destination locations for transfer process.
                FileCopyOperation tFCO = new FileCopyOperation();
                tFCO.fileinfo.id = (int)SQLOutput[0];

                string strsource = SQLOutput[1].ToString().Replace("\\", "/");

                if (strsource.Contains("/"))
                {
                    string[] pathnames = strsource.Split('/');
                    StringBuilder encodedsource = new StringBuilder();
                    foreach (string pth in pathnames)
                    {
                        encodedsource.Append("/" + Uri.EscapeDataString(pth));
                        //encodedsource.Append("/" + HttpUtility.UrlEncode(pth));
                    }
                    tFCO.fileinfo.source = encodedsource.ToString();
                }
                else
                {
                    //For files in root directory, unlikely but just incase.
                    tFCO.fileinfo.source = "/" + Uri.EscapeDataString(strsource);
                    //tFCO.fileinfo.source = "/" + HttpUtility.UrlEncode(strsource);
                }

                tFCO.destination = SQLOutput[2].ToString();
                tFCO.fileinfo.size = (long)SQLOutput[3];
                tFCO.fileinfo.hash = SQLOutput[4].ToString();

                //Add copy operation to list.
                FileCopyList.Add(tFCO);
            }
            SQLConn.Close();

            return FileCopyList;
        }

        public static List<string> GetDirectories(int SoftwareID)
        {
            List<string> DirList = new List<string>();
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("SELECT [path] FROM tblDirectories WHERE software_id = @softwareid", SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID); //Identity.id
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                DirList.Add(SQLOutput[0].ToString());
            }
            SQLConn.Close();

            return DirList;
        }

        public static List<RegistryOperation> GetRegistry(int SoftwareID)
        {
            List<RegistryOperation> RegistryList = new List<RegistryOperation>();


            string QueryString = "select [hkey],[subkey],[value],[type],[data] from [tblRegistry] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                RegistryOperation tReg = new RegistryOperation();
                tReg.hkey = ((int)SQLOutput[0]); //Hive Key.
                tReg.subkey = SQLOutput[1].ToString(); //Sub Key.
                tReg.value = SQLOutput[2].ToString();
                tReg.regtype = ((int)SQLOutput[3]);
                tReg.data = SQLOutput[4].ToString();

                RegistryList.Add(tReg);
            }
            SQLConn.Close();

            return RegistryList;
        }


        public static List<Compatibility> GetCompatibility(int swid)
        {
            //Set compatibility flags in registry.
            List<Compatibility> CompatList = new List<Compatibility>();

            string QueryString = "select filename,compat_type from tblCompatibility where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", swid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                Compatibility tCompat = new Compatibility();
                tCompat.filename = SQLOutput[0].ToString();
                tCompat.compat_type = (int)SQLOutput[1];
                CompatList.Add(tCompat);
            }
            SQLConn.Close();

            return CompatList;
        }

        //Gets Serial Requirements for Queued Installs.
        public static List<SerialNumber> GetSerials(int SoftwareID)
        {
            List<SerialNumber> SerialList = new List<SerialNumber>();
            SqlCommand SQLCmd = new SqlCommand();
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();
            SQLCmd.CommandText = "select [id],[name],[instance],[regKey],[regVal],[format] from [tblSerials] WHERE software_id = @softwareid ORDER BY INSTANCE ASC";
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SerialNumber tSerial = new SerialNumber();
                tSerial.softwareid = SoftwareID;
                tSerial.serialid = (int)SQLOutput[0];
                tSerial.name = SQLOutput[1].ToString();
                tSerial.instancenumber = (int)SQLOutput[2];
                tSerial.regKey = SQLOutput[3].ToString();
                tSerial.regVal = SQLOutput[4].ToString();
                tSerial.format = SQLOutput[5].ToString();
                SerialList.Add(tSerial);
            }
            SQLConn.Close();
            return SerialList;

        }





        public static List<ShortcutOperation> GetShortcuts(int SoftwareID)
        {
            List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLCmd.CommandText = "SELECT [name],[location],[filepath],[runpath],[arguments],[icon] FROM [tblShortcut] WHERE software_id = @softwareid";
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                ShortcutOperation tScut = new ShortcutOperation();
                tScut.name = SQLOutput[0].ToString();
                tScut.location = SQLOutput[1].ToString();
                tScut.filepath = SQLOutput[2].ToString();
                tScut.runpath = SQLOutput[3].ToString();
                tScut.arguments = SQLOutput[4].ToString();
                tScut.icon = SQLOutput[5].ToString();
                ShortcutList.Add(tScut);
            }
            SQLConn.Close();

            return ShortcutList;
        }

        public static List<PreferenceOperation> GetPreferenceFiles(int SoftwareID)
        {
            List<PreferenceOperation> PreferenceOperationList = new List<PreferenceOperation>();
            string QueryString = "select [filepath],[target],[replace] from tblPreferenceFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                PreferenceOperation tmpPreferenceOperation = new PreferenceOperation();
                tmpPreferenceOperation.filename = SQLOutput[0].ToString();
                tmpPreferenceOperation.target = SQLOutput[1].ToString();
                tmpPreferenceOperation.replace = SQLOutput[2].ToString();
                PreferenceOperationList.Add(tmpPreferenceOperation);
            }

            SQLConn.Close();

            return PreferenceOperationList;
        }

        public static List<FirewallRule> GetFirewallRules(int SoftwareID)
        {
            List<FirewallRule> FirewallRuleList = new List<FirewallRule>();

            //Software name used for Windows Firewall rule.
            string softwarename = GetSoftwareName(SoftwareID);

            string QueryString = "select [filepath],[rulename],[proto_scope],[port_scope] from tblFirewallExceptions WHERE software_id = @softwareid";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                FirewallRule rule = new FirewallRule();
                rule.softwarename = softwarename;
                rule.exepath = SQLOutput[0].ToString();
                rule.rulename = SQLOutput[1].ToString();
                if (SQLOutput[2] != DBNull.Value)
                {
                    rule.protocol_value = (int)SQLOutput[2];
                }
                if (SQLOutput[3] != DBNull.Value)
                {
                    rule.port_number = (int)SQLOutput[3];
                }
                FirewallRuleList.Add(rule);
            }
            SQLConn.Close();
            return FirewallRuleList;
        }

        public static List<Redistributable> GetRedistributables(int SoftwareID)
        {
            List<Redistributable> RedistributableList = new List<Redistributable>();

            string server_path = GetFileServer()[0].path;

            //Get Required Redist ID for install.
            string QueryString = "SELECT tblRedistUsage.[redist_id],tblRedist.id,tblRedist.[name],tblRedist.[path],tblRedist.args,tblRedist.filecheck,tblRedist.[version],tblRedist.compressed,tblRedist.compressed_path FROM tblRedistUsage INNER JOIN tblRedist ON tblRedistUsage.redist_id=tblRedist.id WHERE tblRedistUsage.software_id = @softwareid ORDER BY tblRedistUsage.[install_order] ASC";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                Redistributable tmpRedist = new Redistributable();
                tmpRedist.name = SQLOutput["name"].ToString();
                tmpRedist.path = server_path + SQLOutput["path"].ToString();
                tmpRedist.args = SQLOutput["args"].ToString();
                tmpRedist.filecheck = SQLOutput["filecheck"].ToString();
                tmpRedist.version = SQLOutput["version"].ToString();
                tmpRedist.compressed = SQLOutput["compressed"].ToString();
                tmpRedist.compressed_path = SQLOutput["compressed_path"].ToString();

                RedistributableList.Add(tmpRedist);
            }
            SQLConn.Close();

            return RedistributableList;

            //Redistributable         

        }

        public static long GetInstallSize(int SoftwareID)
        {
            string QueryString = "SELECT SUM(filesize) FROM tblFiles where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            long filesize = 0;
            string filesizestr = SQLCmd.ExecuteScalar().ToString();
            if (filesizestr != "")
            {
                filesize = long.Parse(filesizestr);
            }

            SQLConn.Close();
            return filesize;
        }

        public static void AddSerial(string name, int instancenumber, int softwareid, string regKey, string regVal, string SerialFormat)
        {
            //Check no existing serial present with same software id and instance number.
            string QueryString = "SELECT COUNT(instance) from tblSerials where [instance] = @instancenumb and [software_id] = @softwareid";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            int counter = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();

            if (counter != 0)
            {
                //MessageBox.Show("A serial with the same instancen number already exists for this software.");
                throw new Exception("A serial with the same instancen number already exists for this software.");
                //return;
            }


            QueryString = "INSERT into tblSerials ([name],[instance],[regKey],[regVal],[software_id],[format]) VALUES (@name,@instancenumb,@regKey,@regVal,@softwareid,@serformat)";

            SQLConn.Open();
            SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@name", name);
            SQLCmd.Parameters.AddWithValue("@instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.Parameters.AddWithValue("@regKey", regKey);
            SQLCmd.Parameters.AddWithValue("@regVal", regVal);
            SQLCmd.Parameters.AddWithValue("@serformat", SerialFormat);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddUserSerial(int SerialID, string Serial)
        {
            //Add serial number 
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLConn.Open();
            SQLCmd.Parameters.AddWithValue("@sid", SerialID);
            SQLCmd.CommandText = "INSERT INTO tblSerialsAvailable (serial_id,serial_value) VALUES (@sid,@sval)";
            SQLCmd.Parameters.AddWithValue("@sval", Serial);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }


        public static void AddShortcut(string name, string location, string filepath, string runpath, string arguments, string icon, int softwareid)
        {

            string QueryString = "INSERT into tblShortcut ([name],[location],[filepath],[runpath],[arguments],[icon],[software_id]) VALUES (@name,@location,@filepath,@runpath,@arguments,@icon,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@name", name);
            SQLCmd.Parameters.AddWithValue("@location", location);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@runpath", runpath);
            SQLCmd.Parameters.AddWithValue("@arguments", arguments);
            SQLCmd.Parameters.AddWithValue("@icon", icon);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddFirewallRule(string filepath, string rulename, int softwareid)
        {
            string QueryString = "INSERT into tblFirewallExceptions ([filepath],[software_id],[rulename]) VALUES (@filepath,@softwareid,@rulename)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            if (string.IsNullOrEmpty(rulename))
            {
                SQLCmd.Parameters.AddWithValue("@rulename", DBNull.Value);
            }
            else
            {
                SQLCmd.Parameters.AddWithValue("@rulename", rulename);
            }

            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddPreferenceFile(string filepath, string target, string replace, int softwareid)
        {
            string QueryString = "INSERT into tblPreferenceFiles ([filepath],[target],[replace],[software_id]) VALUES (@filepath,@target,@replace,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@target", target);
            SQLCmd.Parameters.AddWithValue("@replace", replace);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }


        public static void AddRegistry(int softwareid, int hkey, string subkey, string value, int regtype, string data)
        {
            string QueryString = "INSERT into tblRegistry ([hkey],[subkey],[value],[type],[data],[software_id]) VALUES (@hkey,@subkey,@value,@type,@data,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@hkey", hkey);
            SQLCmd.Parameters.AddWithValue("@subkey", subkey);
            SQLCmd.Parameters.AddWithValue("@value", value);
            SQLCmd.Parameters.AddWithValue("@type", regtype);
            SQLCmd.Parameters.AddWithValue("@data", data);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();

        }


        public static void AddFile(string source, string destination, long filesize, int softwareid)
        {
            string QueryString = "INSERT into tblFiles ([source],[destination],[filesize],[software_id]) VALUES (@sourcefile,@destinationfile,@filesize,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@sourcefile", source);
            SQLCmd.Parameters.AddWithValue("@destinationfile", destination);
            SQLCmd.Parameters.AddWithValue("@filesize", filesize);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static void AddDirectory(string directorypath, int softwareid)
        {
            string QueryString = "INSERT into tblDirectories ([path],[software_id]) VALUES (@dirpath,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@dirpath", directorypath);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static int GetUnhashedFileCount()
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("SELECT COUNT(id) from tblFiles where hash_md5 is null", SQLConn);
            int count = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();
            return count;
        }


        public static void RescanFileHashes(bool fullrescan, int software_id)
        {
            Server SA = GetFileServer()[0];
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            string QueryString = "SELECT [id],[source],[hash_md5] from tblFiles ";
            if (software_id != 0)
            {
                QueryString += "WHERE software_id = @swid";
            }

            if (fullrescan == false)
            {
                if (software_id == 0)
                {
                    QueryString += " WHERE [hash_md5] is null";
                }
                else
                {
                    QueryString += " AND [hash_md5] is null";
                }
            }
            else
            {
                //Full Rescan, clear existing hashes for progress check.
                QueryString += "; UPDATE tblFiles SET [hash_md5] = NULL WHERE software_id = @swid";
            }


            SQLCmd.CommandText = QueryString;
            SQLConn.Open();
            if (software_id > 0)
            {
                SQLCmd.Parameters.AddWithValue("@swid", software_id);
            }

            SqlDataReader SR = SQLCmd.ExecuteReader();

            List<FileInfoClass> FileHashList = new List<FileInfoClass>();

            while (SR.Read())
            {
                FileInfoClass tmpFH = new FileInfoClass();
                tmpFH.id = (int)SR[0];
                tmpFH.source = SR[1].ToString();
                tmpFH.hash = SR[2].ToString();
                FileHashList.Add(tmpFH);
            }
            SQLConn.Close();


            QueryString = "UPDATE tblFiles SET [hash_md5] = @hash WHERE id = @fileid";

            //RescanFileHashes

            foreach (FileInfoClass FH in FileHashList)
            {
                FH.hash = CalculateMD5(SA.path + "\\" + FH.source);
                SQLConn = new SqlConnection(ConnectionString);
                SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("@fileid", FH.id);
                SQLCmd.Parameters.AddWithValue("@hash", FH.hash);

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }
        }

        public static string CalculateMD5(string filename)
        {
            bool filelockerror = true; //loop on file lock errors.
            while (filelockerror)
            {
                try
                {
                    MD5 md5 = MD5.Create();
                    FileStream stream = File.OpenRead(filename);
                    byte[] hash = md5.ComputeHash(stream);
                    stream.Close();
                    filelockerror = false;
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("File hash failed: " + ex.Message);
                }
            }
            return string.Empty;

        }

    }
}
