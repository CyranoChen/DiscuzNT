using System;
using System.Data;
using System.Data.Common;

using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// AdminModeratorManageLogFactory ��ժҪ˵����
    /// ǰ̨������־���������
    /// </summary>
    public class AdminModeratorLogs
    {
        /// <summary>
        /// �������������־��¼
        /// </summary>
        /// <param name="moderatorname">������</param>
        /// <param name="grouptitle">�������ID</param>
        /// <param name="ip">�ͻ��˵�IP</param>
        /// <param name="fname">��������</param>
        /// <param name="title">���������</param>
        /// <param name="actions">����</param>
        /// <param name="reason">ԭ��</param>
        /// <returns></returns>
        public static bool InsertLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            return Discuz.Data.ModeratorManageLog.InsertLog(moderatoruid, moderatorname, groupid, grouptitle, ip, postdatetime, fid, fname, tid, title, actions, reason);
        }


        /// <summary>
        /// ��ָ������ɾ����־
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public static bool DeleteLog(string condition)
        {
            return Discuz.Data.ModeratorManageLog.DeleteLog(condition);
        }


        public static string SearchModeratorManageLog(string keyword)
        {
            return Discuz.Data.ModeratorManageLog.SearchModeratorManageLog(keyword);
        }

        /// <summary>
        /// �õ���ǰָ��ҳ����ǰ̨������־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage)
        {
            return LogList(pagesize, currentpage, "");
        }


        /// <summary>
        /// �õ���ǰָ��������ҳ����ǰ̨������־��¼(��)
        /// </summary>
        /// <param name="pagesize">��ǰ��ҳ�ĳߴ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public static DataTable LogList(int pagesize, int currentpage, string condition)
        {
            return Data.ModeratorManageLog.GetModeratorLogList(pagesize, currentpage, condition);
        }


        /// <summary>
        /// �õ�ǰ̨������־��¼��
        /// </summary>
        /// <returns></returns>
        public static int RecordCount()
        {
            return Discuz.Data.ModeratorManageLog.RecordCount();
        }


        /// <summary>
        /// �õ�ָ����ѯ�����µ�ǰ̨������־��
        /// </summary>
        /// <param name="condition">��ѯ����</param>
        /// <returns></returns>
        public static int RecordCount(string condition)
        {
            return Discuz.Data.ModeratorManageLog.RecordCount(condition);
        }

        /// <summary>
        /// ��ȡɾ��������־����
        /// </summary>
        /// <param name="deleteMode">ɾ����ʽ</param>
        /// <param name="id">Id</param>
        /// <param name="deleteNum">ɾ������</param>
        /// <param name="deleteFrom">ɾ����</param>
        /// <returns></returns>
        public static string GetDeleteModeratorManageCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return Data.ModeratorManageLog.GetDeleteModeratorManageCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        /// <summary>
        /// ��ȡ������־��������
        /// </summary>
        /// <param name="postDateTimeStart">��ʼ����</param>
        /// <param name="postDateTimeEnd">��������</param>
        /// <param name="userName">�û���</param>
        /// <param name="others">����</param>
        /// <returns></returns>
        public static string GetSearchModeratorManageLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return Data.ModeratorManageLog.GetSearchModeratorManageLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
        }
    }
}