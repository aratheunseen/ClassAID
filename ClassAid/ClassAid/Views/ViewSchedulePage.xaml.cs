using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSchedulePage : ContentPage
    {
        public IValueConverter Converter { get; set; }
        private string _adminkey { get; set; }
        private static ObservableCollection<ScheduleModel> ScheduleList { get; set; }
        private static ObservableCollection<ScheduleModel> ScheduleListForStudent { get; set; }
        private static Admin Admin { get; set; }
        public bool IsAdmin { get; set; }
        public ViewSchedulePage(Admin user)
        {
            InitializeComponent();
            IsAdmin = true;
            Admin = user;
            ScheduleList = new ObservableCollection<ScheduleModel>(LocalDbContex.GetSchedules());
            scheduleCollectionView.ItemsSource = ScheduleList.OrderBy(p => p.DayOfWeek); ;
        }
        public ViewSchedulePage(string adminKey)
        {
            InitializeComponent();
            IsAdmin = false;
            _adminkey = adminKey;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            ScheduleListForStudent = new ObservableCollection<ScheduleModel>
                (LocalDbContex.GetSchedules());
            scheduleCollectionView.ItemsSource = 
                ScheduleListForStudent.OrderBy(p=>p.DayOfWeek);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Action action =
                    new Action(async () => {
                        var tempAdmin = await FirebaseHandler.GetAdminAsync(adminKey);
                        LocalDbContex.ClearTable(TableList.schedule);
                        LocalDbContex.SaveSchedules(tempAdmin.ScheduleList);
                        ScheduleListForStudent = tempAdmin.ScheduleList;
                        await FirebaseHandler.RealTimeConnection(
                            CollectionTables.ScheduleList, ScheduleListForStudent, adminKey);
                    });
                action.Invoke();
            }
        }
        private void Connectivity_ConnectivityChanged(object sender,
            ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Action action =
                    new Action(async () => {
                        var tempAdmin = await FirebaseHandler.GetAdminAsync(_adminkey);
                        LocalDbContex.ClearTable(TableList.schedule);
                        LocalDbContex.SaveSchedules(tempAdmin.ScheduleList);
                        ScheduleListForStudent = tempAdmin.ScheduleList;
                        await FirebaseHandler.RealTimeConnection(
                            CollectionTables.ScheduleList, ScheduleListForStudent, _adminkey);
                    });
                action.Invoke();
            }
        }

        private void DeleteSchedule_Button(object sender, EventArgs e)
        {
            if (IsAdmin)
            {
                ImageButton button = sender as ImageButton;
                var d = (ScheduleModel)button.BindingContext;
                ScheduleList.Remove(d);
                LocalDbContex.DeleteSchedule(d);
                Admin.ScheduleList = new ObservableCollection<ScheduleModel>(ScheduleList);
                FirebaseHandler.UpdateAdmin(Admin);
            }
            else
                DependencyService.Get<Toast>().Show("You are not authorized.");
        }
    }
}