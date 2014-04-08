using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    public class CreditsLogs
    {
        /// <summary>
        /// 添加积分转帐兑换记录
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fromto">来自/到</param>
        /// <param name="sendcredits">付出积分类型</param>
        /// <param name="receivecredits">得到积分类型</param>
        /// <param name="send">付出积分数额</param>
        /// <param name="receive">得到积分数额</param>
        /// <param name="paydate">时间</param>
        /// <param name="operation">积分操作(1=兑换, 2=转帐)</param>
        /// <returns>执行影响的行</returns>
        public static int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {
            return DatabaseProvider.GetInstance().AddCreditsLog(uid, fromto, sendcredits, receivecredits, send, receive, paydate, operation);
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
            DataTable dt = DatabaseProvider.GetInstance().GetCreditsLogList(pagesize, currentpage, uid);
            if (dt != null)
            {

                DataColumn dc = new DataColumn();
                dc.ColumnName = "operationinfo";
                dc.DataType = System.Type.GetType("System.String");
                dc.DefaultValue = "";
                dc.AllowDBNull = false;
                dt.Columns.Add(dc);
                foreach (DataRow dr in dt.Rows)
                {
                    switch (dr["operation"].ToString())
                    {
                        case "1":
                            dr["operationinfo"] = "兑换";
                            break;
                        case "2":
                            dr["operationinfo"] = "转帐";
                            break;
                        case "3":
                            dr["operationinfo"] = "充值";
                            break;
                        default:
                            dr["operationinfo"] = "未知操作类型";
                            break;
                    }


                }
            }
            //dt.Dispose();
            return dt;
        }

        /// <summary>
        /// 获得指定用户的积分交易历史记录总条数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>历史记录总条数</returns>
        public static int GetCreditsLogRecordCount(int uid)
        {
            return DatabaseProvider.GetInstance().GetCreditsLogRecordCount(uid);
        }

    }
}
