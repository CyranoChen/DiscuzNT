using System;

namespace Discuz.Entity
{
	/// <summary>
	/// IndexPageForumInfo 的摘要说明。
	/// </summary>
	public class IndexPageForumInfo : ForumInfo
	{
        private string _havenew;
        /// <summary>
        /// 是否是新主题
        /// </summary>
        public string Havenew
        {
            get { return _havenew; }
            set { _havenew = value; }
        }

        private string _collapse = string.Empty;
        /// <summary>
        /// 是否收起(如果是则输出'display: none;')
        /// </summary>
        public string Collapse
        {
            get { return _collapse; }
            set { _collapse = value; }
        }
    }
}
