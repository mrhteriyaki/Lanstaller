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

        public SoftwareInfo Identity = new SoftwareInfo();

        public List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();
        public List<RegistryOperation> RegistryList = new List<RegistryOperation>();
        public List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();
        public List<SerialNumber> SerialList = new List<SerialNumber>();

        public List<FirewallRule> FirewallRuleList = new List<FirewallRule>();
        public List<PreferenceOperation> PreferenceOperationList = new List<PreferenceOperation>();
        public List<Redistributable> RedistributableList = new List<Redistributable>();


        public class SoftwareInfo
        {
            public int id;
            public string Name;
        }

        public class Tool
        {
            public int id;
            public string Name;
            public string path;
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
            SQLConn.Close();

            return tmpList;
        }

        public static List<Tool> GetTools()
        {
            List<Tool> tmpList = new List<Tool>();
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("SELECT id,[name],[path] from tblTools", SQLConn);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                Tool tmpTool = new Tool();
                tmpTool.id = (int)SQLOutput[0];
                tmpTool.Name = SQLOutput[1].ToString();
                tmpTool.path = SQLOutput[2].ToString();
                tmpList.Add(tmpTool);
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
            if (servertype != "")
            {
                SQLCmd.CommandText = "SELECT TOP (1) [address],[type] FROM [tblServers] WHERE [type] = @servertype ORDER BY [Priority] ASC";
                SQLCmd.Parameters.AddWithValue("servertype", servertype);
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
            SQLCmd.Parameters.AddWithValue("sid", softwareid);
            string softwarename = SQLCmd.ExecuteScalar().ToString();

            SQLConn.Close();

            return softwarename;

        }

        public void GetFiles()
        {
            FileCopyList.Clear();

            string QueryString = "SELECT [id],[source],[destination],[filesize],[hash_md5] from tblFiles WHERE software_id = @softwareid ORDER BY filesize ASC";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                //Prepare full source and destination locations for transfer process.
                FileCopyOperation tFCO = new FileCopyOperation();
                tFCO.fileinfo.id = (int)SQLOutput[0];
                tFCO.fileinfo.source = SQLOutput[1].ToString();
                tFCO.destination = SQLOutput[2].ToString();
                tFCO.fileinfo.size = (long)SQLOutput[3];
                tFCO.fileinfo.hash = SQLOutput[4].ToString();

                //Add copy operation to list.
                FileCopyList.Add(tFCO);
            }
            SQLConn.Close();
        }

        public void GetRegistry()
        {
            RegistryList.Clear();

            string QueryString = "select [hkey],[subkey],[value],[type],[data] from [tblRegistry] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                RegistryOperation tReg = new RegistryOperation();
                tReg.hkey = ((int)SQLOutput[0]); //Hive Key.
                tReg.subkey = SQLOutput[1].ToString(); //Sub Key.
                tReg.value = SQLOutput[2].ToString();
                tReg.regtype = ((int)SQLOutput[3]);
                tReg.data = ReplaceSerial(SQLOutput[4].ToString()); //Includes ReplaceSerial to check for Serial number.

                RegistryList.Add(tReg);
            }
            SQLConn.Close();
        }

        //Gets Serial Requirements for Queued Installs.
        public void GetSerials()
        {
            SerialList.Clear();
            string QueryString = "select [name],[instance],[regKey],[regVal] from [tblSerials] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SerialNumber tSerial = new SerialNumber();
                tSerial.softwareid = Identity.id;
                tSerial.name = SQLOutput[0].ToString();
                tSerial.instancenumber = (int)SQLOutput[1];
                tSerial.regKey = SQLOutput[2].ToString();
                tSerial.regVal = SQLOutput[3].ToString();
                SerialList.Add(tSerial);
            }
            SQLConn.Close();
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

        public void GetShortcuts()
        {
            ShortcutList.Clear();

            string QueryString = "SELECT [name],[location],[filepath],[runpath],[arguments],[icon] FROM [tblShortcut] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
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

        public void GetPreferenceFiles()
        {
            string QueryString = "select [filepath],[target],[replace] from tblPreferenceFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
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

        public void GetFirewallRules()
        {
            //Software name used for Windows Firewall rule.
            string softwarename = GetSoftwareName(Identity.id);

            FirewallRuleList.Clear(); //Reset list.

            string QueryString = "select [filepath] from tblFirewallExceptions WHERE software_id = @softwareid";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
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

        public void GetRedistributables()
        {

            //Get Required Redist ID for install.
            string QueryString = "SELECT tblRedistUsage.[redist_id],tblRedist.id,tblRedist.[name],tblRedist.[path],tblRedist.filecheck,tblRedist.[version] FROM tblRedistUsage INNER JOIN tblRedist ON tblRedistUsage.redist_id=tblRedist.id WHERE tblRedistUsage.software_id = @softwareid ORDER BY tblRedistUsage.[install_order] ASC";
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("softwareid", Identity.id);
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

        public static bool CheckSecurityToken(string token)
        {
            //tblSecurityTokens
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT COUNT(token) FROM tblSecurityTokens WHERE token = @tkval", SQLConn);
            SQLCmd.Parameters.AddWithValue("tkval",token);

            int tokencount = 0;
            SQLConn.Open();
            tokencount = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();

            if (tokencount > 0)
            {
                return true;
            }
            return false;
        }
    }
}
