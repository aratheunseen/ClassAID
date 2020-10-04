using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SVG_Tester : ContentPage
    {
        readonly Shared User;
        public SVG_Tester(Shared user)
        {
            User = user;
            InitializeComponent();            
            ObservableCollection<ScheduleModel> coll;
            coll = new ObservableCollection<ScheduleModel>();
            Connection(coll);
            ScheduleList.ItemsSource = coll;
        }

        private async void Connection(ObservableCollection<ScheduleModel> coll)
        {
            await FirebaseHandler.RealTimeConnection(
                    CollectionTables.ScheduleList, coll, User.Key);
        }

        private void AddScheduleBtn_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}