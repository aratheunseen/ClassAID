using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClassAid.DataContex
{
    public class LocalAdminStorageContex
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;
        public LocalAdminStorageContex()
        {
            InitializeAsync().SafeFireAndForget(false);
        }
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(AdminLocalStorageModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(AdminLocalStorageModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }
        public async Task<Admin> GetItemsAsync(string key)
        {
            var response = await Database.QueryAsync<AdminLocalStorageModel>($"SELECT * FROM [Admin] WHERE [Key] = {key}");
            var res = response[0];
            Admin admin = new Admin()
            {
                Key = res.Key,
                Name = res.Name,
                ID = res.ID,
                JoinDate = res.JoinDate,
                Phone = res.Phone,
                TeacherList = JsonConvert.DeserializeObject<ObservableCollection<Teacher>>(res.TeacherList),
                StudentList = JsonConvert.DeserializeObject<ObservableCollection<Student>>(res.StudentList),
                ScheduleList = JsonConvert.DeserializeObject<ObservableCollection<ScheduleModel>>(res.ScheduleList),
                EventList = JsonConvert.DeserializeObject<ObservableCollection<EventModel>>(res.EventList),
                BatchDetails = JsonConvert.DeserializeObject<BatchDetails>(res.BatchDetails),
            };
            admin.Key = res.Key;
            return admin;
        }

        public void SaveItemAsync(Admin admin)
        {
            AdminLocalStorageModel adminLocal = new AdminLocalStorageModel()
            {
                Key = admin.Key,
                Name = admin.Name,
                ID = admin.ID,
                JoinDate = admin.JoinDate,
                Phone = admin.Phone,
                TeacherList = JsonConvert.SerializeObject(admin.TeacherList),
                StudentList = JsonConvert.SerializeObject(admin.StudentList),
                ScheduleList = JsonConvert.SerializeObject(admin.ScheduleList),
                EventList = JsonConvert.SerializeObject(admin.EventList),
                BatchDetails = JsonConvert.SerializeObject(admin.BatchDetails)
            };
            if (admin.Key != null)
            {
                Database.UpdateAsync(adminLocal);
            }
            else
            {
                Database.InsertAsync(adminLocal);
            }
        }
    }
}
