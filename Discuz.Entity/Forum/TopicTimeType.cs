using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public enum TopicTimeType : int
    {
        All = 0,
        /// <summary>
        /// 一天
        /// </summary>
        Day,
        /// <summary>
        /// 三天
        /// </summary>
        ThreeDays,
        /// <summary>
        /// 五天
        /// </summary>
        FiveDays,
        /// <summary>
        /// 一周
        /// </summary>
        Week,
        /// <summary>
        /// 一个月
        /// </summary>
        Month,
        /// <summary>
        /// 六个月
        /// </summary>
        SixMonth,
        /// <summary>
        /// 一年
        /// </summary>
        Year

    }
}
