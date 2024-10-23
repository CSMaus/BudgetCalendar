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
                Days.Add(new Day { TodaysDate = new DateTime(Year, MonthNumber, i) });
            }
        }
    }
    public class Day
    {
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<List<decimal>> DailySpends { get; set; } // number of categories x number of spends for each category
        public ObservableCollection<decimal> RemainingDailyBudget { get; set; } // remaining budget for each category for this day
        public DateTime TodaysDate { get; set; }
        public decimal RemainingDailyBudgetTotal { get; set; }
        public decimal SpendsDailyBudgetTotal { get; set; }

    }
}
