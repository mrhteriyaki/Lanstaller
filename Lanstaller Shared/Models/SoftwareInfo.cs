using System;
using System.Collections.Generic;
using System.Text;

namespace Lanstaller_Shared.Models
{
    public class SoftwareInfo
    {
        public int id;
        public string Name;
        public int file_count;
        public int registry_count;
        public int shortcut_count;
        public int firewall_count;
        public int preference_count;
        public int redist_count;
        public long install_size;
        public string image_small; //Icon Image for List.
    }
}
