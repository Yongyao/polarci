﻿#pragma checksum "C:\Geosearch\GeoSearch-cisc-2012-07-23-jizhe\GeoSearch\Widgets\GeneralPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C5B97055970D8A46F8787E0FBD47403D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace GeoSearch.Widgets {
    
    
    public partial class GeneralPopup : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Primitives.Popup PopupPage;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer1;
        
        internal System.Windows.Controls.TextBox TextBox_Content;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/GeneralPopup.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.PopupPage = ((System.Windows.Controls.Primitives.Popup)(this.FindName("PopupPage")));
            this.scrollViewer1 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer1")));
            this.TextBox_Content = ((System.Windows.Controls.TextBox)(this.FindName("TextBox_Content")));
        }
    }
}

