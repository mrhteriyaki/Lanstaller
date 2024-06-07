using System;
using System.Collections.Generic;
using System.Text;

namespace Lanstaller_Shared.Models
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

            if (String.IsNullOrEmpty(format))
            {
                return serial_value; //return serial if no format provided.
            }

            char[] keyChars = serial_value.ToCharArray();
            int keyIndex = 0;

            char[] formatChars = format.ToCharArray();

            string output_value = String.Empty;
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

            string output_value = String.Empty;

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
    }
}
