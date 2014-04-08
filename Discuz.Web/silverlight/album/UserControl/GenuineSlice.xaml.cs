using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Windows.Browser;
using System.Windows.Interop;

using DiscuzAlbum.Common;

namespace DiscuzAlbum.UserControl
{
    public class GenuineSlice : ControlBase
    {
        #region Properties
        private Canvas _layoutRoot;
        private Rectangle _imageBorder;
        private Image _mainImage;
        private Canvas _contentHolder;
        private Rectangle _shadowCover;
        private Storyboard _fadeInStoryboard;
        private Storyboard _fadeInShadowStory;
        private Storyboard _fadeOutStoryboard;
        private Downloader _downloader;
        private HtmlTimer _imageTimer;
        private TracingIcon _tracingMouse;
        private Canvas _posCanvas;

        #endregion

        #region Constructor
        public GenuineSlice(TracingIcon pTracingIcon, Canvas pPosCanvas)
        {
            _tracingMouse = pTracingIcon;
            _posCanvas = pPosCanvas;
        }

        public void Control_Loaded(object o, EventArgs e)
        {
            SyncProperty();
        }

        private void SyncProperty()
        {
            _layoutRoot = FindName("LayoutRoot") as Canvas;
            _imageBorder = FindName("ImageBorder") as Rectangle;
            _mainImage = FindName("MainImage") as Image;
            _contentHolder = FindName("ContentHolder") as Canvas;
            _shadowCover = FindName("ShadowCover") as Rectangle;
            _fadeInStoryboard = FindName("FadeIn") as Storyboard;
            _fadeInShadowStory = FindName("FadeInShadow") as Storyboard;
            _fadeOutStoryboard = FindName("FadeOut") as Storyboard;
            _fadeOutStoryboard.Completed += new EventHandler(_fadeOutStoryboard_Completed);
            _fadeInShadowStory.Completed += new EventHandler(_fadeInShadowStory_Completed);

            _downloader = new Downloader();
            _downloader.DownloadFailed += new ErrorEventHandler(_downloader_DownloadFailed);
            _downloader.Completed += new EventHandler(_downloader_Completed);

            _imageTimer = new HtmlTimer();
            _imageTimer.Interval = 20;
            _imageTimer.Tick += new EventHandler(_imageTimer_Tick);

            _mainImage.MouseLeftButtonUp += new MouseEventHandler(_mainImage_MouseLeftButtonUp);
            _mainImage.Cursor = Cursors.Hand;

            // hide this instance.
            _imageBorder.Fill = null;
            _imageBorder.Stroke = null;

            // make event
            _mainImage.MouseEnter += new MouseEventHandler(_mainImage_MouseEnter);
            _mainImage.MouseLeave += new EventHandler(_mainImage_MouseLeave);
        }

        void _mainImage_MouseLeave(object sender, EventArgs e)
        {
            //_tracingMouse.Status = IconStatus.None;
            _tracingMouse.FadeOut();
        }

        void _mainImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(_posCanvas);
            _tracingMouse.Status = IconStatus.Minus;
            _tracingMouse.FadeIn();
            //_tracingMouse.SetValue<double>(Canvas.LeftProperty, pos.X);
            //_tracingMouse.SetValue<double>(Canvas.TopProperty, pos.Y);
        }

        void _fadeInShadowStory_Completed(object sender, EventArgs e)
        {
            double scaleX = (BrowserHost.ActualWidth - 2 * CommonValues.GenuinePadding) / _mainImage.Width;
            double scaleY = (BrowserHost.ActualHeight - 2 * CommonValues.GenuinePadding) / _mainImage.Height;
            double scale = Math.Min(scaleX, scaleY);
            scale = scale > 1 ? 1 : scale;
            ImageScale = new Point(scale, scale);

            _imageBorder.Width = _mainImage.Width * scale + 20;
            _imageBorder.Height = _mainImage.Height * scale + 20;
            _imageBorder.Fill = new SolidColorBrush(Color.FromRgb(204, 204, 204));
            //_imageBorder.Stroke = new SolidColorBrush(Colors.Black);

            _contentHolder.SetValue<double>(Canvas.LeftProperty, -1 * _imageBorder.Width / 2);
            _contentHolder.SetValue<double>(Canvas.TopProperty, -1 * _imageBorder.Height / 2);

            _shadowCover.Width = BrowserHost.ActualWidth * 2;
            _shadowCover.Height = BrowserHost.ActualHeight * 2;
            _shadowCover.SetValue<double>(Canvas.LeftProperty, -1 * _shadowCover.Width / 2);
            _shadowCover.SetValue<double>(Canvas.TopProperty, -1 * _shadowCover.Height / 2);

            _fadeInStoryboard.Begin();
        }

        void _mainImage_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _tracingMouse.Status = IconStatus.None;
            FadeOut();
        }

        void _fadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            _mainImage.SetValue<Uri>(Image.SourceProperty, null);
        }

        void _imageTimer_Tick(object sender, EventArgs e)
        {
            if (_mainImage.Width != 0 && _mainImage.Height != 0)
            {
                _imageTimer.Stop();

                double scaleX = (BrowserHost.ActualWidth - 2 * CommonValues.GenuinePadding) / _mainImage.Width;
                double scaleY = (BrowserHost.ActualHeight - 2 * CommonValues.GenuinePadding) / _mainImage.Height;
                double scale = Math.Min(scaleX, scaleY);
                scale = scale > 1 ? 1 : scale;
                ImageScale = new Point(scale, scale);

                _imageBorder.Width = _mainImage.Width * scale + 20;
                _imageBorder.Height = _mainImage.Height * scale + 20;
                _imageBorder.Fill = new SolidColorBrush(Color.FromRgb(204, 204, 204));
                //_imageBorder.Stroke = new SolidColorBrush(Colors.Black);

                _contentHolder.SetValue<double>(Canvas.LeftProperty, -1 * _imageBorder.Width / 2);
                _contentHolder.SetValue<double>(Canvas.TopProperty, -1 * _imageBorder.Height / 2);

                _shadowCover.Width = BrowserHost.ActualWidth * 2;
                _shadowCover.Height = BrowserHost.ActualHeight * 2;
                _shadowCover.SetValue<double>(Canvas.LeftProperty, -1 * _shadowCover.Width / 2);
                _shadowCover.SetValue<double>(Canvas.TopProperty, -1 * _shadowCover.Height / 2);

                // fade in
                FadeIn();
            }
        }

        void _downloader_Completed(object sender, EventArgs e)
        {
            _mainImage.SetValue<Uri>(Image.SourceProperty, null);
            _mainImage.SetSource(_downloader, null);
            _imageTimer.Start();
        }

        #endregion

        #region Methods
        private Point ImageScale
        {
            set 
            {
                ScaleTransform imageScale = FindName("ImageScale") as ScaleTransform;
                imageScale.ScaleX = value.X;
                imageScale.ScaleY = value.Y;
            }
        }

        public void ShowImage(string pImage)
        {
            //_downloader.DownloadFailed += new ErrorEventHandler(_downloader_DownloadFailed);
            _downloader.Open("GET", new Uri(pImage, UriKind.RelativeOrAbsolute));
            _downloader.Send();
        }

        void _downloader_DownloadFailed(object sender, ErrorEventArgs e)
        {
            
            //throw new NotImplementedException();
        }

        private void FadeIn()
        {
            _tracingMouse.Status = IconStatus.Minus;
            this.Visibility = Visibility.Visible;
            _fadeInShadowStory.Begin();
        }

        private void FadeOut()
        {
            _tracingMouse.Status = IconStatus.Plus;
            _fadeOutStoryboard.Begin();
        }

        protected override string ResourceName
        {
            get { return "GenuineSlice.xaml"; }
        }

        #endregion
    }
}
