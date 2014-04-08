using System;


namespace Discuz.Entity
{
	/// <summary>
	/// 版块描述类
	/// </summary>
    [Serializable]
	public class SimpleForumInfo
	{
		private int m_fid;	//论坛fid
		private string m_name = "";	//论坛名称
        private string m_url; //论坛url
        private int m_postbytopictype;	//发帖必须归类
        private string m_topictypes;	//主题分类
		///<summary>
		///论坛fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///论坛名称
		///</summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value.Trim();}
		}
		///<summary>
		///URL
		///</summary>
		public string Url
		{
			get { return m_url;}
			set { m_url = value;}
		}
        ///<summary>
        ///发帖必须归类
        ///</summary>
        public int Postbytopictype
        {
            get { return m_postbytopictype; }
            set { m_postbytopictype = value; }
        }
        ///<summary>
        ///主题分类
        ///</summary>
        public string Topictypes
        {
            get { return m_topictypes; }
            set { m_topictypes = value; }
        }
	}
}
