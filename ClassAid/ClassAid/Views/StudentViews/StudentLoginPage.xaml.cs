using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentLoginPage : ContentPage
    {
        public static Command TapCommand;
        public StudentLoginPage()
        {
            InitializeComponent();
            TapCommand = new Command(async () => await Navigation.PushAsync(new StudentRegestrationPage()));
            gst.Command = TapCommand;
        }
    }
}