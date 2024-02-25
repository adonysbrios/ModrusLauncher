using CmlLib.Core.Auth;
using CmlLib.Core.VersionLoader;
using CmlLib.Core;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CmlLib.Core.Version;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using CmlLib.Core.Installer.Forge;
using System.Windows.Markup;

namespace ModrusLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        public string version_;
        public string username_;
        public string serverIp_;
        public int serverPort_;
        public int MaxRamUsage;
        public int ScreenWidth;
        public int ScreenHeight;
        public string NewJavaPath_;
        public string percentage;
        public bool ForgeVersion;

        public void resetOpts()
        {
            NewJavaPath_ = "";
            serverPort_ = 2555;
            username_ = "Steve";
            ScreenHeight = 400;
            ScreenWidth = 700;
            MaxRamUsage = 1024;
            serverIp_ = "";
        }

        public void saveOpts()
        {
            List<object> opts = new List<object> {
                username_,
                NewJavaPath_,
                serverIp_,
                ScreenWidth,
                ScreenHeight,
                serverPort_,
                MaxRamUsage,
                version_,
                ForgeVersion
            };
            string config = "config.txt";

            // Convertir cada elemento a string y escribirlo como una línea en el archivo
            List<string> lines = new List<string>();
            foreach (var i in opts)
            {
                lines.Add(i.ToString());
            }
            File.WriteAllLines(config, lines);
        }

        public MainWindow()
        {
            InitializeComponent();
            version_ = "";
            VersionText.Text = version_;
            resetOpts();

            string config = "config.txt";


            if (File.Exists(config))
            {
                string[] lineas = File.ReadAllLines(config);

                /*foreach (string linea in lineas)
                {
                    MessageBox.Show(linea);
                }*/

                if (lineas.Length == 8)
                {
                    username_ = lineas[0];
                    NewJavaPath_ = lineas[1];
                    serverIp_ = lineas[2];
                    int.TryParse(lineas[3], out ScreenWidth);
                    int.TryParse(lineas[4], out ScreenHeight);
                    int.TryParse(lineas[5], out serverPort_);
                    int.TryParse(lineas[6], out MaxRamUsage);
                    version_ = lineas[7];
                    VersionText.Text = version_;
                    bool.TryParse(lineas[8],out ForgeVersion);
                }
            }
            else
            {
                File.WriteAllText(config, "");
            }

        }

        public void UpdateVersion(string data, bool forge=false)
        {
            version_=data;
            if (forge)
            {
                VersionText.Text = version_+"(Forge Install)";
                ForgeVersion = true;
            }
            else
            {
                VersionText.Text = version_;
                ForgeVersion = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SelectType vw = new SelectType(this);
            vw.Show();        
        }
        private void WaitClick(object sender, RoutedEventArgs e)
        {
            return;
        }

        public async Task RunMinecraft()
        {


            var path = new MinecraftPath();
            var launcher = new CMLauncher(path);
            if (version_==null)
            {
                MessageBox.Show("Select a version before play");
                return;
            }

            if(ForgeVersion)
            {
                PlayButton.Content = "Wait...";
                PlayButton.Click += WaitClick;

                launcher.FileChanged += (e) =>
                {
                    DownloadBarGrid.Visibility = Visibility.Visible;
                    ProgressDownload.Text = "[" + e.FileKind.ToString() + "]" + e.FileName + " " + e.ProgressedFileCount + "/" + e.TotalFileCount;
                    DownloadBar.Minimum = 0;
                    DownloadBar.Maximum = e.TotalFileCount;
                    DownloadBar.Value = e.ProgressedFileCount;
                };
                launcher.ProgressChanged += (s, e) =>
                {
                    percentage = e.ProgressPercentage.ToString();
                };

                //Initialize MForge
                var forge = new MForge(launcher);
                forge.FileChanged += (e) =>
                {
                    DownloadBarGrid.Visibility = Visibility.Visible;
                    ProgressDownload.Text = "[" + e.FileKind.ToString() + "]" + e.FileName + " " + e.ProgressedFileCount + "/" + e.TotalFileCount;
                    DownloadBar.Minimum = 0;
                    DownloadBar.Maximum = e.TotalFileCount;
                    DownloadBar.Value = e.ProgressedFileCount;
                };
                forge.ProgressChanged += (s, e) =>
                {
                    percentage = e.ProgressPercentage.ToString();
                };
                forge.InstallerOutput += (s, e) => Console.WriteLine(e);
                var versionName = await forge.Install("1.20.1");

                version_ = versionName;
                VersionText.Text = version_;

                Process process = await launcher.CreateProcessAsync(versionName, new MLaunchOption
                {
                    Session = MSession.CreateOfflineSession(username_),
                    JavaPath = NewJavaPath_,
                    ScreenWidth = ScreenWidth,
                    ScreenHeight = ScreenHeight,
                    ServerIp = serverIp_,
                    ServerPort = serverPort_,
                    MaximumRamMb = MaxRamUsage,

                });
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) =>
                {
                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show("An error has occurred while launching the game, please make sure you have all the files downloaded before launching it in offline mode");

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                    else if (process.ExitCode == 0) { }
                    {

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                };
                process.Start();
                PlayButton.Content = "Play";
                PlayButton.Click += Button_Click_2;
                saveOpts();
                this.Hide();





            }

            PlayButton.Content = "Wait...";
            PlayButton.Click += WaitClick;
            MVersionCollection versions;
            launcher.FileChanged += (e) =>
            {
                DownloadBarGrid.Visibility = Visibility.Visible;
                ProgressDownload.Text = "["+ e.FileKind.ToString() + "]" + e.FileName + " " + e.ProgressedFileCount + "/" + e.TotalFileCount;
                DownloadBar.Minimum = 0;
                DownloadBar.Maximum = e.TotalFileCount;
                DownloadBar.Value = e.ProgressedFileCount;
            };
            launcher.ProgressChanged += (s, e) =>
            {
                percentage = e.ProgressPercentage.ToString();
            };

            try { 
            try
            {
                Process process = await launcher.CreateProcessAsync(version_, new MLaunchOption
                {
                    Session = MSession.CreateOfflineSession(username_),
                    JavaPath = NewJavaPath_,
                    ScreenWidth = ScreenWidth,
                    ScreenHeight = ScreenHeight,
                    ServerIp = serverIp_,
                    ServerPort = serverPort_,
                    MaximumRamMb = MaxRamUsage,

                });
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) =>
                {
                    if (process.ExitCode != 0)
                    {
                        MessageBox.Show("An error has occurred while launching the game, please make sure you have all the files downloaded before launching it in offline mode");

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                    else if (process.ExitCode == 0) { }
                    {

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                };
                process.Start();
                PlayButton.Content = "Play";
                PlayButton.Click += Button_Click_2;
                saveOpts();
                this.Hide();
            }
            catch (System.Net.WebException)
            {
                launcher.VersionLoader = new LocalVersionLoader(launcher.MinecraftPath);
                launcher.FileDownloader = null;


                Process process = await launcher.CreateProcessAsync(version_, new MLaunchOption()
                {
                    Session = MSession.CreateOfflineSession(username_),
                    JavaPath = NewJavaPath_,
                    ScreenWidth = ScreenWidth,
                    ScreenHeight = ScreenHeight,
                    ServerIp = serverIp_,
                    ServerPort = serverPort_,
                    MaximumRamMb = MaxRamUsage,

                }, checkAndDownload: false);
                process.EnableRaisingEvents = true;
                process.Exited += (sender, e) =>
                {
                    if(process.ExitCode != 0) {
                        MessageBox.Show("An error has occurred while launching the game, please make sure you have all the files downloaded before launching it in offline mode");

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                    else if(process.ExitCode == 0) { }
                    {

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            //MainWindow mainWindow = new MainWindow(); // Debes asegurarte de tener la instancia correcta de tu ventana principal
                            this.Show();
                        });

                    }
                };
                process.Start();
                PlayButton.Content = "Play";
                PlayButton.Click += Button_Click_2;
                saveOpts();
                this.Hide();
            }
            } catch (KeyNotFoundException)
            {
                MessageBox.Show("An error has occurred while launching the game, please make sure you have all the files downloaded before launching it in offline mode");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            RunMinecraft();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            saveOpts();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings(
                this, 
                username_,
                serverIp_,
                serverPort_,
                MaxRamUsage,
                ScreenWidth,
                ScreenHeight,
                NewJavaPath_
            );
            settings.Show();
        }
    }
}