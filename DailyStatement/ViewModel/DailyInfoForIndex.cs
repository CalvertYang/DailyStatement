using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyStatement.ViewModel
{
    public class DailyInfoForIndex
    {
        public int DailyInfoId { get; set; }
        public string WorkCategory { get; set; }
        public string ProjectNo { get; set; }
        public string Customer { get; set; }
        public string WorkContent { get; set; }
        public DateTime CreateDate { get; set; }
        public int EmployeeId { get; set; }
        public int WorkingHours { get; set; }
    }
}