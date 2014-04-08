using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 店铺友情链接信息类
    /// </summary>
    public class Shoplinkinfo
    {
        private int _id;//店铺友情链接id
        /// <summary> 
        /// 店铺友情链接id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _shopid;//店铺id
        /// <summary> 
        /// 店铺id
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private int _displayorder;//显示顺序
        /// <summary> 
        /// 显示顺序
        /// </summary>
        public int Displayorder
        {
            get { return _displayorder; }
            set { _displayorder = value; }
        }

        private string _name = "";//名称
        /// <summary> 
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _name.Trim(); }
            set { _name = value.Trim(); }
        }

        private int _linkshopid;//链接到的shopid信息
        /// <summary> 
        /// 链接到的shopid信息
        /// </summary>
        public int Linkshopid
        {
            get { return _linkshopid; }
            set { _linkshopid = value; }
        }

       
    }
}
