using System;
using System.Collections.Generic;
using System.Text;

namespace Lanstaller_Shared.Models
{
    public class RegistryOperation
    {
        public int hkey;
        //1 = Local Machine.
        //2 = Current User.
        //3 = Users.

        public int regtype;
        //string = 1
        //binary = 3
        //dword = 4
        //expanded string = 2
        //multi string = 7
        //qword = 11

        public string subkey;
        public string value;
        public string data;

    }
}
