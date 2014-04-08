using System;
using System.Data;
using System.Data.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminModeratorManageLogFactory 的摘要说明。
	/// 勋章日志操作管理类
	/// </summary>
	public class AdminMedalLogs
	{
		
		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return Discuz.Data.Medals.DeleteLog(condition);
        }


		/// <summary>
		/// 得到当前指定页数的勋章日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage)
        {
            return Discuz.Data.Medals.LogList(pagesize, currentpage);
        }


		/// <summary>
		/// 得到当前指定条件和页数的勋章日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return Discuz.Data.Medals.LogList(pagesize, currentpage, condition);
		}


		/// <summary>
		/// 得到缓存日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return Discuz.Data.Medals.RecordCount();
		}


		/// <summary>
		/// 得到指定查询条件下的勋章日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return Discuz.Data.Medals.RecordCount(condition);
		}

        /// <summary>
        /// 获取删除勋章日志条件
        /// </summary>
        /// <param name="deleteMode">删除方式</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除到</param>
        /// <returns></returns>
        public static string GetDelMedalLogCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return Data.Medals.GetDelMedalLogCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        /// <summary>
        /// 获取搜索勋章列表条件
        /// </summary>
        /// <param name="postDateTimeStart">授予开始日期</param>
        /// <param name="postDateTimeEnd">授予结束日期</param>
        /// <param name="userName">授予人</param>
        /// <param name="reason">理由</param>
        /// <returns></returns>
        public static string GetSearchMedalLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string reason)
        {
            return Data.Medals.GetSearchMedalLogCondition(postDateTimeStart, postDateTimeEnd, userName, reason);
        }
	}
}