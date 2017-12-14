using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class WMSLayers
    {
        public List<WMSLayer> layersList { set; get; }
        public string name { set; get; }
        public string title { get; set; }
        public BBox box { set; get; }
        public string WMSURL { set; get; }
    }
}