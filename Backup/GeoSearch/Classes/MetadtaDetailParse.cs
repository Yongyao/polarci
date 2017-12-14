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
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;

namespace GeoSearch
{
    public class MetadtaDetailParse
    {
        public static string gmd_namespace = "http://www.isotc211.org/2005/gmd";
        public static string gco_namespace = "http://www.isotc211.org/2005/gco";
        public static string srv_namespace = "http://www.isotc211.org/2005/srv";

        public static string hierarchyLevel_dataset = "dataset";
        public static string hierarchyLevel_service = "service";
        
        public static Metadata parseMetadataFromXMLString(string content)
        {
            Metadata metadata = null;
            XDocument doc = null;
            try
            {
                doc = XDocument.Parse(content);
            }
            catch (Exception e)
            {
                e.GetType().ToString();
                return null;
            }
            if (doc != null)
            {
                metadata = new Metadata();
                XElement rootElement = doc.Root;
                if (rootElement != null)
                {                
                    XElement MD_Metadata = null;
                    if (rootElement.Name.Equals(XName.Get("MD_Metadata", gmd_namespace)))
                        MD_Metadata = rootElement;
                    else
                        MD_Metadata = rootElement.Element(XName.Get("MD_Metadata", gmd_namespace));
                    //metadata detail describe by using ISO 19139
                    if (MD_Metadata != null)
                    {
                        //parse hierarchyLevel
                        XElement hierarchyLevel = MD_Metadata.Element(XName.Get("hierarchyLevel", gmd_namespace));
                        if (hierarchyLevel != null)
                        {
                            XElement MD_ScopeCode = getFirstDescendantElementMatchGivenNameInElement(MD_Metadata, XName.Get("MD_ScopeCode", gmd_namespace));
                            string value1 = MD_ScopeCode.Value;
                            if (value1 != null && (!value1.Trim().Equals("")))
                            {
                                metadata.HierarchyLevel = value1;
                            }
                            else
                            {
                                XAttribute a = MD_ScopeCode.Attribute(XName.Get("codeListValue"));
                                if (a != null)
                                {
                                    value1 = a.Value;
                                    if (value1 != null)
                                        metadata.HierarchyLevel = value1;
                                }
                            }
                        }

                        XElement identificationInfo = MD_Metadata.Element(XName.Get("identificationInfo", gmd_namespace));
                        if (identificationInfo != null)
                        {
                            //parse pointOfContact part
                            XElement pointOfContact = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("pointOfContact", gmd_namespace));
                            if (pointOfContact == null)
                                pointOfContact = MD_Metadata.Element(XName.Get("contact", gmd_namespace));

                            if (pointOfContact != null)
                            {
                                XElement CI_ResponsibleParty = pointOfContact.Element(XName.Get("CI_ResponsibleParty", gmd_namespace));
                                if (CI_ResponsibleParty != null)
                                {
                                    XElement individualName = CI_ResponsibleParty.Element(XName.Get("individualName", gmd_namespace));
                                    if (individualName != null)
                                    {
                                        string value = getElementValue(individualName);
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.pointOfContact_IndividualName = value;
                                        }
                                    }

                                    XElement organisationName = CI_ResponsibleParty.Element(XName.Get("organisationName", gmd_namespace));
                                    if (organisationName != null)
                                    {
                                        string value = getElementValue(organisationName);
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.pointOfContact_OrganisationName = value;
                                        }
                                    }

                                    XElement positionName = CI_ResponsibleParty.Element(XName.Get("positionName", gmd_namespace));
                                    if (positionName != null)
                                    {
                                        string value = getElementValue(positionName);
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.pointOfContact_PositionName = value;
                                        }
                                    }

                                    XElement role = CI_ResponsibleParty.Element(XName.Get("role", gmd_namespace));
                                    if (role != null)
                                    {
                                        XElement CI_RoleCode = role.Element(XName.Get("CI_RoleCode", gmd_namespace));
                                        string value = CI_RoleCode.Value;
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.pointOfContact_Role = value;
                                        }
                                        else
                                        {
                                            XAttribute a = CI_RoleCode.Attribute(XName.Get("codeListValue"));
                                            if (a != null)
                                            {
                                                value = a.Value;
                                                if (value != null)
                                                    if (value.Equals(pointOfContact))
                                                        value = "Point of contact: Party who can be contacted for acquiring knowledge about or acquisition of the resource";
                                                metadata.pointOfContact_Role = value;
                                            }

                                        }
                                    }

                                    XElement contactInfo = CI_ResponsibleParty.Element(XName.Get("contactInfo", gmd_namespace));
                                    if (contactInfo != null)
                                    {
                                        XElement voice = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("voice", gmd_namespace));
                                        if (voice != null)
                                        {
                                            string value = getElementValue(voice);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_Voice = value;
                                            }
                                        }

                                        XElement facsimile = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("facsimile", gmd_namespace));
                                        if (facsimile != null)
                                        {
                                            string value = getElementValue(facsimile);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_Facsimile = value;
                                            }
                                        }

                                        XElement deliveryPoint = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("deliveryPoint", gmd_namespace));
                                        if (deliveryPoint != null)
                                        {
                                            string value = getElementValue(deliveryPoint);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_DeliveryPoint = value;
                                            }
                                        }

                                        XElement city = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("city", gmd_namespace));
                                        if (city != null)
                                        {
                                            string value = getElementValue(city);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_City = value;
                                            }
                                        }

                                        XElement administrativeArea = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("administrativeArea", gmd_namespace));
                                        if (administrativeArea != null)
                                        {
                                            string value = getElementValue(administrativeArea);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_AdministrativeArea = value;
                                            }
                                        }

                                        XElement postalCode = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("postalCode", gmd_namespace));
                                        if (postalCode != null)
                                        {
                                            string value = getElementValue(postalCode);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_PostalCode = value;
                                            }
                                        }

                                        XElement country = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("country", gmd_namespace));
                                        if (country != null)
                                        {
                                            string value = getElementValue(country);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_Country = value;
                                            }
                                        }

                                        XElement electronicMailAddress = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("electronicMailAddress", gmd_namespace));
                                        if (electronicMailAddress != null)
                                        {
                                            string value = getElementValue(electronicMailAddress);
                                            if (value != null && (!value.Trim().Equals("")))
                                            {
                                                metadata.pointOfContact_Email = value;
                                            }
                                        }

                                        XElement onlineResource = getFirstDescendantElementMatchGivenNameInElement(contactInfo, XName.Get("onlineResource", gmd_namespace));
                                        if (onlineResource != null)
                                        {
                                            XElement URL = getFirstDescendantElementMatchGivenNameInElement(onlineResource, XName.Get("URL", gmd_namespace));
                                            if (URL != null)
                                            {
                                                string value = URL.Value;
                                                if (value != null && (!value.Trim().Equals("")))
                                                {
                                                    metadata.pointOfContact_OnlineResource = value;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            //parse citation part
                            XElement citation = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("citation", gmd_namespace));
                            if (citation != null)
                            {
                                XElement CI_Citation = citation.Element(XName.Get("CI_Citation", gmd_namespace));
                                if (CI_Citation != null)
                                {
                                    XElement title = CI_Citation.Element(XName.Get("title", gmd_namespace));
                                    if (title != null)
                                    {
                                        string value = getElementValue(title);
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.citation_Title = value;
                                        }
                                    }

                                    XElement CI_Date = getFirstDescendantElementMatchGivenNameInElement(CI_Citation, XName.Get("CI_Date", gmd_namespace));
                                    if (CI_Date != null)
                                    {
                                        XElement date = CI_Date.Element(XName.Get("date", gmd_namespace));
                                        if (date != null)
                                        {
                                            XElement element = date.FirstNode as XElement;
                                            if (element != null)
                                            {
                                                string value = element.Value;
                                                if (value != null && (!value.Trim().Equals("")))
                                                {
                                                    metadata.citation_Date = value;
                                                }
                                            }
                                        }


                                        XElement dateType = CI_Date.Element(XName.Get("dateType", gmd_namespace));
                                        if (dateType != null)
                                        {
                                            XElement CI_DateTypeCode = dateType.Element(XName.Get("CI_DateTypeCode", gmd_namespace));
                                            if (CI_DateTypeCode != null)
                                            {
                                                string value1 = CI_DateTypeCode.Value;
                                                if (value1 != null && (!value1.Trim().Equals("")))
                                                {
                                                    metadata.citation_DataType = value1;
                                                }
                                                else
                                                {
                                                    XAttribute a = CI_DateTypeCode.Attribute(XName.Get("codeListValue"));
                                                    if (a != null)
                                                    {
                                                        value1 = a.Value;
                                                        if (value1 != null)
                                                            metadata.citation_DataType = value1;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    XElement code = getFirstDescendantElementMatchGivenNameInElement(CI_Citation, XName.Get("code", gmd_namespace));
                                    if (code != null)
                                    {
                                        string value = getElementValue(code);
                                        if (value != null && (!value.Trim().Equals("")))
                                        {
                                            metadata.citation_Code = value;
                                        }
                                    }
                                }
                            }

                            //parse abstract, purpose and status
                            XElement abstract1 = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("abstract", gmd_namespace));
                            if (abstract1 != null)
                            {
                                string value = getElementValue(abstract1);
                                if (value != null && (!value.Trim().Equals("")))
                                {
                                    metadata.IdentificationInfo_Abstract = value;
                                }
                            }
                            XElement purpose = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("purpose", gmd_namespace));
                            if (purpose != null)
                            {
                                string value = getElementValue(purpose);
                                if (value != null && (!value.Trim().Equals("")))
                                {
                                    metadata.IdentificationInfo_Purpose = value;
                                }
                            }
                            XElement status = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("status", gmd_namespace));
                            if (status != null)
                            {
                                XElement MD_ProgressCode = status.Element(XName.Get("MD_ProgressCode", gmd_namespace));
                                string value = MD_ProgressCode.Value;
                                if (value != null && (!value.Trim().Equals("")))
                                {
                                    metadata.IdentificationInfo_Status = value;
                                }
                                else
                                {
                                    XAttribute a = MD_ProgressCode.Attribute(XName.Get("codeListValue"));
                                    if (a != null)
                                    {
                                        value = a.Value;
                                        if (value != null)
                                            metadata.IdentificationInfo_Status = value;
                                    }
                                }
                            }

                            //parse Boundingbox
                            XElement EX_GeographicBoundingBox = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("EX_GeographicBoundingBox", gmd_namespace));
                            if (EX_GeographicBoundingBox != null)
                            {
                                BoundingBox BBox = new BoundingBox();
                                metadata.BBox = BBox;
                                XElement westBoundLongitude = EX_GeographicBoundingBox.Element(XName.Get("westBoundLongitude", gmd_namespace));
                                XElement eastBoundLongitude = EX_GeographicBoundingBox.Element(XName.Get("eastBoundLongitude", gmd_namespace));
                                XElement southBoundLatitude = EX_GeographicBoundingBox.Element(XName.Get("southBoundLatitude", gmd_namespace));
                                XElement northBoundLatitude = EX_GeographicBoundingBox.Element(XName.Get("northBoundLatitude", gmd_namespace));
                                if (westBoundLongitude != null)
                                {
                                    XElement Decimal = westBoundLongitude.Element(XName.Get("Decimal", gco_namespace));
                                    string value = Decimal.Value;
                                    if (value != null && (!value.Trim().Equals("")))
                                    {
                                        metadata.BBox.BBox_West = value;
                                    }
                                }
                                if (eastBoundLongitude != null)
                                {
                                    XElement Decimal = eastBoundLongitude.Element(XName.Get("Decimal", gco_namespace));
                                    string value = Decimal.Value;
                                    if (value != null && (!value.Trim().Equals("")))
                                    {
                                        metadata.BBox.BBox_East = value;
                                    }
                                }
                                if (southBoundLatitude != null)
                                {
                                    XElement Decimal = southBoundLatitude.Element(XName.Get("Decimal", gco_namespace));
                                    string value = Decimal.Value;
                                    if (value != null && (!value.Trim().Equals("")))
                                    {
                                        metadata.BBox.BBox_South = value;
                                    }
                                }
                                if (northBoundLatitude != null)
                                {
                                    XElement Decimal = northBoundLatitude.Element(XName.Get("Decimal", gco_namespace));
                                    string value = Decimal.Value;
                                    if (value != null && (!value.Trim().Equals("")))
                                    {
                                        metadata.BBox.BBox_North = value;
                                    }
                                }

                                //metadata.BBox = changeEastWestForError(metadata.BBox);
                            }

                            //parse DescriptiveKeyword
                            metadata.DescriptiveKeywords = new List<DescriptiveKeyword>();
                            IEnumerable<XElement> descriptiveKeywords = identificationInfo.Descendants(XName.Get("descriptiveKeywords", gmd_namespace));
                            if (descriptiveKeywords != null)
                            {
                                foreach (XElement descriptiveKeyword in descriptiveKeywords)
                                {
                                    DescriptiveKeyword d = new DescriptiveKeyword();
                                    metadata.DescriptiveKeywords.Add(d);
                                    XElement MD_Keywords = descriptiveKeyword.Element(XName.Get("MD_Keywords", gmd_namespace));
                                    if (MD_Keywords != null)
                                    {
                                        IEnumerable<XElement> keywords = MD_Keywords.Elements(XName.Get("keyword", gmd_namespace));
                                        if (keywords != null)
                                        {
                                            List<string> Keywords = new List<string>();
                                            d.Keywords = Keywords;
                                            foreach (XElement keyword in keywords)
                                            {
                                                if (keyword != null)
                                                {
                                                    string value = getElementValue(keyword);
                                                    if (value != null && !(value.Trim().Equals("")))
                                                        d.Keywords.Add(value);
                                                }
                                            }
                                        }

                                        XElement MD_KeywordTypeCode = getFirstDescendantElementMatchGivenNameInElement(descriptiveKeyword, XName.Get("MD_KeywordTypeCode", gmd_namespace));
                                        if (MD_KeywordTypeCode != null)
                                        {
                                            string value = MD_KeywordTypeCode.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                                d.ThesaurusCategory = value;
                                            else
                                            {
                                                XAttribute a = MD_KeywordTypeCode.Attribute(XName.Get("codeListValue"));
                                                if (a != null)
                                                {
                                                    value = a.Value;
                                                    if (value != null)
                                                        d.ThesaurusCategory = value;
                                                }
                                            }
                                        }

                                        XElement ThesaurusName = getFirstDescendantElementMatchGivenNameInElement(descriptiveKeyword, XName.Get("title", gmd_namespace));
                                        if (ThesaurusName != null)
                                        {
                                            string value = getElementValue(ThesaurusName);
                                            if (value != null && !(value.Trim().Equals("")))
                                                d.ThesaurusName = value;
                                        }
                                    }
                                }
                            }

                            //parse ContainsOperation
                            metadata.ContainsOperations = new List<ContainsOperation>();
                            IEnumerable<XElement> containsOperations = identificationInfo.Descendants(XName.Get("containsOperations", srv_namespace));
                            if (containsOperations != null)
                            {
                                foreach (XElement containsOperation in containsOperations)
                                {
                                    XElement SV_OperationMetadata = containsOperation.Element(XName.Get("SV_OperationMetadata", srv_namespace));
                                    if (SV_OperationMetadata!=null)
                                    {
                                        ContainsOperation c = new ContainsOperation();
                                        metadata.ContainsOperations.Add(c);
                                        XElement operationName = SV_OperationMetadata.Element(XName.Get("operationName", srv_namespace));
                                        if (operationName != null)
                                        {
                                            string value = getElementValue(operationName);
                                            if (value != null && !(value.Trim().Equals("")))
                                                c.operationName = value;
                                        }

                                        IEnumerable<XElement> DCPList = SV_OperationMetadata.Descendants(XName.Get("DCPList", srv_namespace));
                                        if (DCPList != null)
                                        {
                                            List<string> DCP1 = new List<string>();
                                            c.DCP = DCP1;
                                            foreach (XElement DCP in DCPList)
                                            {
                                                if (DCP != null)
                                                {
                                                    string value = DCP.Value;
                                                    if (value != null && !(value.Trim().Equals("")))
                                                        c.DCP.Add(value);
                                                    else
                                                    {
                                                        XAttribute a = DCP.Attribute(XName.Get("codeListValue"));
                                                        if (a != null)
                                                        {
                                                            value = a.Value;
                                                            if (value != null)
                                                                c.DCP.Add(value);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        IEnumerable<XElement> CI_OnlineResources = SV_OperationMetadata.Descendants(XName.Get("CI_OnlineResource", gmd_namespace));
                                        if (CI_OnlineResources != null)
                                        {
                                            List<OnlineResource> OnlineResources = new List<OnlineResource>();
                                            c.OnlineResources = OnlineResources;
                                            foreach (XElement CI_OnlineResource in CI_OnlineResources)
                                            {
                                                if (CI_OnlineResource != null)
                                                {
                                                    OnlineResource onlineResource = new OnlineResource();
                                                    c.OnlineResources.Add(onlineResource);
                                                    XElement URL = getFirstDescendantElementMatchGivenNameInElement(CI_OnlineResource, XName.Get("URL", gmd_namespace));
                                                    if (URL != null)
                                                    {
                                                        string value = URL.Value;
                                                        if (value != null && !(value.Trim().Equals("")))
                                                            onlineResource.URL = value;
                                                    }

                                                    XElement Protocol = getFirstDescendantElementMatchGivenNameInElement(CI_OnlineResource, XName.Get("protocol", gmd_namespace));
                                                    if (Protocol != null)
                                                    {
                                                        string value = getElementValue(Protocol);
                                                        if (value != null && !(value.Trim().Equals("")))
                                                            onlineResource.Protocol = value;
                                                    }

                                                    XElement Description = getFirstDescendantElementMatchGivenNameInElement(CI_OnlineResource, XName.Get("description", gmd_namespace));
                                                    if (Description != null)
                                                    {
                                                        string value = getElementValue(Description);
                                                        if (value != null && !(value.Trim().Equals("")))
                                                            onlineResource.Description = value;
                                                    }

                                                    XElement CI_OnLineFunctionCode = getFirstDescendantElementMatchGivenNameInElement(CI_OnlineResource, XName.Get("CI_OnLineFunctionCode", gmd_namespace));
                                                    if (CI_OnLineFunctionCode != null)
                                                    {
                                                        XAttribute a = CI_OnLineFunctionCode.Attribute(XName.Get("codeListValue"));
                                                        if (a != null)
                                                        {
                                                            string value = a.Value;
                                                            if (value != null)
                                                                onlineResource.Function = value;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (metadata.HierarchyLevel != null)
                            {
                                if (metadata.HierarchyLevel.Equals(hierarchyLevel_dataset))
                                {
                                    Attributes_Dataset attributes_Dataset = new Attributes_Dataset();
                                    metadata.attributes_Dataset = attributes_Dataset;
                                    XElement MD_MaintenanceFrequencyCode = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("MD_MaintenanceFrequencyCode", gmd_namespace));
                                    if (MD_MaintenanceFrequencyCode != null)
                                    {
                                        XAttribute a = MD_MaintenanceFrequencyCode.Attribute(XName.Get("codeListValue"));
                                        if (a != null)
                                        {
                                            string value = a.Value;
                                            if (value != null)
                                                if (value.Equals("asNeeded"))
                                                    value = "As needed: Data is updated as deemed necessary";
                                            attributes_Dataset.ResourceMaintenance = value;
                                        }
                                    }

                                    XElement supplementalInformation = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("supplementalInformation", gmd_namespace));
                                    if (supplementalInformation != null)
                                    {
                                        string value = getElementValue(supplementalInformation);
                                        if (value != null && !(value.Trim().Equals("")))
                                            attributes_Dataset.SupplementalInformation = value;
                                    }

                                    XElement MD_BrowseGraphic = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("MD_BrowseGraphic", gmd_namespace));
                                    if (MD_BrowseGraphic != null)
                                    {
                                        XElement fileName = getFirstDescendantElementMatchGivenNameInElement(MD_BrowseGraphic, XName.Get("fileName", gmd_namespace));
                                        if (fileName != null)
                                        {
                                            string value = getElementValue(fileName);
                                            if (value != null && !(value.Trim().Equals("")))
                                                attributes_Dataset.GraphicOverview_FileURL = value;
                                        }

                                        XElement fileDescription = getFirstDescendantElementMatchGivenNameInElement(MD_BrowseGraphic, XName.Get("fileDescription", gmd_namespace));
                                        if (fileDescription != null)
                                        {
                                            string value = getElementValue(fileDescription);
                                            if (value != null && !(value.Trim().Equals("")))
                                                attributes_Dataset.GraphicOverview_Description = value;
                                        }
                                    }

                                    XElement MD_LegalConstraints = getFirstDescendantElementMatchGivenNameInElement(identificationInfo, XName.Get("MD_LegalConstraints", gmd_namespace));
                                    if (MD_LegalConstraints != null)
                                    {
                                        XElement accessConstraints = getFirstDescendantElementMatchGivenNameInElement(MD_LegalConstraints, XName.Get("accessConstraints", gmd_namespace));
                                        if (accessConstraints != null)
                                        {
                                            XElement RestrictionCode = accessConstraints.Element(XName.Get("MD_RestrictionCode", gmd_namespace));
                                            if (RestrictionCode != null)
                                            {
                                                string value = RestrictionCode.Value;
                                                if (value != null && !(value.Trim().Equals("")))
                                                    attributes_Dataset.ResourceConstraints_AccessConstraints = value;
                                                else
                                                {
                                                    XAttribute a = RestrictionCode.Attribute(XName.Get("codeListValue"));
                                                    if (a != null)
                                                    {
                                                        value = a.Value;
                                                        if (value != null)
                                                            attributes_Dataset.ResourceConstraints_AccessConstraints = value;
                                                    }
                                                }
                                            }
                                        }

                                        XElement useConstraints = getFirstDescendantElementMatchGivenNameInElement(MD_LegalConstraints, XName.Get("useConstraints", gmd_namespace));
                                        if (useConstraints != null)
                                        {
                                            XElement RestrictionCode = useConstraints.Element(XName.Get("MD_RestrictionCode", gmd_namespace));
                                            if (RestrictionCode != null)
                                            {
                                                string value = RestrictionCode.Value;
                                                if (value != null && !(value.Trim().Equals("")))
                                                    attributes_Dataset.ResourceConstraints_UseConstraints = value;
                                                else
                                                {
                                                    XAttribute a = RestrictionCode.Attribute(XName.Get("codeListValue"));
                                                    if (a != null)
                                                    {
                                                        value = a.Value;
                                                        if (value != null)
                                                            attributes_Dataset.ResourceConstraints_UseConstraints = value;
                                                    }
                                                }
                                            }
                                        }

                                        XElement otherConstraints = getFirstDescendantElementMatchGivenNameInElement(MD_LegalConstraints, XName.Get("otherConstraints", gmd_namespace));
                                        if (otherConstraints != null)
                                        {
                                            string value = getElementValue(otherConstraints);
                                            if (value != null && !(value.Trim().Equals("")))
                                                attributes_Dataset.ResourceConstraints_OtherConstraints = value;
                                        }
                                    }
                                }
                                else if (metadata.HierarchyLevel.Equals(hierarchyLevel_service))
                                {
                                    Attributes_Service attributes_Service = new Attributes_Service();
                                    metadata.attributes_Service = attributes_Service;
                                    XElement SV_ServiceIdentification = identificationInfo.Element(XName.Get("SV_ServiceIdentification", srv_namespace));
                                    if (SV_ServiceIdentification != null)
                                    {
                                        XElement serviceType = SV_ServiceIdentification.Element(XName.Get("serviceType", srv_namespace));
                                        if (serviceType != null)
                                        {
                                            XElement LocalName = serviceType.Element(XName.Get("LocalName", gco_namespace));
                                            if (LocalName != null)
                                            {
                                                string value = LocalName.Value;
                                                if (value != null && !(value.Trim().Equals("")))
                                                    attributes_Service.ServiceType = value;
                                            }
                                        }

                                        XElement serviceTypeVersion = SV_ServiceIdentification.Element(XName.Get("serviceTypeVersion", srv_namespace));
                                        if (serviceTypeVersion != null)
                                        {
                                            string value = getElementValue(serviceTypeVersion);
                                            if (value != null && !(value.Trim().Equals("")))
                                                attributes_Service.ServiceVersion = value;
                                        }

                                        XElement accessProperties = SV_ServiceIdentification.Element(XName.Get("accessProperties", srv_namespace));
                                        if (accessProperties != null)
                                        {
                                            XElement MD_StandardOrderProcess = accessProperties.Element(XName.Get("MD_StandardOrderProcess", gmd_namespace));
                                            if (MD_StandardOrderProcess != null)
                                            {
                                                XElement fees = accessProperties.Element(XName.Get("fees", gmd_namespace));
                                                if (fees != null)
                                                {
                                                    string value = getElementValue(fees);
                                                    if (value != null && !(value.Trim().Equals("")))
                                                        attributes_Service.Fees = value;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    //metadata described using other format than 19139
                    else
                    {
                        //metadata described using CSDGM format
                        if (rootElement.Name.LocalName.Equals("metadata"))
                        {
                            XElement idinfo = rootElement.Element("idinfo");
                            if (idinfo != null)
                            {
                                XElement citation = idinfo.Element("citation");
                                if (citation != null)
                                {
                                    XElement citeinfo = citation.Element("citeinfo");
                                    if (citeinfo != null)
                                    {
                                        XElement title = citeinfo.Element("title");
                                        if (title != null)
                                        {
                                            string value = title.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                                metadata.citation_Title = value.Trim('\r').Trim('\n');
                                        }
                                        XElement pubdate = citeinfo.Element("pubdate");
                                        if (pubdate != null)
                                        {
                                            string value = pubdate.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                                metadata.citation_Date = value.Trim('\r').Trim('\n');
                                        }
                                    }
                                }

                                XElement descript = idinfo.Element("descript");
                                if (descript != null)
                                {
                                    XElement abstractEle = descript.Element("abstract");
                                    if (abstractEle != null)
                                    {
                                        string value = abstractEle.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.IdentificationInfo_Abstract = value.TrimStart('\r').TrimStart('\n').TrimEnd('\r').TrimEnd('\n');
                                    }

                                    XElement purpose = descript.Element("purpose");
                                    if (purpose != null)
                                    {
                                        string value = purpose.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.IdentificationInfo_Purpose = value.TrimStart('\r').TrimStart('\n').TrimEnd('\r').TrimEnd('\n');
                                    }
                                }

                                XElement status = idinfo.Element("status");
                                if (status != null)
                                {
                                    XElement progress = status.Element("progress");
                                    if (progress != null)
                                    {
                                        string value = progress.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.IdentificationInfo_Status = value.Trim('\r').Trim('\n');
                                    }
                                }

                                XElement timeperd = idinfo.Element("timeperd");
                                if (timeperd != null)
                                {
                                    XElement current = timeperd.Element("current");
                                    if (current != null)
                                    {
                                        string value = current.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.citation_DataType = value;
                                    }
                                }
                                
                                XElement accconst = idinfo.Element("accconst");
                                XElement useconst = idinfo.Element("useconst");
                                if(accconst != null || useconst!= null)
                                {                 
                                    metadata.attributes_Dataset = new Attributes_Dataset();
                                    if (accconst != null)
                                    {
                                        string value = accconst.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.attributes_Dataset.ResourceConstraints_AccessConstraints = value;
                                    }

                                    if (useconst != null)
                                    {
                                        string value = useconst.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.attributes_Dataset.ResourceConstraints_UseConstraints = value;
                                    }
                                }

                                XElement spdom = idinfo.Element("spdom");
                                if (spdom != null)
                                {
                                    XElement bounding = spdom.Element("bounding");
                                    if (bounding != null)
                                    {
                                        metadata.BBox = new BoundingBox();
                                        XElement westbc = bounding.Element("westbc");
                                        if (westbc != null)
                                        {
                                            string value = westbc.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                            {
                                                metadata.BBox.BBox_West = value.Trim('\r').Trim('\n');
                                            }
                                        }

                                        XElement eastbc = bounding.Element("eastbc");
                                        if (eastbc != null)
                                        {
                                            string value = eastbc.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                            {
                                                metadata.BBox.BBox_East = value.Trim('\r').Trim('\n');
                                            }
                                        }

                                        XElement northbc = bounding.Element("northbc");
                                        if (northbc != null)
                                        {
                                            string value = northbc.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                            {
                                                metadata.BBox.BBox_North = value.Trim('\r').Trim('\n');
                                            }
                                        }

                                        XElement southbc = bounding.Element("southbc");
                                        if (southbc != null)
                                        {
                                            string value = southbc.Value;
                                            if (value != null && !(value.Trim().Equals("")))
                                            {
                                                metadata.BBox.BBox_South = value.Trim('\r').Trim('\n');
                                            }
                                        }

                                        //metadata.BBox = changeEastWestForError(metadata.BBox);
                                    }
                                }

                                XElement keywords = idinfo.Element("keywords");
                                if (keywords != null)
                                {
                                    IEnumerable<XElement> keywordCategoryList = keywords.Elements();
                                    if (keywordCategoryList != null)
                                    {
                                        metadata.DescriptiveKeywords = new List<DescriptiveKeyword>();
                                        foreach (XElement keywordCategory in keywordCategoryList)
                                        {
                                            if (keywordCategory.HasElements)
                                            {
                                                DescriptiveKeyword dk = new DescriptiveKeyword();
                                                metadata.DescriptiveKeywords.Add(dk);
                                                string ThesaurusName = keywordCategory.Name.LocalName;
                                                dk.ThesaurusName = ThesaurusName;

                                                XElement ThesaurusCategory = keywordCategory.Element(ThesaurusName + "kt");
                                                if (ThesaurusCategory != null)
                                                {
                                                    string value = ThesaurusCategory.Value;
                                                    if (value != null && !(value.Trim().Equals("")))
                                                        dk.ThesaurusCategory = value;
                                                }
                                                IEnumerable<XElement> keywordList = keywordCategory.Elements(ThesaurusName + "key");
                                                if (keywordList != null)
                                                {
                                                    dk.Keywords = new List<string>();
                                                    foreach (XElement keyword in keywordList)
                                                    {
                                                        string value = keyword.Value;
                                                        if (value != null && !(value.Trim().Equals("")))
                                                            dk.Keywords.Add(value);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            XElement Esri = rootElement.Element("Esri");
                            if (Esri != null)
                            {
                                XElement resourceType = Esri.Element("resourceType");
                                if (resourceType != null)
                                {
                                    string value = resourceType.Value;
                                    if (value != null && !(value.Trim().Equals("")))
                                        metadata.HierarchyLevel = value;
                                }

                                XElement ServiceType = Esri.Element("ServiceType");
                                if (ServiceType != null)
                                {
                                    metadata.attributes_Service = new Attributes_Service();
                                    {
                                        string value = resourceType.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.attributes_Service.ServiceType = value;
                                    }

                                    XElement ServiceParam = Esri.Element("ServiceParam");
                                    if (ServiceParam != null)
                                    {
                                        string value = ServiceParam.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.attributes_Service.ServicePerameters = value;
                                    }

                                    XElement Server = Esri.Element("Server");
                                    if (Server != null)
                                    {
                                        string value = Server.Value;
                                        if (value != null && !(value.Trim().Equals("")))
                                            metadata.attributes_Service.ServerURL = value;
                                    }
                                }

                                //XElement onlink = Esri.Element("onlink");
                                //if (onlink != null)
                                //{
                                //    string value = onlink.Value;
                                //    if (value != null && !(value.Trim().Equals("")))
                                //        metadata. = value;
                                //}
                            }

                            XElement metainfo = rootElement.Element("metainfo");
                            if (metainfo != null)
                            {
                                XElement metc = metainfo.Element("metc");
                                if (metc != null && metc.HasElements)
                                {
                                    parseContactInfo(metc, metadata);
                                }
                                else
                                {
                                    XElement ptcontac = idinfo.Element("ptcontac");
                                    if (ptcontac != null && ptcontac.HasElements)
                                    {
                                        parseContactInfo(ptcontac, metadata);
                                    }
                                    else
                                    {
                                        XElement distinfo = rootElement.Element("distinfo");
                                        if (distinfo != null && distinfo.HasElements)
                                        {
                                            XElement distrib = distinfo.Element("distrib");
                                            if (distrib != null && distrib.HasElements)
                                            {
                                                parseContactInfo(distrib, metadata);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return metadata;
        }

        //private static String shorterDoubleString(String value)
        //{
        //    //int indexPoint = value.LastIndexOf(".");
        //    //if (indexPoint > 0)
        //    //    value = value.Substring(0, indexPoint);
        //    return value;
        //}

        //private static BoundingBox changeEastWestForError(BoundingBox BBox1)
        //{
        //    BoundingBox BBox = BBox1;
        //    if ((!BBox.BBox_East.Equals("")) && (!BBox.BBox_West.Equals("")))
        //    {
        //        double east = double.Parse(BBox.BBox_East);
        //        double west = double.Parse(BBox.BBox_West);
        //        if (west > east)
        //        {
        //            String temp = BBox.BBox_East;
        //            BBox.BBox_East = BBox.BBox_West;
        //            BBox.BBox_West = temp;
        //        }
        //    }
        //    return BBox;
        //}

        private static void parseContactInfo(XElement parent, Metadata metadata)
        {
            XElement cntinfo = parent.Element("cntinfo");
            if (cntinfo != null)
            {
                XElement cntperp = cntinfo.Element("cntperp");
                if (cntperp != null)
                {
                    XElement cntper = cntperp.Element("cntper");
                    if (cntper != null)
                    {
                        string value = cntper.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_IndividualName = value;
                    }

                    XElement cntorg = cntperp.Element("cntorg");
                    if (cntorg != null)
                    {
                        string value = cntorg.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_OrganisationName = value;
                    }
                }

                XElement cntvoice = cntinfo.Element("cntvoice");
                if (cntvoice != null)
                {
                    string value = cntvoice.Value;
                    if (value != null && !(value.Trim().Equals("")))
                        metadata.pointOfContact_Voice = value;
                }

                XElement cntfax = cntinfo.Element("cntfax");
                if (cntfax != null)
                {
                    string value = cntfax.Value;
                    if (value != null && !(value.Trim().Equals("")))
                        metadata.pointOfContact_Facsimile = value;
                }

                XElement cntemail = cntinfo.Element("cntemail");
                if (cntemail != null)
                {
                    string value = cntemail.Value;
                    if (value != null && !(value.Trim().Equals("")))
                        metadata.pointOfContact_Email = value;
                }

                XElement cntorgp = cntinfo.Element("cntorgp");
                if (cntorgp != null)
                {
                    XElement cntorg = cntorgp.Element("cntorg");
                    if (cntorg != null)
                    {
                        string value = cntorg.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_OrganisationName = value;
                    }
                }

                XElement cntaddr = cntinfo.Element("cntaddr");
                if (cntaddr != null)
                {
                    //XElement addrtype = cntaddr.Element("addrtype");
                    //if (addrtype != null)
                    //{
                    //    string value = addrtype.Value;
                    //    if (value != null && !(value.Trim().Equals("")))
                    //        metadata. = value;
                    //}

                    XElement address = cntaddr.Element("address");
                    if (address != null)
                    {
                        string value = address.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_DeliveryPoint = value;
                    }

                    XElement city = cntaddr.Element("city");
                    if (city != null)
                    {
                        string value = city.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_City = value;
                    }

                    XElement state = cntaddr.Element("state");
                    if (state != null)
                    {
                        string value = state.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_AdministrativeArea = value;
                    }

                    XElement postal = cntaddr.Element("postal");
                    if (postal != null)
                    {
                        string value = postal.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_PostalCode = value;
                    }

                    XElement country = cntaddr.Element("country");
                    if (country != null)
                    {
                        string value = country.Value;
                        if (value != null && !(value.Trim().Equals("")))
                            metadata.pointOfContact_Country = value;
                    }
                }
            }
        }

        public static string getElementValue(XElement element)
        {
            string value = null;
            XElement ele = element.Element(XName.Get("CharacterString", gco_namespace));
            if(ele != null)
            {
                value = ele.Value;
            }
            return value;
        }

        public static XElement getFirstDescendantElementMatchGivenNameInElement(XElement element, XName xname)
        {
            IEnumerable<XElement> pointOfContacts = element.Descendants(xname);
            IEnumerator<XElement> s = pointOfContacts.GetEnumerator();
            XElement child = null;
            if (s.MoveNext())
            {
                child = s.Current;
            }
            return child;
        }
    }
}
