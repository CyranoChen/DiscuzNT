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
using System.Collections.ObjectModel;
using System.Windows.Interop;

using DiscuzAlbum.UserControl;

namespace DiscuzAlbum
{
    // enum AlbumStatus
    public enum AlbumStatus { Sorted, Random, Hanging };
    public enum ZoomStatus { In, Out };

    public class LayoutManager
    {
        #region Properties
        private Canvas _target;
        private Canvas _targetShadow;
        private SortNavigator _sortNavi;
        private double _sliceWidth;
        private int _cols;
        private int _rows;
        private Collection<Point> _axisSet;
        private Collection<Point> _randomPositionSet;
        private Point _sortCenterPosition;      // for the first sort slice.
        private int _currentSortSliceIndex;       // first sort slice index.
        private int _sortBufferCount;           // buffer slice count.
        private double _zoomoutScale;
        private Canvas _zoomOutTrigger;
        private Canvas _btnSort;
        private Canvas _btnRandom;
        private GenuineSlice _genuineSlice;
        private PhotoDataSet _dataSource;
        private AlbumStatus _status;
        private ZoomStatus _zoomStatus;
        private int _randomSliceCount;
        private int _renderedSliceCount;
        private Storyboard _zoomInStoryboard;
        private Storyboard _zoomOutStoryboard;
        private Storyboard _rotateToAngleStoryboard;
        private Storyboard _animateToPositionStoryboard;
        private Storyboard _layoutStandByStoryboard;

        private PhotoSlice _currentRandomSlice;
        private PhotoSlice _currentSlice;
        private int _currentScreen;
        private int _totalScreens;

        private bool _isSliceRequest;

        private Image _screenArrowLeft;
        private Image _screenArrowRight;

        private TracingIcon _tracingMouse;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for a LayoutManager instance.
        /// </summary>
        /// <param name="pTarget">Target canvas which will hold all photo slices and this manager will take effect on.</param>
        /// <param name="pDataSource">Data source for this manager instance.</param>
        /// <param name="pBtnSortCanvas">Sort layout trigger canvas.</param>
        /// <param name="pBtnRandomCanvas">Random layout trigger canvas.</param>
        /// <param name="pZoomOutTrigger">Zoomout trigter canvas.</param>
        /// <param name="pSortNavi">Sort navigator.</param>
        public LayoutManager(Canvas pTarget, PhotoDataSet pDataSource, Canvas pBtnSortCanvas, Canvas pBtnRandomCanvas, Canvas pZoomOutTrigger, SortNavigator pSortNavi, GenuineSlice pGenuineSlice, TracingIcon pTracingIcon, Image pScreenArrowLeft, Image pScreenArrowRight)
        {
            _target = pTarget;
            _targetShadow = pTarget.FindName("PhotoHolderShadow") as Canvas;
            _sortNavi = pSortNavi;
            _dataSource = pDataSource;
            _zoomOutTrigger = pZoomOutTrigger;
            _btnSort = pBtnSortCanvas;
            _btnSort.Cursor = Cursors.Hand;
            _btnRandom = pBtnRandomCanvas;
            _btnRandom.Cursor = Cursors.Hand;
            _genuineSlice = pGenuineSlice;
            _isSliceRequest = false;
            _tracingMouse = pTracingIcon;
            _screenArrowLeft = pScreenArrowLeft;
            _screenArrowRight = pScreenArrowRight;

            // Generate random & sort params
            GenerateLayoutPostion();
            // Fill thums to thumHolder
            FillThumHolder();

            // Event register.
            _target.MouseLeftButtonUp += new MouseEventHandler(_target_MouseLeftButtonUp);
            _zoomOutTrigger.MouseLeftButtonUp += new MouseEventHandler(_zoomOutTrigger_MouseLeftButtonUp);
            _zoomInStoryboard = _target.FindName("ZoomIn") as Storyboard;
            _zoomOutStoryboard = _target.FindName("ZoomOut") as Storyboard;
            _rotateToAngleStoryboard = _target.FindName("RotateToAngle") as Storyboard;
            _animateToPositionStoryboard = _target.FindName("AnimateToPosition") as Storyboard;
            _zoomInStoryboard.Completed += new EventHandler(_zoomInStoryboard_Completed);
            _zoomOutStoryboard.Completed += new EventHandler(_zoomOutStoryboard_Completed);
            _layoutStandByStoryboard = _target.FindName("LayoutStandBy") as Storyboard;
            _layoutStandByStoryboard.Completed += new EventHandler(_layoutStandByStoryboard_Completed);

            _btnSort.MouseLeftButtonUp += new MouseEventHandler(_btnSort_MouseLeftButtonUp);
            _btnSort.MouseEnter += new MouseEventHandler(_btnSort_MouseEnter);
            _btnSort.MouseLeave += new EventHandler(_btnSort_MouseLeave);
            _btnRandom.MouseLeftButtonUp += new MouseEventHandler(_btnRandom_MouseLeftButtonUp);
            _btnRandom.MouseEnter += new MouseEventHandler(_btnRandom_MouseEnter);
            _btnRandom.MouseLeave += new EventHandler(_btnRandom_MouseLeave);

            _screenArrowLeft.MouseLeftButtonUp += new MouseEventHandler(_screenArrowLeft_MouseLeftButtonUp);
            _screenArrowRight.MouseLeftButtonUp += new MouseEventHandler(_screenArrowRight_MouseLeftButtonUp);

            // By default, enter sorted layout.
            _status = AlbumStatus.Hanging;     // clear status
            Status = AlbumStatus.Sorted;
            _zoomStatus = ZoomStatus.Out;
        }

        void _screenArrowRight_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            RightScreen();
        }

        void _screenArrowLeft_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            LeftScreen();
        }

        void _btnRandom_MouseLeave(object sender, EventArgs e)
        {
            (_target.FindName("BtnRandomOut") as Storyboard).Begin();
        }

        void _btnRandom_MouseEnter(object sender, MouseEventArgs e)
        {
            (_target.FindName("BtnRandomHover") as Storyboard).Begin();
        }

        void _btnSort_MouseLeave(object sender, EventArgs e)
        {
            (_target.FindName("BtnSortOut") as Storyboard).Begin();
        }

        void _btnSort_MouseEnter(object sender, MouseEventArgs e)
        {
            (_target.FindName("BtnSortHover") as Storyboard).Begin();
        }

        private void FillThumHolder()
        {
            for (int i = 0; i < _dataSource.Children.Count; i++)
            {
                ThumSlice thumRec = new ThumSlice(i, _dataSource.Children[i].Thum);

                thumRec.SetValue<double>(Canvas.LeftProperty, 110 * i);
                thumRec.SetValue<double>(Canvas.TopProperty, 15);

                _sortNavi.ThumHolder.Children.Add(thumRec);

                thumRec.OnSliceClicked += new EventHandler<ThumClickEventArgs>(thumRec_OnSliceClicked);
            }
            _sortNavi.MoveHighlightFrameTo(_sortNavi.ThumHolder.Children[0] as ThumSlice);
        }

        void thumRec_OnSliceClicked(object sender, ThumClickEventArgs e)
        {
            // move highlight
            ThumSlice senderSlice = sender as ThumSlice;
            _sortNavi.MoveHighlightFrameTo(senderSlice);
            SyncSortIndex(e.SliceID);
            _currentSlice = _targetShadow.Children[e.SliceID] as PhotoSlice;
        }     

        /// <summary>
        /// Generate random & sort layout positions and param(zoomscale, etc.).
        /// </summary>
        private void GenerateLayoutPostion()
        {
            double sliceWidth;
            double sliceHeight;
            double stageWidth = _target.Width;
            _targetShadow.Width = _target.Width;
            _targetShadow.Height = _target.Height;
            double stageHeight = _target.Height - _btnRandom.Height;
            PhotoSlice measureSlice = new PhotoSlice(this, -1, new PhotoItem());
            sliceWidth = _sliceWidth = measureSlice.Width;
            sliceHeight = measureSlice.Height;

            // Generate position grid.
            _randomPositionSet = new Collection<Point>();

            // Get grid matirix.
            _rows = CommonValues.FixRows;
            _cols = CommonValues.FixCols;

            // each grid's size should be...
            double gridWidth = stageWidth / _cols;
            double gridHeight = stageHeight / _rows;

            // Calculate zoomout scale.
            double scaleX = gridWidth / sliceWidth * CommonValues.FreespaceRatio;
            double scaleY = gridHeight / sliceHeight * CommonValues.FreespaceRatio;

            _zoomoutScale = Math.Min(scaleX, scaleY);
            if (_zoomoutScale > CommonValues.ZoomOutScale)
                _zoomoutScale = CommonValues.ZoomOutScale;

            #region generate random positions.
            _totalScreens = _dataSource.Children.Count / (CommonValues.FixCols * CommonValues.FixRows);
            if ((_dataSource.Children.Count % (CommonValues.FixCols * CommonValues.FixRows)) > 0)
                _totalScreens += 1;

            for (int itemFlag = 0; itemFlag < _dataSource.Children.Count; itemFlag++)
            {
                int screenCount = itemFlag / (CommonValues.FixCols * CommonValues.FixRows);
                int screenIndex = itemFlag % (CommonValues.FixCols * CommonValues.FixRows);
                int rowFlag = screenIndex / CommonValues.FixCols;
                int colFlag = screenIndex % CommonValues.FixCols;

                Debug.WriteLine("slice - " + itemFlag);
                Debug.WriteLine("screen #" + screenCount + " / " + rowFlag + " / " + colFlag );
                double xpos = (gridWidth / 2 + gridWidth * colFlag) / _zoomoutScale - stageWidth / _zoomoutScale / 2 + sliceWidth / 2 + screenCount * stageWidth;
                double ypos = (gridHeight / 2 + gridHeight * rowFlag) / _zoomoutScale - stageHeight / _zoomoutScale / 2;
                _randomPositionSet.Add(new Point(xpos, ypos));
            }

            #endregion

            #region calculate sort params.
            // reset stage's size.
            stageWidth = _target.Width;
            stageHeight = _target.Height - _btnRandom.Height;

            _sortCenterPosition = new Point((stageWidth - sliceWidth) / 2, (stageHeight - sliceHeight) / 2 + 50);
            _sortBufferCount = 0;
            while ((sliceWidth * _sortBufferCount + CommonValues.SortedMargin * (_sortBufferCount - 1)) < stageWidth)
            {
                _sortBufferCount++;
            }
            if (_sortBufferCount < 5)
                _sortBufferCount = 5;           // at least 3 buffers.
            _currentSortSliceIndex = 0;         // initialize this value.

            #endregion
        }

        /// <summary>
        /// If this manager instance's in random layout, and the host's resized, call this func.
        /// </summary>
        public void RefreshRandom()
        {
            if (_status == AlbumStatus.Random)
            {
                GenerateRandomPosition();

                // refresh all slices.
                LayoutToRandom();
            }
        }

        /* GenerateRandomPosition_bak()
        private void GenerateRandomPosition_bak()
        {
            double sliceWidth;
            double sliceHeight;
            double stageWidth = _target.Width;
            double stageHeight = _target.Height - _btnRandom.Height;
            PhotoSlice measureSlice = new PhotoSlice(this, -1, new PhotoItem());
            sliceWidth = measureSlice.Width;
            sliceHeight = measureSlice.Height;

            // Generate position grid.
            _randomPositionSet = new Collection<Point>();

            // Get grid matirix.
            int itemCount = _dataSource.Children.Count;

            // each grid's size should be...
            double gridWidth = BrowserHost.ActualWidth / _cols;
            double gridHeight = (BrowserHost.ActualHeight - _btnRandom.Height - 30) / _rows;

            // Calculate zoomout scale.
            double scaleX = gridWidth / sliceWidth * CommonValues.FreespaceRatio;
            double scaleY = gridHeight / sliceHeight * CommonValues.FreespaceRatio;

            _zoomoutScale = Math.Min(scaleX, scaleY);
            if (_zoomoutScale > CommonValues.ZoomOutScale)
                _zoomoutScale = CommonValues.ZoomOutScale;

            for (int i = 0; i < itemCount; i++)
            {
                double xpos = (gridWidth / 2 + gridWidth * _axisSet[i].X) / _zoomoutScale - (BrowserHost.ActualWidth) / _zoomoutScale / 2 + sliceWidth / 2;
                double ypos = (gridHeight / 2 + gridHeight * _axisSet[i].Y) / _zoomoutScale - (BrowserHost.ActualHeight - _btnRandom.Height - 30) / _zoomoutScale / 2;
                _randomPositionSet.Add(new Point(xpos, ypos));
            }
        }
        */

        private void GenerateRandomPosition()
        {
            double sliceWidth;
            double sliceHeight;
            double stageWidth = BrowserHost.ActualWidth; ;
            double stageHeight = BrowserHost.ActualHeight - _btnRandom.Height;
            _targetShadow.Width = _target.Width;
            _targetShadow.Height = _target.Height;
            PhotoSlice measureSlice = new PhotoSlice(this, -1, new PhotoItem());
            sliceWidth = measureSlice.Width;
            sliceHeight = measureSlice.Height;

            // Generate position grid.
            _randomPositionSet.Clear();

            // each grid's size should be...
            double gridWidth = stageWidth / _cols;
            double gridHeight = stageHeight / _rows;

            // Calculate zoomout scale.
            double scaleX = gridWidth / sliceWidth * CommonValues.FreespaceRatio;
            double scaleY = gridHeight / sliceHeight * CommonValues.FreespaceRatio;

            _zoomoutScale = Math.Min(scaleX, scaleY);
            if (_zoomoutScale > CommonValues.ZoomOutScale)
                _zoomoutScale = CommonValues.ZoomOutScale;

            #region generate random positions.
            _totalScreens = _dataSource.Children.Count / (CommonValues.FixCols * CommonValues.FixRows);
            if ((_dataSource.Children.Count % (CommonValues.FixCols * CommonValues.FixRows)) > 0)
                _totalScreens += 1;

            for (int itemFlag = 0; itemFlag < _dataSource.Children.Count; itemFlag++)
            {
                int screenCount = itemFlag / (CommonValues.FixCols * CommonValues.FixRows);
                int screenIndex = itemFlag % (CommonValues.FixCols * CommonValues.FixRows);
                int rowFlag = screenIndex / CommonValues.FixCols;
                int colFlag = screenIndex % CommonValues.FixCols;

                Debug.WriteLine("slice - " + itemFlag);
                Debug.WriteLine("screen #" + screenCount + " / " + rowFlag + " / " + colFlag);
                double xpos = (gridWidth / 2 + gridWidth * colFlag) / _zoomoutScale - stageWidth / _zoomoutScale / 2 + sliceWidth / 2 + screenCount * stageWidth / _zoomoutScale;
                double ypos = (gridHeight / 2 + gridHeight * rowFlag) / _zoomoutScale - stageHeight / _zoomoutScale / 2;
                Debug.WriteLine(xpos + " / " + ypos);
                _randomPositionSet.Add(new Point(xpos, ypos));
            }

            #endregion
        }

        void _target_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            TryToZoomOut();
        }

        void _zoomOutTrigger_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            _isSliceRequest = false;
            TryToZoomOut();
        }

        private void TryToZoomOut()
        {
            if (!_isSliceRequest)
            {
                if (_zoomStatus != ZoomStatus.Out && _status == AlbumStatus.Random)
                {
                    ZoomOut();
                    (_target.FindName("parentCanvas") as Page).ShowBothArrow();
                }
            }
        }

        void _zoomOutStoryboard_Completed(object sender, EventArgs e)
        {
            _zoomStatus = ZoomStatus.Out;
        }

        void _layoutStandByStoryboard_Completed(object sender, EventArgs e)
        {
            // Standby complete...
            switch (_status)
            { 
                case AlbumStatus.Sorted:
                    LayoutToSorted();
                    break;
                case AlbumStatus.Random:
                    LayoutToRandom();
                    break;
                default:
                    throw (new Exception("Invalid AlbumStatus Encounted."));
            }
        }

        void _btnRandom_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Status = AlbumStatus.Random;
        }

        void _btnSort_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Status = AlbumStatus.Sorted;
        }

        void _zoomInStoryboard_Completed(object sender, EventArgs e)
        {
            _isSliceRequest = false;
            _zoomStatus = ZoomStatus.In;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Get the target canvas of this manager instance.
        /// </summary>
        public Canvas Target
        {
            get { return _target; }
        }

        /// <summary>
        /// Get or set the status of this album.
        /// "Hanging" status can't be set through outside.
        /// </summary>
        public AlbumStatus Status
        {
            get { return _status; }
            set 
            {
                AlbumStatus lastStatus = _status;
                _status = value;
                if (_status != lastStatus)
                {
                    switch (_status)
                    { 
                        case AlbumStatus.Random:
                            (_target.FindName("BtnBottomStrokeLeft") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/btnBottomStroke.png", UriKind.RelativeOrAbsolute));
                            (_target.FindName("BtnBottomStrokeRight") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/btnBottomStroke_flip.png", UriKind.RelativeOrAbsolute));
                            (_target.FindName("ScreenArrowFadeIn") as Storyboard).Begin();
                            (_target.FindName("ArrowCanvas") as Canvas).Visibility = Visibility.Collapsed;
                            _sortNavi.Hide();
                            LayoutToRandom();
                            break;
                        case AlbumStatus.Sorted:
                            (_target.FindName("BtnBottomStrokeLeft") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/btnBottomStroke_flip.png", UriKind.RelativeOrAbsolute));
                            (_target.FindName("BtnBottomStrokeRight") as Image).SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/btnBottomStroke.png", UriKind.RelativeOrAbsolute));
                            (_target.FindName("ScreenArrowFadeOut") as Storyboard).Begin();
                            (_target.FindName("ArrowCanvas") as Canvas).Visibility = Visibility.Visible;
                            _sortNavi.Show();
                            LayoutToSorted();
                            break;
                        default:
                            throw (new Exception("Invalid AlbumStatus Encounted."));
                    }
                }
            }
        }

        public ZoomStatus ZoomStatus
        {
            get { return _zoomStatus; }
        }

        #endregion

        #region Private Methods
        private void LayoutStandBy()
        {
            _layoutStandByStoryboard.Begin();
        }
        
        private void LayoutToRandom()
        {
            GenerateRandomPosition();

            // Update slice counter
            _randomSliceCount = _dataSource.Children.Count;
            _targetShadow.Children.Clear();
            _renderedSliceCount = 0;
            _currentScreen = 0;

            // Show slices.
            for (int i = 0; i < _dataSource.Children.Count; i++)
            {
                PhotoSlice mySlice = new PhotoSlice(this, i, _dataSource.Children[i]);
                mySlice.OnSliceLoaded += new EventHandler<SliceLoadedEventArgs>(mySlice_OnSliceLoaded);
                _targetShadow.Children.Add(mySlice);

                mySlice.SetValue<double>(Canvas.LeftProperty, -1000);
                mySlice.SetValue<double>(Canvas.TopProperty, -1000);
            }
        }

        void mySlice_OnSliceLoaded(object sender, SliceLoadedEventArgs e)
        {
            _renderedSliceCount++;
            (sender as PhotoSlice).ShowLowRes();
            if(_renderedSliceCount ==_randomSliceCount)
                OnRandomSliceInitialed();
        }

        private void OnRandomSliceInitialed()
        {
            for (int i = 0; i < _targetShadow.Children.Count; i++)
            {
                PhotoSlice mySlice = _targetShadow.Children[i] as PhotoSlice;

                // Gernerate  position & rotation.
                Random random = new Random(i);

                // Using the following statements to generate random position.
                //mySlice.SetValue<double>(Canvas.LeftProperty, random.NextDouble() * _target.Width / CommonValues.ZoomOutScale - _target.Width / CommonValues.ZoomOutScale / 2);
                //mySlice.SetValue<double>(Canvas.TopProperty, random.NextDouble() * _target.Height / CommonValues.ZoomOutScale - _target.Width / CommonValues.ZoomOutScale / 2);

                // Using the following statements to generate sort position.
                mySlice.SetValue<double>(Canvas.LeftProperty, _randomPositionSet[i].X);
                mySlice.SetValue<double>(Canvas.TopProperty, _randomPositionSet[i].Y);

                double angleSeed = 0;
                if (i % 3 == 0)
                    angleSeed = random.NextDouble() * 0.5 + 0.5;
                else
                    angleSeed = random.NextDouble() * 0.5;

                mySlice.Rotation.Angle = angleSeed * (CommonValues.maxRotation - CommonValues.minRotation) + CommonValues.minRotation;

                // Register event
                mySlice.MouseLeftButtonUp += new MouseEventHandler(mySlice_MouseLeftButtonUp);
                mySlice.MouseLeave += new EventHandler(mySlice_MouseLeave);
                mySlice.MouseEnter += new MouseEventHandler(mySlice_MouseEnter);
            }

            // Shrink target canvas after initialization.
            _currentScreen = 0;
            MoveScreen(0);
            ZoomOut();
        }

        void mySlice_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_zoomStatus == ZoomStatus.Out)
                (sender as PhotoSlice).ShowOff();
            else
            {
                if ((sender as PhotoSlice) == _currentSlice)
                {
                    _tracingMouse.Status = IconStatus.Plus;
                    _tracingMouse.FadeIn();
                }
                else
                {
                    //_tracingMouse.Status = IconStatus.None;
                    _tracingMouse.FadeOut();
                }
            }
        }

        void mySlice_MouseLeave(object sender, EventArgs e)
        {
            //_tracingMouse.Status = IconStatus.None;

            if (_zoomStatus == ZoomStatus.Out)
                (sender as PhotoSlice).CalmDown();
            else
                _tracingMouse.FadeOut();
        }

        void mySlice_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_zoomStatus == ZoomStatus.In)
            {
                if (_currentSlice == (sender as PhotoSlice))
                    _genuineSlice.ShowImage(_dataSource.Children[(sender as PhotoSlice).ID].Image);
                else
                    _currentSlice = sender as PhotoSlice;
            }

            _isSliceRequest = true;

            PhotoSlice senderSlice = sender as PhotoSlice;
            _currentRandomSlice = senderSlice;
            _currentSlice = senderSlice;

            // Sync resolution status.
            _currentRandomSlice.ShowHighRes();

            // Swap z-index
            //_target.Children.Remove(senderSlice);
            //_target.Children.Add(senderSlice);

            // Calculate des position.
            Point desPoint = GetAnimationDestination(senderSlice);

            // Active animation.
            (_target.FindName("parentCanvas") as Page).HideLeftArrow();
            (_target.FindName("parentCanvas") as Page).HideRightArrow();
            ZoomIn();
            RotateToAngle(-1 * senderSlice.Rotation.Angle);
            AnimateToPosition(desPoint.X, desPoint.Y);
        }

        private Point GetAnimationDestination(PhotoSlice pSlice)
        {
            // length angle
            double length = Math.Sqrt(Math.Pow(GetSliceOriginPosX(pSlice), 2) + Math.Pow(GetSliceOriginPosY(pSlice), 2));
            double lengthAngle = Math.Atan2(GetSliceOriginPosY(pSlice), GetSliceOriginPosX(pSlice)) * 180 / Math.PI;
            double totalAngle = pSlice.Rotation.Angle - lengthAngle;

            //double desX = -1 * GetSliceOriginPosX(senderSlice);
            //double desY = -1 * GetSliceOriginPosY(senderSlice);

            double desX = -1 * length * Math.Cos(totalAngle * Math.PI / 180);
            double desY = length * Math.Sin(totalAngle * Math.PI / 180);

            return new Point(Math.Round(desX), Math.Round(desY));
        }

        private void ZoomIn()
        {
            try
            {
                _currentRandomSlice.ShowHighRes();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Zoom In Exception Encounted: " + ex);
            }

            // arrows will have no function..
            _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_none.png", UriKind.Relative));
            _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_none.png", UriKind.Relative));
            // Make event
            _screenArrowLeft.MouseEnter -= _screenArrowLeft_MouseEnter;
            _screenArrowLeft.MouseLeave -= _screenArrowLeft_MouseLeave;

            _screenArrowRight.MouseEnter -= _screenArrowRight_MouseEnter;
            _screenArrowRight.MouseLeave -= _screenArrowRight_MouseLeave;

            _screenArrowLeft.Cursor = Cursors.Default;
            _screenArrowRight.Cursor = Cursors.Default;


            _zoomInStoryboard.Begin();
        }

        private void ZoomOut()
        {
            SplineDoubleKeyFrame zoomScaleX = _target.FindName("ZoomOutScaleX") as SplineDoubleKeyFrame;
            SplineDoubleKeyFrame zoomScaleY = _target.FindName("ZoomOutScaleY") as SplineDoubleKeyFrame;

            // all slice show low res
            for (int i = 0; i < _targetShadow.Children.Count; i++)
            {
                (_targetShadow.Children[i] as PhotoSlice).ShowLowRes();
            }

            zoomScaleX.Value = _zoomoutScale;
            zoomScaleY.Value = _zoomoutScale;
            _zoomOutStoryboard.Begin();
            AnimateToPosition(0, 0);
            RotateToAngle(0);
            MoveScreen(_currentScreen);
        }

        private void RotateToAngle(double pAngle)
        {
            SplineDoubleKeyFrame rotateKeyFrame = (_target.FindName("RotateKeyFrame") as SplineDoubleKeyFrame);
            rotateKeyFrame.Value = pAngle;
            _rotateToAngleStoryboard.Begin();
        }

        private void AnimateToPosition(double posX, double posY)
        {
            SplineDoubleKeyFrame animateXKeyFrame = (_target.FindName("AnimatePositionX") as SplineDoubleKeyFrame);
            SplineDoubleKeyFrame animateYKeyFrame = (_target.FindName("AnimatePositionY") as SplineDoubleKeyFrame);
            animateXKeyFrame.Value = posX;
            animateYKeyFrame.Value = posY;
            _animateToPositionStoryboard.Begin();
        }

        private double GetSliceOriginPosX(PhotoSlice pSlice)
        {
            return (double)(pSlice.GetValue(Canvas.LeftProperty)) + pSlice.Width / 2 - _target.Width / 2 + (double)_targetShadow.GetValue(Canvas.LeftProperty);
        }

        private double GetSliceOriginPosY(PhotoSlice pSlice)
        {
            return (double)(pSlice.GetValue(Canvas.TopProperty)) + pSlice.Height / 2 - _target.Height / 2 + (double)_targetShadow.GetValue(Canvas.TopProperty); ;
        }

        private void LayoutToSorted()
        {
            // Clear Holder
            _targetShadow.Children.Clear();
            _renderedSliceCount = 0;

            // Reset _target
            RotateToAngle(0);
            AnimateToPosition(0, 0);
            _currentScreen = 0;
            MoveScreen(0);
            ZoomIn();

            // Reset slice pointer
            _currentSortSliceIndex = 0;

            // Add all slices.
            for (int i = 0; i < _dataSource.Children.Count; i++)
            {
                PhotoSlice sortSlice = new PhotoSlice(this, i, _dataSource.Children[i]);
                sortSlice.OnSliceLoaded += new EventHandler<SliceLoadedEventArgs>(sortSlice_OnSliceLoaded);
                sortSlice.MouseLeftButtonUp += new MouseEventHandler(sortSlice_MouseLeftButtonUp);
                sortSlice.MouseEnter += new MouseEventHandler(sortSlice_MouseEnter);
                sortSlice.MouseLeave += new EventHandler(sortSlice_MouseLeave);
                _targetShadow.Children.Add(sortSlice);
                Point desPoint = GetSortSlicePosition(i);
                sortSlice.SetValue<double>(Canvas.LeftProperty, Math.Round(desPoint.X));
                sortSlice.SetValue<double>(Canvas.TopProperty, Math.Round(desPoint.Y));
            }

            _sortNavi.MoveHighlightFrameTo(_sortNavi.ThumHolder.Children[0] as ThumSlice);
            _currentSlice = _targetShadow.Children[0] as PhotoSlice;

            (_target.FindName("parentCanvas") as Page).ShowBothArrow();
            (_target.FindName("parentCanvas") as Page).HideLeftArrow();
            if(_dataSource.Children.Count<2)
            {
                (_target.FindName("parentCanvas") as Page).HideRightArrow();
            }
        }

        void sortSlice_MouseLeave(object sender, EventArgs e)
        {
            //_tracingMouse.Status = IconStatus.None;
            _tracingMouse.FadeOut();
        }

        void sortSlice_MouseEnter(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(_zoomOutTrigger);
            if ((sender as PhotoSlice) == _currentSlice)
            {
                _tracingMouse.Status = IconStatus.Plus;
                //_tracingMouse.SetValue<double>(Canvas.LeftProperty, pos.X);
                //_tracingMouse.SetValue<double>(Canvas.TopProperty, pos.Y);
                _tracingMouse.FadeIn();
            }
            else
            {
                //_tracingMouse.Status = IconStatus.None;
                _tracingMouse.FadeOut();
            }
        }

        void sortSlice_OnSliceLoaded(object sender, SliceLoadedEventArgs e)
        {
            if (e.SliceID == _currentSortSliceIndex)
            {
                (sender as PhotoSlice).ShowHighRes();
                (sender as PhotoSlice).FadeIn();
            }
            else if(Math.Abs(_currentSortSliceIndex - e.SliceID) <= CommonValues.SortBufferCount)
            {
                (sender as PhotoSlice).ShowHighRes();
                (sender as PhotoSlice).FadeOut();
            }
            else
            {
                (sender as PhotoSlice).ShowLowRes();
                (sender as PhotoSlice).FadeOut();
            }
        }

        /// <summary>
        /// Get specified sorted slice's position.
        /// </summary>
        /// <param name="pID">Slice's ID.</param>
        /// <returns>Point position in x/y axis.</returns>
        private Point GetSortSlicePosition(int pID)
        {
            double posX = _sortCenterPosition.X + pID * (CommonValues.SortedMargin + _sliceWidth);
            return new Point(posX, _sortCenterPosition.Y);
        }

        /// <summary>
        /// Sync specified index slice to stage center.
        /// </summary>
        /// <param name="pIndex">Index of the slice which will be place to the stage center.</param>
        private void SyncSortIndex(int pIndex)
        {
            // Verify pIndex
            if(pIndex < 0)
                pIndex = 0;
            if(pIndex >= _dataSource.Children.Count)
                pIndex = _dataSource.Children.Count - 1;

            // Sync Page's sort arrow
            if (pIndex == 0)
            {
                (_target.FindName("parentCanvas") as Page).ShowBothArrow();
                (_target.FindName("parentCanvas") as Page).HideLeftArrow();
            }
            else if (pIndex == _dataSource.Children.Count - 1)
            {
                (_target.FindName("parentCanvas") as Page).ShowBothArrow();
                (_target.FindName("parentCanvas") as Page).HideRightArrow();
            }
            else
            {
                (_target.FindName("parentCanvas") as Page).ShowBothArrow();
            }

            PhotoSlice syncSlice = _targetShadow.Children[pIndex] as PhotoSlice;

            //////
            for (int i = 0; i < _targetShadow.Children.Count; i++)
            {
                if (pIndex == i)
                {
                    (_targetShadow.Children[i] as PhotoSlice).ShowHighRes();
                    (_targetShadow.Children[i] as PhotoSlice).FadeIn();
                }
                else if (Math.Abs(pIndex - i) <= CommonValues.SortBufferCount)
                {
                    (_targetShadow.Children[i] as PhotoSlice).ShowHighRes();
                    (_targetShadow.Children[i] as PhotoSlice).FadeOut();
                }
                else
                {
                    (_targetShadow.Children[i] as PhotoSlice).ShowLowRes();
                    (_targetShadow.Children[i] as PhotoSlice).FadeOut();
                }
            }
            Point desPoint = GetAnimationDestination(syncSlice);
            _isSliceRequest = true;
            AnimateToPosition(desPoint.X, 0);

            // Move navi's highlight
            _sortNavi.MoveHighlightFrameTo(_sortNavi.ThumHolder.Children[pIndex] as ThumSlice);

            _currentSortSliceIndex = pIndex;
            _currentSlice = _targetShadow.Children[pIndex] as PhotoSlice;
        }

        public void LeftScroll()
        {
            SyncSortIndex(_currentSortSliceIndex + 1);
        }

        public void RightScroll()
        {
            SyncSortIndex(_currentSortSliceIndex - 1);
        }

        public void LeftScreen()
        {
            if (_zoomStatus == ZoomStatus.Out)
            {
                if ((_currentScreen - 1) >= 0)
                {
                    _currentScreen--;

                    Debug.WriteLine("Left screen...");
                    MoveScreen(_currentScreen);
                }
            }
        }

        public void RightScreen()
        {
            if (_zoomStatus == ZoomStatus.Out)
            {
                if ((_currentScreen + 1) < _totalScreens)
                {
                    _currentScreen++;
                    Debug.WriteLine("total: " + _totalScreens);
                    Debug.WriteLine("Right screen...");
                    MoveScreen(_currentScreen);
                }
            }
        }

        private void MoveScreen(int pScreen)
        {
            (_target.FindName("MoveScreenX") as SplineDoubleKeyFrame).Value = -1 * pScreen * BrowserHost.ActualWidth / _zoomoutScale;
            (_target.FindName("MoveScreen") as Storyboard).Begin();

            // sync screen arrows.
            if (pScreen == 0)
            {
                _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_none.png", UriKind.Relative));
                _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
                // Make event
                _screenArrowLeft.MouseEnter -= _screenArrowLeft_MouseEnter;
                _screenArrowLeft.MouseLeave -= _screenArrowLeft_MouseLeave;

                _screenArrowRight.MouseEnter -= _screenArrowRight_MouseEnter;
                _screenArrowRight.MouseLeave -= _screenArrowRight_MouseLeave;
                _screenArrowRight.MouseEnter += new MouseEventHandler(_screenArrowRight_MouseEnter);
                _screenArrowRight.MouseLeave += new EventHandler(_screenArrowRight_MouseLeave);

                _screenArrowLeft.Cursor = Cursors.Default;
                _screenArrowRight.Cursor = Cursors.Hand;
            }
            else if (pScreen == (_totalScreens - 1))
            {
                _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
                _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_none.png", UriKind.Relative));
                // Make event
                _screenArrowLeft.MouseEnter -= _screenArrowLeft_MouseEnter;
                _screenArrowLeft.MouseLeave -= _screenArrowLeft_MouseLeave;
                _screenArrowLeft.MouseEnter += new MouseEventHandler(_screenArrowLeft_MouseEnter);
                _screenArrowLeft.MouseLeave += new EventHandler(_screenArrowLeft_MouseLeave);

                _screenArrowRight.MouseEnter -= _screenArrowRight_MouseEnter;
                _screenArrowRight.MouseLeave -= _screenArrowRight_MouseLeave;

                _screenArrowLeft.Cursor = Cursors.Hand;
                _screenArrowRight.Cursor = Cursors.Default;
            }
            else
            {
                _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
                _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
                // Make event
                _screenArrowLeft.MouseEnter -= _screenArrowLeft_MouseEnter;
                _screenArrowLeft.MouseLeave -= _screenArrowLeft_MouseLeave;
                _screenArrowLeft.MouseEnter += new MouseEventHandler(_screenArrowLeft_MouseEnter);
                _screenArrowLeft.MouseLeave += new EventHandler(_screenArrowLeft_MouseLeave);

                _screenArrowRight.MouseEnter -= _screenArrowRight_MouseEnter;
                _screenArrowRight.MouseLeave -= _screenArrowRight_MouseLeave;
                _screenArrowRight.MouseEnter += new MouseEventHandler(_screenArrowRight_MouseEnter);
                _screenArrowRight.MouseLeave += new EventHandler(_screenArrowRight_MouseLeave);

                _screenArrowLeft.Cursor = Cursors.Hand;
                _screenArrowRight.Cursor = Cursors.Hand;
            }

            if ((pScreen + 1) >= _totalScreens)
            {
                _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_none.png", UriKind.Relative));
                // Make event
                _screenArrowRight.MouseEnter -= _screenArrowRight_MouseEnter;
                _screenArrowRight.MouseLeave -= _screenArrowRight_MouseLeave;
            }
        }

        void _screenArrowRight_MouseLeave(object sender, EventArgs e)
        {
            _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
            (_target.FindName("ScreenArrowRightOut") as Storyboard).Begin();
        }

        void _screenArrowRight_MouseEnter(object sender, MouseEventArgs e)
        {
            _screenArrowRight.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_hover.png", UriKind.Relative));
            (_target.FindName("ScreenArrowRightHover") as Storyboard).Begin();
        }

        void _screenArrowLeft_MouseLeave(object sender, EventArgs e)
        {
            _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_normal.png", UriKind.Relative));
            (_target.FindName("ScreenArrowLeftOut") as Storyboard).Begin();
        }

        void _screenArrowLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            _screenArrowLeft.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/screenArrow_hover.png", UriKind.Relative));
            (_target.FindName("ScreenArrowLeftHover") as Storyboard).Begin();
        }

        void sortSlice_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (_currentSlice == sender as PhotoSlice)
            {
                _genuineSlice.ShowImage(_dataSource.Children[(sender as PhotoSlice).ID].Image);
            }
            else
            {
                _currentSlice = sender as PhotoSlice;
                SyncSortIndex((sender as PhotoSlice).ID);
            }
        }

        private TranslateTransform TargetTranslation
        {
            get { return (_target.RenderTransform as TransformGroup).Children[0] as TranslateTransform; }
        }

        private ScaleTransform TargetScale
        {
            get { return (_target.RenderTransform as TransformGroup).Children[1] as ScaleTransform; }
        }

        private RotateTransform TargetRotation
        {
            get { return (_target.RenderTransform as TransformGroup).Children[2] as RotateTransform; }
        }

        #endregion
    }
}
