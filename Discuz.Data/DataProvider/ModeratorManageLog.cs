using System;
using System.Text;
using System.Data;

using Discuz.Common;

namespace Discuz.Data
{
    public class ModeratorManageLog
    {

        /// <summary>
        /// 插入版主管理日志记录
        /// </summary>
        /// <param name="moderatorname">版主名</param>
        /// <param name="grouptitle">所属组的ID</param>
        /// <param name="ip">客户端的IP</param>
        /// <param name="fname">版块的名称</param>
        /// <param name="title">主题的名称</param>
        /// <param name="actions">动作</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        public static bool InsertModeratorLog(string moderatoruid, string moderatorname, int groupid, string grouptitle, string ip,
                                string postdatetime, string fid, string fname, string tid, string title, string actions,
                                string reason)
        {
            return DatabaseProvider.GetInstance().InsertModeratorLog(moderatoruid,
                                                              moderatorname,
                                                              groupid.ToString(),
                                                              grouptitle,
                                                              Utils.GetRealIP(),
                                                              Utils.GetDateTime(),
                                                              fid,
                                                              fname,
                                                              tid,
                                                              title,
                                                              actions,
                                                              reason);
        }

        /// <summary>
        /// 插入版主管理日志记录
        /// </summary>
        /// <param name="moderatorname">版主名</param>
        /// <param name="grouptitle">所属组的ID</param>
        /// <param name="ip">客户端的IP</param>
        /// <param name="fname">版块的名称</param>
        /// <param name="title">主题的名称</param>
        /// <param name="actions">动作</param>
        /// <param name="reason">原因</param>
        /// <returns></returns>
        public static bool InsertLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            return DatabaseProvider.GetInstance().InsertModeratorLog(moderatoruid, moderatorname, groupid, grouptitle, ip, postdatetime, fid, fname, tid, title, actions, reason);
        }


        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return DatabaseProvider.GetInstance().DeleteModeratorLog(condition);
        }


        public static string SearchModeratorManageLog(string keyword)
        {
            return DatabaseProvider.GetInstance().SearchModeratorManageLog(keyword);
        }

        /// <summary>
        /// 得到前台管理日志记录数
        /// </summary>
        /// <returns></returns>
        public static int RecordCount()
        {
            return DatabaseProvider.GetInstance().GetModeratorLogListCount();
        }


        /// <summary>
        /// 得到指定查询条件下的前台管理日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int RecordCount(string condition)
        {
            return DatabaseProvider.GetInstance().GetModeratorLogListCount(condition);
        }

        /// <summary>
        /// 按条件得到勋章日志列表
        /// </summary>
        /// <param name="pageSize">每页数</param>
        /// <param name="currentPage">当前页数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetModeratorLogList(int pageSize,int currentPage,string condition)
        {
            return DatabaseProvider.GetInstance().GetModeratorLogList(pageSize, currentPage, condition);
        }

        /// <summary>
        /// 获取删除管理日志条件
        /// </summary>
        /// <param name="deleteMode">删除方式</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">删除条数</param>
        /// <param name="deleteFrom">删除到</param>
        /// <returns></returns>
        public static string GetDeleteModeratorManageCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return DatabaseProvider.GetInstance().DelModeratorManageCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        /// <summary>
        /// 获取管理日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <param name="others">其它</param>
        /// <returns></returns>
        public static string GetSearchModeratorManageLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return DatabaseProvider.GetInstance().SearchModeratorManageLog(postDateTimeStart, postDateTimeEnd, userName, others);
        }
    }
}
