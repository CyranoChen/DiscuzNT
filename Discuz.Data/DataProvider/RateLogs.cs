using System;
using System.Text;
using System.Data;

using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Data
{
    public class RateLogs
    {
        /// <summary>
        /// 删除评分日志
        /// </summary>
        /// <param name="pid">帖子Id</param>
        public static void DeleteRateLog(int pid)
        {
            DatabaseProvider.GetInstance().DeleteRateLog(pid);
        }

        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <returns>帖子评分列表</returns>
        public static List<RateLogInfo> GetPostRateLogList(int pid)
        {
            List<RateLogInfo> rateLogList = new List<RateLogInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetPostRateLogs(pid, GeneralConfigs.GetConfig().DisplayRateCount);
            while (reader.Read())
            {
                rateLogList.Add(LoadSingleRateLogInfo(reader));
            }
            reader.Close();
            return rateLogList;
        }

        /// <summary>
        /// 获取分页帖子评分列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<RateLogInfo> GetPostRateLogList(int pid, int pageIndex, int pageSize)
        {
            List<RateLogInfo> rateLogList = new List<RateLogInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetPostRateLogList(pid, pageIndex, pageSize);

            while (reader.Read())
            {
                rateLogList.Add(LoadSingleRateLogInfo(reader));
            }
            reader.Close();

            return rateLogList;
        }

        /// <summary>
        /// 获取评分数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static int GetPostRateLogCount(int pid)
        {
            return DatabaseProvider.GetInstance().GetPostRateLogCount(pid);
        }

        #region private method

        /// <summary>
        /// 装载评分日志信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static RateLogInfo LoadSingleRateLogInfo(IDataReader reader)
        {
            RateLogInfo rateLogInfo = new RateLogInfo();

            rateLogInfo.Id = TypeConverter.ObjectToInt(reader["id"]);
            rateLogInfo.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            rateLogInfo.PostDateTime = reader["postdatetime"].ToString();
            rateLogInfo.Reason = reader["reason"].ToString();
            rateLogInfo.Score = TypeConverter.ObjectToInt(reader["score"]);
            rateLogInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            rateLogInfo.UserName = reader["username"].ToString().Trim();
            rateLogInfo.ExtCredits = TypeConverter.ObjectToInt(reader["extcredits"]);

            return rateLogInfo;
        }

        #endregion

        /// <summary>
        /// 创建评分记录
        /// </summary>
        /// <param name="postidlist">被评分帖子pid</param>
        /// <param name="userId">评分者uid</param>
        /// <param name="userName">评分者用户名</param>
        /// <param name="extid">分的积分类型</param>
        /// <param name="score">积分数值</param>
        /// <param name="reason">评分理由</param>
        /// <returns>更新数据行数</returns>
        public static int CreateRateLog(int pid, int userid, string userName, int extid, float score, string reason)
        {
            return DatabaseProvider.GetInstance().InsertRateLog(pid, userid, userName, extid, score, reason);
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public static bool DeleteRateLog()
        {
            return DatabaseProvider.GetInstance().DeleteRateLog();
        }

        /// <summary>
        /// 得到当前指定页数的评分日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public static DataTable GetRateLogList(int pageSize, int currentPage, string postTableName)
        {
            return DatabaseProvider.GetInstance().RateLogList(pageSize, currentPage, postTableName);
        }

        /// <summary>
        /// 得到评分日志记录数
        /// </summary>
        /// <returns></returns>
        public static int GetRateLogCount()
        {
            return DatabaseProvider.GetInstance().GetRateLogCount();
        }

        /// <summary>
        /// 按条件删除评分日志
        /// </summary>
        /// <param name="condition">条件</param>
        public static bool DeleteRateLog(string condition)
        {
            return DatabaseProvider.GetInstance().DeleteRateLog(condition);
        }

        /// <summary>
        /// 按条件获取评分日志列表
        /// </summary>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="postTableName">分表名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetRateLogList(int pageSize, int currentPage, string postTableName, string condition)
        {
            return Discuz.Data.DatabaseProvider.GetInstance().RateLogList(pageSize, currentPage, postTableName, condition);
        }

        /// <summary>
        /// 按条件获取评分日志的条数
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static int GetRateLogCount(string condition)
        {
            return Discuz.Data.DatabaseProvider.GetInstance().GetRateLogCount(condition);
        }

        /// <summary>
        /// 获取评分日志条件
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="postidList">帖子Id列表</param>
        /// <returns></returns>
        public static string GetRateLogCountCondition(int userId, string postidList)
        {
            return DatabaseProvider.GetInstance().GetRateLogCountCondition(userId, postidList);
        }

        /// <summary>
        /// 获取评分日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        public static string GetSearchRateLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return DatabaseProvider.GetInstance().SearchRateLog(postDateTimeStart, postDateTimeEnd, userName, others);
        }
    }
}
