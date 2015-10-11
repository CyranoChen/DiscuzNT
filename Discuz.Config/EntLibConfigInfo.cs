using System;

using Discuz.Common;

namespace Discuz.Config
{
    /// <summary>
    /// 企业版配置信息类文件
    /// </summary>
    public class EntLibConfigInfo : IConfigInfo
    {
        private string _medaldir = "";
        /// <summary>
        /// 勋章图标地址
        /// /// </summary>
        public string Medaldir
        {
            get
            {
                return _medaldir;
            }
            set
            {
                _medaldir = value;
            }
        }

        private string _topicidentifydir = "";
        /// <summary>
        /// 主题鉴定图标地址
        /// </summary>
        public string Topicidentifydir
        {
            get
            {
                return _topicidentifydir;
            }
            set
            {
                _topicidentifydir = value;
            }
        }

        private string _posticondir = "";
        /// <summary>
        /// 主题图标地址
        /// </summary>
        public string Posticondir
        {
            get
            {
                return _posticondir;
            }
            set
            {
                _posticondir = value;
            }
        }


        private string _jsdir = "";
        /// <summary>
        /// js文件地址
        /// </summary>
        public string Jsdir
        {
            get
            {
                return _jsdir;
            }
            set
            {
                _jsdir = value;
            }
        }

        private string _attachmentdir = "";
        /// <summary>
        /// 附件地址,比如开启SQUID附件缓存地址
        /// </summary>
        public string Attachmentdir
        {
            get
            {
                return _attachmentdir;
            }
            set
            {
                _attachmentdir = value;
            }
        }


        /// <summary>
        /// 提供帖子附件保存到Mongodb 服务
        /// </summary>
        public CacheAttachFile Cacheattachfiles = new CacheAttachFile();

        /// <summary>
        /// Sphinx企业级查询配置信息
        /// </summary>
        public SphinxConfig Sphinxconfig = new SphinxConfig();
        /// <summary>
        /// 在线表数据库链接地址，用于将在线表布署到别的数据库中
        /// </summary>
        public OnlineTableConnect Onlinetableconnect = new OnlineTableConnect();

        /// <summary>
        /// 提供数据库缓存服务，将在线表(dnt_online)放入CACHE中
        /// </summary>
        public DBCache Cacheonlineuser = new DBCache();
        /// <summary>
        /// 提供数据库缓存服务，将用户表(dnt_users)放入CACHE中
        /// </summary>
        public DBCache Cacheusers = new DBCache();
        /// <summary>
        /// 提供数据库缓存服务，将主题表(dnt_topic)放入CACHE中
        /// </summary>
        public CacheTopic Cachetopics = new CacheTopic();
        /// <summary>
        /// 提供数据库缓存服务，将帖子表(dnt_post)放入CACHE中
        /// </summary>
        public DBCache Cacheposts = new DBCache();
        /// <summary>
        /// 提供数据库缓存服务，将帖子表(dnt_post)放入CACHE中
        /// </summary>
        public DBCache Cacheattachments = new DBCache();
    }

    /// <summary>
    /// 在线表数据库链接地址，用于将在线表布署到别的数据库中，在开启该功能之前，请确保在相应数据库中已有相应的在线表和存储过程
    /// </summary>
    public class OnlineTableConnect
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable = false;
        /// <summary>
        /// SqlServer数据库链接地址
        /// </summary>
        public string SqlServerConn = "";
    }

    /// <summary>
    /// Sphinx企业级查询配置信息
    /// </summary>
    public class SphinxConfig
    {
        /// <summary>
        /// Mysql增量数据库链接地址
        /// </summary>
        public string MySqlConn = "";
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable = false;
        /// <summary>
        /// Sphinx服务地址
        /// </summary>
        public string SphinxServiceHost = "";
        /// <summary>
        /// Sphinx服务端口
        /// </summary>
        public int SphinxServicePort = 3312;
        /// <summary>
        /// 索引节点信息,如果该节点为空，则sphinx客户端程序会根据当前分表前缀自动拼接出相应索引信息
        /// </summary>
        public string IndexSectionName = "";

        /// <summary>
        /// 客户端连接超时数,单位:毫秒 SPH_CLIENT_TIMEOUT_MILLISEC
        /// </summary>
        private int _sphClientTimeOut = 30000;
        /// <summary>
        /// 客户端连接超时数,单位:毫秒 SPH_CLIENT_TIMEOUT_MILLISEC
        /// </summary>
        public int SphClientTimeOut
        {
            set { _sphClientTimeOut = value > 10000 ? value : 10000; }
            get { return _sphClientTimeOut > 10000 ? _sphClientTimeOut : 10000; }
        }


        /// <summary>
        /// 查询服务数据操作接口
        /// </summary>
        public interface ISqlService
        {
            /// <summary>
            /// 创建Sphinx的增量数据表（目前只支持mysql数据库类型）
            /// </summary>
            /// <param name="tableName">当前分表名称</param>
            /// <returns></returns>
            bool CreatePostTable(string tableName);
            /// <summary>
            /// 创建Sphinx增量表帖子
            /// </summary>
            /// <param name="tableName">当前分表名称</param>
            /// <param name="pid">帖子ID</param>
            /// <param name="tid">主题ID</param>     
            /// <param name="fid">所属版块ID</param>
            /// <param name="posterid">发帖人</param>
            /// <param name="postdatetime">发帖日期</param>
            /// <param name="title">标题</param>
            /// <param name="message">内容</param>
            /// <returns></returns>
            int CreatePost(string tableName, int pid, int tid, int fid, int posterid, string postdatetime, string title, string message);
            /// <summary>
            /// 更新Sphinx增量表帖子
            /// </summary>
            /// <param name="tableName">当前分表名称</param>
            /// <param name="pid">帖子ID</param>
            /// <param name="tid">主题ID</param>     
            /// <param name="fid">所属版块ID</param>
            /// <param name="posterid">发帖人</param>
            /// <param name="postdatetime">发帖日期</param>
            /// <param name="title">标题</param>
            /// <param name="message">内容</param>
            /// <returns></returns>
            int UpdatePost(string tableName, int pid, int tid, int fid, int posterid, string postdatetime, string title, string message);
            /// <summary>
            /// 删除Sphinx增量表帖
            /// </summary>
            /// <param name="tableName">当前分表名称</param>
            /// <param name="pid">帖子ID</param>
            /// <param name="tid">主题ID</param>
            /// <returns></returns>
            //int DeletePost(string tableName, int pid, int tid);

            /// <summary>
            /// 获取要搜索的主题ID(Tid)信息
            /// </summary>
            /// <param name="posterId">发帖者id</param>
            /// <param name="searchForumId">搜索版块id</param>
            /// <param name="resultOrder">结果排序方式</param>
            /// <param name="resultOrderType">结果类型类型</param>
            /// <param name="searchTime">搜索时间</param>
            /// <param name="searchTimeType">搜索时间类型</param>
            /// <param name="postTableId">当前分表ID</param>
            /// <param name="strKeyWord">关键字</param>
            /// <returns></returns>
            string GetSearchPostContentSQL(int posterId, string searchForumId, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, int postTableId, System.Text.StringBuilder strKeyWord);
        }
    }

    /// <summary>
    /// 提供数据库缓存服务，将在线表主题表这类大表放入缓存之中
    /// </summary>
    public class NoSqlDB
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Enable = false;
        /// <summary>
        /// 服务地址
        /// </summary>
        public string Host = "";
        /// <summary>
        /// 服务地址
        /// </summary>
        public int Port = 0;
        /// <summary>
        /// 链接池名称
        /// </summary>
        public string PoolName = "dnt";
        /// <summary>
        /// 初始化链接数
        /// </summary>
        public int IntConnections = 4;
        /// <summary>
        /// 最少链接数
        /// </summary>
        public int MinConnections = 4;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnections = 4;
        /// <summary>
        /// avaiable pool池中线程的最大空闲时间
        /// </summary>
        public int MaxIdle = 30000;
        /// <summary>
        ///  busy pool中线程的最大忙碌时间
        /// </summary>
        public int MaxBusy = 50000;
        /// <summary>
        /// 维护线程休息时间
        /// </summary>
        public int MaintenanceSleep = 300000;
        /// <summary>
        /// TcpClient读操作超时时间
        /// </summary>
        public int TcpClientTimeout = 3000;
        /// <summary>
        /// TcpClient链接超时时间
        /// </summary>
        public int TcpClientConnectTimeout = 30000;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username = "";
        /// <summary>
        /// 密码
        /// </summary>
        public string Password = "";
    }

    /// <summary>
    /// 提供数据库缓存服务，将在线表主题表这类大表放入缓存之中
    /// </summary>
    public class DBCache : NoSqlDB
    {
        /// <summary>
        /// 缓存类型1为mongodb,2为tokyotyrnat
        /// </summary>
        public int CacheType = 1;
    }

    public class CacheTopic : DBCache
    {
        private int _cacheTopicNumber = 10;
        /// <summary>
        /// 每个版块缓存的主题数(注：当用户使用多个WEB园则需要将其设置为0)，
        /// </summary>
        public int CacheTopicNumber
        {
            get
            {
                return _cacheTopicNumber;
            }
            set
            {
                _cacheTopicNumber = value;
            }
        }
    }

    public class CacheAttachFile : DBCache
    {
        private int _attachpostid = 0;
        /// <summary>
        /// 关联Cacheattachfiles开关的附件所属帖子ID，当帖子ID比该值大时,则到mongodb中获取附件流信息
        /// </summary>
        public int Attachpostid
        {
            get
            {
                return _attachpostid;
            }
            set
            {
                _attachpostid = value;
            }
        }
    }
}
