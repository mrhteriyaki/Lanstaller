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
        public static List<ConflictPort> GetPorts(int SoftwareId)
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
                PortList.Add(new ConflictPort
                {
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
            if (Protocol == PortType.TCP)
            {
                var tcpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
                foreach (var tcp in tcpListeners)
                {
                    if (tcp.Port == Port)
                    {
                        return true;
                    }
                }
            }
            else if (Protocol == PortType.UDP)
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

   


}

