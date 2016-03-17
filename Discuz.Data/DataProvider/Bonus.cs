using System;
using System.Text;
using System.Data;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    public class Bonus
    {
        /// <summary>
        /// 添加悬赏日志
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="authorid">悬赏者Id</param>
        /// <param name="winerid">获奖者Id</param> 
        /// <param name="winnerName">获奖者用户名</param>
        /// <param name="postid">帖子Id</param>
        /// <param name="bonus">奖励积分</param>
        /// <param name="isbest">是否是最佳答案，0=不是,1=是较好的答案,2=最佳答案</param>
        public static void AddLog(int tid, int authorid, int winerid, string winnerName, int postid, int bonus, int extid,int isbest)
        {
            DatabaseProvider.GetInstance().AddBonusLog(tid, authorid, winerid, winnerName, postid, bonus, extid, isbest);
        }

        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postTableId">主题所在的分表ID</param>
        /// <returns>悬赏日志集合</returns>
        public static List<BonusLogInfo> GetLogs(int tid, string postTableId)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicBonusLogs(tid, postTableId);
            List<BonusLogInfo> blis = new List<BonusLogInfo>();

            while (reader.Read())
            {
                BonusLogInfo bli = new BonusLogInfo();
                bli.Tid = TypeConverter.ObjectToInt(reader["tid"], 0);
                bli.Authorid = TypeConverter.ObjectToInt(reader["authorid"], 0);
                bli.Answerid = TypeConverter.ObjectToInt(reader["answerid"], 0);
                bli.Answername = reader["answername"].ToString();
                bli.Pid = TypeConverter.ObjectToInt(reader["pid"], 0);
                bli.Bonus = TypeConverter.ObjectToInt(reader["bonus"], 0);
                bli.Isbest = TypeConverter.ObjectToInt(reader["isbest"], 0);
                bli.Extid = Convert.ToByte(reader["extid"]);
                bli.Message = reader["message"].ToString();
                blis.Add(bli);
            }
            reader.Close();
            return blis;
        }


        /// <summary>
        /// 获取指定主题的给分记录
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public static Dictionary<int, BonusLogInfo> GetLogsForEachPost(int tid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicBonusLogsByPost(tid);
            Dictionary<int, BonusLogInfo> blis = new Dictionary<int, BonusLogInfo>();

            while (reader.Read())
            {
                BonusLogInfo bli = new BonusLogInfo();
                bli.Pid = TypeConverter.ObjectToInt(reader["pid"], 0);
                bli.Bonus = TypeConverter.ObjectToInt(reader["bonus"], 0);
                bli.Isbest = TypeConverter.ObjectToInt(reader["isbest"], 0);
                bli.Extid = (byte)reader["extid"];
                blis[bli.Pid] = bli;
            }
            reader.Close();
            return blis;
        }
    }
}
