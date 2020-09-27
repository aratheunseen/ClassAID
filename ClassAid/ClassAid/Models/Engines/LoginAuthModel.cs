using ClassAid.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Engines
{
    public class LoginAuthModel
    {
        public bool isLoggedIn { get; set; }
        public string key { get; set; }
        public UserType User { get; set; }
        [JsonIgnore]
        private Admin _adminData { get; set; }
        [JsonIgnore]
        private Student _studentData { get; set; }
        public Admin AdminData
        {
            get
            {
                return _adminData;
            }
            set
            {
                if (User == UserType.Admin)
                    _adminData = value;
                else
                    _adminData = null;
            }
        }
        public Student StudentData
        {
            get
            {
                return _studentData;
            }
            set
            {
                if (User == UserType.Student)
                    _studentData = value;
                else
                    _studentData = null;
            }
        }
    }
    public enum UserType { Admin, Student }
}
