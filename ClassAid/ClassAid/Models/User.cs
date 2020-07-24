using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public User()
        {

        }
        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public bool CheckInformation()
        {
            if (!Password.Equals("") && !UserName.Equals(""))
                return true;
            else
                return false;
        }
    }
}
