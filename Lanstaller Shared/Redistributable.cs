using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class Redistributable
    {
        public int id;
        public string name;
        public string path;
        public string filecheck;
        public string args;
        public string version;
        public string compressed;
        public string compressed_path;

        public static List<Redistributable> GetRedistributables(int SoftwareID)
        {
            List<Redistributable> RedistributableList = new List<Redistributable>();

            //Get Required Redist ID for install.
            string QueryString = "SELECT tblRedistUsage.[redist_id],tblRedist.id,tblRedist.[name],tblRedist.[path],tblRedist.args,tblRedist.filecheck,tblRedist.[version],tblRedist.compressed,tblRedist.compressed_path FROM tblRedistUsage INNER JOIN tblRedist ON tblRedistUsage.redist_id=tblRedist.id WHERE tblRedistUsage.software_id = @softwareid ORDER BY tblRedistUsage.[install_order] ASC";
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                Redistributable tmpRedist = new Redistributable();
                tmpRedist.name = SQLOutput["name"].ToString();
                tmpRedist.path = SQLOutput["path"].ToString();
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
    }
}
