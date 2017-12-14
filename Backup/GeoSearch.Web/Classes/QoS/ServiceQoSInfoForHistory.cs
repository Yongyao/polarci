using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class ServiceQoSInfoForHistory
    {
        public string serviceURL;
        public string serviceName { get; set; }
        public string serviceType { get; set; }
        //public double rankValue { get; set; }
        //public double FGDCScroe { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public List<ServiceQoSInfoInOneMeasurement> measurementInfoList { get; set; }
    }
}