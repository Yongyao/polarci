using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GeoSearch.Web
{
    [ServiceContract(Namespace = "cisc.gmu.edu/OtherQueryFunctionsService")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OtherQueryFunctionsService
    {
        private const string string_unknown = "Unknown";
        
        [OperationContract]
        public string getRecordDetailMetadata(string MetadataAccessURL)
        {
            string detail = null;

            if (MetadataAccessURL == null || MetadataAccessURL.Trim().Equals(""))
                return null;

            /*Both HTTP Post and Get request, they both can work well*/

            //string postRequest = "<?xml version='1.0' encoding='UTF-8'?>"
            //    + "<csw:GetRecordById xmlns:csw='http://www.opengis.net/cat/csw/2.0.2' service='CSW' version='2.0.2' outputSchema='csw:IsoRecord'>"
            //        + "<csw:Id>"
            //            + id
            //        + "</csw:Id>"
            //        +"<csw:ElementSetName>full</csw:ElementSetName>"
            //    + "</csw:GetRecordById>";

            //object[] parameters = new object[2];
            //parameters[0] = clearingHouseUrlString;
            //parameters[1] = postRequest;
            //detail = HttpPost(CSWURL, postRequest);

            //string urlString = CLHCSWURLString + "?request=GetRecordById&service=CSW&version=2.0.2&ElementSetName=full&outputSchema=csw:IsoRecord&Id=" + id;
            //detail = HttpGet(urlString);


            ////Micrcosoft .Net only support XSLT 1.0, don't support XSLT 2.0 and further version, so we can not use xslTransform to do xml transform. 
            //if (MetadataAccessURL.StartsWith(GOSCSWURLString))
            //{
            //    try
            //    {
            //        //string
            //        XPathDocument myXPathDocument = new XPathDocument(MetadataAccessURL);
            //        XslCompiledTransform myXslTransform = new XslCompiledTransform();
            //        StringWriter strWriter = new StringWriter();
            //        XmlTextWriter writer = new XmlTextWriter(strWriter);
            //        myXslTransform.Load(CSDGM2ISO19115Stylesheet);
            //        myXslTransform.Transform(myXPathDocument, null, writer);
            //        writer.Close();

            //        detail = strWriter.ToString();
            //        //StreamReader stream = new StreamReader(resultDoc);
            //        //Console.Write("**This is result document**\n\n");
            //        //Console.Write(stream.ReadToEnd());
            //    }
            //    catch (Exception e)
            //    {
            //        e.GetType();
            //    }
            //}

            detail = BaseHttpFunctions.HttpGet(MetadataAccessURL);
            return detail;
        }

        [OperationContract]
        public WMSLayers getAllLayerNamesOfWMS(string urlstring)
        {
            WMSLayers wms_layers = new WMSLayers();
            List<WMSLayer> layerList = new List<WMSLayer>();
            wms_layers.layersList = layerList;
            wms_layers.WMSURL = urlstring;

            if (!urlstring.ToLower().Contains("service="))
            {
                if (urlstring.Contains("?"))
                    if (urlstring.EndsWith("&"))
                        urlstring += "SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                    else
                        urlstring += "&SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                else
                {
                    urlstring += "?SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                }
            }

            XDocument doc = null;
            try
            {
                string response = BaseHttpFunctions.HttpGet(urlstring);
                doc = XDocument.Parse(response);
            }
            catch (Exception e)
            {
                e.GetType();
                return null;
            }
            if (doc != null)
            {
                XElement rootElement = doc.Root;

                IEnumerable<XElement> Services = rootElement.Descendants(XName.Get("Service"));
                if (Services != null)
                {
                    foreach (XElement Service in Services)
                    {
                        string name = null;
                        string title = null;
                        XElement nameElement = Service.Element(XName.Get("Name"));
                        if (nameElement != null)
                        {
                            name = nameElement.Value;
                            if (name != null && !(name.Trim().Equals("")))
                                wms_layers.name = name;
                        }

                        XElement titleElement = Service.Element(XName.Get("Title"));
                        if (titleElement != null)
                        {
                            title = titleElement.Value;
                            if (title != null && !(title.Trim().Equals("")))
                                wms_layers.title = title;
                        }
                    }
                }

                Dictionary<XElement, BBox> layer_bbox_map = new Dictionary<XElement, BBox>();

                IEnumerable<XElement> Layers = rootElement.Descendants(XName.Get("Layer"));
                if (Layers != null)
                {
                    foreach (XElement layer in Layers)
                    {
                        string name = null;
                        string title = null;
                        XElement nameElement = layer.Element(XName.Get("Name"));
                        XElement LatLonBoundingBoxElement = layer.Element(XName.Get("LatLonBoundingBox"));
                        WMSLayer layerObject = new WMSLayer();
                        
                        BBox bbox = null;
                        if (LatLonBoundingBoxElement != null)
                        {
                            bbox = new BBox();
                            XAttribute a = LatLonBoundingBoxElement.Attribute(XName.Get("maxy"));
                            if (a != null)
                            {
                                string value = a.Value;
                                if (value != null)
                                    bbox.BBox_Upper_Lat = Double.Parse(value);
                            }
                            a = LatLonBoundingBoxElement.Attribute(XName.Get("miny"));
                            if (a != null)
                            {
                                string value = a.Value;
                                if (value != null)
                                    bbox.BBox_Lower_Lat = Double.Parse(value);
                            }
                            a = LatLonBoundingBoxElement.Attribute(XName.Get("maxx"));
                            if (a != null)
                            {
                                string value = a.Value;
                                if (value != null)
                                    bbox.BBox_Upper_Lon = Double.Parse(value);
                            }
                            a = LatLonBoundingBoxElement.Attribute(XName.Get("minx"));
                            if (a != null)
                            {
                                string value = a.Value;
                                if (value != null)
                                    bbox.BBox_Lower_Lon = Double.Parse(value);
                            }
                            layerObject.box = bbox;
                            layer_bbox_map.Add(layer, bbox);
                        }
                        //if current layer don't contain LatLonBoundingBox and Parent Layer is not exist or do not contain  LatLonBoundingBox either, current layer is not accessible
                        else
                        {
                            if (layer.Parent.Name.Equals(XName.Get("Layer")))
                            {
                                if (layer_bbox_map.ContainsKey(layer.Parent))
                                {
                                    bbox = BBox.CreateBBox(layer_bbox_map[layer.Parent]);
                                    layerObject.box = bbox;
                                    layer_bbox_map.Add(layer, bbox);
                                }
                                else
                                    continue;
                            }
                            else
                                continue;
                        }

                        //if there is not Layer name or LatLonBoundingBox element exist, the layer is not accessible
                        if (nameElement == null)
                            continue;  
                        else
                        {
                            
                            name = nameElement.Value;
                            if (name != null && !(name.Trim().Equals("")))
                                layerObject.name = name;

                            XElement titleElement = layer.Element(XName.Get("Title"));
                            if (titleElement != null)
                            {
                                title = titleElement.Value;
                                if (title != null && !(title.Trim().Equals("")))
                                    layerObject.title = title;
                            }

                            //we want to show title, which provide us more inforamtion than layer name, but when there is not layer title, we still show layer name as title
                            if (layerObject.title == null && layerObject.name != null)
                                layerObject.title = layerObject.name;
                            layerList.Add(layerObject);
                        }
                    }
                }
            }
            return wms_layers;
        }

        [OperationContract]
        public HierachicalWMSLayers getHierachicalLayersOfWMS(string urlstring)
        {
            HierachicalWMSLayers wms_layers = new HierachicalWMSLayers();
            wms_layers.layersList = new List<CascadedWMSLayer>();
            wms_layers.WMSURL = urlstring;

            if (!urlstring.ToLower().Contains("service="))
            {
                if (urlstring.Contains("?"))
                    if (urlstring.EndsWith("&"))
                        urlstring += "SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                    else
                        urlstring += "&SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                else
                {
                    urlstring += "?SERVICE=WMS&REQUEST=GetCapabilities&VERSION=1.1.1";
                }
            }

            XDocument doc = null;
            try
            {
                string response = BaseHttpFunctions.HttpGet(urlstring);
                doc = XDocument.Parse(response);
            }
            catch (Exception e)
            {
                e.GetType();
                return null;
            }
            if (doc != null)
            {
                XElement rootElement = doc.Root;

                IEnumerable<XElement> Services = rootElement.Descendants(XName.Get("Service"));
                if (Services != null)
                {
                    foreach (XElement Service in Services)
                    {
                        string name = null;
                        string title = null;
                        XElement nameElement = Service.Element(XName.Get("Name"));
                        if (nameElement != null)
                        {
                            name = nameElement.Value;
                            if (name != null && !(name.Trim().Equals("")))
                                wms_layers.serviceName = name;
                        }

                        XElement titleElement = Service.Element(XName.Get("Title"));
                        if (titleElement != null)
                        {
                            title = titleElement.Value;
                            if (title != null && !(title.Trim().Equals("")))
                                wms_layers.serviceTitle = title;
                        }
                    }
                }

                Dictionary<XElement, BBox> layer_bbox_map = new Dictionary<XElement, BBox>();

                XElement Capability = rootElement.Element(XName.Get("Capability"));
                XElement firstLevel_Layer = Capability.Element(XName.Get("Layer"));
                if (firstLevel_Layer != null)
                {
                    IEnumerable<XElement> secondLevel_Layers = firstLevel_Layer.Elements(XName.Get("Layer"));
                    if (wms_layers.serviceName == null || wms_layers.serviceName.Trim().Equals(""))
                    {
                        XElement nameElement = firstLevel_Layer.Element(XName.Get("Name"));
                        if (nameElement == null || nameElement.Value.Trim().Equals(""))
                            wms_layers.serviceName = string_unknown;
                        else
                            wms_layers.serviceName = nameElement.Value;
                    }

                    if (wms_layers.serviceTitle == null || wms_layers.serviceTitle.Trim().Equals(""))
                    {
                        XElement titleElement = firstLevel_Layer.Element(XName.Get("Title"));
                        if (titleElement == null || titleElement.Value.Trim().Equals(""))
                            wms_layers.serviceTitle = string_unknown;
                        else
                            wms_layers.serviceTitle = titleElement.Value;
                    }

                    //get BBOX information
                    BBox bbox = getBBoxInformation(firstLevel_Layer);
                    if (bbox != null)
                    {
                        wms_layers.latLonBBox = bbox;
                        layer_bbox_map.Add(firstLevel_Layer, bbox);
                    }

                    wms_layers.subLayers_Number = secondLevel_Layers.Count();
                    foreach (XElement layer in secondLevel_Layers)
                    {
                        wms_layers.layersList.Add(getLayerInfomation(layer, layer_bbox_map, wms_layers));
                    }
                }

            }
            return wms_layers;
        }

        //get BBOX information
        private BBox getBBoxInformation(XElement layer)
        {
            BBox bbox = null;
            XElement LatLonBoundingBoxElement = layer.Element(XName.Get("LatLonBoundingBox"));
            if (LatLonBoundingBoxElement != null)
            {
                bbox = new BBox();
                XAttribute a = LatLonBoundingBoxElement.Attribute(XName.Get("maxy"));
                if (a != null)
                {
                    string value = a.Value;
                    if (value != null)
                        bbox.BBox_Upper_Lat = Double.Parse(value);
                }
                a = LatLonBoundingBoxElement.Attribute(XName.Get("miny"));
                if (a != null)
                {
                    string value = a.Value;
                    if (value != null)
                        bbox.BBox_Lower_Lat = Double.Parse(value);
                }
                a = LatLonBoundingBoxElement.Attribute(XName.Get("maxx"));
                if (a != null)
                {
                    string value = a.Value;
                    if (value != null)
                        bbox.BBox_Upper_Lon = Double.Parse(value);
                }
                a = LatLonBoundingBoxElement.Attribute(XName.Get("minx"));
                if (a != null)
                {
                    string value = a.Value;
                    if (value != null)
                        bbox.BBox_Lower_Lon = Double.Parse(value);
                }

            }
            return bbox;
        }


        private CascadedWMSLayer getLayerInfomation(XElement layer, Dictionary<XElement, BBox> layer_bbox_map, HierachicalWMSLayers wms)
        {
            CascadedWMSLayer layerObject = new CascadedWMSLayer();
            string name = null;
            string title = null;
            XElement nameElement = layer.Element(XName.Get("Name"));
            //XElement LatLonBoundingBoxElement = layer.Element(XName.Get("LatLonBoundingBox"));
            XElement Extent = layer.Element(XName.Get("Extent"));

            IEnumerable<XElement> cascadedLayers = layer.Elements(XName.Get("Layer"));

            //get BBOX information
            BBox bbox = getBBoxInformation(layer);
            if (bbox != null)
            {
                layerObject.latLonBBox = bbox;
                layer_bbox_map.Add(layer, bbox);
            }

            //if current layer don't contain LatLonBoundingBox and Parent Layer is not exist or do not contain  LatLonBoundingBox either, current layer is not accessible
            else
            {
                XElement parent = layer.Parent;
                while (parent.Name.Equals(XName.Get("Layer")))
                {
                    if (layer_bbox_map.ContainsKey(layer.Parent))
                    {
                        bbox = BBox.CreateBBox(layer_bbox_map[layer.Parent]);
                        layerObject.latLonBBox = bbox;
                        layer_bbox_map.Add(layer, bbox);
                        break;
                    }
                    else
                        parent = parent.Parent;
                }
            }

            //if there is not Layer name or LatLonBoundingBox element exist, the layer is not accessible
            if (nameElement != null && layerObject.latLonBBox != null)
            {
                layerObject.canGetMap = true;
                wms.allGetMapEnabledLayers_Number++;
            }
            else
                layerObject.canGetMap = false;


            if (nameElement == null)                
                layerObject.name = string_unknown;
            else
            {
                
                name = nameElement.Value;
                if (name != null && !(name.Trim().Equals("")))
                    layerObject.name = name;
            }

            XElement titleElement = layer.Element(XName.Get("Title"));
            if (titleElement != null)
            {
                title = titleElement.Value;
                if (title != null && !(title.Trim().Equals("")))
                    layerObject.title = title;
                else
                    layerObject.title = string_unknown;
            }

            //we want to show title, which provide us more inforamtion than layer name, but when there is not layer title, we still show layer name as title
            //if (layerObject.title == null && layerObject.name != null)
            //    layerObject.title = layerObject.name;

            //add time extent
            if (Extent != null && Extent.Attribute(XName.Get("name")).Value.Equals("time"))
            {
                layerObject.extent_time_default = Extent.Attribute(XName.Get("default")).Value;
                layerObject.extent_time = Extent.Value;
                layerObject.timeEnabled = true;
            }
            else
                layerObject.timeEnabled = false;

            //get the legend for the layer
            XElement style = layer.Element(XName.Get("Style"));
            if (style != null)
            {
                XElement legendURL = style.Element(XName.Get("LegendURL"));
                if (legendURL != null)
                {
                    XElement OnlineResource = legendURL.Element(XName.Get("OnlineResource"));
                    if (OnlineResource != null)
                    {
                        XAttribute url = OnlineResource.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"));
                        if (url != null)
                        {
                            layerObject.legendURL = url.Value;
                        }
                    }
                }
            }

            //add children layers
            if (cascadedLayers != null)
            {
                layerObject.Children = new System.Collections.ObjectModel.ObservableCollection<CascadedWMSLayer>();
                foreach (XElement childLayer in cascadedLayers)
                {
                    layerObject.Children.Add(getLayerInfomation(childLayer, layer_bbox_map, wms));
                }
            }

            //get queryable information
            XAttribute queryable = layer.Attribute(XName.Get("queryable"));
            if (queryable != null && queryable.Value.Equals("1"))
                layerObject.queryable = true;
            else
                layerObject.queryable = false;
            
            return layerObject;
        }
    }
}
