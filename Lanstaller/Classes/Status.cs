using LanstallerShared;
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


        //Used for status label.
        string status = "Status: Ready";
        int stage = 0;

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

        public void SetPartialState(int index, long bytesDownloaded)
        {
            bytesCopied[index] = bytesDownloaded;
        }



        //Set Stages.
        public void SetStage(int Stage)
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
                status = "Applying Registry - " + SInfo.Name;
            }
            else if (stage == 2)
            {
                status = "Indexing - " + SInfo.Name;
            }
            else if (stage == 3)
            {
                status = "Copying Files - " + SInfo.Name;
            }
            else if (stage == 4)
            {
                status = "Verifying Files - " + SInfo.Name;
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

            if (stage == 3)
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
                "\nFile:" + GetCopyCount().ToString() + " / " + SInfo.file_count.ToString() +
                "\nProgress (GB): " + Math.Round(gbsize, 2).ToString() +
                " / " + Math.Round(InstallSizeGB, 2).ToString();
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
