using System;

using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    /// 负载均衡配置信息类文件
    /// </summary>
    public class LoadBalanceConfigInfo : IConfigInfo
    {
        private bool _appLoadBalance = false;
        /// <summary>
        /// 应用负载均衡
        /// /// </summary>
        public bool AppLoadBalance
        {
            get
            {
                return _appLoadBalance;
            }
            set
            {
                _appLoadBalance = value;
            }
        }

        private string _siteUrl = "";
        /// <summary>
        /// url信息(注:站点之间用","分割)
        /// </summary>
        public string SiteUrl
        {
            get
            {
                return _siteUrl;
            }
            set
            {
                _siteUrl = value;
            }
        }

        //private int _weight;
        ///// <summary>
        ///// 负载均衡下同步缓存的url信息(注:站点之间用","分割)
        ///// </summary>
        //public int Weight
        //{
        //    get
        //    {
        //        return _weight;
        //    }
        //    set
        //    {
        //        _weight = value;
        //    }
        //}

        private string _authCode = "";
        /// <summary>
        /// 负载均衡下同步缓存的认证码信息
        /// </summary>
        public string AuthCode
        {
            get
            {
                return _authCode;
            }
            set
            {
                _authCode = value;
            }
        }

        private bool _recordPageView = false;
        /// <summary>
        /// 记录负载均衡下各站点PV情况
        /// </summary>
        public bool RecordPageView
        {
            get
            {
                return _recordPageView;
            }
            set
            {
                _recordPageView = value;
            }
        }
    }
}
