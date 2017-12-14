using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class Utilities
    {
        public static Color blue = Color.FromArgb(100, 0, 0, 200);
        public static Color green = Color.FromArgb(100, 0, 100, 100);
        public static Color purple = Color.FromArgb(100, 100, 0, 100);

        public static Color polygonLineColor = Color.FromArgb(100, 100, 0, 100);
        public static Color polygonFillColor = Color.FromArgb(20, 20, 0, 100);
        public static Color polygonHighlightFillColor = Color.FromArgb(40, 40, 0, 100);

        public static Color polygonLineColor_bbox = Color.FromArgb(255, 242, 138, 19);
        public static Color polygonFillColor_bbox = Color.FromArgb(255, 242, 225, 19);

        public static int bboxPolygonLineThickness = 2;
        public const double bboxPolygenOpacity = 0.7;
        public const double bboxPolygenOpacity1 = 0.4;
        
        //获得BoudingBox中心点位置
        public static Location getCenterLocation(BBox bbox)
        {
            double lon = 0.0;
            if (bbox.BBox_Lower_Lon <= bbox.BBox_Upper_Lon)
            {
                    lon = (bbox.BBox_Lower_Lon + bbox.BBox_Upper_Lon) / 2;
            }
            //当lower_lon大于upper_lon时，跨越反面子午线 Cross Antimeridian(而不是本初子午线prime meridian),中心点位置需要特别的计算
            else
            {
                if (bbox.BBox_Lower_Lon == 180 && bbox.BBox_Upper_Lon == -180)
                    lon = 0;
                else
                {
                    double crossed_lon_range = (180 - bbox.BBox_Lower_Lon) + (180 + bbox.BBox_Upper_Lon);
                    lon = bbox.BBox_Lower_Lon + crossed_lon_range / 2;
                    if (lon > 180)
                    {
                        lon = bbox.BBox_Upper_Lon - crossed_lon_range / 2;
                    }
                }
            }
            double lat = (bbox.BBox_Lower_Lat + bbox.BBox_Upper_Lat) / 2;
            return new Location(lat, lon);
        }

        //获得BoudingBox对应的Rectangle
        public static MapPolygon getBBoxPolygon(BBox bbox)
        {
            double middle_lon1 = 0.0;
            double middle_lon2 = 0.0;
            if (bbox.BBox_Upper_Lon == -180 && bbox.BBox_Lower_Lon == 180)
            {
                BBox bbox1 = new BBox();
                bbox1.BBox_Lower_Lon = bbox.BBox_Upper_Lon;
                bbox1.BBox_Upper_Lon = bbox.BBox_Lower_Lon;
                bbox1.BBox_Lower_Lat = bbox.BBox_Lower_Lat;
                bbox1.BBox_Upper_Lat = bbox.BBox_Upper_Lat;
                return getBBoxPolygon(bbox1);
            }

            if (bbox.BBox_Lower_Lon <= bbox.BBox_Upper_Lon)
            {
                middle_lon1 = bbox.BBox_Lower_Lon + (bbox.BBox_Upper_Lon - bbox.BBox_Lower_Lon) / 3;
                middle_lon2 = bbox.BBox_Lower_Lon + 2 * (bbox.BBox_Upper_Lon - bbox.BBox_Lower_Lon) / 3;
            }
            //当lower_lon大于upper_lon时，跨越反面子午线 Cross Antimeridian(而不是本初子午线prime meridian),中间点位置需要特别的计算
            else
            {
                if (bbox.BBox_Upper_Lon < 0 && bbox.BBox_Lower_Lon >0)
                {
                    middle_lon1 = (180 - bbox.BBox_Lower_Lon) / 2 + bbox.BBox_Lower_Lon;
                    middle_lon2 = (180 + bbox.BBox_Upper_Lon) / 2 - 180;
                }
                else if (bbox.BBox_Upper_Lon < 0 && bbox.BBox_Lower_Lon <= 0)
                {
                    if (bbox.BBox_Lower_Lon > -70)
                    {
                        middle_lon1 = bbox.BBox_Upper_Lon - (180 + bbox.BBox_Upper_Lon) / 2;
                        middle_lon2 = 90;
                    }
                    else
                    {
                        middle_lon1 = 150;
                        middle_lon2 = 0;
                    }
                }
                else if (bbox.BBox_Upper_Lon > 0 && bbox.BBox_Lower_Lon >= 0)
                {
                    if (bbox.BBox_Lower_Lon < 70)
                    {
                        middle_lon1 = (180.0 - bbox.BBox_Lower_Lon) / 2 + bbox.BBox_Lower_Lon;
                        middle_lon2 = -110;
                    }
                    else
                    {
                        middle_lon1 = -150;
                        middle_lon2 = 0;
                    }
                }
                

            }

            // Create the locations
            Location location1 = new Location(bbox.BBox_Lower_Lat, bbox.BBox_Lower_Lon);
            Location location2 = new Location(bbox.BBox_Lower_Lat, middle_lon1);
            Location location3 = new Location(bbox.BBox_Lower_Lat, middle_lon2);
            Location location4 = new Location(bbox.BBox_Lower_Lat, bbox.BBox_Upper_Lon);
            Location location5 = new Location(bbox.BBox_Upper_Lat, bbox.BBox_Upper_Lon);
            Location location6 = new Location(bbox.BBox_Upper_Lat, middle_lon2);
            Location location7 = new Location(bbox.BBox_Upper_Lat, middle_lon1);
            Location location8 = new Location(bbox.BBox_Upper_Lat, bbox.BBox_Lower_Lon);
            //Location location9 = new Location(bbox.BBox_Lower_Lat, bbox.BBox_Lower_Lon);

            //Create a polygon 
            MapPolygon polygon = new MapPolygon();
            //polygon.Locations = new LocationCollection() { location1, location2, location3, location4, location5, location6, location7, location8, location9 };
            polygon.Locations = new LocationCollection() { location1, location2, location3, location4, location5, location6, location7, location8 };
            polygon.Fill = new SolidColorBrush(polygonFillColor);
            polygon.Stroke = new SolidColorBrush(polygonLineColor);
            polygon.StrokeThickness = bboxPolygonLineThickness;
            polygon.Opacity = bboxPolygenOpacity;

            return polygon;
        }

        //获得loc点坐标对应的Rectangle
        public static MapPolygon getBBoxPolygon(Location loc)
        {
            // Create the locations
            Location location1 = new Location(loc);
            Location location2 = new Location(loc);
            Location location3 = new Location(loc);
            Location location4 = new Location(loc);

            //Create a polygon 
            MapPolygon polygon = new MapPolygon();
            polygon.Locations = new LocationCollection() { location1, location2, location3, location4 };
            polygon.Fill = new SolidColorBrush(polygonFillColor);
            polygon.Stroke = new SolidColorBrush(polygonLineColor);
            polygon.StrokeThickness = bboxPolygonLineThickness;
            polygon.Opacity = bboxPolygenOpacity;
            return polygon;
        }

        //获得合适的bingmaps的ZoomLevel
        public static int getSuitableMapLevel(LocationRect rect)
        {
            int level = 5;
            //int x_level = 5;
            //int y_level = 5;
            double x1 = 360.0 / rect.Width;
            double y1 = 180.0 / rect.Height;

            int x_level = (int)Math.Log(x1, 2) + 2;
            int y_level = (int)Math.Log(y1, 2) + 2;

            if (x_level > y_level)
                level = x_level;
            else
                level = y_level;
            if (level < 5)
                level = 5;
            return level;
        }

        public static LocationRect getLocationRectFromBBox(BBox bbox)
        {
            LocationRect locationRect = new LocationRect(new Location(90, -180), new Location(-90, 180));
            if (bbox != null)
                locationRect = new LocationRect(new Location(bbox.BBox_Upper_Lat, bbox.BBox_Lower_Lon), new Location(bbox.BBox_Lower_Lat, bbox.BBox_Upper_Lon));
            return locationRect;
        }

        public static LocationRect getLocationRectFromBBox(GeoSearch.OtherQueryFunctionsServiceReference.BBox bbox)
        {
            LocationRect locationRect = new LocationRect(new Location(90, -180), new Location(-90, 180));
            if (bbox != null)
                locationRect = new LocationRect(new Location(bbox.BBox_Upper_Lat, bbox.BBox_Lower_Lon), new Location(bbox.BBox_Lower_Lat, bbox.BBox_Upper_Lon));
            return locationRect;
        }

        ////获得合适的bingmaps的ZoomLevel
        //public static LocationRect getLargerRectangleCoverCurrentRectangle(LocationRect rect)
        //{
        //    LocationRect locationRect1 = new LocationRect(rect);
        //    double width = rect.Width;
        //    double height = rect.Height;
        //    if (width < 0)
        //        width = -width;
        //    if (height < 0)
        //        height = -height;


        //    double east = 0.0;
        //    double west = 0.0;
        //    double north = 0.0;
        //    double south = 0.0;

        //    if (rect.West> rect.East)
        //        east = rect.East 

        //    //locationRect1.West -= width / 4;
        //    //locationRect1.East += width / 4;
        //    //locationRect1.North += height / 4;
        //    //locationRect1.South -= height / 4;
        //    return locationRect1;
        //}

        //获得合适的bingmaps的ZoomLevel(比BBox略大一点)
        public static LocationRect getLargerLocationRectFromBBox(BBox bbox)
        {
            double width = 0.0;
            //跨越反面子午线 Cross Antimeridian(即E180与W180交线，而不是本初子午线prime meridian)的情况
            if (bbox.BBox_Upper_Lon < 0 && bbox.BBox_Lower_Lon >= 0)
                width = (180 - bbox.BBox_Lower_Lon) + (180 + bbox.BBox_Upper_Lon);
            else
                width = Math.Abs(bbox.BBox_Upper_Lon - bbox.BBox_Lower_Lon);
            double height = Math.Abs(bbox.BBox_Upper_Lat - bbox.BBox_Lower_Lat);
            
            double east = 0.0;
            double west = 0.0;
            double north = 0.0;
            double south = 0.0;

            if (bbox.BBox_Upper_Lon < bbox.BBox_Lower_Lon)
            {
                //if (bbox.BBox_Upper_Lon >= 0 && bbox.BBox_Lower_Lon >= 0)
                //{
                //    east = 180.0;
                //    west = -180.0;
                //}
                //else if (bbox.BBox_Upper_Lon <= 0 && bbox.BBox_Lower_Lon <= 0)
                //{
                //    east = 180.0;
                //    west = -180.0;
                //}
                //else
                //{
                //    //east = bbox.BBox_Upper_Lon + width / 4;
                //    //west = bbox.BBox_Lower_Lon - width / 4;
                //}
                east = 180.0;
                west = -180.0;
            }
            else
            {
                east = bbox.BBox_Upper_Lon + width / 4;
                west = bbox.BBox_Lower_Lon - width / 4;
            }

            if (east > 180)
                east -= 360;
            else if (east < -180)
                east += 360;

            if (west > 180)
                west -= 360;
            else if (west < -180)
                west += 360;

            {
                north = bbox.BBox_Upper_Lat + height / 4;
                south = bbox.BBox_Lower_Lat - height / 4;
            }

            if (north > 90)
                north = 90;
            if (south < -90)
                south = -90;

            LocationRect locationRect = new LocationRect(new Location(90, -180), new Location(-90, 180));
            if (bbox != null)
                locationRect = new LocationRect(new Location(north, west), new Location(south, east));
            return locationRect;
        }

        public static double getValidatedLatFromString(string valueString)
        {
            double value = 0.0;
            try
            {
                value = double.Parse(valueString);
                if (value > 90)
                    value = 90;
                else if (value < -90)
                    value = -90;
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return value;
        }

        public static double getValidatedLonFromString(string valueString)
        {
            double value = 0.0;
            try
            {
                value = double.Parse(valueString);
                if (value > 180)
                    value -= 360;
                else if (value < -180)
                    value += -360;
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return value;
        }
    }
}
