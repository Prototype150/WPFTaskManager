using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        private string _newTask;
        public string NewTask
        {
            get { return _newTask; }
            set
            {
                _newTask = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(NewTask)));
            }
        }

        public ICommand LogoutCommand { get; set; }
        public ICommand UpdateAllTasksCommand { get; set; }
        public ICommand DeleteTaskCommand { get; set; }
        public ICommand AddTaskCommand { get; set; }

        public MainViewModel(ITaskService taskService)
        {
            Tasks = new ObservableCollection<TaskViewModel>();
            _taskService = taskService;
            
            LogoutCommand = new RelayCommand(Logout);
            UpdateAllTasksCommand = new RelayCommand(UpdateAllTasks);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            AddTaskCommand = new RelayCommand(AddTask);

            GetTasks();
        }

        private async void AddTask(object? parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                var result = await _taskService.AddTask(new TaskModel(MainAccount.Id, NewTask, Tasks.Count()));
                if(result.result != null)
                {
                    Tasks.Add(new TaskViewModel(result.result.Id, result.result.Task, true, false, result.result.SortId, _taskService));
                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tasks)));
                    NewTask = string.Empty;
                }
            }
        }

        private async void DeleteTask(object? parameter)
        {
            int taskId = (int)(parameter ?? -1);
            if(taskId > -1)
            {
                var result = await _taskService.DeleteTask(taskId);
                if (result.result)
                {
                    Tasks.Remove(Tasks.FirstOrDefault(x => x.Id == taskId));
                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tasks)));
                }
            }
        }

        private void UpdateAllTasks(object? parameter)
        {
            foreach (var task in Tasks.Where(x => !x.IsUpdated))
            {
                task.UpdateTaskCommand.Execute(null);
            }
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
                Tasks.Add(new TaskViewModel(task.Id ,task.Task, true, task.IsCompleted, task.SortId, _taskService));
            }
            OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Tasks)));
        }
    }
}