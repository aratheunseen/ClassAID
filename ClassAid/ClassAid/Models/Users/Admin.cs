using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
        public string Key { get; private set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public long Phone { get; set; }
        public string ID { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
        public string UserBase { get; set; }
        public ObservableCollection<Teacher> teacherList { get; set; }
        public List<Student> StudentList 
        { 
            get 
            {
                if (!string.IsNullOrWhiteSpace(UserBase))
                    return JsonConvert.DeserializeObject<List<Student>>(UserBase);
                else
                    return new List<Student>();                
            }
        }
        public string Schedule { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        public ObservableCollection<EventModel> EventList { get; set; }
        public static explicit operator Admin(FirebaseObject<Admin> v)
        {
            throw new NotImplementedException();
        }
    }
}
