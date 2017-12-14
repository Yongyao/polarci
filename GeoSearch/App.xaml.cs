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

namespace GeoSearch
{
    public partial class App : Application
    {
        public static UserControl root;
        //public static Grid root;
        public static Pages.HelpPage helpPage = new Pages.HelpPage();
        
        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }



        private void Application_Startup(object sender, StartupEventArgs e)
        {
            root = new UserControl();
            //root = new Grid();
            root.Content = new MainPage();
            //root.Children.Add(new MainPage());
            this.RootVisual = root;

 
            //root = new Grid();
            //root.Children.Add(new MainPage());
            //this.RootVisual = root;
            //this.RootVisual = new MainPage();
        }

        //added navigation function, for change web page to visualization
        public static void Navigate(UIElement newPage)
        {
            root.Content = newPage;
            //root.Children.RemoveAt(0);
            //root.Children.Add(newPage);
            if (!(newPage is SearchingResultPage))
            {
                if (SearchingResultPage.SBAPopup !=null )
                    SearchingResultPage.SBAPopup.SBAQuickSearchPage.IsOpen = false;
            }
        }

        public static UserControl getCurrentPage()
        {
            //return root.Children.ElementAt(0);
            return root;
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // 如果应用程序是在调试器外运行的，则使用浏览器的
            // 异常机制报告该异常。在 IE 上，将在状态栏中用一个 
            // 黄色警报图标来显示该异常，而 Firefox 则会显示一个脚本错误。
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下
                // 继续运行。 
                // 对于生产应用程序，此错误处理应替换为向网站报告错误
                // 并停止应用程序。
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

        //获取当前的SearchingResultPage,如果当前页面是SearchingResultPage，否则返回为null
        public static SearchingResultPage getCurrentSearchingResultPage()
        {
            SearchingResultPage srp = null;
            UserControl root = App.getCurrentPage();
            if (root.Content is SearchingResultPage)
            {
                srp = root.Content as SearchingResultPage;
            }
            return srp;
        }

        //获取当前页面中的UIElement内容
        public static UIElement getUIElementContentInCurrentPage()
        {
            UIElement elment = App.getCurrentPage().Content;
            return elment;
        }

        //从全屏的BingMaps对应的Grid中获得与之对应的SearchingResultPage
        public static SearchingResultPage getSearchingResultPageFromBingMapsGrid()
        {
            SearchingResultPage srp1 = null;
            UIElement content = App.getCurrentPage().Content;
            if (content != null && content is Grid)
            {
                Grid grid = content as Grid;
                if (grid.Name.Equals("Map_Container"))
                {
                    Button button = grid.FindName("button_BingMapsFullScreen") as Button;
                    if (button != null)
                    {
                        srp1 = button.Tag as SearchingResultPage;
                    }
                }
            }
            return srp1;
        }
    }
}
