using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.ElMatador
{
    class WrapperDownloader
    {
        private static bool downloadFiles(string sourceLocation, out string[,] tempLocations)
        {
            string[] listOfFiles = new string[] { "Wrapper/Build/DSOUND.dll", "Wrapper/Build/ElMatadorGraphWrapper.dll" };
            WebClient wbClient = new WebClient();
            wbClient.DownloadFileCompleted += WbClient_DownloadFileCompleted;

            int numberOfFiles = listOfFiles.Count;
            tempLocations = new string[numberOfFiles, 2];
            for (int i = 0; i < numberOfFiles; i++)
            {
                tempLocations[i, 0] = listOfFiles[i];
                tempLocations[i, 1] = Path.GetTempFileName();
                Console.WriteLine("Update: " + tempLocations[i, 0] + " -> " + tempLocations[i, 1]);
            }

            for (int i = 0; i < numberOfFiles; i++)
            {
                Uri tempUri;
                Uri.TryCreate(sourceLocation + tempLocations[i, 0], UriKind.Absolute, out tempUri);

                try { wbClient.DownloadFile(tempUri, tempLocations[i, 1]); }
                catch { return false; }
            }

            for (int i = 0; i < numberOfFiles; i++)
            {
                FileInfo file = new FileInfo(tempLocations[i, 1]);
                if (file.Length == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
