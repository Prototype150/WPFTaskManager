using DesktopTaskManager.Services;
using DesktopTaskManager.ViewModel;
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

namespace DesktopTaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(string serverIp, string serverPort)
        {
            string _connectionString = $"https://{serverIp}:{serverPort}";
            DataContext = new MainWindowViewModel(new AccountService(_connectionString), new TaskService(_connectionString));

            InitializeComponent();
        }
    }
}