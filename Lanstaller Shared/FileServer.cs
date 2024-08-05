using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace LanstallerShared
{
    public class FileServer
    {
        public string path;
        public int protocol = 0; //1 = Web, 2 = SMB
        public int priority = 0;

        public static List<FileServer> GetFileServer(bool smbOnly = false) //web or smb for type.
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SqlCommand SQLCmd = new SqlCommand();
            SQLCmd.Connection = SQLConn;

            if (smbOnly)
            {
                SQLCmd.CommandText = "SELECT [address],[protocol],[priority] FROM [tblServers] WHERE [protocol] = 2 ORDER BY [Priority] ASC";
            }
            else
            {
                SQLCmd.CommandText = "SELECT [address],[protocol],[priority] FROM [tblServers] ORDER BY [Priority] ASC";
            }


            SQLConn.Open();
            SqlDataReader SQLOutput = SQLCmd.ExecuteReader();
            List<FileServer> tmpList = new List<FileServer>();
            while (SQLOutput.Read())
            {
                tmpList.Add(new FileServer()
                {
                    path = SQLOutput[0].ToString(),
                    protocol = (int)SQLOutput[1],
                    priority = (int)SQLOutput[2]
                });
            }
            SQLConn.Close();

            
            return tmpList;
        }


        public bool IsWAN()
        {
            bool isWAN = true;
            if (this.protocol == 1)
            {
                Uri sUri = new Uri(this.path);
                
                IPAddress[] addresses = Dns.GetHostAddresses(sUri.Host);
                foreach (IPAddress address in addresses)
                {
                    if (IsInLocalSubnet(address))
                    {
                        isWAN = false;
                    }
                }
            }
            else
            {
                //all smb servers assumed lan.
                isWAN = false;
            }
            return isWAN;
        }

        private static bool IsInLocalSubnet(IPAddress address)
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ipInfo in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        IPAddress localAddress = ipInfo.Address;
                        IPAddress subnetMask = ipInfo.IPv4Mask;

                        if (subnetMask != null)
                        {
                            if (IsInSameSubnet(localAddress, address, subnetMask))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        static bool IsInSameSubnet(IPAddress address1, IPAddress address2, IPAddress subnetMask)
        {
            byte[] ipBytes1 = address1.GetAddressBytes();
            byte[] ipBytes2 = address2.GetAddressBytes();
            byte[] maskBytes = subnetMask.GetAddressBytes();

            if (ipBytes1.Length != ipBytes2.Length || ipBytes1.Length != maskBytes.Length)
            {
                //throw new ArgumentException("Lengths of IP address and subnet mask do not match.");
                return false; //mismatch check of ipv4 and ipv6 address.
            }

            for (int i = 0; i < ipBytes1.Length; i++)
            {
                if ((ipBytes1[i] & maskBytes[i]) != (ipBytes2[i] & maskBytes[i]))
                {
                    return false;
                }
            }
            return true;
        }


    }

}
