using System;
using System.Net;
using System.Collections.ObjectModel;
using System.Collections.Generic;

// ????
namespace GeoSearch.Web
{
    public class CascadedWMSLayer
    {
        public ObservableCollection<CascadedWMSLayer> Children { get; set; }
        //The name of layer
        public string name { get; set; }
        //The title of layer
        public string title { get; set; }
        //The rank score of layer
        public double rankValue { get; set; }
        //The getMap response time of layer
        public double responseTime { get; set; }
        //LatLonBoundingBox of the WMS layer (EPSG:4326)
        public BBox latLonBBox { get; set; }
        //Other BoundingBox Expression of the WMS layer
        public List<BBox> otherCRSBBoxList { get; set; }
        //is this the WMS layer time-enabled
        public bool timeEnabled { get; set; }
        //is this the WMS layer queryable
        public bool queryable { get; set;}
        //is this the WMS layer can execute getMap operation
        public bool canGetMap { get; set; }
        //time extent for this layer
        public string extent_time { get; set; }
        //the default time for this layer when execute getMap operation
        public string extent_time_default { get; set; }
        //the URL of this map layer's legend
        public string legendURL { get; set; }

    }
}
