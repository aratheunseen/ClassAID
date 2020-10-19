using ClassAid.DataContex;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Models;
using Xamarin.Essentials;
using Com.OneSignal;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Student Student;
        public AdditionalDetails(Student student)
        {
            Student = student;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            studentName.Focus();
        }
        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(studentName.Text) &&
                !string.IsNullOrWhiteSpace(teamCode.Text) &&
                !string.IsNullOrWhiteSpace(phoneNumber.Text) &&
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
            Student.TeamCode = teamCode.Text.Trim();
            KeyVault keyVault = await FirebaseHandler.ValidateTeamCode(Student.TeamCode);
            if (keyVault != null)
            {
                Student.IsAdmin = false;
                Student.IsActive = false;
                Student.IsRejected = false;
                Student.Name = studentName.Text.Trim();
                Student.Phone = phoneNumber.Text.Trim();
                Student.ID = studentID.Text.Trim();

                Admin Admin = await FirebaseHandler.GetAdminAsync(keyVault.AdminKey);

                Admin.StudentList.Add(new Student()
                {
                    Name = Student.Name,
                    Phone = Student.Phone,
                    ID = Student.ID,
                    Key = Student.Key,
                    IsActive = Student.IsActive,
                    IsRejected = Student.IsRejected
                });
                Preferences.Set(PrefKeys.IsLoggedIn, true);
                Preferences.Set(PrefKeys.AdminKey, Admin.Key);
                Preferences.Set(PrefKeys.IsAdmin, false);
                Preferences.Set(PrefKeys.Key, Student.Key);
                Application.Current.MainPage = new StudentNotActivatedPage(Student, Admin);
                //OneSignal.Current.SendTag("AdminKey", Student.AdminKey);
                Student.AdminKey = keyVault.AdminKey;
                App.UpdateAdminOrSync(Admin);
                FirebaseHandler.UpdateStudent(Student);

                LocalDbContex.CreateTables();
                LocalDbContex.SaveUser(Student);
                LocalDbContex.SaveUser(Admin);
                LocalDbContex.SaveBatchDetails(Admin.BatchDetails);

                OneSignal.Current.SendTag("AdminKey", Admin.Key);
                OneSignal.Current.RegisterForPushNotifications();

            }
            else
            {
                resultText.Text = "Invalid team code. Please try again.";
            }
            activityIndicator.IsRunning = false;
        }
    }
}