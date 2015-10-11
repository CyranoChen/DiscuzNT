using System;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using System.Data.Common;
using Discuz.Web.UI;
using Discuz.Entity;
using System.Text.RegularExpressions;

namespace Discuz.Web
{
    /// <summary>
    /// 购买主题页面类
    /// </summary>
    public class buytopic : TopicPage
    {

        #region 页面变量
        /// <summary>
        /// 最后5个回复列表
        /// </summary>
        public DataTable lastpostlist;
        /// <summary>
        /// 已购买的支付记录列表
        /// </summary>
        public DataTable paymentloglist;
        /// <summary>
        /// 用户积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
        /// <summary>
        /// 主题购买总次数
        /// </summary>
        public int buyers;
        /// <summary>
        /// 是否显示购买信息列表
        /// </summary>
        public int showpayments = DNTRequest.GetInt("showpayments", 0);
        /// <summary>
        /// 在判断此值等于1时显示点击购买主题后的确认购买界面
        /// </summary>
        public int buyit = DNTRequest.GetInt("buyit", 0);
        /// <summary>
        /// 主题售价
        /// </summary>
        public int topicprice;
        /// <summary>
        /// 作者所得
        /// </summary>
        public float netamount;
        /// <summary>
        /// 单个主题最高收入
        /// </summary>
        public int maxincpertopic = Scoresets.GetMaxIncPerTopic();
        /// <summary>
        /// 单个主题最高出售时限(小时)
        /// </summary>
        public int maxchargespan = Scoresets.GetMaxChargeSpan();
        /// <summary>
        /// 积分交易税
        /// </summary>
        public float creditstax = Scoresets.GetCreditsTax() * 100;
        /// <summary>
        /// 主题售价
        /// </summary>
        public int price = 0;
        /// <summary>
        /// 购买后余额
        /// </summary>
        public float userlastprice;

        public PostInfo postinfo;
        public string postmessage = "";
        public static Regex r = new Regex(@"\s*\[free\][\n\r]*([\s\S]+?)[\n\r]*\[\/free\]\s*", RegexOptions.IgnoreCase);
        #endregion

        private int isModer = 0;
        private int pageSize = GeneralConfigs.GetConfig().Tpp;

        protected override void ShowPage()
        {
            if (!SetTopicInfo())
            {
                topic = new TopicInfo();
                forum = new ForumInfo();
                return;
            }

            pagetitle = topic.Title.Trim();

            if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forum.Fid + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                Response.Redirect(string.Format("{0}showforum-{1}{2}", BaseConfigs.GetForumPath, forum.Fid, config.Extname), true);
                return;
            }
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                return;
            }

            postinfo = Posts.GetTopicPostInfo(topicid);
            if (postinfo.Message.ToLower().Contains("[free]") || postinfo.Message.ToLower().Contains("[/free]"))
            {
                for (Match m = r.Match(postinfo.Message); m.Success; m = m.NextMatch())
                {
                    postmessage += "<br /><div class=\"msgheader\">免费内容:</div><div class=\"msgborder\">" + m.Groups[1] + "</div><br />";
                }
            }

            #region 获取主题售价等相关信息
            topicprice = topic.Price;
            //判断是否为回复可见帖, price=0为非购买可见(正常), price>0 为购买可见, price=-1为购买可见但当前用户已购买
            if (topic.Price > 0)
            {
                price = topic.Price;
                //判断当前用户是否已经购买
                if (PaymentLogs.IsBuyer(topicid, userid) || (Utils.StrDateDiffHours(topic.Postdatetime, Scoresets.GetMaxChargeSpan()) > 0 && Scoresets.GetMaxChargeSpan() != 0))
                    price = -1;
            }

            netamount = topicprice - topicprice * creditstax / 100;
            if (topicprice > maxincpertopic)
                netamount = maxincpertopic - maxincpertopic * creditstax / 100;

            if (price != -1)
            {
                UserInfo userInfo = Users.GetUserInfo(userid);

                if (buyit == 1 && !CheckUserExtCredit(userInfo)) return;

                userlastprice = Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()) - topic.Price;
            }
            #endregion

            if (useradminid != 0)
                isModer = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;

            //如果不是提交...
            if (!ispost)
            {
                buyers = PaymentLogs.GetPaymentLogByTidCount(topic.Tid);
                //显示购买信息列表
                if (showpayments == 1)
                {
                    //获取总页数
                    pagecount = buyers % pageSize == 0 ? buyers / pageSize : buyers / pageSize + 1;
                    pagecount = pagecount == 0 ? 1 : pagecount;
                    //修正请求页数中可能的错误
                    pageid = pageid < 1 ? 1 : pageid;
                    pageid = pageid > pagecount ? pagecount : pageid;

                    //获取收入记录并分页显示
                    paymentloglist = PaymentLogs.GetPaymentLogByTid(pageSize, pageid, topic.Tid);
                }

                //判断是否为回复可见帖, hide=0为非回复可见(正常), hide>0为回复可见, hide=-1为回复可见但当前用户已回复
                int hide = (topic.Hide == 1 ? topic.Hide : 0);

                if (Posts.IsReplier(topicid, userid)) hide = -1;

                lastpostlist = Posts.GetLastPostDataTable(GetPostPramsInfo(hide));
            }
            else
            {
                int reval = PaymentLogs.BuyTopic(userid, topic.Tid, topic.Posterid, topic.Price, netamount);
                if (reval > 0)
                {
                    SetUrl(base.ShowTopicAspxRewrite(topic.Tid, 0));
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    MsgForward("buytopic_succeed");
                    AddMsgLine("购买主题成功,返回该主题");
                    return;
                }
                else
                {
                    SetBackLink(base.ShowForumAspxRewrite(topic.Fid, 0));
                    if (reval == -1)
                        AddErrLine("对不起,您的账户余额少于交易额,无法进行交易");
                    else if (reval == -2)
                        AddErrLine("您无权购买本主题");
                    else
                        AddErrLine("未知原因,交易无法进行,给您带来的不方便我们很抱歉");
                    return;
                }
            }
        }

        private bool SetTopicInfo()
        {
            if (userid < 0)
            {
                AddErrLine("您还没有登录，请登录后再操作");
                needlogin = true;
                return false;
            }

            if (topicid == -1)
            {
                AddErrLine("无效的主题ID");
                return false;
            }
            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);

            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return false;
            }
            if (topic.Displayorder == -1 || topic.Displayorder == -2)
            {
                AddErrLine("此主题已被删除或未经审核！");
                return false;
            }
            if (topic.Posterid == userid || topic.Price <= 0)
            {
                System.Web.HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + base.ShowTopicAspxRewrite(topic.Tid, 0));
                return false;
            }
            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && isModer != 1)
            {
                AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.Readperm.ToString(), usergroupinfo.Grouptitle));
                return false;
            }
            forum = Forums.GetForumInfo(topic.Fid);

            if (forum == null)
            {
                AddErrLine("主题对应版块不存在！");
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取PostpramsInfo
        /// </summary>
        /// <param name="hide"></param>
        /// <returns></returns>
        private PostpramsInfo GetPostPramsInfo(int hide)
        {
            #region 参数赋值
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = 5;
            postpramsInfo.Pageindex = 1;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = price;
            postpramsInfo.Ubbmode = false;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserid = userid;

            UserInfo userInfo = Users.GetUserInfo(postpramsInfo.CurrentUserid);
            postpramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;
            #endregion

            return postpramsInfo;
        }

        public bool CheckUserExtCredit(UserInfo userInfo)
        {
            if (userInfo == null)
            {
                AddErrLine("您无权购买本主题");
                needlogin = true;
                return false;
            }

            if (Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()) < topic.Price)
            {
                string addExtCreditsTip = "";
                if (EPayments.IsOpenEPayments())
                    addExtCreditsTip = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
                AddErrLine(string.Format("对不起,您的账户余额 <span class=\"bold\">{0} {1}{2}</span> 交易额为 {3}{2} ,无法进行交易.{4}", Scoresets.GetValidScoreName()[Scoresets.GetTopicAttachCreditsTrans()],
                    Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()),
                    Scoresets.GetValidScoreUnit()[Scoresets.GetTopicAttachCreditsTrans()], topic.Price, addExtCreditsTip));
                return false;
            }
            return true;
        }
    }
}
