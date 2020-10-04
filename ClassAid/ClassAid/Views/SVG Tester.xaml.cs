using System;
using Plugin.FilePicker;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;
using Firebase.Storage;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using System.Diagnostics;
using ClassAid.Models.Users;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SVG_Tester : ContentPage
    {
        Shared User;
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

        private async void AddScheduleBtn_Clicked(object sender, EventArgs e)
        {
            ScheduleModel schedule = new ScheduleModel()
            {
                CourseCode = courseCode.Text,
                Subject = subjectName.Text
            };
            Shared data = await FirebaseHandler.GetUser(User.Key,true);            
            await FirebaseHandler.UpdateShit(schedule, data.Key);
        }
    }
}