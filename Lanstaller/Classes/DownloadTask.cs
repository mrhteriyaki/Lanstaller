using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static LanstallerShared.LanstallerServer;
using System.Windows.Forms;
using LanstallerShared;
using System.Security.Principal;
using System.Threading;

namespace Lanstaller.Classes
{
    public class DownloadTask
    {
        static string _authkey;

        public long downloadedbytes = 0;
        public long totalSize = 0;
        string _Source;
        string _Destination;
        

        public static void SetAuth(string authkey)
        {
            _authkey = authkey;
        }

        public DownloadTask(string Source, string Destination, long fileSize = 0) //Optional file size param allows for more efficient memory buffer allocation.
        {
            _Source = Source;
            _Destination = Destination;
            totalSize = fileSize;
        }


        public async Task DownloadAsync()
        {
            string SourceUri = _Source;
            HttpClient hClient = new HttpClient();
            hClient.DefaultRequestHeaders.Add("authorization", _authkey);
                       
            try
            {
                // Send a GET request to the specified URL
                HttpResponseMessage response = await hClient.GetAsync(SourceUri, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                // Get the total file size from the response headers, if available
                //long? totalBytes = response.Content.Headers.ContentLength;

                // Read the content and write it to the destination file
                //131072 = 128k
                //524288 - 512K.
                //4194304 = 4MB
                //16777216 = 16MB
                int chunkSize = 131072;
                if (totalSize < 131072)
                {
                    chunkSize = 8192;
                }


                Stream contentStream = await response.Content.ReadAsStreamAsync();
                Stream fileStream = new FileStream(_Destination, FileMode.Create, FileAccess.Write, FileShare.None, chunkSize, true);

                byte[] buffer = new byte[chunkSize];
                int bytesRead;
                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    downloadedbytes += bytesRead;
                    /* Report progress
                    if (totalBytes.HasValue)
                    {
                        //Console.WriteLine($"Downloaded {totalRead} of {totalBytes} bytes ({(totalRead * 100 / totalBytes.Value):0.00}%). {FCO.destination}");
                    }
                    else
                    {
                        //Console.WriteLine($"Downloaded {totalRead} bytes. {FCO.destination}");
                    }
                    */
                }
                fileStream.Close();
                //Console.WriteLine("File downloaded successfully. " + FCO.destination);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"File error: {e.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error downloading: " + SourceUri + " " + ex.Message);
            }
            hClient.Dispose();
        }


        public long GetDownloadedBytes()
        {
            return downloadedbytes;
        }

    }
}
