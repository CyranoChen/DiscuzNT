using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Cache;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 个人中心发表日志
    /// </summary>
    public class usercpspacepostblog : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 日志分类列表
        /// </summary>
        public DataTable categoryslist;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        /// <summary>
        /// 空间设置信息
        /// </summary>
        public SpaceConfigInfo spaceconfig;
        /// <summary>
        /// 是否启用了空间的Tag功能
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 获得文章内容
        /// </summary>
        public string blogcontent = DNTRequest.GetString("blogtext");
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Users.GetUserInfo(userid);
            if (config.Enablespace != 1)
            {
                AddErrLine("个人空间功能已被关闭");
                return;
            }
            if (user.Spaceid <= 0)
            {
                AddErrLine("您尚未开通个人空间");
                return;
            }

            enabletag = config.Enabletag == 1;
            categoryslist = Space.Data.DbProvider.GetInstance().GetSpaceCategoryListByUserId(userid);
            spaceconfig = Spaces.GetSpaceConfigByUserId(userid);
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                if (!Utils.IsNumeric(DNTRequest.GetString("poststatus")) || !Utils.IsNumeric(DNTRequest.GetString("commentstatus")))
                {
                    AddErrLine("请您确保 发布类型,评论类型,数据项有效!");
                    return;
                }
                if (DNTRequest.GetString("title") == "")
                {
                    AddErrLine("请您输入文章标题");
                    return;
                }
                if (DNTRequest.GetString("title").Length > 150)
                {
                    AddErrLine("请将文章标题保持在150字以内");
                    return;
                }
                if (blogcontent == "")
                {
                    AddErrLine("请您输入文章内容");
                    return;
                }

                SpacePostInfo spacepostsinfo = new SpacePostInfo();
                spacepostsinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
                spacepostsinfo.Content = Utils.HtmlEncode(ForumUtils.BanWordFilter(blogcontent));
                spacepostsinfo.Category = DNTRequest.GetString("category");
                spacepostsinfo.PostStatus = DNTRequest.GetFormInt("poststatus", 0);
                spacepostsinfo.CommentStatus = DNTRequest.GetFormInt("commentstatus", 0);
                spacepostsinfo.Postdatetime = DateTime.Now;
                spacepostsinfo.Author = username;
                spacepostsinfo.Uid = userid;
                spacepostsinfo.PostUpDateTime = DateTime.Now;
                spacepostsinfo.Commentcount = 0;

                int postid = Space.Data.DbProvider.GetInstance().AddSpacePost(spacepostsinfo);

                //启用了标签功能
                if (enabletag)
                {
                    string tags = DNTRequest.GetString("tags").Trim();
                    string[] tagsArray = null;
                    if (!Utils.StrIsNullOrEmpty(tags))
                    {
                        tagsArray = Utils.SplitString(tags, " ", true, 10);
                        if (tagsArray != null && tagsArray.Length > 0)
                        {
                            Space.Data.DbProvider.GetInstance().CreateSpacePostTags(string.Join(" ", tagsArray), postid, userid, Utils.GetDateTime());
                            SpaceTags.WriteSpacePostTagsCacheFile(postid);
                        }
                    }
                }
                DNTCache.GetCacheService().RemoveObject("/Space/RecentUpdateSpaceAggregationList");
                SetUrl("usercpspacemanageblog.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("文章增加成功");
            }
        }
    }
}