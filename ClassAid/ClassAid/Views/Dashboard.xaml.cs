
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
using System.Linq;
using System;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private Admin Admin { get; set; }
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
                    await Clipboard.SetTextAsync(Admin.TeamCode);
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
                    Preferences.Get(PrefKeys.IsAdmin, false) ? Admin.Name : student.Name,
                    Preferences.Get(PrefKeys.IsAdmin, false) ? Admin.ID : student.ID)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(Admin)));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddEventPage(Admin)));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(Preferences.Get(PrefKeys.IsAdmin, false) ?
                    new StudentProfile(Admin) : new StudentProfile(student)));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(Preferences.Get(PrefKeys.IsAdmin, false) ?
                new ViewSchedulePage(Admin) : new ViewSchedulePage(student.AdminKey)));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage(Admin)));
            }
        }
        #endregion
        public Dashboard(Admin admin)
        {
            InitializeComponent();
            this.Admin = admin;
            teamCode.Text = admin.TeamCode;
            BindEventList();
        }
        public Dashboard(Student student)
        {
            InitializeComponent();
            this.student = student;
            StudentInit();
            BindEventList();
        }
        public Dashboard()
        {
            InitializeComponent();
            InitializeData();
            BindEventList();
        }
        private async void StudentInit()
        {
            mainGrid.Children.Remove(addScheduleBtnImage);
            mainGrid.Children.Remove(addScheduleBtnImage);
            mainGrid.Children.Remove(teamCodeBox);
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var tempAdmin = await FirebaseHandler.GetAdminAsync(student.AdminKey);
                LocalDbContex.ClearTable(TableList.students);
                LocalDbContex.SaveStudents(tempAdmin.StudentList);
                LocalDbContex.ClearTable(TableList.teachers);
                LocalDbContex.SaveTeachers(tempAdmin.TeacherList);
                LocalDbContex.ClearTable(TableList.batchdetails);
                LocalDbContex.SaveBatchDetails(tempAdmin.BatchDetails);
            }
            var ev = LocalDbContex.GetEvents().ToList();
            
            var sc = LocalDbContex.GetSchedules().Where(p =>
            p.DayOfWeek == DateTime.Now.DayOfWeek);
            scheduleView.ItemsSource = sc;
        }
        private void InitializeData()
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                Admin = LocalDbContex.GetAdmin();
                Admin.TeacherList = new ObservableCollection<Teacher>(LocalDbContex.GetTeachers());
                string teamCode = Admin.TeamCode;
                this.teamCode.Text = teamCode;
                Admin.ScheduleList = new ObservableCollection<ScheduleModel>(LocalDbContex.GetSchedules());
                Admin.EventList = new ObservableCollection<EventModel>(LocalDbContex.GetEvents());
                scheduleView.ItemsSource = Admin.ScheduleList.Where(p =>
                p.DayOfWeek == DateTime.Now.DayOfWeek);
            }
            else
            {
                student = LocalDbContex.GetUser();
                StudentInit();
            }
        }
        private void BindEventList()
        {
            firstEventBody.Text = Admin.EventList[0].Details;
            secondEventBody.Text = Admin.EventList[1].Details;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
    }
}

