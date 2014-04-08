using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Discuz.Plugin.Payment.Alipay
{
    /// <summary>
    /// 物流信息类
    /// </summary>
    public class LogisticsInfo
    {
        private string _logistics_type;
        private decimal _logistics_fee;
        private string _logistics_payment;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logistics_type">物流类型:  VIRTUAL(虚拟物品),POST(平邮),EMS(EMS),EXPRESS(其他快递公司)</param>
        /// <param name="logistics_fee">物流费用, 默认为0</param>
        /// <param name="logistics_payment">物流支付类型: SELLER_PAY(卖家支付物流费用,费用不计到总价内),BUYER_PAY(买家支付物流费用,费用需要计到总价内),BUYER_PAY_AFTER_RECEIVE(买家收到货后直接支付给物流公司，费用不用计到总价中)</param>
        public LogisticsInfo(string logistics_type, decimal logistics_fee, string logistics_payment)
        {
            this._logistics_type = logistics_type;
            this._logistics_fee = logistics_fee;
            this._logistics_payment = logistics_payment;
        }


        /// <summary>
        /// 物流类型   
        /// Alipay文档类型:string 
        /// 说明: VIRTUAL:虚拟物品  POST:平邮   EMS:EMS   EXPRESS:其他快递公司
        /// </summary>
        public string Logistics_Type
        {
            get { return _logistics_type; }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 LogisticsType(物流类型)", value, value.ToString());
                }
                _logistics_type = value;
            }
        }

        /// <summary>
        /// 物流费用 
        /// Alipay文档类型:Number(8,2)
        /// 0.00--10000000.00默认为0
        /// </summary>
        public decimal Logistics_Fee
        {
            get
            {
                _logistics_fee = decimal.Round(_logistics_fee, 2);
                return _logistics_fee;
            }
            set
            {
                if (value < 0.00m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(_logistics_fee.ToString(), "Price(商品单价) 必须为0.01在100000000.00之间");
                }
                _logistics_fee = value;
            }
        }

        /// <summary>
        /// 物流支付类型 
        /// Alipay文档类型:string
        /// 说明: SELLER_PAY 卖家支付(卖家支付物流费用,费用不计到总价内)
        ///		  BUYER_PAY 买家支付(买家支付物流费用,费用需要计到总价内)
        ///    	  BUYER_PAY_AFTER_RECEIVE 货到付款(买家收到货后直接支付给物流公司，费用不用计到总价中)
        /// </summary>
        public string Logistics_Payment
        {
            get { return _logistics_payment; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("无效的 LogisticsPayment(物流支付类型)", value, value.ToString());
                _logistics_payment = value;
            }
        }
    }
}
