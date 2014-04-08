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

using DiscuzAlbum.Common;

namespace DiscuzAlbum.UserControl
{
    public class SliceLoadedEventArgs : EventArgs
    {
        private int sliceID;

        public SliceLoadedEventArgs(int pId)
        {
            sliceID = pId;
        }
        public int SliceID
        {
            get { return sliceID; }
            set { sliceID = value; }
        }
    }

    public class PhotoSlice : ControlBase
    {
        #region Properties
        private int _id;
        private LayoutManager _manager;
        private PhotoItem _photoItem;

        private Canvas _layoutRoot;
        private Rectangle _imageStroke;
        private Image _mainImage;
        private Image _mainThum;
        private Image _titleImage;
        private Image _authorImage;

        private Storyboard _loading;
        private Canvas _loadingCanvas;


        public event EventHandler<SliceLoadedEventArgs> OnSliceLoaded;

        #endregion
        public PhotoSlice(LayoutManager pManager, int pID, PhotoItem pPhotoItem)
        {
            _id = pID;
            _manager = pManager;
            _photoItem = pPhotoItem;
        }

        public void Control_Loaded(object o, EventArgs e)
        {
            SyncProperty();

            // hand cursor
            this.Cursor = Cursors.Hand;

            // After this initialize thing, this slice should load the image for it self.
            TitleImageUri = _photoItem.Title;
            AuthorImageUri = _photoItem.Author;
            ShowLowRes();         // by default, load thum.

            // broadcast event.
            EventHandler<SliceLoadedEventArgs> temp = OnSliceLoaded;
            if (temp != null)
                temp(this, new SliceLoadedEventArgs(_id));
        }

        #region Methods
        private void SyncProperty()
        {
            _layoutRoot = FindName("LayoutRoot") as Canvas;
            _imageStroke = FindName("ImageStroke") as Rectangle;
            _mainImage = FindName("MainImage") as Image;
            _mainThum = FindName("MainThum") as Image;
            _titleImage = FindName("TitleImage") as Image;
            _authorImage = FindName("AuthorImage") as Image;
            _loadingCanvas = FindName("LoadingCanvas") as Canvas;
            _loading = FindName("Loading") as Storyboard;
            _loading.Completed += new EventHandler(_loading_Completed);
            _loading.Begin();
        }

        void _loading_Completed(object sender, EventArgs e)
        {
            _loading.Begin();
        }

        public int ID
        {
            get { return _id; }
        }

        public RotateTransform Rotation
        {
            get { return this.FindName("Rotation") as RotateTransform; }
        }

        private string TitleImageUri
        {
            get { return _titleImage.Source.ToString(); }
            set
            {
                //Downloader downloader = new Downloader();
                
                Uri imageUri = new Uri(value, UriKind.RelativeOrAbsolute);
                ////downloader.Uri = imageUri;
                //downloader.Open("GET", imageUri);
                //downloader.Send();
                //downloader.DownloadFailed += delegate {
                //    _titleImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Photo/photoTitle.png", UriKind.RelativeOrAbsolute));
                
                //};
                //downloader.Completed += delegate
                //{
                _titleImage.ImageFailed += delegate { _titleImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Photo/photoTitle.png", UriKind.RelativeOrAbsolute)); };
                _titleImage.SetValue<Uri>(Image.SourceProperty, imageUri);
                //};
                
            }
        }

        private string AuthorImageUri
        {
            get { return _authorImage.Source.ToString(); }
            set
            {
                //Downloader downloader = new Downloader();
                Uri imageUri = new Uri(value, UriKind.RelativeOrAbsolute);
                ////downloader.Uri = imageUri;
                //downloader.Open("GET", imageUri);
                //downloader.Send();
                //downloader.DownloadFailed += delegate{
                //    _authorImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Photo/photoAuthor.png", UriKind.RelativeOrAbsolute));
                //};
                //downloader.Completed += delegate
                //{
                _authorImage.ImageFailed += delegate { _authorImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Photo/photoAuthor.png", UriKind.RelativeOrAbsolute)); };
                _authorImage.SetValue<Uri>(Image.SourceProperty, imageUri);
                //};

            }
        }

        /// <summary>
        /// Show high resolution image.
        /// </summary>
        public void ShowHighRes()
        {
            // hide thum
            _mainThum.SetValue<Uri>(Image.SourceProperty, null);

            _mainImage.SetValue<Uri>(Image.SourceProperty, null);
            Uri imageUri = new Uri(_photoItem.Image, UriKind.RelativeOrAbsolute);
            _mainImage.SetValue<Uri>(Image.SourceProperty, imageUri);

            if (_mainImage.DownloadProgress >= 1)
            {
                _loadingCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                _loadingCanvas.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Show low resolution image.
        /// </summary>
        public void ShowLowRes()
        {
            _loadingCanvas.Visibility = Visibility.Collapsed;
            _mainImage.SetValue<Uri>(Image.SourceProperty, null);
            Uri imageUri = new Uri(_photoItem.Thum, UriKind.RelativeOrAbsolute);
            _mainThum.SetValue<Uri>(Image.SourceProperty, imageUri);
        }

        public void FadeIn()
        {
            (this.FindName("FadeIn") as Storyboard).Begin();
        }

        public void FadeOut()
        {
            (this.FindName("FadeOut") as Storyboard).Begin();
        }

        public void CalmDown()
        {
            (this.FindName("CalmDown") as Storyboard).Begin();
        }

        public void ShowOff()
        {
            (this.FindName("ShowOff") as Storyboard).Begin();
        }

        // The resource name used to initialize the actual object
        protected override string ResourceName
        {
            get { return "PhotoSlice.xaml"; }
        }

        public override string ToString()
        {
            return "PhotoSlice #" + _id;
        }

        #endregion
    }
}