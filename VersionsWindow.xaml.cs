using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;

namespace ModrusLauncher
{
    /// <summary>
    /// Lógica de interacción para VersionsWindow.xaml
    /// </summary>
    public partial class VersionsWindow : Window
    {
        public MainWindow mainWindow;

        public void UpdateVersions(List<string> versiones)
        {
            ListVersion.ItemsSource = versiones;
        }

        async public Task getVersions()
        {
            var path = new MinecraftPath();
            var launcher = new CMLauncher(path);
            MVersionCollection versions;
            List<string> versions_ = new List<string>();
            UpdateVersions(new List<string> {"Cargando..."});

            try
            {
                versions = await launcher.GetAllVersionsAsync();
            }
            catch (System.Net.WebException)
            {
                launcher.VersionLoader = new LocalVersionLoader(launcher.MinecraftPath);
                launcher.FileDownloader = null;
                versions = await launcher.GetAllVersionsAsync();
            }

            foreach (var version in versions)
            {
                versions_.Add(version.Name);
            }

            UpdateVersions(versions_);

        }
        public VersionsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            getVersions();
            this.mainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListVersion.SelectedItem != null && !(ListVersion.SelectedItem.ToString().Equals("Cargando...")))
            {
                string versionSelected = (ListVersion.SelectedItem as string);
                mainWindow.UpdateVersion(versionSelected);
                this.Close();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
