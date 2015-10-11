using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public class ManyouApplicationInfo
    {
        private int appId;

        public int AppId
        {
            get { return appId; }
            set { appId = value; }
        }
        private string appName;

        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }
        private int narrow;

        public int Narrow
        {
            get { return narrow; }
            set { narrow = value; }
        }
        private ApplicationFlag flag;

        public ApplicationFlag Flag
        {
            get { return flag; }
            set { flag = value; }
        }
        private int displayOrder;

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }
        private int version;

        public int Version
        {
            get { return version; }
            set { version = value; }
        }
        private DisplayMethod displayMethod;

        public DisplayMethod DisplayMethod
        {
            get { return displayMethod; }
            set { displayMethod = value; }
        }
    }

    /// <summary>
    /// 应用程序状态枚举
    /// </summary>
    public enum ApplicationFlag
    {
        /// <summary>
        /// 正常使用
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 关闭的
        /// </summary>
        Disabled = 2,
        /// <summary>
        /// 默认的
        /// </summary>
        Default = 1
    }

    public enum DisplayMethod
    {
        /// <summary>
        /// IFrame显示
        /// </summary>
        IFrame,
        /// <summary>
        /// 其他显示方式
        /// </summary>
        Other
    }
}
