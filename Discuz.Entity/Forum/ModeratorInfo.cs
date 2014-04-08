using System;

namespace Discuz.Entity
{
	/// <summary>
	/// ������Ϣ������
	/// </summary>
    [Serializable]
	public class ModeratorInfo
	{

		private int m_uid;	//�û�Uid
		private int m_fid;	//��̳Fid
		private int m_displayorder;	//������ʾ˳��
		private int m_inherited;	//�Ƿ�̳ж���

		///<summary>
		///�û�Uid
		///</summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///��̳Fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///������ʾ˳��
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///�Ƿ�̳ж���
		///</summary>
		public int Inherited
		{
			get { return m_inherited;}
			set { m_inherited = value;}
		}
	}
}
