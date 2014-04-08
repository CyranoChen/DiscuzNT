using System;

namespace Discuz.Common
{
	/// <summary>
	/// SmiliesInfo ��ժҪ˵����
	/// </summary>
    [Serializable]
	public class SmiliesInfo
	{
		private int m_id;	//Smilies��id
		private int m_displayorder;	//��ʾ˳��
		private int m_type;	//����,���������ͼ��
		private string m_code;	//����
		private string m_url;	//��ӦͼƬURL

		///<summary>
		///Smilies��id
		///</summary>
		public int Id
		{
			get { return m_id;}
			set { m_id = value;}
		}
		///<summary>
		///��ʾ˳��
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///����,���������ͼ��
		///</summary>
		public int Type
		{
			get { return m_type;}
			set { m_type = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Code
		{
			get { return m_code;}
			set { m_code = value;}
		}
		///<summary>
		///��ӦͼƬURL
		///</summary>
		public string Url
		{
			get { return m_url;}
			set { m_url = value;}
		}
	}
}
