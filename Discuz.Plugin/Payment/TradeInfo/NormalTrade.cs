using System;
using System.Data;

namespace Discuz.Plugin.Payment.Alipay
{
    /// <summary>
    /// 实物商品交易信息类
    /// </summary>
    public class NormalTrade : DigitalTrade
    {
        
        #region 成员变量
       
        private decimal _discount;
        private LogisticsInfo[] _logistics_info;
        private string _receive_name;
        private string _receive_address;
        private string _receive_zip;
        private string _receive_phone;
        private string _receive_mobile;
    
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public NormalTrade() : base()
        { }
        #endregion

        #region 属性


        #region 业务参数

        /// <summary>
        /// 折扣
        /// Alipay文档类型:Number(8,2) 
        /// -10000000.00--10000000.00
        /// </summary>
        public decimal Discount
        {
            get
            {
                _discount = decimal.Round(_discount, 2);
                return _discount;
            }
            set
            {
                if (value < -10000000.00m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_discount.ToString(), "Discount(折扣) 必须为-10000000.00在100000000.00之间");
                }
                _discount = value;
            }
        }

        #endregion


        #region 协议参数

        public override string Service
        {
            get { return "trade_create_by_buyer"; }
        }

        #endregion


        #region 物流信息


        /// <summary>
        /// 物流信息集合属性,详见<see cref="Discuz.Payment.Alipay.GoodsTradeInfo.LogisticsInfo"/>类
        /// </summary>
        public LogisticsInfo[] Logistics_Info
        {
            get { return _logistics_info; }
            set
            {
                if (value != null && value.Length <=0 )
                {
                    throw new ArgumentOutOfRangeException("无效的 物流信息(收货人姓名)", value, value.ToString());
                }
                _logistics_info = value;
            }
        }

        /// <summary>
        /// 收货人姓名
        /// Alipay文档类型:string(128)
        /// </summary>
        public string Receive_Name
        {
            get { return _receive_name; }
            set
            {
                if (value != null && value.Length > 128)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Name(收货人姓名)", value, value.ToString());
                }
                _receive_name = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 收货人地址
        /// Alipay文档类型:string(256)
        /// </summary>
        public string Receive_Address
        {
            get { return _receive_address; }
            set
            {
                if (value != null && value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Address(收货人地址)", value, value.ToString());
                }
                _receive_address = value.Trim() == "" ? null : value.Trim();
            }
        }
         
        /// <summary>
        /// 收货人邮编
        /// Alipay文档类型:string(6)
        /// </summary>
        public string Receive_Zip
        {
            get { return _receive_zip; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Zip(收货人邮编)", value, value.ToString());
                }
                _receive_zip = value.Trim()==""? null: value.Trim();
            }
        }


        /// <summary>
        /// 收货人电话
        /// Alipay文档类型:string(30)
        /// </summary>
        public string Receive_Phone
        {
            get { return _receive_phone; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Phone(收货人电话)", value, value.ToString());
                }
                _receive_phone = value.Trim() == "" ? null : value.Trim();
            }
        }

        /// <summary>
        /// 收货人手机
        /// Alipay文档类型:string(11)
        /// </summary>
        public string Receive_Mobile
        {
            get { return _receive_mobile; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Phone(收货人手机)", value, value.ToString());
                }
                _receive_mobile = value.Trim() == "" ? null : value.Trim();
            }
        }
      
        /// <summary>
        /// 分账参数
        /// </summary>
        public new string Royalty_Type
        {
            get { return null; }
        }
        /// <summary>
        /// 分账参数
        /// </summary>
        public new string Royalty_Parameters
        {
            get { return null; }
        }

        #endregion 

        #endregion
    }
}
