using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Lanstaller_Shared
{
    public class Security
    {
        public class Token
        {
            public int id { get; set; }
            public string Name { get; set; }
            public string token { get; set; }
        }

        public class Registration
        {
            public int id { get; set; }
            public string name { get; set; }
            public string regcode { get; set; }
            public DateTime expiry { get; set; }
        }

        public static bool CheckSecurityToken(string token)
        {
            //tblSecurityTokens
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT COUNT(token) FROM tblSecurityTokens WHERE token = @tkval", SQLConn);
            SQLCmd.Parameters.AddWithValue("tkval", token);

            int tokencount = 0;
            SQLConn.Open();
            tokencount = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();

            if (tokencount > 0)
            {
                return true;
            }
            return false;
        }

        public static List<Token> GetTokens()
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT id,name,token FROM tblSecurityTokens", SQLConn);

            SQLConn.Open();
            List<Token> tmpList = new List<Token>();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            while (SR.Read())
            {
                Token tmpS = new Token();
                tmpS.id = (int)SR["id"];
                tmpS.Name = SR["name"].ToString();
                tmpS.token = SR["token"].ToString();
                tmpList.Add(tmpS);
            }
            SQLConn.Close();
            return tmpList;
        }

        public static Token GetToken(int id)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT id,name,token FROM tblSecurityTokens where id = @tkid", SQLConn);
            SQLCmd.Parameters.AddWithValue("tkid", id);

            SQLConn.Open();
            Token tST = new Token();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            while (SR.Read())
            {
                tST.id = (int)SR["id"];
                tST.Name = SR["name"].ToString();
                tST.token = SR["token"].ToString();
            }
            SQLConn.Close();
            return tST;
        }


        public static int NewToken(string Name, string registration_code)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            //Validate registration code.
            SQLCmd.CommandText = "SELECT COUNT(id) FROM tblSecurityRegistration WHERE regcode = @rcode AND ([expiry] > GETDATE() OR [expiry] is null)";
            SQLCmd.Parameters.AddWithValue("rcode", registration_code);
            int valid_count = 0;
            SQLConn.Open();
            valid_count = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();
            if (valid_count != 1)
            {
                return 0;
            }
            
            //random length 60-120
            Random r = new Random();
            int rInt = r.Next(60, 120);


            //Generate random code.
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[rInt];
            var random = new Random();


            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var tokenstring = new String(stringChars);


            SQLCmd.CommandText = "INSERT INTO tblSecurityTokens (name,token,registration_date) OUTPUT INSERTED.id VALUES (@nval,@tkval,GETDATE())";
            SQLCmd.Parameters.AddWithValue("nval", Name);
            SQLCmd.Parameters.AddWithValue("tkval", tokenstring);

            SQLConn.Open();
            int id = 0;
            id = (int)SQLCmd.ExecuteScalar();

            SQLConn.Close();

            return id;
        }

        public static void DeleteToken(int id)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("DELETE FROM tblSecurityTokens WHERE id = @tokenid", SQLConn);
            SQLCmd.Parameters.AddWithValue("tokenid", id);

            SQLConn.Open();
            SQLCmd.ExecuteScalar();
            SQLConn.Close();
        }


        //Registration Code - for automatic registration of tokens.

        public static void NewRegistrationCode(string Name, string RegistrationCode, DateTime Expiry)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("INSERT INTO tblSecurityRegistration (name,regcode,expiry) VALUES (@rname, @rcode, @rexp)", SQLConn);
            SQLCmd.Parameters.AddWithValue("rname", Name);
            SQLCmd.Parameters.AddWithValue("rcode", RegistrationCode);
            SQLCmd.Parameters.AddWithValue("rexp", Expiry);

            SQLConn.Open();
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static void DeleteRegistrationCode(int reg_id)
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("DELETE FROM tblSecurityRegistration WHERE id = @rid", SQLConn);
            SQLCmd.Parameters.AddWithValue("rid", reg_id);
            SQLConn.Open();
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        public static List<Registration> GetRegTokens()
        {
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT id,name,regcode,expiry FROM tblSecurityRegistration", SQLConn);

            SQLConn.Open();
            List<Registration> tmpList = new List<Registration>();
            SqlDataReader SR = SQLCmd.ExecuteReader();
            while (SR.Read())
            {
                Registration tmpS = new Registration();
                tmpS.id = (int)SR["id"];
                tmpS.name = SR["name"].ToString();
                tmpS.regcode = SR["regcode"].ToString();
                if (SR["expiry"] != DBNull.Value)
                {
                    tmpS.expiry = (DateTime)SR["expiry"];
                }
                tmpList.Add(tmpS);
            }
            SQLConn.Close();
            return tmpList;
        }

    }
}
