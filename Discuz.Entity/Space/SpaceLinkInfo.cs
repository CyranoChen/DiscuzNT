using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpaceLinkInfo ��ժҪ˵����
	/// </summary>
	public class SpaceLinkInfo
	{
		public SpaceLinkInfo()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}
        /// <summary>
        /// ����ID
        /// </summary>
		private int _linkid;
		public int LinkId
		{
			get { return _linkid; }
			set { _linkid = value; }
		}
        /// <summary>
        /// �û�ID
        /// </summary>
		private int _userid;
		public int UserId
		{
			get { return _userid; }
			set { _userid = value; }
		}
        /// <summary>
        /// ����
        /// </summary>
		private string _linktitle;
		public string LinkTitle
		{
			get { return _linktitle; }
			set { _linktitle = value; }
		}
        /// <summary>
        /// ���ӵ�ַ
        /// </summary>
		private string _linkurl;
		public string LinkUrl
		{
			get { return _linkurl; }
			set { _linkurl = value; }
		}
        /// <summary>
        /// ���ӽ���
        /// </summary>
		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
	}
}
