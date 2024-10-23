using BudgetCalendar.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace BudgetCalendar.ViewModels
{
    public class DataManager
    {
        private const string FileName = "budgetData.json";

        public void SaveAllData(ObservableCollection<Month> allMonths)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(folderPath, "BudgetCalendar");

            if (!Directory.Exists(appFolder))
            {
                Directory.CreateDirectory(appFolder);
            }

            string fullPath = Path.Combine(appFolder, FileName);

            string jsonString = JsonSerializer.Serialize(allMonths, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fullPath, jsonString);
        }

        public ObservableCollection<Month> LoadAllData()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(folderPath, "BudgetCalendar");
            string fullPath = Path.Combine(appFolder, FileName);

            if (!File.Exists(fullPath))
            {
                return new ObservableCollection<Month>();
            }

            string jsonString = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<ObservableCollection<Month>>(jsonString);
        }
    }
}
