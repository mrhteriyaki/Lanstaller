using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace LanstallerShared
{
    public class FileInfoClass
    {
        public int id;
        public long size;
        public string hash;
        public string source;

        public static void AddFile(string source, string destination, long filesize, int softwareid)
        {
            string QueryString = "INSERT into tblFiles ([source],[destination],[filesize],[software_id]) VALUES (@sourcefile,@destinationfile,@filesize,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@sourcefile", source);
            SQLCmd.Parameters.AddWithValue("@destinationfile", destination);
            SQLCmd.Parameters.AddWithValue("@filesize", filesize);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static void AddDirectory(string directorypath, int softwareid)
        {
            string QueryString = "INSERT into tblDirectories ([path],[software_id]) VALUES (@dirpath,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@dirpath", directorypath);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static int GetUnhashedFileCount()
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("SELECT COUNT(id) from tblFiles where hash_md5 is null", SQLConn);
            int count = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();
            return count;
        }


        //Scan files and save hash values to database.
        public static void RescanFileHashes(bool fullrescan, int software_id)
        {
            FileServer SA = FileServer.GetFileServer(true)[0];
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            string QueryString = "SELECT [id],[source],[hash_md5] from tblFiles ";
            if (software_id != 0)
            {
                QueryString += "WHERE software_id = @swid";
            }

            if (fullrescan == false)
            {
                if (software_id == 0)
                {
                    QueryString += " WHERE [hash_md5] is null";
                }
                else
                {
                    QueryString += " AND [hash_md5] is null";
                }
            }
            else
            {
                //Full Rescan, clear existing hashes for progress check.
                QueryString += "; UPDATE tblFiles SET [hash_md5] = NULL WHERE software_id = @swid";
            }


            SQLCmd.CommandText = QueryString;
            SQLConn.Open();
            if (software_id > 0)
            {
                SQLCmd.Parameters.AddWithValue("@swid", software_id);
            }

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
            SQLCmd.CommandText = QueryString;

            foreach (FileInfoClass FH in FileHashList)
            {
                string filePath = SA.path + "\\" + FH.source;
                //Console.Write(filePath);
                FH.hash = CalculateMD5(filePath);
                SQLCmd.Parameters.Clear();
                SQLCmd.Parameters.AddWithValue("@fileid", FH.id);
                SQLCmd.Parameters.AddWithValue("@hash", FH.hash);

                SQLConn.Open();
                SQLCmd.ExecuteNonQuery();
                SQLConn.Close();
            }
        }

        public static string CalculateMD5(string filename)
        {
            int filelockerror = 0; //loop on file lock errors.
            while (filelockerror < 10) //Max 10 attempts.
            {
                try
                {
                    MD5 md5 = MD5.Create();
                    FileStream stream = File.OpenRead(filename);
                    byte[] hash = md5.ComputeHash(stream);
                    stream.Close();
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
                catch(FileNotFoundException fnEx)
                {
                    //File missing - typical result of AV deletion.
                    Logging.LogToFile("File hash failed: " + filename + "\nError:" + fnEx.Message);
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    Logging.LogToFile("File hash failed: " + filename + "\nError:" + ex.Message);

                    if(ex.Message.Contains("virus"))
                    {
                        //MessageBox.Show("Antivirus has blocked verification of file. File:" + filename);
                        return string.Empty;
                    }
                    Thread.Sleep(500); //Wait before looping.
                }
                filelockerror++;
            }
            return string.Empty;

        }
    }
}
