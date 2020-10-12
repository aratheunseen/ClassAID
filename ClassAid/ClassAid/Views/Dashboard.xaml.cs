using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models;
using System.Windows.Input;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Diagnostics;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private Admin admin;
        private Student student;
        public ObservableCollection<EventModel> EventModels { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleModels { get; set; }
        
        #region Declaration
        public ICommand TeamCodeCopyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Clipboard.SetTextAsync(admin.TeamCode);
                    DependencyService.Get<Toast>().Show("Team code copied.");
                });
            }
        }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ChatHub(Preferences.Get(PrefKeys.AdminKey, ""), Preferences.Get(PrefKeys.IsAdmin, false) ? admin.Name : student.Name)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(admin)));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddEventPage(admin)));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(Preferences.Get(PrefKeys.IsAdmin, false) ?
                    new StudentProfile(admin) : new StudentProfile(student)));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewSchedulePage(admin)));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage(admin)));
            }
        }
        #endregion
        public Dashboard(Admin user)
        {
            InitializeComponent();
            this.admin = user;
            teamCode.Text = user.TeamCode;
        }
        public Dashboard(Student student)
        {
            InitializeComponent();
            this.student = student;

            StudentInit();
        }
        public Dashboard()
        {
            InitializeComponent();
            InitializeData();
        }
        private async void StudentInit()
        {
            if (student.RetakeModels != null)
                foreach (var item in student.RetakeModels)
                {
                    if (item.IsActive)
                    {
                        await FirebaseHandler.RealTimeConnection(CollectionTables.EventList, EventModels, item.AdminKey);
                        await FirebaseHandler.RealTimeConnection(CollectionTables.ScheduleList, ScheduleModels, item.AdminKey);
                    }
                }

            addScheduleBtnImage.IsVisible =
                addNoticeBtnImage.IsVisible =
                teamCodeBox.IsVisible = false;
            if (EventModels == null)
                EventModels = new ObservableCollection<EventModel>();
            if (ScheduleModels == null)
                ScheduleModels = new ObservableCollection<ScheduleModel>();
            Debug.WriteLine("Here");
            Debug.WriteLine(student.AdminKey);
            await FirebaseHandler.RealTimeConnection(CollectionTables.EventList, EventModels, student.AdminKey);
            await FirebaseHandler.RealTimeConnection(CollectionTables.ScheduleList, ScheduleModels, student.AdminKey);
            EventModels.CollectionChanged += EventModels_CollectionChanged;
            ScheduleModels.CollectionChanged += ScheduleModels_CollectionChanged;
        }
        private void InitializeData()
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                admin = LocalDbContex.GetAdmin();
                string teamCode1 = admin.TeamCode;
                teamCode.Text = teamCode1;
                admin.BatchDetails = LocalDbContex.GetBatchDetails();
                admin.StudentList = new ObservableCollection<Student>(LocalDbContex.GetStudents());
                admin.ScheduleList = new ObservableCollection<ScheduleModel>(LocalDbContex.GetSchedules());
                admin.TeacherList = new ObservableCollection<Teacher>(LocalDbContex.GetTeachers());
                admin.EventList = new ObservableCollection<EventModel>(LocalDbContex.GetEvents());
            }
            else
            {
                student = LocalDbContex.GetUser();
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    //student = await FirebaseHandler.GetStudentAsync(Preferences.Get(PrefKeys.Key, ""));
                    StudentInit();
                }
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            }
        }
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            student = await FirebaseHandler.GetStudentAsync(Preferences.Get(PrefKeys.Key, ""));
            StudentInit();
        }
        private void ScheduleModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveSchedule(ScheduleModels[0]);
        }
        private void EventModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveEvent(EventModels[0]);
        }
    }
}

