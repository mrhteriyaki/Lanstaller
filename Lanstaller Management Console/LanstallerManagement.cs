using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lanstaller_Shared;

namespace Lanstaller_Management_Console
{
    public class LanstallerManagement
    {
        //Functions only used by Management tools.


        //Rescan file size and update database (use of Pri.Longpath only on windows / .net framework).
        public static void RescanFileSize()
        {
            string SA = SoftwareClass.GetFileServer("smb").path;
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            SQLCmd.CommandText = "SELECT [id],[source] from tblFiles";
            SQLConn.Open();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            List<SoftwareClass.FileCopyOperation> FileList = new List<SoftwareClass.FileCopyOperation>();
            while (SR.Read())
            {
                SoftwareClass.FileCopyOperation tmpFCO = new SoftwareClass.FileCopyOperation();
                tmpFCO.fileinfo.id = (int)SR[0];
                tmpFCO.fileinfo.source = SR[1].ToString();
                Pri.LongPath.FileInfo FI = new Pri.LongPath.FileInfo(SA + "\\" + SR[1].ToString());
                tmpFCO.fileinfo.size = FI.Length;
                FileList.Add(tmpFCO);
            }

            SQLCmd.CommandText = "UPDATE tblFiles SET filesize = @filesize WHERE id = @fileid";
            foreach (SoftwareClass.FileCopyOperation FCO in FileList)
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
