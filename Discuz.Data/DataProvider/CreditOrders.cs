using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Data
{
    public class CreditOrders
    {
        private static CreditOrderInfo LoadCreditOrderInfo(IDataReader reader)
        {
            CreditOrderInfo orderInfo = new CreditOrderInfo();
            orderInfo.Amount = TypeConverter.ObjectToInt(reader["amount"]);
            orderInfo.Buyer = reader["buyer"].ToString();
            orderInfo.CreatedTime = reader["createdtime"].ToString();
            orderInfo.ConfirmedTime=reader["confirmedtime"].ToString();
            orderInfo.Credit = TypeConverter.ObjectToInt(reader["credit"]);
            orderInfo.OrderCode = reader["ordercode"].ToString();
            orderInfo.OrderId = TypeConverter.ObjectToInt(reader["orderid"]);
            orderInfo.OrderStatus = TypeConverter.ObjectToInt(reader["orderstatus"]);
            orderInfo.PayType = TypeConverter.ObjectToInt(reader["paytype"]);
            orderInfo.Price = (decimal)TypeConverter.ObjectToFloat(reader["price"]);
            orderInfo.TradeNo = reader["tradeno"].ToString();
            orderInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            return orderInfo;
        }

        /// <summary>
        /// 创建积分订单信息
        /// </summary>
        /// <param name="creditOrderInfo"></param>
        /// <returns></returns>
        public static int CreateCreditOrder(CreditOrderInfo creditOrderInfo)
        {
            return DatabaseProvider.GetInstance().CreateCreditOrder(creditOrderInfo);
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
            return DatabaseProvider.GetInstance().GetCreditOrderCount(status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
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
            Discuz.Common.Generic.List<CreditOrderInfo> list = new Discuz.Common.Generic.List<CreditOrderInfo>();

            IDataReader reader = DatabaseProvider.GetInstance().GetCreditOrderList(pageIndex, status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(LoadCreditOrderInfo(reader));
                }
                reader.Close();
            }
            return list;
        }

        /// <summary>
        /// 通过ordercode获取积分订单信息
        /// </summary>
        /// <param name="orderCode">订单外部订单号</param>
        /// <returns></returns>
        public static CreditOrderInfo GetCreditOrderByOrderCode(string orderCode)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetCreditOrderByOrderCode(orderCode);
            CreditOrderInfo orderInfo = new CreditOrderInfo();
            if (reader.Read())
            {
                orderInfo = LoadCreditOrderInfo(reader);
            }
            reader.Close();
            return orderInfo;
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
            return DatabaseProvider.GetInstance().UpdateCreditOrderInfo(orderId, tradeNo, orderStatus, confirmedTime);
        }
    }
}
