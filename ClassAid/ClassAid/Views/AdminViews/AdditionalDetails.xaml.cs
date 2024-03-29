﻿using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        public AdditionalDetails()
        {
            InitializeComponent();
        }
        private async void CompleteSignUp_Clicked()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                App.Admin.Name = userRealName.Text;
                App.Admin.Phone = userPhone.Text;
                BatchDetails batch = new BatchDetails()
                {
                    University = instName.Text,
                    Department = deptName.Text,
                    Semester = semName.Text,
                    Section = secName.Text
                };
                resultText.Text = "Creating Unique ID";
                App.Admin.TeamCode = await FirebaseHandler.GetTeamCode(instName.Text, App.Admin.Key);
                resultText.Text = "Creating Profile";
                App.Admin.BatchDetails = batch;
                App.Admin.ScheduleList = new ObservableCollection<ScheduleModel>();
                App.Admin.StudentList.Add(new Student()
                {
                    Name = App.Admin.Name,
                    Phone = App.Admin.Phone,
                    ID = App.Admin.ID,
                    Key = App.Admin.Key,
                    IsActive = true,
                    IsRejected = false
                });
                App.Admin.ID = stuId.Text;
                App.Admin.Phone = userPhone.Text;
                try
                {
                    LocalDbContex.CreateTables();
                    Preferences.Set(PrefKeys.IsLoggedIn, true);
                    Preferences.Set(PrefKeys.AdminKey, App.Admin.Key);
                    Preferences.Set(PrefKeys.Key, App.Admin.Key);
                    Preferences.Set(PrefKeys.IsAdmin, true);
                    FirebaseHandler.UpdateAdmin(App.Admin);
                    LocalDbContex.SaveUser(App.Admin);
                    LocalDbContex.SaveBatchDetails(App.Admin.BatchDetails);
                    Application.Current.MainPage =
                        new NavigationPage(new Dashboard());
                }
                catch (Exception)
                {
                    // throw e;
                    resultText.Text = "Something bad happened. Please check back in a short.";
                }
            }
            else
            {
                DependencyService.Get<Toast>().Show("No INTERNET connection.");
            }
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userPhone.Text) ||
                string.IsNullOrWhiteSpace(userRealName.Text) ||
                string.IsNullOrWhiteSpace(instName.Text) ||
                string.IsNullOrWhiteSpace(deptName.Text) ||
                string.IsNullOrWhiteSpace(secName.Text) ||
                string.IsNullOrWhiteSpace(secName.Text) ||
                string.IsNullOrWhiteSpace(stuId.Text))
            {
                contToDashBtn.Command = null;
            }
            else
            {
                contToDashBtn.Command = new Command(() => CompleteSignUp_Clicked());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            userRealName.Focus();
        }
    }
}