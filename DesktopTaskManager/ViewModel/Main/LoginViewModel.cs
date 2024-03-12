using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel.Main
{
    public class LoginViewModel : BaseViewMainModel
    {
        private IAccountService _accountService;

        public ICommand TestCommand { get; set; }

        public LoginViewModel(IAccountService accountService)
        {
            _accountService = accountService;

            TestCommand = new RelayCommand(Test);
        }

        private void Test(object? parameter)
        {
            
        }
    }
}
