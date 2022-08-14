using Lanstaller_Shared;

namespace LanstallerAPI
{
    public class Authentication
    {
        public static bool CheckLogon(HttpRequest HR)
        {
            //Headers are case insensitive, check .Contains does not work correctly.
            bool authheader = false;
            foreach (string Header in HR.Headers.Keys)
            {
                if (Header.ToLower() == "authorization")
                {
                    authheader = true;
                }
            }

            if (authheader == false)
            {
                return false;
            }
            else if (HR.Headers["authorization"] == "")
            {
                return false;
            }
            else
            {
                //Check Token in DB.
                string token = HR.Headers["Authorization"];
                if (Security.CheckSecurityToken(token) == true)
                {
                    return true;
                }
            }
            return false;
        }


        //Returns Token ID from auth header.
        public static int GetTokenID(HttpRequest HR)
        {
            string token = HR.Headers["Authorization"];
            return Security.GetTokenID(token);
        }

    }
}
