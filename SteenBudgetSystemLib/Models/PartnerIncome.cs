using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class PartnerIncome
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public decimal MainIncome { get; set; }
        public decimal SideIncome { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
        public Partner Partner { get; set; }
    }

}
