using System;

namespace Discuz.Entity
{
    /// <summary>
    /// SpaceCategoryInfo 的摘要说明。
    /// </summary>
    public class SpacePostCategoryInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 日志ID
        /// </summary>
        private int _postid;
        public int PostID
        {
            get { return _postid; }
            set { _postid = value; }
        }

        /// <summary>
        /// 日志分类ID
        /// </summary>
        private int _categoryid;
        public int CategoryID
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }


    }
}
