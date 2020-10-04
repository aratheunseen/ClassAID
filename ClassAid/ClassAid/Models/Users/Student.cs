using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Users
{
    public class Student
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string Phone { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
    }
}
