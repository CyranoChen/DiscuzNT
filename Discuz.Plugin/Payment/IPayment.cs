using System;

namespace Discuz.Plugin.Payment
{
    public interface IPayment
    {
        /// <summary>
        /// 支付网关的地址
        /// </summary>
        string PayUrl { get;}

        /// <summary>
        /// 创建虚拟商品交易URL地址
        /// </summary>
        /// <param name="_goods">商品信息</param>
        /// <returns></returns>
        string CreateDigitalGoodsTradeUrl(ITrade _goods);

        /// <summary>
        /// 创建普通(实物)交易URL地址
        /// </summary>
        /// <param name="_goods">商品信息</param>
        /// <returns></returns>
        string CreateNormalGoodsTradeUrl(ITrade _goods);
      
    }
}
