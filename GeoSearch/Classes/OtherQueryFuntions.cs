using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GeoSearch.StatisticsServiceReference;
using GeoSearch.OtherQueryFunctionsServiceReference;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class OtherQueryFuntions
    {
        public StatisticsServiceClient statisticsServiceClient = new StatisticsServiceClient();
        public OtherQueryFunctionsServiceClient otherQueryFunctionsServiceClient = new OtherQueryFunctionsServiceClient();
        public string recordsDetail = null;
        public int ID;
        //silverlight user client's IP address
        public string CLientIPAddress = "";

        public static List<double> allSearchTimeFromFirstRequest = new List<double>();

        public OtherQueryFuntions()
        {
            otherQueryFunctionsServiceClient.getHierachicalLayersOfWMSCompleted += new EventHandler<OtherQueryFunctionsServiceReference.getHierachicalLayersOfWMSCompletedEventArgs>(otherQueryFunctionsServiceClient_getHierachicalLayersOfWMSCompleted);
            otherQueryFunctionsServiceClient.getRecordDetailMetadataCompleted += new EventHandler<getRecordDetailMetadataCompletedEventArgs>(proxy_getRecordsDetailInfoCompleted);
            statisticsServiceClient.getVisitIPAddressCompleted += new EventHandler<getVisitIPAddressCompletedEventArgs>(proxy_getVisitIPAddressCompleted);
            statisticsServiceClient.getServerInfoFromURLSCompleted += new EventHandler<getServerInfoFromURLSCompletedEventArgs>(proxy_getServerInfoFromURLSCompleted);
        }

        public void getRecordDetailMetadata_Using_WCFService(string MetadataAccessURL, int id)
        {
            if (MetadataAccessURL == null || MetadataAccessURL.Trim().Equals(""))
                return;
            ID = id;
            otherQueryFunctionsServiceClient.getRecordDetailMetadataAsync(MetadataAccessURL);
        }

        public void proxy_getRecordsDetailInfoCompleted(object sender, getRecordDetailMetadataCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                recordsDetail = null;
            }
            else
            {
                recordsDetail = e.Result;
                //SearchingResultPage.showMetadataDetailPopupPage(recordsDetail, new Point(SearchingResultPage.pop.MetadataDetailPage.HorizontalOffset, SearchingResultPage.pop.MetadataDetailPage.VerticalOffset));
                if (ID == SearchingResultPage.ID)
                    SearchingResultPage.showMetadataDetailFloatableWindow(recordsDetail);
            }
        }

        public void otherQueryFunctionsServiceClient_getHierachicalLayersOfWMSAsync(string WMSURL, int id)
        {
            ID = id;
            otherQueryFunctionsServiceClient.getHierachicalLayersOfWMSAsync(WMSURL);
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
                if (ID == SearchingResultPage.ID)
                {
                    SearchingResultPage.addHierachicalLayersToWMSLayer(e.Result);
                }
            }
        }

        public void getVisitClientIPAddress(int id)
        {
            ID = id;
            statisticsServiceClient.getVisitIPAddressAsync();
        }

        void proxy_getVisitIPAddressCompleted(object sender, getVisitIPAddressCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //records = null;
            }
            else
            {
                if (SearchingResultPage.ID == ID)
                {
                    string IPAddress = e.Result;
                    CLientIPAddress = IPAddress;
                    ResponseTimeWindows rtw = new ResponseTimeWindows();
                    string responseTimeString = "";
                    responseTimeString += ("Client side IP: " + IPAddress + "\n");
                    foreach (double responseTime in allSearchTimeFromFirstRequest)
                    {
                        responseTimeString += (responseTime + "\n");
                    }
                    rtw.TextBox_ResponseTime.Text = responseTimeString;
                    rtw.Show();
                }
            }
        }

        public void getServerInformationFromURLS(ObservableCollection<string> URLList, int id)
        {
            ID = id;
            statisticsServiceClient.getServerInfoFromURLSAsync(URLList);
        }

        void proxy_getServerInfoFromURLSCompleted(object sender, getServerInfoFromURLSCompletedEventArgs e) 
        { 
            if (e.Error != null)
            {
                //records = null;
            }
            else
            {
                if (SearchingResultPage.ID == ID)
                {
                    SearchingResultPage srp = null;
                    ObservableCollection<ClientSideRecord> RecordList = null;
                    App app = (App)Application.Current;
                    UIElement uiElement = app.RootVisual;
                    if (uiElement is UserControl)
                    {
                        UserControl control = uiElement as UserControl;
                        UIElement root = control.Content;
                        if (root is SearchingResultPage)
                        {
                            srp = root as SearchingResultPage;
                        }
                    }
                    if (srp != null)
                    {
                        RecordList = srp.FoundRecords;
                        ObservableCollection<ServerInfo> ServerInfoList = e.Result;
                        foreach (ServerInfo serverInfo in ServerInfoList)
                        {
                            foreach (ClientSideRecord record in RecordList)
                            {
                                if (record.AccessURL.Equals(serverInfo.URL))
                                {
                                    record.URLLocation = serverInfo.LonLat;
                                    break;
                                }
                            }
                        }

                        //刷新Bing Maps以更新record的location信息
                        List<ClientSideRecord> list = srp.getRecordsListInCurrentPage(null);
                        srp.showSpecifiedRecordsInBingMap(list);
                    }
                }
            }
        }
    }
}
