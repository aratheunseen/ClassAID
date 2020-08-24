using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAidUniversal.Schedule
{
    public class ScheduleCore
    {
        public List<ScheduleModel> Schedules { get; set; }
        public string Department { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }
    }
}
