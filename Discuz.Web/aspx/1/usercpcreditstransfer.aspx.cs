using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    /// <summary>
    /// 积分转账
    /// </summary>
    public class usercpcreditstransfer : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 交易税
        /// </summary>
        public float creditstax = Scoresets.GetCreditsTax();
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return; 

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                bool isPasswordError = true;

                switch (config.Passwordmode)
                {
                    case 1://动网兼容模式
                        {
                            isPasswordError = Utils.MD5(DNTRequest.GetString("password")) != password;
                            break;
                        }
                    case 0://默认模式
                        {
                            isPasswordError = Utils.MD5(DNTRequest.GetString("password")) != password;
                            break;
                        }
                    default: //第三方加密验证模式
                        {
                            if (PasswordModeProvider.GetInstance() != null)
                                isPasswordError = !PasswordModeProvider.GetInstance().CheckPassword(user, DNTRequest.GetString("password"));
                            break;
                        }
                }
                if (isPasswordError)
                {
                    AddErrLine("密码错误");
                    return;
                }

                int paynum = DNTRequest.GetInt("paynum", 0);
                if (paynum <= 0)
                {
                    AddErrLine("数量必须是大于等于0的整数");
                    return;
                }
                int fromto = Users.GetUserId(DNTRequest.GetString("fromto").Trim());
                if (fromto == -1)
                {
                    AddErrLine("指定的转帐接受人不存在");
                    return;
                }
                int extcredits = DNTRequest.GetInt("extcredits", 0);
                if (extcredits < 1 || extcredits > 8)
                {
                    AddErrLine("请正确选择要转帐的积分类型!");
                    return;
                }
                //对转帐后的积分增减进行修改设置
                string extcreditsName = Scoresets.GetScoreSet(extcredits).Name.Trim();
                if (Utils.StrIsNullOrEmpty(extcreditsName))
                {
                    AddErrLine("错误的输入!");
                    return;
                }
                if ((Users.GetUserExtCredits(userid, extcredits) - paynum) < Scoresets.GetTransferMinCredits())
                {
                    AddErrLine(string.Format("抱歉, 您的 \"{0}\" 不足.系统当前规定转帐余额不得小于{1}", extcreditsName, Scoresets.GetTransferMinCredits().ToString()));
                    return;
                }

                //计算并更新2个扩展积分的新值
                float toPayNum = (float)Math.Round(paynum * (1 - creditstax), 2);
                Users.UpdateUserExtCredits(userid, extcredits, paynum * -1);
                Users.UpdateUserExtCredits(fromto, extcredits, toPayNum);
                CreditsLogs.AddCreditsLog(userid, fromto, extcredits, extcredits, paynum, toPayNum, Utils.GetDateTime(), 2);

                SetUrl("usercpcreaditstransferlog.aspx");
                SetMetaRefresh();
                SetShowBackLink(false);
                AddMsgLine("积分转帐完毕, 正在返回积分兑换与转帐记录");
            }
        }
    } //class end
}