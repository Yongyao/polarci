using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using GeoSearch.CSWQueryServiceReference;
using System.Collections.Generic;

namespace GeoSearch
{
    public class Converters
    {
        public static string getQoSBarImagePathString(double quality)
        {
            string path = "/GeoSearch;component/images/QoS/0bars.png";
            if (quality >= 0 && quality < 6)
            {
                int next = (int)quality;
                path = "/GeoSearch;component/images/QoS/" + next + "bars.png";
            }
            return path;
        }
    }

    public class OpenLayersButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if ((!type.ToLower().Equals(ConstantCollection.ServiceType_WFS)) && (!type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization)) && (!type.Equals(ConstantCollection.ServiceType_WMS)))
                    v = Visibility.Collapsed;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BingMapsButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if ((!type.ToLower().Equals(ConstantCollection.ServiceType_WFS)) && (!type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization)) && (!type.Equals(ConstantCollection.ServiceType_WMS)))
                    v = Visibility.Collapsed;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class OtherVisualizationViewerButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if ((!type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization)) && (!type.Equals(ConstantCollection.ServiceType_WMS)))
                    v = Visibility.Collapsed;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class SWPViewerButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if ((!type.Equals(ConstantCollection.ServiceType_WFS)) && (!type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization)) && (!type.Equals(ConstantCollection.ServiceType_WMS)))
                    v = Visibility.Collapsed;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GetDataButtonVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if ((type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_DownloadableData)))
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

    public class ResourceTypeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = "/GeoSearch;component/images/resourceTypes/dataset1.png";

            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                //services
                if (type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AnalysisAndVisualization)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_AnalysisAndVisualization)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.ServiceType_WMS))
                    source = "/GeoSearch;component/images/resourceTypes/WMS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WFS))
                    source = "/GeoSearch;component/images/resourceTypes/WFS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WCS))
                    source = "/GeoSearch;component/images/resourceTypes/WCS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WPS))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.ServiceType_CSW))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_AlertsRSSAndInformationFeeds)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_AlertsRSSAndInformationFeeds)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_DataServices_AlertsRSSAndInformationFeeds))
                    source = "/GeoSearch;component/images/resourceTypes/RSS.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_DataServices_CatalogRegistryOrMetadataCollection)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_DataServices_CatalogRegistryOrMetadataCollection)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection))
                    source = "/GeoSearch;component/images/resourceTypes/CatalogServer.png";

                //datasets
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_Datasets) || type.Equals(ConstantCollection.resourceTypeValue_CLH_Datasets_nonGeographic)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_Datasets) || type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_OfflineData))
                    source = "/GeoSearch;component/images/resourceTypes/dataset1.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_LiveData))
                    source = "/GeoSearch;component/images/resourceTypes/globe.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_DownloadableData))
                    source = "/GeoSearch;component/images/resourceTypes/downloadableData.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_StaticMapImage))
                    source = "/GeoSearch;component/images/resourceTypes/dataset.png";
                else if (type.Equals(ConstantCollection.resourceTypeValue_GOS_Datasets_MapFiles))
                    source = "/GeoSearch;component/images/resourceTypes/map.png";

                //Initiatives
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_Initiatives) || type.Equals(ConstantCollection.resourceTypeValue_CSR_Initiatives)
                    || type.Equals(ConstantCollection.resourceTypeValue_GOS_Initiatives))
                    source = "/GeoSearch;component/images/resourceTypes/Initiatives.png";

                //MonitoringAndObservationSystems
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_MonitoringAndObservationSystems) || type.Equals(ConstantCollection.resourceTypeValue_GOS_MonitoringAndObservationSystems)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_MonitoringAndObservationSystems))
                    source = "/GeoSearch;component/images/resourceTypes/exec.png";

                //ComputationalModel
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_ComputationalModel) || type.Equals(ConstantCollection.resourceTypeValue_GOS_ComputationalModel)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_ComputationalModel))
                    source = "/GeoSearch;component/images/resourceTypes/models.png";

                //WebsitesAndDocuments
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_WebsitesAndDocuments) || type.Equals(ConstantCollection.resourceTypeValue_GOS_WebsitesAndDocuments)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_WebsitesAndDocuments))
                    source = "/GeoSearch;component/images/resourceTypes/WebsitesAndDocuments.png";

                //SoftwareAndApplications
                else if (type.Equals(ConstantCollection.resourceTypeValue_CLH_SoftwareAndApplications) || type.Equals(ConstantCollection.resourceTypeValue_GOS_SoftwareAndApplications)
                    || type.Equals(ConstantCollection.resourceTypeValue_CSR_SoftwareAndApplications))
                    source = "/GeoSearch;component/images/resourceTypes/applications.png";


                else if (type.ToLower().Equals("directories"))
                    source = "/GeoSearch;component/images/resourceTypes/Folder.png";
                else if (type.ToLower().Equals("pdf"))
                    source = "/GeoSearch;component/images/resourceTypes/pdf.png";
                else if (type.ToLower().Equals("photo"))
                    source = "/GeoSearch;component/images/resourceTypes/photo.png";
                else if (type.ToLower().Equals("layer"))
                    source = "/GeoSearch;component/images/resourceTypes/layer.png";
                else if (type.ToLower().Equals("photo"))
                    source = "/GeoSearch;component/images/resourceTypes/photo.png";
            }
            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GeneralResourceTypeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = null;
            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if (type.Equals(ConstantCollection.resourceType_PickID_AllServices))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.resourceType_DataServices_AnalysisAndVisualization))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.ServiceType_WMS))
                    source = "/GeoSearch;component/images/resourceTypes/WMS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WCS))
                    source = "/GeoSearch;component/images/resourceTypes/WCS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WFS))
                    source = "/GeoSearch;component/images/resourceTypes/WFS.png";
                else if (type.Equals(ConstantCollection.ServiceType_WPS))
                    source = "/GeoSearch;component/images/resourceTypes/network_Service.png";
                else if (type.Equals(ConstantCollection.resourceType_DataServices_AlertsRSSAndInformationFeeds))
                    source = "/GeoSearch;component/images/resourceTypes/RSS.png";
                else if (type.Equals(ConstantCollection.resourceType_DataServices_CatalogRegistryOrMetadataCollection))
                    source = "/GeoSearch;component/images/resourceTypes/CatalogServer.png";
                else if (type.Equals(ConstantCollection.resourceType_Datasets))
                    source = "/GeoSearch;component/images/resourceTypes/globe.png";
                else if (type.Equals(ConstantCollection.resourceType_MonitoringAndObservationSystems))
                    source = "/GeoSearch;component/images/resourceTypes/exec.png";
                else if (type.Equals(ConstantCollection.resourceType_ComputationalModel))
                    source = "/GeoSearch;component/images/resourceTypes/models.png";
                else if (type.Equals(ConstantCollection.resourceType_Initiatives))
                    source = "/GeoSearch;component/images/resourceTypes/Initiatives.png";
                else if (type.Equals(ConstantCollection.resourceType_WebsitesAndDocuments))
                    source = "/GeoSearch;component/images/resourceTypes/WebsitesAndDocuments.png";
                else if (type.Equals(ConstantCollection.resourceType_SoftwareAndApplications))
                    source = "/GeoSearch;component/images/resourceTypes/applications.png";
            }
            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class RelevanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double relevance = (double)value;
            string result = relevance * 100 + "%";
            return result;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class QoSIconTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = "/GeoSearch;component/images/QoS/5bars.png";
            if (value != null && value.GetType() == typeof(double))
            {
                double quality = (double)value;
                source = Converters.getQoSBarImagePathString(quality);
            }
            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class QoSIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            if (value != null && value.GetType() == typeof(double))
            {
                double quality = (double)value;
                if (quality >= 0)
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

    public class ProviderIconVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            string provider = value as string;
            provider = provider.ToLower();
            if (provider != null && !provider.Trim().Equals(""))
            {
                if (provider.Contains("usgs") || provider.Contains("u.s. geological survey"))
                    v = Visibility.Visible;
                else if (provider.Contains("nasa") || provider.Contains("national aeronautics and space administration"))
                    v = Visibility.Visible;
                else if (provider.Contains("jpl") || provider.Contains("jet propulsion laboratory"))
                    v = Visibility.Visible;
                else if (provider.ToUpper().Contains("GCMD") || provider.ToLower().Contains("global change master directory"))
                    v = Visibility.Visible;
                else if (provider.Contains("university of montana"))
                    v = Visibility.Visible;
                else if (provider.Contains("noaa") || provider.Contains("national oceanic and atmospheric administration"))
                    v = Visibility.Visible;
                else if (provider.Contains("esri") || provider.Contains("economic and social research institute"))
                    v = Visibility.Visible;
                //else
                //    v = Visibility.Visible;
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ProviderIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = "/GeoSearch;component/images/Organizer/NASA.png";
            string provider = value as string;
            string OrganizerName = "NASA";
            provider = provider.ToLower();
            if (provider.Contains("usgs") || provider.Contains("u.s. geological survey"))
                OrganizerName = "USGS";
            else if (provider.Contains("nasa") || provider.Contains("national aeronautics and space administration"))
                OrganizerName = "NASA";
            else if (provider.Contains("jpl") || provider.Contains("jet propulsion laboratory"))
                OrganizerName = "NASA";
            else if (provider.ToUpper().Contains("GCMD") || provider.ToLower().Contains("global change master directory"))
                OrganizerName = "NASA";
            else if (provider.Contains("university of montana"))
                OrganizerName = "MU";
            else if (provider.Contains("noaa") || provider.Contains("national oceanic and atmospheric administration"))
                OrganizerName = "NOAA";
            else if (provider.Contains("esri") || provider.Contains("economic and social research institute"))
                OrganizerName = "ESRI";

            source = "/GeoSearch;component/images/Organizer/" + OrganizerName + ".png";

            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class KeywordsShowingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = "";
            //ObservableCollection<string> DescriptiveKeywords = value as ObservableCollection<string>;
            StringList DescriptiveKeywords = value as StringList;
            if (DescriptiveKeywords != null && DescriptiveKeywords.Count > 0)
            {
                foreach (string keyword in DescriptiveKeywords)
                {
                    {
                        source += (keyword + "; ");
                    }
                }
            }
            else
                source = "none";
            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GetDataButtonTextContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = "Get Data";
            string accessURL = value as string;
            int index0 = accessURL.LastIndexOf("/");
            if (index0 > 0)
            {
                string header = accessURL.Substring(0, index0);
                if (header.ToLower().Equals("http:/") || header.ToLower().Equals("ftp:/"))
                    return text;
                accessURL = accessURL.Substring(index0 + 1);
                int index = accessURL.LastIndexOf(".");
                if (index > 0)
                {
                    string extention = accessURL.Substring(index + 1).ToLower();
                    if (extention.IndexOf("?") < 0)
                    {
                        if ((!extention.Equals("html")) && (!extention.Equals("htm")) && (!extention.Equals("asp")) && (!extention.Equals("aspx")) && (!extention.Equals("jsp")) && (!extention.Equals("php")))
                            text = "Download Data";
                    }
                }
            }
            return text;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GESSDataCOREIconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            //Visibility v = Visibility.Visible;
            bool provider = (bool)value;
            if (provider == true)
            {
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


    //------------------------------------For Pivot Viewer---------------------------------

    public class ProviderNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string generalProvider = "Others";
            string provider = value as string;
            provider = provider.ToLower();
            if (provider != null && !provider.Trim().Equals(""))
            {
                if (provider.Contains("usgs") || provider.Contains("u.s. geological survey"))
                    generalProvider = "USGS";
                else if (provider.Contains("nasa") || provider.Contains("national aeronautics and space administration"))
                    generalProvider = "NASA";
                else if (provider.Contains("jpl") || provider.Contains("jet propulsion laboratory"))
                    generalProvider = "NASA";
                else if (provider.ToUpper().Contains("GCMD") || provider.ToLower().Contains("global change master directory"))
                    generalProvider = "NASA";
                else if (provider.Contains("university"))
                    generalProvider = "Universities";
                else if (provider.Contains("noaa") || provider.Contains("national oceanic and atmospheric administration"))
                    generalProvider = "NOAA";
                else if (provider.Contains("esri") || provider.Contains("economic and social research institute"))
                    generalProvider = "ESRI";
                else if (provider.Contains("unknown"))
                    generalProvider = "Unknown";
                else
                    generalProvider = "Others";
            }
            return generalProvider;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class GeoExtensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string extension = "Global";
            BBox bbox = value as BBox;
            if (bbox != null)
                extension = "Global";
            else
                extension = "Local";
            return extension;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ServerLocationDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string locationDescription = null;
            if (value != null && !((string)value).Trim().Equals(""))
            {
                string[] location = ((string)value).Split(',');
                locationDescription = "Unknown";
                try
                {
                    double lon = double.Parse(location[0]);
                    double lat = double.Parse(location[1]);
                    if (lon >= 0 && lat >= 0)
                        locationDescription = "North_East";
                    else if (lon >= 0 && lat <= 0)
                        locationDescription = "South_East";
                    else if (lon <= 0 && lat <= 0)
                        locationDescription = "South_West";
                    else if (lon <= 0 && lat >= 0)
                        locationDescription = "North_West";
                }
                catch (Exception e)
                {
                    e.ToString();
                }
            }
            return locationDescription;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class DescriptiveKeywordsStringListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string keywords = "Keywords: ";
            //ObservableCollection<String> DescriptiveKeywords = value as ObservableCollection<String>;
            StringList DescriptiveKeywords = value as StringList;
            if (DescriptiveKeywords != null && DescriptiveKeywords.Count > 0)
            {
                foreach (string keyword in DescriptiveKeywords)
                {
                    {
                        keywords += (keyword + "; ");
                    }
                }
            }
            else
                keywords = keywords+"none";
            return keywords;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DescriptiveKeywordsVisibilityOnInformationCardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            //ObservableCollection<String> DescriptiveKeywords = value as ObservableCollection<String>;
            StringList DescriptiveKeywords = value as StringList;
            if (DescriptiveKeywords != null && DescriptiveKeywords.Count>0)
            {
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

    public class AbstractStringOnInformationCardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string a = "Abstract: ";
            string abstractValue = value as string;
                a += abstractValue;
            return a;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class AbstractStringVisibilityOnInformationCardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;
            string abstractValue = value as string;
            if (abstractValue != null && !abstractValue.Trim().Equals("") && !abstractValue.Trim().Equals("unknown"))
            {
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



    public class SBA_Health_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count> 0 && SBAs.Contains(SBAVocabularyTree.SBA_Health))
            {
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

    public class SBA_Disasters_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Disasters))
            {
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

    public class SBA_Energy_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Energy))
            {
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

    public class SBA_Climate_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Climate))
            {
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

    public class SBA_Water_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Water))
            {
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

    public class SBA_Weather_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Weather))
            {
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

    public class SBA_Ecosystems_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Ecosystems))
            {
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

    public class SBA_Agriculture_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Agriculture))
            {
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

    public class SBA_Biodiversity_IconVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Collapsed;

            //ObservableCollection<string> SBAs = (ObservableCollection<string>)value;
            StringList SBAs = value as StringList;
            if (SBAs != null && SBAs.Count > 0 && SBAs.Contains(SBAVocabularyTree.SBA_Biodiversity))
            {
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

    public class SBAVocabularyIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string source = null;
            if (value != null && value.GetType() == typeof(string))
            {
                string type = (string)value;
                if (type.Equals(SBAVocabularyTree.SBA_Disasters))
                    source = "/GeoSearch;component/images/SBA/icon_disaster_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Health))
                    source = "/GeoSearch;component/images/SBA/icon_health_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Energy))
                    source = "/GeoSearch;component/images/SBA/icon_energy_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Climate))
                    source = "/GeoSearch;component/images/SBA/icon_climate_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Water))
                    source = "/GeoSearch;component/images/SBA/icon_water_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Weather))
                    source = "/GeoSearch;component/images/SBA/icon_weather_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Ecosystems))
                    source = "/GeoSearch;component/images/SBA/icon_ecosystem_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Agriculture))
                    source = "/GeoSearch;component/images/SBA/icon_agriculture_off.png";
                else if (type.Equals(SBAVocabularyTree.SBA_Biodiversity))
                    source = "/GeoSearch;component/images/SBA/icon_biodiver_off.png";
                else
                    source = "/GeoSearch;component/images/SBA/subtype.png";

            }
            return source;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    } 
}
