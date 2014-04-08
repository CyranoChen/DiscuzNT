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
    public enum IconStatus { Minus, Plus, None };

    public class TracingIcon : ControlBase
    {
        #region Properties
        private Canvas _layoutRoot;
        private Image _plusImage;
        private Image _minusImage;
        private IconStatus _status;

        #endregion
        public TracingIcon()
        {
            //
        }

        public void Control_Loaded(object o, EventArgs e)
        {
            SyncProperty();
        }

        private void SyncProperty()
        {
            _layoutRoot = FindName("LayoutRoot") as Canvas;
            _plusImage = FindName("PlusImage") as Image;
            _minusImage = FindName("MinusImage") as Image;
            Status = IconStatus.None;
            _plusImage.IsHitTestVisible = false;
            _minusImage.IsHitTestVisible = false;
        }

        #region Methods
        public IconStatus Status
        {
            get { return _status; }
            set
            {
                switch (value)
                { 
                    case IconStatus.Minus:
                        _minusImage.Visibility = Visibility.Visible;
                        _plusImage.Visibility = Visibility.Collapsed;
                        break;
                    case IconStatus.Plus:
                        _minusImage.Visibility = Visibility.Collapsed;
                        _plusImage.Visibility = Visibility.Visible;
                        break;
                    case IconStatus.None:
                        _minusImage.Visibility = Visibility.Collapsed;
                        _plusImage.Visibility = Visibility.Collapsed;
                        break;
                }
                _status = value;
            }
        }

        public void FadeIn()
        {
            (this.FindName("FadeIn") as Storyboard).Begin();
        }

        public void FadeOut()
        {
            (this.FindName("FadeOut") as Storyboard).Begin();
        }

        protected override string ResourceName
        {
            get { return "TracingIcon.xaml"; }
        }

        #endregion
    }
}
