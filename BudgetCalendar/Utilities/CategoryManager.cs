using BudgetCalendar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Utilities
{
    public class CategoryManager
    {
        public ObservableCollection<Category> InitializeCategoriesForDay(DateTime date, ObservableCollection<Day> days)
        {
            var previousDay = days.LastOrDefault(d => d.TodaysDate < date);
            if (previousDay == null)
                return new ObservableCollection<Category>();

            var categories = previousDay.Categories.Select(c => new Category { Name = c.Name, Limit = c.Limit, IsDaily = c.IsDaily }).ToArray();
            // convert categories to observable collection
            return new ObservableCollection<Category>(categories);
        }

        public void AddOrUpdateCategory(ObservableCollection<Category> categories, string name, decimal limit, bool isDaily)
        {
            var category = categories.FirstOrDefault(c => c.Name == name);
            if (category != null)
            {
                category.Limit = limit;
                category.IsDaily = isDaily;
            }
            else
            {
                categories.Add(new Category { Name = name, Limit = limit, IsDaily = isDaily });
            }
        }
    }
}
