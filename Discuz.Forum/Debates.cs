using System;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Data;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    public class Debates
    {
        /// <summary>
        /// 获取帖子观点
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>Dictionary泛型</returns>
        public static Dictionary<int, int> GetPostDebateList(int tid)
        {
            if (tid <= 0)
                return null;

            return Discuz.Data.Debates.GetPostDebateList(tid);
        }

        /// <summary>
        /// 获取辩论的扩展信息
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>辩论主题扩展信息</returns>
        public static DebateInfo GetDebateTopic(int tid)
        {
            if (tid <= 0)
                return null;

            return Discuz.Data.Debates.GetDebateTopic(tid);
        }

        /// <summary>
        /// 更新辩论信息
        /// </summary>
        /// <param name="debateInfo">辩论信息</param>
        /// <returns></returns>
        public static bool UpdateDebateTopic(DebateInfo debateInfo)
        {
            if (debateInfo.Tid <= 0)
                return false;
            return Discuz.Data.Debates.UpdateDebateTopic(debateInfo);
        }

        /// <summary>
        /// 返回调用的JSON数据
        /// </summary>
        /// <param name="callback">JS回调函数</param>
        /// <param name="tidlist">主题ID列表</param>
        /// <returns>JS数据</returns>
        public static string GetDebatesJsonList(string callback, string tidlist)
        {
            switch (callback)
            {
                //获取热帖信息
                case "gethotdebatetopic":
                    {
                        string[] debatesrule = Utils.StrIsNullOrEmpty(tidlist) ? new string[0] : tidlist.Split(',');

                        if (debatesrule.Length < 3)
                            break;
                        else if (debatesrule[0] != "views" && debatesrule[0] != "replies" && Utils.IsNumeric(debatesrule[1]) && Utils.IsNumeric(debatesrule[2]))
                            break;

                        return Discuz.Data.Debates.GetDebatesJsonList(callback, debatesrule[0], TypeConverter.StrToInt(debatesrule[1]), TypeConverter.StrToInt(debatesrule[2]));
                    }

                //获取推荐辩论帖帖信息
                case "recommenddebates":
                    {
                        if (!Utils.IsNumericList(tidlist))
                            break;

                        if (Utils.StrIsNullOrEmpty(tidlist))
                            tidlist = GeneralConfigs.GetConfig().Recommenddebates;

                        return Discuz.Data.Debates.GetRecommendDebates(callback, tidlist);
                    }

                default:
                    break;
            }
            return "0";
        }
        /// <summary>
        /// 添加点评
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="message">点评内容</param>
        public static StringBuilder CommentDabetas(int tid, string message, bool ispost)
        {
            StringBuilder xmlnode = IsValidDebates(tid, message, ispost);
            if (!xmlnode.ToString().Contains("<error>"))
            {
                xmlnode.Append("<message>" + message + "</message>");
                Discuz.Data.Debates.CommentDabetas(tid, TypeConverter.ObjectToInt(Data.PostTables.GetPostTableId(tid)), Utils.HtmlEncode(ForumUtils.BanWordFilter(message)));
            }
            return xmlnode;
        }

        private static StringBuilder IsValidDebates(int tid, string message, bool ispost)
        {
            StringBuilder xmlnode = new StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }

            Regex r = new Regex(@"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(message);
            if (m.Count == 0)
            {
                xmlnode.Append("<error>评论内容不能为空</error>");
                return xmlnode;
            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);
            if (tid == 0 || topicinfo.Special != 4)
            {
                xmlnode.Append("<error>本主题不是辩论帖，无法点评</error>");
                return xmlnode;
            }
            if (Debates.GetDebateTopic(tid).Terminaltime > DateTime.Now)
            {
                xmlnode.Append("<error>本辩论帖结束时间未到，无法点评</error>");
                return xmlnode;
            }
            return xmlnode;
        }


        /// <summary>
        /// 验证用户组是否允许顶
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="tips">提示信息</param>
        /// <returns>是否可以顶</returns>
        public static bool AllowDiggs(int userid)
        {
            //判断游客是否可以顶
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 0 && userid == -1)
                return false;

            //判断当前用户是否可以顶
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(Discuz.Data.Users.GetUserInfo(userid).Groupid);
            if (usergroupinfo.Allowdiggs == 0)
                return false;

            return true;
        }


        /// <summary>
        /// 增加Digg
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="type">正反方观点</param>
        /// <param name="userid">用户ID</param>
        public static void AddDebateDigg(int tid, int pid, int type, int userid)
        {
            if (userid < 0)
                return;

            UserInfo userinfo = Discuz.Data.Users.GetUserInfo(userid);
            if (userinfo == null)
                return;

            Discuz.Data.Debates.AddDebateDigg(tid, pid, type, Utils.GetRealIP(), userinfo);
        }

        /// <summary>
        /// 判断是否顶过
        /// </summary>
        /// <param name="pid">帖子ID</param>
        /// <param name="userid">用户ID</param>
        /// <returns>判断是否顶过</returns>
        public static bool IsDigged(int pid, int userid)
        {
            //开放游客后，验证方式为松散验证,24小时内只能顶一次
            if (UserGroups.GetUserGroupInfo(7).Allowdiggs != 1)
                return !DatabaseProvider.GetInstance().AllowDiggs(pid, userid);
            else
            {
                if (Utils.StrIsNullOrEmpty(Utils.GetCookie("debatedigged")))
                    return false;

                foreach (string s in Utils.GetCookie("debatedigged").Split(','))
                {
                    if (pid == Utils.StrToInt(s, 0))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 写入已顶的COOKIES
        /// </summary>
        /// <param name="pid">帖子ID</param>
        public static void WriteCookies(int pid)
        {
            if (Utils.StrIsNullOrEmpty((Utils.GetCookie("debatedigged"))))
                Utils.WriteCookie("debatedigged", pid.ToString(), 1440);
            else
                Utils.WriteCookie("debatedigged", Utils.GetCookie("debatedigged") + "," + pid, 1440);
        }

        /// <summary>
        /// 返回辩论主题的帖子一方的帖子数
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="debateOpinion">帖子观点</param>
        /// <returns>帖子数</returns>
        public static int GetDebatesPostCount(PostpramsInfo postpramsInfo, int debateOpinion)
        {
            return Discuz.Data.Debates.GetDebatesPostCount(postpramsInfo.Tid, debateOpinion);
        }

        /// <summary>
        /// 获取辩论正方帖子列表
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="attachmentlist">附件列表</param>
        /// <param name="ismoder">是否有管理权限</param>
        /// <returns>正方帖子列表</returns>
        public static List<ShowtopicPagePostInfo> GetPositivePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 1, new PostOrderType());
        }

        private static List<ShowtopicPagePostInfo> GetDebatePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachList, 
            bool isModer, int debateOpinion, PostOrderType postOrderType)
        {
            List<ShowtopicPagePostInfo> postList = new List<ShowtopicPagePostInfo>();
            attachList = new List<ShowtopicPageAttachmentInfo>();
            StringBuilder attachmentpidlist = new StringBuilder();
            StringBuilder pidList = new StringBuilder();
            postList = Data.Debates.GetDebatePostList(postpramsInfo, debateOpinion, postOrderType);

            //当因冗余字段不准导致未取得分页信息时，修正冗余字段，并取最后一页
            if (postList.Count == 0 && postpramsInfo.Pageindex > 1)
            {
                int postcount = Data.Debates.GetRealDebatePostCount(postpramsInfo.Tid, debateOpinion);

                postpramsInfo.Pageindex = postcount % postpramsInfo.Pagesize == 0 ? postcount / postpramsInfo.Pagesize : postcount / postpramsInfo.Pagesize + 1;

                postList = Data.Debates.GetDebatePostList(postpramsInfo, debateOpinion, postOrderType);
            }

            StringBuilder attachPidList = new StringBuilder();

            foreach (ShowtopicPagePostInfo post in postList)
            {
                pidList.AppendFormat("{0},", post.Pid);
                if (post.Attachment > 0)
                    attachPidList.AppendFormat("{0},", post.Pid);
            }

            attachList = Attachments.GetAttachmentList(postpramsInfo, attachPidList.ToString().TrimEnd(','));

            Dictionary<int, int> postdiggs = GetPostDiggs(pidList.ToString().Trim(','));
            foreach (ShowtopicPagePostInfo post in postList)
            {
                if (postdiggs.ContainsKey(post.Pid))
                    post.Diggs = postdiggs[post.Pid];
            }

            Posts.ParsePostListExtraInfo(postpramsInfo, attachList, isModer, postList);

            return postList;
        }
        /// <summary>
        /// 返回帖子被顶数
        /// </summary>
        /// <param name="pidlist">帖子ID数组</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, int> GetPostDiggs(string pidlist)
        {
            if (!Utils.IsNumericList(pidlist))
                return new Dictionary<int, int>();

            return Discuz.Data.Debates.GetPostDiggs(pidlist);
        }

        /// <summary>
        /// 反方的帖子列表
        /// </summary>
        /// <param name="postpramsInfo">帖子的附加信息</param>
        /// <param name="attachmentlist">附件列表</param>
        /// <param name="ismoder">是否有管理权限</param>
        /// <returns>反方帖子列表</returns>
        public static List<ShowtopicPagePostInfo> GetNegativePostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 2, new PostOrderType());
        }
     

        /// <summary>
        /// 删除辩论帖子信息
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="opinion">正反方字段，1：正方 2：反方</param>
        /// <param name="pid">帖子Id</param>
        public static void DeleteDebatePost(int tid, int opinion, int pid)
        {
            switch (opinion)
            {
                case 1: Discuz.Data.Debates.DeleteDebatePost(tid, "positivediggs", pid); break;
                case 2: Discuz.Data.Debates.DeleteDebatePost(tid, "negativediggs", pid); break;
            }
        }
    }
}
