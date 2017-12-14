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

// for detailed record/metadata
namespace GeoSearch
{
    public class Attributes_Dataset
    {
        public string ResourceConstraints_AccessConstraints { get; set; }
        public string ResourceConstraints_UseConstraints { get; set; }
        public string ResourceConstraints_OtherConstraints { get; set; }
        public string SupplementalInformation { get; set; }
        public string ResourceMaintenance { get; set; }
        public string GraphicOverview_FileURL { get; set; }
        public string GraphicOverview_Description { get; set; }
    }
}
