using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.Models;
using System.Windows.Input;
using System.Linq;
using System;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {

        #region Declaration
        public ICommand TeamCodeCopyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Clipboard.SetTextAsync(App.Admin.TeamCode);
                    DependencyService.Get<Toast>().Show("Team code copied.");
                });
            }
        }
        public ICommand GoToAboutPageCommamd { get { return new Command(async () => await Navigation.PushAsync(new AboutPage())); } }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(
                    new ChatHub(Preferences.Get(PrefKeys.AdminKey, ""),
                    Preferences.Get(PrefKeys.IsAdmin, false) ? App.Admin.Name : App.Student.Name,
                    Preferences.Get(PrefKeys.IsAdmin, false) ? App.Admin.ID : App.Student.ID)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage()));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddEventPage()));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(new StudentProfile()));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewSchedulePage()));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage()));
            }
        }
        #endregion
        public Dashboard()
        {
            InitializeComponent();
            BindEventScheduleList();
            if (!Preferences.Get(PrefKeys.IsAdmin,false))
            {
                mainGrid.Children.Remove(addScheduleBtnImage);
                mainGrid.Children.Remove(addScheduleBtnImage);
                mainGrid.Children.Remove(teamCodeBox);
            }
            else
                teamCode.Text = App.Admin.TeamCode;
        }
        private void BindEventScheduleList()
        {
            if (App.EventList != null && App.EventList.Count > 0)
            {
                firstEventBody.Text = App.EventList[0].Details;
                try
                {
                    secondEventBody.Text = App.EventList[1].Details;
                }
                catch (Exception) { }
            }
            scheduleView.ItemsSource = App.ScheduleList
                .Where(p => p.DayOfWeek == DateTime.Now.DayOfWeek);
        }
    }
}

