using System;
using System.IO;
using System.Net;
using System.Text;

using Discuz.Common;
using Discuz.Config;
using System.Security.Cryptography;

namespace Discuz.Forum
{
    public class EPayments
    {
        /// <summary>
        /// 检查支付结果
        /// </summary>
        /// <returns></returns>
        public static bool CheckPayment(string notifyId)
        {
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();

            string partnerid = configInfo.Alipaypartnerid != "" ? configInfo.Alipaypartnerid : "2088002872555901";//如果config中关于合作partnerid的设置为空，则使用comsenz提供的partnerid

            string aliPayNotifyUrl1 = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify&";
            string aliPayNotifyUrl2 = "http://notify.alipay.com/trade/notify_query.do?";

            string responseText = GetHttp(string.Format("{0}partner={1}&notify_id={2}", aliPayNotifyUrl1, partnerid, notifyId));

            if (responseText != "true")//可能是由于上一个地址失效，则用第二条ATN线路再次验证
                responseText = GetHttp(string.Format("{0}partner={1}&notify_id={2}", aliPayNotifyUrl2, partnerid, notifyId));

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            if (responseText != "true")
                return false;

            //如果返回信息是get式的，则无需验证参数是否被篡改，因为notifyid只能被校验一次，支付宝校验之后即失效
            if (DNTRequest.IsGet())
                return true;


            //排序
            string[] sortedStr = System.Web.HttpContext.Current.Request.Form.AllKeys;
            //string[] sortedStr = System.Web.HttpContext.Current.Request.QueryString.AllKeys;  本机测试时需要从QueryString中获得回传参数

            //构造Post的数据串
            StringBuilder urlParam = new StringBuilder();

            if (configInfo.Usealipaycustompartnerid == 0)
            {
                string aliPayNotifyUrl = "http://pay.discuz.net/gateway/alipay.php?_type=alipay&_action=verify&_product=Discuz!NT&_version=" + Discuz.Common.Utils.GetAssemblyVersion();
                for (int i = 0; i < sortedStr.Length; i++)
                {
                    if (DNTRequest.GetString(sortedStr[i]) != "")
                    {
                        if (urlParam.ToString() == "")
                            urlParam.Append(sortedStr[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(sortedStr[i])));
                        else
                            urlParam.Append("&" + sortedStr[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(sortedStr[i])));
                    }
                }
                //提交到discuz支付网关
                return GetHttp(aliPayNotifyUrl, urlParam.ToString()) == "true";
            }
            else
            {
                for (int i = 0; i < sortedStr.Length; i++)
                {
                    if (DNTRequest.GetString(sortedStr[i]) != "" && sortedStr[i] != "sign" && sortedStr[i] != "sign_type")
                    {
                        if (urlParam.ToString() == "")
                            urlParam.Append(sortedStr[i] + "=" + DNTRequest.GetString(sortedStr[i]));
                        else
                            urlParam.Append("&" + sortedStr[i] + "=" + DNTRequest.GetString(sortedStr[i]));
                    }
                }
                urlParam.Append(configInfo.Alipaypartnercheckkey);
                return GetMD5(urlParam.ToString(), "utf-8") == DNTRequest.GetString("sign");
            }
        }

        /// <summary>
        /// 将支付宝返回的交易状态翻译为论坛积分交易状态
        /// </summary>
        /// <param name="alipayStatus"></param>
        /// <returns></returns>
        public static int ConvertAlipayTradeStatus(string alipayStatus)
        {
            int status = 0;
            switch (alipayStatus)
            {
                case "WAIT_BUYER_PAY": // 等待买家付款
                    {
                        status = 0; break;
                    }
                case "TRADE_FINISHED": // 交易成功结束
                    {
                        status = 2; break;
                    }

                #region 其他的支付宝返回信息，留待以后使用
                //case "WAIT_SELLER_CONFIRM_TRADE": // 交易已创建，等待卖家确认
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_CONFIRM_TRADE; break;
                //    }
                //case "WAIT_SYS_CONFIRM_PAY":　// 确认买家付款中，暂勿发货
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SYS_CONFIRM_PAY; break;
                //    }
                //case "WAIT_SELLER_SEND_GOODS": // 支付宝收到买家付款，请卖家发货
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_SEND_GOODS; break;
                //    }
                //case "WAIT_BUYER_CONFIRM_GOODS": //  卖家已发货，买家确认中
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_BUYER_CONFIRM_GOODS; break;
                //    }
                //case "WAIT_SYS_PAY_SELLER": // 买家确认收到货，等待支付宝打款给卖家
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SYS_PAY_SELLER; break;
                //    }
                //case "TRADE_CLOSED": //  交易中途关闭(未完成)
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.TRADE_CLOSED; break;
                //    }
                //case "WAIT_SELLER_AGREE": //  等待卖家同意退款
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_AGREE; break;
                //    }
                //case "SELLER_REFUSE_BUYER": // 卖家拒绝买家条件，等待买家修改条件
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.SELLER_REFUSE_BUYER; break;
                //    }
                //case "WAIT_BUYER_RETURN_GOODS": // 卖家同意退款，等待买家退货
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_BUYER_RETURN_GOODS; break;
                //    }
                //case "WAIT_SELLER_CONFIRM_GOODS": // 等待卖家收货
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.WAIT_SELLER_CONFIRM_GOODS; break;
                //    }
                //case "REFUND_SUCCESS": //  退款成功
                //    {
                //        goodstradeloginfo.Status = (int)TradeStatusEnum.REFUND_SUCCESS; break;
                //    }
                #endregion

                default:
                    break;
            }
            return status;
        }

        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        public static string GetMD5(string str, string inputCharset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(str));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 是否开启网上支付功能
        /// </summary>
        /// <returns></returns>
        public static bool IsOpenEPayments()
        {
            bool isOpen = true;
            GeneralConfigInfo configInfo = GeneralConfigs.GetConfig();
            if (configInfo.Cashtocreditrate <= 0)
                isOpen = false;
            if (string.IsNullOrEmpty(configInfo.Alipayaccout) && string.IsNullOrEmpty(configInfo.Tenpayaccout))
                isOpen = false;

            return isOpen;
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="url">请求ATN服务器地址</param>
        /// <returns></returns>
        public static string GetHttp(string url)
        {
            return GetHttp(url, "");
        }

        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="timeout"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetHttp(string url, string postData)
        {
            string strResult;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentLength = postData.Length;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 120000;

            HttpWebResponse response = null;
            try
            {
                StreamWriter swRequestWriter = new StreamWriter(request.GetRequestStream());
                swRequestWriter.Write(postData);
                if (swRequestWriter != null)
                    swRequestWriter.Close();

                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    strResult = reader.ReadToEnd();
                }
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return strResult;
        }
    }
}