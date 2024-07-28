using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lanstaller_Shared
{
    public class FileServer
    {
        public string path;
        public int protocol = 0; //1 = Web, 2 = SMB
        public int priority = 0;

        public static List<FileServer> GetFileServer(bool smbOnly = false) //web or smb for type.
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            if (smbOnly)
            {
                SQLCmd.CommandText = "SELECT [address],[protocol],[priority] FROM [tblServers] WHERE [protocol] = 2 ORDER BY [Priority] ASC";
            }
            else
            {
                SQLCmd.CommandText = "SELECT [address],[protocol],[priority] FROM [tblServers] ORDER BY [Priority] ASC";
            }


            SQLConn.Open();
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            List<FileServer> tmpList = new List<FileServer>();
            while (SQLOutput.Read())
            {
                tmpList.Add(new FileServer()
                {
                    path = SQLOutput[0].ToString(),
                    protocol = (int)SQLOutput[1],
                    priority = (int)SQLOutput[2]
                });
            }
            SQLConn.Close();
            return tmpList;
        }

    }
}
