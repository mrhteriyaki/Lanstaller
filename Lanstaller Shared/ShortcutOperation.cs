using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lanstaller_Shared
{
    public class ShortcutOperation
    {
        public string name;
        public string location;
        public string filepath;
        public string runpath;
        public string icon;
        public string arguments;

        public static List<ShortcutOperation> GetShortcuts(int SoftwareID)
        {
            List<ShortcutOperation> ShortcutList = new List<ShortcutOperation>();

            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
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

        public static void AddShortcut(string name, string location, string filepath, string runpath, string arguments, string icon, int softwareid)
        {

            string QueryString = "INSERT into tblShortcut ([name],[location],[filepath],[runpath],[arguments],[icon],[software_id]) VALUES (@name,@location,@filepath,@runpath,@arguments,@icon,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
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




    }

}
