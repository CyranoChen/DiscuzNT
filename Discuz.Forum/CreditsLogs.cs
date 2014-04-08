using System.Data;

using Discuz.Common;
using Discuz.Data;

namespace Discuz.Forum
{
    /// <summary>
    /// ����ת����ʷ��¼������
    /// </summary>
    public class CreditsLogs
    {

        /// <summary>
        /// ��ӻ���ת�ʶһ��ͳ�ֵ��¼
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <param name="fromto">����/��</param>
        /// <param name="sendcredits">������������</param>
        /// <param name="receivecredits">�õ���������</param>
        /// <param name="send">������������</param>
        /// <param name="receive">�õ���������</param>
        /// <param name="paydate">ʱ��</param>
        /// <param name="operation">���ֲ���(1=�һ�, 2=ת��, 3=��ֵ)</param>
        /// <returns>ִ��Ӱ�����</returns>
        public static int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {
            return uid > 0 ? Discuz.Data.CreditsLogs.AddCreditsLog(uid, fromto, sendcredits, receivecredits, send, receive, paydate, operation) : 0;
        }

        /// <summary>
        /// ����ָ����Χ�Ļ�����־
        /// </summary>
        /// <param name="pagesize">ҳ��С</param>
        /// <param name="currentpage">��ǰҳ��</param>
        /// <param name="uid">�û�id</param>
        /// <returns>������־</returns>
        public static DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
        {
            return (uid > 0 && currentpage > 0) ? Discuz.Data.CreditsLogs.GetCreditsLogList(pagesize, currentpage, uid) : new DataTable();
        }

        /// <summary>
        /// ���ָ���û��Ļ��ֽ�����ʷ��¼������
        /// </summary>
        /// <param name="uid">�û�id</param>
        /// <returns>��ʷ��¼������</returns>
        public static int GetCreditsLogRecordCount(int uid)
        {
            return uid > 0 ? Discuz.Data.CreditsLogs.GetCreditsLogRecordCount(uid) : 0;
        }
    }

}
