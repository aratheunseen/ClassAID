using ClassAid.Models.Users;
using System;
using Xamarin.Forms;
using ClassAid.Models;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Views.AdminViews.Settings;
using Firebase.Database;
using ClassAid.DataContex;
using ClassAid.Models.Engines;

namespace ClassAid.Views.AdminViews
{

    public partial class DashBoardPage : ContentPage
    {
        public static ObservableCollection<ScheduleModel> scheduleCores;
        public static ObservableCollection<Teacher> teachers;
        public static ObservableCollection<EventModel> eventModels;
        public static LocalAdminStorageContex LocalStorage;
        public static LocalAdminStorageContex Database
        {
            get
            {
                if (LocalStorage == null)
                {
                    LocalStorage = new LocalAdminStorageContex();
                }
                return LocalStorage;
            }
        }
        FirebaseClient client;
        private bool canExit = false;
        public Admin admin;
        public string RealName { get { return admin.Name; } }
        public DashBoardPage(LoginAuthModel authModel)
        {
            Action action = async () => admin = await Database.GetItemsAsync(authModel.key);
            action.Invoke();
            InitializeComponent();
            LoadVariables();
        }
        public DashBoardPage(Admin admin)
        {
            this.admin = admin;
            InitializeComponent();
            LoadVariables();
        }
        private void LoadVariables()
        {
            admin.ScheduleList = new ObservableCollection<ScheduleModel>();
            admin.EventList = new ObservableCollection<EventModel>();
            admin.TeacherList = new ObservableCollection<Teacher>();
            client = App.fireSharpClient.GetClient();

            userName.Text = admin.Name;
            userID.Text = admin.ID;

            teacherListView.ItemsSource = admin.TeacherList;
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