using SteenBudgetSystemLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Services
{
    public interface ISessionService
    {
        UserSession CurrentSession { get; }
        void StartSession(User user);
        void EndSession();
    }
}
