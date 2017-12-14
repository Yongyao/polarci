using System;

namespace GeoSearch.Web
{
    public class BBox
    {
        public string BBox_CRS { get; set; }
        public double BBox_Lower_Lon { get; set; }
        public double BBox_Lower_Lat { get; set; }
        public double BBox_Upper_Lon { get; set; }
        public double BBox_Upper_Lat { get; set; }

        public static BBox CreateBBox(BBox box1)
        {
            BBox bbox = new BBox();
            bbox.BBox_CRS = box1.BBox_CRS;
            bbox.BBox_Lower_Lon = box1.BBox_Lower_Lon;
            bbox.BBox_Lower_Lat = box1.BBox_Lower_Lat;
            bbox.BBox_Upper_Lon = box1.BBox_Upper_Lon;
            bbox.BBox_Upper_Lat = box1.BBox_Upper_Lat;
            return bbox;
        }

        //public BBox(BBox box1)
        //{
        //    BBox_CRS = box1.BBox_CRS;
        //    BBox_Lower_Lon = box1.BBox_Lower_Lon;
        //    BBox_Lower_Lat = box1.BBox_Lower_Lat;
        //    BBox_Upper_Lon = box1.BBox_Upper_Lon;
        //    BBox_Upper_Lat = box1.BBox_Upper_Lat;
        //}

        public static BBox CreateBBox(string BBox_CRS1, double BBox_Lower_Lon1, double BBox_Lower_Lat1, double BBox_Upper_Lon1, double BBox_Upper_Lat1)
        {
            BBox bbox = new BBox();
            bbox.BBox_CRS = BBox_CRS1;
            bbox.BBox_Lower_Lon = BBox_Lower_Lon1;
            bbox.BBox_Lower_Lat = BBox_Lower_Lat1;
            bbox.BBox_Upper_Lon = BBox_Upper_Lon1;
            bbox.BBox_Upper_Lat = BBox_Upper_Lat1;
            return bbox;
        }
    }
}
