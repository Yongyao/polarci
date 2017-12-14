using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class WMSLayer
    {
        //The name of layer
        public string name { get; set; }
        //The title of layer
        public string title { get; set; }
        //The rank score of layer
        public double rankValue {get; set;}
        //The getMap response time of layer
        public double responseTime { get; set; }
        //LatLonBoundingBox of the WMS layer
        public BBox box { get; set; }

    }
}