using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullEventPage : ContentPage
    {
        Student Student;
        public ObservableCollection<EventModel> GetAllEvents { get; set; }
        public FullEventPage(Student student)
        {
            GetAllEvents = new ObservableCollection<EventModel>(LocalDbContex.GetEvents());
            Student = student;
            InitializeComponent();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                eventRefreshView.IsRefreshing = true;
                Admin admin = await FirebaseHandler.GetAdminAsync(Student.AdminKey);

                GetAllEvents = admin.EventList;
                eventRefreshView.IsRefreshing = false;
                LocalDbContex.ClearTable(TableList.events);
                LocalDbContex.SaveEvents(admin.EventList);

                LocalDbContex.ClearTable(TableList.schedule);
                LocalDbContex.SaveSchedules(admin.ScheduleList);

                LocalDbContex.ClearTable(TableList.batchdetails);
                LocalDbContex.SaveBatchDetails(admin.BatchDetails);

                LocalDbContex.ClearTable(TableList.students);
                LocalDbContex.SaveStudents(admin.StudentList.Where(p => p.IsActive == true));

                LocalDbContex.ClearTable(TableList.teachers);
                LocalDbContex.SaveTeachers(admin.TeacherList);
            }
        }
    }
}