using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Models
{
    public class Partner
    {
        public int PartnerId { get; set; }
        public string PersoId { get; set; }
        public string Name { get; set; }

        // Add other properties that match the columns in your Partner table

        // Navigation property to link to the User model
        public User User { get; set; }
    }

}
