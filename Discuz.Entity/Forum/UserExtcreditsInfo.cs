using System;

namespace Discuz.Entity
{
	/// <summary>
	/// UserExtcreditsInfo 的摘要说明。
	/// </summary>
	public class UserExtcreditsInfo
	{
    	private string m_name; //积分名称
		private string m_unit; //积分单位
		private float m_rate; //兑换比率
		private float m_init; //注册初始积分
		private float m_topic; //发主题
		private float m_reply; //回复
		private float m_digest; //加精华
		private float m_upload; //上传附件
		private float m_download; //下载附件
		private float m_pm; //发短消息
		private float m_search; //搜索
		private float m_pay; //交易成功
		private float m_vote; //参与投票

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
		/// 积分名称
		/// </summary>
		public string Name
		{
			get { return m_name;}
			set { m_name = value;}
		}

		/// <summary>
		/// 积分单位
		/// </summary>
		public string Unit
		{
			get { return m_unit;}
			set { m_unit = value;}
		}

		/// <summary>
		/// 兑换比率
		/// </summary>
		public float Rate
		{
			get { return m_rate;}
			set { m_rate = value;}
		}

		/// <summary>
		/// 注册初始积分
		/// </summary>
		public float Init
		{
			get { return m_init;}
			set { m_init = value;}
		}

		/// <summary>
		/// 发主题
		/// </summary>
		public float Topic
		{
			get { return m_topic;}
			set { m_topic = value;}
		}

		/// <summary>
		/// 回复
		/// </summary>
		public float Reply
		{
			get { return m_reply;}
			set { m_reply = value;}
		}

		/// <summary>
		/// 加精华
		/// </summary>
		public float Digest
		{
			get { return m_digest;}
			set { m_digest = value;}
		}

		/// <summary>
		/// 上传附件
		/// </summary>
		public float Upload
		{
			get { return m_upload;}
			set { m_upload = value;}
		}

		/// <summary>
		/// 下载附件
		/// </summary>
		public float Download
		{
			get { return m_download;}
			set { m_download = value;}
		}

		/// <summary>
		/// 发短消息
		/// </summary>
		public float Pm
		{
			get { return m_pm;}
			set { m_pm = value;}
		}

		/// <summary>
		/// 搜索
		/// </summary>
		public float Search
		{
			get { return m_search;}
			set { m_search = value;}
		}

		/// <summary>
		/// 交易成功
		/// </summary>
		public float Pay
		{
			get { return m_pay;}
			set { m_pay = value;}
		}

		/// <summary>
		/// 参与投票
		/// </summary>
		public float Vote
		{
			get { return m_vote;}
			set { m_vote = value;}
		}


	}

}
