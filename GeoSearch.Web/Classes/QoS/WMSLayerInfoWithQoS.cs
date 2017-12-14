using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class WMSLayerInfoWithQoS
    {
        public string serviceURL { get; set; }
        public string serviceType { get; set; }
        public List<WMSLayer> WMSLayersList { get; set; }
    }
}