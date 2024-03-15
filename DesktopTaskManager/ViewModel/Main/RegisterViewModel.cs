using DesktopTaskManager.Command;
using DesktopTaskManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopTaskManager.ViewModel.Main
{
    public class RegisterViewModel : BaseMainViewModel
    {
        private IAccountService _accountService;

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set {
                _errorMessage = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ErrorMessage)));
            }
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public ICommand RegisterCommand { get; set; }

        public RegisterViewModel(IAccountService accountService)
        {
            _accountService = accountService;

            RegisterCommand = new RelayCommand(Register);
        }

        private async void Register(object? parameter)
        {
            var result = await _accountService.Register(Username, Password);
            if (result.account == null)
            {
                switch (result.message)
                {
                    case "exists":
                        ErrorMessage = "Account with the username already exists!";
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
