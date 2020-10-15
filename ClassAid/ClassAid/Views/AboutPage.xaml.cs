using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            yearText.Text = DateTime.Now.Year.ToString();
            aboutHyperText.Command = new Command(async =>
            Launcher.OpenAsync(new Uri("https://classaid.pienteger.com/about")));
            privacyHyperText.Command = new Command(async =>
            Launcher.OpenAsync(new Uri("https://www.pienteger.com/privacy")));
            termsHyperText.Command = new Command(async =>
            Launcher.OpenAsync(new Uri("https://classaid.pienteger.com/toc")));
            helpHyperText.Command = new Command(async =>
            Launcher.OpenAsync(new Uri("https://support.pienteger.com/classaid")));
        }
    }
}