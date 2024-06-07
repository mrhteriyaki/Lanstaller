using System;
using System.Collections.Generic;
using System.Text;

namespace Lanstaller_Shared.Models
{
    public class FileCopyOperation
    {
        public FileInfoClass fileinfo = new FileInfoClass();
        public string destination;
        public bool verified = false;
    }
}
