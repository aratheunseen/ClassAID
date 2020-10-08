using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSchedulePage : ContentPage
    {
        public ViewSchedulePage(Admin user)
        {
            InitializeComponent();
            scheduleCollectionView.ItemsSource = user.ScheduleList;
        }
        public ViewSchedulePage(string adminKey)
        {
            InitializeComponent();
            var ScheduleModels = new ObservableCollection<ScheduleModel>();
            Action action =
                new Action(async () =>
                await FirebaseHandler.RealTimeConnection(
                    CollectionTables.ScheduleList, ScheduleModels, adminKey));
            action.Invoke();
        }
        private void DeleteSipeBtn_Invoked(object sender, EventArgs e)
        {

        }

        private void EditSwipeBtn_Invoked(object sender, EventArgs e)
        {
            
        }
    }
}