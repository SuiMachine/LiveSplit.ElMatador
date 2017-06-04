using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace LiveSplit.ElMatador
{
    class WrapperDownloader
    {
        private string path = "https://github.com/SuiMachine/LiveSplit.ElMatador/raw/master/Wrapper/Build/";
        private string[] listOfFiles = new string[] { "DSOUND.dll", "ElMatadorGraphWrapper.dll" };
        private string gameFolderPath { get; set; }

        public WrapperDownloader(string gameFolderPath)
        {
            this.gameFolderPath = gameFolderPath;

            if (downloadFiles())
                MessageBox.Show("Done.", "Download successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("There was an error downloading the file.", "Download failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }


        private bool downloadFiles()
        {
            WebClient wbClient = new WebClient();

            int numberOfFiles = listOfFiles.Length;

            string[,] tempLocations = new string[numberOfFiles, 2];
            for (int i = 0; i < numberOfFiles; i++)
            {
                tempLocations[i, 0] = listOfFiles[i];
                tempLocations[i, 1] = Path.GetTempFileName();
                //Console.WriteLine("Update: " + tempLocations[i, 0] + " -> " + tempLocations[i, 1]);
            }

            for (int i = 0; i < numberOfFiles; i++)
            {
                Uri tempUri;
                Uri.TryCreate(path + tempLocations[i, 0], UriKind.Absolute, out tempUri);

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

            for (int i = 0; i < numberOfFiles; i++)
            {
                File.Copy(tempLocations[i, 1], Path.Combine(gameFolderPath, tempLocations[i, 0]), true);
            }

            return true;
        }
    }
}
