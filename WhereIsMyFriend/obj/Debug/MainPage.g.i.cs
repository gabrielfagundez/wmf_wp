﻿#pragma checksum "C:\wmf_wp\WhereIsMyFriend\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4FC15DF19F4437E44AADE070606E1D8B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WhereIsMyFriend {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid AppTitlePanel;
        
        internal System.Windows.Controls.Grid TitlePanel;
        
        internal System.Windows.Controls.Grid SocialGrid;
        
        internal System.Windows.Controls.Grid LabelMail;
        
        internal System.Windows.Controls.Grid InputMail;
        
        internal System.Windows.Controls.TextBox MailIngresado;
        
        internal System.Windows.Controls.Grid LabelPass;
        
        internal System.Windows.Controls.Grid InputPass;
        
        internal System.Windows.Controls.PasswordBox PassIngresado;
        
        internal System.Windows.Controls.Grid Error;
        
        internal System.Windows.Controls.TextBlock ErrorBlock;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WhereIsMyFriend;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.AppTitlePanel = ((System.Windows.Controls.Grid)(this.FindName("AppTitlePanel")));
            this.TitlePanel = ((System.Windows.Controls.Grid)(this.FindName("TitlePanel")));
            this.SocialGrid = ((System.Windows.Controls.Grid)(this.FindName("SocialGrid")));
            this.LabelMail = ((System.Windows.Controls.Grid)(this.FindName("LabelMail")));
            this.InputMail = ((System.Windows.Controls.Grid)(this.FindName("InputMail")));
            this.MailIngresado = ((System.Windows.Controls.TextBox)(this.FindName("MailIngresado")));
            this.LabelPass = ((System.Windows.Controls.Grid)(this.FindName("LabelPass")));
            this.InputPass = ((System.Windows.Controls.Grid)(this.FindName("InputPass")));
            this.PassIngresado = ((System.Windows.Controls.PasswordBox)(this.FindName("PassIngresado")));
            this.Error = ((System.Windows.Controls.Grid)(this.FindName("Error")));
            this.ErrorBlock = ((System.Windows.Controls.TextBlock)(this.FindName("ErrorBlock")));
        }
    }
}

