using System;
using System.Web;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Aggregation;
using Discuz.Album.Data;

namespace Discuz.Album.Pages
{
    /// <summary>
    /// 查看图片页面
    /// </summary>
    public class showphoto : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 图片所属相册
        /// </summary>
        public AlbumInfo album;
        /// <summary>
        /// 所属相册分类
        /// </summary>
        public AlbumCategoryInfo albumcategory;
        /// <summary>
        /// 是否需要密码访问
        /// </summary>
        public bool needpassword = true;
        /// <summary>
        /// 图片Id
        /// </summary>
        public int photoid = DNTRequest.GetInt("photoid", 0);
        /// <summary>
        /// 图片信息
        /// </summary>
        public PhotoInfo photo;
        /// <summary>
        /// 查看图片方式,0表示查看当前Id图片,1表示查看当前图片的上一张图片,2表示查看当前图片的下一张图片
        /// </summary>
        public byte mode = 0;
        /// <summary>
        /// 一周热门图片列表
        /// </summary>
        public List<PhotoInfo> weekhotphotolist = new List<PhotoInfo>();
        /// <summary>
        /// 图片评论列表
        /// </summary>
        public List<PhotoCommentInfo> comments = new List<PhotoCommentInfo>();
        /// <summary>
        /// 是否能够发表评论
        /// </summary>
        public bool commentable = true;
        /// <summary>
        /// 是否能够编辑
        /// </summary>
        public bool editable = false;
        /// <summary>
        /// 相册弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = Albums.GetPhotoListMenuDivCache();
        /// <summary>
        /// 图片聚合设置信息
        /// </summary>
        public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();        
        /// <summary>
        /// json数据文件名称
        /// </summary>
        public string jsonfilename;
        /// <summary>
        /// 相册RSS UrlRewrite
        /// </summary>
        private string photorssurl = "";
        #endregion

        protected override void ShowPage()
        {
            if (config.Enablealbum != 1)
            {
                AddErrLine("相册功能已被关闭");
                return;
            }

            //一周热图总排行
            weekhotphotolist = AggregationFacade.AlbumAggregation.GetWeekHotPhotoList(photoconfig.Weekhot);
            string go = DNTRequest.GetString("go");
            switch (go)
            {
                case "prev":
                    mode = 1;
                    break;
                case "next":
                    mode = 2;
                    break;
                default:
                    mode = 0;
                    break;
            }

            if (photoid < 1)
            {
                AddErrLine("指定的图片不存在");
                return;
            }

            photo = DTOProvider.GetPhotoInfo(photoid, 0, 0);
            if (photo == null)
            {
                AddErrLine("指定的图片不存在");
                return;
            }

            album = DTOProvider.GetAlbumInfo(photo.Albumid);
            if (album == null)
            {
                AddErrLine("指定的相册不存在");
                return;
            }

            if (mode != 0)
            {
                photo = DTOProvider.GetPhotoInfo(photoid, photo.Albumid, mode);
                if (photo == null)
                {
                    AddErrLine("指定的图片不存在");
                    return;
                }
            }
            
            if (config.Rssstatus == 1)
            {
                if (GeneralConfigs.GetConfig().Aspxrewrite == 1)
                    photorssurl = string.Format("photorss-{0}{1}", photo.Userid, GeneralConfigs.GetConfig().Extname);
                else
                    photorssurl = string.Format("rss.aspx?uid={0}&type=photo", photo.Userid);

                AddLinkRss(string.Format("tools/{0}", photorssurl), "最新图片");
            }

            comments = DTOProvider.GetPhotoCommentCollection(photo.Photoid);
            pagetitle = photo.Title;

            //权限验证部分,私有相册,不是相册所有者
            if (album.Type == 1 && album.Userid != userid)
            {
                if (ForumUtils.GetCookie("album" + photo.Albumid + "password") != Utils.MD5(album.Password))
                {
                    //首先验证Cookie中如果相册密码不正确，则要求输入密码，并以输入值验证
                    string password = DNTRequest.GetFormString("albumpassword");
                    if (album.Password == password)
                    {
                        ForumUtils.WriteCookie("album" + photo.Albumid + "password", Utils.MD5(password));
                        needpassword = false;
                    }
                }
                else
                    needpassword = false;
            }
            else
                needpassword = false;

            if (Utils.InArray(usergroupid.ToString(), config.Photomangegroups))
                needpassword = false;

            albumcategory = DTOProvider.GetAlbumCategory(album.Albumcateid);
            jsonfilename = "cache/album/" + (album.Albumid/1000+1).ToString() + "/" + album.Albumid.ToString() + "_json.txt";

            //非图片所有者时更新图片浏览量
            if (userid != photo.Userid)
                DTOProvider.UpdatePhotoViews(photoid);

            //判断权限
            {
                switch (photo.Commentstatus)
                {
                    case PhotoStatus.Owner:
                        if (userid != photo.Userid)
                            commentable = false;
                        break;
                }
                if (userid < 1)
                    commentable = false;

                //重构时加入指定管理用户组
                if (userid == photo.Userid || Utils.InArray(usergroupid.ToString(), config.Photomangegroups))
                    editable = true;
            }

            // 如果评论数不同步则同步
            if (photo.Comments != comments.Count)
                DbProvider.GetInstance().UpdatePhotoComments(photo.Photoid, comments.Count - photo.Comments);//更新评论数

            if (ispost)
            {
                string message = DNTRequest.GetFormString("message").Trim();
                int delcid = DNTRequest.GetFormInt("delcommentid", 0);
                if (message != string.Empty)
                {
                    SavePhotoComment(message);
                    return;
                }
                else if (delcid > 0)
                {
                    if (editable)
                    {
                        DbProvider.GetInstance().DeletePhotoComment(delcid);
                        //更新评论数
                        DbProvider.GetInstance().UpdatePhotoComments(photo.Photoid, -1);
                        AddMsgLine("删除成功!");
                        SetUrl("showphoto.aspx?photoid=" + photo.Photoid);
                        SetMetaRefresh();
                        return;
                    }
                }
                AddErrLine("非法操作");
                SetMetaRefresh();
            }
        }

        private void SavePhotoComment(string message)
        {
            if (ForumUtils.IsCrossSitePost())
            {
                AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return;
            }
            if (userid < 1)
            {
                AddErrLine("请登录后发表评论");
                return;
            }
            if (userid != photo.Userid && photo.Commentstatus == PhotoStatus.Owner)
            {
                AddErrLine("此图片禁止评论");
                return;
            }
            if (message.Length < 1)
            {
                AddErrLine("评论内容不能为空");
                return;
            }
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {
                if (message.Length < config.Minpostsize)
                {
                    AddErrLine("您发表的内容过少, 系统设置要求评论内容不得少于 " + config.Minpostsize.ToString() + " 字");
                    return;
                }
                else if (message.Length > 2000)
                {
                    AddErrLine("您发表的内容过多, 系统设置要求评论内容不得多于 2000 字");
                    return;
                }

                int interval = Utils.StrDateDiffSeconds(lastposttime, config.Postinterval);
                if (interval < 0)
                {
                    AddErrLine("系统规定发帖间隔为" 
                        + config.Postinterval.ToString() 
                        + "秒, 您还需要等待 " 
                        + (interval*-1).ToString() 
                        + " 秒");
                    return;
                }
            }

            PhotoCommentInfo pcomment = new PhotoCommentInfo();
            pcomment.Content = Utils.RemoveHtml(ForumUtils.BanWordFilter(message));
            pcomment.Ip = DNTRequest.GetIP();
            pcomment.Parentid = DNTRequest.GetFormInt("parent", 0);
            pcomment.Photoid = photo.Photoid;
            pcomment.Postdatetime = DateTime.Now;
            pcomment.Userid = userid;
            pcomment.Username = username;
            pcomment.Commentid = DbProvider.GetInstance().CreatePhotoComment(pcomment);

            //更新最后发帖时间
            //OnlineUsers.UpdatePostTime(olid);

            //更新评论数
            DbProvider.GetInstance().UpdatePhotoComments(photo.Photoid, 1);

            //发送相册图片评论通知
            if (DNTRequest.GetString("sendnotice") == "on")
                SendPhotoComment(pcomment);

            HttpContext.Current.Response.Redirect(string.Format("{0}showphoto.aspx?photoid={1}&reply=1#comments", BaseConfigs.GetForumPath, photo.Photoid));
        }

        /// <summary>
        /// 发送相册图片评论通知
        /// </summary>
        /// <param name="pcomment">评论信息</param>
        private void SendPhotoComment(PhotoCommentInfo pcomment)
        {
            //当回复人与相册所有人不是同一人时，则向相册所有人发送通知
            if (album.Userid != pcomment.Userid && pcomment.Commentid > 0)
            {
                NoticeInfo noticeinfo = new NoticeInfo();
                noticeinfo.Note = Utils.HtmlEncode(string.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 评论了您的{2}--\"{3}\" , 请<a href =\"showphoto.aspx?photoid={4}&reply={5}#comments\">点击这里</a>查看详情.", pcomment.Userid, pcomment.Username, config.Albumname, album.Title, pcomment.Photoid, pcomment.Commentid));
                noticeinfo.Type = NoticeType.AlbumCommentNotice;
                noticeinfo.New = 1;
                noticeinfo.Posterid = pcomment.Userid;
                noticeinfo.Poster = pcomment.Username;
                noticeinfo.Postdatetime = Utils.GetDateTime();
                noticeinfo.Uid = album.Userid;
                Notices.CreateNoticeInfo(noticeinfo);
            }
        }
    }
}