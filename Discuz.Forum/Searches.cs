using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin;

namespace Discuz.Forum
{
    /// <summary>
    /// 搜索操作类
    /// </summary>
    public class Searches
    {

        private static Regex regexSpacePost = new Regex(@"<SpacePosts>([\s\S]+?)</SpacePosts>");

        private static Regex regexAlbum = new Regex(@"<Albums>([\s\S]+?)</Albums>");

        private static Regex regexForumTopics = new Regex(@"<ForumTopics>([\s\S]+?)</ForumTopics>");


        /// <summary>
        /// 根据指定条件进行搜索
        /// </summary>
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
        public static int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, SearchType searchType, string searchforumid, int searchtime, int searchtimetype, int resultorder, int resultordertype)
        {
            bool spaceenabled = false, albumenable = false;

            if (posttableid == 0)
                posttableid = TypeConverter.StrToInt(Posts.GetPostTableId(), 1);

            if (GeneralConfigs.GetConfig().Enablespace == 1 && SpacePluginProvider.GetInstance() != null)
                spaceenabled = true;

            if (GeneralConfigs.GetConfig().Enablealbum == 1 && AlbumPluginProvider.GetInstance() != null)
                albumenable = true;

            return Discuz.Data.Searches.Search(spaceenabled, albumenable, posttableid, userid, usergroupid, keyword, posterid, searchType, searchforumid, searchtime, searchtimetype, resultorder, resultordertype);
        }

        /// <summary>
        /// 获指定的搜索缓存的DataTable
        /// </summary>
        /// <param name="posttableid">帖子分表id</param>
        /// <param name="searchid">搜索缓存的searchid</param>
        /// <param name="pagesize">每页的记录数</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="topiccount">主题记录数</param>
        /// <param name="type">搜索类型</param>
        /// <returns>搜索缓存的DataTable</returns>
        public static DataTable GetSearchCacheList(int posttableid, int searchid, int pagesize, int pageindex, out int topiccount, SearchType searchType)
        {
            topiccount = 0;
            DataTable dt = Discuz.Data.Searches.GetSearchCache(searchid);

            if (dt.Rows.Count == 0)
                return new DataTable();

            string cachedidlist = dt.Rows[0][0].ToString();

            Match m;
            switch (searchType)
            {
                case SearchType.SpacePostTitle:
                    #region 搜索空间日志
                    m = regexSpacePost.Match(cachedidlist);

                    if (m.Success)
                    {
                        string tids = GetCurrentPageTids(m.Groups[1].Value, out topiccount, pagesize, pageindex);

                        if (Utils.StrIsNullOrEmpty(tids))
                            return new DataTable();

                        return SpacePluginProvider.GetInstance() == null ? new DataTable() : SpacePluginProvider.GetInstance().GetResult(pagesize, tids);
                    }
                    #endregion
                    break;
                case SearchType.AlbumTitle:
                    #region 搜索相册

                    m = regexAlbum.Match(cachedidlist);

                    if (m.Success)
                    {
                        string tids = GetCurrentPageTids(m.Groups[1].Value, out topiccount, pagesize, pageindex);

                        if (Utils.StrIsNullOrEmpty(tids))
                            return new DataTable();

                        return AlbumPluginProvider.GetInstance() == null ? new DataTable() : AlbumPluginProvider.GetInstance().GetResult(pagesize, tids);
                    }
                    #endregion
                    break;
                default:
                    #region 搜索论坛

                    m = regexForumTopics.Match(cachedidlist);

                    if (m.Success)
                    {
                        string tids = GetCurrentPageTids(m.Groups[1].Value, out topiccount, pagesize, pageindex);

                        if (Utils.StrIsNullOrEmpty(tids))
                            return new DataTable();

                        if (searchType == SearchType.DigestTopic)
                            return Discuz.Data.Searches.GetSearchDigestTopicsList(pagesize, tids);

                        //if (searchType == SearchType.PostContent)
                        //    return Discuz.Data.Searches.GetSearchPostsTopicsList(pagesize, tids, PostTables.GetPostTableName());
                        //else
                            return Discuz.Data.Searches.GetSearchTopicsList(pagesize, tids);
                    }
                    #endregion
                    break;
            }
            return new DataTable();
        }

        /// <summary>
        /// 获得当前页的Tid列表
        /// </summary>
        /// <param name="tids">全部Tid列表</param>
        /// <returns></returns>
        private static string GetCurrentPageTids(string tids, out int topiccount, int pagesize, int pageindex)
        {
            string[] tid = Utils.SplitString(tids, ",");
            topiccount = tid.Length;
            int pagecount = topiccount % pagesize == 0 ? topiccount / pagesize : topiccount / pagesize + 1;

            if (pagecount < 1)
                pagecount = 1;

            if (pageindex > pagecount)
                pageindex = pagecount;

            int startindex = pagesize * (pageindex - 1);
            StringBuilder strTids = new StringBuilder();
            for (int i = startindex; i < topiccount; i++)
            {
                if (i > startindex + pagesize)
                    break;
                else
                {
                    strTids.Append(tid[i]);
                    strTids.Append(",");
                }
            }
            return strTids.Remove(strTids.Length - 1, 1).ToString();
        }
    }
}
