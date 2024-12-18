﻿using LanstallerShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Windows.Forms;

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
            RemoveLocalInstall(SoftwareID);

            _records.Add(new LocalInstallRecord(SoftwareID, shortcutOperations));
            WriteListDB(_records);
        }

        private void WriteListDB(List<LocalInstallRecord> updateList)
        {
            string tmpfile = _dbpath + ".tmp";

            StreamWriter DBSW = new StreamWriter(tmpfile);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented, // This enables pretty printing
            };

            DBSW.Write(JsonConvert.SerializeObject(updateList, settings));
            DBSW.Close();

            if(File.Exists(_dbpath))
            {
                File.Delete(_dbpath);
            }
            File.Move(tmpfile, _dbpath);
        }

        public void RemoveLocalInstall(int SoftwareID)
        {
            bool updateRequired = false;
            List<LocalInstallRecord> newRecords = new List<LocalInstallRecord>();
            foreach (LocalInstallRecord localInst in _records)
            {
                if (localInst.SoftwareID == SoftwareID)
                {
                    updateRequired = true;
                }
                else
                {
                    newRecords.Add(localInst);
                }
            }
            if (updateRequired)
            {
                _records = newRecords;
                WriteListDB(newRecords);
            }
        }

    }
}
