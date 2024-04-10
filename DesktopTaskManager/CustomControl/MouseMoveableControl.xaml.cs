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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopTaskManager.CustomControl
{
    /// <summary>
    /// Логика взаимодействия для MouseMoveableControl.xaml
    /// </summary>
    public partial class MouseMoveableControl : UserControl
    {
        private Point _mouseDownPoint;
        private UIElement _parent;

        public static readonly DependencyProperty IsHoldingProperty =
            DependencyProperty.Register(nameof(IsHolding), typeof(bool), typeof(MouseMoveableControl), new PropertyMetadata(false));
        public bool IsHolding
        {
            get { return (bool)GetValue(IsHoldingProperty); }
            set { SetValue(IsHoldingProperty, value); }
        }

        public MouseMoveableControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _parent = (Parent as UIElement);
            if (_parent != null)
            {
                _parent.MouseLeave += Parent_MouseLeave;
                _parent.MouseUp += UserControl_MouseUp;
                _parent.MouseMove += UserControl_MouseMove;
            }
        }

        private void Parent_MouseLeave(object sender, MouseEventArgs e)
        {
            IsHolding = false;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IsHolding = true;

            _mouseDownPoint = e.GetPosition(_parent);
            sa.Text = _mouseDownPoint.X + " "+ _mouseDownPoint.Y;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsHolding = false;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsHolding)
            {
                var currentMousePos = Mouse.GetPosition(_parent);

                Margin = new Thickness(-(_mouseDownPoint.X-currentMousePos.X), -(_mouseDownPoint.Y - currentMousePos.Y), _mouseDownPoint.X - currentMousePos.X, _mouseDownPoint.Y - currentMousePos.Y);
            }
        }
    }
}
