using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


// the result of a CSW search
namespace GeoSearch.Web
{
    public class CSWGetRecordsSearchResults
    {
        //this parameter is used to identifer the state of search: succeed or fail , when there is zero record is replied
        public string SearchStatus { get; set; }
        public string elementSet { get; set; }
        public string MetadataRepositoryURL { get; set; }
        public int numberOfRecordsMatched { get; set; }
        public int numberOfRecordsReturned { get; set; }
        public int nextRecord { get; set; }
        public List<Record> recordList { get; set; }  
    }
}