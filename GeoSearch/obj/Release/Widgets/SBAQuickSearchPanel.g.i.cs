﻿#pragma checksum "C:\Geosearch\GeoSearch-cisc-2012-07-23-jizhe\GeoSearch\Widgets\SBAQuickSearchPanel.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D51B1EB571A80812976647D2AEAF09E5"
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
    
    
    public partial class SBAQuickSearchPanel : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.TextBlock TextBox_Title;
        
        internal System.Windows.Controls.TreeView TreeView_SBAVocabulary;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/SBAQuickSearchPanel.xaml", System.UriKind.Relative));
            this.TextBox_Title = ((System.Windows.Controls.TextBlock)(this.FindName("TextBox_Title")));
            this.TreeView_SBAVocabulary = ((System.Windows.Controls.TreeView)(this.FindName("TreeView_SBAVocabulary")));
        }
    }
}

