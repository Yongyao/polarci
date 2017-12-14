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

namespace GeoSearch.Pages
{
    public partial class HelpPage : UserControl
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        private void HyperlinkButton_BackToBasicSearch(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            MainPage mp = new MainPage();
            App.Navigate(mp);
        }
    }
}
