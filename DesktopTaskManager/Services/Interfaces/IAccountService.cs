using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopTaskManager.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(AccountModel? account, string message)> Login(string username, string password);
        Task<(AccountModel? account, string message)> Register(string username, string password);
    }
}
