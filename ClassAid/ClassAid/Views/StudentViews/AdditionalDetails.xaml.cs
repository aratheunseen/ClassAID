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
        public AdditionalDetails()
        {
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
            App.Student.TeamCode = teamCode.Text.Trim();
            KeyVault keyVault = await FirebaseHandler.ValidateTeamCode(App.Student.TeamCode);
            if (keyVault != null)
            {
                App.Student.IsAdmin = false;
                App.Student.IsActive = false;
                App.Student.IsRejected = false;
                App.Student.Name = studentName.Text.Trim();
                App.Student.Phone = phoneNumber.Text.Trim();
                App.Student.ID = studentID.Text.Trim();

                App.Admin = await FirebaseHandler.GetAdminAsync(keyVault.AdminKey);
                App.BatchDetails = App.Admin.BatchDetails;

                App.Admin.StudentList.Add(new Student()
                {
                    Name = App.Student.Name,
                    Phone = App.Student.Phone,
                    ID = App.Student.ID,
                    Key = App.Student.Key,
                    IsActive = App.Student.IsActive,
                    IsRejected = App.Student.IsRejected
                });
                Preferences.Set(PrefKeys.IsLoggedIn, true);
                Preferences.Set(PrefKeys.AdminKey, App.Admin.Key);
                Preferences.Set(PrefKeys.IsAdmin, false);
                Preferences.Set(PrefKeys.Key, App.Student.Key);
                Application.Current.MainPage =
                    new NavigationPage(new StudentNotActivatedPage());
                //OneSignal.Current.SendTag("AdminKey", App.Student.AdminKey);
                App.Student.AdminKey = keyVault.AdminKey;
                App.UpdateAdminOrSync();
                FirebaseHandler.UpdateStudent(App.Student);

                LocalDbContex.CreateTables();
                LocalDbContex.SaveUser(App.Student);
                LocalDbContex.SaveUser(App.Admin);
                LocalDbContex.SaveBatchDetails(App.BatchDetails);
            }
            else
            {
                resultText.Text = "Invalid team code. Please try again.";
            }
            activityIndicator.IsRunning = false;
        }
    }
}