using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品用户评价信息类
    /// </summary>
    public class Goodsusercreditinfo
    {
        private int _id;
        /// <summary> 
        /// 
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

      
        private int _oneweek;//一周内
        /// <summary> 
        /// 一周内
        /// </summary>
        public int Oneweek
        {
            get { return _oneweek; }
            set { _oneweek = value; }
        }

        private int _onemonth;//一个月内
        /// <summary> 
        /// 一个月内
        /// </summary>
        public int Onemonth
        {
            get { return _onemonth; }
            set { _onemonth = value; }
        }

        private int _sixmonth;//六个月内
        /// <summary> 
        /// 六个月内
        /// </summary>
        public int Sixmonth
        {
            get { return _sixmonth; }
            set { _sixmonth = value; }
        }

        private int _sixmonthago;//六个月以前
        /// <summary> 
        /// 六个月以前
        /// </summary>
        public int Sixmonthago
        {
            get { return _sixmonthago; }
            set { _sixmonthago = value; }
        }

        private int _ratefrom;//评价来自 1:卖家 2:买家
        /// <summary> 
        /// 评价来自 1:卖家 2:买家
        /// </summary>
        public int Ratefrom
        {
            get { return _ratefrom; }
            set { _ratefrom = value; }
        }

        private int _ratetype;//评价类型 1:好评 2:中评 3:差评 
        /// <summary> 
        ///评价类型 1:好评 2:中评 3:差评 
        /// </summary>
        public int Ratetype
        {
            get { return _ratetype; }
            set { _ratetype = value; }
        }


    }
}
