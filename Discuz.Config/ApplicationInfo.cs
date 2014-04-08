using System;
using System.Text;

namespace Discuz.Config
{
    /// <summary>
    /// 整合程序配置信息
    /// </summary>
    [Serializable]
    public class ApplicationInfo
    {
        #region Private fields
        private string _appName;
        private string _appUrl;
        private string _apiKey;
        private string _secret;
        private string _callbackUrl;
        private string _ipAddresses;
        #endregion

        #region Properties
        /// <summary>
        /// 整合程序名称 50字节限制
        /// </summary>
        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        /// <summary>
        /// 整合程序Url
        /// </summary>
        public string AppUrl
        {
            get { return _appUrl; }
            set { _appUrl = value; }
        }

        /// <summary>
        /// 整合程序API代码 32位
        /// </summary>
        public string APIKey
        {
            get { return _apiKey; }
            set { _apiKey = value; }
        }

        /// <summary>
        /// 整合程序密钥 32位
        /// </summary>
        public string Secret
        {
            get { return _secret; }
            set { _secret = value; }
        }

        /// <summary>
        /// 登录完成后返回地址 100字节限制
        /// </summary>
        public string CallbackUrl
        {
            get { return _callbackUrl; }
            set { _callbackUrl = value; }
        }

        /// <summary>
        /// 同步数据的地址 100字节，接受dnt事件消息
        /// </summary>
        private string _syncUrl;
        public string SyncUrl
        {
            get { return _syncUrl; }
            set { _syncUrl = value; }
        }

        /// <summary>
        /// 同步模式(0为关闭，1为全都开启，2为部分开启)
        /// </summary>
        private int _syncMode;
        public int SyncMode
        {
            get { return _syncMode; }
            set { _syncMode = value; }
        }

        /// <summary>
        /// 需要同步的事件列表
        /// </summary>
        private string _syncList;
        public string SyncList
        {
            get { return _syncList; }
            set { _syncList = value; }
        }

        /// <summary>
        /// 应用程序类型(1为web,2为desktop)
        /// </summary>
        private int _applicationType = 1;
        public int ApplicationType
        {
            get { return _applicationType; }
            set { _applicationType = value; }
        }

        /// <summary>
        /// 允许的服务器IP地址 逗号分隔
        /// </summary>
        public string IPAddresses
        {
            get { return _ipAddresses; }
            set { _ipAddresses = value; }
        }

        /// <summary>
        /// 应用描述
        /// </summary>
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// 小图标
        /// </summary>
        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        /// <summary>
        /// logo
        /// </summary>
        private string _logo;
        public string Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        #endregion
    }
}
