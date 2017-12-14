using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoSearch.Web;
using System.IO;


//Rank the Result list based on abstact, QoS, Semtatic, etc 

namespace GeoSearch.Web
{
    public class MetadataRanking
    {
        private String strTitle { get; set; }
        private String strAbstract{ get; set; }
        private List<String> strKeywordsList{ get; set; }
        private String strOnlineURL{ get; set; }
        private String strServiceType{ get; set; }
        private String strContact{ get; set; }

        private double titleRanking{ get; set; }
        private double abstractRanking { get; set; }
        private double keywordsRanking { get; set; }
        private double onlineURLRanking { get; set; }
        private double serviceTypeRanking { get; set; }
        private double contactRanking { get; set; }

        //private static int EXACT_RANKING = 4;
        //private static int HIGH_RANKING = 3;
        //private static int MIDDLE_RANKING = 2;
        //private static int LOW_RANKING = 1;
        private static int NO_RANKING = 0;

        private double overallRanking { get; set; }

        public String strQueryAnyText { get; set; }

        //private double a = 0.95;      //title
        //private double b = 0.90;      //keywords
        //private double c = 0.75;      //abstract
        //private double d = 0.65;      //contact
        //private double e = 0.95;      //service type
        
        public List<String> strQueryAnyTextList { get; set; }

        private bool adSearcher;

        //fields from SearchingContent
        public List<string> wordsInAnyText { get; set; }
        public List<string> wordsInTitle { get; set; }
        public List<string> wordsInAbstract { get; set; }
        public List<string> Keywords { get; set; }

        public MetadataRanking()
        {
            this.strTitle = null;
            this.strAbstract = null;
            this.strKeywordsList = new List<String>();
            this.strOnlineURL = null;
            this.strServiceType = null;
            this.strContact = null;
            this.strQueryAnyText = null;
            this.strQueryAnyTextList = null;

            InitialRanking();
        }

        public MetadataRanking(String strQueryAnyText, Record record)
        {
            this.strQueryAnyText = strQueryAnyText;

            this.strTitle = record.Title;
            this.strAbstract = record.Abstract;
            this.strKeywordsList = record.DescriptiveKeywords;
            this.strServiceType = record.Type;
            this.strContact = record.Provider;
            this.strOnlineURL = record.AccessURL;

            this.strQueryAnyTextList = new List<string>();
            adSearcher = false;
            InitialRanking();
        }

        public MetadataRanking(SearchingContent sc, Record record)
        {
            string temp = null;
            if(sc.wordsInAnyText != null)
            foreach(string text in sc.wordsInAnyText)
            {
                temp=text+" ";
            }

            this.strQueryAnyText = temp;
            this.strTitle = record.Title;
            this.strAbstract = record.Abstract;
            this.strKeywordsList = record.DescriptiveKeywords;
            this.strServiceType = record.Type;
            this.strContact = record.Provider;
            this.strOnlineURL = record.AccessURL;

            this.strQueryAnyTextList = new List<string>();
            adSearcher = true;

            wordsInAbstract = sc.wordsInAbstract;
            wordsInAnyText = sc.wordsInAnyText;
            wordsInTitle = sc.wordsInTitle;
            Keywords = sc.Keywords;
	    
            InitialRanking();
        }
        private void InitialRanking()
        {
            this.titleRanking = NO_RANKING;
            this.abstractRanking = NO_RANKING;
            this.keywordsRanking = NO_RANKING;
            this.onlineURLRanking = NO_RANKING;
            this.serviceTypeRanking = NO_RANKING;
            this.contactRanking = NO_RANKING;

            this.overallRanking = -1;
        }

        public double DoRanking()
        {
            InitialQueryList();
            CalculateTitleRanking();
            CalculateAbstractRanking();
            CalculateURLRanking();
            CaluculateServiceType();
            CalculateKeywordsRanking();

            if (!adSearcher)
                DoSimpleRanking();
            else
            {
                if (CheckOnlyAnyTextSearcher())
                    DoSimpleRanking();
                else if (CheckNotAnyTextSearcher())
                {
                    DoRankingWithoutAnyText();
                }
                else
                    DoSimpleRanking();

            }
            return overallRanking;
        }

        public Boolean CheckOnlyAnyTextSearcher()
        {
            Boolean b = false;
            if (wordsInAnyText == null )
                b = true;
            return b;
        }

        public Boolean CheckNotAnyTextSearcher()
        {
            Boolean b = false;
            //if (wordsInAnyText == null && wordsInAbstract == null && wordsInTitle != null && Keywords == null)
            //    b = true;
            if (wordsInAnyText == null)
                b = true;
            return b;
        }
        private void DoRankingWithoutAnyText()
        {
            overallRanking = 1.0;
            //minus from the overallRanking if fields are null
            if (strTitle == null || strTitle == "" || strTitle == " ")
                this.overallRanking -= 0.5;
            if (strAbstract == null || strAbstract == "" || strAbstract == " ")
                this.overallRanking -= 0.05;
            if (strKeywordsList == null)
                this.overallRanking -= 0.3;

            //Consider contact Ranking
            this.overallRanking -= 0.9 * 0.01 + contactRanking * 0.01;

            //Consider onlineURLRanking
            this.onlineURLRanking -= 1 * 0.01 + onlineURLRanking * 0.01;
        }
        private void DoSimpleRanking()
        {
            if (true)
            {
                //
                if (titleRanking != 0 && abstractRanking == 0 && keywordsRanking == 0)
                {
                    this.overallRanking = titleRanking * (0.98 - 0.02) + contactRanking * 0.01 + onlineURLRanking * 0.01;
                }
                // stronger
                if (titleRanking != 0 && abstractRanking != 0 && keywordsRanking == 0)
                {
                    this.overallRanking = titleRanking * 0.8 + abstractRanking * 0.17 + +contactRanking * 0.01 + onlineURLRanking * 0.01;
                }
                // stronge
                if (titleRanking != 0 && abstractRanking != 0 && keywordsRanking != 0)
                {
                    this.overallRanking = titleRanking * 0.8 + abstractRanking * 0.05 + keywordsRanking * 0.13 + +contactRanking * 0.01 + onlineURLRanking * 0.01;
                }

                // titleRanking == 0
                if (titleRanking == 0 && abstractRanking == 0 && keywordsRanking != 0)
                {
                    this.overallRanking = keywordsRanking * 0.92 + contactRanking * 0.01 + onlineURLRanking * 0.01;
                }

                if (titleRanking == 0 && abstractRanking != 0 && keywordsRanking != 0)
                {
                    this.overallRanking = keywordsRanking * 0.8 + abstractRanking * 0.14 + titleRanking * 0.85 + contactRanking * 0.01 + onlineURLRanking * 0.01;
                }

                if (titleRanking == 0 && abstractRanking != 0 && keywordsRanking == 0)
                {
                    this.overallRanking = abstractRanking * 0.86 + contactRanking * 0.01 + onlineURLRanking * 0.01;
                }

                //if all of these 3 fields are 0, it represents searching directs other fields
                if (titleRanking == 0 && abstractRanking == 0 && keywordsRanking == 0)
                {
                    this.overallRanking = 0.82 + contactRanking * 0.01 + onlineURLRanking * 0.01;
                }
            }
            if (IsQueryingWxS())
            {
                this.overallRanking *= serviceTypeRanking;
                if (this.overallRanking == 0)
                {
                    this.overallRanking = serviceTypeRanking;
                }
            }

            //minus from the overallRanking if fields are null
            if (strTitle == null || strTitle == "" || strTitle == " ")
                this.overallRanking -= 0.5;
            if (strAbstract == null || strAbstract == "" || strAbstract == " ")
                this.overallRanking -= 0.05;
            if (strKeywordsList == null)
                this.overallRanking -= 0.3;


        }
        	
	    private void InitialQueryList() {
            //Analyzer  analyzer = new StandardAnalyzer(Version.LUCENE_29);
            //StringReader reader = new StringReader(this.strQueryAnyText);
            //TokenStream ts = analyzer.TokenStream(this.strQueryAnyText, reader);
            //string newstr = "";
            //Token t = new Token();
            // t = ts.Next();
            //while (t != null)
            //{
            //    newstr = t.TermText();
            //    this.strQueryAnyTextList.Add(newstr);
            //    t = ts.Next();
            //} 
            if (strQueryAnyText != null)
            {
                String[] aa = strQueryAnyText.Split(' ');
                strQueryAnyTextList = new List<string>(aa);
            }
	    }

        private void CalculateTitleRanking()
        {
            int frequency = 0;
            if(this.strTitle == null)
            {
                this.titleRanking = 0;
                return;
            }
            foreach (String s in strQueryAnyTextList)
            {
                if (ContainString(strTitle, s) || ContainsWxS(s))
                {
                    frequency += 1;
                }
                    this.titleRanking = frequency / strQueryAnyTextList.Count;
            }
        }

        private void CalculateAbstractRanking()
        {
            int frequency = 0;
            if (this.strAbstract == null)
            {
                this.abstractRanking = 0;
                return;
            }
            foreach (String s in strQueryAnyTextList)
            {
                if (ContainString(strAbstract, s) || ContainsWxS(s))
                {
                    frequency += 1;
                }
                this.abstractRanking = frequency / strQueryAnyTextList.Count;
            }
        }

        private void CalculateKeywordsRanking()
        {
            int frequency = 0;
            if (this.strKeywordsList == null)
            {
                this.keywordsRanking = 0;
                return;
            }
            String strKeywords = " ";
            foreach (String s in strKeywordsList)
            {
                strKeywords = strKeywords + s + " ";
            }
            foreach (String s in strQueryAnyTextList)
            {
                if (ContainString(strKeywords, s) || ContainsWxS(s))
                {
                    frequency += 1;
                }
                this.abstractRanking = frequency / strQueryAnyTextList.Count;
            }
            
        }

        private void CalculateContactRanking()
        {
            if (this.strContact == null)
            {
                this.contactRanking = 0;
                return;
            }
            else if (ContainString(strOnlineURL, "USGS") || ContainString(strOnlineURL, "NASA"))
            {
                this.contactRanking = 0.9;
                return;
            }
            else if (ContainString(strOnlineURL, "ESRI"))
            {
                this.contactRanking = 0.8;
                return;
            }
            else
            {
                this.contactRanking = 0.8;
                return;
            }
        }

        private void CalculateURLRanking()
        {
            if (this.strOnlineURL == null)
            {
                this.onlineURLRanking = 0;
                return;
            }
            else if (ContainString(strServiceType, "WMS") || ContainString(strServiceType, "WCS")
                || ContainString(strServiceType, "WFS") || ContainString(strServiceType, "WPS"))
            {
                this.onlineURLRanking = 1;
                return;
            }
            else if (ContainString(strOnlineURL, "USGS") || ContainString(strOnlineURL, "NASA"))
            {
                this.onlineURLRanking = 0.9;
                return;
            }
            else if (ContainString(strOnlineURL,"ESRI"))
            {
                this.onlineURLRanking = 0.7;
                return;
            }
            else
            {
                this.onlineURLRanking = 0.4;
                return;
            }
        }
        private void CaluculateServiceType()
        {
            
            if (this.strServiceType == null)
            {
                this.serviceTypeRanking = 0;
                return;
            }

            if (!IsQueryingWxS())
            {
                this.serviceTypeRanking = 1;
                return;
            }
            else if (ContainString(strServiceType,"OGC:WMS")|| ContainString(strServiceType, "OGC:WCS")
                || ContainString(strServiceType, "OGC:WFS") || ContainString(strServiceType, "OGC:WPS"))
            {
                this.serviceTypeRanking = 1;
                return;
            }
            else if (ContainString(strServiceType, "WMS") || ContainString(strServiceType, "WCS")
                || ContainString(strServiceType, "WFS") || ContainString(strServiceType, "WPS"))
            {
                this.serviceTypeRanking = 0.9;
                return;
            }
            else
            {
                this.serviceTypeRanking = 0.8;
                return;
            }
        }

        //check if it is querying WxS
        private Boolean ContainsWxS(String s)
        {
            if (ContainString(s, "WMS") || ContainString(s, "WFS") || ContainString(s, "WPS") || ContainString(s, "WCS"))
            {
                return true;
            }
            else
                return false;
        }

        //check if it is querying WxS
        private Boolean IsQueryingWxS()
        {
            if (ContainString(strQueryAnyText, "WMS") || ContainString(strQueryAnyText, "WFS") 
                || ContainString(strQueryAnyText, "WPS") || ContainString(strQueryAnyText, "WCS"))
            {
                return true;
            }
            else
                return false;
        }

        //
        private Boolean ContainString(String strOriginal, String strCompare)
        {
            if (strOriginal!= null && strOriginal.ToUpper().Contains(strCompare.ToUpper()))
                return true;
            else
                return false;
        }
    }
}
