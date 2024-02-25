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

namespace ModrusLauncher
{
    /// <summary>
    /// Lógica de interacción para SelectType.xaml
    /// </summary>
    public partial class SelectType : Window
    {
        MainWindow mainWindow;
        public SelectType(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VersionsWindow mc = new VersionsWindow(mainWindow);
            mc.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            InstallForge if_ = new InstallForge(mainWindow);
            if_.Show();
            this.Close();
        }
    }
}
