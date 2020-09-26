using ClassAid.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEditor;
using TEditor.Abstractions;
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

        private async void t_Clicked(object sender, EventArgs e)
        {
            var toolbar = new ToolbarBuilder().AddBasic().AddH1();
            TEditorResponse response = await CrossTEditor.Current.ShowTEditor(null, toolbar);
            if (!string.IsNullOrEmpty(response.HTML))
                displayWebView.Source = new HtmlWebViewSource() { Html = response.HTML };
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