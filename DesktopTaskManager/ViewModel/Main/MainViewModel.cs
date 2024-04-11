using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        private bool _isAdding;
        public bool IsAdding
        {
            get { return _isAdding; }
            set 
            { 
                _isAdding = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsAdding)));
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DueDate)));
            }
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

        public string DueTodayString
        {
            get { return "Tasks due today: " + Tasks.Where(x => x.DueDate == DateOnly.FromDateTime(DateTime.Today) && (x.TaskState == TaskState.Extended || x.TaskState == TaskState.InProgress)).Count(); }
        }

        private bool _isUpdating;
        public bool IsUpdating
        {
            get => _isUpdating;
            set
            {
                _isUpdating = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsUpdating)));
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
            UpdateAllTasksCommand = new AsyncCommandRelay(UpdateAllTasks);
            DeleteTaskCommand = new RelayCommand(DeleteTask);
            AddTaskCommand = new RelayCommand(AddTask);
            DueDate = DateTime.Now;
            Tasks.CollectionChanged += TasksChanged;
        }

        private void TasksChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Tasks)));
            OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DueTodayString)));
        }

        private async void AddTask(object? parameter)
        {
            IsAdding = true;
            if (!string.IsNullOrWhiteSpace(NewTask))
            {
                var result = await _taskService.AddTask(new TaskModel(MainAccount.Id, NewTask, Tasks.Count(), TaskState.InProgress, DateOnly.FromDateTime(DueDate)));
                if(result.result != null)
                {
                    Tasks.Add(new TaskViewModel(result.result.Id, result.result.Task, true, false, result.result.SortId, result.result.DueDate, TaskState.InProgress, _taskService));
                    NewTask = string.Empty;
                }
            }
            IsAdding = false;
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
                }
            }
        }

        private void UpdateTaskStates()
        {
            foreach (var task in Tasks)
            {
                if((task.TaskState == TaskState.InProgress || task.TaskState == TaskState.Extended)&& task.DueDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    task.TaskState = TaskState.Failed;
                }
            }

            UpdateAllTasks(null);
        }

        private async Task UpdateAllTasks(object? parameter)
        {
            IsUpdating = true;
            List<Task> tasks = new List<Task>();
            foreach (var task in Tasks.Where(x => !x.IsUpdated))
            {
                tasks.Add(Task.Run(async () => await task.UpdateTask(parameter)));
            }

            Task.WhenAll(tasks).ContinueWith(x => IsUpdating = false);
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

            if (tasks.tasks != null)
            {
                foreach (var task in tasks.tasks)
                {
                    Tasks.Add(new TaskViewModel(task.Id, task.Task, true, task.IsCompleted, task.SortId, task.DueDate, task.State, _taskService));
                }

                UpdateTaskStates();
            }
        }
    }
}