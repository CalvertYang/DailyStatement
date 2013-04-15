using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyStatement.ViewModel
{
    public class PersonalWorkingHours
    {
        public string EmployeeName { get; set; }
        public int InternalHours { get; set; }      // Wintriss
        public int ProjectHours { get; set; }       // CN,CP,CO,C*,LINPO
        public int UndefineHours { get; set; }      // ST
        public int DemoHours { get; set; }          // DO
        public int ResearchHours { get; set; }      // CR
        public int Overtime { get; set; }           // 加班

    }
}