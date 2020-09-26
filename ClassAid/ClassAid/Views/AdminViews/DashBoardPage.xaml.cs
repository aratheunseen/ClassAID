using ClassAid.Models.Users;
using System;
using Xamarin.Forms;
using ClassAid.Models;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Views.AdminViews.Settings;
using Firebase.Database;
using ClassAid.DataContex;

namespace ClassAid.Views.AdminViews
{

    public partial class DashBoardPage : ContentPage
    {
        public static ObservableCollection<ScheduleModel> scheduleCores;
        public static ObservableCollection<Teacher> teachers;
        public static ObservableCollection<EventModel> eventModels;
        FirebaseClient client;
        private bool canExit = false;
        public Admin admin;
        public string RealName { get { return admin.Name; } }
        public DashBoardPage(Admin admin, FirebaseClient client)
        {
            scheduleCores = new ObservableCollection<ScheduleModel>();
            eventModels = new ObservableCollection<EventModel>();
            InitializeComponent();
            Action action = new Action(async ()=> admin = await AdminDbHandler.RealTimeConnection<Admin>(client, "Admin", admin));
            action.Invoke();
            teachers = new ObservableCollection<Teacher>();
            admin.teacherList = new ObservableCollection<Teacher>();
            this.client = client;
            this.admin = admin;
            userName.Text = admin.Name;
            userMail.Text = admin.Email;
            userPhone.Text = admin.Phone.ToString();
            userID.Text = admin.ID;

            teacherListView.ItemsSource = admin.teacherList;
            scheduleView.ItemsSource = admin.ScheduleList;
            eventsListView.ItemsSource = admin.EventList;
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
            Navigation.PushAsync(new AddTeacherPage(admin,client));
        }

        private void scheduleAddBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSchedulePage(admin, client));
        }

        private void eventAddBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddEventPage(admin, client));
        }
    }
}