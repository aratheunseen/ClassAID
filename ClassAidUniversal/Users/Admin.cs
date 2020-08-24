using ClassAidUniversal.WebConnection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ClassAidUniversal.Users
{
    public class Admin
    {
        [MinLength(6)]
        public string UserName { get; set; }
        [MinLength(6)]
        [PasswordPropertyText]
        public string Password { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string AdminKey { get; set; }
        public string GetID()
        {
            string r = ProcessKey.GetKey(UserName, Password);
            return r.Substring(0,10);
        }
    }
}
