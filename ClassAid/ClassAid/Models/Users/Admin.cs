using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ClassAid.Models.Users
{
    public class Admin
    {
        [MinLength(6)]
        public string Username { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public Admin(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Admin() { }
        public string Key { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public ObservableCollection<Teacher> TeacherList { get; set; }
        public ObservableCollection<Student> StudentList { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        public ObservableCollection<EventModel> EventList { get; set; }
        public BatchDetails BatchDetails { get; set; }
    }
}
