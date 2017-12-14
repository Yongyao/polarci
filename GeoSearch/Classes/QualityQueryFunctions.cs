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
using GeoSearch.QualityQueryServiceReference;
using System.Collections.ObjectModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class QualityQueryFunctions
    {
        public QualityQueryServiceClient proxy = new QualityQueryServiceClient();
        public ServiceQualityPopup view = null;
        public int ID;

        public QualityQueryFunctions(int id)
        {
            proxy.getServiceQoSHistoricalInfoCompleted += new EventHandler<getServiceQoSHistoricalInfoCompletedEventArgs>(proxy_getServiceQoSHistoricalInfoCompleted);
            proxy.getServiceQoSSummaryInfoCompleted += new EventHandler<getServiceQoSSummaryInfoCompletedEventArgs>(proxy_getServiceQoSSummaryInfoCompleted);
            proxy.getWMSLayersQoSSummaryInfoCompleted +=new EventHandler<getWMSLayersQoSSummaryInfoCompletedEventArgs>(proxy_getWMSLayersQoSSummaryInfoCompleted);
            proxy.getWMSLayersQoSHistoricalInfoCompleted += new EventHandler<getWMSLayersQoSHistoricalInfoCompletedEventArgs>(proxy_getWMSLayersQoSHistoricalInfoCompleted);
            ID = id;
        }

        public void getGeospatialServiceQoSSummaryInfo(ObservableCollection<Record> recordList)
        {
            if (recordList != null)
            {
                ObservableCollection<ServiceInfoForSummary> serviceInfoList = new ObservableCollection<ServiceInfoForSummary>();
                foreach (Record record in recordList)
                {
                    if (record.Type.Equals(ConstantCollection.ServiceType_WMS) || record.Type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization) || record.Type.Equals(ConstantCollection.ServiceType_WCS) || record.Type.Equals(ConstantCollection.ServiceType_WFS) || record.Type.Equals(ConstantCollection.ServiceType_WPS))
                    {
                        string url = record.AccessURL;
                        url = url.Replace("&amp;", "&");
                        int index = url.LastIndexOf("?");
                        string newURL = url;
                        if (index >= 0)
                        {
                            newURL = url.Substring(0, index + 1);
                            string paramsString = url.Substring(index + 1);
                            if (paramsString != null && (!paramsString.Equals("")))
                            {
                                string[] parameters = paramsString.Split('&');
                                string serviceTypeString;
                                for (int i = 0; i < parameters.Length; i++)
                                {
                                    string parameter = parameters[i];
                                    if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_GetCapabilities + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_GetMap + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_Request + "="))
                                        continue;
                                    //else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_Version + "="))
                                    //continue;
                                    else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_Service + "="))
                                    {
                                        int indexOfEqual = parameters[i].IndexOf("=");
                                        if (indexOfEqual > 0)
                                        {
                                            serviceTypeString = parameters[i].Substring(indexOfEqual + 1);
                                            if (serviceTypeString.ToLower().Equals("wms"))
                                                record.Type = ConstantCollection.ServiceType_WMS;
                                            else if (serviceTypeString.ToLower().Equals("wcs"))
                                                record.Type = ConstantCollection.ServiceType_WCS;
                                            else if (serviceTypeString.ToLower().Equals("wfs"))
                                                record.Type = ConstantCollection.ServiceType_WFS;
                                            else if (serviceTypeString.ToLower().Equals("wps"))
                                                record.Type = ConstantCollection.ServiceType_WPS;
                                        }
                                        continue;
                                    }
                                    else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_Format + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(ConstantCollection.OGCServicePamameter_Updatesequence + "="))
                                        continue;
                                    else
                                        newURL += "&" + parameters[i];
                                }
                            }
                        }
                        else
                            newURL += "?";
                        record.RealServiceURL = newURL;

                        string serviceType = record.Type;

                        ServiceInfoForSummary serviceInfo = new ServiceInfoForSummary();
                        serviceInfo.serviceType = serviceType;
                        serviceInfo.serviceURL = record.RealServiceURL;
                        serviceInfoList.Add(serviceInfo);
                    }

                }
                if (serviceInfoList.Count > 0)
                {
                    proxy.getServiceQoSSummaryInfoAsync(serviceInfoList);
                }
            }
        }

        public void getGeospatialServiceQoSHistoricalInfo(ClientSideRecord record, DateTime fromDate, DateTime toDate)
        {
            if (record != null)
            {
                ObservableCollection<ServiceInfoForHistory> serviceInfoForHistoryList = new ObservableCollection<ServiceInfoForHistory>();
                ServiceInfoForHistory serviceInfoForHistory = new ServiceInfoForHistory();
                serviceInfoForHistory.serviceType = record.Type;
                serviceInfoForHistory.serviceURL = record.RealServiceURL;
                string endDate = toDate.ToString("yyyy-MM-dd");
                string startDate = fromDate.ToString("yyyy-MM-dd");
                serviceInfoForHistory.enddate = endDate;
                serviceInfoForHistory.startdate = startDate;
                serviceInfoForHistoryList.Add(serviceInfoForHistory);

                this.proxy.getServiceQoSHistoricalInfoAsync(serviceInfoForHistoryList);
            }
        }

        public void getWMSLayerQoSSummaryInfo(ClientSideRecord record)
        {
            if (record != null)
            {
                ServiceInfoForSummary serviceInfoForSummary = new ServiceInfoForSummary();
                serviceInfoForSummary.serviceType = record.Type;
                serviceInfoForSummary.serviceURL = record.RealServiceURL;
                this.proxy.getWMSLayersQoSSummaryInfoAsync(serviceInfoForSummary);
            }
        }

        public void getWMSLayerQoSHistoralInfo(ClientSideRecord record, string layerName, DateTime fromDate, DateTime toDate)
        {
            if (record != null && layerName!= null && !layerName.Trim().Equals(""))
            {
                LayerInfoForHistory layerInfoForHistory = new LayerInfoForHistory();
                layerInfoForHistory.serviceType = record.Type;
                layerInfoForHistory.serviceURL = record.RealServiceURL;
                layerInfoForHistory.layerName = layerName;
                string endDate = toDate.ToString("yyyy-MM-dd");
                string startDate = fromDate.ToString("yyyy-MM-dd");
                layerInfoForHistory.startdate = startDate;
                layerInfoForHistory.enddate = endDate;
                this.proxy.getWMSLayersQoSHistoricalInfoAsync(layerInfoForHistory);
            }
        }

        void proxy_getServiceQoSSummaryInfoCompleted(object sender, getServiceQoSSummaryInfoCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            else
            {
                ObservableCollection<ServiceQoSInfoForSummary> results = e.Result;
                if (results != null && results.Count > 0)
                {
                    App app = (App)Application.Current;
                    UIElement uiElement = app.RootVisual;
                    if (uiElement is UserControl)
                    {
                        UserControl control = uiElement as UserControl;
                        UIElement root = control.Content;
                        if (root is SearchingResultPage)
                        {
                            SearchingResultPage srp = root as SearchingResultPage;

                            //if user trigger new search during current search stage, current search results will be ignored and this iterative search will be ended.
                            //if not, these search results will be add to current result page
                            if (SearchingResultPage.ID == ID)
                            {
                                foreach (ServiceQoSInfoForSummary performanceInfo in results)
                                {
                                    ObservableCollection<ClientSideRecord> list = srp.TabItem_WebServices.DataContext as ObservableCollection<ClientSideRecord>;
                                    foreach (ClientSideRecord record in list)
                                    //foreach (Record record in srp.searchedRecords)
                                    {
                                        if (record.RealServiceURL != null && record.RealServiceURL.Equals(performanceInfo.serviceURL))
                                        {
                                            record.Quality = performanceInfo.rankValue;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void proxy_getServiceQoSHistoricalInfoCompleted(object sender, getServiceQoSHistoricalInfoCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            else
            {
                ServiceQoSInfoForHistory results = e.Result;
                //if (results != null)
                {
                    App app = (App)Application.Current;
                    UIElement uiElement = app.RootVisual;
                    if (uiElement is UserControl)
                    {
                        UserControl control = uiElement as UserControl;
                        UIElement root = control.Content;
                        if (root is SearchingResultPage)
                        {
                            SearchingResultPage srp = root as SearchingResultPage;
                            //if user trigger new search during current search stage, current search results will be ignored and this iterative search will be ended.
                            //if not, these search results will be add to current result page
                            if (SearchingResultPage.ID == ID)
                            {
                                SearchingResultPage.showQoSDetailInformationInControls(results, view);
                            }
                        }
                    }
                }
            }
        }

        void proxy_getWMSLayersQoSSummaryInfoCompleted(object sender, getWMSLayersQoSSummaryInfoCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            else
            {
                WMSLayerInfoWithQoS results = e.Result;
                if (results != null && results.WMSLayersList!= null && results.WMSLayersList.Count > 0)
                {
                    App app = (App)Application.Current;
                    UIElement uiElement = app.RootVisual;
                    if (uiElement is UserControl)
                    {
                        UserControl control = uiElement as UserControl;
                        UIElement root = control.Content;
                        if (root is SearchingResultPage)
                        {
                            SearchingResultPage srp = root as SearchingResultPage;

                            //if user trigger new search during current search stage, current search results will be ignored and this iterative search will be ended.
                            //if not, these search results will be add to current result page
                            if (SearchingResultPage.ID == ID)
                            {
                                if (view != null)
                                {
                                    view.serviceQualityInfoPanel.showLayerInfoOnTreeView(results);
                                }                            
                           }
                        }
                    }
                }
            }
        }

        void proxy_getWMSLayersQoSHistoricalInfoCompleted(object sender, getWMSLayersQoSHistoricalInfoCompletedEventArgs e)
        {
            if (e.Error != null)
            {
            }
            else
            {
                LayerQoSInfoForHistory results = e.Result;
                if (results != null && results.measurementInfoList.Count > 0)
                {
                    App app = (App)Application.Current;
                    UIElement uiElement = app.RootVisual;
                    if (uiElement is UserControl)
                    {
                        UserControl control = uiElement as UserControl;
                        UIElement root = control.Content;
                        if (root is SearchingResultPage)
                        {
                            SearchingResultPage srp = root as SearchingResultPage;

                            //if user trigger new search during current search stage, current search results will be ignored and this iterative search will be ended.
                            //if not, these search results will be add to current result page
                            if (SearchingResultPage.ID == ID)
                            {
                                if (view != null)
                                {
                                    view.serviceQualityInfoPanel.showLayerHistoryInfoOnChart(results);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
