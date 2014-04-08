using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class AdminVisitLogs
    {
        /// <summary>
        /// 插入版主管理日志记录
        /// </summary>
        /// <param name="uid">用户UID</param>
        /// <param name="username">用户名</param>
        /// <param name="groupid">所属组ID</param>
        /// <param name="grouptitle">所属组名称</param>
        /// <param name="ip">IP地址</param>
        /// <param name="actions">动作</param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static void InsertLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
        {
            Discuz.Data.DatabaseProvider.GetInstance().AddVisitLog(uid, username, groupid, grouptitle, ip, actions, others);
        }


        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public static void DeleteLog()
        {
            DatabaseProvider.GetInstance().DeleteVisitLogs();
        }


        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static void DeleteLog(string condition)
        {
            DatabaseProvider.GetInstance().DeleteVisitLogs(condition);
        }


        /// <summary>
        /// 得到当前指定页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage)
        {
            return DatabaseProvider.GetInstance().GetVisitLogList(pagesize, currentpage);
        }


        /// <summary>
        /// 得到当前指定条件和页数的后台访问日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage, string condition)
        {
            return DatabaseProvider.GetInstance().GetVisitLogList(pagesize, currentpage, condition);
        }


        /// <summary>
        /// 得到后台访问日志记录数
        /// </summary>
        /// <returns></returns>
        public static int RecordCount()
        {
            return DatabaseProvider.GetInstance().GetVisitLogCount();
        }


        /// <summary>
        /// 得到指定查询条件下的后台访问日志记录数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int RecordCount(string condition)
        {
            return DatabaseProvider.GetInstance().GetVisitLogCount(condition);
        }

        /// <summary>
        /// 获取管理日志删除条件
        /// </summary>
        /// <param name="deleteMod">删除方式</param>
        /// <param name="visitId">管理日志Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除从何时起</param>
        /// <returns></returns>
        public static string DelVisitLogCondition(string deleteMod, string visitId, string deleteNum, string deleteFrom)
        {
            return DatabaseProvider.GetInstance().DelVisitLogCondition(deleteMod, visitId, deleteNum, deleteFrom);
        }

        /// <summary>
        /// 获取管理日志条件
        /// </summary>
        /// <param name="postDateTimeStart">访问起始日期</param>
        /// <param name="postDateTimeEnd">访问结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        public static string GetVisitLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return DatabaseProvider.GetInstance().SearchVisitLog(postDateTimeStart, postDateTimeEnd, userName, others);
        }
    }
}
