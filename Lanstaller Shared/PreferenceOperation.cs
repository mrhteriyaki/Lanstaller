using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class PreferenceOperation
    {
        public string filename;
        public string target;
        public string replace;

        public static void AddPreferenceFile(string filepath, string target, string replace, int softwareid)
        {
            string QueryString = "INSERT into tblPreferenceFiles ([filepath],[target],[replace],[software_id]) VALUES (@filepath,@target,@replace,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@target", target);
            SQLCmd.Parameters.AddWithValue("@replace", replace);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static List<PreferenceOperation> GetPreferenceFiles(int SoftwareID)
        {
            List<PreferenceOperation> PreferenceOperationList = new List<PreferenceOperation>();
            string QueryString = "select [filepath],[target],[replace] from tblPreferenceFiles WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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

    }
}
