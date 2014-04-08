using System;

namespace Discuz.Entity
{
    public class CreditOrderInfo
    {
        private int _orderId;
        private int _uid;
        private string _buyer;
        private int _payType;
        private string _tradeNo;
        private decimal _price;
        private int _orderStatus;
        private string _orderCode;
        private string _createdTime;
        private string _confirmedTime;
        private int _credit;
        private int _amount;

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderId
        {
            get { return _orderId; }
            set { _orderId = value; }
        }

        /// <summary>
        /// 外部订单号
        /// </summary>
        public string OrderCode
        {
            get { return _orderCode.Trim(); }
            set { _orderCode = value; }
        }

        /// <summary>
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Buyer
        {
            get { return _buyer.Trim(); }
            set { _buyer = value; }
        }

        /// <summary>
        /// 支付平台类型，1：支付宝
        /// </summary>
        public int PayType
        {
            get { return _payType; }
            set { _payType = value; }
        }

        /// <summary>
        /// 支付平台订单号
        /// </summary>
        public string TradeNo
        {
            get { return _tradeNo.Trim(); }
            set { _tradeNo = value; }
        }

        /// <summary>
        /// 订单价格
        /// </summary>
        public decimal Price
        {
            get
            {
                _price = decimal.Round(_price, 2);
                return _price;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_price.ToString(), "Price(商品单价) 必须为0.01在100000000.00之间");
                }
                _price = value;
            }
        }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus
        {
            get { return _orderStatus; }
            set { _orderStatus = value; }
        }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        public string CreatedTime
        {
            get { return _createdTime; }
            set { _createdTime = value; }
        }

        /// <summary>
        /// 订单确认时间
        /// </summary>
        public string ConfirmedTime
        {
            get { return _confirmedTime; }
            set { _confirmedTime = value; }
        }

        /// <summary>
        /// 接收积分类型(扩建积分号码1-8)
        /// </summary>
        public int Credit
        {
            get { return _credit; }
            set { _credit = value; }
        }

        /// <summary>
        /// 接收积分值
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
