using ClassAid.Models.Users;
using System;
using Xamarin.Forms;
using ClassAid.Models;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Views.AdminViews.Settings;

namespace ClassAid.Views.AdminViews
{

    public partial class DashBoardPage : ContentPage
    {
        public static ObservableCollection<ScheduleModel> scheduleCores;
        public static ObservableCollection<Teacher> teachers;
        public static ObservableCollection<EventModel> eventModels;

        private bool canExit = false;
        public Admin admin;
        public string RealName { get { return admin.Name; } }
        public DashBoardPage(Admin admin)
        {
            scheduleCores = new ObservableCollection<ScheduleModel>();
            eventModels = new ObservableCollection<EventModel>();
            InitializeComponent();

            teachers = new ObservableCollection<Teacher>()
            {
                new Teacher(){Name="Nahid Hasan", Designation="Lect"},
                new Teacher(){Name="Shahriar Saif", Designation="Lect"},
                new Teacher(){Name="Sarwar Parvez", Designation="Ast. Proff"}
            };

            
            this.admin = admin;
            userName.Text = admin.Name;
            userMail.Text = admin.Email;
            userPhone.Text = admin.Phone.ToString();
            userID.Text = admin.ID;

            teacherListView.ItemsSource = teachers;
            scheduleView.ItemsSource = scheduleCores;
            eventsListView.ItemsSource = eventModels;
        }
        protected override bool OnBackButtonPressed()
        {            
            if (!canExit)
            {
                DependencyService.Get<Toast>().Show("Press again to Exit.");
                canExit = true;
            }
            else
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }


        private void scheduleRefreshView_Refreshing(object sender, EventArgs e)
        {
           scheduleRefreshView.IsRefreshing = false;
        }

        private void teacherAddBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTeacherPage(teachers));
        }

        private void scheduleAddBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSchedulePage(scheduleCores, teachers));
        }

        private void eventAddBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddEventPage(eventModels));
        }
    }
}