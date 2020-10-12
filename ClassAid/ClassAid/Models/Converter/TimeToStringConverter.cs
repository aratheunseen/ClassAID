using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ClassAid.Models.Converter
{
    class TimeToStringConverter : IValueConverter
    {
        private static readonly string timeFormat = @"hh\:mm tt";
        public object Convert(object value, Type targetType, object parameter, 
            CultureInfo culture)
        {
            TimeSpan time = (TimeSpan)value;
            DateTime date = DateTime.Today.Add(time);
            //date += time;
            return date.ToString(timeFormat);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
