using System;
using System.Collections.Generic;
using System.Web;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Payment;
using Discuz.Plugin.Payment.Alipay;

namespace Discuz.Web.UI
{
    public class NotifyPage : System.Web.UI.Page
    {
        public NotifyPage()
        {
            if (EPayments.CheckPayment(DNTRequest.GetString("notify_id")))//验证请求是否来自支付宝，或者是伪造的
            {
                //获取需要的信息
                int orderStatus = EPayments.ConvertAlipayTradeStatus(DNTRequest.GetString("trade_status"));
                string orderCode = DNTRequest.GetString("out_trade_no", true);
                string tradeNo = DNTRequest.GetString("trade_no", true);

                if (string.IsNullOrEmpty(orderCode) || string.IsNullOrEmpty(tradeNo) || orderStatus <= 0)
                    return;

                CreditOrderInfo orderInfo = CreditOrders.GetCreditOrderInfoByOrderCode(orderCode);


                //如果订单状态为未成功交易
                if (orderInfo != null && orderInfo.OrderStatus < 2)
                {
                    float[] extcredits = new float[8];
                    extcredits[orderInfo.Credit - 1] = orderInfo.Amount;

                    if (UserCredits.UpdateUserExtCredits(orderInfo.Uid, extcredits, true) != 1)
                        orderStatus = 0;

                    CreditsLogs.AddCreditsLog(orderInfo.Uid, orderInfo.Uid, orderInfo.Credit, orderInfo.Credit, 0, orderInfo.Amount, Utils.GetDateTime(), 3);//添加积分兑换转账和充值记录

                    NoticeInfo notice = new NoticeInfo();
                    notice.Postdatetime = Utils.GetDateTime();
                    notice.Type = NoticeType.GoodsTradeNotice;
                    notice.Poster = "系统";
                    notice.Posterid = 0;
                    notice.Uid = orderInfo.Uid;
                    notice.Note = string.Format("您购买的积分 {0} 已经成功充值，请<a href=\"usercpcreaditstransferlog.aspx\">查收</a>!(支付宝订单号:{1})", ForumUtils.ConvertCreditAndAmountToWord(orderInfo.Credit, orderInfo.Amount), tradeNo);
                    Notices.CreateNoticeInfo(notice);

                    CreditOrders.UpdateCreditOrderInfo(orderInfo.OrderId, tradeNo, orderStatus, Utils.GetDateTime());//修改积分订单记录状态
                }
                //判断当前请求是支付宝GET式（直接跳转）或者是服务器POST式（需返回success使得支付宝停止继续发送通知）
                if (DNTRequest.IsPost())
                    HttpContext.Current.Response.Write("success");
                else
                    HttpContext.Current.Response.Redirect("../usercpcreaditstransferlog.aspx?paysuccess=true");
            }
            else
                HttpContext.Current.Response.Write("fail");
        }
    }
}
