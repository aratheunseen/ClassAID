using ClassAid.Models.Engines;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClassAid.Models.Users
{
    public class Student : Shared
    {
        public Student(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Student()
        {

        }
        public string AdminKey { get; set; }
        public bool IsActive { get; set; }

    }
}
