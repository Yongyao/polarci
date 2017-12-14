using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using GeoSearch.Web;
using System.Xml.Linq;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.IO; 

// This is the CSW query service based on CSW: Get Records 

namespace GeoSearch.Web
{
    [ServiceContract(Namespace = "cisc.gmu.edu/MetadataSearching")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CSWQueryService
    {
        //public const string CLHCSWURLString = "http://clearinghouse.cisc.gmu.edu/geonetwork/srv/en/csw";

        //jizhe
     //   public const string CLHCSWURLString = "http://199.26.254.181/geonetwork/srv/eng/csw";
        public const string CLHCSWURLString = "http://oceanci.cloudapp.net:8080/geonetwork/srv/eng/csw";

        //public const string CLHCSWURLString = "http://129.174.244.107/geonetwork/srv/en/csw"; 
        //public const string CLHCSWURLString = "http://catalog.usgin.org/geoportal/csw";
        //public const string CLHCSWURLString = "http://ec2-184-73-35-62.compute-1.amazonaws.com:80/geonetwork/srv/en/csw";
        //public const string CLHCSWURLString = "http://ec2-75-101-156-185.compute-1.amazonaws.com/geonetwork/srv/en/csw";
        //public const string CLHCSWURLString = "http://ec2-50-19-223-225.compute-1.amazonaws.com/geonetwork/srv/en/csw";
        //public const string GOSCSWURLString = "http://catalog.geodata.gov/geoportal/csw/discovery";
        public const string GOSCSWURLString = "http://geo.data.gov/geoportal/csw";
        public const string CSRCSWURLString = "http://geossregistries.info:1090/GEOSSCSW202/discovery";
        public const string USGIN_AASG_CSWURLString = "http://catalog.usgin.org/geoportal/csw";

        public const string csw_namespace = "http://www.opengis.net/cat/csw/2.0.2";
        public const string dc_namespace = "http://purl.org/dc/elements/1.1/";
        public const string dct_namespace = "http://purl.org/dc/terms/";
        public const string ogc_namespace = "http://www.opengis.net/ogc";
        public const string gmd_namespace = "http://www.isotc211.org/2005/gmd";
        public const string gml_namespace = "http://www.opengis.net/gml";
        public const string ows_namespace = "http://www.opengis.net/ows";
        public const string rim_namespace = "urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0";

        public const string soapenv_namespace = "http://schemas.xmlsoap.org/soap/envelope/";
        public const string xsd_namespace = "http://www.w3.org/2001/XMLSchema";
        public const string xsi_namespace = "http://www.w3.org/2001/XMLSchema-instance";

        public const string GOS_Reference_Onlink = "urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Onlink";
        public const string GOS_Reference_Thumbnail = "urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Thumbnail";
        public const string GOS_Reference_Document = "urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Document";
        public const string GOS_Reference_Server = "urn:x-esri:specification:ServiceType:ArcIMS:Metadata:Server";

        public const string resourceIdentificationKeywords_geossDateCore = "geossDataCore";

        private static Random random = new Random();

        string resultType = "results";
        string elementSetName = "full";
        public double responseTime = 0.0;
        public string resultString = null;
        public string contentToSearching = null;

        private QualityQueryService qualityQueryService = new QualityQueryService();
        private StatisticsService statisticsService = new StatisticsService();


        public Dictionary<string, IPLocation> domainName_IPAndLocation = new Dictionary<string, IPLocation>()
        { 
{"http://map.strassenbau.niedersachsen.de/cgi-bin/mapserv?map=d:/daten/mapserver/eustr/eustr.map&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetCapabilities&Format=application/vnd.ogc.xml",
new IPLocation("http://map.strassenbau.niedersachsen.de/cgi-bin/mapserv?map=d:/daten/mapserver/eustr/eustr.map&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetCapabilities&Format=application/vnd.ogc.xml", "195.37.202.180", "9.85,52.2333")},
{"http://www.sapos-ni-ntrip.de",
new IPLocation("http://www.sapos-ni-ntrip.de", "195.37.202.231", "9.85,52.2333")},
{"http://10.17.151.34/cgi-bin/adabgdi-nldwms.exe?REQUEST=GetCapabilities&VERSION=1.1.1&SERVICE=WMS",
new IPLocation("http://10.17.151.34/cgi-bin/adabgdi-nldwms.exe?REQUEST=GetCapabilities&VERSION=1.1.1&SERVICE=WMS", "10.17.151.34", "0,0")},
{"http://www.lgn.niedersachsen.de/live/live.php?navigation_id=11018&article_id=51387&_psmand=35",
new IPLocation("http://www.lgn.niedersachsen.de/live/live.php?navigation_id=11018&article_id=51387&_psmand=35", "195.37.199.23", "9.85,52.2333")},
{"http://www.gll.niedersachsen.de/live/live.php?navigation_id=10674&article_id=50996&_psmand=34",
new IPLocation("http://www.gll.niedersachsen.de/live/live.php?navigation_id=10674&article_id=50996&_psmand=34", "195.37.199.23", "9.85,52.2333")},
{"http://map.strassenbau.niedersachsen.de/cgi-bin/mapserv?map=D:/daten/mapserver/strnetz/strnetz.map&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetCapabilities&Format=application/vnd.ogc.xml",
new IPLocation("http://map.strassenbau.niedersachsen.de/cgi-bin/mapserv?map=D:/daten/mapserver/strnetz/strnetz.map&SERVICE=WMS&VERSION=1.1.1&REQUEST=GetCapabilities&Format=application/vnd.ogc.xml", "195.37.202.180", "9.85,52.2333")},
{"http://www.lgn.niedersachsen.de/live/live.php?navigation_id=11020&article_id=51659&_psmand=35",
new IPLocation("http://www.lgn.niedersachsen.de/live/live.php?navigation_id=11020&article_id=51659&_psmand=35", "195.37.199.23", "9.85,52.2333")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedcu.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedcu.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=nicrpge.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=nicrpge.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=carbonatite.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=carbonatite.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=ree.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=ree.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=minfac.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=minfac.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=surveys-new.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=surveys-new.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=akgeol.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=akgeol.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ar?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ar?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ca?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ca?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/co?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/co?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ct?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ct?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/de?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/de?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/fl?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/fl?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ga?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ga?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ia?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ia?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/il?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/il?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/in?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/in?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ks?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ks?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ky?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ky?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/la?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/la?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ma?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ma?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/md?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/md?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/me?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/me?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/mi?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/mi?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/mn?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/mn?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/mo?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/mo?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ms?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ms?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/mt?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/mt?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nc?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nc?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nd?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nd?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ne?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ne?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nh?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nh?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nj?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nj?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nm?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nm?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/nv?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/nv?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ny?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ny?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/oh?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/oh?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ok?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ok?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/or?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/or?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/pa?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/pa?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ri?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ri?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/sc?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/sc?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/sd?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/sd?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/tn?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/tn?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/tx?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/tx?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/va?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/va?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/vt?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/vt?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/wa?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/wa?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/wi?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/wi?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/wv?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/wv?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/id?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/id?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=nurewtr.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=nurewtr.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://nsidc.org/cgi-bin/atlas_south?",
new IPLocation("http://nsidc.org/cgi-bin/atlas_south?", "128.138.135.43", "-105.373,40.0878")},
{"http://mrdata.usgs.gov/services/nurewtr?",
new IPLocation("http://mrdata.usgs.gov/services/nurewtr?", "137.227.242.58", "-77.3827,38.9841")},
{"http://196.33.85.22/cgi-bin/CGS_DGS_Bedrock_and_Structural_Geology/wms?",
new IPLocation("http://196.33.85.22/cgi-bin/CGS_DGS_Bedrock_and_Structural_Geology/wms?", "196.33.85.22", "28.0833,-26.2")},
{"http://www2.demis.nl/WMS/wms.ashx?wms=WorldMap&",
new IPLocation("http://www2.demis.nl/WMS/wms.ashx?wms=WorldMap&", "194.171.50.154", "4.3667,52")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=mrds.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=mrds.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://www.gag.niedersachsen.de/live/live.php?navigation_id=25937&article_id=88290&_psmand=39",
new IPLocation("http://www.gag.niedersachsen.de/live/live.php?navigation_id=25937&article_id=88290&_psmand=39", "212.144.255.23", "9.7167,52.3667")},
{"http://ngwd-bdnes.cits.rncan.gc.ca/mapengine/oneGeology/?",
new IPLocation("http://ngwd-bdnes.cits.rncan.gc.ca/mapengine/oneGeology/?", "192.67.45.15", "-75.7,45.4167")},
{"http://128.163.2.13/arcgis/services/KGS_Geology_and_Faults/MapServer/WMSServer",
new IPLocation("http://128.163.2.13/arcgis/services/KGS_Geology_and_Faults/MapServer/WMSServer", "128.163.2.13", "-84.5075,38.0287")},
{"http://barium.isgs.uiuc.edu/ArcGIS/services/ISGS_Bedrock_Geology/MapServer/WMSServer",
new IPLocation("http://barium.isgs.uiuc.edu/ArcGIS/services/ISGS_Bedrock_Geology/MapServer/WMSServer", "128.174.174.52", "-88.2123,40.1095")},
{"http://mapas.igme.es/gis/services/PSysmin/IGME_SGN_EN_Geology/MapServer/WMSServer",
new IPLocation("http://mapas.igme.es/gis/services/PSysmin/IGME_SGN_EN_Geology/MapServer/WMSServer", "193.146.142.150", "-4,40")},
{"http://spatial.dcenr.gov.ie:80/wmsconnector/com.esri.wms.Esrimap/GSI_Bedrock_Geology?",
new IPLocation("http://spatial.dcenr.gov.ie:80/wmsconnector/com.esri.wms.Esrimap/GSI_Bedrock_Geology?", "spatial.dcenr.gov.ie:80", "")},
{"http://pektolit.geo-zs.si/1GEconnector/?",
new IPLocation("http://pektolit.geo-zs.si/1GEconnector/?", "193.2.124.26", "14.5144,46.0553")},
{"http://mapdmz.brgm.fr/cgi-bin/mapserv?map=/carto/mines/mapFiles/sigandes.map&",
new IPLocation("http://mapdmz.brgm.fr/cgi-bin/mapserv?map=/carto/mines/mapFiles/sigandes.map&", "193.56.4.126", "1.9,47.9167")},
{"http://gsv-ws.dpi.vic.gov.au/OneGeology/oneg_wms?",
new IPLocation("http://gsv-ws.dpi.vic.gov.au/OneGeology/oneg_wms?", "203.12.195.120", "150.767,-33.6")},
{"http://neowms.sci.gsfc.nasa.gov/wms/wms",
new IPLocation("http://neowms.sci.gsfc.nasa.gov/wms/wms", "169.154.196.70", "-73.9763,40.7619")},
{"http://wms.jpl.nasa.gov/wms.cgi?",
new IPLocation("http://wms.jpl.nasa.gov/wms.cgi?", "128.149.132.199", "-118.299,33.7866")},
{"http://gdata1.sci.gsfc.nasa.gov/daac-bin/G3/giovanni-wms.cgi?",
new IPLocation("http://gdata1.sci.gsfc.nasa.gov/daac-bin/G3/giovanni-wms.cgi?", "169.154.132.69", "-73.9763,40.7619")},
{"http://nlwis-snite1.agr.gc.ca/cgi-bin/ogc/apaq-aapq_wms_f?",
new IPLocation("http://nlwis-snite1.agr.gc.ca/cgi-bin/ogc/apaq-aapq_wms_f?", "192.197.71.89", "-75.57,45.26")},
{"http://www.opengis.uab.es/cgi-bin/Aragon/MiraMon5_0.cgi?",
new IPLocation("http://www.opengis.uab.es/cgi-bin/Aragon/MiraMon5_0.cgi?", "158.109.62.183", "2.1833,41.3833")},
{"http://www.opengis.uab.es/cgi-bin/iberia/MiraMon5_0.cgi?",
new IPLocation("http://www.opengis.uab.es/cgi-bin/iberia/MiraMon5_0.cgi?", "158.109.62.183", "2.1833,41.3833")},
{"http://atlas.gc.ca/cgi-bin/atlaswms_en?",
new IPLocation("http://atlas.gc.ca/cgi-bin/atlaswms_en?", "132.156.10.87", "-75.7,45.4167")},
{"http://161.111.161.171/cgi-bin/AtlasAves.exe?",
new IPLocation("http://161.111.161.171/cgi-bin/AtlasAves.exe?", "161.111.161.171", "-3.6833,40.4")},
{"http://geodata.epa.gov:80/wmsconnector/com.esri.wms.Esrimap/RAD_evt_tmdl_Image?",
new IPLocation("http://geodata.epa.gov:80/wmsconnector/com.esri.wms.Esrimap/RAD_evt_tmdl_Image?", "geodata.epa.gov:80", "")},
{"http://map.ngdc.noaa.gov:80/wmsconnector/com.esri.wms.Esrimap/Deck41?",
new IPLocation("http://map.ngdc.noaa.gov:80/wmsconnector/com.esri.wms.Esrimap/Deck41?", "map.ngdc.noaa.gov:80", "")},
{"http://www.opengis.uab.es/cgi-bin/ACDC/MiraMon5_0.cgi?",
new IPLocation("http://www.opengis.uab.es/cgi-bin/ACDC/MiraMon5_0.cgi?", "158.109.62.183", "2.1833,41.3833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/bkn?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/bkn?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/grenzen?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/grenzen?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/hoogte?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/hoogte?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/luchtfoto?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/luchtfoto?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/natuur?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/natuur?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/basispakket/nationaalwegenbestand?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/basispakket/nationaalwegenbestand?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.geodaten.bayern.de/ogc/getogc.cgi?",
new IPLocation("http://www.geodaten.bayern.de/ogc/getogc.cgi?", "195.200.71.146", "9,51")},
{"http://wms.alaskamapped.org/cgi-bin/bdl.cgi?",
new IPLocation("http://wms.alaskamapped.org/cgi-bin/bdl.cgi?", "137.229.19.58", "-147.498,64.8834")},
{"http://wms.alaskamapped.org/cgi-bin/bdl_extras.cgi?",
new IPLocation("http://wms.alaskamapped.org/cgi-bin/bdl_extras.cgi?", "137.229.19.58", "-147.498,64.8834")},
{"http://www.biodiversity.bz/cgi-bin/mapserv?map=/Users/belize/biodiversity.bz/wms.map&",
new IPLocation("http://www.biodiversity.bz/cgi-bin/mapserv?map=/Users/belize/biodiversity.bz/wms.map&", "67.43.163.125", "-118.31,34.0619")},
{"http://apps1.gdr.nrcan.gc.ca/cgi-bin/canmin_en-ca_ows?",
new IPLocation("http://apps1.gdr.nrcan.gc.ca/cgi-bin/canmin_en-ca_ows?", "132.156.97.59", "-76.2,45.2167")},
{"http://cgkn2.cgkn.net/cgi-bin/cgknwms?",
new IPLocation("http://cgkn2.cgkn.net/cgi-bin/cgknwms?", "132.156.97.3", "-76.2,45.2167")},
{"http://www.opengis.uab.es/cgi-bin/world/MiraMon5_0.cgi?",
new IPLocation("http://www.opengis.uab.es/cgi-bin/world/MiraMon5_0.cgi?", "158.109.62.183", "2.1833,41.3833")},
{"http://demo.cubewerx.com/demo/cubeserv/cubeserv.cgi?",
new IPLocation("http://demo.cubewerx.com/demo/cubeserv/cubeserv.cgi?", "209.87.237.39", "-75.7,45.4167")},
{"http://www.decisiontools.ca/services/mapserv.exe?map=ows.map&",
new IPLocation("http://www.decisiontools.ca/services/mapserv.exe?map=ows.map&", "132.156.108.210", "-76.2,45.2167")},
{"http://www.geodaten-mv.de/dienste/DOP?",
new IPLocation("http://www.geodaten-mv.de/dienste/DOP?", "195.145.109.67", "9,51")},
{"http://www.geodaten-mv.de/dienste/DTK10g?",
new IPLocation("http://www.geodaten-mv.de/dienste/DTK10g?", "195.145.109.67", "9,51")},
{"http://gdr.ess.nrcan.gc.ca:80/wmsconnector/com.esri.wms.Esrimap/energy_e?",
new IPLocation("http://gdr.ess.nrcan.gc.ca:80/wmsconnector/com.esri.wms.Esrimap/energy_e?", "gdr.ess.nrcan.gc.ca:80", "")},
{"http://maps.gnwtgeomatics.nt.ca:80/wmsconnector/com.esri.wms.Esrimap/bm_lam_p?",
new IPLocation("http://maps.gnwtgeomatics.nt.ca:80/wmsconnector/com.esri.wms.Esrimap/bm_lam_p?", "maps.gnwtgeomatics.nt.ca:80", "")},
{"http://egisws01.nos.noaa.gov:80/wmsconnector/com.esri.wms.Esrimap/census2000mapping?",
new IPLocation("http://egisws01.nos.noaa.gov:80/wmsconnector/com.esri.wms.Esrimap/census2000mapping?", "egisws01.nos.noaa.gov:80", "")},
{"http://public.geoportal-geoportail.gc.ca/wms/cascader",
new IPLocation("http://public.geoportal-geoportail.gc.ca/wms/cascader", "198.103.183.85", "-75.65,45.4833")},
{"http://idecan2.grafcan.es/ServicioWMS/Callejero?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/Callejero?", "213.0.20.230", "-4,40")},
{"http://idecan1.grafcan.es/ServicioWMS/DistOrto2000?",
new IPLocation("http://idecan1.grafcan.es/ServicioWMS/DistOrto2000?", "195.57.95.83", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/EspNat?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/EspNat?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/LIC?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/LIC?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/Cultivos?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/Cultivos?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/Vegetacion?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/Vegetacion?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/Geologico?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/Geologico?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/MOS?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/MOS?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/carto5?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/carto5?", "213.0.20.230", "-4,40")},
{"http://idecan1.grafcan.es/ServicioWMS/MDSombras?",
new IPLocation("http://idecan1.grafcan.es/ServicioWMS/MDSombras?", "195.57.95.83", "-4,40")},
{"http://idecan1.grafcan.es/ServicioWMS/OrtoExpress?",
new IPLocation("http://idecan1.grafcan.es/ServicioWMS/OrtoExpress?", "195.57.95.83", "-4,40")},
{"http://idecan1.grafcan.es/ServicioWMS/Orto2000?",
new IPLocation("http://idecan1.grafcan.es/ServicioWMS/Orto2000?", "195.57.95.83", "-4,40")},
{"http://idecan1.grafcan.es/ServicioWMS/OrtoProd?",
new IPLocation("http://idecan1.grafcan.es/ServicioWMS/OrtoProd?", "195.57.95.83", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/Planeamiento?FEATURE_COUNT=8&",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/Planeamiento?FEATURE_COUNT=8&", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/RedGeodesica?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/RedGeodesica?", "213.0.20.230", "-4,40")},
{"http://idecan2.grafcan.es/ServicioWMS/ZEPA?",
new IPLocation("http://idecan2.grafcan.es/ServicioWMS/ZEPA?", "213.0.20.230", "-4,40")},
{"http://maps.gis.iastate.edu/wms/iris/rf_huc8_com.cgi?",
new IPLocation("http://maps.gis.iastate.edu/wms/iris/rf_huc8_com.cgi?", "129.186.142.201", "-93.4652,42.036")},
{"http://maps.gis.iastate.edu/wms/iris/pf_nhd_sci.cgi?",
new IPLocation("http://maps.gis.iastate.edu/wms/iris/pf_nhd_sci.cgi?", "129.186.142.201", "-93.4652,42.036")},
{"http://mapserver.lgb-rlp.de/cgi-bin/art_des_hohlraums?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/art_des_hohlraums?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/nitratauswaschungsgefaehrdung?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/nitratauswaschungsgefaehrdung?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/archivfunktion?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/archivfunktion?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_ackerzahl?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_ackerzahl?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_bodenart?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_bodenart?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_entstehung?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_entstehung?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_ertragspotential?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_ertragspotential?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_fk?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_fk?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_k_faktor?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_k_faktor?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_nfk?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_nfk?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/bfd5_wurzelraum?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/bfd5_wurzelraum?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/erdbeben?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/erdbeben?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/geologische_uebersichtskarte300?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/geologische_uebersichtskarte300?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/grund_stau_hangnaesse?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/grund_stau_hangnaesse?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_arsen?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_arsen?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_blei?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_blei?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_cadmium?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_cadmium?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_chrom?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_chrom?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_kobalt?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_kobalt?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_kupfer?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_kupfer?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_mangan?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_mangan?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_nickel?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_nickel?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_quecksilber?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_quecksilber?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/hgw_zink?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/hgw_zink?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/ertragspotenzial?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/ertragspotenzial?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/ertragspotenzial_lwn?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/ertragspotenzial_lwn?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/nitratrueckhaltevermoegen?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/nitratrueckhaltevermoegen?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/pot_erosionsgefaehrdung?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/pot_erosionsgefaehrdung?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/pot_sickerwasserspende?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/pot_sickerwasserspende?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/saeurepuffervermoegen?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/saeurepuffervermoegen?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/retentionsvermoegen_pb?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/retentionsvermoegen_pb?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/retentionsvermoegen_cd?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/retentionsvermoegen_cd?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/biotopentwicklungspotenzial?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/biotopentwicklungspotenzial?", "134.93.7.75", "8.2711,50")},
{"http://mapserver.lgb-rlp.de/cgi-bin/wasserrueckhaltevermoegen?",
new IPLocation("http://mapserver.lgb-rlp.de/cgi-bin/wasserrueckhaltevermoegen?", "134.93.7.75", "8.2711,50")},
{"https://extranet.mainz.de/gint-cgi/mapserv?map=/data/mapbender2-int/umn-www/client/a62/wms/mz-uebersicht-wms.map&",
new IPLocation("https://extranet.mainz.de/gint-cgi/mapserv?map=/data/mapbender2-int/umn-www/client/a62/wms/mz-uebersicht-wms.map&", "213.139.155.200", "8.2711,50")},
{"http://www.idee.es/wms/IDEE-Base/IDEE-Base",
new IPLocation("http://www.idee.es/wms/IDEE-Base/IDEE-Base", "213.229.142.106", "-4,40")},
{"http://webmap.em.gov.bc.ca:80/liteview6.5/servlet/MapGuideLiteView?",
new IPLocation("http://webmap.em.gov.bc.ca:80/liteview6.5/servlet/MapGuideLiteView?", "webmap.em.gov.bc.ca:80", "")},
{"http://www.mapserver.niedersachsen.de/freezoneogc/mapserverogc?",
new IPLocation("http://www.mapserver.niedersachsen.de/freezoneogc/mapserverogc?", "195.37.203.229", "9.85,52.2333")},
{"http://www.fylkesatlas.no/wms.aspx?",
new IPLocation("http://www.fylkesatlas.no/wms.aspx?", "93.94.10.117", "10.4167,60.3")},
{"http://www.rijkswaterstaat.nl/services/geoservices/nirov/mit2006?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/nirov/mit2006?", "145.45.0.50", "5.1333,52.0833")},
{"http://www.rijkswaterstaat.nl/services/geoservices/nirov/mjpo?",
new IPLocation("http://www.rijkswaterstaat.nl/services/geoservices/nirov/mjpo?", "145.45.0.50", "5.1333,52.0833")},
{"http://mapas.mma.gov.br/i3geo/ogc.php?",
new IPLocation("http://mapas.mma.gov.br/i3geo/ogc.php?", "200.198.242.78", "-55,-10")},
{"http://wms.alaskamapped.org/cgi-bin/charts.cgi?",
new IPLocation("http://wms.alaskamapped.org/cgi-bin/charts.cgi?", "137.229.19.58", "-147.498,64.8834")},
{"http://wms.alaskamapped.org/cgi-bin/charts_nc.cgi?",
new IPLocation("http://wms.alaskamapped.org/cgi-bin/charts_nc.cgi?", "137.229.19.58", "-147.498,64.8834")},
{"http://www.bgr.de/Service/OneGeology/BGR_EN_Geological_Units/?",
new IPLocation("http://www.bgr.de/Service/OneGeology/BGR_EN_Geological_Units/?", "193.174.160.58", "9.7167,52.3667")},
{"http://geomaps2.gtk.fi/ArcGIS/services/LaosOneGeology/MapServer/WMSServer",
new IPLocation("http://geomaps2.gtk.fi/ArcGIS/services/LaosOneGeology/MapServer/WMSServer", "193.167.179.49", "26,64")},
{"http://geomap.geolba.ac.at:80/GBAWMS/com.esri.wms.Esrimap/GBA_Bedrock_Geology_and_Tectonic_Lineaments?",
new IPLocation("http://geomap.geolba.ac.at:80/GBAWMS/com.esri.wms.Esrimap/GBA_Bedrock_Geology_and_Tectonic_Lineaments?", "geomap.geolba.ac.at:80", "")},
{"http://www.bgr.de/Service/OneGeology/GGS_Geological_Units/?",
new IPLocation("http://www.bgr.de/Service/OneGeology/GGS_Geological_Units/?", "193.174.160.58", "9.7167,52.3667")},
{"http://ogc.bgs.ac.uk/1GEConnector/bedrock/?",
new IPLocation("http://ogc.bgs.ac.uk/1GEConnector/bedrock/?", "194.66.252.136", "-2,54")},
{"http://ogc.bgs.ac.uk/1GEConnector/surface/?",
new IPLocation("http://ogc.bgs.ac.uk/1GEConnector/surface/?", "194.66.252.136", "-2,54")},
{"http://maps2.sgu.se/wmsconnector/com.esri.wms.Esrimap/SGU_Bedrock_and_Superficial_geology?",
new IPLocation("http://maps2.sgu.se/wmsconnector/com.esri.wms.Esrimap/SGU_Bedrock_and_Superficial_geology?", "194.68.99.12", "15,62")},
{"http://mapas.igme.es/gis/services/oneGeology/IGME_EN_Geology/MapServer/WMSServer",
new IPLocation("http://mapas.igme.es/gis/services/oneGeology/IGME_EN_Geology/MapServer/WMSServer", "193.146.142.150", "-4,40")},
{"http://mapy.geology.cz:80/wmsconnector/com.esri.wms.Esrimap/CGS_Solid_Geology?",
new IPLocation("http://mapy.geology.cz:80/wmsconnector/com.esri.wms.Esrimap/CGS_Solid_Geology?", "mapy.geology.cz:80", "")},
{"http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/SIGAfrique_Formations_et_Geologie_Structurale.map&",
new IPLocation("http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/SIGAfrique_Formations_et_Geologie_Structurale.map&", "193.56.4.105", "1.9,47.9167")},
{"http://services.azgs.az.gov/ArcGIS/services/OneGeology/AZGS_Arizona_Geology/MapServer/WMSServer",
new IPLocation("http://services.azgs.az.gov/ArcGIS/services/OneGeology/AZGS_Arizona_Geology/MapServer/WMSServer", "159.87.39.14", "-110.956,32.2369")},
{"http://geocatmin.ingemmet.gob.pe/ArcGIS/services/SERV_GEOLOGIA/MapServer/WMSServer",
new IPLocation("http://geocatmin.ingemmet.gob.pe/ArcGIS/services/SERV_GEOLOGIA/MapServer/WMSServer", "200.62.170.203", "-77.05,-12.05")},
{"http://gis.pch.etat.lu/geology/1GEconnector",
new IPLocation("http://gis.pch.etat.lu/geology/1GEconnector", "194.154.200.87", "6.13,49.6117")},
{"http://mafi-loczy.mafi.hu/ArcGIS/services/Onegeology/MAFI_GEology/MapServer/WMSServer",
new IPLocation("http://mafi-loczy.mafi.hu/ArcGIS/services/Onegeology/MAFI_GEology/MapServer/WMSServer", "109.74.49.74", "20,47")},
{"http://mapsone.brgm.fr/1GmapserverFR/wms?map=/applications/mapserver/mapFiles/Lithology_FR.map",
new IPLocation("http://mapsone.brgm.fr/1GmapserverFR/wms?map=/applications/mapserver/mapFiles/Lithology_FR.map", "193.56.4.107", "1.9,47.9167")},
{"http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/GISEurope_Bedrock_and_Structural_Geology.map&",
new IPLocation("http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/GISEurope_Bedrock_and_Structural_Geology.map&", "193.56.4.105", "1.9,47.9167")},
{"http://ogc.sernageomin.cl/cgi-bin/SNGM_Bedrock_Geology/wms.exe?",
new IPLocation("http://ogc.sernageomin.cl/cgi-bin/SNGM_Bedrock_Geology/wms.exe?", "190.196.161.232", "-70.6667,-33.45")},
{"http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/SIGAfrique_EN_Bedrock_and_Structural_Geology.map&",
new IPLocation("http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/SIGAfrique_EN_Bedrock_and_Structural_Geology.map&", "193.56.4.105", "1.9,47.9167")},
{"http://maps.bgs.ac.uk/ArcGIS/services/BGS_Detailed_Geology/MapServer/WMSServer",
new IPLocation("http://maps.bgs.ac.uk/ArcGIS/services/BGS_Detailed_Geology/MapServer/WMSServer", "194.66.252.150", "-2,54")},
{"http://gis.geosurv.gov.nl.ca/wmsconnector/com.esri.wms.Esrimap/NAD27_MineralResources_NL?",
new IPLocation("http://gis.geosurv.gov.nl.ca/wmsconnector/com.esri.wms.Esrimap/NAD27_MineralResources_NL?", "209.128.28.106", "-52.6667,47.55")},
{"http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/YGSMRB_Bedrock_and_Structural_Geology.map&",
new IPLocation("http://mapdmzrec.brgm.fr/cgi-bin/mapserv?map=/carto/ogg/mapFiles/YGSMRB_Bedrock_and_Structural_Geology.map&", "193.56.4.105", "1.9,47.9167")},
{"http://dov.vlaanderen.be/arcgis/services/ALBON_Geology/MapServer/WMSServer",
new IPLocation("http://dov.vlaanderen.be/arcgis/services/ALBON_Geology/MapServer/WMSServer", "193.191.138.1", "4.3333,50.8333")},
{"http://mapdmzrec.brgm.fr/cgi-bin/mapserv54?map=/carto/ogg/mapFiles/CGMW_Bedrock_and_Structural_Geology.map&",
new IPLocation("http://mapdmzrec.brgm.fr/cgi-bin/mapserv54?map=/carto/ogg/mapFiles/CGMW_Bedrock_and_Structural_Geology.map&", "193.56.4.105", "1.9,47.9167")},
{"http://e-geo.ineti.pt/ArcGIS/services/CGP1M_OGE/MapServer/WMSServer",
new IPLocation("http://e-geo.ineti.pt/ArcGIS/services/CGP1M_OGE/MapServer/WMSServer", "193.137.43.145", "-9.1333,38.7167")},
{"http://maps2.sgu.se/wmsconnector/com.esri.wms.Esrimap/SGU_Bedrock_geology_Fennoscandian_shield?",
new IPLocation("http://maps2.sgu.se/wmsconnector/com.esri.wms.Esrimap/SGU_Bedrock_geology_Fennoscandian_shield?", "194.68.99.12", "15,62")},
{"http://onegeology.naturalsciences.be/1GEconnector/",
new IPLocation("http://onegeology.naturalsciences.be/1GEconnector/", "193.190.234.10", "4.3333,50.8333")},
{"http://www.bgr.de/Service/OneGeology/BGR_Geological_Units_IGME5000/?",
new IPLocation("http://www.bgr.de/Service/OneGeology/BGR_Geological_Units_IGME5000/?", "193.174.160.58", "9.7167,52.3667")},
{"http://sgi2.isprambiente.it:80/wmsconnector/com.esri.wms.Esrimap/oneg_eng?",
new IPLocation("http://sgi2.isprambiente.it:80/wmsconnector/com.esri.wms.Esrimap/oneg_eng?", "sgi2.isprambiente.it:80", "")},
{"http://ngmdb.geos.pdx.edu/cgi-bin/mapserv.cgi?map=/vol/www/ngmdb/htmaps/gmna/GMNA_OWS_display.map&",
new IPLocation("http://ngmdb.geos.pdx.edu/cgi-bin/mapserv.cgi?map=/vol/www/ngmdb/htmaps/gmna/GMNA_OWS_display.map&", "131.252.120.151", "-122.693,45.5073")},
{"http://www.bgr.de/Service/OneGeology/BGR_Geologische_Einheiten/?",
new IPLocation("http://www.bgr.de/Service/OneGeology/BGR_Geologische_Einheiten/?", "193.174.160.58", "9.7167,52.3667")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=copper-smelters.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=copper-smelters.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedznpb.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedznpb.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/wy?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/wy?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=active-mines.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=active-mines.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://81.223.55.147/geoland2?",
new IPLocation("http://81.223.55.147/geoland2?", "81.223.55.147", "11.4,47.2667")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=akages.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=akages.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=ardf.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=ardf.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=karage.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=karage.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=ngdbrock.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=ngdbrock.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=ngs.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=ngs.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=nuresed.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=nuresed.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=vms.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=vms.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/ut?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/ut?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=gravity.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=gravity.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedexmvt.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=sedexmvt.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/az?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/az?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/state/al?",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/state/al?", "137.227.242.58", "-77.3827,38.9841")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=kb.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=kb.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://kaart.maaamet.ee/wms/ENLB_Bedrock_Geology/wms?",
new IPLocation("http://kaart.maaamet.ee/wms/ENLB_Bedrock_Geology/wms?", "213.184.51.74", "24.7281,59.4339")},
{"http://www.onegeology-arg.com.ar:80/geoserver/wms?SERVICE=WMS&",
new IPLocation("http://www.onegeology-arg.com.ar:80/geoserver/wms?SERVICE=WMS&", "www.onegeology-arg.com.ar:80", "")},
{"http://deli.dnr.state.mn.us/cgi-bin/wms?map=DELI_WMS_MAPFILE&",
new IPLocation("http://deli.dnr.state.mn.us/cgi-bin/wms?map=DELI_WMS_MAPFILE&", "156.98.35.213", "-93.0955,44.9522")},
{"http://dnweb12.dirnat.no:80/wmsconnector/com.esri.wms.Esrimap/WMS_NB_Verneforslag?",
new IPLocation("http://dnweb12.dirnat.no:80/wmsconnector/com.esri.wms.Esrimap/WMS_NB_Verneforslag?", "dnweb12.dirnat.no:80", "")},
{"http://www.indexgeo.com.au/cgi-bin/wms-location",
new IPLocation("http://www.indexgeo.com.au/cgi-bin/wms-location", "66.160.183.131", "-88.1526,30.6278")},
{"http://www.gulfofmaine.org/cgi-bin/gomc_esip_wms?",
new IPLocation("http://www.gulfofmaine.org/cgi-bin/gomc_esip_wms?", "75.98.160.71", "-97,38")},
{"http://arcus2.nve.no:80/wmsconnector/com.esri.wms.Esrimap/wms_flomsoner?",
new IPLocation("http://arcus2.nve.no:80/wmsconnector/com.esri.wms.Esrimap/wms_flomsoner?", "arcus2.nve.no:80", "")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_SRTM_DEM_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_SRTM_DEM_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_RedRiverValley_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_RedRiverValley_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_RedRiverValley_DEM_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_RedRiverValley_DEM_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Surficial_Geology_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Surficial_Geology_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Bedrock_Geology_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Bedrock_Geology_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://webmail.mafi.hu/1GEconnector/",
new IPLocation("http://webmail.mafi.hu/1GEconnector/", "109.74.55.7", "20,47")},
{"http://wms.ga.admin.ch/SGS_Geol_Tecto/wms?",
new IPLocation("http://wms.ga.admin.ch/SGS_Geol_Tecto/wms?", "46.137.164.198", "32,49")},
{"http://geo.ngu.no/mapserver/NGU_Bedrock_and_Superficial_Geology/wms?",
new IPLocation("http://geo.ngu.no/mapserver/NGU_Bedrock_and_Superficial_Geology/wms?", "193.156.55.55", "10.4167,63.4167")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_GA_Bedrock_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_GA_Bedrock_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://pektolit.geo-zs.si/AGS_Bedrock_and_Superficial_Geology/wms?",
new IPLocation("http://pektolit.geo-zs.si/AGS_Bedrock_and_Superficial_Geology/wms?", "193.2.124.26", "14.5144,46.0553")},
{"http://geodata1.geogrid.org/mapserv/GSJ_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/GSJ_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://geodata1.geogrid.org/mapserv/MGB_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/MGB_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://onegeology.kigam.re.kr/cgi-bin/KIGAM_Bedrock_Geology/wms?",
new IPLocation("http://onegeology.kigam.re.kr/cgi-bin/KIGAM_Bedrock_Geology/wms?", "203.247.172.181", "127,37.5664")},
{"http://geodata1.geogrid.org/mapserv/GRDC_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/GRDC_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://geodata1.geogrid.org/mapserv/CCOP_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/CCOP_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://onegeology.cprm.gov.br/cgi-bin/BRA_GSB_EN_Bedrock_Geology/wms?",
new IPLocation("http://onegeology.cprm.gov.br/cgi-bin/BRA_GSB_EN_Bedrock_Geology/wms?", "200.20.109.33", "-55,-10")},
{"http://onegeology.cprm.gov.br/cgi-bin/BRA_GSB_Bedrock_Geology/wms?",
new IPLocation("http://onegeology.cprm.gov.br/cgi-bin/BRA_GSB_Bedrock_Geology/wms?", "200.20.109.33", "-55,-10")},
{"http://193.190.223.44/ionicweb/wfs/ONE_GEOLOGY_RMCA?",
new IPLocation("http://193.190.223.44/ionicweb/wfs/ONE_GEOLOGY_RMCA?", "193.190.223.44", "4.5167,50.8167")},
{"unknown",
new IPLocation("unknown", "nknown", "-73.7333,45.6")},
{"http://mapdmz.brgm.fr/cgi-bin/mapserv?map=/carto/sigafrique/mapFiles/sigaf.map&",
new IPLocation("http://mapdmz.brgm.fr/cgi-bin/mapserv?map=/carto/sigafrique/mapFiles/sigaf.map&", "193.56.4.126", "1.9,47.9167")},
{"http://www.bsc-eoc.org/cgi-bin/bsc_ows.asp?",
new IPLocation("http://www.bsc-eoc.org/cgi-bin/bsc_ows.asp?", "205.150.58.109", "-80.45,42.6333")},
{"http://wms.wheregroup.com/cgi-bin/mapserv?map=/data/umn/germany/germany.map&",
new IPLocation("http://wms.wheregroup.com/cgi-bin/mapserv?map=/data/umn/germany/germany.map&", "93.89.10.143", "6.5667,51.3333")},
{"http://195.243.84.83/cgi-bin/mapserv.exe?map=/web/wms/hgn-ogc.map&",
new IPLocation("http://195.243.84.83/cgi-bin/mapserv.exe?map=/web/wms/hgn-ogc.map&", "195.243.84.83", "9,51")},
{"http://www.dinoservices.nl/wms/dinomap/M08M0006",
new IPLocation("http://www.dinoservices.nl/wms/dinomap/M08M0006", "145.222.32.76", "5.15,52.3")},
{"http://www.dinoservices.nl/wms/dinomap/M08M0007",
new IPLocation("http://www.dinoservices.nl/wms/dinomap/M08M0007", "145.222.32.76", "5.15,52.3")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_AGS_EN_Bedrock_and_Structural_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_AGS_EN_Bedrock_and_Structural_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://196.33.85.22/cgi-bin/ZAF_CGS_Bedrock_Geology/wms?",
new IPLocation("http://196.33.85.22/cgi-bin/ZAF_CGS_Bedrock_Geology/wms?", "196.33.85.22", "28.0833,-26.2")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_BUMIGEB_FR_Bedrock_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_BUMIGEB_FR_Bedrock_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://196.33.85.22/cgi-bin/BWA_DGS_Faults/wms?",
new IPLocation("http://196.33.85.22/cgi-bin/BWA_DGS_Faults/wms?", "196.33.85.22", "28.0833,-26.2")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_GSN_Bedrock_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_GSN_Bedrock_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://196.33.85.22/cgi-bin/BWA_DGS_Intrusives/wms?",
new IPLocation("http://196.33.85.22/cgi-bin/BWA_DGS_Intrusives/wms?", "196.33.85.22", "28.0833,-26.2")},
{"http://196.33.85.22/cgi-bin/BWA_DGS_Bedrock_Geology/wms?",
new IPLocation("http://196.33.85.22/cgi-bin/BWA_DGS_Bedrock_Geology/wms?", "196.33.85.22", "28.0833,-26.2")},
{"http://geodata1.geogrid.org/mapserv/GSJ_JMG_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/GSJ_JMG_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://geodata1.geogrid.org/mapserv/DMR_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?",
new IPLocation("http://geodata1.geogrid.org/mapserv/DMR_Combined_Bedrock_and_Superficial_Geology_and_Age/wms?", "163.220.61.68", "135.567,34.8167")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Chronostrat_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_Chronostrat_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://www.idee.es/wms/IDEE-Limite/IDEE-Limite",
new IPLocation("http://www.idee.es/wms/IDEE-Limite/IDEE-Limite", "213.229.142.106", "-4,40")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_FIG_Bedrock_and_Superficial_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_FIG_Bedrock_and_Superficial_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://www.ga.gov.au/wms/getmap?dataset=geows&",
new IPLocation("http://www.ga.gov.au/wms/getmap?dataset=geows&", "192.104.44.6", "149.217,-35.2833")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_Bedrock_and_Superficial_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_Bedrock_and_Superficial_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://certmapper.cr.usgs.gov/arcgis/services/one_geology_wms/USGS_Geologic_Map_of_North_America/MapServer/WMSServer",
new IPLocation("http://certmapper.cr.usgs.gov/arcgis/services/one_geology_wms/USGS_Geologic_Map_of_North_America/MapServer/WMSServer", "137.227.229.75", "-77.3827,38.9841")},
{"http://ogc.bgs.ac.uk/cgi-bin/BGS_1GE_Geology/wms?",
new IPLocation("http://ogc.bgs.ac.uk/cgi-bin/BGS_1GE_Geology/wms?", "194.66.252.136", "-2,54")},
{"http://jupiter.geus.dk/GRL_GEUS_Bedrock_and_Superficial_Geology/wms/mapserv.exe?map=onegeology.map&",
new IPLocation("http://jupiter.geus.dk/GRL_GEUS_Bedrock_and_Superficial_Geology/wms/mapserv.exe?map=onegeology.map&", "195.41.77.165", "12.5833,55.6667")},
{"http://jupiter.geus.dk/DNK_GEUS_Bedrock_and_Superficial_Geology/wms/mapserv.exe?map=onegeology.map&",
new IPLocation("http://jupiter.geus.dk/DNK_GEUS_Bedrock_and_Superficial_Geology/wms/mapserv.exe?map=onegeology.map&", "195.41.77.165", "12.5833,55.6667")},
{"http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_WillistonBasin_Chronostrat_WMS?",
new IPLocation("http://geoapp2.gov.mb.ca/wmsconnector/com.esri.wms.Esrimap/CAN_MGS_WillistonBasin_Chronostrat_WMS?", "198.163.15.112", "-97.1667,49.8833")},
{"http://mrdata.usgs.gov/cgi-bin/mapserv?map=porcu.map&",
new IPLocation("http://mrdata.usgs.gov/cgi-bin/mapserv?map=porcu.map&", "137.227.242.58", "-77.3827,38.9841")},
{"http://maps.gns.cri.nz:80/geoserver/wms?SERVICE=WMS&",
new IPLocation("http://maps.gns.cri.nz:80/geoserver/wms?SERVICE=WMS&", "maps.gns.cri.nz:80", "")},
{"http://nsidc.org/cgi-bin/atlas_north?",
new IPLocation("http://nsidc.org/cgi-bin/atlas_north?", "128.138.135.43", "-105.373,40.0878")},
{"http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_trmm?",
new IPLocation("http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_trmm?", "169.154.132.52", "-73.9763,40.7619")},
{"http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_airs?",
new IPLocation("http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_airs?", "169.154.132.52", "-73.9763,40.7619")},
{"http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_airsnrt?",
new IPLocation("http://disc1.sci.gsfc.nasa.gov/daac-bin/wms_airsnrt?", "169.154.132.52", "-73.9763,40.7619")},
{"http://testing.deegree.org:80/deegree-wms/services?",
new IPLocation("http://testing.deegree.org:80/deegree-wms/services?", "testing.deegree.org:80", "")},
{"http://rmgsc.cr.usgs.gov/arcgis/services/ecosys_US/MapServer/WMSServer",
new IPLocation("http://rmgsc.cr.usgs.gov/arcgis/services/ecosys_US/MapServer/WMSServer", "137.227.229.27", "-77.3827,38.9841")},
        };

        [OperationContract]
        public CSWGetRecordsSearchResults getRecords_BasicSearch(string searchingContent, string startPosition, string maxRecords, string CSWURL, bool queryServicePerformanceScoreAtServerSide, bool calculateRelevanceAtServerSide)
        {
            maxRecords = "20";
            Console.Write("===========");

            CSWGetRecordsSearchResults results = null;

            //jizhe
            //if (searchingContent == null || searchingContent.Trim().Equals(""))
                //return null;

            contentToSearching = searchingContent;
            //string[] listOfKeywords = searchingContent.Split(' ');
            SearchRules rules = SearchRules.getSearchRules(searchingContent);

            //XDocument doc = new XDocument();
            //XElement rootElement = new XElement("");
            //   new XElement("GetRecords"
            //       new XElement("Contact1",
            //           new XElement("Name", "Patrick Hines"),
            //           new XElement("Phone", "206-555-0144"),
            //           new XElement("Address",
            //               new XElement("Street1", "123 Main St"),
            //               new XElement("City", "Mercer Island"),
            //               new XElement("State", "WA"),
            //               new XElement("Postal", "68042")
            //           )
            //       ),
            //       new XElement("Contact2",
            //           new XElement("Name", "Yoshi Latime"),
            //           new XElement("Phone", "503-555-6874"),
            //           new XElement("Address",
            //               new XElement("Street1", "City Center Plaza 516 Main St."),
            //               new XElement("City", "Elgin"),
            //               new XElement("State", "OR"),
            //               new XElement("Postal", "97827")
            //           )
            //       )
            //   );

            //string postRequest = "<?xml version='1.0' encoding='UTF-8'?>"
            //    + "<csw:GetRecords xmlns:csw='http://www.opengis.net/cat/csw/2.0.2' xmlns='http://www.opengis.net/cat/csw/2.0.2'"
            //    + " xmlns:gmd='http://www.isotc211.org/2005/gmd' xmlns:ogc='http://www.opengis.net/ogc' xmlns:gml='http://www.opengis.net/gml'"
            //    + " xmlns:rim='urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0' service='CSW' version='2.0.2' outputFormat='application/xml'" +
            //    " outputSchema='http://www.opengis.net/cat/csw/2.0.2' "
            //    + "resultType='" + resultType + "' maxRecords='" + maxRecords + "' startPosition='" + startPosition + "'>"
            //     + "<csw:Query typeNames='csw:Record'>"
            //        + "<csw:ElementSetName>"
            //            + elementSetName
            //        + "</csw:ElementSetName>"
            //        + "<csw:Constraint version='1.1.0'>"
            //            + "<ogc:Filter>"
            //                + "<ogc:PropertyIsLike escapeChar='\' singleChar='?' wildCard='*'>"
            //                    + "<ogc:PropertyName>anyText</ogc:PropertyName>"
            //                    + "<ogc:Literal>"
            //                        + contentToSearching
            //                    + "</ogc:Literal>"
            //                + "</ogc:PropertyIsLike>"
            //            + "</ogc:Filter>"
            //        + "</csw:Constraint>"
            //     + "</csw:Query>"
            //+ "</csw:GetRecords>";

            XDocument doc1 = new XDocument();
            if (CSWURL.Equals(CLHCSWURLString) || CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
            {

                //XNamespace cswNamespace = csw_namespace;
                //XElement GetRecords = new XElement(cswNamespace + "GetRecords", new XAttribute(XNamespace.Xmlns + "csw", csw_namespace));
                XElement GetRecords = new XElement(XName.Get("GetRecords", csw_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "csw", csw_namespace));
                //GetRecords.SetAttributeValue("xmlns", csw_namespace);
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gmd", gmd_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "ogc", ogc_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gml", gml_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "rim", rim_namespace));

                GetRecords.SetAttributeValue("service", "CSW");
                GetRecords.SetAttributeValue("version", "2.0.2");
                GetRecords.SetAttributeValue("outputFormat", "application/xml");
                GetRecords.SetAttributeValue("outputSchema", csw_namespace);
                GetRecords.SetAttributeValue("resultType", resultType);
                GetRecords.SetAttributeValue("maxRecords", maxRecords);
                GetRecords.SetAttributeValue("startPosition", startPosition);

                XElement Query = new XElement(XName.Get("Query", csw_namespace));
                Query.SetAttributeValue("typeNames", "csw:Record");
                XElement ElementSetName = new XElement(XName.Get("ElementSetName", csw_namespace));
                ElementSetName.SetValue(elementSetName);
                XElement Constraint = new XElement(XName.Get("Constraint", csw_namespace));
                Constraint.SetAttributeValue("version", "1.1.0");
                XElement Filter = new XElement(XName.Get("Filter", ogc_namespace));
                XElement And = new XElement(XName.Get("And", ogc_namespace));

                Constraint.Add(Filter);
                Query.Add(ElementSetName);
                Query.Add(Constraint);
                GetRecords.Add(Query);
                doc1.Add(GetRecords);

                if (Filter != null)
                {
                    //int len = listOfKeywords.Length;
                    //for (int i = 0; i < len; i++)
                    //{
                    //    string keyword = listOfKeywords[i];
                    //    if (!keyword.Trim().Equals(""))
                    //    {
                    //        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("anyText", keyword);
                    //        And.Add(PropertyIsLike);
                    //    }
                    //}

                    if (rules.resourceTypes.Count > 0)
                    {
                        XElement Or = new XElement(XName.Get("Or", ogc_namespace));
                        foreach (string resourceType in rules.resourceTypes)
                        {
                            if (resourceType.Equals(SearchingContent.resourceType_Datasets))
                            {
                                XElement PropertyIsLike0 = createFiltereContent_PropertyIsLikeForCSW("dc:type", SearchingContent.resourceTypeValue_CLH_Datasets);
                                Or.Add(PropertyIsLike0);
                                XElement PropertyIsLike1 = createFiltereContent_PropertyIsLikeForCSW("dc:type", SearchingContent.resourceTypeValue_GOS_Datasets);
                                Or.Add(PropertyIsLike1);
                            }
                            else
                            {
                                XElement PropertyIsLike = null;
                                if (CSWURL.Equals(CLHCSWURLString))
                                    PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:ServiceType", resourceType);
                                else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                                    PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", SearchingContent.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization);
                                Or.Add(PropertyIsLike);
                            }
                        }
                        And.Add(Or);
                    }
                    if (rules.providers.Count > 0)
                    {
                        XElement Or = new XElement(XName.Get("Or", ogc_namespace));
                        foreach (string provider in rules.providers)
                        {
                            if (CSWURL.Equals(CLHCSWURLString))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("OrganisationName", "*" + provider + "*");
                                Or.Add(PropertyIsLike);
                                XElement PropertyIsLike1 = createFiltereContent_PropertyIsLikeForCSW("dc:organization", "*" + provider + "*");
                                Or.Add(PropertyIsLike1);
                            }
                            else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                            {
                                XElement PropertyIsLike1 = createFiltereContent_PropertyIsLikeForCSW("apiso:OrganizationName", "*" + provider + "*");
                                Or.Add(PropertyIsLike1);
                            }
                        }
                        And.Add(Or);
                    }
                    foreach (string keyword in rules.Keywords)
                    {
                        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("anyText", keyword);
                        And.Add(PropertyIsLike);
                    }
                    if (rules.Keywords_eitherOne.Count > 0)
                    {
                        XElement Or = new XElement(XName.Get("Or", ogc_namespace));
                        foreach (string keyword in rules.Keywords_eitherOne)
                        {
                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("anyText", keyword);
                            Or.Add(PropertyIsLike);
                        }
                        And.Add(Or);
                    }
                }
                Filter.Add(And);
            }
            string postRequest = doc1.ToString();

            resultString = BaseHttpFunctions.HttpPost(CSWURL, postRequest);



            //jizhe
           // System.Diagnostics.Debug.WriteLine(postRequest);

          //  System.Diagnostics.Debug.WriteLine("==============================================================");

          //  System.Diagnostics.Debug.WriteLine(resultString);
           

            results = getRecordsFromResponseXML(CSWURL, resultString);

            getOtherInfo(results, searchingContent, queryServicePerformanceScoreAtServerSide, calculateRelevanceAtServerSide);
            return results;
        }

        [OperationContract]
        public CSWGetRecordsSearchResults getRecords_QuickSearchBySBA(SBAVocabulary vocabulary, string startPosition, string maxRecords, string CSWURL, bool queryServicePerformanceScoreAtServerSide, bool calculateRelevanceAtServerSide)
        {
            CSWGetRecordsSearchResults results = null;
            if (vocabulary == null || vocabulary.SBAVocabularyID.Trim().Equals(""))
                return null;

            XDocument doc1 = new XDocument();
            if (CSWURL.Equals(CLHCSWURLString) || CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
            {
                XElement GetRecords = new XElement(XName.Get("GetRecords", csw_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "csw", csw_namespace));
                //GetRecords.SetAttributeValue("xmlns", csw_namespace);
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gmd", gmd_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "ogc", ogc_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gml", gml_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "rim", rim_namespace));

                GetRecords.SetAttributeValue("service", "CSW");
                GetRecords.SetAttributeValue("version", "2.0.2");
                GetRecords.SetAttributeValue("outputFormat", "application/xml");
                GetRecords.SetAttributeValue("outputSchema", csw_namespace);
                GetRecords.SetAttributeValue("resultType", resultType);
                GetRecords.SetAttributeValue("maxRecords", maxRecords);
                GetRecords.SetAttributeValue("startPosition", startPosition);

                XElement Query = new XElement(XName.Get("Query", csw_namespace));
                Query.SetAttributeValue("typeNames", "csw:Record");
                XElement ElementSetName = new XElement(XName.Get("ElementSetName", csw_namespace));
                ElementSetName.SetValue(elementSetName);
                XElement Constraint = new XElement(XName.Get("Constraint", csw_namespace));
                Constraint.SetAttributeValue("version", "1.1.0");
                XElement Filter = new XElement(XName.Get("Filter", ogc_namespace));
                XElement And = new XElement(XName.Get("And", ogc_namespace));

                Constraint.Add(Filter);
                Query.Add(ElementSetName);
                Query.Add(Constraint);
                GetRecords.Add(Query);
                doc1.Add(GetRecords);

                if (Filter != null)
                {
                    List<string> addlist = new List<string>();
                    List<string> orlist = new List<string>();
                    if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture))
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture_EconomyTrade))
                    {
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                        orlist.Add("Economy");
                        orlist.Add("Trade");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture_Fisheries))
                    {
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                        orlist.Add("Fisheries");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture_FoodSecurity))
                    {
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                        orlist.Add("Food Security");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture_GrazingSystems))
                    {
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                        orlist.Add("Grazing Systems");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Agriculture_TimberFuelFiber))
                    {
                        addlist.Add(SBAVocabulary.SBA_Agriculture);
                        orlist.Add("Timber");
                        orlist.Add("Fuel");
                        orlist.Add("Fiber");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Biodiversity))
                        addlist.Add(SBAVocabulary.SBA_Biodiversity);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Biodiversity_Conservation))
                    {
                        addlist.Add(SBAVocabulary.SBA_Biodiversity);
                        orlist.Add("Conservation");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Biodiversity_InvasiveSpecies))
                    {
                        addlist.Add(SBAVocabulary.SBA_Biodiversity);
                        orlist.Add("Invasive Species");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Biodiversity_MigratorySpecies))
                    {
                        addlist.Add(SBAVocabulary.SBA_Biodiversity);
                        orlist.Add("Migratory Species");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Biodiversity_NaturalResources))
                    {
                        addlist.Add(SBAVocabulary.SBA_Biodiversity);
                        orlist.Add("Natural Resources");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate))
                        addlist.Add(SBAVocabulary.SBA_Climate);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate_AdaptingTo))
                    {
                        addlist.Add(SBAVocabulary.SBA_Climate);
                        orlist.Add("Adapting to");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate_Assessing))
                    {
                        addlist.Add(SBAVocabulary.SBA_Climate);
                        orlist.Add("Assessing");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate_Mitigating))
                    {
                        addlist.Add(SBAVocabulary.SBA_Climate);
                        orlist.Add("Mitigating");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate_Predicting))
                    {
                        addlist.Add(SBAVocabulary.SBA_Climate);
                        orlist.Add("Predicting");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Climate_Understanding))
                    {
                        addlist.Add(SBAVocabulary.SBA_Climate);
                        orlist.Add("Understanding");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters))
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_CoastalHazards))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Coastal Hazards");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_Earthquakes))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Earthquakes");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_ExtremeWeather))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Extreme Weather");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_Flood))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Flood");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_Landslides))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Landslides");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_PollutionEvents))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Pollution Events");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_SeaAndLakeIce))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Sea");
                        orlist.Add("Lake");
                        orlist.Add("Ice");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_TropicalCyclones))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Tropical Cyclones");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_TropicalCyclones))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Tropical Cyclones");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_Volcanoes))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Volcanoes");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Disasters_WildlandFires))
                    {
                        addlist.Add(SBAVocabulary.SBA_Disasters);
                        orlist.Add("Wildland Fires");
                    }


                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Ecosystems))
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Ecosystems_AgricultureFisheriesForestry))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Agriculture");
                        orlist.Add("Fisheries");
                        orlist.Add("Forestry");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Ecosystems_CarbonCycle))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Carbon Cycle");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Ecosystems_LandRiverCoastOcean))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Land");
                        orlist.Add("River");
                        orlist.Add("Coast");
                        orlist.Add("Ocean");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy))
                        addlist.Add(SBAVocabulary.SBA_Energy);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy_ElectricityGeneration))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Electricity Generation");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy_GlobalEnergy))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Global Energy");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy_OilGas))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Oil");
                        orlist.Add("Gas");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy_RefiningTransport))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Refining");
                        orlist.Add("Transport");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Energy_RenewableEnergy))
                    {
                        addlist.Add(SBAVocabulary.SBA_Ecosystems);
                        orlist.Add("Renewable Energy");
                    }


                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health))
                        addlist.Add(SBAVocabulary.SBA_Health);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_Accidentals))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Accidentals");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_BirthDefect))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Birth Defect");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_Cancer))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Cancer");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_EnvironmentalStress))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Environmental Stress");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_InfectiousDiseases))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Infectious Diseases");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_Nutrition))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Nutrition");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Health_RespiratoryProblems))
                    {
                        addlist.Add(SBAVocabulary.SBA_Health);
                        orlist.Add("Respiratory Problems");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water))
                        addlist.Add(SBAVocabulary.SBA_Water);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_Biogeochemistry))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Biogeochemistry");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_ClimatePrediction))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Climate Prediction");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_DroughtPrediction))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Drought Prediction");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_Ecosystem))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Ecosystem");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_FisheriesAndHabitat))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Fisheries");
                        orlist.Add("Habitat");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_FloodPrediction))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Flood Prediction");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_HumanHealth))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Human Health");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_ImpactsOfHumans))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Impacts of Humans");
                        orlist.Add("Humans");
                        orlist.Add("Impacts");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_LandUsePlanning))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Land Use Planning");
                        orlist.Add("Land Use");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_Management))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Management");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_ProductionOfFood))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Production of Food");
                        orlist.Add("Food");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_ResourceManagement))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Resource Management");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_TelecomunicationNavigation))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Telecomunication Navigation");
                        orlist.Add("Telecomunication");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_WaterCycle))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Water Cycle");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Water_WeatherPrediction))
                    {
                        addlist.Add(SBAVocabulary.SBA_Water);
                        orlist.Add("Weather Prediction");
                    }

                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather))
                        addlist.Add(SBAVocabulary.SBA_Weather);
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather_Extended10_30days))
                    {
                        addlist.Add(SBAVocabulary.SBA_Weather);
                        orlist.Add("Extended 10 - 30 days");
                        orlist.Add("Extended");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather_Extended10_30days))
                    {
                        addlist.Add(SBAVocabulary.SBA_Weather);
                        orlist.Add("Nowcasting 0 - 2 hs");
                        orlist.Add("Nowcasting");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather_MediumRange3_10days))
                    {
                        addlist.Add(SBAVocabulary.SBA_Weather);
                        orlist.Add("Medium Range 3 - 10 days");
                        orlist.Add("Medium Range");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather_Nowcasting0_2hs))
                    {
                        addlist.Add(SBAVocabulary.SBA_Weather);
                        orlist.Add("Nowcasting 0 - 2 hs");
                        orlist.Add("Nowcasting");
                    }
                    else if (vocabulary.SBAVocabularyID.Equals(SBAVocabulary.SBA_Weather_ShortRange2_72hs))
                    {
                        addlist.Add(SBAVocabulary.SBA_Weather);
                        orlist.Add("Short Range 2 - 72 hs");
                        orlist.Add("Short Range");
                    }

                    foreach (string keyword in addlist)
                    {
                        if (!keyword.Trim().Equals(""))
                        {
                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:subject", keyword);
                            And.Add(PropertyIsLike);
                        }
                    }
                    if (orlist.Count > 0)
                    {
                        XElement Or1 = new XElement(XName.Get("Or", ogc_namespace));
                        foreach (string keyword in orlist)
                        {
                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:subject", keyword);
                            Or1.Add(PropertyIsLike);
                        }
                        And.Add(Or1);
                    }
                }
                Filter.Add(And);
            }
            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(CSWURL, postRequest);
            results = getRecordsFromResponseXML(CSWURL, resultString);

            getOtherInfo(results, vocabulary.SBAVocabularyID, queryServicePerformanceScoreAtServerSide, calculateRelevanceAtServerSide);
            return results;
        }

        [OperationContract]
        public CSWGetRecordsSearchResults getRecords_AdvancedSearch(SearchingContent searchingContent, string startPosition, string maxRecords, string CSWURL, bool queryServicePerformanceScoreAtServerSide, bool calculateRelevanceAtServerSide)
        {
            CSWGetRecordsSearchResults results = null;
            XDocument doc1 = new XDocument();

            if (CSWURL.Equals(CLHCSWURLString) || CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
            {
                XElement GetRecords = new XElement(XName.Get("GetRecords", csw_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "csw", csw_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gmd", gmd_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "ogc", ogc_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "gml", gml_namespace));
                GetRecords.Add(new XAttribute(XNamespace.Xmlns + "rim", rim_namespace));

                GetRecords.SetAttributeValue("service", "CSW");
                GetRecords.SetAttributeValue("version", "2.0.2");
                GetRecords.SetAttributeValue("outputFormat", "application/xml");
                GetRecords.SetAttributeValue("outputSchema", csw_namespace);
                GetRecords.SetAttributeValue("resultType", resultType);
                GetRecords.SetAttributeValue("maxRecords", maxRecords);
                GetRecords.SetAttributeValue("startPosition", startPosition);

                XElement Query = new XElement(XName.Get("Query", csw_namespace));
                Query.SetAttributeValue("typeNames", "csw:Record");
                XElement ElementSetName = new XElement(XName.Get("ElementSetName", csw_namespace));
                ElementSetName.SetValue(elementSetName);
                XElement Constraint = new XElement(XName.Get("Constraint", csw_namespace));
                Constraint.SetAttributeValue("version", "1.1.0");
                XElement Filter = new XElement(XName.Get("Filter", ogc_namespace));
                XElement And = new XElement(XName.Get("And", ogc_namespace));

                Constraint.Add(Filter);
                Query.Add(ElementSetName);
                Query.Add(Constraint);
                GetRecords.Add(Query);
                doc1.Add(GetRecords);

                if (Filter != null)
                {
                    //process the words must emerged in fulltext
                    if (searchingContent.wordsInAnyText != null)
                        foreach (string word in searchingContent.wordsInAnyText)
                        {
                            if (!word.Trim().Equals(""))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("anyText", word);
                                And.Add(PropertyIsLike);
                            }
                        }

                    //process the words must emerged in exact phrase
                    if (searchingContent.exactPhrase != null && !searchingContent.exactPhrase.Trim().Equals(""))
                    {
                        string word = searchingContent.exactPhrase;
                        if (!word.Trim().Equals(""))
                        {
                            if (CSWURL.Equals(CLHCSWURLString))
                            {
                                XElement PropertyIsEqualTo = createFiltereContent_PropertyIsLikeOrPropertyEqualToForCSW("PropertyIsEqualTo", "anyText", "\"" + word + "\"", true, "One");
                                And.Add(PropertyIsEqualTo);
                            }
                            else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeOrPropertyEqualToForCSW("PropertyIsLike", "anyText", "\"" + word + "\"", true, "One");
                                And.Add(PropertyIsLike);
                            }
                        }
                    }

                    //process the words must emerged in title
                    if (searchingContent.wordsInTitle != null)
                        foreach (string word in searchingContent.wordsInTitle)
                        {
                            if (!word.Trim().Equals(""))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:title", word);
                                And.Add(PropertyIsLike);
                            }
                        }

                    //process the words must emerged in abstract
                    if (searchingContent.wordsInAbstract != null)
                        foreach (string word in searchingContent.wordsInAbstract)
                        {
                            if (!word.Trim().Equals(""))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dct:abstract", word);
                                And.Add(PropertyIsLike);
                            }
                        }

                    //process the words must emerged in keywords
                    if (searchingContent.Keywords != null)
                        foreach (string word in searchingContent.Keywords)
                        {
                            if (!word.Trim().Equals(""))
                            {
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:subject", word);
                                And.Add(PropertyIsLike);
                            }
                        }

                    //process the words must not emerged in fulltext
                    if (searchingContent.withoutWordsInAnytext != null)
                        foreach (string word in searchingContent.withoutWordsInAnytext)
                        {
                            if (!word.Trim().Equals(""))
                            {
                                XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("anyText", word);
                                Not.Add(PropertyIsLike);
                                And.Add(Not);
                            }
                        }

                    if (!searchingContent.resourceTypesTree.isSelected)
                    {
                        //if there is no resource type is selected, it is forbidden to get any records
                        if (searchingContent.isNoResourceTypeSelected == true)
                        {
                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", "unkown");
                            And.Add(PropertyIsLike);
                        }
                        else
                        {
                            XElement Or = new XElement(XName.Get("Or", ogc_namespace));
                            foreach (ResourceType tree in searchingContent.resourceTypesTree.Children)
                            {
                                if (tree.isSelected == true)
                                {
                                    if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_PickID_AllServices))
                                    {
                                        Boolean allSelect = true;
                                        List<string> allInclude = new List<string>();
                                        List<string> allExclude = new List<string>();
                                        List<string> allExcludeOGCServiceType = new List<string>();
                                        List<string> allIncludeOGCServiceType = new List<string>();
                                        if (CSWURL.Equals(CLHCSWURLString))
                                        {
                                            allInclude.Add(SearchingContent.resourceTypeValue_CLH_Services);
                                            foreach (ResourceType tree1 in tree.Children)
                                            {
                                                if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_AnalysisAndVisualization))
                                                {
                                                    if (tree1.isSelected == false)
                                                    {
                                                        //allInclude.Clear();
                                                        //break;
                                                        allSelect = false;
                                                    }
                                                    else
                                                    {
                                                        foreach (ResourceType tree2 in tree1.Children)
                                                        {
                                                            if (tree2.isSelected == false)
                                                            {
                                                                allExcludeOGCServiceType.Add(tree2.ResourceTypeID);
                                                                allSelect = false;
                                                            }
                                                            else
                                                            {
                                                                allIncludeOGCServiceType.Add(tree2.ResourceTypeID);
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_AlertsRSSAndInformationFeeds))
                                                {
                                                }
                                                else if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_CatalogRegistryOrMetadataCollection))
                                                {
                                                    if (tree1.isSelected == false)
                                                    {
                                                        allExcludeOGCServiceType.Add(SearchingContent.ServiceType_CSW);
                                                        allSelect = false;
                                                    }
                                                    else
                                                    {
                                                        allIncludeOGCServiceType.Add(SearchingContent.ServiceType_CSW);
                                                    }
                                                }
                                            }
                                            //foreach (string value in allInclude)
                                            //{
                                            //    XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", value);
                                            //    Or.Add(PropertyIsLike);
                                            //}

                                            if (!allSelect)
                                            {
                                                if (allIncludeOGCServiceType.Count > 0)
                                                {
                                                    XElement Or1 = new XElement(XName.Get("Or", ogc_namespace));
                                                    foreach (string value in allIncludeOGCServiceType)
                                                    {
                                                        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:ServiceType", value);
                                                        Or1.Add(PropertyIsLike);
                                                    }
                                                    //And.Add(Or1);

                                                    XElement And1 = new XElement(XName.Get("And", ogc_namespace));
                                                    XElement PropertyIsLike1 = createFiltereContent_PropertyIsLikeForCSW("dc:type", SearchingContent.resourceTypeValue_CLH_Services);
                                                    And1.Add(PropertyIsLike1);
                                                    And1.Add(Or1);
                                                    Or.Add(And1);
                                                    //Or.Add(PropertyIsLike);
                                                }
                                            }
                                            else
                                            {
                                                //if (allExcludeOGCServiceType.Count > 0)
                                                //{
                                                //    foreach (string value in allExcludeOGCServiceType)
                                                //    {
                                                //        XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                                                //        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:ServiceType", value);
                                                //        Not.Add(PropertyIsLike);
                                                //        And.Add(Not);
                                                //    }
                                                //}
                                                XElement PropertyIsLike1 = createFiltereContent_PropertyIsLikeForCSW("dc:type", SearchingContent.resourceTypeValue_CLH_Services);
                                                Or.Add(PropertyIsLike1);
                                            }

                                        }
                                        else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                                        {
                                            foreach (ResourceType tree1 in tree.Children)
                                            {
                                                if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_AnalysisAndVisualization))
                                                {
                                                    if (!SearchingContent.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization.Trim().Equals(""))
                                                    {
                                                        if (tree1.isSelected == true)
                                                            allInclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization);
                                                        else
                                                            allExclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization);
                                                    }
                                                }
                                                else if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_AlertsRSSAndInformationFeeds))
                                                {
                                                    if (!SearchingContent.resourceTypeValue_GOS_DataServices_AlertsRSSAndInformationFeeds.Trim().Equals(""))
                                                    {
                                                        if (tree1.isSelected == true)
                                                            allInclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_AlertsRSSAndInformationFeeds);
                                                        else
                                                            allExclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_AlertsRSSAndInformationFeeds);
                                                    }
                                                }
                                                else if (tree1.ResourceTypeID.Equals(SearchingContent.resourceType_DataServices_CatalogRegistryOrMetadataCollection))
                                                {
                                                    if (!SearchingContent.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection.Trim().Equals(""))
                                                    {
                                                        if (tree1.isSelected == true)
                                                            allInclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection);
                                                        else
                                                            allExclude.Add(SearchingContent.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection);
                                                    }
                                                }
                                            }
                                            foreach (string value in allInclude)
                                            {
                                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", value);
                                                Or.Add(PropertyIsLike);
                                            }

                                            if (allExclude.Count > 0)
                                            {
                                                foreach (string value in allExclude)
                                                {
                                                    XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                                                    XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", value);
                                                    Not.Add(PropertyIsLike);
                                                    And.Add(Not);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string queryKeywords = null;
                                        if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Datasets))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_Datasets;
                                            else if (CSWURL.Equals(CLHCSWURLString))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_Datasets;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_MonitoringAndObservationSystems))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) && (!SearchingContent.resourceTypeValue_GOS_MonitoringAndObservationSystems.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_MonitoringAndObservationSystems;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_MonitoringAndObservationSystems.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_MonitoringAndObservationSystems;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_ComputationalModel))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) && (!SearchingContent.resourceTypeValue_GOS_ComputationalModel.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_ComputationalModel;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_ComputationalModel.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_ComputationalModel;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Initiatives))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) && (!SearchingContent.resourceTypeValue_GOS_Initiatives.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_Initiatives;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_Initiatives.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_Initiatives;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_WebsitesAndDocuments))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) && (!SearchingContent.resourceTypeValue_GOS_WebsitesAndDocuments.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_WebsitesAndDocuments;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_WebsitesAndDocuments.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_WebsitesAndDocuments;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_SoftwareAndApplications))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) && (!SearchingContent.resourceTypeValue_GOS_SoftwareAndApplications.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_SoftwareAndApplications;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_SoftwareAndApplications.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_SoftwareAndApplications;
                                        }
                                        if (queryKeywords != null && !queryKeywords.Trim().Equals(""))
                                        {
                                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", queryKeywords);
                                            Or.Add(PropertyIsLike);
                                        }
                                    }
                                }
                                else
                                {
                                    string queryKeywords = null;
                                    if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_PickID_AllServices))
                                    {
                                        List<string> queryKeywordsList = new List<string>();

                                        if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                                        {
                                            queryKeywordsList.Add(SearchingContent.resourceTypeValue_GOS_DataServices_AnalysisAndVisualization);
                                            queryKeywordsList.Add(SearchingContent.resourceTypeValue_GOS_DataServices_CatalogRegistryOrMetadataCollection);
                                        }
                                        else if (CSWURL.Equals(CLHCSWURLString))
                                        {
                                            queryKeywordsList.Add(SearchingContent.resourceTypeValue_CLH_Services);
                                        }
                                        foreach (string queryKeyword in queryKeywordsList)
                                        {
                                            if (queryKeywords != null && !queryKeywords.Trim().Equals(""))
                                            {
                                                XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                                                XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", queryKeyword);
                                                Not.Add(PropertyIsLike);
                                                And.Add(Not);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Datasets))
                                        {
                                            if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_Datasets;
                                            else if (CSWURL.Equals(CLHCSWURLString))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_Datasets;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_MonitoringAndObservationSystems))
                                        {
                                            if ((CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString)) && (!SearchingContent.resourceTypeValue_GOS_MonitoringAndObservationSystems.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_MonitoringAndObservationSystems;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_MonitoringAndObservationSystems.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_MonitoringAndObservationSystems;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_ComputationalModel))
                                        {
                                            if ((CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString)) && (!SearchingContent.resourceTypeValue_GOS_ComputationalModel.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_ComputationalModel;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_ComputationalModel.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_ComputationalModel;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Initiatives))
                                        {
                                            if ((CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString)) && (!SearchingContent.resourceTypeValue_GOS_Initiatives.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_Initiatives;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_Initiatives.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_Initiatives;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_WebsitesAndDocuments))
                                        {
                                            if ((CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString)) && (!SearchingContent.resourceTypeValue_GOS_WebsitesAndDocuments.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_WebsitesAndDocuments;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_WebsitesAndDocuments.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_WebsitesAndDocuments;
                                        }
                                        else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_SoftwareAndApplications))
                                        {
                                            if ((CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString)) && (!SearchingContent.resourceTypeValue_GOS_SoftwareAndApplications.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_GOS_SoftwareAndApplications;
                                            else if (CSWURL.Equals(CLHCSWURLString) && (!SearchingContent.resourceTypeValue_CLH_SoftwareAndApplications.Trim().Equals("")))
                                                queryKeywords = SearchingContent.resourceTypeValue_CLH_SoftwareAndApplications;
                                        }
                                        if (queryKeywords != null && !queryKeywords.Trim().Equals(""))
                                        {
                                            XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                                            XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:type", queryKeywords);
                                            Not.Add(PropertyIsLike);
                                            And.Add(Not);
                                        }
                                    }

                                }
                            }
                            And.Add(Or);
                        }
                    }

                    //process the resourceIdentificationKeywords Data CORE
                    if (searchingContent.resourceIdentificationKeywords_GEOSSDataCORE.Equals(SearchingContent.resourceIdentificationKeywords_GEOSSDataCORE_ONLY))
                    {
                        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:subject", resourceIdentificationKeywords_geossDateCore);
                        And.Add(PropertyIsLike);
                    }
                    else if (searchingContent.resourceIdentificationKeywords_GEOSSDataCORE.Equals(SearchingContent.resourceIdentificationKeywords_GEOSSDataCORE_NON))
                    {
                        XElement Not = new XElement(XName.Get("Not", ogc_namespace));
                        XElement PropertyIsLike = createFiltereContent_PropertyIsLikeForCSW("dc:subject", resourceIdentificationKeywords_geossDateCore);
                        Not.Add(PropertyIsLike);
                        And.Add(Not);
                    }

                    //process the where
                    if (!searchingContent.where_isAnyWhere)
                    {
                        string relationship = searchingContent.where_Relationship;
                        string Region = searchingContent.where_Region;
                        if (!relationship.Trim().Equals(""))
                        {
                            if (CSWURL.Equals(CLHCSWURLString))
                            {
                                XElement relationshipElement = new XElement(XName.Get(relationship, ogc_namespace));
                                XElement Envelope = new XElement(XName.Get("Envelope", gml_namespace));
                                XElement lowerCorner = new XElement(XName.Get("lowerCorner", gml_namespace));
                                XElement upperCorner = new XElement(XName.Get("upperCorner", gml_namespace));

                                string north = searchingContent.where_North;
                                string south = searchingContent.where_South;
                                string west = searchingContent.where_West;
                                string east = searchingContent.where_East;

                                string lowerCorner_string = west + " " + south;
                                string upperCorner_string = east + " " + north;

                                lowerCorner.SetValue(lowerCorner_string);
                                upperCorner.SetValue(upperCorner_string);
                                Envelope.Add(lowerCorner);
                                Envelope.Add(upperCorner);

                                XElement PropertyName = new XElement(XName.Get("PropertyName", ogc_namespace));
                                PropertyName.SetValue("iso:BoundingBox");

                                relationshipElement.Add(PropertyName);
                                relationshipElement.Add(Envelope);
                                And.Add(relationshipElement);
                            }
                            else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                            {
                                XElement relationshipElement = new XElement(XName.Get(relationship, ogc_namespace));
                                XElement Box = new XElement(XName.Get("Box", gml_namespace));
                                Box.Add(new XAttribute(XNamespace.Xmlns + "gml", gml_namespace));
                                XElement coordinates = new XElement(XName.Get("coordinates", gml_namespace));
                                coordinates.SetAttributeValue("decimal", ".");
                                coordinates.SetAttributeValue("cs", ",");
                                coordinates.SetAttributeValue("ts", " ");

                                string north = searchingContent.where_North;
                                string south = searchingContent.where_South;
                                string west = searchingContent.where_West;
                                string east = searchingContent.where_East;

                                string coordinates_string = south + "," + west + "," + north + "," + east;

                                coordinates.SetValue(coordinates_string);
                                Box.Add(coordinates);

                                XElement PropertyName = new XElement(XName.Get("PropertyName", ogc_namespace));
                                PropertyName.SetValue("ows:BoundingBox");

                                relationshipElement.Add(PropertyName);
                                relationshipElement.Add(Box);
                                And.Add(relationshipElement);
                            }
                        }
                        else if (!Region.Trim().Equals(""))
                        {

                        }
                    }

                    //process the when
                    if (!searchingContent.when_isAnytime)
                    {
                        string TimeType = searchingContent.when_TimeType;
                        if (!TimeType.Trim().Equals(""))
                        {
                            XElement PropertyIsGreaterThanOrEqualTo = new XElement(XName.Get("PropertyIsGreaterThanOrEqualTo", ogc_namespace));
                            XElement PropertyName = new XElement(XName.Get("PropertyName", ogc_namespace));
                            if (TimeType.Equals(SearchingContent.when_Type_TemporalExtent))
                                PropertyName.SetValue("TempExtent_begin");
                            else if (TimeType.Equals(SearchingContent.when_Type_MetadataChangeDate))
                                PropertyName.SetValue("Modified");
                            XElement Literal = new XElement(XName.Get("Literal", ogc_namespace));
                            Literal.SetValue(searchingContent.when_FromTime);
                            PropertyIsGreaterThanOrEqualTo.Add(PropertyName);
                            PropertyIsGreaterThanOrEqualTo.Add(Literal);


                            XElement PropertyIsLessThanOrEqualTo = new XElement(XName.Get("PropertyIsLessThanOrEqualTo", ogc_namespace));
                            XElement PropertyName1 = new XElement(XName.Get("PropertyName", ogc_namespace));
                            if (TimeType.Equals(SearchingContent.when_Type_TemporalExtent))
                                PropertyName1.SetValue("TempExtent_end");
                            else if (TimeType.Equals(SearchingContent.when_Type_MetadataChangeDate))
                                PropertyName1.SetValue("Modified");
                            XElement Literal1 = new XElement(XName.Get("Literal", ogc_namespace));
                            Literal1.SetValue(searchingContent.when_ToTime);
                            PropertyIsLessThanOrEqualTo.Add(PropertyName1);
                            PropertyIsLessThanOrEqualTo.Add(Literal1);

                            And.Add(PropertyIsGreaterThanOrEqualTo);
                            And.Add(PropertyIsLessThanOrEqualTo);
                        }
                    }
                }
                Filter.Add(And);
            }
            else if (CSWURL.Equals(CSRCSWURLString))
            {
                foreach (ResourceType tree in searchingContent.resourceTypesTree.Children)
                {
                    string queryKeywords = null;
                    queryKeywords += "";
                    if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Datasets))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_Datasets;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_MonitoringAndObservationSystems))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_MonitoringAndObservationSystems;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_ComputationalModel))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_ComputationalModel;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Initiatives))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_Initiatives;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_WebsitesAndDocuments))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_WebsitesAndDocuments;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_SoftwareAndApplications))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_SoftwareAndApplications;
                    }
                    else if (tree.ResourceTypeID.Equals(SearchingContent.resourceType_Datasets))
                    {
                        queryKeywords = SearchingContent.resourceTypeValue_CSR_Datasets;
                    }
                }
            }

            string postRequest = doc1.ToString();
            resultString = BaseHttpFunctions.HttpPost(CSWURL, postRequest);
            results = getRecordsFromResponseXML(CSWURL, resultString);
            getOtherInfo(results, searchingContent, queryServicePerformanceScoreAtServerSide, calculateRelevanceAtServerSide);
            return results;
        }


        private XElement createFiltereContent_PropertyIsLikeForCSW(string propertyNameString, string queryKeywords)
        {
            XElement PropertyIsLike = new XElement(XName.Get("PropertyIsLike", ogc_namespace));
            PropertyIsLike.SetAttributeValue("escapeChar", "\\");
            PropertyIsLike.SetAttributeValue("singleChar", "?");
            PropertyIsLike.SetAttributeValue("wildCard", "*");

            XElement PropertyName = new XElement(XName.Get("PropertyName", ogc_namespace));
            PropertyName.SetValue(propertyNameString);

            XElement Literal = new XElement(XName.Get("Literal", ogc_namespace));
            Literal.SetValue(queryKeywords);

            PropertyIsLike.Add(PropertyName);
            PropertyIsLike.Add(Literal);
            return PropertyIsLike;
        }

        private XElement createFiltereContent_PropertyIsLikeOrPropertyEqualToForCSW(string FilterName, string propertyNameString, string queryKeywords, bool needMatchMethod, string matchMethod)
        {
            XElement FilterFunction = new XElement(XName.Get(FilterName, ogc_namespace));
            FilterFunction.SetAttributeValue("escapeChar", "\\");
            FilterFunction.SetAttributeValue("singleChar", "?");
            FilterFunction.SetAttributeValue("wildCard", "*");
            if (needMatchMethod)
                FilterFunction.SetAttributeValue("matchAction", matchMethod);
            XElement PropertyName = new XElement(XName.Get("PropertyName", ogc_namespace));
            PropertyName.SetValue(propertyNameString);

            XElement Literal = new XElement(XName.Get("Literal", ogc_namespace));
            Literal.SetValue(queryKeywords);

            FilterFunction.Add(PropertyName);
            FilterFunction.Add(Literal);
            return FilterFunction;
        }

        private CSWGetRecordsSearchResults getRecordsFromResponseXML(string CSWURL, string response)
        {
            int num = 0;
            CSWGetRecordsSearchResults results = new CSWGetRecordsSearchResults();
            results.MetadataRepositoryURL = CSWURL;
            if (response == null || response.Trim().Equals(""))
            {
                results.numberOfRecordsMatched = 0;
                results.numberOfRecordsReturned = 0;
                results.recordList = null;
                results.SearchStatus = "Connect error or timeout...";
                return results;
            }

            if (CSWURL.Equals(CLHCSWURLString) || CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
            {
                XDocument doc = null;
                try
                {
                    doc = XDocument.Parse(response);
                }
                catch (Exception e)
                {
                    e.ToString();
                    doc = null;
                }
                if (doc != null)
                {
                    XElement rootElement = doc.Root;
                    if (!rootElement.Name.LocalName.Equals("GetRecordsResponse"))
                    {
                        results.numberOfRecordsMatched = 0;
                        results.numberOfRecordsReturned = 0;
                        results.recordList = null;
                        results.SearchStatus = "Search response error...";
                        return results;
                    }

                    XElement SearchResults = rootElement.Element(XName.Get("SearchResults", csw_namespace));
                    if (SearchResults != null)
                    {
                        XAttribute numberOfRecordsMatched = SearchResults.Attribute("numberOfRecordsMatched");
                        string numString = numberOfRecordsMatched.Value;
                        results.numberOfRecordsMatched = int.Parse(numString, System.Globalization.NumberStyles.AllowDecimalPoint);

                        XAttribute numberOfRecordsReturned = SearchResults.Attribute("numberOfRecordsReturned");
                        string numOfReturnString = numberOfRecordsReturned.Value;
                        results.numberOfRecordsReturned = int.Parse(numOfReturnString, System.Globalization.NumberStyles.AllowDecimalPoint);
                        num = results.numberOfRecordsReturned;

                        XAttribute nextRecord = SearchResults.Attribute("nextRecord");
                        string nextRecordString = nextRecord.Value;
                        results.nextRecord = int.Parse(nextRecordString, System.Globalization.NumberStyles.AllowDecimalPoint);

                        XAttribute elementSet = SearchResults.Attribute("elementSet");
                        string elementSetString = elementSet.Value;
                        results.elementSet = elementSetString;
                    }
                    if (num > 0)
                    {
                        List<Record> Records = new List<Record>();
                        results.recordList = Records;
                        IEnumerable<XElement> recordsList = SearchResults.Elements(XName.Get("Record", csw_namespace));
                        foreach (XElement recordElement in recordsList)
                        {
                            string id = null;
                            string title = "unknown";
                            string type = "unknown";
                            string source = "Ocean CI";
                            string accessURL = "unknown";
                            string description = "unknown";
                            string provider = "unknown";
                            string MetadataAccessURL = "";
                            double performanceSummary = -1.0;
                            List<string> formats = null;
                            List<string> descriptiveKeywords = null;
                            bool isDataCore = false;
                            BBox bbox = null;
                            List<string> SBAs = null;

                            XElement idElement = recordElement.Element(XName.Get("identifier", dc_namespace));
                            if (idElement != null && idElement.Value != null && !idElement.Value.Trim().Equals(""))
                                id = idElement.Value;
                            XElement titleElement = recordElement.Element(XName.Get("title", dc_namespace));
                            if (titleElement != null && titleElement.Value != null && !titleElement.Value.Trim().Equals(""))
                                title = titleElement.Value;
                            XElement typeElement = recordElement.Element(XName.Get("type", dc_namespace));
                            if (typeElement != null && typeElement.Value != null && !typeElement.Value.Trim().Equals(""))
                                type = typeElement.Value;
                            if (CSWURL.Equals(CLHCSWURLString))
                            {
                                XElement sourceElement = recordElement.Element(XName.Get("source", dc_namespace));
                                if (sourceElement != null && sourceElement.Value != null && !sourceElement.Value.Trim().Equals(""))
                                    provider = sourceElement.Value;
                                else
                                {
                                    XElement creatorElement = recordElement.Element(XName.Get("creator", dc_namespace));
                                    if (creatorElement != null && creatorElement.Value != null && !creatorElement.Value.Trim().Equals(""))
                                        provider = creatorElement.Value;
                                }

                                XElement urlElement = recordElement.Element(XName.Get("URI", dc_namespace));
                                if (urlElement != null && urlElement.Value != null && !urlElement.Value.Trim().Equals(""))
                                    accessURL = urlElement.Value;

                                MetadataAccessURL = CSWURL + "?&service=CSW&request=GetRecordById&version=2.0.2&ElementSetName=full&outputSchema=csw:IsoRecord&id=" + id;
                           //     source = "GEOSS Clearinghouse (CLH)";
                           //     source = "Ocean CI";
                                string[] tokens = accessURL.Split(new string[] { "/" }, StringSplitOptions.None);
                                if (tokens.Length > 2) {
                                    source = tokens[2];
                                }
                                

                                XElement organization = recordElement.Element(XName.Get("organization", dc_namespace));
                                if (organization != null && organization.Value != null && !organization.Value.Trim().Equals(""))
                                {
                                    provider = organization.Value;
                                }
                                XElement serviceType = recordElement.Element(XName.Get("ServiceType", dc_namespace));
                                if (serviceType != null && serviceType.Value != null && !serviceType.Value.Trim().Equals(""))
                                {
                                    if (serviceType.Value.Equals(SearchingContent.ServiceType_WMS))
                                        type = SearchingContent.ServiceType_WMS;
                                    else if (serviceType.Value.Equals(SearchingContent.ServiceType_WFS))
                                        type = SearchingContent.ServiceType_WFS;
                                    else if (serviceType.Value.Equals(SearchingContent.ServiceType_WCS))
                                        type = SearchingContent.ServiceType_WCS;
                                    else if (serviceType.Value.Equals(SearchingContent.ServiceType_WPS))
                                        type = SearchingContent.ServiceType_WPS;
                                    else if (serviceType.Value.Equals(SearchingContent.ServiceType_CSW))
                                        type = SearchingContent.ServiceType_CSW;
                                }

                                XElement BoundingBox = recordElement.Element(XName.Get("BoundingBox", ows_namespace));
                                if (BoundingBox != null)
                                {
                                    bbox = new BBox();
                                    if (BoundingBox.Attribute("crs") != null)
                                        bbox.BBox_CRS = BoundingBox.Attribute("crs").Value;
                                    else
                                        bbox.BBox_CRS = "WGS84";
                                    XElement LowerCorner = BoundingBox.Element(XName.Get("LowerCorner", ows_namespace));
                                    XElement UpperCorner = BoundingBox.Element(XName.Get("UpperCorner", ows_namespace));
                                    if (LowerCorner != null && !LowerCorner.Value.Trim().Equals(""))
                                    {
                                        String[] values = LowerCorner.Value.Split(' ');
                                        try
                                        {
                                            double lon = double.Parse(values[0]);
                                            double lat = double.Parse(values[1]);
                                            bbox.BBox_Lower_Lon = lon;
                                            bbox.BBox_Lower_Lat = lat;
                                        }
                                        catch (Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }

                                    if (UpperCorner != null && !UpperCorner.Value.Trim().Equals(""))
                                    {
                                        String[] values = UpperCorner.Value.Split(' ');
                                        try
                                        {
                                            double lon = double.Parse(values[0]);
                                            double lat = double.Parse(values[1]);
                                            bbox.BBox_Upper_Lon = lon;
                                            bbox.BBox_Upper_Lat = lat;
                                        }
                                        catch (Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }
                                }
                            }
                            else if (CSWURL.Equals(GOSCSWURLString) || CSWURL.Equals(USGIN_AASG_CSWURLString))
                            {
                                XElement publisherElement = recordElement.Element(XName.Get("publisher", dc_namespace));
                                if (publisherElement != null && publisherElement.Value != null && !publisherElement.Value.Trim().Equals(""))
                                    provider = publisherElement.Value;

                                IEnumerable<XElement> references = recordElement.Elements(XName.Get("references", dct_namespace));
                                if (references != null)
                                {
                                    foreach (XElement reference in references)
                                    {
                                        string schemeValue = reference.Attribute(XName.Get("scheme")).Value;
                                        if (schemeValue.Equals(GOS_Reference_Server) && reference.Value != null && !reference.Value.Trim().Equals(""))
                                            accessURL = reference.Value;
                                        else if (schemeValue.Equals(GOS_Reference_Onlink) && reference.Value != null && !reference.Value.Trim().Equals(""))
                                        {
                                            if (accessURL.Equals("unknown"))
                                                accessURL = reference.Value;
                                        }
                                        else if (schemeValue.Equals(GOS_Reference_Document) && reference.Value != null && !reference.Value.Trim().Equals(""))
                                        {
                                            MetadataAccessURL = reference.Value;
                                            if (CSWURL.Equals(USGIN_AASG_CSWURLString))
                                            {
                                                int index1 = MetadataAccessURL.IndexOf("?getxml=");
                                                MetadataAccessURL = USGIN_AASG_CSWURLString + MetadataAccessURL.Substring(index1);
                                            }
                                        }
                                    }
                                }
                                source = "Geospatial One Stop (GOS)";
                                if(CSWURL.Equals(USGIN_AASG_CSWURLString))
                                    source = "USGIN AASG Geothermal Data Catalog";

                                XElement hasFormat = recordElement.Element(XName.Get("hasFormat", dct_namespace));
                                if (hasFormat != null && hasFormat.Value != null && !hasFormat.Value.Trim().Equals(""))
                                {
                                    if (hasFormat.Value.Equals("wms") && type.Equals("liveData"))
                                        type = SearchingContent.ServiceType_WMS;
                                    else if (hasFormat.Value.Equals("wfs"))
                                        type = SearchingContent.ServiceType_WFS;
                                    else if (hasFormat.Value.Equals("wcs"))
                                        type = SearchingContent.ServiceType_WCS;
                                    else if (hasFormat.Value.Equals("wps"))
                                        type = SearchingContent.ServiceType_WPS;
                                }

                                XElement WGS84BoundingBox = recordElement.Element(XName.Get("WGS84BoundingBox", ows_namespace));
                                if (WGS84BoundingBox == null)
                                {
                                    WGS84BoundingBox = recordElement.Element(XName.Get("BoundingBox", ows_namespace));
                                }
                                else
                                { 
                                    bbox = new BBox();
                                    bbox.BBox_CRS = "WGS84";
                                }
                                if (WGS84BoundingBox != null)
                                {
                                    if (bbox == null)
                                    {
                                        bbox = new BBox();
                                        bbox.BBox_CRS = "Unkown";
                                    }
                                    XElement LowerCorner = WGS84BoundingBox.Element(XName.Get("LowerCorner", ows_namespace));
                                    XElement UpperCorner = WGS84BoundingBox.Element(XName.Get("UpperCorner", ows_namespace));
                                    if (LowerCorner != null && !LowerCorner.Value.Trim().Equals(""))
                                    {
                                        String[] values = LowerCorner.Value.Split(' ');
                                        try
                                        {
                                            double lon = double.Parse(values[0]);
                                            double lat = double.Parse(values[1]);
                                            bbox.BBox_Lower_Lon = lon;
                                            bbox.BBox_Lower_Lat = lat;
                                        }
                                        catch (Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }

                                    if (UpperCorner != null && !UpperCorner.Value.Trim().Equals(""))
                                    {
                                        String[] values = UpperCorner.Value.Split(' ');
                                        try
                                        {
                                            double lon = double.Parse(values[0]);
                                            double lat = double.Parse(values[1]);
                                            bbox.BBox_Upper_Lon = lon;
                                            bbox.BBox_Upper_Lat = lat;
                                        }
                                        catch (Exception e)
                                        {
                                            e.ToString();
                                        }
                                    }
                                }

                                if (bbox != null && CSWURL.Equals(USGIN_AASG_CSWURLString))
                                {
                                    if (bbox.BBox_Lower_Lon > bbox.BBox_Upper_Lon)
                                    {
                                        double low_lon = bbox.BBox_Lower_Lon;
                                        bbox.BBox_Lower_Lon = bbox.BBox_Upper_Lon;
                                        bbox.BBox_Upper_Lon = low_lon;
                                    }
                                }
                            }

                            XElement abstractElement = recordElement.Element(XName.Get("abstract", dct_namespace));
                            if (abstractElement != null && abstractElement.Value != null && !abstractElement.Value.Trim().Equals(""))
                                description = abstractElement.Value;

                            IEnumerable<XElement> subjects = recordElement.Elements(XName.Get("subject", dc_namespace));
                            if (subjects != null)
                            {
                                descriptiveKeywords = new List<string>();
                                string keywords = "";
                                foreach (XElement subject in subjects)
                                {
                                    string keyword = subject.Value;
                                    if (keyword != null && !keyword.Trim().Equals("") && (!descriptiveKeywords.Contains(keyword)))
                                    {
                                        descriptiveKeywords.Add(keyword);
                                        if (keyword.Equals(resourceIdentificationKeywords_geossDateCore))
                                            isDataCore = true;
                                        keywords += keyword;
                                    }
                                }
                                SBAs = getSBAsFromDescriptiveKeywords(keywords);
                            }

                            IEnumerable<XElement> formatsElements = recordElement.Elements(XName.Get("format", dc_namespace));
                            if (formatsElements != null)
                            {
                                formats = new List<string>();
                                foreach (XElement formatElement in formatsElements)
                                {
                                    string format = formatElement.Value;
                                    if (format != null && !format.Trim().Equals("") && (!formats.Contains(format)))
                                    {
                                        formats.Add(format);
                                    }
                                }
                            }

                            int index = accessURL.IndexOf(" ");
                            if (index >= 0)
                                accessURL = accessURL.Substring(0, index);
                            accessURL = accessURL.Trim();

                            string location = null;
                            if (type.Equals(SearchingContent.ServiceType_WPS)
                                || type.Equals(SearchingContent.ServiceType_WMS)
                                || type.Equals(SearchingContent.ServiceType_WCS)
                                || type.Equals(SearchingContent.ServiceType_WFS)
                                || type.Equals(SearchingContent.ServiceType_CSW)
                                || type.Equals(SearchingContent.resourceTypeValue_CLH_Services))
                            {
                                if (domainName_IPAndLocation.ContainsKey(accessURL))
                                    location = domainName_IPAndLocation[accessURL].Location;
                                //在服务器端执行会比较慢，建议放在客户端获取服务器所在位置信息
                                else
                                {
                                    //string url = accessURL;
                                    //string ip = statisticsService.getIPFromDNS(url);
                                    //location = statisticsService.getLocationFromIP(ip);
                                    //string result = "{" + "\"" + url + "\",\n"
                                    //    + "new IPLocation(\"" + url + "\", \"" + ip + "\", \"" + location + "\")},";
                                    //System.Diagnostics.Debug.WriteLine(result);
                                }
                            }
                                       
                            Record record = new Record();
                            record.ID = id;
                            record.Title = title;
                            record.Type = type;
                            record.Source = source;
                            record.Provider = provider;
                            record.MetadataAccessURL = MetadataAccessURL;
                            record.AccessURL = accessURL;
                            record.Abstract = description;
                            record.DescriptiveKeywords = descriptiveKeywords;
                            record.Formats = formats;
                            record.Quality = performanceSummary;
                            record.isDataCore = isDataCore;
                            record.bbox = bbox;
                            record.SBAs = SBAs;

                            if (bbox != null)
                            {                               
                                if(Math.Abs((bbox.BBox_Upper_Lon - bbox.BBox_Lower_Lon))> 200 && Math.Abs((bbox.BBox_Upper_Lat - bbox.BBox_Lower_Lat))> 100)
                                    record.GeoExtensionDescription = "Global";
                                else
                                    record.GeoExtensionDescription = "Local";
                            }
                            else
                                record.GeoExtensionDescription = "Unknown";



                            if (location != null)
                                record.URLLocation = location;
                            Records.Add(record);

                        }
                        results.numberOfRecordsReturned = results.recordList.Count;
                    }
                }
                else
                {
                    results.numberOfRecordsMatched = 0;
                    results.numberOfRecordsReturned = 0;
                    results.recordList = null;
                    results.SearchStatus = "Connect error or timeout...";
                    return results;
                }
            }
            return results;
        }

        private List<string> getSBAsFromDescriptiveKeywords(string keyword)
        {
            List<string> SBAs = new List<string>();
            if (keyword.Contains(SBAVocabulary.SBA_Agriculture) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);
            else if (keyword.Contains(SBAVocabulary.SBA_Agriculture_EconomyTrade) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);
            else if (keyword.Contains(SBAVocabulary.SBA_Agriculture_Fisheries) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);
            else if (keyword.Contains(SBAVocabulary.SBA_Agriculture_FoodSecurity) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);
            else if (keyword.Contains(SBAVocabulary.SBA_Agriculture_GrazingSystems) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);
            else if (keyword.Contains(SBAVocabulary.SBA_Agriculture_TimberFuelFiber) && (!SBAs.Contains(SBAVocabulary.SBA_Agriculture)))
                SBAs.Add(SBAVocabulary.SBA_Agriculture);

            if (keyword.Contains(SBAVocabulary.SBA_Biodiversity) && (!SBAs.Contains(SBAVocabulary.SBA_Biodiversity)))
                SBAs.Add(SBAVocabulary.SBA_Biodiversity);
            else if (keyword.Contains(SBAVocabulary.SBA_Biodiversity_InvasiveSpecies) && (!SBAs.Contains(SBAVocabulary.SBA_Biodiversity)))
                SBAs.Add(SBAVocabulary.SBA_Biodiversity);
            else if (keyword.Contains(SBAVocabulary.SBA_Biodiversity_MigratorySpecies) && (!SBAs.Contains(SBAVocabulary.SBA_Biodiversity)))
                SBAs.Add(SBAVocabulary.SBA_Biodiversity);


            if (keyword.Contains(SBAVocabulary.SBA_Climate) && (!SBAs.Contains(SBAVocabulary.SBA_Climate)))
                SBAs.Add(SBAVocabulary.SBA_Climate);

            if (keyword.Contains(SBAVocabulary.SBA_Disasters) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_CoastalHazards) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_Earthquakes) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_ExtremeWeather) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_TropicalCyclones) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_Volcanoes) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);
            else if (keyword.Contains(SBAVocabulary.SBA_Disasters_WildlandFires) && (!SBAs.Contains(SBAVocabulary.SBA_Disasters)))
                SBAs.Add(SBAVocabulary.SBA_Disasters);


            if (keyword.Contains(SBAVocabulary.SBA_Ecosystems) && (!SBAs.Contains(SBAVocabulary.SBA_Ecosystems)))
                SBAs.Add(SBAVocabulary.SBA_Ecosystems);
            else if (keyword.Contains(SBAVocabulary.SBA_Ecosystems_AgricultureFisheriesForestry) && (!SBAs.Contains(SBAVocabulary.SBA_Ecosystems)))
                SBAs.Add(SBAVocabulary.SBA_Ecosystems);
            else if (keyword.Contains(SBAVocabulary.SBA_Ecosystems_CarbonCycle) && (!SBAs.Contains(SBAVocabulary.SBA_Ecosystems)))
                SBAs.Add(SBAVocabulary.SBA_Ecosystems);


            if (keyword.Contains(SBAVocabulary.SBA_Energy) && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);
            else if (keyword.Contains(SBAVocabulary.SBA_Energy_ElectricityGeneration) && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);
            else if (keyword.Contains(SBAVocabulary.SBA_Energy_GlobalEnergy) && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);
            else if (keyword.ToLower().Contains("oil") && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);
            else if (keyword.ToLower().Contains("gas") && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);
            else if (keyword.Contains(SBAVocabulary.SBA_Energy_RenewableEnergy) && (!SBAs.Contains(SBAVocabulary.SBA_Energy)))
                SBAs.Add(SBAVocabulary.SBA_Energy);


            if (keyword.Contains(SBAVocabulary.SBA_Health) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_BirthDefect) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_Cancer) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_EnvironmentalStress) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_InfectiousDiseases) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_Nutrition) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);
            else if (keyword.Contains(SBAVocabulary.SBA_Health_RespiratoryProblems) && (!SBAs.Contains(SBAVocabulary.SBA_Health)))
                SBAs.Add(SBAVocabulary.SBA_Health);

            if (keyword.Contains(SBAVocabulary.SBA_Water) && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.Contains(SBAVocabulary.SBA_Water_Biogeochemistry) && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.Contains(SBAVocabulary.SBA_Water_DroughtPrediction) && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.ToLower().Contains("river") && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.ToLower().Contains("lake") && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.ToLower().Contains("ice") && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);
            else if (keyword.ToLower().Contains("ocean") && (!SBAs.Contains(SBAVocabulary.SBA_Water)))
                SBAs.Add(SBAVocabulary.SBA_Water);

            if (keyword.Contains(SBAVocabulary.SBA_Weather) && (!SBAs.Contains(SBAVocabulary.SBA_Weather)))
                SBAs.Add(SBAVocabulary.SBA_Weather);

            return SBAs;
        }


        private void getGeospatialServicePerformanceSummaryInfo(List<Record> recordList)
        {
            if (recordList != null)
            {
                List<ServiceInfoForSummary> serviceInfoList = new List<ServiceInfoForSummary>();
                List<Record> serviceRecordList = new List<Record>();
                foreach (Record record in recordList)
                {
                    if (record.Type.Equals(SearchingContent.ServiceType_WMS) || record.Type.Equals(SearchingContent.resourceTypeValue_CLH_Services) || record.Type.Equals(SearchingContent.ServiceType_WCS) || record.Type.Equals(SearchingContent.ServiceType_WFS) || record.Type.Equals(SearchingContent.ServiceType_WPS))
                    {
                        if (record.AccessURL.Equals("unknown"))
                            continue;
                        serviceRecordList.Add(record);
                        string url = record.AccessURL;
                        url = url.Replace("&amp;", "&");
                        int index = url.LastIndexOf("?");
                        string newURL = url;
                        if (index >= 0)
                        {
                            newURL = url.Substring(0, index + 1);
                            string paramsString = url.Substring(index + 1);
                            if (paramsString != null && (!paramsString.Equals("")))
                            {
                                string[] parameters = paramsString.Split('&');
                                string serviceTypeString;
                                for (int i = 0; i < parameters.Length; i++)
                                {
                                    string parameter = parameters[i];
                                    if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_GetCapabilities + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_GetMap + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_Request + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_Version + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_Service + "="))
                                    {
                                        int indexOfEqual = parameters[i].IndexOf("=");
                                        if (indexOfEqual > 0)
                                        {
                                            serviceTypeString = parameters[i].Substring(indexOfEqual + 1);
                                            if (serviceTypeString.ToLower().Equals("wms"))
                                                record.Type = SearchingContent.ServiceType_WMS;
                                            else if (serviceTypeString.ToLower().Equals("wcs"))
                                                record.Type = SearchingContent.ServiceType_WCS;
                                            else if (serviceTypeString.ToLower().Equals("wfs"))
                                                record.Type = SearchingContent.ServiceType_WFS;
                                            else if (serviceTypeString.ToLower().Equals("wps"))
                                                record.Type = SearchingContent.ServiceType_WPS;
                                        }
                                        continue;
                                    }
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_Format + "="))
                                        continue;
                                    else if (parameter.ToLower().Contains(SearchingContent.OGCServicePamameter_Updatesequence + "="))
                                        continue;
                                    else
                                        newURL += "&" + parameters[i];
                                }
                            }
                            if (newURL.Contains("?&"))
                                newURL = newURL.Replace("?&", "?");
                            if (newURL.EndsWith("&"))
                                newURL = newURL.Substring(0, newURL.Length - 1);
                        }
                        else
                            newURL += "?";
                        record.RealServiceURL = newURL;

                        string serviceType = record.Type;
                        if (record.Type.Equals(SearchingContent.resourceTypeValue_CLH_Services))
                            serviceType = SearchingContent.ServiceType_WMS;

                        ServiceInfoForSummary serviceInfo = new ServiceInfoForSummary();
                        serviceInfo.serviceType = serviceType;
                        serviceInfo.serviceURL = record.RealServiceURL;
                        serviceInfoList.Add(serviceInfo);
                    }

                }
                if (serviceInfoList.Count > 0)
                {
                    List<ServiceQoSInfoForSummary> servicePerformanceInfoList = qualityQueryService.getServiceQoSSummaryInfo(serviceInfoList);
                    if (servicePerformanceInfoList != null)
                        foreach (ServiceQoSInfoForSummary performanceInfo in servicePerformanceInfoList)
                        {
                            foreach (Record record in serviceRecordList)
                            {
                                if (record.RealServiceURL != null && record.RealServiceURL.Equals(performanceInfo.serviceURL))
                                {
                                    record.Quality = performanceInfo.rankValue;
                                    break;
                                }
                            }
                        }
                }
            }
        }

        private void getOtherInfo(CSWGetRecordsSearchResults results, Object searchingContent, bool queryServicePerformanceScoreAtServerSide, bool calculateRelevanceAtServerSide)
        {
            if (calculateRelevanceAtServerSide || queryServicePerformanceScoreAtServerSide)
            {
                if (results != null && results.recordList != null)
                {
                    foreach (Record record in results.recordList)
                    {
                        if (calculateRelevanceAtServerSide)
                        {
                            //double relevance = 0.6 + random.NextDouble() * 0.4;
                            //relevance = Math.Round(relevance, 4, MidpointRounding.AwayFromZero);
                            //relevance = Math.Round(relevance, 4);

                            MetadataRanking mr = null;
                            if (searchingContent is string)
                                mr = new MetadataRanking(searchingContent as string, record);
                            else if (searchingContent is SearchingContent)
                                mr = new MetadataRanking(searchingContent as SearchingContent, record);

                            record.Relevance = mr.DoRanking();
                        }
                    }
                    if (queryServicePerformanceScoreAtServerSide)
                    {
                        getGeospatialServicePerformanceSummaryInfo(results.recordList);
                    }
                }
            }
        }

        public static XElement getFirstDescendantElementMatchGivenNameInElement(XElement element, XName xname)
        {
            IEnumerable<XElement> ele = element.Descendants(xname);
            IEnumerator<XElement> s = ele.GetEnumerator();
            XElement child = null;
            if (s.MoveNext())
            {
                child = s.Current;
            }
            return child;
        }

        private void isAllResourceTypeSelected(ResourceType tree, Boolean isAllSelected)
        {
            if (tree.isSelected == false)
            {
                isAllSelected = false;
                return;
            }
            else if (tree.Children != null)
            {
                foreach (ResourceType tree1 in tree.Children)
                {
                    isAllResourceTypeSelected(tree1, isAllSelected);
                }
            }
        }
    }
}
