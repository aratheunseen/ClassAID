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

namespace ClassAid
{
    public partial class App : Application
    {
        internal static Admin Admin { get; set; }
        internal static Student Student { get; set; }
        internal static ObservableCollection<Teacher> TeacherList { get; set; }
        internal static ObservableCollection<Student> StudentList { get; set; }
        internal static ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        internal static ObservableCollection<EventModel> EventList { get; set; }
        internal static ObservableCollection<RetakeStudentModel> RetakeStudentList { get; set; }
        internal static BatchDetails BatchDetails { get; set; }
        private bool FetchOnce { get; set; }
        public App()
        {
            InitializeComponent();
            Admin = new Admin();
            Student = new Student();
            TeacherList = new ObservableCollection<Teacher>();
            StudentList = new ObservableCollection<Student>();
            ScheduleList = new ObservableCollection<ScheduleModel>();
            EventList = new ObservableCollection<EventModel>();
            RetakeStudentList = new ObservableCollection<RetakeStudentModel>();
            BatchDetails = new BatchDetails();

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
                InitializeData();
                MainPage = new NavigationPage(new Dashboard());
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
        private async void InitializeData()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Admin = await FirebaseHandler.GetAdminAsync(
                    Preferences.Get(PrefKeys.AdminKey, ""));

                ScheduleList = Admin.ScheduleList;
                EventList = Admin.EventList;
                MainPage = new NavigationPage(new Dashboard());

                LocalDbContex.SaveSchedules(ScheduleList);
                LocalDbContex.SaveEvents(EventList);

                TeacherList = Admin.TeacherList;
                LocalDbContex.SaveTeachers(TeacherList);

                StudentList = Admin.StudentList;
                LocalDbContex.SaveStudents(StudentList);

                //RetakeStudentList = Admin.RetakeStudentList;

                BatchDetails = Admin.BatchDetails;
                LocalDbContex.SaveBatchDetails(BatchDetails);

                FetchOnce = false;
                await FirebaseHandler.RealTimeConnection
                    (CollectionTables.EventList, EventList, Admin.Key);
                await FirebaseHandler.RealTimeConnection
                    (CollectionTables.ScheduleList, ScheduleList, Admin.Key);    
            }
            else
            {
                ScheduleList = new ObservableCollection<ScheduleModel>
                    (LocalDbContex.GetSchedules());
                EventList = new ObservableCollection<EventModel>
                    (LocalDbContex.GetEvents());
                MainPage = new NavigationPage(new Dashboard());

                TeacherList = new ObservableCollection<Teacher>
                    (LocalDbContex.GetTeachers());
                StudentList = new ObservableCollection<Student>
                    (LocalDbContex.GetStudents());
                //RetakeStudentList = Admin.RetakeStudentList;
                BatchDetails = LocalDbContex.GetBatchDetails();
                if (Preferences.Get(PrefKeys.IsAdmin, false))
                {
                    Admin = LocalDbContex.GetAdminAsUser();
                    Admin.TeacherList = TeacherList;
                    Admin.StudentList = StudentList;
                    Admin.ScheduleList = ScheduleList;
                    Admin.EventList = EventList;
                    Admin.BatchDetails = BatchDetails;
                }
                else
                    Student = LocalDbContex.GetStudentAsUser();
                FetchOnce = true;
            }

        }
        private async void CheckConnection(object sender, ConnectivityChangedEventArgs e)
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
                if (FetchOnce)
                {
                    Admin = await FirebaseHandler.GetAdminAsync(
                        Preferences.Get(PrefKeys.AdminKey, ""));
                    TeacherList = Admin.TeacherList;
                    StudentList = Admin.StudentList;
                    ScheduleList = Admin.ScheduleList;
                    EventList = Admin.EventList;
                    RetakeStudentList = Admin.RetakeStudentList;
                    BatchDetails = Admin.BatchDetails;
                    FetchOnce = false;
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
        public static void UpdateAdminOrSync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                FirebaseHandler.UpdateAdmin(Admin);
            else
            {
                Preferences.Set(PrefKeys.IsSyncPending, true);
                LocalStorageEngine.SaveDataAsync(Admin, FileType.Admin);
                DependencyService.Get<Toast>().Show("No internet access. Sync pending.");
            }
        }
    }
}
