using System;
using System.Collections.Generic;
using System.Text;

namespace ClassAid.Models.Users
{
    public class RetakeStudentModel
    {
        public string AdminKey { get; set; }
        public bool IsActive { get; set; }
        public string TeamCode { get; set; }
    }
}
