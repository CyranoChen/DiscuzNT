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

namespace DiscuzAlbum
{
    public class PhotoDataSet
    {
        #region Properties
        private Collection<PhotoItem> _children;

        #endregion

        #region Constructor
        public PhotoDataSet()
        {
            _children = new Collection<PhotoItem>();
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get the children photo items collection of this data set instance.
        /// </summary>
        public Collection<PhotoItem> Children
        {
            get { return _children; }
        }

        /// <summary>
        /// Fill dummy data into this data set instance.
        /// </summary>
        public void FillDummy(PhotoData[] photoArray)
        {
            _children.Clear();
            
            
            foreach(PhotoData photo in photoArray)
            {
                PhotoItem loopItem = new PhotoItem();
                //int authorId = random.Next(1, 6);
                loopItem.Title = "../../cache/photo/" + (photo.photoid / 1000 + 1) + "/" + photo.photoid.ToString() + "_title.png";//"Assets/Photo/phototitle.png";
                loopItem.Author = "../../cache/user/" + (photo.userid / 1000 + 1) + "/" + photo.userid.ToString() + "_name.png"; //"Assets/Photo/photoauthor.png";;
                //int photoNum = random.Next(1, 14);
                //string insertStr = photoNum.ToString();
                //while (insertStr.Length < 2)
                //    insertStr = "0" + insertStr;
                loopItem.Image = (photo.image.EndsWith(".gif") ? "../../tools/imageconverter.aspx?u=" : string.Empty) + photo.image;//"Assets/Photo/Added/600_400_BIG/Big_image_" + i + ".jpg";
                loopItem.Thum = (photo.thumbnail.EndsWith(".gif") ? "../../tools/imageconverter.aspx?u=" : string.Empty) + photo.thumbnail;//"Assets/Photo/Added/600_400_small/Small_image_" + i + ".jpg";

                _children.Add(loopItem);
           
            }
            
            

            //Random random = new Random();
            //for (int i = 1; i <= 53; i++)
            //{
            //    PhotoItem loopItem = new PhotoItem();
            //    int authorId = random.Next(1, 6);
            //    loopItem.Title = "Assets/Photo/Added/Author_Title/Title_" + authorId + ".png";
            //    loopItem.Author = "Assets/Photo/Added/Author_Title/Author_" + authorId + ".png";
            //    //int photoNum = random.Next(1, 14);
            //    //string insertStr = photoNum.ToString();
            //    //while (insertStr.Length < 2)
            //    //    insertStr = "0" + insertStr;
            //    loopItem.Image = "Assets/Photo/Added/600_400_BIG/Big_image_" + i + ".jpg";
            //    loopItem.Thum = "Assets/Photo/Added/600_400_small/Small_image_" + i + ".jpg";

            //    _children.Add(loopItem);
            //}
        }


        #endregion

    }
}
