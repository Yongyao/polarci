﻿#pragma checksum "C:\GeoSearch-cisc-2012-07-23\GeoSearch\Widgets\CheckBoxWithImageText.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E88AB1D8F1E657FE43F9CA6A39810F29"
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
    
    
    public partial class CheckBoxWithImageText : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.StackPanel StackPanelArea1;
        
        internal System.Windows.Controls.CheckBox isSelected_CheckBox;
        
        internal System.Windows.Controls.Image Image1;
        
        internal System.Windows.Controls.TextBlock Title;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/Widgets/CheckBoxWithImageText.xaml", System.UriKind.Relative));
            this.StackPanelArea1 = ((System.Windows.Controls.StackPanel)(this.FindName("StackPanelArea1")));
            this.isSelected_CheckBox = ((System.Windows.Controls.CheckBox)(this.FindName("isSelected_CheckBox")));
            this.Image1 = ((System.Windows.Controls.Image)(this.FindName("Image1")));
            this.Title = ((System.Windows.Controls.TextBlock)(this.FindName("Title")));
        }
    }
}

