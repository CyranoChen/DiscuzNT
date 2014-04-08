using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 图片聚合信息类
    /// </summary>
    public class PhotoAggregationInfo
    {
        #region 类属性信息


        private int _focusphotoshowtype = 0; //焦点图片的显示类型
        private int _focusphotodays = 7; //焦点图片的显示有效期
        private int _focusphotocount = 8; //焦点图片的显示数目
        private int _focusalbumshowtype = 0; //焦点相册的显示类型
        private int _focusalbumdays = 7; //焦点相册的显示有效期
        private int _focusalbumcount = 8; //焦点相册的显示数目
        private int _weekhot = 10;  //一周热门图片的显示数目

        /// <summary>
        /// 焦点图片的显示类型
        /// </summary>
        public int Focusphotoshowtype
        {
            get { return _focusphotoshowtype; }
            set { _focusphotoshowtype = value; }
        }
        
        /// <summary>
        /// 焦点图片的显示有效期
        /// </summary>
        public int Focusphotodays
        {
            get { return _focusphotodays; }
            set { _focusphotodays = value; }
        }
      
        /// <summary>
        /// 焦点图片的显示数目
        /// </summary>
        public int Focusphotocount
        {
            get { return _focusphotocount; }
            set { _focusphotocount = value; }
        }

      
        /// <summary>
        /// 焦点相册的显示类型
        /// </summary>
        public int Focusalbumshowtype
        {
            get { return _focusalbumshowtype; }
            set { _focusalbumshowtype = value; }
        }
       
        /// <summary>
        /// 焦点相册的显示有效期
        /// </summary>
        public int Focusalbumdays
        {
            get { return _focusalbumdays; }
            set { _focusalbumdays = value; }
        }
      

        /// <summary>
        /// 焦点相册的显示数目
        /// </summary>
        public int Focusalbumcount
        {
            get { return _focusalbumcount; }
            set { _focusalbumcount = value; }
        }
      
        
        /// <summary>
        /// 一周热门图片的显示数目
        /// </summary>
        public int Weekhot
        {
            get { return _weekhot; }
            set { _weekhot = value; }
        }
       
        #endregion
    }
}
