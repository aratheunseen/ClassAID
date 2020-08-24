using System;
using ClassAidUniversal.WebConnection;
using ClassAidUniversal.Users;
namespace ClassAid_Raw_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Admin admin = new Admin();
            admin.UserName = "HasanXX";
            ///"UserName": "MahmudXX",
            admin.Password = "iamawesome";
            admin.Email = "ggg@hello.com";
            admin.Phone = "+8801915577428";
            admin.AdminKey = admin.GetID();
            


            string res = RegisterAdmin.Register(admin);
            Console.WriteLine(res);
        }
    }
}
