using Lanstaller_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Windows.Forms;
using Lanstaller_Shared.Models;

namespace Lanstaller.Classes
{
    public class LocalDatabase
    {
        string _dbpath;
        List<LocalInstallRecord> _records;
        public LocalDatabase(string DBFilePath)
        {
            _dbpath = DBFilePath;
            _records = new List<LocalInstallRecord>();
            LoadSoftwareList();
        }

        public class LocalInstallRecord
        {
            public int SoftwareID;
            public List<ShortcutOperation> Shortcuts;

            public LocalInstallRecord(int softwareID, List<ShortcutOperation> shortcuts)
            {
                SoftwareID = softwareID;
                Shortcuts = shortcuts;
            }
        }

        public void LoadSoftwareList()
        {
            _records.Clear();
            if (File.Exists(_dbpath))
            {
                string dbfiledata = File.ReadAllText(_dbpath);
                if (!String.IsNullOrEmpty(dbfiledata))
                {
                    _records = JsonConvert.DeserializeObject<List<LocalInstallRecord>>(dbfiledata);
                }
            }

            //Remove any deleted software.
            List<int> removal_list = new List<int>();
            foreach(LocalInstallRecord record in _records)
            {
                bool software_removed = true;
                foreach(ShortcutOperation shortcut in record.Shortcuts)
                {
                    if (File.Exists(shortcut.filepath))
                    {
                        software_removed = false;
                    }
                    //Need to add check for Disk Removal.


                }
                if (software_removed)
                {
                    removal_list.Add(record.SoftwareID);
                }
            }

            foreach(int softwareid in removal_list)
            {
                RemoveLocalInstall(softwareid);
            }
            
        }

        public List<int> GetSoftwareIDs()
        {
            List<int> ids = new List<int>();
            foreach (var record in _records)
            {
                ids.Add(record.SoftwareID);
            }
            return ids;
        }

        public List<ShortcutOperation> GetShortcuts(int softwareID)
        {
            foreach (LocalInstallRecord record in _records)
            {
                if (record.SoftwareID == softwareID)
                {
                    return record.Shortcuts;
                }
            }
            return new List<ShortcutOperation>();
        }

        public void AddLocalInstall(int SoftwareID, List<ShortcutOperation> shortcutOperations)
        {
            _records.Add(new LocalInstallRecord(SoftwareID, shortcutOperations));
            WriteListDB(_records);
        }

        private void WriteListDB(List<LocalInstallRecord> updateList)
        {
            StreamWriter DBSW = new StreamWriter(_dbpath);
            DBSW.Write(JsonConvert.SerializeObject(updateList));
            DBSW.Close();
        }

        public void RemoveLocalInstall(int SoftwareID)
        {
            int index = 0;
            foreach (LocalInstallRecord localInst in _records)
            {
                if (localInst.SoftwareID == SoftwareID)
                {
                    _records.RemoveAt(index);
                    WriteListDB(_records);
                    break;
                }
                index++;
            }
        }

    }
}
