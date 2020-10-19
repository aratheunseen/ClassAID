using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullSchedulePage : ContentPage
    {
        public List<ScheduleModel> SaturdaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Saturday).ToList();
            }
        }
        public List<ScheduleModel> SundaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Sunday).ToList();
            }
        }
        public List<ScheduleModel> MondaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Monday).ToList();
            }
        }
        public List<ScheduleModel> TuesdaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Tuesday).ToList();
            }
        }
        public List<ScheduleModel> WednesdaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Wednesday).ToList();
            }
        }
        public List<ScheduleModel> ThursdaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Thursday).ToList();
            }
        }
        public List<ScheduleModel> FrydaySchedules
        {
            get
            {
                return LocalDbContex.GetSchedules()
                    .Where(p => p.DayOfWeek == DayOfWeek.Friday).ToList();
            }
        }

        public FullSchedulePage()
        {
            InitializeComponent();
            Debug.WriteLine(LocalDbContex.GetSchedules().Count());
        }
    }
}