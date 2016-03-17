using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 用户个性设置
    /// </summary>
    public class usercppreference : UserCpPage
    {
        #region 页面变量
        ///// <summary>
        ///// 用户头像
        ///// </summary>
        //public string avatar;
        ///// <summary>
        ///// 用户头像地址
        ///// </summary>
        //public string avatarurl = "";
        ///// <summary>
        ///// 用户头像类型
        ///// </summary>
        //public int avatartype = DNTRequest.GetInt("avatartype", -1);
        ///// <summary>
        ///// 头像宽度
        ///// </summary>
        //public int avatarwidth = 100;
        ///// <summary>
        ///// 头像高度
        ///// </summary>
        //public int avatarheight = 100;
        public int receivepmsetting;
        #endregion


        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (!IsLogin()) return;

            receivepmsetting = (int)user.Newsletter;
            if (DNTRequest.IsPost())
            {
                //if (DNTRequest.GetInt("avatarchanged", 0) == 1)
                //{
                //    if (avatartype != -1)
                //    {
                //        SetAvatar();
                //        if (IsErr()) return;
                //    }
                //    else if (usergroupinfo.Allowavatar > 0)//当允许使用头像时
                //    {
                //        AddErrLine("请指定新头像的信息<br />");
                //        return;
                //    }

                //    //当不允许使用头像时
                //    if (usergroupinfo.Allowavatar == 0)
                //        SetAvatar(user.Avatar, user.Avatarwidth, user.Avatarheight);
                //}
                //else
                //    SetAvatar(1, Users.GetUserInfo(userid).Avatar, 0, 0);

                //Users.UpdateUserPreference(userid, avatar, avatarwidth, avatarheight, DNTRequest.GetInt("templateid", 0));
                Users.UpdateUserPreference(userid, "", 0, 0, DNTRequest.GetInt("templateid", 0));
                UpdateUserForumSetting();
                OnlineUsers.UpdateInvisible(olid, user.Invisible);
                WriteCookie();

                //从原usercppmset.asp.cs 中移动而来
                receivepmsetting = DNTRequest.GetInt("receivesetting", 1);
                user.Newsletter = (ReceivePMSettingType)receivepmsetting;
                Users.UpdateUserPMSetting(user);

                SetUrl("usercppreference.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("修改个性设置完毕");
            }
            //else
            //{
            //    UserInfo userInfo = Users.GetUserInfo(userid);
            //    SetAvatar(1, userInfo.Avatar, 0, 0);

            //    if (avatar.ToLower().StartsWith(@"avatars\common\"))
            //        avatartype = 0;
            //    else if (avatar.ToLower().StartsWith("http://"))
            //    {
            //        avatarurl = avatar;
            //        SetAvatar(2, userInfo.Avatar, userInfo.Avatarwidth, userInfo.Avatarheight);
            //    }
            //}
        }

        /*private void SetAvatar(string avatar, int avatarWidth, int avatarHeight)
        {
            this.avatar = avatar;
            this.avatarwidth = avatarWidth;
            this.avatarheight = avatarHeight;
        }*/

        /*private void SetAvatar(int avatarType, string avatar, int avatarWidth, int avatarHeight)
        {
            this.avatartype = avatarType;
            SetAvatar(avatar, avatarWidth, avatarHeight);
        }*/

        /// <summary>
        /// 设置头像相关信息
        /// </summary>
        /*private void SetAvatar()
        {
            switch (avatartype)
            {
                case 0: //从系统选择
                    avatar = DNTRequest.GetString("usingavatar");
                    avatar = Utils.UrlDecode(avatar.Substring(avatar.IndexOf("avatar")));
                    avatarwidth = 0;
                    avatarheight = 0;
                    if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath) + avatar))
                    {
                        AddErrLine("不存在的头像文件");
                        return;
                    }
                    break;

                case 1: //上传头像

                    if (usergroupinfo.Allowavatar < 3)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有上传头像的权限");
                        return;
                    }
                    avatar = ForumUtils.SaveRequestAvatarFile(userid, config.Maxavatarsize);
                    if (avatar.Equals(""))
                    {
                        AddErrLine(string.Format("头像图片不合法, 系统要求必须为gif jpg png图片, 且宽高不得超过 {0}x{1}, 大小不得超过 {2} 字节",
                                          config.Maxavatarwidth, config.Maxavatarheight, config.Maxavatarsize));
                        return;
                    }
                    Thumbnail thumb = new Thumbnail();
                    if (!thumb.SetImage(avatar))
                    {
                        AddErrLine("非法的图片格式");
                        return;
                    }
                    thumb.SaveThumbnailImage(config.Maxavatarwidth, config.Maxavatarheight);
                    avatarwidth = 0;
                    avatarheight = 0;
                    break;

                case 2: //自定义头像Url

                    if (usergroupinfo.Allowavatar < 2)
                    {
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 没有使用自定义头像的权限", usergroupinfo.Grouptitle));
                        return;
                    }
                    avatar = DNTRequest.GetString("avatarurl").Trim();
                    if (avatar.Length < 10)
                    {
                        AddErrLine("头像路径不合法");
                        return;
                    }
                    if (!avatar.ToLower().StartsWith("http://"))
                    {
                        AddErrLine("头像路径必须以http://开始");
                        return;
                    }
                    // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                    if (!Utils.InArray(Path.GetExtension(avatar).ToLower(), ".jpg,.gif,.png"))
                    {
                        AddErrLine("头像路径必须是.jpg .gif或.png结尾");
                        return;
                    }
                    avatarwidth = DNTRequest.GetInt("avatarwidth", config.Maxavatarwidth);
                    avatarheight = DNTRequest.GetInt("avatarheight", config.Maxavatarheight);
                    if (avatarwidth <= 0 || avatarwidth > config.Maxavatarwidth || avatarheight <= 0 || avatarheight > config.Maxavatarheight)
                    {
                        AddErrLine(string.Format("自定义URL地址头像尺寸必须大于0, 且必须小于系统当前设置的最大尺寸 {0}x{1}", config.Maxavatarwidth, config.Maxavatarheight));
                        return;
                    }
                    break;
            }
        }*/

        private void WriteCookie()
        {
            ForumUtils.WriteCookie("tpp", user.Tpp.ToString());
            ForumUtils.WriteCookie("ppp", user.Ppp.ToString());
            ForumUtils.WriteCookie("pmsound", user.Pmsound.ToString());
            //ForumUtils.WriteCookie("avatar", avatar);
            Utils.WriteCookie(Utils.GetTemplateCookieName(), DNTRequest.GetInt("templateid", 0).ToString(), 999999);
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUserForumSetting()
        {
            user.Uid = userid;
            user.Tpp = DNTRequest.GetInt("tpp", 0);
            user.Ppp = DNTRequest.GetInt("ppp", 0);
            user.Pmsound = DNTRequest.GetInt("pmsound", 0);
            user.Invisible = DNTRequest.GetInt("invisible", 0);
            user.Customstatus = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("customstatus")));

            //PostpramsInfo _postpramsinfo = new PostpramsInfo();
            //_postpramsinfo.Usergroupid = usergroupid;
            //_postpramsinfo.Attachimgpost = config.Attachimgpost;
            //_postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            //_postpramsinfo.Hide = 0;
            //_postpramsinfo.Price = 0;
            ////获取提交的内容并进行脏字和Html处理
            //_postpramsinfo.Sdetail = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature"))); ;
            //_postpramsinfo.Smileyoff = 1;
            //_postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
            //_postpramsinfo.Parseurloff = 1;
            //_postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
            //_postpramsinfo.Allowhtml = 0;
            //_postpramsinfo.Signature = 1;
            //_postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            //_postpramsinfo.Customeditorbuttoninfo = null;
            //_postpramsinfo.Smiliesmax = config.Smiliesmax;
            //_postpramsinfo.Signature = 1;

            //if (DNTRequest.GetString("signature").Length > usergroupinfo.Maxsigsize)
            //{
            //    AddErrLine(string.Format("您的签名长度超过 {0} 字符的限制，请返回修改。", usergroupinfo.Maxsigsize));
            //    return;
            //}

            //user.Sightml = UBB.UBBToHTML(_postpramsinfo);
            //if (user.Sightml.Length >= 1000)
            //{
            //    AddErrLine("您的签名转换后超出系统最大长度， 请返回修改");
            //    return;
            //}

            //user.Sigstatus = (DNTRequest.GetInt("sigstatus", 0) != 0 ? 1 : DNTRequest.GetInt("sigstatus", 0));
            //user.Signature = _postpramsinfo.Sdetail;

            Users.UpdateUserForumSetting(user);
        }
    }
}