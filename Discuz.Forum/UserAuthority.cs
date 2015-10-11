using System;
using System.Text;

using Discuz.Entity;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.Forum
{
    /// <summary>
    /// 用户权限操作类
    /// </summary>
    public class UserAuthority
    {
        /// <summary>
        /// 访问权限控制
        /// </summary>
        /// <param name="forum">访问的版块信息</param>
        /// <param name="usergroupinfo">当前用户的用户组信息</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public static bool VisitAuthority(ForumInfo forum, UserGroupInfo userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowViewByUserId(forum.Permuserlist, userId)) //判断当前用户在当前版块浏览权限
            {
                if (string.IsNullOrEmpty(forum.Viewperm))//当板块权限为空时，按照用户组权限
                {
                    if (userGroupInfo.Allowvisit != 1)
                    {
                        msg = "您当前的身份 \"" + userGroupInfo.Grouptitle + "\" 没有浏览该版块的权限";
                        return false;
                    }
                }
                else//当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, userGroupInfo.Groupid))
                    {
                        msg = "您没有浏览该版块的权限";
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 发帖权限控制
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <param name="usergroupinfo">当前用户的用户组信息</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public static bool PostAuthority(ForumInfo forum, UserGroupInfo userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowPostByUserID(forum.Permuserlist, userId)) //判断当前用户在当前版块发主题权限
            {
                if (string.IsNullOrEmpty(forum.Postperm))//权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有发表主题的权限
                    if (userGroupInfo.Allowpost != 1)
                    {
                        msg = "您当前的身份 \"" + userGroupInfo.Grouptitle + "\" 没有发表主题的权限";
                        return false;
                    }

                }
                else//权限设置不为空时,根据板块权限判断
                {
                    if (!Forums.AllowPost(forum.Postperm, userGroupInfo.Groupid))
                    {
                        msg = "您没有在该版块发表主题的权限";
                        return false;
                    }
                }
            }
            //当用户拥有发帖权限但版块只允许发布特殊主题时，需要判断用户是否能发布特殊主题
            if (forum.Allowspecialonly > 0)
            {
                //当版块设置了只允许特殊主题，但又没有开启任何特殊主题类型，则相当于关闭了版块的发主题功能
                if (forum.Allowpostspecial <= 0)
                {
                    msg = "您没有在该版块发表特殊主题的权限";
                    return false;
                }

                if ((forum.Allowpostspecial & 1) == 1 && userGroupInfo.Allowpostpoll != 1)
                    msg = "您当前的身份 \"" + userGroupInfo.Grouptitle + "\" 没有发布投票的权限";
                else
                    return true;

                if ((forum.Allowpostspecial & 4) == 4 && userGroupInfo.Allowbonus != 1)
                    msg = "您当前的身份 \"" + userGroupInfo.Grouptitle + "\" 没有发布悬赏的权限";
                else
                    return true;

                if ((forum.Allowpostspecial & 16) == 16 && userGroupInfo.Allowdebate != 1)
                    msg = "您当前的身份 \"" + userGroupInfo.Grouptitle + "\" 没有发起辩论的权限";
                else
                    return true;

                return false;
            }
            return true;
        }



        /// <summary>
        /// 上传附件权限控制
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <param name="usergroupinfo">当前用户的用户组信息</param>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        public static bool PostAttachAuthority(ForumInfo forum, UserGroupInfo userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowPostAttachByUserID(forum.Permuserlist, userId))
            {
                if (!Forums.AllowPostAttach(forum.Postattachperm, userGroupInfo.Groupid))
                {
                    msg = "您没有在该版块上传附件的权限";
                    return false;
                }
                else if (userGroupInfo.Allowpostattach != 1)
                {
                    msg = string.Format("您当前的身份 \"{0}\" 没有上传附件的权限", userGroupInfo.Grouptitle);
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 检查用户下载附件的权限
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <param name="userGroupInfo">当前用户的用户组信息</param>
        /// <param name="userId">当前用户Id</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public static bool CheckUsertAttachAuthority(ForumInfo forum, UserGroupInfo userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowGetAttachByUserID(forum.Permuserlist, userId))
            {
                if (Utils.StrIsNullOrEmpty(forum.Getattachperm) && userGroupInfo.Allowgetattach != 1)// 验证用户是否有下载附件的权限
                {
                    msg = string.Format("您当前的身份 \"{0}\" 没有下载或查看附件的权限", userGroupInfo.Grouptitle);
                }
                else
                {
                    if (!Forums.AllowGetAttach(forum.Getattachperm, userGroupInfo.Groupid))
                    {
                        msg = "您没有在该版块下载附件的权限";
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 检查用户是否在新手见习期
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool CheckNewbieSpan(int userId)
        {
            if (GeneralConfigs.GetConfig().Newbiespan > 0)
            {
                ShortUserInfo userInfo = Users.GetShortUserInfo(userId);
                string joindate = (userInfo != null) ? userInfo.Joindate : "";

                if (joindate == "" || Utils.StrDateDiffMinutes(joindate, GeneralConfigs.GetConfig().Newbiespan) < 0)
                    return true;
            } 
            return false;
        }

        public static bool CheckPostTimeSpan(UserGroupInfo userGroupInfo, AdminGroupInfo admininfo, OnlineUserInfo olUserInfo, ShortUserInfo shortUserInfo, ref string msg)
        {
            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (olUserInfo.Adminid != 1 && userGroupInfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postbanperiods, out visittime))
                {
                    msg = "在此时间段( " + visittime + " )内用户不可以发帖";
                    return false;
                }
            }

            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {
                int Interval = Utils.StrDateDiffSeconds(olUserInfo.Lastposttime, GeneralConfigs.GetConfig().Postinterval);
                if (Interval < 0)
                {
                    msg = "系统规定发帖间隔为" + GeneralConfigs.GetConfig().Postinterval.ToString() + "秒, 您还需要等待 " + (Interval * -1).ToString() + " 秒";
                    return false;
                }
                else if (olUserInfo.Userid != -1)
                {
                    //ShortUserInfo shortUserInfo = Discuz.Data.Users.GetShortUserInfo(olUserInfo.Userid);
                    string joindate = (shortUserInfo != null) ? shortUserInfo.Joindate : "";
                    if (joindate == "")
                    {
                        msg = "您的用户资料出现错误";
                        return false;
                    }
                    Interval = Utils.StrDateDiffMinutes(joindate, GeneralConfigs.GetConfig().Newbiespan);
                    if (Interval < 0)
                    {
                        msg = "系统规定新注册用户必须要在" + GeneralConfigs.GetConfig().Newbiespan.ToString() + "分钟后才可以发帖, 您还需要等待 " + (Interval * -1).ToString() + " 分钟";
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 是否允许发特殊主题
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <param name="type">特殊主题类型</param>
        /// <returns></returns>
        public static bool PostSpecialAuthority(ForumInfo forum, string type, ref string msg)
        {
            string[] special = { "" };
            if (forum.Allowpostspecial > 0)
            {
                if (type == "poll" && (forum.Allowpostspecial & 1) != 1)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表投票", forum.Name);
                    return false;
                }
                if (type == "bonus" && (forum.Allowpostspecial & 4) != 4)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表悬赏", forum.Name);
                    return false;
                }
                if (type == "debate" && (forum.Allowpostspecial & 16) != 16)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表辩论", forum.Name);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否允许发特殊主题
        /// </summary>
        /// <param name="UserGroupInfo">用户组信息</param>
        /// <param name="type">特殊主题类型</param>
        /// <returns></returns>
        public static bool PostSpecialAuthority(UserGroupInfo usergroupinfo, string type, ref string msg)
        {
            // 验证用户是否有发布投票的权限
            if (type == "poll" && usergroupinfo.Allowpostpoll != 1)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", usergroupinfo.Grouptitle);
                return false;
            }
            // 验证用户是否有发布悬赏的权限
            if (type == "bonus" && usergroupinfo.Allowbonus != 1)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发布悬赏的权限", usergroupinfo.Grouptitle);
                return false;
            }
            // 验证用户是否有发起辩论的权限
            if (type == "debate" && usergroupinfo.Allowdebate != 1)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", usergroupinfo.Grouptitle);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取主题帖是否可见信息
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <param name="useradminid">当前用户adminid</param>
        /// <param name="uid">当前用户id</param>
        /// <param name="userGroup">当前用户组信息</param>
        /// <param name="postinfo">帖子信息</param>
        /// <returns>0显示；1隐藏</returns>
        public static int GetTopicPostInvisible(ForumInfo forum, int useradminid, int uid, UserGroupInfo userGroup, PostInfo postinfo)
        {
            if (useradminid == 1 || Moderators.IsModer(useradminid, uid, forum.Fid))
                return 0;

            if (!ForumUtils.HasAuditWord(postinfo.Message) && forum.Modnewtopics == 0 && userGroup.ModNewTopics == 0 && !Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postmodperiods))
                return 0;

            return 1;
        }

        /// <summary>
        /// 发回复是否需要审核
        /// </summary>
        /// <param name="forum">主题所在的版块</param>
        /// <param name="useradminid">用户的管理组ID</param>
        ///<param name="topicInfo">所回复的主题信息</param>
        /// <param name="userid">用户ID</param>
        /// <param name="disablepost">是否受灌水限制</param>
        /// <returns>true需要审核；false不需要审核</returns>
        public static bool NeedAudit(ForumInfo forum, int useradminid, TopicInfo topicInfo, int userid, int disablepost, UserGroupInfo userGroup)
        {
            if (useradminid == 1 || Moderators.IsModer(useradminid, userid, forum.Fid))
                return false;
            if (Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postmodperiods))
                return true;
            if (forum.Modnewposts == 1 || userGroup.ModNewPosts == 1)
                return true;
            else if (topicInfo.Displayorder == -2)
                return true;
            return false;

            //bool needaudit = false; //是否需要审核
            //if (Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postmodperiods))
            //{
            //    needaudit = true;
            //}
            //else
            //{
            //    if (forum.Modnewposts == 1 && useradminid != 1)
            //    {
            //        if (useradminid > 1)
            //        {
            //            if (disablepost == 1 && topicInfo.Displayorder != -2)
            //            {
            //                if (useradminid == 3 && !Moderators.IsModer(useradminid, userid, forum.Fid))
            //                    needaudit = true;
            //            }
            //            else
            //                needaudit = true;
            //        }
            //        else
            //            needaudit = true;
            //    }
            //    else if (userGroup.ModNewPosts == 1 && !Moderators.IsModer(useradminid, userid, forum.Fid) && useradminid != 1)
            //        needaudit = true;
            //    else if (useradminid != 1 && topicInfo.Displayorder == -2)
            //        needaudit = true;
            //}
            //return needaudit;
        }

        /// <summary>
        /// 发主题是否需要审核
        /// </summary>
        /// <param name="forum">主题所在的版块</param>
        /// <param name="useradminid">用户的管理组ID</param>
        /// <param name="userid">用户ID</param>
        /// <param name="userGroup">当前用户的用户组</param>
        /// <returns>true需要审核；false不需要审核</returns>
        public static bool NeedAudit(ForumInfo forum, int useradminid, int userid, UserGroupInfo userGroup)
        {
            if (useradminid == 1 || Moderators.IsModer(useradminid, userid, forum.Fid))
                return false;
            if (Scoresets.BetweenTime(GeneralConfigs.GetConfig().Postmodperiods) || forum.Modnewtopics == 1 || userGroup.ModNewTopics == 1)
                return true;
            return false;
        }

        public static bool PostReply(ForumInfo forum, int userid, UserGroupInfo usergroupinfo, TopicInfo topic)
        {
            bool canreply = (usergroupinfo.Radminid == 1);
            //是否有回复的权限
            if (topic.Closed == 0)
            {
                if (userid > -1 && Forums.AllowReplyByUserID(forum.Permuserlist, userid))
                {
                    canreply = true;
                }
                else
                {
                    if (Utils.StrIsNullOrEmpty(forum.Replyperm)) //权限设置为空时，根据用户组权限判断
                    {
                        // 验证用户是否有发表主题的权限
                        if (usergroupinfo.Allowreply == 1)
                            canreply = true;
                    }
                    else if (Forums.AllowReply(forum.Replyperm, usergroupinfo.Groupid))
                        canreply = true;
                }
            }
            return canreply;
        }


        public static bool DownloadAttachment(ForumInfo forum, int userid, UserGroupInfo usergroupinfo)
        {
            bool allowdownloadattach = false;
            //当前用户是否有允许下载附件权限
            if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userid))
            {
                allowdownloadattach = true;
            }
            else
            {
                if (Utils.StrIsNullOrEmpty(forum.Getattachperm)) //权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有有允许下载附件权限
                    if (usergroupinfo.Allowgetattach == 1)
                        allowdownloadattach = true;
                }
                else if (Forums.AllowGetAttach(forum.Getattachperm, usergroupinfo.Groupid))
                    allowdownloadattach = true;
            }
            return allowdownloadattach;
        }

        /// <summary>
        /// 是否有编辑帖子的权限
        /// </summary>
        /// <param name="postInfo"></param>
        /// <param name="userId"></param>
        /// <param name="userAdminId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool CanEditPost(PostInfo postInfo, int userId, int userAdminId, ref string msg)
        {
            //非创始人且作者与当前编辑者不同时
            if (postInfo.Posterid != userId && BaseConfigs.GetFounderUid != userId)
            {
                // Edit By Cyrano, 忽视创始人逻辑
                //if (postInfo.Posterid == BaseConfigs.GetFounderUid)
                //{
                //    msg = "您无权编辑创始人的帖子";
                //    return false;
                //}

                // Edit By Cyrano, 忽视管理组的等级限定
                //if (postInfo.Posterid != -1)
                //{
                //    UserGroupInfo postergroup = UserGroups.GetUserGroupInfo(Users.GetShortUserInfo(postInfo.Posterid).Groupid);
                //    if (postergroup.Radminid > 0 && postergroup.Radminid < userAdminId)
                //    {
                //        msg = "您无权编辑更高权限人的帖子";
                //        return false;
                //    }
                //}
            }
            return true;
        }

        /// <summary>
        /// 搜索权限判断
        /// </summary>
        /// <param name="usergroupinfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Search(UserGroupInfo usergroupinfo, ref string msg)
        {
            if (usergroupinfo.Allowsearch == 0)
            {
                msg = "您当前的身份 " + usergroupinfo.Grouptitle + " 没有搜索的权限";
                return false;
            }

            if (usergroupinfo.Allowsearch == 2 && DNTRequest.GetInt("keywordtype", 0) == 1)
            {
                msg = "您当前的身份 " + usergroupinfo.Grouptitle + " 没有全文搜索的权限";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 搜索权限判断
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="lastsearchtime"></param>
        /// <param name="useradminid"></param>
        /// <param name="usergroupinfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Search(int userid, string lastsearchtime, int useradminid, UserGroupInfo usergroupinfo, ref string msg)
        {
            //　如果当前用户非管理员并且论坛设定了禁止全文搜索时间段，当前时间如果在其中的一个时间段内，不允许全文搜索
            if (useradminid != 1 && DNTRequest.GetInt("keywordtype", 0) == 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(GeneralConfigs.GetConfig().Searchbanperiods, out visittime))
                {
                    msg = "在此时间段( " + visittime + " )内用户不可以进行全文搜索";
                    return false;
                }
            }

            if (useradminid != 1)
            {
                //判断一分钟内搜索的次数是不是超过限制值
                if (!Statistics.CheckSearchCount(GeneralConfigs.GetConfig().Maxspm))
                {
                    msg = "抱歉,系统在一分钟内搜索的次数超过了系统安全设置的上限,请稍候再试";
                    return false;
                }

                int Interval = Utils.StrDateDiffSeconds(lastsearchtime, GeneralConfigs.GetConfig().Searchctrl);
                if (Interval <= 0)
                {
                    msg = "系统规定搜索间隔为" + GeneralConfigs.GetConfig().Searchctrl + "秒, 您还需要等待 " + (Interval * -1) + " 秒";
                    return false;
                }

                //不是管理员，则如果设置搜索扣积分时扣除用户积分
                if (UserCredits.UpdateUserCreditsBySearch(userid) == -1)
                {
                    string addExtCreditsTip = "";
                    if (EPayments.IsOpenEPayments())
                        addExtCreditsTip = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
                    msg = "您的积分不足, 不能执行搜索操作" + addExtCreditsTip;
                    return false;
                }
            }
            return true;
        }
    }
}
