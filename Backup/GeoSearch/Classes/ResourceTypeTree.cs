using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class ResourceTypeTree : INotifyPropertyChanged
    {     
        public string Name { get; set; }
        public string PictureURL { get; set; }
        public ObservableCollection<ResourceTypeTree> Children { get; set; }
        public ResourceTypeTree Parent { get; set; }
        public string ResourceTypeID { get; set; }
        public string Description { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelectedValue = false;
        public bool isSelected
        {
            get
            {
                return this.isSelectedValue;
            }

            set
            {
                if (value != this.isSelectedValue)
                {
                    this.isSelectedValue = value;
                    NotifyPropertyChanged("isSelected");
                }
            }
        }


        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public ResourceTypeTree(string name, string description, string pictureUrl, bool select, string id, ObservableCollection<ResourceTypeTree> children)
        {
            Name = name;
            PictureURL = pictureUrl;
            Children = children;
            isSelected = select;
            ResourceTypeID = id;
            Description = description;
            if (Children != null)
                foreach (ResourceTypeTree r in Children)
                {
                    r.Parent = this;
                }
        }

        public static ObservableCollection<ResourceTypeTree> getResourceTypeList()
        {
            ObservableCollection<ResourceTypeTree> list = new ObservableCollection<ResourceTypeTree>();
            ResourceTypeTree WMS = new ResourceTypeTree("WMS", "OGC Web Map Service (WMS)", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.ServiceType_WMS, null);
            ResourceTypeTree WFS = new ResourceTypeTree("WFS", "OGC Web Feature Service (WFS)", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.ServiceType_WFS, null);
            ResourceTypeTree WCS = new ResourceTypeTree("WCS", "OGC Web Coverage Service (WCS)", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.ServiceType_WCS, null);
            ResourceTypeTree WPS = new ResourceTypeTree("WPS", "OGC Web Processing Service (WPS)", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.ServiceType_WPS, null);
            ObservableCollection<ResourceTypeTree> list1 = new ObservableCollection<ResourceTypeTree>{ WMS, WFS, WCS, WPS };
            ResourceTypeTree AnalysisAndVisualization = new ResourceTypeTree("Analysis & Visualization", "Analysis and visualization", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_DataServices_AnalysisAndVisualization, list1);
            ResourceTypeTree AlertsRSSAndInformationFeeds = new ResourceTypeTree("Alerts & RSS & Information Feeds", "Alerts, RSS, and information Feeds", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_DataServices_AlertsRSSAndInformationFeeds, null);
            ResourceTypeTree CatalogRegistryOrMetadataCollection = new ResourceTypeTree("Catalogues & Inventories & Metadata Collections", "Catalogues, Inventories and metadata Collections", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_DataServices_CatalogRegistryOrMetadataCollection, null);

            ObservableCollection<ResourceTypeTree> list2 = new ObservableCollection<ResourceTypeTree>{ AnalysisAndVisualization, AlertsRSSAndInformationFeeds, CatalogRegistryOrMetadataCollection };
            ResourceTypeTree Services = new ResourceTypeTree("Services", "Web Services", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_PickID_AllServices, list2);
            ResourceTypeTree SoftwareAndApplications = new ResourceTypeTree("Software & Applications", "Software and applications", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_SoftwareAndApplications, null);
            ResourceTypeTree WebsitesAndDocuments = new ResourceTypeTree("Websites & Documents", "Websites and Documents", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_WebsitesAndDocuments, null);
            ResourceTypeTree Initiatives = new ResourceTypeTree("Initiatives", "Initiatives", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_Initiatives, null);
            ResourceTypeTree ComputationalModel = new ResourceTypeTree("Computational Model", "Computational Model", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_ComputationalModel, null);
            ResourceTypeTree MonitoringAndObservationSystems = new ResourceTypeTree("Monitoring & Observation Systems", "Monitoring and Observation Systems", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_MonitoringAndObservationSystems, null);
            ResourceTypeTree Datasets = new ResourceTypeTree("Datasets", "Datasets", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_Datasets, null);


            ObservableCollection<ResourceTypeTree> list3 = new ObservableCollection<ResourceTypeTree>{ Datasets, Services, SoftwareAndApplications, WebsitesAndDocuments, MonitoringAndObservationSystems, ComputationalModel, Initiatives };
            //ResourceType All = new ResourceType("All", "All kinds of resources", "/GeoSearch;component/images/resourceTypes/map.png", true, ConstantCollection.resourceType_PickID_All, list3);
            //list.Add(All);

            return list3;
        }

        public static ResourceType createResourceType(ResourceTypeTree resourceType)
        {
            ResourceType root = new ResourceType();
            root.Name = resourceType.Name;
            root.isSelected = resourceType.isSelected;
            root.ResourceTypeID = resourceType.ResourceTypeID;
            if (resourceType.Children != null)
            {
                root.Children = new ObservableCollection<ResourceType>();
                foreach (ResourceTypeTree rtt in resourceType.Children)
                {
                    ResourceType rt = createResourceType(rtt);
                    root.Children.Add(rt);
                }
            }
            return root;
        }
    }
}
