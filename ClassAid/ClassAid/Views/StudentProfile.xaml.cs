using ClassAid.DataContex;
using ClassAid.Models.Users;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        public Shared User { get; }
        public StudentProfile(Shared user)
        {
            User = user;            
            InitializeComponent();            
            logoutBtn.Command = new Command(() => Logout());
        }
        private void Logout()
        {
            Preferences.Set(PrefKeys.isLoggedIn, false);
            LocalStorageEngine.ClearData(FileType.Shared);
            Application.Current.MainPage =
                new NavigationPage(new StartPage());
        }

        private void Name_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }
    }
}