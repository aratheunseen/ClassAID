using ClassAid.Models.Schedule;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        ObservableCollection<EventModel> eventModels;
        public AddEventPage(ObservableCollection<EventModel> eventModels)
        {
            InitializeComponent();
            this.eventModels = eventModels;
        }        

        private void saveEvent_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            eventModels.Add(
                new EventModel() 
                {
                    Title = eventTitle.Text,
                    Details = eventBody.Text
                });
        }
    }
}