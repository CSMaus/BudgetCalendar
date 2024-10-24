using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class Day
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<List<decimal>> DailySpends { get; set; }  // Spends for each category
        public ObservableCollection<decimal> RemainingDailyBudget { get; set; }  // Remaining budget for each category
        public DateTime TodaysDate { get; set; }
        public decimal RemainingDailyBudgetTotal { get; set; }
        public decimal SpendsDailyBudgetTotal { get; set; }

        // Method to calculate remains for each category for this day
        public void CalculateDailyRemains(Day previousDay)
        {
            bool isWeekend = (TodaysDate.DayOfWeek == DayOfWeek.Saturday || TodaysDate.DayOfWeek == DayOfWeek.Sunday);

            for (int i = 0; i < Categories.Count; i++)
            {
                var prevDayR = previousDay != null ? previousDay.RemainingDailyBudget[i] : 0;

                var category = Categories[i];

                if (category.IsDaily)
                {
                    decimal dailyLimit = category.IsWeekendDifferent && isWeekend ? category.WeekendLimit : category.Limit;
                    decimal spentToday = DailySpends[i].Sum();
                    RemainingDailyBudget[i] = prevDayR + dailyLimit - spentToday;
                }
            }

            SpendsDailyBudgetTotal = DailySpends.Sum(spends => spends.Sum());
            RemainingDailyBudgetTotal = RemainingDailyBudget.Sum();
        }
    }
}
