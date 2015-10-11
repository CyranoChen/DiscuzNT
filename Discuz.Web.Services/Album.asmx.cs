using System;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Entity;

namespace Discuz.Web.services
{
    /// <summary>
    /// Summary description for Album
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Album : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region 证书信息类
        /// <summary>
        /// 证书信息类
        /// </summary>
        public class CredentialInfo
        {
            private string m_authToken;
            /// <summary>
            /// 安全令牌
            /// </summary>
            public string AuthToken
            {
                get { return m_authToken; }
                set { m_authToken = value; }
            }

            private int m_userID;
            /// <summary>
            /// 当前用户id
            /// </summary>
            public int UserID
            {
                get { return m_userID; }
                set { m_userID = value; }
            }

            private string m_passWord;
            /// <summary>
            /// 当前用户口令（密文）
            /// </summary>
            public string Password
            {
                get { return m_passWord; }
                set { m_passWord = value; }
            }
        }
        #endregion

        /// <summary>
        /// WEB权限认证
        /// </summary>
        /// <param name="creinfo">认证信息</param>
        /// <returns>是否通过验正</returns>
        private bool AuthenticateUser(CredentialInfo creinfo)
        {
            if (creinfo.UserID > 0)
            {
                int olid = Discuz.Forum.OnlineUsers.GetOlidByUid(creinfo.UserID);
                if (olid > 0)
                {
                    OnlineUserInfo oluserinfo = Discuz.Forum.OnlineUsers.GetOnlineUser(olid);
                    if (oluserinfo.Userid == creinfo.UserID && Utils.UrlEncode(Discuz.Forum.ForumUtils.SetCookiePassword(oluserinfo.Password.Trim(), GeneralConfigs.GetConfig().Passwordkey)) == creinfo.Password)//检测用户id和口令                  
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [WebMethod]
        public List<ShowtopicPageAttachmentInfo> GetAttachList(int topicid, int forumid, string onlyauthor, int posterid, CredentialInfo creinfo)
        {
            TopicInfo topic = Topics.GetTopicInfo(topicid);
            ForumInfo forum = Forums.GetForumInfo(forumid);
            UserInfo userinfo = new UserInfo();
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(7);//默认为游客
            if (AuthenticateUser(creinfo))
            {
                userinfo = Users.GetUserInfo(creinfo.UserID);
                usergroupinfo = UserGroups.GetUserGroupInfo(userinfo.Groupid);
            }
            // 取得用户权限id,1管理员,2超版,3版主,0普通组,-1特殊组
            int ismoder = Moderators.IsModer(usergroupinfo.Radminid, userinfo.Uid, forumid) ? 1 : 0;
            int price = GetTopicPrice(topic, creinfo, ismoder);

            return GetAttachList(price, onlyauthor, ismoder, posterid, userinfo, usergroupinfo, topic, forum);
        }


        /// <summary>
        /// 获取主题价格
        /// </summary>
        /// <param name="topicInfo"></param>
        /// <returns></returns>
        private int GetTopicPrice(TopicInfo topicInfo,  CredentialInfo creinfo, int ismoder)
        {
            int price = 0;
            if (topicInfo.Special == 0)//普通主题
            {
                //购买帖子操作
                //判断是否为购买可见帖, price=0为非购买可见(正常), price>0 为购买可见, price=-1为购买可见但当前用户已购买                
                if (topicInfo.Price > 0 && creinfo.UserID != topicInfo.Posterid && ismoder != 1)
                {
                    price = topicInfo.Price;
                    //时间乘以-1是因为当Configs.GetMaxChargeSpan()==0时,帖子始终为购买帖
                    if (PaymentLogs.IsBuyer(topicInfo.Tid, creinfo.UserID) ||(Utils.StrDateDiffHours(topicInfo.Postdatetime, Scoresets.GetMaxChargeSpan()) > 0 &&
                         Scoresets.GetMaxChargeSpan() != 0)) //判断当前用户是否已经购买
                    {
                        price = -1;
                    }
                }
            }
            return price;
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
        /// 获取帖子参数信息(PostPramsInfo)
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        private List<ShowtopicPageAttachmentInfo> GetAttachList(int price, string onlyauthor, int ismoder, int posterid, UserInfo userinfo, UserGroupInfo usergroupinfo, TopicInfo topic, ForumInfo forum)
        {
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            //获取当前页主题列表
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topic.Tid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = 10000;     // 得到Ppp设置
            postpramsInfo.Pageindex = 1;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupinfo.Groupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Price = price;
            postpramsInfo.Usergroupreadaccess = (ismoder == 1) ? int.MaxValue : usergroupinfo.Readaccess;
            postpramsInfo.CurrentUserid = userinfo.Uid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            postpramsInfo.Topicinfo = topic;
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            postpramsInfo.Hide = (topic.Hide == 1 && (Posts.IsReplier(topic.Tid, userinfo.Uid) || ismoder == 1)) ? -1 : 1;
            postpramsInfo.Hide = topic.Posterid == userinfo.Uid ? -2 : postpramsInfo.Hide;
            postpramsInfo.Condition = Posts.GetPostPramsInfoCondition(onlyauthor, topic.Tid, posterid);
            postpramsInfo.Usercredits = userinfo == null ? 0 : userinfo.Credits;
            List<ShowtopicPageAttachmentInfo> attachmentlist = new List<ShowtopicPageAttachmentInfo>();
            List<ShowtopicPagePostInfo> postlist = GetPostList(postpramsInfo, out attachmentlist, ismoder == 1);
            int allowGetAttach = GetAllowGetAttachValue(postpramsInfo);
            foreach (ShowtopicPageAttachmentInfo showtopicpageattachinfo in attachmentlist)
            {
                if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userinfo.Uid))
                {
                    showtopicpageattachinfo.Getattachperm = 1;
                    showtopicpageattachinfo.Allowread = 1;
                }
            }
            List<ShowtopicPageAttachmentInfo> attachDeleteList = new List<ShowtopicPageAttachmentInfo>();
            foreach (ShowtopicPageAttachmentInfo attachInfo in attachmentlist)
            {
                if (allowGetAttach == 1 && attachInfo.Allowread == 1)
                {
                    if (attachInfo.Filetype.IndexOf("jpeg") >= 0 || attachInfo.Filetype.IndexOf("png") >= 0)
                    {
                        if (!attachInfo.Filename.ToLower().StartsWith("http"))
                            attachInfo.Filename = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "upload/" + attachInfo.Filename.Trim();
                    }
                    else
                        attachDeleteList.Add(attachInfo);//记录不是JPG或PNG的图片，以便进行remove操作
                }
                else
                    attachDeleteList.Add(attachInfo);//记录不是JPG或PNG的图片，以便进行remove操作
            }
            foreach (ShowtopicPageAttachmentInfo attach in attachDeleteList)
            {
                attachmentlist.Remove(attach);
            }
            return attachmentlist;
        }


        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachList, bool isModer)
        {
            List<ShowtopicPagePostInfo> postList = Data.Posts.GetPostList(postpramsInfo);
            attachList = new List<ShowtopicPageAttachmentInfo>();
         
            //进行相应帖子信息设置
            string pidList = Posts.GetPidListWithAttach(postList);
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

        private static Regex regexAttach = new Regex(@"\[attach\](\d+?)\[\/attach\]", RegexOptions.IgnoreCase);

        private static Regex regexHide = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);

        private static Regex regexAttachImg = new Regex(@"\[attachimg\](\d+?)\[\/attachimg\]", RegexOptions.IgnoreCase);

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
            if (!Utils.InArray(postInfo.Groupid.ToString(), "4,5,6"))
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
                    string[] attHidArray = Posts.GetHiddenAttachIdList(postpramsInfo.Sdetail, postpramsInfo.Hide);
                    List<ShowtopicPageAttachmentInfo> attachDeleteList = new List<ShowtopicPageAttachmentInfo>();
                    foreach (ShowtopicPageAttachmentInfo attach in attachList)
                    {
                        message = Attachments.GetMessageWithAttachInfo(postpramsInfo, allowGetAttach, attHidArray, postInfo, attach, message);
                        if ((postpramsInfo.CurrentUserGroup.Radminid == 1 || attach.Uid == postpramsInfo.CurrentUserid || attach.Attachprice <= 0 || attach.Isbought == 1)//当为发帖人或不为收费附件或已购买该收费附件时
                            || Utils.InArray(attach.Aid.ToString(), attHidArray))
                        {
                            ;                           
                        }
                        else
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
            else//发帖人已经被禁止发言
            {
                if (isModer)
                {
                    postInfo.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + postInfo.Message;
                }
                else
                {
                    postInfo.Message = "该用户帖子内容已被屏蔽";
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
            }
        }
    }
}
