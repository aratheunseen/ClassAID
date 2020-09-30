using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ClassAid.DataContex
{
    public class AdminDbHandler
    {
        public static async Task InsertData(FirebaseClient client, Shared user)
        {
            await client
                .Child(TableName(user.IsAdmin))
                .Child(user.Key)
                .PostAsync(user);
            LocalStorageEngine.SaveDataAsync(user, FileType.Admin);            
        }
        public static async Task UpdateAdmin(FirebaseClient client, Shared user)
        {
            await client
              .Child(TableName(user.IsAdmin))
              .Child(user.Key)
              .PutAsync(user);
            LocalStorageEngine.SaveDataAsync(user, FileType.Admin);
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
        public static async Task<Shared> GetAdmin(FirebaseClient client, string key, bool IsAdmin)
        {
            Shared res = (await client
              .Child(TableName(IsAdmin))
              .OnceAsync<Shared>()).Select(item => item.Object)
            .Where(item => item.Key == key).FirstOrDefault();
            LocalStorageEngine.SaveDataAsync(res, FileType.Admin);
            return res;
        }
        private static string TableName(bool IsAdmin)
        {
            if (IsAdmin)
                return "Admin";
            else
                return "Student";
        }
    }
}
