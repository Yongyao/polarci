using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Maps.MapControl;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public partial class MetadataPanel : UserControl
    {
        public MetadataPanel()
        {
            InitializeComponent();
        }
    }

    public class ResourceAttributesVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = Visibility.Visible;

            if (value == null)
            {
                v = Visibility.Collapsed;
            }
            else
            {
                if (value is string)
                {
                    string value_string = value as string;
                    if(value_string.Trim().Length==0)
                        v = Visibility.Collapsed;
                }
                else if (value is List<string>)
                {
                    List<string> list = value as List<string>;
                    if (list.Count == 0)
                        v = Visibility.Collapsed;
                }
            }
            return v;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TextBoxHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double height = 20;
            if (value is string)
            {
                string value_string = value as string;
                if (value_string.Length <= 100 || value_string == null)
                    height=20;
                else if(value_string.Length <= 200 && value_string.Length > 100)
                    height = 38;
                else
                    height = 76;
            }
            return height;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TextBoxHeightConverter_Title : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double height = 20;
            if (value is string)
            {
                string value_string = value as string;
                if (value_string.Length <= 100 || value_string == null)
                    height = 20;
                else
                    height = 38;
            }
            return height;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class TextBlockHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double height = 20;
            int len = 1;
            int yu = 0;

            if (value is string)
            {
                string value_string = value as string;
                len = value_string.Length / 110;
                yu = value_string.Length % 110;

                height = len * 20;
                if (yu != 0)
                    height += 20;
            }
            return height;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ListBoxHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Double height = 24;
            int count = 0;

            if (value is List<string>)
            {
                List<string> list = value as List<string>;
                count = list.Count;
                //olny directly show 3 three items in listbox, more item need to drag ScrollBar
                if (count > 3)
                    count = 3;
                height = count * height;
            }
            return height;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class DCPConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";

            if (value is List<string>)
            {
                List<string> list = value as List<string>;
                foreach (string dcp in list)
                {
                    result += (dcp+"/");
                }

                int num = result.Length;
                if(num>=1)
                    result = result.Substring(0, num-1);
            }
            return result;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BBoxBorderMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness margin = new Thickness();
            if (value is BoundingBox)
            {
                BoundingBox BBox = value as BoundingBox;
                if (BBox.BBox_East != null && BBox.BBox_West != null && BBox.BBox_North != null && BBox.BBox_South != null)
                {
                    double right = (180.0 - double.Parse(BBox.BBox_East)) / 360.0 * 500.0;
                    margin.Right = right;

                    double left = (180.0 + double.Parse(BBox.BBox_West)) / 360.0 * 500.0;
                    margin.Left = left;

                    double top = (90.0 - double.Parse(BBox.BBox_North)) / 180.0 * 250.0;
                    margin.Top = top;

                    double bottom = (90.0 + double.Parse(BBox.BBox_South)) / 180.0 * 250.0;
                    margin.Bottom = bottom;
                }
            }
            if (margin.Right < margin.Left)
            {
                double right = margin.Right;
                margin.Right = margin.Left;
                margin.Left = right;
            }
            if (margin.Top > margin.Bottom)
            {
                double top = margin.Top;
                margin.Top = margin.Bottom;
                margin.Bottom = top;
            }
            return margin;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BBoxHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double height = 250;
            if (value is BoundingBox)
            {
                BoundingBox BBox = value as BoundingBox;
                if (BBox.BBox_North != null && BBox.BBox_South != null)
                {
                    height = (double.Parse(BBox.BBox_North) - double.Parse(BBox.BBox_South)) / 180 * 250.0;
                }
            }
            if (height < 0)
                height = -height;
            return height;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BBoxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = 250;
            if (value is BoundingBox)
            {
                BoundingBox BBox = value as BoundingBox;
                if (BBox.BBox_East != null && BBox.BBox_West != null)
                {
                    width = (double.Parse(BBox.BBox_East) - double.Parse(BBox.BBox_West)) / 360.0 * 500.0;
                }
            }
            if (width < 0)
                width = -width;
            return width;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BingMapCenterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location center = null;
            if (value is BoundingBox)
            {
                BoundingBox bbox = value as BoundingBox;
                if (bbox != null)
                {
                    BBox box = new BBox();
                    box.BBox_CRS = bbox.BBox_CRS;
                    box.BBox_Lower_Lat = double.Parse(bbox.BBox_South);
                    box.BBox_Upper_Lat = double.Parse(bbox.BBox_North);
                    box.BBox_Lower_Lon = double.Parse(bbox.BBox_West);
                    box.BBox_Upper_Lon = double.Parse(bbox.BBox_East);

                    center = Utilities.getCenterLocation(box);
                }

            }
            return center;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class BingMapZoomLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Thickness margin = new Thickness();
            if (value is BoundingBox)
            {
                BoundingBox BBox = value as BoundingBox;
                if (BBox.BBox_East != null && BBox.BBox_West != null && BBox.BBox_North != null && BBox.BBox_South != null)
                {
                    double right = (180.0 - double.Parse(BBox.BBox_East)) / 360.0 * 500.0;
                    margin.Right = right;

                    double left = (180.0 + double.Parse(BBox.BBox_West)) / 360.0 * 500.0;
                    margin.Left = left;

                    double top = (90.0 - double.Parse(BBox.BBox_North)) / 180.0 * 250.0;
                    margin.Top = top;

                    double bottom = (90.0 + double.Parse(BBox.BBox_South)) / 180.0 * 250.0;
                    margin.Bottom = bottom;
                }
            }
            if (margin.Right < margin.Left)
            {
                double right = margin.Right;
                margin.Right = margin.Left;
                margin.Left = right;
            }
            if (margin.Top > margin.Bottom)
            {
                double top = margin.Top;
                margin.Top = margin.Bottom;
                margin.Bottom = top;
            }
            return margin;
        }

        // Not used.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
