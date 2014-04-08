using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class PaymentLogs
    {
        /// <summary>
        /// 添加积分交易日志
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="posterid">发帖人ID</param>
        /// <param name="price">售价</param>
        /// <param name="netamount">净收入</param>
        public static int CreatePaymentLog(int uid, int tid, int posterid, int price, float netamount)
        {
            return DatabaseProvider.GetInstance().CreatePaymentLog(uid, tid, posterid, price, netamount);
        }

        /// <summary>
        /// 判断用户是否已购买主题
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static bool IsBuyer(int tid, int uid)
        {
            return DatabaseProvider.GetInstance().IsBuyer(tid, uid);
        }

        /// <summary>
        /// 获取指定用户的交易日志
        /// </summary>
        /// <param name="pagesize">每页条数</param>
        /// <param name="currentpage">当前页</param>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
        {
            return DatabaseProvider.GetInstance().GetPayLogInList(pagesize, currentpage, uid);
        }

        /// <summary>
        /// 获取指定用户的收入日志记录数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetPaymentLogInRecordCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogInRecordCount(uid);
        }

        /// <summary>
        /// 返回指定用户支出日志总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        public static int GetPaymentLogOutRecordCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogOutRecordCount(uid);
        }

        /// <summary>
        /// 获取指定主题的购买记录
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public static DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetPaymentLogByTid(pagesize, currentpage, tid);
            if (dt == null)
                dt = new DataTable();

            return dt;
        }

        /// <summary>
        /// 主题购买总次数
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public static int GetPaymentLogByTidCount(int tid)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogByTidCount(tid);
        }

        /// <summary>
        /// 得到积分交易日志记录数
        /// </summary>
        /// <returns></returns>
        public static int RecordCount()
        {
            return DatabaseProvider.GetInstance().GetPaymentLogListCount();
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <returns></returns>
        public static bool DeleteLog()
        {
            return DatabaseProvider.GetInstance().DeletePaymentLog();
        }



        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return DatabaseProvider.GetInstance().DeletePaymentLog(condition);
        }

        /// <summary>
        /// 分页获取日志
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <returns></returns>
        public static DataTable GetPaymentLogList(int pagesize, int currentpage)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogList(pagesize, currentpage);
        }

        /// <summary>
        /// 分页获取日志
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="currentpage">当前页码</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetPaymentLogList(int pagesize, int currentpage, string condition)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogList(pagesize, currentpage,condition);
        }

        /// <summary>
        /// 得到指定查询条件下的积分交易日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static int RecordCount(string condition)
        {
            return DatabaseProvider.GetInstance().GetPaymentLogListCount(condition);
        }

        /// <summary>
        /// 获取交易日志搜索条件
        /// </summary>
        /// <param name="postDateTimeStart">起始日期</param>
        /// <param name="postDateTimeEnd">结束日期</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static string GetSearchPaymentLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName)
        {
            return DatabaseProvider.GetInstance().SearchPaymentLog(postDateTimeStart, postDateTimeEnd, userName);
        }
    }
}
