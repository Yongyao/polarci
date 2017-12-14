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
using System.Windows.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using GeoSearch.CSWQueryServiceReference;

namespace GeoSearch
{
    public partial class SBAQuickSearchPanel : UserControl
    {
        public SBAQuickSearchPanel()
        {
            InitializeComponent();
            ObservableCollection<SBAVocabularyTree> list = SBAVocabularyTree.getSBAVocabularyList();
            TreeView_SBAVocabulary.ItemsSource = list;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = TreeView_SBAVocabulary.GetSelectedContainer();
            SBAVocabularyTree current = tvi.Header as SBAVocabularyTree;
            SBAVocabulary vocabulary = SBAVocabularyTree.createSBAVocabularyFromTreeNode(current);
            
            HyperlinkButton button = sender as HyperlinkButton;
            string id = button.Tag as string;

            RecordsSearchFunctions.cannotStartSearchYet = true;

            RecordsSearchFunctions sf = new RecordsSearchFunctions(); 
            sf.BrowseBySBA_Using_WCFService(vocabulary, ConstantCollection.startPosition, ConstantCollection.maxRecords);
        }
    }
}
