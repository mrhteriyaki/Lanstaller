using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LanstallerShared
{
    public class FirewallRule
    {
        public string softwarename;
        public string exepath;
        public string rulename;
        public int protocol_value = 0;
        public int port_number = 0;

        //Only netsh usage, possible expanded future use.
        public bool enabled;
        public bool direction; //true = in.
        public string localip;
        public string remoteip;
        public int remoteport = 0;
        public bool action; //true = allow.

        public static void AddFirewallRule(string filepath, string rulename, int softwareid)
        {
            string QueryString = "INSERT into tblFirewallExceptions ([filepath],[software_id],[rulename]) VALUES (@filepath,@softwareid,@rulename)";

            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();

            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@filepath", filepath);
            SQLCmd.Parameters.AddWithValue("@softwareid", softwareid);
            if (string.IsNullOrEmpty(rulename))
            {
                SQLCmd.Parameters.AddWithValue("@rulename", DBNull.Value);
            }
            else
            {
                SQLCmd.Parameters.AddWithValue("@rulename", rulename);
            }

            SQLCmd.ExecuteNonQuery();

            SQLConn.Close();
        }


        public static List<FirewallRule> GetFirewallRules(int SoftwareID)
        {
            List<FirewallRule> FirewallRuleList = new List<FirewallRule>();

            //Software name used for Windows Firewall rule.
            string softwarename = SoftwareInfo.GetSoftwareName(SoftwareID);

            string QueryString = "select [filepath],[rulename],[proto_scope],[port_scope] from tblFirewallExceptions WHERE software_id = @softwareid";
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareID);
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                FirewallRule rule = new FirewallRule();
                rule.softwarename = softwarename;
                rule.exepath = SQLOutput[0].ToString();
                rule.rulename = SQLOutput[1].ToString();
                if (SQLOutput[2] != DBNull.Value)
                {
                    rule.protocol_value = (int)SQLOutput[2];
                }
                if (SQLOutput[3] != DBNull.Value)
                {
                    rule.port_number = (int)SQLOutput[3];
                }
                FirewallRuleList.Add(rule);
            }
            SQLConn.Close();
            return FirewallRuleList;
        }
    }

}
