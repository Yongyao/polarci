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
using System.Windows.Browser;
using GeoSearch.Pages;
using System.Windows.Threading;

namespace GeoSearch
{
    public partial class MainPage : UserControl
    {    
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            SearchContentTextBox.KeyUp += new KeyEventHandler(SearchContentTextBox_KeyUp);    
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //let browser focus to current html element 
            System.Windows.Browser.HtmlPage.Plugin.Focus();
            
            //SearchContentTextBox.Focus();
            SearchContentTextBox.Dispatcher.BeginInvoke(() => { SearchContentTextBox.Focus(); });

            //use multi-thread to test if textbox is focused
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(5000);
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start(); 
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (SearchContentTextBox.Focus())
            {
                MessageBox.Show("文本框聚焦成功!");
            }
        } 

        void SearchContentTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && RecordsSearchFunctions.cannotStartSearchYet == false && RecordsSearchFunctions.isContentSearchable(SearchContentTextBox.Text) == true)
            {
                basicSearchFunction(SearchContentTextBox.Text);
            }
        }

        public void basicSearchFunction(string content)
        {
            RecordsSearchFunctions.cannotStartSearchYet = true;
            RecordsSearchFunctions sf = new RecordsSearchFunctions(); 
            this.SearchButton.IsEnabled = false;
            sf.BasicSearch_Using_WCFService(SearchContentTextBox.Text, ConstantCollection.startPosition, ConstantCollection.maxRecords);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecordsSearchFunctions.cannotStartSearchYet == false && RecordsSearchFunctions.isContentSearchable(SearchContentTextBox.Text) == true)
                basicSearchFunction(SearchContentTextBox.Text);
        }

        [ScriptableMember]
        public void UpdateText(string result)
        {
            outputTest.Text = result;
        }

        private void AdvancedSearch_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            AdvancedSearchPage asp = new AdvancedSearchPage();
            if (RecordsSearchFunctions.isContentSearchable(SearchContentTextBox.Text) == true)
            {
                asp.AllOfTheWords_Content.Text = SearchContentTextBox.Text.TrimStart().TrimEnd();
                Dispatcher.BeginInvoke(() =>
                {
                    asp.AllOfTheWords_Content.SelectAll();
                    asp.AllOfTheWords_Content.Focus();
                });
            }
            App.Navigate(asp);
        }

        private void HyperlinkButton_Click_Help(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            App.Navigate(App.helpPage);
        }
    }
}
