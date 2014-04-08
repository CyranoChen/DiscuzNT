using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Space;
using Discuz.Space.Entities;
using Discuz.Space.Provider;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Space.Utilities;
using Discuz.Common.Generic;
using Discuz.Space.Data;

namespace Discuz.Space.Pages
{
    /// <summary>
    /// 开通个人空间
    /// </summary>
    public class spaceregister : PageBase
    {
        private SpaceConfigInfo spaceconfiginfo = new SpaceConfigInfo();

        /// <summary>
        /// 个人空间激活设置
        /// </summary>
        public SpaceActiveConfigInfo spaceactiveconfig = SpaceActiveConfigs.GetConfig();

        protected override void ShowPage()
        {
            pagetitle = string.Format("激活{0}", config.Spacename);

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }

            UserInfo user = Users.GetUserInfo(userid);

            if (config.Enablespace != 1)
            {
                AddErrLine(string.Format("{0}功能已被关闭", config.Spacename));
                return;
            }

            bool isactivespace = false;
            bool isallowapply = true;
            if (user.Spaceid > 0)
                isactivespace = true;
            else
            {
                if (user.Spaceid < 0)
                    isallowapply = false;
                else
                {
                    if (spaceactiveconfig.AllowPostcount == "1" || spaceactiveconfig.AllowDigestcount == "1" ||
                        spaceactiveconfig.AllowScore == "1" || spaceactiveconfig.AllowUsergroups == "1")
                    {
                        if (spaceactiveconfig.AllowPostcount == "1")
                            isallowapply = isallowapply && (Convert.ToInt32(spaceactiveconfig.Postcount) <= user.Posts);
                        if (spaceactiveconfig.AllowDigestcount == "1")
                            isallowapply = isallowapply &&
                                           (Convert.ToInt32(spaceactiveconfig.Digestcount) <= user.Digestposts);
                        if (spaceactiveconfig.AllowScore == "1")
                            isallowapply = isallowapply && (Convert.ToInt32(spaceactiveconfig.Score) <= user.Credits);
                        if (spaceactiveconfig.AllowUsergroups == "1")
                            isallowapply = isallowapply &&
                                           (("," + spaceactiveconfig.Usergroups + ",").IndexOf("," + user.Groupid + ",") !=
                                            -1);
                    }
                    else
                        isallowapply = false;
                }
            }

            if (isactivespace)
            {
                AddErrLine("您已经申请过个人空间！");
                return;
            }
            if (!isallowapply)
            {
                AddErrLine("您未被允许申请个人空间！");
                return;
            }

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                if (DNTRequest.GetString("spacetitle").Length > 100)
                {
                    AddErrLine("个人空间标题不得超过100个字符");
                    return;
                }
                if (DNTRequest.GetString("description").Length > 200)
                {
                    AddErrLine("个人空间描述不得超过200个字符");
                    return;
                }
                if (DNTRequest.GetInt("bpp", 0) == 0)
                {
                    AddErrLine("显示日志篇数必需是一个大于0的数字");
                    return;
                }

                if (page_err == 0)
                {
                    DataRow dr = DbProvider.GetInstance().GetThemes();
                    spaceconfiginfo = new SpaceConfigInfo();
                    spaceconfiginfo.UserID = userid;
                    spaceconfiginfo.Spacetitle = Utils.HtmlEncode(DNTRequest.GetString("spacetitle"));
                    spaceconfiginfo.Description = Utils.HtmlEncode(DNTRequest.GetString("description"));
                    spaceconfiginfo.BlogDispMode = DNTRequest.GetInt("blogdispmode", 0);
                    spaceconfiginfo.Bpp = DNTRequest.GetInt("bpp", 0);
                    spaceconfiginfo.Commentpref = DNTRequest.GetInt("commentpref", 0);
                    spaceconfiginfo.MessagePref = DNTRequest.GetInt("messagepref", 0);
                    spaceconfiginfo.UpdateDateTime = spaceconfiginfo.CreateDateTime = DateTime.Now;
                    string rewritename = DNTRequest.GetFormString("rewritename").Trim();
                    if (rewritename != string.Empty)
                    {
                        if (Globals.CheckSpaceRewriteNameAvailable(rewritename) == 0)
                            spaceconfiginfo.Rewritename = rewritename;
                        else
                        {
                            AddErrLine("您输入的 个性域名 不可用或含有非法字符");
                            return;
                        }
                    }
                    else
                        spaceconfiginfo.Rewritename = "";

                    spaceconfiginfo.ThemeID = int.Parse(dr["themeid"].ToString());
                    spaceconfiginfo.ThemePath = dr["directory"].ToString();
                    spaceconfiginfo.PostCount = 0;
                    spaceconfiginfo.CommentCount = 0;
                    spaceconfiginfo.VisitedTimes = 0;
                    spaceconfiginfo.DefaultTab = 0;

                    string errorinfo = "";
                    int blogid = DbProvider.GetInstance().AddSpaceConfigData(spaceconfiginfo);
                    Users.UpdateUserSpaceId(-blogid, userid);

                    SpaceActiveConfigInfo _spaceconfiginfo = SpaceActiveConfigs.GetConfig();
                    if (_spaceconfiginfo.ActiveType == "0")
                    {
                        if (errorinfo == "")
                        {
                            SetUrl("index.aspx");
                            SetMetaRefresh();
                            SetShowBackLink(true);
                            AddMsgLine("您的申请已经提交，请等待管理员开通您的个人空间");
                        }
                        else
                        {
                            AddErrLine(errorinfo);
                            return;
                        }
                    }
                    else
                    {
                        Discuz.Data.DatabaseProvider.GetInstance().UpdateUserSpaceId(userid);
                        int tabid = Spaces.GetNewTabId(userid);
                        TabInfo tab = new TabInfo();
                        tab.TabID = tabid;
                        tab.UserID = userid;
                        tab.DisplayOrder = 0;
                        tab.TabName = "首页";
                        tab.IconFile = "";
                        tab.Template = "template_25_75.htm";
                        Spaces.AddTab(tab);
                        Spaces.AddLocalModule("builtin_calendarmodule.xml", userid, tabid, 1);
                        Spaces.AddLocalModule("builtin_statisticmodule.xml", userid, tabid, 1);
                        Spaces.AddLocalModule("builtin_postlistmodule.xml", userid, tabid, 2);
                        if (SpaceActiveConfigs.GetConfig().Spacegreeting != string.Empty)
                        {
                            SpacePostInfo spacepostsinfo = new SpacePostInfo();
                            spacepostsinfo.Title = string.Format("欢迎使用 {0} 个人空间", config.Forumtitle);
                            spacepostsinfo.Content = SpaceActiveConfigs.GetConfig().Spacegreeting;
                            spacepostsinfo.Category = string.Empty;
                            spacepostsinfo.PostStatus = 1;
                            spacepostsinfo.CommentStatus = 0;
                            spacepostsinfo.Postdatetime = DateTime.Now;
                            spacepostsinfo.Author = username;
                            spacepostsinfo.Uid = userid;
                            spacepostsinfo.PostUpDateTime = DateTime.Now;
                            spacepostsinfo.Commentcount = 0;
                            DbProvider.GetInstance().AddSpacePost(spacepostsinfo);
                        }
                        
                        ///添加最新主题到日志
                        List<TopicInfo> list = Topics.GetTopicsByUserId(userid, 1, config.Topictoblog, 0, 0);
                        foreach (TopicInfo mytopic in list)
                        {
                            int pid = Posts.GetFirstPostId(mytopic.Tid);
                            PostInfo post = Posts.GetPostInfo(mytopic.Tid, pid);
                            if (post != null && post.Message.Trim() != string.Empty)
                            {
                                SpacePostInfo spacepost = new SpacePostInfo();
                                spacepost.Author = username;
                                string content = Posts.GetPostMessageHTML(post, new AttachmentInfo[0]);
                                spacepost.Category = "";
                                spacepost.Content = content;
                                spacepost.Postdatetime = DateTime.Now;
                                spacepost.PostStatus = 1;
                                spacepost.PostUpDateTime = DateTime.Now;
                                spacepost.Title = post.Title;
                                spacepost.Uid = userid;
                                DbProvider.GetInstance().AddSpacePost(spacepost);
                            }
                        }
						SetUrl("space/");
						SetMetaRefresh();
						SetShowBackLink(true);
						AddMsgLine("恭喜您，您的个人空间已经被开通");
					}
				}
			}
			else
				spaceconfiginfo = BlogProvider.GetSpaceConfigInfo(userid);
		}	
	}
}
