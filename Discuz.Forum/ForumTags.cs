using System.Text;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Forum.ScheduledEvents;
using Discuz.Cache;

namespace Discuz.Forum
{
    /// <summary>
    /// 论坛标签(Tag)操作类
    /// </summary>
    public class ForumTags
    {
        /// <summary>
        /// 论坛热门标签缓存文件(json格式)文件路径
        /// </summary>
        public const string ForumHotTagJSONCacheFileName = "cache\\tag\\hottags_forum_cache_json.txt";
        /// <summary>
        /// 论坛热门标签缓存文件(jsonp格式)文件路径
        /// </summary>
        public const string ForumHotTagJSONPCacheFileName = "cache\\tag\\hottags_forum_cache_jsonp.txt";
      
        /// <summary>
        /// 写入热门标签缓存文件(json格式)
        /// </summary>
        /// <param name="count">数量</param>
        public static void WriteHotTagsListForForumCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONCacheFileName;
            List<TagInfo> tags = Discuz.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, string.Empty, true);
        }

        /// <summary>
        /// 写入热门标签缓存文件(jsonp格式)
        /// </summary>
        /// <param name="count"></param>
        public static void WriteHotTagsListForForumJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONPCacheFileName;
            List<TagInfo> tags = Discuz.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, "forumhottag_callback", true);
        }

        /// <summary>
        /// 写入主题标签缓存文件
        /// </summary>
        /// <param name="tagsArray">标签数组</param>
        /// <param name="topicid">主题Id</param>
        public static void WriteTopicTagsCacheFile(int topicid)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/topic/magic/");
            dir.Append((topicid / 1000 + 1).ToString());
            dir.Append("/");
            string filename = Utils.GetMapPath(dir.ToString() + topicid.ToString() + "_tags.config");
            List<TagInfo> tags = GetTagsListByTopic(topicid);
            Tags.WriteTagsCacheFile(filename, tags, string.Empty, false);
        }

        /// <summary>
        /// 获取主题所包含的Tag
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetTagsListByTopic(int topicid)
        {
            List<TagInfo> tabInfoList = null;
            //只有在开启memcached时才会缓存tag标签
            if ((MemCachedConfigs.GetConfig() != null && MemCachedConfigs.GetConfig().ApplyMemCached) ||
                (RedisConfigs.GetConfig()!=null && RedisConfigs.GetConfig().ApplyRedis))
            {
                DNTCache cache = DNTCache.GetCacheService();
                tabInfoList = cache.RetrieveObject("/Forum/ShowTopic/Tag/" + topicid + "/") as List<TagInfo>;
                if (tabInfoList == null)
                {
                    tabInfoList = Discuz.Data.ForumTags.GetTagsListByTopic(topicid);
                    cache.AddObject("/Forum/ShowTopic/Tag/" + topicid + "/", tabInfoList);
                }
            }
            else
                tabInfoList = Discuz.Data.ForumTags.GetTagsListByTopic(topicid);

            return tabInfoList;
        }

        /// <summary>
        /// 获取主题所包括的Tag字符串(以","分割)
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        public static string GetTagsByTopicId(int topicid)
        {
            List<TagInfo> tags = GetTagsListByTopic(topicid);
            string result = "";
            foreach (TagInfo taginfo in tags)
            {
                if (Utils.StrIsNullOrEmpty(result))
                    result = taginfo.Tagname;
                else
                    result = result + "," + taginfo.Tagname;
            }
            return result;
        }

        /// <summary>
        /// 热门标签
        /// </summary>
        /// <param name="count">标签数</param>
        /// <returns>TagInfo</returns>
        public static TagInfo[] GetCachedHotForumTags(int count)
        {
            TagInfo[] tags;
            DNTCache cache = DNTCache.GetCacheService();
            tags = cache.RetrieveObject("/Forum/Tag/Hot-" + count) as TagInfo[];
            if (tags == null)
            {
                tags = Discuz.Data.ForumTags.GetCachedHotForumTags(count);
                cache.AddObject("/Forum/Tag/Hot-" + count, tags, 360 * 60);
            }
            return tags;
        }

        /// <summary>
        /// 删除主题标题
        /// </summary>
        /// <param name="topicid">主题ID</param>
        public static void DeleteTopicTags(int topicid)
        {
            Discuz.Data.ForumTags.DeleteTopicTags(topicid);
            DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopic/Tag/" + topicid + "/");
        }
        
        /// <summary>
        /// 创建主题标签
        /// </summary>
        /// <param name="tagArray">标签数组</param>
        /// <param name="topicid">主题ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="curdatetime">当前日期</param>
        public static void CreateTopicTags(string[] tagArray, int topicId, int userId, string currentDateTime)
        {
            Discuz.Data.ForumTags.CreateTopicTags(string.Join(" ", tagArray), topicId, userId, currentDateTime);
            ForumTags.WriteTopicTagsCacheFile(topicId);
            DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopic/Tag/" + topicId + "/");
        }
    }
}
