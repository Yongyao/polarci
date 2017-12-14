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
using System.Windows.Media.Imaging;
using GeoSearch.OtherQueryFunctionsServiceReference;
using GeoSearch.Widgets;

namespace GeoSearch
{
    public partial class WMSLayyersSelectionPage : ChildWindow
    {
        OtherQueryFunctionsServiceClient proxy = new OtherQueryFunctionsServiceClient();
        string WMSURL = null;
        
        public WMSLayyersSelectionPage(string WMSCapabilitiesDocumentURL)
        {
            InitializeComponent();

            //int index = WMSCapabilitiesDocumentURL.IndexOf("?");
            //if(index>0)
            //    WMSCapabilitiesDocumentURL = WMSCapabilitiesDocumentURL.Substring(0, index+1);
            WMSURL = WMSCapabilitiesDocumentURL;
            //this.Layers_Treeview.ItemContainerStyle = this.Resources["LayersItemStyle"] as Style; 
            proxy.getAllLayerNamesOfWMSCompleted += new EventHandler<getAllLayerNamesOfWMSCompletedEventArgs>(proxy_getAllLayerNamesOfWMSCompleted);

            LoadingLayers(WMSCapabilitiesDocumentURL);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            List<string> layerNameList = getAllSelectedLayersNames();
            //if no layer is selected, we will do nothing, to void opening a empty layer onto OpenLayers
            if(layerNameList.Count == 0)
                return;
            
            string result = "&layer=";
            foreach (string layerName in layerNameList)
            {
                result += (layerName + ",");
            }

            int len = result.Length;
            result = result.Substring(0, len-1);

            string url = ConstantCollection.OpenLayerView_URL + "server=" + WMSURL + result;
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(url), "OpenlayerViewer");
        }

        private List<string> getAllSelectedLayersNames()
        {
            List<string> layerNameList = new List<string>();
            foreach (TreeViewItem item in this.Layers_Treeview.Items)
            {
                List<string> layerNameList1 = getAllSelectedLayersNames_FromParentItem(item);
                layerNameList.AddRange(layerNameList1);
            }
            return layerNameList;
        }

        //Iterate to add TreeViewItem onto TreeView
        private List<string> getAllSelectedLayersNames_FromParentItem(TreeViewItem parent)
        {
            List<string> layerNameList = new List<string>();
            foreach( TreeViewItem item in parent.Items)
            {
                if(item.Items == null ||item.Items.Count == 0)
                {
                    CheckBoxWithImageText panel = item.Header as CheckBoxWithImageText;
                    if(panel.isSelected_CheckBox.IsChecked == true)
                    {
                        //get the layer name from Textbox's taged object
                        layerNameList.Add(panel.Title.Tag as string);
                    }
                }
                else
                {
                    List<string> layerNameList1 = getAllSelectedLayersNames_FromParentItem(item);
                    layerNameList.AddRange(layerNameList1);
                }
            }
            return layerNameList;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //Function to load WMS Layers by using client proxy to invoke WCF Services
        public void LoadingLayers(string WMSCapabilitiesDocumentURL)
        {
            if (WMSCapabilitiesDocumentURL == null || WMSCapabilitiesDocumentURL.Trim().Equals(""))
                return;
            proxy.getAllLayerNamesOfWMSAsync(WMSCapabilitiesDocumentURL);
        }

        public void addLayers(ObservableCollection<WMSLayer> layerObject)
        {
            TreeViewItem rootTreeNode = new TreeViewItem();

            CheckBoxWithImageText area1 = new CheckBoxWithImageText();
            area1.Title.Text = "All layers [" + layerObject.Count + "]";
            area1.Title.FontSize = 15;
            area1.Title.FontWeight = FontWeights.Bold;
            area1.Image1.Source = new BitmapImage(new Uri("/GeoSearch;component/images/resourceTypes/Layers-icon.png", UriKind.Relative));


            area1.isSelected_CheckBox.Checked += new RoutedEventHandler((sender, e) =>
            {
                foreach (TreeViewItem item in rootTreeNode.Items)
                {
                    CheckBoxWithImageText panel = item.Header as CheckBoxWithImageText;
                    panel.isSelected_CheckBox.IsChecked = true;
                    // .Children[0] as CheckBox).IsChecked = true;
                }
            });
            area1.isSelected_CheckBox.Unchecked += new RoutedEventHandler((sender, e) =>
            {

                foreach (TreeViewItem item in rootTreeNode.Items)
                {

                    CheckBoxWithImageText panel = item.Header as CheckBoxWithImageText;
                    panel.isSelected_CheckBox.IsChecked = false;
                    //((item.Header as StackPanel).Children[0] as CheckBox).IsChecked = false;
                }
            });

            //we customs the TreeviewItem: add checkbox and image icon before title text content,
            //then we use our TreeViewItem UserControl as the Header Object of TreeViewItem.
            rootTreeNode.Header = area1;
            rootTreeNode.IsExpanded = true;

            foreach (WMSLayer layer in layerObject)
            {
                TreeViewItem layerTreeNode = new TreeViewItem();
                CheckBoxWithImageText area2 = new CheckBoxWithImageText();
                //we show the layer's title in tree view as tree node
                area2.Title.Text = layer.title + " (Name: " + layer.name + ")";
                //and the true layer is taged as the object
                area2.Title.Tag = layer.name;
                layerTreeNode.Header = area2;
                rootTreeNode.Items.Add(layerTreeNode);
            }
            this.Layers_Treeview.Items.Add(rootTreeNode);


            //ObservableCollection<Layer> layers = getLayersNamesFromWMSCapabilitiesDocument1();
            //foreach (Layer layer in layers)
            //{
            //    TreeViewItem layerTreeNode = new TreeViewItem();
            //    layerTreeNode.ItemContainerStyle = this.Resources["LayersItemStyle"] as Style;
            //    //layerTreeNode.Items.Add("");
            //    layerTreeNode.Visibility = System.Windows.Visibility.Visible;
            //    layerTreeNode.Width = 200;
            //    layerTreeNode.Tag = "";
            //    //layerTreeNode.Header = layer.name;

            //    //layerTreeNode.DataContext = layer;
            //    rootTreeNode.Items.Add(layerTreeNode);
            //}

            //this.Layers_Treeview.Items.Add(rootTreeNode); 
        }

        void proxy_getAllLayerNamesOfWMSCompleted(object sender, getAllLayerNamesOfWMSCompletedEventArgs e)
        {
            ObservableCollection<WMSLayer> layerObject = null;
            if (e.Error != null)
            {
                layerObject = null;
            }
            else
            {
                layerObject = e.Result.layersList;

                if (layerObject != null)
                    addLayers(layerObject);
                //If WMS layer parsing failed, We will show the problem tip onto the treeview and let user know the url is not correct or not available now.
                else if (layerObject == null)
                {
                    StackPanel sp = new StackPanel();

                    TextBlock tb1 = new TextBlock();
                    tb1.Width = 440;
                    tb1.Height = 40;
                    tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb1.TextWrapping = TextWrapping.Wrap;
                    tb1.FontSize = 15;
                    tb1.FontWeight = FontWeights.Bold;
                    SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                    // Describes the brush's color using RGB values. 
                    // Each value has a range of 0-255.
                    mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
                    tb1.Foreground = mySolidColorBrush;
                    tb1.Text = "The WMS URL is not correct or accessible for now:";

                    TextBlock tb2 = new TextBlock();
                    tb2.Width = 440;
                    tb2.MinHeight = 200;
                    tb2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb2.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tb2.TextWrapping = TextWrapping.Wrap;
                    tb2.FontSize = 13;
                    tb2.FontWeight = FontWeights.Normal;
                    tb2.Text = "\"" + WMSURL + "\""; ;
                    sp.Children.Add(tb1);
                    sp.Children.Add(tb2);

                    scrollViewer1.Content = sp; 
                }
            }
        }
    }
}

