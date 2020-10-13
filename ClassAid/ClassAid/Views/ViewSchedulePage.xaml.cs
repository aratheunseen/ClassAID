using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
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
        private string _adminkey;
        public bool IsAdmin { get; set; }
        public ViewSchedulePage(Admin user)
        {
            InitializeComponent();
            IsAdmin = true;
            //scheduleCollectionView.ItemsSource = user.ScheduleList;
            TempScheduleData scheduleData = new TempScheduleData()
            {
                IsAdmin = true,
                schedules = LocalDbContex.GetSchedules()
            };
            scheduleCollectionView.ItemsSource = LocalDbContex.GetSchedules();
        }
        public ViewSchedulePage(string adminKey)
        {
            InitializeComponent();
            IsAdmin = false;
            _adminkey = adminKey;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            scheduleCollectionView.ItemsSource = LocalDbContex.GetSchedules();
            //if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            //{
            //    var ScheduleModels = new ObservableCollection<ScheduleModel>();
            //    Action action =
            //        new Action(async () =>
            //        await FirebaseHandler.RealTimeConnection(
            //            CollectionTables.ScheduleList, ScheduleModels, adminKey));
            //    action.Invoke();
            //}
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            scheduleCollectionView.ItemsSource = LocalDbContex.GetSchedules();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var ScheduleModels = new ObservableCollection<ScheduleModel>();
                Action action =
                    new Action(async () =>
                    await FirebaseHandler.RealTimeConnection(
                        CollectionTables.ScheduleList, ScheduleModels, _adminkey));
                action.Invoke();
            }
        }

        private void DeleteSchedule_Button(object sender, EventArgs e)
        {
            if (IsAdmin)
            {
                // TODO : Impliment func
            }
            else
                DependencyService.Get<Toast>().Show("You are not authorized.");
        }

        private void EditSchedule_Invoked(object sender, EventArgs e)
        {
            if (IsAdmin)
            {
                // TODO : Impliment func
                
            }
            else
                DependencyService.Get<Toast>().Show("You are not authorized.");
        }
    }
    public class TempScheduleData
    {
        public bool IsAdmin { get; set; }
        public IEnumerable<ScheduleModel> schedules { get; set; }
    }
}