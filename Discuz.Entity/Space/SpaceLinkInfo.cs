using System;

namespace Discuz.Entity
{
	/// <summary>
	/// SpaceLinkInfo 的摘要说明。
	/// </summary>
	public class SpaceLinkInfo
	{
		public SpaceLinkInfo()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
        /// <summary>
        /// 连接ID
        /// </summary>
		private int _linkid;
		public int LinkId
		{
			get { return _linkid; }
			set { _linkid = value; }
		}
        /// <summary>
        /// 用户ID
        /// </summary>
		private int _userid;
		public int UserId
		{
			get { return _userid; }
			set { _userid = value; }
		}
        /// <summary>
        /// 名称
        /// </summary>
		private string _linktitle;
		public string LinkTitle
		{
			get { return _linktitle; }
			set { _linktitle = value; }
		}
        /// <summary>
        /// 链接地址
        /// </summary>
		private string _linkurl;
		public string LinkUrl
		{
			get { return _linkurl; }
			set { _linkurl = value; }
		}
        /// <summary>
        /// 链接介绍
        /// </summary>
		private string _description;
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
	}
}
