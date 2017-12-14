using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GeoSearch.Web
{
    public class ResourceType
    {
        public string Name { get; set; }
        public ObservableCollection<ResourceType> Children { get; set; }
        public ResourceType Parent { get; set; }
        public string ResourceTypeID { get; set; }
        public bool isSelected { get; set; }
    }
}
