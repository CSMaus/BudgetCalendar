using BudgetCalendar.Models;
using BudgetCalendar.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace BudgetCalendar.ViewModels
{
    public class BudgetViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDate;
        private DataManager _dataManager;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                UpdateSelectedDay();
            }
        }

        private Day _selectedDay;
        public Day SelectedDay
        {
            get => _selectedDay;
            set
            {
                _selectedDay = value;
                OnPropertyChanged(nameof(SelectedDay));
            }
        }

        public ObservableCollection<Month> AllMonths { get; set; }
        public ICommand AddNewCategoryCommand { get; set; }

        public BudgetViewModel()
        {
            _dataManager = new DataManager();
            AllMonths = _dataManager.LoadAllData();
            SelectedDate = DateTime.Today;
            UpdateSelectedDay();
            AddNewCategoryCommand = new RelayCommand(() => AddNewCategory());
        }
        private void UpdateSelectedDay()
        {
            // Find the current month based on the selected date
            var currentMonth = AllMonths.FirstOrDefault(m => m.Year == SelectedDate.Year && m.MonthNumber == SelectedDate.Month);

            if (currentMonth == null)
            {
                currentMonth = new Month(SelectedDate.Year, SelectedDate.Month);
                AllMonths.Add(currentMonth);
            }

            SelectedDay = currentMonth.Days.FirstOrDefault(d => d.TodaysDate.Date == SelectedDate.Date);

            if (SelectedDay == null)
            {
                SelectedDay = new Day
                {
                    TodaysDate = SelectedDate,
                    Categories = new ObservableCollection<Category>(),
                    DailySpends = new ObservableCollection<List<decimal>>(),
                    RemainingDailyBudget = new ObservableCollection<decimal>(),
                    RemainingDailyBudgetTotal = 0,
                    SpendsDailyBudgetTotal = 0
                };
                currentMonth.Days.Add(SelectedDay);
            }
        }

        public void AddNewCategory()
        {
            var categoryManager = new CategoryManager();
            var daysArr = AllMonths.SelectMany(m => m.Days).ToArray();
            ObservableCollection<Day> days = new ObservableCollection<Day>(daysArr);
            var categories = categoryManager.InitializeCategoriesForDay(SelectedDate, days);
            categories.Add(new Category { Name = "New Category", Limit = 0, IsDaily = false });
            SelectedDay.Categories = categories;
            OnPropertyChanged(nameof(SelectedDay));
        }
        public void SaveAllData()
        {
            _dataManager.SaveAllData(AllMonths);
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;
            public RelayCommand(Action execute)
            {
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) => true;

            public void Execute(object parameter)
            {
                _execute();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
