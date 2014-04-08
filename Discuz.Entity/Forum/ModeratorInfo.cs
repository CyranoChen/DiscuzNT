using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 版主信息描述类
	/// </summary>
    [Serializable]
	public class ModeratorInfo
	{

		private int m_uid;	//用户Uid
		private int m_fid;	//论坛Fid
		private int m_displayorder;	//版主显示顺序
		private int m_inherited;	//是否继承而来

		///<summary>
		///用户Uid
		///</summary>
		public int Uid
		{
			get { return m_uid;}
			set { m_uid = value;}
		}
		///<summary>
		///论坛Fid
		///</summary>
		public int Fid
		{
			get { return m_fid;}
			set { m_fid = value;}
		}
		///<summary>
		///版主显示顺序
		///</summary>
		public int Displayorder
		{
			get { return m_displayorder;}
			set { m_displayorder = value;}
		}
		///<summary>
		///是否继承而来
		///</summary>
		public int Inherited
		{
			get { return m_inherited;}
			set { m_inherited = value;}
		}
	}
}
