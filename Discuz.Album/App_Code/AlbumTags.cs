using System;
using System.Text;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum.ScheduledEvents;
using Discuz.Common.Generic;
using System.Data;
using Discuz.Data;
using Discuz.Config;
using Discuz.Album.Data;
using Discuz.Forum;

namespace Discuz.Album
{
    /// <summary>
    /// 相册标签(Tag)操作类
    /// </summary>
    public class AlbumTags
    {
        public const string PHOTO_HOT_TAG_CACHE_FILENAME = "cache\\tag\\hottags_photo_cache_jsonp.txt";

        public static void WritePhotoTagsCacheFile(int photoid)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append(BaseConfigs.GetForumPath);
            dir.Append("cache/photo/");
            dir.Append((photoid / 1000 + 1).ToString());
            dir.Append("/");
            string filename = Utils.GetMapPath(dir.ToString() + photoid + "_tags.txt");
            List<TagInfo> tags = GetTagsListByPhotoId(photoid);
            Discuz.Forum.Tags.WriteTagsCacheFile(filename, tags, string.Empty, false);
        }

        public static List<TagInfo> GetTagsListByPhotoId(int photoid)
        {
            List<TagInfo> tags = new List<TagInfo>();
            IDataReader reader = DbProvider.GetInstance().GetTagsListByPhotoId(photoid);
            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();
            return tags;
        }



        /// <summary>
        /// 写入图片热门标签缓存文件
        /// </summary>
        /// <param name="count">数量</param>
        public static void WriteHotTagsListForPhotoJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + PHOTO_HOT_TAG_CACHE_FILENAME;
            List<TagInfo> tags = GetHotTagsListForPhoto(count);
            Discuz.Forum.Tags.WriteTagsCacheFile(filename, tags, "photohottag_callback", true);
        }

        private static List<TagInfo> GetHotTagsListForPhoto(int count)
        {
            List<TagInfo> tags = new List<TagInfo>();
            IDataReader reader = DbProvider.GetInstance().GetHotTagsListForPhoto(count);

            while (reader.Read())
            {
                tags.Add(Discuz.Data.Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();
            return tags;
        }

        public static string GetTagsByPhotoId(int photoid)
        {
            List<TagInfo> tags = GetTagsListByPhotoId(photoid);
            tags.Sort();
            string tagstr = "";
            foreach (TagInfo newtag in tags)
            {
                if (newtag.Orderid == -1)
                    continue;
                tagstr += (newtag.Tagname + " ");  
            }
            return tagstr.Trim();
        }
    }
}
