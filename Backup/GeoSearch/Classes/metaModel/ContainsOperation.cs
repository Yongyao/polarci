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
using System.Collections.Generic;

namespace GeoSearch
{
    public class ContainsOperation
    {
        public string operationName { get; set; }
        public List<string> DCP { get; set; }
        public List<OnlineResource> OnlineResources { get; set; }
    }
}
