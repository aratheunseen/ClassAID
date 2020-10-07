using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        private readonly Admin user;

        public AddEventPage()
        {
        }

        public AddEventPage(Admin user)
        {
            InitializeComponent();
            this.user = user;
        }

        private async void saveEvent_Clicked()
        {
            await Navigation.PopAsync();
            if (user.EventList == null)
            {
                user.EventList =
                    new System.Collections.ObjectModel.ObservableCollection<EventModel>();
            }
            user.EventList.Add(
                new EventModel()
                {
                    Title = eventTitle.Text,
                    Details = eventBody.Text
                });
            await FirebaseHandler.UpdateUser(user);
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(eventTitle.Text) ||
                string.IsNullOrWhiteSpace(eventBody.Text))
                saveEvent.Command = null;
            else
                saveEvent.Command = new Command(() => saveEvent_Clicked());
        }
    }
}
