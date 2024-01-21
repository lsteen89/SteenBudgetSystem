using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteenBudgetSystemLib.Helpers
{
    public class FinancialCalculator
    {
        public string GetRatioText(double sliderValue, bool isRatioConfirmed)
        {
            if (isRatioConfirmed)
            {
                return "Using calculated ratio";
            }
            else
            {
                int xx = (int)sliderValue;
                int yy = 100 - xx;
                return $"Other ratio: {xx:00}/{yy:00}";
            }
        }
        public decimal CalculateUserRatio(decimal userMainIncome, decimal userOtherIncome, decimal partnerMainIncome, decimal partnerOtherIncome)
        {
            decimal totalIncome = userMainIncome + userOtherIncome + partnerMainIncome + partnerOtherIncome;
            if (totalIncome > 0)
            {
                return (userMainIncome + userOtherIncome) / totalIncome * 100;
            }
            else
            {
                return 0;
            }
        }

    }
}
