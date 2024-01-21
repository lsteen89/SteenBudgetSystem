using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Services
{
    public interface IDialogService
    {
        void ShowWindow(string windowName);
        void ShowMessage(string message, string title);
    }
}
