using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Cache;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// ����Ϣ������
    /// </summary>
    public class PrivateMessages
    {
        /// <summary>
        /// ���������û�ע�Ỷӭ�ż����û�����, ������ͬʱ�������û�ע��
        /// </summary>
        public const string SystemUserName = "ϵͳ";


        /// <summary>
        /// ���ָ��ID�Ķ���Ϣ������
        /// </summary>
        /// <param name="pmid">����Ϣpmid</param>
        /// <returns>����Ϣ����</returns>
        public static PrivateMessageInfo GetPrivateMessageInfo(int pmid)
        {
            return pmid > 0 ? Discuz.Data.PrivateMessages.GetPrivateMessageInfo(pmid) : null;
        }

        /// <summary>
        /// �õ����û��Ķ���Ϣ����
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="folder">�����ļ���(0:�ռ���,1:������,2:�ݸ���)</param>
        /// <returns>����Ϣ����</returns>
        public static int GetPrivateMessageCount(int userId, int folder)
        {
            return userId > 0 ? GetPrivateMessageCount(userId, folder, -1) : 0;
        }

        /// <summary>
        /// �õ����û��Ķ���Ϣ����
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="folder">�����ļ���(0:�ռ���,1:������,2:�ݸ���)</param>
        /// <param name="state">����Ϣ״̬(0:�Ѷ�����Ϣ��1:δ������Ϣ��2:�����Ϣ��7���ڣ���-1:ȫ������Ϣ)</param>
        /// <returns>����Ϣ����</returns>
        public static int GetPrivateMessageCount(int userId, int folder, int state)
        {
            return userId > 0 ? Discuz.Data.PrivateMessages.GetPrivateMessageCount(userId, folder, state) : 0;
        }

        /// <summary>
        /// �õ�������Ϣ����
        /// </summary>
        /// <returns>������Ϣ����</returns>
        public static int GetAnnouncePrivateMessageCount()
        {
            DNTCache cache = DNTCache.GetCacheService();
            int announcepmcount = Utils.StrToInt(cache.RetrieveObject("/Forum/AnnouncePrivateMessageCount"), -1);

            if (announcepmcount < 0)
            {
                announcepmcount = Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCount();
                cache.AddObject("/Forum/AnnouncePrivateMessageCount", announcepmcount);
            }
            return announcepmcount;
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="privatemessageinfo">����Ϣ����</param>
        /// <param name="savetosentbox">���ö���Ϣ�Ƿ��ڷ����䱣��(0Ϊ������, 1Ϊ����)</param>
        /// <returns>����Ϣ�����ݿ��е�pmid</returns>
        public static int CreatePrivateMessage(PrivateMessageInfo privatemessageinfo, int savetosentbox)
        {
            return Discuz.Data.PrivateMessages.CreatePrivateMessage(privatemessageinfo, savetosentbox);
        }


        /// <summary>
        /// ɾ��ָ���û��Ķ���Ϣ
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="pmitemid">Ҫɾ���Ķ���Ϣ�б�(����)</param>
        /// <returns>ɾ����¼��</returns>
        public static int DeletePrivateMessage(int userId, string[] pmitemid)
        {
            if (!Utils.IsNumericArray(pmitemid))
                return -1;

            int reval = Discuz.Data.PrivateMessages.DeletePrivateMessages(userId, String.Join(",", pmitemid));
            if (reval > 0)
                Discuz.Data.Users.SetUserNewPMCount(userId, Discuz.Data.PrivateMessages.GetNewPMCount(userId));

            return reval;
        }

        /// <summary>
        /// ɾ��ָ���û���һ������Ϣ
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="pmid">Ҫɾ���Ķ���ϢID</param>
        /// <returns>ɾ����¼��</returns>
        public static int DeletePrivateMessage(int userId, int pmid)
        {
            return userId > 0 ? DeletePrivateMessage(userId, new string[] { pmid.ToString() }) : 0;
        }

        /// <summary>
        /// ���ö���Ϣ״̬
        /// </summary>
        /// <param name="pmid">����ϢID</param>
        /// <param name="state">״ֵ̬</param>
        /// <returns>���¼�¼��</returns>
        public static int SetPrivateMessageState(int pmid, byte state)
        {
            return pmid > 0 ? Discuz.Data.PrivateMessages.SetPrivateMessageState(pmid, state) : 0;
        }

        /// <summary>
        /// ���ָ���û��Ķ���Ϣ�б�
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="folder">����Ϣ����(0:�ռ���,1:������,2:�ݸ���)</param>
        /// <param name="pagesize">ÿҳ��ʾ����Ϣ��</param>
        /// <param name="pageindex">��ǰҪ��ʾ��ҳ��</param>
        /// <param name="inttype">ɸѡ����1Ϊδ��</param>
        /// <returns>����Ϣ�б�</returns>
        public static List<PrivateMessageInfo> GetPrivateMessageCollection(int userId, int folder, int pagesize, int pageindex, int readStatus)
        {
            return (userId > 0 && pageindex > 0 && pagesize > 0) ? Discuz.Data.PrivateMessages.GetPrivateMessageCollection(userId, folder, pagesize, pageindex, readStatus) : null;
        }

        /// <summary>
        /// ��ù�����Ϣ�б�
        /// </summary>
        /// <param name="pagesize">ÿҳ��ʾ����Ϣ��</param>
        /// <param name="pageindex">��ǰҪ��ʾ��ҳ��</param>
        /// <returns>������Ϣ�б�</returns>
        public static List<PrivateMessageInfo> GetAnnouncePrivateMessageCollection(int pagesize, int pageindex)
        {
            if (pagesize == -1)
                return Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCollection(-1, 0);
            return (pagesize > 0 && pageindex > 0) ? Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCollection(pagesize, pageindex) : null;
        }

        /// <summary>
        /// ���ض̱�����ռ������Ϣ�б�
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <param name="pagesize">ÿҳ��ʾ����Ϣ��</param>
        /// <param name="pageindex">��ǰҪ��ʾ��ҳ��</param>
        /// <param name="strwhere">ɸѡ����</param>
        /// <returns>�ռ������Ϣ�б�</returns>
        public static List<PrivateMessageInfo> GetPrivateMessageListForIndex(int userId, int pagesize, int pageindex, int inttype)
        {
            List<PrivateMessageInfo> list = GetPrivateMessageCollection(userId, 0, pagesize, pageindex, inttype);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Message = Utils.GetSubString(list[i].Message, 20, "...");
                }
            }
            return list;
        }

        /// <summary>
        /// �������µ�һ����¼ID
        /// </summary>
        /// <param name="userId">�û�ID</param>
        /// <returns></returns>
        public static int GetLatestPMID(int userId)
        {
            List<PrivateMessageInfo> list = Discuz.Data.PrivateMessages.GetPrivateMessageCollection(userId, 0, 1, 1, 0);
            int latestpmid = 0;

            foreach (PrivateMessageInfo info in list)
            {
                latestpmid = info.Pmid;
                break;
            }
            return latestpmid;
        }

        /// <summary>
        /// ��ȡɾ������Ϣ������
        /// </summary>
        /// <param name="isNew">�Ƿ�ɾ���¶���Ϣ</param>
        /// <param name="postDateTime">��������</param>
        /// <param name="msgFromList">�������б�</param>
        /// <param name="lowerUpper">�Ƿ����ִ�Сд</param>
        /// <param name="subject">����</param>
        /// <param name="message">����</param>
        /// <param name="isUpdateUserNewPm">�Ƿ�����û�����Ϣ��</param>
        /// <returns></returns>
        public static string DeletePrivateMessages(bool isNew, string postDateTime, string msgFromList, bool lowerUpper, string subject, string message, bool isUpdateUserNewPm)
        {
            return Data.PrivateMessages.DeletePrivateMessages(isNew, postDateTime, msgFromList, lowerUpper, subject, message, isUpdateUserNewPm);
        }
    } //class end
}
