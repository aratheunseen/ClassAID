
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
        private Admin admin { get; set; }
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
        public ICommand GoToAboutPageCommamd { get { return new Command(async () => await Navigation.PushAsync(new AboutPage())); } }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(
                    new ChatHub(Preferences.Get(PrefKeys.AdminKey, ""),
                    Preferences.Get(PrefKeys.IsAdmin, false) ? admin.Name : student.Name,
                    Preferences.Get(PrefKeys.IsAdmin, false) ? admin.ID : student.ID)));
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
                await Navigation.PushAsync(Preferences.Get(PrefKeys.IsAdmin, false) ?
                new ViewSchedulePage(admin) : new ViewSchedulePage(student.AdminKey)));
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
        public Dashboard(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            teamCode.Text = admin.TeamCode;
            firstEventBody.Text = this.admin.EventList[0].Details;
            secondEventBody.Text = this.admin.EventList[1].Details;
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
            firstEventBody.Text = ev[0].Details;
            secondEventBody.Text = ev[1].Details;
            var sc = LocalDbContex.GetSchedules().Where(p =>
            p.DayOfWeek == DateTime.Now.DayOfWeek);
            scheduleView.ItemsSource = sc;
        }
        private void InitializeData()
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                admin = LocalDbContex.GetAdmin();
                admin.TeacherList = new ObservableCollection<Teacher>(LocalDbContex.GetTeachers());
                string teamCode1 = admin.TeamCode;
                teamCode.Text = teamCode1;
                admin.ScheduleList = new ObservableCollection<ScheduleModel>(LocalDbContex.GetSchedules());
                admin.EventList = new ObservableCollection<EventModel>(LocalDbContex.GetEvents());

                scheduleView.ItemsSource = admin.ScheduleList.Where(p =>
                p.DayOfWeek == DateTime.Now.DayOfWeek);
            }
            else
            {
                student = LocalDbContex.GetUser();
                StudentInit();
            }
        }
    }
}

