using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ����������Ϣ������
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
		/// �������
		/// </summary>
		public int Searchid
		{
			get { return m_searchid;}
			set { m_searchid = value;}
		}
		/// <summary>
		/// �����ؼ���
		/// </summary>
		public string Keywords
		{
			get { return m_keywords;}
			set { m_keywords = value;}
		}
		/// <summary>
		/// SQL��ѯ���
		/// </summary>
		public string Searchstring
		{
			get { return m_searchstring;}
			set { m_searchstring = value;}
		}
		/// <summary>
		/// ������IP
		/// </summary>
		public string Ip
		{
			get { return m_ip;}
			set { m_ip = value;}
		}
		/// <summary>
		/// ������id
		/// </summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		/// <summary>
		/// �������û���
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
