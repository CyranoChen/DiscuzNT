using System;

namespace Discuz.Entity
{
	/// <summary>
	/// IndexPageForumInfo ��ժҪ˵����
	/// </summary>
	public class IndexPageForumInfo : ForumInfo
	{
        private string _havenew;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public string Havenew
        {
            get { return _havenew; }
            set { _havenew = value; }
        }

        private string _collapse = string.Empty;
        /// <summary>
        /// �Ƿ�����(����������'display: none;')
        /// </summary>
        public string Collapse
        {
            get { return _collapse; }
            set { _collapse = value; }
        }
    }
}
