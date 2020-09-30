using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private static Shared shared;
        private Admin admin;
        private Student student;
        private static string timeFormat = @"dd\:hh\:mm";

        public Dashboard(Admin admin)
        {
            InitializeComponent();
            shared = admin;
            this.admin = admin;
            InitializeData();
            addSchedule.Command =
                new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(admin)));
            addNoticeBtn.Command =
                new Command(async () =>
                await Navigation.PushAsync(new AddEventPage(admin)));

        }
        public Dashboard(Student student)
        {
            InitializeComponent();
            addNoticeImage.IsVisible = false;
            shared = student;
            this.student = student;
            InitializeData();
        }
        // TODO: Remove this section on shipment
        // START
        public Dashboard()
        {
            InitializeComponent();
            addSchedule.Command =
                new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(admin)));
            addNoticeBtn.Command =
                new Command(async () =>
                await Navigation.PushAsync(new AddEventPage(admin)));
            FetchData();
        }
        private async void FetchData()
        {
            // TODO: Replace this with local storage.
            string key = Preferences.Get("adminKey", "");
            admin = 
                await AdminDbHandler
                .GetAdmin(
                    App.fireSharpClient.GetClient(), key);
            shared = admin;
            InitializeData();
        }
        // END
        void Logout()
        {
            Preferences.Set("isLoggedin", "false");
            Application.Current.MainPage =
                new NavigationPage(new StartPage());
        }
        private void InitializeData()
        {
            if (shared.ScheduleList == null)
            {
                shared.ScheduleList = 
                    new ObservableCollection<ScheduleModel>();
            }
            profileBtn.Command = new Command(() => Logout());
            try
            {
                InitLabel();
            }
            catch (Exception)
            {

            }
            // TODO: Remove this section on shipment
            // START
            shared.ScheduleList.CollectionChanged += ScheduleList_CollectionChanged;
        }

        private void ScheduleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitLabel();
        }
        void InitLabel()
        {
            firstScheduleCourseCode.Text = shared.ScheduleList[0].CourseCode;
            firstScheduleCourseName.Text = shared.ScheduleList[0].Subject;
            firstScheduleStart.Text = shared.ScheduleList[0].StartTime.ToString(timeFormat);
            firstScheduleEnd.Text = shared.ScheduleList[0].EndTime.ToString(timeFormat);

            secondScheduleCourseCode.Text = shared.ScheduleList[1].CourseCode;
            secondScheduleCourseName.Text = shared.ScheduleList[1].Subject;
            secondScheduleStart.Text = shared.ScheduleList[1].StartTime.ToString(timeFormat);
            secondScheduleEnd.Text = shared.ScheduleList[1].EndTime.ToString(timeFormat);
        }
        // END
    }
}