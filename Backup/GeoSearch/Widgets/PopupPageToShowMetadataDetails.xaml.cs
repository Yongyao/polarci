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
using System.Windows.Controls.Primitives;

namespace GeoSearch
{
    public partial class PopupPage_ShowMetadataDetails : UserControl
    {
        bool IsLeftBuutonDown_Flag = false;

        Point originalPoint = new Point();
        
        public PopupPage_ShowMetadataDetails()
        {
            InitializeComponent();
            BindControl();
        }

        public void BindControl()
        {

            this.popContentGrid.MouseLeftButtonDown += new MouseButtonEventHandler(pop_LeftMouseDown);
            this.popContentGrid.MouseMove += new MouseEventHandler(pop_mouseMove);
            this.popContentGrid.MouseLeftButtonUp += new MouseButtonEventHandler(pop_leftMouseButtonUp);
            //this.MouseLeftButtonDown += new MouseButtonEventHandler(pop_LeftMouseDown);
            //this.border1.MouseLeftButtonDown += new MouseButtonEventHandler(pop_LeftMouseDown);
            //this.PopupPageTitle.MouseLeftButtonDown += new MouseButtonEventHandler(pop_LeftMouseDown);

        }
        public void CancelBindControl(UIElement uc)
        {

            //uc.MouseLeave -= new MouseEventHandler(ti_MouseLeave);
            //uc.MouseMove -= new MouseEventHandler(ti_MouseMove);

        }

        private void pop_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.IsLeftBuutonDown_Flag = true;
            originalPoint = e.GetPosition(null);
        }

        private void pop_mouseMove(object sender, MouseEventArgs e)
        {
            if (IsLeftBuutonDown_Flag)
            {
                this.MetadataDetailPage.HorizontalOffset += (e.GetPosition(null).X - originalPoint.X);
                this.MetadataDetailPage.VerticalOffset += (e.GetPosition(null).Y - originalPoint.Y);
                originalPoint = e.GetPosition(null);
            }
        }

        private void pop_leftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsLeftBuutonDown_Flag = false;
            originalPoint = e.GetPosition(null);
        }

        private void ClosePop_Click(object sender, RoutedEventArgs e)
        {
            this.MetadataDetailPage.IsOpen = false;
        }
    }
}
