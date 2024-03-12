using DesktopTaskManager.Services;
using DesktopTaskManager.ViewModel.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopTaskManager.ViewModel
{
    public class MainWindowViewModel : BaseViewMainModel
    {
        public MainWindowViewModel()
        {
            CurrentViewModel = new LoginViewModel(new AccountService());
        }

        private BaseViewMainModel _currentViewModel;
        public BaseViewMainModel CurrentViewModel 
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }
    }
}
