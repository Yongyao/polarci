using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class LayerQoSInfoForHistory
    {
        public string serviceURL;
        public string layerName { get; set; }
        public string serviceType { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public List<LayerQoSInfoInOneMeasurement> measurementInfoList { get; set; }
    }
}