using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SVG_Tester : ContentPage
    {
        public SVG_Tester()
        {
            InitializeComponent();
            Intent intent = new Intent();
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {                
                string url = await FirebaseStorageHandler.SaveFile(file.FilePath);
                lbl.Text = url;
            }
        }
    }
}