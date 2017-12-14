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
using System.Windows.Data;
using System.Globalization;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public partial class MyPushPinInfoBox : UserControl
    {

        public enum CreationEventType
        {
            MouseHoverIn,
            MouseLeftButtonClick,
            SelectFromRecordsList
        };

        public CreationEventType creationEventType = CreationEventType.SelectFromRecordsList;
       
        public MyPushPinInfoBox()
        {
            InitializeComponent();
        }

        private void PinInfoBox_Button_Close_Click(object sender, RoutedEventArgs e)
        {
            // The user clicked the 'X' close button from editing a pushpin's text.
            // Hide editable text box and close button.
            Infobox.Visibility = Visibility.Collapsed;
        }

        private void RecordItemTitle_Click(object sender, RoutedEventArgs e)
        {
            TextBlock metadataTitle = (TextBlock)sender;
            string MetadataAccessURL = (string)metadataTitle.Tag;
            SearchingResultPage srp = App.getCurrentSearchingResultPage();
            if (srp != null)
                SearchingResultPage.otherQueryFunction.getRecordDetailMetadata_Using_WCFService(MetadataAccessURL, SearchingResultPage.ID);
            //如果是BingMaps全屏模式,则退出全屏
            else
            {
                SearchingResultPage srp1 = App.getSearchingResultPageFromBingMapsGrid();
                if (srp1 != null)
                {
                    srp1.BingMapsExitFullScreen();
                    SearchingResultPage.otherQueryFunction.getRecordDetailMetadata_Using_WCFService(MetadataAccessURL, SearchingResultPage.ID);
                }
            }
        }

        private void BingMapsButton_Click(object sender, RoutedEventArgs e)
        {
            string WMSURL = null;
            if (sender is Button)
            {
                Button bt = sender as Button;
                WMSURL = bt.Tag as string;
            }
            SearchingResultPage srp = App.getCurrentSearchingResultPage();
            if (srp != null)
                srp.addWMSToWMSLayerControlOnBingMaps(WMSURL);
            //如果是BingMaps全屏模式
            else
            {
                SearchingResultPage srp1 = App.getSearchingResultPageFromBingMapsGrid();
                if (srp1 != null)
                    srp1.addWMSToWMSLayerControlOnBingMaps(WMSURL);
            }    
        }
    }

    public class BBoxStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String bboxString = "";
            if (value is BBox)
            {
                BBox bbox = value as BBox;
                bboxString = "(" + bbox.BBox_Lower_Lon + ", " + bbox.BBox_Lower_Lat + ", " + bbox.BBox_Upper_Lon + ", " + bbox.BBox_Upper_Lat + ")";
            }
            return bboxString;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
