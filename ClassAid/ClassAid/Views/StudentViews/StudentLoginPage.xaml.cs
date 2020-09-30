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
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (true)
            {

            }
        }
    }
}