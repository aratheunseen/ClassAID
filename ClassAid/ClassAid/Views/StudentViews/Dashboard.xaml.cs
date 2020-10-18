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
            
            Student student = LocalDbContex.GetStudentAsUser();
            Action action = new Action(async () =>
            {
                var admin = await FirebaseHandler.GetAdminAsync(student.AdminKey);
                firstEventTitle.Text = admin.EventList[0].Title.Trim();
                secondEventTitle.Text = admin.EventList[1].Title.Trim();
                var sc = LocalDbContex.GetSchedules().ToList();
                sc.AddRange(sc);
                sc.AddRange(sc);
                scheduleView.ItemsSource = sc;
                LocalDbContex.SaveUser(admin);
                LocalDbContex.SaveEvents(admin.EventList);
                LocalDbContex.SaveSchedules(admin.ScheduleList);
            });
            action.Invoke();
        }
    }
}