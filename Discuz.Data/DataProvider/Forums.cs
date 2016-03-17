using System;
using System.Text;
using System.Data;

using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Data
{
    public class Forums
    {
        /// <summary>
        /// 获得分类和版块列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumIndexListTable()
        {
            return DatabaseProvider.GetInstance().GetForumIndexListTable();
        }

        /// <summary>
        /// 获得简介版论坛首页列表
        /// </summary>
        /// <returns>板块列表的DataTable</returns>
        public static List<ArchiverForumInfo> GetArchiverForumIndexList()
        {
            DataTable dt = DatabaseProvider.GetInstance().GetArchiverForumIndexList();
            List<ArchiverForumInfo> archiverForumList = new List<ArchiverForumInfo>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ArchiverForumInfo archiverForumInfo = new ArchiverForumInfo();
                    archiverForumInfo.Fid = TypeConverter.ObjectToInt(dr["fid"]);
                    archiverForumInfo.Layer = TypeConverter.ObjectToInt(dr["layer"]);
                    archiverForumInfo.Name = dr["name"].ToString();
                    archiverForumInfo.ParentidList = dr["parentidlist"].ToString();
                    archiverForumInfo.Status = TypeConverter.ObjectToInt(dr["status"]);
                    archiverForumInfo.ViewPerm = dr["viewperm"].ToString();

                    archiverForumList.Add(archiverForumInfo);
                }
            }

            return archiverForumList;
        }

        /// <summary>
        /// 获得版块下的子版块列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>子版块列表</returns>
        public static DataTable GetSubForumTable(int fid)
        {
            return DatabaseProvider.GetInstance().GetSubForumTable(fid);
        }

        /// <summary>
        /// 返回全部版块列表并缓存
        /// </summary>
        /// <returns>板块信息数组</returns>
        public static List<ForumInfo> GetForumList()
        {
            List<ForumInfo> forumlist = new List<ForumInfo>();
            DataTable dt = GetForumListForDataTable();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ForumInfo forum = new ForumInfo();
                    forum.Fid = TypeConverter.StrToInt(dr["fid"].ToString(), 0);
                    forum.Parentid = TypeConverter.StrToInt(dr["parentid"].ToString(), 0);
                    forum.Layer = TypeConverter.StrToInt(dr["layer"].ToString(), 0);
                    forum.Pathlist = dr["pathlist"].ToString();
                    forum.Parentidlist = dr["parentidlist"].ToString();
                    forum.Subforumcount = TypeConverter.StrToInt(dr["subforumcount"].ToString(), 0);
                    forum.Name = dr["name"].ToString();
                    forum.Status = TypeConverter.StrToInt(dr["status"].ToString(), 0);
                    forum.Colcount = TypeConverter.StrToInt(dr["colcount"].ToString(), 0);
                    forum.Displayorder = TypeConverter.StrToInt(dr["displayorder"].ToString(), 0);
                    forum.Templateid = TypeConverter.StrToInt(dr["templateid"].ToString(), 0);
                    forum.Topics = TypeConverter.StrToInt(dr["topics"].ToString(), 0);
                    forum.CurrentTopics = TypeConverter.StrToInt(dr["curtopics"].ToString(), 0);
                    forum.Posts = TypeConverter.StrToInt(dr["posts"].ToString(), 0);
                    //当前版块的最后发帖日期为空，则表示今日发帖数为0 
                    if (Utils.StrIsNullOrEmpty(dr["lastpost"].ToString()))
                        dr["todayposts"] = 0;
                    else
                    {
                        //当系统日期与最发发帖日期不同时，则表示今日发帖数为0 
                        if (Convert.ToDateTime(dr["lastpost"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                            dr["todayposts"] = 0;
                    }
                    forum.Todayposts = TypeConverter.StrToInt(dr["todayposts"].ToString(), 0);
                    forum.Lasttid = TypeConverter.StrToInt(dr["lasttid"].ToString(), 0);
                    forum.Lasttitle = dr["lasttitle"].ToString();
                    forum.Lastpost = dr["lastpost"].ToString();
                    forum.Lastposterid = TypeConverter.StrToInt(dr["lastposterid"].ToString(), 0);
                    forum.Lastposter = dr["lastposter"].ToString();
                    forum.Allowsmilies = TypeConverter.StrToInt(dr["allowsmilies"].ToString(), 0);
                    forum.Allowrss = TypeConverter.StrToInt(dr["allowrss"].ToString(), 0);
                    forum.Allowhtml = TypeConverter.StrToInt(dr["allowhtml"].ToString(), 0);
                    forum.Allowbbcode = TypeConverter.StrToInt(dr["allowbbcode"].ToString(), 0);
                    forum.Allowimgcode = TypeConverter.StrToInt(dr["allowimgcode"].ToString(), 0);
                    forum.Allowblog = TypeConverter.StrToInt(dr["allowblog"].ToString(), 0);
                    forum.Istrade = TypeConverter.StrToInt(dr["istrade"].ToString(), 0);
                    forum.Allowpostspecial = TypeConverter.StrToInt(dr["allowpostspecial"].ToString(), 0);
                    forum.Allowspecialonly = TypeConverter.StrToInt(dr["allowspecialonly"].ToString(), 0);
                    forum.Alloweditrules = TypeConverter.StrToInt(dr["alloweditrules"].ToString(), 0);
                    forum.Allowthumbnail = TypeConverter.StrToInt(dr["allowthumbnail"].ToString(), 0);
                    forum.Allowtag = TypeConverter.StrToInt(dr["allowtag"].ToString(), 0);
                    forum.Recyclebin = TypeConverter.StrToInt(dr["recyclebin"].ToString(), 0);
                    forum.Modnewposts = TypeConverter.StrToInt(dr["modnewposts"].ToString(), 0);
                    forum.Modnewtopics = TypeConverter.StrToInt(dr["modnewtopics"].ToString(), 0);
                    forum.Jammer = TypeConverter.StrToInt(dr["jammer"].ToString(), 0);
                    forum.Disablewatermark = TypeConverter.StrToInt(dr["disablewatermark"].ToString(), 0);
                    forum.Inheritedmod = TypeConverter.StrToInt(dr["inheritedmod"].ToString(), 0);
                    forum.Autoclose = TypeConverter.StrToInt(dr["autoclose"].ToString(), 0);

                    forum.Password = dr["password"].ToString();
                    forum.Icon = dr["icon"].ToString();
                    forum.Postcredits = dr["postcredits"].ToString();
                    forum.Replycredits = dr["replycredits"].ToString();
                    forum.Redirect = dr["redirect"].ToString();
                    forum.Attachextensions = dr["attachextensions"].ToString();
                    forum.Rules = dr["rules"].ToString();
                    forum.Topictypes = dr["topictypes"].ToString();
                    forum.Viewperm = dr["viewperm"].ToString();
                    forum.Postperm = dr["postperm"].ToString();
                    forum.Replyperm = dr["replyperm"].ToString();
                    forum.Getattachperm = dr["getattachperm"].ToString();
                    forum.Postattachperm = dr["postattachperm"].ToString();
                    forum.Moderators = dr["moderators"].ToString();
                    forum.Description = dr["description"].ToString();
                    forum.Applytopictype = TypeConverter.StrToInt(dr["applytopictype"] == DBNull.Value ? "0" : dr["applytopictype"].ToString(), 0);
                    forum.Postbytopictype = TypeConverter.StrToInt(dr["postbytopictype"] == DBNull.Value ? "0" : dr["postbytopictype"].ToString(), 0);
                    forum.Viewbytopictype = TypeConverter.StrToInt(dr["viewbytopictype"] == DBNull.Value ? "0" : dr["viewbytopictype"].ToString(), 0);
                    forum.Topictypeprefix = TypeConverter.StrToInt(dr["topictypeprefix"] == DBNull.Value ? "0" : dr["topictypeprefix"].ToString(), 0);
                    forum.Permuserlist = dr["permuserlist"].ToString();
                    forum.Seokeywords = dr["seokeywords"].ToString();
                    forum.Seodescription = dr["seodescription"].ToString();
                    forum.Rewritename = dr["rewritename"].ToString();
                    forumlist.Add(forum);
                }
            }
            return forumlist;
        }

        /// <summary>
        /// 获取所有版块信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumListForDataTable()
        {
            return DatabaseProvider.GetInstance().GetForumsTable();
        }

        /// <summary>
        /// 设置当前版块主题数(不含子版块)
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>主题数</returns>
        public static int SetRealCurrentTopics(int fid)
        {
            return DatabaseProvider.GetInstance().SetRealCurrentTopics(fid);
        }

        /// <summary>
        /// 更新指定版块的最新发帖数信息
        /// </summary>
        public static void UpdateForumLastPost(ForumInfo forumInfo, PostInfo postInfo)
        {
            DatabaseProvider.GetInstance().UpdateLastPost(forumInfo, postInfo);
        }
        /// <summary>
        /// 更新所有版块的最后发帖人等信息
        /// </summary>
        public static void ResetLastPostInfo()
        {
            DatabaseProvider.GetInstance().ResetLastPostInfo();
        }

        public static List<IndexPageForumInfo> GetForumIndexList()
        {
            List<IndexPageForumInfo> indexPageForumList = new List<IndexPageForumInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetForumIndexList();
            if (reader != null)
            {
                while (reader.Read())
                {
                    IndexPageForumInfo info = new IndexPageForumInfo();
                    //赋值
                    info.Fid = TypeConverter.StrToInt(reader["fid"].ToString());
                    info.Parentid = TypeConverter.StrToInt(reader["parentid"].ToString());
                    info.Layer = TypeConverter.StrToInt(reader["layer"].ToString());
                    info.Name = reader["name"].ToString();
                    info.Pathlist = reader["pathlist"].ToString();
                    info.Parentidlist = reader["parentidlist"].ToString();
                    info.Subforumcount = TypeConverter.StrToInt(reader["subforumcount"].ToString());
                    info.Status = TypeConverter.StrToInt(reader["status"].ToString());
                    info.Colcount = TypeConverter.StrToInt(reader["colcount"].ToString());
                    info.Displayorder = TypeConverter.StrToInt(reader["displayorder"].ToString());
                    info.Templateid = TypeConverter.StrToInt(reader["templateid"].ToString());
                    info.Topics = TypeConverter.StrToInt(reader["topics"].ToString());
                    info.CurrentTopics = TypeConverter.StrToInt(reader["curtopics"].ToString());
                    info.Posts = TypeConverter.StrToInt(reader["posts"].ToString());
                    info.Todayposts = TypeConverter.StrToInt(reader["todayposts"].ToString());
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lasttid = TypeConverter.StrToInt(reader["lasttid"].ToString());
                    info.Lastposterid = TypeConverter.StrToInt(reader["lastposterid"].ToString());
                    info.Lasttitle = reader["lasttitle"].ToString();
                    info.Allowsmilies = TypeConverter.StrToInt(reader["allowsmilies"].ToString());
                    info.Allowrss = TypeConverter.StrToInt(reader["allowrss"].ToString());
                    info.Allowhtml = TypeConverter.StrToInt(reader["allowhtml"].ToString());
                    info.Allowbbcode = TypeConverter.StrToInt(reader["allowbbcode"].ToString());
                    info.Allowimgcode = TypeConverter.StrToInt(reader["allowimgcode"].ToString());
                    info.Allowblog = TypeConverter.StrToInt(reader["allowblog"].ToString());
                    info.Istrade = TypeConverter.StrToInt(reader["istrade"].ToString());
                    info.Allowpostspecial = TypeConverter.StrToInt(reader["allowpostspecial"].ToString());
                    info.Allowspecialonly = TypeConverter.StrToInt(reader["allowspecialonly"].ToString());
                    info.Alloweditrules = TypeConverter.StrToInt(reader["alloweditrules"].ToString());
                    info.Allowthumbnail = TypeConverter.StrToInt(reader["allowthumbnail"].ToString());
                    info.Recyclebin = TypeConverter.StrToInt(reader["recyclebin"].ToString());
                    info.Modnewposts = TypeConverter.StrToInt(reader["modnewposts"].ToString());
                    info.Jammer = TypeConverter.StrToInt(reader["jammer"].ToString());
                    info.Disablewatermark = TypeConverter.StrToInt(reader["disablewatermark"].ToString());
                    info.Inheritedmod = TypeConverter.StrToInt(reader["inheritedmod"].ToString());
                    info.Autoclose = TypeConverter.StrToInt(reader["autoclose"].ToString());

                    info.Description = reader["description"].ToString();
                    info.Password = reader["password"].ToString();
                    info.Icon = reader["icon"].ToString();
                    info.Postcredits = reader["postcredits"].ToString();
                    info.Replycredits = reader["replycredits"].ToString();
                    info.Redirect = reader["redirect"].ToString();
                    info.Attachextensions = reader["attachextensions"].ToString();
                    info.Moderators = reader["moderators"].ToString();
                    info.Rules = reader["rules"].ToString();
                    info.Topictypes = reader["topictypes"].ToString();
                    info.Viewperm = reader["viewperm"].ToString();
                    info.Postperm = reader["postperm"].ToString();
                    info.Replyperm = reader["replyperm"].ToString();
                    info.Getattachperm = reader["getattachperm"].ToString();
                    info.Postattachperm = reader["postattachperm"].ToString();
                    info.Applytopictype = TypeConverter.StrToInt(reader["applytopictype"] == DBNull.Value ? "0" : reader["applytopictype"].ToString());
                    info.Postbytopictype = TypeConverter.StrToInt(reader["postbytopictype"] == DBNull.Value ? "0" : reader["postbytopictype"].ToString());
                    info.Viewbytopictype = TypeConverter.StrToInt(reader["viewbytopictype"] == DBNull.Value ? "0" : reader["viewbytopictype"].ToString());
                    info.Topictypeprefix = TypeConverter.StrToInt(reader["topictypeprefix"] == DBNull.Value ? "0" : reader["topictypeprefix"].ToString());
                    info.Permuserlist = reader["permuserlist"].ToString();
                    info.Seokeywords = reader["seokeywords"].ToString();
                    info.Seodescription = reader["seodescription"].ToString();
                    info.Rewritename = reader["rewritename"].ToString();

                    //扩展属性
                    info.Havenew = reader["havenew"].ToString();
                    indexPageForumList.Add(info);
                }
                reader.Close();
            }
            return indexPageForumList;
        }

        public static List<IndexPageForumInfo> GetSubForumList(int fid, int colcount)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetSubForumReader(fid);
            List<IndexPageForumInfo> indexPageForumList = new List<IndexPageForumInfo>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    IndexPageForumInfo info = new IndexPageForumInfo();
                    //赋值
                    info.Fid = TypeConverter.StrToInt(reader["fid"].ToString(), 0);
                    info.Parentid = TypeConverter.StrToInt(reader["parentid"].ToString(), 0);
                    info.Layer = TypeConverter.StrToInt(reader["layer"].ToString(), 0);
                    info.Name = reader["name"].ToString();
                    info.Pathlist = reader["pathlist"].ToString();
                    info.Parentidlist = reader["parentidlist"].ToString();
                    info.Subforumcount = TypeConverter.StrToInt(reader["subforumcount"].ToString(), 0);
                    info.Status = TypeConverter.StrToInt(reader["status"].ToString(), 0);
                    info.Colcount = colcount > 0 ? colcount : TypeConverter.StrToInt(reader["colcount"].ToString(), 0);
                    info.Displayorder = TypeConverter.StrToInt(reader["displayorder"].ToString(), 0);
                    info.Templateid = TypeConverter.StrToInt(reader["templateid"].ToString(), 0);
                    info.Topics = TypeConverter.StrToInt(reader["topics"].ToString(), 0);
                    info.CurrentTopics = TypeConverter.StrToInt(reader["curtopics"].ToString(), 0);
                    info.Posts = TypeConverter.StrToInt(reader["posts"].ToString(), 0);
                    info.Todayposts = TypeConverter.StrToInt(reader["todayposts"].ToString(), 0);
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lasttid = TypeConverter.StrToInt(reader["lasttid"].ToString(), 0);
                    info.Lastposterid = TypeConverter.StrToInt(reader["lastposterid"].ToString(), 0);
                    info.Lasttitle = reader["lasttitle"].ToString();
                    info.Allowsmilies = TypeConverter.StrToInt(reader["allowsmilies"].ToString(), 0);
                    info.Allowrss = TypeConverter.StrToInt(reader["allowrss"].ToString(), 0);
                    info.Allowhtml = TypeConverter.StrToInt(reader["allowhtml"].ToString(), 0);
                    info.Allowbbcode = TypeConverter.StrToInt(reader["allowbbcode"].ToString(), 0);
                    info.Allowimgcode = TypeConverter.StrToInt(reader["allowimgcode"].ToString(), 0);
                    info.Allowblog = TypeConverter.StrToInt(reader["allowblog"].ToString(), 0);
                    info.Istrade = TypeConverter.StrToInt(reader["istrade"].ToString(), 0);
                    info.Allowpostspecial = TypeConverter.StrToInt(reader["allowpostspecial"].ToString(), 0);
                    info.Allowspecialonly = TypeConverter.StrToInt(reader["allowspecialonly"].ToString(), 0);
                    info.Alloweditrules = TypeConverter.StrToInt(reader["alloweditrules"].ToString(), 0);
                    info.Allowthumbnail = TypeConverter.StrToInt(reader["allowthumbnail"].ToString(), 0);
                    info.Recyclebin = TypeConverter.StrToInt(reader["recyclebin"].ToString(), 0);
                    info.Modnewposts = TypeConverter.StrToInt(reader["modnewposts"].ToString(), 0);
                    info.Jammer = TypeConverter.StrToInt(reader["jammer"].ToString(), 0);
                    info.Disablewatermark = TypeConverter.StrToInt(reader["disablewatermark"].ToString(), 0);
                    info.Inheritedmod = TypeConverter.StrToInt(reader["inheritedmod"].ToString(), 0);
                    info.Autoclose = TypeConverter.StrToInt(reader["autoclose"].ToString(), 0);

                    info.Description = reader["description"].ToString();
                    info.Password = reader["password"].ToString();
                    info.Icon = reader["icon"].ToString();
                    if (!Utils.StrIsNullOrEmpty(info.Icon))
                        info.Icon = info.Icon.ToLower().IndexOf("http://") == 0 ? info.Icon : BaseConfigs.GetForumPath + info.Icon;

                    info.Postcredits = reader["postcredits"].ToString();
                    info.Replycredits = reader["replycredits"].ToString();
                    info.Redirect = reader["redirect"].ToString();
                    info.Attachextensions = reader["attachextensions"].ToString();
                    info.Moderators = reader["moderators"].ToString();
                    info.Rules = reader["rules"].ToString();
                    info.Topictypes = reader["topictypes"].ToString();
                    info.Viewperm = reader["viewperm"].ToString();
                    info.Postperm = reader["postperm"].ToString();
                    info.Replyperm = reader["replyperm"].ToString();
                    info.Getattachperm = reader["getattachperm"].ToString();
                    info.Postattachperm = reader["postattachperm"].ToString();
                    info.Applytopictype = TypeConverter.StrToInt(reader["applytopictype"].ToString(), 0);
                    info.Postbytopictype = TypeConverter.StrToInt(reader["postbytopictype"].ToString(), 0);
                    info.Viewbytopictype = TypeConverter.StrToInt(reader["viewbytopictype"].ToString(), 0);
                    info.Topictypeprefix = TypeConverter.StrToInt(reader["topictypeprefix"].ToString(), 0);
                    info.Permuserlist = reader["permuserlist"].ToString();
                    info.Seokeywords = reader["seokeywords"].ToString();
                    info.Seodescription = reader["seodescription"].ToString();
                    info.Rewritename = reader["rewritename"].ToString();

                    //扩展属性
                    info.Havenew = reader["havenew"].ToString();

                    indexPageForumList.Add(info);
                }
                reader.Close();
            }
            return indexPageForumList;
        }

        /// <summary>
        /// 检查rewritename是否存在或非法
        /// </summary>
        /// <param name="rewriteName"></param>
        /// <returns>如果存在或者非法的Rewritename则返回true,否则为false</returns>
        public static bool CheckRewriteNameInvalid(string rewriteName)
        {
            return DatabaseProvider.GetInstance().CheckForumRewriteNameExists(rewriteName);
        }
        /// <summary>
        /// 获得所有版块列表的option字符串
        /// </summary>
        /// <returns></returns>
        public static DataTable GetShortForumList()
        {
            return DatabaseProvider.GetInstance().GetShortForumList();
        }

        /// <summary>
        /// 获取开放版块列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOpenForumList()
        {
            return DatabaseProvider.GetInstance().GetOpenForumList();
        }

        /// <summary>
        /// 更新版块和用户模板Id
        /// </summary>
        /// <param name="templateIdList">模板Id列表</param>
        public static void UpdateForumAndUserTemplateId(string templateIdList)
        {
            DatabaseProvider.GetInstance().UpdateForumAndUserTemplateId(templateIdList);
        }

        /// <summary>
        /// 更新版块显示顺序，将大于当前显示顺序的版块显示顺序加1
        /// </summary>
        /// <param name="minDisplayOrder"></param>
        public static void UpdateFourmsDisplayOrder(int minDisplayOrder)
        {
            DatabaseProvider.GetInstance().UpdateForumsDisplayOrder(minDisplayOrder);
        }

        /// <summary>
        /// 更新版块信息
        /// </summary>
        /// <param name="forumInfo">版块信息</param>
        public static void UpdateForumInfo(ForumInfo forumInfo)
        {
            DatabaseProvider.GetInstance().SaveForumsInfo(forumInfo);
        }

        /// <summary>
        /// 创建版块
        /// </summary>
        /// <param name="forumInfo">版块信息</param>
        /// <returns>新建版块Fid</returns>
        public static int CreateForumInfo(ForumInfo forumInfo)
        {
            return DatabaseProvider.GetInstance().InsertForumsInf(forumInfo);
        }

        /// <summary>
        /// 更新板块的字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldname">字段</param>
        /// <returns></returns>
        public static int UpdateForumField(int fid, string fieldname, string fieldvalue)
        {
            return DatabaseProvider.GetInstance().UpdateForumField(fid, fieldname, fieldvalue);
        }
        /// <summary>
        /// 更新版块版主的名字
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="oldName">旧版主名字</param>
        /// <param name="newName">新版主名字，为空则删除该版主</param>
        public static void UpdateModeratorName(string oldName, string newName)
        {
            DatabaseProvider.GetInstance().UpdateModeratorName(oldName, newName);
        }
        /// <summary>
        /// 获取版块名称和主题分类
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumNameIncludeTopicType()
        {
            return DatabaseProvider.GetInstance().GetForumNameIncludeTopicType();
        }

        /// <summary>
        /// 获取主题分类不为空的板块
        /// </summary>
        /// <returns></returns>
        public static DataTable GetExistTopicTypeOfForum()
        {
            return DatabaseProvider.GetInstance().GetExistTopicTypeOfForum();
        }

        /// <summary>
        /// 删除版块
        /// </summary>
        /// <param name="postName">分表名称</param>
        /// <param name="fid">版块Id</param>
        public static void DeleteForum(string postName, string fid)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.DeleteForumTopic(TypeConverter.StrToInt(fid));

            DatabaseProvider.GetInstance().DeleteForumsByFid(postName, fid);
        }

        /// <summary>
        /// 清除主题里面已经移走的主题
        /// </summary>
        public static void ReSetClearMove()
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.ReSetClearMove();

            DatabaseProvider.GetInstance().ReSetClearMove();
        }

        /// <summary>
        /// 更新版块信息
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="topiccount">主题数</param>
        /// <param name="postcount">帖子数</param>
        /// <param name="lasttid">最后回复主题</param>
        /// <param name="lasttitle">最后回复标题</param>
        /// <param name="lastpost">最后回复时间</param>
        /// <param name="lastposterid">最后回复人ID</param>
        /// <param name="lastposter">最后回复人</param>
        /// <param name="todaypostcount">当天发的帖子数</param>
        public static void UpdateForum(int fid, int topiccount, int postcount, int lasttid, string lasttitle, string lastpost,
                         int lastposterid, string lastposter, int todaypostcount)
        {
            DatabaseProvider.GetInstance().UpdateForum(fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        }

        /// <summary>
        /// 获取版主列表中包含用户名的版块列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        //public static DataTable GetModerators(string userName)
        //{
        //    return DatabaseProvider.GetInstance().GetModerators(userName);
        //}

        /// <summary>
        /// 获取包含该特殊用户的版块
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static DataTable GetForumTableBySpecialUser(string userName)
        {
            return DatabaseProvider.GetInstance().GetForumTableBySpecialUser(userName);
        }

        /// <summary>
        /// 获取有特殊用户的版块列表
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <returns></returns>
        public static DataTable GetForumTableWithSpecialUser(int fid)
        {
            return DatabaseProvider.GetInstance().GetForumTableWithSpecialUser(fid);
        }

        /// <summary>
        /// 获取版块最大最小主题Id
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static IDataReader GetMaxAndMinTid(int fid)
        {
            return DatabaseProvider.GetInstance().GetMaxAndMinTid(fid);
        }

        /// <summary>
        /// 要设置的起始版块
        /// </summary>
        /// <param name="start_fid">起始板块ID</param>
        /// <param name="end_fid">终止板块ID</param>
        /// <returns></returns>
        public static IDataReader GetForums(int start_fid, int end_fid)
        {
            return DatabaseProvider.GetInstance().GetForums(start_fid, end_fid);
        }
        /// <summary>
        ///  重置整个论坛所有版块的帖子数（topics and posts）
        /// </summary>
        public static void ResetForumsPosts()
        {
            DatabaseProvider.GetInstance().ResetForumsPosts();
        }

        /// <summary>
        /// 批理设置论坛信息
        /// </summary>
        /// <param name="__foruminfo">复制的论坛信息</param>
        /// <param name="bsp">是否要批量设置的信息字段</param>
        /// <param name="fidlist">目标论坛(fid)串</param>
        /// <returns></returns>
        public static bool BatchSetForumInf(ForumInfo __foruminfo, BatchSetParams bsp, string fidlist)
        {
            return DatabaseProvider.GetInstance().BatchSetForumInf(__foruminfo, bsp, fidlist);
        }

        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static DataTable GetMaxDisplayOrder(int fid)
        {
            return DatabaseProvider.GetInstance().GetForumsMaxDisplayOrder(fid);
        }

        /// <summary>
        /// 获得forum的最大排序号
        /// </summary>
        /// <returns></returns>
        public static int GetMaxDisplayOrder()
        {
            return DatabaseProvider.GetInstance().GetForumsMaxDisplayOrder();
        }

        /// <summary>
        /// 更新论坛排序号
        /// </summary>
        /// <param name="currentdisplayorder"></param>
        public static void UpdateForumsDisplayOrder(int currentdisplayorder)
        {
            DatabaseProvider.GetInstance().UpdateForumsDisplayOrder(currentdisplayorder);
        }

        /// <summary>
        /// 设置子版块数量
        /// </summary>
        /// <param name="fid"></param>
        public static void SetSubForumCount(int fid)
        {
            DatabaseProvider.GetInstance().UpdateSubForumCount(fid);
        }

        /// <summary>
        /// 设置版块状态
        /// </summary>
        /// <param name="status">板块状态</param>
        /// <param name="fid">板块ID</param>
        public static void SetStatusInForum(int status, int fid)
        {
            DatabaseProvider.GetInstance().SetStatusInForum(status, fid);
        }

        /// <summary>
        /// 获取顶级板块Id列表
        /// </summary>
        /// <param name="lastfid"></param>
        /// <param name="statcount"></param>
        /// <returns></returns>
        public static IDataReader GetTopForumFids(int lastfid, int statcount)
        {
            return DatabaseProvider.GetInstance().GetTopForumFids(lastfid, statcount);
        }

        /// <summary>
        /// 移动版块位置
        /// </summary>
        /// <param name="currentfid">当前板块ID</param>
        /// <param name="targetfid"></param>
        /// <param name="isaschildnode"></param>
        /// <param name="extname"></param>
        public static void MovingForumsPos(string currentfid, string targetfid, bool isaschildnode, string extname)
        {
            DatabaseProvider.GetInstance().MovingForumsPos(currentfid, targetfid, isaschildnode, extname);
        }

        /// <summary>
        /// 获取顶级板块列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMainForum()
        {
            return DatabaseProvider.GetInstance().GetMainForum();
        }

        /// <summary>
        /// 批量更新板块状态
        /// </summary>
        /// <param name="fidlist">板块ID列表</param>
        public static void UpdateStatusByFidlist(string fidlist)
        {
            DatabaseProvider.GetInstance().UpdateStatusByFidlist(fidlist);
        }

        /// <summary>
        /// 批量更新板块状态
        /// </summary>
        /// <param name="fidlist">板块ID列表</param>
        public static void UpdateStatusByFidlistOther(string fidlist)
        {
            DatabaseProvider.GetInstance().UpdateStatusByFidlistOther(fidlist);
        }

        /// <summary>
        /// 按父板块获取子板块
        /// </summary>
        /// <param name="parentid">父ID</param>
        /// <returns></returns>
        public static DataTable GetForumByParentid(int parentid)
        {
            return DatabaseProvider.GetInstance().GetForumByParentid(parentid);
        }

        /// <summary>
        /// 更新子版块数量
        /// </summary>
        /// <param name="subforumcount">子板块数</param>
        /// <param name="fid">板块ID</param>
        public static void UpdateSubForumCount(int subforumcount, int fid)
        {
            DatabaseProvider.GetInstance().UpdateSubForumCount(subforumcount, fid);
        }

        /// <summary>
        /// 合并版块
        /// </summary>
        /// <param name="sourcefid"></param>
        /// <param name="targetfid"></param>
        /// <param name="fidlist"></param>
        public static void CombinationForums(string sourcefid, string targetfid, string fidlist)
        {
            DatabaseProvider.GetInstance().CombinationForums(sourcefid, targetfid, fidlist);
        }

        /// <summary>
        /// 是否存在子版块
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static bool IsExistSubForum(int fid)
        {
            return DatabaseProvider.GetInstance().IsExistSubForum(fid);
        }

        /// <summary>
        /// 更新版本子版数
        /// </summary>
        /// <param name="fid">版块Id</param>
        public static void UpdateSubForumCount(int fid)
        {
            DatabaseProvider.GetInstance().UpdateSubForumCount(fid);
        }


        /// <summary>
        /// 获取麽一个字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>
        public static DataTable GetForumField(int fid, string fieldname)
        {
            return DatabaseProvider.GetInstance().GetForumField(fid, fieldname);
        }

        /// <summary>
        /// 获取父ID
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        public static DataTable GetParentIdByFid(int fid)
        {
            return DatabaseProvider.GetInstance().GetParentIdByFid(fid);
        }

        /// <summary>
        /// 获取所有可见版块
        /// </summary>
        /// <returns></returns>
        public static DataTable GetVisibleForumList()
        {
            return DatabaseProvider.GetInstance().GetVisibleForumList();
        }

        /// <summary>
        /// 更新所有版块的主题数
        /// </summary>
        public static void ResetForumsTopics()
        {
            DatabaseProvider.GetInstance().ResetForumsTopics();
        }

        /// <summary>
        /// 更新所有版块的今日发帖数
        /// </summary>
        public static void ResetTodayPosts()
        {
            DatabaseProvider.GetInstance().ResetTodayPosts();
        }

        /// <summary>
        /// 建立更新用户积分存储过程的方法
        /// </summary>
        /// <param name="creditExpression">总积分计算公式</param>
        /// <param name="testCreditExpression">是否需要测试总积分计算公式是否正确</param>
        /// <returns></returns>
        public static bool CreateUpdateUserCreditsProcedure(string creditExpression, bool testCreditExpression)
        {
            return DatabaseProvider.GetInstance().CreateUpdateUserCreditsProcedure(creditExpression, testCreditExpression);
        }

        /// <summary>
        /// 获取第一个版块的信息
        /// </summary>
        /// <returns></returns>
        public static int GetFirstFourmID()
        {
            return DatabaseProvider.GetInstance().GetFirstFourmID();
        }

        /// <summary>
        /// 获取指定版块id下的最后发帖主题id
        /// </summary>
        /// <param name="fidList">版块id列表</param>
        /// <returns></returns>
        public static int GetForumsLastPostTid(string fidList)
        {
            return DatabaseProvider.GetInstance().GetForumsLastPostTid(fidList);
        }

        /// <summary>
        /// 更新指定版块或分类的displayorder信息
        /// </summary>
        /// <param name="displayorder">要更新的displayorder信息</param>
        /// <param name="fid">版块id</param>
        public static void UpdateDisplayorderInForumByFid(int displayorder, int fid)
        {
            DatabaseProvider.GetInstance().UpdateDisplayorderInForumByFid(displayorder, fid);
        }

        /// <summary>
        /// 批理设置版块模板信息
        /// </summary>
        /// <param name="templateID">新的模板id</param>
        /// <param name="fidlist">要更新的版块id列表</param>
        /// <returns></returns>
        public static int UpdateForumTemplateID(int templateID, string fidlist)
        {
            return DatabaseProvider.GetInstance().UpdateForumTemplateID(templateID, fidlist);           
        }
    }
}
