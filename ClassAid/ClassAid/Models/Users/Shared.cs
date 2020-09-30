using System;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using ClassAid.Models.Engines;

namespace ClassAid.Models.Users
{
    public class Shared
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
        public string Name { get; set; }
        public string Key { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get { return DateTime.Now; } }
        public string Email { get; set; }
        public ObservableCollection<Teacher> TeacherList { get; set; }
        public ObservableCollection<Shared> StudentList { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        public ObservableCollection<EventModel> EventList { get; set; }
        public BatchDetails BatchDetails { get; set; }
        public string AdminKey { get; set; }
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
