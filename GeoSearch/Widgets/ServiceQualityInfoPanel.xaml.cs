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
using GeoSearch;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Controls.DataVisualization.Charting;
using GeoSearch.QualityQueryServiceReference;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public partial class ServiceQualityInfoPanel : UserControl
    {
        public QualityQueryFunctions queryPerformanceFunctions;
        public ClientSideRecord record;
        public DateTime FromDateTime_ForService;
        public DateTime ToDateTime_ForService;
        public ObservableCollection<ServiceQoSInfoInOneMeasurement> latestMeasurementInfoList_ForService;
        public DateTime FromDateTime_ForLayer;
        public DateTime ToDateTime_ForLayer;

        public DateTime StartDateTime_ForService;
        public DateTime EndDateTime_ForService;
        public DateTime StartDateTime_ForLayer;
        public DateTime EndDateTime_ForLayer;

        public bool isPanelExtended = false;
        public string layerName = null;

        public static double serviceQoSHistoralInterval = 360;
        public static double layerQoSHistoralInterval = 200;

        
        public ServiceQualityInfoPanel()
        {
            InitializeComponent();
            datePicker_From_ForService.DataContext = this;
            foreach (ComboBoxItem cbi in ComboBox_DateInterval_ForService.Items)
            {
                string intervalString = cbi.Content as string;
                intervalString = intervalString.Trim();
                intervalString = intervalString.Replace("days", "");
                double interval = double.Parse(intervalString);
                if (serviceQoSHistoralInterval == interval)
                {
                    cbi.IsSelected = true;
                    ComboBox_DateInterval_ForService.SelectedItem = cbi;
                    break;                 
                }
            }

            foreach (ComboBoxItem cbi in ComboBox_DateInterval_ForLayer.Items)
            {
                string intervalString = cbi.Content as string;
                intervalString = intervalString.Trim();
                intervalString = intervalString.Replace("days", "");
                double interval = double.Parse(intervalString);
                if (layerQoSHistoralInterval == interval)
                {
                    cbi.IsSelected = true;
                    ComboBox_DateInterval_ForLayer.SelectedItem = cbi;
                    break;
                }
            }
        }

        public void setBasicContent(ClientSideRecord record, QualityQueryFunctions queryPerformanceFunctions, DateTime from, DateTime to)
        {
            this.record = record;
            if (record.Type.Equals(ConstantCollection.ServiceType_WMS))
                Button_WMS_Layers_QoS.Visibility = Visibility.Visible;
            else
                Button_WMS_Layers_QoS.Visibility = Visibility.Collapsed;
            this.queryPerformanceFunctions = queryPerformanceFunctions;
            setServiceQoSDateTimeFromAndTo(from, to);
            setServiceQoSDatePickerFromAndTo();
            TextBox_Title.DataContext = record;
        }

        private void setServiceQoSDatePickerFromAndTo()
        {
            datePicker_From_ForService.SelectedDate = FromDateTime_ForService;
            datePicker_To_ForService.SelectedDate = ToDateTime_ForService;
        }

        private void setLayerQoSDatePickerFromAndTo()
        {
            datePicker_From_ForLayer.SelectedDate = FromDateTime_ForLayer;
            datePicker_To_ForLayer.SelectedDate = ToDateTime_ForLayer;
        }

        private void setServiceQoSDateTimeFromAndTo(DateTime from, DateTime to)
        {
            if (from >= StartDateTime_ForService)
                FromDateTime_ForService = from;
            else
            {
                FromDateTime_ForService = StartDateTime_ForService;
                to = FromDateTime_ForService.AddDays(serviceQoSHistoralInterval-1);
            }

            if (to > EndDateTime_ForService && EndDateTime_ForService > StartDateTime_ForService)
                ToDateTime_ForService = EndDateTime_ForService;                
            else
            {
                if(to >= StartDateTime_ForService)
                    ToDateTime_ForService = to;   
                else
                    ToDateTime_ForService = StartDateTime_ForService;
            }
        }

        private void setLayerQoSDateTimeFromAndTo(DateTime from, DateTime to)
        {
            if (from >= StartDateTime_ForLayer)
                FromDateTime_ForLayer = from;
            else
            {
                FromDateTime_ForLayer = StartDateTime_ForLayer;
                to = FromDateTime_ForLayer.AddDays(layerQoSHistoralInterval-1);
            }

            if (to > EndDateTime_ForLayer && EndDateTime_ForLayer > StartDateTime_ForLayer)
                ToDateTime_ForLayer = EndDateTime_ForLayer;
            else
            {
                if (to >= StartDateTime_ForLayer)
                    ToDateTime_ForLayer = to;
                else
                    ToDateTime_ForLayer = StartDateTime_ForLayer;
            }
        }

        public void showServicePerformanceInfo(ServiceQoSInfoForHistory results)
        {
            StartDateTime_ForService = results.startDateTime;
            EndDateTime_ForService = results.endDateTime;

            setServiceQoSDateTimeFromAndTo(FromDateTime_ForService, ToDateTime_ForService);
            setServiceQoSDatePickerFromAndTo();

            datePicker_From_ForService.DisplayDateStart = StartDateTime_ForService;
            datePicker_From_ForService.DisplayDateEnd = EndDateTime_ForService;

            StackPanel_ServiceQoS_Chart.DataContext = results.measurementInfoList;
            if (latestMeasurementInfoList_ForService == null)
                latestMeasurementInfoList_ForService = results.measurementInfoList;

            foreach (Series s in FGDCScoreChart_ServiceQoS.Series)
            {
                if (s is ColumnSeries)
                {
                    ColumnSeries cs = s as ColumnSeries;
                    var labelStyle = new Style(typeof(AxisLabel));
                    //when the num of measurement info is larger than 12, we will change FGDC Score Chart's view style
                    if (results != null && results.measurementInfoList.Count > 12)
                    {
                        //cs.IndependentValueBinding.StringFormat = "";
                        labelStyle.Setters.Add(new Setter(AxisLabel.StringFormatProperty, ""));
                        cs.IndependentAxis = new CategoryAxis { Orientation = AxisOrientation.X };

                    }
                    else
                    {
                        labelStyle.Setters.Add(new Setter(AxisLabel.StringFormatProperty, "{0:M}"));
                        cs.IndependentAxis = new CategoryAxis { Orientation = AxisOrientation.X };
                    }
                    CategoryAxis axis = (CategoryAxis)cs.IndependentAxis;
                    axis.AxisLabelStyle = labelStyle;

                    //Style dps = cs.DataPointStyle;
                    ////DependencyObject DO =  VisualTreeHelper.GetChild(cs, 0);
                    ////Border b = (Border)VisualTreeHelper.GetChild(dps, 0);
                    //ToolTip tt = new ToolTip();
                    //tt.Content = "dfdsfs";
                    //Style tstyle = (Style)Application.Current.Resources["ToolTipTemplate"];
                    //tt.Style = tstyle;

                    ////cs.SetValue(ToolTipService.ToolTipProperty, tt);
                    //ControlTemplate ct = cs.Template;

                    //ct.SetValue(ToolTipService.ToolTipProperty, tt);
                    //dps.SetValue(ToolTipService.ToolTipProperty, tt);

                    //object o1 = dps.GetValue(ToolTipService.ToolTipProperty);
                    //dps.SetValue(ToolTipService.ToolTipProperty, tt);


                    //foreach (Setter m in dps.Setters)
                    //{
                    //    if (m.Value is ControlTemplate)
                    //    {
                    //        ControlTemplate ct = m.Value as ControlTemplate;
                    //        //DependencyProperty SpeciesProperty = DependencyProperty.Register("", typeof(Grid), typeof(UserControl), null);
                    //        object o = ct.GetValue(ToolTipService.ToolTipProperty);
                    //        //object o = ct.GetValue(DependencyProperty );
                    //        //ct.SetValue(ToolTipService.ToolTipProperty, tt);

                    //        DependencyProperty property = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Style), new PropertyMetadata(null));
                    //        object o2 = ct.GetValue(property);
                    //        //object o3 = ct.GetValue(System.Windows.CoreDependencyProperty);

                    //        DependencyObject vc = VisualTreeHelper.GetChild(dps, 0);

                    //        //DependencyObject t = dps.Template.LoadContent();

                    //        //var fis = LogicalTreeHelperEx.GetChildrenByType<TextBlock>(t, true);
                    //        //var fis = VisualTreeHelperEx.GetChildsByType<TextBlock>(vc, true);

                    //    }
                    //}
                }
            }

            //when the timespan excess 12 day, we will change ResponeTime Chart for Service's view style
            int num = (ToDateTime_ForService - FromDateTime_ForService).Days;
            foreach (Axis a in ResponeTimeChart_ServiceQoS.Axes)
            {
                if (a is DateTimeAxis)
                {
                    DateTimeAxis da = a as DateTimeAxis;
                    double interval = 20;
                    if (results != null)
                        interval = getInterval_DateTimeAxis_LineSeriesChart(num, results.measurementInfoList.Count);
                    else
                        interval = getInterval_DateTimeAxis_LineSeriesChart(num, 0);
                    da.Interval = interval;
                }
            }
        }

        private void GetLatestQoS_Service_Click(object sender, RoutedEventArgs e)
        {
            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-serviceQoSHistoralInterval+1);
            setServiceQoSDateTimeFromAndTo(from, to);
            setServiceQoSDatePickerFromAndTo();
            if (latestMeasurementInfoList_ForService != null)
                StackPanel_ServiceQoS_Chart.DataContext = latestMeasurementInfoList_ForService;
            else
                queryPerformanceFunctions.getGeospatialServiceQoSHistoricalInfo(record, FromDateTime_ForService, ToDateTime_ForService);
        }

        private void GetPreviousQoS_Service_Click(object sender, RoutedEventArgs e)
        {
            if (queryPerformanceFunctions != null && record != null)
            {
                DateTime from = FromDateTime_ForService.AddDays(-serviceQoSHistoralInterval);
                DateTime to = ToDateTime_ForService.AddDays(-serviceQoSHistoralInterval);
                if (from <= StartDateTime_ForService && to <= StartDateTime_ForService)
                {
                    if (FromDateTime_ForService > StartDateTime_ForService)
                    {
                        from = StartDateTime_ForService;
                        to = from.AddDays(serviceQoSHistoralInterval-1);
                        setServiceQoSDateTimeFromAndTo(from, to);
                        setServiceQoSDatePickerFromAndTo();
                        queryPerformanceFunctions.getGeospatialServiceQoSHistoricalInfo(record, FromDateTime_ForService, ToDateTime_ForService);  
                    }
                }
                else
                {
                    setServiceQoSDateTimeFromAndTo(from, to);
                    setServiceQoSDatePickerFromAndTo();
                    queryPerformanceFunctions.getGeospatialServiceQoSHistoricalInfo(record, FromDateTime_ForService, ToDateTime_ForService);                
                }
            }
        }

        private void GetQoSFromDatePickerDateTime_Service_Click(object sender, RoutedEventArgs e)
        {
            if (queryPerformanceFunctions != null && record != null)
            {
                setServiceQoSDateTimeFromAndTo((DateTime)datePicker_From_ForService.SelectedDate, (DateTime)datePicker_To_ForService.SelectedDate);
                queryPerformanceFunctions.getGeospatialServiceQoSHistoricalInfo(record, FromDateTime_ForService, ToDateTime_ForService);
            }
        }

        private void GetLatestQoS_Layer_Click(object sender, RoutedEventArgs e)
        {
            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-layerQoSHistoralInterval+1);
            setLayerQoSDateTimeFromAndTo(from, to);
            setLayerQoSDatePickerFromAndTo();
            queryPerformanceFunctions.getWMSLayerQoSHistoralInfo(record, layerName, FromDateTime_ForLayer, ToDateTime_ForLayer);
        }

        private void GetPreviousQoS_Layer_Click(object sender, RoutedEventArgs e)
        {
            if (queryPerformanceFunctions != null && record != null)
            {
                DateTime from = FromDateTime_ForLayer.AddDays(-layerQoSHistoralInterval);
                DateTime to = ToDateTime_ForLayer.AddDays(-layerQoSHistoralInterval);
                if (from < StartDateTime_ForLayer && to < StartDateTime_ForLayer)
                {
                    if (FromDateTime_ForLayer > StartDateTime_ForLayer)
                    {
                        from = StartDateTime_ForLayer;
                        to = from.AddDays(layerQoSHistoralInterval-1);
                        setLayerQoSDateTimeFromAndTo(from, to);
                        setLayerQoSDatePickerFromAndTo();
                        queryPerformanceFunctions.getWMSLayerQoSHistoralInfo(record, layerName, FromDateTime_ForLayer, ToDateTime_ForLayer);
                    }
                }
                else
                {
                    setLayerQoSDateTimeFromAndTo(from, to);
                    setLayerQoSDatePickerFromAndTo();
                    queryPerformanceFunctions.getWMSLayerQoSHistoralInfo(record, layerName, FromDateTime_ForLayer, ToDateTime_ForLayer);                 
                }           
            }
        }

        private void GetQoSFromDatePickerDateTime_Layer_Click(object sender, RoutedEventArgs e)
        {
            if (queryPerformanceFunctions != null && record != null)
            {
                setLayerQoSDateTimeFromAndTo((DateTime)datePicker_From_ForLayer.SelectedDate, (DateTime)datePicker_To_ForLayer.SelectedDate);
                queryPerformanceFunctions.getWMSLayerQoSHistoralInfo(record, layerName, FromDateTime_ForLayer, ToDateTime_ForLayer);
            }
        }

        private void Button_WMS_Layers_QoS_Click(object sender, RoutedEventArgs e)
        {
            if (isPanelExtended == false)
            {              
                if (TreeView_WMSLayer.Items.Count == 0)
                {
                    queryPerformanceFunctions.getWMSLayerQoSSummaryInfo(record);
                }
                StackPanel_WMSLayerQoS.Visibility = Visibility.Visible;
                Button_WMS_Layers_QoS.Content = "<<   Hide Layers QoS Panel";
                isPanelExtended = true;
                TextBox_Title.MaxWidth = 700;
            }
            else
            {
                StackPanel_WMSLayerQoS.Visibility = Visibility.Collapsed;
                Button_WMS_Layers_QoS.Content = ">>   Show Layers QoS Panel";
                isPanelExtended = false;
                TextBox_Title.MaxWidth = 340;
            }
        }

        public void showLayerInfoOnTreeView(WMSLayerInfoWithQoS wmsLayerInfoWithQoS)
        {
            TreeViewItem rootTreeNode = new TreeViewItem();

            RadioButtonWithTextAndMultiIcon area1 = new RadioButtonWithTextAndMultiIcon();
            area1.TextBlock_Name.Text = "All layers (" + wmsLayerInfoWithQoS.WMSLayersList.Count + ")";
            area1.TextBlock_Name.FontSize = 15;
            area1.TextBlock_Name.FontWeight = FontWeights.Bold;
            area1.isSelected_RadioButton.Visibility = Visibility.Collapsed;
            area1.Image_Layer.Visibility = Visibility.Collapsed;
            area1.Image_QoS.Visibility = Visibility.Collapsed;

            //we customs the TreeviewItem: add checkbox and image icon before title text content,
            //then we use our TreeViewItem UserControl as the Header Object of TreeViewItem.
            rootTreeNode.Header = area1;
            rootTreeNode.IsExpanded = true;

            foreach (GeoSearch.QualityQueryServiceReference.WMSLayer layer in wmsLayerInfoWithQoS.WMSLayersList)
            {
                TreeViewItem layerTreeNode = new TreeViewItem();
                RadioButtonWithTextAndMultiIcon area2 = new RadioButtonWithTextAndMultiIcon();
                //we show the layer's title in tree view as tree node
                area2.TextBlock_Name.Text = layer.title;
                //and the true layer is taged as the object
                area2.TextBlock_Name.Tag = layer.name;
                area2.Image_Map.Visibility = Visibility.Collapsed;

                BitmapImage image = new BitmapImage(new Uri(Converters.getQoSBarImagePathString(layer.rankValue), UriKind.Relative));
                area2.Image_QoS.Source =  image;

                area2.isSelected_RadioButton.Checked += new RoutedEventHandler(radioButton_Selected);
                layerTreeNode.Selected += new RoutedEventHandler(layerTreeNode_Selected);
                layerTreeNode.Header = area2;
                rootTreeNode.Items.Add(layerTreeNode);
            }
            this.TreeView_WMSLayer.Items.Add(rootTreeNode);
        }

        public void showLayerHistoryInfoOnChart(LayerQoSInfoForHistory layerQoSInfoForHistory)
        {
            Grid_WMSLayerQoS.Visibility = Visibility.Visible;

            StartDateTime_ForLayer = layerQoSInfoForHistory.startDateTime;
            EndDateTime_ForLayer = layerQoSInfoForHistory.endDateTime;

            setLayerQoSDateTimeFromAndTo(FromDateTime_ForLayer, ToDateTime_ForLayer);
            setLayerQoSDatePickerFromAndTo();

            datePicker_From_ForLayer.DisplayDateStart = StartDateTime_ForLayer;
            datePicker_From_ForLayer.DisplayDateEnd = EndDateTime_ForLayer;
            ResponeTimeChart_Layer.DataContext = layerQoSInfoForHistory.measurementInfoList;

            //when the timespan excess 12 day, we will change ResponeTime Chart for Layer's view style
            int num = (ToDateTime_ForLayer - FromDateTime_ForLayer).Days;
            foreach (Axis a in ResponeTimeChart_Layer.Axes)
            {
                if (a is DateTimeAxis)
                {
                    DateTimeAxis da = a as DateTimeAxis;

                    double interval = 20;
                    if(layerQoSInfoForHistory != null)
                        interval = getInterval_DateTimeAxis_LineSeriesChart(num, layerQoSInfoForHistory.measurementInfoList.Count);
                    else
                        interval = getInterval_DateTimeAxis_LineSeriesChart(num, 0);
                    da.Interval = interval;
                }
            }
        }

        double getInterval_DateTimeAxis_LineSeriesChart(int timespan_Days, int recordsNum)
        {
            double interval = 20;

            if (timespan_Days >= 12)
            {
                interval = timespan_Days / 4;
            }
            else
            {
                interval = 2;
            }

            if (recordsNum < 3)
            {
                interval = 120;
            }
            return interval;
        }

        void radioButton_Selected(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            if (sender is RadioButton)
            {
                RadioButton button = sender as RadioButton;
                StackPanel StackPanel = button.Parent as StackPanel;
                RadioButtonWithTextAndMultiIcon area = StackPanel.Parent as RadioButtonWithTextAndMultiIcon;
                area.isSelected_RadioButton.IsChecked = true;
                layerName = area.TextBlock_Name.Tag as string;
                List<Object> list = TreeView_WMSLayer.Items.ToList();
                foreach (Object object1 in list)
                {
                    if (object1 is TreeViewItem)
                    {
                        TreeViewItem LayersTreeNode = object1 as TreeViewItem;
                        IEnumerable<TreeViewItem> TreeViewItemList = LayersTreeNode.GetDescendantContainers();
                        foreach (TreeViewItem treeViewItem in TreeViewItemList)
                        {
                            RadioButtonWithTextAndMultiIcon area1 = treeViewItem.Header as RadioButtonWithTextAndMultiIcon;
                            if (area1 != area)
                            {
                                area1.isSelected_RadioButton.IsChecked = false;
                            }
                        }
                    }
                }
            }

            DateTime to = DateTime.Now;
            DateTime from = to.AddDays(-layerQoSHistoralInterval+1);
            setLayerQoSDateTimeFromAndTo(from, to);
            setLayerQoSDatePickerFromAndTo();
            queryPerformanceFunctions.getWMSLayerQoSHistoralInfo(record, layerName, FromDateTime_ForLayer, ToDateTime_ForLayer);
        }

        void layerTreeNode_Selected(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            string layerName = null;
            if (sender is TreeViewItem)
            {         
                TreeViewItem layerTreeNode = sender as TreeViewItem;
                TreeViewItem LayersTreeNode = layerTreeNode.GetParentTreeViewItem();
                IEnumerable<TreeViewItem> TreeViewItemList = LayersTreeNode.GetDescendantContainers();
                foreach (TreeViewItem treeViewItem in TreeViewItemList)
                {
                    if (treeViewItem != layerTreeNode)
                    {
                        RadioButtonWithTextAndMultiIcon area1 = treeViewItem.Header as RadioButtonWithTextAndMultiIcon;
                        area1.isSelected_RadioButton.IsChecked = false;
                    }
                }
                RadioButtonWithTextAndMultiIcon area = layerTreeNode.Header as RadioButtonWithTextAndMultiIcon;
                area.isSelected_RadioButton.IsChecked = true;
                Grid_WMSLayerQoS.Visibility = Visibility.Visible;
                layerName = area.TextBlock_Name.Tag as string;
            }
        }

        private void datePicker_From_ForService_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime from = (DateTime)datePicker_From_ForService.SelectedDate;
            DateTime to = from.AddDays(serviceQoSHistoralInterval-1);
            if(to>EndDateTime_ForService)
            {
                if(EndDateTime_ForService > from)
                    to = EndDateTime_ForService;
            }

            setServiceQoSDateTimeFromAndTo(from, to);
            setServiceQoSDatePickerFromAndTo();

        }

        private void datePicker_From_ForLayer_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime from = (DateTime)datePicker_From_ForLayer.SelectedDate;
            DateTime to = from.AddDays(layerQoSHistoralInterval -1);
            if (to > EndDateTime_ForLayer)
            {
                if (EndDateTime_ForLayer > from)
                    datePicker_To_ForLayer.SelectedDate = EndDateTime_ForLayer;
            }

            setLayerQoSDateTimeFromAndTo(from, to);
            setLayerQoSDatePickerFromAndTo();
        }

        private void ComboBox_DateInterval_ForService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_DateInterval_ForService != null)
            {
                ComboBoxItem cbi = ComboBox_DateInterval_ForService.SelectedItem as ComboBoxItem;
                string intervalString = cbi.Content as string;
                intervalString = intervalString.Trim();
                intervalString = intervalString.Replace("days", "");
                serviceQoSHistoralInterval = double.Parse(intervalString);
                //serviceQoSHistoralInterval = -(interval - 1);
            }
        }

        private void ComboBox_DateInterval_ForLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_DateInterval_ForLayer != null)
            {
                ComboBoxItem cbi = ComboBox_DateInterval_ForLayer.SelectedItem as ComboBoxItem;
                string intervalString = cbi.Content as string;
                intervalString = intervalString.Trim();
                intervalString = intervalString.Replace("days", "");
                layerQoSHistoralInterval = double.Parse(intervalString);
                //layerQoSHistoralInterval = -(interval - 1);
            }
        }
    }

    public class WMSLayersQoSVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if (type.Equals(ConstantCollection.ServiceType_WMS))
                    v = Visibility.Visible;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class MeasureDateTimeConverterForToolTip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = "Measure DateTime: ";
            if (value != null && value.GetType() == typeof(DateTime))
            {
                DateTime dateTime = (DateTime)value;
                s += dateTime.ToString();
            }
            return s;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ResponseTimeConverterForToolTip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = "Response Time: ";
            if (value != null && value.GetType() == typeof(double))
            {
                double responseTime = (double)value;
                s += responseTime.ToString();
            }
            return s;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ShortMeasureDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = null;
            if (value != null && value.GetType() == typeof(DateTime))
            {
                DateTime dateTime = (DateTime)value;
                //s = dateTime.ToString("mm/dd");
                s = dateTime.ToString();
            }
            return s;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    } 
}


