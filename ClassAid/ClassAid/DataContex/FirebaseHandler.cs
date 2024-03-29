﻿using System.Diagnostics;
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
using System.Collections.Generic;
using ClassAid.Models.Schedule;

namespace ClassAid.DataContex
{
    public class FirebaseHandler
    {
        public static FirebaseClient GetClient()
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
        #region AdminArea
        public static async void InsertAdmin(Admin admin)
        {
            await GetClient().Child(admin.Key).PostAsync(admin);
        }
        public static async Task<Admin> GetAdminAsync(string key)
        {
            return await GetClient().Child(key).OnceSingleAsync<Admin>();
        }
        public static async void UpdateAdmin(Admin admin)
        {
            await GetClient().Child(admin.Key).PutAsync(admin);
        }
        #endregion
        #region StudentArea
        public static async void InsertStudent(Student student)
        {
            await GetClient().Child(student.Key).PostAsync(student);
        }
        public static async Task<Student> GetStudentAsync(string key)
        {
            var d = await GetClient().Child(key).OnceSingleAsync<Student>();
            return d;
        }
        public static async void UpdateStudent(Student student)
        {
            await GetClient().Child(student.Key).PutAsync(student);
        }
        #endregion
        #region RealTime
        public static async Task RealTimeConnection<T>(CollectionTables collectionName,
            ObservableCollection<T> collection, string key)
        {
            _ = await Task.Run(() => GetClient()
                 .Child(key).Child(collectionName.ToString())
                 .AsObservable<T>()
                 .Subscribe(d =>
                 {
                     if (!collection.Contains(d.Object) && d.Object != null)
                     {
                         bool isAdmin = Preferences.Get(PrefKeys.IsAdmin, false);
                         collection.Insert(0, d.Object);
                         if (collectionName == CollectionTables.EventList && !isAdmin)
                             LocalDbContex.SaveEvent(d.Object as EventModel);
                         else if (collectionName == CollectionTables.ScheduleList && !isAdmin)
                             LocalDbContex.SaveSchedule(d.Object as ScheduleModel);
                     }
                 }));
        }
        public static async void RealTimeChat(ObservableCollection<ChatModel> collection)
        {
            _ = await Task.Run(() => GetClient().Child("Chat")
                 .Child(Preferences.Get(PrefKeys.AdminKey, ""))
                 .AsObservable<ChatModel>().Subscribe(d =>
                 {
                     ChatModel chat = new ChatModel();
                     chat = d.Object;
                     chat.DeletingKey = d.Key;
                     if (!collection.Any(p => p.DeletingKey == d.Key))
                     {
                         collection.Add(chat);
                         LocalDbContex.SaveChat(chat);
                     }
                     if (collection.Count > 50)
                     {
                         GetClient().Child("Chat")
                         .Child(Preferences.Get(PrefKeys.AdminKey, ""))
                         .Child(collection[0].DeletingKey).DeleteAsync();
                     }
                 }));
        }
        public static async void GetStudentList(ObservableCollection<Student> studentColl, string adminkey)
        {
            _ = await Task.Run(() => GetClient()
            .Child(adminkey)
            .Child("StudentList")
            .AsObservable<Student>().Subscribe(p =>
            {
                if (!studentColl.Contains(p.Object))
                    studentColl.Insert(0, p.Object);
            }));
        }
        #endregion
        #region Teamcode
        public async static Task<string> GetTeamCode(string universityName, string key)
        {
        Start:
            string res = "";
            foreach (var item in universityName.Split())
            {
                res += item[0];
            }
            res += Adminkey.GetCode();
            KeyVault validator = await ValidateTeamCode(res);
            if (validator == null)
            {
                validator = new KeyVault() { TeamCode = res, AdminKey = key };
                await GetClient()
                .Child("AdminCodeLookUp")
                .PostAsync(JsonConvert.SerializeObject(validator));
                return res;
            }
            else
                goto Start;
        }
        public static async Task<KeyVault> ValidateTeamCode(string teamCode)
        {
            return (await GetClient()
                .Child("AdminCodeLookUp")
                .OnceAsync<KeyVault>()).Select(item => item.Object)
                .Where(item => item.TeamCode == teamCode).FirstOrDefault();
        }
        #endregion
        public static async Task<ObservableCollection<Student>> GetPendingStudents(string key)
        {
            var students = new ObservableCollection<Student>();
            _ = await Task.Run(() => GetClient()
            .Child(key)
            .Child("StudentList")
            .AsObservable<Student>().Subscribe(
                p =>
                {
                    if (p.Object.IsActive == false && p.Object.IsRejected == false &&
                    !students.Contains(p.Object))
                        students.Insert(0, p.Object);
                }));
            return students;
        }
        public async static void SendMessage(ChatModel chat)
        {
            var d = await GetClient().Child("Chat")
                .Child(Preferences.Get(PrefKeys.AdminKey, "")).PostAsync(chat);
            // App.Chats.FirstOrDefault(p => p.Time == chat.Time).DeletingKey = d.Key;
            //chat.DeletingKey = d.Key;
            //App.Chats.Add(chat);
            //LocalDbContex.SaveChat(chat);
        }
    }
    public enum CollectionTables
    {
        EventList, ScheduleList, TeacherList, StudentList
    }
}
