using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lanstaller_Shared
{
    public class CompatabilityMode
    {
        public string filename;
        public int compat_type;

        public static List<CompatabilityMode> GetCompatibility(int swid)
        {
            //Set compatibility flags in registry.
            List<CompatabilityMode> CompatList = new List<CompatabilityMode>();

            string QueryString = "select filename,compat_type from tblCompatibility where software_id = @softwareid";

            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", swid);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                CompatabilityMode tCompat = new CompatabilityMode();
                tCompat.filename = SQLOutput[0].ToString();
                tCompat.compat_type = (int)SQLOutput[1];
                CompatList.Add(tCompat);
            }
            SQLConn.Close();

            return CompatList;
        }
    }


}
