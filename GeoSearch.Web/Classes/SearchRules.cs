using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// add searching rules based on user input
namespace GeoSearch.Web
{
    public class SearchRules
    {
        public List<string> Keywords = new List<string>();
        public List<string> Keywords_eitherOne = new List<string>();
        public List<string> Keywords_without = new List<string>();
        public List<string> resourceTypes = new List<string>();
        public List<string> resourceTypes_without = new List<string>();
        public List<string> providers = new List<string>();
        public List<string> providers_without = new List<string>();

        public static SearchRules getSearchRules(string searchContent)
        {
            // jizhe
            //if (searchContent == null || searchContent.Trim().Equals(""))
                //return null;

            SearchRules rules = new SearchRules();
            string Capital_searchContent = searchContent.ToUpper();

            //resource types
            if (Capital_searchContent.Contains("WMS"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WMS);
                Capital_searchContent = Capital_searchContent.Replace("WMS", "");
            }
            if (Capital_searchContent.Contains("WEB MAP SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WMS);
                Capital_searchContent = Capital_searchContent.Replace("WEB MAP SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB MAP SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WMS);
                Capital_searchContent = Capital_searchContent.Replace("WEB MAP SERVICE", "");
            }

            if (Capital_searchContent.Contains("WFS"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WFS);
                Capital_searchContent = Capital_searchContent.Replace("WFS", "");
            }
            if (Capital_searchContent.Contains("WEB FEATURE SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WFS);
                Capital_searchContent = Capital_searchContent.Replace("WEB FEATURE SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB FEATURE SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WFS);
                Capital_searchContent = Capital_searchContent.Replace("WEB FEATURE SERVICE", "");
            }

            if (Capital_searchContent.Contains("WCS"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WCS);
                Capital_searchContent = Capital_searchContent.Replace("WCS", "");
            }
            if (Capital_searchContent.Contains("WEB COVERAGE SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WCS);
                Capital_searchContent = Capital_searchContent.Replace("WEB COVERAGE SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB COVERAGE SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WCS);
                Capital_searchContent = Capital_searchContent.Replace("WEB COVERAGE SERVICE", "");
            }

            if (Capital_searchContent.Contains("WPS"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WPS);
                Capital_searchContent = Capital_searchContent.Replace("WPS", "");
            }
            if (Capital_searchContent.Contains("WEB PROCESSING SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WPS);
                Capital_searchContent = Capital_searchContent.Replace("WEB PROCESSING SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB PROCESSING SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WPS);
                Capital_searchContent = Capital_searchContent.Replace("WEB PROCESSING SERVICE", "");
            }
            if (Capital_searchContent.Contains("WEB PROCESS SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WPS);
                Capital_searchContent = Capital_searchContent.Replace("WEB PROCESS SERVICE", "");
            }
            if (Capital_searchContent.Contains("WEB PROCESS SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_WPS);
                Capital_searchContent = Capital_searchContent.Replace("WEB PROCESS SERVICES", "");
            }

            if (Capital_searchContent.Contains("CSW"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("CSW", "");
            }
            if (Capital_searchContent.Contains("WEB CATALOGUE SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("WEB CATALOGUE SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB CATALOG SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("WEB CATALOG SERVICES", "");
            }
            if (Capital_searchContent.Contains("WEB CATALOGUE SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("WEB CATALOGUE SERVICE", "");
            }
            if (Capital_searchContent.Contains("WEB CATALOG SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("WEB CATALOG SERVICE", "");
            }
            if (Capital_searchContent.Contains("CATALOGUE SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("CATALOGUE SERVICES", "");
            }
            if (Capital_searchContent.Contains("CATALOGUE SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("CATALOGUE SERVICE", "");
            }
            if (Capital_searchContent.Contains("CATALOG SERVICES"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("CATALOG SERVICES", "");
            }
            if (Capital_searchContent.Contains("CATALOG SERVICE"))
            {
                rules.resourceTypes.Add(SearchingContent.ServiceType_CSW);
                Capital_searchContent = Capital_searchContent.Replace("CATALOG SERVICE", "");
            }

            if (Capital_searchContent.Contains("DATASETS"))
            {
                rules.resourceTypes.Add(SearchingContent.resourceType_Datasets);
                Capital_searchContent = Capital_searchContent.Replace("DATASETS", "");
            }
            if (Capital_searchContent.Contains("DATASET"))
            {
                rules.resourceTypes.Add(SearchingContent.resourceType_Datasets);
                Capital_searchContent = Capital_searchContent.Replace("DATASET", "");
            }



            //resource providers
            if (Capital_searchContent.Contains("USGS"))
            {
                rules.providers.Add("USGS");
                rules.providers.Add("U.S. Geological Survey");
                Capital_searchContent = Capital_searchContent.Replace("USGS", "");
            }
            if (Capital_searchContent.Contains("U.S. GEOLOGICAL SURVEY"))
            {
                rules.providers.Add("USGS");
                rules.providers.Add("U.S. Geological Survey");
                Capital_searchContent = Capital_searchContent.Replace("U.S. GEOLOGICAL SURVEY", "");
            }

            if (Capital_searchContent.Contains("NASA"))
            {
                rules.providers.Add("NASA");
                rules.providers.Add("National Aeronautics and Space Administration");
                rules.providers.Add("JPL");
                rules.providers.Add("Jet Propulsion Laboratory");
                Capital_searchContent = Capital_searchContent.Replace("NASA", "");
            }
            if (Capital_searchContent.Contains("NATIONAL AERONAUTICS AND SPACE ADMINISTRATION"))
            {
                rules.providers.Add("NASA");
                rules.providers.Add("National Aeronautics and Space Administration");
                rules.providers.Add("JPL");
                rules.providers.Add("Jet Propulsion Laboratory");
                Capital_searchContent = Capital_searchContent.Replace("NATIONAL AERONAUTICS AND SPACE ADMINISTRATION", "");
            }

            if (Capital_searchContent.Contains("NOAA"))
            {
                rules.providers.Add("NOAA");
                rules.providers.Add("National Oceanic and Atmospheric Administration");
                Capital_searchContent = Capital_searchContent.Replace("NOAA", "");
            }
            if (Capital_searchContent.Contains("NATIONAL OCEANIC AND ATMOSPHERIC ADMINISTRATION"))
            {
                rules.providers.Add("NOAA");
                rules.providers.Add("National Oceanic and Atmospheric Administration");
                Capital_searchContent = Capital_searchContent.Replace("NATIONAL OCEANIC AND ATMOSPHERIC ADMINSTRATION", "");
            }

            //subjects
            string[] listOfKeywords = Capital_searchContent.Split(' ');
            int len = listOfKeywords.Length;
            for (int i = 0; i < len; i++)
            {
                string keyword = listOfKeywords[i];
                if (!keyword.Trim().Equals(""))
                {
                    if (keyword.Trim().ToLower().Equals("water") || keyword.Trim().ToLower().Equals("waters"))
                    {
                        rules.Keywords_eitherOne.Add("water");
                        rules.Keywords_eitherOne.Add("ice");
                        rules.Keywords_eitherOne.Add("ocean");
                        rules.Keywords_eitherOne.Add("sea");
                        rules.Keywords_eitherOne.Add("river");
                        rules.Keywords_eitherOne.Add("lake");
                        rules.Keywords_eitherOne.Add("hydrology");
                        rules.Keywords_eitherOne.Add("rain");
                        rules.Keywords_eitherOne.Add("snow");
                        rules.Keywords_eitherOne.Add("glacier");                      
                    }
                    else
                        rules.Keywords.Add(keyword);
                }
            }
            //rules.Keywords.AddRange(listOfKeywords);
            return rules;
        }
    }
}