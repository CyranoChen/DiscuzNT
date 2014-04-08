using System;
using System.Text;
using System.Data.Common;
using System.Data;

using Discuz.Entity;
using Discuz.Data;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Helps
    {
        /// <summary>
        /// ������Ϣ�����б�
        /// </summary>
        private static List<HelpInfo> helpListTree = null;       

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <returns>�����б�</returns>
        public static List<HelpInfo> GetHelpList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            helpListTree = cache.RetrieveObject("/Forum/helplist") as List<HelpInfo>;

            if (helpListTree == null)
            {
                helpListTree = new List<HelpInfo>();
                List<HelpInfo> helpList = Discuz.Data.Help.GetHelpList();

                CreateHelpTree(helpList, 0);
                cache.AddObject("/Forum/helplist", helpListTree);
            }
            return helpListTree;
        }

        /// <summary>
        /// �ݹ���ذ�����Ϣ�����б�
        /// </summary>
        /// <param name="helpList">Դ������Ϣ�б�</param>
        /// <param name="id">��ǰҪ�ݹ�ĸ��ڵ�helpid��Ϣ()</param>
        private static void CreateHelpTree(List<HelpInfo> helpList, int id)
        {
            foreach (HelpInfo helpInfo in helpList)
            {
                if (helpInfo.Pid == id)
                {
                    helpListTree.Add(helpInfo);
                    CreateHelpTree(helpList, helpInfo.Id);
                }                
            }
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns>��������</returns>
        public static HelpInfo GetMessage(int id)
        {
            return id > 0 ? Discuz.Data.Help.GetMessage(id) : null;
        }


        /// <summary>
        /// ���°�����Ϣ
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="title">��������</param>
        /// <param name="message">��������</param>
        /// <param name="pid">����</param>
        /// <param name="orderby">����ʽ</param>
        public static void UpdateHelp(int id, string title, string message, int pid, int orderby)
        {
            if(id > 0)
                Discuz.Data.Help.UpdateHelp(id, title, message, pid, orderby);

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// ���Ӱ���
        /// </summary>
        /// <param name="title">��������</param>
        /// <param name="message">��������</param>
        /// <param name="pid">����</param>
        public static void AddHelp(string title, string message, int pid)
        {
            Discuz.Data.Help.AddHelp(title, message, pid);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="idlist">����ID����</param>
        public static void DelHelp(string idlist)
        {
            Discuz.Data.Help.DelHelp(idlist);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// ���ذ����ķ����б��SQL���
        /// </summary>
        /// <returns>�����ķ����б��SQL���</returns>
        public static DataTable GetHelpTypes()
        {
            return Discuz.Data.Help.GetHelpTypes();
        }


        /// <summary>
        /// ��ȡ���������Լ���Ӧ��������
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns>���������Լ���Ӧ��������</returns>
        public static List<HelpInfo> GetHelpList(int helpid)
        {
            List<HelpInfo> result = new List<HelpInfo>();
            foreach (HelpInfo helpInfo in GetHelpList())
            {
                if (helpInfo.Id == helpid || helpInfo.Pid == helpid)
                    result.Add(helpInfo);
            }
            return result;
        }

        /// <summary>
        /// ���°������
        /// </summary>
        /// <param name="orderlist">�����</param>
        /// <param name="idlist">����Id</param>
        public static bool UpOrder(string[] orderlist, string[] idlist)
        {
            if (orderlist.Length != idlist.Length)
                return false;

            foreach (string s in orderlist)
            {
                if (Discuz.Common.Utils.IsNumeric(s) == false)
                    return false;
            }
            for (int i = 0; i < idlist.Length; i++)
            {
                Discuz.Data.Help.UpdateOrder(orderlist[i].ToString(), idlist[i].ToString());
            }
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
            return true;
        }
    }
}
