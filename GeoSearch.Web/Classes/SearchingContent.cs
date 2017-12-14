using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Lib for build a CSW search

namespace GeoSearch.Web
{
    public class SearchingContent
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


        //public const string resourceType_Service = "service";
        //public const string resourceType_GOS_GEOService = "geographicService";
        //public const string resourceType_Dataset = "dataset";
        //public const string resourceType_Model = "model";
        //public const string resourceType_CollectionSession = "collectionSession";
        //public const string resourceType_Application = "application";
        //public const string resourceType_Document = "document";
        //public const string resourceType_Video = "video";
        //public const string resourceType_GOS_MapFiles = "mapFiles";
        //public const string resourceType_GOS_StaticMapImage = "staticMapImage";  

        public const string ServiceType_WMS = "OGC:WMS";
        public const string ServiceType_WFS = "OGC:WFS";
        public const string ServiceType_WCS = "OGC:WCS";
        public const string ServiceType_WPS = "OGC:WPS";
        public const string ServiceType_CSW = "OGC:CSW";

        public const string resourceType_PickID_All = "All";
        public const string resourceType_PickID_AllServices = "AllServices";
        //public const string resourceType_PickID_AllAnalysisAndVisualizationServices = "AllAnalysisAndVisualizationServices";

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

        public const string resourceIdentificationKeywords_GEOSSDataCORE_ONLY = "only";
        public const string resourceIdentificationKeywords_GEOSSDataCORE_NON = "non";
        public const string resourceIdentificationKeywords_GEOSSDataCORE_BOTH = "both";
     
        public List<string> wordsInAnyText { get; set; }
        public List<string> wordsInTitle { get; set; }
        public List<string> wordsInAbstract { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> withoutWordsInAnytext { get; set; }
        public string exactPhrase { get; set; }

        //public bool isAllResourceTypes { get; set; }
        public bool isNoResourceTypeSelected { get; set; }
        //public List<string> resourceTypes { get; set; }
        public ResourceType resourceTypesTree { get; set; }

        public bool where_isAnyWhere { get; set; }
        public string where_North { get; set; }
        public string where_South { get; set; }
        public string where_East { get; set; }
        public string where_West { get; set; }
        public string where_Relationship { get; set; }
        public string where_Region { get; set; }

        public bool when_isAnytime { get; set; }
        public string when_TimeType { get; set; }
        public string when_FromTime { get; set; }
        public string when_ToTime { get; set; }

        public string resourceIdentificationKeywords_GEOSSDataCORE { get; set; }

        public static string getSearchContentString(SearchingContent searchingContent)
        {
            return "";
        }
    }
}