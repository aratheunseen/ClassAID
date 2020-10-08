using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        private readonly Admin admin;

        public AddEventPage()
        {
        }

        public AddEventPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private async void SaveEvent_Clicked()
        {
            await Navigation.PopAsync();
            if (admin.EventList == null)
            {
                admin.EventList =
                    new System.Collections.ObjectModel.ObservableCollection<EventModel>();
            }
            admin.EventList.Add(
                new EventModel()
                {
                    Title = eventTitle.Text,
                    Details = eventBody.Text,
                    Time = DateTime.Now.ToString(@"dd\:hh\:mm\t")
                });
            FirebaseHandler.UpdateAdmin(admin);
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(eventTitle.Text) ||
                string.IsNullOrWhiteSpace(eventBody.Text))
                saveEvent.Command = null;
            else
                saveEvent.Command = new Command(() => SaveEvent_Clicked());
        }
    }
}
