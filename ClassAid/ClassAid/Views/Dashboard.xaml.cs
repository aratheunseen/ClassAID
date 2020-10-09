using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models;
using System.Windows.Input;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private Admin admin;
        private Student student;
        public ObservableCollection<EventModel> EventModels { get; set; }
        public ObservableCollection<ScheduleModel> ScheduleModels { get; set; }
        private static readonly string timeFormat = @"dd\:hh\:mm";
        #region Declaration
        public ICommand TeamCodeCopyCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await Clipboard.SetTextAsync(admin.TeamCode);
                    DependencyService.Get<Toast>().Show("Team code copied.");
                });
            }
        }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new SVG_Tester(admin)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(admin)));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddEventPage(admin)));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync( Preferences.Get(PrefKeys.IsAdmin,false) ?
                    new StudentProfile(admin) : new StudentProfile(student)));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewSchedulePage(admin)));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage(admin)));
            }
        }
        #endregion
        public Dashboard(Admin user)
        {
            InitializeComponent();
            this.admin = user;
            teamCode.Text = user.TeamCode;
        }
        public Dashboard(Student student)
        {
            InitializeComponent();
            this.student = student;
            StudentInit();
        }
        private async void StudentInit()
        {
            EventModels.CollectionChanged += EventModels_CollectionChanged;
            ScheduleModels.CollectionChanged += ScheduleModels_CollectionChanged;
            addScheduleBtnImage.IsVisible =
                addNoticeBtnImage.IsVisible =
                teamCodeBox.IsVisible = false;
            await FirebaseHandler.RealTimeConnection(CollectionTables.EventList, EventModels, student.AdminKey);
            await FirebaseHandler.RealTimeConnection(CollectionTables.ScheduleList, ScheduleModels, student.AdminKey);
        }

        private void ScheduleModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveSchedules(ScheduleModels[0]);
        }

        private void EventModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveEvents(EventModels[0]);
        }

        public Dashboard()
        {
            InitializeComponent();
            InitializeData();
        }

        private async void InitializeData()
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                Admin admin =
                    await FirebaseHandler.GetAdminAsync(Preferences.Get(PrefKeys.Key, ""));
                this.admin = admin;
                teamCode.Text = admin.TeamCode;
            }
            else
            {
                Student student =
                    await FirebaseHandler.GetStudentAsync(Preferences.Get(PrefKeys.Key, ""));
                this.student = student;
                StudentInit();
            }
        }
        //  END
    }
}

