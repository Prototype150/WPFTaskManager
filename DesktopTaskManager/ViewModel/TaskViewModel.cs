using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using DesktopTaskManager.ViewModel.Main;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel
{
    public class TaskViewModel : BaseViewModel
    {
        private ITaskService _taskService;

        public ICommand CompleteTaskCommand { get; set; }
        public ICommand UpdateTaskCommand { get; set; }
        public ICommand ExtendTaskCommand { get; set; }

        private int _id;
        public int Id
        {
            get { return _id; }
            set 
            {
                _id = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        private bool _isUpdated;
        public bool IsUpdated
        {
            get { return _isUpdated; }
            private set 
            { 
                _isUpdated = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsUpdated)));
            }
        }

        

        private TaskState _taskState;
        public TaskState TaskState
        {
            get { return _taskState; }
            set
            {
                _taskState = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(TaskState)));
            }
        }

        private string _task;
        public string Task 
        {
            get { return _task; }
            set
            {
                if (value != _task)
                {
                    _task = value;
                    IsUpdated = false;
                    OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Task)));
                }
            }
        }

        public bool IsCompleted
        {
            get { return TaskState == TaskState.Completed || TaskState == TaskState.ExtendedCompleted; }
        }

        private int _sortId;
        public int SortId
        {
            get { return _sortId; }
            set
            {
                _sortId = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(SortId)));
            }
        }

        private DateOnly _dueDate;
        public DateOnly DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DueDate)));
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(DueDateString)));
            }
        }

        public string DueDateString
        {
            get { return DueDate.ToString(); }
        }

        public TaskViewModel(int id,string task, bool isUpdated, bool isCompleted, int sortId, DateOnly dueDate, TaskState taskState, ITaskService taskService)
        {
            Id = id;
            TaskState = taskState;
            SortId = sortId;
            Task = task;
            IsUpdated = isUpdated;
            DueDate = dueDate;

            UpdateTaskCommand = new AsyncCommandRelay(UpdateTask);
            CompleteTaskCommand = new RelayCommand(CompleteTask);
            ExtendTaskCommand = new RelayCommand(ExtendTask);

            _taskService = taskService;
        }

        private void ExtendTask(object? parameter)
        {
            if(TaskState != TaskState.Completed && TaskState != TaskState.ExtendedCompleted && parameter is DateTime newDate)
            {
                DateOnly newDueDate = DateOnly.FromDateTime((DateTime)parameter);
                DueDate = newDueDate;
                TaskState = TaskState.Extended;
                UpdateTask(null);
            }
        }

        private async void CompleteTask(object? parameter)
        {
            switch (TaskState)
            {
                case TaskState.InProgress: {
                        TaskState = TaskState.Completed;
                        break; 
                    }
                case TaskState.Extended: {
                        TaskState = TaskState.ExtendedCompleted; 
                        break;
                    }
            }

            OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            UpdateTask(null);
        }

        public async Task UpdateTask(object? s)
        {
            var result = await _taskService.UpdateTask(new TaskModel(MainViewModel.MainAccount.Id, Task, SortId, TaskState, DueDate) { Id = Id, IsCompleted = IsCompleted });
            if (result.result)
                IsUpdated = true;
        }
    }
}
