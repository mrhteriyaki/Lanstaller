
using LanstallerShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static LanstallerShared.SharedChat;

namespace Lanstaller
{
    internal class ChatClient
    {
        static string _authkey;
        public static void SetAuth(string authkey)
        {
            _authkey = authkey;
        }

        static WebClient GetChatWC()
        {
            WebClient wc = new WebClient();
            wc.Headers.Clear();
            wc.Headers.Add("authorization", _authkey);
            return wc;
        }

        public static string ChatServer;
        public static void SendMessageAPI(ChatMessage Msg)
        {
            //Send request to API.
            string MsgData = Newtonsoft.Json.JsonConvert.SerializeObject(Msg);
            //add json content type header.

            WebClient ChatWC = GetChatWC();

            ChatWC.Headers.Add("Content-Type", "application/json");
            ChatWC.UploadData(ChatServer + "chat/send", "POST", Encoding.UTF8.GetBytes(MsgData));


        }

        public static ChatMessage[] GetMessagesAPI()
        {
            WebClient ChatWC = GetChatWC();
            string JsonData = ChatWC.DownloadString(ChatServer + "chat");
            ChatMessage[] arr = JArray.Parse(JsonData).ToObject<ChatMessage[]>();
            Array.Reverse(arr);
            return arr;
        }

        public static int GetMessageCount(long lastid)
        {
            //Send request to API.
            //ChatWC.UploadData(ChatServer + "chat/send", "POST", Encoding.UTF8.GetBytes(lastid));
            try
            {
                WebClient ChatWC = GetChatWC();
                string countstr = ChatWC.DownloadString(ChatServer + "chat/check?lastid=" + lastid);
                return int.Parse(countstr);
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }

}

