using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Reflection.PortableExecutable;

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

        public static void UpdateSoftwareCounts(SoftwareInfo tmpSoftwareInfo)
        {
            tmpSoftwareInfo.install_size = GetInstallSize(tmpSoftwareInfo.id);

            //Get counts for install items.

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();

            SQLCmd.Parameters.Clear();
            SQLCmd.Parameters.AddWithValue("@softwareid", tmpSoftwareInfo.id);

            SQLCmd.CommandText = @"
                SELECT 
                (SELECT COUNT(id) FROM tblFiles WHERE software_id = @softwareid) AS file_count,
                (SELECT COUNT(id) FROM tblRegistry WHERE software_id = @softwareid) AS registry_count,
                (SELECT COUNT(id) FROM tblShortcut WHERE software_id = @softwareid) AS shortcut_count,
                (SELECT COUNT(id) FROM tblFirewallExceptions WHERE software_id = @softwareid) AS firewall_count,
                (SELECT COUNT(id) FROM tblPreferenceFiles WHERE software_id = @softwareid) AS preference_count,
                (SELECT COUNT(redist_id) FROM tblRedistUsage WHERE software_id = @softwareid) AS redist_count";

            using (SqlDataReader reader = SQLCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    tmpSoftwareInfo.file_count = reader.GetInt32(0);
                    tmpSoftwareInfo.registry_count = reader.GetInt32(1);
                    tmpSoftwareInfo.shortcut_count = reader.GetInt32(2);
                    tmpSoftwareInfo.firewall_count = reader.GetInt32(3);
                    tmpSoftwareInfo.preference_count = reader.GetInt32(4);
                    tmpSoftwareInfo.redist_count = reader.GetInt32(5);
                }
            }

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
            string QueryString = @"
SELECT 
    s.id AS software_id,
    s.name,
    tblImages.small_image,
    COALESCE(f.file_count, 0) AS file_count,
    COALESCE(f.totalsize, 0) AS totalsize,
    COALESCE(r.registry_count, 0) AS registry_count,
    COALESCE(sh.shortcut_count, 0) AS shortcut_count,
    COALESCE(fw.firewall_count, 0) AS firewall_count,
    COALESCE(p.preference_count, 0) AS preference_count,
    COALESCE(rd.redist_count, 0) AS redist_count
FROM 
    tblSoftware s
LEFT JOIN (
    SELECT software_id, COUNT(id) AS file_count, SUM(filesize) as totalsize
    FROM tblFiles
    GROUP BY software_id
) f ON s.id = f.software_id
LEFT JOIN (
    SELECT software_id, COUNT(id) AS registry_count
    FROM tblRegistry
    GROUP BY software_id
) r ON s.id = r.software_id
LEFT JOIN (
    SELECT software_id, COUNT(id) AS shortcut_count
    FROM tblShortcut
    GROUP BY software_id
) sh ON s.id = sh.software_id
LEFT JOIN (
    SELECT software_id, COUNT(id) AS firewall_count
    FROM tblFirewallExceptions
    GROUP BY software_id
) fw ON s.id = fw.software_id
LEFT JOIN (
    SELECT software_id, COUNT(id) AS preference_count
    FROM tblPreferenceFiles
    GROUP BY software_id
) p ON s.id = p.software_id
LEFT JOIN (
    SELECT software_id, COUNT(redist_id) AS redist_count
    FROM tblRedistUsage
    GROUP BY software_id
) rd ON s.id = rd.software_id

LEFT JOIN tblImages on s.id = tblImages.software_id order by [name]
";

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

                tmpSoftware.file_count = SQLOutput.GetInt32(3);
                tmpSoftware.install_size = SQLOutput.GetInt64(4);
                tmpSoftware.registry_count = SQLOutput.GetInt32(5);
                tmpSoftware.shortcut_count = SQLOutput.GetInt32(6);
                tmpSoftware.firewall_count = SQLOutput.GetInt32(7);
                tmpSoftware.preference_count = SQLOutput.GetInt32(8);
                tmpSoftware.redist_count = SQLOutput.GetInt32(9);


                tmpList.Add(tmpSoftware);
            }
            SQLOutput.Close();
            SQLConn.Close();

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
