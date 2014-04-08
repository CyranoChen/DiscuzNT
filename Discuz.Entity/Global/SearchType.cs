using System;
using System.Text;

namespace Discuz.Entity
{
    /// <summary>
    /// 搜索类型
    /// </summary>
    public enum SearchType : int
    {
        /// <summary>
        /// 全部搜索
        /// </summary>
        //All,
        /// <summary>
        /// 搜索精华主题
        /// </summary>
        DigestTopic,
        /// <summary>
        /// 搜索主题标题
        /// </summary>
        TopicTitle,
        /// <summary>
        /// 搜索帖子标题
        /// </summary>
        //PostTitle,
        /// <summary>
        /// 帖子全文检索
        /// </summary>
        PostContent,
        /// <summary>
        /// 搜索相册标题
        /// </summary>
        AlbumTitle,
        /// <summary>
        /// 搜索日志标题
        /// </summary>
        SpacePostTitle,
        /// <summary>
        /// 仅按作者搜索,搜索论坛、空间、相册
        /// </summary>
        ByPoster,
        /// <summary>
        /// 非法的参数信息
        /// </summary>
        Error
    }
}
