using System;
using System.Text;
using System.Data;
using Discuz.Entity;

namespace Discuz.Data
{
    public class Searches
    {
        /// <summary>
        /// 根据指定条件进行搜索
        /// </summary>
        /// <param name="spaceenabled">空间是否开启</param>
        /// <param name="albumenable">相册是否开启</param>
        /// <param name="posttableid">帖子表id</param>
        /// <param name="userid">用户id</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="keyword">关键字</param>
        /// <param name="posterid">发帖者id</param>
        /// <param name="searchType">搜索类型</param>
        /// <param name="searchforumid">搜索版块id</param>
        /// <param name="searchtime">搜索时间</param>
        /// <param name="searchtimetype">搜索时间类型</param>
        /// <param name="resultorder">结果排序方式</param>
        /// <param name="resultordertype">结果类型类型</param>
        /// <returns>如果成功则返回searchid, 否则返回-1</returns>
        public static int Search(bool spaceenabled, bool albumenable, int posttableid, int userid, int usergroupid, string keyword, int posterid, SearchType searchType, string searchforumid, int searchtime, int searchtimetype, int resultorder, int resultordertype)
        {
            return DatabaseProvider.GetInstance().Search(spaceenabled, albumenable, posttableid, userid, usergroupid, keyword, posterid, searchType, searchforumid, searchtime, searchtimetype, resultorder, resultordertype);
        }

        /// <summary>
        /// 获指定的搜索缓存的DataTable
        /// </summary>
        /// <param name="searchid">搜索缓存的searchid</param>
        /// <returns></returns>
        public static DataTable GetSearchCache(int searchid)
        {
            return DatabaseProvider.GetInstance().GetSearchCache(searchid);
        }

        /// <summary>
        /// 获取精华主题列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="tids">全部Tid列表</param>
        /// <returns></returns>
        public static DataTable GetSearchDigestTopicsList(int pagesize, string tids)
        {
            return DatabaseProvider.GetInstance().GetSearchDigestTopicsList(pagesize, tids);
        }


        /// <summary>
        /// 获得按帖子搜索的主题列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="tids">全部Tid列表</param>
        /// <param name="tableName">当前分表名称</param>
        /// <returns></returns>
        public static DataTable GetSearchPostsTopicsList(int pagesize, string tids, string tableName)
        {
            return DatabaseProvider.GetInstance().GetSearchPostsTopicsList(pagesize, tids, tableName);
        }


        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="tids">全部Tid列表</param>
        /// <returns></returns>
        public static DataTable GetSearchTopicsList(int pagesize, string tids)
        {
            return DatabaseProvider.GetInstance().GetSearchTopicsList(pagesize, tids);
        }

        /// <summary>
        /// 开启全文索引
        /// </summary>
        public static void ConfirmFullTextEnable()
        {
            DatabaseProvider.GetInstance().ConfirmFullTextEnable();
        }

    }
}
