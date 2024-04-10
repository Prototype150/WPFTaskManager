using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media;

namespace DesktopTaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(e.Args[0], e.Args[1]);
            mainWindow.Show();
        }
    }
}
