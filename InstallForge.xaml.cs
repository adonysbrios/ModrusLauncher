using CmlLib.Core.Installer.Forge.Versions;
using CmlLib.Core.Version;
using CmlLib.Core.VersionLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ModrusLauncher
{
    /// <summary>
    /// Lógica de interacción para InstallForge.xaml
    /// </summary>
    public partial class InstallForge : Window
    {
        MainWindow mainWindow;
        public InstallForge(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public async Task getForge()
        {
            try {
                FForge.Content = "Wait...";
                string version = versionBox.Text;
                var versionLoader = new ForgeVersionLoader(new HttpClient());
                var versions = await versionLoader.GetForgeVersions(version);
                var recommendedVersion = versions.First(v => v.IsRecommendedVersion);
                MessageBox.Show(recommendedVersion.MinecraftVersionName);
                mainWindow.UpdateVersion(data: recommendedVersion.MinecraftVersionName, forge:true);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                FForge.Content = "Find Forge";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getForge();
        }
    }
}
