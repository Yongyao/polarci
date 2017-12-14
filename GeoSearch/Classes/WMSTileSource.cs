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


// vis the WMS on the Bing 
namespace GeoSearch
{
    public class WMSTileSource : Microsoft.Maps.MapControl.LocationRectTileSource
    {

        public WMSTileSource(string WMSURL, string layerName, Range<double> range, LocationRect locationRect)
            : base(WMSURL + "SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&LAYERS=" + layerName + "&STYLES=&SRS=EPSG%3A4326&BBOX={0}&WIDTH=256&HEIGHT=256&FORMAT=image/png&TRANSPARENT=TRUE", locationRect, range)
        {
            
        }

        public WMSTileSource(string WMSURL, string layerName, string time, Range<double> range, LocationRect locationRect)
            : base(WMSURL + "SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap&LAYERS=" + layerName + "&STYLES=&SRS=EPSG%3A4326&BBOX={0}&WIDTH=256&HEIGHT=256&FORMAT=image/png&TRANSPARENT=TRUE&time=" + time, locationRect, range)
        {

        }

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            if (zoomLevel < ZoomRange.From)
                return null;
            else if (zoomLevel > ZoomRange.To)
                return null;
            else
                return new Uri(String.Format(this.UriFormat, XYZoomToBBox(x, y, zoomLevel)));
        }

        public string XYZoomToBBox(int x, int y, int zoom)
        {

            int TILE_HEIGHT = 256, TILE_WIDTH = 256;

            // From the grid position and zoom, work out the min and max Latitude / Longitude values of this tile  

            double W = (float)(x * TILE_WIDTH) * 360 / (float)(TILE_WIDTH * Math.Pow(2, zoom)) - 180;

            double N = (float)Math.Asin((Math.Exp((0.5 - (y * TILE_HEIGHT) / (TILE_HEIGHT) / Math.Pow(2, zoom)) * 4 * Math.PI) - 1) / (Math.Exp((0.5 - (y * TILE_HEIGHT) / 256 / Math.Pow(2, zoom)) * 4 * Math.PI) + 1)) * 180 / (float)Math.PI;

            double E = (float)((x + 1) * TILE_WIDTH) * 360 / (float)(TILE_WIDTH * Math.Pow(2, zoom)) - 180;

            double S = (float)Math.Asin((Math.Exp((0.5 - ((y + 1) * TILE_HEIGHT) / (TILE_HEIGHT) / Math.Pow(2, zoom)) * 4 * Math.PI) - 1) / (Math.Exp((0.5 - ((y + 1) * TILE_HEIGHT) / 256 / Math.Pow(2, zoom)) * 4 * Math.PI) + 1)) * 180 / (float)Math.PI;

            string[] bounds = new string[] { W.ToString(), S.ToString(), E.ToString(), N.ToString() };

            // Return a comma-separated string of the bounding coordinates  

            return string.Join(",", bounds);

        }
    } 
}
