﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereIsMyFriend.Classes;
using Newtonsoft.Json;
using WhereIsMyFriend.Resources;
using System.Windows.Media;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Menu : PhoneApplicationPage
    {
        
        public Menu()
        {
            InitializeComponent();
            TiltEffect.TiltableItems.Add(typeof(Grid));
            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
            
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ProgressB.IsIndeterminate = false;
            Connecting.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ProgressB.IsIndeterminate = true;
            Connecting.Visibility = System.Windows.Visibility.Visible;

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            LoggedUser user = LoggedUser.Instance;
            UserData luser = user.GetLoggedUser();
            Uri LoggedUserFriends = new Uri(App.webService + "/api/Friends/GetAllFriends/" + luser.Id);
            webClient.DownloadStringAsync(LoggedUserFriends);

        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            List<UserData> friendsList = JsonConvert.DeserializeObject<List<UserData>>(e.Result);
            LoggedUser luser = LoggedUser.Instance;
            luser.setFriends(friendsList);    


            NavigationService.Navigate(new Uri("/LoggedMainPages/Friends.xaml", UriKind.Relative));

        }

        private void HubTile_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ProgressB.IsIndeterminate = true;
            Connecting.Visibility = System.Windows.Visibility.Visible;
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
            App.Mapa = true;
        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
        }

        private void Requests_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ProgressB.IsIndeterminate = true;
            Connecting.Visibility = System.Windows.Visibility.Visible;


            LoggedUser user = LoggedUser.Instance;
            List<UserData> requests = new List<UserData>();
            UserData usuario1 = new UserData();
            UserData usuario2 = new UserData();
            UserData usuario3 = new UserData();
            UserData usuario4 = new UserData();
            usuario1.Name = "Jhon Smith";
            usuario2.Name = "Tom Jones";
            usuario3.Name = "Jamie Oliver";
            usuario4.Name = "Amanda Jones";
            requests.Add(usuario1);
            requests.Add(usuario2);
            requests.Add(usuario3);
            requests.Add(usuario4);
            user.setRequests(requests);
            NavigationService.Navigate(new Uri("/LoggedMainPages/Requests.xaml", UriKind.Relative));

        }

        private void Settings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Settings.xaml", UriKind.Relative));

        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Toolkit.Content/feature.settings.png", UriKind.Relative));
            appBarButton.Text = AppResources.SettingsTitle;
            appBarButton.Click += this.Settings_Click;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor = Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled = false;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;


            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem =
                new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }
    }
}