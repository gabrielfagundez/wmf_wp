﻿#pragma checksum "C:\Users\Carmela\Documents\GitHub\Copia\wmf_wp\WhereIsMyFriend\LoggedMainPages\Friends.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F65BBA84AD84A07D9FA32A70488D97BC"
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


namespace WhereIsMyFriend.LoggedMainPages {
    
    
    public partial class Friends : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid SearchPanel;
        
        internal Microsoft.Phone.Controls.PhoneTextBox txtSearch;
        
        internal System.Windows.Controls.Grid TitlePanel;
        
        internal System.Windows.Controls.TextBlock FriendTitle;
        
        internal System.Windows.Controls.Grid List;
        
        internal System.Windows.Controls.ListBox FriendsList;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/WhereIsMyFriend;component/LoggedMainPages/Friends.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.SearchPanel = ((System.Windows.Controls.Grid)(this.FindName("SearchPanel")));
            this.txtSearch = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("txtSearch")));
            this.TitlePanel = ((System.Windows.Controls.Grid)(this.FindName("TitlePanel")));
            this.FriendTitle = ((System.Windows.Controls.TextBlock)(this.FindName("FriendTitle")));
            this.List = ((System.Windows.Controls.Grid)(this.FindName("List")));
            this.FriendsList = ((System.Windows.Controls.ListBox)(this.FindName("FriendsList")));
        }
    }
}

