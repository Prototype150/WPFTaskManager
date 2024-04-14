using DesktopTaskManager.CustomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopTaskManager.View
{
    /// <summary>
    /// Логика взаимодействия для TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl
    {
        public static readonly DependencyProperty IsTaskOpenProperty =
            DependencyProperty.Register(nameof(IsTaskOpen), typeof(bool), typeof(TaskView), new PropertyMetadata(false));
        public bool IsTaskOpen
        {
            get { return (bool)GetValue(IsTaskOpenProperty); }
            set { SetValue(IsTaskOpenProperty, value); }
        }

        public TaskView()
        {
            InitializeComponent();
        }

        private void mainGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            IsTaskOpen = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsTaskOpen = !IsTaskOpen;
        }

        private void extendCalendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }
    }
}
