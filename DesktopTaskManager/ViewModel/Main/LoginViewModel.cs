using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel.Main
{
    public class LoginViewModel : BaseMainViewModel
    {
        private IAccountService _accountService;

        public string Username { get; set; }
        public string Password { get; set; }
        private string _errorMessage;
        public string ErrorMessage {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            }
        }

        public ICommand LoginCommand { get; set; }

        public LoginViewModel(IAccountService accountService)
        {
            _accountService = accountService;

            LoginCommand = new RelayCommand(Login);
        }

        private async void Login(object? parameter)
        {
            
            var result = await _accountService.Login(Username, Password);
            if(result.account == null)
            {
                switch(result.message)
                {
                    case "wrong_password":
                        ErrorMessage = "Worng password!";
                        break;
                    case "not_exist":
                        ErrorMessage = "Account does not exist!";
                        break;
                    case "empty:username":
                        ErrorMessage = "Username is empty!";
                        break;
                    case "empty:password":
                        ErrorMessage = "Password is empty!";
                        break;
                    default:
                        ErrorMessage = result.message;
                        break;
                }
                return;
            }

            MainViewModel.MainAccount = result.account;
            RequireViewChange(MainViewType.Main);
        }
    }
}
