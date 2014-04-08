using System;
using System.Web;

using Discuz.Common;
using Discuz.Config;
using Discuz.Plugin.Payment;
using Discuz.Plugin.Payment.Alipay;

namespace Discuz.Web.Admin
{
    public class screditset : AdminPage
    {
        public GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsFounderUid(userid))
            {
                Response.Write(base.GetShowMessage());
                Response.End();
                return;
            }
            if (!string.IsNullOrEmpty(DNTRequest.GetString("accout")))
            {
                TestAccout(DNTRequest.GetString("accout"));
            }
            if (IsPostBack)
            {
                configInfo.Alipayaccout = DNTRequest.GetFormString("alipayaccount");
                configInfo.Cashtocreditrate = DNTRequest.GetFormInt("cashtocreditsrate", 0);

                int mincreditstobuy = DNTRequest.GetFormInt("mincreditstobuy", 0);
                //如果现金/积分兑换比率为0，则表示不开启积分充值功能
                if (configInfo.Cashtocreditrate > 0)
                {
                    //为了保证生成的订单价格最低价格为0.1元，则需要根据现金和积分兑换比率来动态调整积分最少购买数量的值
                    while ((decimal)mincreditstobuy / (decimal)configInfo.Cashtocreditrate < 0.10M)
                    {
                        mincreditstobuy++;
                    }
                }

                configInfo.Mincreditstobuy = mincreditstobuy;
                configInfo.Maxcreditstobuy = DNTRequest.GetFormInt("maxcreditstobuy", 0);
                configInfo.Userbuycreditscountperday = DNTRequest.GetFormInt("userbuycreditscountperday", 0);
                configInfo.Alipaypartnercheckkey = DNTRequest.GetFormString("alipaypartnercheckkey");
                configInfo.Alipaypartnerid = DNTRequest.GetFormString("alipaypartnerid");
                configInfo.Usealipaycustompartnerid = DNTRequest.GetFormInt("usealipaycustompartnerid", 1);
                configInfo.Usealipayinstantpay = DNTRequest.GetFormInt("usealipayinstantpay", 0);

                GeneralConfigs.SaveConfig(configInfo);
                GeneralConfigs.ResetConfig();
                base.RegisterStartupScript("PAGE", "window.location.href='global_screditset.aspx';");
            }
        }

        /// <summary>
        /// 后台测试填写帐号是否可用
        /// </summary>
        public void TestAccout(string accout)
        {
            int openPartner = DNTRequest.GetInt("openpartner", 0);
            string partnerId = DNTRequest.GetString("partnerid");
            string partnerKey = DNTRequest.GetString("partnerKey");

            DigitalTrade virtualTrade = new DigitalTrade();
            virtualTrade.Subject = "测试支付宝充值功能";

            if (Utils.IsValidEmail(accout))//如果传递的帐号类型是email
                virtualTrade.Seller_Email = accout;
            else
                virtualTrade.Seller_Id = accout;

            virtualTrade.Return_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            virtualTrade.Notify_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            virtualTrade.Quantity = 1;
            virtualTrade.Price = 0.1M;
            virtualTrade.Payment_Type = 1;
            virtualTrade.PayMethod = "bankPay";

            string payUrl = "";

            if (openPartner == 1)
            {
                virtualTrade.Partner = partnerId;
                virtualTrade.Sign = partnerKey;
                payUrl = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(virtualTrade);
            }
            else
            {
                payUrl = AliPayment.GetService().CreateDigitalGoodsTradeUrl(virtualTrade);
            }

            HttpContext.Current.Response.Redirect(payUrl);
        }
    }
}
