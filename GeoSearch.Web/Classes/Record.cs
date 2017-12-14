using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;


// information about one record
namespace GeoSearch.Web
{
    public class Record
    {
        //Metadata's URL in Registry Center where we can found detail of this metadata
        public string MetadataAccessURL { get; set; }
        //The provider of metadata resource 
        public string Provider { get; set; }
        //The source or metadata, such as, GEOSS CLH, GOS, ECHO, GCMD 
        public string Source { get; set; }
        //title of this metadata
        public string Title { get; set; }
        //Abstract describe the metadata
        public string Abstract { get; set; }
        //Resource Type of this metadata record
        public string Type { get; set; }
        //Resource General Type of this metadata record
        public string GeneralType { get; set; }
        //Resource's online access URL 
        public string AccessURL { get; set; }
        //ID of this metadata in registry center
        public string ID { get; set; }
        //the relevance between the metadata and the searching subject
        public double Relevance { get; set; }
        //formats support by this resource 
        public List<string> Formats { get; set; }
        //a series of keywords to describe this resource
        public List<string> DescriptiveKeywords { get; set; }
        //the unique identifier of this metadata
        //public string UUID { get; set; }
        //the Quality of Recource
        public double Quality { get; set; }
        ////registry center's URL where found the metadata
        //public string MetadataRepositoryURL { get; set; }
        //Real Service Access URL for geting service performance info of service type resource  
        public string RealServiceURL { get; set; }
        //indicate the georesource is described as geossDataCore or not 
        public bool isDataCore { get; set; }
        //BoundingExtent: West, North, East, South
        public BBox bbox { get; set; }
        //the lon and lat location of resource URL
        public string URLLocation { get; set; }
        //Resource's potential Social Benifit Areas
        public List<string> SBAs { get; set; }
        public string GeoExtensionDescription { get; set; }
        public string ServerLocationDescription { get; set; }
        //public object SBAListObject { get; set; }
    }

    //public class Record : INotifyPropertyChanged
    //{
    //    //Metadata's URL in Registry Center where we can found detail of this metadata
    //    public string MetadataAccessURL { get; set; }
    //    //The provider of metadata resource 
    //    public string Provider { get; set; }
    //    //The source or metadata, such as, GEOSS CLH, GOS, ECHO, GCMD 
    //    public string Source { get; set; }
    //    //title of this metadata
    //    public string Title { get; set; }
    //    //Abstract describe the metadata
    //    public string Abstract { get; set; }
    //    //Resource Type of this metadata record
    //    public string Type { get; set; }
    //    //Resource General Type of this metadata record
    //    public string GeneralType { get; set; }
    //    //Resource's online access URL 
    //    public string AccessURL { get; set; }
    //    //ID of this metadata in registry center
    //    public string ID { get; set; }
    //    //the relevance between the metadata and the searching subject
    //    public double Relevance { get; set; }
    //    //the unique identifier of this metadata
    //    //public string UUID { get; set; }
    //    //the Quality of Recource
    //    public double Quality { get; set; }
    //    ////registry center's URL where found the metadata
    //    //public string MetadataRepositoryURL { get; set; }
    //    //Real Service Access URL for geting service performance info of service type resource  
    //    public string RealServiceURL { get; set; }
    //    //indicate the georesource is described as geossDataCore or not 
    //    public bool isDataCore { get; set; }
    //    //BoundingExtent: West, North, East, South
    //    public BBox bbox { get; set; }
    //    //the lon and lat location of resource URL
    //    public string URLLocation { get; set; }

    //    public string GeoExtensionDescription { get; set; }
    //    public string ServerLocationDescription { get; set; }

    //    private object _SBAListObject;
    //    public object SBAListObject
    //    {
    //        get { return _SBAListObject; }
    //        set
    //        {
    //            _SBAListObject = value;
    //            NotifyProperty("SBAListObject");
    //        }
    //    }
    //    //Resource's potential Social Benifit Areas
    //    //public List<string> SBAs { get; set; }
    //    //formats support by this resource 
    //    //public List<string> Formats { get; set; }
    //    //a series of keywords to describe this resource
    //    //public List<string> DescriptiveKeywords { get; set; }

    //    private StringList _SBAs;
    //    public StringList SBAs
    //    {
    //        get { return _SBAs; }
    //        set
    //        {
    //            _SBAs = value;
    //            NotifyProperty("SBAs");
    //        }
    //    }

    //    private StringList _Formats;
    //    public StringList Formats
    //    {
    //        get { return _Formats; }
    //        set
    //        {
    //            _Formats = value;
    //            NotifyProperty("Formats");
    //        }
    //    }

    //    private StringList _DescriptiveKeywords;
    //    public StringList DescriptiveKeywords
    //    {
    //        get { return _DescriptiveKeywords; }
    //        set
    //        {
    //            _DescriptiveKeywords = value;
    //            NotifyProperty("DescriptiveKeywords");
    //        }
    //    }

    //    #region INotifyPropertyChanged
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public void NotifyProperty(string propName)
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this,
    //              new PropertyChangedEventArgs(propName));
    //    }

    //    #endregion
    //}
}
