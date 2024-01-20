using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class Income
    {
        public int IncomeId { get; set; }
        public string PersoId { get; set; }
        public decimal MainIncome { get; set; }
        public decimal SideIncome { get; set; }

        public User User { get; set; }
    }

}
