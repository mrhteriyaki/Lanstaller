using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class SoftwareInfo
    {
        public int id;
        public string Name;
        public int file_count;
        public int registry_count;
        public int shortcut_count;
        public int firewall_count;
        public int preference_count;
        public int redist_count;
        public long install_size;
        public string image_small; //Icon Image for List.

        public static void LoadSoftwareCounts(SoftwareInfo tmpSoftwareInfo)
        {
            //Get counts for install items.

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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

        public static string GetSoftwareName(int softwareid)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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


        public static List<SoftwareInfo> LoadSoftware()
        {
            List<SoftwareInfo> tmpList = new List<SoftwareInfo>();

            //Get List of Software from Server
            string QueryString = "SELECT tblSoftware.[id],tblSoftware.[name],tblImages.small_image FROM tblSoftware LEFT JOIN tblImages on tblSoftware.id = tblImages.software_id order by [name]";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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

        public static long GetInstallSize(int SoftwareID)
        {
            string QueryString = "SELECT SUM(filesize) FROM tblFiles where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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


    }
}
