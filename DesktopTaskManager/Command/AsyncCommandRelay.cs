using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.Command
{
    public delegate Task CommanderAsync(object? parameter);
    public class AsyncCommandRelay : ICommand
    {
        private CommanderAsync _mainCommand;
        public event EventHandler? CanExecuteChanged;

        public AsyncCommandRelay(CommanderAsync command)
        {
            _mainCommand = command;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if (_mainCommand != null)
            {
                Task task = Task.Run(() => _mainCommand(parameter));
            }
        }
    }
}
