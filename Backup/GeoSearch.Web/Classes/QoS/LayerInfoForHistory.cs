using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class LayerInfoForHistory
    {
        public string serviceURL { get; set; }
        public string serviceType { get; set; }

        public string startdate { get; set; }
        public string enddate { get; set; }
        public string layerName { get; set; }
    }
}