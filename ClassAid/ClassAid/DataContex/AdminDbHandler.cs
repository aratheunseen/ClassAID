using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassAid.DataContex
{
    public class AdminDbHandler
    {
        public static async Task UpdateAdmin(FirebaseClient client, Admin newAdmin)
        {
            //var toUpdatePerson = (await client
            //  .Child("Admin")
            //  .OnceAsync<Admin>()).Where(a => a.Object.Key == newAdmin.Key).FirstOrDefault();

            await client
              .Child("Admin")
              .Child(newAdmin.Key)
              .PutAsync(newAdmin);
        }
        public static async Task<T> RealTimeConnection<T>(FirebaseClient client,string tablename, T data)
        {
            object respons = null;
            await Task.Run(() => client
               .Child(tablename)
               .AsObservable<T>()
               .Subscribe(d => respons = d.Object));
            return (T) Convert.ChangeType(respons, typeof(T));
        }
        public static async Task DeletePerson(FirebaseClient client, Admin removableAdmin)
        {
            var toDeletePerson = (await client
              .Child("Admin")
              .OnceAsync<Admin>()).Where(a => a.Object.Key == removableAdmin.Key).FirstOrDefault();
            await client.Child("Admin").Child(toDeletePerson.Key).DeleteAsync();
        }
        //public static async Task<Admin> GetPerson(FirebaseClient client, string table, string key)
        //{
        //    Admin admin = (Admin)(await client.Child(table).OnceAsync<Admin>()).Where(a => a.Object.Key == key).FirstOrDefault();
        //    return admin;
        //}
    }
}
