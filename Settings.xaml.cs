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
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public MainWindow mainWindow;
        public Settings(MainWindow mainWindow,
                string username_,
                string serverIp_,
                int serverPort_,
                int MaxRamUsage,
                int ScreenWidth,
                int ScreenHeight,
                string NewJavaPath_)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            JPath.Text = NewJavaPath_;
            Shost.Text = serverIp_;
            Sport.Text = serverPort_.ToString();
            SWidth.Text = ScreenWidth.ToString();
            SHeight.Text = ScreenHeight.ToString();
            MaxRam.Text = (MaxRamUsage/1024).ToString();
            Username.Text = username_;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.resetOpts();
            JPath.Text = "";
            Shost.Text = "";
            Sport.Text = 2555.ToString();
            SWidth.Text = 700.ToString();
            SHeight.Text = 400.ToString();
            MaxRam.Text = (1024/1024).ToString();
            Username.Text = "Steve";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            mainWindow.username_=Username.Text;
            mainWindow.NewJavaPath_=JPath.Text;
            mainWindow.serverIp_=Shost.Text;
            int.TryParse(SWidth.Text,out mainWindow.ScreenWidth);
            int.TryParse(SHeight.Text, out mainWindow.ScreenHeight);
            int.TryParse(Sport.Text, out mainWindow.serverPort_);
            int mxram;
            int.TryParse(MaxRam.Text, out mxram);
            mainWindow.MaxRamUsage = mxram * 1024;
            mainWindow.saveOpts();
        }
    }
}
