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


//Shared library of functions for client and API (Server).


namespace Lanstaller_Shared
{
    public class SoftwareClass
    {
        public static string ConnectionString;

        public SoftwareInfo Identity = new SoftwareInfo();


        public class SoftwareInfo
        {
            public int id;
            public string Name;
            public int file_count;
            public int registry_count;
            public int shortcut_count;
            public int firewall_count;
        }

        public class FileInfoClass
        {
            public int id;
            public long size;
            public string hash;
            public string source;

        }

        public class FileCopyOperation
        {
            public FileInfoClass fileinfo = new FileInfoClass();
            public string destination;
        }


        public class ShortcutOperation
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
            public int serialid;
            public string name;
            public int instancenumber;
            public string serialnumber;
            public string regKey;
            public string regVal;
            public int softwareid;


            //Filter symbols from serial key input boxes (management and client)
            public static string FilterSerial(string serial_value)
            {
                return serial_value.Replace("-", "").Replace(" ", "");
            }

        }

        public class FirewallRule
        {
            public string softwarename;
            public string exepath;
        }

        public class PreferenceOperation
        {
            public string filename;
            public string target;
            public string replace;
        }

        public class Redistributable
        {
            public string name;
            public string path;
            public string filecheck;
            public string args;
            public string version;
            public string compressed;
            public string compressed_path;
        }

        public class Server
        {
            public string path;
            public string protocol;
        }



        public class RegistryOperation
        {
            public int hkey;
            //1 = Local Machine.
            //2 = Current User.
            //3 = Users.

            public int regtype;
            //string = 1
            //binary = 3
            //dword = 4
            //expanded string = 2
            //multi string = 7
            //qword = 11

            public string subkey;
            public string value;
            public string data;

        }

        public class Compatibility
        {
            public string filename;
            public int compat_type;
        }


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

            SQLConn.Close();
        }

        public static List<SoftwareInfo> LoadSoftware()
        {
            List<SoftwareInfo> tmpList = new List<SoftwareInfo>();

            //Get List of Software from Server
            string QueryString = "SELECT [id],[name] from tblSoftware order by [name]";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SoftwareInfo tmpSoftware = new SoftwareInfo();
                tmpSoftware.id = (int)SQLOutput[0];
                tmpSoftware.Name = SQLOutput[1].ToString();
                tmpList.Add(tmpSoftware);
            }
            SQLOutput.Close();
            SQLConn.Close();

            foreach (SoftwareInfo SW in tmpList)
            {
                LoadSoftwareCounts(SW);
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
            SQLCmd.CommandText = "DELETE FROM tblFiles WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete Registry
            SQLCmd.CommandText = "DELETE FROM tblRegistry WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete Compatibility
            SQLCmd.CommandText = "DELETE FROM tblCompatibility WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete Firewall rules.
            SQLCmd.CommandText = "DELETE FROM tblFirewallExceptions WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete Preference files
            SQLCmd.CommandText = "DELETE FROM tblPreferenceFiles WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete available serials.
            SQLCmd.CommandText = "DELETE FROM tblSerialsAvailable WHERE serial_id = (SELECT id FROM tblSerials WHERE software_id = @softid)";
            SQLCmd.ExecuteNonQuery();

            //Delete serials
            SQLCmd.CommandText = "DELETE FROM tblSerials WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();


            //Delete shortcuts
            SQLCmd.CommandText = "DELETE FROM tblShortcut WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //delete Redist Usage
            SQLCmd.CommandText = "DELETE FROM tblRedistUsage WHERE software_id = @softid";
            SQLCmd.ExecuteNonQuery();

            //Delete software index.
            SQLCmd.CommandText = "DELETE FROM tblSoftware WHERE id = @softid";
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();


        }

        public static Server GetFileServer()
        {
            return GetFileServer("");
        }

        public static Server GetFileServer(string servertype)
        {
            //ServerList.Clear();
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLCmd.CommandText = "SELECT TOP (1) [address],[type] FROM [tblServers] ORDER BY [Priority] ASC";
            //server type specified (web / smb)
            if (servertype != "")
            {
                SQLCmd.CommandText = "SELECT TOP (1) [address],[type] FROM [tblServers] WHERE [type] = @servertype ORDER BY [Priority] ASC";
                SQLCmd.Parameters.AddWithValue("@servertype", servertype);
            }

            SQLConn.Open();
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            Server tmpServ = new Server();
            while (SQLOutput.Read())
            {
                tmpServ.path = SQLOutput[0].ToString();
                tmpServ.protocol = SQLOutput[1].ToString();
            }
            SQLConn.Close();
            return tmpServ;
        }

        public static string GetSoftwareName(int softwareid)
        {
            //ServerList.Clear();
            string QueryString = "SELECT TOP(1) [name] FROM [tblSoftware] WHERE id = @sid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@sid", softwareid);
            string softwarename = "";
            softwarename = SQLCmd.ExecuteScalar().ToString();

            SQLConn.Close();

            return softwarename;

        }

        public static List<FileCopyOperation> GetFiles(int SoftwareID)
        {
            List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();
            //FileCopyList.Clear();

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

                string strsource = SQLOutput[1].ToString();
                strsource = strsource.Replace("\\", "/");

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

            string QueryString = "select [id],[name],[instance],[regKey],[regVal] from [tblSerials] WHERE software_id = @softwareid ORDER BY INSTANCE ASC";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
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
                SerialList.Add(tSerial);
            }
            SQLConn.Close();
            return SerialList;

        }

        public class UserSerial
        {
            public int id;
            public string serial;
            public string used_timestamp;


            //Get available serial numbers.
            public static List<UserSerial> GetUserSerials(int SerialID)
            {
                string QueryString = "select [id],[serial_value],[serial_used] from [tblSerialsAvailable] WHERE serial_id = @serialid";

                List<UserSerial> SerialList = new List<UserSerial>();
                SqlConnection SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("@serialid", SerialID);
                SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
                while (SQLOutput.Read())
                {
                    UserSerial tmpUserSerial = new UserSerial();
                    tmpUserSerial.id = (int)SQLOutput[0];
                    tmpUserSerial.serial = SQLOutput[1].ToString();
                    tmpUserSerial.used_timestamp = SQLOutput[2].ToString();
                    SerialList.Add(tmpUserSerial);
                }
                SQLConn.Close();
                return SerialList;
            }

            //Mark available serial as used.
            public static void SetAvailableSerial(int PoolSerialID)
            {
                SqlConnection SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand("UPDATE [tblSerialsAvailable] SET serial_used = GETDATE() WHERE id = @sid", SQLConn);
                SQLCmd.Parameters.AddWithValue("@sid", PoolSerialID);
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }


            //Add serial to pool.
            public static void AddAvailableSerial(int SerialID, string Serial)
            {
                SqlConnection SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand("INSERT INTO [tblSerialsAvailable] ([serial_id],[serial_value]) VALUES (@sid,@sval)", SQLConn);
                SQLCmd.Parameters.AddWithValue("@sid", SerialID);
                SQLCmd.Parameters.AddWithValue("@sval", Serial);
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }

            //Remove serial from pool.
            public static void DeleteAvailableSerial(int UserSerialID)
            {
                SqlConnection SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SqlCommand SQLCmd = new SqlCommand("DELETE FROM [tblSerialsAvailable] WHERE id = @usersid", SQLConn);
                SQLCmd.Parameters.AddWithValue("@usersid", UserSerialID);
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }
        }

        

        public static List<ShortcutOperation> GetShortcuts(int SoftwareID)
        {
            List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();
            ShortcutList.Clear();

            string QueryString = "SELECT [name],[location],[filepath],[runpath],[arguments],[icon] FROM [tblShortcut] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
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

            FirewallRuleList.Clear(); //Reset list.

            string QueryString = "select [filepath] from tblFirewallExceptions WHERE software_id = @softwareid";
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
                FirewallRuleList.Add(rule);
            }
            SQLConn.Close();
            return FirewallRuleList;
        }

        public static List<Redistributable> GetRedistributables(int SoftwareID)
        {
            List<Redistributable> RedistributableList = new List<Redistributable>();

            string server_path = GetFileServer("web").path;

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

        public static void AddSerial(string name, int instancenumber, int softwareid, string regKey, string regVal)
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


            QueryString = "INSERT into tblSerials ([name],[instance],[regKey],[regVal],[software_id]) VALUES (@name,@instancenumb,@regKey,@regVal,@softwareid)";

            SQLConn.Open();
            SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@name", name);
            SQLCmd.Parameters.AddWithValue("@instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue(@"softwareid", softwareid);
            SQLCmd.Parameters.AddWithValue("@regKey", regKey);
            SQLCmd.Parameters.AddWithValue("@regVal", regVal);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddUserSerial(int SerialID, string Serial)
        {
            string QueryString = "INSERT INTO tblSerialsAvailable (serial_id,serial_value) VALUES (@sid,@sval)";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@sid", SerialID);
            SQLCmd.Parameters.AddWithValue("@sval", Serial) ;
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

        public static void AddFirewallRule(string filepath, int softwareid)
        {
            string QueryString = "INSERT into tblFirewallExceptions ([filepath],[software_id]) VALUES (@filepath,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
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


        public static void RescanFileHashes(bool fullrescan)
        {
            Server SA = GetFileServer("smb");

            string QueryString = "SELECT [id],[source],[hash_md5] from tblFiles";
            if (fullrescan == false)
            {
                QueryString += " WHERE [hash_md5] is null";
            }


            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
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
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

    }
}
