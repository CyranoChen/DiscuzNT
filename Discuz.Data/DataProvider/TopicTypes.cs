using System;
using System.Data;

namespace Discuz.Data
{
    public class TopicTypes
    {
        /// <summary>
        /// 获取主题分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetTopicTypes(string searthKeyWord)
        {
            return DatabaseProvider.GetInstance().GetTopicTypes(searthKeyWord);
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
            DatabaseProvider.GetInstance().UpdateTopicTypes(name, displayorder, description, typeid);
        }

        /// <summary>
        /// 更新主题分类
        /// </summary>
        /// <param name="topictypes">板块的主题分类</param>
        /// <param name="fid">版块ID</param>
        public static void UpdateForumTopicType(string topictypes, int fid)
        {
            DatabaseProvider.GetInstance().UpdateTopicTypeForForum(topictypes, fid);
        }

        /// <summary>
        /// 创建新主题分类
        /// </summary>
        /// <param name="typeName">主题分类名称</param>
        /// <param name="displayorder">序号</param>
        /// <param name="description">描述</param>
        public static void CreateTopicTypes(string typeName, int displayorder, string description)
        {
            DatabaseProvider.GetInstance().AddTopicTypes(typeName, displayorder, description);
        }

        /// <summary>
        /// 删除主题分类
        /// </summary>
        /// <param name="typeidList">主题分类Id列表</param>
        public static void DeleteTopicTypes(string typeidList)
        {
            DatabaseProvider.GetInstance().DeleteTopicTypesByTypeidlist(typeidList);
        }
    }
}
