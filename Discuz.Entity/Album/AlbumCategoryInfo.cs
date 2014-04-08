using System;

namespace Discuz.Entity
{
	/// <summary>
	/// AlbumCategoryInfo 的摘要说明。
	/// </summary>
	public class AlbumCategoryInfo
	{
		public AlbumCategoryInfo()
		{

		}
        private int albumcateid = 0;
        private string title = string.Empty;
        private string description = string.Empty;
        private int albumcount = 0;
        private int displayorder = 0;

        /// <summary>
        /// 相册分类id
        /// </summary>
        public int Albumcateid
        {
            get
            {
                return albumcateid;
            }
            set
            {
                albumcateid = value;
            }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Title
        {
            get
            {
                return title.Trim();
            }
            set
            {
                title = value;
            }
        }
        /// <summary>
        /// 分类描述
        /// </summary>
        public string Description
        {
            get
            {
                return description.Trim();
            }
            set
            {
                description = value;
            }
        }
        /// <summary>
        /// 相册数量
        /// </summary>
        public int Albumcount
        {
            get
            {
                return albumcount;
            }
            set
            {
                albumcount = value;
            }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int Displayorder
        {
            get
            {
                return displayorder;
            }
            set
            {
                displayorder = value;
            }
        }


    }
}
