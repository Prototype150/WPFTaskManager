using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DesktopTaskManager.ViewModel.Main
{
    public class MainViewModel : BaseMainViewModel
    {
        public static AccountModel? MainAccount 
        {
            get;
            set;
        }
    }
}