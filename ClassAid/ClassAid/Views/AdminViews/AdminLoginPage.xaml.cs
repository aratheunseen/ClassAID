using ClassAid.DataContex;
using System;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;
using ClassAid.Models;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using System.Linq;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {
        public static Command TapCommand;
        public static bool isCliclable = false;
        public AdminLoginPage()
        {
            InitializeComponent();
            //privacyURI.Command = new Command(async () =>
            //await Launcher.OpenAsync(new Uri("https://mahmudx.com")));
            TapCommand = new Command(() => BtnAdd_Clicked());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            userName.Focus();
        }
        private async void BtnAdd_Clicked()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Admin admin = new Admin(userName.Text + "admin", userPass.Text)
                {
                    IsAdmin = true
                };
                activityIndicator.IsRunning = true;
                var tempAdmin = await FirebaseHandler.GetAdminAsync(admin.Key);
                if (tempAdmin == null || tempAdmin.Name == null || tempAdmin.BatchDetails == null)
                {
                    activityIndicator.IsRunning = false;
                    App.Admin = admin;
                    App.Admin.TeacherList = new ObservableCollection<Teacher>();
                    App.Admin.StudentList = new ObservableCollection<Student>();
                    App.Admin.ScheduleList = new ObservableCollection<ScheduleModel>();
                    App.Admin.EventList = new ObservableCollection<EventModel>();
                    App.Admin.BatchDetails = new BatchDetails();
                    await Navigation.PushAsync(
                        new AdditionalDetails());
                    FirebaseHandler.InsertAdmin(App.Admin);
                }
                else
                {
                    Preferences.Set(PrefKeys.IsLoggedIn, true);
                    Preferences.Set(PrefKeys.AdminKey, tempAdmin.Key);
                    Preferences.Set(PrefKeys.IsAdmin, true);
                    Preferences.Set(PrefKeys.Key, tempAdmin.Key);
                    activityIndicator.IsRunning = false;
                    App.Admin = tempAdmin;
                    LocalDbContex.CreateTables();
                    LocalDbContex.SaveUser(tempAdmin);
                    LocalDbContex.SaveEvents(App.Admin.EventList);
                    LocalDbContex.SaveSchedules(App.Admin.ScheduleList);

                    Application.Current.MainPage =
                        new NavigationPage(new Dashboard());
                    App.Admin.BatchDetails = tempAdmin.BatchDetails;
                    LocalDbContex.SaveBatchDetails(tempAdmin.BatchDetails);
                    LocalDbContex.SaveTeachers(tempAdmin.TeacherList);
                    LocalDbContex.SaveStudents(tempAdmin.StudentList.Where(p => p.IsActive == true));
                }
            }
            else
            {
                DependencyService.Get<Toast>().Show("No INTERNET connection.");
            }
        }
        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text) ||
                userName.Text.Length < 6 ||
                userPass.Text.Length < 6)

            {
                signInBtn.Command = null;
            }
            else
                signInBtn.Command = TapCommand;
        }
    }
}