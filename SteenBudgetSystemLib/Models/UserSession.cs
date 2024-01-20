using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class UserSession
    {
        public string Username { get; set; }
        public string Roles { get; set; }
        public bool FirstLogin { get; set; }
    }

}
