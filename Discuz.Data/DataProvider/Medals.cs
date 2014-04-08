using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class Medals
    {
        /// <summary>
        /// 返回勋章列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMedal()
        {
            return DatabaseProvider.GetInstance().GetMedal();
        }

        /// <summary>
        /// 创建勋章
        /// </summary>
        /// <param name="medalName">勋章名称</param>
        /// <param name="available">是否可用</param>
        /// <param name="image">图片名称</param>
        public static void CreateMedal(string medalName, int available, string image)
        {
            DatabaseProvider.GetInstance().AddMedal(medalName, available, image);
        }

        /// <summary>
        /// 是否存在勋章授予记录
        /// </summary>
        /// <param name="medalId">勋章Id</param>
        /// <param name="uid">用户Id</param>
        /// <returns></returns>
        public static bool IsExistMedalAwardRecord(int medalId, int uid)
        {
            return DatabaseProvider.GetInstance().IsExistMedalAwardRecord(medalId, uid);
        }

        /// <summary>
        /// 创建勋章授予记录
        /// </summary>
        /// <param name="adminUid">管理员Id</param>
        /// <param name="adminUserName">管理员名称</param>
        /// <param name="ip">IP</param>
        /// <param name="giveUserName">授予人名称</param>
        /// <param name="uid">授予人Id</param>
        /// <param name="action">授予方式说明</param>
        /// <param name="medalId">勋章Id</param>
        /// <param name="reason">理由</param>
        public static void CreateMedalslog(int adminUid, string adminUserName, string ip, string giveUserName, int uid, string action, int medalId, string reason)
        {
            DatabaseProvider.GetInstance().AddMedalslog(adminUid, adminUserName, ip, giveUserName, uid, action, medalId, reason);
        }

        /// <summary>
        /// 更新勋章授予记录
        /// </summary>
        /// <param name="newAction">新授予方式说明</param>
        /// <param name="postDateTime">更新日期</param>
        /// <param name="reason">理由</param>
        /// <param name="oldAction">原授予方式说明</param>
        /// <param name="medalId">勋章Id</param>
        /// <param name="uid">授予人Id</param>
        public static void UpdateMedalslog(string newAction, DateTime postDateTime, string reason, string oldAction, int medalId, int uid)
        {
            DatabaseProvider.GetInstance().UpdateMedalslog(newAction, postDateTime, reason, oldAction, medalId, uid);
        }

        /// <summary>
        /// 更新勋章授予记录
        /// </summary>
        /// <param name="newAction">授予方式说明</param>
        /// <param name="postDateTime">更新日期</param>
        /// <param name="reason">理由</param>
        /// <param name="uid">授予人Id</param>
        public static void UpdateMedalslog(string action, DateTime postDateTime, string reason, int uid)
        {
            DatabaseProvider.GetInstance().UpdateMedalslog(action, postDateTime, reason, uid);
        }

        /// <summary>
        /// 更新勋章
        /// </summary>
        /// <param name="medalid">勋章ID</param>
        /// <param name="name">名称</param>
        /// <param name="image">图片</param>
        public static void UpdateMedal(int medalid, string name, string image)
        {
            DatabaseProvider.GetInstance().UpdateMedal(medalid, name, image);
        }

        /// <summary>
        /// 设置勋章为可用
        /// </summary>
        /// <param name="available"></param>
        /// <param name="medailidlist"></param>
        public static void SetAvailableForMedal(int available, string medailIdList)
        {
            DatabaseProvider.GetInstance().SetAvailableForMedal(available, medailIdList);
        }

        /// <summary>
        /// 获取存在的勋章
        /// </summary>
        /// <returns></returns>
        public static DataTable GetExistMedalList()
        {
            return DatabaseProvider.GetInstance().GetExistMedalList();
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public static bool DeleteLog()
        {
            return DatabaseProvider.GetInstance().DeleteMedalLog();
        }


        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return DatabaseProvider.GetInstance().DeleteMedalLog(condition);
        }


        /// <summary>
        /// 得到当前指定页数的勋章日志记录(表)
        /// </summary>
        /// <param name="pagesize">当前分页的尺寸大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage)
        {
            return DatabaseProvider.GetInstance().GetMedalLogList(pagesize, currentpage);
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
            return DatabaseProvider.GetInstance().GetMedalLogList(pagesize, currentpage, condition);
        }


        /// <summary>
        /// 得到缓存日志记录数
        /// </summary>
        /// <returns></returns>
        public static int RecordCount()
        {
            return DatabaseProvider.GetInstance().GetMedalLogListCount();
        }


        /// <summary>
        /// 得到指定查询条件下的勋章日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int RecordCount(string condition)
        {
            return DatabaseProvider.GetInstance().GetMedalLogListCount(condition);
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
            return DatabaseProvider.GetInstance().DelMedalLogCondition(deleteMode, id, deleteNum, deleteFrom);
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
            return DatabaseProvider.GetInstance().SearchMedalLog(postDateTimeStart, postDateTimeEnd, userName, reason);
        }
    }
}
