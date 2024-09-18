using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class UserSerial
    {
        public int id;
        public string serial;
        public string used_timestamp;


        //Get available serial numbers.
        public static List<UserSerial> GetUserSerials(int SerialID)
        {
            string QueryString = "select [id],[serial_value],[serial_used] from [tblSerialsAvailable] WHERE serial_id = @serialid AND [serial_used] IS NULL";

            List<UserSerial> SerialList = new List<UserSerial>();
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@serialid", SerialID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                UserSerial tmpUserSerial = new UserSerial();
                tmpUserSerial.id = (int)SQLOutput[0];
                tmpUserSerial.serial = SQLOutput[1].ToString();
                tmpUserSerial.used_timestamp = SQLOutput[2].ToString();
                SerialList.Add(tmpUserSerial);
            }
            SQLConn.Close();
            return SerialList;
        }

        //Mark available serial as used.
        public static void SetAvailableSerial(int PoolSerialID)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("UPDATE [tblSerialsAvailable] SET serial_used = GETDATE() WHERE id = @sid", SQLConn);
            SQLCmd.Parameters.AddWithValue("@sid", PoolSerialID);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }


        //Add serial to pool.
        public static void AddAvailableSerial(int SerialID, string Serial)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("INSERT INTO [tblSerialsAvailable] ([serial_id],[serial_value]) VALUES (@sid,@sval)", SQLConn);
            SQLCmd.Parameters.AddWithValue("@sid", SerialID);
            SQLCmd.Parameters.AddWithValue("@sval", Serial);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        //Remove serial from pool.
        public static void DeleteAvailableSerial(int UserSerialID)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("DELETE FROM [tblSerialsAvailable] WHERE id = @usersid", SQLConn);
            SQLCmd.Parameters.AddWithValue("@usersid", UserSerialID);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }
    }
}
