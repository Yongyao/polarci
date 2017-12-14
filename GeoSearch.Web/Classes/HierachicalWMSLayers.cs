using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// 名义图层
namespace GeoSearch.Web
{
    public class HierachicalWMSLayers
    {
        public List<CascadedWMSLayer> layersList { set; get; }
        public string serviceName { set; get; }
        public string serviceTitle { get; set; }
        public BBox latLonBBox { set; get; }
        public List<BBox> otherCRSBBoxList { set; get; }
        public string WMSURL { set; get; }
        public int subLayers_Number { set; get; }
        public int allGetMapEnabledLayers_Number { set; get; }
    }
}