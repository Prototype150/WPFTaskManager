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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopTaskManager.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock : UserControl
    {
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register(nameof(IsEditMode), typeof(bool), typeof(EditableTextBlock), new PropertyMetadata(false));
        public bool IsEditMode
        {
            get {  return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EditableTextBlock), new PropertyMetadata(""));
        public string Text
        {
            get { return (string)GetValue(TextProperty); } 
            set { SetValue(TextProperty, value); }
        }

        public EditableTextBlock()
        {
            InitializeComponent();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditMode = false;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            IsEditMode = true;
        }

        private void editableTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void editableTextBox_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
