using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using ClassAid.CustomComponents;
using ClassAid.Droid.ComponentsRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClassAidDatePicker),typeof(CustomDatePickerRenderer)) ]
namespace ClassAid.Droid.ComponentsRenderer
{
    class CustomDatePickerRenderer : DatePickerRenderer
    {
        [System.Obsolete]
        public CustomDatePickerRenderer(Context context) : base(context)
        {

        }
        [System.Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Android.Graphics.Color placeHolderColor = new Android.Graphics.Color(200, 198, 196);
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackgroundDrawable(gd);
                //Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(placeHolderColor));

            }
        }
    }
}