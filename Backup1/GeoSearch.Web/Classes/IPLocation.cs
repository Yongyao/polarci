using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoSearch.Web
{
    public class IPLocation
    {
        public string URL;
        public string IP;
        public string Location;

        public IPLocation(string url, string ip, string location)
        {
            URL = url;
            IP = ip;
            Location = location;
        }
    }
}