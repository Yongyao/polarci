using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class ServiceQoSInfoForSummary
    {
        public string serviceURL;
        public string serviceType;
        public double fgdcStatusCheckerScore = 0;
        public double rankValue = -1;
    }
}