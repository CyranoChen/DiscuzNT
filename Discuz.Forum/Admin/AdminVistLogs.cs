using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminVistLogFactory ��ժҪ˵����
	/// ��̨������־���������
	/// </summary>
	public class AdminVistLogs
	{
		/// <summary>
		/// �������������־��¼
		/// </summary>
		/// <param name="uid">�û�UID</param>
		/// <param name="username">�û���</param>
		/// <param name="groupid">������ID</param>
		/// <param name="grouptitle">����������</param>
		/// <param name="ip">IP��ַ</param>
		/// <param name="actions">����</param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static bool InsertLog(int uid, string userName, int groupId, string groupTitle, string ip, string actions, string others)
		{
			try
			{
                Discuz.Data.AdminVisitLogs.InsertLog(uid, userName, groupId, groupTitle, ip, actions, others);
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// ɾ����־
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
			try
			{
                Discuz.Data.AdminVisitLogs.DeleteLog();
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// ��ָ������ɾ����־
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
			try
			{
                Discuz.Data.AdminVisitLogs.DeleteLog(condition);
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// �õ���ǰָ��ҳ���ĺ�̨������־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <returns></returns>
		public static DataTable LogList(int pageSize, int currentPage)
		{
            return Discuz.Data.AdminVisitLogs.LogList(pageSize, currentPage);
		}


		/// <summary>
		/// �õ���ǰָ��������ҳ���ĺ�̨������־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static DataTable LogList(int pageSize, int currentPage, string condition)
		{
            return Discuz.Data.AdminVisitLogs.LogList(pageSize, currentPage, condition);
		}


		/// <summary>
		/// �õ���̨������־��¼��
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return Discuz.Data.AdminVisitLogs.RecordCount();
		}


		/// <summary>
		/// �õ�ָ����ѯ�����µĺ�̨������־��¼��
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return Discuz.Data.AdminVisitLogs.RecordCount(condition);
        }

        /// <summary>
        /// ��ȡ������־ɾ������
        /// </summary>
        /// <param name="deleteMod">ɾ����ʽ</param>
        /// <param name="visitId">������־Id</param>
        /// <param name="deleteNum">ɾ������</param>
        /// <param name="deleteFrom">ɾ���Ӻ�ʱ��</param>
        /// <returns></returns>
        public static string DelVisitLogCondition(string deleteMod, string visitId, string deleteNum, string deleteFrom)
        {
            return Data.AdminVisitLogs.DelVisitLogCondition(deleteMod, visitId, deleteNum, deleteFrom);
        }

        /// <summary>
        /// ��ȡ������־����
        /// </summary>
        /// <param name="postDateTimeStart">������ʼ����</param>
        /// <param name="postDateTimeEnd">���ʽ�������</param>
        /// <param name="userName">�û���</param>
        /// <param name="others">����</param>
        /// <returns></returns>
        public static string GetVisitLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return Data.AdminVisitLogs.GetVisitLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
        }

	}
}