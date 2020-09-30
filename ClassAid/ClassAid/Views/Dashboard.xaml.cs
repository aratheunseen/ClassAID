using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Models;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private Shared user;
        private static string timeFormat = @"dd\:hh\:mm";
        public Dashboard(Shared user)
        {
            InitializeComponent();
            this.user = user;
            InitializeData();
        }
        // TODO: Remove this section on shipment
        // START
        public Dashboard()
        {
            InitializeComponent();
            FetchData();
        }
        private async void FetchData()
        {
            try
            {
                user = await LocalStorageEngine.ReadDataAsync<Shared>
                    (FileType.Shared);
            }
            catch (Exception)
            {                
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    string key = Preferences.Get("adminKey", "");
                    user = await FirebaseHandler
                    .GetAdmin(key,user.IsAdmin);
                }
                else
                {
                    DependencyService.Get<Toast>().Show("ERROR. Please connect to Internet to resolve the issue.");
                    Application.Current.MainPage = new StartPage();
                    return;
                }
                
            }
            InitializeData();
        }
        void Logout()
        {
            Preferences.Set(PrefKeys.isLoggedIn, false);
            LocalStorageEngine.ClearData(FileType.Admin);
            Application.Current.MainPage =
                new NavigationPage(new StartPage());
        }
        private void InitializeData()
        {
            if (!user.IsAdmin)
            {
                addScheduleBtnImage.IsVisible = false;
                addNoticeBtnImage.IsVisible = false;
            }
            if (user.ScheduleList == null)            
                user.ScheduleList =
                    new ObservableCollection<ScheduleModel>();
            
            if (user.EventList == null)            
                user.EventList = 
                    new ObservableCollection<EventModel>();

            if (user.TeacherList == null)
                user.TeacherList =
                    new ObservableCollection<Teacher>();

            if (user.StudentList == null)            
                user.StudentList = 
                    new ObservableCollection<Shared>();

            profileBtn.Command = new Command(() => Logout());
            InitLabel();

            addSchedule.Command =
                new Command(async () =>
                await Navigation.PushAsync(
                    new AddSchedulePage(user)));
            addNoticeBtn.Command =
                new Command(async () =>
                await Navigation.PushAsync(
                    new AddEventPage(user)));

            fullSchedule.Command =
                new Command(() => Navigation.PushAsync
                (new ViewSchedulePage(user)));
            fullNotice.Command =
                new Command(() => Navigation.PushAsync
                (new ViewEventPage(user)));

            // TODO: Remove this section on shipment
            // START
            user.ScheduleList.CollectionChanged += ScheduleList_CollectionChanged;
        }

        private void ScheduleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitLabel();
        }
        void InitLabel()
        {
            try
            {
                firstScheduleCourseCode.Text = user.ScheduleList[0].CourseCode;
                firstScheduleCourseName.Text = user.ScheduleList[0].Subject;
                firstScheduleStart.Text = user.ScheduleList[0].StartTime.ToString(timeFormat);
                firstScheduleEnd.Text = user.ScheduleList[0].EndTime.ToString(timeFormat);

                secondScheduleCourseCode.Text = user.ScheduleList[1].CourseCode;
                secondScheduleCourseName.Text = user.ScheduleList[1].Subject;
                secondScheduleStart.Text = user.ScheduleList[1].StartTime.ToString(timeFormat);
                secondScheduleEnd.Text = user.ScheduleList[1].EndTime.ToString(timeFormat);
            }
            catch (Exception) { }
        }
        // END
    }
}