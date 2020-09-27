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
    }
    public enum UserType { Admin, Student }
}
