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

namespace DiscuzAlbum.UserControl
{
    public class SortNavigator
    {
        #region Properties
        private Canvas _assemblyCanvas;
        private Canvas _thumHolder;
        private Canvas _holderClip;
        private Canvas _btnLeft;
        private Canvas _btnRight;
        private ThumSlice _currentThum;

        private Image _topNaviLeft;
        private Image _topNaviMiddle;
        private Image _topNaviRight;
        private Image _btnLeftImage;
        private Image _btnRightImage;
        private Image _dividorA;
        private Image _dividorB;

        private readonly double _slowSpeed = 4;
        private readonly double _fastSpeed = 20;

        private TextBlock _indexText1;
        private TextBlock _indexText2;
        private TextBlock _indexDivider;

        private double _speed;
        private Storyboard _moveTimer;

        private Downloader _fontDownloader;

        #endregion

        public SortNavigator(Canvas pAssembly)
        {
            _assemblyCanvas = pAssembly;
            _speed = 0;         // no animation by default.
            _moveTimer = _assemblyCanvas.FindName("ScrollTimer") as Storyboard;

            _thumHolder = _assemblyCanvas.FindName("ThumHolder") as Canvas;
            _holderClip = _assemblyCanvas.FindName("HolderClip") as Canvas;
            _btnLeft = _assemblyCanvas.FindName("BtnLeft") as Canvas;
            _btnRight = _assemblyCanvas.FindName("BtnRight") as Canvas;

            _topNaviLeft = _assemblyCanvas.FindName("TopNaviLeft") as Image;
            _topNaviMiddle = _assemblyCanvas.FindName("TopNaviMiddle") as Image;
            _topNaviRight = _assemblyCanvas.FindName("TopNaviRight") as Image;
            _btnLeftImage = _assemblyCanvas.FindName("BtnLeftImage") as Image;
            _btnRightImage = _assemblyCanvas.FindName("BtnRightImage") as Image;
            _dividorA = _assemblyCanvas.FindName("DividorA") as Image;
            _dividorB = _assemblyCanvas.FindName("DividorB") as Image;

            _indexDivider = _assemblyCanvas.FindName("IndexDivider") as TextBlock;
            _indexText1 = _assemblyCanvas.FindName("IndexText1") as TextBlock;
            _indexText2 = _assemblyCanvas.FindName("IndexText2") as TextBlock;

            _btnLeft.Cursor = Cursors.Hand;
            _btnRight.Cursor = Cursors.Hand;

            // Regist hover event
            _btnLeft.MouseEnter += new MouseEventHandler(_btnLeft_MouseEnter);
            _btnLeft.MouseLeave += new EventHandler(_btnLeft_MouseLeave);
            _btnRight.MouseEnter += new MouseEventHandler(_btnRight_MouseEnter);
            _btnRight.MouseLeave += new EventHandler(_btnRight_MouseLeave);

            // Regist click event
            _btnLeft.MouseLeftButtonDown += new MouseEventHandler(_btnLeft_MouseLeftButtonDown);
            _btnLeft.MouseLeftButtonUp += new MouseEventHandler(_btnLeft_MouseLeftButtonUp);
            _btnRight.MouseLeftButtonDown += new MouseEventHandler(_btnRight_MouseLeftButtonDown);
            _btnRight.MouseLeftButtonUp += new MouseEventHandler(_btnRight_MouseLeftButtonUp);

            // download font
            _fontDownloader = new Downloader();
            _fontDownloader.DownloadFailed += new ErrorEventHandler(_fontDownloader_DownloadFailed);
            _fontDownloader.Completed += new EventHandler(_fontDownloader_Completed);
            Uri fontUri = new Uri("Assets/Font/FuturaStd-Heavy.otf", UriKind.RelativeOrAbsolute);
            _fontDownloader.Open("GET", fontUri);
            _fontDownloader.Send();
        }

        void _fontDownloader_DownloadFailed(object sender, ErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void _fontDownloader_Completed(object sender, EventArgs e)
        {
            Debug.WriteLine("Font downloaded. " + sender);
            _indexText1.SetFontSource(sender as Downloader);
            _indexText1.FontFamily = "Futura Std Medium";
            _indexText2.SetFontSource(sender as Downloader);
            _indexText2.FontFamily = "Futura Std Medium";
            _indexDivider.SetFontSource(sender as Downloader);
            _indexDivider.FontFamily = "Futura Std Medium";

            //_indexDivider.FontWeight = FontWeights.ExtraBold;
            //_indexText1.FontWeight = FontWeights.ExtraBold;
            //_indexText2.FontWeight = FontWeights.ExtraBold;
        }

        void _btnRight_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Speed = -1 * _slowSpeed;
        }

        void _btnRight_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Speed = -1 * _fastSpeed;
        }

        void _btnLeft_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            Speed = _slowSpeed;
        }

        void _btnLeft_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Speed = _fastSpeed;
        }

        void _btnRight_MouseLeave(object sender, EventArgs e)
        {
            _btnRightImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/topButton_normal.png", UriKind.Relative));
            Speed = 0;
        }

        void _btnRight_MouseEnter(object sender, MouseEventArgs e)
        {
            _btnRightImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/topButton_hover.png", UriKind.Relative));
            Speed = -1 * _slowSpeed;
        }

        void _btnLeft_MouseLeave(object sender, EventArgs e)
        {
            _btnLeftImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/topButton_normal.png", UriKind.Relative));
            Speed = 0;
        }

        void _btnLeft_MouseEnter(object sender, MouseEventArgs e)
        {
            _btnLeftImage.SetValue<Uri>(Image.SourceProperty, new Uri("Assets/Image/topButton_hover.png", UriKind.Relative));
            Speed = _slowSpeed;
        }

        #region Methods
        public Canvas BtnLeft
        {
            get { return _btnLeft; }
        }

        public Canvas BtnRight
        {
            get { return _btnRight; }
        }

        public Canvas ThumHolder
        {
            get { return _thumHolder; }
        }

        public void Show()
        {
            (_assemblyCanvas.FindName("SortNaviIn") as Storyboard).Begin();
        }

        public void Hide()
        {
            (_assemblyCanvas.FindName("SortNaviOut") as Storyboard).Begin();
        }

        public void ChangeWidth(double pWidth)
        {
            _assemblyCanvas.Width = pWidth;

            // sync background.
            _topNaviMiddle.Width = _assemblyCanvas.Width - 11 - _topNaviLeft.Width - _topNaviRight.Width;
            _topNaviRight.SetValue<double>(Canvas.LeftProperty, (double)_topNaviMiddle.GetValue(Canvas.LeftProperty) + _topNaviMiddle.Width);
            _btnRight.SetValue<double>(Canvas.LeftProperty, (double)_topNaviRight.GetValue(Canvas.LeftProperty) - 48);

            // sync dividor
            _dividorB.SetValue<double>(Canvas.LeftProperty, (double)_topNaviRight.GetValue(Canvas.LeftProperty) - 59);

            _holderClip.Width = (double)_btnRight.GetValue(Canvas.LeftProperty) - (double)_holderClip.GetValue(Canvas.LeftProperty) - 15;
            // make _thumHolder's clip
            RectangleGeometry thumHolderClip = new RectangleGeometry();
            thumHolderClip.Rect = new Rect(0, 0, _holderClip.Width, _holderClip.Height);
            _holderClip.Clip = thumHolderClip;

            Canvas currentDisplay = _assemblyCanvas.FindName("CurrentDisplay") as Canvas;
            currentDisplay.SetValue<double>(Canvas.LeftProperty, (_assemblyCanvas.Width - currentDisplay.Width) / 2);
            
            // if the last slice's inside...
            ThumSlice lastThum = _thumHolder.Children[_thumHolder.Children.Count - 1] as ThumSlice;
            if ((double)_thumHolder.GetValue(Canvas.LeftProperty) < (_thumHolder.Width - 15 - (double)lastThum.GetValue(Canvas.LeftProperty) - lastThum.Width))
                _thumHolder.SetValue<double>(Canvas.LeftProperty, (_thumHolder.Width - 15 - (double)lastThum.GetValue(Canvas.LeftProperty) - lastThum.Width));
            if (((double)(lastThum.GetValue(Canvas.LeftProperty)) + lastThum.Width) < _holderClip.Width)
                _thumHolder.SetValue<double>(Canvas.LeftProperty, 0);
        }

        private double Speed
        {
            set
            {
                _speed = value;

                if (_speed == 0)
                {
                    _moveTimer.Completed -= _moveTimer_Completed;
                    _moveTimer.Stop();
                }
                else
                {
                    _moveTimer.Completed -= _moveTimer_Completed;
                    _moveTimer.Completed += new EventHandler(_moveTimer_Completed);
                    _moveTimer.Begin();
                }
            }
        }

        private double GetLegalPosition(double pInput)
        {
            ThumSlice lastThum = _thumHolder.Children[_thumHolder.Children.Count - 1] as ThumSlice;
            if (((double)(lastThum.GetValue(Canvas.LeftProperty)) + lastThum.Width) < _holderClip.Width)
                return 0;
            if (pInput > 0)
                pInput = 0;
            else if (pInput < (_holderClip.Width - (double)(lastThum.GetValue(Canvas.LeftProperty)) - lastThum.Width - 1))
                pInput = _holderClip.Width - (double)(lastThum.GetValue(Canvas.LeftProperty)) - lastThum.Width - 1;

            return pInput;
        }

        void _moveTimer_Completed(object sender, EventArgs e)
        {
            double des = (double)(_thumHolder.GetValue(Canvas.LeftProperty)) + _speed;
            _thumHolder.SetValue<double>(Canvas.LeftProperty, GetLegalPosition(des));
            _moveTimer.Begin();
        }

        public void MoveHighlightFrameTo(ThumSlice pSlice)
        {
            //(_assemblyCanvas.FindName("HighlightDesX") as SplineDoubleKeyFrame).Value = (double)pSlice.GetValue(Canvas.LeftProperty) - 2;
            //(_assemblyCanvas.FindName("HighlightDesY") as SplineDoubleKeyFrame).Value = (double)pSlice.GetValue(Canvas.TopProperty) - 1;
            //(_assemblyCanvas.FindName("MoveHighlightFrameTo") as Storyboard).Begin();

            // Sync text
            string lStr = (pSlice.ID + 1).ToString();
            while (lStr.Length < 3)
                lStr = " " + lStr;
            (_assemblyCanvas.FindName("IndexText1") as TextBlock).Text = lStr;
            (_assemblyCanvas.FindName("IndexText2") as TextBlock).Text = (_thumHolder.Children.Count).ToString();

            // if the high light's out of range, move it back inside.
            ThumSlice lastThum = _thumHolder.Children[_thumHolder.Children.Count - 1] as ThumSlice;
            if (((double)(lastThum.GetValue(Canvas.LeftProperty)) + lastThum.Width) < _holderClip.Width)
            {
                _thumHolder.SetValue<double>(Canvas.LeftProperty, GetLegalPosition(0));
            }
            else
            {
                // if the thum's too left...
                if ((double)_thumHolder.GetValue(Canvas.LeftProperty) + (double)pSlice.GetValue(Canvas.LeftProperty) < 30)
                { 
                    // new holder position
                    double newpos = 30 - (double)pSlice.GetValue(Canvas.LeftProperty);
                    _thumHolder.SetValue<double>(Canvas.LeftProperty, GetLegalPosition(newpos));
                }
                // else if thum's too right...
                else if ((double)_thumHolder.GetValue(Canvas.LeftProperty) + (double)pSlice.GetValue(Canvas.LeftProperty) > (_holderClip.Width - 30 - pSlice.Width))
                {
                    // new holder position
                    double newpos = _holderClip.Width - 30 - pSlice.Width - (double)pSlice.GetValue(Canvas.LeftProperty);
                    _thumHolder.SetValue<double>(Canvas.LeftProperty, GetLegalPosition(newpos));
                }
            }

            for (int i = 0; i < _thumHolder.Children.Count; i++)
            {
                ThumSlice loopSlice = _thumHolder.Children[i] as ThumSlice;
                if (loopSlice == pSlice)
                    loopSlice.DimIn();
                else
                    loopSlice.DimOut();
            }

            _currentThum = pSlice;
        }

        #endregion
    }
}
