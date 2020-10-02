using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Shared user;
        public AdditionalDetails(Shared user)
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
            user.Name = studentName.Text.Trim();
            user.TeamCode = teamCode.Text.Trim();
            user.Phone = phoneNumber.Text.Trim();
            user.ID = studentID.Text.Trim();
            string validate =
                await FirebaseHandler.ValidateTeamCode(user.TeamCode);

            if (!string.IsNullOrWhiteSpace(validate))
            {
                var tempAdmin = await FirebaseHandler.GetUser(null, true, user.TeamCode);
                try
                {
                    user.BatchDetails = tempAdmin.BatchDetails;
                    user.TeacherList = tempAdmin.TeacherList;
                    user.StudentList = tempAdmin.StudentList;
                    user.ScheduleList = tempAdmin.ScheduleList;
                    user.EventList = tempAdmin.EventList;
                    tempAdmin.StudentList.Add(new Student() 
                    {
                        Name = user.Name,
                        Phone = user.Phone,
                        ID = user.ID
                    });
                    await FirebaseHandler.UpdateUser(tempAdmin);
                }
                catch (Exception) { }
                Application.Current.MainPage =
                    new NavigationPage(new Dashboard(user));
                
                await FirebaseHandler.UpdateUser(user);
            }
            else
            {
                resultText.Text = "Invalid team code. Please try again.";
            }
            activityIndicator.IsRunning = false;
        }
    }
}