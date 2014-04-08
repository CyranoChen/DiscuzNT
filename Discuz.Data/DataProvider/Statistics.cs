using System;
using System.Text;
using System.Data;

using Discuz.Common;

namespace Discuz.Data
{
    public class Statistics
    {
        /// <summary>
        /// 获得统计列
        /// </summary>
        /// <returns>统计列</returns>
        public static DataTable GetStatisticsRow()
        {
           return DatabaseProvider.GetInstance().GetStatisticsRow();
        }

        /// <summary>
        /// 获取指定版块中的主题帖子统计数据
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="todaypostcount"></param>
        public static void GetAllForumStatistics(out int topiccount, out int postcount, out int todaypostcount)
        {
            topiccount = 0;
            postcount = 0;
            todaypostcount = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetAllForumStatistics();

            while (reader.Read())
            {
                topiccount = TypeConverter.ObjectToInt(reader["topiccount"], 0);
                postcount = TypeConverter.ObjectToInt(reader["postcount"], 0);
                todaypostcount = TypeConverter.ObjectToInt(reader["todaypostcount"], 0);
            }
            reader.Close();
        }

        /// <summary>
        /// 获取指定版块中的主题帖子统计数据
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="todaypostcount"></param>
        public static void GetForumStatistics(int fid, out int topiccount, out int postcount, out int todaypostcount)
        {
            topiccount = 0;
            postcount = 0;
            todaypostcount = 0;
            IDataReader reader = DatabaseProvider.GetInstance().GetForumStatistics(fid);

            while (reader.Read())
            {
                topiccount = TypeConverter.ObjectToInt(reader["topiccount"], 0);
                postcount = TypeConverter.ObjectToInt(reader["postcount"], 0);
                todaypostcount = TypeConverter.ObjectToInt(reader["todaypostcount"], 0);
            }
            reader.Close();
        }

        /// <summary>
        /// 更新指定名称的统计项
        /// </summary>
        /// <param name="param">项目名称</param>
        /// <param name="Value">指定项的值</param>
        /// <returns>更新数</returns>
        public static int UpdateStatistics(string param, string strValue)
        {
            return DatabaseProvider.GetInstance().UpdateStatistics(param, strValue);
        }

        /// <summary>
        /// 更新最后回复人用户名
        /// </summary>
        /// <param name="uid">Uid</param>
        /// <param name="newUserName">新用户名</param>
        /// <returns></returns>
        public static int UpdateStatisticsLastUserName(int uid, string newUserName)
        {
            return DatabaseProvider.GetInstance().UpdateStatisticsLastUserName(uid, newUserName);
        }
    }
}
