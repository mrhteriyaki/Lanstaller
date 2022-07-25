using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace Lanstaller_Shared
{
    public class SharedChat
    {

        //Chat messages may be missing from client if timestamp is exactly the same.


        public class ChatMessage
        {
            public string message;
            public string sender;
            public DateTime timestamp;
        }

        public static List<ChatMessage> GetChat(DateTime FromTimestamp)
        {
            string QueryString = "SELECT [timestamp],[message],[sender] from tblMessages WHERE [timestamp] > @frmtm ORDER BY [timestamp] ASC";
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);         

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SQLCmd.Parameters.AddWithValue("frmtm", FromTimestamp);
            SqlDataReader SR = SQLCmd.ExecuteReader();
            List<ChatMessage> ChatMessages = new List<ChatMessage>();
            while (SR.Read())
            {
                ChatMessage tmpMsg = new ChatMessage();
                tmpMsg.timestamp = (DateTime)SR["timestamp"];
                tmpMsg.message = SR["message"].ToString();
                tmpMsg.sender = SR["sender"].ToString();
                ChatMessages.Add(tmpMsg);
            }
            SQLConn.Close();

            return ChatMessages;

        }

        public static List<ChatMessage> GetFullChat()
        {
            //Returns last hour of chat messages.
            string QueryString = "SELECT [timestamp],[message],[sender] from tblMessages WHERE [timestamp] > DATEADD(HOUR, -1, GETDATE()) ORDER BY [timestamp] ASC";
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);

            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand(QueryString, SQLConn);
            SqlDataReader SR = SQLCmd.ExecuteReader();
            
            List<ChatMessage> MsgLst = new List<ChatMessage>();
            while (SR.Read())
            {
                ChatMessage tmpMsg = new ChatMessage();
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
            SqlConnection SQLConn = new SqlConnection(SoftwareClass.ConnectionString);
            SQLConn.Open();
            SqlCommand SQLCmd = new SqlCommand("INSERT INTO tblMessages (timestamp,message,sender) VALUES (GETDATE(),@message,@sender)", SQLConn);
            SQLCmd.Parameters.AddWithValue("message", Message);
            SQLCmd.Parameters.AddWithValue("sender", Sender);
            SQLCmd.ExecuteNonQuery();
            SQLConn.Close();
        }




    }
}
