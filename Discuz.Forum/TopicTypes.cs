using System;
using System.Text;
using System.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class TopicTypes
    {
        /// <summary>
        /// 获取主题分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTopicTypes(string searthKeyWord)
        {
            return Data.TopicTypes.GetTopicTypes(searthKeyWord);
        }
        
        public static DataTable GetTopicTypes()
        {
            return GetTopicTypes("");
        }

        /// <summary>
        /// 更新主题分类
        /// </summary>
        /// <param name="name">分类名称</param>
        /// <param name="displayorder">排序号</param>
        /// <param name="description">介绍</param>
        /// <param name="typeid">分类ID</param>
        public static void UpdateTopicTypes(string name, int displayorder, string description, int typeid)
        {
            Data.TopicTypes.UpdateTopicTypes(name, displayorder, description, typeid);
        }

        /// <summary>
        /// 更新主题分类
        /// </summary>
        /// <param name="topictypes">板块的主题分类</param>
        /// <param name="fid">版块ID</param>
        public static void UpdateForumTopicType(string topictypes, int fid)
        {
            Data.TopicTypes.UpdateForumTopicType(topictypes, fid);
        }

        /// <summary>
        /// 检测主题分类是否存在
        /// </summary>
        /// <param name="topicTypeName">主题分类名称</param>
        /// <returns></returns>
        public static bool IsExistTopicType(string topicTypeName)
        {
            foreach (DataRow dr in GetTopicTypes().Rows)
            {
                if (dr["name"].ToString() == topicTypeName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 检测主题分类除指定主题分类Id外是否存在
        /// </summary>
        /// <param name="topicTypeName">主题分类名称</param>
        /// <param name="typeid">主题分类Id</param>
        /// <returns></returns>
        public static bool IsExistTopicType(string topicTypeName, int typeid)
        {
            foreach (DataRow dr in GetTopicTypes().Rows)
            {
                if (dr["name"].ToString() == topicTypeName && dr["id"].ToString() != typeid.ToString())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 创建新主题分类
        /// </summary>
        /// <param name="typeName">主题分类名称</param>
        /// <param name="displayorder">序号</param>
        /// <param name="description">描述</param>
        public static void CreateTopicTypes(string typeName, int displayorder, string description)
        {
            Data.TopicTypes.CreateTopicTypes(typeName, displayorder, description);
        }

        /// <summary>
        /// 删除主题分类
        /// </summary>
        /// <param name="typeidList">主题分类Id列表</param>
        public static void DeleteTopicTypes(string typeidList)
        {
            Data.TopicTypes.DeleteTopicTypes(typeidList);
        }

        public static int GetMaxTopicTypesId()
        {
            DataTable dt = GetTopicTypes();
            return dt == null ? 0 : Discuz.Common.TypeConverter.ObjectToInt(dt.Compute("Max(id)",""));
        }

        /// <summary>
        /// 删除所选的主题分类
        /// </summary>
        /// <param name="typeidlist"></param>
        public static void DeleteForumTopicTypes(string typeidlist)
        {
            //取得ID的数组
            string[] ids = typeidlist.Split(',');

            //取得主题分类的缓存
            Discuz.Common.Generic.SortedList<int, string> topictypearray = new Discuz.Common.Generic.SortedList<int, string>();
            topictypearray = Caches.GetTopicTypeArray();

            //取得版块的fid,topictypes字段

            DataTable dt = Forums.GetForumListForDataTable();

            //处理每一个版块
            foreach (DataRow dr in dt.Rows)
            {
                //如果版块的主题分类字段为空（topictypes==""），则处理下一个
                if (dr["topictypes"].ToString() == "") continue;

                string topictypes = dr["topictypes"].ToString();
                //处理每一个要删除的ID
                foreach (string id in ids)
                {
                    //将删除的ID拼成相应的格式串后，将原来的剔除掉，形成一个新的主题分类的字段
                    topictypes = topictypes.Replace(id + "," + topictypearray[Int32.Parse(id)].ToString() + ",0|", "");
                    topictypes = topictypes.Replace(id + "," + topictypearray[Int32.Parse(id)].ToString() + ",1|", "");
                    //将帖子列表（dnt_topics）中typeid为当前要删除的Id更新为0
                    Data.Topics.ClearTopicType(int.Parse(id));
                }
                //用剔除了要删除的主题ID的主题列表值更新数据库
                ForumInfo forumInfo = Forums.GetForumInfo(int.Parse(dr["fid"].ToString()));
                forumInfo.Topictypes = topictypes;
                AdminForums.UpdateForumInfo(forumInfo);
            }
        }
    }
}
