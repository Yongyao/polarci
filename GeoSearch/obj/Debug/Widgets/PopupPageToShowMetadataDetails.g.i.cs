﻿#pragma checksum "C:\GeoSearch-cisc-2012-07-23\GeoSearch\Widgets\PopupPageToShowMetadataDetails.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CE1B706DB4C3C03954013FE54B7309F9"
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


namespace GeoSearch {
    
    
    public partial class PopupPage_ShowMetadataDetails : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Primitives.Popup MetadataDetailPage;
        
        internal System.Windows.Controls.Grid popContentGrid;
        
        internal System.Windows.Controls.Border border2;
        
        internal System.Windows.Controls.Border border1;
        
        internal System.Windows.Controls.Label PopupPageTitle;
        
        internal System.Windows.Controls.Button closePop_button;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer1;
        
        internal System.Windows.Controls.TextBox DetailTextBlock;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/PopupPageToShowMetadataDetails.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MetadataDetailPage = ((System.Windows.Controls.Primitives.Popup)(this.FindName("MetadataDetailPage")));
            this.popContentGrid = ((System.Windows.Controls.Grid)(this.FindName("popContentGrid")));
            this.border2 = ((System.Windows.Controls.Border)(this.FindName("border2")));
            this.border1 = ((System.Windows.Controls.Border)(this.FindName("border1")));
            this.PopupPageTitle = ((System.Windows.Controls.Label)(this.FindName("PopupPageTitle")));
            this.closePop_button = ((System.Windows.Controls.Button)(this.FindName("closePop_button")));
            this.scrollViewer1 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer1")));
            this.DetailTextBlock = ((System.Windows.Controls.TextBox)(this.FindName("DetailTextBlock")));
        }
    }
}

