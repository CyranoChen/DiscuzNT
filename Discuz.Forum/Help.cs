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
        /// 帮助信息树形列表
        /// </summary>
        private static List<HelpInfo> helpListTree = null;       

        /// <summary>
        /// 获取帮助列表
        /// </summary>
        /// <returns>帮助列表</returns>
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
        /// 递归加载帮助信息树形列表
        /// </summary>
        /// <param name="helpList">源帮助信息列表</param>
        /// <param name="id">当前要递归的父节点helpid信息()</param>
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
        /// 获取帮助内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns>帮助内容</returns>
        public static HelpInfo GetMessage(int id)
        {
            return id > 0 ? Discuz.Data.Help.GetMessage(id) : null;
        }


        /// <summary>
        /// 更新帮助信息
        /// </summary>
        /// <param name="id">帮助ID</param>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        /// <param name="orderby">排序方式</param>
        public static void UpdateHelp(int id, string title, string message, int pid, int orderby)
        {
            if(id > 0)
                Discuz.Data.Help.UpdateHelp(id, title, message, pid, orderby);

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// 增加帮助
        /// </summary>
        /// <param name="title">帮助标题</param>
        /// <param name="message">帮助内容</param>
        /// <param name="pid">帮助</param>
        public static void AddHelp(string title, string message, int pid)
        {
            Discuz.Data.Help.AddHelp(title, message, pid);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="idlist">帮助ID序列</param>
        public static void DelHelp(string idlist)
        {
            Discuz.Data.Help.DelHelp(idlist);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
        }

        /// <summary>
        /// 返回帮助的分类列表的SQL语句
        /// </summary>
        /// <returns>帮助的分类列表的SQL语句</returns>
        public static DataTable GetHelpTypes()
        {
            return Discuz.Data.Help.GetHelpTypes();
        }


        /// <summary>
        /// 获取帮助分类以及相应帮助主题
        /// </summary>
        /// <param name="helpid"></param>
        /// <returns>帮助分类以及相应帮助主题</returns>
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
        /// 更新帮助序号
        /// </summary>
        /// <param name="orderlist">排序号</param>
        /// <param name="idlist">帮助Id</param>
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
