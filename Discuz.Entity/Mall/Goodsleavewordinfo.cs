using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 商品留言类
    /// </summary>
    public class Goodsleavewordinfo
    {
        private int _id;//
        /// <summary> 
        /// 留言id
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
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

        private int _tradelogid;//交易日志id
        /// <summary> 
        /// 交易日志id
        /// </summary>
        public int Tradelogid
        {
            get { return _tradelogid; }
            set { _tradelogid = value; }
        }

        private int _isbuyer;//买家
        /// <summary> 
        /// 买家
        /// </summary>
        public int Isbuyer
        {
            get { return _isbuyer; }
            set { _isbuyer = value; }
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

        private string _username;//用户名
        /// <summary> 
        /// 用户名
        /// </summary>
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _message;//留言
        /// <summary> 
        /// 留言
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private int _invisible;//是否可见
        /// <summary> 
        /// 是否可见
        /// </summary>
        public int Invisible
        {
            get { return _invisible; }
            set { _invisible = value; }
        }

        private string _ip;//IP地址
        /// <summary> 
        /// IP地址
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private int _usesig;//是否启用签名
        /// <summary> 
        /// 是否启用签名
        /// </summary>
        public int Usesig
        {
            get { return _usesig; }
            set { _usesig = value; }
        }

        private int _htmlon;//是否支持html
        /// <summary> 
        /// 是否支持html
        /// </summary>
        public int Htmlon
        {
            get { return _htmlon; }
            set { _htmlon = value; }
        }

        private int _smileyoff;//是否关闭smaile表情
        /// <summary> 
        /// 是否关闭smaile表情
        /// </summary>
        public int Smileyoff
        {
            get { return _smileyoff; }
            set { _smileyoff = value; }
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

        private int _bbcodeoff;//是否允许bbcode
        /// <summary> 
        /// 是否允许bbcode
        /// </summary>
        public int Bbcodeoff
        {
            get { return _bbcodeoff; }
            set { _bbcodeoff = value; }
        }

        private DateTime _postdatetime;//留言日期
        /// <summary> 
        /// 留言日期
        /// </summary>
        public DateTime Postdatetime
        {
            get { return _postdatetime; }
            set { _postdatetime = value; }
        }
		
    }
}
