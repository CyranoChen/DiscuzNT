using System.Data;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Forum
{
    /// <summary>
    /// 积分转帐历史记录操作类
    /// </summary>
    public class CreditsLogs
    {

        /// <summary>
        /// 添加积分转帐兑换和充值记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fromto">来自/到</param>
        /// <param name="sendcredits">付出积分类型</param>
        /// <param name="receivecredits">得到积分类型</param>
        /// <param name="send">付出积分数额</param>
        /// <param name="receive">得到积分数额</param>
        /// <param name="paydate">时间</param>
        /// <param name="operation">积分操作(1=兑换, 2=转帐, 3=充值)</param>
        /// <returns>执行影响的行</returns>
        public static int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {
            return uid > 0 ? Discuz.Data.CreditsLogs.AddCreditsLog(uid, fromto, sendcredits, receivecredits, send, receive, paydate, operation) : 0;
        }

        /// <summary>
        /// 返回指定范围的积分日志
        /// </summary>
        /// <param name="pagesize">页大小</param>
        /// <param name="currentpage">当前页数</param>
        /// <param name="uid">用户id</param>
        /// <returns>积分日志</returns>
        public static DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
        {
            return (uid > 0 && currentpage > 0) ? Discuz.Data.CreditsLogs.GetCreditsLogList(pagesize, currentpage, uid) : new DataTable();
        }

        /// <summary>
        /// 获得指定用户的积分交易历史记录总条数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>历史记录总条数</returns>
        public static int GetCreditsLogRecordCount(int uid)
        {
            return uid > 0 ? Discuz.Data.CreditsLogs.GetCreditsLogRecordCount(uid) : 0;
        }
    }

}
