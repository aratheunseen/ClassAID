using System;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;

namespace ClassAid.Models.Users
{
    public class Shared
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get { return DateTime.Now; } }
        public string Email { get; set; }
        public ObservableCollection<Teacher> TeacherList { get; set; }
        public ObservableCollection<Student> StudentList { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        public ObservableCollection<EventModel> EventList { get; set; }
        public BatchDetails BatchDetails { get; set; }
    }
}
