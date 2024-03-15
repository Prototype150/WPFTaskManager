using DesktopTaskManager.Command;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopTaskManager.ViewModel.Main
{
    public enum MainViewType
    {
        Login,
        Register,
        Main
    }
    public abstract class BaseMainViewModel : BaseViewModel {

        public event Commander OnMainViewChangeRequired;

        public void RequireViewChange(MainViewType type)
        {
            OnMainViewChangeRequired?.Invoke(type);
        }
    }
}