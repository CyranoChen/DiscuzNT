using System;
using Discuz.Common;

namespace Discuz.Config
{

	/// <summary>
    /// 空间开通配置信息类
	/// </summary>
	public class SpaceActiveConfigInfo : IConfigInfo
    {
        #region 私有字段
        private string m_allowPostcount = "0"; //允许通过回复数开通个人空间
        private string m_postcount = "0"; //回复超过N条后开通
        private string m_allowDigestcount = "0"; //允许通过精华数开通个人空间
        private string m_digestcount = "0"; //精华超过N条后开通 
        private string m_allowScore = "0";  //允许通过积分开通个人空间
        private string m_score = "0"; //积分超过N后开通
		private string m_allowUsergroups = "1"; //允许通过用户组来开通个人空间
		private string m_usergroups = "1,2,3,10,11,12,13,14,15"; //允许开通个人空间的用户组ID列表
		private string m_activeType = "1";//开通方式，0手动开通，1自动开通
        private string m_spacefooterinfo = ""; //空间页面底部相关显示信息
        private string m_spacegreeting = "欢迎开通个人空间";//个人空间开通时的欢迎辞
        private int m_enablespacerewrite = 0;//是否启用个人空间Rewrite支持   
        #endregion

        #region 属性
        public string AllowPostcount
		{
			get { return m_allowPostcount;}
			set { m_allowPostcount = value;}
		}

		public string Postcount
		{
			get { return m_postcount;}
			set { m_postcount = value;}
		}

		public string AllowDigestcount
		{
			get { return m_allowDigestcount;}
			set { m_allowDigestcount = value;}
		}

		public string Digestcount
		{
			get { return m_digestcount;}
			set { m_digestcount = value;}
		}

		public string AllowScore
		{
			get { return m_allowScore;}
			set { m_allowScore = value;}
		}

		public string Score
		{
			get { return m_score;}
			set { m_score = value;}
		}

		public string AllowUsergroups
		{
			get { return m_allowUsergroups;}
			set { m_allowUsergroups = value;}
		}

		public string Usergroups
		{
			get { return m_usergroups;}
			set { m_usergroups = value;}
		}

		public string ActiveType
		{
			get { return m_activeType; }
			set { m_activeType = value; }
		}

        /// <summary>
        /// 空间页面底部相关显示信息
        /// </summary>
        public string SpaceFooterInfo
        {
            get
            {
                return Utils.HtmlDecode(m_spacefooterinfo);
            }
            set
            {
                m_spacefooterinfo = Utils.HtmlEncode(value);
            }
        }

        /// <summary>
        /// 个人空间开通时的欢迎辞
        /// </summary>
        public string Spacegreeting
        {
            get { return Utils.HtmlDecode(m_spacegreeting); }
            set { m_spacegreeting = Utils.HtmlEncode(value); }
        }

        /// <summary>
        /// 是否启用个人空间Rewrite支持  
        /// </summary>
        public int Enablespacerewrite
        {
            get { return m_enablespacerewrite; }
            set { m_enablespacerewrite = value; }
        }
        #endregion
    }

    /// <summary>
    /// 空间开通方式
    /// </summary>
	public enum SpaceActiveType
	{
		ManualActive = 0, //手工开通
		AutoActive = 1    //自动开通
	}
}