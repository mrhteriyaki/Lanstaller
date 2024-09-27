using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanstallerShared
{
    public class ConflictProcess
    {
        string processName;
        string processDisplayName;

        public string GetName()
        {
            return processDisplayName;
        }

        public bool IsProcessRunning()
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        public static List<ConflictProcess> GetProcesses(int SoftwareId)
        {
            List<ConflictProcess> ProcList = new List<ConflictProcess>();
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;
            SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareId);
            SQLCmd.CommandText = @"
                        SELECT conflict_process_name,conflict_process_displayname FROM tblConflictProcess 
                        INNER JOIN tblConflictProcessLink 
                        ON tblConflictProcess.conflict_process_id = tblConflictProcessLink.conflict_process_id 
                        WHERE software_id = @softwareid";

            SQLConn.Open();
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            while (SQLOutput.Read())
            {
                ProcList.Add(new ConflictProcess
                {
                    processName = SQLOutput[0].ToString(),
                    processDisplayName = SQLOutput[1].ToString()
                });
            }
            SQLConn.Close();
            return ProcList;
        }
    }
}
