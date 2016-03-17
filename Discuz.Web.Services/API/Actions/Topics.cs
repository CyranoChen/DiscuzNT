using System;
using System.Text;
using Discuz.Common;
using Newtonsoft.Json;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common.Generic;

namespace Discuz.Web.Services.API.Actions
{
    public class Topics : ActionBase
    {
        /// <summary>
        /// 创建主题
        /// </summary>
        /// <returns></returns>
        public string Create()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("topic_info"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            Topic topic;
            try
            {
                topic = JavaScriptConvert.DeserializeObject<Topic>(GetParam("topic_info").ToString());
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (topic == null || AreParamsNullOrZeroOrEmptyString(topic.UId, topic.Fid, topic.Title, topic.Message))//(topic == null || topic.UId == 0 || topic.Fid == 0 || topic.Title == null || topic.Message == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            //如果是桌面程序则需要验证用户身份，如果topic_info中的uid与session_key对应的uid不匹配，则将topic_info中的uid改为session_key对应的uid，防止客户端程序密钥泄漏时的恶意发帖行为
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                ShortUserInfo currentUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
                if (currentUserInfo.Adminid != 1 && Uid != topic.UId)
                {
                    topic.UId = Uid;
                }
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topic.Fid ?? 0);

            if (forumInfo == null || forumInfo.Layer == 0)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            bool enabletag = (Config.Enabletag & forumInfo.Allowtag) == 1;

            //如果设置的主题类型，应该仍可添加topic

            //文档中应说明title长度范围和内容范围
            if (topic.Title.Length > 60)
            {
                ErrorCode = (int)ErrorType.API_EC_TITLE_INVALID;
                return "";
            }

            #region Inner
            ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(topic.UId ?? Uid);

            //新用户广告强力屏蔽检查，尽在传入session_key时验证
            if (Uid > 0)
            {
                ShortUserInfo currentUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);

                if ((Config.Disablepostad == 1) && userInfo.Adminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
                {
                    if ((Config.Disablepostadpostcount != 0 && currentUserInfo.Posts <= Config.Disablepostadpostcount) ||
                        (Config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-Config.Disablepostadregminute) <= Convert.ToDateTime(currentUserInfo.Joindate)))
                    {
                        foreach (string regular in Config.Disablepostadregular.Replace("\r", "").Split('\n'))
                        {
                            if (Posts.IsAD(regular, topic.Title, topic.Message))
                            {
                                ErrorCode = (int)ErrorType.API_EC_SPAM;
                                return "";
                            }
                        }
                    }
                }
            }

            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

            int iconid = topic.Iconid ?? 0;
            if (iconid > 15 || iconid < 0)
            {
                iconid = 0;
            }

            TopicInfo topicInfo = new TopicInfo();
            topicInfo.Fid = topic.Fid ?? 0;
            topicInfo.Iconid = iconid;
            topicInfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(topic.Title));

            bool htmlon = topic.Message.Length != Utils.RemoveHtml(topic.Message).Length && usergroupinfo.Allowhtml == 1;
            string message = ForumUtils.BanWordFilter(topic.Message);
            if (!htmlon)
            {
                message = Utils.HtmlDecode(message);
            }

            if (ForumUtils.HasBannedWord(topicInfo.Title) || ForumUtils.HasBannedWord(message))
            {
                ErrorCode = (int)ErrorType.API_EC_SPAM;
                return "";
            }
            string curdatetime = Utils.GetDateTime();

            topicInfo.Typeid = 0;
            if (forumInfo.Applytopictype == 1)
            {
                if (Discuz.Forum.Forums.IsCurrentForumTopicType(topic.Typeid.ToString(), forumInfo.Topictypes))
                {
                    topicInfo.Typeid = (int)topic.Typeid;
                }
                else if (forumInfo.Postbytopictype == 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PARAM;
                    return "";
                }
            }

            topicInfo.Readperm = 0;
            topicInfo.Price = 0;
            topicInfo.Poster = userInfo.Username;
            topicInfo.Posterid = userInfo.Uid;
            topicInfo.Postdatetime = curdatetime;
            topicInfo.Lastpost = curdatetime;
            topicInfo.Lastposter = userInfo.Username;
            topicInfo.Views = 0;
            topicInfo.Replies = 0;

            if (forumInfo.Modnewtopics == 1)
            {
                topicInfo.Displayorder = -2;
            }
            else
            {
                topicInfo.Displayorder = 0;
            }

            if (Scoresets.BetweenTime(Config.Postmodperiods) || ForumUtils.HasAuditWord(topicInfo.Title) || ForumUtils.HasAuditWord(message))
            {
                topicInfo.Displayorder = -2;
            }

            topicInfo.Highlight = "";
            topicInfo.Digest = 0;
            topicInfo.Rate = 0;
            topicInfo.Hide = 0;
            topicInfo.Attachment = 0;
            topicInfo.Moderated = 0;
            topicInfo.Closed = 0;

            string tags = string.Empty;
            string[] tagArray = null;

            if (!string.IsNullOrEmpty(topic.Tags))
            {
                //标签(Tag)操作                
                tags = topic.Tags.Trim();
                tagArray = Utils.SplitString(tags, ",", true, 2, 10);
                if (enabletag)
                {
                    if (topicInfo.Magic == 0)
                    {
                        topicInfo.Magic = 10000;
                    }
                    topicInfo.Magic = Utils.StrToInt(topicInfo.Magic.ToString() + "1", 0);
                }
            }

            int topicid = Discuz.Forum.Topics.CreateTopic(topicInfo);

            if (enabletag && tagArray != null && tagArray.Length > 0)
            {
                if (!ForumUtils.HasBannedWord(tags))
                {
                    ForumTags.CreateTopicTags(tagArray, topicid, userInfo.Uid, curdatetime);
                }
            }

            PostInfo postinfo = new PostInfo();
            postinfo.Fid = forumInfo.Fid;
            postinfo.Tid = topicid;
            postinfo.Parentid = 0;
            postinfo.Layer = 0;
            postinfo.Poster = userInfo.Username;
            postinfo.Posterid = userInfo.Uid;
            if (userInfo.Adminid == 1)
            {
                postinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
            }
            else
            {
                postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
            }

            postinfo.Postdatetime = curdatetime;
            postinfo.Message = message;
            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";

            if (ForumUtils.HasAuditWord(postinfo.Message))
            {
                postinfo.Invisible = 1;
            }

            if (forumInfo.Modnewtopics == 1 && userInfo.Adminid != 1)
            {
                postinfo.Invisible = 1;
            }
            //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
            if (userInfo.Adminid != 1 && Scoresets.BetweenTime(Config.Postmodperiods))
            {
                postinfo.Invisible = 1;
            }

            postinfo.Usesig = Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
            if (htmlon)
                postinfo.Htmlon = 1;
            else
                postinfo.Htmlon = 0;

            postinfo.Smileyoff = 1 - forumInfo.Allowsmilies;

            postinfo.Bbcodeoff = 1;

            if (usergroupinfo.Allowcusbbcode == 1 && forumInfo.Allowbbcode == 1)
            {
                postinfo.Bbcodeoff = 0;
            }
            postinfo.Parseurloff = 0;
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Topictitle = topicInfo.Title;

            int postid = 0;

            try
            {
                postid = Posts.CreatePost(postinfo);
            }
            catch
            {
                TopicAdmins.DeleteTopics(topicid.ToString(), false);
                ErrorCode = (int)ErrorType.API_EC_UNKNOWN;
                return "";
            }

            Discuz.Forum.Topics.AddParentForumTopics(forumInfo.Parentidlist.Trim(), 1);

            TopicCreateResponse tcr = new TopicCreateResponse();

            tcr.TopicId = topicid;
            tcr.Url = ForumUrl + Discuz.Forum.Urls.ShowTopicAspxRewrite(topicid, 0);


            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用，否则使用论坛默认积分
            float[] values = null;
            if (!forumInfo.Postcredits.Equals(""))
            {
                int index = 0;
                float tempval = 0;
                values = new float[8];
                foreach (string ext in Utils.SplitString(forumInfo.Postcredits, ","))
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
                    tempval = Utils.StrToFloat(ext, 0);
                    values[index - 1] = tempval;
                    index++;
                    if (index > 8)
                    {
                        break;
                    }
                }
            }



            #region 更新积分

            if (userInfo.Adminid != 1)
            {
                bool needaudit = false; //是否需要审核

                if (Scoresets.BetweenTime(Config.Postmodperiods))
                {
                    needaudit = true;
                }
                else
                {
                    if (forumInfo.Modnewtopics == 1 && userInfo.Adminid != 1)
                    {
                        //if (userinfo.Adminid > 1)
                        //{
                        //if (disablepost == 1 && topicinfo.Displayorder != -2)
                        //{
                        //if (useradminid == 3 && !Moderators.IsModer(useradminid, userid, forumid))
                        //{
                        //    needaudit = true;
                        //}
                        //else
                        //{
                        //    needaudit = false;
                        //}
                        //}
                        //else
                        //{
                        //needaudit = true;
                        //}
                        //}
                        //else
                        //{
                        needaudit = true;
                        //}
                    }
                    else
                    {
                        if (userInfo.Adminid != 1 && topicInfo.Displayorder == -2)
                        {
                            needaudit = true;
                        }
                    }
                }
                if (needaudit)
                {
                    //需要审核
                    tcr.NeedAudit = true;
                }
                else
                {
                    UpdateScore(userInfo.Uid, values);
                }
            }
            else
            {
                UpdateScore(userInfo.Uid, values);
            }

            #endregion


            #endregion

            //同步到其他应用程序
            Sync.NewTopic(topicid.ToString(), topicInfo.Title, topicInfo.Poster, topicInfo.Posterid.ToString(), topicInfo.Fid.ToString(), ApiKey);

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(tcr);
            }
            return SerializationHelper.Serialize(tcr);

        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <returns></returns>
        public string Reply()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("reply_info"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            Reply reply;
            try
            {
                reply = JavaScriptConvert.DeserializeObject<Reply>(GetParam("reply_info").ToString());
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (reply == null || AreParamsNullOrZeroOrEmptyString(reply.Tid, reply.Fid, reply.Message))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (reply.Title == null)
            {
                reply.Title = string.Empty;
            }

            TopicInfo topicinfo = Discuz.Forum.Topics.GetTopicInfo(reply.Tid);
            if (topicinfo == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            if (topicinfo.Closed == 1)
            {
                ErrorCode = (int)ErrorType.API_EC_TOPIC_CLOSED;
                return "";
            }


            ForumInfo foruminfo = Discuz.Forum.Forums.GetForumInfo(reply.Fid);
            if (foruminfo == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int groupid = 0;
            ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
            if (userinfo == null)
                groupid = 7;
            else
                groupid = userinfo.Groupid;
            UserGroupInfo usergroupinfo = Discuz.Forum.UserGroups.GetUserGroupInfo(groupid);

            if (topicinfo.Readperm > usergroupinfo.Readaccess && topicinfo.Posterid != Uid && usergroupinfo.Radminid != 1 && (userinfo != null && !Utils.InArray(userinfo.Username, foruminfo.Moderators.Split(','))))
            {
                ErrorCode = (int)ErrorType.API_EC_TOPIC_READ_PERM;
                return "";
            }

            if (foruminfo.Password != "")
            {
                ErrorCode = (int)ErrorType.API_EC_FORUM_PASSWORD;
                return "";
            }

            if (!Discuz.Forum.Forums.AllowViewByUserId(foruminfo.Permuserlist, Uid)) //判断当前用户在当前版块浏览权限
            {
                if (foruminfo.Viewperm == null || foruminfo.Viewperm == string.Empty)//当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        ErrorCode = (int)ErrorType.API_EC_FORUM_PERM;
                        return "";
                    }
                }
                else//当板块权限不为空，按照板块权限
                {
                    if (!Discuz.Forum.Forums.AllowView(foruminfo.Viewperm, groupid))
                    {
                        ErrorCode = (int)ErrorType.API_EC_FORUM_PERM;
                        return "";
                    }
                }
            }

            //是否有回复的权限
            if (!Discuz.Forum.Forums.AllowReplyByUserID(foruminfo.Permuserlist, Uid))
            {
                if (foruminfo.Replyperm == null || foruminfo.Replyperm == string.Empty)//当板块权限为空时根据用户组权限判断
                {
                    // 验证用户是否有发表主题的权限
                    if (usergroupinfo.Allowreply != 1)
                    {
                        ErrorCode = (int)ErrorType.API_EC_REPLY_PERM;
                        return "";
                    }
                }
                else//板块权限不为空时根据板块权限判断
                {
                    if (!Discuz.Forum.Forums.AllowReply(foruminfo.Replyperm, groupid))
                    {
                        ErrorCode = (int)ErrorType.API_EC_REPLY_PERM;
                        return "";
                    }
                }
            }


            // 如果是受灌水限制用户, 则判断是否是灌水
            if (userinfo != null)
            {
                string joindate = userinfo.Joindate;

                if (Utils.StrDateDiffMinutes(joindate, Config.Newbiespan) < 0)
                {
                    ErrorCode = (int)ErrorType.API_EC_FRESH_USER;
                    return "";
                }

            }

            if (reply.Title.IndexOf("　") != -1)
            {
                ErrorCode = (int)ErrorType.API_EC_FRESH_USER;
                return "";
            }
            else if (reply.Title.Length > 60)
            {
                ErrorCode = (int)ErrorType.API_EC_FRESH_USER;
                return "";
            }

            if (reply.Message.Length < Config.Minpostsize)
            {
                ErrorCode = (int)ErrorType.API_EC_MESSAGE_LENGTH;
                return "";
            }
            if (reply.Message.Length > Config.Maxpostsize)
            {
                ErrorCode = (int)ErrorType.API_EC_MESSAGE_LENGTH;
                return "";
            }

            //新用户广告强力屏蔽检查
            if ((Config.Disablepostad == 1) && usergroupinfo.Radminid < 1 || userinfo == null)  //如果开启新用户广告强力屏蔽检查或是游客
            {
                if (userinfo == null || (Config.Disablepostadpostcount != 0 && userinfo.Posts <= Config.Disablepostadpostcount) ||
                    (Config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-Config.Disablepostadregminute) <= Convert.ToDateTime(userinfo.Joindate)))
                {
                    foreach (string regular in Config.Disablepostadregular.Replace("\r", "").Split('\n'))
                    {
                        if (Posts.IsAD(regular, reply.Title, reply.Message))
                        {
                            ErrorCode = (int)ErrorType.API_EC_SPAM;
                            return "";
                        }
                    }
                }
            }

            if (ForumUtils.HasBannedWord(reply.Title) || ForumUtils.HasBannedWord(reply.Message))
            {
                ErrorCode = (int)ErrorType.API_EC_SPAM;
                return "";
            }

            PostInfo postInfo = PostReply(Uid, reply, usergroupinfo, userinfo, foruminfo, topicinfo.Title);
            if (topicinfo.Replies < (Config.Ppp + 9))
            {
                ForumUtils.DeleteTopicCacheFile(topicinfo.Tid);
            }

            TopicReplyResponse trr = new TopicReplyResponse();
            trr.PostId = postInfo.Pid;
            trr.Url = ForumUrl + string.Format("showtopic.aspx?topicid={0}&page=end#{1}", reply.Tid, trr.PostId);
            trr.NeedAudit = postInfo.Invisible == 1;

            //同步到其他应用程序
            Sync.Reply(postInfo.Pid.ToString(), postInfo.Tid.ToString(), postInfo.Topictitle, postInfo.Poster, postInfo.Posterid.ToString(), postInfo.Fid.ToString(), ApiKey);

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(trr);
            }
            return SerializationHelper.Serialize(trr);
        }

        /// <summary>
        /// 获得最新回复
        /// </summary>
        /// <returns></returns>
        public string GetRecentReplies()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("fid,tid,page_size,page_index"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int tid = Utils.StrToInt(GetParam("tid"), 0);
            int fid = Utils.StrToInt(GetParam("fid"), 0);
            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
            if (topicInfo == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            PostpramsInfo postPramsInfo = GetPostParamInfo(topicInfo, forumInfo);

            System.Data.DataTable lastpostlist = Posts.GetPagedLastDataTable(postPramsInfo);

            List<Post> list = new List<Post>();
            foreach (System.Data.DataRow dr in lastpostlist.Rows)
            {
                Post post = new Post();
                post.AdIndex = Utils.StrToInt(dr["adindex"], 0);
                post.Invisible = Utils.StrToInt(dr["invisible"], 0);
                post.Layer = Utils.StrToInt(dr["layer"], 0);
                post.Message = dr["message"].ToString();
                post.Pid = Utils.StrToInt(dr["pid"], 0);
                post.PostDateTime = DateTime.Parse(dr["postdatetime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                post.PosterAvator = dr["avatar"].ToString().Replace("\\", "/");
                post.PosterAvatorWidth = Utils.StrToInt(dr["avatarwidth"], 0);
                post.PosterAvatorHeight = Utils.StrToInt(dr["avatarheight"], 0);
                post.PosterEmail = dr["email"].ToString().Trim();
                post.PosterId = Utils.StrToInt(dr["posterid"], 0);
                post.PosterLocation = dr["location"].ToString();
                post.PosterName = dr["poster"].ToString();
                post.PosterShowEmail = Utils.StrToInt(dr["showemail"], 0);
                post.PosterSignature = dr["signature"].ToString();
                post.Rate = Utils.StrToInt(dr["rate"], 0);
                post.RateTimes = Utils.StrToInt(dr["ratetimes"], 0);
                post.UseSignature = Utils.StrToInt(dr["usesig"], 0);

                list.Add(post);
            }

            TopicGetRencentRepliesResponse tgrrr = new TopicGetRencentRepliesResponse();

            tgrrr.List = true;
            tgrrr.Count = topicInfo.Replies;
            tgrrr.Posts = list.ToArray();


            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(tgrrr);
            }
            return Util.AddMessageCDATA(SerializationHelper.Serialize(tgrrr));
        }

        /// <summary>
        /// 获得主题列表
        /// </summary>
        /// <returns></returns>
        public string GetList()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("fid,page_size,page_index"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int fid = Utils.StrToInt(GetParam("fid"), 0);
            int pageSize = GetIntParam("page_size", 20);
            int pageIndex = GetIntParam("page_index", 1);

            string topicTypeIdList = GetParam("type_id_list").ToString();//主题分类条件idlist
            string condition = string.Empty;//查询主题的条件

            if (topicTypeIdList != string.Empty && Utils.IsNumericList(topicTypeIdList))//如果条件不为空且是逗号分割的list，则添加condition条件
            {
                condition = " AND [typeid] IN (" + topicTypeIdList + ") ";
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            int count = Discuz.Forum.Topics.GetTopicCount(fid, true, string.Empty);
            List<TopicInfo> topicList = Discuz.Forum.Topics.GetTopicList(fid, pageSize, pageIndex,
                                                              0, 600, Config.Hottopic, forumInfo.Autoclose,
                                                              forumInfo.Topictypeprefix, condition);
            TopicGetListResponse tglr = new TopicGetListResponse();
            List<ForumTopic> list = new List<ForumTopic>();

            foreach (TopicInfo topicInfo in topicList)
            {
                ForumTopic topic = new ForumTopic();
                topic.Author = topicInfo.Poster;
                topic.AuthorId = topicInfo.Posterid;
                topic.LastPosterId = topicInfo.Lastposterid;
                topic.LastPostTime = DateTime.Parse(topicInfo.Lastpost).ToString("yyyy-MM-dd HH:mm:ss");
                topic.ReplyCount = topicInfo.Replies;
                topic.ViewCount = topicInfo.Views;
                topic.Title = topicInfo.Title;
                topic.TopicId = topicInfo.Tid;
                topic.Url = ForumUrl + Discuz.Forum.Urls.ShowTopicAspxRewrite(topic.TopicId, 0);
                list.Add(topic);
            }

            tglr.Count = count;
            tglr.Topics = list.ToArray();
            tglr.List = true;

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(tglr);
            }
            return SerializationHelper.Serialize(tglr);
        }

        /// <summary>
        /// 获得关注主题列表
        /// </summary>
        /// <returns></returns>
        public string GetAttentionList()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("fid,page_size,page_index"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int fid = GetIntParam("fid", 0);
            int pageSize = GetIntParam("page_size", 20);
            int pageIndex = GetIntParam("page_index", 1);
            //ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(fid);
            //topiclist = Topics.GetAttentionTopics(forumidlist, 16, pageid, keyword);
            //counts = Topics.GetAttentionTopicCount(forumidlist, keyword);

            int count = Discuz.Forum.Topics.GetAttentionTopicCount(fid.ToString(), string.Empty);
            List<TopicInfo> topicList = Discuz.Forum.Topics.GetAttentionTopics(fid.ToString(), pageSize, pageIndex, string.Empty);

            TopicGetListResponse tglr = new TopicGetListResponse();
            List<ForumTopic> list = new List<ForumTopic>();

            foreach (TopicInfo topicInfo in topicList)
            {
                ForumTopic topic = new ForumTopic();
                topic.Author = topicInfo.Poster;
                topic.AuthorId = topicInfo.Posterid;
                topic.LastPosterId = topicInfo.Lastposterid;
                topic.LastPostTime = DateTime.Parse(topicInfo.Lastpost).ToString("yyyy-MM-dd HH:mm:ss");
                topic.ReplyCount = topicInfo.Replies;
                topic.ViewCount = topicInfo.Views;
                topic.Title = topicInfo.Title;
                topic.TopicId = topicInfo.Tid;
                topic.Url = ForumUrl + Discuz.Forum.Urls.ShowTopicAspxRewrite(topic.TopicId, 0);
                list.Add(topic);
            }

            tglr.Count = count;
            tglr.Topics = list.ToArray();
            tglr.List = true;

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(tglr);
            }
            return SerializationHelper.Serialize(tglr);
        }

        /// <summary>
        /// 删除指定主题
        /// </summary>
        /// <returns></returns>
        public string Delete()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            //if (Uid < 1)
            //{
            //    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
            //    return "";
            //}

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return string.Empty;
            }

            if (!CheckRequiredParams("topic_ids"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }

            string topicIds = GetParam("topic_ids").ToString();
            if (!Utils.IsNumericList(topicIds))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }

            if (topicIds.Split(',').Length > 20)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }
            int forumId = TypeConverter.ObjectToInt(GetParam("fid"));

            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (!CheckRequiredParams("fid"))
                {
                    ErrorCode = (int)ErrorType.API_EC_PARAM;
                    return string.Empty;
                }
                ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(Uid);
                if (user == null || !Moderators.IsModer(user.Adminid, Uid, forumId))
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return string.Empty;
                }

                if (!Discuz.Forum.Topics.InSameForum(topicIds, forumId))
                {
                    ErrorCode = (int)ErrorType.API_EC_PARAM;
                    return string.Empty;
                }
            }

            bool result = Discuz.Forum.TopicAdmins.DeleteTopics(topicIds, false) > 0;

            TopicDeleteResponse tdr = new TopicDeleteResponse();
            tdr.Successfull = result ? 1 : 0;
            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", result.ToString().ToLower());

            return SerializationHelper.Serialize(tdr);

        }

        /// <summary>
        /// 编辑主题
        /// </summary>
        /// <returns></returns>
        public string Edit()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            ShortUserInfo currentUserInfo = null;

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }

                //判断客户端如果不是管理员就不能修改
                currentUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
                if (currentUserInfo.Adminid != 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }

            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("topic_info,tid"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            Topic topic;
            try
            {
                topic = JavaScriptConvert.DeserializeObject<Topic>(GetParam("topic_info").ToString());
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (topic == null)// || AreParamsNullOrZeroOrEmptyString(topic.UId, topic.Fid, topic.Title, topic.Message))//(topic == null || topic.UId == 0 || topic.Fid == 0 || topic.Title == null || topic.Message == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int topicId = GetIntParam("tid");



            //如果设置的主题类型，应该仍可添加topic

            //文档中应说明title长度范围和内容范围
            if (!AreParamsNullOrZeroOrEmptyString(topic.Title) && topic.Title.Length > 60)
            {
                ErrorCode = (int)ErrorType.API_EC_TITLE_INVALID;
                return "";
            }

            //内容长度限制应该在客户程序里实现
            //if (topic.Message.Length < Config.Minpostsize)
            //{
            //    //AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + Config.Minpostsize.ToString() + " 字多于 " + Config.Maxpostsize.ToString() + " 字");
            //    ErrorCode = (int)ErrorType.API_EC_PARAM;
            //    return "";

            //}
            //else if (topic.Message.Length > Config.Maxpostsize)
            //{
            //    //AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + Config.Minpostsize.ToString() + " 字多于 " + Config.Maxpostsize.ToString() + " 字");
            //    ErrorCode = (int)ErrorType.API_EC_PARAM;
            //    return "";
            //}

            #region Inner
            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(topicId);


            ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(topicInfo.Posterid);

            //新用户广告强力屏蔽检查
            if (Uid > 0 && (topic.Title != null || topic.Message != null))
            {
                if (currentUserInfo == null)
                {
                    currentUserInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
                }

                if ((Config.Disablepostad == 1) && userInfo.Adminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
                {
                    if ((Config.Disablepostadpostcount != 0 && currentUserInfo.Posts <= Config.Disablepostadpostcount) ||
                        (Config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-Config.Disablepostadregminute) <= Convert.ToDateTime(currentUserInfo.Joindate)))
                    {
                        foreach (string regular in Config.Disablepostadregular.Replace("\r", "").Split('\n'))
                        {
                            if (Posts.IsAD(regular, (topic.Title ?? string.Empty), (topic.Message ?? string.Empty)))
                            {
                                ErrorCode = (int)ErrorType.API_EC_SPAM;
                                return "";
                            }
                        }
                    }
                }
            }

            UserGroupInfo userGroupInfo = UserGroups.GetUserGroupInfo(userInfo.Groupid);

            int iconid = topic.Iconid ?? 0;
            if (iconid > 15 || iconid < 0)
            {
                iconid = 0;
            }

            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topic.Fid ?? topicInfo.Fid);

            bool enabletag = (Config.Enabletag & forumInfo.Allowtag) == 1;

            if (topic.Fid != null)
                topicInfo.Fid = topic.Fid ?? topicInfo.Fid;
            if (topic.Iconid != null)
                topicInfo.Iconid = iconid;
            if (topic.Title != null)
            {
                topicInfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(topic.Title));
                if (ForumUtils.HasAuditWord(topicInfo.Title))
                {
                    topicInfo.Displayorder = -2;
                }
            }

            string message = null;
            if (topic.Message != null)
            {
                bool htmlon = topic.Message.Length != Utils.RemoveHtml(topic.Message).Length && userGroupInfo.Allowhtml == 1;
                message = ForumUtils.BanWordFilter(topic.Message);
                if (!htmlon)
                {
                    message = Utils.HtmlDecode(message);
                }
                if (ForumUtils.HasBannedWord(topicInfo.Title) || ForumUtils.HasBannedWord(message))
                {
                    ErrorCode = (int)ErrorType.API_EC_SPAM;
                    return "";
                }
                if (ForumUtils.HasAuditWord(message))
                {
                    topicInfo.Displayorder = -2;
                }
            }

            string tags = string.Empty;
            string[] tagArray = null;

            if (!string.IsNullOrEmpty(topic.Tags))
            {
                //标签(Tag)操作                
                tags = topic.Tags.Trim();
                tagArray = Utils.SplitString(tags, ",", true, 2, 10);
                if (enabletag)
                {
                    if (topicInfo.Magic == 0)
                    {
                        topicInfo.Magic = 10000;
                    }
                    topicInfo.Magic = Utils.StrToInt(topicInfo.Magic.ToString() + "1", 0);
                }
            }

            if (forumInfo.Applytopictype == 1)
            {
                if (Discuz.Forum.Forums.IsCurrentForumTopicType(topic.Typeid.ToString(), forumInfo.Topictypes))
                {
                    topicInfo.Typeid = (int)topic.Typeid;
                }
                else if (forumInfo.Postbytopictype == 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_PARAM;
                    return "";
                }
                else
                {
                    topicInfo.Typeid = 0;
                }
            }

            int result = Discuz.Forum.Topics.UpdateTopic(topicInfo);

            if (enabletag && tagArray != null && tagArray.Length > 0)
            {
                if (!ForumUtils.HasBannedWord(tags))
                {
                    ForumTags.CreateTopicTags(tagArray, topicInfo.Tid, userInfo.Uid, topicInfo.Postdatetime);
                }

            }

            PostInfo postInfo = Discuz.Forum.Posts.GetPostInfo(topicInfo.Tid, Discuz.Forum.Posts.GetFirstPostId(topicInfo.Tid));
            if (topic.Fid != null)
                postInfo.Fid = topicInfo.Fid;
            if (topic.Title != null)
            {
                postInfo.Title = topicInfo.Title;
                postInfo.Topictitle = topicInfo.Title;
            }

            if (topic.Message != null)
            {
                postInfo.Message = message;
                if (ForumUtils.HasAuditWord(postInfo.Message))
                {
                    postInfo.Invisible = 1;
                }
            }
            result = Posts.UpdatePost(postInfo);

            TopicEditResponse ter = new TopicEditResponse();
            ter.Successfull = result;

            #endregion

            if (Format == FormatType.JSON)
            {
                return (result == 1).ToString().ToLower();
            }
            return SerializationHelper.Serialize(ter);

        }

        /// <summary>
        /// 获取主题及帖子列表
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }

            if (!CheckRequiredParams("tid,page_size,page_index"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int tid = Utils.StrToInt(GetParam("tid"), 0);
            TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
            if (topicInfo == null)
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }
            ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(topicInfo.Fid);

            PostpramsInfo postPramsInfo = GetPostParamInfo(topicInfo, forumInfo);
            List<ShowtopicPageAttachmentInfo> attachmentList = new List<ShowtopicPageAttachmentInfo>();

            List<ShowtopicPagePostInfo> postList = Posts.GetPostList(postPramsInfo, out attachmentList, false);

            List<Post> list = new List<Post>();
            foreach (ShowtopicPagePostInfo postInfo in postList)
            {
                Post post = new Post();
                post.AdIndex = postInfo.Adindex;
                post.Invisible = postInfo.Invisible;
                post.Layer = postInfo.Layer;
                post.Message = postInfo.Message;
                post.Pid = postInfo.Pid;
                post.PostDateTime = postInfo.Postdatetime;
                post.PosterAvator = postInfo.Avatar;
                post.PosterAvatorWidth = postInfo.Avatarwidth;
                post.PosterAvatorHeight = postInfo.Avatarheight;
                post.PosterEmail = postInfo.Email;
                post.PosterId = postInfo.Posterid;
                post.PosterLocation = postInfo.Location;
                post.PosterName = postInfo.Poster;
                post.PosterShowEmail = postInfo.Showemail;
                post.PosterSignature = postInfo.Signature;
                post.Rate = postInfo.Rate;
                post.RateTimes = postInfo.Ratetimes;
                post.UseSignature = postInfo.Usesig;

                list.Add(post);

            }
            TopicGetResponse tgr = new TopicGetResponse();
            tgr.Author = topicInfo.Poster;
            tgr.AuthorId = topicInfo.Posterid;
            tgr.Fid = topicInfo.Fid;
            tgr.Iconid = topicInfo.Iconid;
            tgr.LastPosterId = topicInfo.Lastposterid;
            tgr.LastPostTime = topicInfo.Lastpost;
            tgr.List = list.Count > 1;
            tgr.ReplyCount = topicInfo.Replies;
            tgr.Tags = ForumTags.GetTagsByTopicId(topicInfo.Tid);
            tgr.Title = topicInfo.Title;
            tgr.TopicId = topicInfo.Tid;
            tgr.Url = ForumUrl + Discuz.Forum.Urls.ShowTopicAspxRewrite(topicInfo.Tid, 0);
            tgr.ViewCount = topicInfo.Views;
            tgr.TypeId = topicInfo.Typeid;

            SortedList<int, string> topicTypeList = Caches.GetTopicTypeArray();
            topicTypeList.TryGetValue(topicInfo.Typeid, out tgr.TypeName);

            tgr.Posts = list.ToArray();
            tgr.Attachments = ConvertAttachmentArray(attachmentList);

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(tgr);
            }
            return Util.AddTitleCDATA(Util.AddMessageCDATA(SerializationHelper.Serialize(tgr)));
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <returns></returns>
        public string DeleteReplies()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                if (Uid < 1)
                {
                    ErrorCode = (int)ErrorType.API_EC_SESSIONKEY;
                    return "";
                }
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return string.Empty;
            }

            if (!CheckRequiredParams("post_ids,tid"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return string.Empty;
            }
            string successfulIds = string.Empty;

            int tid = GetIntParam("tid");
            //如果是桌面程序则需要验证用户身份
            if (this.App.ApplicationType == (int)ApplicationType.DESKTOP)
            {
                ShortUserInfo userInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
                TopicInfo topicInfo = Discuz.Forum.Topics.GetTopicInfo(tid);
                if (!Discuz.Forum.Moderators.IsModer(userInfo.Adminid, Uid, topicInfo.Fid))
                {
                    ErrorCode = (int)ErrorType.API_EC_PERMISSION_DENIED;
                    return "";
                }
            }

            int i = 0;
            string postTableId = Discuz.Forum.Posts.GetPostTableId(tid);
            foreach (string s in GetParam("post_ids").ToString().Split(','))
            {
                int pid = TypeConverter.StrToInt(s);
                if (pid < 1)
                    continue;
                if (Discuz.Forum.Posts.DeletePost(postTableId, pid, false, true) > 0)
                {
                    successfulIds += (pid + ",");
                    i++;
                }
                if (i >= 20)
                {
                    break;
                }
            }

            if (successfulIds.Length > 0)
                successfulIds = successfulIds.Remove(successfulIds.Length - 1);
            if (Format == FormatType.JSON)
                return string.Format("\"{0}\"", successfulIds);

            TopicDeleteRepliesResponse tdrr = new TopicDeleteRepliesResponse();
            tdrr.Result = successfulIds;
            return SerializationHelper.Serialize(tdrr);
        }

        #region Private Method
        private void UpdateScore(int userid, float[] values)
        {
            if (values != null)
            {
                ///使用版块内积分
                UserCredits.UpdateUserExtCredits(userid, values, false);
            }
            else
            {
                ///使用默认积分
                UserCredits.UpdateUserCreditsByPostTopic(userid);
            }
        }

        /// <summary>
        /// 将Entity内附件列表类型转换成API内附件列表类型
        /// </summary>
        /// <param name="attachmentList"></param>
        /// <returns></returns>
        private Attachment[] ConvertAttachmentArray(List<ShowtopicPageAttachmentInfo> attachmentList)
        {
            List<Attachment> apiAttachmentList = new List<Attachment>();
            foreach (ShowtopicPageAttachmentInfo s in attachmentList)
            {
                Attachment a = new Attachment();
                a.AId = s.Aid;
                a.AllowRead = s.Allowread;
                a.IsImage = s.Attachimgpost;
                a.OriginalFileName = s.Attachment;
                a.AttachPrice = s.Attachprice;
                a.Description = s.Description;
                a.DownloadCount = s.Downloads;
                a.FileName = s.Filename;
                a.FileSize = s.Filesize;
                a.FileType = s.Filetype;
                a.DownloadPerm = s.Getattachperm;
                a.Inserted = s.Inserted;
                a.IsBought = s.Isbought;
                a.PId = s.Pid;
                a.PostDateTime = s.Postdatetime;
                a.Preview = s.Preview;
                a.ReadPerm = s.Readperm;
                a.TId = s.Tid;
                a.UId = s.Uid;
                apiAttachmentList.Add(a);
            }
            return apiAttachmentList.ToArray();
        }

        /// <summary>
        /// 发回复
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="reply"></param>
        /// <param name="usergroupinfo"></param>
        /// <param name="userinfo"></param>
        /// <param name="foruminfo"></param>
        /// <param name="topictitle"></param>
        /// <returns></returns>
        private PostInfo PostReply(int uid, Reply reply, UserGroupInfo usergroupinfo, ShortUserInfo userinfo, ForumInfo foruminfo, string topictitle)
        {
            int hide = 0;
            if (ForumUtils.IsHidePost(reply.Message) && usergroupinfo.Allowhidecode == 1)
            {
                hide = 1;
            }

            string curdatetime = Utils.GetDateTime();
            //int replyuserid = postinfo.Posterid;
            string replyTitle = reply.Title;
            if (replyTitle.Length >= 50)
            {
                replyTitle = Utils.CutString(replyTitle, 0, 50) + "...";
            }

            PostInfo postinfo = new PostInfo();
            postinfo.Fid = reply.Fid;
            postinfo.Tid = reply.Tid;
            postinfo.Parentid = 0;
            postinfo.Layer = 1;
            postinfo.Poster = (userinfo == null ? "游客" : userinfo.Username);
            postinfo.Posterid = uid;

            bool htmlon = reply.Message.Length != Utils.RemoveHtml(reply.Message).Length && usergroupinfo.Allowhtml == 1;
            string message = ForumUtils.BanWordFilter(reply.Message);
            if (!htmlon)
            {
                message = Utils.HtmlDecode(message);
            }
            postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(replyTitle));
            postinfo.Message = message;
            postinfo.Postdatetime = curdatetime;

            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";
            postinfo.Debateopinion = 0;//DNTRequest.GetInt("debateopinion", 0);
            if (foruminfo.Modnewposts == 1 && usergroupinfo.Radminid != 1 && usergroupinfo.Radminid != 2)
            {
                postinfo.Invisible = 1;
            }
            else
            {
                postinfo.Invisible = 0;
            }

            //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
            if (usergroupinfo.Radminid != 1)
            {
                if (Scoresets.BetweenTime(Config.Postmodperiods))
                {
                    postinfo.Invisible = 1;
                }

                if (ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                {
                    postinfo.Invisible = 1;
                }
            }

            postinfo.Usesig = 1;//Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
            postinfo.Htmlon = htmlon ? 1 : 0;

            postinfo.Smileyoff = 1 - foruminfo.Allowsmilies;

            postinfo.Bbcodeoff = 1;
            if (usergroupinfo.Allowcusbbcode == 1 && foruminfo.Allowbbcode == 1)
            {
                postinfo.Bbcodeoff = 0;
            }
            postinfo.Parseurloff = 0;
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Topictitle = topictitle;

            // 产生新帖子
            int postid = Posts.CreatePost(postinfo);

            if (hide == 1)
            {
                Discuz.Forum.Topics.UpdateTopicHide(reply.Tid);
            }
            Discuz.Forum.Topics.UpdateTopicReplyCount(reply.Tid);
            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用，否则使用论坛默认积分
            float[] values = null;
            if (!foruminfo.Replycredits.Equals(""))
            {
                int index = 0;
                float tempval = 0;
                values = new float[8];
                foreach (string ext in Utils.SplitString(foruminfo.Replycredits, ","))
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
                    tempval = Utils.StrToFloat(ext, 0.0f);
                    values[index - 1] = tempval;
                    index++;
                    if (index > 8)
                    {
                        break;
                    }
                }
            }

            if (postinfo.Invisible != 1 && userinfo != null)
            {
                if (values != null)
                {
                    ///使用版块内积分
                    UserCredits.UpdateUserExtCredits(uid, values, false);
                }
                else
                {
                    ///使用默认积分
                    UserCredits.UpdateUserCreditsByPosts(uid);
                }
            }
            postinfo.Pid = postid;
            return postinfo;
        }


        /// <summary>
        /// 获取帖子参数
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <param name="forumInfo"></param>
        /// <returns></returns>
        private PostpramsInfo GetPostParamInfo(TopicInfo topicInfo, ForumInfo forumInfo)
        {
            //判断是否为回复可见帖, hide=0为非回复可见(正常), hide > 0为回复可见, hide=-1为回复可见但当前用户已回复
            int hide = 0;
            if (topicInfo.Hide == 1)
            {
                hide = topicInfo.Hide;
                if (Uid > 0 && Posts.IsReplier(topicInfo.Tid, Uid))
                {
                    hide = -1;
                }
            }
            //判断是否为回复可见帖, price=0为非购买可见(正常), price > 0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topicInfo.Price > 0)
            {
                price = topicInfo.Price;
                if (Uid > 0 && PaymentLogs.IsBuyer(topicInfo.Tid, Uid))//判断当前用户是否已经购买
                {
                    price = -1;
                }
            }

            ShortUserInfo userInfo = new ShortUserInfo();
            if (Uid > 0)
            {
                userInfo = Discuz.Forum.Users.GetShortUserInfo(Uid);
            }
            PostpramsInfo postpramsinfo = new PostpramsInfo();
            postpramsinfo.Fid = topicInfo.Fid;
            postpramsinfo.Tid = topicInfo.Tid;
            postpramsinfo.Jammer = forumInfo.Jammer;
            postpramsinfo.Pagesize = Utils.StrToInt(GetParam("page_size"), 10);
            postpramsinfo.Pageindex = Utils.StrToInt(GetParam("page_index"), 1);
            postpramsinfo.Getattachperm = forumInfo.Getattachperm;
            postpramsinfo.Usergroupid = userInfo.Uid < 1 ? 7 : userInfo.Groupid;
            postpramsinfo.Attachimgpost = Config.Attachimgpost;
            postpramsinfo.Showattachmentpath = Config.Showattachmentpath;
            postpramsinfo.Hide = hide;
            postpramsinfo.Price = price;
            postpramsinfo.Ubbmode = false;

            postpramsinfo.Showimages = forumInfo.Allowimgcode;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Smiliesmax = Config.Smiliesmax;
            postpramsinfo.Bbcodemode = Config.Bbcodemode;
            postpramsinfo.CurrentUserGroup = Discuz.Forum.UserGroups.GetUserGroupInfo(postpramsinfo.Usergroupid);
            postpramsinfo.Usercredits = userInfo.Credits;
            return postpramsinfo;
        }

        #endregion
    }
}
