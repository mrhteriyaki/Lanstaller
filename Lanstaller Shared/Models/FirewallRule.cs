using System;
using System.Collections.Generic;
using System.Text;

namespace Lanstaller_Shared.Models
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
    }

}
