using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 店铺信息类
    /// </summary>
    public class Shopinfo
    {
        private int _shopid;//店铺id
        /// <summary> 
        /// 店铺id
        /// </summary>
        public int Shopid
        {
            get { return _shopid; }
            set { _shopid = value; }
        }

        private string _logo= "";//店标
        /// <summary> 
        /// 店标
        /// </summary>
        public string Logo
        {
            get { return _logo.Trim(); }
            set { _logo = value.Trim(); }
        }

        private string _shopname;//店铺名称
        /// <summary> 
        /// 店铺名称
        /// </summary>
        public string Shopname
        {
            get { return _shopname; }
            set { _shopname = value; }
        }

        private int _themeid;//主题
        /// <summary> 
        /// 主题
        /// </summary>
        public int Themeid
        {
            get { return _themeid; }
            set { _themeid = value; }
        }

        private string _themepath;//主题路径
        /// <summary> 
        /// 主题路径
        /// </summary>
        public string Themepath
        {
            get { return _themepath; }
            set { _themepath = value; }
        }

        private int _uid;//用户(掌柜)id
        /// <summary> 
        /// 用户(掌柜)id
        /// </summary>
        public int Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private string _username;//用户(掌柜)名
        /// <summary> 
        /// 用户(掌柜)名
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _introduce;//店铺介绍
        /// <summary> 
        /// 店铺介绍
        /// </summary>
        public string Introduce
        {
            get { return _introduce; }
            set { _introduce = value; }
        }

        private int _lid;//所在地id
        /// <summary> 
        /// 所在地id
        /// </summary>
        public int Lid
        {
            get { return _lid; }
            set { _lid = value; }
        }

        private string _locus;//所在地
        /// <summary> 
        /// 所在地
        /// </summary>
        public string Locus
        {
            get { return _locus; }
            set { _locus = value; }
        }

        private string _bulletin;//公告
        /// <summary> 
        /// 公告
        /// </summary>
        public string Bulletin
        {
            get { return _bulletin; }
            set { _bulletin = value; }
        }

        private DateTime _createdatetime;//创店时间
        /// <summary> 
        /// 创店时间
        /// </summary>
        public DateTime Createdatetime
        {
            get { return _createdatetime; }
            set { _createdatetime = value; }
        }

        private int _invisible;//是否可见(0:可见, 1:不可见)
        /// <summary> 
        /// 是否可见(0:可见, 1:不可见)
        /// </summary>
        public int Invisible
        {
            get { return _invisible; }
            set { _invisible = value; }
        }

        private int _viewcount;//访问量
        /// <summary> 
        /// 访问量
        /// </summary>
        public int Viewcount
        {
            get { return _viewcount; }
            set { _viewcount = value; }
        }
    }
}
