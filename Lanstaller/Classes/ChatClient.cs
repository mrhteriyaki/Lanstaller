
using Lanstaller_Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Lanstaller_Shared.SharedChat;

namespace Lanstaller
{
    internal class ChatClient
    {
        static WebClient ChatWC = new System.Net.WebClient();
        static string _authkey;
        static readonly object _ChatWClock = new object();
        public static void InitChatWC(string authkey)
        {
            _authkey = authkey;
            ChatWC.Headers.Clear();
            ChatWC.Headers.Add("authorization", _authkey);
        }

        public static string ChatServer;
        public static void SendMessageAPI(ChatMessage Msg)
        {
            //Send request to API.
            string MsgData = Newtonsoft.Json.JsonConvert.SerializeObject(Msg);
            //add json content type header.
            lock(_ChatWClock)
            {
                ChatWC.Headers.Add("Content-Type", "application/json");
                ChatWC.UploadData(ChatServer + "chat/send", "POST", Encoding.UTF8.GetBytes(MsgData));
            }
            
        }

        public static ChatMessage[] GetMessagesAPI()
        {
            lock (_ChatWClock)
            {
                string JsonData = ChatWC.DownloadString(ChatServer + "chat");
                ChatMessage[] arr = JArray.Parse(JsonData).ToObject<ChatMessage[]>();
                Array.Reverse(arr);
                return arr;
            }
        }

        public static int GetMessageCount(long lastid)
        {
            lock (_ChatWClock)
            {
                //Send request to API.
                //ChatWC.UploadData(ChatServer + "chat/send", "POST", Encoding.UTF8.GetBytes(lastid));
                string countstr = ChatWC.DownloadString(ChatServer + "chat/check?lastid=" + lastid);
                return int.Parse(countstr);
            }
        }

    }
}
