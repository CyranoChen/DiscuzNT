using System;

namespace Discuz.Entity
{
	/// <summary>
	/// UserExtcreditsInfo ��ժҪ˵����
	/// </summary>
	public class UserExtcreditsInfo
	{
    	private string m_name; //��������
		private string m_unit; //���ֵ�λ
		private float m_rate; //�һ�����
		private float m_init; //ע���ʼ����
		private float m_topic; //������
		private float m_reply; //�ظ�
		private float m_digest; //�Ӿ���
		private float m_upload; //�ϴ�����
		private float m_download; //���ظ���
		private float m_pm; //������Ϣ
		private float m_search; //����
		private float m_pay; //���׳ɹ�
		private float m_vote; //����ͶƱ

		public UserExtcreditsInfo()
		{
            m_name="";
			m_unit="";
			m_rate=0;
			m_init=0;
			m_topic=0;
			m_reply=0;
			m_digest=0;
			m_upload=0;
			m_download=0;
			m_pm=0;
			m_search=0;
			m_pay=0;
			m_vote=0;
		}

		/// <summary>
		/// ��������
		/// </summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value;}
		}

		/// <summary>
		/// ���ֵ�λ
		/// </summary>
		public string Unit
		{
			get { return m_unit;}
			set { m_unit = value;}
		}

		/// <summary>
		/// �һ�����
		/// </summary>
		public float Rate
		{
			get { return m_rate;}
			set { m_rate = value;}
		}

		/// <summary>
		/// ע���ʼ����
		/// </summary>
		public float Init
		{
			get { return m_init;}
			set { m_init = value;}
		}

		/// <summary>
		/// ������
		/// </summary>
		public float Topic
		{
			get { return m_topic;}
			set { m_topic = value;}
		}

		/// <summary>
		/// �ظ�
		/// </summary>
		public float Reply
		{
			get { return m_reply;}
			set { m_reply = value;}
		}

		/// <summary>
		/// �Ӿ���
		/// </summary>
		public float Digest
		{
			get { return m_digest;}
			set { m_digest = value;}
		}

		/// <summary>
		/// �ϴ�����
		/// </summary>
		public float Upload
		{
			get { return m_upload;}
			set { m_upload = value;}
		}

		/// <summary>
		/// ���ظ���
		/// </summary>
		public float Download
		{
			get { return m_download;}
			set { m_download = value;}
		}

		/// <summary>
		/// ������Ϣ
		/// </summary>
		public float Pm
		{
			get { return m_pm;}
			set { m_pm = value;}
		}

		/// <summary>
		/// ����
		/// </summary>
		public float Search
		{
			get { return m_search;}
			set { m_search = value;}
		}

		/// <summary>
		/// ���׳ɹ�
		/// </summary>
		public float Pay
		{
			get { return m_pay;}
			set { m_pay = value;}
		}

		/// <summary>
		/// ����ͶƱ
		/// </summary>
		public float Vote
		{
			get { return m_vote;}
			set { m_vote = value;}
		}


	}

}
