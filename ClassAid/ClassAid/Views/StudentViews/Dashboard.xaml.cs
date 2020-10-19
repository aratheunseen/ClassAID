using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private readonly Student Student;
        #region NavigationBinding
        public ICommand GoToAboutPageCommamd { get { return new Command(async () => await Navigation.PushAsync(new AboutPage())); } }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(
                    new ChatHub(Student.AdminKey, Student.Name, Student.ID)));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(new Profile()));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new FullSchedulePage()));
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
            InitializeComponent();

            Student = LocalDbContex.GetStudentAsUser();
            var el = LocalDbContex.GetEvents().ToList();
            if (el.Count() != 0)
            {
                firstEventTitle.Text = el[0].Title.Trim();
                try { secondEventTitle.Text = el[1].Title.Trim(); }
                catch (Exception) { }
            }
            var sc = LocalDbContex.GetSchedules().ToList();
            sc.AddRange(sc);
            sc.AddRange(sc);
            scheduleView.ItemsSource = sc;

            FetchData(Student);
        }
        private async void FetchData(Student student)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Admin admin = await FirebaseHandler.GetAdminAsync(student.AdminKey);

                if (admin.EventList.Count() != 0)
                {
                    firstEventTitle.Text = admin.EventList[0].Title.Trim();
                    try { secondEventTitle.Text = admin.EventList[1].Title.Trim(); }
                    catch (Exception) { }
                }
                scheduleView.ItemsSource = admin.ScheduleList;

                LocalDbContex.ClearTable(TableList.events);
                LocalDbContex.SaveEvents(admin.EventList);

                LocalDbContex.ClearTable(TableList.schedule);
                LocalDbContex.SaveSchedules(admin.ScheduleList);

                LocalDbContex.ClearTable(TableList.batchdetails);
                LocalDbContex.SaveBatchDetails(admin.BatchDetails);

                LocalDbContex.ClearTable(TableList.students);
                LocalDbContex.SaveStudents(admin.StudentList);

                LocalDbContex.ClearTable(TableList.teachers);
                LocalDbContex.SaveTeachers(admin.TeacherList);
            }
        }
    }
}