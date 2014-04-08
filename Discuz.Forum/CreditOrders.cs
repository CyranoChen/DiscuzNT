using System;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Payment;
using Discuz.Plugin.Payment.Alipay;

namespace Discuz.Forum
{
    public class CreditOrders
    {
        /// <summary>
        /// 创建积分订单记录
        /// </summary>
        /// <param name="uId">用户uid</param>
        /// <param name="buyer">用户登录名</param>
        /// <param name="credit">操作积分类型(扩展积分1-8)</param>
        /// <param name="amount">操作积分数量</param>
        /// <param name="paytype">支付种类（支付宝或其他）</param>
        /// <param name="outTradeNo">外部订单号（支付宝要求的形如：20080104140009iwGampfQkzFgMZ0yoT）</param>
        /// <returns></returns>
        public static int CreateCreditOrder(int uId, string buyer, int credit, int amount, int paytype, string outTradeNo)
        {
            if (uId < 0 || string.IsNullOrEmpty(buyer) || credit < 1 || credit > 8 || amount < 1 || string.IsNullOrEmpty(outTradeNo))
                return 0;
            CreditOrderInfo order = new CreditOrderInfo();
            order.Uid = uId;
            order.OrderCode = outTradeNo;
            order.Amount = amount;
            order.Credit = credit;
            order.Buyer = buyer;
            order.OrderStatus = 0;
            order.PayType = paytype;
            order.Price = decimal.Round(((decimal)amount / (decimal)GeneralConfigs.GetConfig().Cashtocreditrate), 2);
            return Data.CreditOrders.CreateCreditOrder(order);
        }

        /// <summary>
        /// 获取符合查询条件的订单数量
        /// </summary>
        /// <param name="status">订单状态（0：未付款  1：已付款，卖家未发货  2：交易成功）</param>
        /// <param name="orderId">订单id</param>
        /// <param name="tradeNo">支付宝订单号</param>
        /// <param name="buyer">买家用户名</param>
        /// <param name="submitStartTime">查询提交开始时间</param>
        /// <param name="submitLastTime">查询提交结束时间</param>
        /// <param name="confirmStartTime">查询确认开始时间</param>
        /// <param name="confirmLastTime">查询确认结束时间</param>
        /// <returns></returns>
        public static int GetCreditOrderCount(int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime)
        {
            return Data.CreditOrders.GetCreditOrderCount(status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
        }

        /// <summary>
        /// 获取符合查询条件的订单
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="status">订单状态（0：未付款  1：已付款，卖家未发货  2：交易成功）</param>
        /// <param name="orderId">订单id</param>
        /// <param name="tradeNo">支付宝订单号</param>
        /// <param name="buyer">买家用户名</param>
        /// <param name="submitStartTime">查询提交开始时间</param>
        /// <param name="submitLastTime">查询提交结束时间</param>
        /// <param name="confirmStartTime">查询确认开始时间</param>
        /// <param name="confirmLastTime">查询确认结束时间</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<CreditOrderInfo> GetCreditOrderList(int pageIndex, int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime)
        {
            return Data.CreditOrders.GetCreditOrderList(pageIndex, status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
        }

        /// <summary>
        /// 通过ordercode获取积分订单信息
        /// </summary>
        /// <param name="orderCode">订单外部订单号</param>
        /// <returns></returns>
        public static CreditOrderInfo GetCreditOrderInfoByOrderCode(string orderCode)
        {
            return Data.CreditOrders.GetCreditOrderByOrderCode(orderCode);
        }

        /// <summary>
        /// 更新积分订单信息
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <param name="tradeNo">支付宝订单号</param>
        /// <param name="orderStatus">订单状态</param>
        /// <param name="confirmedTime">确认时间</param>
        /// <returns></returns>
        public static int UpdateCreditOrderInfo(int orderId, string tradeNo, int orderStatus, string confirmedTime)
        {
            return Data.CreditOrders.UpdateCreditOrderInfo(orderId, tradeNo, orderStatus, confirmedTime);
        }

    }
}
