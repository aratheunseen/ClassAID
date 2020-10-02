using System;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using ClassAid.Models.Engines;
using System.Net.Http.Headers;

namespace ClassAid.Models.Users
{
    public class Shared : Student
    {
        public Shared(string Username, string Password)
        {
            this.Username = Username.ToLower();
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Shared()
        {

        }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Key { get; set; }

        public DateTime JoinDate { get { return DateTime.Now; } }
        public string Email { get; set; }
        public ObservableCollection<Teacher> TeacherList { get; set; } = new ObservableCollection<Teacher>();
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; } = new ObservableCollection<ScheduleModel>();
        public ObservableCollection<EventModel> EventList { get; set; } = new ObservableCollection<EventModel>();
        public BatchDetails BatchDetails { get; set; }
        public string TeamCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class PrefKeys
    {
        public static string isSyncPending { get { return "isSyncPending"; } }
        public static string isLoggedIn { get { return "isLoggedIn"; } }
        public static string adminKey { get { return "adminKey"; } }
    }
}
