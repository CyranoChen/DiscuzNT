using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Data
{
    public class TopicTagCaches
    {
        /// <summary>
        /// 整理相关主题表
        /// </summary>
        public static void NeatenRelateTopics()
        {
            DatabaseProvider.GetInstance().NeatenRelateTopics();
        }

        /// <summary>
        /// 根据主题的Tag获取相关主题(游客可见级别的)
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<TopicInfo> GetRelatedTopicList(int topicId, int count)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetRelatedTopics(topicId, count);

            Discuz.Common.Generic.List<TopicInfo> topics = new Discuz.Common.Generic.List<TopicInfo>();
            while (reader.Read())
            {
                TopicInfo topic = new TopicInfo();
                topic.Tid = TypeConverter.ObjectToInt(reader["linktid"]);
                topic.Title = reader["linktitle"].ToString();
                topics.Add(topic);
            }

            reader.Close();
            return topics;
        }

        /// <summary>
        /// 删除主题的相关主题记录
        /// </summary>
        /// <param name="topicid"></param>
        public static void DeleteRelatedTopics(int topicId)
        {
            DatabaseProvider.GetInstance().DeleteRelatedTopics(topicId);
        }
    }
}
