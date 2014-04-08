using System;
using System.Data;

using Discuz.Config;

namespace Discuz.Plugin.Payment.Alipay
{
    /// <summary>
    /// 虚拟商品交易信息类
    /// </summary>
    public class DigitalTrade : Discuz.Plugin.Payment.ITrade
    {
        #region 成员变量

        protected string _input_charset = "utf-8";  //或gbk等
        protected string _agent;
        protected string _sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
        protected string _sign_type = "MD5";
        protected string _body;
        protected string _subject;
        protected string _out_trade_no;
        protected decimal _price;
        protected string _show_url;
        protected int _quantity;
        protected int _payment_type;
        protected string _partner = "2088002872555901";
        protected string _notify_url;
        protected string _return_url;
        protected string _seller_email;
        protected string _seller_id;
        protected string _buyer_email;
        protected string _buyer_id;
        protected string _buyer_msg;
        protected string _royaltyprice;
        protected string _pay_method = "bankPay";
        #endregion

        protected int userCustomPartnerId = GeneralConfigs.GetConfig().Usealipaycustompartnerid;



        #region 构造函数

        public DigitalTrade()
        {
            TradeConfigInfo tradeConfigInfo = TradeConfigs.GetConfig();
            _input_charset = tradeConfigInfo.Alipayconfiginfo.Inputcharset;
            _partner = tradeConfigInfo.Alipayconfiginfo.Partner;
            _sign = tradeConfigInfo.Alipayconfiginfo.Sign;
            //tradeConfigInfo.Alipayconfiginfo.Inputcharset = "utf-8";
            //tradeConfigInfo.Alipayconfiginfo.Partner = "2088002052150939";
            //tradeConfigInfo.Alipayconfiginfo.Sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
            TradeConfigs.SaveConfig(tradeConfigInfo);
        }

        #endregion

        #region 属性

        #region Discuz支付配置参数
        /// <summary>
        /// 支持平台类型
        /// </summary>
        public string _Type
        {
            get
            {
                return "alipay";
            }
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        public string _Action
        {
            get
            {
                return "create";
            }
        }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string _Product
        {
            get
            {
                return "Discuz!NT";
            }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public string _Version
        {
            get
            {
                return Discuz.Common.Utils.GetAssemblyVersion();
            }
        }
        #endregion

        #region 业务参数


        /// <summary>
        /// 商品描述 
        /// Alipay文档类型:string(400)
        /// </summary>
        public string Body
        {
            get { return _body; }
            set
            {
                if (value != null && value.Trim().Length > 400)
                {
                    //throw new ArgumentOutOfRangeException("无效的 Body(商品描述)", value, value.ToString());
                    _body = Discuz.Common.Utils.RemoveHtml(Discuz.Common.Utils.CutString(value, 0, 400));
                }
                else
                {
                    _body = value.Trim() == "" ? null : value.Trim();
                }
            }
        }

        /// <summary>
        /// 商品名称 
        /// Alipay文档类型:string(256)
        /// </summary>
        public string Subject
        {
            get
            {
                if (_subject == null)
                {
                    throw new ArgumentNullException(_subject);
                }
                return _subject;
            }
            set
            {
                if (value != null && value.Length > 256)
                {
                    //throw new ArgumentOutOfRangeException("无效的 Subject(商品名称)", value, value.ToString());
                    _subject = Discuz.Common.Utils.RemoveHtml(Discuz.Common.Utils.CutString(value, 0, 256));
                }
                else
                {
                    _subject = value.Trim() == "" ? null : value.Trim();
                }
            }
        }

        /// <summary>
        /// 外部交易号
        /// Alipay文档类型:string(64) 
        /// 合作伙伴交易号 (确保在合作伙伴系统中唯一)
        /// </summary>
        public string Out_Trade_No
        {
            get
            {
                if (_out_trade_no == null)
                {
                    //构造订单号 (形如:20080104140009iwGampfQkzFgMZ0yoT)
                    _out_trade_no = Discuz.Common.Utils.GetDateTime();
                    _out_trade_no = _out_trade_no.Replace("-", "");
                    _out_trade_no = _out_trade_no.Replace(":", "");
                    _out_trade_no = _out_trade_no.Replace(" ", "");

                    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    Random rnd = new Random();
                    for (int i = 1; i <= 18; i++)
                    {
                        _out_trade_no += chars.Substring(rnd.Next(chars.Length), 1);
                    }
                    return _out_trade_no;
                }
                return _out_trade_no;
            }
            set
            {
                if (value != null && value.Length > 64)
                {
                    throw new ArgumentOutOfRangeException("无效的 OutTradeNo(外部交易号)", value, value.ToString());
                }
                _out_trade_no = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 商品单价
        /// Alipay文档类型:Number(13,2) 
        /// 单位:元(RMB) 0.01--10000000.00  
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
        /// 默认支付方式（bankPay：网银；cartoon：一卡通；directPay：余额）
        /// </summary>
        public string PayMethod
        {
            get { return _pay_method; }
            set
            {
                if (value != "bankPay" && value != "cartoon" && value != "directPay")
                    throw new ArgumentOutOfRangeException("无效的默认支付方式", value, value.ToString());
                _pay_method = value.Trim();
            }
        }

        /// <summary>
        /// 商品展示网址 
        /// Alipay文档类型:string(400)
        /// </summary>
        public string Show_Url
        {
            get { return _show_url; }
            set
            {
                if (value != null && value.Length > 400)
                    throw new ArgumentOutOfRangeException("无效的 ShowUrl(商品展示网址)", value, value.ToString());
                _show_url = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 购买数量,不超过六位的正整数
        /// Alipay文档类型:Number(6,0)
        /// </summary>
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0 || value > 1000000)
                {
                    throw new ArgumentNullException(_quantity.ToString(), "Quantity(购买数量) 必须为小于1000000的正整数");
                }
                _quantity = value;
            }
        }

        /// <summary>
        /// 支付类型, 
        /// Alipay文档类型:此字段类型与alipay开发文档所要求的类型(为string)有差异
        /// 说明: 1:商品购买   2:服务购买   3:网络拍卖   4:捐赠   5:邮费补偿   6:奖金
        /// </summary>
        public int Payment_Type
        {
            get { return _payment_type; }
            set
            {
                if (value < 0 && value > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 PaymentType(支付类型)", value, value.ToString());
                }
                _payment_type = value;
            }
        }
        #endregion


        #region 协议参数

        public virtual string Service
        {
            get { return "create_direct_pay_by_user"; }
        }

        /// <summary>
        /// 如果一些交易网站的交易，有一定的“代理”所属关系，代理商可以在交易中传递该参数，来表明代理的身份。这里传送的值，请使用代理商所属支付宝账户的PartnerID
        /// </summary>
        public string Agent
        {
            get { return _agent; }
            set
            {
                if (value != null && value.Length > 16)
                    throw new ArgumentOutOfRangeException("无效的 Agent(代理)", value, value.ToString());
                _agent = value;
            }
        }

        /// <summary>
        /// 参数编码字符集,默认为"utf-8"(Alipay文档默认值为"GBK")
        /// </summary>
        public string Input_Charset
        {
            get
            {
                if (_input_charset == null)
                {
                    return "utf-8";
                }
                else
                {
                    return _input_charset;
                }
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 Input_Charset(参数编码字符集)", value, value.ToString());
                }
                _input_charset = value;
            }
        }


        /// <summary>
        /// 签名,在此输入交易安全校验码(key)
        /// </summary>
        public string Sign
        {
            get
            {
                if (_sign == null)
                {
                    _sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
                    //throw new ArgumentNullException(_sign);
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
        /// 签名方式,加密参数的算法如Md5或DSA(只实现了Md5)
        /// </summary>
        public string Sign_Type
        {
            get
            {
                //if (_sign_type == null)
                //{
                //    throw new ArgumentNullException(_sign_type);
                //}
                //return _sign_type;
                return "MD5";
            }
            //set
            //{
            //    if (value != null && value.Length > 50)
            //        throw new ArgumentOutOfRangeException("无效的 Sign_Type(签名方式)", value, value.ToString());
            //    _sign_type = value;
            //}
        }

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
                    _partner = "2088002872555901";
                    //throw new ArgumentNullException(_partner);
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
        /// 通知返回URL,仅适用于异步返回处理结果的接口。有些服务是无法立即返回处理结果的，那么需要通过这个URL将处理结果异步返回给合作伙。
        /// </summary>
        public string Notify_Url
        {
            get { return _notify_url; }
            set
            {
                _notify_url = value;
            }
        }


        /// <summary>
        /// 结果返回URL，仅适用于立即返回处理结果的接口。支付宝处理完请求后，立即将处理结果返回给这个URL
        /// Alipay文档类型:string
        /// </summary>
        public string Return_Url
        {
            get { return _return_url; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("无效的 Return_Url(结果返回URL)", value, value.ToString());
                _return_url = value;
            }
        }
        #endregion


        #region  买卖双方信息

        /// <summary>
        /// 卖家在支付宝的注册Email
        /// Alipay文档类型:string(100)
        /// </summary>
        public string Seller_Email
        {
            get
            {
                if (_seller_email == null && _seller_id == null)
                {
                    throw new ArgumentNullException(_seller_email, "Seller_Id,Seller_Email不能同时为空");
                }
                return _seller_email;
            }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("无效的 SellerEmail(卖家在支付宝的注册EMAIL)", value, value.ToString());
                }

                _seller_email = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 卖家在支付宝的注册ID
        /// Alipay文档类型:string(30)
        /// </summary>
        public string Seller_Id
        {
            get
            {
                if (_seller_email == null && _seller_id == null)
                {
                    throw new ArgumentNullException(_seller_id, "Seller_Id,Seller_Email不能同时为空");
                }
                return _seller_id;
            }
            set
            {
                if (value != null && value.Length > 30)
                    throw new ArgumentOutOfRangeException("无效的 SellerId(卖家在支付宝的注册ID)", value, value.ToString());

                _seller_id = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 买家在支付宝的注册Email
        /// Alipay文档类型:string(100)
        /// </summary>
        public string Buyer_Email
        {
            get { return _buyer_email; }
            set
            {
                if (value != null && value.Length > 100)
                    throw new ArgumentOutOfRangeException("无效的 BuyerEmail(买家在支付宝的注册Email)", value, value.ToString());
                _buyer_email = value.Trim() == "" ? null : value.Trim();
            }
        }
        /// <summary>
        /// 买家在支付宝的注册ID
        /// Alipay文档类型:string(30) 
        /// </summary>
        public string Buyer_Id
        {
            get { return _buyer_id; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("无效的 BuyerId(买家在支付宝的注册ID)", value, value.ToString());
                _buyer_id = value.Trim() == "" ? null : value.Trim();
            }
        }
        /// <summary>
        /// 买家留言
        /// Alipay文档类型:string(200)
        /// </summary>
        public string Buyer_Msg
        {
            get { return _buyer_msg; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentOutOfRangeException("无效的 BuyerMsg(买家留言)", value, value.ToString());
                _buyer_msg = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 分账参数
        /// </summary>
        public string Royalty_Type
        {
            get
            {
                if (userCustomPartnerId == 0 && Math.Round(Convert.ToDouble(Price) * 0.015, 2) > 0)
                {
                    return "10";
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 分账参数
        /// </summary>
        public string Royalty_Parameters
        {
            get
            {
                if (userCustomPartnerId == 0 && Math.Round(Convert.ToDouble(Price) * 0.015, 2) > 0)
                {
                    return "comsenz@comsenz.com^" + Math.Round(Convert.ToDouble(Price) * 0.015, 2) + "^手续费";
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion




        #endregion
    }
}
