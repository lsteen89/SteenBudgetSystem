using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class PartnerIncome
    {
        public int PartnerIncomeId { get; set; }
        public string PartnerId { get; set; } 
        public decimal MainIncome { get; set; }
        public decimal SideIncome { get; set; }
        public Partner Partner { get; set; }
    }

}
