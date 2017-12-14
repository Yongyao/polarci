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

namespace GeoSearch
{
    public class ConstantCollection
    {
        public const string timeType_MetedataChangeDate = "Metedata_Change_Date";
        public const string timeType_TemporalExtent = "TemporalExtent";

        public const string where_Relationship_BBOX = "BBOX";
        public const string where_Relationship_Equals = "Equals";
        public const string where_Relationship_Disjoint = "Disjoint";
        public const string where_Relationship_Intersects = "Intersects";
        public const string where_Relationship_Touches = "Touches";
        public const string where_Relationship_Crosses = "Crosses";
        public const string where_Relationship_Within = "Within";
        public const string where_Relationship_Contains = "Contains";
        public const string where_Relationship_Overlaps = "Overlaps";
        public const string where_Relationship_Beyond = "Beyond";
        public const string where_Relationship_DWithin = "DWithin";

        public const string when_Type_TemporalExtent = "TempExtent";
        public const string when_Type_MetadataChangeDate = "MetadataChangeDate";

        public const string ServiceType_WMS = "OGC:WMS";
        public const string ServiceType_WFS = "OGC:WFS";
        public const string ServiceType_WCS = "OGC:WCS";
        public const string ServiceType_WPS = "OGC:WPS";
        public const string ServiceType_CSW = "OGC:CSW";

        public const string resourceType_PickID_All = "All";
        public const string resourceType_PickID_AllServices = "AllServices";

        public const string resourceGeneralType_PivotViewer_Datasets = "Datasets";
        public const string resourceGeneralType_PivotViewer_MonitoringAndObservationSystems = "Monitoring & Observation Systems";
        public const string resourceGeneralType_PivotViewer_ComputationalModel = "Computational Model";
        public const string resourceGeneralType_PivotViewer_Initiatives = "Initiatives";
        public const string resourceGeneralType_WebsitesAndDocuments = "Websites & Documents";
        public const string resourceGeneralType_WebServices = "Web Services";
        public const string resourceGeneralType_SoftwareAndApplications = "Software & Applications";

        public const string resourceType_Datasets = "Datasets";
        public const string resourceType_MonitoringAndObservationSystems = "MonitoringAndObservationSystems";
        public const string resourceType_ComputationalModel = "ComputationalModel";
        public const string resourceType_Initiatives = "Initiatives";
        public const string resourceType_WebsitesAndDocuments = "WebsitesAndDocuments";
        public const string resourceType_DataServices_AnalysisAndVisualization = "DataServices_AnalysisAndVisualization";
        public const string resourceType_DataServices_AlertsRSSAndInformationFeeds = "DataServices_AlertsRSSAndInformationFeeds";
        public const string resourceType_DataServices_CatalogRegistryOrMetadataCollection = "DataServices_CatalogRegistryOrMetadataCollection";
        public const string resourceType_SoftwareAndApplications = "SoftwareAndApplications";

        public const string resourceTypeValue_CSR_Datasets = "datasets";
        public const string resourceTypeValue_CSR_MonitoringAndObservationSystems = "observingSystemOrSensorNetwork";
        public const string resourceTypeValue_CSR_ComputationalModel = "computationModel";
        public const string resourceTypeValue_CSR_Initiatives = "Initiatives";
        public const string resourceTypeValue_CSR_WebsitesAndDocuments = "websitesDocuments";
        public const string resourceTypeValue_CSR_DataServices_AnalysisAndVisualization = "AnalysisAndVisualization";
        public const string resourceTypeValue_CSR_DataServices_AlertsRSSAndInformationFeeds = "feedRSSAlert";
        public const string resourceTypeValue_CSR_DataServices_CatalogRegistryOrMetadataCollection = "catalogRegistryMetadataCollection";
        public const string resourceTypeValue_CSR_SoftwareAndApplications = "softwareOrApplication";

        public const string resourceTypeValue_CLH_Datasets = "dataset";
        public const string resourceTypeValue_CLH_Datasets_nonGeographic = "nonGeographicDataset";
        public const string resourceTypeValue_CLH_MonitoringAndObservationSystems = "";
        public const string resourceTypeValue_CLH_ComputationalModel = "model";
        public const string resourceTypeValue_CLH_Initiatives = "collectionSession";
        public const string resourceTypeValue_CLH_WebsitesAndDocuments = "document";
        public const string resourceTypeValue_CLH_Services = "service";
        public const string resourceTypeValue_CLH_DataServices_AnalysisAndVisualization = "service";
        public const string resourceTypeValue_CLH_DataServices_AlertsRSSAndInformationFeeds = "";
        public const string resourceTypeValue_CLH_DataServices_CatalogRegistryOrMetadataCollection = "service";
        public const string resourceTypeValue_CLH_SoftwareAndApplications = "application";

        public const string resourceTypeValue_GOS_Datasets = "*data*";
        public const string resourceTypeValue_GOS_Datasets_StaticMapImage = "staticMapImage";
        public const string resourceTypeValue_GOS_Datasets_DownloadableData = "downloadableData";
        public const string resourceTypeValue_GOS_Datasets_OfflineData = "offlineData";
        public const string resourceTypeValue_GOS_Datasets_MapFiles = "mapFiles";
        public const string resourceTypeValue_GOS_Datasets_LiveData = "liveData";   
        public const string resourceTypeValue_GOS_MonitoringAndObservationSystems = "";
        public const string resourceTypeValue_GOS_ComputationalModel = "model";
        public const string resourceTypeValue_GOS_Initiatives = "geographicActivities";
        public const string resourceTypeValue_GOS_WebsitesAndDocuments = "document";
        public const string resourceTypeValue_GOS_DataServices_AnalysisAndVisualization = "geographicService";
        public const string resourceTypeValue_GOS_DataServices_AlertsRSSAndInformationFeeds = "";
        public const string resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection = "clearinghouse";
        public const string resourceTypeValue_GOS_SoftwareAndApplications = "application";



        public const string OGCServicePamameter_GetCapabilities = "getcapabilities";
        public const string OGCServicePamameter_GetMap = "getmap";
        public const string OGCServicePamameter_Request = "request";
        public const string OGCServicePamameter_Version = "version";
        public const string OGCServicePamameter_Service = "service";
        public const string OGCServicePamameter_Format = "format";
        public const string OGCServicePamameter_Updatesequence = "updatesequence";


        public const string resourceIdentificationKeywords_geossDateCore = "geossDataCore";
        public const string resourceIdentificationKeywords_GEOSSDataCORE_ONLY = "only";
        public const string resourceIdentificationKeywords_GEOSSDataCORE_NON = "non";
        public const string resourceIdentificationKeywords_GEOSSDataCORE_BOTH = "both";


        public const string SortBy_Default = "Default";
        public const string SortBy_Relevance = "Relevance";
        public const string SortBy_ResourceQuality = "QoS";
        public const string SortBy_Title = "Title";
        public const string SortBy_Relevance_Title = "Relevance and Title";
        public const string SortBy_Relevance_ResourceQuality = "Relevance and QoS";
        public const string SortBy_Source_Title = "Source and Title";
        public const string SortBy_DataCORE_Relevance = "Data-CORE and Relevance";

        public const string CLHCSWURLString = "http://clearinghouse.cisc.gmu.edu/geonetwork/srv/en/csw";
        //public const string CLHCSWURLString = "http://129.174.244.107/geonetwork/srv/en/csw";      
        //public const string CLHCSWURLString = "http://catalog.usgin.org/geoportal/csw";
        //public const string CLHCSWURLString = "http://ec2-184-73-35-62.compute-1.amazonaws.com:80/geonetwork/srv/en/csw";
        //public const string CLHCSWURLString = "http://ec2-75-101-156-185.compute-1.amazonaws.com/geonetwork/srv/en/csw";
        //public const string CLHCSWURLString = "http://ec2-50-19-223-225.compute-1.amazonaws.com/geonetwork/srv/en/csw"; 
        //public const string GOSCSWURLString = "http://catalog.geodata.gov/geoportal/csw/discovery";
        //"http://ec2-50-19-223-225.compute-1.amazonaws.com/geonetwork/srv/en/main.home"
        public const string GOSCSWURLString = "http://geo.data.gov/geoportal/csw"; 
        public const string CSRCSWURLString = "http://geossregistries.info:1090/GEOSSCSW202/discovery";
        public const string USGIN_AASG_CSWURLString = "http://catalog.usgin.org/geoportal/csw";
        public const string euroGEOSSString = "http://217.108.210.73/broker/services/cswiso";


        public const string WorldWind_WebVersion_URL = "http://wms.gmu.edu/worldwind/index.jsp?wmsURL=";
        //public const string OpenLayerView_URL = "http://ec2-204-236-246-78.compute-1.amazonaws.com/viewer/?";
        //public const string OpenLayerView_URL = "http://ec2-50-17-31-70.compute-1.amazonaws.com/viewer/?";
        public const string OpenLayerView_URL = "http://ec2-23-20-201-248.compute-1.amazonaws.com/viewer2/?";

        public const string SWPView_URL = "http://wms.gmu.edu/SWPViewer/bing7.jsp?";
        //public const string SWPView_URL = "http://129.174.119.35:8080/SWPViewer/bing7.jsp?";

        public const string searchStatus_InProcessing = "Processing";
        public const string searchStatus_Finished = "Finished";
        public const string searchStatus_Terminated = "Terminated";

        public static string firstTimeSearchNum = "50";
        public static int eachInvokeSearchNum_ExceptFirstTime = 300;
        public static string maxRecords = "1000";
        public static string startPosition = "1";

        //public const double recordsNumberInEachPage = 10;
        public const double ExpectedMaximumNum_PushpinOnBingMaps = 10;

        public static bool queryServicePerformanceScoreAtServerSide = true;
        public static bool calculateRelevanceAtServerSide = true;
        public static bool isQueryServicePerformanceScore = true;
        public static bool isCalculateRelevance = true;

        public static bool isQueryServerLocationLonLat = true;
        public static bool queryServerLocationLonLatAtServerSide = false;
    }
}
