using System;
using Plugin.FilePicker;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;
using Firebase.Storage;
using System.Collections.ObjectModel;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SVG_Tester : ContentPage
    {
        public SVG_Tester()
        {
            InitializeComponent();
            ObservableCollection<string> coll = new ObservableCollection<string>();
            coll.Add("Hello!");
            Action ac = (async()=> await FirebaseHandler.ListOfAdmin(coll));
            ac.Invoke();
            adminCodeList.ItemsSource = coll;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null)
            {
                //string url = await FirebaseStorageHandler.SaveFile(file.GetStream());
                string url = await new FirebaseStorage("gs://classaidapp.appspot.com")
                .Child("data")
                .PutAsync(file.GetStream());
                lbl.Text = url;
            }

            //var result = await FilePicker.PickAsync();
            //if (result != null)
            //{
            //    Text = $"File Name: {result.FileName}";
            //    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
            //        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            //    {
            //        var stream = await result.OpenReadAsync();
            //        Image = ImageSource.FromStream(() => stream);
            //    }
            //}
        }
    }
}