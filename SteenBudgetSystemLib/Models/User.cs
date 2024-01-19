using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Persoid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int EmailConfirmed { get; set; }

    }
}
