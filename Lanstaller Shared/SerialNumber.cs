using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lanstaller_Shared
{
    public class SerialNumber
    {
        public int serialid;
        public string name;
        public int instancenumber;
        public string serialnumber;
        public string regKey; //Registry Key
        public string regVal; //Registry Value Name
        public int softwareid;
        public string format;

        //Filter symbols from serial key input boxes (management and client)
        public static string FilterSerial(string serial_value)
        {
            return serial_value.Replace("-", "").Replace(" ", "");
        }

        public string FormatSerial(string serial_value)
        {
            //Converts serial into formatted version for registry.
            //Eg add hypen to *****-*****-*****

            if (string.IsNullOrEmpty(format))
            {
                return serial_value; //return serial if no format provided.
            }

            char[] keyChars = serial_value.ToCharArray();
            int keyIndex = 0;

            char[] formatChars = format.ToCharArray();

            string output_value = string.Empty;
            for (int i = 0; i < formatChars.Length; i++)
            {
                if (formatChars[i] == '*') //Regular Character
                {
                    output_value += keyChars[keyIndex];
                    keyIndex++;
                }
                else //Insert static characters from format.
                {
                    output_value += formatChars[i];
                }
            }
            return output_value;
        }

        public string UnformatSerial(string serial_value)
        {
            //Check if serial length long enough for unformat.
            if (format.Length == 0 || serial_value.Length != format.Length)
            {
                return serial_value;
            }

            char[] keyChars = serial_value.ToCharArray();
            char[] formatChars = format.ToCharArray();

            string output_value = string.Empty;

            for (int i = 0; i < formatChars.Length; i++)
            {
                if (formatChars[i] == '*')
                {
                    output_value = output_value + keyChars[i];
                }
            }
            return output_value;
        }

        public int GetFormattedLength() //Returns how many regular characters to expect in Format.
        {
            int count = 0;
            foreach (char c in format)
            {
                if (c == '*')
                {
                    count++;
                }
            }
            return count;
        }

        public static void AddSerial(string name, int instancenumber, int softwareid, string regKey, string regVal, string SerialFormat)
        {
            //Check no existing serial present with same software id and instance number.
            string QueryString = "SELECT COUNT(instance) from tblSerials where [instance] = @instancenumb and [software_id] = @softwareid";
            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            int counter = (int)SQLCmd.ExecuteScalar();
            SQLConn.Close();

            if (counter != 0)
            {
                //MessageBox.Show("A serial with the same instancen number already exists for this software.");
                throw new Exception("A serial with the same instancen number already exists for this software.");
                //return;
            }


            QueryString = "INSERT into tblSerials ([name],[instance],[regKey],[regVal],[software_id],[format]) VALUES (@name,@instancenumb,@regKey,@regVal,@softwareid,@serformat)";

            SQLConn.Open();
            SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@name", name);
            SQLCmd.Parameters.AddWithValue("@instancenumb", instancenumber);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            SQLCmd.Parameters.AddWithValue("@regKey", regKey);
            SQLCmd.Parameters.AddWithValue("@regVal", regVal);
            SQLCmd.Parameters.AddWithValue("@serformat", SerialFormat);
            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }

        public static void AddUserSerial(int SerialID, string Serial)
        {
            //Add serial number 
            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLConn.Open();
            SQLCmd.Parameters.AddWithValue("@sid", SerialID);
            SQLCmd.CommandText = "INSERT INTO tblSerialsAvailable (serial_id,serial_value) VALUES (@sid,@sval)";
            SQLCmd.Parameters.AddWithValue("@sval", Serial);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        //Gets Serial Requirements for Queued Installs.
        public static List<SerialNumber> GetSerials(int SoftwareID)
        {
            List<SerialNumber> SerialList = new List<SerialNumber>();
            SqlCommand SQLCmd = new SqlCommand();
            SqlConnection SQLConn = new SqlConnection(LanstallerShared.ConnectionString);
            SQLCmd.Connection = SQLConn;

            SQLConn.Open();
            SQLCmd.CommandText = "select [id],[name],[instance],[regKey],[regVal],[format] from [tblSerials] WHERE software_id = @softwareid ORDER BY INSTANCE ASC";
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                SerialNumber tSerial = new SerialNumber();
                tSerial.softwareid = SoftwareID;
                tSerial.serialid = (int)SQLOutput[0];
                tSerial.name = SQLOutput[1].ToString();
                tSerial.instancenumber = (int)SQLOutput[2];
                tSerial.regKey = SQLOutput[3].ToString();
                tSerial.regVal = SQLOutput[4].ToString();
                tSerial.format = SQLOutput[5].ToString();
                SerialList.Add(tSerial);
            }
            SQLConn.Close();
            return SerialList;

        }

    }
}
