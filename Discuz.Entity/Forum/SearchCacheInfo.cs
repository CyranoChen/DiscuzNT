using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ËÑË÷»º´æĞÅÏ¢ÃèÊöÀà
	/// </summary>
	public class SearchCacheInfo
	{
		
		private int m_searchid;
		private string m_keywords;
		private string m_searchstring;
		private string m_ip;
		private int m_uid;
		private int m_groupid;
		private string m_postdatetime;
		private string m_expiration;
		private int m_topics;
		private string m_tids;

		/// <summary>
		/// ËÑË÷ĞòºÅ
		/// </summary>
		public int Searchid
		{
			get { return m_searchid;}
			set { m_searchid = value;}
		}
		/// <summary>
		/// ËÑË÷¹Ø¼ü´Ê
		/// </summary>
		public string Keywords
		{
			get { return m_keywords;}
			set { m_keywords = value;}
		}
		/// <summary>
		/// SQL²éÑ¯Óï¾ä
		/// </summary>
		public string Searchstring
		{
			get { return m_searchstring;}
			set { m_searchstring = value;}
		}
		/// <summary>
		/// ËÑË÷ÕßIP
		/// </summary>
		public string Ip
		{
			get { return m_ip;}
			set { m_ip = value;}
		}
		/// <summary>
		/// ËÑË÷Õßid
		/// </summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		/// <summary>
		/// ËÑË÷ÕßÓÃ»§×é
		/// </summary>
		public int Groupid
		{
			get { return m_groupid;}
			set { m_groupid = value;}
		}
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		public string Expiration
		{
			get { return m_expiration;}
			set { m_expiration = value;}
		}
		public int Topics
		{
			get { return m_topics;}
			set { m_topics = value;}
		}
		public string Tids
		{
			get { return m_tids;}
			set { m_tids = value;}
		}
	}
}
