using System;
using System.Data;
using System.Data.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminModeratorManageLogFactory ��ժҪ˵����
	/// ѫ����־����������
	/// </summary>
	public class AdminMedalLogs
	{
		
		/// <summary>
		/// ��ָ������ɾ����־
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return Discuz.Data.Medals.DeleteLog(condition);
        }


		/// <summary>
		/// �õ���ǰָ��ҳ����ѫ����־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage)
        {
            return Discuz.Data.Medals.LogList(pagesize, currentpage);
        }


		/// <summary>
		/// �õ���ǰָ��������ҳ����ѫ����־��¼(��)
		/// </summary>
		/// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
		/// <param name="currentpage">��ǰҳ��</param>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return Discuz.Data.Medals.LogList(pagesize, currentpage, condition);
		}


		/// <summary>
		/// �õ�������־��¼��
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return Discuz.Data.Medals.RecordCount();
		}


		/// <summary>
		/// �õ�ָ����ѯ�����µ�ѫ����־��
		/// </summary>
		/// <param name="condition">��ѯ����</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return Discuz.Data.Medals.RecordCount(condition);
		}

        /// <summary>
        /// ��ȡɾ��ѫ����־����
        /// </summary>
        /// <param name="deleteMode">ɾ����ʽ</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">ɾ������</param>
        /// <param name="deleteFrom">ɾ����</param>
        /// <returns></returns>
        public static string GetDelMedalLogCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return Data.Medals.GetDelMedalLogCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        /// <summary>
        /// ��ȡ����ѫ���б�����
        /// </summary>
        /// <param name="postDateTimeStart">���迪ʼ����</param>
        /// <param name="postDateTimeEnd">�����������</param>
        /// <param name="userName">������</param>
        /// <param name="reason">����</param>
        /// <returns></returns>
        public static string GetSearchMedalLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string reason)
        {
            return Data.Medals.GetSearchMedalLogCondition(postDateTimeStart, postDateTimeEnd, userName, reason);
        }
	}
}