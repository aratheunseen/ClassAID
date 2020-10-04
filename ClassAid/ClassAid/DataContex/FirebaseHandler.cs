using ClassAid.Models;
using ClassAid.Models.Engines;
using ClassAid.Models.Users;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
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
                      AuthTokenAsyncFactory = () => Task.FromResult(authKey),
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
        public static async Task UpdateUser(Shared user)
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
        public static async Task RealTimeConnection<T>(CollectionTables collectionName,
            ObservableCollection<T> collection, string tablename = "Admin")
        {
            await Task.Run(() => client
               .Child(tablename).Child(collectionName.ToString())
               .AsObservable<T>()
               .Subscribe(d => collection.Add(d.Object)));
            //return (T)Convert.ChangeType(respons, typeof(T));
        }
        public static async Task ListOfAdmin(ObservableCollection<string> collection,
            string tablename = "AdminCodeLookUp")
        {
            await Task.Run(() => client
               .Child(tablename)
               .AsObservable<string>()
               .Subscribe(d => collection.Add(d.Object)));
            //return (T)Convert.ChangeType(respons, typeof(T));
        }
        public async static Task<string> GetTeamCode(string universityName)
        {
            Start:
            string res = "";
            foreach (var item in universityName.Split())
            {
                res += item[0];
            }
            res += Adminkey.GetCode();
            string validator = await ValidateTeamCode(res);
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
        public static async Task<string> ValidateTeamCode(string teamCode)
        {
            return (await client
                .Child("AdminCodeLookUp")
                .OnceAsync<string>()).Select(item => item.Object)
                .Where(item => item == teamCode).FirstOrDefault();
        }
        public static async Task<Shared> GetUser(string key, bool IsAdmin)
        {
            Shared res = (await client
              .Child(TableName(IsAdmin))
              .OnceAsync<Shared>()).Select(item => item.Object)
            .Where(item => item.Key == key).FirstOrDefault();
            LocalStorageEngine.SaveDataAsync(res, FileType.Shared);
            return res;
        }
        public static async Task<Shared> GetAdmin(string teamcode)
        {
            return (await client
              .Child("Admin")
              .OnceAsync<Shared>()).Select(item => item.Object)
            .Where(item => item.TeamCode == teamcode).FirstOrDefault();
        }
        private static string TableName(bool IsAdmin)
        {
            if (IsAdmin)
                return "Admin";
            else
                return "Student";
        }
    }
    public enum CollectionTables
    {
        EventList, ScheduleList, TeacherList, StudentList
    }
}
