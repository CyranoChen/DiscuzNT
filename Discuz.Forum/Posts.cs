using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;
using Discuz.Plugin.Preview;

namespace Discuz.Forum
{
    /// <summary>
    /// 帖子操作类
    /// </summary>
    public class Posts
    {
        private static Regex regexAttach = new Regex(@"\[attach\](\d+?)\[\/attach\]", RegexOptions.IgnoreCase);

        private static Regex regexHide = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);

        private static Regex regexAttachImg = new Regex(@"\[attachimg\](\d+?)\[\/attachimg\]", RegexOptions.IgnoreCase);

        /// <summary>
        /// 得到指定主题的帖子所在分表ID
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>分表ID</returns>
        public static string GetPostTableId(int tid)
        {
            return tid > 0 ? Discuz.Data.PostTables.GetPostTableId(tid) : "";
        }

        /// <summary>
        /// 得到用户帖子分表信息
        /// </summary>
        /// <returns>分表记录集</returns>
        public static DataTable GetAllPostTableName()
        {
            return Discuz.Data.PostTables.GetAllPostTableName();
        }

        public static int GetMaxPostTableId()
        {
            return TypeConverter.ObjectToInt(GetAllPostTableName().Compute("Max(id)", ""));
        }

        /// <summary>
        /// 得到当前所用帖子表分表ID
        /// </summary>
        /// <returns>分表ID</returns>
        public static string GetPostTableId()
        {
            return Discuz.Data.PostTables.GetPostTableId();
        }

        /// <summary>
        /// 得到多个主题的帖子所在分表的ID
        /// </summary>
        /// <param name="tidList">主题ID列表</param>
        /// <returns>分表ID字符数组</returns>
        public static string[] GetPostTableIdArray(string tidList)
        {
            string id = "";
            if (!Utils.IsNumericList(tidList))
            {
                return null;
            }

            string[] reval = tidList.Split(',');
            int mintid = Utils.StrToInt(reval[0], 0);
            int maxtid = Utils.StrToInt(reval[0], 0);
            for (int i = 0; i < reval.Length; i++)
            {
                if (mintid > TypeConverter.StrToInt(reval[i]))
                    mintid = TypeConverter.StrToInt(reval[i]);

                if (maxtid < TypeConverter.StrToInt(reval[i]))
                    maxtid = TypeConverter.StrToInt(reval[i]);
            }

            DataTable dt = Discuz.Data.PostTables.GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select("mintid<=" + maxtid.ToString() + " AND (maxtid<=0 OR maxtid>=" + mintid.ToString() + ")");
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (!Utils.StrIsNullOrEmpty(id))
                        {
                            id = id + ",";
                        }
                        id = id + dr[dr.Length - 1]["id"];
                    }
                }
            }
            dt.Dispose();
            return id.Split(',');
        }

        /// <summary>
        /// 根据最大主题ID和最小主题ID得到帖子所在分表的ID
        /// </summary>
        /// <param name="minTid">最小主题ID</param>
        /// <param name="maxTid">最大主题ID</param>
        /// <returns>分表ID字符数组</returns>
        public static string[] GetPostTableIdArray(int minTid, int maxTid)
        {
            string id = "";
            if (minTid > maxTid)
            {
                return null;
            }

            DataTable dt = Discuz.Data.PostTables.GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("[mintid]<={0} AND ([maxtid]<=0 OR [maxtid]>={1})", maxTid.ToString(), minTid.ToString()));
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (!Utils.StrIsNullOrEmpty(id))
                        {
                            id = id + ",";
                        }
                        id = id + dr[i]["id"];
                    }
                }
            }
            dt.Dispose();
            return id.Split(',');
        }

        private static object lockHelper = new object();     
        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postInfo">帖子信息类</param>
        /// <returns>返回帖子id</returns>
        public static int CreatePost(PostInfo postInfo)
        {
            int pid = 0;
            lock (lockHelper)
            {
                pid = Data.Posts.CreatePost(postInfo, GetPostTableId(postInfo.Tid));
            }
            //本帖具有正反方立场
            if (postInfo.Debateopinion > 0)
            {
                DebatePostExpandInfo dpei = new DebatePostExpandInfo();
                dpei.Tid = postInfo.Tid;
                dpei.Pid = pid;
                dpei.Opinion = postInfo.Debateopinion;
                dpei.Diggs = 0;
                Data.Debates.CreateDebateExpandInfo(dpei);
            }

            //将数据同步到sphinx增量表中
            if (pid > 0 && EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Sphinxconfig.Enable)
            {
                GetSphinxSqlService().CreatePost(GetPostTableName(), pid, postInfo.Tid, postInfo.Fid, postInfo.Posterid, postInfo.Postdatetime, postInfo.Title, postInfo.Message);
            }
            return pid;
        }

        #region sphinx SQL服务
        private static SphinxConfig.ISqlService sphinxSqlService;

        private static SphinxConfig.ISqlService GetSphinxSqlService()
        {
            if (sphinxSqlService == null)
            {
                try
                {
                    sphinxSqlService = (SphinxConfig.ISqlService)Activator.CreateInstance(Type.GetType("Discuz.EntLib.SphinxClient.SphinxSqlService, Discuz.EntLib", false, true));
                }
                catch
                {
                    throw new Exception("请检查BIN目录下有无Discuz.EntLib.dll文件");
                }
            }
            return sphinxSqlService;
        }
        #endregion


        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="postsInfo">帖子信息</param>
        /// <returns>更新数量</returns>
        public static int UpdatePost(PostInfo postInfo)
        {
            if (postInfo == null || postInfo.Pid < 1)
                return 0;

            //将数据同步到sphinx增量表中
            if (postInfo.Pid > 0 && EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Sphinxconfig.Enable)
            {
                GetSphinxSqlService().UpdatePost(GetPostTableName(), postInfo.Pid, postInfo.Tid, postInfo.Fid, postInfo.Posterid, postInfo.Postdatetime, postInfo.Title, postInfo.Message);
            }

            return Data.Posts.UpdatePost(postInfo, GetPostTableId(postInfo.Tid));
        }

        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="postTableId">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="reserveAttach">保留附件</param>
        /// <param name="chanagePostStatistic">是否更新帖子数量统计</param>
        /// <returns>删除数量</returns>
        public static int DeletePost(string postTableId, int pid, bool reserveAttach, bool chanagePostStatistic)
        {
            //TODO:需要删除dnt_postdebatefields表中的垃圾数据
            if (!reserveAttach)
            {
                //删除附件 
                Attachments.DeleteAttachmentByPid(pid);
            }
            Data.RateLogs.DeleteRateLog(pid);

            GeneralConfigInfo configinfo = GeneralConfigs.GetConfig();
            PostInfo postInfo = Data.Posts.GetPostInfo(postTableId, pid);
            //后台设置的项为多少天外的老帖删除不减积分，而不是多少天内删帖可以不减分
            //float[] creditsValue = Forums.GetValues(Forums.GetForumInfo(postInfo.Fid).Postcredits);
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (config.Losslessdel == 0 || Utils.StrDateDiffHours(postInfo.Postdatetime, config.Losslessdel * 24) < 0)
            {
                //获取版块积分规则
                float[] creditsValue = Forums.GetValues(Forums.GetForumInfo(postInfo.Fid).Replycredits);
                //如果未定义版块积分规则
                if (creditsValue == null)
                    creditsValue = Scoresets.GetUserExtCredits(CreditsOperationType.PostReply);
                UserCredits.UpdateUserExtCredits(postInfo.Posterid, creditsValue, 1, CreditsOperationType.PostReply, -1, true);
                UserCredits.UpdateUserCredits(postInfo.Posterid); 
            }

            return Data.Posts.DeletePost(postTableId, pid, chanagePostStatistic);
        }


        /// <summary>
        /// 获得指定的帖子描述信息
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pid">帖子id</param>
        /// <returns>帖子描述信息</returns>
        public static PostInfo GetPostInfo(int tid, int pid)
        {
            if (tid < 1)
                return null;

            return Data.Posts.GetPostInfo(GetPostTableId(tid), pid);
        }

        /// <summary>
        /// 获取需要审核的回复
        /// </summary>
        /// <param name="fidList">版块ID列表</param>
        /// <param name="ppp">每页帖子数</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="postTableId">分表</param>
        /// <param name="filter">可见性过滤器</param>
        /// <returns></returns>
        public static List<PostInfo> GetUnauditPost(string fidList, int ppp, int pageIndex, int postTableId, int filter)
        {
            if (!Utils.IsNumericList(fidList) || postTableId < 1 || pageIndex < 1)
                return null;

            List<PostInfo> list = Data.Posts.GetUnauditPost(fidList, ppp, pageIndex, postTableId, filter);
            if (list != null)
            {
                foreach (PostInfo postInfo in list)
                {
                    ForumInfo forumInfo = Forums.GetForumInfo(postInfo.Fid);
                    if (forumInfo != null)
                        postInfo.Forumname = forumInfo.Name;
                    TopicInfo topicInfo = Topics.GetTopicInfo(postInfo.Tid);
                    if (topicInfo != null)
                        postInfo.Topictitle = topicInfo.Title;
                }
            }
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
            if (!Utils.IsNumericList(fidList) || postTableId < 1)
                return 0;

            return Data.Posts.GetUnauditNewPostCount(fidList, postTableId, filter);
        }

        /// <summary>
        /// 获得指定主题的帖子列表
        /// </summary>
        /// <param name="tidList">主题ID列表</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPostList(string tidList)
        {
            if (!Utils.IsNumericList(tidList))
                return null;

            string[] postTableIdArray = GetPostTableIdArray(tidList);
            if (postTableIdArray == null || postTableIdArray.Length < 1)
                return null;

            return Data.Posts.GetPostDataTable(tidList, postTableIdArray);
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="Tid">主题ID</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static DataTable GetPostListTitle(int Tid)
        {
            return Tid > 0 ? DatabaseProvider.GetInstance().GetPostListTitle(Tid, PostTables.GetPostTableName(Tid)) : new DataTable();
        }

        /// <summary>
        /// 获取被包含在[hide]标签内的附件id
        /// </summary>
        /// <param name="content">帖子内容</param>
        /// <param name="hide">隐藏标记</param>
        /// <returns>隐藏的附件id数组</returns>
        public static string[] GetHiddenAttachIdList(string content, int hide)
        {
            if (hide == 0)
                return new string[0];

            StringBuilder tmpStr = new StringBuilder();
            StringBuilder hidContent = new StringBuilder();
            foreach (Match m in regexHide.Matches(content))
            {
                if (hide == 1)
                    hidContent.Append(m.Groups[0].ToString());
            }

            foreach (Match ma in regexAttach.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            foreach (Match ma in regexAttachImg.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            if (tmpStr.Length == 0)
                return new string[0];

            return tmpStr.Remove(tmpStr.Length - 1, 1).ToString().Split(',');
        }


        /// <summary>
        /// 获取指定tid的帖子DataTable
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子DataTable</returns>
        public static DataTable GetPostTree(int tid, int hide, int userCredit)
        {
            DataTable dt = Data.Posts.GetPostTree(tid);

            dt.Columns.Add("spaces", Type.GetType("System.String"));
            foreach (DataRow dr in dt.Rows)
            {
                dr["spaces"] = Utils.GetSpacesString(Utils.StrToInt(dr["layer"].ToString(), 0));
                if (hide == -1)
                    dr["message"] = Utils.CutString(Utils.HtmlEncode(dr["message"].ToString()), 0, 50);
                else
                    dr["message"] = UBB.HideDetail(Utils.CutString(Utils.HtmlEncode(dr["message"].ToString()), 0, 50), hide, userCredit);
                if (!dr["message"].Equals(""))
                    dr["title"] = dr["message"];
            }
            return dt;
        }

        /// <summary>
        /// 获得指定主题的第一个帖子的id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>帖子id</returns>
        public static int GetFirstPostId(int tid)
        {
            return tid > 0 ? Data.Posts.GetFirstPostId(tid) : 0;
        }

        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>是否是指定主题的回复者</returns>
        public static bool IsReplier(int tid, int uid)
        {
            return (tid > 0 && uid > 0) ? Data.Posts.IsReplier(tid, uid) : false;
        }


        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        public static int UpdatePostRateTimes(int tid, string postidlist)
        {
            if (!Utils.IsNumericList(postidlist))
            {
                return 0;
            }
            return Data.Posts.UpdatePostRateTimes(tid, postidlist);
        }

        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <returns>帖子评分列表</returns>
        public static List<RateLogInfo> GetPostRateLogList(int pid)
        {
            return pid > 0 ? Data.RateLogs.GetPostRateLogList(pid) : null;
        }

        /// <summary>
        /// 获取分页帖子评分列表
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<RateLogInfo> GetPostRateLogList(int pid, int pageIndex, int pageSize)
        {
            if (pid <= 0)
                return null;
            if (pageIndex <= 0)
                pageIndex = 1;
            if (pageSize <= 0)
                pageSize = 10;

            return Data.RateLogs.GetPostRateLogList(pid, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取评分数
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static int GetPostRateLogCount(int pid)
        {
            if (pid <= 0)
                return 0;
            return Data.RateLogs.GetPostRateLogCount(pid);
        }

        /// <summary>
        /// 得到空间格式的帖子内容
        /// </summary>
        /// <param name="postinfo">帖子描述</param>
        /// <param name="attArray">附件集合</param>
        /// <returns>空间格式</returns>
        public static string GetPostMessageHTML(PostInfo postInfo, AttachmentInfo[] attachmentArray)
        {
            string message = "";
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            //处理帖子内容
            postpramsInfo.Smileyoff = postInfo.Smileyoff;
            postpramsInfo.Bbcodeoff = postInfo.Bbcodeoff;
            postpramsInfo.Parseurloff = postInfo.Parseurloff;
            postpramsInfo.Allowhtml = postInfo.Htmlon;
            postpramsInfo.Sdetail = postInfo.Message;
            postpramsInfo.Showimages = 1 - postInfo.Smileyoff;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Pid = postInfo.Pid;
            //强制隐藏hide内容
            postpramsInfo.Hide = 1;
            //设定这是为个人空间进行的解析
            postpramsInfo.Isforspace = 1;

            //先简单判断是否是动网兼容模式
            if (!postpramsInfo.Ubbmode)
                message = UBB.UBBToHTML(postpramsInfo);
            else
                message = Utils.HtmlEncode(postInfo.Message);

            if (postpramsInfo.Jammer == 1)
                message = ForumUtils.AddJammer(postInfo.Message);

            if (postInfo.Attachment > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
            {
                //获取在[hide]标签中的附件id
                string[] attHidArray = GetHiddenAttachIdList(postpramsInfo.Sdetail, postpramsInfo.Hide);

                ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();
                info.Posterid = postInfo.Posterid;
                info.Pid = postInfo.Pid;

                for (int i = 0; i < attachmentArray.Length; i++)
                {
                    ShowtopicPageAttachmentInfo sAtt = new ShowtopicPageAttachmentInfo();
                    sAtt.Aid = attachmentArray[i].Aid;
                    sAtt.Attachment = attachmentArray[i].Attachment;
                    sAtt.Description = attachmentArray[i].Description;
                    sAtt.Downloads = attachmentArray[i].Downloads;
                    sAtt.Filename = attachmentArray[i].Filename;
                    sAtt.Filesize = attachmentArray[i].Filesize;
                    sAtt.Filetype = attachmentArray[i].Filetype;
                    sAtt.Pid = attachmentArray[i].Pid;
                    sAtt.Postdatetime = attachmentArray[i].Postdatetime;
                    sAtt.Readperm = attachmentArray[i].Readperm;
                    sAtt.Sys_index = attachmentArray[i].Sys_index;
                    sAtt.Sys_noupload = attachmentArray[i].Sys_noupload;
                    sAtt.Tid = attachmentArray[i].Tid;
                    sAtt.Uid = attachmentArray[i].Uid;
                    message = Attachments.GetMessageWithAttachInfo(postpramsInfo, 1, attHidArray, info, sAtt, message);
                }
            }
            return message;
        }

        /// <summary>
        /// 加载单个附件实体对象
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ShowtopicPageAttachmentInfo LoadSingleAttachmentInfo(IDataReader reader)
        {
            ShowtopicPageAttachmentInfo attInfo = new ShowtopicPageAttachmentInfo();
            attInfo.Aid = TypeConverter.ObjectToInt(reader["aid"]);
            attInfo.Tid = TypeConverter.ObjectToInt(reader["tid"]);
            attInfo.Pid = TypeConverter.ObjectToInt(reader["pid"]);
            attInfo.Postdatetime = reader["postdatetime"].ToString();
            attInfo.Readperm = TypeConverter.ObjectToInt(reader["readperm"]);
            attInfo.Filename = reader["filename"].ToString();
            attInfo.Description = reader["description"].ToString();
            attInfo.Filetype = reader["filetype"].ToString();
            attInfo.Filesize = TypeConverter.ObjectToInt(reader["filesize"]);
            attInfo.Attachment = reader["attachment"].ToString();
            attInfo.Downloads = TypeConverter.ObjectToInt(reader["downloads"]);
            attInfo.Attachprice = TypeConverter.ObjectToInt(reader["attachprice"]);
            attInfo.Uid = TypeConverter.ObjectToInt(reader["uid"]);
            return attInfo;
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachList, bool isModer)
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            List<ShowtopicPagePostInfo> postList = Data.Posts.GetPostList(postpramsInfo);
            attachList = new List<ShowtopicPageAttachmentInfo>();

            if (postList.Count == 0)
                return postList;

            int adCount = Advertisements.GetInPostAdCount("", postpramsInfo.Fid);

            foreach (ShowtopicPagePostInfo postInfo in postList)
            {
                LoadExtraPostInfo(postInfo, adCount);
            }

            //进行相应帖子信息设置
            string pidList = GetPidListWithAttach(postList);
            attachList = Attachments.GetAttachmentList(postpramsInfo, pidList);
            ParsePostListExtraInfo(postpramsInfo, attachList, isModer, postList);

            return postList;
        }

        /// <summary>
        /// 解析帖子列表附加信息及内容
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="attachList">附件列表</param>
        /// <param name="isModer">是否为版主</param>
        /// <param name="postList">帖子列表</param>
        public static void ParsePostListExtraInfo(PostpramsInfo postpramsInfo, List<ShowtopicPageAttachmentInfo> attachList, bool isModer, List<ShowtopicPagePostInfo> postList)
        {
            int originalHideStatus = postpramsInfo.Hide;
            // 计算是否允许查看附件
            int allowGetAttach = GetAllowGetAttachValue(postpramsInfo);

            #region 计算辩论帖是否被顶过
            string diggedPidList = string.Empty;
            TopicInfo topicInfo = postpramsInfo.Topicinfo == null ? Topics.GetTopicInfo(postpramsInfo.Tid) : postpramsInfo.Topicinfo;
            if (topicInfo.Special == 4 && UserGroups.GetUserGroupInfo(7).Allowdiggs != 1)
            {
                diggedPidList = Discuz.Data.Debates.GetUesrDiggs(postpramsInfo.Tid, postpramsInfo.CurrentUserid);
            }
            #endregion

            //UserInfo userInfo = Users.GetUserInfo(postpramsInfo.CurrentUserid);
            //postpramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;

            foreach (ShowtopicPagePostInfo postInfo in postList)
            {
                LoadPostMessage(postpramsInfo, attachList, isModer, allowGetAttach, originalHideStatus, postInfo);

                if (topicInfo.Special == 4)
                {
                    if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 1)
                        postInfo.Digged = Debates.IsDigged(postInfo.Pid, postpramsInfo.CurrentUserid);
                    else
                        postInfo.Digged = Utils.InArray(postInfo.Pid.ToString(), diggedPidList); //diggslist.Contains(reader["pid"].ToString());
                }
            }
        }

        /// <summary>
        /// 从post列表中计算出还有附件的pid列表字符串
        /// </summary>
        /// <param name="postList"></param>
        /// <returns></returns>
        public static string GetPidListWithAttach(List<ShowtopicPagePostInfo> postList)
        {
            StringBuilder pidList = new StringBuilder(",");
            for (int i = 0; i < postList.Count; i++)
            {
                if (postList[i].Attachment > 0)
                {
                    pidList.Append(postList[i].Pid);
                    pidList.Append(",");
                }
            }
            return pidList.ToString().Trim(',');
        }

        /// <summary>
        /// 计算是否允许查看附件
        /// </summary>
        /// <param name="postpramsInfo"></param>
        /// <returns></returns>
        private static int GetAllowGetAttachValue(PostpramsInfo postpramsInfo)
        {
            if (Forums.AllowGetAttachByUserID(Forums.GetForumInfo(postpramsInfo.Fid).Permuserlist, postpramsInfo.CurrentUserid))
                return 1;

            int allowGetAttach = 0;

            if (postpramsInfo.Getattachperm.Equals("") || postpramsInfo.Getattachperm == null)
                allowGetAttach = postpramsInfo.CurrentUserGroup.Allowgetattach;
            else if (Forums.AllowGetAttach(postpramsInfo.Getattachperm, postpramsInfo.Usergroupid))
                allowGetAttach = 1;

            return allowGetAttach;
        }

        /// <summary>
        /// 加载帖子扩展信息
        /// </summary>
        /// <param name="postInfo">帖子信息</param>
        /// <param name="adCount">帖内广告个数</param>
        private static void LoadExtraPostInfo(ShowtopicPagePostInfo postInfo, int adCount)
        {
            UserGroupInfo tmpGroupInfo = UserGroups.GetUserGroupInfo(postInfo.Groupid);
            string[] postCustomFloorNameArray = Utils.SplitString(GeneralConfigs.GetConfig().Postnocustom, "\n");
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            // 勋章
            if (postInfo.Medals != string.Empty)
                postInfo.Medals = Caches.GetMedalsList(postInfo.Medals);

            postInfo.Stars = tmpGroupInfo.Stars;
            if (tmpGroupInfo.Color.Equals(""))
                postInfo.Status = tmpGroupInfo.Grouptitle;
            else
                postInfo.Status = string.Format("<span style=\"color:{0}\">{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);

            if (postCustomFloorNameArray.Length >= postInfo.Id)
                postInfo.Postnocustom = postCustomFloorNameArray[postInfo.Id - 1];

            postInfo.Adindex = random.Next(0, adCount);
        }

        /// <summary>
        /// 根据附件加载帖子内容
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="attachList">附件列表</param>
        /// <param name="isModer">是否是管理人员</param>
        /// <param name="allowGetAttach">是否允许获取附件</param>
        /// <param name="originalHideStatus">帖子原始Hide属性</param>
        /// <param name="postInfo">帖子信息 </param>
        private static void LoadPostMessage(PostpramsInfo postpramsInfo, List<ShowtopicPageAttachmentInfo> attachList, bool isModer, int allowGetAttach, int originalHideStatus, ShowtopicPagePostInfo postInfo)
        {
            UserGroupInfo tmpGroupInfo;

            bool showMessage = !Utils.InArray(postInfo.Groupid.ToString(), "4,5,6") && postInfo.Invisible == 0;

            //如果当前帖子不可见，但是查看用户是管理组时
            if (!showMessage && isModer)
            {
                postInfo.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + postInfo.Message;
            }
            else if (!showMessage)
            {
                postInfo.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽</div>";
                List<ShowtopicPageAttachmentInfo> delattlist = new List<ShowtopicPageAttachmentInfo>();
                foreach (ShowtopicPageAttachmentInfo attach in attachList)
                {
                    if (attach.Pid == postInfo.Pid)
                        delattlist.Add(attach);
                }

                foreach (ShowtopicPageAttachmentInfo attach in delattlist)
                {
                    attachList.Remove(attach);
                }
            }

            //如果当前帖子可见 或者 当前查看用户是管理组时
            if (showMessage || isModer)
            {
                //处理帖子内容
                postpramsInfo.Smileyoff = postInfo.Smileyoff;
                postpramsInfo.Bbcodeoff = postInfo.Bbcodeoff;
                postpramsInfo.Parseurloff = postInfo.Parseurloff;
                postpramsInfo.Allowhtml = postInfo.Htmlon;
                postpramsInfo.Sdetail = postInfo.Message;
                postpramsInfo.Pid = postInfo.Pid;
                //校正hide处理
                tmpGroupInfo = UserGroups.GetUserGroupInfo(postInfo.Groupid);
                if (tmpGroupInfo.Allowhidecode == 0)
                    postpramsInfo.Hide = 0;

                //先简单判断是否是动网兼容模式
                if (!postpramsInfo.Ubbmode)
                    postInfo.Message = UBB.UBBToHTML(postpramsInfo);
                else
                    postInfo.Message = Utils.HtmlEncode(postInfo.Message);

                if (postpramsInfo.Jammer == 1)
                    postInfo.Message = ForumUtils.AddJammer(postInfo.Message);

                string message = postInfo.Message;
                if (postInfo.Attachment > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
                {
                    //获取在[hide]标签中的附件id
                    string[] attHidArray = GetHiddenAttachIdList(postpramsInfo.Sdetail, postpramsInfo.Hide);
                    List<ShowtopicPageAttachmentInfo> attachDeleteList = new List<ShowtopicPageAttachmentInfo>();
                    foreach (ShowtopicPageAttachmentInfo attach in attachList)
                    {
                        message = Attachments.GetMessageWithAttachInfo(postpramsInfo, allowGetAttach, attHidArray, postInfo, attach, message);
                        //if (attach.Inserted == 1 && (postpramsInfo.CurrentUserGroup.Radminid == 1 || attach.Uid == postpramsInfo.CurrentUserid || attach.Attachprice <= 0 || attach.Isbought == 1)//当为发帖人或不为收费附件或已购买该收费附件时
                        //    || Utils.InArray(attach.Aid.ToString(), attHidArray))

                        //如果附件已经插入到帖子内容中，则显示内容控制完全交给GetMessageWithAttachInfo
                        if ((attach.Inserted == 1 || Utils.InArray(attach.Aid.ToString(), attHidArray)))
                            attachDeleteList.Add(attach);
                    }

                    foreach (ShowtopicPageAttachmentInfo attach in attachDeleteList)
                    {
                        attachList.Remove(attach);
                    }
                    postInfo.Message = message;
                }

                //恢复hide初值
                postpramsInfo.Hide = originalHideStatus;
            }
        }

        /// <summary>
        /// 通过主题ID得到主帖内容,此方法可继续扩展
        /// </summary>
        /// <param name="tid"></param>
        /// <returns>PostInfo</returns>
        public static PostInfo GetTopicPostInfo(int tid)
        {
            return tid > 0 ? Data.Posts.GetTopicPostInfo(tid) : null;
        }


        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public static ShowtopicPagePostInfo GetSinglePost(PostpramsInfo postPramsInfo, out Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo> attachmentList, bool ismoder)
        {
            //此代码已被移至showdebate.aspx.cs页面
            //UserInfo userInfo = Users.GetUserInfo(postPramsInfo.CurrentUserid);
            //postPramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;

            ShowtopicPagePostInfo postInfo = Discuz.Data.Posts.GetPostInfoWithAttachments(postPramsInfo, out attachmentList);
            if (postInfo != null)
            {
                int allowGetAttach = GetAllowGetAttachValue(postPramsInfo);
                Attachments.CheckPurchasedAttachments(attachmentList, postPramsInfo.CurrentUserid);
                int adcount = Advertisements.GetInPostAdCount("", postInfo.Fid);
                postInfo.Id = 1;
                LoadExtraPostInfo(postInfo, adcount);
                LoadPostMessage(postPramsInfo, attachmentList, ismoder, allowGetAttach, postPramsInfo.Hide, postInfo);
            }
            return postInfo;
        }


        /// <summary>
        /// 获取悬赏帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <param name="attachmentList">输出附件列表</param>
        /// <param name="isModer">当前是否为管理人员</param>
        /// <returns></returns>
        public static List<ShowbonusPagePostInfo> GetPostListWithBonus(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentList, bool isModer)
        {
            List<ShowtopicPagePostInfo> postList = GetPostList(postpramsInfo, out attachmentList, isModer);

            List<ShowbonusPagePostInfo> bonusPostList = new List<ShowbonusPagePostInfo>();

            Dictionary<int, BonusLogInfo> bonusDetails = Discuz.Data.Bonus.GetLogsForEachPost(postpramsInfo.Tid);
            foreach (ShowtopicPagePostInfo sppi in postList)
            {
                ShowbonusPagePostInfo item = new ShowbonusPagePostInfo();

                #region 属性赋值
                item.Adindex = sppi.Adindex;
                item.Attachment = sppi.Attachment;
                item.Avatar = sppi.Avatar;
                item.Avatarheight = sppi.Avatarheight;
                item.Avatarwidth = sppi.Avatarwidth;
                item.Bbcodeoff = sppi.Bbcodeoff;
                item.Bday = sppi.Bday;
                item.Credits = sppi.Credits;
                item.Customstatus = sppi.Customstatus;
                item.Digestposts = sppi.Digestposts;
                item.Email = sppi.Email;
                item.Extcredits1 = sppi.Extcredits1;
                item.Extcredits2 = sppi.Extcredits2;
                item.Extcredits3 = sppi.Extcredits3;
                item.Extcredits4 = sppi.Extcredits4;
                item.Extcredits5 = sppi.Extcredits5;
                item.Extcredits6 = sppi.Extcredits6;
                item.Extcredits7 = sppi.Extcredits7;
                item.Extcredits8 = sppi.Extcredits8;
                item.Fid = sppi.Fid;
                item.Gender = sppi.Gender;
                item.Groupid = sppi.Groupid;
                item.Htmlon = sppi.Htmlon;
                item.Icq = sppi.Icq;
                item.Id = sppi.Id;
                item.Invisible = sppi.Invisible;
                item.Ip = sppi.Ip;
                item.Joindate = sppi.Joindate;
                item.Lastactivity = sppi.Lastactivity;
                item.Lastedit = sppi.Lastedit;
                item.Layer = sppi.Layer;
                item.Location = sppi.Location;
                item.Medals = sppi.Medals;
                item.Message = sppi.Message;
                item.Msn = sppi.Msn;
                item.Nickname = sppi.Nickname;
                item.Onlinestate = sppi.Onlinestate;
                item.Parseurloff = sppi.Parseurloff;
                item.Pid = sppi.Pid;
                item.Postdatetime = sppi.Postdatetime;
                item.Poster = sppi.Poster;
                item.Posterid = sppi.Posterid;
                item.Posts = sppi.Posts;
                item.Qq = sppi.Qq;
                item.Rate = sppi.Rate;
                item.Ratetimes = sppi.Ratetimes;
                item.Showemail = sppi.Showemail;
                item.Signature = sppi.Signature;
                item.Skype = sppi.Skype;
                item.Smileyoff = sppi.Smileyoff;
                item.Spaceid = sppi.Spaceid;
                item.Stars = sppi.Stars;
                item.Status = sppi.Status;
                item.Title = sppi.Title;
                item.Userinvisible = sppi.Userinvisible;
                item.Username = sppi.Username;
                item.Usesig = sppi.Usesig;
                item.Website = sppi.Website;
                item.Yahoo = sppi.Yahoo;

                if (bonusDetails.ContainsKey(item.Pid))
                {
                    item.Bonus = bonusDetails[item.Pid].Bonus;
                    item.Bonusextid = bonusDetails[item.Pid].Extid;
                    item.Isbest = bonusDetails[item.Pid].Isbest;
                }
                #endregion

                if (bonusDetails.ContainsKey(item.Pid) || item.Layer == 0)
                    bonusPostList.Add(item);
            }
            return bonusPostList;
        }

        /// <summary>
        /// 屏蔽帖子内容
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postList">帖子ID</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        public static bool BanPosts(int tid, string postList, int invisible)
        {
            if (invisible != -1 && (invisible == 0 || invisible == -2))
            {
                Data.Posts.BanPosts(GetPostTableId(tid), postList, invisible);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回帖子列表
        /// </summary>
        /// <param name="postList">帖子ID</param>
        /// <param name="tid">主题ID</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPostList(string postList, string tid)
        {
            //TODO:考虑此方法是否有其它方法可代替
            if (!Utils.IsNumericArray(postList.Split(',')))
                return null;

            string tableId = GetPostTableId(TypeConverter.StrToInt(tid, -1));
            return Data.Posts.GetPostList(postList, tableId);
        }

        /// <summary>
        /// 检查帖子标题与内容中是否有广告
        /// </summary>
        /// <param name="regular">验证广告正则</param>
        /// <param name="title">帖子标题</param>
        /// <param name="message">帖子内容</param>
        /// <returns>帖子标题与内容中是否有广告</returns>
        public static bool IsAD(string regular, string title, string message)
        {
            if (regular.Trim() == "")
                return false;

            return (Regex.IsMatch(title, regular) || Regex.IsMatch(ForumUtils.RemoveSpecialChars(message, GeneralConfigs.GetConfig().Antispamreplacement), regular));
        }

        /// <summary>
        /// 获得广告信息的关键词
        /// </summary>
        /// <param name="regular"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetADKeywords(string regular, string title, string message)
        {
            Regex r = new Regex(regular);
            StringBuilder builder = new StringBuilder();
            Match m;
            for (m = r.Match(title); m.Success; m = m.NextMatch())
            {
                builder.Append(" ");
                builder.Append(m.Groups[0].ToString());
            }
            for (m = r.Match(message); m.Success; m = m.NextMatch())
            {
                builder.Append(" ");
                builder.Append(m.Groups[0].ToString());
            }

            return builder.ToString();

        }

        /// <summary>
        /// 通过待验证的帖子
        /// </summary>
        /// <param name="postTableId"></param>
        /// <param name="pidlist"></param>
        public static void AuditPost(int postTableId, string validate, string delete, string ignore, string fidlist)
        {
            Data.Posts.AuditPost(postTableId, validate, delete, ignore, fidlist);

            //为用户增加发帖的积分
            if (!string.IsNullOrEmpty(validate))
            {
                foreach (DataRow postInfo in GetPostListByCondition(BaseConfigs.GetTablePrefix + "posts" + postTableId, "[pid] IN (" + validate + ")").Rows)
                {
                    ForumInfo forumInfo = Forums.GetForumInfo(TypeConverter.ObjectToInt(postInfo["fid"]));//获取主题的版块信息

                    float[] forumReplycredits = Forums.GetValues(forumInfo.Replycredits);
                    if (forumReplycredits != null) //使用版块内积分
                        UserCredits.UpdateUserCreditsByPosts(TypeConverter.ObjectToInt(postInfo["posterid"]), forumReplycredits);
                    else //使用默认积分
                        UserCredits.UpdateUserCreditsByPosts(TypeConverter.ObjectToInt(postInfo["posterid"]));
                }
            }
        }

        /// <summary>
        /// 获取版主是否有权限管理的帖子列表中的帖子
        /// </summary>
        /// <param name="moderatorUserName">版主名称</param>
        /// <param name="postTableId">分表ID</param>
        /// <param name="pidList">帖子ID列表</param>
        /// <returns></returns>
        public static bool GetModPostCountByPidList(string moderatorUserName, string postTableId, string pidList)
        {
            string fidList = Moderators.GetFidListByModerator(moderatorUserName);
            if (fidList == "")
                return false;
            return pidList.Split(',').Length == Data.Posts.GetModPostCountByPidList(fidList, postTableId, pidList);
        }

        /// <summary>
        /// 获得指定用户回复指定主题次数
        /// </summary>
        /// <param name="topicId">主题Id</param>
        /// <param name="posterId">用户Id</param>
        /// <returns>回复次数</returns>
        public static int GetPostCountByPosterId(string onlyauthor, int topicId, int posterId, int replies)
        {
            if (onlyauthor == "" || onlyauthor == "0")
                return replies + 1;
            else
                return Data.Posts.GetPostCountByPosterId(Posts.GetPostTableId(topicId), topicId, posterId);
        }

        /// <summary>
        /// 获得最新分表名称
        /// </summary>
        /// <returns></returns>
        public static string GetPostTableName()
        {
            return PostTables.GetPostTableName();
        }

        /// <summary>
        /// 获得最新分表名称
        /// </summary>
        /// <param name="topicId">主题ID</param>
        /// <returns></returns>
        public static string GetPostPramsInfoCondition(string onlyauthor, int topicid, int posterid)
        {
            if (!(Utils.StrIsNullOrEmpty(onlyauthor) || onlyauthor.Equals("0")))
                return string.Format(" {0}.posterid={1}", PostTables.GetPostTableName(topicid), posterid);
            else
                return "";
        }

        /// <summary>
        /// 获取指定分表的帖数
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static int GetPostTableCount(string tableName)
        {
            return (PostTables.GetPostTableCount(tableName) / 10000) + 1;
        }

        /// <summary>
        /// 获取指定主题下小于pid的有效帖子数
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <param name="tid">主题ID</param>
        /// <returns></returns>
        public static int GetPostCountBeforePid(int pid, int tid)
        {
            if (tid <= 0 || pid <= 0)
                return 0;
            return Data.Posts.GetPostCountBeforePid(pid, tid);
        }

        /// <summary>
        /// 获取分表列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPostTableList()
        {
            return PostTables.GetPostTableList();
        }

        /// <summary>
        /// 更新分表描述
        /// </summary>
        /// <param name="detachTableId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static int UpdateDetachTable(int detachTableId, string description)
        {
            return PostTables.UpdateDetachTable(detachTableId, description);
        }

        /// <summary>
        /// 更新当前表中最大ID的记录用的最大和最小tid字段		
        /// </summary>
        /// <param name="posttablename"></param>
        /// <param name="tablelistmaxid"></param>
        public static void UpdateMinMaxField()
        {
            string tableprefix = BaseConfigs.GetTablePrefix + "posts";
            foreach (DataRow dr in Posts.GetAllPostTable().Rows)
            {
                //对除当前表之外的帖子表进行统计
                if (Posts.GetPostTableName() != (tableprefix + dr["id"].ToString()))
                {
                    //更新当前表中最大ID的记录用的最大和最小tid字段		
                    DatabaseProvider.GetInstance().UpdateMinMaxField(tableprefix + dr["id"].ToString(), Utils.StrToInt(dr["id"], 0));
                }
            }
        }

        public static bool UpdateMinMaxField(int tablelistmaxid)
        {
            //更新当前表中最大ID的记录用的最大和最小tid字段		
            if (tablelistmaxid > 0 && tablelistmaxid < 213) //表值总数不能大于213
            {
                PostTables.UpdateMinMaxField(BaseConfigs.GetTablePrefix + "posts" + tablelistmaxid, tablelistmaxid);
                return true;
            }
            return false;
        }

        public static void AddPostTableToTableList(string description, int posttablename)
        {
            PostTables.AddPostTableToTableList(description, posttablename);
        }

        public static int GetMaxPostTableTid(string posttabelname)
        {
            return PostTables.GetMaxPostTableTid(posttabelname);
        }

        public static DataTable GetMaxTid()
        {
            return PostTables.GetMaxTid();
        }

        public static DataTable GetPostCountFromIndex(string postsid)
        {
            return Data.Posts.GetPostCountFromIndex(postsid);
        }

        public static DataTable GetPostCountTable(string postsid)
        {
            return Data.Posts.GetPostCountTable(postsid);
        }

        /// <summary>
        /// 创建存储过程
        /// </summary>
        /// <param name="tablelistmaxid"></param>
        public static void CreateStoreProc(int tablelistmaxid)
        {
            if (Databases.IsStoreProc())
                PostTables.CreateStoreProc(tablelistmaxid);
        }

        public static void CreateORFillIndex(string DbName, string postid)
        {
            PostTables.CreateORFillIndex(DbName, postid);
        }

        /// <summary>
        /// 获取全部分表信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllPostTable()
        {
            return Data.PostTables.GetAllPostTableName();
        }

        public static void ResetPostTables()
        {
            Data.PostTables.ResetPostTables();
        }

        /// <summary>
        /// 根据主题ID列表取出主题帖子
        /// </summary>
        /// <param name="posttableid">分表ID</param>
        /// <param name="tidlist">主题ID列表</param>
        /// <returns></returns>
        public static void WriteAggregationPostData(PostInfo[] posts, string tablelist, string tidlist, string configPath, string topiclistnodepath, string websitetopiclistnodepath)
        {
            //得到所选择帖子信息
            DataTable dt = Data.Posts.GetTopicListByTidlist(tablelist, tidlist);
            Discuz.Common.Xml.XmlDocumentExtender doc = new Discuz.Common.Xml.XmlDocumentExtender();
            if (File.Exists(configPath))
                doc.Load(configPath);
            //清除以前选择
            XmlNode topiclistnode = doc.InitializeNode(topiclistnodepath, false);
            XmlNode oldTopicListNode = topiclistnode.Clone();   //复制一份到新节点
            topiclistnode.RemoveAll();      //清除新节点的内容，只存留其结构
            XmlNode websitetopiclistnode = doc.InitializeNode(websitetopiclistnodepath);

            string selecttidlist = DNTRequest.GetString("tid");
            foreach (string tid in tidlist.Split(','))
            {
                XmlNode topic = GetOldTopicNode(oldTopicListNode, tid);
                if (topic == null)
                {
                    topic = GetTopicInDataTable(posts, dt, doc, tid);
                }
                if (topic != null)
                    topiclistnode.AppendChild(topic);

            }
            foreach (XmlNode node in topiclistnode)
            {
                if (("," + selecttidlist + ",").IndexOf("," + node.ChildNodes[20].InnerText + ",") >= 0)
                    websitetopiclistnode.AppendChild(node.Clone());
            }
            doc.Save(configPath);
        }

        private static XmlNode GetTopicInDataTable(PostInfo[] posts, DataTable dt, Discuz.Common.Xml.XmlDocumentExtender doc, string tid)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["tid"].ToString() != tid)
                    continue;
                PostInfo newpost = null;
                foreach (PostInfo post in posts)
                {
                    if (post.Tid.ToString() == dr["tid"].ToString())
                        newpost = post;
                }
                //创建Topic节点
                XmlElement topic = doc.CreateElement("Topic");
                doc.AppendChildElementByDataRow(ref topic, dt.Columns, dr, "tid,message");
                doc.AppendChildElementByNameValue(ref topic, "topicid", dr["tid"].ToString());
                string tempubbstr = UBB.ClearUBB(dr["message"].ToString());
                if (tempubbstr.Length > 200)
                    tempubbstr = tempubbstr.Substring(0, 200) + "...";

                if (newpost != null)
                    tempubbstr = newpost.Message;

                doc.AppendChildElementByNameValue(ref topic, "shortdescription", tempubbstr, true);
                doc.AppendChildElementByNameValue(ref topic, "fulldescription", UBB.ClearUBB(dr["message"].ToString()), true);
                ForumInfo forumInfo = Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"]));
                if (forumInfo != null)
                {
                    doc.AppendChildElementByNameValue(ref topic, "forumname", forumInfo.Name);
                    doc.AppendChildElementByNameValue(ref topic, "forumnamerewritename", forumInfo.Rewritename);
                }
                else
                {
                    doc.AppendChildElementByNameValue(ref topic, "forumname", "");
                    doc.AppendChildElementByNameValue(ref topic, "forumnamerewritename", "");
                }
                return topic;
            }
            return null;
        }

        /// <summary>
        /// 从原有主题列表中选择已经有的主题
        /// </summary>
        /// <param name="oldTopicList"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        private static XmlNode GetOldTopicNode(XmlNode oldTopicList, string tid)
        {
            foreach (XmlNode topic in oldTopicList.ChildNodes)
            {
                if (topic.ChildNodes[20].InnerText.Trim() == tid)
                    return topic.Clone();
            }
            return null;
        }

        public static void WriteAggregationHotTopicsData(string tidlist, string configPath, string topiclistnodepath, string websitetopiclistnodepath)
        {
            //得到所选择帖子信息
            DataTable dt = Data.Topics.GetTopicList(tidlist, -1);
            Discuz.Common.Xml.XmlDocumentExtender doc = new Discuz.Common.Xml.XmlDocumentExtender();
            if (File.Exists(configPath))
                doc.Load(configPath);
            //清除以前选择
            XmlNode topiclistnode = doc.InitializeNode(topiclistnodepath);
            XmlNode websitetopiclistnode = doc.InitializeNode(websitetopiclistnodepath);
            foreach (string tid in tidlist.Split(','))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["tid"].ToString().Trim() != tid)
                        continue;
                    //创建Topic节点
                    XmlElement topic = doc.CreateElement("Topic");
                    doc.AppendChildElementByDataRow(ref topic, dt.Columns, dr, "");
                    ForumInfo forumInfo = Forums.GetForumInfo(TypeConverter.ObjectToInt(dr["fid"]));
                    if (forumInfo != null)
                    {
                        doc.AppendChildElementByNameValue(ref topic, "forumname", forumInfo.Name);
                        doc.AppendChildElementByNameValue(ref topic, "forumnamerewritename", forumInfo.Rewritename);
                    }
                    else
                    {
                        doc.AppendChildElementByNameValue(ref topic, "forumname", "");
                        doc.AppendChildElementByNameValue(ref topic, "forumnamerewritename", "");
                    };
                    topiclistnode.AppendChild(topic);
                    break;
                }
            }
            string selecttidlist = DNTRequest.GetString("tid");
            foreach (XmlNode node in topiclistnode)
            {
                if (("," + selecttidlist + ",").IndexOf("," + node.ChildNodes[0].InnerText + ",") >= 0)
                    websitetopiclistnode.AppendChild(node.Clone());
            }
            doc.Save(configPath);
        }


        public static DataTable GetPostInfo(bool istopic, int tid, int pid)
        {
            string posttablename = string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, Posts.GetPostTableId(tid));
            return istopic ? Data.Posts.GetMainPostInfo(posttablename, tid) : Data.Posts.GetPostInfoByPid(posttablename, pid);
        }


        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="postpramsInfo">参数对象</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetLastPostDataTable(PostpramsInfo postpramsInfo)
        {
            return GetPagedLastDataTable(postpramsInfo);
        }


        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPagedLastDataTable(PostpramsInfo postpramsInfo)
        {
            DataTable dt = Data.Posts.GetPagedLastPostDataTable(postpramsInfo);
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("adindex", Type.GetType("System.Int32"));
                return dt;
            }

            dt.Columns.Add("adindex", Type.GetType("System.Int32"));
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            int adcount = Advertisements.GetInPostAdCount("", postpramsInfo.Fid);

            foreach (DataRow dr in dt.Rows)
            {
                //　ubb转为html代码在页面显示
                postpramsInfo.Smileyoff = Utils.StrToInt(dr["smileyoff"], 0);
                postpramsInfo.Bbcodeoff = Utils.StrToInt(dr["bbcodeoff"], 0);
                postpramsInfo.Parseurloff = Utils.StrToInt(dr["parseurloff"], 0);
                postpramsInfo.Allowhtml = Utils.StrToInt(dr["htmlon"], 0);
                postpramsInfo.Pid = Utils.StrToInt(dr["pid"], 0);
                postpramsInfo.Sdetail = dr["message"].ToString();

                if (postpramsInfo.Price > 0 && Utils.StrToInt(dr["layer"], 0) == 0)
                    dr["message"] = string.Format("<div class=\"paystyle\">此帖为交易帖,要付 {0} <span class=\"bold\">{1}</span>{2} 才可查看</div>", Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans()).Name, postpramsInfo.Price, Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans()).Unit);
                else
                {
                    if (!postpramsInfo.Ubbmode)
                        dr["message"] = UBB.UBBToHTML(postpramsInfo);
                    else
                        dr["message"] = Utils.HtmlEncode(dr["message"].ToString());
                }
                dr["adindex"] = random.Next(0, adcount);

                //是不是加干扰码
                if (postpramsInfo.Jammer == 1)
                    dr["message"] = ForumUtils.AddJammer(dr["message"].ToString());

                //是不是隐藏会员email
                if (Utils.StrToInt(dr["showemail"], 0) == 1)
                    dr["email"] = "";
            }
            return dt;
        }

        public static string GetPostMessage(UserGroupInfo usergroupinfo, AdminGroupInfo adminGroupInfo, string postmessage, bool ishtmlon)
        {
            string message;
            if (adminGroupInfo != null && adminGroupInfo.Admingid == 1)
            {
                if (usergroupinfo.Allowhtml == 0)
                    message = Utils.HtmlEncode(postmessage);
                else
                    message = ishtmlon ? postmessage : Utils.HtmlEncode(postmessage);
            }
            else
            {
                if (usergroupinfo.Allowhtml == 0)
                    message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                else
                    message = ishtmlon ? ForumUtils.BanWordFilter(postmessage) : Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
            }
            return message;
        }

        /// <summary>
        /// 获取分表帖数
        /// </summary>
        /// <param name="postTableid">分表Id</param>
        /// <returns></returns>
        public static int GetPostsCount(string postTableid)
        {
            return Discuz.Data.Posts.GetPostsCount(postTableid);
        }

        /// <summary>
        /// 通过未审核的帖子
        /// </summary>
        /// <param name="currentPostTableId">当前表ID</param>
        /// <param name="pidlist">帖子ID列表</param>
        public static void PassPost(int currentPostTableId, string pidlist)
        {
            Discuz.Data.Posts.PassPost(currentPostTableId, pidlist);
        }

        /// <summary>
        /// 获取帖子登记
        /// </summary>
        /// <param name="currentPostTableId">分表ID</param>
        /// <param name="postid">帖子ID</param>
        /// <returns></returns>
        public static void GetPostLayer(int currentPostTableId)
        {
            DataTable dt = new DataTable();
            string pid = "";
            foreach (string idlist in DNTRequest.GetString("pid").Split(','))
            {
                pid = idlist.Split('|')[0];
                if (pid.Trim() != "")
                {
                    dt = Discuz.Data.Posts.GetPostLayer(currentPostTableId, int.Parse(pid));
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["layer"].ToString().Trim() == "0")
                            Discuz.Forum.TopicAdmins.DeleteTopics(dt.Rows[0]["tid"].ToString(), false);
                        else
                            Discuz.Forum.Posts.DeletePost(currentPostTableId.ToString(), Convert.ToInt32(pid), false, false);
                    }
                }
            }
        }

        /// <summary>
        /// 更新我的帖子
        /// </summary>
        public static void UpdateMyPost()
        {
            Discuz.Data.Posts.UpdateMyPost();
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
            return Data.Posts.GetTopicAuditCondition(fid, poster, title, moderatorName, postDateTimeStart,
                postDateTimeEnd, delDateTimeStart, delDateTimeEnd);
        }

        /// <summary>
        /// 是否存在满足条件的需要审核的帖子
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static bool IsExistAuditTopic(string condition)
        {
            return Data.Posts.IsExistAuditTopic(condition);
        }

        /// <summary>
        /// 按条件获取帖子列表
        /// </summary>
        /// <param name="postTableName">分表名称</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static DataTable GetPostListByCondition(string postTableName, string condition)
        {
            return Data.Posts.GetPostListByCondition(postTableName, condition);
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
        public static string SearchPost(int forumid, string posttableid, DateTime postdatetimeStart, DateTime postdatetimeEnd, string poster, bool lowerupper, string ip, string message)
        {
            return Discuz.Data.Posts.SearchPost(forumid, posttableid, postdatetimeStart, postdatetimeEnd, poster, lowerupper, ip, message);
        }

        /// <summary>
        /// 清除用户所发帖数以及精华数
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="days">删除用户多少天内的帖子</param>
        public static void ClearPosts(int uid, int days)
        {
            if (days == 0)
            {
                //清除用户所发的帖子
                foreach (DataRow dr in Posts.GetAllPostTableName().Rows)
                {
                    if (dr["id"].ToString() != "")
                        Discuz.Data.Posts.DeletePostByPosterid(int.Parse(dr["id"].ToString()), uid);
                }
                Discuz.Data.Topics.DeleteTopicByPosterid(uid);
                Discuz.Data.Users.ClearPosts(uid);
            }
            else
            {
                Discuz.Data.Posts.DeletePostByUidAndDays(uid, days);
            }
            Discuz.Data.Attachments.DeleteAttachmentByUid(uid, days);
        }

        /// <summary>
        /// 获取用户单位时间内的发帖数
        /// </summary>
        /// <param name="topNumber">Top条数</param>
        /// <param name="dateType">时间类型</param>
        /// <param name="dateNum">时间数</param>
        /// <returns></returns>
        public static List<UserPostCountInfo> GetUserPostCountList(int topNumber, DateType dateType, int dateNum)
        {
            return Discuz.Data.Posts.GetUserPostCountList(topNumber, dateType, dateNum, PostTables.GetPostTableName());
        }

        /// <summary>
        /// 把一个字符串中的 低序位 ASCII 字符 替换成 &#x  字符
        /// http://blog.csdn.net/jiljil/archive/2009/06/06/4247196.aspx
        /// 转换  ASCII  0 - 8  -> &#x0 - &#x8
        /// 转换  ASCII 11 - 12 -> &#xB - &#xC
        /// 转换  ASCII 14 - 31 -> &#xE - &#x1F
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ReplaceMessageChar(string message)
        {
            message = message.Replace(Convert.ToChar(0), ' ');
            message = message.Replace("]]>", "]]&gt;");
            StringBuilder tempmessage = new StringBuilder();
            foreach (char cc in message)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    tempmessage.AppendFormat("&#x{0:X};", ss);
                else
                    tempmessage.Append(cc);
            }
            return tempmessage.ToString();
        }
    }
}


