using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.Command
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public delegate void Commander(object? parameter);
        private Commander _mainCommand;

        public RelayCommand(Commander mainCommand) 
        {
            _mainCommand = mainCommand;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _mainCommand?.Invoke(parameter);
        }
    }
}
