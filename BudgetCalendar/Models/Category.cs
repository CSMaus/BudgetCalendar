using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetCalendar.Models
{
    public class Category
    {
        public string Name { get; set; }
        public decimal Limit { get; set; }
        public bool IsDaily { get; set; }

    }
    
}
