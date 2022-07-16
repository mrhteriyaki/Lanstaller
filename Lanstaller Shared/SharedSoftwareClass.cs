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


//Shared library of functions for client and API (Server).


namespace Lanstaller_Shared
{
    public class SoftwareClass
    {
        public static string ConnectionString;

        public int id;
        public string Name;

        public static List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();
        public static List<RegistryOperation> RegistryList = new List<RegistryOperation>();
        public static List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();
        public static List<SerialNumber> SerialList = new List<SerialNumber>();

        public static List<FirewallRule> FirewallRuleList = new List<FirewallRule>();
        public static List<PreferenceOperation> PreferenceOperationList = new List<PreferenceOperation>();
        public static List<Redistributable> RedistributableList = new List<Redistributable>();


        public class FileInfoClass
        {
            public int id;
            public long size;
            public string hash;
            public string source;
        }

        public class FileCopyOperation
        {
            public FileInfoClass fileinfo;
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
            public string name;
            public int instancenumber;
            public string serialnumber;
            public string regKey;
            public string regVal;
            public int softwareid;
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
            public string version;
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

        public static string GetServers()
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

        public static string GetSoftwareName(int softwareid)
        {
            //ServerList.Clear();
            string QueryString = "SELECT TOP(1) [name] FROM [tblSoftware] WHERE id = @sid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("sid", softwareid);
            string softwarename = SQLCmd.ExecuteScalar().ToString();

            SQLConn.Close();

            return softwarename;

        }

        public static void GetFiles(int softwareid, string ServerAddress)
        {
            FileCopyList.Clear();

            string QueryString = "select [source],[destination],[filesize] from tblFiles WHERE software_id = @softwareid ORDER BY filesize ASC";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                //Prepare full source and destination locations for transfer process.

                FileCopyOperation tFCO = new FileCopyOperation();
                tFCO.fileinfo.source = ServerAddress + "\\" + SQLOutput[0].ToString();
                tFCO.destination = SQLOutput[1].ToString();
                tFCO.fileinfo.size = (long)SQLOutput[2];

                //Add copy operation to list.
                FileCopyList.Add(tFCO);

            }
            SQLConn.Close();
        }

        public static void GetShortcuts(int SoftwareID)
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
                tScut.location = SQLOutput[1].ToString();
                tScut.filepath = SQLOutput[2].ToString();
                tScut.runpath = SQLOutput[3].ToString();
                tScut.arguments = SQLOutput[4].ToString();
                tScut.icon = SQLOutput[5].ToString();

                ShortcutList.Add(tScut);
            }
            SQLConn.Close();
        }

        public static void GetPreferenceFiles(int softwareid)
        {
            string QueryString = "select [filepath],[target],[replace] from tblPreferenceFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
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
        }

        public static void GetFirewallRules(int softwareid)
        {
            //Software name used for Windows Firewall rule.
            string softwarename = GetSoftwareName(softwareid);

            FirewallRuleList.Clear(); //Reset list.

            string QueryString = "select [filepath] from tblFirewallExceptions WHERE software_id = @softwareid";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                FirewallRule rule = new FirewallRule();
                rule.softwarename = softwarename;
                rule.exepath = SQLOutput[0].ToString();
                FirewallRuleList.Add(rule);
            }
            SQLConn.Close();
        }

        public static void GetRedistributables(int softwareid)
        {

            //Get Required Redist ID for install.
            string QueryString = "SELECT tblRedistUsage.[redist_id],tblRedist.id,tblRedist.[name],tblRedist.[path],tblRedist.filecheck,tblRedist.[version] FROM tblRedistUsage INNER JOIN tblRedist ON tblRedistUsage.redist_id=tblRedist.id WHERE tblRedistUsage.software_id = @softwareid ORDER BY tblRedistUsage.[install_order] ASC";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", softwareid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                Redistributable tmpRedist = new Redistributable();
                tmpRedist.name = SQLOutput[2].ToString();
                tmpRedist.path = SQLOutput[3].ToString();
                tmpRedist.filecheck = SQLOutput[4].ToString();
                tmpRedist.version = SQLOutput[5].ToString();

                RedistributableList.Add(tmpRedist);
            }
            SQLConn.Close();


            //Redistributable         

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
                //MessageBox.Show("A serial with the same instancen number already exists for this software.");
                throw new Exception("A serial with the same instancen number already exists for this software.");
                //return;
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


        public static void AddFile(string source, string destination, long filesize, int softwareid)
        {
            string QueryString = "INSERT into tblFiles ([source],[destination],[filesize],[software_id]) VALUES (@sourcefile,@destinationfile,@filesize,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("sourcefile", source);
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
                tmpFCO.fileinfo.id = (int)SR[0];
                tmpFCO.fileinfo.source = SR[1].ToString();
                Pri.LongPath.FileInfo FI = new Pri.LongPath.FileInfo(SA + "\\" + SR[1].ToString());
                tmpFCO.fileinfo.size = FI.Length;
                FileList.Add(tmpFCO);
            }
            SQLConn.Close();

            QueryString = "UPDATE tblFiles SET filesize = @filesize WHERE id = @fileid";
            foreach (FileCopyOperation FCO in FileList)
            {
                SQLConn = new SqlConnection(ConnectionString);
                SQLConn.Open();
                SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("filesize", FCO.fileinfo.size);
                SQLCmd.Parameters.AddWithValue("fileid", FCO.fileinfo.id);
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }


        }

        public static void RescanFileHashes(bool fullrescan)
        {
            string SA = GetServers();

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
                FH.hash = CalculateMD5(SA + "\\" + FH.source);
                SQLConn = new SqlConnection(ConnectionString);
                SQLCmd = new SqlCommand(QueryString, SQLConn);
                SQLCmd.Parameters.AddWithValue("fileid", FH.id);
                SQLCmd.Parameters.AddWithValue("hash", FH.hash);

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
