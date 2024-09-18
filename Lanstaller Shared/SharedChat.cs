using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading;

namespace LanstallerShared
{
    public class SharedChat
    {

        //Chat messages may be missing from client if timestamp is exactly the same.


        public class ChatMessage
        {
            public long id { get; set; }
            public string message { get; set; } //get;set; required for Json.
            public string sender { get; set; }
            public DateTime timestamp { get; set; }
        }
        
        public static int GetMessageCount(int lastId)
        {
            string QueryString = "SELECT COUNT([id]) from tblMessages WHERE [id] > @lid AND [timestamp] > DATEADD(day,-2,GETDATE())";
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);         

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("@lid", lastId);
            int count = 0;
            count = (int)SQLCmd.ExecuteScalar();           
            SQLConn.Close();

            return count;

        }
        

        public static List<ChatMessage> GetFullChat()
        {
            //Returns last hour of chat messages.
            //string QueryString = "SELECT TOP(50) [timestamp],[message],[sender] from tblMessages WHERE [timestamp] > DATEADD(HOUR, -1, GETDATE()) ORDER BY [timestamp] ASC";
            string QueryString = "SELECT TOP(35) [id],[timestamp],[message],[sender] from tblMessages WHERE [timestamp] > DATEADD(day,-2,GETDATE()) ORDER BY [id] DESC";
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SR = SQLCmd.ExecuteReader();
            
            List<ChatMessage> MsgLst = new List<ChatMessage>();
            while (SR.Read())
            {
                ChatMessage tmpMsg = new ChatMessage();
                tmpMsg.id = (long)SR["id"];
                tmpMsg.timestamp = (DateTime)SR["timestamp"];
                tmpMsg.message = SR["message"].ToString();
                tmpMsg.sender = SR["sender"].ToString();
                MsgLst.Add(tmpMsg);
            }
            SQLConn.Close();
            return MsgLst;
        }

        public static void SendMessage(string Message, string Sender)
        {
            SqlConnection SQLConn = new SqlConnection(LanstallerServer.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("INSERT INTO tblMessages (timestamp,message,sender) VALUES (GETDATE(),@message,@sender)", SQLConn);
            SQLCmd.Parameters.AddWithValue("@message", Message);
            SQLCmd.Parameters.AddWithValue("@sender", Sender);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }

        




    }
}
