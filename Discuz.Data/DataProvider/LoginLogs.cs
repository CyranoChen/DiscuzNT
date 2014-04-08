using System;
using System.Text;
using System.Data;

namespace Discuz.Data
{
    /// <summary>
    /// 登陆日志数据操作类
    /// </summary>
    public class LoginLogs
    {
        /// <summary>
        /// 返加登录错误日志列表
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns></returns>
        public static DataTable GetErrLoginRecordByIP(string ip)
        {
            return DatabaseProvider.GetInstance().GetErrLoginRecordByIP(ip);
        }

        /// <summary>
        /// 添加登录错误次数
        /// </summary>
        /// <param name="ip">ip地址</param>
        public static void AddErrLoginCount(string ip)
        {
            DatabaseProvider.GetInstance().AddErrLoginCount(ip);
        }

        /// <summary>
        /// 重置登录错误次数
        /// </summary>
        /// <param name="ip">ip地址</param>
        public static void ResetErrLoginCount(string ip)
        {
            DatabaseProvider.GetInstance().ResetErrLoginCount(ip);
        }

        /// <summary>
        /// 添加登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        public static void AddErrLoginRecord(string ip)
        {
            DatabaseProvider.GetInstance().AddErrLoginRecord(ip);
        }

        /// <summary>
        /// 删除指定ip地址的登录错误日志
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>int</returns>
        public static int DeleteErrLoginRecord(string ip)
        {
            return DatabaseProvider.GetInstance().DeleteErrLoginRecord(ip);
        }
    }
}
