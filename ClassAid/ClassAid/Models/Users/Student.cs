using ClassAid.Models.Engines;
using Newtonsoft.Json;

namespace ClassAid.Models.Users
{
    public class Student
    {
        public Student(string Username, string Password)
        {
            this.Username = Username.ToLower();
            this.Password = Password;
            Key = Cryptography.EncryptString(this.Username, this.Password);
        }
        public Student()
        {

        }
        public string TeamCode { get; set; }
        public string AdminKey { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
