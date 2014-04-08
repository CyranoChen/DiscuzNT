using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品信息参数,继承自 <see cref="Discuz.Entity.PostpramsInfo"/> PostpramsInfo
    /// </summary>
    public class GoodspramsInfo : PostpramsInfo
    {
        public int __goodsid;

        /// <summary>
        /// 商品id
        /// </summary>
        public int Goodsid
        {
            set { __goodsid = value; }
            get { return __goodsid; }
        }
    }

    /// <summary>
    /// 商品信息的摘要说明。
    /// </summary>
    public class Goodsinfo
    {
        private int _goodsid;//商品ID
        /// <summary> 
        /// 商品ID
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private int _shopid;//店铺ID
        /// <summary> 
        /// 店铺ID
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private int _categoryid;//商品分类
        /// <summary> 
        /// 商品分类
        /// </summary>
        public int Categoryid
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        private string _parentcategorylist;//商品所属大类列表,冗余categories表中的parentidlist字段
        /// <summary> 
        /// 商品所属大类列表,冗余categories表中的parentidlist字段
        /// </summary>
        public string Parentcategorylist
        {
            get { return _parentcategorylist; }
            set { _parentcategorylist = value; }
        }

        private string _shopcategorylist = "";// 店铺商品分类. 格式(1,3,4)
        /// <summary> 
        /// 店铺商品分类. 格式(1,3,4)
        /// </summary>
        public string Shopcategorylist
        {
            get { return _shopcategorylist; }
            set { _shopcategorylist = value; }
        }

        private int _recommend;//是否是推荐商品(0为不推荐,)
        /// <summary> 
        /// 是否是推荐商品(0为不推荐,)
        /// </summary>
        public int Recommend
        {
            get { return _recommend; }
            set { _recommend = value; }
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

        private int _selleruid;//卖家uid
        /// <summary> 
        /// 卖家uid
        /// </summary>
        public int Selleruid
        {
            get { return _selleruid; }
            set { _selleruid = value; }
        }

        private string _seller;//卖家帐号(对应支付宝)
        /// <summary> 
        /// 卖家帐号(对应支付宝)
        /// </summary>
        public string Seller
        {
            get { return _seller; }
            set { _seller = value; }
        }

        private string _account;//卖家帐号(对应支付宝)
        /// <summary> 
        /// 卖家帐号(对应支付宝)
        /// </summary>
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private string _title = "";//标题(对应支付宝:subject)
        /// <summary> 
        /// 标题(对应支付宝:subject)
        /// </summary>
        public string Title
        {
            get { return _title.Trim(); }
            set { _title = value.Trim(); }
        }

        private int _magic;//
        /// <summary> 
        /// 
        /// </summary>
        public int Magic
        {
            get { return _magic; }
            set { _magic = value; }
        }

        private decimal _price;//价格(现价)
        /// <summary> 
        /// 价格(现价)
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _amount;//数量
        /// <summary> 
        /// 数量
        /// </summary>
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private int _quality;//成色(质量)(1: 全新, 2:二手)
        /// <summary> 
        /// 成色(质量)(1: 全新, 2:二手)
        /// </summary>
        public int Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private int _lid;//关联locations表中的id
        /// <summary> 
        /// 关联locations表中的id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _locus;//所在地,格式为"(国家),省(州),市"
        /// <summary> 
        /// 所在地,格式为"(国家),省(州),市"
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private int _transport;//物流方式
        /// <summary> 
        /// 物流方式
        /// 类型: 0 虚拟物品或无需邮递
        ///       1 卖家承担运费
        ///       2 买家承担运费  
        /// 3 支付给物流公司
        /// </summary>
        public int Transport
        {
            get { return _transport; }
            set { _transport = value; }
        }

        private decimal _ordinaryfee;//平邮附加费
        /// <summary> 
        /// 平邮附加费
        /// </summary>
        public decimal Ordinaryfee
        {
            get { return _ordinaryfee; }
            set { _ordinaryfee = value; }
        }

        private decimal _expressfee;//快递附加费
        /// <summary> 
        /// 快递附加费
        /// </summary>
        public decimal Expressfee
        {
            get { return _expressfee; }
            set { _expressfee = value; }
        }

        private decimal _emsfee;//EMS附加费
        /// <summary> 
        /// EMS附加费
        /// </summary>
        public decimal Emsfee
        {
            get { return _emsfee; }
            set { _emsfee = value; }
        }

        private int _itemtype;//商品类型(商品,服务,拍卖,捐赠,邮费,奖金)
        /// <summary> 
        /// 商品类型(商品,服务,拍卖,捐赠,邮费,奖金)
        /// </summary>
        public int Itemtype
        {
            get { return _itemtype; }
            set { _itemtype = value; }
        }

        private DateTime _dateline;//开始时间
        /// <summary> 
        /// 开始时间
        /// </summary>
        public DateTime Dateline
        {
            get { return _dateline; }
            set { _dateline = value; }
        }

        private DateTime _expiration;//过期时间
        /// <summary> 
        /// 过期时间
        /// </summary>
        public DateTime Expiration
        {
            get { return _expiration; }
            set { _expiration = value; }
        }

        private string _lastbuyer = "";//最后买家用户名
        /// <summary> 
        /// 最后买家用户名
        /// </summary>
        public string Lastbuyer
        {
            get { return _lastbuyer; }
            set { _lastbuyer = value.Trim(); }
        }

        private DateTime _lasttrade;//最后交易时间
        /// <summary> 
        /// 最后交易时间
        /// </summary>
        public DateTime Lasttrade
        {
            get { return _lasttrade; }
            set { _lasttrade = value; }
        }

        private DateTime _lastupdate;//最后更新时间
        /// <summary> 
        /// 最后更新时间
        /// </summary>
        public DateTime Lastupdate
        {
            get { return _lastupdate; }
            set { _lastupdate = value; }
        }

        private int _totalitems;//总交易量
        /// <summary> 
        /// 总交易量
        /// </summary>
        public int Totalitems
        {
            get { return _totalitems; }
            set { _totalitems = value; }
        }

        private decimal _tradesum;//总交易额
        /// <summary> 
        /// 总交易额
        /// </summary>
        public decimal Tradesum
        {
            get { return _tradesum; }
            set { _tradesum = value; }
        }

        private int _closed;//是否关闭
        /// <summary> 
        /// 是否关闭 1:为关闭 0: 不关闭
        /// </summary>
        public int Closed
        {
            get { return _closed; }
            set { _closed = value; }
        }

        private int _aid;//商品图片的aid
        /// <summary> 
        /// 商品图片的aid
        /// </summary>
        public int Aid
        {
            get { return _aid; }
            set { _aid = value; }
        }

        private string _goodspic = "";//商品图片路径(关联aid)
        /// <summary> 
        /// 商品图片路径(关联aid)
        /// </summary>
        public string Goodspic
        {
            get { return _goodspic; }
            set { _goodspic = value; }
        }

        private int _displayorder;//显示顺序 0为正常 <0不显示   -1为回收站   -2待审核  -3未上架(不可见)
        /// <summary> 
        /// 显示顺序 0为正常 <0不显示   -1为回收站   -2待审核  -3未上架(不可见)
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        private decimal _costprice;//商品原价
        /// <summary> 
        /// 商品原价
        /// </summary>
        public decimal Costprice
        {
            get { return _costprice; }
            set { _costprice = value; }
        }

        private int _invoice;//有无发票(0:无,1:有)
        /// <summary> 
        /// 有无发票(0:无,1:有)
        /// </summary>
        public int Invoice
        {
            get { return _invoice; }
            set { _invoice = value; }
        }

        private int _repair;//保修(单位:天 0为不保修)
        /// <summary> 
        /// 保修(单位:天 0为不保修)
        /// </summary>
        public int Repair
        {
            get { return _repair; }
            set { _repair = value; }
        }

        private string _message;//详细信息
        /// <summary> 
        /// 详细信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _otherlink;//其它链接(可链接到第三方如TAOBAO等)
        /// <summary> 
        /// 其它链接(可链接到第三方如TAOBAO等)
        /// </summary>
        public string Otherlink
        {
            get { return _otherlink; }
            set { _otherlink = value; }
        }

        private int _readperm;//阅读权限
        /// <summary> 
        /// 阅读权限
        /// </summary>
        public int Readperm
        {
            get { return _readperm; }
            set { _readperm = value; }
        }

        private int _tradetype;//交易方式( 1:支付宝在线交易  0:线下交易 或第三方)
        /// <summary> 
        /// 交易方式( 支付宝在线交易  线下交易 或第三方)
        /// </summary>
        public int Tradetype
        {
            get { return _tradetype; }
            set { _tradetype = value; }
        }

        private int _tagid;//tags标签
        /// <summary> 
        /// tags标签
        /// </summary>
        public int Tagid
        {
            get { return _tagid; }
            set { _tagid = value; }
        }

        private int _viewcount;//浏览次数
        /// <summary> 
        /// 浏览次数
        /// </summary>
        public int Viewcount
        {
            get { return _viewcount; }
            set { _viewcount = value; }
        }

        //private int _invisible;//是否上架 0:上架(可见) 1:
        ///// <summary> 
        ///// 是否上架 0:上架(可见) 1:未上架(不可见)
        ///// </summary>
        //public int Invisible
        //{
        //    get { return _invisible; }
        //    set { _invisible = value; }
        //}

        private int _smileyoff;//
        ///<summary>
        ///是否关闭smile表情
        ///</summary>
        public int Smileyoff
        {
            get { return _smileyoff; }
            set { _smileyoff = value; }
        }

        private int _bbcodeoff;//是否支持UBB
        /// <summary> 
        /// 是否支持UBB
        /// </summary>
        public int Bbcodeoff
        {
            get { return _bbcodeoff; }
            set { _bbcodeoff = value; }
        }

        private int _parseurloff;//是否关闭url自动解析
        /// <summary> 
        /// 是否关闭url自动解析
        /// </summary>
        public int Parseurloff
        {
            get { return _parseurloff; }
            set { _parseurloff = value; }
        }


        private string _highlight = "";//高亮样式代码
        /// <summary> 
        /// 高亮样式代码
        /// </summary>
        public string Highlight
        {
            get { return _highlight; }
            set { _highlight = value; }
        }

        private string _htmltitle = "";//标题(可含html代码,非数据库字段)
        /// <summary> 
        /// 显示标题(可含html代码,非数据库字段)
        /// </summary>
        public string Htmltitle
        {
            get { return _htmltitle; }
            set { _htmltitle = value; }
        }
    }

    /// <summary>
    /// 商品信息显示列表类
    /// </summary>
    //public class ShowGoodsinfoList : GoodsInfo
    //{

    //}
}


