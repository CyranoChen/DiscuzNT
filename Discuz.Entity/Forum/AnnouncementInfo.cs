using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 公告信息描述类
	/// </summary>
	public class AnnouncementInfo
	{

		private int m_id;	//公告id
		private string m_poster;	//公告发布者用户名
		private int m_posterid;	//发布者用户id
		private string m_title;	//公告标题
		private int m_displayorder;	//显示顺序
		private DateTime m_starttime;	//起始时间
		private DateTime m_endtime;	//结束时间
		private string m_message;	//公告内容

		///<summary>
		///公告id
		///</summary>
		public int Id
		{
			get { return m_id;}
			set { m_id = value;}
		}
		///<summary>
		///公告发布者用户名
		///</summary>
		public string Poster
		{
			get { return m_poster;}
			set { m_poster = value;}
		}
		///<summary>
		///发布者用户id
		///</summary>
		public int Posterid
		{
			get { return m_posterid;}
			set { m_posterid = value;}
		}
		///<summary>
		///公告标题
		///</summary>
		public string Title
		{
			get { return m_title;}
			set { m_title = value;}
		}
		///<summary>
		///显示顺序
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///起始时间
		///</summary>
		public DateTime Starttime
		{
			get { return m_starttime;}
			set { m_starttime = value;}
		}
		///<summary>
		///结束时间
		///</summary>
		public DateTime Endtime
		{
			get { return m_endtime;}
			set { m_endtime = value;}
		}
		///<summary>
		///公告内容
		///</summary>
		public string Message
		{
			get { return m_message;}
			set { m_message = value;}
		}
	}
}
