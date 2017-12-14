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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Browser;
using System.Collections;
using System.Windows.Data;
using System.Globalization;
using GeoSearch.Widgets;
using GeoSearch.Pages;
using System.ComponentModel;
using GeoSearch.CSWQueryServiceReference;
using System.Collections.Specialized;
using Microsoft.Maps.MapControl;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;

namespace GeoSearch
{
    public partial class SearchingResultPage : UserControl
    {
        public static int ID = 0;
        public static bool default_ClassifiedOrNot = true;
        //public static RecordsSearchFunctions searchingFunctions = new RecordsSearchFunctions(-999);
        public static OtherQueryFuntions otherQueryFunction = new OtherQueryFuntions();
        
        //public ObservableCollection<Record> FoundRecords = new ObservableCollection<Record>();
        public ObservableCollection<ClientSideRecord> FoundRecords = new ObservableCollection<ClientSideRecord>();
        public PagedCollectionView UncategoriedPagedCollectionView = null;
        
        public List<CategoryPageMap> categoryPage_records = new List<CategoryPageMap>();

        public static PopupPage_ShowMetadataDetails metadataPopup = new PopupPage_ShowMetadataDetails();
        public static GeneralPopup generalPopup = new GeneralPopup();
        public static SBAQuickSearchPopup SBAPopup = null;
   
        public MapLayer m_PushpinLayer;
        public MapLayer m_RecordsBBoxLayer;
        public MapLayer m_PinInfoBoxLayer;
        public List<RecordAndPushPinMap> recordAndPushPinMap_list = new List<RecordAndPushPinMap>();
        public MyPushPinInfoBox iPushPinInoBox;
        public List<string> WMSList_shownOnBingMapsWMSLayerControl = new List<string>();
        //WMS Layer与其对应BingMaps 图层的映射
        public Dictionary<OtherQueryFunctionsServiceReference.CascadedWMSLayer, MapTileLayer> WMSLayer_MapLayer_Map = new Dictionary<OtherQueryFunctionsServiceReference.CascadedWMSLayer, MapTileLayer>();
        //public TabItem tabItem_mapContainerNestIn = null;
        //当前动画设置准备要显示的图层
        public List<string> TimeEnableWMSLayer_TimeList_toBeShown = new List<string>();
        //当前图层包括的所有time-enable图层
        public List<string> TimeEnableWMSLayer_TimeList_all = new List<string>();
        public int Index_WMSAnimationFrom = -1;
        public int Index_WMSAnimationTo = -1;
        //Flag来标识是否需要计算当前要做animation的潜在time-enable图层
        public bool needToCalculateTimeList = false;
        //潜在的Animation图层的时间与MapTileLayer的映射
        public Dictionary<string, MapTileLayer> WMSLayerTimeName_MapLayer_Map = new Dictionary<string, MapTileLayer>();

        //Animation状态的枚举对象
        public enum AnimationState
        {
            Started,
            Stoped,
            Paused,
            ReadyToStart,
            NotReady
        };
        public AnimationState animationState = AnimationState.ReadyToStart;
        //Animation的显示模式是一次把所有的图层都加上去还是只加载将要显示的几个
        public bool AnimationMode_AllLayerAddAtBegining = false;
        //如果是非一次加载图层的模式，那么允许同时加载的动画图层的数量
        public static int initalAddMapTileNum_Animation = 10;

        //标识Animation中当前显示的图层的index
        public int currentShowIndex_Animation = 0;

        public System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public const double Opacity_Default_WMSLayers = 0.5;
        public double Opacity_WMSLayers = 0.5;
        public double Opacity_WMSAnimationLayers = 0.6; 

        public bool noRecord = true;
        public bool classifiedWithCategories = true;
        public bool isRecordsListShow = true;
        public bool isBingMapShow = true;
        public bool isBingMapFullScreen = false;

        public double BingMap_Height = 0.0;
        public double BingMap_Width = 0.0;

        public const double smallChangeHight = 10;
        public const double listBoxWidthChangeInterval = 15;
        public const double narrowListBox_Width = 465;
        public const double wideListBox_Width = 830;

        public static SearchingResultPage currentResultPage;

        //Cache part 
        public static Dictionary<String,OtherQueryFunctionsServiceReference.HierachicalWMSLayers> cachedWMSLayer_Map = new Dictionary<String,OtherQueryFunctionsServiceReference.HierachicalWMSLayers>();
      
        //改变Browser窗体大小时的事件
        void content_Resized(object sender, EventArgs e)
        {
            setContentHeightWidthToFixBrowserWindowSize();
            changeControlLocationOnBingMaps();
        }

        //设置BingMaps上的浮动控件的位置使其适合窗体的大小变化
        public void changeControlLocationOnBingMaps()
        {
            //若BingMaps的高度过窄，则将Animation控件移动到右边，否则在放置在左边WMS Layer的下面
            if (myBingMap.ActualHeight < 660 && myBingMap.ActualWidth>=700)
            {
                if (AnimationControlBorder.Parent != RightContainerOnTopOfBingMaps_StackPanel)
                {
                    StackPanel sp = AnimationControlBorder.Parent as StackPanel;
                    sp.Children.Remove(AnimationControlBorder);
                    RightContainerOnTopOfBingMaps_StackPanel.Children.Add(AnimationControlBorder);
                }
            }
            else
            {
                if (AnimationControlBorder.Parent != LeftContainerOnTopOfBingMaps_StackPanel)
                {
                    StackPanel sp = AnimationControlBorder.Parent as StackPanel;
                    sp.Children.Remove(AnimationControlBorder);
                    LeftContainerOnTopOfBingMaps_StackPanel.Children.Add(AnimationControlBorder);
                }
            }
        }

        //设置Results_Container的高度
        public void setContentHeightWidthToFixBrowserWindowSize()
        {
            double content_height = (double)HtmlPage.Window.Eval("document.documentElement.clientHeight") - 150;
            double content_width = (double)HtmlPage.Window.Eval("document.documentElement.clientWidth");
            if (content_height < 300)
                content_height = 300;
            if (content_width < 600)
                content_width = 600;

            content_width -= 10;
            Results_Container.Height = content_height;
            Results_Container.Width = content_width;
            border_Header.Width = content_width;
            border_Bottom.Width = content_width;
            if (isBingMapShow && isRecordsListShow)
            {
                DataTemplate dt = (DataTemplate)LayoutRoot.Resources["RecordTemplate"];
                if (classifiedWithCategories)
                {
                    Map_Container.Height = Results_Container.Height - 100;
                    foreach (CategoryPageMap map1 in categoryPage_records)
                    {
                        if (map1.listBox != null)
                        {
                            if (map1.pagedCollectionView != null && map1.pagedCollectionView.Count > 4)
                                map1.listBox.Width = narrowListBox_Width+ listBoxWidthChangeInterval;
                            else
                                map1.listBox.Width = narrowListBox_Width;
                            map1.listBox.Height = Map_Container.Height;
                            map1.listBox.ItemTemplate = dt;
                        }
                    }
                }
                else
                {
                    Map_Container.Height = Results_Container.Height - 70;
                    RecordsList.Height = Map_Container.Height;
                    PagedCollectionView view = RecordsList.ItemsSource as PagedCollectionView;
                    if (view != null && view.Count >4)
                        RecordsList.Width = narrowListBox_Width + listBoxWidthChangeInterval;
                    else
                        RecordsList.Width = narrowListBox_Width;
                    RecordsList.ItemTemplate = dt;
                }
                MetaDateDetail_Container.Height = Map_Container.Height -20;
                Map_Container.Width = content_width - 500;
            }
            else if (isBingMapShow && !isRecordsListShow)
            {
                Map_Container.Width = content_width - 12;
            }
            else if (!isBingMapShow && isRecordsListShow)
            {
                DataTemplate dt = (DataTemplate)LayoutRoot.Resources["RecordTemplate_800"];           
                if (classifiedWithCategories)
                {
                    Map_Container.Height = Results_Container.Height - 90;
                    foreach (CategoryPageMap map1 in categoryPage_records)
                    {
                        if (map1.listBox != null)
                        {
                            map1.listBox.Width = wideListBox_Width;
                            map1.listBox.ItemTemplate = dt;
                        }
                    }
                }
                else
                {
                    RecordsList.Width = wideListBox_Width;
                    RecordsList.ItemTemplate = dt;
                }
            }
        }

        //添加了内容后，由于出现水平和垂直滚动条，导致页面不美观，需要调整内容的高度来隐藏垂直滚动条
        public static void resizeResultContainerHeight(SearchingResultPage srp, double smallChange)
        {
            if (srp.classifiedWithCategories)
            {
                TabItem ti = srp.TabControl_CategoryPage.SelectedItem as TabItem;
                foreach (CategoryPageMap map in srp.categoryPage_records)
                {
                    if (map.listBox != null)
                    {
                        map.listBox.Height += smallChange;
                        srp.Map_Container_Container.Height += smallChange;
                        srp.button_Container.Height += smallChange;
                        srp.Map_Container.Height += smallChange;
                        if (smallChange > 0)
                            srp.Map_Container.Height -= 6;
                        else if (smallChange < 0)
                            srp.Map_Container.Height += 6;
                    }
                }
            }
            else
            {
                srp.RecordsList.Height += smallChange;
                srp.Map_Container_Container.Height += smallChange;
                srp.button_Container.Height += smallChange;
                srp.Map_Container.Height += smallChange;
                if (smallChange > 0)
                    srp.Map_Container.Height -= 6;
                else if (smallChange < 0)
                    srp.Map_Container.Height += 6;
            }
        }

        public void maybeChangeListBoxWidth(PagedCollectionView view)
        {
            if (classifiedWithCategories)
            {
                foreach (CategoryPageMap map in categoryPage_records)
                {
                    if (view == map.pagedCollectionView)
                    {
                        if (map.listBox != null)
                        {
                            if (isBingMapShow && isRecordsListShow)
                            {
                                if (view.Count > 4)
                                    map.listBox.Width = narrowListBox_Width + listBoxWidthChangeInterval;
                                else
                                    map.listBox.Width = narrowListBox_Width;
                            }
                            else if (!isBingMapShow && isRecordsListShow)
                            {
                                map.listBox.Width = wideListBox_Width;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                if (isBingMapShow && isRecordsListShow)
                {
                    if (view.Count > 4)
                        RecordsList.Width = narrowListBox_Width + listBoxWidthChangeInterval;
                    else
                        RecordsList.Width = narrowListBox_Width;
                }
                else if (!isBingMapShow && isRecordsListShow)
                {
                    RecordsList.Width = wideListBox_Width;
                }
            }
        }

        //public static void addPushpin(double latitude, double longitude)
        //{
        //    Pushpin pushpin = new Pushpin();
        //    pushpin.MouseEnter += OnMouseEnter;
        //    pushpin.MouseLeave += OnMouseLeave;
        //    m_PushpinLayer.AddChild(pushpin, new Location(latitude, longitude), PositionOrigin.BottomCenter);
        //}

        //public static void addPushpinInfoBox(Record record)
        //{
        //    //pushpin = new Pushpin();
        //    //pushpin.MouseEnter += OnMouseEnter;
        //    //pushpin.MouseLeave += OnMouseLeave;
        //    //m_PushpinLayer.AddChild(pushpin, new Location(latitude, longitude), PositionOrigin.BottomCenter);
        //}

        //private static void OnMouseLeave(object sender, MouseEventArgs e)
        //{
        //    Pushpin pushpin = sender as Pushpin;
        //    // remove the pushpin transform when mouse leaves 
        //    pushpin.RenderTransform = null;
        //}

        //private static void OnMouseEnter(object sender, MouseEventArgs e)
        //{
        //    Pushpin pushpin = sender as Pushpin;

        //    // scaling will shrink (less than 1) or enlarge (greater than 1) source element 
        //    ScaleTransform st = new ScaleTransform();
        //    st.ScaleX = 1.4;
        //    st.ScaleY = 1.4;

        //    // set center of scaling to center of pushpin 
        //    st.CenterX = (pushpin as FrameworkElement).Height / 2;
        //    st.CenterY = (pushpin as FrameworkElement).Height / 2;

        //    pushpin.RenderTransform = st;
        //}



        //create a new MyPushPinInfoBox, clear original MyPushPinInfoBox in m_PinInfoBoxLayer and put new create one into it and set position
        //根据record信息创建新的MyPushPinInfoBox对象，作为m_PinInfoBoxLayer中新的UIElement内容，并设置位置
        public MyPushPinInfoBox createNewPushpinInfoBox(ClientSideRecord record, Pushpin pin)
        {   
            if (m_PinInfoBoxLayer != null)
            {
                m_PinInfoBoxLayer.Children.Clear();
                MyPushPinInfoBox iPushPinInoBox = new MyPushPinInfoBox();
                iPushPinInoBox.DataContext = record;
                m_PinInfoBoxLayer.Children.Add(iPushPinInoBox);
                MapLayer.SetPosition(iPushPinInoBox, pin.Location);
                MapLayer.SetPositionOrigin(iPushPinInoBox, PositionOrigin.BottomCenter);
                MapLayer.SetPositionOffset(iPushPinInoBox, new Point(0, -50));
                return iPushPinInoBox;
            }
            else
                return null;
        }

        //高亮显示指定record对应的PushPin的BBOX并显示基本信息框MyPushPinInfoBox
        public void highlightSpecifiedRecord(ClientSideRecord record, MyPushPinInfoBox.CreationEventType type)
        {
            m_PinInfoBoxLayer.Children.Clear();
            if (recordAndPushPinMap_list.Count > 0 && record != null)
            {
                foreach (RecordAndPushPinMap map in recordAndPushPinMap_list)
                {
                    if (map.record == record)
                    {
                        //var currentPinInfoBoxes = pushPins_list[index];
                        //currentPinInfoBoxes.setOptions({ visible: true });
                        createNewPushpinInfoBox(record, map.pin);
                        map.bboxPolygon.Fill = new SolidColorBrush(Utilities.purple);
                        map.bboxPolygon.Stroke = new SolidColorBrush(Utilities.purple);
                    }
                    else
                    {
                        //pin = pinInfoBoxs_Array[i].setOptions({ visible: false });
                        map.bboxPolygon.Fill = new SolidColorBrush(Utilities.polygonFillColor);
                        map.bboxPolygon.Stroke = new SolidColorBrush(Utilities.polygonLineColor);
                    }
                }
            }
        }

        //添加指定record对应的pushpin和bbox到bingmap的不同图层
        public void addPushpinAndBBox(ClientSideRecord record)
        {
            Location loc = null;
            Pushpin pin = new Pushpin();
            // Define the pushpin location
            if (record.bbox != null)
                loc = Utilities.getCenterLocation(record.bbox);
            else
            {
                string[] location = record.URLLocation.Split(',');
                try
                {
                    double lon = double.Parse(location[0]);
                    double lat = double.Parse(location[1]);
                    loc = new Location(lat, lon);
                }
                catch (Exception e)
                {
                    e.ToString();
                }
                pin.Background = new SolidColorBrush(Utilities.blue);
            }
            // Add a pin to the map        
            pin.Location = loc;
            pin.Content = recordAndPushPinMap_list.Count + 1;
            pin.MouseEnter += new MouseEventHandler(pin_MouseEnter);
            pin.MouseLeave += new MouseEventHandler(pin_MouseLeave);
            pin.MouseLeftButtonUp += new MouseButtonEventHandler(pin_MouseLeftButtonUp);

            //define the bbox rectangles
            MapPolygon polygon = null;
            if(record.bbox != null)
                polygon = Utilities.getBBoxPolygon(record.bbox);
            else
                polygon = Utilities.getBBoxPolygon(loc);
            m_PushpinLayer.Children.Add(pin);
            m_RecordsBBoxLayer.Children.Add(polygon);
            recordAndPushPinMap_list.Add(new RecordAndPushPinMap(record, pin, polygon));
        }

        //pushpin上的鼠标左键up事件
        public void pin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Pushpin)
            {
                Pushpin pin1 = sender as Pushpin;
                foreach (RecordAndPushPinMap map in recordAndPushPinMap_list)
                {
                    //if (map.pin == pin1)
                    //{
                    //    MyPushPinInfoBox infoBox = createNewPushpinInfoBox(map.record, pin1);
                    //    infoBox.creationEventType = MyPushPinInfoBox.CreationEventType.MouseLeftButtonClick;
                    //    map.bboxPolygon.Fill = new SolidColorBrush(purple);
                    //    map.bboxPolygon.Stroke = new SolidColorBrush(purple);
                    //}
                    //else
                    //{
                    //    //pin = pinInfoBoxs_Array[i].setOptions({ visible: false });
                    //    map.bboxPolygon.Fill = new SolidColorBrush(polygonFillColor);
                    //    map.bboxPolygon.Stroke = new SolidColorBrush(polygonLineColor);
                    //}

                    if (map.pin == pin1)
                    {
                        ClientSideRecord record = map.record;
                        if (classifiedWithCategories)
                        {
                            TabItem currentTabItem = TabControl_CategoryPage.SelectedItem as TabItem;
                            //highlightSpecifiedRecord(record, MyPushPinInfoBox.CreationEventType.MouseLeftButtonClick);
                            iPushPinInoBox.creationEventType = MyPushPinInfoBox.CreationEventType.MouseLeftButtonClick;
                            foreach (CategoryPageMap map1 in categoryPage_records)
                            {
                                if (currentTabItem == map1.tabItem)
                                {
                                    PagedCollectionView view = map1.pagedCollectionView;
                                    if (view != null)
                                    {
                                        view.MoveCurrentTo(record);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            PagedCollectionView view = RecordsList.ItemsSource as PagedCollectionView;
                            view.MoveCurrentTo(record);            
                        }
                    }
                }
            }
        }

        //pushpin上的鼠标离开事件
        public void pin_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Pushpin)
            {
                //Pushpin pin = sender as Pushpin;               
                foreach(Object o in m_PinInfoBoxLayer.Children)
                {
                    if(o is MyPushPinInfoBox)
                    {
                        MyPushPinInfoBox infoBox = o as MyPushPinInfoBox;
                        if(infoBox.creationEventType == MyPushPinInfoBox.CreationEventType.MouseHoverIn)
                            m_PinInfoBoxLayer.Children.Clear();
                    }
                }
            }        
        }

        //pushpin上的鼠标进入事件
        public void pin_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Pushpin)
            {
                Pushpin pin1 = sender as Pushpin;
                foreach (RecordAndPushPinMap map in recordAndPushPinMap_list)
                {
                    if (map.pin == pin1)
                    {
                        MyPushPinInfoBox infoBox = createNewPushpinInfoBox(map.record, pin1);
                        iPushPinInoBox = infoBox;
                        infoBox.creationEventType = MyPushPinInfoBox.CreationEventType.MouseHoverIn;
                        map.bboxPolygon.Fill = new SolidColorBrush(Utilities.purple);
                        map.bboxPolygon.Stroke = new SolidColorBrush(Utilities.purple);
                    }
                    else
                    {
                        //pin = pinInfoBoxs_Array[i].setOptions({ visible: false });
                        map.bboxPolygon.Fill = new SolidColorBrush(Utilities.polygonFillColor);
                        map.bboxPolygon.Stroke = new SolidColorBrush(Utilities.polygonLineColor);
                    }
                }
            }
        }

        //清空bingmap上所有图层中的内容，并清空缓存在recordAndPushPinMap_list中的映射对象
        public void initialMapEntitiesAndLists()
        {
            m_PushpinLayer.Children.Clear();
            m_RecordsBBoxLayer.Children.Clear();
            m_PinInfoBoxLayer.Children.Clear();
            recordAndPushPinMap_list.Clear();
        }

        public SearchingResultPage(bool classify)
        {
            currentResultPage = this;
            classifiedWithCategories = classify;
            InitializeComponent();
            //add a map layer in which to draw the Pushpin and bbox
            m_PushpinLayer = new MapLayer();
            //add a map layer in which to draw the PushpinInfoBox
            m_PinInfoBoxLayer = new MapLayer();
            //add a map layer in which to draw the records' bbox
            m_RecordsBBoxLayer = new MapLayer();
            
            //图层的顺序是先添加的在下面后添加在上层，因此为了能够选中pushpin必须让BBOX在pushpin的下层，先加进去
            myBingMap.Children.Add(m_RecordsBBoxLayer);
            myBingMap.Children.Add(m_PushpinLayer);
            myBingMap.Children.Add(m_PinInfoBoxLayer);
            //地图视角变化时，调整中心点坐标文本框的值
            myBingMap.ViewChangeOnFrame += new EventHandler<MapEventArgs>(myBingMap_ViewChangeOnFrame);
            
            if (!classifiedWithCategories)
            {
                RecordsList.DataContext = FoundRecords;
                Grid_UnclassifiedContainer.Visibility = Visibility.Visible;
                TabControl_CategoryPage.Visibility = Visibility.Collapsed;
            }
            else
            {
                Grid_UnclassifiedContainer.Visibility = Visibility.Collapsed;
                ObservableCollection<ResourceTypeTree> types = ResourceTypeTree.getResourceTypeList();
                foreach (ResourceTypeTree type1 in types)
                {
                    string key = type1.ResourceTypeID;
                    ObservableCollection<ClientSideRecord> recordsList = new ObservableCollection<ClientSideRecord>();
                    CategoryPageMap map = null;
                    if (key.Equals(ConstantCollection.resourceType_PickID_AllServices))
                    {
                        TabItem_WebServices.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_WebServices, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_Datasets))
                    {
                        TabItem_Datasets.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_Datasets, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_MonitoringAndObservationSystems))
                    {
                        TabItem_MonitoringAndObservationSystems.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_MonitoringAndObservationSystems, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_ComputationalModel))
                    {
                        TabItem_ComputationalModel.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_ComputationalModel, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_Initiatives))
                    {
                        TabItem_Initiatives.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_Initiatives, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_WebsitesAndDocuments))
                    {
                        TabItem_WebsitesAndDocuments.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_WebsitesAndDocuments, recordsList);
                    }
                    else if (key.Equals(ConstantCollection.resourceType_SoftwareAndApplications))
                    {
                        TabItem_SoftwareAndApplications.DataContext = recordsList;
                        map = new CategoryPageMap(key, TabItem_SoftwareAndApplications, recordsList);
                    }
                    setRecordCountTextBlock(map);
                    categoryPage_records.Add(map);
                }
                //给每个tabItem添加Focus的事件,并且把各子元素控件添加到map中去
                foreach (CategoryPageMap map in categoryPage_records)
                {
                    TabItem ti = map.tabItem;
                    //Set the DataPager and ListBox to the same data source.
                    Grid g = ti.Content as Grid;
                    foreach (UIElement uiElement in g.Children)
                    {                 
                        if (uiElement is DataPager)
                        {
                            DataPager dp = uiElement as DataPager;
                            map.dataPager = dp;
                        }
                        else if (uiElement is StackPanel)
                        {
                            StackPanel sp1 = uiElement as StackPanel;
                            map.stackPanel_SortingRuleComboList = sp1;
                            foreach (UIElement uiElement1 in sp1.Children)
                            {
                                if (uiElement1 is ComboBox)
                                {
                                    ComboBox cb = uiElement1 as ComboBox;
                                    map.comboBox_SortingRule = cb;
                                }
                            }
                        }
                        else if (uiElement is ScrollViewer)
                        {
                            ScrollViewer sv = uiElement as ScrollViewer;
                            if (sv.Content is StackPanel)
                            {
                                StackPanel sp2 = sv.Content as StackPanel;
                                
                                foreach (UIElement uiElement1 in sp2.Children)
                                {
                                    if (uiElement1 is ListBox)
                                    {
                                        ListBox lb = uiElement1 as ListBox;
                                        map.listBox = lb;
                                        map.stackPanelContainer_ForRecordsListBoxAndBingMap = sp2;
                                    }
                                }
                            }     
                        }
                    }

                    ti.GotFocus += new RoutedEventHandler(tabItem_GotFocus);
                }
            }
            SearchContentTextBox.KeyUp += new KeyEventHandler(SearchContentTextBox_KeyUp);
            this.Loaded += new RoutedEventHandler(setSearchContentTextBoxFocus);
            this.GotFocus += new RoutedEventHandler(SearchingResultPage_GotFocus);

            //设置主体内容大小，来适应浏览器窗口大小及其变化
            setContentHeightWidthToFixBrowserWindowSize();
            //setRationalHeightWidth_For_MapContainer();
            App.Current.Host.Content.Resized += new EventHandler(content_Resized);

            //WMS Layer Animation的时间监听器的响应事件
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
        }

        //地图视角每一帧变化时的事件
        void myBingMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            // Gets the map object that raised this event.
            Map map = sender as Map;
            // Determine if we have a valid map object.
            if (map != null)
            {
                // Gets the center of the current map view for this particular frame.
                Location mapCenter = map.Center;

                // Updates the latitude and longitude values, in real time,
                // as the map animates to the new location.
                txtLatitude.Text = string.Format(CultureInfo.InvariantCulture,
                    "{0:F5}", mapCenter.Latitude);
                txtLongitude.Text = string.Format(CultureInfo.InvariantCulture,
                    "{0:F5}", mapCenter.Longitude);
            }
        }

        //地图单机鼠标的事件
        void myBingMap_MouseClick(object sender, MapMouseEventArgs e)
        {
            Location location = myBingMap.Mode.ViewportPointToLocation(e.ViewportPoint);

            // Updates the latitude and longitude values, in real time,
            // as the map animates to the new location.
            txtLatitude.Text = string.Format(CultureInfo.InvariantCulture,
                "{0:F5}", location.Latitude);
            txtLongitude.Text = string.Format(CultureInfo.InvariantCulture,
                "{0:F5}", location.Longitude);
        }

        //变化tabItem时，切换在地图上显示的内容的事件
        void tabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TabItem)
            {
                TabItem ti0 = (TabItem)sender;
                //将Map_Container_Container从旧的父容器中删除
                Object parent = Map_Container_Container.Parent;
                if (parent is Grid)
                {
                    Grid grid = parent as Grid;
                    grid.Children.Remove(Map_Container_Container);
                }
                else if (parent is StackPanel)
                {
                    StackPanel sp = parent as StackPanel;
                    sp.Children.Remove(Map_Container_Container);
                }

                foreach (CategoryPageMap map in categoryPage_records)
                {
                    TabItem ti = map.tabItem;
                    StackPanel sp = map.stackPanelContainer_ForRecordsListBoxAndBingMap;
                    if (ti0 == ti)
                    {
                        PagedCollectionView view = map.pagedCollectionView;
                        if (view != null)
                        {
                            List<ClientSideRecord> list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(view);
                            showSpecifiedRecordsInBingMap(list);
                        }
                        sp.Children.Add(Map_Container_Container);
                        if(isRecordsListShow)
                            map.listBox.Visibility = Visibility.Visible;
                        else
                            map.listBox.Visibility = Visibility.Collapsed;

                        if (isBingMapShow)
                            Map_Container.Visibility = Visibility.Visible;
                        else
                            Map_Container.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        void SearchingResultPage_GotFocus(object sender, RoutedEventArgs e)
        {
            showSBAVocabularyQuickSearchPage();
        }

        //将结果页面上每个Category Tab上对应的记录条目个数的textblock对象保存到map映射中
        void setRecordCountTextBlock(CategoryPageMap map)
        {
            TabItem ti = map.tabItem;
            StackPanel sp = ti.Header as StackPanel;
            UIElement[] childs = sp.Children.ToArray();
            UIElement uiElement = childs[2];
            if (uiElement is TextBlock)
            {
                TextBlock tb = uiElement as TextBlock;
                map.textblockForRecordCount = tb;
            }
        }

        public void setRecordCountTextBlockValue()
        {
            foreach (CategoryPageMap map in categoryPage_records)
            {
                TabItem ti = map.tabItem;
                map.textblockForRecordCount.Text = "(" + map.recordslist.Count + ")";
            }
        }

        void setSearchContentTextBoxFocus(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();
            SearchContentTextBox.Dispatcher.BeginInvoke(() => { SearchContentTextBox.Focus(); SearchContentTextBox.SelectAll(); });
        }

        void SearchContentTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && RecordsSearchFunctions.cannotStartSearchYet == false)
            {
                startOneNewSearch();
            }
        }

        private void BasicSearching_Click(object sender, RoutedEventArgs e)
        {
            if (RecordsSearchFunctions.cannotStartSearchYet == false)
                startOneNewSearch();
        }

        private void startOneNewSearch()
        {
            if (RecordsSearchFunctions.isContentSearchable(SearchContentTextBox.Text) == false)
                return;

            RecordsSearchFunctions.cannotStartSearchYet = true;
            SearchButton.IsEnabled = false;
            
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("/GeoSearchTestPage.aspx", UriKind.Relative));
            //App.Navigate(new SearchingResultPage(default_ClassifiedOrNot));
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("/GeoSearch;component/Pages/SearchResultPage.xaml", UriKind.Relative));
            RecordsSearchFunctions sf = new RecordsSearchFunctions();
            //searchingFunctions = new SearchingFunctions(this);
            //System.Windows.Application.LoadComponent(this, new System.Uri("/GeoSearch;component/SearchingResultPage.xaml", System.UriKind.Relative));
            sf.BasicSearch_Using_WCFService(SearchContentTextBox.Text, ConstantCollection.startPosition, ConstantCollection.maxRecords);
        }

        public void setSearchStutusBarInformation(int returnedRecordsNumber, int totalMatchedNumber, double responseTime, string searchStatus)
        {
            SearchSummaryTextBlock.Text = "Retrieved " + returnedRecordsNumber + " [ of " + totalMatchedNumber + " ] " + " results ( using " + responseTime + " seconds )";
            SearchStatusTextBlock.Text = searchStatus;
        }

        public void cleanUpEveryThing()
        {
            //myDispatcherTimer = null;
            TimeEnableWMSLayer_TimeList_all.Clear();
            TimeEnableWMSLayer_TimeList_toBeShown.Clear();
            //WMSList_shownOnBingMapsWMSLayerControl.Clear();
            recordAndPushPinMap_list.Clear();
            FoundRecords.Clear();
            foreach (CategoryPageMap cpm in categoryPage_records)
            {
                if (cpm.recordslist != null)
                    cpm.recordslist.Clear();
                cpm.tabItem.Visibility = Visibility.Collapsed;
            }

            m_PushpinLayer.Children.Clear();
            m_RecordsBBoxLayer.Children.Clear();
            m_PinInfoBoxLayer.Children.Clear();
            iPushPinInoBox = null;

            foreach (MapTileLayer layer1 in WMSLayerTimeName_MapLayer_Map.Values)
            {
                if (layer1 != null)
                    myBingMap.Children.Remove(layer1);
            }
            WMSLayerTimeName_MapLayer_Map.Clear();

            foreach (MapTileLayer layer1 in WMSLayer_MapLayer_Map.Values)
            {
                if (layer1 != null)
                    myBingMap.Children.Remove(layer1);
            }
            WMSLayer_MapLayer_Map.Clear();

            button_ShowWMSAnimationControl.IsEnabled = false;
            animationState = AnimationState.ReadyToStart;

            LayerBorder.Visibility = Visibility.Collapsed;
            AnimationControlBorder.Visibility = Visibility.Collapsed;
        }

        public static void setSearchButtonEnabled()
        {
            SearchingResultPage srp = App.getCurrentSearchingResultPage();
            if (srp == currentResultPage && srp != null)
                srp.SearchButton.IsEnabled = true;
        }

        //将当前显示页面切换成SearchingResultPage
        public static void showSearchingResultPage(string contentToSearching, int returnedRecordsNumber, int totalMatchedNumber,double responseTime, string searchStatus, ObservableCollection<Record> records, string sortingRule, bool isResultsCategorizedInTabItems)
        {
            App app = (App)Application.Current;
            SearchingResultPage srp = currentResultPage;
            //策略1，用old page
            //{
            //    //试图释放上一次查询结果页面对应的资源，但是失败了
            //    if (srp != null)
            //    {
            //        srp.DataContext = null;
            //        srp.PivotViewer_Container.PivotViewer.DataContext = null;
            //        srp.PivotViewer_Container.PivotViewer.ItemsSource = null;
            //        srp.FoundRecords = new ObservableCollection<Record>();
            //        //srp.PivotViewer_Container.PivotViewer.ItemsSource = srp.FoundRecords;
            //        //srp.PivotViewer_Container.Visibility = Visibility.Collapsed;
            //        //srp.Results_Container.Visibility = Visibility.Visible;
            //        srp.NavigateToNormalViewer();
            //        srp.cleanUpEveryThing();
            //        GC.Collect();
            //        GC.WaitForPendingFinalizers();
            //    }
            //    SearchingResultPage newResultPage = null;
            //    //用旧的页面也不是创建新的页面来节省内存占用
            //    newResultPage = srp;
            //    if (newResultPage == null)
            //        newResultPage = new SearchingResultPage(isResultsCategorizedInTabItems);

            //    newResultPage.setResultPageContents(contentToSearching, returnedRecordsNumber, totalMatchedNumber, responseTime, searchStatus, records, sortingRule);
            //    //navigate to searching result page
            //    App.Navigate(newResultPage);
            //}

            //策略2，用new page
            {
                SearchingResultPage newResultPage = new SearchingResultPage(isResultsCategorizedInTabItems);
                newResultPage.setResultPageContents(contentToSearching, returnedRecordsNumber, totalMatchedNumber, responseTime, searchStatus, records, sortingRule);
                //navigate to searching result page
                App.Navigate(newResultPage);
                //试图释放上一次查询结果页面对应的资源，但是失败了
                if (srp != null)
                {
                    srp.DataContext = null;
                    srp.PivotViewer_Container.PivotViewer.DataContext = null;
                    srp.PivotViewer_Container.PivotViewer.ItemsSource = null;
                    //srp.FoundRecords = new ObservableCollection<Record>();
                    srp.FoundRecords = new ObservableCollection<ClientSideRecord>();
                    //srp.PivotViewer_Container.PivotViewer.ItemsSource = srp.FoundRecords;
                    //srp.PivotViewer_Container.Visibility = Visibility.Collapsed;
                    //srp.Results_Container.Visibility = Visibility.Visible;
                    srp.NavigateToNormalViewer();
                    srp.cleanUpEveryThing();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            //HtmlPage.Window.Invoke("ShowNormalSizeBingMap");
        }

        //设置或者添加records到结果页面
        public void setResultPageContents(string contentToSearching, int returnedRecordsNumber, int totalMatchedNumber, double responseTime, string searchStatus, ObservableCollection<Record> records, string sortingRule)
        {
            SearchContentTextBox.Text = contentToSearching;
            setSearchStutusBarInformation(returnedRecordsNumber, totalMatchedNumber, responseTime, searchStatus);

            if (records == null)
                return;

            ObservableCollection<ClientSideRecord> newRecords = new ObservableCollection<ClientSideRecord>();
            foreach (Record record in records)
            {
                newRecords.Add(ClientSideRecord.cloneRecord(record));
            }

            foreach (ClientSideRecord record in newRecords)
            {
                insertRecordsIntoResultPage(record, sortingRule);
            }
            //直接在bingmaps上显示已经查到的条目，因为后面可能没有别的条目被查到了，所以不会触发刷新bingmaps上刷新pushpin的事件
            showSpecifiedRecordsInBingMap(newRecords.ToList());
                
            if (records.Count>0)
                noRecord = false;

            if (classifiedWithCategories)
            {
                if(!noRecord)
                    TabControl_CategoryPage.Visibility = Visibility.Visible;
                
                foreach (CategoryPageMap map in categoryPage_records)
                {
                    TabItem ti = map.tabItem;
                    //显示第一个visible的tabItem
                    if (ti.Visibility.Equals(Visibility.Visible))
                    {
                        TabControl_CategoryPage.SelectedItem = ti;
                        //将Map_Container_Container从旧的父容器中删除
                        Object parent = Map_Container_Container.Parent;
                        if (parent is Grid)
                        {
                            Grid grid = parent as Grid;
                            grid.Children.Remove(Map_Container_Container);
                        }
                        else if (parent is StackPanel)
                        {
                            StackPanel sp = parent as StackPanel;
                            sp.Children.Remove(Map_Container_Container);
                        }
                        //将Map_Container_Container添加到新的容器中
                        StackPanel sp1 = map.stackPanelContainer_ForRecordsListBoxAndBingMap;
                        sp1.Children.Add(Map_Container_Container);
                        setContentHeightWidthToFixBrowserWindowSize();
                        break;
                    }
                }
                setRecordCountTextBlockValue();
            }
            else
            {
                RecordsList.DataContext = FoundRecords;
                if (returnedRecordsNumber > 0 && records != null && records.Count() > 0)
                {
                    UncategoriedPagedCollectionView = new PagedCollectionView(FoundRecords);
                    UncategoriedPagedCollectionView.PageChanged += new EventHandler<EventArgs>(pageChangeHandler);
                    UncategoriedPagedCollectionView.CollectionChanged += new NotifyCollectionChangedEventHandler(collectionChangedHandler);
                    UncategoriedPagedCollectionView.CurrentChanged += new EventHandler(currentItemChangeHandler);  

                    List<SortDescription> sortDescriptionsList = getSortDescriptions(sortingRule);
                    if (sortDescriptionsList != null)
                    {
                        foreach (SortDescription sortDescription in sortDescriptionsList)
                        {
                            UncategoriedPagedCollectionView.SortDescriptions.Add(sortDescription);
                        }
                    }
                    //Set the DataPager and ListBox to the same data source.
                    dataPager1.Source = UncategoriedPagedCollectionView;
                    //searchingResultPage.dataPager0.Source = itemListView;
                    RecordsList.ItemsSource = UncategoriedPagedCollectionView;
                    RecordsList.SelectedIndex = 0;

                    StackPanel_SortBy.Visibility = Visibility.Visible;
                    dataPager1.Visibility = Visibility.Visible;
                    //searchingResultPage.dataPager0.Visibility = Visibility.Visible;
                }
                else
                {
                    StackPanel_SortBy.Visibility = Visibility.Collapsed;
                    dataPager1.Visibility = Visibility.Collapsed;
                    //searchingResultPage.dataPager0.Visibility = Visibility.Collapsed;
                }

                if (sortingRule != null && !sortingRule.Trim().Equals(""))
                {
                    foreach (ComboBoxItem cbi in ComboBox_SortBy.Items)
                    {

                        string value = cbi.Content as string;
                        if (value.Equals(sortingRule))
                        {
                            cbi.IsSelected = true;
                            ComboBox_SortBy.SelectedItem = cbi;
                        }
                    }
                }
            }
        }

        public static void showGeneralPopupPage(string content, Point offset)
        {
            generalPopup.TextBox_Content.Text = content;
            generalPopup.PopupPage.IsOpen = true;
            generalPopup.PopupPage.HorizontalOffset = offset.X;
            generalPopup.PopupPage.VerticalOffset = offset.Y;
        }

        //将WMS的指定图层以统一默认的不透明度叠置到BingMaps上
        public void addWMSLayerToBingMaps(string WMSURL, OtherQueryFunctionsServiceReference.CascadedWMSLayer layer)
        {
            addWMSLayerToBingMaps(WMSURL, layer, Opacity_WMSLayers);
        }

        //将WMS的指定图层以指定的不透明度叠置到BingMaps上
        public void addWMSLayerToBingMaps(string WMSURL, OtherQueryFunctionsServiceReference.CascadedWMSLayer layer, double opacity)
        {
            if (!WMSLayer_MapLayer_Map.ContainsKey(layer))
            {
                MapTileLayer tileLayer = new MapTileLayer();
                //LocationRectTileSource tileSource = new LocationRectTileSource(
                //                                               // "{UriScheme}://www.microsoft.com/maps/isdk/ajax/layers/lidar/{quadkey}.png",
                //                                               "http://wms.gmu.edu/SWPortalBing/servlet/BingmapWmsWapper?TILEID={quadkey}&url=http://rmgsc.cr.usgs.gov/arcgis/services/ecosys_US/MapServer/WMSServer?&VERSION=1.1.1&REQUEST=GetMap&LAYERS=0&SRS=EPSG:4326&BBOX=-180,-90,180,90",
                //                                                new LocationRect(new Location(90, -180), new Location(-90, 180)),
                //                                                new Range<double>(5, 19));
                //tileLayer.TileSources.Add(tileSource);

                LocationRect locationRect = Utilities.getLocationRectFromBBox(layer.latLonBBox);
                string WMSURL1 = processWMSURL(WMSURL);

                //BingMaps从第5级开始可以正常显示WMS
                WMSTileSource source = new WMSTileSource(WMSURL1, layer.name, new Range<double>(5, 19), locationRect);
                tileLayer.TileSources.Add(source);
                tileLayer.TileWidth = 256;
                tileLayer.TileHeight = 256;
                tileLayer.Opacity = opacity;
                myBingMap.Children.Add(tileLayer);
                //重新添加pushpin、bbox和pushpinInfobox图层，目的是让pushpin出于最上层
                myBingMap.Children.Remove(m_PushpinLayer);
                myBingMap.Children.Add(m_PushpinLayer);
                myBingMap.Children.Remove(m_PinInfoBoxLayer);
                myBingMap.Children.Add(m_PinInfoBoxLayer);
                myBingMap.Children.Remove(m_RecordsBBoxLayer);
                myBingMap.Children.Add(m_RecordsBBoxLayer);

                WMSLayer_MapLayer_Map.Add(layer, tileLayer);

                //添加WMS layer后显示的中心和zoomlevel
                //myBingMap.SetView(locationRect1);
                Location center = locationRect.Center;
                //WMS图层的中心在(0,0)附近就把它移动到美国本土显示（因为绝大多数这种情况，地图是覆盖全球的，而将在地图中心位置设置为（0,0）太平洋位置，不太好）
                if (center.Latitude + center.Longitude <= 10 && Math.Abs(center.Latitude) <= 10 && Math.Abs(center.Longitude) <= 10)
                    myBingMap.Center = new Location(40, -95);
                else
                    myBingMap.Center = center;

                //myBingMap.SetView(Utilities.getLargerRectangleCoverCurrentRectangle(box));

                int zoomLevel = Utilities.getSuitableMapLevel(locationRect);
                if (myBingMap.ZoomLevel != zoomLevel)
                    myBingMap.ZoomLevel = zoomLevel;
            }
        }

        //将WMS的指定图层上time-enable的时间序列图层叠置到BingMaps上，但是让这些图层先不可见，等待animation启动时，按需显示其中的某一个
        public void addTimeEnableWMSLayersToBingMapsByTimeNamesWithViewChange(string WMSURL, OtherQueryFunctionsServiceReference.CascadedWMSLayer layer, List<string> timeNames)
        {
                WMSURL = processWMSURL(WMSURL);
                LocationRect locationRect = Utilities.getLocationRectFromBBox(layer.latLonBBox);
                foreach (string timeName in timeNames)
                {
                    MapTileLayer tileLayer = new MapTileLayer();
                    tileLayer.Visibility = Visibility.Collapsed;
                    //BingMaps从第5级开始可以正常显示WMS
                    WMSTileSource source = new WMSTileSource(WMSURL, layer.name, timeName, new Range<double>(3, 19), locationRect);
                    tileLayer.TileSources.Add(source);
                    tileLayer.TileWidth = 256;
                    tileLayer.TileHeight = 256;
                    tileLayer.Opacity = Opacity_WMSAnimationLayers;
                    myBingMap.Children.Add(tileLayer);
                    WMSLayerTimeName_MapLayer_Map.Add(timeName, tileLayer);
                }
                //重新添加pushpin、bbox和pushpinInfobox图层，目的是让pushpin出于最上层
                myBingMap.Children.Remove(m_PushpinLayer);
                myBingMap.Children.Add(m_PushpinLayer);
                myBingMap.Children.Remove(m_PinInfoBoxLayer);
                myBingMap.Children.Add(m_PinInfoBoxLayer);
                myBingMap.Children.Remove(m_RecordsBBoxLayer);
                myBingMap.Children.Add(m_RecordsBBoxLayer);

                //添加WMS layer后显示的中心和zoomlevel
                //myBingMap.SetView(locationRect1);
                Location center = locationRect.Center;
                //WMS图层的中心在(0,0)附近就把它移动到美国本土显示（因为绝大多数这种情况，地图是覆盖全球的，而将在地图中心位置设置为（0,0）太平洋位置，不太好）
                if (center.Latitude + center.Longitude <= 10 && Math.Abs(center.Latitude) <= 10 && Math.Abs(center.Longitude) <= 10)
                    myBingMap.Center = new Location(40, -95);
                else
                    myBingMap.Center = center;

                //myBingMap.SetView(Utilities.getLargerRectangleCoverCurrentRectangle(box));

                int zoomLevel = Utilities.getSuitableMapLevel(locationRect);
                if (myBingMap.ZoomLevel != zoomLevel)
                    myBingMap.ZoomLevel = zoomLevel;
        }

        //将WMS的指定图层上time-enable的时间序列中某一个time对应图层叠置到BingMaps上，但是该图层先不可见，等待animation启动时，按需显示
        public void addTimeEnableWMSLayerToBingMapsByTimeNameWithoutViewChange(string WMSURL, string layerName, string timeName, LocationRect locationRect)
        {
            MapTileLayer tileLayer = new MapTileLayer();
            tileLayer.Visibility = Visibility.Collapsed;
            //BingMaps从第5级开始可以正常显示WMS
            WMSTileSource source = new WMSTileSource(WMSURL, layerName, timeName, new Range<double>(3, 19), locationRect);
            tileLayer.TileSources.Add(source);
            tileLayer.TileWidth = 256;
            tileLayer.TileHeight = 256;
            tileLayer.Opacity = Opacity_WMSAnimationLayers;
            myBingMap.Children.Add(tileLayer);
            WMSLayerTimeName_MapLayer_Map.Add(timeName, tileLayer);
        }

        //将WMS的指定图层上time-enable的时间序列图层叠置到BingMaps上，但是让这些图层先不可见，等待animation启动时，按需显示其中的某一个
        public void addTimeEnableWMSLayersToBingMapsByTimeNamesWithoutViewChange(string WMSURL, OtherQueryFunctionsServiceReference.CascadedWMSLayer layer, List<string> timeNames)
        {
            WMSURL = processWMSURL(WMSURL);
            LocationRect locationRect = Utilities.getLocationRectFromBBox(layer.latLonBBox);
            foreach (string timeName in timeNames)
            {
                if (!WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName))
                    addTimeEnableWMSLayerToBingMapsByTimeNameWithoutViewChange(WMSURL, layer.name, timeName, locationRect);
            }
            //重新添加pushpin、bbox和pushpinInfobox图层，目的是让pushpin出于最上层
            myBingMap.Children.Remove(m_PushpinLayer);
            myBingMap.Children.Add(m_PushpinLayer);
            myBingMap.Children.Remove(m_PinInfoBoxLayer);
            myBingMap.Children.Add(m_PinInfoBoxLayer);
            myBingMap.Children.Remove(m_RecordsBBoxLayer);
            myBingMap.Children.Add(m_RecordsBBoxLayer);
        }

        //处理WMS的URL链接使其可以被WMSTileSource接受
        string processWMSURL(string orignalURL)
        {
            string newURL = orignalURL;
            int index = orignalURL.IndexOf("?");
            if (index > 0)
            {
                if (!orignalURL.EndsWith("?") && !orignalURL.EndsWith("&"))
                    newURL = orignalURL + "&";
            }
            else
                newURL = orignalURL + "?";
            return newURL;
        }

        //将WMS的指定图层从BingMaps上删除
        //public void deleteWMSLayerFromBingMaps(string WMSURL, OtherQueryFunctionsServiceReference.WMSLayer layer)
        public void deleteWMSLayerFromBingMaps(string WMSURL, OtherQueryFunctionsServiceReference.CascadedWMSLayer layer)       
        {
            if (WMSLayer_MapLayer_Map.ContainsKey(layer))
            {
                MapTileLayer tileLayer = WMSLayer_MapLayer_Map[layer];
                myBingMap.Children.Remove(tileLayer);
                WMSLayer_MapLayer_Map.Remove(layer);
            }
        }

        public static void showMetadataDetailPopupPage(string result, Point offset)
        {
            metadataPopup.PopupPageTitle.Content = "Metadata Detail";
            metadataPopup.DetailTextBlock.Text = result;
            metadataPopup.MetadataDetailPage.IsOpen = true;

            metadataPopup.MetadataDetailPage.HorizontalOffset = offset.X;
            metadataPopup.MetadataDetailPage.VerticalOffset = offset.Y;
        }

        public static void showMetadataDetailFloatableWindow(string result)
        {        
            //HyperlinkButton hb = (HyperlinkButton)sender;
            //Uri metaDataURL = hb.NavigateUri;
            //HtmlWindow hw = HtmlPage.PopupWindow(metaDataURL, "_blank", null);

            //HyperlinkButton metadataButton = (HyperlinkButton)sender;
            //string id = (string)metadataButton.Tag;
            //sf.getRecordDetailMetadata_Using_WCFService(id);
            //showMetadataDetailPopupPage(sf.recordsDetail, new Point(pop.MetadataDetailPage.HorizontalOffset, pop.MetadataDetailPage.VerticalOffset));

            Metadata metadata = MetadtaDetailParse.parseMetadataFromXMLString(result);
            ////Use Floatable Windows developed by Sincorde
            //MetadataDetailWindow mdw = new MetadataDetailWindow();
            //mdw.TitleContent = "Metadata Detail";
            //mdw.textBlock_metadataDetail_XML.Text = result;
            //
            //mdw.myMetadataPanel.DataContext = metadata;
            //mdw.Show();
            SearchingResultPage srp = App.getCurrentSearchingResultPage();
            if (srp != null)
            {
                srp.MetaDateDetail_Container.DataContext = metadata;
                srp.textBlock_metadataDetail_XML.Text = result;
                if (srp.MetaDateDetail_Container.Visibility != Visibility.Visible)
                {
                    resizeResultContainerHeight(srp, -smallChangeHight);
                }
                srp.MetaDateDetail_Container.Visibility = Visibility.Visible;

                if(metadata.BBox != null)
                {
                    BoundingBox bbox = metadata.BBox;
                    BBox box = new BBox();
                    try
                    {
                        box.BBox_CRS = bbox.BBox_CRS;
                        box.BBox_Lower_Lat = double.Parse(bbox.BBox_South);
                        box.BBox_Upper_Lat = double.Parse(bbox.BBox_North);
                        box.BBox_Lower_Lon = double.Parse(bbox.BBox_West);
                        box.BBox_Upper_Lon = double.Parse(bbox.BBox_East);

                        srp.myMetadataPanel.BBox_BingMap.Center = Utilities.getCenterLocation(box);
                        //srp.myMetadataPanel.BBox_BingMap.Children.Clear();
                        srp.myMetadataPanel.BBox_MapLayer.Children.Clear();
                        MapPolygon polygon = Utilities.getBBoxPolygon(box);
                        polygon.Fill = new SolidColorBrush(Utilities.polygonFillColor_bbox);
                        polygon.Stroke = new SolidColorBrush(Utilities.polygonLineColor_bbox);
                        //polygon.StrokeThickness = bboxPolygonLineThickness;
                        polygon.Opacity = Utilities.bboxPolygenOpacity1;     
                        srp.myMetadataPanel.BBox_MapLayer.Children.Add(polygon);
                        //LocationRect rect = Utilities.getLocationRectFromBBox(box);
                        //srp.myMetadataPanel.BBox_BingMap.ZoomLevel = Utilities.getSuitableMapLevel(rect);
                        srp.myMetadataPanel.BBox_BingMap.SetView(Utilities.getLargerLocationRectFromBBox(box));
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
            }
        }

        //获取指定PagedCollctionView的当前页面中的所有条目的列表
        public List<ClientSideRecord> getRecordsInCurrentPageOfSpecifiedPagedCollctionView(PagedCollectionView view)
        {
            int currentPage = view.PageIndex;
            List<ClientSideRecord> list = new List<ClientSideRecord>();
            if (view.PageIndex >= 0)
            {
                for (int i = 0; i < Math.Min(view.PageSize, view.Count); i++)
                {
                    list.Add((ClientSideRecord)view.GetItemAt(i));
                }
            }
            return list;
        }

        //切换Records result pages时的事件,不要选择或高亮任何条目
        public void pageChangeHandler(object sender, EventArgs e)
        {
            if (sender is PagedCollectionView)
            {             
                PagedCollectionView view = (PagedCollectionView)sender;
                if (classifiedWithCategories)
                {
                    foreach (CategoryPageMap map in categoryPage_records)
                    {
                        if (view == map.pagedCollectionView)
                        {
                            map.listBox.SelectedIndex = 0;
                            map.listBox.SelectedItem = null;
                            break;
                        }
                    }
                }
                else
                {
                    RecordsList.SelectedIndex = 0;
                    RecordsList.SelectedItem = null;
                }
                //maybeChangeListBoxWidth();
            }
        }

        //Page中内容变化后触发的事件
        public void collectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender is PagedCollectionView)
            {
                PagedCollectionView view0 = (PagedCollectionView)sender;
                //if (classifiedWithCategories)
                //{
                //    TabItem ti = (TabItem)TabControl_CategoryPage.SelectedItem;
                //    foreach (CategoryPageMap map in categoryPage_records)
                //    {
                //        if (ti == map.tabItem)
                //        {
                //            PagedCollectionView view = map.pagedCollectionView;
                //            if (view != null)
                //            {
                //                List<Record> list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(view);
                //                showSpecifiedRecordsInBingMap(list);
                //            }
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    List<Record> list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(view0);
                //    showSpecifiedRecordsInBingMap(list);
                //}
                List<ClientSideRecord> list = getRecordsListInCurrentPage(view0);
                if(list!=null && list.Count>0)
                    showSpecifiedRecordsInBingMap(list);
                maybeChangeListBoxWidth(view0);
            }
        }

        //得到当前显示页面上的records list(如果PagedCollectionView view0为空，则先去获取当前PagedCollectionView)
        public List<ClientSideRecord> getRecordsListInCurrentPage(PagedCollectionView view0)
        {
            List<ClientSideRecord> list = null;
            if (view0 == null)
            {
                if (classifiedWithCategories)
                {
                    TabItem ti = (TabItem)TabControl_CategoryPage.SelectedItem;
                    foreach (CategoryPageMap map in categoryPage_records)
                    {
                        if (ti == map.tabItem)
                        {
                            PagedCollectionView view = map.pagedCollectionView;
                            if (view != null)
                            {
                                list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(view);
                                showSpecifiedRecordsInBingMap(list);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(UncategoriedPagedCollectionView);
                    showSpecifiedRecordsInBingMap(list);
                }
            }
            else
            {
                //如果内容变化的PagedCollectionView不是以当前正在显示的页面中的，则返回空
                if (classifiedWithCategories)
                {
                    TabItem ti = (TabItem)TabControl_CategoryPage.SelectedItem;
                    foreach (CategoryPageMap map in categoryPage_records)
                    {
                        if (ti == map.tabItem)
                        {
                            PagedCollectionView view = map.pagedCollectionView;
                            if (view != null && view0 != view)
                            {
                                return null; 
                            }
                        }
                    }
                }

                list = getRecordsInCurrentPageOfSpecifiedPagedCollctionView(view0);
            }
            return list;
        }

        //当前pages中选中条目变化时的事件
        public void currentItemChangeHandler(object sender, EventArgs e)
        {
            if (sender is PagedCollectionView)
            {
                PagedCollectionView view = (PagedCollectionView)sender;
                //int index = view.CurrentPosition;
                //if (index >= 0)
                //{
                //    //object[] parameters = new object[1];
                //    //parameters[0] = index;
                //    //HtmlPage.Window.Invoke("highlightSpecifiedRecord", parameters);
                //    highlightSpecifiedRecord(index);
                //}

                highlightSpecifiedRecord(view.CurrentItem as ClientSideRecord, MyPushPinInfoBox.CreationEventType.SelectFromRecordsList);
                if (MetaDateDetail_Container.Visibility == Visibility.Visible)
                {
                    if (view.CurrentItem != null)
                    {
                        string MetadataAccessURL = (string)(view.CurrentItem as ClientSideRecord).MetadataAccessURL;
                        if (MetadataAccessURL != null && !MetadataAccessURL.Trim().Equals(""))
                            otherQueryFunction.getRecordDetailMetadata_Using_WCFService(MetadataAccessURL, ID);
                    }
                }
            }
        }

        //将指定的Records显示在bing map上
        public void showSpecifiedRecordsInBingMap(List<ClientSideRecord> records)
        {
            //HtmlPage.Window.Invoke("initialMapEntitiesAndArrays");
            initialMapEntitiesAndLists();
            foreach (ClientSideRecord record in records)
            {
                //要显示pushpin则要么boundingbox存在，要么URL Location存在，并且没有超过自定义的显示数目限制
                if ((record.bbox != null || (record.URLLocation != null && !record.URLLocation.Trim().Equals(""))) && recordAndPushPinMap_list.Count < ConstantCollection.ExpectedMaximumNum_PushpinOnBingMaps)  
                {
                    //object[] parameters = new object[6];
                    //parameters[0] = record.bbox.BBox_Lower_Lon;
                    //parameters[1] = record.bbox.BBox_Lower_Lat;
                    //parameters[2] = record.bbox.BBox_Upper_Lon;
                    //parameters[3] = record.bbox.BBox_Upper_Lat;
                    //parameters[4] = record.Title;
                    //parameters[5] = record.Provider;

                    //HtmlPage.Window.Invoke("AddPushPinAndBBox", parameters);
                    addPushpinAndBBox(record);
                }
            }

        }

        //设置指定TabItem的dataPager和排序方法combx控件
        private void setDataPagerAndSortingCombx(TabItem ti, ObservableCollection<ClientSideRecord> list, string sortingRule)
        {
            if (ti.Visibility.Equals(Visibility.Collapsed))
            {
                ti.Visibility = Visibility.Visible;
                PagedCollectionView itemListView = new PagedCollectionView(list);
                itemListView.PageChanged += new EventHandler<EventArgs>(pageChangeHandler);
                itemListView.CollectionChanged += new NotifyCollectionChangedEventHandler(collectionChangedHandler);
                itemListView.CurrentChanged += new EventHandler(currentItemChangeHandler);   

                List<SortDescription> sortDescriptionsList = getSortDescriptions(sortingRule);
                if (sortDescriptionsList != null)
                {
                    foreach (SortDescription sortDescription in sortDescriptionsList)
                    {
                        itemListView.SortDescriptions.Add(sortDescription);
                    }
                }

                //Set the DataPager and ListBox to the same data source.
                foreach (CategoryPageMap map in categoryPage_records)
                {
                    if (map.tabItem == ti)
                    {
                        map.pagedCollectionView = itemListView;
                        map.dataPager.Source = itemListView;
                        map.dataPager.Visibility = Visibility.Visible;

                        if (sortingRule != null && !sortingRule.Trim().Equals(""))
                        {
                            ComboBox cb = map.comboBox_SortingRule;
                            foreach (ComboBoxItem cbi in cb.Items)
                            {
                                string value = cbi.Content as string;
                                if (value.Equals(sortingRule))
                                {
                                    cbi.IsSelected = true;
                                    cb.SelectedItem = cbi;
                                    break;
                                }
                            }
                        }            
                        map.listBox.ItemsSource = itemListView;
                        map.listBox.SelectedIndex = 0;
                        break;
                    }
                }       
            }
        }

        public void insertRecordsIntoResultPage(ClientSideRecord record, string sortingRule)
        {
            if (!classifiedWithCategories)
            {
                FoundRecords.Add(record);
            }
            else
            {
                //ClientSideRecord newRecord = ClientSideRecord.cloneRecord(record);
                FoundRecords.Add(record);
                string type = record.Type;
                if (type.Equals(ConstantCollection.resourceTypeValue_CLH_Services)
                        || type.Equals(ConstantCollection.ServiceType_CSW)
                        || type.Equals(ConstantCollection.ServiceType_WMS)
                        || type.Equals(ConstantCollection.ServiceType_WFS)
                        || type.Equals(ConstantCollection.ServiceType_WPS)
                        || type.Equals(ConstantCollection.ServiceType_WCS)
                        || type.Equals(ConstantCollection.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection)
                        || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_AlertsRSSAndInformationFeeds)
                        || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_AnalysisAndVisualization)
                        || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_CatalogRegistryOrMetadataCollection))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_WebServices.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_WebServices, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_WebServices;
                }

                //datasets
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_Datasets) || type.Equals(ConstantCollection.resourceTypeValue_CLH_Datasets_nonGeographic)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_Datasets) || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_OfflineData)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_LiveData)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_DownloadableData)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_StaticMapImage)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_MapFiles))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_Datasets.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_Datasets, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_PivotViewer_Datasets;
                }

                //Initiatives
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_Initiatives) || type.Equals(ConstantCollection.resourceTypeValue_CSR_Initiatives)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Initiatives))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_Initiatives.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_Initiatives, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_PivotViewer_Initiatives;
                }

                //MonitoringAndObservationSystems
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_MonitoringAndObservationSystems) || type.Equals(ConstantCollection.resourceTypeValue_GOS_MonitoringAndObservationSystems)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_MonitoringAndObservationSystems))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_MonitoringAndObservationSystems.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_MonitoringAndObservationSystems, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_PivotViewer_MonitoringAndObservationSystems;
                }

                //ComputationalModel
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_ComputationalModel) || type.Equals(ConstantCollection.resourceTypeValue_GOS_ComputationalModel)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_ComputationalModel))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_ComputationalModel.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_ComputationalModel, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_PivotViewer_ComputationalModel;
                }

                //WebsitesAndDocuments
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_WebsitesAndDocuments) || type.Equals(ConstantCollection.resourceTypeValue_GOS_WebsitesAndDocuments)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_WebsitesAndDocuments))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_WebsitesAndDocuments.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_WebsitesAndDocuments, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_WebsitesAndDocuments;
                }

                //SoftwareAndApplications
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_SoftwareAndApplications) || type.Equals(ConstantCollection.resourceTypeValue_GOS_SoftwareAndApplications)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_SoftwareAndApplications))
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_SoftwareAndApplications.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_SoftwareAndApplications, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceTypeValue_GOS_SoftwareAndApplications;
                }

                else
                {
                    ObservableCollection<ClientSideRecord> list = TabItem_Datasets.DataContext as ObservableCollection<ClientSideRecord>;
                    list.Add(record);
                    setDataPagerAndSortingCombx(TabItem_Datasets, list, sortingRule);
                    record.GeneralType = ConstantCollection.resourceGeneralType_PivotViewer_Datasets;
                }
            }
        }

        private void HyperlinkButton_RecordItemTitle_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton metadataTitle = (HyperlinkButton)sender;
            string MetadataAccessURL = (string)metadataTitle.Tag;
            otherQueryFunction.getRecordDetailMetadata_Using_WCFService(MetadataAccessURL, ID);  
        }

        private void MetadataDetailButton_Click(object sender, RoutedEventArgs e)
        {
            Button metadataButton = (Button)sender;
            string MetadataAccessURL = (string)metadataButton.Tag;
            otherQueryFunction.getRecordDetailMetadata_Using_WCFService(MetadataAccessURL, ID);
        }

        private void OpenLayersButton_Click(object sender, RoutedEventArgs e)
        {      
            //the code below are three kinds of mothod to show html content of specified url in browser. 
            //string url = "http://ec2-204-236-246-78.compute-1.amazonaws.com/viewer/?server=http://mrdata.usgs.gov/cgi-bin/state/ct?&layer=Connecticut_Lithology";

            //1. invoke javascript function define by myself in silverlight hosthtml file(such as, html and aspx)
            //HtmlPage.Window.Invoke("openNewPage", parameters);

            //2. invoke c# function to show html content in a new tab.
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "_blank"); 
            //System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "SWPViewer");

            //3. invoke c# function to show html content in a new popup windows of browser
            //System.Windows.Browser.HtmlPage.PopupWindow(new Uri(url, UriKind.Absolute), "SWPViewer",
            //     new System.Windows.Browser.HtmlPopupWindowOptions()
            //     {
            //         Width = 500,
            //         Height = 500
            //     }); 

            //4. use div container to float html content in the top of current page.
            
            Button swpbutton = sender as Button;
            string URL = swpbutton.Tag as string;
            ClientSideRecord record = swpbutton.DataContext as ClientSideRecord;
            if (URL == null && URL.Trim().Equals(""))
                URL = record.AccessURL;

            WMSLayyersSelectionPage cw = new WMSLayyersSelectionPage(URL);
            cw.Show();
        }

        private void WWTButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WWButton_Click(object sender, RoutedEventArgs e)
        {
            Button WWButton = sender as Button;
            string url = WWButton.Tag as string;
            ClientSideRecord record = WWButton.DataContext as ClientSideRecord;
            if (url == null && url.Trim().Equals(""))
                url = record.AccessURL;
            url = ConstantCollection.WorldWind_WebVersion_URL + url;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "WWViewer");
        }

        private void SWPButton_Click(object sender, RoutedEventArgs e)
        {
            Button SWPButton = sender as Button;
            string url = SWPButton.Tag as string;
            ClientSideRecord record = SWPButton.DataContext as ClientSideRecord;
            string serviceType = record.Type;
            if (url == null && url.Trim().Equals(""))
                url = record.AccessURL;
            if (serviceType.Equals(ConstantCollection.ServiceType_WFS) || serviceType.Equals(ConstantCollection.ServiceType_WMS) || serviceType.Equals(ConstantCollection.ServiceType_WCS))
            {
                serviceType = serviceType.Replace("OGC:", "");
                //url = record.RealServiceURL;
            }
            else if (serviceType.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization))
            {
                serviceType = "WMS";
                bool isWFS = url.ToLower().Contains("wfs");
                if (isWFS)
                    serviceType = "WFS";
                int index = url.IndexOf("?");
                if (index > 0)
                    url = url.Substring(0, index + 1);
            }
            url = ConstantCollection.SWPView_URL + "serviceURL=" + url + "&serviceType=" + serviceType;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "SWPViewer");
        }

        private void AdvancedSearch_Click(object sender, RoutedEventArgs e)
        {
            //hideBingMap();
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

        private void queryAndShowServiceQoSDetailInformation(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ClientSideRecord record = element.DataContext as ClientSideRecord;
            DateTime current = DateTime.Now;
            DateTime fromDate = current.AddDays(-ServiceQualityInfoPanel.serviceQoSHistoralInterval+1);
            ServiceQualityPopup serviceQualityPopup = new ServiceQualityPopup();
            QualityQueryFunctions pf = new QualityQueryFunctions(ID);
            serviceQualityPopup.serviceQualityInfoPanel.setBasicContent(record, pf, fromDate, current);
            pf.view = serviceQualityPopup;
            pf.getGeospatialServiceQoSHistoricalInfo(record, fromDate, current);
        }

        public static void showQoSDetailInformationInControls(GeoSearch.QualityQueryServiceReference.ServiceQoSInfoForHistory results, ServiceQualityPopup serviceQualityPopup)
        {
            //if (results != null)
            {
                if (serviceQualityPopup == null)
                    serviceQualityPopup = new ServiceQualityPopup();
                //FrameworkElement element = sender as FrameworkElement;
                //GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual as UIElement);
                //Point offset = gt.Transform(new Point(0, 0));
                ////Canvas.SetLeft(generalPopup.PopupPage, offset.X);
                ////Canvas.SetTop(generalPopup.PopupPage, offset.Y);
                //serviceQualityPopup.ServiceQualityPage.HorizontalOffset = offset.X;
                //serviceQualityPopup.ServiceQualityPage.VerticalOffset = offset.Y;
                serviceQualityPopup.serviceQualityInfoPanel.showServicePerformanceInfo(results);
                serviceQualityPopup.ServiceQualityPage.HorizontalOffset = 10;
                serviceQualityPopup.ServiceQualityPage.VerticalOffset = 120;
                serviceQualityPopup.ServiceQualityPage.IsOpen = true;
            }
        }

        public void showSBAVocabularyQuickSearchPage()
        {
            if (SBAPopup == null)
            {
                SBAPopup = new SBAQuickSearchPopup();
                GeneralTransform gt = this.SearchButton.TransformToVisual(Application.Current.RootVisual);
                Point offset = gt.Transform(new Point(0, 0));
                SBAPopup.SBAQuickSearchPage.HorizontalOffset = offset.X + 10;
                SBAPopup.SBAQuickSearchPage.VerticalOffset = offset.Y + 100;
                SBAPopup.SBAQuickSearchPage.VerticalAlignment = VerticalAlignment.Center;
                SBAPopup.SBAQuickSearchPage.IsOpen = true;
            }
            else
            {
                if(SBAPopup.showSBA)
                    SBAPopup.SBAQuickSearchPage.IsOpen = true;
            }
        }

        private void sortingRecords(ComboBox cb)
        {
            //both code section below works well for sorting, but the second one is better, it will using the sorting rules refresh the order when new records added, so the result in pager is dynamic sorted. 
            
            //Mothod 1
            //List<Record> recordsList = searchedRecords.ToList();
            //recordsList = recordsList.OrderByDescending(s => s.Relevance).ThenBy(s => s.Title).ToList();
            //searchedRecords = new ObservableCollection<Record>(recordsList);
            //PagedCollectionView itemListView = new PagedCollectionView(searchedRecords);
            //// Set the DataPager and ListBox to the same data source.
            //dataPager1.Source = itemListView;
            //dataPager0.Source = itemListView;
            //RecordsList.ItemsSource = itemListView;
            //RecordsList.SelectedIndex = -1;
            if (!classifiedWithCategories)
            {
                if (dataPager1 != null && dataPager1.Source != null)
                {
                    ComboBoxItem cbi = ComboBox_SortBy.SelectedItem as ComboBoxItem;
                    string sortBy = cbi.Content as string;
                    //Method 2
                    PagedCollectionView itemListView = dataPager1.Source as PagedCollectionView;
                    if (itemListView.CanSort == true)
                    {
                        itemListView.SortDescriptions.Clear();
                        List<SortDescription> sortDescriptionsList = getSortDescriptions(sortBy);
                        foreach (SortDescription sortDescription in sortDescriptionsList)
                        {
                            itemListView.SortDescriptions.Add(sortDescription);
                        }
                    }
                }
            }
            else
            {
                StackPanel sp = cb.Parent as StackPanel;
                if (sp != null)
                {
                    Grid sp0 = sp.Parent as Grid;
                    foreach (UIElement uiElement in sp0.Children)
                    {
                        if (uiElement is DataPager)
                        {
                            DataPager dp = uiElement as DataPager;
                            if (dp != null && dp.Source != null)
                            {
                                ComboBoxItem cbi = cb.SelectedItem as ComboBoxItem;
                                string sortBy = cbi.Content as string;
                                //Method 2
                                PagedCollectionView itemListView = dp.Source as PagedCollectionView;
                                if (itemListView.CanSort == true)
                                {
                                    itemListView.SortDescriptions.Clear();
                                    List<SortDescription> sortDescriptionsList = getSortDescriptions(sortBy);
                                    foreach (SortDescription sortDescription in sortDescriptionsList)
                                    {
                                        itemListView.SortDescriptions.Add(sortDescription);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static List<SortDescription> getSortDescriptions(string sortingRule)
        {
            List<SortDescription> sortDescriptionsList = null;
            if (sortingRule != null && !sortingRule.Trim().Equals(""))
            {
                sortDescriptionsList = new List<SortDescription>();
                if (sortingRule.Equals(ConstantCollection.SortBy_Default))
                {
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_Relevance))
                {
                    sortDescriptionsList.Add(new SortDescription("Relevance", ListSortDirection.Descending));
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_ResourceQuality))
                {
                    sortDescriptionsList.Add(new SortDescription("Quality", ListSortDirection.Descending));
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_Title))
                {
                    sortDescriptionsList.Add(new SortDescription("Title", ListSortDirection.Ascending));
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_Relevance_Title))
                {
                    sortDescriptionsList.Add(new SortDescription("Relevance", ListSortDirection.Descending));
                    sortDescriptionsList.Add(new SortDescription("Title", ListSortDirection.Ascending));
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_Relevance_ResourceQuality))
                {
                    sortDescriptionsList.Add(new SortDescription("Relevance", ListSortDirection.Descending));
                    sortDescriptionsList.Add(new SortDescription("Quality", ListSortDirection.Descending));
                }
                else if (sortingRule.Equals(ConstantCollection.SortBy_Source_Title))
                {
                    sortDescriptionsList.Add(new SortDescription("Source", ListSortDirection.Ascending));
                    sortDescriptionsList.Add(new SortDescription("Title", ListSortDirection.Ascending));
                }
                else if(sortingRule.Equals(ConstantCollection.SortBy_DataCORE_Relevance))
                {
                    sortDescriptionsList.Add(new SortDescription("isDataCore", ListSortDirection.Descending));
                    sortDescriptionsList.Add(new SortDescription("Relevance", ListSortDirection.Descending));
                }
            }
            return sortDescriptionsList;
        }

        private void ComboBox_SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox cb = sender as ComboBox;
                sortingRecords(cb);
            }
        }

        private void UIElement_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            string content = element.Tag as string;
            if (element is HyperlinkButton && element.Name.Equals("HyperLinkButtonResourceTitle"))
            {
                content = "Title: " + (element as HyperlinkButton).Content as string;
            }
            else if (element is Image && element.Name.Equals("Image_ResourceType"))
            {
                content = "Resource Type: " + element.Tag as string;
            }
            else if (element is Image && element.Name.Equals("Image_ServiceQuality"))
            {
                string qualityString = "excellent";
                double quality = (double)element.Tag;
                if (quality == 5)
                    qualityString = "excellent";
                else if (quality == 4)
                    qualityString = "good";
                else if (quality == 3)
                    qualityString = "fine";
                else if (quality == 2)
                    qualityString = "bad";
                else if (quality == 1)
                    qualityString = "extremly bad";
                else if (quality == 0)
                    qualityString = "null";
                content = "Performance: " + qualityString + "\n" + "click this icon for more information";
            }
            else if (element is Button && element.Name.Equals("WWButton"))
            {
                content = "Visualizing OGC Service in World Wind Web Version requires extra runtime enviroment and libs,\n" + "clike 'help' hyperlink button for more informtion";
            }
            else
                return;

            GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual as UIElement);
            Point offset = gt.Transform(new Point(0, 0));
            double controlTop = offset.Y;
            double controlLeft = offset.X;
            //// Obtain transform information based off root element 
            //GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual);
            //// Find the four corners of the element 
            //Point topLeft = gt.Transform(new Point(0, 0));
            //Point topRight = gt.Transform(new Point(element.RenderSize.Width, 0));
            //Point bottomLeft = gt.Transform(new Point(0, element.RenderSize.Height));
            //Point bottomRight = gt.Transform(new Point(element.RenderSize.Width, element.RenderSize.Height));
            showGeneralPopupPage(content, new Point(controlLeft + 30, controlTop + 20));
        }

        private void UIElement_MouseLeave(object sender, MouseEventArgs e)
        {
            generalPopup.PopupPage.IsOpen = false;
        }

        private void HyperlinkButton_Click_Help(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            App.Navigate(App.helpPage);
        }

        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            Button GetDataButton = sender as Button;
            string url = GetDataButton.Tag as string;
            if(url!=null && (!url.Trim().Equals("")))
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "GetData");
        }

        private void SearchSummaryViewer_Click(object sender, RoutedEventArgs e)
        {
            otherQueryFunction.getVisitClientIPAddress(ID);
        }

        //单击显示或隐藏元数据条目列表的按钮的对应事件
        private void button_HideOrShowRecordsList_Click(object sender, RoutedEventArgs e)
        {
            Object o = button_HideOrShowRecordsList.Content;
            if (o is TextBlock)
            {
                TextBlock tb = o as TextBlock;
                isRecordsListShow = !isRecordsListShow;
                button_HideOrShowBingMap.IsEnabled = isRecordsListShow;
                if (!isRecordsListShow)
                {
                    tb.Text = "》";
                    changeUIElementToolTipStringContent(button_HideOrShowRecordsList, "Show RecordsList");
                    if (!classifiedWithCategories)
                        RecordsList.Visibility = Visibility.Collapsed;
                    else
                    {
                        foreach (CategoryPageMap map in categoryPage_records)
                        {
                            if (map.listBox!= null)
                                map.listBox.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    tb.Text = "《";
                    changeUIElementToolTipStringContent(button_HideOrShowRecordsList, "Hide RecordsList");
                    if (!classifiedWithCategories)
                        RecordsList.Visibility = Visibility.Visible;
                    else
                    {
                        foreach (CategoryPageMap map in categoryPage_records)
                        {
                            if (map.listBox != null)
                                map.listBox.Visibility = Visibility.Visible;
                        }
                    }
                }
                setContentHeightWidthToFixBrowserWindowSize();
            }
        }

        //单击显示或隐藏BingMap的按钮的对应事件
        private void button_HideOrShowBingMap_Click(object sender, RoutedEventArgs e)
        {
            Object o = button_HideOrShowBingMap.Content;
            if (o is TextBlock)
            {
                TextBlock tb = o as TextBlock;
                isBingMapShow = !isBingMapShow;
                button_HideOrShowRecordsList.IsEnabled = isBingMapShow;
                if (!isBingMapShow)
                {
                    tb.Text = "》";
                    changeUIElementToolTipStringContent(button_HideOrShowBingMap, "Show Bing Maps");
                    Map_Container.Visibility = Visibility.Collapsed;
                }
                else
                {
                    tb.Text = "《";
                    changeUIElementToolTipStringContent(button_HideOrShowBingMap, "Hide Bing Maps");
                    Map_Container.Visibility = Visibility.Visible;
                }
                setContentHeightWidthToFixBrowserWindowSize();
            }
        }

        //单击地图上全屏按钮的鼠标事件
        private void button_BingMapsFullScreen_Click(object sender, RoutedEventArgs e)
        {
            if (!isBingMapFullScreen)
            {          
                UserControl root = App.getCurrentPage();
                if (root.Content is SearchingResultPage)
                {
                    isBingMapFullScreen = !isBingMapFullScreen;
                    SearchingResultPage srp = root.Content as SearchingResultPage;
                    button_BingMapsFullScreen.Tag = srp;
                    Map_Container_Container.Children.Remove(Map_Container);
                    BingMap_Height = Map_Container.Height;
                    BingMap_Width = Map_Container.Width;
                    Map_Container.Height = (double)HtmlPage.Window.Eval("document.documentElement.clientHeight");
                    Map_Container.Width = (double)HtmlPage.Window.Eval("document.documentElement.clientWidth");
                    App.Navigate(Map_Container);

                    button_BingMapsFullScreen.Visibility = Visibility.Collapsed;
                    button_BingMapsExitFullScreent.Visibility = Visibility.Visible;   
                }
            }
        }

        //单击地图上退出全屏按钮的鼠标事件
        private void button_BingMapsExitFullScreen_Click(object sender, RoutedEventArgs e)
        {
            BingMapsExitFullScreen();
        }

        //BingMaps退出全屏的事件内容
        public void BingMapsExitFullScreen()
        {
            if (isBingMapFullScreen)
            {
                isBingMapFullScreen = !isBingMapFullScreen;
                App.Navigate(button_BingMapsFullScreen.Tag as UIElement);
                Map_Container.Height = BingMap_Height;
                Map_Container.Width = BingMap_Width;
                Map_Container_Container.Children.Add(Map_Container);

                button_BingMapsFullScreen.Visibility = Visibility.Visible;
                button_BingMapsExitFullScreent.Visibility = Visibility.Collapsed;
            }
        }

        //修改控件提示信息的文本内容
        private void changeUIElementToolTipStringContent(UIElement ui_element, string content)
        {
            ToolTip tt = ToolTipService.GetToolTip(ui_element) as ToolTip;
            ContentControl cc = null;
            foreach (UIElement element in (tt.Content as StackPanel).Children)
            {
                if (element is ContentControl)
                {
                    cc = element as ContentControl;
                    cc.Content = content;
                }
            } 
        }

        //单击bingmaps按钮的事件，在bingmaps上显示当前WMS的图层树结构
        private void BingMapsButton_Click(object sender, RoutedEventArgs e)
        {
            string WMSURL = null;
            if (sender is Button)
            {
                Button bt = sender as Button;
                WMSURL = bt.Tag as string;
            }
            addWMSToWMSLayerControlOnBingMaps(WMSURL);
        }

        //在bingmaps上显示当前WMS的图层树结构
        public void addWMSToWMSLayerControlOnBingMaps(string WMSURL)
        {
            if (LayerBorder.Visibility == Visibility.Collapsed)
            {
                LayerBorder.Visibility = Visibility.Visible;
                button_ShowWMSLayersControl.IsEnabled = false;
            }
            //如果服务的地址不为空
            if (WMSURL != null)
            {
                //如果当前WMS未在Bing Maps WMS Layer Control上显示，则添加
                if (!WMSList_shownOnBingMapsWMSLayerControl.Contains(WMSURL))
                {
                    if (!cachedWMSLayer_Map.ContainsKey(WMSURL))
                    {
                        otherQueryFunction.otherQueryFunctionsServiceClient_getHierachicalLayersOfWMSAsync(WMSURL, ID);
                    }
                    else
                    {
                        OtherQueryFunctionsServiceReference.HierachicalWMSLayers wms = cachedWMSLayer_Map[WMSURL];
                        addHierachicalLayersToWMSLayerControl(wms);
                    }
                    WMSList_shownOnBingMapsWMSLayerControl.Add(WMSURL);
                }
            }
            else
            {
                setErrorInfoWhenLoadingFailedOnWMSLayerControl(WMSURL);
            }
        }

        //迭代删除树节点及其所对应的图层
        void deleteTreeItemAndWMSLayers(TreeViewItem item)
        {
            GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer = item.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
            if (WMSLayer_MapLayer_Map.ContainsKey(layer))
            {
                MapTileLayer mapLayer = WMSLayer_MapLayer_Map[layer];
                WMSLayer_MapLayer_Map.Remove(layer);
                myBingMap.Children.Remove(mapLayer);
            }
            if (item.Items.Count > 0)
            {
                foreach (TreeViewItem item1 in item.Items)
                {
                    deleteTreeItemAndWMSLayers(item1);
                }
            }
        }

        //请求图层信息完成以后，显示在bingmaps,采用级联cascade图层形式
        void otherQueryFunctionsServiceClient_getHierachicalLayersOfWMSCompleted(object sender, OtherQueryFunctionsServiceReference.getHierachicalLayersOfWMSCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // how to process error
            }
            else
            {
                if (e.Result != null)
                    cachedWMSLayer_Map.Add(e.Result.WMSURL, e.Result);
                addHierachicalLayersToWMSLayerControl(e.Result);
            }
        }

        public static void addHierachicalLayersToWMSLayer(OtherQueryFunctionsServiceReference.HierachicalWMSLayers layers)
        {
            if (layers != null)
                cachedWMSLayer_Map.Add(layers.WMSURL, layers);
            {
                App app = (App)Application.Current;
                UIElement uiElement = app.RootVisual;
                if (uiElement is UserControl)
                {
                    UserControl control = uiElement as UserControl;
                    UIElement root = control.Content;
                    SearchingResultPage srp = null;
                    if (root is SearchingResultPage)
                        srp = root as SearchingResultPage;
                    else
                    {
                        srp = SearchingResultPage.currentResultPage;
                    }
                    if (srp != null)
                        srp.addHierachicalLayersToWMSLayerControl(layers);
                }
            }
        }

        //将WMS图层信息显示在bingmaps上的WMS Layer Control中,采用级联cascade图层形式
        void addHierachicalLayersToWMSLayerControl(GeoSearch.OtherQueryFunctionsServiceReference.HierachicalWMSLayers wms)
        {
            ObservableCollection<GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer> layerObject = null;
            if (wms != null)
                layerObject = wms.layersList;
            else
                layerObject = null;
            if (layerObject != null)
            {
                //如果已经添加过，则不再添加
                foreach (TreeViewItem item in Layers_Treeview.Items)
                {
                    if (item.Tag == wms)
                        return;
                }
            
                TreeViewItem rootTreeNode = new TreeViewItem();
                rootTreeNode.Tag = wms;
                rootTreeNode.Padding = new Thickness(0);
                CheckBoxWithMultiImagesText_BingMap area1 = new CheckBoxWithMultiImagesText_BingMap();
                string wmsTitle = wms.serviceTitle;
                if (wmsTitle == null || wmsTitle.Trim().Equals(""))
                    wmsTitle = wms.serviceName;
                if (wmsTitle == null || wmsTitle.Trim().Equals(""))
                    wmsTitle = "All Layers";

                wmsTitle += " [" + wms.allGetMapEnabledLayers_Number + "]";
                area1.Title.Text = wmsTitle;
                changeUIElementToolTipStringContent(area1.Title, wmsTitle);
                area1.Title.FontSize = 12;
                area1.Title.FontWeight = FontWeights.Bold;
                area1.Image_NodeType.Source = new BitmapImage(new Uri("/GeoSearch;component/images/resourceTypes/WMS.png", UriKind.Relative));
                area1.Image_removeTreeViewItem.Visibility = Visibility.Visible;
                //area1.Image_removeTreeViewItem.Tag = e.Result.WMSURL;
                area1.Image_removeTreeViewItem.Tag = rootTreeNode;

                //鼠标左键点击根节点上删除WMS Layers的图片时的事件，将指定的WMS从树结构中删除，同时从图上删除
                area1.Image_removeTreeViewItem.MouseLeftButtonUp += new MouseButtonEventHandler((sender1, e1) =>
                {
                    Image img = sender1 as Image;
                    //string wmsURL = img.Tag as String;
                    TreeViewItem tvi = img.Tag as TreeViewItem;
                    Layers_Treeview.Items.Remove(tvi);
                    //string WMSURL = (tvi.Tag as GeoSearch.OtherQueryFunctionsServiceReference.WMSLayers).WMSURL;
                    string WMSURL = (tvi.Tag as GeoSearch.OtherQueryFunctionsServiceReference.HierachicalWMSLayers).WMSURL;
                    WMSList_shownOnBingMapsWMSLayerControl.Remove(WMSURL);
                    foreach (TreeViewItem item in tvi.Items)
                        deleteTreeItemAndWMSLayers(item);
                });

                area1.isSelected_CheckBox.Checked += new RoutedEventHandler((sender1, e1) =>
                {
                    foreach (TreeViewItem item in rootTreeNode.Items)
                    {
                        CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                        if (panel.isSelected_CheckBox.IsChecked != true)
                            panel.isSelected_CheckBox.IsChecked = true;
                    }
                });
                area1.isSelected_CheckBox.Unchecked += new RoutedEventHandler((sender1, e1) =>
                {
                    foreach (TreeViewItem item in rootTreeNode.Items)
                    {

                        CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                        if (panel.isSelected_CheckBox.IsChecked != false)
                            panel.isSelected_CheckBox.IsChecked = false;
                    }
                });

                ToolTipService.SetToolTip(area1.Image_NodeType, null);

                //we customizes the TreeviewItem: add checkbox and image icon before title text content,
                //then we use our TreeViewItem UserControl as the Header Object of TreeViewItem.
                rootTreeNode.Header = area1;
                rootTreeNode.IsExpanded = true;

                foreach (GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer in layerObject)
                {
                    addTreeItemToParentTreeItem(layer, wms, rootTreeNode);
                }
                Layers_Treeview.Items.Add(rootTreeNode);
            }
            //If WMS layer parsing failed, We will show the problem tip onto the treeview and let user know the url is not correct or not available now.
            else if (layerObject == null)
            {
                string WMSURL = null;
                if (wms != null)
                    WMSURL = wms.WMSURL;
                setErrorInfoWhenLoadingFailedOnWMSLayerControl(WMSURL);
            }
        }

        void setErrorInfoWhenLoadingFailedOnWMSLayerControl(string WMSURL)
        {
            TreeViewItem rootTreeNode = new TreeViewItem();
            rootTreeNode.Padding = new Thickness(0);
            CheckBoxWithImageText_BingMap area1 = new CheckBoxWithImageText_BingMap();
            area1.Title.Text = "Loading Error";

            if (WMSURL == null || WMSURL.Trim().Equals(""))
                WMSURL = "Unknown WMS URL";
            changeUIElementToolTipStringContent(area1.Title, "Loading Error (" + WMSURL + ")");
            area1.Title.FontSize = 12;
            area1.Title.FontWeight = FontWeights.Bold;
            area1.Image1.Source = new BitmapImage(new Uri("/GeoSearch;component/images/closeButton4.png", UriKind.Relative));
            area1.Image_removeTreeViewItem.Visibility = Visibility.Visible;
            area1.Image_removeTreeViewItem.Tag = rootTreeNode;

            area1.Image_removeTreeViewItem.MouseLeftButtonUp += new MouseButtonEventHandler((sender1, e1) =>
            {
                Image img = sender1 as Image;
                TreeViewItem tvi = img.Tag as TreeViewItem;
                Layers_Treeview.Items.Remove(tvi);
            });

            rootTreeNode.Header = area1;
            Layers_Treeview.Items.Add(rootTreeNode);
        }
            

        //迭代添加树节点
        void addTreeItemToParentTreeItem(GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer, GeoSearch.OtherQueryFunctionsServiceReference.HierachicalWMSLayers wms, TreeViewItem parentItem)
        {
            TreeViewItem layerTreeNode = new TreeViewItem();
            layerTreeNode.Padding = new Thickness(0);
            CheckBoxWithMultiImagesText_BingMap area2 = new CheckBoxWithMultiImagesText_BingMap();
            //we show the layer's title in tree view as tree node
            string showName = layer.title + " (Name: " + layer.name + ")";
            area2.Title.Text = showName;
            //and the true layer is taged as the object
            changeUIElementToolTipStringContent(area2.Title, showName);
            area2.Title.Tag = layer.name;
            area2.Title.FontSize = 12;
            area2.isSelected_CheckBox.Tag = layer;
            layerTreeNode.Header = area2;
            layerTreeNode.Tag = layer;
            parentItem.Items.Add(layerTreeNode);
            layerTreeNode.IsExpanded = true;

            if (layer.canGetMap && layer.Children.Count > 0)
            {
                area2.showCombinedLayer_CheckBox.Visibility = Visibility.Visible;
                area2.showCombinedLayer_CheckBox.Tag = layer;
            }
            else if ((!layer.canGetMap) && layer.Children.Count == 0)
            {
                area2.isSelected_CheckBox.Visibility = Visibility.Collapsed;
                area2.showCombinedLayer_CheckBox.Visibility = Visibility.Collapsed;
            }
            if (layer.timeEnabled)
            {
                area2.Image_Animation.Visibility = Visibility.Visible;
                area2.Image_Animation.Tag = layer.extent_time;
            }
            else
                area2.Image_Animation.Visibility = Visibility.Collapsed;

            if (layer.legendURL != null && !layer.legendURL.Trim().Equals(""))
            {
                area2.Image_Legend.Visibility = Visibility.Visible;
                area2.Image_Legend.Tag = layer.legendURL;
            }
            else
                area2.Image_Legend.Visibility = Visibility.Collapsed;

            if (layer.Children != null && layer.Children.Count > 0)
            {
                foreach (GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 in layer.Children)
                    addTreeItemToParentTreeItem(layer1, wms, layerTreeNode);
                area2.Image_NodeType.Source = new BitmapImage(new Uri("/GeoSearch;component/images/resourceTypes/Layers-icon.png", UriKind.Relative));
            }

            if (layer.legendURL != null && !layer.legendURL.Trim().Equals(""))
            {
                area2.setLegendImage(layer.legendURL);
                area2.Image_Legend.MouseLeftButtonDown += new MouseButtonEventHandler((sender1, e1) =>
                {
                    if (area2.ScrollViewer_Legend.Visibility == Visibility.Collapsed)
                        area2.ScrollViewer_Legend.Visibility = Visibility.Visible;
                    else
                        area2.ScrollViewer_Legend.Visibility = Visibility.Collapsed;
                });
            }
            else
            {
                area2.ScrollViewer_Legend.Visibility = Visibility.Collapsed; 
            }

            if (area2.Image_Animation.Visibility == Visibility.Visible)
            {
                //Animation按钮被点击的事件
                area2.Image_Animation.MouseLeftButtonDown += new MouseButtonEventHandler((sender1, e1) =>
                {
                    if (sender1 is Image)
                    {
                        Image img = sender1 as Image;
                        string extent_time = img.Tag as string;

                        clearAllTimeEnableWMSLayersForAnimationFromBingMaps();

                        if (extent_time != null && !extent_time.Trim().Equals(""))
                        {
                            string[] timespans = extent_time.Split(',');
                            DateTime from_total = DateTime.Now;
                            DateTime to_total = DateTime.Parse("1000-01-01");
                            if (timespans != null)
                            {                           
                                for (int i = 0; i < timespans.Length; i++)
                                {
                                    string timespan = timespans[i];
                                    if(timespan != null && !timespan.Trim().Equals(""))
                                    {
                                        DateTime from = DateTime.Now;
                                        DateTime to = DateTime.Now;
                                        if (timespan.Contains("/"))
                                        {
                                            string[] timespanExpression = timespan.Split('/');
                                            from = DateTime.Parse(timespanExpression[0]);
                                            to = DateTime.Parse(timespanExpression[1]);

                                            TimeEnableWMSLayer_TimeList_all.Add(timespanExpression[0]);
                                            timespanExpression[2] = timespanExpression[2].Replace("P", "");

                                            string a = timespanExpression[2].Substring(0, 1);
                                            int interval = int.Parse(a);
                                            DateTime next = from;
                                            while (true)
                                            {
                                                if (timespanExpression[2].EndsWith("M"))
                                                    next = next.AddMonths(interval);
                                                else if (timespanExpression[2].EndsWith("D"))
                                                    next = next.AddDays(interval);

                                                if (next.CompareTo(to) == -1 || next.CompareTo(to) == 0)
                                                {
                                                    TimeEnableWMSLayer_TimeList_all.Add(next.ToString("yyyy-MM-dd"));
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            from = to = DateTime.Parse(timespan);
                                            TimeEnableWMSLayer_TimeList_all.Add(timespan);
                                        }

                                        if (to.CompareTo(to_total) == 1)
                                        {
                                            to_total = to;
                                        }
                                        if (from.CompareTo(from_total) == -1)
                                        {
                                            from_total = from;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TimeEnableWMSLayer_TimeList_all.Add(extent_time);
                            }
                            AnimatiobTitle_TextBlock.Tag = wms.WMSURL;
                            button_Animation_Start.Tag = layer;
                            datePicker_From_ForAnimation.SelectedDate = from_total;
                            datePicker_From_ForAnimation.DisplayDateStart = from_total;
                            datePicker_From_ForAnimation.DisplayDateEnd = to_total;
                            datePicker_To_ForAnimation.SelectedDate = to_total;
                            datePicker_To_ForAnimation.DisplayDateStart = from_total;
                            datePicker_To_ForAnimation.DisplayDateEnd = to_total;
                            CurrentTimeName_TextBlock.Text = "";

                            //默认的Animation图层的数目为所有的图层
                            TimeEnableWMSLayer_TimeList_toBeShown.AddRange(TimeEnableWMSLayer_TimeList_all);
                            //允许一次加载图层的数量不应该多于当前Animation要显示图层的数量
                            if (initalAddMapTileNum_Animation > TimeEnableWMSLayer_TimeList_toBeShown.Count)
                                initalAddMapTileNum_Animation = TimeEnableWMSLayer_TimeList_toBeShown.Count;

                            changeControlLocationOnBingMaps();
                            AnimationControlBorder.Visibility = Visibility.Visible;
                        }
                        if (AnimationMode_AllLayerAddAtBegining)
                        {
                            string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
                            addTimeEnableWMSLayersToBingMapsByTimeNamesWithViewChange(WMSURL, layer, TimeEnableWMSLayer_TimeList_toBeShown);
                        }
                        else
                        {
                            string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
                            addTimeEnableWMSLayersToBingMapsByTimeNamesWithViewChange(WMSURL, layer, TimeEnableWMSLayer_TimeList_toBeShown.GetRange(0, initalAddMapTileNum_Animation));
                        }

                        //设置时间选择滑块
                        AnimationDateFrom_Slider.Maximum = TimeEnableWMSLayer_TimeList_all.Count-1;
                        AnimationDateTo_Slider.Maximum = TimeEnableWMSLayer_TimeList_all.Count-1;
                        AnimationDateFrom_Slider.Minimum = 0;
                        AnimationDateTo_Slider.Minimum = 0;
                        AnimationDateFrom_Slider.Value = 0;
                        AnimationDateTo_Slider.Value = TimeEnableWMSLayer_TimeList_all.Count-1;
                        
                        Index_WMSAnimationFrom = 0;
                        Index_WMSAnimationTo = TimeEnableWMSLayer_TimeList_all.Count-1;
                        //默认下次任何修改时间段的操作，都需要重新计算潜在的animation图层列表
                        needToCalculateTimeList = true;

                        button_Animation_Start.IsEnabled = true;
                        button_Animation_Pause.IsEnabled = false;
                        button_Animation_Stop.IsEnabled = false;
                        button_Animation_Previous.IsEnabled = false;
                        button_Animation_Next.IsEnabled = false;
                    }
                });
            }

            if (layer.canGetMap)
            {
                area2.Image_NodeType.MouseLeftButtonDown += new MouseButtonEventHandler((sender1, e1) =>
                {
                    if (area2.CurrentLayerOpacityController_StackPanel.Visibility == Visibility.Collapsed)
                        area2.CurrentLayerOpacityController_StackPanel.Visibility = Visibility.Visible;
                    else
                        area2.CurrentLayerOpacityController_StackPanel.Visibility = Visibility.Collapsed;
                });
                area2.WMSLayerOpacity_Slider.Tag = layer;
                area2.WMSLayerOpacity_Slider.Value = Opacity_WMSLayers;
                area2.WMSLayerOpacity_Slider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(CurrentWMSLayerOpacity_Slider_ValueChanged);
            }
            else
            { 
                ToolTipService.SetToolTip(area2.Image_NodeType, null);
            }

            if (area2.showCombinedLayer_CheckBox.Visibility == Visibility.Visible)
            { 
                area2.showCombinedLayer_CheckBox.Checked += new RoutedEventHandler((sender1, e1) =>
                {
                    if (sender1 is CheckBox)
                    {
                        CheckBox cb = sender1 as CheckBox;
                        GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = cb.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                        if (layer1.canGetMap)
                        {
                            //统一的Opacity值具有更高的优先级，默认情况下一旦改变，则所有图层都以该不透明度创建
                            if (Opacity_Default_WMSLayers != Opacity_WMSLayers)
                                addWMSLayerToBingMaps(wms.WMSURL, layer1);
                            else
                                addWMSLayerToBingMaps(wms.WMSURL, layer1, area2.WMSLayerOpacity_Slider.Value);
                        }
                    }
                });

                area2.showCombinedLayer_CheckBox.Unchecked += new RoutedEventHandler((sender1, e1) =>
                {
                    if (sender1 is CheckBox)
                    {
                        CheckBox cb = sender1 as CheckBox;
                        GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = cb.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                        if (layer1.canGetMap)
                            deleteWMSLayerFromBingMaps(wms.WMSURL, layer1);
                    }
                });
            }

            area2.isSelected_CheckBox.Checked += new RoutedEventHandler((sender1, e1) =>
            {
                if (sender1 is CheckBox)
                {
                    CheckBox cb = sender1 as CheckBox;
                    GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = cb.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                    if (layer1.Children != null && layer1.Children.Count > 0)
                    {
                        foreach (TreeViewItem item in layerTreeNode.Items)
                        {
                            CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                            if (panel.isSelected_CheckBox.IsChecked == false)
                            {
                                panel.isSelected_CheckBox.IsChecked = true;
                            }
                        }
                    }
                    else
                    {
                        if (layer1.canGetMap)
                        {
                            //统一的Opacity值具有更高的优先级，默认情况下一旦改变，则所有图层都以该不透明度创建
                            if (Opacity_Default_WMSLayers != Opacity_WMSLayers)
                                addWMSLayerToBingMaps(wms.WMSURL, layer1);
                            else
                                addWMSLayerToBingMaps(wms.WMSURL, layer1, area2.WMSLayerOpacity_Slider.Value);
                        }
                    }

                    bool allSelected = true;
                    foreach (TreeViewItem item in parentItem.Items)
                    {
                        CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                        if (panel.isSelected_CheckBox.IsChecked == false)
                        {
                            allSelected = false;
                            break;
                        }
                    }
                    if (allSelected)
                    {
                        CheckBoxWithMultiImagesText_BingMap area1 = parentItem.Header as CheckBoxWithMultiImagesText_BingMap;
                        area1.isSelected_CheckBox.IsChecked = true;
                    }
                }
            });

            area2.isSelected_CheckBox.Unchecked += new RoutedEventHandler((sender1, e1) =>
            {
                CheckBox cb = sender1 as CheckBox;
                GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = cb.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;

                if (layer1.Children != null && layer1.Children.Count > 0)
                {
                    foreach (TreeViewItem item in layerTreeNode.Items)
                    {
                        CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                        if (panel.isSelected_CheckBox.IsChecked == true)
                        {
                            panel.isSelected_CheckBox.IsChecked = false;
                        }
                    }
                }
                else
                {
                    if (layer1.canGetMap)
                        deleteWMSLayerFromBingMaps(wms.WMSURL, layer1);
                }
                
                bool allUnSelected = true;
                foreach (TreeViewItem item in parentItem.Items)
                {
                    CheckBoxWithMultiImagesText_BingMap panel = item.Header as CheckBoxWithMultiImagesText_BingMap;
                    if (panel.isSelected_CheckBox.IsChecked == true)
                    {
                        allUnSelected = false;
                        break;
                    }
                }
                if (allUnSelected)
                {
                    CheckBoxWithMultiImagesText_BingMap area1 = parentItem.Header as CheckBoxWithMultiImagesText_BingMap;
                    area1.isSelected_CheckBox.IsChecked = false;
                }
            });
        }

        //某一WMS Layer对应的不透明度滚动条的取值改变的事件
        void CurrentWMSLayerOpacity_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider)
            {
                Slider slider = sender as Slider;
                GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = slider.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                if (WMSLayer_MapLayer_Map.ContainsKey(layer1))
                {
                    MapTileLayer layer = WMSLayer_MapLayer_Map[layer1];
                    layer.Opacity = slider.Value;
                }
            }
        }

        //关闭BingMap上WMSLayers选择控件时的响应事件
        private void button_CloseLayerBorder_Click(object sender, RoutedEventArgs e)
        {
            LayerBorder.Visibility = Visibility.Collapsed;
            button_ShowWMSLayersControl.IsEnabled = true;
            //Layers_Treeview.Items.Clear();
            //清空bingMaps上的WMS Layers
            //clearAllWMSLayersFromBingMaps();
        }

        //单击BingMap上WMSLayers删除按钮的响应事件
        private void button_ClearAllWMSLayers_Click(object sender, RoutedEventArgs e)
        {
            foreach(Object o in Layers_Treeview.Items)
            {
                if (o is TreeViewItem)
                    uncheckAllCheckBoxOfTreeViewItem(o as TreeViewItem);
            }


            button_ShowWMSAnimationControl.IsEnabled = false;
            if (animationState != AnimationState.ReadyToStart)
            {
                if (animationState == AnimationState.Started)
                    animationState = AnimationState.Paused;

                button_ShowWMSAnimationControl.IsEnabled = true;
                button_Animation_Start.IsEnabled = true;
                button_Animation_Pause.IsEnabled = false;
                button_Animation_Stop.IsEnabled = false;
                button_Animation_Previous.IsEnabled = true;
                button_Animation_Next.IsEnabled = true;
            }

            foreach (MapTileLayer layer1 in WMSLayerTimeName_MapLayer_Map.Values)
            {
                if (layer1 != null)
                    myBingMap.Children.Remove(layer1);
            }
            WMSLayerTimeName_MapLayer_Map.Clear();

            foreach (MapTileLayer layer1 in WMSLayer_MapLayer_Map.Values)
            {
                if (layer1 != null)
                    myBingMap.Children.Remove(layer1);
            }
            WMSLayer_MapLayer_Map.Clear();
        }

        //取消treeviewItem及其所有子节点上的checkBox的选择
        void uncheckAllCheckBoxOfTreeViewItem(TreeViewItem tvi)
        {
            if (tvi.Header is CheckBoxWithMultiImagesText_BingMap)
            {
                CheckBoxWithMultiImagesText_BingMap header = tvi.Header as CheckBoxWithMultiImagesText_BingMap;
                if (header.isSelected_CheckBox.IsChecked == true)
                    header.isSelected_CheckBox.IsChecked = false;
                
                if(tvi.Items != null)
                {
                    foreach (Object o in tvi.Items)
                    {
                        TreeViewItem tvi1 = o as TreeViewItem;
                        uncheckAllCheckBoxOfTreeViewItem(tvi1);
                    }
                }
            }
        }

        //清空bingMaps上的WMS Layers
        void clearAllWMSLayersFromBingMaps()
        {
            List<MapTileLayer> layers = WMSLayer_MapLayer_Map.Values.ToList();
            foreach (MapTileLayer layer in layers)
            {
                myBingMap.Children.Remove(layer);
            }
            WMSLayer_MapLayer_Map.Clear();
            WMSList_shownOnBingMapsWMSLayerControl.Clear();
        }

        //清空现有显示animation信息
        void clearAllTimeEnableWMSLayersForAnimationFromBingMaps()
        {
            animationState = AnimationState.NotReady;
            TimeEnableWMSLayer_TimeList_toBeShown.Clear();
            TimeEnableWMSLayer_TimeList_all.Clear();

            foreach (MapTileLayer mapLayer in WMSLayerTimeName_MapLayer_Map.Values)
                myBingMap.Children.Remove(mapLayer);
            WMSLayerTimeName_MapLayer_Map.Clear();
        }

        //显示WMS Layers Control控件
        private void button_ShowWMSLayersControl_Click(object sender, RoutedEventArgs e)
        {
            if (LayerBorder.Visibility == Visibility.Collapsed)
            {
                LayerBorder.Visibility = Visibility.Visible;
                button_ShowWMSLayersControl.IsEnabled = false;
            }
        }

        //隐藏元数据细节面板
        private void Button_HideMetaDataDetail_Click(object sender, RoutedEventArgs e)
        {
            MetaDateDetail_Container.Visibility = Visibility.Collapsed;
            DependencyObject obj1 = Map_Container_Container.Parent;
            if (obj1 != null && obj1 is StackPanel)
            {
                StackPanel sp1 = obj1 as StackPanel;
                DependencyObject obj2 = sp1.Parent;
                if (obj2 != null && obj2 is ScrollViewer)
                {
                    ScrollViewer sv = obj2 as ScrollViewer;
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    resizeResultContainerHeight(this, smallChangeHight);
                }
            }
        }

        //单击关闭Animation控件的按钮的事件
        private void button_CloseAnimationControlBorder_Click(object sender, RoutedEventArgs e)
        {
            AnimationControlBorder.Visibility = Visibility.Collapsed;
            animationState = AnimationState.Paused;
            //隐藏当前显示的动画图层
            if(TimeEnableWMSLayer_TimeList_toBeShown.Count>0 )
            {
                string timeName = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt(currentShowIndex_Animation % TimeEnableWMSLayer_TimeList_toBeShown.Count);
                if (WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName))
                {
                    MapTileLayer currentLayer = WMSLayerTimeName_MapLayer_Map[timeName];
                    if (currentLayer != null)
                    {
                        currentLayer.Dispatcher.BeginInvoke(() =>
                        {
                            currentLayer.Visibility = Visibility.Collapsed;
                        });
                    }
                }

                string timeName1 = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt((currentShowIndex_Animation+1) % TimeEnableWMSLayer_TimeList_toBeShown.Count);
                if (WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName1))
                {
                    MapTileLayer nextLayer = WMSLayerTimeName_MapLayer_Map[timeName1];
                    if (nextLayer != null)
                    {
                        nextLayer.Dispatcher.BeginInvoke(() =>
                        {
                            nextLayer.Visibility = Visibility.Collapsed;
                        });
                    }
                }
            }
            button_ShowWMSAnimationControl.IsEnabled = true;
            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
        }

        //每过指定时间段执行的动画操作
        void Each_Tick(object o, EventArgs sender)
        {
            //for (int i = 0; i < TimeEnableWMSLayer_TimeList_currentShow.Count; i++)
            //while (isAnimationStart)      
            if (animationState == AnimationState.Started)
            {
                changeLayerForAnimation();
                int interval = interval = (int)(AnimationTimeInterval_Slider.Value * 1000);
                myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
                currentShowIndex_Animation++;
            }
            else
                myDispatcherTimer.Stop();
        }

        //切换当前显示Animation WMS Layer到TimeEnableWMSLayer_TimeList_currentShow中指定的图层
        void changeLayerForAnimation()
        {
            //if (!AnimationMode_AllLayerAddAtBegining)
                //AddNewLayerDeleteOldLayerForAnimation();
            showCurrentAndHidePreviousLayerForAnimation();
        }

        //为animation隐藏上一图层，显示下一图层
        void showCurrentAndHidePreviousLayerForAnimation()
        {
            if (currentShowIndex_Animation < 0)
                currentShowIndex_Animation += TimeEnableWMSLayer_TimeList_toBeShown.Count;
            if (TimeEnableWMSLayer_TimeList_toBeShown.Count > 0)
            {
                if (currentShowIndex_Animation >= 0)
                {
                    //显示当前图层，如果已经添加了该图层的情况下
                    bool currentLayerIsShow = false;
                    string timeName = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt(currentShowIndex_Animation % TimeEnableWMSLayer_TimeList_toBeShown.Count);
                    if (WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName))
                    {
                        MapTileLayer currentLayer = WMSLayerTimeName_MapLayer_Map[timeName];
                        //currentLayer.Visibility = Visibility.Visible;
                        //UIThread.Invoke(() => currentLayer.Visibility = Visibility.Visible); 
                        //System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                        //{
                        //    currentLayer.Visibility = Visibility.Visible;
                        //});
                        if (currentLayer != null)
                        {
                            currentLayer.Dispatcher.BeginInvoke(() =>
                            {
                                currentLayer.Visibility = Visibility.Visible;
                            });

                            CurrentTimeName_TextBlock.Dispatcher.BeginInvoke(() =>
                            {
                                CurrentTimeName_TextBlock.Text = timeName;
                            });
                        }
                        currentLayerIsShow = true;
                    }

                    //添加下面initalAddMapTileNum_Animation个图层中没有被添加的图层
                    List<string> currentToShowTime = new List<string>();
                    if (!AnimationMode_AllLayerAddAtBegining)
                    {
                        List<string> currentToAddedTime = new List<string>();
                        for (int i = 0; i < initalAddMapTileNum_Animation; i++)
                        {
                            string timeName1 = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt((currentShowIndex_Animation + i) % TimeEnableWMSLayer_TimeList_toBeShown.Count);
                            //没有则添加
                            if (!WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName1))
                                currentToAddedTime.Add(timeName1);
                            currentToShowTime.Add(timeName1);
                        }
                        GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = button_Animation_Start.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                        string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
                        if (layer1 == null || WMSURL == null)
                            return;
                        addTimeEnableWMSLayersToBingMapsByTimeNamesWithoutViewChange(WMSURL, layer1, currentToAddedTime);

                        if (!currentLayerIsShow)
                        {
                            MapTileLayer currentLayer = WMSLayerTimeName_MapLayer_Map[timeName];
                            if (currentLayer != null)
                            {
                                currentLayer.Dispatcher.BeginInvoke(() =>
                                {
                                    currentLayer.Visibility = Visibility.Visible;
                                });

                                CurrentTimeName_TextBlock.Dispatcher.BeginInvoke(() =>
                                {
                                    CurrentTimeName_TextBlock.Text = timeName;
                                });
                            }
                            currentLayerIsShow = true;
                        }
                    }
                    //如果是一次性全加载的模式，则隐藏之前的图层
                    if (AnimationMode_AllLayerAddAtBegining)
                    {
                        if (currentShowIndex_Animation >= 1)
                        {
                            string PreviousTimeName = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt((currentShowIndex_Animation - 1) % TimeEnableWMSLayer_TimeList_toBeShown.Count);
                            MapTileLayer previousLayer = WMSLayerTimeName_MapLayer_Map[PreviousTimeName];
                            previousLayer.Dispatcher.BeginInvoke(() =>
                            {
                                previousLayer.Visibility = Visibility.Collapsed;
                            });
                        }
                    }
                    //如果是非一次性全部加载的模式，则删除其他图层
                    else
                    {
                        List<string> keys = WMSLayerTimeName_MapLayer_Map.Keys.ToList();
                        foreach (string time in keys)
                        {
                            if (!currentToShowTime.Contains(time))
                            {
                                if (WMSLayerTimeName_MapLayer_Map.ContainsKey(time))
                                {
                                    MapTileLayer layer = WMSLayerTimeName_MapLayer_Map[time];
                                    if (layer != null)
                                    {
                                        myBingMap.Children.Remove(layer);
                                        WMSLayerTimeName_MapLayer_Map.Remove(time);
                                    }
                                }
                            }
                            //如果是需要显示的，但是不是当前图层则隐藏
                            else
                            {
                                if (!time.Equals(timeName))
                                {
                                    MapTileLayer layer2 = WMSLayerTimeName_MapLayer_Map[time];
                                    if (layer2 != null)
                                    {
                                        layer2.Dispatcher.BeginInvoke(() =>
                                        {
                                            layer2.Visibility = Visibility.Collapsed;
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //为animation预取新的图层，删除旧的图层（该函数针对非一次性加载所有图层的animation模式）
        //void AddNewLayerDeleteOldLayerForAnimation()
        //{
        //    if (currentShowIndex_Animation < 0)
        //        currentShowIndex_Animation += TimeEnableWMSLayer_TimeList_toBeShown.Count;

        //    if (TimeEnableWMSLayer_TimeList_toBeShown.Count > 0)
        //    {
        //        List<string> currentToShowTime = new List<string>();
        //        //预取next三个图层，如果没有的则添加
        //        {
        //            List<string> currentToAddedTime = new List<string>();
        //            for (int i = 0; i < initalAddMapTileNum_Animation; i++)
        //            {
        //                string timeName = TimeEnableWMSLayer_TimeList_toBeShown.ElementAt((currentShowIndex_Animation + i) % TimeEnableWMSLayer_TimeList_toBeShown.Count);
        //                if(!WMSLayerTimeName_MapLayer_Map.ContainsKey(timeName))
        //                    currentToAddedTime.Add(timeName);

        //                currentToShowTime.Add(timeName);
        //            }
        //            GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer1 = button_Animation_Start.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
        //            string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
        //            if (layer1 == null || WMSURL == null)
        //                return;
        //            addTimeEnableWMSLayersToBingMapsByTimeNamesWithoutViewChange(WMSURL, layer1, currentToAddedTime);
        //        }

        //        //删除其他图层
        //        {
        //            List<string> keys = WMSLayerTimeName_MapLayer_Map.Keys.ToList();
        //            foreach (string time in keys)
        //            {
        //                if (!currentToShowTime.Contains(time))
        //                {
        //                    if (WMSLayerTimeName_MapLayer_Map.ContainsKey(time))
        //                    {
        //                        MapTileLayer layer = WMSLayerTimeName_MapLayer_Map[time];
        //                        if (layer != null)
        //                        {
        //                            myBingMap.Children.Remove(layer);
        //                            WMSLayerTimeName_MapLayer_Map.Remove(time);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //单击播放按钮时，开始animation
        private void button_Animation_Start_Click(object sender, RoutedEventArgs e)
        {
            button_Animation_Start.IsEnabled = false;
            button_Animation_Pause.IsEnabled = true;
            button_Animation_Stop.IsEnabled = true;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;

            if (TimeEnableWMSLayer_TimeList_toBeShown == null || TimeEnableWMSLayer_TimeList_toBeShown.Count == 0 || TimeEnableWMSLayer_TimeList_all == null || TimeEnableWMSLayer_TimeList_all.Count == 0)
                return;
            //若未加载时间序列图层，则先解析才能做动画
            else if (TimeEnableWMSLayer_TimeList_toBeShown.Count > 0 && WMSLayerTimeName_MapLayer_Map.Count == 0)
            {
                GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer layer = button_Animation_Start.Tag as GeoSearch.OtherQueryFunctionsServiceReference.CascadedWMSLayer;
                if (AnimationMode_AllLayerAddAtBegining)
                {
                    
                    string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
                    addTimeEnableWMSLayersToBingMapsByTimeNamesWithViewChange(WMSURL, layer, TimeEnableWMSLayer_TimeList_toBeShown);
                }
                else
                {
                    string WMSURL = AnimatiobTitle_TextBlock.Tag as string;
                    addTimeEnableWMSLayersToBingMapsByTimeNamesWithViewChange(WMSURL, layer, TimeEnableWMSLayer_TimeList_toBeShown.GetRange(0, initalAddMapTileNum_Animation));
                }
            }

            animationState = AnimationState.Started;
            //Thread t = new Thread(new ThreadStart(animation));
            //t.Start();
            //int interval1 = (int)(AnimationTimeInterval_Slider.Value * 1000);
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10); // 等待间隔 

            myDispatcherTimer.Start();
        }

        //单击终止按钮时，终止animation
        private void button_Animation_Stop_Click(object sender, RoutedEventArgs e)
        {
            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            animationState = AnimationState.Stoped;
            currentShowIndex_Animation = 0;
        }

        //单击暂停按钮时，暂停animation
        private void button_Animation_Pause_Click(object sender, RoutedEventArgs e)
        {
            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            animationState = AnimationState.Paused;
        }

        //单击后退按钮时，后退到animation上一个时间对应的图层
        private void button_Animation_Previous_Click(object sender, RoutedEventArgs e)
        {
            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            animationState = AnimationState.Paused;
            currentShowIndex_Animation--;
            changeLayerForAnimation();
        }

        //单击前进按钮时，快进到animation下一个时间对应的图层
        private void button_Animation_Next_Click(object sender, RoutedEventArgs e)
        {
            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            animationState = AnimationState.Paused;
            currentShowIndex_Animation++;
            changeLayerForAnimation();
        }

        //ShowWMSAnimationControl 按钮被点击的事件，显示Animation控件，并让该按钮变为不可选
        private void button_ShowWMSAnimationControl_Click(object sender, RoutedEventArgs e)
        {
            if (AnimationControlBorder.Visibility == Visibility.Collapsed)
            {
                AnimationControlBorder.Visibility = Visibility.Visible;
                button_ShowWMSAnimationControl.IsEnabled = false;
            }
        }

        //Animation开始时间的datepicker中的选中时间被修改时，则可能需要同时调整结束时间的datepicker可选范围及选中的值
        private void datePicker_From_ForAnimation_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {      
            CalendarDateRange selectableRange = new CalendarDateRange(((DateTime)datePicker_From_ForAnimation.SelectedDate).AddDays(1), (DateTime)datePicker_To_ForAnimation.DisplayDateEnd);
            datePicker_To_ForAnimation.BlackoutDates.Clear();
            CalendarDateRange unselectableRange = new CalendarDateRange((DateTime)datePicker_To_ForAnimation.DisplayDateStart, ((DateTime)datePicker_From_ForAnimation.SelectedDate).AddDays(-1));
            datePicker_To_ForAnimation.BlackoutDates.Add(unselectableRange);
            DateTime currentSelectDate_ForTo = (DateTime)datePicker_To_ForAnimation.SelectedDate;
            if (currentSelectDate_ForTo.CompareTo(selectableRange.Start) == -1 || currentSelectDate_ForTo.CompareTo(selectableRange.End) == 1)
            {
                datePicker_To_ForAnimation.SelectedDate = selectableRange.End;
            }

            CalendarDateRange currentSelectedRange = new CalendarDateRange(((DateTime)datePicker_From_ForAnimation.SelectedDate), (DateTime)datePicker_To_ForAnimation.SelectedDate);

            calculateAnimationTimeList(currentSelectedRange);

            animationState = AnimationState.Stoped;
            currentShowIndex_Animation = 0;

            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            //默认下次任何修改时间段的操作，都需要重新计算潜在的animation图层列表
            needToCalculateTimeList = true;
        }

        //Animation结束时间的datepicker中的选中时间被修改时，则可能需要同时调整开始时间的datepicker可选范围及选中的值
        private void datePicker_To_ForAnimation_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarDateRange selectableRange = new CalendarDateRange((DateTime)datePicker_From_ForAnimation.DisplayDateStart, ((DateTime)datePicker_To_ForAnimation.SelectedDate).AddDays(-1));
            datePicker_From_ForAnimation.BlackoutDates.Clear();
            CalendarDateRange unselectableRange = new CalendarDateRange(((DateTime)datePicker_To_ForAnimation.SelectedDate).AddDays(1), (DateTime)datePicker_From_ForAnimation.DisplayDateEnd);
            datePicker_From_ForAnimation.BlackoutDates.Add(unselectableRange);
            DateTime currentSelectDate_ForFrom = (DateTime)datePicker_From_ForAnimation.SelectedDate;
            if (currentSelectDate_ForFrom.CompareTo(selectableRange.Start) == -1 || currentSelectDate_ForFrom.CompareTo(selectableRange.End) == 1)
            {
                datePicker_From_ForAnimation.SelectedDate = selectableRange.Start;
            }

            CalendarDateRange currentSelectedRange = new CalendarDateRange(((DateTime)datePicker_From_ForAnimation.SelectedDate), (DateTime)datePicker_To_ForAnimation.SelectedDate);

            calculateAnimationTimeList(currentSelectedRange);

            animationState = AnimationState.Stoped;
            currentShowIndex_Animation = 0;

            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
            //默认下次任何修改时间段的操作，都需要重新计算潜在的animation图层列表
            needToCalculateTimeList = true;
        }

        //计算潜在需要做anization的WMS图层的时间值列表
        void calculateAnimationTimeList(CalendarDateRange currentSelectedRange)
        {
            if (needToCalculateTimeList)
                TimeEnableWMSLayer_TimeList_toBeShown = getAnimationTimeList(currentSelectedRange);
            else
                TimeEnableWMSLayer_TimeList_toBeShown = TimeEnableWMSLayer_TimeList_all.GetRange(Index_WMSAnimationFrom, Index_WMSAnimationTo - Index_WMSAnimationFrom + 1);
        }

        //找到需要Animation演示的图层的timeName
        private List<string> getAnimationTimeList(CalendarDateRange currentSelectedRange)
        {
            List<string> timeList = new List<string>();
            if (TimeEnableWMSLayer_TimeList_all == null || TimeEnableWMSLayer_TimeList_all.Count == 0)
                return timeList;
            else
            {
                int to = -1;
                int from = -1;

                
                int to_shouldSmallerThan = TimeEnableWMSLayer_TimeList_all.Count - 1;
                int from_shouldSmallerThan = TimeEnableWMSLayer_TimeList_all.Count - 1;
                int from_shouldLargerThan = 0;
                int to_shouldLargerThan = 0;

                int currentLocation_ForTo = TimeEnableWMSLayer_TimeList_all.Count - 1;
                int currentLocation_ForFrom = TimeEnableWMSLayer_TimeList_all.Count - 1;

                while (true)
                {
                    string time = TimeEnableWMSLayer_TimeList_all.ElementAt(currentLocation_ForTo);
                    if (DateTime.Parse(time).CompareTo(currentSelectedRange.Start) >= 0 && DateTime.Parse(time).CompareTo(currentSelectedRange.End) <= 0)
                    {
                        if (currentLocation_ForTo == TimeEnableWMSLayer_TimeList_all.Count - 1)
                        {
                            to = currentLocation_ForTo;
                            break;
                        }
                        else
                        {
                            string time1 = TimeEnableWMSLayer_TimeList_all.ElementAt(currentLocation_ForTo + 1);
                            if (DateTime.Parse(time1).CompareTo(currentSelectedRange.End) > 0)
                            {
                                to = currentLocation_ForTo;
                                break;
                            }
                            if (DateTime.Parse(time1).CompareTo(currentSelectedRange.End) == 0)
                            {
                                to = currentLocation_ForTo + 1;
                                break;
                            }
                            else
                            {
                                if (currentLocation_ForTo >= 2)
                                {
                                    currentLocation_ForTo = currentLocation_ForTo + (to_shouldSmallerThan - currentLocation_ForTo) / 2;

                                    if (currentLocation_ForTo > TimeEnableWMSLayer_TimeList_all.Count - 1)
                                        currentLocation_ForTo = TimeEnableWMSLayer_TimeList_all.Count - 1;
                                }
                                else
                                {
                                    currentLocation_ForTo++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Parse(time).CompareTo(currentSelectedRange.Start) < 0)
                        {
                            if (currentLocation_ForTo == TimeEnableWMSLayer_TimeList_all.Count - 1)
                            {
                                to = -1;
                                break;
                            }

                            if (to_shouldLargerThan < currentLocation_ForTo)
                                to_shouldLargerThan = currentLocation_ForTo;
                            if (currentLocation_ForTo >= 2)
                            {
                                currentLocation_ForTo = currentLocation_ForTo + (to_shouldSmallerThan - currentLocation_ForTo) / 2;
                                if (currentLocation_ForTo > TimeEnableWMSLayer_TimeList_all.Count - 1)
                                    currentLocation_ForTo = TimeEnableWMSLayer_TimeList_all.Count - 1;
                            }
                            else
                                currentLocation_ForTo++;
                        }
                        else if (DateTime.Parse(time).CompareTo(currentSelectedRange.End) > 0)
                        {
                            if (currentLocation_ForTo == 0)
                            {
                                to = -1;
                                break;
                            }
                            if (to_shouldSmallerThan > currentLocation_ForTo)
                                to_shouldSmallerThan = currentLocation_ForTo;
                            currentLocation_ForTo /= 2;
                            if (to_shouldLargerThan > currentLocation_ForTo)
                                currentLocation_ForTo = to_shouldLargerThan;
                        }  
                    }
                }

                while (true)
                {
                    string time = TimeEnableWMSLayer_TimeList_all.ElementAt(currentLocation_ForFrom);
                    if (DateTime.Parse(time).CompareTo(currentSelectedRange.Start) >= 0 && DateTime.Parse(time).CompareTo(currentSelectedRange.End) <= 0)
                    {
                        if (currentLocation_ForFrom == 0)
                        {
                            from = currentLocation_ForFrom;
                            break;
                        }
                        else
                        {
                            string time1 = TimeEnableWMSLayer_TimeList_all.ElementAt(currentLocation_ForFrom - 1);
                            if (DateTime.Parse(time1).CompareTo(currentSelectedRange.Start) < 0)
                            {
                                from = currentLocation_ForFrom;
                                break;
                            }
                            if (DateTime.Parse(time1).CompareTo(currentSelectedRange.End) == 0)
                            {
                                from = currentLocation_ForFrom - 1;
                                break;
                            }
                            else
                            {
                                if (currentLocation_ForFrom >= 1)
                                {
                                    if (from_shouldSmallerThan > currentLocation_ForFrom)
                                        from_shouldSmallerThan = currentLocation_ForFrom;
                                    currentLocation_ForFrom =  currentLocation_ForFrom / 2;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (DateTime.Parse(time).CompareTo(currentSelectedRange.Start) < 0)
                        {
                            if (currentLocation_ForFrom == TimeEnableWMSLayer_TimeList_all.Count - 1)
                            {
                                from = -1;
                                break;
                            }

                            if (from_shouldLargerThan < currentLocation_ForFrom)
                                from_shouldLargerThan = currentLocation_ForFrom;

                            if (currentLocation_ForFrom >= 2)
                                currentLocation_ForFrom = currentLocation_ForFrom + (from_shouldSmallerThan - currentLocation_ForFrom) / 2;
                            else
                                currentLocation_ForFrom++;

                            if (currentLocation_ForFrom > TimeEnableWMSLayer_TimeList_all.Count - 1)
                                currentLocation_ForFrom = TimeEnableWMSLayer_TimeList_all.Count - 1;
                        }
                        else if (DateTime.Parse(time).CompareTo(currentSelectedRange.End) > 0)
                        {
                            if (currentLocation_ForFrom == 0)
                            {
                                from = -1;
                                break;
                            }
                            if (currentLocation_ForFrom >= 1)
                            {
                                if (from_shouldSmallerThan > currentLocation_ForFrom)  
                                    from_shouldSmallerThan = currentLocation_ForFrom;
                                currentLocation_ForFrom = currentLocation_ForFrom / 2;

                                if (from_shouldLargerThan > currentLocation_ForFrom)
                                    currentLocation_ForFrom = from_shouldLargerThan;
                            }
                        }
                    }
                }

                if (to != -1 && from != -1)
                {
                    Index_WMSAnimationFrom = from;
                    Index_WMSAnimationTo = to;
                    AnimationDateFrom_Slider.Value = Index_WMSAnimationFrom;
                    AnimationDateTo_Slider.Value = Index_WMSAnimationTo;
                    timeList = TimeEnableWMSLayer_TimeList_all.GetRange(from, to - from + 1);
                }
            }
            return timeList;
        }

        //Animation起始时间调节滚动条位置变化的事件
        private void AnimationDateFrom_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AnimationDateFrom_Slider.Value >= AnimationDateTo_Slider.Value)
            {
                AnimationDateFrom_Slider.Value = AnimationDateTo_Slider.Value - 1;
            }
            Index_WMSAnimationFrom = (int)(AnimationDateFrom_Slider.Value);
            string data = TimeEnableWMSLayer_TimeList_all.ElementAt(Index_WMSAnimationFrom);
            try{
                datePicker_From_ForAnimation.SelectedDate = DateTime.Parse(data);
            }
            catch (Exception e1)
            {
                e1.ToString();
            }
            //如果是拖拽滑动条来调整animation的时间段，则不需要计算时间列表，直接通过index从list中选择子集
            needToCalculateTimeList = false;

            animationState = AnimationState.Stoped;
            currentShowIndex_Animation = 0;

            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
        }

        //Animation结束时间调节滚动条位置变化的事件
        private void AnimationDateTo_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AnimationDateFrom_Slider.Value >= AnimationDateTo_Slider.Value)
            {
                AnimationDateTo_Slider.Value = AnimationDateFrom_Slider.Value+1;
            }
            Index_WMSAnimationTo = (int)(AnimationDateTo_Slider.Value);
            string data = TimeEnableWMSLayer_TimeList_all.ElementAt(Index_WMSAnimationTo);
            try
            {
                datePicker_To_ForAnimation.SelectedDate = DateTime.Parse(data);
            }
            catch (Exception e1)
            {
                e1.ToString();
            }
            //如果是拖拽滑动条来调整animation的时间段，则不需要计算时间列表，直接通过index从list中选择子集
            needToCalculateTimeList = false;

            animationState = AnimationState.Stoped;
            currentShowIndex_Animation = 0;

            button_Animation_Start.IsEnabled = true;
            button_Animation_Pause.IsEnabled = false;
            button_Animation_Stop.IsEnabled = false;
            button_Animation_Previous.IsEnabled = true;
            button_Animation_Next.IsEnabled = true;
        }

        //WMS Layers不透明度滚动条位置变化的事件
        private void WMSLayersOpacity_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WMSLayersOpacity_Slider != null)
            {
                Opacity_WMSLayers = WMSLayersOpacity_Slider.Value;
                foreach (TreeViewItem child_item in Layers_Treeview.Items)
                {
                    changeAllLyersOpacitySliderValue(child_item, Opacity_WMSLayers);
                }
            }
        }

        void changeAllLyersOpacitySliderValue(TreeViewItem item, double opacity)
        {
            if (item.Header is CheckBoxWithMultiImagesText_BingMap)
            {
                CheckBoxWithMultiImagesText_BingMap header = item.Header as CheckBoxWithMultiImagesText_BingMap;
                header.WMSLayerOpacity_Slider.Value = opacity;
                foreach (TreeViewItem child_item in item.Items)
                {
                    changeAllLyersOpacitySliderValue(child_item, opacity);
                }
            }
        }

        //WMS Animation Layers不透明度滚动条位置变化的事件
        private void WMSAnimationLayersOpacity_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WMSAnimationLayersOpacity_Slider != null)
            {
                Opacity_WMSAnimationLayers = WMSAnimationLayersOpacity_Slider.Value;
                //if (!animationState.Equals(AnimationState.Started))
                foreach (MapTileLayer layer in WMSLayerTimeName_MapLayer_Map.Values.ToList())
                {
                    if (layer.Opacity != Opacity_WMSAnimationLayers)
                        layer.Opacity = Opacity_WMSAnimationLayers;
                }
            }
        }

        //切换到Pivot Viewer视图
        private void PivotViewer_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (FoundRecords != null && FoundRecords.Count > 0)
            {
                PivotViewer_Container.Visibility = Visibility.Visible;
                if (animationState == AnimationState.Started)
                    animationState = AnimationState.Paused;
                Results_Container.Visibility = Visibility.Collapsed;
                NormalViewer_HyperlinkButton.IsEnabled = true;
                PivotViewer_HyperlinkButton.IsEnabled = false;
                if (PivotViewer_Container.PivotViewer.ItemsSource == null)
                {
                    PivotViewer_Container.PivotViewer.ItemsSource = FoundRecords;
                    PivotViewer_Container.PivotViewer.UpdateLayout();
                }
            }
        }

        //切换到Normal Viewer视图
        private void NormalViewer_HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToNormalViewer();
        }

        private void NavigateToNormalViewer()
        {
            PivotViewer_Container.Visibility = Visibility.Collapsed;
            Results_Container.Visibility = Visibility.Visible;
            NormalViewer_HyperlinkButton.IsEnabled = false;
            PivotViewer_HyperlinkButton.IsEnabled = true;
        }
    }

    public static class UIThread
    {
        private static readonly Dispatcher dispatcher;

        static UIThread()
        {
            // Store a reference to the current Dispatcher once per application 
            dispatcher = Deployment.Current.Dispatcher;
        }
        /// <summary> 
        ///   Invokes the given action on the UI thread - if the current thread is the UI thread this will just invoke the action directly on 
        ///   the current thread so it can be safely called without the calling method being aware of which thread it is on. 
        /// </summary> 
        public static void Invoke(Action action)
        {
            if (dispatcher.CheckAccess())
                action.Invoke();
            else
                dispatcher.BeginInvoke(action);
        }
    } 

    public class CategoryPageMap
    {
        public string Key;
        public TabItem tabItem;
        public ObservableCollection<ClientSideRecord> recordslist;
        public TextBlock textblockForRecordCount;
        public PagedCollectionView pagedCollectionView;
        public ListBox listBox;
        public StackPanel stackPanelContainer_ForRecordsListBoxAndBingMap;
        public DataPager dataPager;
        public StackPanel stackPanel_SortingRuleComboList;
        public ComboBox comboBox_SortingRule;

        public CategoryPageMap(String key, TabItem tabitme, ObservableCollection<ClientSideRecord> list)
        {
            Key = key;
            tabItem = tabitme;
            recordslist = list;
        }
    }

    public class RecordAndPushPinMap
    {
        public ClientSideRecord record;
        public Pushpin pin;
        public MapPolygon bboxPolygon;

        public RecordAndPushPinMap(ClientSideRecord record1, Pushpin pin1, MapPolygon bboxPolygon1)
        {
            record = record1;
            pin = pin1;
            bboxPolygon = bboxPolygon1;
        }
    }
}
