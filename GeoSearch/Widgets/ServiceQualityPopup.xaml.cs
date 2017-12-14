using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GeoSearch
{
    public partial class ServiceQualityPopup : UserControl
    {
        public ServiceQualityPopup()
        {
            InitializeComponent();
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            this.ServiceQualityPage.IsOpen = false;
        } 
    }
}
