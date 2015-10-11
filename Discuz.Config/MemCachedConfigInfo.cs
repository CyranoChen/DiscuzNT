using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    /// MemCached配置信息类文件
    /// </summary>
    public class MemCachedConfigInfo : IConfigInfo
    {
        private bool _applyMemCached;
        /// <summary>
        /// 是否应用MemCached
        /// </summary>
        public bool ApplyMemCached
        {
            get
            {
                return _applyMemCached;
            }
            set
            {
                _applyMemCached = value;
            }
        }

        private string _serverList;
        /// <summary>
        /// 链接地址
        /// </summary>
        public string ServerList
        {
            get
            {
                return _serverList;
            }
            set
            {
                _serverList = value;
            }
        }

        private string _poolName;
        /// <summary>
        /// 链接池名称
        /// </summary>
        public string PoolName
        {
            get
            {
                return Utils.StrIsNullOrEmpty(_poolName) ? "DiscuzNT_MemCache" : _poolName;
            }
            set
            {
                _poolName = value;
            }
        }

        private int _intConnections;
        /// <summary>
        /// 初始化链接数
        /// </summary>
        public int IntConnections
        {
            get
            {
                return _intConnections > 0 ? _intConnections : 3;
            }
            set
            {
                _intConnections = value;
            }
        }

        private int _minConnections;
        /// <summary>
        /// 最少链接数
        /// </summary>
        public int MinConnections
        {
            get
            {
                return _minConnections > 0 ? _minConnections : 3;
            }
            set
            {
                _minConnections = value;
            }
        }

        private int _maxConnections;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnections
        {
            get
            {
                return _maxConnections > 0 ?_maxConnections : 5;
            }
            set
            {
                _maxConnections = value;
            }
        }

        private int _socketConnectTimeout;
        /// <summary>
        /// Socket链接超时时间
        /// </summary>
        public int SocketConnectTimeout
        {
            get
            {
                return _socketConnectTimeout > 1000 ? _socketConnectTimeout : 1000;
            }
            set
            {
                _socketConnectTimeout = value;
            }
        }

        private int _socketTimeout;
        /// <summary>
        /// socket超时时间
        /// </summary>
        public int SocketTimeout
        {
            get
            {
                return _socketTimeout > 1000 ? _maintenanceSleep : 3000;
            }
            set
            {
                _socketTimeout = value;
            }
        }

        private int _maintenanceSleep;
        /// <summary>
        /// 维护线程休息时间
        /// </summary>
        public int MaintenanceSleep
        {
            get
            {
                return _maintenanceSleep > 0 ? _maintenanceSleep : 30;
            }
            set
            {
                _maintenanceSleep = value;
            }
        }

        private bool _failOver;
        /// <summary>
        /// 失效转移，即服务器失效后，由其它服务器接管其工作,详情参见http://baike.baidu.com/view/1084309.htm
        /// </summary>
        public bool FailOver
        {
            get
            {
                return _failOver;
            }
            set
            {
                _failOver = value;
            }
        }

        private bool _nagle;
        /// <summary>
        /// 是否用nagle算法启动socket
        /// </summary>
        public bool Nagle
        {
            get
            {
                return _nagle;
            }
            set
            {
                _nagle = value;
            }
        }

        private int _localCacheTime = 30000;
        /// <summary>
        /// 本地缓存到期时间，该设置会与memcached搭配使用，单位:秒
        /// </summary>
        public int LocalCacheTime
        {
            get
            {
                return _localCacheTime;
            }
            set
            {
                _localCacheTime = value;
            }
        }

        private bool _recordeLog = false;
        /// <summary>
        /// 是否记录日志,该设置仅用于排查memcached运行时出现的问题,如memcached工作正常,请关闭该项
        /// </summary>
        public bool RecordeLog
        {
            get
            {
                return _recordeLog;
            }
            set
            {
                _recordeLog = value;
            }
        }

        private int _cacheShowTopicPageNumber = 5;
        /// <summary>
        /// 缓存帖子列表分页数(showtopic页数使用缓存前N页的帖子列表信息)
        /// </summary>
        public int CacheShowTopicPageNumber
        {
            get
            {
                return _cacheShowTopicPageNumber;
            }
            set
            {
                _cacheShowTopicPageNumber = value;
            }
        }

        /// <summary>
        /// 缓存showforum页面分页数
        /// </summary>
        public int CacheShowForumPageNumber{set;get;}

        /// <summary>
        /// 缓存showforum页面时间(单位:分钟)
        /// </summary>
        public int CacheShowForumCacheTime{set;get;}


        private HashingAlgorithm _hashingAlgorithm;
        /// <summary>
        /// 是否用nagle算法启动socket
        /// </summary>
        public HashingAlgorithm HashingAlgorithm
        {
            get
            {
                return _hashingAlgorithm;
            }
            set
            {
                _hashingAlgorithm = value;
            }
        }

        //private string _syncCacheUrl = "";
        ///// <summary>
        ///// 负载均衡下同步缓存的url信息(注:站点之间用","分割)
        ///// </summary>
        //public string SyncCacheUrl
        //{
        //    get
        //    {
        //        return _syncCacheUrl;
        //    }
        //    set
        //    {
        //        _syncCacheUrl = value;
        //    }
        //}

        //private string _authCode = "";
        ///// <summary>
        ///// 负载均衡下同步缓存的认证码信息
        ///// </summary>
        //public string AuthCode
        //{
        //    get
        //    {
        //        return _authCode;
        //    }
        //    set
        //    {
        //        _authCode = value;
        //    }
        //}
    }


    ///<summary>
    ///Hashing algorithms we can use
    ///</summary>
    public enum HashingAlgorithm
    {
        ///<summary>native String.hashCode() - fast (cached) but not compatible with other clients</summary>
        Native = 0,
        ///<summary>original compatibility hashing alg (works with other clients)</summary>
        OldCompatibleHash = 1,
        ///<summary>new CRC32 based compatibility hashing algorithm (fast and works with other clients)</summary>
        NewCompatibleHash = 2,
        ///<summary>Consistent Hashing algorithm</summary>
        KetamaHash = 3
    }

   
}
