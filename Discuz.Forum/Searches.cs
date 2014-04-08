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
    /// ����������
    /// </summary>
    public class Searches
    {

        private static Regex regexSpacePost = new Regex(@"<SpacePosts>([\s\S]+?)</SpacePosts>");

        private static Regex regexAlbum = new Regex(@"<Albums>([\s\S]+?)</Albums>");

        private static Regex regexForumTopics = new Regex(@"<ForumTopics>([\s\S]+?)</ForumTopics>");


        /// <summary>
        /// ����ָ��������������
        /// </summary>
        /// <param name="posttableid">���ӱ�id</param>
        /// <param name="userid">�û�id</param>
        /// <param name="usergroupid">�û���id</param>
        /// <param name="keyword">�ؼ���</param>
        /// <param name="posterid">������id</param>
        /// <param name="searchType">��������</param>
        /// <param name="searchforumid">�������id</param>
        /// <param name="searchtime">����ʱ��</param>
        /// <param name="searchtimetype">����ʱ������</param>
        /// <param name="resultorder">�������ʽ</param>
        /// <param name="resultordertype">�����������</param>
        /// <returns>����ɹ��򷵻�searchid, ���򷵻�-1</returns>
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
        /// ��ָ�������������DataTable
        /// </summary>
        /// <param name="posttableid">���ӷֱ�id</param>
        /// <param name="searchid">���������searchid</param>
        /// <param name="pagesize">ÿҳ�ļ�¼��</param>
        /// <param name="pageindex">��ǰҳ��</param>
        /// <param name="topiccount">�����¼��</param>
        /// <param name="type">��������</param>
        /// <returns>���������DataTable</returns>
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
                    #region �����ռ���־
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
                    #region �������

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
                    #region ������̳

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
        /// ��õ�ǰҳ��Tid�б�
        /// </summary>
        /// <param name="tids">ȫ��Tid�б�</param>
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
