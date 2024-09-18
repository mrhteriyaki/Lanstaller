using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class RegistryOperation
    {
        public int hkey;
        //1 = Local Machine.
        //2 = Current User.
        //3 = Users.

        public int regtype;
        //string = 1
        //binary = 3
        //dword = 4
        //expanded string = 2
        //multi string = 7
        //qword = 11

        public string subkey;
        public string value;
        public string data;

        public static void AddRegistry(int softwareid, int hkey, string subkey, string value, int regtype, string data)
        {
            string QueryString = "INSERT into tblRegistry ([hkey],[subkey],[value],[type],[data],[software_id]) VALUES (@hkey,@subkey,@value,@type,@data,@softwareid)";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@hkey", hkey);
            SQLCmd.Parameters.AddWithValue("@subkey", subkey);
            SQLCmd.Parameters.AddWithValue("@value", value);
            SQLCmd.Parameters.AddWithValue("@type", regtype);
            SQLCmd.Parameters.AddWithValue("@data", data);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();

        }

        public static List<RegistryOperation> GetRegistry(int SoftwareID)
        {
            List<RegistryOperation> RegistryList = new List<RegistryOperation>();


            string QueryString = "select [hkey],[subkey],[value],[type],[data] from [tblRegistry] WHERE software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                RegistryOperation tReg = new RegistryOperation();
                tReg.hkey = (int)SQLOutput[0]; //Hive Key.
                tReg.subkey = SQLOutput[1].ToString(); //Sub Key.
                tReg.value = SQLOutput[2].ToString();
                tReg.regtype = (int)SQLOutput[3];
                tReg.data = SQLOutput[4].ToString();

                RegistryList.Add(tReg);
            }
            SQLConn.Close();

            return RegistryList;
        }


    }
}
