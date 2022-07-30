
using Lanstaller_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Lanstaller.Classes.APIClient;

namespace Lanstaller
{
    internal class ChatClient
    {


        public static void SendMessageAPI(string Message, string Sender)
        {
            //Send request to API.
            WC.UploadData(APIServer + "chat/send", "POST", Encoding.UTF8.GetBytes(Message));

        }

        public static List<SharedChat.ChatMessage> GetMessagesAPI(DateTime fromtime)
        {
            return null;
        }

        public static List<SharedChat.ChatMessage> GetFullMessagesAPI()
        {
            return null;
        }



    }
}
