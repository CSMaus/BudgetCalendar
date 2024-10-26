using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class Day
    {
        public ObservableCollection<Category> Categories { get; set; }
        // public ObservableCollection<List<decimal>> DailySpends { get; set; }
        public ObservableCollection<string> DailySpends { get; set; }
        public ObservableCollection<decimal> DailySpendsSum { get; set; }
        public ObservableCollection<decimal> RemainingBudget { get; set; }

        // will try to use these only
        public ObservableCollection<SpendsRemainsByCategoryInDay> SRByCatInDay{ get; set; }

        public DateTime TodaysDate { get; set; }
        public decimal RemainingDailyBudgetTotal { get; set; }
        public decimal SpendsDailyBudgetTotal { get; set; }




        public void CalculateDailyRemains(Day previousDay)
        {
            bool isWeekend = (TodaysDate.DayOfWeek == DayOfWeek.Saturday || TodaysDate.DayOfWeek == DayOfWeek.Sunday);

            if (Categories == null)
            {
                Categories = new ObservableCollection<Category>();
                DailySpends = new ObservableCollection<string>();
                DailySpendsSum = new ObservableCollection<decimal>();
                RemainingBudget = new ObservableCollection<decimal>();
            }

            for (int i = 0; i < Categories.Count; i++)
            {
                var prevDayR = previousDay != null ? previousDay.RemainingBudget[i] : 0;

                var category = Categories[i];

                if (category.IsDaily)
                {
                    decimal dailyLimit = category.IsWeekendDifferent && isWeekend ? category.WeekendLimit : category.Limit;
                    decimal spentToday = DailySpendsSum[i];
                    RemainingBudget[i] = prevDayR + dailyLimit - spentToday;
                }
            }

            SpendsDailyBudgetTotal = DailySpendsSum.Sum(); // spends => spends.Sum());
            RemainingDailyBudgetTotal = RemainingBudget.Sum();


            OnPropertyChanged(nameof(RemainingDailyBudgetTotal));
            OnPropertyChanged(nameof(SpendsDailyBudgetTotal));
            OnPropertyChanged(nameof(DailySpends));
        }

        public void UpdateDailySpendsSum(int index)
        {
            var spends = DailySpends[index].Split('+');
            decimal sum = 0;
            foreach (var s in spends)
            {
                if (decimal.TryParse(s, out decimal value))
                {
                    sum += value;
                }
                else if (decimal.TryParse(s.Replace(".", ","), out value))
                {
                    sum += value;
                }
            }
            DailySpendsSum[index] = sum;
        }

        public void UpdateDailySpendsSumTotal()
        {
            DailySpendsSum.Clear();

            foreach (var spend in DailySpends)
            {
                var spends = spend.Split('+');
                decimal sum = 0;
                foreach (var s in spends)
                {
                    if (decimal.TryParse(s, out decimal value))
                    {
                        sum += value;
                    }
                    else if(decimal.TryParse(s.Replace(".", ","), out value))
                    {
                        sum += value;
                    }
                }
                DailySpendsSum.Add(sum);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
