using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class FileCopyOperation
    {
        public FileInfoClass fileinfo = new FileInfoClass();
        public string destination;
        public bool verified = false;

        public static List<FileCopyOperation> GetFiles(int SoftwareID)
        {
            List<FileCopyOperation> FileCopyList = new List<FileCopyOperation>();

            string QueryString = "SELECT [id],[source],[destination],[filesize],[hash_md5] from tblFiles WHERE software_id = @softwareid ORDER BY filesize ASC";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
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

    }
}
