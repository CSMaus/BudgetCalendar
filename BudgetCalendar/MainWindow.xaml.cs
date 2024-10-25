using BudgetCalendar.Models;
using BudgetCalendar.ViewModels;
using BudgetCalendar.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BudgetViewModel _budgetViewModel;
        private Budget1UserControl budget1UserControl;

        public MainWindow()
        {
            InitializeComponent();
            budget1UserControl = new Budget1UserControl();
            _budgetViewModel = new BudgetViewModel();
            BudgetUC1.Navigate(budget1UserControl);
            UpdateDataContext(_budgetViewModel);
        }

        private void UpdateDataContext(BudgetViewModel viewModel)
        {
            budget1UserControl.DataContext = null;
            budget1UserControl.DataContext = viewModel;
        }

        public void OnSaveButtonClicked(Month currentMonth)
        {
            DataManager dataManager = new DataManager(); // TODO
            Console.WriteLine("Data saved to JSON.");
        }
        public void OnLoadButtonClicked()
        {
            DataManager dataManager = new DataManager(); // TODO
            Console.WriteLine("Data loaded from JSON.");
        }
    }
}
