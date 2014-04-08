using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;

namespace Discuz.Entity
{
	/// <summary>
	/// 短消息信息描述类
	/// </summary>
	public class PrivateMessageInfo
	{

		private int m_pmid;	//短消息ID
		private string m_msgfrom;	//发送人姓名
		private int m_msgfromid;	//发件人UID
		private string m_msgto;	//收送人姓名
		private int m_msgtoid;	//收件人UID
		private int m_folder;	//文件箱
		private int m_new = 0;	//是否未读
		private string m_subject;	//标题
		private string m_postdatetime;	//发送时间
		private string m_message;	//短消息内容

		///<summary>
		///短消息ID
		///</summary>
		public int Pmid
		{
			get { return m_pmid;}
			set { m_pmid = value;}
		}
		///<summary>
		///发送人姓名
		///</summary>
		public string Msgfrom
		{
			get { return m_msgfrom;}
			set { m_msgfrom = value;}
		}
		///<summary>
		///发件人UID
		///</summary>
		public int Msgfromid
		{
			get { return m_msgfromid;}
			set { m_msgfromid = value;}
		}

		///<summary>
		///收件人姓名
		///</summary>
		public string Msgto
		{
			get { return m_msgto;}
			set { m_msgto = value;}
		}

		///<summary>
		///收件人UID
		///</summary>
		public int Msgtoid
		{
			get { return m_msgtoid;}
			set { m_msgtoid = value;}
		}
		///<summary>
        ///文件箱(0:收件箱,1:发件箱,2:草稿箱)
		///</summary>
		public int Folder
		{
			get { return m_folder;}
			set { m_folder = value;}
		}
		///<summary>
		///是否未读
		///</summary>
		public int New
		{
			get { return m_new;}
			set { m_new = value;}
		}
		///<summary>
		///标题
		///</summary>
		public string Subject
		{
			get { return m_subject;}
			set { m_subject = value;}
		}
		///<summary>
		///发送时间
		///</summary>
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///短消息内容
		///</summary>
		public string Message
		{
			get { return m_message;}
			set { m_message = value;}
		}

	}
}
