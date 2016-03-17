using System;
using System.Data;
using System.Text;

using Discuz.Entity;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Cache.Data;

namespace Discuz.Data
{
    public class Posts
    {
        /// <summary>
        /// 是否启用缓存帖子表
        /// </summary>
        public static bool appDBCache = (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheposts.Enable);

        public static ICachePosts IPostService = appDBCache ? DBCacheService.GetPostsService() : null;

        public static PostInfo GetLastPostByTid(int tid, string tableName)
        {
            PostInfo postinfo = new PostInfo();
            DataTable dt = DatabaseProvider.GetInstance().GetLastPostByTid(tid, tableName);
            if (dt.Rows.Count > 0)
            {
                postinfo.Pid = TypeConverter.ObjectToInt(dt.Rows[0]["pid"]);
                postinfo.Tid = TypeConverter.ObjectToInt(dt.Rows[0]["tid"]);
                postinfo.Title = dt.Rows[0]["title"].ToString().Trim();
                postinfo.Postdatetime = dt.Rows[0]["postdatetime"].ToString().Trim();
                postinfo.Poster = dt.Rows[0]["poster"].ToString().Trim();
                postinfo.Posterid = TypeConverter.ObjectToInt(dt.Rows[0]["posterid"]);
                postinfo.Topictitle = Topics.GetTopicInfo(postinfo.Tid, 0, 0).Title;
            }
            else
            {
                postinfo.Pid = 0;
                postinfo.Tid = 0;
                postinfo.Title = "从未";
                postinfo.Topictitle = "从未";
                postinfo.Postdatetime = "1900-1-1";
                postinfo.Poster = "";
                postinfo.Posterid = 0;
            }
            dt.Dispose();
            return postinfo;
        }

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="postTableId">分表ID</param>
        /// <returns>帖子ID</returns>
        public static int CreatePost(PostInfo postInfo, string postTableId)
        {
            int postId = DatabaseProvider.GetInstance().CreatePost(postInfo, postTableId);
            //更新TTCache缓存中的用户信息
            if (postInfo.Invisible == 0 && Users.appDBCache && Users.IUserService != null)
            {
                UserInfo userInfo = Users.IUserService.GetUserInfo(postInfo.Posterid);
                if (userInfo != null)
                {
                    userInfo.Lastpost = postInfo.Postdatetime;
                    userInfo.Lastpostid = postId;
                    userInfo.Lastposttitle = postInfo.Title;
                    userInfo.Posts = userInfo.Posts + 1;
                    userInfo.Lastactivity = DateTime.Now.ToString();
                    userInfo.Newpm = 1;
                    Users.IUserService.UpdateUser(userInfo);
                }
            }

            //更新Cache缓存中的用户信息
            if (postInfo.Invisible == 0 && Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.ResetTopicByTid(postInfo.Tid);

            //创建Cache缓存中的帖子信息(权限于当前正在使用的分表)
            if (appDBCache && IPostService != null && PostTables.GetPostTableId(postInfo.Tid) == postTableId)
            {
                postInfo.Pid = postId;
                IPostService.CreatePost(postInfo, postTableId);
            }

            return postId;
        }

        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <returns>更新数量</returns>
        public static int UpdatePost(PostInfo postInfo, string postTableId)
        {
            int result = DatabaseProvider.GetInstance().UpdatePost(postInfo, postTableId);
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
            {
                IPostService.UpdatePost(postInfo, postTableId);
            }
            return result;
        }

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="chanagePostStatistic">是否更新帖子数量统计</param>
        /// <returns>删除数量</returns>
        public static int DeletePost(string postTableId, int pid, bool chanagePostStatistic)
        {
            if (Users.appDBCache && Users.IUserService != null)
            {
                PostInfo postInfo = GetPostInfo(postTableId, pid);
                if (postInfo != null)
                    Users.IUserService.DeleteUsers(postInfo.Posterid.ToString());
            }

            int tid = 0;//该变量用于获取帖子所属TID，用于后面的CACHE缓存更新
            if (Topics.appDBCache && Topics.ITopicService != null)
            {
                PostInfo postInfo = Posts.GetPostInfo(postTableId, pid);
                if (postInfo != null) tid = postInfo.Tid;
            }
            int result = DatabaseProvider.GetInstance().DeletePost(postTableId, pid, chanagePostStatistic);

            if (Topics.appDBCache && Topics.ITopicService != null && tid > 0)
                Topics.ITopicService.ResetTopicByTid(tid);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.DeletePost(postTableId, pid);

            return result;
        }

        public static PostInfo GetPostInfo(string postTableId, int pid)
        {
            PostInfo postInfo = new PostInfo();
            IDataReader reader = DatabaseProvider.GetInstance().GetPostInfo(postTableId, pid);
            if (reader.Read())
            {
                postInfo = LoadSinglePostInfo(reader);
                reader.Close();
                return postInfo;
            }
            reader.Close();
            return null;
        }

        /// <summary>
        /// 获取需要审核的回复
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="ppp">每页帖子书</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="postTableId">分表</param>
        /// <param name="filter">可见性过滤器</param>
        /// <returns></returns>
        public static List<PostInfo> GetUnauditPost(string fidList, int ppp, int pageIndex, int postTableId, int filter)
        {
            List<PostInfo> list = new List<PostInfo>();
            IDataReader reader = DatabaseProvider.GetInstance().GetUnauditNewPost(fidList, ppp, pageIndex, postTableId, filter);
            while (reader.Read())
            {
                list.Add(LoadSinglePostInfo(reader));
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取需要审核的回复数
        /// </summary>
        /// <param name="fidList">版块ID</param>
        /// <param name="postTableId">分表ID</param>
        /// <param name="filter">可见性过滤器</param>
        /// <returns></returns>
        public static int GetUnauditNewPostCount(string fidList, int postTableId, int filter)
        {
            return DatabaseProvider.GetInstance().GetUnauditNewPostCount(fidList, postTableId, filter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tidList"></param>
        /// <param name="postTableIdArray"></param>
        /// <returns></returns>
        public static DataTable GetPostDataTable(string tidList, string[] postTableIdArray)
        {
            return DatabaseProvider.GetInstance().GetPostList(tidList, postTableIdArray);
        }

        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetLastPostDataTable(PostpramsInfo postpramsInfo)
        {
            return DatabaseProvider.GetInstance().GetLastPostList(postpramsInfo, PostTables.GetPostTableId(postpramsInfo.Tid));
        }

        /// <summary>
        /// 获得最后回复的帖子列表，支持分页
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <returns></returns>
        public static DataTable GetPagedLastPostDataTable(PostpramsInfo postpramsInfo)
        {
            return DatabaseProvider.GetInstance().GetPagedLastPostList(postpramsInfo, PostTables.GetPostTableName(postpramsInfo.Tid));
        }

        /// <summary>
        /// 获取指定tid的帖子DataTable
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子DataTable</returns>
        public static DataTable GetPostTree(int tid)
        {
            return DatabaseProvider.GetInstance().GetPostTree(tid, PostTables.GetPostTableId(tid));
        }

        /// <summary>
        /// 获取指定tid的帖子总数
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子总数</returns>
        public static int GetPostCount(int tid)
        {
            return DatabaseProvider.GetInstance().GetPostCountByTid(tid, PostTables.GetPostTableName(tid));
        }

        /// <summary>
        /// 获取指定主题下小于pid的有效帖子数
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static int GetPostCountBeforePid(int pid, int tid)
        {
            return DatabaseProvider.GetInstance().GetPostsCountBeforePid(pid, tid);
        }

        /// <summary>
        /// 根据分表名更新主题的最后回复等信息
        /// </summary>
        /// <param name="postTableName">分表名</param>
        public static void ResetLastRepliesInfoOfTopics(int postTableID)
        {
            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.ResetLastRepliesInfoOfTopics(postTableID);

            DatabaseProvider.GetInstance().ResetLastRepliesInfoOfTopics(postTableID);
        }

        /// <summary>
        /// 获得指定主题的第一个帖子的id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>帖子id</returns>
        public static int GetFirstPostId(int tid)
        {
            return DatabaseProvider.GetInstance().GetFirstPostId(tid, PostTables.GetPostTableId(tid));
        }

        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>是否是指定主题的回复者</returns>
        public static bool IsReplier(int tid, int uid)
        {
            return DatabaseProvider.GetInstance().IsReplier(tid, uid, PostTables.GetPostTableId(tid));
        }

        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        public static int UpdatePostRateTimes(int tid, string postidlist)
        {
            string postTableId = PostTables.GetPostTableId(tid);
            int result = DatabaseProvider.GetInstance().UpdatePostRateTimes(postidlist, postTableId);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.UpdatePostRateTimes(postTableId, postidlist);

            return result;
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsInfo)
        {
            IDataReader reader;
            string postTableId = PostTables.GetPostTableId(postpramsInfo.Tid);

            if (!postpramsInfo.Condition.Equals(""))
                reader = DatabaseProvider.GetInstance().GetPostListByCondition(postpramsInfo, postTableId);
            else
            {
                //更新Cache缓存中的帖子信息(目前只对当前分表进行查询，这主要出于将所有分表数据转换对于某些中型论坛的服务器硬盘内存要求过高的暂时方案)
                if (postTableId == PostTables.GetPostTableId() && appDBCache && IPostService != null)
                {
                    List<ShowtopicPagePostInfo> list = IPostService.GetPostList(postpramsInfo, postTableId);
                    if (list != null && list.Count > 0)
                        return list;
                }

                reader = DatabaseProvider.GetInstance().GetPostList(postpramsInfo, postTableId);
            }

            return LoadPostList(postpramsInfo, reader);
        }



        public static int UpdatePostAttachmentType(int tid, int pid, int attType)
        {
            string postTableId = PostTables.GetPostTableId(tid);
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.UpdatePostAttachmentType(postTableId, pid, attType);

            return DatabaseProvider.GetInstance().UpdatePostAttachmentType(pid, postTableId, attType);
        }

        /// <summary>
        /// 更新帖子评分
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="rate"></param>
        /// <param name="posttableid"></param>
        /// <returns></returns>
        public static int UpdatePostRate(int pid, float rate, string posttableid)
        {
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.UpdatePostAttachmentType(posttableid, pid, rate);

            return DatabaseProvider.GetInstance().UpdatePostRate(pid, rate, posttableid);
        }

        public static int CancelPostRate(string pidIdList, string posttableid)
        {
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.CancelPostRate(pidIdList, posttableid);

            return DatabaseProvider.GetInstance().CancelPostRate(pidIdList, posttableid);
        }

        /// <summary>
        /// 通过主题ID得到主帖内容,此方法可继续扩展
        /// </summary>
        /// <param name="tid"></param>
        /// <returns>ShowtopicPagePostInfo</returns>
        public static PostInfo GetTopicPostInfo(int tid)
        {
            PostInfo postInfo = new PostInfo();
            IDataReader reader = DatabaseProvider.GetInstance().GetSinglePost(tid, PostTables.GetPostTableId(tid));

            if (reader.Read())
            {
                postInfo = LoadSinglePostInfo(reader);
            }

            reader.Close();
            return postInfo;

        }

        /// <summary>
        /// 装载帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<ShowtopicPagePostInfo> LoadPostList(PostpramsInfo postpramsInfo, IDataReader reader)
        {
            List<ShowtopicPagePostInfo> postList = new List<ShowtopicPagePostInfo>();

            //序号(楼层)的初值
            int id = (postpramsInfo.Pageindex - 1) * postpramsInfo.Pagesize;

            while (reader.Read())
            {
                //当帖子中的posterid字段为0时, 表示该数据出现异常
                if (TypeConverter.ObjectToInt(reader["posterid"]) == 0)
                    continue;

                ShowtopicPagePostInfo postInfo = LoadSingleShowtopicPagePostInfo(reader);
                //扩展属性
                id++;

                postInfo.Id = id;

                postList.Add(postInfo);
            }
            reader.Close();

            return postList;
        }

        /// <summary>
        /// 获取ShowtopicPagePostInfo对象与ShowtopicPageAttachmentInfo对象列表
        /// </summary>
        /// <param name="postPramsInfo">参数对象</param>
        /// <param name="attachmentList">输出附件列表</param>
        /// <returns></returns>
        public static ShowtopicPagePostInfo GetPostInfoWithAttachments(PostpramsInfo postPramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentList)
        {
            attachmentList = new List<ShowtopicPageAttachmentInfo>();
            //得到帖子对应主题的所有附件,因为ACCESS不支持存储过程,故此方法在ACCESS版中做了特殊处理
            IDataReader attachmentReader;
            IDataReader reader = DatabaseProvider.GetInstance().GetSinglePost(out attachmentReader, postPramsInfo, PostTables.GetPostTableId(postPramsInfo.Tid));
            //Access版运行此处逻辑
            bool isAccess = false;
            if (attachmentReader == null)
                attachmentReader = reader;
            else
                isAccess = true;

            while (attachmentReader.Read())
                attachmentList.Add(Attachments.LoadSingleAttachmentInfo(attachmentReader));
            //bool next = false;
            //Access版运行此处逻辑
            if (!isAccess)
            {
                reader.NextResult();
            }
            ShowtopicPagePostInfo postInfo = null;
            if (reader.Read())
                postInfo = LoadSingleShowtopicPagePostInfo(reader);

            reader.Close();

            if (!attachmentReader.IsClosed)
                attachmentReader.Close();

            return postInfo;
        }

        /// <summary>
        /// 屏蔽帖子内容
        /// </summary>
        /// <param name="tableId">分表Id</param>
        /// <param name="postList">帖子Id列表</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        public static void BanPosts(string tableId, string postList, int invisible)
        {
            DatabaseProvider.GetInstance().SetPostsBanned(tableId, postList, invisible);
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.SetPostsBanned(tableId, postList, invisible);
        }

        /// <summary>
        /// 返回帖子列表
        /// </summary>
        /// <param name="postList">帖子ID</param>
        /// <param name="tableId">帖子分表ID</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPostList(string postList, string tableId)
        {
            return DatabaseProvider.GetInstance().GetPostList(postList, tableId);
        }

        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="validatePidList">需要验证的帖子Id列表</param>
        /// <param name="deletePidList">需要删除的帖子Id列表</param>
        /// <param name="ignorePidList">需要忽略的帖子ID列表</param>
        /// <param name="fidList">版块Id列表</param>
        public static void AuditPost(int postTableId, string validatePidList, string deletePidList, string ignorePidList, string fidList)
        {
            DatabaseProvider.GetInstance().AuditPost(postTableId, validatePidList, deletePidList, ignorePidList, fidList);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.PassPost(postTableId, validatePidList, deletePidList, ignorePidList, fidList);
        }

        /// <summary>
        /// 获取版主有权限管理的帖子数
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="postTableId">分表ID</param>
        /// <param name="pidList">帖子ID列表</param>
        /// <returns></returns>
        public static int GetModPostCountByPidList(string fidList, string postTableId, string pidList)
        {
            return DatabaseProvider.GetInstance().GetModPostCountByPidList(fidList, postTableId, pidList);
        }

        /// <summary>
        /// 获得指定用户回复指定主题次数
        /// </summary>
        /// <param name="postTableId">帖子分表Id</param>
        /// <param name="topicId">主题Id</param>
        /// <param name="posterId">用户Id</param>
        /// <returns>回复次数</returns>
        public static int GetPostCountByPosterId(string postTableId, int topicId, int posterId)
        {
            return DatabaseProvider.GetInstance().GetPostCount(postTableId, topicId, posterId);
        }

        public static DataTable GetPostCountFromIndex(string postsid)
        {
            return DatabaseProvider.GetInstance().GetPostCountFromIndex(postsid);
        }

        public static DataTable GetPostCountTable(string postsid)
        {
            return DatabaseProvider.GetInstance().GetPostCountTable(postsid);
        }

        public static void DeletePostByPosterid(int tableId, int uid)
        {
            DatabaseProvider.GetInstance().DeletePostByPosterid(tableId, uid);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.DeletePostByPosterid(tableId, uid);
        }

        /// <summary>
        /// 删除用户指定天数内的帖子
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="days">天数</param>
        public static void DeletePostByUidAndDays(int uid, int days)
        {
            DatabaseProvider.GetInstance().DeletePostByUidAndDays(uid, days);
        }

        /// <summary>
        /// 根据主题ID列表取出主题帖子
        /// </summary>
        /// <param name="posttableid">分表ID</param>
        /// <param name="tidlist">主题ID列表</param>
        /// <returns></returns>
        public static DataTable GetTopicListByTidlist(string posttableid, string tidlist)
        {
            return DatabaseProvider.GetInstance().GetTopicListByTidlist(posttableid, tidlist);
        }

        /// <summary>
        /// 获取帖子信息
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="pid">帖子Id</param>
        /// <returns></returns>
        public static DataTable GetPostInfoByPid(string postTableName, int pid)
        {
            return DatabaseProvider.GetInstance().GetPost(postTableName, pid);
        }

        /// <summary>
        /// 获取主帖子信息
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public static DataTable GetMainPostInfo(string postTableName, int tid)
        {
            return DatabaseProvider.GetInstance().GetMainPostByTid(postTableName, tid);
        }

        /// <summary>
        /// 获取主题的最后一个回复
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="posttablename">分表名称</param>
        /// <returns></returns>
        public static IDataReader GetLastPostByFid(int fid, string posttablename)
        {
            return DatabaseProvider.GetInstance().GetLastPostByFid(fid, posttablename);
        }

        /// <summary>
        /// 更新帖子作者名称
        /// </summary>
        /// <param name="uid">要更新的帖子作者的Uid</param>
        /// <param name="newUserName">作者的新用户名</param>
        public static void UpdatePostPoster(int uid, string newUserName)
        {
            DatabaseProvider.GetInstance().UpdatePostPoster(uid, newUserName);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.UpdatePostPoster(uid, newUserName);
        }

        #region Private Method


        /// <summary>
        /// 装帖子信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static PostInfo LoadSinglePostInfo(IDataReader reader)
        {
            PostInfo postInfo = new PostInfo();
            postInfo.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            postInfo.Fid = TypeConverter.ObjectToInt(reader["fid"]);
            postInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            postInfo.Parentid = TypeConverter.ObjectToInt(reader["parentid"]);
            postInfo.Layer = TypeConverter.ObjectToInt(reader["layer"]);
            postInfo.Poster = reader["poster"].ToString();
            postInfo.Posterid = TypeConverter.ObjectToInt(reader["posterid"]);
            postInfo.Title = reader["title"].ToString();
            postInfo.Postdatetime = reader["postdatetime"].ToString();
            postInfo.Message = reader["message"].ToString();
            postInfo.Ip = reader["ip"].ToString();
            postInfo.Lastedit = reader["lastedit"].ToString();
            postInfo.Invisible = TypeConverter.ObjectToInt(reader["invisible"]);
            postInfo.Usesig = TypeConverter.ObjectToInt(reader["usesig"]);
            postInfo.Htmlon = TypeConverter.ObjectToInt(reader["htmlon"]);
            postInfo.Smileyoff = TypeConverter.ObjectToInt(reader["smileyoff"]);
            postInfo.Bbcodeoff = TypeConverter.ObjectToInt(reader["bbcodeoff"]);
            postInfo.Parseurloff = TypeConverter.ObjectToInt(reader["parseurloff"]);
            postInfo.Attachment = TypeConverter.ObjectToInt(reader["attachment"]);
            postInfo.Rate = TypeConverter.ObjectToInt(reader["rate"]);
            postInfo.Ratetimes = TypeConverter.ObjectToInt(reader["ratetimes"]);
            return postInfo;
        }

        /// <summary>
        /// 装载ShowtopicPagePostInfo对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static ShowtopicPagePostInfo LoadSingleShowtopicPagePostInfo(IDataReader reader)
        {
            ShowtopicPagePostInfo postInfo = new ShowtopicPagePostInfo();
            postInfo.Pid = Int32.Parse(reader["pid"].ToString());
            postInfo.Attachment = Int32.Parse(reader["attachment"].ToString());
            postInfo.Fid = TypeConverter.ObjectToInt(reader["fid"]);
            postInfo.Title = reader["title"].ToString().Trim();
            postInfo.Layer = Int32.Parse(reader["layer"].ToString());
            postInfo.Message = reader["message"].ToString().TrimEnd();
            postInfo.Lastedit = reader["lastedit"].ToString().Trim();
            postInfo.Postdatetime = reader["postdatetime"].ToString().Trim();

            postInfo.Poster = reader["poster"].ToString().Trim();
            postInfo.Posterid = Int32.Parse(reader["posterid"].ToString());
            postInfo.Invisible = Int32.Parse(reader["invisible"].ToString());
            postInfo.Usesig = Int32.Parse(reader["usesig"].ToString());
            postInfo.Htmlon = Int32.Parse(reader["htmlon"].ToString());
            postInfo.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
            postInfo.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
            postInfo.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
            postInfo.Rate = Int32.Parse(reader["rate"].ToString());
            postInfo.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
            postInfo.Ubbmessage = reader["message"].ToString().TrimEnd();
            if (postInfo.Posterid > 0)
            {
                postInfo.Oltime = reader["oltime"].ToString().Trim();
                postInfo.Lastvisit = reader["lastvisit"].ToString().Trim();
                postInfo.Nickname = reader["nickname"].ToString().Trim();
                postInfo.Username = reader["username"].ToString().Trim();
                postInfo.Groupid = Utils.StrToInt(reader["groupid"], 0);
                postInfo.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                postInfo.Gender = Utils.StrToInt(reader["gender"], 2);
                postInfo.Bday = reader["bday"].ToString().Trim();
                postInfo.Showemail = Utils.StrToInt(reader["showemail"], 0);
                postInfo.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                postInfo.Credits = Utils.StrToInt(reader["credits"], 0);
                postInfo.Extcredits1 = TypeConverter.StrToFloat(reader["extcredits1"].ToString());
                postInfo.Extcredits2 = TypeConverter.StrToFloat(reader["extcredits2"].ToString());
                postInfo.Extcredits3 = TypeConverter.StrToFloat(reader["extcredits3"].ToString());
                postInfo.Extcredits4 = TypeConverter.StrToFloat(reader["extcredits4"].ToString());
                postInfo.Extcredits5 = TypeConverter.StrToFloat(reader["extcredits5"].ToString());
                postInfo.Extcredits6 = TypeConverter.StrToFloat(reader["extcredits6"].ToString());
                postInfo.Extcredits7 = TypeConverter.StrToFloat(reader["extcredits7"].ToString());
                postInfo.Extcredits8 = TypeConverter.StrToFloat(reader["extcredits8"].ToString());
                postInfo.Posts = Utils.StrToInt(reader["posts"], 0);
                postInfo.Joindate = reader["joindate"].ToString().Trim();
                postInfo.Lastactivity = reader["lastactivity"].ToString().Trim();
                postInfo.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                postInfo.Avatar = reader["avatar"].ToString();
                postInfo.Avatarwidth = Utils.StrToInt(reader["avatarwidth"], 0);
                postInfo.Avatarheight = Utils.StrToInt(reader["avatarheight"], 0);
                postInfo.Medals = reader["medals"].ToString();
                postInfo.Signature = reader["signature"].ToString();
                postInfo.Location = reader["location"].ToString();
                postInfo.Customstatus = reader["customstatus"].ToString();
                postInfo.Website = reader["website"].ToString();
                postInfo.Icq = reader["icq"].ToString();
                postInfo.Qq = reader["qq"].ToString();
                postInfo.Msn = reader["msn"].ToString();
                postInfo.Yahoo = reader["yahoo"].ToString();
                postInfo.Skype = reader["skype"].ToString();

                //部分属性需要根据不同情况来赋值

                //根据用户自己的设置决定是否显示邮箱地址
                if (postInfo.Showemail == 0)
                    postInfo.Email = "";
                else
                    postInfo.Email = reader["email"].ToString().Trim();

                // 最后活动时间50分钟内的为在线, 否则不在线
                if (Utils.StrDateDiffMinutes(postInfo.Lastactivity, 50) < 0)
                    postInfo.Onlinestate = 1;
                else
                    postInfo.Onlinestate = 0;

                //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                postInfo.Ip = reader["ip"].ToString().Trim();
            }
            else
            {
                postInfo.Nickname = "游客";
                postInfo.Username = "游客";
                postInfo.Groupid = 7;
                postInfo.Showemail = 0;
                postInfo.Digestposts = 0;
                postInfo.Credits = 0;
                postInfo.Extcredits1 = 0;
                postInfo.Extcredits2 = 0;
                postInfo.Extcredits3 = 0;
                postInfo.Extcredits4 = 0;
                postInfo.Extcredits5 = 0;
                postInfo.Extcredits6 = 0;
                postInfo.Extcredits7 = 0;
                postInfo.Extcredits8 = 0;
                postInfo.Posts = 0;
                postInfo.Joindate = "2006-9-1 1:1:1";
                postInfo.Lastactivity = "2006-9-1 1:1:1"; ;
                postInfo.Userinvisible = 0;
                postInfo.Avatar = "";
                postInfo.Avatarwidth = 0;
                postInfo.Avatarheight = 0;
                postInfo.Medals = "";
                postInfo.Signature = "";
                postInfo.Location = "";
                postInfo.Customstatus = "";
                postInfo.Website = "";
                postInfo.Icq = "";
                postInfo.Qq = "";
                postInfo.Msn = "";
                postInfo.Yahoo = "";
                postInfo.Skype = "";
                //部分属性需要根据不同情况来赋值
                postInfo.Email = "";
                postInfo.Onlinestate = 1;
                postInfo.Medals = "";

                postInfo.Ip = reader["ip"].ToString().Trim();
                if (postInfo.Ip.IndexOf('.') > -1)
                {
                    postInfo.Ip = postInfo.Ip.Substring(0, postInfo.Ip.LastIndexOf(".") + 1) + "*";
                }
            }
            return postInfo;
        }

        #endregion

        /// <summary>
        /// 删除帖子
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns></returns>
        public static bool DeleteTopicByTid(int tid, string postTableName)
        {
            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.DeleteTopicByTid(tid, postTableName);

            return DatabaseProvider.GetInstance().DeleteTopicByTid(tid, postTableName);
        }

        /// <summary>
        /// 获取帖子列表
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="posttableid">分表id</param>
        /// <returns></returns>
        public static DataSet GetPosts(int tid, int pagesize, int pageindex, string posttableid)
        {
            return DatabaseProvider.GetInstance().GetPosts(tid, pagesize, pageindex, posttableid);
        }

        /// <summary>
        /// 获取分表帖数
        /// </summary>
        /// <param name="postTableid">分表Id</param>
        /// <returns></returns>
        public static int GetPostsCount(string postTableid)
        {
            return DatabaseProvider.GetInstance().GetPostsCount(postTableid);
        }

        /// <summary>
        /// 获取版块帖数
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="postTableName">分表名称</param>
        /// <returns></returns>
        public static int GetPostCount(int fid, string postTableName)
        {
            return DatabaseProvider.GetInstance().GetPostCount(fid, postTableName);
        }

        /// <summary>
        /// 获取版块当天帖数
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="postTableName">分表名称</param>
        /// <returns></returns>
        public static int GetTodayPostCount(int fid, string postTableName)
        {
            return DatabaseProvider.GetInstance().GetTodayPostCount(fid, postTableName);
        }

        /// <summary>
        /// 获取完整分表名
        /// </summary>
        /// <param name="postTableId">分表Id</param>
        /// <returns></returns>
        public static string GetPostTableName(int postTableId)
        {
            return BaseConfigs.GetTablePrefix + "posts" + postTableId;
        }

        /// <summary>
        /// 获取用户发帖数
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="postTableName">分表名称</param>
        /// <returns></returns>
        public static int GetPostCountByUid(int uid, string postTableName)
        {
            return DatabaseProvider.GetInstance().GetPostCountByUid(uid, postTableName);
        }

        /// <summary>
        /// 获取用户当天发帖数
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="postTableName">分表名称</param>
        /// <returns></returns>
        public static int GetTodayPostCountByUid(int uid, string postTableName)
        {
            return DatabaseProvider.GetInstance().GetTodayPostCountByUid(uid, postTableName);
        }


        /// <summary>
        /// 获取板块最后一帖
        /// </summary>
        /// <param name="fid">板块id</param>
        /// <param name="posttablename">分表名称</param>
        /// <param name="topiccount">主题数</param>
        /// <param name="postcount">回帖数</param>
        /// <param name="lasttid">最后发表主题ID</param>
        /// <param name="lasttitle">最后发表标题</param>
        /// <param name="lastpost">最后发表时间</param>
        /// <param name="lastposterid">最后发表人ID</param>
        /// <param name="lastposter">最后发表人</param>
        /// <param name="todaypostcount">今日发帖数</param>
        /// <returns></returns>
        public static IDataReader GetForumLastPost(int fid, string posttablename, int topiccount, int postcount, int lasttid,
                                     string lasttitle, string lastpost, int lastposterid, string lastposter,
                                     int todaypostcount)
        {
            return DatabaseProvider.GetInstance().GetForumLastPost(fid, posttablename, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        }

        /// <summary>
        /// 重设统计信息
        /// </summary>
        /// <param name="UserCount">用户数</param>
        /// <param name="TopicsCount">主题数</param>
        /// <param name="PostCount">帖子数</param>
        /// <param name="lastuserid">最后注册用户ID</param>
        /// <param name="lastusername">最后注册用户名称</param>
        public static void ReSetStatistic(int UserCount, int TopicsCount, int PostCount, string lastuserid, string lastusername)
        {
            DatabaseProvider.GetInstance().ReSetStatistic(UserCount, TopicsCount, PostCount, lastuserid, lastusername);
        }

        /// <summary>
        /// 获取未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">分表ID</param>
        /// <returns></returns>
        //public static DataTable GetUnauditPost(int currentPostTableId)
        //{
        //    return DatabaseProvider.GetInstance().GetUnauditPost(currentPostTableId);
        //}

        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        public static void PassPost(int currentPostTableId, string pidlist)
        {
            if (Users.appDBCache && Users.IUserService != null)
                Users.IUserService.RemoveUser(currentPostTableId, pidlist);

            DatabaseProvider.GetInstance().PassPost(currentPostTableId, pidlist);

            if (Topics.appDBCache && Topics.ITopicService != null)
                Topics.ITopicService.PassPostTopic(currentPostTableId, pidlist);

            //更新Cache缓存中的帖子信息
            if (appDBCache && IPostService != null)
                IPostService.PassPost(currentPostTableId, pidlist);
        }

        /// <summary>
        /// 获取帖子登记
        /// </summary>
        /// <param name="currentPostTableId">分表ID</param>
        /// <param name="postid">帖子ID</param>
        /// <returns></returns>
        public static DataTable GetPostLayer(int currentPostTableId, int postid)
        {
            return DatabaseProvider.GetInstance().GetPostLayer(currentPostTableId, postid);
        }

        /// <summary>
        /// 更新我的帖子
        /// </summary>
        public static void UpdateMyPost()
        {
            DatabaseProvider.GetInstance().UpdateMyPost();
        }

        /// <summary>
        /// 获取帖子审核的条件
        /// </summary>
        /// <param name="fid">版块Id</param>
        /// <param name="poster">帖子作者</param>
        /// <param name="title">标题</param>
        /// <param name="moderatorName">版主名称</param>
        /// <param name="postDateTimeStart">主题发布起始日期</param>
        /// <param name="postDateTimeEnd">主题发布结束日期</param>
        /// <param name="delDateTimeStart">删除起始日期</param>
        /// <param name="delDateTimeEnd">删除结束日期</param>
        /// <returns></returns>
        public static string GetTopicAuditCondition(int fid, string poster, string title, string moderatorName, DateTime postDateTimeStart,
            DateTime postDateTimeEnd, DateTime delDateTimeStart, DateTime delDateTimeEnd)
        {
            return DatabaseProvider.GetInstance().SearchTopicAudit(fid, poster, title, moderatorName, postDateTimeStart,
                postDateTimeEnd, delDateTimeStart, delDateTimeEnd);
        }

        /// <summary>
        /// 是否存在满足条件的需要审核的帖子
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static bool IsExistAuditTopic(string condition)
        {
            return DatabaseProvider.GetInstance().AuditTopicCount(condition);
        }

        /// <summary>
        /// 按条件获取帖子列表
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetPostListByCondition(string postTableName, string condition)
        {
            return DatabaseProvider.GetInstance().PostGridBind(postTableName, condition);
        }

        /// <summary>
        /// 查询帖子的条件
        /// </summary>
        /// <param name="forumid"></param>
        /// <param name="posttableid"></param>
        /// <param name="postdatetimeStart"></param>
        /// <param name="postdatetimeEnd"></param>
        /// <param name="poster"></param>
        /// <param name="lowerupper"></param>
        /// <param name="ip"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd,
                          string poster, bool lowerupper, string ip, string message)
        {
            return DatabaseProvider.GetInstance().SearchPost(forumid, posttableid, postdatetimeStart, postdatetimeEnd, poster, lowerupper, ip, message);
        }

        /// <summary>
        /// 获取用户单位时间内的发帖数
        /// </summary>
        /// <param name="topNumber">Top条数</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="dateNum">时间数</param>
        /// <param name="postTableName">当前帖子分表名</param>
        /// <returns></returns>
        public static List<UserPostCountInfo> GetUserPostCountList(int topNumber, DateType dateType, int dateNum, string postTableName)
        {
            List<UserPostCountInfo> userPostCountInfoList = new List<UserPostCountInfo>();

            IDataReader iDataReader = DatabaseProvider.GetInstance().GetUserPostCountList(topNumber, dateType, dateNum, postTableName);
            while (iDataReader.Read())
            {
                UserPostCountInfo userPostCountInfo = new UserPostCountInfo();
                userPostCountInfo.Uid = TypeConverter.ObjectToInt(iDataReader["uid"]);
                userPostCountInfo.Username = iDataReader["username"].ToString();
                userPostCountInfo.PostCount = TypeConverter.ObjectToInt(iDataReader["postcount"]);

                userPostCountInfoList.Add(userPostCountInfo);
            }
            iDataReader.Close();

            return userPostCountInfoList;
        }
    }
}
