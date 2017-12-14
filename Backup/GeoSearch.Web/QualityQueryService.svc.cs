using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Linq;
using System.Collections.Generic;

namespace GeoSearch.Web
{
    [ServiceContract(Namespace = "cisc.gmu.edu/QualityQueryService")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QualityQueryService
    {
        public const string ServicePerformanceCheckerURL = "http://wms.gmu.edu/CISCchecker/SWPOperator";
        //public const string ServicePerformanceCheckerURL = "http://ec2-50-19-223-225.compute-1.amazonaws.com:8080/CISCchecker/SWPOperator";

        public const string CISCChecker_namespace = "http://cisc.gmu.edu.CISCChecker";

        public const string soapenv_namespace = "http://schemas.xmlsoap.org/soap/envelope/";
        public const string xsd_namespace = "http://www.w3.org/2001/XMLSchema";
        public const string xsi_namespace = "http://www.w3.org/2001/XMLSchema-instance";


        public const string GetServiceBriefRankList_String = "GetServiceBriefRankList";
        public const string ServiceInfo_String = "ServiceInfo";
        public const string URL_String = "URL";
        public const string ServiceType_String = "ServiceType";

        public const string GetServiceBriefRankListResponse_String = "GetServiceBriefRankListResponse";
        public const string ServiceRankInfo_String = "ServiceRankInfo";
        public const string RankValue_String = "RankValue";
        public const string FGDCScore_String = "FGDCScore";

        public const string GetServicePerforDetail_String = "GetServicePerforDetail";
        public const string ServiceInfoWithDate_String = "ServiceInfoWithDate";
        public const string FromDate_String = "FromDate";
        public const string ToDate_String = "ToDate";
        public const string StartDate_String = "StartDate";
        public const string EndDate_String = "EndDate";

        public const string GetServicePerforDetailResponse_String = "GetServicePerforDetailResponse";
        public const string ServiceDetailInfo_String = "ServiceDetailInfo";
        public const string DailyPerfor_String = "DailyPerfor";
        public const string responseTime_String = "ResponseTime";
        public const string measureDate_String = "Date";

        public const string GetLayerPerforRank_String = "GetLayerPerforRank";
        public const string LayerRankInfo_String = "LayerRankInfo";
        public const string GetLayerPerforRankResponse_String = "GetLayerPerforRankResponse";
        public const string Layer_String = "Layer";

        public const string LayerName_String = "LayerName";
        public const string LayerTitle_String = "LayerTitle";
        public const string GetLayerPerforDetailResponse_String = "GetLayerPerforDetailResponse";
        public const string LayerDetailInfo_String = "LayerDetailInfo";

        public const string GetLayerPerforDetail_String = "GetLayerPerforDetail";
        public const string LayerInfoWithDate_String = "LayerInfoWithDate";

        public string resultString = null;

        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        //[OperationContract]
        //public void DoWork()
        //{
        //     Add your operation implementation here
        //    return;
        //}

        [OperationContract]
        public List<ServiceQoSInfoForSummary> getServiceQoSSummaryInfo(List<ServiceInfoForSummary> serviceInfoList)
        {
            XDocument doc1 = new XDocument();
            XElement Envelope = new XElement(XName.Get("Envelope", soapenv_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsd", xsd_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "soapenv", soapenv_namespace));

            XElement Body = new XElement(XName.Get("Body", soapenv_namespace));
            XElement GerBriefRankList = new XElement(XName.Get(GetServiceBriefRankList_String, CISCChecker_namespace));
            GerBriefRankList.Add(new XAttribute(XNamespace.Xmlns + "ns", CISCChecker_namespace));

            List<ServiceQoSInfoForSummary> servicePerformanceInfoList = new List<ServiceQoSInfoForSummary>();
            foreach (ServiceInfoForSummary serviceInfo in serviceInfoList)
            {
                string url = serviceInfo.serviceURL;
                string serviceType = serviceInfo.serviceType;
                if (url != null && !url.Trim().Equals("") && serviceType != null && !serviceType.Trim().Equals(""))
                {
                    //XElement ServiceInfo = new XElement(XName.Get("ServiceInfo", CISCChecker_namespace));
                    XElement ServiceInfo = new XElement(ServiceInfo_String);
                    ServiceInfo.SetAttributeValue(URL_String, url);
                    ServiceInfo.SetAttributeValue(ServiceType_String, serviceType);
                    GerBriefRankList.Add(ServiceInfo);
                }
            }
            Body.Add(GerBriefRankList);
            Envelope.Add(Body);
            doc1.Add(Envelope);

            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(ServicePerformanceCheckerURL, postRequest);
            if (resultString == null || resultString.Trim().Equals(""))
            {
                return null;
            }
            XDocument doc = XDocument.Parse(resultString);
            if (doc != null)
            {
                XElement rootElement = doc.Root;
                if (!rootElement.Name.LocalName.Equals("Envelope"))
                {
                    return null;
                }

                XElement responeBody = rootElement.Element(XName.Get("Body", soapenv_namespace));
                if (responeBody != null)
                {
                    XElement GerBriefRankListResponse = responeBody.Element(XName.Get(GetServiceBriefRankListResponse_String, CISCChecker_namespace));
                    if (GerBriefRankListResponse != null)
                    {
                        //IEnumerable<XElement> rankinfoList = GerBriefRankListResponse.Elements(XName.Get("rankinfo", CISCChecker_namespace));
                        IEnumerable<XElement> rankinfoList = GerBriefRankListResponse.Elements(ServiceRankInfo_String);
                        if (rankinfoList != null)
                        {
                            foreach (XElement rankInfo in rankinfoList)
                            {
                                ServiceQoSInfoForSummary performanceInfo = new ServiceQoSInfoForSummary();
                                string rankvalue = rankInfo.Attribute(RankValue_String).Value;
                                string serviceType = rankInfo.Attribute(ServiceType_String).Value;
                                string url = rankInfo.Attribute(URL_String).Value;
                                string fgdcScore = rankInfo.Attribute(FGDCScore_String).Value;

                                performanceInfo.rankValue = double.Parse(rankvalue);
                                performanceInfo.fgdcStatusCheckerScore = double.Parse(fgdcScore);
                                performanceInfo.serviceURL = url;
                                performanceInfo.serviceType = serviceType;

                                servicePerformanceInfoList.Add(performanceInfo);
                            }
                        }
                    }
                    else
                        return null;
                }
            }
            else
                return null;

            return servicePerformanceInfoList;
        }

        [OperationContract]
        public ServiceQoSInfoForHistory getServiceQoSHistoricalInfo(List<ServiceInfoForHistory> serviceInfoList)
        {
            XDocument doc1 = new XDocument();
            XElement Envelope = new XElement(XName.Get("Envelope", soapenv_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsd", xsd_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "soapenv", soapenv_namespace));

            XElement Body = new XElement(XName.Get("Body", soapenv_namespace));
            XElement GetServicePerforDetail = new XElement(XName.Get(GetServicePerforDetail_String, CISCChecker_namespace));
            GetServicePerforDetail.Add(new XAttribute(XNamespace.Xmlns + "ns", CISCChecker_namespace));

            ServiceQoSInfoForHistory performanceInfo = null;
            foreach (ServiceInfoForHistory serviceInfo in serviceInfoList)
            {
                string url = serviceInfo.serviceURL;
                string serviceType = serviceInfo.serviceType;
                string startdate = serviceInfo.startdate;
                string enddate = serviceInfo.enddate;
                if (serviceType.Equals(SearchingContent.resourceTypeValue_CLH_Services))
                    serviceType = SearchingContent.ServiceType_WMS;

                if (url != null && !url.Trim().Equals("") && serviceType != null && !serviceType.Trim().Equals("")
                    && startdate != null && !startdate.Trim().Equals("")
                    && enddate != null && !enddate.Trim().Equals(""))
                {
                    //XElement ServiceInfo = new XElement(XName.Get("ServiceInfo", CISCChecker_namespace));
                    XElement ServiceInfoWithDate = new XElement(ServiceInfoWithDate_String);
                    ServiceInfoWithDate.SetAttributeValue(URL_String, url);
                    ServiceInfoWithDate.SetAttributeValue(ServiceType_String, serviceType);
                    ServiceInfoWithDate.SetAttributeValue(FromDate_String, startdate);
                    ServiceInfoWithDate.SetAttributeValue(ToDate_String, enddate);
                    GetServicePerforDetail.Add(ServiceInfoWithDate);
                }
            }
            Body.Add(GetServicePerforDetail);
            Envelope.Add(Body);
            doc1.Add(Envelope);

            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(ServicePerformanceCheckerURL, postRequest);
            if (resultString == null || resultString.Trim().Equals(""))
            {
                return null;
            }
            XDocument doc = XDocument.Parse(resultString);
            if (doc != null)
            {
                XElement rootElement = doc.Root;
                if (!rootElement.Name.LocalName.Equals("Envelope"))
                {
                    return null;
                }

                XElement responeBody = rootElement.Element(XName.Get("Body", soapenv_namespace));
                if (responeBody != null)
                {
                    XElement GetServicePerforDetailResponse = responeBody.Element(XName.Get(GetServicePerforDetailResponse_String, CISCChecker_namespace));
                    if (GetServicePerforDetailResponse != null)
                    {
                        XElement ServiceDetailPerforInfo = GetServicePerforDetailResponse.Element(ServiceDetailInfo_String);
                        if (ServiceDetailPerforInfo != null)
                        {
                            IEnumerable<XElement> DailyPerfor = ServiceDetailPerforInfo.Elements(DailyPerfor_String);
                            if (DailyPerfor != null && DailyPerfor.Count() > 0)
                            {
                                performanceInfo = new ServiceQoSInfoForHistory();
                                performanceInfo.serviceURL = ServiceDetailPerforInfo.Attribute(URL_String).Value;
                                performanceInfo.serviceType = ServiceDetailPerforInfo.Attribute(ServiceType_String).Value;
                                performanceInfo.startDateTime = DateTime.Parse(ServiceDetailPerforInfo.Attribute(StartDate_String).Value);
                                performanceInfo.endDateTime = DateTime.Parse(ServiceDetailPerforInfo.Attribute(EndDate_String).Value);

                                performanceInfo.measurementInfoList = new List<ServiceQoSInfoInOneMeasurement>();

                                foreach (XElement rankInfo in DailyPerfor)
                                {
                                    ServiceQoSInfoInOneMeasurement measurement = new ServiceQoSInfoInOneMeasurement();
                                    string measureDateTime = rankInfo.Attribute(measureDate_String).Value;
                                    string responseTime = rankInfo.Attribute(responseTime_String).Value;
                                    string fgdcScore = rankInfo.Attribute(FGDCScore_String).Value;
                                    measurement.responseTime = double.Parse(responseTime);
                                    measurement.FGDCScroe = double.Parse(fgdcScore);
                                    measurement.measureDateTime = DateTime.Parse(measureDateTime.Replace(" ", "T"));
                                    performanceInfo.measurementInfoList.Add(measurement);
                                }
                            }
                        }
                    }
                    else
                        return null;
                }
            }
            else
                return null;

            return performanceInfo;
        }

        [OperationContract]
        public WMSLayerInfoWithQoS getWMSLayersQoSSummaryInfo(ServiceInfoForSummary serviceInfo)
        {
            XDocument doc1 = new XDocument();
            XElement Envelope = new XElement(XName.Get("Envelope", soapenv_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsd", xsd_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "soapenv", soapenv_namespace));
            XElement Body = new XElement(XName.Get("Body", soapenv_namespace));
            XElement GetLayerPerforRank = new XElement(XName.Get(GetLayerPerforRank_String, CISCChecker_namespace));
            GetLayerPerforRank.Add(new XAttribute(XNamespace.Xmlns + "ns", CISCChecker_namespace));
            string url = serviceInfo.serviceURL;
            string serviceType = serviceInfo.serviceType;
            if (serviceType.Equals(SearchingContent.resourceTypeValue_CLH_Services))
                serviceType = SearchingContent.ServiceType_WMS;

            if (url != null && !url.Trim().Equals("") && serviceType != null && !serviceType.Trim().Equals(""))
            {
                //XElement ServiceInfo = new XElement(XName.Get("ServiceInfo", CISCChecker_namespace));
                XElement ServiceInfo = new XElement(ServiceInfo_String);
                ServiceInfo.SetAttributeValue(URL_String, url);
                ServiceInfo.SetAttributeValue(ServiceType_String, serviceType);
                GetLayerPerforRank.Add(ServiceInfo);
            }
            Body.Add(GetLayerPerforRank);
            Envelope.Add(Body);
            doc1.Add(Envelope);

            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(ServicePerformanceCheckerURL, postRequest);
            if (resultString == null || resultString.Trim().Equals(""))
            {
                return null;
            }
            WMSLayerInfoWithQoS wmsLayerInfoWithQoS = null;
            XDocument doc = XDocument.Parse(resultString);
            if (doc != null)
            {
                XElement rootElement = doc.Root;
                if (!rootElement.Name.LocalName.Equals("Envelope"))
                {
                    return null;
                }

                XElement responeBody = rootElement.Element(XName.Get("Body", soapenv_namespace));
                if (responeBody != null)
                {

                    XElement GetLayerPerforRankResponse = responeBody.Element(XName.Get(GetLayerPerforRankResponse_String, CISCChecker_namespace));
                    if (GetLayerPerforRankResponse != null)
                    {
                        XElement LayerPerforRank = GetLayerPerforRankResponse.Element(LayerRankInfo_String);
                        if (LayerPerforRank != null)
                        {
                            wmsLayerInfoWithQoS = new WMSLayerInfoWithQoS();
                            wmsLayerInfoWithQoS.serviceURL = LayerPerforRank.Attribute(URL_String).Value;
                            wmsLayerInfoWithQoS.serviceType = LayerPerforRank.Attribute(ServiceType_String).Value;
                            IEnumerable<XElement> rankinfoList = LayerPerforRank.Elements(Layer_String);
                            if (rankinfoList != null)
                            {
                                List<WMSLayer> WMSLayersList = new List<WMSLayer>();
                                //wmsLayerInfoWithQoS.WMSLayersList = WMSLayersList;
                                foreach (XElement rankInfo in rankinfoList)
                                {
                                    WMSLayer wmsLayer = new WMSLayer();
                                    wmsLayer.name = rankInfo.Attribute(LayerName_String).Value;
                                    wmsLayer.title = rankInfo.Attribute(LayerTitle_String).Value;
                                    string rankvalue = rankInfo.Attribute(RankValue_String).Value;
                                    string responseTime = rankInfo.Attribute(responseTime_String).Value;
                                    wmsLayer.rankValue = double.Parse(rankvalue);
                                    wmsLayer.responseTime = double.Parse(responseTime);
                                    WMSLayersList.Add(wmsLayer);
                                }
                                if (WMSLayersList.Count > 0)
                                    wmsLayerInfoWithQoS.WMSLayersList = WMSLayersList;
                            }
                        }
                    }
                    else
                        return null;
                }
            }
            else
                return null;

            return wmsLayerInfoWithQoS;
        }

        [OperationContract]
        public LayerQoSInfoForHistory getWMSLayersQoSHistoricalInfo(LayerInfoForHistory layerInfoForHistory)
        {
            XDocument doc1 = new XDocument();
            XElement Envelope = new XElement(XName.Get("Envelope", soapenv_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsd", xsd_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi_namespace));
            Envelope.Add(new XAttribute(XNamespace.Xmlns + "soapenv", soapenv_namespace));
            XElement Body = new XElement(XName.Get("Body", soapenv_namespace));

            XElement GetLayerPerforDetail = new XElement(XName.Get(GetLayerPerforDetail_String, CISCChecker_namespace));
            GetLayerPerforDetail.Add(new XAttribute(XNamespace.Xmlns + "ns", CISCChecker_namespace));

            LayerQoSInfoForHistory performanceInfo = null;
            {
                string url = layerInfoForHistory.serviceURL;
                string serviceType = layerInfoForHistory.serviceType;
                string startdate = layerInfoForHistory.startdate;
                string enddate = layerInfoForHistory.enddate;
                string layerName = layerInfoForHistory.layerName;
                if (serviceType.Equals(SearchingContent.resourceTypeValue_CLH_Services))
                    serviceType = SearchingContent.ServiceType_WMS;

                if (url != null && !url.Trim().Equals("") && serviceType != null && !serviceType.Trim().Equals("")
                    && startdate != null && !startdate.Trim().Equals("")
                    && enddate != null && !enddate.Trim().Equals("") && layerName != null && !layerName.Trim().Equals(""))
                {
                    XElement LayerInfoWithDate = new XElement(LayerInfoWithDate_String);
                    LayerInfoWithDate.SetAttributeValue(URL_String, url);
                    LayerInfoWithDate.SetAttributeValue(ServiceType_String, serviceType);
                    LayerInfoWithDate.SetAttributeValue(FromDate_String, startdate);
                    LayerInfoWithDate.SetAttributeValue(ToDate_String, enddate);
                    LayerInfoWithDate.SetAttributeValue(LayerName_String, layerName);
                    GetLayerPerforDetail.Add(LayerInfoWithDate);
                }
            }
            Body.Add(GetLayerPerforDetail);
            Envelope.Add(Body);
            doc1.Add(Envelope);

            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(ServicePerformanceCheckerURL, postRequest);
            if (resultString == null || resultString.Trim().Equals(""))
            {
                return null;
            }
            XDocument doc = XDocument.Parse(resultString);
            if (doc != null)
            {
                XElement rootElement = doc.Root;
                if (!rootElement.Name.LocalName.Equals("Envelope"))
                {
                    return null;
                }

                XElement responeBody = rootElement.Element(XName.Get("Body", soapenv_namespace));
                if (responeBody != null)
                {
                    XElement GetLayerPerforDetailResponse = responeBody.Element(XName.Get(GetLayerPerforDetailResponse_String, CISCChecker_namespace));
                    if (GetLayerPerforDetailResponse != null)
                    {
                        XElement LayerPerforInfoDetail = GetLayerPerforDetailResponse.Element(LayerDetailInfo_String);
                        if (LayerPerforInfoDetail != null)
                        {
                            IEnumerable<XElement> DailyPerforList = LayerPerforInfoDetail.Elements(DailyPerfor_String);
                            if (DailyPerforList != null && DailyPerforList.Count() > 0)
                            {
                                performanceInfo = new LayerQoSInfoForHistory();
                                performanceInfo.serviceURL = LayerPerforInfoDetail.Attribute(URL_String).Value;
                                //performanceInfo.layerName = LayerPerforInfoDetail.Attribute(LayerName_String).Value;
                                performanceInfo.serviceType = LayerPerforInfoDetail.Attribute(ServiceType_String).Value;
                                performanceInfo.startDateTime = DateTime.Parse(LayerPerforInfoDetail.Attribute(StartDate_String).Value);
                                performanceInfo.endDateTime = DateTime.Parse(LayerPerforInfoDetail.Attribute(EndDate_String).Value);
                                performanceInfo.measurementInfoList = new List<LayerQoSInfoInOneMeasurement>();

                                foreach (XElement rankInfo in DailyPerforList)
                                {
                                    LayerQoSInfoInOneMeasurement measurement = new LayerQoSInfoInOneMeasurement();
                                    string measureDateTime = rankInfo.Attribute(measureDate_String).Value;
                                    string responseTime = rankInfo.Attribute(responseTime_String).Value;

                                    measurement.responseTime = double.Parse(responseTime);
                                    measurement.measureDateTime = DateTime.Parse(measureDateTime.Replace(" ", "T"));
                                    performanceInfo.measurementInfoList.Add(measurement);
                                }
                            }
                        }
                    }
                    else
                        return null;
                }
            }
            else
                return null;
            return performanceInfo;
        }
    }
}
