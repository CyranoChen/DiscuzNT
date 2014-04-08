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
    /// ��̳��ǩ(Tag)������
    /// </summary>
    public class ForumTags
    {
        /// <summary>
        /// ��̳���ű�ǩ�����ļ�(json��ʽ)�ļ�·��
        /// </summary>
        public const string ForumHotTagJSONCacheFileName = "cache\\tag\\hottags_forum_cache_json.txt";
        /// <summary>
        /// ��̳���ű�ǩ�����ļ�(jsonp��ʽ)�ļ�·��
        /// </summary>
        public const string ForumHotTagJSONPCacheFileName = "cache\\tag\\hottags_forum_cache_jsonp.txt";
      
        /// <summary>
        /// д�����ű�ǩ�����ļ�(json��ʽ)
        /// </summary>
        /// <param name="count">����</param>
        public static void WriteHotTagsListForForumCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONCacheFileName;
            List<TagInfo> tags = Discuz.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, string.Empty, true);
        }

        /// <summary>
        /// д�����ű�ǩ�����ļ�(jsonp��ʽ)
        /// </summary>
        /// <param name="count"></param>
        public static void WriteHotTagsListForForumJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + ForumHotTagJSONPCacheFileName;
            List<TagInfo> tags = Discuz.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, tags, "forumhottag_callback", true);
        }

        /// <summary>
        /// д�������ǩ�����ļ�
        /// </summary>
        /// <param name="tagsArray">��ǩ����</param>
        /// <param name="topicid">����Id</param>
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
        /// ��ȡ������������Tag
        /// </summary>
        /// <param name="topicid">����Id</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetTagsListByTopic(int topicid)
        {
            List<TagInfo> tabInfoList = null;
            //ֻ���ڿ���memcachedʱ�ŻỺ��tag��ǩ
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
        /// ��ȡ������������Tag�ַ���(��","�ָ�)
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
        /// ���ű�ǩ
        /// </summary>
        /// <param name="count">��ǩ��</param>
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
        /// ɾ���������
        /// </summary>
        /// <param name="topicid">����ID</param>
        public static void DeleteTopicTags(int topicid)
        {
            Discuz.Data.ForumTags.DeleteTopicTags(topicid);
            DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopic/Tag/" + topicid + "/");
        }
        
        /// <summary>
        /// ���������ǩ
        /// </summary>
        /// <param name="tagArray">��ǩ����</param>
        /// <param name="topicid">����ID</param>
        /// <param name="userid">�û�ID</param>
        /// <param name="curdatetime">��ǰ����</param>
        public static void CreateTopicTags(string[] tagArray, int topicId, int userId, string currentDateTime)
        {
            Discuz.Data.ForumTags.CreateTopicTags(string.Join(" ", tagArray), topicId, userId, currentDateTime);
            ForumTags.WriteTopicTagsCacheFile(topicId);
            DNTCache.GetCacheService().RemoveObject("/Forum/ShowTopic/Tag/" + topicId + "/");
        }
    }
}
