using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DiscuzAlbum.UserControl;
using DiscuzAlbum.Common;

using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DiscuzAlbum
{
    public class ThumClickEventArgs : EventArgs
    {
        private int sliceID;

        public ThumClickEventArgs(int pId)
        {
            sliceID = pId;
        }
        public int SliceID
        {
            get { return sliceID; }
            set { sliceID = value; }
        }
    }

    public class ThumSlice : ControlBase
    {
        #region Properties
        private int _id;
        private string _thumUri;

        private Image _thumImg;
        private Canvas _layoutRoot;

        public event EventHandler<ThumClickEventArgs> OnSliceClicked;

        #endregion

        public ThumSlice(int pID, string pThumUri)
		{
            _id = pID;
            _thumUri = pThumUri;
		}

        public void Control_Loaded(object o, EventArgs e)
        {
            // hand cursor
            this.Cursor = Cursors.Hand;

            _thumImg = this.FindName("ThumImg") as Image;
            _layoutRoot = this.FindName("LayoutRoot") as Canvas;

            _layoutRoot.MouseEnter += new MouseEventHandler(_layoutRoot_MouseEnter);
            _layoutRoot.MouseLeave += new EventHandler(_layoutRoot_MouseLeave);
            _layoutRoot.MouseLeftButtonUp += new MouseEventHandler(_layoutRoot_MouseLeftButtonUp);

            Uri imageUri = new Uri(_thumUri, UriKind.RelativeOrAbsolute);
            _thumImg.SetValue<Uri>(Image.SourceProperty, imageUri);
        }

        void _layoutRoot_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            // broadcast event.
            EventHandler<ThumClickEventArgs> temp = OnSliceClicked;
            if (temp != null)
                temp(this, new ThumClickEventArgs(_id));
        }

        void _layoutRoot_MouseLeave(object sender, EventArgs e)
        {
            (this.FindName("FrameHide") as Storyboard).Begin();
        }

        void _layoutRoot_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (this.FindName("FrameShow") as Storyboard).Begin();
        }

        #region Mehods

        public void DimIn()
        {
            (this.FindName("DimRec") as Rectangle).Opacity = 0;
        }

        public void DimOut()
        {
            (this.FindName("DimRec") as Rectangle).Opacity = 0.6;
        }

        // The resource name used to initialize the actual object
        protected override string ResourceName
        {
            get { return "ThumSlice.xaml"; }
        }

        public override string ToString()
        {
            return "ThumSlice #" + _id;
        }

        public int ID
        {
            get { return _id; }
        }

        #endregion
    }
}