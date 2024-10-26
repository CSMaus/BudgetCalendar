using BudgetCalendar.Models;
using BudgetCalendar.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace BudgetCalendar.ViewModels
{
    public class BudgetViewModel : INotifyPropertyChanged
    {
        private DateTime _selectedDate;
        private DataManager _dataManager;

        private Category _NewCategoryTemp;
        public Category NewCategoryTemp
        {
            get => _NewCategoryTemp;
            set
            {
                _NewCategoryTemp = value;
                OnPropertyChanged(nameof(NewCategoryTemp));
            }
        }
        private bool _isPanelAddCategory;
        public bool IsPanelAddCategory
        {
            get => _isPanelAddCategory;
            set
            {
                _isPanelAddCategory = value;
                OnPropertyChanged(nameof(IsPanelAddCategory));
            }
        }

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
                UpdateSelectedSpendsWhenChangeDay();
            }
        }

        public ObservableCollection<SpendsRemainsByCategoryInDay> SelectedSpends { get; set; } = new ObservableCollection<SpendsRemainsByCategoryInDay>();


        public ObservableCollection<Month> AllMonths { get; set; }
        public ICommand AddNewCategoryCommand { get; }
        public ICommand AddSpendCommand { get; } // add new spend in list of sends of selected category of selected day

        private int _selectedCategoryIndex;
        public int SelectedCategoryIndex
        {
            get => _selectedCategoryIndex;
            set
            {
                _selectedCategoryIndex = value;
                OnPropertyChanged(nameof(SelectedCategoryIndex));
            }
        }

        public BudgetViewModel()
        {
            SelectedSpends = new ObservableCollection<SpendsRemainsByCategoryInDay>();
            SelectedSpends.CollectionChanged += SelectedSpends_CollectionChanged;

            NewCategoryTemp = new Category { Name = "New Category", Limit = 0, IsDaily = false, IsWeekendDifferent = false, WeekendLimit = 0 };

            _dataManager = new DataManager();
            AllMonths = _dataManager.LoadAllData();
            SelectedDate = DateTime.Today;
            UpdateSelectedDay();
            // need to add here uploading all daa from json
            // UpdateSelectedSpendsWhenChangeDay();

            SelectedDay.DailySpends.CollectionChanged += DailySpends_CollectionChanged;
            AddNewCategoryCommand = new RelayCommand(() => AddNewCategory());
        }

        private void SelectedSpends_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SpendsRemainsByCategoryInDay item in e.NewItems)
                {
                    item.PropertyChanged += UpdateSpends_PropertyChanged;
                }
            }
            else if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (SpendsRemainsByCategoryInDay item in e.OldItems)
                {
                    item.PropertyChanged -= UpdateSpends_PropertyChanged;
                }
            }
        }

        private void UpdateSpends_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Spends")
            {
                if(sender is SpendsRemainsByCategoryInDay item)
                {
                    int CategoryIndex = SelectedSpends.IndexOf(item);
                    UpdateSpendsndRemains(CategoryIndex);
                }
                // SelectedDay.UpdateDailySpendsSum(SelectedCategoryIndex);
            }
        }

        public void UpdateSelectedSpendsWhenChangeDay()
        {
            if (SelectedDay == null)
            {
                return;
            }

            if (SelectedSpends.Count != SelectedDay.Categories.Count)
            {
                SelectedSpends.Clear();
                for (int i = 0; i < SelectedDay.Categories.Count; i++)
                {
                    var spend = new SpendsRemainsByCategoryInDay
                    {
                        Spends = SelectedDay.DailySpends[i],
                        SpendsSum = SelectedDay.DailySpendsSum[i],
                        Remains = SelectedDay.RemainingBudget[i],
                        CategoryName = SelectedDay.Categories[i].Name
                    };
                    SelectedSpends.Add(spend);
                }
            }
            else
            {
                for (int i = 0; i < SelectedSpends.Count; i++)
                {
                    SelectedSpends[i].Spends = SelectedDay.DailySpends[i];
                    SelectedSpends[i].SpendsSum = SelectedDay.DailySpendsSum[i];
                    SelectedSpends[i].Remains = SelectedDay.RemainingBudget[i];
                    SelectedSpends[i].CategoryName = SelectedDay.Categories[i].Name;
                }
            }
        }

        public void UpdateSpendsndRemains(int index)
        {
            var spends = SelectedSpends[index].Spends.Split('+');
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
            SelectedSpends[index].SpendsSum = sum;

            var limit = SelectedDay.Categories[index].Limit;
            SelectedSpends[index].Remains = limit - sum;

            // now update all values for selected day
            SelectedDay.DailySpends[index] = SelectedSpends[index].Spends;
            SelectedDay.DailySpendsSum[index] = sum;
            SelectedDay.RemainingBudget[index] = limit - sum;

            SelectedDay.SpendsDailyBudgetTotal += sum;
            SelectedDay.RemainingDailyBudgetTotal = SelectedDay.RemainingBudget.Sum();
        }



        private void DailySpends_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Replace && e.NewStartingIndex >= 0)
            {
                //SelectedDay.UpdateDailySpendsSum(e.NewStartingIndex);
            }
        }

        private void UpdateSelectedDay()
        {
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
                    DailySpends = new ObservableCollection<string>(),
                    DailySpendsSum = new ObservableCollection<decimal>(),
                    RemainingBudget = new ObservableCollection<decimal>(),
                    RemainingDailyBudgetTotal = 0,
                    SpendsDailyBudgetTotal = 0
                };
                currentMonth.Days.Add(SelectedDay);
            }

            var previousDay = currentMonth.Days.FirstOrDefault(d => d.TodaysDate < SelectedDay.TodaysDate);
            SelectedDay.CalculateDailyRemains(previousDay);

            currentMonth.CalculateMonthlyRemains();
        }

        public void AddNewSpend()
        {
            // TODO: change it to OnDailySpends Item changed
            if (SelectedDay != null && SelectedCategoryIndex >= 0 && SelectedCategoryIndex < SelectedDay.DailySpends.Count)
            {
             

                // Recalculate the remains after adding the new spend
                // SelectedDay.CalculateDailyRemains(null);  // instead of null need to define previous day
            }
        }

        public void AddNewCategory()
        {
            var currentMonth = AllMonths.FirstOrDefault(m => m.Year == SelectedDate.Year && m.MonthNumber == SelectedDate.Month);

            if (currentMonth == null)
            {
                return;
            }


            var newCategory = new Category
            {
                Name = NewCategoryTemp.Name,
                Limit = NewCategoryTemp.Limit,
                IsDaily = NewCategoryTemp.IsDaily,
                IsWeekendDifferent = NewCategoryTemp.IsWeekendDifferent,
                WeekendLimit = NewCategoryTemp.WeekendLimit
            };
            SelectedDay.Categories.Add(newCategory);

            SelectedDay.DailySpends.Add("");
            SelectedDay.DailySpendsSum.Add(0);
            SelectedDay.RemainingBudget.Add(0);

            foreach (var day in currentMonth.Days)
            {
                // TODO:
                // when loop over all days, need to be sure, that collections (like categories) are not null
                // i e need to initialize them
                decimal dayLim = newCategory.Limit;
                if (!day.Categories.Any(c => c.Name == newCategory.Name))
                {
                    day.Categories.Add(newCategory);
                    day.DailySpends.Add("");
                    day.DailySpendsSum.Add(0);
                    day.RemainingBudget.Add(dayLim);
                }
            }

            OnPropertyChanged(nameof(SelectedDay));
            UpdateSelectedSpendsWhenChangeDay();
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
