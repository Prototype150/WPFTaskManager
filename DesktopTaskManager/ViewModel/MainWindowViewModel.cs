using DesktopTaskManager.Command;
using DesktopTaskManager.Services;
using DesktopTaskManager.Services.Interfaces;
using DesktopTaskManager.ViewModel.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel
{
    public class MainWindowViewModel : BaseMainViewModel
    {
        private IAccountService _accountService;
        private ITaskService _taskService;

        public ICommand SwitchMainViewCommand { get; set; }

        public MainWindowViewModel(IAccountService accountService, ITaskService taskService)
        {
            _taskService = taskService;
            _accountService = accountService;
            SwitchMainViewCommand = new RelayCommand(SwitchMainView);
            CurrentViewModel = new LoginViewModel(_accountService);

            CurrentViewModel.OnMainViewChangeRequired += SwitchMainView;
        }

        private void SwitchMainView(object? parameter)
        {
            MainViewType type = (MainViewType)parameter;
            switch (type)
            {
                case MainViewType.Login:
                    CurrentViewModel = new LoginViewModel(_accountService);
                    CurrentViewModel.OnMainViewChangeRequired += SwitchMainView;
                    break;
                case MainViewType.Register:
                    CurrentViewModel = new RegisterViewModel(_accountService);
                    CurrentViewModel.OnMainViewChangeRequired += SwitchMainView;
                    break;
                case MainViewType.Main:
                    CurrentViewModel = new MainViewModel(_taskService);
                    CurrentViewModel.OnMainViewChangeRequired += SwitchMainView;
                    (CurrentViewModel as MainViewModel)?.GetTasks();
                    break;
            }
        }

        private BaseMainViewModel _currentViewModel;
        public BaseMainViewModel CurrentViewModel 
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }
    }
}
