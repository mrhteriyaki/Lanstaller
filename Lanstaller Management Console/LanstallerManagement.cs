using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanstallerShared;
using System.IO;

namespace Lanstaller_Management_Console
{
    public class LanstallerManagement
    {
        //Functions only used by Management tools.


        //Rescan file size and update database (use of Pri.Longpath only on windows / .net framework).
        public static void RescanFileSize(int software_id)
        {
            string SA = FileServer.GetFileServer().FirstOrDefault(s => s.protocol == 2).path;

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLCmd.CommandText = "SELECT [id],[source] from tblFiles WHERE software_id = @swid";
            SQLCmd.Parameters.AddWithValue("@swid", software_id);
            SQLConn.Open();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            List<FileCopyOperation> FileList = new List<FileCopyOperation>();
            while (SR.Read())
            {
                FileCopyOperation tmpFCO = new FileCopyOperation();
                tmpFCO.fileinfo.id = (int)SR[0];
                tmpFCO.fileinfo.source = SR[1].ToString();
                FileInfo FI = new FileInfo(SA + "\\" + SR[1].ToString());
                tmpFCO.fileinfo.size = FI.Length;
                FileList.Add(tmpFCO);
            }
            SR.Close();
            SQLCmd.CommandText = "UPDATE tblFiles SET filesize = @filesize WHERE id = @fileid";
            foreach (FileCopyOperation FCO in FileList)
            {
                SQLCmd.Parameters.AddWithValue("filesize", FCO.fileinfo.size);
                SQLCmd.Parameters.AddWithValue("fileid", FCO.fileinfo.id);
                SQLCmd.ExecuteNonQuery();
                SQLCmd.Parameters.Clear();
            }
            SQLConn.Close();
        }

      
    }
}
