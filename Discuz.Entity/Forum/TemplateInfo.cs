using System;

namespace Discuz.Entity
{
	/// <summary>
	/// TemplateInfo 的摘要说明。
	/// </summary>
    [Serializable]
	public class TemplateInfo
	{
		private int m_templateid;	//模板id
		private string m_name;	//模板名称
		private string m_directory;	//模板所在目录
		private string m_copyright;	//模板版权声明
        private string m_templateurl = ""; //模板图片目录(可以是绝对或相对地址)

		///<summary>
		///模板id
		///</summary>
		public int Templateid
		{
			get { return m_templateid;}
			set { m_templateid = value;}
		}
		///<summary>
		///模板名称
		///</summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value;}
		}
		///<summary>
		///模板所在目录
		///</summary>
		public string Directory
		{
			get { return m_directory;}
			set { m_directory = value;}
		}
		///<summary>
		///模板版权声明
		///</summary>
		public string Copyright
		{
			get { return m_copyright;}
			set { m_copyright = value;}
		}

        ///<summary>
		///模板http_url绝对地址
		///</summary>
		public string Templateurl
        {
            get { return m_templateurl; }
            set { m_templateurl = value; }
		}
       
	}
}
