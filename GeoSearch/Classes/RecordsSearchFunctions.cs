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
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Browser;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;
using GeoSearch.CSWQueryServiceReference;
using GeoSearch.QualityQueryServiceReference;
using GeoSearch.StatisticsServiceReference;
using GeoSearch.OtherQueryFunctionsServiceReference;

namespace GeoSearch
{
    public class RecordsSearchFunctions
    {
        //Flag used to show whether search allow be trigger now or not
        public static bool cannotStartSearchYet = false;
        //Flag used to show the searching state
        public static bool isSearchNow = false;
      
        //public SearchingResultPage searchingResultPage = null;
        
        public string resultString = null;
        public string contentToSearching = null;
        public SearchingContent searchingContentObject = null;
        public SBAVocabulary vocabularyObject = null;

        //sort rules
        public string sortingRule = ConstantCollection.SortBy_Relevance;
        private DateTime time_BeforeSearch;
        
        public CSWQueryServiceClient cswQueryServiceClient = new CSWQueryServiceClient();
        public QualityQueryServiceClient qualityQueryBrokerClient = new QualityQueryServiceClient();

        //jizhe

        //public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.CLHCSWURLString, ConstantCollection.GOSCSWURLString, ConstantCollection.USGIN_AASG_CSWURLString });
        //public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.USGIN_AASG_CSWURLString});
        //public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.CLHCSWURLString });
        //public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.GOSCSWURLString });

        public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.PolarCIString });

        //public List<string> catalogURLList = new List<string>(new string[] { ConstantCollection.CLHCSWURLString });

        //express all catalogs' status info, i.e., is terminated with fail or not. 
        public List<CatalogsSearchStatus> catalogsSearchStatusList;
        //all search time for each request in iteration calculated from first request. 
        public List<double> allSearchTimeFromFirstRequest;
        //use to save which catalogs have take into account of total matched number's calculation  
        //public List<string> hasCountedInTotalNumber_catalogURLList = new List<string>();
        //identify if all search sources'searching is over
        public bool isAllSearchTerminated = false;
        //Total matched records number in different catalogs 
        public int allMatchedRecordsNumberInSearchedCatalogs = 0;
        //Idetification for indicate if this is the first request for a search task and no response back yet 
        public bool isFirstInvokeToSearch = true;
        //the number of records have retrieved back to client
        public int currentGotRecordsNumber = 0;
        //the potential records number will be retrieved back to client, if all current requests are responsed. 
        //public int potentialRecordsNumber = 0;
        //Client side needed maximum records number
        public int maxRecordsExpectToGet = int.Parse(ConstantCollection.maxRecords);

        public static int IDCount = 0;
        public int ID;
        public QualityQueryFunctions queryPerformanceFunctions;
        public OtherQueryFuntions otherQueryFuntions;

        //public StatisticsServiceClient statisticsServiceClient = new StatisticsServiceClient();
        //silverlight user client's IP address
        //public string CLientIPAddress = "";

        public bool isResult_CategorizedInTabItems = true;

        //创建一个新的RecordsSearchFunctions对象，并且让SearchingResultPage的ID和它的ID一致
        public RecordsSearchFunctions()
        {
            IDCount++;
            SearchingResultPage.ID = ID = IDCount;
            cswQueryServiceClient.getRecords_BasicSearchCompleted += new EventHandler<getRecords_BasicSearchCompletedEventArgs>(proxy_GetRecords_BasicSearchCompleted);
            cswQueryServiceClient.getRecords_AdvancedSearchCompleted += new EventHandler<getRecords_AdvancedSearchCompletedEventArgs>(proxy_GetRecords_AdvancedSearchCompleted);
            cswQueryServiceClient.getRecords_QuickSearchBySBACompleted += new EventHandler<getRecords_QuickSearchBySBACompletedEventArgs>(proxy_getRecords_QuickSearchBySBACompleted);
            queryPerformanceFunctions = new QualityQueryFunctions(ID);
            otherQueryFuntions = new OtherQueryFuntions();
        }

        //to check if the content is searchable before send out the request.
        //if the content is empty or just blank charactor or other unsearchable content,
        //the request is blocked at client side for effectivity purpose.
        public static bool isContentSearchable(string searchContent)
        {
            bool searchable = true;
            if (searchContent == null || searchContent.Trim().Equals(""))
                searchable = false;
            return searchable;
        }

        private void initialCatalogStutus()
        {
            catalogsSearchStatusList = new List<CatalogsSearchStatus>();
            foreach (string catalogUrl in catalogURLList)
            {
                CatalogsSearchStatus catalogsSearchStatus = new CatalogsSearchStatus(catalogUrl, false, false);
                catalogsSearchStatusList.Add(catalogsSearchStatus);
            }
            OtherQueryFuntions.allSearchTimeFromFirstRequest = allSearchTimeFromFirstRequest = new List<double>();
        }

        public void BasicSearch_Using_WCFService(String SearchingContent, string startPosition, string maxRecords)
        {

            System.Diagnostics.Debug.WriteLine("=============");

            contentToSearching = SearchingContent;
            maxRecordsExpectToGet = int.Parse(maxRecords);
            initialCatalogStutus();

            foreach (string catalogURL in catalogURLList)
            {
                cswQueryServiceClient.getRecords_BasicSearchAsync(contentToSearching, ConstantCollection.startPosition, ConstantCollection.firstTimeSearchNum, catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);

                //Thread t = new Thread(new ThreadStart(proxy.getRecords_BasicSearchAsync(contentToSearching, SearchingFunctions.startPosition, SearchingFunctions.firstTimeSearchNum, CSWURL)));
                //threads.Add(t);
                //t.Start();
            }
            time_BeforeSearch = DateTime.Now;
        }

        //Iterate to get all the records(each step only get certain number of records)
        public void proxy_GetRecords_BasicSearchCompleted(object sender, getRecords_BasicSearchCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                cannotStartSearchYet = false;
            }
            else
            {
                SearchingResultPage.setSearchButtonEnabled();
                CSWGetRecordsSearchResults results = e.Result;
                iterateGetAllRecords(results);
            }
        }

        public void AdvancedSearch_Using_WCFService(SearchingContent searchingContent, string startPosition, string maxRecords, List<string> catalogURLList)
        {
            contentToSearching = "";
            searchingContentObject = searchingContent;
            maxRecordsExpectToGet = int.Parse(maxRecords);
            this.catalogURLList = catalogURLList;
            initialCatalogStutus();
            foreach (string catalogURL in catalogURLList)
            {
                cswQueryServiceClient.getRecords_AdvancedSearchAsync(searchingContent, ConstantCollection.startPosition, ConstantCollection.firstTimeSearchNum, catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);
            }
            time_BeforeSearch = DateTime.Now;
        }

        //Iterate to get all the records(each step only get certain number of records)
        public void proxy_GetRecords_AdvancedSearchCompleted(object sender, getRecords_AdvancedSearchCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //records = null;
                cannotStartSearchYet = false;
            }
            else
            {
                CSWGetRecordsSearchResults results = e.Result;
                iterateGetAllRecords(results);
            }
        }

        public void BrowseBySBA_Using_WCFService(SBAVocabulary vocabulary, string startPosition, string maxRecords)
        {
            contentToSearching = "";
            vocabularyObject = vocabulary;
            maxRecordsExpectToGet = int.Parse(maxRecords);
            initialCatalogStutus();

            foreach (string catalogURL in catalogURLList)
            {
                cswQueryServiceClient.getRecords_QuickSearchBySBAAsync(vocabulary, ConstantCollection.startPosition, ConstantCollection.firstTimeSearchNum, catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);
            }
            time_BeforeSearch = DateTime.Now;
        }

        void proxy_getRecords_QuickSearchBySBACompleted(object sender, getRecords_QuickSearchBySBACompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //records = null;
                cannotStartSearchYet = false;
            }
            else
            {
                CSWGetRecordsSearchResults results = e.Result;
                iterateGetAllRecords(results);
            }
        }

        private void iterateGetAllRecords(CSWGetRecordsSearchResults results)
        {
            if (results == null)
            {
                return;
            }
            // if current searched records num equal or larger than expect, then stop current search and do not add records into result list any more.
            if (currentGotRecordsNumber >= maxRecordsExpectToGet)
                return;

            TimeSpan tspan = DateTime.Now.Subtract(time_BeforeSearch);
            double time1 = tspan.TotalMilliseconds;
            double responseTime = time1 / 1000;
            allSearchTimeFromFirstRequest.Add(responseTime);

            int numberOfRecordsMatchedInCurrentCatalog = results.numberOfRecordsMatched;

            string catalogURL = results.MetadataRepositoryURL;
            CatalogsSearchStatus currentCatalogStatus = null;
            foreach (CatalogsSearchStatus catalogsSearchStatus in catalogsSearchStatusList)
            {
                if (catalogsSearchStatus.CatalogURL.Equals(catalogURL))
                    currentCatalogStatus = catalogsSearchStatus;
            }
            if (!currentCatalogStatus.hasGotTotalMatchedNumber)
            {
                allMatchedRecordsNumberInSearchedCatalogs += results.numberOfRecordsMatched;
                currentCatalogStatus.matchedNumber = results.numberOfRecordsMatched;
                currentCatalogStatus.hasGotTotalMatchedNumber = true;
            }

            ObservableCollection<Record> records = null;
            records = results.recordList;
            bool continueSearching = false;
            string seachingStatus = ConstantCollection.searchStatus_InProcessing;

            //foreach (Record record in records)
            //{
            //    StringList a = new StringList();
            //    a.AddRange(record.SBAs);
            //    record.SBAListObject = a;
            //}

            if (ConstantCollection.isQueryServicePerformanceScore && (!ConstantCollection.queryServicePerformanceScoreAtServerSide))
            {
                if (records != null && records.Count > 0)
                {
                    queryPerformanceFunctions.getGeospatialServiceQoSSummaryInfo(records);
                }
            }
            if (ConstantCollection.isQueryServerLocationLonLat && (!ConstantCollection.queryServerLocationLonLatAtServerSide))
            {
                if (records != null && records.Count > 0)
                {
                    ObservableCollection<string> URLList = new ObservableCollection<string>();
                    foreach (Record record in records)
                    {
                        //currently, we only query the server information of OGC web services 
                        if (record.Type.Equals(ConstantCollection.ServiceType_CSW) || record.Type.Equals(ConstantCollection.ServiceType_WMS) || record.Type.Equals(ConstantCollection.ServiceType_WCS) || record.Type.Equals(ConstantCollection.ServiceType_WPS) || record.Type.Equals(ConstantCollection.ServiceType_WFS))
                        {
                            if (record.URLLocation == null || record.URLLocation.Trim().Equals(""))
                            {
                                URLList.Add(record.AccessURL);
                            }
                        }
                    }

                    if (URLList.Count > 0)
                    {
                        otherQueryFuntions.getServerInformationFromURLS(URLList, ID);
                    }
                }
            }


            int local_potentialNumber = currentGotRecordsNumber + results.numberOfRecordsReturned;
            int potentialNumber = local_potentialNumber;
            foreach (CatalogsSearchStatus catalogsSearchStatus in catalogsSearchStatusList)
            {
                if (catalogsSearchStatus.isTerminated == false && catalogsSearchStatus.potentioalRetrieveNumber > 0)
                    potentialNumber += catalogsSearchStatus.potentioalRetrieveNumber;
            }

            int nextRecordInCurrentCatalog = results.nextRecord;

            if (nextRecordInCurrentCatalog == 0 || nextRecordInCurrentCatalog > numberOfRecordsMatchedInCurrentCatalog)
                nextRecordInCurrentCatalog = numberOfRecordsMatchedInCurrentCatalog;
            currentCatalogStatus.nextRecord = nextRecordInCurrentCatalog;

            //if potentialNumber less than maxRecordsWantToResult, and nextRecordInCurrentCatalog less then numberOfRecordsMatchedInCurrentCatalog, then continue search
            if (potentialNumber < maxRecordsExpectToGet && nextRecordInCurrentCatalog < numberOfRecordsMatchedInCurrentCatalog)
            {
                continueSearching = true;
                seachingStatus = ConstantCollection.searchStatus_InProcessing;
                //User can start a new search now, no needs to want all the records of current are queried. 
                cannotStartSearchYet = false;
            }
            else
            {
                cannotStartSearchYet = false;
                continueSearching = false;

                //if local potentialNumber greater than maxRecordsWantToResult, then change status 
                if (local_potentialNumber >= maxRecordsExpectToGet)
                {
                    seachingStatus = ConstantCollection.searchStatus_Finished;
                    isAllSearchTerminated = true;
                }
                else if (nextRecordInCurrentCatalog >= numberOfRecordsMatchedInCurrentCatalog)
                {
                    currentCatalogStatus.isTerminated = true;

                    bool allFinished = true;
                    foreach (CatalogsSearchStatus catalogsSearchStatus in catalogsSearchStatusList)
                    {
                        if (catalogsSearchStatus.isTerminated == false)
                            allFinished = false;
                    }

                    if (allFinished)
                    {
                        seachingStatus = ConstantCollection.searchStatus_Finished;
                        isAllSearchTerminated = true;
                    }
                    else
                        seachingStatus = ConstantCollection.searchStatus_InProcessing;
                }
            }

            //if zero record is responsed and the search status is abnormal, then set the search status to show the problems. 
            if (results.numberOfRecordsMatched == 0 && results.numberOfRecordsReturned == 0 && results.SearchStatus != null)
            {
                seachingStatus = results.SearchStatus;

                //if response error also means this catalog's search is over
                currentCatalogStatus.isTerminated = true;
                currentCatalogStatus.isFailed = true;

                bool allFinished = true;
                bool allFailed = true;
                foreach (CatalogsSearchStatus catalogsSearchStatus in catalogsSearchStatusList)
                {
                    if (catalogsSearchStatus.isTerminated == false)
                        allFinished = false;
                    if (catalogsSearchStatus.isFailed == false)
                        allFailed = false;
                }

                if (allFinished)
                {
                    isAllSearchTerminated = true;
                    if (!allFailed)
                        seachingStatus = ConstantCollection.searchStatus_Finished;
                }
            }

            //if currentSearchedNum less than maxRecordsWantToResult and numOfRecords, continue search
            if (continueSearching)
            {
                int requestRecordsNumber = ConstantCollection.eachInvokeSearchNum_ExceptFirstTime;
                if (potentialNumber < maxRecordsExpectToGet)
                {
                    //if (maxRecordsExpectToGet - potentialNumber) < retrieve interval, set retrieve interval to that value
                    int num = maxRecordsExpectToGet - potentialNumber;
                    if (num < requestRecordsNumber)
                        requestRecordsNumber = num;
                }
                //if number of rest matched records is less then retrieve interval, just set current retrieve interval to the rest number
                int num1 = currentCatalogStatus.matchedNumber - results.nextRecord + 1;
                if (num1 > 0 && num1 < requestRecordsNumber)
                    requestRecordsNumber = num1;


                int nextRecord = results.nextRecord;
                if (searchingContentObject != null)
                    cswQueryServiceClient.getRecords_AdvancedSearchAsync(searchingContentObject, nextRecord + "", requestRecordsNumber + "", catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);
                else if (vocabularyObject != null)
                    cswQueryServiceClient.getRecords_QuickSearchBySBAAsync(vocabularyObject, nextRecord + "", requestRecordsNumber + "", catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);
                else
                    cswQueryServiceClient.getRecords_BasicSearchAsync(contentToSearching, nextRecord + "", requestRecordsNumber + "", catalogURL, ConstantCollection.queryServicePerformanceScoreAtServerSide && ConstantCollection.isQueryServicePerformanceScore, ConstantCollection.calculateRelevanceAtServerSide && ConstantCollection.isCalculateRelevance);

                currentCatalogStatus.potentioalRetrieveNumber = requestRecordsNumber;
            }
            SearchingResultPage currentResultPage = null;
            if (isFirstInvokeToSearch)
            {
                currentGotRecordsNumber = results.numberOfRecordsReturned;
                SearchingResultPage.showSearchingResultPage(contentToSearching, currentGotRecordsNumber, allMatchedRecordsNumberInSearchedCatalogs, responseTime, seachingStatus, records, sortingRule, isResult_CategorizedInTabItems);
                isFirstInvokeToSearch = false;
            }
            else
            {
                App app = (App)Application.Current;
                UIElement uiElement = app.RootVisual;
                if (uiElement is UserControl)
                {
                    UserControl control = uiElement as UserControl;
                    UIElement root = control.Content;
                    if (root is SearchingResultPage)
                    {
                        currentResultPage = root as SearchingResultPage;
                        //if user trigger new search during current search stage, current search results will be ignored and this iterative search will be ended.
                        //if not, these search results will be add to current result page
                        if (SearchingResultPage.ID == ID)
                        {
                            int newAddNum = results.numberOfRecordsReturned;
                            bool willOverflow = false;
                            if (currentGotRecordsNumber + newAddNum > maxRecordsExpectToGet)
                            {
                                newAddNum = maxRecordsExpectToGet - currentGotRecordsNumber;
                                willOverflow = true;
                            }
                            int numAfterAddOperation = newAddNum + currentGotRecordsNumber;
                            currentResultPage.setSearchStutusBarInformation(numAfterAddOperation, allMatchedRecordsNumberInSearchedCatalogs, responseTime, seachingStatus);

                            if (!willOverflow)
                            {
                                if (currentResultPage.noRecord == true)
                                {
                                    currentResultPage.setResultPageContents(contentToSearching, numAfterAddOperation, allMatchedRecordsNumberInSearchedCatalogs, responseTime, seachingStatus, records, sortingRule);
                                }

                                else
                                {
                                    if (records != null)
                                        foreach (Record record in records)
                                        {
                                            ClientSideRecord newRecord = ClientSideRecord.cloneRecord(record);
                                            currentResultPage.insertRecordsIntoResultPage(newRecord, sortingRule);
                                        }
                                    currentResultPage.setRecordCountTextBlockValue();
                                }
                            }
                            else
                            {
                                int i = 0;
                                foreach (Record record in records)
                                {
                                    if (i++ < newAddNum)
                                    {
                                        ClientSideRecord newRecord = ClientSideRecord.cloneRecord(record);
                                        currentResultPage.insertRecordsIntoResultPage(newRecord, sortingRule);
                                    }
                                }
                                currentResultPage.setRecordCountTextBlockValue();
                            }
                            currentGotRecordsNumber = numAfterAddOperation;
                        }
                        else
                            return;
                    }
                }
                currentCatalogStatus.potentioalRetrieveNumber = 0;
            }

            if (isAllSearchTerminated)
            {
                if (currentResultPage != null)
                    currentResultPage.SearchSummaryViewer.Visibility = Visibility.Visible;
            }
        }



        //public ObservableCollection<Record> BasicSearch(String SearchingContent, string startPosition, string maxRecords)
        //{    
        //    if (SearchingContent == null || SearchingContent.Trim().Equals(""))
        //        return null;

        //    contentToSearching = SearchingContent;

        //    string postRequest = "<?xml version='1.0' encoding='UTF-8'?>"
        //        + "<csw:GetRecords xmlns:csw='http://www.opengis.net/cat/csw/2.0.2' xmlns='http://www.opengis.net/cat/csw/2.0.2'"
        //        + " xmlns:gmd='http://www.isotc211.org/2005/gmd' xmlns:ogc='http://www.opengis.net/ogc' xmlns:gml='http://www.opengis.net/gml'"
        //        + " xmlns:rim='urn:oasis:names:tc:ebxml-regrep:xsd:rim:3.0' service='CSW' version='2.0.2' outputFormat='application/xml'" +
        //        " outputSchema='http://www.opengis.net/cat/csw/2.0.2' "
        //        + "resultType='" + resultType + "' maxRecords='" + maxRecords + "' startPosition='" + startPosition + "'>"
        //         + "<csw:Query typeNames='csw:Record'>"
        //            + "<csw:ElementSetName>"
        //                + elementSetName
        //            + "</csw:ElementSetName>"
        //            + "<csw:Constraint version='1.1.0'>"
        //                + "<ogc:Filter>"
        //                    + "<ogc:PropertyIsLike escapeChar='\' singleChar='?' wildCard='*'>"
        //                        + "<ogc:PropertyName>anyText</ogc:PropertyName>"
        //                        + "<ogc:Literal>"
        //                            + contentToSearching
        //                        + "</ogc:Literal>"
        //                    + "</ogc:PropertyIsLike>"
        //                + "</ogc:Filter>"
        //            + "</csw:Constraint>"
        //         + "</csw:Query>"
        //    + "</csw:GetRecords>";

        //    object[] parameters = new object[2];
        //    parameters[0] = CSWURL;
        //    parameters[1] = postRequest;

        //    DateTime time_BeforeSearch = DateTime.Now;
        //    resultString = (string)HtmlPage.Window.Invoke("doRequestUsingPost", parameters);

        //    ObservableCollection<Record> records = getRecordsFromResponseXML(resultString);

        //    TimeSpan tspan = DateTime.Now.Subtract(time_BeforeSearch);
        //    double time1 = tspan.TotalMilliseconds;
        //    responseTime = time1 / 1000;

        //    return records;
        //}

        //public ObservableCollection<Record> getRecordsFromResponseXML(string response)
        //{
        //    ObservableCollection<Record> Records = new ObservableCollection<Record>();
        //    XDocument doc = XDocument.Parse(response);
        //    if (doc != null)
        //    {
        //        XElement rootElement = doc.Root;
        //        XElement SearchResults = rootElement.Element(XName.Get("SearchResults", "http://www.opengis.net/cat/csw/2.0.2"));
        //        if (SearchResults != null)
        //        {
        //            XAttribute numberOfRecordsMatched = SearchResults.Attribute("numberOfRecordsMatched");
        //            string numString = numberOfRecordsMatched.Value;

        //            if (!"0".Equals(numString))
        //                numOfRecords = int.Parse(numString, System.Globalization.NumberStyles.AllowDecimalPoint);

        //        }
        //        if (numOfRecords > 0)
        //        {
        //            IEnumerable<XElement> recordsList = SearchResults.Elements(XName.Get("Record", "http://www.opengis.net/cat/csw/2.0.2"));
        //            foreach (XElement recordElement in recordsList)
        //            {
        //                String id = null;
        //                String title = "unknown";
        //                String type = "unknown";
        //                String source = "unknown";
        //                String accessURL = "unknown";
        //                String relevance = "";
        //                String description = "unknown";

        //                XElement idElement = recordElement.Element(XName.Get("identifier", "http://purl.org/dc/elements/1.1/"));
        //                if (idElement != null && idElement.Value != null && idElement.Value.Trim() != "")
        //                    id = idElement.Value;
        //                XElement titleElement = recordElement.Element(XName.Get("title", "http://purl.org/dc/elements/1.1/"));
        //                if (titleElement != null && titleElement.Value != null && titleElement.Value.Trim() != "")
        //                    title = titleElement.Value;
        //                XElement typeElement = recordElement.Element(XName.Get("type", "http://purl.org/dc/elements/1.1/"));
        //                if (typeElement != null && typeElement.Value != null && typeElement.Value.Trim() != "")
        //                    type = typeElement.Value;
        //                XElement sourceElement = recordElement.Element(XName.Get("source", "http://purl.org/dc/elements/1.1/"));
        //                if (sourceElement != null && sourceElement.Value != null && sourceElement.Value.Trim() != "")
        //                    source = sourceElement.Value;
        //                XElement urlElement = recordElement.Element(XName.Get("URI", "http://purl.org/dc/elements/1.1/"));
        //                if (urlElement != null && urlElement.Value != null && urlElement.Value.Trim() != "")
        //                    accessURL = urlElement.Value;
        //                XElement descriptionElement = recordElement.Element(XName.Get("abstract", "http://purl.org/dc/terms/"));
        //                if (descriptionElement != null && descriptionElement.Value != null && descriptionElement.Value.Trim() != "")
        //                    description = descriptionElement.Value;

        //                Searching.Record record = new Searching.Record();
        //                record.ID = id;
        //                record.Title = title;
        //                record.Type = type;
        //                record.Source = source;
        //                record.MetadataAccessURL = CSWURL + "?&service=CSW&request=GetRecordById&version=2.0.2&ElementSetName=full&outputSchema=csw:IsoRecord&id=" + id;
        //                record.AccessURL = accessURL;
        //                record.Relevance = relevance;
        //                record.Description = description;

        //                Records.Add(record);
        //            }
        //        }
        //    }
        //    return Records;
        //}

        //public string getRecordDetailMetadata(string id)
        //{
        //    string detail = null;

        //    if (id == null || id.Trim().Equals(""))
        //        return null;

        //    string postRequest = "<?xml version='1.0' encoding='UTF-8'?>"
        //        + "<csw:GetRecordById xmlns:csw='http://www.opengis.net/cat/csw/2.0.2' service='CSW' version='2.0.2' outputSchema='csw:IsoRecord'>"
        //            + "<csw:Id>"
        //                + id
        //            + "</csw:Id>"
        //        + "</csw:GetRecordById>";

        //    object[] parameters = new object[2];
        //    parameters[0] = clearingHouseUrlString;
        //    parameters[1] = postRequest;

        //    detail = (string)HtmlPage.Window.Invoke("doRequestUsingPost", parameters);

        //    return detail;
        //}

    }

    public class CatalogsSearchStatus
    {
        public string CatalogURL;
        public bool isTerminated;
        public bool isFailed;
        public int matchedNumber = 0;
        public int potentioalRetrieveNumber = 0;
        public bool hasGotTotalMatchedNumber = false;
        public int nextRecord;

        public CatalogsSearchStatus(String url, bool terminated, bool failed)
        {
            CatalogURL = url;
            isTerminated = terminated;
            isFailed = failed;
        }

    }
}
