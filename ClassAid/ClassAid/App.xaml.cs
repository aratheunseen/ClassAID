using System;
using Xamarin.Forms;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Users;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Views.StudentViews;
using System.Linq;

namespace ClassAid
{
    public partial class App : Application
    {
        internal static Admin Admin { get; set; }
        internal static Student Student { get; set; }
        internal static ObservableCollection<ChatModel> Chats { get; set; }

        public App()
        {
            InitializeComponent();
            Admin = new Admin();
            Student = new Student();
            Admin.TeacherList = new ObservableCollection<Teacher>();
            Admin.StudentList = new ObservableCollection<Student>();
            Admin.ScheduleList = new ObservableCollection<ScheduleModel>();
            Admin.EventList = new ObservableCollection<EventModel>();
            Admin.BatchDetails = new BatchDetails();

            bool loginState = Preferences.Get(PrefKeys.IsLoggedIn, false);
            var networkState = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += CheckConnection;

            if (!loginState && networkState == NetworkAccess.Internet)
            {
                MainPage = new NavigationPage(new StartPage());
                if (networkState != NetworkAccess.Internet)
                    DependencyService.Get<Toast>().Show("No INTERNET connection.");
            }
            else
            {
                if (Preferences.Get(PrefKeys.IsAdmin, false))
                    MainPage = new NavigationPage(new Views.Dashboard());
                else
                {
                    Student student = LocalDbContex.GetStudentAsUser();
                    if (student.IsActive)
                        MainPage = new NavigationPage(new Views.StudentViews.Dashboard());
                    else
                    {
                        MainPage = new StudentNotActivatedPage(student, LocalDbContex.GetAdminAsUser());
                    }
                }
                Chats = new ObservableCollection<ChatModel>(LocalDbContex.GetChats());
            }

            OneSignal.Current.StartInit("7ab7ae00-d9e6-47cb-a4ed-f5045215fc9f")
            .Settings(new Dictionary<string, bool>()
            {
                {
                    IOSSettings.kOSSettingsKeyAutoPrompt, false
                },
                {
                    IOSSettings.kOSSettingsKeyInAppLaunchURL, false
                }
            })
            .InFocusDisplaying(OSInFocusDisplayOption.Notification)
            .EndInit();
        }
        private void CheckConnection(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Preferences.Get(PrefKeys.IsSyncPending, false))
                {
                    try
                    {
                        Admin admin = LocalStorageEngine.ReadDataAsync<Admin>
                      (FileType.Admin);
                        FirebaseHandler.UpdateAdmin(admin);
                        DependencyService.Get<Toast>().Show("Synced successfully.");
                        Preferences.Set(PrefKeys.IsSyncPending, true);
                    }
                    catch (Exception)
                    {
                        DependencyService.Get<Toast>().Show("Sync failed. ERROR code PCAiDx06");
                    }
                }
            }
            else
                return;
        }
        protected override void OnStart()
        {
            AppCenter.Start("android=6c9e3d95-2ad6-4960-bdd8-d63483b3a7a2;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        public static void LogOut()
        {
            Preferences.Set(PrefKeys.IsLoggedIn, false);
            Current.MainPage =
                new NavigationPage(new StartPage());
            LocalDbContex.DropTables();
            OneSignal.Current.DeleteTag("AdminKey");
        }
        public static void UpdateAdminOrSync(Admin admin)
        {
            //Admin admin
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                FirebaseHandler.UpdateAdmin(admin);
            else
            {
                Preferences.Set(PrefKeys.IsSyncPending, true);
                LocalStorageEngine.SaveDataAsync(admin, FileType.Admin);
                DependencyService.Get<Toast>().Show("No internet access. Sync pending.");
            }
        }
    }
}
