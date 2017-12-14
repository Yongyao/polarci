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
    public class Metadata
    {
        public string pointOfContact_IndividualName { get; set; }
        public string pointOfContact_OrganisationName { get; set; }
        public string pointOfContact_PositionName { get; set; }
        public string pointOfContact_Role { get; set; }
        public string pointOfContact_Voice { get; set; }
        public string pointOfContact_Facsimile { get; set; }
        public string pointOfContact_Email { get; set; } 
        public string pointOfContact_OnlineResource { get; set; }
        public string pointOfContact_DeliveryPoint { get; set; }
        public string pointOfContact_City { get; set; }
        public string pointOfContact_AdministrativeArea { get; set; }
        public string pointOfContact_PostalCode { get; set; }
        public string pointOfContact_Country { get; set; }

        public string citation_Title { get; set; }
        public string citation_Code { get; set; }
        public string citation_Date { get; set; }
        public string citation_DataType { get; set; }
        public string IdentificationInfo_Abstract { get; set; }
        public string IdentificationInfo_Purpose { get; set; }
        public string IdentificationInfo_Status { get; set; }
        public string HierarchyLevel { get; set; }

        public BoundingBox BBox { get; set; }

        public List<DescriptiveKeyword> DescriptiveKeywords { get; set; }
        public List<ContainsOperation> ContainsOperations { get; set; }

        public Attributes_Dataset attributes_Dataset { get; set; }
        public Attributes_Service attributes_Service { get; set; }
    }
}
