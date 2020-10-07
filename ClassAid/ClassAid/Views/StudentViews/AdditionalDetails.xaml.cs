using ClassAid.DataContex;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Models.Schedule;
using System.Collections.ObjectModel;
using ClassAid.Models;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Admin user;
        public AdditionalDetails(Admin user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(studentName.Text) &&
                !string.IsNullOrWhiteSpace(teamCode.Text) && 
                !string.IsNullOrWhiteSpace(studentID.Text))
            {
                studentSignIn.Command = new Command(() => Button_Clicked());
            }
            else
            {
                studentSignIn.Command = null;
            }
        }
        public async void Button_Clicked()
        {
            activityIndicator.IsRunning = true;
            user.IsAdmin = false;
            user.IsActive = false;
            user.Name = studentName.Text.Trim();
            user.TeamCode = teamCode.Text.Trim();
            user.Phone = phoneNumber.Text.Trim();
            user.ID = studentID.Text.Trim();
            KeyVault validate =
                await FirebaseHandler.ValidateTeamCode(user.TeamCode);
            if (validate != null)
            {
                var tempAdmin = await FirebaseHandler.GetUser(validate.AdminKey,true);
                
                if (tempAdmin.BatchDetails == null)
                    user.BatchDetails = new BatchDetails();
                else
                    user.BatchDetails = tempAdmin.BatchDetails;

                if (tempAdmin.TeacherList == null)
                    user.TeacherList = new ObservableCollection<Teacher>();
                else
                    user.TeacherList = tempAdmin.TeacherList;

                if (tempAdmin.StudentList == null)
                {
                    user.StudentList = new ObservableCollection<Student>();
                    tempAdmin.StudentList = new ObservableCollection<Student>();
                }
                else
                    user.StudentList = tempAdmin.StudentList; 

                if (tempAdmin.ScheduleList == null)
                    user.ScheduleList = new ObservableCollection<ScheduleModel>();
                else
                    user.ScheduleList = tempAdmin.ScheduleList;

                if (tempAdmin.EventList == null)
                    user.EventList = new ObservableCollection<EventModel>();
                else
                    user.EventList = tempAdmin.EventList;

                tempAdmin.StudentList.Add(new Student() 
                {
                    Name = user.Name,
                    Phone = user.Phone,
                    ID = user.ID,
                    Key = user.Key,
                    IsActive = user.IsActive
                });

                await FirebaseHandler.UpdateUser(tempAdmin);
                Application.Current.MainPage =
                    new NavigationPage(new Dashboard(user));
                user.AdminKey = validate.AdminKey;
                await FirebaseHandler.UpdateUser(user);
            }
            else
            {
                resultText.Text = "Invalid team code. Please try again.";
            }
            activityIndicator.IsRunning = false;
        }

        private void studentName_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}