﻿#pragma checksum "C:\GeoSearch-cisc-2012-07-23\GeoSearch\Widgets\ServiceQualityPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "042D6C0E6B215CB21AA04EAC33489864"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GeoSearch;
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
    
    
    public partial class ServiceQualityPopup : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Primitives.Popup ServiceQualityPage;
        
        internal System.Windows.Controls.Grid Grid_ServiceQoS;
        
        internal GeoSearch.ServiceQualityInfoPanel serviceQualityInfoPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/ServiceQualityPopup.xaml", System.UriKind.Relative));
            this.ServiceQualityPage = ((System.Windows.Controls.Primitives.Popup)(this.FindName("ServiceQualityPage")));
            this.Grid_ServiceQoS = ((System.Windows.Controls.Grid)(this.FindName("Grid_ServiceQoS")));
            this.serviceQualityInfoPanel = ((GeoSearch.ServiceQualityInfoPanel)(this.FindName("serviceQualityInfoPanel")));
        }
    }
}

