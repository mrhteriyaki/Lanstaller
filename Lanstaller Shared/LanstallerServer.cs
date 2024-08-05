

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions; //Regex
using System.Diagnostics;

using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Data.SqlTypes;
using System.Threading;


//Shared library of functions for client and API (Server).


namespace LanstallerShared
{
    public class LanstallerServer
    {
        public static string ConnectionString;

        public static string GetSystemData(string setting)
        {
            SqlConnection SQLConn = new SqlConnection(ConnectionString);
            SqlCommand SQLCmd = new SqlCommand("SELECT [data] from tblSystem WHERE setting = @setval", SQLConn);
            SQLCmd.Parameters.AddWithValue("@setval", setting);
            SQLConn.Open();
            string data = SQLCmd.ExecuteScalar().ToString();
            SQLConn.Close();
            return data;
        }

    }
}
