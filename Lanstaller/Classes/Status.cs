﻿using LanstallerShared;
using System;


namespace Lanstaller.Classes
{
    //Status Class - Provide Information on current installation progress.
    public class Status
    {
        public Status(SoftwareInfo Info)
        {
            SInfo = Info;
            copyStates = new bool[SInfo.file_count];
            bytesCopied = new long[SInfo.file_count];
            for (int i = 0; i < SInfo.file_count; i++)
            {
                copyStates[i] = false;
                bytesCopied[i] = 0;
            }
            InstallSizeGB = GetGBSize(SInfo.install_size);
        }

        SoftwareInfo SInfo;

        //Locks.
        readonly object _progresslock = new object();
        readonly object _statuslock = new object();

        //Progres of current install.
        bool[] copyStates;
        long[] bytesCopied;
        long InstalledBytes = 0;
        double InstallSizeGB;
        int VerificationIndex = 0;

        //Used for status label.
        string status = "Status: Ready";
        double stage = 0;

        public void ResetCopyState()
        {
            lock (_progresslock)
            {
                InstalledBytes = 0;
            }
        }


        public void SetCopyState(int index, long byteSize)
        {
            copyStates[index] = true;
            lock (_progresslock)
            {
                InstalledBytes += byteSize;
            }
        }

        public void SetVerificationState(int FileCount)
        {
            VerificationIndex = FileCount;
        }

        public void SetPartialState(int index, long bytesDownloaded)
        {
            bytesCopied[index] = bytesDownloaded;
        }
        


        //Set Stages.
        public void SetStage(double Stage)
        {
            stage = Stage;
            //-1 = Error
            //0 = Ready.
            //1 = Registry
            //2 = Indexing Files
            //3 = Copy Files
            //4 = Wait for verification
            //5 = Generate Shortcuts
            //6 = Windows Settings
            //7 = Preferences
            //8 = Redistributables
            //9 Complete

            if (stage == 1)
            {
                status = SInfo.Name + "\nApplying Registry";
            }
            else if (stage == 2.1)
            {
                status = SInfo.Name + "\nDownloading file list";
            }
            else if (stage == 2.2)
            {
                status = SInfo.Name + "\nBuilding directory list" ;
            }
            else if (stage == 2.3)
            {
                status = SInfo.Name + "\nUpdating file list";
            }
            else if (stage == 3.1)
            {
                status = SInfo.Name + "\nGenerating directories";
            }
            else if (stage == 3.2)
            {
                status = SInfo.Name + "\nGetting file server";
            }
            else if (stage == 3.3) //3.3 is matched up change of format in GetStatus()
            {
                status = SInfo.Name + "\nCopying Files";
            }
            else if (stage == 4)
            {
                status = SInfo.Name + "\nVerifying Files";
            }
            else if (stage == 5)
            {
                status = "Generating Shortcuts";
            }
            else if (stage == 6)
            {
                status = "Updating Windows Settings";
            }
            else if (stage == 7)
            {
                status = "Applying Preferences";
            }
            else if (stage == 8)
            {
                status = "Installing Redistributables";
            }
            else if (stage == 9)
            {
                status = "Install Complete: " + SInfo.Name;
            }
        }

        public void SetError(string message)
        {
            //Incomplete, handles failure messages.
            stage = -1;
            status = message;

        }

        public string GetStatus()
        {

            if (stage == 3.3)
            {
                long copiedBytes = InstalledBytes;
                for (long i = 0; i < SInfo.file_count; i++)
                {
                    if (!copyStates[i])
                    {
                        copiedBytes += bytesCopied[i];
                    }
                }

                double gbsize = GetGBSize(copiedBytes);

                status = "Installing: \n" + SInfo.Name +
                "\nFile: " + GetCopyCount().ToString() + " / " + SInfo.file_count.ToString() +
                "\nProgress (GB): " + Math.Round(gbsize, 2).ToString() +
                " / " + Math.Round(InstallSizeGB, 2).ToString();
            }
            else if (stage == 4)
            {
                status = SInfo.Name + "\nVerifying Files: " + VerificationIndex.ToString() + " / " + SInfo.file_count.ToString();
            }


            lock (_statuslock)
            {
                return status;
            }

        }


        long GetCopyCount()
        {
            long count = 0;
            foreach (bool cSt in copyStates)
            {
                if (cSt)
                {
                    count++;
                }
            }
            return count;
        }


        double GetGBSize(long bytes)
        {
            return (double)bytes / 1073741824;
        }


        public int GetProgressPercentage()
        {
            lock (_progresslock)
            {
                if (SInfo.install_size == 0 || InstalledBytes == 0 || stage == -1)
                {
                    return 0;
                }
                double perc = ((double)InstalledBytes / (double)SInfo.install_size) * 100;
                return Convert.ToInt32(perc);
            }
        }



    }
}
