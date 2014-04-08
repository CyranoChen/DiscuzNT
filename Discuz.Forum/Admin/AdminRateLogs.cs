using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminRateLogFactory ��ժҪ˵����
	/// ��̨������־���������
	/// </summary>
	public class AdminRateLogs
	{
		/// <summary>
		/// ������ּ�¼
		/// </summary>
		/// <param name="postidlist">����������pid</param>
		/// <param name="userid">������uid</param>
		/// <param name="username">�������û���</param>
		/// <param name="extid">�ֵĻ�������</param>
		/// <param name="score">������ֵ</param>
		/// <param name="reason">��������</param>
		/// <returns>������������</returns>
		public static int InsertLog(string postidlist, int userid, string username, int extid, float score, string reason)
		{
            int reval = 0;
            foreach (string pid in Utils.SplitString(postidlist, ","))
            {
                reval += Data.RateLogs.CreateRateLog(Utils.StrToInt(pid, 0), userid, username, extid, score, reason);
            }
            return reval;
		}


		/// <summary>
		/// ɾ����־
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
            return Data.RateLogs.DeleteRateLog();
		}

		/// <summary>
		/// ��ָ������ɾ����־
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return Data.RateLogs.DeleteRateLog(condition);
		}

		/// <summary>
		/// �õ���ǰָ��ҳ����������־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <returns></returns>
		public static DataTable GetRateLogList(int pagesize, int currentpage)
		{
            return Data.RateLogs.GetRateLogList(pagesize, currentpage, Posts.GetPostTableName());
		}

		/// <summary>
		/// �õ���ǰָ��������ҳ����������־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return Data.RateLogs.GetRateLogList(pagesize, currentpage, Posts.GetPostTableName(), condition);
		}

		/// <summary>
		/// �õ�������־��¼��
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return Data.RateLogs.GetRateLogCount();
		}

		/// <summary>
		/// �õ�ָ����ѯ�����µ�������־��
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return Data.RateLogs.GetRateLogCount(condition);
        }

        /// <summary>
        /// ��ȡ������־��������
        /// </summary>
        /// <param name="postDateTimeStart">��ʼ����</param>
        /// <param name="postDateTimeEnd">��������</param>
        /// <param name="userName">�û���</param>
        /// <param name="others">����</param>
        /// <returns></returns>
        public static string GetSearchRateLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return Data.RateLogs.GetSearchRateLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
        }

        public static string GetRateLogCountCondition(int userid, string postidlist)
        {
            return Data.RateLogs.GetRateLogCountCondition(userid, postidlist);
        }
    }
}
