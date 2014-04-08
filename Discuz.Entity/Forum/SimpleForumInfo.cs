using System;


namespace Discuz.Entity
{
	/// <summary>
	/// ���������
	/// </summary>
    [Serializable]
	public class SimpleForumInfo
	{
		private int m_fid;	//��̳fid
		private string m_name = "";	//��̳����
        private string m_url; //��̳url
        private int m_postbytopictype;	//�����������
        private string m_topictypes;	//�������
		///<summary>
		///��̳fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///��̳����
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
        ///�����������
        ///</summary>
        public int Postbytopictype
        {
            get { return m_postbytopictype; }
            set { m_postbytopictype = value; }
        }
        ///<summary>
        ///�������
        ///</summary>
        public string Topictypes
        {
            get { return m_topictypes; }
            set { m_topictypes = value; }
        }
	}
}
