using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.Models;
using System.Windows.Input;
using System.Linq;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {

        #region Declaration
        public ICommand TeamCodeCopyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Clipboard.SetTextAsync(App.Admin.TeamCode);
                    DependencyService.Get<Toast>().Show("Team code copied.");
                });
            }
        }
        public ICommand GoToAboutPageCommamd { get { return new Command(async () => await Navigation.PushAsync(new AboutPage())); } }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(
                    new ChatHub(Preferences.Get(PrefKeys.AdminKey, ""),
                    Preferences.Get(PrefKeys.IsAdmin, false) ? App.Admin.Name : App.Student.Name,
                    Preferences.Get(PrefKeys.IsAdmin, false) ? App.Admin.ID : App.Student.ID)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage()));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddEventPage()));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(new StudentProfile()));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewSchedulePage()));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage()));
            }
        }
        #endregion
        public Dashboard()
        {
            App.Admin = LocalDbContex.GetAdminAsUser();
            App.Admin.EventList = new ObservableCollection<EventModel>(LocalDbContex.GetEvents());
            App.Admin.ScheduleList = new ObservableCollection<ScheduleModel>(LocalDbContex.GetSchedules());
            InitializeComponent();
            BindEventScheduleList();
        }
        private void BindEventScheduleList()
        {
            App.Admin.BatchDetails = LocalDbContex.GetBatchDetails();
            App.Admin.StudentList = new ObservableCollection<Student>(LocalDbContex.GetStudents());
            App.Admin.TeacherList = new ObservableCollection<Teacher>(LocalDbContex.GetTeachers());
            teamCode.Text = App.Admin.TeamCode;
            if (App.Admin.EventList != null && App.Admin.EventList.Count > 0)
            {
                firstEventTitle.Text = App.Admin.EventList[0].Title;
                try
                {
                    secondEventTitle.Text = App.Admin.EventList[1].Title;
                }
                catch (Exception) { }
            }
            scheduleView.ItemsSource = App.Admin.ScheduleList
                .Where(p => p.DayOfWeek == DateTime.Now.DayOfWeek);
            App.Admin.ScheduleList.CollectionChanged += ScheduleList_CollectionChanged;
            App.Admin.EventList.CollectionChanged += EventList_CollectionChanged;
        }

        private void EventList_CollectionChanged(object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            if (App.Admin.EventList != null && App.Admin.EventList.Count > 0)
            {
                int c = App.Admin.EventList.Count;
                firstEventTitle.Text = App.Admin.EventList[0].Title;
                try
                {
                    secondEventTitle.Text = App.Admin.EventList[1].Title;
                }
                catch (Exception) { }
            }
        }

        private void ScheduleList_CollectionChanged(object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            scheduleView.ItemsSource = App.Admin.ScheduleList
                .Where(p => p.DayOfWeek == DateTime.Now.DayOfWeek).OrderBy(p => p.StartTime);
        }
    }
}

