using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopTaskManager.Services.Interfaces
{
    public interface ITaskService
    {
        Task<(IEnumerable<TaskModel>? tasks, string message)> GetAccountTasks(int accountId);
        Task<(bool result, string message)> UpdateTask(TaskModel task);
        Task<(bool result, string message)> DeleteTask(int taskId);
        Task<(TaskModel result, string message)> AddTask(TaskModel newTask);
    }
}
