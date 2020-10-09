using ClassAid.DataContex;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Models;
using Xamarin.Essentials;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Student student;
        public AdditionalDetails(Student student)
        {
            this.student = student;
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
            student.TeamCode = teamCode.Text.Trim();
            KeyVault keyVault = await FirebaseHandler.ValidateTeamCode(student.TeamCode);
            if (keyVault != null)
            {
                student.IsAdmin = false;
                student.IsActive = false;
                student.Name = studentName.Text.Trim();
                student.Phone = phoneNumber.Text.Trim();
                student.ID = studentID.Text.Trim();
                var tempAdmin = await FirebaseHandler.GetAdminAsync(keyVault.AdminKey);
                tempAdmin.StudentList.Add(new Student()
                {
                    Name = student.Name,
                    Phone = student.Phone,
                    ID = student.ID,
                    Key = student.Key,
                    IsActive = student.IsActive
                });
                Preferences.Set(PrefKeys.IsLoggedIn, true);
                Preferences.Set(PrefKeys.AdminKey, tempAdmin.Key);
                Preferences.Set(PrefKeys.IsAdmin, false);
                Preferences.Set(PrefKeys.Key, student.Key);
                Application.Current.MainPage =
                    new NavigationPage(new Dashboard(student));
                student.AdminKey = keyVault.AdminKey;
                FirebaseHandler.UpdateAdmin(tempAdmin);
                FirebaseHandler.UpdateStudent(student);
                LocalDbContex.CreateTables();
                LocalDbContex.SaveUser(student);
                LocalDbContex.SaveBatchDetails(tempAdmin.BatchDetails);
            }
            else
            {
                resultText.Text = "Invalid team code. Please try again.";
            }
            activityIndicator.IsRunning = false;
        }
    }
}