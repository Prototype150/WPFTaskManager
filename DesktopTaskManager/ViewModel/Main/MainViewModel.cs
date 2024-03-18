using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel.Main
{
    public class MainViewModel : BaseMainViewModel
    {
        private ITaskService _taskService;

        public ObservableCollection<TaskViewModel> Tasks { get; set; }

        public static AccountModel? MainAccount
        {
            get;
            set;
        }

        public static string Username
        {
            get => MainAccount?.Username ?? "";
        }

        public static string Id
        {
            get => MainAccount?.Id.ToString() ?? "";
        }

        public ICommand LogoutCommand { get; set; }

        public MainViewModel(ITaskService taskService)
        {
            Tasks = new ObservableCollection<TaskViewModel>();
            _taskService = taskService;
            
            LogoutCommand = new RelayCommand(Logout);

            GetTasks();
        }

        private void Logout(object? parameter)
        {
            MainAccount = null;
            RequireViewChange(MainViewType.Login);
        }

        public async void GetTasks()
        {
            if (MainAccount == null)
            {
                return;
            }

            var tasks = await _taskService.GetAccountTasks(MainAccount.Id);
            foreach (var task in tasks.tasks)
            {
                Tasks.Add(new TaskViewModel(task.Id ,task.Task, task.IsCompleted, task.SortId, _taskService));
            }
            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Tasks)));
        }
    }
}