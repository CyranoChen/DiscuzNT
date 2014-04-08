using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DiscuzAlbum
{
    public class PhotoItem
    {
        #region Properties
        private string _title;
        private string _author;
        private string _image;
        private string _thum;

        #endregion

        #region Constructor
        public PhotoItem()
        { 
            // Nothing to initial.
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get or set the title text image of this photo.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Get or set the author text image of this photo.
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// Get or set the image(uri) of this photo.
        /// </summary>
        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Get or set the thum(uri) of this photo.
        /// </summary>
        public string Thum
        {
            get { return _thum; }
            set { _thum = value; }
        }

        /// <summary>
        /// Override base ToString() method.
        /// </summary>
        /// <returns>Image path info string.</returns>
        public override string ToString()
        {
            return "PhotoItem | " + _image;
        }

        #endregion
    }
}
