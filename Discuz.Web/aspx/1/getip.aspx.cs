using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 查看IP信息页
    /// </summary>
    public class getip : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前版块名称
        /// </summary>
        public string forumname = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string posttitle = "";
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid = DNTRequest.GetInt("pid", 0);
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// IP地址
        /// </summary>
        public string ip = "";
        /// <summary>
        /// IP地址所在地查询结果
        /// </summary>
        public string iplocation = "";
        #endregion

        protected override void ShowPage()
        {
            if (postid == 0)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }

            PostInfo postInfo = Posts.GetPostInfo(topicid, postid);
            if (postInfo == null)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }

            ip = postInfo.Ip;
            iplocation = IpSearch.GetAddressWithIP(ip);


            // Edit By Cyrano，修复IP地址库不存在报错的逻辑
            // 如果数据库文件不存在
            if (iplocation == null)
                iplocation = "(IP数据库文件不存在,无法查询)";
            else if (iplocation == "") // 如果没有查到
                iplocation = "没有查询到该用户的地理所在地";
            else
                iplocation = iplocation.Replace("CZ88.NET", string.Empty).Trim();

            // 获取该主题的信息
            TopicInfo topic = Topics.GetTopicInfo(postInfo.Tid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            ForumInfo forum = Forums.GetForumInfo(postInfo.Fid);
            forumname = forum.Name;
            pagetitle = topic.Title;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            if (admininfo == null || admininfo.Allowviewip != 1)
            {
                AddErrLine("你没有查看IP的权限");
                return;
            }
            if (DNTRequest.GetString("action") == "ipban")
            {
                if (admininfo.Allowbanip != 1)
                {
                    AddErrLine("你无权禁止用户IP,请返回");
                    return;
                }
                if (Utils.InIPArray(DNTRequest.GetString("ip"), Utils.SplitString(config.Ipdenyaccess, "\n")))
                {
                    Users.UpdateUserGroup(postInfo.Posterid, 6);
                    AddErrLine("IP已在列表中存在,无需重复添加");
                    return;
                }
                if (GeneralConfigs.SetIpDenyAccess(DNTRequest.GetString("ip")))
                {
                    //调整用户到禁止IP组
                    Users.UpdateUserGroup(postInfo.Posterid, 6);

                    SetUrl(base.ShowTopicAspxRewrite(topic.Tid, 0));
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    MsgForward("getip_succeed");
                    base.AddMsgLine("IP已加入到用户禁止列表中");
                    base.ispost = true;
                }
                else
                {
                    base.AddErrLine("未知原因,IP无法加到禁止列表中");
                }
            }
        }
    }
}