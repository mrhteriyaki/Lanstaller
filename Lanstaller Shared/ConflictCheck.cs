using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace LanstallerShared
{
    public class ConflictCheck
    {
        public List<ConflictPort> ConflictPorts = new List<ConflictPort>();
        public List<ConflictProcess> ConflictProcesses = new List<ConflictProcess>();
        public class ConflictPort
        {
            int Port;
            PortType Protocol; //1 = TCP, 2 = UDP

            public enum PortType
            {
                TCP = 1,
                UDP = 2
            }

            public int GetPortNumber()
            {
                return Port;
            }


            internal static List<ConflictPort> GetPorts(int SoftwareId)
            {
                List<ConflictPort> PortList = new List<ConflictPort>();
                SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
                SqlCommand SQLCmd = new SqlCommand();
                SQLCmd.Connection = SQLConn;
                SQLCmd.Parameters.AddWithValue("@softwareid", SoftwareId);
                SQLCmd.CommandText = "SELECT conflict_port_number,conflict_port_type FROM tblConflictPorts where software_id = @softwareid";
                SQLConn.Open();
                SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
                while (SQLOutput.Read())
                {
                    PortList.Add(new ConflictPort { 
                        Port = (int)SQLOutput[0],
                        Protocol = (PortType)SQLOutput[1]
                    });
                }
                SQLConn.Close();
                return PortList;
            }


            public bool CheckPortUsage()
            {
                // Get the TCP listening ports
                if(Protocol == PortType.TCP)
                {
                    var tcpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
                    foreach (var tcp in tcpListeners)
                    {
                        if (tcp.Port == Port)
                        {
                            return true;
                        }
                    }
                }else if(Protocol == PortType.UDP)
                {
                    var udpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners();
                    foreach (var udp in udpListeners)
                    {
                        if (udp.Port == Port)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

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

            internal static List<ConflictProcess> GetProcesses(int SoftwareId)
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


        public static ConflictCheck GetConflicts(int SoftwareId)
        {
            ConflictCheck Conflicts = new ConflictCheck();
            Conflicts.ConflictProcesses = ConflictProcess.GetProcesses(SoftwareId);
            Conflicts.ConflictPorts = ConflictPort.GetPorts(SoftwareId);
            return Conflicts;
        }

    }
}
