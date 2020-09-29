using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClassAid.DataContex
{
    public class AdminDbHandler
    {
        public static async Task InsertData(FirebaseClient client, Admin admin)
        {
            await client.Child("Admin").Child(admin.Key).PostAsync(admin);
        }
        public static async Task UpdateAdmin(FirebaseClient client, Admin newAdmin)
        {
            //var toUpdatePerson = (await client
            //  .Child("Admin")
            //  .OnceAsync<Admin>())
            //  .Where(a => a.Key == newAdmin.Key)
            //  .FirstOrDefault();

            await client
              .Child("Admin")
              .Child(newAdmin.Key)
              .PutAsync(newAdmin);
        }
        public static async Task<T> RealTimeConnection<T>(FirebaseClient client, string tablename, T data)
        {
            object respons = null;
            await Task.Run(() => client
               .Child(tablename)
               .AsObservable<T>()
               .Subscribe(d => respons = d.Object));
            return (T)Convert.ChangeType(respons, typeof(T));
        }
        public static async Task DeletePerson(FirebaseClient client, Admin removableAdmin)
        {
            var toDeletePerson = (await client
              .Child("Admin")
              .OnceAsync<Admin>()).Where(a => a.Object.Key == removableAdmin.Key).FirstOrDefault();
            await client.Child("Admin").Child(toDeletePerson.Key).DeleteAsync();
        }
        //public async Task<Admin> GetPerson(string key)
        //{
        //    var allPersons = await GetAllPersons();
        //    await firebase
        //      .Child("Persons")
        //      .OnceAsync<Person>();
        //    return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
        //}
        public static async Task<Admin> GetAdmin(FirebaseClient client, string key)
        {
            return (Admin)(await client
              .Child("Admin")
              .OnceAsync<Admin>()).Select(item => item.Object)
            .Where(item => item.Key == key).FirstOrDefault();
            //client
            //  .Child("Admin")
            //  .OnceAsync<Admin>().Result.Count.ToString());
        }
    }
}
