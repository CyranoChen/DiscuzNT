using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Discuz.Entity;
using Discuz.Common;
using Discuz.Forum;
using System.Data;
using Discuz.Config;

namespace Discuz.Web.Services.API.Actions
{
    public class Forums : ActionBase
    {
        /// <summary>
        /// 创建板块
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

            if (!CheckRequiredParams("forum_info"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            Forum forum;
            try
            {
                forum = JavaScriptConvert.DeserializeObject<Forum>(GetParam("forum_info").ToString());
            }
            catch
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (forum == null || AreParamsNullOrZeroOrEmptyString(forum.Name))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            if (!Utils.StrIsNullOrEmpty(forum.RewriteName) && Discuz.Forum.Forums.CheckRewriteNameInvalid(forum.RewriteName))
            {
                ErrorCode = (int)ErrorType.API_EC_REWRITENAME;
                return "";
            }


            int fid;
            if (forum.ParentId > 0)
            {
                #region 添加与当前论坛同级的论坛

                //添加与当前论坛同级的论坛
                //DataRow dr = AdminForums.GetForum(forum.ParentId);
                ForumInfo forumInfo = Discuz.Forum.Forums.GetForumInfo(forum.ParentId);

                //找出当前要插入的记录所用的FID
                string parentidlist = null;
                if (forumInfo.Parentidlist == "0")
                {
                    parentidlist = forumInfo.Fid.ToString();
                }
                else
                {
                    parentidlist = forumInfo.Parentidlist + "," + forumInfo.Fid;
                }

                int maxdisplayorder = 0;
                DataTable dt = AdminForums.GetMaxDisplayOrder(forum.ParentId);
                if ((dt.Rows.Count > 0) && (dt.Rows[0][0].ToString() != ""))
                {
                    maxdisplayorder = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    maxdisplayorder = forumInfo.Displayorder;
                }

                AdminForums.UpdateForumsDisplayOrder(maxdisplayorder);
                fid = InsertForum(forum, forumInfo.Layer + 1, parentidlist, 0, maxdisplayorder + 1);

                AdminForums.SetSubForumCount(forumInfo.Fid);

                #endregion
            }
            else
            {
                #region 按根论坛插入

                int maxdisplayorder = AdminForums.GetMaxDisplayOrder();
                fid = InsertForum(forum, 0, "0", 0, maxdisplayorder);

                #endregion
            }
            //string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();

            ForumCreateResponse fcr = new ForumCreateResponse();
            fcr.Fid = fid;
            fcr.Url = ForumUrl + Urls.ShowForumAspxRewrite(fid, 1, forum.RewriteName);


            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(fcr);
            }
            return SerializationHelper.Serialize(fcr);
        }

        /// <summary>
        /// 获得板块信息
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

            if (!CheckRequiredParams("fid"))
            {
                ErrorCode = (int)ErrorType.API_EC_PARAM;
                return "";
            }

            int fid = Utils.StrToInt(GetParam("fid"), 0);
            if (fid < 1)
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
            //string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();

            ForumGetResponse fgr = new ForumGetResponse();
            fgr.Fid = fid;
            fgr.Url = ForumUrl + Urls.ShowForumAspxRewrite(fid, 1, forumInfo.Rewritename);
            fgr.CurTopics = forumInfo.CurrentTopics;
            fgr.Description = forumInfo.Description;
            fgr.Icon = forumInfo.Icon;
            fgr.LastPost = forumInfo.Lastpost;
            fgr.LastPoster = forumInfo.Lastposter.Trim();
            fgr.LastPosterId = forumInfo.Lastposterid;
            fgr.LastTid = forumInfo.Lasttid;
            fgr.LastTitle = forumInfo.Lasttitle.Trim();
            fgr.Moderators = forumInfo.Moderators;
            fgr.Name = forumInfo.Name;
            fgr.ParentId = forumInfo.Parentid;
            fgr.ParentIdList = forumInfo.Parentidlist.Trim();
            fgr.PathList = forumInfo.Pathlist.Trim();
            fgr.Posts = forumInfo.Posts;
            fgr.Rules = forumInfo.Rules;
            fgr.Status = forumInfo.Status;
            fgr.SubForumCount = forumInfo.Subforumcount;
            fgr.TodayPosts = forumInfo.Todayposts;
            fgr.Topics = forumInfo.Topics;


            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(fgr);
            }
            return SerializationHelper.Serialize(fgr);
        }

        /// <summary>
        /// 获得首页版块信息
        /// </summary>
        /// <returns></returns>
        public string GetIndexList()
        {
            if (Signature != GetParam("sig").ToString())
            {
                ErrorCode = (int)ErrorType.API_EC_SIGNATURE;
                return "";
            }

            if (CallId <= LastCallId)
            {
                ErrorCode = (int)ErrorType.API_EC_CALLID;
                return "";
            }
            int userGroupId = 7;
            if (Uid > 0)
                userGroupId = Discuz.Forum.Users.GetShortUserInfo(Uid).Groupid;

            int topicCount, postCount, todayCount;
            List<IndexPageForumInfo> list = Discuz.Forum.Forums.GetForumIndexCollection(1, userGroupId, 0, out topicCount, out postCount, out todayCount);

            List<IndexForum> newList = new List<IndexForum>();

            foreach (IndexPageForumInfo f in list)
            {
                IndexForum newf = new IndexForum();
                newf.Fid = f.Fid;
                newf.Url = ForumUrl + Urls.ShowForumAspxRewrite(f.Fid, 1, f.Rewritename);
                newf.CurTopics = f.CurrentTopics;
                newf.Description = f.Description;
                newf.Icon = f.Icon;
                newf.LastPost = f.Lastpost;
                newf.LastPoster = f.Lastposter.Trim();
                newf.LastPosterId = f.Lastposterid;
                newf.LastTid = f.Lasttid;
                newf.LastTitle = f.Lasttitle.Trim();
                newf.Moderators = f.Moderators;
                newf.Name = f.Name;
                newf.ParentId = f.Parentid;
                newf.ParentIdList = f.Parentidlist.Trim();
                newf.PathList = f.Pathlist.Trim();
                newf.Posts = f.Posts;
                newf.Rules = f.Rules;
                newf.Status = f.Status;
                newf.SubForumCount = f.Subforumcount;
                newf.TodayPosts = f.Todayposts;
                newf.Topics = f.Topics;

                newList.Add(newf);
            }

            ForumGetIndexListResponse fgilr = new ForumGetIndexListResponse();
            fgilr.Forums = newList.ToArray();
            fgilr.List = true;

            if (Format == FormatType.JSON)
            {
                return JavaScriptConvert.SerializeObject(fgilr);
            }
            return SerializationHelper.Serialize(fgilr);

        }


        #region Private Methods
        private int InsertForum(Forum forum, int layer, string parentidlist, int subforumcount, int systemdisplayorder)
        {
            #region 添加新论坛
            ForumInfo foruminfo = new ForumInfo();

            foruminfo.Parentid = forum.ParentId;
            foruminfo.Layer = layer;
            foruminfo.Parentidlist = parentidlist;
            foruminfo.Subforumcount = subforumcount;
            foruminfo.Name = forum.Name.Trim();

            foruminfo.Status = forum.Status == null ? 1 : Convert.ToInt32(forum.Status);

            foruminfo.Displayorder = systemdisplayorder;

            foruminfo.Templateid = forum.TemplateId;
            foruminfo.Allowsmilies = forum.AllowSmilies;
            foruminfo.Allowrss = forum.AllowRss;
            foruminfo.Allowhtml = 1;
            foruminfo.Allowbbcode = forum.AllowBbcode;
            foruminfo.Allowimgcode = forum.AllowImgcode;
            foruminfo.Allowblog = 0;
            foruminfo.Istrade = 0;

            //__foruminfo.Allowpostspecial = 0; //需要作与运算如下
            //__foruminfo.Allowspecialonly = 0;　//需要作与运算如下
            //$allow辩论 = allowpostspecial & 16;
            //$allow悬赏 = allowpostspecial & 4;
            //$allow投票 = allowpostspecial & 1;

            foruminfo.Alloweditrules = forum.AllowEditRules;
            foruminfo.Recyclebin = forum.RecycleBin;
            foruminfo.Modnewposts = forum.ModNewPosts;
            foruminfo.Modnewtopics = forum.ModNewTopics;
            foruminfo.Jammer = forum.Jammer;
            foruminfo.Disablewatermark = forum.DisableWatermark;
            foruminfo.Inheritedmod = forum.InheritedMod;
            foruminfo.Allowthumbnail = forum.AllowThumbnail;
            foruminfo.Allowtag = forum.AllowTag;

            foruminfo.Allowpostspecial = 0;
            foruminfo.Allowspecialonly = 0;

            foruminfo.Autoclose = forum.AutoClose;

            foruminfo.Description = forum.Description == null ? string.Empty : forum.Description;
            foruminfo.Password = string.Empty;
            foruminfo.Icon = forum.Icon == null ? string.Empty : forum.Icon;
            foruminfo.Postcredits = "";
            foruminfo.Replycredits = "";
            foruminfo.Redirect = string.Empty;
            foruminfo.Attachextensions = string.Empty;
            foruminfo.Moderators = forum.Moderators == null ? string.Empty : forum.Moderators;
            foruminfo.Rules = forum.Rules == null ? string.Empty : forum.Rules;
            foruminfo.Seokeywords = forum.SeoKeywords == null ? string.Empty : forum.SeoKeywords;
            foruminfo.Seodescription = forum.SeoDescription == null ? string.Empty : forum.SeoDescription;
            foruminfo.Rewritename = forum.RewriteName == null ? string.Empty : forum.RewriteName;
            foruminfo.Topictypes = string.Empty;
            foruminfo.Colcount = 1;
            foruminfo.Viewperm = string.Empty;
            foruminfo.Postperm = string.Empty;
            foruminfo.Replyperm = string.Empty;
            foruminfo.Getattachperm = string.Empty;
            foruminfo.Postattachperm = string.Empty;

            return Discuz.Forum.AdminForums.CreateForums(foruminfo);


            #endregion
        }
        #endregion
    }
}
