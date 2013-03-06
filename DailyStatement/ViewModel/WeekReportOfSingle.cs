using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyStatement.ViewModel
{
    public class WeekReportOfSingle
    {
        public string WorkName { get; set; }
        public int Sunday { get; set; }
        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesday { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Total {
            get { return this.Sunday + this.Monday + this.Tuesday + this.Wednesday + this.Thursday + this.Friday + this.Saturday; }
        }
    }
}