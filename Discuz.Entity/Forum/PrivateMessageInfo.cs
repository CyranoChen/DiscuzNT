using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;

namespace Discuz.Entity
{
	/// <summary>
	/// ����Ϣ��Ϣ������
	/// </summary>
	public class PrivateMessageInfo
	{

		private int m_pmid;	//����ϢID
		private string m_msgfrom;	//����������
		private int m_msgfromid;	//������UID
		private string m_msgto;	//����������
		private int m_msgtoid;	//�ռ���UID
		private int m_folder;	//�ļ���
		private int m_new = 0;	//�Ƿ�δ��
		private string m_subject;	//����
		private string m_postdatetime;	//����ʱ��
		private string m_message;	//����Ϣ����

		///<summary>
		///����ϢID
		///</summary>
		public int Pmid
		{
			get { return m_pmid;}
			set { m_pmid = value;}
		}
		///<summary>
		///����������
		///</summary>
		public string Msgfrom
		{
			get { return m_msgfrom;}
			set { m_msgfrom = value;}
		}
		///<summary>
		///������UID
		///</summary>
		public int Msgfromid
		{
			get { return m_msgfromid;}
			set { m_msgfromid = value;}
		}

		///<summary>
		///�ռ�������
		///</summary>
		public string Msgto
		{
			get { return m_msgto;}
			set { m_msgto = value;}
		}

		///<summary>
		///�ռ���UID
		///</summary>
		public int Msgtoid
		{
			get { return m_msgtoid;}
			set { m_msgtoid = value;}
		}
		///<summary>
        ///�ļ���(0:�ռ���,1:������,2:�ݸ���)
		///</summary>
		public int Folder
		{
			get { return m_folder;}
			set { m_folder = value;}
		}
		///<summary>
		///�Ƿ�δ��
		///</summary>
		public int New
		{
			get { return m_new;}
			set { m_new = value;}
		}
		///<summary>
		///����
		///</summary>
		public string Subject
		{
			get { return m_subject;}
			set { m_subject = value;}
		}
		///<summary>
		///����ʱ��
		///</summary>
		public string Postdatetime
		{
			get { return m_postdatetime;}
			set { m_postdatetime = value;}
		}
		///<summary>
		///����Ϣ����
		///</summary>
		public string Message
		{
			get { return m_message;}
			set { m_message = value;}
		}

	}
}
