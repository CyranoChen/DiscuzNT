using System;
using System.Text;
using System.Collections;

namespace Discuz.Config
{
    /// <summary>
    /// API配置信息类
    /// </summary>
    [Serializable]
    public class APIConfigInfo : IConfigInfo
    {
        private bool _enable;
        private ApplicationInfoCollection _appCollection;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// 整合程序集合
        /// </summary>
        public ApplicationInfoCollection AppCollection
        {
            get { return _appCollection; }
            set { _appCollection = value; }
        }
        
    }
}
