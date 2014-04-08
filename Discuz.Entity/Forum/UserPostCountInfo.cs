using System;

namespace Discuz.Entity
{
    /// <summary>
    /// 日期类型
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        Minute = 1,
        /// <summary>
        /// 小时
        /// </summary>
        Hour = 2,
        /// <summary>
        /// 天
        /// </summary>
        Day = 3,
        /// <summary>
        /// 周
        /// </summary>
        Week =4,
        /// <summary>
        /// 月
        /// </summary>
        Month = 5,
        /// <summary>
        /// 年
        /// </summary>
        Year = 6
    }

    /// <summary>
    /// 用户发帖数
    /// </summary>
    public class UserPostCountInfo
    {
        ///<summary>
        ///用户UID
        ///</summary>
        public int Uid { set; get; }

        ///<summary>
        ///用户名
        ///</summary>
        public string Username { set; get; }

        ///<summary>
        ///发帖数
        ///</summary>
        public int PostCount { set; get; }
    }
}
