using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ClassAid.DataContex
{
    public class FirebaseHandler
    {
        private static FirebaseClient client
        {
            get
            {
                string server = "https://classaidapp.firebaseio.com/";
                string authKey = "q4ckBo2jl1p2EB0qg9eTnAwXwPKYwt2DbcSCOc5V";
                return new FirebaseClient(
                  server,
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(authKey)
                  });
            }
        }
        public static async Task InsertData(Shared user)
        {
            await client
                .Child(TableName(user.IsAdmin))
                .Child(user.Key)
                .PostAsync(user);
            LocalStorageEngine.SaveDataAsync(user, FileType.Admin);
        }
        public static async Task UpdateAdmin(Shared user)
        {
            await client
              .Child(TableName(user.IsAdmin))
              .Child(user.Key)
              .PutAsync(user);
            LocalStorageEngine.SaveDataAsync(user, FileType.Admin);
        }
        #region RealTime
        //public static async Task<T> RealTimeConnection<T>(string tablename, T data)
        //{
        //    object respons = null;
        //    await Task.Run(() => client
        //       .Child(tablename)
        //       .AsObservable<T>()
        //       .Subscribe(d => respons = d.Object));
        //    return (T)Convert.ChangeType(respons, typeof(T));
        //}
        //public IDisposable StreamChat<T>(string table)
        //{
        //    IDisposable observable = client
        //      .Child(table)
        //      .AsObservable<T>()
        //      .Subscribe();
        //    return observable;
        //}
        #endregion
        public static async Task<Shared> GetAdmin(string key, bool IsAdmin)
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
