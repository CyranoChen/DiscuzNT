using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;
using Discuz.Cache;

namespace Discuz.Forum
{
    /// <summary>
    /// 版块操作类
    /// </summary>
    public class Forums
    {
        /// <summary>
        /// 获得简介版论坛首页列表
        /// </summary>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <returns>板块列表的DataTable</returns>
        public static List<ArchiverForumInfo> GetArchiverForumIndexList(int hideprivate, int usergroupid)
        {
            List<ArchiverForumInfo> archiverForumList = Discuz.Data.Forums.GetArchiverForumIndexList();
            List<ForumInfo> forums = GetForumList();

            List<ArchiverForumInfo> parentForumList = new List<ArchiverForumInfo>();
            List<ArchiverForumInfo> subForumList = new List<ArchiverForumInfo>();
            List<ArchiverForumInfo> resultForumList = new List<ArchiverForumInfo>();

            foreach (ArchiverForumInfo info in archiverForumList)
            {
                //string parentidlist = dr["parentidlist"].ToString().Trim();
                if (info.Status == 0)
                    continue;

                foreach (ForumInfo f in forums)  //删除不可见的子板块
                    if (Utils.InArray(f.Fid.ToString(), info.ParentidList) && f.Status == 0)
                        continue;

                if (info.Layer == 0)
                {
                    if (hideprivate == 0 || info.ViewPerm == "" || Utils.InArray(usergroupid.ToString(), info.ViewPerm))
                        parentForumList.Add(info);
                }
                else
                {
                    if (hideprivate == 0 || info.ViewPerm == "" || Utils.InArray(usergroupid.ToString(), info.ViewPerm))
                        subForumList.Add(info);
                }
                //删除不可见的板块
                //if (info.Status == 0)
                //    dr.Delete();
                //else
                //{
                //    foreach (ForumInfo f in forums)
                //        //删除不可见的子板块
                //        if (Utils.InArray(f.Fid.ToString(), parentidlist) && f.Status == 0)
                //            dr.Delete();
                //}
            }

            foreach (ArchiverForumInfo info in parentForumList)
            {
                resultForumList.Add(info);
                foreach (ArchiverForumInfo subInfo in subForumList)
                {
                    if (Utils.InArray(info.Fid.ToString().Trim(), subInfo.ParentidList))
                        resultForumList.Add(subInfo);
                }
            }


            return resultForumList;

            //if (hideprivate == 1)
            //{
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        //如果当前用户组没有权限
            //        if (dr["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), dr["viewperm"].ToString()))
            //            dr.Delete();
            //    }
            //    dt.AcceptChanges();
            //}
            //return dt;
        }

        /// <summary>
        /// 获得版块下的子版块列表
        /// </summary>
        /// <param name="fid">版块id(当为0时则获取'版块分类'信息)</param>
        /// <returns>子版块列表</returns>
        public static DataTable GetForumList(int fid)
        {
            return fid >= 0 ? GetSubForumListTable(fid) : new DataTable();
        }

        public static DataTable GetSubForumListTable(int fid)
        {
            DataTable dt = Discuz.Data.Forums.GetSubForumTable(fid);

            if (dt != null)
            {
                int status = 0; //是否显示
                int colcount = 1; //设置该论坛的子论坛在列表时分几列显示                

                foreach (DataRow dr in dt.Rows)
                {
                    //如果板块可见
                    if (TypeConverter.ObjectToInt(dr["status"]) > 0)
                    {
                        if (colcount > 1)
                        {
                            dr["status"] = ++status;
                            dr["colcount"] = colcount;
                        }
                        //如果有子板块且按列显示
                        else if (TypeConverter.ObjectToInt(dr["subforumcount"]) > 0 && TypeConverter.ObjectToInt(dr["colcount"]) > 0)
                        {
                            colcount = Utils.StrToInt(dr["colcount"].ToString(), 0);
                            status = colcount;
                            dr["status"] = status + 1;
                        }
                    }
                }
            }
            return dt;
        }

        public static List<ForumInfo> GetSubForumList(int fid)
        {
            List<ForumInfo> forumlist = new List<ForumInfo>();

            foreach (ForumInfo info in Data.Forums.GetForumList())
            {
                if (info.Parentid == fid && info.Status == 1)
                    forumlist.Add(info);
            }
            return forumlist;
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权浏览该版块
        /// </summary>
        /// <param name="viewperm">查看权限的用户组id列表</param>
        /// <param name="usergroupid">用户组id</param>
        /// <returns></returns>
        public static bool AllowView(string viewperm, int usergroupid)
        {
            return HasPerm(viewperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发主题
        /// </summary>
        /// <param name="postperm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns></returns>
        public static bool AllowPost(string postperm, int usergroupid)
        {
            return HasPerm(postperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发回复
        /// </summary>
        /// <param name="replyperm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns></returns>
        public static bool AllowReply(string replyperm, int usergroupid)
        {
            return HasPerm(replyperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发主题或恢复
        /// </summary>
        /// <param name="perm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns>bool</returns>
        private static bool HasPerm(string perm, int usergroupid)
        {
            if (Utils.StrIsNullOrEmpty(perm))
                return true;

            return Utils.InArray(usergroupid.ToString(), perm);
        }


        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块下载附件
        /// </summary>
        /// <param name="getattachperm">允许下载附件的用户组id列表</param>
        /// <param name="usergroupid">当前用户组</param>
        /// <returns></returns>
        public static bool AllowGetAttach(string getattachperm, int usergroupid)
        {
            return HasPerm(getattachperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块上传附件
        /// </summary>
        /// <param name="postattachperm"></param>
        /// <param name="usergroupid"></param>
        /// <returns></returns>
        public static bool AllowPostAttach(string postattachperm, int usergroupid)
        {
            return HasPerm(postattachperm, usergroupid);
        }


        /// <summary>
        /// 返回全部版块列表并缓存
        /// </summary>
        /// <returns>板块信息数组</returns>
        public static List<ForumInfo> GetForumList()
        {
            DNTCache cache = DNTCache.GetCacheService();
            List<ForumInfo> forumList = cache.RetrieveObject("/Forum/ForumList") as List<ForumInfo>;

            if (forumList == null || forumList.Count == 0)
            {
                forumList = Discuz.Data.Forums.GetForumList();
                cache.AddObject("/Forum/ForumList", forumList);
            }
            return forumList;
        }

        /// <summary>
        /// 获取所有版块信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetForumListForDataTable()
        {
            return Data.Forums.GetForumListForDataTable();
        }

        /// <summary>
        /// 获取指定FID的板块信息列表
        /// </summary>
        /// <param name="fidList">板块FID列表</param>
        /// <returns></returns>
        public static List<ForumInfo> GetForumList(string fidList)
        {
            List<ForumInfo> forumList = new List<ForumInfo>();
            foreach (ForumInfo info in GetForumList())
            {
                foreach (string fid in fidList.Split(','))
                {
                    if (fid == info.Fid.ToString())
                        forumList.Add(info);
                }
            }
            return forumList;
        }

        /// <summary>
        /// 获得指定的分类或版块信息
        /// </summary>
        /// <param name="fid">分类或版块ID</param>
        /// <returns></returns>
        public static ForumInfo GetForumInfo(int fid)
        {
            return GetForumInfo(fid, true);
        }

        /// <summary>
        /// 获得指定的分类或版块信息
        /// </summary>
        /// <param name="fid">分类或版块ID</param>
        /// <param name="clone">返回值是否为clone对象</param>
        /// <returns></returns>
        public static ForumInfo GetForumInfo(int fid, bool clone)
        {
            if (fid < 1)
                return null;

            List<ForumInfo> forumList = GetForumList();
            if (forumList == null)
                return null;

            foreach (ForumInfo foruminfo in forumList)
            {
                if (foruminfo.Fid == fid)
                {
                    foruminfo.Pathlist = foruminfo.Pathlist.Replace("a><a", "a> &raquo; <a");
                    return clone ? foruminfo.Clone() : foruminfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置当前版块主题数(不含子版块)
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>主题数</returns>
        public static int SetRealCurrentTopics(int fid)
        {
            return Discuz.Data.Forums.SetRealCurrentTopics(fid);
        }

        /// <summary>
        /// 获得可见的板块列表
        /// </summary>
        /// <returns>返回值是以英文逗号分割的板块ID</returns>
        public static string GetVisibleForum()
        {
            StringBuilder result = new StringBuilder();
            List<ForumInfo> forumList = GetVisibleForumList();

            if (forumList == null)
                return "";

            foreach (ForumInfo foruminfo in forumList)
            {
                result.AppendFormat(",{0}", foruminfo.Fid);
            }
            return result.Length > 0 ? result.Remove(0, 1).ToString() : "";
        }

        public static List<ForumInfo> GetVisibleForumList()
        {
            List<ForumInfo> forumList = GetForumList();
            List<ForumInfo> visibleForumList = new List<ForumInfo>();

            foreach (ForumInfo foruminfo in forumList)
            {
                if (foruminfo.Status > 0)
                {
                    Predicate<ForumInfo> match = new Predicate<ForumInfo>(delegate(ForumInfo info) { return info.Fid == foruminfo.Parentid; });
                    if (foruminfo.Layer > 0 && visibleForumList.Find(match) == null)
                        continue;

                    if (Utils.StrIsNullOrEmpty(foruminfo.Viewperm)) //当板块权限为空时，按照用户组权限
                    {
                        if (UserGroups.GetUserGroupInfo(7).Allowvisit != 1)
                            continue;
                    }
                    else //当板块权限不为空，按照板块权限
                    {
                        if (!AllowView(foruminfo.Viewperm, 7))
                            continue;
                    }
                    //当版块未设置密码时
                    if (Utils.StrIsNullOrEmpty(foruminfo.Password))
                        visibleForumList.Add(foruminfo);
                }
            }
            return visibleForumList;
        }

        /// <summary>
        /// 得到当前版块的主题类型选项
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <returns>主题类型字符串</returns>
        public static string GetCurrentTopicTypesOption(int fid, string topictypes)
        {
            //判断当前版块没有相应主题分类时
            if (Utils.StrIsNullOrEmpty(topictypes) || topictypes == "|")
                return "";

            DNTCache cache = DNTCache.GetCacheService();
            string topictypeoptions = cache.RetrieveObject("/Forum/TopicTypesOption" + fid) as string;
            if (topictypeoptions == null)
            {
                StringBuilder builder = new StringBuilder("<option value=\"0\">分类</option>");

                foreach (string topictype in topictypes.Split('|'))
                {
                    if (!Utils.StrIsNullOrEmpty(topictype.Trim()))
                        builder.AppendFormat("<option value=\"{0}\">{1}</option>", topictype.Split(',')[0], topictype.Split(',')[1]);
                }
                topictypeoptions = builder.ToString();
                cache.AddObject("/Forum/TopicTypesOption" + fid, topictypeoptions);
            }
            return topictypeoptions;
        }

        /// <summary>
        /// 得到当前版块的主题类型链接串 
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <returns>当前版块的主题类型链接串</returns>
        public static string GetCurrentTopicTypesLink(int fid, string topictypes, string fullpagename)
        {
            if (Utils.StrIsNullOrEmpty(topictypes))
                return "";

            DNTCache cache = DNTCache.GetCacheService();
            string topictypelinks = cache.RetrieveObject("/Forum/TopicTypesLink" + fid) as string;
            if (topictypelinks == null)
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder dropbuilder = new StringBuilder();
                foreach (string topictype in topictypes.Split('|'))
                {
                    if (!Utils.StrIsNullOrEmpty(topictype.Trim()))
                    {
                        //平版模式
                        if (topictype.Split(',')[2] == "0")
                            builder.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", fullpagename, fid, topictype.Split(',')[0], topictype.Split(',')[1]);
                        //下拉类型
                        else
                            dropbuilder.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", fullpagename, fid, topictype.Split(',')[0], topictype.Split(',')[1]);
                    }
                }
                //主题类型链接增加未分类选项
                builder.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", fullpagename, fid, 0, "未分类");
                if (!Utils.StrIsNullOrEmpty(dropbuilder.ToString()))
                {
                    builder.Append("<a id=\"topictypedrop\" onmouseover=\"showMenu(this.id, true);\">更多分类...</a>");
                    builder.Append("<ul class=\"p_pop\" id=\"topictypedrop_menu\" style=\"display: none\">");
                    builder.AppendFormat("<li class='topictype'>{0}</li>", dropbuilder.ToString().Trim());
                    builder.Append("</ul>");
                }
                topictypelinks = builder.ToString();
                cache.AddObject("/Forum/TopicTypesLink" + fid, topictypelinks);
            }
            return topictypelinks;
        }


        /// <summary>
        /// 获取指定版块特殊用户的权限
        /// </summary>
        /// <param name="Permuserlist"></param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        private static int GetForumSpecialUserPower(string Permuserlist, int userid)
        {
            foreach (string currentinf in Permuserlist.Split('|'))
            {
                if (!Utils.StrIsNullOrEmpty(currentinf) && currentinf.Split(',')[1] == userid.ToString())
                    return TypeConverter.StrToInt(currentinf.Split(',')[2]);
            }
            return 0;
        }

        /// <summary>
        /// 检查特殊用户权限
        /// </summary>
        /// <param name="permUserList">特殊用户列表</param>
        /// <param name="userId">查看权限用户ID</param>
        /// <param name="forumSpecialUserPower">论坛特殊用户权限</param>
        /// <returns></returns>
        private static bool ValidateSpecialUserPerm(string permUserList, int userId, ForumSpecialUserPower forumSpecialUserPower)
        {
            if (!Utils.StrIsNullOrEmpty(permUserList))
            {
                ForumSpecialUserPower forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(permUserList, userId);
                if (((int)(forumspecialuserpower & forumSpecialUserPower)) > 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 返回用户是否有权浏览该版块
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>BOOL</returns>
        public static bool AllowViewByUserId(string permUserList, int userId)
        {
            return ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.ViewByUser);
        }

        /// <summary>
        /// 返回用户是否有权在该版块发主题
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowPostByUserID(string permUserList, int userId)
        {
            return ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.PostByUser);
        }


        /// <summary>
        /// 返回用户是否有权在该版块发回复
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowReplyByUserID(string permUserList, int userId)
        {
            return ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.ReplyByUser);
        }

        /// <summary>
        /// 返回用户是否有权在该版块下载附件
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowGetAttachByUserID(string permUserList, int userId)
        {
            return ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.DownloadAttachByUser);
        }

        /// <summary>
        /// 返回用户是否有权在该版块上传附件
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowPostAttachByUserID(string permUserList, int userId)
        {
            return ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.PostAttachByUser);
        }

        /// <summary>
        /// 判断指定的主题分类是否属于本版块可用的范围之内
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        /// <param name="topictypes">本版可用的主题分类</param>
        /// <returns>bool</returns>
        public static bool IsCurrentForumTopicType(string typeid, string topictypes)
        {
            if (Utils.StrIsNullOrEmpty(topictypes))
                return true;

            foreach (string topictype in topictypes.Split('|'))
            {
                if (!Utils.StrIsNullOrEmpty(topictype.Trim()) && typeid.Trim() == topictype.Split(',')[0].Trim())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 更新指定版块的最新发帖数信息
        /// </summary>
        /// <param name="foruminfo"></param>
        public static void UpdateLastPost(ForumInfo foruminfo)
        {
            PostInfo postinfo = new PostInfo();
            int tid = Discuz.Data.Topics.GetLastPostTid(foruminfo, Forums.GetVisibleForum());
            if (tid > 0)
                postinfo = Discuz.Data.Posts.GetLastPostByTid(tid, PostTables.GetPostTableName(tid));
            else
            {
                postinfo.Pid = postinfo.Tid = 0;
                postinfo.Title = postinfo.Topictitle = "从未";
                postinfo.Postdatetime = "1900-1-1";
                postinfo.Poster = "";
                postinfo.Posterid = 0;
            }
            Discuz.Data.Forums.UpdateForumLastPost(foruminfo, postinfo);

            if (foruminfo.Layer > 1)
            {
                UpdateParentForumLastPost(foruminfo, postinfo);
            }
        }

        /// <summary>
        /// 更新父版块最后发帖信息
        /// </summary>
        /// <param name="foruminfo"></param>
        /// <param name="postinfo"></param>
        private static void UpdateParentForumLastPost(ForumInfo foruminfo, PostInfo postinfo)
        {
            int parentFid = Utils.StrToInt(foruminfo.Parentidlist.Split(',')[1], 0);//去parentidlist属性split后的第一个数组元素，因为第0个是分类

            if (parentFid > 0)
            {
                string fidList = "";

                //获取到游客可以访问到的版块fidlist
                foreach (string fid in AdminForums.FindChildNode(parentFid.ToString()).Split(','))
                {
                    if (fid == "0") continue;
                    foreach (DataRow dr in Forums.GetOpenForumList().Rows)
                    {
                        if (dr["fid"].ToString().Trim() == fid.Trim())
                        {
                            fidList += fid + ",";
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(fidList))
                    return;

                int forumsTid = Data.Forums.GetForumsLastPostTid(fidList.TrimEnd(','));

                if (forumsTid > 0)
                    postinfo = Discuz.Data.Posts.GetLastPostByTid(forumsTid, PostTables.GetPostTableName(forumsTid));
                else
                {
                    postinfo.Pid = postinfo.Tid = 0;
                    postinfo.Title = postinfo.Topictitle = "从未";
                    postinfo.Postdatetime = "1900-1-1";
                    postinfo.Poster = "";
                    postinfo.Posterid = 0;
                }
                Discuz.Data.Forums.UpdateForumLastPost(GetForumInfo(parentFid), postinfo);
            }
        }
        /// <summary>
        /// 更新所有版块的最后发帖人等信息
        /// </summary>
        public static void ResetLastPostInfo()
        {
            Discuz.Data.Forums.ResetLastPostInfo();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumIndexCollection"></param>
        /// <returns></returns>
        private static List<IndexPageForumInfo> GetRealForumIndexCollection(List<IndexPageForumInfo> forumIndexCollection)
        {
            List<IndexPageForumInfo> parentforums = new List<IndexPageForumInfo>();
            List<IndexPageForumInfo> subforums = new List<IndexPageForumInfo>();
            List<IndexPageForumInfo> result = new List<IndexPageForumInfo>();
            foreach (IndexPageForumInfo forum in forumIndexCollection)
            {
                if (forum.Parentid == 0)
                    parentforums.Add(forum);
                else
                    subforums.Add(forum);
            }

            foreach (IndexPageForumInfo forum in parentforums)
            {
                result.Add(forum);
                foreach (IndexPageForumInfo sub in subforums)
                {
                    if (sub.Parentid == forum.Fid)
                    {
                        sub.Colcount = forum.Colcount;
                        result.Add(sub);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取首页版块列表集合
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="hideprivate">0为显示  1为不显示</param>
        /// <param name="usergroupid"></param>
        /// <param name="moderstyle">版主显示样式0为横排，1为下拉</param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="todaycount"></param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<IndexPageForumInfo> GetForumIndexCollection(int hidePrivate, int userGroupId, int moderStyle, out int topicCount, out int postCount, out int todayCount)
        {
            Discuz.Common.Generic.List<IndexPageForumInfo> coll = new Discuz.Common.Generic.List<IndexPageForumInfo>();

            topicCount = 0;
            postCount = 0;
            todayCount = 0;
            int status = 0, colcount = 1;

            foreach (IndexPageForumInfo info in Discuz.Data.Forums.GetForumIndexList())
            {
                //判断是否为私密论坛
                if (!Utils.StrIsNullOrEmpty(info.Viewperm) && !Utils.InArray(userGroupId.ToString(), info.Viewperm))
                {
                    //hideprivate：0为显示  1为不显示
                    if (hidePrivate == 0)
                    {
                        info.Lasttitle = "";
                        info.Lastposter = "";
                        info.Status = -1;
                    }
                    else
                        continue;
                }

                //判断是否收起
                if (info.Layer == 0 && Utils.GetCookie("discuz_collapse").IndexOf("_category_" + info.Fid + "_") > -1)
                    info.Collapse = "display: none;";

                if (info.Status > 0)
                {
                    if (info.Parentid == 0 && info.Subforumcount > 0)
                    {
                        colcount = info.Colcount;
                        status = colcount;
                        info.Status = status + 1;
                    }
                    else
                    {
                        info.Status = ++status;
                        info.Colcount = colcount;
                    }
                }

                info.Moderators = GetModerators(info, moderStyle);

                if (Utils.StrIsNullOrEmpty(info.Lastpost) ||
                    (TypeConverter.StrToDateTime(info.Lastpost).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd")))
                    info.Todayposts = 0;

                //判断是否为私密论坛
                if (!Utils.StrIsNullOrEmpty(info.Viewperm) && !Utils.InArray(userGroupId.ToString(), info.Viewperm))
                {
                    //hideprivate：0为显示  1为不显示
                    if (hidePrivate == 0)
                    {
                        info.Lasttitle = "";
                        info.Lastposter = "";
                        info.Status = -1;
                    }
                }

                if (info.Layer > 0)
                {
                    //更新缓存中的主题数，帖子数，今日发帖数
                    ForumInfo forumInfo = GetForumInfo(info.Fid, false);
                    if (forumInfo != null)
                    {
                        forumInfo.Topics = info.Topics;
                        forumInfo.Posts = info.Posts;
                        forumInfo.Todayposts = info.Todayposts;
                    }

                    topicCount = topicCount + info.Topics;
                    postCount = postCount + info.Posts;
                    todayCount = todayCount + info.Todayposts;
                }
                coll.Add(info);
            }
            return GetRealForumIndexCollection(coll);
        }

        /// <summary>
        /// 获取版主信息
        /// </summary>
        /// <param name="info">版块信息</param>
        /// <param name="moderStyle">版主显示样式,0为横排，1为下拉</param>
        /// <returns></returns>
        private static string GetModerators(IndexPageForumInfo info, int moderStyle)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder subModeratorsBuilder = new StringBuilder();

            //如果当前用户权限不够
            if (sb.Length > 0)
                sb.Remove(0, sb.Length);

            int mainModerCount = info.Layer <= 0 ? 3 : 6;

            int count = 0;
            foreach (string moderators in Utils.SplitString(info.Moderators, ","))
            {
                if (!Utils.StrIsNullOrEmpty(moderators.Trim()))
                {
                    if (moderStyle == 0)
                    {
                        string link = string.Format("<a href=\"{0}userinfo.aspx?username={1}\" target=\"_blank\">{2}</a>,", BaseConfigs.GetForumPath, Utils.UrlEncode(moderators.Trim()), moderators.Trim());
                        //如果主版主列表个数已经超过限定值，则将版主加入到子列表中
                        if (count++ < mainModerCount)
                            sb.Append(link);
                        else
                            subModeratorsBuilder.AppendFormat("<li>{0}</li>", link.TrimEnd(','));
                    }
                    else
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", moderators.Trim(), moderators.Trim());
                }
            }
            if (!Utils.StrIsNullOrEmpty(sb.ToString()) && moderStyle == 1)
            {
                sb.Insert(0, string.Format("<select style=\"width: 100px;\" onchange=\"window.open('{0}userinfo.aspx?username=' + escape(this.value));\">", BaseConfigs.GetForumPath));
                sb.Append("</select>");
            }
            if (!string.IsNullOrEmpty(subModeratorsBuilder.ToString()))
            {
                subModeratorsBuilder.Insert(0, string.Format("<a id=\"forum{0}_submoderators\" href=\"###\" onclick=\"showMenu({{'ctrlid':this.id, 'pos':'21'}})\">......</a><ul id=\"forum{0}_submoderators_menu\" class=\"p_pop moders\" style=\"position: absolute; z-index: 301; left: 998.5px; top: 93px; display: none;\">", info.Fid));
                subModeratorsBuilder.Append("</ul>");
                sb.Append(subModeratorsBuilder);
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 获得子版块列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="colcount">每行显示几个版块</param>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="moderstyle">版主显示样式</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<IndexPageForumInfo> GetSubForumCollection(int fid, int colcount, int hideprivate, int usergroupid, int moderstyle)
        {
            Discuz.Common.Generic.List<IndexPageForumInfo> coll = new Discuz.Common.Generic.List<IndexPageForumInfo>();

            if (fid > 0)
            {
                coll = Discuz.Data.Forums.GetSubForumList(fid, colcount);

                foreach (IndexPageForumInfo info in coll)
                {
                    info.Description = UBB.ParseSimpleUBB(info.Description); //替换版块介绍中的UBB
                    info.Moderators = GetModerators(info, moderstyle);

                    if (Utils.StrIsNullOrEmpty(info.Lastpost) ||
                       (TypeConverter.StrToDateTime(info.Lastpost).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd")))
                        info.Todayposts = 0;

                    //判断是否为私密论坛
                    if (!Utils.StrIsNullOrEmpty(info.Viewperm) && !Utils.InArray(usergroupid.ToString(), info.Viewperm))
                    {
                        //hideprivate：0为显示  1为不显示
                        if (hideprivate == 0)
                        {
                            info.Lasttitle = "";
                            info.Lastposter = "";
                            info.Status = -1;
                        }
                    }
                }
            }
            return coll;
        }


        /// <summary>
        /// 检查rewritename是否存在或非法
        /// </summary>
        /// <param name="rewriteName"></param>
        /// <returns>如果存在或者非法的Rewritename则返回true,否则为false</returns>
        public static bool CheckRewriteNameInvalid(string rewriteName)
        {
            //先检查此name是否非法
            foreach (string illegalName in "install,upgrade,admin,aspx,tools,archive,space".Split(','))
            {
                if (rewriteName.IndexOf(illegalName) != -1)
                    return true;
            }

            if (!Regex.IsMatch(rewriteName, @"([\w|\-|_])+"))
                return true;

            //再检查是否存在
            return Discuz.Data.Forums.CheckRewriteNameInvalid(rewriteName);
        }

        /// <summary>
        /// 获得所有版块列表的option字符串
        /// </summary>
        /// <returns></returns>
        public static string GetDropdownOptions()
        {
            DNTCache cache = DNTCache.GetCacheService();
            string result = cache.RetrieveObject("/Forum/DropdownOptions") as string;

            if (result == null)
            {
                StringBuilder stringbuilder = new StringBuilder();
                DataTable dt = Discuz.Data.Forums.GetShortForumList();
                string blank = Utils.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");

                foreach (DataRow dr in dt.Select("parentid=0"))
                {
                    stringbuilder.AppendFormat("<option value=\"{0}\" disabled='true'>{1}</option>", dr[0].ToString().Trim(), dr[1].ToString().Trim());
                    stringbuilder.Append(BindNode(dr[0].ToString().Trim(), dt, blank));
                }
                result = stringbuilder.ToString();
                cache.AddObject("/Forum/DropdownOptions", result);
            }
            return result;
        }

        /// <summary>
        /// 获得版主所见下拉列表选项
        /// </summary>
        /// <param name="username">版主用户名</param>
        /// <returns></returns>
        public static string GetModerDropdownOptions(string username)
        {
            StringBuilder stringbuilder = new StringBuilder();

            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (Utils.InArray(username, f.Moderators))
                    stringbuilder.AppendFormat("<option value=\"{0}\">{1}</option>", f.Fid, f.Name.Trim());
            }
            return stringbuilder.ToString();
        }

        private static string BindNode(string parentid, DataTable dt, string blank)
        {
            StringBuilder stringbuilder = new StringBuilder();

            foreach (DataRow dr in dt.Select("parentid=" + parentid))
            {
                stringbuilder.AppendFormat("<option value=\"{0}\">{1}{2}</option>", dr[0].ToString().Trim(), blank, dr[1].ToString().Trim());
                stringbuilder.Append(BindNode(dr[0].ToString().Trim(), dt, Utils.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + blank));
            }
            return stringbuilder.ToString();
        }

        /// <summary>
        /// 获取开放版块列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOpenForumList()
        {
            return Data.Forums.GetOpenForumList();
        }

        /// <summary>
        /// 更新版块显示顺序，将大于当前显示顺序的版块显示顺序加1
        /// </summary>
        /// <param name="minDisplayOrder"></param>
        public static void UpdateFourmsDisplayOrder(int minDisplayOrder)
        {
            Data.Forums.UpdateFourmsDisplayOrder(minDisplayOrder);
        }

        /// <summary>
        /// 获取非默认模板数
        /// </summary>
        /// <returns></returns>
        public static int GetSpecifyForumTemplateCount()
        {
            int count = 0;
            foreach (ForumInfo forumInfo in GetForumList())
            {
                if (forumInfo.Templateid != 0 && forumInfo.Templateid != GeneralConfigs.GetDefaultTemplateID())
                    count++;
            }
            return count;
        }


        /// <summary>
        /// 更新板块的字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldname">字段</param>
        /// <returns></returns>
        public static int UpdateForumField(int fid, string fieldname, string fieldvalue)
        {
            return Data.Forums.UpdateForumField(fid, fieldname, fieldvalue);
        }
        /// <summary>
        /// 更新版块版主的名字
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="oldName">旧版主名字</param>
        /// <param name="newName">新版主名字，为空则删除该版主</param>
        public static void UpdateModeratorName(string oldName, string newName)
        {
            Data.Forums.UpdateModeratorName(oldName, newName);
        }

        /// <summary>
        /// 获取主题分类关联的版块链接
        /// </summary>
        /// <param name="topicTypeId">主题分类Id</param>
        /// <returns></returns>
        public static string GetForumLinkOfAssociatedTopicType(int topicTypeId)
        {
            string linkString = "";

            //处理每个论坛版块            
            DataTable ForumNameIncludeTopicType = Data.Forums.GetForumNameIncludeTopicType();
            foreach (DataRow dr in ForumNameIncludeTopicType.Rows)
            {
                //将主题分类列表用间隔符“|”切开
                foreach (string type in dr["topictypes"].ToString().Split('|'))
                {
                    //查找主题ID（Id+","的做法是为了方便匹配，因为主题ID在版块中保存在每一项的开始，如果等于0，就说明找到了，小于0表示未找到，大于0表示并非主题ID）
                    if (type.IndexOf(topicTypeId + ",") == 0)
                    {
                        //形成字符串
                        linkString += "<a href='" + BaseConfigs.GetForumPath + "showforum.aspx?forumid=" + dr["fid"] + "&typeid=" + topicTypeId + "&search=1' target='_blank'>" + dr["name"].ToString().Trim() + "</a>";
                        linkString += "[<a href='forum_editforums.aspx?fid=" + dr["fid"] + "&tabindex=4'>编辑</a>],";
                        //每一个主题分类只能存在于一个版块中，找到后就不必再向下查找，所以跳出本版块，接着查找下一版块
                        break;
                    }
                }
            }
            //如果有str不为空说明有包含该主题ID的版块，所以去掉最后的一个“,”
            return linkString.TrimEnd(',');
        }

        /// <summary>
        /// 获取主题分类不为空的板块
        /// </summary>
        /// <returns></returns>
        public static DataTable GetExistTopicTypeOfForum()
        {
            return Data.Forums.GetExistTopicTypeOfForum();
        }

        /// <summary>
        /// 删除版块
        /// </summary>
        /// <param name="fid">版块Id</param>
        public static bool DeleteForum(string fid)
        {
            if (Discuz.Data.Forums.IsExistSubForum(int.Parse(fid)))
                return false;

            Data.Forums.DeleteForum(Posts.GetPostTableName(), fid);
            DNTCache.GetCacheService().RemoveObject("/Forum/UI/ForumListBoxOptions");
            DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");
            return true;
        }
        /// <summary>
        /// 更新版本子版数
        /// </summary>
        /// <param name="fid">版块Id</param>
        public static void UpdateSubForumCount(int fid)
        {
            Data.Forums.UpdateSubForumCount(fid);
        }

        /// <summary>
        /// 获取某一个字段
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <param name="fieldname">字段名</param>
        /// <returns></returns>
        public static DataTable GetForumField(int fid, string fieldname)
        {
            return Data.Forums.GetForumField(fid, fieldname);
        }

        /// <summary>
        /// 获取积分数组
        /// </summary>
        /// <param name="credits"></param>
        /// <returns></returns>
        public static float[] GetValues(string credits)
        {
            float[] values = new float[8];
            if (!Utils.StrIsNullOrEmpty(credits))
            {
                int index = 0;
                foreach (string ext in Utils.SplitString(credits, ","))
                {
                    if (index == 0)
                    {
                        if (!ext.Equals("True"))
                        {
                            values = null;
                            break;
                        }
                        index++;
                        continue;
                    }
                    values[index - 1] = Utils.StrToFloat(ext, 0.0f);
                    index++;
                    if (index > 8)
                        break;
                }
                return values;
            }
            return null;
        }

        /// <summary>
        /// 获取访问过的版块信息
        /// </summary>
        /// <returns></returns>
        public static SimpleForumInfo[] GetVisitedForums()
        {
            string visitedForums = Utils.GetCookie("visitedforums");
            if (visitedForums == "")
                return new SimpleForumInfo[0];

            List<SimpleForumInfo> simpleForumList = new List<SimpleForumInfo>();
            foreach (string fid in visitedForums.Split(','))
            {
                foreach (ForumInfo forumInfo in Forums.GetForumList())
                {
                    if (forumInfo.Fid.ToString() == fid)
                    {
                        SimpleForumInfo simpleForumInfo = new SimpleForumInfo();
                        simpleForumInfo.Fid = forumInfo.Fid;
                        simpleForumInfo.Name = Utils.RemoveHtml(forumInfo.Name);  //如果不过滤掉HTML代码，则如果版块名称中存在html代码，会出现js错误，并且快速发帖出显示也不正常
                        simpleForumInfo.Url = Urls.ShowForumAspxRewrite(forumInfo.Fid, 1, forumInfo.Rewritename);
                        simpleForumInfo.Postbytopictype = forumInfo.Postbytopictype;
                        simpleForumInfo.Topictypes = forumInfo.Topictypes;
                        simpleForumList.Add(simpleForumInfo);
                        break;
                    }
                }
            }
            return simpleForumList.ToArray();
        }

        /// <summary>
        /// 获取最后发帖版块信息
        /// </summary>
        /// <returns></returns>
        public static SimpleForumInfo GetLastPostedForum()
        {
            string lastPostedForum = Utils.GetCookie("lastpostedforum");
            if (lastPostedForum == "")
                return null;
            foreach (ForumInfo forumInfo in Forums.GetForumList())
            {
                if (forumInfo.Fid.ToString() == lastPostedForum)
                {
                    SimpleForumInfo simpleForumInfo = new SimpleForumInfo();
                    simpleForumInfo.Fid = forumInfo.Fid;
                    simpleForumInfo.Name = Utils.RemoveHtml(forumInfo.Name);    //如果不过滤掉HTML代码，则如果版块名称中存在html代码，会出现js错误，并且快速发帖出显示也不正常
                    simpleForumInfo.Url = Urls.ShowForumAspxRewrite(forumInfo.Fid, 1, forumInfo.Rewritename);
                    simpleForumInfo.Postbytopictype = forumInfo.Postbytopictype;
                    simpleForumInfo.Topictypes = forumInfo.Topictypes;
                    return simpleForumInfo;
                }
            }
            return null;
        }

        public static string ShowForumCondition(int sqlid, int cond)
        {
            return DatabaseProvider.GetInstance().ShowForumCondition(sqlid, cond);
        }

        /// <summary>
        /// 更新所有版块的主题数
        /// </summary>
        public static void ResetForumsTopics()
        {
            Discuz.Data.Forums.ResetForumsTopics();
        }

        /// <summary>
        /// 更新所有版块的今日发帖数
        /// </summary>
        public static void ResetTodayPosts()
        {
            Discuz.Data.Forums.ResetTodayPosts();
        }

        /// <summary>
        /// 获取第一个版块的信息
        /// </summary>
        /// <returns></returns>
        public static int GetFirstFourmID()
        {
            return Discuz.Data.Forums.GetFirstFourmID();
        }
    }
}
