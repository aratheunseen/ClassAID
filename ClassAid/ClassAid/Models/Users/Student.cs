using ClassAid.Models.Schedule;

namespace ClassAid.Models.Users
{
    public class Student
    {
        public string TeamCode { get; set; }
        public string AdminKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public BatchDetails BatchDetails { get; set; } = new BatchDetails();
    }
}
