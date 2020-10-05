using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewSchedulePage : ContentPage
    {
        public ViewSchedulePage(Shared user)
        {
            InitializeComponent();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {

        }
    }
}