using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmDeletationPage : ContentPage
    {
        ScheduleModel ScheduleModel;
        public ConfirmDeletationPage(ScheduleModel schedule)
        {
            ScheduleModel = schedule;
            InitializeComponent();
        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            App.Admin.ScheduleList.Remove(ScheduleModel);
            LocalDbContex.DeleteSchedule(ScheduleModel);
            App.UpdateAdminOrSync(App.Admin); 
            Navigation.PopAsync();
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}