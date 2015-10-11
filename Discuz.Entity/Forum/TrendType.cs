using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 趋势统计类型
    /// </summary>
    public enum TrendType
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        Login,
        /// <summary>
        /// 新注册用户
        /// </summary>
        Register,
        /// <summary>
        /// 主题
        /// </summary>
        Topic,
        /// <summary>
        /// 投票
        /// </summary>
        Poll,
        /// <summary>
        /// 悬赏
        /// </summary>
        Bonus,
        /// <summary>
        /// 辩论
        /// </summary>
        Debate,
        /// <summary>
        /// 主题回帖
        /// </summary>
        Post,
        /// <summary>
        /// 全部
        /// </summary>
        All
    }
}
