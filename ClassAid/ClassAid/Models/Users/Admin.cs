using System;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using ClassAid.Models.Engines;

namespace ClassAid.Models.Users
{
    public class Admin : Student
    {
        public Admin(string Username, string Password)
        {
            this.Username = Username.ToLower();
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Admin()
        {

        }
        public DateTime JoinDate { get { return DateTime.Now; } }
        public ObservableCollection<Teacher> TeacherList { get; set; } = new ObservableCollection<Teacher>();
        public ObservableCollection<Student> StudentList { get; set; } = new ObservableCollection<Student>();
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; } = new ObservableCollection<ScheduleModel>();
        public ObservableCollection<EventModel> EventList { get; set; } = new ObservableCollection<EventModel>();
        public ObservableCollection<RetakeStudentModel> RetakeStudentList { get; set; }
        public BatchDetails BatchDetails { get; set; } = new BatchDetails();
    }
    public class PrefKeys
    {
        public static string IsSyncPending { get { return "isSyncPending"; } }
        public static string IsLoggedIn { get { return "isLoggedIn"; } }
        public static string AdminKey { get { return "adminKey"; } }
        public static string IsAdmin { get { return "isAdmin"; } }
        public static string Key { get { return "key"; } }
    }
}
