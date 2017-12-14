using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using GeoSearch.CSWQueryServiceReference;
using System.Windows.Browser;

namespace GeoSearch.Pages
{
    public partial class AdvancedSearchPage : UserControl
    {
        ObservableCollection<ResourceTypeTree> allResourceTypes = null;
        bool isNoResourceTypeSelected = true;
        bool isAllResourceTypeSelected = true;
        bool classifiedWithCategories = true;
        
        public AdvancedSearchPage()
        {
            InitializeComponent();
            allResourceTypes = ResourceTypeTree.getResourceTypeList();
            TreeView_ResourceType.ItemsSource = allResourceTypes;
       
            RetrieveNumber rn = new RetrieveNumber();
            rn.TotalNumber = int.Parse(ConstantCollection.maxRecords);
            rn.firstNumber = int.Parse(ConstantCollection.firstTimeSearchNum);
            rn.regularNumber = ConstantCollection.eachInvokeSearchNum_ExceptFirstTime;
            Grid_RetrieveNumber.DataContext = rn;

            CheckBox_QueryQoS.IsChecked = ConstantCollection.isQueryServicePerformanceScore;
            if (CheckBox_QueryQoS.IsChecked == true)
            {
                CheckBox_QueryQoS_AtServerSide.IsEnabled = true;
                CheckBox_QueryQoS_AtClientSide.IsEnabled = true;
            }
            else
            {
                CheckBox_QueryQoS_AtServerSide.IsEnabled = false;
                CheckBox_QueryQoS_AtClientSide.IsEnabled = false;
            }
            if (ConstantCollection.queryServicePerformanceScoreAtServerSide)
                CheckBox_QueryQoS_AtServerSide.IsChecked = true;
            else
                CheckBox_QueryQoS_AtClientSide.IsChecked = true;

            CheckBox_CalculateRelevance.IsChecked = ConstantCollection.isCalculateRelevance;
            if (CheckBox_CalculateRelevance.IsChecked == true)
            {
                CheckBox_CalculateRelevance_AtServerSide.IsEnabled = true;
                CheckBox_CalculateRelevance_AtClientSide.IsEnabled = false;
            }
            else
            {
                CheckBox_CalculateRelevance_AtServerSide.IsEnabled = false;
                CheckBox_CalculateRelevance_AtClientSide.IsEnabled = false;
            }
            CheckBox_CalculateRelevance_AtServerSide.IsChecked = true;
            setContentHeightWidthToFixBrowserWindowSize();
            App.Current.Host.Content.Resized += new EventHandler(content_Resized);
        }

                //设置Results_Container的高度
        public void setContentHeightWidthToFixBrowserWindowSize()
        {
            double content_height = (double)HtmlPage.Window.Eval("document.documentElement.clientHeight");
            double content_width = (double)HtmlPage.Window.Eval("document.documentElement.clientWidth");
            if (content_height < 300)
                content_height = 300;
            if (content_width < 600)
                content_width = 600;

            ContentArea_ScrollViewer.Height = content_height - 200;
        }

        //改变Browser窗体大小时的事件
        void content_Resized(object sender, EventArgs e)
        {
            setContentHeightWidthToFixBrowserWindowSize();
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                (e.OriginalSource as Control).Background = new SolidColorBrush(Colors.Yellow);
                //tbMessage.Text = e.Error.Exception.Message;
                ToolTipService.SetToolTip((e.OriginalSource as TextBox), e.Error.Exception.Message);
            }

            if (e.Action == ValidationErrorEventAction.Removed)
            {
                (e.OriginalSource as Control).Background = new SolidColorBrush(Colors.White);
                //tbMessage.Text = "";
                ToolTipService.SetToolTip((e.OriginalSource as TextBox), null);
            }
        }

        private ResourceType createResourceType(ResourceTypeTree resourceType)
        {
            ResourceType root = new ResourceType();
            root.Name = resourceType.Name;
            root.isSelected = resourceType.isSelected;
            if (root.isSelected == true)
                isNoResourceTypeSelected = false;
            else
                isAllResourceTypeSelected = false;
            root.ResourceTypeID = resourceType.ResourceTypeID;
            if (resourceType.Children != null)
            {
                root.Children = new ObservableCollection<ResourceType>();
                foreach (ResourceTypeTree rtt in resourceType.Children)
                {
                    ResourceType rt = createResourceType(rtt);
                    root.Children.Add(rt);
                }
            }
            return root;
        }

        private void Button_AdvancedSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchingContent searchingContent = createSearchingContent();
            setSearchConfig();

            if (RecordsSearchFunctions.cannotStartSearchYet == false)
            {
                RecordsSearchFunctions.cannotStartSearchYet = true;
                RecordsSearchFunctions sf = new RecordsSearchFunctions(); 
                sf.isResult_CategorizedInTabItems = classifiedWithCategories;
                sf.sortingRule = (ComboBox_SortBy.SelectedItem as ComboBoxItem).Content as string;
                if (ComboBox_SortBy.SelectedIndex != 0)
                {
                    sf.sortingRule = ((ComboBoxItem)ComboBox_SortBy.SelectedItem).Content as string;
                }
                Button_AdvancedSearch.IsEnabled = false;
                List<string> catalogURLList = getSearchCatalogURLList();
                sf.AdvancedSearch_Using_WCFService(searchingContent, ConstantCollection.startPosition, ConstantCollection.maxRecords, catalogURLList);
            }
        }

        private List<string> getSearchCatalogURLList()
        {
            List<string> catalogURLList = new List<string>();

            if (CheckBox_RecordsHost_CLH.IsChecked == true)
                catalogURLList.Add(ConstantCollection.CLHCSWURLString);
            if (CheckBox_RecordsHost_GOS.IsChecked == true)
                catalogURLList.Add(ConstantCollection.GOSCSWURLString);
            if (CheckBox_RecordsHost_USGIN_AASG_CSW.IsChecked == true)
                catalogURLList.Add(ConstantCollection.USGIN_AASG_CSWURLString);

            if (catalogURLList.Count == 0)
                catalogURLList.Add(ConstantCollection.CLHCSWURLString);
            return catalogURLList;
        }

        private SearchingContent createSearchingContent()
        {
            SearchingContent searchingContent = new SearchingContent();

            string wordsInFullText = AllOfTheWords_Content.Text;
            if (wordsInFullText != null && wordsInFullText.Trim() != "")
            {
                string[] listOfKeywords = wordsInFullText.Split(' ');
                searchingContent.wordsInAnyText = new System.Collections.ObjectModel.ObservableCollection<string>(listOfKeywords);
            }

            string ExactPhrase = ExactPhrase_Content.Text;
            if (ExactPhrase != null && ExactPhrase.Trim() != "")
            {
                searchingContent.exactPhrase = ExactPhrase;
            }

            string WithoutWords = WithoutWords_Content.Text;
            if (WithoutWords != null && WithoutWords.Trim() != "")
            {
                string[] listOfKeywords = WithoutWords.Split(' ');
                searchingContent.withoutWordsInAnytext = new System.Collections.ObjectModel.ObservableCollection<string>(listOfKeywords);
            }

            string WordsInAbstract = WordsInAbstract_Content.Text;
            if (WordsInAbstract != null && WordsInAbstract.Trim() != "")
            {
                string[] listOfKeywords = WordsInAbstract.Split(' ');
                searchingContent.wordsInAbstract = new System.Collections.ObjectModel.ObservableCollection<string>(listOfKeywords);
            }

            string WordsInTitle = WordsInTitle_Content.Text;
            if (WordsInTitle != null && WordsInTitle.Trim() != "")
            {
                string[] listOfKeywords = WordsInTitle.Split(' ');
                searchingContent.wordsInTitle = new System.Collections.ObjectModel.ObservableCollection<string>(listOfKeywords);
            }

            string WordsInKeywords = Keywords_Content.Text;
            if (WordsInKeywords != null && WordsInKeywords.Trim() != "")
            {
                string[] listOfKeywords = WordsInKeywords.Split(' ');
                searchingContent.Keywords = new System.Collections.ObjectModel.ObservableCollection<string>(listOfKeywords);
            }

            ResourceType root = new ResourceType();
            root.Children = new ObservableCollection<ResourceType>();
            foreach (ResourceTypeTree rt in allResourceTypes)
            {
                //ResourceType node = new ResourceType();
                //root.Children.Add(node);
                //createResourceType(rt, node);
                ResourceType node = createResourceType(rt);
                root.Children.Add(node);
            }
            root.isSelected = isAllResourceTypeSelected;

            searchingContent.resourceTypesTree = root;
            searchingContent.isNoResourceTypeSelected = isNoResourceTypeSelected;

            string NorthString = text_NorthBound.Text;
            string WestString = text_WestBound.Text;
            string SouthString = text_SouthBound.Text;
            string EastString = text_EastBound.Text;
            try
            {
                double NorthValue = double.Parse(NorthString);
                double SouthValue = double.Parse(SouthString);
                double EastValue = double.Parse(EastString);
                double WestValue = double.Parse(WestString);
                if(NorthValue.Equals(90.0) && SouthValue.Equals(-90.0) && EastValue.Equals(180.0) && WestValue.Equals(-180.0))
                    searchingContent.where_isAnyWhere = true;
                else
                {
                    searchingContent.where_isAnyWhere = false;
                    ComboBoxItem cbi = (ComboBoxItem)ComboBox_WhereType.SelectedItem;
                    string item = cbi.Content as string;

                    if (item.Equals("encloses"))
                        searchingContent.where_Relationship = "BBOX";
                    else if (item.Equals("intersects"))
                        searchingContent.where_Relationship = "Intersects";
                    else if (item.Equals("is"))
                        searchingContent.where_Relationship = "Equals";
                    else if (item.Equals("is fully outside of"))
                        searchingContent.where_Relationship = "Disjoint";
                    else if (item.Equals("overlaps"))
                        searchingContent.where_Relationship = "Overlaps";

                    searchingContent.where_North = text_NorthBound.Text;
                    searchingContent.where_West = text_WestBound.Text;
                    searchingContent.where_South = text_SouthBound.Text;
                    searchingContent.where_East = text_EastBound.Text;
                }


                if (RadioButton_Time_Anytime.IsChecked == true)
                {
                    searchingContent.when_isAnytime = true;
                }
                else
                {
                    searchingContent.when_isAnytime = false;
                    if (RadioButton_Time_MetadataChangeDate.IsChecked == true && datePicker_Time_MetadataChangeDate_From.SelectedDate != null && datePicker_Time_MetadataChangeDate_To.SelectedDate != null)
                    {
                        searchingContent.when_TimeType = "MetadataChangeDate";
                        DateTime dt_from = (DateTime)datePicker_Time_MetadataChangeDate_From.SelectedDate;
                        DateTime dt_to = (DateTime)datePicker_Time_MetadataChangeDate_To.SelectedDate;
                        if (dt_from != null)
                        {
                            string from_time = dt_from.ToString("yyyy-MM-dd");
                            searchingContent.when_FromTime = from_time + "T00:00:00";
                        }
                        if (dt_to != null)
                        {
                            string to_time = dt_to.ToString("yyyy-MM-dd");
                            searchingContent.when_ToTime = to_time + "T23:59:59";
                        }
                    }
                    else if (RadioButton_Time_TemporalExtent.IsChecked == true && datePicker_Time_TemporalExtent_From.SelectedDate != null && datePicker_Time_TemporalExtent_To.SelectedDate != null)
                    {
                        searchingContent.when_TimeType = "TempExtent";
                        DateTime dt_from = (DateTime)datePicker_Time_TemporalExtent_From.SelectedDate;
                        DateTime dt_to = (DateTime)datePicker_Time_TemporalExtent_To.SelectedDate;
                        if (dt_from != null)
                        {
                            string from_time = dt_from.ToString("yyyy-MM-dd");
                            searchingContent.when_FromTime = from_time + "T00:00:00";
                        }
                        if (dt_to != null)
                        {
                            string to_time = dt_to.ToString("yyyy-MM-dd");
                            searchingContent.when_ToTime = to_time + "T00:00:00";
                        }
                    }
                }
            }
            catch(Exception e)
            {
                e.GetType().ToString();
                searchingContent.where_isAnyWhere = true;
            }

            if (RadioButton_OnlyDataCore.IsChecked == true)
                searchingContent.resourceIdentificationKeywords_GEOSSDataCORE = ConstantCollection.resourceIdentificationKeywords_GEOSSDataCORE_ONLY;
            else if (RadioButton_NonDataCore.IsChecked == true)
                searchingContent.resourceIdentificationKeywords_GEOSSDataCORE = ConstantCollection.resourceIdentificationKeywords_GEOSSDataCORE_NON;
            else if (RadioButton_BothDataCoreAndNonDataCore.IsChecked == true)
                searchingContent.resourceIdentificationKeywords_GEOSSDataCORE = ConstantCollection.resourceIdentificationKeywords_GEOSSDataCORE_BOTH;

            return searchingContent;
        }

        private void setSearchConfig()
        {
            if (TextBox_MaximumRetrievalNumber.Text != null && (!TextBox_MaximumRetrievalNumber.Text.Equals("")))
            {          
                int value = int.Parse(TextBox_MaximumRetrievalNumber.Text);
                if (value <= RetrieveNumber.maximum_totalNumber && value >= RetrieveNumber.minimum_totalNumber)
                {
                    ConstantCollection.maxRecords = value+"";
                }
            }
            if (TextBox_FirstRetrievalNumber.Text != null && (!TextBox_FirstRetrievalNumber.Text.Equals("")))
            {
                int value = int.Parse(TextBox_FirstRetrievalNumber.Text);
                if (value <= RetrieveNumber.maximum_firstNumber && value >= RetrieveNumber.minimum_firstNumber)
                {
                    ConstantCollection.firstTimeSearchNum = value + "";
                }
            }
            if (TextBox_RegularRetrievalNumber.Text != null && (!TextBox_RegularRetrievalNumber.Text.Equals("")))
            {
                int value = int.Parse(TextBox_RegularRetrievalNumber.Text);
                if (value <= RetrieveNumber.maximum_regularNumber && value >= RetrieveNumber.minimum_regularNumber)
                {
                    ConstantCollection.eachInvokeSearchNum_ExceptFirstTime = value;
                }
            }

            ConstantCollection.isQueryServicePerformanceScore = (bool)CheckBox_QueryQoS.IsChecked;
            if (ConstantCollection.isQueryServicePerformanceScore)
                ConstantCollection.queryServicePerformanceScoreAtServerSide = (bool)CheckBox_QueryQoS_AtServerSide.IsChecked;

            ConstantCollection.isCalculateRelevance = (bool)CheckBox_CalculateRelevance.IsChecked;
            if (ConstantCollection.isCalculateRelevance)
                ConstantCollection.calculateRelevanceAtServerSide = (bool)CheckBox_CalculateRelevance_AtServerSide.IsChecked;
        }

        private void HyperlinkButton_BackToBasicSearch(object sender, RoutedEventArgs e)
        {
            App app = (App)Application.Current;
            MainPage mp = new MainPage();
            mp.SearchContentTextBox.Text = getWhatToSearchString_NotAccurate();
            App.Navigate(mp);
            //HtmlPage.Window.Invoke("HideBingMap");
        }

        public string getWhatToSearchString_NotAccurate()
        {
            List<string> searchContentWordsList = new List<string>();
            if (RecordsSearchFunctions.isContentSearchable(AllOfTheWords_Content.Text) == true)
                splitStringAndPutIntoList(' ', AllOfTheWords_Content.Text, searchContentWordsList);
            if (RecordsSearchFunctions.isContentSearchable(ExactPhrase_Content.Text) == true)
                searchContentWordsList.Add(ExactPhrase_Content.Text);
            if (RecordsSearchFunctions.isContentSearchable(WordsInTitle_Content.Text) == true)
                splitStringAndPutIntoList(' ', WordsInTitle_Content.Text, searchContentWordsList);
            if (RecordsSearchFunctions.isContentSearchable(WordsInAbstract_Content.Text) == true)
                splitStringAndPutIntoList(' ', WordsInAbstract_Content.Text, searchContentWordsList);
            if (RecordsSearchFunctions.isContentSearchable(Keywords_Content.Text) == true)
                splitStringAndPutIntoList(' ', Keywords_Content.Text, searchContentWordsList);
            if (RecordsSearchFunctions.isContentSearchable(WithoutWords_Content.Text) == true)
                splitStringAndRemoveSameOneFromList(' ', WithoutWords_Content.Text, searchContentWordsList);

            string content = "";
            foreach (string word in searchContentWordsList)
            {
                content += word + " ";
            }
            content = content.TrimEnd().TrimStart();
            return content;
        }

        private void splitStringAndPutIntoList(char identify, string stringToSplit, List<string> whereToPut)
        {
            string[] words = stringToSplit.Split(identify);
            for (int i = 0; i < words.Length; i++)
            {
                if ((!words[i].Trim().Equals("")) && (!whereToPut.Contains(words[i].Trim())))
                    whereToPut.Add(words[i].Trim());
            }
        }

        private void splitStringAndRemoveSameOneFromList(char identify, string stringToSplit, List<string> listOfWords)
        {
            string[] words = stringToSplit.Split(identify);
            for (int i = 0; i < words.Length; i++)
            {
                if ((!words[i].Trim().Equals("")) && (listOfWords.Contains(words[i].Trim())))
                    listOfWords.Remove(words[i].Trim());
            }
        }

        private void CheckBox_ResourceTypes_All_Click(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBox_AllResourceTypes = (CheckBox)sender;
            if (CheckBox_AllResourceTypes.IsChecked == true)
            {
                foreach (TreeViewItem tvi in TreeView_ResourceType.GetContainers())
                {
                    ResourceTypeTree current = tvi.Header as ResourceTypeTree;
                    if (current.isSelected != true)
                        current.isSelected = true;
                    if (current.Children != null && current.Children.Count > 0)
                    {
                        selecteAllChildItem(current);
                    }
                }
            }
            else
            {
                foreach (TreeViewItem tvi in TreeView_ResourceType.GetContainers())
                {
                    ResourceTypeTree current = tvi.Header as ResourceTypeTree;
                    if (current.isSelected != false)
                        current.isSelected = false;
                    if (current.Children != null && current.Children.Count > 0)
                    {
                        unSelecteAllChildItem(current);
                    }
                }
            }
        }

        //private void OneOf_CheckBox_ResourceTypes_Click(object sender, RoutedEventArgs e)
        //{
        //    CheckBox OneOf_CheckBoxs_ResourceTypes = (CheckBox)sender;
        //    if (OneOf_CheckBoxs_ResourceTypes.IsChecked == false)
        //        CheckBox_ResourceTypes_All.IsChecked = false;
        //    else
        //    { 
        //        if(areAllResourceTypesSelected())
        //            CheckBox_ResourceTypes_All.IsChecked = true;
        //    }
        //}

        //private bool areAllResourceTypesSelected()
        //{
        //    //if (CheckBox_ResourceTypes_Application.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Dataset.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Service.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Document.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Video.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Map.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_Model.IsChecked == false)
        //    //    return false;
        //    //if (CheckBox_ResourceTypes_CollectionSession.IsChecked == false)
        //    //    return false;

        //    return true;
        //}

        private void datePickers_Time_GotFocus(object sender, RoutedEventArgs e)
        {
            DatePicker dp = sender as DatePicker;
            if (dp == datePicker_Time_MetadataChangeDate_From || dp == datePicker_Time_MetadataChangeDate_To)
                RadioButton_Time_MetadataChangeDate.IsChecked = true;
            else if (dp == datePicker_Time_TemporalExtent_From || dp == datePicker_Time_TemporalExtent_To)
                RadioButton_Time_TemporalExtent.IsChecked = true;
        }

        private void BBox_textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            double north = 90.0000;
            double south = -90.0000;
            double west = -180.0000;
            double east = 180.0000;
            bool isTextBoxInpurError_North = false;
            bool isTextBoxInpurError_South = false;
            bool isTextBoxInpurError_West = false;
            bool isTextBoxInpurError_East = false;

            try
            {
                north = double.Parse(text_NorthBound.Text);
            }
            catch (Exception e1)
            {
                e1.GetType();
                isTextBoxInpurError_North = true;
            }
            try
            {
                south = double.Parse(text_SouthBound.Text);
            }
            catch (Exception e1)
            {
                e1.GetType();
                isTextBoxInpurError_South = true;
            }
            try
            {
                west = double.Parse(text_WestBound.Text);
            }
            catch (Exception e1)
            {
                e1.GetType();
                isTextBoxInpurError_West = true;
            }
            try
            {
                east = double.Parse(text_EastBound.Text);
            }
            catch (Exception e1)
            {
                e1.GetType();
                isTextBoxInpurError_East = true;
            }
            if (north > 90.0 || isTextBoxInpurError_North)
            {
                text_NorthBound.Text = "90.0000";
                north = 90.0000;
            }
            if (south < -90.0 || isTextBoxInpurError_South)
            {
                text_SouthBound.Text = "-90.0000";
                south = -90.0000;
            }
            if (west < -180.0 || isTextBoxInpurError_West)
            {
                text_WestBound.Text = "-180.0000";
                west = -180.0000;
            }
            if (east > 180.0 || isTextBoxInpurError_East)
            {
                text_EastBound.Text = "180.0000";
                east = 180.0000;
            }
    
            Thickness margin = new Thickness();
            {
                double right = (180.0 - east) / 360.0 * 500.0;
                margin.Right = right;

                double left = (180.0 + west) / 360.0 * 500.0;
                margin.Left = left;

                double top = (90.0 - north) / 180.0 * 250.0;
                margin.Top = top;

                double bottom = (90.0 + south) / 180.0 * 250.0;
                margin.Bottom = bottom;
            }

            double height = 250;
            {
                height = (north - south) / 180 * 250.0;
            }

            double width = 250;
            {
                width = (east - west) / 360.0 * 500.0;
            }

            border_ExtentBBox.Margin = margin;
            border_ExtentBBox.Height = height;
            border_ExtentBBox.Width = width;

            border_ExtentBBox1.Margin = margin;
            border_ExtentBBox1.Height = height;
            border_ExtentBBox1.Width = width;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox button = sender as CheckBox;
            button.IsChecked = true;
            ResourceTypeTree resourceType = button.DataContext as ResourceTypeTree;
            if (resourceType != null)
            {
                if (resourceType.isSelected != true)
                    resourceType.isSelected = true;

                if (TreeView_ResourceType.GetSelectedContainer() != null)
                {
                    TreeViewItem tvi = TreeView_ResourceType.GetSelectedContainer();
                    ResourceTypeTree current = tvi.Header as ResourceTypeTree;
                    if (current == resourceType)
                    {
                        if (resourceType.Children != null && resourceType.Children.Count > 0)
                        {
                            selecteAllChildItem(resourceType);
                        }
                    }
                }
                //if all child treeViemItem of Parent treeviewItem of current treeviewItem are select, the parent should be selected 
                if (resourceType.Parent != null)
                {
                    if (resourceType.Parent.ResourceTypeID != ConstantCollection.resourceType_PickID_All)
                        resourceType.Parent.isSelected = true;

                    bool allSelect = true;
                    foreach (ResourceTypeTree type1 in resourceType.Parent.Children)
                    {
                        if (type1.isSelected != true)
                            allSelect = false;
                    }

                    if (allSelect == true)
                    {
                        resourceType.Parent.isSelected = true;
                    }
                }

                bool allSelect1 = true;
                foreach (ResourceTypeTree rt in allResourceTypes)
                {
                    if (rt.isSelected == false)
                        allSelect1 = false;
                }
                if(allSelect1)
                    CheckBox_ResourceTypes_All.IsChecked = true;
            }
            ObservableCollection<ResourceTypeTree> a = TreeView_ResourceType.ItemsSource as ObservableCollection<ResourceTypeTree>;
        }

        void selecteAllChildItem(ResourceTypeTree resourceType)
        {
            if (resourceType.Children != null && resourceType.Children.Count > 0)
            {
                foreach (ResourceTypeTree type1 in resourceType.Children)
                {
                    if (type1.isSelected != true)
                        type1.isSelected = true;

                    if (type1.Children != null && type1.Children.Count > 0)
                    {
                        selecteAllChildItem(type1);
                    }
                }
            }
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox button = sender as CheckBox;
            button.IsChecked = false;
            ResourceTypeTree resourceType = button.DataContext as ResourceTypeTree;
            if (resourceType != null)
            {
                if (resourceType.isSelected != false)
                    resourceType.isSelected = false;

                //just unselect all the child treeviewItem of current selected treeviewItem, and the root treeViewItem "All"
                if (TreeView_ResourceType.GetSelectedContainer()!= null)
                {
                    CheckBox_ResourceTypes_All.IsChecked = false;
                    foreach (ResourceTypeTree r in allResourceTypes)
                    {
                        if (r.ResourceTypeID.Equals(ConstantCollection.resourceType_PickID_All))
                            r.isSelected = false;
                    }

                    TreeViewItem tvi = TreeView_ResourceType.GetSelectedContainer();
                    ResourceTypeTree current = tvi.Header as ResourceTypeTree;
                    if (current == resourceType)
                    {
                        if (resourceType.Children != null && resourceType.Children.Count > 0)
                        {
                            unSelecteAllChildItem(resourceType);
                        }
                    }
                }

                //if (resourceType.ParentResourceType != null)
                //{
                //    resourceType.ParentResourceType.isSelected = false;
                //    unSelecteAllAncestorItem(resourceType.ParentResourceType);
                //}

                //if all child treeViemItem of Parent treeviewItem of current treeviewItem are unselect, the parent should be unselected 
                if (resourceType.Parent != null)
                {
                    bool allUnSelect = true;
                    foreach (ResourceTypeTree type1 in resourceType.Parent.Children)
                    {
                        if (type1.isSelected != false)
                            allUnSelect = false;
                    }

                    if (allUnSelect == true)
                    {
                        resourceType.Parent.isSelected = false;
                    }
                }
            }
            ObservableCollection<ResourceTypeTree> a = TreeView_ResourceType.ItemsSource as ObservableCollection<ResourceTypeTree>;
        }

        void unSelecteAllChildItem(ResourceTypeTree resourceType)
        {
            if (resourceType.Children != null && resourceType.Children.Count > 0)
            {
                foreach (ResourceTypeTree type1 in resourceType.Children)
                {
                    if (type1.isSelected != false)
                        type1.isSelected = false;

                    if (type1.Children != null && type1.Children.Count > 0)
                    {
                        unSelecteAllChildItem(type1);
                    }
                }
            }
        }

        void unSelecteAllAncestorItem(ResourceTypeTree resourceType)
        {
            if (resourceType.Parent != null)
            {
                resourceType.Parent.isSelected = false;
                unSelecteAllAncestorItem(resourceType.Parent);
            }
        }

        private void expander_ResourceType_Expanded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(() => { TreeView_ResourceType.ExpandToDepth(1); });
        }

        private void CheckBox_ClassifiedWithResourceTypes_Click(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBox_ClassifiedWithResourceTypes = (CheckBox)sender;
            if (CheckBox_ClassifiedWithResourceTypes.IsChecked == true)
                classifiedWithCategories = true;
            else
                classifiedWithCategories = false;
        }

        private void CheckBox_QueryQoS_Click(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBox_QueryQoS = (CheckBox)sender;
            if (CheckBox_QueryQoS.IsChecked == true)
            {
                CheckBox_QueryQoS_AtServerSide.IsEnabled = true;
                CheckBox_QueryQoS_AtClientSide.IsEnabled = true;
            }
            else
            {
                CheckBox_QueryQoS_AtServerSide.IsEnabled = false;
                CheckBox_QueryQoS_AtClientSide.IsEnabled = false;
            }
        }

        private void CheckBox_CalculateRelevance_Click(object sender, RoutedEventArgs e)
        {
            CheckBox CheckBox_CalculateRelevance = (CheckBox)sender;
            if (CheckBox_CalculateRelevance.IsChecked == true)
            {
                CheckBox_CalculateRelevance_AtServerSide.IsEnabled = true;
                CheckBox_CalculateRelevance_AtClientSide.IsEnabled = false;
            }
            else
            {
                CheckBox_CalculateRelevance_AtServerSide.IsEnabled = false;
                CheckBox_CalculateRelevance_AtClientSide.IsEnabled = false;
            }
        }

        //private void isNoResourceTypeSelected(ResourceTypeTree tree, Boolean isNothingSelected)
        //{
        //    if (tree.isSelected)
        //    {
        //        isNothingSelected = false;
        //        return;
        //    }
        //    else if(tree.Children != null)
        //    {
        //        foreach(ResourceTypeTree tree1 in tree.Children)
        //        {
        //            isNoResourceTypeSelected(tree1, isNothingSelected);
        //        }
        //    }
        //}
    }
}
