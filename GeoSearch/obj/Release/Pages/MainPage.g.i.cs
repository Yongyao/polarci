﻿#pragma checksum "C:\Geosearch\GeoSearch-cisc-2012-07-23-jizhe\GeoSearch\Pages\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DF16DA304CEE91D04CF8FD7FAD602972"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Windows.Controls;
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
    
    
    public partial class MainPage : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Windows.Controls.WatermarkedTextBox SearchContentTextBox;
        
        internal System.Windows.Controls.TextBlock SearchContentTextBox_InterTextBox;
        
        internal System.Windows.Controls.Button SearchButton;
        
        internal System.Windows.Controls.HyperlinkButton AdvancedSearchButton;
        
        internal System.Windows.Controls.TextBlock outputTest;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Pages/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.SearchContentTextBox = ((Microsoft.Windows.Controls.WatermarkedTextBox)(this.FindName("SearchContentTextBox")));
            this.SearchContentTextBox_InterTextBox = ((System.Windows.Controls.TextBlock)(this.FindName("SearchContentTextBox_InterTextBox")));
            this.SearchButton = ((System.Windows.Controls.Button)(this.FindName("SearchButton")));
            this.AdvancedSearchButton = ((System.Windows.Controls.HyperlinkButton)(this.FindName("AdvancedSearchButton")));
            this.outputTest = ((System.Windows.Controls.TextBlock)(this.FindName("outputTest")));
        }
    }
}

