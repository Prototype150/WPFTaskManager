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

    }
}
