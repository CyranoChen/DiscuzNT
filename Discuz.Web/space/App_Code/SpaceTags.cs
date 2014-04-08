using System;
using System.Text;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using System.Data;
using Discuz.Data;
using Discuz.Common.Generic;
using Discuz.Forum.ScheduledEvents;
using System.IO;
using Discuz.Space.Data;
using Discuz.Forum;

namespace Discuz.Space
{
    /// <summary>
    /// 空间相关标签类
    /// </summary>
    public class SpaceTags
    {
        public const string SpaceHotTagJSONPCacheFileName = "cache\\tag\\hottags_space_cache_jsonp.txt";

        /// <summary>
        /// 写入空间日志标签缓存文件(jsonp)
        /// </summary>
        /// <param name="postid"></param>
        public static void WriteSpacePostTagsCacheFile(int postid)
        {
            string filename = Utils.GetMapPath(GetSpacePostTagCacheFilePath(postid));
            List<TagInfo> tags = GetTagsListBySpacePost(postid);
            Discuz.Forum.Tags.WriteTagsCacheFile(filename, tags, string.Empty, false);
        }

        /// <summary>
        /// 获取日志所使用的Tag列表
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static List<TagInfo> GetTagsListBySpacePost(int postid)
        {
            List<TagInfo> tags = new List<TagInfo>();

            IDataReader reader = Data.DbProvider.GetInstance().GetTagsListBySpacePost(postid);

            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }

        /// <summary>
        /// 获取空间日志缓存文件路径
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static string GetSpacePostTagCacheFilePath(int postid)
        {
            return string.Format("{0}cache/spaceposttag/{1}/{2}_tags.txt", BaseConfigs.GetForumPath, postid / 1000 + 1, postid);
        }

        /// <summary>
        /// 写入空间热门标签缓存文件
        /// </summary>
        /// <param name="count">数量</param>
        public static void WriteHotTagsListForSpaceJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + SpaceHotTagJSONPCacheFileName;
            List<TagInfo> tags = GetHotTagsListForSpace(count);
            Discuz.Forum.Tags.WriteTagsCacheFile(filename, tags, "spacehottag_callback", true);
        }

        private static List<TagInfo> GetHotTagsListForSpace(int count)
        {
            List<TagInfo> tags = new List<TagInfo>();

            IDataReader reader = DbProvider.GetInstance().GetHotTagsListForSpace(count);

            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();

            return tags;
        }
        /// <summary>
        /// 删除日志对Tag的引用记录
        /// </summary>
        /// <param name="spacepostid"></param>
        public static void DeleteSpacePostTags(int spacepostid)
        {
            DbProvider.GetInstance().DeleteSpacePostTags(spacepostid);
        }

        /// <summary>
        /// 获取日志缓存文件内容，如果不存在则生成之
        /// </summary>
        /// <param name="postid">日志Id</param>
        /// <returns></returns>
        public static string GetSpacePostTagsCacheFile(int postid)
        {
            if (postid > 0)
            {
                string filename = Utils.GetMapPath(SpaceTags.GetSpacePostTagCacheFilePath(postid));
                string tags = string.Empty;
                if (!File.Exists(filename))
                {
                    SpaceTags.WriteSpacePostTagsCacheFile(postid);
                }

                if (File.Exists(filename))
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                        {
                            tags = sr.ReadToEnd();
                        }
                    }
                }

                return tags;
            }
            return string.Empty;
        }
    }
}
