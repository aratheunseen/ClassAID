using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Views;

namespace ClassAid
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new StartPage();
            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
