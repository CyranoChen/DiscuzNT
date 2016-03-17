using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.UI.MobileControls;
using System.Collections;
using System.Data;
using System.Web;
using System.Data.Common;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Plugin.PasswordMode;

namespace Discuz.Web
{
    /// <summary>
    /// 版主管理
    /// </summary>
    public class modcp : PageBase
    {
        public DataTable ModeratorLogs = new DataTable();
        public DataTable permuserlist = new DataTable();
        public List<IpInfo> showbannediplist = new List<IpInfo>();
        public List<ForumInfo> showforumlist = new List<ForumInfo>();
        public DataTable announcementlist = new DataTable();
        //public ForumInfo __foruminfo = new ForumInfo();
        public List<ForumInfo> foruminfolist = new List<ForumInfo>();
        public DataTable moderatorLogs = new DataTable();
        public List<ForumInfo> forumslist = new List<ForumInfo>();
        public List<TopicInfo> topiclist = new List<TopicInfo>();
        public List<PostInfo> postlist = new List<PostInfo>();
        public ForumInfo foruminfo;
        protected AdminUserGroups adminusergroup;
        public AdminGroupInfo admingroupinfo;

        public string subject = "", displayorder = "", message = "", tip = "", forumname = "";
        public int mpp = 16, pageid = DNTRequest.GetInt("page", 1), pagecount = 0, counts = 0;
        public int id = -1, uid = -1, banuid = -1, groupid = -1, groupexpiry = 0;
        public int forumid = DNTRequest.GetInt("forumid", 0);
        public int fid = DNTRequest.GetInt("forumid", 0);
        public string uname = "", location = "", bio = "", signature = "", grouptitle = "", curstatus = "", reason = "";
        public string operation = DNTRequest.GetString("operation");
        public string pagenumbers = "";
        public string starttime = Utils.GetDateTime();
        public string endtime = Utils.GetDateTime(1);
        public string op = DNTRequest.GetString("op");
        public DataTable posttablelist = new DataTable();
        public int tableid = DNTRequest.GetInt("tablelist", Utils.StrToInt(Posts.GetPostTableId(), 1));
        public string forumliststr = "";
        public bool banusersubmit = false;
        public bool editusersubmit = false;
        public bool editannouncement = false;
        public bool deleteannoucement = false;
        public string forumnav = "";
        public ShowtopicPagePostInfo showtopicpagepostinfo;
        public bool ismoder;
        public int filter;
        public bool needshowlogin = false;
        public bool alloweditrules = true;
        public bool allowdeleteavatar = false;
        public string about = "";//ajax标识审核的是主题还是回复
        public int last = 0;//ajax标识分页的最后一条
        public int auditTopicCount = 0;
        public int auditPostCount = 0;

        protected override void ShowPage()
        {
            //pageid = DNTRequest.GetInt("page", 1);
            pagetitle = "管理面板";
            about = DNTRequest.GetString("about");
            auditTopicCount = Topics.GetUnauditNewTopicCount(DNTRequest.GetString("forumid"), -2);
            auditPostCount = Posts.GetUnauditNewPostCount(DNTRequest.GetString("forumid"), tableid, 1);
            if (useradminid < 1 || useradminid > 3)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有管理权限", usergroupinfo.Grouptitle));
                return;
            }

            if (Utils.StrIsNullOrEmpty(Utils.GetCookie("cplogincookie")))
            {
                if (operation != "login")
                {
                    Utils.WriteCookie("reurl", DNTRequest.GetRawUrl());
                    Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=login&forumid=" + forumid);
                    return;
                }
                needshowlogin = true;
            }

            Utils.WriteCookie("cplogincookie", Utils.GetCookie("cplogincookie"), 20);
            ismoder = Moderators.IsModer(useradminid, userid, forumid);
            admingroupinfo = AdminUserGroups.AdminGetAdminGroupInfo(this.usergroupid);

            if (admingroupinfo == null)
            {
                AddErrLine("您所在的管理组不存在");
                return;
            }

            #region 公告管理
            if (admingroupinfo.Allowpostannounce == 1 && Utils.InArray(operation.ToLower(), "addannouncements,list,manage,add,editannouncements,updateannouncements"))
            {
                switch (operation.ToLower())
                {
                    case "addannouncements": AddAnnouncements(); break;
                    case "list": ShowAnnouncements(); break;
                    case "manage": ManageAnnouncements(); break;
                    case "add": AddAnnouncements(); break;
                    case "editannouncements": EditAnnouncements(); break;
                    case "updateannouncements": UpdateAnnouncements(); break;
                }
                return;
            }
            #endregion

            switch (operation.ToLower())
            {
                #region 用户管理
                case "edituser":
                    if (admingroupinfo.Allowedituser == 1)
                        EditUser();
                    break;
                case "updateuser":
                    if (admingroupinfo.Allowedituser == 1)
                        UpdateUser();
                    break;
                case "banusersearch":
                    if (admingroupinfo.Allowbanuser == 1)
                        BanUserSearch();
                    break;
                case "banuser":
                    if (admingroupinfo.Allowbanuser == 1)
                        UpdateBanUser();
                    break;
                case "ipban":
                    if (admingroupinfo.Allowbanip == 1)
                    {
                        string ipkey = DNTRequest.GetInt("ip1new", 0) + "." + DNTRequest.GetInt("ip2new", 0) + "." + DNTRequest.GetInt("ip3new", 0) + "." + DNTRequest.GetInt("ip4new", 0);

                        if (ipkey == "0.0.0.0" && Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("chkbanip")))
                            EditBanIp();
                        else
                        {
                            if (!VertifyIp(ipkey))
                                return;

                            BanIp(ipkey);
                            DelBanIp();
                        }
                    }
                    break;
                case "showbannedlist": ShowBannedList(); break;
                #endregion

                #region 版块管理
                case "forumaccesslist":
                    SetDropdownOptions();//带缩进的论坛信息
                    SearchForumSpecialUser();
                    if (DNTRequest.GetString("op") == "access_successful")
                        tip = "access_successful";
                    break;
                case "forumaccessupdate": UpdatePermuserListUser(); break;
                case "editforum": SetDropdownOptions(); GetForumInfo(); break;
                case "updateforum": UpdateForum(); break;
                #endregion

                #region 版块管理
                case "audittopic":
                    if (admingroupinfo.Allowmodpost == 1)
                    {
                        SetDropdownOptions();
                        posttablelist = Posts.GetAllPostTableName();
                        GetTopicList();
                        AuditNewTopic();
                    }
                    break;
                case "auditpost":
                    if (admingroupinfo.Allowmodpost == 1)
                    {
                        SetDropdownOptions();
                        posttablelist = Posts.GetAllPostTableName();
                        AuditPost();
                        GetPostList();
                    }
                    break;
                case "attention":
                    SetDropdownOptions();
                    GetAttentionTopics();
                    break;
                #endregion

                case "userout": UserOut(); break;
                case "login": Login(); break;
                case "logs": GetLogs(); break;

                case "deleteuserpost": DelUserPost(); break;
                default: break;
            }
        }

        #region 登录
        private void Login()
        {

            if (needshowlogin)
            {
                UserInfo userInfo = null;
                string username = DNTRequest.GetFormString("cpname");
                string password = DNTRequest.GetFormString("cppwd");
                if (Utils.StrIsNullOrEmpty(username) || Utils.StrIsNullOrEmpty(password))
                    return;

                //第三方加密验证模式
                if (config.Passwordmode > 1)
                {
                    userInfo = Users.CheckThirdPartPassword(username, password, -1, null);
                    if (userInfo == null)
                    {
                        AddErrLine("用户名或密码错误");
                        return;
                    }
                    uid = userInfo.Uid;
                }
                else
                {
                    uid = config.Passwordmode == 1 ? Users.CheckDvBbsPassword(username, password) : Users.CheckPassword(username, password, true);
                    if (uid == -1)
                    {
                        AddErrLine("用户名或密码错误");
                        return;
                    }
                    userInfo = Users.GetUserInfo(uid);
                }

                if (userInfo.Adminid > 3 || userInfo.Adminid < 1)
                {
                    AddErrLine("您当前的身份没有管理权限");
                    return;
                }
                Utils.WriteCookie("cplogincookie", userInfo.Username, 20);
                Context.Response.Redirect(Utils.GetCookie("reurl"));
            }
            else
                Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=attention&forumid=87&forumid=" + forumid);
        }


        #endregion

        #region 管理公告

        /// <summary>
        /// 添加公告
        /// </summary>
        public void AddAnnouncements()
        {
            if (ispost)
            {
                string subject = DNTRequest.GetString("subject").Trim();
                string message = DNTRequest.GetString("message").Trim();
                if (Utils.StrIsNullOrEmpty(subject) || Utils.StrIsNullOrEmpty(message))
                {
                    AddErrLine("主题或内容不能为空");
                    return;
                }
                DateTime startTime;
                DateTime endTime;
                DateTime.TryParse(DNTRequest.GetString("starttime"), out startTime);
                DateTime.TryParse(DNTRequest.GetString("endtime"), out endTime);

                if (startTime >= endTime)
                {
                    AddErrLine("开始日期或结束日期非法,或者是开始日期与结束日期倒置");
                    return;
                }
                Announcements.CreateAnnouncement(this.username, this.userid, subject, 0, startTime.ToString(), endTime.ToString(), message);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "添加公告", "添加公告");
                nowdatetime = DateTime.Now.ToShortDateString();
                Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list&op=add&forumid=" + DNTRequest.GetFormInt("fid", 0));
            }
        }

        /// <summary>
        /// 显示公告
        /// </summary>
        /// <returns></returns>
        public void ShowAnnouncements()
        {
            if (Utils.InArray(DNTRequest.GetString("op"), "add,delsuccessful"))
                tip = DNTRequest.GetString("op");

            counts = Announcements.GetAnnouncementList().Rows.Count;
            pagecount = counts % 5 == 0 ? counts / 5 : counts / 5 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;
            pageid = DNTRequest.GetInt("page", 1) > pagecount ? pagecount : DNTRequest.GetInt("page", 1);
            announcementlist = Announcements.GetAnnouncementList(5, pageid);

            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "modcp.aspx?operation=list", 5);
        }


        public void ManageAnnouncements()
        {
            UpdateDisplayOrder();
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("aidlist")))
                DeleteAnnouncements(DNTRequest.GetFormString("aidlist"));
        }

        public void UpdateDisplayOrder()
        {
            string[] hid = DNTRequest.GetFormString("hid").Split(',');

            displayorder = DNTRequest.GetFormString("displayorder");
            announcementlist = Forum.Announcements.GetAnnouncementList();
            if (announcementlist.Rows.Count == 0)
            {
                AddErrLine("当前没有任何公告");
                return;
            }

            Forum.Announcements.UpdateAnnouncementDisplayOrder(displayorder, hid, userid, useradminid);
        }

        /// <summary>
        /// 管理员可以删除超版的，超版不可以删除管理员的
        /// </summary>
        /// <param name="uid"></param>
        public void DeleteAnnouncements(string aidlist)
        {
            if (aidlist != "")
            {
                DelAnnouncementOperation(aidlist);

                tip = "delsuccessful";
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "删除公告", "删除公告：" + aidlist);
                Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list&op=delsuccessful&forumid=" + DNTRequest.GetFormInt("fid", 0));
                return;
            }
        }

        private string GetGroupTile()
        {
            UserGroupInfo usergroupinfo = AdminUserGroups.GetUserGroupInfo(Users.GetShortUserInfo(userid).Groupid);
            return usergroupinfo == null ? "" : usergroupinfo.Grouptitle;
        }

        private void DelAnnouncementOperation(string aidlist)
        {
            Announcements.DeleteAnnouncements(aidlist);
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "删除公告", "删除公告,公告ID为: " + DNTRequest.GetString("id"));
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");
        }

        /// <summary>
        /// 编辑公告
        /// </summary>
        /// <param name="uid"></param>
        public void EditAnnouncements()
        {
            if (!ispost)
            {
                #region 根据一个id获得一条公告
                if (DNTRequest.GetInt("id", 0) != 0)
                {
                    AnnouncementInfo announcementInfo = Announcements.GetAnnouncement(DNTRequest.GetInt("id", 0));
                    id = DNTRequest.GetInt("id", 0);
                    if (useradminid == 1 || ((announcementInfo.Posterid == userid && announcementInfo.Posterid > 0)))
                        GetOneAnnouncement(announcementInfo);
                    else
                    {
                        editannouncement = true;
                        AddMsgLine("该公告已经删除或您没有权利编辑它，请返回");
                        SetUrl("modcp.aspx?operation=list");
                        SetMetaRefresh();
                    }
                }
                #endregion
            }
        }

        private void GetOneAnnouncement(AnnouncementInfo announcementInfo)
        {
            subject = announcementInfo.Title;
            displayorder = announcementInfo.Displayorder.ToString();
            starttime = announcementInfo.Starttime.ToString();
            endtime = announcementInfo.Endtime.ToString();
            message = announcementInfo.Message;
        }

        public void UpdateAnnouncements()
        {
            int id = TypeConverter.StrToInt(DNTRequest.GetFormString("id"));

            if (id != 0)
            {
                int posterid = Announcements.GetAnnouncement(TypeConverter.StrToInt(DNTRequest.GetString("id"))).Posterid;
                if (useradminid == 1 || (posterid == userid && posterid > 0))
                {
                    if (!UpdateAnnouncementOperation(id))
                    {    //移除公告缓存
                        Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
                        Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");
                        return;
                    }
                }
            }
            Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list");
        }

        private bool UpdateAnnouncementOperation(int id)
        {
            string subject = DNTRequest.GetString("subject").Trim();
            string message = DNTRequest.GetString("message").Trim();
            if (Utils.StrIsNullOrEmpty(subject) || Utils.StrIsNullOrEmpty(message))
            {
                AddErrLine("主题或内容不能为空");
                return false;
            }
            DateTime startTime;
            DateTime endTime;
            DateTime.TryParse(DNTRequest.GetString("starttime"), out startTime);
            DateTime.TryParse(DNTRequest.GetString("endtime"), out endTime);
            if (startTime >= endTime)
            {
                AddErrLine("开始日期或结束日期非法,或者是开始日期与结束日期倒置");
                return false;
            }
            AnnouncementInfo announcementInfo = new AnnouncementInfo();
            announcementInfo.Id = id;
            announcementInfo.Poster = this.username;
            announcementInfo.Title = subject;
            announcementInfo.Displayorder = TypeConverter.StrToInt(DNTRequest.GetString("displayorder"));
            announcementInfo.Starttime = startTime;
            announcementInfo.Endtime = endTime;
            announcementInfo.Message = message;
            Announcements.UpdateAnnouncement(announcementInfo);
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "更新公告", "更新公告,标题为:" + DNTRequest.GetString("title"));
            return true;
        }

        #endregion

        #region 禁止ip访问

        public void ShowBannedList()
        {
            int icount = 0;

            pageid = DNTRequest.GetInt("page", 1);
            showbannediplist = Ips.GetBannedIpList(5, pageid, out icount);
            counts = icount;
            pagecount = counts % 5 == 0 ? counts / 5 : counts / 5 + 1;
            pagecount = pagecount == 0 ? 1 : pagecount;
            pageid = pageid > pagecount ? pagecount : pageid;
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "modcp.aspx?operation=showbannedlist&forumid=" + forumid, 5);
        }

        public void DelBanIp()
        {
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("chkbanip")))
            {
                AddErrLine("请选择要删除的IP");
                return;
            }
            Ips.DelBanIp(DNTRequest.GetFormString("chkbanip"));
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "删除被禁止访问的ip", "");
        }

        private void EditBanIp()
        {
            string[] expiration = DNTRequest.GetFormString("expiration").Split(',');
            string[] hiddenexpiration = DNTRequest.GetFormString("hiddenexpiration").Split(',');
            string[] hiddenid = DNTRequest.GetFormString("hiddenid").Split(',');

            if (expiration.Length != hiddenexpiration.Length)
                return;

            Ips.EditBanIp(expiration, hiddenexpiration, hiddenid, useradminid, userid);
            //if (useradminid == 1) //1-管理员 2-超版
            //{
            //    for (int i = 0; i < expiration.Length; i++)
            //    {
            //        if (expiration[i] != hiddenexpiration[i])
            //            Ips.EditBanIp(Utils.StrToInt(hiddenid[i].ToString(), -1), expiration[i]);
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < expiration.Length; i++)
            //    {
            //        if (this.userid != Users.GetShortUserInfo(TypeConverter.StrToInt(hiddenid[i])).Uid)
            //            continue;

            //        if (expiration[i] != hiddenexpiration[i])
            //            Ips.EditBanIp(Utils.StrToInt(hiddenid[i].ToString(), -1), expiration[i]);
            //    }
            //}

            Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=showbannedlist");
        }

        private bool VertifyIp(string ipkey)
        {
            Hashtable IPHash = new Hashtable();

            if (Utils.IsRuleTip(IPHash, "ip", out ipkey) == false)
            {
                AddErrLine("格式有错误");
                return false;//IP格式错误
            }
            return true;
        }

        public void BanIp(string ipkey)
        {
            if (VertifyIp(ipkey))
            {
                Ips.AddBannedIp(ipkey, DNTRequest.GetFormInt("validitynew", 30), this.username);
                //string[] ip = ipkey.Split('.');
                //Double deteline = DNTRequest.GetFormInt("validitynew", 30);
                //deteline = deteline == 0 ? 1 :  Math.Round(deteline);

                //#region 直接添加 被禁止的ip
                //if ((Utils.StrToInt(ip[0], 0) < 255 || Utils.StrToInt(ip[1], 0) < 255 || Utils.StrToInt(ip[2], 0) < 255 || Utils.StrToInt(ip[3], 0) < 255) && (ip[0] != "0" && ip[1] != "0" && ip[2] != "0" && ip[3] != "0"))
                //{
                //    IpInfo info = new IpInfo();
                //    info.Ip1= TypeConverter.StrToInt(ip[0]);
                //    info.Ip2 = TypeConverter.StrToInt(ip[1], 0);
                //    info.Ip3 = TypeConverter.StrToInt(ip[2], 0);
                //    info.Ip4 = TypeConverter.StrToInt(ip[3], 0);
                //    info.Username= this.username;
                //    info.Dateline = DateTime.Now.ToShortDateString();
                //    info.Expiration= DateTime.Now.AddDays(deteline).ToString("yyyy-MM-dd");
                //    Ips.AddBannedIp(info);
                //}

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "添加被禁止访问的ip", "");
            }
            Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=showbannedlist&forumid=" + DNTRequest.GetFormInt("fid", 0));

        }
        #endregion

        #region 编辑用户
        public void EditUser()
        {
            uname = DNTRequest.GetFormString("username");
            UserInfo userinfo = (uname != "") ? Users.GetUserInfo(Users.GetUserId(uname)) : Users.GetUserInfo(DNTRequest.GetInt("uid", 0));
            if (userinfo == null)
                return;

            if (userinfo.Adminid >= 1 && userinfo.Adminid <= 3 && userinfo.Adminid <= this.useradminid)
            {
                AddErrLine("您无权编辑此用户信息");
                return;
            }
            uid = userinfo.Uid;
            uname = userinfo.Username;
            location = userinfo.Location;
            bio = userinfo.Bio;
            signature = userinfo.Signature;
            allowdeleteavatar = Avatars.ExistAvatar(uid.ToString());
        }

        public void UpdateUser()
        {
            editusersubmit = true;

            UserInfo userinfo = Users.GetUserInfo(DNTRequest.GetFormInt("uid", 0));

            if (userinfo.Adminid >= 1 && userinfo.Adminid <= 3 && userinfo.Adminid <= this.useradminid)
            {
                AddErrLine("您无权编辑此用户信息");
                return;
            }
            if (DNTRequest.GetString("bio").Length > 500)
            {
                //如果自我介绍超过500...
                AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (DNTRequest.GetString("signature").Length > 500)
            {
                //如果签名超过500...
                AddErrLine("签名不得超过500个字符");
                return;
            }

            userinfo.Location = Utils.HtmlEncode(DNTRequest.GetFormString("locationnew"));
            userinfo.Bio = Utils.HtmlEncode(DNTRequest.GetFormString("bionew"));
            if (userinfo.Signature != Utils.HtmlEncode(DNTRequest.GetFormString("signaturenew")))
            {
                userinfo.Signature = Utils.HtmlEncode(DNTRequest.GetFormString("signaturenew"));
                PostpramsInfo postPramsInfo = new PostpramsInfo();
                postPramsInfo.Usergroupid = usergroupid;
                postPramsInfo.Attachimgpost = config.Attachimgpost;
                postPramsInfo.Showattachmentpath = config.Showattachmentpath;
                postPramsInfo.Hide = 0;
                postPramsInfo.Price = 0;
                //获取提交的内容并进行脏字和Html处理
                postPramsInfo.Sdetail = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signaturenew"))); ;
                postPramsInfo.Smileyoff = 1;
                postPramsInfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
                postPramsInfo.Parseurloff = 1;
                postPramsInfo.Showimages = usergroupinfo.Allowsigimgcode;
                postPramsInfo.Allowhtml = 0;
                postPramsInfo.Signature = 1;
                postPramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                postPramsInfo.Customeditorbuttoninfo = null;
                postPramsInfo.Smiliesmax = config.Smiliesmax;
                postPramsInfo.Signature = 1;

                userinfo.Sightml = UBB.UBBToHTML(postPramsInfo);
            }

            //删除头像
            if (DNTRequest.GetString("delavatar") == "1")
            {
                Avatars.DeleteAvatar(userinfo.Uid.ToString());
            }

            if (!Users.UpdateUser(userinfo))
            {
                AddErrLine("用户未更新成功");
                return;
            }

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "更新编辑用户", "");
            if (DNTRequest.GetFormString("operation") == "edituser")
            {
                op = "updateuser";
                SetUrl("modcp.aspx?operation=edituser&forumid=" + fid);
                MsgForward("modcp_succeed");
                AddMsgLine("用户资料成功更新");
                SetMetaRefresh();
            }
        }
        #endregion

        #region 禁止用户
        public void BanUserSearch()
        {
            uname = DNTRequest.GetFormString("username");
            uid = DNTRequest.GetFormInt("uid", 0);
            UserInfo userinfo = (uname != "") ? Users.GetUserInfo(Users.GetUserId(uname)) : Users.GetUserInfo(uid);
            if (userinfo == null)
                return;

            if (userinfo.Adminid >= 1 && userinfo.Adminid <= 3 && userinfo.Adminid <= this.useradminid)
            {
                AddErrLine("您无权编辑此用户信息");
                return;
            }

            uid = userinfo.Uid;
            uname = userinfo.Username;
            groupid = userinfo.Adminid;
            groupexpiry = userinfo.Groupexpiry;
            grouptitle = AdminUserGroups.GetUserGroupInfo(userinfo.Groupid).Grouptitle;
            curstatus = grouptitle;
        }

        public void UpdateBanUser()
        {
            int banexpirynew = DNTRequest.GetFormInt("banexpirynew", -1);
            string expday = (banexpirynew == 0) ? "29990101" : string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays(banexpirynew));

            uid = TypeConverter.StrToInt(DNTRequest.GetFormString("uid"));
            ShortUserInfo userinfo = Users.GetShortUserInfo(uid);
            if (userinfo.Adminid >= 1 && userinfo.Adminid <= 3 && userinfo.Adminid <= this.useradminid)
            {
                AddErrLine("您无权编辑此用户信息");
                return;
            }

            Users.UpdateBanUser(TypeConverter.StrToInt(DNTRequest.GetFormString("bannew")), expday, uid);

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "更新被禁止访问或发帖的用户", "");
            banusersubmit = true;
            SetUrl("modcp.aspx?operation=banusersearch");
            MsgForward("modcp_succeed");
            AddMsgLine("用户资料成功更新");
            SetMetaRefresh();
        }

        #endregion

        #region 搜索在某论坛内的特殊用户

        protected void SearchForumSpecialUser()
        {
            int forumid = DNTRequest.GetFormInt("forumid", 0);
            if (DNTRequest.GetFormString("suser") != "" && Users.GetUserId(DNTRequest.GetFormString("suser")) == -1)
            {
                forumliststr = forumliststr.Replace("value=\"" + forumid + "\"", "value=\"" + forumid + "\" selected");
                return;
            }

            if (forumid == 0)//搜索全部论坛内的特殊用户
                GetAllSpecialUser(DNTRequest.GetFormString("suser"));
            else if (forumid != -1 && forumid != 0)//搜索某一个论坛内的特殊用户
                GetAllSpecialUser(forumid);

            forumliststr = forumliststr.Replace("value=\"" + forumid + "\"", "value=\"" + forumid + "\" selected");
        }

        private string GetAccess(string permuserlist, int sid, string filter)
        {
            string result = permuserlist;

            for (int i = 0; i < result.Split('|').Length; i++)
            {
                uid = Utils.StrToInt(result.Split('|')[i].Split(',')[1].ToString(), 0);

                if (uid == sid)
                {
                    string puser = result.Split('|')[i].Split(',')[0] + "," + uid + "," + Cheked_Access(Utils.StrToInt(result.Split('|')[i].Split(',')[2], 0)).ToString();

                    if (filter == "updatepermuserlistuser")
                    {
                        result = result.Replace(result.Split('|')[i], puser);
                        return result;
                    }
                    else if (filter == "del")
                    {
                        ArrayList al = new ArrayList(permuserlist.Split('|'));

                        foreach (string s in result.Split('|'))
                        {
                            if (Utils.StrToInt(s.Split(',')[1], 0) == sid)
                            {
                                al.Remove(s);
                            }
                        }

                        if (al.Count == 0)
                        {
                            return result = "";
                        }

                        foreach (string user in al)
                        {
                            result = "";
                            result += user + "|";
                        }

                        if (result != "")
                        {
                            if (result.Contains("||"))
                            {
                                result = result.Replace("||", "|");
                            }
                            result = result.Substring(0, result.Length - 1);
                        }
                    }
                    else
                    {
                        return result.Split('|')[i];
                    }

                }
            }
            return result;
        }


        protected void UpdatePermuserListUser()
        {
            string sname = DNTRequest.GetFormString("new_user");

            int suid = Users.GetUserId(sname);
            if (suid <= 0)
            {
                AddErrLine("该用户不存在");
                return;
            }

            UserInfo userinfo = Users.GetUserInfo(suid);
            if (userinfo.Uid == this.userid && userinfo.Adminid >= 1 && userinfo.Adminid <= 3 && userinfo.Adminid <= this.useradminid)
            {
                AddErrLine("您无权变更管理员或此特殊用户权限或您自己的特殊权限");
                return;
            }

            GetAllSpecialUser("");

            int forumid = DNTRequest.GetFormInt("forumid", -1);
            string permuserlist = Users.SearchSpecialUser(forumid);
            if (!Utils.StrIsNullOrEmpty(permuserlist))
            {
                if (permuserlist.Contains(sname))
                {
                    //回复默认
                    string getaccess = GetAccess(permuserlist, suid, DNTRequest.GetFormInt("deleteaccess", -1) == 0 ? "del" : "updatepermuserlistuser");
                    Users.UpdateSpecialUser(getaccess, forumid);
                }
                Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=forumaccesslist&op=access_successful");
                return;
            }
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), "更新用户权限", "");
            Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=forumaccesslist&forumid=" + DNTRequest.GetFormInt("fid", 0));
        }
        #endregion

        #region 查找特殊权限用户列表
        public void GetAllSpecialUser(string uname)
        {
            ForumInfo[] foruminfo = AdminForums.GetForumSpecialUser(uname);

            if (foruminfo == null)
                return;

            if (foruminfo.Length > 0)
            {
                foreach (ForumInfo fi in foruminfo)
                {
                    foruminfolist.Add(fi);
                }
            }
        }

        public void GetAllSpecialUser(int forumid)
        {
            ForumInfo[] forumInfoArray = AdminForums.GetForumSpecialUser(forumid);
            foruminfolist = new List<ForumInfo>();
            if (forumInfoArray != null)
            {
                foreach (ForumInfo fi in forumInfoArray)
                {
                    foruminfolist.Add(fi);
                }
            }
        }

        protected string GetPowerImg(int power, ForumSpecialUserPower thePower)
        {
            return IsPower(power, thePower) ? "access_allow.gif" : "access_normal.gif";
        }

        protected bool IsPower(int power, ForumSpecialUserPower thePower)
        {
            return (power & (int)thePower) != 0;
        }
        #endregion

        #region 选择特殊权限项

        public int Cheked_Access(int special_value)
        {
            int power = 0;

            if (DNTRequest.GetFormInt("new_view", 0) != 0)
                power = GetPower(DNTRequest.GetFormInt("new_view", 0), ForumSpecialUserPower.ViewByUser);

            if (DNTRequest.GetFormInt("new_post", 0) != 0)
                power = GetPower(DNTRequest.GetFormInt("new_post", 0), ForumSpecialUserPower.PostByUser);

            if (DNTRequest.GetFormInt("new_reply", 0) != 0)
                power = GetPower(DNTRequest.GetFormInt("new_reply", 0), ForumSpecialUserPower.ReplyByUser);

            if (DNTRequest.GetFormInt("new_postattach", 0) != 0)
                power = GetPower(DNTRequest.GetFormInt("new_postattach", 0), ForumSpecialUserPower.DownloadAttachByUser);

            if (DNTRequest.GetFormInt("new_getattach", 0) != 0)
                power = GetPower(DNTRequest.GetFormInt("new_getattach", 0), ForumSpecialUserPower.PostAttachByUser);

            return ((special_value < 0) || (special_value < power)) ? power : special_value - power;
        }

        protected int GetPower(int power, ForumSpecialUserPower thePower)
        {
            return power |= (int)thePower;
        }

        #endregion


        public List<ForumInfo> GetForumList()
        {
            foreach (ForumInfo f in Forums.GetForumList())
            {
                if (useradminid == 3 && Utils.InArray(username, f.Moderators))
                    forumslist.Add(f);
            }
            return forumslist;
        }

        public void GetForumInfo()
        {
            if (DNTRequest.GetInt("forumid", 0) == 0)
            {
                forumid = Forums.GetFirstFourmID();
                foruminfo = Forums.GetForumInfo(forumid);
            }
            else
            {
                foruminfo = Forums.GetForumInfo(DNTRequest.GetInt("forumid", 0));
            }

            if (foruminfo != null)
            {
                alloweditrules = !(foruminfo.Alloweditrules == 0 && useradminid == 3);
            }
            else
            {
                AddErrLine("参数错误");
                return;
            }
        }


        public void SetDropdownOptions()
        {
            forumliststr = (useradminid == 3) ? Forums.GetModerDropdownOptions(username) : Forums.GetDropdownOptions();
            forumliststr = forumliststr.Replace("value=\"" + DNTRequest.GetInt("forumid", 0) + "\"",
                                                "value=\"" + DNTRequest.GetInt("forumid", 0) + "\" selected");
        }

        public void UpdateForum()
        {
            forumid = DNTRequest.GetFormInt("forumid", 0);
            if (forumid == 0)
                return;

            ForumInfo foruminfo = Forums.GetForumInfo(forumid);
            if (useradminid != 3 || foruminfo.Alloweditrules != 0)
                foruminfo.Rules = DNTRequest.GetString("rulesmessage");

            foruminfo.Description = DNTRequest.GetString("descriptionmessage");
            AdminForums.UpdateForumInfo(foruminfo);
            Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=editforum&forumid=" + forumid, false);
        }

        private void UpdateUserCredits(string tidlist)
        {
            int fidstack = -1;
            foreach (string tid in tidlist.Split(','))//如果该方法不需要使用于批量操作，可去掉foreach
            {
                TopicInfo topic = Topics.GetTopicInfo(int.Parse(tid));    //获取主题信息
                float[] values = new float[8];
                if (topic.Fid != fidstack)  //当上一个和当前主题不在一个版块内时，重新读取版块的积分设置
                {
                    values = Forums.GetValues(Forums.GetForumInfo(topic.Fid).Postcredits);
                    fidstack = topic.Fid;
                }
                if (values != null) //使用版块内积分
                    UserCredits.UpdateUserExtCredits(topic.Posterid, values, false);
                else //使用默认积分
                    UserCredits.UpdateUserCreditsByPostTopic(topic.Posterid);
            }
        }



        private void UpdateUserCredits(int postTableId, string pidlist)
        {
            int fidstack = -1;
            foreach (string pid in pidlist.Split(','))
            {
                PostInfo post = Posts.GetPostInfo(postTableId, int.Parse(pid));  //获取帖子信息
                float[] values = new float[8];
                if (post.Fid != fidstack)    //当上一个和当前主题不在一个版块内时，重新读取版块的积分设置
                {
                    values = Forums.GetValues(Forums.GetForumInfo(post.Fid).Replycredits);
                    fidstack = post.Fid;
                }
                if (values != null) //使用版块内积分
                    UserCredits.UpdateUserExtCredits(post.Posterid, values, false);
                else //使用默认积分
                    UserCredits.UpdateUserCreditsByPosts(post.Posterid);
            }
        }

        public void GetTopicList()
        {
            filter = DNTRequest.GetInt("filter", 0);
            if (filter != -2 && filter != -3)
                return;

            forumid = DNTRequest.GetInt("forumid", 0);
            foruminfo = Forums.GetForumInfo(forumid);
            StringBuilder sb = new StringBuilder();
            string forumidlist = "0";
            if (forumid == 0 && !Utils.InArray(useradminid.ToString(), "1,2"))
            {
                foreach (ForumInfo f in GetForumList())
                {
                    sb.Append(f.Fid + ",");
                }
                forumidlist = sb.ToString().TrimEnd(',');
            }
            else
                forumidlist = forumid.ToString();

            forumname = (foruminfo != null) ? foruminfo.Name : "";

            int pageid = DNTRequest.GetInt("page", 1);
            topiclist = Topics.GetUnauditNewTopic(forumidlist, 16, pageid, filter);
            counts = Topics.GetUnauditNewTopicCount(forumidlist, filter);
            last = counts >= 16 ? 15 : counts;
            pagecount = counts % 16 == 0 ? counts / 16 : counts / 16 + 1;
            pagecount = (pagecount == 0) ? 1 : pagecount;
            pageid = (pageid > pagecount) ? pagecount : pageid;

            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "modcp.aspx?operation=audittopic&forumid=" + forumid + "&filter=" + filter, 8);
        }

        public void GetPostList()
        {
            filter = DNTRequest.GetInt("filter", 0);
            if (filter != 1 && filter != -3)
                return;

            forumid = DNTRequest.GetInt("forumid", 0);
            foruminfo = Forums.GetForumInfo(forumid);
            pageid = DNTRequest.GetInt("page", 1);
            StringBuilder sb = new StringBuilder();
            string forumidlist = "0";
            if (forumid == 0 && !Utils.InArray(useradminid.ToString(), "1,2"))
            {
                foreach (ForumInfo f in GetForumList())
                {
                    sb.Append(f.Fid + ",");
                }
                forumidlist = sb.ToString();
            }
            else
                forumidlist = forumid.ToString();

            forumname = (foruminfo != null) ? foruminfo.Name : "";

            if (forumid != 0)
                forumnav = ShowForumAspxRewrite(foruminfo.Pathlist.Trim(), forumid, 1);

            tableid = DNTRequest.GetInt("tablelist", Utils.StrToInt(Posts.GetPostTableId(), 1));
            postlist = Posts.GetUnauditPost(forumidlist, 16, pageid, tableid, filter);
            counts = Posts.GetUnauditNewPostCount(forumidlist, tableid, filter);
            last = counts >= 16 ? 15 : counts;
            pagecount = counts % 16 == 0 ? counts / 16 : counts / 16 + 1;
            pagecount = (pagecount == 0) ? 1 : pagecount;
            pageid = (pageid > pagecount) ? pagecount : pageid;

            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("modcp.aspx?operation=auditpost&forumid={0}&filter={1}&tablelist={2}", forumid, filter, tableid), 8);
        }

        /// <summary>
        /// 获取指定版块id的版块路径
        /// </summary>
        /// <param name="fId"></param>
        /// <returns></returns>
        public string GetPostForumNav(int fId)
        {
            if (fId <= 0)
                return "";
            return ShowForumAspxRewrite(Forums.GetForumInfo(fId).Pathlist.Trim(), fId, 1) + " » ";
        }

        public void AuditNewTopic()
        {
            string tidlist = DNTRequest.GetString("topicidlist");
            string fidlist = DNTRequest.GetString("fidlist");
            string[] fidarray = Utils.SplitString(fidlist, ",", true);
            if (!Utils.IsNumericArray(fidarray))
                return;

            if (useradminid == 3)
            {
                for (int i = 0; i < fidarray.Length; i++)
                {
                    if (!Moderators.IsModer(3, userid, Utils.StrToInt(fidarray[i], 0)))
                        return;
                }
            }

            DataTable dt = Posts.GetAllPostTableName();
            Dictionary<string, string> logs = new Dictionary<string, string>();
            Dictionary<string, string> tidtablelist = new Dictionary<string, string>();
            if (tidlist != "")
            {
                foreach (string tid in tidlist.Split(','))
                {
                    string tableid = Posts.GetPostTableId(Utils.StrToInt(tid, 0));
                    tidtablelist[tableid] = (!tidtablelist.ContainsKey(tableid) ? "" : tidtablelist[tableid].ToString()) + tid + ",";
                }

                foreach (KeyValuePair<string, string> key in tidtablelist)
                {
                    string validate = string.Empty, delete = string.Empty, ignore = string.Empty;
                    foreach (string tid in tidlist.Split(','))
                    {
                        if (key.Value.Contains(tid))
                        {
                            switch (DNTRequest.GetString("mod_" + tid).ToLower())
                            {
                                case "validate":
                                    logs.Add("帖子ID为<a href=\"" + config.Forumurl + "showtopic.aspx?tid=" + tid + "\">" + tid + "</a>  " + DNTRequest.GetString("pm_" + tid) + "", "审核通过帖子");
                                    validate = string.Concat(validate, ",", tid.ToString());
                                    //UpdateUserCredits(tid);
                                    break;

                                case "delete":
                                    logs.Add("帖子ID为" + tid + "  " + DNTRequest.GetString("pm_" + tid), "删除未审核帖子");
                                    delete = string.Concat(delete, ",", tid.ToString());
                                    break;

                                case "ignore":
                                    logs.Add("帖子ID为" + tid + "  " + DNTRequest.GetString("pm_" + tid), "忽略未审核主题");
                                    ignore = string.Concat(ignore, ",", tid.ToString());
                                    break;
                            }
                        }
                    }
                    if (delete != "")
                        delete = delete.Remove(0, 1);

                    if (ignore != "")
                        ignore = ignore.Remove(0, 1);

                    if (validate != "")
                        validate = validate.Remove(0, 1);

                    if (Utils.SplitString(delete, ",", true).Length > 1 && admingroupinfo.Allowmassprune != 1)
                    {
                        AddErrLine("您所在的用户组无法批量删帖");
                        return;
                    }
                    Topics.PassAuditNewTopic(key.Key, ignore, validate, delete, fidlist);
                }

                foreach (KeyValuePair<string, string> log in logs)
                {
                    AdminVistLogs.InsertLog(this.uid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), log.Value.ToString(), log.Key.ToString());
                }
                SetUrl(BaseConfigs.GetForumPath + "modcp.aspx?operation=audittopic&forumid=" + forumid);
                SetMetaRefresh(0);
            }
        }

        public void AuditPost()
        {
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("pidlist")) && ismoder)
            {
                Dictionary<string, string> logs = new Dictionary<string, string>();
                string validate = "", delete = "", ignore = "";
                tableid = DNTRequest.GetInt("tableid", 0);

                string[] pidlist = DNTRequest.GetString("pidlist").Split(',');
                string[] tidlist = DNTRequest.GetString("tidlist").Split(',');
                string[] fidlist = DNTRequest.GetString("fidlist").Split(',');
                if (!Utils.IsNumericArray(fidlist))
                    return;
                if (useradminid == 3)
                {
                    for (int i = 0; i < fidlist.Length; i++)
                    {
                        if (!Moderators.IsModer(3, userid, Utils.StrToInt(fidlist[i], 0)))
                            return;
                    }
                }
                for (int i = 0; i < pidlist.Length; i++)
                {
                    switch (DNTRequest.GetString("mod_" + pidlist[i]).ToLower())
                    {
                        case "validate":
                            logs.Add("帖子ID为<A href=\"" + config.Forumurl + "showtopic.aspx?pid=" + pidlist[i] + "\">" + pidlist[i] + "</a>  " + DNTRequest.GetString("pm_" + pidlist[i]) + "", "审核通过帖子");
                            validate = string.Concat(validate, ",", pidlist[i]);
                            UpdateUserCredits(Utils.StrToInt(tidlist[i], 0), pidlist[i]);
                            break;

                        case "delete":
                            logs.Add("帖子ID为" + pidlist[i] + "  " + DNTRequest.GetString("pm_" + pidlist[i]), "删除未审核帖子");
                            delete = string.Concat(delete, ",", pidlist[i]);
                            break;

                        case "ignore":
                            logs.Add("帖子ID为" + pidlist[i] + "  " + DNTRequest.GetString("pm_" + pidlist[i]), "忽略未审核帖子");
                            ignore = string.Concat(ignore, ",", pidlist[i]);
                            break;
                    }
                    //UpdateUserCredits(Utils.StrToInt(tidlist[i], 0), pidlist[i]);
                }

                if (delete != "")
                    delete = delete.Remove(0, 1);

                if (validate != "")
                    validate = validate.Remove(0, 1);

                if (ignore != "")
                    ignore = ignore.Remove(0, 1);

                if (Utils.SplitString(delete, ",", true).Length > 1 && admingroupinfo.Allowmassprune != 1)
                {
                    AddErrLine("您所在的用户组无法批量删帖");
                    return;
                }

                Posts.AuditPost(tableid, validate, delete, ignore, DNTRequest.GetString("fidlist"));
                foreach (KeyValuePair<string, string> log in logs)
                {
                    AdminVistLogs.InsertLog(this.uid, this.username, this.usergroupid, GetGroupTile(), DNTRequest.GetIP(), log.Value.ToString(), log.Key.ToString());
                }
                SetUrl(BaseConfigs.GetForumPath + "modcp.aspx?operation=auditpost&forumid=" + forumid);
                SetMetaRefresh(0);
            }

        }

        public void GetLogs()
        {
            int lpp = DNTRequest.GetInt("lpp", 20);
            int pageid = DNTRequest.GetInt("page", 1);
            string keyword = DNTRequest.GetString("keyword");
            if (keyword == "")
            {
                moderatorLogs = AdminModeratorLogs.LogList(lpp, pageid);
                counts = AdminModeratorLogs.RecordCount();
            }
            else
            {
                moderatorLogs = AdminModeratorLogs.LogList(lpp, pageid, AdminModeratorLogs.SearchModeratorManageLog(keyword));
                counts = AdminModeratorLogs.RecordCount(AdminModeratorLogs.SearchModeratorManageLog(keyword));
            }
            pagecount = counts % lpp == 0 ? counts / lpp : counts / lpp + 1;
            pagecount = (pagecount == 0) ? 1 : pagecount;
            pageid = (pageid > pagecount) ? pagecount : pageid;
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "modcp.aspx?operation=logs&keyword=" + keyword + "&lpp=" + lpp, 8);
        }

        public void UserOut()
        {
            ForumUtils.ClearUserCookie("cplogincookie");
            if (DNTRequest.GetInt("forumid", 0) == 0)
                Context.Response.Redirect(BaseConfigs.GetForumPath + "index.aspx");
            else
                Context.Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + DNTRequest.GetInt("forumid", 0) + config.Extname);
        }


        public void GetAttentionTopics()
        {
            int pageid = DNTRequest.GetInt("page", 1);
            string keyword = DNTRequest.GetString("keyword");
            string linkurl = "modcp.aspx?operation=attention&keyword=" + keyword + "&forumid=" + forumid;
            string forumidlist = "0";
            StringBuilder sb = new StringBuilder();
            if (forumid == 0 && !Utils.InArray(useradminid.ToString(), "1,2"))
            {
                foreach (ForumInfo f in GetForumList())
                {
                    sb.Append(f.Fid + ",");
                }
                if (sb.Length > 0)
                    forumidlist = sb.ToString().Remove(sb.ToString().Length - 1);
            }
            else
                forumidlist = forumid.ToString();

            int disattentiontype = DNTRequest.GetFormInt("disattentiontype", -1);

            if (disattentiontype != -1 && ispost)
            {
                switch (disattentiontype)
                {
                    case 0:
                        if (DNTRequest.GetFormString("topicid") != "")
                        {
                            Topics.UpdateTopicAttentionByTidList(DNTRequest.GetFormString("topicid"), 0);
                            Context.Response.Redirect(BaseConfigs.GetForumPath + linkurl);
                            return;
                        }
                        break;

                    default:
                        Topics.UpdateTopicAttentionByFidList(forumidlist, Math.Abs(disattentiontype));
                        Context.Response.Redirect(BaseConfigs.GetForumPath + linkurl);
                        return;
                }
            }
            else
            {
                topiclist = Topics.GetAttentionTopics(forumidlist, 16, pageid, keyword);
                counts = Topics.GetAttentionTopicCount(forumidlist, keyword);
                pagecount = counts % 16 == 0 ? counts / 16 : counts / 16 + 1;
                pagecount = (pagecount == 0) ? 1 : pagecount;
                pageid = (pageid > pagecount) ? pagecount : pageid;

                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, linkurl, 8);
            }
        }

        public void DelUserPost()
        {
            switch (DNTRequest.GetFormString("deletetype"))
            {
                case "topics":

                    break;
                case "posts":

                    break;
                default:
                    break;
            }
        }

        public void GetAuditTopicCountByFid(string fid)
        {
            counts = Topics.GetUnauditNewTopicCount(fid, filter);
        }
    }
}
