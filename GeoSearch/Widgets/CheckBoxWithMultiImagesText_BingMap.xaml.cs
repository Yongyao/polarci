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
using ImageTools.IO;
using ImageTools.IO.Gif;
using System.Windows.Media.Imaging;
using ImageTools;

namespace GeoSearch.Widgets
{
    public partial class CheckBoxWithMultiImagesText_BingMap : UserControl
    {
        public ExtendedImage addGIF = null;
        public CheckBoxWithMultiImagesText_BingMap()
        {
            InitializeComponent();
            //为支持GIF图片增加的解码器
            Decoders.AddDecoder<GifDecoder>(); 
        }

        public void setLegendImage(string URL)
        {
            if (!URL.ToLower().Contains("gif"))
            {
                Image_Legend_Content.Source = new BitmapImage(new Uri(URL, UriKind.Absolute));
                Image_Legend_Content.Visibility = Visibility.Visible;
                Image_Legend_GIF_Content.Visibility = Visibility.Collapsed;
            }
            else
            {
                addGIF = new ExtendedImage();
                addGIF.UriSource = new Uri(URL, UriKind.Absolute);
                addGIF.LoadingFailed += GIF_LoadingFailed;
                addGIF.LoadingCompleted += On_Load;
            }
        }

        private void GIF_LoadingFailed(object sender, UnhandledExceptionEventArgs e)
        {
            // Your image could not be loaded.
        }

        private void On_Load(object sender, EventArgs e)
        {
            Image_Legend_GIF_Content.Dispatcher.BeginInvoke(() =>
            {
                Image_Legend_GIF_Content.Source = addGIF;
            });
            Image_Legend_Content.Dispatcher.BeginInvoke(() =>
            {
                Image_Legend_Content.Visibility = Visibility.Collapsed;
            });
            Image_Legend_GIF_Content.Dispatcher.BeginInvoke(() =>
            {
                Image_Legend_GIF_Content.Visibility = Visibility.Visible;
            });       
        }
    }
}
