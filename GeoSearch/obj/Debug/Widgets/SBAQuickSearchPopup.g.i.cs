﻿#pragma checksum "C:\GeoSearch-cisc-2012-07-23\GeoSearch\Widgets\SBAQuickSearchPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "68D53B9A5382D97FEE7DD5A9A2E63804"
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
    
    
    public partial class SBAQuickSearchPopup : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Primitives.Popup SBAQuickSearchPage;
        
        internal System.Windows.Controls.Grid Grid_ServiceQoS;
        
        internal GeoSearch.SBAQuickSearchPanel SBAQuickSearchPanel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/SBAQuickSearchPopup.xaml", System.UriKind.Relative));
            this.SBAQuickSearchPage = ((System.Windows.Controls.Primitives.Popup)(this.FindName("SBAQuickSearchPage")));
            this.Grid_ServiceQoS = ((System.Windows.Controls.Grid)(this.FindName("Grid_ServiceQoS")));
            this.SBAQuickSearchPanel = ((GeoSearch.SBAQuickSearchPanel)(this.FindName("SBAQuickSearchPanel")));
        }
    }
}

