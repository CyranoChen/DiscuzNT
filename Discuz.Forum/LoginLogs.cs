using System.Data;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{

	/// <summary>
	/// 登录日志操作类
	/// </summary>
	public class LoginLogs
	{
        private static object lockHelper = new object();

		/// <summary>
		/// 增加错误次数并返回错误次数, 如不存在登录错误日志则建立
		/// </summary>
		/// <param name="ip">ip地址</param>
        /// <returns>int</returns>
		public static int UpdateLoginLog(string ip, bool update)
		{
            lock (lockHelper)
            {
                DataTable dt = Discuz.Data.LoginLogs.GetErrLoginRecordByIP(ip);           
                if (dt.Rows.Count > 0)
                {
                    int errcount = Utils.StrToInt(dt.Rows[0][0].ToString(), 0);
                    if (Utils.StrDateDiffMinutes(dt.Rows[0][1].ToString(), 0) < 15)
                    {
                        if ((errcount >= 5) || (!update))
                        {
                            return errcount;
                        }
                        else
                        {
                            Discuz.Data.LoginLogs.AddErrLoginCount(ip);
                            return errcount + 1;
                        }
                    }
                    Discuz.Data.LoginLogs.ResetErrLoginCount(ip);
                    return 1;
                }
                else
                {
                    if (update)
                        Discuz.Data.LoginLogs.AddErrLoginRecord(ip);

                    return 1;
                }
            }
		}

		/// <summary>
		/// 删除指定ip地址的登录错误日志
		/// </summary>
		/// <param name="ip">ip地址</param>
        /// <returns>int</returns>
		public static int DeleteLoginLog(string ip)
		{
            return Utils.IsIP(ip) ? Discuz.Data.LoginLogs.DeleteErrLoginRecord(ip) : 0;	
        }
	}
}
