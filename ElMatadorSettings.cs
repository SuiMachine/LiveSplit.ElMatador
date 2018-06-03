using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace LiveSplit.ElMatador
{
    public partial class ElMatadorSettings : UserControl
    {
        public string GamePath { get; set; }

        public ElMatadorSettings()
        {
            InitializeComponent();

            this.TB_Path.DataBindings.Add("Text", this, "GamePath", false, DataSourceUpdateMode.OnPropertyChanged);


            // defaults
            if(GamePath == null || GamePath == "")
            {
                //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 289280
                var registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 289280");
                if(registry != null)
                {
                    GamePath = (string)registry.GetValue("InstallLocation");
                }

            }
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "GamePath", this.GamePath));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            string temp = GetStringFromXML(settings, "GamePath", "");
            if(temp != "")
            {
                this.GamePath = temp;
            }

        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            return settings[setting] != null ?
                (Boolean.TryParse(settings[setting].InnerText, out bool val) ? val : default_)
                : default_;
        }

        static string GetStringFromXML(XmlNode settings, string setting, string default_ = "")
        {
            if (settings[setting] != null)
            {
                return settings[setting].InnerText != "" ? settings[setting].InnerText : default_;
            }
            else
                return default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        private void B_ReinstallWrapper_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(GamePath, "pc_matador.exe")))
            {
                WrapperDownloader temp = new WrapperDownloader(GamePath);
            }
            else
                MessageBox.Show("Specify a location of the game first!");
        }

        private void B_BrowsePath_Click(object sender, EventArgs e)
        {
            FileDialog fd = new OpenFileDialog
            {
                Filter = "pc_matador.exe|pc_matador.exe"
            };
            DialogResult result = fd.ShowDialog();
            
            if(result == DialogResult.OK)
            {
                DirectoryInfo folder = Directory.GetParent(fd.FileName);
                TB_Path.Text = folder.ToString();
            }
        }
    }
}
