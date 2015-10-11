using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 删除帖子页
    /// </summary>
    public class delpost : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid = DNTRequest.GetInt("postid", -1);
        /// <summary>
        /// 帖子信息
        /// </summary>
        public PostInfo post;
        /// <summary>
        /// 所属主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 所属主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// 所属主题标题
        /// </summary>
        public string topictitle = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 当前版块的分页id
        /// </summary>
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        /// <summary>
        /// 是否允许删除帖子, 初始false为不允许
        /// </summary>
        private bool allowDelPost = false;
        #endregion

        //private bool isModer = false;

        protected override void ShowPage()
        {
            if (postid == -1)
            {
                AddErrLine("无效的帖子ID");
                return;
            }

            // 获取该帖子的信息
            post = Posts.GetPostInfo(topicid, postid);
            if (post == null)
            {
                AddErrLine("不存在的帖子ID");
                return;
            }
            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }
            if (topicid != post.Tid)
            {
                AddErrLine("主题ID无效");
                return;
            }

            topictitle = topic.Title;
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name;
            pagetitle = string.Format("删除{0}", post.Title);
            forumnav = ShowForumAspxRewrite(forum.Pathlist.Trim(), forumid, forumpageid);

            if (!CheckPermission(post,DNTRequest.GetInt("opinion", -1)))  return;

            if (!allowDelPost)
            {
                AddErrLine("当前不允许删帖");
                return;
            }

            // 通过验证的用户可以删除帖子，如果是主题帖则另处理
            if (post.Layer == 0)
            {
                TopicAdmins.DeleteTopics(topicid.ToString(), byte.Parse(forum.Recyclebin.ToString()), false);
                //重新统计论坛帖数
                Forums.SetRealCurrentTopics(forum.Fid);
                ForumTags.DeleteTopicTags(topicid);
            }
            else
            {
                int reval;
                if (topic.Special == 4)
                {
                    if (DNTRequest.GetInt("opinion", -1) != 1 && DNTRequest.GetInt("opinion", -1) != 2)
                    {
                        AddErrLine("参数错误");
                        return;
                    }
                    reval = Posts.DeletePost(Posts.GetPostTableId(topicid), postid, false, true);
                    Debates.DeleteDebatePost(topicid, DNTRequest.GetInt("opinion", -1), postid);
                }
                else
                    reval = Posts.DeletePost(Posts.GetPostTableId(topicid), postid, false, true);

                // 删除主题游客缓存
                ForumUtils.DeleteTopicCacheFile(topicid);
                //再次确保回复数精确
                Topics.UpdateTopicReplyCount(topic.Tid);
                //更新指定版块的最新发帖数信息
                Forums.UpdateLastPost(forum);

                if (reval > 0 && Utils.StrDateDiffHours(post.Postdatetime, config.Losslessdel * 24) < 0)
                    UserCredits.UpdateUserCreditsByDeletePosts(post.Posterid);
            }

            SetUrl(post.Layer == 0 ? base.ShowForumAspxRewrite(post.Fid, 0) : Urls.ShowTopicAspxRewrite(post.Tid, 1));
            SetMetaRefresh();
            SetShowBackLink(false);
            AddMsgLine("删除帖子成功, 返回主题");
        }

        private bool CheckPermission(PostInfo post, int opinion)
        {
            //isModer = Moderators.IsModer(useradminid, userid, forumid);
            //if (userid == post.Posterid && !isModer)
            //{
            //    if (post.Layer < 1 && topic.Replies > 0)
            //    {
            //        AddErrLine("已经被回复过的主帖不能被删除");
            //        return false;
            //    }
            //    if ((config.Edittimelimit !=0 && Utils.StrDateDiffMinutes(post.Postdatetime, config.Edittimelimit) > 0) || post.Posterid != userid)   //不是作者或者超过编辑时限,0为不限时
            //    {
            //        AddErrLine(string.Format("已经超过了{0}分钟的编辑帖子时限,不能删除帖子",config.Edittimelimit));
            //        return false;
            //    }
            //    allowDelPost = true;
            //}
            //else
            //{
            //    AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            //    // 如果所属管理组有删帖的管理权限,并且是管理员或总版主
            //    if (admininfo != null && admininfo.Allowdelpost == 1 && Moderators.IsModer(useradminid, userid, forumid))
            //    {
            //        allowDelPost = true;
            //        if (post.Layer == 0)//管理者跳转至删除主题
            //            HttpContext.Current.Response.Redirect(string.Format("{0}topicadmin.aspx?action=moderate&operat=delete&forumid={1}&topicid={2}", forumpath, post.Fid, post.Tid));
            //        else    //跳转至批量删帖
            //            HttpContext.Current.Response.Redirect(string.Format("{0}topicadmin.aspx?action=moderate&operat=delposts&forumid={1}&topicid={2}&postid={3}&opinion={4}", forumpath, post.Fid, post.Tid, post.Pid, opinion));

            //        return false;
            //    }
            //    else
            //        allowDelPost = false;
            //}

            if (userid == post.Posterid)
            {
                if (post.Layer < 1 && topic.Replies > 0)
                {
                    AddErrLine("已经被回复过的主帖不能被删除");
                    return false;
                }
                if (config.Deletetimelimit == -1)
                {
                    AddErrLine("抱歉,系统不允许删除帖子");
                    return false;
                }
                if ((config.Deletetimelimit != 0 && Utils.StrDateDiffMinutes(post.Postdatetime, config.Deletetimelimit) > 0) || post.Posterid != userid)   //不是作者或者超过编辑时限,0为不限时
                {
                    AddErrLine(string.Format("已经超过了{0}分钟的删除帖子时限,不能删除帖子", config.Deletetimelimit));
                    return false;
                }
                allowDelPost = true;
                return true;
            }
            return false;
        }
    }
}
