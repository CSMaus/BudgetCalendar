using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class SpendsRemainsByCategoryInDay : INotifyPropertyChanged
    {
        private string _CategoryName;
        private decimal _SpendsSum;
        private string _Spends;
        private decimal _Remains;

        public string CategoryName
        {
            get => _CategoryName;
            set
            {
                _CategoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        public string Spends
        {
            get => _Spends;
            set
            {
                _Spends = value;
                OnPropertyChanged(nameof(Spends));
            }
        }

        public decimal SpendsSum
        {
            get => _SpendsSum;
            set
            {
                _SpendsSum = value;
                OnPropertyChanged(nameof(SpendsSum));
            }
        }
        public decimal Remains
        {
            get => _Remains;
            set
            {
                _Remains = value;
                OnPropertyChanged(nameof(Remains));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
