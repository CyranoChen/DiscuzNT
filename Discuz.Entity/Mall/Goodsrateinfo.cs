using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品评价类
    /// </summary>
    public class Goodsrateinfo
    {
        private int _id;
        /// <summary> 
        /// 评价id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _goodstradelogid;//商品交易日志ID
        /// <summary> 
        /// 商品交易日志ID
        /// </summary>
        public int Goodstradelogid
        {
            get { return _goodstradelogid; }
            set { _goodstradelogid = value; }
        }

        private string _message;//评价内容
        /// <summary> 
        /// 评价内容
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _explain;//解释内容
        /// <summary> 
        /// 解释内容
        /// </summary>
        public string Explain
        {
            get { return _explain; }
            set { _explain = value; }
        }

        private string _ip;//ip
        /// <summary> 
        /// ip
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _uid;//用户id
        /// <summary> 
        /// 用户id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private int _uidtype;//用户id类型 1:卖家 2:买家
        /// <summary> 
        /// 用户id类型 1:卖家 2:买家
        /// </summary>
        public int Uidtype
        {
            get { return _uidtype; }
            set { _uidtype = value; }
        }


        private int _ratetouid;//被评价人的uid
        /// <summary> 
        /// 被评价人的uid
        /// </summary>
        public int Ratetouid
        {
            get { return _ratetouid; }
            set { _ratetouid = value; }
        }


        private string _ratetousername;//被评价人的用户名
        /// <summary> 
        /// 被评价人的用户名
        /// </summary>
        public string Ratetousername
        {
            get { return _ratetousername; }
            set { _ratetousername = value; }
        }
        

        private string _username;//用户名
        /// <summary> 
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private DateTime _postdatetime;//评价日期
        /// <summary> 
        /// 评价日期
        /// </summary>
        public DateTime Postdatetime
        {
            get { return _postdatetime; }
            set { _postdatetime = value; }
        }

        private int _goodsid;//商品id
        /// <summary> 
        /// 商品id
        /// </summary>
        public int Goodsid
        {
            get { return _goodsid; }
            set { _goodsid = value; }
        }

        private string _goodstitle;//商品标题
        /// <summary> 
        /// 商品标题
        /// </summary>
        public string Goodstitle
        {
            get { return _goodstitle; }
            set { _goodstitle = value; }
        }

        private decimal _price;//交易价格
        /// <summary> 
        /// 交易价格
        /// </summary>
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _ratetype;//评价类型 1:好评 2:中评 3:差评 
        /// <summary> 
        /// 评价类型 1:好评 2:中评 3:差评 
        /// </summary>
        public int Ratetype
        {
            get { return _ratetype; }
            set { _ratetype = value; }
        }
    }
}
