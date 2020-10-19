using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using ClassAid.Views.AdminViews;
using ClassAid.Views.AdminViews.Settings;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSchedulePage : ContentPage
    {
        public ObservableCollection<ScheduleModel> SaturdaySchedules
        {
            get
            {
                return App.Admin.ScheduleList;
            }
        }
        //public ObservableCollection<ScheduleModel> SundaySchedules { get; set; }
        //public ObservableCollection<ScheduleModel> MondaySchedules { get; set; }
        //public ObservableCollection<ScheduleModel> TuesdaySchedules { get; set; }
        //public ObservableCollection<ScheduleModel> WednesdaySchedules { get; set; }
        //public ObservableCollection<ScheduleModel> ThursdaySchedules { get; set; }
        //public ObservableCollection<ScheduleModel> FridaySchedules { get; set; }
        public ViewSchedulePage()
        {
            InitializeComponent();
            addScheduleBtn.Clicked += AddScheduleBtn_Clicked;

        }
        private async void AddScheduleBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddSchedulePage());
        }

        private void DeleteSchedule_Button(object sender, EventArgs e)
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                ImageButton button = sender as ImageButton;
                var d = (ScheduleModel)button.BindingContext;
                Navigation.PushAsync(new ConfirmDeletationPage(d));
            }
        }
    }
}