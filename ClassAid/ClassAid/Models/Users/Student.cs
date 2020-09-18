using ClassAid.Models.Engines;
using System;
using System.ComponentModel.DataAnnotations;

namespace ClassAid.Models.Users
{
    public class Student
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Student(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public string Key { get; private set; }
        [Required]
        public string AdminKey { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string ID { get; set; }
        public DateTime JoinDate { get { return DateTime.Now; } }
        public bool IsActive { get; set; }

    }
}
