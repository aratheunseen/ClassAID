using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ClassAid.DataContex
{
    public class AdminLocalStorageModel
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string TeacherList { get; set; }
        public string StudentList { get; set; }
        public string ScheduleList { get; set; }
        public string EventList { get; set; }
        public string BatchDetails { get; set; }
    }
}
