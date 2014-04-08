using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.Entity
{
    public enum TopicOrderType
    {
        ID = 0,
        /// <summary>
        /// 浏览量
        /// </summary>
        Views = 1,
        /// <summary>
        /// 最后回复
        /// </summary>
        LastPost = 2,
        /// <summary>
        /// 按最新主题查
        /// </summary>
        PostDateTime = 3,
        /// <summary>
        /// 按精华主题查
        /// </summary>
        Digest = 4,
        /// <summary>
        /// 按回复数
        /// </summary>
        Replies = 5,
        /// <summary>
        /// 评分数
        /// </summary>
        Rate = 6
    }
}
