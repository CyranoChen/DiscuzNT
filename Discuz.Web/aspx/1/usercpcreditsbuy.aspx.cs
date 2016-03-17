using System;
using System.Data;
using System.Web;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.Payment;
using Discuz.Plugin.Payment.Alipay;
using Discuz.Web.UI;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    /// <summary>
    /// 积分兑换
    /// </summary>
    public class usercpcreditsbuy : UserCpPage
    {
        #region 页面变量
        /// <summary>
        /// 积分交易税
        /// </summary>
        public float creditstax = Scoresets.GetCreditsTax();
        /// <summary>
        /// 积分计算器js脚本
        /// </summary>
        public string jscreditsratearray = "<script type=\"text/javascript\">\r\nvar creditsrate = new Array();\r\n{0}\r\n</script>";
        /// <summary>
        /// 交易积分
        /// </summary>
        public int creditstrans = Scoresets.GetCreditsTrans();
        /// <summary>
        /// 交易积分名称
        /// </summary>
        public string creditstransname = Scoresets.GetValidScoreName()[Scoresets.GetCreditsTrans()];
        /// <summary>
        /// 交易积分单位
        /// </summary>
        public string creditstransunit = Scoresets.GetValidScoreUnit()[Scoresets.GetCreditsTrans()];
        /// <summary>
        /// 购买的积分数量
        /// </summary>
        public int creditsamount = DNTRequest.GetInt("amount", 1);
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "积分充值";

            if (!EPayments.IsOpenEPayments())
            {
                AddErrLine("论坛未开启积分充值服务！");
                return;
            }

            string jsCreditsRateArray = "";
            foreach (DataRow dr in Scoresets.GetScorePaySet(0).Rows)
            {
                jsCreditsRateArray += "creditsrate[" + dr["id"] + "] = " + dr["rate"] + ";\r\n";
            }
            jscreditsratearray = string.Format(jscreditsratearray, jsCreditsRateArray);

            if (!IsLogin()) return;

            if (!string.IsNullOrEmpty(DNTRequest.GetString("redirect")))
            {
                switch (DNTRequest.GetString("redirect"))//根据该值判断操作类型
                {
                    case "alipay":
                        RedirectToAlipay();
                        break;
                    default:
                        return;
                }
            }
        }
        /// <summary>
        /// 定位到支付宝支付界面
        /// </summary>
        public void RedirectToAlipay()
        {
            DigitalTrade alipayTrade = new DigitalTrade();
            alipayTrade.Subject = string.Format("{0} 论坛积分充值({1}:{2}{3}),用户:{4}", config.Forumtitle, creditstransname, creditsamount, creditstransunit, username);
            if (Utils.IsValidEmail(config.Alipayaccout))//如果用户保存在配置文件里面的支付宝帐号格式是email的话
                alipayTrade.Seller_Email = config.Alipayaccout;
            else
                alipayTrade.Seller_Id = config.Alipayaccout;

            alipayTrade.Return_Url = Utils.GetRootUrl(forumpath) + "tools/notifypage.aspx";
            alipayTrade.Notify_Url = Utils.GetRootUrl(forumpath) + "tools/notifypage.aspx";
            alipayTrade.Quantity = 1;
            decimal price = decimal.Round(((decimal)creditsamount / (decimal)config.Cashtocreditrate), 2);
            alipayTrade.Price = price > 0.1M ? price : 0.1M;//限定每个订单的最低价格是0.1元
            alipayTrade.Payment_Type = 1;
            alipayTrade.PayMethod = "bankPay";//跳转到支付宝时默认支付类型是网上银行
            string payUrl = "";

            alipayTrade.Partner = config.Alipaypartnerid;
            alipayTrade.Sign = config.Alipaypartnercheckkey;
            payUrl = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(alipayTrade);

            CreditOrders.CreateCreditOrder(userid, username, creditstrans, creditsamount, 1, alipayTrade.Out_Trade_No);//创建积分充值订单
            HttpContext.Current.Response.Redirect(payUrl);//跳转到支付宝即时到帐支付页面
        }
    }
}