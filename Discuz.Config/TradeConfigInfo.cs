using System;
using System.Text;

namespace Discuz.Config
{
    
    /// <summary>
    /// 交易配置信息类
    /// </summary>
    [Serializable]
    public class TradeConfigInfo : IConfigInfo
    {
        /// <summary>
        /// 支付宝配置信息
        /// </summary>
        private AliPayConfigInfo _alipayconfiginfo;
        /// <summary>
        /// 支付宝配置信息
        /// </summary>
        public AliPayConfigInfo Alipayconfiginfo
        {
            get {return _alipayconfiginfo;}
            set { _alipayconfiginfo = value; }
        }
    }

     
    /// <summary>
    /// 支付宝配置信息类
    /// </summary>
    [Serializable]
    public class AliPayConfigInfo 
    {
        #region 私有字段
        private string _inputCharset = "utf-8";
        private string _partner = "2088002052150939"; //Discuz Partner ID
        private string _sign = "gh0bis45h89m5mwcoe85us4qrwispes0"; //Discuz 交易安全校验码(key)
        private string _acount = ""; //支付宝帐号(用于兑换或交易功能)
        #endregion

        #region 属性
        /// <summary>
        /// 参数编码字符集,默认为"utf-8"(Alipay文档默认值为"GBK")
        /// </summary>
        public string Inputcharset
        {
            get
            {
                if (_inputCharset == null)
                {
                    return "utf-8";
                }
                else
                {
                    return _inputCharset;
                }
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 Input_Charset(参数编码字符集)", value, value.ToString());
                }
                _inputCharset = value;
            }
        }

        #region 注释的代码
        //private string _agent = "";

        ///// <summary>
        ///// 如果一些交易网站的交易，有一定的“代理”所属关系，代理商可以在交易中传递该参数，来表明代理的身份。这里传送的值，请使用代理商所属支付宝账户的PartnerID
        ///// </summary>
        //public string Agent
        //{
        //    get { return _agent; }
        //    set
        //    {
        //        if (value != null && value.Length > 16)
        //            throw new ArgumentOutOfRangeException("无效的 Agent(代理)", value, value.ToString());
        //        _agent = value;
        //    }
        //}
        #endregion

        /// <summary>
        /// 合作伙伴在支付宝的用户ID
        /// Alipay文档类型:string(16)
        /// </summary>
        public string Partner
        {
            get
            {
                if (_partner == null)
                {
                    throw new ArgumentNullException(_partner);
                }
                return _partner;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("无效的 Partner(合作伙伴ID)", value, value.ToString());
                _partner = value;
            }
        }


        /// <summary>
        /// 签名,在此输入交易安全校验码（key），不同于支付宝文档的Sign
        /// </summary>
        public string Sign
        {
            get
            {
                if (_sign == null)
                {
                    throw new ArgumentNullException(_sign);
                }
                return _sign;
            }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("无效的 Sign(签名)", value, value.ToString());
                _sign = value;
            }
        }

        /// <summary>
        /// 支付宝帐号(用于兑换或交易功能)
        /// </summary>
        public string Acount
        {
            get
            {
                if (_acount == null)
                {
                    throw new ArgumentNullException(_acount);
                }
                return _acount;
            }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("无效的支付宝帐号", value, value.ToString());
                _acount = value;
            }
        }
        #endregion
    }
}
