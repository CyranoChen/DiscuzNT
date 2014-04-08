using System;

namespace Discuz.Entity
{
	/// <summary>
	/// 管理组描述类
	/// </summary>
    [Serializable]
	public class AdminGroupInfo
	{

		private short m_admingid;			//管理组id
		private byte m_alloweditpost;		//允许编辑帖子
		private byte m_alloweditpoll;		//允许编辑投票
		private byte m_allowstickthread;	//允许置顶
		private byte m_allowmodpost;		//允许审核帖子
		private byte m_allowdelpost;		//允许删除帖子
		private byte m_allowmassprune;		//允许批量删除
		private byte m_allowrefund;			//允许强制退款(当主题被设置为收费阅读时有效)
		private byte m_allowcensorword;		//允许设置词语过滤
		private byte m_allowviewip;			//允许查看IP
		private byte m_allowbanip;			//允许禁止IP
		private byte m_allowedituser;		//允许编辑用户
		private byte m_allowmoduser;		//允许审核用户
		private byte m_allowbanuser;		//允许禁止用户
		private byte m_allowpostannounce;	//允许发布公告
		private byte m_allowviewlog;		//允许查看论坛运行记录
		private byte m_disablepostctrl;		//发帖不受审核、过滤、灌水等限制
        private int m_allowviewrealname = 0;    //是否允许查看用户实名

		///<summary>
		///管理组id 
		///</summary>
		public short Admingid
		{
			get { return m_admingid;}
			set { m_admingid = value;}
		}
		///<summary>
		///允许编辑帖子
		///</summary>
		public byte Alloweditpost
		{
			get { return m_alloweditpost;}
			set { m_alloweditpost = value;}
		}
		///<summary>
		///允许编辑投票
		///</summary>
		public byte Alloweditpoll
		{
			get { return m_alloweditpoll;}
			set { m_alloweditpoll = value;}
		}
		///<summary>
		///允许置顶
		///</summary>
		public byte Allowstickthread
		{
			get { return m_allowstickthread;}
			set { m_allowstickthread = value;}
		}
		///<summary>
		///允许审核帖子
		///</summary>
		public byte Allowmodpost
		{
			get { return m_allowmodpost;}
			set { m_allowmodpost = value;}
		}
		///<summary>
		///允许删除帖子
		///</summary>
		public byte Allowdelpost
		{
			get { return m_allowdelpost;}
			set { m_allowdelpost = value;}
		}
		///<summary>
		///允许批量删除
		///</summary>
		public byte Allowmassprune
		{
			get { return m_allowmassprune;}
			set { m_allowmassprune = value;}
		}
		///<summary>
		///允许强制退款(当主题被设置为收费阅读时有效)
		///</summary>
		public byte Allowrefund
		{
			get { return m_allowrefund;}
			set { m_allowrefund = value;}
		}
		///<summary>
		///允许设置词语过滤
		///</summary>
		public byte Allowcensorword
		{
			get { return m_allowcensorword;}
			set { m_allowcensorword = value;}
		}
		///<summary>
		///允许查看IP
		///</summary>
		public byte Allowviewip
		{
			get { return m_allowviewip;}
			set { m_allowviewip = value;}
		}
		///<summary>
		///允许禁止IP
		///</summary>
		public byte Allowbanip
		{
			get { return m_allowbanip;}
			set { m_allowbanip = value;}
		}
		///<summary>
		///允许编辑用户
		///</summary>
		public byte Allowedituser
		{
			get { return m_allowedituser;}
			set { m_allowedituser = value;}
		}
		///<summary>
		///允许审核用户
		///</summary>
		public byte Allowmoduser
		{
			get { return m_allowmoduser;}
			set { m_allowmoduser = value;}
		}
		///<summary>
		///允许禁止用户
		///</summary>
		public byte Allowbanuser
		{
			get { return m_allowbanuser;}
			set { m_allowbanuser = value;}
		}
		///<summary>
		///允许发布公告
		///</summary>
		public byte Allowpostannounce
		{
			get { return m_allowpostannounce;}
			set { m_allowpostannounce = value;}
		}
		///<summary>
		///允许查看论坛运行记录
		///</summary>
		public byte Allowviewlog
		{
			get { return m_allowviewlog;}
			set { m_allowviewlog = value;}
		}
		///<summary>
		///发帖不受审核、过滤、灌水等限制
		///</summary>
		public byte Disablepostctrl
		{
			get { return m_disablepostctrl;}
			set { m_disablepostctrl = value;}
		}

        /// <summary>
        /// 是否允许查看用户实名
        /// </summary>
        public int Allowviewrealname
        {
            get { return m_allowviewrealname; }
            set { m_allowviewrealname = value; }
        }
	}
}
