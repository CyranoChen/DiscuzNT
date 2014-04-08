using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Config
{

    /// <summary>
    /// 论坛基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class InvitationConfigInfo : IConfigInfo
    {
        private int m_invitecodeexpiretime = 0;//邀请码使用期限
        private int m_invitecodemaxcount = 0;//邀请码最大使用次数
        private int m_invitecodeaddcreditsline = 0;//邀请多少用户之后开始加分
        private string m_invitecodeprice = "0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00";//邀请码价格定制
        private string m_invitationloginuserdescription = "";//邀请功能介绍(针对注册用户)
        private string m_invitationvisitordescription = "";//邀请功能介绍(针对游客)
        private string m_invitationemailmodel = "";//邮件邀请内容模板
        private int m_invitecodeusermaxbuy = 25;//用户最多可以购买多少个邀请码
        private int m_invitecodeusercreateperday = 5;//用户每天最多可以获得多少个开放式邀请码

        /// <summary>
        /// 邀请码使用期限(天)
        /// </summary>
        public int InviteCodeExpireTime
        {
            get { return m_invitecodeexpiretime; }
            set { m_invitecodeexpiretime = value; }
        }

        /// <summary>
        /// 邀请码最大使用次数(次)
        /// </summary>
        public int InviteCodeMaxCount
        {
            get { return m_invitecodemaxcount; }
            set { m_invitecodemaxcount = value; }
        }

        /// <summary>
        /// 邀请码邀请成功奖励积分及格线(人)
        /// </summary>
        public int InviteCodePayCount
        {
            get { return m_invitecodeaddcreditsline; }
            set { m_invitecodeaddcreditsline = value; }
        }

        /// <summary>
        /// 封闭式邀请码价格
        /// </summary>
        public string InviteCodePrice
        {
            get { return m_invitecodeprice; }
            set { m_invitecodeprice = value; }
        }

        /// <summary>
        /// 邀请功能注册用户介绍文字
        /// </summary>
        public string InvitationLoginUserDescription
        {
            get { return m_invitationloginuserdescription; }
            set { m_invitationloginuserdescription = value; }
        }

        /// <summary>
        /// 邀请功能游客介绍文字
        /// </summary>
        public string InvitationVisitorDescription
        {
            get { return m_invitationvisitordescription; }
            set { m_invitationvisitordescription = value; }
        }

        /// <summary>
        /// 邀请功能发送邮件内容模板
        /// </summary>
        public string InvitationEmailTemplate
        {
            get { return m_invitationemailmodel; }
            set { m_invitationemailmodel = value; }
        }

        /// <summary>
        /// 用户最多可以购买的邀请码个数(封闭式)
        /// </summary>
        public int InviteCodeMaxCountToBuy
        {
            get { return m_invitecodeusermaxbuy; }
            set { m_invitecodeusermaxbuy = value; }
        }

        /// <summary>
        /// 用户每天最多可以获得多少个开放式邀请码
        /// </summary>
        public int InviteCodeUserCreatePerDay
        {
            get { return m_invitecodeusercreateperday; }
            set { m_invitecodeusercreateperday = value; }
        }

    }
}
