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

        public ICommand UpdateTaskStateCommand { get; set; }

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

        private string _task;
        public string Task {
            get { return _task; }
            set
            {
                _task = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Task)));
            }
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
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

        public TaskViewModel(int id,string task, bool isCompleted, int sortId, ITaskService taskService)
        {
            Id = id;
            SortId = sortId;
            Task = task;
            IsCompleted = isCompleted;

            UpdateTaskStateCommand = new RelayCommand(UpdateTaskState);

            _taskService = taskService;
        }

        private async void UpdateTaskState(object? parameter)
        {
            var result = await _taskService.UpdateTask(new TaskModel(MainViewModel.MainAccount.Id, Task, SortId) { Id = Id, IsCompleted = !IsCompleted });
            if(result.result)
                IsCompleted = !IsCompleted;
        }
    }
}
