using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public partial class creditsordermanage : AdminPage
    {
        /// <summary>
        /// 积分订单列表
        /// </summary>
        public List<CreditOrderInfo> orderList = new List<CreditOrderInfo>();
        /// <summary>
        /// 查询条件 订单状态
        /// </summary>
        public int status = DNTRequest.GetInt("orderstatus", -1);
        /// <summary>
        /// 查询条件 订单id
        /// </summary>
        public int orderId = DNTRequest.GetInt("orderid", 0);
        /// <summary>
        /// 查询条件 支付宝订单号
        /// </summary>
        public string tradeNo = DNTRequest.GetString("tradeno", true);
        /// <summary>
        /// 查询条件 支付用户名
        /// </summary>
        public string buyer = DNTRequest.GetString("buyer");
        /// <summary>
        /// 查询条件 订单提交开始时间
        /// </summary>
        public string submitStartDate = Utils.IsDateString(DNTRequest.GetString("submitstartdate")) ? DNTRequest.GetString("submitstartdate") : "";
        /// <summary>
        /// 查询条件 订单提交结束时间
        /// </summary>
        public string submitLastDate = Utils.IsDateString(DNTRequest.GetString("submitlastdate")) ? DNTRequest.GetString("submitlastdate") : "";
        /// <summary>
        /// 查询条件 订单确认开始时间
        /// </summary>
        public string confirmedStartDate = Utils.IsDateString(DNTRequest.GetString("confirmstartdate")) ? DNTRequest.GetString("confirmstartdate") : "";
        /// <summary>
        /// 查询条件 订单确认结束时间
        /// </summary>
        public string confirmedLastDate = Utils.IsDateString(DNTRequest.GetString("confirmlastdate")) ? DNTRequest.GetString("confirmlastdate") : "";
        /// <summary>
        /// 订单页码
        /// </summary>
        public int pageIndex = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 订单页码总数
        /// </summary>
        public int pageCount;
        /// <summary>
        /// 订单总数
        /// </summary>
        public int orderCount;


        protected void Page_Load(object sender, EventArgs e)
        {
            orderList = CreditOrders.GetCreditOrderList(pageIndex, status, orderId, tradeNo, buyer, submitStartDate, submitLastDate, confirmedStartDate, confirmedLastDate);
            orderCount = CreditOrders.GetCreditOrderCount(status, orderId, tradeNo, buyer, submitStartDate, submitLastDate, confirmedStartDate, confirmedLastDate);
            pageCount = ((orderCount - 1) / 20) + 1;
        }

        /// <summary>
        /// 将订单数值状态转换为文字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string ConvertStatusNoToWord(int number)
        {
            switch (number)
            {
                case 0:
                    return "等待付款";
                case 1:
                    return "已付款，等待发货";
                case 2:
                    return "交易成功";
                default:
                    return "未知状态";
            }
        }

        /// <summary>
        /// 订单列表页码生成
        /// </summary>
        /// <returns></returns>
        public string ShowPageIndex()
        {
            string str = "";
            int startIndex = pageIndex - 5 > 0 ? (pageIndex - 5 - (pageIndex + 5 < pageCount ? 0 : (pageIndex + 5 - pageCount))) : 1;
            int lastIndex = pageIndex + 5 < pageCount ? pageIndex + 5 + (pageIndex - 5 > 0 ? 0 : ((pageIndex - 5) * -1) + 1) : pageCount;
            for (int i = startIndex; i <= lastIndex; i++)
            {
                if (i != pageIndex)
                    str += string.Format("<td style=\"height:20px;line-height:20px;\"><a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"###\" onclick=\"goPageIndex({0})\">{0}</a></td>", i);
                else
                    str += string.Format("<td style=\"height:20px;line-height:20px;font-weight:700;\"><span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span></td> ", i);
            }
            return str;
        }
    }
}
