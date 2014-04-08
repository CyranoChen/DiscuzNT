using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 交易状态枚举
    /// </summary>
    public enum TradeStatusEnum
    {
        /// <summary>
        /// 未生效的交易
        /// </summary>
        UnStart = 0,
        /// <summary>
        /// 等待买家付款
        /// </summary>
        WAIT_BUYER_PAY = 1,
        /// <summary>
        /// 交易已创建,等待卖家确认
        /// </summary>
        WAIT_SELLER_CONFIRM_TRADE = 2,
        /// <summary>
        /// 确认买家付款中，暂勿发货
        /// </summary>
        WAIT_SYS_CONFIRM_PAY = 3,
        /// <summary>
        /// 买家已付款(或支付宝收到买家付款),请卖家发货
        /// </summary>
        WAIT_SELLER_SEND_GOODS = 4,
        /// <summary>
        /// 卖家已发货，买家确认中
        /// </summary>
        WAIT_BUYER_CONFIRM_GOODS = 5,
        /// <summary>
        /// 买家确认收到货，等待支付宝打款给卖家
        /// </summary>
        WAIT_SYS_PAY_SELLER = 6,
        /// <summary>
        /// 交易成功结束
        /// </summary>
        TRADE_FINISHED = 7,
        /// <summary>
        /// 交易中途关闭(未完成)
        /// </summary>
        TRADE_CLOSED = 8,
        /// <summary>
        /// 等待卖家同意退款
        /// </summary>
        WAIT_SELLER_AGREE = 10,
        /// <summary>
        /// 卖家拒绝买家条件，等待买家修改条件
        /// </summary>
        SELLER_REFUSE_BUYER = 11,
        /// <summary>
        /// 卖家同意退款，等待买家退货
        /// </summary>
        WAIT_BUYER_RETURN_GOODS = 12,
        /// <summary>
        /// 等待卖家收货
        /// </summary>
        WAIT_SELLER_CONFIRM_GOODS = 13,
        /// <summary>
        /// 双方已经一致，等待支付宝退款
        /// </summary>
        WAIT_ALIPAY_REFUND = 14,
        /// <summary>
        /// 支付宝处理中
        /// </summary>
        ALIPAY_CHECK = 15,
        /// <summary>
        /// 结束的退款
        /// </summary>
        OVERED_REFUND = 16,
        /// <summary>
        /// 退款成功(卖家已收到退货)
        /// </summary>
        REFUND_SUCCESS = 17,
        /// <summary>
        /// 退款关闭
        /// </summary>
        REFUND_CLOSED = 18
    }

    /// <summary>
    /// 交易数据统计类
    /// </summary>
    public class Goosdstradestatisticinfo
    {
        private int __userid;//用户id
        /// <summary>
        /// 用户id
        /// </summary>
        public int Userid
        {
            get { return __userid; }
            set { __userid = value; }
        }

        private int __sellerattention;//卖家关注交易数
        /// <summary>
        /// 卖家关注交易数
        /// </summary>
        public int Sellerattention
        {
            get { return __sellerattention;}
            set { __sellerattention = value;}
        }

        private int __sellertrading;//卖家交易进行中的交易数
        /// <summary>
        /// 卖家交易进行中的交易数
        /// </summary>
        public int Sellertrading 
        {
            get { return __sellertrading;}
            set { __sellertrading = value; }
        }

        private int __sellerrate;//需卖家评价的交易数
        /// <summary>
        /// 需卖家评价的交易数
        /// </summary>
        public int Sellerrate
        {
            get { return __sellerrate;}
            set { __sellerrate = value; }
        }

        private decimal __sellnumbersum;//卖家售出商品总数
        /// <summary>
        /// 卖家售出商品总数
        /// </summary>
        public decimal Sellnumbersum
        {
            get { return __sellnumbersum; }
            set { __sellnumbersum = value; }
        }

        private decimal __selltradesum;//卖家销售成交总额
        /// <summary>
        /// 卖家销售成交总额
        /// </summary>
        public decimal Selltradesum
        {
            get { return __selltradesum; }
            set { __selltradesum = value; }
        }


        private int __buyerattention;//买家关注交易数
        /// <summary>
        /// 买家关注交易数
        /// </summary>
        public int Buyerattention
        {
            get { return __buyerattention;}
            set { __buyerattention = value; }
        }

        private int __buyertradeing;//买家交易进行中的交易数
        /// <summary>
        /// 买家交易进行中的交易数
        /// </summary>
        public int Buyertrading   
        {
            get { return __buyertradeing;}
            set { __buyertradeing = value; }
        }

        private int __buyerrate;//需买家评价的交易数
        /// <summary>
        /// 需买家评价的交易数
        /// </summary>
        public int Buyerrate
        {
            get { return __buyerrate;}
            set { __buyerrate = value; }
        }

        private decimal __buynumbersum;//买入商品总数
        /// <summary>
        /// 买入商品总数
        /// </summary>
        public decimal Buynumbersum
        {
            get { return __buynumbersum; }
            set { __buynumbersum = value; }
        }

        private decimal __buytradesum;//买入成交总额
        /// <summary>
        /// 买入成交总额
        /// </summary>
        public decimal Buytradesum
        {
            get { return __buytradesum; }
            set { __buytradesum = value; }
        }

    }

    /// <summary>
    /// 商品交易信息的摘要说明。
    /// </summary>
    public class Goodstradeloginfo
    {
        private int _id;//交易日志ID
        /// <summary> 
        /// 交易日志ID
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _goodsid;//商品ID
        /// <summary> 
        /// 商品ID
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private string _orderid = "";//订单号ID
        /// <summary> 
        /// 订单号ID
        /// </summary>
        public string Orderid
        {
            get { return _orderid; }
            set { _orderid = value; }
        }

        private string _tradeno = "";//支付宝订单号
        /// <summary> 
        /// 支付宝订单号
        /// </summary>
        public string Tradeno 
        {
            get { return _tradeno; }
            set { _tradeno = value; }
        }

        private string _subject = "";//商品标题(对应支持宝)
        /// <summary> 
        /// 商品标题(对应支持宝)
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        private decimal _price;//价格
        /// <summary> 
        /// 价格
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _quality;//成色
        /// <summary> 
        /// 成色
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private int _categoryid;//商品类型
        /// <summary> 
        /// 商品类型
        /// </summary>
        public int Categoryid
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        private int _number;//数量
        /// <summary> 
        /// 数量
        /// </summary>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        private decimal _tax;//交易手续费
        /// <summary> 
        /// 交易手续费
        /// </summary>
        public decimal Tax
        {
            get { return _tax; }
            set { _tax = value; }
        }

        private string _locus = "";//物品所在地
        /// <summary> 
        /// 物品所在地
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private int _sellerid;//
        /// <summary> 
        /// 
        /// </summary>
        public int Sellerid
        {
            get { return _sellerid; }
            set { _sellerid = value; }
        }

        private string _seller = "";//卖家名
        /// <summary> 
        /// 卖家名
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private string _selleraccount = "";//卖家交易帐号
        /// <summary> 
        /// 卖家交易帐号
        /// </summary>
        public string Selleraccount
        {
            get { return _selleraccount; }
            set { _selleraccount = value; }
        }

        private int _buyerid;//买家ID
        /// <summary> 
        /// 买家ID
        /// </summary>
        public int Buyerid
        {
            get { return _buyerid; }
            set { _buyerid = value; }
        }

        private string _buyer = "";//买家名
        /// <summary> 
        /// 买家名
        /// </summary>
        public string Buyer
        {
            get { return _buyer; }
            set { _buyer = value; }
        }

        private string _buyercontact = "";//买家联系方式
        /// <summary> 
        /// 买家联系方式
        /// </summary>
        public string Buyercontact
        {
            get { return _buyercontact; }
            set { _buyercontact = value; }
        }

        private int _buyercredits;//买家暂扣积分
        /// <summary> 
        /// 买家暂扣积分
        /// </summary>
        public int Buyercredit
        {
            get { return _buyercredits; }
            set { _buyercredits = value; }
        }

        private string _buyermsg = "";//买家留言
        /// <summary> 
        /// 买家留言
        /// </summary>
        public string Buyermsg
        {
            get { return _buyermsg; }
            set { _buyermsg = value; }
        }



        private int _status;//状态,详细设置参见枚举类型:TradeStatus
        /// <summary> 
        /// 状态,详细设置参见枚举类型:TradeStatus
        /// </summary>
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        private DateTime _lastupdate;//状态最后更新
        /// <summary> 
        /// 状态最后更新
        /// </summary>
        public DateTime Lastupdate
        {
            get { return _lastupdate; }
            set { _lastupdate = value; }
        }

        private int _offline;//是否离线交易
        /// <summary> 
        /// 是否离线交易
        /// </summary>
        public int Offline
        {
            get { return _offline; }
            set { _offline = value; }
        }

        private string _buyername = "";//买家姓名
        /// <summary> 
        /// 买家姓名
        /// </summary>
        public string Buyername
        {
            get { return _buyername; }
            set { _buyername = value; }
        }

        private string _buyerzip = "";//买家邮编
        /// <summary> 
        /// 买家邮编
        /// </summary>
        public string Buyerzip
        {
            get { return _buyerzip; }
            set { _buyerzip = value; }
        }

        private string _buyerphone = "";//买家电话
        /// <summary> 
        /// 买家电话
        /// </summary>
        public string Buyerphone
        {
            get { return _buyerphone; }
            set { _buyerphone = value; }
        }

        private string _buyermobile = "";//买家手机
        /// <summary> 
        /// 买家手机
        /// </summary>
        public string Buyermobile
        {
            get { return _buyermobile; }
            set { _buyermobile = value; }
        }

        private int _transport;//物流类型: 0: VIRTUAL(虚拟物品), 1:POST(平邮),  2: EMS(EMS), 3:EXPRESS(其他快递公司)
        /// <summary> 
        /// 物流类型: 0: VIRTUAL(虚拟物品), 1:POST(平邮),  2: EMS(EMS), 3:EXPRESS(其他快递公司)
        /// </summary>
        public int Transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        private int _transportpay;//物流支付类型: SELLER_PAY(卖家支付物流费用,费用不计到总价内),BUYER_PAY(买家支付物流费用,费用需要计到总价内),BUYER_PAY_AFTER_RECEIVE(买家收到货后直接支付给物流公司，费用不用计到总价中)
        /// <summary> 
        /// 物流支付类型: SELLER_PAY(卖家支付物流费用,费用不计到总价内),BUYER_PAY(买家支付物流费用,费用需要计到总价内),BUYER_PAY_AFTER_RECEIVE(买家收到货后直接支付给物流公司，费用不用计到总价中)
        /// </summary>
        public int Transportpay
        {
            get { return _transportpay; }
            set { _transportpay = value; }
        }

        private decimal _transportfee;//运输费用
        /// <summary> 
        /// 运输费用
        /// </summary>
        public decimal Transportfee
        {
            get { return _transportfee; }
            set { _transportfee = value; }
        }

        private decimal _tradesum;//交易总额
        /// <summary> 
        /// 交易总额
        /// </summary>
        public decimal Tradesum
        {
            get { return _tradesum; }
            set { _tradesum = value; }
        }


        private decimal _baseprice;//商品原价
        /// <summary> 
        /// 商品原价
        /// </summary>
        public decimal Baseprice
        {
            get { return _baseprice; }
            set { _baseprice = value; }
        }

        private int _discount;//折扣
        /// <summary> 
        /// 折扣
        /// </summary>
        public int Discount
        {
            get { return _discount; }
            set { _discount = value; }
        }

        private int _ratestatus;//评价状态(0:双方未评 1:等待对方评价 2:评价对方 3:双方已评)
        /// <summary> 
        /// 评价状态(0:双方未评 1:等待对方评价 2:评价对方 3:双方已评)
        /// </summary>
        public int Ratestatus
        {
            get { return _ratestatus; }
            set { _ratestatus = value; }
        }

        private string _message = "";//订单留言
        /// <summary> 
        /// 订单留言
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
