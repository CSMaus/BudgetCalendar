using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class Category : INotifyPropertyChanged
    {
        private string _name;
        private decimal _limit;
        private bool _isDaily;
        private bool _isWeekendDifferent;
        private decimal _weekendLimit;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public decimal Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged(nameof(Limit));
            }
        }

        public bool IsDaily
        {
            get => _isDaily;
            set
            {
                _isDaily = value;
                OnPropertyChanged(nameof(IsDaily));
            }
        }

        public bool IsWeekendDifferent
        {
            get => _isWeekendDifferent;
            set
            {
                _isWeekendDifferent = value;
                OnPropertyChanged(nameof(IsWeekendDifferent));
            }
        }

        public decimal WeekendLimit
        {
            get => _weekendLimit;
            set
            {
                _weekendLimit = value;
                OnPropertyChanged(nameof(WeekendLimit));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
