using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;
using Discuz.Plugin.Space;
using Newtonsoft.Json;

namespace Discuz.Web.UI
{
    /// <summary>
    /// Ajax相关功能操作类
    /// </summary>
    public class SessionAjaxPage : PageBase
    {
        protected override void ShowPage()
        {
            ////如果是Flash提交
            //if (Utils.StrIsNullOrEmpty(DNTRequest.GetUrlReferrer()))
            //{
            //    string[] input = DecodeUid(DNTRequest.GetString("input")).Split(','); //下标0为Uid，1为Olid
            //    UserInfo userInfo = Users.GetUserInfo(TypeConverter.StrToInt((input[0])));
            //    if (userInfo == null || DNTRequest.GetString("appid") != Utils.MD5(userInfo.Username + userInfo.Password + userInfo.Uid + input[1]))
            //        return;
            //}
            //if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost())) //如果是跨站提交...
            //    return;
            string type = DNTRequest.GetString("t");
            switch (type)
            {
                case "forumtree":
                    GetForumTree();		//获得指定版块的子版块信息,以xml文件输出
                    break;
                case "topictree":
                    GetTopicTree();		//获得指定主题的回复信息,以xml文件输出
                    break;
                //case "checkusername":
                //    CheckUserName();    //检查用户名是否存在
                //    break;
                case "quickreply":
                    QuickReply();	//快速回复主题
                    break;
                case "report":  //举报功能
                    Report();
                    break;
                //case "album":  //相册
                //    GetAlbum();
                //    break;
                //case "checkrewritename":
                //    CheckRewriteName();
                //    break;
                //case "ratelist":
                //    GetRateLogList();	//帖子评分记录
                //    break;
                //case "smilies":
                //    GetSmilies();
                //    break;
                //case "relatekw":
                //    GetRelateKeyword();
                //    break;
                //case "gettopictags":
                //    GetTopicTags();
                //    break;
                //case "topicswithsametag":
                //    GetTopicsWithSameTag();
                //    break;
                //case "getforumhottags":
                //    GetForumHotTags();
                //    break;
                //case "getspaceposttags":
                //    GetSpacePostTags();
                //    break;
                //case "getspacehottags":
                //    GetSpaceHotTags();
                //    break;
                //case "getphototags":
                //    GetPhotoTags();
                //    break;
                //case "getphotohottags":
                //    GetPhotoHotTags();
                //    break;
                //case "getgoodstradelog":
                //    GetGoodsTradeLog(DNTRequest.GetInt("goodsid", 0), DNTRequest.GetInt("pagesize", 0), DNTRequest.GetInt("pageindex", 0), DNTRequest.GetString("orderby", true), DNTRequest.GetInt("ascdesc", 1));
                //    break;
                //case "getgoodsleavewordbyid":
                //    GetGoodsLeaveWordById(DNTRequest.GetInt("leavewordid", 0));
                //    break;
                //case "getgoodsleaveword":
                //    GetGoodsLeaveWord(DNTRequest.GetInt("goodsid", 0), DNTRequest.GetInt("pagesize", 0), DNTRequest.GetInt("pageindex", 0));
                //    break;
                //case "ajaxgetgoodsratelist":
                //    GetGoodsRatesList(DNTRequest.GetInt("uid", 0), DNTRequest.GetInt("uidtype", 0), DNTRequest.GetInt("ratetype", 0), DNTRequest.GetString("filter", true));
                //    break;
                //case "getmallhottags":
                //    GetMallHotTags();
                //    break;
                //case "gethotgoods":
                //    GetHotGoods(DNTRequest.GetInt("days", 0), DNTRequest.GetInt("categoryid", 0), DNTRequest.GetInt("count", 0));
                //    break;
                //case "getshopinfo": //获取热门或新开的店铺信息
                //    GetShopInfoJson(DNTRequest.GetInt("shoptype", 0));
                //    break;
                //case "getgoodslist":
                //    GetGoodsList(DNTRequest.GetInt("categoryid", 0), DNTRequest.GetInt("order", 0), DNTRequest.GetInt("topnumber", 0));
                //    break;
                //case "gethotdebatetopic":
                //    Getdebatesjsonlist("gethotdebatetopic", DNTRequest.GetString("tidlist", true));
                //    break;
                //case "recommenddebates":
                //    Getdebatesjsonlist("recommenddebates", DNTRequest.GetString("tidlist", true));
                //    break;
                //case "addcommentdebates":
                //    ResponseXML(Debates.CommentDabetas(DNTRequest.GetInt("tid", 0), DNTRequest.GetString("commentdebates", true), ispost));
                //    break;
                case "diggdebates":
                    DiggDebates();
                    break;
                case "getdebatepostpage":
                    GetDebatePostPage();
                    break;

                //case "getpostinfo":
                //    GetPostInfo();
                //    break;
                //case "getattachpaymentlog"://获取指定符件id的附件交易日志
                //    GetAttachPaymentLogByAid(DNTRequest.GetInt("aid", 0));
                //    break;
                case "checkuserextcredit":
                    CheckUserExtCredit(DNTRequest.GetInt("aid", 0));
                    break;
                case "confirmbuyattach"://购买指定附件
                    ConfirmBuyAttach(DNTRequest.GetInt("aid", 0));
                    break;

                case "getnewpms":
                    GetNewPms();//获得新短消息内容
                    break;
                case "getnewnotifications":
                    GetNewNotifications();
                    break;

                case "getajaxforums":
                    GetAjaxForumsJsonList();
                    break;

                //case "deletefriendship":
                //    DeleteFriendship();
                //    break;

                //case "passfriendshiprequest":
                //    PassFriendshipRequest();
                //    break;

                //case "ignorefriendshiprequest":
                //    IgnoreFriendshipRequest();
                //    break;

                //case "getuserfriendshiprequestcount":
                //    GetUserFriendshipRequestCount();
                //    break;

                //case "getuserfriendshipcount":
                //    GetUserFriendshipCount();
                //    break;

                //case "ignoreappinvite":
                //    IgnoreApplicationInvite();
                //    break;

                //case "updatefriendgroup":
                //    UpdateFriendGroup();
                //    break;

                //case "createfriendgrouop":
                //    CreateNewFriendGroup();
                //    break;

                //case "getfriendgroupjson":
                //    GetFriendGroupJson();
                //    break;

                //case "getfriendsjsonlist":
                //    GetFriendsJsonList();
                //    break;

                //case "getlastfrienditemonpage":
                //    GetLastFriendItemOnPage(DNTRequest.GetInt("pageid", 0), DNTRequest.GetInt("pagesize", 0), DNTRequest.GetString("fusername", true), DNTRequest.GetString("lastdatetime", true), DNTRequest.GetInt("groupid", -1));
                //    break;
                case "passpost":
                case "deletepost":
                case "ignorepost":
                case "passtopic":
                case "ignoretopic":
                case "deletetopic":
                    AuditPost(type, DNTRequest.GetString("reason"));
                    break; ;
                case "deletepostsbyuidanddays":
                    DeletePostsByUidAndDays(DNTRequest.GetInt("uid", 0), 7);
                    break;
                case "getattachlist":
                    GetAttachList();
                    break;
                case "deleteattach":
                    DeleteAttach();
                    break;
                case "imagelist":
                    GetImageList();
                    break;

            }


            //if (DNTRequest.GetString("Filename") != "" && DNTRequest.GetString("Upload") != "")
            //{
            //    string uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0];
            //    ResponseText(UploadTempAvatar(uid));
            //    return;
            //}
            //if (DNTRequest.GetString("avatar1") != "" && DNTRequest.GetString("avatar2") != "" && DNTRequest.GetString("avatar3") != "")
            //{
            //    string uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0];
            //    CreateDir(uid);
            //    if (!(SaveAvatar("avatar1", uid) && SaveAvatar("avatar2", uid) && SaveAvatar("avatar3", uid)))
            //    {
            //        File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
            //        ResponseText("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
            //        return;
            //    }
            //    File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
            //    ResponseText("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
            //    return;
            //}
        }

        //#region 头像
        ///// <summary>
        ///// 解码Uid
        ///// </summary>
        ///// <param name="encodeUid"></param>
        ///// <returns></returns>
        //private string DecodeUid(string encodeUid)
        //{
        //    return DES.Decode(encodeUid.Replace(' ', '+'), config.Passwordkey);
        //}

        ///// <summary>
        ///// 创建文件夹
        ///// </summary>
        ///// <param name="uid"></param>
        //private void CreateDir(string uid)
        //{
        //    uid = Avatars.FormatUid(uid);
        //    string avatarDir = string.Format("{0}avatars/upload/{1}/{2}/{3}",
        //        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2));
        //    if (!Directory.Exists(Utils.GetMapPath(avatarDir)))
        //        Directory.CreateDirectory(Utils.GetMapPath(avatarDir));
        //}

        ///// <summary>
        ///// 保存头像文件
        ///// </summary>
        ///// <param name="avatar"></param>
        ///// <param name="uid"></param>
        ///// <returns></returns>
        //private bool SaveAvatar(string avatar, string uid)
        //{
        //    byte[] b = FlashDataDecode(DNTRequest.GetString(avatar));
        //    if (b.Length == 0)
        //        return false;
        //    uid = Avatars.FormatUid(uid);
        //    string size = "";
        //    if (avatar == "avatar1")
        //        size = "large";
        //    else if (avatar == "avatar2")
        //        size = "medium";
        //    else
        //        size = "small";
        //    string avatarFileName = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg",
        //        BaseConfigs.GetForumPath, uid.Substring(0, 3), uid.Substring(3, 2), uid.Substring(5, 2), uid.Substring(7, 2), size);
        //    FileStream fs = new FileStream(Utils.GetMapPath(avatarFileName), FileMode.Create);
        //    fs.Write(b, 0, b.Length);
        //    fs.Close();
        //    return true;
        //}

        ///// <summary>
        ///// 解码Flash头像传送的数据
        ///// </summary>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //private byte[] FlashDataDecode(string s)
        //{
        //    byte[] r = new byte[s.Length / 2];
        //    int l = s.Length;
        //    for (int i = 0; i < l; i = i + 2)
        //    {
        //        int k1 = ((int)s[i]) - 48;
        //        k1 -= k1 > 9 ? 7 : 0;
        //        int k2 = ((int)s[i + 1]) - 48;
        //        k2 -= k2 > 9 ? 7 : 0;
        //        r[i / 2] = (byte)(k1 << 4 | k2);
        //    }
        //    return r;
        //}

        ///// <summary>
        ///// 上传临时头像文件
        ///// </summary>
        ///// <returns></returns>
        //private string UploadTempAvatar(string uid)
        //{
        //    string filename = "avatar_" + uid + ".jpg";
        //    string uploadUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "upload/";
        //    string uploadDir = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\");
        //    if (!Directory.Exists(uploadDir + "temp\\"))
        //        Utils.CreateDir(uploadDir + "temp\\");

        //    filename = "temp/" + filename;
        //    HttpContext.Current.Request.Files[0].SaveAs(uploadDir + filename);
        //    return uploadUrl + filename;
        //}
        //#endregion

        private void DeleteAttach()
        {
            int aid = DNTRequest.GetInt("aid", 0);
            //int fid = DNTRequest.GetInt("fid", 0);
            AttachmentInfo attachmentInfo = Attachments.GetAttachmentInfo(aid);
            //if ((attachmentInfo.Uid == userid && attachmentInfo.Tid == 0 && attachmentInfo.Pid == 0) 
            //    || Moderators.IsModer(useradminid, userid, fid))
            if (attachmentInfo.Uid == userid || Moderators.IsModer(useradminid, userid, Posts.GetPostInfo(attachmentInfo.Tid, attachmentInfo.Pid).Fid))
            {
                Attachments.DeleteAttachment(aid.ToString());
                ResponseJSON(string.Format("[{{'aid':{0}}}]", aid).ToString());
                return;
            }
            ResponseJSON(string.Format("[{{'erraid':{0}}}]", aid).ToString());
        }

        private void GetAttachList()
        {
            StringBuilder sb = new StringBuilder();
            string posttime = DNTRequest.GetString("posttime");
            List<AttachmentInfo> attachlist;
            if (DNTRequest.GetString("file") != "")
                attachlist = Data.Attachments.GetNoUsedAttachmentList(userid, posttime, AttachmentFileType.FileAttachment);
            else
                attachlist = Data.Attachments.GetNoUsedAttachmentList(userid, posttime);
            sb.Append("[");
            foreach (AttachmentInfo info in attachlist)
            {
                sb.Append(string.Format("{{'aid':{0},'attachment':'{1}','filetype':'{2}','readperm':{3},'attachprice':{4},'width':{5},'height':{6},'extname':'{7}','attachkey':'{8}'}},", info.Aid, info.Attachment.Replace("'", "\\'"), info.Filetype, info.Readperm, info.Attachprice, info.Width, info.Height, Utils.GetFileExtName(info.Attachment), Discuz.Common.Thumbnail.GetKey(info.Aid)));
            }
            //if (sb.ToString() != "" && sb.ToString()[sb.Length-1]==',')
            ResponseJSON(sb.ToString().TrimEnd(',') + "]");
            //else
            //ResponseJSON(sb.Append("]").ToString());
        }

        private void GetImageList()
        {
            StringBuilder sb = new StringBuilder();
            string posttime = DNTRequest.GetString("posttime");
            List<AttachmentInfo> attachList = Data.Attachments.GetNoUsedAttachmentList(userid, posttime, AttachmentFileType.ImageAttachment);
            sb.Append("[");

            foreach (AttachmentInfo info in attachList)
            {
                sb.AppendFormat("{{'aid':{0},'attachment':'{1}','attachkey':'{2}','description':''}},", info.Aid, info.Attachment.Replace("'", "\\'"), Discuz.Common.Thumbnail.GetKey(info.Aid));
            }
            ResponseJSON(sb.ToString().TrimEnd(',') + "]");


            //StringBuilder xml = new StringBuilder(5120);
            //string posttime = DNTRequest.GetString("posttime");
            //List<AttachmentInfo> attachlist = Data.Attachments.GetNoUsedAttachmentList(userid, posttime, AttachmentFileType.ImageAttachment);


            //xml.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            //xml.Append("<root><![CDATA[");
            //if (posttime == "" && attachlist.Count > 0)
            //    xml.Append("<p>以下是你上次上传但没有使用的附件:</p>");
            //xml.Append("<table cellspacing=\"2\" cellpadding=\"2\" class=\"imgl\">");
            //for (int i = 0; i < attachlist.Count; i++)
            //{
            //    if ((i + 1) % 4 == 1)
            //        xml.Append("<tr>");
            //    xml.AppendFormat("<td valign=\"bottom\" id=\"image_td_{0}\" width=\"25%\">", attachlist[i].Aid);
            //    xml.AppendFormat("<input type=\"hidden\" value=\"{1}\" name=\"attachid\"><input type=\"hidden\" value=\"0\" name=\"attachprice_{1}\"><input type=\"hidden\" value=\"0\" name=\"readperm_{1}\"><a href=\"javascript:;\" title=\"{0}\"><img src=\"tools/ajax.aspx?t=image&aid={1}&size=300x300&key={2}&nocache=yes&type=fixnone\" id=\"image_{1}\" onclick=\"insertAttachimgTag('{1}')\" width=\"110\" cwidth=\"300\" /></a>",
            //        attachlist[i].Attachment, attachlist[i].Aid, GetKey(attachlist[i].Aid));
            //    xml.Append("<p class=\"imgf\">");
            //    xml.AppendFormat("<a href=\"javascript:;\" onclick=\"delImgAttach({0},1)\" class=\"del y\">删除</a>", attachlist[i].Aid);
            //    xml.AppendFormat("<input type=\"text\" class=\"px xg2\" value=\"描述\" onclick=\"this.style.display='none';$('image_desc_{0}').style.display='';$('image_desc_{0}').focus();\" />", attachlist[i].Aid);
            //    xml.AppendFormat("<input type=\"text\" name=\"attachdesc_{0}\" class=\"px\" style=\"display: none\" id=\"image_desc_{0}\" />", attachlist[i].Aid);
            //    xml.Append("</p>");
            //    xml.Append("</td>");
            //    if ((i + 1) % 4 == 0)
            //        xml.Append("</tr>");
            //}
            //if (attachlist.Count % 4 != 0)
            //{
            //    for (int i = 0; i < 4 - attachlist.Count % 4; i++)
            //        xml.Append("<td width=\"25%\"></td>");
            //    xml.Append("</tr>");
            //}
            //xml.Append("</table>");
            //xml.AppendFormat("<script type=\"text/javascript\">ATTACHNUM['imageunused'] += {0};updateattachnum('image');</script>", attachlist.Count);
            //xml.Append("]]></root>");
            //ResponseXML(xml);
        }

        private void GetAjaxForumsJsonList()
        {
            StringBuilder sb = new StringBuilder();
            List<ForumInfo> forumlist = Forums.GetSubForumList(DNTRequest.GetInt("fid", 0));
            sb.Append("[");
            if (forumlist != null && forumlist.Count > 0)
            {
                foreach (ForumInfo info in forumlist)
                {
                    if (config.Hideprivate == 1 && info.Viewperm != "" && !Utils.InArray(usergroupid.ToString(), info.Viewperm))
                        continue;
                    sb.Append(string.Format("{{'forumname':'{0}','fid':{1},'parentid':{2},'applytopictype':{3},'topictypeselectoptions':'{4}','postbytopictype':{5}}},", info.Name.Trim(), info.Fid.ToString(), info.Parentid.ToString(), info.Applytopictype.ToString(), Forums.GetCurrentTopicTypesOption(info.Fid, info.Topictypes), info.Postbytopictype.ToString()));
                }
                if (sb.ToString() != "")
                    ResponseJSON(sb.ToString().Remove(sb.ToString().Length - 1) + "]");
            }
            ResponseJSON(sb.Append("]").ToString());
        }

        //private void GetPostInfo()
        //{
        //    PostInfo info = Posts.GetPostInfo(DNTRequest.GetInt("tid", 0), Posts.GetTopicPostInfo(DNTRequest.GetInt("tid", 0)).Pid);
        //    StringBuilder xmlnode = IsValidGetPostInfo(info);
        //    if (!xmlnode.ToString().Contains("<error>"))
        //    {
        //        xmlnode.Append("<post>\r\n\t");
        //        xmlnode.AppendFormat("<message>{0}</message>\r\n", info.Message);
        //        xmlnode.AppendFormat("<tid>{0}</tid>\r\n", info.Tid);
        //        xmlnode.Append("</post>\r\n\t");
        //    }
        //    ResponseXML(xmlnode);
        //}

        private StringBuilder IsValidGetPostInfo(PostInfo info)
        {
            StringBuilder xmlnode = new StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }
            if (info == null)
            {
                xmlnode.Append("<error>读取帖子失败</error>");
                return xmlnode;
            }
            return xmlnode;
        }

        /// <summary>
        /// 获取分页的辩论帖子数据
        /// </summary>
        private void GetDebatePostPage()
        {
            int opinion = DNTRequest.GetInt("opinion", 0);
            int pageid = DNTRequest.GetInt("page", 1);
            int topicid = DNTRequest.GetInt("tid", 0);
            string errormsg = "";
            int ismoder = 0;
            AdminGroupInfo admininfo;
            byte disablepostctrl;
            List<ShowtopicPageAttachmentInfo> positiveattatchments;
            List<ShowtopicPageAttachmentInfo> negativeattatchments;
            int pagesize = config.Debatepagesize;

            TopicInfo topic = Topics.GetTopicInfo(topicid);
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            int hide = 1;

            if (topic == null)
            {
                errormsg = "该主题不存在";
                ResponseText(errormsg);
                return;
            }

            ForumInfo forum = Forums.GetForumInfo(topic.Fid);
            if (ValidatePurview() == string.Empty)
            {
                // 检查是否具有版主的身份
                if (useradminid != 0)
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;
                    //得到管理组信息
                    admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
                    if (admininfo != null)
                    {
                        disablepostctrl = admininfo.Disablepostctrl;
                    }
                }
            }
            if (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1))
            {
                hide = -1;
            }


            if (topic.Closed > 1)
            {
                topicid = topic.Closed;
                topic = Topics.GetTopicInfo(topicid);
                // 如果该主题不存在
                if (topic == null || topic.Closed > 1)
                {
                    ResponseText("不存在的主题ID");
                    return;
                }
            }

            if (topic.Displayorder == -1)
            {
                ResponseText("此主题已被删除！");
                return;
            }
            if (topic.Displayorder == -2)
            {
                ResponseText("此主题未经审核！");
                return;
            }
            if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forum.Fid + "password"))
            {
                ResponseText("本版块被管理员设置了密码");
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + forum.Fid + config.Extname, true);
                return;
            }

            string msg = "";
            if (!Discuz.Forum.UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                ResponseText(msg);
                return;
            }

            if (topic.Special != 4)
            {
                ResponseText("本主题不是辩论主题");
                return;
            }

            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = pagesize;
            postpramsInfo.Pageindex = pageid;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = 0;
            postpramsInfo.Usergroupreadaccess = ismoder == 1 ? int.MaxValue : usergroupinfo.Readaccess;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;

            List<ShowtopicPagePostInfo> postlist = new List<ShowtopicPagePostInfo>();
            if (opinion == 1)
                postlist = Debates.GetPositivePostList(postpramsInfo, out positiveattatchments, ismoder == 1);
            else if (opinion == 2)
                postlist = Debates.GetNegativePostList(postpramsInfo, out negativeattatchments, ismoder == 1);

            DebateInfo debateExpand = Debates.GetDebateTopic(topic.Tid);

            int positivepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 1);
            int negativepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 2);

            int positivepagecount = (positivepostlistcount % pagesize == 0) ? (positivepostlistcount / pagesize) : (positivepostlistcount / pagesize + 1);
            int negativepagecount = (negativepostlistcount % pagesize == 0) ? (negativepostlistcount / pagesize) : (negativepostlistcount / pagesize + 1);

            bool isenddebate = (debateExpand.Terminaltime < DateTime.Now) ? true : false;
            int bbcodeoff = (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1) ? 0 : 1;
            int smileyoff = 1 - forum.Allowsmilies;
            int parseurloff = 0;
            StringBuilder builder = new StringBuilder("{\"postlist\":");
            builder.Append(JavaScriptConvert.SerializeObject(postlist));
            builder.Append(",'debateexpand':");
            builder.Append(JavaScriptConvert.SerializeObject(debateExpand));
            builder.Append(",'pagenumbers':'");
            if (opinion == 1)
                builder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, positivepagecount, "showdebatepage(\\'" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=1&tid=" + topic.Tid + "&{0}\\'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",\\'" + isenddebate + "\\',1," + userid + "," + topicid + ")", 8));
            else
                builder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, negativepagecount, "showdebatepage(\\'" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=2&tid=" + topic.Tid + "&{0}\\'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",\\'" + isenddebate + "\\',2," + userid + "," + topicid + ")", 8));

            builder.Append("'}");

            ResponseText(builder);
        }

        private void ResponseText(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        private void ResponseText(StringBuilder builder)
        {
            ResponseText(builder.ToString());
        }

        private string ValidatePurview()
        {
            return string.Empty;
        }

        #region debate
        /// <summary>
        /// 顶辩论帖
        /// </summary>
        private void DiggDebates()
        {
            StringBuilder xmlnode = IsValidDebates(DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0), DNTRequest.GetInt("type", -1));
            if (!xmlnode.ToString().Contains("<error>"))
            {
                Debates.AddDebateDigg(DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0), DNTRequest.GetInt("type", -1), userid);

                if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 1)
                    Debates.WriteCookies(DNTRequest.GetInt("pid", 0));
            }

            ResponseXML(xmlnode);
        }

        /// <summary>
        /// 关于辩论帖的验证
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="CountenanceType">观点</param>
        /// <returns>返回错误信息</returns>
        private StringBuilder IsValidDebates(int tid, int pid, int CountenanceType)
        {
            StringBuilder xmlnode = new StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }
            if (!Debates.AllowDiggs(userid))
            {
                xmlnode.Append("<error>您所在的用户组不允许此操作</error>");
                return xmlnode;
            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);
            if (tid == 0 || topicinfo.Special != 4 || pid == 0)
            {
                xmlnode.Append("<error>本主题不是辩论帖，无法支持</error>");
                return xmlnode;
            }
            if (Debates.GetDebateTopic(tid).Terminaltime < DateTime.Now)
            {
                xmlnode.Append("<error>本辩论帖结束时间已到，无法再参与</error>");
                return xmlnode;
            }
            if (CountenanceType != 1 && CountenanceType != 2)
            {
                xmlnode.Append("<error>支持方不能为空</error>");
                return xmlnode;
            }
            if (Debates.IsDigged(pid, userid))
            {
                xmlnode.Append("<error>投过票了</error>");
                return xmlnode;
            }
            return xmlnode;
        }

        ///// <summary>
        ///// 获取图片标签
        ///// </summary>
        //private void GetPhotoTags()
        //{
        //    if (!ispost || ForumUtils.IsCrossSitePost())
        //    {
        //        Response.Write("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
        //        return;
        //    }
        //    if (DNTRequest.GetInt("photoid", 0) <= 0) return;

        //    string filename = Utils.GetMapPath(string.Format("{0}cache/photo/{1}/{2}_tags.txt", BaseConfigs.GetForumPath, DNTRequest.GetInt("photoid", 0) / 1000 + 1, DNTRequest.GetInt("photoid", 0)));
        //    if (!File.Exists(filename))
        //        AlbumPluginProvider.GetInstance().WritePhotoTagsCacheFile(DNTRequest.GetInt("photoid", 0));

        //    WriteFile(filename);
        //}
        #endregion

        ///// <summary>
        ///// 获取指定路径下的文件内容并输出
        ///// </summary>
        ///// <param name="filename">文件所在路径</param>
        //private void WriteFile(string filename)
        //{
        //    string tags = "";

        //    if (File.Exists(filename))
        //    {
        //        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        //            {
        //                tags = sr.ReadToEnd();
        //            }
        //        }
        //    }

        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Write(tags);
        //    HttpContext.Current.Response.End();
        //}

        ///// <summary>
        ///// 获取图片热门标签
        ///// </summary>
        //private void GetPhotoHotTags()
        //{
        //    string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + AlbumPluginProvider.GetInstance().PHOTO_HOT_TAG_CACHE_FILENAME);
        //    if (!File.Exists(filename))
        //        AlbumPluginProvider.GetInstance().WriteHotTagsListForPhotoJSONPCacheFile(60);

        //    WriteFile(filename);
        //}

        ///// <summary>
        ///// 获取空间热门标签
        ///// </summary>
        //private void GetSpaceHotTags()
        //{
        //    string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + SpacePluginProvider.GetInstance().SpaceHotTagJSONPCacheFileName);
        //    if (!File.Exists(filename))
        //        SpacePluginProvider.GetInstance().WriteHotTagsListForSpaceJSONPCacheFile(60);

        //    WriteFile(filename);
        //}

        ///// <summary>
        ///// 获取论坛热门标签
        ///// </summary>
        //private void GetForumHotTags()
        //{
        //    string filename = Utils.GetMapPath(BaseConfigs.GetForumPath + ForumTags.ForumHotTagJSONPCacheFileName);
        //    if (!File.Exists(filename))
        //        ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);

        //    WriteFile(filename);
        //}


        ///// <summary>
        ///// 空间日志标签缓存文件
        ///// </summary>
        //private void GetSpacePostTags()
        //{
        //    SpacePluginProvider.GetInstance().GetSpacePostTagsCacheFile(DNTRequest.GetInt("postid", 0));
        //}



        //        /// <summary>
        //        /// 获取根据Tag的相关主题
        //        /// </summary>
        //        private void GetTopicsWithSameTag()
        //        {
        //            if (DNTRequest.GetInt("tagid", 0) > 0)
        //            {
        //                TagInfo tag = Tags.GetTagInfo(DNTRequest.GetInt("tagid", 0));
        //                if (tag != null)
        //                {
        //                    List<TopicInfo> topics = Topics.GetTopicsWithSameTag(DNTRequest.GetInt("tagid", 0), config.Tpp);
        //                    StringBuilder builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
        //                    builder.Append("<root><![CDATA[ \r\n");
        //                    builder.Append(@"<div class=""tagthread"" style=""width:300px"">
        //                                <a class=""close"" href=""javascript:;hideMenu()"" title=""关闭""><img src=""images/common/close.gif"" alt=""关闭"" /></a>
        //                                <h4>标签: ");
        //                    builder.Append(string.Format("<font color='{1}'>{0}</font>", tag.Tagname, tag.Color));
        //                    builder.Append("</h4>\r\n<ul>\r\n");
        //                    foreach (TopicInfo topic in topics)
        //                    {
        //                        builder.Append(string.Format(@"<li><a href=""{0}"" target=""_blank"">{1}</a></li>", base.ShowTopicAspxRewrite(topic.Tid, 1), topic.Title));
        //                    }
        //                    builder.Append(string.Format(@"<li class=""more""><a href=""tags.aspx?tagid={0}"" target=""_blank"">查看更多</a></li>", tag.Tagid));
        //                    builder.Append("</ul>\r\n");
        //                    builder.Append(@"</div>
        //                                ]]></root>");

        //                    ResponseXML(builder);
        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// 读取主题标签缓存文件
        //        /// </summary>
        //        private void GetTopicTags()
        //        {
        //            if (DNTRequest.GetInt("topicid", 0) > 0)
        //            {
        //                StringBuilder dir = new StringBuilder();
        //                dir.Append(BaseConfigs.GetForumPath);
        //                dir.Append("cache/topic/magic/");
        //                dir.Append((DNTRequest.GetInt("topicid", 0) / 1000 + 1).ToString());
        //                dir.Append("/");
        //                string filename = Utils.GetMapPath(dir.ToString() + DNTRequest.GetInt("topicid", 0) + "_tags.config");
        //                if (!File.Exists(filename))
        //                {
        //                    ForumTags.WriteTopicTagsCacheFile(DNTRequest.GetInt("topicid", 0));
        //                }

        //                WriteFile(filename);
        //            }
        //        }

        //        /// <summary>
        //        /// 获取关键字分词
        //        /// </summary>
        //        private void GetRelateKeyword()
        //        {
        //            string title = Utils.UrlEncode(Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("titleenc").Trim())));
        //            string content = Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("contentenc").Trim()));
        //            content = content.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("　", "");
        //            content = Utils.GetUnicodeSubString(content, 500, string.Empty);
        //            content = Utils.UrlEncode(content);

        //            string xmlContent = Utils.GetSourceTextByUrl(string.Format("http://keyword.discuz.com/related_kw.html?title={0}&content={1}&ics=utf-8&ocs=utf-8", title, content));

        //            XmlDocument xmldoc = new XmlDocument();
        //            xmldoc.LoadXml(xmlContent);

        //            XmlNodeList xnl = xmldoc.GetElementsByTagName("kw");
        //            StringBuilder builder = new StringBuilder();
        //            foreach (XmlNode node in xnl)
        //            {
        //                builder.AppendFormat("{0} ", node.InnerText);
        //            }

        //            StringBuilder xmlBuilder = new StringBuilder(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
        //                                            <root><![CDATA[
        //                                            <script type=""text/javascript"">
        //                                            var tagsplit = $('tags').value.split(' ');
        //                                            var inssplit = '{0}';
        //                                            var returnsplit = inssplit.split(' ');
        //                                            var result = '';
        //                                            for(i in tagsplit) {{
        //                                                for(j in returnsplit) {{
        //                                                    if(tagsplit[i] == returnsplit[j]) {{
        //                                                        tagsplit[i] = '';break;
        //                                                    }}
        //                                                }}
        //                                            }}
        //
        //                                            for(i in tagsplit) {{
        //                                                if(tagsplit[i] != '') {{
        //                                                    result += tagsplit[i] + ' ';
        //                                                }}
        //                                            }}
        //                                            $('tags').value = result + '{0}';
        //                                            </script>
        //                                            ]]></root>", builder.ToString()));

        //            ResponseXML(xmlBuilder);
        //        }

        //        /// <summary>
        //        /// 输出表情字符串
        //        /// </summary>
        //        private void GetSmilies()
        //        {
        //            //如果不是提交...
        //            if (ForumUtils.IsCrossSitePost()) return;

        //            HttpContext.Current.Response.Clear();
        //            HttpContext.Current.Response.Write("{" + Caches.GetSmiliesCache() + "}");
        //            HttpContext.Current.Response.End();
        //        }

        //        /// <summary>
        //        /// 检查Rewritename是否存在
        //        /// </summary>
        //        private void CheckRewriteName()
        //        {
        //            if (userid == -1) return;

        //            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();

        //            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //            xmlnode.Append("<result>");
        //            xmlnode.Append(SpacePluginProvider.GetInstance().CheckSpaceRewriteNameAvailable(DNTRequest.GetString("rewritename").Trim()).ToString());
        //            xmlnode.Append("</result>");
        //            ResponseXML(xmlnode);
        //        }

        #region QuickReply

        /// <summary>
        /// 验证回帖的条件
        /// </summary>
        /// <param name="xmlnode">xml节点</param>
        /// <param name="topicid">主题id</param>
        /// <param name="topic">主题信息</param>
        /// <param name="forum">版块信息</param>
        /// <param name="admininfo">管理组信息</param>
        /// <param name="postmessage">帖子内容</param>
        /// <returns>验证结果</returns>
        private bool IsQuickReplyValid(StringBuilder xmlnode, int topicid, TopicInfo topic, ForumInfo forum, AdminGroupInfo admininfo, string postmessage)
        {
            //如果不是提交...
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return false;
            }
            if (topicid == -1)
            {
                xmlnode.Append("<error>无效的主题ID</error>");
                return false;
            }
            if (topic == null)
            {
                xmlnode.Append("<error>不存在的主题ID</error>");
                return false;
            }
            string msg = "";
            if (!UserAuthority.CheckPostTimeSpan(usergroupinfo, admininfo, oluserinfo, Users.GetShortUserInfo(oluserinfo.Userid), ref msg))
            {
                xmlnode.AppendFormat("<error>" + msg + "</error>");
                return false;
            }
            //　如果当前用户非管理员并且该主题已关闭,不允许用户发帖
            if ((admininfo == null || !Moderators.IsModer(admininfo.Admingid, userid, forum.Fid)) && topic.Closed == 1)
            {
                xmlnode.Append("<error>主题已关闭无法回复</error>");
                return false;
            }
            //if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && !Utils.InArray(username, forum.Moderators.Split(',')))
            //{
            //    xmlnode.AppendFormat("<error>本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够</error>", topic.Readperm, usergroupinfo.Grouptitle);
            //    return false;
            //}
            if (!UserAuthority.PostReply(forum, userid, usergroupinfo, topic))
            {
                xmlnode.AppendFormat("<error>" + (topic.Closed == 1 ? "主题已关闭无法回复" : "您没有发表回复的权限") + "</error>");
                return false;
            }

            if (DNTRequest.GetString(config.Antispamposttitle).IndexOf("　") != -1)
            {
                xmlnode.Append("<error>主题不能包含全角空格符</error>");
                return false;
            }
            if (DNTRequest.GetString(config.Antispamposttitle).Length > 60)
            {
                xmlnode.AppendFormat("<error>主题最大长度为60个字符,当前为 {0} 个字符</error>", DNTRequest.GetString(config.Antispamposttitle).Length.ToString());
                return false;
            }
            if (Utils.StrIsNullOrEmpty(postmessage))
            {
                xmlnode.Append("<error>内容不能为空</error>");
                return false;
            }
            if (admininfo != null && admininfo.Disablepostctrl != 1)
            {
                if (postmessage.Length < config.Minpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过少, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
                else if (postmessage.Length > config.Maxpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过多, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
            }
            if (topic.Special == 4 && DNTRequest.GetInt("debateopinion", 0) == 0)
            {
                xmlnode.AppendFormat("<error>请选择您在辩论中的观点</error>");
                return false;
            }

            ShortUserInfo userinfo = new ShortUserInfo();

            if (userid > 0)
            {
                userinfo = Users.GetShortUserInfo(userid);
            }
            //新用户广告强力屏蔽检查
            if ((config.Disablepostad == 1) && useradminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
            {
                if ((config.Disablepostadpostcount != 0 && userinfo.Posts <= config.Disablepostadpostcount) ||
                    (config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(userinfo.Joindate)))
                {
                    foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                    {
                        if (Posts.IsAD(regular, DNTRequest.GetString(config.Antispamposttitle), postmessage))
                        {
                            xmlnode.AppendFormat("<error>发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系</error>");
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 快速回复,使用场景：快速回复，回复某楼
        /// </summary>
        private void QuickReply()
        {
            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            int topicid = DNTRequest.GetInt("topicid", -1);
            int postid = 0;
            string topictitle = "";
            int layer = 1, parentid = 0;

            //如果有选择回复帖，则内容中加入回复人字样
            string postmessage = (DNTRequest.GetString("toreplay_user").Trim() != "" ? DNTRequest.GetString("toreplay_user").Trim() + "\n\n" : "") + DNTRequest.GetString(config.Antispampostmessage).TrimEnd();
            string posttitle = DNTRequest.GetString(config.Antispamposttitle).Trim();

            if ((posttitle != "" && Utils.GetCookie("lastposttitle") == Utils.MD5(posttitle)) || Utils.GetCookie("lastpostmessage") == Utils.MD5(postmessage))
            {
                ResponseXML(xmlnode.Append("<error>请勿重复发帖</error>"));
                return;
            }

            if (useradminid != 1 && (ForumUtils.HasBannedWord(posttitle) || ForumUtils.HasBannedWord(postmessage)))
            {
                string bannedWord = ForumUtils.GetBannedWord(posttitle) == string.Empty ? ForumUtils.GetBannedWord(postmessage) : ForumUtils.GetBannedWord(posttitle);
                ResponseXML(xmlnode.Append(string.Format("<error>对不起, 您提交的内容包含不良信息 ({0}), 因此无法提交, 请返回修改!</error>", bannedWord)));
                return;
            }

            TopicInfo topic = Topics.GetTopicInfo(topicid);
            // 获取主题ID
            int forumid = topic.Fid;

            ForumInfo forum = Forums.GetForumInfo(forumid);
            if (forum == null || forum.Fid < 1 || forum.Layer == 0)
            {
                ResponseXML(xmlnode.Append("<error>版块信息无效!</error>"));
                return;
            }

            string forumname = forum.Name.Trim();
            //是否允许表情
            int smileyoff = 1 - forum.Allowsmilies;

            PostInfo postinfo = new PostInfo();

            #region 用户发帖是否不受灌水，过滤的限制
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            int postDisabled = admininfo != null ? admininfo.Disablepostctrl : config.Disablepostad;
            bool needaudit = UserAuthority.NeedAudit(forum, useradminid, topic, userid, postDisabled, usergroupinfo);//验证当前用户发帖是否需要审核
            #endregion

            if (!IsQuickReplyValid(xmlnode, topicid, topic, forum, admininfo, postmessage))
            {
                ResponseXML(xmlnode);
                return;
            }
         
            #region 校验回帖信息有效性
            //获取回复帖ID
            int replyPostid = DNTRequest.GetInt("postid", -1);
            //回复贴作者UID
            int replyuserid = 0;
            if (replyPostid > 0 && DNTRequest.GetString("postreplynotice") == "true")
            {
                postinfo = Posts.GetPostInfo(topicid, replyPostid);
                if (postinfo == null)
                {
                    ResponseXML(xmlnode.Append("<error>无效的帖子ID!</error>"));
                    return;
                }
                if (topicid != postinfo.Tid)
                {
                    ResponseXML(xmlnode.Append("<error>主题ID无效!</error>"));
                    return;
                }
                replyuserid = postinfo.Posterid;//回复某楼作者，用于下面发送通知时使用
            }
            else
                replyuserid = topic.Posterid;  //回复楼主
            #endregion

            #region 生成帖子信息
            //是否解析HTML
            bool ishtmlon = (TypeConverter.StrToInt(DNTRequest.GetString("htmlon")) == 1);

            if (useradminid == 1)
            {
                postinfo.Title = Utils.HtmlEncode(posttitle);
                postinfo.Message = (usergroupinfo.Allowhtml == 0) ? Utils.HtmlEncode(postmessage) :
                                 ishtmlon ? postmessage : Utils.HtmlEncode(postmessage);
            }
            else
            {
                postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(posttitle));
                if (usergroupinfo.Allowhtml == 0)
                    postinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                else
                    postinfo.Message = ishtmlon ? ForumUtils.BanWordFilter(postmessage) : Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
            }
            postinfo.Topictitle = topic.Title;
            postinfo.Fid = forumid;
            postinfo.Tid = topicid;
            postinfo.Parentid = parentid;
            postinfo.Layer = layer;
            postinfo.Poster = username;
            postinfo.Posterid = userid;
            postinfo.Postdatetime = Utils.GetDateTime();
            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";

            int disablepost = (admininfo != null) ? admininfo.Disablepostctrl : 0;

            //判断当前版块以及用户所属组的审核设置来确定帖子是否需要审核
            postinfo.Invisible = needaudit ? 1 : 0;

            //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
            if (postinfo.Invisible != 1 && useradminid != 1)
            {
                if (Scoresets.BetweenTime(config.Postmodperiods) || 
                    ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                    postinfo.Invisible = 1;
            }

            postinfo.Usesig = TypeConverter.StrToInt(DNTRequest.GetString("usesig"));
            postinfo.Htmlon = (usergroupinfo.Allowhtml == 1 && ishtmlon) ? 1 : postinfo.Htmlon;
            postinfo.Smileyoff = (smileyoff == 0) ? TypeConverter.StrToInt(DNTRequest.GetString("smileyoff")) : 1;
            postinfo.Bbcodeoff = (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1) ? TypeConverter.StrToInt(DNTRequest.GetString("bbcodeoff")) : 0;
            postinfo.Parseurloff = TypeConverter.StrToInt(DNTRequest.GetString("parseurloff"));
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Debateopinion = DNTRequest.GetInt("debateopinion", 0);

            #endregion

            try
            {
                postid = Posts.CreatePost(postinfo);
                Utils.WriteCookie("lastposttitle", Utils.MD5(posttitle));
                Utils.WriteCookie("lastpostmessage", Utils.MD5(postmessage));
            }
            catch
            {
                ResponseXML(xmlnode.Append("<error>提交失败,请稍后重试！</error>"));
                return;
            }

            OnlineUsers.UpdateAction(olid, UserAction.PostReply.ActionID, forumid, forum.Name, topicid, topictitle);

            if (postinfo.Invisible == 1)
            {
                ResponseXML(xmlnode.Append("<error>发表回复成功, 但需要经过审核才可以显示!</error>"));
                return;
            }

            //当回复成功后，发送通知
            if (postid > 0 && DNTRequest.GetString("postreplynotice") == "true")
            {
                postinfo.Pid = postid;
                Notices.SendPostReplyNotice(postinfo, topic, replyuserid);
            }

            //向第三方应用同步数据 （是否在帖子需要）
            Sync.Reply(postid.ToString(), topic.Tid.ToString(), topic.Title, postinfo.Poster, postinfo.Posterid.ToString(), topic.Fid.ToString(), "");

            int hide = (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1) ? 1 : 0;
            if (hide == 1)
            {
                topic.Hide = hide;
                Topics.UpdateTopicHide(topicid);
            }
            //更新topic的attention为0     
            if (Moderators.IsModer(useradminid, userid, topic.Fid) && topic.Attention == 1)
                Topics.UpdateTopicAttentionByTidList(topicid.ToString(), 0);
            else if (topic.Posterid != -1 && userid == topic.Posterid)
                Topics.UpdateTopicAttentionByTidList(topicid.ToString(), 1);

            Topics.UpdateTopicReplyCount(topicid);

            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用,否则使用论坛默认积分
            float[] values = Forums.GetValues(forum.Replycredits);
            if (values != null)
            {
                UserCredits.UpdateUserExtCredits(userid, values, false);//使用版块内积分
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(values);
            }
            else
            {
                UserCredits.UpdateUserCreditsByPosts(userid);//使用默认积分
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(Scoresets.GetUserExtCredits(CreditsOperationType.PostReply));
            }
            xmlnode = GetNewPostXML(xmlnode, postinfo, forum, topic, postid);

            // 删除主题游客缓存
            if (topic.Replies < (config.Ppp + 10))
                ForumUtils.DeleteTopicCacheFile(topicid);

            ResponseXML(xmlnode);
        }


        /// <summary>
        /// 得到输出xml字符串
        /// </summary>
        /// <param name="xmlnode">xml节点</param>
        /// <param name="postinfo">帖子信息</param>
        /// <param name="forum">版块信息</param>
        /// <param name="topic">主题信息</param>
        /// <param name="postid">帖子id</param>
        /// <returns>xml结果</returns>
        private StringBuilder GetNewPostXML(StringBuilder xmlnode, PostInfo postinfo, ForumInfo forum, TopicInfo topic, int postid)
        {
            int hide = (topic.Hide == 1 || ForumUtils.IsHidePost(postinfo.Message)) ? -1 : 1;
            if (usergroupinfo.Allowhidecode == 0)
                hide = 0;

            //判断是否为回复可见帖, price=0为非购买可见(正常), price > 0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topic.Price > 0)
                price = (PaymentLogs.IsBuyer(topic.Tid, userid)) ? -1 : topic.Price;//判断当前用户是否已经购买

            PostpramsInfo postPramsInfo = new PostpramsInfo();
            postPramsInfo.Fid = forum.Fid;
            postPramsInfo.Tid = postinfo.Tid;
            postPramsInfo.Pid = postinfo.Pid;
            postPramsInfo.Jammer = forum.Jammer;
            postPramsInfo.Pagesize = 1;
            postPramsInfo.Pageindex = 1;
            postPramsInfo.Getattachperm = forum.Getattachperm;
            postPramsInfo.Usergroupid = usergroupid;
            postPramsInfo.Attachimgpost = config.Attachimgpost;
            postPramsInfo.Showattachmentpath = config.Showattachmentpath;
            postPramsInfo.Hide = hide;
            postPramsInfo.Price = price;
            postPramsInfo.Ubbmode = false;
            postPramsInfo.Showimages = forum.Allowimgcode;
            postPramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postPramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postPramsInfo.Smiliesmax = config.Smiliesmax;
            postPramsInfo.Bbcodemode = config.Bbcodemode;
            postPramsInfo.Smileyoff = postinfo.Smileyoff;
            postPramsInfo.Bbcodeoff = postinfo.Bbcodeoff;
            postPramsInfo.Parseurloff = postinfo.Parseurloff;
            postPramsInfo.Allowhtml = postinfo.Htmlon;
            postPramsInfo.Sdetail = postinfo.Message;
            postPramsInfo.CurrentUserid = userid;
            UserInfo userInfo = Users.GetUserInfo(postPramsInfo.CurrentUserid);
            postPramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;

            string message = UBB.UBBToHTML(postPramsInfo);

            if (userid == -1)
            {
                xmlnode.Append("<post>\r\n\t");
                xmlnode.AppendFormat("<id>{0}</id>\r\n\t", topic.Replies + 2);
                xmlnode.AppendFormat("<postdatetime>{0}</postdatetime>\r\n\t", postinfo.Postdatetime.Substring(0, postinfo.Postdatetime.Length - 3));
                xmlnode.AppendFormat("<message><![CDATA[{0}]]></message>\r\n\t", message);
                xmlnode.AppendFormat("<olimg><![CDATA[{0}]]></olimg>\r\n", OnlineUsers.GetGroupImg(7));
                xmlnode.AppendFormat("<location>{0}</location>\r\n", Utils.SplitString(IpSearch.GetAddressWithIP(postinfo.Ip), " ")[0] + "网友");
                xmlnode.Append("</post>\r\n");
                return xmlnode;
            }


            UserInfo userinfo = Users.GetUserInfo(userid);

            int adcount = Advertisements.GetInPostAdCount("", postinfo.Fid);
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //头衔、星星
            UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(Utils.StrToInt(usergroupid, UserCredits.GetCreditsUserGroupId(Utils.StrToInt(userinfo.Credits.ToString(), 0)).Groupid));
            string status = (!Utils.StrIsNullOrEmpty(tmpUserGroupInfo.Color)) ? "<font color=\"" + tmpUserGroupInfo.Color + "\">" + tmpUserGroupInfo.Grouptitle + "</font>" : tmpUserGroupInfo.Grouptitle;
            string medals = Utils.StrIsNullOrEmpty(userinfo.Medals) ? "" : Caches.GetMedalsList(userinfo.Medals);

            xmlnode.Append("<post>\r\n\t");
            xmlnode.AppendFormat("<ismoder>{0}</ismoder>", Moderators.IsModer(useradminid, userid, topic.Fid) ? 1 : 0);
            xmlnode.AppendFormat(Advertisements.GetInPostAdXMLByFloor("", postinfo.Fid, templatepath, (topic.Replies + 2) % 15));
            xmlnode.AppendFormat("<id>{0}</id>\r\n\t", topic.Replies + 2);
            xmlnode.AppendFormat("<status><![CDATA[{0}]]></status>\r\n\t", status);
            xmlnode.AppendFormat("<stars>{0}</stars>\r\n\t", tmpUserGroupInfo.Stars);
            xmlnode.AppendFormat("<fid>{0}</fid>\r\n\t", postinfo.Fid);
            xmlnode.AppendFormat("<invisible>{0}</invisible>\r\n\t", postinfo.Invisible);
            xmlnode.AppendFormat("<ip>{0}</ip>\r\n\t", postinfo.Ip);
            xmlnode.AppendFormat("<lastedit>{0}</lastedit>\r\n\t", postinfo.Lastedit);
            xmlnode.AppendFormat("<layer>{0}</layer>\r\n\t", postinfo.Layer);
            xmlnode.AppendFormat("<message><![CDATA[{0}]]></message>\r\n\t", message);
            xmlnode.AppendFormat("<parentid>{0}</parentid>\r\n\t", postinfo.Parentid);
            xmlnode.AppendFormat("<pid>{0}</pid>\r\n\t", postid);
            xmlnode.AppendFormat("<postdatetime>{0}</postdatetime>\r\n\t", postinfo.Postdatetime.Substring(0, postinfo.Postdatetime.Length - 3));
            xmlnode.AppendFormat("<poster>{0}</poster>\r\n\t", postinfo.Poster);
            xmlnode.AppendFormat("<posterid>{0}</posterid>\r\n\t", postinfo.Posterid);
            xmlnode.AppendFormat("<smileyoff>{0}</smileyoff>\r\n\t", postinfo.Smileyoff);
            xmlnode.AppendFormat("<topicid>{0}</topicid>\r\n\t", postinfo.Tid);
            xmlnode.AppendFormat("<title>{0}</title>\r\n\t", Utils.HtmlEncode(postinfo.Title));
            xmlnode.AppendFormat("<usesig>{0}</usesig>\r\n", postinfo.Usesig);
            xmlnode.AppendFormat("<debateopinion>{0}</debateopinion>", postinfo.Debateopinion);
            xmlnode.AppendFormat("<uid>{0}</uid>\r\n\t", userinfo.Uid);
            xmlnode.AppendFormat("<accessmasks>{0}</accessmasks>\r\n\t", userinfo.Accessmasks);
            xmlnode.AppendFormat("<adminid>{0}</adminid>\r\n\t", userinfo.Adminid);
            xmlnode.AppendFormat("<bday>{0}</bday>\r\n\t", userinfo.Bday);
            xmlnode.AppendFormat("<credits>{0}</credits>\r\n\t", userinfo.Credits);
            xmlnode.AppendFormat("<digestposts>{0}</digestposts>\r\n\t", userinfo.Digestposts);
            xmlnode.AppendFormat("<email>{0}</email>\r\n\t", userinfo.Email.Trim());

            string[] score = Scoresets.GetValidScoreName();
            xmlnode.AppendFormat("<score1>{0}</score1>\r\n\t", score[1]);
            xmlnode.AppendFormat("<score2>{0}</score2>\r\n\t", score[2]);
            xmlnode.AppendFormat("<score3>{0}</score3>\r\n\t", score[3]);
            xmlnode.AppendFormat("<score4>{0}</score4>\r\n\t", score[4]);
            xmlnode.AppendFormat("<score5>{0}</score5>\r\n\t", score[5]);
            xmlnode.AppendFormat("<score6>{0}</score6>\r\n\t", score[6]);
            xmlnode.AppendFormat("<score7>{0}</score7>\r\n\t", score[7]);
            xmlnode.AppendFormat("<score8>{0}</score8>\r\n\t", score[8]);
            string[] scoreunit = Scoresets.GetValidScoreUnit();
            xmlnode.AppendFormat("<scoreunit1>{0}</scoreunit1>\r\n\t", scoreunit[1]);
            xmlnode.AppendFormat("<scoreunit2>{0}</scoreunit2>\r\n\t", scoreunit[2]);
            xmlnode.AppendFormat("<scoreunit3>{0}</scoreunit3>\r\n\t", scoreunit[3]);
            xmlnode.AppendFormat("<scoreunit4>{0}</scoreunit4>\r\n\t", scoreunit[4]);
            xmlnode.AppendFormat("<scoreunit5>{0}</scoreunit5>\r\n\t", scoreunit[5]);
            xmlnode.AppendFormat("<scoreunit6>{0}</scoreunit6>\r\n\t", scoreunit[6]);
            xmlnode.AppendFormat("<scoreunit7>{0}</scoreunit7>\r\n\t", scoreunit[7]);
            xmlnode.AppendFormat("<scoreunit8>{0}</scoreunit8>\r\n\t", scoreunit[8]);

            xmlnode.AppendFormat("<extcredits1>{0}</extcredits1>\r\n\t", userinfo.Extcredits1);
            xmlnode.AppendFormat("<extcredits2>{0}</extcredits2>\r\n\t", userinfo.Extcredits2);
            xmlnode.AppendFormat("<extcredits3>{0}</extcredits3>\r\n\t", userinfo.Extcredits3);
            xmlnode.AppendFormat("<extcredits4>{0}</extcredits4>\r\n\t", userinfo.Extcredits4);
            xmlnode.AppendFormat("<extcredits5>{0}</extcredits5>\r\n\t", userinfo.Extcredits5);
            xmlnode.AppendFormat("<extcredits6>{0}</extcredits6>\r\n\t", userinfo.Extcredits6);
            xmlnode.AppendFormat("<extcredits7>{0}</extcredits7>\r\n\t", userinfo.Extcredits7);
            xmlnode.AppendFormat("<extcredits8>{0}</extcredits8>\r\n\t", userinfo.Extcredits8);
            xmlnode.AppendFormat("<extgroupids>{0}</extgroupids>\r\n\t", userinfo.Extgroupids.Trim());
            xmlnode.AppendFormat("<gender>{0}</gender>\r\n\t", userinfo.Gender);
            xmlnode.AppendFormat("<icq>{0}</icq>\r\n\t", userinfo.Icq);
            xmlnode.AppendFormat("<joindate>{0}</joindate>\r\n\t", userinfo.Joindate);
            xmlnode.AppendFormat("<lastactivity>{0}</lastactivity>\r\n\t", userinfo.Lastactivity);
            xmlnode.AppendFormat("<medals><![CDATA[{0}]]></medals>\r\n\t", medals);
            xmlnode.AppendFormat("<nickname>{0}</nickname>\r\n\t", userinfo.Nickname);
            xmlnode.AppendFormat("<oltime>{0}</oltime>\r\n\t", userinfo.Oltime);
            xmlnode.AppendFormat("<onlinestate>{0}</onlinestate>\r\n\t", userinfo.Onlinestate);
            xmlnode.AppendFormat("<showemail>{0}</showemail>\r\n\t", userinfo.Showemail);
            xmlnode.AppendFormat("<signature><![CDATA[{0}]]></signature>\r\n\t", userinfo.Sightml);
            xmlnode.AppendFormat("<sigstatus>{0}</sigstatus>\r\n\t", userinfo.Sigstatus);
            xmlnode.AppendFormat("<skype>{0}</skype>\r\n\t", userinfo.Skype);
            xmlnode.AppendFormat("<website>{0}</website>\r\n\t", userinfo.Website);
            xmlnode.AppendFormat("<yahoo>{0}</yahoo>\r\n", userinfo.Yahoo);
            xmlnode.AppendFormat("<qq>{0}</qq>\r\n", userinfo.Qq);
            xmlnode.AppendFormat("<msn>{0}</msn>\r\n", userinfo.Msn);
            xmlnode.AppendFormat("<posts>{0}</posts>\r\n", userinfo.Posts);
            xmlnode.AppendFormat("<location>{0}</location>\r\n", userinfo.Location);
            xmlnode.AppendFormat("<showavatars>{0}</showavatars>\r\n", config.Showavatars);
            xmlnode.AppendFormat("<userstatusby>{0}</userstatusby>\r\n", config.Userstatusby);
            xmlnode.AppendFormat("<starthreshold>{0}</starthreshold>\r\n", config.Starthreshold);
            xmlnode.AppendFormat("<forumtitle>{0}</forumtitle>\r\n", config.Forumtitle);
            xmlnode.AppendFormat("<showsignatures>{0}</showsignatures>\r\n", config.Showsignatures);
            xmlnode.AppendFormat("<maxsigrows>{0}</maxsigrows>\r\n", config.Maxsigrows);
            xmlnode.AppendFormat("<enablespace>{0}</enablespace>\r\n", config.Enablespace);
            xmlnode.AppendFormat("<enablealbum>{0}</enablealbum>\r\n", config.Enablealbum);
            xmlnode.AppendFormat("<olimg><![CDATA[{0}]]></olimg>\r\n", OnlineUsers.GetGroupImg(userinfo.Groupid));
            string[] customauthorinfo = GeneralConfigs.GetConfig().Customauthorinfo.Split('|');
            xmlnode.AppendFormat("<postleftshow><![CDATA[{0}]]></postleftshow>\r\n", customauthorinfo[0]);
            xmlnode.AppendFormat("<userfaceshow><![CDATA[{0}]]></userfaceshow>\r\n", customauthorinfo[1]);
            xmlnode.AppendFormat("<lastvisit>{0}</lastvisit>\r\n", userinfo.Lastvisit);
            if (userinfo.Uid == topic.Posterid)
            {
                xmlnode.AppendFormat("<onlyauthor>{0}</onlyauthor>\r\n", 1);
            }
            else
            {
                xmlnode.AppendFormat("<onlyauthor>{0}</onlyauthor>\r\n", 0);
            }

            xmlnode.Append("</post>\r\n");

            return xmlnode;
        }


        #endregion

        #region 举报功能

        private void Report()
        {
            if (ForumUtils.IsCrossSitePost())
                return;

            if (userid == -1)
                return;

            string reportUrl = DNTRequest.GetString("report_url");
            string reportmessage = DNTRequest.GetString("reportmessage");
            StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            int fid = DNTRequest.GetInt("fid", 0);
            if (reportmessage == string.Empty || reportmessage.Length < 15)
            {
                xmlnode.Append("<error>您的理由必须多于15个字</error>");
            }
            else
            {
                if (!Utils.StrIsNullOrEmpty(reportUrl))
                {
                    PrivateMessageInfo pm = new PrivateMessageInfo();
                    string message = string.Format(@"下面的链接地址被举报,<br /><a href='{0}' target='_blank'>{0}</a><br />请检查<br/>举报理由：{1}", reportUrl, Utils.HtmlEncode(reportmessage));
                    string curdate = Utils.GetDateTime();
                    Hashtable ht = Users.GetReportUsers();
                    foreach (DictionaryEntry de in ht)
                    {
                        UserInfo info = Users.GetUserInfo(Utils.StrToInt(de.Key, 0));
                        if (Moderators.IsModer(info.Adminid, info.Uid, fid))
                        {
                            pm.Message = message;
                            pm.Subject = "举报信息";
                            pm.Msgto = de.Value.ToString();
                            pm.Msgtoid = Utils.StrToInt(de.Key, 0);
                            pm.Msgfrom = username;
                            pm.Msgfromid = userid;
                            pm.New = 1;
                            pm.Postdatetime = curdate;
                            pm.Folder = 0;
                            PrivateMessages.CreatePrivateMessage(pm, 0);
                        }
                    }
                }
            }
            ResponseXML(xmlnode);
        }

        #endregion

        /// <summary>
        /// 获得指定版块的子版块信息,以xml文件输出
        /// </summary>
        public void GetForumTree()
        {
            StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlnode.Append("<data>\n");

            DataTable dt = Forums.GetForumList(DNTRequest.GetInt("fid", 0));
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (config.Hideprivate == 1 && dr["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), dr["viewperm"].ToString()))
                        continue;

                    xmlnode.Append("<forum name=\"");
                    xmlnode.Append(Utils.RemoveHtml(dr["name"].ToString().Trim()).Replace("&", "&amp;"));
                    xmlnode.Append("\" fid=\"");
                    xmlnode.Append(dr["fid"]);
                    xmlnode.Append("\" subforumcount=\"");
                    xmlnode.Append(dr["subforumcount"]);
                    xmlnode.Append("\" layer=\"");
                    xmlnode.Append(dr["layer"]);
                    xmlnode.Append("\" parentid=\"");
                    xmlnode.Append(dr["parentid"]);
                    xmlnode.Append("\" parentidlist=\"");
                    xmlnode.Append(dr["parentidlist"].ToString().Trim());
                    xmlnode.Append("\" />\n");
                }
            }
            xmlnode.Append("</data>\n");
            //向页面输出xml内容
            ResponseXML(xmlnode);
        }

        /// <summary>
        /// 获得指定主题的回复信息,以xml文件输出
        /// </summary>
        public void GetTopicTree()
        {
            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            TopicInfo topic = Topics.GetTopicInfo(DNTRequest.GetInt("topicid", 0));
            ForumInfo forum = Forums.GetForumInfo(topic.Fid);
            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && !Utils.InArray(username, forum.Moderators))
            {
                xmlnode.Append("<error>本主题阅读权限为: " + topic.Readperm + ", 您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 阅读权限不够</error>");
                ResponseXML(xmlnode);
                return;
            }

            xmlnode.Append("<data>\n");

            DataTable dt = Posts.GetPostTree(DNTRequest.GetInt("topicid", 0), -1, Users.GetUserInfo(userid).Credits);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Select("layer>0", "pid asc"))
                {
                    if (TypeConverter.ObjectToInt(dr["layer"]) != 0)
                    {
                        xmlnode.AppendFormat("<post title=\"{0}\"", UBB.ClearBR(Utils.ClearUBB(dr["title"].ToString())));
                        xmlnode.AppendFormat(" pid=\"{0}\"", dr["pid"]);
                        xmlnode.Append(" message=\"");
                        if (Utils.StrIsNullOrEmpty(UBB.ClearBR(Utils.ClearUBB(dr["message"].ToString()))))
                            xmlnode.Append(dr["title"]);
                        else
                            xmlnode.Append(dr["message"].ToString().IndexOf("[hide]") > -1 ? "*** 隐藏帖 ***" : UBB.ClearBR(Utils.ClearUBB(dr["message"].ToString())));

                        xmlnode.AppendFormat("\" postdatetime=\"{0}\"", DateTime.Parse(dr["postdatetime"].ToString()).ToString("yyyy-MM-dd HH:mm"));
                        xmlnode.AppendFormat(" poster=\"{0}\"", Utils.HtmlEncode(dr["poster"].ToString()));
                        xmlnode.AppendFormat(" posterid=\"{0}\" />\n", dr["posterid"]);
                    }
                }
                if (xmlnode.Length > 0)
                    xmlnode = xmlnode.Replace("&", "");
            }

            xmlnode.Append("</data>\n");
            ResponseXML(xmlnode);
        }


        /// <summary>
        /// 检验当前用户是否可以购买指定附件的校检结果
        /// </summary>
        /// <param name="aId">当前要购买的附件id</param>
        /// <returns></returns>
        private void CheckUserExtCredit(int aid)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(Utils.JsonCharFilter(GetCheckUserExtCreditJson(Attachments.GetAttachmentInfo(aid))));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 检验当前用户是否可以购买指定附件的校检结果
        /// </summary>
        /// <param name="attachmentinfo">当前要购买的附件</param>
        /// <returns>JSON数据信息串</returns>
        private string GetCheckUserExtCreditJson(AttachmentInfo attachmentinfo)
        {
            string content = "";

            if (userid <= 0)
                content = "[{'haserror': true, 'errormsg' : '您尚未登录, 因此无法买卖附件'}]";

            if (attachmentinfo.Aid <= 0)
                content = "[{'haserror': true, 'errormsg' : '当前的附件信息无效'}]";

            if (userid > 0 && attachmentinfo.Aid > 0)
            {
                if (attachmentinfo.Uid == userid)
                    content = "[{'haserror': true, 'errormsg' : '购买附件与发帖人为同一人.'}]";

                int extcredit = Users.GetUserExtCreditsByUserid(userid, Scoresets.GetTopicAttachCreditsTrans());
                if (extcredit >= attachmentinfo.Attachprice)
                    content = string.Format("[{{'haserror': false, 'errormsg' : '', 'attachname': '{0}', 'posterid':{1}, 'poster':'{2}', 'attachprice':{3}, 'extname':'{4}', 'leavemoney':{5}, 'aid':{6}}}]",
                        attachmentinfo.Attachment,
                        attachmentinfo.Uid,
                        Users.GetUserInfo(attachmentinfo.Uid).Username,
                        attachmentinfo.Attachprice,
                        Scoresets.GetTopicAttachCreditsTransName(),
                        extcredit - attachmentinfo.Attachprice,
                        attachmentinfo.Aid);
                else
                {
                    string addExtCreditsTip = "";
                    if (EPayments.IsOpenEPayments())
                        addExtCreditsTip = "<a style=\"color:#FF0000\" href=\"usercpcreditspay.aspx\">充值</a>";
                    content = "[{'haserror': true, 'errormsg' : '对不起,您的账户余额不足 " + extcredit + ", 请返回!" + addExtCreditsTip + "'}]";
                }
            }
            return content;
        }

        /// <summary>
        /// 购买指定附件
        /// </summary>
        /// <param name="aId">要购买的附件id</param>
        public void ConfirmBuyAttach(int aid)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            AttachmentInfo attachmentinfo = Attachments.GetAttachmentInfo(aid);
            string content = GetCheckUserExtCreditJson(attachmentinfo);

            //当通过积分校验时(注：参见GetCheckUserExtCreditJson方法)
            if (content.StartsWith("[{'haserror': false"))
            {
                //修改卖家相应扩展积分
                Users.UpdateUserExtCredits(attachmentinfo.Uid, Scoresets.GetTopicAttachCreditsTrans(), attachmentinfo.Attachprice * (1 - Scoresets.GetCreditsTax()));
                //修改买家相应扩展积分
                Users.UpdateUserExtCredits(userid, Scoresets.GetTopicAttachCreditsTrans(), -attachmentinfo.Attachprice);

                AttachPaymentlogInfo attachpaymentloginfo = new AttachPaymentlogInfo();
                attachpaymentloginfo.Aid = aid;
                attachpaymentloginfo.Amount = attachmentinfo.Attachprice;
                attachpaymentloginfo.Authorid = attachmentinfo.Uid;
                attachpaymentloginfo.NetAmount = Utils.StrToInt(attachmentinfo.Attachprice * (1 - Scoresets.GetCreditsTax()), 0);
                attachpaymentloginfo.PostDateTime = DateTime.Now;
                attachpaymentloginfo.UserName = username;
                attachpaymentloginfo.Uid = userid;
                AttachPaymentLogs.CreateAttachPaymentLog(attachpaymentloginfo);

                content = "[{'haserror': false, 'errormsg' : '', 'aid' : " + aid + "}]";
            }
            HttpContext.Current.Response.Write(Utils.JsonCharFilter(content));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获得当前用户新消息的json内容
        /// </summary>
        public void GetNewPms()
        {
            if (userid < 1)
                return;

            ResponseJSON<PrivateMessageInfo[]>(PrivateMessages.GetPrivateMessageCollection(userid, 1, 50, 1, 1).ToArray());
        }

        public void GetNewNotifications()
        {
            if (userid < 1)
                return;

            ResponseJSON<NoticeInfo[]>(Notices.GetNewNotices(userid));
        }

        #region friendship

        ///// <summary>
        ///// 解除好友关系
        ///// </summary>
        //public void DeleteFriendship()
        //{
        //    if (userid < 1)
        //        return;

        //    string fUidList = DNTRequest.GetString("fuidlist");

        //    if (!Utils.IsNumericList(fUidList))
        //        return;

        //    Friendship.DeleteFriendship(userid, fUidList);

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<result>1</result>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}

        ///// <summary>
        ///// 通过好友请求
        ///// </summary>
        //public void PassFriendshipRequest()
        //{
        //    if (userid < 1)
        //        return;

        //    int groupId = DNTRequest.GetInt("togroupid", 0);
        //    int fromUid = DNTRequest.GetInt("fromuid", 0);
        //    if (fromUid <= 0)
        //        return;

        //    int result = (int)Friendship.PassFriendship(fromUid, userid, usergroupinfo, groupId);

        //    //更新在线表信息
        //    if (result > 0 && oluserinfo.Newfriendrequest > 0)
        //        OnlineUsers.UpdateNewFriendsRequest(olid, -1);

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<result>" + result + "</result>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}
        ///// <summary>
        ///// 忽略好友请求
        ///// </summary>
        //public void IgnoreFriendshipRequest()
        //{
        //    if (userid < 1)
        //        return;

        //    string fromUidList = DNTRequest.GetString("fuidlist");

        //    if (!Utils.IsNumericList(fromUidList))
        //        return;

        //    Friendship.IgnoreFriendshipRequest(userid, fromUidList);

        //    //更新在线表信息

        //    if (oluserinfo.Newfriendrequest > 0)
        //        OnlineUsers.UpdateNewFriendsRequest(olid, -1);

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<result>1</result>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}
        ///// <summary>
        ///// 获取用户好友请求个数
        ///// </summary>
        //public void GetUserFriendshipRequestCount()
        //{
        //    if (userid < 1)
        //        return;

        //    int count = Friendship.GetUserFriendRequestCount(userid);

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<count>" + count + "</count>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}
        ///// <summary>
        ///// 获取用户好友个数
        ///// </summary>
        //public void GetUserFriendshipCount()
        //{
        //    if (userid < 1)
        //        return;

        //    string fusername = DNTRequest.GetString("fusername");
        //    string lastdatetime = DNTRequest.GetString("lastdatetime");
        //    int groupid = DNTRequest.GetInt("groupid", -1);
        //    int count = 0;

        //    if (string.IsNullOrEmpty(fusername) && string.IsNullOrEmpty(lastdatetime) && groupid < 0)
        //        count = Friendship.GetUserFriendsCount(userid);
        //    else
        //    {
        //        Dictionary<FriendshipListSerachEnum, string> conditionTable = new Dictionary<FriendshipListSerachEnum, string>();
        //        if (!string.IsNullOrEmpty(fusername))
        //            conditionTable.Add(FriendshipListSerachEnum.FriendUserName, fusername);

        //        if (!string.IsNullOrEmpty(lastdatetime))
        //            conditionTable.Add(FriendshipListSerachEnum.LastDateTime, lastdatetime);

        //        if (groupid >= 0)
        //            conditionTable.Add(FriendshipListSerachEnum.FriendGroupId, groupid.ToString());
        //        count = Friendship.GetUserFriendsCountByCondition(userid, conditionTable);
        //    }

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<count>" + count + "</count>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}
        ///// <summary>
        ///// 忽略应用邀请
        ///// </summary>
        //public void IgnoreApplicationInvite()
        //{
        //    if (userid < 1)
        //        return;

        //    string idList = DNTRequest.GetString("appinviteid");

        //    int result = ManyouApplications.IgnoreApplicationInvite(idList);

        //    //更新在线表信息
        //    OnlineUsers.UpdateNewApplicationRequest(olid, ManyouApplications.GetApplicationInviteCount(userid));

        //    StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //    xmlnode.Append("<result>" + result + "</result>");
        //    //向页面输出xml内容
        //    ResponseXML(xmlnode);
        //}

        //public void UpdateFriendGroup()
        //{
        //    if (userid < 1)
        //        return;
        //    int gid = DNTRequest.GetInt("gid", 0);
        //    FriendshipGroupInfo friendshipGroupInfo = null;
        //    if (gid > 0)
        //    {
        //        friendshipGroupInfo = Friendship.GetFriendshipGroupInfo(gid);
        //    }
        //    if (friendshipGroupInfo == null)
        //    {
        //        friendshipGroupInfo = new FriendshipGroupInfo();
        //        friendshipGroupInfo.GroupId = 0;
        //        friendshipGroupInfo.GroupName = "未分组";
        //        friendshipGroupInfo.OwnerId = userid;
        //    }

        //    if (userid != friendshipGroupInfo.OwnerId)
        //        return;

        //    int result = Friendship.UpdateFriendGroup(userid, DNTRequest.GetInt("fuid", 0), friendshipGroupInfo.GroupId);

        //    if (result > 0)
        //    {
        //        //if (friendshipGroupInfo.GroupId > 0)
        //        //{
        //        //    Dictionary<FriendshipListSerachEnum, string> conditionTable = new Dictionary<FriendshipListSerachEnum, string>();
        //        //    conditionTable.Add(FriendshipListSerachEnum.FriendGroupId, friendshipGroupInfo.GroupId.ToString());

        //        //    Friendship.UpdateFriendshipGroupCount(friendshipGroupInfo.GroupId, Friendship.GetUserFriendsCountByCondition(userid, conditionTable));
        //        //}

        //        StringBuilder xmlnode = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
        //        xmlnode.Append("<groupname>" + friendshipGroupInfo.GroupName + "</groupname>");
        //        //向页面输出xml内容
        //        ResponseXML(xmlnode);
        //    }
        //}

        //public void CreateNewFriendGroup()
        //{
        //    if (userid < 1)
        //        return;

        //    if (config.Friendgroupmaxcount != 0 && Friendship.GetFriendshipGroupsCount(userid) >= config.Friendgroupmaxcount)
        //        return;

        //    string groupName = DNTRequest.GetString("gname", true);
        //    int groupId = Friendship.CreateFriendshipGroup(userid, groupName);
        //    FriendshipGroupInfo friendGroupInfo = Friendship.GetFriendshipGroupInfo(groupId);

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("{gid:");
        //    sb.Append(friendGroupInfo.GroupId);
        //    sb.Append(",gname:'");
        //    sb.Append(friendGroupInfo.GroupName.Trim());
        //    sb.Append("',ownerid:");
        //    sb.Append(friendGroupInfo.OwnerId);
        //    sb.Append(",friendcount:");
        //    sb.Append(friendGroupInfo.FriendshipCount);
        //    sb.Append("}");

        //    ResponseJSON(sb.ToString());
        //}

        //public void GetFriendGroupJson()
        //{
        //    ResponseJSON(Friendship.GetFriendshipGroupJson(userid));
        //}

        //public void GetFriendsJsonList()
        //{
        //    if (userid < 1)
        //        return;

        //    int pageId = DNTRequest.GetInt("pageid", 1);
        //    int pageSize = DNTRequest.GetInt("pagesize", 10);
        //    string fUserName = DNTRequest.GetString("fusername", true);
        //    string lastDateTime = DNTRequest.GetString("lastdatetime", true);
        //    int groupId = DNTRequest.GetInt("groupid", -1);

        //    pageId = pageId <= 0 ? 1 : pageId;
        //    if (pageSize == 0)
        //        return;
        //    List<FriendshipInfo> friendList = new List<FriendshipInfo>();
        //    if (string.IsNullOrEmpty(fUserName) && string.IsNullOrEmpty(lastDateTime) && groupId < 0)
        //    {
        //        friendList = Friendship.GetUserFriendsList(userid, pageId, pageSize);
        //    }
        //    else
        //    {
        //        Dictionary<FriendshipListSerachEnum, string> conditionTable = new Dictionary<FriendshipListSerachEnum, string>();
        //        if (!string.IsNullOrEmpty(fUserName))
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.FriendUserName, fUserName);
        //        }
        //        if (!string.IsNullOrEmpty(lastDateTime) && Utils.IsDateString(lastDateTime))
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.LastDateTime, lastDateTime);
        //        }
        //        if (groupId >= 0)
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.FriendGroupId, groupId.ToString());
        //        }
        //        friendList = Friendship.GetUserFriendsListByCondition(userid, pageId, pageSize, conditionTable);
        //    }

        //    StringBuilder Json = new StringBuilder();
        //    Json.Append("[");
        //    foreach (FriendshipInfo friendshipInfo in friendList)
        //    {
        //        Json.Append("{");
        //        Json.AppendFormat("uid:{0},fuid:{1},fusername:\"{2}\",groupid:{3},datetime:\"{4}\",userurl:\"{5}\",avatarurl:\"{6}\"",
        //            friendshipInfo.Uid, friendshipInfo.FriendUid, friendshipInfo.FriendUserName, friendshipInfo.GroupId, friendshipInfo.DateTime, Urls.UserInfoAspxRewrite(friendshipInfo.FriendUid), Avatars.GetAvatarUrl(friendshipInfo.FriendUid, AvatarSize.Small));
        //        Json.Append("},");

        //    }
        //    ResponseJSON(Json.ToString().TrimEnd(',') + "]");
        //}

        //public void GetLastFriendItemOnPage(int pageId, int pageSize, string fUserName, string lastDateTime, int groupId)
        //{
        //    if (userid < 1)
        //        return;

        //    pageId = pageId <= 0 ? 1 : pageId;
        //    if (pageSize == 0)
        //        return;

        //    List<FriendshipInfo> friendList = new List<FriendshipInfo>();
        //    int friendsCount;

        //    if (string.IsNullOrEmpty(fUserName) && string.IsNullOrEmpty(lastDateTime) && groupId < 0)
        //    {
        //        friendsCount = Friendship.GetUserFriendsCount(userid);
        //        if (friendsCount < pageId * pageSize)
        //            return;

        //        friendList = Friendship.GetUserFriendsList(userid, pageId, pageSize);
        //    }
        //    else
        //    {
        //        Dictionary<FriendshipListSerachEnum, string> conditionTable = new Dictionary<FriendshipListSerachEnum, string>();
        //        if (!string.IsNullOrEmpty(fUserName))
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.FriendUserName, fUserName);
        //        }
        //        if (!string.IsNullOrEmpty(lastDateTime) && Utils.IsDateString(lastDateTime))
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.LastDateTime, lastDateTime);
        //        }
        //        if (groupId >= 0)
        //        {
        //            conditionTable.Add(FriendshipListSerachEnum.FriendGroupId, groupId.ToString());
        //        }

        //        friendsCount = Friendship.GetUserFriendsCountByCondition(userid, conditionTable);
        //        if (friendsCount < pageId * pageSize)
        //            return;

        //        friendList = Friendship.GetUserFriendsListByCondition(userid, pageId, pageSize, conditionTable);
        //    }

        //    FriendshipInfo friendInfo = friendList[friendList.Count - 1];

        //    string userUrl = Urls.UserInfoAspxRewrite(friendInfo.FriendUid);
        //    string avatarUrl = Avatars.GetAvatarUrl(friendInfo.FriendUid, AvatarSize.Small);

        //    string Json = string.Format("uid:{0},fuid:{1},fusername:\"{2}\",groupid:{3},datetime:\"{4}\",userurl:\"{5}\",avatarurl:\"{6}\"", friendInfo.Uid, friendInfo.FriendUid, friendInfo.FriendUserName, friendInfo.GroupId, friendInfo.DateTime, userUrl, avatarUrl);
        //    ResponseJSON("{" + Json + "}");
        //}

        #endregion

        #region Helper
        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(System.Text.StringBuilder xmlnode)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "Text/XML";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(xmlnode.ToString());
            System.Web.HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 输出json内容
        /// </summary>
        /// <param name="json"></param>
        private void ResponseJSON(string json)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/json";
            System.Web.HttpContext.Current.Response.Expires = 0;
            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(json);
            System.Web.HttpContext.Current.Response.End();
        }

        private void ResponseJSON<T>(T jsonobj)
        {
            ResponseJSON(JavaScriptConvert.SerializeObject(jsonobj));
        }
        #endregion

        #region audit
        /// <summary>
        /// 审核主题的操作
        /// </summary>
        /// <param name="type">操作类型</param>
        public void AuditPost(string type, string reason)
        {

            if (usergroupinfo.Radminid > 0)
            {
                string tid = DNTRequest.GetString("tid");
                string pid = "";
                int tableId = DNTRequest.GetInt("tableid", TypeConverter.StrToInt(Posts.GetPostTableId()));
                //判断版主是否有权限管理主题
                if (usergroupinfo.Radminid == 3 && "passtopic,ignoretopic,deletetopic".Contains(type) && !Topics.GetModTopicCountByTidList(username, tid))
                    return;

                if ("passpost,ignorepost,deletepost".Contains(type))
                {
                    string idList = DNTRequest.GetString("pid");
                    foreach (string id in idList.Split(','))
                        pid += id.Split('|')[0] + ",";
                    pid = pid.TrimEnd(',');
                }
                //判断版主是否有权限管理帖子
                if (usergroupinfo.Radminid == 3 && "passpost,ignorepost,deletepost".Contains(type) && !Posts.GetModPostCountByPidList(username, Posts.GetPostTableId(), pid))
                    return;

                if (("passtopic,ignoretopic,deletetopic".Contains(type) && !CreateNoticeInfo(type, tid, reason)) ||
                    ("passpost,ignorepost,deletepost".Contains(type) && !CreateNoticeInfo(type, DNTRequest.GetString("pid"), reason)))
                    return;
                switch (type)
                {
                    case "passtopic":
                        Topics.PassAuditNewTopic(tableId.ToString(), "", tid, "", DNTRequest.GetString("forumid"));
                        CallbackJson("tid", tid);
                        break;
                    case "ignoretopic":
                        Topics.PassAuditNewTopic(tableId.ToString(), tid, "", "", DNTRequest.GetString("forumid"));
                        CallbackJson("tid", tid);
                        break;
                    case "deletetopic":
                        Topics.PassAuditNewTopic(tableId.ToString(), "", "", tid, DNTRequest.GetString("forumid"));
                        CallbackJson("tid", tid);
                        break;
                    case "passpost":
                        Posts.AuditPost(tableId, pid, "", "", "");
                        CallbackJson("pid", pid);
                        break;
                    case "ignorepost":
                        Posts.AuditPost(tableId, "", "", pid, "");
                        CallbackJson("pid", pid);
                        break;
                    case "deletepost":
                        Posts.AuditPost(tableId, "", pid, "", "");
                        CallbackJson("pid", pid);
                        break;
                }
            }
        }

        private bool CreateNoticeInfo(string type, string idList, string reason)
        {
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(Users.GetUserInfo(userid).Groupid);
            if ((usergroupinfo.Reasonpm == 1 || usergroupinfo.Reasonpm == 3) && reason == "")
            {
                CallbackJson("message", "\"请输入操作理由\"");
                return false;
            }

            string operation = "";
            switch (type)
            {
                case "passtopic":
                    operation = "审核主题";
                    break;
                case "ignoretopic":
                    operation = "忽略主题";
                    break;
                case "deletetopic":
                    operation = "删除主题";
                    break;
                case "passpost":
                    operation = "审核帖子";
                    break;
                case "ignorepost":
                    operation = "忽略帖子";
                    break;
                case "deletepost":
                    operation = "删除帖子";
                    break;
            }
            if ("passtopic,ignoretopic,deletetopic".Contains(type))
            {
                foreach (string tid in idList.Split(','))
                {
                    TopicInfo topic = Topics.GetTopicInfo(TypeConverter.StrToInt(tid));
                    if (topic == null || topic.Posterid == -1)
                        continue;

                    if (usergroupinfo.Reasonpm == 3 || reason != "")
                    {
                        #region 发通知
                        string message = "你发表的主题 ";
                        if (type == "passtopic")
                            message += string.Format("<a href='{0}' target='_blank'>{1}</a> 已经审核通过！ &nbsp; <a href='{0}' target='_blank'>查看 &rsaquo;</a> ",
                                Urls.ShowTopicAspxRewrite(TypeConverter.StrToInt(tid), 0), topic.Title);
                        if (type == "deletetopic")
                            message += string.Format("{0} 没有通过审核，现已被删除！", topic.Title);
                        if (reason != "")
                            message += string.Format("<div class='notequote'><blockquote>{0}</blockquote></div>", Utils.EncodeHtml(reason));

                        SendNoticeInfo(message, topic.Posterid, TypeConverter.StrToInt(tid));
                        #endregion
                    }

                    #region 记录系统日志
                    AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                         usergroupinfo.Grouptitle, Utils.GetRealIP(), Utils.GetDateTime(),
                                                         topic.Fid.ToString(), Forums.GetForumInfo(topic.Fid).Name, topic.Tid.ToString(), topic.Title,
                                                         operation, reason);
                    #endregion
                }
            }
            else
            {
                foreach (string id in idList.Split(','))
                {
                    int pid = TypeConverter.StrToInt(id.Split('|')[0]);
                    int tid = TypeConverter.StrToInt(id.Split('|')[1]);
                    PostInfo postInfo = Posts.GetPostInfo(tid, pid);
                    if (postInfo == null || postInfo.Posterid == -1)
                        continue;

                    if (usergroupinfo.Reasonpm == 3 || reason != "")
                    {
                        string message = "你发表的回复";
                        if (type == "passpost")
                            message += string.Format("已经审核通过！ &nbsp; <a target='_blank' href='{0}#{1}'>查看 &rsaquo;</a>", Urls.ShowTopicAspxRewrite(tid, 0), pid);
                        if (type == "deletepost")
                            message += "没有通过审核，现已被删除！";
                        if (reason != "")
                            message += string.Format(" <p class='summary'>回复内容：<span class='xg1'>{0}</span></p> <div class='notequote'><blockquote>{1}</blockquote></div>",
                                Utils.GetSubString(Utils.ClearUBB(postInfo.Message), 15, "..."), Utils.EncodeHtml(reason));

                        SendNoticeInfo(message, postInfo.Posterid, pid);
                    }
                    #region 记录系统日志
                    TopicInfo topic = Topics.GetTopicInfo(tid);
                    AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                         usergroupinfo.Grouptitle, Utils.GetRealIP(), Utils.GetDateTime(),
                                                         topic.Fid.ToString(), Forums.GetForumInfo(topic.Fid).Name, topic.Tid.ToString(), topic.Title,
                                                         operation, reason);
                    #endregion
                }
            }
            return true;
        }

        private void SendNoticeInfo(string message, int uid, int fromid)
        {
            NoticeInfo ni = new NoticeInfo();
            ni.New = 1;
            ni.Note = message;
            ni.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ni.Type = NoticeType.TopicAdmin;
            ni.Poster = username;
            ni.Posterid = userid;
            ni.Uid = uid;
            ni.Fromid = fromid;
            Notices.CreateNoticeInfo(ni);
        }

        public void DeletePostsByUidAndDays(int uid, int days)
        {
            if (useradminid == 1 || useradminid == 2)
            {
                Posts.ClearPosts(uid, days);
                CallbackJson("uid", uid.ToString());
            }
        }
        /// <summary>
        /// AJAX返回JSON的方法
        /// </summary>
        /// <param name="type">标识主题贴还是回复贴</param>
        /// <param name="id">tid或者pid</param>
        public void CallbackJson(string type, string id)
        {
            StringBuilder Json = new StringBuilder("[");
            Json.Append("{" + type + ":");
            Json.Append(id);
            Json.Append("}]");
            ResponseJSON(Json.ToString());
        }
        #endregion

    } //class
} //namespace
