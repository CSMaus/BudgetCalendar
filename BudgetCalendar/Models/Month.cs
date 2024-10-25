using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class Month
    {
        public decimal Income { get; set; }
        public ObservableCollection<Day> Days { get; set; }
        public ObservableCollection<decimal> RemainingMonthlyBudget { get; set; } // this number of items will be equal to number of categories
        public ObservableCollection<decimal> SpendsMonthlyBudget { get; set; }  // this number of items will be equal to number of categories
        public decimal RemainingMonthlyBudgetTotal { get; set; }
        public decimal SpendsMonthlyBudgetTotal { get; set; }
        public int Year { get; set; }
        public int MonthNumber { get; set; }

        public Month(int year, int monthNumber)
        {
            Year = year;
            MonthNumber = monthNumber;
            Days = new ObservableCollection<Day>();
            RemainingMonthlyBudget = new ObservableCollection<decimal>();
            SpendsMonthlyBudget = new ObservableCollection<decimal>();
            InitializeDays();
        }

        private void InitializeDays()
        {
            int daysInMonth = DateTime.DaysInMonth(Year, MonthNumber);
            for (int i = 1; i <= daysInMonth; i++)
            {
                var newDay = new Day { TodaysDate = new DateTime(Year, MonthNumber, i) };
                if (i==1)
                {
                    newDay.CalculateDailyRemains(null);
                }
                else
                {
                    newDay.CalculateDailyRemains(Days[i - 2]);
                }
                Days.Add(newDay);
            }
        }
        public void CalculateMonthlyRemains()
        {
            var totalDays = Days.Count;

            // need to select here todays date, not Day[0]
            int todaysDateIndex = DateTime.Now.Day - 1;

            for (int i = 0; i < Days[todaysDateIndex].Categories.Count; i++) // BudgetCalendar.Models.Day.Categories.get returned null.
            {
                var category = Days[todaysDateIndex].Categories[i];

                if (!category.IsDaily)
                {
                    decimal totalSpent = 0;

                    for (int dayIndex = 0; dayIndex < totalDays; dayIndex++)
                    {
                        totalSpent += Days[dayIndex].DailySpendsSum[i];
                    }

                    RemainingMonthlyBudget[i] = category.Limit - totalSpent;
                }
            }
            SpendsMonthlyBudgetTotal = SpendsMonthlyBudget.Sum();
            RemainingMonthlyBudgetTotal = RemainingMonthlyBudget.Sum();
        }
    }
}
