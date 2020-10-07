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
using System.Diagnostics;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private Admin user;
        private static readonly string timeFormat = @"dd\:hh\:mm";
        #region Declaration
        public ICommand TeamCodeCopyCommand 
        { 
            get 
            { 
                return new Command(async () =>
                {
                    await Clipboard.SetTextAsync(user.TeamCode);
                    DependencyService.Get<Toast>().Show("Team code copied.");
                });
            } 
        }
        public ICommand GroupMessageCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new SVG_Tester(user)));
            }
        }
        public ICommand AddScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new AddSchedulePage(user)));
            }
        }
        public ICommand AddEventCommand
        {
            get
            {
                return new Command(async ()=>
                await Navigation.PushAsync(new AddEventPage(user)));
            }
        }
        public ICommand ProfileBtnCommand
        {
            get
            {
                return new Command(() =>
                Navigation.PushAsync(new StudentProfile(user)));
            }
        }
        public ICommand FullScheduleCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewSchedulePage(user)));
            }
        }
        public ICommand FullEventCommand
        {
            get
            {
                return new Command(async () =>
                await Navigation.PushAsync(new ViewEventPage(user)));
            }
        }
        #endregion
        public Dashboard(Admin user)
        {
            this.user = user;
            InitializeComponent();
            InitializeData();
        }
        // TODO: Remove this section on shipment
        // START
        public Dashboard()
        {
            //FetchData();
            InitializeComponent();
            InitializeData();
        }

        private async void InitializeData()
        {
            if (user.ScheduleList == null)
                user.ScheduleList =
                    new ObservableCollection<ScheduleModel>();

            if (user.EventList == null)
                user.EventList =
                    new ObservableCollection<EventModel>();

            if (user.TeacherList == null)
                user.TeacherList =
                    new ObservableCollection<Teacher>();

            if (user.StudentList == null)
                user.StudentList =
                    new ObservableCollection<Student>();
            if (!user.IsAdmin)
            {
                addScheduleBtnImage.IsVisible = false;
                addNoticeBtnImage.IsVisible = false;
                teamCodeBox.IsVisible = false;

                await FirebaseHandler.RealTimeConnection(
                    CollectionTables.ScheduleList,
                    user.ScheduleList, user.AdminKey);
                await FirebaseHandler.RealTimeConnection(
                    CollectionTables.StudentList,
                    user.StudentList, user.AdminKey);
                await FirebaseHandler.RealTimeConnection(
                    CollectionTables.EventList,
                    user.EventList, user.AdminKey);
                await FirebaseHandler.RealTimeConnection(
                    CollectionTables.TeacherList,
                    user.TeacherList, user.AdminKey);
            }
            InitLabel();
            // TODO: Remove this section on shipment
            // START
            user.ScheduleList.CollectionChanged += ScheduleList_CollectionChanged;
        }

        private void ScheduleList_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InitLabel();
        }
        void InitLabel()
        {
            try
            {
                teamCode.Text = user.TeamCode;
                firstScheduleCourseCode.Text = user.ScheduleList[0].CourseCode;
                firstScheduleCourseName.Text = user.ScheduleList[0].Subject;
                firstScheduleStart.Text = user.ScheduleList[0].StartTime.ToString(timeFormat);
                firstScheduleEnd.Text = user.ScheduleList[0].EndTime.ToString(timeFormat);

                secondScheduleCourseCode.Text = user.ScheduleList[1].CourseCode;
                secondScheduleCourseName.Text = user.ScheduleList[1].Subject;
                secondScheduleStart.Text = user.ScheduleList[1].StartTime.ToString(timeFormat);
                secondScheduleEnd.Text = user.ScheduleList[1].EndTime.ToString(timeFormat);
            }
            catch (Exception) { }
        }
        //  END
    }
}

