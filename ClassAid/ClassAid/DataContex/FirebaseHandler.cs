using ClassAid.Models;
using ClassAid.Models.Engines;
using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

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
            LocalStorageEngine.SaveDataAsync(user, FileType.Shared);
            await client
                .Child(TableName(user.IsAdmin))
                .Child(user.Key)
                .PostAsync(user);
        }
        public static async Task UpdateAdmin(Shared user)
        {
            LocalStorageEngine.SaveDataAsync(user, FileType.Shared);
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await client
              .Child(TableName(user.IsAdmin)).Child(user.Key).PutAsync(user);
            }
            else
            {
                DependencyService.Get<Toast>().Show("No Internet connection. Saved for later syncing.");
                Preferences.Set(PrefKeys.isSyncPending, true);
            }
        }
        #region RealTime
        public static async Task<T> RealTimeConnection<T>(string tablename, T data)
        {
            object respons = null;
            await Task.Run(() => client
               .Child(tablename)
               .AsObservable<T>()
               .Subscribe(d => respons = d.Object));
            return (T)Convert.ChangeType(respons, typeof(T));
        }
        public async static Task<string> ValidateAdminCode(string universityName)
        {
            Start:
            string res = "";
            foreach (var item in universityName.Split())
            {
                res += item[0];
            }
            res += Adminkey.GetCode();
            string validator = (await client
              .Child("AdminCodeLookUp")
              .OnceAsync<string>()).Select(item => item.Object)
            .Where(item => item == res).FirstOrDefault();
            if (validator == null)
            {
                await client
                .Child("AdminCodeLookUp")
                .PostAsync(JsonConvert.SerializeObject(res));
                return res;
            }
            else
                goto Start;
        }
        #endregion
        public static async Task<Shared> GetAdmin(string key, bool IsAdmin)
        {
            Shared res = (await client
              .Child(TableName(IsAdmin))
              .OnceAsync<Shared>()).Select(item => item.Object)
            .Where(item => item.Key == key).FirstOrDefault();
            LocalStorageEngine.SaveDataAsync(res, FileType.Shared);
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
