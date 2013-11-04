﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhereIsMyFriend.Resources;
using WhereIsMyFriend.Classes;
using Newtonsoft.Json;
using Microsoft.Phone.Notification;
using System.Text;
using System.Windows.Media;
using WhereIsMyFriend.Classes;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Net.NetworkInformation;

namespace WhereIsMyFriend
{
    public partial class MainPage : PhoneApplicationPage
    {
        private UserData UsuarioLogin;
        //private HttpNotificationChannel pushChannel;
        // Constructor
        public MainPage()
        {
            /// Holds the push channel that is created or found.


            // The name of our push channel.
            //string channelName = "ToastSampleChannel";

            InitializeComponent();
            Loaded += MainPage_Loaded;
            if (PageTitle.Text == "iniciar sesión"){
                PageTitle.FontSize = 83;
            }


            // Try to find the push channel.
            //pushChannel = HttpNotificationChannel.Find(channelName);

            // //If the channel was not found, then create a new connection to the push service.
            //if (pushChannel == null)
            //{
                //pushChannel = new HttpNotificationChannel(channelName);

                // Register for all the events before attempting to open the channel.
                //pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
                //pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

                // Register for this notification only if you need to receive the notifications while your application is running.
                //pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

                //pushChannel.Open();

                // Bind this new channel for toast events.
                //pushChannel.BindToShellToast();
            //}
            //else
            //{
            //    //// The channel was already open, so just register for all the events.
            //    //pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(PushChannel_ChannelUriUpdated);
            //    //pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(PushChannel_ErrorOccurred);

            //    //// Register for this notification only if you need to receive the notifications while your application is running.
            //    //pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(PushChannel_ShellToastNotificationReceived);

            //    //// Display the URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
            //    //System.Diagnostics.Debug.WriteLine(pushChannel.ChannelUri.ToString());
            //    //MessageBox.Show(String.Format("Channel Uri is {0}",
            //    //    pushChannel.ChannelUri.ToString()));
              
            //}


            // Código de ejemplo para traducir ApplicationBar
            BuildLocalizedApplicationBar();
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationService.RemoveBackEntry();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            while ((this.NavigationService.BackStack != null) && (this.NavigationService.BackStack.Any()))
            {
                this.NavigationService.RemoveBackEntry();
            }
        }
        private void Click_check(object sender, EventArgs e)
        {
            if (IsNetworkAvailable())
            {
                if (MailIngresado.Text == "")
                {
                    ProgressB.IsIndeterminate = false;
                    ErrorBlock.Text = AppResources.invalidMail;
                    ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                }
                else
                    if (PassIngresado.Password == "")
                    {
                        ProgressB.IsIndeterminate = false;
                        ErrorBlock.Text = AppResources.invalidPassword;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {

                        ProgressB.IsIndeterminate = true;
                        Connecting.Visibility = System.Windows.Visibility.Visible;
                        ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                        var webClient = new WebClient();
                        webClient.Headers[HttpRequestHeader.ContentType] = "text/json";
                        webClient.UploadStringCompleted += this.sendPostCompleted;

                        string json = "{\"Mail\":\"" + MailIngresado.Text + "\"," +
                                          "\"Password\":\"" + PassIngresado.Password + "\"," + "\"DeviceId\":\"" + "" + "\"," + "\"Platform\":\"" + "wp" + "\"}";
                        //string json = "{\"Mail\":\"" + MailIngresado.Text + "\"," +
                        //                          "\"Password\":\"" + PassIngresado.Password + "\"}";

                        webClient.UploadStringAsync((new Uri(App.webService + "/api/Users/LoginWhere")), "POST", json);

                    }
            }
            else
            {
                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = AppResources.NoInternetConnection,
                    Message = AppResources.NoInternetConnectionMessage,
                    LeftButtonContent = AppResources.OkTitle,
                    Background = mybrush,
                    IsFullScreen = false,
                };


                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            break;
                        case CustomMessageBoxResult.None:
                            // Acción.
                            break;
                        default:
                            break;
                    }
                };

                messageBox.Show();


            }
        }


        private void sendPostCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if ((e.Error != null) && (e.Error.GetType().Name == "WebException"))
            {
                WebException we = (WebException)e.Error;
                HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;

                switch (response.StatusCode)
                {

                    case HttpStatusCode.NotFound: // 404
                        System.Diagnostics.Debug.WriteLine("Not found!");
                        ErrorBlock.Text = AppResources.WrongMailError;
                        ProgressB.IsIndeterminate = false;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case HttpStatusCode.Unauthorized: // 401
                        System.Diagnostics.Debug.WriteLine("Not authorized!");
                        ErrorBlock.Text = AppResources.WrongPasswordError;
                        Connecting.Visibility = System.Windows.Visibility.Collapsed;
                        ProgressB.IsIndeterminate = false;
                        ErrorBlock.Visibility = System.Windows.Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ErrorBlock.Visibility = System.Windows.Visibility.Collapsed;
                LoggedUser user = LoggedUser.Instance;
                user.SetLoggedUser(JsonConvert.DeserializeObject<UserData>(e.Result));
                WebClient webClientFriend = new WebClient();
                webClientFriend.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedFriends);
                Uri LoggedUserFriends = new Uri(App.webService + "/api/Friends/GetAllFriends/" + user.GetLoggedUser().Id);
                webClientFriend.DownloadStringAsync(LoggedUserFriends);

            }
        }
        void webClient_DownloadStringCompletedFriends(object sender, DownloadStringCompletedEventArgs e)
        {
            
                List<UserData> friendsList = JsonConvert.DeserializeObject<List<UserData>>(e.Result);
                LoggedUser luser = LoggedUser.Instance;
                luser.setFriends(friendsList);
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompletedRequests);
                Uri LoggedUserRequests = new Uri(App.webService + "/api/Solicitudes/GetAll/" + luser.GetLoggedUser().Id);
                webClient.DownloadStringAsync(LoggedUserRequests);
          
        }
        void webClient_DownloadStringCompletedRequests(object sender, DownloadStringCompletedEventArgs e)
        {
                List<RequestData> requestsList = JsonConvert.DeserializeObject<List<RequestData>>(e.Result);
                LoggedUser luser = LoggedUser.Instance;
                luser.setRequests(requestsList);
                NavigationService.Navigate(new Uri("/LoggedMainPages/Menu.xaml", UriKind.Relative));

        }

        private bool IsNetworkAvailable()
        {
            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
                return false;
            else
                return true;
        }

      

        void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {

            Dispatcher.BeginInvoke(() =>
            {
                // Display the new URI for testing purposes.   Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());
                MessageBox.Show(String.Format("Channel Uri is {0}",
                    e.ChannelUri.ToString()));

            });
        }
        void PushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }
        void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;

            message.AppendFormat("Received Toast {0}:\n", DateTime.Now.ToShortTimeString());

            // Parse out the information that was part of the message.
            foreach (string key in e.Collection.Keys)
            {
                message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

                if (string.Compare(
                    key,
                    "wp:Param",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.CompareOptions.IgnoreCase) == 0)
                {
                    relativeUri = e.Collection[key];
                }
            }




            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() => {




                SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "",
                    Message = " Tom Jones wants to knnow where you are",
                    LeftButtonContent = "Accept",
                    RightButtonContent = "Cancel",
                    Background = mybrush,
                    IsFullScreen = false
                };


                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            // Acción.
                            break;
                        case CustomMessageBoxResult.RightButton:
                            // Acción.
                            break;
                        case CustomMessageBoxResult.None:
                            // Acción.
                            break;
                        default:
                            break;
                    }
                };

                messageBox.Show();
            
            
            
            
            
            });

        }

        void PushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            string message;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body))
            {
                message = reader.ReadToEnd();
            }
            SolidColorBrush mybrush = new SolidColorBrush(Color.FromArgb(255, 0, 175, 240));



            CustomMessageBox messageBox = new CustomMessageBox()
            {
                Caption = "",
                Message = message + " wants to know where you are",
                LeftButtonContent = "Accept",
                RightButtonContent = "Cancel",
                Background = mybrush,
                IsFullScreen = false
            };


            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        // Acción.
                        break;
                    case CustomMessageBoxResult.RightButton:
                        // Acción.
                        break;
                    case CustomMessageBoxResult.None:
                        // Acción.
                        break;
                    default:
                        break;
                }
            };


            Dispatcher.BeginInvoke(() =>
                messageBox.Show());
        }
        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButton =
                new ApplicationBarIconButton(new
                Uri("/Toolkit.Content/ApplicationBar.Check.png", UriKind.Relative));
            appBarButton.Text = AppResources.AppBarLoginButtonText;
            appBarButton.Click += this.Click_check;
            ApplicationBar.Buttons.Add(appBarButton);
            ApplicationBar.BackgroundColor =  Color.FromArgb(255, 0, 175, 240);
            ApplicationBar.IsMenuEnabled=false;
            ApplicationBar.IsVisible= true;
            ApplicationBar.Opacity= (double)(.99);
            ApplicationBar.Mode = ApplicationBarMode.Default;
  

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem =
                new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }




    }
}
