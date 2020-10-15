using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
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
        }
        private void DeleteSchedule_Button(object sender, EventArgs e)
        {
            if (Preferences.Get(PrefKeys.IsAdmin,false))
            {
                ImageButton button = sender as ImageButton;
                var d = (ScheduleModel)button.BindingContext;
                App.Admin.ScheduleList.Remove(d);
                LocalDbContex.DeleteSchedule(d);
                App.UpdateAdminOrSync();
            }
            else
                DependencyService.Get<Toast>().Show("You are not authorized.");
        }
    }
}