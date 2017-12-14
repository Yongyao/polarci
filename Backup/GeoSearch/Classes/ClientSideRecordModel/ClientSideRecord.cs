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
using System.ComponentModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public class ClientSideRecord : INotifyPropertyChanged
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

        public string GeoExtensionDescription { get; set; }
        public string ServerLocationDescription { get; set; }

        //Resource's potential Social Benifit Areas
        private StringList _SBAs;
        public StringList SBAs
        {
            get { return _SBAs; }
            set
            {
                _SBAs = value;
                NotifyProperty("SBAs");
            }
        }

        //formats support by this resource 
        private StringList _Formats;
        public StringList Formats
        {
            get { return _Formats; }
            set
            {
                _Formats = value;
                NotifyProperty("Formats");
            }
        }

        //a series of keywords to describe this resource
        private StringList _DescriptiveKeywords;
        public StringList DescriptiveKeywords
        {
            get { return _DescriptiveKeywords; }
            set
            {
                _DescriptiveKeywords = value;
                NotifyProperty("DescriptiveKeywords");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyProperty(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                  new PropertyChangedEventArgs(propName));
        }

        #endregion

        public static ClientSideRecord cloneRecord(Record record)
        {
            if(record != null)
            {
                ClientSideRecord newRecord = new ClientSideRecord();
                newRecord.Abstract = record.Abstract;
                newRecord.AccessURL = record.AccessURL;
                newRecord.bbox = record.bbox;
                newRecord.GeneralType = record.GeneralType;
                newRecord.GeoExtensionDescription = record.GeoExtensionDescription;
                newRecord.ID = record.ID;
                newRecord.isDataCore = record.isDataCore;
                newRecord.MetadataAccessURL = record.MetadataAccessURL;
                newRecord.Provider = record.Provider;
                newRecord.Quality = record.Quality;
                newRecord.RealServiceURL = record.RealServiceURL;
                newRecord.Relevance = record.Relevance;
                newRecord.ServerLocationDescription = record.ServerLocationDescription;
                newRecord.Source = record.Source;
                newRecord.Title = record.Title;
                newRecord.Type = record.Type;
                newRecord.URLLocation = record.URLLocation;

                if (record.SBAs.Count > 0)
                {
                    newRecord.SBAs = new StringList();
                    newRecord.SBAs.AddRange(record.SBAs);
                }

                if (record.DescriptiveKeywords.Count > 0)
                {
                    newRecord.DescriptiveKeywords = new StringList();
                    newRecord.DescriptiveKeywords.AddRange(record.DescriptiveKeywords);
                }

                if (record.Formats.Count > 0)
                {
                    newRecord.Formats = new StringList();
                    newRecord.Formats.AddRange(record.Formats);
                }

                return newRecord;
            }
            else
                return null;
        }
    }
}
