using System;

namespace Discuz.Entity
{
	/// <summary>
	/// CustomEditorButtonInfo ��ժҪ˵����
	/// </summary>
    [Serializable]
	public class CustomEditorButtonInfo
	{
		private int m_id;	//�Զ����
		private int m_available;	//�Ƿ���Ч
		private string m_tag;	//Discuz!NT�����ǩ����
		private string m_icon;	//ͼ���ַ
		private string m_replacement;	//�滻����
		private string m_example;	//����(������ʾ�ڰ�����)
		private string m_explanation;	//����(������ʾ�ڰ�����)
		private int m_params;	//��������
		private int m_nest;	//������
		private string m_paramsdescript;	//������Ӧ������
		private string m_paramsdefvalue;	//������Ӧ��Ĭ��ֵ

		///<summary>
		///�Զ����
		///</summary>
		public int Id
		{
			get { return m_id;}
			set { m_id = value;}
		}
		///<summary>
		///�Ƿ���Ч
		///</summary>
		public int Available
		{
			get { return m_available;}
			set { m_available = value;}
		}
		///<summary>
		///Discuz!NT�����ǩ����
		///</summary>
		public string Tag
		{
			get { return m_tag;}
			set { m_tag = value;}
		}
		///<summary>
		///ͼ���ַ
		///</summary>
		public string Icon
		{
			get { return m_icon;}
			set { m_icon = value;}
		}
		///<summary>
		///�滻����
		///</summary>
		public string Replacement
		{
			get { return m_replacement;}
			set { m_replacement = value;}
		}
		///<summary>
		///����(������ʾ�ڰ�����)
		///</summary>
		public string Example
		{
			get { return m_example;}
			set { m_example = value;}
		}
		///<summary>
		///����(������ʾ�ڰ�����)
		///</summary>
		public string Explanation
		{
			get { return m_explanation;}
			set { m_explanation = value;}
		}
		///<summary>
		///��������
		///</summary>
		public int Params
		{
			get { return m_params;}
			set { m_params = value;}
		}
		///<summary>
		///������
		///</summary>
		public int Nest
		{
			get { return m_nest;}
			set { m_nest = value;}
		}
		///<summary>
		///������Ӧ������
		///</summary>
		public string Paramsdescript
		{
			get { return m_paramsdescript;}
			set { m_paramsdescript = value;}
		}
		///<summary>
		///������Ӧ��Ĭ��ֵ
		///</summary>
		public string Paramsdefvalue
		{
			get { return m_paramsdefvalue;}
			set { m_paramsdefvalue = value;}
		}
	}
}
