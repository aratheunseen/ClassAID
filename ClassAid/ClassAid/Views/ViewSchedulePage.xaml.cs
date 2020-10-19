using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSchedulePage : ContentPage
    {
        public ViewSchedulePage()
        {
            InitializeComponent();
            scheduleCollectionView.ItemsSource =
                App.Admin.ScheduleList.OrderBy(p => p.DayOfWeek);
            if (Preferences.Get(PrefKeys.IsAdmin, false))
                addScheduleBtn.Clicked += AddScheduleBtn_Clicked;
            else
            {
                addScheduleBtn.IsVisible = false;
                mainGrid.Children.Remove(ScheduleBtnArea);
            }
        }

        private async void AddScheduleBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddSchedulePage());
        }

        private void DeleteSchedule_Button(object sender, EventArgs e)
        {
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                ImageButton button = sender as ImageButton;
                var d = (ScheduleModel)button.BindingContext;
                App.Admin.ScheduleList.Remove(d);
                LocalDbContex.DeleteSchedule(d);
                App.UpdateAdminOrSync(App.Admin);
            }
            else
                DependencyService.Get<Toast>().Show("You are not authorized.");
        }
    }
}