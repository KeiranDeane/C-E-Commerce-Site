using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GradedUnitVersion5KeiranDeane.ViewModels;
namespace GradedUnitVersion5KeiranDeane.Models
{
    
    public class AnalyticsViewModel
    {
        public List<OrderDateGroup> OrderData { get; set; }

        public List<OrderDateGroup> OrderDataForToday { get; set; }
    }
}