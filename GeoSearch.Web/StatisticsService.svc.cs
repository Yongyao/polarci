using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Linq;
using System.Net;
using System.Collections.Generic;

// ??????????
// Get the client information (e.g. IP) and keep log 

namespace GeoSearch.Web
{
    [ServiceContract(Namespace = "cisc.gmu.edu/StatisticsService")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StatisticsService
    {
        [OperationContract]
        public string getVisitIPAddress()
        {
            string CustomerIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            ////NOTE: This is another way to do it but the one method is more straight forward:      
            //if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //else if (System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            //    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            return CustomerIP;
        }

        [OperationContract]
        public string getLocationFromIP(string ip)
        {
            string queryUrl = "http://api.ipinfodb.com/v2/ip_query.php?key=af7babafb98ede38afa390e72fc4251044cb50b4561b2f7e0af335a57c28dbfe&ip=" + ip + "&timezone=true";

            //"http://api.ipinfodb.com/v3/ip-city/?key=af7babafb98ede38afa390e72fc4251044cb50b4561b2f7e0af335a57c28dbfe&ip=74.125.45.100"
            //OK;;74.125.45.100;US;UNITED STATES;GEORGIA;ATLANTA;30301;33.809;-84.3548;-05:00
            XDocument oXDoc = XDocument.Load(queryUrl);
            XElement rootElement = oXDoc.Root;
            try
            {
                string fsd = rootElement.Element("Status").Value;
                string lon;
                string lat;
                if (rootElement.Element("Status").Value == "OK")
                {
                    lon = rootElement.Element("Longitude").Value;
                    lat = rootElement.Element("Latitude").Value;
                    return lon + "," + lat;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                e.GetType().ToString();
                return "";
            }
        }

        [OperationContract]
        public string getIPFromDNS(string domain)
        {
            string subDomain;
            // return the index of the first double backslash
            int http = domain.IndexOf("//");

            // if that index of the double backslash is greater than 6, subDomain
            // will be the string before the first backslash else
            if (http > 6)
            {
                subDomain = domain.Substring(0, domain.IndexOf("/"));

            }
            else
            {
                string nohttp = domain.Substring(http + 2);
                //int as = nohttp.IndexOf("/");
                if (nohttp.Contains("/"))
                {
                    subDomain = nohttp.Substring(0, nohttp.IndexOf("/"));
                }
                else
                {
                    subDomain = nohttp;
                }

            }

            IPHostEntry myiphost = null;
            //myiphost = Dns.Resolve(subDomain);
            try
            {
                myiphost = Dns.GetHostEntry(subDomain);
            }
            catch (Exception e)
            {
                e.ToString(); 
                //IPAddress address = new IPAddress(subDomain);
                //myiphost = Dns.GetHostByName()
                //if(myiphost = new IPHostEntry(subDomain);)
                return subDomain;
            }

            List<string> relist = new List<string>();
            for (int i = 0; i < myiphost.AddressList.Length; i++)
            {
                relist.Add(myiphost.AddressList[i].ToString());
            }
            // 在此处添加操作实现
            if (relist.Count > 0)
                return relist[relist.Count - 1];
            else
                return null;
        }

        [OperationContract]
        public List<ServerInfo> getServerInfoFromURLS(List<string> URLList)
        {
            List<ServerInfo> ServerInfoList = new List<ServerInfo>();
            foreach (string URL in URLList)
            {
                string IP = getIPFromDNS(URL);
                string lonlat = null;
                if (IP != null && !IP.Trim().Equals(""))
                {
                    lonlat = getLocationFromIP(IP);
                }
                ServerInfo serverInfo = new ServerInfo();
                serverInfo.URL = URL;
                serverInfo.IPAddress = IP;
                serverInfo.LonLat = lonlat;
                ServerInfoList.Add(serverInfo);
            }
            return ServerInfoList;
        }
    }
}
