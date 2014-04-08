using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ������������
	/// </summary>
    [Serializable]
	public class AdminGroupInfo
	{

		private short m_admingid;			//������id
		private byte m_alloweditpost;		//����༭����
		private byte m_alloweditpoll;		//����༭ͶƱ
		private byte m_allowstickthread;	//�����ö�
		private byte m_allowmodpost;		//�����������
		private byte m_allowdelpost;		//����ɾ������
		private byte m_allowmassprune;		//��������ɾ��
		private byte m_allowrefund;			//����ǿ���˿�(�����ⱻ����Ϊ�շ��Ķ�ʱ��Ч)
		private byte m_allowcensorword;		//�������ô������
		private byte m_allowviewip;			//����鿴IP
		private byte m_allowbanip;			//�����ֹIP
		private byte m_allowedituser;		//����༭�û�
		private byte m_allowmoduser;		//��������û�
		private byte m_allowbanuser;		//�����ֹ�û�
		private byte m_allowpostannounce;	//����������
		private byte m_allowviewlog;		//����鿴��̳���м�¼
		private byte m_disablepostctrl;		//����������ˡ����ˡ���ˮ������
        private int m_allowviewrealname = 0;    //�Ƿ�����鿴�û�ʵ��

		///<summary>
		///������id 
		///</summary>
		public short Admingid
		{
			get { return m_admingid;}
			set { m_admingid = value;}
		}
		///<summary>
		///����༭����
		///</summary>
		public byte Alloweditpost
		{
			get { return m_alloweditpost;}
			set { m_alloweditpost = value;}
		}
		///<summary>
		///����༭ͶƱ
		///</summary>
		public byte Alloweditpoll
		{
			get { return m_alloweditpoll;}
			set { m_alloweditpoll = value;}
		}
		///<summary>
		///�����ö�
		///</summary>
		public byte Allowstickthread
		{
			get { return m_allowstickthread;}
			set { m_allowstickthread = value;}
		}
		///<summary>
		///�����������
		///</summary>
		public byte Allowmodpost
		{
			get { return m_allowmodpost;}
			set { m_allowmodpost = value;}
		}
		///<summary>
		///����ɾ������
		///</summary>
		public byte Allowdelpost
		{
			get { return m_allowdelpost;}
			set { m_allowdelpost = value;}
		}
		///<summary>
		///��������ɾ��
		///</summary>
		public byte Allowmassprune
		{
			get { return m_allowmassprune;}
			set { m_allowmassprune = value;}
		}
		///<summary>
		///����ǿ���˿�(�����ⱻ����Ϊ�շ��Ķ�ʱ��Ч)
		///</summary>
		public byte Allowrefund
		{
			get { return m_allowrefund;}
			set { m_allowrefund = value;}
		}
		///<summary>
		///�������ô������
		///</summary>
		public byte Allowcensorword
		{
			get { return m_allowcensorword;}
			set { m_allowcensorword = value;}
		}
		///<summary>
		///����鿴IP
		///</summary>
		public byte Allowviewip
		{
			get { return m_allowviewip;}
			set { m_allowviewip = value;}
		}
		///<summary>
		///�����ֹIP
		///</summary>
		public byte Allowbanip
		{
			get { return m_allowbanip;}
			set { m_allowbanip = value;}
		}
		///<summary>
		///����༭�û�
		///</summary>
		public byte Allowedituser
		{
			get { return m_allowedituser;}
			set { m_allowedituser = value;}
		}
		///<summary>
		///��������û�
		///</summary>
		public byte Allowmoduser
		{
			get { return m_allowmoduser;}
			set { m_allowmoduser = value;}
		}
		///<summary>
		///�����ֹ�û�
		///</summary>
		public byte Allowbanuser
		{
			get { return m_allowbanuser;}
			set { m_allowbanuser = value;}
		}
		///<summary>
		///����������
		///</summary>
		public byte Allowpostannounce
		{
			get { return m_allowpostannounce;}
			set { m_allowpostannounce = value;}
		}
		///<summary>
		///����鿴��̳���м�¼
		///</summary>
		public byte Allowviewlog
		{
			get { return m_allowviewlog;}
			set { m_allowviewlog = value;}
		}
		///<summary>
		///����������ˡ����ˡ���ˮ������
		///</summary>
		public byte Disablepostctrl
		{
			get { return m_disablepostctrl;}
			set { m_disablepostctrl = value;}
		}

        /// <summary>
        /// �Ƿ�����鿴�û�ʵ��
        /// </summary>
        public int Allowviewrealname
        {
            get { return m_allowviewrealname; }
            set { m_allowviewrealname = value; }
        }
	}
}
