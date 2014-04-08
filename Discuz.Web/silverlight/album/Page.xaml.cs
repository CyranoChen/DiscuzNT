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
using System.Xml;
using System.Net;
using System.Windows.Browser;
using System.Windows.Browser.Net;
using System.IO;
using System.Text;
using System.Windows.Interop;

using DiscuzAlbum.UserControl;
using System.Windows.Browser.Serialization;


namespace DiscuzAlbum
{
    public partial class Page : Canvas
    {
        #region Properties
        private LayoutManager _layoutManager;
        private PhotoDataSet _photoDataSet;

        private SortNavigator _sortNavi;

        private Point _originPosition;
        private Point _originSize;

        private GenuineSlice _genuineSlice;

        private Image _sortArrowLeft;
        private Image _sortArrowRight;
        private Rectangle _sortArrowLeftHot;
        private Rectangle _sortArrowRightHot;

        private Image _screenArrowLeft;
        private Image _screenArrowRight;

        private TracingIcon _tracingIcon;

        #endregion

        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();

            // Make GrassSphere dock to the bottom.
            DockGrassSphere();

            // Make tracing icon
            _tracingIcon = new TracingIcon();
            
            
            // Make sort Arrow
            _sortArrowLeft = this.FindName("LeftSortArrow") as Image;
            _sortArrowRight = this.FindName("RightSortArrow") as Image;
            _sortArrowLeftHot = this.FindName("LeftSortArrowHot") as Rectangle;
            _sortArrowRightHot = this.FindName("RightSortArrowHot") as Rectangle;
            _screenArrowLeft = this.FindName("ScreenArrowLeft") as Image;
            _screenArrowRight = this.FindName("ScreenArrowRight") as Image;
            _sortArrowLeftHot.Cursor = Cursors.Hand;
            _sortArrowRightHot.Cursor = Cursors.Hand;
            _sortArrowLeftHot.MouseEnter += new MouseEventHandler(_sortArrowLeft_MouseEnter);
            _sortArrowLeftHot.MouseLeave += new EventHandler(_sortArrowLeft_MouseLeave);
            _sortArrowLeftHot.MouseLeftButtonUp += new MouseEventHandler(_sortArrowLeft_MouseLeftButtonUp);
            _sortArrowRightHot.MouseEnter += new MouseEventHandler(_sortArrowRight_MouseEnter);
            _sortArrowRightHot.MouseLeave += new EventHandler(_sortArrowRight_MouseLeave);
            _sortArrowRightHot.MouseLeftButtonUp += new MouseEventHandler(_sortArrowRight_MouseLeftButtonUp);

            // Make Sort Navi
            _sortNavi = new SortNavigator(this.FindName("SortNaviCanvas") as Canvas);

            // Add genuine slice
            _genuineSlice = new GenuineSlice(_tracingIcon, BgCanvas);
            this.Children.Add(_genuineSlice);
            this.Children.Add(_tracingIcon);
            CenterGenuineSlice();

            // Sync the instance of PhotoDataSet.
            //-------------- NOTICE ------------------------------
            // Current set is just load a fixed dummy data set.
            // Change this part to sync PhotoDataSet if necessary.
            //-------------- END OF NOTICE -----------------------

            //InitailizeAlbum();
            GetJSONData();
        }

        private void InitailizeAlbum()
        {
            // Update origin position & size
            _originPosition = new Point((double)PhotoHolder.GetValue(Canvas.LeftProperty), (double)PhotoHolder.GetValue(Canvas.TopProperty));
            _originSize = new Point(PhotoHolder.Width, PhotoHolder.Height);

            // Initialize layout manager.
            _layoutManager = new LayoutManager(PhotoHolder, _photoDataSet, BtnSortCanvas, BtnRandomCanvas, BgCanvas, _sortNavi, _genuineSlice, _tracingIcon, _screenArrowLeft, _screenArrowRight);

            // Add host resize event listener.
            BrowserHost.Resize += new EventHandler(BrowserHost_Resize);
        }

        void _sortArrowRight_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_layoutManager.Status == AlbumStatus.Sorted)
                _layoutManager.LeftScroll();
            else
                _layoutManager.RightScreen();
        }

        void _sortArrowLeft_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_layoutManager.Status == AlbumStatus.Sorted)
                _layoutManager.RightScroll();
            else
                _layoutManager.LeftScreen();
        }

        void _sortArrowRight_MouseLeave(object sender, EventArgs e)
        {
            (this.FindName("RightSortArrow") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/sortArrow_normal.png", UriKind.Relative));
            (this.FindName("SortRightArrowOut") as Storyboard).Begin();
        }

        void _sortArrowRight_MouseEnter(object sender, MouseEventArgs e)
        {
            (this.FindName("RightSortArrow") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/sortArrow_hover.png", UriKind.Relative));
            (this.FindName("SortRightArrowHover") as Storyboard).Begin();
        }

        void _sortArrowLeft_MouseLeave(object sender, EventArgs e)
        {
            (this.FindName("LeftSortArrow") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/sortArrow_normal.png", UriKind.Relative));
            (this.FindName("SortLeftArrowOut") as Storyboard).Begin();
        }

        void _sortArrowLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            (this.FindName("LeftSortArrow") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/sortArrow_hover.png", UriKind.Relative));
            (this.FindName("SortLeftArrowHover") as Storyboard).Begin();
        }

        void BrowserHost_Resize(object sender, EventArgs e)
        {
            SyncToHost();
        }

        #region Methods
        /// <summary>
        /// Sync to host, when host's get resized.
        /// </summary>
        private void SyncToHost()
        { 
            // fix background
            BgCanvas.Width = BrowserHost.ActualWidth;
            BgCanvas.Height = BrowserHost.ActualHeight;

            // fix arrow canvas
            Canvas arrowCanvas = this.FindName("ArrowCanvas") as Canvas;
            arrowCanvas.SetValue<double>(Canvas.LeftProperty, (BrowserHost.ActualWidth - arrowCanvas.Width) / 2);
            arrowCanvas.SetValue<double>(Canvas.TopProperty, (BrowserHost.ActualHeight - arrowCanvas.Height) / 2);

            // fix screen arrows.
            _screenArrowLeft.SetValue<double>(Canvas.LeftProperty, 0);
            _screenArrowLeft.SetValue<double>(Canvas.TopProperty, (BrowserHost.ActualHeight - _screenArrowLeft.Height) / 2);
            _screenArrowRight.SetValue<double>(Canvas.LeftProperty, (BrowserHost.ActualWidth - _screenArrowRight.Width));
            _screenArrowRight.SetValue<double>(Canvas.TopProperty, (BrowserHost.ActualHeight - _screenArrowRight.Height) / 2);

            // Sync grass
            DockGrassSphere();

            // Sync genuine..
            CenterGenuineSlice();

            // Sync tracing mouse
            _tracingIcon.SetValue<double>(Canvas.LeftProperty, (BrowserHost.ActualWidth - _tracingIcon.Width) / 2 - 10);
            _tracingIcon.SetValue<double>(Canvas.TopProperty, (BrowserHost.ActualHeight - _tracingIcon.Height) / 2 - 20);

            // Sync Content
            TranslateTransform holderTranslate = this.FindName("PhotoHolderTranslation") as TranslateTransform;
            if (_layoutManager.Status == AlbumStatus.Sorted)
            {
                holderTranslate.X = Math.Round((BrowserHost.ActualWidth - _originSize.X) / 2);
                holderTranslate.Y = Math.Round((BrowserHost.ActualHeight - _originSize.Y) / 2);
            }
            else if (_layoutManager.Status == AlbumStatus.Random)
            {
                holderTranslate.X = Math.Round((BrowserHost.ActualWidth - PhotoHolder.Width) / 2);
                holderTranslate.Y = Math.Round((BrowserHost.ActualHeight - PhotoHolder.Height) / 2);
            }

            // Change sort navi's width
            _sortNavi.ChangeWidth(BrowserHost.ActualWidth);

            // Status switch's position
            StatusSwitchCanvas.SetValue<double>(Canvas.LeftProperty, (BgCanvas.Width - StatusSwitchCanvas.Width) / 2);
            StatusSwitchCanvas.SetValue<double>(Canvas.TopProperty, BgCanvas.Height - StatusSwitchCanvas.Height);

            // Refresh random layout.
            _layoutManager.RefreshRandom();
        }

        /// <summary>
        /// Make sure genuine slice's centered.
        /// </summary>
        private void CenterGenuineSlice()
        {
            _genuineSlice.SetValue<double>(Canvas.LeftProperty, BrowserHost.ActualWidth / 2);
            _genuineSlice.SetValue<double>(Canvas.TopProperty, BrowserHost.ActualHeight / 2);
        }

        /// <summary>
        /// Dock grass sphere to the bottom of background canvas.
        /// </summary>
        private void DockGrassSphere()
        {
            GrassSphere.SetValue<double>(Canvas.LeftProperty, (BgCanvas.Width - GrassSphere.Width) / 2);
            GrassSphere.SetValue<double>(Canvas.TopProperty, BgCanvas.Height - GrassSphere.Height + 70);
        }

        public void HideLeftArrow()
        {
            //_sortArrowLeft.Visibility = Visibility.Collapsed;
            //_sortArrowLeftHot.Visibility = Visibility.Collapsed;

            _sortArrowLeft.Opacity = 0.3;
            _sortArrowLeftHot.Visibility = Visibility.Collapsed;
        }

        public void HideRightArrow()
        {
            //_sortArrowRight.Visibility = Visibility.Collapsed;
            //_sortArrowRightHot.Visibility = Visibility.Collapsed;

            _sortArrowRight.Opacity = 0.3;
            _sortArrowRightHot.Visibility = Visibility.Collapsed;
        }

        public void ShowBothArrow()
        {
            //_sortArrowLeft.Visibility = Visibility.Visible;
            //_sortArrowLeftHot.Visibility = Visibility.Visible;
            //_sortArrowRight.Visibility = Visibility.Visible;
            //_sortArrowRightHot.Visibility = Visibility.Visible;

            _sortArrowLeft.Opacity = 1;
            _sortArrowLeftHot.Visibility = Visibility.Visible;
            _sortArrowRight.Opacity = 1;
            _sortArrowRightHot.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Sync photo xml data to photo data set.
        /// </summary>
        private void SyncPhotoData()
        {
            //_photoDataSet = new PhotoDataSet();
            //_photoDataSet.FillDummy();
        }


        private void GetJSONData()
        {
            string serverUri = HtmlPage.DocumentUri.ToString();
            int thisApp = serverUri.IndexOf("/silverlight/album");

            serverUri = serverUri.Substring(0, thisApp) + "/services/MixObjects.asmx/AlbumData?albumid="; //"http://localhost/services/MixObjects.asmx/AlbumData?albumid=";
            string value = "";
            HtmlPage.QueryString.TryGetValue("albumid", out value);
            System.Uri webServiceUri = new System.Uri(serverUri + value);
            BrowserHttpWebRequest request = new BrowserHttpWebRequest(webServiceUri);
            IAsyncResult iar = request.BeginGetResponse(new AsyncCallback(OnResponseDownload), request);

        }

        private void OnResponseDownload(IAsyncResult iar)
        {
            try
            {
                HttpWebResponse response =
                    ((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new ApplicationException("HttpStatusCode " + response.StatusCode.ToString() + " was returned.");
                StreamReader responseReader = new StreamReader(response.GetResponseStream());

                string jsonData = responseReader.ReadToEnd();
                responseReader.Close();
                response.Close();
                XmlReader xr = XmlReader.Create(new StringReader(jsonData));
                xr.ReadToFollowing("string");
                xr.Read();

                JavaScriptSerializer jss = new JavaScriptSerializer();
                PhotoClass pc = jss.Deserialize<PhotoClass>(xr.Value);
                
                _photoDataSet = new PhotoDataSet();
                _photoDataSet.FillDummy(pc.items);


                InitailizeAlbum();
                SyncPhotoData();
                SyncToHost();


                
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        #endregion
    }
    
    public class PhotoClass
    {
        public PhotoData[] items;
    }
    
    public class PhotoData
    {
        public int photoid;
        public int userid;
        public string title;
        public string image;
        public string square;
        public string thumbnail;
    }
}
