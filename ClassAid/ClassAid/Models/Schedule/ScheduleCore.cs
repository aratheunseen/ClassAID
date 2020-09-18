using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Schedule
{
    public class ScheduleCore
    {
        public List<ScheduleModel> Schedules { get; set; }
        public string Department { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }
        public string University { get; set; }
    }
}
