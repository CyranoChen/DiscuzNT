using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Common;

namespace Discuz.Data
{
    /// <summary>
    /// 标签数据操作类
    /// </summary>
    public class Tags
    {
        /// <summary>
        /// 将reader转化为实体类
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static TagInfo LoadSingleTagInfo(IDataReader reader)
        {
            TagInfo tag = new TagInfo();
            tag.Tagid = TypeConverter.ObjectToInt(reader["tagid"]);
            tag.Tagname = reader["tagname"].ToString();
            tag.Userid = TypeConverter.ObjectToInt(reader["userid"]);
            tag.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
            tag.Orderid = TypeConverter.ObjectToInt(reader["orderid"]);
            tag.Color = reader["color"].ToString();
            tag.Count = TypeConverter.ObjectToInt(reader["count"]);
            tag.Fcount = TypeConverter.ObjectToInt(reader["fcount"]);
            tag.Pcount = TypeConverter.ObjectToInt(reader["pcount"]);
            tag.Scount = TypeConverter.ObjectToInt(reader["scount"]);
            tag.Vcount = TypeConverter.ObjectToInt(reader["vcount"]);

            return tag;
        }

        /// <summary>
        /// 获取标签信息(不存在返回null)
        /// </summary>
        /// <param name="tagid">标签id</param>
        /// <returns></returns>
        public static TagInfo GetTagInfo(int tagid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTagInfo(tagid);
            TagInfo tag = null;
            if (reader.Read())
                tag = LoadSingleTagInfo(reader);

            reader.Close();
            return tag;
        }


        /// <summary>
        /// 绑定标签信息列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<TagInfo> GetTagInfoList(IDataReader reader)
        {
            List<TagInfo> tags = new List<TagInfo>();
            while (reader.Read())
            {
                tags.Add(Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();
            return tags;
        }


        /// <summary>
        /// 更新TAG
        /// </summary>
        /// <param name="tagid">标签ID</param>
        /// <param name="orderid">排序</param>
        /// <param name="color">颜色</param>
        public static void UpdateForumTags(int tagid, int orderid, string color)
        {
            DatabaseProvider.GetInstance().UpdateForumTags(tagid, orderid, color);
        }

        /// <summary>
        /// 返回论坛Tag列表
        /// </summary>
        /// <param name="tagname">查询关键字</param>
        /// <param name="type">全部0 锁定1 开放2</param>
        /// <returns></returns>
        public static DataTable GetForumTags(string tagName, int type)
        {
            return DatabaseProvider.GetInstance().GetForumTags(tagName, type);
        }
    }


    /// <summary>
    /// 论坛标签(Tag)操作类
    /// </summary>
    public class ForumTags
    {
        /// <summary>
        /// 获取主题所包含的Tag
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetTagsListByTopic(int topicid)
        {
            return Tags.GetTagInfoList(DatabaseProvider.GetInstance().GetTagsListByTopic(topicid));
        }

        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns>List</returns>
        public static List<TagInfo> GetHotTagsListForForum(int count)
        {
            return Tags.GetTagInfoList(DatabaseProvider.GetInstance().GetHotTagsListForForum(count));
        }


        /// <summary>
        /// 热门标签
        /// </summary>
        /// <param name="count">标签数</param>
        /// <returns>TagInfo</returns>
        public static TagInfo[] GetCachedHotForumTags(int count)
        {
            List<TagInfo> tagList = new List<TagInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetHotTagsListForForum(count);

            while (reader.Read())
            {
                tagList.Add(Tags.LoadSingleTagInfo(reader));
            }
            reader.Close();
            return tagList.ToArray();
        }

        /// <summary>
        /// 删除主题标题
        /// </summary>
        /// <param name="topicid">主题ID</param>
        public static void DeleteTopicTags(int topicid)
        {
            DatabaseProvider.GetInstance().DeleteTopicTags(topicid);
        }

        /// <summary>
        /// 创建主题标签
        /// </summary>
        /// <param name="tagArray">标签数组</param>
        /// <param name="topicid">主题ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="currentDateTime">当前日期</param>
        public static void CreateTopicTags(string tagList, int topicId, int userId, string currentDateTime)
        {
            DatabaseProvider.GetInstance().CreateTopicTags(tagList, topicId, userId, currentDateTime);
        }
    }
}
