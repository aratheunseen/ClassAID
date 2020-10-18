using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public Dashboard()
        {
            InitializeComponent();
            var sc = LocalDbContex.GetEvents().ToList();
            Student student = LocalDbContex.GetStudentAsUser();
            //Action action = new Action(async () =>
            //{
            //    var admin = await FirebaseHandler.GetAdminAsync(student.AdminKey);
            firstEventTitle.Text = sc[0].Title;
            secondEventTitle.Text = sc[1].Title;
            scheduleView.ItemsSource = LocalDbContex.GetSchedules();
            //    LocalDbContex.SaveUser(admin);
            //    LocalDbContex.SaveEvents(admin.EventList);
            //    LocalDbContex.SaveSchedules(admin.ScheduleList);
            //});
            //action.Invoke();
        }
    }
}