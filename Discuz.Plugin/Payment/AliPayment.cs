using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

using Discuz.Common;
using Discuz.Plugin.Payment.Alipay;

namespace Discuz.Plugin.Payment
{
    /// <summary>
    /// 支付宝操作类
    /// </summary>
    public class AliPayment : IPayment
    {

        #region 单体模式实例化

        private static volatile AliPayment instance = null;

        private static object lockHelper = new object();

        private AliPayment()
        { }

        /// <summary>
        /// 单体模式返回当前类的实例
        /// </summary>
        /// <returns></returns>
        public static AliPayment GetService()
        {

            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new AliPayment();
                    }
                }
            }

            return instance;
        }

        #endregion


        #region 成员变量
        /// <summary>
        /// 支付宝网关地址
        /// </summary>
        private string _aliPay = "http://pay.discuz.net/gateway/alipay.php?";
        #endregion


        #region 属性
        /// <summary>
        /// 支付网关地址
        /// </summary>
        public string PayUrl
        {
            set { _aliPay = value; }
            get { return _aliPay; }
        }
        #endregion


        #region (URL)参数快速排序

        /// <summary>
        /// 把数组划分为两个部分
        /// </summary>
        /// <param name="arr">划分的数组</param>
        /// <param name="low">数组低端上标</param>
        /// <param name="high">数组高端下标</param>
        /// <returns></returns>
        public static int Partition(string[] strArray, int low, int high)
        {
            //进行一趟快速排序,返回中心轴记录位置
            // arr[0] = arr[low];
            string pivot = strArray[low];//把中心轴置于arr[0]
            while (low < high)
            {
                while (low < high && System.String.CompareOrdinal(strArray[high], pivot) >= 0)
                    --high;
                //将比中心轴记录小的移到低端
                Swap(ref strArray[high], ref strArray[low]);
                while (low < high && System.String.CompareOrdinal(strArray[low], pivot) <= 0)
                    ++low;
                Swap(ref strArray[high], ref strArray[low]);
                //将比中心轴记录大的移到高端
            }
            strArray[low] = pivot; //中心轴移到正确位置
            return low;  //返回中心轴位置
        }

        public static void Swap(ref string i, ref string j)
        {
            string t;
            t = i;
            i = j;
            j = t;
        }

        /// <summary>
        /// 快速排序算法
        /// </summary>
        /// <param name="arr">划分的数组</param>
        /// <param name="low">数组低端上标</param>
        /// <param name="high">数组高端下标</param>
        public static void QuickSort(string[] strArray, int low, int high)
        {
            if (low <= high - 1)//当 arr[low,high]为空或只一个记录无需排序
            {
                int pivot = Partition(strArray, low, high);
                //左子树
                QuickSort(strArray, low, pivot - 1);
                //右子树
                QuickSort(strArray, pivot + 1, high);
            }
        }

        #endregion


        /// <summary>
        /// 与ASP兼容的MD5加密算法
        /// </summary>
        public static string GetMD5(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }


        /// <summary>
        /// 反射出指定对象实例的所有属性值
        /// </summary>
        /// <param name="obj">指定对象实例</param>
        /// <returns></returns>
        private static string[] GetUrlParam(object obj)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string urlParam = "";
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.GetValue(obj, null) != null)
                {
                    if (pi.Name == "Sign" || pi.Name == "Sign_Type")
                    {
                        continue;
                    }

                    //物流信息时
                    if (pi.Name == "Logistics_Info")
                    {
                        LogisticsInfo[] logisticsInfoArray = ((NormalTrade)obj).Logistics_Info;
                        int i = 0;
                        foreach (LogisticsInfo logisticsInfo in logisticsInfoArray)
                        {
                            if (logisticsInfo.Logistics_Type != "")
                            {
                                //物流参数的下标(第一种物流方式为"",第二种为"_1",第三种为"_2",以此类推)
                                string orderflag = "";
                                if (i > 0)
                                {
                                    orderflag = "_" + i;
                                }
                                urlParam += "logistics_type" + orderflag + "=" + logisticsInfo.Logistics_Type + "&logistics_fee" + orderflag + "=" + logisticsInfo.Logistics_Fee + "&logistics_payment" + orderflag + "=" + logisticsInfo.Logistics_Payment +"&";
                                i++;
                            }
                        }
                    }
                    else
                    {
                        urlParam += pi.Name.ToLower().Replace("input_charset", "_input_charset") + "=" + pi.GetValue(obj, null).ToString() +"&";
                    }
                }
            }

            if (urlParam.EndsWith("&"))
            { 
                urlParam = urlParam.Substring(0,urlParam.Length-1);
            }

            return urlParam.Split('&');
        }

        
        /// <summary>
        /// 创建TradeUrl
        /// </summary>
        /// <param name="strArray">参数字符串数组</param>
        /// <returns>返回url字符串</returns>
        private static string CreateTradeUrl(string[] strArray)
        {
            string url = "";
            foreach (string urlParam in strArray)
            {
                if (url == "")
                {
                    url = urlParam;
                }
                else
                {
                    url = url + "&" + urlParam;
                }
            }
            return url;
        }

        /// <summary>
        /// 创建UrlEncode的TradeUrl
        /// </summary>
        /// <param name="strArray">参数字符串数组</param>
        /// <returns>返回url字符串</returns>
        private static string CreateEncodeUrl(string[] strArray)
        {
            string url = "";
            foreach (string urlParam in strArray)
            {
                if (url == "")
                {
                    url = urlParam.Split('=')[0] + "=" + Utils.UrlEncode(urlParam.Split('=')[1]);
                }
                else
                {
                    url = url + "&" + urlParam.Split('=')[0] + "=" + Utils.UrlEncode(urlParam.Split('=')[1]);
                }
            }
            return url;
        }

        /// <summary>
        /// 构造虚拟商品url
        /// </summary>
        /// <param name="digitalGoods">虚拟商品信息</param>
        /// <param name="key">账户的交易安全校验码(key)</param>
        /// <returns></returns>
        public string CreateDigitalGoodsTradeUrl(ITrade _goods)
        {
            DigitalTrade digitalGoods = (DigitalTrade)_goods;
            string tradeUrl = ""; //未进行UrlEncode编码的链接参数串
            string encodeUrl = "";//进行UrlEncode编码的链接参数串
            string[] urlParamArray = GetUrlParam(digitalGoods);
            //排序参数
            QuickSort(urlParamArray, 0, urlParamArray.Length - 1);
            tradeUrl = CreateTradeUrl(urlParamArray);
            encodeUrl = CreateEncodeUrl(urlParamArray);
            return PayUrl + encodeUrl + string.Format("&sign_type={0}",digitalGoods.Sign_Type);
        }


        /// <summary>
        /// 构造实物商品url
        /// </summary>
        /// <param name="normalGoods">实特商品信息</param>
        /// <param name="key">账户的交易安全校验码(key)</param>
        /// <returns></returns>
        public string CreateNormalGoodsTradeUrl(ITrade _goods)
        {
            DigitalTrade normalGoods = (NormalTrade)_goods;
            string tradeUrl = ""; //未进行UrlEncode编码的链接参数串
            string encodeUrl = "";//进行UrlEncode编码的链接参数串
            string[] urlParamArray = GetUrlParam(normalGoods);
            //排序参数
            QuickSort(urlParamArray, 0, urlParamArray.Length - 1);
            tradeUrl = CreateTradeUrl(urlParamArray);
            encodeUrl = CreateEncodeUrl(urlParamArray);
            return PayUrl + encodeUrl + string.Format("&sign_type={0}", normalGoods.Sign_Type);
        }

    
    }

}
