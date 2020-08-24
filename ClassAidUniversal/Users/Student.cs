using System.ComponentModel.DataAnnotations;

namespace ClassAidUniversal.Users
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string AdminCode { get; set; }
    }
}
