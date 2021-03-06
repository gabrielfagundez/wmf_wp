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
using Microsoft.Phone.Net.NetworkInformation;
using WhereIsMyFriend.Classes;
using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Threading;

namespace WhereIsMyFriend.LoggedMainPages
{
    public partial class Menu : PhoneApplicationPage
    {
        string latitud, longitud = string.Empty;//viene del mapa (es para mandar pos siempre)
        DispatcherTimer newTimer = new DispatcherTimer();
        public Menu()
        {
            InitializeComponent();
            // Código de ejemplo para traducir ApplicationBar
            this.Loaded += MainPage_Loaded;
            BuildLocalizedApplicationBar();


            
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Shell.SystemTray.BackgroundColor = Colors.Black;
        }
        /*private void rc_PushReached(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                requestsImage.Text = (LoggedUser.Instance.getRequests().Requests.Count<RequestData>() + 1).ToString();
            });

        }*/

        void OnTimerTick(Object sender, EventArgs args)
        {
            if (IsNetworkAvailable())
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Requests Update en f!");
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                    LoggedUser user = LoggedUser.Instance;
                    UserData luser = user.GetLoggedUser();
                    Uri LoggedUserRequests = new Uri(App.webService + "/api/Solicitudes/GetAll/" + luser.Id);
                    webClient.DownloadStringAsync(LoggedUserRequests);
                }
                catch (WebException webex)
                {
                    HttpWebResponse webResp = (HttpWebResponse)webex.Response;

                    switch (webResp.StatusCode)
                    {
                        case HttpStatusCode.NotFound: // 404
                            break;
                        case HttpStatusCode.InternalServerError: // 500
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");

                        break;
                    default:
                        break;
                }
            }
            else
            {
                List<RequestData> requestsList = JsonConvert.DeserializeObject<List<RequestData>>(e.Result);
                LoggedUser luser = LoggedUser.Instance;
                RequestsList r = new RequestsList();
                r.Requests = requestsList;
                luser.setRequests(r);
                requestsImage.Text = LoggedUser.Instance.getRequests().Requests.Count<RequestData>().ToString();
            }
        }

        private void iniMap()
        {
            App.Geolocator = null;//mapa

            if (App.Geolocator == null)
            {
                //Si nunca se inicializo
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.MovementThreshold = 15; // The units are meters.
                App.Geolocator.PositionChanged += geolocator_PositionChanged2;
            }

        }

        public void geolocator_PositionChanged2(Geolocator sender, PositionChangedEventArgs args)
        {
            App.isGpsEnabled = true;

            if (!App.RunningInBackground)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    System.Diagnostics.Debug.WriteLine("Actualizamos en frente");
                    latitud = args.Position.Coordinate.Latitude.ToString("0.00000").Replace(",",".");
                    longitud = args.Position.Coordinate.Longitude.ToString("0.00000").Replace(",", ".");

                    var pos = ConvertGeocoordinate(args.Position.Coordinate);
                    PointsHandler ph = PointsHandler.Instance;
                    ph.myPosition = pos;

                    var webClient = new WebClient();
                    webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                    webClient.UploadStringCompleted += this.sendPostCompleted1;
                    LoggedUser user = LoggedUser.Instance;
                    string json = "{\"Mail\":\"" + user.GetLoggedUser().Mail + "\"," +
                        "\"Name\":\"" + user.GetLoggedUser().Name + "\"," +
                                    "\"Latitude\":\"" + latitud + "\"," +
                                      "\"Longitude\":\"" + longitud + "\"}";
                    System.Diagnostics.Debug.WriteLine(json);

                    webClient.UploadStringAsync((new Uri(App.webService + "/api/Geolocation/SetLocation/")), "POST", json);
                });
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Actualizamos en background");
                latitud = args.Position.Coordinate.Latitude.ToString("0.00000").Replace(",",".");
                longitud = args.Position.Coordinate.Longitude.ToString("0.00000").Replace(",", ".");
                var webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                webClient.UploadStringCompleted += this.sendPostCompleted1;
                LoggedUser user = LoggedUser.Instance;
                string json = "{\"Mail\":\"" + user.GetLoggedUser().Mail + "\"," +
                    "\"Name\":\"" + user.GetLoggedUser().Name + "\"," +
                                "\"Latitude\":\"" + latitud + "\"," +
                                  "\"Longitude\":\"" + longitud + "\"}";
                System.Diagnostics.Debug.WriteLine(json);

                webClient.UploadStringAsync((new Uri(App.webService + "/api/Geolocation/SetLocation/")), "POST", json);




            }
        }


        private void sendPostCompleted1(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.Result.ToString());
            }
        }


        public static GeoCoordinate ConvertGeocoordinate(Geocoordinate geocoordinate)
        {
            return new GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            iniMap();
            //  RequestsCounter rc = RequestsCounter.Instance;
            // rc.PushReached += rc_PushReached;
            requestsImage.Text = LoggedUser.Instance.getRequests().Requests.Count<RequestData>().ToString();
            newTimer.Interval = TimeSpan.FromSeconds(5);
            newTimer.Tick += OnTimerTick;
            newTimer.Start();

            requestsImage.Text = LoggedUser.Instance.getRequests().Requests.Count<RequestData>().ToString();
            while ((this.NavigationService.BackStack != null) && (this.NavigationService.BackStack.Any()))
            {
                this.NavigationService.RemoveBackEntry();
            }
        }

        private void HubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        { 
            App.VengoDeMapa = false;
            NavigationService.Navigate(new Uri("/LoggedMainPages/Friends.xaml", UriKind.Relative));
  

        }

        private bool IsNetworkAvailable()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }

        private void HubTile_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
            App.Mapa = true;
        }

        private void PhoneApplicationPage_OrientationChanged_1(object sender, OrientationChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoggedMainPages/Mapa.xaml", UriKind.Relative));
        }

        private void Requests_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            NavigationService.Navigate(new Uri("/LoggedMainPages/Requests.xaml", UriKind.Relative));
          
        }

        private void Logout_Click(object sender, EventArgs e)
        {
                        //mapa*******************
            App.Geolocator.PositionChanged -= geolocator_PositionChanged2;
            App.Geolocator = null;
            //mapa*******************
            NavigationService.Navigate(new Uri("/LoggedMainPages/LogoutConfirmation.xaml", UriKind.Relative));

        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {

            newTimer.Stop();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Assets/Images/Logout-32.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLogoutButtonText;
            appBarButton.Click += this.Logout_Click;
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
